using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using System.Data.Common;
using System.Transactions;


namespace DAL
{
    public class JuridicoController
    {
        DCDataContext db;

        //JUR_TIPO_PROCESSO
        public List<JUR_TIPO_PROCESSO> ObterTipoProcesso()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_TIPO_PROCESSOs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public JUR_TIPO_PROCESSO ObterTipoProcesso(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_TIPO_PROCESSOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirTipoProcesso(JUR_TIPO_PROCESSO _tpProcesso)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.JUR_TIPO_PROCESSOs.InsertOnSubmit(_tpProcesso);
                db.SubmitChanges();

                return _tpProcesso.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarTipoProcesso(JUR_TIPO_PROCESSO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_TIPO_PROCESSO _tpProcesso = ObterTipoProcesso(_novo.CODIGO);

            if (_tpProcesso != null)
            {
                _tpProcesso.CODIGO = _novo.CODIGO;
                _tpProcesso.DESCRICAO = _novo.DESCRICAO;
                _tpProcesso.STATUS = _novo.STATUS;

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
        public void ExcluirTipoProcesso(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_TIPO_PROCESSO _tpProcesso = ObterTipoProcesso(codigo);

            if (_tpProcesso != null)
            {
                try
                {
                    db.JUR_TIPO_PROCESSOs.DeleteOnSubmit(_tpProcesso);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - JUR_TIPO_PROCESSO

        //JUR_TIPO_INSTANCIA
        public List<JUR_TIPO_INSTANCIA> ObterTipoInstancia()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_TIPO_INSTANCIAs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public JUR_TIPO_INSTANCIA ObterTipoInstancia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_TIPO_INSTANCIAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirTipoInstancia(JUR_TIPO_INSTANCIA _tpInstancia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.JUR_TIPO_INSTANCIAs.InsertOnSubmit(_tpInstancia);
                db.SubmitChanges();

                return _tpInstancia.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarTipoInstancia(JUR_TIPO_INSTANCIA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_TIPO_INSTANCIA _tpInstancia = ObterTipoInstancia(_novo.CODIGO);

            if (_tpInstancia != null)
            {
                _tpInstancia.CODIGO = _novo.CODIGO;
                _tpInstancia.DESCRICAO = _novo.DESCRICAO;
                _tpInstancia.ORDEM = _novo.ORDEM;
                _tpInstancia.STATUS = _novo.STATUS;

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
        public void ExcluirTipoInstancia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_TIPO_INSTANCIA _tpInstancia = ObterTipoInstancia(codigo);

            if (_tpInstancia != null)
            {
                try
                {
                    db.JUR_TIPO_INSTANCIAs.DeleteOnSubmit(_tpInstancia);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - JUR_TIPO_INSTANCIA

        //JUR_PROCESSO
        public List<JUR_PROCESSO> ObterProcesso()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_PROCESSOs
                    orderby m.NUMERO
                    select m).ToList();
        }
        public JUR_PROCESSO ObterProcesso(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_PROCESSOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirProcesso(JUR_PROCESSO _processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.JUR_PROCESSOs.InsertOnSubmit(_processo);
                db.SubmitChanges();

                return _processo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProcesso(JUR_PROCESSO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_PROCESSO _processo = ObterProcesso(_novo.CODIGO);

            if (_processo != null)
            {
                _processo.CODIGO = _novo.CODIGO;
                _processo.DATA_RECEBIMENTO = _novo.DATA_RECEBIMENTO;
                _processo.REQUERENTE = _novo.REQUERENTE;
                _processo.CARGO = _novo.CARGO;
                _processo.JUR_TIPO_PROCESSO = _novo.JUR_TIPO_PROCESSO;
                _processo.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _processo.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _processo.STATUS = _novo.STATUS;

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
        public void ExcluirProcesso(JUR_PROCESSO _excluir)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_PROCESSO _processo = ObterProcesso(_excluir.CODIGO);

            if (_processo != null)
            {
                _processo.CODIGO = _excluir.CODIGO;
                _processo.STATUS = _excluir.STATUS;

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
        //FIM - JUR_PROCESSO

        //JUR_PROCESSO
        public List<JUR_PROCESSO_INSTANCIA> ObterProcessoInstancia()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_PROCESSO_INSTANCIAs
                    orderby m.JUR_TIPO_INSTANCIA1.ORDEM
                    select m).ToList();
        }
        public JUR_PROCESSO_INSTANCIA ObterProcessoInstancia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.JUR_PROCESSO_INSTANCIAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirProcessoInstancia(JUR_PROCESSO_INSTANCIA _processoInstancia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.JUR_PROCESSO_INSTANCIAs.InsertOnSubmit(_processoInstancia);
                db.SubmitChanges();

                return _processoInstancia.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProcessoInstancia(JUR_PROCESSO_INSTANCIA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_PROCESSO_INSTANCIA _processoInstancia = ObterProcessoInstancia(_novo.CODIGO);

            if (_processoInstancia != null)
            {
                _processoInstancia.CODIGO = _novo.CODIGO;
                _processoInstancia.DATA_CONDENACAO = _novo.DATA_CONDENACAO;
                _processoInstancia.FORMA_PGTO = _novo.FORMA_PGTO;
                _processoInstancia.OBSERVACAO = _novo.OBSERVACAO;
                _processoInstancia.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _processoInstancia.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

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
        public void ExcluirProcessoInstancia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            JUR_PROCESSO_INSTANCIA _processoInstancia = ObterProcessoInstancia(codigo);

            if (_processoInstancia != null)
            {
                try
                {
                    db.JUR_PROCESSO_INSTANCIAs.DeleteOnSubmit(_processoInstancia);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - JUR_PROCESSO

        //RELATORIO
        public List<SP_OBTER_PROCESSO_JURIDICOResult> ObterProcessoJuridico(DateTime? dataIni, DateTime? dataFim, string numero, int? tipoProcesso, int? instancia, char? status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_PROCESSO_JURIDICO(dataIni, dataFim, numero, tipoProcesso, instancia, status) select m).ToList();
        }
        //FIM - RELATORIO

    }
}
