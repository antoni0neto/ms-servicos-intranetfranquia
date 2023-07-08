using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using DAL;
using System.Text;
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class desenv_pocket_con : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

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
                if (tela != "1" && tela != "2" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_producao/prod_menu_cad.aspx";

                CarregarColecoes();
                CarregarGrupo();
                //CarregarTecido();
                //CarregarCorFornecedor("", "", "");
                CarregarGriffe();
                CarregarSignedNome();
                //CarregarFornecedor();
            }

            btAdicionar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAdicionar, null) + ";");
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
        }

        private StringBuilder MontarPocket(List<DESENV_PRODUTO> listaProdutos, bool print, bool v2, bool gradeReal)
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

                int loop = pocket.Select(i => i.index).Distinct().Count();

                int count = 0;
                List<DESENV_PRODUTO> prod = new List<DESENV_PRODUTO>();

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

                        b = Pocket.MontarConteudoV2(b, pocketProduto.Select(j => j.produto).ToList(), gradeReal);

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
        private StringBuilder MontarPocketHBF(List<DESENV_PRODUTO> listaProdutos, bool print, bool gradeReal)
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

                int loop = pocket.Select(i => i.index).Distinct().Count();

                int count = 0;
                List<DESENV_PRODUTO> prod = new List<DESENV_PRODUTO>();

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

                        b = Pocket.MontarConteudoHBF(b, pocketProduto.Select(j => j.produto).ToList(), gradeReal);

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

                if (chkVersaoFicha.Checked)
                    divPocket.InnerHtml = MontarPocketHBF(ObterProduto(), false, chkGradeReal.Checked).ToString();
                else
                    divPocket.InnerHtml = MontarPocket(ObterProduto(), false, true, chkGradeReal.Checked).ToString();
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

                if (chkVersaoFicha.Checked)
                    wr.Write(MontarPocketHBF(ObterProduto(), true, chkGradeReal.Checked).ToString());
                else
                    wr.Write(MontarPocket(ObterProduto(), true, true, chkGradeReal.Checked).ToString());

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

        private int ObterQtdeCopia(string modelo)
        {
            int qtde = 0;

            foreach (ListItem item in lstModeloSelecionado.Items)
                if (item.Value == modelo)
                    qtde += 1;

            return qtde;
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
                    produto = desenvController.ObterProduto(ddlColecoes.SelectedValue, item.Value.ToUpper(), 'A').ToList();
                    //produto = desenvController.ObterProduto(ddlColecoes.SelectedValue, Convert.ToInt32(ddlOrigem.SelectedValue), item.Value.ToUpper(), 'A').ToList();
                    if (listaProdutos.Where(p => p.MODELO == produto[0].MODELO).Count() <= 0)
                        listaProdutos.AddRange(produto);
                }
            }

            //Verificar se esse filtro continua --------
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

        #region "CONTROLES LISTA"
        protected void btAdicionar_Click(object sender, EventArgs e)
        {
            labBuscar.Text = "";

            if (ddlColecoes.SelectedValue.Trim() == "")
            {
                labBuscar.Text = "Selecione a Coleção.";
                return;
            }

            List<SP_OBTER_VIEWResult> listaProdutos = new List<SP_OBTER_VIEWResult>();

            //Obter apenas produtos que tenham modelos e estão ativos
            listaProdutos = desenvController.ObterView(ddlColecoes.SelectedValue.Trim());

            ////Filtrar por Colecoes
            //if (ddlColecoes.SelectedValue.Trim() != "0" && ddlColecoes.SelectedValue.Trim() != "")
            //    listaProdutos = listaProdutos.Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim()).ToList();

            //////Filtrar por Origem
            //Filtrar por griffe
            var origemCon = "";
            if (lstOrigem.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstOrigem.GetSelectedIndices())
                {
                    var itemList = lstOrigem.Items[v].Value.Trim() + ",";
                    origemCon = origemCon + itemList;
                }

                origemCon = origemCon + ",";
                origemCon = origemCon.Replace(",,", "");
            }
            if (origemCon != "" && origemCon != "0")
                listaProdutos = listaProdutos.Where(p => origemCon.Contains(p.DESENV_PRODUTO_ORIGEM.ToString().Trim())).ToList();

            //Filtrar por grupo
            if (ddlGrupo.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.GRUPO_PRODUTO.Trim() == ddlGrupo.SelectedValue.Trim()).ToList();

            //Filtrar por Modelo
            if (txtModelo.Text.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.PRODUTO.Trim().ToUpper().Contains(txtModelo.Text.Trim().ToUpper())).ToList();

            //Filtrar por nome
            if (txtNome.Text.Trim() != "")
            {
                var produtoFiltro = baseController.BuscaProdutosDescricao(txtNome.Text.ToUpper().Trim());
                listaProdutos = listaProdutos.Where(p => produtoFiltro.Any(x => x.PRODUTO1.Trim() == p.PRODUTO.Trim())).ToList();
            }

            //Filtrar por Tecido
            if (ddlTecido.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.TECIDO != null && p.TECIDO.Trim().Contains(ddlTecido.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por Cor Fornecedor
            if (ddlCorFornecedor.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim().Contains(ddlCorFornecedor.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por griffe
            var griffeCon = "";
            if (lstGriffe.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGriffe.GetSelectedIndices())
                {
                    var itemList = lstGriffe.Items[v].Value.Trim() + ",";
                    griffeCon = griffeCon + itemList;
                }

                griffeCon = griffeCon + ",";
                griffeCon = griffeCon.Replace(",,", "");
            }
            if (griffeCon != "")
                listaProdutos = listaProdutos.Where(p => p.GRIFFE != null && griffeCon.Contains(p.GRIFFE.Trim())).ToList();



            //Filtrar por Produto Acabado
            if (ddlProdutoAcabado.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.PRODUTO_ACABADO == Convert.ToChar(ddlProdutoAcabado.SelectedValue)).ToList();

            //Filtrar por Signed
            if (ddlSigned.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.SIGNED != null && p.SIGNED == ddlSigned.SelectedValue).ToList();

            //Filtrar por Signed Nome
            if (ddlSignedNome.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.SIGNED_NOME != null && p.SIGNED_NOME.Contains(ddlSignedNome.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por Signed Nome
            if (ddlFornecedor.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.FORNECEDOR != null && p.FORNECEDOR.Contains(ddlFornecedor.SelectedValue.Trim().ToUpper())).ToList();

            if (ddlFiltroQtde.SelectedValue != "")
            {
                //<asp:ListItem Value="1" Text="Somente Atacado"></asp:ListItem>
                //<asp:ListItem Value="2" Text="Somente Varejo"></asp:ListItem>
                //<asp:ListItem Value="3" Text="Atacado + (Atacado ou Varejo)"></asp:ListItem>
                //<asp:ListItem Value="4" Text="Varejo + (Atacado ou Varejo)"></asp:ListItem>
                //<asp:ListItem Value="5" Text="Atacado E Varejo"></asp:ListItem>

                if (ddlFiltroQtde.SelectedValue == "1")
                    listaProdutos = listaProdutos.Where(p => p.QTDE_ATACADO > 0 && p.QTDE_VAREJO <= 0).ToList();
                else if (ddlFiltroQtde.SelectedValue == "2")
                    listaProdutos = listaProdutos.Where(p => p.QTDE_VAREJO > 0 && p.QTDE_ATACADO <= 0).ToList();
                else if (ddlFiltroQtde.SelectedValue == "3")
                    listaProdutos = listaProdutos.Where(p => p.QTDE_ATACADO > 0).ToList();
                else if (ddlFiltroQtde.SelectedValue == "4")
                    listaProdutos = listaProdutos.Where(p => p.QTDE_VAREJO > 0).ToList();
                else if (ddlFiltroQtde.SelectedValue == "5")
                    listaProdutos = listaProdutos.Where(p => p.QTDE_VAREJO > 0 && p.QTDE_ATACADO > 0).ToList();
            }

            if (ddlTecidoLiberado.SelectedValue != "")
            {
                if (ddlTecidoLiberado.SelectedValue == "S")
                    listaProdutos = listaProdutos.Where(p => p.DATA_APROVACAO_TECIDO != null).ToList();
                else
                    listaProdutos = listaProdutos.Where(p => p.DATA_APROVACAO_TECIDO == null).ToList();
            }

            if (ddlCortado.SelectedValue != "")
                listaProdutos = listaProdutos.Where(p => p.CORTADO == ddlCortado.SelectedValue).ToList();


            //Se multiplos, agrupar
            if (chkMultiplo.Checked)
                listaProdutos = listaProdutos.GroupBy(p => new { GRUPO_PRODUTO = p.GRUPO_PRODUTO, PRODUTO = p.PRODUTO }).Select(
                                                                                            k => new SP_OBTER_VIEWResult
                                                                                            {
                                                                                                GRUPO_PRODUTO = k.Key.GRUPO_PRODUTO,
                                                                                                PRODUTO = k.Key.PRODUTO
                                                                                            }).ToList();

            ListItem item = null;
            string _nome = "";
            foreach (SP_OBTER_VIEWResult produto in listaProdutos)
            {
                item = lstModeloSelecionado.Items.FindByValue(produto.PRODUTO.Trim());
                if (item == null || (item != null && chkMultiplo.Checked))
                {
                    item = new ListItem();
                    item.Value = produto.PRODUTO.Trim();
                    if (produto.DESC_PRODUTO != null && produto.DESC_PRODUTO.Trim() != "")
                    {
                        _nome = produto.DESC_PRODUTO.Trim().Replace(produto.GRUPO_PRODUTO.Trim(), "");
                    }
                    else
                    {
                        var produtoLinx = baseController.BuscaProduto(produto.PRODUTO);
                        if (produtoLinx != null)
                            _nome = produtoLinx.DESC_PRODUTO.Trim().Replace(produto.GRUPO_PRODUTO.Trim(), "");
                    }
                    item.Text = produto.GRUPO_PRODUTO.Trim() + " - " + produto.PRODUTO.Trim() + ((_nome == "") ? "" : (" - " + _nome));
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
                CarregarTecido(colecao, "");
                CarregarFornecedor(colecao);
                CarregarCorFornecedor(colecao, "", "");
            }

            lstModeloSelecionado.Items.Clear();
        }
        //protected void ddlOrigem_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string origem = ddlOrigem.SelectedValue.Trim();
        //    if (origem != "" && origem != "0")
        //    {
        //        CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), origem.Trim(), "");
        //        CarregarTecido(ddlColecoes.SelectedValue.Trim(), origem.Trim());
        //    }
        //    else
        //    {
        //        CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), "", "");
        //    }

        //    lstModeloSelecionado.Items.Clear();
        //}
        protected void ddlTecido_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCorFornecedor(ddlColecoes.SelectedValue, "", ddlTecido.SelectedValue);
        }
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

                ddlColecoes.DataSource = colecoes;
                ddlColecoes.DataBind();

                //if (Session["COLECAO"] != null)
                //{
                //    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                //    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                //    ddlColecoes_SelectedIndexChanged(null, null);
                //}
            }
        }
        private void CarregarGrupo()
        {
            var grupoProduto = (prodController.ObterGrupoProduto("01"));
            if (grupoProduto != null)
            {
                grupoProduto.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = grupoProduto;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            var origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
            origem = origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (origem != null)
            {
                origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                lstOrigem.DataSource = origem;
                lstOrigem.DataBind();

            }
        }
        private void CarregarTecido(string colecao, string origem)
        {
            var tecidoFiltro = new List<DESENV_PRODUTO>();

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
            var corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');

            if (origem != "" && origem != "0")
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
            List<PRODUTOS_GRIFFE> griffe = (baseController.BuscaGriffes());

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
        private void CarregarFornecedor(string colecao)
        {
            List<DESENV_PRODUTO> fornecedorFiltro = new List<DESENV_PRODUTO>();

            var _fornecedor = desenvController.ObterDesenvolvimentoColecao(ddlColecoes.SelectedValue).Where(p => p.FORNECEDOR != null && p.STATUS == 'A').Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();
            foreach (var item in _fornecedor)
                if (item.Trim() != "")
                    fornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR = item.Trim() });

            fornecedorFiltro = fornecedorFiltro.OrderBy(p => p.FORNECEDOR).ToList();
            fornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR = "" });
            ddlFornecedor.DataSource = fornecedorFiltro;
            ddlFornecedor.DataBind();
        }
        #endregion


    }
}
