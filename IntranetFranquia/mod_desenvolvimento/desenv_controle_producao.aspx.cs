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
    public partial class desenv_controle_producao : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        const string CONTROLE_PRODUCAO = "CONTROLE_PRODUCAO";

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

                Session[CONTROLE_PRODUCAO] = null;

                btExcel.Enabled = false;
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }


        private List<SP_OBTER_CONTROLE_PRODUCAOResult> ObterControleProducao(string colecao, int? desenvOrigem, string grupoProduto, string produto, string nome, string fabricante, string griffe, string corFornecedor, string tecido)
        {
            var controleProducaoLista = desenvController.ObterControleProducao(colecao, desenvOrigem, grupoProduto, produto, nome, fabricante, griffe, corFornecedor, tecido);

            if (cbSemCorte.Checked)
                controleProducaoLista = controleProducaoLista.Where(p => p.HB == null || p.HB == 0).ToList();

            return controleProducaoLista;
        }
        private void CarregarControleProducao()
        {
            int? desenvOrigem = null;
            if (ddlOrigem.SelectedValue != "0")
                desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

            var controleProducao = ObterControleProducao(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim().ToUpper(), txtNome.Text.Trim().ToUpper(), ddlFabricante.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim(), ddlCorFornecedor.SelectedValue.Trim(), ddlTecido.SelectedValue.Trim());

            gvControleProducao.DataSource = controleProducao;
            gvControleProducao.DataBind();

            PreencherValorFiltro(controleProducao);
            //CarregarFiltroFabricante(controleMostruario.ToList());

            Session[CONTROLE_PRODUCAO] = controleProducao;
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

                CarregarControleProducao();

                Session["COLECAO"] = ddlColecoes.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvControleProducao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CONTROLE_PRODUCAOResult _producao = e.Row.DataItem as SP_OBTER_CONTROLE_PRODUCAOResult;
                }
            }
        }
        protected void gvControleProducao_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (Session[CONTROLE_PRODUCAO] != null)
            {
                IEnumerable<SP_OBTER_CONTROLE_PRODUCAOResult> controleProducaoLista = (IEnumerable<SP_OBTER_CONTROLE_PRODUCAOResult>)Session[CONTROLE_PRODUCAO];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvControleProducao, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvControleProducao, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvControleProducao, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                controleProducaoLista = controleProducaoLista.OrderBy(e.SortExpression + sortDirection);
                gvControleProducao.DataSource = controleProducaoLista;
                gvControleProducao.DataBind();
            }
        }

        private void PreencherValorFiltro(List<SP_OBTER_CONTROLE_PRODUCAOResult> lstProdutos)
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

        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
                CarregarCorFornecedor(colecao);
                CarregarTecido(colecao);
            }

        }
        private void CarregarColecoes()
        {
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
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
            List<PRODUTOS_GRIFFE> griffe = (baseController.BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarCorFornecedor(string colecao)
        {
            List<DESENV_PRODUTO> corFornecedor = (desenvController.ObterProduto(colecao)).GroupBy(p => new { FORNECEDOR_COR = p.FORNECEDOR_COR })
                .Select(x => new DESENV_PRODUTO { FORNECEDOR_COR = x.Key.FORNECEDOR_COR }).OrderBy(p => p.FORNECEDOR_COR).ToList();

            if (corFornecedor != null)
            {
                corFornecedor.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
                ddlCorFornecedor.DataSource = corFornecedor;
                ddlCorFornecedor.DataBind();
            }
        }
        private void CarregarTecido(string colecao)
        {
            List<DESENV_PRODUTO> tecido = (desenvController.ObterProduto(colecao)).GroupBy(p => new { TECIDO_POCKET = p.TECIDO_POCKET })
                .Select(x => new DESENV_PRODUTO { TECIDO_POCKET = x.Key.TECIDO_POCKET }).OrderBy(p => p.TECIDO_POCKET).ToList();

            if (tecido != null)
            {
                tecido.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
                ddlTecido.DataSource = tecido;
                ddlTecido.DataBind();
            }
        }
        #endregion

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[CONTROLE_PRODUCAO] != null)
                {
                    var controleProducao = (Session[CONTROLE_PRODUCAO] as List<SP_OBTER_CONTROLE_PRODUCAOResult>);

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "CTROL_PRODUCAO_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    //Abaixo codifica os caracteres para o alfabeto latino
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                    Response.Charset = "ISO-8859-1";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvControleProducao.AllowPaging = false;
                    gvControleProducao.PageSize = 1000;
                    gvControleProducao.DataSource = controleProducao;
                    gvControleProducao.DataBind();

                    gvControleProducao.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvControleProducao.HeaderRow.Cells.Count; i++)
                    {
                        gvControleProducao.HeaderRow.Cells[i].Attributes.Remove("href");
                        gvControleProducao.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                        gvControleProducao.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvControleProducao.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }
                    gvControleProducao.RenderControl(htw);
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
