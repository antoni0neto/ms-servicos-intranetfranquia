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
    public partial class rh_sol_alteracao_venda : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        const int TIPO_SOLICITACAO = 7;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVenda.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("rh_menu.aspx");

                if (Request.QueryString["s"] == null || Request.QueryString["s"] == "")
                    Response.Redirect("rh_menu.aspx");

                USUARIO usuario = new USUARIO();
                usuario = ((USUARIO)Session["USUARIO"]);

                int codigoWF = 0;
                codigoWF = Convert.ToInt32(Request.QueryString["s"].ToString());

                string tela = "";
                if (Request.QueryString["t"] != null && Request.QueryString["t"] != "")
                {
                    tela = Request.QueryString["t"].ToString();

                    if (tela == "1")
                        hrefVoltar.HRef = "rh_sol_painel_pendente.aspx";
                    if (tela == "2")
                        hrefVoltar.HRef = "rh_sol_painel.aspx";
                }

                CarregarFilial();
                CarregarStatus(0, 0);

                var solicitacao = rhController.ObterSolAlteracaoVenda(codigoWF);
                if (solicitacao != null)
                {
                    //FILTRAR POR PERMISSAO EM LOJA
                    List<USUARIOLOJA> lojaUsuario = new BaseController().BuscaUsuarioLoja(usuario);
                    if (!lojaUsuario.Select(x => x.CODIGO_LOJA).Contains(solicitacao.CODIGO_FILIAL))
                        Response.Redirect("rh_menu.aspx");

                    hidCodigoWF.Value = solicitacao.RH_SOL_WORKFLOW.ToString();
                    litNumeroSolicitacao.Visible = true;
                    litNumeroSolicitacao.Text = "Número da Solicitação:&nbsp;&nbsp; <font color='red' size='3'>" + ("000000" + hidCodigoWF.Value).Substring(hidCodigoWF.Value.Length, 6) + "</font>";

                    ddlFilial.SelectedValue = solicitacao.CODIGO_FILIAL;
                    ddlFilial_SelectedIndexChanged(null, null);
                    ddlFuncionarioDe.SelectedValue = solicitacao.RH_FUNCIONARIO_DE.ToString();
                    ddlFuncionarioPara.SelectedValue = solicitacao.RH_FUNCIONARIO_PARA.ToString();
                    txtTicket.Text = solicitacao.TICKET;
                    txtDataVenda.Text = solicitacao.DATA_TICKET.ToString("dd/MM/yyyy");
                    txtTerminal.Text = solicitacao.TERMINAL;
                    if (solicitacao.VALOR_VENDA != null)
                        txtValorVenda.Text = solicitacao.VALOR_VENDA.ToString();

                    var status = rhController.ObterWorkflowSolicitacaoHistorico(solicitacao.RH_SOL_WORKFLOW);

                    gvStatusHistorico.Visible = false;
                    labHistoricoTitulo.Visible = false;
                    if (status != null && status.Count > 0)
                    {
                        labHistoricoTitulo.Visible = true;
                        gvStatusHistorico.Visible = true;

                        gvStatusHistorico.DataSource = status;
                        gvStatusHistorico.DataBind();
                    }

                    var ultimoStatusData = status.Max(p => p.DATA_INCLUSAO);
                    var ultimoStatus = status.Where(p => p.DATA_INCLUSAO == ultimoStatusData).SingleOrDefault();

                    //se for loja, nao pode fazer nada... se status = 3 (EM PROCESSAMENTO) ou 4(FINALIZADA)
                    if ((usuario.CODIGO_PERFIL == 2 || usuario.CODIGO_PERFIL == 3) && (ultimoStatus.RH_SOL_STATUS == 3 || ultimoStatus.RH_SOL_STATUS == 4))
                        pnlStatus.Visible = false;

                    if (usuario.CODIGO_PERFIL == 2 || usuario.CODIGO_PERFIL == 3)
                    {
                        CarregarStatus(1, usuario.CODIGO_PERFIL);
                    }
                    else
                    {
                        CarregarStatus(Convert.ToInt32(ultimoStatus.RH_SOL_STATUS1.ORDEM), usuario.CODIGO_PERFIL);
                        btAlterarVenda.Visible = true;
                    }

                    if (ultimoStatus.RH_SOL_STATUS == 4)
                        pnlStatus.Visible = false;

                }

                if (hidCodigoWF.Value == "")
                {
                    ddlStatus.Enabled = false;
                    ddlStatus.SelectedValue = "1";
                }

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btEnviar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviar, null) + ";");
            btAlterarVenda.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAlterarVenda, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilialAtual = new List<FILIAI>();
                lstFilialAtual = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();

                var filialDePara = baseController.BuscaFilialDePara();
                if (lstFilialAtual.Count > 0)
                {
                    lstFilialAtual = lstFilialAtual.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

                    lstFilialAtual.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilialAtual;
                    ddlFilial.DataBind();

                    if (lstFilialAtual.Count == 2)
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
        private void CarregarFuncionario(string filial)
        {
            int codigoPerfil = 0;
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            List<SP_OBTER_FUNCIONARIOResult> funcionario = new List<SP_OBTER_FUNCIONARIOResult>();

            if (usuario != null)
            {
                codigoPerfil = usuario.CODIGO_PERFIL;

                if (codigoPerfil == 1 || codigoPerfil == 11)
                    filial = "";

                funcionario = rhController.ObterFuncionarioComCodigo(null, "", filial, "", "").Where(p => p.DATA_DEMISSAO == null).OrderBy(p => p.VENDEDOR_APELIDO).ToList();
            }

            funcionario.Insert(0, new SP_OBTER_FUNCIONARIOResult { CODIGO = 0, NOME = "Selecione" });
            ddlFuncionarioDe.DataSource = funcionario;
            ddlFuncionarioDe.DataBind();

            ddlFuncionarioPara.DataSource = funcionario;
            ddlFuncionarioPara.DataBind();

            if (ddlFuncionarioDe.Items.Count == 2)
                ddlFuncionarioDe.SelectedIndex = 1;

            if (ddlFuncionarioPara.Items.Count == 2)
                ddlFuncionarioPara.SelectedIndex = 1;
        }

        private void CarregarStatus(int ordem, int codigoPerfil)
        {
            //Pode gerar registros quando estiver em processamento
            if ((codigoPerfil == 1 || codigoPerfil == 11) && ordem == 3)
                ordem = ordem - 1;

            var status = rhController.ObterStatusSolicitacao().Where(p => p.ORDEM > ordem).OrderBy(o => o.ORDEM).ToList();

            status.Insert(0, new RH_SOL_STATUS { CODIGO = 0, DESCRICAO = "Selecione" });
            ddlStatus.DataSource = status;
            ddlStatus.DataBind();

            if (ordem > 0 && (codigoPerfil == 2 || codigoPerfil == 3))
            {
                ddlStatus.SelectedValue = "2";
                ddlStatus.Enabled = false;
            }
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
        private bool ValidarCampos(bool inclusao)
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

            labFuncionarioDe.ForeColor = _OK;
            if (ddlFuncionarioDe.SelectedValue.Trim() == "0" || ddlFuncionarioDe.SelectedValue.Trim() == "")
            {
                labFuncionarioDe.ForeColor = _notOK;
                retorno = false;
            }

            labFuncionarioPara.ForeColor = _OK;
            if (ddlFuncionarioPara.SelectedValue.Trim() == "0" || ddlFuncionarioPara.SelectedValue.Trim() == "")
            {
                labFuncionarioPara.ForeColor = _notOK;
                retorno = false;
            }

            labTicket.ForeColor = _OK;
            if (txtTicket.Text.Trim() == "")
            {
                labTicket.ForeColor = _notOK;
                retorno = false;
            }

            labDataVenda.ForeColor = _OK;
            if (txtDataVenda.Text.Trim() == "" || !ValidarData(txtDataVenda.Text.Trim()))
            {
                labDataVenda.ForeColor = _notOK;
                retorno = false;
            }

            labTerminal.ForeColor = _OK;
            if (txtTerminal.Text.Trim() == "")
            {
                labTerminal.ForeColor = _notOK;
                retorno = false;
            }

            labValor.ForeColor = _OK;
            decimal valorVenda = 0;
            if (txtValorVenda.Text.Trim() == "" || !decimal.TryParse(txtValorVenda.Text.Trim(), out valorVenda))
            {
                labValor.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue == "0")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }

            labMotivo.ForeColor = _OK;
            if (txtMotivo.Text.Trim() == "")
            {
                labMotivo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "INCLUSAO"
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos(true))
                {
                    labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                int codigoWorkFlow = 0;
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                //STATUS AGUARDANDO ENVIO, TIPO SOLICITACAO ALTERACAO DE VENDA
                if (hidCodigoWF.Value == "")
                {
                    codigoWorkFlow = rhController.IniciarWorkflowSolicitacao(2, TIPO_SOLICITACAO, txtMotivo.Text.Trim(), codigoUsuario);
                }
                else
                {
                    codigoWorkFlow = Convert.ToInt32(hidCodigoWF.Value);
                    rhController.AtualizarWorkflowSolicitacao(codigoWorkFlow, Convert.ToInt32(ddlStatus.SelectedValue), TIPO_SOLICITACAO, txtMotivo.Text.Trim(), codigoUsuario);
                }

                int codigoSolicitacao = 0;
                codigoSolicitacao = InserirSolicitacao(codigoWorkFlow);

                //Enviar Email
                if (Constante.enviarEmail)
                    EnviarEmail(codigoWorkFlow);

                labPopUp.Text = ("000000" + codigoWorkFlow).Substring(codigoWorkFlow.ToString().Length, 6);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('rh_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('rh_menu.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private int InserirSolicitacao(int codigoWorkFlow)
        {
            int retorno = 0;

            RH_SOL_ALTER_VENDA solAlterVenda = null;

            if (hidCodigoWF.Value == "")
                solAlterVenda = new RH_SOL_ALTER_VENDA();
            else
                solAlterVenda = rhController.ObterSolAlteracaoVenda(Convert.ToInt32(hidCodigoWF.Value));

            solAlterVenda.CODIGO_FILIAL = ddlFilial.SelectedValue;
            solAlterVenda.RH_FUNCIONARIO_DE = Convert.ToInt32(ddlFuncionarioDe.SelectedValue);
            solAlterVenda.RH_FUNCIONARIO_PARA = Convert.ToInt32(ddlFuncionarioPara.SelectedValue);
            solAlterVenda.TICKET = txtTicket.Text.Trim();
            solAlterVenda.DATA_TICKET = Convert.ToDateTime(txtDataVenda.Text.Trim());
            solAlterVenda.TERMINAL = txtTerminal.Text.Trim();
            solAlterVenda.VALOR_VENDA = Convert.ToDecimal(txtValorVenda.Text.Trim());

            if (hidCodigoWF.Value == "")
            {
                solAlterVenda.RH_SOL_WORKFLOW = codigoWorkFlow;
                solAlterVenda.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                solAlterVenda.DATA_INCLUSAO = DateTime.Now;
                retorno = rhController.InserirSolAlteracaoVenda(solAlterVenda);
            }
            else
                rhController.AtualizarSolAlteracaoVenda(solAlterVenda);

            return retorno;
        }
        #endregion

        #region "GRID HISTORICO STATUS"
        protected void gvStatusHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_SOL_WORKFLOW _wk = e.Row.DataItem as RH_SOL_WORKFLOW;

                    if (_wk != null)
                    {
                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                            _litStatus.Text = _wk.RH_SOL_STATUS1.DESCRICAO;

                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = _wk.DATA_INCLUSAO.ToString("dd/MM/yyyy HH:mm");

                        Literal _litMotivo = e.Row.FindControl("litMotivo") as Literal;
                        if (_litMotivo != null)
                            _litMotivo.Text = _wk.MOTIVO;
                    }
                }
            }

        }
        #endregion

        #region "EMAIL"
        private void EnviarEmail(int codigoWF)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Solicitação de Alteração de Venda - No.: " + ("000000" + codigoWF).Substring(codigoWF.ToString().Length, 6);
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(codigoWF);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails RH
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(6, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            //Adicionar usuário do histórico da solicitação
            var solWF = rhController.ObterWorkflowSolicitacaoHistorico(codigoWF);
            foreach (var s in solWF)
                if (s != null)
                {
                    var usu = new BaseController().BuscaUsuario(s.USUARIO_INCLUSAO);
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                }

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(int codigoWF)
        {
            var solicitacao = rhController.ObterSolAlteracaoVenda(codigoWF);
            var statusHistorico = rhController.ObterWorkflowSolicitacaoHistorico(codigoWF);

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Alteração de Venda</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divSolicitacao' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <h2>Alteração de Venda</h2>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    No. Solicitação:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ("000000" + codigoWF.ToString()).Substring(codigoWF.ToString().Length, 6));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(solicitacao.CODIGO_FILIAL)).FILIAL.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Funcionário De:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.RH_FUNCIONARIO.VENDEDOR + " - " + solicitacao.RH_FUNCIONARIO.NOME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Para Funcionário:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.RH_FUNCIONARIO1.VENDEDOR + " - " + solicitacao.RH_FUNCIONARIO1.NOME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Ticket:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.TICKET);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Data Ticket:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DATA_TICKET.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Terminal:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.TERMINAL);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            if (solicitacao.VALOR_VENDA != null)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Valor Venda:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + solicitacao.VALOR_VENDA);
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
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
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        Histórico da Solicitação");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        <table cellpadding='2' cellspacing='0' style='width: 650pt; padding: 0px; color: black;");
            sb.Append("                            font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            sb.Append("                            border: 1px solid #ccc;'>");
            sb.Append("                            <tr style='background-color: #ccc;'>");
            sb.Append("                                <td>");
            sb.Append("                                    Status");
            sb.Append("                                </td>");
            sb.Append("                                <td style='text-align: center;'>");
            sb.Append("                                    Data");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    Motivo");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            int codigoUsuario = 0;
            foreach (var hist in statusHistorico)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td style='border-right: 1px solid #ccc; width: 150px;'>");
                sb.Append("                                    " + hist.RH_SOL_STATUS1.DESCRICAO);
                sb.Append("                                </td>");
                sb.Append("                                <td style='border-right: 1px solid #ccc; text-align: center; width: 150px;'>");
                sb.Append("                                    " + hist.DATA_INCLUSAO.ToString("dd/MM/yyyy HH:mm"));
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + hist.MOTIVO);
                sb.Append("                                </td>");
                sb.Append("                            </tr>");

                codigoUsuario = hist.USUARIO_INCLUSAO;
            }
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                         &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

        protected void btAlterarVenda_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoFilial = ddlFilial.SelectedValue.Trim();
                DateTime dataVenda = Convert.ToDateTime(txtDataVenda.Text);
                string ticket = txtTicket.Text.Trim();

                //Response.Redirect(("rh_alt_venda_vendedor.aspx?c=" + codigoFilial + "&d=" + dataVenda + "&tic=" + ticket), "_blank", "");

                string vendedor = rhController.ObterFuncionario(Convert.ToInt32(ddlFuncionarioDe.SelectedValue)).VENDEDOR;
                string vendedorPara = rhController.ObterFuncionario(Convert.ToInt32(ddlFuncionarioPara.SelectedValue)).VENDEDOR;

                if (vendedorPara == "0" || vendedorPara == "" || vendedorPara == "Selecione")
                {
                    labErro.Text = "Selecione para qual Vendedor irá o Ticket.";
                    return;
                }

                int ret = baseController.AtualizarTicketLojaVenda(codigoFilial, dataVenda, ticket, vendedorPara);
                if (ret != 1)
                {
                    labErro.Text = "Ticket não foi encontrado. Envie esta tela para o TI (informatica@hbf.com.br).";
                    return;
                }

                var tlv = baseController.BuscaTicketLojaVendaVendedores(codigoFilial, dataVenda, ticket, vendedor).SingleOrDefault();
                if (tlv != null)
                {
                    LOJA_VENDA_VENDEDORE _novo = new LOJA_VENDA_VENDEDORE();
                    _novo.CODIGO_FILIAL = tlv.CODIGO_FILIAL;
                    _novo.TICKET = tlv.TICKET;
                    _novo.DATA_VENDA = tlv.DATA_VENDA;
                    _novo.ID_VENDEDOR = tlv.ID_VENDEDOR;
                    _novo.VENDEDOR = vendedorPara;
                    _novo.COMISSAO = tlv.COMISSAO;
                    _novo.DATA_PARA_TRANSFERENCIA = DateTime.Now;
                    _novo.TIPO_VENDEDOR = tlv.TIPO_VENDEDOR;
                    _novo.CODIGO_CLIENTE = tlv.CODIGO_CLIENTE;
                    _novo.ACESSO_GERENCIAL = tlv.ACESSO_GERENCIAL;
                    _novo.AUSENTE = tlv.AUSENTE;

                    baseController.ExcluirTicketLojaVendaVendedores(_novo.CODIGO_FILIAL, _novo.DATA_VENDA, _novo.TICKET, _novo.VENDEDOR);
                    baseController.InserirTicketLojaVendaVendedores(_novo);
                }
                baseController.ExcluirTicketLojaVendaVendedores(codigoFilial, dataVenda, ticket, vendedor);

                labErro.Text = "Venda atualizada com sucesso. Por favor, finalize a solicitação.";

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
