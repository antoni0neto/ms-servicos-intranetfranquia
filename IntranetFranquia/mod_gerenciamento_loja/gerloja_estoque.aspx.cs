using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class gerloja_estoque : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        int gQtdeVenda = 0;
        int gQtdeVendaSemana = 0;
        int gQtdeEstoque = 0;
        int gES1 = 0;
        int gES2 = 0;
        int gES3 = 0;
        int gES4 = 0;
        int gES5 = 0;
        int gES6 = 0;
        int gES7 = 0;

        string headerTam1 = "";
        string headerTam2 = "";
        string headerTam3 = "";
        string headerTam4 = "";
        string headerTam5 = "";
        string headerTam6 = "";
        string headerTam7 = "";

        const string ESTOQUE_LOJA_FILIAL = "ESTOQUE_LOJA_FILIAL";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_estoque/estoque_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";

                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();

                CarregarFilial();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_ESTOQUE_LOJAResult> ObterEstoqueLoja()
        {

            decimal precoIni = -1;
            decimal precoFim = -1;
            int qtdeEstoqueIni = -1;
            int qtdeEstoqueFim = -1;

            if (txtPrecoDe.Text.Trim() != "")
                precoIni = Convert.ToDecimal(txtPrecoDe.Text.Trim());

            if (txtPrecoAte.Text.Trim() != "")
                precoFim = Convert.ToDecimal(txtPrecoAte.Text.Trim());

            if (txtQtdeDe.Text.Trim() != "")
                qtdeEstoqueIni = Convert.ToInt32(txtQtdeDe.Text.Trim());

            if (txtQtdeAte.Text.Trim() != "")
                qtdeEstoqueFim = Convert.ToInt32(txtQtdeAte.Text.Trim());

            var estoqueLojas = lojaController.ObterEstoqueLoja(ddlFilial.SelectedItem.Text.Trim(), txtProduto.Text.Trim(), ddlGrupo.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim(), ddlColecoes.SelectedValue.Trim(),
                precoIni, precoFim, qtdeEstoqueIni, qtdeEstoqueFim);

            if (ddlCor.SelectedValue != null && ddlCor.SelectedValue != "")
                estoqueLojas = estoqueLojas.Where(p => p.DESC_COR.Trim() == ddlCor.SelectedItem.Text.Trim()).ToList();

            if (ddlTipo.SelectedValue != "")
                estoqueLojas = estoqueLojas.Where(p => p.COD_CATEGORIA == ddlTipo.SelectedValue).ToList();

            return estoqueLojas;

        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var estoqueLoja = ObterEstoqueLoja();

                gvEstoqueLoja.DataSource = estoqueLoja;
                gvEstoqueLoja.DataBind();

                Session[ESTOQUE_LOJA_FILIAL] = estoqueLoja;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvEstoqueLoja_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ESTOQUE_LOJAResult estoque = e.Row.DataItem as SP_OBTER_ESTOQUE_LOJAResult;

                    //System.Web.UI.WebControls.ImageButton imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.ImageButton;
                    //if (File.Exists(Server.MapPath(estoque.FOTO1)))
                    //    imgProduto.ImageUrl = estoque.FOTO1;
                    //else if (File.Exists(Server.MapPath(estoque.FOTO2)))
                    //    imgProduto.ImageUrl = estoque.FOTO2;
                    //else
                    //    imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                    Label labPrecoLoja = e.Row.FindControl("labPrecoLoja") as Label;
                    labPrecoLoja.Text = "R$ " + Convert.ToDecimal(estoque.PRECO_LOJA).ToString("###,###,##0.00");

                    gQtdeVenda += Convert.ToInt32(estoque.QTDE_VENDA);
                    gQtdeVendaSemana += Convert.ToInt32(estoque.QTDE_VENDA_SEMANA);
                    gQtdeEstoque += Convert.ToInt32(estoque.ESTOQUE);
                    gES1 += Convert.ToInt32(estoque.ES1);
                    gES2 += Convert.ToInt32(estoque.ES2);
                    gES3 += Convert.ToInt32(estoque.ES3);
                    gES4 += Convert.ToInt32(estoque.ES4);
                    gES5 += Convert.ToInt32(estoque.ES5);
                    gES6 += Convert.ToInt32(estoque.ES6);
                    gES7 += Convert.ToInt32(estoque.ES7);

                    if (!headerTam1.Contains(estoque.TAMANHO_1))
                        headerTam1 = headerTam1 + estoque.TAMANHO_1 + "-";
                    if (!headerTam2.Contains(estoque.TAMANHO_2))
                        headerTam2 = headerTam2 + estoque.TAMANHO_2 + "-";
                    if (!headerTam3.Contains(estoque.TAMANHO_3))
                        headerTam3 = headerTam3 + estoque.TAMANHO_3 + "-";
                    if (!headerTam4.Contains(estoque.TAMANHO_4))
                        headerTam4 = headerTam4 + estoque.TAMANHO_4 + "-";
                    if (!headerTam5.Contains(estoque.TAMANHO_5))
                        headerTam5 = headerTam5 + estoque.TAMANHO_5 + "-";
                    if (!headerTam6.Contains(estoque.TAMANHO_6))
                        headerTam6 = headerTam6 + estoque.TAMANHO_6 + "-";
                    if (!headerTam7.Contains(estoque.TAMANHO_7))
                        headerTam7 = headerTam7 + estoque.TAMANHO_7 + "-";
                }
            }
        }
        protected void gvEstoqueLoja_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvEstoqueLoja.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[5].Text = gQtdeVenda.ToString();
                _footer.Cells[6].Text = gQtdeVendaSemana.ToString();
                _footer.Cells[7].Text = gQtdeEstoque.ToString();
                _footer.Cells[8].Text = gES1.ToString();
                _footer.Cells[9].Text = gES2.ToString();
                _footer.Cells[10].Text = gES3.ToString();
                _footer.Cells[11].Text = gES4.ToString();
                _footer.Cells[12].Text = gES5.ToString();
                _footer.Cells[13].Text = gES6.ToString();
                _footer.Cells[14].Text = gES7.ToString();
            }

            GerarHeaderTamanho();
        }
        protected void gvEstoqueLoja_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ESTOQUE_LOJAResult> estoqueLoja;

            if (Session[ESTOQUE_LOJA_FILIAL] != null)
                estoqueLoja = (IEnumerable<SP_OBTER_ESTOQUE_LOJAResult>)Session[ESTOQUE_LOJA_FILIAL];
            else
                estoqueLoja = ObterEstoqueLoja();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            estoqueLoja = estoqueLoja.OrderBy(e.SortExpression + sortDirection);
            gvEstoqueLoja.DataSource = estoqueLoja;
            gvEstoqueLoja.DataBind();

            GerarHeaderTamanho();
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = colecoes;
                ddlColecoes.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            var _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = (baseController.BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });

                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }

        private void GerarHeaderTamanho()
        {
            GridViewRow _header = gvEstoqueLoja.HeaderRow;
            if (_header != null)
            {
                headerTam1 = headerTam1 + "-";
                headerTam1 = headerTam1.Replace("--", "");
                headerTam2 = headerTam2 + "-";
                headerTam2 = headerTam2.Replace("--", "");
                headerTam3 = headerTam3 + "-";
                headerTam3 = headerTam3.Replace("--", "");
                headerTam4 = headerTam4 + "-";
                headerTam4 = headerTam4.Replace("--", "");
                headerTam5 = headerTam5 + "-";
                headerTam5 = headerTam5.Replace("--", "");
                headerTam6 = headerTam6 + "-";
                headerTam6 = headerTam6.Replace("--", "");
                headerTam7 = headerTam7 + "-";
                headerTam7 = headerTam7.Replace("--", "");
                _header.Cells[8].Text = headerTam1 ?? "-";
                _header.Cells[9].Text = headerTam2 ?? "-";
                _header.Cells[10].Text = headerTam3 ?? "-";
                _header.Cells[11].Text = headerTam4 ?? "-";
                _header.Cells[12].Text = headerTam5 ?? "-";
                _header.Cells[13].Text = headerTam6 ?? "-";
                _header.Cells[14].Text = headerTam7 ?? "-";

            }
        }
        #endregion

        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var codigoProduto = txtProduto.Text.Trim();

                var produto = baseController.BuscaProduto(codigoProduto);
                if (produto != null)
                {
                    var produtoCores = baseController.BuscaProdutoCores(produto.PRODUTO1);
                    if (produtoCores != null && produtoCores.Count() > 0)
                    {
                        produtoCores.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "", DESC_COR_PRODUTO = "" });

                        ddlCor.DataSource = produtoCores;
                        ddlCor.DataBind();
                    }
                    else
                    {
                        ddlCor.DataSource = new List<PRODUTO_CORE>();
                        ddlCor.DataBind();
                    }
                }
                else
                {
                    ddlCor.DataSource = new List<PRODUTO_CORE>();
                    ddlCor.DataBind();
                }
            }
            catch (Exception)
            {
            }
        }


    }
}
