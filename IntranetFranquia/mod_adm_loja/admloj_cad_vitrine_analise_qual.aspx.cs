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
    public partial class admloj_cad_vitrine_analise_qual : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["fot"] == null || Request.QueryString["fot"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("admloj_menu.aspx");


                var codVitrineMundoFoto = Convert.ToInt32(Request.QueryString["fot"].ToString());

                var fotoVitrine = lojaController.ObterVitrineMundoFotoPorCodigo(codVitrineMundoFoto);
                if (fotoVitrine != null)
                {
                    imgFoto.ImageUrl = fotoVitrine.FOTO;
                    imgFoto.Attributes.Add("data-zoom-image", fotoVitrine.FOTO.Replace("~", "..").Replace("Image_VITRINE", "Image_VITRINE/Zoom"));

                    hidCodigoVitrineMundoFoto.Value = fotoVitrine.CODIGO.ToString();

                    chkHarmonia.Checked = (fotoVitrine.HARMONIA == 'S') ? true : false;
                    chkPosicao.Checked = (fotoVitrine.POSICAO == 'S') ? true : false;
                    chkMontagem.Checked = (fotoVitrine.MONTAGEM == 'S') ? true : false;
                    chkCubos.Checked = (fotoVitrine.CUBOS == 'S') ? true : false;
                    chkAcessorios.Checked = (fotoVitrine.ACESSORIOS == 'S') ? true : false;

                    txtObservacao.Text = fotoVitrine.OBSERVACAO;

                    if (fotoVitrine.HARMONIA == 'S' && fotoVitrine.POSICAO == 'S' && fotoVitrine.MONTAGEM == 'S' && fotoVitrine.CUBOS == 'S' && fotoVitrine.ACESSORIOS == 'S')
                        chkMarcarTodos.Checked = true;

                    if (fotoVitrine.ORDEM == 0)
                    {
                        chkMontagem.Enabled = false;
                        chkCubos.Enabled = false;
                        chkAcessorios.Enabled = false;
                    }
                }

                var codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL;
                if (codigoPerfil == 2)
                {
                    btSalvar.Enabled = false;
                    btSalvar.Visible = false;
                }

            }

            //Evitar duplo clique no botão
            //btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#imgFoto').elevateZoom({ zoomType: 'inner', cursor: 'crosshair', zoomWindowFadeIn: 500, zoomWindowFadeOut: 750 });});", true);

        }

        #region "DADOS INICIAIS"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            //labEmail.ForeColor = _OK;
            //if (txtEmail.Text.Trim() == "")
            //{
            //    labEmail.ForeColor = _notOK;
            //    retorno = false;
            //}

            return retorno;
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var vitrineMundoFoto = lojaController.ObterVitrineMundoFotoPorCodigo(Convert.ToInt32(hidCodigoVitrineMundoFoto.Value));
                if (vitrineMundoFoto != null)
                {

                    vitrineMundoFoto.HARMONIA = (chkHarmonia.Checked) ? 'S' : 'N';
                    vitrineMundoFoto.POSICAO = (chkPosicao.Checked) ? 'S' : 'N';
                    vitrineMundoFoto.MONTAGEM = (chkMontagem.Checked) ? 'S' : 'N';
                    vitrineMundoFoto.CUBOS = (chkCubos.Checked) ? 'S' : 'N';
                    vitrineMundoFoto.ACESSORIOS = (chkAcessorios.Checked) ? 'S' : 'N';
                    vitrineMundoFoto.OBSERVACAO = txtObservacao.Text.Trim();
                    vitrineMundoFoto.DATA_QUALIFICACAO = DateTime.Now;
                    vitrineMundoFoto.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    lojaController.AtualizarVitrineMundoFoto(vitrineMundoFoto);
                }

                btSalvar.Enabled = false;

                labErro.Text = "Foto qualificada com sucesso.";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void chkMarcarTodos_CheckedChanged(object sender, EventArgs e)
        {
            chkHarmonia.Checked = chkMarcarTodos.Checked;
            chkPosicao.Checked = chkMarcarTodos.Checked;
            chkMontagem.Checked = chkMarcarTodos.Checked;
            chkCubos.Checked = chkMarcarTodos.Checked;
            chkAcessorios.Checked = chkMarcarTodos.Checked;
        }

        #region "EMAIL"
        //private void EnviarEmail()
        //{
        //    USUARIO usuario = (USUARIO)Session["USUARIO"];

        //    email_envio email = new email_envio();

        //    email.ASSUNTO = "Intranet: Foto Semana " + txtSemana.Text + " - " + txtFilial.Text + " - " + txtSetor.Text;
        //    email.REMETENTE = usuario;
        //    email.MENSAGEM = MontarCorpoEmail(email.ASSUNTO);

        //    List<string> destinatario = new List<string>();
        //    //Adiciona e-mails 
        //    destinatario.Add("luciana@hbf.com.br");
        //    destinatario.Add(usuario.EMAIL);

        //    var filial = baseController.ObterFilialIntranet(hidCodigoFilial.Value);
        //    if (filial != null)
        //        destinatario.Add(filial.email);

        //    var supervisor = baseController.BuscaSupervisorLoja(hidCodigoFilial.Value);
        //    if (supervisor != null)
        //    {
        //        int codigoUsuario = supervisor.codigo_usuario;
        //        var sup = baseController.BuscaUsuario(codigoUsuario);
        //        if (sup != null)
        //            destinatario.Add(sup.EMAIL);
        //    }

        //    email.DESTINATARIOS = destinatario;

        //    if (destinatario.Count > 0)
        //        email.EnviarEmail();
        //}
        //private string MontarCorpoEmail(string assunto)
        //{
        //    int codigoUsuario = 0;
        //    codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("");

        //    sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
        //    sb.Append("<html>");
        //    sb.Append("<head>");
        //    sb.Append("    <title>Observaçao da Foto</title>");
        //    sb.Append("    <meta charset='UTF-8' />");
        //    sb.Append("</head>");
        //    sb.Append("<body>");
        //    sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("        background: white; white-space: nowrap;'>");
        //    sb.Append("        <br />");
        //    sb.Append("        <div id='divEmail' align='left'>");
        //    sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
        //    sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("                background: white; white-space: nowrap;'>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td style='text-align:center;'>");
        //    sb.Append("                        <h2> " + assunto.Replace("Intranet: Foto", "") + "</h2>");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td style='text-align:center;'>");
        //    sb.Append("                        <hr />");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td style='line-height: 20px;'>");
        //    sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
        //    sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        //    sb.Append("                            background: white; white-space: nowrap;'>");
        //    sb.Append("                            <tr style='text-align: left;'>");
        //    sb.Append("                                <td style='width: 235px;'>");
        //    sb.Append("                                    Filial:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + txtFilial.Text);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");

        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Semana:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + txtSemana.Text);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");

        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Setor:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + txtSetor.Text);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");

        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Foto:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                     <img alt='' Width='' Height='' src='http://cmaxweb.dnsalias.com:8585" + imgFoto.ImageUrl.Replace("~", "") + "' />&nbsp;");
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");

        //    sb.Append("                            <tr>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    Observação:");
        //    sb.Append("                                </td>");
        //    sb.Append("                                <td>");
        //    sb.Append("                                    " + txtEmail.Text);
        //    sb.Append("                                </td>");
        //    sb.Append("                            </tr>");


        //    sb.Append("                        </table>");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td>");
        //    sb.Append("                        &nbsp;");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("                <tr>");
        //    sb.Append("                    <td>");
        //    sb.Append("                        &nbsp;");
        //    sb.Append("                    </td>");
        //    sb.Append("                </tr>");
        //    sb.Append("            </table>");
        //    sb.Append("        </div>");
        //    sb.Append("        <br />");
        //    sb.Append("        <br />");
        //    sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
        //    sb.Append("    </div>");
        //    sb.Append("</body>");
        //    sb.Append("</html>");


        //    return sb.ToString();
        //}
        #endregion




    }
}


