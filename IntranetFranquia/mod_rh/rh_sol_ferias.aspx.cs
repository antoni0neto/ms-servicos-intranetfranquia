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
    public partial class rh_sol_ferias : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        const int TIPO_SOLICITACAO = 5;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

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

                var solicitacao = rhController.ObterSolFerias(codigoWF);
                if (solicitacao != null)
                {
                    //FILTRAR POR PERMISSAO EM LOJA
                    List<USUARIOLOJA> lojaUsuario = new BaseController().BuscaUsuarioLoja(usuario);
                    if (!lojaUsuario.Select(x => x.CODIGO_LOJA).Contains(solicitacao.CODIGO_FILIAL))
                        Response.Redirect("rh_menu.aspx");

                    hidCodigoWF.Value = solicitacao.RH_SOL_WORKFLOW.ToString();
                    litNumeroSolicitacao.Visible = true;
                    litNumeroSolicitacao.Text = "Número da Solicitação:&nbsp;&nbsp; <font color='red' size='3'>" + ("000000" + hidCodigoWF.Value).Substring(hidCodigoWF.Value.Length, 6) + "</font>";

                    if (solicitacao.CODIGO_FILIAL_REGISTRO != null)
                        ddlFilialRegistro.SelectedValue = solicitacao.CODIGO_FILIAL_REGISTRO;
                    ddlFilialAtual.SelectedValue = solicitacao.CODIGO_FILIAL;
                    ddlFilial_SelectedIndexChanged(null, null);
                    ddlFuncionario.SelectedValue = solicitacao.RH_FUNCIONARIO.ToString();
                    txtDataInicial.Text = solicitacao.DATA_INICIO.ToString("dd/MM/yyyy");
                    ddlDiasGozo.SelectedValue = solicitacao.DIAS_GOZO.ToString();
                    txtAutorizadoPor.Text = solicitacao.AUTORIZADO_POR;
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
                    cbImprimir.Visible = true;
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

                var lstFilialT = baseController.BuscaFiliais();
                lstFilialT = lstFilialT.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
                lstFilialT.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilialRegistro.DataSource = lstFilialT;
                ddlFilialRegistro.DataBind();
            }
        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFuncionario(ddlFilialAtual.SelectedValue);
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

            labFilialRegistro.ForeColor = _OK;
            if (ddlFilialRegistro.SelectedValue.Trim() == "")
            {
                labFilialRegistro.ForeColor = _notOK;
                retorno = false;
            }

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

            labDataInicial.ForeColor = _OK;
            if (txtDataInicial.Text.Trim() == "" || !ValidarData(txtDataInicial.Text.Trim()))
            {
                labDataInicial.ForeColor = _notOK;
                retorno = false;
            }

            labDiasGozo.ForeColor = _OK;
            if (ddlDiasGozo.SelectedValue == "")
            {
                labDiasGozo.ForeColor = _notOK;
                retorno = false;
            }

            labAutorizadoPor.ForeColor = _OK;
            if (txtAutorizadoPor.Text.Trim() == "")
            {
                labAutorizadoPor.ForeColor = _notOK;
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
                //STATUS AGUARDANDO ENVIO, TIPO SOLICITACAO TRANSFERENCIA VENDEDOR
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

                if (cbImprimir.Checked)
                {
                    var solicitacao = rhController.ObterSolFerias(codigoWorkFlow);
                    if (solicitacao != null)
                        GerarRelatorio(solicitacao);
                }

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

            RH_SOL_FERIA solFerias = null;

            if (hidCodigoWF.Value == "")
                solFerias = new RH_SOL_FERIA();
            else
                solFerias = rhController.ObterSolFerias(Convert.ToInt32(hidCodigoWF.Value));

            solFerias.CODIGO_FILIAL_REGISTRO = ddlFilialRegistro.SelectedValue;
            solFerias.CODIGO_FILIAL = ddlFilialAtual.SelectedValue;
            solFerias.RH_FUNCIONARIO = Convert.ToInt32(ddlFuncionario.SelectedValue);
            solFerias.DATA_INICIO = Convert.ToDateTime(txtDataInicial.Text);
            solFerias.DIAS_GOZO = Convert.ToInt32(ddlDiasGozo.SelectedValue);
            solFerias.AUTORIZADO_POR = txtAutorizadoPor.Text.Trim().ToUpper();
            solFerias.OBSERVACAO = txtObs.Text.Trim();

            if (hidCodigoWF.Value == "")
            {
                solFerias.RH_SOL_WORKFLOW = codigoWorkFlow;
                solFerias.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                solFerias.DATA_INCLUSAO = DateTime.Now;
                retorno = rhController.InserirSolFerias(solFerias);
            }
            else
                rhController.AtualizarSolFerias(solFerias);

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

        #region "IMPRESSAO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (hidCodigoWF.Value != "")
                {
                    var solicitacao = rhController.ObterSolFerias(Convert.ToInt32(hidCodigoWF.Value));
                    if (solicitacao != null)
                        GerarRelatorio(solicitacao);
                    else
                        labErro.Text = "Problemas ao gerar o Formulário. Entre em contato com TI.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void GerarRelatorio(RH_SOL_FERIA _solicitacao)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "SOLICITACAO_FER_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_solicitacao));
                wr.Flush();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(RH_SOL_FERIA _solicitacao)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarFormulario(_solicitacao);

            return _texto;
        }
        private StringBuilder MontarFormulario(RH_SOL_FERIA _solicitacao)
        {
            StringBuilder _texto = new StringBuilder();
            _texto.Append("");

            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Formulário de Férias</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("</head>");
            _texto.Append("<body onload='window.print();'>");
            _texto.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("        background: white; white-space: nowrap; text-align:center;'>");
            _texto.Append("        <br />");
            _texto.Append("        <div id='divFerias' align='center' style='border: 1px solid #000; width: 517pt; height:960px;'>");
            _texto.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("                background: white; white-space: nowrap;'>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2' style='line-height:60px;'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2' style='text-align:center;'>");
            _texto.Append("                        <img alt='HBF' width='80' height='80' src='http://cmaxweb.dnsalias.com:8585/image/hbf_mini.jpg' />");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2' style='line-height:60px;'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2' style='text-align:center;'>");
            _texto.Append("                        <h2>Formulário de Solicitação de Férias</h2>");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2' style='line-height:60px;'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td style='width:40px;'></td>");
            _texto.Append("                    <td style='line-height: 20px; text-align:left;'>");
            _texto.Append("                        <table border='0' cellpadding='4' cellspacing='5' style='width: 350pt; padding: 0px;");
            _texto.Append("                            color: black; font-size: 10.5pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("                            background: white; white-space: nowrap;'>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td style='width: 165px;'>");
            _texto.Append("                                    Data Solicitação:");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    " + DateTime.Now.ToString("dd/MM/yyyy"));
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td style='width: 165px;'>");
            _texto.Append("                                    Filial Registro:");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(_solicitacao.CODIGO_FILIAL_REGISTRO)).FILIAL.Trim());
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td style='width: 165px;'>");
            _texto.Append("                                    Filial Atual:");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(_solicitacao.CODIGO_FILIAL)).FILIAL.Trim());
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td>");
            _texto.Append("                                    Nome do Funcionário:<font style='font-weight:bold'></font>");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    " + _solicitacao.RH_FUNCIONARIO1.VENDEDOR + " - " + _solicitacao.RH_FUNCIONARIO1.NOME);
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td>");
            _texto.Append("                                    Data Início:");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    " + _solicitacao.DATA_INICIO.ToString("dd/MM/yyyy"));
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td>");
            _texto.Append("                                    Dias de Gozo");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    " + _solicitacao.DIAS_GOZO);
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td>");
            _texto.Append("                                    Autorizado Por");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    " + _solicitacao.AUTORIZADO_POR);
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td colspan='2' style='line-height:20px;'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            if (_solicitacao.OBSERVACAO != null && _solicitacao.OBSERVACAO.Trim() != "")
            {
                _texto.Append("                            <tr>");
                _texto.Append("                                <td valign='top'>");
                _texto.Append("                                    Observação");
                _texto.Append("                                </td>");
                _texto.Append("                                <td valign='top'>");
                _texto.Append("                                    " + _solicitacao.OBSERVACAO.Replace("\r", "<br/>").Replace("\n", "<br/>"));
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
            }
            _texto.Append("                            <tr>");
            _texto.Append("                                <td colspan='2' style='line-height:40px;'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td>");
            _texto.Append("                                    GERENTE");
            _texto.Append("                                </td>");
            _texto.Append("                                <td>");
            _texto.Append("                                    <hr>");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td colspan='2' style='line-height:20px;'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                        </table>");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("            </table>");
            _texto.Append("        </div>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }
        #endregion

        #region "EMAIL"
        private void EnviarEmail(int codigoWF)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Solicitação de Férias - No.: " + ("000000" + codigoWF).Substring(codigoWF.ToString().Length, 6);
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
            var solicitacao = rhController.ObterSolFerias(codigoWF);
            var statusHistorico = rhController.ObterWorkflowSolicitacaoHistorico(codigoWF);

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Férias</title>");
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
            sb.Append("                        <h2>Solicitação de Férias</h2>");
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
            sb.Append("                                    Filial Registro:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(solicitacao.CODIGO_FILIAL_REGISTRO)).FILIAL.Trim());
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
            sb.Append("                                    " + solicitacao.RH_FUNCIONARIO1.VENDEDOR + " - " + solicitacao.RH_FUNCIONARIO1.NOME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Data Início:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DATA_INICIO.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Dias de Gozo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DIAS_GOZO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Autorizado Por:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.AUTORIZADO_POR);
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
        #endregion
    }
}
