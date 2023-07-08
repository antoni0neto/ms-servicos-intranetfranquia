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
    public partial class rh_sol_transf_temp_vendedor : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        const int TIPO_SOLICITACAO = 1;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

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

                var solicitacao = rhController.ObterSolTransfTemp(codigoWF);
                if (solicitacao != null)
                {

                    //FILTRAR POR PERMISSAO EM LOJA
                    List<USUARIOLOJA> lojaUsuario = new BaseController().BuscaUsuarioLoja(usuario);
                    if (!lojaUsuario.Select(x => x.CODIGO_LOJA).Contains(solicitacao.CODIGO_FILIAL_ATUAL))
                        Response.Redirect("rh_menu.aspx");

                    hidCodigoWF.Value = solicitacao.RH_SOL_WORKFLOW.ToString();
                    litNumeroSolicitacao.Visible = true;
                    litNumeroSolicitacao.Text = "Número da Solicitação:&nbsp;&nbsp; <font color='red' size='3'>" + ("000000" + hidCodigoWF.Value).Substring(hidCodigoWF.Value.Length, 6) + "</font>";

                    ddlFilialAtual.SelectedValue = solicitacao.CODIGO_FILIAL_ATUAL;
                    ddlFilial_SelectedIndexChanged(null, null);
                    ddlFuncionario.SelectedValue = solicitacao.RH_FUNCIONARIO.ToString();
                    ddlFilialPara.SelectedValue = solicitacao.CODIGO_FILIAL_PARA;
                    txtDataInicial.Text = solicitacao.DATA_INICIAL.ToString("dd/MM/yyyy");
                    txtDataFinal.Text = solicitacao.DATA_FINAL.ToString("dd/MM/yyyy");

                    txtCargo.Text = solicitacao.DESC_CARGO;
                    txtPeriodoTrabalho.Text = solicitacao.PERIODO_TRABALHO;
                    if (solicitacao.COMISSAO != null)
                        txtComissao.Text = solicitacao.COMISSAO.ToString();
                    txtObs.Text = solicitacao.OBSERVACAO;

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
                        CarregarStatus(1, usuario.CODIGO_PERFIL);
                    else
                        CarregarStatus(Convert.ToInt32(ultimoStatus.RH_SOL_STATUS1.ORDEM), usuario.CODIGO_PERFIL);

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
                    ddlFilialAtual.DataSource = lstFilialAtual;
                    ddlFilialAtual.DataBind();

                    if (lstFilialAtual.Count == 2)
                    {
                        ddlFilialAtual.SelectedIndex = 1;
                        ddlFilial_SelectedIndexChanged(null, null);
                    }
                }
            }
        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFuncionario(ddlFilialAtual.SelectedValue);
            CarregarFilialPara(ddlFilialAtual.SelectedValue);
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
            ddlFuncionario.DataSource = funcionario;
            ddlFuncionario.DataBind();

            if (ddlFuncionario.Items.Count == 2)
                ddlFuncionario.SelectedIndex = 1;
        }
        private void CarregarFilialPara(string filial)
        {

            var lstFilialPara = baseController.BuscaFiliais().OrderBy(o => o.FILIAL).ToList();
            var filialDePara = baseController.BuscaFilialDePara();
            lstFilialPara = lstFilialPara.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            lstFilialPara.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
            ddlFilialPara.DataSource = lstFilialPara;
            ddlFilialPara.DataBind();
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

            labFilialAtual.ForeColor = _OK;
            if (ddlFilialAtual.SelectedValue.Trim() == "")
            {
                labFilialAtual.ForeColor = _notOK;
                retorno = false;
            }

            labFuncionario.ForeColor = _OK;
            if (ddlFuncionario.SelectedValue.Trim() == "0" || ddlFuncionario.SelectedValue.Trim() == "")
            {
                labFuncionario.ForeColor = _notOK;
                retorno = false;
            }

            labFilialPara.ForeColor = _OK;
            if (ddlFilialPara.SelectedValue.Trim() == "")
            {
                labFilialPara.ForeColor = _notOK;
                retorno = false;
            }

            labDataInicial.ForeColor = _OK;
            if (txtDataInicial.Text.Trim() == "" || !ValidarData(txtDataInicial.Text.Trim()))
            {
                labDataInicial.ForeColor = _notOK;
                retorno = false;
            }

            labDataFinal.ForeColor = _OK;
            if (txtDataFinal.Text.Trim() == "" || !ValidarData(txtDataFinal.Text.Trim()))
            {
                labDataFinal.ForeColor = _notOK;
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

            labComissao.ForeColor = _OK;
            decimal comissao = 0;
            if (txtComissao.Text.Trim() != "" && !decimal.TryParse(txtComissao.Text.Trim(), out comissao))
            {
                labComissao.ForeColor = _notOK;
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

                labErro.Text = "";
                if (Convert.ToDateTime(txtDataFinal.Text) <= Convert.ToDateTime(txtDataInicial.Text))
                {
                    labErro.Text = "Data Final deve ser maior que Data Inicial.";
                    return;
                }

                int codigoWorkFlow = 0;
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                //STATUS AGUARDANDO ENVIO, TIPO SOLICITACAO TRANSFERENCIA TEMPORARIA VENDEDOR
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

            RH_SOL_TRANSF_TEMP solTT = null;

            if (hidCodigoWF.Value == "")
                solTT = new RH_SOL_TRANSF_TEMP();
            else
                solTT = rhController.ObterSolTransfTemp(Convert.ToInt32(hidCodigoWF.Value));

            solTT.CODIGO_FILIAL_ATUAL = ddlFilialAtual.SelectedValue;
            solTT.RH_FUNCIONARIO = Convert.ToInt32(ddlFuncionario.SelectedValue);
            solTT.CODIGO_FILIAL_PARA = ddlFilialPara.SelectedValue;
            solTT.DATA_INICIAL = Convert.ToDateTime(txtDataInicial.Text);
            solTT.DATA_FINAL = Convert.ToDateTime(txtDataFinal.Text);
            solTT.DESC_CARGO = txtCargo.Text.Trim().ToUpper();
            solTT.PERIODO_TRABALHO = txtPeriodoTrabalho.Text.Trim().ToUpper();
            if (txtComissao.Text.Trim() != "")
                solTT.COMISSAO = Convert.ToDecimal(txtComissao.Text.Trim());
            solTT.OBSERVACAO = txtObs.Text.Trim();

            if (hidCodigoWF.Value == "")
            {
                solTT.RH_SOL_WORKFLOW = codigoWorkFlow;
                solTT.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                solTT.DATA_INCLUSAO = DateTime.Now;
                retorno = rhController.InserirSolTransfTemp(solTT);
            }
            else
                rhController.AtualizarSolTransfTemp(solTT);

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

        private void EnviarEmail(int codigoWF)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Solicitação de Transferência Temporária de Funcionário - No.: " + ("000000" + codigoWF).Substring(codigoWF.ToString().Length, 6);
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
            var solicitacao = rhController.ObterSolTransfTemp(codigoWF);
            var statusHistorico = rhController.ObterWorkflowSolicitacaoHistorico(codigoWF);

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Transferência Temporária de Funcionário</title>");
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
            sb.Append("                        <h2>Transferência Temporária de Funcionário</h2>");
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
            sb.Append("                                    Filial Atual:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(solicitacao.CODIGO_FILIAL_ATUAL)).FILIAL.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Funcionário:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.RH_FUNCIONARIO1.VENDEDOR + " - " + solicitacao.RH_FUNCIONARIO1.NOME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Para Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(solicitacao.CODIGO_FILIAL_PARA)).FILIAL.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Data Inicial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DATA_INICIAL.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Data Final:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DATA_FINAL.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            if (solicitacao.DESC_CARGO != null && solicitacao.DESC_CARGO.Trim() != "")
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Cargo:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + solicitacao.DESC_CARGO);
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            if (solicitacao.PERIODO_TRABALHO != null && solicitacao.PERIODO_TRABALHO.Trim() != "")
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Período Trabalho:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + solicitacao.PERIODO_TRABALHO);
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            if (solicitacao.COMISSAO != null)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Comissão:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + solicitacao.COMISSAO.ToString() + " %");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            if (solicitacao.OBSERVACAO != null && solicitacao.OBSERVACAO.Trim() != "")
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Observação:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    <font color='red'>" + solicitacao.OBSERVACAO.Replace("\r", "<br/>").Replace("\n", "<br/>") + "</font>");
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
    }
}
