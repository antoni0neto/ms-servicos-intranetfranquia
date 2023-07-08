using DAL;
using Newtonsoft.Json;
using Relatorios.mod_ecomv2.mag2.dtos;
using Relatorios.mod_ecomv2.mag2.utils;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Relatorios.mod_ecomv2.mag2
{
    public class MagentoV2
    {
        private readonly EcomController _eController;
        private readonly BaseController _baseController;
        private int _codigoUsuario;
        private string _pathSistema;
        private string _token;
        private readonly RestClient _client;

        public MagentoV2()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;

            _client = new RestClient(Constante.urlBaseMagentoV2);
            _token = Constante.tokenMagentoV2;

            _eController = new EcomController();
            _baseController = new BaseController();
        }
        public MagentoV2(int codigoUsuario, string pathSistema)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;

            _codigoUsuario = codigoUsuario;
            _pathSistema = pathSistema;

            _client = new RestClient(Constante.urlBaseMagentoV2);
            _token = Constante.tokenMagentoV2;

            _eController = new EcomController();
            _baseController = new BaseController();
        }

        [Obsolete]
        private string ObterToken(string userMag = "", string passMag = "")
        {
            //var request = new RestRequest("integration/admin/token", Method.POST);
            //request.AddHeader("content-type", "application/json");

            //if (userMag == "")
            //    userMag = Constante.userMagento;
            //if (passMag == "")
            //    passMag = Constante.apiKeyMagento;

            //var data = new
            //{
            //    username = userMag,
            //    password = passMag
            //};

            //request.AddParameter(
            //        "application/json",
            //        JsonConvert.SerializeObject(data),
            //        ParameterType.RequestBody);

            //var response = _client.Execute(request);

            //if (response.StatusCode != HttpStatusCode.OK)
            //    throw new Exception("Não foi possível realizar a autenticação no Magento 2");

            //return response.Content.Replace("\"", "");

            return "rio2lhaci809n2lhrqyxqsfqpgdjsxv8";
        }

        public bool SalvarProdutoEstoque(int codigoEcomProduto)
        {
            //Obter Produto Configuravel
            var ecomProdutoConfig = _eController.ObterMagentoProduto(codigoEcomProduto);
            var webSites = _eController.ObterWebSiteSku(ecomProdutoConfig.SKU);

            //Obter Qtdes para Estoque
            var ecomProdutoEstoque = _eController.ObterProdutoLinxLiberacao(ecomProdutoConfig.COLECAO, "", "", "", ecomProdutoConfig.CODIGO).FirstOrDefault();

            var idProdutoMagConfig = 0;
            if (ecomProdutoConfig.ID_PRODUTO_MAG != null)
                idProdutoMagConfig = Convert.ToInt32(ecomProdutoConfig.ID_PRODUTO_MAG);

            Hashtable hashEstoque = new Hashtable();
            if (ecomProdutoEstoque.QTDE_1 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_1, ecomProdutoEstoque.QTDE_1);
            if (ecomProdutoEstoque.QTDE_2 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_2, ecomProdutoEstoque.QTDE_2);
            if (ecomProdutoEstoque.QTDE_3 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_3, ecomProdutoEstoque.QTDE_3);
            if (ecomProdutoEstoque.QTDE_4 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_4, ecomProdutoEstoque.QTDE_4);
            if (ecomProdutoEstoque.QTDE_5 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_5, ecomProdutoEstoque.QTDE_5);
            if (ecomProdutoEstoque.QTDE_6 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_6, ecomProdutoEstoque.QTDE_6);
            if (ecomProdutoEstoque.QTDE_7 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_7, ecomProdutoEstoque.QTDE_7);
            if (ecomProdutoEstoque.QTDE_8 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_8, ecomProdutoEstoque.QTDE_8);
            if (ecomProdutoEstoque.QTDE_9 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_9, ecomProdutoEstoque.QTDE_9);

            var skuProdutosSimples = new List<string>();
            var codigosEcomProdutoSimples = new List<int>();
            var tamanho = "";
            var qtdeEstoque = "";

            foreach (DictionaryEntry entry in hashEstoque)
            {
                tamanho = entry.Key.ToString();

                if (tamanho == "UN")
                    tamanho = "Único";
                else if (tamanho == "2G")
                    tamanho = "GG";
                else if (tamanho == "3G")
                    tamanho = "XG";

                qtdeEstoque = entry.Value.ToString();

                // verificar se o produto simples ja esta cadastrado no magento
                var produtoSimplesExiste = _eController.ObterMagentoProdutoSimplesTamanho(idProdutoMagConfig, tamanho);
                // se produto simples nao existe, insere e adiciona estoque inicial
                if (produtoSimplesExiste == null)
                {
                    // Insere Produto Simples no Magento + Estoque Inicial
                    var productDetailSimple = InserirProdutoMagentoV2(true, ecomProdutoConfig, tamanho, qtdeEstoque);

                    //Insere Produto Simples na Intranet
                    var produtoSimplesN = CriarCopiaECOMPRODUTO(ecomProdutoConfig, productDetailSimple.Id, tamanho);
                    var codigoEcom = _eController.InserirMagentoProduto(produtoSimplesN);
                    skuProdutosSimples.Add(productDetailSimple.Sku);
                    codigosEcomProdutoSimples.Add(codigoEcom);

                    foreach (var webSite in webSites)
                        AssociarProdutosWebsiteMagentoV2(produtoSimplesN.SKU, webSite.ECOM_WEBSITE1.ID);

                }
                else
                {
                    // se produto ja existe, faz somente a reposicao do estoque
                    var estoqueAtual = ObterEstoqueMagentoV2(produtoSimplesExiste);
                    AtualizarEstoqueSimplesMagentoV2(produtoSimplesExiste, Convert.ToInt32(qtdeEstoque) + estoqueAtual.Qty);
                }
            }

            //Inserir configuravel
            if (idProdutoMagConfig == 0)
            {
                // Insere produto configuravel
                var produtoconfig = InserirProdutoMagentoV2(false, ecomProdutoConfig, "", "0");
                idProdutoMagConfig = produtoconfig.Id;

                // Insere Options para relacionar com produto simples
                InserirProdutoConfigOptionsMagentoV2(ecomProdutoConfig, "143", "Tamanho");

                // Marcar Ids das Imagens do Magento no produto configuravel
                var fotos = InserirImagensMagentoV2(ecomProdutoConfig);
                ecomProdutoConfig = AtualizarFotosIdsMag(ecomProdutoConfig, fotos);

                //Associar Sites
                foreach (var webSite in webSites)
                    AssociarProdutosWebsiteMagentoV2(ecomProdutoConfig.SKU, webSite.ECOM_WEBSITE1.ID);
            }
            else
            {
                // habilitar o produto na reposição, caso esteja desabilitado
                HabilitarProdutoConfiguravelMagentoV2(ecomProdutoConfig);
            }

            //associar produtos simples
            foreach (var skuSimples in skuProdutosSimples)
                AssociarSkuSimplesMagentoV2(ecomProdutoConfig, skuSimples);

            //Atualizar path da imagem
            var fotoShoppingUrlExt = ObterPathFotoShoppingExterno(ecomProdutoConfig);
            ecomProdutoConfig.FOTO_SHOPPING_URLEXT = fotoShoppingUrlExt;

            //atualizar envio do configuravel
            ecomProdutoConfig.ID_PRODUTO_MAG = idProdutoMagConfig;
            ecomProdutoConfig.STATUS_CADASTRO = 'B';
            ecomProdutoConfig.VISIBILIDADE = "4";
            _eController.AtualizarMagentoProduto(ecomProdutoConfig);

            // associar produto configuravel no simples, na intranet (pai -> filho)
            foreach (var codigoEcomProdutoId in codigosEcomProdutoSimples)
            {
                var ecomProd = _eController.ObterMagentoProduto(codigoEcomProdutoId);
                ecomProd.ID_PRODUTO_MAG_CONFIG = idProdutoMagConfig;
                _eController.AtualizarMagentoProduto(ecomProd);
            }

            // Relacionar produtos com suas respectivas cores
            // Deve associar o novo produto a cores ja cadastradas
            // Deve associar cores ja cadastradas ao novo produto
            AssociarCoresConfig(ecomProdutoConfig.PRODUTO);

            //inserir estoque enviado deste produto
            InserirNFEstoque(ecomProdutoEstoque);

            return true;
        }
        public void AtualizarProduto(ECOM_PRODUTO ecomProdutoConfig, bool updateImage)
        {
            var prodSimples = _eController.ObterMagentoProdutoSimples(Convert.ToInt32(ecomProdutoConfig.ID_PRODUTO_MAG), ecomProdutoConfig.PRODUTO);
            foreach (var produtoSimples in prodSimples)
                AtualizarProdutoMagentoV2(produtoSimples);

            //Configuravel
            AtualizarProdutoMagentoV2(ecomProdutoConfig);

            if (updateImage)
            {
                //Atualiza imagens, posicionamento e ordem das imagens
                AtualizarImagensMagentoV2(ecomProdutoConfig);

                //Atualizar path da imagem
                var fotoShoppingUrlExt = ObterPathFotoShoppingExterno(ecomProdutoConfig);
                ecomProdutoConfig.FOTO_SHOPPING_URLEXT = fotoShoppingUrlExt;
                _eController.AtualizarMagentoProduto(ecomProdutoConfig);
            }
        }
        public ECOM_PRODUTO AtualizarFotosIdsMag(ECOM_PRODUTO ecomProduto, List<FotoMagV2> fotos)
        {
            foreach (var foto in fotos)
            {
                if (ecomProduto.FOTO_LOOK_POS != null && foto.Position == ecomProduto.FOTO_LOOK_POS)
                    ecomProduto.FOTO_LOOK_MAG = foto.EntryId.ToString();
                else if (ecomProduto.FOTO_FRENTE_CAB_POS != null && foto.Position == ecomProduto.FOTO_FRENTE_CAB_POS)
                    ecomProduto.FOTO_FRENTE_CAB_MAG = foto.EntryId.ToString();
                else if (ecomProduto.FOTO_FRENTE_SEMCAB_POS != null && foto.Position == ecomProduto.FOTO_FRENTE_SEMCAB_POS)
                    ecomProduto.FOTO_FRENTE_SEMCAB_MAG = foto.EntryId.ToString();
                else if (ecomProduto.FOTO_COSTAS_POS != null && foto.Position == ecomProduto.FOTO_COSTAS_POS)
                    ecomProduto.FOTO_COSTAS_MAG = foto.EntryId.ToString();
                else if (ecomProduto.FOTO_DETALHE_POS != null && foto.Position == ecomProduto.FOTO_DETALHE_POS)
                    ecomProduto.FOTO_DETALHE_MAG = foto.EntryId.ToString();
                else if (ecomProduto.FOTO_LADO_POS != null && foto.Position == ecomProduto.FOTO_LADO_POS)
                    ecomProduto.FOTO_LADO_MAG = foto.EntryId.ToString();
            }

            return ecomProduto;
        }
        public string ObterPathFotoShoppingExterno(ECOM_PRODUTO ecomProduto)
        {
            var produto = ObterProdutoMagentoV2(ecomProduto.SKU);
            return produto.MediaGalleryEntries?[0].File;
        }
        public void AtualizarCategoria(int categoriaId, string sku, int position)
        {
            AtualizarCategoriaMagentoV2(categoriaId, sku, position);
        }
        public void SalvarProdutoDevolucao(int codigoEcomProduto, string nfEntrada, bool defeito)
        {
            //Obter Produto Configuravel
            var ecomProdutoConfig = _eController.ObterMagentoProduto(codigoEcomProduto);

            //Obter Qtdes para Estoque
            var ecomProdutoEstoqueDevolucao = _eController.ObterProdutoLinxDevolucao(ecomProdutoConfig.CODIGO, nfEntrada).FirstOrDefault();
            if (ecomProdutoEstoqueDevolucao == null)
                throw new Exception("Produto não foi encontrado para devolução.");

            if (!defeito)
            {
                Hashtable hashEstoque = new Hashtable();
                if (ecomProdutoEstoqueDevolucao.QTDE_1 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_1, ecomProdutoEstoqueDevolucao.QTDE_1);
                if (ecomProdutoEstoqueDevolucao.QTDE_2 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_2, ecomProdutoEstoqueDevolucao.QTDE_2);
                if (ecomProdutoEstoqueDevolucao.QTDE_3 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_3, ecomProdutoEstoqueDevolucao.QTDE_3);
                if (ecomProdutoEstoqueDevolucao.QTDE_4 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_4, ecomProdutoEstoqueDevolucao.QTDE_4);
                if (ecomProdutoEstoqueDevolucao.QTDE_5 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_5, ecomProdutoEstoqueDevolucao.QTDE_5);
                if (ecomProdutoEstoqueDevolucao.QTDE_6 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_6, ecomProdutoEstoqueDevolucao.QTDE_6);
                if (ecomProdutoEstoqueDevolucao.QTDE_7 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_7, ecomProdutoEstoqueDevolucao.QTDE_7);
                if (ecomProdutoEstoqueDevolucao.QTDE_8 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_8, ecomProdutoEstoqueDevolucao.QTDE_8);
                if (ecomProdutoEstoqueDevolucao.QTDE_9 > 0)
                    hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_9, ecomProdutoEstoqueDevolucao.QTDE_9);

                //Loop nos produtos simples
                foreach (DictionaryEntry entry in hashEstoque)
                {
                    var tamanho = entry.Key.ToString();
                    var qtdeEstoque = entry.Value;

                    if (tamanho == "UN")
                        tamanho = "Único";
                    else if (tamanho == "2G")
                        tamanho = "GG";
                    else if (tamanho == "3G")
                        tamanho = "XG";

                    // Obter Produto Simples
                    var produtoSimples = _eController.ObterMagentoProdutoSimplesTamanho(ecomProdutoConfig.ID_PRODUTO_MAG.GetValueOrDefault(), tamanho);

                    // repor estoque
                    var estoqueAtual = ObterEstoqueMagentoV2(produtoSimples);
                    AtualizarEstoqueSimplesMagentoV2(produtoSimples, Convert.ToInt32(qtdeEstoque) + estoqueAtual.Qty);
                }
            }

            InserirEstoqueDevolucao(ecomProdutoEstoqueDevolucao, defeito);
        }
        public bool AtualizarPrecoPromo(List<string> skus, decimal preco, int storeId)
        {
            AtualizarPrecoPromoMagentoV2(skus, preco, storeId);
            return true;
        }
        public void ObterESalvarEstoque(int codigoUsuario)
        {
            var produtosEstoqueMagento = ObterEstoquePorSkusMagentoV2();

            // utilizo o codigo do usuario para nao concorrer com outros usuarios
            _eController.ExcluirEstoqueMagentoTemp(codigoUsuario);

            var produtosEstoque = new List<ECOM_ESTOQUE_TEMPV2>();
            foreach (var es in produtosEstoqueMagento.Items)
            {
                produtosEstoque.Add(new ECOM_ESTOQUE_TEMPV2
                {
                    SKU = es.Sku,
                    QTY = es.Quantity,
                    SOURCE_CODE = "default",
                    STATUS = (es.Status == 1),
                    USUARIO = codigoUsuario
                });

            }

            _eController.InserirEstoqueMagentoTempV2Dapper(produtosEstoque);
        }
        public void ObterESalvarEstoqueSalable(int codigoUsuario, string colecao)
        {
            var produtosEstoque = new List<ECOM_ESTOQUE_TEMPV2>();

            var produtosEstoqueMagento = ObterEstoquePorSkusMagentoV2();
            var produtosColecao = _eController.ObterMagentoProdutoSimplesCadastrado(colecao);

            // utilizo o codigo do usuario para nao concorrer com outros usuarios
            _eController.ExcluirEstoqueMagentoTemp(codigoUsuario);

            var produtosEstoqueMagentoPorColecao = produtosEstoqueMagento.Items
                                                        .Where(x => produtosColecao.Any(y => y.SKU.Trim().ToLower() == x.Sku.Trim().ToLower()));
            foreach (var es in produtosEstoqueMagentoPorColecao)
            {
                int qtdeEstoqueSalable = ObterEstoqueSalablePorSkuMagentoV2(es.Sku);

                produtosEstoque.Add(new ECOM_ESTOQUE_TEMPV2
                {
                    SKU = es.Sku,
                    QTY = es.Quantity,
                    SOURCE_CODE = "default",
                    STATUS = (es.Status == 1),
                    USUARIO = codigoUsuario,
                    SALABLE = qtdeEstoqueSalable
                });
            }

            _eController.InserirEstoqueMagentoTempV2Dapper(produtosEstoque);
        }
        public void HabilitarProdutoConfiguravel(ECOM_PRODUTO ecomProduto)
        {
            HabilitarProdutoConfiguravelMagentoV2(ecomProduto);
        }
        public bool SalvarEntrega(string pedido, string pedidoExterno, List<string> trackNumbers, CarrierCode carrierCode, bool gerarEtiqueta, bool mktplace, string chaveNFE, string msgParaCliente)
        {
            var title = "";
            var trackNumberAux = trackNumbers[0];

            if (mktplace)
            {
                title = "Marketplace";
            }
            else
            {
                if (carrierCode == CarrierCode.mandae)
                {
                    title = "Mandae";
                }
                if (carrierCode == CarrierCode.correios)
                {
                    title = "Correios";
                }
                if (carrierCode == CarrierCode.frenetshipping)
                {
                    trackNumberAux = $"https://www.loggi.com/rastreador/7e3b2131be/{trackNumbers[0]}";
                    title = "Loggi";
                }
                else
                {
                    title = "Correios";
                }
            }

            //Salvar Entrega
            var pedidoEntrega = _eController.ObterPedidoEntregaPorPedidoLinx(pedido);
            if (pedidoEntrega == null)
            {
                //obter dados do pedido
                var orderV2 = ObterPedidoMagentoV2(pedidoExterno);

                //pedido interno
                var pedidoInterno = false;
                if (carrierCode == CarrierCode.custom && (!gerarEtiqueta && !mktplace))
                {
                    title = "Retirado Loja";
                    pedidoInterno = true;

                    //finalizar pedido
                    InserirComentarioMagentoV2(orderV2.Items[0].ParentId, "complete", "Pedido Interno");
                }
                else
                {
                    //adicionar chaveNFE nos comentarios do pedido para leitura do MKP
                    var msgChave = "chave de acesso: " + chaveNFE;
                    InserirComentarioMagentoV2(orderV2.Items[0].ParentId, "complete_shipped", msgChave);
                }

                var msg = "Oi! Seu pedido foi enviado. Já já ele chega pra você!";

                var shipmentId = InserirEntregaMagentoV2(orderV2, trackNumberAux, title, carrierCode, msg);

                pedidoEntrega = new ECOM_PEDIDO_ENTREGA_MAG();
                pedidoEntrega.PEDIDO = pedido;
                pedidoEntrega.PEDIDO_EXTERNO = pedidoExterno;
                pedidoEntrega.PEDIDO_ENTREGA = shipmentId;
                pedidoEntrega.CHAVE_NFE = chaveNFE;
                pedidoEntrega.CARRIER = carrierCode.ToString();
                _eController.InserirPedidoEntrega(pedidoEntrega);

                if (!pedidoInterno)
                {
                    var entregaTN = new ECOM_PEDIDO_ENTREGA_MAG_TRACK();
                    entregaTN.TRACK_NUMBER = trackNumbers[0];
                    entregaTN.PEDIDO = pedido;
                    _eController.InserirPedidoEntregaTrack(entregaTN);
                }
            }

            //Atualizar Envio do Email
            pedidoEntrega.EMAIL_ENVIADO = true;
            pedidoEntrega.DATA_ENVIO = DateTime.Now;
            _eController.AtualizarPedidoEntrega(pedidoEntrega);

            return true;
        }
        public bool AtualizarPedidoCompleto(string pedidoExterno)
        {
            var orderV2 = ObterPedidoMagentoV2(pedidoExterno);
            InserirComentarioMagentoV2(orderV2.Items[0].ParentId, "complete", "");

            return true;
        }

        public OrderV2RuleCouponResult GerarCupom(string codigoCupom, double valorCupom, bool freteGratis, string[] cats = null)
        {
            var rule = InserirRegraCupomMagentoV2(codigoCupom, valorCupom, freteGratis);
            var coupon = InserirCupomMagentoV2(rule.RuleId, codigoCupom);
            return coupon;
        }
        public bool ExcluirCupom(int couponId, int ruleId)
        {
            RemoverCupomMagentoV2(couponId);
            RemoverRegraCupomMagentoV2(ruleId);

            return true;
        }

        public void AssociarProdutoComOutrosProdutos(string sku, List<string> linkedSkus, string linkedType)
        {
            AssociarProdutosEComOutrosProdutoMagentoV2(sku, linkedSkus, linkedType);
        }
        public void DesassociarProdutoComOutrosProdutos(string sku)
        {
            var produto = ObterProdutoMagentoV2(sku);
            foreach (var p in produto.ProductLinks)
            {
                if (p.LinkType != "brother")
                    DesassociarProdutosEComOutrosProdutoMagentoV2(sku, p.LinkedProductSku, p.LinkType);
            }
        }

        public void AssociarCategorias(ECOM_PRODUTO ecomProduto)
        {
            //Configuravel
            AtualizarProdutoMagentoV2(ecomProduto);
        }

        public void AtualizarEstoque(ECOM_PRODUTO ecomProduto, int qty)
        {
            AtualizarEstoqueSimplesMagentoV2(ecomProduto, qty);
        }

        private void InserirEstoqueDevolucao(SP_OBTER_ECOM_PRODUTO_DEVOLUCAOResult produtoDevolvido, bool defeito)
        {
            //inserir devolucao estoque deste produto
            var devIntra = new ECOM_ESTOQUE_DEV();
            devIntra.PRODUTO = produtoDevolvido.PRODUTO;
            devIntra.COR = produtoDevolvido.COR;
            devIntra.NOME_CLIFOR = produtoDevolvido.NOME_CLIFOR;
            devIntra.FILIAL = produtoDevolvido.FILIAL;
            devIntra.NF_ENTRADA = produtoDevolvido.NF_ENTRADA;
            devIntra.SERIE_NF_ENTRADA = produtoDevolvido.SERIE_NF_ENTRADA;
            devIntra.RECEBIMENTO = produtoDevolvido.RECEBIMENTO;
            devIntra.DEFEITO = defeito;
            devIntra.DATA_ENTRADA = DateTime.Now;
            devIntra.USUARIO_ENTRADA = _codigoUsuario;
            _eController.InserirMagentoEstoqueDevolvido(devIntra);
        }
        private void InserirNFEstoque(SP_OBTER_ECOM_PRODUTO_LIBERACAOResult ecomProdutoEstoque)
        {
            var estoqueEnviado = new ECOM_ESTOQUE();
            estoqueEnviado.PRODUTO = ecomProdutoEstoque.PRODUTO;
            estoqueEnviado.COR = ecomProdutoEstoque.COR;
            estoqueEnviado.FILIAL = ecomProdutoEstoque.FILIAL;
            estoqueEnviado.NF_SAIDA = ecomProdutoEstoque.NF_SAIDA;
            estoqueEnviado.SERIE_NF = ecomProdutoEstoque.SERIE_NF;
            estoqueEnviado.EMISSAO = Convert.ToDateTime(ecomProdutoEstoque.EMISSAO);
            estoqueEnviado.DATA_ENVIO = DateTime.Now;
            estoqueEnviado.USUARIO_ENVIO = _codigoUsuario;
            _eController.InserirMagentoEstoqueEnviado(estoqueEnviado);
        }
        private void InserirProdutoLink(string sku, string linkedSku, string linkedType)
        {
            var produtoLink = new ECOM_PRODUTO_LINK();
            produtoLink.SKU = sku;
            produtoLink.LINKED_SKU = linkedSku;
            produtoLink.LINKED_TYPE = linkedType;
            produtoLink.POSITION = 0;
            _eController.InserirProdutoLink(produtoLink);
        }
        private void AssociarCoresConfig(string produto)
        {
            //somente os configuraveis
            var produtos = _eController.ObterMagentoProdutoAssociacaoConfig(produto).Where(x => x.ID_PRODUTO_MAG_CONFIG == 0 && x.ID_PRODUTO_MAG > 0);

            foreach (var ecomProduto in produtos)
            {
                var linkedSkus = produtos.Where(x => x.ID_PRODUTO_MAG > 0 && x.SKU != ecomProduto.SKU).Select(x => x.SKU).ToList();
                if (linkedSkus.Any())
                    AssociarProdutosEComOutrosProdutoMagentoV2(ecomProduto.SKU, linkedSkus, "brother");
            }
        }


        #region "HTTP API"
        private OrderV2 ObterPedidoMagentoV2(string pedidoExterno)
        {
            var url = "V1/orders?searchCriteria[filter_groups][0][filters][0][field]=increment_id&searchCriteria[filter_groups][0]";
            url += $"[filters][0][value]={pedidoExterno}&searchCriteria[filter_groups][0][filters][0][condition_type]=eq&";
            url += "fields=items[increment_id,entity_id,items[item_id,product_type,qty_ordered]]";

            var request = new RestRequest(url, Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível obter o pedido {pedidoExterno}. \n {response.StatusCode} \n {response.Content}");

            var dataResponse = JsonConvert.DeserializeObject<OrderV2>(response.Content);
            return dataResponse;
        }
        private void InserirComentarioMagentoV2(int parentId, string status, string comment)
        {
            var request = new RestRequest($"V1/orders/{parentId}/comments", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarComentario(parentId, status, comment);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível inserir um comentário no pedido. \n {response.StatusCode} \n {response.Content}");
        }
        private string InserirEntregaMagentoV2(OrderV2 order, string trackNumber, string title, CarrierCode carrierCode, string comment)
        {
            var request = new RestRequest($"V1/order/{order.Items[0].ParentId}/ship", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarEntrega(order, trackNumber, title, carrierCode, comment);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível criar uma entrega para este pedido. \n {response.StatusCode} \n {response.Content}");

            return response.Content.Replace("\"", "");
        }

        private ProductV2DetailResult ObterProdutoMagentoV2(string sku)
        {
            var request = new RestRequest($"all/V1/products/{sku}", Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível encontrar o produto {sku}. \n {response.StatusCode} \n {response.Content}");

            var dataResponse = JsonConvert.DeserializeObject<ProductV2DetailResult>(response.Content);
            return dataResponse;
        }
        private ProductV2DetailResult InserirProdutoMagentoV2(bool simple, ECOM_PRODUTO ecomProduto, string tamanho, string qtde)
        {
            var request = new RestRequest("all/V1/products", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            ProductV2 data = null;
            if (simple)
                data = CriarProdutoSimples(ecomProduto, tamanho, qtde);
            else
                data = CriarProdutoConfiguravel(ecomProduto);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível cadastrar o produto {ecomProduto.SKU}. \n {response.StatusCode} \n {response.Content}");

            var dataResponse = JsonConvert.DeserializeObject<ProductV2DetailResult>(response.Content);
            return dataResponse;
        }
        private void AtualizarProdutoMagentoV2(ECOM_PRODUTO ecomProduto)
        {
            var request = new RestRequest($"all/V1/products/{ecomProduto.SKU}", Method.PUT);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarProdutoParaAtualizacao(ecomProduto);

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter> { new SingleOrArrayConverter<string>() }
            };

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data, settings),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível atualizar o produto {ecomProduto.SKU}. \n {response.StatusCode} \n {response.Content}");
        }
        private void HabilitarProdutoConfiguravelMagentoV2(ECOM_PRODUTO ecomProduto)
        {
            var request = new RestRequest("all/V1/products/" + ecomProduto.SKU, Method.PUT);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarProdutoConfiguravel(ecomProduto);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível habilitar o produto configurável {ecomProduto.SKU}");
        }

        private void AssociarProdutosWebsiteMagentoV2(string sku, int webSiteId)
        {
            var request = new RestRequest($"all/V1/products/{sku}/websites", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarProdutoWebsite(sku, webSiteId);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível associar o produto {sku} ao website");
        }

        private void AssociarProdutosEComOutrosProdutoMagentoV2(string sku, List<string> linkedSkus, string linkedType)
        {
            var request = new RestRequest($"all/V1/products/{sku}/links", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarLinksProduto(sku, linkedSkus, linkedType);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível associar o produto {sku} e suas cores");
        }
        private void DesassociarProdutosEComOutrosProdutoMagentoV2(string sku, string linkedSku, string linkedType)
        {
            var request = new RestRequest($"all/V1/products/{sku}/links/{linkedType}/{linkedSku}", Method.DELETE);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível desassociar o produto {sku} do produto {linkedSku}");
        }


        private ProductV2Inventory ObterEstoquePorSkusMagentoV2()
        {
            //var url = $"inventory/source-items?searchCriteria[filter_groups][0][filters][0][field]=sku&searchCriteria[filter_groups][0][filters][0][value]={string.Join(",", skus)}&searchCriteria[filter_groups][0][filters][0][condition_type]=in";
            var url = $"V1/inventory/source-items?searchCriteria[filter_groups][0][filters][0][field]=source_code&searchCriteria[filter_groups][0][filters][0][value]=default&searchCriteria[filter_groups][0][filters][0][condition_type]=eq";

            var request = new RestRequest(url, Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível obter o estoque dos produtos");

            var dataResponse = JsonConvert.DeserializeObject<ProductV2Inventory>(response.Content);
            return dataResponse;
        }
        private int ObterEstoqueSalablePorSkuMagentoV2(string sku)
        {
            var url = $"V1/inventory/get-product-salable-quantity/{sku}/1";

            var request = new RestRequest(url, Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível obter o estoque salable do produto");

            return Convert.ToInt32(response.Content.Replace("\"", ""));
        }

        private ProductV2Stock ObterEstoqueMagentoV2(ECOM_PRODUTO ecomProduto)
        {
            var request = new RestRequest($"V1/stockItems/{ecomProduto.SKU}", Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível atualizar o estoque {ecomProduto.SKU}");

            var dataResponse = JsonConvert.DeserializeObject<ProductV2Stock>(response.Content);
            return dataResponse;
        }
        private void AtualizarEstoqueSimplesMagentoV2(ECOM_PRODUTO ecomProduto, int qtdeEstoque)
        {
            var request = new RestRequest("V1/inventory/source-items", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarEstoque(ecomProduto, qtdeEstoque);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível atualizar o estoque {ecomProduto.SKU}. \n {response.StatusCode} \n {response.Content}");
        }

        private void InserirCategoriaMagentoV2(int categoriaId, string sku, int position = 0)
        {
            var request = new RestRequest($"all/V1/categories/{categoriaId}/products", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarCategoria(categoriaId, sku, position);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível inserir o produto {sku} na {categoriaId}");
        }
        private void AtualizarCategoriaMagentoV2(int categoriaId, string sku, int position = 0)
        {
            var request = new RestRequest($"all/V1/categories/{categoriaId}/products", Method.PUT);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarCategoria(categoriaId, sku, position);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível inserir o produto {sku} na {categoriaId}");
        }
        private void RemoverCategoriaMagentoV2(int categoriaId, string sku)
        {
            var request = new RestRequest($"all/V1/categories/{categoriaId}/products/{sku}", Method.DELETE);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível excluir o produto {sku} da {categoriaId}");
        }

        private List<FotoMagV2> InserirImagensMagentoV2(ECOM_PRODUTO ecomProduto)
        {
            var dataResponse = new List<FotoMagV2>();
            var fotos = CriarImagem(ecomProduto);

            foreach (var foto in fotos)
            {
                var request = new RestRequest($"all/V1/products/{ecomProduto.SKU}/media", Method.POST);

                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Bearer " + _token);

                request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(foto),
                        ParameterType.RequestBody);

                var response = _client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Não foi possível atualizar imagem do produto {ecomProduto.SKU}. \n {response.StatusCode} \n {response.Content}");

                var fotoId = response.Content.Replace("\"", "");

                dataResponse.Add(new FotoMagV2 { EntryId = Convert.ToInt32(fotoId), Position = foto.Entry.Position });
            }

            return dataResponse;
        }
        private void AtualizarImagensMagentoV2(ECOM_PRODUTO ecomProduto)
        {
            var dataResponse = new List<FotoMagV2>();
            var fotos = CriarImagemParaAtualizacao(ecomProduto);

            foreach (var foto in fotos)
            {
                var request = new RestRequest($"all/V1/products/{ecomProduto.SKU}/media/{foto.Entry.Id}", Method.PUT);

                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Bearer " + _token);

                request.AddParameter(
                        "application/json",
                        JsonConvert.SerializeObject(foto),
                        ParameterType.RequestBody);

                var response = _client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Não foi possível atualizar imagem do produto {ecomProduto.SKU}. \n {response.StatusCode} \n {response.Content}");

            }
        }

        private void InserirProdutoConfigOptionsMagentoV2(ECOM_PRODUTO ecomProduto, string attributeId, string label)
        {
            var request = new RestRequest($"all/V1/configurable-products/{ecomProduto.SKU}/options", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarConfigOptions(attributeId, label);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível criar as options do produto configurável {ecomProduto.SKU}");
        }
        private void AssociarSkuSimplesMagentoV2(ECOM_PRODUTO ecomProduto, string skuSimples)
        {
            var request = new RestRequest($"all/V1/configurable-products/{ecomProduto.SKU}/child", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = new
            {
                childSku = skuSimples
            };

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível associar o produto configurável {ecomProduto.SKU} ao SKU {skuSimples}");
        }

        private void AtualizarPrecoPromoMagentoV2(List<string> skus, decimal preco, int storeId)
        {
            var request = new RestRequest($"all/V1/products/special-price", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarPrecoPromoParaAtualizacao(skus, preco, storeId);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível atualizar o preço produto {skus.ToString()} ");

        }

        private OrderV2RuleResult InserirRegraCupomMagentoV2(string codigoCupom, double valorCupom, bool freteGratis)
        {
            var request = new RestRequest($"all/V1/salesRules", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarRegraCupom(codigoCupom, valorCupom, freteGratis);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível inserir a regra do cupom {codigoCupom}");

            var dataResponse = JsonConvert.DeserializeObject<OrderV2RuleResult>(response.Content);
            return dataResponse;
        }
        private OrderV2RuleCouponResult InserirCupomMagentoV2(int ruleId, string codigoCupom)
        {
            var request = new RestRequest($"all/V1/coupons", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var data = CriarCupom(ruleId, codigoCupom);

            request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível inserir o cupom {codigoCupom}");

            var dataResponse = JsonConvert.DeserializeObject<OrderV2RuleCouponResult>(response.Content);
            return dataResponse;
        }

        private void RemoverCupomMagentoV2(int couponId)
        {
            var request = new RestRequest($"all/V1/coupons/{couponId}", Method.DELETE);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível excluir o cupom de Id {couponId}");
        }
        private void RemoverRegraCupomMagentoV2(int ruleId)
        {
            var request = new RestRequest($"all/V1/salesRules/{ruleId}", Method.DELETE);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + _token);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Não foi possível excluir a regra do cupom de Id {ruleId}");
        }
        #endregion

        #region "CRIAR OBJETOS"
        private OrderV2Rule CriarRegraCupom(string codigoCupom, double valorCupom, bool freteGratis)
        {
            var data = new OrderV2Rule();

            var rule = new OrderV2RuleRule();
            rule.Name = "INTRA - " + codigoCupom;
            rule.Description = "INTRA - " + codigoCupom;
            rule.WebsiteIds = new List<int> { 1 };
            rule.CustomerGroupIds = new List<int> { 0, 1, 2, 3 };
            rule.FromDate = DateTime.Today.ToString("yyyy-MM-dd");
            rule.UsesPerCustomer = 1;
            rule.IsActive = true;
            rule.IsAdvanced = true;
            rule.SimpleAction = "cart_fixed";
            rule.DiscountAmount = valorCupom;
            rule.DiscountStep = 0;
            rule.ApplyToShipping = false;
            rule.CouponType = "SPECIFIC_COUPON";
            rule.UseAutoGeneration = false;
            rule.UsesPerCoupon = 1;
            rule.SimpleFreeShipping = (freteGratis) ? "1" : "0";

            data.Rule = rule;
            return data;
        }
        private OrderV2RuleCoupon CriarCupom(int ruleId, string codigoCupom)
        {
            var data = new OrderV2RuleCoupon();

            var coupon = new OrderV2RuleCouponCoupon();
            coupon.RuleId = ruleId;
            coupon.Code = codigoCupom;
            coupon.UsageLimit = 1;
            coupon.UsagePerCustomer = 1;
            coupon.IsPrimary = true;
            coupon.Type = 0;

            data.Coupon = coupon;
            return data;
        }

        private ProductV2WebSite CriarProdutoWebsite(string sku, int webSiteId)
        {
            var data = new ProductV2WebSite();

            data.ProductWebsiteLink = new ProductV2WebSiteLink
            {
                Sku = sku,
                WebsiteId = webSiteId
            };

            return data;
        }

        private OrderV2Ship CriarEntrega(OrderV2 order, string trackNumber, string title, CarrierCode carrierCode, string comment)
        {
            var data = new OrderV2Ship();

            var items = new List<OrderV2ShipItems>();
            foreach (var it in order.Items[0].Items.Where(x => x.ProductType == "configurable"))
                items.Add(new OrderV2ShipItems { OrderItemId = it.ItemId, Qty = it.Qty });

            data.Items = items;
            data.Notify = true;
            data.AppendComment = true;
            data.Comment = new OrderV2ShipComment
            {
                IsVisibleOnFront = 0,
                Comment = comment
            };

            var tracks = new List<OrderV2ShipTrack>();
            tracks.Add(new OrderV2ShipTrack
            {
                TrackNumber = trackNumber,
                Title = title,
                CarrierCode = carrierCode.ToString()
            });

            data.Tracks = tracks;

            return data;
        }
        private OrderV2Comment CriarComentario(int parentId, string status, string comment)
        {
            var data = new OrderV2Comment();

            data.StatusHistory = new OrderV2CommentStatus
            {
                Comment = comment,
                CreatedAt = DateTime.UtcNow,
                IsCustomerNotified = 0,
                IsVisibleOnFront = 0,
                ParentId = parentId,
                Status = status
            };

            return data;
        }

        private ProductV2Category CriarCategoria(int categoriaId, string sku, int position)
        {
            var data = new ProductV2Category();

            data.ProductLink = new ProductV2CategorySku { CategoryId = categoriaId.ToString(), Sku = sku, Position = position };

            return data;
        }

        private ProductV2Link CriarLinksProduto(string sku, List<string> linkedSkus, string linkedType)
        {
            var data = new ProductV2Link();

            var items = new List<ProductV2LinkItems>();
            foreach (var linkedSku in linkedSkus)
            {
                items.Add(new ProductV2LinkItems
                {
                    Sku = sku,
                    LinkType = linkedType,
                    LinkedProductSku = linkedSku,
                    LinkedProductType = "configurable",
                    Position = 0
                });
            }
            data.Items = items;
            return data;
        }

        private List<ProductV2Image> CriarImagem(ECOM_PRODUTO ecomProduto)
        {
            var data = new List<ProductV2Image>();

            var fotoMagV2 = new List<FotoMagV2>();

            bool grupo6Foto = (ecomProduto.COD_CATEGORIA == "01") ? true : false;

            if (grupo6Foto)
                fotoMagV2.Add(new FotoMagV2 { Position = Convert.ToInt32(ecomProduto.FOTO_LOOK_POS), PathFoto = ecomProduto.FOTO_LOOK });

            fotoMagV2.Add(new FotoMagV2 { Position = Convert.ToInt32(ecomProduto.FOTO_FRENTE_CAB_POS), PathFoto = ecomProduto.FOTO_FRENTE_CAB });
            if (grupo6Foto)
                fotoMagV2.Add(new FotoMagV2 { Position = Convert.ToInt32(ecomProduto.FOTO_FRENTE_SEMCAB_POS), PathFoto = ecomProduto.FOTO_FRENTE_SEMCAB });

            fotoMagV2.Add(new FotoMagV2 { Position = Convert.ToInt32(ecomProduto.FOTO_COSTAS_POS), PathFoto = ecomProduto.FOTO_COSTAS });
            fotoMagV2.Add(new FotoMagV2 { Position = Convert.ToInt32(ecomProduto.FOTO_DETALHE_POS), PathFoto = ecomProduto.FOTO_DETALHE });

            if (grupo6Foto)
                fotoMagV2.Add(new FotoMagV2 { Position = Convert.ToInt32(ecomProduto.FOTO_LADO_POS), PathFoto = ecomProduto.FOTO_LADO });

            foreach (var fo in fotoMagV2)
            {
                var productImage = new ProductV2Image();
                var entryImage = new ProductV2ImageEntry();

                entryImage.MediaType = "image";
                entryImage.Label = "Handbook " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.GRUPO_PRODUTO.ToLower()) + " " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.ECOM_COR1.COR.ToLower()) + " " + fo.Position.ToString();
                entryImage.Label = entryImage.Label.Replace(".", "");
                entryImage.Position = fo.Position;
                entryImage.Disabled = false;

                List<string> types = new List<string>();
                if (fo.Position.ToString() == ecomProduto.FOTO_VITRINE_POS.ToString())
                {
                    types.Add("small_image");
                    types.Add("thumbnail");
                    types.Add("image");
                }

                if (fo.Position.ToString() == ecomProduto.FOTO_HOVER_POS.ToString()) // Na V2 grava a posicao da foto (FOTO_HOVER_MAG)
                {
                    types.Add("hover_image");
                }
                if (fo.Position.ToString() == ecomProduto.FOTO_HOVER_POS.ToString()) // Na V2 grava a posicao da foto (FOTO_HOVER_MAG)
                {
                    types.Add("hover_image");
                }


                entryImage.Types = types;

                var path = @_pathSistema + fo.PathFoto.ToString().Replace("~", "").Replace("/", "\\");
                entryImage.File = path;

                var content = new ProductV2ImageContent();
                content.Type = "image/jpeg";
                content.Name = "handbook-" + ecomProduto.GRUPO_PRODUTO.ToLower() + "-" + ecomProduto.ECOM_COR1.COR.ToLower() + "-" + fo.Position.ToString();
                content.Name = content.Name.Replace(".", "");

                byte[] bytes = File.ReadAllBytes(path);
                string fileBase64 = Convert.ToBase64String(bytes);
                content.Base64EncodedData = fileBase64;

                entryImage.Content = content;

                productImage.Entry = entryImage;

                data.Add(productImage);
            }

            return data;
        }
        private List<ProductV2ImageUpdate> CriarImagemParaAtualizacao(ECOM_PRODUTO ecomProduto)
        {
            var data = new List<ProductV2ImageUpdate>();

            var fotoMagV2 = new List<FotoMagV2>();

            bool grupo6Foto = (ecomProduto.COD_CATEGORIA == "01") ? true : false;

            if (grupo6Foto)
                fotoMagV2.Add(new FotoMagV2 { EntryId = Convert.ToInt32(ecomProduto.FOTO_LOOK_MAG), Position = Convert.ToInt32(ecomProduto.FOTO_LOOK_POS), PathFoto = ecomProduto.FOTO_LOOK });

            fotoMagV2.Add(new FotoMagV2 { EntryId = Convert.ToInt32(ecomProduto.FOTO_FRENTE_CAB_MAG), Position = Convert.ToInt32(ecomProduto.FOTO_FRENTE_CAB_POS), PathFoto = ecomProduto.FOTO_FRENTE_CAB });
            if (grupo6Foto)
                fotoMagV2.Add(new FotoMagV2 { EntryId = Convert.ToInt32(ecomProduto.FOTO_FRENTE_SEMCAB_MAG), Position = Convert.ToInt32(ecomProduto.FOTO_FRENTE_SEMCAB_POS), PathFoto = ecomProduto.FOTO_FRENTE_SEMCAB });

            fotoMagV2.Add(new FotoMagV2 { EntryId = Convert.ToInt32(ecomProduto.FOTO_COSTAS_MAG), Position = Convert.ToInt32(ecomProduto.FOTO_COSTAS_POS), PathFoto = ecomProduto.FOTO_COSTAS });
            fotoMagV2.Add(new FotoMagV2 { EntryId = Convert.ToInt32(ecomProduto.FOTO_DETALHE_MAG), Position = Convert.ToInt32(ecomProduto.FOTO_DETALHE_POS), PathFoto = ecomProduto.FOTO_DETALHE });

            if (grupo6Foto)
                fotoMagV2.Add(new FotoMagV2 { EntryId = Convert.ToInt32(ecomProduto.FOTO_LADO_MAG), Position = Convert.ToInt32(ecomProduto.FOTO_LADO_POS), PathFoto = ecomProduto.FOTO_LADO });

            foreach (var fo in fotoMagV2)
            {
                var productImage = new ProductV2ImageUpdate();
                var entryImage = new ProductV2ImageEntryUpdate();

                entryImage.Id = fo.EntryId;
                entryImage.MediaType = "image";
                entryImage.Label = "Handbook " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.GRUPO_PRODUTO.ToLower()) + " " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.ECOM_COR1.COR.ToLower()) + " " + fo.Position.ToString();
                entryImage.Label = entryImage.Label.Replace(".", "");
                entryImage.Position = fo.Position;
                entryImage.Disabled = false;

                List<string> types = new List<string>();
                if (fo.Position.ToString() == ecomProduto.FOTO_VITRINE_POS.ToString())
                {
                    types.Add("small_image");
                    types.Add("thumbnail");
                    types.Add("image");
                }

                if (fo.Position.ToString() == ecomProduto.FOTO_HOVER_POS.ToString()) // Na V2 grava a posicao da foto (FOTO_HOVER_MAG)
                {
                    types.Add("hover_image");
                }

                entryImage.Types = types;

                var path = @_pathSistema + fo.PathFoto.ToString().Replace("~", "").Replace("/", "\\");
                entryImage.File = path;

                var content = new ProductV2ImageContentUpdate();
                content.Type = "image/jpeg";
                content.Name = "handbook-" + ecomProduto.GRUPO_PRODUTO.ToLower() + "-" + ecomProduto.ECOM_COR1.COR.ToLower() + "-" + fo.Position.ToString();
                content.Name = content.Name.Replace(".", "");

                byte[] bytes = File.ReadAllBytes(path);
                string fileBase64 = Convert.ToBase64String(bytes);
                content.Base64EncodedData = fileBase64;

                entryImage.Content = content;

                productImage.Entry = entryImage;

                data.Add(productImage);
            }

            return data;
        }

        private StockV2 CriarEstoque(ECOM_PRODUTO product, int qtde)
        {
            var data = new StockV2();

            var items = new List<StockV2Items>();
            items.Add(new StockV2Items
            {
                Sku = product.SKU,
                SourceCode = "default",
                Quantity = qtde,
                Status = 1
            });

            data.SourceItems = items;

            return data;
        }
        private AttrOptionsV2 CriarConfigOptions(string attributeId, string label)
        {
            var data = new AttrOptionsV2();

            var option = new OptionV2();
            option.AttributeId = attributeId;
            option.Label = label;
            option.Position = 0;
            option.IsUseDefault = true;
            option.Values = new List<OptionValueIndexV2>() { new OptionValueIndexV2 { ValueIndex = 0 } };

            data.Option = option;

            return data;
        }

        private ProductV2ExtAttrStockItem CriarEstoqueInicial(bool simples, string qtde)
        {
            var data = new ProductV2ExtAttrStockItem();

            data.IsInStock = true;
            if (simples)
                data.Qty = qtde;

            return data;
        }
        private List<ProductV2ExtAttrCategoryLinks> CriarCategorias(List<string> categoriasIds)
        {
            var data = new List<ProductV2ExtAttrCategoryLinks>();

            foreach (var categoryId in categoriasIds)
                data.Add(new ProductV2ExtAttrCategoryLinks { Position = 0, CategoryId = categoryId });

            return data;
        }
        private ProductV2ExtAttr CriarAtributosExt(bool simples, string qtde, List<string> categoriasIds)
        {
            var data = new ProductV2ExtAttr();

            data.CategoryLinks = CriarCategorias(categoriasIds);
            data.StockItem = CriarEstoqueInicial(simples, qtde);

            return data;
        }
        private List<ProductV2CustomAttr> CriarAtributosCustom(ECOM_PRODUTO ecomProduto, bool simples, string tamanho)
        {
            var data = new List<ProductV2CustomAttr>();

            if (ecomProduto.ECOM_TIPO_COMPRIMENTO != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "tipo_comprimento",
                    Value = ecomProduto.ECOM_TIPO_COMPRIMENTO1.ATTR_VALUE.ToString()
                });
            }

            if (ecomProduto.ECOM_TIPO_ESTILO != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "tipo_estilo",
                    Value = ecomProduto.ECOM_TIPO_ESTILO1.ATTR_VALUE.ToString()
                });
            }

            if (ecomProduto.ECOM_TIPO_GOLA != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "tipo_gola",
                    Value = ecomProduto.ECOM_TIPO_GOLA1.ATTR_VALUE.ToString()
                });
            }

            if (ecomProduto.ECOM_TIPO_MANGA != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "tipo_manga",
                    Value = ecomProduto.ECOM_TIPO_MANGA1.ATTR_VALUE.ToString()
                });
            }

            if (ecomProduto.ECOM_TIPO_MODELAGEM != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "tipo_modelagem",
                    Value = ecomProduto.ECOM_TIPO_MODELAGEM1.ATTR_VALUE.ToString()
                });
            }

            if (ecomProduto.ECOM_TIPO_TECIDO != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "tipo_tecido",
                    Value = ecomProduto.ECOM_TIPO_TECIDO1.ATTR_VALUE.ToString()
                });
            }

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "description",
                Value = ecomProduto.PRODUTO_DESC
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "short_description",
                Value = ecomProduto.PRODUTO_DESC_CURTA
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "country_of_manufacture",
                Value = "BR"
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "meta_title",
                Value = ecomProduto.META_TITULO
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "meta_keyword",
                Value = ecomProduto.META_KEYWORD
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "meta_description",
                Value = ecomProduto.META_DESCRICAO
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "color",
                Value = ecomProduto.ECOM_COR1.ATTR_VALUE.ToString()
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "grupo_produto",
                Value = (ecomProduto.ECOM_GRUPO_PRODUTO1.ATTR_GPMAGENTO == null) ? ObterGrupoProdutoValueAttr(ecomProduto.GRUPO_PRODUTO) : ecomProduto.ECOM_GRUPO_PRODUTO1.ATTR_GPMAGENTO.ToString()
            });

            var ncm = _baseController.BuscaProduto(ecomProduto.PRODUTO);
            if (ncm != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "gtin14",
                    Value = ncm.CLASSIF_FISCAL.Trim()
                });
            }

            var griffe = ecomProduto.GRIFFE.ToLower();
            var attrGriffe = (griffe == "feminino" || griffe == "petit" || griffe == "tulp") ? "81" : ((griffe == "masculino") ? "80" : "84");
            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "gender",
                Value = attrGriffe
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "lead_time",
                Value = "1"
            });

            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "volume_width",
                Value = ecomProduto.ECOM_CAIXA_DIMENSAO1.LARGURA_CM.ToString().Replace(",", ".")
            });
            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "volume_length",
                Value = ecomProduto.ECOM_CAIXA_DIMENSAO1.COMPRIMENTO_CM.ToString().Replace(",", ".")
            });
            data.Add(new ProductV2CustomAttr
            {
                AttributeCode = "volume_height",
                Value = ecomProduto.ECOM_CAIXA_DIMENSAO1.ALTURA_CM.ToString().Replace(",", ".")
            });

            if (ecomProduto.PRECO_PROMO != null)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "special_price",
                    Value = ecomProduto.PRECO_PROMO.ToString().Replace(",", ".")
                });
            }

            // # dados dos produtos simples
            if (simples)
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "size",
                    Value = ObterTamanhoValueAttr(tamanho)
                });
            }
            else // # dados dos produtos configuraveis
            {
                data.Add(new ProductV2CustomAttr
                {
                    AttributeCode = "url_key",
                    Value = ecomProduto.URL_PRODUTO
                });
            }

            return data;
        }

        private ProductV2 CriarProdutoSimples(ECOM_PRODUTO ecomProduto, string tamanho, string qtde)
        {
            var data = new ProductV2();

            var product = new ProductV2DetailSimple
            {
                Sku = ecomProduto.SKU + "-" + ecomProduto.ECOM_COR1.COR + "-" + tamanho,
                Name = ecomProduto.NOME_MAG + " - " + ecomProduto.ECOM_COR1.COR + " - " + tamanho,
                AttributeSetId = 4,
                Status = 1,
                Price = Convert.ToDouble(ecomProduto.PRECO),
                Visibility = 1, // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
                TypeId = "simple",
                Weight = ecomProduto.PESO_KG.ToString().Replace(",", ".")
            };

            product.ExtensionAttributes = CriarAtributosExt(true, qtde, new List<string>());
            product.CustomAttributes = CriarAtributosCustom(ecomProduto, true, tamanho);

            data.Product = product;

            return data;
        }
        private ProductV2 CriarProdutoConfiguravel(ECOM_PRODUTO ecomProduto)
        {
            var data = new ProductV2();

            var product = new ProductV2Detail
            {
                Sku = ecomProduto.SKU,
                Name = ecomProduto.NOME_MAG,
                AttributeSetId = 4,
                Status = 1,
                Visibility = Convert.ToInt32(ecomProduto.VISIBILIDADE), // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
                TypeId = "configurable",
                Weight = ecomProduto.PESO_KG.ToString().Replace(",", ".")
            };

            //obter categorias
            var cats = new List<string>();
            cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO.ToString());
            var catsProduto = _eController.ObterProdutoCategoria(ecomProduto.CODIGO);
            foreach (var c in catsProduto)
                cats.Add(c.ECOM_GRUPO_PRODUTO.ToString());

            product.ExtensionAttributes = CriarAtributosExt(false, "0", cats);
            product.CustomAttributes = CriarAtributosCustom(ecomProduto, false, "");

            data.Product = product;

            return data;
        }

        private List<ProductV2UpdateExtAttrCategoryLinks> CriarCategoriasParaAtualizacao(List<string> categoriasIds)
        {
            var data = new List<ProductV2UpdateExtAttrCategoryLinks>();

            foreach (var categoryId in categoriasIds)
                data.Add(new ProductV2UpdateExtAttrCategoryLinks { Position = 0, CategoryId = categoryId });

            return data;
        }
        private ProductV2UpdateExtAttr CriarAtributosExtParaAtualizacao(List<string> categoriasIds)
        {
            var data = new ProductV2UpdateExtAttr();

            data.CategoryLinks = CriarCategoriasParaAtualizacao(categoriasIds);
            return data;
        }
        private List<ProductV2UpdateCustomAttr> CriarAtributosCustomParaAtualizacao(ECOM_PRODUTO ecomProduto)
        {
            var data = new List<ProductV2UpdateCustomAttr>();

            if (ecomProduto.ECOM_TIPO_COMPRIMENTO != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "tipo_comprimento",
                    Value = new List<string> { ecomProduto.ECOM_TIPO_COMPRIMENTO1.ATTR_VALUE.ToString() }
                });
            }

            if (ecomProduto.ECOM_TIPO_ESTILO != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "tipo_estilo",
                    Value = new List<string> { ecomProduto.ECOM_TIPO_ESTILO1.ATTR_VALUE.ToString() }
                });
            }

            if (ecomProduto.ECOM_TIPO_GOLA != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "tipo_gola",
                    Value = new List<string> { ecomProduto.ECOM_TIPO_GOLA1.ATTR_VALUE.ToString() }
                });
            }

            if (ecomProduto.ECOM_TIPO_MANGA != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "tipo_manga",
                    Value = new List<string> { ecomProduto.ECOM_TIPO_MANGA1.ATTR_VALUE.ToString() }
                });
            }

            if (ecomProduto.ECOM_TIPO_MODELAGEM != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "tipo_modelagem",
                    Value = new List<string> { ecomProduto.ECOM_TIPO_MODELAGEM1.ATTR_VALUE.ToString() }
                });
            }

            if (ecomProduto.ECOM_TIPO_TECIDO != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "tipo_tecido",
                    Value = new List<string> { ecomProduto.ECOM_TIPO_TECIDO1.ATTR_VALUE.ToString() }
                });
            }

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "description",
                Value = new List<string> { ecomProduto.PRODUTO_DESC }
            });

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "short_description",
                Value = new List<string> { ecomProduto.PRODUTO_DESC_CURTA }
            });

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "meta_title",
                Value = new List<string> { ecomProduto.META_TITULO }
            });

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "meta_keyword",
                Value = new List<string> { ecomProduto.META_KEYWORD }
            });

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "meta_description",
                Value = new List<string> { ecomProduto.META_DESCRICAO }
            });

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "color",
                Value = new List<string> { ecomProduto.ECOM_COR1.ATTR_VALUE.ToString() }
            });

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "grupo_produto",
                Value = new List<string> { ObterGrupoProdutoValueAttr(ecomProduto.GRUPO_PRODUTO) }
            });

            var ncm = _baseController.BuscaProduto(ecomProduto.PRODUTO);
            if (ncm != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "gtin14",
                    Value = new List<string> { ncm.CLASSIF_FISCAL.Trim() }
                });
            }

            var griffe = ecomProduto.GRIFFE.ToLower();
            var attrGriffe = (griffe == "feminino" || griffe == "petit" || griffe == "tulp") ? "81" : ((griffe == "masculino") ? "80" : "84");
            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "gender",
                Value = new List<string> { attrGriffe }
            });


            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "category_ids",
                Value = new List<string> { }
            });

            if (ecomProduto.PRECO_PROMO != null)
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "special_price",
                    Value = new List<string> { ecomProduto.PRECO_PROMO.ToString().Replace(",", ".") }
                });
            }
            else
            {
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "special_price",
                    Value = null
                });
                data.Add(new ProductV2UpdateCustomAttr
                {
                    AttributeCode = "special_from_date",
                    Value = null
                });
            }

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "lead_time",
                Value = new List<string> { "1" }
            });

            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "volume_width",
                Value = new List<string> { ecomProduto.ECOM_CAIXA_DIMENSAO1.LARGURA_CM.ToString().Replace(",", ".") }
            });
            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "volume_length",
                Value = new List<string> { ecomProduto.ECOM_CAIXA_DIMENSAO1.COMPRIMENTO_CM.ToString().Replace(",", ".") }
            });
            data.Add(new ProductV2UpdateCustomAttr
            {
                AttributeCode = "volume_height",
                Value = new List<string> { ecomProduto.ECOM_CAIXA_DIMENSAO1.ALTURA_CM.ToString().Replace(",", ".") }
            });

            return data;
        }

        private ProductV2Update CriarProdutoParaAtualizacao(ECOM_PRODUTO ecomProduto)
        {
            var data = new ProductV2Update();

            var product = new ProductV2UpdateDetailSimple
            {
                Sku = ecomProduto.SKU,
                Name = ecomProduto.NOME_MAG,
                Status = 1,
                Price = (ecomProduto.ID_PRODUTO_MAG_CONFIG == 0) ? 0 : Convert.ToDouble(ecomProduto.PRECO),
                Visibility = Convert.ToInt32(ecomProduto.VISIBILIDADE), // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
                TypeId = (ecomProduto.ID_PRODUTO_MAG_CONFIG == 0) ? "configurable" : "simple",
                Weight = ecomProduto.PESO_KG.ToString().Replace(",", ".")
            };

            //obter categorias
            var cats = new List<string>();
            if (ecomProduto.ID_PRODUTO_MAG_CONFIG == 0) // se produto configuravel
            {
                cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO.ToString());
                var catsProduto = _eController.ObterProdutoCategoria(ecomProduto.CODIGO);
                foreach (var c in catsProduto)
                    cats.Add(c.ECOM_GRUPO_PRODUTO.ToString());
            }

            product.ExtensionAttributes = CriarAtributosExtParaAtualizacao(cats);
            product.CustomAttributes = CriarAtributosCustomParaAtualizacao(ecomProduto);

            data.Product = product;

            return data;
        }

        private ProductV2PriceSpecial CriarPrecoPromoParaAtualizacao(List<string> skus, decimal preco, int storeId = 0)
        {
            var data = new ProductV2PriceSpecial();

            var prices = new List<ProductV2PriceSpecialSkus>();
            foreach (var sku in skus)
            {
                prices.Add(new ProductV2PriceSpecialSkus
                {
                    Sku = sku,
                    Price = preco,
                    StoreId = storeId
                });
            }

            data.Prices = prices;
            return data;
        }

        #endregion

        private string ObterTamanhoValueAttr(string tamanho)
        {
            var valueAttr = "";
            switch (tamanho)
            {
                case "105":
                    valueAttr = "283";
                    break;
                case "32":
                    valueAttr = "284";
                    break;
                case "3G":
                    valueAttr = "285";
                    break;
                case "90":
                    valueAttr = "286";
                    break;
                case "95":
                    valueAttr = "287";
                    break;
                case "33":
                    valueAttr = "288";
                    break;
                case "34":
                    valueAttr = "289";
                    break;
                case "35":
                    valueAttr = "290";
                    break;
                case "36":
                    valueAttr = "291";
                    break;
                case "37":
                    valueAttr = "292";
                    break;
                case "38":
                    valueAttr = "293";
                    break;
                case "39":
                    valueAttr = "294";
                    break;
                case "40":
                    valueAttr = "295";
                    break;
                case "":
                    valueAttr = "";
                    break;
                case "41":
                    valueAttr = "296";
                    break;
                case "42":
                    valueAttr = "297";
                    break;
                case "43":
                    valueAttr = "298";
                    break;
                case "44":
                    valueAttr = "299";
                    break;
                case "45":
                    valueAttr = "300";
                    break;
                case "46":
                    valueAttr = "301";
                    break;
                case "100":
                    valueAttr = "220";
                    break;
                case "47":
                    valueAttr = "302";
                    break;
                case "48":
                    valueAttr = "303";
                    break;
                case "Único":
                case "UN":
                    valueAttr = "304";
                    break;
                case "XP":
                    valueAttr = "305";
                    break;
                case "PP":
                    valueAttr = "306";
                    break;
                case "PQ":
                    valueAttr = "307";
                    break;
                case "MD":
                    valueAttr = "308";
                    break;
                case "GD":
                    valueAttr = "309";
                    break;
                case "GG":
                    valueAttr = "310";
                    break;
                case "XG":
                    valueAttr = "311";
                    break;
                default:
                    break;
            }

            return valueAttr;
        }
        private string ObterGrupoProdutoValueAttr(string grupoProduto)
        {
            grupoProduto = grupoProduto.Trim().ToLower();
            grupoProduto = grupoProduto.Replace("basic", "camiseta");
            grupoProduto = grupoProduto.Replace("calca", "calça");
            grupoProduto = grupoProduto.Replace("macacao", "macacão");
            grupoProduto = grupoProduto.Replace("malhao", "malhão");
            grupoProduto = grupoProduto.Replace("sandalia", "sandália");
            grupoProduto = grupoProduto.Replace("tenis", "tênis");
            grupoProduto = grupoProduto.Replace("moleton", "moletom");
            grupoProduto = grupoProduto.Replace("oculos", "Óculos");
            grupoProduto = Utils.WebControls.AlterarPrimeiraLetraMaiscula(grupoProduto);


            var valueAttr = "";
            switch (grupoProduto)
            {
                case "Bermuda":
                    valueAttr = "249";
                    break;
                case "Blazer":
                    valueAttr = "250";
                    break;
                case "Blusa":
                    valueAttr = "251";
                    break;
                case "Body":
                    valueAttr = "252";
                    break;
                case "Bolsa":
                    valueAttr = "376";
                    break;
                case "Bota":
                    valueAttr = "253";
                    break;
                case "Calça":
                    valueAttr = "254";
                    break;
                case "Cardigan":
                case "Camisa":
                    valueAttr = "255";
                    break;
                case "Camiseta":
                    valueAttr = "256";
                    break;
                case "Carteira":
                    valueAttr = "257";
                    break;
                case "Chinelo":
                    valueAttr = "258";
                    break;
                case "Cinto":
                    valueAttr = "259";
                    break;
                case "Colete":
                    valueAttr = "260";
                    break;
                case "Jaqueta":
                    valueAttr = "261";
                    break;
                case "Leg":
                    valueAttr = "262";
                    break;
                case "Macacão":
                    valueAttr = "263";
                    break;
                case "Macaquinho":
                    valueAttr = "264";
                    break;
                case "Malhão":
                    valueAttr = "265";
                    break;
                case "Mochila":
                    valueAttr = "266";
                    break;
                case "Moletom":
                    valueAttr = "267";
                    break;
                case "Óculos":
                    valueAttr = "268";
                    break;
                case "P.coat":
                    valueAttr = "269";
                    break;
                case "Pochete":
                    valueAttr = "270";
                    break;
                case "Polo":
                    valueAttr = "271";
                    break;
                case "Rasteira":
                    valueAttr = "377";
                    break;
                case "Regata":
                    valueAttr = "272";
                    break;
                case "Saia":
                    valueAttr = "273";
                    break;
                case "Sandália":
                    valueAttr = "274";
                    break;
                case "Sapato":
                    valueAttr = "275";
                    break;
                case "Scarpin":
                    valueAttr = "379";
                    break;
                case "Shorts":
                    valueAttr = "276";
                    break;
                case "Shorts saia":
                    valueAttr = "277";
                    break;
                case "Tamanco":
                    valueAttr = "278";
                    break;
                case "Tênis":
                    valueAttr = "279";
                    break;
                case "Top":
                    valueAttr = "280";
                    break;
                case "Trench":
                    valueAttr = "378";
                    break;
                case "Tricot":
                    valueAttr = "281";
                    break;
                case "Vestido":
                case "Vestido longo":
                    valueAttr = "282";
                    break;

                default:
                    break;
            }

            return valueAttr;
        }
        private ECOM_PRODUTO CriarCopiaECOMPRODUTO(ECOM_PRODUTO ecomProduto, int idProdutoMag, string tamanho)
        {
            var novo = new ECOM_PRODUTO();

            novo.ID_PRODUTO_MAG = idProdutoMag;
            novo.ID_PRODUTO_MAG_CONFIG = null;
            novo.COLECAO = ecomProduto.COLECAO;
            novo.PRODUTO = ecomProduto.PRODUTO;
            novo.NOME = ecomProduto.NOME;
            novo.NOME_MAG = ecomProduto.NOME_MAG + " - " + ecomProduto.ECOM_COR1.COR + " - " + tamanho;
            novo.PRODUTO_DESC = ecomProduto.PRODUTO_DESC;
            novo.PRODUTO_DESC_CURTA = ecomProduto.PRODUTO_DESC_CURTA;
            novo.SKU = ecomProduto.SKU + "-" + ecomProduto.ECOM_COR1.COR + "-" + tamanho;
            novo.COR = ecomProduto.COR;
            novo.TAMANHO = tamanho;
            novo.ECOM_COR = ecomProduto.ECOM_COR;
            novo.PESO_KG = ecomProduto.PESO_KG;
            novo.GRUPO_PRODUTO = ecomProduto.GRUPO_PRODUTO;
            novo.ECOM_GRUPO_PRODUTO = ecomProduto.ECOM_GRUPO_PRODUTO;
            novo.PRECO = ecomProduto.PRECO;
            novo.PRECO_PROMO = ecomProduto.PRECO_PROMO;
            novo.PRECO_PROMO_DATA_DE = ecomProduto.PRECO_PROMO_DATA_DE;
            novo.PRECO_PROMO_DATA_ATE = ecomProduto.PRECO_PROMO_DATA_ATE;
            novo.ECOM_GRUPO_MACRO = ecomProduto.ECOM_GRUPO_MACRO;
            novo.ECOM_TIPO_MODELAGEM = ecomProduto.ECOM_TIPO_MODELAGEM;
            novo.ECOM_TIPO_TECIDO = ecomProduto.ECOM_TIPO_TECIDO;
            novo.ECOM_TIPO_MANGA = ecomProduto.ECOM_TIPO_MANGA;
            novo.ECOM_TIPO_GOLA = ecomProduto.ECOM_TIPO_GOLA;
            novo.ECOM_TIPO_COMPRIMENTO = ecomProduto.ECOM_TIPO_COMPRIMENTO;
            novo.ECOM_TIPO_ESTILO = ecomProduto.ECOM_TIPO_ESTILO;
            novo.ECOM_TIPO_LINHA = ecomProduto.ECOM_TIPO_LINHA;
            novo.ECOM_SIGNED = ecomProduto.ECOM_SIGNED;
            novo.VISIBILIDADE = "1";
            novo.ECOM_CAIXA_DIMENSAO = ecomProduto.ECOM_CAIXA_DIMENSAO;
            novo.META_TITULO = ecomProduto.META_TITULO;
            novo.META_KEYWORD = ecomProduto.META_KEYWORD;
            novo.META_DESCRICAO = ecomProduto.META_DESCRICAO;
            novo.URL_PRODUTO = ecomProduto.URL_PRODUTO;
            novo.OBSERVACAO = ecomProduto.OBSERVACAO;
            novo.CAD_MKTPLACE = ecomProduto.CAD_MKTPLACE;
            novo.GOOGLE_SHOPPING = ecomProduto.GOOGLE_SHOPPING;

            novo.FOTO_LOOK_POS = ecomProduto.FOTO_LOOK_POS;
            novo.FOTO_LOOK = ecomProduto.FOTO_LOOK;

            novo.FOTO_FRENTE_CAB_POS = ecomProduto.FOTO_FRENTE_CAB_POS;
            novo.FOTO_FRENTE_CAB = ecomProduto.FOTO_FRENTE_CAB;

            novo.FOTO_FRENTE_SEMCAB_POS = ecomProduto.FOTO_FRENTE_SEMCAB_POS;
            novo.FOTO_FRENTE_SEMCAB = ecomProduto.FOTO_FRENTE_SEMCAB;

            novo.FOTO_COSTAS_POS = ecomProduto.FOTO_COSTAS_POS;
            novo.FOTO_COSTAS = ecomProduto.FOTO_COSTAS;

            novo.FOTO_DETALHE_POS = ecomProduto.FOTO_DETALHE_POS;
            novo.FOTO_DETALHE = ecomProduto.FOTO_DETALHE;

            novo.FOTO_LADO_POS = ecomProduto.FOTO_LADO_POS;
            novo.FOTO_LADO = ecomProduto.FOTO_LADO;

            novo.FOTO_VITRINE_POS = ecomProduto.FOTO_VITRINE_POS;
            novo.FOTO_HOVER = ecomProduto.FOTO_HOVER;

            novo.HABILITADO = ecomProduto.HABILITADO;
            novo.STATUS_CADASTRO = 'B';
            novo.DATA_INCLUSAO = DateTime.Now;

            novo.USUARIO_INCLUSAO = _codigoUsuario;

            novo.GRIFFE = ecomProduto.GRIFFE;
            novo.COD_CATEGORIA = ecomProduto.COD_CATEGORIA;

            return novo;
        }
    }
}