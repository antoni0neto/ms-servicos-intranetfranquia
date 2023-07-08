using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Relatorios.mod_rh.email
{
    public static class EmailFuncionario
    {


        public static void EnviarEmailAlteracaoFun(RH_FUNCIONARIO funcionario, bool inclusao, USUARIO usuario, string telefone, string UF)
        {

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  " + ((inclusao) ? "Inclusão" : "Alteração") + " de Funcionário - " + funcionario.NOME.Trim();
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(funcionario, inclusao, usuario, telefone, UF);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails RH
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(75, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private static string MontarCorpoEmail(RH_FUNCIONARIO funcionario, bool inclusao, USUARIO usuario, string telefone, string UF)
        {

            BaseController baseController = new BaseController();

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Alteração de Funcionário</title>");
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
            sb.Append("                        <h2>" + ((inclusao) ? "Inclusão" : "Alteração") + " de Funcionário</h2>");
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
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Filial:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaFilialCodigo(Convert.ToInt32(funcionario.CODIGO_FILIAL)).FILIAL.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    Nome:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + funcionario.VENDEDOR + " - " + funcionario.NOME);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    CPF:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + funcionario.CPF);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cargo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + funcionario.DESC_CARGO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Telefone:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + telefone);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    UF:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + UF);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Admissão:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((funcionario.DATA_ADMISSAO == null) ? "-" : Convert.ToDateTime(funcionario.DATA_ADMISSAO).ToString("dd/MM/yyyy")));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    <span style='color: red;'>Demissão</span>:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ((funcionario.DATA_DEMISSAO == null) ? "-" : Convert.ToDateTime(funcionario.DATA_DEMISSAO).ToString("dd/MM/yyyy")));
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
            sb.Append("        <span>Enviado por: " + (usuario.NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }





    }
}