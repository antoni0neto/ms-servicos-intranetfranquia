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
    public partial class contabil_receita_x_despesa : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        Color corTitulo = System.Drawing.SystemColors.GradientActiveCaption;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";

        decimal dJaneiro = 0;
        decimal dFevereiro = 0;
        decimal dMarco = 0;
        decimal dAbril = 0;
        decimal dMaio = 0;
        decimal dJunho = 0;
        decimal dJulho = 0;
        decimal dAgosto = 0;
        decimal dSetembro = 0;
        decimal dOutubro = 0;
        decimal dNovembro = 0;
        decimal dDezembro = 0;
        decimal dTotal = 0;

        List<SP_OBTER_RECEITA_X_DESPESAResult> gReceitaDespesa = new List<SP_OBTER_RECEITA_X_DESPESAResult>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
                CarregarMatrizContabil();
                CarregarContaContabilTipo();
                CarregarContaContabil("");
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            string ano = "";
            ano = ddlAno.SelectedValue;

            try
            {
                labErro.Text = "";
                CarregarReceitasXDespesas(Convert.ToInt32(ano), ddlMatrizContabil.SelectedValue.Trim(), ddlTipoConta.SelectedValue.Trim(), ddlContaContabil.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        #region "DADOS INICIAIS"
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
        private void CarregarMatrizContabil()
        {
            var matrizContabil = contabilController.ObterMatrizContabil();

            ddlMatrizContabil.DataSource = matrizContabil;
            ddlMatrizContabil.DataBind();
            ddlMatrizContabil.Items.Insert(0, new ListItem("", ""));
        }
        private void CarregarContaContabilTipo()
        {
            var contaContabilTipo = contabilController.ObterContaContabilTipo().Where(p => p.TIPO_CONTA.Trim().Substring(0, 2) == "3." || p.TIPO_CONTA.Trim().Substring(0, 2) == "4.");

            ddlTipoConta.DataSource = contaContabilTipo;
            ddlTipoConta.DataBind();
            ddlTipoConta.Items.Insert(0, new ListItem("", ""));
        }
        protected void ddlTipoConta_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarContaContabil(ddlTipoConta.SelectedValue.Trim());
        }
        private void CarregarContaContabil(string tipoConta)
        {
            var contaContabil = new ContabilidadeController().ObterContas().Where(p => p.CONTA_CONTABIL.Trim().Substring(0, 2) == "3." || p.CONTA_CONTABIL.Trim().Substring(0, 2) == "4.").ToList();

            if (tipoConta != "")
                contaContabil = contaContabil.Where(p => p.CONTA_CONTABIL.Substring(0, 8).Trim() == tipoConta.Trim()).ToList();

            ddlContaContabil.DataSource = contaContabil;
            ddlContaContabil.DataBind();
            ddlContaContabil.Items.Insert(0, new ListItem("", ""));
        }

        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,###,##0.00;(###,###,###,###,##0.00)") + "</font> ";
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
            return Convert.ToDecimal(valor);
        }
        #endregion

        #region "ACOES"
        private void CarregarReceitasXDespesas(int ano, string codMatrizContabil, string tipoConta, string contaContabil)
        {
            //Obter Receitas X Despesas
            gReceitaDespesa = contabilController.ObterReceitaXDespesa(ano, codMatrizContabil, tipoConta, contaContabil);

            if (gReceitaDespesa != null)
            {
                gvClassificacao.DataSource = gReceitaDespesa;
                gvClassificacao.DataBind();
            }
        }
        #endregion

        #region "RECEITA X DESPESA"

        protected void gvClassificacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string classificacao = "";
            string conta_contabil = "";
            string matrizContabil = "";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_RECEITA_X_DESPESAResult rd = e.Row.DataItem as SP_OBTER_RECEITA_X_DESPESAResult;

                    if (rd != null && rd.CLASSIFICACAO.Trim() != "")
                    {

                        if (ddlMatrizContabil.SelectedValue.Trim() != "")
                            matrizContabil = ddlMatrizContabil.SelectedValue.Trim();

                        if (rd.CLASSIFICACAO.Trim().Length == 8)
                        {
                            e.Row.BackColor = Color.Lavender;
                            classificacao = "<a href='#' class='adre' title='" + rd.CLASSIFICACAO.Trim() + " - " + rd.DENOMINACAO.Trim() + "'><font size='2' face='Calibri'>" + rd.CLASSIFICACAO.Trim() + " - " + ((rd.DENOMINACAO.Trim().Length <= 23) ? rd.DENOMINACAO.Trim() : rd.DENOMINACAO.Trim().Substring(0, 23)) + "</font></a>";
                        }
                        else
                        {
                            conta_contabil = rd.CLASSIFICACAO.Trim().Replace(".", "@");
                            classificacao = "<a href='#' class='adre' title='" + rd.CLASSIFICACAO.Trim() + " - " + rd.DENOMINACAO.Trim() + "'><font size='1' face='Calibri'>&nbsp;&nbsp;" + rd.CLASSIFICACAO.Trim() + " - </font><font size='2' face='Calibri'>" + ((rd.DENOMINACAO.Trim().Length <= 21) ? rd.DENOMINACAO.Trim() : rd.DENOMINACAO.Trim().Substring(0, 21)) + "</font></a>";
                        }

                        Literal _litClassificacao = e.Row.FindControl("litClassificacao") as Literal;
                        if (_litClassificacao != null)
                            _litClassificacao.Text = classificacao;

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarLink(rd.JANEIRO, conta_contabil, matrizContabil, 1, ddlAno.SelectedItem.Text.Trim(), 2);


                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarLink(rd.FEVEREIRO, conta_contabil, matrizContabil, 2, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarLink(rd.MARCO, conta_contabil, matrizContabil, 3, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarLink(rd.ABRIL, conta_contabil, matrizContabil, 4, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarLink(rd.MAIO, conta_contabil, matrizContabil, 5, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarLink(rd.JUNHO, conta_contabil, matrizContabil, 6, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarLink(rd.JULHO, conta_contabil, matrizContabil, 7, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarLink(rd.AGOSTO, conta_contabil, matrizContabil, 8, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarLink(rd.SETEMBRO, conta_contabil, matrizContabil, 9, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarLink(rd.OUTUBRO, conta_contabil, matrizContabil, 10, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarLink(rd.NOVEMBRO, conta_contabil, matrizContabil, 11, ddlAno.SelectedItem.Text.Trim(), 2);

                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarLink(rd.DEZEMBRO, conta_contabil, matrizContabil, 12, ddlAno.SelectedItem.Text.Trim(), 2);

                        totalLinha = Convert.ToDecimal(FiltroValor(rd.JANEIRO, 1) + FiltroValor(rd.FEVEREIRO, 2) + FiltroValor(rd.MARCO, 3) + FiltroValor(rd.ABRIL, 4) + FiltroValor(rd.MAIO, 5) + FiltroValor(rd.JUNHO, 6) + FiltroValor(rd.JULHO, 7) + FiltroValor(rd.AGOSTO, 8) + FiltroValor(rd.SETEMBRO, 9) + FiltroValor(rd.OUTUBRO, 10) + FiltroValor(rd.NOVEMBRO, 11) + FiltroValor(rd.DEZEMBRO, 12));
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        if (rd.CLASSIFICACAO.Trim().Length == 13)
                        {
                            dJaneiro += FiltroValor(rd.JANEIRO, 1);
                            dFevereiro += FiltroValor(rd.FEVEREIRO, 2);
                            dMarco += FiltroValor(rd.MARCO, 3);
                            dAbril += FiltroValor(rd.ABRIL, 4);
                            dMaio += FiltroValor(rd.MAIO, 5);
                            dJunho += FiltroValor(rd.JUNHO, 6);
                            dJulho += FiltroValor(rd.JULHO, 7);
                            dAgosto += FiltroValor(rd.AGOSTO, 8);
                            dSetembro += FiltroValor(rd.SETEMBRO, 9);
                            dOutubro += FiltroValor(rd.OUTUBRO, 10);
                            dNovembro += FiltroValor(rd.NOVEMBRO, 11);
                            dDezembro += FiltroValor(rd.DEZEMBRO, 12);
                            dTotal += totalLinha;
                        }

                    }
                }
            }
        }
        protected void gvClassificacao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvClassificacao.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[1].Text = FormatarValor(FiltroValor(dJaneiro, 1));
                footer.Cells[2].Text = FormatarValor(FiltroValor(dFevereiro, 2));
                footer.Cells[3].Text = FormatarValor(FiltroValor(dMarco, 3));
                footer.Cells[4].Text = FormatarValor(FiltroValor(dAbril, 4));
                footer.Cells[5].Text = FormatarValor(FiltroValor(dMaio, 5));
                footer.Cells[6].Text = FormatarValor(FiltroValor(dJunho, 6));
                footer.Cells[7].Text = FormatarValor(FiltroValor(dJulho, 7));
                footer.Cells[8].Text = FormatarValor(FiltroValor(dAgosto, 8));
                footer.Cells[9].Text = FormatarValor(FiltroValor(dSetembro, 9));
                footer.Cells[10].Text = FormatarValor(FiltroValor(dOutubro, 10));
                footer.Cells[11].Text = FormatarValor(FiltroValor(dNovembro, 11));
                footer.Cells[12].Text = FormatarValor(FiltroValor(dDezembro, 12));
                footer.Cells[13].Text = FormatarValor(dTotal);
            }
        }

        #endregion

        private string FormatarLink(decimal valor, string contaContabil, string matrizContabil, int mes, string ano, int tela)
        {
            string url_link = "";
            string mesExtenso = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(mes).ToLower();

            mesExtenso = char.ToUpper(mesExtenso[0]) + mesExtenso.Substring(1);

            if (contaContabil.Length == 13)
            {
                url_link = "contabil_receita_x_despesa_det.aspx?cc=" + contaContabil + "&mc=" + matrizContabil + "&m=" + mes.ToString() + "&a=" + ano;
                return "<a href='" + url_link + "' target='_blank' class='adre' title='" + mesExtenso + "'>&nbsp;" + FormatarValor(FiltroValor(valor, mes)) + "</a>";
            }
            else
            {
                return FormatarValor(FiltroValor(valor, mes)).Trim();
            }
        }

    }
}
