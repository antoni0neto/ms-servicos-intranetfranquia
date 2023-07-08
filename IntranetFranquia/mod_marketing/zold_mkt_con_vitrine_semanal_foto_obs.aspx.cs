using DAL;
using Relatorios.mod_ecom.mag;
using Relatorios.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class zold_mkt_con_vitrine_semanal_foto_obs : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("mkt_menu.aspx");

                string fotoDiretorio = "";

                fotoDiretorio = Request.QueryString["p"].ToString();

                //var fotoVitrine = lojaController.ObterVitrineFotoPorFotoDiretorio(fotoDiretorio);
                //if (fotoVitrine != null)
                //{
                //    imgFoto.ImageUrl = fotoDiretorio;
                //    imgFoto.Attributes.Add("data-zoom-image", fotoDiretorio.Replace("~", "..").Replace("Image_VITRINE", "Image_VITRINE/Zoom"));

                //    txtSemana.Text = fotoVitrine.DIA_INICIAL.ToString("dd/MM/yyyy") + " - " + fotoVitrine.DIA_FINAL.ToString("dd/MM/yyyy");
                //    txtFilial.Text = fotoVitrine.FILIAL;
                //    txtSetor.Text = fotoVitrine.MKT_VITRINE_SETOR1.SETOR;
                //    txtTipo.Text = fotoVitrine.DESCRICAO;

                //    txtEmail.Text = fotoVitrine.OBS;

                //    hidCodigoFilial.Value = fotoVitrine.CODIGO_FILIAL;
                //    hidCodigoVitrine.Value = fotoVitrine.CODIGO.ToString();

                //    var fotoCarrossel = lojaController.ObterVitrineFotoCarrossel(fotoVitrine.ANOSEMANA, fotoVitrine.CODIGO_FILIAL, fotoDiretorio);
                //    if (fotoCarrossel != null)
                //    {
                //        btPrev.CommandArgument = fotoCarrossel.FOTO_ANTERIOR;
                //        btNext.CommandArgument = fotoCarrossel.FOTO_PROXIMA;

                //        if (fotoCarrossel.FOTO_ANTERIOR == "")
                //            btPrev.Visible = false;
                //        if (fotoCarrossel.FOTO_PROXIMA == "")
                //            btNext.Visible = false;
                //    }
                //}


                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

            //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#imgFoto').elevateZoom({ zoomType: 'inner', cursor: 'crosshair', zoomWindowFadeIn: 500, zoomWindowFadeOut: 750 });});", true);

        }

        #region "DADOS INICIAIS"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labEmail.ForeColor = _OK;
            if (txtEmail.Text.Trim() == "")
            {
                labEmail.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //if (!ValidarCampos())
                //{
                //    labErro.Text = "Informe a Observação da Foto...";
                //    return;
                //}

                //EnviarEmail();

                //var vitrine = lojaController.ObterVitrineFoto(Convert.ToInt32(hidCodigoVitrine.Value));
                //if (vitrine != null)
                //{
                //    vitrine.OBS = txtEmail.Text.Trim();
                //    lojaController.AtualizarVitrineFoto(vitrine);
                //}

                //btSalvar.Enabled = false;

                //labErro.Text = "E-mail enviado com sucesso.";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                //dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();

            email.ASSUNTO = "Intranet: Foto Semana " + txtSemana.Text + " - " + txtFilial.Text + " - " + txtSetor.Text;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(email.ASSUNTO);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            destinatario.Add("luciana@hbf.com.br");
            destinatario.Add(usuario.EMAIL);

            var filial = baseController.ObterFilialIntranet(hidCodigoFilial.Value);
            if (filial != null)
                destinatario.Add(filial.email);

            var supervisor = baseController.BuscaSupervisorLoja(hidCodigoFilial.Value);
            if (supervisor != null)
            {
                int codigoUsuario = supervisor.codigo_usuario;
                var sup = baseController.BuscaUsuario(codigoUsuario);
                if (sup != null)
                    destinatario.Add(sup.EMAIL);
            }

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(string assunto)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Observaçao da Foto</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divEmail' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <h2> " + assunto.Replace("Intranet: Foto", "") + "</h2>");
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
            sb.Append("                                    Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + txtFilial.Text);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Semana:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + txtSemana.Text);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Setor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + txtSetor.Text);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Foto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                     <img alt='' Width='' Height='' src='http://cmaxweb.dnsalias.com:8585" + imgFoto.ImageUrl.Replace("~", "") + "' />&nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Observação:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + txtEmail.Text);
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

        protected void btNext_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                Response.Redirect("mkt_con_vitrine_semanal_foto_obs.aspx?p=" + btNext.CommandArgument);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btPrev_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                Response.Redirect("mkt_con_vitrine_semanal_foto_obs.aspx?p=" + btPrev.CommandArgument);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


    }
}


