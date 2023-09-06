﻿using System;
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
using System.Text;

namespace Relatorios
{
    public partial class dre_dre_novo : System.Web.UI.Page
    {

        DREController dreController = new DREController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        Color corTitulo = System.Drawing.Color.Gainsboro;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";
        string link = "";
        string filial = "";
        string ctbContabil = "";
        string ano = "";

        string gCodigoFilial = "";
        List<SP_OBTER_DREResult> gDRE = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaAtacado = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaVarejo = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaReceitaBruta = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaReceitaLiquida = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaTaxaCartao = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaImposto = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaCmvAtacado = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaCmvVarejo = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaMargemContribuicao = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaMargemContribuicaoVarejo = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaMargemContribuicaoAtacado = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaCustoFixo = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaMargemBruta = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaDespesaCTO = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaDespesaEspecifica = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaDespesaVenda = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaDespesaADM = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaEBITDA = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaDepreciacao = new List<SP_OBTER_DREResult>();
        List<SP_OBTER_DREResult> gListaDespesaEventual = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaLucroBruto = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaReceitaDespesaFinanceira = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaLAIR = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaImpostoTaxa = new List<SP_OBTER_DREResult>();

        List<SP_OBTER_DREResult> gListaResultado = new List<SP_OBTER_DREResult>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
                //Carregar Filiais
                CarregarFilial();

                //Preencher dados filial e ano
                string codigoFilial = Request.QueryString["f"];
                string ano = Request.QueryString["a"];
                if ((codigoFilial != null && codigoFilial != "") && (ano != null && ano != ""))
                {
                    ddlFilial.SelectedValue = (new BaseController().BuscaFilialCodigo(Convert.ToInt32(codigoFilial))).COD_FILIAL;
                    ddlAno.SelectedValue = ano;
                }

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime dataIni;
            DateTime dataFim;
            string ano = "";
            int mesIni = 1;
            int mesFim = 12;

            ano = ddlAno.SelectedValue;
            dataIni = Convert.ToDateTime(ano + "-01-01");
            dataFim = Convert.ToDateTime(ano + "-12-31");
            mesIni = Convert.ToInt32(ddlMesDe.SelectedValue);
            mesFim = Convert.ToInt32(ddlMesAte.SelectedValue);

            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                gCodigoFilial = ddlFilial.SelectedValue;

