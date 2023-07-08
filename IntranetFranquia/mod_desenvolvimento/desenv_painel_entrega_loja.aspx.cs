using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class desenv_painel_entrega_loja : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        const string PAINEL_ENTREGA_LOJA_PRODUTO = "PAINEL_ENTREGA_LOJA_PRODUTO";

        int gQtdeTotal = 0;
        decimal gValorTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                CarregarColecoesLinx();
                CarregarGriffe();
                CarregarGrupo("");
                CarregarSemanas();


                Session[PAINEL_ENTREGA_LOJA_PRODUTO] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_PAINEL_ENTREGA_LOJAResult> ObterPainelEntregaLoja(bool dropSemana)
        {
            DateTime dataIni = DateTime.Now;
            DateTime dataFim = DateTime.Now;

            if (dropSemana)
            {
                var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));
                dataIni = semanaSel.DATA_INI;
                dataFim = semanaSel.DATA_FIM;
            }
            else
            {
                dataIni = Convert.ToDateTime(txtPeriodoInicial.Text);
                dataFim = Convert.ToDateTime(txtPeriodoFinal.Text);
            }

            var colecaoLinxCon = "";
            if (lstColecao.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstColecao.GetSelectedIndices())
                {
                    var item = lstColecao.Items[v].Value.Trim() + ",";
                    colecaoLinxCon = colecaoLinxCon + item;
                }

                colecaoLinxCon = colecaoLinxCon + ",";
                colecaoLinxCon = colecaoLinxCon.Replace(",,", "");
            }

            var griffeCon = "";
            if (lstGriffe.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGriffe.GetSelectedIndices())
                {
                    var item = lstGriffe.Items[v].Value.Trim() + ",";
                    griffeCon = griffeCon + item;
                }

                griffeCon = griffeCon + ",";
                griffeCon = griffeCon.Replace(",,", "");
            }

            var grupoProdutoCon = "";
            if (lstGrupoProduto.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGrupoProduto.GetSelectedIndices())
                {
                    var item = lstGrupoProduto.Items[v].Value.Trim() + ",";
                    grupoProdutoCon = grupoProdutoCon + item;
                }

                grupoProdutoCon = grupoProdutoCon + ",";
                grupoProdutoCon = grupoProdutoCon.Replace(",,", "");
            }

            var gPainelEntrega = desenvController.ObterPainelEntregaLoja(dataIni, dataFim, colecaoLinxCon, griffeCon, grupoProdutoCon, ddlTipo.SelectedValue, ddlFabricacao.SelectedValue);

            var painelEntrega = new List<SP_OBTER_PAINEL_ENTREGA_LOJAResult>();
            painelEntrega.Add(gPainelEntrega);

            return painelEntrega;
        }
        private List<SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult> ObterPainelEntregaLojaProduto(bool dropSemana)
        {
            DateTime dataIni = DateTime.Now;
            DateTime dataFim = DateTime.Now;

            if (dropSemana)
            {
                var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));
                dataIni = semanaSel.DATA_INI;
                dataFim = semanaSel.DATA_FIM;
            }
            else
            {
                dataIni = Convert.ToDateTime(txtPeriodoInicial.Text);
                dataFim = Convert.ToDateTime(txtPeriodoFinal.Text);
            }

            var colecaoLinxCon = "";
            if (lstColecao.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstColecao.GetSelectedIndices())
                {
                    var item = lstColecao.Items[v].Value.Trim() + ",";
                    colecaoLinxCon = colecaoLinxCon + item;
                }

                colecaoLinxCon = colecaoLinxCon + ",";
                colecaoLinxCon = colecaoLinxCon.Replace(",,", "");
            }

            var griffeCon = "";
            if (lstGriffe.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGriffe.GetSelectedIndices())
                {
                    var item = lstGriffe.Items[v].Value.Trim() + ",";
                    griffeCon = griffeCon + item;
                }

                griffeCon = griffeCon + ",";
                griffeCon = griffeCon.Replace(",,", "");
            }

            var grupoProdutoCon = "";
            if (lstGrupoProduto.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGrupoProduto.GetSelectedIndices())
                {
                    var item = lstGrupoProduto.Items[v].Value.Trim() + ",";
                    grupoProdutoCon = grupoProdutoCon + item;
                }

                grupoProdutoCon = grupoProdutoCon + ",";
                grupoProdutoCon = grupoProdutoCon.Replace(",,", "");
            }

            var gPainelEntregaProduto = desenvController.ObterPainelEntregaLojaProduto(dataIni, dataFim, colecaoLinxCon, griffeCon, grupoProdutoCon, ddlTipo.SelectedValue, ddlFabricacao.SelectedValue);

            return gPainelEntregaProduto;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var dropSemana = true;
                if (ddlSemana.SelectedValue == "0")
                {
                    dropSemana = false;

                    /*VALIDA DATA*/
                    if (txtPeriodoInicial.Text == "")
                    {
                        labErro.Text = "Informe a Data Inicial.";
                        return;
                    }

                    if (txtPeriodoFinal.Text == "")
                    {
                        labErro.Text = "Informe a Data Final.";
                        return;
                    }
                }

                Session[PAINEL_ENTREGA_LOJA_PRODUTO] = null;
                ddlFiltroStatus.SelectedValue = "";

                var entregaLoja = ObterPainelEntregaLoja(dropSemana);

                gvEntregaLoja.DataSource = entregaLoja;
                gvEntregaLoja.DataBind();

                var entregaLojaProduto = ObterPainelEntregaLojaProduto(dropSemana);

                gvEntregaLojaProduto.DataSource = entregaLojaProduto;
                gvEntregaLojaProduto.DataBind();

                Session[PAINEL_ENTREGA_LOJA_PRODUTO] = entregaLojaProduto;

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvEntregaLoja_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PAINEL_ENTREGA_LOJAResult loja = e.Row.DataItem as SP_OBTER_PAINEL_ENTREGA_LOJAResult;

                    Label labQtdeNovo = e.Row.FindControl("labQtdeNovo") as Label;
                    labQtdeNovo.Text = Convert.ToInt32(loja.QTDE_NOVO).ToString("###,###,###,###,##0");

                    Label labValorNovo = e.Row.FindControl("labValorNovo") as Label;
                    labValorNovo.Text = "R$ " + Convert.ToDecimal(loja.VALOR_NOVO).ToString("###,###,###,###,##0.00");

                    Label labQtdeRepo = e.Row.FindControl("labQtdeRepo") as Label;
                    labQtdeRepo.Text = Convert.ToInt32(loja.QTDE_REPO).ToString("###,###,###,###,##0");

                    Label labValorRepo = e.Row.FindControl("labValorRepo") as Label;
                    labValorRepo.Text = "R$ " + Convert.ToDecimal(loja.VALOR_REPO).ToString("###,###,###,###,##0.00");

                    Label labQtdeTotal = e.Row.FindControl("labQtdeTotal") as Label;
                    labQtdeTotal.Text = Convert.ToInt32(loja.QTDE_TOTAL).ToString("###,###,###,###,##0");

                    Label labValorTotal = e.Row.FindControl("labValorTotal") as Label;
                    labValorTotal.Text = "R$ " + Convert.ToDecimal(loja.VALOR_TOTAL).ToString("###,###,###,###,##0.00");
                }
            }
        }
        protected void gvEntregaLoja_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvEntregaLoja.FooterRow;
            if (_footer != null)
            {
            }
        }

        protected void gvEntregaLojaProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult prod = e.Row.DataItem as SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult;

                    ImageButton imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                    if (File.Exists(Server.MapPath(prod.FOTO1)))
                        imgProduto.ImageUrl = prod.FOTO1;
                    else if (File.Exists(Server.MapPath(prod.FOTO2)))
                        imgProduto.ImageUrl = prod.FOTO2;
                    else if (File.Exists(Server.MapPath(prod.FOTO3)))
                        imgProduto.ImageUrl = prod.FOTO3;
                    else
                        imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                    Label labQtdeTotal = e.Row.FindControl("labQtdeTotal") as Label;
                    labQtdeTotal.Text = Convert.ToInt32(prod.QTDE_TOTAL).ToString("###,###,###,##0");

                    Label labValorTotal = e.Row.FindControl("labValorTotal") as Label;
                    labValorTotal.Text = "R$ " + Convert.ToDecimal(prod.VALOR_TOTAL).ToString("###,###,###,##0.00");

                    Label labStatus = e.Row.FindControl("labStatus") as Label;
                    labStatus.Text = ((prod.STATUS == "N") ? "NOVO" : "REPOSIÇÃO");

                    gQtdeTotal += Convert.ToInt32(prod.QTDE_TOTAL);
                    gValorTotal += Convert.ToDecimal(prod.VALOR_TOTAL);
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvEntregaLojaProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvEntregaLojaProduto.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[7].Text = gQtdeTotal.ToString("###,###,##0");
                _footer.Cells[8].Text = "R$ " + gValorTotal.ToString("###,###,###,###,##0.00");
            }

        }
        protected void gvEntregaLojaProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult> prod;

            if (Session[PAINEL_ENTREGA_LOJA_PRODUTO] != null)
                prod = (IEnumerable<SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult>)Session[PAINEL_ENTREGA_LOJA_PRODUTO];
            else
                prod = ObterPainelEntregaLojaProduto(((ddlSemana.SelectedValue != "0") ? true : false));

            if (ddlFiltroStatus.SelectedValue != "")
                prod = prod.Where(p => p.STATUS == ddlFiltroStatus.SelectedValue).ToList();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prod = prod.OrderBy(e.SortExpression + sortDirection);
            gvEntregaLojaProduto.DataSource = prod;
            gvEntregaLojaProduto.DataBind();


        }

        #region "DADOS INICIAIS"
        private void CarregarColecoesLinx()
        {
            var colecoes = baseController.BuscaColecoesLinx();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
            lstColecao.DataSource = colecoes;
            lstColecao.DataBind();

        }
        private void CarregarGrupo(string tipo)
        {
            var grupoProdutos = (prodController.ObterGrupoProduto(tipo));

            grupoProdutos = grupoProdutos.OrderBy(p => p.GRUPO_PRODUTO).ToList();
            grupoProdutos.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
            lstGrupoProduto.DataSource = grupoProdutos;
            lstGrupoProduto.DataBind();

        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                lstGriffe.DataSource = griffe;
                lstGriffe.DataBind();
            }
        }
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarGrupo(ddlTipo.SelectedValue);
        }
        private void CarregarSemanas()
        {
            var semana = lojaController.ObterSemanaVenda();

            semana.RemoveAt(0);
            semana.Insert(0, new LOJA_VENDA_SEMANA { CODIGO = 0, SEMANA = "" });

            ddlSemana.DataSource = semana;
            ddlSemana.DataBind();
        }
        protected void ddlSemana_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSemana.SelectedValue != "0")
            {
                txtPeriodoInicial.Text = "";
                txtPeriodoInicial.Enabled = false;

                txtPeriodoFinal.Text = "";
                txtPeriodoFinal.Enabled = false;
            }
            else
            {
                txtPeriodoInicial.Enabled = true;
                txtPeriodoFinal.Enabled = true;
            }
        }

        #endregion

        protected void ddlFiltroStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult> prod;

                if (Session[PAINEL_ENTREGA_LOJA_PRODUTO] != null)
                    prod = (IEnumerable<SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult>)Session[PAINEL_ENTREGA_LOJA_PRODUTO];
                else
                    prod = ObterPainelEntregaLojaProduto(((ddlSemana.SelectedValue != "0") ? true : false));

                if (ddlFiltroStatus.SelectedValue != "")
                    prod = prod.Where(p => p.STATUS == ddlFiltroStatus.SelectedValue).ToList();

                gvEntregaLojaProduto.DataSource = prod;
                gvEntregaLojaProduto.DataBind();

            }
            catch (Exception)
            {
            }
        }
    }
}
