using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;

namespace Relatorios
{
    public partial class rh_ponto_batida_pendente_editar : System.Web.UI.Page
    {
        RHController rhController = new RHController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoBatida = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("rh_menu.aspx");

                codigoBatida = Convert.ToInt32(Request.QueryString["p"].ToString());
                RH_PONTO_BATIDA _batida = rhController.ObterPontoBatida(codigoBatida);
                if (_batida == null)
                    Response.Redirect("rh_menu.aspx");

                CarregarFilial("");
                CarregarTipoBatida("");
                CarregarFuncionario("", "");
                hidCodigoBatida.Value = _batida.CODIGO.ToString();

                txtFilial.Text = new BaseController().BuscaFilialCodigo(Convert.ToInt32(_batida.CODIGO_FILIAL)).FILIAL.Trim();
                ddlFuncionarioN.SelectedValue = _batida.CODIGO_FILIAL;
                txtDataRef.Text = _batida.DATA.ToString("dd/MM/yyyy");
                txtDataReferenciaNova.Text = _batida.DATA.ToString("dd/MM/yyyy");
                txtFuncionario.Text = (_batida.RH_FUNCIONARIO == null) ? _batida.FUNCIONARIO_NOME : _batida.RH_FUNCIONARIO1.NOME;
                txtTipoBatida.Text = _batida.RH_PONTO_BATIDA_TIPO1.DESCRICAO;
                ddlTipoBatidaN.SelectedValue = _batida.RH_PONTO_BATIDA_TIPO.ToString();
                if (_batida.ENTRADA1 != null)
                {
                    txtEntrada1.Text = Convert.ToDateTime(_batida.ENTRADA1).ToString("HH:mm");
                    txtEntrada1N.Text = Convert.ToDateTime(_batida.ENTRADA1).ToString("HH:mm");
                }
                if (_batida.SAIDA1 != null)
                {
                    txtSaida1.Text = Convert.ToDateTime(_batida.SAIDA1).ToString("HH:mm");
                    txtSaida1N.Text = Convert.ToDateTime(_batida.SAIDA1).ToString("HH:mm");
                }
                if (_batida.ENTRADA2 != null)
                {
                    txtEntrada2.Text = Convert.ToDateTime(_batida.ENTRADA2).ToString("HH:mm");
                    txtEntrada2N.Text = Convert.ToDateTime(_batida.ENTRADA2).ToString("HH:mm");
                }
                if (_batida.SAIDA2 != null)
                {
                    txtSaida2.Text = Convert.ToDateTime(_batida.SAIDA2).ToString("HH:mm");
                    txtSaida2N.Text = Convert.ToDateTime(_batida.SAIDA2).ToString("HH:mm");
                }

                txtTotal.Text = CalcularPeriodo(_batida.ENTRADA1, _batida.SAIDA1, _batida.ENTRADA2, _batida.SAIDA2);
                txtTotalN.Text = CalcularPeriodo(_batida.ENTRADA1, _batida.SAIDA1, _batida.ENTRADA2, _batida.SAIDA2);
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial(string codigoFilial)
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilialN.DataSource = lstFilial;
                    ddlFilialN.DataBind();

