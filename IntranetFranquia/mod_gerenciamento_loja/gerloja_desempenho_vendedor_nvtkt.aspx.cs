using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;

namespace Relatorios
{
    public partial class gerloja_desempenho_vendedor_nvtkt : System.Web.UI.Page
    {
        DesempenhoController desempenhoController = new DesempenhoController();

        decimal gValPago = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {


                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                var vendedor = Request.QueryString["p"].ToString();

                hidVendedor.Value = vendedor;

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (txtDataInicial.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Data Inicial.";
                    return;
                }
                if (txtDataFinal.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Data Final.";
                    return;
                }

                CarregarTicketsVendedor();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private List<SP_OBTER_VENDA_CLIENTENV_TICKETResult> ObterTickets()
        {
            var tks = desempenhoController.ObterDesempenhoVendedorNVTicket(hidVendedor.Value, Convert.ToDateTime(txtDataInicial.Text.Trim()), Convert.ToDateTime(txtDataFinal.Text.Trim()));

            return tks;
        }
        private void CarregarTicketsVendedor()
        {
            var tickets = ObterTickets();

            gvTicket.DataSource = tickets;
            gvTicket.DataBind();
        }

        protected void gvTicket_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_CLIENTENV_TICKETResult tkt = e.Row.DataItem as SP_OBTER_VENDA_CLIENTENV_TICKETResult;

                    Literal litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                    litDataVenda.Text = tkt.DATA_VENDA.ToString("dd/MM/yyyy");

                    Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                    litValorPago.Text = "R$ " + Convert.ToDecimal(tkt.VALOR_PAGO).ToString("###,###,###,##0.00");

                    gValPago += Convert.ToDecimal(tkt.VALOR_PAGO);

                }
            }
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvTicket_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTicket.FooterRow;
            if (footer != null)
            {

                footer.Cells[6].Text = "R$ " + gValPago.ToString("###,###,###,###,##0.00");
            }
        }
        protected void gvTicket_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_VENDA_CLIENTENV_TICKETResult> nv = ObterTickets();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvTicket, e, out sort);

            if (sort == SortDirection.Ascending)
            {
                Utils.WebControls.GetBoundFieldIndexByName(gvTicket, e.SortExpression, " - >>");
            }
            else
            {
                Utils.WebControls.GetBoundFieldIndexByName(gvTicket, e.SortExpression, " - <<");
            }

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            nv = nv.OrderBy(e.SortExpression + sortDirection);
            gvTicket.DataSource = nv;
            gvTicket.DataBind();
        }
    }
}
