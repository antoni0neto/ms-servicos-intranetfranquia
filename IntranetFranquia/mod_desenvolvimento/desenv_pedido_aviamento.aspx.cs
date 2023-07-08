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
    public partial class desenv_pedido_aviamento : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        const string DESENV_MATERIAL_PED = "DESENV_MATERIAL_PED";

        decimal qtdeMaterial = 0;
        decimal valTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarFabricante();
                CarregarGriffe();
                CarregarGrupos();

                var codigoCarrinhoCab = "";
                if (Request.QueryString["p"] != null)
                    codigoCarrinhoCab = Request.QueryString["p"].ToString();

                hidCodigoCarrinhoCab.Value = codigoCarrinhoCab;

                var carrinhoCab = desenvController.ObterCarrinhoMaterialCab(Convert.ToInt32(codigoCarrinhoCab));
                if (carrinhoCab.DATA_PEDIDO != null)
                {
                    btBuscar.Enabled = false;
                    btExcluirCarrinho.Enabled = false;
                    btGerarPedido.Enabled = false;
                    btAdicionarQtdeEstoque.Enabled = false;
                    hidBloqueio.Value = "S";
                }

                ddlColecoes.SelectedValue = baseController.BuscaColecaoAtual(carrinhoCab.COLECAO).COLECAO;
                ddlColecoes.Enabled = false;
                CarregarOrigem(ddlColecoes.SelectedValue);

                CarregarCarrinho(codigoCarrinhoCab);

                labPedidoTipo.Text = carrinhoCab.DESENV_MATERIAL_PEDIDO_PERFIL1.PERFIL;
                if (carrinhoCab.DESENV_MATERIAL_PEDIDO_PERFIL == 3)
                {
                    btBuscar.Enabled = false;
                    btAdicionarDiff.Enabled = false;
                }
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
            var _grupo = (prodController.ObterGrupoProduto("01"));
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
            _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'A') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFabricante.DataSource = _fornecedores;
                ddlFabricante.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();

                ddlMaterialGrupoEstoque.DataSource = _matGrupo;
                ddlMaterialGrupoEstoque.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;
            if (grupo.Trim() != "")
                _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();
            else
                _matSubGrupo = desenvController.ObterMaterialSubGrupo();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

                ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                ddlMaterialSubGrupo.DataBind();
            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            var materiais = desenvController.ObterMaterialCorV2(grupo, subGrupo, "");
            var coresBasicas = prodController.ObterCoresBasicas();

            coresBasicas = coresBasicas.Where(p => materiais.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlCor.DataSource = coresBasicas;
            ddlCor.DataBind();

            if (coresBasicas.Count == 2)
                ddlCor.SelectedIndex = 1;

        }

        #endregion

        #region "MATERIAL DO PRODUTO"
        private void CarregarProdutoMaterial(string pedidoIntra, string fornecedorMaterial, string colecao, int? desenvOrigem, string produto, string grupoProduto, string griffe)
        {
            var produtoMaterial = ObterProdutoCompraMATERIAL(pedidoIntra, fornecedorMaterial, colecao, desenvOrigem, produto, grupoProduto, griffe);

            gvProdutoFicTec.DataSource = produtoMaterial;
            gvProdutoFicTec.DataBind();

            Session[DESENV_MATERIAL_PED] = produtoMaterial;
        }
        protected void gvProdutoFicTec_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_COMPRA_MATERIALResult produto = e.Row.DataItem as SP_OBTER_PRODUTO_COMPRA_MATERIALResult;

                    if (produto != null)
                    {

                        Button _btAdicionarCarrinho = e.Row.FindControl("btAdicionarCarrinho") as Button;
                        _btAdicionarCarrinho.CommandArgument = produto.CODIGO_DESENV_FICTEC.ToString();

                        Button _btAdicionarDOEstoque = e.Row.FindControl("btAdicionarDOEstoque") as Button;
                        _btAdicionarDOEstoque.CommandArgument = produto.CODIGO_DESENV_FICTEC.ToString();

                        //var materialNoCarrinho = desenvController.ObterCarrinhoMaterialCab(produto.CODIGO_DESENV_FICTEC, Convert.ToInt32(hidCodigoCarrinhoCab.Value), produto.pedido);
                        //if (materialNoCarrinho != null)
                        //    e.Row.BackColor = Color.SandyBrown;

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvProdutoFicTec_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[DESENV_MATERIAL_PED] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_COMPRA_MATERIALResult> produtoMaterial = (IEnumerable<SP_OBTER_PRODUTO_COMPRA_MATERIALResult>)Session[DESENV_MATERIAL_PED];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvProdutoFicTec, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoFicTec, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoFicTec, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                produtoMaterial = produtoMaterial.OrderBy(e.SortExpression + sortDirection);
                gvProdutoFicTec.DataSource = produtoMaterial;
                gvProdutoFicTec.DataBind();
            }
        }


        #endregion

        #region "GERAR PEDIDO"
        protected void btGerarPedido_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {

                    string codigoCarrinhoCab = hidCodigoCarrinhoCab.Value;

                    if (gvCarrinho.Rows.Count > 0)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('desenv_pedido_aviamento_gerarprepedido.aspx?ccc=" + codigoCarrinhoCab + "');";
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
        #endregion

        private List<SP_OBTER_PRODUTO_COMPRA_MATERIALResult> ObterProdutoCompraMATERIAL(string pedidoIntra, string fornecedorMaterial, string colecao, int? desenvOrigem, string produto, string grupoProduto, string griffe)
        {
            var grupoMaterial = ddlMaterialGrupo.SelectedItem.Text.Trim();
            var subGrupoMaterial = (ddlMaterialSubGrupo.SelectedItem != null) ? ddlMaterialSubGrupo.SelectedItem.Text.Trim() : "";
            var corMaterial = (ddlCor.SelectedItem != null) ? ddlCor.SelectedValue.Trim() : "";

            var produtoMaterial = desenvController.ObterProdutoCompraMATERIAL(pedidoIntra, fornecedorMaterial, colecao, produto, grupoProduto, griffe, grupoMaterial, subGrupoMaterial);

            if (desenvOrigem > 0)
                produtoMaterial = produtoMaterial.Where(p => p.DESENV_PRODUTO_ORIGEM == desenvOrigem).ToList();

            if (corMaterial != "")
                produtoMaterial = produtoMaterial.Where(p => p.COR_MATERIAL.Trim() == corMaterial).ToList();

            return produtoMaterial;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            int? desenvOrigem = null;

            try
            {
                labPedido.Text = "";
                labMsg.Text = "";
                labMsgEstoque.Text = "";

                if (ddlColecoes.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Coleção.";
                    return;
                }

                if (ddlOrigem.SelectedValue != "0")
                    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

                CarregarCarrinho(hidCodigoCarrinhoCab.Value);
                CarregarProdutoMaterial("", ddlFabricante.SelectedValue.Trim(), ddlColecoes.SelectedValue.Trim(), desenvOrigem, txtProduto.Text.Trim(), ddlGrupo.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarCarrinho(string codigoCarrinhoCab)
        {
            var carrinho = desenvController.ObterMaterialCarrinhoPrePedido(Convert.ToInt32(codigoCarrinhoCab));
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_CARRINHO_PREPEDIDOResult carrinho = e.Row.DataItem as SP_OBTER_MATERIAL_CARRINHO_PREPEDIDOResult;

                    if (carrinho != null)
                    {
                        Literal litOrigem = e.Row.FindControl("litOrigem") as Literal;
                        litOrigem.Text = carrinho.DESC_PEDIDO_ORIGEM;

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        TextBox txtHB = e.Row.FindControl("txtHB") as TextBox;
                        txtHB.Text = (carrinho.HB == null || carrinho.HB == 0) ? "" : carrinho.HB.ToString();

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.DESC_PRODUTO;

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = carrinho.COR_FORNECEDOR_PRODUTO;

                        Literal litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        litDetalhe.Text = carrinho.DETALHE;

                        Literal litSubGrupo = e.Row.FindControl("litSubGrupo") as Literal;
                        litSubGrupo.Text = carrinho.SUBGRUPO;

                        Literal litCorMaterial = e.Row.FindControl("litCorMaterial") as Literal;
                        litCorMaterial.Text = carrinho.COR_FORNECEDOR_MATERIAL;

                        Literal litUnidadeMedida = e.Row.FindControl("litUnidadeMedida") as Literal;
                        litUnidadeMedida.Text = desenvController.ObterMaterial(carrinho.MATERIAL).UNID_FICHA_TEC;

                        TextBox txtQtdeMaterial = e.Row.FindControl("txtQtdeMaterial") as TextBox;
                        txtQtdeMaterial.Text = carrinho.QTDE.ToString();

                        TextBox txtCusto = e.Row.FindControl("txtCusto") as TextBox;
                        txtCusto.Text = carrinho.CUSTO.ToString();

                        TextBox txtDesconto = e.Row.FindControl("txtDesconto") as TextBox;
                        txtDesconto.Text = carrinho.DESCONTO_ITEM.ToString();

                        Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                        litValTotal.Text = Convert.ToDecimal(carrinho.VALOR_TOTAL).ToString("###,###,###,##0.00000");

                        qtdeMaterial += carrinho.QTDE;
                        valTotal += Convert.ToDecimal(carrinho.VALOR_TOTAL);



                        if (carrinho.QTDE > 0 && carrinho.CUSTO > 0)
                            e.Row.BackColor = Color.PaleGreen;

                        if (carrinho.GRADEADO == true)
                            e.Row.BackColor = Color.LightPink;

                        Button btExcluirItemCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as Button;
                        if (hidBloqueio.Value == "S")
                        {
                            btExcluirItemCarrinho.Enabled = false;
                            txtQtdeMaterial.Enabled = false;
                            txtCusto.Enabled = false;
                            txtDesconto.Enabled = false;
                            txtHB.Enabled = false;
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
        protected void gvCarrinho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinho.FooterRow;
            if (footer != null)
            {

                footer.Cells[1].Text = "Total";
                footer.Cells[10].Text = qtdeMaterial.ToString();
                footer.Cells[13].Text = "R$ " + valTotal.ToString("###,###,###,##0.0000");
            }
        }

        protected void btExcluirItemCarrinho_Click(object sender, EventArgs e)
        {
            try
            {

                labPedido.Text = "";
                labMsg.Text = "";
                labMsgEstoque.Text = "";

                Button bt = (Button)(sender);
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][0].ToString());
                char tipo = Convert.ToChar(gvCarrinho.DataKeys[row.RowIndex][1].ToString());

                if (tipo == 'P')
                    desenvController.ExcluirCarrinhoMaterial(codigoItemCarrinho);
                else
                    desenvController.ExcluirCarrinhoMaterialEstoque(codigoItemCarrinho);

                CarregarCarrinho(hidCodigoCarrinhoCab.Value);
            }
            catch (Exception)
            {
            }
        }
        protected void btExcluirCarrinho_Click(object sender, EventArgs e)
        {
            try
            {

                labPedido.Text = "";
                labMsg.Text = "";
                labMsgEstoque.Text = "";

                desenvController.ExcluirCarrinhoMaterialPorCab(Convert.ToInt32(hidCodigoCarrinhoCab.Value));
                desenvController.ExcluirCarrinhoMaterialEstoquePorCab(Convert.ToInt32(hidCodigoCarrinhoCab.Value));

                CarregarCarrinho(hidCodigoCarrinhoCab.Value);

            }
            catch (Exception)
            {
            }
        }
        protected void gvCarrinho_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_MATERIAL_CARRINHO_PREPEDIDOResult> carrinhoMaterial = desenvController.ObterMaterialCarrinhoPrePedido(Convert.ToInt32(hidCodigoCarrinhoCab.Value));

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvCarrinho, e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(gvCarrinho, e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(gvCarrinho, e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            carrinhoMaterial = carrinhoMaterial.OrderBy(e.SortExpression + sortDirection);
            gvCarrinho.DataSource = carrinhoMaterial;
            gvCarrinho.DataBind();
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
                labMsgEstoque.Text = "";
                labMsg.Text = "";

                Button bt = (Button)(sender);

                int codigoProdutoFicTec = Convert.ToInt32(bt.CommandArgument);
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                int codigoCarrinhoCab = Convert.ToInt32(hidCodigoCarrinhoCab.Value);

                var produtoNoCarrinho = desenvController.ObterCarrinhoMaterial(codigoProdutoFicTec, codigoCarrinhoCab, "C");
                if (produtoNoCarrinho != null)
                {
                    labPedido.Text = "Aviamento já foi adicionado.";
                    return;
                }

                GridViewRow row = (GridViewRow)bt.NamingContainer;
                Literal litQtde = row.FindControl("litQtde") as Literal;
                var qtde = 0M;
                if (litQtde.Text != "")
                {
                    qtde = Convert.ToDecimal(litQtde.Text);
                }

                var carrinho = new DESENV_MATERIAL_CARRINHO
                {
                    DESENV_PRODUTO_FICTEC = codigoProdutoFicTec,
                    USUARIO = codigoUsuario,
                    CUSTO = 0,
                    QTDE = qtde,
                    DESCONTO_ITEM = 0,
                    DATA_INCLUSAO = DateTime.Now,
                    DESENV_MATERIAL_CARRINHO_CAB = Convert.ToInt32(hidCodigoCarrinhoCab.Value),
                    PEDIDO_ORIGEM = "C" // COMPRA
                };
                desenvController.InserirCarrinhoMaterial(carrinho);

                CarregarCarrinho(hidCodigoCarrinhoCab.Value);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void btAdicionarDOEstoque_Click(object sender, EventArgs e)
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
                labMsgEstoque.Text = "";
                btAdicionarDiff.Enabled = false;

                Button bt = (Button)(sender);

                int codigoProdutoFicTec = Convert.ToInt32(bt.CommandArgument);
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                int codigoCarrinhoCab = Convert.ToInt32(hidCodigoCarrinhoCab.Value);

                var produtoNoCarrinho = desenvController.ObterCarrinhoMaterial(codigoProdutoFicTec, codigoCarrinhoCab, "E");
                if (produtoNoCarrinho != null)
                {
                    labMsgEstoque.Text = "O Aviamento deste produto já foi adicionado.";
                    return;
                }

                GridViewRow row = (GridViewRow)bt.NamingContainer;
                Literal litQtde = row.FindControl("litQtde") as Literal;
                var qtde = 0M;
                if (litQtde.Text != "")
                {
                    qtde = Convert.ToDecimal(litQtde.Text);
                }

                // validar qtde do material no estoque
                hidSaldoEstoque.Value = "";
                btAdicionarDiff.CommandArgument = "";

                var ficTec = desenvController.ObterFichaTecnicaCodigo(codigoProdutoFicTec);
                var saldoEstoque = desenvController.ObterMaterialEstoqueSaldoPedido(ficTec.MATERIAL, ficTec.COR_MATERIAL, 0); // aqui nao conta o que ja esta no carrinho
                if (saldoEstoque != null)
                {
                    if (qtde > saldoEstoque.QTDE_ESTOQUE)
                    {
                        labMsgEstoque.Text = "Saldo de estoque indisponível. Saldo: " + saldoEstoque.QTDE_ESTOQUE.ToString();
                        hidSaldoEstoque.Value = saldoEstoque.QTDE_ESTOQUE.ToString();
                        btAdicionarDiff.CommandArgument = codigoProdutoFicTec.ToString();
                        btAdicionarDiff.Enabled = true;

                        return;
                    }
                }
                else
                {
                    labMsgEstoque.Text = "Saldo de estoque indisponível. Saldo: 0,000";
                    return;
                }

                var carrinho = new DESENV_MATERIAL_CARRINHO
                {
                    DESENV_PRODUTO_FICTEC = codigoProdutoFicTec,
                    USUARIO = codigoUsuario,
                    CUSTO = 0,
                    DESCONTO_ITEM = 0,
                    QTDE = qtde,
                    DATA_INCLUSAO = DateTime.Now,
                    DESENV_MATERIAL_CARRINHO_CAB = Convert.ToInt32(hidCodigoCarrinhoCab.Value),
                    PEDIDO_ORIGEM = "E" // RETIRAR DO ESTOQUE
                };
                desenvController.InserirCarrinhoMaterial(carrinho);

                CarregarCarrinho(hidCodigoCarrinhoCab.Value);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void btAdicionarQtdeEstoque_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labMsgEstoque.Text = "";

                if (ddlMaterialGrupoEstoque.SelectedValue == "")
                {
                    labMsg.Text = "Selecione o Grupo do Material para estoque.";
                    return;
                }
                if (ddlMaterialSubGrupoEstoque.SelectedValue == "")
                {
                    labMsg.Text = "Selecione o Subgrupo do Material para estoque.";
                    return;
                }
                if (ddlMaterialCorEstoque.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Cor do Material para estoque.";
                    return;
                }
                if (ddlMaterialCorFornecedorEstoque.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Cor do Fornecedor do Material para estoque.";
                    return;
                }

                if (txtMaterialQtdeEstoque.Text.Trim() == "")
                {
                    labMsg.Text = "Informe a quantidade do Material para estoque.";
                    return;
                }

                if (txtMaterialCustoEstoque.Text.Trim() == "")
                {
                    labMsg.Text = "Informe o custo do Material para estoque.";
                    return;
                }

                var carrinhoEstoque = new DESENV_MATERIAL_CARRINHO_ESTOQUE();
                carrinhoEstoque.DESENV_MATERIAL_CARRINHO_CAB = Convert.ToInt32(hidCodigoCarrinhoCab.Value);
                carrinhoEstoque.DATA_INCLUSAO = DateTime.Now;
                carrinhoEstoque.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                carrinhoEstoque.GRUPO = ddlMaterialGrupoEstoque.SelectedItem.Text.Trim();
                carrinhoEstoque.SUBGRUPO = ddlMaterialSubGrupoEstoque.SelectedItem.Text.Trim();
                carrinhoEstoque.COR_MATERIAL = ddlMaterialCorEstoque.SelectedValue.Trim();
                carrinhoEstoque.COR_FORNECEDOR = ddlMaterialCorFornecedorEstoque.SelectedValue.Trim();
                carrinhoEstoque.QTDE = Convert.ToDecimal(txtMaterialQtdeEstoque.Text);
                carrinhoEstoque.CUSTO = Convert.ToDecimal(txtMaterialCustoEstoque.Text);
                carrinhoEstoque.DESCONTO_ITEM = 0.00M;
                var material = desenvController.ObterMaterialCorV2(carrinhoEstoque.GRUPO, carrinhoEstoque.SUBGRUPO, "").Where(p => p.COR_MATERIAL.Trim() == carrinhoEstoque.COR_MATERIAL && p.REFER_FABRICANTE.Trim() == carrinhoEstoque.COR_FORNECEDOR.Trim()).FirstOrDefault();
                carrinhoEstoque.MATERIAL = material.MATERIAL.Substring(0, 10).Replace("|", "");

                desenvController.InserirCarrinhoMaterialEstoque(carrinhoEstoque);

                CarregarCarrinho(hidCodigoCarrinhoCab.Value);

                ddlMaterialCorEstoque.SelectedValue = "";
                ddlMaterialCorFornecedorEstoque.SelectedValue = "";
                txtMaterialQtdeEstoque.Text = "";
                txtMaterialCustoEstoque.Text = "";

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;

            }
        }

        #region "PEDIDO DE COMPRA PARA ESTOQUE"
        private void CarregarSubGruposEstoque(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;
            if (grupo.Trim() != "")
                _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();
            else
                _matSubGrupo = desenvController.ObterMaterialSubGrupo();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });

                ddlMaterialSubGrupoEstoque.DataSource = _matSubGrupo;
                ddlMaterialSubGrupoEstoque.DataBind();
            }
        }
        protected void ddlMaterialGrupoEstoque_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarSubGruposEstoque(ddlMaterialGrupoEstoque.SelectedItem.Text.Trim());

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }
        private void CarregarCoresEstoque(string grupo, string subGrupo)
        {
            var materiais = desenvController.ObterMaterialCorV2(grupo, subGrupo, "");
            var coresBasicas = prodController.ObterCoresBasicas();

            coresBasicas = coresBasicas.Where(p => materiais.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlMaterialCorEstoque.DataSource = coresBasicas;
            ddlMaterialCorEstoque.DataBind();

            if (coresBasicas.Count == 2)
            {
                ddlMaterialCorEstoque.SelectedIndex = 1;
                CarregarCorFornecedorEstoque(ddlMaterialGrupoEstoque.SelectedItem.Text.Trim(), ddlMaterialSubGrupoEstoque.SelectedItem.Text.Trim(), ddlMaterialCorEstoque.SelectedValue);
            }

        }
        protected void ddlMaterialSubGrupoEstoque_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCoresEstoque(ddlMaterialGrupoEstoque.SelectedItem.Text.Trim(), ddlMaterialSubGrupoEstoque.SelectedItem.Text.Trim());
        }
        private void CarregarCorFornecedorEstoque(string grupo, string subGrupo, string cor)
        {
            var materiais = desenvController.ObterMaterialCorV2(grupo, subGrupo, "");
            materiais = materiais.Where(p => p.COR_MATERIAL.Trim() == cor.Trim()).ToList();

            materiais.Insert(0, new SP_OBTER_MATERIAL_CORESResult { REFER_FABRICANTE = "" });

            ddlMaterialCorFornecedorEstoque.DataSource = materiais;
            ddlMaterialCorFornecedorEstoque.DataBind();

            if (materiais.Count == 2)
                ddlMaterialCorFornecedorEstoque.SelectedIndex = 1;

        }
        protected void ddlMaterialCorEstoque_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCorFornecedorEstoque(ddlMaterialGrupoEstoque.SelectedItem.Text.Trim(), ddlMaterialSubGrupoEstoque.SelectedItem.Text.Trim(), ddlMaterialCorEstoque.SelectedValue);
        }

        #endregion

        protected void btAdicionarDiff_Click(object sender, EventArgs e)
        {
            try
            {
                labMsgEstoque.Text = "";

                if (btAdicionarDiff.CommandArgument != "")
                {
                    int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    int codigoProdutoFicTec = Convert.ToInt32(btAdicionarDiff.CommandArgument);

                    if (hidSaldoEstoque.Value != "")
                    {
                        var carrinho = new DESENV_MATERIAL_CARRINHO
                        {
                            DESENV_PRODUTO_FICTEC = codigoProdutoFicTec,
                            USUARIO = codigoUsuario,
                            CUSTO = 0,
                            DESCONTO_ITEM = 0,
                            QTDE = Convert.ToDecimal(hidSaldoEstoque.Value),
                            DATA_INCLUSAO = DateTime.Now,
                            DESENV_MATERIAL_CARRINHO_CAB = Convert.ToInt32(hidCodigoCarrinhoCab.Value),
                            PEDIDO_ORIGEM = "E" // RETIRAR DO ESTOQUE
                        };
                        desenvController.InserirCarrinhoMaterial(carrinho);

                        btAdicionarDiff.Enabled = false;

                        CarregarCarrinho(hidCodigoCarrinhoCab.Value);

                    }
                }

            }
            catch (Exception ex)
            {
                labMsgEstoque.Text = ex.Message;
            }
        }


        protected void txtCusto_TextChanged(object sender, EventArgs e)
        {
            labPedido.Text = "";
            labMsg.Text = "";

            TextBox txt = (TextBox)sender;

            GridViewRow row = (GridViewRow)txt.NamingContainer;
            int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][0].ToString());
            char tipo = Convert.ToChar(gvCarrinho.DataKeys[row.RowIndex][1].ToString());

            //atualizar grade do produto no carrinho
            decimal custo = (txt.Text.Trim() == "") ? 0M : Convert.ToDecimal(txt.Text.Trim());

            desenvController.AtualizarMaterialCarrinhoCusto(Convert.ToInt32(hidCodigoCarrinhoCab.Value), custo, codigoItemCarrinho, tipo);

            CarregarCarrinho(hidCodigoCarrinhoCab.Value);
        }
        protected void txtQtdeMaterial_TextChanged(object sender, EventArgs e)
        {
            labPedido.Text = "";
            labMsg.Text = "";
            labMsgEstoque.Text = "";

            TextBox txt = (TextBox)sender;

            GridViewRow row = (GridViewRow)txt.NamingContainer;
            int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][0].ToString());
            char tipo = Convert.ToChar(gvCarrinho.DataKeys[row.RowIndex][1].ToString());

            //atualizar grade do produto no carrinho
            decimal qtdeGrade = (txt.Text.Trim() == "") ? 0 : Convert.ToDecimal(txt.Text.Trim());

            if (tipo == 'P')
            {
                var carrinho = desenvController.ObterCarrinhoMaterial(codigoItemCarrinho);

                // SE estiver retirando do estoque, valida esstoque na alteracao...
                if (carrinho.PEDIDO_ORIGEM == "E")
                {
                    var ficTec = desenvController.ObterFichaTecnicaCodigo(carrinho.DESENV_PRODUTO_FICTEC);
                    var saldoEstoque = desenvController.ObterMaterialEstoqueSaldoPedido(ficTec.MATERIAL, ficTec.COR_MATERIAL, carrinho.CODIGO);
                    if (saldoEstoque != null)
                    {
                        if (qtdeGrade > saldoEstoque.QTDE_ESTOQUE)
                        {
                            labMsgEstoque.Text = "Saldo de estoque indisponível. Saldo: " + saldoEstoque.QTDE_ESTOQUE.ToString();
                            return;
                        }
                    }
                }

                carrinho.QTDE = qtdeGrade;
                desenvController.AtualizarCarrinhoMaterial(carrinho);
            }
            else
            {
                var carrinho = desenvController.ObterCarrinhoMaterialEstoque(codigoItemCarrinho);
                carrinho.QTDE = qtdeGrade;
                desenvController.AtualizarCarrinhoMaterialEstoque(carrinho);
            }

            CarregarCarrinho(hidCodigoCarrinhoCab.Value);

        }
        protected void txtDesconto_TextChanged(object sender, EventArgs e)
        {
            labPedido.Text = "";
            labMsg.Text = "";

            TextBox txt = (TextBox)sender;

            GridViewRow row = (GridViewRow)txt.NamingContainer;
            int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][0].ToString());
            char tipo = Convert.ToChar(gvCarrinho.DataKeys[row.RowIndex][1].ToString());

            //atualizar grade do produto no carrinho
            decimal descontoItem = (txt.Text.Trim() == "") ? 0 : Convert.ToDecimal(txt.Text.Trim());

            if (tipo == 'P')
            {
                var carrinho = desenvController.ObterCarrinhoMaterial(codigoItemCarrinho);
                carrinho.DESCONTO_ITEM = descontoItem;
                desenvController.AtualizarCarrinhoMaterial(carrinho);
            }
            else
            {
                var carrinho = desenvController.ObterCarrinhoMaterialEstoque(codigoItemCarrinho);
                carrinho.DESCONTO_ITEM = descontoItem;
                desenvController.AtualizarCarrinhoMaterialEstoque(carrinho);
            }

            CarregarCarrinho(hidCodigoCarrinhoCab.Value);

        }

        protected void txtHB_TextChanged(object sender, EventArgs e)
        {
            labPedido.Text = "";
            labMsg.Text = "";

            TextBox txt = (TextBox)sender;

            GridViewRow row = (GridViewRow)txt.NamingContainer;
            int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex][0].ToString());
            char tipo = Convert.ToChar(gvCarrinho.DataKeys[row.RowIndex][1].ToString());

            //atualizar grade do produto no carrinho
            int hb = (txt.Text.Trim() == "") ? 0 : Convert.ToInt32(txt.Text.Trim());

            if (tipo == 'P')
            {
                var carrinho = desenvController.ObterCarrinhoMaterial(codigoItemCarrinho);
                carrinho.HB = hb;
                desenvController.AtualizarCarrinhoMaterial(carrinho);
            }


            CarregarCarrinho(hidCodigoCarrinhoCab.Value);
        }

    }
}

