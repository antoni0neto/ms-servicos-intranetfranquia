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
    public partial class VendaPorVendedor : System.Web.UI.Page
    {
        SaidaProdutoController saidaProdutoController = new SaidaProdutoController();
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
            }
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
        }        

        private void CarregaGridViewVendas()
        {
            GridViewVendas.DataSource = saidaProdutoController.BuscaVendasVendedor(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
            GridViewVendas.DataBind();
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void ButtonPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
                return;

            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewVendas();

            PovoaGrafico();
        }

        private void PovoaGrafico()
        {
            List<SP_Vendas_Vendedor_GraficoResult> tb_vendas = saidaProdutoController.BuscaVendasVendedorGrafico(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
            int qtde_linhas = tb_vendas.Count; 
            //cria os arrays para alimentar o gráfico 
            string[] valoresx = new string[qtde_linhas];
            int[] valoresy = new int[qtde_linhas];
            for (int vx = 0; qtde_linhas > vx; vx++)
            {
                valoresx[vx] = Convert.ToString(tb_vendas[vx].VENDEDOR);
                valoresy[vx] = Convert.ToInt32(tb_vendas[vx].VALOR);
            }
            //fim do For //insere os valores dos array no gráficos 

            Chart1.Series["Series1"].Points.DataBindXY(valoresx, valoresy);
            //setando o tipo de gráfico a ser usado. Neste caso o gráfico de pizza 
            Chart1.Series["Series1"].ChartType = SeriesChartType.RangeBar;
            //setando os labels denrto de cada fatia do grafico 
            Chart1.Series["Series1"]["PieLabelStyle"] = "inside";
            //Defininto estilo 3D para o gráfico 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

            Chart1.DataBind();

            //LimpaTela();
        }

        protected void GridViewVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_Vendas_VendedorResult vendas = e.Row.DataItem as SP_Vendas_VendedorResult;

            if (vendas != null)
            {
                Literal literalValor = e.Row.FindControl("LiteralValor") as Literal;

                if (literalValor != null)
                    literalValor.Text = Convert.ToDecimal(vendas.VALOR).ToString("##,###.##");

                Literal literalComissao = e.Row.FindControl("LiteralComissao") as Literal;

                if (literalComissao != null)
                    literalComissao.Text = Convert.ToDecimal((vendas.VALOR*4)/100).ToString("##,###.##");

                Literal literalRanking = e.Row.FindControl("LiteralRanking") as Literal;

                if (literalRanking != null)
                {
                    List<Sp_Ranking_VendaResult> sp_Ranking_VendaResult = desempenhoController.BuscaDesempenhoVendedor(TextBoxDataInicio.Text, TextBoxDataFim.Text);

                    if (sp_Ranking_VendaResult != null)
                    {
                        int i = 0;

                        foreach (Sp_Ranking_VendaResult ranking in sp_Ranking_VendaResult)
                        {
                            i++;

                            if (ranking.Nome_Vendedor.Equals(vendas.VENDEDOR))
                            {
                                literalRanking.Text = i.ToString();

                                break;
                            }
                        }
                    }
                }
            }
        }

        protected void GridViewVendas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewVendas.PageIndex = e.NewPageIndex;
            CarregaGridViewVendas();
            PovoaGrafico();
        }
    }
}
