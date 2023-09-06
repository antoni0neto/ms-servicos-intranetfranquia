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
    public partial class dre_dre_v10_loja : System.Web.UI.Page
    {

        DREController dreController = new DREController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        Color corTitulo = System.Drawing.Color.Gainsboro;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";
        string DRE_500_LOJA = "DRE_500_LOJA";

        #region "DEC GLOBAIS"

        string link = "";
        string ano = "";
        string tipo = "";
        string unidadeNegocio = "";
        string codigoFilial = "";

        string gCodigoFilial = "";
        List<SP_OBTER_DRE500_LOJAResult> gDRE = new List<SP_OBTER_DRE500_LOJAResult>();

        List<SP_OBTER_DRE500_LOJAResult> gListaLoja = new List<SP_OBTER_DRE500_LOJAResult>();

        List<SP_OBTER_DRE500_LOJAResult> gListaReceitaLiquida = new List<SP_OBTER_DRE500_LOJAResult>();
        List<SP_OBTER_DRE500_LOJAResult> gListaTaxaCartao = new List<SP_OBTER_DRE500_LOJAResult>();
        List<SP_OBTER_DRE500_LOJAResult> gListaImposto = new List<SP_OBTER_DRE500_LOJAResult>();

        List<SP_OBTER_DRE500_LOJAResult> gListaCmvLoja = new List<SP_OBTER_DRE500_LOJAResult>();

        List<SP_OBTER_DRE500_LOJAResult> gListaMargemContribuicao = new List<SP_OBTER_DRE500_LOJAResult>();
        List<SP_OBTER_DRE500_LOJAResult> gListaMargemContribuicaoLoja = new List<SP_OBTER_DRE500_LOJAResult>();

        List<SP_OBTER_DRE500_LOJAResult> gListaDespesaADM = new List<SP_OBTER_DRE500_LOJAResult>();

        List<SP_OBTER_DRE500_LOJAResult> gListaEBITDA = new List<SP_OBTER_DRE500_LOJAResult>();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
                CarregarFilial("LOJA");
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

            labErro.Text = "";

            ano = ddlAno.SelectedValue;
            dataIni = Convert.ToDateTime(ano + "-01-01");
            dataFim = Convert.ToDateTime(ano + "-12-31");
            mesIni = Convert.ToInt32(ddlMesDe.SelectedValue);
            mesFim = Convert.ToInt32(ddlMesAte.SelectedValue);


            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                gCodigoFilial = ddlFilial.SelectedValue;

            if (ddlTipo.SelectedValue == "")
            {
                labErro.Text = "Selecione o Tipo de DRE.";
                return;
            }

            try
            {

                labErro.Text = "";
                btExcel.Enabled = false;

                Session[DRE_500_LOJA] = null;
                gDRE = dreController.ObterDRE500Loja(dataIni, dataFim, ddlTipo.SelectedValue, ddlUnidadeNegocio.SelectedValue, gCodigoFilial, mesIni, mesFim);
                Session[DRE_500_LOJA] = gDRE.OrderBy(p => p.ID).ThenBy(p => p.GRUPO).ThenBy(p => p.LINHA).ThenBy(p => p.TIPO).ThenBy(p => p.FILIAL).ToList();
                gvDREExcel.Visible = true;
                gvDREExcel.DataSource = Session[DRE_500_LOJA] as List<SP_OBTER_DRE500_LOJAResult>;
                gvDREExcel.DataBind();
                gvDREExcel.Visible = false;

                CarregarReceitaLiquida();
                CarregarCMV();
                CarregarMargemContribuicao();
                CarregarDespesaADM();
                CarregarEBTIDA();

                btExcel.Enabled = true;

                #region "Fechamento Cor"

                foreach (TemplateField c in gvReceitaLiquida.Columns)
                {
                    int mes = 0;

                    if (c.HeaderText == "Janeiro")
                        mes = 1;
                    else if (c.HeaderText == "Fevereiro")
                        mes = 2;
                    else if (c.HeaderText == "Março")
                        mes = 3;
                    else if (c.HeaderText == "Abril")
                        mes = 4;
                    else if (c.HeaderText == "Maio")
                        mes = 5;
                    else if (c.HeaderText == "Junho")
                        mes = 6;
                    else if (c.HeaderText == "Julho")
                        mes = 7;
                    else if (c.HeaderText == "Agosto")
                        mes = 8;
                    else if (c.HeaderText == "Setembro")
                        mes = 9;
                    else if (c.HeaderText == "Outubro")
                        mes = 10;
                    else if (c.HeaderText == "Novembro")
                        mes = 11;
                    else if (c.HeaderText == "Dezembro")
                        mes = 12;

                    c.HeaderStyle.BackColor = Color.Gainsboro;
                    var drefechamento = dreController.ObterDREFechamentoPeriodo(Convert.ToInt32(ano), mes, ddlTipo.SelectedValue);
                    if (drefechamento != null)
                        c.HeaderStyle.BackColor = Color.Gold;

                }
                #endregion

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("deadlocked"))
                    labErro.Text = "Deadlock: Realize a consulta novamente.";
                else
                    labErro.Text = ex.Message + "\n" + ex.StackTrace;
            }

        }

        #region "DADOS INICIAIS"
        protected void ddlUnidadeNegocio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFilial(ddlUnidadeNegocio.SelectedValue);
        }
        private void CarregarFilial(string unidadeNegocio = "")
        {
            var filiais = baseController.ObterFiliaisDRE(unidadeNegocio);

            if (unidadeNegocio == "ATACADO")
                filiais = filiais.Where(p => p.MOSTRAR_ATACADO == true).ToList();
            else if (unidadeNegocio == "ECOMMERCE")
                filiais = filiais.Where(p => p.MOSTRAR_ECOMMERCE == true).ToList();
            else if (unidadeNegocio == "FABRICA")
                filiais = filiais.Where(p => p.MOSTRAR_FABRICA == true).ToList();
            else if (unidadeNegocio == "LOJA")
                filiais = filiais.Where(p => p.MOSTRAR_VAREJO == true).ToList();

            if (filiais != null)
            {
                filiais.Insert(0, new DRE_FILIAL { CODIGO_FILIAL = "", FILIAL = "", UNIDADE_NEGOCIO = "", ATIVO = true });
                ddlFilial.DataSource = filiais;
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
            // 0 - Faturamento Bruto Total
            // 1 - Faturamento Bruto Varejo
            // 2 - Faturamento Bruto Atacado

            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            decimal fatBrutoJaneiro = FiltroValor(gListaLoja.Sum(p => p.JANEIRO), 1);
            decimal fatBrutoFevereiro = FiltroValor(gListaLoja.Sum(p => p.FEVEREIRO), 2);
            decimal fatBrutoMarco = FiltroValor(gListaLoja.Sum(p => p.MARCO), 3);
            decimal fatBrutoAbril = FiltroValor(gListaLoja.Sum(p => p.ABRIL), 4);
            decimal fatBrutoMaio = FiltroValor(gListaLoja.Sum(p => p.MAIO), 5);
            decimal fatBrutoJunho = FiltroValor(gListaLoja.Sum(p => p.JUNHO), 6);
            decimal fatBrutoJulho = FiltroValor(gListaLoja.Sum(p => p.JULHO), 7);
            decimal fatBrutoAgosto = FiltroValor(gListaLoja.Sum(p => p.AGOSTO), 8);
            decimal fatBrutoSetembro = FiltroValor(gListaLoja.Sum(p => p.SETEMBRO), 9);
            decimal fatBrutoOutubro = FiltroValor(gListaLoja.Sum(p => p.OUTUBRO), 10);
            decimal fatBrutoNovembro = FiltroValor(gListaLoja.Sum(p => p.NOVEMBRO), 11);
            decimal fatBrutoDezembro = FiltroValor(gListaLoja.Sum(p => p.DEZEMBRO), 12);
            decimal fatBrutoTotal = FiltroValor(gListaLoja.Sum(p => p.TOTAL), 0);

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
                fatBrutoJaneiro = FiltroValor(gListaLoja.Sum(p => p.JANEIRO), 1);
                fatBrutoFevereiro = FiltroValor(gListaLoja.Sum(p => p.FEVEREIRO), 2);
                fatBrutoMarco = FiltroValor(gListaLoja.Sum(p => p.MARCO), 3);
                fatBrutoAbril = FiltroValor(gListaLoja.Sum(p => p.ABRIL), 4);
                fatBrutoMaio = FiltroValor(gListaLoja.Sum(p => p.MAIO), 5);
                fatBrutoJunho = FiltroValor(gListaLoja.Sum(p => p.JUNHO), 6);
                fatBrutoJulho = FiltroValor(gListaLoja.Sum(p => p.JULHO), 7);
                fatBrutoAgosto = FiltroValor(gListaLoja.Sum(p => p.AGOSTO), 8);
                fatBrutoSetembro = FiltroValor(gListaLoja.Sum(p => p.SETEMBRO), 9);
                fatBrutoOutubro = FiltroValor(gListaLoja.Sum(p => p.OUTUBRO), 10);
                fatBrutoNovembro = FiltroValor(gListaLoja.Sum(p => p.NOVEMBRO), 11);
                fatBrutoDezembro = FiltroValor(gListaLoja.Sum(p => p.DEZEMBRO), 12);
                fatBrutoTotal = FiltroValor(gListaLoja.Sum(p => p.TOTAL), 0);
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

            gListaLoja = gDRE.Where(p => p.ID == 1).ToList();

            gListaImposto = gDRE.Where(p => p.ID == 33).ToList();

            var gListaLojaVarejo = gListaLoja.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DRE500_LOJAResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "RECEITAS BRUTAS",
                                                                                LINHA = "LOJA",
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

            var deducoesReceitaBruta = gListaImposto;
            deducoesReceitaBruta = deducoesReceitaBruta.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                k => new SP_OBTER_DRE500_LOJAResult
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

            var receitaFaturamento = gListaLojaVarejo.Union(deducoesReceitaBruta).ToList();

            receitaFaturamento.Insert(receitaFaturamento.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "", LINHA = "1" });

            gListaReceitaLiquida = gDRE.Where(p => p.ID == 5).ToList();
            var receitaLiquida = gListaReceitaLiquida.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                k => new SP_OBTER_DRE500_LOJAResult
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

            receitaFaturamento.Insert(receitaFaturamento.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "", LINHA = "2" });

            if (receitaFaturamento != null)
            {
                gvReceitaLiquida.DataSource = receitaFaturamento;
                gvReceitaLiquida.DataBind();
            }
        }
        private void CarregarCMV()
        {
            gListaCmvLoja = gDRE.Where(p => p.ID == 16).ToList();

            var cmv = gListaCmvLoja;
            cmv = cmv.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DRE500_LOJAResult
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

            cmv.Insert(cmv.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "" });
            cmv.Insert(cmv.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "CMV" });
            cmv.Insert(cmv.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "" });
            if (cmv != null)
            {
                gvCMV.DataSource = cmv;
                gvCMV.DataBind();
            }
        }
        private void CarregarMargemContribuicao()
        {
            gListaMargemContribuicaoLoja = gDRE.Where(p => p.ID == 8).ToList();

            gListaMargemContribuicao = gListaMargemContribuicaoLoja;

            var margemContribuicao = gListaMargemContribuicao.GroupBy(p => new { GRUPO = p.GRUPO }).Select(k => new SP_OBTER_DRE500_LOJAResult
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


            margemContribuicao.Insert(margemContribuicao.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "" });

            //Carregar margem de contribuicao
            gvMargem.DataSource = margemContribuicao;
            gvMargem.DataBind();
        }
        private void CarregarDespesaADM()
        {
            gListaDespesaADM = gDRE.Where(p => p.ID == 15).ToList();

            var despADMAgrupado = gListaDespesaADM.GroupBy(b => new { GRUPO = b.GRUPO }).Select(
                                                                            k => new SP_OBTER_DRE500_LOJAResult
                                                                            {
                                                                                ID = 1,
                                                                                GRUPO = "DESPESAS",
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

            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "", LINHA = "" });
            despADMAgrupado.Insert(despADMAgrupado.Count, new SP_OBTER_DRE500_LOJAResult { GRUPO = "", LINHA = "" });

            if (despADMAgrupado != null)
            {
                gvDespesaAdm.DataSource = despADMAgrupado;
                gvDespesaAdm.DataBind();
            }
        }
        private void CarregarEBTIDA()
        {
            gListaEBITDA = gDRE.Where(p => p.ID == 17).ToList();

            gListaEBITDA.Insert(gListaEBITDA.Count, new SP_OBTER_DRE500_LOJAResult { ID = 97, GRUPO = "" });

            //Carregar gvEbtida
            gvEbtida.DataSource = gListaEBITDA;
            gvEbtida.DataBind();
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
                    SP_OBTER_DRE500_LOJAResult receitaLiquida = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (receitaLiquida != null && receitaLiquida.GRUPO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + receitaLiquida.GRUPO + "&l=" + receitaLiquida.LINHA + "&le=" + receitaLiquida.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;

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
                    {
                        e.Row.Height = Unit.Pixel(9);
                        _litHelp.Text = "";
                    }
                    else if (receitaLiquida.LINHA == "2")
                    {
                        e.Row.Height = Unit.Pixel(5);
                        _litHelp.Text = "";
                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (receitaLiquida.ID == 1)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                var faturamentoBruto = gListaLoja.ToList();
                                faturamentoBruto = faturamentoBruto.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DRE500_LOJAResult
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
                                gvFaturamento.DataSource = faturamentoBruto.OrderBy(p => p.LINHA);
                                gvFaturamento.DataBind();
                            }
                            img.Visible = true;
                        }
                        if (receitaLiquida.ID == 2)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                //var deducoesBruto = gListaImposto.Union(gListaTaxaCartao).ToList();
                                var deducoesBruto = gListaImposto.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DRE500_LOJAResult
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
                                gvFaturamento.DataSource = deducoesBruto.OrderBy(p => p.LINHA);
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
                    SP_OBTER_DRE500_LOJAResult faturamento = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (faturamento != null && faturamento.GRUPO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + faturamento.GRUPO + "&l=" + faturamento.LINHA + "&le=" + faturamento.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;

                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                        {
                            if (faturamento.LINHA.ToUpper().Contains("LOJA") || faturamento.LINHA.ToUpper().Contains("ECOMMERCE"))
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
                                                                        k => new SP_OBTER_DRE500_LOJAResult
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

                                gvSubGrid.DataSource = gListaImposto.OrderBy(p => p.LINHA);
                                gvSubGrid.DataBind();
                            }
                            img.Visible = true;
                        }

                        if (faturamento.LINHA.Trim().ToUpper() == "LOJA" || faturamento.LINHA.Trim().ToUpper() == "ECOMMERCE")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                gvSubGrid.DataSource = gListaLoja.Where(p => p.LINHA == faturamento.LINHA.Trim().ToUpper()).OrderBy(p => p.FILIAL);
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
                    SP_OBTER_DRE500_LOJAResult subitem = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {

                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.LINHA.ToUpper().Contains("LOJA") || subitem.LINHA.ToUpper().Contains("ECOMMERCE"))
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
                    SP_OBTER_DRE500_LOJAResult cmv = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (cmv != null && cmv.GRUPO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + cmv.GRUPO + "&l=" + cmv.LINHA + "&le=" + cmv.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;

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
                    {
                        e.Row.Height = Unit.Pixel(3);
                        _litHelp.Text = "";
                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cmv.ID == 1)
                        {
                            GridView gvCMVItem = e.Row.FindControl("gvCMVItem") as GridView;
                            if (gvCMVItem != null)
                            {
                                var cmvAgrupado = gListaCmvLoja.ToList();
                                cmvAgrupado = cmvAgrupado.GroupBy(b => new { LINHA = b.LINHA }).Select(
                                                                        k => new SP_OBTER_DRE500_LOJAResult
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
                                gvCMVItem.DataSource = cmvAgrupado.OrderBy(p => p.LINHA);
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
                    SP_OBTER_DRE500_LOJAResult cmvlinha = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (cmvlinha != null && cmvlinha.GRUPO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + cmvlinha.GRUPO + "&l=" + cmvlinha.LINHA + "&le=" + cmvlinha.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;

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
                        if (cmvlinha.LINHA.Trim().ToUpper() == "LOJA")
                        {
                            GridView gvCMVItemSub = e.Row.FindControl("gvCMVItemSub") as GridView;
                            if (gvCMVItemSub != null)
                            {
                                gvCMVItemSub.DataSource = gListaCmvLoja;
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
                    SP_OBTER_DRE500_LOJAResult subitem = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    if (subitem != null && subitem.GRUPO != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.LINHA.ToUpper().Contains("LOJA"))
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
                    SP_OBTER_DRE500_LOJAResult margem = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (margem != null && margem.GRUPO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + margem.GRUPO + "&l=" + margem.LINHA + "&le=" + margem.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;

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
                    {
                        e.Row.Height = Unit.Pixel(1);
                        _litHelp.Text = "";
                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (margem.GRUPO != "")
                        {
                            GridView gvMargemItem = e.Row.FindControl("gvMargemItem") as GridView;
                            if (gvMargemItem != null)
                            {
                                var margemContribuicao = gListaMargemContribuicao.GroupBy(p => new { LINHA = p.LINHA }).Select(k => new SP_OBTER_DRE500_LOJAResult
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


                                gvMargemItem.DataSource = margemContribuicao.OrderBy(p => p.LINHA);
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
                    SP_OBTER_DRE500_LOJAResult _margem = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (_margem != null && _margem.LINHA != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + _margem.GRUPO + "&l=" + _margem.LINHA + "&le=" + _margem.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;

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
                    SP_OBTER_DRE500_LOJAResult subitem = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

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

        #region "MARGEM BRUTA"

        //protected void gvMargemBruta_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.DataItem != null)
        //        {
        //            SP_OBTER_DRE500_LOJAResult margemBruta = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

        //            Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
        //            if (margemBruta != null && margemBruta.GRUPO != "")
        //            {
        //                //criar link para ajuda
        //                var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + margemBruta.GRUPO + "&l=" + margemBruta.LINHA + "&le=" + margemBruta.TIPO + "&tt=" + ddlTipo.SelectedValue;
        //                var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
        //                _litHelp.Text = linkOk;

        //                Literal _litMargemBruta = e.Row.FindControl("litMargemBruta") as Literal;
        //                if (_litMargemBruta != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _litMargemBruta.Text = "<span style='font-weight:normal'><font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;<i>" + margemBruta.LINHA + " " + margemBruta.GRUPO.Trim().ToUpper() + "</i></font></span>";
        //                    else
        //                        _litMargemBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + margemBruta.GRUPO.Trim().ToUpper() + "</font>";
        //                Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
        //                if (_janeiro != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _janeiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JANEIRO, 1)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _janeiro.Text = FormatarValor(FiltroValor(margemBruta.JANEIRO, 1)).Trim();

        //                Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
        //                if (_fevereiro != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _fevereiro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.FEVEREIRO, 2)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _fevereiro.Text = FormatarValor(FiltroValor(margemBruta.FEVEREIRO, 2)).Trim();

        //                Literal _marco = e.Row.FindControl("litMarco") as Literal;
        //                if (_marco != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _marco.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.MARCO, 3)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _marco.Text = FormatarValor(FiltroValor(margemBruta.MARCO, 3)).Trim();

        //                Literal _abril = e.Row.FindControl("litAbril") as Literal;
        //                if (_abril != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _abril.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.ABRIL, 4)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _abril.Text = FormatarValor(FiltroValor(margemBruta.ABRIL, 4)).Trim();

        //                Literal _maio = e.Row.FindControl("litMaio") as Literal;
        //                if (_maio != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _maio.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.MAIO, 5)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _maio.Text = FormatarValor(FiltroValor(margemBruta.MAIO, 5)).Trim();

        //                Literal _junho = e.Row.FindControl("litJunho") as Literal;
        //                if (_junho != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _junho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JUNHO, 6)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _junho.Text = FormatarValor(FiltroValor(margemBruta.JUNHO, 6)).Trim();

        //                Literal _julho = e.Row.FindControl("litJulho") as Literal;
        //                if (_julho != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _julho.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.JULHO, 7)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _julho.Text = FormatarValor(FiltroValor(margemBruta.JULHO, 7)).Trim();

        //                Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
        //                if (_agosto != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _agosto.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.AGOSTO, 8)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _agosto.Text = FormatarValor(FiltroValor(margemBruta.AGOSTO, 8)).Trim();

        //                Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
        //                if (_setembro != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _setembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.SETEMBRO, 9)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _setembro.Text = FormatarValor(FiltroValor(margemBruta.SETEMBRO, 9)).Trim();

        //                Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
        //                if (_outubro != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _outubro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.OUTUBRO, 10)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _outubro.Text = FormatarValor(FiltroValor(margemBruta.OUTUBRO, 10)).Trim();

        //                Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
        //                if (_novembro != null)
        //                    if (margemBruta.LINHA.Contains("%"))
        //                        _novembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.NOVEMBRO, 11)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _novembro.Text = FormatarValor(FiltroValor(margemBruta.NOVEMBRO, 11)).Trim();

        //                Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
        //                if (_dezembro != null)
        //                    if (margemBruta.LINHA.Contains("%") || (margemBruta.LINHA.Trim() == "MARK-UP"))
        //                        _dezembro.Text = "<span style='font-weight:normal'>" + FormatarValor(FiltroValor(margemBruta.DEZEMBRO, 12)).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _dezembro.Text = FormatarValor(FiltroValor(margemBruta.DEZEMBRO, 12)).Trim();

        //                Literal _total = e.Row.FindControl("litTotal") as Literal;
        //                if (_total != null)
        //                    if (margemBruta.LINHA.Contains("%") || (margemBruta.LINHA.Trim() == "MARK-UP"))
        //                        _total.Text = "<span style='font-weight:normal'>" + FormatarValor(margemBruta.TOTAL).Trim() + ((margemBruta.LINHA.Contains("%") ? "<font size='2' face='Calibri'>%</font>" : "")) + "</span>";
        //                    else
        //                        _total.Text = FormatarValor(margemBruta.TOTAL).Trim();


        //            }

        //            if (margemBruta.GRUPO == "")
        //            {
        //                e.Row.Height = Unit.Pixel(1);
        //                _litHelp.Text = "";
        //            }

        //        }
        //    }
        //}
        //protected void gvMargemBruta_DataBound(object sender, EventArgs e)
        //{
        //    if (gvReceitaLiquida.Rows.Count >= 5)
        //    {
        //        //Tratamento de cores para linhas
        //        GridViewRow firstBlankRow = gvMargemBruta.Rows[gvMargemBruta.Rows.Count - 3];
        //        if (firstBlankRow != null)
        //            firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";

        //        GridViewRow lastBlankRow = gvMargemBruta.Rows[gvMargemBruta.Rows.Count - 1];
        //        if (lastBlankRow != null)
        //        {
        //            //Tratamento para cor
        //            lastBlankRow.BackColor = corTitulo;
        //            int count = gvMargemBruta.Columns.Count;
        //            for (int i = 0; i < count; i++)
        //                lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);

        //            lastBlankRow.Attributes["style"] = "line-height: 2px;";
        //        }

        //        GridViewRow headerRow = gvMargemBruta.HeaderRow;
        //        if (headerRow != null)
        //            headerRow.Attributes["style"] = "line-height: 3px;";
        //    }
        //}

        #endregion

        #region "DESPESAS ADMINISTRATIVAS"

        protected void gvDespesaAdm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DRE500_LOJAResult despadm = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (despadm != null && despadm.GRUPO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + despadm.GRUPO + "&l=" + despadm.LINHA + "&le=" + despadm.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;

                        Literal _litDespesa = e.Row.FindControl("litDespesa") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despadm.GRUPO.Trim().ToUpper() + "</font> ";

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
                    {
                        e.Row.Height = Unit.Pixel(3);
                        _litHelp.Text = "";
                    }

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
                                despAdmPessoal = despAdmPessoal.GroupBy(b => new { DESC_CENTRO_CUSTO = b.DESC_CENTRO_CUSTO.Trim(), ORDEM = b.ORDEM }).Select(
                                                                        k => new SP_OBTER_DRE500_LOJAResult
                                                                        {
                                                                            ID = 1,
                                                                            DESC_CENTRO_CUSTO = k.Key.DESC_CENTRO_CUSTO.Trim().ToUpper(),
                                                                            ORDEM = k.Key.ORDEM,
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
                                gvDespesaAdmItem.DataSource = despAdmPessoal.OrderBy(p => p.ORDEM);
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
                    SP_OBTER_DRE500_LOJAResult despadmlinha = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (despadmlinha != null && despadmlinha.DESC_CENTRO_CUSTO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + despadmlinha.GRUPO + "&l=" + despadmlinha.LINHA + "&le=" + despadmlinha.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;


                        Literal _litDespesa = e.Row.FindControl("litDespesaItem") as Literal;
                        if (_litDespesa != null)
                            _litDespesa.Text = "<font size='2' face='Calibri'>&nbsp;" + despadmlinha.DESC_CENTRO_CUSTO.Trim().ToUpper().Substring(0, ((despadmlinha.DESC_CENTRO_CUSTO.Trim().Length > 20) ? 20 : despadmlinha.DESC_CENTRO_CUSTO.Trim().Length)) + "</font> ";

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
                                var lstdespAdmLinha = gListaDespesaADM.Where(p => p.DESC_CENTRO_CUSTO.Trim().ToUpper() == despadmlinha.DESC_CENTRO_CUSTO.Trim().ToUpper()).GroupBy(j => new
                                {
                                    DESC_CENTRO_CUSTO = j.DESC_CENTRO_CUSTO,
                                    LINHA = j.LINHA
                                }).Select(k => new SP_OBTER_DRE500_LOJAResult
                                {
                                    ID = 1,
                                    DESC_CENTRO_CUSTO = k.Key.DESC_CENTRO_CUSTO.Trim().ToUpper(),
                                    LINHA = k.Key.LINHA.Trim().ToUpper(),
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

                                gvDespesaAdmItemSub.DataSource = lstdespAdmLinha.OrderBy(p => p.LINHA).ThenBy(x => x.DESC_CONTA_CONTABIL);
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
                    SP_OBTER_DRE500_LOJAResult subitem = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    if (subitem != null && (subitem.LINHA != "" || subitem.DESC_CENTRO_CUSTO.Trim().ToUpper().Contains("DIRETORIA COMERCIAL") || subitem.DESC_CENTRO_CUSTO.Trim().ToUpper().Contains("VM - VISUAL MERCHANDISING") || subitem.DESC_CENTRO_CUSTO.Trim().ToUpper().Contains("EXPANSAO E OBRAS")))
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + ((subitem.LINHA == null || subitem.LINHA == "") ? subitem.DESC_CENTRO_CUSTO.Trim().ToUpper() : subitem.LINHA.Trim().ToUpper().Substring(0, ((subitem.LINHA.Trim().Length > 21) ? 21 : subitem.LINHA.Trim().Length))) + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        _janeiro.Text = FormatarValor(FiltroValor(subitem.JANEIRO, 1));

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        _fevereiro.Text = FormatarValor(FiltroValor(subitem.FEVEREIRO, 2));

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        _marco.Text = FormatarValor(FiltroValor(subitem.MARCO, 3));

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        _abril.Text = FormatarValor(FiltroValor(subitem.ABRIL, 4));

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        _maio.Text = FormatarValor(FiltroValor(subitem.MAIO, 5));

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        _junho.Text = FormatarValor(FiltroValor(subitem.JUNHO, 6));

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        _julho.Text = FormatarValor(FiltroValor(subitem.JULHO, 7));

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        _agosto.Text = FormatarValor(FiltroValor(subitem.AGOSTO, 8));

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        _setembro.Text = FormatarValor(FiltroValor(subitem.SETEMBRO, 9));

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        _outubro.Text = FormatarValor(FiltroValor(subitem.OUTUBRO, 10));

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        _novembro.Text = FormatarValor(FiltroValor(subitem.NOVEMBRO, 11));

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        _dezembro.Text = FormatarValor(FiltroValor(subitem.DEZEMBRO, 12));

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
                            var lstdespAdmSub = gListaDespesaADM.Where(g => g.LINHA.Trim().ToUpper() == subitem.LINHA.Trim().ToUpper() && g.DESC_CENTRO_CUSTO.ToUpper() == subitem.DESC_CENTRO_CUSTO).GroupBy(j => new
                            {
                                COD_CENTRO_CUSTO = j.COD_CENTRO_CUSTO.Trim().ToUpper(),
                                DESC_CENTRO_CUSTO = j.DESC_CENTRO_CUSTO.Trim().ToUpper(),
                                CONTA_CONTABIL = j.CONTA_CONTABIL.Trim().ToUpper(),
                                DESC_CONTA_CONTABIL = j.DESC_CONTA_CONTABIL.Trim().ToUpper()
                            }).Select(k => new SP_OBTER_DRE500_LOJAResult
                            {
                                ID = 1,
                                COD_CENTRO_CUSTO = k.Key.COD_CENTRO_CUSTO.Trim().ToUpper(),
                                DESC_CENTRO_CUSTO = k.Key.DESC_CENTRO_CUSTO.Trim().ToUpper(),
                                CONTA_CONTABIL = k.Key.CONTA_CONTABIL.Trim().ToUpper(),
                                DESC_CONTA_CONTABIL = k.Key.DESC_CONTA_CONTABIL.Trim().ToUpper(),
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

                            gvDespesaAdmItemSubSub.DataSource = lstdespAdmSub.OrderBy(p => p.DESC_CONTA_CONTABIL);
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
                    SP_OBTER_DRE500_LOJAResult subitem = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    if (subitem != null && (subitem.LINHA != "" || subitem.DESC_CENTRO_CUSTO.Trim().ToUpper() == "DIRETORIA COMERCIAL" || subitem.DESC_CENTRO_CUSTO.Trim().ToUpper() == "VM - VISUAL MERCHANDISING" || subitem.DESC_CENTRO_CUSTO.Trim().ToUpper() == "EXPANSAO E OBRAS"))
                    {

                        var rateio = subitem.DESC_CENTRO_CUSTO.Trim().Substring(0, 2);
                        if (rateio.ToUpper() == "R-")
                            rateio = "R";
                        else rateio = "";

                        Literal _litSubItem = e.Row.FindControl("litSubSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;&nbsp;" + ((subitem.DESC_CONTA_CONTABIL == null || subitem.DESC_CONTA_CONTABIL == "") ? subitem.DESC_CENTRO_CUSTO.Trim().ToUpper() : subitem.DESC_CONTA_CONTABIL.Trim().ToUpper().Substring(0, ((subitem.DESC_CONTA_CONTABIL.Trim().Length > 20) ? 20 : subitem.DESC_CONTA_CONTABIL.Trim().Length))) + "</font>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        _janeiro.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(1, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Janeiro'>" + FormatarValor(FiltroValor(subitem.JANEIRO, 1)) + "</a>";

                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        _fevereiro.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(2, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Fevereiro'>" + FormatarValor(FiltroValor(subitem.FEVEREIRO, 2)) + "</a>";

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        _marco.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(3, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Marco'>" + FormatarValor(FiltroValor(subitem.MARCO, 3)) + "</a>";

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        _abril.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(4, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Abril'>" + FormatarValor(FiltroValor(subitem.ABRIL, 4)) + "</a>";

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        _maio.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(5, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Maio'>" + FormatarValor(FiltroValor(subitem.MAIO, 5)) + "</a>";

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        _junho.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(6, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Junho'>" + FormatarValor(FiltroValor(subitem.JUNHO, 6)) + "</a>";

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        _julho.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(7, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Julho'>" + FormatarValor(FiltroValor(subitem.JULHO, 7)) + "</a>";

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        _agosto.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(8, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Agosto'>" + FormatarValor(FiltroValor(subitem.AGOSTO, 8)) + "</a>";

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        _setembro.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(9, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Setembro'>" + FormatarValor(FiltroValor(subitem.SETEMBRO, 9)) + "</a>";

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        _outubro.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(10, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Outubro'>" + FormatarValor(FiltroValor(subitem.OUTUBRO, 10)) + "</a>";

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        _novembro.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(11, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Novembro'>" + FormatarValor(FiltroValor(subitem.NOVEMBRO, 11)) + "</a>";

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        _dezembro.Text = "<a href=\"javascript: openwindow('" + CriarLinkLancamento(12, subitem.COD_CENTRO_CUSTO, subitem.CONTA_CONTABIL, rateio) + "')\" class='adre' title='Dezembro'>" + FormatarValor(FiltroValor(subitem.DEZEMBRO, 12)) + "</a>";

                        Literal _total = e.Row.FindControl("litTotal") as Literal;
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
                    SP_OBTER_DRE500_LOJAResult ebtida = e.Row.DataItem as SP_OBTER_DRE500_LOJAResult;

                    Literal _litHelp = e.Row.FindControl("litHelp") as Literal;
                    if (ebtida != null && ebtida.GRUPO != "")
                    {
                        //criar link para ajuda
                        var link = "dre_ajuda.aspx?id=" + _litHelp.Text + "&g=" + ebtida.GRUPO + "&l=" + ebtida.LINHA + "&le=" + ebtida.TIPO + "&tt=" + ddlTipo.SelectedValue;
                        var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/help.png' width='10px' /></a>";
                        _litHelp.Text = linkOk;


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
                    {
                        e.Row.Height = Unit.Pixel(1);
                        _litHelp.Text = "";
                    }

                }
            }
        }
        protected void gvEbtida_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            if (gvEbtida.Rows.Count >= 3)
            {
                GridViewRow firstBlankRow = gvEbtida.Rows[gvEbtida.Rows.Count - 3];
                if (firstBlankRow != null)
                    firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";
            }

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

        private string CriarLinkLancamento(int mes, string codCentroCusto, string contaContabil, string rateio)
        {
            //links
            ano = ddlAno.SelectedItem.Text.Trim();
            unidadeNegocio = "L";
            codigoFilial = ddlFilial.SelectedValue.Trim();
            contaContabil = (contaContabil == null) ? "" : contaContabil.Replace(".", "@").Trim();

            return link = "dre_dre_v10_conta_contabil_loja.aspx?cu=" + codCentroCusto + "&cc=" + contaContabil + "&un=" + unidadeNegocio + "&ff=" + codigoFilial + "&m=" + mes.ToString() + "&a=" + ano + "&query=&procedure=drev10&rat=" + rateio;
        }

    }
}
