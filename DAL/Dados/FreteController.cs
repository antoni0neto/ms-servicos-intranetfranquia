using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class FreteController
    {
        DCDataContext db;

        public List<FRETE_GRUPO_PRECOMIN> ObterGrupoPrecoMin()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from g in db.FRETE_GRUPO_PRECOMINs select g).ToList();

        }

        public int InserirGrupoPrecoMin(FRETE_GRUPO_PRECOMIN _grupoPrecoMin)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_GRUPO_PRECOMINs.InsertOnSubmit(_grupoPrecoMin);
                db.SubmitChanges();

                return _grupoPrecoMin.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarGrupoPrecoMin(FRETE_GRUPO_PRECOMIN _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_GRUPO_PRECOMIN _grupoPrecoMin = ObterGrupoPrecoMin(_novo.CODIGO);

            if (_grupoPrecoMin != null)
            {
                _grupoPrecoMin.CODIGO = _novo.CODIGO;
                _grupoPrecoMin.GRUPO_PRODUTO = _novo.GRUPO_PRODUTO;
                _grupoPrecoMin.VALOR_UNITARIO = _novo.VALOR_UNITARIO;

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

        public void ExcluirGrupoPrecoMin(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_GRUPO_PRECOMIN _grupoPrecoMin = ObterGrupoPrecoMin(CODIGO);

            if (_grupoPrecoMin != null)
            {
                try
                {
                    db.FRETE_GRUPO_PRECOMINs.DeleteOnSubmit(_grupoPrecoMin);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public FRETE_GRUPO_PRECOMIN ObterGrupoPrecoMin(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FRETE_GRUPO_PRECOMINs where m.CODIGO == codigo select m).SingleOrDefault();
        }

        public FRETE_GRUPO_PRECOMIN ObterGrupoPrecoMin(string CODIGO_GRUPO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FRETE_GRUPO_PRECOMINs where m.GRUPO_PRODUTO.Trim() == CODIGO_GRUPO.Trim() select m).FirstOrDefault();
        }

        public List<FRETE_TIPO_ENVIO> ObterTipoEnvio()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FRETE_TIPO_ENVIOs select m).OrderBy(e => e.DESCRICAO).ToList();
        }

        public List<FRETE_TIPO_ENVIO> ObterTipoEnvioAtivo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FRETE_TIPO_ENVIOs where m.STATUS == '1' select m).OrderBy(e => e.DESCRICAO).ToList();
        }

        public FRETE_TIPO_ENVIO ObterTipoEnvio(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FRETE_TIPO_ENVIOs where m.CODIGO == CODIGO select m).SingleOrDefault();
        }

        public FRETE_TIPO_ENVIO ObterTipoEnvio(string DESCRICAO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FRETE_TIPO_ENVIOs where m.DESCRICAO.Trim() == DESCRICAO.Trim() select m).FirstOrDefault();
        }

        public List<FRETE_VALOR_PESO> ObterValorPeso()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from vp in db.FRETE_VALOR_PESOs select vp).ToList();
        }

        public FRETE_VALOR_PESO ObterValorPeso(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return db.FRETE_VALOR_PESOs.Where(f => f.CODIGO == CODIGO).SingleOrDefault();
        }

        public FRETE_VALOR_PESO ObterValorPeso(int FRETE_TIPO_ENVIO, decimal PESO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return db.FRETE_VALOR_PESOs.Where(f => f.FRETE_TIPO_ENVIO == FRETE_TIPO_ENVIO && f.PESO == PESO).SingleOrDefault();
        }

        public int InserirValorPeso(FRETE_VALOR_PESO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_VALOR_PESOs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarValorPeso(FRETE_VALOR_PESO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_VALOR_PESO _valorPeso = ObterValorPeso(_novo.CODIGO);

            if (_valorPeso != null)
            {
                _valorPeso.FRETE_TIPO_ENVIO = _novo.FRETE_TIPO_ENVIO;
                _valorPeso.PESO = _novo.PESO;
                _valorPeso.VALOR = _novo.VALOR;
                _valorPeso.DESCONTO_PORC = _novo.DESCONTO_PORC;

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

        public void ExcluirValorPeso(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_VALOR_PESO _valorPeso = ObterValorPeso(CODIGO);

            if (_valorPeso != null)
            {
                try
                {
                    db.FRETE_VALOR_PESOs.DeleteOnSubmit(_valorPeso);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int InserirTipoEnvio(FRETE_TIPO_ENVIO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_TIPO_ENVIOs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarTipoEnvio(FRETE_TIPO_ENVIO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TIPO_ENVIO _tipoEnvio = ObterTipoEnvio(_novo.CODIGO);

            if (_tipoEnvio != null)
            {
                _tipoEnvio.DESCRICAO = _novo.DESCRICAO;
                _tipoEnvio.STATUS = _novo.STATUS;

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

        public void ExcluirTipoEnvio(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TIPO_ENVIO _tipoEnvio = ObterTipoEnvio(CODIGO);

            if (_tipoEnvio != null)
            {
                try
                {
                    db.FRETE_TIPO_ENVIOs.DeleteOnSubmit(_tipoEnvio);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<FRETE_TIPO_PAGAMENTO> ObterTipoPagamento()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.FRETE_TIPO_PAGAMENTOs select p).ToList();
        }

        public List<FRETE_TIPO_PAGAMENTO> ObterTipoPagamentoAtivo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.FRETE_TIPO_PAGAMENTOs where p.STATUS == '1' select p).ToList();
        }

        public FRETE_TIPO_PAGAMENTO ObterTipoPagamento(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.FRETE_TIPO_PAGAMENTOs where p.CODIGO == CODIGO select p).SingleOrDefault();
        }

        public FRETE_TIPO_PAGAMENTO ObterTipoPagamento(string DESCRICAO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.FRETE_TIPO_PAGAMENTOs where p.DESCRICAO.Trim() == DESCRICAO.Trim() select p).FirstOrDefault();
        }

        public int InserirTipoPagamento(FRETE_TIPO_PAGAMENTO _tipoPagamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_TIPO_PAGAMENTOs.InsertOnSubmit(_tipoPagamento);
                db.SubmitChanges();

                if (_tipoPagamento.CODIGO == 1) //O PRIMEIRO REGISTRO SEMPRE SERÁ 'PRÉ PAGO'
                {
                    _tipoPagamento.DESCRICAO = "PRÉ PAGO";
                    _tipoPagamento.STATUS = '1';
                }
                else if (_tipoPagamento.DESCRICAO != null && _tipoPagamento.DESCRICAO != "PRÉ PAGO")
                {
                    db.SubmitChanges();

                }

                return _tipoPagamento.CODIGO;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarTipoPagamento(FRETE_TIPO_PAGAMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TIPO_PAGAMENTO _tipoPagamento = ObterTipoPagamento(_novo.CODIGO);

            if (_tipoPagamento != null)
            {
                try
                {
                    if (_tipoPagamento.CODIGO == 1) //NÃO É PERMITIDA ATUALIZAÇÃO DO TIPO DE PAGAMENTO 'PRÉ PAGO'
                    {
                        _tipoPagamento.DESCRICAO = "PRÉ PAGO";
                        _tipoPagamento.STATUS = '1';
                        db.SubmitChanges();
                    }
                    else if (_novo.DESCRICAO != null && _novo.DESCRICAO.ToUpper() != "PRÉ PAGO")
                    {
                        _tipoPagamento.DESCRICAO = _novo.DESCRICAO;
                        _tipoPagamento.STATUS = _novo.STATUS;
                        db.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ExcluirTipoPagamento(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TIPO_PAGAMENTO _tipoPagamento = ObterTipoPagamento(CODIGO);

            if (_tipoPagamento != null)
            {
                try
                {
                    if (_tipoPagamento.CODIGO > 1) //NÃO É PERMITIDA EXCLUSÃO DO TIPO DE PAGAMENTO 'PRÉ PAGO'
                    {
                        db.FRETE_TIPO_PAGAMENTOs.DeleteOnSubmit(_tipoPagamento);
                        db.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<FRETE_TIPO_AMOSTRA> ObterTipoAmostra()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from a in db.FRETE_TIPO_AMOSTRAs select a).ToList();
        }

        public List<FRETE_TIPO_AMOSTRA> ObterTipoAmostraAtivo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from a in db.FRETE_TIPO_AMOSTRAs where a.STATUS == '1' select a).ToList();
        }

        public FRETE_TIPO_AMOSTRA ObterTipoAmostra(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from a in db.FRETE_TIPO_AMOSTRAs where a.CODIGO == CODIGO select a).SingleOrDefault();
        }

        public FRETE_TIPO_AMOSTRA ObterTipoAmostra(string DESCRICAO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from a in db.FRETE_TIPO_AMOSTRAs where a.DESCRICAO.Trim() == DESCRICAO.Trim() select a).FirstOrDefault();
        }

        public int InserirTipoAmostra(FRETE_TIPO_AMOSTRA _tipoAmostra)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_TIPO_AMOSTRAs.InsertOnSubmit(_tipoAmostra);
                db.SubmitChanges();

                return _tipoAmostra.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarTipoAmostra(FRETE_TIPO_AMOSTRA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TIPO_AMOSTRA _tipoAmostra = ObterTipoAmostra(_novo.CODIGO);

            if (_tipoAmostra != null)
            {
                _tipoAmostra.DESCRICAO = _novo.DESCRICAO;
                _tipoAmostra.STATUS = _novo.STATUS;

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

        public void ExcluirTipoAmostra(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TIPO_AMOSTRA _tipoAmostra = ObterTipoAmostra(CODIGO);

            if (_tipoAmostra != null)
            {
                try
                {
                    db.FRETE_TIPO_AMOSTRAs.DeleteOnSubmit(_tipoAmostra);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<FRETE_TAXA_COMBUSTIVEL> ObterTaxaCombustivel()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.FRETE_TAXA_COMBUSTIVELs orderby c.MES, c.ANO select c).ToList();
        }

        public FRETE_TAXA_COMBUSTIVEL ObterTaxaCombustivel(DateTime DATA_RECEBIMENTO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.FRETE_TAXA_COMBUSTIVELs
                    where ((c.MES == DATA_RECEBIMENTO.Month && c.ANO == DATA_RECEBIMENTO.Year) || DATA_RECEBIMENTO <= DateTime.Parse("01/01/1900"))
                    orderby c.MES, c.ANO
                    select c).FirstOrDefault();
        }

        public List<int> ObterAnosCadastradosTaxaCombustivel()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return db.FRETE_TAXA_COMBUSTIVELs.OrderBy(c => c.ANO).Select(c => c.ANO).Distinct().ToList();
        }

        public FRETE_TAXA_COMBUSTIVEL ObterTaxaCombustivel(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.FRETE_TAXA_COMBUSTIVELs where c.CODIGO == CODIGO select c).SingleOrDefault();
        }

        public FRETE_TAXA_COMBUSTIVEL ObterTaxaCombustivel(int mes, int ano)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.FRETE_TAXA_COMBUSTIVELs where c.MES == mes where c.ANO == ano select c).FirstOrDefault();
        }

        public int InserirTaxaCombustivel(FRETE_TAXA_COMBUSTIVEL _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_TAXA_COMBUSTIVELs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarTaxaCombustivel(FRETE_TAXA_COMBUSTIVEL _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TAXA_COMBUSTIVEL _taxaCombustivel = ObterTaxaCombustivel(_novo.CODIGO);

            if (_taxaCombustivel != null)
            {
                _taxaCombustivel.VALOR_TAXA = _novo.VALOR_TAXA;
                _taxaCombustivel.MES = _novo.MES;
                _taxaCombustivel.ANO = _novo.ANO;

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

        public void ExcluirTaxaCombustivel(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_TAXA_COMBUSTIVEL _taxaCombustivel = ObterTaxaCombustivel(CODIGO);

            if (_taxaCombustivel != null)
            {
                try
                {
                    db.FRETE_TAXA_COMBUSTIVELs.DeleteOnSubmit(_taxaCombustivel);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<FRETE_RECEBIMENTO_NOTA> ObterRecebimentoNota()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.FRETE_RECEBIMENTO_NOTAs select n).ToList();
        }

        public FRETE_RECEBIMENTO_NOTA ObterRecebimentoNota(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.FRETE_RECEBIMENTO_NOTAs
                    where n.CODIGO == CODIGO
                    select n).SingleOrDefault();
        }

        public FRETE_RECEBIMENTO_NOTA ObterRecebimentoNota(string AWB)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.FRETE_RECEBIMENTO_NOTAs
                    where n.AWB == AWB
                    select n).FirstOrDefault();
        }

        public List<FRETE_RECEBIMENTO_NOTA> ObterRecebimentoNota(string AWB, DateTime DATA_RECEBIMENTO, int FRETE_TIPO_ENVIO, int FRETE_TIPO_PAGAMENTO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.FRETE_RECEBIMENTO_NOTAs
                    where (n.AWB.Contains(AWB) || AWB == "")
                    where (n.DATA_RECEBIMENTO.Date == DATA_RECEBIMENTO.Date || DATA_RECEBIMENTO <= DateTime.Parse("01/01/1900"))
                    where (n.FRETE_TIPO_ENVIO == FRETE_TIPO_ENVIO || FRETE_TIPO_ENVIO == 0)
                    where (n.FRETE_TIPO_PAGAMENTO == FRETE_TIPO_PAGAMENTO || FRETE_TIPO_PAGAMENTO == 0)
                    select n).ToList();
        }

        public List<FRETE_RECEBIMENTO_ITEM> ObterRecebimentoItemsPorNota(int FRETE_RECEBIMENTO_NOTA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.FRETE_RECEBIMENTO_ITEMs
                    where i.FRETE_RECEBIMENTO_NOTA == FRETE_RECEBIMENTO_NOTA
                    select i).ToList();
        }

        public FRETE_RECEBIMENTO_ITEM ObterRecebimentoItem(int CODIGO)
        {
            return ObterRecebimentoItem(CODIGO, 0, "").FirstOrDefault();
        }

        public FRETE_RECEBIMENTO_ITEM ObterRecebimentoItem(int FRETE_RECEBIMENTO_NOTA, string PRODUTO)
        {
            return ObterRecebimentoItem(0, FRETE_RECEBIMENTO_NOTA, PRODUTO).FirstOrDefault();
        }

        public List<FRETE_RECEBIMENTO_ITEM> ObterRecebimentoItem(int CODIGO, int FRETE_RECEBIMENTO_NOTA, string PRODUTO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.FRETE_RECEBIMENTO_ITEMs
                    where (i.CODIGO == CODIGO || CODIGO == 0)
                    where (i.FRETE_RECEBIMENTO_NOTA == FRETE_RECEBIMENTO_NOTA || FRETE_RECEBIMENTO_NOTA == 0)
                    where (i.PRODUTO.Trim() == PRODUTO.Trim() || PRODUTO == "")
                    select i).ToList();
        }

        public int InserirRecebimentoNota(FRETE_RECEBIMENTO_NOTA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_RECEBIMENTO_NOTAs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarRecebimentoNota(FRETE_RECEBIMENTO_NOTA _novo)
        {
            try
            {
                db = new DCDataContext(Constante.ConnectionStringIntranet);
                FRETE_RECEBIMENTO_NOTA _recebimentoNota = ObterRecebimentoNota(_novo.CODIGO);

                if (_recebimentoNota != null)
                {
                    _recebimentoNota.AWB = _novo.AWB;
                    _recebimentoNota.DATA_RECEBIMENTO = _novo.DATA_RECEBIMENTO;
                    _recebimentoNota.FRETE_TIPO_ENVIO = _novo.FRETE_TIPO_ENVIO;
                    _recebimentoNota.FRETE_TIPO_PAGAMENTO = _novo.FRETE_TIPO_PAGAMENTO;
                    _recebimentoNota.OBS = _novo.OBS;

                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirRecebimentoNota(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_RECEBIMENTO_NOTA _recebimentoNota = ObterRecebimentoNota(CODIGO);

            if (_recebimentoNota != null)
            {
                try
                {
                    db.FRETE_RECEBIMENTO_NOTAs.DeleteOnSubmit(_recebimentoNota);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int InserirRecebimentoNota(FRETE_RECEBIMENTO_ITEM _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_RECEBIMENTO_ITEMs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarRecebimenItem(FRETE_RECEBIMENTO_ITEM _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_RECEBIMENTO_ITEM _recebimentoItem = ObterRecebimentoItem(_novo.CODIGO);

            if (_recebimentoItem != null)
            {
                _recebimentoItem.FRETE_RECEBIMENTO_NOTA = _novo.FRETE_RECEBIMENTO_NOTA;
                _recebimentoItem.FRETE_TIPO_AMOSTRA = _novo.FRETE_TIPO_AMOSTRA;
                _recebimentoItem.GRUPO_PRODUTO = _novo.GRUPO_PRODUTO;
                _recebimentoItem.GRIFFE = _novo.GRIFFE;
                _recebimentoItem.QTDE_PECAS = _novo.QTDE_PECAS;
                _recebimentoItem.VALOR_UNITARIO = _novo.VALOR_UNITARIO;
                _recebimentoItem.STATUS = _novo.STATUS;
                _recebimentoItem.PRODUTO = _novo.PRODUTO;
                _recebimentoItem.OBS = _novo.OBS;

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

        public void ExcluirRecebimentoItem(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_RECEBIMENTO_ITEM _recebimentoItem = ObterRecebimentoItem(CODIGO);

            if (_recebimentoItem != null)
            {
                try
                {
                    db.FRETE_RECEBIMENTO_ITEMs.DeleteOnSubmit(_recebimentoItem);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<FRETE_IMPOSTO> ObterFreteImposto(int CODIGO, string AWB, DateTime DATA_RECEBIMENTO, string STATUS)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from imp in db.FRETE_IMPOSTOs
                    where (imp.CODIGO == CODIGO || CODIGO == 0)
                    where (imp.FRETE_RECEBIMENTO_NOTA1.AWB == AWB || string.IsNullOrEmpty(AWB))
                    where (imp.FRETE_RECEBIMENTO_NOTA1.DATA_RECEBIMENTO == DATA_RECEBIMENTO.Date || DATA_RECEBIMENTO <= DateTime.Parse("01/01/1900"))
                    where (imp.STATUS.ToString() == STATUS || string.IsNullOrEmpty(STATUS))
                    where imp.STATUS == '1'
                    select imp).ToList();
        }

        public FRETE_IMPOSTO ObterFreteImposto(int CODIGO)
        {
            return ObterFreteImposto(CODIGO, "", DateTime.MinValue, "").SingleOrDefault();
        }

        public int InserirFreteImposto(FRETE_IMPOSTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.FRETE_IMPOSTOs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarFreteImposto(FRETE_IMPOSTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_IMPOSTO _imposto = ObterFreteImposto(_novo.CODIGO);

            if (_imposto != null)
            {
                _imposto.FRETE_RECEBIMENTO_NOTA = _novo.FRETE_RECEBIMENTO_NOTA;
                _imposto.TAXA_DOLAR = _novo.TAXA_DOLAR;
                _imposto.PESO = _novo.PESO;
                _imposto.VALOR_FATURA_DOLAR = _novo.VALOR_FATURA_DOLAR;
                _imposto.VALOR_FRETE = _novo.VALOR_FRETE;
                _imposto.VALOR_II = _novo.VALOR_II;
                _imposto.VALOR_ICMS = _novo.VALOR_ICMS;
                _imposto.VALOR_DESEMBARACO = _novo.VALOR_DESEMBARACO;
                _imposto.VALOR_REEMB_INFRA = _novo.VALOR_REEMB_INFRA;
                _imposto.FRETE_TAXA_COMBUSTIVEL = _novo.FRETE_TAXA_COMBUSTIVEL;
                _imposto.BOLETO = _novo.BOLETO;
                _imposto.STATUS = _novo.STATUS;

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

        public void ExcluirFreteImposto(int CODIGO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_IMPOSTO _imposto = ObterFreteImposto(CODIGO);

            if (_imposto != null)
            {
                try
                {
                    _imposto.STATUS = '0';
                    AtualizarFreteImposto(_imposto);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        #region "Calculo do Imposto"

        public decimal CalculaValorII(decimal valorFrete, decimal valorTotalFaturaEmDolar, decimal taxaDolar, decimal multiplicador)
        {
            return Math.Round((valorFrete + (valorTotalFaturaEmDolar * taxaDolar)) * multiplicador, 4);
        }

        public decimal CalculaValorICMS(decimal valorII, decimal valorFrete, decimal valorTotalFaturaEmDolar, decimal taxaDolar, decimal divisor, decimal multiplicadorPercentualICMS)
        {
            return Math.Round((((valorTotalFaturaEmDolar * taxaDolar) + valorII + valorFrete) / divisor) * multiplicadorPercentualICMS, 4);
        }

        public decimal CalculaValorReembolsoInfra(decimal peso, decimal taxaDolar, decimal multiplicador)
        {
            return Math.Round((peso * multiplicador) * taxaDolar, 4);
        }

        public decimal CalculaValorDesembaraco(decimal taxaDolar, decimal multiplicador)
        {
            return Math.Round((taxaDolar * multiplicador), 4);
        }

        public decimal CalculaTotalImposto(decimal valorII, decimal valorICMS, decimal valorDesembaraco, decimal valorReembInfra)
        {
            return Math.Round((valorII + valorICMS + valorDesembaraco + valorReembInfra), 4);
        }

        public decimal CalculaValorFrete(string AWB, decimal peso, decimal taxaDolar)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_RECEBIMENTO_NOTA nota = ObterRecebimentoNota(AWB);

            if (nota != null)
            {
                //TIPO DE PAGAMENTO 'PRÉ PAGO' NÃO CALCULA FRETE
                if (nota.FRETE_TIPO_PAGAMENTO > 1)
                {
                    return CalculaValorFrete(nota.FRETE_TIPO_ENVIO, peso, taxaDolar);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                throw new NullReferenceException("ão foi possível calcular o frete. Verifique cadastro de Nota.");
            }
        }

        public decimal CalculaValorFrete(int FRETE_TIPO_ENVIO, decimal peso, decimal taxaDolar)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            FRETE_VALOR_PESO freteValorPeso = (from vp in db.FRETE_VALOR_PESOs
                                               where vp.FRETE_TIPO_ENVIO == FRETE_TIPO_ENVIO
                                               where vp.PESO <= peso
                                               orderby vp.PESO descending
                                               select vp).FirstOrDefault();

            if (freteValorPeso != null)
            {
                return Math.Round((freteValorPeso.VALOR - (freteValorPeso.VALOR * (freteValorPeso.DESCONTO_PORC / 100))) * taxaDolar, 4);
            }
            else
            {
                throw new NullReferenceException("Não foi possível calcular o frete. Verifique cadastro de Custo de Peso.");
            }
        }

        public decimal CalculaTotalNotaDolar(FRETE_RECEBIMENTO_NOTA nota)
        {
            return Math.Round(ObterRecebimentoItemsPorNota(nota.CODIGO).Sum(it => it.VALOR_UNITARIO * it.QTDE_PECAS), 4);
        }

        public decimal CalculaTotalNota(FRETE_RECEBIMENTO_NOTA nota, decimal taxaDolar)
        {
            return Math.Round((ObterRecebimentoItemsPorNota(nota.CODIGO).Sum(it => it.VALOR_UNITARIO * it.QTDE_PECAS) * taxaDolar), 4);
        }

        #endregion

    }
}
