using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using Relatorios.mod_ecom.mag;
using System.Drawing.Drawing2D;


namespace Relatorios
{
    public partial class ecom_teste_integracao : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            //btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        #region "CLIENTES"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    //SP_OBTER_CLIENTE_ATC_BLOQResult cliBloq = e.Row.DataItem as SP_OBTER_CLIENTE_ATC_BLOQResult;
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProduto.FooterRow;
            if (_footer != null)
            {
            }
        }
        #endregion


        protected void btEnviarProduto_Click(object sender, EventArgs e)
        {

            try
            {


                MagentoService.MagentoService mservice = new MagentoService.MagentoService();

                var mlogin = mservice.login("linx_api", "sGf45Erk5FcusoR903F5dgghS");

                //string mlogin = "5589217bd241cf83b57e02ad5b806c9b";

                // get attribute set
                var sets = mservice.catalogProductAttributeSetList(mlogin);
                var setId = sets[0].set_id.ToString();

                string sku = txtSKU.Text.Trim();

                string sku1 = CriarProdutoSimples(mservice, mlogin, setId, sku + "-01", "Azul", "P");
                string sku2 = CriarProdutoSimples(mservice, mlogin, setId, sku + "-02", "Vermelho", "M");

                MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();

                product.name = "Configuravel Leo";
                product.description = "Description Leo";
                product.status = "1";
                product.visibility = "4";
                product.tax_class_id = "2";
                product.weight = "0.01";
                product.price = "9.69";

                string[] associatedSkus = { sku1, sku2 };
                product.associated_skus = associatedSkus;

                //ESTOQUE
                MagentoService.catalogInventoryStockItemUpdateEntity estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
                estoque.manage_stock = 1;
                estoque.is_in_stock = 1;
                estoque.manage_stockSpecified = true;
                estoque.is_in_stockSpecified = true;
                product.stock_data = estoque;

                int idProduto = 0;
                idProduto = mservice.catalogProductCreate(mlogin, "configurable", setId, sku, product, "");


                //tipo_produto - Top;Shorts Saia;Malhão;Regata;Body;Tomara que Caia
                string retornoImage = "";

                //ARQUIVO DA IMAGEM
                MagentoService.catalogProductImageFileEntity f = new MagentoService.catalogProductImageFileEntity();
                Byte[] bytes = File.ReadAllBytes(txtPathImage.Text);
                String fileBase64 = Convert.ToBase64String(bytes);

                f.content = fileBase64;
                f.mime = "image/jpeg";
                f.name = "nome da imagem";

                //IMAGEM
                MagentoService.catalogProductAttributeMediaCreateEntity imageP = new MagentoService.catalogProductAttributeMediaCreateEntity();
                imageP.label = "label da imagem";
                imageP.position = "1";
                imageP.file = f;

                string[] typ = { "image" };
                imageP.types = typ;

                retornoImage = mservice.catalogProductAttributeMediaCreate(mlogin, idProduto.ToString(), imageP, "", "");

                //mservice.catalogProductUpdate(mlogin, idProduto.ToString(), product, "", "");

            }
            catch (Exception ex)
            {

                //string t = "";
            }


        }

        private string CriarProdutoSimples(MagentoService.MagentoService mservice, string mlogin, string setId, string sku, string c, string s)
        {
            MagentoService.catalogProductCreateEntity product = new MagentoService.catalogProductCreateEntity();


            product.name = "Produto do Leo In Stoeck 1";
            product.description = "Teste do Leo";

            string[] cat = { "15", "16" };
            product.category_ids = cat;

            string[] web = { "http://leo", "http://leo2" };
            product.websites = web;

            product.short_description = "Teste do Leo Short Description";
            product.weight = "5.69";
            product.status = "1"; // 1 - Habilitado, 2 - Desabilitado
            product.visibility = "1"; // 1 = Not visible, 2 = Catalog, 3 = Search, 4 = Catalog/Search
            product.tax_class_id = "2";
            product.meta_title = "meta title";
            product.meta_keyword = "meta keyword";
            product.meta_description = "meta description";
            product.url_path = "produto-do-leo-camiseta-vermelha";
            product.url_key = "produto-do-leo-camiseta-vermelha";

            product.price = "9.99";
            product.special_price = "5.99";
            product.special_from_date = "2017-08-05";
            product.special_to_date = "2017-08-010";

            //ESTOQUE
            MagentoService.catalogInventoryStockItemUpdateEntity estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
            estoque.qty = "10";
            estoque.is_in_stock = 1;
            estoque.manage_stock = 1;
            estoque.manage_stockSpecified = true;
            estoque.is_in_stockSpecified = true;
            product.stock_data = estoque;

            MagentoService.catalogProductAdditionalAttributesEntity att = new MagentoService.catalogProductAdditionalAttributesEntity();

            MagentoService.associativeEntity[] asso = new MagentoService.associativeEntity[9];
            asso[0] = new MagentoService.associativeEntity();
            asso[0].key = "color";
            asso[0].value = c;

            asso[1] = new MagentoService.associativeEntity();
            asso[1].key = "tamanho";
            asso[1].value = s;

            asso[2] = new MagentoService.associativeEntity();
            asso[2].key = "tipo_estilos";
            asso[2].value = "Casual";

            asso[3] = new MagentoService.associativeEntity();
            asso[3].key = "tipo_modelagem";
            asso[3].value = "Moletom Fechado";

            asso[4] = new MagentoService.associativeEntity();
            asso[4].key = "tipo_produto";
            asso[4].value = "Tomara que Caia";

            asso[5] = new MagentoService.associativeEntity();
            asso[5].key = "tipo_tecido";
            asso[5].value = "Sued";

            asso[6] = new MagentoService.associativeEntity();
            asso[6].key = "tipo_manga";
            asso[6].value = "Manga 3/4";

            asso[7] = new MagentoService.associativeEntity();
            asso[7].key = "tipo_gola";
            asso[7].value = "Com Capuz";

            asso[8] = new MagentoService.associativeEntity();
            asso[8].key = "tipo_comprimento";
            asso[8].value = "Midi";

            //tipo_produto - Top;Shorts Saia;Malhão;Regata;Body;Tomara que Caia

            att.single_data = asso;

            product.additional_attributes = att;

            int idProduto = 0;

            idProduto = mservice.catalogProductCreate(mlogin, "simple", setId, sku, product, "");

            string retornoImage = "";

            //ARQUIVO DA IMAGEM
            MagentoService.catalogProductImageFileEntity f = new MagentoService.catalogProductImageFileEntity();
            Byte[] bytes = File.ReadAllBytes(txtPathImage.Text);
            String fileBase64 = Convert.ToBase64String(bytes);

            f.content = fileBase64;
            f.mime = "image/jpeg";
            f.name = "nome da imagem";

            //IMAGEM
            MagentoService.catalogProductAttributeMediaCreateEntity imageP = new MagentoService.catalogProductAttributeMediaCreateEntity();
            imageP.label = "label da imagem";
            imageP.position = "1";
            imageP.file = f;

            string[] typ = { "image" };
            imageP.types = typ;

            retornoImage = mservice.catalogProductAttributeMediaCreate(mlogin, idProduto.ToString(), imageP, "", "");



            estoque = new MagentoService.catalogInventoryStockItemUpdateEntity();
            estoque.qty = "15";
            estoque.is_in_stock = 1;
            estoque.manage_stock = 1;
            estoque.manage_stockSpecified = true;
            estoque.is_in_stockSpecified = true;
            mservice.catalogInventoryStockItemUpdate(mlogin, idProduto.ToString(), estoque);


            return sku;
        }

        protected void btImgProduct_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            string fileName = string.Empty;
            string ext = string.Empty;

            string error = "";

            try
            {

                if (!uploadImgProduct.HasFile)
                {
                    error = "Selecione um arquivo para upload...";
                    return;
                }

                if (uploadImgProduct.FileContent.Length == 0)
                {
                    error = "TAMANHO DO ARQUVO";
                    return;
                }

                //As the input is external, always do case-insensitive comparison unless you actually care about the case.
                if (!uploadImgProduct.PostedFile.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    error = "SELECIONE UMA IMAGEM";
                    return;
                }

                //Obter variaveis do arquivo
                ext = System.IO.Path.GetExtension(uploadImgProduct.PostedFile.FileName);


                fileName = Guid.NewGuid() + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm") + ext;
                path = Server.MapPath("~/IMAGE_ECOMMERCE/") + fileName;

                uploadImgProduct.PostedFile.SaveAs(path);

                txtPathImage.Text = path;

                imgProduto.ImageUrl = path;


            }
            catch (Exception)
            {
            }
        }

        protected void btCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoProduto = txtSKU.Text;

                Magento mag = new Magento();

                //mag.AssociarProdutoCategoria(Convert.ToInt32(codigoProduto), Convert.ToInt32(txtCodigoCategoria.Text));
                //mag.AtualizarPosicaoProdutoCategoria(Convert.ToInt32(codigoProduto), Convert.ToInt32(txtCodigoCategoria.Text), "10");
                //mag.DesassociarProdutoCategoria(Convert.ToInt32(codigoProduto), Convert.ToInt32(txtCodigoCategoria.Text));

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void btTeste_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("~\\FotosVerao2018\\Tulp");

            var files = Directory.GetFiles(path, "*.jpg").OrderBy(p => p);

            StringBuilder b = new StringBuilder();

            int i = 1;
            foreach (var f in files)
            {
                string name = f.Substring(f.LastIndexOf('\\') + 1, 10).Replace(".", "").Trim();

                var produto = name.Substring(0, 5);
                var cor = name.Substring(5, (name.Length - 6));



                var produtoLinx = baseController.BuscaProduto(produto);

                if (produtoLinx != null)
                {

                    var corLinx = prodController.ObterCoresBasicas(cor);
                    var precoLinx = baseController.BuscaPrecoTAProduto(produto);

                    var look = new LOK_LOOKBOOK();
                    look.PRODUTO = produto;
                    look.DESC_PRODUTO = Utils.WebControls.AlterarPrimeiraLetraMaiscula((produtoLinx.DESC_PRODUTO.Trim()));
                    look.COR = cor;
                    look.DESC_COR = Utils.WebControls.AlterarPrimeiraLetraMaiscula(corLinx.DESC_COR.Trim());
                    look.GRUPO_PRODUTO = Utils.WebControls.AlterarPrimeiraLetraMaiscula(produtoLinx.GRUPO_PRODUTO.Trim());
                    look.GRIFFE = Utils.WebControls.AlterarPrimeiraLetraMaiscula(produtoLinx.GRIFFE.Trim());
                    look.PRECO = (precoLinx == null) ? 0 : Convert.ToDecimal(precoLinx.PRECO1);
                    look.FOTO_NOME = name;
                    look.LEGENDA = look.GRUPO_PRODUTO + " " + look.DESC_COR + " " + look.PRODUTO + " R$" + Convert.ToDecimal(look.PRECO).ToString("###,###,##0.00");

                    baseController.IncluirLokLookbook(look);


                    var griffeC = "\"" + look.GRIFFE + "\", ";
                    var grupoProdutoC = "\"" + look.GRUPO_PRODUTO.Replace("CALCA", "CALÇA").Replace("P.COAT", "P. COAT").Replace("TENIS", "TÊNIS") + "\", ";
                    var produtoC = "\"" + look.PRODUTO + "\", ";
                    var descCorC = "\"" + look.DESC_COR + "\", ";
                    var precoC = "\"" + Convert.ToDecimal(look.PRECO).ToString("###,###,##0.00") + "\", ";
                    var fotoC = "\"" + "@drawable/lb" + (look.PRODUTO + look.COR) + "\", ";
                    var legendaC = "\"" + look.LEGENDA.Replace("CALCA", "CALÇA").Replace("P.COAT", "P. COAT").Replace("TENIS", "TÊNIS") + "\"";

                    b.AppendLine("product = new Product(" +
                                            i.ToString() + ", " +
                                            griffeC +
                                            grupoProdutoC +
                                            produtoC +
                                            descCorC +
                                            precoC +
                                            fotoC +
                                            legendaC + ");");

                    b.AppendLine("productList.add(product);");
                    b.AppendLine("");


                    i = i + 1;

                }
                else
                {
                    var look = new LOK_LOOKBOOK();
                    look.PRODUTO = produto;
                    look.DESC_PRODUTO = "";
                    look.COR = cor;
                    look.DESC_COR = "";
                    look.GRUPO_PRODUTO = "";
                    look.GRIFFE = "";
                    look.PRECO = 0;
                    look.FOTO_NOME = name;
                    look.LEGENDA = look.GRUPO_PRODUTO + " " + look.DESC_COR + " " + look.PRODUTO + " R$" + Convert.ToDecimal(look.PRECO).ToString("###,###,##0.00");

                    baseController.IncluirLokLookbook(look);
                }
            }

            string t = b.ToString();

            string a = "";

            a = "leandro";




        }

        protected void btCriarFoto_Click(object sender, EventArgs e)
        {
            string path = @"\\192.168.1.8\d$\APP_FINAL_BKP\INVERNO2018\MASCULINO\";
            string pathDest = @"\\192.168.1.8\d$\APP_FINAL_BKP\INVERNO2018\";


            var invfoto = baseController.ObterInvFoto();


            string produto = "";
            string cor = "";
            string foto = "";

            string pathFoto = "";
            string pathFotoDest = "";
            int i = 1;
            foreach (var l in invfoto)
            {
                produto = l.PRODUTO.Trim();
                cor = l.COR.Trim();
                foto = l.FOTO.Replace(".jpg", ".png");

                pathFoto = path + foto;
                pathFotoDest = (pathDest + produto + cor + ".png");

                if (File.Exists(pathFoto))
                {
                    //GenerateThumbnails(pathFoto, (pathDest + produtocor + ".jpg"));

                    if (File.Exists(pathFotoDest))
                        pathFotoDest = pathFotoDest.Replace(".png", "_" + (i.ToString() + ".png"));

                    File.Copy(pathFoto, pathFotoDest, true);

                    var inv = baseController.ObterInvFoto(produto, cor, foto.Replace(".png", ".jpg"));
                    if (inv != null)
                    {
                        inv.OK = true;
                        baseController.AtualizarInvFoto(inv);
                    }

                }

                i = i + 1;

            }

        }

        protected void btRelacionar_Click(object sender, EventArgs e)
        {
            Magento mag = new Magento();

            mag.InserirProdutoRelacionado("6173", "6157", Magento.PRODUTORELACIONADO.RELACIONADO);
            mag.InserirProdutoRelacionado("6173", "5703", Magento.PRODUTORELACIONADO.RELACIONADO);
            mag.InserirProdutoRelacionado("6173", "5789", Magento.PRODUTORELACIONADO.RELACIONADO);
            mag.InserirProdutoRelacionado("6173", "5721", Magento.PRODUTORELACIONADO.RELACIONADO);

            mag.InserirProdutoRelacionado("6173", "3809", Magento.PRODUTORELACIONADO.AGREGADO);
            mag.InserirProdutoRelacionado("6173", "5121", Magento.PRODUTORELACIONADO.AGREGADO);

            mag.RemoverProdutoRelacionado("6173", "5121", Magento.PRODUTORELACIONADO.AGREGADO);
            mag.RemoverProdutoRelacionado("6173", "5721", Magento.PRODUTORELACIONADO.AGREGADO);

            //MagentoService.MagentoService _mservice;

            //// autenticacao magento
            //_mservice = new MagentoService.MagentoService();
            //var _mlogin = _mservice.login(Constante.userMagento, Constante.apiKeyMagento);

            //var b = _mservice.catalogProductLinkAssign(_mlogin, "related", "6173", "6157", null, ""); //BLUSA

            //var c = _mservice.catalogProductLinkAssign(_mlogin, "related", "6173", "5703", null, ""); //VESTIDO

            //var d = _mservice.catalogProductLinkAssign(_mlogin, "related", "6173", "5789", null, ""); // SHORTS

            //var XASD = _mservice.catalogProductLinkAssign(_mlogin, "related", "6173", "5721", null, ""); // CALÇA

            //var VV = _mservice.catalogProductLinkAssign(_mlogin, "up_sell", "6173", "3809", null, ""); // TOP

            //var AD = _mservice.catalogProductLinkAssign(_mlogin, "up_sell", "6173", "5121", null, ""); // MACACAO

            // related - Produtos Relacionados
            // up_sell - Vendas Agregadas
            // cross_sell - Vendas Cruzadas
            // grouped - Vendas Agrupados

        }


        protected void btObterProdutoCategoria_Click(object sender, EventArgs e)
        {

            MagentoService.MagentoService _mservice;

            // autenticacao magento
            _mservice = new MagentoService.MagentoService();
            var _mlogin = _mservice.login(Constante.userMagento, Constante.apiKeyMagento);

            var b = _mservice.catalogCategoryAssignedProducts(_mlogin, 16);

            //string teste = "";

            //teste = "eu";


        }

        protected void btNatal_Click(object sender, EventArgs e)
        {

            string origem = @"\\192.168.1.8\d$\Intranet\FotosHandbookOnline\";

            string dest = @"\\192.168.1.8\d$\Natal\";

            var n = new EcomController().ObterFotosNatal();

            foreach (var nn in n)
            {
                var nomeFoto = nn.FOTO + ".png";

                if (File.Exists(origem + nomeFoto))
                {
                    File.Copy(origem + nomeFoto, dest + nomeFoto, true);
                }
            }

        }



        protected void btDiminuirFoto_Click(object sender, EventArgs e)
        {
            try
            {
                var path = @"\\192.168.1.8\d$\Intranet\FotosHandbookTratamento\";

                var dirs = Directory.GetDirectories(path);

                var i = 1;

                foreach (var d in dirs)
                {

                    if (!d.ToLower().Contains("items"))
                    {
                        if (!d.ToLower().Contains("_"))
                        {



                            var lastIndex = d.LastIndexOf('\\') + 1;
                            var dirName = d.Substring(lastIndex);
                            var fileName = dirName.Substring(0, dirName.Length);

                            var vals = fileName.Split('-');
                            var produto = vals[1].Substring(0, 5);

                            var produtoLinx = baseController.BuscaProduto(produto);
                            if (produtoLinx != null)
                            {
                                var n = d.Replace(fileName, produtoLinx.GRIFFE.Trim() + "_" + fileName);
                                Directory.Move(d, n);
                            }
                        }
                    }

                    i = i + 1;
                }

                var t = 0;

                t = i;


            }
            catch (Exception ex)
            {
            }

        }
        private void SaveImage(Stream stream, string fileName)
        {

            string _fileName = string.Empty;
            string _ext = string.Empty;

            try
            {
                var pathTo = @"\\192.168.1.8\d$\Intranet\Fotos\FotosSmall\";

                var filePath = pathTo + fileName;

                //Obter stream da imagem
                GenerateThumbnails(1, stream, filePath);

            }
            catch (Exception ex)
            {
            }
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 300
                // height 320
                var newWidth = 0;
                var newHeight = 0;

                int newWidth2Image = 300;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > newWidth2Image || newHeight > 320)
                {
                    newWidth = (int)((newWidth) * 0.95);
                    newHeight = (int)((newHeight) * 0.95);
                }

                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

    }
}

