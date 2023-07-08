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

namespace Relatorios
{
    public partial class rh_ponto_batida : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        int coluna = 0;

        List<RH_PONTO_BATIDA> gPontoBatida = new List<RH_PONTO_BATIDA>();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataBatida.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()-1) });});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();

                dialogPai.Visible = false;
                btCancelarBatida.Visible = false;
            }

            //Carregar lista para preencher as cores do calendario
            gPontoBatida = rhController.ObterPontoBatida(ddlFilial.SelectedValue);

            //Evitar duplo clique no botão
            btGerarBatida.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarBatida, null) + ";");
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
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
        }
        private List<RH_PONTO_BATIDA_TIPO> ObterTipoBatida()
        {
            var tipoBatida = rhController.ObterBatidaTipo().Where(p => p.STATUS == 'A').ToList();
            //tipoBatida.Insert(0, new RH_PONTO_BATIDA_TIPO { CODIGO = -1, DESCRICAO = "Selecione" });
            return tipoBatida;
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue.Trim() == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labDataBatida.ForeColor = _OK;
            if (txtDataBatida.Text.Trim() == "")
            {
                labDataBatida.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
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
            if (Convert.ToInt32(s1.Replace(":", "")) < Convert.ToInt32(e1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) < Convert.ToInt32(e2.Replace(":", "")))
                return false;

            if (Convert.ToInt32(e2.Replace(":", "")) < Convert.ToInt32(s1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) < Convert.ToInt32(s1.Replace(":", "")))
                return false;

            if (Convert.ToInt32(s2.Replace(":", "")) < Convert.ToInt32(e1.Replace(":", "")))
                return false;

            return true;
        }

        protected void calBatida_DayRender(object sender, DayRenderEventArgs e)
        {
            //PENDENTE - Honeydew
            //INCOMPLETO - IndianRed
            //COMPLETO - PaleGreen
            e.Day.IsSelectable = false;
            if (e.Day.Date < DateTime.Now.AddDays(-1))
            {
                e.Cell.BackColor = Color.Lavender;

                var feriadoTrabalhado = rhController.ObterFeriado(ddlFilial.SelectedValue.Trim()).Where(p => p.DATA == e.Day.Date && p.TRABALHADO == 'N').ToList();
                if (feriadoTrabalhado != null && feriadoTrabalhado.Count() > 0)
                    e.Cell.BackColor = Color.SkyBlue;

                if (gPontoBatida != null && gPontoBatida.Count >= 0)
                {
                    var diaBatida = gPontoBatida.Where(p => p.DATA == e.Day.Date).ToList();
                    if (diaBatida != null && diaBatida.Count() > 0)
                    {
                        e.Cell.BackColor = Color.PaleGreen;
                        foreach (var f in diaBatida)
                        {
                            if ((f.STATUS != 'B' && f.STATUS != 'N' && f.STATUS != 'E') || f.RH_PONTO_BATIDA_TIPO == 0)
                            {
                                e.Cell.BackColor = Color.IndianRed;
                                break;
                            }
                        }
                    }
                }
            }
        }
        protected void calBatida_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
        }
        #endregion

        #region "INCLUSAO"
        protected void btIncluirBatida_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                if (!ValidarData(txtDataBatida.Text.Trim()))
                {
                    labErro.Text = "Informa a Data Referência Válida.";
                    return;
                }

                /*//verificar se ja foi inserido
                var pb = rhController.ObterPontoBatida(ddlFilial.SelectedValue.Trim(), Convert.ToDateTime(txtDataBatida.Text.Trim()), ddlFuncionario.SelectedValue);
                if (pb != null)
                {
                    labErro.Text = "Este DIA já foi inserido para este FUNCIONÁRIO. Para alterar, clique em BUSCAR.";
                    return;
                }

                if (!ValidarData(txtEntrada1.Text.Trim()))
                {
                    labErro.Text = "Informa a Hora da Entrada 1 Válida.";
                    return;
                }

                if (!ValidarData(txtSaida1.Text.Trim()))
                {
                    labErro.Text = "Informa a Hora da Saída 1 Válida.";
                    return;
                }

                if (!ValidarData(txtEntrada2.Text.Trim()))
                {
                    labErro.Text = "Informa a Hora da Entrada 2 Válida.";
                    return;
                }

                if (!ValidarData(txtSaida2.Text.Trim()))
                {
                    labErro.Text = "Informa a Hora da Saída 2 Válida.";
                    return;
                }

                if (!ValidarPeriodo(txtEntrada1.Text, txtSaida1.Text, txtEntrada2.Text, txtSaida2.Text))
                {
                    labErro.Text = "O período da HORA está incorreto. Informe as horas em ORDEM.";
                    return;
                }

                int diaSemana = ((int)Convert.ToDateTime(txtDataBatida.Text).DayOfWeek) + 1;
                var perTrab = rhController.ObterFuncionarioPeriodoTrab(ddlFuncionario.SelectedValue).Where(p => diaSemana >= p.PONTO_PERIODO_TRAB1.DIA_INICIAL && diaSemana <= p.PONTO_PERIODO_TRAB1.DIA_FINAL).ToList();
                if (perTrab == null || perTrab.Count() <= 0)
                {
                    labErro.Text = "FUNCIONÁRIO NÃO POSSUI PERÍODO DE TRABALHO CADASTRADO PARA ESTE DIA DA SEMANA. ENTRE EM CONTATO COM O RH.";
                    return;
                }*/

                //int codigoBatida = 0;
                //codigoBatida = InserirBatida(perTrab[0].PONTO_PERIODO_TRAB);

                //CarregarBatidas(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataBatida.Text), ddlFuncionario.SelectedValue);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private int InserirBatida(string codigoFilial,
                                    DateTime data,
                                    int tipoBatida,
                                    int? codigoFuncionario,
                                    string nomeFuncionario,
                                    int? periodoTrabalhado,
                                    char status,
                                    string entrada1,
                                    string saida1,
                                    string entrada2,
                                    string saida2
        )
        {
            RH_PONTO_BATIDA _pontoBatida = null;
            int codigoBatida = 0;

            //Salvar BATIDA
            _pontoBatida = new RH_PONTO_BATIDA();
            _pontoBatida.DATA = data;
            if (codigoFuncionario != null)
                _pontoBatida.RH_FUNCIONARIO = Convert.ToInt32(codigoFuncionario);
            _pontoBatida.RH_PONTO_BATIDA_TIPO = tipoBatida;
            _pontoBatida.RH_PONTO_PERIODO_TRAB = periodoTrabalhado;
            _pontoBatida.CODIGO_FILIAL = codigoFilial;
            _pontoBatida.ENTRADA1 = Convert.ToDateTime(data.ToString("yyyy-MM-dd") + " " + entrada1);
            _pontoBatida.SAIDA1 = Convert.ToDateTime(data.ToString("yyyy-MM-dd") + " " + saida1);
            _pontoBatida.ENTRADA2 = Convert.ToDateTime(data.ToString("yyyy-MM-dd") + " " + entrada2);
            _pontoBatida.SAIDA2 = Convert.ToDateTime(data.ToString("yyyy-MM-dd") + " " + saida2);
            _pontoBatida.ENTRADA3 = null;
            _pontoBatida.SAIDA3 = null;
            _pontoBatida.HORAS_NORMAIS = null;
            _pontoBatida.HORAS_EXTRAS = null;
            _pontoBatida.STATUS = status;
            _pontoBatida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            _pontoBatida.DATA_INCLUSAO = DateTime.Now;
            //_pontoBatida.OBSERVACAO = txtObservacao.Text.Trim();
            _pontoBatida.FUNCIONARIO_NOME = nomeFuncionario.ToUpper();

            codigoBatida = rhController.InserirPontoBatida(_pontoBatida);

            return codigoBatida;
        }

        #endregion

        protected void btCancelarBatida_Click(object sender, EventArgs e)
        {
            try
            {

                txtDataBatida.Text = "";

                ddlFilial.Enabled = true;
                txtDataBatida.Enabled = true;

                gvBatida.DataSource = new List<RH_PONTO_BATIDA>();
                gvBatida.DataBind();

                gvBatidaNova.DataSource = new List<RH_PONTO_BATIDA>();
                gvBatidaNova.DataBind();

                btCancelarBatida.Visible = false;
                btSalvar.Visible = false;

                labAjuda.Visible = false;

                labErro.Text = "";
            }
            catch (Exception ex)
            {
                labErro.Text = "Erro ao Sair da Batida: " + ex.Message;
            }
        }
        protected void btGerarBatida_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlFilial.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                if (txtDataBatida.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Data Referência.";
                    return;
                }

                if (!ValidarData(txtDataBatida.Text.Trim()))
                {
                    labErro.Text = "Informe a Data Referência Válida.";
                    return;
                }

                //valida se é feriado não trabalhado
                var feriadoTrabalhado = rhController.ObterFeriado(ddlFilial.SelectedValue.Trim()).Where(p => p.DATA == Convert.ToDateTime(txtDataBatida.Text.Trim()) && p.TRABALHADO == 'N').ToList();
                if (feriadoTrabalhado != null && feriadoTrabalhado.Count() > 0)
                {
                    labErro.Text = "Não será possível realizar a Batida neste dia. Este dia está marcado como Feriado não Trabalhado. (" + feriadoTrabalhado[0].DESCRICAO + ").";
                    return;
                }

                ddlFilial.Enabled = false;
                txtDataBatida.Enabled = false;

                btCancelarBatida.Visible = true;
                btSalvar.Visible = true;

                List<SP_GERAR_BATIDA_DIAResult> batidaDia = new List<SP_GERAR_BATIDA_DIAResult>();
                int usuario = 0;
                string filial = "";
                DateTime dataBatida = DateTime.Now;

                filial = ddlFilial.SelectedValue.Trim();
                dataBatida = Convert.ToDateTime(txtDataBatida.Text);
                usuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                batidaDia = rhController.GerarBatidaPonto(filial, dataBatida, usuario);

                CarregarBatidas(filial, dataBatida);
                CarregarBatidasNovas(filial, dataBatida);

                //Validar se pode alterar
                var pBatida = gPontoBatida.Where(p => p.DATA == Convert.ToDateTime(txtDataBatida.Text.Trim()) && (p.STATUS != 'N' || p.STATUS == null)).Take(1).SingleOrDefault();
                if (pBatida != null)
                {
                    if (DateTime.Today > pBatida.DATA_INCLUSAO.AddDays(7).Date.AddHours(23).AddMinutes(59))
                    {
                        btSalvar.Visible = false;
                        GridViewRow footer = gvBatidaNova.FooterRow;
                        if (footer != null)
                            footer.Enabled = false;
                    }
                }

                labAjuda.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarBatidas(string filial, DateTime? data)
        {
            List<RH_PONTO_BATIDA> _pontoBatida = new List<RH_PONTO_BATIDA>();
            _pontoBatida = rhController.ObterPontoBatida(filial).Where(p => p.STATUS == null || p.STATUS != 'N').ToList();

            if (data != null)
                _pontoBatida = _pontoBatida.Where(p => p.DATA == Convert.ToDateTime(data)).ToList();

            gvBatida.DataSource = _pontoBatida.OrderBy(p => (p.RH_FUNCIONARIO == null) ? p.FUNCIONARIO_NOME : p.RH_FUNCIONARIO1.NOME);
            gvBatida.DataBind();

        }
        private void CarregarBatidasNovas(string filial, DateTime? data)
        {
            List<RH_PONTO_BATIDA> _pontoBatida = new List<RH_PONTO_BATIDA>();
            _pontoBatida = rhController.ObterPontoBatida(filial).Where(p => p.STATUS == 'N').ToList();

            if (data != null)
                _pontoBatida = _pontoBatida.Where(p => p.DATA == Convert.ToDateTime(data)).ToList();

            if (_pontoBatida == null || _pontoBatida.Count() <= 0)
                _pontoBatida.Add(new RH_PONTO_BATIDA { CODIGO = 0 });

            gvBatidaNova.DataSource = _pontoBatida.OrderBy(p => (p.RH_FUNCIONARIO == null) ? p.FUNCIONARIO_NOME : p.RH_FUNCIONARIO1.NOME);
            gvBatidaNova.DataBind();
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_PONTO_BATIDA batida = e.Row.DataItem as RH_PONTO_BATIDA;

                    coluna += 1;
                    if (batida != null)
                    {

                        if (batida.CODIGO == 0)
                        {
                            e.Row.Visible = false;
                            return;
                        }

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            if (batida.CODIGO > 0)
                                _litColuna.Text = coluna.ToString();

                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            if (batida.DATA != null && batida.CODIGO > 0)
                                _litData.Text = batida.DATA.ToString("dd/MM/yyyy");

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                        {
                            if (batida.RH_FUNCIONARIO != null)
                                _litNome.Text = batida.RH_FUNCIONARIO1.VENDEDOR + " - " + batida.RH_FUNCIONARIO1.NOME;
                            else
                                _litNome.Text = batida.FUNCIONARIO_NOME;
                        }
                        DropDownList _ddlTipoBatida = e.Row.FindControl("ddlTipoBatida") as DropDownList;
                        if (_ddlTipoBatida != null)
                        {
                            if (batida.CODIGO <= 0)
                                _ddlTipoBatida.Visible = false;

                            _ddlTipoBatida.DataSource = ObterTipoBatida();
                            _ddlTipoBatida.DataBind();

                            if (batida.RH_PONTO_BATIDA_TIPO != null)
                                _ddlTipoBatida.SelectedValue = batida.RH_PONTO_BATIDA_TIPO.ToString();
                        }

                        TextBox _txtEntrada1 = e.Row.FindControl("txtEntrada1") as TextBox;
                        if (_txtEntrada1 != null)
                        {
                            if (batida.CODIGO <= 0)
                                _txtEntrada1.Visible = false;

                            if (batida.ENTRADA1 != null)
                                _txtEntrada1.Text = Convert.ToDateTime(batida.ENTRADA1).ToString("HH:mm");

                        }

                        TextBox _txtSaida1 = e.Row.FindControl("txtSaida1") as TextBox;
                        if (_txtSaida1 != null)
                        {
                            if (batida.CODIGO <= 0)
                                _txtSaida1.Visible = false;

                            if (batida.SAIDA1 != null)
                                _txtSaida1.Text = Convert.ToDateTime(batida.SAIDA1).ToString("HH:mm");
                        }

                        TextBox _txtEntrada2 = e.Row.FindControl("txtEntrada2") as TextBox;
                        if (_txtEntrada2 != null)
                        {
                            if (batida.CODIGO <= 0)
                                _txtEntrada2.Visible = false;

                            if (batida.ENTRADA2 != null)
                                _txtEntrada2.Text = Convert.ToDateTime(batida.ENTRADA2).ToString("HH:mm");
                        }

                        TextBox _txtSaida2 = e.Row.FindControl("txtSaida2") as TextBox;
                        if (_txtSaida2 != null)
                        {
                            if (batida.CODIGO <= 0)
                                _txtSaida2.Visible = false;

                            if (batida.SAIDA2 != null)
                                _txtSaida2.Text = Convert.ToDateTime(batida.SAIDA2).ToString("HH:mm");
                        }

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                        {
                            if (batida.CODIGO <= 0)
                                _litTotal.Visible = false;

                            _litTotal.Text = CalcularPeriodo(batida.ENTRADA1, batida.SAIDA1, batida.ENTRADA2, batida.SAIDA2);
                        }

                        if (batida.RH_PONTO_BATIDA_TIPO != 1 && batida.RH_PONTO_BATIDA_TIPO != 0 && batida.RH_PONTO_BATIDA_TIPO != null) //NORMAL e "Selecione"
                        {
                            _txtEntrada1.Enabled = false;
                            _txtSaida1.Enabled = false;
                            _txtEntrada2.Enabled = false;
                            _txtSaida2.Enabled = false;
                        }

                        if (batida.STATUS == 'E')
                            e.Row.ForeColor = Color.Red;

                        if (DateTime.Today > batida.DATA_INCLUSAO.AddDays(7).Date.AddHours(23).AddMinutes(59))
                            e.Row.Enabled = false;

                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        if (_btExcluir != null)
                        {
                            _btExcluir.Visible = false;
                            _btExcluir.CommandArgument = batida.CODIGO.ToString();
                            if ((batida.DATA_ALTERACAO == null || batida.DATA_EXCLUSAO == null) && (batida.STATUS == 'N'))
                                _btExcluir.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvBatida_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if (gv != null)
            {
                GridViewRow footer = gv.FooterRow;
                if (footer != null)
                {
                    DropDownList _ddlTipoBatidaF = footer.FindControl("ddlTipoBatidaF") as DropDownList;
                    if (_ddlTipoBatidaF != null)
                    {
                        _ddlTipoBatidaF.DataSource = ObterTipoBatida();
                        _ddlTipoBatidaF.DataBind();
                    }

                }
            }
        }

        protected void ddlTipoBatida_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddlTipoBatida = (DropDownList)sender;
            if (_ddlTipoBatida != null)
            {
                GridViewRow row = (GridViewRow)_ddlTipoBatida.NamingContainer;
                if (row != null)
                {
                    Literal _litTotal = row.FindControl("litTotal") as Literal;

                    TextBox _txtEntrada1 = row.FindControl("txtEntrada1") as TextBox;
                    if (_txtEntrada1 == null)
                        _txtEntrada1 = row.FindControl("txtEntrada1F") as TextBox;
                    TextBox _txtSaida1 = row.FindControl("txtSaida1") as TextBox;
                    if (_txtSaida1 == null)
                        _txtSaida1 = row.FindControl("txtSaida1F") as TextBox;
                    TextBox _txtEntrada2 = row.FindControl("txtEntrada2") as TextBox;
                    if (_txtEntrada2 == null)
                        _txtEntrada2 = row.FindControl("txtEntrada2F") as TextBox;
                    TextBox _txtSaida2 = row.FindControl("txtSaida2") as TextBox;
                    if (_txtSaida2 == null)
                        _txtSaida2 = row.FindControl("txtSaida2F") as TextBox;

                    _txtEntrada1.Enabled = true;
                    _txtSaida1.Enabled = true;
                    _txtEntrada2.Enabled = true;
                    _txtSaida2.Enabled = true;
                    if (_ddlTipoBatida.SelectedValue != "1")
                    {
                        _txtEntrada1.Enabled = false;
                        _txtSaida1.Enabled = false;
                        _txtEntrada2.Enabled = false;
                        _txtSaida2.Enabled = false;
                    }

                    _txtEntrada1.Text = "";
                    _txtSaida1.Text = "";
                    _txtEntrada2.Text = "";
                    _txtSaida2.Text = "";

                    if (_litTotal != null)
                        _litTotal.Text = CalcularPeriodo(null, null, null, null);
                }
            }
        }
        protected void txtHora_TextChanged(object sender, EventArgs e)
        {
            DateTime? entrada1 = null;
            DateTime? saida1 = null;
            DateTime? entrada2 = null;
            DateTime? saida2 = null;
            string msg = "";

            try
            {

                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        TextBox _entrada1 = row.FindControl("txtEntrada1") as TextBox;
                        if (_entrada1 == null)
                            _entrada1 = row.FindControl("txtEntrada1F") as TextBox;

                        TextBox _saida1 = row.FindControl("txtSaida1") as TextBox;
                        if (_saida1 == null)
                            _saida1 = row.FindControl("txtSaida1F") as TextBox;

                        TextBox _entrada2 = row.FindControl("txtEntrada2") as TextBox;
                        if (_entrada2 == null)
                            _entrada2 = row.FindControl("txtEntrada2F") as TextBox;

                        TextBox _saida2 = row.FindControl("txtSaida2") as TextBox;
                        if (_saida2 == null)
                            _saida2 = row.FindControl("txtSaida2F") as TextBox;

                        Literal _litTotal = row.FindControl("litTotal") as Literal;

                        if (_entrada1.Text.Trim() != "")
                        {
                            if (!ValidarData(_entrada1.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da ENTRADA 1 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            entrada1 = Convert.ToDateTime(_entrada1.Text);
                        }

                        if (_saida1.Text.Trim() != "")
                        {
                            if (!ValidarData(_saida1.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da SAÍDA 1 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            saida1 = Convert.ToDateTime(_saida1.Text);
                        }

                        if (_entrada2.Text.Trim() != "")
                        {
                            if (!ValidarData(_entrada2.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da ENTRADA 2 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            entrada2 = Convert.ToDateTime(_entrada2.Text);
                        }

                        if (_saida2.Text.Trim() != "")
                        {
                            if (!ValidarData(_saida2.Text.Trim()))
                            {
                                txt.Text = "";
                                msg = "Hora da SAÍDA 2 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            saida2 = Convert.ToDateTime(_saida2.Text);
                        }

                        if (_litTotal != null)
                            _litTotal.Text = CalcularPeriodo(entrada1, saida1, entrada2, saida2);

                        if (txt.ID == "txtEntrada1" || txt.ID == "txtEntrada1F")
                            _saida1.Focus();
                        if (txt.ID == "txtSaida1" || txt.ID == "txtSaida1F")
                            _entrada2.Focus();
                        if (txt.ID == "txtEntrada2" || txt.ID == "txtEntrada2F")
                            _saida2.Focus();

                    }
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }

        }

        protected void btIncluirFuncionario_Click(object sender, EventArgs e)
        {
            GridViewRow footer = gvBatidaNova.FooterRow;
            string msg = "";

            DateTime? entrada1F = null;
            DateTime? saida1F = null;
            DateTime? entrada2F = null;
            DateTime? saida2F = null;

            try
            {
                if (footer != null)
                {
                    TextBox _txtNome = footer.FindControl("txtNomeF") as TextBox;
                    DropDownList _ddlTipoBatida = footer.FindControl("ddlTipoBatidaF") as DropDownList;
                    TextBox _entrada1F = footer.FindControl("txtEntrada1F") as TextBox;
                    TextBox _saida1F = footer.FindControl("txtSaida1F") as TextBox;
                    TextBox _entrada2F = footer.FindControl("txtEntrada2F") as TextBox;
                    TextBox _saida2F = footer.FindControl("txtSaida2F") as TextBox;

                    if (_txtNome.Text.Trim() == "")
                    {
                        msg = "Informe o NOME do Funcionário.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        return;
                    }

                    if (_ddlTipoBatida.SelectedValue == "-1" || _ddlTipoBatida.SelectedValue == "0")
                    {
                        msg = "Selecione o Tipo de Batida.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        return;
                    }

                    if (_ddlTipoBatida.SelectedValue == "1")
                    {
                        if (_entrada1F.Text == "" || _saida1F.Text == "" || _entrada2F.Text == "" || _saida2F.Text == "")
                        {
                            msg = "Informe o PERÍODO trabalhado corretamente!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (_entrada1F.Text.Trim() != "")
                        {
                            if (!ValidarData(_entrada1F.Text.Trim()))
                            {
                                msg = "Hora da ENTRADA 1 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            entrada1F = Convert.ToDateTime(_entrada1F.Text);
                        }

                        if (_saida1F.Text.Trim() != "")
                        {
                            if (!ValidarData(_saida1F.Text.Trim()))
                            {
                                msg = "Hora da SAÍDA 1 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            saida1F = Convert.ToDateTime(_saida1F.Text);
                        }

                        if (_entrada2F.Text.Trim() != "")
                        {
                            if (!ValidarData(_entrada2F.Text.Trim()))
                            {
                                msg = "Hora da ENTRADA 2 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            entrada2F = Convert.ToDateTime(_entrada2F.Text);
                        }

                        if (_saida2F.Text.Trim() != "")
                        {
                            if (!ValidarData(_saida2F.Text.Trim()))
                            {
                                msg = "Hora da SAÍDA 2 Inválida!";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }
                            saida2F = Convert.ToDateTime(_saida2F.Text);
                        }

                        if (!ValidarPeriodo(_entrada1F.Text, _saida1F.Text, _entrada2F.Text, _saida2F.Text))
                        {
                            labErro.Text = "O período da HORA está incorreto. Informe as horas em ORDEM.";
                            return;
                        }
                    }

                    //INSERIR
                    InserirBatida(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataBatida.Text), Convert.ToInt32(_ddlTipoBatida.SelectedValue), null, _txtNome.Text.Trim(), null, 'N', _entrada1F.Text, _saida1F.Text, _entrada2F.Text, _saida2F.Text);
                    coluna = gvBatida.Rows.Count;
                    CarregarBatidasNovas(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataBatida.Text));
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.Replace("'", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        int codigoPontoBatida = 0;
                        Int32.TryParse(gvBatidaNova.DataKeys[row.RowIndex].Value.ToString(), out codigoPontoBatida);

                        if (codigoPontoBatida > 0)
                        {
                            var pontoBatida = rhController.ObterPontoBatida(codigoPontoBatida);
                            if (pontoBatida != null)
                            {
                                pontoBatida.DATA_EXCLUSAO = DateTime.Now;
                                pontoBatida.USUARIO_EXCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                pontoBatida.STATUS = 'E';
                                rhController.AtualizarPontoBatida(pontoBatida);
                                //CarregarBatidas(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataBatida.Text.Trim()));
                                CarregarBatidasNovas(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataBatida.Text.Trim()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    string msg = "";
                    labMsg.Text = "";

                    int codigoPontoBatida = 0;

                    Literal _litNome = null;
                    DropDownList _ddlTipoBatida = null;
                    TextBox _txtEntrada1 = null;
                    TextBox _txtSaida1 = null;
                    TextBox _txtEntrada2 = null;
                    TextBox _txtSaida2 = null;
                    foreach (GridViewRow row in gvBatida.Rows)
                    {
                        row.BackColor = Color.White;

                        codigoPontoBatida = Convert.ToInt32(gvBatida.DataKeys[row.RowIndex].Value);
                        RH_PONTO_BATIDA _pontoBatida = rhController.ObterPontoBatida(codigoPontoBatida);

                        _ddlTipoBatida = row.FindControl("ddlTipoBatida") as DropDownList;

                        if (_pontoBatida.STATUS != 'N' && _ddlTipoBatida.SelectedValue != "0")
                        {

                            _litNome = row.FindControl("litNome") as Literal;
                            _txtEntrada1 = row.FindControl("txtEntrada1") as TextBox;
                            _txtSaida1 = row.FindControl("txtSaida1") as TextBox;
                            _txtEntrada2 = row.FindControl("txtEntrada2") as TextBox;
                            _txtSaida2 = row.FindControl("txtSaida2") as TextBox;

                            /*if (_ddlTipoBatida.SelectedValue == "0")
                            {
                                msg = "Selecione o Tipo de Batida.";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                row.BackColor = Color.MistyRose;
                                return;
                            }*/

                            if (_ddlTipoBatida.SelectedValue == "1")
                            {
                                if (_txtEntrada1.Text == "" || _txtSaida1.Text == "" || _txtEntrada2.Text == "" || _txtSaida2.Text == "")
                                {
                                    msg = "Informe o PERÍODO trabalhado corretamente!";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                    row.BackColor = Color.MistyRose;
                                    return;
                                }

                                if (_txtEntrada1.Text.Trim() != "")
                                {
                                    if (!ValidarData(_txtEntrada1.Text.Trim()))
                                    {
                                        msg = "Hora da ENTRADA 1 Inválida!";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                        row.BackColor = Color.MistyRose;
                                        return;
                                    }
                                    //entrada1F = Convert.ToDateTime(_txtEntrada1.Text);
                                }

                                if (_txtSaida1.Text.Trim() != "")
                                {
                                    if (!ValidarData(_txtSaida1.Text.Trim()))
                                    {
                                        msg = "Hora da SAÍDA 1 Inválida!";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                        row.BackColor = Color.MistyRose;
                                        return;
                                    }
                                    //saida1F = Convert.ToDateTime(_txtSaida1.Text);
                                }

                                if (_txtEntrada2.Text.Trim() != "")
                                {
                                    if (!ValidarData(_txtEntrada2.Text.Trim()))
                                    {
                                        msg = "Hora da ENTRADA 2 Inválida!";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                        row.BackColor = Color.MistyRose;
                                        return;
                                    }
                                    //entrada2F = Convert.ToDateTime(_txtEntrada2.Text);
                                }

                                if (_txtSaida2.Text.Trim() != "")
                                {
                                    if (!ValidarData(_txtSaida2.Text.Trim()))
                                    {
                                        msg = "Hora da SAÍDA 2 Inválida!";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                        row.BackColor = Color.MistyRose;
                                        return;
                                    }
                                    //saida2F = Convert.ToDateTime(_txtSaida2.Text);
                                }

                                if (!ValidarPeriodo(_txtEntrada1.Text, _txtSaida1.Text, _txtEntrada2.Text, _txtSaida2.Text))
                                {
                                    msg = "O período da HORA está incorreto. Informe as horas em ORDEM.";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                    row.BackColor = Color.MistyRose;
                                    return;
                                }
                            }

                            int? pontoPeriodoTrab = null;

                            if (_ddlTipoBatida.SelectedValue != "99")
                            {
                                int diaSemana = ((int)Convert.ToDateTime(txtDataBatida.Text).DayOfWeek) + 1;
                                if (_pontoBatida.RH_FUNCIONARIO != null)
                                {
                                    var perTrab = rhController.ObterFuncionarioPeriodoTrab(_pontoBatida.RH_FUNCIONARIO.ToString()).Where(p => diaSemana >= p.RH_PONTO_PERIODO_TRAB1.DIA_INICIAL && diaSemana <= p.RH_PONTO_PERIODO_TRAB1.DIA_FINAL).ToList();
                                    if (perTrab != null && perTrab.Count() > 0)
                                        pontoPeriodoTrab = perTrab[0].RH_PONTO_PERIODO_TRAB;

                                    if (pontoPeriodoTrab == null)
                                    {
                                        msg = "Entre em contato com o RH para cadastrar o Horário de Trabalho de " + _litNome.Text.Trim() + ".";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                        row.BackColor = Color.Pink;
                                        return;
                                    }
                                }
                            }

                            //SALVAR
                            _pontoBatida.CODIGO_FILIAL = ddlFilial.SelectedValue;
                            _pontoBatida.DATA = Convert.ToDateTime(txtDataBatida.Text.Trim());
                            //_pontoBatida.FUNCIONARIO = 0
                            if (pontoPeriodoTrab != null && pontoPeriodoTrab > 0)
                                _pontoBatida.RH_PONTO_PERIODO_TRAB = pontoPeriodoTrab;
                            _pontoBatida.RH_PONTO_BATIDA_TIPO = Convert.ToInt32(_ddlTipoBatida.SelectedValue);
                            if (_txtEntrada1.Text.Trim() != "")
                                _pontoBatida.ENTRADA1 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtEntrada1.Text);
                            if (_txtSaida1.Text.Trim() != "")
                                _pontoBatida.SAIDA1 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtSaida1.Text);
                            if (_txtEntrada2.Text.Trim() != "")
                                _pontoBatida.ENTRADA2 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtEntrada2.Text);
                            if (_txtSaida2.Text.Trim() != "")
                                _pontoBatida.SAIDA2 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + _txtSaida2.Text);
                            if (_pontoBatida.STATUS == null)
                                _pontoBatida.STATUS = 'B';
                            _pontoBatida.DATA_ALTERACAO = DateTime.Now;
                            _pontoBatida.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                            rhController.AtualizarPontoBatida(_pontoBatida);
                        }
                    }

                    //CarregarBatidas(ddlFilial.SelectedValue, Convert.ToDateTime(txtDataBatida.Text.Trim()));

                    /*litPopUp.Text = txtDataBatida.Text + "<br />" + ddlFilial.SelectedItem.Text.Trim().ToUpper();
                    labMensagem.Text = "FECHADA COM SUCESSO";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('rh_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('rh_menu.aspx', '_self'); } } }); });", true);
                    dialogPai.Visible = true;*/

                    labErro.Text = "Batida Atualizada com Sucesso.";
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }

        private void EnviarEmail(List<RH_PONTO_BATIDA> batida)
        {
            /*email_envio email = new email_envio();
            email.ASSUNTO = "INTRANET: BATIDA DE PONTO";
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = MontarCorpoEmail(batida);

            List<string> destinatario = new List<string>();
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(2, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();*/
        }
        private string MontarCorpoEmail(List<RH_PONTO_BATIDA> batida)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");

            return sb.ToString();
        }

    }
}
