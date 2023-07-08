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
    public partial class rh_vale_mercadoria_mensal : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        int qtdePeca = 0;
        decimal vendaBruta = 0;
        decimal valorPago = 0;
        decimal desconto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "rh_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";

                CarregarFilial();
                CarregarDataAno();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        private List<SP_VALE_MERCADORIAResult> ObterValeMercadoria()
        {

            return lojaController.ObterValeMercadoria(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), ddlFilial.SelectedValue);
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlMes.SelectedValue == "")
                {
                    labErro.Text = "Informe o Mês.";
                    return;
                }


                gvVendedores.DataSource = ObterValeMercadoria();
                gvVendedores.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvVendedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_VALE_MERCADORIAResult mer = e.Row.DataItem as SP_VALE_MERCADORIAResult;

                    Literal litQtdePeca = e.Row.FindControl("litQtdePeca") as Literal;
                    litQtdePeca.Text = mer.QTDE.ToString();

                    Literal litVendaBruta = e.Row.FindControl("litVendaBruta") as Literal;
                    litVendaBruta.Text = "R$ " + Convert.ToDecimal(mer.VALOR_VENDA_BRUTA).ToString("###,###,###,##0.00");

                    Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                    litValorPago.Text = "R$ " + Convert.ToDecimal(mer.VALOR_PAGO).ToString("###,###,###,##0.00");

                    Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                    litDesconto.Text = "R$ " + Convert.ToDecimal(mer.DESCONTO).ToString("###,###,###,##0.00");

                    qtdePeca += Convert.ToInt32(mer.QTDE);
                    vendaBruta += Convert.ToDecimal(mer.VALOR_VENDA_BRUTA);
                    valorPago += Convert.ToDecimal(mer.VALOR_PAGO);
                    desconto += Convert.ToDecimal(mer.DESCONTO);

                }
            }
        }
        protected void gvVendedores_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVendedores.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[5].Text = qtdePeca.ToString();
                footer.Cells[5].HorizontalAlign = HorizontalAlign.Center;

                footer.Cells[6].Text = "R$ " + vendaBruta.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = "R$ " + valorPago.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = "R$ " + desconto.ToString("###,###,###,##0.00");
            }
        }
        protected void gvVendedores_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_VALE_MERCADORIAResult> vale = ObterValeMercadoria();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            vale = vale.OrderBy(e.SortExpression + sortDirection);
            gvVendedores.DataSource = vale;
            gvVendedores.DataBind();


        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();
                //lstFilial = baseController.BuscaFiliais();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                    }
                }
            }
        }
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        #endregion




    }
}
