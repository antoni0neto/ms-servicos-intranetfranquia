using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.Drawing;

namespace Relatorios
{
    public partial class DesempenhoAgoraRedev2 : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        public int totalVendas = 0;
        public int totalCotas = 0;
        public double totalPercAtingido = 0;
        public int totalTickets = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarAgoraRede();
            }
        }

        private void CarregarAgoraRede()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                {
                    if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 3)
                    {
                        var lstAtingido = new List<AtingidoAgora>();

                        var agoraRede = baseController.ObterAgoraRede("", null);
                        foreach (var ar in agoraRede)
                        {
                            lstAtingido.Add(
                                    new AtingidoAgora
                                    {
                                        Filial = ar.FILIAL,
                                        Vendas = Convert.ToInt32(ar.VENDAS),
                                        Cota = Convert.ToInt32(ar.COTAS),
                                        Atingido = ar.ATINGIDO.ToString("N2") + "%",
                                        Tickets = ar.TICKETS,
                                        TicketMedio = ar.TICKET_MEDIO,
                                        Pa = ar.PA.ToString("N2"),
                                        Hora = ((ar.DATA_ULTIMA_VENDA == null) ? "-" : Convert.ToDateTime(ar.DATA_ULTIMA_VENDA).ToString("HH:mm"))
                                    });
                        }

                        var ecom = baseController.ObterEcomAgoraRede();
                        if (ecom != null)
                            lstAtingido.Add(
                                    new AtingidoAgora
                                    {
                                        Filial = ecom.FILIAL,
                                        Vendas = Convert.ToInt32(ecom.VALOR_VENDA),
                                        Cota = Convert.ToInt32(ecom.COTAS),
                                        Atingido = ecom.ATINGIDO.ToString("N2") + "%",
                                        Tickets = Convert.ToInt32(ecom.TOT_PEDIDO),
                                        TicketMedio = Convert.ToDecimal(ecom.TICKET_MEDIO),
                                        Hora = ecom.ULTIMA_COMPRA

                                    });

                        gvAtingido.DataSource = lstAtingido.OrderByDescending(p => p.Vendas);
                        gvAtingido.DataBind();

                    }
                }
            }
        }

        protected void gvAtingido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AtingidoAgora atingidoAgora = e.Row.DataItem as AtingidoAgora;

            if (atingidoAgora != null)
            {

                Literal litVendas = e.Row.FindControl("litVendas") as Literal;
                litVendas.Text = atingidoAgora.Vendas.ToString("##,###");

                Literal litCotas = e.Row.FindControl("litCotas") as Literal;
                litCotas.Text = atingidoAgora.Cota.ToString("##,###");

                Literal litAtingido = e.Row.FindControl("litAtingido") as Literal;
                litAtingido.Text = atingidoAgora.Atingido;

                Literal litPA = e.Row.FindControl("litPA") as Literal;
                litPA.Text = atingidoAgora.Pa;

                Literal litTickets = e.Row.FindControl("litTickets") as Literal;
                litTickets.Text = atingidoAgora.Tickets.ToString();

                Literal litTicketMedio = e.Row.FindControl("litTicketMedio") as Literal;
                litTicketMedio.Text = atingidoAgora.TicketMedio.ToString("##,##0.00");

                Literal litUltimaVenda = e.Row.FindControl("litUltimaVenda") as Literal;
                litUltimaVenda.Text = atingidoAgora.Hora;

                if (atingidoAgora.Filial.ToLower() == "handbook online")
                    e.Row.BackColor = Color.FloralWhite;

                totalVendas += (atingidoAgora.Vendas == null) ? 0 : atingidoAgora.Vendas;
                totalCotas += (atingidoAgora.Cota == null) ? 0 : atingidoAgora.Cota;
                totalTickets += (atingidoAgora.Tickets == null) ? 0 : atingidoAgora.Tickets;
            }
        }
        protected void gvAtingido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAtingido.FooterRow;

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[2].Text = totalVendas.ToString("##,###");
                footer.Cells[3].Text = totalCotas.ToString("##,###");
                footer.Cells[4].Text = (totalCotas <= 0) ? "0" : ((Convert.ToDecimal(totalVendas) / Convert.ToDecimal(totalCotas)) * 100).ToString("N2") + "%";
                footer.Cells[6].Text = totalTickets.ToString("##,###");

            }
        }
    }
}
