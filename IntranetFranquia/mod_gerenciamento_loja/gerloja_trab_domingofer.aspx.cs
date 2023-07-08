using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.Text;
using System.IO;

namespace Relatorios
{
    public partial class gerloja_trab_domingofer : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_rh/rh_menu.aspx";

                CarregarSupervisores();
                CarregarSemanaRH();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarSupervisores()
        {
            var supers = baseController.BuscaUsuarioPerfil(3);

            supers = supers.OrderBy(p => p.NOME_USUARIO).ToList();

            supers.Insert(0, new USUARIO { CODIGO_USUARIO = 0, NOME_USUARIO = "" });

            ddlSupervisor.DataSource = supers;
            ddlSupervisor.DataBind();


        }
        private void CarregarSemanaRH()
        {
            var semana = rhController.ObterSemanaRH2120("");
            semana.Insert(0, new SP_OBTER_RH_SEMANAResult { SEMANA = "Selecione" });
            ddlSemana.DataSource = semana;
            ddlSemana.DataBind();

        }

        #endregion

        private void CarregarTrabDomingo()
        {

            var trab = rhController.ObterTrabDomFer(ddlSemana.SelectedValue, Convert.ToInt32(ddlSupervisor.SelectedValue));

            gvTrabDomingo.DataSource = trab;
            gvTrabDomingo.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                labErro.Text = "";
                if (ddlSemana.SelectedValue == "Selecione")
                {
                    labErro.Text = "Informe a Competência.";
                    return;
                }

                CarregarTrabDomingo();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvTrabDomingo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_TRAB_DOMFERResult trab = e.Row.DataItem as SP_OBTER_TRAB_DOMFERResult;

                    TextBox txtTotDomingo = e.Row.FindControl("txtTotDomingo") as TextBox;
                    txtTotDomingo.Text = trab.TOT_DOMINGO.ToString();

                    TextBox txtTotFeriado = e.Row.FindControl("txtTotFeriado") as TextBox;
                    txtTotFeriado.Text = trab.TOT_FERIADO.ToString();

                    Label labDataAtualizacao = e.Row.FindControl("labDataAtualizacao") as Label;
                    labDataAtualizacao.Text = (trab.DATA_ATUALIZACAO == null) ? "-" : Convert.ToDateTime(trab.DATA_ATUALIZACAO).ToString("dd/MM/yyyy HH:mm");

                    //CODIGO_SUPER, CODIGO_FILIAL, VENDEDOR, CODIGO_TRAB
                    Button btAtualizar = e.Row.FindControl("btAtualizar") as Button;
                    btAtualizar.CommandArgument = trab.CODIGO_SUPER.ToString() + "," + trab.CODIGO_FILIAL.Trim() + "," + trab.VENDEDOR.Trim() + "," + trab.CODIGO_TRAB.ToString();

                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                //CODIGO_SUPER, CODIGO_FILIAL, VENDEDOR, CODIGO_TRAB
                Button bt = (Button)sender;

                var valAr = bt.CommandArgument.Split(',');
                var codigoSuper = Convert.ToInt32(valAr[0].ToString().Trim());
                var codigoFilial = valAr[1].Trim().ToString().Trim();
                var vendedor = valAr[2].Trim().ToString().Trim();
                var codigoTrab = Convert.ToInt32(valAr[3].ToString().Trim());

                GridViewRow row = (GridViewRow)bt.NamingContainer;

                TextBox txtTotDomingo = row.FindControl("txtTotDomingo") as TextBox;
                TextBox txtTotFeriado = row.FindControl("txtTotFeriado") as TextBox;

                var totDomingo = (txtTotDomingo.Text.Trim() == "") ? 0 : Convert.ToInt32(txtTotDomingo.Text.Trim());
                var totFeriado = (txtTotFeriado.Text.Trim() == "") ? 0 : Convert.ToInt32(txtTotFeriado.Text.Trim());

                if (codigoTrab > 0)
                {
                    var altera = rhController.ObterTrabDomFer(codigoTrab);
                    altera.TOT_DOMINGO = totDomingo;
                    altera.TOT_FERIADO = totFeriado;
                    altera.DATA_ATUALIZACAO = DateTime.Now;

                    rhController.AtualizarTrabDomFer(altera);
                }
                else
                {
                    var novo = new LOJA_TRAB_DOMFER();
                    novo.CODIGO_USUARIO = codigoSuper;
                    novo.CODIGO_FILIAL = codigoFilial;
                    novo.VENDEDOR = vendedor;
                    novo.TOT_DOMINGO = totDomingo;
                    novo.TOT_FERIADO = totFeriado;

                    var semana = rhController.ObterSemanaRH2120(ddlSemana.SelectedValue).FirstOrDefault();

                    novo.DATA_INI = Convert.ToDateTime(semana.DATA_INI);
                    novo.DATA_FIM = Convert.ToDateTime(semana.DATA_FIM);
                    novo.DATA_ATUALIZACAO = DateTime.Now;

                    rhController.InserirTrabDomFer(novo);
                }

                CarregarTrabDomingo();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlSemana.SelectedValue == "Selecione")
                {
                    labErro.Text = "Para imprimir o relatório, selecione a Competência.";
                    return;
                }

