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
    public partial class jur_processo_cad : System.Web.UI.Page
    {
        JuridicoController jurController = new JuridicoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataRecebimento.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataJulgamento.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataCondenacao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                int codigoProcesso = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "")
                    Response.Redirect("jur_menu.aspx");

                codigoProcesso = Convert.ToInt32(Request.QueryString["p"].ToString());
                JUR_PROCESSO _processo = jurController.ObterProcesso(codigoProcesso);

                CarregarTipoProcesso();
                CarregarInstancia();

                if (_processo != null)
                {
                    hidProcesso.Value = codigoProcesso.ToString();

                    txtNumeroProcesso.Text = _processo.NUMERO.ToUpper();
                    txtNumeroProcesso.Enabled = false;
                    txtDataRecebimento.Text = _processo.DATA_RECEBIMENTO.ToString("dd/MM/yyyy");
                    txtRequerente.Text = _processo.REQUERENTE.ToUpper();
                    txtCargo.Text = _processo.CARGO;
                    ddlTipoProcesso.SelectedValue = _processo.JUR_TIPO_PROCESSO.ToString();

                    hrefVoltar.HRef = "jur_processo_altera.aspx";
                    labTitulo.Text = "Alterar Processo";
                    labTituloMenu.Text = labTitulo.Text;

                    RecarregarProcessoInstancia();

                    btExcluirProcesso.Visible = true;
                    btSalvar.Visible = true;
                    chkEmail.Visible = true;
                }

                dialogPai.Visible = false;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarTipoProcesso()
        {
            List<JUR_TIPO_PROCESSO> _processo = jurController.ObterTipoProcesso().Where(p => p.STATUS == 'A').ToList();

            _processo.Insert(0, new JUR_TIPO_PROCESSO { CODIGO = 0, DESCRICAO = "Selecione" });

            if (_processo != null)
            {
                ddlTipoProcesso.DataSource = _processo;
                ddlTipoProcesso.DataBind();
            }
        }
        private void CarregarInstancia()
        {
            List<JUR_TIPO_INSTANCIA> _instancia = jurController.ObterTipoInstancia().Where(p => p.STATUS == 'A').ToList();
            _instancia.Insert(0, new JUR_TIPO_INSTANCIA { CODIGO = 0, DESCRICAO = "Selecione" });
            if (_instancia != null)
            {
                ddlTipoInstancia.DataSource = _instancia.OrderBy(p => p.ORDEM);
                ddlTipoInstancia.DataBind();
            }
        }
        private bool ValidarCampos(bool inclusao)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labNumero.ForeColor = _OK;
            if (txtNumeroProcesso.Text.Trim() == "")
            {
                labNumero.ForeColor = _notOK;
                retorno = false;
            }

            labDataRecebimento.ForeColor = _OK;
            if (txtDataRecebimento.Text.Trim() == "")
            {
                labDataRecebimento.ForeColor = _notOK;
                retorno = false;
            }


            labDataJulgamento.ForeColor = _OK;
            if (txtDataJulgamento.Text.Trim() == "" && inclusao)
            {
                labDataJulgamento.ForeColor = _notOK;
                retorno = false;
            }

            labRequerente.ForeColor = _OK;
            if (txtRequerente.Text.Trim() == "")
            {
                labRequerente.ForeColor = _notOK;
                retorno = false;
            }

            labCargo.ForeColor = _OK;
            if (txtCargo.Text.Trim() == "")
            {
                labCargo.ForeColor = _notOK;
                retorno = false;
            }

            labTipoProcesso.ForeColor = _OK;
            if (ddlTipoProcesso.SelectedValue.Trim() == "" || ddlTipoProcesso.SelectedValue.Trim() == "0")
            {
                labTipoProcesso.ForeColor = _notOK;
                retorno = false;
            }

            labTipoInstancia.ForeColor = _OK;
            if ((ddlTipoInstancia.SelectedValue.Trim() == "" || ddlTipoInstancia.SelectedValue.Trim() == "0") && inclusao)
            {
                labTipoInstancia.ForeColor = _notOK;
                retorno = false;
            }
            return retorno;
        }
        private bool ValidarInstancia(int codigoProcesso, int codigoInstancia)
        {
            bool retorno = true;

            int ordemInstanciaSelecionada = 0;
            ordemInstanciaSelecionada = jurController.ObterTipoInstancia().Where(p => p.CODIGO == codigoInstancia).SingleOrDefault().ORDEM;

            int ordemInstanciaUltima = 0;
            var ultimaInstancia = jurController.ObterProcessoInstancia().Where(p => p.JUR_PROCESSO == codigoProcesso);
            if (ultimaInstancia != null && ultimaInstancia.Count() > 0)
                ordemInstanciaUltima = ultimaInstancia.Max(x => x.JUR_TIPO_INSTANCIA1.ORDEM);

            if (ordemInstanciaSelecionada <= ordemInstanciaUltima)
                retorno = false;

            return retorno;
        }
        #endregion

        #region "INCLUSAO"
        protected void btIncluirProcesso_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                if (!ValidarCampos(true))
                {
                    labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                //Valida se é alteração
                if (hidProcesso.Value != "")
                {
                    if (!ValidarInstancia(Convert.ToInt32(hidProcesso.Value), Convert.ToInt32(ddlTipoInstancia.SelectedValue)))
                    {
                        labErro.Text = "Selecione uma Instância acima da Última cadastrada.";
                        return;
                    }
                }
                int codigoProcesso = 0;
                codigoProcesso = InserirProcesso();
                InserirProcessoInstancia(codigoProcesso);

                if (hidProcesso.Value.Trim() == "")
                {
                    labPopUp.Text = txtNumeroProcesso.Text;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('jur_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('jur_menu.aspx', '_self'); }, 'Processo Novo': function () { window.open('jur_processo_cad.aspx?p=0', '_self'); } } }); });", true);
                    dialogPai.Visible = true;
                }
                else
                {
                    RecarregarProcessoInstancia();
                }

                txtDataJulgamento.Text = "";
                txtDataCondenacao.Text = "";
                txtFormaPgto.Text = "";
                txtObservacao.Text = "";
                ddlTipoInstancia.SelectedValue = "0";

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private int InserirProcesso()
        {
            JUR_PROCESSO _processo = null;
            int codigoProcesso = 0;

            //Salvar PEDIDO
            _processo = new JUR_PROCESSO();
            _processo.NUMERO = txtNumeroProcesso.Text.Trim().ToUpper();
            _processo.DATA_RECEBIMENTO = Convert.ToDateTime(txtDataRecebimento.Text.Trim());
            _processo.REQUERENTE = txtRequerente.Text.Trim().ToUpper();
            _processo.CARGO = txtCargo.Text.Trim().ToUpper();
            _processo.JUR_TIPO_PROCESSO = Convert.ToInt32(ddlTipoProcesso.SelectedValue);
            _processo.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            _processo.DATA_INCLUSAO = DateTime.Now;
            _processo.STATUS = 'A';

            if (hidProcesso.Value == "")
            {
                codigoProcesso = jurController.InserirProcesso(_processo);
            }
            else
            {
                codigoProcesso = Convert.ToInt32(hidProcesso.Value);
                _processo.CODIGO = codigoProcesso;
                jurController.AtualizarProcesso(_processo);
            }

            return codigoProcesso;

        }
        private void InserirProcessoInstancia(int codigoProcesso)
        {
            JUR_PROCESSO_INSTANCIA _instancia = null;
            //Inserir INSTÂNCIA
            if (codigoProcesso > 0)
            {
                _instancia = new JUR_PROCESSO_INSTANCIA();
                _instancia.JUR_PROCESSO = codigoProcesso;
                _instancia.JUR_TIPO_INSTANCIA = Convert.ToInt32(ddlTipoInstancia.SelectedValue);

                if (txtDataJulgamento.Text.Trim() != "")
                    _instancia.DATA_JULGAMENTO = Convert.ToDateTime(txtDataJulgamento.Text.Trim());
                if (txtDataCondenacao.Text.Trim() != "")
                    _instancia.DATA_CONDENACAO = Convert.ToDateTime(txtDataCondenacao.Text.Trim());
                if (txtFormaPgto.Text.Trim() != "")
                    _instancia.FORMA_PGTO = txtFormaPgto.Text.Trim().ToUpper();
                if (txtObservacao.Text.Trim() != "")
                    _instancia.OBSERVACAO = txtObservacao.Text.Trim().ToUpper();

                _instancia.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _instancia.DATA_INCLUSAO = DateTime.Now;
                _instancia.STATUS = 'A';

                jurController.InserirProcessoInstancia(_instancia);
            }

        }
        #endregion

        #region "GRID PROCESSO"
        private void RecarregarProcessoInstancia()
        {
            List<JUR_PROCESSO_INSTANCIA> _processoInstancia = new List<JUR_PROCESSO_INSTANCIA>();
            _processoInstancia = jurController.ObterProcessoInstancia().Where(p => p.JUR_PROCESSO == Convert.ToInt32(hidProcesso.Value)).ToList();

            gvProcesso.DataSource = _processoInstancia;
            gvProcesso.DataBind();

        }
        protected void gvProcesso_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    JUR_PROCESSO_INSTANCIA pInstancia = e.Row.DataItem as JUR_PROCESSO_INSTANCIA;

                    if (pInstancia != null)
                    {
                        Literal _litInstancia = e.Row.FindControl("litInstancia") as Literal;
                        if (_litInstancia != null)
                            _litInstancia.Text = pInstancia.JUR_TIPO_INSTANCIA1.DESCRICAO;

                        Literal _litDataJulgamento = e.Row.FindControl("litDataJulgamento") as Literal;
                        if (_litDataJulgamento != null)
                            _litDataJulgamento.Text = Convert.ToDateTime(pInstancia.DATA_JULGAMENTO).ToString("dd/MM/yyyy");

                        TextBox _txtDataCondenacao = e.Row.FindControl("txtDataCondenacao") as TextBox;
                        if (_txtDataCondenacao != null)
                        {
                            _txtDataCondenacao.Text = (pInstancia.DATA_CONDENACAO == null) ? "" : Convert.ToDateTime(pInstancia.DATA_CONDENACAO).ToString("dd/MM/yyyy");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + _txtDataCondenacao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
                        }

                        TextBox _txtFormaPgto = e.Row.FindControl("txtFormaPgto") as TextBox;
                        if (_txtFormaPgto != null)
                            _txtFormaPgto.Text = (pInstancia.FORMA_PGTO == null) ? "" : pInstancia.FORMA_PGTO;

                        TextBox _txtObservacao = e.Row.FindControl("txtObservacao") as TextBox;
                        if (_txtObservacao != null)
                            _txtObservacao.Text = (pInstancia.OBSERVACAO == null) ? "" : pInstancia.OBSERVACAO;

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = pInstancia.CODIGO.ToString();
                    }
                }
            }
        }
        protected void txt_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        int codigoProcessoInstancia = 0;
                        codigoProcessoInstancia = Convert.ToInt32(gvProcesso.DataKeys[row.RowIndex].Value);
                        if (codigoProcessoInstancia > 0)
                        {
                            JUR_PROCESSO_INSTANCIA _processoInstancia = new JUR_PROCESSO_INSTANCIA();
                            _processoInstancia = jurController.ObterProcessoInstancia(codigoProcessoInstancia);
                            if (_processoInstancia != null)
                            {
                                TextBox _txtDataCondenacao = row.FindControl("txtDataCondenacao") as TextBox;
                                TextBox _txtFormaPgto = row.FindControl("txtFormaPgto") as TextBox;
                                TextBox _txtObservacao = row.FindControl("txtObservacao") as TextBox;

                                DateTime? _dataCondenacao = null;
                                if (_txtDataCondenacao != null && _txtDataCondenacao.Text.Trim() != "")
                                    _dataCondenacao = Convert.ToDateTime(_txtDataCondenacao.Text);

                                string _formaPgto = null;
                                if (_txtFormaPgto != null)
                                    _formaPgto = _txtFormaPgto.Text.Trim().ToUpper();

                                string _obs = null;
                                if (_txtObservacao != null)
                                    _obs = _txtObservacao.Text.Trim().ToUpper();

                                _processoInstancia.DATA_CONDENACAO = _dataCondenacao;
                                _processoInstancia.FORMA_PGTO = _formaPgto;
                                _processoInstancia.OBSERVACAO = _obs;
                                _processoInstancia.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                _processoInstancia.DATA_INCLUSAO = DateTime.Now;

                                jurController.AtualizarProcessoInstancia(_processoInstancia);

                                RecarregarProcessoInstancia();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    int codigoProcessoInstancia = 0;
                    codigoProcessoInstancia = Convert.ToInt32(bt.CommandArgument);
                    jurController.ExcluirProcessoInstancia(codigoProcessoInstancia);

                    RecarregarProcessoInstancia();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
                    if (!ValidarCampos(false))
                    {
                        labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                        return;
                    }

                    labMsg.Text = "";
                    InserirProcesso();

                    if (chkEmail.Checked)
                        EnviarEmail(jurController.ObterProcesso(Convert.ToInt32(hidProcesso.Value)));

                    labPopUp.Text = txtNumeroProcesso.Text;
                    labMensagem.Text = "ALTERADO COM SUCESSO";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('jur_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('jur_menu.aspx', '_self'); }, 'Processo Novo': function () { window.open('jur_processo_cad.aspx?p=0', '_self'); } } }); });", true);
                    dialogPai.Visible = true;

                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        protected void btExcluirProcesso_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    labMsg.Text = "";
                    JUR_PROCESSO processo = null;

                    int codigoProcesso = 0;
                    codigoProcesso = (hidProcesso.Value.Trim() == "") ? 0 : Convert.ToInt32(hidProcesso.Value);

                    processo = new JUR_PROCESSO();
                    processo.CODIGO = codigoProcesso;
                    processo.STATUS = 'E';
                    jurController.ExcluirProcesso(processo);

                    labPopUp.Text = txtNumeroProcesso.Text;
                    labMensagem.Text = "EXCLUÍDO COM SUCESSO";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('jur_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('jur_menu.aspx', '_self'); }, 'Processo Novo': function () { window.open('jur_processo_cad.aspx?p=0', '_self'); } } }); });", true);
                    dialogPai.Visible = true;
                }
                catch (Exception ex)
                {
                    labMsg.Text = ex.Message;
                }
            }
        }

        private void EnviarEmail(JUR_PROCESSO processo)
        {
            email_envio email = new email_envio();
            email.ASSUNTO = "INTRANET: Jurídico - Processo Alterado: " + txtNumeroProcesso.Text.Trim().ToUpper();
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = MontarCorpoEmail(processo);

            List<string> destinatario = new List<string>();
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(2, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(JUR_PROCESSO processo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>PROCESSO JURIDICO</title>");
            sb.Append("    <meta charset='UTF-8'>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <span style='font-family:Calibri; font-size:medium;'>PROCESSO JURÍDICO</span>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <div id='divProcesso' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 1000pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            sb.Append("                            width: 750pt'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 150px; font-family:Calibri;'>");
            sb.Append("                                    Número:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + processo.NUMERO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Requerente:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + processo.REQUERENTE);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cargo:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + processo.CARGO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Tipo de Processo:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + processo.JUR_TIPO_PROCESSO1.DESCRICAO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        <table border='0' width='100%'>");
            sb.Append("                            <tr style='background: #ccc;'>");
            sb.Append("                                <td style='width: 250px; font-family:Calibri;'>");
            sb.Append("                                    Fase");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width: 180px; font-family:Calibri;'>");
            sb.Append("                                    Data de Julgamento");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width: 180px; font-family:Calibri;'>");
            sb.Append("                                    Data de Sentença");
            sb.Append("                                </td>");
            sb.Append("                                <td style='width: 200px; font-family:Calibri;'>");
            sb.Append("                                    Forma de Pagamento");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    Observação");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            int count = 1;
            foreach (JUR_PROCESSO_INSTANCIA i in jurController.ObterProcessoInstancia().Where(p => p.JUR_PROCESSO == processo.CODIGO).ToList())
            {
                sb.Append("                            <tr style='background: " + ((count % 2 == 0) ? "#F5F5F5;" : "#FFFFFF;") + "'>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + i.JUR_TIPO_INSTANCIA1.DESCRICAO);
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + ((i.DATA_JULGAMENTO == null) ? "" : Convert.ToDateTime(i.DATA_JULGAMENTO).ToString("dd/MM/yyyy")));
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + ((i.DATA_CONDENACAO == null) ? "" : Convert.ToDateTime(i.DATA_CONDENACAO).ToString("dd/MM/yyyy")));
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + i.FORMA_PGTO);
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + i.OBSERVACAO);
                sb.Append("                                </td>");
                sb.Append("                            </tr>");

                count += 1;
            }
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
