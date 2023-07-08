using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;

namespace Relatorios
{
    public partial class gerloja_visita_supervisor_cotas : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();
        DesempenhoController desempenhoController = new DesempenhoController();

        decimal gVendaValor = 0;
        decimal gCotaValor = 0;

        decimal totCotaAnt = 0;
        decimal totVendaAnt = 0;
        decimal totPremioAnt = 0;
        decimal totCotaAtu = 0;
        decimal totVendaAtu = 0;
        decimal totPremioAtu = 0;

        Color corVerde = Color.PaleGreen;
        Color corVermelho = Color.LightPink;
        Color corAmarelo = Color.PaleGoldenrod;
        Color corNeutra = Color.LightBlue;

        int ticket1 = 0;
        int ticketTotal = 0;
        int qtdeProduto = 0;
        int totalAtendido = 0;
        decimal valorPago = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                var usuario = ((USUARIO)Session["USUARIO"]);

                CarregarFilial(usuario);

                hidCodigoUsuario.Value = usuario.CODIGO_USUARIO.ToString();

                TransformarData(DateTime.Today.Year, DateTime.Today.Month);
                btProximo.Enabled = ValidarBTProximo(DateTime.Today.Year, DateTime.Today.Month);

                //CarregarCalendario(DateTime.Today.Year.ToString(), DateTime.Today.Month.ToString(), usuario.CODIGO_USUARIO);
            }

            btAnterior.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAnterior, null) + ";");
            btProximo.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btProximo, null) + ";");
            btAtualizar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial(USUARIO usuario)
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });

                if (usuario.CODIGO_PERFIL != 2 && usuario.CODIGO_PERFIL != 3)
                {
                    filiais.Add(new FILIAI { COD_FILIAL = "M", FILIAL = "LOJAS MAURILIO" });
                    filiais.Add(new FILIAI { COD_FILIAL = "D", FILIAL = "LOJAS DIRCEU" });
                }

                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }

            }
        }
        private void TransformarData(int ano, int mes)
        {
            DateTime ini = new DateTime(ano, mes, 1);
            DateTime fim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

            if (fim > DateTime.Now)
                fim = DateTime.Now.AddDays(-1);

            txtPeriodoInicial.Text = ini.ToString("dd/MM/yyyy");
            txtPeriodoFinal.Text = fim.ToString("dd/MM/yyyy");

            hidAno.Value = ini.Year.ToString();
            hidMes.Value = (ini.Month.ToString().Length == 1) ? ("0" + ini.Month.ToString()) : ini.Month.ToString();
        }
        private string CalcularCor(decimal valor)
        {
            var amarelo = "#FFFF00";//// Amarelo #FFFF00;
            var vermelho = "#FF4500";//// Vermelho #FF4500;
            var verde = "#98FB98";//// Verde #98FB98;
            var roxo = "#DA70D6";//// Roxo #BA55D3;

            if (valor <= 0)
                return "";

            if (valor > 90)
                return verde;
            else if (valor <= 90 && valor > 80)
                return amarelo;
            else if (valor <= 80 && valor > 70)
                return vermelho;
            else if (valor <= 70)
                return roxo;

            return "";
        }
        private bool ValidarBTProximo(int ano, int mes)
        {
            var ret = false;

            var mesAgora = DateTime.Today.Month;
            var anoAgora = DateTime.Today.Year;

            if (mesAgora > mes || anoAgora > ano)
                ret = true;

            return ret;
        }
        #endregion

        protected void btAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                var ano = Convert.ToInt32(hidAno.Value);
                var anoAux = Convert.ToInt32(hidAno.Value);
                var mes = Convert.ToInt32(hidMes.Value);
                mes = mes - 1;

                if (mes == 0)
                {
                    ano = ano - 1;
                    mes = 12;
                }

                hidAno.Value = ano.ToString();
                var mesAux = (mes.ToString().Length == 1) ? "0" + mes.ToString() : mes.ToString();
                hidMes.Value = mesAux;

                CarregarCalendario(ano.ToString(), mesAux, Convert.ToInt32(hidCodigoUsuario.Value));
                CarregarVendedores(ddlFilial.SelectedValue.Trim());

                if (anoAux != ano)
                    CarregarCalendarioAno(ano.ToString(), Convert.ToInt32(hidCodigoUsuario.Value));

            }
            catch (Exception)
            {
            }

        }
        protected void btProximo_Click(object sender, EventArgs e)
        {
            try
            {
                var ano = Convert.ToInt32(hidAno.Value);
                var anoAux = Convert.ToInt32(hidAno.Value);

                var mes = Convert.ToInt32(hidMes.Value);
                mes = mes + 1;

                if (mes == 13)
                {
                    ano = ano + 1;
                    mes = 1;
                }

                hidAno.Value = ano.ToString();
                var mesAux = (mes.ToString().Length == 1) ? "0" + mes.ToString() : mes.ToString();
                hidMes.Value = mesAux;

                CarregarCalendario(ano.ToString(), mesAux, Convert.ToInt32(hidCodigoUsuario.Value));
                CarregarVendedores(ddlFilial.SelectedValue.Trim());

                if (anoAux != ano)
                    CarregarCalendarioAno(ano.ToString(), Convert.ToInt32(hidCodigoUsuario.Value));

            }
            catch (Exception)
            {
            }
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            CarregarCalendario(hidAno.Value, hidMes.Value, Convert.ToInt32(hidCodigoUsuario.Value));
            CarregarCalendarioAno(hidAno.Value, Convert.ToInt32(hidCodigoUsuario.Value));
            CarregarVendedores(ddlFilial.SelectedValue.Trim());
        }

        #region "PRIMEIRO QUADRO"
        private void CarregarCalendario(string ano, string mes, int codigoUsuario)
        {

            char filialSelect = '1';
            if (ddlFilial.SelectedValue != "")
            {
                if (ddlFilial.SelectedValue == "M" || ddlFilial.SelectedValue == "D")
                    filialSelect = '3';
                else
                    filialSelect = '2';
            }

            gVendaValor = 0;
            gCotaValor = 0;

            TransformarData(Convert.ToInt32(ano), Convert.ToInt32(mes));
            btProximo.Enabled = ValidarBTProximo(Convert.ToInt32(ano), Convert.ToInt32(mes));

            var analiseCotas = lojaController.ObterCalendarioAnaliseCota(ano, mes, codigoUsuario, ddlFilial.SelectedValue, filialSelect);

            repAnaliseCotas.DataSource = analiseCotas;
            repAnaliseCotas.DataBind();

            labVendaValor.Text = "R$ " + gVendaValor.ToString("###,###,###,##0.00");
            labCotaValor.Text = "R$ " + gCotaValor.ToString("###,###,###,##0.00");
            labDiffValor.Text = "R$ " + (gVendaValor - gCotaValor).ToString("###,###,##0.00");
            labAtingidoPorc.Text = (gCotaValor <= 0) ? "0.00%" : ((gVendaValor / (gCotaValor * 1.00M)) * 100.00M).ToString("##0.00") + "%";

            labPeriodo.Text = ObterMes(Convert.ToInt32(mes), Convert.ToInt32(ano)).ToUpper();

        }
        protected void repAnaliseCotas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SP_OBTER_CALENDARIO_ANALISECOTAResult analiseCota = e.Item.DataItem as SP_OBTER_CALENDARIO_ANALISECOTAResult;

            if (analiseCota != null)
            {


                Table tbSegunda = e.Item.FindControl("tbSegunda") as Table;
                Table tbTerca = e.Item.FindControl("tbTerca") as Table;
                Table tbQuarta = e.Item.FindControl("tbQuarta") as Table;
                Table tbQuinta = e.Item.FindControl("tbQuinta") as Table;
                Table tbSexta = e.Item.FindControl("tbSexta") as Table;
                Table tbSabado = e.Item.FindControl("tbSabado") as Table;
                Table tbDomingo = e.Item.FindControl("tbDomingo") as Table;

                tbSegunda.Style.Add("background-color", CalcularCor(analiseCota.SEGUNDA_PORC));
                tbTerca.Style.Add("background-color", CalcularCor(analiseCota.TERCA_PORC));
                tbQuarta.Style.Add("background-color", CalcularCor(analiseCota.QUARTA_PORC));
                tbQuinta.Style.Add("background-color", CalcularCor(analiseCota.QUINTA_PORC));
                tbSexta.Style.Add("background-color", CalcularCor(analiseCota.SEXTA_PORC));
                tbSabado.Style.Add("background-color", CalcularCor(analiseCota.SABADO_PORC));
                tbDomingo.Style.Add("background-color", CalcularCor(analiseCota.DOMINGO_PORC));

                Label litSegundaDia = e.Item.FindControl("litSegundaDia") as Label;
                litSegundaDia.Text = (analiseCota.SEGUNDA_DIA == 0) ? "" : analiseCota.SEGUNDA_DIA.ToString();
                Literal litSegundaPorc = e.Item.FindControl("litSegundaPorc") as Literal;
                litSegundaPorc.Text = (analiseCota.SEGUNDA_PORC == 0) ? "" : Convert.ToDecimal(analiseCota.SEGUNDA_PORC).ToString("##0.00") + "%";
                Literal litSegundaValor = e.Item.FindControl("litSegundaValor") as Literal;
                litSegundaValor.Text = (analiseCota.SEGUNDA_VENDA == 0) ? "" : "R$ " + Convert.ToDecimal(analiseCota.SEGUNDA_VENDA).ToString("###,###,##0.00");

                Label litTercaDia = e.Item.FindControl("litTercaDia") as Label;
                litTercaDia.Text = (analiseCota.TERCA_DIA == 0) ? "" : analiseCota.TERCA_DIA.ToString();
                Literal litTercaPorc = e.Item.FindControl("litTercaPorc") as Literal;
                litTercaPorc.Text = (analiseCota.TERCA_PORC == 0) ? "" : Convert.ToDecimal(analiseCota.TERCA_PORC).ToString("##0.00") + "%";
                Literal litTercaValor = e.Item.FindControl("litTercaValor") as Literal;
                litTercaValor.Text = (analiseCota.TERCA_VENDA == 0) ? "" : "R$ " + Convert.ToDecimal(analiseCota.TERCA_VENDA).ToString("###,###,##0.00");

                Label litQuartaDia = e.Item.FindControl("litQuartaDia") as Label;
                litQuartaDia.Text = (analiseCota.QUARTA_DIA == 0) ? "" : analiseCota.QUARTA_DIA.ToString();
                Literal litQuartaPorc = e.Item.FindControl("litQuartaPorc") as Literal;
                litQuartaPorc.Text = (analiseCota.QUARTA_PORC == 0) ? "" : Convert.ToDecimal(analiseCota.QUARTA_PORC).ToString("##0.00") + "%";
                Literal litQuartaValor = e.Item.FindControl("litQuartaValor") as Literal;
                litQuartaValor.Text = (analiseCota.QUARTA_VENDA == 0) ? "" : "R$ " + Convert.ToDecimal(analiseCota.QUARTA_VENDA).ToString("###,###,##0.00");

                Label litQuintaDia = e.Item.FindControl("litQuintaDia") as Label;
                litQuintaDia.Text = (analiseCota.QUINTA_DIA == 0) ? "" : analiseCota.QUINTA_DIA.ToString();
                Literal litQuintaPorc = e.Item.FindControl("litQuintaPorc") as Literal;
                litQuintaPorc.Text = (analiseCota.QUINTA_PORC == 0) ? "" : Convert.ToDecimal(analiseCota.QUINTA_PORC).ToString("##0.00") + "%";
                Literal litQuintaValor = e.Item.FindControl("litQuintaValor") as Literal;
                litQuintaValor.Text = (analiseCota.QUINTA_VENDA == 0) ? "" : "R$ " + Convert.ToDecimal(analiseCota.QUINTA_VENDA).ToString("###,###,##0.00");


                Label litSextaDia = e.Item.FindControl("litSextaDia") as Label;
                litSextaDia.Text = (analiseCota.SEXTA_DIA == 0) ? "" : analiseCota.SEXTA_DIA.ToString();
                Literal litSextaPorc = e.Item.FindControl("litSextaPorc") as Literal;
                litSextaPorc.Text = (analiseCota.SEXTA_PORC == 0) ? "" : Convert.ToDecimal(analiseCota.SEXTA_PORC).ToString("##0.00") + "%";
                Literal litSextaValor = e.Item.FindControl("litSextaValor") as Literal;
                litSextaValor.Text = (analiseCota.SEXTA_VENDA == 0) ? "" : "R$ " + Convert.ToDecimal(analiseCota.SEXTA_VENDA).ToString("###,###,##0.00");


                Label litSabadoDia = e.Item.FindControl("litSabadoDia") as Label;
                litSabadoDia.Text = (analiseCota.SABADO_DIA == 0) ? "" : analiseCota.SABADO_DIA.ToString();
                Literal litSabadoPorc = e.Item.FindControl("litSabadoPorc") as Literal;
                litSabadoPorc.Text = (analiseCota.SABADO_PORC == 0) ? "" : Convert.ToDecimal(analiseCota.SABADO_PORC).ToString("##0.00") + "%";
                Literal litSabadoValor = e.Item.FindControl("litSabadoValor") as Literal;
                litSabadoValor.Text = (analiseCota.SABADO_VENDA == 0) ? "" : "R$ " + Convert.ToDecimal(analiseCota.SABADO_VENDA).ToString("###,###,##0.00");


                Label litDomingoDia = e.Item.FindControl("litDomingoDia") as Label;
                litDomingoDia.Text = (analiseCota.DOMINGO_DIA == 0) ? "" : analiseCota.DOMINGO_DIA.ToString();
                Literal litDomingoPorc = e.Item.FindControl("litDomingoPorc") as Literal;
                litDomingoPorc.Text = (analiseCota.DOMINGO_PORC == 0) ? "" : Convert.ToDecimal(analiseCota.DOMINGO_PORC).ToString("##0.00") + "%";
                Literal litDomingoValor = e.Item.FindControl("litDomingoValor") as Literal;
                litDomingoValor.Text = (analiseCota.DOMINGO_VENDA == 0) ? "" : "R$ " + Convert.ToDecimal(analiseCota.DOMINGO_VENDA).ToString("###,###,##0.00");

                gVendaValor += (analiseCota.SEGUNDA_VENDA + analiseCota.TERCA_VENDA + analiseCota.QUARTA_VENDA + analiseCota.QUINTA_VENDA + analiseCota.SEXTA_VENDA + analiseCota.SABADO_VENDA + analiseCota.DOMINGO_VENDA);
                gCotaValor += (analiseCota.SEGUNDA_COTA + analiseCota.TERCA_COTA + analiseCota.QUARTA_COTA + analiseCota.QUINTA_COTA + analiseCota.SEXTA_COTA + analiseCota.SABADO_COTA + analiseCota.DOMINGO_COTA);

            }
        }

        private void CarregarCalendarioAno(string ano, int codigoUsuario)
        {

            char filialSelect = '1';
            if (ddlFilial.SelectedValue != "")
            {
                if (ddlFilial.SelectedValue == "M" || ddlFilial.SelectedValue == "D")
                    filialSelect = '3';
                else
                    filialSelect = '2';
            }

            var analiseCotasAno = lojaController.ObterCalendarioAnaliseCotaAno(Convert.ToInt32(ano), codigoUsuario, ddlFilial.SelectedValue, filialSelect);

            gvCotaComp.DataSource = analiseCotasAno;
            gvCotaComp.DataBind();

            labAnoAnt.Text = analiseCotasAno.Select(p => p.ANO_ANT).Distinct().First().ToString();
            labAnoAtu.Text = analiseCotasAno.Select(p => p.ANO_ATU).First().ToString();

        }
        protected void gvCotaComp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CALENDARIO_ANALISECOTA_ANOResult cotaAno = e.Row.DataItem as SP_OBTER_CALENDARIO_ANALISECOTA_ANOResult;

                    Label labCotaValAnt = e.Row.FindControl("labCotaValAnt") as Label;
                    labCotaValAnt.Text = "R$ " + cotaAno.COTAVAL_ANT.ToString("###,###,###,##0.00");

                    Label labVendaValAnt = e.Row.FindControl("labVendaValAnt") as Label;
                    labVendaValAnt.Text = "R$ " + cotaAno.VENDAVAL_ANT.ToString("###,###,###,##0.00");

                    Label labAtingidoPorcAnt = e.Row.FindControl("labAtingidoPorcAnt") as Label;
                    labAtingidoPorcAnt.Text = cotaAno.ATINGIDOPORC_ANT.ToString("##0.00") + "%";

                    Label labCotaValAtual = e.Row.FindControl("labCotaValAtual") as Label;
                    labCotaValAtual.Text = "R$ " + cotaAno.COTAVAL_ATU.ToString("###,###,###,##0.00");

                    Label labVendaValAtual = e.Row.FindControl("labVendaValAtual") as Label;
                    labVendaValAtual.Text = "R$ " + cotaAno.VENDAVAL_ATU.ToString("###,###,###,##0.00");

                    Label labAtingidoPorcAtual = e.Row.FindControl("labAtingidoPorcAtual") as Label;
                    labAtingidoPorcAtual.Text = cotaAno.ATINGIDOPORC_ATU.ToString("##0.00") + "%";

                    totCotaAnt += cotaAno.COTAVAL_ANT;
                    totVendaAnt += cotaAno.VENDAVAL_ANT;
                    totPremioAnt += cotaAno.QTDE_PREMIO_ANT;

                    totCotaAtu += cotaAno.COTAVAL_ATU;
                    totVendaAtu += cotaAno.VENDAVAL_ATU;
                    totPremioAtu += cotaAno.QTDE_PREMIO_ATU;

                    //var ano = Convert.ToInt32(hidAno.Value);
                    //var mes = Convert.ToInt32(hidMes.Value);
                    //if (ano == cotaAno.ANO_ATU && mes == cotaAno.MES)
                    //    e.Row.BackColor = Color.Honeydew;

                }
            }
        }
        protected void gvCotaComp_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCotaComp.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[1].Text = "R$ " + totCotaAnt.ToString("###,###,###,##0.00");
                footer.Cells[2].Text = "R$ " + totVendaAnt.ToString("###,###,###,##0.00");

                var totAtinAnt = 0.00M;
                if (totCotaAnt > 0)
                    totAtinAnt = (totVendaAnt / totCotaAnt * 1.00M) * 100.00M;
                footer.Cells[3].Text = totAtinAnt.ToString("##0.00") + "%";

                footer.Cells[4].Text = totPremioAnt.ToString();

                footer.Cells[6].Text = "R$ " + totCotaAtu.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = "R$ " + totVendaAtu.ToString("###,###,###,##0.00");

                var totAtinAtu = 0.00M;
                if (totCotaAnt > 0)
                    totAtinAtu = (totVendaAtu / totCotaAtu * 1.00M) * 100.00M;
                footer.Cells[8].Text = totAtinAtu.ToString("##0.00") + "%";

                footer.Cells[9].Text = totPremioAtu.ToString();
            }
        }

        #endregion


        private void CarregarVendedores(string codigoFilial)
        {

            pnlFilial.Visible = false;

            if (codigoFilial != "" && codigoFilial != "M" && codigoFilial != "D")
            {
                DateTime dataIni = Convert.ToDateTime(txtPeriodoInicial.Text);
                DateTime dataFim = Convert.ToDateTime(txtPeriodoFinal.Text);

                CarregarDesempenhoVendedores(codigoFilial, dataIni, dataFim);

                ticket1 = 0;
                ticketTotal = 0;
                qtdeProduto = 0;
                totalAtendido = 0;
                valorPago = 0;

                var dataIniAnoAnt = dataIni.AddYears(-1);
                var dataFimAnoAnt = dataFim.AddYears(-1);
                CarregarDesempenhoVendedoresAnoAnterior(codigoFilial, dataIniAnoAnt, dataFimAnoAnt);

                ticket1 = 0;
                ticketTotal = 0;
                qtdeProduto = 0;
                totalAtendido = 0;
                valorPago = 0;

                var dataIniMesAnt = dataIni.AddMonths(-1);
                var dataFimMesAnt = dataIni.AddDays(-1);
                CarregarDesempenhoVendedoresMesAnterior(codigoFilial, dataIniMesAnt, dataFimMesAnt);

                pnlFilial.Visible = true;

            }
            else
            {
                gvDesempenhoVendedor.DataSource = new List<SP_OBTER_DESEMPENHO_VENDEDORResult>();
                gvDesempenhoVendedor.DataBind();

                gvDesempenhoVendedorAnoAnt.DataSource = new List<SP_OBTER_DESEMPENHO_VENDEDORResult>();
                gvDesempenhoVendedorAnoAnt.DataBind();

                gvDesempenhoVendedorMesAnterior.DataSource = new List<SP_OBTER_DESEMPENHO_VENDEDORResult>();
                gvDesempenhoVendedorMesAnterior.DataBind();

                labMesAnteriorVendedor.Text = "";
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
                Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                litValorPago.Text = "R$ " + Convert.ToDecimal(des.VALOR_PAGO).ToString("###,###,##0.00");

                Literal litPA = e.Row.FindControl("litPA") as Literal;
                litPA.Text = Convert.ToDecimal(des.PA).ToString("N2");

                Literal litTicketMedio = e.Row.FindControl("litTicketMedio") as Literal;
                litTicketMedio.Text = Convert.ToDecimal(des.TICKET_MEDIO).ToString("N2");

                Literal litTicket1PORC = e.Row.FindControl("litTicket1PORC") as Literal;
                litTicket1PORC.Text = Convert.ToDecimal(des.TICKET_1_PORC).ToString("N0") + "%";

                ticket1 += des.TICKET_1;
                ticketTotal += des.TICKET_TOTAL;
                qtdeProduto += des.QTDE_PRODUTO_TOTAL;
                totalAtendido += des.TICKET_TOTAL;
                valorPago += Convert.ToDecimal(des.VALOR_PAGO);

            }
        }
        protected void gvDesempenhoVendedor_DataBound(object sender, EventArgs e)
        {
            decimal totPA = 0;
            decimal totTicketMedio = 0;

            GridView gv = (GridView)sender;

            // FOOTER
            GridViewRow footer = gv.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[2].Text = "R$ " + valorPago.ToString("###,###,##0.00");

                footer.Cells[3].Text = totalAtendido.ToString();

                totPA = (ticketTotal <= 0) ? 0 : (qtdeProduto * 1.00M / ticketTotal);
                footer.Cells[4].Text = totPA.ToString("N2"); //PA

                totTicketMedio = (ticketTotal <= 0) ? 0 : (valorPago * 1.00M / ticketTotal);
                footer.Cells[5].Text = totTicketMedio.ToString("N2"); //TICKET MEDIO

                footer.Cells[6].Text = (ticketTotal <= 0) ? "0" : ((ticket1 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

            }

            //LOOP NAS LINHAS PARA PINTAR E CALCULAR A NOTA
            foreach (GridViewRow row in gv.Rows)
            {

                //TICKETS
                Literal litTicket1 = row.FindControl("litTicket1PORC") as Literal;
                if (litTicket1 != null)
                    row.Cells[6].BackColor = PintarLinhaTicket(litTicket1, ticket1, ticketTotal);

                //PA
                Literal litPA = row.FindControl("litPA") as Literal;
                if (litPA != null)
                {
                    if (Convert.ToDecimal(litPA.Text) > totPA)
                    {
                        row.Cells[4].BackColor = corVerde;
                    }
                    else
                    {
                        row.Cells[4].BackColor = corVermelho;
                    }
                }

                //TICKET MEDIO
                Literal litTicketMedio = row.FindControl("litTicketMedio") as Literal;
                if (litTicketMedio != null)
                {
                    if (Convert.ToDecimal(litTicketMedio.Text) > totTicketMedio)
                    {
                        row.Cells[5].BackColor = corVerde;
                    }
                    else
                    {
                        row.Cells[5].BackColor = corVermelho;
                    }
                }
            }

        }

        private void CarregarDesempenhoVendedoresAnoAnterior(string codigoFilial, DateTime dataIni, DateTime dataFim)
        {
            var desVendedores = desempenhoController.ObterDesempenhoVendedores(codigoFilial, dataIni, dataFim, "");

            ticket1 = desVendedores.Sum(p => p.TICKET_1);
            ticketTotal = desVendedores.Sum(p => p.TICKET_TOTAL);
            qtdeProduto = desVendedores.Sum(p => p.QTDE_PRODUTO_TOTAL);
            totalAtendido = ticketTotal;
            valorPago = desVendedores.Sum(p => p.VALOR_PAGO);

            var desVendedoresTot = new List<SP_OBTER_DESEMPENHO_VENDEDORResult>();
            desVendedoresTot.Add(new SP_OBTER_DESEMPENHO_VENDEDORResult
            {
                NOME_VENDEDOR = "Ano Anterior " + labAnoAnt.Text.Trim(),
                TICKET_1 = ticket1,
                TICKET_TOTAL = ticketTotal,
                QTDE_PRODUTO_TOTAL = qtdeProduto,
                VALOR_PAGO = valorPago
            });

            gvDesempenhoVendedorAnoAnt.DataSource = desVendedoresTot;
            gvDesempenhoVendedorAnoAnt.DataBind();
        }
        protected void gvDesempenhoVendedorAnoAnt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_DESEMPENHO_VENDEDORResult des = e.Row.DataItem as SP_OBTER_DESEMPENHO_VENDEDORResult;

            if (des != null)
            {

                Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                litValorPago.Text = "R$ " + Convert.ToDecimal(des.VALOR_PAGO).ToString("###,###,##0.00");

                decimal totPA = 0;
                totPA = (des.TICKET_TOTAL <= 0) ? 0 : (des.QTDE_PRODUTO_TOTAL * 1.00M / des.TICKET_TOTAL);
                Literal litPA = e.Row.FindControl("litPA") as Literal;
                litPA.Text = totPA.ToString("N2");

                decimal totTicketMedio = 0;
                totTicketMedio = (des.TICKET_TOTAL <= 0) ? 0 : (des.VALOR_PAGO * 1.00M / des.TICKET_TOTAL);
                Literal litTicketMedio = e.Row.FindControl("litTicketMedio") as Literal;
                litTicketMedio.Text = totTicketMedio.ToString("N2");

                decimal ticket1Porc = 0;
                ticket1Porc = (des.TICKET_TOTAL <= 0) ? 0 : ((des.TICKET_1 * 1.00M / des.TICKET_TOTAL) * 100.00M);
                Literal litTicket1PORC = e.Row.FindControl("litTicket1PORC") as Literal;
                litTicket1PORC.Text = ticket1Porc.ToString("N0") + "%";

            }
        }

        private void CarregarDesempenhoVendedoresMesAnterior(string codigoFilial, DateTime dataIni, DateTime dataFim)
        {
            var desVendedores = desempenhoController.ObterDesempenhoVendedores(codigoFilial, dataIni, dataFim, "");

            gvDesempenhoVendedorMesAnterior.DataSource = desVendedores;
            gvDesempenhoVendedorMesAnterior.DataBind();

            labMesAnteriorVendedor.Text = ObterMes(dataIni.Month, dataIni.Year);
        }
        protected void gvDesempenhoVendedorMesAnterior_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_DESEMPENHO_VENDEDORResult des = e.Row.DataItem as SP_OBTER_DESEMPENHO_VENDEDORResult;

            if (des != null)
            {
                Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                litValorPago.Text = "R$ " + Convert.ToDecimal(des.VALOR_PAGO).ToString("###,###,##0.00");

                Literal litPA = e.Row.FindControl("litPA") as Literal;
                litPA.Text = Convert.ToDecimal(des.PA).ToString("N2");

                Literal litTicketMedio = e.Row.FindControl("litTicketMedio") as Literal;
                litTicketMedio.Text = Convert.ToDecimal(des.TICKET_MEDIO).ToString("N2");

                Literal litTicket1PORC = e.Row.FindControl("litTicket1PORC") as Literal;
                litTicket1PORC.Text = Convert.ToDecimal(des.TICKET_1_PORC).ToString("N0") + "%";

                ticket1 += des.TICKET_1;
                ticketTotal += des.TICKET_TOTAL;
                qtdeProduto += des.QTDE_PRODUTO_TOTAL;
                totalAtendido += des.TICKET_TOTAL;
                valorPago += Convert.ToDecimal(des.VALOR_PAGO);

            }
        }
        protected void gvDesempenhoVendedorMesAnterior_DataBound(object sender, EventArgs e)
        {
            decimal totPA = 0;
            decimal totTicketMedio = 0;

            GridView gv = (GridView)sender;

            // FOOTER
            GridViewRow footer = gv.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[3].Text = "R$ " + valorPago.ToString("###,###,##0.00");

                footer.Cells[4].Text = totalAtendido.ToString();

                totPA = (ticketTotal <= 0) ? 0 : (qtdeProduto * 1.00M / ticketTotal);
                footer.Cells[5].Text = totPA.ToString("N2"); //PA

                totTicketMedio = (ticketTotal <= 0) ? 0 : (valorPago * 1.00M / ticketTotal);
                footer.Cells[6].Text = totTicketMedio.ToString("N2"); //TICKET MEDIO

                footer.Cells[7].Text = (ticketTotal <= 0) ? "0" : ((ticket1 * 1.00M / ticketTotal) * 100.00M).ToString("N0") + "%";

            }

            //LOOP NAS LINHAS PARA PINTAR E CALCULAR A NOTA
            foreach (GridViewRow row in gv.Rows)
            {

                //TICKETS
                Literal litTicket1 = row.FindControl("litTicket1PORC") as Literal;
                if (litTicket1 != null)
                    row.Cells[7].BackColor = PintarLinhaTicket(litTicket1, ticket1, ticketTotal);

                //PA
                Literal litPA = row.FindControl("litPA") as Literal;
                if (litPA != null)
                {
                    if (Convert.ToDecimal(litPA.Text) > totPA)
                    {
                        row.Cells[5].BackColor = corVerde;
                    }
                    else
                    {
                        row.Cells[5].BackColor = corVermelho;
                    }
                }

                //TICKET MEDIO
                Literal litTicketMedio = row.FindControl("litTicketMedio") as Literal;
                if (litTicketMedio != null)
                {
                    if (Convert.ToDecimal(litTicketMedio.Text) > totTicketMedio)
                    {
                        row.Cells[6].BackColor = corVerde;
                    }
                    else
                    {
                        row.Cells[6].BackColor = corVermelho;
                    }
                }
            }

        }


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

        private string ObterMes(int mes, int ano)
        {
            var mesAux = "";

            switch (mes)
            {
                case 1:
                    mesAux = "Janeiro";
                    break;
                case 2:
                    mesAux = "Fevereiro";
                    break;
                case 3:
                    mesAux = "Março";
                    break;
                case 4:
                    mesAux = "Abril";
                    break;
                case 5:
                    mesAux = "Maio";
                    break;
                case 6:
                    mesAux = "Junho";
                    break;
                case 7:
                    mesAux = "Julho";
                    break;
                case 8:
                    mesAux = "Agosto";
                    break;
                case 9:
                    mesAux = "Setembro";
                    break;
                case 10:
                    mesAux = "Outubro";
                    break;
                case 11:
                    mesAux = "Novembro";
                    break;
                case 12:
                    mesAux = "Dezembro";
                    break;
            }

            return (mesAux + " de " + ano.ToString());
        }

        #endregion

        protected void btAbrir60Mais_Click(object sender, EventArgs e)
        {
            Response.Redirect(("gerloja_mais_vendidos.aspx?t=1"), "_blank", "");
        }

        protected void btGriffePorc_Click(object sender, EventArgs e)
        {
            Response.Redirect(("gerloja_griffe_porc.aspx?t=1"), "_blank", "");
        }

    }
}
