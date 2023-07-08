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
    public partial class rh_sol_demissao : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        const int TIPO_SOLICITACAO = 4;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPedidoDemissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataRealDemissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

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

                var solicitacao = rhController.ObterSolDemissao(codigoWF);
                if (solicitacao != null)
                {

                    //FILTRAR POR PERMISSAO EM LOJA
                    List<USUARIOLOJA> lojaUsuario = new BaseController().BuscaUsuarioLoja(usuario);
                    if (!lojaUsuario.Select(x => x.CODIGO_LOJA).Contains(solicitacao.CODIGO_FILIAL))
                        Response.Redirect("rh_menu.aspx");

                    hidCodigoWF.Value = solicitacao.RH_SOL_WORKFLOW.ToString();
                    litNumeroSolicitacao.Visible = true;
                    litNumeroSolicitacao.Text = "Número da Solicitação:&nbsp;&nbsp; <font color='red' size='3'>" + ("000000" + hidCodigoWF.Value).Substring(hidCodigoWF.Value.Length, 6) + "</font>";

                    btImprimir.CommandArgument = solicitacao.RH_SOL_WORKFLOW.ToString();

                    if (solicitacao.CODIGO_FILIAL_REGISTRO != null)
                        ddlFilialRegistro.SelectedValue = solicitacao.CODIGO_FILIAL_REGISTRO;
                    ddlFilialAtual.SelectedValue = solicitacao.CODIGO_FILIAL;
                    ddlFilial_SelectedIndexChanged(null, null);
                    ddlFuncionario.SelectedValue = solicitacao.RH_FUNCIONARIO.ToString();
                    txtCPF.Text = solicitacao.CPF;
                    txtDataPedidoDemissao.Text = solicitacao.DATA_PED_DEMISSAO.ToString("dd/MM/yyyy");
                    if (solicitacao.DATA_REAL_DEMISSAO != null)
                        txtDataRealDemissao.Text = Convert.ToDateTime(solicitacao.DATA_REAL_DEMISSAO).ToString("dd/MM/yyyy");
                    if (solicitacao.FOTO_CARTA_PEDIDO != null)
                        imgCartaPedidoDemissao.ImageUrl = solicitacao.FOTO_CARTA_PEDIDO;
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

                var lstFilialR = baseController.BuscaFiliais();
                lstFilialR = lstFilialR.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
                lstFilialR.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilialRegistro.DataSource = lstFilialR;
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
        protected void ddlFuncionario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFuncionario.SelectedValue != "" && ddlFuncionario.SelectedValue != "0")
            {
                var f = rhController.ObterFuncionario(Convert.ToInt32(ddlFuncionario.SelectedValue));
                if (f != null)
                    txtCPF.Text = f.CPF;
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

            labCPF.ForeColor = _OK;
            if (txtCPF.Text.Trim() == "")
            {
                labCPF.ForeColor = _notOK;
                retorno = false;
            }

            labDataPedidoDemissao.ForeColor = _OK;
            if (txtDataPedidoDemissao.Text.Trim() == "" || !ValidarData(txtDataPedidoDemissao.Text.Trim()))
            {
                labDataPedidoDemissao.ForeColor = _notOK;
                retorno = false;
            }

            labDataRealDemissao.ForeColor = _OK;
            if (txtDataRealDemissao.Text.Trim() != "" && !ValidarData(txtDataRealDemissao.Text.Trim()))
            {
                labDataRealDemissao.ForeColor = _notOK;
                retorno = false;
            }

            labCartaPedidoDemissao.ForeColor = _OK;
            if (imgCartaPedidoDemissao.ImageUrl.Trim() == "")
            {
                labCartaPedidoDemissao.ForeColor = _notOK;
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

                if (!Utils.WebControls.ValidarCPF(txtCPF.Text.Trim()))
                {
                    labErro.Text = "CPF INVÁLIDO.";
                    return;
                }

                //Se status finalizada, deverá informa a data real da demissão
                if (ddlStatus.SelectedValue == "4" && txtDataRealDemissao.Text.Trim() == "" && !ValidarData(txtDataRealDemissao.Text.Trim()))
                {
                    labErro.Text = "Informa a Data REAL da Demissão.";
                    return;
                }

                bool aberturaSol = false;
                int codigoWorkFlow = 0;
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                //STATUS AGUARDANDO ENVIO, TIPO SOLICITACAO DEMISSAO
                if (hidCodigoWF.Value == "")
                {
                    codigoWorkFlow = rhController.IniciarWorkflowSolicitacao(2, TIPO_SOLICITACAO, txtMotivo.Text.Trim(), codigoUsuario);
                    aberturaSol = true;
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
                    EnviarEmail(codigoWorkFlow, aberturaSol);

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

            RH_SOL_DEMISSAO solDemissao = null;

            if (hidCodigoWF.Value == "")
                solDemissao = new RH_SOL_DEMISSAO();
            else
                solDemissao = rhController.ObterSolDemissao(Convert.ToInt32(hidCodigoWF.Value));

            solDemissao.CODIGO_FILIAL_REGISTRO = ddlFilialRegistro.SelectedValue;
            solDemissao.CODIGO_FILIAL = ddlFilialAtual.SelectedValue;
            solDemissao.RH_FUNCIONARIO = Convert.ToInt32(ddlFuncionario.SelectedValue);
            solDemissao.CPF = txtCPF.Text.Trim();
            solDemissao.DATA_PED_DEMISSAO = Convert.ToDateTime(txtDataPedidoDemissao.Text);
            if (txtDataRealDemissao.Text.Trim() != "")
                solDemissao.DATA_REAL_DEMISSAO = Convert.ToDateTime(txtDataRealDemissao.Text);

            solDemissao.FOTO_CARTA_PEDIDO = imgCartaPedidoDemissao.ImageUrl;
            solDemissao.OBSERVACAO = txtObs.Text.Trim();

            if (hidCodigoWF.Value == "")
            {
                solDemissao.RH_SOL_WORKFLOW = codigoWorkFlow;
                solDemissao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                solDemissao.DATA_INCLUSAO = DateTime.Now;
                retorno = rhController.InserirSolDemissao(solDemissao);
            }
            else
                rhController.AtualizarSolDemissao(solDemissao);

            return retorno;
        }

        protected void btFotoCartaIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                imgCartaPedidoDemissao.ImageUrl = GravarImagem();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: (btFotoCartaIncluir_Click) \n\n" + ex.Message;
            }

        }
        private string GravarImagem()
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            Stream _stream = null;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(uploadFotoCarta.PostedFile.FileName);
                _fileName = Guid.NewGuid() + "_CARTA" + _ext;
                _path = Server.MapPath("~/Image_RH/") + _fileName;

                //Obter stream da imagem
                _stream = uploadFotoCarta.PostedFile.InputStream;
                if (_stream != null)
                    GenerateThumbnails(1, _stream, _path);

                retornoImagem = "~/Image_RH/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 220
                // height 130
                var newWidth = 0;
                var newHeight = 0;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > 1700 || newHeight > 2000)
                {
                    newWidth = (int)((newWidth) * 0.95);
                    newHeight = (int)((newHeight) * 0.95);
                }

                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
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

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            StreamWriter wr = null;
            try
            {
                int codigoWF = 0;

                if (btImprimir.CommandArgument != "")
                {
                    codigoWF = Convert.ToInt32(btImprimir.CommandArgument);

                    var solicitacao = rhController.ObterSolDemissao(codigoWF);
                    if (solicitacao != null)
                    {
                        string nomeArquivo = "CARTA_IMP_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                        wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                        wr.Write(MontarCarta(solicitacao).ToString());
                        wr.Flush();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
            finally
            {
                if (wr != null)
                    wr.Close();
            }
        }
        private StringBuilder MontarCarta(RH_SOL_DEMISSAO demissao)
        {
            StringBuilder _texto = new StringBuilder();

            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append(" <head>");
            _texto.Append("     <title>Carta</title>");
            _texto.Append("     <meta charset='UTF-8'>");
            _texto.Append("</head>");
            _texto.Append("<body onLoad=''>");
            _texto.Append("     <div id='fichaCarta' align='center' style='border: 0px solid #000;'>");
            _texto.Append("            <table border='0' cellpadding='0' cellspacing='0'>");
            // FIM CABECALHO

            _texto.Append("                 <tr>");
            _texto.Append("                     <td style='text-align:right;'>");
            _texto.Append("                         <input type='button' onclick='window.print();' value='Imprimir'");
            _texto.Append("                     </td>");
            _texto.Append("                 </tr>");
            _texto.Append("                 <tr>");
            _texto.Append("                     <td>");
            _texto.Append("                         <table border='0' cellpadding='0' cellspacing='0'>");
            _texto.Append("                             <tr>");
            _texto.Append("                                 <td>");
            _texto.Append("                                     <img alt='Foto Carta' Width='669px' Height='947px' src='..\\.." + demissao.FOTO_CARTA_PEDIDO.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.Append("                                 </td>");
            _texto.Append("                             </tr>");
            _texto.Append("                         </table>");
            _texto.Append("                     </td>");
            _texto.Append("                </tr>");

            // INICIO RODAPÉ
            _texto.Append("            </table>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }

        private void EnviarEmail(int codigoWF, bool aberturaSol)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.REMETENTE = usuario;

            if (aberturaSol)
            {
                email.ASSUNTO = "Intranet:  Solicitação de Demissão - No.: " + ("000000" + codigoWF).Substring(codigoWF.ToString().Length, 6);
                email.MENSAGEM = MontarCorpoEmail(codigoWF);
            }
            else
            {
                email.ASSUNTO = "Intranet:  Solicitação - No.: " + ("000000" + codigoWF).Substring(codigoWF.ToString().Length, 6);
                email.MENSAGEM = MontarCorpoEmailResp(codigoWF);
            }

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
            var solicitacao = rhController.ObterSolDemissao(codigoWF);
            var statusHistorico = rhController.ObterWorkflowSolicitacaoHistorico(codigoWF);

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Demissão de Funcionário</title>");
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
            sb.Append("                        <h2>Demissão de Funcionário</h2>");
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
            sb.Append("                                    Data Pedido de Demissão:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + solicitacao.DATA_PED_DEMISSAO.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            if (solicitacao.DATA_REAL_DEMISSAO != null)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Data Real de Demissão:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + Convert.ToDateTime(solicitacao.DATA_REAL_DEMISSAO).ToString("dd/MM/yyyy"));
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
        private string MontarCorpoEmailResp(int codigoWF)
        {
            var solicitacao = rhController.ObterSolDemissao(codigoWF);
            var statusHistorico = rhController.ObterWorkflowSolicitacaoHistorico(codigoWF);

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>DFunc</title>");
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
            sb.Append("                        <h2>Solicitação em Andamento</h2>");
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
            sb.Append("        <span>Enviado por: RH</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
