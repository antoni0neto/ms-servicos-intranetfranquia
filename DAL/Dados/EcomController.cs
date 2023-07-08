using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace DAL
{
    public class EcomController
    {
        //DCDataContext db;
        LINXDataContext LINXdb;
        INTRADataContext Intradb;

        public List<TBL_NATAL> ObterFotosNatal()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.TBL_NATALs select m).ToList();
        }

        public ECOM_PARAMETROS_API ObterParametroAPI()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PARAMETROS_APIs select m).SingleOrDefault();
        }
        public ECOM_CORREIOS_LOGIN ObterLoginCorreios()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_LOGINs select m).SingleOrDefault();
        }

        public void InserirParametroAPI(ECOM_PARAMETROS_API parametroAPI)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PARAMETROS_APIs.InsertOnSubmit(parametroAPI);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarParametroAPI(ECOM_PARAMETROS_API parametroAPI)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PARAMETROS_API _parametroAPI = ObterParametroAPI();

            if (parametroAPI != null)
            {

                _parametroAPI.CONTA_CONTABIL = parametroAPI.CONTA_CONTABIL;
                _parametroAPI.CTB_CONTA_CONTABIL = parametroAPI.CTB_CONTA_CONTABIL;
                _parametroAPI.CODIGO_FILIAL = parametroAPI.CODIGO_FILIAL;
                _parametroAPI.FILIAL = parametroAPI.FILIAL;
                _parametroAPI.RATEIO_CENTRO_CUSTO_ICR = parametroAPI.RATEIO_CENTRO_CUSTO_ICR;
                _parametroAPI.RATEIO_CENTRO_CUSTO_LTA = parametroAPI.RATEIO_CENTRO_CUSTO_LTA;
                _parametroAPI.RATEIO_CENTRO_CUSTO_IAC = parametroAPI.RATEIO_CENTRO_CUSTO_IAC;
                _parametroAPI.CONTA_PORTADORA_ICR = parametroAPI.CONTA_PORTADORA_ICR;
                _parametroAPI.CONTA_PORTADORA_LTA = parametroAPI.CONTA_PORTADORA_LTA;
                _parametroAPI.CONTA_PORTADORA_IAC = parametroAPI.CONTA_PORTADORA_IAC;
                _parametroAPI.TIPO_VENDA = parametroAPI.TIPO_VENDA;
                _parametroAPI.TIPO_DOCUMENTO = parametroAPI.TIPO_DOCUMENTO;
                _parametroAPI.TRANSPORTADORA = parametroAPI.TRANSPORTADORA;
                _parametroAPI.CODIGO_TAB_PRECO = parametroAPI.CODIGO_TAB_PRECO;
                _parametroAPI.COLECAO = parametroAPI.COLECAO;
                _parametroAPI.REPRESENTANTE = parametroAPI.REPRESENTANTE;
                _parametroAPI.APROVACAO = parametroAPI.APROVACAO;
                _parametroAPI.EMAIL_REMET = parametroAPI.EMAIL_REMET;
                _parametroAPI.SMPT = parametroAPI.SMPT;
                _parametroAPI.PORT = parametroAPI.PORT;
                _parametroAPI.USER_AUTH = parametroAPI.USER_AUTH;
                _parametroAPI.PASS_AUTH = parametroAPI.PASS_AUTH;
                _parametroAPI.CONFIGURA_EMAIL = parametroAPI.CONFIGURA_EMAIL;
                _parametroAPI.DATA_ALTERACAO = parametroAPI.DATA_ALTERACAO;
                _parametroAPI.VALOR_PEDIDO_APROVACAO = parametroAPI.VALOR_PEDIDO_APROVACAO;


                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ECOM_PRODUTO ObterMagentoProduto(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public ECOM_PRODUTO ObterMagentoProdutoId(int idProdutoMag)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs where m.ID_PRODUTO_MAG == idProdutoMag select m).SingleOrDefault();
        }
        public ECOM_PRODUTO ObterMagentoProdutoConfigPorSKU(string sku)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.SKU == sku
                    && m.ID_PRODUTO_MAG_CONFIG == 0
                    && m.ID_PRODUTO_MAG > 0
                    && m.HABILITADO == 'S'
                    && m.VISIBILIDADE == "4"
                    select m).SingleOrDefault();
        }
        public ECOM_PRODUTO ObterMagentoProdutoConfig(string produto, string cor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.PRODUTO == produto
                    && m.COR == cor
                    && m.ID_PRODUTO_MAG_CONFIG == 0
                    select m).SingleOrDefault();
        }
        public ECOM_PRODUTO ObterMagentoProdutoSimplesTamanho(int idMagentoConfig, string tamanho)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.ID_PRODUTO_MAG_CONFIG == idMagentoConfig
                    && m.TAMANHO == tamanho
                    select m).SingleOrDefault();
        }
        public ECOM_PRODUTO ObterMagentoProdutoPorcor(string produtoLinx, string cor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.PRODUTO == produtoLinx
                    && m.COR == cor
                    && m.ID_PRODUTO_MAG_CONFIG == 0
                    select m).SingleOrDefault();
        }
        public List<ECOM_PRODUTO> ObterMagentoTodosProdutos(string produto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.PRODUTO == produto
                    select m).ToList();
        }
        public List<ECOM_PRODUTO> ObterMagentoProdutoSimples(int idMagentoConfig, string produto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.ID_PRODUTO_MAG_CONFIG == idMagentoConfig
                    && m.PRODUTO == produto
                    select m).ToList();
        }
        public List<ECOM_PRODUTO> ObterMagentoProdutoAssociacao(string produtoLinx)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.PRODUTO == produtoLinx
                    && m.ID_PRODUTO_MAG_CONFIG != 0
                    select m).ToList();
        }
        public List<ECOM_PRODUTO> ObterMagentoProdutoAssociacaoConfig(string produtoLinx)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.PRODUTO == produtoLinx
                    && m.ID_PRODUTO_MAG_CONFIG == 0
                    select m).ToList();
        }
        public bool ValidarNomeProdutoRepetido(string produtoLinx, string nome)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            var produtoExiste = (from m in Intradb.ECOM_PRODUTOs
                                 where m.PRODUTO != produtoLinx
                                 && m.NOME_MAG.ToLower() == nome.ToLower()
                                 && m.ID_PRODUTO_MAG_CONFIG == 0
                                 select m).FirstOrDefault();

            if (produtoExiste == null)
                return true;

            return false;
        }

        public List<ECOM_PRODUTO> ObterMagentoProdutoSimplesCadastrado()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.ID_PRODUTO_MAG_CONFIG != 0
                    && m.ID_PRODUTO_MAG > 0
                    select m).ToList();
        }
        public List<ECOM_PRODUTO> ObterMagentoProdutoSimplesCadastrado(string colecao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTOs
                    where m.ID_PRODUTO_MAG_CONFIG != 0
                    && m.ID_PRODUTO_MAG > 0
                    && m.COLECAO == colecao
                    select m).ToList();
        }
        public List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_AUXResult> ObterMagentoProdutoConfigCadastrado(string colecao, string griffe, string grupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_RELACIONADO_AUX(colecao, griffe, grupoProduto) select m).ToList();
        }


        public int InserirMagentoProduto(ECOM_PRODUTO produtoMag)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PRODUTOs.InsertOnSubmit(produtoMag);
                Intradb.SubmitChanges();

                return produtoMag.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMagentoProduto(ECOM_PRODUTO produtoMag)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PRODUTO oldprodutoMag = ObterMagentoProduto(produtoMag.CODIGO);

            if (oldprodutoMag != null)
            {

                oldprodutoMag.ID_PRODUTO_MAG = produtoMag.ID_PRODUTO_MAG;
                oldprodutoMag.ID_PRODUTO_MAG_CONFIG = produtoMag.ID_PRODUTO_MAG_CONFIG;
                oldprodutoMag.COLECAO = produtoMag.COLECAO;
                oldprodutoMag.PRODUTO = produtoMag.PRODUTO;
                oldprodutoMag.NOME = produtoMag.NOME;
                oldprodutoMag.NOME_MAG = produtoMag.NOME_MAG;
                oldprodutoMag.PRODUTO_DESC = produtoMag.PRODUTO_DESC;
                oldprodutoMag.PRODUTO_DESC_CURTA = produtoMag.PRODUTO_DESC_CURTA;
                oldprodutoMag.SKU = produtoMag.SKU;
                oldprodutoMag.COR = produtoMag.COR;
                oldprodutoMag.TAMANHO = produtoMag.TAMANHO;
                oldprodutoMag.ECOM_COR = produtoMag.ECOM_COR;
                oldprodutoMag.PESO_KG = produtoMag.PESO_KG;
                oldprodutoMag.GRUPO_PRODUTO = produtoMag.GRUPO_PRODUTO;
                oldprodutoMag.ECOM_GRUPO_PRODUTO = produtoMag.ECOM_GRUPO_PRODUTO;
                oldprodutoMag.PRECO = produtoMag.PRECO;
                oldprodutoMag.PRECO_PROMO = produtoMag.PRECO_PROMO;
                oldprodutoMag.PRECO_PROMO_DATA_DE = produtoMag.PRECO_PROMO_DATA_DE;
                oldprodutoMag.PRECO_PROMO_DATA_ATE = produtoMag.PRECO_PROMO_DATA_ATE;
                oldprodutoMag.ECOM_GRUPO_MACRO = produtoMag.ECOM_GRUPO_MACRO;
                oldprodutoMag.ECOM_TIPO_MODELAGEM = produtoMag.ECOM_TIPO_MODELAGEM;
                oldprodutoMag.ECOM_TIPO_TECIDO = produtoMag.ECOM_TIPO_TECIDO;
                oldprodutoMag.ECOM_TIPO_MANGA = produtoMag.ECOM_TIPO_MANGA;
                oldprodutoMag.ECOM_TIPO_GOLA = produtoMag.ECOM_TIPO_GOLA;
                oldprodutoMag.ECOM_TIPO_COMPRIMENTO = produtoMag.ECOM_TIPO_COMPRIMENTO;
                oldprodutoMag.ECOM_TIPO_ESTILO = produtoMag.ECOM_TIPO_ESTILO;
                oldprodutoMag.ECOM_TIPO_LINHA = produtoMag.ECOM_TIPO_LINHA;
                oldprodutoMag.ECOM_SIGNED = produtoMag.ECOM_SIGNED;
                oldprodutoMag.ECOM_TIPO_RELACIONADO = produtoMag.ECOM_TIPO_RELACIONADO;
                oldprodutoMag.VISIBILIDADE = produtoMag.VISIBILIDADE;
                oldprodutoMag.ECOM_CAIXA_DIMENSAO = produtoMag.ECOM_CAIXA_DIMENSAO;
                oldprodutoMag.META_TITULO = produtoMag.META_TITULO;
                oldprodutoMag.META_KEYWORD = produtoMag.META_KEYWORD;
                oldprodutoMag.META_DESCRICAO = produtoMag.META_DESCRICAO;
                oldprodutoMag.URL_PRODUTO = produtoMag.URL_PRODUTO;
                oldprodutoMag.OBSERVACAO = produtoMag.OBSERVACAO;

                oldprodutoMag.FOTO_LOOK = produtoMag.FOTO_LOOK;
                oldprodutoMag.FOTO_LOOK_MAG = produtoMag.FOTO_LOOK_MAG;
                oldprodutoMag.FOTO_LOOK_POS = produtoMag.FOTO_LOOK_POS;

                oldprodutoMag.FOTO_FRENTE_CAB = produtoMag.FOTO_FRENTE_CAB;
                oldprodutoMag.FOTO_FRENTE_CAB_MAG = produtoMag.FOTO_FRENTE_CAB_MAG;
                oldprodutoMag.FOTO_FRENTE_CAB_POS = produtoMag.FOTO_FRENTE_CAB_POS;

                oldprodutoMag.FOTO_FRENTE_SEMCAB = produtoMag.FOTO_FRENTE_SEMCAB;
                oldprodutoMag.FOTO_FRENTE_SEMCAB_MAG = produtoMag.FOTO_FRENTE_SEMCAB_MAG;
                oldprodutoMag.FOTO_FRENTE_SEMCAB_POS = produtoMag.FOTO_FRENTE_SEMCAB_POS;

                oldprodutoMag.FOTO_COSTAS = produtoMag.FOTO_COSTAS;
                oldprodutoMag.FOTO_COSTAS_MAG = produtoMag.FOTO_COSTAS_MAG;
                oldprodutoMag.FOTO_COSTAS_POS = produtoMag.FOTO_COSTAS_POS;

                oldprodutoMag.FOTO_DETALHE = produtoMag.FOTO_DETALHE;
                oldprodutoMag.FOTO_DETALHE_MAG = produtoMag.FOTO_DETALHE_MAG;
                oldprodutoMag.FOTO_DETALHE_POS = produtoMag.FOTO_DETALHE_POS;

                oldprodutoMag.FOTO_LADO = produtoMag.FOTO_LADO;
                oldprodutoMag.FOTO_LADO_MAG = produtoMag.FOTO_LADO_MAG;
                oldprodutoMag.FOTO_LADO_POS = produtoMag.FOTO_LADO_POS;

                oldprodutoMag.FOTO_VITRINE_POS = produtoMag.FOTO_VITRINE_POS;
                oldprodutoMag.FOTO_HOVER_POS = produtoMag.FOTO_HOVER_POS;

                oldprodutoMag.FOTO_HOVER = produtoMag.FOTO_HOVER;
                oldprodutoMag.FOTO_HOVER_MAG = produtoMag.FOTO_HOVER_MAG;

                oldprodutoMag.HABILITADO = produtoMag.HABILITADO;
                oldprodutoMag.STATUS_CADASTRO = produtoMag.STATUS_CADASTRO;
                oldprodutoMag.DATA_ALTERACAO = produtoMag.DATA_ALTERACAO;
                oldprodutoMag.USUARIO_ALTERACAO = produtoMag.USUARIO_ALTERACAO;

                oldprodutoMag.GRIFFE = produtoMag.GRIFFE;
                oldprodutoMag.COD_CATEGORIA = produtoMag.COD_CATEGORIA;

                oldprodutoMag.CAD_MKTPLACE = produtoMag.CAD_MKTPLACE;

                oldprodutoMag.FOTO_SHOPPING_URLEXT = produtoMag.FOTO_SHOPPING_URLEXT;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public ECOM_EXPEDICAO ObterMagentoExpedicao(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EXPEDICAOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public ECOM_EXPEDICAO ObterMagentoExpedicaoPorPedido(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EXPEDICAOs
                    where m.PEDIDO == pedido.Trim()
                    select m).SingleOrDefault();
        }
        public void InserirMagentoExpedicao(ECOM_EXPEDICAO expedicao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_EXPEDICAOs.InsertOnSubmit(expedicao);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirMagentoExpedicao(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_EXPEDICAO expedicao = ObterMagentoExpedicao(codigo);
                if (expedicao != null)
                {
                    Intradb.ECOM_EXPEDICAOs.DeleteOnSubmit(expedicao);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_EXPEDICAO_PRODUTO> ObterMagentoExpedicaoProduto(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EXPEDICAO_PRODUTOs
                    where m.PEDIDO == pedido
                    select m).ToList();
        }
        public void InserirMagentoExpedicaoProduto(ECOM_EXPEDICAO_PRODUTO expProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_EXPEDICAO_PRODUTOs.InsertOnSubmit(expProduto);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_EXPEDICAO_VOLUME> ObterMagentoExpedicaoVolumePorPedido(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EXPEDICAO_VOLUMEs
                    where m.PEDIDO == pedido
                    select m).ToList();
        }
        public decimal ObterMagentoPesoVolumePorPedido(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            var volumeExp = (from m in Intradb.ECOM_EXPEDICAO_VOLUMEs
                             where m.PEDIDO == pedido
                             select m).ToList();

            if (volumeExp == null || volumeExp.Count() <= 0)
                return 0;

            var resp = volumeExp.Select(p => p.PESO_KG).Sum();

            if (resp == null || resp <= 0)
                return 0;

            return resp;
        }
        public ECOM_EXPEDICAO_VOLUME ObterMagentoExpedicaoVolumePorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EXPEDICAO_VOLUMEs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirMagentoExpedicaoVolume(ECOM_EXPEDICAO_VOLUME expedicaoVol)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_EXPEDICAO_VOLUMEs.InsertOnSubmit(expedicaoVol);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirMagentoExpedicaoVolume(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_EXPEDICAO_VOLUME expedicaoVol = ObterMagentoExpedicaoVolumePorCodigo(codigo);
                if (expedicaoVol != null)
                {
                    Intradb.ECOM_EXPEDICAO_VOLUMEs.DeleteOnSubmit(expedicaoVol);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ECOM_PEDIDO_FRETE ObterFrete(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_FRETEs
                    where m.PEDIDO == pedido
                    select m).SingleOrDefault();

        }
        public void InserirFrete(ECOM_PEDIDO_FRETE frete)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PEDIDO_FRETEs.InsertOnSubmit(frete);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFrete(ECOM_PEDIDO_FRETE frete)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PEDIDO_FRETE _frete = ObterFrete(frete.PEDIDO);

            if (_frete != null)
            {
                _frete.PESO_KG = frete.PESO_KG;
                _frete.TIPO_FRETE = frete.TIPO_FRETE;
                _frete.FORMA_ENTREGA = frete.FORMA_ENTREGA;
                _frete.VALOR_FRETE = frete.VALOR_FRETE;
                _frete.RETIRADA_LOJA = frete.RETIRADA_LOJA;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ECOM_CAIXA_DIMENSAO> ObterMagentoCaixaDimensao()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CAIXA_DIMENSAOs
                    select m).ToList();
        }
        public ECOM_CAIXA_DIMENSAO ObterMagentoCaixaDimensaoPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CAIXA_DIMENSAOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }


        public void InserirMagentoFaturamento(ECOM_FATURAMENTO faturamento)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_FATURAMENTOs.InsertOnSubmit(faturamento);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ECOM_FATURAMENTO ObterMagentoFaturamento(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FATURAMENTOs
                    where m.PEDIDO.Trim() == pedido.Trim()
                    select m).SingleOrDefault();
        }

        public ECOM_PEDIDO_ENTREGA_MAG ObterPedidoEntregaPorPedidoLinx(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_ENTREGA_MAGs where m.PEDIDO.Contains(pedido) select m).SingleOrDefault();
        }
        public ECOM_PEDIDO_ENTREGA_MAG ObterPedidoEntregaPorPedidoExterno(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_ENTREGA_MAGs where m.PEDIDO_EXTERNO.Contains(pedidoExterno) select m).SingleOrDefault();
        }
        public void InserirPedidoEntrega(ECOM_PEDIDO_ENTREGA_MAG pedidoEntrega)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PEDIDO_ENTREGA_MAGs.InsertOnSubmit(pedidoEntrega);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPedidoEntrega(ECOM_PEDIDO_ENTREGA_MAG pedidoEntrega)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PEDIDO_ENTREGA_MAG _pedidoEntrega = ObterPedidoEntregaPorPedidoLinx(pedidoEntrega.PEDIDO);

            if (_pedidoEntrega != null)
            {

                _pedidoEntrega.PEDIDO_ENTREGA = pedidoEntrega.PEDIDO_ENTREGA;
                _pedidoEntrega.PEDIDO_EXTERNO = pedidoEntrega.PEDIDO_EXTERNO;
                _pedidoEntrega.CARRIER = pedidoEntrega.CARRIER;
                _pedidoEntrega.EMAIL_ENVIADO = pedidoEntrega.EMAIL_ENVIADO;
                _pedidoEntrega.DATA_ENVIO = pedidoEntrega.DATA_ENVIO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ECOM_PEDIDO_ENTREGA_MAG_TRACK ObterPedidoEntregaTrack(string trackNumber)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_ENTREGA_MAG_TRACKs where m.TRACK_NUMBER == trackNumber select m).SingleOrDefault();
        }
        public void InserirPedidoEntregaTrack(ECOM_PEDIDO_ENTREGA_MAG_TRACK pedidoEntregaTrack)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PEDIDO_ENTREGA_MAG_TRACKs.InsertOnSubmit(pedidoEntregaTrack);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_CORREIOS_POST> ObterCorreiosPostNaoEntregue()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_POSTs where m.ENTREGA_REALIZADA == false select m).ToList();
        }
        public List<ECOM_CORREIOS_POST> ObterCorreiosPost(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_POSTs where m.PEDIDO.Contains(pedido.Trim()) select m).ToList();
        }
        public ECOM_CORREIOS_POST ObterCorreiosPostTrackNumber(string trackNumber)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_POSTs where m.TRACK_NUMBER == trackNumber select m).SingleOrDefault();
        }
        public void InserirCorreiosPost(ECOM_CORREIOS_POST correioPost)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_CORREIOS_POSTs.InsertOnSubmit(correioPost);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCorreiosPost(ECOM_CORREIOS_POST correioPost)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_CORREIOS_POST correios = ObterCorreiosPostTrackNumber(correioPost.TRACK_NUMBER);

            if (correios != null)
            {

                correios.TRACK_NUMBER = correioPost.TRACK_NUMBER;
                correios.PEDIDO = correioPost.PEDIDO;
                correios.ECOM_EXPEDICAO_VOLUME = correioPost.ECOM_EXPEDICAO_VOLUME;
                correios.VALOR_TARIFA = correioPost.VALOR_TARIFA;
                correios.ENTREGA_DOMICILIAR = correioPost.ENTREGA_DOMICILIAR;
                correios.PRAZO_DIAS_UTEIS = correioPost.PRAZO_DIAS_UTEIS;
                correios.PESO_GRAMA = correioPost.PESO_GRAMA;
                correios.DATA_ENVIO = correioPost.DATA_ENVIO;
                correios.SHIP_SERVICE_MANDAE = correioPost.SHIP_SERVICE_MANDAE;
                correios.ENTREGA_REALIZADA = correioPost.ENTREGA_REALIZADA;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public List<ECOM_CORREIOS_COLETA> ObterCorreiosColetaAberto()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_COLETAs
                    where m.DATA_COLETA == DateTime.Today
                    && m.DATA_CONTROLE == null
                    select m).ToList();
        }
        public List<ECOM_CORREIOS_COLETA> ObterCorreiosColetaHoje()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_COLETAs
                    where m.DATA_COLETA == DateTime.Today
                    select m).ToList();
        }
        public ECOM_CORREIOS_COLETA ObterCorreiosColetaPedido(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_COLETAs where m.PEDIDO == pedido select m).SingleOrDefault();
        }
        public ECOM_CORREIOS_COLETA ObterCorreiosColetaTrackNumber(string trackNumber)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORREIOS_COLETAs where m.TRACK_NUMBER == trackNumber select m).SingleOrDefault();
        }
        public void InserirCorreiosColeta(ECOM_CORREIOS_COLETA correioColeta)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_CORREIOS_COLETAs.InsertOnSubmit(correioColeta);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCorreiosColeta(ECOM_CORREIOS_COLETA correioColeta)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_CORREIOS_COLETA _correioColeta = ObterCorreiosColetaTrackNumber(correioColeta.TRACK_NUMBER);

            if (_correioColeta != null)
            {

                _correioColeta.DATA_CONTROLE = correioColeta.DATA_CONTROLE;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<SP_OBTER_FOTO_MAQUINAResult> ObterFotoMaquinaSequencia(DateTime dataFoto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_FOTO_MAQUINA(dataFoto) select m).ToList();
        }

        public ECOM_FOTO_MAQUINA ObterFotoMaquinaPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<DateTime> ObterFotoMaquinaTransferencia()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINAs
                    orderby m.DATA_FOTO
                    select m).OrderBy(p => p.DATA_FOTO).Select(p => p.DATA_FOTO).Distinct().ToList();
        }
        public List<ECOM_FOTO_MAQUINA> ObterFotoMaquinaPorData(DateTime dataFoto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINAs
                    where m.DATA_FOTO == dataFoto
                    orderby m.DATA_INCLUSAO descending, m.SEQ_INICIAL
                    select m).ToList();
        }
        public int InserirFotoMaquina(ECOM_FOTO_MAQUINA fotoMaquina)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_FOTO_MAQUINAs.InsertOnSubmit(fotoMaquina);
                Intradb.SubmitChanges();

                return fotoMaquina.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFotoMaquina(ECOM_FOTO_MAQUINA fotoMaquina)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_FOTO_MAQUINA _fotoMaquina = ObterFotoMaquinaPorCodigo(fotoMaquina.CODIGO);

            if (_fotoMaquina != null)
            {

                _fotoMaquina.DATA_FOTO = fotoMaquina.DATA_FOTO;
                _fotoMaquina.SEQ_INICIAL = fotoMaquina.SEQ_INICIAL;
                _fotoMaquina.SEQ_FINAL = fotoMaquina.SEQ_FINAL;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirFotoMaquina(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_FOTO_MAQUINA fotoMaquina = ObterFotoMaquinaPorCodigo(codigo);
                if (fotoMaquina != null)
                {
                    Intradb.ECOM_FOTO_MAQUINAs.DeleteOnSubmit(fotoMaquina);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ECOM_FOTO_MAQUINA_PROD ObterFotoMaquinaProdutoPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_PRODs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<ECOM_FOTO_MAQUINA_PROD> ObterFotoMaquinaProdutoPorMaquina(int ecomFotoMaquina)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_PRODs
                    where m.ECOM_FOTO_MAQUINA == ecomFotoMaquina
                    select m).ToList();
        }
        public List<SP_OBTER_FOTO_MAQUINA_PROD_FOTOResult> ObterFotoMaquinaProdutoPorMaquinaFOTO(int ecomFotoMaquina)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_FOTO_MAQUINA_PROD_FOTO(ecomFotoMaquina) select m).ToList();

        }
        public ECOM_FOTO_MAQUINA_PROD ObterFotoMaquinaProdutoUltimoAberto()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_PRODs
                    where m.ECOM_FOTO_MAQUINA1.DATA_FOTO == DateTime.Today
                    && m.PRODUTO == null
                    orderby m.CODIGO
                    select m).FirstOrDefault();
        }
        public int InserirFotoMaquinaProduto(ECOM_FOTO_MAQUINA_PROD fotoMaquinaProd)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_FOTO_MAQUINA_PRODs.InsertOnSubmit(fotoMaquinaProd);
                Intradb.SubmitChanges();

                return fotoMaquinaProd.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFotoMaquinaProduto(ECOM_FOTO_MAQUINA_PROD fotoMaquinaProd)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_FOTO_MAQUINA_PROD _fotoMaquinaProd = ObterFotoMaquinaProdutoPorCodigo(fotoMaquinaProd.CODIGO);

            if (_fotoMaquinaProd != null)
            {

                _fotoMaquinaProd.PRODUTO = fotoMaquinaProd.PRODUTO;
                _fotoMaquinaProd.DESC_PRODUTO = fotoMaquinaProd.DESC_PRODUTO;
                _fotoMaquinaProd.COR = fotoMaquinaProd.COR;
                _fotoMaquinaProd.DESC_COR = fotoMaquinaProd.DESC_COR;
                _fotoMaquinaProd.DESC_OUTRO = fotoMaquinaProd.DESC_OUTRO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirFotoMaquinaProduto(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_FOTO_MAQUINA_PROD fotoMaquinaProd = ObterFotoMaquinaProdutoPorCodigo(codigo);
                if (fotoMaquinaProd != null)
                {
                    Intradb.ECOM_FOTO_MAQUINA_PRODs.DeleteOnSubmit(fotoMaquinaProd);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InserirFotoMaquinaHist(ECOM_FOTO_MAQUINA_HIST fotoMaquinaHist)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_FOTO_MAQUINA_HISTs.InsertOnSubmit(fotoMaquinaHist);
                Intradb.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ECOM_FOTO_MAQUINA_HIST ObterFotoMaquinaHistorico(string produto, string cor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_HISTs
                    where m.PRODUTO == produto
                    && m.COR == cor
                    select m).FirstOrDefault();
        }

        public List<ECOM_COR> ObterMagentoCor()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORs orderby m.COR select m).ToList();
        }
        public ECOM_COR ObterMagentoCor(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public ECOM_COR ObterMagentoCorPorDescricao(string corMagento)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_CORs
                    where m.COR == corMagento
                    select m).SingleOrDefault();
        }
        public ECOM_COR_LINX ObterMagentoCorLinx(string cor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_COR_LINXes
                    where m.COR.Trim() == cor
                    select m).SingleOrDefault();
        }

        public ECOM_PRODUTO_DESCRICAO ObterProdutoDescricao(string produto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_DESCRICAOs where m.PRODUTO == produto select m).SingleOrDefault();
        }
        public void InserirProdutoDescricao(ECOM_PRODUTO_DESCRICAO produtoDescricao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PRODUTO_DESCRICAOs.InsertOnSubmit(produtoDescricao);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoDescricao(ECOM_PRODUTO_DESCRICAO produtoDescricao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PRODUTO_DESCRICAO _produtoDescricao = ObterProdutoDescricao(produtoDescricao.PRODUTO);

            if (_produtoDescricao != null)
            {
                _produtoDescricao.PRODUTO_DESC = produtoDescricao.PRODUTO_DESC;
                _produtoDescricao.PECA_DESC = produtoDescricao.PECA_DESC;
                _produtoDescricao.USO_DESC = produtoDescricao.USO_DESC;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirProdutoDescricao(string produto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_PRODUTO_DESCRICAO produtoDescricao = ObterProdutoDescricao(produto);
                if (produtoDescricao != null)
                {
                    Intradb.ECOM_PRODUTO_DESCRICAOs.DeleteOnSubmit(produtoDescricao);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_GRUPO_MACRO> ObterMagentoGrupoMacro()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_MACROs orderby m.GRUPO_MACRO select m).ToList();
        }
        public ECOM_GRUPO_MACRO ObterMagentoGrupoMacroPorDescricao(string grupoMacro)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_MACROs
                    where m.GRUPO_MACRO == grupoMacro
                    select m).SingleOrDefault();
        }
        public ECOM_GRUPO_MACRO_LINX ObterMagentoGrupoMacroLinx(string grupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_MACRO_LINXes
                    where m.GRUPO_PRODUTO.Trim() == grupoProduto
                    select m).SingleOrDefault();
        }

        public ECOM_GRUPO_PRODUTO ObterMagentoGrupoProduto(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_PRODUTOs
                    where m.CODIGO == codigo
                    orderby m.GRUPO
                    select m).SingleOrDefault();
        }
        public List<ECOM_GRUPO_PRODUTO> ObterMagentoGrupoProduto()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_PRODUTOs orderby m.GRUPO select m).ToList();
        }
        public List<ECOM_GRUPO_PRODUTO> ObterMagentoGrupoProdutoBloco()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_PRODUTOs where m.MOSTRAR_BLOCO == true orderby m.GRUPO select m).ToList();
        }
        public List<ECOM_GRUPO_PRODUTO> ObterMagentoGrupoProdutoAberto()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            var g = (from m in Intradb.ECOM_GRUPO_PRODUTOs
                     join n in Intradb.ECOM_BLOCO_PRODUTOs on m.CODIGO equals n.ECOM_GRUPO_PRODUTO
                     where m.MOSTRAR_BLOCO == true
                     && n.ECOM_ORDEM_DATA1.DATA_ORDEM == null
                     orderby m.GRUPO
                     select m).ToList();
            var q = g.Distinct().ToList();
            return q;

        }
        public ECOM_GRUPO_PRODUTO ObterMagentoGrupoProdutoDePara(int codigoGrupo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_PRODUTOs
                    where m.CODIGO == codigoGrupo
                    select m).SingleOrDefault();
        }
        public ECOM_GRUPO_PRODUTO ObterMagentoGrupoProdutoCor(int codigoPai, int codigoCor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_GRUPO_PRODUTOs
                    where m.CODIGO_PAI == codigoPai && m.ECOM_COR == codigoCor
                    select m).SingleOrDefault();
        }



        public List<SP_OBTER_FOTO_MAQUINA_LOOKBOOKResult> ObterFotoMaquinaLookBookSequencia(DateTime dataFoto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_FOTO_MAQUINA_LOOKBOOK(dataFoto) select m).ToList();
        }

        public ECOM_FOTO_MAQUINA_LOOKBOOK ObterFotoMaquinaLookBookPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_LOOKBOOKs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<DateTime> ObterFotoMaquinaLookBookTransferencia()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_LOOKBOOKs
                    orderby m.DATA_FOTO
                    select m).Select(p => p.DATA_FOTO).Distinct().ToList();
        }
        public List<ECOM_FOTO_MAQUINA_LOOKBOOK> ObterFotoMaquinaLookBookPorData(DateTime dataFoto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_LOOKBOOKs
                    where m.DATA_FOTO == dataFoto
                    orderby m.DATA_INCLUSAO descending, m.SEQ_INICIAL
                    select m).ToList();
        }
        public int InserirFotoMaquinaLookBook(ECOM_FOTO_MAQUINA_LOOKBOOK fotoMaquinaLookBook)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_FOTO_MAQUINA_LOOKBOOKs.InsertOnSubmit(fotoMaquinaLookBook);
                Intradb.SubmitChanges();

                return fotoMaquinaLookBook.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFotoMaquinaLookBook(ECOM_FOTO_MAQUINA_LOOKBOOK fotoMaquinaLookBook)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_FOTO_MAQUINA_LOOKBOOK _fotoMaquinaLookBook = ObterFotoMaquinaLookBookPorCodigo(fotoMaquinaLookBook.CODIGO);

            if (_fotoMaquinaLookBook != null)
            {

                _fotoMaquinaLookBook.DATA_FOTO = fotoMaquinaLookBook.DATA_FOTO;
                _fotoMaquinaLookBook.SEQ_INICIAL = fotoMaquinaLookBook.SEQ_INICIAL;
                _fotoMaquinaLookBook.SEQ_FINAL = fotoMaquinaLookBook.SEQ_FINAL;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirFotoMaquinaLookBook(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_FOTO_MAQUINA_LOOKBOOK fotoMaquinaLookBook = ObterFotoMaquinaLookBookPorCodigo(codigo);
                if (fotoMaquinaLookBook != null)
                {
                    Intradb.ECOM_FOTO_MAQUINA_LOOKBOOKs.DeleteOnSubmit(fotoMaquinaLookBook);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ECOM_FOTO_MAQUINA_PROD_LOOKBOOK ObterFotoMaquinaProdutoLookBookPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_PROD_LOOKBOOKs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<ECOM_FOTO_MAQUINA_PROD_LOOKBOOK> ObterFotoMaquinaProdutoLookBookPorMaquina(int ecomFotoMaquinaLookBook)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_PROD_LOOKBOOKs
                    where m.ECOM_FOTO_MAQUINA_LOOKBOOK == ecomFotoMaquinaLookBook
                    select m).ToList();
        }
        public ECOM_FOTO_MAQUINA_PROD_LOOKBOOK ObterFotoMaquinaProdutoLookBookUltimoAberto()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_PROD_LOOKBOOKs
                    where m.ECOM_FOTO_MAQUINA_LOOKBOOK1.DATA_FOTO == DateTime.Today
                    && m.PRODUTO == null
                    orderby m.CODIGO
                    select m).FirstOrDefault();
        }
        public int InserirFotoMaquinaProdutoLookBook(ECOM_FOTO_MAQUINA_PROD_LOOKBOOK fotoMaquinaProdLookBook)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_FOTO_MAQUINA_PROD_LOOKBOOKs.InsertOnSubmit(fotoMaquinaProdLookBook);
                Intradb.SubmitChanges();

                return fotoMaquinaProdLookBook.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFotoMaquinaProdutoLookBook(ECOM_FOTO_MAQUINA_PROD_LOOKBOOK fotoMaquinaProdLookBook)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_FOTO_MAQUINA_PROD_LOOKBOOK _fotoMaquinaProdLookBook = ObterFotoMaquinaProdutoLookBookPorCodigo(fotoMaquinaProdLookBook.CODIGO);

            if (_fotoMaquinaProdLookBook != null)
            {

                _fotoMaquinaProdLookBook.PRODUTO = fotoMaquinaProdLookBook.PRODUTO;
                _fotoMaquinaProdLookBook.DESC_PRODUTO = fotoMaquinaProdLookBook.DESC_PRODUTO;
                _fotoMaquinaProdLookBook.COR = fotoMaquinaProdLookBook.COR;
                _fotoMaquinaProdLookBook.DESC_COR = fotoMaquinaProdLookBook.DESC_COR;
                _fotoMaquinaProdLookBook.DESC_OUTRO = fotoMaquinaProdLookBook.DESC_OUTRO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirFotoMaquinaProdutoLookBook(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_FOTO_MAQUINA_PROD_LOOKBOOK fotoMaquinaProdLookBook = ObterFotoMaquinaProdutoLookBookPorCodigo(codigo);
                if (fotoMaquinaProdLookBook != null)
                {
                    Intradb.ECOM_FOTO_MAQUINA_PROD_LOOKBOOKs.DeleteOnSubmit(fotoMaquinaProdLookBook);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InserirFotoMaquinaLookBookHist(ECOM_FOTO_MAQUINA_LOOKBOOK_HIST fotoMaquinaLookBookHist)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_FOTO_MAQUINA_LOOKBOOK_HISTs.InsertOnSubmit(fotoMaquinaLookBookHist);
                Intradb.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ECOM_FOTO_MAQUINA_LOOKBOOK_HIST ObterFotoMaquinaLookBookHistorico(string produto, string cor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_MAQUINA_LOOKBOOK_HISTs
                    where m.PRODUTO == produto
                    && m.COR == cor
                    select m).FirstOrDefault();
        }




        public List<ECOM_TIPO_MODELAGEM> ObterMagentoTipoModelagem(string griffe, string grupoProduto)
        {
            var gp = grupoProduto.Split('/');
            var grupos = new List<string>();
            foreach (var g in gp)
                grupos.Add(g);

            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_MODELAGEMs
                    where m.GRIFFE.Contains(griffe) &&
                         grupos.Contains(m.GRUPO_PRODUTO)
                    orderby m.TIPO_MODELAGEM
                    select m).Distinct().OrderBy(p => p.TIPO_MODELAGEM).ToList();
        }
        public List<ECOM_TIPO_TECIDO> ObterMagentoTipoTecido(string griffe, string grupoProduto)
        {
            var gp = grupoProduto.Split('/');
            var grupos = new List<string>();
            foreach (var g in gp)
                grupos.Add(g);

            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_TECIDOs
                    where m.GRIFFE.Contains(griffe) &&
                    grupos.Contains(m.GRUPO_PRODUTO)
                    orderby m.TIPO_TECIDO
                    select m).ToList().Distinct().OrderBy(p => p.TIPO_TECIDO).ToList();
        }
        public List<ECOM_TIPO_MANGA> ObterMagentoTipoManga(string griffe, string grupoProduto)
        {
            var gp = grupoProduto.Split('/');
            var grupos = new List<string>();
            foreach (var g in gp)
                grupos.Add(g);

            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_MANGAs
                    where m.GRIFFE.Contains(griffe) &&
                        grupos.Contains(m.GRUPO_PRODUTO)
                    orderby m.TIPO_MANGA
                    select m).Distinct().OrderBy(p => p.TIPO_MANGA).ToList();
        }
        public List<ECOM_TIPO_GOLA> ObterMagentoTipoGola(string griffe, string grupoProduto)
        {
            var gp = grupoProduto.Split('/');
            var grupos = new List<string>();
            foreach (var g in gp)
                grupos.Add(g);

            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_GOLAs
                    where m.GRIFFE.Contains(griffe) &&
                        grupos.Contains(m.GRUPO_PRODUTO)
                    orderby m.TIPO_GOLA
                    select m).Distinct().OrderBy(p => p.TIPO_GOLA).ToList();
        }
        public List<ECOM_TIPO_COMPRIMENTO> ObterMagentoTipoComprimento(string griffe, string grupoProduto)
        {
            var gp = grupoProduto.Split('/');
            var grupos = new List<string>();
            foreach (var g in gp)
                grupos.Add(g);

            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_COMPRIMENTOs
                    where m.GRIFFE.Contains(griffe) &&
                        grupos.Contains(m.GRUPO_PRODUTO)
                    orderby m.TIPO_COMPRIMENTO
                    select m).Distinct().OrderBy(p => p.TIPO_COMPRIMENTO).ToList();
        }
        public List<ECOM_TIPO_ESTILO> ObterMagentoTipoEstilo(string griffe, string grupoProduto)
        {
            var gp = grupoProduto.Split('/');
            var grupos = new List<string>();
            foreach (var g in gp)
                grupos.Add(g);

            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_ESTILOs
                    where m.GRIFFE.Contains(griffe) &&
                        grupos.Contains(m.GRUPO_PRODUTO)
                    orderby m.TIPO_ESTILO
                    select m).Distinct().OrderBy(p => p.TIPO_ESTILO).ToList();
        }
        public List<ECOM_TIPO_LINHA> ObterMagentoTipoLinha()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_LINHAs orderby m.TIPO_LINHA select m).ToList();
        }
        public List<ECOM_TIPO_LINHA> ObterMagentoTipoLinha(string colecao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_LINHAs where m.COLECAO == colecao orderby m.TIPO_LINHA select m).ToList();
        }
        public List<ECOM_SIGNED> ObterMagentoSigned(string colecao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_SIGNEDs where m.COLECAO == colecao orderby m.SIGNED select m).ToList();
        }

        public List<SP_OBTER_ECOM_PRODUTOResult> ObterProdutoLinx(string colecao, string grupoProduto, string produto, string griffe)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PRODUTO(colecao, grupoProduto, produto, griffe) select m).ToList();
        }
        public List<SP_OBTER_ECOM_PRODUTO_CADResult> ObterProduto(string colecao, string grupoProduto, string produto, string griffe)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_CAD(colecao, grupoProduto, produto, griffe) select m).ToList();
        }

        public List<SP_OBTER_ECOM_PRODUTO_TRATAMENTOResult> ObterProdutoTratamento(string colecao, string grupoProduto, int hb, string produto, string griffe, char mostruario)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_TRATAMENTO(colecao, grupoProduto, hb, produto, griffe, mostruario) select m).ToList();
        }


        public List<SP_OBTER_ECOM_PRODUTO_DESCRICAOResult> ObterProdutoLinxParaDescricao(string colecao, string grupoProduto, string produto, string griffe)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_DESCRICAO(colecao, grupoProduto, produto, griffe) select m).ToList();
        }

        public List<SP_OBTER_ECOM_PRODUTO_MAGENTOResult> ObterProdutoMagento(string colecao, string grupoProduto, int? grupoMagento, string produto, string griffe, int? ecomProduto)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_MAGENTO(colecao, grupoProduto, grupoMagento, produto, griffe, ecomProduto) select m).ToList();
        }

        public List<SP_OBTER_ECOM_PRODUTO_LIBERACAOResult> ObterProdutoLinxLiberacao(string colecao, string grupoProduto, string produto, string griffe, int? ecomProduto)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_LIBERACAO(colecao, grupoProduto, produto, griffe, ecomProduto) select m).ToList();
        }
        public List<SP_OBTER_ECOM_PRODUTO_DEVOLUCAOResult> ObterProdutoLinxDevolucao(int? ecomProduto, string nfEntrada)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_DEVOLUCAO(ecomProduto, nfEntrada) select m).ToList();
        }
        public List<SP_OBTER_ECOM_PRODUTO_DEV_MOTIVOResult> ObterProdutoLinxDevolucaoMotivo(string produto, string nfEntrada, string motivo, DateTime? dataIni, DateTime? dataFim)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_DEV_MOTIVO(produto, nfEntrada, motivo, dataIni, dataFim) select m).ToList();
        }

        public int ObterProdutoLinxLiberacaoCount()
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            var count = (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_LIBERACAO_COUNT() select m).SingleOrDefault();

            if (count == null)
                return 0;

            return Convert.ToInt32(count.TOT_LIB);
        }
        public int ObterProdutoLinxDevolucaoCount()
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            var count = (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_DEVOLUCAO_COUNT() select m).SingleOrDefault();

            if (count == null)
                return 0;

            return Convert.ToInt32(count.TOT_DEV);
        }


        public List<SP_OBTER_ECOM_PRODUTO_RESERVADOResult> ObterProdutoReservado()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_RESERVADO() select m).ToList();
        }

        public List<SP_OBTER_ECOM_PEDIDO_EXPEDICAO_LISTAPRODResult> ObterPedidoExpedicaoProduto(string tipoFrete, DateTime dataAprovIni, DateTime dataAprovFim)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PEDIDO_EXPEDICAO_LISTAPROD(tipoFrete, dataAprovIni, dataAprovFim) select m).ToList();
        }

        public List<SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult> ObterPedidoExpedicao(string pedido, char aprovacao)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PEDIDO_EXPEDICAO(pedido, aprovacao) select m).ToList();
        }
        public List<SP_OBTER_ECOM_PEDIDO_EXPEDICAO_PRODUTOResult> ObterProdutoPedidoExpedicao(string pedido, string produto, string cor, string tamanho)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PEDIDO_EXPEDICAO_PRODUTO(pedido, produto, cor, tamanho) select m).ToList();
        }

        public List<SP_OBTER_ECOM_PEDIDO_FATURAMENTOResult> ObterPedidoFaturamento(string pedido)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PEDIDO_FATURAMENTO(pedido) select m).ToList();
        }

        public List<SP_OBTER_ECOM_PEDIDO_FATURADOResult> ObterPedidoFaturado(string pedido, string filial, string nfSaida, string serieNF, DateTime emissao)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PEDIDO_FATURADO(pedido, filial, nfSaida, serieNF, emissao) select m).ToList();
        }

        public SP_OBTER_NF_CHAVE_NFEResult ObterPedidoChaveNFE(string chaveNFE, string pedidoLinx, string pedidoOnline)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_NF_CHAVE_NFE(chaveNFE, pedidoLinx, pedidoOnline) select m).SingleOrDefault();
        }

        public void InserirMagentoEstoqueEnviado(ECOM_ESTOQUE estoqueMag)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_ESTOQUEs.InsertOnSubmit(estoqueMag);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ECOM_ESTOQUE_DEV ObterProdutoEstoqueDevolvido(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from f in Intradb.ECOM_ESTOQUE_DEVs where f.CODIGO == codigo select f).SingleOrDefault();
        }
        public void InserirMagentoEstoqueDevolvido(ECOM_ESTOQUE_DEV estoqueMagDev)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_ESTOQUE_DEVs.InsertOnSubmit(estoqueMagDev);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMagentoEstoqueDevolvido(ECOM_ESTOQUE_DEV estoqueMagDev)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            var devEstoqMot = ObterProdutoEstoqueDevolvido(estoqueMagDev.CODIGO);

            if (devEstoqMot != null)
            {
                devEstoqMot.PRODUTO = estoqueMagDev.PRODUTO;
                devEstoqMot.COR = estoqueMagDev.COR;
                devEstoqMot.NOME_CLIFOR = estoqueMagDev.NOME_CLIFOR;
                devEstoqMot.FILIAL = estoqueMagDev.FILIAL;
                devEstoqMot.NF_ENTRADA = estoqueMagDev.NF_ENTRADA;
                devEstoqMot.SERIE_NF_ENTRADA = estoqueMagDev.SERIE_NF_ENTRADA;
                devEstoqMot.RECEBIMENTO = estoqueMagDev.RECEBIMENTO;
                devEstoqMot.DATA_ENTRADA = estoqueMagDev.DATA_ENTRADA;
                devEstoqMot.USUARIO_ENTRADA = estoqueMagDev.USUARIO_ENTRADA;
                devEstoqMot.MOTIVO = estoqueMagDev.MOTIVO;
                devEstoqMot.DEFEITO = estoqueMagDev.DEFEITO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<SP_OBTER_ECOM_PRODUTO_ESTOQUEResult> ObterProdutoEstoqueMagento(string colecao, string grupoProduto, string produto, string cor, string griffe, string codCategoria)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_ESTOQUE(colecao, grupoProduto, produto, cor, griffe, codCategoria) select m).ToList();
        }

        public List<SP_OBTER_ECOM_FOTO_SEM_SELECAOResult> ObterProdutoFotoSemSelecao()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ECOM_FOTO_SEM_SELECAO() select m).ToList();
        }
        public List<SP_OBTER_ECOM_FOTO_SEM_SELECAO_APPLBResult> ObterProdutoFotoSemSelecaoAppLB()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ECOM_FOTO_SEM_SELECAO_APPLB() select m).ToList();
        }

        public List<SP_OBTER_ECOM_PRODUTO_RELACIONADOResult> ObterProdutoRelacionado(string colecao, string grupoProduto, string produto, string griffe, int ecomCor, int ecomGrupoMacro, string tecido, string corFornecedor, int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_RELACIONADO(colecao, grupoProduto, produto, griffe, ecomCor, ecomGrupoMacro, tecido, corFornecedor, codigo) select m).ToList();
        }
        public List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_DETResult> ObterProdutoRelacionadoDET(int codProduto, string colecao, string grupoLinx, int? codEcomGrupoProduto, string produto, string corLinx, string tecido, string corFornecedor, string griffe, string codCategoria)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_RELACIONADO_DET(codProduto, colecao, grupoLinx, codEcomGrupoProduto, produto, corLinx, tecido, corFornecedor, griffe, codCategoria) select m).ToList();
        }
        public SP_OBTER_ECOM_PRODUTO_RELACIONADO_LOOKResult ObterProdutoRelacionadoLook(string produto, string cor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_RELACIONADO_LOOK(produto, cor) select m).SingleOrDefault();
        }
        public List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_PRODUTOResult> ObterProdutoRelacionadoProduto(int ecomProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_RELACIONADO_PRODUTO(ecomProduto) select m).ToList();
        }

        public List<SP_OBTER_ECOM_CLIENTES_COMPRARAMTBResult> ObterProdutoClientesCompraramTB(string produto, string cor)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return (from m in LINXdb.SP_OBTER_ECOM_CLIENTES_COMPRARAMTB(produto, cor) select m).ToList();
        }





        public void InserirEstoqueMagentoTemp(ECOM_ESTOQUE_TEMP estoqueTemp)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_ESTOQUE_TEMPs.InsertOnSubmit(estoqueTemp);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InserirEstoqueMagentoTempDapper(List<ECOM_ESTOQUE_TEMP> estoqueTemp)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var conn = Intradb.Connection;
                conn.Execute(@"insert ECOM_ESTOQUE_TEMP(PRODUCT_ID, SKU, QTY, IS_IN_STOCK, USUARIO, VISIBILIDADE) values(@PRODUCT_ID, @SKU, @QTY, @IS_IN_STOCK, @USUARIO, @VISIBILIDADE)", estoqueTemp);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InserirEstoqueMagentoTempV2Dapper(List<ECOM_ESTOQUE_TEMPV2> estoqueTempV2)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var conn = Intradb.Connection;
                conn.Execute(@"insert ECOM_ESTOQUE_TEMPV2(SKU, QTY, SOURCE_CODE, STATUS, USUARIO, SALABLE) values(@SKU, @QTY, @SOURCE_CODE, @STATUS, @USUARIO, @SALABLE)", estoqueTempV2);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public int ExcluirEstoqueMagentoTemp(int codigoUsuario)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_EXCLUIR_ESTOQUE_TEMP(codigoUsuario) select m).FirstOrDefault().OK;
        }
        public List<SP_OBTER_ESTOQUE_TEMPResult> ObterEstoqueMagentoTemp(int codigoUsuario)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ESTOQUE_TEMP(codigoUsuario) select m).ToList();
        }
        public List<SP_OBTER_ESTOQUE_TEMPV2Result> ObterEstoqueMagentoTempV2(int codigoUsuario)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ESTOQUE_TEMPV2(codigoUsuario) select m).ToList();
        }

        public List<SP_OBTER_ESTOQUE_TEMP_VALResult> ObterEstoqueMagentoTempParaValidar(int codigoUsuario)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ESTOQUE_TEMP_VAL(codigoUsuario) select m).ToList();
        }
        public List<SP_OBTER_ESTOQUE_TEMP_VALV2Result> ObterEstoqueMagentoTempV2ParaValidar(int codigoUsuario)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return (from m in LINXdb.SP_OBTER_ESTOQUE_TEMP_VALV2(codigoUsuario) select m).ToList();
        }
        public List<SP_OBTER_ESTOQUE_TEMP_VALSALABLEV2Result> ObterEstoqueMagentoTempV2SalableParaValidar(int codigoUsuario)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            Intradb.CommandTimeout = 0;
            return (from m in Intradb.SP_OBTER_ESTOQUE_TEMP_VALSALABLEV2(codigoUsuario) select m).ToList();
        }


        public SP_OBTER_ECOM_ETIQUETA_IMPResult ObterEtiquetaImpressao(string nomeCliFor, string trackNumber, string pedido)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return (from m in LINXdb.SP_OBTER_ECOM_ETIQUETA_IMP(nomeCliFor, trackNumber, pedido) select m).SingleOrDefault();
        }

        public List<SP_OBTER_ECOM_PRODUTO_MANDAEResult> ObterProdutoMandaeItens(string pedido)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return (from m in LINXdb.SP_OBTER_ECOM_PRODUTO_MANDAE(pedido) select m).ToList();
        }

        public SP_OBTER_ROMANEIO_MANDAEResult ObterItemRomaneioMandae(string trackNumber)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ROMANEIO_MANDAE(trackNumber) select m).SingleOrDefault();
        }

        public List<ECOM_FOTO_LOOKBOOK> ObterFotoLookbook()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_LOOKBOOKs
                    where m.DATA_TRANSF == null
                    select m).ToList();
        }
        public ECOM_FOTO_LOOKBOOK ObterFotoLookbook(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_LOOKBOOKs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public ECOM_FOTO_LOOKBOOK ObterFotoLookbook(string nomeFoto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_FOTO_LOOKBOOKs
                    where m.NOME_FOTO == nomeFoto
                    select m).SingleOrDefault();
        }
        public void AtualizarFotoLookbook(ECOM_FOTO_LOOKBOOK lookbook)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            var _look = ObterFotoLookbook(lookbook.CODIGO);

            if (_look != null)
            {
                _look.NOME_FOTO = lookbook.NOME_FOTO;
                _look.DATA_TRANSF = lookbook.DATA_TRANSF;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public List<SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult> ObterProdutoBlocoOrdem(int codigoBloco)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEM(codigoBloco) select m).ToList();
        }
        public ECOM_BLOCO_PRODUTO_ORDEM ObterBlocoProdutoOrdemPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTO_ORDEMs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public List<ECOM_BLOCO_PRODUTO_ORDEM> ObterBlocoProdutoOrdemPorCatBloco(int codigoBloco, int ecomGrupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTO_ORDEMs
                    where m.ECOM_BLOCO_PRODUTO == codigoBloco
                    && m.ECOM_BLOCO_PRODUTO1.ECOM_GRUPO_PRODUTO == ecomGrupoProduto
                    orderby m.ORDEM descending
                    select m).ToList();
        }
        public List<ECOM_BLOCO_PRODUTO_ORDEM> ObterBlocoProdutoOrdemPorCodigoEcomProduto(int ecomProduto, string tipoCategoria)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTO_ORDEMs
                    where m.ECOM_PRODUTO == ecomProduto
                    && m.TIPO_CATEGORIA == tipoCategoria
                    select m).ToList();
        }
        public List<ECOM_BLOCO_PRODUTO_ORDEM> ObterBlocoProdutoOrdemPorCodigoEcomProduto(int ecomProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTO_ORDEMs
                    where m.ECOM_PRODUTO == ecomProduto
                    orderby m.ECOM_BLOCO_PRODUTO1.ECOM_GRUPO_PRODUTO1.GRUPO ascending, m.ECOM_BLOCO_PRODUTO1.BLOCO ascending
                    select m).ToList();
        }
        public List<ECOM_BLOCO_PRODUTO_ORDEM> ObterBlocoProdutoOrdemPorCodigoEcomGrupoProduto(int ecomGrupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTO_ORDEMs
                    where m.ECOM_BLOCO_PRODUTO1.ECOM_GRUPO_PRODUTO == ecomGrupoProduto
                    select m).ToList();
        }
        public ECOM_BLOCO_PRODUTO_ORDEM ObterBlocoProdutoOrdemPorIdMagProduto(int codigoBloco, int idMagProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTO_ORDEMs
                    where m.ECOM_PRODUTO1.ID_PRODUTO_MAG == idMagProduto
                    && m.ECOM_BLOCO_PRODUTO == codigoBloco
                    select m).SingleOrDefault();
        }
        public ECOM_BLOCO_PRODUTO_ORDEM ObterBlocoProdutoOrdemPorCodigoeEcomGrupoProduto(int ecomGrupoProduto, int ecomProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTO_ORDEMs
                    where m.ECOM_PRODUTO == ecomProduto
                    && m.ECOM_BLOCO_PRODUTO1.ECOM_GRUPO_PRODUTO == ecomGrupoProduto
                    select m).FirstOrDefault();
        }
        public int InserirBlocoProdutoOrdem(ECOM_BLOCO_PRODUTO_ORDEM blocoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_BLOCO_PRODUTO_ORDEMs.InsertOnSubmit(blocoProduto);
                Intradb.SubmitChanges();

                return blocoProduto.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarBlocoProdutoOrdem(ECOM_BLOCO_PRODUTO_ORDEM blocoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            var _blocoProduto = ObterBlocoProdutoOrdemPorCodigo(blocoProduto.CODIGO);

            if (_blocoProduto != null)
            {
                _blocoProduto.ORDEM = blocoProduto.ORDEM;
                if (blocoProduto.DATA_ULT_ATUALIZACAO != null)
                    _blocoProduto.DATA_ULT_ATUALIZACAO = blocoProduto.DATA_ULT_ATUALIZACAO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirBlocoProdutoOrdemPorCatBloco(int codigoBloco, int ecomGrupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var car = ObterBlocoProdutoOrdemPorCatBloco(codigoBloco, ecomGrupoProduto);
                foreach (var c in car)
                {
                    if (c != null)
                    {
                        Intradb.ECOM_BLOCO_PRODUTO_ORDEMs.DeleteOnSubmit(c);
                        Intradb.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirBlocoProdutoOrdem(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_BLOCO_PRODUTO_ORDEM blocoProd = ObterBlocoProdutoOrdemPorCodigo(codigo);
                if (blocoProd != null)
                {
                    Intradb.ECOM_BLOCO_PRODUTO_ORDEMs.DeleteOnSubmit(blocoProd);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InserirBlocoProdutoOrdemHist(ECOM_BLOCO_PRODUTO_ORDEM_HIST blocoHist)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_BLOCO_PRODUTO_ORDEM_HISTs.InsertOnSubmit(blocoHist);
                Intradb.SubmitChanges();

                return blocoHist.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ECOM_TIPO_RELACIONADO> ObterTipoRelacionado()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_TIPO_RELACIONADOs
                    orderby m.DESCRICAO
                    select m).ToList();
        }

        public List<ECOM_BLOCO_PRODUTO> ObterBloco()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTOs
                    where m.ECOM_ORDEM_DATA1.DATA_ORDEM == null
                    orderby m.ORDEM
                    select m).ToList();
        }
        public List<ECOM_BLOCO_PRODUTO> ObterBlocoPorCategoriaMag(int ecomGrupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTOs
                    where m.ECOM_GRUPO_PRODUTO == ecomGrupoProduto
                    orderby m.ECOM_GRUPO_PRODUTO1.GRUPO, m.BLOCO
                    select m).ToList();
        }
        public ECOM_BLOCO_PRODUTO ObterBloco(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_BLOCO_PRODUTOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirBloco(ECOM_BLOCO_PRODUTO bloco)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_BLOCO_PRODUTOs.InsertOnSubmit(bloco);
                Intradb.SubmitChanges();

                return bloco.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarBloco(ECOM_BLOCO_PRODUTO bloco)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_BLOCO_PRODUTO _bloco = ObterBloco(bloco.CODIGO);

            if (_bloco != null)
            {

                _bloco.ECOM_ORDEM_DATA = bloco.ECOM_ORDEM_DATA;
                _bloco.ECOM_GRUPO_PRODUTO = bloco.ECOM_GRUPO_PRODUTO;
                _bloco.BLOCO = bloco.BLOCO;
                _bloco.ORDEM = bloco.ORDEM;
                _bloco.DATA_INCLUSAO = bloco.DATA_INCLUSAO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirBloco(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_BLOCO_PRODUTO bloco = ObterBloco(codigo);
                if (bloco != null)
                {
                    Intradb.ECOM_BLOCO_PRODUTOs.DeleteOnSubmit(bloco);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ECOM_ORDEM_DATA ObterUltimaDataOrdem()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_ORDEM_DATAs
                    where m.DATA_ORDEM == null
                    select m).SingleOrDefault();

        }

        public List<SP_OBTER_ECOM_PRODUTO_CATMAGENTOResult> ObterProdutoPorCategoriaMagParaOrdem(int ecomGrupoProduto, string colecao, string produto, string cor, string tecido, string corFornecedor,
            int tipoModelagem, int tipoTecido, int tipoManga, int tipoGola, int tipoComprimento, int tipoEstilo, string tipoCategoria, string grupoProduto, string griffe)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_CATMAGENTO(ecomGrupoProduto, colecao, produto, cor, tecido, corFornecedor, tipoModelagem, tipoTecido, tipoManga, tipoGola, tipoComprimento, tipoEstilo, tipoCategoria, grupoProduto, griffe) select m).ToList();
        }

        public List<SP_OBTER_ECOM_PRODUTO_SEMBLOCOResult> ObterProdutoSemBloco(int ecomGrupoProduto, string colecao, string produto, string cor, string tecido, string corFornecedor,
            int tipoModelagem, int tipoTecido, int tipoManga, int tipoGola, int tipoComprimento, int tipoEstilo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PRODUTO_SEMBLOCO(ecomGrupoProduto, colecao, produto, cor, tecido, corFornecedor, tipoModelagem, tipoTecido, tipoManga, tipoGola, tipoComprimento, tipoEstilo) select m).ToList();
        }

        public List<SP_OBTER_ECOM_VENDACOMP_COLOUTLETResult> ObterVendaComparativoColecaoOutlet(DateTime dataIni, DateTime dataFim, string griffe, string grupoProduto)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return (from m in LINXdb.SP_OBTER_ECOM_VENDACOMP_COLOUTLET(dataIni, dataFim, griffe, grupoProduto) select m).ToList();
        }
        public List<SP_OBTER_ECOM_VENDA_COM_ANALYTICSResult> ObterVendaComparativoAnalyticsMag(DateTime dataIni, DateTime dataFim)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_VENDA_COM_ANALYTICS(dataIni, dataFim) select m).ToList();
        }


        public List<SP_OBTER_ECOM_PEDIDO_MAGResult> ObterPedidosMag(string pedidoExterno, DateTime dataIni, DateTime dataFim, string nome, string metodo, string statusMag, decimal valPagoIni, decimal valPagoFim, string email, string cpf)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PEDIDO_MAG(pedidoExterno, dataIni, dataFim, nome, metodo, statusMag, valPagoIni, valPagoFim, email, cpf) select m).ToList();
        }
        public List<SP_OBTER_ECOM_PEDIDO_MAG_BOLETOResult> ObterPedidosMagBoleto(string pedidoExterno, DateTime dataIni, DateTime dataFim, string nome, string email, string cpf, string statusMag, string statusCliente)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_PEDIDO_MAG_BOLETO(pedidoExterno, dataIni, dataFim, nome, email, cpf, statusMag, statusCliente) select m).ToList();
        }

        public List<ECOM_PEDIDO_MAG_STATUS> ObterEcomPedidoMagStatus()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_STATUS
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public List<ECOM_PEDIDO_CLIENTE_HIST_STA> ObterEcomPedidoMagClienteHistStatus()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_CLIENTE_HIST_STAs
                    orderby m.MOTIVO
                    select m).ToList();
        }

        public List<ECOM_PEDIDO_CLIENTE_HIST> ObterEcomPedidoMagClienteHist(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_CLIENTE_HISTs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    orderby m.DATA_CRIACAO descending
                    select m).ToList();
        }
        public int InserirEcomPedidoMagClienteHist(ECOM_PEDIDO_CLIENTE_HIST hist)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PEDIDO_CLIENTE_HISTs.InsertOnSubmit(hist);
                Intradb.SubmitChanges();

                return hist.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* TABELAS PARA RELACIONADOS*/
        public List<ECOM_REL_CONFIG_OPERACOE> ObterEcomRelOperacoes()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_OPERACOEs
                    orderby m.OPERACAO
                    select m).ToList();
        }
        public ECOM_REL_CONFIG_OPERACOE ObterEcomRelOperacoes(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_OPERACOEs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }

        public List<ECOM_REL_CONFIG_FILTRO> ObterEcomRelFiltros()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_FILTROs
                    orderby m.CAMPO_ECOM_PRODUTO
                    select m).ToList();
        }
        public ECOM_REL_CONFIG_FILTRO ObterEcomRelFiltrosPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_FILTROs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }

        public List<SP_OBTER_ECOM_REL_CONFIGResult> ObterEcomRelConfig(string griffe, string grupoProduto, int codigoConfigTipo, string nome)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_REL_CONFIG(griffe, grupoProduto, codigoConfigTipo, nome) select m).ToList();
        }
        public List<ECOM_REL_CONFIG> ObterEcomRelConfig()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIGs
                    orderby m.NOME
                    select m).ToList();
        }
        public List<ECOM_REL_CONFIG> ObterEcomRelConfigPorTipo(int codigoConfigTipo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIGs
                    where m.ECOM_REL_CONFIG_TIPO == codigoConfigTipo
                    orderby m.NOME
                    select m).ToList();
        }
        public ECOM_REL_CONFIG ObterEcomRelConfigPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIGs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirEcomRelConfig(ECOM_REL_CONFIG config)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_REL_CONFIGs.InsertOnSubmit(config);
                Intradb.SubmitChanges();

                return config.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEcomRelConfig(ECOM_REL_CONFIG config)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_REL_CONFIG _config = ObterEcomRelConfigPorCodigo(config.CODIGO);

            if (_config != null)
            {

                _config.CODIGO = config.CODIGO;
                _config.NOME = config.NOME;
                _config.ECOM_REL_CONFIG_TIPO = config.ECOM_REL_CONFIG_TIPO;
                _config.DATA_INCLUSAO = config.DATA_INCLUSAO;
                _config.STATUS = config.STATUS;


                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEcomRelConfig(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_REL_CONFIG config = ObterEcomRelConfigPorCodigo(codigo);
                if (config != null)
                {
                    Intradb.ECOM_REL_CONFIGs.DeleteOnSubmit(config);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_REL_CONFIG_CAMPO> ObterEcomRelConfigCampo()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_CAMPOs
                    orderby m.ECOM_REL_CONFIG_FILTRO1.CAMPO_WHERE
                    select m).ToList();
        }
        public List<ECOM_REL_CONFIG_CAMPO> ObterEcomRelConfigCampoPorRelConfig(int codigoConfig)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_CAMPOs
                    where m.ECOM_REL_CONFIG == codigoConfig
                    orderby m.ECOM_REL_CONFIG_FILTRO1.CAMPO_WHERE
                    select m).ToList();
        }
        public ECOM_REL_CONFIG_CAMPO ObterEcomRelConfigCampoPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_CAMPOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public ECOM_REL_CONFIG_CAMPO ObterEcomRelConfigCampoPorFiltro(int codigoConfig, int codigoConfigFiltro)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_CAMPOs
                    where m.ECOM_REL_CONFIG == codigoConfig && m.ECOM_REL_CONFIG_FILTRO == codigoConfigFiltro
                    select m).SingleOrDefault();
        }
        public int InserirEcomRelConfigCampo(ECOM_REL_CONFIG_CAMPO configCampo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_REL_CONFIG_CAMPOs.InsertOnSubmit(configCampo);
                Intradb.SubmitChanges();

                return configCampo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEcomRelConfigCampo(ECOM_REL_CONFIG_CAMPO configCampo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_REL_CONFIG_CAMPO _configCampo = ObterEcomRelConfigCampoPorCodigo(configCampo.CODIGO);

            if (_configCampo != null)
            {

                _configCampo.CODIGO = configCampo.CODIGO;
                _configCampo.ECOM_REL_CONFIG = configCampo.ECOM_REL_CONFIG;
                _configCampo.ECOM_REL_CONFIG_FILTRO = configCampo.ECOM_REL_CONFIG_FILTRO;
                _configCampo.ECOM_REL_CONFIG_OPERACOES = configCampo.ECOM_REL_CONFIG_OPERACOES;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEcomRelConfigCampo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_REL_CONFIG_CAMPO configCampo = ObterEcomRelConfigCampoPorCodigo(codigo);
                if (configCampo != null)
                {
                    Intradb.ECOM_REL_CONFIG_CAMPOs.DeleteOnSubmit(configCampo);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_REL_CONFIG_CAMPO_VAL> ObterEcomRelConfigCampoValor()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_CAMPO_VALs
                    orderby m.VALOR
                    select m).ToList();
        }
        public List<ECOM_REL_CONFIG_CAMPO_VAL> ObterEcomRelConfigCampoValorPorConfigCampo(int codigoConfigCampo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_CAMPO_VALs
                    where m.ECOM_REL_CONFIG_CAMPO == codigoConfigCampo
                    orderby m.VALOR
                    select m).ToList();
        }
        public ECOM_REL_CONFIG_CAMPO_VAL ObterEcomRelConfigCampoValorPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_CAMPO_VALs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirEcomRelConfigCampoValor(ECOM_REL_CONFIG_CAMPO_VAL configCampoVal)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_REL_CONFIG_CAMPO_VALs.InsertOnSubmit(configCampoVal);
                Intradb.SubmitChanges();

                return configCampoVal.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEcomRelConfigCampoValor(ECOM_REL_CONFIG_CAMPO_VAL configCampoVal)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_REL_CONFIG_CAMPO_VAL _configCampoVal = ObterEcomRelConfigCampoValorPorCodigo(configCampoVal.CODIGO);

            if (_configCampoVal != null)
            {
                _configCampoVal.CODIGO = configCampoVal.CODIGO;
                _configCampoVal.ECOM_REL_CONFIG_CAMPO = configCampoVal.ECOM_REL_CONFIG_CAMPO;
                _configCampoVal.VALOR = configCampoVal.VALOR;
                _configCampoVal.VALOR_DESC = configCampoVal.VALOR_DESC;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEcomRelConfigCampoValor(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_REL_CONFIG_CAMPO_VAL configCampoVal = ObterEcomRelConfigCampoValorPorCodigo(codigo);
                if (configCampoVal != null)
                {
                    Intradb.ECOM_REL_CONFIG_CAMPO_VALs.DeleteOnSubmit(configCampoVal);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_REL_CONFIG_DEPARA> ObterEcomRelConfigDEPARA()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_DEPARAs
                    select m).ToList();
        }
        public ECOM_REL_CONFIG_DEPARA ObterEcomRelConfigDEPARAPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_DEPARAs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public ECOM_REL_CONFIG_DEPARA ObterEcomRelConfigDEPARARel(int codigoEntrada, int codigoSaida)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_REL_CONFIG_DEPARAs
                    where m.ECOM_REL_CONFIG_DE == codigoEntrada && m.ECOM_REL_CONFIG_PARA == codigoSaida
                    select m).SingleOrDefault();
        }
        public int InserirEcomRelConfigDEPARA(ECOM_REL_CONFIG_DEPARA configDEPARA)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_REL_CONFIG_DEPARAs.InsertOnSubmit(configDEPARA);
                Intradb.SubmitChanges();

                return configDEPARA.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEcomRelConfigDEPARA(ECOM_REL_CONFIG_DEPARA configDEPARA)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_REL_CONFIG_DEPARA _configDEPARA = ObterEcomRelConfigDEPARAPorCodigo(configDEPARA.CODIGO);

            if (_configDEPARA != null)
            {
                _configDEPARA.CODIGO = configDEPARA.CODIGO;
                _configDEPARA.ECOM_REL_CONFIG_DE = configDEPARA.ECOM_REL_CONFIG_DE;
                _configDEPARA.ECOM_REL_CONFIG_PARA = configDEPARA.ECOM_REL_CONFIG_PARA;
                _configDEPARA.DATA_INCLUSAO = configDEPARA.DATA_INCLUSAO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEcomRelConfigDEPARA(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_REL_CONFIG_DEPARA configDEPARA = ObterEcomRelConfigDEPARAPorCodigo(codigo);
                if (configDEPARA != null)
                {
                    Intradb.ECOM_REL_CONFIG_DEPARAs.DeleteOnSubmit(configDEPARA);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_PRODUTO_RELACIONADO> ObterEcomRelacionado()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADOs
                    select m).ToList();
        }
        public List<ECOM_PRODUTO_RELACIONADO> ObterEcomRelacionadoPorCodigoPai(int codigoEcomPai)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADOs
                    where m.ECOM_PRODUTO == codigoEcomPai
                    orderby m.PRIORIDADE
                    select m).ToList();
        }
        public ECOM_PRODUTO_RELACIONADO ObterEcomRelacionadoPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirEcomRelacionado(ECOM_PRODUTO_RELACIONADO prodRel)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PRODUTO_RELACIONADOs.InsertOnSubmit(prodRel);
                Intradb.SubmitChanges();

                return prodRel.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEcomRelacionado(ECOM_PRODUTO_RELACIONADO prodRel)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PRODUTO_RELACIONADO _prodRel = ObterEcomRelacionadoPorCodigo(prodRel.CODIGO);

            if (_prodRel != null)
            {
                _prodRel.CODIGO = prodRel.CODIGO;
                _prodRel.ECOM_PRODUTO = prodRel.ECOM_PRODUTO;
                _prodRel.ECOM_PRODUTO_REL = prodRel.ECOM_PRODUTO_REL;
                _prodRel.TIPO = prodRel.TIPO;
                _prodRel.PRIORIDADE = prodRel.PRIORIDADE;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEcomRelacionado(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_PRODUTO_RELACIONADO prodrel = ObterEcomRelacionadoPorCodigo(codigo);
                if (prodrel != null)
                {
                    Intradb.ECOM_PRODUTO_RELACIONADOs.DeleteOnSubmit(prodrel);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirEcomRelacionadoPorCodigoPai(int codigoEcomPai)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var prodRels = ObterEcomRelacionadoPorCodigoPai(codigoEcomPai);
                foreach (var p in prodRels)
                {
                    if (p != null)
                    {
                        Intradb.ECOM_PRODUTO_RELACIONADOs.DeleteOnSubmit(p);
                        Intradb.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_PRODUTO_RELACIONADO_TEMP> ObterEcomRelacionadoTemp()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADO_TEMPs
                    select m).ToList();
        }
        public List<ECOM_PRODUTO_RELACIONADO_TEMP> ObterEcomRelacionadoTempPorCodigoPai(int codigoEcomPai)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADO_TEMPs
                    where m.ECOM_PRODUTO == codigoEcomPai
                    orderby m.PRIORIDADE
                    select m).ToList();
        }
        public ECOM_PRODUTO_RELACIONADO_TEMP ObterEcomRelacionadoTempPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADO_TEMPs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }

        public ECOM_PRODUTO_RELACIONADO_TEMP ObterEcomRelacionadoTempParaSKU(int codigoEcomPai, string tipo, int prioridade, int posicao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADO_TEMPs
                    where m.ECOM_PRODUTO == codigoEcomPai
                    && m.TIPO == tipo
                    && m.PRIORIDADE == prioridade
                    && (m.POSICAO == posicao || m.POSICAO == 0)
                    orderby m.POSICAO descending
                    select m).FirstOrDefault();
        }

        public int InserirEcomRelacionadoTemp(ECOM_PRODUTO_RELACIONADO_TEMP prodRel)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PRODUTO_RELACIONADO_TEMPs.InsertOnSubmit(prodRel);
                Intradb.SubmitChanges();

                return prodRel.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEcomRelacionadoTemp(ECOM_PRODUTO_RELACIONADO_TEMP prodRel)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PRODUTO_RELACIONADO_TEMP _prodRel = ObterEcomRelacionadoTempPorCodigo(prodRel.CODIGO);

            if (_prodRel != null)
            {
                _prodRel.CODIGO = prodRel.CODIGO;
                _prodRel.ECOM_PRODUTO = prodRel.ECOM_PRODUTO;
                _prodRel.ECOM_PRODUTO_REL = prodRel.ECOM_PRODUTO_REL;
                _prodRel.TIPO = prodRel.TIPO;
                _prodRel.PRIORIDADE = prodRel.PRIORIDADE;
                _prodRel.TOT_REL = prodRel.TOT_REL;
                _prodRel.FIXO = prodRel.FIXO;
                _prodRel.POSICAO = prodRel.POSICAO;
                _prodRel.SKU_MANUAL = prodRel.SKU_MANUAL;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEcomRelacionadoTemp(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_PRODUTO_RELACIONADO_TEMP prodrel = ObterEcomRelacionadoTempPorCodigo(codigo);
                if (prodrel != null)
                {
                    Intradb.ECOM_PRODUTO_RELACIONADO_TEMPs.DeleteOnSubmit(prodrel);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ECOM_PRODUTO_RELACIONADO_TEMP_EXC> ObterEcomRelacionadoTempExcPorCodigoPai(int codigoEcomPai)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_RELACIONADO_TEMP_EXCs
                    where m.ECOM_PRODUTO == codigoEcomPai
                    select m).ToList();
        }
        public void ExcluirEcomRelacionadoTempExc(int codigoEcomPai)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var ecomRel = ObterEcomRelacionadoTempExcPorCodigoPai(codigoEcomPai);

                foreach (var prodrel in ecomRel)
                {
                    //ECOM_PRODUTO_RELACIONADO_TEMP prodrel = ObterEcomRelacionadoTempPorCodigo(codigo);
                    if (prodrel != null)
                    {
                        Intradb.ECOM_PRODUTO_RELACIONADO_TEMP_EXCs.DeleteOnSubmit(prodrel);
                        Intradb.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_ECOM_REL_CONFIG_FILTRO_PORTABELAResult> ObterEcomRelConfigFiltroValores(int codigoConfigFiltro, string griffe, string grupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_REL_CONFIG_FILTRO_PORTABELA(codigoConfigFiltro, griffe, grupoProduto) select m).ToList();
        }

        public SP_OBTER_ECOM_REL_PRODUTO_CONFIGResult ObterConfigDoProduto(string produto, string cor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_REL_PRODUTO_CONFIG(produto, cor) select m).SingleOrDefault();
        }
        public List<SP_OBTER_ECOM_REL_PRODUTO_CONFIG_RELResult> ObterProdutoRelacionadoPorCodigoConfig(string produto, string cor, int codigoConfigDe)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_REL_PRODUTO_CONFIG_REL(produto, cor, codigoConfigDe) select m).ToList();
        }

        public SP_EXCLUIR_PEDIDO_ECOMResult ExcluirPedidoEcom(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_EXCLUIR_PEDIDO_ECOM(pedido) select m).SingleOrDefault();
        }

        public ECOM_PEDIDO_APROVACAO_HIST ObterEcomAprovacaoHist(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_APROVACAO_HISTs
                    where m.PEDIDO == pedido
                    select m).SingleOrDefault();
        }
        public string InserirEcomAprovacaoHist(ECOM_PEDIDO_APROVACAO_HIST histAprovacao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PEDIDO_APROVACAO_HISTs.InsertOnSubmit(histAprovacao);
                Intradb.SubmitChanges();

                return histAprovacao.PEDIDO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirEcomAprovacaoHist(string pedido)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_PEDIDO_APROVACAO_HIST aprovHist = ObterEcomAprovacaoHist(pedido);
                if (aprovHist != null)
                {
                    Intradb.ECOM_PEDIDO_APROVACAO_HISTs.DeleteOnSubmit(aprovHist);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ECOM_PEDIDO_MAG ObterPedidoMag(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAGs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    select m).SingleOrDefault();
        }
        public void AtualizarPedidoMag(ECOM_PEDIDO_MAG pedidoMag)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PEDIDO_MAG _pedidoMag = ObterPedidoMag(pedidoMag.PEDIDO_EXTERNO);

            if (_pedidoMag != null)
            {

                _pedidoMag.FRAUDE = pedidoMag.FRAUDE;
                _pedidoMag.PROBLEMA = pedidoMag.PROBLEMA;
                _pedidoMag.DESC_PROBLEMA = pedidoMag.DESC_PROBLEMA;
                _pedidoMag.OBS_RET_LOJA = pedidoMag.OBS_RET_LOJA;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ECOM_PEDIDO_MAG> ObterPedidoMagPorClienteAtacado(string clienteAtacado)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAGs
                    where m.CLIENTE_ATACADO == clienteAtacado
                    orderby m.DATA_CRIACAO
                    select m).ToList();
        }

        public ECOM_PEDIDO_MAG_STATUS ObterPedidoMagStatus(string codigoStatus)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_STATUS
                    where m.CODIGO == codigoStatus
                    select m).SingleOrDefault();
        }

        public ECOM_PEDIDO_MAG_PGTO ObterPedidoMagPagto(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_PGTOs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    select m).SingleOrDefault();
        }

        public List<ECOM_PEDIDO_MAG_PRODUTO> ObterPedidoMagProduto(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_PRODUTOs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    orderby m.ITEM_ID
                    select m).ToList();
        }

        public List<ECOM_PEDIDO_MAG_HIST> ObterPedidoMagHistorico(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_HISTs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    orderby m.DATA descending
                    select m).ToList();
        }

        public ECOM_PEDIDO_PAGARME ObterPedidoMagPagarme(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_PAGARMEs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    select m).SingleOrDefault();
        }

        public ECOM_PEDIDO_MAG_ESTORNO ObterPedidoMagEstorno(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_ESTORNOs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    select m).SingleOrDefault();
        }

        public bool InserirPedidoMagEstorno(ECOM_PEDIDO_MAG_ESTORNO magEstorno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PEDIDO_MAG_ESTORNOs.InsertOnSubmit(magEstorno);
                Intradb.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPedidoMagEstorno(ECOM_PEDIDO_MAG_ESTORNO magEstorno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PEDIDO_MAG_ESTORNO _novo = ObterPedidoMagEstorno(magEstorno.PEDIDO_EXTERNO);

            if (_novo != null)
            {
                _novo.AJUSTE_MANUTENCAO = magEstorno.AJUSTE_MANUTENCAO;
                _novo.VALOR_REEMBOLSO = magEstorno.VALOR_REEMBOLSO;
                _novo.NOME = magEstorno.NOME;
                _novo.CPF = magEstorno.CPF;
                _novo.BANCO = magEstorno.BANCO;
                _novo.AGENCIA = magEstorno.AGENCIA;
                _novo.CONTA = magEstorno.CONTA;
                _novo.TIPO_CONTA = magEstorno.TIPO_CONTA;
                _novo.MOTIVO = magEstorno.MOTIVO;


                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public SP_OBTER_ECOM_NF_PORPEDIDOResult ObterEcomNFporPedido(string pedido)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return (from mv in LINXdb.SP_OBTER_ECOM_NF_PORPEDIDO(pedido) select mv).SingleOrDefault();
        }

        public List<ECOM_LISTANEGRA> ObterListaNegra()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_LISTANEGRAs
                    select m).ToList();
        }
        public ECOM_LISTANEGRA ObterListaNegraPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_LISTANEGRAs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public ECOM_LISTANEGRA ObterListaNegraPorValor(string valor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_LISTANEGRAs
                    where m.VALOR.ToLower().Trim() == valor.ToLower().Trim()
                    select m).SingleOrDefault();
        }
        public int InserirListaNegra(ECOM_LISTANEGRA ln)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_LISTANEGRAs.InsertOnSubmit(ln);
                Intradb.SubmitChanges();

                return ln.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirListaNegra(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_LISTANEGRA ln = ObterListaNegraPorCodigo(codigo);
                if (ln != null)
                {
                    Intradb.ECOM_LISTANEGRAs.DeleteOnSubmit(ln);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_ECOM_CLIENTE_CRMResult> ObterEcomClienteCRM(int ano, int mesDe, int mesAte, string cliente, string cpf, string email)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_CLIENTE_CRM(ano, mesDe, mesAte, cliente, cpf, email) select m).ToList();
        }
        public List<SP_OBTER_ECOM_CLIENTE_CRM_DETResult> ObterEcomClienteCRMDET(string clienteAtacado)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_CLIENTE_CRM_DET(clienteAtacado) select m).ToList();
        }
        public List<SP_OBTER_ECOM_CLIENTE_CRM_DET_PRODResult> ObterEcomClienteCRMDETPROD(string clienteAtacado, int ano, int mes)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_CLIENTE_CRM_DET_PROD(clienteAtacado, ano, mes) select m).ToList();
        }

        public SP_OBTER_ECOM_CLIENTE_CRM_DADOSResult ObterEcomClienteCRMDados(string clienteAtacado)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_CLIENTE_CRM_DADOS(clienteAtacado) select m).FirstOrDefault();
        }

        public List<SP_OBTER_ECOM_CLIENTE_CRM_MAISCOMPRAResult> ObterEcomClienteCRMMaisCompra(string clienteAtacado)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_CLIENTE_CRM_MAISCOMPRA(clienteAtacado) select m).ToList();
        }
        public List<SP_OBTER_ECOM_CLIENTE_CRM_ULTIMOSResult> ObterEcomClienteCRMUltimasCompras(string clienteAtacado)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.SP_OBTER_ECOM_CLIENTE_CRM_ULTIMOS(clienteAtacado) select m).ToList();
        }

        public List<SP_OBTER_ECOM_MAIS_VENDIDOSResult> ObterMaisVendidos(DateTime dataIni, DateTime dataFim, string colecao, string codigoFilial, string griffe)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from mv in Intradb.SP_OBTER_ECOM_MAIS_VENDIDOS(dataIni, dataFim, codigoFilial, colecao, griffe) select mv).ToList();
        }

        public ECOM_PEDIDO_PGTO ObterPedidoPgto(string pedidoLinx)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_PGTOs
                    where m.PEDIDO == pedidoLinx
                    select m).SingleOrDefault();
        }
        public ECOM_PEDIDO_PGTO ObterPedidoPgtoPorPedidoExterno(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_PGTOs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    select m).SingleOrDefault();
        }
        public void AtualizarPedidoPgto(ECOM_PEDIDO_PGTO ecomPagto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PEDIDO_PGTO _pedidoPgto = ObterPedidoPgto(ecomPagto.PEDIDO);

            if (_pedidoPgto != null)
            {

                _pedidoPgto.PONTOS_HANDCLUB = ecomPagto.PONTOS_HANDCLUB;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public SP_OBTER_ECOM_PRODUTO_BLOCOCONT4Result ObterEcomProdutoBlocoTOT4(int ecomBlocoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from mv in Intradb.SP_OBTER_ECOM_PRODUTO_BLOCOCONT4(ecomBlocoProduto) select mv).SingleOrDefault();
        }

        public SP_ATUALIZAR_ECOM_PRODUTOBLOCO_RANDResult AtualizarEcomBlocoProdutoRandom(int ecomBlocoProduto, int ecomGrupoProduto, int tipo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from mv in Intradb.SP_ATUALIZAR_ECOM_PRODUTOBLOCO_RAND(ecomBlocoProduto, ecomGrupoProduto, tipo) select mv).SingleOrDefault();
        }

        public List<ECOM_EMAIL_BLOCO> ObterEmailBloco()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_BLOCOs
                    select m).ToList();
        }
        public ECOM_EMAIL_BLOCO ObterEmailBlocoPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_BLOCOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public List<ECOM_EMAIL_BLOCO> ObterEmailBlocoPorFixo(char fixo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_BLOCOs
                    where m.FIXO == fixo
                    orderby m.ORDEM
                    select m).ToList();
        }

        public List<ECOM_EMAIL> ObterEmail()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAILs
                    orderby m.DATA_CRIACAO descending
                    select m).ToList();
        }
        public ECOM_EMAIL ObterEmailPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAILs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirEmail(ECOM_EMAIL email)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_EMAILs.InsertOnSubmit(email);
                Intradb.SubmitChanges();

                return email.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEmail(ECOM_EMAIL email)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_EMAIL _novo = ObterEmailPorCodigo(email.CODIGO);

            if (_novo != null)
            {
                _novo.NOME = email.NOME;
                _novo.DATA_CRIACAO = email.DATA_CRIACAO;
                _novo.DATA_FINAL = email.DATA_FINAL;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEmail(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_EMAIL email = ObterEmailPorCodigo(codigo);
                if (email != null)
                {
                    Intradb.ECOM_EMAILs.DeleteOnSubmit(email);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_EMAIL_CORPO> ObterEmailCorpo()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_CORPOs
                    select m).ToList();
        }
        public List<ECOM_EMAIL_CORPO> ObterEmailCorpoPorCodigoEmail(int codigoEmail)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_CORPOs
                    where m.ECOM_EMAIL == codigoEmail
                    orderby m.ORDEM
                    select m).ToList();
        }
        public int ObterEmailCorpoUltimaOrdem(int codigoEmail)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            var emailCorpo = (from m in Intradb.ECOM_EMAIL_CORPOs
                              where m.ECOM_EMAIL == codigoEmail
                              && m.ECOM_EMAIL_BLOCO1.FIXO == 'N'
                              orderby m.ORDEM descending
                              select m).FirstOrDefault();

            if (emailCorpo == null)
                return 1;

            return (emailCorpo.ORDEM + 1);
        }
        public ECOM_EMAIL_CORPO ObterEmailCorpoPorOrdem(int codigoEmail, int ordem)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_CORPOs
                    where m.ECOM_EMAIL == codigoEmail
                    && m.ORDEM == ordem
                    select m).FirstOrDefault();
        }
        public ECOM_EMAIL_CORPO ObterEmailCorpoPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_CORPOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirEmailCorpo(ECOM_EMAIL_CORPO emailCorpo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_EMAIL_CORPOs.InsertOnSubmit(emailCorpo);
                Intradb.SubmitChanges();

                return emailCorpo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEmailCorpo(ECOM_EMAIL_CORPO emailCorpo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_EMAIL_CORPO _novo = ObterEmailCorpoPorCodigo(emailCorpo.CODIGO);

            if (_novo != null)
            {
                _novo.ORDEM = emailCorpo.ORDEM;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEmailCorpo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_EMAIL_CORPO emailCorpo = ObterEmailCorpoPorCodigo(codigo);
                if (emailCorpo != null)
                {
                    Intradb.ECOM_EMAIL_CORPOs.DeleteOnSubmit(emailCorpo);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_EMAIL_CORPO_LINK> ObterEmailCorpoLink(int codigoEmailCorpo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_CORPO_LINKs
                    where m.ECOM_EMAIL_CORPO == codigoEmailCorpo
                    orderby m.ORDEM
                    select m).ToList();
        }
        public ECOM_EMAIL_CORPO_LINK ObterEmailCorpoLink(int codigoEmailCorpo, int ordem)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_CORPO_LINKs
                    where m.ECOM_EMAIL_CORPO == codigoEmailCorpo
                    && m.ORDEM == ordem
                    select m).SingleOrDefault();
        }
        public ECOM_EMAIL_CORPO_LINK ObterEmailCorpoLinkPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_EMAIL_CORPO_LINKs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirEmailCorpoLink(ECOM_EMAIL_CORPO_LINK corpoLink)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_EMAIL_CORPO_LINKs.InsertOnSubmit(corpoLink);
                Intradb.SubmitChanges();

                return corpoLink.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarEmailCorpoLink(ECOM_EMAIL_CORPO_LINK corpoLink)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_EMAIL_CORPO_LINK _novo = ObterEmailCorpoLinkPorCodigo(corpoLink.CODIGO);

            if (_novo != null)
            {
                _novo.ECOM_EMAIL_CORPO = corpoLink.ECOM_EMAIL_CORPO;
                _novo.LINK_REDIRECT = corpoLink.LINK_REDIRECT;
                _novo.LINK_IMAGEM = corpoLink.LINK_IMAGEM;
                _novo.TEXTO = corpoLink.TEXTO;
                _novo.PRECO = corpoLink.PRECO;
                _novo.PRECO_PROMO = corpoLink.PRECO_PROMO;
                _novo.ALT = corpoLink.ALT;
                _novo.ORDEM = corpoLink.ORDEM;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirEmailCorpoLink(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_EMAIL_CORPO_LINK corpoLink = ObterEmailCorpoLinkPorCodigo(codigo);
                if (corpoLink != null)
                {
                    Intradb.ECOM_EMAIL_CORPO_LINKs.DeleteOnSubmit(corpoLink);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ECOM_PRODUTO_CAT> ObterProdutoCategoria()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_CATs
                    select m).ToList();
        }
        public List<ECOM_PRODUTO_CAT> ObterProdutoCategoria(int codigoEcomProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_CATs
                    where m.ECOM_PRODUTO == codigoEcomProduto
                    orderby m.ECOM_GRUPO_PRODUTO1.GRUPO
                    select m).ToList();
        }
        public List<ECOM_PRODUTO_CAT> ObterProdutoCategoriaPorCategoria(int codigoEcomGrupo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_CATs
                    where m.ECOM_GRUPO_PRODUTO == codigoEcomGrupo
                    orderby m.ECOM_GRUPO_PRODUTO1.GRUPO
                    select m).ToList();
        }
        public ECOM_PRODUTO_CAT ObterProdutoCategoria(int codigoEcomProduto, int codigoGrupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_CATs
                    where m.ECOM_PRODUTO == codigoEcomProduto
                    && m.ECOM_GRUPO_PRODUTO == codigoGrupoProduto
                    select m).SingleOrDefault();
        }
        public void InserirProdutoCategoria(ECOM_PRODUTO_CAT produtoCat)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PRODUTO_CATs.InsertOnSubmit(produtoCat);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProdutoCategoria(int codigoEcomProduto, int codigoGrupoProduto)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_PRODUTO_CAT produtoCat = ObterProdutoCategoria(codigoEcomProduto, codigoGrupoProduto);
                if (produtoCat != null)
                {
                    Intradb.ECOM_PRODUTO_CATs.DeleteOnSubmit(produtoCat);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ECOM_PEDIDO_MAG_CUPOM> ObterPedidoCupom()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_CUPOMs
                    orderby m.DATA_CRIACAO ascending
                    select m).ToList();
        }
        public List<ECOM_PEDIDO_MAG_CUPOM> ObterPedidoCupom(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_CUPOMs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    orderby m.DATA_CRIACAO ascending
                    select m).ToList();
        }
        public ECOM_PEDIDO_MAG_CUPOM ObterPedidoCupomPorCodigo(string pedidoExterno, string cupom, int? versao = null)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_MAG_CUPOMs
                    where
                        m.PEDIDO_EXTERNO == pedidoExterno
                        && m.CUPOM == cupom
                        && m.VERSAO == versao
                    select m).SingleOrDefault();
        }
        public int InserirPedidoCupom(ECOM_PEDIDO_MAG_CUPOM pedidoCupom)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PEDIDO_MAG_CUPOMs.InsertOnSubmit(pedidoCupom);
                Intradb.SubmitChanges();

                return pedidoCupom.ID_CUPOM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPedidoCupom(ECOM_PEDIDO_MAG_CUPOM pedidoCupom)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PEDIDO_MAG_CUPOM _pedidoCupom = ObterPedidoCupomPorCodigo(pedidoCupom.PEDIDO_EXTERNO, pedidoCupom.CUPOM, pedidoCupom.VERSAO);

            if (_pedidoCupom != null)
            {
                _pedidoCupom.DATA_EXCLUSAO = pedidoCupom.DATA_EXCLUSAO;
                _pedidoCupom.MOTIVO = pedidoCupom.MOTIVO;
                _pedidoCupom.NF_ENTRADA = pedidoCupom.NF_ENTRADA;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirPedidoCupom(string pedidoExterno, string cupom, int? versao = null)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_PEDIDO_MAG_CUPOM pedidoCupom = ObterPedidoCupomPorCodigo(pedidoExterno, cupom, versao);
                if (pedidoCupom != null)
                {
                    Intradb.ECOM_PEDIDO_MAG_CUPOMs.DeleteOnSubmit(pedidoCupom);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ECOM_PEDIDO_ENDERECO ObterPedidoEnderecoEtiqueta(string pedidoExterno)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PEDIDO_ENDERECOs
                    where m.PEDIDO_EXTERNO == pedidoExterno
                    select m).SingleOrDefault();
        }
        public void AtualizarPedidoEnderecoEtiqueta(ECOM_PEDIDO_ENDERECO etiEndereco)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            ECOM_PEDIDO_ENDERECO _etiEndereco = ObterPedidoEnderecoEtiqueta(etiEndereco.PEDIDO_EXTERNO);

            if (_etiEndereco != null)
            {
                _etiEndereco.CEP = etiEndereco.CEP;
                _etiEndereco.ENDERECO = etiEndereco.ENDERECO;
                _etiEndereco.NUMERO = etiEndereco.NUMERO;
                _etiEndereco.COMPLEMENTO = etiEndereco.COMPLEMENTO;
                _etiEndereco.BAIRRO = etiEndereco.BAIRRO;
                _etiEndereco.CIDADE = etiEndereco.CIDADE;
                _etiEndereco.UF = etiEndereco.UF;
                _etiEndereco.USUARIO = etiEndereco.USUARIO;
                _etiEndereco.DATA_ALTERACAO = etiEndereco.DATA_ALTERACAO;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ECOM_AJUDA_VIDEO> ObterAjudaVideo()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_AJUDA_VIDEOs
                    orderby m.CODIGO ascending
                    select m).ToList();
        }
        public ECOM_AJUDA_VIDEO ObterAjudaVideoPorCodigo(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_AJUDA_VIDEOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }


        public ECOM_PRODUTO_LINK ObterProdutoLink(string sku, string linkedSku, string linkedType)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_PRODUTO_LINKs
                    where m.SKU == sku
                    && m.LINKED_SKU == linkedSku
                    && m.LINKED_TYPE == linkedType
                    select m).SingleOrDefault();
        }
        public void InserirProdutoLink(ECOM_PRODUTO_LINK link)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_PRODUTO_LINKs.InsertOnSubmit(link);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProdutoLink(string sku, string linkedSku, string linkedType)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                ECOM_PRODUTO_LINK produtoLink = ObterProdutoLink(sku, linkedSku, linkedType);
                if (produtoLink != null)
                {
                    Intradb.ECOM_PRODUTO_LINKs.DeleteOnSubmit(produtoLink);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ECOM_WEBSITE_SKU> ObterWebSiteSku(string sku)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ECOM_WEBSITE_SKUs
                    where m.SKU == sku
                    orderby m.ECOM_WEBSITE1.NAME
                    select m).ToList();
        }
        public void InserirWebSiteSku(ECOM_WEBSITE_SKU webSiteSku)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ECOM_WEBSITE_SKUs.InsertOnSubmit(webSiteSku);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