                    if (codigoFilial != "")
                    {
                        ddlFilialN.SelectedValue = codigoFilial;
                        ddlFilialN_SelectedIndexChanged(null, null);
                    }
                }
            }
        }
        protected void ddlFilialN_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CarregarFuncionario(ddlFilialN.SelectedValue, "");
        }
        private void CarregarFuncionario(string filial, string codigoFun)
        {
            ddlFuncionarioN.DataSource = ObterFuncionario(filial);
            ddlFuncionarioN.DataBind();

            if (codigoFun != "")
                ddlFuncionarioN.SelectedValue = codigoFun;
        }
        private void CarregarTipoBatida(string codigoTipoBatida)
        {
            ddlTipoBatidaN.DataSource = ObterTipoBatida();
            ddlTipoBatidaN.DataBind();

            if (codigoTipoBatida != "")
                ddlTipoBatidaN.SelectedValue = codigoTipoBatida;
        }

        private List<RH_PONTO_BATIDA_TIPO> ObterTipoBatida()
        {
            var tipoBatida = rhController.ObterBatidaTipo().Where(p => p.STATUS == 'A').ToList();
            return tipoBatida;
        }
        private List<SP_OBTER_FUNCIONARIOResult> ObterFuncionario(string filial)
        {
            int codigoPerfil = 0;
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            List<SP_OBTER_FUNCIONARIOResult> funcionario = new List<SP_OBTER_FUNCIONARIOResult>();

            if (usuario != null)
            {
                codigoPerfil = usuario.CODIGO_PERFIL;

                if (codigoPerfil == 1 || codigoPerfil == 11)
                    filial = "";

                funcionario = rhController.ObterFuncionario(null, "", filial, "", "").ToList().Select(c => new SP_OBTER_FUNCIONARIOResult()
                {
                    CODIGO = c.CODIGO,
                    VENDEDOR = c.VENDEDOR,
                    NOME = string.Format("{0} - {1}", c.NOME.Trim(), c.VENDEDOR.Trim()),
                    VENDEDOR_APELIDO = c.NOME //USADO APENAS PARA ORDENACAO
                }).Distinct().OrderBy(c => c.VENDEDOR_APELIDO).ToList(); ;
            }

            funcionario.Insert(0, new SP_OBTER_FUNCIONARIOResult { CODIGO = 0, NOME = "Selecione" });
            return funcionario;
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

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
        #endregion

        #region "ACOES"
        protected void ddlTipoBatidaN_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddlTipoBatida = (DropDownList)sender;
            if (_ddlTipoBatida != null)
            {
                txtEntrada1N.Enabled = true;
                txtSaida1N.Enabled = true;
                txtEntrada2N.Enabled = true;
                txtSaida2N.Enabled = true;
                if (_ddlTipoBatida.SelectedValue != "1")
                {
                    txtEntrada1N.Enabled = false;
                    txtSaida1N.Enabled = false;
                    txtEntrada2N.Enabled = false;
                    txtSaida2N.Enabled = false;
                }

                txtEntrada1N.Text = "";
                txtSaida1N.Text = "";
                txtEntrada2N.Text = "";
                txtSaida2N.Text = "";

                txtTotalN.Text = CalcularPeriodo(null, null, null, null);
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

                    if (txtEntrada1N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtEntrada1N.Text.Trim()))
                        {
                            txt.Text = "";
                            msg = "Hora da ENTRADA 1 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }
                        entrada1 = Convert.ToDateTime(txtEntrada1N.Text);
                    }

                    if (txtSaida1N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtSaida1N.Text.Trim()))
                        {
                            txt.Text = "";
                            msg = "Hora da SAÍDA 1 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }
                        saida1 = Convert.ToDateTime(txtSaida1N.Text);
                    }

                    if (txtEntrada2N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtEntrada2N.Text.Trim()))
                        {
                            txt.Text = "";
                            msg = "Hora da ENTRADA 2 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }
                        entrada2 = Convert.ToDateTime(txtEntrada2N.Text);
                    }

                    if (txtSaida2N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtSaida2N.Text.Trim()))
                        {
                            txt.Text = "";
                            msg = "Hora da SAÍDA 2 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }
                        saida2 = Convert.ToDateTime(txtSaida2N.Text);
                    }

                    if (txtTotalN != null)
                        txtTotalN.Text = CalcularPeriodo(entrada1, saida1, entrada2, saida2);

                    if (txt.ID == "txtEntrada1N")
                        txtSaida1N.Focus();
                    if (txt.ID == "txtSaida1N")
                        txtEntrada2N.Focus();
                    if (txt.ID == "txtEntrada2N")
                        txtSaida2N.Focus();

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }

        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            int codigoBatida = 0;
            string msg = "";
            RH_PONTO_BATIDA _pontoBatida = null;
            try
            {
                labErro.Text = "";

                //Obter referencia para Obter o Codigo da Batida
                codigoBatida = Convert.ToInt32(hidCodigoBatida.Value);
                _pontoBatida = rhController.ObterPontoBatida(codigoBatida);

                if (ddlFilialN.SelectedValue == "")
                {
                    msg = "Selecione a Filial.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    labErro.Text = msg;
                    return;
                }

                if (ddlFuncionarioN.SelectedValue == "0")
                {
                    msg = "Selecione o Funcionário.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    labErro.Text = msg;
                    return;
                }

                if (ddlTipoBatidaN.SelectedValue == "0")
                {
                    msg = "Selecione o Tipo de Batida.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    labErro.Text = msg;
                    return;
                }

                if (ddlTipoBatidaN.SelectedValue == "99")
                {
                    msg = "Tipo de Batida não pode ser PENDENTE.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    labErro.Text = msg;
                    return;
                }

                if (ddlTipoBatidaN.SelectedValue == "1" || ddlTipoBatidaN.SelectedValue == "6")
                {
                    if (txtEntrada1N.Text == "" || txtSaida1N.Text == "" || txtEntrada2N.Text == "" || txtSaida2N.Text == "")
                    {
                        msg = "Informe o PERÍODO trabalhado corretamente!";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        labErro.Text = msg;
                        return;
                    }

                    if (txtEntrada1N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtEntrada1N.Text.Trim()))
                        {
                            msg = "Hora da ENTRADA 1 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErro.Text = msg;
                            return;
                        }
                    }

                    if (txtSaida1N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtSaida1N.Text.Trim()))
                        {
                            msg = "Hora da SAÍDA 1 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErro.Text = msg;
                            return;
                        }
                    }

                    if (txtEntrada2N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtEntrada2N.Text.Trim()))
                        {
                            msg = "Hora da ENTRADA 2 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErro.Text = msg;
                            return;
                        }
                    }

                    if (txtSaida2N.Text.Trim() != "")
                    {
                        if (!ValidarData(txtSaida2N.Text.Trim()))
                        {
                            msg = "Hora da SAÍDA 2 Inválida!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            labErro.Text = msg;
                            return;
                        }
                    }

                    if (!ValidarPeriodo(txtEntrada1N.Text, txtSaida1N.Text, txtEntrada2N.Text, txtSaida2N.Text))
                    {
                        msg = "O período da HORA está incorreto. Informe as horas em ORDEM.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                        labErro.Text = msg;
                        return;
                    }
                }

                int? pontoPeriodoTrab = null;
                int diaSemana = ((int)Convert.ToDateTime(txtDataReferenciaNova.Text.Trim()).DayOfWeek) + 1;
                var perTrab = rhController.ObterFuncionarioPeriodoTrab(ddlFuncionarioN.SelectedValue).Where(p => diaSemana >= p.RH_PONTO_PERIODO_TRAB1.DIA_INICIAL && diaSemana <= p.RH_PONTO_PERIODO_TRAB1.DIA_FINAL).ToList();
                if (perTrab != null && perTrab.Count() > 0)
                    pontoPeriodoTrab = perTrab[0].RH_PONTO_PERIODO_TRAB;

                if (pontoPeriodoTrab == null)
                {
                    msg = "Funcionário não possui Período de Trabalho cadastrado.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    labErro.Text = msg;
                    return;
                }

                var verificaBatida = rhController.ObterPontoBatida().Where(p => p.RH_FUNCIONARIO != null && p.RH_FUNCIONARIO == Convert.ToInt32(ddlFuncionarioN.SelectedValue) && p.DATA == _pontoBatida.DATA && p.STATUS != 'E').SingleOrDefault();
                if (verificaBatida != null && codigoBatida != verificaBatida.CODIGO)
                {
                    msg = "Funcionário já possui batida neste dia.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    labErro.Text = msg;
                    return;
                }

                _pontoBatida.CODIGO_FILIAL = ddlFilialN.SelectedValue;
                _pontoBatida.RH_FUNCIONARIO = Convert.ToInt32(ddlFuncionarioN.SelectedValue);
                _pontoBatida.RH_PONTO_BATIDA_TIPO = Convert.ToInt32(ddlTipoBatidaN.SelectedValue);
                _pontoBatida.RH_PONTO_PERIODO_TRAB = pontoPeriodoTrab;
                _pontoBatida.ENTRADA1 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + txtEntrada1N.Text.Trim());
                _pontoBatida.SAIDA1 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + txtSaida1N.Text.Trim());
                _pontoBatida.ENTRADA2 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + txtEntrada2N.Text.Trim());
                _pontoBatida.SAIDA2 = Convert.ToDateTime(_pontoBatida.DATA.ToString("yyyy-MM-dd") + " " + txtSaida2N.Text.Trim());
                _pontoBatida.STATUS = 'B';
                _pontoBatida.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _pontoBatida.DATA_ALTERACAO = DateTime.Now;

                rhController.AtualizarPontoBatida(_pontoBatida);

                labErro.Text = "Batida atualizada com sucesso.";
                btSalvar.Visible = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }
}
