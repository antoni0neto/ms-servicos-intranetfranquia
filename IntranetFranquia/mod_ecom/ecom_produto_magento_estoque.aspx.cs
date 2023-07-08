using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Collections;
using Relatorios.mod_ecom.mag;


namespace Relatorios
{
    public partial class ecom_produto_magento_estoque : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarCategoriaMag();

                CarregarJQuery();

                Session["ESTOQUE_ECOM_MAG"] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarCategoriaMag()
        {
            var y = eController.ObterMagentoGrupoProdutoBloco();

            y.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "" });
            ddlCategoriaMag.DataSource = y.OrderBy(p => p.GRUPO);
            ddlCategoriaMag.DataBind();
        }


        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        private List<SP_OBTER_ESTOQUE_TEMPResult> ObterProduto(int codigoUsuario)
        {
            var produtos = eController.ObterMagentoProdutoSimplesCadastrado();

            if (ddlColecao.SelectedValue != "")
                produtos = produtos.Where(p => p.COLECAO == ddlColecao.SelectedValue.Trim()).ToList();

            if (ddlGrupo.SelectedValue != "")
                produtos = produtos.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).ToList();

            if (txtProduto.Text != "")
                produtos = produtos.Where(p => p.PRODUTO == txtProduto.Text.Trim()).ToList();

            if (ddlGriffe.SelectedValue != "")
                produtos = produtos.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlCategoriaMag.SelectedValue != "" && ddlCategoriaMag.SelectedValue != "0")
                produtos = produtos.Where(p => p.ECOM_GRUPO_PRODUTO == Convert.ToInt32(ddlCategoriaMag.SelectedValue)).ToList();

            var idsMagento = new List<string>();

            foreach (var prod in produtos)
            {
                idsMagento.Add(prod.ID_PRODUTO_MAG.ToString());
            }

            var usuario = baseController.BuscaUsuario(codigoUsuario);

            Magento mag = new Magento(usuario.USER_MAG_API, usuario.PASS_MAG_API);
            mag.ObterEstoqueMagento(idsMagento, codigoUsuario);

            var estoqueMagento = eController.ObterEstoqueMagentoTemp(codigoUsuario);

            if (ddlHabilitado.SelectedValue != "")
            {
                if (ddlHabilitado.SelectedValue == "S")
                    estoqueMagento = estoqueMagento.Where(p => p.VISIBILIDADE == "4").ToList();
                else
                    estoqueMagento = estoqueMagento.Where(p => p.VISIBILIDADE == "1").ToList();
            }


            if (ddlEstoqueLinx.SelectedValue != "")
            {
                if (ddlEstoqueLinx.SelectedValue == "S")
                    estoqueMagento = estoqueMagento.Where(p => p.QTDE_LINX > 0).ToList();
                else
                    estoqueMagento = estoqueMagento.Where(p => p.QTDE_LINX <= 0).ToList();
            }

            if (ddlEstoqueMag.SelectedValue != "")
            {
                if (ddlEstoqueMag.SelectedValue == "S")
                    estoqueMagento = estoqueMagento.Where(p => p.QTDE_MAG > 0).ToList();
                else
                    estoqueMagento = estoqueMagento.Where(p => p.QTDE_MAG <= 0).ToList();
            }

            if (ddlEstoqueDiff.SelectedValue != "")
            {
                if (ddlEstoqueDiff.SelectedValue == "S")
                    estoqueMagento = estoqueMagento.Where(p => (p.QTDE_RESERVADA + p.QTDE_MAG) != p.QTDE_LINX).ToList();
                else
                    estoqueMagento = estoqueMagento.Where(p => (p.QTDE_RESERVADA + p.QTDE_MAG) == p.QTDE_LINX).ToList();
            }

            return estoqueMagento;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                Session["ESTOQUE_ECOM_MAG"] = null;

                //if (ddlColecao.SelectedValue == "" && ddlGrupo.SelectedValue == "")
                //{
                //    labErro.Text = "Por favor, selecione a Coleção e/ou o Grupo do Produto.";
                //    return;
                //}

                var estoqueMag = ObterProduto(codigoUsuario);

                gvProduto.DataSource = estoqueMag;
                gvProduto.DataBind();

                btExcel.Enabled = false;
                if (estoqueMag != null && estoqueMag.Count() > 0)
                {
                    Session["ESTOQUE_ECOM_MAG"] = estoqueMag;
                    btExcel.Enabled = true;
                }

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ESTOQUE_TEMPResult prod = e.Row.DataItem as SP_OBTER_ESTOQUE_TEMPResult;

                    if ((prod.QTDE_RESERVADA + prod.QTDE_MAG) != prod.QTDE_LINX)
                        e.Row.BackColor = Color.LightCoral;

                    Button btDesabilitarProduto = e.Row.FindControl("btDesabilitarProduto") as Button;
                    if (btDesabilitarProduto != null)
                    {
                        btDesabilitarProduto.CommandArgument = prod.ID_PRODUTO_MAG.ToString();

                        if (prod.VISIBILIDADE == "1")
                        {
                            btDesabilitarProduto.BackColor = Color.Firebrick;
                            btDesabilitarProduto.ForeColor = Color.White;
                            btDesabilitarProduto.Text = "Habilitar";
                            btDesabilitarProduto.CommandName = "1";
                        }
                        else
                        {
                            btDesabilitarProduto.BackColor = Color.PaleGreen;
                            btDesabilitarProduto.ForeColor = Color.Black;
                            btDesabilitarProduto.Text = "Desabilitar";
                            btDesabilitarProduto.CommandName = "4";
                        }
                    }
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["ESTOQUE_ECOM_MAG"] != null)
            {
                IEnumerable<SP_OBTER_ESTOQUE_TEMPResult> produtos = (IEnumerable<SP_OBTER_ESTOQUE_TEMPResult>)Session["ESTOQUE_ECOM_MAG"];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                produtos = produtos.OrderBy(e.SortExpression + sortDirection);
                gvProduto.DataSource = produtos;
                gvProduto.DataBind();
            }

            CarregarJQuery();
        }

        #endregion

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ESTOQUE_ECOM_MAG"] != null)
                {
                    pnlExcel.Visible = true;

                    gvExcel.DataSource = (Session["ESTOQUE_ECOM_MAG"] as List<SP_OBTER_ESTOQUE_TEMPResult>);
                    gvExcel.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ESTOQUE_ECOM_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                    Response.Charset = "ISO-8859-1";

                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvExcel.AllowPaging = false;
                    gvExcel.PageSize = 1000;
                    gvExcel.DataSource = (Session["ESTOQUE_ECOM_MAG"] as List<SP_OBTER_ESTOQUE_TEMPResult>);
                    gvExcel.DataBind();


                    gvExcel.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvExcel.HeaderRow.Cells.Count; i++)
                    {
                        gvExcel.HeaderRow.Cells[i].Attributes.Remove("href");
                        gvExcel.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                        gvExcel.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvExcel.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }

                    for (int x = 0; x < gvExcel.Rows.Count; x++)
                    {
                        gvExcel.Rows[x].Cells[6].Style.Add("mso-number-format", "\\@");
                    }

                    gvExcel.RenderControl(htw);
                    Response.Write(sw.ToString().Replace("<a", "<p").Replace("</a>", "</p>"));
                    Response.End();

                    pnlExcel.Visible = false;
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

        protected void btDesabilitarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;

                string idProdutoMag = bt.CommandArgument;

                if (Session["USUARIO"] == null)
                    Response.Redirect("");

                USUARIO usuario = ((USUARIO)Session["USUARIO"]);
                Magento mag = new Magento();

                var ecomProduto = eController.ObterMagentoProdutoId(Convert.ToInt32(idProdutoMag));

                if (bt.CommandName == "1") // habilitar
                {
                    //habilitar produto do magento

                    ecomProduto.VISIBILIDADE = "4";
                    ecomProduto.DATA_ALTERACAO = DateTime.Now;
                    ecomProduto.USUARIO_ALTERACAO = usuario.CODIGO_USUARIO;
                    bt.Text = "Desabilitar";
                    bt.CommandName = "4";
                    bt.BackColor = Color.PaleGreen;
                    bt.ForeColor = Color.Black;
                }
                else
                {
                    ecomProduto.VISIBILIDADE = "1";
                    ecomProduto.DATA_ALTERACAO = DateTime.Now;
                    ecomProduto.USUARIO_ALTERACAO = usuario.CODIGO_USUARIO;
                    bt.Text = "Habilitar";
                    bt.BackColor = Color.Firebrick;
                    bt.ForeColor = Color.White;
                    bt.CommandName = "1";
                }
                eController.AtualizarMagentoProduto(ecomProduto);

                mag.AtualizarProdutoConfiguravel(ecomProduto);

                CarregarJQuery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }
}

