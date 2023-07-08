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
    public partial class ListaProduto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListColecao();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListProduto(string colecao)
        {
            ddlProduto.DataSource = baseController.BuscaProdutos(colecao);
            ddlProduto.DataBind();
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlProduto_DataBound(object sender, EventArgs e)
        {
            ddlProduto.Items.Add(new ListItem("Selecione", "0"));
            ddlProduto.SelectedValue = "0";
        }

        protected void ButtonImprimir_Click(object sender, EventArgs e)
        {

        }

        protected void btProduto_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") || ddlColecao.SelectedValue.ToString().Equals("")) 
                return;

            CarregaDropDownListProduto(ddlColecao.SelectedValue.ToString());
        }

        protected void btMovimento_Click(object sender, EventArgs e)
        {
            if (ddlProduto.SelectedValue.ToString().Equals("0") || ddlProduto.SelectedValue.ToString().Equals("") || ddlProduto.SelectedValue.ToString().Equals("Selecione"))
                return;

            Sp_Pega_Data_BancoResult dt = baseController.BuscaDataBanco();

            string data = dt.data.Year.ToString() + dt.data.Month.ToString("00") + dt.data.Day.ToString("00");

            Sp_Busca_Semana_454Result bs = baseController.BuscaSemana454(data);

            string semana = bs.ano_semana.ToString();

            if (semana.Substring(4, 2).Equals("01"))
                semana = (dt.data.Year - 1).ToString() + "52";
            else
                semana = (bs.ano_semana - 1).ToString();

            List<string> anoSemanasPassada = new List<string>();

            if (Convert.ToInt32(semana.Substring(4, 2)) > 4)
            {
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 4).ToString());
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 3).ToString());
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
            }
            else
            {
                if (semana.Substring(4, 2).Equals("04"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 3).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
                if (semana.Substring(4, 2).Equals("03"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
                if (semana.Substring(4, 2).Equals("02"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "50");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
                if (semana.Substring(4, 2).Equals("01"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "49");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "50");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
            }

            string[] datasInicio = new string[5];
            string[] datasFim = new string[5];

            int i = 0;

            foreach (string item in anoSemanasPassada)
            {
                datasInicio[i] = baseController.AjustaData(baseController.BuscaDataInicio(item));
                datasFim[i] = baseController.AjustaData(baseController.BuscaDataFim(item));

                i++;
            }

            txtData_5.Text = datasInicio[0].Substring(6, 2) + "/" + datasInicio[0].Substring(4, 2) + "/" + datasInicio[0].Substring(0, 4) + "-" + datasFim[0].Substring(6, 2) + "/" + datasFim[0].Substring(4, 2) + "/" + datasFim[0].Substring(0, 4);
            txtData_4.Text = datasInicio[1].Substring(6, 2) + "/" + datasInicio[1].Substring(4, 2) + "/" + datasInicio[1].Substring(0, 4) + "-" + datasFim[1].Substring(6, 2) + "/" + datasFim[1].Substring(4, 2) + "/" + datasFim[1].Substring(0, 4);
            txtData_3.Text = datasInicio[2].Substring(6, 2) + "/" + datasInicio[2].Substring(4, 2) + "/" + datasInicio[2].Substring(0, 4) + "-" + datasFim[2].Substring(6, 2) + "/" + datasFim[2].Substring(4, 2) + "/" + datasFim[2].Substring(0, 4);
            txtData_2.Text = datasInicio[3].Substring(6, 2) + "/" + datasInicio[3].Substring(4, 2) + "/" + datasInicio[3].Substring(0, 4) + "-" + datasFim[3].Substring(6, 2) + "/" + datasFim[3].Substring(4, 2) + "/" + datasFim[3].Substring(0, 4);
            txtData_1.Text = datasInicio[4].Substring(6, 2) + "/" + datasInicio[4].Substring(4, 2) + "/" + datasInicio[4].Substring(0, 4) + "-" + datasFim[4].Substring(6, 2) + "/" + datasFim[4].Substring(4, 2) + "/" + datasFim[4].Substring(0, 4);
            
            GridViewMovimentoProduto.DataSource = baseController.BuscaMovimentoProduto(ddlProduto.SelectedValue.ToString(), datasFim[0], datasFim[1], datasFim[2], datasFim[3], datasFim[4]);
            GridViewMovimentoProduto.DataBind();
        }

        protected void GridViewMovimentoProduto_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewMovimentoProduto.Rows)
            {
                Literal literalFilial = item.FindControl("LiteralNomeFilial") as Literal;

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

        protected void GridViewMovimentoProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Movimento_ProdutoResult movimentoProduto = e.Row.DataItem as Sp_Movimento_ProdutoResult;

            if (movimentoProduto != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralNomeFilial") as Literal;

                if (literalFilial != null)
                    literalFilial.Text = movimentoProduto.filial;

                if (literalFilial.Text.Substring(0, 3).Equals("AAA"))
                    e.Row.BackColor = System.Drawing.Color.Pink;
            }
        }
    }
}
