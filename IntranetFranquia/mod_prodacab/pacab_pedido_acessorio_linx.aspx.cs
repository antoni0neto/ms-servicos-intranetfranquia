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
    public partial class pacab_pedido_acessorio_linx : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string PROD_ACAB_PREPEDCOMPRA_ACESS = "PROD_ACAB_PEDCOMPRA_ACESS";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();

                CarregarFabricante();
                CarregarFilial();

                CarregarCarrinhoLinx();

                Session[PROD_ACAB_PREPEDCOMPRA_ACESS] = null;
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
            List<SP_OBTER_GRUPOResult> _grupo = (new ProducaoController().ObterGrupoProduto("02"));
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
        #endregion

        #region "PRODUTO"
        private void CarregarProdutoAcabado(string colecao, int? desenvOrigem, string grupo, string produto, string filial, string fornecedor)
        {
            var produtoAcabado = ObterProdutoAcabadoAcessorioPrePedido(colecao, desenvOrigem, grupo, produto, filial, fornecedor);

            gvProdutoAcabado.DataSource = produtoAcabado;
            gvProdutoAcabado.DataBind();

            Session[PROD_ACAB_PREPEDCOMPRA_ACESS] = produtoAcabado;
        }
        protected void gvProdutoAcabado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PREPEDIDOResult produtoAcabado = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PREPEDIDOResult;

                    if (produtoAcabado != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(produtoAcabado.COLECAO)).DESC_COLECAO;


                        var acessorioNoCarrinho = desenvController.ObterCarrinhoLinxAcessorio(produtoAcabado.PRODUTO, produtoAcabado.COR, ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);
                        if (acessorioNoCarrinho != null)
                            e.Row.BackColor = Color.SandyBrown;

                        //Popular GRID VIEW FILHO
                        if (produtoAcabado.FOTO1 != null && produtoAcabado.FOTO1.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_ACESSORIO> _fotoProduto = new List<DESENV_ACESSORIO>();
                                _fotoProduto.Add(new DESENV_ACESSORIO { FOTO1 = produtoAcabado.FOTO1, FOTO2 = produtoAcabado.FOTO2 });
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

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvProdutoAcabado_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PROD_ACAB_PREPEDCOMPRA_ACESS] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PREPEDIDOResult> produtoAcabado = (IEnumerable<SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PREPEDIDOResult>)Session[PROD_ACAB_PREPEDCOMPRA_ACESS];

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
                    DESENV_ACESSORIO _acessorio = e.Row.DataItem as DESENV_ACESSORIO;
                    if (_acessorio != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _acessorio.FOTO1;

                    }
                }
            }
        }

        #endregion

        private List<SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PREPEDIDOResult> ObterProdutoAcabadoAcessorioPrePedido(string colecao, int? desenvOrigem, string grupo, string produto, string filial, string fornecedor)
        {
            var produtoAcabado = desenvController.ObterProdutoAcabadoAcessorioPrePedido(colecao, desenvOrigem, grupo, produto, filial, fornecedor);

            return produtoAcabado;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            int? desenvOrigem = null;

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

                if (ddlOrigem.SelectedValue != "0")
                    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

                CarregarProdutoAcabado(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim().ToUpper(), ddlFilial.SelectedItem.Text.Trim(), ddlFabricante.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarCarrinhoLinx()
        {
            var carrinho = desenvController.ObterCarrinhoLinxAcessorioPorUsuario(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_ACESSORIO_CARRINHO_LINX carrinho = e.Row.DataItem as DESENV_ACESSORIO_CARRINHO_LINX;

                    if (carrinho != null)
                    {

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = baseController.BuscaProduto(carrinho.PRODUTO).DESC_PRODUTO.Trim();

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = prodController.ObterCoresBasicas(carrinho.COR_PRODUTO).DESC_COR;


                        var acessorio = desenvController.ObterAcessorio(carrinho.PRODUTO, carrinho.COR_PRODUTO);

                        Literal litRefFabricante = e.Row.FindControl("litRefFabricante") as Literal;
                        litRefFabricante.Text = acessorio.REFER_FABRICANTE;

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = (acessorio.CUSTO == null) ? "" : ("R$ " + acessorio.CUSTO.ToString());

                        TextBox txtCustoNota = e.Row.FindControl("txtCustoNota") as TextBox;
                        txtCustoNota.Text = (acessorio.CUSTO_NOTA == null) ? "" : (acessorio.CUSTO_NOTA.ToString());

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
                desenvController.ExcluirCarrinhoLinxAcessorioPorUsuario(codigoUsuario);
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
                desenvController.ExcluirCarrinhoLinxAcessorioPorCodigo(codigoCarrinho);

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

                var acessorioNoCarrinho = desenvController.ObterCarrinhoLinxAcessorio(produto, cor, codigoUsuario);
                if (acessorioNoCarrinho != null)
                {
                    labPedido.Text = "Produto já foi adicionado.";
                    return;
                }

                var acessorioTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                if (acessorioTemPedido != null && acessorioTemPedido.PEDIDO_LINX != null)
                {
                    labPedido.Text = "Produto já possui pedido gerado no LINX: " + acessorioTemPedido.PEDIDO_LINX;
                    return;
                }

                var carrinho = new DESENV_ACESSORIO_CARRINHO_LINX
                {
                    PEDIDO = pedido,
                    PRODUTO = produto,
                    COR_PRODUTO = cor,
                    ENTREGA = entrega,
                    USUARIO = codigoUsuario,
                };
                desenvController.InserirCarrinhoLinxAcessorio(carrinho);

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
                    _url = "fnAbrirTelaCadastroMaior('pacab_pedido_acessorio_grade_linx.aspx?c=" + codigoCarrinho.ToString() + "');";
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
                        _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_linx_gerarv10.aspx');";
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

            int desenvOrigem = 0;
            if (ddlOrigem.SelectedValue != "0")
                desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

            labPedido.Text = "";
            labMsg.Text = "";

            var produtos = ObterProdutoAcabadoAcessorioPrePedido(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim().ToUpper(), ddlFilial.SelectedItem.Text.Trim(), ddlFabricante.SelectedValue.Trim());
            foreach (var p in produtos)
            {
                string pedido = p.PEDIDO_INTRA;
                string produto = p.PRODUTO;
                string cor = p.COR;
                DateTime entrega = Convert.ToDateTime(p.ENTREGA);

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var acessorioNoCarrinho = desenvController.ObterCarrinhoLinxAcessorio(produto, cor, codigoUsuario);
                if (acessorioNoCarrinho != null)
                {
                    //labPedido.Text = "Produto já foi adicionado.";
                    continue;
                }

                var acessorioTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                if (acessorioTemPedido != null && acessorioTemPedido.PEDIDO_LINX != null)
                {
                    //labPedido.Text = "Produto já possui pedido gerado no LINX: " + acessorioTemPedido.PEDIDO_LINX;
                    continue;
                }

                var carrinho = new DESENV_ACESSORIO_CARRINHO_LINX
                {
                    PEDIDO = pedido,
                    PRODUTO = produto,
                    COR_PRODUTO = cor,
                    ENTREGA = entrega,
                    USUARIO = codigoUsuario,
                };
                desenvController.InserirCarrinhoLinxAcessorio(carrinho);
            }

            CarregarCarrinhoLinx();
        }

        protected void txtCustoNota_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            Literal litProduto = row.FindControl("litProduto") as Literal;
            var produtos = desenvController.ObterAcessorioPorProduto(litProduto.Text.Trim());
            foreach (var p in produtos)
            {
                var pp = desenvController.ObterAcessorio(p.CODIGO);
                pp.CUSTO_NOTA = Convert.ToDecimal(txt.Text.Trim());
                desenvController.AtualizarAcessorio(pp);
            }

            CarregarCarrinhoLinx();

        }

    }
}

