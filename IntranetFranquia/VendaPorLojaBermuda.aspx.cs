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
    public partial class VendaPorLojaBermuda : System.Web.UI.Page
    {
        SaidaProdutoController saidaProdutoController = new SaidaProdutoController();
        BaseController baseController = new BaseController();

        int totalVendas = 0;
        int totalCotas = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Sp_Vendas_Loja_BermudaResult> lista = saidaProdutoController.BuscaVendasLojaBermuda("20121112", "20121130");
                List<VendasBermuda> listaFinal = new List<VendasBermuda>();
                COTASLOJA cl = null;

                if (lista != null)
                {
                    foreach (Sp_Vendas_Loja_BermudaResult item in lista)
                    {
                        VendasBermuda vb = new VendasBermuda();

                        vb.Filial = baseController.BuscaFilialCodigo(Convert.ToInt32(item.FILIAL)).FILIAL;

                        vb.Data_minima = item.DATA_MINIMA.ToString();
                        vb.Data_maxima = item.DATA_MAXIMA.ToString();

                        vb.Qtde = item.QTDE.ToString();

                        cl = baseController.BuscaCotaFilial(baseController.BuscaFilialCodigo(Convert.ToInt32(item.FILIAL)).FILIAL);

                        if (cl != null)
                        {
                            vb.Cota = cl.COTA.ToString();

                            vb.PercAtingido = ((Convert.ToDecimal(item.QTDE) / Convert.ToDecimal(cl.COTA)) * 100).ToString("N2") + " %";

                            totalCotas += Convert.ToInt32(cl.COTA);
                        }

                        listaFinal.Add(vb);
                    }
                }

                GridViewVendas.DataSource = listaFinal;
                GridViewVendas.DataBind();

                PovoaGrafico();
            }
        }

        private void PovoaGrafico()
        {
            List<Sp_Vendas_Loja_BermudaResult> tb_vendas = saidaProdutoController.BuscaVendasLojaBermuda("20121112", "20121130");
            int qtde_linhas = tb_vendas.Count; 
            //cria os arrays para alimentar o gráfico 
            string[] valoresx = new string[qtde_linhas];
            int[] valoresy = new int[qtde_linhas];
            for (int vx = 0; qtde_linhas > vx; vx++)
            {
                valoresx[vx] = baseController.BuscaFilialCodigo(Convert.ToInt32(tb_vendas[vx].FILIAL)).FILIAL;
                valoresy[vx] = Convert.ToInt32(tb_vendas[vx].QTDE);
            }
            //fim do For //insere os valores dos array no gráficos 

            Chart1.Series["Series1"].Points.DataBindXY(valoresx, valoresy);
            //setando o tipo de gráfico a ser usado. Neste caso o gráfico de pizza 
            Chart1.Series["Series1"].ChartType = SeriesChartType.Bar;
            //setando os labels denrto de cada fatia do grafico 
            //Chart1.Series["Series1"]["PieLabelStyle"] = "inside";
            //Defininto estilo 3D para o gráfico 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

            Chart1.DataBind();

            //LimpaTela();
        }

        protected void GridViewVendas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewVendas.FooterRow;

            foreach (GridViewRow item in GridViewVendas.Rows)
            {
                totalVendas += Convert.ToInt32(item.Cells[3].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[3].Text = totalVendas.ToString();
                footer.Cells[4].Text = totalCotas.ToString();
                footer.Cells[5].Text = ((Convert.ToDecimal(totalVendas) / Convert.ToDecimal(totalCotas)) * 100).ToString("N2") + " %";
            }
        }
    }
}
