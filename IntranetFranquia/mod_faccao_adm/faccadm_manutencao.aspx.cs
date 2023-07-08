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
    public partial class faccadm_manutencao : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVigenciaIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVigenciaFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("faccadm_menu.aspx");

                if (Request.QueryString["d"] == null || Request.QueryString["d"] == "")
                    Response.Redirect("faccadm_menu.aspx");

                USUARIO usuario = new USUARIO();
                usuario = ((USUARIO)Session["USUARIO"]);

                int codigoFaccaoDocumento = 0;
                codigoFaccaoDocumento = Convert.ToInt32(Request.QueryString["d"].ToString());

                CarregarFaccao("");
                CarregarTipoDocumento(0);

                var faccaoDocumento = faccController.ObterFaccaoDocumento(codigoFaccaoDocumento);
                if (faccaoDocumento != null)
                {

                    ddlFaccao.SelectedValue = faccaoDocumento.NOME_CLIFOR;
                    ddlTipoDocumento.SelectedValue = faccaoDocumento.FACC_TIPO_DOCUMENTO.ToString();
                    if (faccaoDocumento.DATA_VIGENCIA_INI != null)
                        txtDataVigenciaIni.Text = Convert.ToDateTime(faccaoDocumento.DATA_VIGENCIA_INI).ToString("dd/MM/yyyy");
                    if (faccaoDocumento.DATA_VIGENCIA_FIM != null)
                        txtDataVigenciaFim.Text = Convert.ToDateTime(faccaoDocumento.DATA_VIGENCIA_FIM).ToString("dd/MM/yyyy");
                    txtDocumentoCaminho.Text = faccaoDocumento.DOCUMENTO;
                    txtObs.Text = faccaoDocumento.OBS;

                    hidCodigoFaccaoDocumento.Value = codigoFaccaoDocumento.ToString();
                    btExcluir.Visible = true;
                }

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btEnviar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFaccao(string faccao)
        {
            var faccaoCadastro = faccController.ObterFaccaoCadastro("", "");

            faccaoCadastro.Insert(0, new SP_OBTER_FACCAO_CADASTROResult { NOME_CLIFOR = "Selecione" });
            ddlFaccao.DataSource = faccaoCadastro;
            ddlFaccao.DataBind();

            if (faccao.Trim() != "")
                ddlFaccao.SelectedValue = faccao;
        }
        private void CarregarTipoDocumento(int codigotipoDocumento)
        {
            var tipoDocumento = faccController.ObterTipoDocumento().OrderBy(p => p.DOCUMENTO).ToList();

            tipoDocumento.Insert(0, new FACC_TIPO_DOCUMENTO { CODIGO = 0, DOCUMENTO = "Selecione" });
            ddlTipoDocumento.DataSource = tipoDocumento;
            ddlTipoDocumento.DataBind();

            if (codigotipoDocumento != 0)
                ddlTipoDocumento.SelectedValue = codigotipoDocumento.ToString();
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
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labFaccao.ForeColor = _OK;
            if (ddlFaccao.SelectedValue.Trim() == "Selecione")
            {
                labFaccao.ForeColor = _notOK;
                retorno = false;
            }

            labTipoDocumento.ForeColor = _OK;
            if (ddlTipoDocumento.SelectedValue.Trim() == "0")
            {
                labTipoDocumento.ForeColor = _notOK;
                retorno = false;
            }

            labDataVigenciaIni.ForeColor = _OK;
            if (txtDataVigenciaIni.Text.Trim() == "" || !ValidarData(txtDataVigenciaIni.Text.Trim()))
            {
                labDataVigenciaIni.ForeColor = _notOK;
                retorno = false;
            }

            labDataVigenciaFim.ForeColor = _OK;
            if (txtDataVigenciaFim.Text.Trim() != "" && !ValidarData(txtDataVigenciaFim.Text.Trim()))
            {
                labDataVigenciaFim.ForeColor = _notOK;
                retorno = false;
            }

            //labDocumento.ForeColor = _OK;
            //if (txtDocumentoCaminho.Text.Trim() == "")
            //{
            //    labDocumento.ForeColor = _notOK;
            //    retorno = false;
            //}

            return retorno;
        }
        #endregion

        #region "DOCUMENTO"
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                //VALIDAR O TIPO DE VIGENCIA DO DOCUMENTO SELECIONADO
                var tipoVigencia = faccController.ObterTipoDocumento(Convert.ToInt32(ddlTipoDocumento.SelectedValue));
                if (tipoVigencia != null && (tipoVigencia.TIPO_VIGENCIA == 'A' || tipoVigencia.TIPO_VIGENCIA == 'M') && txtDataVigenciaFim.Text.Trim() == "")
                {
                    labErro.Text = "Para este <strong>Tipo de Documento</strong> é obrigatório informar a <strong>Data Final da Vigência</strong>.";
                    return;
                }

                //VALIDAR VIGENCIA DUPLICADA POR FACCAO E TIPO DE DOCUMENTO
                /*var documentoDup = faccController.ObterFaccaoDocumento(ddlFaccao.SelectedValue, Convert.ToInt32(ddlTipoDocumento.SelectedValue));
                if (documentoDup != null && documentoDup.Count() > 0)
                {
                    if (documentoDup.Where(p => Convert.ToInt32(txtDataVigenciaIni.Text.Trim() >= p.DATA_VIGENCIA_INI)))

                    labErro.Text = "Este <strong>Tipo de Documento</strong> já foi inserido para esta <strong>Facção</strong>.";
                    return;
                }*/

                InserirFaccaoDocumento();

                labPopUp.Text = ddlTipoDocumento.SelectedItem.Text.Trim();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('faccadm_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('faccadm_menu.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private int InserirFaccaoDocumento()
        {
            int retorno = 0;

            FACC_FACCAO_DOCUMENTO faccaoDocumento = null;

            if (hidCodigoFaccaoDocumento.Value == "")
                faccaoDocumento = new FACC_FACCAO_DOCUMENTO();
            else
                faccaoDocumento = faccController.ObterFaccaoDocumento(Convert.ToInt32(hidCodigoFaccaoDocumento.Value));

            faccaoDocumento.NOME_CLIFOR = ddlFaccao.SelectedValue;
            faccaoDocumento.FACC_TIPO_DOCUMENTO = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
            faccaoDocumento.DATA_VIGENCIA_INI = Convert.ToDateTime(txtDataVigenciaIni.Text);
            if (txtDataVigenciaFim.Text.Trim() != "")
                faccaoDocumento.DATA_VIGENCIA_FIM = Convert.ToDateTime(txtDataVigenciaFim.Text);
            faccaoDocumento.DOCUMENTO = txtDocumentoCaminho.Text;
            faccaoDocumento.OBS = txtObs.Text.Trim();

            faccaoDocumento.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            faccaoDocumento.DATA_INCLUSAO = DateTime.Now;
            faccaoDocumento.STATUS = 'A';

            if (hidCodigoFaccaoDocumento.Value == "")
            {
                retorno = faccController.InserirFaccaoDocumento(faccaoDocumento);
            }
            else
                faccController.AtualizarFaccaoDocumento(faccaoDocumento);

            AtualizarObrigatoriedade(faccaoDocumento.NOME_CLIFOR, faccaoDocumento.FACC_TIPO_DOCUMENTO);
            return retorno;
        }
        private bool AtualizarObrigatoriedade(string nomeCliFor, int codigoTipoDocumento)
        {
            var faccaoTipoDocumento = faccController.ObterFaccaoTipoDocumento(nomeCliFor, codigoTipoDocumento);
            if (faccaoTipoDocumento == null)
            {
                FACC_FACCAO_TIPO_DOCUMENTO faccaoTipoDocumentoNovo = new FACC_FACCAO_TIPO_DOCUMENTO();
                faccaoTipoDocumentoNovo.NOME_CLIFOR = nomeCliFor;
                faccaoTipoDocumentoNovo.FACC_TIPO_DOCUMENTO = codigoTipoDocumento;
                faccaoTipoDocumentoNovo.OBRIGATORIO = 'S';
                faccController.InserirFaccaoTipoDocumento(faccaoTipoDocumentoNovo);
            }
            else
            {
                faccaoTipoDocumento.OBRIGATORIO = 'S';
                faccController.AtualizarFaccaoTipoDocumento(faccaoTipoDocumento);
            }

            return true;
        }

        protected void btDocumentoIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                //VALIDAR EXTENSAO
                if (System.IO.Path.GetExtension(uploadDocumento.PostedFile.FileName).ToLower().Replace(".", "") != "pdf")
                {
                    labErro.Text = "Selecione um arquivo no formato <strong>PDF</strong>.";
                    return;
                }

                if (uploadDocumento.PostedFile.ContentLength > 3010000)
                {
                    labErro.Text = "Selecione um arquivo com Tamanho máximo de <strong>3MB</strong>.";
                    return;
                }

                txtDocumentoCaminho.Text = GravarDocumento();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: (btDocumentoIncluir_Click) \n\n" + ex.Message;
            }

        }
        private string GravarDocumento()
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(uploadDocumento.PostedFile.FileName);
                _fileName = Guid.NewGuid() + "_DOC_FACC" + _ext;
                _path = Server.MapPath("~/Image_FACCADM/") + _fileName;
                uploadDocumento.SaveAs(_path);
                retornoImagem = "~/Image_FACCADM/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }


        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoFaccaoDocumento = 0;

                if (hidCodigoFaccaoDocumento.Value != "")
                {
                    codigoFaccaoDocumento = Convert.ToInt32(hidCodigoFaccaoDocumento.Value);
                    faccController.ExcluirFaccaoDocumento(codigoFaccaoDocumento);

                    labPopUp.Text = ddlTipoDocumento.SelectedItem.Text.Trim();
                    labMensagem.Text = "EXCLUÍDO COM SUCESSO.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('faccadm_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('faccadm_menu.aspx', '_self'); } } }); });", true);
                    dialogPai.Visible = true;

                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region "IMPRESSAO DOCUMENTO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (txtDocumentoCaminho.Text.Trim() == "")
                {
                    labErro.Text = "Não existe Documento para Impressão. Informe um arquivo.";
                    return;
                }

                Response.Redirect(txtDocumentoCaminho.Text, "_blank", "");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
            finally
            {
            }
        }
        #endregion

    }
}
