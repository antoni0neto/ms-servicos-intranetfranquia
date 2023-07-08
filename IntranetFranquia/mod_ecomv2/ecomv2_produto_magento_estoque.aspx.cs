using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Linq.Dynamic;
using Relatorios.mod_ecom.mag;
using Relatorios.mod_ecomv2.mag2;

namespace Relatorios
{
    public partial class ecomv2_produto_magento_estoque : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        const string ESTOQUE_ECOM_MAG = "ESTOQUE_ECOM_MAG_1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarCategoriaMag();

                CarregarJQuery();

                Session[ESTOQUE_ECOM_MAG] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();

        }
        private void CarregarGrupo()
        {
            var grupos = prodController.ObterGrupoProduto("");

            grupos.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
            ddlGrupo.DataSource = grupos;
            ddlGrupo.DataBind();

        }
        private void CarregarGriffe()
        {
            var griffes = baseController.BuscaGriffes();

            griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
            ddlGriffe.DataSource = griffes;
            ddlGriffe.DataBind();

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

        private List<SP_OBTER_ESTOQUE_TEMPV2Result> ObterProduto(int codigoUsuario)
        {
            //var produtos = eController.ObterMagentoProdutoSimplesCadastrado().AsQueryable();

            //if (ddlColecao.SelectedValue != "")
            //    produtos = produtos.Where(p => p.COLECAO == ddlColecao.SelectedValue.Trim()).AsQueryable();

            //if (ddlGrupo.SelectedValue != "")
            //    produtos = produtos.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).AsQueryable();

            //if (txtProduto.Text != "")
            //    produtos = produtos.Where(p => p.PRODUTO == txtProduto.Text.Trim()).AsQueryable();

            //if (ddlGriffe.SelectedValue != "")
            //    produtos = produtos.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).AsQueryable();

            //if (ddlCategoriaMag.SelectedValue != "" && ddlCategoriaMag.SelectedValue != "0")
            //    produtos = produtos.Where(p => p.ECOM_GRUPO_PRODUTO == Convert.ToInt32(ddlCategoriaMag.SelectedValue)).AsQueryable();

            var usuario = baseController.BuscaUsuario(codigoUsuario);
            var mag = new MagentoV2();
            mag.ObterESalvarEstoque(codigoUsuario);

            var estoqueMagento = eController.ObterEstoqueMagentoTempV2(codigoUsuario);

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

                Session[ESTOQUE_ECOM_MAG] = null;
                var estoqueMag = ObterProduto(codigoUsuario);

                gvProduto.DataSource = estoqueMag;
                gvProduto.DataBind();

                btExcel.Enabled = false;
                if (estoqueMag != null && estoqueMag.Count() > 0)
                {
                    Session[ESTOQUE_ECOM_MAG] = estoqueMag;
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
                    SP_OBTER_ESTOQUE_TEMPV2Result prod = e.Row.DataItem as SP_OBTER_ESTOQUE_TEMPV2Result;

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
            if (Session[ESTOQUE_ECOM_MAG] != null)
            {
                IEnumerable<SP_OBTER_ESTOQUE_TEMPV2Result> produtos = (IEnumerable<SP_OBTER_ESTOQUE_TEMPV2Result>)Session[ESTOQUE_ECOM_MAG];

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
                if (Session[ESTOQUE_ECOM_MAG] != null)
                {
                    pnlExcel.Visible = true;

                    gvExcel.DataSource = (Session[ESTOQUE_ECOM_MAG] as List<SP_OBTER_ESTOQUE_TEMPV2Result>);
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
                    gvExcel.DataSource = (Session[ESTOQUE_ECOM_MAG] as List<SP_OBTER_ESTOQUE_TEMPV2Result>);
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
                if (Session["USUARIO"] == null)
                    Response.Redirect("");

                var mag = new MagentoV2();

                string idProdutoMag = bt.CommandArgument;
                var ecomProduto = eController.ObterMagentoProdutoId(Convert.ToInt32(idProdutoMag));

                if (bt.CommandName == "1") // habilitar
                {
                    //habilitar produto do magento
                    ecomProduto.VISIBILIDADE = "4";
                    ecomProduto.DATA_ALTERACAO = DateTime.Now;
                    bt.Text = "Desabilitar";
                    bt.CommandName = "4";
                    bt.BackColor = Color.PaleGreen;
                    bt.ForeColor = Color.Black;
                }
                else
                {
                    ecomProduto.VISIBILIDADE = "1";
                    ecomProduto.DATA_ALTERACAO = DateTime.Now;
                    bt.Text = "Habilitar";
                    bt.BackColor = Color.Firebrick;
                    bt.ForeColor = Color.White;
                    bt.CommandName = "1";
                }

                mag.HabilitarProdutoConfiguravel(ecomProduto);
                eController.AtualizarMagentoProduto(ecomProduto);

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }
}

