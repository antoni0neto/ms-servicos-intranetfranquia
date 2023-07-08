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
    public partial class gerloja_desempenho_vendedorv2 : System.Web.UI.Page
    {
        DesempenhoController desempenhoController = new DesempenhoController();
        BaseController baseController = new BaseController();

        int ticket0 = 0;
        int ticket1 = 0;
        int ticket2 = 0;
        int ticket3 = 0;
        int ticket4 = 0;
        int ticket5 = 0;
        int ticketTotal = 0;

        int comLink = 0;
        int semLink = 0;

        int qtdeProduto = 0;
        int totalAtendido = 0;
        decimal notaFinal = 0;
        int numeroDias = 0;
        int totalVendedor = 0;
        decimal valorPago = 0;

        Color corVerde = Color.PaleGreen;
        Color corVermelho = Color.LightPink;
        Color corAmarelo = Color.PaleGoldenrod;
        Color corNeutra = Color.LightBlue;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!IsPostBack)
            {
                CarregarFilial();

                //Esconder paineis
                pnlFilial.Visible = false;

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                filiais.Add(new FILIAI { COD_FILIAL = "G", FILIAL = "GERAL" });

                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais.Count() == 3)
                    ddlFilial.SelectedIndex = 1;
            }
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                DateTime dataIni = DateTime.Today;
                DateTime dataFim = DateTime.Today;

                if (txtPeriodoInicial.Text.Trim() == "" || !DateTime.TryParse(txtPeriodoInicial.Text.Trim(), out dataIni))
                {
                    labErro.Text = "Informe uma Data Inicial.";
                    return;
                }

                if (txtPeriodoFinal.Text.Trim() == "" || !DateTime.TryParse(txtPeriodoFinal.Text.Trim(), out dataFim))
                {
                    labErro.Text = "Informe uma Data Final.";
                    return;
                }

                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma Filial.";
                    return;
                }

                pnlFilial.Visible = false;
                pnlGeral.Visible = false;
                if (ddlFilial.SelectedValue != "G")
                {
                    CarregarDesempenhoVendedores(ddlFilial.SelectedValue.Trim(), dataIni, dataFim);
                    CarregarDesempenhoRede(dataIni, dataFim);
                    pnlFilial.Visible = true;
                }
                else
                {
                    CarregarDesempenhoFiliais(dataIni, dataFim);
                    pnlGeral.Visible = true;
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID VIEW DESEMPENHO VENDEDORES"
        private void CarregarDesempenhoVendedores(string codigoFilial, DateTime dataIni, DateTime dataFim)
        {
            var desVendedores = desempenhoController.ObterDesempenhoVendedores(codigoFilial, dataIni, dataFim, "");

            gvDesempenhoVendedor.DataSource = desVendedores;
            gvDesempenhoVendedor.DataBind();
        }
        protected void gvDesempenhoVendedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_DESEMPENHO_VENDEDORResult des = e.Row.DataItem as SP_OBTER_DESEMPENHO_VENDEDORResult;

            if (des != null)
            {
                Literal litTicket0 = e.Row.FindControl("litTicket0") as Literal;
                litTicket0.Text = des.TICKET_0.ToString();
                Literal litTicket0PORC = e.Row.FindControl("litTicket0PORC") as Literal;
                litTicket0PORC.Text = Convert.ToDecimal(des.TICKET_0_PORC).ToString("N2") + "%";

                Literal litTicket1 = e.Row.FindControl("litTicket1") as Literal;
                litTicket1.Text = des.TICKET_1.ToString();
                Literal litTicket1PORC = e.Row.FindControl("litTicket1PORC") as Literal;
                litTicket1PORC.Text = Convert.ToDecimal(des.TICKET_1_PORC).ToString("N2") + "%";

                Literal litTicket2 = e.Row.FindControl("litTicket2") as Literal;
                litTicket2.Text = des.TICKET_2.ToString();
                Literal litTicket2PORC = e.Row.FindControl("litTicket2PORC") as Literal;
                litTicket2PORC.Text = Convert.ToDecimal(des.TICKET_2_PORC).ToString("N2") + "%";

                Literal litTicket3 = e.Row.FindControl("litTicket3") as Literal;
                litTicket3.Text = des.TICKET_3.ToString();
                Literal litTicket3PORC = e.Row.FindControl("litTicket3PORC") as Literal;
                litTicket3PORC.Text = Convert.ToDecimal(des.TICKET_3_PORC).ToString("N2") + "%";

                Literal litTicket4 = e.Row.FindControl("litTicket4") as Literal;
                litTicket4.Text = des.TICKET_4.ToString();
                Literal litTicket4PORC = e.Row.FindControl("litTicket4PORC") as Literal;
                litTicket4PORC.Text = Convert.ToDecimal(des.TICKET_4_PORC).ToString("N2") + "%";

                Literal litTicket5 = e.Row.FindControl("litTicket5") as Literal;
                litTicket5.Text = des.TICKET_5.ToString();
                Literal litTicket5PORC = e.Row.FindControl("litTicket5PORC") as Literal;
                litTicket5PORC.Text = Convert.ToDecimal(des.TICKET_5_PORC).ToString("N2") + "%";

                Literal litPA = e.Row.FindControl("litPA") as Literal;
                litPA.Text = Convert.ToDecimal(des.PA).ToString("N2");

                Literal liMediaAtendimento = e.Row.FindControl("liMediaAtendimento") as Literal;
                liMediaAtendimento.Text = Convert.ToDecimal(des.MEDIA_ATENDIMENTO).ToString("N2");

                Literal litTicketMedio = e.Row.FindControl("litTicketMedio") as Literal;
                litTicketMedio.Text = Convert.ToDecimal(des.TICKET_MEDIO).ToString("N2");

                Literal litNotaLetra = e.Row.FindControl("litNotaLetra") as Literal;
                litNotaLetra.Text = "";

                Literal litNotaFinal = e.Row.FindControl("litNotaFinal") as Literal;
                litNotaFinal.Text = Convert.ToDecimal(des.NOTA_FINAL).ToString("###,###,##0.00");

                ticket0 += des.TICKET_0;
                ticket1 += des.TICKET_1;
                ticket2 += des.TICKET_2;
                ticket3 += des.TICKET_3;
                ticket4 += des.TICKET_4;
                ticket5 += des.TICKET_5;
                ticketTotal += des.TICKET_TOTAL;

                comLink += Convert.ToInt32(des.CLIENTE_LINK);
                semLink += des.CLIENTE_SEMLINK;

                qtdeProduto += des.QTDE_PRODUTO_TOTAL;
                totalAtendido += des.TICKET_TOTAL;
                notaFinal += Convert.ToDecimal(des.NOTA_FINAL);
                numeroDias += des.NUMERO_DIAS;
                totalVendedor += 1;
                valorPago += Convert.ToDecimal(des.VALOR_PAGO);

            }
        }
        protected void gvDesempenhoVendedor_DataBound(object sender, EventArgs e)
        {
            decimal totPA = 0;
            decimal totMediaAtendimento = 0;
            decimal totTicketMedio = 0;

            // FOOTER
            GridViewRow footer = gvDesempenhoVendedor.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "Total";

                footer.Cells[1].Text = ticket0.ToString();
                footer.Cells[2].Text = (ticketTotal <= 0) ? "0" : ((ticket0 * 1.00M / ticketTotal) * 100.00M).ToString("N2") + "%";

                footer.Cells[3].Text = ticket1.ToString();
                footer.Cells[4].Text = (ticketTotal <= 0) ? "0" : ((ticket1 * 1.00M / ticketTotal) * 100.00M).ToString("N2") + "%";

                footer.Cells[5].Text = ticket2.ToString();
                footer.Cells[6].Text = (ticketTotal <= 0) ? "0" : ((ticket2 * 1.00M / ticketTotal) * 100.00M).ToString("N2") + "%";

                footer.Cells[7].Text = ticket3.ToString();
                footer.Cells[8].Text = (ticketTotal <= 0) ? "0" : ((ticket3 * 1.00M / ticketTotal) * 100.00M).ToString("N2") + "%";

                footer.Cells[9].Text = ticket4.ToString();
                footer.Cells[10].Text = (ticketTotal <= 0) ? "0" : ((ticket4 * 1.00M / ticketTotal) * 100.00M).ToString("N2") + "%";

                footer.Cells[11].Text = ticket5.ToString();
                footer.Cells[12].Text = (ticketTotal <= 0) ? "0" : ((ticket5 * 1.00M / ticketTotal) * 100.00M).ToString("2") + "%";

                totPA = (ticketTotal <= 0) ? 0 : (qtdeProduto * 1.00M / ticketTotal);
                footer.Cells[13].Text = totPA.ToString("N2"); //PA

                totMediaAtendimento = (numeroDias <= 0) ? 0 : (ticketTotal * 1.00M / numeroDias);
                footer.Cells[14].Text = totMediaAtendimento.ToString("N2"); //MEDIA ATENDIMENTO

                totTicketMedio = (ticketTotal <= 0) ? 0 : (valorPago * 1.00M / ticketTotal);
                footer.Cells[15].Text = totTicketMedio.ToString("N2"); //TICKET MEDIO

                footer.Cells[16].Text = "";

                footer.Cells[17].Text = comLink.ToString();
                footer.Cells[18].Text = semLink.ToString();

                footer.Cells[19].Text = qtdeProduto.ToString(); // total de peças / qtde de produtos
                footer.Cells[20].Text = totalAtendido.ToString();
                footer.Cells[21].Text = notaFinal.ToString("###,###,##0.00");

                footer.Cells[22].Text = (totalVendedor <= 0) ? "0" : (numeroDias * 1.00M / totalVendedor).ToString("N2");
            }

            //LOOP NAS LINHAS PARA PINTAR E CALCULAR A NOTA
            var notaValor = 0;
            foreach (GridViewRow row in gvDesempenhoVendedor.Rows)
            {

                //TICKETS
                Literal litTicket1 = row.FindControl("litTicket1PORC") as Literal;
                Literal litTicket2 = row.FindControl("litTicket2PORC") as Literal;
                Literal litTicket3 = row.FindControl("litTicket3PORC") as Literal;
                Literal litTicket4 = row.FindControl("litTicket4PORC") as Literal;
                Literal litTicket5 = row.FindControl("litTicket5PORC") as Literal;
                row.Cells[1].BackColor = Color.LightBlue;
                row.Cells[2].BackColor = Color.LightBlue;
                row.Cells[3].BackColor = Color.LightBlue;
                row.Cells[4].BackColor = PintarLinhaTicket(litTicket1, ticket1, ticketTotal);
                row.Cells[5].BackColor = Color.LightBlue;
                row.Cells[6].BackColor = PintarLinhaTicket(litTicket2, ticket2, ticketTotal);
                row.Cells[7].BackColor = Color.LightBlue;
                row.Cells[8].BackColor = PintarLinhaTicket(litTicket3, ticket3, ticketTotal);
                row.Cells[9].BackColor = Color.LightBlue;
                row.Cells[10].BackColor = PintarLinhaTicket(litTicket4, ticket4, ticketTotal);
                row.Cells[11].BackColor = Color.LightBlue;
                row.Cells[12].BackColor = PintarLinhaTicket(litTicket5, ticket5, ticketTotal);

                notaValor = 4;
                //PA
                Literal litPA = row.FindControl("litPA") as Literal;
                if (Convert.ToDecimal(litPA.Text) > totPA)
                {
                    row.Cells[13].BackColor = corVerde;
                }
                else
                {
                    notaValor = notaValor - 1;
                    row.Cells[13].BackColor = corVermelho;
                }

                //MEDIA ATENDIMENTO
                Literal liMediaAtendimento = row.FindControl("liMediaAtendimento") as Literal;
                if (Convert.ToDecimal(liMediaAtendimento.Text) > totMediaAtendimento)
                {
                    row.Cells[14].BackColor = corVerde;
                }
                else
                {
                    notaValor = notaValor - 1;
                    row.Cells[14].BackColor = corVermelho;
                }

                //TICKET MEDIO
                Literal litTicketMedio = row.FindControl("litTicketMedio") as Literal;
                if (Convert.ToDecimal(litTicketMedio.Text) > totTicketMedio)
                {
                    row.Cells[15].BackColor = corVerde;
                }
                else
                {
                    notaValor = notaValor - 1;
                    row.Cells[15].BackColor = corVermelho;
                }

                //NOTA
                Literal litNotaLetra = row.FindControl("litNotaLetra") as Literal;
                if (notaValor == 4)
                {
                    litNotaLetra.Text = "A";
                    row.Cells[16].BackColor = corVerde;
                }
                else if (notaValor == 3)
                {
                    litNotaLetra.Text = "B";
                    row.Cells[16].BackColor = corAmarelo;
                }
                else if (notaValor == 2)
                {
                    litNotaLetra.Text = "B-";
                    row.Cells[16].BackColor = corNeutra;
                }
                else
                {
                    litNotaLetra.Text = "C";
                    row.Cells[16].BackColor = corVermelho;
                }

                if (Convert.ToInt32(row.Cells[18].Text) > 0)
                    row.Cells[18].BackColor = corVermelho;

            }

        }

        private void CarregarDesempenhoRede(DateTime dataIni, DateTime dataFim)
        {
            var desRede = desempenhoController.ObterDesempenhoFiliais("", dataIni, dataFim);

            var media = 0M;
            foreach (var des in desRede)
                media += (des.NUMERO_DIAS > 0 && des.NUMERO_VENDEDOR > 0) ? ((des.QTDE_PRODUTO_TOTAL * 1.00M) / (des.NUMERO_DIAS * 1.00M) / des.NUMERO_VENDEDOR) : 0;

            //agrupar
            var ticket1 = desRede.Sum(p => p.TICKET_1);
            var ticket2 = desRede.Sum(p => p.TICKET_2);
            var ticket3 = desRede.Sum(p => p.TICKET_3);
            var ticket4 = desRede.Sum(p => p.TICKET_4);
            var ticket5 = desRede.Sum(p => p.TICKET_5);

            var ticketTotal = desRede.Sum(p => p.TICKET_TOTAL);
            var qtdeProdutoTotal = desRede.Sum(p => p.QTDE_PRODUTO_TOTAL);
            var valorPagoTotal = desRede.Sum(p => p.VALOR_PAGO);
            var filialTotal = desRede.Count();

            var redePA = (ticketTotal <= 0) ? 0 : (qtdeProdutoTotal * 1.00M / ticketTotal);
            var redeTicketMedio = (ticketTotal <= 0) ? 0 : (valorPagoTotal * 1.00M / ticketTotal);

            var mediaQtdePeca = (filialTotal <= 0) ? 0 : (media * 1.00M / filialTotal);

            var qtde1 = (ticketTotal <= 0) ? 0 : (((ticket1 * 1.00M) / ticketTotal) * 100.00M);
            var qtde12 = (ticketTotal <= 0) ? 0 : ((((ticket1 + ticket2) * 1.00M) / ticketTotal) * 100.00M);
            var qtde45 = (ticketTotal <= 0) ? 0 : ((((ticket4 + ticket5) * 1.00M) / ticketTotal) * 100.00M);

            var rede = new List<SP_OBTER_DESEMPENHO_VENDEDOR_REDEResult>();
            rede.Add(new SP_OBTER_DESEMPENHO_VENDEDOR_REDEResult
                {
                    PA = redePA,
                    MEDIA_ATENDIMENTO = mediaQtdePeca,
                    TICKET_MEDIO = redeTicketMedio,
                    TICKET_1_PORC = qtde1,
                    TICKET_2_PORC = qtde12,
                    TICKET_4_PORC = qtde45,
                });

            gvRede.DataSource = rede;
            gvRede.DataBind();
        }
        protected void gvRede_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_DESEMPENHO_VENDEDOR_REDEResult des = e.Row.DataItem as SP_OBTER_DESEMPENHO_VENDEDOR_REDEResult;

            if (des != null)
            {

                Literal litPA = e.Row.FindControl("litPA") as Literal;
                litPA.Text = Convert.ToDecimal(des.PA).ToString("N2");

                Literal litMediaQtdePeca = e.Row.FindControl("litMediaQtdePeca") as Literal;
                litMediaQtdePeca.Text = Convert.ToDecimal(des.MEDIA_ATENDIMENTO).ToString("N2");

                Literal litTicketMedio = e.Row.FindControl("litTicketMedio") as Literal;
                litTicketMedio.Text = Convert.ToDecimal(des.TICKET_MEDIO).ToString("N2");

                Literal litQtde1 = e.Row.FindControl("litQtde1") as Literal;
                litQtde1.Text = Convert.ToDecimal(des.TICKET_1_PORC).ToString("N0") + "%";

                Literal litQtde12 = e.Row.FindControl("litQtde12") as Literal;
                litQtde12.Text = Convert.ToDecimal(des.TICKET_2_PORC).ToString("N0") + "%";

                Literal litQtde45 = e.Row.FindControl("litQtde45") as Literal;
                litQtde45.Text = Convert.ToDecimal(des.TICKET_4_PORC).ToString("N0") + "%";

            }
        }
        #endregion

        #region "GRID VIEW DESEMPENHO FILIAIS"
        private void CarregarDesempenhoFiliais(DateTime dataIni, DateTime dataFim)
        {
            var desFiliais = desempenhoController.ObterDesempenhoFiliais("", dataIni, dataFim);

            //filtrar por filiais que o usuario pode ver
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            var filiais = baseController.BuscaFiliais(usuario);
            desFiliais = desFiliais.Where(p => filiais.Any(x => x.COD_FILIAL.Trim() == p.CODIGO_FILIAL.Trim())).ToList();

            gvDesempenhoFilial.DataSource = desFiliais;
            gvDesempenhoFilial.DataBind();
        }
        protected void gvDesempenhoFilial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_DESEMPENHO_VENDEDOR_REDEResult des = e.Row.DataItem as SP_OBTER_DESEMPENHO_VENDEDOR_REDEResult;

            if (des != null)
            {
                Literal litTicket0 = e.Row.FindControl("litTicket0") as Literal;
                litTicket0.Text = des.TICKET_0.ToString();
                Literal litTicket0PORC = e.Row.FindControl("litTicket0PORC") as Literal;
                litTicket0PORC.Text = Convert.ToDecimal(des.TICKET_0_PORC).ToString("N0") + "%";

                Literal litTicket1 = e.Row.FindControl("litTicket1") as Literal;
                litTicket1.Text = des.TICKET_1.ToString();
                Literal litTicket1PORC = e.Row.FindControl("litTicket1PORC") as Literal;
                litTicket1PORC.Text = Convert.ToDecimal(des.TICKET_1_PORC).ToString("N0") + "%";

                Literal litTicket2 = e.Row.FindControl("litTicket2") as Literal;
                litTicket2.Text = des.TICKET_2.ToString();
                Literal litTicket2PORC = e.Row.FindControl("litTicket2PORC") as Literal;
                litTicket2PORC.Text = Convert.ToDecimal(des.TICKET_2_PORC).ToString("N0") + "%";

                Literal litTicket3 = e.Row.FindControl("litTicket3") as Literal;
                litTicket3.Text = des.TICKET_3.ToString();
                Literal litTicket3PORC = e.Row.FindControl("litTicket3PORC") as Literal;
                litTicket3PORC.Text = Convert.ToDecimal(des.TICKET_3_PORC).ToString("N0") + "%";

                Literal litTicket4 = e.Row.FindControl("litTicket4") as Literal;
                litTicket4.Text = des.TICKET_4.ToString();
                Literal litTicket4PORC = e.Row.FindControl("litTicket4PORC") as Literal;
                litTicket4PORC.Text = Convert.ToDecimal(des.TICKET_4_PORC).ToString("N0") + "%";

                Literal litTicket5 = e.Row.FindControl("litTicket5") as Literal;
                litTicket5.Text = des.TICKET_5.ToString();
                Literal litTicket5PORC = e.Row.FindControl("litTicket5PORC") as Literal;
                litTicket5PORC.Text = Convert.ToDecimal(des.TICKET_5_PORC).ToString("N0") + "%";

                Literal litPA = e.Row.FindControl("litPA") as Literal;
                litPA.Text = Convert.ToDecimal(des.PA).ToString("N2");

                Literal liMediaAtendimento = e.Row.FindControl("liMediaAtendimento") as Literal;
                liMediaAtendimento.Text = Convert.ToDecimal(des.MEDIA_ATENDIMENTO).ToString("N2");

                Literal litTicketMedio = e.Row.FindControl("litTicketMedio") as Literal;
                litTicketMedio.Text = Convert.ToDecimal(des.TICKET_MEDIO).ToString("N2");

                Literal litNotaLetra = e.Row.FindControl("litNotaLetra") as Literal;
                litNotaLetra.Text = "";

                Literal litNotaFinal = e.Row.FindControl("litNotaFinal") as Literal;
                litNotaFinal.Text = Convert.ToDecimal(des.NOTA_FINAL).ToString("###,###,##0.00");

                ticket0 += des.TICKET_0;
                ticket1 += des.TICKET_1;
                ticket2 += des.TICKET_2;
                ticket3 += des.TICKET_3;
                ticket4 += des.TICKET_4;
                ticket5 += des.TICKET_5;
                ticketTotal += des.TICKET_TOTAL;

                comLink += Convert.ToInt32(des.CLIENTE_LINK);
                semLink += des.CLIENTE_SEMLINK;

                qtdeProduto += des.QTDE_PRODUTO_TOTAL;
                totalAtendido += des.TICKET_TOTAL;
                notaFinal += Convert.ToDecimal(des.NOTA_FINAL);
                numeroDias += des.NUMERO_DIAS;
                totalVendedor += 1;
                valorPago += Convert.ToDecimal(des.VALOR_PAGO);

            }
        }
        protected void gvDesempenhoFilial_DataBound(object sender, EventArgs e)
        {
            decimal totPA = 0;
            decimal totMediaAtendimento = 0;
            decimal totTicketMedio = 0;


            // FOOTER
            GridViewRow footer = gvDesempenhoFilial.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "Total";

                footer.Cells[1].Text = ticket0.ToString();
                footer.Cells[2].Text = (ticketTotal <= 0) ? "0" : ((ticket0 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

                footer.Cells[3].Text = ticket1.ToString();
                footer.Cells[4].Text = (ticketTotal <= 0) ? "0" : ((ticket1 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

                footer.Cells[5].Text = ticket2.ToString();
                footer.Cells[6].Text = (ticketTotal <= 0) ? "0" : ((ticket2 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

                footer.Cells[7].Text = ticket3.ToString();
                footer.Cells[8].Text = (ticketTotal <= 0) ? "0" : ((ticket3 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

                footer.Cells[9].Text = ticket4.ToString();
                footer.Cells[10].Text = (ticketTotal <= 0) ? "0" : ((ticket4 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

                footer.Cells[11].Text = ticket5.ToString();
                footer.Cells[12].Text = (ticketTotal <= 0) ? "0" : ((ticket5 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

                totPA = (ticketTotal <= 0) ? 0 : (qtdeProduto * 1.00M / ticketTotal);
                footer.Cells[13].Text = totPA.ToString("N2"); //PA

                totMediaAtendimento = (numeroDias <= 0) ? 0 : (ticketTotal * 1.00M / numeroDias);
                footer.Cells[14].Text = totMediaAtendimento.ToString("N2"); //MEDIA ATENDIMENTO

                totTicketMedio = (ticketTotal <= 0) ? 0 : (valorPago * 1.00M / ticketTotal);
                footer.Cells[15].Text = totTicketMedio.ToString("N2"); //TICKET MEDIO

                footer.Cells[16].Text = "";

                footer.Cells[17].Text = comLink.ToString();
                footer.Cells[18].Text = semLink.ToString();

                footer.Cells[19].Text = qtdeProduto.ToString(); // total de peças / qtde de produtos
                footer.Cells[20].Text = totalAtendido.ToString();
                footer.Cells[21].Text = notaFinal.ToString("###,###,##0.00");

                footer.Cells[22].Text = (totalVendedor <= 0) ? "0" : (numeroDias * 1.00M / totalVendedor).ToString("N2");
            }

            //LOOP NAS LINHAS PARA PINTAR E CALCULAR A NOTA
            var notaValor = 0;
            foreach (GridViewRow row in gvDesempenhoFilial.Rows)
            {

                //TICKETS
                Literal litTicket1 = row.FindControl("litTicket1PORC") as Literal;
                Literal litTicket2 = row.FindControl("litTicket2PORC") as Literal;
                Literal litTicket3 = row.FindControl("litTicket3PORC") as Literal;
                Literal litTicket4 = row.FindControl("litTicket4PORC") as Literal;
                Literal litTicket5 = row.FindControl("litTicket5PORC") as Literal;
                row.Cells[1].BackColor = Color.LightBlue;
                row.Cells[2].BackColor = Color.LightBlue;
                row.Cells[3].BackColor = Color.LightBlue;
                row.Cells[4].BackColor = PintarLinhaTicket(litTicket1, ticket1, ticketTotal);
                row.Cells[5].BackColor = Color.LightBlue;
                row.Cells[6].BackColor = PintarLinhaTicket(litTicket2, ticket2, ticketTotal);
                row.Cells[7].BackColor = Color.LightBlue;
                row.Cells[8].BackColor = PintarLinhaTicket(litTicket3, ticket3, ticketTotal);
                row.Cells[9].BackColor = Color.LightBlue;
                row.Cells[10].BackColor = PintarLinhaTicket(litTicket4, ticket4, ticketTotal);
                row.Cells[11].BackColor = Color.LightBlue;
                row.Cells[12].BackColor = PintarLinhaTicket(litTicket5, ticket5, ticketTotal);

                notaValor = 4;
                //PA
                Literal litPA = row.FindControl("litPA") as Literal;
                if (Convert.ToDecimal(litPA.Text) > totPA)
                {
                    row.Cells[13].BackColor = corVerde;
                }
                else
                {
                    notaValor = notaValor - 1;
                    row.Cells[13].BackColor = corVermelho;
                }

                //MEDIA ATENDIMENTO
                Literal liMediaAtendimento = row.FindControl("liMediaAtendimento") as Literal;
                if (Convert.ToDecimal(liMediaAtendimento.Text) > totMediaAtendimento)
                {
                    row.Cells[14].BackColor = corVerde;
                }
                else
                {
                    notaValor = notaValor - 1;
                    row.Cells[14].BackColor = corVermelho;
                }

                //TICKET MEDIO
                Literal litTicketMedio = row.FindControl("litTicketMedio") as Literal;
                if (Convert.ToDecimal(litTicketMedio.Text) > totTicketMedio)
                {
                    row.Cells[15].BackColor = corVerde;
                }
                else
                {
                    notaValor = notaValor - 1;
                    row.Cells[15].BackColor = corVermelho;
                }

                //NOTA
                Literal litNotaLetra = row.FindControl("litNotaLetra") as Literal;
                if (notaValor == 4)
                {
                    litNotaLetra.Text = "A";
                    row.Cells[16].BackColor = corVerde;
                }
                else if (notaValor == 3)
                {
                    litNotaLetra.Text = "B";
                    row.Cells[16].BackColor = corAmarelo;
                }
                else if (notaValor == 2)
                {
                    litNotaLetra.Text = "B-";
                    row.Cells[16].BackColor = corNeutra;
                }
                else
                {
                    litNotaLetra.Text = "C";
                    row.Cells[16].BackColor = corVermelho;
                }

                if (Convert.ToInt32(row.Cells[18].Text) > 0)
                    row.Cells[18].BackColor = corVermelho;

            }

        }

        #endregion


        private Color PintarLinhaTicket(Literal litTicketPORC, int qtdeTicket, int totalTicket)
        {
            Color cor = corNeutra;

            var ticketValPorc = Convert.ToDecimal(litTicketPORC.Text.Trim().Substring(0, litTicketPORC.Text.Trim().Length - 1));
            var ticketPorc = ((ticketValPorc * 1.00M) / 100);

            if (ticketPorc > ((qtdeTicket * 1.00M / totalTicket) * 1.05M))
                cor = corVermelho;
            if (ticketPorc < ((qtdeTicket * 1.00M / totalTicket) / 1.05M))
                cor = corVerde;
            if ((ticketPorc < ((qtdeTicket * 1.00M / totalTicket) * 1.05M)) && (ticketPorc > ((qtdeTicket * 1.00M / totalTicket) / 1.05M)))
                cor = corAmarelo;

            return cor;
        }
    }
}