            try
            {

                labErro.Text = "";
                btExcel.Enabled = false;
                hidFilial.Value = "";

                Session["DRE_20_REL"] = null;
                gDRE = dreController.ObterDRE2017(dataIni, dataFim, gCodigoFilial, mesIni, mesFim);
                Session["DRE_20_REL"] = gDRE.OrderBy(p => p.ID).ThenBy(p => p.GRUPO).ThenBy(p => p.LINHA).ThenBy(p => p.TIPO).ThenBy(p => p.FILIAL).ToList();
                gvDREExcel.Visible = true;
                gvDREExcel.DataSource = Session["DRE_20_REL"] as List<SP_OBTER_DREResult>;
                gvDREExcel.DataBind();
                gvDREExcel.Visible = false;

                CarregarReceitaLiquida();
                CarregarCMV();
                CarregarMargemContribuicao();
                CarregarCustoFixo();
                CarregarMargemBruta();
                CarregarDespesaCTO();
                CarregarDespesaEspecifica();
                CarregarDespesaVenda();
                CarregarDespesaADM();
                CarregarEBTIDA();
                CarregarDepreciacao();
                CarregarDespesaEventual();
                CarregarLucroBruto();
                CarregarReceitaDespFinanceira();
                CarregarLAIR();
                CarregarImpostosTaxas();
                CarregarResultado();

                btExcel.Enabled = true;
                hidFilial.Value = ddlFilial.SelectedItem.Text.Trim();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }

        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliaisAtivaInativa();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "000000", FILIAL = "ATACADO" });
                filial.Insert(2, new FILIAI { COD_FILIAL = "999999", FILIAL = "CORPORATIVO" });
                filial.Insert(3, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A' && p.ANO == 2017).ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = "2017";
                    }
                    catch (Exception) { }
                }
            }
        }
        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + "</font> ";
            return retorno;
        }
        private string FormatarValor(decimal? valor, int mes)
        {
            // 0 - Faturamento Bruto Total
            // 1 - Faturamento Bruto Varejo
            // 2 - Faturamento Bruto Atacado

            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            decimal fatBrutoJaneiro = FiltroValor(gListaReceitaBruta.Sum(p => p.JANEIRO), 1);
            decimal fatBrutoFevereiro = FiltroValor(gListaReceitaBruta.Sum(p => p.FEVEREIRO), 2);
            decimal fatBrutoMarco = FiltroValor(gListaReceitaBruta.Sum(p => p.MARCO), 3);
            decimal fatBrutoAbril = FiltroValor(gListaReceitaBruta.Sum(p => p.ABRIL), 4);
            decimal fatBrutoMaio = FiltroValor(gListaReceitaBruta.Sum(p => p.MAIO), 5);
            decimal fatBrutoJunho = FiltroValor(gListaReceitaBruta.Sum(p => p.JUNHO), 6);
            decimal fatBrutoJulho = FiltroValor(gListaReceitaBruta.Sum(p => p.JULHO), 7);
            decimal fatBrutoAgosto = FiltroValor(gListaReceitaBruta.Sum(p => p.AGOSTO), 8);
            decimal fatBrutoSetembro = FiltroValor(gListaReceitaBruta.Sum(p => p.SETEMBRO), 9);
            decimal fatBrutoOutubro = FiltroValor(gListaReceitaBruta.Sum(p => p.OUTUBRO), 10);
            decimal fatBrutoNovembro = FiltroValor(gListaReceitaBruta.Sum(p => p.NOVEMBRO), 11);
            decimal fatBrutoDezembro = FiltroValor(gListaReceitaBruta.Sum(p => p.DEZEMBRO), 12);
            decimal fatBrutoTotal = FiltroValor(gListaReceitaBruta.Sum(p => p.TOTAL), 0);

            decimal valorPercentual = 0;
            if (mes == 1 && fatBrutoJaneiro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoJaneiro) * 100;
            else if (mes == 2 && fatBrutoFevereiro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoFevereiro) * 100;
            else if (mes == 3 && fatBrutoMarco > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoMarco) * 100;
            else if (mes == 4 && fatBrutoAbril > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoAbril) * 100;
            else if (mes == 5 && fatBrutoMaio > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoMaio) * 100;
            else if (mes == 6 && fatBrutoJunho > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoJunho) * 100;
            else if (mes == 7 && fatBrutoJulho > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoJulho) * 100;
            else if (mes == 8 && fatBrutoAgosto > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoAgosto) * 100;
            else if (mes == 9 && fatBrutoSetembro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoSetembro) * 100;
            else if (mes == 10 && fatBrutoOutubro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoOutubro) * 100;
            else if (mes == 11 && fatBrutoNovembro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoNovembro) * 100;
            else if (mes == 12 && fatBrutoDezembro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoDezembro) * 100;
            else
                valorPercentual = (fatBrutoTotal > 0) ? (Convert.ToDecimal(valor / fatBrutoTotal) * 100) : 0; // TOTAL

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") +
                ((valorPercentual != 0) ? ("<br />(" + valorPercentual.ToString("##0.00;##0.00") + " %)" + "</font> ") : "");
            return retorno;
        }
        private string FormatarValor(decimal? valor, int mes, int tipoDivisao)
        {
            // 0 - Faturamento Bruto Total
            // 1 - Faturamento Bruto Varejo
            // 2 - Faturamento Bruto Atacado

            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            decimal fatBrutoJaneiro = 0;
            decimal fatBrutoFevereiro = 0;
            decimal fatBrutoMarco = 0;
            decimal fatBrutoAbril = 0;
            decimal fatBrutoMaio = 0;
            decimal fatBrutoJunho = 0;
            decimal fatBrutoJulho = 0;
            decimal fatBrutoAgosto = 0;
            decimal fatBrutoSetembro = 0;
            decimal fatBrutoOutubro = 0;
            decimal fatBrutoNovembro = 0;
            decimal fatBrutoDezembro = 0;
            decimal fatBrutoTotal = 0;

            // Varejo
            if (tipoDivisao == 1)
            {
                fatBrutoJaneiro = FiltroValor(gListaVarejo.Sum(p => p.JANEIRO), 1);
                fatBrutoFevereiro = FiltroValor(gListaVarejo.Sum(p => p.FEVEREIRO), 2);
                fatBrutoMarco = FiltroValor(gListaVarejo.Sum(p => p.MARCO), 3);
                fatBrutoAbril = FiltroValor(gListaVarejo.Sum(p => p.ABRIL), 4);
                fatBrutoMaio = FiltroValor(gListaVarejo.Sum(p => p.MAIO), 5);
                fatBrutoJunho = FiltroValor(gListaVarejo.Sum(p => p.JUNHO), 6);
                fatBrutoJulho = FiltroValor(gListaVarejo.Sum(p => p.JULHO), 7);
                fatBrutoAgosto = FiltroValor(gListaVarejo.Sum(p => p.AGOSTO), 8);
                fatBrutoSetembro = FiltroValor(gListaVarejo.Sum(p => p.SETEMBRO), 9);
                fatBrutoOutubro = FiltroValor(gListaVarejo.Sum(p => p.OUTUBRO), 10);
                fatBrutoNovembro = FiltroValor(gListaVarejo.Sum(p => p.NOVEMBRO), 11);
                fatBrutoDezembro = FiltroValor(gListaVarejo.Sum(p => p.DEZEMBRO), 12);
                fatBrutoTotal = FiltroValor(gListaVarejo.Sum(p => p.TOTAL), 0);
            }

            decimal valorPercentual = 0;
            if (mes == 1 && fatBrutoJaneiro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoJaneiro) * 100;
            else if (mes == 2 && fatBrutoFevereiro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoFevereiro) * 100;
            else if (mes == 3 && fatBrutoMarco > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoMarco) * 100;
            else if (mes == 4 && fatBrutoAbril > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoAbril) * 100;
            else if (mes == 5 && fatBrutoMaio > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoMaio) * 100;
            else if (mes == 6 && fatBrutoJunho > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoJunho) * 100;
            else if (mes == 7 && fatBrutoJulho > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoJulho) * 100;
            else if (mes == 8 && fatBrutoAgosto > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoAgosto) * 100;
            else if (mes == 9 && fatBrutoSetembro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoSetembro) * 100;
            else if (mes == 10 && fatBrutoOutubro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoOutubro) * 100;
            else if (mes == 11 && fatBrutoNovembro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoNovembro) * 100;
            else if (mes == 12 && fatBrutoDezembro > 0)
                valorPercentual = Convert.ToDecimal(valor / fatBrutoDezembro) * 100;
            else
                valorPercentual = (fatBrutoTotal > 0) ? (Convert.ToDecimal(valor / fatBrutoTotal) * 100) : 0; // TOTAL

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") +
                ((valorPercentual != 0) ? ("<br />(" + valorPercentual.ToString("##0.00;##0.00") + " %)" + "</font> ") : "");
            return retorno;
        }

        private decimal FiltroValor(decimal? valor, int mes)
        {
            int mesFiltroDe = 0;
            int mesFiltroAte = 0;

            if (ddlMesDe.SelectedValue != "")
            {
                mesFiltroDe = Convert.ToInt32(ddlMesDe.SelectedValue);
                if (mes < mesFiltroDe)
                    valor = 0;
            }

            if (ddlMesAte.SelectedValue != "")
            {
                mesFiltroAte = Convert.ToInt32(ddlMesAte.SelectedValue);
                if (mes > mesFiltroAte)
                    valor = 0;
            }

            return Convert.ToDecimal(valor);
        }

        #endregion

        #region "ACOES"

        private void CarregarReceitaLiquida()
        {

            gListaAtacado = gDRE.Where(p => p.ID == 1).ToList();
            gListaVarejo = gDRE.Where(p => p.ID == 2).ToList();
            gListaImposto = gDRE.Where(p => p.ID == 3).ToList();
            gListaTaxaCartao = gDRE.Where(p => p.ID == 4).ToList();

            gListaReceitaBruta = gListaVarejo.Union(gListaAtacado).ToList();
            gListaReceitaBruta = gListaReceitaBruta.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "RECEITAS BRUTAS",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            var deducoesReceitaBruta = gListaImposto.Union(gListaTaxaCartao);
            deducoesReceitaBruta = deducoesReceitaBruta.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                k => new SP_OBTER_DREResult
                                                                {
                                                                    ID = 2,
                                                                    GRUPO = "DEDUÇÕES RECEITAS BRUTAS",
                                                                    JANEIRO = k.Sum(j => j.JANEIRO),
                                                                    FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                    MARCO = k.Sum(j => j.MARCO),
                                                                    ABRIL = k.Sum(j => j.ABRIL),
                                                                    MAIO = k.Sum(j => j.MAIO),
                                                                    JUNHO = k.Sum(j => j.JUNHO),
                                                                    JULHO = k.Sum(j => j.JULHO),
                                                                    AGOSTO = k.Sum(j => j.AGOSTO),
                                                                    SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                    OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                    NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                    DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                    TOTAL = k.Sum(j => j.TOTAL)
                                                                }).ToList();

            var receitaFaturamento = gListaReceitaBruta.Union(deducoesReceitaBruta).ToList();

            receitaFaturamento.Insert(receitaFaturamento.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "1" });

            gListaReceitaLiquida = gDRE.Where(p => p.ID == 5).ToList();
            var receitaLiquida = gListaReceitaLiquida.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                k => new SP_OBTER_DREResult
                                                                {
                                                                    ID = 3,
                                                                    GRUPO = "RECEITAS LÍQUIDAS",
                                                                    JANEIRO = k.Sum(j => j.JANEIRO),
                                                                    FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                    MARCO = k.Sum(j => j.MARCO),
                                                                    ABRIL = k.Sum(j => j.ABRIL),
                                                                    MAIO = k.Sum(j => j.MAIO),
                                                                    JUNHO = k.Sum(j => j.JUNHO),
                                                                    JULHO = k.Sum(j => j.JULHO),
                                                                    AGOSTO = k.Sum(j => j.AGOSTO),
                                                                    SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                    OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                    NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                    DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                    TOTAL = k.Sum(j => j.TOTAL)
                                                                }).ToList();
            receitaFaturamento.AddRange(receitaLiquida);

            receitaFaturamento.Insert(receitaFaturamento.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "2" });

            if (receitaFaturamento != null)
            {
                gvReceitaLiquida.DataSource = receitaFaturamento;
                gvReceitaLiquida.DataBind();
            }
        }
        private void CarregarCMV()
        {
            gListaCmvAtacado = gDRE.Where(p => p.ID == 6).ToList();
            gListaCmvVarejo = gDRE.Where(p => p.ID == 7).ToList();


            var cmv = gListaCmvAtacado.Union(gListaCmvVarejo).ToList();
            cmv = cmv.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "CMV",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            cmv.Insert(cmv.Count, new SP_OBTER_DREResult { GRUPO = "" });
            cmv.Insert(cmv.Count, new SP_OBTER_DREResult { GRUPO = "CMV" });
            cmv.Insert(cmv.Count, new SP_OBTER_DREResult { GRUPO = "" });
            if (cmv != null)
            {
                gvCMV.DataSource = cmv;
                gvCMV.DataBind();
            }
        }
        private void CarregarMargemContribuicao()
        {
            gListaMargemContribuicaoAtacado = gDRE.Where(p => p.ID == 8).ToList();
            gListaMargemContribuicaoVarejo = gDRE.Where(p => p.ID == 9).ToList();

            gListaMargemContribuicao = gListaMargemContribuicaoAtacado.Union(gListaMargemContribuicaoVarejo).ToList();

            var margemContribuicao = gListaMargemContribuicao.GroupBy(p => new { GRUPO = p.GRUPO }).Select(k => new SP_OBTER_DREResult
            {
                GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                LINHA = "AGRUPADO",
                JANEIRO = k.Sum(g => g.JANEIRO),
                FEVEREIRO = k.Sum(g => g.FEVEREIRO),
                MARCO = k.Sum(g => g.MARCO),
                ABRIL = k.Sum(g => g.ABRIL),
                MAIO = k.Sum(g => g.MAIO),
                JUNHO = k.Sum(g => g.JUNHO),
                JULHO = k.Sum(g => g.JULHO),
                AGOSTO = k.Sum(g => g.AGOSTO),
                SETEMBRO = k.Sum(g => g.SETEMBRO),
                OUTUBRO = k.Sum(g => g.OUTUBRO),
                NOVEMBRO = k.Sum(g => g.NOVEMBRO),
                DEZEMBRO = k.Sum(g => g.DEZEMBRO),
                TOTAL = k.Sum(g => g.TOTAL)
            }).ToList();


            margemContribuicao.Insert(margemContribuicao.Count, new SP_OBTER_DREResult { GRUPO = "" });

            //Carregar margem de contribuicao
            gvMargem.DataSource = margemContribuicao;
            gvMargem.DataBind();
        }
        private void CarregarCustoFixo()
        {
            gListaCustoFixo = gDRE.Where(p => p.ID == 11).ToList();

            var custoFixoAgrupado = gListaCustoFixo;
            custoFixoAgrupado = custoFixoAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "CUSTOS FIXOS",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            custoFixoAgrupado.Insert(custoFixoAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (custoFixoAgrupado != null)
            {
                gvCustoFixo.DataSource = custoFixoAgrupado;
                gvCustoFixo.DataBind();
            }
        }
        private void CarregarMargemBruta()
        {
            gListaMargemBruta = gDRE.Where(p => p.ID == 12).ToList();

            gListaMargemBruta.Insert(gListaMargemBruta.Count, new SP_OBTER_DREResult { ID = 97, GRUPO = "" });

            //Carregar margem bruta
            gvMargemBruta.DataSource = gListaMargemBruta;
            gvMargemBruta.DataBind();
        }
        private void CarregarDespesaCTO()
        {
            gListaDespesaCTO = gDRE.Where(p => p.ID == 13).ToList();

            var despCTOAgrupado = gListaDespesaCTO;
            despCTOAgrupado = despCTOAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS CTO",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            despCTOAgrupado.Insert(despCTOAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            despCTOAgrupado.Insert(despCTOAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (despCTOAgrupado != null)
            {
                gvDespesaCTO.DataSource = despCTOAgrupado;
                gvDespesaCTO.DataBind();
            }
        }
        private void CarregarDespesaEspecifica()
        {
            gListaDespesaEspecifica = gDRE.Where(p => p.ID == 14).ToList();

            var despEspecificaAgrupado = gListaDespesaEspecifica;
            despEspecificaAgrupado = despEspecificaAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS ESPECÍFICAS",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            despEspecificaAgrupado.Insert(despEspecificaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            despEspecificaAgrupado.Insert(despEspecificaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (despEspecificaAgrupado != null)
            {
                gvDespesaEspecifica.DataSource = despEspecificaAgrupado;
                gvDespesaEspecifica.DataBind();
            }
        }
        private void CarregarDespesaVenda()
        {
            gListaDespesaVenda = gDRE.Where(p => p.ID == 15).ToList();

            var despVendaAgrupado = gListaDespesaVenda;
            despVendaAgrupado = despVendaAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS VENDAS",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            despVendaAgrupado.Insert(despVendaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            despVendaAgrupado.Insert(despVendaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (despVendaAgrupado != null)
            {
                gvDespesaVenda.DataSource = despVendaAgrupado;
                gvDespesaVenda.DataBind();
            }
        }
        private void CarregarDespesaADM()
        {
            gListaDespesaADM = gDRE.Where(p => p.ID == 16).ToList();

            var despADMAgrupado = gListaDespesaADM.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS ADMINISTRATIVAS",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (despADMAgrupado != null)
            {
                gvDespesaAdm.DataSource = despADMAgrupado;
                gvDespesaAdm.DataBind();
            }
        }
        private void CarregarEBTIDA()
        {
            gListaEBITDA = gDRE.Where(p => p.ID == 17).ToList();

            gListaEBITDA.Insert(gListaEBITDA.Count, new SP_OBTER_DREResult { ID = 97, GRUPO = "" });

            //Carregar gvEbtida
            gvEbtida.DataSource = gListaEBITDA;
            gvEbtida.DataBind();
        }
        private void CarregarDepreciacao()
        {
            gListaDepreciacao = gDRE.Where(p => p.ID == 18).ToList();

            var depreciacaoAgrupado = gListaDepreciacao;
            depreciacaoAgrupado = depreciacaoAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            depreciacaoAgrupado.Insert(depreciacaoAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            depreciacaoAgrupado.Insert(depreciacaoAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (depreciacaoAgrupado != null)
            {
                gvDepreciacaoAmortizacao.DataSource = depreciacaoAgrupado;
                gvDepreciacaoAmortizacao.DataBind();
            }
        }
        private void CarregarDespesaEventual()
        {
            gListaDespesaEventual = gDRE.Where(p => p.ID == 19).ToList();

            var despesaEventualAgrupado = gListaDespesaEventual.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 22,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            despesaEventualAgrupado.Insert(despesaEventualAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            despesaEventualAgrupado.Insert(despesaEventualAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (despesaEventualAgrupado != null)
            {
                gvDespesaEventual.DataSource = despesaEventualAgrupado;
                gvDespesaEventual.DataBind();
            }
        }
        private void CarregarLucroBruto()
        {
            gListaLucroBruto = gDRE.Where(p => p.ID == 20).ToList();

            gListaLucroBruto.Insert(gListaLucroBruto.Count, new SP_OBTER_DREResult { ID = 97, GRUPO = "" });

            //Carregar gListaLucroBruto
            gvLucroBruto.DataSource = gListaLucroBruto;
            gvLucroBruto.DataBind();
        }
        private void CarregarReceitaDespFinanceira()
        {
            gListaReceitaDespesaFinanceira = gDRE.Where(p => p.ID == 21).ToList();

            var receitaDespesaAgrupado = gListaReceitaDespesaFinanceira;
            receitaDespesaAgrupado = receitaDespesaAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            receitaDespesaAgrupado.Insert(receitaDespesaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            receitaDespesaAgrupado.Insert(receitaDespesaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (receitaDespesaAgrupado != null)
            {
                gvReceitaDespesaFin.DataSource = receitaDespesaAgrupado;
                gvReceitaDespesaFin.DataBind();
            }
        }
        private void CarregarLAIR()
        {
            gListaLAIR = gDRE.Where(p => p.ID == 22).ToList();

            gListaLAIR.Insert(gListaLAIR.Count, new SP_OBTER_DREResult { ID = 97, GRUPO = "" });

            //Carregar gvLAIR
            gvLAIR.DataSource = gListaLAIR;
            gvLAIR.DataBind();
        }
        private void CarregarImpostosTaxas()
        {
            gListaImpostoTaxa = gDRE.Where(p => p.ID == 23).ToList();

            var impostotaxaAgrupado = gListaImpostoTaxa;
            impostotaxaAgrupado = impostotaxaAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DREResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                                TOTAL = k.Sum(j => j.TOTAL)
                                                                            }).ToList();

            impostotaxaAgrupado.Insert(impostotaxaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });
            impostotaxaAgrupado.Insert(impostotaxaAgrupado.Count, new SP_OBTER_DREResult { GRUPO = "", LINHA = "" });

            if (impostotaxaAgrupado != null)
            {
                gvImpostoTaxa.DataSource = impostotaxaAgrupado;
                gvImpostoTaxa.DataBind();
            }
        }
        private void CarregarResultado()
        {
            gListaResultado = gDRE.Where(p => p.ID == 24).ToList();

            gListaResultado.Insert(gListaResultado.Count, new SP_OBTER_DREResult { ID = 97, GRUPO = "" });

            //Carregar gvResultado
            gvResultado.DataSource = gListaResultado;
            gvResultado.DataBind();
        }

        protected void imgAjuda_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string _url = "";
            if (b != null)
            {
                try
                {
                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('dre_ajuda.aspx');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        #endregion

        #region "RECEITA LIQUIDA"

        protected void gvReceitaLiquida_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult receitaLiquida = e.Row.DataItem as SP_OBTER_DREResult;

                    if (receitaLiquida != null && receitaLiquida.GRUPO != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + receitaLiquida.GRUPO.Trim() + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(receitaLiquida.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(receitaLiquida.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(receitaLiquida.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(receitaLiquida.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(receitaLiquida.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(receitaLiquida.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(receitaLiquida.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(receitaLiquida.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(receitaLiquida.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(receitaLiquida.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(receitaLiquida.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(receitaLiquida.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(receitaLiquida.TOTAL);

                    }

                    //Controla tamanho da linha vazia
                    if (receitaLiquida.LINHA == "1")
                        e.Row.Height = Unit.Pixel(9);
                    else if (receitaLiquida.LINHA == "2")
                        e.Row.Height = Unit.Pixel(5);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (receitaLiquida.ID == 1)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                var faturamentoBruto = gListaAtacado.Union(gListaVarejo).ToList();
                                faturamentoBruto = faturamentoBruto.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 99,
                                                                            LINHA = k.Key.LINHA.Replace("Faturamento", "").Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvFaturamento.DataSource = faturamentoBruto;
                                gvFaturamento.DataBind();
                            }
                            img.Visible = true;
                        }
                        if (receitaLiquida.ID == 2)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                var deducoesBruto = gListaImposto.Union(gListaTaxaCartao).ToList();
                                deducoesBruto = deducoesBruto.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 99,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvFaturamento.DataSource = deducoesBruto;
                                gvFaturamento.DataBind();
                            }
                            img.Visible = true;
                        }


                    }
                }
            }
        }
        protected void gvReceitaLiquida_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            if (gvReceitaLiquida.Rows.Count >= 5)
            {
                GridViewRow firstBlankRow = gvReceitaLiquida.Rows[gvReceitaLiquida.Rows.Count - 3];
                if (firstBlankRow != null)
                {
                    //Linha acima de receitas liquidas
                    firstBlankRow.BackColor = corTitulo;
                    firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";
                    firstBlankRow.Attributes["style"] = "line-height: 1px";
                }

                GridViewRow lastBlankRow = gvReceitaLiquida.Rows[gvReceitaLiquida.Rows.Count - 1];
                if (lastBlankRow != null)
                {
                    //Linha abaixo de receitas liquidas
                    lastBlankRow.BackColor = corTitulo;
                    int count = gvReceitaLiquida.Columns.Count;
                    for (int i = 0; i < count; i++)
                        lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);

                    lastBlankRow.Attributes["style"] = "line-height: 1px";
                }

                GridViewRow lastRow = gvReceitaLiquida.Rows[gvReceitaLiquida.Rows.Count - 2];
                if (lastRow != null)
                    lastRow.BackColor = corFundo;
            }
        }

        protected void gvFaturamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult faturamento = e.Row.DataItem as SP_OBTER_DREResult;

                    if (faturamento != null && faturamento.GRUPO != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                        {
                            if (faturamento.LINHA.ToUpper().Contains("ATACADO") || faturamento.LINHA.ToUpper().Contains("VAREJO") || faturamento.LINHA.ToUpper().Contains("ECOMMERCE"))
                                _litFaturamento.Text = "<font size='2' face='Calibri'>&nbsp;" + faturamento.LINHA.Trim().ToUpper() + "</font> ";
                            else
                                _litFaturamento.Text = "<font size='2' face='Calibri'>&nbsp;(-) " + faturamento.LINHA.Trim().ToUpper() + "</font> ";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(faturamento.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(faturamento.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(faturamento.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(faturamento.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(faturamento.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(faturamento.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(faturamento.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(faturamento.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(faturamento.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(faturamento.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(faturamento.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(faturamento.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(faturamento.TOTAL);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (faturamento.LINHA.Trim().ToUpper() == "IMPOSTOS S/ VENDA")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                gListaImposto = gListaImposto.GroupBy(b => new { TIPO = b.TIPO }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 99,
                                                                            LINHA = k.Key.TIPO.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();

                                gvSubGrid.DataSource = gListaImposto;
                                gvSubGrid.DataBind();
                            }
                            img.Visible = true;
                        }

                        if (faturamento.LINHA.Trim().ToUpper() == "VAREJO")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                gvSubGrid.DataSource = gListaVarejo.Where(p => !p.FILIAL.ToUpper().Contains("ONLINE"));
                                gvSubGrid.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvFaturamento_DataBound(object sender, EventArgs e)
        {
            GridView gvFaturamento = (GridView)sender;
            if (gvFaturamento != null)
                if (gvFaturamento.HeaderRow != null)
                    gvFaturamento.HeaderRow.Visible = false;
        }

        protected void gvSubGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.LINHA.ToUpper().Contains("VAREJO") && !subitem.FILIAL.ToUpper().Contains("ONLINE"))
                                _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";
                            else
                                _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;(-) " + subitem.LINHA.Trim().ToUpper() + "</font> ";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);

                    }

                }
            }
        }
        protected void gvSubGrid_DataBound(object sender, EventArgs e)
        {
            GridView gvSubItem = (GridView)sender;
            if (gvSubItem != null)
                if (gvSubItem.HeaderRow != null)
                    gvSubItem.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvSubItem.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "CMV"

        protected void gvCMV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult cmv = e.Row.DataItem as SP_OBTER_DREResult;

                    if (cmv != null && cmv.GRUPO != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + cmv.GRUPO.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(cmv.JANEIRO, 1), 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(cmv.FEVEREIRO, 2), 2);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(cmv.MARCO, 3), 3);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(cmv.ABRIL, 4), 4);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(cmv.MAIO, 5), 5);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(cmv.JUNHO, 6), 6);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(cmv.JULHO, 7), 7);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(cmv.AGOSTO, 8), 8);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(cmv.SETEMBRO, 9), 9);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(cmv.OUTUBRO, 10), 10);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(cmv.NOVEMBRO, 11), 11);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(cmv.DEZEMBRO, 12), 12);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(cmv.TOTAL, 0);

                    }

                    if (cmv.GRUPO == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cmv.ID == 1)
                        {
                            GridView gvCMVItem = e.Row.FindControl("gvCMVItem") as GridView;
                            if (gvCMVItem != null)
                            {
                                var cmvAgrupado = gListaCmvAtacado.Union(gListaCmvVarejo).ToList();
                                cmvAgrupado = cmvAgrupado.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvCMVItem.DataSource = cmvAgrupado;
                                gvCMVItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvCMV_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvCMV.Rows[gvCMV.Rows.Count - 3];
            if (firstBlankRow != null)
                // Linha abaixo de custo dos produtos
                firstBlankRow.BackColor = corTitulo;

            GridViewRow lastBlankRow = gvCMV.Rows[gvCMV.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvCMV.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvCMV.Rows[gvCMV.Rows.Count - 2];
            if (lastRow != null)
                lastRow.Visible = false;

            GridViewRow headerRow = gvCMV.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 1px;";

        }

        protected void gvCMVItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult cmvlinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (cmvlinha != null && cmvlinha.GRUPO != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                            _litFaturamento.Text = "<font size='2' face='Calibri'>&nbsp;" + cmvlinha.LINHA.Trim().ToUpper() + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(cmvlinha.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(cmvlinha.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(cmvlinha.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(cmvlinha.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(cmvlinha.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(cmvlinha.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(cmvlinha.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(cmvlinha.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(cmvlinha.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(cmvlinha.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(cmvlinha.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(cmvlinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(cmvlinha.TOTAL);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cmvlinha.LINHA.Trim().ToUpper() == "VAREJO")
                        {
                            GridView gvCMVItemSub = e.Row.FindControl("gvCMVItemSub") as GridView;
                            if (gvCMVItemSub != null)
                            {
                                gvCMVItemSub.DataSource = gListaCmvVarejo;
                                gvCMVItemSub.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvCMVItem_DataBound(object sender, EventArgs e)
        {
            GridView gvCMVItem = (GridView)sender;
            if (gvCMVItem != null)
                if (gvCMVItem.HeaderRow != null)
                    gvCMVItem.HeaderRow.Visible = false;
        }

        protected void gvCMVItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.LINHA.ToUpper().Contains("VAREJO"))
                                _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";
                            else
                                _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;(-) " + subitem.LINHA.Trim().ToUpper() + "</font> ";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);

                    }

                }
            }
        }
        protected void gvCMVItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvCMVItemSub = (GridView)sender;
            if (gvCMVItemSub != null)
                if (gvCMVItemSub.HeaderRow != null)
                    gvCMVItemSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvCMVItemSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "MARGEM DE CONTRIBUICAO"

        protected void gvMargem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult margem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (margem != null && margem.GRUPO != "")
                    {
                        Literal _litMargem = e.Row.FindControl("litMargem") as Literal;
                        if (_litMargem != null)
                            _litMargem.Text = "<font size='2' face='Calibri'>&nbsp;" + margem.GRUPO.Trim() + "</font>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(margem.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(margem.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(margem.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(margem.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(margem.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(margem.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(margem.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(margem.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(margem.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(margem.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(margem.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(margem.DEZEMBRO, 12)).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(margem.TOTAL).Trim();

                    }

                    if (margem.GRUPO == "")
                        e.Row.Height = Unit.Pixel(1);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (margem.GRUPO != "")
                        {
                            GridView gvMargemItem = e.Row.FindControl("gvMargemItem") as GridView;
                            if (gvMargemItem != null)
                            {
                                var margemContribuicao = gListaMargemContribuicao.GroupBy(p => new { LINHA = p.LINHA }).Select(k => new SP_OBTER_DREResult
                                {
                                    GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                                    LINHA = k.Key.LINHA.Trim(),
                                    JANEIRO = k.Sum(g => g.JANEIRO),
                                    FEVEREIRO = k.Sum(g => g.FEVEREIRO),
                                    MARCO = k.Sum(g => g.MARCO),
                                    ABRIL = k.Sum(g => g.ABRIL),
                                    MAIO = k.Sum(g => g.MAIO),
                                    JUNHO = k.Sum(g => g.JUNHO),
                                    JULHO = k.Sum(g => g.JULHO),
                                    AGOSTO = k.Sum(g => g.AGOSTO),
                                    SETEMBRO = k.Sum(g => g.SETEMBRO),
                                    OUTUBRO = k.Sum(g => g.OUTUBRO),
                                    NOVEMBRO = k.Sum(g => g.NOVEMBRO),
                                    DEZEMBRO = k.Sum(g => g.DEZEMBRO),
                                    TOTAL = k.Sum(g => g.TOTAL)
                                }).ToList();


                                gvMargemItem.DataSource = margemContribuicao;
                                gvMargemItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvMargem_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            /*GridViewRow firstBlankRow = gvMargem.Rows[gvMargem.Rows.Count - 1];
            if (firstBlankRow != null)
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";*/

            GridViewRow lastBlankRow = gvMargem.Rows[gvMargem.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = corTitulo;
                int count = gvMargem.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);

                lastBlankRow.Attributes["style"] = "line-height: 2px;";
            }

            GridViewRow headerRow = gvMargem.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 1px;";
        }

        protected void gvMargemItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult _margem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (_margem != null && _margem.LINHA != "")
                    {
                        Literal _litVenda = e.Row.FindControl("litVenda") as Literal;
                        if (_litVenda != null)
                            _litVenda.Text = "<font size='2' face='Calibri'>&nbsp;" + _margem.LINHA.Replace("RECEITA", "").Trim().ToUpper() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(_margem.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(_margem.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(_margem.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(_margem.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(_margem.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(_margem.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(_margem.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(_margem.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(_margem.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(_margem.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(_margem.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(_margem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(_margem.TOTAL);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        GridView gvMargemItemSub = e.Row.FindControl("gvMargemItemSub") as GridView;
                        if (gvMargemItemSub != null)
                        {
                            var margem = gDRE.Where(p => p.ID == 10 && p.LINHA == _margem.LINHA);

                            gvMargemItemSub.DataSource = margem;
                            gvMargemItemSub.DataBind();
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvMargemItem_DataBound(object sender, EventArgs e)
        {
            GridView gvMargemItem = (GridView)sender;
            if (gvMargemItem != null)
                if (gvMargemItem.HeaderRow != null)
                    gvMargemItem.HeaderRow.Visible = false;
        }

        protected void gvMargemItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper().Contains("MARK-UP")))
                                _litSubItem.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;<i>" + subitem.TIPO.Replace("MARGEM", "MARG").Trim() + "</i></font></span>";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.JANEIRO, 1)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1)).Trim();
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)).Trim();
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.MARCO, 3)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.ABRIL, 4)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4)).Trim();
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.MAIO, 5)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5)).Trim();
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.JUNHO, 6)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6)).Trim();
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.JULHO, 7)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7)).Trim();
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.AGOSTO, 8)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8)).Trim();
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.SETEMBRO, 9)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9)).Trim();
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.OUTUBRO, 10)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10)).Trim();
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)).Trim();
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                                _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (subitem.TIPO.Contains("%") || (subitem.TIPO.Trim().ToUpper() == "MARK-UP"))
                            _total.Text = "<span style='font-weight:normal'>" + FormatarValor(subitem.TOTAL).Trim() + ((subitem.TIPO.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                        else
                            _total.Text = FormatarValor(subitem.TOTAL).Trim();

                    }
                }
            }
        }
        protected void gvMargemItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvMargemItemSub = (GridView)sender;
            if (gvMargemItemSub != null)
                if (gvMargemItemSub.HeaderRow != null)
                    gvMargemItemSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvMargemItemSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "CUSTO FIXO"

        protected void gvCustoFixo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult cfixo = e.Row.DataItem as SP_OBTER_DREResult;

                    if (cfixo != null && cfixo.GRUPO != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litCustoFixo") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + cfixo.GRUPO.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(cfixo.JANEIRO, 1), 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(cfixo.FEVEREIRO, 2), 2);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(cfixo.MARCO, 3), 3);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(cfixo.ABRIL, 4), 4);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(cfixo.MAIO, 5), 5);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(cfixo.JUNHO, 6), 6);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(cfixo.JULHO, 7), 7);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(cfixo.AGOSTO, 8), 8);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(cfixo.SETEMBRO, 9), 9);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(cfixo.OUTUBRO, 10), 10);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(cfixo.NOVEMBRO, 11), 11);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(cfixo.DEZEMBRO, 12), 12);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(cfixo.TOTAL, 0);


                    }

                    if (cfixo.GRUPO == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cfixo.ID == 1)
                        {
                            GridView gvCustoFixoItem = e.Row.FindControl("gvCustoFixoItem") as GridView;
                            if (gvCustoFixoItem != null)
                            {
                                var custoFixoLinha = gListaCustoFixo.ToList();
                                custoFixoLinha = custoFixoLinha.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvCustoFixoItem.DataSource = custoFixoLinha.OrderBy(p => p.LINHA);
                                gvCustoFixoItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvCustoFixo_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvCustoFixo.Rows[gvCustoFixo.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvCustoFixo.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow headerRow = gvCustoFixo.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";

        }

        protected void gvCustoFixoItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult custoFixoLinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (custoFixoLinha != null && custoFixoLinha.LINHA != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + custoFixoLinha.LINHA.Trim().ToUpper() + "</font>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.JANEIRO, 1)) + "";

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.FEVEREIRO, 2)) + "";

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.MARCO, 3)) + "";

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.ABRIL, 4)) + "";

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.MAIO, 5)) + "";

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.JUNHO, 6)) + "";

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.JULHO, 7)) + "";

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.AGOSTO, 8)) + "";

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.SETEMBRO, 9)) + "";

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.OUTUBRO, 10)) + "";

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.NOVEMBRO, 11)) + "";

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = "&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.DEZEMBRO, 12)) + "";
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(custoFixoLinha.TOTAL);

                        System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                        if (img != null)
                        {
                            img.Visible = false;

                            GridView gvCustoFixoItemSub = e.Row.FindControl("gvCustoFixoItemSub") as GridView;
                            if (gvCustoFixoItemSub != null)
                            {
                                gvCustoFixoItemSub.DataSource = gListaCustoFixo.Where(p => p.LINHA.Trim().ToUpper() == custoFixoLinha.LINHA.Trim().ToUpper()).GroupBy(j => new
                                {
                                    TIPO = j.TIPO
                                }).Select(k => new SP_OBTER_DREResult
                                {
                                    TIPO = k.Key.TIPO.Trim().ToUpper(),
                                    JANEIRO = k.Sum(s => s.JANEIRO),
                                    FEVEREIRO = k.Sum(s => s.FEVEREIRO),
                                    MARCO = k.Sum(s => s.MARCO),
                                    ABRIL = k.Sum(s => s.ABRIL),
                                    MAIO = k.Sum(s => s.MAIO),
                                    JUNHO = k.Sum(s => s.JUNHO),
                                    JULHO = k.Sum(s => s.JULHO),
                                    AGOSTO = k.Sum(s => s.AGOSTO),
                                    SETEMBRO = k.Sum(s => s.SETEMBRO),
                                    OUTUBRO = k.Sum(s => s.OUTUBRO),
                                    NOVEMBRO = k.Sum(s => s.NOVEMBRO),
                                    DEZEMBRO = k.Sum(s => s.DEZEMBRO),
                                    TOTAL = k.Sum(s => s.TOTAL)
                                });
                                gvCustoFixoItemSub.DataBind();
                            }
                            img.Visible = true;

                        }

                    }
                }
            }
        }
        protected void gvCustoFixoItem_DataBound(object sender, EventArgs e)
        {
            GridView gvCustoFixoItem = (GridView)sender;
            if (gvCustoFixoItem != null)
                if (gvCustoFixoItem.HeaderRow != null)
                    gvCustoFixoItem.HeaderRow.Visible = false;
        }

        protected void gvCustoFixoItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string tipoNome = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.TIPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            tipoNome = subitem.TIPO.Trim().ToUpper().Replace("SERVIÇOS", "").Replace("SERVIÇO", "");
                            _litSubItem.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;" + (((tipoNome.Length < 16) ? tipoNome : tipoNome.Substring(0, 16))) + "</font></span> ";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12));

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);

                    }
                }
            }
        }
        protected void gvCustoFixoItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvCustoFixoItemSub = (GridView)sender;
            if (gvCustoFixoItemSub != null)
                if (gvCustoFixoItemSub.HeaderRow != null)
                    gvCustoFixoItemSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvCustoFixoItemSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "MARGEM BRUTA"

        protected void gvMargemBruta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult margemBruta = e.Row.DataItem as SP_OBTER_DREResult;

                    if (margemBruta != null && margemBruta.GRUPO != "")
                    {
                        Literal _litMargemBruta = e.Row.FindControl("litMargemBruta") as Literal;
                        if (_litMargemBruta != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litMargemBruta.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + margemBruta.LINHA + " " + margemBruta.GRUPO.Trim().ToUpper() + "</i></font></span>";
                            else
                                _litMargemBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + margemBruta.GRUPO.Trim().ToUpper() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JANEIRO, 1)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _janeiro.Text = FormatarValor(FiltroValor(margemBruta.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.FEVEREIRO, 2)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _fevereiro.Text = FormatarValor(FiltroValor(margemBruta.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.MARCO, 3)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _marco.Text = FormatarValor(FiltroValor(margemBruta.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.ABRIL, 4)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _abril.Text = FormatarValor(FiltroValor(margemBruta.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.MAIO, 5)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _maio.Text = FormatarValor(FiltroValor(margemBruta.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JUNHO, 6)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _junho.Text = FormatarValor(FiltroValor(margemBruta.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JULHO, 7)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _julho.Text = FormatarValor(FiltroValor(margemBruta.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.AGOSTO, 8)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _agosto.Text = FormatarValor(FiltroValor(margemBruta.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.SETEMBRO, 9)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _setembro.Text = FormatarValor(FiltroValor(margemBruta.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.OUTUBRO, 10)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _outubro.Text = FormatarValor(FiltroValor(margemBruta.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.NOVEMBRO, 11)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _novembro.Text = FormatarValor(FiltroValor(margemBruta.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            if (margemBruta.LINHA.Contains("%") || (margemBruta.LINHA.Trim() == "MARK-UP"))
                                _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.DEZEMBRO, 12)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _dezembro.Text = FormatarValor(FiltroValor(margemBruta.DEZEMBRO, 12)).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            if (margemBruta.LINHA.Contains("%") || (margemBruta.LINHA.Trim() == "MARK-UP"))
                                _total.Text = "<span style='font-weight:normal'>" + FormatarValor(margemBruta.TOTAL).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _total.Text = FormatarValor(margemBruta.TOTAL).Trim();


                    }

                    if (margemBruta.GRUPO == "")
                        e.Row.Height = Unit.Pixel(1);

                }
            }
        }
        protected void gvMargemBruta_DataBound(object sender, EventArgs e)
        {
            if (gvReceitaLiquida.Rows.Count >= 5)
            {
                //Tratamento de cores para linhas
                GridViewRow firstBlankRow = gvMargemBruta.Rows[gvMargemBruta.Rows.Count - 3];
                if (firstBlankRow != null)
                    firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";

                GridViewRow lastBlankRow = gvMargemBruta.Rows[gvMargemBruta.Rows.Count - 1];
                if (lastBlankRow != null)
                {
                    //Tratamento para cor
                    lastBlankRow.BackColor = corTitulo;
                    int count = gvMargemBruta.Columns.Count;
                    for (int i = 0; i < count; i++)
                        lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);

                    lastBlankRow.Attributes["style"] = "line-height: 2px;";
                }

                GridViewRow headerRow = gvMargemBruta.HeaderRow;
                if (headerRow != null)
                    headerRow.Attributes["style"] = "line-height: 3px;";
            }
        }

        #endregion

        #region "DESPESAS CTO"

        protected void gvDespesaCTO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult desploja = e.Row.DataItem as SP_OBTER_DREResult;

                    if (desploja != null && desploja.GRUPO != "")
                    {
                        int ano = Convert.ToInt32(ddlAno.SelectedValue);


                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                        {
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + desploja.GRUPO.Trim().ToUpper() + "</font> ";
                        }
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(desploja.JANEIRO, 1), 1, 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(desploja.FEVEREIRO, 2), 2, 1);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(desploja.MARCO, 3), 3, 1);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(desploja.ABRIL, 4), 4, 1);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(desploja.MAIO, 5), 5, 1);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(desploja.JUNHO, 6), 6, 1);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(desploja.JULHO, 7), 7, 1);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(desploja.AGOSTO, 8), 8, 1);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(desploja.SETEMBRO, 9), 9, 1);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(desploja.OUTUBRO, 10), 10, 1);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(desploja.NOVEMBRO, 11), 11, 1);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(desploja.DEZEMBRO, 12), 12, 1);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(desploja.TOTAL, 0);

                    }

                    if (desploja.GRUPO == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (desploja.ID == 1)
                        {
                            GridView gvDespesaCTOItem = e.Row.FindControl("gvDespesaCTOItem") as GridView;
                            if (gvDespesaCTOItem != null)
                            {
                                var despCTOLinha = gListaDespesaCTO;
                                despCTOLinha = despCTOLinha.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).OrderBy(y => y.LINHA).ToList();
                                gvDespesaCTOItem.DataSource = despCTOLinha;
                                gvDespesaCTOItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvDespesaCTO_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvDespesaCTO.Rows[gvDespesaCTO.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvDespesaCTO.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvDespesaCTO.Rows[gvDespesaCTO.Rows.Count - 2];
            if (lastRow != null)
                lastRow.Visible = false;

            GridViewRow headerRow = gvDespesaCTO.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";
        }

        protected void gvDespesaCTOItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult desplojalinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (desplojalinha != null && desplojalinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;" + (((desplojalinha.LINHA.Trim().Length < 18) ? desplojalinha.LINHA.Trim() : desplojalinha.LINHA.Trim().Substring(0, 18))) + "</font></span>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(desplojalinha.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(desplojalinha.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(desplojalinha.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(desplojalinha.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(desplojalinha.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(desplojalinha.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(desplojalinha.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(desplojalinha.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(desplojalinha.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(desplojalinha.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(desplojalinha.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(desplojalinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(desplojalinha.TOTAL);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        //img.Visible = false;
                        GridView gvDespesaCTOItemSub = e.Row.FindControl("gvDespesaCTOItemSub") as GridView;
                        if (gvDespesaCTOItemSub != null)
                        {
                            gvDespesaCTOItemSub.DataSource = gListaDespesaCTO.Where(p => p.LINHA.Trim().ToUpper() == desplojalinha.LINHA.Trim().ToUpper());
                            gvDespesaCTOItemSub.DataBind();
                        }
                        //img.Visible = true;
                    }
                }
            }
        }
        protected void gvDespesaCTOItem_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaCTOItem = (GridView)sender;
            if (gvDespesaCTOItem != null)
                if (gvDespesaCTOItem.HeaderRow != null)
                    gvDespesaCTOItem.HeaderRow.Visible = false;
        }

        protected void gvDespesaCTOItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);

                    }

                }
            }
        }
        protected void gvDespesaCTOItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaCTOItemSub = (GridView)sender;
            if (gvDespesaCTOItemSub != null)
                if (gvDespesaCTOItemSub.HeaderRow != null)
                    gvDespesaCTOItemSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvDespesaCTOItemSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "DESPESAS ESPEFICICAS"

        protected void gvDespesaEspecifica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult desploja = e.Row.DataItem as SP_OBTER_DREResult;

                    if (desploja != null && desploja.GRUPO != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                        {
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + desploja.GRUPO.Trim().ToUpper() + "</font> ";
                        }
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(desploja.JANEIRO, 1), 1, 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(desploja.FEVEREIRO, 2), 2, 1);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(desploja.MARCO, 3), 3, 1);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(desploja.ABRIL, 4), 4, 1);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(desploja.MAIO, 5), 5, 1);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(desploja.JUNHO, 6), 6, 1);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(desploja.JULHO, 7), 7, 1);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(desploja.AGOSTO, 8), 8, 1);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(desploja.SETEMBRO, 9), 9, 1);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(desploja.OUTUBRO, 10), 10, 1);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(desploja.NOVEMBRO, 11), 11, 1);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(desploja.DEZEMBRO, 12), 12, 1);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(desploja.TOTAL, 0, 1);

                    }

                    if (desploja.GRUPO == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (desploja.ID == 1)
                        {
                            GridView gvDespesaEspecificaItem = e.Row.FindControl("gvDespesaEspecificaItem") as GridView;
                            if (gvDespesaEspecificaItem != null)
                            {
                                var despEspLinha = gListaDespesaEspecifica;
                                despEspLinha = despEspLinha.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).OrderBy(y => y.LINHA).ToList();
                                gvDespesaEspecificaItem.DataSource = despEspLinha;
                                gvDespesaEspecificaItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvDespesaEspecifica_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvDespesaEspecifica.Rows[gvDespesaEspecifica.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvDespesaEspecifica.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvDespesaEspecifica.Rows[gvDespesaEspecifica.Rows.Count - 2];
            if (lastRow != null)
                lastRow.Visible = false;

            GridViewRow headerRow = gvDespesaEspecifica.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";
        }

        protected void gvDespesaEspecificaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult desplojalinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (desplojalinha != null && desplojalinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;" + (((desplojalinha.LINHA.Trim().Length < 18) ? desplojalinha.LINHA.Trim() : desplojalinha.LINHA.Trim().Substring(0, 18))) + "</font></span>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(desplojalinha.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(desplojalinha.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(desplojalinha.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(desplojalinha.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(desplojalinha.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(desplojalinha.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(desplojalinha.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(desplojalinha.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(desplojalinha.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(desplojalinha.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(desplojalinha.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(desplojalinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(desplojalinha.TOTAL);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        //img.Visible = false;
                        GridView gvDespesaEspecificaItemSub = e.Row.FindControl("gvDespesaEspecificaItemSub") as GridView;
                        if (gvDespesaEspecificaItemSub != null)
                        {
                            gvDespesaEspecificaItemSub.DataSource = gListaDespesaEspecifica.Where(p => p.LINHA.Trim().ToUpper() == desplojalinha.LINHA.Trim().ToUpper());
                            gvDespesaEspecificaItemSub.DataBind();
                        }
                        //img.Visible = true;
                    }
                }
            }
        }
        protected void gvDespesaEspecificaItem_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaEspecificaItem = (GridView)sender;
            if (gvDespesaEspecificaItem != null)
                if (gvDespesaEspecificaItem.HeaderRow != null)
                    gvDespesaEspecificaItem.HeaderRow.Visible = false;
        }

        protected void gvDespesaEspecificaItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);

                    }

                }
            }
        }
        protected void gvDespesaEspecificaItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaEspecificaItemSub = (GridView)sender;
            if (gvDespesaEspecificaItemSub != null)
                if (gvDespesaEspecificaItemSub.HeaderRow != null)
                    gvDespesaEspecificaItemSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvDespesaEspecificaItemSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "DESPESAS VENDAS"

        protected void gvDespesaVenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult despvenda = e.Row.DataItem as SP_OBTER_DREResult;

                    if (despvenda != null && despvenda.GRUPO != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despvenda.GRUPO.Trim().ToUpper() + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(despvenda.JANEIRO, 1), 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(despvenda.FEVEREIRO, 2), 2);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(despvenda.MARCO, 3), 3);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(despvenda.ABRIL, 4), 4);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(despvenda.MAIO, 5), 5);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(despvenda.JUNHO, 6), 6);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(despvenda.JULHO, 7), 7);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(despvenda.AGOSTO, 8), 8);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(despvenda.SETEMBRO, 9), 9);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(despvenda.OUTUBRO, 10), 10);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(despvenda.NOVEMBRO, 11), 11);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(despvenda.DEZEMBRO, 12), 12);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(despvenda.TOTAL, 0);

                    }

                    if (despvenda.GRUPO == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (despvenda.ID == 1)
                        {
                            GridView gvDespesaVendaItem = e.Row.FindControl("gvDespesaVendaItem") as GridView;
                            if (gvDespesaVendaItem != null)
                            {
                                var despVendaLinha = gListaDespesaVenda;
                                despVendaLinha = despVendaLinha.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvDespesaVendaItem.DataSource = despVendaLinha;
                                gvDespesaVendaItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvDespesaVenda_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvDespesaVenda.Rows[gvDespesaVenda.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvDespesaVenda.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvDespesaVenda.Rows[gvDespesaVenda.Rows.Count - 2];
            if (lastRow != null)
                lastRow.Visible = false;

            GridViewRow headerRow = gvDespesaVenda.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";
        }

        protected void gvDespesaVendaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult despvendalinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (despvendalinha != null && despvendalinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span><font size='2' face='Calibri'>&nbsp;" + despvendalinha.LINHA.Trim().ToUpper().Substring(0, ((despvendalinha.LINHA.Trim().Length > 19) ? 19 : despvendalinha.LINHA.Trim().Length)) + "</font></span>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(despvendalinha.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(despvendalinha.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(despvendalinha.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(despvendalinha.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(despvendalinha.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(despvendalinha.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(despvendalinha.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(despvendalinha.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(despvendalinha.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(despvendalinha.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(despvendalinha.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(despvendalinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(despvendalinha.TOTAL);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        //img.Visible = false;

                        GridView gvDespesaVendaItemSub = e.Row.FindControl("gvDespesaVendaItemSub") as GridView;
                        if (gvDespesaVendaItemSub != null)
                        {
                            gvDespesaVendaItemSub.DataSource = gListaDespesaVenda.Where(p => p.LINHA.Trim().ToUpper() == despvendalinha.LINHA.Trim().ToUpper()).GroupBy(j => new
                            {
                                LINHA = j.LINHA,
                                TIPO = j.TIPO
                            }).Select(k => new SP_OBTER_DREResult
                            {
                                LINHA = k.Key.LINHA.Trim().ToUpper(),
                                TIPO = k.Key.TIPO.Trim().ToUpper(),
                                JANEIRO = k.Sum(s => s.JANEIRO),
                                FEVEREIRO = k.Sum(s => s.FEVEREIRO),
                                MARCO = k.Sum(s => s.MARCO),
                                ABRIL = k.Sum(s => s.ABRIL),
                                MAIO = k.Sum(s => s.MAIO),
                                JUNHO = k.Sum(s => s.JUNHO),
                                JULHO = k.Sum(s => s.JULHO),
                                AGOSTO = k.Sum(s => s.AGOSTO),
                                SETEMBRO = k.Sum(s => s.SETEMBRO),
                                OUTUBRO = k.Sum(s => s.OUTUBRO),
                                NOVEMBRO = k.Sum(s => s.NOVEMBRO),
                                DEZEMBRO = k.Sum(s => s.DEZEMBRO),
                                TOTAL = k.Sum(s => s.TOTAL)
                            });
                            gvDespesaVendaItemSub.DataBind();
                        }
                        //img.Visible = true;

                    }
                }
            }
        }
        protected void gvDespesaVendaItem_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaVendaItem = (GridView)sender;
            if (gvDespesaVendaItem != null)
                if (gvDespesaVendaItem.HeaderRow != null)
                    gvDespesaVendaItem.HeaderRow.Visible = false;
        }

        protected void gvDespesaVendaItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string item = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.TIPO != "")
                    {
                        //Inicialização das variaveis
                        link = "";
                        ctbContabil = "";
                        ano = ddlAno.SelectedItem.Text.Trim();
                        filial = ddlFilial.SelectedValue.Trim();
                        var t = "dVen";

                        var conta = desenvController.ObterContaContabilDescricao(subitem.TIPO.Trim()).Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 1) == "3").ToList();
                        if (conta != null && conta.Count() == 1)
                            ctbContabil = conta[0].CONTA_CONTABIL.Trim().Replace(".", "@");

                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            item = subitem.TIPO.Trim().ToUpper().Replace("SOBRE ", "");
                            _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;" + item.Substring(0, ((item.Length > 18) ? 18 : item.Length)) + "</font> "; //20
                        }

                        //Inicio campos
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=1&a=" + ano + "&t=" + t) : "javascript:;";
                            _janeiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(subitem.JANEIRO, 1)) + "</a>";
                        }

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=2&a=" + ano + "&t=" + t) : "javascript:;";
                            _fevereiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)) + "</a>";
                        }

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=3&a=" + ano + "&t=" + t) : "javascript:;";
                            _marco.Text = "<a href='" + link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(subitem.MARCO, 3)) + "</a>";
                        }

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=4&a=" + ano + "&t=" + t) : "javascript:;";
                            _abril.Text = "<a href='" + link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(subitem.ABRIL, 4)) + "</a>";
                        }

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=5&a=" + ano + "&t=" + t) : "javascript:;";
                            _maio.Text = "<a href='" + link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(subitem.MAIO, 5)) + "</a>";
                        }

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=6&a=" + ano + "&t=" + t) : "javascript:;";
                            _junho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(subitem.JUNHO, 6)) + "</a>";
                        }

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=7&a=" + ano + "&t=" + t) : "javascript:;";
                            _julho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(subitem.JULHO, 7)) + "</a>";
                        }

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=8&a=" + ano + "&t=" + t) : "javascript:;";
                            _agosto.Text = "<a href='" + link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(subitem.AGOSTO, 8)) + "</a>";
                        }

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=9&a=" + ano + "&t=" + t) : "javascript:;";
                            _setembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.SETEMBRO, 9)) + "</a>";
                        }

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=10&a=" + ano + "&t=" + t) : "javascript:;";
                            _outubro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(subitem.OUTUBRO, 10)) + "</a>";
                        }

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=11&a=" + ano + "&t=" + t) : "javascript:;";
                            _novembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)) + "</a>";
                        }

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            link = (subitem.LINHA == "DESPESAS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=12&a=" + ano + "&t=" + t) : "javascript:;";
                            _dezembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)) + "</a>";
                        }

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);

                    }
                }
            }
        }
        protected void gvDespesaVendaItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaVendaItemSub = (GridView)sender;
            if (gvDespesaVendaItemSub != null)
                if (gvDespesaVendaItemSub.HeaderRow != null)
                    gvDespesaVendaItemSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvDespesaVendaItemSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "DESPESAS ADMINISTRATIVAS"

        protected void gvDespesaAdm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult despadm = e.Row.DataItem as SP_OBTER_DREResult;

                    if (despadm != null && despadm.GRUPO != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                        {
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despadm.GRUPO.Trim().ToUpper() + "</font> ";
                        }
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(despadm.JANEIRO, 1), 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(despadm.FEVEREIRO, 2), 2);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(despadm.MARCO, 3), 3);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(despadm.ABRIL, 4), 4);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(despadm.MAIO, 5), 5);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(despadm.JUNHO, 6), 6);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(despadm.JULHO, 7), 7);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(despadm.AGOSTO, 8), 8);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(despadm.SETEMBRO, 9), 9);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(despadm.OUTUBRO, 10), 10);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(despadm.NOVEMBRO, 11), 11);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(despadm.DEZEMBRO, 12), 12);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(despadm.TOTAL, 0);

                    }

                    if (despadm.GRUPO == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (despadm.ID == 1)
                        {
                            GridView gvDespesaAdmItem = e.Row.FindControl("gvDespesaAdmItem") as GridView;
                            if (gvDespesaAdmItem != null)
                            {
                                var despAdmPessoal = gListaDespesaADM;
                                despAdmPessoal = despAdmPessoal.GroupBy(b => new { LINHA = b.LINHA.Trim() }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvDespesaAdmItem.DataSource = despAdmPessoal;
                                gvDespesaAdmItem.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvDespesaAdm_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvDespesaAdm.Rows[gvDespesaAdm.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvDespesaAdm.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvDespesaAdm.Rows[gvDespesaAdm.Rows.Count - 2];
            if (lastRow != null)
                lastRow.Visible = false;

            GridViewRow headerRow = gvDespesaAdm.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";
        }

        protected void gvDespesaAdmItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult despadmlinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (despadmlinha != null && despadmlinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despadmlinha.LINHA.Trim().ToUpper().Substring(0, ((despadmlinha.LINHA.Trim().Length > 20) ? 20 : despadmlinha.LINHA.Trim().Length)) + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(despadmlinha.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(despadmlinha.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(despadmlinha.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(despadmlinha.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(despadmlinha.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(despadmlinha.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(despadmlinha.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(despadmlinha.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(despadmlinha.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(despadmlinha.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(despadmlinha.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(despadmlinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(despadmlinha.TOTAL);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;


                        if (despadmlinha.ID == 1)
                        {
                            GridView gvDespesaAdmItemSub = e.Row.FindControl("gvDespesaAdmItemSub") as GridView;
                            if (gvDespesaAdmItemSub != null)
                            {
                                var lstdespAdmLinha = gListaDespesaADM.Where(p => p.LINHA.ToUpper().Contains(despadmlinha.LINHA.ToUpper())).GroupBy(j => new
                                {
                                    LINHA = j.LINHA,
                                    TIPO = j.TIPO
                                }).Select(k => new SP_OBTER_DREResult
                                {
                                    ID = 1,
                                    LINHA = k.Key.LINHA.Trim().ToUpper(),
                                    TIPO = k.Key.TIPO.Trim().ToUpper(),
                                    JANEIRO = k.Sum(s => s.JANEIRO),
                                    FEVEREIRO = k.Sum(s => s.FEVEREIRO),
                                    MARCO = k.Sum(s => s.MARCO),
                                    ABRIL = k.Sum(s => s.ABRIL),
                                    MAIO = k.Sum(s => s.MAIO),
                                    JUNHO = k.Sum(s => s.JUNHO),
                                    JULHO = k.Sum(s => s.JULHO),
                                    AGOSTO = k.Sum(s => s.AGOSTO),
                                    SETEMBRO = k.Sum(s => s.SETEMBRO),
                                    OUTUBRO = k.Sum(s => s.OUTUBRO),
                                    NOVEMBRO = k.Sum(s => s.NOVEMBRO),
                                    DEZEMBRO = k.Sum(s => s.DEZEMBRO),
                                    TOTAL = k.Sum(s => s.TOTAL)
                                }).ToList();

                                gvDespesaAdmItemSub.DataSource = lstdespAdmLinha.OrderBy(p => p.TIPO).ThenBy(x => x.LINHA);
                                gvDespesaAdmItemSub.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvDespesaAdmItem_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaAdmItem = (GridView)sender;
            if (gvDespesaAdmItem != null)
                if (gvDespesaAdmItem.HeaderRow != null)
                    gvDespesaAdmItem.HeaderRow.Visible = false;
        }

        protected void gvDespesaAdmItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.TIPO != "")
                    {
                        //Inicialização das variaveis
                        link = "";
                        ctbContabil = "";
                        ano = ddlAno.SelectedItem.Text.Trim();
                        filial = ddlFilial.SelectedValue.Trim();
                        var t = "dAdm";

                        var conta = desenvController.ObterContaContabilDescricao(subitem.TIPO.Trim()).Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 1) == "3").ToList();
                        if (conta != null && conta.Count() == 1)
                            ctbContabil = conta[0].CONTA_CONTABIL.Trim().Replace(".", "@");

                        //Inicio campos
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.TIPO.Trim().ToUpper().Substring(0, ((subitem.TIPO.Trim().Length > 21) ? 21 : subitem.TIPO.Trim().Length)) + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=1&a=" + ano + "&t=" + t) : "javascript:;";
                            _janeiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(subitem.JANEIRO, 1)) + "</a>";
                        }

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=2&a=" + ano + "&t=" + t) : "javascript:;";
                            _fevereiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)) + "</a>";
                        }

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=3&a=" + ano + "&t=" + t) : "javascript:;";
                            _marco.Text = "<a href='" + link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(subitem.MARCO, 3)) + "</a>";
                        }

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=4&a=" + ano + "&t=" + t) : "javascript:;";
                            _abril.Text = "<a href='" + link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(subitem.ABRIL, 4)) + "</a>";
                        }

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=5&a=" + ano + "&t=" + t) : "javascript:;";
                            _maio.Text = "<a href='" + link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(subitem.MAIO, 5)) + "</a>";
                        }

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=6&a=" + ano + "&t=" + t) : "javascript:;";
                            _junho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(subitem.JUNHO, 6)) + "</a>";
                        }

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=7&a=" + ano + "&t=" + t) : "javascript:;";
                            _julho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(subitem.JULHO, 7)) + "</a>";
                        }

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=8&a=" + ano + "&t=" + t) : "javascript:;";
                            _agosto.Text = "<a href='" + link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(subitem.AGOSTO, 8)) + "</a>";
                        }

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=9&a=" + ano + "&t=" + t) : "javascript:;";
                            _setembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.SETEMBRO, 9)) + "</a>";
                        }

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=10&a=" + ano + "&t=" + t) : "javascript:;";
                            _outubro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(subitem.OUTUBRO, 10)) + "</a>";
                        }

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=11&a=" + ano + "&t=" + t) : "javascript:;";
                            _novembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)) + "</a>";
                        }

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            link = (subitem.LINHA != "SALÁRIOS") ? ("dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=12&a=" + ano + "&t=" + t) : "javascript:;";
                            _dezembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)) + "</a>";
                        }

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);
                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;

                        GridView gvDespesaAdmItemSubSub = e.Row.FindControl("gvDespesaAdmItemSubSub") as GridView;
                        if (gvDespesaAdmItemSubSub != null)
                        {
                            var lstdespAdmSub = gListaDespesaADM.Where(g => g.LINHA.Trim().ToUpper().Contains(subitem.LINHA.Trim().ToUpper()) && g.TIPO.ToUpper().Contains(subitem.TIPO)).GroupBy(j => new
                            {
                                LINHA = j.LINHA.Trim().ToUpper(),
                                TIPO = j.TIPO.Trim().ToUpper(),
                                FILIAL = j.FILIAL.Trim().ToUpper()
                            }).Select(k => new SP_OBTER_DREResult
                            {
                                ID = 1,
                                LINHA = k.Key.LINHA.Trim().ToUpper(),
                                TIPO = k.Key.TIPO.Trim().ToUpper(),
                                FILIAL = k.Key.FILIAL.Trim().ToUpper(),
                                JANEIRO = k.Sum(s => s.JANEIRO),
                                FEVEREIRO = k.Sum(s => s.FEVEREIRO),
                                MARCO = k.Sum(s => s.MARCO),
                                ABRIL = k.Sum(s => s.ABRIL),
                                MAIO = k.Sum(s => s.MAIO),
                                JUNHO = k.Sum(s => s.JUNHO),
                                JULHO = k.Sum(s => s.JULHO),
                                AGOSTO = k.Sum(s => s.AGOSTO),
                                SETEMBRO = k.Sum(s => s.SETEMBRO),
                                OUTUBRO = k.Sum(s => s.OUTUBRO),
                                NOVEMBRO = k.Sum(s => s.NOVEMBRO),
                                DEZEMBRO = k.Sum(s => s.DEZEMBRO),
                                TOTAL = k.Sum(s => s.TOTAL)
                            }).ToList();

                            gvDespesaAdmItemSubSub.DataSource = lstdespAdmSub;
                            gvDespesaAdmItemSubSub.DataBind();
                        }
                        img.Visible = true;

                    }
                }
            }
        }
        protected void gvDespesaAdmItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaAdmItemSub = (GridView)sender;
            if (gvDespesaAdmItemSub != null)
                if (gvDespesaAdmItemSub.HeaderRow != null)
                    gvDespesaAdmItemSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvDespesaAdmItemSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        protected void gvDespesaAdmItemSubSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult subitem = e.Row.DataItem as SP_OBTER_DREResult;

                    if (subitem != null && subitem.FILIAL != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + subitem.FILIAL.Trim().ToUpper().Substring(0, ((subitem.FILIAL.Trim().Length > 20) ? 20 : subitem.FILIAL.Trim().Length)) + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12));

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(subitem.TOTAL);

                    }
                }
            }
        }
        protected void gvDespesaAdmItemSubSub_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaAdmItemSubSub = (GridView)sender;
            if (gvDespesaAdmItemSubSub != null)
                if (gvDespesaAdmItemSubSub.HeaderRow != null)
                    gvDespesaAdmItemSubSub.HeaderRow.Visible = false;

            foreach (GridViewRow row in gvDespesaAdmItemSubSub.Rows)
                row.Attributes["style"] = "border: 1px solid #FFF";
        }

        #endregion

        #region "EBITDA"

        protected void gvEbtida_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult ebtida = e.Row.DataItem as SP_OBTER_DREResult;

                    if (ebtida != null && ebtida.GRUPO != "")
                    {
                        Literal _litEbtida = e.Row.FindControl("litEbtida") as Literal;
                        if (_litEbtida != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litEbtida.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>% " + ebtida.GRUPO.Trim() + "</i></font></span>";
                            else
                                _litEbtida.Text = "<font size='2' face='Calibri'>&nbsp;" + ebtida.GRUPO.Trim() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            if (ebtida.LINHA.Contains("%"))
                                _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.JANEIRO, 1)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.JANEIRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _janeiro.Text = FormatarValor(FiltroValor(ebtida.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            if (ebtida.LINHA.Contains("%"))
                                _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.FEVEREIRO, 2)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.FEVEREIRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _fevereiro.Text = FormatarValor(FiltroValor(ebtida.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            if (ebtida.LINHA.Contains("%"))
                                _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.MARCO, 3)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.MARCO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _marco.Text = FormatarValor(FiltroValor(ebtida.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            if (ebtida.LINHA.Contains("%"))
                                _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.ABRIL, 4)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.ABRIL < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _abril.Text = FormatarValor(FiltroValor(ebtida.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            if (ebtida.LINHA.Contains("%"))
                                _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.MAIO, 5)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.MAIO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _maio.Text = FormatarValor(FiltroValor(ebtida.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            if (ebtida.LINHA.Contains("%"))
                                _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.JUNHO, 6)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.JUNHO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _junho.Text = FormatarValor(FiltroValor(ebtida.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            if (ebtida.LINHA.Contains("%"))
                                _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.JULHO, 7)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.JULHO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _julho.Text = FormatarValor(FiltroValor(ebtida.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            if (ebtida.LINHA.Contains("%"))
                                _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.AGOSTO, 8)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.AGOSTO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _agosto.Text = FormatarValor(FiltroValor(ebtida.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            if (ebtida.LINHA.Contains("%"))
                                _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.SETEMBRO, 9)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.SETEMBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _setembro.Text = FormatarValor(FiltroValor(ebtida.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            if (ebtida.LINHA.Contains("%"))
                                _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.OUTUBRO, 10)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.OUTUBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _outubro.Text = FormatarValor(FiltroValor(ebtida.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            if (ebtida.LINHA.Contains("%"))
                                _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.NOVEMBRO, 11)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.NOVEMBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _novembro.Text = FormatarValor(FiltroValor(ebtida.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            if (ebtida.LINHA.Contains("%"))
                                _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.DEZEMBRO, 12)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.DEZEMBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _dezembro.Text = FormatarValor(FiltroValor(ebtida.DEZEMBRO, 12)).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            if (ebtida.LINHA.Contains("%"))
                                _total.Text = "<span style='font-weight:normal'>" + FormatarValor(ebtida.TOTAL).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.DEZEMBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _total.Text = FormatarValor(ebtida.TOTAL).Trim();

                    }

                    if (ebtida.GRUPO == "")
                        e.Row.Height = Unit.Pixel(1);

                }
            }
        }
        protected void gvEbtida_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvEbtida.Rows[gvEbtida.Rows.Count - 3];
            if (firstBlankRow != null)
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";

            GridViewRow lastBlankRow = gvEbtida.Rows[gvEbtida.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = corTitulo;
                int count = gvEbtida.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
                lastBlankRow.Attributes["style"] = "line-height: 2px;";
            }

            GridViewRow headerRow = gvEbtida.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 3px;";
        }

        #endregion

        #region "DEPRECIAÇÃO E AMORTIZAÇÃO"

        protected void gvDepreciacaoAmortizacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult depreciacao = e.Row.DataItem as SP_OBTER_DREResult;

                    if (depreciacao != null && depreciacao.GRUPO != "")
                    {
                        Literal _litDepreciacao = e.Row.FindControl("litDepreciacao") as Literal;
                        if (_litDepreciacao != null)
                            _litDepreciacao.Text = "<font size='2' face='Calibri'>&nbsp;" + depreciacao.GRUPO.Trim().Substring(0, depreciacao.GRUPO.Trim().Length - 2) + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(depreciacao.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(depreciacao.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(depreciacao.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(depreciacao.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(depreciacao.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(depreciacao.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(depreciacao.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(depreciacao.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(depreciacao.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(depreciacao.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(depreciacao.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(depreciacao.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(depreciacao.TOTAL);

                    }

                    if (depreciacao.GRUPO == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (depreciacao.ID == 1)
                        {
                            GridView gvDepreciacaoAmortizacaoItem = e.Row.FindControl("gvDepreciacaoAmortizacaoItem") as GridView;
                            if (gvDepreciacaoAmortizacaoItem != null)
                            {
                                var depreciacaoAmortizacao = gListaDepreciacao.Where(i => i.GRUPO.Trim().ToUpper() == depreciacao.GRUPO.ToUpper().Trim()).GroupBy(b => new { TIPO = b.TIPO }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            TIPO = k.Key.TIPO.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvDepreciacaoAmortizacaoItem.DataSource = depreciacaoAmortizacao;
                                gvDepreciacaoAmortizacaoItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvDepreciacaoAmortizacao_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvDepreciacaoAmortizacao.Rows[gvDepreciacaoAmortizacao.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvDepreciacaoAmortizacao.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow headerRow = gvDepreciacaoAmortizacao.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";

        }

        protected void gvDepreciacaoAmortizacaoItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult depreciacaoLinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (depreciacaoLinha != null && depreciacaoLinha.TIPO != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + depreciacaoLinha.TIPO.Trim().ToUpper().Replace("DEPRECIAÇÃO", "").Substring(0, ((depreciacaoLinha.TIPO.Trim().Replace("DEPRECIAÇÃO", "").Length > 22) ? 22 : depreciacaoLinha.TIPO.Replace("DEPRECIAÇÃO", "").Trim().Length)) + "</font></span>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(depreciacaoLinha.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(depreciacaoLinha.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(depreciacaoLinha.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(depreciacaoLinha.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(depreciacaoLinha.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(depreciacaoLinha.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(depreciacaoLinha.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(depreciacaoLinha.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(depreciacaoLinha.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(depreciacaoLinha.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(depreciacaoLinha.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(depreciacaoLinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(depreciacaoLinha.TOTAL);

                    }
                }
            }
        }
        protected void gvDepreciacaoAmortizacaoItem_DataBound(object sender, EventArgs e)
        {
            GridView gvDepreciacaoAmortizacaoItem = (GridView)sender;
            if (gvDepreciacaoAmortizacaoItem != null)
                if (gvDepreciacaoAmortizacaoItem.HeaderRow != null)
                    gvDepreciacaoAmortizacaoItem.HeaderRow.Visible = false;
        }

        #endregion

        #region "DESPESAS EVENTUAIS"

        protected void gvDespesaEventual_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult eventual = e.Row.DataItem as SP_OBTER_DREResult;

                    if (eventual != null && eventual.GRUPO != "")
                    {
                        Literal _litEventual = e.Row.FindControl("litEventual") as Literal;
                        if (_litEventual != null)
                            _litEventual.Text = "<font size='2' face='Calibri'>&nbsp;" + eventual.GRUPO.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(eventual.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(eventual.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(eventual.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(eventual.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(eventual.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(eventual.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(eventual.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(eventual.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(eventual.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(eventual.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(eventual.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(eventual.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(eventual.TOTAL);

                    }

                    if (eventual.GRUPO == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (eventual.ID == 22)
                        {
                            GridView gvDespesaEventualItem = e.Row.FindControl("gvDespesaEventualItem") as GridView;
                            if (gvDespesaEventualItem != null)
                            {
                                var eventualDespesa = gListaDespesaEventual.Where(i => i.GRUPO.Trim().ToUpper() == eventual.GRUPO.ToUpper().Trim()).GroupBy(b => new { TIPO = b.TIPO }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            TIPO = k.Key.TIPO.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvDespesaEventualItem.DataSource = eventualDespesa;
                                gvDespesaEventualItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvDespesaEventual_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvDespesaEventual.Rows[gvDespesaEventual.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvDespesaEventual.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow headerRow = gvDespesaEventual.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";

        }

        protected void gvDespesaEventualItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult eventualLinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (eventualLinha != null && eventualLinha.TIPO != "")
                    {
                        //Inicialização das variaveis
                        link = "";
                        ctbContabil = "";
                        ano = ddlAno.SelectedItem.Text.Trim();
                        filial = ddlFilial.SelectedValue.Trim();
                        var t = "dEve";

                        var conta = desenvController.ObterContaContabilDescricao(eventualLinha.TIPO.Trim()).Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 1) == "3").ToList();
                        if (conta != null && conta.Count() == 1)
                            ctbContabil = conta[0].CONTA_CONTABIL.Trim().Replace(".", "@");
                        else if (eventualLinha.TIPO.Trim() == "EXPANSÃO")
                            ctbContabil = "3@2@2@99@9999";

                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + eventualLinha.TIPO.Trim().ToUpper() + "</font></span>";

                        //Inicio campos
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=1&a=" + ano + "&t=" + t;
                            _janeiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.JANEIRO, 1)) + "</a>";
                        }

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=2&a=" + ano + "&t=" + t;
                            _fevereiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.FEVEREIRO, 2)) + "</a>";
                        }

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=3&a=" + ano + "&t=" + t;
                            _marco.Text = "<a href='" + link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.MARCO, 3)) + "</a>";
                        }

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=4&a=" + ano + "&t=" + t;
                            _abril.Text = "<a href='" + link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.ABRIL, 4)) + "</a>";
                        }

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=5&a=" + ano + "&t=" + t;
                            _maio.Text = "<a href='" + link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.MAIO, 5)) + "</a>";
                        }

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=6&a=" + ano + "&t=" + t;
                            _junho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.JUNHO, 6)) + "</a>";
                        }

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=7&a=" + ano + "&t=" + t;
                            _julho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.JULHO, 7)) + "</a>";
                        }

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=8&a=" + ano + "&t=" + t;
                            _agosto.Text = "<a href='" + link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.AGOSTO, 8)) + "</a>";
                        }

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=9&a=" + ano + "&t=" + t;
                            _setembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.SETEMBRO, 9)) + "</a>";
                        }

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=10&a=" + ano + "&t=" + t;
                            _outubro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.OUTUBRO, 10)) + "</a>";
                        }

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=11&a=" + ano + "&t=" + t;
                            _novembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.NOVEMBRO, 11)) + "</a>";
                        }

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=12&a=" + ano + "&t=" + t;
                            _dezembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.DEZEMBRO, 12)) + "</a>";
                        }

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(eventualLinha.TOTAL);

                    }
                }
            }
        }
        protected void gvDespesaEventualItem_DataBound(object sender, EventArgs e)
        {
            GridView gvDespesaEventualItem = (GridView)sender;
            if (gvDespesaEventualItem != null)
                if (gvDespesaEventualItem.HeaderRow != null)
                    gvDespesaEventualItem.HeaderRow.Visible = false;
        }

        #endregion

        #region "LUCRO BRUTO"

        protected void gvLucroBruto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult lucrobruto = e.Row.DataItem as SP_OBTER_DREResult;

                    if (lucrobruto != null && lucrobruto.GRUPO != "")
                    {
                        Literal _litLucroBruto = e.Row.FindControl("litLucroBruto") as Literal;
                        if (_litLucroBruto != null)
                            _litLucroBruto.Text = "<font size='2' face='Calibri'>&nbsp;" + lucrobruto.GRUPO.Trim().ToUpper() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(lucrobruto.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(lucrobruto.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(lucrobruto.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(lucrobruto.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(lucrobruto.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(lucrobruto.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(lucrobruto.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(lucrobruto.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(lucrobruto.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(lucrobruto.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(lucrobruto.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(lucrobruto.DEZEMBRO, 12)).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(lucrobruto.TOTAL);

                    }

                    if (lucrobruto.GRUPO == "")
                        e.Row.Height = Unit.Pixel(1);

                }
            }
        }
        protected void gvLucroBruto_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvLucroBruto.Rows[gvLucroBruto.Rows.Count - 2];
            if (firstBlankRow != null)
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";

            GridViewRow lastBlankRow = gvLucroBruto.Rows[gvLucroBruto.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = corTitulo;
                int count = gvLucroBruto.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
                lastBlankRow.Attributes["style"] = "line-height: 2px;";
            }

            GridViewRow headerRow = gvLucroBruto.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 3px;";
        }

        #endregion

        #region "RECEITAS E DESPESAS FINANCEIRAS"

        protected void gvReceitaDespesaFin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult receitadesp = e.Row.DataItem as SP_OBTER_DREResult;

                    if (receitadesp != null && receitadesp.GRUPO != "")
                    {
                        Literal _litReceitaDespesa = e.Row.FindControl("litReceitaDespesa") as Literal;
                        if (_litReceitaDespesa != null)
                            _litReceitaDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + receitadesp.GRUPO.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(receitadesp.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(receitadesp.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(receitadesp.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(receitadesp.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(receitadesp.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(receitadesp.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(receitadesp.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(receitadesp.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(receitadesp.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(receitadesp.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(receitadesp.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(receitadesp.DEZEMBRO, 12));

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(receitadesp.TOTAL);

                    }

                    if (receitadesp.GRUPO == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (receitadesp.ID == 1)
                        {
                            GridView gvReceitaDespesaFinItem = e.Row.FindControl("gvReceitaDespesaFinItem") as GridView;
                            if (gvReceitaDespesaFinItem != null)
                            {
                                var receitaDespFin = gListaReceitaDespesaFinanceira.Where(i => i.GRUPO.Trim().ToUpper() == receitadesp.GRUPO.ToUpper().Trim()).GroupBy(b => new { TIPO = b.TIPO }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            TIPO = k.Key.TIPO.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvReceitaDespesaFinItem.DataSource = receitaDespFin;
                                gvReceitaDespesaFinItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvReceitaDespesaFin_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvReceitaDespesaFin.Rows[gvReceitaDespesaFin.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvReceitaDespesaFin.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow headerRow = gvReceitaDespesaFin.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";

        }

        protected void gvReceitaDespesaFinItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult receitadespesaLinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (receitadespesaLinha != null && receitadespesaLinha.TIPO != "")
                    {
                        //Inicialização das variaveis
                        link = "";
                        ctbContabil = "";
                        ano = ddlAno.SelectedItem.Text.Trim();
                        filial = ddlFilial.SelectedValue.Trim();
                        var t = "dFin";

                        var conta = desenvController.ObterContaContabilDescricao(receitadespesaLinha.TIPO.Trim()).Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 1) == "3").ToList();
                        if (conta != null && conta.Count() == 1)
                            ctbContabil = conta[0].CONTA_CONTABIL.Trim().Replace(".", "@");

                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + receitadespesaLinha.TIPO.Trim().ToUpper().Substring(0, ((receitadespesaLinha.TIPO.Trim().Length > 22) ? 22 : receitadespesaLinha.TIPO.Trim().Length)) + "</font></span>";

                        //Inicio campos
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=1&a=" + ano + "&t=" + t;
                            _janeiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.JANEIRO, 1)) + "</a>";
                        }

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=2&a=" + ano + "&t=" + t;
                            _fevereiro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.FEVEREIRO, 2)) + "</a>";
                        }

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=3&a=" + ano + "&t=" + t;
                            _marco.Text = "<a href='" + link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.MARCO, 3)) + "</a>";
                        }

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=4&a=" + ano + "&t=" + t;
                            _abril.Text = "<a href='" + link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.ABRIL, 4)) + "</a>";
                        }

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=5&a=" + ano + "&t=" + t;
                            _maio.Text = "<a href='" + link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.MAIO, 5)) + "</a>";
                        }

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=6&a=" + ano + "&t=" + t;
                            _junho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.JUNHO, 6)) + "</a>";
                        }

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=7&a=" + ano + "&t=" + t;
                            _julho.Text = "<a href='" + link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.JULHO, 7)) + "</a>";
                        }

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=8&a=" + ano + "&t=" + t;
                            _agosto.Text = "<a href='" + link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.AGOSTO, 8)) + "</a>";
                        }

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=9&a=" + ano + "&t=" + t;
                            _setembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.SETEMBRO, 9)) + "</a>";
                        }

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=10&a=" + ano + "&t=" + t;
                            _outubro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.OUTUBRO, 10)) + "</a>";
                        }

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=11&a=" + ano + "&t=" + t;
                            _novembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.NOVEMBRO, 11)) + "</a>";
                        }

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            link = "dre_dre_conta_contabil.aspx?cc=" + ctbContabil + "&ff=" + filial + "&m=12&a=" + ano + "&t=" + t;
                            _dezembro.Text = "<a href='" + link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.DEZEMBRO, 12)) + "</a>";
                        }

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(receitadespesaLinha.TOTAL);

                    }
                }
            }
        }
        protected void gvReceitaDespesaFinItem_DataBound(object sender, EventArgs e)
        {
            GridView gvReceitaDespesaFinItem = (GridView)sender;
            if (gvReceitaDespesaFinItem != null)
                if (gvReceitaDespesaFinItem.HeaderRow != null)
                    gvReceitaDespesaFinItem.HeaderRow.Visible = false;
        }

        #endregion

        #region "LAIR"

        protected void gvLAIR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult lair = e.Row.DataItem as SP_OBTER_DREResult;

                    if (lair != null && lair.GRUPO != "")
                    {
                        Literal _litLAIR = e.Row.FindControl("litLAIR") as Literal;
                        if (_litLAIR != null)
                            _litLAIR.Text = "<font size='2' face='Calibri'>&nbsp;" + lair.GRUPO.Trim().ToUpper() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(lair.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(lair.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(lair.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(lair.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(lair.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(lair.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(lair.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(lair.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(lair.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(lair.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(lair.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(lair.DEZEMBRO, 12)).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(lair.TOTAL);

                    }

                    if (lair.GRUPO == "")
                        e.Row.Height = Unit.Pixel(1);

                }
            }
        }
        protected void gvLAIR_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvLAIR.Rows[gvLAIR.Rows.Count - 2];
            if (firstBlankRow != null)
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";

            GridViewRow lastBlankRow = gvLAIR.Rows[gvLAIR.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = corTitulo;
                int count = gvLAIR.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
                lastBlankRow.Attributes["style"] = "line-height: 2px;";
            }

            GridViewRow headerRow = gvLAIR.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 3px;";
        }

        #endregion

        #region "IMPOSTOS"

        protected void gvImpostoTaxa_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult impostotaxa = e.Row.DataItem as SP_OBTER_DREResult;

                    if (impostotaxa != null && impostotaxa.GRUPO != "")
                    {
                        Literal _litImpostoTaxa = e.Row.FindControl("litImpostoTaxa") as Literal;
                        if (_litImpostoTaxa != null)
                            _litImpostoTaxa.Text = "<font size='2' face='Calibri'>&nbsp;" + impostotaxa.GRUPO.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(impostotaxa.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(impostotaxa.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(impostotaxa.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(impostotaxa.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(impostotaxa.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(impostotaxa.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(impostotaxa.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(impostotaxa.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(impostotaxa.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(impostotaxa.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(impostotaxa.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(impostotaxa.DEZEMBRO, 12));

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(impostotaxa.TOTAL);

                    }

                    if (impostotaxa.GRUPO == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (impostotaxa.ID == 1)
                        {
                            GridView gvImpostoTaxaItem = e.Row.FindControl("gvImpostoTaxaItem") as GridView;
                            if (gvImpostoTaxaItem != null)
                            {
                                var lstimpostotaxa = gListaImpostoTaxa.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DREResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO),
                                                                            TOTAL = k.Sum(j => j.TOTAL)
                                                                        }).ToList();
                                gvImpostoTaxaItem.DataSource = lstimpostotaxa;
                                gvImpostoTaxaItem.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvImpostoTaxa_DataBound(object sender, EventArgs e)
        {
            GridViewRow lastBlankRow = gvImpostoTaxa.Rows[gvImpostoTaxa.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                // Linha abaixo da de cima
                lastBlankRow.BackColor = corTitulo;
                int count = gvImpostoTaxa.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow headerRow = gvImpostoTaxa.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 4px;";

        }

        protected void gvImpostoTaxaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult impostotaxaLinha = e.Row.DataItem as SP_OBTER_DREResult;

                    if (impostotaxaLinha != null && impostotaxaLinha.LINHA != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + impostotaxaLinha.LINHA.Trim().ToUpper() + "</font></span>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(impostotaxaLinha.JANEIRO, 1));
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(impostotaxaLinha.FEVEREIRO, 2));
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(impostotaxaLinha.MARCO, 3));
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(impostotaxaLinha.ABRIL, 4));
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(impostotaxaLinha.MAIO, 5));
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(impostotaxaLinha.JUNHO, 6));
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(impostotaxaLinha.JULHO, 7));
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(impostotaxaLinha.AGOSTO, 8));
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(impostotaxaLinha.SETEMBRO, 9));
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(impostotaxaLinha.OUTUBRO, 10));
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(impostotaxaLinha.NOVEMBRO, 11));
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(impostotaxaLinha.DEZEMBRO, 12));

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(impostotaxaLinha.TOTAL);

                    }
                }
            }
        }
        protected void gvImpostoTaxaItem_DataBound(object sender, EventArgs e)
        {
            GridView gvImpostoTaxaItem = (GridView)sender;
            if (gvImpostoTaxaItem != null)
                if (gvImpostoTaxaItem.HeaderRow != null)
                    gvImpostoTaxaItem.HeaderRow.Visible = false;
        }

        #endregion

        #region "RESULTADO LUCRO LIQUIDO/PREJUÍZO"

        protected void gvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREResult resultado = e.Row.DataItem as SP_OBTER_DREResult;

                    if (resultado != null && resultado.GRUPO != "")
                    {
                        Literal _litResultado = e.Row.FindControl("litResultado") as Literal;
                        if (_litResultado != null)
                            _litResultado.Text = "<font size='2' face='Calibri'>&nbsp;" + resultado.GRUPO.Trim().ToUpper() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(resultado.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(resultado.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(resultado.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(resultado.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(resultado.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(resultado.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(resultado.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(resultado.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(resultado.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(resultado.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(resultado.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(resultado.DEZEMBRO, 12)).Trim();

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(resultado.TOTAL);

                    }

                    if (resultado.GRUPO == "")
                        e.Row.Height = Unit.Pixel(1);

                }
            }
        }
        protected void gvResultado_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvResultado.Rows[gvResultado.Rows.Count - 2];
            if (firstBlankRow != null)
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";

            GridViewRow lastBlankRow = gvResultado.Rows[gvResultado.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = corTitulo;
                int count = gvResultado.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
                lastBlankRow.Attributes["style"] = "line-height: 2px;";
            }

            GridViewRow headerRow = gvResultado.HeaderRow;
            if (headerRow != null)
                headerRow.Attributes["style"] = "line-height: 3px;";
        }

        #endregion

        #region "EXCEL"
        protected void btExcel_Click(object sender, EventArgs e)
        {
            string filial = "";

            try
            {
                if (Session["DRE_20_REL"] != null)
                {

                    filial = hidFilial.Value.Replace(" ", "");

                    gvDREExcel.Visible = true;
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "DRE_" + ddlAno.SelectedItem.Text.Trim() + "_" + ddlMesDe.SelectedItem.Text + "-" + ddlMesAte.SelectedItem.Text + "_" + filial + "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvDREExcel.AllowPaging = false;
                    gvDREExcel.PageSize = 1000;
                    gvDREExcel.DataSource = (Session["DRE_20_REL"] as List<SP_OBTER_DREResult>);
                    gvDREExcel.DataBind();


                    gvDREExcel.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvDREExcel.HeaderRow.Cells.Count; i++)
                    {
                        gvDREExcel.HeaderRow.Cells[i].Attributes.Remove("href");
                        gvDREExcel.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                        gvDREExcel.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvDREExcel.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }
                    gvDREExcel.RenderControl(htw);
                    Response.Write(sw.ToString().Replace("<a", "<p").Replace("</a>", "</p>"));
                    Response.End();

                    gvDREExcel.Visible = false;
                    btExcel.Enabled = false;
                    hidFilial.Value = "";
                }
            }
            catch (Exception ex)
            {
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion

    }
}
