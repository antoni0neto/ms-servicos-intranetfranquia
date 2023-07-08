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
    public partial class desenv_categoria_ordem_produto : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string PRODUTO_ANALISE_CAT = "PRODUTO_ANALISE_CAT";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGriffe();

                Session[PRODUTO_ANALISE_CAT] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btBuscarFiltro.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscarFiltro, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();
        }
        private void CarregarGriffe()
        {
            var griffes = baseController.BuscaGriffes();
            griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "Selecione" });

            ddlGriffe.DataSource = griffes;
            ddlGriffe.DataBind();
        }
        private void CarregarGrupo()
        {
            var grupoProduto = prodController.ObterGrupoProduto("");

            grupoProduto.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
            ddlGrupoProduto.DataSource = grupoProduto;
            ddlGrupoProduto.DataBind();
        }
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCategorias(ddlColecao.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim());
        }
        protected void ddlGriffe_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCategorias(ddlColecao.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim());
        }
        private void CarregarCategorias(string colecao, string griffe)
        {
            var categorias = desenvController.ObterCategoria(colecao, griffe).OrderBy(p => p.NOME).ToList();
            categorias.Insert(0, new DESENV_CATEGORIA { CODIGO = 0, NOME = "Selecione" });
            ddlCategoria.DataSource = categorias;
            ddlCategoria.DataBind();
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
        private void CarregarTecido(string colecao, string griffe)
        {
            var tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.GRIFFE.Trim() == griffe.Trim() && p.TECIDO_POCKET != null && p.STATUS == 'A').Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();
            foreach (var item in _tecido)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor(string colecao, string griffe)
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(colecao.Trim()).Where(p => p.GRIFFE.Trim() == griffe.Trim() && p.FORNECEDOR_COR != null && p.STATUS == 'A');
            var _corFornecedorAux = _corFornecedor.Select(s => s.FORNECEDOR_COR.Trim()).Distinct().ToList();

            foreach (var item in _corFornecedorAux)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR_COR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.FORNECEDOR_COR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
            ddlCorFornecedor.DataSource = corFornecedorFiltro;
            ddlCorFornecedor.DataBind();
        }

        #endregion

        #region "CARRINHO"
        protected void btBuscar_Click(object sender, EventArgs e)
        {


            try
            {
                labMsg.Text = "";

                if (ddlColecao.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Coleção.";
                    return;
                }

                if (ddlGriffe.SelectedValue == "Selecione")
                {
                    labMsg.Text = "Selecione a Griffe.";
                    return;
                }

                if (ddlCategoria.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione a Categoria.";
                    return;
                }

                //carregar filtros
                CarregarGrupo();
                CarregarCorLinx();
                CarregarTecido(ddlColecao.SelectedValue, ddlGriffe.SelectedValue);
                CarregarCorFornecedor(ddlColecao.SelectedValue, ddlGriffe.SelectedValue);


                CarregarCarrinho();

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }
        private void CarregarCarrinho()
        {
            var carrinho = desenvController.ObterCarrinhoProdutoAnalise(ddlColecao.SelectedValue, ddlGriffe.SelectedValue, Convert.ToInt32(ddlCategoria.SelectedValue));
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DESENV_ANALISEPRODUTOResult carrinho = e.Row.DataItem as SP_OBTER_DESENV_ANALISEPRODUTOResult;

                    if (carrinho != null)
                    {
                        System.Web.UI.WebControls.Image imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                        if (File.Exists(Server.MapPath(carrinho.FOTO1)))
                            imgProduto.ImageUrl = carrinho.FOTO1;
                        else if (File.Exists(Server.MapPath(carrinho.FOTO2)))
                            imgProduto.ImageUrl = carrinho.FOTO2;
                        else
                            imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.DESC_PRODUTO;

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = carrinho.DESC_COR;

                        Literal litQtdeVarejo = e.Row.FindControl("litQtdeVarejo") as Literal;
                        litQtdeVarejo.Text = carrinho.QTDE_VAREJO.ToString();

                        Literal litPreco = e.Row.FindControl("litPreco") as Literal;
                        litPreco.Text = "R$ " + Convert.ToDecimal(carrinho.PRECO).ToString("###,###,##0.00");

                        Button btExcluirCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as Button;
                        if (btExcluirCarrinho != null)
                            btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString();

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
                desenvController.ExcluirProdutoCategoriaPorCategoria(ddlColecao.SelectedValue, ddlGriffe.SelectedValue, Convert.ToInt32(ddlCategoria.SelectedValue));
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
                desenvController.ExcluirProdutoCategoria(codigoCarrinho);

                CarregarCarrinho();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region "PRODUTO"

        protected void btBuscarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarProduto();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }
        private List<SP_OBTER_DESENV_ANALISEPRODUTO_FILTROResult> ObterProduto()
        {
            var analiseFiltro = desenvController.ObterCarrinhoProdutoAnaliseFiltro(ddlColecao.SelectedValue, ddlGriffe.SelectedValue, ddlGrupoProduto.SelectedValue, txtProduto.Text, ddlTecido.SelectedValue, ddlCorLinx.SelectedValue, ddlCorFornecedor.SelectedValue);

            return analiseFiltro;
        }
        private void CarregarProduto()
        {
            var produtos = ObterProduto();

            gvProduto.DataSource = produtos;
            gvProduto.DataBind();

            Session[PRODUTO_ANALISE_CAT] = produtos;
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DESENV_ANALISEPRODUTO_FILTROResult produto = e.Row.DataItem as SP_OBTER_DESENV_ANALISEPRODUTO_FILTROResult;

                    if (produto != null)
                    {
                        System.Web.UI.WebControls.Image imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                        if (File.Exists(Server.MapPath(produto.FOTO1)))
                            imgProduto.ImageUrl = produto.FOTO1;
                        else if (File.Exists(Server.MapPath(produto.FOTO2)))
                            imgProduto.ImageUrl = produto.FOTO2;
                        else
                            imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                        var produtoNoCarrinho = desenvController.ObterProdutoCategoriaNoCarrinho(ddlColecao.SelectedValue, ddlGriffe.SelectedValue, Convert.ToInt32(ddlCategoria.SelectedValue), Convert.ToInt32(produto.CODIGO));
                        if (produtoNoCarrinho != null)
                            e.Row.BackColor = Color.Wheat;

                        Literal litPreco = e.Row.FindControl("litPreco") as Literal;
                        litPreco.Text = "R$ " + Convert.ToDecimal(produto.PRECO).ToString("###,###,##0.00");

                        Literal litCategorias = e.Row.FindControl("litCategorias") as Literal;
                        litCategorias.Text = FormatarTextoBloco(produto.CATEGORIAS);

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
            if (Session[PRODUTO_ANALISE_CAT] != null)
            {
                IEnumerable<SP_OBTER_DESENV_ANALISEPRODUTO_FILTROResult> produto = (IEnumerable<SP_OBTER_DESENV_ANALISEPRODUTO_FILTROResult>)Session[PRODUTO_ANALISE_CAT];

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

                var codigoProduto = Convert.ToInt32(gvProduto.DataKeys[row.RowIndex][0].ToString());

                var produtoNoCarrinho = desenvController.ObterProdutoCategoriaNoCarrinho(ddlColecao.SelectedValue, ddlGriffe.SelectedValue, Convert.ToInt32(ddlCategoria.SelectedValue), codigoProduto);
                if (produtoNoCarrinho != null)
                {
                    labMsg.Text = "Produto já está relacionado a esta Categoria.";
                    return;
                }

                var carrinho = new DESENV_PRODUTO_CATEGORIA
                {
                    DESENV_CATEGORIA = Convert.ToInt32(ddlCategoria.SelectedValue),
                    DESENV_PRODUTO = codigoProduto,
                    ORDEM = 999,
                    DATA_INCLUSAO = DateTime.Now
                };
                desenvController.InserirProdutoCategoria(carrinho);

                bt.Enabled = false;

                CarregarCarrinho();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string FormatarTextoBloco(string campo)
        {
            return campo.Replace("#@@@", "<font size='2' color=''>").Replace("@@@%", "</font><br /><br />");
        }

        #endregion

    }
}

