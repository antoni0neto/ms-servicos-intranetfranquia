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
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class desenv_controle_producao_calen : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        int contSkuRisco = 0;
        int contQtdeRisco = 0;
        int contSkuCorte = 0;
        int contQtdeCorte = 0;
        int contSkuFaccao = 0;
        int contQtdeFaccao = 0;
        int contSkuAcab = 0;
        int contQtdeAcab = 0;

        int somaSkuRisco = 0;
        int somaQtdeRisco = 0;
        int somaSkuCorte = 0;
        int somaQtdeCorte = 0;
        int somaSkuFaccao = 0;
        int somaQtdeFaccao = 0;
        int somaSkuAcab = 0;
        int somaQtdeAcab = 0;

        decimal mediaSkuRisco = 0;
        decimal mediaQtdeRisco = 0;
        decimal mediaSkuCorte = 0;
        decimal mediaQtdeCorte = 0;
        decimal mediaSkuFaccao = 0;
        decimal mediaQtdeFaccao = 0;
        decimal mediaSkuAcab = 0;
        decimal mediaQtdeAcab = 0;

        int totSku = 0;
        int totSkuVarejo = 0;
        int totSkuAtacado = 0;

        int totQtdeVarejo = 0;
        int totQtdeAtacado = 0;
        int totQtdeTotal = 0;

        int faltaCorte = 0;
        int faltaFaccao = 0;
        int faltaTecido = 0;

        bool mesAnterior = false;
        decimal mediaFaltaCorte = 0;
        decimal qtdeSemanaCorte = 0;

        decimal mediaFaltaFaccao = 0;
        decimal qtdeSemanaFaccao = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "../mod_faccao/facc_menu.aspx";

                var usuario = ((USUARIO)Session["USUARIO"]);

                var ano = DateTime.Now.Year.ToString();
                var mes = DateTime.Now.Month.ToString();
                mes = (mes.Length == 1) ? "0" + mes : mes;
                var codigoUsuario = usuario.CODIGO_USUARIO;

                hidAno.Value = ano;
                hidMes.Value = mes;
                CarregarColecoes();

                hidCodigoUsuario.Value = codigoUsuario.ToString();
                //CarregarCalendario(ano, mes);

            }

            btAtualizar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizar, null) + ";");
            btProximo.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btProximo, null) + ";");
            btAnterior.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAnterior, null) + ";");
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();

                colecoes.RemoveAt(0);
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecaoPlan.DataSource = colecoes;
                ddlColecaoPlan.DataBind();
            }
        }
        private void LimparVariaveis()
        {
            contSkuRisco = 0;
            contQtdeRisco = 0;
            contSkuCorte = 0;
            contQtdeCorte = 0;
            contSkuFaccao = 0;
            contQtdeFaccao = 0;
            contSkuAcab = 0;
            contQtdeAcab = 0;

            somaSkuRisco = 0;
            somaQtdeRisco = 0;
            somaSkuCorte = 0;
            somaQtdeCorte = 0;
            somaSkuFaccao = 0;
            somaQtdeFaccao = 0;
            somaSkuAcab = 0;
            somaQtdeAcab = 0;

            mediaSkuRisco = 0;
            mediaQtdeRisco = 0;
            mediaSkuCorte = 0;
            mediaQtdeCorte = 0;
            mediaSkuFaccao = 0;
            mediaQtdeFaccao = 0;
            mediaSkuAcab = 0;
            mediaQtdeAcab = 0;
        }
        #endregion

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
        private void CarregarCalendario(string ano, string mes)
        {
            mesAnterior = false;

            /*CALENDARIO LADO ESQUERDO - MES ATUAL*/
            labPeriodo1.Text = ObterMes(Convert.ToInt32(mes), Convert.ToInt32(ano)).ToUpper();
            var calendarioProducao1 = desenvController.ObterCalendarioControleProducao(ano, mes, ddlColecao.SelectedValue.Trim());
            repProducao1.DataSource = calendarioProducao1;
            repProducao1.DataBind();
            /*FIM*/

            mesAnterior = true;

            /*CALENDARIO LADO DIREITO - MES ANTERIOR*/
            LimparVariaveis();
            var iniMesAnterior = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), 1);
            iniMesAnterior = iniMesAnterior.AddMonths(-1);

            labPeriodo2.Text = ObterMes(iniMesAnterior.Month, iniMesAnterior.Year).ToUpper();
            var calendarioProducao2 = desenvController.ObterCalendarioControleProducao(iniMesAnterior.Year.ToString(), iniMesAnterior.Month.ToString(), ddlColecao.SelectedValue.Trim());
            repProducao2.DataSource = calendarioProducao2;
            repProducao2.DataBind();
            /*FIM*/

            mesAnterior = false;
        }

        protected void repProducao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SP_OBTER_CALENDARIO_FACCAOResult agenda = e.Item.DataItem as SP_OBTER_CALENDARIO_FACCAOResult;

            if (agenda != null)
            {
                Label litSegundaDia = e.Item.FindControl("litSegundaDia") as Label;
                litSegundaDia.Text = (agenda.SEGUNDA_DIA == 0) ? "" : agenda.SEGUNDA_DIA.ToString();
                Literal litSegundaRiscoSKU = e.Item.FindControl("litSegundaRiscoSKU") as Literal;
                litSegundaRiscoSKU.Text = Convert.ToDecimal(agenda.SEGUNDA_RISCO_SKU).ToString("###,###,##0");
                Literal litSegundaRiscoQTDE = e.Item.FindControl("litSegundaRiscoQTDE") as Literal;
                litSegundaRiscoQTDE.Text = Convert.ToDecimal(agenda.SEGUNDA_RISCO_QTDE).ToString("###,###,##0");
                Literal litSegundaCorteSKU = e.Item.FindControl("litSegundaCorteSKU") as Literal;
                litSegundaCorteSKU.Text = Convert.ToDecimal(agenda.SEGUNDA_CORTE_SKU).ToString("###,###,##0");
                Literal litSegundaCorteQTDE = e.Item.FindControl("litSegundaCorteQTDE") as Literal;
                litSegundaCorteQTDE.Text = Convert.ToDecimal(agenda.SEGUNDA_CORTE_QTDE).ToString("###,###,##0");
                Literal litSegundaFaccaoSKU = e.Item.FindControl("litSegundaFaccaoSKU") as Literal;
                litSegundaFaccaoSKU.Text = Convert.ToDecimal(agenda.SEGUNDA_FACCAO_SKU).ToString("###,###,##0");
                Literal litSegundaFaccaoQTDE = e.Item.FindControl("litSegundaFaccaoQTDE") as Literal;
                litSegundaFaccaoQTDE.Text = Convert.ToDecimal(agenda.SEGUNDA_FACCAO_QTDE).ToString("###,###,##0");
                Literal litSegundaAcabSKU = e.Item.FindControl("litSegundaAcabSKU") as Literal;
                litSegundaAcabSKU.Text = Convert.ToDecimal(agenda.SEGUNDA_ACAB_SKU).ToString("###,###,##0");
                Literal litSegundaAcabQTDE = e.Item.FindControl("litSegundaAcabQTDE") as Literal;
                litSegundaAcabQTDE.Text = Convert.ToDecimal(agenda.SEGUNDA_ACAB_QTDE).ToString("###,###,##0");

                Label litTercaDia = e.Item.FindControl("litTercaDia") as Label;
                litTercaDia.Text = (agenda.TERCA_DIA == 0) ? "" : agenda.TERCA_DIA.ToString();
                Literal litTercaRiscoSKU = e.Item.FindControl("litTercaRiscoSKU") as Literal;
                litTercaRiscoSKU.Text = Convert.ToDecimal(agenda.TERCA_RISCO_SKU).ToString("###,###,##0");
                Literal litTercaRiscoQTDE = e.Item.FindControl("litTercaRiscoQTDE") as Literal;
                litTercaRiscoQTDE.Text = Convert.ToDecimal(agenda.TERCA_RISCO_QTDE).ToString("###,###,##0");
                Literal litTercaCorteSKU = e.Item.FindControl("litTercaCorteSKU") as Literal;
                litTercaCorteSKU.Text = Convert.ToDecimal(agenda.TERCA_CORTE_SKU).ToString("###,###,##0");
                Literal litTercaCorteQTDE = e.Item.FindControl("litTercaCorteQTDE") as Literal;
                litTercaCorteQTDE.Text = Convert.ToDecimal(agenda.TERCA_CORTE_QTDE).ToString("###,###,##0");
                Literal litTercaFaccaoSKU = e.Item.FindControl("litTercaFaccaoSKU") as Literal;
                litTercaFaccaoSKU.Text = Convert.ToDecimal(agenda.TERCA_FACCAO_SKU).ToString("###,###,##0");
                Literal litTercaFaccaoQTDE = e.Item.FindControl("litTercaFaccaoQTDE") as Literal;
                litTercaFaccaoQTDE.Text = Convert.ToDecimal(agenda.TERCA_FACCAO_QTDE).ToString("###,###,##0");
                Literal litTercaAcabSKU = e.Item.FindControl("litTercaAcabSKU") as Literal;
                litTercaAcabSKU.Text = Convert.ToDecimal(agenda.TERCA_ACAB_SKU).ToString("###,###,##0");
                Literal litTercaAcabQTDE = e.Item.FindControl("litTercaAcabQTDE") as Literal;
                litTercaAcabQTDE.Text = Convert.ToDecimal(agenda.TERCA_ACAB_QTDE).ToString("###,###,##0");

                Label litQuartaDia = e.Item.FindControl("litQuartaDia") as Label;
                litQuartaDia.Text = (agenda.QUARTA_DIA == 0) ? "" : agenda.QUARTA_DIA.ToString();
                Literal litQuartaRiscoSKU = e.Item.FindControl("litQuartaRiscoSKU") as Literal;
                litQuartaRiscoSKU.Text = Convert.ToDecimal(agenda.QUARTA_RISCO_SKU).ToString("###,###,##0");
                Literal litQuartaRiscoQTDE = e.Item.FindControl("litQuartaRiscoQTDE") as Literal;
                litQuartaRiscoQTDE.Text = Convert.ToDecimal(agenda.QUARTA_RISCO_QTDE).ToString("###,###,##0");
                Literal litQuartaCorteSKU = e.Item.FindControl("litQuartaCorteSKU") as Literal;
                litQuartaCorteSKU.Text = Convert.ToDecimal(agenda.QUARTA_CORTE_SKU).ToString("###,###,##0");
                Literal litQuartaCorteQTDE = e.Item.FindControl("litQuartaCorteQTDE") as Literal;
                litQuartaCorteQTDE.Text = Convert.ToDecimal(agenda.QUARTA_CORTE_QTDE).ToString("###,###,##0");
                Literal litQuartaFaccaoSKU = e.Item.FindControl("litQuartaFaccaoSKU") as Literal;
                litQuartaFaccaoSKU.Text = Convert.ToDecimal(agenda.QUARTA_FACCAO_SKU).ToString("###,###,##0");
                Literal litQuartaFaccaoQTDE = e.Item.FindControl("litQuartaFaccaoQTDE") as Literal;
                litQuartaFaccaoQTDE.Text = Convert.ToDecimal(agenda.QUARTA_FACCAO_QTDE).ToString("###,###,##0");
                Literal litQuartaAcabSKU = e.Item.FindControl("litQuartaAcabSKU") as Literal;
                litQuartaAcabSKU.Text = Convert.ToDecimal(agenda.QUARTA_ACAB_SKU).ToString("###,###,##0");
                Literal litQuartaAcabQTDE = e.Item.FindControl("litQuartaAcabQTDE") as Literal;
                litQuartaAcabQTDE.Text = Convert.ToDecimal(agenda.QUARTA_ACAB_QTDE).ToString("###,###,##0");

                Label litQuintaDia = e.Item.FindControl("litQuintaDia") as Label;
                litQuintaDia.Text = (agenda.QUINTA_DIA == 0) ? "" : agenda.QUINTA_DIA.ToString();
                Literal litQuintaRiscoSKU = e.Item.FindControl("litQuintaRiscoSKU") as Literal;
                litQuintaRiscoSKU.Text = Convert.ToDecimal(agenda.QUINTA_RISCO_SKU).ToString("###,###,##0");
                Literal litQuintaRiscoQTDE = e.Item.FindControl("litQuintaRiscoQTDE") as Literal;
                litQuintaRiscoQTDE.Text = Convert.ToDecimal(agenda.QUINTA_RISCO_QTDE).ToString("###,###,##0");
                Literal litQuintaCorteSKU = e.Item.FindControl("litQuintaCorteSKU") as Literal;
                litQuintaCorteSKU.Text = Convert.ToDecimal(agenda.QUINTA_CORTE_SKU).ToString("###,###,##0");
                Literal litQuintaCorteQTDE = e.Item.FindControl("litQuintaCorteQTDE") as Literal;
                litQuintaCorteQTDE.Text = Convert.ToDecimal(agenda.QUINTA_CORTE_QTDE).ToString("###,###,##0");
                Literal litQuintaFaccaoSKU = e.Item.FindControl("litQuintaFaccaoSKU") as Literal;
                litQuintaFaccaoSKU.Text = Convert.ToDecimal(agenda.QUINTA_FACCAO_SKU).ToString("###,###,##0");
                Literal litQuintaFaccaoQTDE = e.Item.FindControl("litQuintaFaccaoQTDE") as Literal;
                litQuintaFaccaoQTDE.Text = Convert.ToDecimal(agenda.QUINTA_FACCAO_QTDE).ToString("###,###,##0");
                Literal litQuintaAcabSKU = e.Item.FindControl("litQuintaAcabSKU") as Literal;
                litQuintaAcabSKU.Text = Convert.ToDecimal(agenda.QUINTA_ACAB_SKU).ToString("###,###,##0");
                Literal litQuintaAcabQTDE = e.Item.FindControl("litQuintaAcabQTDE") as Literal;
                litQuintaAcabQTDE.Text = Convert.ToDecimal(agenda.QUINTA_ACAB_QTDE).ToString("###,###,##0");

                Label litSextaDia = e.Item.FindControl("litSextaDia") as Label;
                litSextaDia.Text = (agenda.SEXTA_DIA == 0) ? "" : agenda.SEXTA_DIA.ToString();
                Literal litSextaRiscoSKU = e.Item.FindControl("litSextaRiscoSKU") as Literal;
                litSextaRiscoSKU.Text = Convert.ToDecimal(agenda.SEXTA_RISCO_SKU).ToString("###,###,##0");
                Literal litSextaRiscoQTDE = e.Item.FindControl("litSextaRiscoQTDE") as Literal;
                litSextaRiscoQTDE.Text = Convert.ToDecimal(agenda.SEXTA_RISCO_QTDE).ToString("###,###,##0");
                Literal litSextaCorteSKU = e.Item.FindControl("litSextaCorteSKU") as Literal;
                litSextaCorteSKU.Text = Convert.ToDecimal(agenda.SEXTA_CORTE_SKU).ToString("###,###,##0");
                Literal litSextaCorteQTDE = e.Item.FindControl("litSextaCorteQTDE") as Literal;
                litSextaCorteQTDE.Text = Convert.ToDecimal(agenda.SEXTA_CORTE_QTDE).ToString("###,###,##0");
                Literal litSextaFaccaoSKU = e.Item.FindControl("litSextaFaccaoSKU") as Literal;
                litSextaFaccaoSKU.Text = Convert.ToDecimal(agenda.SEXTA_FACCAO_SKU).ToString("###,###,##0");
                Literal litSextaFaccaoQTDE = e.Item.FindControl("litSextaFaccaoQTDE") as Literal;
                litSextaFaccaoQTDE.Text = Convert.ToDecimal(agenda.SEXTA_FACCAO_QTDE).ToString("###,###,##0");
                Literal litSextaAcabSKU = e.Item.FindControl("litSextaAcabSKU") as Literal;
                litSextaAcabSKU.Text = Convert.ToDecimal(agenda.SEXTA_ACAB_SKU).ToString("###,###,##0");
                Literal litSextaAcabQTDE = e.Item.FindControl("litSextaAcabQTDE") as Literal;
                litSextaAcabQTDE.Text = Convert.ToDecimal(agenda.SEXTA_ACAB_QTDE).ToString("###,###,##0");

                Label litSabadoDia = e.Item.FindControl("litSabadoDia") as Label;
                litSabadoDia.Text = (agenda.SABADO_DIA == 0) ? "" : agenda.SABADO_DIA.ToString();
                Literal litSabadoRiscoSKU = e.Item.FindControl("litSabadoRiscoSKU") as Literal;
                litSabadoRiscoSKU.Text = Convert.ToDecimal(agenda.SABADO_RISCO_SKU).ToString("###,###,##0");
                Literal litSabadoRiscoQTDE = e.Item.FindControl("litSabadoRiscoQTDE") as Literal;
                litSabadoRiscoQTDE.Text = Convert.ToDecimal(agenda.SABADO_RISCO_QTDE).ToString("###,###,##0");
                Literal litSabadoCorteSKU = e.Item.FindControl("litSabadoCorteSKU") as Literal;
                litSabadoCorteSKU.Text = Convert.ToDecimal(agenda.SABADO_CORTE_SKU).ToString("###,###,##0");
                Literal litSabadoCorteQTDE = e.Item.FindControl("litSabadoCorteQTDE") as Literal;
                litSabadoCorteQTDE.Text = Convert.ToDecimal(agenda.SABADO_CORTE_QTDE).ToString("###,###,##0");
                Literal litSabadoFaccaoSKU = e.Item.FindControl("litSabadoFaccaoSKU") as Literal;
                litSabadoFaccaoSKU.Text = Convert.ToDecimal(agenda.SABADO_FACCAO_SKU).ToString("###,###,##0");
                Literal litSabadoFaccaoQTDE = e.Item.FindControl("litSabadoFaccaoQTDE") as Literal;
                litSabadoFaccaoQTDE.Text = Convert.ToDecimal(agenda.SABADO_FACCAO_QTDE).ToString("###,###,##0");
                Literal litSabadoAcabSKU = e.Item.FindControl("litSabadoAcabSKU") as Literal;
                litSabadoAcabSKU.Text = Convert.ToDecimal(agenda.SABADO_ACAB_SKU).ToString("###,###,##0");
                Literal litSabadoAcabQTDE = e.Item.FindControl("litSabadoAcabQTDE") as Literal;
                litSabadoAcabQTDE.Text = Convert.ToDecimal(agenda.SABADO_ACAB_QTDE).ToString("###,###,##0");

                Label litDomingoDia = e.Item.FindControl("litDomingoDia") as Label;
                litDomingoDia.Text = (agenda.DOMINGO_DIA == 0) ? "" : agenda.DOMINGO_DIA.ToString();
                Literal litDomingoRiscoSKU = e.Item.FindControl("litDomingoRiscoSKU") as Literal;
                litDomingoRiscoSKU.Text = Convert.ToDecimal(agenda.DOMINGO_RISCO_SKU).ToString("###,###,##0");
                Literal litDomingoRiscoQTDE = e.Item.FindControl("litDomingoRiscoQTDE") as Literal;
                litDomingoRiscoQTDE.Text = Convert.ToDecimal(agenda.DOMINGO_RISCO_QTDE).ToString("###,###,##0");
                Literal litDomingoCorteSKU = e.Item.FindControl("litDomingoCorteSKU") as Literal;
                litDomingoCorteSKU.Text = Convert.ToDecimal(agenda.DOMINGO_CORTE_SKU).ToString("###,###,##0");
                Literal litDomingoCorteQTDE = e.Item.FindControl("litDomingoCorteQTDE") as Literal;
                litDomingoCorteQTDE.Text = Convert.ToDecimal(agenda.DOMINGO_CORTE_QTDE).ToString("###,###,##0");
                Literal litDomingoFaccaoSKU = e.Item.FindControl("litDomingoFaccaoSKU") as Literal;
                litDomingoFaccaoSKU.Text = Convert.ToDecimal(agenda.DOMINGO_FACCAO_SKU).ToString("###,###,##0");
                Literal litDomingoFaccaoQTDE = e.Item.FindControl("litDomingoFaccaoQTDE") as Literal;
                litDomingoFaccaoQTDE.Text = Convert.ToDecimal(agenda.DOMINGO_FACCAO_QTDE).ToString("###,###,##0");
                Literal litDomingoAcabSKU = e.Item.FindControl("litDomingoAcabSKU") as Literal;
                litDomingoAcabSKU.Text = Convert.ToDecimal(agenda.DOMINGO_ACAB_SKU).ToString("###,###,##0");
                Literal litDomingoAcabQTDE = e.Item.FindControl("litDomingoAcabQTDE") as Literal;
                litDomingoAcabQTDE.Text = Convert.ToDecimal(agenda.DOMINGO_ACAB_QTDE).ToString("###,###,##0");

                Label labRiscoSKUSoma = e.Item.FindControl("labRiscoSKUSoma") as Label;
                somaSkuRisco = agenda.SEGUNDA_RISCO_SKU + agenda.TERCA_RISCO_SKU + agenda.QUARTA_RISCO_SKU + agenda.QUINTA_RISCO_SKU + agenda.SEXTA_RISCO_SKU + agenda.SABADO_RISCO_SKU + agenda.DOMINGO_RISCO_SKU;
                labRiscoSKUSoma.Text = Convert.ToDecimal(somaSkuRisco).ToString("###,###,##0");
                Label labRiscoQTDESoma = e.Item.FindControl("labRiscoQTDESoma") as Label;
                somaQtdeRisco = agenda.SEGUNDA_RISCO_QTDE + agenda.TERCA_RISCO_QTDE + agenda.QUARTA_RISCO_QTDE + agenda.QUINTA_RISCO_QTDE + agenda.SEXTA_RISCO_QTDE + agenda.SABADO_RISCO_QTDE + agenda.DOMINGO_RISCO_QTDE;
                labRiscoQTDESoma.Text = Convert.ToDecimal(somaQtdeRisco).ToString("###,###,##0");
                Label labCorteSKUSoma = e.Item.FindControl("labCorteSKUSoma") as Label;
                somaSkuCorte = agenda.SEGUNDA_CORTE_SKU + agenda.TERCA_CORTE_SKU + agenda.QUARTA_CORTE_SKU + agenda.QUINTA_CORTE_SKU + agenda.SEXTA_CORTE_SKU + agenda.SABADO_CORTE_SKU + agenda.DOMINGO_CORTE_SKU;
                labCorteSKUSoma.Text = Convert.ToDecimal(somaSkuCorte).ToString("###,###,##0");
                Label labCorteQTDESoma = e.Item.FindControl("labCorteQTDESoma") as Label;
                somaQtdeCorte = agenda.SEGUNDA_CORTE_QTDE + agenda.TERCA_CORTE_QTDE + agenda.QUARTA_CORTE_QTDE + agenda.QUINTA_CORTE_QTDE + agenda.SEXTA_CORTE_QTDE + agenda.SABADO_CORTE_QTDE + agenda.DOMINGO_CORTE_QTDE;
                labCorteQTDESoma.Text = Convert.ToDecimal(somaQtdeCorte).ToString("###,###,##0");
                Label labFaccaoSKUSoma = e.Item.FindControl("labFaccaoSKUSoma") as Label;
                somaSkuFaccao = agenda.SEGUNDA_FACCAO_SKU + agenda.TERCA_FACCAO_SKU + agenda.QUARTA_FACCAO_SKU + agenda.QUINTA_FACCAO_SKU + agenda.SEXTA_FACCAO_SKU + agenda.SABADO_FACCAO_SKU + agenda.DOMINGO_FACCAO_SKU;
                labFaccaoSKUSoma.Text = Convert.ToDecimal(somaSkuFaccao).ToString("###,###,##0");
                Label labFaccaoQTDESoma = e.Item.FindControl("labFaccaoQTDESoma") as Label;
                somaQtdeFaccao = agenda.SEGUNDA_FACCAO_QTDE + agenda.TERCA_FACCAO_QTDE + agenda.QUARTA_FACCAO_QTDE + agenda.QUINTA_FACCAO_QTDE + agenda.SEXTA_FACCAO_QTDE + agenda.SABADO_FACCAO_QTDE + agenda.DOMINGO_FACCAO_QTDE;
                labFaccaoQTDESoma.Text = Convert.ToDecimal(somaQtdeFaccao).ToString("###,###,##0");
                Label labAcabSKUSoma = e.Item.FindControl("labAcabSKUSoma") as Label;
                somaSkuAcab = agenda.SEGUNDA_ACAB_SKU + agenda.TERCA_ACAB_SKU + agenda.QUARTA_ACAB_SKU + agenda.QUINTA_ACAB_SKU + agenda.SEXTA_ACAB_SKU + agenda.SABADO_ACAB_SKU + agenda.DOMINGO_ACAB_SKU;
                labAcabSKUSoma.Text = Convert.ToDecimal(somaSkuAcab).ToString("###,###,##0");
                Label labAcabQTDESoma = e.Item.FindControl("labAcabQTDESoma") as Label;
                somaQtdeAcab = agenda.SEGUNDA_ACAB_QTDE + agenda.TERCA_ACAB_QTDE + agenda.QUARTA_ACAB_QTDE + agenda.QUINTA_ACAB_QTDE + agenda.SEXTA_ACAB_QTDE + agenda.SABADO_ACAB_QTDE + agenda.DOMINGO_ACAB_QTDE;
                labAcabQTDESoma.Text = Convert.ToDecimal(somaQtdeAcab).ToString("###,###,##0");

                Label labRiscoSKUMedia = e.Item.FindControl("labRiscoSKUMedia") as Label;
                contSkuRisco = ((agenda.SEGUNDA_RISCO_SKU > 0) ? 1 : 0) + ((agenda.TERCA_RISCO_SKU > 0) ? 1 : 0) + ((agenda.QUARTA_RISCO_SKU > 0) ? 1 : 0) + ((agenda.QUINTA_RISCO_SKU > 0) ? 1 : 0) + ((agenda.SEXTA_RISCO_SKU > 0) ? 1 : 0) + ((agenda.SABADO_RISCO_SKU > 0) ? 1 : 0) + ((agenda.DOMINGO_RISCO_SKU > 0) ? 1 : 0);
                mediaSkuRisco = (contSkuRisco <= 0) ? 0.00M : Convert.ToDecimal((somaSkuRisco / (contSkuRisco * 1.00M)));
                labRiscoSKUMedia.Text = Convert.ToDecimal(mediaSkuRisco).ToString("###,###,##0");

                Label labRiscoQTDEMedia = e.Item.FindControl("labRiscoQTDEMedia") as Label;
                contQtdeRisco = ((agenda.SEGUNDA_RISCO_QTDE > 0) ? 1 : 0) + ((agenda.TERCA_RISCO_QTDE > 0) ? 1 : 0) + ((agenda.QUARTA_RISCO_QTDE > 0) ? 1 : 0) + ((agenda.QUINTA_RISCO_QTDE > 0) ? 1 : 0) + ((agenda.SEXTA_RISCO_QTDE > 0) ? 1 : 0) + ((agenda.SABADO_RISCO_QTDE > 0) ? 1 : 0) + ((agenda.DOMINGO_RISCO_QTDE > 0) ? 1 : 0);
                mediaQtdeRisco = (contQtdeRisco <= 0) ? 0.00M : Convert.ToDecimal((somaQtdeRisco / (contQtdeRisco * 1.00M)));
                labRiscoQTDEMedia.Text = Convert.ToDecimal(mediaQtdeRisco).ToString("###,###,##0");

                Label labCorteSKUMedia = e.Item.FindControl("labCorteSKUMedia") as Label;
                contSkuCorte = ((agenda.SEGUNDA_CORTE_SKU > 0) ? 1 : 0) + ((agenda.TERCA_CORTE_SKU > 0) ? 1 : 0) + ((agenda.QUARTA_CORTE_SKU > 0) ? 1 : 0) + ((agenda.QUINTA_CORTE_SKU > 0) ? 1 : 0) + ((agenda.SEXTA_CORTE_SKU > 0) ? 1 : 0) + ((agenda.SABADO_CORTE_SKU > 0) ? 1 : 0) + ((agenda.DOMINGO_CORTE_SKU > 0) ? 1 : 0);
                mediaSkuCorte = (contSkuCorte <= 0) ? 0.00M : Convert.ToDecimal((somaSkuCorte / (contSkuCorte * 1.00M)));
                labCorteSKUMedia.Text = Convert.ToDecimal(mediaSkuCorte).ToString("###,###,##0");

                Label labCorteQTDEMedia = e.Item.FindControl("labCorteQTDEMedia") as Label;
                contQtdeCorte = ((agenda.SEGUNDA_CORTE_QTDE > 0) ? 1 : 0) + ((agenda.TERCA_CORTE_QTDE > 0) ? 1 : 0) + ((agenda.QUARTA_CORTE_QTDE > 0) ? 1 : 0) + ((agenda.QUINTA_CORTE_QTDE > 0) ? 1 : 0) + ((agenda.SEXTA_CORTE_QTDE > 0) ? 1 : 0) + ((agenda.SABADO_CORTE_QTDE > 0) ? 1 : 0) + ((agenda.DOMINGO_CORTE_QTDE > 0) ? 1 : 0);
                mediaQtdeCorte = (contQtdeCorte <= 0) ? 0.00M : Convert.ToDecimal((somaQtdeCorte / (contQtdeCorte * 1.00M)));
                labCorteQTDEMedia.Text = Convert.ToDecimal(mediaQtdeCorte).ToString("###,###,##0");
                if (mesAnterior)
                {
                    mediaFaltaCorte += mediaQtdeCorte;
                    qtdeSemanaCorte = qtdeSemanaCorte + ((mediaQtdeCorte > 0) ? 1 : 0);
                }

                Label labFaccaoSKUMedia = e.Item.FindControl("labFaccaoSKUMedia") as Label;
                contSkuFaccao = ((agenda.SEGUNDA_FACCAO_SKU > 0) ? 1 : 0) + ((agenda.TERCA_FACCAO_SKU > 0) ? 1 : 0) + ((agenda.QUARTA_FACCAO_SKU > 0) ? 1 : 0) + ((agenda.QUINTA_FACCAO_SKU > 0) ? 1 : 0) + ((agenda.SEXTA_FACCAO_SKU > 0) ? 1 : 0) + ((agenda.SABADO_FACCAO_SKU > 0) ? 1 : 0) + ((agenda.DOMINGO_FACCAO_SKU > 0) ? 1 : 0);
                mediaSkuFaccao = (contSkuFaccao <= 0) ? 0.00M : Convert.ToDecimal((somaSkuFaccao / (contSkuFaccao * 1.00M)));
                labFaccaoSKUMedia.Text = Convert.ToDecimal(mediaSkuFaccao).ToString("###,###,##0");

                Label labFaccaoQTDEMedia = e.Item.FindControl("labFaccaoQTDEMedia") as Label;
                contQtdeFaccao = ((agenda.SEGUNDA_FACCAO_QTDE > 0) ? 1 : 0) + ((agenda.TERCA_FACCAO_QTDE > 0) ? 1 : 0) + ((agenda.QUARTA_FACCAO_QTDE > 0) ? 1 : 0) + ((agenda.QUINTA_FACCAO_QTDE > 0) ? 1 : 0) + ((agenda.SEXTA_FACCAO_QTDE > 0) ? 1 : 0) + ((agenda.SABADO_FACCAO_QTDE > 0) ? 1 : 0) + ((agenda.DOMINGO_FACCAO_QTDE > 0) ? 1 : 0);
                mediaQtdeFaccao = (contQtdeFaccao <= 0) ? 0.00M : Convert.ToDecimal((somaQtdeFaccao / (contQtdeFaccao * 1.00M)));
                labFaccaoQTDEMedia.Text = Convert.ToDecimal(mediaQtdeFaccao).ToString("###,###,##0");
                if (mesAnterior)
                {
                    mediaFaltaFaccao += mediaQtdeFaccao;
                    qtdeSemanaFaccao = qtdeSemanaFaccao + ((mediaQtdeFaccao > 0) ? 1 : 0);
                }

                Label labAcabSKUMedia = e.Item.FindControl("labAcabSKUMedia") as Label;
                contSkuAcab = ((agenda.SEGUNDA_ACAB_SKU > 0) ? 1 : 0) + ((agenda.TERCA_ACAB_SKU > 0) ? 1 : 0) + ((agenda.QUARTA_ACAB_SKU > 0) ? 1 : 0) + ((agenda.QUINTA_ACAB_SKU > 0) ? 1 : 0) + ((agenda.SEXTA_ACAB_SKU > 0) ? 1 : 0) + ((agenda.SABADO_ACAB_SKU > 0) ? 1 : 0) + ((agenda.DOMINGO_ACAB_SKU > 0) ? 1 : 0);
                mediaSkuAcab = (contSkuAcab <= 0) ? 0.00M : Convert.ToDecimal((somaSkuAcab / (contSkuAcab * 1.00M)));
                labAcabSKUMedia.Text = Convert.ToDecimal(mediaSkuAcab).ToString("###,###,##0");

                Label labAcabQTDEMedia = e.Item.FindControl("labAcabQTDEMedia") as Label;
                contQtdeAcab = ((agenda.SEGUNDA_ACAB_QTDE > 0) ? 1 : 0) + ((agenda.TERCA_ACAB_QTDE > 0) ? 1 : 0) + ((agenda.QUARTA_ACAB_QTDE > 0) ? 1 : 0) + ((agenda.QUINTA_ACAB_QTDE > 0) ? 1 : 0) + ((agenda.SEXTA_ACAB_QTDE > 0) ? 1 : 0) + ((agenda.SABADO_ACAB_QTDE > 0) ? 1 : 0) + ((agenda.DOMINGO_ACAB_QTDE > 0) ? 1 : 0);
                mediaQtdeAcab = (contQtdeAcab <= 0) ? 0.00M : Convert.ToDecimal((somaQtdeAcab / (contQtdeAcab * 1.00M)));
                labAcabQTDEMedia.Text = Convert.ToDecimal(mediaQtdeAcab).ToString("###,###,##0");

                LimparVariaveis();
            }
        }

        protected void btAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                var ano = Convert.ToInt32(hidAno.Value);
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

                CarregarCalendario(ano.ToString(), mesAux);

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

                CarregarCalendario(ano.ToString(), mesAux);

            }
            catch (Exception)
            {
            }
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            CarregarCalendario(hidAno.Value, hidMes.Value);
        }


        #region "PLANEJAMENTO"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecaoPlan.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção";
                    return;
                }

                CarregarCalendario(hidAno.Value, hidMes.Value);

                faltaCorte = 0;
                faltaFaccao = 0;

                var plan = ObterCalendarioControlePlanejamento();

                gvPlanejamento.DataSource = plan;
                gvPlanejamento.DataBind();

                totSku = 0;
                totQtdeVarejo = 0;
                totQtdeAtacado = 0;
                totQtdeTotal = 0;

                var produc = ObterCalendarioControleProducao();

                gvProducao.DataSource = produc;
                gvProducao.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private List<SP_OBTER_CALENDARIO_PLANEJAMENTOResult> ObterCalendarioControlePlanejamento()
        {
            var plan = desenvController.ObterCalendarioControlePlanejamento(ddlColecaoPlan.SelectedValue);

            return plan;
        }

        protected void gvPlanejamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CALENDARIO_PLANEJAMENTOResult plan = e.Row.DataItem as SP_OBTER_CALENDARIO_PLANEJAMENTOResult;

                    totSku += Convert.ToInt32(plan.SKU);
                    totQtdeVarejo += Convert.ToInt32(plan.QTDE_VAREJO);
                    totQtdeAtacado += Convert.ToInt32(plan.QTDE_ATACADO);
                    totQtdeTotal += Convert.ToInt32(plan.QTDE_TOTAL);
                }
            }
        }
        protected void gvPlanejamento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPlanejamento.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[2].Text = totSku.ToString();
                footer.Cells[3].Text = totQtdeVarejo.ToString("###,###,##0");
                footer.Cells[4].Text = totQtdeAtacado.ToString("###,###,##0");
                footer.Cells[5].Text = totQtdeTotal.ToString("###,###,##0");

                faltaCorte += totQtdeTotal;
            }
        }
        protected void gvPlanejamento_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_CALENDARIO_PLANEJAMENTOResult> plan = ObterCalendarioControlePlanejamento();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvPlanejamento, e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(gvPlanejamento, e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(gvPlanejamento, e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            plan = plan.OrderBy(e.SortExpression + sortDirection);
            gvPlanejamento.DataSource = plan;
            gvPlanejamento.DataBind();
        }

        #endregion

        #region "EM ANDAMENTO"

        private List<SP_OBTER_CALENDARIO_PRODUCAOResult> ObterCalendarioControleProducao()
        {
            var produc = desenvController.ObterCalendarioControleProducao(ddlColecaoPlan.SelectedValue);

            return produc;
        }
        protected void gvProducao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CALENDARIO_PRODUCAOResult produc = e.Row.DataItem as SP_OBTER_CALENDARIO_PRODUCAOResult;

                    e.Row.Cells[2].Style.Add("background-color", produc.COR_HEXA);

                    if (produc.LOCAL.ToLower() == "corte")
                        e.Row.Cells[2].ForeColor = Color.White;

                    totSku += Convert.ToInt32(produc.SKU);
                    totSkuVarejo += Convert.ToInt32(produc.SKUVarejo);
                    totSkuAtacado += Convert.ToInt32(produc.SKUAtacado);

                    totQtdeVarejo += Convert.ToInt32(produc.QTDE_VAREJO);
                    totQtdeAtacado += Convert.ToInt32(produc.QTDE_ATACADO);
                    totQtdeTotal += Convert.ToInt32(produc.QTDE_TOTAL);

                    if (produc.LOCAL.ToLower() == "modelagem"
                        || produc.LOCAL.ToLower() == "risco"
                        || produc.LOCAL.ToLower() == "corte"
                        || produc.LOCAL.ToLower() == "pré-risco"
                    )
                    {
                        faltaCorte += Convert.ToInt32(produc.QTDE_TOTAL);
                        faltaTecido += Convert.ToInt32(produc.QTDE_TOTAL);
                    }

                    if (produc.LOCAL.ToLower() == "facção")
                        faltaFaccao += Convert.ToInt32(produc.QTDE_TOTAL);

                    Literal litAbrirLocal = e.Row.FindControl("litAbrirLocal") as Literal;
                    if (produc.LOCAL.ToLower() != "pré-pedido")
                        litAbrirLocal.Text = CriarLinkLocal(ddlColecaoPlan.SelectedValue.Trim(), produc.LOCAL.ToLower());

                }
            }
        }
        protected void gvProducao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProducao.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = totSkuVarejo.ToString();
                footer.Cells[4].Text = totQtdeVarejo.ToString("###,###,##0");

                footer.Cells[5].Text = totSkuAtacado.ToString();
                footer.Cells[6].Text = totQtdeAtacado.ToString("###,###,##0");

                footer.Cells[7].Text = totSku.ToString();
                footer.Cells[8].Text = totQtdeTotal.ToString("###,###,##0");


                /*AQUI ESTOU REUTILIZANDO VARIAVEIS QTDE_TOTAL E QTDE_ATACADO*/
                var x = 0;
                if (qtdeSemanaCorte > 0)
                    x = Convert.ToInt32(((mediaFaltaCorte * 1.00M) / qtdeSemanaCorte * 1.00M));

                var fCorte = new List<SP_OBTER_CALENDARIO_PRODUCAOResult>();
                fCorte.Add(new SP_OBTER_CALENDARIO_PRODUCAOResult
                {
                    LOCAL = "CORTE",
                    QTDE_TOTAL = faltaCorte,
                    QTDE_ATACADO = x
                });

                /*AQUI ESTOU REUTILIZANDO VARIAVEIS QTDE_TOTAL E QTDE_ATACADO*/
                x = 0;
                if (qtdeSemanaFaccao > 0)
                    x = Convert.ToInt32(((mediaFaltaFaccao * 1.00M) / qtdeSemanaFaccao * 1.00M));

                fCorte.Add(new SP_OBTER_CALENDARIO_PRODUCAOResult
                {
                    LOCAL = "FACÇÃO",
                    QTDE_TOTAL = faltaFaccao,
                    QTDE_ATACADO = x
                });

                /*AQUI ESTOU REUTILIZANDO VARIAVEIS QTDE_TOTAL E QTDE_ATACADO*/
                x = 0;
                if (qtdeSemanaCorte > 0)
                    x = Convert.ToInt32(((mediaFaltaCorte * 1.00M) / qtdeSemanaCorte * 1.00M));

                fCorte.Add(new SP_OBTER_CALENDARIO_PRODUCAOResult
                {
                    LOCAL = "LIBERAÇÃO TECIDO",
                    QTDE_TOTAL = faltaTecido,
                    QTDE_ATACADO = x
                });

                gvFaltaCorte.DataSource = fCorte;
                gvFaltaCorte.DataBind();


            }
        }
        protected void gvProducao_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_CALENDARIO_PRODUCAOResult> produc = ObterCalendarioControleProducao();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvPlanejamento, e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(gvProducao, e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(gvProducao, e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produc = produc.OrderBy(e.SortExpression + sortDirection);
            gvProducao.DataSource = produc;
            gvProducao.DataBind();
        }

        private string CriarLinkLocal(string colecao, string local)
        {
            var link = "desenv_controle_producao_calen_det.aspx?co=" + colecao.Trim() + "&loc=" + local.Trim().ToLower().Replace(" ", "_");
            link = utils.StringUtil.RemoverAcento(link);
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }

        #endregion

        protected void gvFaltaCorte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CALENDARIO_PRODUCAOResult produc = e.Row.DataItem as SP_OBTER_CALENDARIO_PRODUCAOResult;

                    var falt = desenvController.ObterCalendarioControleProducaoFaltante(Convert.ToInt32(produc.QTDE_TOTAL), Convert.ToInt32(produc.QTDE_ATACADO));
                    if (falt != null && falt.Count() > 0)
                    {
                        Label labPrevTermino = e.Row.FindControl("labPrevTermino") as Label;
                        labPrevTermino.Text = falt.FirstOrDefault().DIAFINAL.ToString("dd/MM/yyyy");
                    }

                    Literal litAbrirFalta = e.Row.FindControl("litAbrirFalta") as Literal;
                    litAbrirFalta.Text = CriarLinkFalta(Convert.ToInt32(produc.QTDE_TOTAL), Convert.ToInt32(produc.QTDE_ATACADO));
                }
            }
        }

        private string CriarLinkFalta(int qtdeFaltaCorte, int qtdeMediaDia)
        {
            var link = "desenv_controle_producao_calen_falta.aspx?qtfal=" + qtdeFaltaCorte.ToString() + "&diame=" + qtdeMediaDia.ToString();
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }

        protected void btAbrirPainel_Click(object sender, EventArgs e)
        {
            Response.Redirect("desenv_painel_producaov2.aspx?col=" + ddlColecaoPlan.SelectedValue.Trim(), "_blank", "");
        }
    }
}
