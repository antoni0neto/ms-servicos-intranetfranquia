using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class EstoqueController
    {
        BaseController baseController = new BaseController();
        DCDataContext db;
        INTRADataContext dbIntra;
        LINXDataContext dbLinx;

        //OLD
        public List<EstoqueLoja> BuscaMovimentoProduto(string categoria, string colecao, string anoSemana, string griffe)
        {
            string dataInicioSemana = baseController.AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimSemana = baseController.AjustaData(baseController.BuscaDataFim(anoSemana));

            int count_venda = 0;
            int count_estoque = 0;
            int count_reposicao = 0;
            int count_novo = 0;
            int reposicao = 0;
            int novo = 0;

            List<EstoqueLoja> estoqueLoja = new List<EstoqueLoja>();

            List<Sp_Venda_Estoque_LojaResult> colecaoVendaEstoque = baseController.BuscaVendaEstoque(dataInicioSemana, dataFimSemana, colecao.Trim(), categoria.Trim(), griffe.Trim());

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            List<Sp_Compras_VarejoResult> compras = db.Sp_Compras_Varejo().ToList();

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            foreach (Sp_Venda_Estoque_LojaResult item in colecaoVendaEstoque)
            {
                EstoqueLoja el = new EstoqueLoja();

                el.Grupo = item.GRUPO;
                el.SubGrupo = item.SUBGRUPO;
                el.TipoProduto = item.TIPO;
                el.LinhaProduto = item.LINHA;

                el.Venda = Convert.ToInt32(item.VENDA).ToString("###,###,##0");
                el.Estoque = Convert.ToInt32(item.ESTOQUE).ToString("###,###,##0");

                Sp_Estoque_LojaResult sp_Estoque_LojaResult = new Sp_Estoque_LojaResult();

                sp_Estoque_LojaResult = db.Sp_Estoque_Loja(dataFimSemana, colecao.Trim(), categoria.Trim(), griffe.Trim(), item.GRUPO.Trim(), item.SUBGRUPO.Trim(), item.TIPO.Trim(), item.LINHA.Trim()).SingleOrDefault();

                if (sp_Estoque_LojaResult != null)
                {
                    if (Convert.ToInt32(item.ESTOQUE) > 0)
                    {
                        reposicao = Convert.ToInt32(sp_Estoque_LojaResult.ESTOQUE);
                        novo = 0;
                    }
                    else
                    {
                        novo = Convert.ToInt32(sp_Estoque_LojaResult.ESTOQUE);
                        reposicao = 0;
                    }
                }

                if (compras != null)
                {
                    foreach (Sp_Compras_VarejoResult itemCompras in compras)
                    {
                        if (item.GRUPO == itemCompras.GRUPO &
                            item.SUBGRUPO == itemCompras.SUBGRUPO &
                            item.TIPO == itemCompras.TIPO &
                            item.LINHA == itemCompras.LINHA)
                        {
                            el.Mar = Convert.ToInt32(itemCompras.QTDE_ORIGINAL).ToString("###,###,##0");

                            break;
                        }
                    }
                }

                el.Reposicao = reposicao.ToString("###,###,##0");
                el.Novo = novo.ToString("###,###,##0");
                el.Ern = (Convert.ToInt32(item.ESTOQUE) + reposicao + novo).ToString("###,###,##0");

                estoqueLoja.Add(el);

                count_venda += Convert.ToInt32(item.VENDA);
                count_estoque += Convert.ToInt32(item.ESTOQUE);
                count_reposicao += reposicao;
                count_novo += novo;
            }

            if (colecaoVendaEstoque.Count() > 0)
            {
                EstoqueLoja el = new EstoqueLoja();

                el.SubGrupo = "Total";
                el.TipoProduto = "Total";
                el.LinhaProduto = "Total";
                el.Venda = count_venda.ToString("###,###,##0");
                el.Estoque = count_estoque.ToString("###,###,##0");
                el.Reposicao = count_reposicao.ToString("###,###,##0");
                el.Novo = count_novo.ToString("###,###,##0");
                el.Ern = (count_estoque + count_reposicao + count_novo).ToString("###,###,##0");

                estoqueLoja.Add(el);
            }

            return estoqueLoja;
        }

        //NOTA FISCAL MERCADORIA DE ENTRADA
        public List<SP_OBTER_NF_MERCADORIA_LOJAResult> ObterNFMercadoriaLoja(string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from d in db.SP_OBTER_NF_MERCADORIA_LOJA(filial) select d).ToList();
        }
        public List<SP_OBTER_NF_MERCADORIA_LOJA_PRODUTOResult> ObterNFMercadoriaLojaProduto(string filial, string romaneio)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from d in db.SP_OBTER_NF_MERCADORIA_LOJA_PRODUTO(filial, romaneio) select d).ToList();
        }
        // FIM NOTA FISCAL MERCADORIA DE ENTRADA

        //ESTOQUE_LOJA_RECEBIMENTO
        public List<ESTOQUE_LOJA_NF_RECEB> ObterEstoqueLojaReceb()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_NF_RECEBs select j).ToList();
        }
        public ESTOQUE_LOJA_NF_RECEB ObterEstoqueLojaReceb(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_NF_RECEBs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public List<ESTOQUE_LOJA_NF_RECEB> ObterEstoqueLojaReceb(string filial, string romaneio, string nf_transferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_NF_RECEBs
                    where j.FILIAL == filial &&
                            j.ROMANEIO_PRODUTO == romaneio &&
                            j.NUMERO_NF_TRANSFERENCIA == nf_transferencia
                    select j).ToList();
        }
        public int InserirEstoqueLojaReceb(ESTOQUE_LOJA_NF_RECEB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.ESTOQUE_LOJA_NF_RECEBs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEstoqueLojaReceb(ESTOQUE_LOJA_NF_RECEB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            ESTOQUE_LOJA_NF_RECEB _estoqueLoja = ObterEstoqueLojaReceb(_novo.CODIGO);

            if (_novo != null)
            {
                _estoqueLoja.CODIGO = _novo.CODIGO;

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
        public void ExcluirEstoqueLojaReceb(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                ESTOQUE_LOJA_NF_RECEB _estoqueLoja = ObterEstoqueLojaReceb(codigo);
                if (_estoqueLoja != null)
                {
                    db.ESTOQUE_LOJA_NF_RECEBs.DeleteOnSubmit(_estoqueLoja);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // FIM ESTOQUE_LOJA_RECEBIMENTO

        //ESTOQUE_LOJA_NF_RECEB_PRODUTO
        public List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> ObterEstoqueLojaRecebProduto()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_NF_RECEB_PRODUTOs select j).ToList();
        }
        public List<ESTOQUE_LOJA_NF_RECEB_PRODUTO> ObterEstoqueLojaRecebProdutoPorNota(string filial, string romaneio)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_NF_RECEB_PRODUTOs
                    join g in db.ESTOQUE_LOJA_NF_RECEBs
                    on j.ESTOQUE_LOJA_NF_RECEB equals g.CODIGO
                    where g.FILIAL.Trim() == filial && g.ROMANEIO_PRODUTO.Trim() == romaneio
                    orderby j.PRODUTO, j.COR_PRODUTO
                    select j).ToList();
        }
        public ESTOQUE_LOJA_NF_RECEB_PRODUTO ObterEstoqueLojaRecebProduto(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.ESTOQUE_LOJA_NF_RECEB_PRODUTOs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public int InserirEstoqueLojaRecebProduto(ESTOQUE_LOJA_NF_RECEB_PRODUTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.ESTOQUE_LOJA_NF_RECEB_PRODUTOs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEstoqueLojaRecebProduto(ESTOQUE_LOJA_NF_RECEB_PRODUTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            ESTOQUE_LOJA_NF_RECEB_PRODUTO _estoqueLojaProduto = ObterEstoqueLojaRecebProduto(_novo.CODIGO);

            if (_estoqueLojaProduto != null)
            {
                _estoqueLojaProduto.CODIGO = _novo.CODIGO;
                _estoqueLojaProduto.STATUS_CONFERENCIA = _novo.STATUS_CONFERENCIA;
                _estoqueLojaProduto.OBSERVACAO = (_novo.OBSERVACAO == null) ? "" : _novo.OBSERVACAO;
                _estoqueLojaProduto.CONFERIDO = _novo.CONFERIDO;

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
        public void ExcluirEstoqueLojaRecebProduto(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                ESTOQUE_LOJA_NF_RECEB_PRODUTO _estoqueLojaProduto = ObterEstoqueLojaRecebProduto(codigo);
                if (_estoqueLojaProduto != null)
                {
                    db.ESTOQUE_LOJA_NF_RECEB_PRODUTOs.DeleteOnSubmit(_estoqueLojaProduto);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // FIM ESTOQUE_LOJA_RECEBIMENTO

        //OBTER NOTAS
        public List<SP_OBTER_PRODUTO_RECEBIDO_LOJAResult> ObterProdutoRecebidoLoja(string codigoProduto, string numeroNota)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from j in db.SP_OBTER_PRODUTO_RECEBIDO_LOJA(codigoProduto, numeroNota) select j).ToList();
        }
        public List<SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTAResult> ObterProdutoRecebidoLojaNota(string produto, string corProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from j in db.SP_OBTER_PRODUTO_RECEBIDO_LOJA_NOTA(produto, corProduto) select j).ToList();
        }
        public List<SP_GERAR_RELATORIO_MERCADORIAResult> GerarRelatorioMercadoria(string produto, string filial, string notafiscal, DateTime? emissao_ini, DateTime? emissao_fim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from j in db.SP_GERAR_RELATORIO_MERCADORIA(produto, filial, notafiscal, emissao_ini, emissao_fim) select j).ToList();
        }
        //FIM OBTER NOTAS

        // SP_OBTER_GRADE_LOJA_ENTRADA
        public SP_OBTER_GRADE_LOJA_ENTRADAResult ObterGradeLojaEntradaLinha(string produto, string corProduto, string filial, string romaneio_produto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.SP_OBTER_GRADE_LOJA_ENTRADA(produto, corProduto, filial, romaneio_produto) select j).SingleOrDefault();
        }
        // FIM - SP_OBTER_GRADE_LOJA_ENTRADA

        public CTB_NF_ENTRADA_BAIXA ObterNFEntrada(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.CTB_NF_ENTRADA_BAIXAs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public CTB_NF_ENTRADA_BAIXA ObterNFEntrada(string nomeClifor, string nfEntrada, string serieNfEntrada, DateTime recebimento)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.CTB_NF_ENTRADA_BAIXAs
                    where j.NOME_CLIFOR == nomeClifor
                    && j.NF_ENTRADA == nfEntrada
                    && j.SERIE_NF_ENTRADA == serieNfEntrada
                    && j.RECEBIMENTO == recebimento
                    select j).SingleOrDefault();
        }
        public void InserirNFEntrada(CTB_NF_ENTRADA_BAIXA nfEntradaBaixa)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.CTB_NF_ENTRADA_BAIXAs.InsertOnSubmit(nfEntradaBaixa);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirNFEntrada(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                CTB_NF_ENTRADA_BAIXA nfEntradaBaixa = ObterNFEntrada(codigo);
                if (nfEntradaBaixa != null)
                {
                    dbIntra.CTB_NF_ENTRADA_BAIXAs.DeleteOnSubmit(nfEntradaBaixa);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SP_OBTER_NF_PRODUTO_ACABADOResult> ObterNFEntradaProdutoAcabado(DateTime dataIni, DateTime dataFim, string filial, string nomeClifor)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from j in dbLinx.SP_OBTER_NF_PRODUTO_ACABADO(dataIni, dataFim, filial, nomeClifor) select j).ToList();
        }


        //NOTA FISCAL MERCADORIA DE FABRICA
        public List<SP_OBTER_NF_MERCADORIA_FABResult> ObterNFMercadoriaFabrica(string filial)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_OBTER_NF_MERCADORIA_FAB(filial) select d).ToList();
        }
        public List<SP_OBTER_NF_MERCADORIA_FAB_PRODUTOResult> ObterNFMercadoriaFabricaProduto(string filial, string nfSaida, string serieNf)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_OBTER_NF_MERCADORIA_FAB_PRODUTO(filial, nfSaida, serieNf) select d).ToList();
        }
        public List<SP_GERAR_RELATORIO_MERCADORIA_FABResult> GerarRelatorioMercadoriaFabrica(string produto, string filial, string notafiscal, DateTime? emissao_ini, DateTime? emissao_fim)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_GERAR_RELATORIO_MERCADORIA_FAB(produto, filial, notafiscal, emissao_ini, emissao_fim) select j).ToList();
        }
        // FIM NOTA FISCAL MERCADORIA DE FABRICA

        //ESTOQUE_ FABRICA
        public List<ESTOQUE_FAB_NF_RECEB> ObterEstoqueFabricaReceb()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.ESTOQUE_FAB_NF_RECEBs select j).ToList();
        }
        public ESTOQUE_FAB_NF_RECEB ObterEstoqueFabricaReceb(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.ESTOQUE_FAB_NF_RECEBs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public List<ESTOQUE_FAB_NF_RECEB> ObterEstoqueFabricaReceb(string filial, string nfSaida, string serieNf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.ESTOQUE_FAB_NF_RECEBs
                    where j.FILIAL == filial &&
                            j.NF_SAIDA == nfSaida &&
                            j.SERIE_NF == serieNf
                    select j).ToList();
        }
        public int InserirEstoqueFabricaReceb(ESTOQUE_FAB_NF_RECEB _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.ESTOQUE_FAB_NF_RECEBs.InsertOnSubmit(_novo);
                dbIntra.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEstoqueFabricaReceb(ESTOQUE_FAB_NF_RECEB _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            ESTOQUE_FAB_NF_RECEB _estoqueLoja = ObterEstoqueFabricaReceb(_novo.CODIGO);

            if (_novo != null)
            {
                _estoqueLoja.CODIGO = _novo.CODIGO;

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
        public void ExcluirEstoqueFabricaReceb(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ESTOQUE_FAB_NF_RECEB _estoqueLoja = ObterEstoqueFabricaReceb(codigo);
                if (_estoqueLoja != null)
                {
                    dbIntra.ESTOQUE_FAB_NF_RECEBs.DeleteOnSubmit(_estoqueLoja);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // FIM ESTOQUE_LOJA_RECEBIMENTO

        //ESTOQUE_ FABRICA PRODUTO
        public List<ESTOQUE_FAB_NF_RECEB_PRODUTO> ObterEstoqueFabricaRecebProduto()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.ESTOQUE_FAB_NF_RECEB_PRODUTOs select j).ToList();
        }
        public List<ESTOQUE_FAB_NF_RECEB_PRODUTO> ObterEstoqueFabricaRecebProdutoPorNota(string filial, string nfSaida, string serieNf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.ESTOQUE_FAB_NF_RECEB_PRODUTOs
                    join g in dbIntra.ESTOQUE_FAB_NF_RECEBs
                    on j.ESTOQUE_FAB_NF_RECEB equals g.CODIGO
                    where g.FILIAL.Trim() == filial && g.NF_SAIDA.Trim() == nfSaida && g.SERIE_NF == serieNf
                    orderby j.PRODUTO, j.COR_PRODUTO
                    select j).ToList();
        }
        public ESTOQUE_FAB_NF_RECEB_PRODUTO ObterEstoqueFabricaRecebProduto(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from j in dbIntra.ESTOQUE_FAB_NF_RECEB_PRODUTOs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public int InserirEstoqueFabricaRecebProduto(ESTOQUE_FAB_NF_RECEB_PRODUTO _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.ESTOQUE_FAB_NF_RECEB_PRODUTOs.InsertOnSubmit(_novo);
                dbIntra.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEstoqueFabricaRecebProduto(ESTOQUE_FAB_NF_RECEB_PRODUTO _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            ESTOQUE_LOJA_NF_RECEB_PRODUTO _estoqueLojaProduto = ObterEstoqueLojaRecebProduto(_novo.CODIGO);

            if (_estoqueLojaProduto != null)
            {
                _estoqueLojaProduto.CODIGO = _novo.CODIGO;
                _estoqueLojaProduto.STATUS_CONFERENCIA = _novo.STATUS_CONFERENCIA;
                _estoqueLojaProduto.OBSERVACAO = (_novo.OBSERVACAO == null) ? "" : _novo.OBSERVACAO;

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
        public void ExcluirEstoqueFabricaRecebProduto(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ESTOQUE_FAB_NF_RECEB_PRODUTO _estoqueLojaProduto = ObterEstoqueFabricaRecebProduto(codigo);
                if (_estoqueLojaProduto != null)
                {
                    dbIntra.ESTOQUE_FAB_NF_RECEB_PRODUTOs.DeleteOnSubmit(_estoqueLojaProduto);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // FIM ESTOQUE_LOJA_RECEBIMENTO



    }
}
