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
    public partial class gest_historico_hand_cliente_produto : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                string ticket = "";
                string codigoFilial = "";
                string dataVenda = "";

                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "" ||
                    Request.QueryString["cf"] == null || Request.QueryString["cf"] == "" ||
                    Request.QueryString["dv"] == null || Request.QueryString["dv"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("gest_menu.aspx");

                ticket = Request.QueryString["t"].ToString();
                codigoFilial = Request.QueryString["cf"].ToString();
                dataVenda = Request.QueryString["dv"].ToString();

                hidTicket.Value = ticket;
                hidCodigoFilial.Value = codigoFilial;
                hidDataVEnda.Value = dataVenda.ToString();

                CarregarProdutos(ticket, codigoFilial, dataVenda);
            }

        }

        #region "DADOS INICIAIS"
        #endregion

        #region "PRODUTOs"
        private List<SP_OBTER_VENDA_PRODUTO_TICKETResult> ObterProdutoPorTicket(string ticket, string codigoFilial, string dataVenda)
        {
            return lojaController.ObterProdutoPorTicket(ticket, codigoFilial, Convert.ToDateTime(dataVenda));
        }
        private void CarregarProdutos(string ticket, string codigoFilial, string dataVenda)
        {
            var produtos = ObterProdutoPorTicket(ticket, codigoFilial, dataVenda);

            gvProdutos.DataSource = produtos;
            gvProdutos.DataBind();
        }
        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_PRODUTO_TICKETResult prod = e.Row.DataItem as SP_OBTER_VENDA_PRODUTO_TICKETResult;

                    if (prod != null)
                    {

                    }
                }
            }
        }
        protected void gvProdutos_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProdutos.FooterRow;
            if (footer != null)
            {

            }
        }
        protected void gvProdutos_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_VENDA_PRODUTO_TICKETResult> prods = ObterProdutoPorTicket(hidTicket.Value, hidCodigoFilial.Value, hidDataVEnda.Value);

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
