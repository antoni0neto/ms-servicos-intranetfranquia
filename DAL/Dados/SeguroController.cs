using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using System.Data.Common;
using System.Transactions;


namespace DAL
{
    public class SeguroController
    {
        DCDataContext db;

        //SEG_TIPO_SEGURO
        public List<SEG_TIPO_SEGURO> ObterTipoSeguro()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_TIPO_SEGUROs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public SEG_TIPO_SEGURO ObterTipoSeguro(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_TIPO_SEGUROs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirTipoSeguro(SEG_TIPO_SEGURO _tpSeguro)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_TIPO_SEGUROs.InsertOnSubmit(_tpSeguro);
                db.SubmitChanges();

                return _tpSeguro.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarTipoSeguro(SEG_TIPO_SEGURO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_TIPO_SEGURO _tpSeguro = ObterTipoSeguro(_novo.CODIGO);

            if (_tpSeguro != null)
            {
                _tpSeguro.CODIGO = _novo.CODIGO;
                _tpSeguro.DESCRICAO = _novo.DESCRICAO;
                _tpSeguro.STATUS = _novo.STATUS;

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
        public void ExcluirTipoSeguro(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_TIPO_SEGURO _tpSeguro = ObterTipoSeguro(codigo);

            if (_tpSeguro != null)
            {
                try
                {
                    db.SEG_TIPO_SEGUROs.DeleteOnSubmit(_tpSeguro);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_TIPO_SEGURO

        //SEG_SEGURADORA
        public List<SEG_SEGURADORA> ObterSeguradora()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGURADORAs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public SEG_SEGURADORA ObterSeguradora(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGURADORAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirSeguradora(SEG_SEGURADORA _seguradora)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_SEGURADORAs.InsertOnSubmit(_seguradora);
                db.SubmitChanges();

                return _seguradora.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSeguradora(SEG_SEGURADORA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURADORA _seguradora = ObterSeguradora(_novo.CODIGO);

            if (_seguradora != null)
            {
                _seguradora.CODIGO = _novo.CODIGO;
                _seguradora.DESCRICAO = _novo.DESCRICAO;
                _seguradora.STATUS = _novo.STATUS;

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
        public void ExcluirSeguradora(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURADORA _seguradora = ObterSeguradora(codigo);

            if (_seguradora != null)
            {
                try
                {
                    db.SEG_SEGURADORAs.DeleteOnSubmit(_seguradora);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_SEGURADORA

        //SEG_CORRETOR
        public List<SEG_CORRETOR> ObterCorretor()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_CORRETORs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public SEG_CORRETOR ObterCorretor(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_CORRETORs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirCorretor(SEG_CORRETOR _corretor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_CORRETORs.InsertOnSubmit(_corretor);
                db.SubmitChanges();

                return _corretor.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCorretor(SEG_CORRETOR _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_CORRETOR _corretor = ObterCorretor(_novo.CODIGO);

            if (_corretor != null)
            {
                _corretor.CODIGO = _novo.CODIGO;
                _corretor.DESCRICAO = _novo.DESCRICAO;
                _corretor.STATUS = _novo.STATUS;

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
        public void ExcluirCorretor(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_CORRETOR _corretor = ObterCorretor(codigo);

            if (_corretor != null)
            {
                try
                {
                    db.SEG_CORRETORs.DeleteOnSubmit(_corretor);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_CORRETOR

        //SEG_TIPO_COBERTURA
        public List<SEG_TIPO_COBERTURA> ObterTipoCobertura()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_TIPO_COBERTURAs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public SEG_TIPO_COBERTURA ObterTipoCobertura(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_TIPO_COBERTURAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirTipoCobertura(SEG_TIPO_COBERTURA _tpCobertura)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_TIPO_COBERTURAs.InsertOnSubmit(_tpCobertura);
                db.SubmitChanges();

                return _tpCobertura.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarTipoCobertura(SEG_TIPO_COBERTURA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_TIPO_COBERTURA _tpCobertura = ObterTipoCobertura(_novo.CODIGO);

            if (_tpCobertura != null)
            {
                _tpCobertura.CODIGO = _novo.CODIGO;
                _tpCobertura.DESCRICAO = _novo.DESCRICAO;
                _tpCobertura.STATUS = _novo.STATUS;

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
        public void ExcluirTipoCobertura(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_TIPO_COBERTURA _tpCobertura = ObterTipoCobertura(codigo);

            if (_tpCobertura != null)
            {
                try
                {
                    db.SEG_TIPO_COBERTURAs.DeleteOnSubmit(_tpCobertura);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_TIPO_COBERTURA

        //SEG_SEGURO
        public List<SEG_SEGURO> ObterSeguro()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGUROs
                    orderby m.DATA_VENCIMENTO
                    select m).ToList();
        }
        public SEG_SEGURO ObterSeguro(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGUROs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirSeguro(SEG_SEGURO _seguro)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_SEGUROs.InsertOnSubmit(_seguro);
                db.SubmitChanges();

                return _seguro.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSeguro(SEG_SEGURO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURO _seguro = ObterSeguro(_novo.CODIGO);

            if (_seguro != null)
            {
                _seguro.CODIGO = _novo.CODIGO;
                _seguro.EMPRESA = _novo.EMPRESA;
                _seguro.SEG_TIPO_SEGURO = _novo.SEG_TIPO_SEGURO;
                _seguro.APOLICE = _novo.APOLICE;
                _seguro.PROPOSTA = _novo.PROPOSTA;
                _seguro.SEG_SEGURADORA = _novo.SEG_SEGURADORA;
                _seguro.SEG_CORRETOR = _novo.SEG_CORRETOR;
                _seguro.DATA_VENCIMENTO = _novo.DATA_VENCIMENTO;
                _seguro.VALOR_PREMIO = _novo.VALOR_PREMIO;
                _seguro.OBSERVACAO = _novo.OBSERVACAO;
                _seguro.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                _seguro.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                _seguro.STATUS = _novo.STATUS;

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
        public void ExcluirSeguro(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURO _seguro = ObterSeguro(codigo);

            if (_seguro != null)
            {
                try
                {
                    db.SEG_SEGUROs.DeleteOnSubmit(_seguro);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_SEGURO

        //SEG_SEGURO_ITEM
        public List<SEG_SEGURO_ITEM> ObterSeguroItem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGURO_ITEMs
                    orderby m.ITEM
                    select m).ToList();
        }
        public List<SEG_SEGURO_ITEM> ObterSeguroItem(string seg_seguro)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGURO_ITEMs
                    where m.SEG_SEGURO == Convert.ToInt32(seg_seguro)
                    orderby m.ITEM
                    select m).ToList();
        }
        public SEG_SEGURO_ITEM ObterSeguroItem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGURO_ITEMs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirSeguroItem(SEG_SEGURO_ITEM _seguroItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_SEGURO_ITEMs.InsertOnSubmit(_seguroItem);
                db.SubmitChanges();

                return _seguroItem.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSeguroItem(SEG_SEGURO_ITEM _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURO_ITEM _seguroItem = ObterSeguroItem(_novo.CODIGO);

            if (_seguroItem != null)
            {
                _seguroItem.CODIGO = _novo.CODIGO;
                _seguroItem.ITEM = _novo.ITEM;
                _seguroItem.SEG_SEGURO = _novo.SEG_SEGURO;

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
        public void ExcluirSeguroItem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURO_ITEM _seguroItem = ObterSeguroItem(codigo);

            if (_seguroItem != null)
            {
                try
                {
                    db.SEG_SEGURO_ITEMs.DeleteOnSubmit(_seguroItem);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_SEGURO_ITEM

        //SEG_ITEM_COB
        public List<SEG_ITEM_COB> ObterItemCoberturaSeguro(int seg_seguro)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.SEG_ITEM_COBs
                    join c in db.SEG_SEGURO_ITEM_COBs
                    on g.CODIGO equals c.SEG_ITEM_COB into itemLeft
                    from c in itemLeft.DefaultIfEmpty()
                    where c.CODIGO == null && g.SEG_SEGURO == seg_seguro
                    select g).ToList();
        }
        public List<SEG_ITEM_COB> ObterItemCobertura()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_ITEM_COBs
                    orderby m.SEG_TIPO_COBERTURA1.DESCRICAO
                    select m).ToList();
        }
        public SEG_ITEM_COB ObterItemCobertura(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_ITEM_COBs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirItemCobertura(SEG_ITEM_COB _itemCob)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_ITEM_COBs.InsertOnSubmit(_itemCob);
                db.SubmitChanges();

                return _itemCob.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarItemCobertura(SEG_ITEM_COB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_ITEM_COB _itemCob = ObterItemCobertura(_novo.CODIGO);

            if (_itemCob != null)
            {
                _itemCob.CODIGO = _novo.CODIGO;
                _itemCob.SEG_SEGURO = _novo.SEG_SEGURO;
                _itemCob.SEG_TIPO_COBERTURA = _novo.SEG_TIPO_COBERTURA;
                _itemCob.VALOR = _novo.VALOR;

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
        public void ExcluirItemCobertura(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_ITEM_COB _itemCob = ObterItemCobertura(codigo);

            if (_itemCob != null)
            {
                try
                {
                    db.SEG_ITEM_COBs.DeleteOnSubmit(_itemCob);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_ITEM_COB

        //SEG_SEGURO_ITEM_COB
        public List<SEG_SEGURO_ITEM_COB> ObterSeguroItemCobertura(string seg_seguro_item)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGURO_ITEM_COBs
                    where m.SEG_SEGURO_ITEM == Convert.ToInt32(seg_seguro_item)
                    select m).ToList();
        }
        public SEG_SEGURO_ITEM_COB ObterSeguroItemCobertura(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SEG_SEGURO_ITEM_COBs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirSeguroItemCobertura(SEG_SEGURO_ITEM_COB _seguroItemCob)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SEG_SEGURO_ITEM_COBs.InsertOnSubmit(_seguroItemCob);
                db.SubmitChanges();

                return _seguroItemCob.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSeguroItemCobertura(SEG_SEGURO_ITEM_COB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURO_ITEM_COB _seguroItemCob = ObterSeguroItemCobertura(_novo.CODIGO);

            if (_seguroItemCob != null)
            {
                _seguroItemCob.CODIGO = _novo.CODIGO;
                _seguroItemCob.SEG_SEGURO_ITEM = _novo.SEG_SEGURO_ITEM;
                _seguroItemCob.SEG_ITEM_COB = _novo.SEG_ITEM_COB;

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
        public void ExcluirSeguroItemCobertura(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            SEG_SEGURO_ITEM_COB _seguroItemCob = ObterSeguroItemCobertura(codigo);

            if (_seguroItemCob != null)
            {
                try
                {
                    db.SEG_SEGURO_ITEM_COBs.DeleteOnSubmit(_seguroItemCob);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - SEG_SEGURO_ITEM_COB

        //SP_OBTER_SEGUROSResult
        public List<SP_OBTER_SEGUROSResult> ObterRelSeguros(int? codigoSeguro, int? seguroItem, int? empresa, int? tipoSeguro, string apolice, string proposta, int? seguradora, int? corretor, DateTime? vencimentoIni, DateTime? vencimentoFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_SEGUROS(codigoSeguro, seguroItem, empresa, tipoSeguro, apolice, proposta, seguradora, corretor, vencimentoIni, vencimentoFim) select m).ToList();
        }
        //FIM - SP_OBTER_SEGUROSResult

        public void ExcluirItemCoberturaSP(int seg_seguro_item)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SP_EXCLUIR_ITEM_COBERTURA(seg_seguro_item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
