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
    public partial class DesempenhoAgoraRede : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        public int totalVendas = 0;
        public int totalTickets = 0;
        public int totalCotas = 1;
        public double totalPercAtingido = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Sp_Pega_Data_BancoResult data = baseController.BuscaDataBanco();

                if (data != null)
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
                                //List<FILIAI> filiais = baseController.BuscaFiliais_Agora();
                                List<SP_OBTER_FILIAIS_MARCADASResult> filiais = baseController.ListarFiliaisMarcadas('S', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X');

                                List<FILIAIS_DE_PARA> dePara = new List<FILIAIS_DE_PARA>();
                                dePara = baseController.BuscaFilialDePara();

                                if (filiais != null)
                                {
                                    List<AtingidoAgora> listaAtingidoAgora = new List<AtingidoAgora>();

                                    //foreach (FILIAI item in filiais)
                                    foreach (SP_OBTER_FILIAIS_MARCADASResult item in filiais)
                                    {
                                        if (dePara.Where(p => p.DE.Trim() == item.COD_FILIAL.Trim()).Count() <= 0)
                                        {
                                            decimal? vendas = baseController.BuscaVendasAgora(data.data, item.COD_FILIAL);
                                            decimal? cota = baseController.BuscaCotaAgora(data.data, item.COD_FILIAL);

                                            if (cota == null || cota == 0)
                                                cota = 1;

                                            int? tickets = baseController.BuscaTicketsAgora(data.data, item.COD_FILIAL);
                                            //decimal? qt = baseController.BuscaQtAgora(data.data, item.COD_FILIAL);

                                            Sp_Desempenho_Venda_LojaResult dvl = desempenhoController.BuscaDesempenhoVendaLoja(Convert.ToDateTime(data.data).ToString("dd/MM/yyyy"), Convert.ToDateTime(data.data).ToString("dd/MM/yyyy"), item.COD_FILIAL);

                                            AtingidoAgora atingidoAgora = new AtingidoAgora();

                                            if (vendas != null & tickets != null & dvl != null)
                                            {
                                                atingidoAgora.Filial = item.FILIAL;
                                                atingidoAgora.Vendas = Convert.ToInt32(vendas);
                                                atingidoAgora.Cota = Convert.ToInt32(cota);
                                                atingidoAgora.Atingido = ((Convert.ToDecimal(atingidoAgora.Vendas) / Convert.ToDecimal(atingidoAgora.Cota)) * 100).ToString("N2") + "%";

                                                DateTime? dataHoraUltimaVenda = baseController.BuscaDataHoraUltimaVenda(data.data, item.COD_FILIAL);

                                                if (dataHoraUltimaVenda != null)
                                                    atingidoAgora.Hora = (Convert.ToDateTime(dataHoraUltimaVenda)).ToString("HH:mm");

                                                atingidoAgora.Tickets = Convert.ToInt32(tickets);
                                                atingidoAgora.Pa = (Convert.ToDecimal(dvl.Vendas_qt_total + dvl.Vendas_N) / Convert.ToDecimal(tickets)).ToString("N2");

                                                listaAtingidoAgora.Add(atingidoAgora);
                                            }
                                            else
                                            {
                                                atingidoAgora.Filial = item.FILIAL;
                                                atingidoAgora.Vendas = 0;
                                                atingidoAgora.Cota = Convert.ToInt32(cota);
                                                atingidoAgora.Atingido = "0%";
                                                atingidoAgora.Tickets = 0;
                                                atingidoAgora.Pa = "0%";

                                                listaAtingidoAgora.Add(atingidoAgora);
                                            }
                                        }
                                    }

                                    var ecom = baseController.ObterEcomAgoraRede();
                                    if (ecom != null)
                                        listaAtingidoAgora.Add(new AtingidoAgora { Filial = ecom.FILIAL, Tickets = Convert.ToInt32(ecom.TOT_PEDIDO), Hora = ecom.ULTIMA_COMPRA, Vendas = Convert.ToInt32(ecom.VALOR_VENDA) });

                                    listaAtingidoAgora = listaAtingidoAgora.OrderBy(c => c.Vendas).ToList();
                                    listaAtingidoAgora.Reverse();

                                    int i = 0;

                                    foreach (AtingidoAgora item in listaAtingidoAgora)
                                    {
                                        i++;

                                        item.Item = i;
                                    }

                                    GridViewAtingido.DataSource = listaAtingidoAgora;
                                    GridViewAtingido.DataBind();
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void GridViewAtingido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AtingidoAgora atingidoAgora = e.Row.DataItem as AtingidoAgora;

            if (atingidoAgora != null)
            {
                Literal literalVendas = e.Row.FindControl("LiteralVendas") as Literal;
                if (literalVendas != null && atingidoAgora.Vendas != null)
                    literalVendas.Text = atingidoAgora.Vendas.ToString("##,###");

                Literal literalCota = e.Row.FindControl("LiteralCota") as Literal;
                if (literalCota != null && atingidoAgora.Cota != null)
                    literalCota.Text = atingidoAgora.Cota.ToString("##,###");

                totalVendas += (atingidoAgora.Vendas == null) ? 0 : atingidoAgora.Vendas;
                totalCotas += (atingidoAgora.Cota == null) ? 0 : atingidoAgora.Cota;
                totalTickets += (atingidoAgora.Tickets == null) ? 0 : atingidoAgora.Tickets;

                Literal literalTicketMedio = e.Row.FindControl("LiteralTicketMedio") as Literal;
                if (literalTicketMedio != null && atingidoAgora.Tickets != null)
                {
                    if (atingidoAgora.Tickets == 0)
                        atingidoAgora.Tickets = 1;
                    literalTicketMedio.Text = (Convert.ToDecimal(atingidoAgora.Vendas) / Convert.ToDecimal(atingidoAgora.Tickets)).ToString("##,##0.00");
                }

                if (atingidoAgora.Filial.ToLower() == "handbook online")
                    e.Row.BackColor = Color.FloralWhite;
            }
        }

        protected void GridViewAtingido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewAtingido.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = totalVendas.ToString("##,###");
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalCotas.ToString("##,###");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[4].Text = ((Convert.ToDecimal(totalVendas) / Convert.ToDecimal(totalCotas)) * 100).ToString("N2") + "%";
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[5].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[6].Text = totalTickets.ToString("##,###");
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
    }
}
