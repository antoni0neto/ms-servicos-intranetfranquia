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
    public partial class pacab_pedido_produto_linxv2 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string PROD_ACAB_PEDCOMPRA_PROD = "PROD_ACAB_PEDCOMPRA_PROD";

        int qtdeIntra = 0;
        int qtdeLinx = 0;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();

                CarregarFabricante();
                CarregarFilial();
                CarregarGriffe();

                CarregarCarrinhoLinx();

                Session[PROD_ACAB_PEDCOMPRA_PROD] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
            }

        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                    ddlColecoes_SelectedIndexChanged(null, null);
                }
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (prodController.ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        private void CarregarFabricante()
        {
            var _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            _fornecedores.Where(p => ((p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S')).GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFabricante.DataSource = _fornecedores;
                ddlFabricante.DataBind();
            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "000029", FILIAL = "CD - LUGZY               " });
                filial.Insert(2, new FILIAI { COD_FILIAL = "000041", FILIAL = "CD MOSTRUARIO            " });
                filial.Insert(3, new FILIAI { COD_FILIAL = "1029", FILIAL = "ATACADO HANDBOOK         " });
                filial.Insert(4, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE          " });

                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }
        private void CarregarGriffe()
        {
            var griffe = (baseController.BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var codigoProduto = txtProduto.Text.Trim();

                var produto = baseController.BuscaProduto(codigoProduto);
                if (produto != null)
                {
                    var produtoCores = baseController.BuscaProdutoCores(produto.PRODUTO1);
                    if (produtoCores != null && produtoCores.Count() > 0)
                    {
                        produtoCores.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "", DESC_COR_PRODUTO = "" });

                        ddlCor.DataSource = produtoCores;
                        ddlCor.DataBind();
                    }
                    else
                    {
                        ddlCor.DataSource = new List<PRODUTO_CORE>();
                        ddlCor.DataBind();
                    }
                }
                else
                {
                    ddlCor.DataSource = new List<PRODUTO_CORE>();
                    ddlCor.DataBind();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region "PRODUTO"
        private void CarregarProdutoAcabado()
        {
            var produtoAcabado = ObterProdutoAcabadoProdutoPrePedido();

            gvProdutoAcabado.DataSource = produtoAcabado;
            gvProdutoAcabado.DataBind();

            Session[PROD_ACAB_PEDCOMPRA_PROD] = produtoAcabado;
        }
        protected void gvProdutoAcabado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADO_PRODUTO_PREPEDIDOResult produtoAcabado = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADO_PRODUTO_PREPEDIDOResult;

                    if (produtoAcabado != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(produtoAcabado.COLECAO)).DESC_COLECAO;

                        ImageButton btImprimir = e.Row.FindControl("btImprimir") as ImageButton;
                        btImprimir.CommandArgument = produtoAcabado.COD_DESENV_PRODUTO.ToString() + "|" + produtoAcabado.PEDIDO_INTRA.Trim();

                        var produtoNoCarrinho = desenvController.ObterCarrinhoLinxProduto(produtoAcabado.PRODUTO, produtoAcabado.COR, ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);
                        if (produtoNoCarrinho != null)
                            e.Row.BackColor = Color.PaleGreen;

                        Literal litDataPrevEntrega = e.Row.FindControl("litDataPrevEntrega") as Literal;
                        litDataPrevEntrega.Text = Convert.ToDateTime(produtoAcabado.LIMITE_ENTREGA).ToString("dd/MM/yyyy");

                        ImageButton imgExc = e.Row.FindControl("imgExc") as ImageButton;
                        imgExc.CommandArgument = produtoAcabado.PEDIDO_INTRA.Trim() + "|" + produtoAcabado.PRODUTO.Trim() + "|" + produtoAcabado.COR.Trim() + "|" + produtoAcabado.ENTREGA.ToString("yyyy-MM-dd");

                        //Popular GRID VIEW FILHO
                        if (produtoAcabado.FOTO1 != null && produtoAcabado.FOTO1.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { FOTO = produtoAcabado.FOTO1, FOTO2 = produtoAcabado.FOTO2 });
                                gvFoto.DataSource = _fotoProduto;
                                gvFoto.DataBind();
                            }
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }

                        qtdeIntra += produtoAcabado.QTDE_INTRA;
                        qtdeLinx += produtoAcabado.QTDE_LINX;

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvProdutoAcabado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProdutoAcabado.FooterRow;
            if (footer != null)
            {
                footer.Cells[3].Text = "Total";

                footer.Cells[10].Text = qtdeIntra.ToString();
                footer.Cells[12].Text = qtdeLinx.ToString();

            }
        }
        protected void gvProdutoAcabado_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PROD_ACAB_PEDCOMPRA_PROD] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTO_PREPEDIDOResult> produtoAcabado = (IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTO_PREPEDIDOResult>)Session[PROD_ACAB_PEDCOMPRA_PROD];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvProdutoAcabado, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoAcabado, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoAcabado, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                produtoAcabado = produtoAcabado.OrderBy(e.SortExpression + sortDirection);
                gvProdutoAcabado.DataSource = produtoAcabado;
                gvProdutoAcabado.DataBind();
            }
        }
        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO;

                    }
                }
            }
        }

        #endregion

        private List<SP_OBTER_PRODUTO_ACABADO_PRODUTO_PREPEDIDOResult> ObterProdutoAcabadoProdutoPrePedido()
        {
            var desenvOrigem = 0;
            if (ddlOrigem.SelectedValue != "0")
                desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

            var produtoAcabado = desenvController.ObterProdutoAcabadoProdutoPrePedido(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim(), ddlFilial.SelectedItem.Text.Trim(), ddlFabricante.SelectedValue.Trim());

            if (ddlGriffe.SelectedValue.Trim() != "")
                produtoAcabado = produtoAcabado.Where(p => p.GRIFFE.Trim() == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlCor.SelectedValue != "")
                produtoAcabado = produtoAcabado.Where(p => p.COR.Trim() == ddlCor.SelectedValue.Trim()).ToList();

            if (txtPedidoIntra.Text.Trim() != "")
                produtoAcabado = produtoAcabado.Where(p => p.PEDIDO_INTRA.Trim().ToLower() == txtPedidoIntra.Text.Trim().ToLower()).ToList();

            if (ddlPedidoAberto.SelectedValue != "")
            {
                if (ddlPedidoAberto.SelectedValue == "S")
                    produtoAcabado = produtoAcabado.Where(p => p.PEDIDO_LINX == null || p.PEDIDO_LINX.Trim() == "").ToList();
                else
                    produtoAcabado = produtoAcabado.Where(p => p.PEDIDO_LINX != null && p.PEDIDO_LINX.Trim() != "").ToList();
            }

            return produtoAcabado;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                if (ddlFilial.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Filial.";
                    return;
                }

                if (ddlColecoes.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Coleção.";
                    return;
                }

                CarregarProdutoAcabado();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarCarrinhoLinx()
        {
            var carrinho = desenvController.ObterCarrinhoLinxProdutoPorUsuario(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO_CARRINHO_LINX carrinho = e.Row.DataItem as DESENV_PRODUTO_CARRINHO_LINX;

                    if (carrinho != null)
                    {

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = baseController.BuscaProduto(carrinho.PRODUTO).DESC_PRODUTO.Trim();

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = prodController.ObterCoresBasicas(carrinho.COR_PRODUTO).DESC_COR;


                        var produto = desenvController.ObterProdutoCor(carrinho.PRODUTO, carrinho.COR_PRODUTO).FirstOrDefault();

                        Literal litFornecedor = e.Row.FindControl("litFornecedor") as Literal;
                        litFornecedor.Text = produto.FORNECEDOR;

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = (produto.CUSTO == null) ? "" : ("R$ " + produto.CUSTO.ToString());

                        TextBox txtCustoNota = e.Row.FindControl("txtCustoNota") as TextBox;
                        txtCustoNota.Text = (produto.CUSTO_NOTA == null) ? "" : (produto.CUSTO_NOTA.ToString());

                        var pPrePedido = desenvController.ObterComprasProdutoPrePedido(carrinho.PEDIDO, carrinho.PRODUTO, carrinho.COR_PRODUTO, carrinho.ENTREGA);
                        Literal litGradeTotal = e.Row.FindControl("litGradeTotal") as Literal;
                        litGradeTotal.Text = pPrePedido.QTDE_ORIGINAL.ToString();

                        Literal litNF = e.Row.FindControl("litNF") as Literal;
                        litNF.Text = pPrePedido.NOTA_FISCAL;

                        Button _btAdicionarGrade = e.Row.FindControl("btAdicionarGrade") as Button;
                        if (_btAdicionarGrade != null)
                            _btAdicionarGrade.CommandArgument = carrinho.CODIGO.ToString();

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
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                desenvController.ExcluirCarrinhoLinxProdutoPorUsuario(codigoUsuario);
                CarregarCarrinhoLinx();
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
                desenvController.ExcluirCarrinhoLinxProdutoPorCodigo(codigoCarrinho);

                CarregarCarrinhoLinx();
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

                labPedido.Text = "";
                labMsg.Text = "";

                Button bt = (Button)(sender);

                GridViewRow row = (GridViewRow)bt.NamingContainer;

                string pedido = gvProdutoAcabado.DataKeys[row.RowIndex][0].ToString();
                string produto = gvProdutoAcabado.DataKeys[row.RowIndex][1].ToString();
                string cor = gvProdutoAcabado.DataKeys[row.RowIndex][2].ToString();
                DateTime entrega = Convert.ToDateTime(gvProdutoAcabado.DataKeys[row.RowIndex][3].ToString());

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var produtoNoCarrinho = desenvController.ObterCarrinhoLinxProduto(produto, cor, codigoUsuario);
                if (produtoNoCarrinho != null)
                {
                    labPedido.Text = "Produto já foi adicionado.";
                    return;
                }

                var produtoTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                if (produtoTemPedido != null && produtoTemPedido.PEDIDO_LINX != null)
                {
                    labPedido.Text = "Produto já possui pedido gerado no LINX: " + produtoTemPedido.PEDIDO_LINX;
                    return;
                }

                var carrinho = new DESENV_PRODUTO_CARRINHO_LINX
                {
                    PEDIDO = pedido,
                    PRODUTO = produto,
                    COR_PRODUTO = cor,
                    ENTREGA = entrega,
                    USUARIO = codigoUsuario,
                };
                desenvController.InserirCarrinhoLinxProduto(carrinho);

                row.BackColor = Color.PaleGreen;
                row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");



                CarregarCarrinhoLinx();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void btAdicionarGrade_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    Button bt = (Button)(sender);
                    int codigoCarrinho = Convert.ToInt32(bt.CommandArgument);

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_grade_linxv2.aspx?c=" + codigoCarrinho.ToString() + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        protected void btGerarPedido_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    if (gvCarrinho.Rows.Count > 0)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_linx_gerarv2.aspx');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        protected void btAtualizarCarrinho_Click(object sender, EventArgs e)
        {
            CarregarCarrinhoLinx();
        }

        protected void btAdicionarTodos_Click(object sender, EventArgs e)
        {
            if (Session["USUARIO"] == null)
            {
                Response.Redirect("~/login.aspx");
                return;
            }

            if (ddlFilial.SelectedValue == "")
            {
                labMsg.Text = "Selecione a Filial.";
                return;
            }

            if (ddlColecoes.SelectedValue == "")
            {
                labMsg.Text = "Selecione a Coleção.";
                return;
            }

            labPedido.Text = "";
            labMsg.Text = "";

            var produtos = ObterProdutoAcabadoProdutoPrePedido();
            foreach (var p in produtos)
            {
                string pedido = p.PEDIDO_INTRA;
                string produto = p.PRODUTO;
                string cor = p.COR;
                DateTime entrega = Convert.ToDateTime(p.ENTREGA);

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var produtoNoCarrinho = desenvController.ObterCarrinhoLinxProduto(produto, cor, codigoUsuario);
                if (produtoNoCarrinho != null)
                {
                    //labPedido.Text = "Produto já foi adicionado.";
                    continue;
                }

                var produtoTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                if (produtoTemPedido != null && produtoTemPedido.PEDIDO_LINX != null)
                {
                    //labPedido.Text = "Produto já possui pedido gerado no LINX: " + acessorioTemPedido.PEDIDO_LINX;
                    continue;
                }

                var carrinho = new DESENV_PRODUTO_CARRINHO_LINX
                {
                    PEDIDO = pedido,
                    PRODUTO = produto,
                    COR_PRODUTO = cor,
                    ENTREGA = entrega,
                    USUARIO = codigoUsuario,
                };
                desenvController.InserirCarrinhoLinxProduto(carrinho);
            }

            CarregarCarrinhoLinx();
        }

        protected void txtCustoNota_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            Literal litProduto = row.FindControl("litProduto") as Literal;
            var produtos = desenvController.ObterProdutoPorProduto(litProduto.Text.Trim());
            foreach (var p in produtos)
            {
                var pp = desenvController.ObterProduto(p.CODIGO);
                pp.CUSTO_NOTA = Convert.ToDecimal(txt.Text.Trim());
                desenvController.AtualizarProduto(pp);
            }

            CarregarCarrinhoLinx();

        }

        protected void btImprimir_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";

            if (b != null)
            {
                try
                {
                    var val = b.CommandArgument.Split('|');

                    string desenvProduto = val[0].ToString();
                    string pedidoIntra = val[1].ToString();
                    GerarRelatorio(Convert.ToInt32(desenvProduto), pedidoIntra);

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        #region "RELATORIO"
        private void GerarRelatorio(int codigoProduto, string pedidoIntra)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PROD_ACAB_" + codigoProduto.ToString() + "_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(codigoProduto, pedidoIntra));
                wr.Flush();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Produto Acabado - Ficha</title>   ");
            _texto.Append("         <meta charset='UTF-8'>          ");
            _texto.Append("         <style type='text/css'>");
            _texto.Append("             @media print");
            _texto.Append("             {");
            _texto.Append("                 .tdback");
            _texto.Append("                 {");
            _texto.Append("                     background-color: WindowFrame !important;");
            _texto.Append("                     -webkit-print-color-adjust: exact;");
            _texto.Append("                 }");
            _texto.Append("             }");
            _texto.Append("         </style>");
            _texto.Append("     </head>");
            _texto.Append("");
            _texto.Append("<body onLoad='window.print();'>");


            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }
        private StringBuilder MontarRelatorioHTML(int codigoProduto, string pedidoIntra)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarLogisticaProdutoAcabado(_texto, codigoProduto, pedidoIntra);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        public StringBuilder MontarLogisticaProdutoAcabado(StringBuilder _texto, int codigoProduto, string pedidoIntra)
        {
            ProducaoController prodController = new ProducaoController();
            BaseController baseController = new BaseController();

            int contLinha = 0;
            int tLinhaVarejo = 6;
            int tLinhaAtacado = 5;
            int tLinhaControleQ = 8;

            var desenvProduto = desenvController.ObterProduto(codigoProduto);

            string pcVarejo = "-";
            string pcAtacado = "-";
            string pcMostruario = "-";

            string dataPedidoVarejo = "-";
            string dataPedidoAtacado = "-";
            string dataPedidoMostruario = "-";

            string dataEntregaVarejo = "-";
            string dataEntregaAtacado = "-";
            string dataEntregaMostruario = "-";

            string custo = "";
            string valorTotal = "";
            string fornecedor = desenvProduto.FORNECEDOR.Trim();

            int qtdeTotal = 0;

            SP_OBTER_PRODUTO_COMPRA_INTRANET_TAMResult atacadoQtde = null;
            SP_OBTER_PRODUTO_COMPRA_INTRANET_TAMResult varejoQtde = null;
            SP_OBTER_PRODUTO_COMPRA_INTRANET_TAMResult mostruarioQtde = null;

            char mostruario = (ddlFilial.SelectedItem.Text.Trim().ToLower().Contains("mostruario")) ? 'S' : 'N';

            var pedidoLinx = desenvController.ObterProdutoAcabadoIntranet(pedidoIntra, desenvProduto.MODELO, desenvProduto.COR, DateTime.Now, mostruario);
            if (pedidoLinx != null && pedidoLinx.Count() > 0)
            {
                var m = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("CD MOSTRUARIO")).LastOrDefault();
                if (m != null)
                {
                    pcMostruario = m.PEDIDO;
                    dataPedidoMostruario = m.EMISSAO.ToString("dd/MM/yyyy");
                    dataEntregaMostruario = (m.LIMITE_ENTREGA == null) ? "-" : Convert.ToDateTime(m.LIMITE_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (m.CUSTO1 == null) ? "-" : Convert.ToDecimal(m.CUSTO1).ToString("###,###,##0.00");
                    fornecedor = m.FORNECEDOR;
                }

                var a = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("ATACADO HANDBOOK")).LastOrDefault();
                if (a != null)
                {
                    pcAtacado = a.PEDIDO;
                    dataPedidoAtacado = a.EMISSAO.ToString("dd/MM/yyyy");
                    dataEntregaAtacado = (a.LIMITE_ENTREGA == null) ? "-" : Convert.ToDateTime(a.LIMITE_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (a.CUSTO1 == null) ? "-" : Convert.ToDecimal(a.CUSTO1).ToString("###,###,##0.00");
                    fornecedor = a.FORNECEDOR;
                }

                var v = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("CD - LUGZY")).LastOrDefault();
                if (v != null)
                {
                    pcVarejo = v.PEDIDO;
                    dataPedidoVarejo = v.EMISSAO.ToString("dd/MM/yyyy");
                    dataEntregaVarejo = (v.LIMITE_ENTREGA == null) ? "-" : Convert.ToDateTime(v.LIMITE_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (v.CUSTO1 == null) ? "-" : Convert.ToDecimal(v.CUSTO1).ToString("###,###,##0.00");
                    fornecedor = v.FORNECEDOR;
                }

                atacadoQtde = desenvController.ObterProdutoAcabadoIntranetTamanho(pcAtacado, desenvProduto.MODELO, desenvProduto.COR, DateTime.Now);
                varejoQtde = desenvController.ObterProdutoAcabadoIntranetTamanho(pcVarejo, desenvProduto.MODELO, desenvProduto.COR, DateTime.Now);
                mostruarioQtde = desenvController.ObterProdutoAcabadoIntranetTamanho(pcMostruario, desenvProduto.MODELO, desenvProduto.COR, DateTime.Now);

                qtdeTotal = Convert.ToInt32(((atacadoQtde != null) ? atacadoQtde.QTDE_ORIGINAL : 0) + ((varejoQtde != null) ? varejoQtde.QTDE_ORIGINAL : 0) + ((mostruarioQtde != null) ? mostruarioQtde.QTDE_ORIGINAL : 0));

                valorTotal = "R$ " + Convert.ToDecimal((((((atacadoQtde != null) ? atacadoQtde.QTDE_ORIGINAL : 0) + ((varejoQtde != null) ? varejoQtde.QTDE_ORIGINAL : 0) + ((mostruarioQtde != null) ? mostruarioQtde.QTDE_ORIGINAL : 0)))) * Convert.ToDecimal(((custo == "-") ? "0" : custo))).ToString("###,###,###,##0.00");

            }

            _texto.AppendLine(" <br />");
            _texto.AppendLine("    <span>LOGÍSTICA - PRODUTO ACABADO</span>");
            _texto.AppendLine("<div id='divLog' align='center'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 20px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='text-align: left; border-top: 2px solid #000; border-bottom: none;'>");
            _texto.AppendLine("                            <td rowspan='7' style='border: 1px solid #000;'>");
            _texto.AppendLine("                                <img alt='Foto Peça' width='90' height='120' src='..\\.." + desenvProduto.FOTO.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 184px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;" + ((mostruario == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 200px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;COLEÇÃO: " + baseController.BuscaColecaoAtual(desenvProduto.COLECAO).DESC_COLECAO.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                &nbsp;PRODUTO ACABADO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;QTDE: " + qtdeTotal.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;NOME: " + desenvProduto.GRUPO + " " + desenvProduto.DESC_MODELO.Replace(desenvProduto.GRUPO, ""));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                &nbsp;GRIFFE: " + desenvProduto.GRIFFE);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;PRODUTO: " + desenvProduto.MODELO);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;COR: " + desenvProduto.COR + " - " + prodController.ObterCoresBasicas(desenvProduto.COR).DESC_COR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;CUSTO: " + custo);
            _texto.AppendLine("                            </td>");

            decimal preco = 0;
            var precoTO = baseController.BuscaPrecoOriginalProduto(desenvProduto.MODELO.Trim());
            if (precoTO != null && precoTO.PRECO1 > 0)
                preco = Convert.ToDecimal(precoTO.PRECO1);
            else
            {
                var precoTL = baseController.BuscaPrecoLojaProduto(desenvProduto.MODELO.Trim());
                if (precoTL != null && precoTL.PRECO1 > 0)
                    preco = Convert.ToDecimal(precoTL.PRECO1);
            }


            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                 &nbsp;VENDA: R$ " + preco.ToString("###,###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;FORNECEDOR: " + desenvProduto.FORNECEDOR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Total: " + valorTotal);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Mostruário: " + pcMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Atacado: " + pcAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Varejo: " + pcVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");


            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='4'>");
            _texto.AppendLine("                                <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse; width: 517pt;'>");
            _texto.AppendLine("                                    <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                                        <td style=' text-align: left; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            -");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            TOTAL");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;MOSTRUÁRIO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO7.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? (mostruarioQtde.CO1 + mostruarioQtde.CO2 + mostruarioQtde.CO3 + mostruarioQtde.CO4 + mostruarioQtde.CO5 + mostruarioQtde.CO6 + mostruarioQtde.CO7).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;VAREJO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO7.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? (varejoQtde.CO1 + varejoQtde.CO2 + varejoQtde.CO3 + varejoQtde.CO4 + varejoQtde.CO5 + varejoQtde.CO6 + varejoQtde.CO7).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;ATACADO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO7.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? (atacadoQtde.CO1 + atacadoQtde.CO2 + atacadoQtde.CO3 + atacadoQtde.CO4 + atacadoQtde.CO5 + atacadoQtde.CO6 + atacadoQtde.CO7).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;TOTAL");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO1 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO1 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO1 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO2 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO2 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO2 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO3 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO3 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO3 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO4 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO4 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO4 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO5 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO5 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO5 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO6 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO6 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO6 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO7 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO7 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO7 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " +
                                        (((varejoQtde != null) ? (varejoQtde.CO1 + varejoQtde.CO2 + varejoQtde.CO3 + varejoQtde.CO4 + varejoQtde.CO5 + varejoQtde.CO6 + varejoQtde.CO7) : 0)
                                        +
                                        ((mostruarioQtde != null) ? (mostruarioQtde.CO1 + mostruarioQtde.CO2 + mostruarioQtde.CO3 + mostruarioQtde.CO4 + mostruarioQtde.CO5 + mostruarioQtde.CO6 + mostruarioQtde.CO7) : 0)
                                        +
                                        ((atacadoQtde != null) ? (atacadoQtde.CO1 + atacadoQtde.CO2 + atacadoQtde.CO3 + atacadoQtde.CO4 + atacadoQtde.CO5 + atacadoQtde.CO6 + atacadoQtde.CO7) : 0)).ToString()

                );
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            //VAREJO
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE VAREJO</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                -");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP ESTOQUE VAREJO
            for (contLinha = 1; contLinha <= tLinhaVarejo; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP ESTOQUE VAREJO

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //ATACADO
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE ATACADO</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP ESTOQUE ATACADO
            for (contLinha = 1; contLinha <= tLinhaAtacado; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP ESTOQUE ATACADO

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //CONTROLE DE QUALIDADE
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>CONTR QUALIDADE</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP CONTROLE DE QUALIDADE
            for (contLinha = 1; contLinha <= tLinhaControleQ; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP CONTROLE DE QUALIDADE

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //RODAPE
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                PILOTO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                MOSTRUÁRIO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='8' style='text-align:left;'>");
            _texto.AppendLine("                                SCS - 2ª QUALIDADE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                PERDA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                AMOSTRA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 12px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%;'>");
            _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                OBSERVAÇÕES:");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: 1px solid #FFF; line-height: 15px;'>");
            _texto.AppendLine("                            <td style='text-align: center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("</div>");

            return _texto;
        }
        #endregion

        protected void imgExc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgExc = (ImageButton)sender;
                var imgs = imgExc.CommandArgument.Split('|');
                var pedidoIntra = imgs[0];
                var produto = imgs[1];
                var corProduto = imgs[2];
                var entrega = Convert.ToDateTime(imgs[3]);

                desenvController.ExcluirComprasProdutoPrePedido(pedidoIntra, produto, corProduto, entrega);
                CarregarProdutoAcabado();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

