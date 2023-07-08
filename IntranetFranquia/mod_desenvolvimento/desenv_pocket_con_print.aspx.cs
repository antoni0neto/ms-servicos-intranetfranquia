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
    public partial class desenv_pocket_con_print : System.Web.UI.Page
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
                CarregarTecido();
                CarregarCorFornecedor("", "");
            }
        }

        private int ObterQtdeCopia(string modelo)
        {
            int qtde = 0;

            foreach (ListItem item in lstModeloSelecionado.Items)
                if (item.Value == modelo)
                    qtde += 1;

            return qtde;
        }
        private StringBuilder MontarPocket(List<DESENV_PRODUTO> listaProdutos, bool print, bool v2)
        {
            StringBuilder b = new StringBuilder();

            //Carregar produtos
            List<PocketEntity> pocket = new List<PocketEntity>();
            if (listaProdutos != null && listaProdutos.Count > 0)
            {
                string modelo = listaProdutos[0].MODELO.Trim().ToUpper();
                int _index = 0;
                int _cores = 0;

                foreach (DESENV_PRODUTO p in listaProdutos)
                {
                    _cores += 1;
                    if (modelo == p.MODELO.Trim().ToUpper() && _cores < 4)
                    {
                        //adicionar produto
                        pocket.Add(new PocketEntity { index = _index, qtdeCopia = ObterQtdeCopia(p.MODELO.Trim().ToUpper()), produto = p });
                        //obter modelo
                        modelo = p.MODELO.Trim().ToUpper();
                    }
                    else
                    {
                        _index += 1;
                        _cores = 1;
                        pocket.Add(new PocketEntity { index = _index, qtdeCopia = ObterQtdeCopia(p.MODELO.Trim().ToUpper()), produto = p });
                        modelo = p.MODELO.Trim().ToUpper();
                    }
                }
            }

            if (pocket.Count > 0)
            {
                string corTecido = "";
                int loop = pocket.Select(i => i.index).Distinct().Count();

                int count = 0;
                List<DESENV_PRODUTO> prod = new List<DESENV_PRODUTO>();

                //montar cabeçalho
                b = Pocket.MontarCabecalho(b, print);

                //Montar Painel com Foto  do Tecido *************************************
                if (ddlCorFornecedor.SelectedValue.Trim() != "")
                {
                    b = Pocket.AbreLinha(b, false, "dHRCorTecido");
                    var cor = desenvController.ObterCorPocket(pocket[0].produto.FORNECEDOR_COR);
                    if (cor != null)
                        corTecido = cor.IMAGEM;

                    b = Pocket.MontarFotoTecido(b, corTecido);
                    b = Pocket.FechaLinha(b);

                    b = Pocket.AbreLinha(b, false, ("XXX"));
                    b = Pocket.ColunaVazia(b, 5, false);
                    b = Pocket.FechaLinha(b);
                    //FIM Montar Painel com Foto  do Tecido *********************************
                }

                b = Pocket.AbreLinha(b, false, "dInicio");

                int qtdeCopia = 1;
                for (int i = 0; i < loop; i++)
                {
                    List<PocketEntity> pocketProduto = pocket.Where(j => j.index == i).ToList();
                    qtdeCopia = (pocketProduto != null && pocketProduto.Count > 0) ? pocketProduto[0].qtdeCopia : 0;

                    while (qtdeCopia > 0)
                    {
                        count += 1;

                        b = Pocket.MontarConteudoV2(b, pocketProduto.Select(j => j.produto).ToList(), false);

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
        private List<DESENV_PRODUTO> ObterProduto()
        {

            List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();

            List<DESENV_PRODUTO> produto = null;
            foreach (ListItem item in lstModeloSelecionado.Items)
            {
                if (item != null)
                {
                    produto = new List<DESENV_PRODUTO>();
                    produto = desenvController.ObterProduto().Where(p => p.MODELO == item.Value.ToUpper() && p.STATUS == 'A').ToList();
                    if (listaProdutos.Where(p => p.MODELO == produto[0].MODELO).Count() <= 0)
                        listaProdutos.AddRange(produto);
                }
            }

            //se selecionada descricao fornecedor, nao traz as outras cores
            if (ddlCorFornecedor.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.FORNECEDOR_COR.ToUpper().Trim() == ddlCorFornecedor.SelectedValue.Trim().ToUpper()).ToList();

            //ordenação
            if (chkGrupoOrder.Checked)
                listaProdutos = listaProdutos.OrderBy(i => i.GRUPO).ThenBy(j => j.MODELO).ThenBy(j => j.COR).ToList();
            else if (chkTecidoOrder.Checked)
                listaProdutos = listaProdutos.OrderBy(i => i.TECIDO_POCKET).ThenBy(j => j.MODELO).ThenBy(j => j.COR).ToList();
            else
                listaProdutos = listaProdutos.OrderBy(i => i.MODELO).ThenBy(j => j.COR).ToList();

            return listaProdutos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            HtmlGenericControl divPocket;
            try
            {
                //Descarregar hMTL na DIV
                divPocket = new HtmlGenericControl();
                divPocket.InnerHtml = MontarPocket(ObterProduto(), false, true).ToString();
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
                wr.Write(MontarPocket(ObterProduto(), true, true).ToString());
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

        #region "CONTROLES LISTA"
        protected void btAdicionar_Click(object sender, EventArgs e)
        {
            labBuscar.Text = "";
            if (ddlColecoes.SelectedValue.Trim() == "" && (ddlOrigem.SelectedValue.Trim() == "" || ddlOrigem.SelectedValue.Trim() == "0") && ddlGrupo.SelectedValue.Trim() == "" && txtModelo.Text.Trim() == "" && txtNome.Text.Trim() == "" && ddlTecido.SelectedValue.Trim() == "" && ddlCorFornecedor.SelectedValue.Trim() == "")
            {
                labBuscar.Text = "Informe pelo menos um campo para pesquisa.";
                return;
            }

            //if (ddlColecoes.SelectedValue.Trim() != "" && (ddlOrigem.SelectedValue.Trim() == "" || ddlOrigem.SelectedValue.Trim() == "0"))
            //{
            //    labBuscar.Text = "Informe a Origem da Coleção.";
            //    return;
            //}

            List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();

            //Obter apenas produtos que tenham modelos e estão ativos
            listaProdutos = desenvController.ObterProduto().Where(p => p.MODELO.Trim() != ""
                && p.GRUPO.Trim() != ""
                && p.STATUS == 'A'
                //&& p.PRODUTO_ACABADO == 'N'
                ).ToList();

            //Filtrar por Colecoes
            if (ddlColecoes.SelectedValue.Trim() != "0" && ddlColecoes.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim()).ToList();

            //Filtrar por Origem
            if (ddlOrigem.SelectedValue.Trim() != "0" && ddlOrigem.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == ddlOrigem.SelectedValue).ToList();

            //Filtrar por grupo
            if (ddlGrupo.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.GRUPO.Trim() == ddlGrupo.SelectedValue.Trim()).ToList();

            //Filtrar por Modelo
            if (txtModelo.Text.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.MODELO.Trim().ToUpper().Contains(txtModelo.Text.Trim().ToUpper())).ToList();

            //Filtrar por nome
            if (txtNome.Text.Trim() != "")
            {
                var produtoFiltro = baseController.BuscaProdutosDescricao(txtNome.Text);
                listaProdutos = listaProdutos.Where(p => produtoFiltro.Any(x => x.PRODUTO1.Trim() == p.MODELO.Trim())).ToList();
            }

            //Filtrar por Tecido
            if (ddlTecido.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.TECIDO_POCKET != null && p.TECIDO_POCKET.Trim().Contains(ddlTecido.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por Cor Fornecedor
            if (ddlCorFornecedor.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim().Contains(ddlCorFornecedor.SelectedValue.Trim().ToUpper())).ToList();

            //Se multiplos, agrupar
            if (chkMultiplo.Checked)
                listaProdutos = listaProdutos.GroupBy(p => new { GRUPO = p.GRUPO, MODELO = p.MODELO }).Select(
                                                                                            k => new DESENV_PRODUTO
                                                                                            {
                                                                                                GRUPO = k.Key.GRUPO,
                                                                                                MODELO = k.Key.MODELO
                                                                                            }).ToList();

            ListItem item = null;
            string _nome = "";
            foreach (DESENV_PRODUTO produto in listaProdutos)
            {
                item = lstModeloSelecionado.Items.FindByValue(produto.MODELO.Trim());
                if (item == null || (item != null && chkMultiplo.Checked))
                {
                    item = new ListItem();
                    item.Value = produto.MODELO.Trim();
                    var produtoLinx = new BaseController().BuscaProduto(produto.MODELO);
                    if (produtoLinx != null)
                        _nome = produtoLinx.DESC_PRODUTO.Trim().Replace(produto.GRUPO.Trim(), "");
                    item.Text = produto.GRUPO.Trim() + " - " + produto.MODELO.Trim() + ((_nome == "") ? "" : (" - " + _nome));
                    lstModeloSelecionado.Items.Add(item);
                    _nome = "";
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            foreach (int indexSelected in lstModeloSelecionado.GetSelectedIndices())
            {
                lstModeloSelecionado.Items.RemoveAt(indexSelected);
            }
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            lstModeloSelecionado.Items.Clear();
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
                CarregarTecido();
                CarregarCorFornecedor(colecao, "");
            }
            else
                CarregarCorFornecedor("", "");
        }
        protected void ddlOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string origem = ddlOrigem.SelectedValue.Trim();
            if (origem != "" && origem != "0")
                CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), origem.Trim());
            else
                CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), "");
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
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
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (new ProducaoController().ObterGrupoProduto("01"));
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
        private void CarregarTecido()
        {
            List<DESENV_PRODUTO> tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao(ddlColecoes.SelectedValue).Where(p => p.TECIDO_POCKET != null && p.STATUS == 'A').Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();
            foreach (var item in _tecido)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor(string colecao, string origem)
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(ddlColecoes.SelectedValue).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');

            if (colecao != "")
                _corFornecedor = _corFornecedor.Where(p => p.COLECAO != null && p.COLECAO.Trim() == colecao.Trim()).ToList();

            if (origem != "" && origem != "0")
                _corFornecedor = _corFornecedor.Where(p => p.DESENV_PRODUTO_ORIGEM != null && p.DESENV_PRODUTO_ORIGEM.ToString() == origem).ToList();

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
    }
}
