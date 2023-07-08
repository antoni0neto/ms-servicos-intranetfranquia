using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;

namespace Relatorios
{
    public partial class fin_confere_venda_mensal_tickets : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        decimal qtdePeca = 0;
        decimal vendaBruta = 0;
        decimal valorPago = 0;
        decimal desconto = 0;
        decimal valorTroca = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int ano = 0;
                int mes = 0;
                string cpf = "";

                if (Request.QueryString["a"] == null || Request.QueryString["a"] == "" ||
                    Request.QueryString["m"] == null || Request.QueryString["m"] == "" ||
                    Request.QueryString["cpf"] == null || Request.QueryString["cpf"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("facc_menu.aspx");

                ano = Convert.ToInt32(Request.QueryString["a"].ToString());
                mes = Convert.ToInt32(Request.QueryString["m"].ToString());
                cpf = Request.QueryString["cpf"].ToString();

                hidAno.Value = ano.ToString();
                hidMes.Value = mes.ToString();
                hidCPF.Value = cpf.ToString();

                CarregarTickets(ano, mes, cpf);
            }

        }

        #region "DADOS INICIAIS"
        #endregion

        #region "TICKETS"
        private List<SP_OBTER_TICKET_POR_CLIENTEResult> ObterTicketPorCliente(int ano, int mes, string cpf)
        {
            return lojaController.ObterTicketPorCliente(ano, mes, cpf);
        }
        private void CarregarTickets(int ano, int mes, string cpf)
        {
            var tkt = ObterTicketPorCliente(ano, mes, cpf);

            if (tkt != null && tkt.Count() > 0)
            {
                txtCPF.Text = tkt[0].CPF;
                txtCliente.Text = tkt[0].CLIENTE_VAREJO.ToUpper();
            }

            gvTickets.DataSource = tkt;
            gvTickets.DataBind();
        }
        protected void gvTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_TICKET_POR_CLIENTEResult tkt = e.Row.DataItem as SP_OBTER_TICKET_POR_CLIENTEResult;

                    if (tkt != null)
                    {
                        Literal litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                        litDataVenda.Text = Convert.ToDateTime(tkt.DATA_DIGITACAO).ToString("dd/MM/yyyy HH:mm:ss");

                        Literal litValVendaBruta = e.Row.FindControl("litValVendaBruta") as Literal;
                        litValVendaBruta.Text = "R$" + Convert.ToDecimal(tkt.VALOR_VENDA_BRUTA).ToString("###,###,###,##0.00");

                        Literal litValTroca = e.Row.FindControl("litValTroca") as Literal;
                        litValTroca.Text = "R$" + Convert.ToDecimal(tkt.VALOR_TROCA).ToString("###,###,###,##0.00");

                        Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                        litValorPago.Text = "R$" + Convert.ToDecimal(tkt.VALOR_PAGO).ToString("###,###,###,##0.00");

                        Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                        litDesconto.Text = "R$" + Convert.ToDecimal(tkt.DESCONTO).ToString("###,###,###,##0.00");

                        if (tkt.DATA_HORA_CANCELAMENTO != null)
                        {
                            e.Row.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            qtdePeca += Convert.ToInt32(tkt.QTDE);
                            vendaBruta += Convert.ToDecimal(tkt.VALOR_VENDA_BRUTA);
                            valorTroca += Convert.ToDecimal(tkt.VALOR_TROCA);
                            valorPago += Convert.ToDecimal(tkt.VALOR_PAGO);
                            desconto += Convert.ToDecimal(tkt.DESCONTO);
                        }

                        var histVenda = lojaController.ObterVendaCanceladaHist(tkt.CODIGO_FILIAL, tkt.TICKET, tkt.CODIGO_CLIENTE);
                        if (histVenda != null && histVenda.Count() > 0)
                        {
                            GridView gvHistCancelamento = e.Row.FindControl("gvHistCancelamento") as GridView;
                            gvHistCancelamento.DataSource = histVenda;
                            gvHistCancelamento.DataBind();
                        }

                    }
                }
            }
        }
        protected void gvTickets_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTickets.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[6].Text = qtdePeca.ToString();
                footer.Cells[7].Text = "R$" + vendaBruta.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = "R$" + valorTroca.ToString("###,###,###,##0.00");
                footer.Cells[9].Text = "R$" + valorPago.ToString("###,###,###,##0.00");
                footer.Cells[10].Text = "R$" + desconto.ToString("###,###,###,##0.00");
            }
        }
        protected void gvTickets_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_TICKET_POR_CLIENTEResult> tkt = ObterTicketPorCliente(Convert.ToInt32(hidAno.Value), Convert.ToInt32(hidMes.Value), hidCPF.Value);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            tkt = tkt.OrderBy(e.SortExpression + sortDirection);
            gvTickets.DataSource = tkt;
            gvTickets.DataBind();
        }
        #endregion

        protected void gvHistCancelamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HIST_VENDA_CANCELADAResult tkt = e.Row.DataItem as SP_OBTER_HIST_VENDA_CANCELADAResult;

                    if (tkt != null)
                    {

                        Literal litDataEvento = e.Row.FindControl("litDataEvento") as Literal;
                        litDataEvento.Text = Convert.ToDateTime(tkt.DATA_EVENTO).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                }
            }
        }
        protected void gvHistCancelamento_DataBound(object sender, EventArgs e)
        {
        }




    }
}
