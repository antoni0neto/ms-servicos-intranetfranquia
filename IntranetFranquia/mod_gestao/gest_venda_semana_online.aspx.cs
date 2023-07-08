using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class gest_venda_semana_online : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();
        ProducaoController prodController = new ProducaoController();

        int gQtdeVenda = 0;
        int gEstoqueLoja = 0;
        int gEstoqueFabrica = 0;

        int gQtdeVendaSemaana = 0;

        List<SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult> gVendaProduto = new List<SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult>();
        const string VENDA_PRODUTO_SEMANA = "VENDA_PRODUTO_SEMANA_ONLINE1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gest_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";

                CarregarSemanas();
                CarregarColecoes();
                CarregarColecoesLinx();
                CarregarSubGrupo();
                CarregarGriffe();
                CarregarGrupo("01");
                CarregarFornecedor("01");
                CarregarSignedNome();
                CarregarTecido("", "");
                CarregarCorFornecedor("", "", "");
                Session["COLECAO"] = null;

                Session[VENDA_PRODUTO_SEMANA] = null;

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btExcel.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btExcel, null) + ";");
            btLimparRemarcacao.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btLimparRemarcacao, null) + ";");
        }

        private List<SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult> ObterVendaProdutoSemanaOnline(DateTime dataIni, string colecao, int desenvOrigem, string grupoProduto, string produto, string nome, string tecido, string corFornecedor, string griffe, string produtoAcabado, string signed, string signedNome, string fornecedor, string colecaoLINX, string subGrupoProduto)
        {

            var colecaoLinxCon = "";
            if (lstColecao.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstColecao.GetSelectedIndices())
                {
                    var item = lstColecao.Items[v].Value.Trim() + ",";
                    colecaoLinxCon = colecaoLinxCon + item;
                }

                colecaoLinxCon = colecaoLinxCon + ",";
                colecaoLinxCon = colecaoLinxCon.Replace(",,", "");
            }

            var griffeCon = "";
            if (lstGriffe.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGriffe.GetSelectedIndices())
                {
                    var item = lstGriffe.Items[v].Value.Trim() + ",";
                    griffeCon = griffeCon + item;
                }

                griffeCon = griffeCon + ",";
                griffeCon = griffeCon.Replace(",,", "");
            }

            var grupoProdutoCon = "";
            if (lstGrupoProduto.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGrupoProduto.GetSelectedIndices())
                {
                    var item = lstGrupoProduto.Items[v].Value.Trim() + ",";
                    grupoProdutoCon = grupoProdutoCon + item;
                }

                grupoProdutoCon = grupoProdutoCon + ",";
                grupoProdutoCon = grupoProdutoCon.Replace(",,", "");
            }

            gVendaProduto = lojaController.ObterVendaProdutoSemanaOnline(dataIni, colecao, desenvOrigem, grupoProdutoCon, produto, nome, tecido, corFornecedor, griffeCon, produtoAcabado, signed, signedNome, fornecedor, colecaoLinxCon, subGrupoProduto);


            if (txtPrecoIni.Text != "")
                gVendaProduto = gVendaProduto.Where(p => p.PRECO >= Convert.ToDecimal(txtPrecoIni.Text)).ToList();

            if (txtPrecoFim.Text != "")
                gVendaProduto = gVendaProduto.Where(p => p.PRECO <= Convert.ToDecimal(txtPrecoFim.Text)).ToList();

            if (txtQtdeVendidaIni.Text != "")
                gVendaProduto = gVendaProduto.Where(p => p.QTDE_VENDIDA >= Convert.ToInt32(txtQtdeVendidaIni.Text)).ToList();

            if (txtQtdeVendidaFim.Text != "")
                gVendaProduto = gVendaProduto.Where(p => p.QTDE_VENDIDA <= Convert.ToInt32(txtQtdeVendidaFim.Text)).ToList();

            if (txtEstoqueIni.Text != "")
                gVendaProduto = gVendaProduto.Where(p => p.ESTOQUE_LOJA >= Convert.ToInt32(txtEstoqueIni.Text)).ToList();

            if (txtEstoqueFim.Text != "")
                gVendaProduto = gVendaProduto.Where(p => p.ESTOQUE_LOJA <= Convert.ToInt32(txtEstoqueFim.Text)).ToList();

            Session[VENDA_PRODUTO_SEMANA] = gVendaProduto;

            return gVendaProduto;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                labErroRem.Text = "";

                if (ddlSemana.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Semana.";
                    return;
                }

                // validar um filtro pelo menos
                if (!ValidarFiltroSelecionado())
                {
                    labErro.Text = "É obrigatório informar pelo menos um filtro.";
                    return;
                }

                int desenvOrigem = 0;
                if (ddlOrigem.SelectedValue != "")
                    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

                var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));

                var produtos = ObterVendaProdutoSemanaOnline(semanaSel.DATA_INI, ddlColecoes.SelectedValue.Trim(), desenvOrigem, "", txtModelo.Text.Trim(), txtNome.Text.Trim(), ddlTecido.SelectedValue, ddlCorFornecedor.SelectedValue, "", ddlProdutoAcabado.SelectedValue, ddlSigned.SelectedValue, ddlSignedNome.SelectedValue, ddlFornecedor.SelectedValue.Trim(), "", ddlSubGrupoProduto.SelectedValue.Trim());

                gvProduto.DataSource = produtos;
                gvProduto.DataBind();

                if (produtos.Count <= 0)
                {
                    labErro.Text = "Nenhum produto encontrado.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private bool ValidarFiltroSelecionado()
        {

            if (
                ddlColecoes.SelectedValue == "" &&
                (ddlOrigem.SelectedValue == "" || ddlOrigem.SelectedValue == "0") &&
                lstGrupoProduto.GetSelectedIndices().Count() <= 0 &&
                txtModelo.Text == "" &&
                txtNome.Text == "" &&
                ddlTecido.SelectedValue == "" &&
                ddlCorFornecedor.SelectedValue == "" &&
                lstGriffe.GetSelectedIndices().Count() <= 0 &&
                ddlProdutoAcabado.SelectedValue == "" &&
                ddlSigned.SelectedValue == "" &&
                ddlSignedNome.SelectedValue == "" &&
                ddlFornecedor.SelectedValue == "" &&
                lstColecao.GetSelectedIndices().Count() <= 0
                )
            {
                return false;
            }

            return true;
        }

        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult prod = e.Row.DataItem as SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult;

                    gQtdeVendaSemaana = 0;

                    ImageButton _imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                    _imgProduto.CommandArgument = prod.CODIGO_DESENV_PRODUTO.ToString() + "|" + prod.CODIGO_DESENV_ACESSORIO.ToString();
                    if (File.Exists(Server.MapPath(prod.FOTO1)))
                        _imgProduto.ImageUrl = prod.FOTO1;
                    else if (File.Exists(Server.MapPath(prod.FOTO2)))
                        _imgProduto.ImageUrl = prod.FOTO2;
                    else if (File.Exists(Server.MapPath(prod.FOTO3)))
                        _imgProduto.ImageUrl = prod.FOTO3;
                    else
                        _imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                    Literal litDataInicial = e.Row.FindControl("litDataInicial") as Literal;
                    litDataInicial.Text = prod.DATA_INICIAL.ToString("dd/MM/yyyy");

                    Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                    litCusto.Text = "R$ " + Convert.ToDecimal(prod.CUSTO).ToString("###,###,###,##0.00");

                    Literal litPreco = e.Row.FindControl("litPreco") as Literal;
                    litPreco.Text = "R$ " + Convert.ToDecimal(prod.PRECO).ToString("###,###,###,##0.00");

                    TextBox txPrecoRemarc = e.Row.FindControl("txPrecoRemarc") as TextBox;
                    var produtoRem = lojaController.ObterLojaVendaRemarc(prod.PRODUTO);
                    if (produtoRem != null)
                        txPrecoRemarc.Text = produtoRem.PRECO.ToString();

                    Literal litAbrirProduto = e.Row.FindControl("litAbrirProduto") as Literal;
                    litAbrirProduto.Text = CriarLinkVendaFilial(prod.PRODUTO, prod.COR, Convert.ToDateTime(prod.DATA_INI), Convert.ToDateTime(prod.DATA_FIM));

                    gQtdeVenda += prod.QTDE_VENDIDA;
                    gEstoqueLoja += prod.ESTOQUE_LOJA;
                    gEstoqueFabrica += prod.ESTOQUE_FAB;

                    ImageButton _ibtImprimir = e.Row.FindControl("ibtImprimir") as ImageButton;
                    _ibtImprimir.CommandArgument = prod.PRODUTO;
                    if (prod.TIPO_PRODUTO == "01")
                    {
                        if (((Convert.ToInt32(prod.PRODUTO.Substring(0, 1)) % 2) == 0)) // PRODUTO NACIONAL
                        {
                            var precoAprovado = prodController.ObterProdutoPrecoAprovado("", prod.PRODUTO, "", "", "", "", null).FirstOrDefault();
                            if (precoAprovado != null && precoAprovado.SIMULACAO == 'S')
                            {
                                if (!precoAprovado.REVENDA)
                                    _ibtImprimir.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        _ibtImprimir.Visible = false;
                    }

                    //Historico
                    gQtdeVendaSemaana += prod.QTDE_VENDIDA;
                    var semanaHistorico = lojaController.ObterVendaProdutoSemanaOnlineHistorico(prod.PRODUTO, prod.COR, prod.DATA_INICIAL).Where(p => p.DATA_INI != prod.DATA_INI);
                    if (semanaHistorico != null && semanaHistorico.Count() > 0)
                    {
                        GridView gvProdutoHistorico = e.Row.FindControl("gvProdutoHistorico") as GridView;
                        gvProdutoHistorico.DataSource = semanaHistorico.OrderByDescending(p => p.QTDE_SEMANA);
                        gvProdutoHistorico.DataBind();

                        // o valor desta coluna é substituida pq a semanaHistorico conta TODAS as semanas, enquanto na tabela historico, esta faltando semanas... trazendo o numero errado...
                        e.Row.Cells[5].Text = (semanaHistorico.Count() + 1).ToString();
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
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProduto.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[3].Text = "Total";
                _footer.Cells[3].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[11].Text = gQtdeVenda.ToString();
                _footer.Cells[12].Text = gEstoqueLoja.ToString();
                _footer.Cells[13].Text = gEstoqueFabrica.ToString();

            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[VENDA_PRODUTO_SEMANA] != null)
            {

                IEnumerable<SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult> produtos = (IEnumerable<SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult>)Session[VENDA_PRODUTO_SEMANA];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                produtos = produtos.OrderBy(e.SortExpression + sortDirection);
                gvProduto.DataSource = produtos;
                gvProduto.DataBind();
            }

        }
        protected void imgProduto_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    var codigos = b.CommandArgument.Split('|');
                    var codigoProduto = (codigos[0] == "") ? "" : codigos[0].ToString();
                    var codigoAcessorio = (codigos[1] == "") ? "" : codigos[1].ToString();

                    if (codigoProduto != "" && codigoProduto != "0")
                        _url = "fnAbrirTelaCadastroMaiorVert('../mod_desenvolvimento/desenv_tripa_view_foto.aspx?p=" + codigoProduto + "');";
                    else if (codigoAcessorio != "" && codigoAcessorio != "0")
                        _url = "fnAbrirTelaCadastroMaiorVert('../mod_desenvolvimento/desenv_tripa_view_foto_acessorio.aspx?p=" + codigoAcessorio + "');";
                    else
                        return;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        private string CriarLinkVendaFilial(string produto, string cor, DateTime dataIni, DateTime dataFim)
        {
            var link = "gest_venda_semana_filial.aspx?p=" + produto.Trim() + "&c=" + cor + "&di=" + dataIni.ToString("yyyy-MM-dd") + "&df=" + dataFim.ToString("yyyy-MM-dd") + "";
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }
        protected void gvProdutoHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    FN_OBTER_VENDA_SEMANA_PRODUTO_ONLINEResult prod = e.Row.DataItem as FN_OBTER_VENDA_SEMANA_PRODUTO_ONLINEResult;

                    Literal litPreco = e.Row.FindControl("litPreco") as Literal;
                    litPreco.Text = "R$ " + Convert.ToDecimal(prod.PRECO).ToString("###,###,###,##0.00");

                    Literal litAbrirProduto = e.Row.FindControl("litAbrirProduto") as Literal;
                    litAbrirProduto.Text = CriarLinkVendaFilial(prod.PRODUTO, prod.COR, Convert.ToDateTime(prod.DATA_INI), Convert.ToDateTime(prod.DATA_FIM));

                    gQtdeVendaSemaana += Convert.ToInt32(prod.QTDE_VENDIDA);

                }
            }
        }
        protected void gvProdutoHistorico_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = ((GridView)sender).FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[3].Text = gQtdeVendaSemaana.ToString();

            }
        }

        #region "DADOS INICIAIS"
        private void CarregarSemanas()
        {
            var semana = lojaController.ObterSemanaVenda();

            semana.RemoveAt(0);
            semana.Insert(0, new LOJA_VENDA_SEMANA { CODIGO = 0, SEMANA = "Selecione" });

            ddlSemana.DataSource = semana;
            ddlSemana.DataBind();

            ddlSemana.SelectedIndex = 1;
        }
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                CarregarOrigem(colecao);
                CarregarTecido(colecao, "");
                CarregarCorFornecedor(colecao, "", "");
                CarregarSignedNome();
            }

        }
        protected void ddlOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string origem = ddlOrigem.SelectedValue.Trim();
            if (origem != "" && origem != "0")
            {
                CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), origem.Trim(), "");
                CarregarTecido(ddlColecoes.SelectedValue.Trim(), origem.Trim());
            }
            else
            {
                CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), "", "");
            }

        }
        protected void ddlTecido_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCorFornecedor(ddlColecoes.SelectedValue, ddlOrigem.SelectedValue, ddlTecido.SelectedValue);
        }
        private void CarregarColecoes()
        {
            var _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

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
        private void CarregarColecoesLinx()
        {
            var colecoes = baseController.BuscaColecoesLinx();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
            lstColecao.DataSource = colecoes;
            lstColecao.DataBind();

        }
        private void CarregarGrupo(string tipo)
        {
            var grupoProdutos = (prodController.ObterGrupoProduto(tipo));

            grupoProdutos = grupoProdutos.OrderBy(p => p.GRUPO_PRODUTO).ToList();
            grupoProdutos.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
            lstGrupoProduto.DataSource = grupoProdutos;
            lstGrupoProduto.DataBind();

        }
        private void CarregarSubGrupo()
        {
            var subGrupo = prodController.ObterSubGrupoProduto("", "");

            subGrupo.Insert(0, new SP_OBTER_SUBGRUPO_PRODUTOResult { SUBGRUPO_PRODUTO = "" });
            ddlSubGrupoProduto.DataSource = subGrupo;
            ddlSubGrupoProduto.DataBind();

        }
        private void CarregarOrigem(string col)
        {
            var _origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
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
        private void CarregarTecido(string colecao, string origem)
        {
            List<DESENV_PRODUTO> tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.TECIDO_POCKET != null && p.STATUS == 'A');

            if (origem != "")
                _tecido = _tecido.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == origem).ToList();

            var _tecidoAux = _tecido.Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();

            foreach (var item in _tecidoAux)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor(string colecao, string origem, string tecido)
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');

            if (origem != "")
                _corFornecedor = _corFornecedor.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == origem).ToList();

            if (tecido != "")
                _corFornecedor = _corFornecedor.Where(p => p.TECIDO_POCKET != null && p.TECIDO_POCKET.Trim() == tecido.Trim()).ToList();

            var _corFornecedorAux = _corFornecedor.Select(s => s.FORNECEDOR_COR.Trim()).Distinct().ToList();

            foreach (var item in _corFornecedorAux)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR_COR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.FORNECEDOR_COR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
            ddlCorFornecedor.DataSource = corFornecedorFiltro;
            ddlCorFornecedor.DataBind();
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                lstGriffe.DataSource = griffe;
                lstGriffe.DataBind();
            }
        }
        private void CarregarSignedNome()
        {
            List<DESENV_PRODUTO> signedNomeFiltro = new List<DESENV_PRODUTO>();

            var _signednome = desenvController.ObterDesenvolvimentoColecao(ddlColecoes.SelectedValue).Where(p => p.SIGNED_NOME != null && p.STATUS == 'A').Select(s => s.SIGNED_NOME.Trim()).Distinct().ToList();
            foreach (var item in _signednome)
                if (item.Trim() != "")
                    signedNomeFiltro.Add(new DESENV_PRODUTO { SIGNED_NOME = item.Trim() });

            signedNomeFiltro = signedNomeFiltro.OrderBy(p => p.SIGNED_NOME).ToList();
            signedNomeFiltro.Insert(0, new DESENV_PRODUTO { SIGNED_NOME = "" });
            ddlSignedNome.DataSource = signedNomeFiltro;
            ddlSignedNome.DataBind();
        }
        private void CarregarFornecedor(string tipo)
        {
            var fornecedores = desenvController.ObterFornecedorVendaProduto(tipo);
            fornecedores.Insert(0, new SP_OBTER_FORNECEDOR_VENDAResult { FORNECEDOR = "" });
            ddlFornecedor.DataSource = fornecedores;
            ddlFornecedor.DataBind();
        }

        #endregion

        #region "IMPRESSAO"
        protected void ibtImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroRem.Text = "";

                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        string colecao = "";
                        string produto = b.CommandArgument;

                        bool produtoNacional = false;
                        if ((Convert.ToInt32(produto.Substring(0, 1)) % 2) == 0)
                            produtoNacional = true;

                        bool revenda = false;
                        var produtoLinx = baseController.BuscaProduto(produto);
                        if (produtoLinx != null)
                            revenda = produtoLinx.REVENDA;

                        GerarRelatorio(colecao, produto, produtoNacional, revenda);

                        //_url = "fnAbrirTelaCadastroMaior('prod_rel_produto_preco_aprovado_imp.aspx?c=" + colecao + "&p=" + produto + "&m=" + mostruario + "')";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private PROD_HB ObterProduto(string colecao, string produto, char mostruario)
        {
            var _produto = prodController.ObterHB(produto.Trim()).Where(p => p.MOSTRUARIO == mostruario).Take(1).SingleOrDefault();
            return _produto;
        }
        private void GerarRelatorio(string colecao, string produto, bool produtoNacional, bool revenda)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PROD_PRECO_" + produto + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(colecao, produto, produtoNacional, revenda));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
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
        private StringBuilder MontarRelatorioHTML(string colecao, string produto, bool produtoNacional, bool revenda)
        {
            StringBuilder _texto = new StringBuilder();

            if (produtoNacional && !revenda)
            {
                _texto = MontarRelatorioNacional(_texto, colecao, produto);
            }
            else
            {
                _texto = MontarRelatorioImportado(_texto, produto);
            }

            return _texto;
        }

        private StringBuilder MontarRelatorioNacional(StringBuilder _texto, string colecao, string produto)
        {
            char mostruario = ' ';

            var simulacaoProducao = prodController.ObterCustoSimulacao(produto, 'N').Where(p => p.SIMULACAO == 'N');
            var simulacaoMostruario = prodController.ObterCustoSimulacao(produto, 'S').Where(p => p.SIMULACAO == 'N');
            if (simulacaoProducao != null && simulacaoProducao.Count() > 0)//PRODUTO APROVADO PRODUCAO
                mostruario = 'N';
            else if (simulacaoMostruario != null && simulacaoMostruario.Count() > 0)//PRODUTO APROVADO MOSTRUARIO
                mostruario = 'S';

            var _produto = ObterProduto(colecao, produto, mostruario);

            var griffe = "-";
            var desenvProduto = desenvController.ObterProduto(_produto.COLECAO.Trim(), _produto.CODIGO_PRODUTO_LINX.Trim()).FirstOrDefault();
            if (desenvProduto != null)
                griffe = desenvProduto.GRIFFE.Trim();

            _texto.AppendLine("");
            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Custo/Preço Nacional</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("    <style type='text/css'>");
            _texto.AppendLine("        @media print {");
            _texto.AppendLine("            .background-force {");
            _texto.AppendLine("                -webkit-print-color-adjust: exact;");
            _texto.AppendLine("            }");
            _texto.AppendLine("        }");
            _texto.AppendLine("    </style>");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload=''>");
            _texto.AppendLine("    <br />");
            _texto.AppendLine("    <div id='divNacional' align='center'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='517' style='width: 517pt;");
            _texto.AppendLine("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Coleção");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;" + baseController.BuscaColecaoAtual(_produto.COLECAO).DESC_COLECAO.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;HB");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center; width:60px;'>");
            _texto.AppendLine("                                " + _produto.HB.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Produto");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.CODIGO_PRODUTO_LINX);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Cor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + prodController.ObterCoresBasicas(_produto.COR).DESC_COR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Griffe");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;" + griffe);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Nome");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:left;' colspan='3'>");
            _texto.AppendLine("                                &nbsp;&nbsp;" + _produto.NOME);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Data");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.DATA_INCLUSAO.ToString("dd/MM/yyyy"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding:10px;' class='background-force'>");
            _texto.AppendLine("                    Tecidos");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Tecido");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Mts/Kgs");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Quantidade");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Preço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Consumo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            var lstTecidos = new List<PROD_HB>();
            //Adiciona pai na lista
            lstTecidos.Add(_produto);
            var tecidoDetalhe = new List<PROD_HB>();
            tecidoDetalhe = prodController.ObterDetalhesHB(_produto.CODIGO);
            foreach (PROD_HB det in tecidoDetalhe)
                if (det != null)
                    lstTecidos.Add(det);

            int totalGrade = 0;
            decimal gastoPorCorte = 0;
            decimal consumoTecidoPreco = 0;
            decimal totalTecido = 0;
            decimal valorTotalTecido = 0;
            decimal valorTotalCusto = 0;

            foreach (var tec in lstTecidos)
            {
                totalGrade = prodController.ObterQtdeGradeHB(tec.CODIGO, 3);

                if (tec.GASTO_PECA_CUSTO == null)
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_FOLHA * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + tec.RETALHOS) / totalGrade));
                }
                else
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_PECA_CUSTO * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte) / totalGrade));
                }

                totalTecido = totalGrade * consumoTecidoPreco;
                valorTotalTecido = consumoTecidoPreco * Convert.ToDecimal(tec.CUSTO_TECIDO);

                if (mostruario == 'S')
                    valorTotalTecido = valorTotalTecido + (valorTotalTecido * Convert.ToDecimal(10.000 / 100.000));


                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + tec.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + tec.TECIDO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + totalTecido.ToString("###,###,##0.000"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + totalGrade.ToString());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + Convert.ToDecimal(tec.CUSTO_TECIDO).ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + consumoTecidoPreco.ToString("###,###,##0.000"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalTecido.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalTecido;
            }


            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Aviamentos");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Aviamento");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Consumo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Preço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");


            var lstAviamentos = new List<SP_OBTER_HB_AVIAMENTO_CUSTOResult>();
            lstAviamentos = prodController.ObterAviamentoCusto(_produto.CODIGO);
            //var aviamentoExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'A');

            valorTotalCusto = 0;
            decimal valorTotalAviamento = 0;
            foreach (var avi in lstAviamentos)
            {

                valorTotalAviamento = Convert.ToDecimal(avi.CONSUMO * avi.PRECO);

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + avi.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + avi.AVIAMENTO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + avi.CONSUMO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + avi.PRECO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalAviamento.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalAviamento;
            }
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Serviços");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Serviço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; width: 130px;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            var lstServicos = prodController.ObterCustoServico(_produto.CODIGO_PRODUTO_LINX, Convert.ToChar(_produto.MOSTRUARIO));

            valorTotalCusto = 0;
            decimal valorTotalServico = 0;
            foreach (var ser in lstServicos)
            {

                valorTotalServico = ((ser.CUSTO_PECA != null) ? Convert.ToDecimal(ser.CUSTO_PECA) : Convert.ToDecimal(ser.CUSTO));

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + ser.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + prodController.ObterServicoProducao(ser.SERVICO).DESCRICAO.ToUpper());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalServico.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalServico;
            }
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Outros");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Item");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; width: 130px;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            var custoOrigem = prodController.ObterCustoOrigem(_produto.CODIGO_PRODUTO_LINX, mostruario, 'N').Where(p => p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 5 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 6 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 14 ||
                                                                                                                  p.COD_CUSTO_ORIGEM == 15).OrderBy(p => p.PRODUTO_CUSTO_COD_ORIGEM.DESCRICAO).ToList();

            valorTotalCusto = 0;
            foreach (var c in custoOrigem)
            {

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + c.PRODUTO_CUSTO_COD_ORIGEM.DESCRICAO.ToString());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + Convert.ToDecimal(c.CUSTO_TOTAL).ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += Convert.ToDecimal(c.CUSTO_TOTAL);
            }



            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px; text-align:center;' class='background-force'>");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                <img alt='Foto Peça' Width='180px' src='..\\.." + _produto.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td valign='top'>");
            _texto.AppendLine("                                <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td colspan='5'>Formação MARK-UP</td>");
            _texto.AppendLine("                                    </tr>");


            var lstCustoSimulacao = prodController.ObterCustoSimulacao(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO)).Where(p => p.CUSTO_MOSTRUARIO == 'N').ToList();

            List<decimal> precoProduto = new List<decimal>();
            List<decimal> markUp = new List<decimal>();
            List<decimal> lucro = new List<decimal>();
            foreach (PRODUTO_CUSTO_SIMULACAO custoS in lstCustoSimulacao)
            {
                if (custoS != null)
                {
                    precoProduto.Add(custoS.PRECO_PRODUTO);
                    markUp.Add(custoS.MARKUP);
                    lucro.Add(custoS.LUCRO_PORC);
                }
            }

            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[0].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[1].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[2].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[3].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[4].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[0].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[1].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[2].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[3].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[4].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");


            var CMV = prodController.ObterCustoOrigem(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                                        p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 3 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 5 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 6 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 14 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 15
                                                                                                                        ).Sum(p => p.CUSTO_TOTAL);


            var mkp = 0.00M;
            var precoProdutoFinal = 0.00M;
            var precoAprovadoProducao = prodController.ObterCustoSimulacaoPrecoAprovado(_produto.CODIGO_PRODUTO_LINX.Trim(), 'N');
            if (precoAprovadoProducao != null)
            {
                mkp = precoAprovadoProducao.MARKUP;
                precoProdutoFinal = precoAprovadoProducao.PRECO_PRODUTO;
            }
            else
            {
                var precoAprovadoMostruario = prodController.ObterCustoSimulacaoPrecoAprovado(_produto.CODIGO_PRODUTO_LINX.Trim(), 'S');
                if (precoAprovadoMostruario != null)
                {
                    mkp = precoAprovadoMostruario.MARKUP;
                    precoProdutoFinal = precoAprovadoMostruario.PRECO_PRODUTO;
                }
            }

            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1; text-align:left;'>&nbsp; Custo (TS)</td>");
            _texto.AppendLine("                                        <td colspan='2' style='text-align: left;'>&nbsp;&nbsp;R$ " + Convert.ToDecimal(CMV).ToString("###,###,##0.00") + "</td>");
            _texto.AppendLine("                                        <td colspan='2'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1; text-align:left;'>&nbsp; MKUP</td>");
            _texto.AppendLine("                                        <td colspan='2' style='text-align: left;'>&nbsp;&nbsp;" + (mkp.ToString("0.0")) + "</td>");
            _texto.AppendLine("                                        <td colspan='2'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");

            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1; text-align:left;'>&nbsp; Preço</td>");
            _texto.AppendLine("                                        <td colspan='2' style='text-align: left;'>&nbsp;&nbsp;R$ " + (precoProdutoFinal.ToString("###,###,##0.00")) + "</td>");
            _texto.AppendLine("                                        <td colspan='2'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");


            return _texto;
        }


        private StringBuilder MontarRelatorioNacionalZZZZZ(StringBuilder _texto, string colecao, string produto)
        {
            char mostruario = ' ';

            var simulacaoProducao = prodController.ObterCustoSimulacao(produto, 'N').Where(p => p.SIMULACAO == 'N');
            var simulacaoMostruario = prodController.ObterCustoSimulacao(produto, 'S').Where(p => p.SIMULACAO == 'N');
            if (simulacaoProducao != null && simulacaoProducao.Count() > 0)//PRODUTO APROVADO PRODUCAO
                mostruario = 'N';
            else if (simulacaoMostruario != null && simulacaoMostruario.Count() > 0)//PRODUTO APROVADO MOSTRUARIO
                mostruario = 'S';

            var _produto = ObterProduto(colecao, produto, mostruario);

            _texto.AppendLine("");
            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Custo/Preço Nacional</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("    <style type='text/css'>");
            _texto.AppendLine("        @media print");
            _texto.AppendLine("        {");
            _texto.AppendLine("            .background-force");
            _texto.AppendLine("            {");
            _texto.AppendLine("                -webkit-print-color-adjust: exact;");
            _texto.AppendLine("            }");
            _texto.AppendLine("        }");
            _texto.AppendLine("    </style>");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload=''>");
            _texto.AppendLine("    <br />");
            _texto.AppendLine("    <div id='divNacional' align='center'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='517' style='width: 517pt;");
            _texto.AppendLine("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Grupo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.GRUPO);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;HB");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.HB);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Mostruário");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + ((mostruario == 'S') ? "Sim" : "Não"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='8'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Nome");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.NOME);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Data");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.DATA_INCLUSAO.ToString("dd/MM/yyyy"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-bottom: none; border-top: none;'>&nbsp;</td>");
            _texto.AppendLine("                            <td style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                &nbsp;Código");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                " + _produto.CODIGO_PRODUTO_LINX);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding:10px;' class='background-force'>");
            _texto.AppendLine("                    Tecidos");
            _texto.AppendLine("                     <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Tecido");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Mts/Kgs");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Quantidade");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Preço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Consumo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            List<PROD_HB> lstTecidos = new List<PROD_HB>();
            //Adiciona pai na lista
            lstTecidos.Add(_produto);
            List<PROD_HB> tecidoDetalhe = new List<PROD_HB>();
            tecidoDetalhe = prodController.ObterDetalhesHB(_produto.CODIGO);
            foreach (PROD_HB det in tecidoDetalhe)
                if (det != null)
                    lstTecidos.Add(det);
            //var tecidoExtra = prodController.ObterMaterialExtra(_produto.CODIGO, 'T');

            int totalGrade = 0;
            decimal gastoPorCorte = 0;
            decimal consumoTecidoPreco = 0;
            decimal totalTecido = 0;
            decimal valorTotalTecido = 0;
            decimal valorTotalCusto = 0;
            foreach (var tec in lstTecidos)
            {
                totalGrade = prodController.ObterQtdeGradeHB(tec.CODIGO, 3);

                if (tec.GASTO_PECA_CUSTO == null)
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_FOLHA * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + tec.RETALHOS) / totalGrade));
                }
                else
                {
                    gastoPorCorte = Convert.ToDecimal(tec.GASTO_PECA_CUSTO * totalGrade);
                    consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte) / totalGrade));
                }

                totalTecido = totalGrade * consumoTecidoPreco;

                valorTotalTecido = consumoTecidoPreco * Convert.ToDecimal(tec.CUSTO_TECIDO);

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + tec.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + tec.TECIDO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + totalTecido.ToString("###,###,##0.000"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + totalGrade.ToString());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + Convert.ToDecimal(tec.CUSTO_TECIDO).ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + consumoTecidoPreco.ToString("###,###,##0.000"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalTecido.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalTecido;
            }
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Aviamentos");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Aviamento");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Consumo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Preço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            List<SP_OBTER_HB_AVIAMENTO_CUSTOResult> lstAviamentos = new List<SP_OBTER_HB_AVIAMENTO_CUSTOResult>();
            lstAviamentos = prodController.ObterAviamentoCusto(_produto.CODIGO);
            //var aviamentoExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'A');

            valorTotalCusto = 0;
            decimal valorTotalAviamento = 0;
            foreach (var avi in lstAviamentos)
            {

                valorTotalAviamento = Convert.ToDecimal(avi.CONSUMO * avi.PRECO);

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + avi.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + avi.AVIAMENTO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                " + avi.CONSUMO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + avi.PRECO);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalAviamento.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalAviamento;
            }
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px;' class='background-force'>");
            _texto.AppendLine("                    Serviços");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Fornecedor");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;Serviço");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                Total");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            var lstServicos = prodController.ObterCustoServico(_produto.CODIGO_PRODUTO_LINX, Convert.ToChar(_produto.MOSTRUARIO));

            valorTotalCusto = 0;
            decimal valorTotalServico = 0;
            foreach (var ser in lstServicos)
            {

                valorTotalServico = ((ser.CUSTO_PECA != null) ? Convert.ToDecimal(ser.CUSTO_PECA) : Convert.ToDecimal(ser.CUSTO));

                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + ser.FORNECEDOR);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;" + prodController.ObterServicoProducao(ser.SERVICO).DESCRICAO.ToUpper());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align:center;'>");
                _texto.AppendLine("                                R$ " + valorTotalServico.ToString("###,###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");

                valorTotalCusto += valorTotalServico;
            }
            _texto.AppendLine("                        <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align:center;'>");
            _texto.AppendLine("                                R$ " + valorTotalCusto.ToString("###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='padding: 10px; text-align:center;' class='background-force'>");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                <img alt='Foto Peça' Width='180px' src='..\\.." + _produto.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td valign='top'>");
            _texto.AppendLine("                                <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");

            //List<PRODUTO_CUSTO_SIMULACAO> lstCustoSimulacao = new List<PRODUTO_CUSTO_SIMULACAO>();
            //lstCustoSimulacao = prodController.ObterCustoSimulacao().Where(p =>
            //                                                            p.PRODUTO.Trim() == _produto.CODIGO_PRODUTO_LINX.Trim() &&
            //                                                            p.MOSTRUARIO == _produto.MOSTRUARIO &&
            //                                                            p.CUSTO_MOSTRUARIO == 'N').ToList();

            var lstCustoSimulacao = prodController.ObterCustoSimulacao(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO)).Where(p => p.CUSTO_MOSTRUARIO == 'N').ToList();

            List<decimal> precoProduto = new List<decimal>();
            List<decimal> markUp = new List<decimal>();
            List<decimal> lucro = new List<decimal>();
            foreach (PRODUTO_CUSTO_SIMULACAO custoS in lstCustoSimulacao)
            {
                if (custoS != null)
                {
                    precoProduto.Add(custoS.PRECO_PRODUTO);
                    markUp.Add(custoS.MARKUP);
                    lucro.Add(custoS.LUCRO_PORC);
                }
            }

            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[0].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[1].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[2].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[3].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + markUp[4].ToString("0.0"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + lucro[0].ToString("###") + "%");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + lucro[1].ToString("###") + "%");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + lucro[2].ToString("###") + "%");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + lucro[3].ToString("###") + "%");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + lucro[4].ToString("###") + "%");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='background-color:#FFE4E1;'>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[0].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[1].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[2].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[3].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            R$ " + precoProduto[4].ToString("###,###,##0.00"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1;'>Custo</td>");
            _texto.AppendLine("                                    </tr>");

            var CMV = prodController.ObterCustoOrigem(_produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(_produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                                        p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 3 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                        //p.COD_CUSTO_ORIGEM == 5 ||
                                                                                                                        p.COD_CUSTO_ORIGEM == 6
                                                                                                                        );

            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4'>&nbsp;</td>");
            _texto.AppendLine("                                        <td>R$ " + Convert.ToDecimal(CMV.Sum(p => p.CUSTO_TOTAL)).ToString("###,###,##0.00") + "</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");


            //var precoAprovado = prodController.ObterCustoSimulacao().Where(p =>
            //                                                                p.PRODUTO.Trim() == _produto.CODIGO_PRODUTO_LINX.Trim() &&
            //                                                                    //p.MOSTRUARIO == 'S' &&
            //                                                                p.SIMULACAO == 'N' &&
            //                                                                p.CUSTO_MOSTRUARIO == 'N').OrderBy(x => x.DATA_INCLUSAO).ToList();

            var precoAprovado = prodController.ObterCustoSimulacaoPrecoAprovado(_produto.CODIGO_PRODUTO_LINX.Trim());

            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='3'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1;'> Aprovado</td>");
            _texto.AppendLine("                                        <td> R$ " + (precoAprovado[precoAprovado.Count() - 1].PRECO_PRODUTO.ToString("###,###,##0.00")) + "</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='3'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1;'> Lucro %</td>");
            _texto.AppendLine("                                        <td> R$ " + (precoAprovado[precoAprovado.Count() - 1].LUCRO_PORC.ToString("###,###,##0.00")) + "%</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='3'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1;'> MKUP</td>");
            _texto.AppendLine("                                        <td> R$ " + (precoAprovado[precoAprovado.Count() - 1].MARKUP.ToString("0.0")) + "</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1;'> TT</td>");
            _texto.AppendLine("                                    </tr>");

            var tt = CMV.Where(p => p.COD_CUSTO_ORIGEM != 5);
            decimal valorTT = 0;
            if (tt != null && tt.Count() > 0)
            {
                valorTT = (tt.Sum(p => p.CUSTO_TOTAL).Value);
                valorTT = valorTT + (valorTT * Convert.ToDecimal(0.10));
                valorTT = Math.Round(valorTT, 2);
            }

            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4'>&nbsp;</td>");
            _texto.AppendLine("                                        <td>R$ " + valorTT.ToString("###,###,##0.00") + "</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='5'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='background-color:#FFE4E1;'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4'>&nbsp;</td>");
            _texto.AppendLine("                                        <td>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4'>&nbsp;</td>");
            _texto.AppendLine("                                        <td>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");



            return _texto;
        }




        private StringBuilder MontarRelatorioImportado(StringBuilder _texto, string produto)
        {

            var imp = prodController.ObterProdutoImportadoPreco(produto);

            _texto.Append("");
            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Custo/Preço Importado</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("</head>");
            _texto.Append("<body onload='window.print();'>");
            _texto.Append("    <br />");
            _texto.Append("    <div id='divImportado' align='center'>");
            _texto.Append("        <table border='1' cellpadding='0' cellspacing='0' width='517' style='width: 517pt;");
            _texto.Append("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("            background: white; white-space: nowrap;'>");
            _texto.Append("            <tr>");
            _texto.Append("                <td style='line-height:23px; width:290px; padding:30px;'>");
            _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='2' style='text-align:center'>");
            _texto.Append("                                <font size='3' color='red'>" + imp[0].DESC_PRODUTO + "</font>");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='2' style='text-align:center'>");
            _texto.Append("                                " + imp[0].PRODUTO);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                R$ " + imp[0].CUSTO);
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                " + imp[0].INFO);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                FOB");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                " + imp[0].FOB);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                QUANT");
            _texto.Append("                            </td>");
            _texto.Append("                            <td>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            int cont = 0;
            foreach (var i in imp)
            {
                _texto.Append("                        <tr>");
                _texto.Append("                            <td>" + ((cont == 0) ? "COR" : "") + "</td>");
                _texto.Append("                            <td>" + i.DESC_COR_PRODUTO + "</td>");
                _texto.Append("                        </tr>");
                cont += 1;
            }
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("                <td style=' line-height:50px;' valign='top'>");
            _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='width:110px;'>");
            _texto.Append("                                &nbsp;&nbsp;INICIAL");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: none'>");
            _texto.Append("                                &nbsp;&nbsp;" + imp[0].INICIAL);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:right; border-left: none'>");
            _texto.Append("                                " + (Math.Round(Convert.ToDouble(imp[0].MARKUP), 2).ToString()) + "&nbsp;&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr>");
            _texto.Append("                <td colspan='2'>");
            _texto.Append("                    <table border='1' width='100%'>");
            cont = 0;

            _texto.Append("                        <tr>");
            foreach (var i in imp)
            {
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                " + ((i.NUMERO_FOTO != null) ? "<img alt='Foto' src='" + i.PATH_FOTO + "' width='200' />" : "&nbsp;") + "");
                _texto.Append("                            </td>");

                if (cont == 2)
                {
                    _texto.Append("                        </tr>");
                    _texto.Append("                        <tr>");
                }

                cont += 1;
            }
            _texto.Append("                        </tr>");

            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");


            return _texto;
        }
        #endregion


        protected void btLimparRemarcacao_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroRem.Text = "";

                var produtos = lojaController.ObterLojaVendaRemarc();
                foreach (var p in produtos)
                    lojaController.ExcluirLojaVendaRemarc(p.PRODUTO);

                gvProduto.DataSource = new List<SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult>();
                gvProduto.DataBind();

                labErroRem.Text = "Todos os preços dos produtos foram excluídos.";
            }
            catch (Exception ex)
            {
                labErroRem.Text = ex.Message;
            }
        }
        protected void txPrecoRemarc_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            try
            {
                var produto = gvProduto.DataKeys[row.RowIndex].Value.ToString();
                var produtoRem = lojaController.ObterLojaVendaRemarc(produto);
                if (produtoRem == null)
                {
                    produtoRem = new LOJA_VENDA_REMARC();
                    produtoRem.PRODUTO = produto;
                    produtoRem.PRECO = Convert.ToDecimal(txt.Text.Trim());
                    lojaController.InserirLojaVendaRemarc(produtoRem);
                }
                else
                {
                    produtoRem.PRODUTO = produto;
                    produtoRem.PRECO = Convert.ToDecimal(txt.Text.Trim());
                    lojaController.AtualizarLojaVendaRemarc(produtoRem);
                }

                row.BackColor = Color.PaleGreen;
            }
            catch (Exception)
            {
                row.BackColor = Color.Red;
            }
        }


        protected void btFecharSemana_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    labErro.Text = "";
            //    labErroFechamento.Text = "";

            //    var semanaOK = lojaController.GerarVendaProdutoSemana();

            //    if (semanaOK.OK == 0)
            //    {
            //        labErroFechamento.Text = "Movimento da semana não está fechado. Entre em contato com TI.";
            //        return;
            //    }

            //    if (semanaOK.OK == -1)
            //    {
            //        labErroFechamento.Text = "Atenção! A geração de venda dos produtos desta Semana já foi gerada.";
            //        return;
            //    }

            //    if (semanaOK.OK == 1)
            //    {
            //        labErroFechamento.Text = "Atenção! Movimento de Venda de Produtos da Semana gerado com sucesso.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    labErroFechamento.Text = ex.Message;
            //}

        }
        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlSemana.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Semana.";
                    return;
                }

                //Obter comissao e gravar na lista global
                // validar um filtro pelo menos
                if (!ValidarFiltroSelecionado())
                {
                    labErro.Text = "É obrigatório informar pelo menos um filtro.";
                    return;
                }

                int desenvOrigem = 0;
                if (ddlOrigem.SelectedValue != "")
                    desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

                var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));

                var produtos = ObterVendaProdutoSemanaOnline(semanaSel.DATA_INI, ddlColecoes.SelectedValue.Trim(), desenvOrigem, "", txtModelo.Text.Trim(), txtNome.Text.Trim(), ddlTecido.SelectedValue, ddlCorFornecedor.SelectedValue, "", ddlProdutoAcabado.SelectedValue, ddlSigned.SelectedValue, ddlSignedNome.SelectedValue, ddlFornecedor.SelectedValue.Trim(), "", ddlSubGrupoProduto.SelectedValue.Trim());

                gvExcel.DataSource = produtos;
                gvExcel.DataBind();

                if (produtos != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "VENDA_PRODUTO_SEMANA_" + DateTime.Today.Day + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvExcel.AllowPaging = false;
                    gvExcel.PageSize = 1000;
                    gvExcel.DataSource = produtos;
                    gvExcel.DataBind();


                    gvExcel.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvExcel.HeaderRow.Cells.Count; i++)
                    {
                        gvExcel.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvExcel.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }
                    gvExcel.RenderControl(htw);
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void btGerarTXT_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroRem.Text = "";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "REMARCACAO_PRECO_" + DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Year.ToString() + ".txt"));
                Response.ContentType = "text/plain";
                StringWriter sw = new StringWriter();

                var produtos = lojaController.ObterLojaVendaRemarc();
                foreach (var p in produtos)
                    sw.WriteLine(p.PRODUTO.Trim() + "" + p.PRECO.ToString());

                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                labErroRem.Text = ex.Message;
            }
        }
    }
}

