using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
using System.Text;
using System.IO;

namespace Relatorios
{
    public partial class ecom_ordem_cad_blocoproduto : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        const string PRODUTO_ECOM_ORDEM_CAR = "PRODUTO_ECOM_ORDEM_CAR";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarCategoriaMag();

                CarregarColecoes();
                CarregarCorLinx();

                Session[PRODUTO_ECOM_ORDEM_CAR] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btBuscarFiltro.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscarFiltro, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarCategoriaMag()
        {
            var catMag = ecomController.ObterMagentoGrupoProdutoAberto();
            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();
        }
        protected void ddlCategoriaMag_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidTipoCategoria.Value = "";
            var codigoGrupoProduto = Convert.ToInt32(ddlCategoriaMag.SelectedValue);

            CarregarBloco(codigoGrupoProduto);

            var ecomGrupoProduto = ecomController.ObterMagentoGrupoProduto(codigoGrupoProduto);
            if (ecomGrupoProduto != null)
                hidTipoCategoria.Value = ecomGrupoProduto.TIPO_CATEGORIA;
        }
        private void CarregarBloco(int ecomGrupoProduto)
        {
            var bloco = ecomController.ObterBlocoPorCategoriaMag(ecomGrupoProduto);

            bloco = bloco.OrderBy(p => p.BLOCO).ToList();
            bloco.Insert(0, new ECOM_BLOCO_PRODUTO { CODIGO = 0, BLOCO = "Selecione" });

            ddlBloco.DataSource = bloco;
            ddlBloco.DataBind();
        }

        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
                }
            }
        }
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarTecido(ddlColecao.SelectedValue.Trim());
            CarregarCorFornecedor(ddlColecao.SelectedValue.Trim());
        }

        private void CarregarGrupoProduto(string grupoMagento)
        {
            var grupos = prodController.ObterGrupoProduto("01");

            if (grupoMagento != "")
            {
                var grupoArr = grupoMagento.Split('/');
                var gp = new List<string>();
                foreach (var g in grupoArr)
                    gp.Add(g);

                grupos = prodController.ObterGrupoProduto("01").Where(p => gp.Contains(p.GRUPO_PRODUTO.Trim())).ToList();
            }

            if (grupos != null)
            {
                grupos.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoProduto.DataSource = grupos;
                ddlGrupoProduto.DataBind();
            }
        }
        private void CarregarGriffe(string griffeMagento)
        {
            var griffes = baseController.BuscaGriffes();

            if (griffeMagento != "")
            {
                var griffeArr = griffeMagento.Split('/');
                var gf = new List<string>();
                foreach (var g in griffeArr)
                    gf.Add(g);

                griffes = baseController.BuscaGriffes().Where(p => gf.Contains(p.GRIFFE.Trim())).ToList();
            }

            if (griffes != null)
            {
                griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffes;
                ddlGriffe.DataBind();
            }
        }
        protected void ddlGrupoGriffe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hidTipoCategoria.Value == "2")
            {
                CarregarTipoModelagem(ddlGriffe.SelectedValue.Trim(), ddlGrupoProduto.SelectedValue.Trim());
                CarregarTipoTecido(ddlGriffe.SelectedValue.Trim(), ddlGrupoProduto.SelectedValue.Trim());
                CarregarTipoManga(ddlGriffe.SelectedValue.Trim(), ddlGrupoProduto.SelectedValue.Trim());
                CarregarTipoGola(ddlGriffe.SelectedValue.Trim(), ddlGrupoProduto.SelectedValue.Trim());
                CarregarTipoComprimento(ddlGriffe.SelectedValue.Trim(), ddlGrupoProduto.SelectedValue.Trim());
                CarregarTipoEstilo(ddlGriffe.SelectedValue.Trim(), ddlGrupoProduto.SelectedValue.Trim());
            }
        }


        private void CarregarCorLinx()
        {
            var corLinx = prodController.ObterCoresBasicas();
            if (corLinx != null)
            {
                corLinx.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
                ddlCorLinx.DataSource = corLinx;
                ddlCorLinx.DataBind();
            }
        }
        private void CarregarTecido(string colecao)
        {
            var tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.TECIDO_POCKET != null && p.STATUS == 'A').Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();
            foreach (var item in _tecido)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor(string colecao)
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');
            var _corFornecedorAux = _corFornecedor.Select(s => s.FORNECEDOR_COR.Trim()).Distinct().ToList();

            foreach (var item in _corFornecedorAux)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR_COR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.FORNECEDOR_COR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
            ddlCorFornecedor.DataSource = corFornecedorFiltro;
            ddlCorFornecedor.DataBind();
        }

        private void CarregarTipoModelagem(string griffe, string grupoProduto)
        {
            ddlTipoModelagem.Enabled = true;
            var tipoModelagemLista = ecomController.ObterMagentoTipoModelagem(griffe, grupoProduto);

            var tipoModelagem = new List<ECOM_TIPO_MODELAGEM>();
            foreach (var t in tipoModelagemLista)
            {
                if (tipoModelagem.Where(p => p.TIPO_MODELAGEM == t.TIPO_MODELAGEM).Count() <= 0)
                    tipoModelagem.Add(t);
            }
            if (tipoModelagem != null && tipoModelagem.Count() > 0)
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = 0, TIPO_MODELAGEM = "" });
            }
            else
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = 0, TIPO_MODELAGEM = "-" });
                ddlTipoModelagem.Enabled = false;
            }

            ddlTipoModelagem.DataSource = tipoModelagem;
            ddlTipoModelagem.DataBind();
        }
        private void CarregarTipoTecido(string griffe, string grupoProduto)
        {
            ddlTipoTecido.Enabled = true;
            var tipoTecidoLista = ecomController.ObterMagentoTipoTecido(griffe, grupoProduto);

            var tipoTecido = new List<ECOM_TIPO_TECIDO>();
            foreach (var t in tipoTecidoLista)
            {
                if (tipoTecido.Where(p => p.TIPO_TECIDO == t.TIPO_TECIDO).Count() <= 0)
                    tipoTecido.Add(t);
            }

            if (tipoTecido != null && tipoTecido.Count() > 0)
            {
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = 0, TIPO_TECIDO = "" });
            }
            else
            {
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = 0, TIPO_TECIDO = "-" });
                ddlTipoTecido.Enabled = false;
            }

            ddlTipoTecido.DataSource = tipoTecido;
            ddlTipoTecido.DataBind();
        }
        private void CarregarTipoManga(string griffe, string grupoProduto)
        {
            ddlTipoManga.Enabled = true;
            var tipoMangaLista = ecomController.ObterMagentoTipoManga(griffe, grupoProduto);

            var tipoManga = new List<ECOM_TIPO_MANGA>();
            foreach (var t in tipoMangaLista)
            {
                if (tipoManga.Where(p => p.TIPO_MANGA == t.TIPO_MANGA).Count() <= 0)
                    tipoManga.Add(t);
            }

            if (tipoManga != null && tipoManga.Count() > 0)
            {
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = 0, TIPO_MANGA = "" });
            }
            else
            {
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = 0, TIPO_MANGA = "-" });
                ddlTipoManga.Enabled = false;
            }

            ddlTipoManga.DataSource = tipoManga;
            ddlTipoManga.DataBind();
        }
        private void CarregarTipoGola(string griffe, string grupoProduto)
        {
            ddlTipoGola.Enabled = true;
            var tipoGolaLista = ecomController.ObterMagentoTipoGola(griffe, grupoProduto);

            var tipoGola = new List<ECOM_TIPO_GOLA>();
            foreach (var t in tipoGolaLista)
            {
                if (tipoGola.Where(p => p.TIPO_GOLA == t.TIPO_GOLA).Count() <= 0)
                    tipoGola.Add(t);
            }

            if (tipoGola != null && tipoGola.Count() > 0)
            {
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = 0, TIPO_GOLA = "" });
            }
            else
            {
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = 0, TIPO_GOLA = "-" });
                ddlTipoGola.Enabled = false;
            }

            ddlTipoGola.DataSource = tipoGola;
            ddlTipoGola.DataBind();
        }
        private void CarregarTipoComprimento(string griffe, string grupoProduto)
        {
            ddlTipoComprimento.Enabled = true;
            var tipoComprimentoLista = ecomController.ObterMagentoTipoComprimento(griffe, grupoProduto);

            var tipoComprimento = new List<ECOM_TIPO_COMPRIMENTO>();
            foreach (var t in tipoComprimentoLista)
            {
                if (tipoComprimento.Where(p => p.TIPO_COMPRIMENTO == t.TIPO_COMPRIMENTO).Count() <= 0)
                    tipoComprimento.Add(t);
            }

            if (tipoComprimento != null && tipoComprimento.Count() > 0)
            {
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = 0, TIPO_COMPRIMENTO = "" });
            }
            else
            {
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = 0, TIPO_COMPRIMENTO = "-" });
                ddlTipoComprimento.Enabled = false;
            }

            ddlTipoComprimento.DataSource = tipoComprimento;
            ddlTipoComprimento.DataBind();
        }
        private void CarregarTipoEstilo(string griffe, string grupoProduto)
        {
            ddlTipoEstilo.Enabled = true;
            var tipoEstiloLista = ecomController.ObterMagentoTipoEstilo(griffe, grupoProduto);

            var tipoEstilo = new List<ECOM_TIPO_ESTILO>();
            foreach (var t in tipoEstiloLista)
            {
                if (tipoEstilo.Where(p => p.TIPO_ESTILO == t.TIPO_ESTILO).Count() <= 0)
                    tipoEstilo.Add(t);
            }

            if (tipoEstilo != null && tipoEstilo.Count() > 0)
            {
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = 0, TIPO_ESTILO = "" });
            }
            else
            {
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = 0, TIPO_ESTILO = "-" });
                ddlTipoEstilo.Enabled = false;
            }

            ddlTipoEstilo.DataSource = tipoEstilo;
            ddlTipoEstilo.DataBind();
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {


            try
            {
                labMsg.Text = "";

                if (ddlCategoriaMag.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione a Categoria Magento.";
                    return;
                }

                if (ddlBloco.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione o Bloco.";
                    return;
                }

                if (hidTipoCategoria.Value == "")
                {
                    labMsg.Text = "Ocorreu um erro ao obter o tipo de categoria. Entre em contato com TI.";
                    return;
                }

                var grupoMagento = ecomController.ObterMagentoGrupoProduto(Convert.ToInt32(ddlCategoriaMag.SelectedValue));

                CarregarGrupoProduto(grupoMagento.GRUPO_PRODUTO);
                CarregarGriffe(grupoMagento.GRIFFE);
                CarregarTipoModelagem(grupoMagento.GRIFFE, grupoMagento.GRUPO_PRODUTO);
                CarregarTipoTecido(grupoMagento.GRIFFE, grupoMagento.GRUPO_PRODUTO);
                CarregarTipoManga(grupoMagento.GRIFFE, grupoMagento.GRUPO_PRODUTO);
                CarregarTipoGola(grupoMagento.GRIFFE, grupoMagento.GRUPO_PRODUTO);
                CarregarTipoComprimento(grupoMagento.GRIFFE, grupoMagento.GRUPO_PRODUTO);
                CarregarTipoEstilo(grupoMagento.GRIFFE, grupoMagento.GRUPO_PRODUTO);


                CarregarCarrinho();

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarCarrinho()
        {
            var carrinho = ecomController.ObterBlocoProdutoOrdemPorCatBloco(Convert.ToInt32(ddlBloco.SelectedValue), Convert.ToInt32(ddlCategoriaMag.SelectedValue));
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_BLOCO_PRODUTO_ORDEM carrinho = e.Row.DataItem as ECOM_BLOCO_PRODUTO_ORDEM;

                    if (carrinho != null)
                    {
                        System.Web.UI.WebControls.Image imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                        imgProduto.ImageUrl = carrinho.ECOM_PRODUTO1.FOTO_FRENTE_CAB;

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.ECOM_PRODUTO1.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.ECOM_PRODUTO1.NOME;

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = prodController.ObterCoresBasicas(carrinho.ECOM_PRODUTO1.COR).DESC_COR;

                        Literal litPrecoTL = e.Row.FindControl("litPrecoTL") as Literal;
                        litPrecoTL.Text = "R$ " + ((carrinho.ECOM_PRODUTO1.PRECO_PROMO == null) ? Convert.ToDecimal(carrinho.ECOM_PRODUTO1.PRECO).ToString("###,###,##0.00") : Convert.ToDecimal(carrinho.ECOM_PRODUTO1.PRECO_PROMO).ToString("###,###,##0.00"));

                        var estoqueProduto = baseController.BuscaEstoqueLoja(carrinho.ECOM_PRODUTO1.PRODUTO, carrinho.ECOM_PRODUTO1.COR, "HANDBOOK ONLINE");
                        Literal litEstoque = e.Row.FindControl("litEstoque") as Literal;
                        litEstoque.Text = (estoqueProduto == null) ? "0" : estoqueProduto.ESTOQUE.ToString();

                        Button _btExcluirCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as Button;
                        if (_btExcluirCarrinho != null)
                            _btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString();

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void btExcluirCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                ecomController.ExcluirBlocoProdutoOrdemPorCatBloco(Convert.ToInt32(ddlBloco.SelectedValue), Convert.ToInt32(ddlCategoriaMag.SelectedValue));
                CarregarCarrinho();
            }
            catch (Exception)
            {
            }
        }
        protected void btExcluirItemCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)(sender);
                int codigoCarrinho = Convert.ToInt32(bt.CommandArgument);
                ecomController.ExcluirBlocoProdutoOrdem(codigoCarrinho);

                CarregarCarrinho();
            }
            catch (Exception)
            {
            }
        }
        protected void btAdicionarCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                labMsg.Text = "";

                Button bt = (Button)(sender);

                GridViewRow row = (GridViewRow)bt.NamingContainer;

                var codigo = Convert.ToInt32(gvProduto.DataKeys[row.RowIndex][0].ToString());
                var idProdutoMag = Convert.ToInt32(gvProduto.DataKeys[row.RowIndex][1].ToString());

                var produtoNoCarrinho = ecomController.ObterBlocoProdutoOrdemPorIdMagProduto(Convert.ToInt32(ddlBloco.SelectedValue), idProdutoMag);
                if (produtoNoCarrinho != null)
                {
                    labMsg.Text = "Produto já está relacionado a este Bloco.";
                    return;
                }

                //var produtoEmBloco = ecomController.ObterBlocoProdutoOrdemPorCodigoEcomProduto(codigo, hidTipoCategoria.Value);
                //if (produtoEmBloco != null && produtoEmBloco.Count() > 0)
                //{
                //    labMsg.Text = "Produto já possui Bloco.";
                //    return;
                //}

                var carrinho = new ECOM_BLOCO_PRODUTO_ORDEM
                {
                    ECOM_BLOCO_PRODUTO = Convert.ToInt32(ddlBloco.SelectedValue),
                    ECOM_PRODUTO = codigo,
                    ORDEM = 999,
                    DATA_INCLUSAO = DateTime.Now,
                    TIPO_CATEGORIA = hidTipoCategoria.Value
                };
                ecomController.InserirBlocoProdutoOrdem(carrinho);

                bt.Enabled = false;

                CarregarCarrinho();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region "PRODUTO"

        protected void btBuscarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                if (hidTipoCategoria.Value == "")
                {
                    labMsg.Text = "Ocorreu um erro ao obter o tipo de categoria. Entre em contato com TI.";
                    return;
                }

                CarregarProduto();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }
        private List<SP_OBTER_ECOM_PRODUTO_CATMAGENTOResult> ObterProdutoPorCategoriaMagParaOrdem()
        {
            int ecomGrupoProduto = Convert.ToInt32(ddlCategoriaMag.SelectedValue);
            string colecao = ddlColecao.SelectedValue.Trim();
            string produto = txtProduto.Text.Trim();
            string cor = ddlCorLinx.SelectedValue.Trim();
            string tecido = ddlTecido.SelectedValue.Trim();
            string corFornecedor = ddlCorFornecedor.SelectedValue.Trim();
            int tipoModelagem = Convert.ToInt32(ddlTipoModelagem.SelectedValue);
            int tipoTecido = Convert.ToInt32(ddlTipoTecido.SelectedValue);
            int tipoManga = Convert.ToInt32(ddlTipoManga.SelectedValue);
            int tipoGola = Convert.ToInt32(ddlTipoGola.SelectedValue);
            int tipoComprimento = Convert.ToInt32(ddlTipoComprimento.SelectedValue);
            int tipoEstilo = Convert.ToInt32(ddlTipoEstilo.SelectedValue);
            string tipoCategoria = hidTipoCategoria.Value;
            string grupoProduto = (ddlGrupoProduto.SelectedValue != null) ? ddlGrupoProduto.SelectedValue.Trim() : "";
            string griffe = (ddlGriffe.SelectedValue != null) ? ddlGriffe.SelectedValue.Trim() : "";

            var produtos = ecomController.ObterProdutoPorCategoriaMagParaOrdem(ecomGrupoProduto, colecao, produto, cor, tecido, corFornecedor, tipoModelagem, tipoTecido, tipoManga, tipoGola, tipoComprimento, tipoEstilo, tipoCategoria, grupoProduto, griffe);

            if (ddlBlocoSelec.SelectedValue != "")
            {
                var produtosDaCategoria = ecomController.ObterBlocoProdutoOrdemPorCodigoEcomGrupoProduto(ecomGrupoProduto).Select(x => x.ECOM_PRODUTO);
                if (ddlBlocoSelec.SelectedValue == "S")
                    produtos = produtos.Where(p => produtosDaCategoria.Contains(p.CODIGO)).ToList();
                else
                    produtos = produtos.Where(p => !produtosDaCategoria.Contains(p.CODIGO)).ToList();
            }


            return produtos;
        }
        private void CarregarProduto()
        {
            var produtos = ObterProdutoPorCategoriaMagParaOrdem();

            gvProduto.DataSource = produtos;
            gvProduto.DataBind();

            Session[PRODUTO_ECOM_ORDEM_CAR] = produtos;
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_CATMAGENTOResult produto = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_CATMAGENTOResult;

                    if (produto != null)
                    {
                        System.Web.UI.WebControls.Image imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                        imgProduto.ImageUrl = produto.FOTO_FRENTE_CAB;

                        var produtoNoCarrinho = ecomController.ObterBlocoProdutoOrdemPorIdMagProduto(Convert.ToInt32(ddlBloco.SelectedValue), Convert.ToInt32(produto.ID_PRODUTO_MAG));
                        if (produtoNoCarrinho != null)
                            e.Row.BackColor = Color.Wheat;

                        var produtoNaCategoria = ecomController.ObterBlocoProdutoOrdemPorCodigoeEcomGrupoProduto(Convert.ToInt32(ddlCategoriaMag.SelectedValue), produto.CODIGO);
                        if (produtoNaCategoria != null)
                            e.Row.BackColor = Color.Wheat;

                        Literal litPrecoTL = e.Row.FindControl("litPrecoTL") as Literal;
                        litPrecoTL.Text = "R$ " + ((produto.PRECO_PROMO == null) ? Convert.ToDecimal(produto.PRECO).ToString("###,###,##0.00") : Convert.ToDecimal(produto.PRECO_PROMO).ToString("###,###,##0.00"));

                        Literal litBlocos = e.Row.FindControl("litBlocos") as Literal;
                        litBlocos.Text = FormatarTextoBloco(produto.BLOCOS);

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PRODUTO_ECOM_ORDEM_CAR] != null)
            {
                IEnumerable<SP_OBTER_ECOM_PRODUTO_CATMAGENTOResult> produto = (IEnumerable<SP_OBTER_ECOM_PRODUTO_CATMAGENTOResult>)Session[PRODUTO_ECOM_ORDEM_CAR];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvProduto, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvProduto, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvProduto, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                produto = produto.OrderBy(e.SortExpression + sortDirection);
                gvProduto.DataSource = produto;
                gvProduto.DataBind();
            }
        }

        private string FormatarTextoBloco(string campo)
        {
            return campo.Replace("#@@@", "<font size='2' color=''>").Replace("@@@%", "</font><br /><br />");
        }
        #endregion




    }
}

