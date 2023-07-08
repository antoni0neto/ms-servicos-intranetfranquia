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
    public partial class ListaMovimentoProduto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] datas = baseController.RetornaDatas();

            txtData_5.Text = datas[0].Substring(6, 2) + "/" + datas[0].Substring(4, 2) + "/" + datas[0].Substring(0, 4) + "-" + datas[1].Substring(6, 2) + "/" + datas[1].Substring(4, 2) + "/" + datas[1].Substring(0, 4);
            txtData_4.Text = datas[2].Substring(6, 2) + "/" + datas[2].Substring(4, 2) + "/" + datas[2].Substring(0, 4) + "-" + datas[3].Substring(6, 2) + "/" + datas[3].Substring(4, 2) + "/" + datas[3].Substring(0, 4);
            txtData_3.Text = datas[4].Substring(6, 2) + "/" + datas[4].Substring(4, 2) + "/" + datas[4].Substring(0, 4) + "-" + datas[5].Substring(6, 2) + "/" + datas[5].Substring(4, 2) + "/" + datas[5].Substring(0, 4);
            txtData_2.Text = datas[6].Substring(6, 2) + "/" + datas[6].Substring(4, 2) + "/" + datas[6].Substring(0, 4) + "-" + datas[7].Substring(6, 2) + "/" + datas[7].Substring(4, 2) + "/" + datas[7].Substring(0, 4);
            txtData_1.Text = datas[8].Substring(6, 2) + "/" + datas[8].Substring(4, 2) + "/" + datas[8].Substring(0, 4) + "-" + datas[9].Substring(6, 2) + "/" + datas[9].Substring(4, 2) + "/" + datas[9].Substring(0, 4);

            GridViewProduto.DataSource = baseController.BuscaMovimentoProdutos(datas[1], datas[3], datas[5], datas[7], datas[9]);
            GridViewProduto.DataBind();
        }

        protected void GridViewProduto_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewProduto.Rows)
            {
                Literal literalFilial = item.FindControl("LiteralFilial") as Literal;

                if (!literalFilial.Text.Substring(0, 3).Equals("AAA"))
                {
                    item.Cells[1].BackColor = System.Drawing.Color.Moccasin;
                    item.Cells[2].BackColor = System.Drawing.Color.Moccasin;
                    item.Cells[3].BackColor = System.Drawing.Color.Moccasin;

                    item.Cells[4].BackColor = System.Drawing.Color.PapayaWhip;
                    item.Cells[5].BackColor = System.Drawing.Color.PapayaWhip;

                    item.Cells[6].BackColor = System.Drawing.Color.NavajoWhite;
                    item.Cells[7].BackColor = System.Drawing.Color.NavajoWhite;
                    item.Cells[8].BackColor = System.Drawing.Color.NavajoWhite;

                    item.Cells[9].BackColor = System.Drawing.Color.PeachPuff;
                    item.Cells[10].BackColor = System.Drawing.Color.PeachPuff;
                    item.Cells[11].BackColor = System.Drawing.Color.PeachPuff;

                    item.Cells[12].BackColor = System.Drawing.Color.Bisque;
                    item.Cells[13].BackColor = System.Drawing.Color.Bisque;
                    item.Cells[14].BackColor = System.Drawing.Color.Bisque;

                    item.Cells[15].BackColor = System.Drawing.Color.BlanchedAlmond;
                    item.Cells[16].BackColor = System.Drawing.Color.BlanchedAlmond;
                    item.Cells[17].BackColor = System.Drawing.Color.BlanchedAlmond;
                }
            }
        }

        protected void GridViewProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Movimento_ProdutosResult movimentoProduto = e.Row.DataItem as Sp_Movimento_ProdutosResult;

            if (movimentoProduto != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                    literalFilial.Text = movimentoProduto.filial;

                if (literalFilial.Text.Substring(0, 3).Equals("AAA"))
                    e.Row.BackColor = System.Drawing.Color.Pink;
            }
        }
    }
}
