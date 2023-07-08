using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Relatorios
{
    public class email_envio
    {

        public string ASSUNTO { get; set; }
        public string MENSAGEM { get; set; }
        public string ANEXO { get; set; }
        public USUARIO REMETENTE { get; set; }
        public List<string> DESTINATARIOS { get; set; }

        public bool EnviarEmail()
        {
            try
            {
                UsuarioController usuarioController = new UsuarioController();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(usuarioController.BuscaPorCodigoUsuario(REMETENTE.CODIGO_USUARIO).EMAIL);

                foreach (string email in DESTINATARIOS.Distinct().ToList())
                    if (email != "")
                        mail.To.Add(email);

                mail.Subject = ASSUNTO;
                mail.IsBodyHtml = true;
                mail.Body = MENSAGEM;

                if (ANEXO != "")
                {
                    if (File.Exists(ANEXO))
                    {
                        Attachment att = new Attachment(ANEXO);
                        mail.Attachments.Add(att);
                    }
                }

                SmtpClient smtp = new SmtpClient("email-ssl.com.br");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Port = 587;
                smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

                smtp.Send(mail);
                //smtp.SendAsync(mail, null);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}