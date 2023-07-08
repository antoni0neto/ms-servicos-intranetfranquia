using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class ContagemController
    {
        DCDataContext db;

        //CONT_PROGRAMACAO
        public List<CONT_PROGRAMACAO> ObterProgramacao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_PROGRAMACAOs select m).ToList();
        }
        public List<CONT_PROGRAMACAO> ObterProgramacao(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_PROGRAMACAOs
                    where m.CODIGO_FILIAL == codigoFilial
                    select m).ToList();
        }
        public List<CONT_PROGRAMACAO> ObterProgramacao(int ano, int mes, int dia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DateTime data;
            data = Convert.ToDateTime(ano.ToString() + "/" + mes.ToString() + "/" + dia.ToString()).Date;

            return (from m in db.CONT_PROGRAMACAOs
                    where m.DATA == data
                    select m).ToList();
        }
        public List<CONT_PROGRAMACAO> ObterProgramacao(int ano, int mes)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DateTime dataIni;
            DateTime dataFim;
            dataIni = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-01").Date;

            int diaFinal = DateTime.DaysInMonth(ano, mes);
            dataFim = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-" + diaFinal.ToString()).Date;

            return (from m in db.CONT_PROGRAMACAOs
                    where m.DATA >= dataIni && m.DATA <= dataFim
                    select m).ToList();
        }
        public CONT_PROGRAMACAO ObterProgramacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_PROGRAMACAOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public List<CONT_PROGRAMACAO> ObterDataParaContagem(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.CONT_PROGRAMACAOs
                    join j in db.CONT_RESULTADOs
                    on i.DATA equals j.DATA into progLeft
                    from j in progLeft.Where(p => p.CODIGO_FILIAL == codigoFilial).DefaultIfEmpty()
                    where j.CODIGO == null && i.CODIGO_FILIAL == codigoFilial
                    orderby i.DATA
                    select i).ToList();
        }
        public int InserirProgramacao(CONT_PROGRAMACAO _programacao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.CONT_PROGRAMACAOs.InsertOnSubmit(_programacao);
                db.SubmitChanges();

                return _programacao.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProgramacao(CONT_PROGRAMACAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            CONT_PROGRAMACAO _programacao = ObterProgramacao(_novo.CODIGO);

            if (_novo != null)
            {
                _programacao.CODIGO = _novo.CODIGO;
                _programacao.DATA = _novo.DATA;
                _programacao.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _programacao.TIPO = _novo.TIPO;
                _programacao.OBS = _novo.OBS;


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
        public void ExcluirProgramacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                CONT_PROGRAMACAO _programacao = ObterProgramacao(codigo);
                if (_programacao != null)
                {
                    db.CONT_PROGRAMACAOs.DeleteOnSubmit(_programacao);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CONT_RESULTADO> ObterResultado(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_RESULTADOs
                    where m.CODIGO_FILIAL.Trim() == codigoFilial.Trim()
                    orderby m.DATA
                    select m).ToList();
        }
        public List<CONT_RESULTADO> ObterResultado(int ano, int mes)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DateTime dataIni;
            DateTime dataFim;
            dataIni = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-01").Date;

            int diaFinal = DateTime.DaysInMonth(ano, mes);
            dataFim = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-" + diaFinal.ToString()).Date;

            return (from m in db.CONT_RESULTADOs
                    where m.DATA >= dataIni && m.DATA <= dataFim
                    orderby m.DATA
                    select m).ToList();
        }
        public CONT_RESULTADO ObterResultado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_RESULTADOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirResultado(CONT_RESULTADO _resultado)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.CONT_RESULTADOs.InsertOnSubmit(_resultado);
                db.SubmitChanges();

                return _resultado.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirResultado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                CONT_RESULTADO _resultado = ObterResultado(codigo);
                if (_resultado != null)
                {
                    db.CONT_RESULTADOs.DeleteOnSubmit(_resultado);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CONT_RESULTADO_FECHAMENTO ObterResultadoFechamento(DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_RESULTADO_FECHAMENTOs
                    where m.DATA == data
                    select m).SingleOrDefault();
        }
        public void InserirResultadoFechamento(CONT_RESULTADO_FECHAMENTO _resultadoFec)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.CONT_RESULTADO_FECHAMENTOs.InsertOnSubmit(_resultadoFec);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirResultadoFechamento(DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                CONT_RESULTADO_FECHAMENTO _resultadoFec = ObterResultadoFechamento(data);
                if (_resultadoFec != null)
                {
                    db.CONT_RESULTADO_FECHAMENTOs.DeleteOnSubmit(_resultadoFec);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool MesFechado(int ano, int mes)
        {
            DateTime dataIni;
            dataIni = Convert.ToDateTime(ano.ToString() + "-" + mes.ToString() + "-01").Date;
            var x = ObterResultadoFechamento(dataIni);

            if (x != null)
                return true;

            return false;
        }


        public List<SP_OBTER_CONT_RESULT_LOJAResult> ObterResultadoPorLoja(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_CONT_RESULT_LOJA(codigoFilial) select i).ToList();
        }
        public List<SP_OBTER_CONT_RESULT_LOJAResult> ObterResultadoPorLoja(string codigoFilial, DateTime periodoIni, DateTime periodoFim)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_CONT_RESULT_LOJA(codigoFilial)
                    .Where(p => p.DATA_CONTAGEM >= periodoIni && p.DATA_CONTAGEM <= periodoFim)
                    select i).ToList();
        }


        public List<CONT_LOJA_60MAI> ObterLoja60Mais(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_LOJA_60MAIs
                    where m.CODIGO_FILIAL.Trim() == codigoFilial.Trim()
                    && m.DATA_ESTOQUE_LOJA == null
                    orderby m.PRODUTO
                    select m).ToList();
        }
        public CONT_LOJA_60MAI ObterLoja60Mais(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_LOJA_60MAIs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirLoja60Mais(CONT_LOJA_60MAI _loja60mais)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.CONT_LOJA_60MAIs.InsertOnSubmit(_loja60mais);
                db.SubmitChanges();

                return _loja60mais.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarLoja60Mais(CONT_LOJA_60MAI _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            CONT_LOJA_60MAI _loja60mais = ObterLoja60Mais(_novo.CODIGO);

            if (_novo != null)
            {
                _loja60mais.CODIGO = _novo.CODIGO;
                _loja60mais.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _loja60mais.FILIAL = _novo.FILIAL;
                _loja60mais.PRODUTO = _novo.PRODUTO;
                _loja60mais.QTDE_VENDIDA = _novo.QTDE_VENDIDA;
                _loja60mais.PERIODO_INI = _novo.PERIODO_INI;
                _loja60mais.PERIODO_FIM = _novo.PERIODO_FIM;
                _loja60mais.EMAIL = _novo.EMAIL;
                _loja60mais.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _loja60mais.STATUS = _novo.STATUS;
                _loja60mais.ESTOQUE_LOJA_EXP = _novo.ESTOQUE_LOJA_EXP;
                _loja60mais.ESTOQUE_LOJA_XP = _novo.ESTOQUE_LOJA_XP;
                _loja60mais.ESTOQUE_LOJA_PP = _novo.ESTOQUE_LOJA_PP;
                _loja60mais.ESTOQUE_LOJA_P = _novo.ESTOQUE_LOJA_P;
                _loja60mais.ESTOQUE_LOJA_M = _novo.ESTOQUE_LOJA_M;
                _loja60mais.ESTOQUE_LOJA_G = _novo.ESTOQUE_LOJA_G;
                _loja60mais.ESTOQUE_LOJA_GG = _novo.ESTOQUE_LOJA_GG;
                _loja60mais.DATA_ESTOQUE_LOJA = _novo.DATA_ESTOQUE_LOJA;
                _loja60mais.RESPONSAVEL = _novo.RESPONSAVEL;
                _loja60mais.QTDE_ESTOQUE_RET = _novo.QTDE_ESTOQUE_RET;
                _loja60mais.DATA_ESTOQUE_RET = _novo.DATA_ESTOQUE_RET;
                _loja60mais.QTDE_TRANSITO = _novo.QTDE_TRANSITO;
                _loja60mais.OBSERVACAO = _novo.OBSERVACAO;
                _loja60mais.USUARIO_BAIXA = _novo.USUARIO_BAIXA;


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
        public void ExcluirLoja60Mais(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                CONT_LOJA_60MAI _loja60mais = ObterLoja60Mais(codigo);
                if (_loja60mais != null)
                {
                    db.CONT_LOJA_60MAIs.DeleteOnSubmit(_loja60mais);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_OBTER_ESTOQUE_PROD_LOJAResult ObterEstoqueProdutoPorLoja(string filial, string codigoProduto, string codigoCor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_ESTOQUE_PROD_LOJA(filial, codigoProduto, codigoCor) select i).SingleOrDefault();
        }
        public SP_OBTER_TRANSITO_PROD_LOJAResult ObterTransitoProdutoPorLoja(string filial, string codigoProduto, string codigoCor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_TRANSITO_PROD_LOJA(filial, codigoProduto, codigoCor) select i).SingleOrDefault();
        }

        public List<SP_OBTER_60MAIS_RESULTADOResult> Obter60MaisResultado(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_60MAIS_RESULTADO(codigoFilial) select i).ToList();
        }


        public List<CONT_RESULTADO_FINAL> ObterResultadoFinal(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.CONT_RESULTADO_FINALs
                    where m.CODIGO_FILIAL.Trim() == codigoFilial.Trim()
                    orderby m.DATA_CONTAGEM
                    select m).ToList();
        }
        public int InserirResultadoFinal(CONT_RESULTADO_FINAL _resultadoFinal)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.CONT_RESULTADO_FINALs.InsertOnSubmit(_resultadoFinal);
                db.SubmitChanges();

                return _resultadoFinal.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //CONTAGEM FISICA
        //ESTOQUE_LOJA_CONT
        public List<ESTOQUE_LOJA_CONT> ObterEstoqueLojaContagem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_CONTs
                    orderby j.DATA_CONTAGEM
                    select j).ToList();
        }
        public List<ESTOQUE_LOJA_CONT> ObterEstoqueLojaContagem(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_CONTs
                    where j.CODIGO_FILIAL == codigoFilial
                    select j).ToList();
        }
        public ESTOQUE_LOJA_CONT ObterEstoqueLojaContagem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_CONTs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public int InserirEstoqueLojaContagem(ESTOQUE_LOJA_CONT _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.ESTOQUE_LOJA_CONTs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEstoqueLojaContagem(ESTOQUE_LOJA_CONT _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            ESTOQUE_LOJA_CONT _estoqueLojaCont = ObterEstoqueLojaContagem(_novo.CODIGO);

            if (_estoqueLojaCont != null)
            {
                _estoqueLojaCont.CODIGO = _novo.CODIGO;
                _estoqueLojaCont.CODIGO_PAI = _novo.CODIGO_PAI;
                _estoqueLojaCont.DESCRICAO = _novo.DESCRICAO;
                _estoqueLojaCont.QTDE_LINHA_ARQ = _novo.QTDE_LINHA_ARQ;
                _estoqueLojaCont.QTDE_LINHA_VALIDA = _novo.QTDE_LINHA_VALIDA;
                _estoqueLojaCont.QTDE_LINHA_INVALIDA = _novo.QTDE_LINHA_INVALIDA;

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
        public void ExcluirEstoqueLojaContagem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                ESTOQUE_LOJA_CONT _estoqueLojaCont = ObterEstoqueLojaContagem(codigo);
                if (_estoqueLojaCont != null)
                {
                    db.ESTOQUE_LOJA_CONTs.DeleteOnSubmit(_estoqueLojaCont);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ESTOQUE_LOJA_CONT_ARQ
        public List<ESTOQUE_LOJA_CONT_ARQ> ObterEstoqueLojaContagemArq(int codigoEstoqueLojaCont)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_CONT_ARQs
                    where j.ESTOQUE_LOJA_CONT == codigoEstoqueLojaCont
                    select j).ToList();
        }
        public ESTOQUE_LOJA_CONT_ARQ ObterEstoqueLojaContagemArqCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_CONT_ARQs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public int InserirEstoqueLojaContagemArq(ESTOQUE_LOJA_CONT_ARQ _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.ESTOQUE_LOJA_CONT_ARQs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ESTOQUE_LOJA_CONT_RET
        public List<ESTOQUE_LOJA_CONT_RET> ObterEstoqueLojaContagemRet(int codigoEstoqueLojaCont)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_CONT_RETs
                    where j.ESTOQUE_LOJA_CONT == codigoEstoqueLojaCont
                    select j).ToList();
        }
        public ESTOQUE_LOJA_CONT_RET ObterEstoqueLojaContagemRetCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_CONT_RETs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public int InserirEstoqueLojaContagemRet(ESTOQUE_LOJA_CONT_RET _estoqueLojaContRet)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.ESTOQUE_LOJA_CONT_RETs.InsertOnSubmit(_estoqueLojaContRet);
                db.SubmitChanges();

                return _estoqueLojaContRet.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEstoqueLojaContagemRet(ESTOQUE_LOJA_CONT_RET _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            ESTOQUE_LOJA_CONT_RET estoqueLojaContRet = ObterEstoqueLojaContagemRetCodigo(_novo.CODIGO);

            if (_novo != null)
            {
                estoqueLojaContRet.CODIGO = _novo.CODIGO;
                estoqueLojaContRet.ESTOQUE_REC = _novo.ESTOQUE_REC;

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

        public bool GerarSaldoEstoquePA(DateTime dataContagem, string filial, int codigoEstoqueLojaCont)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            var resp = (from j in db.SP_GERAR_SALDO_ESTOQUE_PA(dataContagem, filial, codigoEstoqueLojaCont) select j).SingleOrDefault();
            if (resp != null)
                return Convert.ToBoolean(resp.OK);

            return false;
        }

        public List<SP_OBTER_REL_DIFF_EST_CONTAGEMResult> ObterEstoqueLojaContRel(int estoqueLojaCont)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from j in db.SP_OBTER_REL_DIFF_EST_CONTAGEM(estoqueLojaCont) select j).ToList();
        }

        public List<SP_OBTER_REL_DIFF_EST_RECONTAGEMResult> ObterEstoqueLojaRecontRel(int estoqueLojaCont)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from j in db.SP_OBTER_REL_DIFF_EST_RECONTAGEM(estoqueLojaCont) select j).ToList();
        }

    }
}
