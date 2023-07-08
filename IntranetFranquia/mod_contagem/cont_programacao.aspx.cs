using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Relatorios
{
    public partial class cont_programacao : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();

        CultureInfo culture = new CultureInfo("pt-BR");
        DateTimeFormatInfo dtfi;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Carregar filtros
                CarregarFilial();
                CarregarDataAno();

                CarregarProgramacaoPorMes(DateTime.Now.Year, "");

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            btFecharMes.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btFecharMes, null) + ";");
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
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilialFiltro.DataSource = filial;
                ddlFilialFiltro.DataBind();
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

                ddlAnoFiltro.DataSource = dataAno;
                ddlAnoFiltro.DataBind();

                ddlAnoFechamento.DataSource = dataAno;
                ddlAnoFechamento.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                        ddlAnoFiltro.SelectedValue = DateTime.Now.Year.ToString();
                        ddlAnoFechamento.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        private void CarregarDataDia(int ano, int mes)
        {
            int diaFinal = 30;
            if (mes == 1 || mes == 3 || mes == 5 || mes == 7 || mes == 8 || mes == 10 || mes == 12)
                diaFinal = 31;

            if (mes == 2) // Se fevereiro
            {
                if (DateTime.IsLeapYear(ano)) // É Ano Bissexto?
                    diaFinal = 29;
                else
                    diaFinal = 28;
            }

            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem { Value = "", Text = "Selecione" });
            for (int i = 1; i <= diaFinal; i++)
                listItem.Add(new ListItem { Value = i.ToString(), Text = i.ToString() });

            ddlDia.DataSource = listItem;
            ddlDia.DataBind();

        }
        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAno.SelectedValue != "" && ddlMes.SelectedValue != "")
            {
                CarregarDataDia(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue));
                btnSalvar.Enabled = !contController.MesFechado(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue));
            }
        }
        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAno.SelectedValue != "" && ddlMes.SelectedValue != "")
            {
                CarregarDataDia(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue));
                btnSalvar.Enabled = !contController.MesFechado(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue));
            }
            else
            {
                gvProgramacao.DataSource = new List<CONT_PROGRAMACAO>();
                gvProgramacao.DataBind();
            }


        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescricao.Enabled = true;
            if (ddlFilial.SelectedValue != "")
            {
                txtDescricao.Text = "";
                txtDescricao.Enabled = false;
            }
        }
        protected void ddlDia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDia.SelectedValue != "" && ddlDia.SelectedValue != "Selecione")
            {
                CarregarProgramacao(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), Convert.ToInt32(ddlDia.SelectedValue));
            }
            else
            {
                gvProgramacao.DataSource = new List<CONT_PROGRAMACAO>();
                gvProgramacao.DataBind();
            }
        }
        #endregion

        #region "PROGRAMAÇÃO"
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                int ano = 0;
                int mes = 0;
                int dia = 0;

                if (ddlAno.SelectedValue != "")
                    ano = Convert.ToInt32(ddlAno.SelectedValue);

                if (ddlMes.SelectedValue != "")
                    mes = Convert.ToInt32(ddlMes.SelectedValue);

                if (ddlDia.SelectedValue != "" && ddlDia.SelectedValue != "Selecione")
                    dia = Convert.ToInt32(ddlDia.SelectedValue);

                if (ano == 0)
                {
                    labErro.Text = "Selecione o Ano.";
                    return;
                }

                if (mes == 0)
                {
                    labErro.Text = "Selecione o Mês.";
                    return;
                }

                if (dia == 0)
                {
                    labErro.Text = "Selecione o Dia.";
                    return;
                }

                if (ddlFilial.SelectedValue == "" && txtDescricao.Text.Trim() == "")
                {
                    labErro.Text = "Selecione uma Filial ou Informe uma Descrição";
                    return;
                }

                CONT_PROGRAMACAO contProgramacao = new CONT_PROGRAMACAO();
                contProgramacao.DATA = Convert.ToDateTime(ano.ToString() + "/" + mes.ToString() + "/" + dia.ToString());
                contProgramacao.CODIGO_FILIAL = ddlFilial.SelectedValue;
                contProgramacao.TIPO = (ddlFilial.SelectedValue == "" ? 'D' : 'F');
                contProgramacao.OBS = txtDescricao.Text.Trim().ToUpper();
                contProgramacao.RESPONSAVEL = txtResponsavel.Text.Trim().ToUpper();

                contController.InserirProgramacao(contProgramacao);

                ddlFilial.SelectedValue = "";
                txtDescricao.Text = "";
                txtDescricao.Enabled = true;

                CarregarProgramacao(ano, mes, dia);
                CarregarProgramacaoPorMes(Convert.ToInt32(ddlAno.SelectedValue), ddlFilialFiltro.SelectedValue);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarProgramacao(int ano, int mes, int dia)
        {
            List<CONT_PROGRAMACAO> contProgramacaoLista = new List<CONT_PROGRAMACAO>();
            contProgramacaoLista = contController.ObterProgramacao(ano, mes, dia);
            gvProgramacao.DataSource = contProgramacaoLista;
            gvProgramacao.DataBind();
        }
        protected void gvProgramacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CONT_PROGRAMACAO contProg = e.Row.DataItem as CONT_PROGRAMACAO;

                    if (contProg != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(contProg.DATA).ToString("dd/MM/yyyy");

                        dtfi = culture.DateTimeFormat;
                        string dia_da_semana = dtfi.GetDayName(Convert.ToDateTime(contProg.DATA).DayOfWeek);
                        Literal _litDiaSemana = e.Row.FindControl("litDiaSemana") as Literal;
                        if (_litDiaSemana != null)
                            _litDiaSemana.Text = Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana);

                        string itemProgramacao = "";

                        if (contProg.CODIGO_FILIAL != null && contProg.CODIGO_FILIAL.Trim() != "")
                        {
                            var filial = baseController.BuscaFilialCodigo(int.Parse(contProg.CODIGO_FILIAL)).FILIAL;
                            itemProgramacao = filial;
                        }
                        else
                        {
                            itemProgramacao = contProg.OBS;
                        }

                        Literal _litItem = e.Row.FindControl("litItem") as Literal;
                        if (_litItem != null)
                            _litItem.Text = itemProgramacao;

                        Literal _litResponsavel = e.Row.FindControl("litResponsavel") as Literal;
                        if (_litResponsavel != null)
                            _litResponsavel.Text = contProg.RESPONSAVEL;

                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = contProg.CODIGO.ToString();
                            _btExcluir.Visible = !contController.MesFechado(Convert.ToDateTime(contProg.DATA).Year, Convert.ToDateTime(contProg.DATA).Month);
                        }

                    }

                }
            }
        }
        protected void btExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton _bt = (ImageButton)sender;

                if (_bt != null)
                {
                    int codigoContProgramacao = 0;
                    codigoContProgramacao = Convert.ToInt32(_bt.CommandArgument);
                    contController.ExcluirProgramacao(codigoContProgramacao);

                    if (ddlAno.SelectedValue != "" && ddlMes.SelectedValue != "" && ddlDia.SelectedValue != "" && ddlDia.SelectedValue != "Selecione")
                        CarregarProgramacao(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), Convert.ToInt32(ddlDia.SelectedValue));

                    CarregarProgramacaoPorMes(Convert.ToInt32(ddlAno.SelectedValue), ddlFilialFiltro.SelectedValue);
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region"POR MES"
        private void CarregarProgramacaoPorMes(int ano, string codigoFilial)
        {

            gvJaneiro.DataSource = ObterProgramacaoPorMes(ano, 1, codigoFilial);
            gvJaneiro.DataBind();

            gvFevereiro.DataSource = ObterProgramacaoPorMes(ano, 2, codigoFilial);
            gvFevereiro.DataBind();

            gvMarco.DataSource = ObterProgramacaoPorMes(ano, 3, codigoFilial);
            gvMarco.DataBind();

            gvAbril.DataSource = ObterProgramacaoPorMes(ano, 4, codigoFilial);
            gvAbril.DataBind();

            gvMaio.DataSource = ObterProgramacaoPorMes(ano, 5, codigoFilial);
            gvMaio.DataBind();

            gvJunho.DataSource = ObterProgramacaoPorMes(ano, 6, codigoFilial);
            gvJunho.DataBind();

            gvJulho.DataSource = ObterProgramacaoPorMes(ano, 7, codigoFilial);
            gvJulho.DataBind();

            gvAgosto.DataSource = ObterProgramacaoPorMes(ano, 8, codigoFilial);
            gvAgosto.DataBind();

            gvSetembro.DataSource = ObterProgramacaoPorMes(ano, 9, codigoFilial);
            gvSetembro.DataBind();

            gvOutubro.DataSource = ObterProgramacaoPorMes(ano, 10, codigoFilial);
            gvOutubro.DataBind();

            gvNovembro.DataSource = ObterProgramacaoPorMes(ano, 11, codigoFilial);
            gvNovembro.DataBind();

            gvDezembro.DataSource = ObterProgramacaoPorMes(ano, 12, codigoFilial);
            gvDezembro.DataBind();

        }
        private List<CONT_PROGRAMACAO> ObterProgramacaoPorMes(int ano, int mes, string codigoFilial)
        {
            if (codigoFilial == "")
                return contController.ObterProgramacao(ano, mes)
                    .OrderBy(p => p.DATA).ToList();
            else
                return contController.ObterProgramacao(ano, mes).Where(p => p.CODIGO_FILIAL != null && p.CODIGO_FILIAL.Trim() == codigoFilial.Trim())
                    .OrderBy(p => p.DATA).ToList();
        }
        protected void ddlAnoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarProgramacaoPorMes(Convert.ToInt32(ddlAnoFiltro.SelectedValue), ddlFilialFiltro.SelectedValue);
        }
        protected void ddlFilialFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarProgramacaoPorMes(Convert.ToInt32(ddlAnoFiltro.SelectedValue), ddlFilialFiltro.SelectedValue);
        }


        #endregion

        #region "FECHAMENTO MES"
        protected void btFecharMes_Click(object sender, EventArgs e)
        {
            DateTime dataIni;
            DateTime dataFim;
            int diaFinal = 0;

            try
            {
                labErro.Text = "";
                int ano = 0;
                int mes = 0;

                if (ddlAnoFechamento.SelectedValue != "")
                    ano = Convert.ToInt32(ddlAnoFechamento.SelectedValue);

                if (ddlMesFechamento.SelectedValue != "")
                    mes = Convert.ToInt32(ddlMesFechamento.SelectedValue);

                if (ano == 0)
                {
                    labErro.Text = "Selecione o Ano.";
                    return;
                }

                if (mes == 0)
                {
                    labErro.Text = "Selecione o Mês.";
                    return;
                }

                dataIni = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-01").Date;
                diaFinal = DateTime.DaysInMonth(ano, mes);
                dataFim = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-" + diaFinal.ToString()).Date;

                string listFilialErro = "";
                foreach (ListItem f in ddlFilial.Items)
                {
                    if (f.Value.Trim() != "")
                    {
                        var dataContagem = contController.ObterDataParaContagem(f.Value)
                            .Where(p => p.DATA >= dataIni &&
                                        p.DATA <= dataFim &&
                                        p.TIPO == 'F');
                        if (dataContagem != null && dataContagem.Count() > 0)
                        {
                            listFilialErro = listFilialErro + f.Text.Trim() + ", ";
                        }
                    }
                }

                if (listFilialErro != "")
                {
                    listFilialErro = listFilialErro.Trim() + ",";
                    listFilialErro = listFilialErro.Replace(",,", "");

                    labErro.Text = "Para fechar este Mês, insira o resultado para todas as Filiais (" + listFilialErro + "), ou exclua as programações canceladas.";
                    return;
                }


                bool ok = InserirResultado(ano, mes);
                if (ok)
                {
                    CONT_RESULTADO_FECHAMENTO fechamento = new CONT_RESULTADO_FECHAMENTO();
                    fechamento.DATA = Convert.ToDateTime(ano.ToString() + "-" + ((mes.ToString().Length == 1) ? ("0" + mes.ToString()) : mes.ToString()) + "-01");
                    contController.InserirResultadoFechamento(fechamento);
                    labErro.Text = "Fechamento de " + ddlMesFechamento.SelectedItem.Text + " de " + ano.ToString() + " realizado com sucesso.";
                }

                ddlAnoFechamento_SelectedIndexChanged(null, null);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private bool InserirResultado(int ano, int mes)
        {
            bool resp = false;
            DateTime dataIni;
            DateTime dataFim;
            int diaFinal = 0;

            var resultado = contController.ObterResultado(ano, mes);

            CONT_RESULTADO_FINAL final = null;
            foreach (var res in resultado)
            {

                dataIni = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-01").Date;
                diaFinal = DateTime.DaysInMonth(ano, mes);
                dataFim = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-" + diaFinal.ToString()).Date;

                var resultadoFinal = contController.ObterResultadoPorLoja(res.CODIGO_FILIAL)
                    .Where(p => p.DATA_CONTAGEM >= dataIni &&
                                p.DATA_CONTAGEM <= dataFim);

                foreach (var f in resultadoFinal)
                {
                    final = new CONT_RESULTADO_FINAL();
                    final.CONT_RESULTADO = f.CODIGO_RESULTADO;
                    final.CODIGO_FILIAL = f.CODIGO_FILIAL;
                    final.DATA_CONTAGEM = f.DATA_CONTAGEM;
                    final.DIAS_ULTIMA_CONTAGEM = f.DIAS_ULTIMA_CONTAGEM;
                    final.VALOR_ACEITAVEL = f.VALOR_ACEITAVEL;
                    final.VALOR_ACEITAVEL_PORC = f.VALOR_ACEITAVEL_PORC;
                    final.RESULTADO_PECAS = f.RESULTADO_PECAS;
                    final.PORC_PERDA = f.PORC_PERDA;
                    final.PECAS_POR_DIA = f.PECAS_POR_DIA;
                    final.PORC_STATUS = f.PORC_STATUS;
                    final.LEGENDA = f.LEGENDA;
                    final.GERENTE = f.GERENTE;

                    contController.InserirResultadoFinal(final);
                }
            }

            resp = true;
            return resp;
        }

        protected void ddlAnoFechamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            btFecharMes.Enabled = false;
            if (ddlMesFechamento.SelectedValue != "")
                btFecharMes.Enabled = !contController.MesFechado(Convert.ToInt32(ddlAnoFechamento.SelectedValue), Convert.ToInt32(ddlMesFechamento.SelectedValue));
        }

        #endregion



    }
}