using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_venda_compara_col_outlet : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        int gQtdeOutlet = 0;
        int gQtdeColecao = 0;
        int gQtdeTotal = 0;

        decimal gValorOutlet = 0;
        decimal gValorColecao = 0;
        decimal gValorTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

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
                    hrefVoltar.HRef = "ecom_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "ecom_menu_externo.aspx";

                CarregarGrupo();
                CarregarGriffe();

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_ECOM_VENDACOMP_COLOUTLETResult> ObterVendaComparativoColecaoOutlet(DateTime dataIni, DateTime dataFim, string griffe, string grupoProduto)
        {
            return ecomController.ObterVendaComparativoColecaoOutlet(dataIni, dataFim, griffe, grupoProduto);
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                DateTime dataIni;
                DateTime dataFim;

                if (txtDataIni.Text == "" || !DateTime.TryParse(txtDataIni.Text, out dataIni))
                {
                    labErro.Text = "Informe uma data válida para o Período Inicial";
                    return;
                }

                if (txtDataFim.Text == "" || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                {
                    labErro.Text = "Informe uma data válida para o Período Fim";
                    return;
                }

                var produtos = ObterVendaComparativoColecaoOutlet(dataIni, dataFim, ddlGriffe.SelectedValue.Trim(), ddlGrupo.SelectedValue.Trim());

                produtos = produtos.GroupBy(p => new { DATA_INI = p.DATA_INI, DATA_FIM = p.DATA_FIM, GRIFFE = p.GRIFFE }).Select(x => new SP_OBTER_ECOM_VENDACOMP_COLOUTLETResult
                {
                    DATA_INI = x.Key.DATA_INI,
                    DATA_FIM = x.Key.DATA_FIM,
                    GRIFFE = x.Key.GRIFFE,
                    QTDE_OUTLET = x.Sum(p => p.QTDE_OUTLET),
                    QTDE_COLECAO = x.Sum(p => p.QTDE_COLECAO),
                    VALOR_OUTLET = x.Sum(p => p.VALOR_OUTLET),
                    VALOR_COLECAO = x.Sum(p => p.VALOR_COLECAO),

                    QTDE_TOTAL = x.Sum(p => p.QTDE_TOTAL),
                    VALOR_TOTAL = x.Sum(p => p.VALOR_TOTAL),

                    PORC_OUTLET = (x.Sum(p => p.VALOR_OUTLET) / x.Sum(p => p.VALOR_TOTAL) * 100.00M),
                    PORC_COLECAO = (x.Sum(p => p.VALOR_COLECAO) / x.Sum(p => p.VALOR_TOTAL) * 100.00M)
                }).ToList();


                gvProduto.DataSource = produtos;
                gvProduto.DataBind();

                if (produtos.Count <= 0)
                {
                    labErro.Text = "Nenhum registro encontrado.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_VENDACOMP_COLOUTLETResult venda = e.Row.DataItem as SP_OBTER_ECOM_VENDACOMP_COLOUTLETResult;

                    Literal litQtdeOutlet = e.Row.FindControl("litQtdeOutlet") as Literal;
                    litQtdeOutlet.Text = venda.QTDE_OUTLET.ToString();
                    Literal litValorOutlet = e.Row.FindControl("litValorOutlet") as Literal;
                    litValorOutlet.Text = "R$ " + Convert.ToDecimal(venda.VALOR_OUTLET).ToString("###,###,###0.00");

                    Literal litQtdeColecao = e.Row.FindControl("litQtdeColecao") as Literal;
                    litQtdeColecao.Text = venda.QTDE_COLECAO.ToString();
                    Literal litValorColecao = e.Row.FindControl("litValorColecao") as Literal;
                    litValorColecao.Text = "R$ " + Convert.ToDecimal(venda.VALOR_COLECAO).ToString("###,###,###0.00");

                    Literal litQtdeTotal = e.Row.FindControl("litQtdeTotal") as Literal;
                    litQtdeTotal.Text = venda.QTDE_TOTAL.ToString();
                    Literal litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                    litValorTotal.Text = "R$ " + Convert.ToDecimal(venda.VALOR_TOTAL).ToString("###,###,###0.00");

                    Literal litPorcOutlet = e.Row.FindControl("litPorcOutlet") as Literal;
                    litPorcOutlet.Text = Convert.ToDecimal(venda.PORC_OUTLET).ToString("##0.00") + "%";
                    Literal litPorcColecao = e.Row.FindControl("litPorcColecao") as Literal;
                    litPorcColecao.Text = Convert.ToDecimal(venda.PORC_COLECAO).ToString("##0.00") + "%";

                    gQtdeOutlet += Convert.ToInt32(venda.QTDE_OUTLET);
                    gQtdeColecao += Convert.ToInt32(venda.QTDE_COLECAO);
                    gQtdeTotal += Convert.ToInt32(venda.QTDE_TOTAL);

                    gValorOutlet += Convert.ToDecimal(venda.VALOR_OUTLET);
                    gValorColecao += Convert.ToDecimal(venda.VALOR_COLECAO);
                    gValorTotal += Convert.ToDecimal(venda.VALOR_TOTAL);

                    //Grupos
                    var vendaPorGrupo = ObterVendaComparativoColecaoOutlet(Convert.ToDateTime(venda.DATA_INI), Convert.ToDateTime(venda.DATA_FIM), venda.GRIFFE, ddlGrupo.SelectedValue.Trim());
                    if (vendaPorGrupo != null && vendaPorGrupo.Count() > 0)
                    {
                        GridView gvProdutoGrupo = e.Row.FindControl("gvProdutoGrupo") as GridView;
                        gvProdutoGrupo.DataSource = vendaPorGrupo.OrderByDescending(p => p.VALOR_TOTAL);
                        gvProdutoGrupo.DataBind();
                    }
                    else
                    {
                        System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                        if (img != null)
                            img.Visible = false;
                    }

                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProduto.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[2].Text = "Total";
                _footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[3].Text = gQtdeOutlet.ToString();
                _footer.Cells[4].Text = "R$ " + Convert.ToDecimal(gValorOutlet).ToString("###,###,###0.00");

                _footer.Cells[5].Text = gQtdeColecao.ToString();
                _footer.Cells[6].Text = "R$ " + Convert.ToDecimal(gValorColecao).ToString("###,###,###0.00");

                _footer.Cells[7].Text = gQtdeTotal.ToString();
                _footer.Cells[8].Text = "R$ " + Convert.ToDecimal(gValorTotal).ToString("###,###,###0.00");

                _footer.Cells[9].Text = (gValorOutlet / gValorTotal * 100.00M).ToString("##0.00") + "%";
                _footer.Cells[10].Text = (gValorColecao / gValorTotal * 100.00M).ToString("##0.00") + "%";

            }
        }

        protected void gvProdutoGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_VENDACOMP_COLOUTLETResult venda = e.Row.DataItem as SP_OBTER_ECOM_VENDACOMP_COLOUTLETResult;

                    Literal litQtdeOutlet = e.Row.FindControl("litQtdeOutlet") as Literal;
                    litQtdeOutlet.Text = venda.QTDE_OUTLET.ToString();
                    Literal litValorOutlet = e.Row.FindControl("litValorOutlet") as Literal;
                    litValorOutlet.Text = "R$ " + Convert.ToDecimal(venda.VALOR_OUTLET).ToString("###,###,###0.00");

                    Literal litQtdeColecao = e.Row.FindControl("litQtdeColecao") as Literal;
                    litQtdeColecao.Text = venda.QTDE_COLECAO.ToString();
                    Literal litValorColecao = e.Row.FindControl("litValorColecao") as Literal;
                    litValorColecao.Text = "R$ " + Convert.ToDecimal(venda.VALOR_COLECAO).ToString("###,###,###0.00");

                    Literal litQtdeTotal = e.Row.FindControl("litQtdeTotal") as Literal;
                    litQtdeTotal.Text = venda.QTDE_TOTAL.ToString();
                    Literal litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                    litValorTotal.Text = "R$ " + Convert.ToDecimal(venda.VALOR_TOTAL).ToString("###,###,###0.00");

                    Literal litPorcOutlet = e.Row.FindControl("litPorcOutlet") as Literal;
                    litPorcOutlet.Text = Convert.ToDecimal(venda.PORC_OUTLET).ToString("##0.00") + "%";
                    Literal litPorcColecao = e.Row.FindControl("litPorcColecao") as Literal;
                    litPorcColecao.Text = Convert.ToDecimal(venda.PORC_COLECAO).ToString("##0.00") + "%";
                }
            }
        }
        protected void gvProdutoGrupo_DataBound(object sender, EventArgs e)
        {

        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            var _grupo = (new ProducaoController().ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        #endregion

    }
}
