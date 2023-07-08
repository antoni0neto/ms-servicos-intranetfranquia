using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace DAL
{
    public class NotificacaoController
    {
        DCDataContext db;

        //NOTIFICACAO
        // 1 - FICHA TECNICA
        // 2 - COMPRA DE MATERIAL
        // 3 - MATERIAL
        public List<NOTIFICACAO> ObterNotificacao(int tela)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.NOTIFICACAOs
                    where m.TELA == tela
                    orderby m.DATA_INCLUSAO
                    select m).ToList();
        }
        public NOTIFICACAO ObterNotificacaoCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.NOTIFICACAOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirNotificacao(NOTIFICACAO _not)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.NOTIFICACAOs.InsertOnSubmit(_not);
                db.SubmitChanges();

                return _not.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarNotificacao(NOTIFICACAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            NOTIFICACAO _not = ObterNotificacaoCodigo(_novo.CODIGO);

            if (_not != null)
            {
                _not.USUARIO_LEITURA = _novo.USUARIO_LEITURA;
                _not.DATA_LEITURA = _novo.DATA_LEITURA;

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirNotificacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            NOTIFICACAO _not = ObterNotificacaoCodigo(codigo);

            if (_not != null)
            {
                try
                {
                    db.NOTIFICACAOs.DeleteOnSubmit(_not);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM NOTIFICACAO
    }
}
