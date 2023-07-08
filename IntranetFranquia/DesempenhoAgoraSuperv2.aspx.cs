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
    public partial class DesempenhoAgoraSuperv2 : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        int totalVendas = 0;
        int totalCotas = 0;
        int totalTickets = 0;
        double totalPercAtingido = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                var agoraRede = baseController.ObterAgoraRede("", null);

                var usuario = usuarioController.ValidaUsuarioSuper("maurilio");
                CarregarAgoraSuper(usuario, gvMaurilho, agoraRede);

                LimparTotais();
                usuario = usuarioController.ValidaUsuarioSuper("dirceu");
                CarregarAgoraSuper(usuario, gvDirceu, agoraRede);


                CarregarAgoraRede();
            }
        }

        private void LimparTotais()
        {
            totalVendas = 0;
            totalTickets = 0;
            totalCotas = 0;
            totalPercAtingido = 0;
        }
        private void CarregarAgoraSuper(USUARIO usuario, GridView gvAtingido, List<SP_AGORA_REDEResult> agoraRede)
        {
            var lstAtingido = new List<AtingidoAgora>();

            var lojasSuper = baseController.BuscaUsuarioLoja(usuario);

            List<string> idLojas = new List<string>();
            foreach (var i in lojasSuper)
                idLojas.Add(i.CODIGO_LOJA.Trim());

            agoraRede = agoraRede.Where(p => idLojas.Contains(p.CODIGO_FILIAL.Trim())).ToList();

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

            gvAtingido.DataSource = lstAtingido.OrderByDescending(p => p.Vendas);
            gvAtingido.DataBind();
        }
        private void CarregarAgoraRede()
        {
            var lstAtingido = new List<AtingidoAgora>();
            var vendas = 0;
            var cotas = 0;
            var tickets = 0;
            var qtdeProduto = 0;

            var agoraRede = baseController.ObterAgoraRede("", null);
            foreach (var ar in agoraRede)
            {
                vendas += Convert.ToInt32(ar.VENDAS);
                cotas += Convert.ToInt32(ar.COTAS);
                tickets += Convert.ToInt32(ar.TICKETS);
                qtdeProduto += ar.QTDE_PRODUTO_LIQ;
            }

            lstAtingido.Add(
                    new AtingidoAgora
                    {
                        Filial = "REDE",
                        Vendas = vendas,
                        Cota = cotas,
                        Atingido = (cotas <= 0) ? "0" : ((vendas * 1.00M / cotas) * 100.00M).ToString("N2") + "%",
                        Tickets = tickets,
                        TicketMedio = (tickets <= 0) ? 0M : (vendas * 1.00M / tickets),
                        Pa = (tickets <= 0) ? "0,00" : (qtdeProduto * 1.00M / tickets).ToString("N2"),
                        Hora = "-"
                    });

            gvRede.DataSource = lstAtingido.OrderByDescending(p => p.Vendas);
            gvRede.DataBind();
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
            GridViewRow footer = ((GridView)sender).FooterRow;

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
