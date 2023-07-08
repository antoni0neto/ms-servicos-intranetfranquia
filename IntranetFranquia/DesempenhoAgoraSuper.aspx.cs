using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class DesempenhoAgoraSuper : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();
        UsuarioController usuarioController = new UsuarioController();

        public int totalVendas = 0;
        public int totalTickets = 0;
        public int totalCotas = 1;
        public double totalPercAtingido = 0;

        public int totalVendasRede = 0;
        public int totalCotasRede = 1;
        public int totalTicketsRede = 1;
        public int totalQtsRede = 0;

        public int totalTicketsDennis = 0;
        public int totalTicketsClaudia = 0;
        public int totalTicketsMarcielly = 0;

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
                        USUARIO usuario = usuarioController.ValidaUsuarioSuper("Marcielly");

                        if (usuario != null)
                        {
                            if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 3)
                            {
                                List<USUARIOLOJA> usuarioLoja = new List<USUARIOLOJA>();
                                usuarioLoja = baseController.BuscaUsuarioLoja(usuario);

                                //List<FILIAI> filiais = baseController.BuscaFiliais_Agora(usuario);
                                List<string> idLojas = new List<string>();
                                foreach (USUARIOLOJA i in usuarioLoja)
                                    idLojas.Add(i.CODIGO_LOJA);
                                List<SP_OBTER_FILIAIS_MARCADASResult> filiais = baseController.ListarFiliaisMarcadas('X', 'S', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X');
                                filiais = filiais.Where(p => idLojas.Contains(p.COD_FILIAL)).ToList();

                                if (filiais != null)
                                {
                                    List<AtingidoAgora> listaAtingidoAgora = new List<AtingidoAgora>();

                                    //foreach (FILIAI item in filiais.Where(p => p.TIPO_FILIAL.Trim().Equals("LOJA")))
                                    foreach (SP_OBTER_FILIAIS_MARCADASResult item in filiais)
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

                                            totalVendasRede += atingidoAgora.Vendas;
                                            totalTicketsRede += atingidoAgora.Tickets;
                                            totalTicketsMarcielly += atingidoAgora.Tickets;
                                            totalQtsRede += Convert.ToInt32(dvl.Vendas_qt_total + dvl.Vendas_N);

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

                                        totalCotasRede += atingidoAgora.Cota;
                                    }

                                    listaAtingidoAgora = listaAtingidoAgora.OrderBy(c => c.Atingido).ToList();
                                    listaAtingidoAgora.Reverse();

                                    int i = 0;

                                    foreach (AtingidoAgora item in listaAtingidoAgora)
                                    {
                                        i++;

                                        item.Item = i;
                                    }

                                    GridViewAtingidoMarcielly.DataSource = listaAtingidoAgora.OrderBy(p => p.Filial);
                                    GridViewAtingidoMarcielly.DataBind();
                                }
                            }
                        }

                        totalVendas = 0;
                        totalTickets = 0;
                        totalCotas = 1;
                        totalPercAtingido = 0;

                        usuario = usuarioController.ValidaUsuarioSuper("Dennis");

                        if (usuario != null)
                        {
                            if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 3)
                            {
                                List<USUARIOLOJA> usuarioLoja = new List<USUARIOLOJA>();
                                usuarioLoja = baseController.BuscaUsuarioLoja(usuario);

                                List<string> idLojas = new List<string>();
                                foreach (USUARIOLOJA i in usuarioLoja)
                                    idLojas.Add(i.CODIGO_LOJA);
                                List<SP_OBTER_FILIAIS_MARCADASResult> filiais = baseController.ListarFiliaisMarcadas('X', 'S', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X');
                                filiais = filiais.Where(p => idLojas.Contains(p.COD_FILIAL)).ToList();

                                if (filiais != null)
                                {
                                    List<AtingidoAgora> listaAtingidoAgora = new List<AtingidoAgora>();

                                    foreach (SP_OBTER_FILIAIS_MARCADASResult item in filiais)
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

                                            totalVendasRede += atingidoAgora.Vendas;
                                            totalTicketsRede += atingidoAgora.Tickets;
                                            totalTicketsDennis += atingidoAgora.Tickets;
                                            totalQtsRede += Convert.ToInt32(dvl.Vendas_qt_total + dvl.Vendas_N);

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

                                        totalCotasRede += atingidoAgora.Cota;
                                    }

                                    listaAtingidoAgora = listaAtingidoAgora.OrderBy(c => c.Atingido).ToList();
                                    listaAtingidoAgora.Reverse();

                                    int i = 0;

                                    foreach (AtingidoAgora item in listaAtingidoAgora)
                                    {
                                        i++;

                                        item.Item = i;
                                    }

                                    GridViewAtingidoDennis.DataSource = listaAtingidoAgora.OrderBy(p => p.Filial);
                                    GridViewAtingidoDennis.DataBind();
                                }
                            }
                        }
                        totalVendas = 0;
                        totalTickets = 0;
                        totalCotas = 1;
                        totalPercAtingido = 0;

                        usuario = usuarioController.ValidaUsuarioSuper("Claudia");

                        if (usuario != null)
                        {
                            if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 3)
                            {
                                List<USUARIOLOJA> usuarioLoja = new List<USUARIOLOJA>();
                                usuarioLoja = baseController.BuscaUsuarioLoja(usuario);

                                List<string> idLojas = new List<string>();
                                foreach (USUARIOLOJA i in usuarioLoja)
                                    idLojas.Add(i.CODIGO_LOJA);
                                List<SP_OBTER_FILIAIS_MARCADASResult> filiais = baseController.ListarFiliaisMarcadas('X', 'S', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X');
                                filiais = filiais.Where(p => idLojas.Contains(p.COD_FILIAL)).ToList();

                                if (filiais != null)
                                {
                                    List<AtingidoAgora> listaAtingidoAgora = new List<AtingidoAgora>();

                                    foreach (SP_OBTER_FILIAIS_MARCADASResult item in filiais)
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

                                            totalVendasRede += atingidoAgora.Vendas;
                                            totalTicketsRede += atingidoAgora.Tickets;
                                            totalTicketsClaudia += atingidoAgora.Tickets;
                                            totalQtsRede += Convert.ToInt32(dvl.Vendas_qt_total + dvl.Vendas_N);

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

                                        totalCotasRede += atingidoAgora.Cota;
                                    }

                                    listaAtingidoAgora = listaAtingidoAgora.OrderBy(c => c.Atingido).ToList();
                                    listaAtingidoAgora.Reverse();

                                    int i = 0;

                                    foreach (AtingidoAgora item in listaAtingidoAgora)
                                    {
                                        i++;

                                        item.Item = i;
                                    }

                                    GridViewAtingidoClaudia.DataSource = listaAtingidoAgora.OrderBy(p => p.Filial);
                                    GridViewAtingidoClaudia.DataBind();
                                }
                            }
                        }

                        List<AtingidoAgora> tListaAtingidoAgora = new List<AtingidoAgora>();

                        AtingidoAgora tAtingidoAgora = new AtingidoAgora();

                        tAtingidoAgora.Filial = "Rede";
                        tAtingidoAgora.Vendas = totalVendasRede;
                        tAtingidoAgora.Cota = totalCotasRede;
                        tAtingidoAgora.Atingido = ((Convert.ToDecimal(tAtingidoAgora.Vendas) / Convert.ToDecimal(tAtingidoAgora.Cota)) * 100).ToString("N2") + "%";
                        tAtingidoAgora.Tickets = totalTicketsRede;
                        tAtingidoAgora.Pa = (Convert.ToDecimal(totalQtsRede) / Convert.ToDecimal(totalTicketsRede)).ToString("N2");

                        tListaAtingidoAgora.Add(tAtingidoAgora);

                        GridViewAtingidoRede.DataSource = tListaAtingidoAgora.OrderBy(p => p.Filial);
                        GridViewAtingidoRede.DataBind();
                    }
                }
            }
        }

        protected void GridViewAtingidoRede_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AtingidoAgora atingidoAgora = e.Row.DataItem as AtingidoAgora;

            if (atingidoAgora != null)
            {
                Literal literalVendas = e.Row.FindControl("LiteralVendas") as Literal;

                if (literalVendas != null)
                    literalVendas.Text = atingidoAgora.Vendas.ToString("##,###");

                Literal literalCota = e.Row.FindControl("LiteralCota") as Literal;

                if (literalCota != null)
                    literalCota.Text = atingidoAgora.Cota.ToString("##,###");

                Literal literalTicketMedio = e.Row.FindControl("LiteralTicketMedio") as Literal;

                if (literalTicketMedio != null)
                {
                    if (atingidoAgora.Tickets == 0)
                        atingidoAgora.Tickets = 1;

                    literalTicketMedio.Text = (Convert.ToDecimal(atingidoAgora.Vendas) / Convert.ToDecimal(atingidoAgora.Tickets)).ToString("##,##0.00");
                }
            }
        }

        protected void GridViewAtingidoMarcielly_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AtingidoAgora atingidoAgora = e.Row.DataItem as AtingidoAgora;

            if (atingidoAgora != null)
            {
                Literal literalVendas = e.Row.FindControl("LiteralVendas") as Literal;

                if (literalVendas != null)
                    literalVendas.Text = atingidoAgora.Vendas.ToString("##,###");

                Literal literalCota = e.Row.FindControl("LiteralCota") as Literal;

                if (literalCota != null)
                    literalCota.Text = atingidoAgora.Cota.ToString("##,###");

                Literal literalTicketMedio = e.Row.FindControl("LiteralTicketMedio") as Literal;

                if (literalTicketMedio != null)
                {
                    if (atingidoAgora.Tickets == 0)
                        atingidoAgora.Tickets = 1;

                    literalTicketMedio.Text = (Convert.ToDecimal(atingidoAgora.Vendas) / Convert.ToDecimal(atingidoAgora.Tickets)).ToString("##,##0.00");
                }

                totalVendas += atingidoAgora.Vendas;
                totalCotas += atingidoAgora.Cota;
            }
        }
        protected void GridViewAtingidoDennis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AtingidoAgora atingidoAgora = e.Row.DataItem as AtingidoAgora;

            if (atingidoAgora != null)
            {
                Literal literalVendas = e.Row.FindControl("LiteralVendas") as Literal;

                if (literalVendas != null)
                    literalVendas.Text = atingidoAgora.Vendas.ToString("##,###");

                Literal literalCota = e.Row.FindControl("LiteralCota") as Literal;

                if (literalCota != null)
                    literalCota.Text = atingidoAgora.Cota.ToString("##,###");

                Literal literalTicketMedio = e.Row.FindControl("LiteralTicketMedio") as Literal;

                if (literalTicketMedio != null)
                {
                    if (atingidoAgora.Tickets == 0)
                        atingidoAgora.Tickets = 1;

                    literalTicketMedio.Text = (Convert.ToDecimal(atingidoAgora.Vendas) / Convert.ToDecimal(atingidoAgora.Tickets)).ToString("##,##0.00");
                }

                totalVendas += atingidoAgora.Vendas;
                totalCotas += atingidoAgora.Cota;
            }
        }
        protected void GridViewAtingidoClaudia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AtingidoAgora atingidoAgora = e.Row.DataItem as AtingidoAgora;

            if (atingidoAgora != null)
            {
                Literal literalVendas = e.Row.FindControl("LiteralVendas") as Literal;

                if (literalVendas != null)
                    literalVendas.Text = atingidoAgora.Vendas.ToString("##,###");

                Literal literalCota = e.Row.FindControl("LiteralCota") as Literal;

                if (literalCota != null)
                    literalCota.Text = atingidoAgora.Cota.ToString("##,###");

                Literal literalTicketMedio = e.Row.FindControl("LiteralTicketMedio") as Literal;

                if (literalTicketMedio != null)
                {
                    if (atingidoAgora.Tickets == 0)
                        atingidoAgora.Tickets = 1;

                    literalTicketMedio.Text = (Convert.ToDecimal(atingidoAgora.Vendas) / Convert.ToDecimal(atingidoAgora.Tickets)).ToString("##,##0.00");
                }

                totalVendas += atingidoAgora.Vendas;
                totalCotas += atingidoAgora.Cota;
            }
        }

        protected void GridViewAtingidoMarcielly_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewAtingidoMarcielly.FooterRow;

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[2].Text = totalVendas.ToString("##,###");
                footer.Cells[3].Text = totalCotas.ToString("##,###");
                footer.Cells[4].Text = ((Convert.ToDecimal(totalVendas) / Convert.ToDecimal(totalCotas)) * 100).ToString("N2") + "%";
                footer.Cells[6].Text = totalTicketsMarcielly.ToString("##,###");
            }
        }
        protected void GridViewAtingidoDennis_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewAtingidoDennis.FooterRow;

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[2].Text = totalVendas.ToString("##,###");
                footer.Cells[3].Text = totalCotas.ToString("##,###");
                footer.Cells[4].Text = ((Convert.ToDecimal(totalVendas) / Convert.ToDecimal(totalCotas)) * 100).ToString("N2") + "%";
                footer.Cells[6].Text = totalTicketsDennis.ToString("##,###");
            }
        }
        protected void GridViewAtingidoClaudia_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewAtingidoClaudia.FooterRow;

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[2].Text = totalVendas.ToString("##,###");
                footer.Cells[3].Text = totalCotas.ToString("##,###");
                footer.Cells[4].Text = ((Convert.ToDecimal(totalVendas) / Convert.ToDecimal(totalCotas)) * 100).ToString("N2") + "%";
                footer.Cells[6].Text = totalTicketsClaudia.ToString("##,###");
            }
        }

    }
}
