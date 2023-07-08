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
using System.Text;

namespace Relatorios
{
    public partial class dre_dre : System.Web.UI.Page
    {
        DREController dreController = new DREController();
        BaseController baseController = new BaseController();

        Color corTitulo = System.Drawing.SystemColors.GradientActiveCaption;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";

        char intercompany = 'S';
        bool orcamento = false;

        decimal dJaneiro, fatBrutoJaneiro = 0;
        decimal dFevereiro, fatBrutoFevereiro = 0;
        decimal dMarco, fatBrutoMarco = 0;
        decimal dAbril, fatBrutoAbril = 0;
        decimal dMaio, fatBrutoMaio = 0;
        decimal dJunho, fatBrutoJunho = 0;
        decimal dJulho, fatBrutoJulho = 0;
        decimal dAgosto, fatBrutoAgosto = 0;
        decimal dSetembro, fatBrutoSetembro = 0;
        decimal dOutubro, fatBrutoOutubro = 0;
        decimal dNovembro, fatBrutoNovembro = 0;
        decimal dDezembro, fatBrutoDezembro = 0;
        decimal dTotal, fatBrutoTotal = 0;

        List<SP_DRE_OBTER_DADOSResult> gListaAtacado = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaVarejo = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaReceitaBruta = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaReceitaLiquida = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaTaxaCartao = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaImposto = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaCmvAtacado = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaCmvVarejo = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaMargemContribuicao = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaMargemContribuicaoVarejo = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaMargemContribuicaoAtacado = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaMargemContribuicaoVarejoAux = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaMargemContribuicaoAtacadoAux = new List<SP_DRE_OBTER_DADOSResult>();
        decimal gTotalReceitaBrutaVarejo = 0;
        decimal gTotalReceitaBrutaAtacado = 0;
        decimal gTotalMargemContribuicaoVarejo = 0;
        decimal gTotalMargemContribuicaoAtacado = 0;
        decimal gTotalCustoProdutoVarejo = 0;
        decimal gTotalCustoProdutoAtacado = 0;

        List<SP_DRE_OBTER_DADOSResult> gListaCustoFixo = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaMargemBruta = new List<SP_DRE_OBTER_DADOSResult>();
        decimal gTotalReceitaLiquida = 0;
        decimal gTotalMargemBruta = 0;

        List<SP_DRE_OBTER_DADOSResult> gListaDespesaCTO = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaDespesaEspecifica = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaDespesaVenda = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaDespesaAdmPessoal = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaDespesaAdmOutros = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaEBITDA = new List<SP_DRE_OBTER_DADOSResult>();
        decimal gTotalEbitda = 0;

        List<SP_DRE_OBTER_DADOSResult> gListaDepreciacao = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaDespesaEventual = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaLucroBruto = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaReceitaDespesaFinanceira = new List<SP_DRE_OBTER_DADOSResult>();
        List<SP_DRE_OBTER_DADOSResult> gListaLAIR = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaImpostoTaxa = new List<SP_DRE_OBTER_DADOSResult>();

        List<SP_DRE_OBTER_DADOSResult> gListaResultado = new List<SP_DRE_OBTER_DADOSResult>();

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

                //Carregar com a data atual
                //btBuscar_Click(null, null);
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;
            string filial = "";
            string ano = "";

            ano = ddlAno.SelectedValue;
            v_dataini = Convert.ToDateTime(ano + "-01-01");
            v_datafim = Convert.ToDateTime(ano + "-12-31");

            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                filial = ddlFilial.SelectedValue;

            intercompany = 'S';
            orcamento = false;

            try
            {

                labErro.Text = "";

                CarregarReceitaLiquida(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarCMV(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarCustoFixo(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarDespesaCTO(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarDespesaEspecifica(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarDespesaVenda(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarDespesaADM(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarDepreciacao(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarDespesaEventual(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarReceitaDespFinanceira(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
                CarregarImpostosTaxas(v_dataini, v_datafim, filial, intercompany);
                ZerarValores();
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
                filial.Insert(2, new FILIAI { COD_FILIAL = "999999", FILIAL = "VAREJO" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
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
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

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
        private void ZerarValores()
        {
            dJaneiro = 0;
            dFevereiro = 0;
            dMarco = 0;
            dAbril = 0;
            dMaio = 0;
            dJunho = 0;
            dJulho = 0;
            dAgosto = 0;
            dSetembro = 0;
            dOutubro = 0;
            dNovembro = 0;
            dDezembro = 0;
            dTotal = 0;
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
        private void GuardarFaturamentoBrutoPorMes(List<SP_DRE_OBTER_DADOSResult> valorCalculo)
        {
            fatBrutoJaneiro = FiltroValor(valorCalculo.Sum(p => p.JANEIRO), 1);
            fatBrutoFevereiro = FiltroValor(valorCalculo.Sum(p => p.FEVEREIRO), 2);
            fatBrutoMarco = FiltroValor(valorCalculo.Sum(p => p.MARCO), 3);
            fatBrutoAbril = FiltroValor(valorCalculo.Sum(p => p.ABRIL), 4);
            fatBrutoMaio = FiltroValor(valorCalculo.Sum(p => p.MAIO), 5);
            fatBrutoJunho = FiltroValor(valorCalculo.Sum(p => p.JUNHO), 6);
            fatBrutoJulho = FiltroValor(valorCalculo.Sum(p => p.JULHO), 7);
            fatBrutoAgosto = FiltroValor(valorCalculo.Sum(p => p.AGOSTO), 8);
            fatBrutoSetembro = FiltroValor(valorCalculo.Sum(p => p.SETEMBRO), 9);
            fatBrutoOutubro = FiltroValor(valorCalculo.Sum(p => p.OUTUBRO), 10);
            fatBrutoNovembro = FiltroValor(valorCalculo.Sum(p => p.NOVEMBRO), 11);
            fatBrutoDezembro = FiltroValor(valorCalculo.Sum(p => p.DEZEMBRO), 12);
            fatBrutoTotal = (fatBrutoJaneiro +
                            fatBrutoFevereiro +
                            fatBrutoMarco +
                            fatBrutoAbril +
                            fatBrutoMaio +
                            fatBrutoJunho +
                            fatBrutoJulho +
                            fatBrutoAgosto +
                            fatBrutoSetembro +
                            fatBrutoOutubro +
                            fatBrutoNovembro +
                            fatBrutoDezembro);
        }
        #endregion

        #region "ACOES"

        private void CarregarReceitaLiquida(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {

            //Obter Atacado
            gListaAtacado = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 1, orcamento);

            if (filial != "000000") //ATACADO
            {
                gListaVarejo = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 2, orcamento);
                gListaTaxaCartao = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 3, orcamento);
            }

            gListaImposto = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 4, orcamento);

            gListaReceitaBruta = gListaVarejo.Union(gListaAtacado).ToList();
            gListaReceitaBruta = gListaReceitaBruta.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "RECEITAS BRUTAS",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            GuardarFaturamentoBrutoPorMes(gListaReceitaBruta);

            var deducoesReceitaBruta = gListaTaxaCartao.Union(gListaImposto);
            deducoesReceitaBruta = deducoesReceitaBruta.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                k => new SP_DRE_OBTER_DADOSResult
                                                                {
                                                                    Id = 2,
                                                                    Grupo = "DEDUÇÕES RECEITAS BRUTAS",
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
                                                                    DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                }).ToList();

            var receitaLiquida = gListaReceitaBruta.Union(deducoesReceitaBruta).ToList();
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "1" });
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "RECEITAS LÍQUIDAS" });
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "2" });

            if (receitaLiquida != null)
            {
                gvReceitaLiquida.DataSource = receitaLiquida;
                gvReceitaLiquida.DataBind();
            }
        }
        private void CarregarCMV(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaCmvAtacado = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 5, orcamento);
            if (filial != "000000")
            {
                gListaCmvVarejo = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 6, orcamento);
            }

            var cmv = gListaCmvAtacado.Union(gListaCmvVarejo).ToList();
            cmv = cmv.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "CMV",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            cmv.Insert(cmv.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });
            cmv.Insert(cmv.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "CMV" });
            cmv.Insert(cmv.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });
            if (cmv != null)
            {
                gvCMV.DataSource = cmv;
                gvCMV.DataBind();
            }
        }
        private void CarregarCustoFixo(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaCustoFixo = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 9, orcamento);

            var custoFixoAgrupado = gListaCustoFixo;
            custoFixoAgrupado = custoFixoAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "CUSTOS FIXOS",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            custoFixoAgrupado.Insert(custoFixoAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (custoFixoAgrupado != null)
            {
                gvCustoFixo.DataSource = custoFixoAgrupado;
                gvCustoFixo.DataBind();
            }
        }
        private void CarregarDespesaCTO(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaDespesaCTO = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 17, orcamento);

            var despCTOAgrupado = gListaDespesaCTO;
            despCTOAgrupado = despCTOAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "DESPESAS CTO",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            despCTOAgrupado.Insert(despCTOAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            despCTOAgrupado.Insert(despCTOAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (despCTOAgrupado != null)
            {
                gvDespesaCTO.DataSource = despCTOAgrupado;
                gvDespesaCTO.DataBind();
            }
        }
        private void CarregarDespesaEspecifica(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaDespesaEspecifica = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 23, orcamento);

            var despEspecificaAgrupado = gListaDespesaEspecifica;
            despEspecificaAgrupado = despEspecificaAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "DESPESAS ESPECÍFICAS",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            despEspecificaAgrupado.Insert(despEspecificaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            despEspecificaAgrupado.Insert(despEspecificaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (despEspecificaAgrupado != null)
            {
                gvDespesaEspecifica.DataSource = despEspecificaAgrupado;
                gvDespesaEspecifica.DataBind();
            }
        }
        private void CarregarDespesaVenda(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaDespesaVenda = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 7, orcamento); //SALARIOS
            gListaDespesaVenda.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 8, orcamento)); // COMISSAO REPRESENTANTE

