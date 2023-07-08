using DAL;
using Relatorios.mod_ecom.mag;
using Relatorios.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_edit : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        string gCodCategoria = "";
        const string extensaoFoto = "jpg";

        const string PRODCATMAG = "PRODCATMAG";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrecoPromoDe.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrecoPromoAte.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["co"] == null || Request.QueryString["co"] == "" ||
                    Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                string colecao = "";
                string produto = "";
                string cor = "";

                colecao = Request.QueryString["co"].ToString();
                produto = Request.QueryString["p"].ToString();
                cor = Request.QueryString["c"].ToString();

                hidColecao.Value = colecao;
                hidProduto.Value = produto;
                hidCor.Value = cor;

                // limpar sesssao
                Session[PRODCATMAG] = null;

                var p = baseController.BuscaProduto(hidProduto.Value);

                if (p != null)
                {

                    gCodCategoria = p.COD_CATEGORIA.Trim();
                    hidCodCategoria.Value = gCodCategoria;

                    string griffe = p.GRIFFE.Trim();
                    hidGriffe.Value = griffe;

                    string grupoProduto = p.GRUPO_PRODUTO.Trim();
                    string codCategoria = p.COD_CATEGORIA.Trim();

                    CarregarCaixaDimensao();
                    CarregarCorMagento();
                    CarregarGrupoMacro();
                    CarregarTipoModelagem(griffe, grupoProduto);
                    CarregarTipoTecido(griffe, grupoProduto);
                    CarregarTipoManga(griffe, grupoProduto);
                    CarregarTipoGola(griffe, grupoProduto);
                    CarregarTipoComprimento(griffe, grupoProduto);
                    CarregarTipoEstilo(griffe, grupoProduto);
                    CarregarTipoLinha(colecao);
                    CarregarSigned(colecao);
                    CarregarGrupoMagento(codCategoria, griffe, grupoProduto);
                    BloquearRadioVitrine();
                    CarregarOrdemFoto();
                    CarregarTipoRelacionado();
                    CarregarCategoriaMag(griffe.Trim(), grupoProduto.Trim());

                    // Carregar Produto
                    txtProduto.Text = p.PRODUTO1.Trim();
                    txtProduto.Enabled = false;

                    txtNomeProduto.Text = p.DESC_PRODUTO.Trim();
                    txtNomeProduto.Enabled = false;

                    var c = prodController.ObterCoresBasicas(cor).DESC_COR.Trim();
                    txtCorLinx.Text = c;
                    txtCorLinx.Enabled = false;

                    txtSKU.Text = produto + "-" + cor;
                    txtSKU.Enabled = false;

                    txtGrupoProduto.Text = grupoProduto;
                    txtGrupoProduto.Enabled = false;

                    txtStatusCadastro.Text = "-";
                    txtStatusCadastro.Enabled = false;

                    //fotos
                    string nomeFoto = produto + cor;

                    string pathFotoLook = "~/FotosHandbookOnline/" + nomeFoto + "L." + extensaoFoto;
                    string pathFotoFrenteCab = "~/FotosHandbookOnline/" + nomeFoto + "F." + extensaoFoto;
                    string pathFotoFrenteSemCab = "~/FotosHandbookOnline/" + nomeFoto + "G." + extensaoFoto;
                    string pathFotoCostas = "~/FotosHandbookOnline/" + nomeFoto + "C." + extensaoFoto;
                    string pathFotoDetalhe = "~/FotosHandbookOnline/" + nomeFoto + "D." + extensaoFoto;
                    string pathFotoLado = "~/FotosHandbookOnline/" + nomeFoto + "A." + extensaoFoto;

                    if (File.Exists(Server.MapPath(pathFotoLook)))
                        imgFotoLook.ImageUrl = pathFotoLook;
                    if (File.Exists(Server.MapPath(pathFotoFrenteCab)))
                        imgFotoFrenteCabeca.ImageUrl = pathFotoFrenteCab;
                    if (File.Exists(Server.MapPath(pathFotoFrenteSemCab)))
                        imgFotoFrenteSemCabeca.ImageUrl = pathFotoFrenteSemCab;
                    if (File.Exists(Server.MapPath(pathFotoCostas)))
                        imgFotoCostas.ImageUrl = pathFotoCostas;
                    if (File.Exists(Server.MapPath(pathFotoDetalhe)))
                        imgFotoDetalhe.ImageUrl = pathFotoDetalhe;
                    if (File.Exists(Server.MapPath(pathFotoLado)))
                        imgFotoLado.ImageUrl = pathFotoLado;

                    //sugestao de cor e grupo macro, caso ja esteja cadastrado, o codigo abaixo altera para o valor cadastrado
                    var corDePara = eController.ObterMagentoCorLinx(cor);
                    if (corDePara != null)
                        ddlCorMagento.SelectedValue = eController.ObterMagentoCorPorDescricao(corDePara.COR_MAGENTO).CODIGO.ToString();

                    var grupoMacroDePara = eController.ObterMagentoGrupoMacroLinx(grupoProduto.Trim());
                    if (grupoMacroDePara != null)
                        ddlGrupoMacro.SelectedValue = eController.ObterMagentoGrupoMacroPorDescricao(grupoMacroDePara.GRUPO_MACRO).CODIGO.ToString();

                    var desenvProduto = desenvController.ObterProdutoCor(produto, cor).FirstOrDefault();
                    if (desenvProduto != null)
                    {
                        imgProduto.ImageUrl = desenvProduto.FOTO;
                        if (desenvProduto.SIGNED_NOME != null && desenvProduto.SIGNED_NOME != "")
                        {
                            var signedMagento = eController.ObterMagentoSigned(colecao).Where(sm => sm.SIGNED.ToLower().Contains(desenvProduto.SIGNED_NOME.ToLower())).FirstOrDefault();
                            if (signedMagento != null)
                                ddlSigned.SelectedValue = signedMagento.CODIGO.ToString();
                        }
                    }

                    //Sugestao de Descricao
                    var pDescricao = eController.ObterProdutoDescricao(produto);
                    if (pDescricao != null)
                    {
                        string descricaoProduto = pDescricao.PRODUTO_DESC + Environment.NewLine +
                                                    pDescricao.PECA_DESC + Environment.NewLine +
                                                    pDescricao.USO_DESC + Environment.NewLine +
                                                    "Composição: " + ObterComposicaoProduto(produto);
                        txtDescProduto.Text = descricaoProduto;
                    }
                    else
                    {
                        string descricaoProduto = Environment.NewLine +
                                                    Environment.NewLine +
                                                    "Composição: " + ObterComposicaoProduto(produto);
                        txtDescProduto.Text = descricaoProduto;
                    }

                    //Marcar Vitrine/Hover
                    rbVitrineFrenteCabeca.Checked = true;
                    rbHoverDetalhe.Checked = true;

                    // Obter produto configuravel cadastrado
                    var produtoMag = eController.ObterMagentoProdutoConfig(produto, cor);
                    if (produtoMag != null)
                    {
                        hidCodigo.Value = produtoMag.CODIGO.ToString();

                        if (produtoMag.ID_PRODUTO_MAG != null)
                        {
                            btSalvar.Enabled = false;
                            btSalvarContinuar.Enabled = false;
                        }
                        else
                        {
                            btAtualizarMagento.Enabled = false;
                        }

                        cbCadastrarMKTPlace.Checked = (produtoMag.CAD_MKTPLACE == null) ? false : Convert.ToBoolean(produtoMag.CAD_MKTPLACE);

                        //produto
                        ddlCorMagento.SelectedValue = produtoMag.ECOM_COR.ToString();
                        ddlGrupoMagento.SelectedValue = produtoMag.ECOM_GRUPO_PRODUTO.ToString();
                        ddlGrupoMagento_SelectedIndexChanged(ddlGrupoMagento, null);
                        var produtoBloco = eController.ObterBlocoProdutoOrdemPorCodigoeEcomGrupoProduto(Convert.ToInt32(produtoMag.ECOM_GRUPO_PRODUTO), produtoMag.CODIGO);
                        if (produtoBloco != null)
                            ddlBloco.SelectedValue = produtoBloco.ECOM_BLOCO_PRODUTO.ToString();

                        txtNomeProdutoMagento.Text = produtoMag.NOME_MAG;
                        txtDescProduto.Text = produtoMag.PRODUTO_DESC;
                        txtDescProdutoCurta.Text = produtoMag.PRODUTO_DESC_CURTA;
                        txtPeso.Text = produtoMag.PESO_KG.ToString();
                        ddlGrupoMacro.SelectedValue = produtoMag.ECOM_GRUPO_MACRO.ToString();
                        txtPreco.Text = produtoMag.PRECO.ToString();
                        if (produtoMag.PRECO_PROMO != null)
                            txtPrecoPromocional.Text = produtoMag.PRECO_PROMO.ToString();
                        if (produtoMag.PRECO_PROMO_DATA_DE != null)
                            txtDataPrecoPromoDe.Text = Convert.ToDateTime(produtoMag.PRECO_PROMO_DATA_DE).ToString("dd/MM/yyyy");
                        if (produtoMag.PRECO_PROMO_DATA_ATE != null)
                            txtDataPrecoPromoAte.Text = Convert.ToDateTime(produtoMag.PRECO_PROMO_DATA_ATE).ToString("dd/MM/yyyy");

                        //--retirado para BF 18 / 11 / 2019----
                        //verifica desconto
                        var precoTL = baseController.BuscaPrecoTLProduto(p.PRODUTO1.Trim()).PRECO1;
                        if (precoTL != null && precoTL != produtoMag.PRECO)
                        {
                            txtPrecoPromocional.Text = precoTL.ToString();

                            if (precoTL != produtoMag.PRECO_PROMO)
                                txtPrecoPromocional.BackColor = Color.Gold;
                        }

                        txtObservacao.Text = produtoMag.OBSERVACAO;
                        if (produtoMag.OBSERVACAO != null && produtoMag.OBSERVACAO.Trim() != "")
                            txtObservacao.BackColor = Color.Coral;

                        txtURLProduto.Text = produtoMag.URL_PRODUTO;
                        ddlVisibilidade.SelectedValue = produtoMag.VISIBILIDADE;
                        ddlCaixa.SelectedValue = produtoMag.ECOM_CAIXA_DIMENSAO.ToString();

                        if (produtoMag.STATUS_CADASTRO == 'B')
                            txtStatusCadastro.Text = "Cadastrado";
                        else if (produtoMag.STATUS_CADASTRO == 'A')
                            txtStatusCadastro.Text = "Enviar ao Magento";
                        else
                            txtStatusCadastro.Text = "-";

                        //Caracteristicas
                        if (produtoMag.ECOM_TIPO_MODELAGEM != null)
                            ddlTipoModelagem.SelectedValue = produtoMag.ECOM_TIPO_MODELAGEM.ToString();
                        if (produtoMag.ECOM_TIPO_TECIDO != null)
                            ddlTipoTecido.SelectedValue = produtoMag.ECOM_TIPO_TECIDO.ToString();
                        if (produtoMag.ECOM_TIPO_MANGA != null)
                            ddlTipoManga.SelectedValue = produtoMag.ECOM_TIPO_MANGA.ToString();
                        if (produtoMag.ECOM_TIPO_GOLA != null)
                            ddlTipoGola.SelectedValue = produtoMag.ECOM_TIPO_GOLA.ToString();
                        if (produtoMag.ECOM_TIPO_COMPRIMENTO != null)
                            ddlTipoComprimento.SelectedValue = produtoMag.ECOM_TIPO_COMPRIMENTO.ToString();
                        if (produtoMag.ECOM_TIPO_ESTILO != null)
                            ddlTipoEstilo.SelectedValue = produtoMag.ECOM_TIPO_ESTILO.ToString();
                        if (produtoMag.ECOM_TIPO_LINHA != null)
                            ddlTipoLinha.SelectedValue = produtoMag.ECOM_TIPO_LINHA.ToString();
                        if (produtoMag.ECOM_SIGNED != null)
                            ddlSigned.SelectedValue = produtoMag.ECOM_SIGNED.ToString();

                        ddlTipoRelacionado.SelectedValue = produtoMag.ECOM_TIPO_RELACIONADO.ToString();

                        //SEO
                        txtMetaTitulo.Text = produtoMag.META_TITULO;
                        txtMetaKeyword.Text = produtoMag.META_KEYWORD;
                        txtMetaDescricao.Text = produtoMag.META_DESCRICAO;

                        ddlHabilitado.SelectedValue = produtoMag.HABILITADO.ToString();

                        //Tratar Foto da Vitrine
                        rbVitrineLook.Checked = false;
                        rbVitrineFrenteCabeca.Checked = false;
                        rbVitrineFrenteSemCabeca.Checked = false;
                        rbVitrineCostas.Checked = false;
                        rbVitrineDetalhe.Checked = false;
                        rbVitrineLado.Checked = false;
                        if (produtoMag.FOTO_VITRINE_POS == produtoMag.FOTO_LOOK_POS)
                            rbVitrineLook.Checked = true;
                        else if (produtoMag.FOTO_VITRINE_POS == produtoMag.FOTO_FRENTE_CAB_POS)
                            rbVitrineFrenteCabeca.Checked = true;
                        else if (produtoMag.FOTO_VITRINE_POS == produtoMag.FOTO_FRENTE_SEMCAB_POS)
                            rbVitrineFrenteSemCabeca.Checked = true;
                        else if (produtoMag.FOTO_VITRINE_POS == produtoMag.FOTO_COSTAS_POS)
                            rbVitrineCostas.Checked = true;
                        else if (produtoMag.FOTO_VITRINE_POS == produtoMag.FOTO_DETALHE_POS)
                            rbVitrineDetalhe.Checked = true;
                        else if (produtoMag.FOTO_VITRINE_POS == produtoMag.FOTO_LADO_POS)
                            rbVitrineLado.Checked = true;
                        else
                            rbVitrineFrenteCabeca.Checked = true;

                        //Tratar Hover da Imagem
                        rbHoverLook.Checked = false;
                        rbHoverFrenteCabeca.Checked = false;
                        rbHoverFrenteSemCabeca.Checked = false;
                        rbHoverCostas.Checked = false;
                        rbHoverDetalhe.Checked = false;
                        rbHoverLado.Checked = false;
                        if (produtoMag.FOTO_HOVER == produtoMag.FOTO_LOOK)
                            rbHoverLook.Checked = true;
                        else if (produtoMag.FOTO_HOVER == produtoMag.FOTO_FRENTE_CAB)
                            rbHoverFrenteCabeca.Checked = true;
                        else if (produtoMag.FOTO_HOVER == produtoMag.FOTO_FRENTE_SEMCAB)
                            rbHoverFrenteSemCabeca.Checked = true;
                        else if (produtoMag.FOTO_HOVER == produtoMag.FOTO_COSTAS)
                            rbHoverCostas.Checked = true;
                        else if (produtoMag.FOTO_HOVER == produtoMag.FOTO_DETALHE)
                            rbHoverDetalhe.Checked = true;
                        else if (produtoMag.FOTO_HOVER == produtoMag.FOTO_LADO)
                            rbHoverLado.Checked = true;
                        else
                            rbHoverCostas.Checked = true;

                        if (produtoMag.FOTO_LOOK_POS != null)
                            ddlOrdemLook.SelectedValue = produtoMag.FOTO_LOOK_POS.ToString();
                        if (produtoMag.FOTO_FRENTE_CAB_POS != null)
                            ddlOrdemFrenteCabeca.SelectedValue = produtoMag.FOTO_FRENTE_CAB_POS.ToString();
                        if (produtoMag.FOTO_FRENTE_SEMCAB_POS != null)
                            ddlOrdemFrenteSemCabeca.SelectedValue = produtoMag.FOTO_FRENTE_SEMCAB_POS.ToString();
                        if (produtoMag.FOTO_COSTAS_POS != null)
                            ddlOrdemCostas.SelectedValue = produtoMag.FOTO_COSTAS_POS.ToString();
                        if (produtoMag.FOTO_DETALHE_POS != null)
                            ddlOrdemDetalhe.SelectedValue = produtoMag.FOTO_DETALHE_POS.ToString();
                        if (produtoMag.FOTO_LADO_POS != null)
                            ddlOrdemLado.SelectedValue = produtoMag.FOTO_LADO_POS.ToString();


                        CarregarBlocos(produtoMag);
                        CarregarCategorias();

                    }
                    else
                    {
                        if (grupoProduto.ToLower() == "calca")
                            grupoProduto = "Calça";
                        if (grupoProduto.ToLower() == "macacao")
                            grupoProduto = "Macacão";
                        if (grupoProduto.ToLower() == "tenis")
                            grupoProduto = "Tênis";

                        //txtMetaTitulo.Text = Utils.WebControls.AlterarPrimeiraLetraMaiscula(grupoProduto.ToLower().Replace("basic", "camiseta")) + " " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(c.ToLower()) + " " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(griffe.ToLower()) + " Handbook";

                        txtMetaTitulo.Text = Utils.WebControls.AlterarPrimeiraLetraMaiscula(grupoProduto.ToLower().Replace("basic", "camiseta")) + " " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(griffe.ToLower());

                        txtMetaKeyword.Text = "roupas online, moda feminina, moda masculina, roupas em promoção, handbook, " + grupoProduto.ToLower().Replace("basic", "camiseta") + ", " + c.ToLower() + ", " + griffe.ToLower() + ", fashion, roupas, estilo, looks, moda, outono, inverno";
                        if (griffe.ToLower().Trim() == "petit")
                            txtMetaKeyword.Text = txtMetaKeyword.Text + ", mulheres pequenas, mulheres baixinhas";

                        txtURLProduto.Text = "handbook-" + grupoProduto.ToLower().Replace("basic", "camiseta").Replace(" ", "-") + "-" + griffe.ToLower() + "-" + c.ToLower().RemoverAcento().Replace(" ", "-");

                        //se ja existe um produto de outra cor cadastrado, preenche alguns campos automaticamente
                        var produtoExiste = eController.ObterMagentoTodosProdutos(produto).Where(x => x.ID_PRODUTO_MAG_CONFIG == 0).FirstOrDefault();
                        if (produtoExiste != null)
                        {
                            txtNomeProdutoMagento.Text = produtoExiste.NOME_MAG;
                            ddlGrupoMagento.SelectedValue = produtoExiste.ECOM_GRUPO_PRODUTO.ToString();
                            txtDescProduto.Text = produtoExiste.PRODUTO_DESC;
                            txtDescProdutoCurta.Text = produtoExiste.PRODUTO_DESC_CURTA;
                            txtPeso.Text = produtoExiste.PESO_KG.ToString();
                            ddlGrupoMacro.SelectedValue = produtoExiste.ECOM_GRUPO_MACRO.ToString();

                            txtPreco.Text = produtoExiste.PRECO.ToString();
                            if (produtoExiste.PRECO_PROMO != null)
                                txtPrecoPromocional.Text = produtoExiste.PRECO_PROMO.ToString();
                            if (produtoExiste.PRECO_PROMO_DATA_DE != null)
                                txtDataPrecoPromoDe.Text = Convert.ToDateTime(produtoExiste.PRECO_PROMO_DATA_DE).ToString("dd/MM/yyyy");
                            if (produtoExiste.PRECO_PROMO_DATA_ATE != null)
                                txtDataPrecoPromoAte.Text = Convert.ToDateTime(produtoExiste.PRECO_PROMO_DATA_ATE).ToString("dd/MM/yyyy");

                            //caracteristicas
                            if (produtoExiste.ECOM_TIPO_MODELAGEM != null)
                                ddlTipoModelagem.SelectedValue = produtoExiste.ECOM_TIPO_MODELAGEM.ToString();
                            if (produtoExiste.ECOM_TIPO_TECIDO != null)
                                ddlTipoTecido.SelectedValue = produtoExiste.ECOM_TIPO_TECIDO.ToString();
                            if (produtoExiste.ECOM_TIPO_MANGA != null)
                                ddlTipoManga.SelectedValue = produtoExiste.ECOM_TIPO_MANGA.ToString();
                            if (produtoExiste.ECOM_TIPO_GOLA != null)
                                ddlTipoGola.SelectedValue = produtoExiste.ECOM_TIPO_GOLA.ToString();
                            if (produtoExiste.ECOM_TIPO_COMPRIMENTO != null)
                                ddlTipoComprimento.SelectedValue = produtoExiste.ECOM_TIPO_COMPRIMENTO.ToString();
                            if (produtoExiste.ECOM_TIPO_ESTILO != null)
                                ddlTipoEstilo.SelectedValue = produtoExiste.ECOM_TIPO_ESTILO.ToString();
                            if (produtoExiste.ECOM_TIPO_LINHA != null)
                                ddlTipoLinha.SelectedValue = produtoExiste.ECOM_TIPO_LINHA.ToString();
                            if (produtoExiste.ECOM_SIGNED != null)
                                ddlSigned.SelectedValue = produtoExiste.ECOM_SIGNED.ToString();

                            ddlTipoRelacionado.SelectedValue = produtoExiste.ECOM_TIPO_RELACIONADO.ToString();

                        }
                        else
                        {
                            // sugestao de valores para algums campos
                            var precoTO = baseController.BuscaPrecoOriginalProduto(p.PRODUTO1.Trim());
                            var precoTL = baseController.BuscaPrecoTLProduto(p.PRODUTO1.Trim());
                            if (precoTO != null)
                            {
                                txtPreco.Text = precoTO.PRECO1.ToString();

                                if (precoTL != null && precoTL.PRECO1 < precoTO.PRECO1)
                                    txtPrecoPromocional.Text = precoTL.PRECO1.ToString();
                            }

                        }

                        btAtualizarMagento.Enabled = false;
                    }
                }

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btAtualizarMagento.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizarMagento, null) + ";");
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
            btSalvarContinuar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvarContinuar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarCaixaDimensao()
        {
            var caixaDim = eController.ObterMagentoCaixaDimensao();

            caixaDim.Insert(0, new ECOM_CAIXA_DIMENSAO { CODIGO = 0, DESCRICAO = "Selecione" });
            ddlCaixa.DataSource = caixaDim;
            ddlCaixa.DataBind();

            //selecao padrao CAIXA P
            ddlCaixa.SelectedValue = "2";
        }
        private void CarregarCorMagento()
        {
            var corMagento = eController.ObterMagentoCor();
            if (corMagento != null)
            {
                corMagento.Insert(0, new ECOM_COR { CODIGO = 0, COR = "Selecione" });
                ddlCorMagento.DataSource = corMagento;
                ddlCorMagento.DataBind();
            }
        }
        private void CarregarGrupoMacro()
        {
            var grupoMacro = eController.ObterMagentoGrupoMacro();
            if (grupoMacro != null)
            {
                grupoMacro.Insert(0, new ECOM_GRUPO_MACRO { CODIGO = 0, GRUPO_MACRO = "Selecione" });
                ddlGrupoMacro.DataSource = grupoMacro;
                ddlGrupoMacro.DataBind();
            }
        }
        private void CarregarTipoModelagem(string griffe, string grupoProduto)
        {
            var tipoModelagem = eController.ObterMagentoTipoModelagem(griffe, grupoProduto);
            if (tipoModelagem != null && tipoModelagem.Count() > 0)
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = 0, TIPO_MODELAGEM = "Selecione" });
            }
            else
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = -1, TIPO_MODELAGEM = "-" });
                ddlTipoModelagem.Enabled = false;
            }

            ddlTipoModelagem.DataSource = tipoModelagem;
            ddlTipoModelagem.DataBind();
        }
        private void CarregarTipoTecido(string griffe, string grupoProduto)
        {
            var tipoTecido = eController.ObterMagentoTipoTecido(griffe, grupoProduto);
            if (tipoTecido != null && tipoTecido.Count() > 0)
            {
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = 0, TIPO_TECIDO = "Selecione" });
            }
            else
            {
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = -1, TIPO_TECIDO = "-" });
                ddlTipoTecido.Enabled = false;
            }

            ddlTipoTecido.DataSource = tipoTecido;
            ddlTipoTecido.DataBind();
        }
        private void CarregarTipoManga(string griffe, string grupoProduto)
        {
            var tipoManga = eController.ObterMagentoTipoManga(griffe, grupoProduto);
            if (tipoManga != null && tipoManga.Count() > 0)
            {
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = 0, TIPO_MANGA = "Selecione" });
            }
            else
            {
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = -1, TIPO_MANGA = "-" });
                ddlTipoManga.Enabled = false;
            }

            ddlTipoManga.DataSource = tipoManga;
            ddlTipoManga.DataBind();
        }
        private void CarregarTipoGola(string griffe, string grupoProduto)
        {
            var tipoGola = eController.ObterMagentoTipoGola(griffe, grupoProduto);
            if (tipoGola != null && tipoGola.Count() > 0)
            {
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = 0, TIPO_GOLA = "Selecione" });
            }
            else
            {
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = -1, TIPO_GOLA = "-" });
                ddlTipoGola.Enabled = false;
            }

            ddlTipoGola.DataSource = tipoGola;
            ddlTipoGola.DataBind();
        }
        private void CarregarTipoComprimento(string griffe, string grupoProduto)
        {
            var tipoComprimento = eController.ObterMagentoTipoComprimento(griffe, grupoProduto);
            if (tipoComprimento != null && tipoComprimento.Count() > 0)
            {
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = 0, TIPO_COMPRIMENTO = "Selecione" });
            }
            else
            {
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = -1, TIPO_COMPRIMENTO = "-" });
                ddlTipoComprimento.Enabled = false;
            }

            ddlTipoComprimento.DataSource = tipoComprimento;
            ddlTipoComprimento.DataBind();
        }
        private void CarregarTipoEstilo(string griffe, string grupoProduto)
        {
            var tipoEstilo = eController.ObterMagentoTipoEstilo(griffe, grupoProduto);
            if (tipoEstilo != null && tipoEstilo.Count() > 0)
            {
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = 0, TIPO_ESTILO = "Selecione" });
            }
            else
            {
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = -1, TIPO_ESTILO = "-" });
                ddlTipoEstilo.Enabled = false;
            }

            ddlTipoEstilo.DataSource = tipoEstilo;
            ddlTipoEstilo.DataBind();
        }
        private void CarregarTipoLinha(string colecao)
        {
            var tipoLinha = eController.ObterMagentoTipoLinha(colecao);
            if (tipoLinha != null && tipoLinha.Count() > 0)
            {
                tipoLinha.Insert(0, new ECOM_TIPO_LINHA { CODIGO = 0, TIPO_LINHA = "" });
            }
            else
            {
                tipoLinha.Insert(0, new ECOM_TIPO_LINHA { CODIGO = -1, TIPO_LINHA = "-" });
                ddlTipoLinha.Enabled = false;
            }

            ddlTipoLinha.DataSource = tipoLinha;
            ddlTipoLinha.DataBind();
        }
        private void CarregarSigned(string colecao)
        {
            var signed = eController.ObterMagentoSigned(colecao);
            if (signed != null)
            {
                signed.Insert(0, new ECOM_SIGNED { CODIGO = 0, SIGNED = "" });
            }

            ddlSigned.DataSource = signed;
            ddlSigned.DataBind();
        }
        private void CarregarGrupoMagento(string codCategoria, string griffe, string grupoProduto)
        {
            var grupoMagento = eController.ObterMagentoGrupoProduto();
            if (grupoMagento != null)
            {
                if (codCategoria == "01") //PEÇAS
                {
                    grupoMagento = grupoMagento.Where(p => p.GRIFFE.Contains(griffe.Trim()) && p.GRUPO_PRODUTO.Contains(grupoProduto.Trim())).ToList();
                }
                else //ACESSORIOS
                {
                    grupoMagento = grupoMagento.Where(p => p.CODIGO_PAI == 6).ToList();
                }

                grupoMagento = grupoMagento.OrderBy(p => p.GRUPO).ToList();
                grupoMagento.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });
                ddlGrupoMagento.DataSource = grupoMagento;
                ddlGrupoMagento.DataBind();
            }
        }
        private void CarregarCategoriaMag(string griffe = "", string grupoProduto = "")
        {
            var catMag = eController.ObterMagentoGrupoProdutoAberto().Where(p => p.ATIVO == true).ToList();

            if (griffe != "")
                catMag = catMag.Where(p => p.GRIFFE == null || p.GRIFFE == "" || p.GRIFFE.Contains(griffe.Trim())).ToList();

            if (grupoProduto != "")
                catMag = catMag.Where(p => p.GRUPO_PRODUTO == null || p.GRUPO_PRODUTO == "" || p.GRUPO_PRODUTO.Contains(grupoProduto.Trim())).ToList();


            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();
        }

        protected void ddlGrupoMagento_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarBloco(Convert.ToInt32(ddlGrupoMagento.SelectedValue));
        }
        private void CarregarBloco(int ecomGrupoProduto)
        {
            var bloco = eController.ObterBlocoPorCategoriaMag(ecomGrupoProduto);
            bloco.Insert(0, new ECOM_BLOCO_PRODUTO { CODIGO = 0, BLOCO = "" });

            ddlBloco.DataSource = bloco;
            ddlBloco.DataBind();
        }
        private void CarregarTipoRelacionado()
        {
            var tipoRelacionado = eController.ObterTipoRelacionado();
            ddlTipoRelacionado.DataSource = tipoRelacionado;
            ddlTipoRelacionado.DataBind();
        }

        private void CarregarOrdemFoto()
        {
            var ordemFoto = new List<ListItem>();
            ordemFoto.Add(new ListItem { Value = "", Text = "" });
            ordemFoto.Add(new ListItem { Value = "1", Text = "1" });
            ordemFoto.Add(new ListItem { Value = "2", Text = "2" });
            ordemFoto.Add(new ListItem { Value = "3", Text = "3" });
            if (gCodCategoria == "01")
            {
                ordemFoto.Add(new ListItem { Value = "4", Text = "4" });
                ordemFoto.Add(new ListItem { Value = "5", Text = "5" });
                ordemFoto.Add(new ListItem { Value = "6", Text = "6" });
            }

            ddlOrdemLook.DataSource = ordemFoto;
            ddlOrdemLook.DataBind();

            ddlOrdemFrenteCabeca.DataSource = ordemFoto;
            ddlOrdemFrenteCabeca.DataBind();

            ddlOrdemFrenteSemCabeca.DataSource = ordemFoto;
            ddlOrdemFrenteSemCabeca.DataBind();

            ddlOrdemCostas.DataSource = ordemFoto;
            ddlOrdemCostas.DataBind();

            ddlOrdemDetalhe.DataSource = ordemFoto;
            ddlOrdemDetalhe.DataBind();

            ddlOrdemLado.DataSource = ordemFoto;
            ddlOrdemLado.DataBind();

            if (gCodCategoria == "01")
            {
                ddlOrdemLook.SelectedValue = "3";
                ddlOrdemFrenteCabeca.SelectedValue = "5";
                ddlOrdemFrenteSemCabeca.SelectedValue = "1";
                ddlOrdemCostas.SelectedValue = "2";
                ddlOrdemDetalhe.SelectedValue = "4";
                ddlOrdemLado.SelectedValue = "6";
            }
            else
            {
                ddlOrdemLook.SelectedValue = "";
                ddlOrdemLook.Enabled = false;
                ddlOrdemFrenteCabeca.SelectedValue = "1";
                ddlOrdemFrenteSemCabeca.SelectedValue = "";
                ddlOrdemFrenteSemCabeca.Enabled = false;
                ddlOrdemCostas.SelectedValue = "2";
                ddlOrdemDetalhe.SelectedValue = "3";
                ddlOrdemLado.SelectedValue = "";
                ddlOrdemLado.Enabled = false;
            }

        }
        private void BloquearRadioVitrine()
        {
            if (gCodCategoria == "02")
            {
                rbHoverLook.Enabled = false;
                rbHoverFrenteSemCabeca.Enabled = false;
                rbHoverLado.Enabled = false;

                rbVitrineLook.Enabled = false;
                rbVitrineFrenteSemCabeca.Enabled = false;
                rbVitrineLado.Enabled = false;
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labCorMagento.ForeColor = _OK;
            if (ddlCorMagento.SelectedValue == "0")
            {
                labCorMagento.ForeColor = _notOK;
                retorno = false;
            }

            labGrupoMagento.ForeColor = _OK;
            if (ddlGrupoMagento.SelectedValue == "0")
            {
                labGrupoMagento.ForeColor = _notOK;
                retorno = false;
            }

            labNomeProdutoMagento.ForeColor = _OK;
            if (txtNomeProdutoMagento.Text.Trim() == "")
            {
                labNomeProdutoMagento.ForeColor = _notOK;
                retorno = false;
            }

            labDescProduto.ForeColor = _OK;
            if (txtDescProduto.Text.Trim() == "")
            {
                labDescProduto.ForeColor = _notOK;
                retorno = false;
            }

            labDescProdutoCurta.ForeColor = _OK;
            if (txtDescProdutoCurta.Text.Trim() == "")
            {
                labDescProdutoCurta.ForeColor = _notOK;
                retorno = false;
            }

            labTipoRelacionado.ForeColor = _OK;
            if (ddlTipoRelacionado.SelectedValue == "0")
            {
                labTipoRelacionado.ForeColor = _notOK;
                retorno = false;
            }

            labPeso.ForeColor = _OK;
            if (txtPeso.Text.Trim() == "")
            {
                labPeso.ForeColor = _notOK;
                retorno = false;
            }

            labGrupoMacro.ForeColor = _OK;
            if (ddlGrupoMacro.SelectedValue == "0")
            {
                labGrupoMacro.ForeColor = _notOK;
                retorno = false;
            }

            labPreco.ForeColor = _OK;
            if (txtPreco.Text.Trim() == "")
            {
                labPreco.ForeColor = _notOK;
                retorno = false;
            }

            //labPrecoPromocional.ForeColor = _OK;
            //if (txtPrecoPromocional.Text.Trim() == "")
            //{
            //    labPrecoPromocional.ForeColor = _notOK;
            //    retorno = false;
            //}

            labURLProduto.ForeColor = _OK;
            if (txtURLProduto.Text.Trim() == "")
            {
                labURLProduto.ForeColor = _notOK;
                retorno = false;
            }

            labCaixa.ForeColor = _OK;
            if (ddlCaixa.SelectedValue == "0")
            {
                labCaixa.ForeColor = _notOK;
                retorno = false;
            }

            labVisibilidade.ForeColor = _OK;
            if (ddlVisibilidade.SelectedValue == "0")
            {
                labVisibilidade.ForeColor = _notOK;
                retorno = false;
            }

            labTipoModelagem.ForeColor = _OK;
            if (ddlTipoModelagem.SelectedValue == "0")
            {
                labTipoModelagem.ForeColor = _notOK;
                retorno = false;
            }

            labTipoTecido.ForeColor = _OK;
            if (ddlTipoTecido.SelectedValue == "0")
            {
                labTipoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labTipoManga.ForeColor = _OK;
            if (ddlTipoManga.SelectedValue == "0")
            {
                labTipoManga.ForeColor = _notOK;
                retorno = false;
            }

            labTipoGola.ForeColor = _OK;
            if (ddlTipoGola.SelectedValue == "0")
            {
                labTipoGola.ForeColor = _notOK;
                retorno = false;
            }

            labTipoComprimento.ForeColor = _OK;
            if (ddlTipoComprimento.SelectedValue == "0")
            {
                labTipoComprimento.ForeColor = _notOK;
                retorno = false;
            }

            labTipoEstilo.ForeColor = _OK;
            if (ddlTipoEstilo.SelectedValue == "0")
            {
                labTipoEstilo.ForeColor = _notOK;
                retorno = false;
            }

            //labTipoLinha.ForeColor = _OK;
            //if (ddlTipoLinha.SelectedValue == "0")
            //{
            //    labTipoLinha.ForeColor = _notOK;
            //    retorno = false;
            //}

            labMetaTitulo.ForeColor = _OK;
            if (txtMetaTitulo.Text.Trim() == "")
            {
                labMetaTitulo.ForeColor = _notOK;
                retorno = false;
            }

            labMetaKeyword.ForeColor = _OK;
            if (txtMetaKeyword.Text.Trim() == "")
            {
                labMetaKeyword.ForeColor = _notOK;
                retorno = false;
            }

            labMetaDescricao.ForeColor = _OK;
            if (txtMetaDescricao.Text.Trim() == "")
            {
                labMetaDescricao.ForeColor = _notOK;
                retorno = false;
            }

            labHabilitado.ForeColor = _OK;
            if (ddlHabilitado.SelectedValue == "")
            {
                labHabilitado.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarPreco()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labPreco.ForeColor = _OK;
            labPrecoPromocional.ForeColor = _OK;
            if (txtPrecoPromocional.Text.Trim() != "" && (Convert.ToDecimal(txtPrecoPromocional.Text) > Convert.ToDecimal(txtPreco.Text)))
            {
                labPreco.ForeColor = _notOK;
                labPrecoPromocional.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarFotos(string produto, string cor)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            string pathPhoto = Server.MapPath("~/FotosHandbookOnline") + "\\" + produto + cor + "@@@." + extensaoFoto;

            bool grupo6Foto = (hidCodCategoria.Value == "01") ? true : false;

            labFotoLook.ForeColor = _OK;
            if (!File.Exists(@pathPhoto.Replace("@@@", "L")) && grupo6Foto)
            {
                labFotoLook.ForeColor = _notOK;
                retorno = false;
            }

            labFotoFrenteCabeca.ForeColor = _OK;
            if (!File.Exists(@pathPhoto.Replace("@@@", "F")))
            {
                labFotoFrenteCabeca.ForeColor = _notOK;
                retorno = false;
            }

            labFotoFrenteSemCabeca.ForeColor = _OK;
            if (!File.Exists(@pathPhoto.Replace("@@@", "G")) && grupo6Foto)
            {
                labFotoFrenteSemCabeca.ForeColor = _notOK;
                retorno = false;
            }

            labFotoCostas.ForeColor = _OK;
            if (!File.Exists(@pathPhoto.Replace("@@@", "C")))
            {
                labFotoCostas.ForeColor = _notOK;
                retorno = false;
            }

            labFotoDetalhe.ForeColor = _OK;
            if (!File.Exists(@pathPhoto.Replace("@@@", "D")))
            {
                labFotoDetalhe.ForeColor = _notOK;
                retorno = false;
            }

            labFotoLado.ForeColor = _OK;
            if (!File.Exists(@pathPhoto.Replace("@@@", "A")) && grupo6Foto)
            {
                labFotoLado.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarPeriodoDataPromocional()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labDataPrecoPromocionalDe.ForeColor = _OK;
            labDataPrecoPromocionalAte.ForeColor = _OK;
            if (txtDataPrecoPromoDe.Text != "" && txtDataPrecoPromoAte.Text != "")
            {
                if (Convert.ToDateTime(txtDataPrecoPromoAte.Text) < Convert.ToDateTime(txtDataPrecoPromoDe.Text))
                {
                    labDataPrecoPromocionalDe.ForeColor = _notOK;
                    labDataPrecoPromocionalAte.ForeColor = _notOK;
                    retorno = false;
                }

            }

            return retorno;
        }
        private bool ValidarPesoKG()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            //labPeso.ForeColor = _OK;
            //if (txtPeso.Text.Trim() != "" && (Convert.ToDecimal(txtPeso.Text) > 1))
            //{
            //    labPeso.ForeColor = _notOK;
            //    retorno = false;
            //}

            return retorno;
        }
        private bool ValidarNomeRepetido()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labNomeProdutoMagento.ForeColor = _OK;

            string produto = txtProduto.Text.Trim();
            string nome = txtNomeProdutoMagento.Text.Trim();

            if (!eController.ValidarNomeProdutoRepetido(produto, nome))
            {
                labNomeProdutoMagento.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarOrdemFotos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            bool grupo6Foto = (hidCodCategoria.Value == "01") ? true : false;

            labOrdemFoto.ForeColor = _OK;

            if (ddlOrdemLook.SelectedValue == "" && grupo6Foto)
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ddlOrdemFrenteCabeca.SelectedValue == "")
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ddlOrdemFrenteSemCabeca.SelectedValue == "" && grupo6Foto)
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ddlOrdemCostas.SelectedValue == "")
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ddlOrdemDetalhe.SelectedValue == "")
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ddlOrdemLado.SelectedValue == "" && grupo6Foto)
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarOrdemFotosRepetida()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            bool grupo6Foto = (hidCodCategoria.Value == "01") ? true : false;

            labOrdemFoto.ForeColor = _OK;

            string ordemLook = ddlOrdemLook.SelectedValue;
            string ordemFrenteCabeca = ddlOrdemFrenteCabeca.SelectedValue;
            string ordemFrenteSemCabeca = ddlOrdemFrenteSemCabeca.SelectedValue;
            string ordemCostas = ddlOrdemCostas.SelectedValue;
            string ordemDetalhe = ddlOrdemDetalhe.SelectedValue;
            string ordemLado = ddlOrdemLado.SelectedValue;

            if ((ordemLook == ordemFrenteCabeca ||
                ordemLook == ordemFrenteSemCabeca ||
                ordemLook == ordemCostas ||
                ordemLook == ordemDetalhe ||
                ordemLook == ordemLado) && grupo6Foto
                )
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ordemFrenteCabeca == ordemLook ||
                ordemFrenteCabeca == ordemFrenteSemCabeca ||
                ordemFrenteCabeca == ordemCostas ||
                ordemFrenteCabeca == ordemDetalhe ||
                ordemFrenteCabeca == ordemLado
                )
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if ((ordemFrenteSemCabeca == ordemLook ||
                ordemFrenteSemCabeca == ordemFrenteCabeca ||
                ordemFrenteSemCabeca == ordemCostas ||
                ordemFrenteSemCabeca == ordemDetalhe ||
                ordemFrenteSemCabeca == ordemLado) && grupo6Foto
                )
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ordemCostas == ordemLook ||
                ordemCostas == ordemFrenteCabeca ||
                ordemCostas == ordemFrenteSemCabeca ||
                ordemCostas == ordemDetalhe ||
                ordemCostas == ordemLado
                )
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if (ordemDetalhe == ordemLook ||
                ordemDetalhe == ordemFrenteCabeca ||
                ordemDetalhe == ordemFrenteSemCabeca ||
                ordemDetalhe == ordemCostas ||
                ordemDetalhe == ordemLado
                )
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            if ((ordemLado == ordemLook ||
                ordemLado == ordemFrenteCabeca ||
                ordemLado == ordemFrenteSemCabeca ||
                ordemLado == ordemCostas ||
                ordemLado == ordemDetalhe) && grupo6Foto
                )
            {
                labOrdemFoto.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!SalvarEValidar())
                {
                    return;
                }

                btSalvar.Enabled = false;
                btSalvarContinuar.Enabled = false;

                labErro.Text = "Produto cadastrado com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btSalvarContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!SalvarEValidar())
                {
                    return;
                }

                string produto = hidProduto.Value;
                string cor = "";
                //verifica se existe outra cor deste produto para continuar cadastrando
                var coresProduto = baseController.BuscaProdutoCores(produto).Where(p => p.COR_PRODUTO.Trim() != hidCor.Value);
                if (coresProduto != null && coresProduto.Count() > 0)
                {
                    foreach (var corProduto in coresProduto)
                    {
                        //verifica se ja esta cadastrado
                        var corProdutoMag = eController.ObterMagentoProdutoPorcor(produto, corProduto.COR_PRODUTO.Trim());
                        if (corProdutoMag == null)
                        {
                            cor = corProduto.COR_PRODUTO.Trim();
                            Response.Redirect("ecom_cad_produto_edit.aspx?co=" + hidColecao.Value + "&p=" + produto + "&c=" + cor + "");
                            break;
                        }
                    }
                }

                btSalvar.Enabled = false;
                btSalvarContinuar.Enabled = false;

                labErro.Text = "Produto cadastrado com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private bool SalvarEValidar()
        {


            if (!ValidarCampos())
            {
                labErro.Text = "Preencha corretamente os campos em vermelho.";
                return false;
            }

            if (!ValidarNomeRepetido())
            {
                labErro.Text = "Este Nome já foi cadastrado em outro Produto.";
                return false;
            }

            if (!ValidarPreco())
            {
                labErro.Text = "Preço Promocional não pode ser maior que o Preço normal do Produto.";
                return false;
            }

            if (!ValidarPeriodoDataPromocional())
            {
                labErro.Text = "Data \"ATÉ\" não pode ser menos que a Data \"DE\"";
                return false;
            }

            if (!ValidarPesoKG())
            {
                labErro.Text = "O Peso deve ser menor que 1 KG.";
                return false;
            }

            if (txtNomeProdutoMagento.Text.Trim() != "" && txtNomeProdutoMagento.Text.Trim().Length > 35)
            {
                labErro.Text = "O nome do Produto no magento não pode ser maior que 35 caracteres.";
                return false;
            }

            if (!rbVitrineLook.Checked && !rbVitrineFrenteCabeca.Checked && !rbVitrineFrenteSemCabeca.Checked && !rbVitrineCostas.Checked && !rbVitrineDetalhe.Checked && !rbVitrineLado.Checked)
            {
                labErro.Text = "Por favor, selecione a Foto que será a \"VITRINE\".";
                return false;
            }

            if (!rbHoverLook.Checked && !rbHoverFrenteCabeca.Checked && !rbHoverFrenteSemCabeca.Checked && !rbHoverCostas.Checked && !rbHoverDetalhe.Checked && !rbHoverLado.Checked)
            {
                labErro.Text = "Por favor, selecione a Foto que será o \"HOVER\".";
                return false;
            }
            if (!ValidarOrdemFotos())
            {
                labErro.Text = "Por favor, selecione a Ordenação das Fotos.";
                return false;
            }

            if (!ValidarOrdemFotosRepetida())
            {
                labErro.Text = "Por favor, verifique a Ordenação das Fotos. Não é permitido selecionar ordens repetidas.";
                return false;
            }

            SalvarProdutoConfig();

            return true;
        }
        private bool SalvarProdutoConfig()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            var produtoMag = new ECOM_PRODUTO();
            if (hidCodigo.Value != "")
                produtoMag = eController.ObterMagentoProduto(Convert.ToInt32(hidCodigo.Value));

            if (hidIdMagento.Value != "")
                produtoMag.ID_PRODUTO_MAG = Convert.ToInt32(hidIdMagento.Value);
            if (hidIdMagentoConfig.Value != "")
                produtoMag.ID_PRODUTO_MAG_CONFIG = Convert.ToInt32(hidIdMagentoConfig.Value);
            else
                produtoMag.ID_PRODUTO_MAG_CONFIG = 0;

            produtoMag.CAD_MKTPLACE = cbCadastrarMKTPlace.Checked;
            produtoMag.PRODUTO = txtProduto.Text.Trim();
            produtoMag.NOME = txtNomeProduto.Text.Trim();
            produtoMag.NOME_MAG = txtNomeProdutoMagento.Text.Trim();
            produtoMag.PRODUTO_DESC = txtDescProduto.Text.Trim();
            produtoMag.PRODUTO_DESC_CURTA = txtDescProdutoCurta.Text.Trim();
            produtoMag.SKU = txtSKU.Text;
            produtoMag.COR = hidCor.Value;
            produtoMag.ECOM_COR = Convert.ToInt32(ddlCorMagento.SelectedValue);
            produtoMag.PESO_KG = Convert.ToDecimal(txtPeso.Text);
            produtoMag.GRUPO_PRODUTO = txtGrupoProduto.Text;
            produtoMag.ECOM_GRUPO_PRODUTO = Convert.ToInt32(ddlGrupoMagento.SelectedValue);
            produtoMag.PRECO = Convert.ToDecimal(txtPreco.Text);
            if (txtPrecoPromocional.Text != "")
                produtoMag.PRECO_PROMO = Convert.ToDecimal(txtPrecoPromocional.Text);
            else
                produtoMag.PRECO_PROMO = null;

            if (txtDataPrecoPromoDe.Text != "")
                produtoMag.PRECO_PROMO_DATA_DE = Convert.ToDateTime(txtDataPrecoPromoDe.Text);
            else
                produtoMag.PRECO_PROMO_DATA_DE = null;

            if (txtDataPrecoPromoAte.Text != "")
                produtoMag.PRECO_PROMO_DATA_ATE = Convert.ToDateTime(txtDataPrecoPromoAte.Text);
            else
                produtoMag.PRECO_PROMO_DATA_ATE = null;

            produtoMag.ECOM_GRUPO_MACRO = Convert.ToInt32(ddlGrupoMacro.SelectedValue);

            if (Convert.ToInt32(ddlTipoModelagem.SelectedValue) > 0)
                produtoMag.ECOM_TIPO_MODELAGEM = Convert.ToInt32(ddlTipoModelagem.SelectedValue);
            if (Convert.ToInt32(ddlTipoTecido.SelectedValue) > 0)
                produtoMag.ECOM_TIPO_TECIDO = Convert.ToInt32(ddlTipoTecido.SelectedValue);
            if (Convert.ToInt32(ddlTipoManga.SelectedValue) > 0)
                produtoMag.ECOM_TIPO_MANGA = Convert.ToInt32(ddlTipoManga.SelectedValue);
            if (Convert.ToInt32(ddlTipoGola.SelectedValue) > 0)
                produtoMag.ECOM_TIPO_GOLA = Convert.ToInt32(ddlTipoGola.SelectedValue);
            if (Convert.ToInt32(ddlTipoComprimento.SelectedValue) > 0)
                produtoMag.ECOM_TIPO_COMPRIMENTO = Convert.ToInt32(ddlTipoComprimento.SelectedValue);
            if (Convert.ToInt32(ddlTipoEstilo.SelectedValue) > 0)
                produtoMag.ECOM_TIPO_ESTILO = Convert.ToInt32(ddlTipoEstilo.SelectedValue);

            if (Convert.ToInt32(ddlTipoLinha.SelectedValue) > 0)
                produtoMag.ECOM_TIPO_LINHA = Convert.ToInt32(ddlTipoLinha.SelectedValue);
            else
                produtoMag.ECOM_TIPO_LINHA = null;

            if (Convert.ToInt32(ddlSigned.SelectedValue) > 0)
                produtoMag.ECOM_SIGNED = Convert.ToInt32(ddlSigned.SelectedValue);
            else
                produtoMag.ECOM_SIGNED = null;

            produtoMag.OBSERVACAO = txtObservacao.Text.Trim();
            produtoMag.ECOM_TIPO_RELACIONADO = Convert.ToInt32(ddlTipoRelacionado.SelectedValue);
            produtoMag.VISIBILIDADE = ddlVisibilidade.SelectedValue;
            produtoMag.ECOM_CAIXA_DIMENSAO = Convert.ToInt32(ddlCaixa.SelectedValue);

            produtoMag.META_TITULO = txtMetaTitulo.Text.Trim();
            produtoMag.META_KEYWORD = txtMetaKeyword.Text.Trim();
            produtoMag.META_DESCRICAO = txtMetaDescricao.Text.Trim();

            var nomeProduto = produtoMag.NOME_MAG.RemoverAcento();
            nomeProduto = nomeProduto.ToLower().Replace(produtoMag.GRUPO_PRODUTO.ToLower().Trim(), "").Trim().Replace(" ", "-").Replace("  ", "-");
            if (Convert.ToInt32(ddlSigned.SelectedValue) > 0)
                nomeProduto = nomeProduto + "-" + ddlSigned.SelectedItem.Text.ToLower().Trim().Replace(" ", "-");

            var corMangento = eController.ObterMagentoCor(Convert.ToInt32(produtoMag.ECOM_COR)).COR.RemoverAcento();

            produtoMag.URL_PRODUTO = ("handbook-" + produtoMag.GRUPO_PRODUTO.Trim().ToLower().Replace(" ", "-") + "-" + hidGriffe.Value.Trim().ToLower().Replace("feminino", "feminina") + "-" + corMangento.ToLower().Trim().Replace(" ", "-") + "-" + nomeProduto.ToLower().Trim().Replace(" ", "-")).Replace("--", "-");

            if (imgFotoLook.ImageUrl != "")
                produtoMag.FOTO_LOOK = imgFotoLook.ImageUrl;
            if (imgFotoFrenteCabeca.ImageUrl != "")
                produtoMag.FOTO_FRENTE_CAB = imgFotoFrenteCabeca.ImageUrl;
            if (imgFotoFrenteSemCabeca.ImageUrl != "")
                produtoMag.FOTO_FRENTE_SEMCAB = imgFotoFrenteSemCabeca.ImageUrl;
            if (imgFotoCostas.ImageUrl != "")
                produtoMag.FOTO_COSTAS = imgFotoCostas.ImageUrl;
            if (imgFotoDetalhe.ImageUrl != "")
                produtoMag.FOTO_DETALHE = imgFotoDetalhe.ImageUrl;
            if (imgFotoLado.ImageUrl != "")
                produtoMag.FOTO_LADO = imgFotoLado.ImageUrl;

            //VITRINE
            if (rbVitrineLook.Checked)
                produtoMag.FOTO_VITRINE_POS = Convert.ToInt32(ddlOrdemLook.SelectedValue);
            else if (rbVitrineFrenteCabeca.Checked)
                produtoMag.FOTO_VITRINE_POS = Convert.ToInt32(ddlOrdemFrenteCabeca.SelectedValue);
            else if (rbVitrineFrenteSemCabeca.Checked)
                produtoMag.FOTO_VITRINE_POS = Convert.ToInt32(ddlOrdemFrenteSemCabeca.SelectedValue);
            else if (rbVitrineCostas.Checked)
                produtoMag.FOTO_VITRINE_POS = Convert.ToInt32(ddlOrdemCostas.SelectedValue);
            else if (rbVitrineDetalhe.Checked)
                produtoMag.FOTO_VITRINE_POS = Convert.ToInt32(ddlOrdemDetalhe.SelectedValue);
            else if (rbVitrineLado.Checked)
                produtoMag.FOTO_VITRINE_POS = Convert.ToInt32(ddlOrdemLado.SelectedValue);

            //HOVER
            if (rbHoverLook.Checked)
                produtoMag.FOTO_HOVER = produtoMag.FOTO_LOOK;
            else if (rbHoverFrenteCabeca.Checked)
                produtoMag.FOTO_HOVER = produtoMag.FOTO_FRENTE_CAB;
            else if (rbHoverFrenteSemCabeca.Checked)
                produtoMag.FOTO_HOVER = produtoMag.FOTO_FRENTE_SEMCAB;
            else if (rbHoverCostas.Checked)
                produtoMag.FOTO_HOVER = produtoMag.FOTO_COSTAS;
            else if (rbHoverDetalhe.Checked)
                produtoMag.FOTO_HOVER = produtoMag.FOTO_DETALHE;
            else if (rbHoverLado.Checked)
                produtoMag.FOTO_HOVER = produtoMag.FOTO_LADO;

            if (ddlOrdemLook.SelectedValue != "")
                produtoMag.FOTO_LOOK_POS = Convert.ToInt32(ddlOrdemLook.SelectedValue);
            if (ddlOrdemFrenteCabeca.SelectedValue != "")
                produtoMag.FOTO_FRENTE_CAB_POS = Convert.ToInt32(ddlOrdemFrenteCabeca.SelectedValue);
            if (ddlOrdemFrenteSemCabeca.SelectedValue != "")
                produtoMag.FOTO_FRENTE_SEMCAB_POS = Convert.ToInt32(ddlOrdemFrenteSemCabeca.SelectedValue);
            if (ddlOrdemCostas.SelectedValue != "")
                produtoMag.FOTO_COSTAS_POS = Convert.ToInt32(ddlOrdemCostas.SelectedValue);
            if (ddlOrdemDetalhe.SelectedValue != "")
                produtoMag.FOTO_DETALHE_POS = Convert.ToInt32(ddlOrdemDetalhe.SelectedValue);
            if (ddlOrdemLado.SelectedValue != "")
                produtoMag.FOTO_LADO_POS = Convert.ToInt32(ddlOrdemLado.SelectedValue);

            produtoMag.HABILITADO = Convert.ToChar(ddlHabilitado.SelectedValue);
            produtoMag.GRIFFE = hidGriffe.Value;
            produtoMag.COD_CATEGORIA = hidCodCategoria.Value;
            produtoMag.GOOGLE_SHOPPING = true;

            var codigoEcom = 0;

            if (hidCodigo.Value != "")
            {
                produtoMag.STATUS_CADASTRO = 'B';
                produtoMag.DATA_ALTERACAO = DateTime.Now;
                produtoMag.USUARIO_ALTERACAO = usuario.CODIGO_USUARIO;
                eController.AtualizarMagentoProduto(produtoMag);

                codigoEcom = produtoMag.CODIGO;
            }
            else
            {
                produtoMag.COLECAO = hidColecao.Value;
                produtoMag.STATUS_CADASTRO = 'A';
                produtoMag.DATA_INCLUSAO = DateTime.Now;
                produtoMag.USUARIO_INCLUSAO = usuario.CODIGO_USUARIO;
                codigoEcom = eController.InserirMagentoProduto(produtoMag);

                if (codigoEcom > 0)
                {
                    if (Session[PRODCATMAG] != null)
                    {
                        var produtoCATs = new List<ECOM_PRODUTO_CAT>();
                        produtoCATs.AddRange((List<ECOM_PRODUTO_CAT>)Session[PRODCATMAG]);
                        foreach (var p in produtoCATs)
                        {
                            var produtoCat = new ECOM_PRODUTO_CAT();
                            produtoCat.ECOM_GRUPO_PRODUTO = p.ECOM_GRUPO_PRODUTO;
                            produtoCat.ECOM_PRODUTO = codigoEcom;
                            eController.InserirProdutoCategoria(produtoCat);
                        }
                    }
                }
            }

            //manutencao de categorias de cores
            var corCatOK = eController.ObterMagentoGrupoProdutoCor(Convert.ToInt32(produtoMag.ECOM_GRUPO_PRODUTO), Convert.ToInt32(produtoMag.ECOM_COR));
            if (corCatOK != null)
            {
                var cateCorIn = eController.ObterProdutoCategoria(codigoEcom, corCatOK.CODIGO);
                if (cateCorIn == null)
                {
                    var produtoCat = new ECOM_PRODUTO_CAT();
                    produtoCat.ECOM_PRODUTO = codigoEcom;
                    produtoCat.ECOM_GRUPO_PRODUTO = corCatOK.CODIGO;
                    eController.InserirProdutoCategoria(produtoCat);
                }
            }

            if (ddlBloco.SelectedValue != "" && ddlBloco.SelectedValue != "0")
                GerenciarProdutoBloco(produtoMag, Convert.ToInt32(ddlBloco.SelectedValue));

            AtualizarTodosProdutos(produtoMag, usuario.CODIGO_USUARIO);

            return true;
        }

        private void GerenciarProdutoBloco(ECOM_PRODUTO produtoMag, int codigoBloco)
        {
            var produtoNoBloco = eController.ObterBlocoProdutoOrdemPorCodigoEcomProduto(produtoMag.CODIGO, "1");
            foreach (var p in produtoNoBloco)
            {
                if (p != null)
                    eController.ExcluirBlocoProdutoOrdem(p.CODIGO);
            }

            var blocoProdutoOrdem = new ECOM_BLOCO_PRODUTO_ORDEM
            {
                ECOM_BLOCO_PRODUTO = codigoBloco,
                ECOM_PRODUTO = produtoMag.CODIGO,
                ORDEM = 999,
                DATA_INCLUSAO = DateTime.Now,
                TIPO_CATEGORIA = "1"
            };

            eController.InserirBlocoProdutoOrdem(blocoProdutoOrdem);
        }

        private bool AtualizarTodosProdutos(ECOM_PRODUTO produtoMagConfig, int codigoUsuario)
        {

            var produtos = eController.ObterMagentoTodosProdutos(produtoMagConfig.PRODUTO);
            foreach (var p in produtos)
            {

                if (p.COR == produtoMagConfig.COR)
                {
                    p.CAD_MKTPLACE = produtoMagConfig.CAD_MKTPLACE;
                    p.GOOGLE_SHOPPING = produtoMagConfig.GOOGLE_SHOPPING;

                    p.FOTO_FRENTE_CAB = produtoMagConfig.FOTO_FRENTE_CAB;
                    //p.FOTO_FRENTE_CAB_MAG = produtoMagConfig.FOTO_FRENTE_CAB_MAG;
                    p.FOTO_FRENTE_CAB_POS = produtoMagConfig.FOTO_FRENTE_CAB_POS;

                    p.FOTO_FRENTE_SEMCAB = produtoMagConfig.FOTO_FRENTE_SEMCAB;
                    //p.FOTO_FRENTE_SEMCAB_MAG = produtoMagConfig.FOTO_FRENTE_SEMCAB_MAG;
                    p.FOTO_FRENTE_SEMCAB_POS = produtoMagConfig.FOTO_FRENTE_SEMCAB_POS;

                    p.FOTO_COSTAS = produtoMagConfig.FOTO_COSTAS;
                    //p.FOTO_COSTAS_MAG = produtoMagConfig.FOTO_COSTAS_MAG;
                    p.FOTO_COSTAS_POS = produtoMagConfig.FOTO_COSTAS_POS;

                    p.FOTO_DETALHE = produtoMagConfig.FOTO_DETALHE;
                    //p.FOTO_DETALHE_MAG = produtoMagConfig.FOTO_DETALHE_MAG;
                    p.FOTO_DETALHE_POS = produtoMagConfig.FOTO_DETALHE_POS;

                    p.FOTO_LOOK = produtoMagConfig.FOTO_LOOK;
                    //p.FOTO_LOOK_MAG = produtoMagConfig.FOTO_LOOK_MAG;
                    p.FOTO_LOOK_POS = produtoMagConfig.FOTO_LOOK_POS;

                    p.FOTO_LADO = produtoMagConfig.FOTO_LADO;
                    //p.FOTO_LADO_MAG = produtoMagConfig.FOTO_LADO_MAG;
                    p.FOTO_LADO_POS = produtoMagConfig.FOTO_LADO_POS;

                    p.FOTO_VITRINE_POS = produtoMagConfig.FOTO_VITRINE_POS;
                    p.FOTO_HOVER = produtoMagConfig.FOTO_HOVER;
                    //p.FOTO_HOVER_MAG = produtoMagConfig.FOTO_HOVER_MAG;

                    if (p.ID_PRODUTO_MAG_CONFIG > 0)
                        p.NOME_MAG = produtoMagConfig.NOME_MAG + " - " + produtoMagConfig.ECOM_COR1.COR + " - " + produtoMagConfig.TAMANHO;
                }

                p.NOME_MAG = produtoMagConfig.NOME_MAG;
                p.PRODUTO_DESC = produtoMagConfig.PRODUTO_DESC;
                p.PRODUTO_DESC_CURTA = produtoMagConfig.PRODUTO_DESC_CURTA;
                p.PESO_KG = produtoMagConfig.PESO_KG;
                p.GRUPO_PRODUTO = produtoMagConfig.GRUPO_PRODUTO;
                p.ECOM_GRUPO_PRODUTO = produtoMagConfig.ECOM_GRUPO_PRODUTO;
                p.PRECO = produtoMagConfig.PRECO;
                p.PRECO_PROMO = produtoMagConfig.PRECO_PROMO;
                p.PRECO_PROMO_DATA_DE = produtoMagConfig.PRECO_PROMO_DATA_DE;
                p.PRECO_PROMO_DATA_ATE = produtoMagConfig.PRECO_PROMO_DATA_ATE;
                p.ECOM_GRUPO_MACRO = produtoMagConfig.ECOM_GRUPO_MACRO;
                p.ECOM_TIPO_MODELAGEM = produtoMagConfig.ECOM_TIPO_MODELAGEM;
                p.ECOM_TIPO_TECIDO = produtoMagConfig.ECOM_TIPO_TECIDO;
                p.ECOM_TIPO_MANGA = produtoMagConfig.ECOM_TIPO_MANGA;
                p.ECOM_TIPO_GOLA = produtoMagConfig.ECOM_TIPO_GOLA;
                p.ECOM_TIPO_COMPRIMENTO = produtoMagConfig.ECOM_TIPO_COMPRIMENTO;
                p.ECOM_TIPO_ESTILO = produtoMagConfig.ECOM_TIPO_ESTILO;
                p.ECOM_TIPO_LINHA = produtoMagConfig.ECOM_TIPO_LINHA;
                p.ECOM_SIGNED = produtoMagConfig.ECOM_SIGNED;
                p.ECOM_TIPO_RELACIONADO = produtoMagConfig.ECOM_TIPO_RELACIONADO;

                p.ECOM_CAIXA_DIMENSAO = produtoMagConfig.ECOM_CAIXA_DIMENSAO;

                p.STATUS_CADASTRO = 'A';
                if (p.ID_PRODUTO_MAG != null)
                    p.STATUS_CADASTRO = 'B';

                p.DATA_ALTERACAO = produtoMagConfig.DATA_ALTERACAO;
                p.USUARIO_ALTERACAO = produtoMagConfig.USUARIO_ALTERACAO;

                p.GRIFFE = produtoMagConfig.GRIFFE;
                p.COD_CATEGORIA = produtoMagConfig.COD_CATEGORIA;

                eController.AtualizarMagentoProduto(p);
            }


            return true;
        }

        private void CarregarCategorias()
        {

            List<ECOM_PRODUTO_CAT> produtoCATs = new List<ECOM_PRODUTO_CAT>();

            if (hidCodigo.Value != "")
                produtoCATs = eController.ObterProdutoCategoria(Convert.ToInt32(hidCodigo.Value));

            if (Session[PRODCATMAG] != null)
                produtoCATs.AddRange((List<ECOM_PRODUTO_CAT>)Session[PRODCATMAG]);


            gvCategoria.DataSource = produtoCATs;
            gvCategoria.DataBind();

        }
        protected void gvCategoria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_PRODUTO_CAT mom = e.Row.DataItem as ECOM_PRODUTO_CAT;

                    if (mom != null)
                    {
                        Literal litCategoria = e.Row.FindControl("litCategoria") as Literal;
                        litCategoria.Text = mom.ECOM_GRUPO_PRODUTO1.GRUPO;

                        ImageButton btExcluirCategoriaProd = e.Row.FindControl("btExcluirCategoriaProd") as ImageButton;
                        btExcluirCategoriaProd.CommandArgument = mom.ECOM_GRUPO_PRODUTO.ToString();
                    }
                }
            }
        }
        protected void gvCategoria_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvCategoria.FooterRow;
            if (_footer != null)
            {
            }
        }
        protected void btAdicionarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                labErroCategoria.Text = "";

                if (ddlCategoriaMag.SelectedValue == "" || ddlCategoriaMag.SelectedValue == "0")
                {
                    labErroCategoria.Text = "Selecione a categoria...";
                    return;
                }

                if (!ValidarCategoriaDuplicada(ddlCategoriaMag.SelectedItem.Text.Trim()))
                {
                    labErroCategoria.Text = "A categoria selecionada já está inserida neste produto...";
                    return;
                }

                var codigoGrupoProduto = Convert.ToInt32(ddlCategoriaMag.SelectedValue);

                if (hidCodigo.Value != "")
                {
                    //inserir
                    int codigoEcom = Convert.ToInt32(hidCodigo.Value);
                    var produtoCat = new ECOM_PRODUTO_CAT();
                    produtoCat.ECOM_PRODUTO = codigoEcom;
                    produtoCat.ECOM_GRUPO_PRODUTO = codigoGrupoProduto;
                    eController.InserirProdutoCategoria(produtoCat);
                    CarregarCategorias();
                }
                else
                {
                    //PRODCATMAG
                    List<ECOM_PRODUTO_CAT> produtoCATs = new List<ECOM_PRODUTO_CAT>();
                    var prodCAt = new ECOM_PRODUTO_CAT();
                    prodCAt.ECOM_PRODUTO = 0;
                    prodCAt.ECOM_GRUPO_PRODUTO = codigoGrupoProduto;
                    prodCAt.ECOM_GRUPO_PRODUTO1 = eController.ObterMagentoGrupoProduto(codigoGrupoProduto);

                    if (Session[PRODCATMAG] != null)
                        produtoCATs = (List<ECOM_PRODUTO_CAT>)Session[PRODCATMAG];

                    produtoCATs.Add(prodCAt);
                    Session[PRODCATMAG] = produtoCATs;
                }

                CarregarCategorias();

            }
            catch (Exception ex)
            {
                labErroCategoria.Text = ex.Message;
            }
        }
        protected void btExcluirCategoriaProd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErroCategoria.Text = "";

                ImageButton imgBtn = ((ImageButton)sender);
                var codigoGrupoProduto = Convert.ToInt32(imgBtn.CommandArgument);

                int codigoEcom = 0;

                if (hidCodigo.Value != "")
                    codigoEcom = Convert.ToInt32(hidCodigo.Value);

                var prodCat = eController.ObterProdutoCategoria(codigoEcom, codigoGrupoProduto);
                if (prodCat == null)
                {
                    List<ECOM_PRODUTO_CAT> produtoCATs = new List<ECOM_PRODUTO_CAT>();
                    if (Session[PRODCATMAG] != null)
                        produtoCATs = (List<ECOM_PRODUTO_CAT>)Session[PRODCATMAG];

                    int _index = produtoCATs.FindIndex(p => p.ECOM_PRODUTO == 0 && p.ECOM_GRUPO_PRODUTO == codigoGrupoProduto);
                    if (_index >= 0)
                        produtoCATs.RemoveAt(_index);

                    Session[PRODCATMAG] = produtoCATs;
                }
                else
                {

                    //excluir
                    eController.ExcluirProdutoCategoria(codigoEcom, codigoGrupoProduto);
                }

                CarregarCategorias();

            }
            catch (Exception ex)
            {
                labErroCategoria.Text = ex.Message;
            }
        }
        private bool ValidarCategoriaDuplicada(string categoria)
        {
            bool retorno = true;

            foreach (GridViewRow row in gvCategoria.Rows)
            {
                if (row != null)
                {
                    Literal litCategoria = row.FindControl("litCategoria") as Literal;
                    if (litCategoria != null)
                    {
                        if (litCategoria.Text.Trim() == categoria.Trim())
                        {
                            retorno = false;
                            break;
                        }
                    }
                }
            }

            return retorno;
        }

        private void CarregarBlocos(ECOM_PRODUTO produtoMag)
        {
            if (produtoMag.ID_PRODUTO_MAG > 0)
            {
                var blocos = eController.ObterBlocoProdutoOrdemPorCodigoEcomProduto(produtoMag.CODIGO);
                gvBlocos.DataSource = blocos;
                gvBlocos.DataBind();
            }
        }
        protected void gvBlocos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_BLOCO_PRODUTO_ORDEM bloco = e.Row.DataItem as ECOM_BLOCO_PRODUTO_ORDEM;

                    if (bloco != null)
                    {
                        Literal litCategoria = e.Row.FindControl("litCategoria") as Literal;
                        litCategoria.Text = bloco.ECOM_BLOCO_PRODUTO1.ECOM_GRUPO_PRODUTO1.GRUPO;

                        Literal litBloco = e.Row.FindControl("litBloco") as Literal;
                        litBloco.Text = bloco.ECOM_BLOCO_PRODUTO1.BLOCO;

                    }
                }
            }
        }
        protected void gvBlocos_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvBlocos.FooterRow;
            if (_footer != null)
            {
            }
        }

        private void SalvarProdutoMagento()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            string mapPath = Server.MapPath("..");

            Magento mag = new Magento(usuario.CODIGO_USUARIO, mapPath, usuario.USER_MAG_API, usuario.PASS_MAG_API);

            var produtos = eController.ObterMagentoTodosProdutos(hidProduto.Value).Where(x => x.ID_PRODUTO_MAG_CONFIG == 0 && x.ID_PRODUTO_MAG > 0);
            var updateImage = false;
            foreach (var p in produtos)
            {
                if (p.COR == hidCor.Value && cbAtualizarImagem.Checked)
                    updateImage = true;

                mag.AtualizarProdutoMagento(p, updateImage);
                updateImage = false;
            }

        }
        protected void btAtualizarMagento_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!SalvarEValidar())
                {
                    return;
                }

                if (!ValidarFotos(hidProduto.Value.Trim(), hidCor.Value.Trim()))
                {
                    string diretorioFotos = Server.MapPath("~/FotosHandbookOnline") + "\\";
                    labErro.Text = "As Fotos não estão disponíveis no diretório \"" + diretorioFotos + "\".";
                    return;
                }

                //SalvarProdutoConfig();

                SalvarProdutoMagento();

                btAtualizarMagento.Enabled = false;

                labErro.Text = "Produto atualizado com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void txtDescProduto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                txtDescProdutoCurta.Text = txt.Text;

                txtMetaDescricao.Text = txt.Text + Environment.NewLine + "Compre "
                                                        + Utils.WebControls.AlterarPrimeiraLetraMaiscula(txtGrupoProduto.Text.Trim().ToLower()) + " "
                                                        + Utils.WebControls.AlterarPrimeiraLetraMaiscula(hidGriffe.Value.ToLower()) + " "
                                                        + Utils.WebControls.AlterarPrimeiraLetraMaiscula(ddlCorMagento.SelectedItem.Text.ToLower()) + " na Handbook! "
                                                        + Environment.NewLine
                                                        + "A nossa mais nova loja de moda online. Entregas para todo o Brasil. Clique e Confira!";

            }
            catch (Exception)
            {
            }
        }

        private string ObterComposicaoProduto(string produto)
        {
            var composicaoProduto = prodController.ObterComposicaoPorProdutoLinx(produto);
            if (composicaoProduto != null)
                return composicaoProduto.COMPOSICAO;

            return "";
        }
        protected void txtNomeProdutoMagento_TextChanged(object sender, EventArgs e)
        {
            if (txtNomeProdutoMagento.Text.Trim() != "" && ddlCorMagento.SelectedValue != "0")
            {
                txtMetaTitulo.Text = txtNomeProdutoMagento.Text.Trim() + " " + ddlCorMagento.SelectedItem.Text.Trim() + " | Handbook";
            }
        }
        protected void ddlCorMagento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtNomeProdutoMagento.Text.Trim() != "" && ddlCorMagento.SelectedValue != "0")
            {
                txtMetaTitulo.Text = txtNomeProdutoMagento.Text.Trim() + " " + ddlCorMagento.SelectedItem.Text.Trim() + " | Handbook";
            }
        }


    }
}


