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
    public partial class pacab_pedido_produtov2 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        const string PROD_ACAB_PROD_PEDCOMPRA = "PROD_ACAB_PROD_PEDCOMPRA";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarFabricante();
                CarregarGriffe();
                CarregarCarrinho();
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
            _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

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
        #endregion

        #region "PRODUTO"
        private void CarregarProdutoAcabado(string colecao, int? desenvOrigem, string grupo, string produto)
        {
            var produtoAcabado = ObterProdutoAcabadoProdutoV2(colecao, desenvOrigem, grupo, produto);

            gvProdutoAcabado.DataSource = produtoAcabado;
            gvProdutoAcabado.DataBind();

            Session[PROD_ACAB_PROD_PEDCOMPRA] = produtoAcabado;
        }
        protected void gvProdutoAcabado_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADO_PRODUTOV2Result produto = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADO_PRODUTOV2Result;

                    if (produto != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(produto.COLECAO)).DESC_COLECAO;

                        Button _btAdicionarCarrinho = e.Row.FindControl("btAdicionarCarrinho") as Button;
                        if (_btAdicionarCarrinho != null)
                            _btAdicionarCarrinho.CommandArgument = produto.CODIGO.ToString();

                        var produtoNoCarrinho = desenvController.ObterCarrinhoProduto(produto.CODIGO, ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);
                        if (produtoNoCarrinho != null)
                            e.Row.BackColor = Color.PaleGreen;

                        //Popular GRID VIEW FILHO
                        if (produto.FOTO1 != null && produto.FOTO1.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                var _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = produto.CODIGO, FOTO = produto.FOTO1, FOTO2 = produto.FOTO2 });
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
            if (Session[PROD_ACAB_PROD_PEDCOMPRA] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTOV2Result> produtoAcabado = (IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTOV2Result>)Session[PROD_ACAB_PROD_PEDCOMPRA];

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

                        System.Web.UI.WebControls.Image _imgFotoPeca2 = e.Row.FindControl("imgFotoPeca2") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca2 != null)
                            _imgFotoPeca2.ImageUrl = _produto.FOTO2;
                    }
                }
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

                    var fornecedor = ddlFabricante.SelectedValue.Trim();

                    if (gvCarrinho.Rows.Count > 0)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_editv2.aspx?f=" + fornecedor + "');";
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

        private List<SP_OBTER_PRODUTO_ACABADO_PRODUTOV2Result> ObterProdutoAcabadoProdutoV2(string colecao, int? desenvOrigem, string grupo, string produto)
        {

            var produtoAcabadoProduto = desenvController.ObterProdutoAcabadoProdutoV2(colecao, desenvOrigem, grupo, produto);

            if (ddlFabricante.SelectedValue != "")
                produtoAcabadoProduto = produtoAcabadoProduto.Where(p => p.FORNECEDOR.Trim() == ddlFabricante.SelectedValue.Trim()).ToList();

            if (ddlGriffe.SelectedValue != "")
                produtoAcabadoProduto = produtoAcabadoProduto.Where(p => p.GRIFFE.Contains(ddlGriffe.SelectedValue.Trim())).ToList();


            return produtoAcabadoProduto;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            int? desenvOrigem = null;

            try
            {
                labMsg.Text = "";

                if (ddlColecoes.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Coleção.";
                    return;
                }

                if (ddlOrigem.SelectedValue != "0")
                    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

                CarregarCarrinho();
                CarregarProdutoAcabado(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim().ToUpper());
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
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

                Button bt = (Button)(sender);
                GridViewRow row = (GridViewRow)bt.NamingContainer;

                int codigoProduto = Convert.ToInt32(bt.CommandArgument);
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var produtoNoCarrinho = desenvController.ObterCarrinhoProduto(codigoProduto, codigoUsuario);
                if (produtoNoCarrinho != null)
                {
                    labPedido.Text = "Produto já foi adicionado.";
                    return;
                }

                var carrinho = new DESENV_PRODUTO_CARRINHOV2
                {
                    DESENV_PRODUTO = codigoProduto,
                    USUARIO = codigoUsuario,
                    GRADE1 = 0,
                    GRADE2 = 0,
                    GRADE3 = 0,
                    GRADE4 = 0,
                    GRADE5 = 0,
                    GRADE6 = 0,
                    GRADE7 = 0,
                    GRADE8 = 0,
                    GRADE9 = 0,
                    GRADE10 = 0,
                    GRADE11 = 0,
                    GRADE12 = 0,
                    GRADE13 = 0,
                    GRADE14 = 0,
                    GRADE_TOTAL = 0,
                    DATA_INCLUSAO = DateTime.Now,
                    ETI_COMPOSICAO = true,
                    ETI_BARRA = true,
                    TAG = false,
                    AVIAMENTO = false
                };
                desenvController.InserirCarrinhoProduto(carrinho);


                row.BackColor = Color.PaleGreen;
                row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");

                CarregarCarrinho();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CarregarCarrinho()
        {
            var carrinho = desenvController.ObterCarrinhoProdutoPorUsuario(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO);
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO_CARRINHOV2 carrinho = e.Row.DataItem as DESENV_PRODUTO_CARRINHOV2;

                    if (carrinho != null)
                    {
                        var produto = desenvController.ObterProduto(carrinho.DESENV_PRODUTO);

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = produto.MODELO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = baseController.BuscaProduto(produto.MODELO).DESC_PRODUTO.Trim();

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = prodController.ObterCoresBasicas(produto.COR).DESC_COR;

                        Literal litRefFabricante = e.Row.FindControl("litFornecedor") as Literal;
                        litRefFabricante.Text = produto.FORNECEDOR;

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = (produto.CUSTO == null) ? "" : ("R$ " + produto.CUSTO.ToString());

                        var produtoLinx = baseController.BuscaProduto(produto.MODELO);

                        DropDownList ddlGrade = e.Row.FindControl("ddlGrade") as DropDownList;
                        TextBox txtGrade = e.Row.FindControl("txtGrade") as TextBox;
                        txtGrade.Visible = false;
                        ddlGrade.Visible = false;
                        if (produto.GRADE != null && produto.GRADE.Trim() == "UNICO")
                        {
                            txtGrade.Visible = true;
                            txtGrade.Text = carrinho.GRADE1.ToString();
                        }
                        else
                        {
                            ddlGrade.Visible = true;
                            var grade = desenvController.ObterGradeProduto(produtoLinx.GRADE.Trim());
                            if (grade != null && grade.Count() > 0)
                            {
                                grade.Insert(0, new DESENV_PRODUTO_GRADE { CODIGO = 0, NOME = "" });
                                ddlGrade.DataSource = grade;
                                ddlGrade.DataBind();

                                if (carrinho.DESENV_PRODUTO_GRADE != null)
                                    ddlGrade.SelectedValue = carrinho.DESENV_PRODUTO_GRADE.ToString();
                            }
                        }

                        Button _btAdicionarGrade = e.Row.FindControl("btAdicionarGrade") as Button;
                        if (_btAdicionarGrade != null)
                            _btAdicionarGrade.CommandArgument = carrinho.CODIGO.ToString();

                        Button _btExcluirCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as Button;
                        if (_btExcluirCarrinho != null)
                            _btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString();

                        if (carrinho.GRADE_TOTAL > 0 && produto.CUSTO > 0)
                            e.Row.BackColor = Color.PaleGreen;


                        CheckBox cbComposicao = e.Row.FindControl("cbComposicao") as CheckBox;
                        if (carrinho.ETI_COMPOSICAO)
                            cbComposicao.Checked = true;

                        CheckBox cbPreco = e.Row.FindControl("cbPreco") as CheckBox;
                        if (carrinho.ETI_BARRA)
                            cbPreco.Checked = true;

                        CheckBox cbTAG = e.Row.FindControl("cbTAG") as CheckBox;
                        if (carrinho.TAG)
                            cbTAG.Checked = true;

                        CheckBox cbAviamento = e.Row.FindControl("cbAviamento") as CheckBox;
                        if (carrinho.AVIAMENTO)
                            cbAviamento.Checked = true;

                        TextBox txtObs = e.Row.FindControl("txtObs") as TextBox;
                        txtObs.Text = carrinho.OBS;

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void btExcluirItemCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)(sender);
                int codigoCarrinho = Convert.ToInt32(bt.CommandArgument);
                desenvController.ExcluirCarrinhoProduto(codigoCarrinho);

                CarregarCarrinho();
            }
            catch (Exception)
            {
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
                    _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_gradev2.aspx?c=" + codigoCarrinho.ToString() + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btExcluirCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                desenvController.ExcluirCarrinhoProdutoPorUsuario(codigoUsuario);
                CarregarCarrinho();
            }
            catch (Exception)
            {
            }
        }
        protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = (DropDownList)sender;

                int codigoGrade = Convert.ToInt32(ddl.SelectedValue);

                GridViewRow row = (GridViewRow)ddl.NamingContainer;
                int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex].Value);

                //atualizar grade do produto no carrinho
                if (codigoGrade > 0)
                {
                    var carrinho = desenvController.ObterCarrinhoProduto(codigoItemCarrinho);
                    var grade = desenvController.ObterGradeProduto(codigoGrade);

                    carrinho.DESENV_PRODUTO_GRADE = codigoGrade;
                    carrinho.GRADE1 = grade.GRADE1;
                    carrinho.GRADE2 = grade.GRADE2;
                    carrinho.GRADE3 = grade.GRADE3;
                    carrinho.GRADE4 = grade.GRADE4;
                    carrinho.GRADE5 = grade.GRADE5;
                    carrinho.GRADE6 = grade.GRADE6;
                    carrinho.GRADE7 = grade.GRADE7;
                    carrinho.GRADE8 = grade.GRADE8;
                    carrinho.GRADE9 = grade.GRADE9;
                    carrinho.GRADE10 = grade.GRADE10;
                    carrinho.GRADE11 = grade.GRADE11;
                    carrinho.GRADE12 = grade.GRADE12;
                    carrinho.GRADE13 = grade.GRADE13;
                    carrinho.GRADE14 = grade.GRADE14;
                    carrinho.GRADE_TOTAL = (
                        carrinho.GRADE1 +
                        carrinho.GRADE2 +
                        carrinho.GRADE3 +
                        carrinho.GRADE4 +
                        carrinho.GRADE5 +
                        carrinho.GRADE6 +
                        carrinho.GRADE7 +
                        carrinho.GRADE8 +
                        carrinho.GRADE9 +
                        carrinho.GRADE10 +
                        carrinho.GRADE11 +
                        carrinho.GRADE12 +
                        carrinho.GRADE13 +
                        carrinho.GRADE14
                        );

                    desenvController.AtualizarCarrinhoProduto(carrinho);

                }

            }
            catch (Exception)
            {
            }
        }
        protected void btCadastrarGrade_Click(object sender, EventArgs e)
        {
            try
            {
                //Abrir pop-up
                var _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_grade_cadastrov2.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void txtGrade_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            GridViewRow row = (GridViewRow)txt.NamingContainer;
            int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex].Value);

            //atualizar grade do produto no carrinho
            int qtdeGrade = (txt.Text.Trim() == "") ? 0 : Convert.ToInt32(txt.Text.Trim());

            var carrinho = desenvController.ObterCarrinhoProduto(codigoItemCarrinho);

            carrinho.DESENV_PRODUTO_GRADE = null;
            carrinho.GRADE1 = qtdeGrade;
            carrinho.GRADE2 = 0;
            carrinho.GRADE3 = 0;
            carrinho.GRADE4 = 0;
            carrinho.GRADE5 = 0;
            carrinho.GRADE6 = 0;
            carrinho.GRADE7 = 0;
            carrinho.GRADE8 = 0;
            carrinho.GRADE9 = 0;
            carrinho.GRADE10 = 0;
            carrinho.GRADE11 = 0;
            carrinho.GRADE12 = 0;
            carrinho.GRADE13 = 0;
            carrinho.GRADE14 = 0;
            carrinho.GRADE_TOTAL = (
                carrinho.GRADE1 +
                carrinho.GRADE2 +
                carrinho.GRADE3 +
                carrinho.GRADE4 +
                carrinho.GRADE5 +
                carrinho.GRADE6 +
                carrinho.GRADE7 +
                carrinho.GRADE8 +
                carrinho.GRADE9 +
                carrinho.GRADE10 +
                carrinho.GRADE11 +
                carrinho.GRADE12 +
                carrinho.GRADE13 +
                carrinho.GRADE14
                );

            desenvController.AtualizarCarrinhoProduto(carrinho);


        }

        protected void cbItens_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;

                int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex].Value);

                //atualizar grade do produto no carrinho
                if (codigoItemCarrinho > 0)
                {

                    var carrinho = desenvController.ObterCarrinhoProduto(codigoItemCarrinho);

                    if (cb.ID.ToLower().Contains("composicao"))
                        carrinho.ETI_COMPOSICAO = cb.Checked;

                    else if (cb.ID.ToLower().Contains("preco"))
                        carrinho.ETI_BARRA = cb.Checked;

                    else if (cb.ID.ToLower().Contains("tag"))
                        carrinho.TAG = cb.Checked;

                    else if (cb.ID.ToLower().Contains("aviamento"))
                        carrinho.AVIAMENTO = cb.Checked;

                    desenvController.AtualizarCarrinhoProduto(carrinho);
                }
            }
            catch (Exception)
            {
            }
        }

        protected void txtObs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;

                int codigoItemCarrinho = Convert.ToInt32(gvCarrinho.DataKeys[row.RowIndex].Value);

                //atualizar grade do produto no carrinho
                if (codigoItemCarrinho > 0)
                {
                    var carrinho = desenvController.ObterCarrinhoProduto(codigoItemCarrinho);
                    carrinho.OBS = txt.Text;
                    desenvController.AtualizarCarrinhoProduto(carrinho);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

