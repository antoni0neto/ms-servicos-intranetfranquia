using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_controle_mostruario : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        const string CONTROLE_MOSTRUARIO = "CONTROLE_MOSTRUARIO";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "../mod_atacado/atac_menu.aspx";

                CarregarColecoes();
                CarregarGrupo();
                CarregarFabricante();
                CarregarGriffe();

                Session[CONTROLE_MOSTRUARIO] = null;

                btExcel.Enabled = false;
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }


        private List<SP_OBTER_CONTROLE_MOSTRUARIOResult> ObterControleMostruario(string colecao, int? desenvOrigem, string grupoProduto, string produto, string nome, string fabricante, string griffe)
        {
            var controleMostruarioLista = desenvController.ObterControleMostruario(colecao, desenvOrigem, grupoProduto, produto, nome, fabricante, griffe);

            return controleMostruarioLista;
        }
        private void CarregarControleMostruario()
        {
            int? desenvOrigem = null;
            if (ddlOrigem.SelectedValue != "0")
                desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

            var controleMostruario = ObterControleMostruario(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim().ToUpper(), txtNome.Text.Trim().ToUpper(), ddlFabricante.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim());

            gvControleMostruario.DataSource = controleMostruario;
            gvControleMostruario.DataBind();

            PreencherValorFiltro(controleMostruario);
            CarregarFiltroFabricante(controleMostruario.ToList());

            Session[CONTROLE_MOSTRUARIO] = controleMostruario;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecoes.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                CarregarControleMostruario();

                Session["COLECAO"] = ddlColecoes.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvControleMostruario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CONTROLE_MOSTRUARIOResult _mostruario = e.Row.DataItem as SP_OBTER_CONTROLE_MOSTRUARIOResult;

                    if (_mostruario != null)
                    {
                        TextBox _txtObs = e.Row.FindControl("txtObs") as TextBox;
                        if (_txtObs != null)
                            _txtObs.Text = _mostruario.OBS_MOSTRUARIO;

                        ImageButton _ibtMarcar = e.Row.FindControl("ibtMarcar") as ImageButton;
                        if (_ibtMarcar != null)
                        {
                            _ibtMarcar.Visible = true;
                            if (_mostruario.MOSTRUARIO_OK == "OK")
                                _ibtMarcar.Visible = false;
                            else
                                _ibtMarcar.CommandArgument = _mostruario.CODIGO.ToString();
                        }

                    }
                }
            }
        }
        protected void gvControleMostruario_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (Session[CONTROLE_MOSTRUARIO] != null)
            {
                IEnumerable<SP_OBTER_CONTROLE_MOSTRUARIOResult> controleMostruarioLista = (IEnumerable<SP_OBTER_CONTROLE_MOSTRUARIOResult>)Session[CONTROLE_MOSTRUARIO];

                controleMostruarioLista = Filtrar(controleMostruarioLista);

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvControleMostruario, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvControleMostruario, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvControleMostruario, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                controleMostruarioLista = controleMostruarioLista.OrderBy(e.SortExpression + sortDirection);
                gvControleMostruario.DataSource = controleMostruarioLista;
                gvControleMostruario.DataBind();
            }
        }
        protected void txtObs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        int codigo = Convert.ToInt32(gvControleMostruario.DataKeys[row.RowIndex].Value);
                        var desenvProduto = desenvController.ObterProduto(codigo);
                        if (desenvProduto != null)
                        {
                            desenvProduto.OBS_MOSTRUARIO = txt.Text.Trim().ToUpper();
                            desenvController.AtualizarProduto(desenvProduto);

                            var listaAntiga = (List<SP_OBTER_CONTROLE_MOSTRUARIOResult>)Session[CONTROLE_MOSTRUARIO];
                            var d = listaAntiga.Where(p => p.CODIGO == codigo).FirstOrDefault();
                            listaAntiga.Remove(d);
                            d.OBS_MOSTRUARIO = desenvProduto.OBS_MOSTRUARIO;
                            listaAntiga.Add(d);
                            Session[CONTROLE_MOSTRUARIO] = listaAntiga;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            var controleMostruario = Filtrar((IEnumerable<SP_OBTER_CONTROLE_MOSTRUARIOResult>)Session[CONTROLE_MOSTRUARIO]);

            gvControleMostruario.DataSource = controleMostruario;
            gvControleMostruario.DataBind();

            PreencherValorFiltro(controleMostruario.ToList());
            //CarregarFiltroFabricante(controleMostruario.ToList());
        }

        private IEnumerable<SP_OBTER_CONTROLE_MOSTRUARIOResult> Filtrar(IEnumerable<SP_OBTER_CONTROLE_MOSTRUARIOResult> lista)
        {
            if (lista != null)
            {

                if (ddlFiltroFabricante.SelectedValue != "")
                {
                    lista = lista.Where(p => p.FABRICANTE.Trim() == ddlFiltroFabricante.SelectedValue.Trim());
                }

                if (ddlFiltroHB.SelectedValue != "")
                {
                    if (ddlFiltroHB.SelectedValue == "0")
                        lista = lista.Where(p => p.HB == null);
                    else if (ddlFiltroHB.SelectedValue == "1")
                        lista = lista.Where(p => p.HB > 0);
                    else
                        lista = lista.Where(p => p.HB == 9999);
                }

                if (ddlFiltroFaccao.SelectedValue != "")
                {
                    if (ddlFiltroFaccao.SelectedValue == "0")
                        lista = lista.Where(p => p.FACCAO == "");
                    else
                        lista = lista.Where(p => p.FACCAO != "");
                }

                if (ddlFiltroCusto.SelectedValue != "")
                {
                    if (ddlFiltroCusto.SelectedValue == "0")
                        lista = lista.Where(p => p.CUSTO == "");
                    else
                        lista = lista.Where(p => p.CUSTO != "");
                }

                if (ddlFiltroFaturado.SelectedValue != "")
                {
                    if (ddlFiltroFaturado.SelectedValue == "0")
                        lista = lista.Where(p => p.FATURADO <= 0);
                    else
                        lista = lista.Where(p => p.FATURADO > 0);
                }

                if (ddlFiltroPedido.SelectedValue != "")
                {
                    if (ddlFiltroPedido.SelectedValue == "0")
                        lista = lista.Where(p => p.PEDIDO_COMPRA == "");
                    else
                        lista = lista.Where(p => p.PEDIDO_COMPRA != "");
                }
            }

            return lista;
        }
        private void PreencherValorFiltro(List<SP_OBTER_CONTROLE_MOSTRUARIOResult> lstProdutos)
        {
            try
            {
                int totalModelo = 0;
                int totalEstilo = 0;

                var estilo = lstProdutos.Where(p => p.PRODUTO != null && p.PRODUTO.Trim() != "" && p.PRODUTO.Trim().Length >= 4);
                if (estilo != null)
                    totalEstilo = estilo.Count();

                var modelo = estilo.Select(j => j.PRODUTO).Distinct();
                if (modelo != null)
                    totalModelo = modelo.Count();

                labModeloFiltroValor.Text = totalModelo.ToString();
                labEstiloFiltroValor.Text = totalEstilo.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CarregarFiltroFabricante(List<SP_OBTER_CONTROLE_MOSTRUARIOResult> lstProdutos)
        {
            List<SP_OBTER_CONTROLE_MOSTRUARIOResult> fabFiltro = new List<SP_OBTER_CONTROLE_MOSTRUARIOResult>();

            var _fab = lstProdutos.Select(s => s.FABRICANTE.Trim()).Distinct().ToList();
            foreach (var item in _fab)
                if (item.Trim() != "")
                    fabFiltro.Add(new SP_OBTER_CONTROLE_MOSTRUARIOResult { FABRICANTE = item.Trim() });

            fabFiltro = fabFiltro.OrderBy(p => p.FABRICANTE).ToList();
            fabFiltro.Insert(0, new SP_OBTER_CONTROLE_MOSTRUARIOResult { FABRICANTE = "" });
            ddlFiltroFabricante.DataSource = fabFiltro;
            ddlFiltroFabricante.DataBind();
        }

        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
            }

        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                    ddlColecoes_SelectedIndexChanged(null, null);
                }
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (new ProducaoController().ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        private void CarregarFabricante()
        {
            var fabricante = desenvController.ObterFabricante();

            if (fabricante != null)
            {
                fabricante.Insert(0, new SP_OBTER_FABRICANTEResult { FABRICANTE = "" });
                ddlFabricante.DataSource = fabricante;
                ddlFabricante.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        #endregion

        protected void ibtMarcar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton _ibt = (ImageButton)sender;
            if (_ibt != null)
            {
                int codigoDesenvProduto = Convert.ToInt32(_ibt.CommandArgument);
                var desenvProduto = desenvController.ObterProduto(codigoDesenvProduto);
                if (desenvProduto != null)
                {
                    desenvProduto.USUARIO_MOSTRUARIO_OK = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    desenvController.AtualizarProduto(desenvProduto);

                    GridViewRow row = (GridViewRow)_ibt.NamingContainer;
                    if (row != null)
                        ((ImageButton)row.FindControl("ibtMarcar")).Visible = false;

                }
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[CONTROLE_MOSTRUARIO] != null)
                {
                    var controleMostruario = (Session[CONTROLE_MOSTRUARIO] as List<SP_OBTER_CONTROLE_MOSTRUARIOResult>);
                    if (ddlFiltroFabricante.SelectedValue != "")
                        controleMostruario = controleMostruario.Where(p => p.FABRICANTE.Trim() == ddlFiltroFabricante.SelectedValue.Trim()).ToList();

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "CTROL_MOSTRUARIO_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    //Abaixo codifica os caracteres para o alfabeto latino
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                    Response.Charset = "ISO-8859-1";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvControleMostruario.AllowPaging = false;
                    gvControleMostruario.PageSize = 1000;
                    gvControleMostruario.DataSource = controleMostruario;
                    gvControleMostruario.DataBind();

                    gvControleMostruario.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvControleMostruario.HeaderRow.Cells.Count; i++)
                    {
                        gvControleMostruario.HeaderRow.Cells[i].Attributes.Remove("href");
                        gvControleMostruario.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                        gvControleMostruario.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvControleMostruario.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }
                    gvControleMostruario.RenderControl(htw);
                    Response.Write(sw.ToString().Replace("<a", "<p").Replace("</a>", "</p>"));
                    Response.End();
                }
            }
            catch (Exception ex)
            {
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void cbExcel_CheckedChanged(object sender, EventArgs e)
        {
            if (cbExcel.Checked)
                btExcel.Enabled = true;
            else
                btExcel.Enabled = false;
        }

    }
}
