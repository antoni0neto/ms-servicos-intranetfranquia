using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using Dapper;
using System.Data;

namespace DAL
{
    public class FaccaoController
    {
        DCDataContext db;
        INTRADataContext dbIntra;


        public List<PROD_FORNECEDOR_SUB> ObterFornecedorSub(string fornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_FORNECEDOR_SUBs
                    where m.FORNECEDOR.Trim() == fornecedor.Trim()
                    orderby m.FORNECEDOR_SUB
                    select m).ToList();
        }

        //PROD_HB_SAIDA
        //AUXILIAR PARA MELHORAR DESEMPENHO
        public List<SP_OBTER_LISTA_SAIDAResult> ObterListaSaida(int prod_processo, int query)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_LISTA_SAIDA(prod_processo, query)
                    select m).ToList();
        }
        public List<PROD_HB_SAIDA> ObterListaSaidaEncaixe(int prod_processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_SAIDAs
                    where m.DATA_LIBERACAO == null
                    && m.PROD_PROCESSO == prod_processo
                    select m).ToList();
        }
        public List<PROD_HB_SAIDA> ObterListaSaidaEmissao(int prod_processo)
        {

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_SAIDAs
                    where m.DATA_LIBERACAO != null
                    && m.EMISSAO == null
                    && m.PROD_PROCESSO == prod_processo
                    select m).ToList();
        }
        public PROD_HB_SAIDA ObterSaidaHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_SAIDAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_HB_SAIDA ObterSaidaEmAberto(int prodHb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_SAIDAs where m.PROD_HB == prodHb && m.DATA_LIBERACAO == null select m).SingleOrDefault();
        }
        public PROD_HB_SAIDA ObterCustoProdutoMaior(string colecao, int prodServico, string produto, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_SAIDAs
                    where m.PROD_HB1.CODIGO_PRODUTO_LINX.Trim() == produto.Trim() &&
                        m.PROD_HB1.MOSTRUARIO == mostruario &&
                        m.PROD_HB1.COLECAO == colecao &&
                        m.PROD_SERVICO == prodServico &&
                        m.DATA_LIBERACAO != null
                    //m.CNPJ.Trim() != "10680869000854" // CNPJ HANDBOOK
                    orderby m.PRECO descending
                    select m).FirstOrDefault();
        }
        public PROD_HB_SAIDA ObterCustoProdutoMenor(string colecao, int prodServico, string produto, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_SAIDAs
                    where m.PROD_HB1.CODIGO_PRODUTO_LINX.Trim() == produto.Trim() &&
                        m.PROD_HB1.MOSTRUARIO == mostruario &&
                        m.PROD_HB1.COLECAO == colecao &&
                        m.PROD_SERVICO == prodServico &&
                        m.DATA_LIBERACAO != null
                    //m.CNPJ.Trim() != "10680869000854" // CNPJ HANDBOOK
                    orderby m.PRECO ascending
                    select m).FirstOrDefault();
        }
        public List<PROD_HB_SAIDA> ObterSaidaHB(string prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_SAIDAs
                    where m.PROD_HB == Convert.ToInt32(prod_hb)
                    orderby m.EMISSAO
                    select m).ToList();
        }
        public int InserirSaidaHB(PROD_HB_SAIDA _saida)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.PROD_HB_SAIDAs.InsertOnSubmit(_saida);
                db.SubmitChanges();

                return _saida.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSaidaHB(PROD_HB_SAIDA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PROD_HB_SAIDA _saida = ObterSaidaHB(_novo.CODIGO);

            if (_novo != null)
            {
                _saida.CODIGO = _novo.CODIGO;
                _saida.PROD_HB = _novo.PROD_HB;
                _saida.CNPJ = _novo.CNPJ;
                _saida.FORNECEDOR = _novo.FORNECEDOR;
                _saida.FORNECEDOR_SUB = _novo.FORNECEDOR_SUB;
                _saida.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _saida.NF_SAIDA = _novo.NF_SAIDA;
                _saida.SERIE_NF = _novo.SERIE_NF;
                _saida.EMISSAO = _novo.EMISSAO;
                _saida.USUARIO_EMISSAO = _novo.USUARIO_EMISSAO;
                _saida.VOLUME = _novo.VOLUME;
                _saida.CUSTO = _novo.CUSTO;
                _saida.PRECO = _novo.PRECO;
                _saida.PROD_SERVICO = _novo.PROD_SERVICO;
                _saida.TIPO = _novo.TIPO;
                _saida.USUARIO_LIBERACAO = _novo.USUARIO_LIBERACAO;
                _saida.DATA_LIBERACAO = _novo.DATA_LIBERACAO;
                _saida.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _saida.PROD_PROCESSO = _novo.PROD_PROCESSO;

                _saida.GRADE_EXP = _novo.GRADE_EXP;
                _saida.GRADE_XP = _novo.GRADE_XP;
                _saida.GRADE_PP = _novo.GRADE_PP;
                _saida.GRADE_P = _novo.GRADE_P;
                _saida.GRADE_M = _novo.GRADE_M;
                _saida.GRADE_G = _novo.GRADE_G;
                _saida.GRADE_GG = _novo.GRADE_GG;
                _saida.GRADE_TOTAL = _novo.GRADE_TOTAL;

                _saida.SALDO = _novo.SALDO;
                _saida.DATA_PREV_ENTREGA = _novo.DATA_PREV_ENTREGA;

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

        public void VoltarHBParaEncaixe(int codigoSaida)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PROD_HB_SAIDA _saida = ObterSaidaHB(codigoSaida);

            if (_saida != null)
            {
                _saida.CNPJ = null;
                _saida.FORNECEDOR = null;
                _saida.DATA_LIBERACAO = null;
                _saida.USUARIO_LIBERACAO = null;
                _saida.FORNECEDOR_SUB = null;
                _saida.PROD_SERVICO = null;
                _saida.CUSTO = null;
                _saida.PRECO = null;

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

        public void ExcluirSaidaHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                PROD_HB_SAIDA _saida = ObterSaidaHB(codigo);
                if (_saida != null)
                {
                    db.PROD_HB_SAIDAs.DeleteOnSubmit(_saida);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PROD_HB_SAIDA

        //PROD_HB_SAIDA_LOTE
        public List<PROD_HB_SAIDA_LOTE> ObterListaSaidaEncaixeLote(int prod_processo, int? usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            if (usuario != null)
            {
                return (from m in db.PROD_HB_SAIDA_LOTEs
                        where m.PROD_HB_SAIDA1.DATA_LIBERACAO == null
                        && m.PROD_HB_SAIDA1.PROD_PROCESSO == prod_processo
                        && m.USUARIO_LOTE == usuario
                        && m.DATA_ENCAIXE == null
                        select m).ToList();
            }
            else
            {
                return (from m in db.PROD_HB_SAIDA_LOTEs
                        where m.PROD_HB_SAIDA1.DATA_LIBERACAO == null
                        && m.PROD_HB_SAIDA1.PROD_PROCESSO == prod_processo
                        //&& m.USUARIO_LOTE == usuario
                        && m.DATA_ENCAIXE == null
                        select m).ToList();
            }

        }
        public List<PROD_HB_SAIDA_LOTE> ObterListaSaidaEmissaoLote(int prod_processo)
        {

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_SAIDA_LOTEs
                    where m.PROD_HB_SAIDA1.PROD_PROCESSO == prod_processo
                    && m.PROD_HB_SAIDA1.EMISSAO == null
                    && m.DATA_ENCAIXE != null
                    && m.DATA_EMISSAO == null
                    select m).ToList();
        }
        public PROD_HB_SAIDA_LOTE ObterSaidaHBLote(int codigo)
        {

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_SAIDA_LOTEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_HB_SAIDA_LOTE ObterSaidaHBLote(string prod_hb_saida)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_SAIDA_LOTEs
                    where m.PROD_HB_SAIDA == Convert.ToInt32(prod_hb_saida)
                    orderby m.PROD_HB_SAIDA1.PROD_HB1.HB
                    select m).FirstOrDefault();
        }
        public int InserirSaidaHBLote(PROD_HB_SAIDA_LOTE _saidaLote)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_SAIDA_LOTEs.InsertOnSubmit(_saidaLote);
                db.SubmitChanges();

                return _saidaLote.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSaidaHBLote(PROD_HB_SAIDA_LOTE _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PROD_HB_SAIDA_LOTE _saidaLote = ObterSaidaHBLote(_novo.CODIGO);

            if (_novo != null)
            {
                _saidaLote.CODIGO = _novo.CODIGO;
                _saidaLote.LOTE = _novo.LOTE;
                _saidaLote.PROD_HB_SAIDA = _novo.PROD_HB_SAIDA;
                _saidaLote.USUARIO_LOTE = _novo.USUARIO_LOTE;
                _saidaLote.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _saidaLote.DATA_ENCAIXE = _novo.DATA_ENCAIXE;
                _saidaLote.DATA_EMISSAO = _novo.DATA_EMISSAO;

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
        public void ExcluirSaidaHBLote(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_SAIDA_LOTE _saidaLote = ObterSaidaHBLote(codigo);
                if (_saidaLote != null)
                {
                    db.PROD_HB_SAIDA_LOTEs.DeleteOnSubmit(_saidaLote);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ObterNumeroLote(int codigoUsuario, int prod_processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.SP_OBTER_SEQ_PROD_HB_SAIDA_LOTE(codigoUsuario, prod_processo) select m).FirstOrDefault().SEQ_PROD_HB_SAIDA_LOTE.Value;
        }
        //FIM PROD_HB_SAIDA_LOTE

        //PROD_HB_ENTRADA
        public List<SP_OBTER_LISTA_ENTRADAResult> ObterListaEntrada(int prod_processo, string fornecedor, char? mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_LISTA_ENTRADA(prod_processo, fornecedor, mostruario)
                    select m).ToList();
        }
        public List<PROD_HB_ENTRADA> ObterListaEntrada(int prod_processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_ENTRADAs
                    where m.PROD_HB_SAIDA1.PROD_PROCESSO == prod_processo
                    orderby m.RECEBIMENTO
                    select m).ToList();
        }
        public List<PROD_HB_ENTRADA> ObterEntradaHB()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_ENTRADAs
                    orderby m.RECEBIMENTO
                    select m).ToList();
        }
        public PROD_HB_ENTRADA ObterEntradaHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_ENTRADAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<PROD_HB_ENTRADA> ObterEntradaHB(string prod_hb_saida)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_ENTRADAs
                    where m.PROD_HB_SAIDA == Convert.ToInt32(prod_hb_saida)
                    orderby m.RECEBIMENTO
                    select m).ToList();
        }
        public int InserirEntradaHB(PROD_HB_ENTRADA _entrada)
        {

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_ENTRADAs.InsertOnSubmit(_entrada);
                db.SubmitChanges();

                return _entrada.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEntradaHB(PROD_HB_ENTRADA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PROD_HB_ENTRADA _entrada = ObterEntradaHB(_novo.CODIGO);

            if (_novo != null)
            {
                _entrada.CODIGO = _novo.CODIGO;
                _entrada.PROD_HB_SAIDA = _novo.PROD_HB_SAIDA;
                _entrada.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _entrada.NF_ENTRADA = _novo.NF_ENTRADA;
                _entrada.SERIE_NF = _novo.SERIE_NF;
                _entrada.EMISSAO = _novo.EMISSAO;
                _entrada.RECEBIMENTO = _novo.RECEBIMENTO;
                _entrada.USUARIO_RECEBIMENTO = _novo.USUARIO_RECEBIMENTO;
                _entrada.GRADE_EXP = _novo.GRADE_EXP;
                _entrada.GRADE_XP = _novo.GRADE_XP;
                _entrada.GRADE_PP = _novo.GRADE_PP;
                _entrada.GRADE_P = _novo.GRADE_P;
                _entrada.GRADE_M = _novo.GRADE_M;
                _entrada.GRADE_G = _novo.GRADE_G;
                _entrada.GRADE_GG = _novo.GRADE_GG;
                _entrada.GRADE_TOTAL = _novo.GRADE_TOTAL;
                _entrada.STATUS = _novo.STATUS;
                _entrada.DATA_DISTRIBUICAO = _novo.DATA_DISTRIBUICAO;
                _entrada.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _entrada.PRODUTO_ACABADO = _novo.PRODUTO_ACABADO;
                _entrada.CUSTO_DESCONTO = _novo.CUSTO_DESCONTO;
                _entrada.QTDE_NOTA = _novo.QTDE_NOTA;
                _entrada.OBSERVACAO = _novo.OBSERVACAO;

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
        public void ExcluirEntradaHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_ENTRADA _entrada = ObterEntradaHB(codigo);
                if (_entrada != null)
                {
                    db.PROD_HB_ENTRADAs.DeleteOnSubmit(_entrada);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PROD_HB_ENTRADA

        //PROD_HB_ENTRADA_QTDE
        public List<PROD_HB_ENTRADA_QTDE> ObterEntradaQtde(string prod_hb_entrada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_ENTRADA_QTDEs
                    where m.PROD_HB_ENTRADA == Convert.ToInt32(prod_hb_entrada)
                    select m).ToList();
        }
        public SP_OBTER_QTDE_ENTRADA_HBResult ObterEntradaQtdeHB(int codigoHB, int prodProcessoGrade, int prodProcessoFaccao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.SP_OBTER_QTDE_ENTRADA_HB(codigoHB, prodProcessoGrade, prodProcessoFaccao)
                    select m).SingleOrDefault();
        }
        public PROD_HB_ENTRADA_QTDE ObterEntradaQtdeCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_ENTRADA_QTDEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirEntradaQtde(PROD_HB_ENTRADA_QTDE _entradaQtde)
        {

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                _entradaQtde.ENTRADA_LINX = 'N';
                _entradaQtde.LIBERADO_LOJA = 'N';

                db.PROD_HB_ENTRADA_QTDEs.InsertOnSubmit(_entradaQtde);
                db.SubmitChanges();

                return _entradaQtde.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEntradaQtde(PROD_HB_ENTRADA_QTDE _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PROD_HB_ENTRADA_QTDE _entrada = ObterEntradaQtdeCodigo(_novo.CODIGO);

            if (_novo != null)
            {
                _entrada.CODIGO = _novo.CODIGO;
                _entrada.PROD_HB_ENTRADA = _novo.PROD_HB_ENTRADA;
                _entrada.PROD_PROCESSO = _novo.PROD_PROCESSO;
                _entrada.GRADE_EXP = _novo.GRADE_EXP;
                _entrada.GRADE_XP = _novo.GRADE_XP;
                _entrada.GRADE_PP = _novo.GRADE_PP;
                _entrada.GRADE_P = _novo.GRADE_P;
                _entrada.GRADE_M = _novo.GRADE_M;
                _entrada.GRADE_G = _novo.GRADE_G;
                _entrada.GRADE_GG = _novo.GRADE_GG;
                _entrada.GRADE_TOTAL = _novo.GRADE_TOTAL;
                _entrada.STATUS = _novo.STATUS;
                _entrada.ENTRADA_LINX = _novo.ENTRADA_LINX;
                _entrada.LIBERADO_LOJA = _novo.LIBERADO_LOJA;

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
        public void ExcluirEntradaQtde(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_ENTRADA_QTDE _entradaQtde = ObterEntradaQtdeCodigo(codigo);
                if (_entradaQtde != null)
                {
                    db.PROD_HB_ENTRADA_QTDEs.DeleteOnSubmit(_entradaQtde);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PROD_HB_ENTRADA_QTDE

        //SP_GERAR_ROTAMODELO_PRODUTOResult
        public SP_GERAR_ROTAMODELO_PRODUTOResult GerarRotaModeloProduto(string produto, string cor, bool temPCP, bool temCorte, bool temEmtampa, bool temLavanderia, bool temFaccao, bool temAcabamento, bool temConferencia, bool temEstoque, bool temCor2, bool temCor3, bool temColante, bool temForro, bool temGalao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.SP_GERAR_ROTAMODELO_PRODUTO(produto, cor, temPCP, temCorte, temEmtampa, temLavanderia, temFaccao, temAcabamento, temConferencia, temEstoque, temCor2, temCor3, temColante, temForro, temGalao) select m).SingleOrDefault();
        }
        // FIM SP_GERAR_ROTAMODELO_PRODUTOResult

        //SP_GERAR_ORDEM_PRODUCAOResult
        public SP_GERAR_ORDEM_PRODUCAOResult GerarOrdemProducao(int codigoHB)
        {
            int totalGrade = 0;
            var prodController = new ProducaoController();

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            var prod_hb = prodController.ObterHB(codigoHB);
            if (prod_hb != null)
            {
                var grade = prodController.ObterGradeHB(prod_hb.CODIGO, 3);
                if (grade != null)
                {
                    db = new DCDataContext(Constante.ConnectionString);

                    totalGrade = Convert.ToInt32(grade.GRADE_EXP + grade.GRADE_XP + grade.GRADE_PP + grade.GRADE_P + grade.GRADE_M + grade.GRADE_G + grade.GRADE_GG);

                    return (from m in db.SP_GERAR_ORDEM_PRODUCAO(prod_hb.CODIGO_PRODUTO_LINX.Trim(),
                                                                prod_hb.COR.Trim(),
                                                                totalGrade,
                                                                prod_hb.CODIGO_PRODUTO_LINX.Trim(),
                                                                prod_hb.COLECAO.Trim(),
                                                                prod_hb.HB,
                                                                prod_hb.MOSTRUARIO,
                                                                "INTRANET",
                                                                grade.GRADE_EXP,
                                                                grade.GRADE_XP,
                                                                grade.GRADE_PP,
                                                                grade.GRADE_P,
                                                                grade.GRADE_M,
                                                                grade.GRADE_G,
                                                                grade.GRADE_GG)
                            select m).FirstOrDefault();
                }
            }

            return null;
        }
        // FIM SP_GERAR_ORDEM_PRODUCAOResult

        public PRODUTOS_TAB_OPERACOE ObterTabelaOperacao(string tabelaOperacoes)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.PRODUTOS_TAB_OPERACOEs
                    where m.TABELA_OPERACOES == tabelaOperacoes
                    select m).SingleOrDefault();
        }
        public PRODUTOS_TAB_OPERACOE ObterTabelaOperacoesRotas(string tabelaOperacoes)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.PRODUTOS_TAB_OPERACOEs
                    where m.TABELA_OPERACOES == tabelaOperacoes
                    select m).SingleOrDefault();
        }
        public PRODUCAO_RECURSO ObterRecursoProdutivo(string nomeCliFor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            var r = (from m in db.PRODUCAO_RECURSOs
                     where m.NOME_CLIFOR.Trim() == nomeCliFor.Trim()
                     select m).ToList();

            return r.FirstOrDefault();
        }
        public PRODUCAO_FASE ObterFase(string faseProducao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.PRODUCAO_FASEs
                    where m.FASE_PRODUCAO.Trim() == faseProducao.Trim()
                    select m).SingleOrDefault();
        }

        //SP_OBTER_FACCAO_RESUMOResult
        public List<SP_OBTER_FACCAO_RESUMOResult> ObterFaccaoResumo(string colecao, int? hb, char? mostruario, string fornecedor, int? prod_servico, string status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_RESUMO(colecao, hb, mostruario, fornecedor, prod_servico, status)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_RESUMOResult

        //SP_OBTER_FACCAO_GRUPO_PRODUTOResult
        public List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> ObterFaccaoGrupo(string grupoProduto, string colecao, int? prod_servico, string fornecedor, string fornecedorSub, char? mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_GRUPO_PRODUTO(grupoProduto, colecao, prod_servico, fornecedor, fornecedorSub, mostruario, 1)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_GRUPO_PRODUTOResult

        //SP_OBTER_FACCAO_PERIODOResult
        public List<SP_OBTER_FACCAO_PERIODOResult> ObterFaccaoPeriodo(DateTime dataInicial, DateTime dataFinal, string colecaoIntra, string griffe, string grupoProduto, int prodServico, string fornecedor, string fornecedorSub, char? mostruario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_FACCAO_PERIODO(dataInicial, dataFinal, colecaoIntra, griffe, grupoProduto, prodServico, fornecedor, fornecedorSub, mostruario) select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_PERIODOResult

        //SP_OBTER_FACCAO_TIMEResult
        public List<SP_OBTER_FACCAO_TIMEResult> ObterFaccaoTime(string colecao, int? hb, char? mostruario, string fornecedor, string fornecedorSub, int? prod_servico, string status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.SP_OBTER_FACCAO_TIME(colecao, hb, mostruario, fornecedor, fornecedorSub, prod_servico, status)
                    select m).ToList();
        }
        public List<SP_OBTER_FACCAO_TIMEV2Result> ObterFaccaoTimeV2(string colecao, int? hb, char? mostruario, string fornecedor, string fornecedorSub, int? prod_servico, string status)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from m in dbIntra.SP_OBTER_FACCAO_TIMEV2(colecao, hb, mostruario, fornecedor, fornecedorSub, prod_servico, status)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_TIMEResult



        //SP_OBTER_FACCAO_TIME_ENCAIXEResult
        public List<SP_OBTER_FACCAO_TIME_ENCAIXEResult> ObterFaccaoTimeEncaixe(string colecao, int? hb, char? mostruario, string fornecedor, int? prod_servico, int? prod_processo, string status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_TIME_ENCAIXE(colecao, hb, mostruario, fornecedor, prod_servico, prod_processo, status)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_TIME_ENCAIXEResult

        public List<SP_OBTER_HB_CORTE_BAIXA_HOJEResult> ObterHBCorteHoje()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_HB_CORTE_BAIXA_HOJE()
                    select m).ToList();
        }

        //SP_OBTER_FACCAO_HISTORICOResult
        public List<SP_OBTER_FACCAO_HISTORICOResult> ObterFaccaoHistorico(string colecao, int hb, char mostruario, int prod_processo, int prod_servico)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from m in dbIntra.SP_OBTER_FACCAO_HISTORICO(colecao, hb, mostruario, prod_processo, prod_servico)
                    select m).ToList();
        }
        public List<SP_OBTER_FACCAO_HISTORICO_OUTROSResult> ObterFaccaoHistoricoOutros(string colecao, int hb, char mostruario, int prod_processo, int prod_servico)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from m in dbIntra.SP_OBTER_FACCAO_HISTORICO_OUTROS(colecao, hb, mostruario, prod_processo, prod_servico)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_HISTORICOResult

        //SP_OBTER_FACCAO_STATUSResult
        public List<SP_OBTER_FACCAO_STATUSResult> ObterFaccaoStatus(string fornecedor, string fornecedorSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_STATUS(fornecedor, fornecedorSub, 1)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_STATUSResult

        //SP_OBTER_FACCAO_STATUS_DETALHEResult
        public List<SP_OBTER_FACCAO_STATUS_DETALHEResult> ObterFaccaoStatusDetalhe(string fornecedor, string fornecedorSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_STATUS_DETALHE(fornecedor, fornecedorSub, 1)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_STATUS_DETALHEResult

        public List<PROD_HB_FACCAO_PROBLEMA> ObterFaccaoProblema()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_FACCAO_PROBLEMAs
                    orderby m.PROBLEMA
                    select m).ToList();
        }


        public List<PROD_HB_SAIDA> ObterSaidaFornecedor(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            var retorno = (from m in db.PROD_HB_SAIDAs
                           where
                                m.PROD_PROCESSO == 20
                            && m.SALDO == 'S'
                            && m.FORNECEDOR != null
                            && (m.PROD_SERVICO == 1 || m.PROD_SERVICO == 6)
                            && m.PROD_HB == prod_hb
                           orderby m.FORNECEDOR
                           select m).ToList().GroupBy(p => new { PROD_HB = p.PROD_HB, CNPJ = p.CNPJ, FORNECEDOR = p.FORNECEDOR, FORNECEDOR_SUB = p.FORNECEDOR_SUB }).Select(g => new PROD_HB_SAIDA
                           {
                               PROD_HB = g.Key.PROD_HB,
                               CNPJ = g.Key.CNPJ,
                               FORNECEDOR = g.Key.FORNECEDOR,
                               FORNECEDOR_SUB = g.Key.FORNECEDOR_SUB,
                               GRADE_TOTAL = g.Sum(x => x.GRADE_TOTAL)
                           }).OrderBy(p => p.FORNECEDOR).ToList();
            return retorno;
        }

        public List<SP_OBTER_FACCAO_FORN_ENTResult> ObterEntradaFornecedor(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.SP_OBTER_FACCAO_FORN_ENT(prod_hb) select i).ToList();

        }

        //PROD_HB_SAIDA_VOLUME
        public List<PROD_HB_SAIDA_VOLUME> ObterVolumeSaida(int prod_hb_saida)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_SAIDA_VOLUMEs
                    where m.PROD_HB_SAIDA == prod_hb_saida
                    select m).ToList();
        }
        public void InserirVolumeSaida(PROD_HB_SAIDA_VOLUME _volumeSaida)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_SAIDA_VOLUMEs.InsertOnSubmit(_volumeSaida);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PROD_HB_SAIDA_VOLUME

        public List<SP_OBTER_FACCAO_ENTREGAResult> ObterFaccaoEntrega(string colecao, int? hb, char? mostruario, string fornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_FACCAO_ENTREGA(colecao, hb, mostruario, fornecedor) select i).ToList();
        }

        public List<SP_OBTER_FACCAO_ATAC_VAR_FALResult> ObterFaccaoAtacadoVarejoFaltante(string colecao, int? hb, char? mostruario, int? prod_servico, string fornecedor, int? faccaoProblema)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_FACCAO_ATAC_VAR_FAL(colecao, hb, mostruario, prod_servico, "", fornecedor, faccaoProblema) select i).ToList();
        }

        //SP_OBTER_FACCAO_CUSTOResult
        public List<SP_OBTER_FACCAO_CUSTOResult> ObterFaccaoCusto(string colecao, int? hb, string produto, string nome, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_CUSTO(colecao, hb, produto, nome, mostruario)
                    select m).ToList();
        }
        // FIM SP_OBTER_FACCAO_STATUSResult
        public List<SP_OBTER_FACCAO_ANALISE_DESEMPENHOResult> ObterFaccaoAnaliseDesempenho(string fornecedor, string colecao, char? mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            var conn = db.Connection;

            var p = new DynamicParameters();
            p.Add("@P_FORNECEDOR", fornecedor);
            p.Add("@P_COLECAO", colecao);
            p.Add("@P_MOSTRUARIO", mostruario);

            var desempenho = conn.Query<SP_OBTER_FACCAO_ANALISE_DESEMPENHOResult>("SP_OBTER_FACCAO_ANALISE_DESEMPENHO", p,
        commandType: CommandType.StoredProcedure).ToList();

            return desempenho;

        }
        // FIM SP_OBTER_FACCAO_STATUSResult

        public List<SP_OBTER_FACCAO_DIARIOResult> ObterFaccaoDiario(DateTime data, int prodServico, char mostruario, int query)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            var conn = db.Connection;

            var p = new DynamicParameters();
            p.Add("@P_DATA", data);
            p.Add("@P_PROD_SERVICO", prodServico);
            p.Add("@P_MOSTRUARIO", mostruario);
            p.Add("@P_QUERY", query);

            var faccaoDiario = conn.Query<SP_OBTER_FACCAO_DIARIOResult>("SP_OBTER_FACCAO_DIARIO", p,
        commandType: CommandType.StoredProcedure).ToList();

            return faccaoDiario;

            //return (from m in db.SP_OBTER_FACCAO_ANALISE_DESEMPENHO(fornecedor, colecao, mostruario) select m).ToList();
        }

        public List<SP_OBTER_FACCAO_PRODUCAO_DIARIOResult> ObterFaccaoProducaoDiario(DateTime data, string colecao, int codigoServico, int hb)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            var conn = dbIntra.Connection;

            var p = new DynamicParameters();
            p.Add("@V_DATA", data);
            p.Add("@V_COLECAO", colecao);
            p.Add("@V_COD_SERVICO", codigoServico);
            p.Add("@V_HB", hb);

            var faccaoDiario = conn.Query<SP_OBTER_FACCAO_PRODUCAO_DIARIOResult>("SP_OBTER_FACCAO_PRODUCAO_DIARIO", p,
        commandType: CommandType.StoredProcedure).ToList();

            return faccaoDiario;

            //return (from m in db.SP_OBTER_FACCAO_ANALISE_DESEMPENHO(fornecedor, colecao, mostruario) select m).ToList();
        }


        public List<SP_OBTER_FACCAO_DIARIO_ATACVARResult> ObterFaccaoDiarioAtacadoVarejo(DateTime data, int prodServico, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            var conn = db.Connection;

            var p = new DynamicParameters();
            p.Add("@P_DATA", data);
            p.Add("@P_PROD_SERVICO", prodServico);
            p.Add("@P_MOSTRUARIO", mostruario);

            var faccaoDiario = conn.Query<SP_OBTER_FACCAO_DIARIO_ATACVARResult>("SP_OBTER_FACCAO_DIARIO_ATACVAR", p,
        commandType: CommandType.StoredProcedure).ToList();

            return faccaoDiario;

        }

        public List<SP_OBTER_FACCAO_DIARIO_ATACVAR_RECEBResult> ObterAcabamentoDiarioAtacadoVarejoReceb(DateTime data, int prodServico, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            var conn = db.Connection;

            var p = new DynamicParameters();
            p.Add("@P_DATA", data);
            p.Add("@P_PROD_SERVICO", prodServico);
            p.Add("@P_MOSTRUARIO", mostruario);

            var faccaoDiario = conn.Query<SP_OBTER_FACCAO_DIARIO_ATACVAR_RECEBResult>("SP_OBTER_FACCAO_DIARIO_ATACVAR_RECEB", p,
        commandType: CommandType.StoredProcedure).ToList();

            return faccaoDiario;

        }


        /*****************************************************/
        /*****************************************************/
        /*****************************************************/
        /* FACÇÃO ADMINISTRAÇÃO                              */
        /*****************************************************/
        /*****************************************************/
        /*****************************************************/
        /*****************************************************/

        //FACC_TIPO_DOCUMENTO
        public List<FACC_TIPO_DOCUMENTO> ObterTipoDocumento()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_TIPO_DOCUMENTOs
                    orderby m.CODIGO
                    select m).ToList();
        }
        public FACC_TIPO_DOCUMENTO ObterTipoDocumento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_TIPO_DOCUMENTOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirTipoDocumento(FACC_TIPO_DOCUMENTO _tpDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FACC_TIPO_DOCUMENTOs.InsertOnSubmit(_tpDocumento);
                db.SubmitChanges();

                return _tpDocumento.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarTipoDocumento(FACC_TIPO_DOCUMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FACC_TIPO_DOCUMENTO _tipoDocumento = ObterTipoDocumento(_novo.CODIGO);

            if (_novo != null)
            {
                _tipoDocumento.CODIGO = _novo.CODIGO;
                _tipoDocumento.DOCUMENTO = _novo.DOCUMENTO;
                _tipoDocumento.DESCRICAO = _novo.DESCRICAO;
                _tipoDocumento.TIPO_VIGENCIA = _novo.TIPO_VIGENCIA;
                _tipoDocumento.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _tipoDocumento.STATUS = _novo.STATUS;

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

        //FACC_FACCAO_TIPO_DOCUMENTO
        public FACC_FACCAO_TIPO_DOCUMENTO ObterFaccaoTipoDocumento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_FACCAO_TIPO_DOCUMENTOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public FACC_FACCAO_TIPO_DOCUMENTO ObterFaccaoTipoDocumento(string nomeCliFor, int codigoTipoDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_FACCAO_TIPO_DOCUMENTOs
                    where m.NOME_CLIFOR.Trim() == nomeCliFor.Trim() &&
                        m.FACC_TIPO_DOCUMENTO == codigoTipoDocumento
                    select m).SingleOrDefault();
        }
        public int InserirFaccaoTipoDocumento(FACC_FACCAO_TIPO_DOCUMENTO _faccTipoDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FACC_FACCAO_TIPO_DOCUMENTOs.InsertOnSubmit(_faccTipoDocumento);
                db.SubmitChanges();

                return _faccTipoDocumento.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFaccaoTipoDocumento(FACC_FACCAO_TIPO_DOCUMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FACC_FACCAO_TIPO_DOCUMENTO _faccTipoDocumento = ObterFaccaoTipoDocumento(_novo.CODIGO);

            if (_novo != null)
            {
                _faccTipoDocumento.CODIGO = _novo.CODIGO;
                _faccTipoDocumento.NOME_CLIFOR = _novo.NOME_CLIFOR;
                _faccTipoDocumento.FACC_TIPO_DOCUMENTO = _novo.FACC_TIPO_DOCUMENTO;
                _faccTipoDocumento.OBRIGATORIO = _novo.OBRIGATORIO;

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
        public void ExcluirFaccaoTipoDocumento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                FACC_FACCAO_TIPO_DOCUMENTO _faccTipoDocumento = ObterFaccaoTipoDocumento(codigo);
                if (_faccTipoDocumento != null)
                {
                    db.FACC_FACCAO_TIPO_DOCUMENTOs.DeleteOnSubmit(_faccTipoDocumento);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //SP_OBTER_FACCAO_CADASTROResult
        public List<SP_OBTER_FACCAO_CADASTROResult> ObterFaccaoCadastro(string nomeCliFor, string cgcCPF)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_CADASTRO(nomeCliFor, cgcCPF)
                    select m).ToList();
        }

        //SP_OBTER_FACCAO_CADASTRO_DOCResult
        public List<SP_OBTER_FACCAO_CADASTRO_DOCResult> ObterFaccaoCadastroDoc(string nomeCliFor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_CADASTRO_DOC(nomeCliFor)
                    select m).ToList();
        }

        //FACC_SOCIO
        public List<FACC_SOCIO> ObterFaccaoSocio()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_SOCIOs
                    orderby m.NOME
                    select m).ToList();
        }
        public List<FACC_SOCIO> ObterFaccaoSocio(string nome_cliFor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_SOCIOs
                    where m.NOME_CLIFOR.Trim() == nome_cliFor.Trim()
                    select m).ToList();
        }
        public FACC_SOCIO ObterFaccaoSocio(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FACC_SOCIOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirFaccaoSocio(FACC_SOCIO _faccaoSocio)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FACC_SOCIOs.InsertOnSubmit(_faccaoSocio);
                db.SubmitChanges();

                return _faccaoSocio.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFaccaoSocio(FACC_SOCIO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FACC_SOCIO _faccaoSocio = ObterFaccaoSocio(_novo.CODIGO);

            if (_novo != null)
            {
                _faccaoSocio.CODIGO = _novo.CODIGO;
                _faccaoSocio.NOME_CLIFOR = _novo.NOME_CLIFOR;
                _faccaoSocio.NOME = _novo.NOME;
                _faccaoSocio.CNPJCPF = _novo.CNPJCPF;
                _faccaoSocio.TELEFONE = _novo.TELEFONE;
                _faccaoSocio.EMAIL = _novo.EMAIL;
                _faccaoSocio.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _faccaoSocio.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _faccaoSocio.STATUS = _novo.STATUS;

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
        public void ExcluirFaccaoSocio(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                FACC_SOCIO _faccaoSocio = ObterFaccaoSocio(codigo);
                if (_faccaoSocio != null)
                {
                    db.FACC_SOCIOs.DeleteOnSubmit(_faccaoSocio);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //FACC_FACCAO_DOCUMENTO
        public List<FACC_FACCAO_DOCUMENTO> ObterFaccaoDocumento(string nomeCliFor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_FACCAO_DOCUMENTOs
                    where m.NOME_CLIFOR.ToString() == nomeCliFor
                    orderby m.FACC_TIPO_DOCUMENTO1.DESCRICAO
                    select m).ToList();
        }
        public List<FACC_FACCAO_DOCUMENTO> ObterFaccaoDocumento(string nomeCliFor, int faccTipoDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.FACC_FACCAO_DOCUMENTOs
                    where m.NOME_CLIFOR.Trim() == nomeCliFor.Trim() && m.FACC_TIPO_DOCUMENTO == faccTipoDocumento
                    select m).ToList();
        }
        public FACC_FACCAO_DOCUMENTO ObterFaccaoDocumento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.FACC_FACCAO_DOCUMENTOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirFaccaoDocumento(FACC_FACCAO_DOCUMENTO _faccTipoDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FACC_FACCAO_DOCUMENTOs.InsertOnSubmit(_faccTipoDocumento);
                db.SubmitChanges();

                return _faccTipoDocumento.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFaccaoDocumento(FACC_FACCAO_DOCUMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FACC_FACCAO_DOCUMENTO _faccaoSocio = ObterFaccaoDocumento(_novo.CODIGO);

            if (_novo != null)
            {
                _faccaoSocio.CODIGO = _novo.CODIGO;
                _faccaoSocio.NOME_CLIFOR = _novo.NOME_CLIFOR;
                _faccaoSocio.FACC_TIPO_DOCUMENTO = _novo.FACC_TIPO_DOCUMENTO;
                _faccaoSocio.DATA_VIGENCIA_INI = _novo.DATA_VIGENCIA_INI;
                _faccaoSocio.DATA_VIGENCIA_FIM = _novo.DATA_VIGENCIA_FIM;
                _faccaoSocio.DOCUMENTO = _novo.DOCUMENTO;
                _faccaoSocio.OBS = _novo.OBS;
                _faccaoSocio.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _faccaoSocio.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _faccaoSocio.STATUS = _novo.STATUS;

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
        public void ExcluirFaccaoDocumento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                FACC_FACCAO_DOCUMENTO _faccTipoDocumento = ObterFaccaoDocumento(codigo);
                if (_faccTipoDocumento != null)
                {
                    db.FACC_FACCAO_DOCUMENTOs.DeleteOnSubmit(_faccTipoDocumento);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_FACCAO_DOCUMENTOResult> ObterFaccaoDocumento(string nomeCliFor, int? codigoTipoDocumento, DateTime? vigenciaIni, DateTime? viagenciaFim, char? docFaltante)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_DOCUMENTO(nomeCliFor, codigoTipoDocumento, vigenciaIni, viagenciaFim, docFaltante)
                    select m).ToList();
        }

        public List<SP_OBTER_FACCAO_ANALISE_MENSALResult> ObterFaccaoAnaliseMensal(string fornecedor, DateTime dataIni, DateTime dataFim, string colecao, char? mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_ANALISE_MENSAL(fornecedor, dataIni, dataFim, colecao, mostruario)
                    select m).ToList();
        }

        public List<SP_OBTER_FACCAO_ANALISE_VOLUMEResult> ObterFaccaoAnaliseVolume(string fornecedor, string colecao, char? mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_ANALISE_VOLUME(fornecedor, colecao, mostruario)
                    select m).ToList();
        }

        public List<SP_OBTER_FACCAO_ANALISE_ENTREGAResult> ObterFaccaoAnaliseEntrega(string fornecedor, string colecao, char? mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_FACCAO_ANALISE_ENTREGA(fornecedor, colecao, mostruario)
                    select m).ToList();
        }

        public SP_OBTER_FACCAO_QTDE_FUNResult ObterFaccaoQtdeFun(string fornecedor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.SP_OBTER_FACCAO_QTDE_FUN(fornecedor)
                    select m).FirstOrDefault();
        }

        public SP_OBTER_FACCAO_AVALResult ObterFaccaoAvaliacao(string colecao, int? hb, string produto, string fornecedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_FACCAO_AVAL(colecao, hb, produto, fornecedor)
                    select m).FirstOrDefault();
        }

        public List<SP_OBTER_DETALHES_HB_FALTANTEResult> ObterHBLiberado(String Colecao, int HB, String Fornecedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_DETALHES_HB_FALTANTE(Colecao, HB, Fornecedor)
                    select m).ToList();
        }

        public PROD_HB_LIBERADO ObterHBLiberado(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_LIBERADOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }

        public int InserirHBLiberado(PROD_HB_LIBERADO _hbLiberado)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.PROD_HB_LIBERADOs.InsertOnSubmit(_hbLiberado);
                dbIntra.SubmitChanges();

                return _hbLiberado.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarHBLiberado(PROD_HB_LIBERADO _hbLiberadoNEW)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            PROD_HB_LIBERADO _hbLiberadoOLD = ObterHBLiberado(_hbLiberadoNEW.CODIGO);

            if (_hbLiberadoOLD != null)
            {
                _hbLiberadoOLD.VERIFICADO = _hbLiberadoNEW.VERIFICADO;
                _hbLiberadoOLD.DATA_VERIFICADO = _hbLiberadoNEW.DATA_VERIFICADO;

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

        public List<SP_OBTER_FACCAO_AVALResult> ObterFaccaoAvaliacaoRel(string colecao, int? hb, string produto, string fornecedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_FACCAO_AVAL(colecao, hb, produto, fornecedor)
                    select m).ToList();
        }
        public List<PROD_HB_AVALIACAO> ObterFaccaoAvaliacaoHB(int prodHB)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_AVALIACAOs
                    where m.PROD_HB == prodHB
                    select m).ToList();
        }
        public PROD_HB_AVALIACAO ObterFaccaoAvaliacao(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_AVALIACAOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirFaccaoAvaliacao(PROD_HB_AVALIACAO _prod_hb_aval)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.PROD_HB_AVALIACAOs.InsertOnSubmit(_prod_hb_aval);
                dbIntra.SubmitChanges();

                return _prod_hb_aval.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFaccaoAvaliacao(PROD_HB_AVALIACAO _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            PROD_HB_AVALIACAO _prod_hb_aval = ObterFaccaoAvaliacao(_novo.CODIGO);

            if (_novo != null)
            {
                _prod_hb_aval.CODIGO = _novo.CODIGO;
                _prod_hb_aval.PROD_HB = _novo.PROD_HB;
                _prod_hb_aval.FORNECEDOR = _novo.FORNECEDOR;
                _prod_hb_aval.DATA_AVALIACAO = _novo.DATA_AVALIACAO;
                _prod_hb_aval.USUARIO_AVALIACAO = _novo.USUARIO_AVALIACAO;
                _prod_hb_aval.NOTA1 = _novo.NOTA1;
                _prod_hb_aval.OBS1 = _novo.OBS1;
                _prod_hb_aval.NOTA2 = _novo.NOTA2;
                _prod_hb_aval.OBS2 = _novo.OBS2;

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

    }
}

