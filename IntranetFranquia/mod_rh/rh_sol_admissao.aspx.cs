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
using Relatorios.mod_rh.email;

namespace Relatorios
{
    public partial class rh_sol_admissao : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        const int TIPO_SOLICITACAO = 3;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIniPrevisto.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

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
                CarregarPeriodoExp();
                CarregarCargos();

                var solicitacao = rhController.ObterSolAdmissao(codigoWF);
                if (solicitacao != null)
                {

                    //FILTRAR POR PERMISSAO EM LOJA
                    List<USUARIOLOJA> lojaUsuario = new BaseController().BuscaUsuarioLoja(usuario);
                    if (!lojaUsuario.Select(x => x.CODIGO_LOJA).Contains(solicitacao.CODIGO_FILIAL))
                        Response.Redirect("rh_menu.aspx");

                    hidCodigoWF.Value = solicitacao.RH_SOL_WORKFLOW.ToString();
                    litNumeroSolicitacao.Visible = true;
                    litNumeroSolicitacao.Text = "Número da Solicitação:&nbsp;&nbsp; <font color='red' size='3'>" + ("000000" + hidCodigoWF.Value).Substring(hidCodigoWF.Value.Length, 6) + "</font>";

                    ddlFilialAtual.SelectedValue = solicitacao.CODIGO_FILIAL;
                    txtFuncionario.Text = solicitacao.NOME_FUNCIONARIO;
                    txtCPF.Text = solicitacao.CPF;
                    txtDataIniPrevisto.Text = solicitacao.DATA_INI_PREVISTO.ToString("dd/MM/yyyy");
                    txtCargo.Text = solicitacao.DESC_CARGO;
                    txtTelefone.Text = solicitacao.TELEFONE;
                    txtRG.Text = solicitacao.RG;
                    ddlSexo.SelectedValue = solicitacao.SEXO.ToString();
                    txtObs.Text = solicitacao.OBSERVACAO;
                    txtPeriodoTrabalho.Text = solicitacao.PERIODO_TRABALHO;

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

                Session["PERIODO_TRABALHO"] = null;
                CarregarPeriodoTrabalho(0);

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btEnviar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarCargos()
        {
            List<RH_CARGO> cargos = new List<RH_CARGO>();
            cargos = baseController.BuscarCargos('L');

            if (cargos != null)
            {
                cargos.Insert(0, new RH_CARGO { DESC_CARGO = "Selecione" });
                ddlCargo.DataSource = cargos;
                ddlCargo.DataBind();
            }
        }
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
                    }
                }
            }
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
        private void CarregarPeriodoExp()
        {

            var periodoExp = rhController.ObterPeriodoExperiencia();

            periodoExp.Insert(0, new RH_FUNCIONARIO_PERIODO_EXP { CODIGO = 0, DESCRICAO = "Selecione" });

            ddlPeriodoExp.DataSource = periodoExp;
            ddlPeriodoExp.DataBind();

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
            if (txtFuncionario.Text.Trim() == "")
            {
                labFuncionario.ForeColor = _notOK;
                retorno = false;
            }

            labCPF.ForeColor = _OK;
            if (txtCPF.Text.Trim() == "")
            {
                labCPF.ForeColor = _notOK;
                retorno = false;
            }

            labInicioPrevisto.ForeColor = _OK;
            if (txtDataIniPrevisto.Text.Trim() == "" || !ValidarData(txtDataIniPrevisto.Text.Trim()))
            {
                labInicioPrevisto.ForeColor = _notOK;
                retorno = false;
            }

            labCargo.ForeColor = _OK;
            if (txtCargo.Text.Trim() == "")
            {
                labCargo.ForeColor = _notOK;
                retorno = false;
            }

            labTelefone.ForeColor = _OK;
            if (txtTelefone.Text.Trim() == "")
            {
                labTelefone.ForeColor = _notOK;
                retorno = false;
            }

            labSexo.ForeColor = _OK;
            if (ddlSexo.SelectedValue == "")
            {
                labSexo.ForeColor = _notOK;
                retorno = false;
            }

            labPeriodoTrabalho.ForeColor = _OK;
            if (txtPeriodoTrabalho.Text.Trim() == "")
            {
                labPeriodoTrabalho.ForeColor = _notOK;
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
            int iLinha = 0;

            try
            {
                iLinha = 10;
                labErro.Text = "";
                iLinha = 20;
                if (!ValidarCampos(true))
                {
                    labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                iLinha = 30;
                if (!Utils.WebControls.ValidarCPF(txtCPF.Text.Trim()))
                {
                    labErro.Text = "CPF INVÁLIDO.";
                    return;
                }

                iLinha = 40;
                if (ddlStatus.SelectedValue == "4" && cbCriarFunc.Checked)
                    if (!InserirFuncionario())
                        return;

                iLinha = 50;
                int codigoWorkFlow = 0;
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                //STATUS AGUARDANDO ENVIO, TIPO SOLICITACAO TRANSFERENCIA TEMPORARIA VENDEDOR
                if (hidCodigoWF.Value == "")
                {
                    iLinha = 60;
                    codigoWorkFlow = rhController.IniciarWorkflowSolicitacao(2, TIPO_SOLICITACAO, txtMotivo.Text.Trim(), codigoUsuario);
                }
                else
                {
                    iLinha = 70;
                    codigoWorkFlow = Convert.ToInt32(hidCodigoWF.Value);
                    rhController.AtualizarWorkflowSolicitacao(codigoWorkFlow, Convert.ToInt32(ddlStatus.SelectedValue), TIPO_SOLICITACAO, txtMotivo.Text.Trim(), codigoUsuario);
                }

                iLinha = 80;
                int codigoSolicitacao = 0;
                codigoSolicitacao = InserirSolicitacao(codigoWorkFlow);

                iLinha = 90;

                //Enviar Email
                if (Constante.enviarEmail)
                    EnviarEmail(codigoWorkFlow);

                iLinha = 100;

                labPopUp.Text = ("000000" + codigoWorkFlow).Substring(codigoWorkFlow.ToString().Length, 6);
                iLinha = 110;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('rh_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('rh_menu.aspx', '_self'); } } }); });", true);
                iLinha = 120;
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + " - Linha: " + iLinha.ToString();
            }
        }
        private int InserirSolicitacao(int codigoWorkFlow)
        {
            int retorno = 0;

            RH_SOL_ADMISSAO solAdmissao = null;

            if (hidCodigoWF.Value == "")
                solAdmissao = new RH_SOL_ADMISSAO();
            else
                solAdmissao = rhController.ObterSolAdmissao(Convert.ToInt32(hidCodigoWF.Value));

            solAdmissao.CODIGO_FILIAL = ddlFilialAtual.SelectedValue;
            solAdmissao.NOME_FUNCIONARIO = txtFuncionario.Text.Trim().ToUpper();
            solAdmissao.CPF = txtCPF.Text.Trim();
            solAdmissao.RG = txtRG.Text.Trim();
            solAdmissao.DESC_CARGO = txtCargo.Text.Trim().ToUpper();
            solAdmissao.SEXO = Convert.ToChar(ddlSexo.SelectedValue);
            solAdmissao.DATA_INI_PREVISTO = Convert.ToDateTime(txtDataIniPrevisto.Text.Trim());
            solAdmissao.TELEFONE = txtTelefone.Text.Trim();
            solAdmissao.OBSERVACAO = txtObs.Text.Trim();
            solAdmissao.PERIODO_TRABALHO = txtPeriodoTrabalho.Text.Trim();

            if (hidCodigoWF.Value == "")
            {

                solAdmissao.RH_SOL_WORKFLOW = codigoWorkFlow;
                solAdmissao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                solAdmissao.DATA_INCLUSAO = DateTime.Now;
                retorno = rhController.InserirSolAdmissao(solAdmissao);
            }
            else
                rhController.AtualizarSolAdmissao(solAdmissao);

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
            email.ASSUNTO = "Intranet:  Solicitação de Admissão - No.: " + ("000000" + codigoWF).Substring(codigoWF.ToString().Length, 6);
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
            var solicitacao = rhController.ObterSolAdmissao(codigoWF);
            var statusHistorico = rhController.ObterWorkflowSolicitacaoHistorico(codigoWF);

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Admissão de Funcionário</title>");
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
            sb.Append("                        <h2>Admissão de Funcionário</h2>");
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
            sb.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(solicitacao.CODIGO_FILIAL)).FILIAL.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Funcionário:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.NOME_FUNCIONARIO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cpfFormatado = solicitacao.CPF.Trim();
            if (cpfFormatado.Length == 11)
                cpfFormatado = Convert.ToUInt64(cpfFormatado).ToString(@"000\.000\.000\-00");
            else
                cpfFormatado = Convert.ToUInt64(cpfFormatado).ToString(@"00\.000\.000\/0000\-00");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    CPF:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + cpfFormatado);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    RG:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.RG);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cargo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DESC_CARGO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Sexo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((solicitacao.SEXO == 'M') ? "Masculino" : "Feminino"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Início Previsto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DATA_INI_PREVISTO.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            if (solicitacao.PERIODO_TRABALHO != null && solicitacao.PERIODO_TRABALHO.Trim() != "")
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Horário de Trabalho:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + solicitacao.PERIODO_TRABALHO);
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Telefone:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.TELEFONE);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
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

        #region "CRIAR FUNCIONARIO"
        private bool ValidarCamposFunc()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labVendedorApelido.ForeColor = _OK;
            if (txtVendedorApelido.Text.Trim() == "")
            {
                labVendedorApelido.ForeColor = _notOK;
                retorno = false;
            }

            labComissao.ForeColor = _OK;
            if (txtComissao.Text.Trim() == "")
            {
                labComissao.ForeColor = _notOK;
                retorno = false;
            }

            labPeriodoExp.ForeColor = _OK;
            if (ddlPeriodoExp.SelectedValue.Trim() == "0")
            {
                labPeriodoExp.ForeColor = _notOK;
                retorno = false;
            }

            labCidade.ForeColor = _OK;
            if (txtCidade.Text.Trim() == "")
            {
                labCidade.ForeColor = _notOK;
                retorno = false;
            }

            labUF.ForeColor = _OK;
            if (txtUF.Text.Trim() == "")
            {
                labUF.ForeColor = _notOK;
                retorno = false;
            }

            labCargoFun.ForeColor = _OK;
            if (ddlCargo.SelectedValue.Trim() == "" || ddlCargo.SelectedValue.Trim() == "Selecione")
            {
                labCargoFun.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCriarFunc.Checked = false;
            cbCriarFunc.Visible = false;
            pnlCriarFuncionario.Visible = false;
            if (ddlStatus.SelectedValue == "4")
                cbCriarFunc.Visible = true;
        }
        protected void cbCriarFunc_CheckedChanged(object sender, EventArgs e)
        {
            pnlCriarFuncionario.Visible = false;
            if (cbCriarFunc.Checked)
                pnlCriarFuncionario.Visible = true;
        }

        protected bool InserirFuncionario()
        {
            //Validar campos obrigatórios
            try
            {
                labErro.Text = "";

                if (!ValidarCamposFunc())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return false;
                }

                if (!Utils.WebControls.ValidarCPF(txtCPF.Text.Trim()))
                {
                    labErro.Text = "CPF INVÁLIDO.";
                    return false;
                }

                var funcionarios = rhController.ObterFuncionario().Where(p => p.CPF.Trim() == txtCPF.Text.Trim()).ToList();
                if (funcionarios != null && funcionarios.Count() > 0)
                {
                    labErro.Text = "Este CPF já está cadastrado para o Funcionário: " + funcionarios[0].NOME.ToUpper() + " na Filial: " + funcionarios[0].CODIGO_FILIAL + ".";
                    return false;
                }

                //INCLUIR FUNCIONARIO NO LINX
                var funLINX = rhController.CriarFuncionarioLINX(
                     "",
                     "",
                     ddlFilialAtual.SelectedValue,
                     txtVendedorApelido.Text.Trim().ToUpper(),// APELIDO
                     txtFuncionario.Text.Trim().ToUpper(),
                     Convert.ToDecimal(txtComissao.Text),
                     txtRG.Text.ToUpper(),
                     "", //ENDERECO
                     "", //COMPLEMENTO
                     txtCidade.Text.Trim().ToUpper(), //CIDADE
                     "", //CEP
                     txtTelefone.Text,
                     txtUF.Text.Trim().ToUpper(), //UF
                     txtCPF.Text.Trim(),
                     ((cbGerenteLoja.Checked) ? '1' : '0'),
                     cbOperaCaixa.Checked,
                     cbAcessoGerencial.Checked,
                     Convert.ToDateTime(txtDataIniPrevisto.Text.Trim()),
                     null, //DEMISSAO
                     ddlCargo.SelectedValue,
                     'I');

                if (funLINX == null || funLINX.VENDEDOR == "")
                {
                    labErro.Text = "Erro ao inserir funcionário no LINX. Entre em contato com TI.";
                    return false;
                }

                //INCLUIR FUNCIONARIO NA INTRANET
                var funcionario = new RH_FUNCIONARIO();
                funcionario.CODIGO_FILIAL = ddlFilialAtual.SelectedValue;
                funcionario.NOME = txtFuncionario.Text.Trim().ToUpper();
                funcionario.CPF = txtCPF.Text.Trim();
                funcionario.RH_FUNCIONARIO_PERIODO_EXP = Convert.ToInt32(ddlPeriodoExp.SelectedValue);
                funcionario.DATA_ADMISSAO = Convert.ToDateTime(txtDataIniPrevisto.Text.Trim());
                funcionario.DATA_DEMISSAO = null;
                funcionario.DESC_CARGO = ddlCargo.SelectedValue;
                funcionario.FOTO = "";
                funcionario.BATE_PONTO = (cbBatePonto.Checked) ? 'S' : 'N';
                funcionario.DATA_INCLUSAO = DateTime.Now;
                funcionario.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                funcionario.STATUS = 'A';
                funcionario.SEXO = Convert.ToChar(ddlSexo.SelectedValue);
                funcionario.VENDEDOR = funLINX.VENDEDOR;
                var codigoFuncionario = rhController.InserirFuncionario(funcionario);

                //PERIODO DE TRABALHO
                if (Session["PERIODO_TRABALHO"] != null)
                {
                    RH_FUNCIONARIO_PONTO_PERIODO_TRAB _funcPeriodo = null;

                    //LOOP NA SESSION PARA INSERIR APENAS OS NOVOS//
                    List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> listaPeriodoTrabalho = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                    listaPeriodoTrabalho = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];
                    foreach (RH_FUNCIONARIO_PONTO_PERIODO_TRAB per in listaPeriodoTrabalho)
                    {

                        if (per != null)
                        {
                            _funcPeriodo = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                            _funcPeriodo.RH_FUNCIONARIO = codigoFuncionario;
                            _funcPeriodo.RH_PONTO_PERIODO_TRAB = per.RH_PONTO_PERIODO_TRAB;
                            _funcPeriodo.DATA_INCLUSAO = DateTime.Now;
                            _funcPeriodo.STATUS = 'A';

                            rhController.InserirFuncionarioPeriodoTrab(_funcPeriodo);
                        }
                    }
                }

                var usuario = ((USUARIO)Session["USUARIO"]);
                EmailFuncionario.EnviarEmailAlteracaoFun(funcionario, true, usuario, txtTelefone.Text.Trim(), txtUF.Text.Trim());

                return true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
                return false;
            }
        }

        #region "PERIODO DE TRABALHO"
        private List<RH_PONTO_PERIODO_TRAB> ObterPeriodoTrabalho()
        {
            List<RH_PONTO_PERIODO_TRAB> _periodoTrab = new List<RH_PONTO_PERIODO_TRAB>();
            _periodoTrab = rhController.ObterPeriodoTrabalho();
            return _periodoTrab;
        }
        private bool ValidarPeriodoTrabalho(string descPeriodo)
        {
            bool retorno = true;

            foreach (GridViewRow row in gvPeriodoTrabalho.Rows)
            {
                if (row != null)
                {
                    Label _labPeriodo = row.FindControl("labPeriodo") as Label;
                    if (_labPeriodo != null)
                    {
                        if (_labPeriodo.Text.Trim() == descPeriodo.Trim())
                        {
                            retorno = false;
                            break;
                        }
                    }
                }
            }

            return retorno;
        }

        private void CarregarPeriodoTrabalho(int codigoFuncionario)
        {
            List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> _perFunc = rhController.ObterFuncionarioPeriodoTrab(codigoFuncionario.ToString());

            if (Session["PERIODO_TRABALHO"] != null)
                _perFunc.AddRange((List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"]);

            if (_perFunc == null || _perFunc.Count <= 0)
                _perFunc.Add(new RH_FUNCIONARIO_PONTO_PERIODO_TRAB { RH_FUNCIONARIO = 0, RH_PONTO_PERIODO_TRAB = 0 });

            gvPeriodoTrabalho.DataSource = _perFunc;
            gvPeriodoTrabalho.DataBind();
        }
        protected void gvPeriodoTrabalho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RH_FUNCIONARIO_PONTO_PERIODO_TRAB _perFunc = e.Row.DataItem as RH_FUNCIONARIO_PONTO_PERIODO_TRAB;

                    if (_perFunc != null)
                    {
                        DropDownList _ddlPeriodo = e.Row.FindControl("ddlPeriodo") as DropDownList;
                        if (_ddlPeriodo != null)
                        {
                            _ddlPeriodo.DataSource = ObterPeriodoTrabalho();
                            _ddlPeriodo.DataBind();

                            _ddlPeriodo.SelectedValue = rhController.ObterPeriodoTrabalho(_perFunc.RH_PONTO_PERIODO_TRAB).CODIGO.ToString();
                        }

                        Label _labPeriodo = e.Row.FindControl("labPeriodo") as Label;
                        if (_labPeriodo != null)
                            if (_perFunc.RH_PONTO_PERIODO_TRAB1 != null)
                                _labPeriodo.Text = _perFunc.RH_PONTO_PERIODO_TRAB1.DESCRICAO;

                        ImageButton _ibtEditar = e.Row.FindControl("btEditarPeriodo") as ImageButton;
                        ImageButton _ibtSalvarPeriodo = e.Row.FindControl("btSalvarPeriodo") as ImageButton;
                        ImageButton _ibtSair = e.Row.FindControl("btSairPeriodo") as ImageButton;
                        ImageButton _ibtExcluir = e.Row.FindControl("btExcluirPeriodo") as ImageButton;
                        _ibtEditar.Visible = false;
                        _ibtSalvarPeriodo.Visible = false;
                        _ibtSair.Visible = false;
                        _ibtExcluir.Visible = false;
                        if (_perFunc.RH_PONTO_PERIODO_TRAB > 0)
                        {
                            _ibtExcluir.Visible = true;
                            _ibtExcluir.CommandArgument = _perFunc.CODIGO.ToString();

                            if (e.Row.RowState.ToString().ToUpper().Contains("EDIT"))
                            {
                                _ibtSalvarPeriodo.Visible = true;
                                _ibtSair.Visible = true;
                            }
                            else
                            {
                                _ibtEditar.Visible = true;
                            }
                        }
                    }
                }
            }

        }
        protected void gvPeriodoTrabalho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPeriodoTrabalho.FooterRow;
            if (footer != null)
            {
                DropDownList _ddlPeriodoFooter = footer.FindControl("ddlPeriodoFooter") as DropDownList;
                if (_ddlPeriodoFooter != null)
                {
                    _ddlPeriodoFooter.DataSource = ObterPeriodoTrabalho();
                    _ddlPeriodoFooter.DataBind();
                }
            }
        }
        protected void btEditarPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {

                        //Obter código da COR
                        string codigoPeriodoTrabalhoFuncionario = gvPeriodoTrabalho.DataKeys[row.RowIndex].Value.ToString();
                        /*var _perFunc = rhController.ObterFuncionarioPeriodoTrab(codigoPeriodoTrabalhoFuncionario);
                        if (_perFunc != null && _perFunc.Count() > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui movimento. Não será possível Alterar.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }*/

                        gvPeriodoTrabalho.EditIndex = row.RowIndex;

                        int codigoFuncionario = 0;

                        CarregarPeriodoTrabalho(codigoFuncionario);

                        //Botão acionado é escondido
                        bt.Visible = false;

                        ImageButton _btSairPeriodo = row.FindControl("btSairPeriodo") as ImageButton;
                        if (_btSairPeriodo != null)
                            _btSairPeriodo.Visible = true;

                        ImageButton _btSalvarPeriodo = row.FindControl("btSalvarPeriodo") as ImageButton;
                        if (_btSalvarPeriodo != null)
                            _btSalvarPeriodo.Visible = true;

                        Label _labPeriodo = row.FindControl("labPeriodo") as Label;

                        hidDescricaoPeriodo.Value = _labPeriodo.Text.Trim();

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btEditarPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSalvarPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {

                        int codigoFuncionario = 0;

                        string codigoPerTrabFuncionario = "";
                        DropDownList _ddlPeriodo = row.FindControl("ddlPeriodo") as DropDownList;

                        if (!ValidarPeriodoTrabalho(_ddlPeriodo.SelectedItem.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Este PERÍODO Já foi inserido.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> _perTrabFun = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                        if (Session["PERIODO_TRABALHO"] != null)
                            _perTrabFun = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];

                        //Obter código
                        codigoPerTrabFuncionario = gvPeriodoTrabalho.DataKeys[row.RowIndex].Value.ToString();

                        RH_FUNCIONARIO_PONTO_PERIODO_TRAB perTrab = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                        perTrab = rhController.ObterFuncionarioPeriodoTrab(Convert.ToInt32(codigoPerTrabFuncionario));
                        if (perTrab == null)
                        {
                            int _index = _perTrabFun.FindIndex(p => p.RH_PONTO_PERIODO_TRAB1.DESCRICAO.Trim() == hidDescricaoPeriodo.Value.Trim());
                            _perTrabFun.RemoveAt(_index);
                        }
                        else
                        {
                            rhController.ExcluirFuncionarioPeriodoTrab(codigoPerTrabFuncionario);
                        }

                        perTrab = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                        perTrab.RH_FUNCIONARIO = codigoFuncionario;
                        perTrab.RH_PONTO_PERIODO_TRAB = Convert.ToInt32(_ddlPeriodo.SelectedValue.Trim());
                        perTrab.RH_PONTO_PERIODO_TRAB1 = rhController.ObterPeriodoTrabalho(Convert.ToInt32(_ddlPeriodo.SelectedValue.Trim()));
                        _perTrabFun.Add(perTrab);

                        Session["PERIODO_TRABALHO"] = _perTrabFun;
                        CarregarPeriodoTrabalho(codigoFuncionario);

                        ImageButton ibtSair = row.FindControl("btSairPeriodo") as ImageButton;
                        btSairPeriodo_Click(ibtSair, null);
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btSalvarPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSairPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {
                        gvPeriodoTrabalho.EditIndex = -1;

                        int codigoFuncionario = 0;

                        CarregarPeriodoTrabalho(codigoFuncionario);

                        //Botão acionado é escondido
                        bt.Visible = false;

                        hidDescricaoPeriodo.Value = "";

                        ImageButton _btEditarPeriodo = row.FindControl("btEditarPeriodo") as ImageButton;
                        if (_btEditarPeriodo != null)
                            _btEditarPeriodo.Visible = true;

                        ImageButton _btSalvarPeriodo = row.FindControl("btSalvarPeriodo") as ImageButton;
                        if (_btSalvarPeriodo != null)
                            _btSalvarPeriodo.Visible = false;

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btSairPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btExcluirPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {
                        int codigoFuncionario = 0;
                        string codigoPerTrabFuncionario = "";
                        string periodoTrabalho = "";

                        #region "Validação de Campos"
                        Label _labPeriodo = row.FindControl("labPeriodo") as Label;
                        if (_labPeriodo == null)
                        {
                            DropDownList _ddlPeriodo = row.FindControl("_ddlPeriodo") as DropDownList;
                            periodoTrabalho = _ddlPeriodo.SelectedItem.Text.Trim();
                        }
                        else
                        {
                            periodoTrabalho = _labPeriodo.Text.Trim();
                        }
                        #endregion

                        RH_FUNCIONARIO_PONTO_PERIODO_TRAB perFunc = null;

                        codigoPerTrabFuncionario = gvPeriodoTrabalho.DataKeys[row.RowIndex].Value.ToString();

                        if (codigoFuncionario > 0 && codigoPerTrabFuncionario != "")
                            perFunc = rhController.ObterFuncionarioPeriodoTrab(Convert.ToInt32(codigoPerTrabFuncionario));
                        if (perFunc == null)
                        {
                            List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> _listaPeriodo = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                            if (Session["PERIODO_TRABALHO"] != null)
                                _listaPeriodo = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];

                            int _index = _listaPeriodo.FindIndex(p => p.RH_PONTO_PERIODO_TRAB1.DESCRICAO.Trim() == periodoTrabalho);
                            _listaPeriodo.RemoveAt(_index);

                            Session["PERIODO_TRABALHO"] = _listaPeriodo;
                        }
                        else
                        {
                            rhController.ExcluirFuncionarioPeriodoTrab(perFunc.CODIGO);
                        }

                        gvPeriodoTrabalho.EditIndex = -1;
                        CarregarPeriodoTrabalho(codigoFuncionario);

                        hidDescricaoPeriodo.Value = "";
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btExcluirPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }

        }
        protected void btIncluirPeriodo_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {
                        int codigoFuncionario = 0;

                        string PONTO_PERIODO_TRAB = "";
                        string descPeriodo = "";
                        DropDownList _ddlPeriodoFooter = row.FindControl("ddlPeriodoFooter") as DropDownList;

                        if (_ddlPeriodoFooter != null && _ddlPeriodoFooter.SelectedValue.Trim() != "")
                        {
                            PONTO_PERIODO_TRAB = _ddlPeriodoFooter.SelectedValue;
                            descPeriodo = _ddlPeriodoFooter.SelectedItem.Text.Trim();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Selecione um PERÍODO.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (!ValidarPeriodoTrabalho(descPeriodo))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Este PERÍODO Já foi inserido.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> periodos = new List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>();
                        RH_FUNCIONARIO_PONTO_PERIODO_TRAB perFunc = new RH_FUNCIONARIO_PONTO_PERIODO_TRAB();
                        perFunc.RH_PONTO_PERIODO_TRAB = Convert.ToInt32(PONTO_PERIODO_TRAB);
                        perFunc.RH_PONTO_PERIODO_TRAB1 = rhController.ObterPeriodoTrabalho(Convert.ToInt32(PONTO_PERIODO_TRAB));

                        if (Session["PERIODO_TRABALHO"] != null)
                            periodos = (List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB>)Session["PERIODO_TRABALHO"];

                        periodos.Add(perFunc);
                        Session["PERIODO_TRABALHO"] = periodos;

                        //Carregar Periodos
                        CarregarPeriodoTrabalho(codigoFuncionario);

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btIncluirPeriodo_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        #endregion

        #endregion

    }
}
