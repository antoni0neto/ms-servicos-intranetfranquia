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
    public partial class desenv_pocket_con_gemeav2 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        HtmlGenericControl div;

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
                CarregarCorFornecedor("", "", "");
                CarregarGriffe();
                CarregarSignedNome();
            }

            btAdicionar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAdicionar, null) + ";");
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");

        }

        private List<DESENV_PRODUTO> ObterProduto()
        {

            List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();

            List<DESENV_PRODUTO> produtos = null;
            foreach (ListItem item in lstModeloSelecionado.Items)
            {
                if (item != null)
                {

                    produtos = new List<DESENV_PRODUTO>();
                    produtos = desenvController.ObterProduto(ddlColecoes.SelectedValue, item.Value.ToUpper(), 'A');
                    listaProdutos.AddRange(produtos);
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
        private void CarregarProdutos()
        {
            var produtos = ObterProduto();

            gvProduto.DataSource = produtos;
            gvProduto.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labImpressao.Text = "";
                labBuscar.Text = "";

                CarregarProdutos();

                cbLiberarTodosGemea.Checked = false;
                cbLiberarTodos_CheckedChanged(cbLiberarTodos1Peca, null);
                cbLiberarTodos_CheckedChanged(cbLiberarTodosGemea, null);
            }
            catch (Exception ex)
            {
                labBuscar.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO dProduto = e.Row.DataItem as DESENV_PRODUTO;

                    if (dProduto != null)
                    {
                        if (dProduto.DATA_LIB_1PECA != null)
                        {
                            e.Row.Cells[1].BackColor = Color.IndianRed;
                            Literal litDataLiberacao1Peca = e.Row.FindControl("litDataLiberacao1Peca") as Literal;
                            litDataLiberacao1Peca.Text = Convert.ToDateTime(dProduto.DATA_LIB_1PECA).ToString("dd/MM/yyyy");
                        }

                        if (dProduto.DATA_LIB_GEMEA != null)
                        {
                            e.Row.Cells[3].BackColor = Color.IndianRed;
                            Literal litDataLiberacaoGemea = e.Row.FindControl("litDataLiberacaoGemea") as Literal;
                            litDataLiberacaoGemea.Text = Convert.ToDateTime(dProduto.DATA_LIB_GEMEA).ToString("dd/MM/yyyy");
                        }

                        Label _labOrigem = e.Row.FindControl("labOrigem") as Label;
                        if (_labOrigem != null)
                            _labOrigem.Text = dProduto.DESENV_PRODUTO_ORIGEM1.DESCRICAO;

                        Label _labNomeProduto = e.Row.FindControl("labNomeProduto") as Label;
                        if (_labNomeProduto != null)
                        {
                            string descProduto = (dProduto.DESC_MODELO == null) ? "" : dProduto.DESC_MODELO.Replace(dProduto.GRUPO.Trim(), "");
                            _labNomeProduto.Text = " - " + dProduto.GRIFFE.Trim() + " - " + dProduto.GRUPO.Trim() + " " + dProduto.MODELO.Trim() + " " + descProduto + " - " + dProduto.TECIDO_POCKET;
                        }

                        System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                        if (_imgProduto != null)
                            _imgProduto.ImageUrl = dProduto.FOTO;

                        GridView _gvProdutoCor = e.Row.FindControl("gvProdutoCor") as GridView;
                        if (_gvProdutoCor != null)
                        {
                            var p = desenvController.ObterProduto(dProduto.COLECAO, dProduto.MODELO, dProduto.COR);
                            var produtos = new List<DESENV_PRODUTO>();
                            produtos.Add(p);
                            _gvProdutoCor.DataSource = produtos;
                            _gvProdutoCor.DataBind();
                        }
                    }
                }
            }
        }
        protected void gvProdutoCor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO dProduto = e.Row.DataItem as DESENV_PRODUTO;

                    if (dProduto != null)
                    {
                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(dProduto.COR).DESC_COR;

                        Literal _litAtacado = e.Row.FindControl("litAtacado") as Literal;
                        if (_litAtacado != null)
                            _litAtacado.Text = dProduto.QTDE_ATACADO.ToString();

                        Literal _litVarejo = e.Row.FindControl("litVarejo") as Literal;
                        if (_litVarejo != null)
                            _litVarejo.Text = dProduto.QTDE.ToString();

                        Literal _litMostruario = e.Row.FindControl("litMostruario") as Literal;
                        if (_litMostruario != null)
                        {
                            var grade = prodController.ObterGradeRealProduto(dProduto.COLECAO, dProduto.MODELO, dProduto.COR, 'S');
                            if (grade != null)
                                _litMostruario.Text = (grade.QTDE_ATACADO + grade.QTDE_VAREJO).ToString();
                        }
                    }
                }
            }
        }
        protected void cbLiberarTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = null;
            foreach (GridViewRow r in gvProduto.Rows)
            {
                cb = r.FindControl("cbLiberar1Peca") as CheckBox;
                if (cb != null)
                    cb.Checked = cbLiberarTodos1Peca.Checked;
                cb = r.FindControl("cbLiberarGemea") as CheckBox;
                if (cb != null)
                    cb.Checked = cbLiberarTodosGemea.Checked;
            }


        }

        #region "CONTROLES LISTA"
        protected void btAdicionar_Click(object sender, EventArgs e)
        {
            labBuscar.Text = "";
            labImpressao.Text = "";

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
                var produtoFiltro = baseController.BuscaProdutosDescricao(txtNome.Text.ToUpper().Trim());
                listaProdutos = listaProdutos.Where(p => produtoFiltro.Any(x => x.PRODUTO1.Trim() == p.MODELO.Trim())).ToList();
            }

            //Filtrar por Tecido
            if (ddlTecido.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.TECIDO_POCKET != null && p.TECIDO_POCKET.Trim().Contains(ddlTecido.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por Cor Fornecedor
            if (ddlCorFornecedor.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim().Contains(ddlCorFornecedor.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por griffe
            if (ddlGriffe.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.GRIFFE != null && p.GRIFFE.Trim().Contains(ddlGriffe.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por Produto Acabado
            if (ddlProdutoAcabado.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.PRODUTO_ACABADO != null && p.PRODUTO_ACABADO == Convert.ToChar(ddlProdutoAcabado.SelectedValue)).ToList();

            //Filtrar por Signed
            if (ddlSigned.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.SIGNED != null && p.SIGNED == Convert.ToChar(ddlSigned.SelectedValue)).ToList();

            //Filtrar por Signed Nome
            if (ddlSignedNome.SelectedValue.Trim() != "")
                listaProdutos = listaProdutos.Where(p => p.SIGNED_NOME != null && p.SIGNED_NOME.Contains(ddlSignedNome.SelectedValue.Trim().ToUpper())).ToList();

            if (ddlLiberado1Peca.SelectedValue != "")
            {
                if (ddlLiberado1Peca.SelectedValue == "S")
                    listaProdutos = listaProdutos.Where(p => p.DATA_LIB_1PECA != null).ToList();
                else
                    listaProdutos = listaProdutos.Where(p => p.DATA_LIB_1PECA == null).ToList();
            }

            if (ddlLiberadoGemea.SelectedValue != "")
            {
                if (ddlLiberadoGemea.SelectedValue == "S")
                    listaProdutos = listaProdutos.Where(p => p.DATA_LIB_GEMEA != null).ToList();
                else
                    listaProdutos = listaProdutos.Where(p => p.DATA_LIB_GEMEA == null).ToList();
            }


            ListItem item = null;
            string _nome = "";
            foreach (DESENV_PRODUTO produto in listaProdutos)
            {
                item = lstModeloSelecionado.Items.FindByValue(produto.MODELO.Trim());
                if (item == null)
                {
                    item = new ListItem();
                    item.Value = produto.MODELO.Trim();
                    var produtoLinx = baseController.BuscaProduto(produto.MODELO);
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
                CarregarCorFornecedor(colecao, "", ddlTecido.SelectedValue.Trim());
            }
            else
                CarregarCorFornecedor("", "", "");

            lstModeloSelecionado.Items.Clear();
        }
        protected void ddlOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string origem = ddlOrigem.SelectedValue.Trim();
            if (origem != "" && origem != "0")
                CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), origem.Trim(), ddlTecido.SelectedValue.Trim());
            else
                CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), "", ddlTecido.SelectedValue.Trim());

            lstModeloSelecionado.Items.Clear();
        }
        protected void ddlTecido_SelectedIndexChanged(object sender, EventArgs e)
        {
            string origem = (ddlOrigem.SelectedValue == "" || ddlOrigem.SelectedValue == "0") ? "" : ddlOrigem.SelectedValue.Trim();
            CarregarCorFornecedor(ddlColecoes.SelectedValue.Trim(), origem, ddlTecido.SelectedValue.Trim());
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
        private void CarregarCorFornecedor(string colecao, string origem, string tecido)
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(ddlColecoes.SelectedValue).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');

            if (colecao != "")
                _corFornecedor = _corFornecedor.Where(p => p.COLECAO != null && p.COLECAO.Trim() == colecao.Trim()).ToList();

            if (origem != "" && origem != "0")
                _corFornecedor = _corFornecedor.Where(p => p.DESENV_PRODUTO_ORIGEM != null && p.DESENV_PRODUTO_ORIGEM.ToString() == origem).ToList();

            if (tecido != "")
                _corFornecedor = _corFornecedor.Where(p => p.TECIDO_POCKET != "" && p.TECIDO_POCKET.Contains(tecido.Trim())).ToList();

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
            List<PRODUTOS_GRIFFE> griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
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
        #endregion

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labImpressao.Text = "";

                var produtos = new List<DESENV_PRODUTO>();
                bool lib = false;

                CheckBox cb = null;
                foreach (GridViewRow row in gvProduto.Rows)
                {
                    cb = row.FindControl("cbLiberar1Peca") as CheckBox;
                    if (cb != null && cb.Checked)
                    {
                        string colecao = gvProduto.DataKeys[row.RowIndex][0].ToString();
                        string produto = gvProduto.DataKeys[row.RowIndex][1].ToString();
                        string cor = gvProduto.DataKeys[row.RowIndex][2].ToString();
                        var p = desenvController.ObterProduto(colecao, produto, cor);
                        if (p != null)
                        {
                            p.DATA_LIB_1PECA = DateTime.Now;
                            desenvController.AtualizarProduto(p);
                            lib = true;
                        }
                    }

                    cb = row.FindControl("cbLiberarGemea") as CheckBox;
                    if (cb != null && cb.Checked)
                    {
                        string colecao = gvProduto.DataKeys[row.RowIndex][0].ToString();
                        string produto = gvProduto.DataKeys[row.RowIndex][1].ToString();
                        string cor = gvProduto.DataKeys[row.RowIndex][2].ToString();
                        var p = desenvController.ObterProduto(colecao, produto, cor);
                        if (p != null)
                        {
                            produtos.Add(p);
                            lib = true;
                        }

                    }
                }

                if (lib)
                {
                    if (produtos.Count() > 0)
                    {
                        GerarRelatorio(produtos);
                    }
                    labImpressao.Text = "Liberado com sucesso.";
                }
                else
                {
                    labImpressao.Text = "Não existem produtos selecionados para liberação.";
                }

            }
            catch (Exception ex)
            {
                labImpressao.Text = "ERRO: " + ex.Message;
            }
        }
        private void GerarRelatorio(List<DESENV_PRODUTO> produtos)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "DESENV_LIB_PRODV2" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(produtos));
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
        private StringBuilder MontarRelatorioHTML(List<DESENV_PRODUTO> produtos)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);

            //string modelo = "";
            int contador = 1;
            string breakClass = "";
            foreach (var produto in produtos)
            {
                breakClass = "";

                if (contador == 4)
                {
                    contador = 0;
                    breakClass = "breakafter";
                }

                //if (modelo != produto.MODELO)
                _texto = MontarGemeaFoto(_texto, breakClass, produto);
                //else
                //    _texto = MontarGemea(_texto, breakClass, produto);

                //modelo = produto.MODELO;


                produto.DATA_LIB_GEMEA = DateTime.Now;
                if (produto.DATA_LIB_1PECA == null)
                    produto.DATA_LIB_1PECA = DateTime.Now;
                desenvController.AtualizarProduto(produto);

                contador += 1;

            }

            _texto = MontarRodape(_texto);
            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Liberação Produto - Gemêa</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("         <style type='text/css'>");
            _texto.Append("             .breakafter");
            _texto.Append("             {");
            _texto.Append("                 page-break-after: always;");
            _texto.Append("             }");
            _texto.Append("             .breakbefore");
            _texto.Append("             {");
            _texto.Append("                 page-break-before: always;");
            _texto.Append("             }");
            _texto.Append("         </style>");
            _texto.Append("</head>");
            _texto.AppendLine("<body onLoad='window.print();'>");

            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }
        private StringBuilder MontarGemeaFoto(StringBuilder _texto, string breakClass, DESENV_PRODUTO produto)
        {
            int qtdeMostruario = 0;
            var grade = prodController.ObterGradeRealProduto(produto.COLECAO, produto.MODELO, produto.COR, 'S');
            if (grade != null)
                qtdeMostruario = Convert.ToInt32(grade.QTDE_ATACADO + grade.QTDE_VAREJO);

            string nomeProduto = baseController.BuscaProduto(produto.MODELO).DESC_PRODUTO.Trim();
            string nomeCor = prodController.ObterCoresBasicas(produto.COR).DESC_COR.Trim();
            string tecido = produto.TECIDO_POCKET;

            _texto.AppendLine("    <div id='divGemea" + produto.CODIGO.ToString() + "' align='center' class='" + breakClass + "'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width :100%;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='width:100%;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' height='220' style='width: 100%;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='text-align: center; border-left: 1px solid black; border-bottom: 1px solid black; border-top: 1px solid black; '>");
            _texto.AppendLine("                                <img alt='Foto' Width='80px' Height='100px' src='..\\.." + ((produto.FOTO == null) ? "" : produto.FOTO.Replace("~", "").Replace("/", "\\")) + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='3' style='text-align:center; line-height:40px; border:1px solid black;'>");
            _texto.AppendLine("                                <span style='color: red; font-size: 23px;'>" + produto.GRIFFE + "</span><br />");
            _texto.AppendLine("                                <span style='color: red; font-size: 25px;'>" + nomeProduto + " - " + produto.MODELO + "</span><br />");
            _texto.AppendLine("                                <span style='color: red; font-size: 23px;'>" + nomeCor + " - " + tecido + "</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='width: 25%; text-align: center; font-size: 16px; border-left: 1px solid black; border-bottom: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Origem");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 25%; text-align: center; font-size: 16px; border-left: 1px solid black; border-bottom: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Atacado");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 25%; text-align: center; font-size: 16px; border-left: 1px solid black; border-bottom: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Varejo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 25%; text-align: center; font-size: 16px; border-left: 1px solid black; border-bottom: 1px solid black; border-right: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Mostruário");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='text-align:left;'>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 28px; border-left: 1px solid black; border-bottom: 1px solid black; '>");
            _texto.AppendLine("                                " + produto.DESENV_PRODUTO_ORIGEM1.DESCRICAO.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; '>");
            _texto.AppendLine("                                " + produto.QTDE_ATACADO.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; '>");
            _texto.AppendLine("                                " + produto.QTDE.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; border-right: 1px solid black; '>");
            _texto.AppendLine("                                " + qtdeMostruario.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");

            return _texto;
        }
        private StringBuilder MontarGemea(StringBuilder _texto, string breakClass, DESENV_PRODUTO produto)
        {
            int qtdeMostruario = 0;
            var grade = prodController.ObterGradeRealProduto(produto.COLECAO, produto.MODELO, produto.COR, 'S');
            if (grade != null)
                qtdeMostruario = Convert.ToInt32(grade.QTDE_ATACADO + grade.QTDE_VAREJO);

            string nomeProduto = baseController.BuscaProduto(produto.MODELO).DESC_PRODUTO.Trim();
            string nomeCor = prodController.ObterCoresBasicas(produto.COR).DESC_COR.Trim();
            string tecido = produto.TECIDO_POCKET;


            _texto.AppendLine("    <div id='divGemea" + produto.CODIGO.ToString() + "' align='center' class='" + breakClass + "'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width :100%;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='width:49%;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' height='220' style='width: 100%;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='3' style='text-align:center; line-height:40px; border:1px solid black;'>");
            _texto.AppendLine("                                <span style='color: red; font-size: 25px;'>" + nomeProduto + " - " + produto.MODELO + "</span><br />");
            _texto.AppendLine("                                <span style='color: red; font-size: 23px;'>" + nomeCor + " - " + tecido + "</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='width: 33%; text-align: center; font-size: 15px; border-left: 1px solid black; border-bottom: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Atacado");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 33%; text-align: center; font-size: 15px; border-left: 1px solid black; border-bottom: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Varejo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 33%; text-align: center; font-size: 15px; border-left: 1px solid black; border-bottom: 1px solid black; border-right: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Mostruário");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='text-align:left;'>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; '>");
            _texto.AppendLine("                                " + produto.QTDE_ATACADO.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; '>");
            _texto.AppendLine("                                " + produto.QTDE.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; border-right: 1px solid black; '>");
            _texto.AppendLine("                                " + qtdeMostruario.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width:2%;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td style='width:49%;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' height='220' style='width: 100%;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='3' style='text-align:center; line-height:40px; border:1px solid black;'>");
            _texto.AppendLine("                                <span style='color: red; font-size: 25px;'>" + nomeProduto + " - " + produto.MODELO + "</span><br />");
            _texto.AppendLine("                                <span style='color: red; font-size: 23px;'>" + nomeCor + " - " + tecido + "</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='width: 33%; text-align: center; font-size: 15px; border-left: 1px solid black; border-bottom: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Atacado");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 33%; text-align: center; font-size: 15px; border-left: 1px solid black; border-bottom: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Varejo");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 33%; text-align: center; font-size: 15px; border-left: 1px solid black; border-bottom: 1px solid black; border-right: 1px solid black; background-color: #f1ecec; '>");
            _texto.AppendLine("                                Mostruário");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='text-align:left;'>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; '>");
            _texto.AppendLine("                                " + produto.QTDE_ATACADO.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; '>");
            _texto.AppendLine("                                " + produto.QTDE.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center; font-size: 20px; border-left: 1px solid black; border-bottom: 1px solid black; border-right: 1px solid black; '>");
            _texto.AppendLine("                                " + qtdeMostruario.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");

            return _texto;
        }



    }
}

