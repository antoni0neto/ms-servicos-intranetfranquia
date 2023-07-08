using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;

namespace Relatorios
{
    public partial class gest_venda_semana_filial : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        ProducaoController prodController = new ProducaoController();

        int gQtde = 0;
        decimal gValor = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //p=" + produto.Trim() + "&c=" + cor + "&di=" + dataIni + "&df=" + dataFim + "";


                string produto = "";
                string cor = "";
                string dataIni = "";
                string dataFim = "";

                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Request.QueryString["di"] == null || Request.QueryString["di"] == "" ||
                    Request.QueryString["df"] == null || Request.QueryString["df"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("gest_menu.aspx");

                produto = Request.QueryString["p"].ToString();
                cor = Request.QueryString["c"].ToString();
                dataIni = Request.QueryString["di"].ToString();
                dataFim = Request.QueryString["df"].ToString();

                hidProduto.Value = produto;
                hidCor.Value = cor;
                hidDataIni.Value = dataIni.ToString();
                hidDataFim.Value = dataFim.ToString();

                txtProduto.Text = produto;
                txtCor.Text = prodController.ObterCoresBasicas(cor).DESC_COR;
                txtDataIni.Text = Convert.ToDateTime(dataIni).ToString("dd/MM/yyyy");
                txtDataFim.Text = Convert.ToDateTime(dataFim).ToString("dd/MM/yyyy"); ;

                CarregarVendaPorFilial(produto, cor, dataIni, dataFim);
            }

        }

        #region "DADOS INICIAIS"
        #endregion

        #region "PRODUTOs"
        private List<SP_OBTER_VENDA_PRODUTO_SEMANA_FILIALResult> ObterVendaProdutoSemanaPorFilial(string produto, string cor, string dataIni, string dataFim)
        {
            return lojaController.ObterVendaProdutoSemanaPorFilial(produto, cor, Convert.ToDateTime(dataIni), Convert.ToDateTime(dataFim));
        }
        private void CarregarVendaPorFilial(string produto, string cor, string dataIni, string dataFim)
        {
            var produtos = ObterVendaProdutoSemanaPorFilial(produto, cor, dataIni, dataFim);

            gvProdutos.DataSource = produtos;
            gvProdutos.DataBind();
        }
        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_PRODUTO_SEMANA_FILIALResult prod = e.Row.DataItem as SP_OBTER_VENDA_PRODUTO_SEMANA_FILIALResult;

                    if (prod != null)
                    {
                        Literal litValor = e.Row.FindControl("litValor") as Literal;
                        litValor.Text = "R$ " + Convert.ToDecimal(prod.VALOR).ToString("###,###,##0.00");

                        gQtde += Convert.ToInt32(prod.QTDE);
                        gValor += Convert.ToDecimal(prod.VALOR);
                    }
                }
            }
        }
        protected void gvProdutos_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProdutos.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[2].Text = gQtde.ToString();
                footer.Cells[3].Text = "R$ " + gValor.ToString("###,###,##0.00");

            }
        }
        protected void gvProdutos_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_VENDA_PRODUTO_SEMANA_FILIALResult> prods = ObterVendaProdutoSemanaPorFilial(hidProduto.Value, hidCor.Value, hidDataIni.Value, hidDataFim.Value);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prods = prods.OrderBy(e.SortExpression + sortDirection);
            gvProdutos.DataSource = prods;
            gvProdutos.DataBind();
        }
        #endregion






    }
}