            var despVendaAgrupado = gListaDespesaVenda;
            despVendaAgrupado = despVendaAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "DESPESAS VENDAS",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            despVendaAgrupado.Insert(despVendaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            despVendaAgrupado.Insert(despVendaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (despVendaAgrupado != null)
            {
                gvDespesaVenda.DataSource = despVendaAgrupado;
                gvDespesaVenda.DataBind();
            }
        }
        private void CarregarDespesaADM(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaDespesaAdmPessoal = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 18, orcamento);
            gListaDespesaAdmPessoal.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 14, orcamento));
            gListaDespesaAdmPessoal.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 15, orcamento));

            gListaDespesaAdmOutros = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 10, orcamento);
            gListaDespesaAdmOutros.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 11, orcamento));
            gListaDespesaAdmOutros.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 12, orcamento));

            var despADMAgrupado = gListaDespesaAdmPessoal.Union(gListaDespesaAdmOutros).ToList();
            despADMAgrupado = despADMAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "DESPESAS ADMINISTRATIVAS",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (despADMAgrupado != null)
            {
                gvDespesaAdm.DataSource = despADMAgrupado;
                gvDespesaAdm.DataBind();
            }
        }
        private void CarregarDepreciacao(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaDepreciacao.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 13, orcamento)); //DEPRECIACAO E AMORTIZACAO
            gListaDepreciacao.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 19, orcamento)); //SERVICOS NAO RECORRENTES

            var depreciacaoAgrupado = gListaDepreciacao;
            depreciacaoAgrupado = depreciacaoAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = k.Key.GRUPO.Trim().ToUpper(),
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            depreciacaoAgrupado.Insert(depreciacaoAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            depreciacaoAgrupado.Insert(depreciacaoAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (depreciacaoAgrupado != null)
            {
                gvDepreciacaoAmortizacao.DataSource = depreciacaoAgrupado;
                gvDepreciacaoAmortizacao.DataBind();
            }
        }
        private void CarregarDespesaEventual(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaDespesaEventual.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 22, orcamento)); //DESPESA EVENTUAL

            var despesaEventualAgrupado = gListaDespesaEventual;
            despesaEventualAgrupado = despesaEventualAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 22,
                                                                                Grupo = k.Key.GRUPO.Trim().ToUpper(),
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            despesaEventualAgrupado.Insert(despesaEventualAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            despesaEventualAgrupado.Insert(despesaEventualAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (despesaEventualAgrupado != null)
            {
                gvDespesaEventual.DataSource = despesaEventualAgrupado;
                gvDespesaEventual.DataBind();
            }
        }
        private void CarregarReceitaDespFinanceira(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaReceitaDespesaFinanceira.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 20, orcamento)); //Receitas Financeiras
            gListaReceitaDespesaFinanceira.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 16, orcamento)); //Despesas Financeiras

            var receitaDespesaAgrupado = gListaReceitaDespesaFinanceira;
            receitaDespesaAgrupado = receitaDespesaAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = k.Key.GRUPO.Trim().ToUpper(),
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            receitaDespesaAgrupado.Insert(receitaDespesaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            receitaDespesaAgrupado.Insert(receitaDespesaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (receitaDespesaAgrupado != null)
            {
                gvReceitaDespesaFin.DataSource = receitaDespesaAgrupado;
                gvReceitaDespesaFin.DataBind();
            }
        }
        private void CarregarImpostosTaxas(DateTime dataIni, DateTime dataFim, string filial, char intercompany)
        {
            gListaImpostoTaxa.AddRange(dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 21, orcamento)); // IMPOSTOS E TAXAS

            var impostotaxaAgrupado = gListaImpostoTaxa;
            impostotaxaAgrupado = impostotaxaAgrupado.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = k.Key.GRUPO.Trim().ToUpper(),
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            impostotaxaAgrupado.Insert(impostotaxaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });
            impostotaxaAgrupado.Insert(impostotaxaAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "", Linha = "" });

            if (impostotaxaAgrupado != null)
            {
                gvImpostoTaxa.DataSource = impostotaxaAgrupado;
                gvImpostoTaxa.DataBind();
            }
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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult receitaLiquida = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (receitaLiquida != null && receitaLiquida.Grupo != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                        {
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + receitaLiquida.Grupo.Trim() + "</font> ";
                        }

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

                        totalLinha = Convert.ToDecimal(FiltroValor(receitaLiquida.JANEIRO, 1) + FiltroValor(receitaLiquida.FEVEREIRO, 2) + FiltroValor(receitaLiquida.MARCO, 3) + FiltroValor(receitaLiquida.ABRIL, 4) + FiltroValor(receitaLiquida.MAIO, 5) + FiltroValor(receitaLiquida.JUNHO, 6) + FiltroValor(receitaLiquida.JULHO, 7) + FiltroValor(receitaLiquida.AGOSTO, 8) + FiltroValor(receitaLiquida.SETEMBRO, 9) + FiltroValor(receitaLiquida.OUTUBRO, 10) + FiltroValor(receitaLiquida.NOVEMBRO, 11) + FiltroValor(receitaLiquida.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(FiltroValor(receitaLiquida.JANEIRO, 1));
                        dFevereiro += Convert.ToDecimal(FiltroValor(receitaLiquida.FEVEREIRO, 2));
                        dMarco += Convert.ToDecimal(FiltroValor(receitaLiquida.MARCO, 3));
                        dAbril += Convert.ToDecimal(FiltroValor(receitaLiquida.ABRIL, 4));
                        dMaio += Convert.ToDecimal(FiltroValor(receitaLiquida.MAIO, 5));
                        dJunho += Convert.ToDecimal(FiltroValor(receitaLiquida.JUNHO, 6));
                        dJulho += Convert.ToDecimal(FiltroValor(receitaLiquida.JULHO, 7));
                        dAgosto += Convert.ToDecimal(FiltroValor(receitaLiquida.AGOSTO, 8));
                        dSetembro += Convert.ToDecimal(FiltroValor(receitaLiquida.SETEMBRO, 9));
                        dOutubro += Convert.ToDecimal(FiltroValor(receitaLiquida.OUTUBRO, 10));
                        dNovembro += Convert.ToDecimal(FiltroValor(receitaLiquida.NOVEMBRO, 11));
                        dDezembro += Convert.ToDecimal(FiltroValor(receitaLiquida.DEZEMBRO, 12));
                        dTotal += totalLinha;

                    }

                    //Controla tamanho da linha vazia
                    if (receitaLiquida.Linha == "1")
                        e.Row.Height = Unit.Pixel(9);
                    else if (receitaLiquida.Linha == "2")
                        e.Row.Height = Unit.Pixel(5);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (receitaLiquida.Id == 1)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                var faturamentoBruto = gListaAtacado.Union(gListaVarejo).ToList();
                                faturamentoBruto = faturamentoBruto.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Replace("Faturamento", "").Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();
                                gvFaturamento.DataSource = faturamentoBruto;
                                gvFaturamento.DataBind();
                            }
                            img.Visible = true;
                        }
                        if (receitaLiquida.Id == 2)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                var deducoesBruto = gListaImposto.Union(gListaTaxaCartao).ToList();
                                deducoesBruto = deducoesBruto.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 2,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
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
            {
                lastRow.BackColor = corFundo;
                lastRow.Cells[1].Text = "<a href='../fat/fat_receita_liquida.aspx' class='adre' title='Receitas Líquidas'>&nbsp;RECEITAS LÍQUIDAS</a>";
                lastRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                lastRow.Cells[1].Font.Name = "Calibri";
                lastRow.Cells[2].Text = FormatarValor(dJaneiro);
                lastRow.Cells[2].Font.Name = "Calibri";
                lastRow.Cells[3].Text = FormatarValor(dFevereiro);
                lastRow.Cells[3].Font.Name = "Calibri";
                lastRow.Cells[4].Text = FormatarValor(dMarco);
                lastRow.Cells[4].Font.Name = "Calibri";
                lastRow.Cells[5].Text = FormatarValor(dAbril);
                lastRow.Cells[5].Font.Name = "Calibri";
                lastRow.Cells[6].Text = FormatarValor(dMaio);
                lastRow.Cells[6].Font.Name = "Calibri";
                lastRow.Cells[7].Text = FormatarValor(dJunho);
                lastRow.Cells[7].Font.Name = "Calibri";
                lastRow.Cells[8].Text = FormatarValor(dJulho);
                lastRow.Cells[8].Font.Name = "Calibri";
                lastRow.Cells[9].Text = FormatarValor(dAgosto);
                lastRow.Cells[9].Font.Name = "Calibri";
                lastRow.Cells[10].Text = FormatarValor(dSetembro);
                lastRow.Cells[10].Font.Name = "Calibri";
                lastRow.Cells[11].Text = FormatarValor(dOutubro);
                lastRow.Cells[11].Font.Name = "Calibri";
                lastRow.Cells[12].Text = FormatarValor(dNovembro);
                lastRow.Cells[12].Font.Name = "Calibri";
                lastRow.Cells[13].Text = FormatarValor(dDezembro);
                lastRow.Cells[13].Font.Name = "Calibri";
                lastRow.Cells[14].Text = FormatarValor(dTotal);
                lastRow.Cells[14].Font.Name = "Calibri";

                var ga = gListaAtacado.GroupBy(x => new { GRUPO = x.Grupo }).Select(p => new SP_DRE_OBTER_DADOSResult
                {
                    JANEIRO = p.Sum(g => g.JANEIRO),
                    FEVEREIRO = p.Sum(g => g.FEVEREIRO),
                    MARCO = p.Sum(g => g.MARCO),
                    ABRIL = p.Sum(g => g.ABRIL),
                    MAIO = p.Sum(g => g.MAIO),
                    JUNHO = p.Sum(g => g.JUNHO),
                    JULHO = p.Sum(g => g.JULHO),
                    AGOSTO = p.Sum(g => g.AGOSTO),
                    SETEMBRO = p.Sum(g => g.SETEMBRO),
                    OUTUBRO = p.Sum(g => g.OUTUBRO),
                    NOVEMBRO = p.Sum(g => g.NOVEMBRO),
                    DEZEMBRO = p.Sum(g => g.DEZEMBRO)
                }).SingleOrDefault();

                decimal dJaneiroAtac = 0;
                decimal dFevereiroAtac = 0;
                decimal dMarcoAtac = 0;
                decimal dAbrilAtac = 0;
                decimal dMaioAtac = 0;
                decimal dJunhoAtac = 0;
                decimal dJulhoAtac = 0;
                decimal dAgostoAtac = 0;
                decimal dSetembroAtac = 0;
                decimal dOutubroAtac = 0;
                decimal dNovembroAtac = 0;
                decimal dDezembroAtac = 0;

                if (ga != null)
                {
                    dJaneiroAtac = Convert.ToDecimal(ga.JANEIRO);
                    dFevereiroAtac = Convert.ToDecimal(ga.FEVEREIRO);
                    dMarcoAtac = Convert.ToDecimal(ga.MARCO);
                    dAbrilAtac = Convert.ToDecimal(ga.ABRIL);
                    dMaioAtac = Convert.ToDecimal(ga.MAIO);
                    dJunhoAtac = Convert.ToDecimal(ga.JUNHO);
                    dJulhoAtac = Convert.ToDecimal(ga.JULHO);
                    dAgostoAtac = Convert.ToDecimal(ga.AGOSTO);
                    dSetembroAtac = Convert.ToDecimal(ga.SETEMBRO);
                    dOutubroAtac = Convert.ToDecimal(ga.OUTUBRO);
                    dNovembroAtac = Convert.ToDecimal(ga.NOVEMBRO);
                    dDezembroAtac = Convert.ToDecimal(ga.DEZEMBRO);
                }

                gListaMargemContribuicaoVarejo.Add(new SP_DRE_OBTER_DADOSResult
                {
                    Grupo = "MARGEM DE CONTRIBUIÇÃO",
                    Linha = "Receita Varejo",
                    JANEIRO = dJaneiro - dJaneiroAtac,
                    FEVEREIRO = dFevereiro - dFevereiroAtac,
                    MARCO = dMarco - dMarcoAtac,
                    ABRIL = dAbril - dAbrilAtac,
                    MAIO = dMaio - dMaioAtac,
                    JUNHO = dJunho - dJunhoAtac,
                    JULHO = dJulho - dJulhoAtac,
                    AGOSTO = dAgosto - dAgostoAtac,
                    SETEMBRO = dSetembro - dSetembroAtac,
                    OUTUBRO = dOutubro - dOutubroAtac,
                    NOVEMBRO = dNovembro - dNovembroAtac,
                    DEZEMBRO = dDezembro - dDezembroAtac
                });

                gListaMargemContribuicaoAtacado.Add(new SP_DRE_OBTER_DADOSResult
                {
                    Grupo = "MARGEM DE CONTRIBUIÇÃO",
                    Linha = "Receita Atacado",
                    JANEIRO = dJaneiroAtac,
                    FEVEREIRO = dFevereiroAtac,
                    MARCO = dMarcoAtac,
                    ABRIL = dAbrilAtac,
                    MAIO = dMaioAtac,
                    JUNHO = dJunhoAtac,
                    JULHO = dJulhoAtac,
                    AGOSTO = dAgostoAtac,
                    SETEMBRO = dSetembroAtac,
                    OUTUBRO = dOutubroAtac,
                    NOVEMBRO = dNovembroAtac,
                    DEZEMBRO = dDezembroAtac
                });

                gListaMargemContribuicao.AddRange(gListaMargemContribuicaoVarejo);
                if (ga != null)
                    gListaMargemContribuicao.AddRange(gListaMargemContribuicaoAtacado);

                //????? 
                gListaReceitaLiquida.AddRange(gListaMargemContribuicaoVarejo);
                if (ga != null)
                    gListaReceitaLiquida.AddRange(gListaMargemContribuicaoAtacado);
            }
        }

        protected void gvFaturamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult faturamento = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (faturamento != null && faturamento.Grupo != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                        {
                            if (faturamento.Linha.Contains("ATACADO") || faturamento.Linha.Contains("VAREJO"))
                            {
                                if (faturamento.Linha.ToUpper().Contains("VAREJO"))
                                    url_link = "../fat/fat_varejo.aspx";
                                else
                                    url_link = "../fat/fat_atacado.aspx";
                                _litFaturamento.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + faturamento.Linha.Trim() + "'>&nbsp;" + faturamento.Linha.Trim().ToUpper() + "</a></font> ";
                            }
                            else
                            {
                                _litFaturamento.Text = "<font size='2' face='Calibri'>&nbsp;(-) " + faturamento.Linha.Trim().ToUpper() + "</font> ";
                            }
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

                        totalLinha = Convert.ToDecimal(FiltroValor(faturamento.JANEIRO, 1) + FiltroValor(faturamento.FEVEREIRO, 2) + FiltroValor(faturamento.MARCO, 3) + FiltroValor(faturamento.ABRIL, 4) + FiltroValor(faturamento.MAIO, 5) + FiltroValor(faturamento.JUNHO, 6) + FiltroValor(faturamento.JULHO, 7) + FiltroValor(faturamento.AGOSTO, 8) + FiltroValor(faturamento.SETEMBRO, 9) + FiltroValor(faturamento.OUTUBRO, 10) + FiltroValor(faturamento.NOVEMBRO, 11) + FiltroValor(faturamento.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (faturamento.Linha.Trim().ToUpper() == "IMPOSTOS S/VENDA")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                gListaImposto = gListaImposto.GroupBy(b => new { TIPO = b.Tipo }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Linha = k.Key.TIPO.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();

                                gvSubGrid.DataSource = gListaImposto;
                                gvSubGrid.DataBind();
                            }
                            img.Visible = true;
                        }

                        if (faturamento.Linha.Trim().ToUpper() == "VAREJO")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                gvSubGrid.DataSource = gListaVarejo;
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Grupo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.Linha.Contains("Varejo"))
                                _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.Filial.Trim().ToUpper() + "</font> ";
                            else
                                _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;(-) " + subitem.Linha.Trim().ToUpper() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult cmv = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (cmv != null && cmv.Grupo != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + cmv.Grupo.Trim() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(cmv.JANEIRO, 1) + FiltroValor(cmv.FEVEREIRO, 2) + FiltroValor(cmv.MARCO, 3) + FiltroValor(cmv.ABRIL, 4) + FiltroValor(cmv.MAIO, 5) + FiltroValor(cmv.JUNHO, 6) + FiltroValor(cmv.JULHO, 7) + FiltroValor(cmv.AGOSTO, 8) + FiltroValor(cmv.SETEMBRO, 9) + FiltroValor(cmv.OUTUBRO, 10) + FiltroValor(cmv.NOVEMBRO, 11) + FiltroValor(cmv.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha, 0);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(FiltroValor(cmv.JANEIRO, 1));
                        dFevereiro += Convert.ToDecimal(FiltroValor(cmv.FEVEREIRO, 2));
                        dMarco += Convert.ToDecimal(FiltroValor(cmv.MARCO, 3));
                        dAbril += Convert.ToDecimal(FiltroValor(cmv.ABRIL, 4));
                        dMaio += Convert.ToDecimal(FiltroValor(cmv.MAIO, 5));
                        dJunho += Convert.ToDecimal(FiltroValor(cmv.JUNHO, 6));
                        dJulho += Convert.ToDecimal(FiltroValor(cmv.JULHO, 7));
                        dAgosto += Convert.ToDecimal(FiltroValor(cmv.AGOSTO, 8));
                        dSetembro += Convert.ToDecimal(FiltroValor(cmv.SETEMBRO, 9));
                        dOutubro += Convert.ToDecimal(FiltroValor(cmv.OUTUBRO, 10));
                        dNovembro += Convert.ToDecimal(FiltroValor(cmv.NOVEMBRO, 11));
                        dDezembro += Convert.ToDecimal(FiltroValor(cmv.DEZEMBRO, 12));
                        dTotal += totalLinha;

                    }

                    if (cmv.Grupo == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cmv.Id == 1)
                        {
                            GridView gvCMVItem = e.Row.FindControl("gvCMVItem") as GridView;
                            if (gvCMVItem != null)
                            {
                                var cmvAgrupado = gListaCmvAtacado.Union(gListaCmvVarejo).ToList();
                                cmvAgrupado = cmvAgrupado.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
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

            //Obter custo do produto
            gListaMargemContribuicao.Add(new SP_DRE_OBTER_DADOSResult
            {
                Grupo = "MARGEM DE CONTRIBUIÇÃO",
                Linha = "CMV",
                JANEIRO = dJaneiro,
                FEVEREIRO = dFevereiro,
                MARCO = dMarco,
                ABRIL = dAbril,
                MAIO = dMaio,
                JUNHO = dJunho,
                JULHO = dJulho,
                AGOSTO = dAgosto,
                SETEMBRO = dSetembro,
                OUTUBRO = dOutubro,
                NOVEMBRO = dNovembro,
                DEZEMBRO = dDezembro
            });

            // SOMAR MARGEM DE CONTRIBUIÇÃO //TESTE
            gListaMargemContribuicao = gListaMargemContribuicao.GroupBy(p => new { GRUPO = p.Grupo }).Select(k => new SP_DRE_OBTER_DADOSResult
            {
                Grupo = "MARGEM DE CONTRIBUIÇÃO",
                Linha = "AGRUPADO",
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
                DEZEMBRO = k.Sum(g => g.DEZEMBRO)
            }).ToList();

            if (gListaMargemContribuicao.Count > 0)
            {
                gListaMargemContribuicao.Insert(gListaMargemContribuicao.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });

                //Carregar margem de contribuicao
                gvMargem.DataSource = gListaMargemContribuicao;
                gvMargem.DataBind();
            }
        }

        protected void gvCMVItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult cmvlinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (cmvlinha != null && cmvlinha.Grupo != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                        {
                            if (cmvlinha.Linha.ToUpper().Trim() == "VAREJO")
                                url_link = "../cmv/cmv_varejo.aspx";
                            else
                                url_link = "../cmv/cmv_atacado.aspx";
                            _litFaturamento.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + cmvlinha.Linha.Trim() + "'>&nbsp;" + cmvlinha.Linha.Trim().ToUpper() + "</a></font> ";
                        }

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

                        totalLinha = Convert.ToDecimal(FiltroValor(cmvlinha.JANEIRO, 1) + FiltroValor(cmvlinha.FEVEREIRO, 2) + FiltroValor(cmvlinha.MARCO, 3) + FiltroValor(cmvlinha.ABRIL, 4) + FiltroValor(cmvlinha.MAIO, 5) + FiltroValor(cmvlinha.JUNHO, 6) + FiltroValor(cmvlinha.JULHO, 7) + FiltroValor(cmvlinha.AGOSTO, 8) + FiltroValor(cmvlinha.SETEMBRO, 9) + FiltroValor(cmvlinha.OUTUBRO, 10) + FiltroValor(cmvlinha.NOVEMBRO, 11) + FiltroValor(cmvlinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cmvlinha.Linha.Trim().ToUpper() == "VAREJO")
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Grupo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.Linha.ToUpper().Contains("VAREJO"))
                                _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.Filial.Trim().ToUpper() + "</font> ";
                            else
                                _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;(-) " + subitem.Linha.Trim().ToUpper() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult margem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (margem != null && margem.Grupo != "")
                    {
                        Literal _litMargem = e.Row.FindControl("litMargem") as Literal;
                        if (_litMargem != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _litMargem.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + margem.Grupo.Trim() + "</i></font></span>";
                            else
                                _litMargem.Text = "<font size='2' face='Calibri'>&nbsp;" + margem.Grupo.Trim() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.JANEIRO, 1)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _janeiro.Text = FormatarValor(FiltroValor(margem.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.FEVEREIRO, 2)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _fevereiro.Text = FormatarValor(FiltroValor(margem.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.MARCO, 3)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _marco.Text = FormatarValor(FiltroValor(margem.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.ABRIL, 4)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _abril.Text = FormatarValor(FiltroValor(margem.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.MAIO, 5)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _maio.Text = FormatarValor(FiltroValor(margem.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.JUNHO, 6)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _junho.Text = FormatarValor(FiltroValor(margem.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.JULHO, 7)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _julho.Text = FormatarValor(FiltroValor(margem.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.AGOSTO, 8)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _agosto.Text = FormatarValor(FiltroValor(margem.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.SETEMBRO, 9)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _setembro.Text = FormatarValor(FiltroValor(margem.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.OUTUBRO, 10)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _outubro.Text = FormatarValor(FiltroValor(margem.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.NOVEMBRO, 11)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _novembro.Text = FormatarValor(FiltroValor(margem.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            if (margem.Linha.Contains("%") || (margem.Linha.Trim() == "MARK-UP"))
                                _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.DEZEMBRO, 12)).Trim() + ((margem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _dezembro.Text = FormatarValor(FiltroValor(margem.DEZEMBRO, 12)).Trim();

                        totalLinha = Convert.ToDecimal(FiltroValor(margem.JANEIRO, 1) + FiltroValor(margem.FEVEREIRO, 2) + FiltroValor(margem.MARCO, 3) + FiltroValor(margem.ABRIL, 4) + FiltroValor(margem.MAIO, 5) + FiltroValor(margem.JUNHO, 6) + FiltroValor(margem.JULHO, 7) + FiltroValor(margem.AGOSTO, 8) + FiltroValor(margem.SETEMBRO, 9) + FiltroValor(margem.OUTUBRO, 10) + FiltroValor(margem.NOVEMBRO, 11) + FiltroValor(margem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;

                        //guarda o total da margem de contribuicao para utilizar durante o loop da linha
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(FiltroValor(margem.JANEIRO, 1));
                        dFevereiro += Convert.ToDecimal(FiltroValor(margem.FEVEREIRO, 2));
                        dMarco += Convert.ToDecimal(FiltroValor(margem.MARCO, 3));
                        dAbril += Convert.ToDecimal(FiltroValor(margem.ABRIL, 4));
                        dMaio += Convert.ToDecimal(FiltroValor(margem.MAIO, 5));
                        dJunho += Convert.ToDecimal(FiltroValor(margem.JUNHO, 6));
                        dJulho += Convert.ToDecimal(FiltroValor(margem.JULHO, 7));
                        dAgosto += Convert.ToDecimal(FiltroValor(margem.AGOSTO, 8));
                        dSetembro += Convert.ToDecimal(FiltroValor(margem.SETEMBRO, 9));
                        dOutubro += Convert.ToDecimal(FiltroValor(margem.OUTUBRO, 10));
                        dNovembro += Convert.ToDecimal(FiltroValor(margem.NOVEMBRO, 11));
                        dDezembro += Convert.ToDecimal(FiltroValor(margem.DEZEMBRO, 12));
                        dTotal += totalLinha;

                    }

                    if (margem.Grupo == "")
                        e.Row.Height = Unit.Pixel(1);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (margem.Grupo != "")
                        {
                            GridView gvMargemItem = e.Row.FindControl("gvMargemItem") as GridView;
                            List<SP_DRE_OBTER_DADOSResult> _margemAtacado = new List<SP_DRE_OBTER_DADOSResult>();
                            List<SP_DRE_OBTER_DADOSResult> _margemVarejo = new List<SP_DRE_OBTER_DADOSResult>();

                            if (gvMargemItem != null)
                            {
                                //adiciona CMV Atacado
                                _margemAtacado.AddRange(gListaCmvAtacado.GroupBy(p => new { GRUPO = p.Grupo }).Select(g => new SP_DRE_OBTER_DADOSResult
                                {
                                    Grupo = "MARGEM DE CONTRIBUIÇÃO",
                                    JANEIRO = g.Sum(x => x.JANEIRO),
                                    FEVEREIRO = g.Sum(x => x.FEVEREIRO),
                                    MARCO = g.Sum(x => x.MARCO),
                                    ABRIL = g.Sum(x => x.ABRIL),
                                    MAIO = g.Sum(x => x.MAIO),
                                    JUNHO = g.Sum(x => x.JUNHO),
                                    JULHO = g.Sum(x => x.JULHO),
                                    AGOSTO = g.Sum(x => x.AGOSTO),
                                    SETEMBRO = g.Sum(x => x.SETEMBRO),
                                    OUTUBRO = g.Sum(x => x.OUTUBRO),
                                    NOVEMBRO = g.Sum(x => x.NOVEMBRO),
                                    DEZEMBRO = g.Sum(x => x.DEZEMBRO)
                                }));
                                var margemA = _margemAtacado.Union(gListaMargemContribuicaoAtacado).ToList();
                                margemA = margemA.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = "ATACADO",
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();



                                //adiciona CMV Varejo
                                _margemVarejo.AddRange(gListaCmvVarejo.GroupBy(p => new { GRUPO = p.Grupo }).Select(g => new SP_DRE_OBTER_DADOSResult
                                {
                                    Grupo = "MARGEM DE CONTRIBUIÇÃO",
                                    JANEIRO = g.Sum(x => x.JANEIRO),
                                    FEVEREIRO = g.Sum(x => x.FEVEREIRO),
                                    MARCO = g.Sum(x => x.MARCO),
                                    ABRIL = g.Sum(x => x.ABRIL),
                                    MAIO = g.Sum(x => x.MAIO),
                                    JUNHO = g.Sum(x => x.JUNHO),
                                    JULHO = g.Sum(x => x.JULHO),
                                    AGOSTO = g.Sum(x => x.AGOSTO),
                                    SETEMBRO = g.Sum(x => x.SETEMBRO),
                                    OUTUBRO = g.Sum(x => x.OUTUBRO),
                                    NOVEMBRO = g.Sum(x => x.NOVEMBRO),
                                    DEZEMBRO = g.Sum(x => x.DEZEMBRO)
                                }));
                                var margemV = _margemVarejo.Union(gListaMargemContribuicaoVarejo).ToList();
                                margemV = margemV.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = "VAREJO",
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();

                                List<SP_DRE_OBTER_DADOSResult> _margem = new List<SP_DRE_OBTER_DADOSResult>();
                                //Auxiliar para calculo da % e markup
                                if (ddlFilial.SelectedValue == "" || ddlFilial.SelectedValue == "0" || ddlFilial.SelectedValue == "000000")
                                {
                                    gListaMargemContribuicaoAtacadoAux.AddRange(margemA);
                                    _margem = margemA;
                                }

                                if (ddlFilial.SelectedValue != "000000")
                                {
                                    gListaMargemContribuicaoVarejoAux.AddRange(margemV);
                                    _margem = margemV;
                                }

                                if (ddlFilial.SelectedValue == "0" || ddlFilial.SelectedValue == "")
                                    _margem = margemA.Union(margemV).ToList();


                                gvMargemItem.DataSource = _margem;
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult _margem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (_margem != null && _margem.Linha != "")
                    {
                        Literal _litVenda = e.Row.FindControl("litVenda") as Literal;
                        if (_litVenda != null)
                            _litVenda.Text = "<font size='2' face='Calibri'>&nbsp;" + _margem.Linha.Replace("RECEITA", "").Trim().ToUpper() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(_margem.JANEIRO, 1) + FiltroValor(_margem.FEVEREIRO, 2) + FiltroValor(_margem.MARCO, 3) + FiltroValor(_margem.ABRIL, 4) + FiltroValor(_margem.MAIO, 5) + FiltroValor(_margem.JUNHO, 6) + FiltroValor(_margem.JULHO, 7) + FiltroValor(_margem.AGOSTO, 8) + FiltroValor(_margem.SETEMBRO, 9) + FiltroValor(_margem.OUTUBRO, 10) + FiltroValor(_margem.NOVEMBRO, 11) + FiltroValor(_margem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        if (_margem.Linha.ToUpper().Contains("VAREJO"))
                            gTotalMargemContribuicaoVarejo = totalLinha;
                        else
                            gTotalMargemContribuicaoAtacado = totalLinha;
                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        GridView gvMargemItemSub = e.Row.FindControl("gvMargemItemSub") as GridView;
                        if (gvMargemItemSub != null)
                        {
                            decimal pJaneiro = 0;
                            decimal pFevereiro = 0;
                            decimal pMarco = 0;
                            decimal pAbril = 0;
                            decimal pMaio = 0;
                            decimal pJunho = 0;
                            decimal pJulho = 0;
                            decimal pAgosto = 0;
                            decimal pSetembro = 0;
                            decimal pOutubro = 0;
                            decimal pNovembro = 0;
                            decimal pDezembro = 0;

                            List<SP_DRE_OBTER_DADOSResult> margem = new List<SP_DRE_OBTER_DADOSResult>();

                            if (_margem.Linha.ToUpper().Contains("VAREJO"))
                            {
                                //OBTER % MARGEM DE CONTRIBUICAO
                                pJaneiro = (gListaVarejo.Sum(i => i.JANEIRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.JANEIRO) / gListaVarejo.Sum(i => i.JANEIRO)) * 100));
                                pFevereiro = (gListaVarejo.Sum(i => i.FEVEREIRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.FEVEREIRO) / gListaVarejo.Sum(i => i.FEVEREIRO)) * 100));
                                pMarco = (gListaVarejo.Sum(i => i.MARCO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.MARCO) / gListaVarejo.Sum(i => i.MARCO)) * 100));
                                pAbril = (gListaVarejo.Sum(i => i.ABRIL) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.ABRIL) / gListaVarejo.Sum(i => i.ABRIL)) * 100));
                                pMaio = (gListaVarejo.Sum(i => i.MAIO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.MAIO) / gListaVarejo.Sum(i => i.MAIO)) * 100));
                                pJunho = (gListaVarejo.Sum(i => i.JUNHO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.JUNHO) / gListaVarejo.Sum(i => i.JUNHO)) * 100));
                                pJulho = (gListaVarejo.Sum(i => i.JULHO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.JULHO) / gListaVarejo.Sum(i => i.JULHO)) * 100));
                                pAgosto = (gListaVarejo.Sum(i => i.AGOSTO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.AGOSTO) / gListaVarejo.Sum(i => i.AGOSTO)) * 100));
                                pSetembro = (gListaVarejo.Sum(i => i.SETEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.SETEMBRO) / gListaVarejo.Sum(i => i.SETEMBRO)) * 100));
                                pOutubro = (gListaVarejo.Sum(i => i.OUTUBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.OUTUBRO) / gListaVarejo.Sum(i => i.OUTUBRO)) * 100));
                                pNovembro = (gListaVarejo.Sum(i => i.NOVEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.NOVEMBRO) / gListaVarejo.Sum(i => i.NOVEMBRO)) * 100));
                                pDezembro = (gListaVarejo.Sum(i => i.DEZEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.DEZEMBRO) / gListaVarejo.Sum(i => i.DEZEMBRO)) * 100));

                                margem.Insert(0, new SP_DRE_OBTER_DADOSResult
                                {
                                    Grupo = "% MARGEM CONTRIBUIÇÃO",
                                    Linha = "% VAREJO",
                                    JANEIRO = pJaneiro,
                                    FEVEREIRO = pFevereiro,
                                    MARCO = pMarco,
                                    ABRIL = pAbril,
                                    MAIO = pMaio,
                                    JUNHO = pJunho,
                                    JULHO = pJulho,
                                    AGOSTO = pAgosto,
                                    SETEMBRO = pSetembro,
                                    OUTUBRO = pOutubro,
                                    NOVEMBRO = pNovembro,
                                    DEZEMBRO = pDezembro
                                });

                                //OBTER MARK-UP
                                pJaneiro = ((gListaCmvVarejo.Sum(i => i.JANEIRO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.JANEIRO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.JANEIRO))));
                                pFevereiro = ((gListaCmvVarejo.Sum(i => i.FEVEREIRO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.FEVEREIRO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.FEVEREIRO))));
                                pMarco = ((gListaCmvVarejo.Sum(i => i.MARCO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.MARCO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.MARCO))));
                                pAbril = ((gListaCmvVarejo.Sum(i => i.ABRIL)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.ABRIL)) / ((-1) * (gListaCmvVarejo.Sum(i => i.ABRIL))));
                                pMaio = ((gListaCmvVarejo.Sum(i => i.MAIO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.MAIO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.MAIO))));
                                pJunho = ((gListaCmvVarejo.Sum(i => i.JUNHO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.JUNHO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.JUNHO))));
                                pJulho = ((gListaCmvVarejo.Sum(i => i.JULHO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.JULHO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.JULHO))));
                                pAgosto = ((gListaCmvVarejo.Sum(i => i.AGOSTO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.AGOSTO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.AGOSTO))));
                                pSetembro = ((gListaCmvVarejo.Sum(i => i.SETEMBRO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.SETEMBRO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.SETEMBRO))));
                                pOutubro = ((gListaCmvVarejo.Sum(i => i.OUTUBRO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.OUTUBRO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.OUTUBRO))));
                                pNovembro = ((gListaCmvVarejo.Sum(i => i.NOVEMBRO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.NOVEMBRO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.NOVEMBRO))));
                                pDezembro = ((gListaCmvVarejo.Sum(i => i.DEZEMBRO)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.DEZEMBRO)) / ((-1) * (gListaCmvVarejo.Sum(i => i.DEZEMBRO))));

                                margem.Insert(1, new SP_DRE_OBTER_DADOSResult
                                {
                                    Grupo = "MARK-UP",
                                    Linha = "MARK-UP VAREJO",
                                    JANEIRO = pJaneiro,
                                    FEVEREIRO = pFevereiro,
                                    MARCO = pMarco,
                                    ABRIL = pAbril,
                                    MAIO = pMaio,
                                    JUNHO = pJunho,
                                    JULHO = pJulho,
                                    AGOSTO = pAgosto,
                                    SETEMBRO = pSetembro,
                                    OUTUBRO = pOutubro,
                                    NOVEMBRO = pNovembro,
                                    DEZEMBRO = pDezembro
                                });

                                gTotalReceitaBrutaVarejo = 0;
                                gTotalReceitaBrutaVarejo = Convert.ToDecimal((
                                                                          FiltroValor(gListaVarejo.Sum(i => i.JANEIRO), 1)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.FEVEREIRO), 2)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.MARCO), 3)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.ABRIL), 4)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.MAIO), 5)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.JUNHO), 6)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.JULHO), 7)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.AGOSTO), 8)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.SETEMBRO), 9)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.OUTUBRO), 10)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.NOVEMBRO), 11)
                                                                        + FiltroValor(gListaVarejo.Sum(i => i.DEZEMBRO), 12)));

                                gTotalCustoProdutoVarejo = 0;
                                gTotalCustoProdutoVarejo = Convert.ToDecimal((
                                                                          FiltroValor(gListaCmvVarejo.Sum(i => i.JANEIRO), 1)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.FEVEREIRO), 2)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.MARCO), 3)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.ABRIL), 4)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.MAIO), 5)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.JUNHO), 6)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.JULHO), 7)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.AGOSTO), 8)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.SETEMBRO), 9)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.OUTUBRO), 10)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.NOVEMBRO), 11)
                                                                        + FiltroValor(gListaCmvVarejo.Sum(i => i.DEZEMBRO), 12)));

                            }

                            if (_margem.Linha.ToUpper().Contains("ATACADO"))
                            {

                                //OBTER % MARGEM DE CONTRIBUICAO
                                pJaneiro = (gListaAtacado.Sum(i => i.JANEIRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.JANEIRO) / gListaAtacado.Sum(i => i.JANEIRO)) * 100));
                                pFevereiro = (gListaAtacado.Sum(i => i.FEVEREIRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.FEVEREIRO) / gListaAtacado.Sum(i => i.FEVEREIRO)) * 100));
                                pMarco = (gListaAtacado.Sum(i => i.MARCO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.MARCO) / gListaAtacado.Sum(i => i.MARCO)) * 100));
                                pAbril = (gListaAtacado.Sum(i => i.ABRIL) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.ABRIL) / gListaAtacado.Sum(i => i.ABRIL)) * 100));
                                pMaio = (gListaAtacado.Sum(i => i.MAIO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.MAIO) / gListaAtacado.Sum(i => i.MAIO)) * 100));
                                pJunho = (gListaAtacado.Sum(i => i.JUNHO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.JUNHO) / gListaAtacado.Sum(i => i.JUNHO)) * 100));
                                pJulho = (gListaAtacado.Sum(i => i.JULHO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.JULHO) / gListaAtacado.Sum(i => i.JULHO)) * 100));
                                pAgosto = (gListaAtacado.Sum(i => i.AGOSTO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.AGOSTO) / gListaAtacado.Sum(i => i.AGOSTO)) * 100));
                                pSetembro = (gListaAtacado.Sum(i => i.SETEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.SETEMBRO) / gListaAtacado.Sum(i => i.SETEMBRO)) * 100));
                                pOutubro = (gListaAtacado.Sum(i => i.OUTUBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.OUTUBRO) / gListaAtacado.Sum(i => i.OUTUBRO)) * 100));
                                pNovembro = (gListaAtacado.Sum(i => i.NOVEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.NOVEMBRO) / gListaAtacado.Sum(i => i.NOVEMBRO)) * 100));
                                pDezembro = (gListaAtacado.Sum(i => i.DEZEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.DEZEMBRO) / gListaAtacado.Sum(i => i.DEZEMBRO)) * 100));

                                margem.Insert(0, new SP_DRE_OBTER_DADOSResult
                                {
                                    Grupo = "% MARGEM CONTRIBUIÇÃO",
                                    Linha = "% ATACADO",
                                    JANEIRO = pJaneiro,
                                    FEVEREIRO = pFevereiro,
                                    MARCO = pMarco,
                                    ABRIL = pAbril,
                                    MAIO = pMaio,
                                    JUNHO = pJunho,
                                    JULHO = pJulho,
                                    AGOSTO = pAgosto,
                                    SETEMBRO = pSetembro,
                                    OUTUBRO = pOutubro,
                                    NOVEMBRO = pNovembro,
                                    DEZEMBRO = pDezembro
                                });

                                //OBTER MARK-UP
                                pJaneiro = ((gListaCmvAtacado.Sum(i => i.JANEIRO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.JANEIRO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.JANEIRO))));
                                pFevereiro = ((gListaCmvAtacado.Sum(i => i.FEVEREIRO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.FEVEREIRO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.FEVEREIRO))));
                                pMarco = ((gListaCmvAtacado.Sum(i => i.MARCO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.MARCO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.MARCO))));
                                pAbril = ((gListaCmvAtacado.Sum(i => i.ABRIL)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.ABRIL)) / ((-1) * (gListaCmvAtacado.Sum(i => i.ABRIL))));
                                pMaio = ((gListaCmvAtacado.Sum(i => i.MAIO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.MAIO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.MAIO))));
                                pJunho = ((gListaCmvAtacado.Sum(i => i.JUNHO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.JUNHO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.JUNHO))));
                                pJulho = ((gListaCmvAtacado.Sum(i => i.JULHO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.JULHO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.JULHO))));
                                pAgosto = ((gListaCmvAtacado.Sum(i => i.AGOSTO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.AGOSTO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.AGOSTO))));
                                pSetembro = ((gListaCmvAtacado.Sum(i => i.SETEMBRO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.SETEMBRO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.SETEMBRO))));
                                pOutubro = ((gListaCmvAtacado.Sum(i => i.OUTUBRO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.OUTUBRO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.OUTUBRO))));
                                pNovembro = ((gListaCmvAtacado.Sum(i => i.NOVEMBRO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.NOVEMBRO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.NOVEMBRO))));
                                pDezembro = ((gListaCmvAtacado.Sum(i => i.DEZEMBRO)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.DEZEMBRO)) / ((-1) * (gListaCmvAtacado.Sum(i => i.DEZEMBRO))));

                                margem.Insert(1, new SP_DRE_OBTER_DADOSResult
                                {
                                    Grupo = "MARK-UP",
                                    Linha = "MARK-UP ATACADO",
                                    JANEIRO = pJaneiro,
                                    FEVEREIRO = pFevereiro,
                                    MARCO = pMarco,
                                    ABRIL = pAbril,
                                    MAIO = pMaio,
                                    JUNHO = pJunho,
                                    JULHO = pJulho,
                                    AGOSTO = pAgosto,
                                    SETEMBRO = pSetembro,
                                    OUTUBRO = pOutubro,
                                    NOVEMBRO = pNovembro,
                                    DEZEMBRO = pDezembro
                                });

                                gTotalReceitaBrutaAtacado = 0;
                                gTotalReceitaBrutaAtacado = Convert.ToDecimal((
                                                                          FiltroValor(gListaAtacado.Sum(i => i.JANEIRO), 1)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.FEVEREIRO), 2)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.MARCO), 3)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.ABRIL), 4)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.MAIO), 5)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.JUNHO), 6)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.JULHO), 7)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.AGOSTO), 8)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.SETEMBRO), 9)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.OUTUBRO), 10)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.NOVEMBRO), 11)
                                                                        + FiltroValor(gListaAtacado.Sum(i => i.DEZEMBRO), 12)));

                                gTotalCustoProdutoAtacado = 0;
                                gTotalCustoProdutoAtacado = Convert.ToDecimal((
                                                                          FiltroValor(gListaCmvAtacado.Sum(i => i.JANEIRO), 1)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.FEVEREIRO), 2)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.MARCO), 3)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.ABRIL), 4)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.MAIO), 5)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.JUNHO), 6)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.JULHO), 7)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.AGOSTO), 8)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.SETEMBRO), 9)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.OUTUBRO), 10)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.NOVEMBRO), 11)
                                                                        + FiltroValor(gListaCmvAtacado.Sum(i => i.DEZEMBRO), 12)));
                            }

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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Grupo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim().Contains("MARK-UP")))
                                _litSubItem.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;<i>" + subitem.Grupo.Replace("MARGEM", "MARG").Trim() + "</i></font></span>";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.JANEIRO, 1)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1)).Trim();
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)).Trim();
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.MARCO, 3)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.ABRIL, 4)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4)).Trim();
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.MAIO, 5)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5)).Trim();
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.JUNHO, 6)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6)).Trim();
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.JULHO, 7)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7)).Trim();
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.AGOSTO, 8)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8)).Trim();
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.SETEMBRO, 9)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9)).Trim();
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.OUTUBRO, 10)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10)).Trim();
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)).Trim();
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            if (subitem.Linha.Contains("%") || (subitem.Linha.Trim() == "MARK-UP"))
                                _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)).Trim() + ((subitem.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)).Trim();

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        //guarda o total da margem de contribuicao para utilizar durante o loop da linha
                        if (_total != null)
                        {
                            if (subitem.Linha.Contains("%") && subitem.Linha.Contains("VAREJO"))
                            {
                                _total.Text = "<span style='font-weight:normal'>" + ((gTotalReceitaBrutaVarejo == 0) ? "0,00" : FormatarValor((gTotalMargemContribuicaoVarejo / gTotalReceitaBrutaVarejo) * 100).Trim()) + "</span><font size='2' face='Calibri'>%</font>";
                            }
                            else if (subitem.Linha.Trim().Contains("MARK-UP") && subitem.Linha.Contains("VAREJO"))
                            {
                                _total.Text = "<span style='font-weight:normal'>" + ((gTotalCustoProdutoVarejo == 00) ? "0,00" : FormatarValor((gTotalReceitaBrutaVarejo / gTotalCustoProdutoVarejo) * (-1))) + "</span>";
                            }
                            else if (subitem.Linha.Contains("%") && subitem.Linha.Contains("ATACADO"))
                            {
                                _total.Text = "<span style='font-weight:normal'>" + ((gTotalReceitaBrutaAtacado == 0) ? "0,00" : FormatarValor((gTotalMargemContribuicaoAtacado / gTotalReceitaBrutaAtacado) * 100).Trim()) + "</span><font size='2' face='Calibri'>%</font>";
                            }
                            else if (subitem.Linha.Trim().Contains("MARK-UP") && subitem.Linha.Contains("ATACADO"))
                            {
                                _total.Text = "<span style='font-weight:normal'>" + ((gTotalCustoProdutoAtacado == 0) ? "0,00" : FormatarValor((gTotalReceitaBrutaAtacado / gTotalCustoProdutoAtacado) * (-1))) + "</span>";
                            }
                        }
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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult cfixo = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (cfixo != null && cfixo.Grupo != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litCustoFixo") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + cfixo.Grupo.Trim() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(cfixo.JANEIRO, 1) + FiltroValor(cfixo.FEVEREIRO, 2) + FiltroValor(cfixo.MARCO, 3) + FiltroValor(cfixo.ABRIL, 4) + FiltroValor(cfixo.MAIO, 5) + FiltroValor(cfixo.JUNHO, 6) + FiltroValor(cfixo.JULHO, 7) + FiltroValor(cfixo.AGOSTO, 8) + FiltroValor(cfixo.SETEMBRO, 9) + FiltroValor(cfixo.OUTUBRO, 10) + FiltroValor(cfixo.NOVEMBRO, 11) + FiltroValor(cfixo.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha, 0);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(FiltroValor(cfixo.JANEIRO, 1));
                        dFevereiro += Convert.ToDecimal(FiltroValor(cfixo.FEVEREIRO, 2));
                        dMarco += Convert.ToDecimal(FiltroValor(cfixo.MARCO, 3));
                        dAbril += Convert.ToDecimal(FiltroValor(cfixo.ABRIL, 4));
                        dMaio += Convert.ToDecimal(FiltroValor(cfixo.MAIO, 5));
                        dJunho += Convert.ToDecimal(FiltroValor(cfixo.JUNHO, 6));
                        dJulho += Convert.ToDecimal(FiltroValor(cfixo.JULHO, 7));
                        dAgosto += Convert.ToDecimal(FiltroValor(cfixo.AGOSTO, 8));
                        dSetembro += Convert.ToDecimal(FiltroValor(cfixo.SETEMBRO, 9));
                        dOutubro += Convert.ToDecimal(FiltroValor(cfixo.OUTUBRO, 10));
                        dNovembro += Convert.ToDecimal(FiltroValor(cfixo.NOVEMBRO, 11));
                        dDezembro += Convert.ToDecimal(FiltroValor(cfixo.DEZEMBRO, 12));
                        dTotal += totalLinha;

                    }

                    if (cfixo.Grupo == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cfixo.Id == 1)
                        {
                            GridView gvCustoFixoItem = e.Row.FindControl("gvCustoFixoItem") as GridView;
                            if (gvCustoFixoItem != null)
                            {
                                var custoFixoLinha = gListaCustoFixo.ToList();
                                custoFixoLinha = custoFixoLinha.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();
                                gvCustoFixoItem.DataSource = custoFixoLinha;
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

            //ADD MARGEM BRUTA 
            gListaMargemBruta.AddRange(gListaMargemContribuicao.Where(p => p.Grupo != ""));

            // CUSTO FIXO
            gListaMargemBruta.AddRange(gListaCustoFixo.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "MARGEM DE CONTRIBUIÇÃO",
                                                                                Linha = "AGRUPADO",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }));

            // SOMAR MARGEM DE CONTRIBUIÇÃO
            gListaMargemBruta = gListaMargemBruta.GroupBy(p => new { Grupo = p.Grupo }).Select(k => new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "MARGEM BRUTA",
                Linha = "AGRUPADO",
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
                DEZEMBRO = k.Sum(g => g.DEZEMBRO)
            }).ToList();


            if (gListaMargemBruta.Count > 0)
            {

                //OBTER % MARGEM BRUTA
                decimal pJaneiro = (gListaReceitaLiquida.Sum(i => i.JANEIRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.JANEIRO) / gListaReceitaLiquida.Sum(i => i.JANEIRO)) * 100));
                decimal pFevereiro = (gListaReceitaLiquida.Sum(i => i.FEVEREIRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.FEVEREIRO) / gListaReceitaLiquida.Sum(i => i.FEVEREIRO)) * 100));
                decimal pMarco = (gListaReceitaLiquida.Sum(i => i.MARCO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.MARCO) / gListaReceitaLiquida.Sum(i => i.MARCO)) * 100));
                decimal pAbril = (gListaReceitaLiquida.Sum(i => i.ABRIL) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.ABRIL) / gListaReceitaLiquida.Sum(i => i.ABRIL)) * 100));
                decimal pMaio = (gListaReceitaLiquida.Sum(i => i.MAIO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.MAIO) / gListaReceitaLiquida.Sum(i => i.MAIO)) * 100));
                decimal pJunho = (gListaReceitaLiquida.Sum(i => i.JUNHO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.JUNHO) / gListaReceitaLiquida.Sum(i => i.JUNHO)) * 100));
                decimal pJulho = (gListaReceitaLiquida.Sum(i => i.JULHO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.JULHO) / gListaReceitaLiquida.Sum(i => i.JULHO)) * 100));
                decimal pAgosto = (gListaReceitaLiquida.Sum(i => i.AGOSTO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.AGOSTO) / gListaReceitaLiquida.Sum(i => i.AGOSTO)) * 100));
                decimal pSetembro = (gListaReceitaLiquida.Sum(i => i.SETEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.SETEMBRO) / gListaReceitaLiquida.Sum(i => i.SETEMBRO)) * 100));
                decimal pOutubro = (gListaReceitaLiquida.Sum(i => i.OUTUBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.OUTUBRO) / gListaReceitaLiquida.Sum(i => i.OUTUBRO)) * 100));
                decimal pNovembro = (gListaReceitaLiquida.Sum(i => i.NOVEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.NOVEMBRO) / gListaReceitaLiquida.Sum(i => i.NOVEMBRO)) * 100));
                decimal pDezembro = (gListaReceitaLiquida.Sum(i => i.DEZEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.DEZEMBRO) / gListaReceitaLiquida.Sum(i => i.DEZEMBRO)) * 100));

                gListaMargemBruta.Insert(gListaMargemBruta.Count, new SP_DRE_OBTER_DADOSResult
                {
                    Id = 98,
                    Grupo = "% MARGEM BRUTA",
                    Linha = "%",
                    JANEIRO = pJaneiro,
                    FEVEREIRO = pFevereiro,
                    MARCO = pMarco,
                    ABRIL = pAbril,
                    MAIO = pMaio,
                    JUNHO = pJunho,
                    JULHO = pJulho,
                    AGOSTO = pAgosto,
                    SETEMBRO = pSetembro,
                    OUTUBRO = pOutubro,
                    NOVEMBRO = pNovembro,
                    DEZEMBRO = pDezembro
                });

                gTotalReceitaLiquida = 0;
                gTotalReceitaLiquida = Convert.ToDecimal((gListaReceitaLiquida.Sum(i => i.JANEIRO)
                                                        + gListaReceitaLiquida.Sum(i => i.FEVEREIRO)
                                                        + gListaReceitaLiquida.Sum(i => i.MARCO)
                                                        + gListaReceitaLiquida.Sum(i => i.ABRIL)
                                                        + gListaReceitaLiquida.Sum(i => i.MAIO)
                                                        + gListaReceitaLiquida.Sum(i => i.JUNHO)
                                                        + gListaReceitaLiquida.Sum(i => i.JULHO)
                                                        + gListaReceitaLiquida.Sum(i => i.AGOSTO)
                                                        + gListaReceitaLiquida.Sum(i => i.SETEMBRO)
                                                        + gListaReceitaLiquida.Sum(i => i.OUTUBRO)
                                                        + gListaReceitaLiquida.Sum(i => i.NOVEMBRO)
                                                        + gListaReceitaLiquida.Sum(i => i.DEZEMBRO)));

                gListaMargemBruta.Insert(gListaMargemBruta.Count, new SP_DRE_OBTER_DADOSResult { Id = 97, Grupo = "" });
                //gListaMargemBruta.Insert(gListaMargemBruta.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });

                //Carregar margem bruta
                gvMargemBruta.DataSource = gListaMargemBruta;
                gvMargemBruta.DataBind();
            }
        }

        protected void gvCustoFixoItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult custoFixoLinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (custoFixoLinha != null && custoFixoLinha.Linha != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + custoFixoLinha.Linha.Trim().ToUpper() + "</font></span>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=1&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _janeiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.JANEIRO, 1)) + "</a>";
                        }

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=2&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _fevereiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.FEVEREIRO, 2)) + "</a>";
                        }

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=3&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _marco.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.MARCO, 3)) + "</a>";
                        }

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=4&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _abril.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.ABRIL, 4)) + "</a>";
                        }

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=5&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _maio.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.MAIO, 5)) + "</a>";
                        }

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=6&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _junho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.JUNHO, 6)) + "</a>";
                        }

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=7&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _julho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.JULHO, 7)) + "</a>";
                        }

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=8&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _agosto.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.AGOSTO, 8)) + "</a>";
                        }

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=9&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _setembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.SETEMBRO, 9)) + "</a>";
                        }

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=10&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _outubro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.OUTUBRO, 10)) + "</a>";
                        }

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=11&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _novembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.NOVEMBRO, 11)) + "</a>";
                        }

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            url_link = "dre_dre_custo_fixo.aspx?m=12&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=" + custoFixoLinha.Linha;
                            _dezembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(custoFixoLinha.DEZEMBRO, 12)) + "</a>";
                        }

                        totalLinha = Convert.ToDecimal(FiltroValor(custoFixoLinha.JANEIRO, 1) + FiltroValor(custoFixoLinha.FEVEREIRO, 2) + FiltroValor(custoFixoLinha.MARCO, 3) + FiltroValor(custoFixoLinha.ABRIL, 4) + FiltroValor(custoFixoLinha.MAIO, 5) + FiltroValor(custoFixoLinha.JUNHO, 6) + FiltroValor(custoFixoLinha.JULHO, 7) + FiltroValor(custoFixoLinha.AGOSTO, 8) + FiltroValor(custoFixoLinha.SETEMBRO, 9) + FiltroValor(custoFixoLinha.OUTUBRO, 10) + FiltroValor(custoFixoLinha.NOVEMBRO, 11) + FiltroValor(custoFixoLinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                        if (img != null)
                        {
                            img.Visible = false;

                            if (
                                    custoFixoLinha.Linha.Trim().ToUpper() == "ENCARGOS" ||
                                    custoFixoLinha.Linha.Trim().ToUpper() == "BENEFICIOS")
                            {
                                GridView gvCustoFixoItemSub = e.Row.FindControl("gvCustoFixoItemSub") as GridView;
                                if (gvCustoFixoItemSub != null)
                                {
                                    gvCustoFixoItemSub.DataSource = gListaCustoFixo.Where(p => p.Linha.Trim().ToUpper() == custoFixoLinha.Linha.Trim().ToUpper()).GroupBy(j => new
                                    {
                                        TIPO = j.Tipo
                                    }).Select(k => new SP_DRE_OBTER_DADOSResult
                                    {
                                        Tipo = k.Key.TIPO.Trim().ToUpper(),
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
                                        DEZEMBRO = k.Sum(s => s.DEZEMBRO)
                                    });
                                    gvCustoFixoItemSub.DataBind();
                                }
                                img.Visible = true;
                            }
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Tipo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;" + subitem.Tipo.Trim().ToUpper().Replace("SALARIOLIQUIDO", "SALÁRIO LÍQUIDO") + "</font> ";

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

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult margemBruta = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (margemBruta != null && margemBruta.Grupo != "")
                    {
                        Literal _litMargemBruta = e.Row.FindControl("litMargemBruta") as Literal;
                        if (_litMargemBruta != null)
                            if (margemBruta.Linha.Contains("%"))
                                _litMargemBruta.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + margemBruta.Grupo.Trim() + "</i></font></span>";
                            else
                                _litMargemBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + margemBruta.Grupo.Trim() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            if (margemBruta.Linha.Contains("%"))
                                _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JANEIRO, 1)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _janeiro.Text = FormatarValor(FiltroValor(margemBruta.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            if (margemBruta.Linha.Contains("%"))
                                _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.FEVEREIRO, 2)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _fevereiro.Text = FormatarValor(FiltroValor(margemBruta.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            if (margemBruta.Linha.Contains("%"))
                                _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.MARCO, 3)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _marco.Text = FormatarValor(FiltroValor(margemBruta.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            if (margemBruta.Linha.Contains("%"))
                                _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.ABRIL, 4)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _abril.Text = FormatarValor(FiltroValor(margemBruta.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            if (margemBruta.Linha.Contains("%"))
                                _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.MAIO, 5)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _maio.Text = FormatarValor(FiltroValor(margemBruta.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            if (margemBruta.Linha.Contains("%"))
                                _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JUNHO, 6)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _junho.Text = FormatarValor(FiltroValor(margemBruta.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            if (margemBruta.Linha.Contains("%"))
                                _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JULHO, 7)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _julho.Text = FormatarValor(FiltroValor(margemBruta.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            if (margemBruta.Linha.Contains("%"))
                                _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.AGOSTO, 8)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _agosto.Text = FormatarValor(FiltroValor(margemBruta.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            if (margemBruta.Linha.Contains("%"))
                                _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.SETEMBRO, 9)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _setembro.Text = FormatarValor(FiltroValor(margemBruta.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            if (margemBruta.Linha.Contains("%"))
                                _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.OUTUBRO, 10)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _outubro.Text = FormatarValor(FiltroValor(margemBruta.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            if (margemBruta.Linha.Contains("%"))
                                _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.NOVEMBRO, 11)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _novembro.Text = FormatarValor(FiltroValor(margemBruta.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            if (margemBruta.Linha.Contains("%") || (margemBruta.Linha.Trim() == "MARK-UP"))
                                _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.DEZEMBRO, 12)).Trim() + ((margemBruta.Linha.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _dezembro.Text = FormatarValor(FiltroValor(margemBruta.DEZEMBRO, 12)).Trim();

                        totalLinha = Convert.ToDecimal(FiltroValor(margemBruta.JANEIRO, 1) + FiltroValor(margemBruta.FEVEREIRO, 2) + FiltroValor(margemBruta.MARCO, 3) + FiltroValor(margemBruta.ABRIL, 4) + FiltroValor(margemBruta.MAIO, 5) + FiltroValor(margemBruta.JUNHO, 6) + FiltroValor(margemBruta.JULHO, 7) + FiltroValor(margemBruta.AGOSTO, 8) + FiltroValor(margemBruta.SETEMBRO, 9) + FiltroValor(margemBruta.OUTUBRO, 10) + FiltroValor(margemBruta.NOVEMBRO, 11) + FiltroValor(margemBruta.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;

                        //guarda o total da margem de contribuicao para utilizar durante o loop da linha
                        if (_total != null)
                            if (margemBruta.Linha.Contains("%")) //totalLinha g
                                _total.Text = "<span style='font-weight:normal'>" + ((gTotalReceitaLiquida == 0) ? "0" : FormatarValor((gTotalMargemBruta / gTotalReceitaLiquida) * 100).Trim()) + "</span><font size='2' face='Calibri'>%</font>";
                            else
                            {
                                _total.Text = FormatarValor(totalLinha);
                                gTotalMargemBruta = totalLinha;
                            }

                        //SOMATORIO
                        dJaneiro += FiltroValor(margemBruta.JANEIRO, 1);
                        dFevereiro += FiltroValor(margemBruta.FEVEREIRO, 2);
                        dMarco += FiltroValor(margemBruta.MARCO, 3);
                        dAbril += FiltroValor(margemBruta.ABRIL, 4);
                        dMaio += FiltroValor(margemBruta.MAIO, 5);
                        dJunho += FiltroValor(margemBruta.JUNHO, 6);
                        dJulho += FiltroValor(margemBruta.JULHO, 7);
                        dAgosto += FiltroValor(margemBruta.AGOSTO, 8);
                        dSetembro += FiltroValor(margemBruta.SETEMBRO, 9);
                        dOutubro += FiltroValor(margemBruta.OUTUBRO, 10);
                        dNovembro += FiltroValor(margemBruta.NOVEMBRO, 11);
                        dDezembro += FiltroValor(margemBruta.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (margemBruta.Grupo == "")
                        e.Row.Height = Unit.Pixel(1);

                }
            }
        }
        protected void gvMargemBruta_DataBound(object sender, EventArgs e)
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

        #endregion

        #region "DESPESAS CTO"

        protected void gvDespesaCTO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "#";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult desploja = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (desploja != null && desploja.Grupo != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                        {
                            //url_link = "../desp_loja/desp_loja_grafico.aspx";
                            _litDespesa.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + desploja.Grupo.Trim() + "'>&nbsp;" + desploja.Grupo.Trim().ToUpper() + "</a></font> ";
                        }
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(desploja.JANEIRO, 1), 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(desploja.FEVEREIRO, 2), 2);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(desploja.MARCO, 3), 3);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(desploja.ABRIL, 4), 4);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(desploja.MAIO, 5), 5);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(desploja.JUNHO, 6), 6);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(desploja.JULHO, 7), 7);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(desploja.AGOSTO, 8), 8);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(desploja.SETEMBRO, 9), 9);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(desploja.OUTUBRO, 10), 10);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(desploja.NOVEMBRO, 11), 11);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(desploja.DEZEMBRO, 12), 12);

                        totalLinha = Convert.ToDecimal(FiltroValor(desploja.JANEIRO, 1) + FiltroValor(desploja.FEVEREIRO, 2) + FiltroValor(desploja.MARCO, 3) + FiltroValor(desploja.ABRIL, 4) + FiltroValor(desploja.MAIO, 5) + FiltroValor(desploja.JUNHO, 6) + FiltroValor(desploja.JULHO, 7) + FiltroValor(desploja.AGOSTO, 8) + FiltroValor(desploja.SETEMBRO, 9) + FiltroValor(desploja.OUTUBRO, 10) + FiltroValor(desploja.NOVEMBRO, 11) + FiltroValor(desploja.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha, 0);

                        //SOMATORIO
                        dJaneiro += FiltroValor(desploja.JANEIRO, 1);
                        dFevereiro += FiltroValor(desploja.FEVEREIRO, 2);
                        dMarco += FiltroValor(desploja.MARCO, 3);
                        dAbril += FiltroValor(desploja.ABRIL, 4);
                        dMaio += FiltroValor(desploja.MAIO, 5);
                        dJunho += FiltroValor(desploja.JUNHO, 6);
                        dJulho += FiltroValor(desploja.JULHO, 7);
                        dAgosto += FiltroValor(desploja.AGOSTO, 8);
                        dSetembro += FiltroValor(desploja.SETEMBRO, 9);
                        dOutubro += FiltroValor(desploja.OUTUBRO, 10);
                        dNovembro += FiltroValor(desploja.NOVEMBRO, 11);
                        dDezembro += FiltroValor(desploja.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (desploja.Grupo == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (desploja.Id == 1)
                        {
                            GridView gvDespesaCTOItem = e.Row.FindControl("gvDespesaCTOItem") as GridView;
                            if (gvDespesaCTOItem != null)
                            {
                                var despCTOLinha = gListaDespesaCTO;
                                despCTOLinha = despCTOLinha.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).OrderBy(y => y.Linha).ToList();
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult desplojalinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (desplojalinha != null && desplojalinha.Linha != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;" + (((desplojalinha.Linha.Trim().Length < 18) ? desplojalinha.Linha.Trim() : desplojalinha.Linha.Trim().Substring(0, 18))) + "</font></span>";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(desplojalinha.JANEIRO, 1) + FiltroValor(desplojalinha.FEVEREIRO, 2) + FiltroValor(desplojalinha.MARCO, 3) + FiltroValor(desplojalinha.ABRIL, 4) + FiltroValor(desplojalinha.MAIO, 5) + FiltroValor(desplojalinha.JUNHO, 6) + FiltroValor(desplojalinha.JULHO, 7) + FiltroValor(desplojalinha.AGOSTO, 8) + FiltroValor(desplojalinha.SETEMBRO, 9) + FiltroValor(desplojalinha.OUTUBRO, 10) + FiltroValor(desplojalinha.NOVEMBRO, 11) + FiltroValor(desplojalinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        //img.Visible = false;
                        GridView gvDespesaCTOItemSub = e.Row.FindControl("gvDespesaCTOItemSub") as GridView;
                        if (gvDespesaCTOItemSub != null)
                        {
                            gvDespesaCTOItemSub.DataSource = gListaDespesaCTO.Where(p => p.Linha.Trim().ToUpper() == desplojalinha.Linha.Trim().ToUpper());
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Grupo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.Filial.Trim().ToUpper() + "</font> ";

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

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;
            string url_link = "#";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult desploja = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (desploja != null && desploja.Grupo != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                        {
                            //url_link = "../desp_loja/desp_loja_grafico.aspx";
                            _litDespesa.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + desploja.Grupo.Trim() + "'>&nbsp;" + desploja.Grupo.Trim().ToUpper() + "</a></font> ";
                        }
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(FiltroValor(desploja.JANEIRO, 1), 1);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(FiltroValor(desploja.FEVEREIRO, 2), 2);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(FiltroValor(desploja.MARCO, 3), 3);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(FiltroValor(desploja.ABRIL, 4), 4);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(FiltroValor(desploja.MAIO, 5), 5);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(FiltroValor(desploja.JUNHO, 6), 6);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(FiltroValor(desploja.JULHO, 7), 7);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(FiltroValor(desploja.AGOSTO, 8), 8);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(FiltroValor(desploja.SETEMBRO, 9), 9);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(FiltroValor(desploja.OUTUBRO, 10), 10);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(FiltroValor(desploja.NOVEMBRO, 11), 11);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(FiltroValor(desploja.DEZEMBRO, 12), 12);

                        totalLinha = Convert.ToDecimal(FiltroValor(desploja.JANEIRO, 1) + FiltroValor(desploja.FEVEREIRO, 2) + FiltroValor(desploja.MARCO, 3) + FiltroValor(desploja.ABRIL, 4) + FiltroValor(desploja.MAIO, 5) + FiltroValor(desploja.JUNHO, 6) + FiltroValor(desploja.JULHO, 7) + FiltroValor(desploja.AGOSTO, 8) + FiltroValor(desploja.SETEMBRO, 9) + FiltroValor(desploja.OUTUBRO, 10) + FiltroValor(desploja.NOVEMBRO, 11) + FiltroValor(desploja.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha, 0);

                        //SOMATORIO
                        dJaneiro += FiltroValor(desploja.JANEIRO, 1);
                        dFevereiro += FiltroValor(desploja.FEVEREIRO, 2);
                        dMarco += FiltroValor(desploja.MARCO, 3);
                        dAbril += FiltroValor(desploja.ABRIL, 4);
                        dMaio += FiltroValor(desploja.MAIO, 5);
                        dJunho += FiltroValor(desploja.JUNHO, 6);
                        dJulho += FiltroValor(desploja.JULHO, 7);
                        dAgosto += FiltroValor(desploja.AGOSTO, 8);
                        dSetembro += FiltroValor(desploja.SETEMBRO, 9);
                        dOutubro += FiltroValor(desploja.OUTUBRO, 10);
                        dNovembro += FiltroValor(desploja.NOVEMBRO, 11);
                        dDezembro += FiltroValor(desploja.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (desploja.Grupo == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (desploja.Id == 1)
                        {
                            GridView gvDespesaEspecificaItem = e.Row.FindControl("gvDespesaEspecificaItem") as GridView;
                            if (gvDespesaEspecificaItem != null)
                            {
                                var despEspLinha = gListaDespesaEspecifica;
                                despEspLinha = despEspLinha.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).OrderBy(y => y.Linha).ToList();
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult desplojalinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (desplojalinha != null && desplojalinha.Linha != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;" + (((desplojalinha.Linha.Trim().Length < 18) ? desplojalinha.Linha.Trim() : desplojalinha.Linha.Trim().Substring(0, 18))) + "</font></span>";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(desplojalinha.JANEIRO, 1) + FiltroValor(desplojalinha.FEVEREIRO, 2) + FiltroValor(desplojalinha.MARCO, 3) + FiltroValor(desplojalinha.ABRIL, 4) + FiltroValor(desplojalinha.MAIO, 5) + FiltroValor(desplojalinha.JUNHO, 6) + FiltroValor(desplojalinha.JULHO, 7) + FiltroValor(desplojalinha.AGOSTO, 8) + FiltroValor(desplojalinha.SETEMBRO, 9) + FiltroValor(desplojalinha.OUTUBRO, 10) + FiltroValor(desplojalinha.NOVEMBRO, 11) + FiltroValor(desplojalinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        //img.Visible = false;
                        GridView gvDespesaEspecificaItemSub = e.Row.FindControl("gvDespesaEspecificaItemSub") as GridView;
                        if (gvDespesaEspecificaItemSub != null)
                        {
                            gvDespesaEspecificaItemSub.DataSource = gListaDespesaEspecifica.Where(p => p.Linha.Trim().ToUpper() == desplojalinha.Linha.Trim().ToUpper());
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Grupo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.Filial.Trim().ToUpper() + "</font> ";

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

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult despvenda = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (despvenda != null && despvenda.Grupo != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                        {
                            url_link = "#";
                            _litDespesa.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + despvenda.Grupo.Trim() + "'>&nbsp;" + despvenda.Grupo.Trim().ToUpper() + "</a></font> ";
                        }
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

                        totalLinha = Convert.ToDecimal(FiltroValor(despvenda.JANEIRO, 1) + FiltroValor(despvenda.FEVEREIRO, 2) + FiltroValor(despvenda.MARCO, 3) + FiltroValor(despvenda.ABRIL, 4) + FiltroValor(despvenda.MAIO, 5) + FiltroValor(despvenda.JUNHO, 6) + FiltroValor(despvenda.JULHO, 7) + FiltroValor(despvenda.AGOSTO, 8) + FiltroValor(despvenda.SETEMBRO, 9) + FiltroValor(despvenda.OUTUBRO, 10) + FiltroValor(despvenda.NOVEMBRO, 11) + FiltroValor(despvenda.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha, 0);

                        //SOMATORIO
                        dJaneiro += FiltroValor(despvenda.JANEIRO, 1);
                        dFevereiro += FiltroValor(despvenda.FEVEREIRO, 2);
                        dMarco += FiltroValor(despvenda.MARCO, 3);
                        dAbril += FiltroValor(despvenda.ABRIL, 4);
                        dMaio += FiltroValor(despvenda.MAIO, 5);
                        dJunho += FiltroValor(despvenda.JUNHO, 6);
                        dJulho += FiltroValor(despvenda.JULHO, 7);
                        dAgosto += FiltroValor(despvenda.AGOSTO, 8);
                        dSetembro += FiltroValor(despvenda.SETEMBRO, 9);
                        dOutubro += FiltroValor(despvenda.OUTUBRO, 10);
                        dNovembro += FiltroValor(despvenda.NOVEMBRO, 11);
                        dDezembro += FiltroValor(despvenda.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (despvenda.Grupo == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (despvenda.Id == 1)
                        {
                            GridView gvDespesaVendaItem = e.Row.FindControl("gvDespesaVendaItem") as GridView;
                            if (gvDespesaVendaItem != null)
                            {
                                var despVendaLinha = gListaDespesaVenda;
                                despVendaLinha = despVendaLinha.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult despvendalinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (despvendalinha != null && despvendalinha.Linha != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span><font size='2' face='Calibri'>&nbsp;" + despvendalinha.Linha.Trim().ToUpper().Substring(0, ((despvendalinha.Linha.Trim().Length > 19) ? 19 : despvendalinha.Linha.Trim().Length)) + "</font></span>";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(despvendalinha.JANEIRO, 1) + FiltroValor(despvendalinha.FEVEREIRO, 2) + FiltroValor(despvendalinha.MARCO, 3) + FiltroValor(despvendalinha.ABRIL, 4) + FiltroValor(despvendalinha.MAIO, 5) + FiltroValor(despvendalinha.JUNHO, 6) + FiltroValor(despvendalinha.JULHO, 7) + FiltroValor(despvendalinha.AGOSTO, 8) + FiltroValor(despvendalinha.SETEMBRO, 9) + FiltroValor(despvendalinha.OUTUBRO, 10) + FiltroValor(despvendalinha.NOVEMBRO, 11) + FiltroValor(despvendalinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;

                        if (despvendalinha.Linha.Trim().ToUpper() == "SALÁRIOS" ||
                                despvendalinha.Linha.Trim().ToUpper() == "ENCARGOS" ||
                                despvendalinha.Linha.Trim().ToUpper() == "BENEFICIOS")
                        {
                            GridView gvDespesaVendaItemSub = e.Row.FindControl("gvDespesaVendaItemSub") as GridView;
                            if (gvDespesaVendaItemSub != null)
                            {
                                gvDespesaVendaItemSub.DataSource = gListaDespesaVenda.Where(p => p.Linha.Trim().ToUpper() == despvendalinha.Linha.Trim().ToUpper()).GroupBy(j => new
                                {
                                    TIPO = j.Tipo
                                }).Select(k => new SP_DRE_OBTER_DADOSResult
                                {
                                    Tipo = k.Key.TIPO.Trim().ToUpper(),
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
                                    DEZEMBRO = k.Sum(s => s.DEZEMBRO)
                                });
                                gvDespesaVendaItemSub.DataBind();
                            }
                            img.Visible = true;
                        }
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Tipo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;" + subitem.Tipo.Trim().ToUpper().Replace("SALARIOLIQUIDO", "SALÁRIO LÍQUIDO") + "</font> ";

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

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult despadm = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (despadm != null && despadm.Grupo != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                        {
                            url_link = "#";
                            _litDespesa.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + despadm.Grupo.Trim() + "'>&nbsp;" + despadm.Grupo.Trim().ToUpper() + "</a></font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(despadm.JANEIRO, 1) + FiltroValor(despadm.FEVEREIRO, 2) + FiltroValor(despadm.MARCO, 3) + FiltroValor(despadm.ABRIL, 4) + FiltroValor(despadm.MAIO, 5) + FiltroValor(despadm.JUNHO, 6) + FiltroValor(despadm.JULHO, 7) + FiltroValor(despadm.AGOSTO, 8) + FiltroValor(despadm.SETEMBRO, 9) + FiltroValor(despadm.OUTUBRO, 10) + FiltroValor(despadm.NOVEMBRO, 11) + FiltroValor(despadm.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha, 0);

                        //SOMATORIO
                        dJaneiro += FiltroValor(despadm.JANEIRO, 1);
                        dFevereiro += FiltroValor(despadm.FEVEREIRO, 2);
                        dMarco += FiltroValor(despadm.MARCO, 3);
                        dAbril += FiltroValor(despadm.ABRIL, 4);
                        dMaio += FiltroValor(despadm.MAIO, 5);
                        dJunho += FiltroValor(despadm.JUNHO, 6);
                        dJulho += FiltroValor(despadm.JULHO, 7);
                        dAgosto += FiltroValor(despadm.AGOSTO, 8);
                        dSetembro += FiltroValor(despadm.SETEMBRO, 9);
                        dOutubro += FiltroValor(despadm.OUTUBRO, 10);
                        dNovembro += FiltroValor(despadm.NOVEMBRO, 11);
                        dDezembro += FiltroValor(despadm.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (despadm.Grupo == "")
                        e.Row.Height = Unit.Pixel(3);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (despadm.Id == 1)
                        {
                            GridView gvDespesaAdmItem = e.Row.FindControl("gvDespesaAdmItem") as GridView;
                            if (gvDespesaAdmItem != null)
                            {
                                var despAdmPessoal = gListaDespesaAdmPessoal;
                                despAdmPessoal = despAdmPessoal.GroupBy(b => new { LINHA = b.Linha.Trim().Substring(0, 7) }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();

                                var despAdmOutros = gListaDespesaAdmOutros;
                                despAdmOutros = despAdmOutros.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 2,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();

                                gvDespesaAdmItem.DataSource = despAdmPessoal.Union(despAdmOutros);
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


            /*BIND EBITDA*/
            //  ADD MARGEM BRUTA
            gListaEBITDA.AddRange(gListaMargemBruta.Where(p => p.Id == 99));

            // ADD DESPESA CTO - DESPESAS CTO
            gListaEBITDA.AddRange(gListaDespesaCTO.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 99,
                                                                                Grupo = "DESPESAS CTO",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList());


            //  ADD DESPESA ESP - DESPESAS ESPECÍFICAS
            gListaEBITDA.AddRange(gListaDespesaEspecifica.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 99,
                                                                                Grupo = "DESPESAS ESPECÍFICAS",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList());
            //  ADD DESPESA VENDA
            gListaEBITDA.AddRange(gListaDespesaVenda.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 99,
                                                                                Linha = "DESPESAS VENDAS",
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
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList());
            //  ADD DESPESA ADMINISTRATIVA
            gListaEBITDA.AddRange(gListaDespesaAdmPessoal.Union(gListaDespesaAdmOutros).GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                           k => new SP_DRE_OBTER_DADOSResult
                                                                           {
                                                                               Id = 99,
                                                                               Linha = "DESPESAS ADM",
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
                                                                               DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                           }).ToList());

            //AGRUPA TUDO E SOMA OS MESES
            gListaEBITDA = gListaEBITDA.GroupBy(p => new { ID = p.Id }).Select(
                                                                           k => new SP_DRE_OBTER_DADOSResult
                                                                           {
                                                                               Id = 99,
                                                                               Grupo = "EBITDA",
                                                                               Linha = "",
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
                                                                               DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                           }).ToList();

            //ADD PORCENTAGEM
            if (gListaEBITDA.Count > 0)
            {
                //OBTER % MARGEM EBITDA
                decimal pJaneiro = (gListaReceitaLiquida.Sum(i => i.JANEIRO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.JANEIRO) / gListaReceitaLiquida.Sum(i => i.JANEIRO)) * 100));
                decimal pFevereiro = (gListaReceitaLiquida.Sum(i => i.FEVEREIRO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.FEVEREIRO) / gListaReceitaLiquida.Sum(i => i.FEVEREIRO)) * 100));
                decimal pMarco = (gListaReceitaLiquida.Sum(i => i.MARCO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.MARCO) / gListaReceitaLiquida.Sum(i => i.MARCO)) * 100));
                decimal pAbril = (gListaReceitaLiquida.Sum(i => i.ABRIL) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.ABRIL) / gListaReceitaLiquida.Sum(i => i.ABRIL)) * 100));
                decimal pMaio = (gListaReceitaLiquida.Sum(i => i.MAIO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.MAIO) / gListaReceitaLiquida.Sum(i => i.MAIO)) * 100));
                decimal pJunho = (gListaReceitaLiquida.Sum(i => i.JUNHO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.JUNHO) / gListaReceitaLiquida.Sum(i => i.JUNHO)) * 100));
                decimal pJulho = (gListaReceitaLiquida.Sum(i => i.JULHO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.JULHO) / gListaReceitaLiquida.Sum(i => i.JULHO)) * 100));
                decimal pAgosto = (gListaReceitaLiquida.Sum(i => i.AGOSTO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.AGOSTO) / gListaReceitaLiquida.Sum(i => i.AGOSTO)) * 100));
                decimal pSetembro = (gListaReceitaLiquida.Sum(i => i.SETEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.SETEMBRO) / gListaReceitaLiquida.Sum(i => i.SETEMBRO)) * 100));
                decimal pOutubro = (gListaReceitaLiquida.Sum(i => i.OUTUBRO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.OUTUBRO) / gListaReceitaLiquida.Sum(i => i.OUTUBRO)) * 100));
                decimal pNovembro = (gListaReceitaLiquida.Sum(i => i.NOVEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.NOVEMBRO) / gListaReceitaLiquida.Sum(i => i.NOVEMBRO)) * 100));
                decimal pDezembro = (gListaReceitaLiquida.Sum(i => i.DEZEMBRO) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.DEZEMBRO) / gListaReceitaLiquida.Sum(i => i.DEZEMBRO)) * 100));

                gListaEBITDA.Insert(gListaEBITDA.Count, new SP_DRE_OBTER_DADOSResult
                {
                    Id = 99,
                    Grupo = "% Margem EBITDA",
                    Linha = "%",
                    JANEIRO = pJaneiro,
                    FEVEREIRO = pFevereiro,
                    MARCO = pMarco,
                    ABRIL = pAbril,
                    MAIO = pMaio,
                    JUNHO = pJunho,
                    JULHO = pJulho,
                    AGOSTO = pAgosto,
                    SETEMBRO = pSetembro,
                    OUTUBRO = pOutubro,
                    NOVEMBRO = pNovembro,
                    DEZEMBRO = pDezembro
                });

                gTotalReceitaLiquida = 0;
                gTotalReceitaLiquida = Convert.ToDecimal((gListaReceitaLiquida.Sum(i => i.JANEIRO)
                                                        + gListaReceitaLiquida.Sum(i => i.FEVEREIRO)
                                                        + gListaReceitaLiquida.Sum(i => i.MARCO)
                                                        + gListaReceitaLiquida.Sum(i => i.ABRIL)
                                                        + gListaReceitaLiquida.Sum(i => i.MAIO)
                                                        + gListaReceitaLiquida.Sum(i => i.JUNHO)
                                                        + gListaReceitaLiquida.Sum(i => i.JULHO)
                                                        + gListaReceitaLiquida.Sum(i => i.AGOSTO)
                                                        + gListaReceitaLiquida.Sum(i => i.SETEMBRO)
                                                        + gListaReceitaLiquida.Sum(i => i.OUTUBRO)
                                                        + gListaReceitaLiquida.Sum(i => i.NOVEMBRO)
                                                        + gListaReceitaLiquida.Sum(i => i.DEZEMBRO)));
            }

            gListaEBITDA.Insert(gListaEBITDA.Count, new SP_DRE_OBTER_DADOSResult { Id = 99, Grupo = "", Linha = "" });

            gvEbtida.DataSource = gListaEBITDA;
            gvEbtida.DataBind();

        }

        protected void gvDespesaAdmItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "#";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult despadmlinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (despadmlinha != null && despadmlinha.Linha != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                        {
                            if (despadmlinha.Linha.Trim().Contains("UTILIDADES"))
                                url_link = "../desp_adm/adm_utilidades.aspx";
                            _litDespesa.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + despadmlinha.Linha.Trim() + "'>&nbsp;" + despadmlinha.Linha.Trim().ToUpper().Substring(0, ((despadmlinha.Linha.Trim().Length > 20) ? 20 : despadmlinha.Linha.Trim().Length)) + "</a></font> ";
                        }
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

                        totalLinha = Convert.ToDecimal(FiltroValor(despadmlinha.JANEIRO, 1) + FiltroValor(despadmlinha.FEVEREIRO, 2) + FiltroValor(despadmlinha.MARCO, 3) + FiltroValor(despadmlinha.ABRIL, 4) + FiltroValor(despadmlinha.MAIO, 5) + FiltroValor(despadmlinha.JUNHO, 6) + FiltroValor(despadmlinha.JULHO, 7) + FiltroValor(despadmlinha.AGOSTO, 8) + FiltroValor(despadmlinha.SETEMBRO, 9) + FiltroValor(despadmlinha.OUTUBRO, 10) + FiltroValor(despadmlinha.NOVEMBRO, 11) + FiltroValor(despadmlinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;


                        if (despadmlinha.Id == 1)
                        {
                            GridView gvDespesaAdmItemSub = e.Row.FindControl("gvDespesaAdmItemSub") as GridView;
                            if (gvDespesaAdmItemSub != null)
                            {
                                var lstdespAdmLinha = gListaDespesaAdmPessoal.GroupBy(j => new
                                    {
                                        LINHA = j.Linha.Replace("Pessoal - ", "")
                                    }).Select(k => new SP_DRE_OBTER_DADOSResult
                                    {
                                        Id = 1,
                                        Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                        DEZEMBRO = k.Sum(s => s.DEZEMBRO)
                                    }).ToList();

                                gvDespesaAdmItemSub.DataSource = lstdespAdmLinha.OrderBy(p => p.Tipo).ThenBy(x => x.Linha);
                                gvDespesaAdmItemSub.DataBind();
                            }
                            img.Visible = true;
                        }

                        if (despadmlinha.Id == 2)
                        {
                            GridView gvDespesaAdmItemSub = e.Row.FindControl("gvDespesaAdmItemSub") as GridView;
                            if (gvDespesaAdmItemSub != null)
                            {
                                var lstdespAdmLinha2 = gListaDespesaAdmOutros.Where(i => i.Linha.Trim().ToUpper() == despadmlinha.Linha.Trim().ToUpper()).GroupBy(j => new
                                {
                                    LINHA = j.Tipo.Trim()
                                }).Select(k => new SP_DRE_OBTER_DADOSResult
                                {
                                    Id = 1,
                                    Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                    DEZEMBRO = k.Sum(s => s.DEZEMBRO)
                                }).ToList();

                                gvDespesaAdmItemSub.DataSource = lstdespAdmLinha2.OrderBy(p => p.Tipo).ThenBy(x => x.Linha);
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

        /*protected void lnkSubItem_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            string msg = "";
            string _url = "";
            if (lnk != null)
            {
                try
                {
                    string descContaContabil = lnk.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_tripa_editar.aspx?p=" + descContaContabil + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
        }*/
        protected void gvDespesaAdmItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "";
            string conta_contabil = "";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Linha != "")
                    {

                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.Linha.Trim().ToUpper().Substring(0, ((subitem.Linha.Trim().Length > 21) ? 21 : subitem.Linha.Trim().Length)) + "</font> ";

                            var conta = new DesenvolvimentoController().ObterContaContabilDescricao(subitem.Linha.Trim()).Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 1) == "3").ToList();
                            if (conta != null && conta.Count() == 1)
                                conta_contabil = conta[0].CONTA_CONTABIL.Trim().Replace(".", "@");
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=1&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _janeiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(subitem.JANEIRO, 1)) + "</a>";
                        }
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=2&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _fevereiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)) + "</a>";
                        }
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=3&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _marco.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(subitem.MARCO, 3)) + "</a>";
                        }
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=4&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _abril.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(subitem.ABRIL, 4)) + "</a>";
                        }
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=5&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _maio.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(subitem.MAIO, 5)) + "</a>";
                        }
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=6&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _junho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(subitem.JUNHO, 6)) + "</a>";
                        }
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=7&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _julho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(subitem.JULHO, 7)) + "</a>";
                        }
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=8&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _agosto.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(subitem.AGOSTO, 8)) + "</a>";
                        }
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=9&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _setembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.SETEMBRO, 9)) + "</a>";
                        }
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=10&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _outubro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(subitem.OUTUBRO, 10)) + "</a>";
                        }
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=11&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _novembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)) + "</a>";
                        }
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=12&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _dezembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)) + "</a>";
                        }

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);
                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;

                        if (subitem.Linha.Trim().ToUpper() == "SALÁRIOS" ||
                                subitem.Linha.Trim().ToUpper() == "ENCARGOS" ||
                                subitem.Linha.Trim().ToUpper() == "BENEFICIOS")
                        {
                            GridView gvDespesaAdmItemSubSub = e.Row.FindControl("gvDespesaAdmItemSubSub") as GridView;
                            if (gvDespesaAdmItemSubSub != null)
                            {
                                var lstdespAdmSub = gListaDespesaAdmPessoal.Where(g => g.Linha.Trim().ToUpper().Contains(subitem.Linha.Trim().ToUpper())).GroupBy(j => new
                                {
                                    TIPO = j.Tipo.Trim().ToUpper()
                                }).Select(k => new SP_DRE_OBTER_DADOSResult
                                {
                                    Id = 1,
                                    Tipo = k.Key.TIPO.Trim().ToUpper(),
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
                                    DEZEMBRO = k.Sum(s => s.DEZEMBRO)
                                }).ToList();

                                gvDespesaAdmItemSubSub.DataSource = lstdespAdmSub;
                                gvDespesaAdmItemSubSub.DataBind();
                            }
                            img.Visible = true;
                        }
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
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Tipo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + subitem.Tipo.Trim().ToUpper().Substring(0, ((subitem.Tipo.Trim().Length > 20) ? 20 : subitem.Tipo.Trim().Length)) + "</font>";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(subitem.JANEIRO, 1) + FiltroValor(subitem.FEVEREIRO, 2) + FiltroValor(subitem.MARCO, 3) + FiltroValor(subitem.ABRIL, 4) + FiltroValor(subitem.MAIO, 5) + FiltroValor(subitem.JUNHO, 6) + FiltroValor(subitem.JULHO, 7) + FiltroValor(subitem.AGOSTO, 8) + FiltroValor(subitem.SETEMBRO, 9) + FiltroValor(subitem.OUTUBRO, 10) + FiltroValor(subitem.NOVEMBRO, 11) + FiltroValor(subitem.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult ebtida = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (ebtida != null && ebtida.Grupo != "")
                    {
                        Literal _litEbtida = e.Row.FindControl("litEbtida") as Literal;
                        if (_litEbtida != null)
                            if (ebtida.Linha.Contains("%"))
                                _litEbtida.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + ebtida.Grupo.Trim() + "</i></font></span>";
                            else
                                _litEbtida.Text = "<font size='2' face='Calibri'>&nbsp;" + ebtida.Grupo.Trim() + "</font>";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            if (ebtida.Linha.Contains("%"))
                                _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.JANEIRO, 1)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.JANEIRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _janeiro.Text = FormatarValor(FiltroValor(ebtida.JANEIRO, 1)).Trim();

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            if (ebtida.Linha.Contains("%"))
                                _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.FEVEREIRO, 2)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.FEVEREIRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _fevereiro.Text = FormatarValor(FiltroValor(ebtida.FEVEREIRO, 2)).Trim();

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            if (ebtida.Linha.Contains("%"))
                                _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.MARCO, 3)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.MARCO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _marco.Text = FormatarValor(FiltroValor(ebtida.MARCO, 3)).Trim();

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            if (ebtida.Linha.Contains("%"))
                                _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.ABRIL, 4)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.ABRIL < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _abril.Text = FormatarValor(FiltroValor(ebtida.ABRIL, 4)).Trim();

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            if (ebtida.Linha.Contains("%"))
                                _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.MAIO, 5)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.MAIO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _maio.Text = FormatarValor(FiltroValor(ebtida.MAIO, 5)).Trim();

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            if (ebtida.Linha.Contains("%"))
                                _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.JUNHO, 6)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.JUNHO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _junho.Text = FormatarValor(FiltroValor(ebtida.JUNHO, 6)).Trim();

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            if (ebtida.Linha.Contains("%"))
                                _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.JULHO, 7)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.JULHO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _julho.Text = FormatarValor(FiltroValor(ebtida.JULHO, 7)).Trim();

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            if (ebtida.Linha.Contains("%"))
                                _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.AGOSTO, 8)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.AGOSTO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _agosto.Text = FormatarValor(FiltroValor(ebtida.AGOSTO, 8)).Trim();

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            if (ebtida.Linha.Contains("%"))
                                _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.SETEMBRO, 9)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.SETEMBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _setembro.Text = FormatarValor(FiltroValor(ebtida.SETEMBRO, 9)).Trim();

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            if (ebtida.Linha.Contains("%"))
                                _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.OUTUBRO, 10)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.OUTUBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _outubro.Text = FormatarValor(FiltroValor(ebtida.OUTUBRO, 10)).Trim();

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            if (ebtida.Linha.Contains("%"))
                                _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.NOVEMBRO, 11)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.NOVEMBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _novembro.Text = FormatarValor(FiltroValor(ebtida.NOVEMBRO, 11)).Trim();

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            if (ebtida.Linha.Contains("%"))
                                _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.DEZEMBRO, 12)).Trim() + ((ebtida.Linha.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.DEZEMBRO < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _dezembro.Text = FormatarValor(FiltroValor(ebtida.DEZEMBRO, 12)).Trim();

                        totalLinha = Convert.ToDecimal(FiltroValor(ebtida.JANEIRO, 1) + FiltroValor(ebtida.FEVEREIRO, 2) + FiltroValor(ebtida.MARCO, 3) + FiltroValor(ebtida.ABRIL, 4) + FiltroValor(ebtida.MAIO, 5) + FiltroValor(ebtida.JUNHO, 6) + FiltroValor(ebtida.JULHO, 7) + FiltroValor(ebtida.AGOSTO, 8) + FiltroValor(ebtida.SETEMBRO, 9) + FiltroValor(ebtida.OUTUBRO, 10) + FiltroValor(ebtida.NOVEMBRO, 11) + FiltroValor(ebtida.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;

                        //guarda o total da margem de contribuicao para utilizar durante o loop da linha
                        if (_total != null)
                            if (ebtida.Linha.Contains("%")) //totalLinha g
                                _total.Text = "<span style='font-weight:normal'>" + ((gTotalReceitaLiquida == 0) ? "0" : FormatarValor((gTotalEbitda / gTotalReceitaLiquida) * 100).Trim()) + "</span><font size='2' face='Calibri' color='" + ((gTotalReceitaLiquida == 0) ? "" : ((((gTotalEbitda / gTotalReceitaLiquida) * 100) < 0) ? tagCorNegativo : "")) + "'>%</font>";
                            else
                            {
                                _total.Text = FormatarValor(totalLinha);
                                gTotalEbitda = totalLinha;
                            }

                        //SOMATORIO
                        dJaneiro += FiltroValor(ebtida.JANEIRO, 1);
                        dFevereiro += FiltroValor(ebtida.FEVEREIRO, 2);
                        dMarco += FiltroValor(ebtida.MARCO, 3);
                        dAbril += FiltroValor(ebtida.ABRIL, 4);
                        dMaio += FiltroValor(ebtida.MAIO, 5);
                        dJunho += FiltroValor(ebtida.JUNHO, 6);
                        dJulho += FiltroValor(ebtida.JULHO, 7);
                        dAgosto += FiltroValor(ebtida.AGOSTO, 8);
                        dSetembro += FiltroValor(ebtida.SETEMBRO, 9);
                        dOutubro += FiltroValor(ebtida.OUTUBRO, 10);
                        dNovembro += FiltroValor(ebtida.NOVEMBRO, 11);
                        dDezembro += FiltroValor(ebtida.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (ebtida.Grupo == "")
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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult depreciacao = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (depreciacao != null && depreciacao.Grupo != "")
                    {
                        Literal _litDepreciacao = e.Row.FindControl("litDepreciacao") as Literal;
                        if (_litDepreciacao != null)
                            _litDepreciacao.Text = "<font size='2' face='Calibri'>&nbsp;" + depreciacao.Grupo.Trim().Substring(0, depreciacao.Grupo.Trim().Length - 2) + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(depreciacao.JANEIRO, 1) + FiltroValor(depreciacao.FEVEREIRO, 2) + FiltroValor(depreciacao.MARCO, 3) + FiltroValor(depreciacao.ABRIL, 4) + FiltroValor(depreciacao.MAIO, 5) + FiltroValor(depreciacao.JUNHO, 6) + FiltroValor(depreciacao.JULHO, 7) + FiltroValor(depreciacao.AGOSTO, 8) + FiltroValor(depreciacao.SETEMBRO, 9) + FiltroValor(depreciacao.OUTUBRO, 10) + FiltroValor(depreciacao.NOVEMBRO, 11) + FiltroValor(depreciacao.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += FiltroValor(depreciacao.JANEIRO, 1);
                        dFevereiro += FiltroValor(depreciacao.FEVEREIRO, 2);
                        dMarco += FiltroValor(depreciacao.MARCO, 3);
                        dAbril += FiltroValor(depreciacao.ABRIL, 4);
                        dMaio += FiltroValor(depreciacao.MAIO, 5);
                        dJunho += FiltroValor(depreciacao.JUNHO, 6);
                        dJulho += FiltroValor(depreciacao.JULHO, 7);
                        dAgosto += FiltroValor(depreciacao.AGOSTO, 8);
                        dSetembro += FiltroValor(depreciacao.SETEMBRO, 9);
                        dOutubro += FiltroValor(depreciacao.OUTUBRO, 10);
                        dNovembro += FiltroValor(depreciacao.NOVEMBRO, 11);
                        dDezembro += FiltroValor(depreciacao.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (depreciacao.Grupo == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (depreciacao.Id == 1)
                        {
                            GridView gvDepreciacaoAmortizacaoItem = e.Row.FindControl("gvDepreciacaoAmortizacaoItem") as GridView;
                            if (gvDepreciacaoAmortizacaoItem != null)
                            {
                                var depreciacaoAmortizacao = gListaDepreciacao.Where(i => i.Grupo.Trim().ToUpper() == depreciacao.Grupo.ToUpper().Trim()).GroupBy(b => new { TIPO = b.Tipo }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Tipo = k.Key.TIPO.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
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

            //adicionar total depreciacao/servicos nao recorrentes
            gListaLucroBruto.Add(new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "LUCRO BRUTO",
                Linha = "",
                JANEIRO = dJaneiro,
                FEVEREIRO = dFevereiro,
                MARCO = dMarco,
                ABRIL = dAbril,
                MAIO = dMaio,
                JUNHO = dJunho,
                JULHO = dJulho,
                AGOSTO = dAgosto,
                SETEMBRO = dSetembro,
                OUTUBRO = dOutubro,
                NOVEMBRO = dNovembro,
                DEZEMBRO = dDezembro
            });
            /*
            //adicionar total ebtida
            gListaLucroBruto.AddRange(gListaEBITDA.Where(g => g.Grupo == "EBITDA"));

            // SOMAR LUCRO BRUTO
            gListaLucroBruto = gListaLucroBruto.GroupBy(p => new { ID = p.Id }).Select(k => new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "LUCRO BRUTO",
                Linha = "AGRUPADO",
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
                DEZEMBRO = k.Sum(g => g.DEZEMBRO)
            }).ToList();

            gListaLucroBruto.Insert(gListaLucroBruto.Count, new SP_DRE_OBTER_DADOSResult { Id = 99, Grupo = "" });

            //Carregar lucro bruto
            gvLucroBruto.DataSource = gListaLucroBruto;
            gvLucroBruto.DataBind();*/

        }

        protected void gvDepreciacaoAmortizacaoItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult depreciacaoLinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (depreciacaoLinha != null && depreciacaoLinha.Tipo != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + depreciacaoLinha.Tipo.Trim().ToUpper().Replace("DEPRECIAÇÃO", "").Substring(0, ((depreciacaoLinha.Tipo.Trim().Replace("DEPRECIAÇÃO", "").Length > 22) ? 22 : depreciacaoLinha.Tipo.Replace("DEPRECIAÇÃO", "").Trim().Length)) + "</font></span>";

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

                        totalLinha = Convert.ToDecimal(FiltroValor(depreciacaoLinha.JANEIRO, 1) + FiltroValor(depreciacaoLinha.FEVEREIRO, 2) + FiltroValor(depreciacaoLinha.MARCO, 3) + FiltroValor(depreciacaoLinha.ABRIL, 4) + FiltroValor(depreciacaoLinha.MAIO, 5) + FiltroValor(depreciacaoLinha.JUNHO, 6) + FiltroValor(depreciacaoLinha.JULHO, 7) + FiltroValor(depreciacaoLinha.AGOSTO, 8) + FiltroValor(depreciacaoLinha.SETEMBRO, 9) + FiltroValor(depreciacaoLinha.OUTUBRO, 10) + FiltroValor(depreciacaoLinha.NOVEMBRO, 11) + FiltroValor(depreciacaoLinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult eventual = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (eventual != null && eventual.Grupo != "")
                    {
                        Literal _litEventual = e.Row.FindControl("litEventual") as Literal;
                        if (_litEventual != null)
                            _litEventual.Text = "<font size='2' face='Calibri'>&nbsp;" + eventual.Grupo.Trim() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(eventual.JANEIRO, 1) + FiltroValor(eventual.FEVEREIRO, 2) + FiltroValor(eventual.MARCO, 3) + FiltroValor(eventual.ABRIL, 4) + FiltroValor(eventual.MAIO, 5) + FiltroValor(eventual.JUNHO, 6) + FiltroValor(eventual.JULHO, 7) + FiltroValor(eventual.AGOSTO, 8) + FiltroValor(eventual.SETEMBRO, 9) + FiltroValor(eventual.OUTUBRO, 10) + FiltroValor(eventual.NOVEMBRO, 11) + FiltroValor(eventual.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += FiltroValor(eventual.JANEIRO, 1);
                        dFevereiro += FiltroValor(eventual.FEVEREIRO, 2);
                        dMarco += FiltroValor(eventual.MARCO, 3);
                        dAbril += FiltroValor(eventual.ABRIL, 4);
                        dMaio += FiltroValor(eventual.MAIO, 5);
                        dJunho += FiltroValor(eventual.JUNHO, 6);
                        dJulho += FiltroValor(eventual.JULHO, 7);
                        dAgosto += FiltroValor(eventual.AGOSTO, 8);
                        dSetembro += FiltroValor(eventual.SETEMBRO, 9);
                        dOutubro += FiltroValor(eventual.OUTUBRO, 10);
                        dNovembro += FiltroValor(eventual.NOVEMBRO, 11);
                        dDezembro += FiltroValor(eventual.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (eventual.Grupo == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (eventual.Id == 22)
                        {
                            GridView gvDespesaEventualItem = e.Row.FindControl("gvDespesaEventualItem") as GridView;
                            if (gvDespesaEventualItem != null)
                            {
                                var eventualDespesa = gListaDespesaEventual.Where(i => i.Grupo.Trim().ToUpper() == eventual.Grupo.ToUpper().Trim()).GroupBy(b => new { TIPO = b.Tipo }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Tipo = k.Key.TIPO.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
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

            //adicionar total despesa eventual
            gListaLucroBruto.Add(new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "LUCRO BRUTO",
                Linha = "",
                JANEIRO = dJaneiro,
                FEVEREIRO = dFevereiro,
                MARCO = dMarco,
                ABRIL = dAbril,
                MAIO = dMaio,
                JUNHO = dJunho,
                JULHO = dJulho,
                AGOSTO = dAgosto,
                SETEMBRO = dSetembro,
                OUTUBRO = dOutubro,
                NOVEMBRO = dNovembro,
                DEZEMBRO = dDezembro
            });

            //adicionar total ebtida
            gListaLucroBruto.AddRange(gListaEBITDA.Where(g => g.Grupo == "EBITDA"));

            // SOMAR LUCRO BRUTO
            gListaLucroBruto = gListaLucroBruto.GroupBy(p => new { ID = p.Id }).Select(k => new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "LUCRO BRUTO",
                Linha = "AGRUPADO",
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
                DEZEMBRO = k.Sum(g => g.DEZEMBRO)
            }).ToList();

            gListaLucroBruto.Insert(gListaLucroBruto.Count, new SP_DRE_OBTER_DADOSResult { Id = 99, Grupo = "" });

            //Carregar lucro bruto
            gvLucroBruto.DataSource = gListaLucroBruto;
            gvLucroBruto.DataBind();

        }

        protected void gvDespesaEventualItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string conta_contabil = "";
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult eventualLinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (eventualLinha != null && eventualLinha.Tipo != "")
                    {

                        var conta = new DesenvolvimentoController().ObterContaContabilDescricao(eventualLinha.Tipo.Trim()).Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 1) == "3").ToList();
                        if (conta != null && conta.Count() == 1)
                            conta_contabil = conta[0].CONTA_CONTABIL.Trim().Replace(".", "@");

                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + eventualLinha.Tipo.Trim().ToUpper() + "</font></span>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=1&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _janeiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.JANEIRO, 1)) + "</a>";
                        }

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=2&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _fevereiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.FEVEREIRO, 2)) + "</a>";
                        }

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=3&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _marco.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.MARCO, 3)) + "</a>";
                        }

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=4&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _abril.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.ABRIL, 4)) + "</a>";
                        }

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=5&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _maio.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.MAIO, 5)) + "</a>";
                        }

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=6&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _junho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.JUNHO, 6)) + "</a>";
                        }

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=7&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _julho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.JULHO, 7)) + "</a>";
                        }

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=8&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _agosto.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.AGOSTO, 8)) + "</a>";
                        }

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=9&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _setembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.SETEMBRO, 9)) + "</a>";
                        }

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=10&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _outubro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.OUTUBRO, 10)) + "</a>";
                        }

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=11&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _novembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.NOVEMBRO, 11)) + "</a>";
                        }

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=12&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _dezembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(eventualLinha.DEZEMBRO, 12)) + "</a>";
                        }

                        totalLinha = Convert.ToDecimal(FiltroValor(eventualLinha.JANEIRO, 1) + FiltroValor(eventualLinha.FEVEREIRO, 2) + FiltroValor(eventualLinha.MARCO, 3) + FiltroValor(eventualLinha.ABRIL, 4) + FiltroValor(eventualLinha.MAIO, 5) + FiltroValor(eventualLinha.JUNHO, 6) + FiltroValor(eventualLinha.JULHO, 7) + FiltroValor(eventualLinha.AGOSTO, 8) + FiltroValor(eventualLinha.SETEMBRO, 9) + FiltroValor(eventualLinha.OUTUBRO, 10) + FiltroValor(eventualLinha.NOVEMBRO, 11) + FiltroValor(eventualLinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult lucrobruto = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (lucrobruto != null && lucrobruto.Grupo != "")
                    {
                        Literal _litLucroBruto = e.Row.FindControl("litLucroBruto") as Literal;
                        if (_litLucroBruto != null)
                            _litLucroBruto.Text = "<font size='2' face='Calibri'>&nbsp;" + lucrobruto.Grupo.Trim() + "</font>";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(lucrobruto.JANEIRO, 1) + FiltroValor(lucrobruto.FEVEREIRO, 2) + FiltroValor(lucrobruto.MARCO, 3) + FiltroValor(lucrobruto.ABRIL, 4) + FiltroValor(lucrobruto.MAIO, 5) + FiltroValor(lucrobruto.JUNHO, 6) + FiltroValor(lucrobruto.JULHO, 7) + FiltroValor(lucrobruto.AGOSTO, 8) + FiltroValor(lucrobruto.SETEMBRO, 9) + FiltroValor(lucrobruto.OUTUBRO, 10) + FiltroValor(lucrobruto.NOVEMBRO, 11) + FiltroValor(lucrobruto.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += FiltroValor(lucrobruto.JANEIRO, 1);
                        dFevereiro += FiltroValor(lucrobruto.FEVEREIRO, 2);
                        dMarco += FiltroValor(lucrobruto.MARCO, 3);
                        dAbril += FiltroValor(lucrobruto.ABRIL, 4);
                        dMaio += FiltroValor(lucrobruto.MAIO, 5);
                        dJunho += FiltroValor(lucrobruto.JUNHO, 6);
                        dJulho += FiltroValor(lucrobruto.JULHO, 7);
                        dAgosto += FiltroValor(lucrobruto.AGOSTO, 8);
                        dSetembro += FiltroValor(lucrobruto.SETEMBRO, 9);
                        dOutubro += FiltroValor(lucrobruto.OUTUBRO, 10);
                        dNovembro += FiltroValor(lucrobruto.NOVEMBRO, 11);
                        dDezembro += FiltroValor(lucrobruto.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (lucrobruto.Grupo == "")
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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult receitadesp = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (receitadesp != null && receitadesp.Grupo != "")
                    {
                        Literal _litReceitaDespesa = e.Row.FindControl("litReceitaDespesa") as Literal;
                        if (_litReceitaDespesa != null)
                            _litReceitaDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + receitadesp.Grupo.Trim() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(receitadesp.JANEIRO, 1) + FiltroValor(receitadesp.FEVEREIRO, 2) + FiltroValor(receitadesp.MARCO, 3) + FiltroValor(receitadesp.ABRIL, 4) + FiltroValor(receitadesp.MAIO, 5) + FiltroValor(receitadesp.JUNHO, 6) + FiltroValor(receitadesp.JULHO, 7) + FiltroValor(receitadesp.AGOSTO, 8) + FiltroValor(receitadesp.SETEMBRO, 9) + FiltroValor(receitadesp.OUTUBRO, 10) + FiltroValor(receitadesp.NOVEMBRO, 11) + FiltroValor(receitadesp.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += FiltroValor(receitadesp.JANEIRO, 1);
                        dFevereiro += FiltroValor(receitadesp.FEVEREIRO, 2);
                        dMarco += FiltroValor(receitadesp.MARCO, 3);
                        dAbril += FiltroValor(receitadesp.ABRIL, 4);
                        dMaio += FiltroValor(receitadesp.MAIO, 5);
                        dJunho += FiltroValor(receitadesp.JUNHO, 6);
                        dJulho += FiltroValor(receitadesp.JULHO, 7);
                        dAgosto += FiltroValor(receitadesp.AGOSTO, 8);
                        dSetembro += FiltroValor(receitadesp.SETEMBRO, 9);
                        dOutubro += FiltroValor(receitadesp.OUTUBRO, 10);
                        dNovembro += FiltroValor(receitadesp.NOVEMBRO, 11);
                        dDezembro += FiltroValor(receitadesp.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (receitadesp.Grupo == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (receitadesp.Id == 1)
                        {
                            GridView gvReceitaDespesaFinItem = e.Row.FindControl("gvReceitaDespesaFinItem") as GridView;
                            if (gvReceitaDespesaFinItem != null)
                            {
                                var receitaDespFin = gListaReceitaDespesaFinanceira.Where(i => i.Grupo.Trim().ToUpper() == receitadesp.Grupo.ToUpper().Trim()).GroupBy(b => new { TIPO = b.Tipo }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Tipo = k.Key.TIPO.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
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

            //adicionar total depreciacao/servicos nao recorrentes
            gListaLAIR.Add(new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "LAIR",
                Linha = "",
                JANEIRO = dJaneiro,
                FEVEREIRO = dFevereiro,
                MARCO = dMarco,
                ABRIL = dAbril,
                MAIO = dMaio,
                JUNHO = dJunho,
                JULHO = dJulho,
                AGOSTO = dAgosto,
                SETEMBRO = dSetembro,
                OUTUBRO = dOutubro,
                NOVEMBRO = dNovembro,
                DEZEMBRO = dDezembro
            });

            //adicionar total lucro bruto
            gListaLAIR.AddRange(gListaLucroBruto.Where(i => i.Id == 99));

            // SOMAR LUCRO BRUTO
            gListaLAIR = gListaLAIR.GroupBy(p => new { ID = p.Id }).Select(k => new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "LAIR",
                Linha = "AGRUPADO",
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
                DEZEMBRO = k.Sum(g => g.DEZEMBRO)
            }).ToList();

            gListaLAIR.Insert(gListaLAIR.Count, new SP_DRE_OBTER_DADOSResult { Id = 99, Grupo = "" });

            //Carregar lucro bruto
            gvLAIR.DataSource = gListaLAIR;
            gvLAIR.DataBind();

        }

        protected void gvReceitaDespesaFinItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string conta_contabil = "";
            string url_link = "";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult receitadespesaLinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (receitadespesaLinha != null && receitadespesaLinha.Tipo != "")
                    {

                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                        {
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + receitadespesaLinha.Tipo.Trim().ToUpper().Substring(0, ((receitadespesaLinha.Tipo.Trim().Length > 22) ? 22 : receitadespesaLinha.Tipo.Trim().Length)) + "</font></span>";
                            var conta = new DesenvolvimentoController().ObterContaContabilDescricao(receitadespesaLinha.Tipo.Trim()).Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 1) == "3").ToList();
                            if (conta != null && conta.Count() == 1)
                                conta_contabil = conta[0].CONTA_CONTABIL.Trim().Replace(".", "@");
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=1&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _janeiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Janeiro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.JANEIRO, 1)) + "</a>";
                        }

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=2&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _fevereiro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Fevereiro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.FEVEREIRO, 2)) + "</a>";
                        }

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=3&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _marco.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Março'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.MARCO, 3)) + "</a>";
                        }

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=4&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _abril.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Abril'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.ABRIL, 4)) + "</a>";
                        }

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=5&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _maio.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Maio'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.MAIO, 5)) + "</a>";
                        }

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=6&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _junho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Junho'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.JUNHO, 6)) + "</a>";
                        }

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=7&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _julho.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Julho'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.JULHO, 7)) + "</a>";
                        }

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=8&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _agosto.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Agosto'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.AGOSTO, 8)) + "</a>";
                        }

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=9&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _setembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Setembro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.SETEMBRO, 9)) + "</a>";
                        }

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=10&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _outubro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Outubro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.OUTUBRO, 10)) + "</a>";
                        }

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=11&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _novembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Novembro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.NOVEMBRO, 11)) + "</a>";
                        }
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                        {
                            url_link = "dre_dre_conta_contabil.aspx?cc=" + conta_contabil + "&ff=" + ddlFilial.SelectedValue.Trim() + "&m=12&a=" + ddlAno.SelectedItem.Text.Trim() + "&t=1";
                            _dezembro.Text = "<a href='" + url_link + "' target='_blank' class='adre' title='Dezembro'>&nbsp;" + FormatarValor(FiltroValor(receitadespesaLinha.DEZEMBRO, 12)) + "</a>";
                        }

                        totalLinha = Convert.ToDecimal(FiltroValor(receitadespesaLinha.JANEIRO, 1) + FiltroValor(receitadespesaLinha.FEVEREIRO, 2) + FiltroValor(receitadespesaLinha.MARCO, 3) + FiltroValor(receitadespesaLinha.ABRIL, 4) + FiltroValor(receitadespesaLinha.MAIO, 5) + FiltroValor(receitadespesaLinha.JUNHO, 6) + FiltroValor(receitadespesaLinha.JULHO, 7) + FiltroValor(receitadespesaLinha.AGOSTO, 8) + FiltroValor(receitadespesaLinha.SETEMBRO, 9) + FiltroValor(receitadespesaLinha.OUTUBRO, 10) + FiltroValor(receitadespesaLinha.NOVEMBRO, 11) + FiltroValor(receitadespesaLinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult lair = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (lair != null && lair.Grupo != "")
                    {
                        Literal _litLAIR = e.Row.FindControl("litLAIR") as Literal;
                        if (_litLAIR != null)
                            _litLAIR.Text = "<font size='2' face='Calibri'>&nbsp;" + lair.Grupo.Trim() + "</font>";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(lair.JANEIRO, 1) + FiltroValor(lair.FEVEREIRO, 2) + FiltroValor(lair.MARCO, 3) + FiltroValor(lair.ABRIL, 4) + FiltroValor(lair.MAIO, 5) + FiltroValor(lair.JUNHO, 6) + FiltroValor(lair.JULHO, 7) + FiltroValor(lair.AGOSTO, 8) + FiltroValor(lair.SETEMBRO, 9) + FiltroValor(lair.OUTUBRO, 10) + FiltroValor(lair.NOVEMBRO, 11) + FiltroValor(lair.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += FiltroValor(lair.JANEIRO, 1);
                        dFevereiro += FiltroValor(lair.FEVEREIRO, 2);
                        dMarco += FiltroValor(lair.MARCO, 3);
                        dAbril += FiltroValor(lair.ABRIL, 4);
                        dMaio += FiltroValor(lair.MAIO, 5);
                        dJunho += FiltroValor(lair.JUNHO, 6);
                        dJulho += FiltroValor(lair.JULHO, 7);
                        dAgosto += FiltroValor(lair.AGOSTO, 8);
                        dSetembro += FiltroValor(lair.SETEMBRO, 9);
                        dOutubro += FiltroValor(lair.OUTUBRO, 10);
                        dNovembro += FiltroValor(lair.NOVEMBRO, 11);
                        dDezembro += FiltroValor(lair.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (lair.Grupo == "")
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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult impostotaxa = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (impostotaxa != null && impostotaxa.Grupo != "")
                    {
                        Literal _litImpostoTaxa = e.Row.FindControl("litImpostoTaxa") as Literal;
                        if (_litImpostoTaxa != null)
                            _litImpostoTaxa.Text = "<font size='2' face='Calibri'>&nbsp;" + impostotaxa.Grupo.Trim() + "</font> ";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(impostotaxa.JANEIRO, 1) + FiltroValor(impostotaxa.FEVEREIRO, 2) + FiltroValor(impostotaxa.MARCO, 3) + FiltroValor(impostotaxa.ABRIL, 4) + FiltroValor(impostotaxa.MAIO, 5) + FiltroValor(impostotaxa.JUNHO, 6) + FiltroValor(impostotaxa.JULHO, 7) + FiltroValor(impostotaxa.AGOSTO, 8) + FiltroValor(impostotaxa.SETEMBRO, 9) + FiltroValor(impostotaxa.OUTUBRO, 10) + FiltroValor(impostotaxa.NOVEMBRO, 11) + FiltroValor(impostotaxa.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += FiltroValor(impostotaxa.JANEIRO, 1);
                        dFevereiro += FiltroValor(impostotaxa.FEVEREIRO, 2);
                        dMarco += FiltroValor(impostotaxa.MARCO, 3);
                        dAbril += FiltroValor(impostotaxa.ABRIL, 4);
                        dMaio += FiltroValor(impostotaxa.MAIO, 5);
                        dJunho += FiltroValor(impostotaxa.JUNHO, 6);
                        dJulho += FiltroValor(impostotaxa.JULHO, 7);
                        dAgosto += FiltroValor(impostotaxa.AGOSTO, 8);
                        dSetembro += FiltroValor(impostotaxa.SETEMBRO, 9);
                        dOutubro += FiltroValor(impostotaxa.OUTUBRO, 10);
                        dNovembro += FiltroValor(impostotaxa.NOVEMBRO, 11);
                        dDezembro += FiltroValor(impostotaxa.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (impostotaxa.Grupo == "")
                        e.Row.Height = Unit.Pixel(4);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (impostotaxa.Id == 1)
                        {
                            GridView gvImpostoTaxaItem = e.Row.FindControl("gvImpostoTaxaItem") as GridView;
                            if (gvImpostoTaxaItem != null)
                            {
                                var lstimpostotaxa = gListaImpostoTaxa.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
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

            //adicionar total imposto, taxas e contribuções
            gListaResultado.Add(new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "RESULTADO",
                Linha = "",
                JANEIRO = dJaneiro,
                FEVEREIRO = dFevereiro,
                MARCO = dMarco,
                ABRIL = dAbril,
                MAIO = dMaio,
                JUNHO = dJunho,
                JULHO = dJulho,
                AGOSTO = dAgosto,
                SETEMBRO = dSetembro,
                OUTUBRO = dOutubro,
                NOVEMBRO = dNovembro,
                DEZEMBRO = dDezembro
            });

            //adicionar LAIR
            gListaResultado.AddRange(gListaLAIR.Where(i => i.Id == 99));

            // SOMAR RESULTADO
            gListaResultado = gListaResultado.GroupBy(p => new { ID = p.Id }).Select(k => new SP_DRE_OBTER_DADOSResult
            {
                Id = 99,
                Grupo = "RESULTADO",
                Linha = "AGRUPADO",
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
                DEZEMBRO = k.Sum(g => g.DEZEMBRO)
            }).ToList();

            gListaResultado.Insert(gListaResultado.Count, new SP_DRE_OBTER_DADOSResult { Id = 99, Grupo = "" });

            //Carregar Resultado
            gvResultado.DataSource = gListaResultado;
            gvResultado.DataBind();
        }

        protected void gvImpostoTaxaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult impostotaxaLinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (impostotaxaLinha != null && impostotaxaLinha.Linha != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + impostotaxaLinha.Linha.Trim().ToUpper() + "</font></span>";

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

                        totalLinha = Convert.ToDecimal(FiltroValor(impostotaxaLinha.JANEIRO, 1) + FiltroValor(impostotaxaLinha.FEVEREIRO, 2) + FiltroValor(impostotaxaLinha.MARCO, 3) + FiltroValor(impostotaxaLinha.ABRIL, 4) + FiltroValor(impostotaxaLinha.MAIO, 5) + FiltroValor(impostotaxaLinha.JUNHO, 6) + FiltroValor(impostotaxaLinha.JULHO, 7) + FiltroValor(impostotaxaLinha.AGOSTO, 8) + FiltroValor(impostotaxaLinha.SETEMBRO, 9) + FiltroValor(impostotaxaLinha.OUTUBRO, 10) + FiltroValor(impostotaxaLinha.NOVEMBRO, 11) + FiltroValor(impostotaxaLinha.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

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
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult resultado = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (resultado != null && resultado.Grupo != "")
                    {
                        Literal _litResultado = e.Row.FindControl("litResultado") as Literal;
                        if (_litResultado != null)
                            _litResultado.Text = "<font size='2' face='Calibri'>&nbsp;" + resultado.Grupo.Trim() + "</font>";
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

                        totalLinha = Convert.ToDecimal(FiltroValor(resultado.JANEIRO, 1) + FiltroValor(resultado.FEVEREIRO, 2) + FiltroValor(resultado.MARCO, 3) + FiltroValor(resultado.ABRIL, 4) + FiltroValor(resultado.MAIO, 5) + FiltroValor(resultado.JUNHO, 6) + FiltroValor(resultado.JULHO, 7) + FiltroValor(resultado.AGOSTO, 8) + FiltroValor(resultado.SETEMBRO, 9) + FiltroValor(resultado.OUTUBRO, 10) + FiltroValor(resultado.NOVEMBRO, 11) + FiltroValor(resultado.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += FiltroValor(resultado.JANEIRO, 1);
                        dFevereiro += FiltroValor(resultado.FEVEREIRO, 2);
                        dMarco += FiltroValor(resultado.MARCO, 3);
                        dAbril += FiltroValor(resultado.ABRIL, 4);
                        dMaio += FiltroValor(resultado.MAIO, 5);
                        dJunho += FiltroValor(resultado.JUNHO, 6);
                        dJulho += FiltroValor(resultado.JULHO, 7);
                        dAgosto += FiltroValor(resultado.AGOSTO, 8);
                        dSetembro += FiltroValor(resultado.SETEMBRO, 9);
                        dOutubro += FiltroValor(resultado.OUTUBRO, 10);
                        dNovembro += FiltroValor(resultado.NOVEMBRO, 11);
                        dDezembro += FiltroValor(resultado.DEZEMBRO, 12);
                        dTotal += totalLinha;

                    }

                    if (resultado.Grupo == "")
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

    }
}