                if (ddlSupervisor.SelectedValue == "0")
                {
                    labErro.Text = "Para imprimir o relatório, selecione o Supervisor.";
                    return;
                }

                var trabDom = rhController.ObterTrabDomFer(ddlSemana.SelectedValue, Convert.ToInt32(ddlSupervisor.SelectedValue));
                if (trabDom != null && trabDom.Count() > 0)
                {
                    GerarRelatorio(trabDom);
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }
        private void GerarRelatorio(List<SP_OBTER_TRAB_DOMFERResult> trabDomFer)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "TRAB_DOMFER" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(trabDomFer));
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
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_TRAB_DOMFERResult> trabDomFe)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarFormulario(trabDomFe);

            return _texto;
        }
        private StringBuilder MontarFormulario(List<SP_OBTER_TRAB_DOMFERResult> trabDomFe)
        {
            StringBuilder _texto = new StringBuilder();
            _texto.AppendLine("");

            var supervisor = baseController.BuscaUsuario(trabDomFe[0].CODIGO_SUPER).NOME_USUARIO;
            var dataIni = trabDomFe[0].DATA_INI;
            var dataFim = trabDomFe[0].DATA_FIM;

            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Domingos e Feriados Trabalhados</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload='window.print();'>");
            _texto.AppendLine("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("        background: white; white-space: nowrap;'>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <div id='divHB' align='left'>");
            _texto.AppendLine("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                background: white; white-space: nowrap;'>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <h1>Domingos e Feriados Trabalhados dos Gerentes</h1>");
            _texto.AppendLine("                        <h3>REF.: VALE REFEIÇÃO</h3>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='line-height: 20px;'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                            background: white; white-space: nowrap;'>");
            _texto.AppendLine("                            <tr style='text-align: left;'>");
            _texto.AppendLine("                                <td style='width: 235px;'>");
            _texto.AppendLine("                                    Supervisor:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + supervisor);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Período:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + Convert.ToDateTime(dataIni).ToString("dd/MM/yyyy") + " A " + Convert.ToDateTime(dataFim).ToString("dd/MM/yyyy"));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        <table cellpadding='2' cellspacing='0' style='width: 650pt; padding: 0px; color: black;");
            _texto.AppendLine("                            font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            _texto.AppendLine("                            border: 1px solid #ccc;'>");
            _texto.AppendLine("                            <tr style='background-color: #ccc;'>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Filial");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Gerente");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td style='text-align: center;'>");
            _texto.AppendLine("                                    Tot Domingo");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td style='text-align: center;'>");
            _texto.AppendLine("                                    Tot Feriado");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");


            foreach (var trab in trabDomFe)
            {
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td style='border: 1px solid #ccc;'>");
                _texto.AppendLine("                                    " + trab.FILIAL);
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td style='border: 1px solid #ccc;'>");
                _texto.AppendLine("                                    " + trab.NOME);
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td style='text-align: center; border: 1px solid #ccc;'>");
                _texto.AppendLine("                                    " + trab.TOT_DOMINGO.ToString());
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td style='text-align: center; border: 1px solid #ccc;'>");
                _texto.AppendLine("                                    " + trab.TOT_FERIADO.ToString());
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
            }
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("            </table>");
            _texto.AppendLine("        </div>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <br />");


            var nomeUsuario = "Intranet";
            var usuario = (USUARIO)Session["USUARIO"];
            if (usuario != null)
                nomeUsuario = usuario.NOME_USUARIO;

            _texto.AppendLine("        <span>Enviado por: " + usuario.NOME_USUARIO + "</span>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");




            return _texto;
        }

        protected void btEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlSemana.SelectedValue == "Selecione")
                {
                    labErro.Text = "Para imprimir o relatório, selecione a Competência.";
                    return;
                }

                if (ddlSupervisor.SelectedValue == "0")
                {
                    labErro.Text = "Para imprimir o relatório, selecione o Supervisor.";
                    return;
                }

                var trabDom = rhController.ObterTrabDomFer(ddlSemana.SelectedValue, Convert.ToInt32(ddlSupervisor.SelectedValue));
                if (trabDom != null && trabDom.Count() > 0)
                {
                    EnviarEmail(trabDom);
                    labErro.Text = "E-mail enviado com sucesso.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }
        private void EnviarEmail(List<SP_OBTER_TRAB_DOMFERResult> trabDomFe)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";

            var supervisor = baseController.BuscaUsuario(trabDomFe[0].CODIGO_SUPER).NOME_USUARIO;
            var dataIni = trabDomFe[0].DATA_INI;
            var dataFim = trabDomFe[0].DATA_FIM;

            var semanaTit = Convert.ToDateTime(dataIni).ToString("dd/MM/yyyy") + " A " + Convert.ToDateTime(dataFim).ToString("dd/MM/yyyy");

            assunto = "[Intranet]: Trabalho Domingo/Feriado - " + semanaTit + " - Supervisor: " + baseController.BuscaUsuario(trabDomFe[0].CODIGO_SUPER).NOME_USUARIO;
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarFormulario(trabDomFe).ToString();

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(60, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }

    }
}
