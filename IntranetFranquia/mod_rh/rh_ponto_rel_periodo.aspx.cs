using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Globalization;

namespace Relatorios
{
    public partial class rh_ponto_rel_periodo : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        int coluna = 0;

        CultureInfo culture = new CultureInfo("pt-BR");
        DateTimeFormatInfo dtfi = null;

        double segundosTotalHora = 0;
        double segundosFaltaHora = 0;
        double segundosExtraHora = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();

                int ultimoDia = 0;
                string mes = DateTime.Now.Month.ToString();
                string ano = DateTime.Now.Year.ToString();

                ultimoDia = DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes));

                if (mes.Length == 1)
                    mes = "0" + mes;
                txtDataInicial.Text = Convert.ToDateTime(ano + "-" + mes + "-01").ToString("dd/MM/yyyy");
                txtDataFinal.Text = Convert.ToDateTime(ano + "-" + mes + "-" + ultimoDia.ToString()).ToString("dd/MM/yyyy");
            }

            //Evitar duplo clique no botão
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

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();

                var filialDePara = new BaseController().BuscaFilialDePara();
                if (lstFilial.Count > 0)
                {
                    lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        ddlFilial_SelectedIndexChanged(null, null);
                    }
                }
            }
        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFuncionario(ddlFilial.SelectedValue);
        }
        private List<RH_PONTO_BATIDA_TIPO> ObterTipoBatida()
        {
            var tipoBatida = rhController.ObterBatidaTipo().Where(p => p.STATUS == 'A').ToList();
            tipoBatida.Insert(0, new RH_PONTO_BATIDA_TIPO { CODIGO = 0, DESCRICAO = "Selecione" });
            return tipoBatida;
        }
        private void CarregarFuncionario(string filial)
        {
            var funcionario = rhController.ObterFuncionario(null, "", filial, "", "").Where(p => p.BATE_PONTO == 'S').OrderBy(o => o.NOME).ToList();

            funcionario.Insert(0, new SP_OBTER_FUNCIONARIOResult { CODIGO = 0, NOME = "" });
            ddlFuncionario.DataSource = funcionario;
            ddlFuncionario.DataBind();

            if (ddlFuncionario.Items.Count == 2)
                ddlFuncionario.SelectedIndex = 1;
        }

        private bool ValidarData(string strHora)
        {
            try
            {
                if (strHora.Length == 5)
                    strHora = "1900-01-01 " + strHora;

                Convert.ToDateTime(strHora);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool ValidarPeriodo(string e1, string s1, string e2, string s2)
        {
            if (Convert.ToInt32(s1.Replace(":", "")) <= Convert.ToInt32(e1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) <= Convert.ToInt32(e2.Replace(":", "")))
                return false;

            if (Convert.ToInt32(e2.Replace(":", "")) <= Convert.ToInt32(s1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) <= Convert.ToInt32(s1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) <= Convert.ToInt32(e1.Replace(":", "")))
                return false;

            return true;
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlFilial.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                if (txtDataInicial.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Data INICIAL.";
                    return;
                }

                if (!ValidarData(txtDataInicial.Text.Trim()))
                {
                    labErro.Text = "Informe a Data INICIAL Válida.";
                    return;
                }

                if (txtDataFinal.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Data FINAL.";
                    return;
                }

                if (!ValidarData(txtDataFinal.Text.Trim()))
                {
                    labErro.Text = "Informe a Data FINAL Válida.";
                    return;
                }

                if (ddlFuncionario.SelectedValue.Trim() == "" || ddlFuncionario.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione o Funcionário.";
                    return;
                }

                int? codigoFuncionario = null;
                if (ddlFuncionario.SelectedValue.Trim() != "" && ddlFuncionario.SelectedValue.Trim() != "0")
                    codigoFuncionario = Convert.ToInt32(ddlFuncionario.SelectedValue);

                CarregarBatidas(ddlFilial.SelectedValue.Trim(), Convert.ToDateTime(txtDataInicial.Text.Trim()), Convert.ToDateTime(txtDataFinal.Text.Trim()), codigoFuncionario);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private List<SP_OBTER_REL_BATIDAResult> ObterRelBatida(string filial, DateTime dataInicial, DateTime dataFinal, int? codigoFuncionario)
        {
            List<SP_OBTER_REL_BATIDAResult> retorno = new List<SP_OBTER_REL_BATIDAResult>();

            retorno = rhController.ObterRelatorioBatida(filial, dataInicial, dataFinal, codigoFuncionario);

            return retorno;
        }
        private void CarregarBatidas(string filial, DateTime dataInicial, DateTime dataFinal, int? codigoFuncionario)
        {
            var relBatida = ObterRelBatida(filial, dataInicial, dataFinal, codigoFuncionario);

            gvBatida.DataSource = relBatida;
            gvBatida.DataBind();
        }
        private string CalcularPeriodo(DateTime? e1, DateTime? s1, DateTime? e2, DateTime? s2)
        {
            string periodoCalculado = "";

            TimeSpan periodo1 = TimeSpan.Zero;
            TimeSpan periodo2 = TimeSpan.Zero;
            double totalSegundos = 0;
            if (s1 != null && e1 != null)
                periodo1 = Convert.ToDateTime(s1).Subtract(Convert.ToDateTime(e1));

            if (s2 != null && e2 != null)
                periodo2 = Convert.ToDateTime(s2).Subtract(Convert.ToDateTime(e2));

            totalSegundos = periodo1.TotalSeconds + periodo2.TotalSeconds;

            TimeSpan time = TimeSpan.FromSeconds(totalSegundos);
            periodoCalculado = time.ToString(@"hh\:mm");

            return periodoCalculado;
        }

        #region "GRID BATIDA"
        protected void gvBatida_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            dtfi = culture.DateTimeFormat;
            string diaSemana = "";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_REL_BATIDAResult relBatida = e.Row.DataItem as SP_OBTER_REL_BATIDAResult;

                    coluna += 1;
                    if (relBatida != null)
                    {
                        string data = "";
                        int diaSemanaSql = (int)relBatida.DATA.DayOfWeek + 1;

                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                        {
                            diaSemana = dtfi.GetDayName(relBatida.DATA.DayOfWeek);
                            if (diaSemanaSql == 1)
                                data = "<font color='red'>" + relBatida.DATA.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(diaSemana).Substring(0, 3) + "</font>";
                            else
                                data = relBatida.DATA.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(diaSemana).Substring(0, 3);
                            _litData.Text = data;
                        }

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = "&nbsp;" + relBatida.NOME_FUNCIONARIO;

                        Literal _litTipoBatida = e.Row.FindControl("litTipoBatida") as Literal;
                        if (_litTipoBatida != null)
                            if (relBatida.TIPO_BATIDA != null)
                                _litTipoBatida.Text = relBatida.TIPO_BATIDA;

                        Literal _litEntrada1 = e.Row.FindControl("litEntrada1") as Literal;
                        if (_litEntrada1 != null)
                            if (relBatida.ENTRADA1 != null)
                                _litEntrada1.Text = Convert.ToDateTime(relBatida.ENTRADA1).ToString("HH:mm").Replace("00:00", "");

                        Literal _litSaida1 = e.Row.FindControl("litSaida1") as Literal;
                        if (_litSaida1 != null)
                            if (relBatida.SAIDA1 != null)
                                _litSaida1.Text = Convert.ToDateTime(relBatida.SAIDA1).ToString("HH:mm").Replace("00:00", "");

                        Literal _litEntrada2 = e.Row.FindControl("litEntrada2") as Literal;
                        if (_litEntrada2 != null)
                            if (relBatida.ENTRADA2 != null)
                                _litEntrada2.Text = Convert.ToDateTime(relBatida.ENTRADA2).ToString("HH:mm").Replace("00:00", "");

                        Literal _litSaida2 = e.Row.FindControl("litSaida2") as Literal;
                        if (_litSaida2 != null)
                            if (relBatida.SAIDA2 != null)
                                _litSaida2.Text = Convert.ToDateTime(relBatida.SAIDA2).ToString("HH:mm").Replace("00:00", "");

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            if (relBatida.TOTAL != null)
                            {
                                _litTotal.Text = Convert.ToDateTime(relBatida.TOTAL).ToString("HH:mm").Replace("00:00", "");
                                segundosTotalHora += relBatida.TOTAL.Subtract(Convert.ToDateTime("1900-01-01 00:00:00")).TotalSeconds;
                            }

                        Literal _litIntervalo = e.Row.FindControl("litIntervalo") as Literal;
                        if (_litIntervalo != null)
                            if (relBatida.INTERVALO != null)
                                _litIntervalo.Text = Convert.ToDateTime(relBatida.INTERVALO).ToString("HH:mm").Replace("00:00", "");

                        Literal _litFalta = e.Row.FindControl("litFalta") as Literal;
                        if (_litFalta != null)
                            if (relBatida.FALTA != null)
                                if (relBatida.FALTA >= Convert.ToDateTime("1900-01-01 00:00:00.000") &&
                                    (relBatida.TIPO_BATIDA_CODIGO == 1 ||
                                    relBatida.TIPO_BATIDA_CODIGO == 3))
                                {
                                    _litFalta.Text = Convert.ToDateTime(relBatida.FALTA).ToString("HH:mm").Replace("00:00", "");
                                    segundosFaltaHora += relBatida.FALTA.Subtract(Convert.ToDateTime("1900-01-01 00:00:00")).TotalSeconds;
                                }


                        Literal _litHoraExtra = e.Row.FindControl("litHoraExtra") as Literal;
                        if (_litHoraExtra != null)
                            if (relBatida.HORA_EXTRA != null)
                                if (relBatida.HORA_EXTRA >= Convert.ToDateTime("1900-01-01 00:00:00.000") &&
                                    (relBatida.TIPO_BATIDA_CODIGO == 1 ||
                                    relBatida.TIPO_BATIDA_CODIGO == 3))
                                {
                                    _litHoraExtra.Text = Convert.ToDateTime(relBatida.HORA_EXTRA).ToString("HH:mm").Replace("00:00", "");
                                    segundosExtraHora += relBatida.HORA_EXTRA.Subtract(Convert.ToDateTime("1900-01-01 00:00:00")).TotalSeconds;
                                }

                        //Pinta linha FERIADO
                        var feriadoLocal = rhController.ObterFeriado(relBatida.CODIGO_FILIAL).Where(p => p.DATA == relBatida.DATA);
                        var feriadoNacional = rhController.ObterFeriado().Where(p => p.TIPO == 'N' && p.DATA == relBatida.DATA);
                        //FERIADO NAO TRABALHADO
                        if ((feriadoLocal.Where(p => p.TRABALHADO == 'N') != null && feriadoLocal.Where(p => p.TRABALHADO == 'N').Count() > 0) || (feriadoNacional.Where(p => p.TRABALHADO == 'N') != null && feriadoNacional.Where(p => p.TRABALHADO == 'N').Count() > 0))
                        {
                            e.Row.BackColor = Color.Wheat;
                            _litTipoBatida.Text = "Feriado Não Trab";
                        }

                        //FERIADO TRABALHADO
                        if ((feriadoLocal.Where(p => p.TRABALHADO == 'S') != null && feriadoLocal.Where(p => p.TRABALHADO == 'S').Count() > 0) || (feriadoNacional.Where(p => p.TRABALHADO == 'S') != null && feriadoNacional.Where(p => p.TRABALHADO == 'S').Count() > 0))
                            e.Row.BackColor = Color.Pink;

                        if (relBatida.TIPO_BATIDA_CODIGO == 2) //FOLGA
                            e.Row.BackColor = Color.LightGreen;
                        if (relBatida.TIPO_BATIDA_CODIGO == 3) //FALTA
                            e.Row.BackColor = Color.IndianRed;
                        if (relBatida.TIPO_BATIDA_CODIGO == 99) //PENDENTE
                            e.Row.BackColor = Color.Lavender;

                    }
                }
            }
        }
        protected void gvBatida_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvBatida.FooterRow;
            if (footer != null)
            {

                footer.Cells[9].Text = RetornarHoraTotal(segundosFaltaHora);
                footer.Cells[10].Text = RetornarHoraTotal(segundosExtraHora);
                footer.Cells[11].Text = RetornarHoraTotal(segundosTotalHora);

                labTotalHoraTrabalhada.Text = RetornarHoraTotal((segundosTotalHora + segundosExtraHora) - segundosFaltaHora);
            }
        }

        private string RetornarHoraTotal(double seconds)
        {
            string retorno = "";
            string totalHora = "";
            string totalMinuto = "";
            DateTime dataZero = new DateTime(1900, 01, 01);
            TimeSpan tsHora = TimeSpan.FromSeconds(seconds);

            dataZero = dataZero + tsHora;

            int minuto = 0;
            int hora = 0;
            int dia = 0;
            int diaHora = 0;

            minuto = dataZero.Minute;
            hora = dataZero.Hour;
            dia = dataZero.Day - 1;
            diaHora = dia * 24;

            totalHora = (diaHora + hora).ToString();
            totalHora = (totalHora.Length == 1) ? ("0" + totalHora) : totalHora;
            totalMinuto = minuto.ToString();
            totalMinuto = (totalMinuto.Length == 1) ? ("0" + totalMinuto) : totalMinuto;

            retorno = totalHora + ":" + totalMinuto;

            /*string[] splitHora = tsHora.TotalHours.ToString("00.00").Split(',');

            totalHora = splitHora[0];
            totalMinuto = Convert.ToInt32(splitHora[1]).ToString("00");
            totalMinuto = (Convert.ToInt32(totalMinuto) * 60 / 100).ToString("00");

            retorno = totalHora + ":" + totalMinuto;*/

            return retorno;
        }
        #endregion

    }
}
