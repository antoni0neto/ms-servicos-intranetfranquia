using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class DREController
    {
        DCDataContext db;
        LINXDataContext linxDB;
        INTRADataContext intraDB;

        //SP_OBTER_DRE
        public List<SP_OBTER_DREResult> ObterDRE2017(DateTime dataIni, DateTime dataFim, string codigoFilial, int mesIni, int mesFim)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE(dataIni, dataFim, codigoFilial, mesIni, mesFim) orderby m.FILIAL select m).ToList();
        }
        public List<SP_DRE_CC_LANCAMENTOResult> ObterDREContaContabil(string contaContabil, string filial, DateTime dataIni, DateTime dataFim, string tela)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_DRE_CC_LANCAMENTO(contaContabil, filial, dataIni, dataFim, tela) orderby m.DEBITO select m).ToList();
        }

        public List<SP_OBTER_DRE50Result> ObterDRE50(DateTime dataIni, DateTime dataFim, string tipo, string unidadeNegocio, string codigoFilial, int mesIni, int mesFim)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE50(dataIni, dataFim, tipo, unidadeNegocio, codigoFilial, mesIni, mesFim) orderby m.FILIAL select m).ToList();
        }
        public List<SP_OBTER_DRE50_LANCAMENTOResult> ObterDRE50ContaContabil(string contaContabil, string tipo, string unidadeNegocio, string codigoFilial, DateTime dataIni, DateTime dataFim, string query, string centroCusto)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE50_LANCAMENTO(contaContabil, tipo, unidadeNegocio, codigoFilial, dataIni, dataFim, query, centroCusto) orderby m.DEBITO select m).ToList();
        }

        public List<SP_OBTER_DRE_LANCAMENTOResult> ObterDRELancamento(DateTime? dataIni, DateTime? dataFim, string tipo, string codFilial, string contaContabil, string centroCusto, int? lancamento)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE_LANCAMENTO(dataIni, dataFim, tipo, codFilial, contaContabil, centroCusto, lancamento) orderby m.TIPO select m).ToList();
        }

        public List<SP_OBTER_DRE_TRANSPORTEResult> ObterDRETransporte(DateTime dataIni, DateTime dataFim)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE_TRANSPORTE(dataIni, dataFim) select m).ToList();
        }
        public List<SP_OBTER_DRE_CORREIOResult> ObterDRECorreio(DateTime dataIni, DateTime dataFim)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE_CORREIO(dataIni, dataFim) select m).ToList();
        }

        public DRE_TRANSPORTE ObterDRETransporte(string codigoFilial, int ano, int mes)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_TRANSPORTEs
                    where m.CODIGO_FILIAL == codigoFilial &&
                            m.ANO == ano &&
                            m.MES == mes
                    select m).SingleOrDefault();
        }
        public void InserirDRETransporte(DRE_TRANSPORTE _transporte)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                intraDB.DRE_TRANSPORTEs.InsertOnSubmit(_transporte);
                intraDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarDRETransporte(DRE_TRANSPORTE _novo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            DRE_TRANSPORTE _transporte = ObterDRETransporte(_novo.CODIGO_FILIAL, _novo.ANO, _novo.MES);

            if (_novo != null)
            {

                _transporte.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _transporte.FILIAL = _novo.FILIAL;
                _transporte.CENTRO_CUSTO = _novo.CENTRO_CUSTO;
                _transporte.ANO = _novo.ANO;
                _transporte.MES = _novo.MES;
                _transporte.VALOR = _novo.VALOR;

                try
                {
                    intraDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DRE_CORREIO ObterDRECorreio(string codigoFilial, int ano, int mes)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_CORREIOs
                    where m.CODIGO_FILIAL == codigoFilial &&
                            m.ANO == ano &&
                            m.MES == mes
                    select m).SingleOrDefault();
        }
        public void InserirDRECorreio(DRE_CORREIO _correio)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                intraDB.DRE_CORREIOs.InsertOnSubmit(_correio);
                intraDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarDRECorreio(DRE_CORREIO _novo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            DRE_CORREIO _correio = ObterDRECorreio(_novo.CODIGO_FILIAL, _novo.ANO, _novo.MES);

            if (_novo != null)
            {

                _correio.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _correio.FILIAL = _novo.FILIAL;
                _correio.CENTRO_CUSTO = _novo.CENTRO_CUSTO;
                _correio.ANO = _novo.ANO;
                _correio.MES = _novo.MES;
                _correio.VALOR = _novo.VALOR;

                try
                {
                    intraDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<DRE_CTB_LANCAMENTO_ITEM> ObterDRELancamento(int lancamento)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_CTB_LANCAMENTO_ITEMs
                    where m.LANCAMENTO == lancamento
                    select m).ToList();
        }
        public DRE_CTB_LANCAMENTO_ITEM ObterDRELancamento(int lancamento, int item)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_CTB_LANCAMENTO_ITEMs
                    where m.LANCAMENTO == lancamento &&
                            m.ITEM == item
                    select m).SingleOrDefault();
        }
        public void AtualizarDRELancamento(DRE_CTB_LANCAMENTO_ITEM _novo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            DRE_CTB_LANCAMENTO_ITEM _lancamento = ObterDRELancamento(_novo.LANCAMENTO, _novo.ITEM);

            if (_novo != null)
            {
                _lancamento.CONTA_CONTABIL = _novo.CONTA_CONTABIL;
                _lancamento.CREDITO = _novo.CREDITO;
                _lancamento.DEBITO = _novo.DEBITO;
                _lancamento.HISTORICO = _novo.HISTORICO;
                _lancamento.ITEM = _novo.ITEM;
                _lancamento.LANCAMENTO = _novo.LANCAMENTO;
                _lancamento.RATEIO_CENTRO_CUSTO = _novo.RATEIO_CENTRO_CUSTO;
                _lancamento.RATEIO_FILIAL = _novo.RATEIO_FILIAL;
                _lancamento.USUARIO_MOV = _novo.USUARIO_MOV;

                _lancamento.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                _lancamento.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                _lancamento.OBS_ALTERACAO = _novo.OBS_ALTERACAO;

                try
                {
                    intraDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //SP_DRE_OBTER_DADOS
        public List<SP_DRE_OBTER_DADOSResult> ObterDRE(DateTime dataIni, DateTime dataFim, string filial, char intercompany, int query, bool orcamento)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_DRE_OBTER_DADOS(dataIni, dataFim, filial, intercompany, query, orcamento) orderby m.Filial select m).ToList();
        }

        //SP_DRE_COMPARATIVO
        public List<SP_DRE_COMPARATIVOResult> ObterDREComparativo(DateTime dataIni, DateTime dataFim, string filial, int mesIni, int mesFim)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_DRE_COMPARATIVO(dataIni, dataFim, filial, mesIni, mesFim) select m).ToList();
        }

        //SP_DRE_MAODEOBRADIRETAResult
        public List<SP_DRE_MAODEOBRADIRETAResult> ObterMaoDeObraDireta(int mes, int ano)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_DRE_MAODEOBRADIRETA(mes, ano) select m).ToList();
        }

        //SP_DRE_RESUMO
        public List<SP_DRE_RESUMOResult> ObterDRELoja(int? ano, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_DRE_RESUMO(ano, filial) orderby m.FILIAL select m).ToList();
        }

        public List<CTB_CENTRO_CUSTO_GRUPO> ObterCentroCustoGrupo()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CTB_CENTRO_CUSTO_GRUPOs
                    orderby m.DESC_GRUPO_CENTRO_CUSTO
                    select m).ToList();
        }
        public CTB_CENTRO_CUSTO_GRUPO ObterCentroCustoGrupo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CTB_CENTRO_CUSTO_GRUPOs
                    where m.ID_GRUPO_CENTRO_CUSTO == codigo
                    select m).SingleOrDefault();
        }

        public List<CTB_CENTRO_CUSTO> ObterCentroCusto()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CTB_CENTRO_CUSTOs
                    where m.CENTRO_CUSTO != "1"
                    && m.CENTRO_CUSTO != "2"
                    && m.CENTRO_CUSTO != "3"
                    && m.CENTRO_CUSTO != "4"
                    && m.CENTRO_CUSTO != "5"
                    && m.CENTRO_CUSTO != "6"
                    && m.CENTRO_CUSTO != "7"
                    && m.CENTRO_CUSTO != "8"
                    && m.CENTRO_CUSTO != "9"
                    && m.CENTRO_CUSTO != "10"
                    orderby m.DESC_CENTRO_CUSTO
                    select m).ToList();
        }
        public CTB_CENTRO_CUSTO ObterCentroCusto(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CTB_CENTRO_CUSTOs
                    where m.ID_GRUPO_CENTRO_CUSTO == codigo
                    select m).SingleOrDefault();
        }

        //SP_OBTER_CENTRO_CUSTO_LANCAMENTO
        public List<SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult> ObterCentroCustoLancamento(DateTime dataIni, DateTime dataFim, int? idGrupo, string centroCusto, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_CENTRO_CUSTO_LANCAMENTO(dataIni, dataFim, idGrupo, centroCusto, filial) select m).ToList();
        }

        public List<CONTA_CONTABIL_DRE> ObterContaContabilDRE()
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            return (from m in db.CONTA_CONTABIL_DREs
                    orderby m.GRUPO, m.CONTA_CONTABIL
                    select m).ToList();
        }
        public CONTA_CONTABIL_DRE ObterContaContabilDRE(string contaContabil)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            return (from m in db.CONTA_CONTABIL_DREs
                    where m.CONTA_CONTABIL == contaContabil
                    select m).SingleOrDefault();
        }

        public List<DRE_AJUDA> ObterAjudaDRE()
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_AJUDAs
                    select m).ToList();
        }
        public DRE_AJUDA ObterAjuda(int ID, string grupo, string linha, string linhaExtra, string tipo)
        {
            DRE_AJUDA ajuda = new DRE_AJUDA();
            if (grupo != "" || linha != "")
            {
                ajuda = ObterAjudaDREPorGrupoELinha(ID, grupo, linha, linhaExtra, tipo);
                if (ajuda == null)
                {
                    if (linha != "")
                        ajuda = ObterAjudaDREPorLinha(ID, linha, linhaExtra, tipo);
                    else
                        ajuda = ObterAjudaDREPorGrupo(ID, grupo, tipo);
                }
            }

            return ajuda;
        }
        public DRE_AJUDA ObterAjudaDREPorGrupoELinha(int id, string grupo, string linha, string linhaExtra, string tipo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_AJUDAs
                    where m.ID == id && m.GRUPO == grupo && m.LINHA == linha && m.LINHA_EXTRA == linhaExtra && m.TIPO == tipo
                    select m).SingleOrDefault();
        }
        public DRE_AJUDA ObterAjudaDREPorGrupo(int id, string grupo, string tipo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_AJUDAs
                    where m.ID == id && m.GRUPO == grupo && m.TIPO == tipo
                    select m).SingleOrDefault();
        }
        public DRE_AJUDA ObterAjudaDREPorLinha(int id, string linha, string linhaExtra, string tipo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_AJUDAs
                    where m.ID == id && m.LINHA == linha && m.LINHA_EXTRA == linhaExtra && m.TIPO == tipo
                    select m).SingleOrDefault();
        }
        public void InserirAjudaDRE(DRE_AJUDA _novo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                intraDB.DRE_AJUDAs.InsertOnSubmit(_novo);
                intraDB.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAjudaDRE(DRE_AJUDA _novo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);

            var ajuda = ObterAjuda(_novo.ID, _novo.GRUPO, _novo.LINHA, _novo.LINHA_EXTRA, _novo.TIPO);

            if (_novo != null)
            {
                ajuda.ID = _novo.ID;
                ajuda.GRUPO = _novo.GRUPO;
                ajuda.LINHA = _novo.LINHA;
                ajuda.LINHA_EXTRA = _novo.LINHA_EXTRA;
                ajuda.DESCRICAO = _novo.DESCRICAO;
                ajuda.QUERY = _novo.QUERY;

                try
                {
                    intraDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<DRE_ORCAMENTO> ObterOrcamento(int ano)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            return (from m in db.DRE_ORCAMENTOs
                    where m.ANO == ano
                    select m).ToList();
        }
        public DRE_ORCAMENTO ObterOrcamentoCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            return (from m in db.DRE_ORCAMENTOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void AtualizarOrcamento(DRE_ORCAMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);

            DRE_ORCAMENTO _orcamento = ObterOrcamentoCodigo(_novo.CODIGO);

            if (_novo != null)
            {
                _orcamento.CODIGO = _novo.CODIGO;
                _orcamento.JANEIRO = _novo.JANEIRO;
                _orcamento.FEVEREIRO = _novo.FEVEREIRO;
                _orcamento.MARCO = _novo.MARCO;
                _orcamento.ABRIL = _novo.ABRIL;
                _orcamento.MAIO = _novo.MAIO;
                _orcamento.JUNHO = _novo.JUNHO;
                _orcamento.JULHO = _novo.JULHO;
                _orcamento.AGOSTO = _novo.AGOSTO;
                _orcamento.SETEMBRO = _novo.SETEMBRO;
                _orcamento.OUTUBRO = _novo.OUTUBRO;
                _orcamento.NOVEMBRO = _novo.NOVEMBRO;
                _orcamento.DEZEMBRO = _novo.DEZEMBRO;

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

        public List<DRE_FECHAMENTO_PERIODO> ObterDREFechamentoPeriodo()
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_FECHAMENTO_PERIODOs
                    select m).ToList();
        }
        public DRE_FECHAMENTO_PERIODO ObterDREFechamentoPeriodo(int ano, int mes, string tipo)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_FECHAMENTO_PERIODOs
                    where m.ANO == ano && m.MES == mes && m.TIPO == tipo
                    select m).SingleOrDefault();
        }



        public List<SP_OBTER_DRE500_FABRICAResult> ObterDRE500Fabrica(DateTime dataIni, DateTime dataFim, string tipo, string unidadeNegocio, string codigoFilial, int mesIni, int mesFim)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE500_FABRICA(dataIni, dataFim, tipo, unidadeNegocio, codigoFilial, mesIni, mesFim) orderby m.FILIAL select m).ToList();
        }
        public List<SP_OBTER_DREV10_LANCAMENTOResult> ObterDREV10Lancamento(string codCentroCusto, string contaContabil, string unidadeNegocio, string codigoFilial, DateTime dataIni, DateTime dataFim)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DREV10_LANCAMENTO(codCentroCusto, contaContabil, unidadeNegocio, codigoFilial, dataIni, dataFim) select m).ToList();
        }

        public List<SP_OBTER_DRE500_LOJAResult> ObterDRE500Loja(DateTime dataIni, DateTime dataFim, string tipo, string unidadeNegocio, string codigoFilial, int mesIni, int mesFim)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DRE500_LOJA(dataIni, dataFim, tipo, unidadeNegocio, codigoFilial, mesIni, mesFim) orderby m.FILIAL select m).ToList();
        }
        public List<SP_OBTER_DREV10_LANCAMENTO_LOJAResult> ObterDREV10LancamentoLoja(string codCentroCusto, string contaContabil, string unidadeNegocio, string codigoFilial, DateTime dataIni, DateTime dataFim, string rateio)
        {
            linxDB = new LINXDataContext(Constante.ConnectionString);
            linxDB.CommandTimeout = 0;
            return (from m in linxDB.SP_OBTER_DREV10_LANCAMENTO_LOJA(codCentroCusto, contaContabil, unidadeNegocio, codigoFilial, dataIni, dataFim, rateio) select m).ToList();
        }


        public DREV10_RATEIOFAT ObterDRERateioFilial(string codigoFilial)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DREV10_RATEIOFATs
                    where m.CODIGO_FILIAL == codigoFilial
                    select m).SingleOrDefault();
        }

        public DRE_PERMISSAO ObterDREPermissao(int codigoUsuario)
        {
            intraDB = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in intraDB.DRE_PERMISSAOs
                    where m.CODIGO_USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }

    }
}
