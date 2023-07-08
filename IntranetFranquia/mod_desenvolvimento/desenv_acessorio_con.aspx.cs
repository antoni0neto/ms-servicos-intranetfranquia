using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class desenv_acessorio_con : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

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
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                CarregarColecoes();
                CarregarGrupo();
                CarregarFornecedor("", "");
                CarregarGriffe();
            }

            btAdicionar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAdicionar, null) + ";");
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
        }

        private StringBuilder MontarPocket(List<DESENV_ACESSORIO> listaAcessorios, bool print)
        {
            StringBuilder b = new StringBuilder();

            //Carregar produtos
            List<PocketEntity> pocket = new List<PocketEntity>();
            if (listaAcessorios != null && listaAcessorios.Count > 0)
            {
                string produto = listaAcessorios[0].PRODUTO.Trim().ToUpper();
                int _index = 0;
                int _cores = 0;

                foreach (DESENV_ACESSORIO acess in listaAcessorios)
                {
                    _cores += 1;
                    if (produto == acess.PRODUTO.Trim().ToUpper() && _cores < 4)
                    {
                        //adicionar produto
                        pocket.Add(new PocketEntity { index = _index, qtdeCopia = ObterQtdeCopia(acess.PRODUTO.Trim().ToUpper()), acessorio = acess });
                        //obter modelo
                        produto = acess.PRODUTO.Trim().ToUpper();
                    }
                    else
                    {
                        _index += 1;
                        _cores = 1;
                        pocket.Add(new PocketEntity { index = _index, qtdeCopia = ObterQtdeCopia(acess.PRODUTO.Trim().ToUpper()), acessorio = acess });
                        produto = acess.PRODUTO.Trim().ToUpper();
                    }
                }
            }

            if (pocket.Count > 0)
            {

                int loop = pocket.Select(i => i.index).Distinct().Count();

                int count = 0;
                List<DESENV_ACESSORIO> acess = new List<DESENV_ACESSORIO>();

                //montar cabeçalho
                b = Pocket.MontarCabecalho(b, print);
                b = Pocket.AbreLinha(b, false, "dInicio");

                int qtdeCopia = 1;
                for (int i = 0; i < loop; i++)
                {
                    List<PocketEntity> pocketProduto = pocket.Where(j => j.index == i).ToList();
                    qtdeCopia = (pocketProduto != null && pocketProduto.Count > 0) ? pocketProduto[0].qtdeCopia : 0;

                    while (qtdeCopia > 0)
                    {
                        count += 1;

                        b = Pocket.MontarConteudoAcessorio(b, pocketProduto.Select(j => j.acessorio).ToList());

                        if (count <= 2)
                            b = Pocket.ColunaVazia(b, 0, false);

                        if (count == 3)
                        {
                            //Fecha linha
                            b = Pocket.FechaLinha(b);

                            //Pula Linha
                            if (!print)
                            {
                                b = Pocket.AbreLinha(b, false, ("dHR" + i.ToString()));
                                b = Pocket.ColunaVazia(b, 5, true);
                                b = Pocket.FechaLinha(b);
                                b = Pocket.AbreLinha(b, false, ("dES" + i.ToString()));
                                b = Pocket.ColunaVazia(b, 5, false);
                                b = Pocket.FechaLinha(b);
                            }

                            //Abre nova linha
                            b = Pocket.AbreLinha(b, true, ("dLinha" + i.ToString()));
                            count = 0;
                        }

                        qtdeCopia -= 1;
                    }

                }

                //montar rodape
                b = Pocket.FechaLinha(b);
                b = Pocket.MontarRodape(b);
            }

            return b;
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            HtmlGenericControl divPocket;
            try
            {
                //Descarregar hMTL na DIV
                divPocket = new HtmlGenericControl();
                divPocket.InnerHtml = MontarPocket(ObterAcessorio(), false).ToString();
                pnlPocket.Controls.Add(divPocket);

                Session["COLECAO"] = ddlColecoes.SelectedValue;
            }
            catch (Exception ex)
            {
                labBuscar.Text = "ERRO: " + ex.Message;
            }
        }
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            labBuscar.Text = "";

            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "POCKET_IMP_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarPocket(ObterAcessorio(), true).ToString());
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

            }
            catch (Exception ex)
            {
                labBuscar.Text = "ERRO: " + ex.Message;
            }
            finally
            {
                wr.Close();
            }
        }

        private int ObterQtdeCopia(string produto)
        {
            int qtde = 0;

            foreach (ListItem item in lstProdutoSelecionado.Items)
                if (item.Value == produto)
                    qtde += 1;

            return qtde;
        }
        private List<DESENV_ACESSORIO> ObterAcessorio()
        {

            List<DESENV_ACESSORIO> listaAcessorios = new List<DESENV_ACESSORIO>();

            List<DESENV_ACESSORIO> acessorio = null;
            foreach (ListItem item in lstProdutoSelecionado.Items)
            {
                if (item != null)
                {
                    acessorio = new List<DESENV_ACESSORIO>();
                    acessorio = desenvController.ObterAcessorio(ddlColecoes.SelectedValue.Trim()).Where(p =>
                                                                        p.PRODUTO == item.Value.ToUpper() &&
                                                                        p.STATUS == 'A').ToList();
                    if (listaAcessorios.Where(p => p.PRODUTO == acessorio[0].PRODUTO).Count() <= 0)
                        listaAcessorios.AddRange(acessorio);
                }
            }

            //ordenação
            if (chkGrupoOrder.Checked)
                listaAcessorios = listaAcessorios.OrderBy(i => i.GRUPO).ThenBy(j => j.PRODUTO).ThenBy(j => j.COR).ToList();
            else
                listaAcessorios = listaAcessorios.OrderBy(i => i.PRODUTO).ThenBy(j => j.COR).ToList();

            return listaAcessorios;
        }

        #region "CONTROLES LISTA"
        protected void btAdicionar_Click(object sender, EventArgs e)
        {
            labBuscar.Text = "";

            if (ddlColecoes.SelectedValue.Trim() == "")
            {
                labBuscar.Text = "Selecione a Coleção.";
                return;
            }

            //if (ddlOrigem.SelectedValue.Trim() == "" || ddlOrigem.SelectedValue.Trim() == "0")
            //{
            //    labBuscar.Text = "Selecione a Origem da Coleção";
            //    return;
            //}

            List<DESENV_ACESSORIO> listaAcessorios = new List<DESENV_ACESSORIO>();

            //Obter apenas produtos que tenham acessorios e estão ativos
            listaAcessorios = desenvController.ObterAcessorio(ddlColecoes.SelectedValue.Trim()).Where(p => p.PRODUTO.Trim() != ""
                && p.GRUPO.Trim() != ""
                && p.STATUS == 'A'
                ).ToList();

            //Filtrar por Origem
            if (ddlOrigem.SelectedValue.Trim() != "0" && ddlOrigem.SelectedValue.Trim() != "")
                listaAcessorios = listaAcessorios.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == ddlOrigem.SelectedValue).ToList();

            //Filtrar por grupo
            if (ddlGrupo.SelectedValue.Trim() != "")
                listaAcessorios = listaAcessorios.Where(p => p.GRUPO.Trim() == ddlGrupo.SelectedValue.Trim()).ToList();

            //Filtrar por Produto
            if (txtProduto.Text.Trim() != "")
                listaAcessorios = listaAcessorios.Where(p => p.PRODUTO.Trim().ToUpper().Contains(txtProduto.Text.Trim().ToUpper())).ToList();

            //Filtrar por nome
            if (txtNome.Text.Trim() != "")
            {
                var produtoFiltro = baseController.BuscaProdutosDescricao(txtNome.Text.ToUpper().Trim());
                listaAcessorios = listaAcessorios.Where(p => produtoFiltro.Any(x => x.PRODUTO1.Trim() == p.PRODUTO.Trim())).ToList();
            }

            //Filtrar  Fornecedor
            if (ddlFornecedor.SelectedValue.Trim() != "")
                listaAcessorios = listaAcessorios.Where(p => p.FORNECEDOR != null && p.FORNECEDOR.Trim().Contains(ddlFornecedor.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por griffe
            if (ddlGriffe.SelectedValue.Trim() != "")
                listaAcessorios = listaAcessorios.Where(p => p.GRIFFE != null && p.GRIFFE.Trim().Contains(ddlGriffe.SelectedValue.Trim().ToUpper())).ToList();

            //Se multiplos, agrupar
            if (chkMultiplo.Checked)
                listaAcessorios = listaAcessorios.GroupBy(p => new { GRUPO = p.GRUPO, PRODUTO = p.PRODUTO }).Select(
                                                                                            k => new DESENV_ACESSORIO
                                                                                            {
                                                                                                GRUPO = k.Key.GRUPO,
                                                                                                PRODUTO = k.Key.PRODUTO
                                                                                            }).ToList();

            ListItem item = null;
            string _nome = "";
            foreach (DESENV_ACESSORIO acess in listaAcessorios)
            {
                item = lstProdutoSelecionado.Items.FindByValue(acess.PRODUTO.Trim());
                if (item == null || (item != null && chkMultiplo.Checked))
                {
                    item = new ListItem();
                    item.Value = acess.PRODUTO.Trim();
                    var produtoLinx = baseController.BuscaProduto(acess.PRODUTO);
                    if (produtoLinx != null)
                        _nome = produtoLinx.DESC_PRODUTO.Trim().Replace(acess.GRUPO.Trim(), "");
                    item.Text = acess.GRUPO.Trim() + " - " + acess.PRODUTO.Trim() + ((_nome == "") ? "" : (" - " + _nome));
                    lstProdutoSelecionado.Items.Add(item);
                    _nome = "";
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            foreach (int indexSelected in lstProdutoSelecionado.GetSelectedIndices())
            {
                lstProdutoSelecionado.Items.RemoveAt(indexSelected);
            }
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            lstProdutoSelecionado.Items.Clear();
        }
        #endregion

        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
            }
            lstProdutoSelecionado.Items.Clear();
        }
        protected void ddlOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string origem = ddlOrigem.SelectedValue.Trim();
            lstProdutoSelecionado.Items.Clear();
        }
        private void CarregarColecoes()
        {
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
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

        private void CarregarFornecedor(string colecao, string origem)
        {
            List<DESENV_ACESSORIO> fornecedorFiltro = new List<DESENV_ACESSORIO>();

            var _fornecedor = desenvController.ObterDesenvolvimentoColecaoAcessorio(ddlColecoes.SelectedValue).Where(p => p.FORNECEDOR != null && p.STATUS == 'A');

            if (colecao != "")
                _fornecedor = _fornecedor.Where(p => p.COLECAO != null && p.COLECAO.Trim() == colecao.Trim()).ToList();

            if (origem != "")
                _fornecedor = _fornecedor.Where(p => p.DESENV_PRODUTO_ORIGEM != null && p.DESENV_PRODUTO_ORIGEM.ToString() == origem).ToList();

            var _fornecedorAux = _fornecedor.Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

            foreach (var item in _fornecedorAux)
                if (item.Trim() != "")
                    fornecedorFiltro.Add(new DESENV_ACESSORIO { FORNECEDOR = item.Trim() });

            fornecedorFiltro = fornecedorFiltro.OrderBy(p => p.FORNECEDOR).ToList();
            fornecedorFiltro.Insert(0, new DESENV_ACESSORIO { FORNECEDOR = "" });
            ddlFornecedor.DataSource = fornecedorFiltro;
            ddlFornecedor.DataBind();
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (baseController.BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        #endregion
    }
}
