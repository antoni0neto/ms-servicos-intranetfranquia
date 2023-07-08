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
    public partial class ecom_ordem_cad_blocoproduto_sembloco : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarCategoriaMag();
                CarregarColecoes();
                CarregarCorLinx();
                CarregarGriffe();
            }

            //Evitar duplo clique no botão
            btBuscarFiltro.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscarFiltro, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarCategoriaMag()
        {
            var catMag = ecomController.ObterMagentoGrupoProdutoBloco();
            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();
        }
        protected void ddlCategoriaMag_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var cat = ecomController.ObterMagentoGrupoProduto(Convert.ToInt32(ddlCategoriaMag.SelectedValue));

            //if (cat != null)
            //{
            //    //CarregarTipoModelagem(cat.GRIFFE, cat.GRUPO_PRODUTO);
            //    //CarregarTipoTecido(cat.GRIFFE, cat.GRUPO_PRODUTO);
            //    //CarregarTipoManga(cat.GRIFFE, cat.GRUPO_PRODUTO);
            //    //CarregarTipoGola(cat.GRIFFE, cat.GRUPO_PRODUTO);
            //    //CarregarTipoComprimento(cat.GRIFFE, cat.GRUPO_PRODUTO);
            //    //CarregarTipoEstilo(cat.GRIFFE, cat.GRUPO_PRODUTO);
            //}
        }

        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
                }
            }
        }
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarTecido(ddlColecao.SelectedValue.Trim());
            CarregarCorFornecedor(ddlColecao.SelectedValue.Trim());
        }

        private void CarregarCorLinx()
        {
            var corLinx = prodController.ObterCoresBasicas();
            if (corLinx != null)
            {
                corLinx.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
                ddlCorLinx.DataSource = corLinx;
                ddlCorLinx.DataBind();
            }
        }
        private void CarregarTecido(string colecao)
        {
            var tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.TECIDO_POCKET != null && p.STATUS == 'A').Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();
            foreach (var item in _tecido)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor(string colecao)
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');
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
            var griffes = baseController.BuscaGriffes();

            if (griffes != null)
            {
                griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffes;
                ddlGriffe.DataBind();
            }
        }
        #endregion


        #region "PRODUTO"

        protected void btBuscarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlCategoriaMag.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Categoria Magento.";
                    return;
                }

                CarregarProduto();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private List<SP_OBTER_ECOM_PRODUTO_SEMBLOCOResult> ObterProdutoSemBloco()
        {
            int ecomGrupoProduto = Convert.ToInt32(ddlCategoriaMag.SelectedValue);
            string colecao = ddlColecao.SelectedValue.Trim();
            string produto = txtProduto.Text.Trim();
            string cor = ddlCorLinx.SelectedValue.Trim();
            string tecido = ddlTecido.SelectedValue.Trim();
            string corFornecedor = ddlCorFornecedor.SelectedValue.Trim();
            int tipoModelagem = 0;
            int tipoTecido = 0;
            int tipoManga = 0;
            int tipoGola = 0;
            int tipoComprimento = 0;
            int tipoEstilo = 0;

            var produtos = ecomController.ObterProdutoSemBloco(ecomGrupoProduto, colecao, produto, cor, tecido, corFornecedor, tipoModelagem, tipoTecido, tipoManga, tipoGola, tipoComprimento, tipoEstilo);

            if (ddlGriffe.SelectedValue != "")
                produtos = produtos.Where(p => p.GRIFFE.Trim() == ddlGriffe.SelectedValue.Trim()).ToList();


            if (ddlSemBloco.SelectedValue != "")
            {
                /*
                    <asp:ListItem Value="1" Text="Sem nenhum bloco"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Sem bloco da categoria"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Com bloco da categoria"></asp:ListItem>             
                 */
                if (ddlSemBloco.SelectedValue == "1")
                    produtos = produtos.Where(p => p.COD_PRODUTO_ORDEM == 0).ToList();
                else
                {
                    var produtosDaCategoria = ecomController.ObterBlocoProdutoOrdemPorCodigoEcomGrupoProduto(ecomGrupoProduto).Select(x => x.ECOM_PRODUTO);
                    if (ddlSemBloco.SelectedValue == "3")
                        produtos = produtos.Where(p => produtosDaCategoria.Contains(p.CODIGO)).ToList();
                    else
                        produtos = produtos.Where(p => !produtosDaCategoria.Contains(p.CODIGO)).ToList();
                }
            }

            return produtos;
        }
        private void CarregarProduto()
        {
            var produtos = ObterProdutoSemBloco();

            gvProduto.DataSource = produtos;
            gvProduto.DataBind();
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_SEMBLOCOResult produto = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_SEMBLOCOResult;

                    if (produto != null)
                    {
                        System.Web.UI.WebControls.Image imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                        imgProduto.ImageUrl = produto.FOTO_FRENTE_CAB;

                        Literal litBlocos = e.Row.FindControl("litBlocos") as Literal;
                        litBlocos.Text = FormatarTextoBloco(produto.BLOCOS);
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_PRODUTO_SEMBLOCOResult> produto = ObterProdutoSemBloco();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvProduto, e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(gvProduto, e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(gvProduto, e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produto = produto.OrderBy(e.SortExpression + sortDirection);
            gvProduto.DataSource = produto;
            gvProduto.DataBind();

        }

        private string FormatarTextoBloco(string campo)
        {
            return campo.Replace("#@@@", "<font size='2' color=''>").Replace("@@@%", "</font><br /><br />");
        }
        #endregion






    }
}

