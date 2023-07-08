using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class MenuProduto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btGeraMovimentoProduto_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AtualizaProduto.aspx");
        }

        protected void btListaMovimentoProduto_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ListaMovimentoProduto.aspx");
        }

        protected void btListaProduto_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ListaProduto.aspx");
        }

        protected void btImprimeMovimentoProduto_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ImprimeMovimentoProduto.aspx");
        }
    }
}
