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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class gerloja_acessorio_venda : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        int gVendaPeriodo = 0;
        int gVendaTotal = 0;
        int gEstoque = 0;

        const string ACESS_VENDA_PRODUTO_DIARIO = "ACESS_VENDA_PRODUTO_DIARIO";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

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
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                CarregarColecoes();
                CarregarColecoesLinx();
                CarregarSubGrupo();
                CarregarGriffe();
                CarregarGrupo("");
                CarregarFornecedor("");
                CarregarSignedNome();
                CarregarTecido("", "");
                CarregarCorFornecedor("", "", "");

                Session[ACESS_VENDA_PRODUTO_DIARIO] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_VENDA_PRODUTO_DIARIOResult> ObterVendaProdutoDiario()
        {
            DateTime dataIni = Convert.ToDateTime(txtPeriodoInicial.Text);
            DateTime dataFim = Convert.ToDateTime(txtPeriodoFinal.Text);

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

            var gVendaProduto = lojaController.ObterVendaProdutosDiario(dataIni, dataFim, colecaoLinxCon, ddlColecoes.SelectedValue.Trim(), 0, griffeCon, grupoProdutoCon, ddlFornecedor.SelectedValue.Trim(), txtProduto.Text.Trim(), txtNome.Text.ToUpper(), ddlProdutoAcabado.SelectedValue, ddlSignedNome.SelectedValue.Trim(), ddlTipo.SelectedValue, ddlSubGrupoProduto.SelectedValue.Trim(), ddlTecido.SelectedValue.Trim(), ddlCorFornecedor.SelectedValue.Trim());

            if (ddlSubCategoria.SelectedValue != "")
                gVendaProduto = gVendaProduto.Where(p => p.COD_SUBCATEGORIA == ddlSubCategoria.SelectedValue).ToList();

            return gVendaProduto;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                /*VALIDA DATA*/
                if (txtPeriodoInicial.Text == "")
                {
                    labErro.Text = "Informe a Data Inicial.";
                    return;
                }

                if (txtPeriodoFinal.Text == "")
                {
                    labErro.Text = "Informe a Data Final.";
                    return;
                }

                var vendaProdutoDiario = ObterVendaProdutoDiario();

                gvVendaAcessorio.DataSource = vendaProdutoDiario;
                gvVendaAcessorio.DataBind();

                Session[ACESS_VENDA_PRODUTO_DIARIO] = vendaProdutoDiario;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvVendaAcessorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_PRODUTO_DIARIOResult vendaAcessorio = e.Row.DataItem as SP_OBTER_VENDA_PRODUTO_DIARIOResult;

                    System.Web.UI.WebControls.Image imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    if (File.Exists(Server.MapPath(vendaAcessorio.FOTO1)))
                        imgProduto.ImageUrl = vendaAcessorio.FOTO1;
                    else if (File.Exists(Server.MapPath(vendaAcessorio.FOTO2)))
                        imgProduto.ImageUrl = vendaAcessorio.FOTO2;
                    else if (File.Exists(Server.MapPath(vendaAcessorio.FOTO3)))
                        imgProduto.ImageUrl = vendaAcessorio.FOTO3;
                    else
                        imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                    Label _labCor = e.Row.FindControl("labCor") as Label;
                    _labCor.Text = vendaAcessorio.COR_PRODUTO + " - " + vendaAcessorio.DESC_COR_PRODUTO;


                    Label _labQtdeLiqPeriodo = e.Row.FindControl("labQtdeLiqPeriodo") as Label;
                    _labQtdeLiqPeriodo.Text = Convert.ToInt32(vendaAcessorio.QTDE_LIQ_PERIODO).ToString();

                    Label _labCusto = e.Row.FindControl("labCusto") as Label;
                    _labCusto.Text = "R$ " + Convert.ToDecimal(vendaAcessorio.CUSTO).ToString("###,###,##0.00");

                    Label _labPrecoOriginal = e.Row.FindControl("labPrecoOriginal") as Label;
                    _labPrecoOriginal.Text = "R$ " + Convert.ToDecimal(vendaAcessorio.PRECO_ORIGINAL).ToString("###,###,##0.00");

                    Label _labPrecoLoja = e.Row.FindControl("labPrecoLoja") as Label;
                    _labPrecoLoja.Text = "R$ " + Convert.ToDecimal(vendaAcessorio.PRECO_LOJA).ToString("###,###,##0.00");

                    gVendaPeriodo += Convert.ToInt32(vendaAcessorio.QTDE_LIQ_PERIODO);
                    gVendaTotal += Convert.ToInt32(vendaAcessorio.QTDE_LIQ_TOTAL);
                    gEstoque += Convert.ToInt32(vendaAcessorio.ESTOQUE);

                }
            }
        }
        protected void gvVendaAcessorio_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvVendaAcessorio.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[7].Text = gVendaPeriodo.ToString();
                _footer.Cells[8].Text = gVendaTotal.ToString();
                _footer.Cells[9].Text = gEstoque.ToString();
            }
        }
        protected void gvVendaAcessorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_VENDA_PRODUTO_DIARIOResult> vendaAcessorio;

            if (Session[ACESS_VENDA_PRODUTO_DIARIO] != null)
                vendaAcessorio = (IEnumerable<SP_OBTER_VENDA_PRODUTO_DIARIOResult>)Session[ACESS_VENDA_PRODUTO_DIARIO];
            else
                vendaAcessorio = ObterVendaProdutoDiario();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            vendaAcessorio = vendaAcessorio.OrderBy(e.SortExpression + sortDirection);
            gvVendaAcessorio.DataSource = vendaAcessorio;
            gvVendaAcessorio.DataBind();
        }

        #region "DADOS INICIAIS"
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
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarGrupo(ddlTipo.SelectedValue);
            CarregarFornecedor(ddlTipo.SelectedValue);
        }


        #endregion


    }
}
