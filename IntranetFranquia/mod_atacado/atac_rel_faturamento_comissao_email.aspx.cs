using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using Relatorios.mod_desenvolvimento.modelo_pocket;
using System.Text;

namespace Relatorios
{
    public partial class atac_rel_faturamento_comissao_email : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (
                    Request.QueryString["r"] == null || Request.QueryString["r"] == "" ||
                    Request.QueryString["f"] == null || Request.QueryString["f"] == "" ||
                    Request.QueryString["a"] == null || Request.QueryString["a"] == "" ||
                    Request.QueryString["m"] == null || Request.QueryString["m"] == "" ||
                    Session["USUARIO"] == null)
                    Response.Redirect("atac_menu.aspx");

                try
                {

                    string representante = Request.QueryString["r"].ToString();
                    string filial = Request.QueryString["f"].ToString();
                    string ano = Request.QueryString["a"].ToString();
                    string mes = Request.QueryString["m"].ToString();

                    hidRepresentante.Value = representante;
                    hidFilial.Value = filial;
                    hidAno.Value = ano;
                    hidMes.Value = mes;

                    CarregarEmail();

                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }

            //Evitar duplo clique no botão
            btEnviarEmail.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviarEmail, null) + ";");
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");

        }

        private void CarregarEmail()
        {
            HtmlGenericControl divEmail;
            StringBuilder email;

            divEmail = new HtmlGenericControl();
            email = new StringBuilder();

            email = MontarEmailRepresentante(hidRepresentante.Value, hidFilial.Value, hidAno.Value, hidMes.Value, false, true);

            if (email != null)
            {
                //Descarregar hMTL na DIV
                divEmail.InnerHtml = email.ToString();
                pnlEmail.Controls.Add(divEmail);
            }
        }
        private void EnviarEmail()
        {
            string corpoEmail = "";

            //Montar corpo do e-mail
            corpoEmail = MontarEmailRepresentante(hidRepresentante.Value, hidFilial.Value, hidAno.Value, hidMes.Value, false, false).ToString();

            ATACADO_REP_EMAIL atacRepEmail = new ATACADO_REP_EMAIL();

            atacRepEmail.REPRESENTANTE = hidRepresentante.Value;
            atacRepEmail.FILIAL = hidFilial.Value;

            if (hidMes.Value.Length == 1)
                hidMes.Value = "0" + hidMes.Value;

            atacRepEmail.COMPETENCIA = Convert.ToDateTime(hidAno.Value + "-" + hidMes.Value + "-01");
            atacRepEmail.DATA_ENVIO = DateTime.Now;
            atacRepEmail.USUARIO_ENVIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            atacRepEmail.ENVIADO_PARA = hidEmail.Value;
            atacRepEmail.CONTEUDO_EMAIL = corpoEmail;

            int codigo = atacController.InserirAtacadoRepresentanteEmail(atacRepEmail);

            if (codigo <= 0)
                throw new Exception("ERRO AO INSERIR E-MAIL. EMAIL NÃO ENVIADO.");

            //******************************************************************************************************************
            //ENVIAR E-MAIL ****************************************************************************************************
            email_envio email = new email_envio();

            string mesExtenso = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(atacRepEmail.COMPETENCIA.Month).ToUpper();

            email.ASSUNTO = "FATURAMENTO MENSAL: " + hidFilial.Value.Trim() + " - " + mesExtenso + "/" + atacRepEmail.COMPETENCIA.Year.ToString();
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = corpoEmail;

            List<string> destinatario = new List<string>();
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(5, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            if (hidEmail.Value.Trim() != "")
            {
                string[] sEmail = hidEmail.Value.Trim().Split(';');
                if (sEmail != null && sEmail.Count() > 0)
                    foreach (string s in sEmail)
                        destinatario.Add(s);
            }

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();

        }
        protected void btEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                hidEmail.Value = "";
                string email = txtEmail.Text.Trim();

                if (email == "")
                {
                    labErro.Text = "Informe pelo menos um E-Mail.";
                    return;
                }

                email = email.Replace(",", ";");
                if (!Utils.WebControls.ValidarEmail(email))
                {
                    labErro.Text = "Informe E-Mail válido. (Exemplo: representante@hbf.com.br)";
                    return;
                }

                hidEmail.Value = email;
                txtEmail.Text = "";

                EnviarEmail();

                labErro.Font.Bold = true;
                labErro.Text = "E-Mail enviado com Sucesso.";

                hidEmail.Value = "";
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "COMISSAO_REP_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarEmailRepresentante(hidRepresentante.Value, hidFilial.Value, hidAno.Value, hidMes.Value, true, true));
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

        private StringBuilder MontarEmailRepresentante(string representante, string filial, string ano, string mes, bool print, bool center)
        {
            /*
             *  SE ALTERAR ESTA FUNÇÃO DEVERÁ SER ALTERADO A FUNCÃO QUE ENVIA E-MAIL AUTOMATICAMENTE. NOME: X
             *
             *
            * ***************************************************************************************************************************
             * ***************************************************************************************************************************
             * ***************************************************************************************************************************
             * ***************************************************************************************************************************
             * ***************************************************************************************************************************
             * ***************************************************************************************************************************
             * *************************************************************************************************************************** 
             * ***************************************************************************************************************************
             */

            //Obter dados
            BaseController baseController = new BaseController();
            string cnpj = "";
            string endereco = "";
            string razaoSocial = "";
            string dataIni = "";
            string dataFim = "";

            string razaoSocialFilial = "";
            string cnpjFilial = "";

            int qtdeTotal = 0;
            decimal valorTotal = 0;
            decimal valorTotalComissao = 0;
            decimal valorComissaoReceber = 0;

            var nomeCliFor = baseController.ObterCadastroCLIFOR(representante);
            if (nomeCliFor != null)
            {
                razaoSocial = nomeCliFor.RAZAO_SOCIAL.Trim();
                endereco = nomeCliFor.ENDERECO.Trim() + ", " + ((nomeCliFor.NUMERO == null) ? "" : (nomeCliFor.NUMERO.Trim() + ", ")) + ((nomeCliFor.COMPLEMENTO == null) ? "" : (nomeCliFor.COMPLEMENTO.Trim() + ", ")) + ((nomeCliFor.BAIRRO == null) ? "" : (nomeCliFor.BAIRRO.Trim() + ", ")) + ((nomeCliFor.CIDADE == null) ? "" : (nomeCliFor.CIDADE.Trim() + ", ")) + nomeCliFor.UF.Trim() + " - " + nomeCliFor.CEP.Trim();
                cnpj = nomeCliFor.CGC_CPF.Trim();
                if (cnpj.Length == 11)
                    cnpj = Convert.ToUInt64(cnpj).ToString(@"000\.000\.000\-00");
                else
                    cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            }

            var filialCliFor = baseController.ObterCadastroCLIFOR(filial);
            if (filialCliFor != null)
            {
                razaoSocialFilial = filialCliFor.RAZAO_SOCIAL.Trim();
                cnpjFilial = filialCliFor.CGC_CPF.Trim();
                if (cnpjFilial.Length == 11)
                    cnpjFilial = Convert.ToUInt64(cnpjFilial).ToString(@"000\.000\.000\-00");
                else
                    cnpjFilial = Convert.ToUInt64(cnpjFilial).ToString(@"00\.000\.000\/0000\-00");
            }

            if (mes.Length == 1)
                mes = "0" + mes;

            int ultimoDiaMes = DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes));
            dataIni = Convert.ToDateTime(ano + "-" + mes + "-01").ToString("dd/MM/yyyy");
            dataFim = Convert.ToDateTime(ano + "-" + mes + "-" + ultimoDiaMes).ToString("dd/MM/yyyy");

            var comissaoRepresentante = new AtacadoController().ObterComissaoRepresentante(Convert.ToDateTime(dataIni), Convert.ToDateTime(dataFim), representante).Where(p => p.FILIAL.Trim() == filial.Trim()).ToList();

            StringBuilder _texto = null;

            if (comissaoRepresentante != null && comissaoRepresentante.Count() > 0)
            {
                _texto = new StringBuilder();

                _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
                _texto.Append("<html>");
                _texto.Append("<head>");
                _texto.Append("    <title>FAT REPRESENTANTE</title>");
                _texto.Append("    <meta charset='UTF-8' />");
                _texto.Append("    <style type='text/css'>");
                _texto.Append("        @media print {");
                _texto.Append("            .background-force {");
                _texto.Append("                -webkit-print-color-adjust: exact;");
                _texto.Append("            }");
                _texto.Append("        }");
                _texto.Append("        .break {");
                _texto.Append("            page-break-before: always;");
                _texto.Append("        }");
                _texto.Append("    </style>");
                _texto.Append("</head>");
                _texto.Append("<body onload='" + ((print) ? "window.print();" : "") + "' style='padding: 0; margin: 0;'>");
                _texto.Append("    <div id='divRepresentante' align='" + ((center) ? "center" : "left") + "' class='' style='font: Calibri;'>");
                _texto.Append("        <table border='0' cellpadding='2' cellspacing='0' style='padding: 0px; color: black;");
                _texto.Append("        font-size: 9.0pt; font-weight: 700; font-family: Calibri; background: white; white-space: nowrap;'>");
                _texto.Append("            <tr>");
                _texto.Append("                <td valign='top' style='height: 950px;'>");
                _texto.Append("                    <div style='font: Calibri; border: 0px solid #696969; '>");
                _texto.Append("                        <table border='0' cellpadding='11' cellspacing='2' width='600px' style='padding: 0px;");
                _texto.Append("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td style='text-align:center; border: 0px solid #696969;' colspan='2' valign='middle'>");
                _texto.Append("                                    <span style='font-size: 16px;'>" + filial.Trim().ToUpper() + "</span>");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td style='font-size:14pt;'>");
                _texto.Append("                                    <br /><br />");
                _texto.Append("                                    FATURAMENTO MENSAL DE REPRESENTANTE");
                _texto.Append("                                </td>");
                _texto.Append("                                <td style='font-size:13pt;'>");
                _texto.Append("                                    Emissão: " + DateTime.Now.ToString("dd/MM/yyyy"));
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    &nbsp;");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    Ao representante, <br /><br />");
                _texto.Append("                                    " + representante.Trim() + " - " + razaoSocial.ToUpper() + "<br />");
                _texto.Append("                                    CNPJ: " + cnpj + "     <br />");
                _texto.Append("                                    " + endereco);
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    <br /><br />Prezados,<br />");
                _texto.Append("                                    <span style='font-size:10.5pt;'>");
                _texto.Append("                                        Seguem abaixo valores de pedidos Faturados, Devolvidos e Cancelados no período de");
                _texto.Append("                                        <span style='color:red; font-size:11.5pt;'>" + dataIni + "</span> a");
                _texto.Append("                                        <span style='color: red; font-size: 11.5pt;'>" + dataFim + "</span>.<br />");
                _texto.Append("                                        Comunicamos que a sua comissão será paga somente sobre os <span style='color:red; font-size:11.5pt;'>PEDIDOS FATURADOS</span>.");
                _texto.Append("                                    </span> <br /><br />");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                //FATURADOS
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    <table border='0' cellpadding='2' cellspacing='0' width='100%' style='font-size:8pt;'>");
                _texto.Append("                                        <tr class='background-force' style='line-height:19px;'>");
                _texto.Append("                                            <td colspan='7' style='background-color: #BEBEBE; color: #000; border: 1px solid #000; border-bottom: none;'>");
                _texto.Append("                                                <font size='3'>PEDIDOS FATURADOS</font>");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr class='background-force' style='background-color: #e9e7e7; font-size: 9.5pt;'>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; '>");
                _texto.Append("                                                Cliente");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000;'>");
                _texto.Append("                                                Coleção");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 65px;'>");
                _texto.Append("                                                Pedido");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000;'>");
                _texto.Append("                                                Motivo");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 80px;'>");
                _texto.Append("                                                Qtde Total");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 80px;'>");
                _texto.Append("                                                Valor Total");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; width: 90px;'>");
                _texto.Append("                                                Comissão");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");

                qtdeTotal = 0;
                valorTotal = 0;
                valorTotalComissao = 0;
                foreach (var faturado in comissaoRepresentante.Where(p => p.TIPO == "F" || (p.TIPO == "D" && p.COMISSAO > 0)))
                {
                    _texto.Append("                                        <tr>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + faturado.CLIENTE + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + faturado.DESC_COLECAO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + faturado.PEDIDO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + faturado.MOTIVO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align:right; '>");
                    _texto.Append("                                                " + faturado.QTDE_TOTAL.ToString() + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align:right; '>");
                    _texto.Append("                                                R$ " + Convert.ToDecimal(faturado.VALOR_TOTAL).ToString("###,###,###,###,##0.00") + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000; text-align: right;'>");
                    _texto.Append("                                                R$ " + Convert.ToDecimal(faturado.COMISSAO).ToString("###,###,###,###,##0.00") + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                        </tr>");

                    qtdeTotal += Convert.ToInt32(faturado.QTDE_TOTAL);
                    valorTotal += Convert.ToDecimal(faturado.VALOR_TOTAL);
                    valorTotalComissao += Convert.ToDecimal(faturado.COMISSAO);
                }
                _texto.Append("                                        <tr>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000;'>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr class='background-force' style='background-color: #e9e7e7; font-size: 11.5pt;'>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; font-size: 12pt;'>");
                _texto.Append("                                                &nbsp;<b>Total</b>");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align: right; font-size: 12pt; '>");
                _texto.Append("                                                <b>" + qtdeTotal.ToString() + "</b>&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align: right; font-size: 12pt; '>");
                _texto.Append("                                                <strong>R$ " + valorTotal.ToString("###,###,###,###,##0.00") + "</strong>&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000; text-align: right; font-size:12pt;'>");
                _texto.Append("                                                <strong>R$ " + valorTotalComissao.ToString("###,###,###,###,##0.00") + "</strong>&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                    </table>");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");

                //DEVOLVIDOS
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    <table border='0' cellpadding='2' cellspacing='0' width='100%' style='font-size:8pt;'>");
                _texto.Append("                                        <tr class='background-force' style='line-height:19px;'>");
                _texto.Append("                                            <td colspan='7' style='background-color: #BEBEBE; color: #000; border: 1px solid #000; border-bottom: none;'>");
                _texto.Append("                                                <font size='3'>PEDIDOS DEVOLVIDOS</font>");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr class='background-force' style='background-color: #e9e7e7; font-size: 9.5pt;'>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; '>");
                _texto.Append("                                                Cliente");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000;'>");
                _texto.Append("                                                Coleção");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 65px;'>");
                _texto.Append("                                                Pedido");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000;'>");
                _texto.Append("                                                Motivo");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 80px;'>");
                _texto.Append("                                                Qtde Total");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 80px;'>");
                _texto.Append("                                                Valor Total");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; width: 90px;'>");
                _texto.Append("                                                Comissão");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");

                //SOMA TOTAL A RECEBER
                valorComissaoReceber = valorTotalComissao;

                qtdeTotal = 0;
                valorTotal = 0;
                valorTotalComissao = 0;
                foreach (var devolvido in comissaoRepresentante.Where(p => p.TIPO == "D"))
                {
                    _texto.Append("                                        <tr>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + devolvido.CLIENTE + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + devolvido.DESC_COLECAO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + devolvido.PEDIDO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + devolvido.MOTIVO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align:right; '>");
                    _texto.Append("                                                " + devolvido.QTDE_TOTAL.ToString() + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align:right; '>");
                    _texto.Append("                                                R$ " + Convert.ToDecimal(devolvido.VALOR_TOTAL * (-1)).ToString("###,###,###,###,##0.00") + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000; text-align: right;'>");
                    _texto.Append("                                                R$ " + Convert.ToDecimal(devolvido.COMISSAO).ToString("###,###,###,###,##0.00") + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                        </tr>");

                    qtdeTotal += Convert.ToInt32(devolvido.QTDE_TOTAL);
                    valorTotal += Convert.ToDecimal(devolvido.VALOR_TOTAL);
                    valorTotalComissao += Convert.ToDecimal(devolvido.COMISSAO);
                }
                _texto.Append("                                        <tr>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000;'>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr class='background-force' style='background-color: #e9e7e7; font-size: 9.5pt;'>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; font-size: 9pt;'>");
                _texto.Append("                                                &nbsp;<b>Total</b>");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align: right; font-size: 9pt; '>");
                _texto.Append("                                                " + qtdeTotal.ToString() + "&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align: right; font-size: 9pt; '>");
                _texto.Append("                                                R$ " + (valorTotal * (-1)).ToString("###,###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000; text-align: right; font-size:9pt;'>");
                _texto.Append("                                                R$ " + valorTotalComissao.ToString("###,###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr style='line-height:10px;'>");
                _texto.Append("                                            <td colspan='7' style='text-align:right;'>");
                _texto.Append("                                                Este quadro é apenas para visualização de pedidos devolvidos.");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                    </table>");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");

                //CANCELADOS
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    <table border='0' cellpadding='2' cellspacing='0' width='100%' style='font-size:8pt;'>");
                _texto.Append("                                        <tr class='background-force' style='line-height:19px;'>");
                _texto.Append("                                            <td colspan='7' style='background-color: #BEBEBE; color: #000; border: 1px solid #000; border-bottom: none;'>");
                _texto.Append("                                                <font size='3'>PEDIDOS CANCELADOS</font>");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr class='background-force' style='background-color: #e9e7e7; font-size: 9.5pt;'>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; '>");
                _texto.Append("                                                Cliente");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000;'>");
                _texto.Append("                                                Coleção");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 65px;'>");
                _texto.Append("                                                Pedido");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000;'>");
                _texto.Append("                                                Motivo");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 80px;'>");
                _texto.Append("                                                Qtde Total");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; width: 80px;'>");
                _texto.Append("                                                Valor Total");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; width: 90px;'>");
                _texto.Append("                                                Comissão");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");

                qtdeTotal = 0;
                valorTotal = 0;
                valorTotalComissao = 0;
                foreach (var cancelado in comissaoRepresentante.Where(p => p.TIPO == "C"))
                {
                    _texto.Append("                                        <tr>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + cancelado.CLIENTE + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + cancelado.DESC_COLECAO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + cancelado.PEDIDO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                    _texto.Append("                                                &nbsp;" + cancelado.MOTIVO + "");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align:right; '>");
                    _texto.Append("                                                " + cancelado.QTDE_TOTAL.ToString() + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align:right; '>");
                    _texto.Append("                                                R$ " + Convert.ToDecimal(cancelado.VALOR_TOTAL).ToString("###,###,###,###,##0.00") + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000; text-align: right;'>");
                    _texto.Append("                                                R$ " + Convert.ToDecimal(cancelado.COMISSAO).ToString("###,###,###,###,##0.00") + "&nbsp;");
                    _texto.Append("                                            </td>");
                    _texto.Append("                                        </tr>");

                    qtdeTotal += Convert.ToInt32(cancelado.QTDE_TOTAL);
                    valorTotal += Convert.ToDecimal(cancelado.VALOR_TOTAL);
                    valorTotalComissao += Convert.ToDecimal(cancelado.COMISSAO);
                }
                _texto.Append("                                        <tr>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000;'>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr class='background-force' style='background-color: #e9e7e7; font-size: 9.5pt;'>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; font-size: 9pt;'>");
                _texto.Append("                                                &nbsp;<b>Total</b>");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; '>");
                _texto.Append("                                                &nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align: right; font-size: 9pt; '>");
                _texto.Append("                                                " + qtdeTotal.ToString() + "&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-right: solid 0px #000; border-top: solid 0px #000; text-align: right; font-size: 9pt; '>");
                _texto.Append("                                                R$ " + valorTotal.ToString("###,###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                            <td style='border: solid 1px #000; border-top: solid 0px #000; text-align: right; font-size:9pt;'>");
                _texto.Append("                                                R$ " + valorTotalComissao.ToString("###,###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                        <tr style='line-height:10px;'>");
                _texto.Append("                                            <td colspan='7' style='text-align:right;'>");
                _texto.Append("                                                Este quadro é apenas para visualização de pedidos cancelados.");
                _texto.Append("                                            </td>");
                _texto.Append("                                        </tr>");
                _texto.Append("                                    </table>");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    <span style='font-size:11.5pt;'>VALOR TOTAL DA COMISSÃO A SER PAGO:</span>&nbsp;<span style='color:red; font-size:13.0pt;'>R$ " + valorComissaoReceber.ToString("###,###,###,###,##0.00") + "</span>");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    &nbsp;");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    Atenciosamente,");
                _texto.Append("                                    <br />");
                _texto.Append("                                    <br />");
                _texto.Append("                                    " + razaoSocialFilial + "<br />");
                _texto.Append("                                    CNPJ: " + cnpjFilial + "<br />");
                _texto.Append("                                    <img alt='HBF' Width='60px' Height='60px' src='http://cmaxweb.dnsalias.com:8585/image/hbf_mini.jpg' />");
                _texto.Append("                                    <br />");
                _texto.Append("                                    <br />");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                            <tr>");
                _texto.Append("                                <td colspan='2'>");
                _texto.Append("                                    <p><font face='Calibri' size='2' color='#696969'>O conteúdo desta mensagem é de uso restrito e confidencial. Estas informações não podem ser divulgadas sem ");
                _texto.Append("                                    prévia autorização<br /> escrita. Se você não é o destinatário desta mensagem, ou o responsável pela sua entrega, apague-a ");
                _texto.Append("                                    imediatamente e avise ao <br />remetente, respondendo a esta mensagem.</font></p>");
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
                _texto.Append("                        </table>");
                _texto.Append("                        <br />");
                _texto.Append("                        <br />");
                _texto.Append("                        <br />");
                _texto.Append("                    </div>");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("        </table>");
                _texto.Append("    </div>");
                _texto.Append("</body>");
                _texto.Append("</html>");
            }

            return _texto;
        }

    }
}
