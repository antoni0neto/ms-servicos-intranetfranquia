using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class ContabilidadeController
    {
        DCDataContext db;
        INTRADataContext dbIntra;
        LINXDataContext dbLinx;

        //SP_OBTER_RECEITA_X_DESPESA
        public List<SP_OBTER_RECEITA_X_DESPESAResult> ObterReceitaXDespesa(int ano, string codMatrizContabil, string tipoConta, string contaContabil)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_RECEITA_X_DESPESA(ano, codMatrizContabil, tipoConta, contaContabil) orderby m.CLASSIFICACAO select m).ToList();
        }
        public List<SP_OBTER_LANCAMENTO_RECEITADESPESAResult> ObterReceitaXDespesaLancamentos(string contaContabil, string matrizContabil, DateTime ini, DateTime fim, string codigoFilial)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_LANCAMENTO_RECEITADESPESA(contaContabil, matrizContabil, ini, fim, codigoFilial) select m).ToList();
        }


        public List<W_CTB_MATRIZ_CONTABIL> ObterMatrizContabil()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.W_CTB_MATRIZ_CONTABILs
                    where m.INATIVO == false
                    && !m.COD_CLIFOR.Trim().Contains("88")
                    orderby m.NOME_CLIFOR
                    select m).ToList();
        }
        public List<CTB_CONTA_TIPO> ObterContaContabilTipo()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.CTB_CONTA_TIPOs
                .ToList()
                .Select(c => new CTB_CONTA_TIPO()
                {
                    TIPO_CONTA = c.TIPO_CONTA.Trim(),
                    DESC_CONTA_TIPO = string.Format("{0} - {1}", c.TIPO_CONTA.Trim(), c.DESC_CONTA_TIPO.Trim())
                })
                .Distinct()
                .OrderBy(c => c.TIPO_CONTA)
                .ToList();
        }

        public List<EVENTO> ObterEventos()
        {
            db = new DCDataContext(Constante.ConnectionStringJGM);
            return (from e in db.EVENTOs
                    select new
                    {
                        COD_EVENTO = e.COD_EVENTO,
                        DESCRICAO = e.DESCRICAO.Trim()
                    })
                .ToList()
                .Distinct()
                .Select(e => new EVENTO()
                {
                    COD_EVENTO = e.COD_EVENTO,
                    DESCRICAO = string.Format("{0} - {1}",
                                    e.COD_EVENTO,
                                    e.DESCRICAO)
                })
                .Distinct()
                .OrderBy(e => e.DESCRICAO)
                .ToList();
        }
        public List<CTB_CONTA_PLANO> ObterContas()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.CTB_CONTA_PLANOs
                .Where(c => c.INATIVA == false)
                .ToList()
                .Select(c => new CTB_CONTA_PLANO()
                {
                    CONTA_CONTABIL = c.CONTA_CONTABIL.Trim(),
                    DESC_CONTA = string.Format("{0} - {1}", c.CONTA_CONTABIL.Trim(), c.DESC_CONTA.Trim())
                })
                .Distinct()
                .OrderBy(c => c.CONTA_CONTABIL)
                .ToList();
        }
        public List<CTB_CONTA_PLANO> ObterContasFiltro(string filtro_1, string filtro_2, string filto_3, string filtro_4, string filtro_5, string filtro_6)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.CTB_CONTA_PLANOs
                .Where(
                    c => c.INATIVA == false &&
                        (SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_1) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_2) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filto_3) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_4) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_5) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_6))
                    )
                .ToList()
                .Select(c => new CTB_CONTA_PLANO()
                {
                    CONTA_CONTABIL = c.CONTA_CONTABIL.Trim(),
                    DESC_CONTA = string.Format("{0} - {1}", c.CONTA_CONTABIL.Trim(), c.DESC_CONTA.Trim())
                })
                .Distinct()
                .OrderBy(c => c.CONTA_CONTABIL)
                .ToList();
        }
        public List<CTB_CONTA_PLANO> ObterContasFiltroOrdemNome(string filtro_1, string filtro_2, string filto_3, string filtro_4, string filtro_5, string filtro_6)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.CTB_CONTA_PLANOs
                .Where(
                    c => c.INATIVA == false &&
                        (SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_1) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_2) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filto_3) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_4) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_5) ||
                        SqlMethods.Like(c.CONTA_CONTABIL.Trim(), filtro_6))
                    )
                .ToList()
                .Select(c => new CTB_CONTA_PLANO()
                {
                    CONTA_CONTABIL = c.CONTA_CONTABIL.Trim(),
                    DESC_DETALHADA = c.DESC_CONTA.Trim(),
                    DESC_CONTA = string.Format("{0} - {1}", c.CONTA_CONTABIL.Trim(), c.DESC_CONTA.Trim())
                })
                .Distinct()
                .OrderBy(c => c.DESC_DETALHADA)
                .ToList();
        }
        public CTB_CONTA_PLANO ObterConta(string CONTA_CONTABIL)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return ObterContas().Where(c => c.CONTA_CONTABIL.Trim() == CONTA_CONTABIL.Trim()).SingleOrDefault();
        }
        public CTB_CONTA_PLANO ObterContaDireto(string CONTA_CONTABIL)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CTB_CONTA_PLANOs
                    where m.CONTA_CONTABIL.Trim() == CONTA_CONTABIL.Trim()
                    select m).SingleOrDefault();
        }
        public int InserirParamContabilidade(GER_PARAMETROS_CONTABILIDADE _paramContabilidade)
        {
            db = new DCDataContext(Constante.ConnectionStringJGM);
            try
            {
                db.GER_PARAMETROS_CONTABILIDADEs.InsertOnSubmit(_paramContabilidade);
                db.SubmitChanges();

                return _paramContabilidade.COD_EVENTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SP_OBTER_PARAMETROS_CONTABILIDADEResult> ObterParametrosContabilidade()
        {
            db = new DCDataContext(Constante.ConnectionStringJGM);
            return db.SP_OBTER_PARAMETROS_CONTABILIDADE().ToList();
        }
        public GER_PARAMETROS_CONTABILIDADE ObterParametrosContabilidade(int COD_EVENTO, string CONTA_DEBITO, string CONTA_CREDITO)
        {
            db = new DCDataContext(Constante.ConnectionStringJGM);
            return db.GER_PARAMETROS_CONTABILIDADEs
                .SingleOrDefault(c => c.COD_EVENTO == COD_EVENTO && c.CONTA_DEBITO == CONTA_DEBITO && c.CONTA_CREDITO == CONTA_CREDITO);
        }
        public void ExcluirParamContabilidade(int COD_EVENTO, string CONTA_DEBITO, string CONTA_CREDITO)
        {
            db = new DCDataContext(Constante.ConnectionStringJGM);
            GER_PARAMETROS_CONTABILIDADE _param = ObterParametrosContabilidade(COD_EVENTO, CONTA_DEBITO, CONTA_CREDITO);

            if (_param != null)
            {
                try
                {
                    db.GER_PARAMETROS_CONTABILIDADEs.DeleteOnSubmit(_param);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //SP_OBTER_EVENTO_SEM_CONTAB
        public List<SP_OBTER_EVENTO_SEM_CONTABResult> ObterEventoSemContabilizacao()
        {
            db = new DCDataContext(Constante.ConnectionStringJGM);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_EVENTO_SEM_CONTAB() orderby m.DESCRICAO select m).ToList();
        }

        public List<CTB_LANCAMENTO_ITEM> ObterLancamento(int lancamento)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from s in db.CTB_LANCAMENTO_ITEMs
                    where s.LANCAMENTO == lancamento
                    orderby s.ITEM
                    select s).ToList();
        }

        //CTB_FORNECEDOR_CONTA
        public List<CTB_FORNECEDOR_CONTA> ObterFornecedorConta()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_FORNECEDOR_CONTAs orderby m.FORNECEDOR, m.CONTA_CONTABIL select m).ToList();
        }
        public CTB_FORNECEDOR_CONTA ObterFornecedorConta(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_FORNECEDOR_CONTAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<CTB_FORNECEDOR_CONTA> ObterFornecedorConta(string clifor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_FORNECEDOR_CONTAs
                    where m.CLIFOR.Trim() == clifor.Trim()
                    orderby m.FORNECEDOR, m.CONTA_CONTABIL
                    select m).ToList();
        }
        public void InserirFornecedorConta(CTB_FORNECEDOR_CONTA _fornecedorConta)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_FORNECEDOR_CONTAs.InsertOnSubmit(_fornecedorConta);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFornecedorConta(CTB_FORNECEDOR_CONTA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            CTB_FORNECEDOR_CONTA _fornecedorConta = ObterFornecedorConta(_novo.CODIGO);

            if (_novo != null)
            {
                _fornecedorConta.CODIGO = _novo.CODIGO;
                _fornecedorConta.CLIFOR = _novo.CLIFOR;
                _fornecedorConta.FORNECEDOR = _novo.FORNECEDOR;
                _fornecedorConta.CONTA_CONTABIL = _novo.CONTA_CONTABIL;
                _fornecedorConta.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                _fornecedorConta.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                _fornecedorConta.USUARIO_EXCLUSAO = _novo.USUARIO_EXCLUSAO;
                _fornecedorConta.DATA_EXCLUSAO = _novo.DATA_EXCLUSAO;

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
        public void ExcluirFornecedorConta(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_FORNECEDOR_CONTA _fornecedorConta = ObterFornecedorConta(codigo);
                if (_fornecedorConta != null)
                {
                    db.CTB_FORNECEDOR_CONTAs.DeleteOnSubmit(_fornecedorConta);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM CTB_FORNECEDOR_CONTA

        //SP_OBTER_FAT_DESONERACAOResult
        public List<SP_OBTER_FAT_DESONERACAOResult> ObterFaturamentoDesoneracao(string empresa, DateTime dataIni, DateTime dataFim, string codFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_FAT_DESONERACAO(empresa, dataIni, dataFim, codFilial) select m).ToList();
        }
        public List<SP_OBTER_FAT_IMPOSTO_ICMSResult> ObterImpostoFatICMS(string empresa, DateTime dataIni, DateTime dataFim)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_FAT_IMPOSTO_ICMS(empresa, dataIni, dataFim) select m).ToList();
        }
        public List<SP_OBTER_FAT_IMPOSTO_CFOPResult> ObterBaseImpostoCFOP(string empresa, DateTime dataIni, DateTime dataFim, bool entrada, string inSQLCFOP)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_FAT_IMPOSTO_CFOP(empresa, dataIni, dataFim, entrada, inSQLCFOP) select m).ToList();
        }
        public SP_OBTER_FAT_REG_MONOFASICOResult ObterRegimoMonofasicoValor(string empresa, DateTime dataIni, DateTime dataFim)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_FAT_REG_MONOFASICO(empresa, dataIni, dataFim) select m).SingleOrDefault();
        }
        public SP_OBTER_FAT_CTA_PCResult ObterValorCTAPF(string empresa, DateTime dataIni, DateTime dataFim, int item)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_FAT_CTA_PC(empresa, dataIni, dataFim, item) select m).SingleOrDefault();
        }

        //SP_OBTER_DEC_FATURAMENTOResult
        public List<SP_OBTER_DEC_FATURAMENTOResult> ObterDeclaracaoFAT(string empresa, DateTime data, string codFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_DEC_FATURAMENTO(empresa, data, codFilial) select m).ToList();
        }

        //CTB_FORNECEDOR_CONTA
        public List<CTB_APURACAO_TIPO> ObterApuracaoTipo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_TIPOs
                    orderby m.TIPO
                    select m).ToList();
        }
        public CTB_APURACAO_TIPO ObterApuracaoTipo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_TIPOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        //FIM CTB_FORNECEDOR_CONTA

        //CTB_IMPOSTO_TIPO
        public List<CTB_IMPOSTO_TIPO> ObterImpostoTipo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_IMPOSTO_TIPOs
                    orderby m.TIPO
                    select m).ToList();
        }
        public CTB_IMPOSTO_TIPO ObterImpostoTipo(string tipo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_IMPOSTO_TIPOs
                    where m.TIPO.Trim() == tipo.Trim()
                    select m).SingleOrDefault();
        }
        //FIM CTB_LX_IMPOSTO_TIPO

        //CTB_APURACAO_ITEM
        public List<CTB_APURACAO_ITEM> ObterApuracaoItem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_ITEMs orderby m.ITEM select m).ToList();
        }
        public List<CTB_APURACAO_ITEM> ObterApuracaoItem(int ctbApuracaoTipo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_ITEMs
                    where m.CTB_APURACAO_TIPO == ctbApuracaoTipo
                    orderby m.ORDEM
                    select m).ToList();
        }
        //FIM CTB_APURACAO_ITEM

        //CTB_APURACAO_EMPRESA
        public List<CTB_APURACAO_EMPRESA> ObterApuracaoEmpresa()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_EMPRESAs orderby m.COMPETENCIA select m).ToList();
        }
        public CTB_APURACAO_EMPRESA ObterApuracaoEmpresa(int empresa, DateTime competencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_EMPRESAs
                    where m.EMPRESA == empresa
                    && m.COMPETENCIA == competencia
                    select m).SingleOrDefault();
        }
        public CTB_APURACAO_EMPRESA ObterApuracaoEmpresa(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_EMPRESAs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirApuracaoEmpresa(CTB_APURACAO_EMPRESA _apEmpresa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_APURACAO_EMPRESAs.InsertOnSubmit(_apEmpresa);
                db.SubmitChanges();

                return _apEmpresa.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirApuracaoEmpresa(int empresa, DateTime competencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_APURACAO_EMPRESA _apEmpresa = ObterApuracaoEmpresa(empresa, competencia);
                if (_apEmpresa != null)
                {
                    db.CTB_APURACAO_EMPRESAs.DeleteOnSubmit(_apEmpresa);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM CTB_APURACAO_EMPRESA

        //CTB_APURACAO_EMPRESA_ITEM
        public List<CTB_APURACAO_EMPRESA_ITEM> ObterApuracaoEmpresaItem(int ctbApuracaoEmpresa, int ctbApuracaoTipo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_EMPRESA_ITEMs
                    where m.CTB_APURACAO_EMPRESA == ctbApuracaoEmpresa
                    && m.CTB_APURACAO_ITEM1.CTB_APURACAO_TIPO == ctbApuracaoTipo
                    orderby m.CTB_APURACAO_ITEM1.ORDEM
                    select m).ToList();
        }
        public CTB_APURACAO_EMPRESA_ITEM ObterApuracaoEmpresaItem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_EMPRESA_ITEMs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirApuracaoEmpresaItem(CTB_APURACAO_EMPRESA_ITEM _apEmpresaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_APURACAO_EMPRESA_ITEMs.InsertOnSubmit(_apEmpresaItem);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirApuracaoEmpresaItem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_APURACAO_EMPRESA_ITEM _apEmpresaItem = ObterApuracaoEmpresaItem(codigo);
                if (_apEmpresaItem != null)
                {
                    db.CTB_APURACAO_EMPRESA_ITEMs.DeleteOnSubmit(_apEmpresaItem);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM CTB_APURACAO_EMPRESA_ITEM

        //CTB_APURACAO_CALC
        public CTB_APURACAO_EMPRESA_CALC ObterApuracaoCalculo(int codigoApuracaoEmpresa, int codigoImposto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_EMPRESA_CALCs
                    where
                    m.CTB_APURACAO_EMPRESA == codigoApuracaoEmpresa &&
                    m.CTB_IMPOSTO_TIPO == codigoImposto
                    orderby m.DATA_INCLUSAO descending
                    select m).FirstOrDefault();
        }
        public CTB_APURACAO_EMPRESA_CALC ObterApuracaoCalculo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_APURACAO_EMPRESA_CALCs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirApuracaoCalculo(CTB_APURACAO_EMPRESA_CALC _apCalc)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_APURACAO_EMPRESA_CALCs.InsertOnSubmit(_apCalc);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarApuracaoCalculo(CTB_APURACAO_EMPRESA_CALC _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            CTB_APURACAO_EMPRESA_CALC _apCalc = ObterApuracaoCalculo(_novo.CODIGO);

            if (_novo != null)
            {
                _apCalc.CTB_APURACAO_EMPRESA = _novo.CTB_APURACAO_EMPRESA;
                _apCalc.CTB_IMPOSTO_TIPO = _novo.CTB_IMPOSTO_TIPO;
                _apCalc.ALIQUOTA = _novo.ALIQUOTA;
                _apCalc.VALOR_DARF = _novo.VALOR_DARF;
                _apCalc.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _apCalc.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        //FIM CTB_APURACAO_CALC

        public List<CTB_ESFERA> ObterEsfera()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_ESFERAs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public CTB_ESFERA ObterEsfera(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_ESFERAs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }

        //CTB_FILIAL_RESPONSAVEL
        public List<CTB_FILIAL_RESPONSAVEL> ObterRespFilialFiscal()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_FILIAL_RESPONSAVELs
                    select m).ToList();
        }
        public List<CTB_FILIAL_RESPONSAVEL> ObterRespFilialFiscal(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_FILIAL_RESPONSAVELs
                    where
                    m.CODIGO_FILIAL == codigoFilial
                    select m).ToList();
        }
        public CTB_FILIAL_RESPONSAVEL ObterRespFilialFiscal(string codigoFilial, int usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_FILIAL_RESPONSAVELs
                    where
                    m.CODIGO_FILIAL == codigoFilial &&
                    m.USUARIO_RESP == usuario
                    select m).FirstOrDefault();
        }
        public CTB_FILIAL_RESPONSAVEL ObterRespFilialFiscal(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_FILIAL_RESPONSAVELs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirRespFilialFiscal(CTB_FILIAL_RESPONSAVEL _filialUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_FILIAL_RESPONSAVELs.InsertOnSubmit(_filialUsuario);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarRespFilialFiscal(CTB_FILIAL_RESPONSAVEL _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            CTB_FILIAL_RESPONSAVEL _filialUsuario = ObterRespFilialFiscal(_novo.CODIGO);

            if (_novo != null)
            {
                _filialUsuario.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _filialUsuario.USUARIO_RESP = _novo.USUARIO_RESP;
                _filialUsuario.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                _filialUsuario.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;

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
        public void ExcluirRespFilialFiscal(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_FILIAL_RESPONSAVEL _filialUsuario = ObterRespFilialFiscal(codigo);
                if (_filialUsuario != null)
                {
                    db.CTB_FILIAL_RESPONSAVELs.DeleteOnSubmit(_filialUsuario);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM CTB_FILIAL_USUARIO

        //CTB_OBRIGACAO
        public List<CTB_OBRIGACAO> ObterObrigacaoFiscal()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_OBRIGACAOs
                    select m).ToList();
        }
        public CTB_OBRIGACAO ObterObrigacaoFiscal(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_OBRIGACAOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirObrigacaoFiscal(CTB_OBRIGACAO _obrigacao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_OBRIGACAOs.InsertOnSubmit(_obrigacao);
                db.SubmitChanges();

                return _obrigacao.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarObrigacaoFiscal(CTB_OBRIGACAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            CTB_OBRIGACAO _obrigacao = ObterObrigacaoFiscal(_novo.CODIGO);

            if (_novo != null)
            {
                _obrigacao.DESCRICAO = _novo.DESCRICAO;
                _obrigacao.CTB_ESFERA = _novo.CTB_ESFERA;
                _obrigacao.REGIME_TRIBUTACAO = _novo.REGIME_TRIBUTACAO;
                _obrigacao.PERIODO = _novo.PERIODO;
                _obrigacao.DIA_UTIL = _novo.DIA_UTIL;
                _obrigacao.DIA = _novo.DIA;
                _obrigacao.DATA_INI = _novo.DATA_INI;
                _obrigacao.DATA_FIM = _novo.DATA_FIM;
                _obrigacao.UF = _novo.UF;
                _obrigacao.UTILIZAR_VALOR = _novo.UTILIZAR_VALOR;
                _obrigacao.STATUS = _novo.STATUS;
                _obrigacao.FUNDAMENTACAO = _novo.FUNDAMENTACAO;
                _obrigacao.OBSERVACAO = _novo.OBSERVACAO;
                _obrigacao.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _obrigacao.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _obrigacao.INSCRICAO_ESTADUAL = _novo.INSCRICAO_ESTADUAL;

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
        public void ExcluirObrigacaoFiscal(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_OBRIGACAO _obrigacao = ObterObrigacaoFiscal(codigo);
                if (_obrigacao != null)
                {
                    db.CTB_OBRIGACAOs.DeleteOnSubmit(_obrigacao);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM CTB_OBRIGACAO

        //CTB_OBRIGACAO_FILIAL
        public List<CTB_OBRIGACAO_FILIAL> ObterObrigacaoFiscalFilial()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_OBRIGACAO_FILIALs
                    select m).ToList();
        }
        public CTB_OBRIGACAO_FILIAL ObterObrigacaoFiscalFilial(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.CTB_OBRIGACAO_FILIALs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirObrigacaoFiscalFilial(CTB_OBRIGACAO_FILIAL _obrigacaoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_OBRIGACAO_FILIALs.InsertOnSubmit(_obrigacaoFilial);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarObrigacaoFiscalFilial(CTB_OBRIGACAO_FILIAL _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            CTB_OBRIGACAO_FILIAL _obrigacaoFilial = ObterObrigacaoFiscalFilial(_novo.CODIGO);

            if (_novo != null)
            {
                _obrigacaoFilial.CTB_OBRIGACAO = _novo.CTB_OBRIGACAO;

                _obrigacaoFilial.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _obrigacaoFilial.EMPRESA = _novo.EMPRESA;
                _obrigacaoFilial.COMPETENCIA = _novo.COMPETENCIA;
                _obrigacaoFilial.USUARIO_RESP = _novo.USUARIO_RESP;
                _obrigacaoFilial.VALOR = _novo.VALOR;
                _obrigacaoFilial.DATA_CONCLUSAO = _novo.DATA_CONCLUSAO;
                _obrigacaoFilial.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                _obrigacaoFilial.DATA_ALTERACAO = _novo.DATA_ALTERACAO;

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
        public void ExcluirObrigacaoFiscalFilial(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_OBRIGACAO_FILIAL _obrigacaoFilial = ObterObrigacaoFiscalFilial(codigo);
                if (_obrigacaoFilial != null)
                {
                    db.CTB_OBRIGACAO_FILIALs.DeleteOnSubmit(_obrigacaoFilial);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM CTB_OBRIGACAO_FILIAL

        //SP_OBTER_DEC_FATURAMENTOResult
        public List<SP_OBTER_RESPONSAVEL_FISCALResult> ObterResponsavelFiscal()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_RESPONSAVEL_FISCAL() select m).ToList();
        }
        public List<SP_OBTER_RESUMO_IMPOSTOResult> ObterResumoImposto(int anoIni, int mesIni, int anoFim, int mesFim, string empresa, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.SP_OBTER_RESUMO_IMPOSTO(anoIni, mesIni, anoFim, mesFim, empresa, codigoFilial) select m).ToList();
        }

        public List<SP_OBTER_NF_ALIQ_ICMSResult> ObterNFAliqImposto(int tipo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.SP_OBTER_NF_ALIQ_ICMS(tipo) select m).ToList();
        }
        public void InserirNFAliqImposto(CTB_ALIQUOTA_ICMS_BAIXA aliqICMSBaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.CTB_ALIQUOTA_ICMS_BAIXAs.InsertOnSubmit(aliqICMSBaixa);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EmpresaFilial> BuscaEmpresaFiliail()
        {
            db = new DCDataContext(Constante.ConnectionStringJGM);

            return (from f in db.EmpresaFilials orderby f.NomeFantasia select f).ToList();
        }

        public List<SP_FUNCIONARIO_CFResult> BuscaFuncionarioCustoFixo(Int32 Mes, Int32 Ano, int? centroCustoGrupo, string centroCusto, string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_FUNCIONARIO_CF(Mes, Ano, centroCustoGrupo, centroCusto, filial) select m).ToList();
        }

        public FUNCIONARIO_CUSTOFIXO_AUX ObterFUNCIONARIO_CUSTOFIXO_AUX(FUNCIONARIO_CUSTOFIXO_AUX FUNCIONARIO_CF_AUX)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from fcf in db.FUNCIONARIO_CUSTOFIXO_AUXes where fcf.CPF == FUNCIONARIO_CF_AUX.CPF && fcf.ANO == FUNCIONARIO_CF_AUX.ANO && fcf.MES == FUNCIONARIO_CF_AUX.MES select fcf).SingleOrDefault();
        }

        public void IncluirFUNCIONARIO_CUSTOFIXO_AUX(FUNCIONARIO_CUSTOFIXO_AUX FUNCIONARIO_CF_AUX)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.FUNCIONARIO_CUSTOFIXO_AUXes.InsertOnSubmit(FUNCIONARIO_CF_AUX);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirFUNCIONARIO_CUSTOFIXO_AUX(FUNCIONARIO_CUSTOFIXO_AUX FUNCIONARIO_CF_AUX)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                FUNCIONARIO_CUSTOFIXO_AUX _FUNCIONARIO_CF_AUX = ObterFUNCIONARIO_CUSTOFIXO_AUX(FUNCIONARIO_CF_AUX);
                if (_FUNCIONARIO_CF_AUX != null)
                {
                    db.FUNCIONARIO_CUSTOFIXO_AUXes.DeleteOnSubmit(_FUNCIONARIO_CF_AUX);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluirFUNCIONARIO_CUSTOFIXO_HIST(FUNCIONARIO_CUSTOFIXO_HIST FUNCIONARIO_CF_HIST)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.FUNCIONARIO_CUSTOFIXO_HISTs.InsertOnSubmit(FUNCIONARIO_CF_HIST);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FUNCIONARIO_CENTROCUSTO_MESANO ObterFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CENTROCUSTO_MESANO FUNCIONARIO_CC)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from fcc in db.FUNCIONARIO_CENTROCUSTO_MESANOs where fcc.CPF == FUNCIONARIO_CC.CPF && fcc.ANO == FUNCIONARIO_CC.ANO && fcc.MES == FUNCIONARIO_CC.MES select fcc).SingleOrDefault();
        }

        public void IncluirFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CENTROCUSTO_MESANO FUNCIONARIO_CC)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.FUNCIONARIO_CENTROCUSTO_MESANOs.InsertOnSubmit(FUNCIONARIO_CC);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CENTROCUSTO_MESANO FUNCIONARIO_CC)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                FUNCIONARIO_CENTROCUSTO_MESANO _FUNCIONARIO_CC = ObterFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CC);
                if (_FUNCIONARIO_CC != null)
                {
                    db.FUNCIONARIO_CENTROCUSTO_MESANOs.DeleteOnSubmit(_FUNCIONARIO_CC);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_FILIAIS_DEPOSITO_BANCOResult> BuscaFiliaisDepositos(string dataDeposito, Char tipoData, string filial, string nomeBanco, int codigoBanco, int status, Char IgnorarData)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 600;
            return (from m in db.SP_OBTER_FILIAIS_DEPOSITO_BANCO(dataDeposito, tipoData, filial, nomeBanco, codigoBanco, status, IgnorarData) select m).ToList();
        }

        public List<SP_OBTER_FECHAMENTOS_DEPOSITO_BANCOResult> BuscaFechamentosDepositos(int codigoSaldo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 600;
            return (from m in db.SP_OBTER_FECHAMENTOS_DEPOSITO_BANCO(codigoSaldo) select m).ToList();
        }

        public List<SP_OBTER_CONTAS_CORRENTESResult> ListarContasCorrentes(string NomeBanco)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 600;
            return (from m in db.SP_OBTER_CONTAS_CORRENTES(NomeBanco) select m).ToList();
        }


        //CTB_BALANCO_CONTA
        public List<CTB_BALANCO_CONTA> ObterBalancoConta()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CTB_BALANCO_CONTAs orderby m.GRUPO, m.SUBGRUPO, m.CLASSIFICACAO, m.CONTA select m).ToList();
        }
        public CTB_BALANCO_CONTA ObterBalancoConta(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CTB_BALANCO_CONTAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public CTB_BALANCO_CONTA ObterBalancoConta(string conta)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CTB_BALANCO_CONTAs
                    where m.CONTA.Trim() == conta.Trim()
                    select m).SingleOrDefault();
        }
        public CTB_BALANCO_CONTA ObterBalancoConta(string grupo, string subGrupo, string classificacao, string conta)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CTB_BALANCO_CONTAs
                    where
                        m.GRUPO.Trim() == grupo.Trim() &&
                        m.SUBGRUPO.Trim() == subGrupo.Trim() &&
                        m.CLASSIFICACAO.Trim() == classificacao.Trim() &&
                        m.CONTA.Trim() == conta.Trim()
                    orderby m.GRUPO, m.SUBGRUPO, m.CLASSIFICACAO, m.CONTA
                    select m).SingleOrDefault();
        }
        public void InserirBalancoConta(CTB_BALANCO_CONTA _balancoConta)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.CTB_BALANCO_CONTAs.InsertOnSubmit(_balancoConta);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarBalancoConta(CTB_BALANCO_CONTA _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            CTB_BALANCO_CONTA _balancoConta = ObterBalancoConta(_novo.CODIGO);

            if (_novo != null)
            {
                _balancoConta.CODIGO = _novo.CODIGO;
                _balancoConta.GRUPO = _novo.GRUPO;
                _balancoConta.SUBGRUPO = _novo.SUBGRUPO;
                _balancoConta.CLASSIFICACAO = _novo.CLASSIFICACAO;
                _balancoConta.CONTA = _novo.CONTA;
                _balancoConta.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                _balancoConta.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;

                try
                {
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirBalancoConta(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_BALANCO_CONTA _balancoConta = ObterBalancoConta(codigo);
                if (_balancoConta != null)
                {
                    dbIntra.CTB_BALANCO_CONTAs.DeleteOnSubmit(_balancoConta);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM CTB_BALANCO_CONTA

        public List<SP_OBTER_VISAO_CONTAResult> ObterVisaoConta()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            return (from m in dbLinx.SP_OBTER_VISAO_CONTA() select m)
                .Select(c => new SP_OBTER_VISAO_CONTAResult()
                {
                    CLASSIFICACAO = c.CLASSIFICACAO.Trim(),
                    DESCR_CONTA = string.Format("{0} - {1}", c.CLASSIFICACAO.Trim(), c.DESCR_CONTA.Trim())
                })
                .Distinct()
                .OrderBy(c => c.CLASSIFICACAO)
                .ToList();
        }

        public SP_OBTER_VISAO_CONTAResult ObterVisaoConta(string classificacao)
        {
            return ObterVisaoConta().Where(p => p.CLASSIFICACAO.Trim() == classificacao).SingleOrDefault();
        }

        public List<SP_OBTER_VC_FORNECEDORResult> ObterVendaConsigFornecedor()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            return (from m in dbLinx.SP_OBTER_VC_FORNECEDOR() select m).ToList();
        }
        public List<SP_OBTER_VC_CTRL_REMESSAResult> ObterVendaConsigControleRemessa(string fornecedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_VC_CTRL_REMESSA(fornecedor) select m).ToList();
        }
        public List<SP_OBTER_VC_CTRL_PGTOResult> ObterVendaConsigControlePGTO(string fornecedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_VC_CTRL_PGTO(fornecedor) select m).ToList();
        }

        public List<SP_OBTER_EXTRATO_SACOLAResult> ObterExtratoSacola(string sacolaItem, string fornecedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_EXTRATO_SACOLA(sacolaItem, fornecedor) select m).ToList();
        }

        public List<SP_OBTER_MOSTR_REPREResult> ObterMostruarioRepresentante()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            return (from m in dbLinx.SP_OBTER_MOSTR_REPRE() select m).ToList();
        }
        public List<SP_OBTER_MOSTR_REPRE_REMESSAResult> ObterMostruarioRemessa(string representante)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_MOSTR_REPRE_REMESSA(representante) select m).ToList();
        }
        public List<SP_OBTER_MOSTR_REPRE_BONIFICACAOResult> ObterMostruarioBonificacao(string representante)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_MOSTR_REPRE_BONIFICACAO(representante) select m).ToList();
        }
        public List<SP_OBTER_MOSTR_REPRE_RECEBIMENTOResult> ObterMostruarioRecebimento(string representante)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_MOSTR_REPRE_RECEBIMENTO(representante) select m).ToList();
        }


        public List<SP_OBTER_VENDA_VALE_FUNCIONARIO_ERRADOResult> ObterTicketValeFuncionario(string codigoFilial, DateTime? dataIni, DateTime? dataFim, string codFormaPgto, string ticket)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_VENDA_VALE_FUNCIONARIO_ERRADO(codigoFilial, dataIni, dataFim, codFormaPgto, ticket) select m).ToList();
        }
        public void AtualizarFormaPgtoTicket(string codigoFilial, string ticket, DateTime dataVenda, string codFormaPgto)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            dbLinx.SP_ATU_FORMA_PGTO_VENDA(codigoFilial, ticket, dataVenda, codFormaPgto);
        }


        //CTB_OBRIGACAO
        public List<CTB_CERTIFICADO_DIG> ObterCertificadoDigital()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CTB_CERTIFICADO_DIGs
                    where m.DATA_EXCLUSAO == null
                    select m).ToList();
        }
        public List<CTB_CERTIFICADO_DIG> ObterCertificadoDigitalAVencer(int dias)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            var dataAVencer = DateTime.Now.AddDays(dias);

            return (from m in dbIntra.CTB_CERTIFICADO_DIGs
                    where m.DATA_EXCLUSAO == null
                    && m.VIGENCIA_ATE >= DateTime.Today
                    && m.VIGENCIA_ATE <= dataAVencer
                    select m).ToList();
        }
        public CTB_CERTIFICADO_DIG ObterCertificadoDigital(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CTB_CERTIFICADO_DIGs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirCertificadoDigital(CTB_CERTIFICADO_DIG _certDig)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.CTB_CERTIFICADO_DIGs.InsertOnSubmit(_certDig);
                dbIntra.SubmitChanges();

                return _certDig.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCertificadoDigital(CTB_CERTIFICADO_DIG _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            CTB_CERTIFICADO_DIG _certDig = ObterCertificadoDigital(_novo.CODIGO);

            if (_novo != null)
            {
                _certDig.CNPJ_CPF = _novo.CNPJ_CPF;
                _certDig.RESP_COMPRA = _novo.RESP_COMPRA;
                _certDig.RESP_ASSINATURA = _novo.RESP_ASSINATURA;
                _certDig.TITULAR = _novo.TITULAR;
                _certDig.CERTIFICADO = _novo.CERTIFICADO;
                _certDig.TIPO = _novo.TIPO;
                _certDig.CERTIFICADORA = _novo.CERTIFICADORA;
                _certDig.VIGENCIA_DE = _novo.VIGENCIA_DE;
                _certDig.VIGENCIA_ATE = _novo.VIGENCIA_ATE;
                _certDig.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                _certDig.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                _certDig.DATA_EXCLUSAO = _novo.DATA_EXCLUSAO;
                _certDig.USUARIO_EXCLUSAO = _novo.USUARIO_EXCLUSAO;

                try
                {
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM CTB_OBRIGACAO


    }
}
