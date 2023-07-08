using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.Net.Mail;
using System.Net;

namespace Relatorios
{
    public class notificacao_envio
    {
        public static void GerarNotificacao(int codigo, string tabela, int tela, string msg)
        {
            try
            {
                NotificacaoController notController = new NotificacaoController();

                NOTIFICACAO not = new NOTIFICACAO();
                not.CODIGO_TABELA = codigo;
                not.TABELA = tabela;
                not.TELA = tela;
                not.MENSAGEM = msg;
                not.DATA_INCLUSAO = DateTime.Now;

                notController.InserirNotificacao(not);

                // TELA 1 - desenv_ficha_tecnica.aspx
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}