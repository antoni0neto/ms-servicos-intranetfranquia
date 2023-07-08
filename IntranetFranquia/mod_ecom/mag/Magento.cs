using DAL;
using Relatorios.mod_producao.relatorios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Relatorios.mod_ecom.mag
{
    public class Magento
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        private int _codigoUsuario { get; set; }
        private string _pathSistema { get; set; }
        private string _mlogin { get; set; }
        private string _setId { get; set; }
        private MagentoService.MagentoService _mservice { get; set; }

        public Magento()
        {
            // autenticacao magento
            _mservice = new MagentoService.MagentoService();
            _mlogin = _mservice.login(Constante.userMagento, Constante.apiKeyMagento);
        }

        public Magento(string userMag, string passMag)
        {
            // autenticacao magento
            _mservice = new MagentoService.MagentoService();

            if (userMag == "")
                userMag = Constante.userMagento;
            if (passMag == "")
                passMag = Constante.apiKeyMagento;

            _mlogin = _mservice.login(userMag, passMag);
        }

        public Magento(int codigoUsuario, string pathSistema)
        {
            _codigoUsuario = codigoUsuario;
            _pathSistema = pathSistema;

            // autenticacao magento
            _mservice = new MagentoService.MagentoService();
            _mlogin = _mservice.login(Constante.userMagento, Constante.apiKeyMagento);

            var sets = _mservice.catalogProductAttributeSetList(_mlogin);
            _setId = sets[0].set_id.ToString();
        }

        public Magento(int codigoUsuario, string pathSistema, string userMag, string passMag)
        {
            _codigoUsuario = codigoUsuario;
            _pathSistema = pathSistema;

            if (userMag == null || userMag == "")
                userMag = Constante.userMagento;
            if (passMag == null || passMag == "")
                passMag = Constante.apiKeyMagento;

            // autenticacao magento
            _mservice = new MagentoService.MagentoService();
            _mlogin = _mservice.login(Constante.userMagento, Constante.apiKeyMagento);

            var sets = _mservice.catalogProductAttributeSetList(_mlogin);
            _setId = sets[0].set_id.ToString();

        }

        public bool SalvarProdutoMagentoEstoque(int codigoEcomProduto)
        {
            //Obter Produto Configuravel
            var ecomProdutoConfig = eController.ObterMagentoProduto(codigoEcomProduto);

            //Obter Qtdes para Estoque
            var ecomProdutoEstoque = eController.ObterProdutoLinxLiberacao(ecomProdutoConfig.COLECAO, "", "", "", ecomProdutoConfig.CODIGO).FirstOrDefault();

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
            if (ecomProdutoEstoque.QTDE_10 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_10, ecomProdutoEstoque.QTDE_10);
            if (ecomProdutoEstoque.QTDE_11 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_11, ecomProdutoEstoque.QTDE_11);
            if (ecomProdutoEstoque.QTDE_12 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_12, ecomProdutoEstoque.QTDE_12);
            if (ecomProdutoEstoque.QTDE_13 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_13, ecomProdutoEstoque.QTDE_13);
            if (ecomProdutoEstoque.QTDE_14 > 0)
                hashEstoque.Add(ecomProdutoEstoque.TAMANHO_14, ecomProdutoEstoque.QTDE_14);

            //Loop nos produtos simples
            string sku = "";
            var tamanho = "";
            var qtdeEstoque = "";
            var idsProdutoMagSimples = new List<int>();
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
                sku = (ecomProdutoConfig.SKU + "-" + ecomProdutoConfig.ECOM_COR1.COR + "-" + tamanho);

                // verificar se o produto simples ja esta cadastrado no magento
                var produtoSimplesExiste = eController.ObterMagentoProdutoSimplesTamanho(idProdutoMagConfig, tamanho);
                // se produto simples nao existe, insere e adiciona estoque inicial
                if (produtoSimplesExiste == null)
                {
                    // Insere Produto Simples no Magento + Estoque Inicial
                    var idProdutoMagSimples = InserirProdutoSimples(ecomProdutoConfig, sku, tamanho, qtdeEstoque);
                    //Insere Produto Simples na Intranet

                    var produtoSimplesN = CriarCopiaECOMPRODUTO(ecomProdutoConfig, idProdutoMagSimples, tamanho);

                    eController.InserirMagentoProduto(produtoSimplesN);

                    InserirImagens(produtoSimplesN, idProdutoMagSimples);

                    idsProdutoMagSimples.Add(idProdutoMagSimples);
                }
                else
                {
                    // se produto ja existe, faz somente a reposicao do estoque
                    AtualizarEstoque(produtoSimplesExiste, Convert.ToInt32(qtdeEstoque), false);
                    // leandro
                    //idsProdutoMagSimples.Add(Convert.ToInt32(produtoSimplesExiste.ID_PRODUTO_MAG));
                }

            }

            //Inserir configuravel
            if (idProdutoMagConfig == 0)
            {
                idProdutoMagConfig = InserirProdutoConfiguravel(ecomProdutoConfig, ecomProdutoConfig.SKU);

                //Primeiro cadastro inserir na categoria novidades
                if (ecomProdutoConfig.COD_CATEGORIA == "01")
                {
                    var codigoGrupoProduto = 0;
                    if (ecomProdutoConfig.GRIFFE.ToLower().Trim() == "feminino" || ecomProdutoConfig.GRIFFE.ToLower().Trim() == "tulp")
                        codigoGrupoProduto = 75;
                    else if (ecomProdutoConfig.GRIFFE.ToLower().Trim() == "masculino")
                        codigoGrupoProduto = 76;
                    else if (ecomProdutoConfig.GRIFFE.ToLower().Trim() == "petit")
                        codigoGrupoProduto = 158;

                    var produtoCAT = eController.ObterProdutoCategoria(ecomProdutoConfig.CODIGO, codigoGrupoProduto);
                    if (produtoCAT == null)
                    {
                        produtoCAT = new ECOM_PRODUTO_CAT();
                        produtoCAT.ECOM_PRODUTO = ecomProdutoConfig.CODIGO;
                        produtoCAT.ECOM_GRUPO_PRODUTO = codigoGrupoProduto;
                        eController.InserirProdutoCategoria(produtoCAT);
                    }
                }

            }
            else
            {
                // habilitar o produto na reposição, caso esteja desabilitado
                HabilitarProduto(ecomProdutoConfig, true);
            }

            //inserir estoque enviado deste produto
            //inserir registro que esta nota fiscal ja foi enviada
            var estoqueEnviado = new ECOM_ESTOQUE();
            estoqueEnviado.PRODUTO = ecomProdutoEstoque.PRODUTO;
            estoqueEnviado.COR = ecomProdutoEstoque.COR;
            estoqueEnviado.FILIAL = ecomProdutoEstoque.FILIAL;
            estoqueEnviado.NF_SAIDA = ecomProdutoEstoque.NF_SAIDA;
            estoqueEnviado.SERIE_NF = ecomProdutoEstoque.SERIE_NF;
            estoqueEnviado.EMISSAO = Convert.ToDateTime(ecomProdutoEstoque.EMISSAO);
            estoqueEnviado.DATA_ENVIO = DateTime.Now;
            estoqueEnviado.USUARIO_ENVIO = _codigoUsuario;
            eController.InserirMagentoEstoqueEnviado(estoqueEnviado);

            //atualizar envio do configuravel
            ecomProdutoConfig.ID_PRODUTO_MAG = idProdutoMagConfig;
            ecomProdutoConfig.STATUS_CADASTRO = 'B';
            ecomProdutoConfig.VISIBILIDADE = "4";
            eController.AtualizarMagentoProduto(ecomProdutoConfig);

            // associar produto configuravel no simples, na intranet (pai -> filho)
            foreach (var ecomProdutoId in idsProdutoMagSimples)
            {
                var ecomProd = eController.ObterMagentoProdutoId(ecomProdutoId);
                ecomProd.ID_PRODUTO_MAG_CONFIG = idProdutoMagConfig;
                eController.AtualizarMagentoProduto(ecomProd);
            }

            List<string> skuSimples = new List<string>();
            var todosProdutoSimples = eController.ObterMagentoProdutoSimples(idProdutoMagConfig, ecomProdutoConfig.PRODUTO);
            foreach (var ss in todosProdutoSimples)
                skuSimples.Add(ss.SKU);

            //associar produtos simples
            AssociarSkusMagento(ecomProdutoConfig, skuSimples.ToArray());

            return true;
        }
        public bool SalvarProdutoMagentoDevolucao(int codigoEcomProduto, string nfEntrada)
        {
            //Obter Produto Configuravel
            var ecomProdutoConfig = eController.ObterMagentoProduto(codigoEcomProduto);

            //Obter Qtdes para Estoque
            var ecomProdutoEstoqueDevolucao = eController.ObterProdutoLinxDevolucao(ecomProdutoConfig.CODIGO, nfEntrada).FirstOrDefault();

            var idProdutoMagConfig = 0;
            idProdutoMagConfig = Convert.ToInt32(ecomProdutoConfig.ID_PRODUTO_MAG);

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
            if (ecomProdutoEstoqueDevolucao.QTDE_10 > 0)
                hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_10, ecomProdutoEstoqueDevolucao.QTDE_10);
            if (ecomProdutoEstoqueDevolucao.QTDE_11 > 0)
                hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_11, ecomProdutoEstoqueDevolucao.QTDE_11);
            if (ecomProdutoEstoqueDevolucao.QTDE_12 > 0)
                hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_12, ecomProdutoEstoqueDevolucao.QTDE_12);
            if (ecomProdutoEstoqueDevolucao.QTDE_13 > 0)
                hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_13, ecomProdutoEstoqueDevolucao.QTDE_13);
            if (ecomProdutoEstoqueDevolucao.QTDE_14 > 0)
                hashEstoque.Add(ecomProdutoEstoqueDevolucao.TAMANHO_14, ecomProdutoEstoqueDevolucao.QTDE_14);


            //Loop nos produtos simples
            var tamanho = "";
            var qtdeEstoque = "";
            foreach (DictionaryEntry entry in hashEstoque)
            {
                tamanho = entry.Key.ToString();
                qtdeEstoque = entry.Value.ToString();

                if (tamanho == "UN")
                    tamanho = "Único";
                else if (tamanho == "2G")
                    tamanho = "GG";
                else if (tamanho == "3G")
                    tamanho = "XG";

                // Obter Produto Simples
                var produtoSimplesExiste = eController.ObterMagentoProdutoSimplesTamanho(idProdutoMagConfig, tamanho);

                // se produto ja existe, faz somente a reposicao do estoque
                AtualizarEstoque(produtoSimplesExiste, Convert.ToInt32(qtdeEstoque), false);
            }

            //inserir devolucao estoque deste produto
            var devIntra = new ECOM_ESTOQUE_DEV();
            devIntra.PRODUTO = ecomProdutoEstoqueDevolucao.PRODUTO;
            devIntra.COR = ecomProdutoEstoqueDevolucao.COR;
            devIntra.NOME_CLIFOR = ecomProdutoEstoqueDevolucao.NOME_CLIFOR;
            devIntra.FILIAL = ecomProdutoEstoqueDevolucao.FILIAL;
            devIntra.NF_ENTRADA = ecomProdutoEstoqueDevolucao.NF_ENTRADA;
            devIntra.SERIE_NF_ENTRADA = ecomProdutoEstoqueDevolucao.SERIE_NF_ENTRADA;
            devIntra.RECEBIMENTO = ecomProdutoEstoqueDevolucao.RECEBIMENTO;
            devIntra.DATA_ENTRADA = DateTime.Now;
            devIntra.USUARIO_ENTRADA = _codigoUsuario; ;
            eController.InserirMagentoEstoqueDevolvido(devIntra);

            return true;
        }
        public bool RetirarEstoqueMagentoPorTroca(int idProdutoMagConfig, string tamanho, int qtdeEstoque)
        {
            if (tamanho == "UN")
                tamanho = "Único";
            else if (tamanho == "2G")
                tamanho = "GG";
            else if (tamanho == "3G")
                tamanho = "XG";

            // Obter Produto Simples
            var produtoSimplesExiste = eController.ObterMagentoProdutoSimplesTamanho(idProdutoMagConfig, tamanho);

            // remove qtde do estoque
            qtdeEstoque = (qtdeEstoque * -1);
            AtualizarEstoque(produtoSimplesExiste, qtdeEstoque, false);

            return true;
        }

        private bool HabilitarProduto(ECOM_PRODUTO ecomProduto, bool habilitar)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

            product.status = (habilitar) ? "1" : "2"; // 1 - Habilitado, 2 - Desabilitado
            product.visibility = (habilitar) ? "4" : "1"; // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search

            bool updateOK = _mservice.catalogProductUpdate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), product, "", "");

            // habilitar na intranet
            ecomProduto.VISIBILIDADE = "4";
            ecomProduto.HABILITADO = 'S';
            eController.AtualizarMagentoProduto(ecomProduto);

            return updateOK;
        }
        private int InserirProdutoConfiguravel(ECOM_PRODUTO ecomProduto, string sku)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

            List<string> cats = new List<string>();

            //Categoria Grupo
            cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO.ToString());

            ////Categoria Signed
            //if (ecomProduto.ECOM_SIGNED != null && ecomProduto.ECOM_SIGNED1.CATEGORIA != null)
            //    cats.Add(ecomProduto.ECOM_SIGNED1.CATEGORIA.ToString());

            //Categoria Novidade Peças
            if (ecomProduto.COD_CATEGORIA == "01")
            {
                if (ecomProduto.GRIFFE.ToLower().Trim() == "feminino")
                    cats.Add("75");
                else if (ecomProduto.GRIFFE.ToLower().Trim() == "masculino")
                    cats.Add("76");
                else if (ecomProduto.GRIFFE.ToLower().Trim() == "petit")
                    cats.Add("158");
            }

            // Colecao FEMININO/MASCULINO
            if (ecomProduto.ECOM_GRUPO_PRODUTO1 != null && ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI != null)
                cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI.ToString());

            //Categorias Gerais
            var catsProduto = eController.ObterProdutoCategoria(ecomProduto.CODIGO);
            foreach (var c in catsProduto)
                cats.Add(c.ECOM_GRUPO_PRODUTO.ToString());

            product.category_ids = cats.ToArray();

            product.name = ecomProduto.NOME_MAG;
            product.description = ecomProduto.PRODUTO_DESC;
            product.short_description = ecomProduto.PRODUTO_DESC_CURTA;

            product.status = (ecomProduto.HABILITADO == 'S') ? "1" : "2"; // 1 - Habilitado, 2 - Desabilitado
            product.visibility = ecomProduto.VISIBILIDADE; // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
            product.tax_class_id = "0";
            product.meta_title = ecomProduto.META_TITULO;
            product.meta_keyword = ecomProduto.META_KEYWORD;
            product.meta_description = ecomProduto.META_DESCRICAO;
            product.url_path = ecomProduto.URL_PRODUTO.Trim();
            product.url_key = ecomProduto.URL_PRODUTO.Trim();

            product.weight = ecomProduto.PESO_KG.ToString().Replace(",", ".");
            product.price = ecomProduto.PRECO.ToString().Replace(",", ".");

            if (ecomProduto.PRECO_PROMO != null)
                product.special_price = ecomProduto.PRECO_PROMO.ToString().Replace(",", ".");
            if (ecomProduto.PRECO_PROMO_DATA_DE != null)
                product.special_from_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_DE).ToString("yyyy-MM-dd");
            if (ecomProduto.PRECO_PROMO_DATA_ATE != null)
                product.special_to_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_ATE).ToString("yyyy-MM-dd");

            ////associar skus
            //product.associated_skus = skuAsso;

            MagentoService.catalogProductAdditionalAttributesEntity att = new MagentoService.catalogProductAdditionalAttributesEntity();
            att.single_data = SalvarAtributos(ecomProduto, true, "");

            product.additional_attributes = att;

            //ESTOQUE
            MagentoService.catalogInventoryStockItemUpdateEntity estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
            estoque.manage_stock = 1;
            estoque.is_in_stock = 1;
            estoque.manage_stockSpecified = true;
            estoque.is_in_stockSpecified = true;
            product.stock_data = estoque;

            int idProduto = 0;
            idProduto = _mservice.catalogProductCreate(_mlogin, "configurable", _setId, sku, product, "");

            InserirImagens(ecomProduto, idProduto);

            return idProduto;
        }
        private int InserirProdutoSimples(ECOM_PRODUTO ecomProduto, string sku, string tamanho, string qtde)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

            //string[] cat = { ecomProduto.ECOM_GRUPO_PRODUTO.ToString() };
            //product.category_ids = cat;

            product.name = ecomProduto.NOME_MAG + " - " + ecomProduto.ECOM_COR1.COR + " - " + tamanho;
            product.description = ecomProduto.PRODUTO_DESC;
            product.short_description = ecomProduto.PRODUTO_DESC_CURTA;

            product.weight = ecomProduto.PESO_KG.ToString().Replace(",", ".");
            product.status = (ecomProduto.HABILITADO == 'S') ? "1" : "2"; // 1 - Habilitado, 2 - Desabilitado
            product.visibility = "1"; // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
            product.tax_class_id = "0";
            product.meta_title = ecomProduto.META_TITULO;
            product.meta_keyword = ecomProduto.META_KEYWORD;
            product.meta_description = ecomProduto.META_DESCRICAO;
            //product.url_path = ecomProduto.URL_PRODUTO;
            //product.url_key = ecomProduto.URL_PRODUTO;

            product.price = ecomProduto.PRECO.ToString().Replace(",", ".");

            if (ecomProduto.PRECO_PROMO != null)
                product.special_price = ecomProduto.PRECO_PROMO.ToString().Replace(",", ".");
            if (ecomProduto.PRECO_PROMO_DATA_DE != null)
                product.special_from_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_DE).ToString("yyyy-MM-dd");
            if (ecomProduto.PRECO_PROMO_DATA_ATE != null)
                product.special_to_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_ATE).ToString("yyyy-MM-dd");

            //estoque inicial
            product.stock_data = InserirEstoque(qtde);

            MagentoService.catalogProductAdditionalAttributesEntity att = new MagentoService.catalogProductAdditionalAttributesEntity();
            att.single_data = SalvarAtributos(ecomProduto, false, tamanho);

            product.additional_attributes = att;

            int idProduto = 0;

            idProduto = _mservice.catalogProductCreate(_mlogin, "simple", _setId, sku, product, "");

            //InserirImagens(ecomProduto, idProduto);

            return idProduto;
        }
        private MagentoService.catalogInventoryStockItemUpdateEntity InserirEstoque(string qtdeEstoque)
        {
            //ESTOQUE
            MagentoService.catalogInventoryStockItemUpdateEntity estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
            estoque.qty = qtdeEstoque;
            estoque.is_in_stock = 1;
            estoque.manage_stock = 1;
            estoque.manage_stockSpecified = true;
            estoque.is_in_stockSpecified = true;

            return estoque;
        }
        private bool InserirImagens(ECOM_PRODUTO ecomProduto, int idProduto)
        {
            Hashtable hashFoto = new Hashtable();

            bool grupo6Foto = (ecomProduto.COD_CATEGORIA == "01") ? true : false;

            if (grupo6Foto)
                hashFoto.Add(ecomProduto.FOTO_LOOK_POS.ToString(), ecomProduto.FOTO_LOOK);

            hashFoto.Add(ecomProduto.FOTO_FRENTE_CAB_POS.ToString(), ecomProduto.FOTO_FRENTE_CAB);
            if (grupo6Foto)
                hashFoto.Add(ecomProduto.FOTO_FRENTE_SEMCAB_POS.ToString(), ecomProduto.FOTO_FRENTE_SEMCAB);

            hashFoto.Add(ecomProduto.FOTO_COSTAS_POS.ToString(), ecomProduto.FOTO_COSTAS);
            hashFoto.Add(ecomProduto.FOTO_DETALHE_POS.ToString(), ecomProduto.FOTO_DETALHE);
            if (grupo6Foto)
                hashFoto.Add(ecomProduto.FOTO_LADO_POS.ToString(), ecomProduto.FOTO_LADO);

            if (ecomProduto.FOTO_HOVER != null)
                hashFoto.Add("7", ecomProduto.FOTO_HOVER);

            MagentoService.catalogProductImageFileEntity f = null;
            MagentoService.catalogProductAttributeMediaCreateEntity imageP = null;
            string pathFoto = "";
            string pathFotoMag = "";

            foreach (DictionaryEntry entry in hashFoto)
            {
                f = new MagentoService.catalogProductImageFileEntity();
                pathFoto = "";

                pathFoto = @_pathSistema + entry.Value.ToString().Replace("~", "").Replace("/", "\\");

                Byte[] bytes = File.ReadAllBytes(pathFoto);
                String fileBase64 = Convert.ToBase64String(bytes);

                if (ecomProduto.TAMANHO == null)
                    ecomProduto.TAMANHO = "";

                f.content = fileBase64;
                f.mime = "image/jpeg";
                f.name = "handbook-" + ecomProduto.GRUPO_PRODUTO.ToLower() + "-" + ecomProduto.ECOM_COR1.COR.ToLower() + "-" + ecomProduto.TAMANHO.Trim().ToUpper() + "-" + entry.Key.ToString();

                //IMAGEM
                imageP = new MagentoService.catalogProductAttributeMediaCreateEntity();
                imageP.label = "Handbook " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.GRUPO_PRODUTO.ToLower()) + " " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.ECOM_COR1.COR.ToLower()) + " " + ecomProduto.TAMANHO.Trim().ToUpper() + " " + entry.Key.ToString();
                imageP.position = entry.Key.ToString();
                imageP.file = f;

                List<string> types = new List<string>();
                if (entry.Key.ToString() == ecomProduto.FOTO_VITRINE_POS.ToString())
                {
                    types.Add("small_image");
                    types.Add("thumbnail");
                    types.Add("image");
                }

                imageP.types = types.ToArray();

                if (entry.Key.ToString() == "1" || entry.Key.ToString() == "2" || entry.Key.ToString() == "3" || entry.Key.ToString() == "4" || entry.Key.ToString() == "5")
                    imageP.exclude = "0";
                else if (entry.Key.ToString() == "6" || entry.Key.ToString() == "7")
                    imageP.exclude = "1";

                pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, idProduto.ToString(), imageP, "", "");

                //string nomeFotoIntra = entry.Value.ToString().Split('/')[2].Split('.')[0].ToString();
                if (ecomProduto.FOTO_LOOK_POS != null && imageP.position == ecomProduto.FOTO_LOOK_POS.ToString())
                    ecomProduto.FOTO_LOOK_MAG = pathFotoMag;
                else if (ecomProduto.FOTO_FRENTE_CAB_POS != null && imageP.position == ecomProduto.FOTO_FRENTE_CAB_POS.ToString())
                    ecomProduto.FOTO_FRENTE_CAB_MAG = pathFotoMag;
                else if (ecomProduto.FOTO_FRENTE_SEMCAB_POS != null && imageP.position == ecomProduto.FOTO_FRENTE_SEMCAB_POS.ToString())
                    ecomProduto.FOTO_FRENTE_SEMCAB_MAG = pathFotoMag;
                else if (ecomProduto.FOTO_COSTAS_POS != null && imageP.position == ecomProduto.FOTO_COSTAS_POS.ToString())
                    ecomProduto.FOTO_COSTAS_MAG = pathFotoMag;
                else if (ecomProduto.FOTO_DETALHE_POS != null && imageP.position == ecomProduto.FOTO_DETALHE_POS.ToString())
                    ecomProduto.FOTO_DETALHE_MAG = pathFotoMag;
                else if (ecomProduto.FOTO_LADO_POS != null && imageP.position == ecomProduto.FOTO_LADO_POS.ToString())
                    ecomProduto.FOTO_LADO_MAG = pathFotoMag;
                else if (imageP.position == "7")
                    ecomProduto.FOTO_HOVER_MAG = pathFotoMag;

                eController.AtualizarMagentoProduto(ecomProduto);
            }

            return true;
        }

        private MagentoService.associativeEntity[] SalvarAtributos(ECOM_PRODUTO ecomProduto, bool configuravel, string tamanho)
        {
            int totalAttr = 0;
            Hashtable hash = new Hashtable();

            if (ecomProduto.ECOM_TIPO_ESTILO != null)
            {
                hash.Add("tipo_estilos", ecomProduto.ECOM_TIPO_ESTILO1.TIPO_ESTILO);
                totalAttr += 1;
            }
            if (ecomProduto.ECOM_TIPO_MODELAGEM != null)
            {
                hash.Add("tipo_modelagem", ecomProduto.ECOM_TIPO_MODELAGEM1.TIPO_MODELAGEM);
                totalAttr += 1;
            }
            if (ecomProduto.ECOM_TIPO_TECIDO != null)
            {
                hash.Add("tipo_tecido", ecomProduto.ECOM_TIPO_TECIDO1.TIPO_TECIDO);
                totalAttr += 1;
            }
            if (ecomProduto.ECOM_TIPO_MANGA != null)
            {
                hash.Add("tipo_manga", ecomProduto.ECOM_TIPO_MANGA1.TIPO_MANGA);
                totalAttr += 1;
            }
            if (ecomProduto.ECOM_TIPO_GOLA != null)
            {
                hash.Add("tipo_gola", ecomProduto.ECOM_TIPO_GOLA1.TIPO_GOLA);
                totalAttr += 1;
            }
            if (ecomProduto.ECOM_TIPO_COMPRIMENTO != null)
            {
                hash.Add("tipo_comprimento", ecomProduto.ECOM_TIPO_COMPRIMENTO1.TIPO_COMPRIMENTO);
                totalAttr += 1;
            }
            if (ecomProduto.ECOM_TIPO_LINHA != null)
            {
                hash.Add("tipo_linha", ecomProduto.ECOM_TIPO_LINHA1.TIPO_LINHA);
                totalAttr += 1;
            }
            else
            {
                hash.Add("tipo_linha", "");
                totalAttr += 1;
            }

            if (ecomProduto.ECOM_SIGNED != null)
            {
                hash.Add("signed", ecomProduto.ECOM_SIGNED1.SIGNED);
                totalAttr += 1;
            }
            else
            {
                hash.Add("signed", "");
                totalAttr += 1;
            }

            if (tamanho != "")
            {
                hash.Add("tamanho", tamanho);
                totalAttr += 1;
            }

            var ncm = baseController.BuscaProduto(ecomProduto.PRODUTO);
            if (ncm != null)
            {
                if (ncm.CLASSIF_FISCAL.Trim() != "")
                {
                    hash.Add("ncm", ncm.CLASSIF_FISCAL.Trim());
                    totalAttr += 1;
                }
            }

            //Regra - (Valor Promocional/Valor Real)*100
            //Se não tiver valor promocional, colocar no campo o valor 999.99.
            //Importante os valores estarem com até 2 decimais.

            var porcentagemDesconto = 999.99M;
            if (ecomProduto.PRECO_PROMO != null && ecomProduto.PRECO_PROMO > 0)
                porcentagemDesconto = ((Convert.ToDecimal(ecomProduto.PRECO_PROMO) / Convert.ToDecimal(ecomProduto.PRECO)) * 100.00M);

            hash.Add("discount_percent", Math.Round(porcentagemDesconto, 2));
            totalAttr += 1;

            hash.Add("color", ecomProduto.ECOM_COR1.COR);
            totalAttr += 1;

            hash.Add("cor_produto", ecomProduto.COR);
            totalAttr += 1;

            hash.Add("macro", ecomProduto.ECOM_GRUPO_MACRO1.GRUPO_MACRO);
            totalAttr += 1;

            hash.Add("produto", ecomProduto.PRODUTO);
            totalAttr += 1;

            // dias uteis
            hash.Add("prazo_adicional", "1");
            totalAttr += 1;

            var ignoreMkt = (Convert.ToBoolean(ecomProduto.CAD_MKTPLACE)) ? "0" : "1";
            hash.Add("ignore_marketplace", ignoreMkt);
            totalAttr += 1;

            var grupoProduto = ecomProduto.GRUPO_PRODUTO.Trim().ToLower();

            grupoProduto = grupoProduto.Replace("basic", "camiseta");
            grupoProduto = grupoProduto.Replace("calca", "calça");
            grupoProduto = grupoProduto.Replace("macacao", "macacão");
            grupoProduto = grupoProduto.Replace("malhao", "malhão");
            grupoProduto = grupoProduto.Replace("sandalia", "sandália");
            grupoProduto = grupoProduto.Replace("tenis", "tênis");
            grupoProduto = grupoProduto.Replace("moleton", "moletom");
            grupoProduto = grupoProduto.Replace("oculos", "Óculos");


            grupoProduto = Utils.WebControls.AlterarPrimeiraLetraMaiscula(grupoProduto);

            hash.Add("grupo_produto", grupoProduto);
            totalAttr += 1;

            if (ecomProduto.GRIFFE != null)
            {
                hash.Add("griffe", ecomProduto.GRIFFE.Trim());
                totalAttr += 1;
            }


            // controle dos correios
            hash.Add("volume_comprimento", ecomProduto.ECOM_CAIXA_DIMENSAO1.COMPRIMENTO_CM.ToString());
            totalAttr += 1;
            hash.Add("volume_largura", ecomProduto.ECOM_CAIXA_DIMENSAO1.LARGURA_CM.ToString());
            totalAttr += 1;
            hash.Add("volume_altura", ecomProduto.ECOM_CAIXA_DIMENSAO1.ALTURA_CM.ToString());
            totalAttr += 1;

            if (ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_GOOGLE_CAT != null)
            {
                hash.Add("googleshopping_category", ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_GOOGLE_CAT);
                totalAttr += 1;
            }

            MagentoService.associativeEntity[] asso = new MagentoService.associativeEntity[totalAttr];

            totalAttr = 0;
            foreach (DictionaryEntry entry in hash)
            {
                asso[totalAttr] = new MagentoService.associativeEntity();
                asso[totalAttr].key = entry.Key.ToString();
                asso[totalAttr].value = entry.Value.ToString();

                totalAttr += 1;
            }

            return asso;
        }

        //OUTROS
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
            //novo.FOTO_LOOK_MAG = ecomProduto.FOTO_LOOK_MAG;

            novo.FOTO_FRENTE_CAB_POS = ecomProduto.FOTO_FRENTE_CAB_POS;
            novo.FOTO_FRENTE_CAB = ecomProduto.FOTO_FRENTE_CAB;
            //novo.FOTO_FRENTE_CAB_MAG = ecomProduto.FOTO_FRENTE_CAB_MAG;

            novo.FOTO_FRENTE_SEMCAB_POS = ecomProduto.FOTO_FRENTE_SEMCAB_POS;
            novo.FOTO_FRENTE_SEMCAB = ecomProduto.FOTO_FRENTE_SEMCAB;
            //novo.FOTO_FRENTE_SEMCAB_MAG = ecomProduto.FOTO_FRENTE_SEMCAB_MAG;

            novo.FOTO_COSTAS_POS = ecomProduto.FOTO_COSTAS_POS;
            novo.FOTO_COSTAS = ecomProduto.FOTO_COSTAS;
            //novo.FOTO_COSTAS_MAG = ecomProduto.FOTO_COSTAS_MAG;

            novo.FOTO_DETALHE_POS = ecomProduto.FOTO_DETALHE_POS;
            novo.FOTO_DETALHE = ecomProduto.FOTO_DETALHE;
            //novo.FOTO_DETALHE_MAG = ecomProduto.FOTO_DETALHE_MAG;

            novo.FOTO_LADO_POS = ecomProduto.FOTO_LADO_POS;
            novo.FOTO_LADO = ecomProduto.FOTO_LADO;
            //novo.FOTO_LADO_MAG = ecomProduto.FOTO_LADO_MAG;

            novo.FOTO_VITRINE_POS = ecomProduto.FOTO_VITRINE_POS;
            novo.FOTO_HOVER = ecomProduto.FOTO_HOVER;
            //novo.FOTO_HOVER_MAG = ecomProduto.FOTO_HOVER_MAG;

            novo.HABILITADO = ecomProduto.HABILITADO;
            novo.STATUS_CADASTRO = 'B';
            novo.DATA_INCLUSAO = DateTime.Now;

            novo.USUARIO_INCLUSAO = _codigoUsuario;

            novo.GRIFFE = ecomProduto.GRIFFE;
            novo.COD_CATEGORIA = ecomProduto.COD_CATEGORIA;

            return novo;
        }

        public int AtualizarProdutoMagento(ECOM_PRODUTO ecomProduto, bool updateImage)
        {
            var prodSimples = eController.ObterMagentoProdutoSimples(Convert.ToInt32(ecomProduto.ID_PRODUTO_MAG), ecomProduto.PRODUTO);
            foreach (var produtoSimples in prodSimples)
            {
                AtualizarProdutoSimples(produtoSimples);
                //if (updateImage)
                //    AtualizarImagens(produtoSimples);
            }

            bool updateOK = AtualizarProdutoConfiguravel(ecomProduto);

            if (updateImage)
                AtualizarImagens(ecomProduto);

            return 0;
        }
        public bool AtualizarProdutoConfiguravel(ECOM_PRODUTO ecomProduto)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

            List<string> cats = new List<string>();

            //Categoria Grupo
            cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO.ToString());

            ////Categoria Signed
            //if (ecomProduto.ECOM_SIGNED != null && ecomProduto.ECOM_SIGNED1.CATEGORIA != null)
            //    cats.Add(ecomProduto.ECOM_SIGNED1.CATEGORIA.ToString());

            ////Categoria Novidade Peças
            //if (ecomProduto.COD_CATEGORIA == "01")
            //{
            //    if (ecomProduto.GRIFFE.ToLower().Trim() == "feminino")
            //        cats.Add("75");
            //    else if (ecomProduto.GRIFFE.ToLower().Trim() == "masculino")
            //        cats.Add("76");
            //    else if (ecomProduto.GRIFFE.ToLower().Trim() == "petit")
            //        cats.Add("158");
            //}

            // Colecao FEMININO/MASCULINO
            if (ecomProduto.ECOM_GRUPO_PRODUTO1 != null && ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI != null)
                cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI.ToString());

            //Categorias Gerais
            var catsProduto = eController.ObterProdutoCategoria(ecomProduto.CODIGO);
            foreach (var c in catsProduto)
                cats.Add(c.ECOM_GRUPO_PRODUTO.ToString());

            product.category_ids = cats.ToArray();

            product.name = ecomProduto.NOME_MAG;
            product.description = ecomProduto.PRODUTO_DESC;
            product.short_description = ecomProduto.PRODUTO_DESC_CURTA;

            product.status = (ecomProduto.HABILITADO == 'S') ? "1" : "2"; // 1 - Habilitado, 2 - Desabilitado
            product.visibility = ecomProduto.VISIBILIDADE; // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
            product.tax_class_id = "0";
            product.meta_title = ecomProduto.META_TITULO;
            product.meta_keyword = ecomProduto.META_KEYWORD;
            product.meta_description = ecomProduto.META_DESCRICAO;
            product.url_path = ecomProduto.URL_PRODUTO;
            product.url_key = ecomProduto.URL_PRODUTO;

            product.weight = ecomProduto.PESO_KG.ToString().Replace(",", ".");
            product.price = ecomProduto.PRECO.ToString().Replace(",", ".");

            if (ecomProduto.PRECO_PROMO != null)
                product.special_price = ecomProduto.PRECO_PROMO.ToString().Replace(",", ".");
            else
                product.special_price = "";

            if (ecomProduto.PRECO_PROMO_DATA_DE != null)
                product.special_from_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_DE).ToString("yyyy-MM-dd");
            else
                product.special_from_date = "";

            if (ecomProduto.PRECO_PROMO_DATA_ATE != null)
                product.special_to_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_ATE).ToString("yyyy-MM-dd");
            else
                product.special_to_date = "";

            ////associar skus
            //product.associated_skus = skuAsso;

            MagentoService.catalogProductAdditionalAttributesEntity att = new MagentoService.catalogProductAdditionalAttributesEntity();
            att.single_data = SalvarAtributos(ecomProduto, true, "");

            product.additional_attributes = att;

            //ESTOQUE
            MagentoService.catalogInventoryStockItemUpdateEntity estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
            estoque.manage_stock = 1;
            estoque.is_in_stock = 1;
            estoque.manage_stockSpecified = true;
            estoque.is_in_stockSpecified = true;
            product.stock_data = estoque;

            bool updateOK = _mservice.catalogProductUpdate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), product, "", "");

            return updateOK;
        }
        private bool AtualizarProdutoSimples(ECOM_PRODUTO ecomProduto)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

            //List<string> cats = new List<string>();
            //cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO.ToString());
            //var catsProduto = eController.ObterMagentoProdutoCategoriaPorCodigoProduto(ecomProduto.CODIGO);
            //foreach (var c in catsProduto)
            //    cats.Add(c.ECOM_CATEGORIA.ToString());

            //product.category_ids = cats.ToArray();
            product.name = ecomProduto.NOME_MAG + " - " + ecomProduto.ECOM_COR1.COR + " - " + ecomProduto.TAMANHO;
            product.description = ecomProduto.PRODUTO_DESC;
            product.short_description = ecomProduto.PRODUTO_DESC_CURTA;

            product.weight = ecomProduto.PESO_KG.ToString().Replace(",", ".");
            product.status = (ecomProduto.HABILITADO == 'S') ? "1" : "2"; // 1 - Habilitado, 2 - Desabilitado
            product.visibility = "1"; // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
            product.tax_class_id = "0";
            product.meta_title = ecomProduto.META_TITULO;
            product.meta_keyword = ecomProduto.META_KEYWORD;
            product.meta_description = ecomProduto.META_DESCRICAO;
            //product.url_path = ecomProduto.URL_PRODUTO;
            //product.url_key = ecomProduto.URL_PRODUTO;

            product.price = ecomProduto.PRECO.ToString().Replace(",", ".");

            if (ecomProduto.PRECO_PROMO != null)
                product.special_price = ecomProduto.PRECO_PROMO.ToString().Replace(",", ".");
            else
                product.special_price = "";

            if (ecomProduto.PRECO_PROMO_DATA_DE != null)
                product.special_from_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_DE).ToString("yyyy-MM-dd");
            else
                product.special_from_date = "";

            if (ecomProduto.PRECO_PROMO_DATA_ATE != null)
                product.special_to_date = Convert.ToDateTime(ecomProduto.PRECO_PROMO_DATA_ATE).ToString("yyyy-MM-dd");
            else
                product.special_to_date = "";

            MagentoService.catalogProductAdditionalAttributesEntity att = new MagentoService.catalogProductAdditionalAttributesEntity();
            att.single_data = SalvarAtributos(ecomProduto, false, "");

            product.additional_attributes = att;

            bool updateOK = _mservice.catalogProductUpdate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), product, "", "");

            return true;
        }
        private bool AtualizarImagens(ECOM_PRODUTO ecomProduto)
        {
            Hashtable hashFoto = new Hashtable();

            bool grupo6Foto = (ecomProduto.COD_CATEGORIA == "01") ? true : false;

            if (grupo6Foto)
                hashFoto.Add(ecomProduto.FOTO_LOOK_POS.ToString(), ecomProduto.FOTO_LOOK);

            hashFoto.Add(ecomProduto.FOTO_FRENTE_CAB_POS.ToString(), ecomProduto.FOTO_FRENTE_CAB);
            if (grupo6Foto)
                hashFoto.Add(ecomProduto.FOTO_FRENTE_SEMCAB_POS.ToString(), ecomProduto.FOTO_FRENTE_SEMCAB);

            hashFoto.Add(ecomProduto.FOTO_COSTAS_POS.ToString(), ecomProduto.FOTO_COSTAS);
            hashFoto.Add(ecomProduto.FOTO_DETALHE_POS.ToString(), ecomProduto.FOTO_DETALHE);
            if (grupo6Foto)
                hashFoto.Add(ecomProduto.FOTO_LADO_POS.ToString(), ecomProduto.FOTO_LADO);
            hashFoto.Add("7", ecomProduto.FOTO_HOVER);

            MagentoService.catalogProductImageFileEntity f = null;
            MagentoService.catalogProductAttributeMediaCreateEntity imageP = null;
            string pathFoto = "";

            foreach (DictionaryEntry entry in hashFoto)
            {
                f = new MagentoService.catalogProductImageFileEntity();
                pathFoto = "";

                pathFoto = @_pathSistema + entry.Value.ToString().Replace("~", "").Replace("/", "\\");

                Byte[] bytes = File.ReadAllBytes(pathFoto);
                String fileBase64 = Convert.ToBase64String(bytes);

                if (ecomProduto.TAMANHO == null)
                    ecomProduto.TAMANHO = "";

                f.content = fileBase64;
                f.mime = "image/jpeg";
                f.name = "handbook-" + ecomProduto.GRUPO_PRODUTO.ToLower() + "-" + ecomProduto.ECOM_COR1.COR.ToLower() + "-" + ecomProduto.TAMANHO.Trim().ToUpper() + "-" + entry.Key.ToString();

                //IMAGEM
                imageP = new MagentoService.catalogProductAttributeMediaCreateEntity();
                imageP.label = "Handbook " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.GRUPO_PRODUTO.ToLower()) + " " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ecomProduto.ECOM_COR1.COR.ToLower()) + " " + ecomProduto.TAMANHO.ToLower() + " " + entry.Key.ToString();
                imageP.position = entry.Key.ToString();
                imageP.file = f;

                List<string> types = new List<string>();
                if (entry.Key.ToString() == ecomProduto.FOTO_VITRINE_POS.ToString())
                {
                    types.Add("small_image");
                    types.Add("thumbnail");
                    types.Add("image");
                }

                imageP.types = types.ToArray();

                if (entry.Key.ToString() == "1" || entry.Key.ToString() == "2" || entry.Key.ToString() == "3" || entry.Key.ToString() == "4" || entry.Key.ToString() == "5")
                    imageP.exclude = "0";
                else if (entry.Key.ToString() == "6" || entry.Key.ToString() == "7")
                    imageP.exclude = "1";

                string nomeFotoIntra = entry.Value.ToString().Split('/')[2].Split('.')[0].ToString();

                if (ecomProduto.FOTO_LOOK_POS != null && imageP.position == ecomProduto.FOTO_LOOK_POS.ToString())
                {

                    var excludeOK = true;
                    if (ecomProduto.FOTO_LOOK_MAG != null)
                        excludeOK = ExcluirImagemMagento(ecomProduto.ID_PRODUTO_MAG.ToString(), ecomProduto.FOTO_LOOK_MAG);

                    var pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), imageP, "", "");

                    if (excludeOK && pathFotoMag != "")
                    {
                        ecomProduto.FOTO_LOOK_MAG = pathFotoMag;
                        eController.AtualizarMagentoProduto(ecomProduto);
                    }
                }
                else if (ecomProduto.FOTO_FRENTE_CAB_POS != null && imageP.position == ecomProduto.FOTO_FRENTE_CAB_POS.ToString())
                {

                    var excludeOK = true;
                    if (ecomProduto.FOTO_FRENTE_CAB_MAG != null)
                        excludeOK = ExcluirImagemMagento(ecomProduto.ID_PRODUTO_MAG.ToString(), ecomProduto.FOTO_FRENTE_CAB_MAG);

                    var pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), imageP, "", "");

                    if (excludeOK && pathFotoMag != "")
                    {
                        ecomProduto.FOTO_FRENTE_CAB_MAG = pathFotoMag;
                        eController.AtualizarMagentoProduto(ecomProduto);
                    }
                }
                else if (ecomProduto.FOTO_FRENTE_SEMCAB_POS != null && imageP.position == ecomProduto.FOTO_FRENTE_SEMCAB_POS.ToString())
                {

                    var excludeOK = true;
                    if (ecomProduto.FOTO_FRENTE_SEMCAB_MAG != null)
                        excludeOK = ExcluirImagemMagento(ecomProduto.ID_PRODUTO_MAG.ToString(), ecomProduto.FOTO_FRENTE_SEMCAB_MAG);

                    var pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), imageP, "", "");

                    if (excludeOK && pathFotoMag != "")
                    {
                        ecomProduto.FOTO_FRENTE_SEMCAB_MAG = pathFotoMag;
                        eController.AtualizarMagentoProduto(ecomProduto);
                    }
                }
                else if (ecomProduto.FOTO_COSTAS_POS != null && imageP.position == ecomProduto.FOTO_COSTAS_POS.ToString())
                {

                    var excludeOK = true;
                    if (ecomProduto.FOTO_COSTAS_MAG != null)
                        excludeOK = ExcluirImagemMagento(ecomProduto.ID_PRODUTO_MAG.ToString(), ecomProduto.FOTO_COSTAS_MAG);

                    var pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), imageP, "", "");

                    if (excludeOK && pathFotoMag != "")
                    {
                        ecomProduto.FOTO_COSTAS_MAG = pathFotoMag;
                        eController.AtualizarMagentoProduto(ecomProduto);
                    }
                }
                else if (ecomProduto.FOTO_DETALHE_POS != null && imageP.position == ecomProduto.FOTO_DETALHE_POS.ToString())
                {
                    var excludeOK = true;
                    if (ecomProduto.FOTO_DETALHE_MAG != null)
                        excludeOK = ExcluirImagemMagento(ecomProduto.ID_PRODUTO_MAG.ToString(), ecomProduto.FOTO_DETALHE_MAG);

                    var pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), imageP, "", "");

                    if (excludeOK && pathFotoMag != "")
                    {
                        ecomProduto.FOTO_DETALHE_MAG = pathFotoMag;
                        eController.AtualizarMagentoProduto(ecomProduto);
                    }
                }
                else if (ecomProduto.FOTO_LADO_POS != null && imageP.position == ecomProduto.FOTO_LADO_POS.ToString())
                {
                    var excludeOK = true;
                    if (ecomProduto.FOTO_LADO_MAG != null)
                        excludeOK = ExcluirImagemMagento(ecomProduto.ID_PRODUTO_MAG.ToString(), ecomProduto.FOTO_LADO_MAG);

                    var pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), imageP, "", "");

                    if (excludeOK && pathFotoMag != "")
                    {
                        ecomProduto.FOTO_LADO_MAG = pathFotoMag;
                        eController.AtualizarMagentoProduto(ecomProduto);
                    }
                }
                else if (imageP.position == "7")
                {

                    var excludeOK = true;
                    if (ecomProduto.FOTO_HOVER_MAG != null)
                        excludeOK = ExcluirImagemMagento(ecomProduto.ID_PRODUTO_MAG.ToString(), ecomProduto.FOTO_HOVER_MAG);

                    var pathFotoMag = _mservice.catalogProductAttributeMediaCreate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), imageP, "", "");

                    if (excludeOK && pathFotoMag != "")
                    {
                        ecomProduto.FOTO_HOVER_MAG = pathFotoMag;
                        eController.AtualizarMagentoProduto(ecomProduto);
                    }
                }


            }

            return true;
        }

        private bool ExcluirImagemMagento(string idProdutoMag, string url)
        {
            try
            {
                return _mservice.catalogProductAttributeMediaRemove(_mlogin, idProdutoMag, url, "");
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool AtualizarEstoque(ECOM_PRODUTO ecomProduto, int qtdeEstoque, bool overwrite)
        {
            string[] products = { ecomProduto.ID_PRODUTO_MAG.ToString() };

            MagentoService.catalogInventoryStockItemEntity[] estoqueItem = null;
            estoqueItem = _mservice.catalogInventoryStockItemList(_mlogin, products);

            string qtdeProdutoAtual = estoqueItem[0].qty;

            if (overwrite)
                qtdeProdutoAtual = qtdeEstoque.ToString();
            else
                qtdeProdutoAtual = (Convert.ToDecimal(qtdeProdutoAtual.Replace(".", ",")) + qtdeEstoque).ToString();

            MagentoService.catalogInventoryStockItemUpdateEntity estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
            estoque.qty = qtdeProdutoAtual;

            if (Convert.ToDecimal(qtdeProdutoAtual) > 0)
                estoque.is_in_stock = 1;
            else
                estoque.is_in_stock = 0;
            estoque.is_in_stockSpecified = true;

            estoque.manage_stock = 1;
            estoque.manage_stockSpecified = true;

            _mservice.catalogInventoryStockItemUpdate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), estoque);

            HabilitarEstoqueMagento(ecomProduto.ID_PRODUTO_MAG_CONFIG.ToString());

            return true;
        }

        private bool HabilitarEstoqueMagento(string idProdutoConfig)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();
            product.visibility = "4"; // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search

            //ESTOQUE
            MagentoService.catalogInventoryStockItemUpdateEntity estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
            estoque.manage_stock = 1;
            estoque.is_in_stock = 1;
            estoque.manage_stockSpecified = true;
            estoque.is_in_stockSpecified = true;
            product.stock_data = estoque;

            bool updateOK = _mservice.catalogProductUpdate(_mlogin, idProdutoConfig, product, "", "");

            return updateOK;
        }

        public void ObterEstoqueMagento(List<string> idsMagento, int codigoUsuario)
        {
            string[] products = idsMagento.ToArray();
            var estoqueItem = _mservice.catalogInventoryStockItemList(_mlogin, products);

            // utilizo o codigo do usuario para nao concorrer com outros usuarios
            eController.ExcluirEstoqueMagentoTemp(codigoUsuario);

            var estoqueList = new List<ECOM_ESTOQUE_TEMP>();
            foreach (var es in estoqueItem)
            {

                //var produtoMag = _mservice.catalogProductInfo(_mlogin, es.product_id, "", null, "");

                estoqueList.Add(new ECOM_ESTOQUE_TEMP
                {
                    PRODUCT_ID = es.product_id,
                    SKU = es.sku,
                    QTY = es.qty,
                    IS_IN_STOCK = es.is_in_stock,
                    USUARIO = codigoUsuario,
                    VISIBILIDADE = ""
                });

            }

            eController.InserirEstoqueMagentoTempDapper(estoqueList);



            //foreach (var es in estoqueItem)
            //{
            //    eController.InserirEstoqueMagentoTemp(new ECOM_ESTOQUE_TEMP
            //    {
            //        PRODUCT_ID = es.product_id,
            //        SKU = es.sku,
            //        QTY = es.qty,
            //        IS_IN_STOCK = es.is_in_stock,
            //        USUARIO = codigoUsuario
            //    });
            //}
        }

        //associar SKUS
        public bool AssociarSkus(string produtoLinx)
        {
            List<string> skuAsso = null;
            var produtoAssociacaoConfig = eController.ObterMagentoProdutoAssociacaoConfig(produtoLinx);

            foreach (var pConfig in produtoAssociacaoConfig)
            {
                var produtosAssociacao = eController.ObterMagentoProdutoAssociacao(pConfig.PRODUTO);

                skuAsso = new List<string>();
                foreach (var pa in produtosAssociacao)
                {
                    skuAsso.Add(pa.SKU);
                }

                AssociarSkusMagento(pConfig, skuAsso.ToArray());
            }

            return true;
        }
        private bool AssociarSkusMagento(ECOM_PRODUTO ecomProduto, string[] skuAsso)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

            //associar skus
            product.associated_skus = skuAsso;

            bool updateOK = _mservice.catalogProductUpdate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), product, "", "");

            return updateOK;
        }

        //Criar Entrega (Transportes)
        public bool SalvarEntregaMagento(string pedido, string pedidoExterno, List<string> trackNumbers, bool entregaMandae, bool gerarEtiqueta, bool mktplace, string chaveNFE, string msgParaCliente)
        {

            var carrier = "";
            var carrierTitulo = "";

            if (entregaMandae)
            {
                carrier = "mandae";
                carrierTitulo = "Mandaê";
            }
            else
            {
                carrier = "pedroteixeira_correios";
                carrierTitulo = "Correios";
            }

            //Salvar Entrega
            var pedidoEntrega = eController.ObterPedidoEntregaPorPedidoLinx(pedido);
            if (pedidoEntrega == null)
            {
                //adicionar chaveNFE nos comentarios do pedido para leitura do MKP
                var msgChave = "chave de acesso: " + chaveNFE;
                var orderStatus = _mservice.salesOrderAddComment(_mlogin, pedidoExterno, "complete_shipped", msgChave, "0");

                var shipmentId = _mservice.salesOrderShipmentCreate(_mlogin, pedidoExterno, null, msgChave, 0, ((mktplace) ? 0 : 1));

                pedidoEntrega = new ECOM_PEDIDO_ENTREGA_MAG();
                pedidoEntrega.PEDIDO = pedido;
                pedidoEntrega.PEDIDO_EXTERNO = pedidoExterno;
                pedidoEntrega.PEDIDO_ENTREGA = shipmentId;
                pedidoEntrega.CHAVE_NFE = chaveNFE;

                if (!gerarEtiqueta && !mktplace)
                {
                    carrier = "custom";
                    carrierTitulo = "Loja";

                    //finalizar pedido
                    _mservice.salesOrderAddComment(_mlogin, pedidoExterno, "complete", "", "0");
                }

                if (mktplace)
                {
                    carrier = "custom";
                    carrierTitulo = "B2W";
                }

                pedidoEntrega.CARRIER = carrier;
                eController.InserirPedidoEntrega(pedidoEntrega);
            }

            bool sendInfo = false;
            bool enviaEmail = false;
            if (pedidoEntrega != null && pedidoEntrega.EMAIL_ENVIADO != true)
            {
                foreach (var tN in trackNumbers)
                {
                    var entregaTNExiste = eController.ObterPedidoEntregaTrack(tN);
                    if (entregaTNExiste == null)
                    {

                        int trackNumberRet = _mservice.salesOrderShipmentAddTrack(_mlogin, pedidoEntrega.PEDIDO_ENTREGA, pedidoEntrega.CARRIER, carrierTitulo, tN);
                        var entregaTN = new ECOM_PEDIDO_ENTREGA_MAG_TRACK();
                        entregaTN.TRACK_NUMBER = tN;
                        entregaTN.PEDIDO = pedido;
                        eController.InserirPedidoEntregaTrack(entregaTN);

                        enviaEmail = true;
                    }
                }

                if (enviaEmail)
                {
                    if (!mktplace)
                        sendInfo = _mservice.salesOrderShipmentSendInfo(_mlogin, pedidoEntrega.PEDIDO_ENTREGA, msgParaCliente);

                    ////Atualizar Pedido para "Pedido em Transporte" - XXX
                    //var orderStatus = _mservice.salesOrderAddComment(_mlogin, pedidoEntrega.PEDIDO_EXTERNO, "complete_shipped", "Pedido foi faturado e saiu para Entrega", "0");
                }
            }

            //Atualizar Envio do Email
            pedidoEntrega.EMAIL_ENVIADO = true;
            pedidoEntrega.DATA_ENVIO = DateTime.Now;
            eController.AtualizarPedidoEntrega(pedidoEntrega);

            return true;
        }

        // Atribuir desconto e periodo de desconto
        public bool AtribuirDesconto(string produtoLinx, decimal precoNovo, DateTime? dataInicioDesconto, DateTime? dataFimDesconto)
        {
            string fromDate = "";
            string toDate = "";

            if (dataInicioDesconto != null)
                fromDate = Convert.ToDateTime(dataInicioDesconto).ToString("yyyy-MM-dd");

            if (dataFimDesconto != null)
                toDate = Convert.ToDateTime(dataFimDesconto).ToString("yyyy-MM-dd");

            var produtoAssociacaoConfig = eController.ObterMagentoProdutoAssociacaoConfig(produtoLinx);
            foreach (var pConfig in produtoAssociacaoConfig)
            {
                var ecomProduto = eController.ObterMagentoProduto(pConfig.CODIGO);

                //atualizar configuravel
                _mservice.catalogProductSetSpecialPrice(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), precoNovo.ToString(), fromDate, toDate, "", "");

                ecomProduto.PRECO_PROMO = precoNovo;
                ecomProduto.PRECO_PROMO_DATA_DE = dataInicioDesconto;
                ecomProduto.PRECO_PROMO_DATA_ATE = dataFimDesconto;

                eController.AtualizarMagentoProduto(ecomProduto);

                var produtoSimples = eController.ObterMagentoProdutoSimples(Convert.ToInt32(pConfig.ID_PRODUTO_MAG), pConfig.PRODUTO);
                foreach (var pS in produtoSimples)
                {
                    //atualizar simples
                    _mservice.catalogProductSetSpecialPrice(_mlogin, pS.ID_PRODUTO_MAG.ToString(), precoNovo.ToString(), fromDate, toDate, "", "");

                    pS.PRECO_PROMO = precoNovo;
                    pS.PRECO_PROMO_DATA_DE = dataInicioDesconto;
                    pS.PRECO_PROMO_DATA_ATE = dataFimDesconto;

                    eController.AtualizarMagentoProduto(pS);
                }
            }

            return true;
        }

        //Associar Categorias a Produtos
        //Grupo e Outlets, tratados de forma diferente
        public bool AlterarGrupoOutlet(string produtoLinx, int codigoGrupo, int codigoGrupoOutlet, decimal? precoNovo, DateTime? dataInicioDesconto, DateTime? dataFimDesconto, string posicao = "0")
        {
            var produtoAssociacaoConfig = eController.ObterMagentoProdutoAssociacaoConfig(produtoLinx);
            foreach (var pConfig in produtoAssociacaoConfig)
            {
                var ecomProduto = eController.ObterMagentoProduto(pConfig.CODIGO);

                //excluir a categoria dele e a pai
                _mservice.catalogCategoryRemoveProduct(_mlogin, codigoGrupo, ecomProduto.ID_PRODUTO_MAG.ToString(), "");
                if (ecomProduto.ECOM_GRUPO_PRODUTO1 != null && ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI != null)
                    _mservice.catalogCategoryRemoveProduct(_mlogin, Convert.ToInt32(ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI), ecomProduto.ID_PRODUTO_MAG.ToString(), "");

                //atualizar categoria para outlet na intranet
                var ecomProdutoAux = eController.ObterMagentoProduto(pConfig.CODIGO);
                ecomProdutoAux.ECOM_GRUPO_PRODUTO = codigoGrupoOutlet;
                eController.AtualizarMagentoProduto(ecomProdutoAux);

                // atualizar categria para outlet no magento
                _mservice.catalogCategoryAssignProduct(_mlogin, codigoGrupoOutlet, ecomProdutoAux.ID_PRODUTO_MAG.ToString(), posicao, "");
                if (ecomProdutoAux.ECOM_GRUPO_PRODUTO1 != null && ecomProdutoAux.ECOM_GRUPO_PRODUTO1.CODIGO_PAI != null)
                    _mservice.catalogCategoryAssignProduct(_mlogin, Convert.ToInt32(ecomProdutoAux.ECOM_GRUPO_PRODUTO1.CODIGO_PAI), ecomProdutoAux.ID_PRODUTO_MAG.ToString(), posicao, "");

                var produtoSimples = eController.ObterMagentoProdutoSimples(Convert.ToInt32(pConfig.ID_PRODUTO_MAG), pConfig.PRODUTO);
                foreach (var pS in produtoSimples)
                {
                    ////atualizar simples
                    //_mservice.catalogCategoryRemoveProduct(_mlogin, codigoGrupo, pS.ID_PRODUTO_MAG.ToString(), "");
                    //_mservice.catalogCategoryAssignProduct(_mlogin, codigoGrupoOutlet, pS.ID_PRODUTO_MAG.ToString(), posicao, "");

                    pS.ECOM_GRUPO_PRODUTO = codigoGrupoOutlet;

                    eController.AtualizarMagentoProduto(pS);
                }
            }

            if (precoNovo != null)
                AtribuirDesconto(produtoLinx, Convert.ToDecimal(precoNovo), dataInicioDesconto, dataFimDesconto);

            return true;
        }

        //public bool AssociarProdutoCategoria(int codigo, int codigoCategoria, string posicao = "0")
        //{
        //    var ecomProduto = eController.ObterMagentoProduto(codigo);

        //    var ret = _mservice.catalogCategoryAssignProduct(_mlogin, codigoCategoria, ecomProduto.ID_PRODUTO_MAG.ToString(), posicao, "");
        //    if (ret)
        //    {
        //        var produtoCategoria = new ECOM_PRODUTO_CATEGORIA();
        //        produtoCategoria.ECOM_PRODUTO = ecomProduto.CODIGO;
        //        produtoCategoria.ECOM_CATEGORIA = codigoCategoria;
        //        produtoCategoria.POSICAO = "1";
        //        produtoCategoria.DATA_TRANSFER = DateTime.Now;
        //        eController.InserirMagentoProdutoCategoria(produtoCategoria);
        //    }

        //    return ret;
        //}
        //public bool DesassociarProdutoCategoria(int codigo, int codigoCategoria)
        //{
        //    var ecomProduto = eController.ObterMagentoProduto(codigo);

        //    var ret = _mservice.catalogCategoryRemoveProduct(_mlogin, codigoCategoria, ecomProduto.ID_PRODUTO_MAG.ToString(), "");
        //    eController.ExcluirMagentoProdutoCategoriaPorProdutoECategoria(codigo, codigoCategoria);

        //    return ret;
        //}
        //public bool AtualizarPosicaoProdutoCategoria(int codigo, int codigoCategoria, string posicao)
        //{
        //    var ecomProduto = eController.ObterMagentoProduto(codigo);

        //    var ret = _mservice.catalogCategoryUpdateProduct(_mlogin, codigoCategoria, ecomProduto.ID_PRODUTO_MAG.ToString(), posicao, "");

        //    var produtoCategoria = eController.ObterMagentoProdutoCategoriaPorProdutoECategoria(codigo, codigoCategoria);
        //    if (produtoCategoria != null)
        //    {
        //        produtoCategoria.POSICAO = posicao;
        //        produtoCategoria.DATA_TRANSFER = DateTime.Now;
        //        eController.AtualizarMagentoProdutoCategoria(produtoCategoria);
        //    }

        //    return ret;
        //}

        public bool InserirProdutoRelacionado(string idMagento, string idMagentoRelacionado, PRODUTORELACIONADO produtoRelacionado)
        {
            string type = "";
            if (produtoRelacionado == PRODUTORELACIONADO.RELACIONADO)
                type = "related";
            else if (produtoRelacionado == PRODUTORELACIONADO.AGREGADO)
                type = "up_sell";

            var ok = _mservice.catalogProductLinkAssign(_mlogin, type, idMagento, idMagentoRelacionado, null, "");

            return ok;
        }
        public bool RemoverProdutoRelacionado(string idMagento, string idMagentoRelacionado, PRODUTORELACIONADO produtoRelacionado)
        {
            string type = "";
            if (produtoRelacionado == PRODUTORELACIONADO.RELACIONADO)
                type = "related";
            else if (produtoRelacionado == PRODUTORELACIONADO.AGREGADO)
                type = "up_sell";

            var ok = _mservice.catalogProductLinkRemove(_mlogin, type, idMagento, idMagentoRelacionado, "");

            return ok;
        }
        public enum PRODUTORELACIONADO
        {
            RELACIONADO = 1,
            AGREGADO = 2
        }

        public static string GerarEtiquetaMandae(SP_OBTER_ECOM_ETIQUETA_IMPResult etiqueta, string diretorioPadrao)
        {
            StringBuilder _texto = new StringBuilder();

            //criar codigo de barras
            Util.GerarCodigoBarrasCode128DLL(diretorioPadrao, etiqueta.TRACK_NUMBER.Trim());

            var shipService = "";
            if (etiqueta.SHIP_SERVICE_MANDAE.ToLower() == "rapido" || etiqueta.SHIP_SERVICE_MANDAE.ToLower() == "rápido")
                shipService = "Rápido";
            else if (etiqueta.SHIP_SERVICE_MANDAE.ToLower() == "economico" || etiqueta.SHIP_SERVICE_MANDAE.ToLower() == "econômico")
                shipService = "Econômico";
            else
                shipService = "Erro";

            _texto.AppendLine("");

            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Etiqueta Mandaê</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("    <style type='text/css' media='print'>");
            _texto.AppendLine("        @page {");
            _texto.AppendLine("            margin: 0 -6cm;");
            _texto.AppendLine("        }");
            _texto.AppendLine("        html {");
            _texto.AppendLine("            margin: 0 6cm");
            _texto.AppendLine("        }");
            _texto.AppendLine("    </style>");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload='window.print();' style='padding: 5px; margin: 0px; margin-left:20px; margin-top:20px;'>");
            _texto.AppendLine("    <div style='width: 100%; height:250px; text-align: center;'>");
            _texto.AppendLine("        <div style='background-color:white; border: 0px solid black; font-size: 10.2pt; font-family: Arial, sans-serif; width:300px; height:250px; text-align:center;'>");
            _texto.AppendLine("            <div style='background-color:white; font-size: 16.2pt; font-weight: 700; font-family: Arial, sans-serif; text-align:center;'>");
            _texto.AppendLine("                " + shipService);
            _texto.AppendLine("            </div>");
            _texto.AppendLine("            <br />");
            //_texto.AppendLine("            <div style='font-size: 8.2pt; font-family: Arial, sans-serif;'>");
            //_texto.AppendLine("                " + trackingId);
            //_texto.AppendLine("            </div>");
            _texto.AppendLine("            <div style='font-weight:900'>");
            _texto.AppendLine("                <img alt='Barras' width='345' height='89' src='..\\.." + ("~/Image_BARCODE/" + etiqueta.TRACK_NUMBER.Trim() + ".png").Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("            </div>");
            _texto.AppendLine("            <div style='background-color:white; line-height:11px; font-size: 10.2pt; font-family: Arial, sans-serif; text-align:left; word-spacing:normal; margin: 0; padding: 0;'>");
            _texto.AppendLine("                <p>");
            _texto.AppendLine("                    <div style='float:left; margin-left: 10px'>");
            _texto.AppendLine("                        <span><strong>Pedido: </strong>" + ((etiqueta.PEDIDO_EXTERNO == null) ? ("TR" + etiqueta.PEDIDO.Trim()) : etiqueta.PEDIDO_EXTERNO.Trim()) + "<br /></span>");
            _texto.AppendLine("                    </div>");
            _texto.AppendLine("                    <div style='float:right; margin-right: 10px;'>");
            _texto.AppendLine("                        <span><strong>NF: </strong>" + etiqueta.NF_SAIDA.Trim() + "<br /></span>");
            _texto.AppendLine("                    </div>");
            _texto.AppendLine("                </p>");
            _texto.AppendLine("            </div>");
            _texto.AppendLine("            <br />");
            _texto.AppendLine("            <div style='background-color: white; line-height: 12px; font-size: 8.2pt; font-family: Arial, sans-serif; text-align: left; word-spacing: normal; margin-left: 10px; padding: 0; '>");
            _texto.AppendLine("                <p>");
            _texto.AppendLine("                    <span><strong>DESTINATÁRIO</strong><br /></span>");
            _texto.AppendLine("                    <span>" + etiqueta.NOME.Trim() + "<br /></span>");
            _texto.AppendLine("                    <span>" + etiqueta.ENDERECO.Trim() + ", " + etiqueta.NUMERO.Trim() + ((etiqueta.COMPLEMENTO == "") ? "" : (", " + etiqueta.COMPLEMENTO.Trim())) + "<br /></span>");
            _texto.AppendLine("                    <span>" + etiqueta.BAIRRO.Trim() + "<br /></span>");
            _texto.AppendLine("                    <span>" + etiqueta.CEP.Trim() + " " + etiqueta.CIDADE.Trim() + " - " + etiqueta.UF.Trim() + "<br /></span>");
            _texto.AppendLine("                </p>");
            _texto.AppendLine("            </div>");
            _texto.AppendLine("            <div style='background-color: white; line-height: 12px; font-size: 8.2pt; font-family: Arial, sans-serif; text-align: left; word-spacing: normal; margin-left: 10px; padding: 0;'>");
            _texto.AppendLine("                <p>");
            _texto.AppendLine("                    <span><strong>REMETENTE</strong><br /></span>");
            _texto.AppendLine("                    <span>Handbook Online<br /></span>");
            _texto.AppendLine("                    <span>Rua Bento de Matos, 122<br /></span>");
            _texto.AppendLine("                    <span>Chácara Santo Antônio<br /></span>");
            _texto.AppendLine("                    <span>04713-030 São Paulo - SP<br /></span>");
            _texto.AppendLine("                </p>");
            _texto.AppendLine("            </div>");
            _texto.AppendLine("        </div>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");

            _texto.AppendLine("");
            return _texto.ToString();
        }

        public bool AtualizarPedidoStatus(string pedidoExterno, string status, string msg, string enviarEmail)
        {
            if (enviarEmail == "")
                enviarEmail = "0";

            var orderStatus = _mservice.salesOrderAddComment(_mlogin, pedidoExterno, status, msg, enviarEmail);
            return orderStatus;
        }

        public bool AtualizarPrecoPromo(string idProdutoMag, decimal preco)
        {
            var productPriceUpdated = _mservice.catalogProductSetSpecialPrice(_mlogin, idProdutoMag, preco.ToString(), "", "", "", "");

            if (productPriceUpdated == 1)
                return true;

            return false;
        }
        public bool AtualizarCategoria(ECOM_PRODUTO ecomProduto)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

            List<string> cats = new List<string>();

            //Categoria Grupo
            cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO.ToString());

            // Colecao FEMININO/MASCULINO
            if (ecomProduto.ECOM_GRUPO_PRODUTO1 != null && ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI != null)
                cats.Add(ecomProduto.ECOM_GRUPO_PRODUTO1.CODIGO_PAI.ToString());

            //Categorias Gerais
            var catsProduto = eController.ObterProdutoCategoria(ecomProduto.CODIGO);
            foreach (var c in catsProduto)
                cats.Add(c.ECOM_GRUPO_PRODUTO.ToString());

            product.category_ids = cats.ToArray();

            bool updateOK = _mservice.catalogProductUpdate(_mlogin, ecomProduto.ID_PRODUTO_MAG.ToString(), product, "", "");

            return updateOK;
        }

        public int GerarCupom(string codigoCupom, decimal valorCupom, bool freteGratis, string[] cats = null)
        {
            MagentoService.Promotion promo = new MagentoService.Promotion();
            promo.name = "INTRA - " + codigoCupom;
            promo.active = "1";
            promo.usesPerCustomer = "1";
            promo.usesPerCustomer = "1";
            promo.code = codigoCupom;

            MagentoService.PromotionAction action = new MagentoService.PromotionAction();
            action.discount = valorCupom.ToString();
            action.applyToShipping = "0";
            action.discountType = "cart_fixed";
            action.discountValueType = "fixed";
            if (freteGratis)
                action.freeShipping = "ONLY_PRODUCTS";
            else
                action.freeShipping = "NO";

            if (cats != null && cats.Count() > 0)
            {
                MagentoService.PromotionCategory[] categories = new MagentoService.PromotionCategory[cats.Count()];
                var i = 0;
                foreach (var c in cats)
                {
                    categories[i] = new MagentoService.PromotionCategory();
                    categories[i].id = c;

                    i += 1;
                }
                action.applyInCategories = categories;
            }

            promo.action = action;

            _mservice.promotionCreate(_mlogin, ref promo);

            if (promo.id != null && promo.id != "" && promo.id != "0")
                return Convert.ToInt32(promo.id);

            return 0;
        }
        public bool ExcluirCupom(string idCupom)
        {
            var ret = _mservice.promotionDelete(_mlogin, idCupom);
            return (ret == "1") ? true : false;
        }




    }
}