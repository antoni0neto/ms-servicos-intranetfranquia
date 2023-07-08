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
    public partial class dre_dre_compara : System.Web.UI.Page
    {
        DREController dreController = new DREController();
        BaseController baseController = new BaseController();

        Color corTitulo = System.Drawing.SystemColors.GradientActiveCaption;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";

        decimal dAnoAnt2 = 0, dFatAnoAnt2 = 0;
        decimal dAnoAnt1 = 0, dFatAnoAnt1 = 0;
        decimal dAnoOrc = 0, dFatAnoOrc = 0;
        decimal dAnoReal = 0, dFatAnoReal = 0;
        decimal dAtingidoValor = 0;

        decimal porcAnoAnt2 = 0;
        decimal porcAnoAnt1 = 0;
        decimal porcAnoOrc = 0;
        decimal porcAnoReal = 0;

        string ret = "";


        List<SP_DRE_COMPARATIVOResult> gDREGeral = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaAtacado = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaVarejo = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaReceitaBruta = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaReceitaLiquida = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaTaxaCartao = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaImposto = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaCmvAtacado = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaCmvVarejo = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaMargemContribuicao = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaMargemContribuicaoVarejo = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaMargemContribuicaoAtacado = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaMargemContribuicaoVarejoAux = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaMargemContribuicaoAtacadoAux = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaCustoFixo = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaMargemBruta = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaDespesaCTO = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaDespesaEspecifica = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaDespesaVenda = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaDespesaAdmPessoal = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaDespesaAdmOutros = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaEBITDA = new List<SP_DRE_COMPARATIVOResult>();


        List<SP_DRE_COMPARATIVOResult> gListaDepreciacao = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaDespesaEventual = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaLucroBruto = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaReceitaDespesaFinanceira = new List<SP_DRE_COMPARATIVOResult>();
        List<SP_DRE_COMPARATIVOResult> gListaLAIR = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaImpostoTaxa = new List<SP_DRE_COMPARATIVOResult>();

        List<SP_DRE_COMPARATIVOResult> gListaResultado = new List<SP_DRE_COMPARATIVOResult>();

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

            int mesIni = 1;
            int mesFim = 12;

            ano = ddlAno.SelectedValue;
            v_dataini = Convert.ToDateTime(ano + "-01-01");
            v_datafim = Convert.ToDateTime(ano + "-12-31");

            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                filial = ddlFilial.SelectedValue;

            if (ddlMesDe.SelectedValue != "")
                mesIni = Convert.ToInt32(ddlMesDe.SelectedValue);

            if (ddlMesAte.SelectedValue != "")
                mesFim = Convert.ToInt32(ddlMesAte.SelectedValue);

            try
            {

                labErro.Text = "";

                gDREGeral = dreController.ObterDREComparativo(v_dataini, v_datafim, filial, mesIni, mesFim);

                CarregarReceitaLiquida(gDREGeral, filial);
                ZerarValores();
                CarregarCMV(gDREGeral, filial);
                ZerarValores();
                CarregarCustoFixo(gDREGeral);
                ZerarValores();
                CarregarDespesaCTO(gDREGeral);
                ZerarValores();
                CarregarDespesaEspecifica(gDREGeral);
                ZerarValores();
                CarregarDespesaVenda(gDREGeral);
                ZerarValores();
                CarregarDespesaADM(gDREGeral);
                ZerarValores();
                CarregarDepreciacao(gDREGeral);
                ZerarValores();
                CarregarDespesaEventual(gDREGeral);
                ZerarValores();
                CarregarReceitaDespFinanceira(gDREGeral);
                ZerarValores();
                CarregarImpostosTaxas(gDREGeral);
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

            filial = baseController.BuscaFiliais();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "000000", FILIAL = "ATACADO" });
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
        private string FormatarValor(decimal? valor, int coluna)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            decimal valorPercentual = 0;
            if (coluna == 1 && dFatAnoAnt2 > 0)
                valorPercentual = Convert.ToDecimal(valor / dFatAnoAnt2) * 100;
            else if (coluna == 2 && dFatAnoAnt1 > 0)
                valorPercentual = Convert.ToDecimal(valor / dFatAnoAnt2) * 100;
            else if (coluna == 3 && dFatAnoOrc > 0)
                valorPercentual = Convert.ToDecimal(valor / dFatAnoOrc) * 100;
            else if (coluna == 4 && dFatAnoReal > 0)
                valorPercentual = (dFatAnoReal > 0) ? (Convert.ToDecimal(valor / dFatAnoReal) * 100) : 0; // TOTAL

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") +
                ("<br />(" + valorPercentual.ToString("##0.00;##0.00") + " %)" + "</font> ");
            return retorno;
        }
        private string FormatarValor(decimal? valor, bool porc)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + ((porc) ? "%" : "") + "</font> ";
            return retorno;
        }
        private void ZerarValores()
        {
            dAnoAnt2 = 0;
            dAnoAnt1 = 0;
            dAnoOrc = 0;
            dAnoReal = 0;
            dAtingidoValor = 0;
        }
        private decimal FiltroValor(decimal? valor)
        {
            //int mesFiltroDe = 0;
            //int mesFiltroAte = 0;

            /*if (ddlMesDe.SelectedValue != "")
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
            }*/

            /*
            if (periodoFiltro == "1S")
                if (mes > 6)
                    valor = 0;
            if (periodoFiltro == "2S")
                if (mes < 7)
                    valor = 0;
            */


            return Convert.ToDecimal(valor);
        }
        private string CalculaPorc(decimal anoReal, decimal anoCalculo, int tipo, bool inverteValor)
        {
            decimal valor = 0;

            if (anoReal != 0 && anoCalculo != 0)
            {
                if (tipo == 1)
                    valor = ((((anoReal - anoCalculo) / anoCalculo) * 100) * ((inverteValor) ? -1 : 1));
                else
                    valor = ((((anoReal - anoCalculo))) * ((inverteValor) ? -1 : 1));
            }

            return FormatarValor(FiltroValor(valor), true);

            //if (tipo == 1)
            //    return FormatarValor(FiltroValor(((anoReal == 0 || anoCalculo == 0) ? 0 : ());
            //else
            //    return FormatarValor(FiltroValor(((anoReal == 0 || anoCalculo == 0) ? 0 : (((anoReal - anoCalculo))) * ((inverteValor) ? -1 : 1))));
        }


        private void GuardarFaturamentoBruto(List<SP_DRE_COMPARATIVOResult> valorCalculo)
        {
            dFatAnoAnt2 = Convert.ToDecimal(valorCalculo.Sum(p => p.AnoAnt2));
            dFatAnoAnt1 = Convert.ToDecimal(valorCalculo.Sum(p => p.AnoAnt1));
            dFatAnoOrc = Convert.ToDecimal(valorCalculo.Sum(p => p.AnoOrc));
            dFatAnoReal = Convert.ToDecimal(valorCalculo.Sum(p => p.AnoReal));
        }
        #endregion

        #region "ACOES"

        private void CarregarReceitaLiquida(List<SP_DRE_COMPARATIVOResult> _dreGeral, string filial)
        {

            //Obter Atacado
            gListaAtacado.AddRange(_dreGeral.Where(p => p.ID == 1));

            if (filial != "000000") //ATACADO
            {
                gListaVarejo.AddRange(_dreGeral.Where(p => p.ID == 2));
                gListaTaxaCartao.AddRange(_dreGeral.Where(p => p.ID == 3));
            }

            gListaImposto.AddRange(_dreGeral.Where(p => p.ID == 4));

            gListaReceitaBruta = gListaVarejo.Union(gListaAtacado).ToList();
            gListaReceitaBruta = gListaReceitaBruta.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "RECEITAS BRUTAS",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            GuardarFaturamentoBruto(gListaReceitaBruta);

            var deducoesReceitaBruta = gListaTaxaCartao.Union(gListaImposto);
            deducoesReceitaBruta = deducoesReceitaBruta.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                k => new SP_DRE_COMPARATIVOResult
                                                                {
                                                                    ID = 2,
                                                                    GRUPO = "DEDUÇÕES RECEITAS BRUTAS",
                                                                    AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                    AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                    AnoOrc = k.Sum(j => j.AnoOrc),
                                                                    AnoReal = k.Sum(j => j.AnoReal)
                                                                }).ToList();

            var receitaLiquida = gListaReceitaBruta.Union(deducoesReceitaBruta).ToList();
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "1" });
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "RECEITAS LÍQUIDAS" });
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "2" });

            if (receitaLiquida != null)
            {
                gvReceitaLiquida.DataSource = receitaLiquida;
                gvReceitaLiquida.DataBind();
            }
        }
        private void CarregarCMV(List<SP_DRE_COMPARATIVOResult> _dreGeral, string filial)
        {
            gListaCmvAtacado.AddRange(_dreGeral.Where(p => p.ID == 5));
            if (filial != "000000")
            {
                gListaCmvVarejo.AddRange(_dreGeral.Where(p => p.ID == 6));
            }

            var cmv = gListaCmvAtacado.Union(gListaCmvVarejo).ToList();
            cmv = cmv.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "CMV",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            cmv.Insert(cmv.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "" });
            cmv.Insert(cmv.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "CMV" });
            cmv.Insert(cmv.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "" });
            if (cmv != null)
            {
                gvCMV.DataSource = cmv;
                gvCMV.DataBind();
            }
        }
        private void CarregarCustoFixo(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaCustoFixo.AddRange(_dreGeral.Where(p => p.ID == 9));

            var custoFixoAgrupado = gListaCustoFixo;
            custoFixoAgrupado = custoFixoAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "CUSTOS FIXOS",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            custoFixoAgrupado.Insert(custoFixoAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (custoFixoAgrupado != null)
            {
                gvCustoFixo.DataSource = custoFixoAgrupado;
                gvCustoFixo.DataBind();
            }
        }
        private void CarregarDespesaCTO(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaDespesaCTO.AddRange(_dreGeral.Where(p => p.ID == 17));

            var despCTOAgrupado = gListaDespesaCTO;
            despCTOAgrupado = despCTOAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS CTO",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            despCTOAgrupado.Insert(despCTOAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            despCTOAgrupado.Insert(despCTOAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (despCTOAgrupado != null)
            {
                gvDespesaCTO.DataSource = despCTOAgrupado;
                gvDespesaCTO.DataBind();
            }
        }
        private void CarregarDespesaEspecifica(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaDespesaEspecifica.AddRange(_dreGeral.Where(p => p.ID == 23));

            var despEspAgrupado = gListaDespesaEspecifica;
            despEspAgrupado = despEspAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS ESPECÍFICAS",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            despEspAgrupado.Insert(despEspAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            despEspAgrupado.Insert(despEspAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (despEspAgrupado != null)
            {
                gvDespesaEspecifica.DataSource = despEspAgrupado;
                gvDespesaEspecifica.DataBind();
            }
        }
        private void CarregarDespesaVenda(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaDespesaVenda.AddRange(_dreGeral.Where(p => p.ID == 7));
            gListaDespesaVenda.AddRange(_dreGeral.Where(p => p.ID == 8));

            var despVendaAgrupado = gListaDespesaVenda;
            despVendaAgrupado = despVendaAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS VENDAS",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            despVendaAgrupado.Insert(despVendaAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            despVendaAgrupado.Insert(despVendaAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (despVendaAgrupado != null)
            {
                gvDespesaVenda.DataSource = despVendaAgrupado;
                gvDespesaVenda.DataBind();
            }
        }
        private void CarregarDespesaADM(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaDespesaAdmPessoal.AddRange(_dreGeral.Where(p => p.ID == 18));
            gListaDespesaAdmPessoal.AddRange(_dreGeral.Where(p => p.ID == 14));
            gListaDespesaAdmPessoal.AddRange(_dreGeral.Where(p => p.ID == 15));

            gListaDespesaAdmOutros.AddRange(_dreGeral.Where(p => p.ID == 10));
            gListaDespesaAdmOutros.AddRange(_dreGeral.Where(p => p.ID == 11));
            gListaDespesaAdmOutros.AddRange(_dreGeral.Where(p => p.ID == 12));

            var despADMAgrupado = gListaDespesaAdmPessoal.Union(gListaDespesaAdmOutros).ToList();
            despADMAgrupado = despADMAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS ADMINISTRATIVAS",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (despADMAgrupado != null)
            {
                gvDespesaAdm.DataSource = despADMAgrupado;
                gvDespesaAdm.DataBind();
            }
        }
        private void CarregarDepreciacao(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaDepreciacao.AddRange(_dreGeral.Where(p => p.ID == 13));
            gListaDepreciacao.AddRange(_dreGeral.Where(p => p.ID == 19));

            var depreciacaoAgrupado = gListaDepreciacao;
            depreciacaoAgrupado = depreciacaoAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            depreciacaoAgrupado.Insert(depreciacaoAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            depreciacaoAgrupado.Insert(depreciacaoAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (depreciacaoAgrupado != null)
            {
                gvDepreciacaoAmortizacao.DataSource = depreciacaoAgrupado;
                gvDepreciacaoAmortizacao.DataBind();
            }
        }
        private void CarregarDespesaEventual(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaDespesaEventual.AddRange(_dreGeral.Where(p => p.ID == 22));

            var despesaEventualAgrupado = gListaDespesaEventual;
            despesaEventualAgrupado = despesaEventualAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 22,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            despesaEventualAgrupado.Insert(despesaEventualAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            despesaEventualAgrupado.Insert(despesaEventualAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (despesaEventualAgrupado != null)
            {
                gvDespesaEventual.DataSource = despesaEventualAgrupado;
                gvDespesaEventual.DataBind();
            }
        }
        private void CarregarReceitaDespFinanceira(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaReceitaDespesaFinanceira.AddRange(_dreGeral.Where(p => p.ID == 20));
            gListaReceitaDespesaFinanceira.AddRange(_dreGeral.Where(p => p.ID == 16));

            var receitaDespesaAgrupado = gListaReceitaDespesaFinanceira;
            receitaDespesaAgrupado = receitaDespesaAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            receitaDespesaAgrupado.Insert(receitaDespesaAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            receitaDespesaAgrupado.Insert(receitaDespesaAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (receitaDespesaAgrupado != null)
            {
                gvReceitaDespesaFin.DataSource = receitaDespesaAgrupado;
                gvReceitaDespesaFin.DataBind();
            }
        }
        private void CarregarImpostosTaxas(List<SP_DRE_COMPARATIVOResult> _dreGeral)
        {
            gListaImpostoTaxa.AddRange(_dreGeral.Where(p => p.ID == 21));

            var impostotaxaAgrupado = gListaImpostoTaxa;
            impostotaxaAgrupado = impostotaxaAgrupado.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = k.Key.GRUPO.Trim().ToUpper(),
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList();

            impostotaxaAgrupado.Insert(impostotaxaAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });
            impostotaxaAgrupado.Insert(impostotaxaAgrupado.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "", LINHA = "" });

            if (impostotaxaAgrupado != null)
            {
                gvImpostoTaxa.DataSource = impostotaxaAgrupado;
                gvImpostoTaxa.DataBind();
            }
        }

        #endregion

        #region "RECEITA LIQUIDA"

        protected void gvReceitaLiquida_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = (Convert.ToInt32(ddlAno.SelectedValue) - 2).ToString();
                e.Row.Cells[3].Text = (Convert.ToInt32(ddlAno.SelectedValue) - 1).ToString();
                e.Row.Cells[4].Text = ddlAno.SelectedValue + " Orçado";
                e.Row.Cells[5].Text = ddlAno.SelectedValue + " Efetivo";
                e.Row.Cells[6].Text = "(%) Comp " + ddlAno.SelectedValue + " Efet/" + (Convert.ToInt32(ddlAno.SelectedValue) - 2).ToString();
                e.Row.Cells[7].Text = "(%) Comp " + ddlAno.SelectedValue + " Efet/" + (Convert.ToInt32(ddlAno.SelectedValue) - 1).ToString();
                e.Row.Cells[8].Text = "(%) Comp " + ddlAno.SelectedValue + " Efet/ " + ddlAno.SelectedValue + " Orçado";
                e.Row.Cells[9].Text = "Diferença (R$) " + ddlAno.SelectedValue;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult receitaLiquida = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (receitaLiquida != null && receitaLiquida.GRUPO != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + receitaLiquida.GRUPO.Trim() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(receitaLiquida.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(receitaLiquida.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(receitaLiquida.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(receitaLiquida.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(receitaLiquida.AnoReal, receitaLiquida.AnoAnt2, 1, ((receitaLiquida.ID == 1) ? false : true));

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(receitaLiquida.AnoReal, receitaLiquida.AnoAnt1, 1, ((receitaLiquida.ID == 1) ? false : true));

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(receitaLiquida.AnoReal, receitaLiquida.AnoOrc, 1, ((receitaLiquida.ID == 1) ? false : true));

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((receitaLiquida.AnoReal - receitaLiquida.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(receitaLiquida.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(receitaLiquida.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(receitaLiquida.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(receitaLiquida.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((receitaLiquida.AnoReal - receitaLiquida.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Replace("Faturamento", "").Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 2,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
                lastRow.Cells[1].Text = "&nbsp;RECEITAS LÍQUIDAS";
                lastRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                lastRow.Cells[1].Font.Name = "Calibri";
                lastRow.Cells[2].Text = FormatarValor(dAnoAnt2);
                lastRow.Cells[2].Font.Name = "Calibri";
                lastRow.Cells[3].Text = FormatarValor(dAnoAnt1);
                lastRow.Cells[3].Font.Name = "Calibri";
                lastRow.Cells[4].Text = FormatarValor(dAnoOrc);
                lastRow.Cells[4].Font.Name = "Calibri";
                lastRow.Cells[5].Text = FormatarValor(dAnoReal);
                lastRow.Cells[5].Font.Name = "Calibri";
                lastRow.Cells[6].Text = CalculaPorc(dAnoReal, dAnoAnt2, 1, false);
                lastRow.Cells[6].Font.Name = "Calibri";
                lastRow.Cells[7].Text = CalculaPorc(dAnoReal, dAnoAnt1, 1, false);
                lastRow.Cells[7].Font.Name = "Calibri";
                lastRow.Cells[8].Text = CalculaPorc(dAnoReal, dAnoOrc, 1, false);
                lastRow.Cells[8].Font.Name = "Calibri";
                lastRow.Cells[9].Text = FormatarValor(dAtingidoValor);
                lastRow.Cells[9].Font.Name = "Calibri";

                var ga = gListaAtacado.GroupBy(x => new { GRUPO = x.GRUPO }).Select(p => new SP_DRE_COMPARATIVOResult
                {
                    AnoAnt2 = p.Sum(j => j.AnoAnt2),
                    AnoAnt1 = p.Sum(j => j.AnoAnt1),
                    AnoOrc = p.Sum(j => j.AnoOrc),
                    AnoReal = p.Sum(j => j.AnoReal)
                }).SingleOrDefault();

                decimal dAnoAnt2Atac = 0;
                decimal dAnoAnt1Atac = 0;
                decimal dAnoOrcAtac = 0;
                decimal dAnoRealAtac = 0;

                if (ga != null)
                {
                    dAnoAnt2Atac = Convert.ToDecimal(ga.AnoAnt2);
                    dAnoAnt1Atac = Convert.ToDecimal(ga.AnoAnt1);
                    dAnoOrcAtac = Convert.ToDecimal(ga.AnoOrc);
                    dAnoRealAtac = Convert.ToDecimal(ga.AnoReal);
                }

                gListaMargemContribuicaoVarejo.Add(new SP_DRE_COMPARATIVOResult
                {
                    GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                    LINHA = "Receita Varejo",
                    AnoAnt2 = dAnoAnt2 - dAnoAnt2Atac,
                    AnoAnt1 = dAnoAnt1 - dAnoAnt1Atac,
                    AnoOrc = dAnoOrc - dAnoOrcAtac,
                    AnoReal = dAnoReal - dAnoRealAtac
                });

                gListaMargemContribuicaoAtacado.Add(new SP_DRE_COMPARATIVOResult
                {
                    GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                    LINHA = "Receita Atacado",
                    AnoAnt2 = dAnoAnt2Atac,
                    AnoAnt1 = dAnoAnt1Atac,
                    AnoOrc = dAnoOrcAtac,
                    AnoReal = dAnoRealAtac
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult faturamento = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (faturamento != null && faturamento.GRUPO != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                            _litFaturamento.Text = "<font size='2' face='Calibri'>&nbsp; " + ((faturamento.LINHA.Trim().ToUpper().Contains("VAREJO") || faturamento.LINHA.Trim().ToUpper().Contains("ATACADO")) ? "" : "(-) ") + faturamento.LINHA.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(faturamento.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(faturamento.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(faturamento.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(faturamento.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(faturamento.AnoReal, faturamento.AnoAnt2, 1, ((faturamento.LINHA == "IMPOSTOS S/VENDA" || faturamento.LINHA == "TAXA CARTÕES") ? true : false));

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(faturamento.AnoReal, faturamento.AnoAnt1, 1, ((faturamento.LINHA == "IMPOSTOS S/VENDA" || faturamento.LINHA == "TAXA CARTÕES") ? true : false));

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(faturamento.AnoReal, faturamento.AnoOrc, 1, ((faturamento.LINHA == "IMPOSTOS S/VENDA" || faturamento.LINHA == "TAXA CARTÕES") ? true : false));

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((faturamento.AnoReal - faturamento.AnoOrc)));

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (faturamento.LINHA.Trim().ToUpper() == "IMPOSTOS S/VENDA")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                gListaImposto = gListaImposto.GroupBy(b => new { TIPO = b.TIPO }).Select(
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            LINHA = k.Key.TIPO.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.LINHA.Contains("Varejo"))
                                _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";
                            else
                                _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;(-) " + subitem.LINHA.Trim().ToUpper() + "</font> ";
                        }

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));
                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt2, 1, ((subitem.LINHA == "COFINS" || subitem.LINHA == "ICMS" || subitem.LINHA == "PIS") ? true : false));

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt1, 1, ((subitem.LINHA == "COFINS" || subitem.LINHA == "ICMS" || subitem.LINHA == "PIS") ? true : false));

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoOrc, 1, ((subitem.LINHA == "COFINS" || subitem.LINHA == "ICMS" || subitem.LINHA == "PIS") ? true : false));

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((subitem.AnoReal - subitem.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult cmv = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (cmv != null && cmv.GRUPO != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + cmv.GRUPO.Trim() + "</font> ";


                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                        {
                            ret = FormatarValor(FiltroValor(cmv.AnoAnt2), 1);
                            _litAnoAnt2.Text = ret;
                            porcAnoAnt2 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                        {
                            ret = FormatarValor(FiltroValor(cmv.AnoAnt1), 2);
                            _litAnoAnt1.Text = ret;
                            porcAnoAnt1 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                        {
                            ret = FormatarValor(FiltroValor(cmv.AnoOrc), 3);
                            _litAnoOrc.Text = ret;
                            porcAnoOrc = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                        {
                            ret = FormatarValor(FiltroValor(cmv.AnoReal), 4);
                            _litAnoReal.Text = ret;
                            porcAnoReal = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt2, 2, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt1, 2, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(porcAnoReal, porcAnoOrc, 2, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((cmv.AnoReal - cmv.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(cmv.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(cmv.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(cmv.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(cmv.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((cmv.AnoReal - cmv.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            gListaMargemContribuicao.Add(new SP_DRE_COMPARATIVOResult
            {
                GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                LINHA = "CMV",
                AnoAnt2 = dAnoAnt2,
                AnoAnt1 = dAnoAnt1,
                AnoOrc = dAnoOrc,
                AnoReal = dAnoReal
            });

            // SOMAR MARGEM DE CONTRIBUIÇÃO
            gListaMargemContribuicao = gListaMargemContribuicao.GroupBy(p => new { GRUPO = p.GRUPO }).Select(k => new SP_DRE_COMPARATIVOResult
            {
                GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                LINHA = "AGRUPADO",
                AnoAnt2 = k.Sum(g => g.AnoAnt2),
                AnoAnt1 = k.Sum(g => g.AnoAnt1),
                AnoOrc = k.Sum(g => g.AnoOrc),
                AnoReal = k.Sum(g => g.AnoReal)
            }).ToList();

            if (gListaMargemContribuicao.Count > 0)
            {
                gListaMargemContribuicao.Insert(gListaMargemContribuicao.Count, new SP_DRE_COMPARATIVOResult { GRUPO = "" });

                //Carregar margem de contribuicao
                gvMargem.DataSource = gListaMargemContribuicao;
                gvMargem.DataBind();
            }
        }

        protected void gvCMVItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult cmvlinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (cmvlinha != null && cmvlinha.GRUPO != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                            _litFaturamento.Text = "<font size='2' face='Calibri'>&nbsp;" + cmvlinha.LINHA.Trim().ToUpper() + "</font> ";


                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(cmvlinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(cmvlinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(cmvlinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(cmvlinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(cmvlinha.AnoReal, cmvlinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(cmvlinha.AnoReal, cmvlinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(cmvlinha.AnoReal, cmvlinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((cmvlinha.AnoReal - cmvlinha.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.LINHA.ToUpper().Contains("VAREJO"))
                                _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";
                            else
                                _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;(-) " + subitem.FILIAL.Trim().ToUpper() + "</font> ";
                        }

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((subitem.AnoReal - subitem.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult margem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (margem != null && margem.GRUPO != "")
                    {
                        Literal _litMargem = e.Row.FindControl("litMargem") as Literal;
                        if (_litMargem != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litMargem.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + margem.GRUPO.Trim() + "</i></font></span>";
                            else
                                _litMargem.Text = "<font size='2' face='Calibri'>&nbsp;" + margem.GRUPO.Trim() + "</font>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAnoAnt2.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.AnoAnt2)).Trim() + ((margem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt2.Text = FormatarValor(FiltroValor(margem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAnoAnt1.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.AnoAnt1)).Trim() + ((margem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt1.Text = FormatarValor(FiltroValor(margem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAnoOrc.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.AnoOrc)).Trim() + ((margem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoOrc.Text = FormatarValor(FiltroValor(margem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAnoReal.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margem.AnoReal)).Trim() + ((margem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoReal.Text = FormatarValor(FiltroValor(margem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAnoAnt2Porc.Text = "-";
                            else
                                _litAnoAnt2Porc.Text = CalculaPorc(margem.AnoReal, margem.AnoAnt2, 1, false);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAnoAnt1Porc.Text = "-";
                            else
                                _litAnoAnt1Porc.Text = CalculaPorc(margem.AnoReal, margem.AnoAnt1, 1, false);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAnoOrcPorc.Text = "-";
                            else
                                _litAnoOrcPorc.Text = CalculaPorc(margem.AnoReal, margem.AnoOrc, 1, false);




                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            if (margem.LINHA.Contains("%") || (margem.LINHA.Trim() == "MARK-UP"))
                                _litAtingidoValor.Text = "-";
                            else
                                _litAtingidoValor.Text = FormatarValor(FiltroValor((margem.AnoReal - margem.AnoOrc))); ;


                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(margem.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(margem.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(margem.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(margem.AnoReal));

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
                            List<SP_DRE_COMPARATIVOResult> _margemAtacado = new List<SP_DRE_COMPARATIVOResult>();
                            List<SP_DRE_COMPARATIVOResult> _margemVarejo = new List<SP_DRE_COMPARATIVOResult>();

                            if (gvMargemItem != null)
                            {
                                //adiciona CMV Atacado
                                _margemAtacado.AddRange(gListaCmvAtacado.GroupBy(p => new { GRUPO = p.GRUPO }).Select(g => new SP_DRE_COMPARATIVOResult
                                {
                                    GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                                    AnoAnt2 = g.Sum(x => x.AnoAnt2),
                                    AnoAnt1 = g.Sum(x => x.AnoAnt1),
                                    AnoOrc = g.Sum(x => x.AnoOrc),
                                    AnoReal = g.Sum(x => x.AnoReal)
                                }));
                                var margemA = _margemAtacado.Union(gListaMargemContribuicaoAtacado).ToList();
                                margemA = margemA.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = "ATACADO",
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
                                                                        }).ToList();



                                //adiciona CMV Varejo
                                _margemVarejo.AddRange(gListaCmvVarejo.GroupBy(p => new { GRUPO = p.GRUPO }).Select(g => new SP_DRE_COMPARATIVOResult
                                {
                                    GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                                    AnoAnt2 = g.Sum(x => x.AnoAnt2),
                                    AnoAnt1 = g.Sum(x => x.AnoAnt1),
                                    AnoOrc = g.Sum(x => x.AnoOrc),
                                    AnoReal = g.Sum(x => x.AnoReal)
                                }));
                                var margemV = _margemVarejo.Union(gListaMargemContribuicaoVarejo).ToList();
                                margemV = margemV.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = "VAREJO",
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
                                                                        }).ToList();

                                List<SP_DRE_COMPARATIVOResult> _margem = new List<SP_DRE_COMPARATIVOResult>();
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
                    SP_DRE_COMPARATIVOResult _margem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (_margem != null && _margem.LINHA != "")
                    {
                        Literal _litVenda = e.Row.FindControl("litVenda") as Literal;
                        if (_litVenda != null)
                            _litVenda.Text = "<font size='2' face='Calibri'>&nbsp;" + _margem.LINHA.Replace("RECEITA", "").Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(_margem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(_margem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(_margem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(_margem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(_margem.AnoReal, _margem.AnoAnt2, 1, false);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(_margem.AnoReal, _margem.AnoAnt1, 1, false);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(_margem.AnoReal, _margem.AnoOrc, 1, false);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((_margem.AnoReal - _margem.AnoOrc))); ; ;

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        GridView gvMargemItemSub = e.Row.FindControl("gvMargemItemSub") as GridView;
                        if (gvMargemItemSub != null)
                        {
                            decimal pAnoAnt2 = 0;
                            decimal pAnoAnt1 = 0;
                            decimal pAnoOrc = 0;
                            decimal pAnoReal = 0;

                            List<SP_DRE_COMPARATIVOResult> margem = new List<SP_DRE_COMPARATIVOResult>();

                            if (_margem.LINHA.ToUpper().Contains("VAREJO"))
                            {
                                //OBTER % MARGEM DE CONTRIBUICAO
                                pAnoAnt2 = (gListaVarejo.Sum(i => i.AnoAnt2) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.AnoAnt2) / gListaVarejo.Sum(i => i.AnoAnt2)) * 100));
                                pAnoAnt1 = (gListaVarejo.Sum(i => i.AnoAnt1) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.AnoAnt1) / gListaVarejo.Sum(i => i.AnoAnt1)) * 100));
                                pAnoOrc = (gListaVarejo.Sum(i => i.AnoOrc) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.AnoOrc) / gListaVarejo.Sum(i => i.AnoOrc)) * 100));
                                pAnoReal = (gListaVarejo.Sum(i => i.AnoReal) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoVarejoAux.Sum(j => j.AnoReal) / gListaVarejo.Sum(i => i.AnoReal)) * 100));

                                margem.Insert(0, new SP_DRE_COMPARATIVOResult
                                {
                                    GRUPO = "% MARGEM CONTRIBUIÇÃO",
                                    LINHA = "% VAREJO",
                                    AnoAnt2 = pAnoAnt2,
                                    AnoAnt1 = pAnoAnt1,
                                    AnoOrc = pAnoOrc,
                                    AnoReal = pAnoReal
                                });

                                //OBTER MARK-UP
                                pAnoAnt2 = ((gListaCmvVarejo.Sum(i => i.AnoAnt2)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.AnoAnt2)) / ((-1) * (gListaCmvVarejo.Sum(i => i.AnoAnt2))));
                                pAnoAnt1 = ((gListaCmvVarejo.Sum(i => i.AnoAnt1)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.AnoAnt1)) / ((-1) * (gListaCmvVarejo.Sum(i => i.AnoAnt1))));
                                pAnoOrc = ((gListaCmvVarejo.Sum(i => i.AnoOrc)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.AnoOrc)) / ((-1) * (gListaCmvVarejo.Sum(i => i.AnoOrc))));
                                pAnoReal = ((gListaCmvVarejo.Sum(i => i.AnoReal)) == 0) ? 0 : Convert.ToDecimal((gListaVarejo.Sum(i => i.AnoReal)) / ((-1) * (gListaCmvVarejo.Sum(i => i.AnoReal))));

                                margem.Insert(1, new SP_DRE_COMPARATIVOResult
                                {
                                    GRUPO = "MARK-UP",
                                    LINHA = "MARK-UP VAREJO",
                                    AnoAnt2 = pAnoAnt2,
                                    AnoAnt1 = pAnoAnt1,
                                    AnoOrc = pAnoOrc,
                                    AnoReal = pAnoReal
                                });


                            }

                            if (_margem.LINHA.ToUpper().Contains("ATACADO"))
                            {

                                //OBTER % MARGEM DE CONTRIBUICAO
                                pAnoAnt2 = (gListaAtacado.Sum(i => i.AnoAnt2) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.AnoAnt2) / gListaAtacado.Sum(i => i.AnoAnt2)) * 100));
                                pAnoAnt1 = (gListaAtacado.Sum(i => i.AnoAnt1) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.AnoAnt1) / gListaAtacado.Sum(i => i.AnoAnt1)) * 100));
                                pAnoOrc = (gListaAtacado.Sum(i => i.AnoOrc) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.AnoOrc) / gListaAtacado.Sum(i => i.AnoOrc)) * 100));
                                pAnoReal = (gListaAtacado.Sum(i => i.AnoReal) == 0) ? 0 : Convert.ToDecimal(((gListaMargemContribuicaoAtacadoAux.Sum(j => j.AnoReal) / gListaAtacado.Sum(i => i.AnoReal)) * 100));

                                margem.Insert(0, new SP_DRE_COMPARATIVOResult
                                {
                                    GRUPO = "% MARGEM CONTRIBUIÇÃO",
                                    LINHA = "% ATACADO",
                                    AnoAnt2 = pAnoAnt2,
                                    AnoAnt1 = pAnoAnt1,
                                    AnoOrc = pAnoOrc,
                                    AnoReal = pAnoReal
                                });

                                //OBTER MARK-UP
                                pAnoAnt2 = ((gListaCmvAtacado.Sum(i => i.AnoAnt2)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.AnoAnt2)) / ((-1) * (gListaCmvAtacado.Sum(i => i.AnoAnt2))));
                                pAnoAnt1 = ((gListaCmvAtacado.Sum(i => i.AnoAnt1)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.AnoAnt1)) / ((-1) * (gListaCmvAtacado.Sum(i => i.AnoAnt1))));
                                pAnoOrc = ((gListaCmvAtacado.Sum(i => i.AnoOrc)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.AnoOrc)) / ((-1) * (gListaCmvAtacado.Sum(i => i.AnoOrc))));
                                pAnoReal = ((gListaCmvAtacado.Sum(i => i.AnoReal)) == 0) ? 0 : Convert.ToDecimal((gListaAtacado.Sum(i => i.AnoReal)) / ((-1) * (gListaCmvAtacado.Sum(i => i.AnoReal))));

                                margem.Insert(1, new SP_DRE_COMPARATIVOResult
                                {
                                    GRUPO = "MARK-UP",
                                    LINHA = "MARK-UP ATACADO",
                                    AnoAnt2 = pAnoAnt2,
                                    AnoAnt1 = pAnoAnt1,
                                    AnoOrc = pAnoOrc,
                                    AnoReal = pAnoReal
                                });
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            if (subitem.LINHA.Contains("%") || (subitem.LINHA.Trim().Contains("MARK-UP")))
                                _litSubItem.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;<i>" + subitem.GRUPO.Replace("MARGEM", "MARG").Trim() + "</i></font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            if (subitem.LINHA.Contains("%") || (subitem.LINHA.Trim() == "MARK-UP"))
                                _litAnoAnt2.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.AnoAnt2)).Trim() + ((subitem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            if (subitem.LINHA.Contains("%") || (subitem.LINHA.Trim() == "MARK-UP"))
                                _litAnoAnt1.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.AnoAnt1)).Trim() + ((subitem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            if (subitem.LINHA.Contains("%") || (subitem.LINHA.Trim() == "MARK-UP"))
                                _litAnoOrc.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.AnoOrc)).Trim() + ((subitem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            if (subitem.LINHA.Contains("%") || (subitem.LINHA.Trim() == "MARK-UP"))
                                _litAnoReal.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(subitem.AnoReal)).Trim() + ((subitem.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = "-";

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = "-";

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = "-";

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = "-";

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
                    SP_DRE_COMPARATIVOResult cfixo = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (cfixo != null && cfixo.GRUPO != "")
                    {
                        Literal _litCustoFixo = e.Row.FindControl("litCustoFixo") as Literal;
                        if (_litCustoFixo != null)
                            _litCustoFixo.Text = "<font size='2' face='Calibri'>&nbsp;" + cfixo.GRUPO.Trim() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                        {
                            ret = FormatarValor(FiltroValor(cfixo.AnoAnt2), 1);
                            _litAnoAnt2.Text = ret;
                            porcAnoAnt2 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                        {
                            ret = FormatarValor(FiltroValor(cfixo.AnoAnt1), 2);
                            _litAnoAnt1.Text = ret;
                            porcAnoAnt1 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                        {
                            ret = FormatarValor(FiltroValor(cfixo.AnoOrc), 3);
                            _litAnoOrc.Text = ret;
                            porcAnoOrc = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                        {
                            ret = FormatarValor(FiltroValor(cfixo.AnoReal), 4);
                            _litAnoReal.Text = ret;
                            porcAnoReal = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt2, 2, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt1, 2, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(porcAnoReal, porcAnoOrc, 2, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((cfixo.AnoReal - cfixo.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(cfixo.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(cfixo.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(cfixo.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(cfixo.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((cfixo.AnoReal - cfixo.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            gListaMargemBruta.AddRange(gListaMargemContribuicao.Where(p => p.GRUPO != ""));

            // CUSTO FIXO
            gListaMargemBruta.AddRange(gListaCustoFixo.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "MARGEM DE CONTRIBUIÇÃO",
                                                                                LINHA = "AGRUPADO",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }));

            // SOMAR MARGEM DE CONTRIBUIÇÃO
            gListaMargemBruta = gListaMargemBruta.GroupBy(p => new { Grupo = p.GRUPO }).Select(k => new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "MARGEM BRUTA",
                LINHA = "AGRUPADO",
                AnoAnt2 = k.Sum(g => g.AnoAnt2),
                AnoAnt1 = k.Sum(g => g.AnoAnt1),
                AnoOrc = k.Sum(g => g.AnoOrc),
                AnoReal = k.Sum(g => g.AnoReal)
            }).ToList();


            if (gListaMargemBruta.Count > 0)
            {

                //OBTER % MARGEM BRUTA
                decimal pAnoAnt2 = (gListaReceitaLiquida.Sum(i => i.AnoAnt2) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.AnoAnt2) / gListaReceitaLiquida.Sum(i => i.AnoAnt2)) * 100));
                decimal pAnoAnt1 = (gListaReceitaLiquida.Sum(i => i.AnoAnt1) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.AnoAnt1) / gListaReceitaLiquida.Sum(i => i.AnoAnt1)) * 100));
                decimal pAnoOrc = (gListaReceitaLiquida.Sum(i => i.AnoOrc) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.AnoOrc) / gListaReceitaLiquida.Sum(i => i.AnoOrc)) * 100));
                decimal pAnoReal = (gListaReceitaLiquida.Sum(i => i.AnoReal) == 0) ? 0 : Convert.ToDecimal(((gListaMargemBruta.Sum(j => j.AnoReal) / gListaReceitaLiquida.Sum(i => i.AnoReal)) * 100));

                gListaMargemBruta.Insert(gListaMargemBruta.Count, new SP_DRE_COMPARATIVOResult
                {
                    ID = 98,
                    GRUPO = "% MARGEM BRUTA",
                    LINHA = "%",
                    AnoAnt2 = pAnoAnt2,
                    AnoAnt1 = pAnoAnt1,
                    AnoOrc = pAnoOrc,
                    AnoReal = pAnoReal,
                });

                gListaMargemBruta.Insert(gListaMargemBruta.Count, new SP_DRE_COMPARATIVOResult { ID = 97, GRUPO = "" });
                //gListaMargemBruta.Insert(gListaMargemBruta.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });

                //Carregar margem bruta
                gvMargemBruta.DataSource = gListaMargemBruta;
                gvMargemBruta.DataBind();

            }
        }

        protected void gvCustoFixoItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult custoFixoLinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (custoFixoLinha != null && custoFixoLinha.LINHA != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + custoFixoLinha.LINHA.Trim().ToUpper() + "</font></span>";
                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(custoFixoLinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(custoFixoLinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(custoFixoLinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(custoFixoLinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(custoFixoLinha.AnoReal, custoFixoLinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(custoFixoLinha.AnoReal, custoFixoLinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(custoFixoLinha.AnoReal, custoFixoLinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((custoFixoLinha.AnoReal - custoFixoLinha.AnoOrc)));

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

        #endregion

        #region "MARGEM BRUTA"

        protected void gvMargemBruta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult margemBruta = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (margemBruta != null && margemBruta.GRUPO != "")
                    {
                        Literal _litMargemBruta = e.Row.FindControl("litMargemBruta") as Literal;
                        if (_litMargemBruta != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litMargemBruta.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + margemBruta.GRUPO.Trim() + "</i></font></span>";
                            else
                                _litMargemBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + margemBruta.GRUPO.Trim() + "</font>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAnoAnt2.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.AnoAnt2)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt2.Text = FormatarValor(FiltroValor(margemBruta.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAnoAnt1.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.AnoAnt1)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt1.Text = FormatarValor(FiltroValor(margemBruta.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAnoOrc.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.AnoOrc)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoOrc.Text = FormatarValor(FiltroValor(margemBruta.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAnoReal.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.AnoReal)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
                            else
                                _litAnoReal.Text = FormatarValor(FiltroValor(margemBruta.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAnoAnt2Porc.Text = CalculaPorc(margemBruta.AnoReal, margemBruta.AnoAnt2, 2, false);
                            else
                                _litAnoAnt2Porc.Text = CalculaPorc(margemBruta.AnoReal, margemBruta.AnoAnt2, 1, false);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAnoAnt1Porc.Text = CalculaPorc(margemBruta.AnoReal, margemBruta.AnoAnt1, 2, false);
                            else
                                _litAnoAnt1Porc.Text = CalculaPorc(margemBruta.AnoReal, margemBruta.AnoAnt1, 1, false);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAnoOrcPorc.Text = CalculaPorc(margemBruta.AnoReal, margemBruta.AnoOrc, 2, false);
                            else
                                _litAnoOrcPorc.Text = CalculaPorc(margemBruta.AnoReal, margemBruta.AnoOrc, 1, false);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            if (margemBruta.LINHA.Contains("%"))
                                _litAtingidoValor.Text = "-";
                            else
                                _litAtingidoValor.Text = FormatarValor(FiltroValor((margemBruta.AnoReal - margemBruta.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(margemBruta.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(margemBruta.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(margemBruta.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(margemBruta.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((margemBruta.AnoReal - margemBruta.AnoOrc)));

                    }

                    if (margemBruta.GRUPO == "")
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult desploja = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (desploja != null && desploja.GRUPO != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + desploja.GRUPO.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoAnt2), 1);
                            _litAnoAnt2.Text = ret;
                            porcAnoAnt2 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoAnt1), 2);
                            _litAnoAnt1.Text = ret;
                            porcAnoAnt1 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoOrc), 3);
                            _litAnoOrc.Text = ret;
                            porcAnoOrc = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoReal), 4);
                            _litAnoReal.Text = ret;
                            porcAnoReal = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt2, 2, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt1, 2, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(porcAnoReal, porcAnoOrc, 2, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((desploja.AnoReal - desploja.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(desploja.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(desploja.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(desploja.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(desploja.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((desploja.AnoReal - desploja.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)

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
                    SP_DRE_COMPARATIVOResult desplojalinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (desplojalinha != null && desplojalinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;" + (((desplojalinha.LINHA.Trim().Length < 18) ? desplojalinha.LINHA.Trim() : desplojalinha.LINHA.Trim().Substring(0, 18))) + "</font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(desplojalinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(desplojalinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(desplojalinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(desplojalinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(desplojalinha.AnoReal, desplojalinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(desplojalinha.AnoReal, desplojalinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(desplojalinha.AnoReal, desplojalinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((desplojalinha.AnoReal - desplojalinha.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((subitem.AnoReal - subitem.AnoOrc)));

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

        #region "DESPESAS ESPECIFICAS"

        protected void gvDespesaEspecifica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult desploja = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (desploja != null && desploja.GRUPO != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + desploja.GRUPO.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoAnt2), 1);
                            _litAnoAnt2.Text = ret;
                            porcAnoAnt2 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoAnt1), 2);
                            _litAnoAnt1.Text = ret;
                            porcAnoAnt1 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoOrc), 3);
                            _litAnoOrc.Text = ret;
                            porcAnoOrc = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                        {
                            ret = FormatarValor(FiltroValor(desploja.AnoReal), 4);
                            _litAnoReal.Text = ret;
                            porcAnoReal = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt2, 2, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt1, 2, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(porcAnoReal, porcAnoOrc, 2, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((desploja.AnoReal - desploja.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(desploja.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(desploja.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(desploja.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(desploja.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((desploja.AnoReal - desploja.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)

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
                    SP_DRE_COMPARATIVOResult desplojalinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (desplojalinha != null && desplojalinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;" + (((desplojalinha.LINHA.Trim().Length < 18) ? desplojalinha.LINHA.Trim() : desplojalinha.LINHA.Trim().Substring(0, 18))) + "</font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(desplojalinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(desplojalinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(desplojalinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(desplojalinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(desplojalinha.AnoReal, desplojalinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(desplojalinha.AnoReal, desplojalinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(desplojalinha.AnoReal, desplojalinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((desplojalinha.AnoReal - desplojalinha.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.FILIAL.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((subitem.AnoReal - subitem.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult despvenda = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (despvenda != null && despvenda.GRUPO != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despvenda.GRUPO.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                        {
                            ret = FormatarValor(FiltroValor(despvenda.AnoAnt2), 1);
                            _litAnoAnt2.Text = ret;
                            porcAnoAnt2 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                        {
                            ret = FormatarValor(FiltroValor(despvenda.AnoAnt1), 2);
                            _litAnoAnt1.Text = ret;
                            porcAnoAnt1 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                        {
                            ret = FormatarValor(FiltroValor(despvenda.AnoOrc), 3);
                            _litAnoOrc.Text = ret;
                            porcAnoOrc = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                        {
                            ret = FormatarValor(FiltroValor(despvenda.AnoReal), 4);
                            _litAnoReal.Text = ret;
                            porcAnoReal = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt2, 2, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt1, 2, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(porcAnoReal, porcAnoOrc, 2, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((despvenda.AnoReal - despvenda.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(despvenda.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(despvenda.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(despvenda.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(despvenda.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((despvenda.AnoReal - despvenda.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
                    SP_DRE_COMPARATIVOResult despvendalinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (despvendalinha != null && despvendalinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<span><font size='2' face='Calibri'>&nbsp;" + despvendalinha.LINHA.Trim().ToUpper().Substring(0, ((despvendalinha.LINHA.Trim().Length > 19) ? 19 : despvendalinha.LINHA.Trim().Length)) + "</font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(despvendalinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(despvendalinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(despvendalinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(despvendalinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(despvendalinha.AnoReal, despvendalinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(despvendalinha.AnoReal, despvendalinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(despvendalinha.AnoReal, despvendalinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((despvendalinha.AnoReal - despvendalinha.AnoOrc)));

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;

                        if (despvendalinha.LINHA.Trim().ToUpper() == "SALÁRIOS" || despvendalinha.LINHA.Trim().ToUpper() == "ENCARGOS")
                        {
                            GridView gvDespesaVendaItemSub = e.Row.FindControl("gvDespesaVendaItemSub") as GridView;
                            if (gvDespesaVendaItemSub != null)
                            {
                                gvDespesaVendaItemSub.DataSource = gListaDespesaVenda.Where(p => p.LINHA.Trim().ToUpper() == despvendalinha.LINHA.Trim().ToUpper()).GroupBy(j => new
                                {
                                    TIPO = j.TIPO
                                }).Select(k => new SP_DRE_COMPARATIVOResult
                                {
                                    TIPO = k.Key.TIPO.Trim().ToUpper(),
                                    AnoAnt2 = k.Sum(s => s.AnoAnt2),
                                    AnoAnt1 = k.Sum(s => s.AnoAnt1),
                                    AnoOrc = k.Sum(s => s.AnoOrc),
                                    AnoReal = k.Sum(s => s.AnoReal)
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.TIPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;" + subitem.TIPO.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((subitem.AnoReal - subitem.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult despadm = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (despadm != null && despadm.GRUPO != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despadm.GRUPO.Trim().ToUpper() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                        {
                            ret = FormatarValor(FiltroValor(despadm.AnoAnt2), 1);
                            _litAnoAnt2.Text = ret;
                            porcAnoAnt2 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                        {
                            ret = FormatarValor(FiltroValor(despadm.AnoAnt1), 2);
                            _litAnoAnt1.Text = ret;
                            porcAnoAnt1 = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                        {
                            ret = FormatarValor(FiltroValor(despadm.AnoOrc), 3);
                            _litAnoOrc.Text = ret;
                            porcAnoOrc = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                        {
                            ret = FormatarValor(FiltroValor(despadm.AnoReal), 4);
                            _litAnoReal.Text = ret;
                            porcAnoReal = Convert.ToDecimal(ret.Substring(ret.LastIndexOf('(') + 1, ret.LastIndexOf(')') - ret.LastIndexOf('(') - 2).Trim());
                        }

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt2, 2, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(porcAnoReal, porcAnoAnt1, 2, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(porcAnoReal, porcAnoOrc, 2, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((despadm.AnoReal - despadm.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(despadm.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(despadm.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(despadm.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(despadm.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((despadm.AnoReal - despadm.AnoOrc)));

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
                                var despAdmPessoal = gListaDespesaAdmPessoal;
                                despAdmPessoal = despAdmPessoal.GroupBy(b => new { LINHA = b.LINHA.Trim().Substring(0, 7) }).Select(
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)

                                                                        }).ToList();

                                var despAdmOutros = gListaDespesaAdmOutros;
                                despAdmOutros = despAdmOutros.GroupBy(b => new { LINHA = b.LINHA.Trim().ToUpper() }).Select(
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 2,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            gListaEBITDA.AddRange(gListaMargemBruta.Where(p => p.ID == 99));
            //  ADD DESPESA LOJA - DESPESAS CTO
            gListaEBITDA.AddRange(gListaDespesaCTO.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 99,
                                                                                GRUPO = "DESPESAS CTO",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList());

            //  ADD DESPESA LOJA - ESPECIFICA
            gListaEBITDA.AddRange(gListaDespesaEspecifica.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 99,
                                                                                GRUPO = "DESPESAS ESPECÍFICAS",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList());
            //  ADD DESPESA VENDA
            gListaEBITDA.AddRange(gListaDespesaVenda.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_DRE_COMPARATIVOResult
                                                                            {
                                                                                ID = 99,
                                                                                LINHA = "DESPESAS VENDAS",
                                                                                AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                                AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                                AnoOrc = k.Sum(j => j.AnoOrc),
                                                                                AnoReal = k.Sum(j => j.AnoReal)
                                                                            }).ToList());
            //  ADD DESPESA ADMINISTRATIVA
            gListaEBITDA.AddRange(gListaDespesaAdmPessoal.Union(gListaDespesaAdmOutros).GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                           k => new SP_DRE_COMPARATIVOResult
                                                                           {
                                                                               ID = 99,
                                                                               LINHA = "DESPESAS ADM",
                                                                               AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                               AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                               AnoOrc = k.Sum(j => j.AnoOrc),
                                                                               AnoReal = k.Sum(j => j.AnoReal)
                                                                           }).ToList());

            //AGRUPA TUDO E SOMA OS MESES
            gListaEBITDA = gListaEBITDA.GroupBy(p => new { ID = p.ID }).Select(
                                                                           k => new SP_DRE_COMPARATIVOResult
                                                                           {
                                                                               ID = 99,
                                                                               GRUPO = "EBITDA",
                                                                               LINHA = "",
                                                                               AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                               AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                               AnoOrc = k.Sum(j => j.AnoOrc),
                                                                               AnoReal = k.Sum(j => j.AnoReal)
                                                                           }).ToList();

            //ADD PORCENTAGEM
            if (gListaEBITDA.Count > 0)
            {
                //OBTER % MARGEM EBITDA
                decimal pAnoAnt2 = (gListaReceitaLiquida.Sum(i => i.AnoAnt2) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.AnoAnt2) / gListaReceitaLiquida.Sum(i => i.AnoAnt2)) * 100));
                decimal pAnoAnt1 = (gListaReceitaLiquida.Sum(i => i.AnoAnt1) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.AnoAnt1) / gListaReceitaLiquida.Sum(i => i.AnoAnt1)) * 100));
                decimal pAnoOrc = (gListaReceitaLiquida.Sum(i => i.AnoOrc) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.AnoOrc) / gListaReceitaLiquida.Sum(i => i.AnoOrc)) * 100));
                decimal pAnoReal = (gListaReceitaLiquida.Sum(i => i.AnoReal) == 0) ? 0 : Convert.ToDecimal(((gListaEBITDA.Sum(j => j.AnoReal) / gListaReceitaLiquida.Sum(i => i.AnoReal)) * 100));

                gListaEBITDA.Insert(gListaEBITDA.Count, new SP_DRE_COMPARATIVOResult
                {
                    ID = 99,
                    GRUPO = "% Margem EBITDA",
                    LINHA = "%",
                    AnoAnt2 = pAnoAnt2,
                    AnoAnt1 = pAnoAnt1,
                    AnoOrc = pAnoOrc,
                    AnoReal = pAnoReal
                });

            }

            gListaEBITDA.Insert(gListaEBITDA.Count, new SP_DRE_COMPARATIVOResult { ID = 99, GRUPO = "", LINHA = "" });

            gvEbtida.DataSource = gListaEBITDA;
            gvEbtida.DataBind();
        }

        protected void gvDespesaAdmItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult despadmlinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (despadmlinha != null && despadmlinha.LINHA != "")
                    {
                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despadmlinha.LINHA.Trim().ToUpper().Substring(0, ((despadmlinha.LINHA.Trim().Length > 20) ? 20 : despadmlinha.LINHA.Trim().Length)) + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(despadmlinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(despadmlinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(despadmlinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(despadmlinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(despadmlinha.AnoReal, despadmlinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(despadmlinha.AnoReal, despadmlinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(despadmlinha.AnoReal, despadmlinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((despadmlinha.AnoReal - despadmlinha.AnoOrc)));

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;

                        if (!despadmlinha.LINHA.Trim().ToUpper().Contains("BENEFICIOS"))
                        {
                            if (despadmlinha.ID == 1)
                            {
                                GridView gvDespesaAdmItemSub = e.Row.FindControl("gvDespesaAdmItemSub") as GridView;
                                if (gvDespesaAdmItemSub != null)
                                {
                                    var lstdespAdmLinha = gListaDespesaAdmPessoal.GroupBy(j => new
                                        {
                                            LINHA = j.LINHA.Replace("Pessoal - ", "")
                                        }).Select(k => new SP_DRE_COMPARATIVOResult
                                        {
                                            ID = 1,
                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                            AnoAnt2 = k.Sum(s => s.AnoAnt2),
                                            AnoAnt1 = k.Sum(s => s.AnoAnt1),
                                            AnoOrc = k.Sum(s => s.AnoOrc),
                                            AnoReal = k.Sum(s => s.AnoReal)
                                        }).ToList();

                                    gvDespesaAdmItemSub.DataSource = lstdespAdmLinha.OrderBy(p => p.TIPO).ThenBy(x => x.LINHA);
                                    gvDespesaAdmItemSub.DataBind();
                                }
                                img.Visible = true;
                            }

                            if (despadmlinha.ID == 2)
                            {
                                GridView gvDespesaAdmItemSub = e.Row.FindControl("gvDespesaAdmItemSub") as GridView;
                                if (gvDespesaAdmItemSub != null)
                                {
                                    var lstdespAdmLinha2 = gListaDespesaAdmOutros.Where(i => i.LINHA.Trim().ToUpper() == despadmlinha.LINHA.Trim().ToUpper()).GroupBy(j => new
                                    {
                                        LINHA = j.TIPO.Trim()
                                    }).Select(k => new SP_DRE_COMPARATIVOResult
                                    {
                                        ID = 1,
                                        LINHA = k.Key.LINHA.Trim().ToUpper(),
                                        AnoAnt2 = k.Sum(s => s.AnoAnt2),
                                        AnoAnt1 = k.Sum(s => s.AnoAnt1),
                                        AnoOrc = k.Sum(s => s.AnoOrc),
                                        AnoReal = k.Sum(s => s.AnoReal)
                                    }).ToList();

                                    gvDespesaAdmItemSub.DataSource = lstdespAdmLinha2.OrderBy(p => p.TIPO).ThenBy(x => x.LINHA);
                                    gvDespesaAdmItemSub.DataBind();
                                }
                                img.Visible = true;
                            }
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
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.LINHA != "")
                    {

                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.LINHA.Trim().ToUpper().Substring(0, ((subitem.LINHA.Trim().Length > 21) ? 21 : subitem.LINHA.Trim().Length)) + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((subitem.AnoReal - subitem.AnoOrc)));

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;

                        if (subitem.LINHA.Trim().ToUpper() == "SALÁRIOS" || subitem.LINHA.Trim().ToUpper() == "ENCARGOS")
                        {
                            GridView gvDespesaAdmItemSubSub = e.Row.FindControl("gvDespesaAdmItemSubSub") as GridView;
                            if (gvDespesaAdmItemSubSub != null)
                            {
                                var lstdespAdmSub = gListaDespesaAdmPessoal.Where(g => g.LINHA.Trim().ToUpper().Contains(subitem.LINHA.Trim().ToUpper())).GroupBy(j => new
                                {
                                    TIPO = j.TIPO.Trim().ToUpper()
                                }).Select(k => new SP_DRE_COMPARATIVOResult
                                {
                                    ID = 1,
                                    TIPO = k.Key.TIPO.Trim().ToUpper(),
                                    AnoAnt2 = k.Sum(s => s.AnoAnt2),
                                    AnoAnt1 = k.Sum(s => s.AnoAnt1),
                                    AnoOrc = k.Sum(s => s.AnoOrc),
                                    AnoReal = k.Sum(s => s.AnoReal)
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult subitem = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (subitem != null && subitem.TIPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + subitem.TIPO.Trim().ToUpper().Substring(0, ((subitem.TIPO.Trim().Length > 20) ? 20 : subitem.TIPO.Trim().Length)) + "</font>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(subitem.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(subitem.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(subitem.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(subitem.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(subitem.AnoReal, subitem.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((subitem.AnoReal - subitem.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult ebtida = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (ebtida != null && ebtida.GRUPO != "")
                    {
                        Literal _litEbtida = e.Row.FindControl("litEbtida") as Literal;
                        if (_litEbtida != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litEbtida.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + ebtida.GRUPO.Trim() + "</i></font></span>";
                            else
                                _litEbtida.Text = "<font size='2' face='Calibri'>&nbsp;" + ebtida.GRUPO.Trim() + "</font>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAnoAnt2.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.AnoAnt2)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.AnoAnt2 < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt2.Text = FormatarValor(FiltroValor(ebtida.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAnoAnt1.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.AnoAnt1)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.AnoAnt1 < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _litAnoAnt1.Text = FormatarValor(FiltroValor(ebtida.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAnoOrc.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.AnoOrc)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.AnoOrc < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _litAnoOrc.Text = FormatarValor(FiltroValor(ebtida.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAnoReal.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(ebtida.AnoReal)).Trim() + ((ebtida.LINHA.Contains("%") ? "<font size='2' face='Calibri' color='" + ((ebtida.AnoReal < 0) ? tagCorNegativo : "") + "'>%</font>" : "")) + "</span>";
                            else
                                _litAnoReal.Text = FormatarValor(FiltroValor(ebtida.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAnoAnt2Porc.Text = CalculaPorc(ebtida.AnoReal, ebtida.AnoAnt2, 2, false);
                            else
                                _litAnoAnt2Porc.Text = CalculaPorc(ebtida.AnoReal, ebtida.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAnoAnt1Porc.Text = CalculaPorc(ebtida.AnoReal, ebtida.AnoAnt1, 2, false);
                            else
                                _litAnoAnt1Porc.Text = CalculaPorc(ebtida.AnoReal, ebtida.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAnoOrcPorc.Text = CalculaPorc(ebtida.AnoReal, ebtida.AnoOrc, 2, false);
                            else
                                _litAnoOrcPorc.Text = CalculaPorc(ebtida.AnoReal, ebtida.AnoOrc, 1, true);


                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            if (ebtida.LINHA.Contains("%"))
                                _litAtingidoValor.Text = "-";
                            else
                                _litAtingidoValor.Text = FormatarValor(FiltroValor((ebtida.AnoReal - ebtida.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(ebtida.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(ebtida.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(ebtida.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(ebtida.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((ebtida.AnoReal - ebtida.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult depreciacao = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (depreciacao != null && depreciacao.GRUPO != "")
                    {
                        Literal _litDepreciacao = e.Row.FindControl("litDepreciacao") as Literal;
                        if (_litDepreciacao != null)
                            _litDepreciacao.Text = "<font size='2' face='Calibri'>&nbsp;" + depreciacao.GRUPO.Trim() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(depreciacao.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(depreciacao.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(depreciacao.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(depreciacao.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(depreciacao.AnoReal, depreciacao.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(depreciacao.AnoReal, depreciacao.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(depreciacao.AnoReal, depreciacao.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((depreciacao.AnoReal - depreciacao.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(depreciacao.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(depreciacao.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(depreciacao.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(depreciacao.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((depreciacao.AnoReal - depreciacao.AnoOrc)));
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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            TIPO = k.Key.TIPO.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            gListaLucroBruto.Add(new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "LUCRO BRUTO",
                LINHA = "",
                AnoAnt2 = dAnoAnt2,
                AnoAnt1 = dAnoAnt1,
                AnoOrc = dAnoOrc,
                AnoReal = dAnoReal
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult depreciacaoLinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (depreciacaoLinha != null && depreciacaoLinha.TIPO != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + depreciacaoLinha.TIPO.Trim().ToUpper().Replace("DEPRECIAÇÃO", "").Substring(0, ((depreciacaoLinha.TIPO.Trim().Replace("DEPRECIAÇÃO", "").Length > 25) ? 25 : depreciacaoLinha.TIPO.Replace("DEPRECIAÇÃO", "").Trim().Length)) + "</font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(depreciacaoLinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(depreciacaoLinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(depreciacaoLinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(depreciacaoLinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(depreciacaoLinha.AnoReal, depreciacaoLinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(depreciacaoLinha.AnoReal, depreciacaoLinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(depreciacaoLinha.AnoReal, depreciacaoLinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((depreciacaoLinha.AnoReal - depreciacaoLinha.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult eventual = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (eventual != null && eventual.GRUPO != "")
                    {
                        Literal _litEventual = e.Row.FindControl("litEventual") as Literal;
                        if (_litEventual != null)
                            _litEventual.Text = "<font size='2' face='Calibri'>&nbsp;" + eventual.GRUPO.Trim() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(eventual.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(eventual.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(eventual.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(eventual.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(eventual.AnoReal, eventual.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(eventual.AnoReal, eventual.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(eventual.AnoReal, eventual.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((eventual.AnoReal - eventual.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(eventual.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(eventual.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(eventual.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(eventual.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((eventual.AnoReal - eventual.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            TIPO = k.Key.TIPO.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            gListaLucroBruto.Add(new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "LUCRO BRUTO",
                LINHA = "",
                AnoAnt2 = dAnoAnt2,
                AnoAnt1 = dAnoAnt1,
                AnoOrc = dAnoOrc,
                AnoReal = dAnoReal
            });

            //adicionar total ebtida
            gListaLucroBruto.AddRange(gListaEBITDA.Where(g => g.GRUPO == "EBITDA"));

            // SOMAR LUCRO BRUTO
            gListaLucroBruto = gListaLucroBruto.GroupBy(p => new { ID = p.ID }).Select(k => new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "LUCRO BRUTO",
                LINHA = "AGRUPADO",
                AnoAnt2 = k.Sum(g => g.AnoAnt2),
                AnoAnt1 = k.Sum(g => g.AnoAnt1),
                AnoOrc = k.Sum(g => g.AnoOrc),
                AnoReal = k.Sum(g => g.AnoReal)
            }).ToList();

            gListaLucroBruto.Insert(gListaLucroBruto.Count, new SP_DRE_COMPARATIVOResult { ID = 99, GRUPO = "" });

            //Carregar lucro bruto
            gvLucroBruto.DataSource = gListaLucroBruto;
            gvLucroBruto.DataBind();

        }

        protected void gvDespesaEventualItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult eventualLinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (eventualLinha != null && eventualLinha.TIPO != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + eventualLinha.TIPO.Trim().ToUpper() + "</font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(eventualLinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(eventualLinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(eventualLinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(eventualLinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(eventualLinha.AnoReal, eventualLinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(eventualLinha.AnoReal, eventualLinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(eventualLinha.AnoReal, eventualLinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((eventualLinha.AnoReal - eventualLinha.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult lucrobruto = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (lucrobruto != null && lucrobruto.GRUPO != "")
                    {
                        Literal _litLucroBruto = e.Row.FindControl("litLucroBruto") as Literal;
                        if (_litLucroBruto != null)
                            _litLucroBruto.Text = "<font size='2' face='Calibri'>&nbsp;" + lucrobruto.GRUPO.Trim() + "</font>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(lucrobruto.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(lucrobruto.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(lucrobruto.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(lucrobruto.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(lucrobruto.AnoReal, lucrobruto.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(lucrobruto.AnoReal, lucrobruto.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(lucrobruto.AnoReal, lucrobruto.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((lucrobruto.AnoReal - lucrobruto.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(lucrobruto.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(lucrobruto.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(lucrobruto.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(lucrobruto.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((lucrobruto.AnoReal - lucrobruto.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult receitadesp = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (receitadesp != null && receitadesp.GRUPO != "")
                    {
                        Literal _litReceitaDespesa = e.Row.FindControl("litReceitaDespesa") as Literal;
                        if (_litReceitaDespesa != null)
                            _litReceitaDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + receitadesp.GRUPO.Trim() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(receitadesp.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(receitadesp.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(receitadesp.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(receitadesp.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(receitadesp.AnoReal, receitadesp.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(receitadesp.AnoReal, receitadesp.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(receitadesp.AnoReal, receitadesp.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((receitadesp.AnoReal - receitadesp.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(receitadesp.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(receitadesp.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(receitadesp.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(receitadesp.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((receitadesp.AnoReal - receitadesp.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            TIPO = k.Key.TIPO.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            gListaLAIR.Add(new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "LAIR",
                LINHA = "",
                AnoAnt2 = dAnoAnt2,
                AnoAnt1 = dAnoAnt1,
                AnoOrc = dAnoOrc,
                AnoReal = dAnoReal
            });

            //adicionar total lucro bruto
            gListaLAIR.AddRange(gListaLucroBruto.Where(i => i.ID == 99));

            // SOMAR LUCRO BRUTO
            gListaLAIR = gListaLAIR.GroupBy(p => new { ID = p.ID }).Select(k => new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "LAIR",
                LINHA = "AGRUPADO",
                AnoAnt2 = k.Sum(g => g.AnoAnt2),
                AnoAnt1 = k.Sum(g => g.AnoAnt1),
                AnoOrc = k.Sum(g => g.AnoOrc),
                AnoReal = k.Sum(g => g.AnoReal)
            }).ToList();

            gListaLAIR.Insert(gListaLAIR.Count, new SP_DRE_COMPARATIVOResult { ID = 99, GRUPO = "" });

            //Carregar lucro bruto
            gvLAIR.DataSource = gListaLAIR;
            gvLAIR.DataBind();

        }

        protected void gvReceitaDespesaFinItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult receitadespesaLinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (receitadespesaLinha != null && receitadespesaLinha.TIPO != "")
                    {

                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + receitadespesaLinha.TIPO.Trim().ToUpper().Substring(0, ((receitadespesaLinha.TIPO.Trim().Length > 26) ? 26 : receitadespesaLinha.TIPO.Trim().Length)) + "</font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(receitadespesaLinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(receitadespesaLinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(receitadespesaLinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(receitadespesaLinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(receitadespesaLinha.AnoReal, receitadespesaLinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(receitadespesaLinha.AnoReal, receitadespesaLinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(receitadespesaLinha.AnoReal, receitadespesaLinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((receitadespesaLinha.AnoReal - receitadespesaLinha.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult lair = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (lair != null && lair.GRUPO != "")
                    {
                        Literal _litLAIR = e.Row.FindControl("litLAIR") as Literal;
                        if (_litLAIR != null)
                            _litLAIR.Text = "<font size='2' face='Calibri'>&nbsp;" + lair.GRUPO.Trim() + "</font>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(lair.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(lair.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(lair.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(lair.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(lair.AnoReal, lair.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(lair.AnoReal, lair.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(lair.AnoReal, lair.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((lair.AnoReal - lair.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(lair.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(lair.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(lair.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(lair.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((lair.AnoReal - lair.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult impostotaxa = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (impostotaxa != null && impostotaxa.GRUPO != "")
                    {
                        Literal _litImpostoTaxa = e.Row.FindControl("litImpostoTaxa") as Literal;
                        if (_litImpostoTaxa != null)
                            _litImpostoTaxa.Text = "<font size='2' face='Calibri'>&nbsp;" + impostotaxa.GRUPO.Trim() + "</font> ";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(impostotaxa.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(impostotaxa.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(impostotaxa.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(impostotaxa.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(impostotaxa.AnoReal, impostotaxa.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(impostotaxa.AnoReal, impostotaxa.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(impostotaxa.AnoReal, impostotaxa.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((impostotaxa.AnoReal - impostotaxa.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(impostotaxa.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(impostotaxa.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(impostotaxa.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(impostotaxa.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((impostotaxa.AnoReal - impostotaxa.AnoOrc)));

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
                                                                        k => new SP_DRE_COMPARATIVOResult
                                                                        {
                                                                            ID = 1,
                                                                            LINHA = k.Key.LINHA.Trim().ToUpper(),
                                                                            AnoAnt2 = k.Sum(j => j.AnoAnt2),
                                                                            AnoAnt1 = k.Sum(j => j.AnoAnt1),
                                                                            AnoOrc = k.Sum(j => j.AnoOrc),
                                                                            AnoReal = k.Sum(j => j.AnoReal)
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
            gListaResultado.Add(new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "RESULTADO",
                LINHA = "",
                AnoAnt2 = dAnoAnt2,
                AnoAnt1 = dAnoAnt1,
                AnoOrc = dAnoOrc,
                AnoReal = dAnoReal
            });

            //adicionar LAIR
            gListaResultado.AddRange(gListaLAIR.Where(i => i.ID == 99));

            // SOMAR RESULTADO
            gListaResultado = gListaResultado.GroupBy(p => new { ID = p.ID }).Select(k => new SP_DRE_COMPARATIVOResult
            {
                ID = 99,
                GRUPO = "RESULTADO",
                LINHA = "AGRUPADO",
                AnoAnt2 = k.Sum(g => g.AnoAnt2),
                AnoAnt1 = k.Sum(g => g.AnoAnt1),
                AnoOrc = k.Sum(g => g.AnoOrc),
                AnoReal = k.Sum(g => g.AnoReal)
            }).ToList();

            gListaResultado.Insert(gListaResultado.Count, new SP_DRE_COMPARATIVOResult { ID = 99, GRUPO = "" });

            //Carregar Resultado
            gvResultado.DataSource = gListaResultado;
            gvResultado.DataBind();
        }

        protected void gvImpostoTaxaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_COMPARATIVOResult impostotaxaLinha = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (impostotaxaLinha != null && impostotaxaLinha.LINHA != "")
                    {
                        Literal _litLinha = e.Row.FindControl("litLinha") as Literal;
                        if (_litLinha != null)
                            _litLinha.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;" + impostotaxaLinha.LINHA.Trim().ToUpper() + "</font></span>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(impostotaxaLinha.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(impostotaxaLinha.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(impostotaxaLinha.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(impostotaxaLinha.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(impostotaxaLinha.AnoReal, impostotaxaLinha.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(impostotaxaLinha.AnoReal, impostotaxaLinha.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(impostotaxaLinha.AnoReal, impostotaxaLinha.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((impostotaxaLinha.AnoReal - impostotaxaLinha.AnoOrc)));

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
                    SP_DRE_COMPARATIVOResult resultado = e.Row.DataItem as SP_DRE_COMPARATIVOResult;

                    if (resultado != null && resultado.GRUPO != "")
                    {
                        Literal _litResultado = e.Row.FindControl("litResultado") as Literal;
                        if (_litResultado != null)
                            _litResultado.Text = "<font size='2' face='Calibri'>&nbsp;" + resultado.GRUPO.Trim() + "</font>";

                        Literal _litAnoAnt2 = e.Row.FindControl("litAnoAnt2") as Literal;
                        if (_litAnoAnt2 != null)
                            _litAnoAnt2.Text = FormatarValor(FiltroValor(resultado.AnoAnt2));

                        Literal _litAnoAnt1 = e.Row.FindControl("litAnoAnt1") as Literal;
                        if (_litAnoAnt1 != null)
                            _litAnoAnt1.Text = FormatarValor(FiltroValor(resultado.AnoAnt1));

                        Literal _litAnoOrc = e.Row.FindControl("litAnoOrc") as Literal;
                        if (_litAnoOrc != null)
                            _litAnoOrc.Text = FormatarValor(FiltroValor(resultado.AnoOrc));

                        Literal _litAnoReal = e.Row.FindControl("litAnoReal") as Literal;
                        if (_litAnoReal != null)
                            _litAnoReal.Text = FormatarValor(FiltroValor(resultado.AnoReal));

                        Literal _litAnoAnt2Porc = e.Row.FindControl("litAnoAnt2Porc") as Literal;
                        if (_litAnoAnt2Porc != null)
                            _litAnoAnt2Porc.Text = CalculaPorc(resultado.AnoReal, resultado.AnoAnt2, 1, true);

                        Literal _litAnoAnt1Porc = e.Row.FindControl("litAnoAnt1Porc") as Literal;
                        if (_litAnoAnt1Porc != null)
                            _litAnoAnt1Porc.Text = CalculaPorc(resultado.AnoReal, resultado.AnoAnt1, 1, true);

                        Literal _litAnoOrcPorc = e.Row.FindControl("litAnoOrcPorc") as Literal;
                        if (_litAnoOrcPorc != null)
                            _litAnoOrcPorc.Text = CalculaPorc(resultado.AnoReal, resultado.AnoOrc, 1, true);

                        Literal _litAtingidoValor = e.Row.FindControl("litAtingidoValor") as Literal;
                        if (_litAtingidoValor != null)
                            _litAtingidoValor.Text = FormatarValor(FiltroValor((resultado.AnoReal - resultado.AnoOrc)));

                        //SOMATORIO
                        dAnoAnt2 += Convert.ToDecimal(FiltroValor(resultado.AnoAnt2));
                        dAnoAnt1 += Convert.ToDecimal(FiltroValor(resultado.AnoAnt1));
                        dAnoOrc += Convert.ToDecimal(FiltroValor(resultado.AnoOrc));
                        dAnoReal += Convert.ToDecimal(FiltroValor(resultado.AnoReal));
                        dAtingidoValor += Convert.ToDecimal(FiltroValor((resultado.AnoReal - resultado.AnoOrc)));

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

    }
}
