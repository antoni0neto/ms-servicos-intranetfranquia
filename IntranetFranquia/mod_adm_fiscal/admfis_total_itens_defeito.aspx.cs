using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class admfis_total_itens_defeito : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregarTotalNotaFiscal();
        }

        private void CarregarTotalNotaFiscal()
        {
            gvTotalNotaDefeito.DataSource = baseController.BuscaItensNotaDefeito();
            gvTotalNotaDefeito.DataBind();
        }

        protected void gvTotalNotaDefeito_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvTotalNotaDefeito.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvTotalNotaDefeito.Rows)
                    total += (((Literal)g.FindControl("litContador")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litContador")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[2].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvTotalNotaDefeito_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<Sp_Busca_Pendencias_Nota_DefeitoResult> totalDefeitoLoja = baseController.BuscaItensNotaDefeito();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            totalDefeitoLoja = totalDefeitoLoja.OrderBy(e.SortExpression + sortDirection);
            gvTotalNotaDefeito.DataSource = totalDefeitoLoja;
            gvTotalNotaDefeito.DataBind();
        }
    }
}