using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace DAL
{
    public class EnvioEmail
    {
        DCDataContext db;

        public void marcaEmailEnviado(IMPORTACAO_NEL nelNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_NEL nel = (from n in db.IMPORTACAO_NELs where n.CODIGO_NEL == nelNew.CODIGO_NEL select n).SingleOrDefault();

            if (nel != null)
            {
                nel.FLAG_LI = nelNew.FLAG_LI;
                nel.FLAG_BOOKING = nelNew.FLAG_BOOKING;
                nel.FLAG_EMBARQUE = nelNew.FLAG_EMBARQUE;
            }

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                string men = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") +
                        " - Erro na Landing Page: Não foi possível marcar o e-mail enviado \r\n" +
                        "LandingPage: NinhoVerdeII \r\n" +
                        "Erro: " + ex.Message + " \r\n";
            }
        }

        public bool EnviaEmail(string email, string body)
        {
            try
            {
                System.Net.Mail.MailMessage meg = new System.Net.Mail.MailMessage();

                meg.To.Add(email);
                meg.Subject = "Teste - Nel";
                meg.IsBodyHtml = true;
                meg.From = new System.Net.Mail.MailAddress("intranet@hbf.com.br");


                meg.Body = body;

                System.Net.Mail.SmtpClient mailsender = new System.Net.Mail.SmtpClient();

                SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");

                smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);
                smtp.Send(meg);

                return true;
            }
            catch (SmtpException smtpException)
            {
                string men = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") +
                        " - Erro na Landing Page: Não foi possível enviar o e-mail \r\n" +
                        "Email.: " + email + "\r\n" +
                        "LandingPage: NinhoVerdeII \r\n" +
                        "SmtpException \r\n" +
                        "Erro Message: " + smtpException.Message + " \r\n" +
                        "Erro InnerException: " + smtpException.InnerException + " \r\n";
                return false;
            }
            catch (Exception ex)
            {
                string men = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") +
                        " - Erro na Landing Page: Não foi possível enviar o e-mail \r\n" +
                        "Email.: " + email + "\r\n" +
                        "LandingPage: NinhoVerdeII \r\n" +
                        "Exception \r\n" +
                        "Erro Message:  " + ex.Message + " \r\n" +
                        "Erro InnerException: " + ex.InnerException + " \r\n";
                return false;
            }
        }
        
        public void EnviaEmails()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            
            //Li
            List<IMPORTACAO_NEL> listaNel = (from n in db.IMPORTACAO_NELs where n.FLAG_LI == false orderby n.CODIGO_NEL select n).ToList();

            if (listaNel != null && listaNel.Count() > 0)
            {
                foreach (IMPORTACAO_NEL nel in listaNel)
                {
                    if (nel.DATA_DESPACHANTE != null)
                    {
                        DateTime date = new DateTime(Convert.ToDateTime(nel.DATA_DESPACHANTE).Year, Convert.ToDateTime(nel.DATA_DESPACHANTE).Month, Convert.ToDateTime(nel.DATA_DESPACHANTE).Day);

                        int i = 0;

                        while (i < 3)
                        {
                            switch (date.DayOfWeek)
                            {
                                case DayOfWeek.Saturday:
                                    date = date.AddDays(2);
                                    break;

                                case DayOfWeek.Sunday:
                                    date = date.AddDays(1);
                                    break;

                                default:
                                    date = date.AddDays(1);
                                    i++;
                                    break;
                            }
                        }

                        string data = date.ToShortDateString();

                        DateTime dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        if (date.ToShortDateString().Equals(dateNow.ToShortDateString()))
                        {
                            if (EnviaEmail("flavia.luz@hbf.com.br", "NeL: " + nel.DESCRICAO_NEL + " liberada nesta data pelo Despachante !!!"))
                            {
                                nel.FLAG_LI = true;

                                marcaEmailEnviado(nel);
                            }
                        }
                    }
                }
            }

            //Booking
            listaNel = (from n in db.IMPORTACAO_NELs where n.FLAG_BOOKING == false orderby n.CODIGO_NEL select n).ToList();

            if (listaNel != null && listaNel.Count() > 0)
            {
                foreach (IMPORTACAO_NEL nel in listaNel)
                {
                    if (nel.DATA_LI != null)
                    {
                        if (nel.DATA_BOOKING == null)
                        {
                            if (EnviaEmail("flavia.luz@hbf.com.br", "NeL: " + nel.DESCRICAO_NEL + " diferida sem Data de Booking definida !!!"))
                            {
                                nel.FLAG_BOOKING = true;

                                marcaEmailEnviado(nel);
                            }
                        }
                    }
                }
            }

            //Embarque
            listaNel = (from n in db.IMPORTACAO_NELs where n.FLAG_EMBARQUE == false orderby n.CODIGO_NEL select n).ToList();

            if (listaNel != null && listaNel.Count() > 0)
            {
                foreach (IMPORTACAO_NEL nel in listaNel)
                {
                    if (nel.DATA_BOOKING != null)
                    {
                        if (nel.DATA_EMBARQUE == null)
                        {
                            if (EnviaEmail("flavia.luz@hbf.com.br", "NeL: " + nel.DESCRICAO_NEL + " com Booking sem Data de Embarque definida !!!"))
                            {
                                nel.FLAG_EMBARQUE = true;

                                marcaEmailEnviado(nel);
                            }
                        }
                    }
                }
            }
        }
    }
}
