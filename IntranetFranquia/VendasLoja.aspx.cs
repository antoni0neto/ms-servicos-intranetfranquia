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
    public partial class VendasLoja : System.Web.UI.Page
    {
        SaidaProdutoController saidaProdutoController = new SaidaProdutoController();
        BaseController baseController = new BaseController();

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
            GridViewVendas.DataSource = saidaProdutoController.BuscaVendasLoja(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
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
            List<SP_Vendas_Loja_GraficoResult> tb_vendas = saidaProdutoController.BuscaVendasLojaGrafico(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
            int qtde_linhas = tb_vendas.Count; 
            //cria os arrays para alimentar o gráfico 
            string[] valoresx = new string[qtde_linhas];
            int[] valoresy = new int[qtde_linhas];
            for (int vx = 0; qtde_linhas > vx; vx++)
            {
                valoresx[vx] = Convert.ToString(tb_vendas[vx].PRODUTO);
                valoresy[vx] = Convert.ToInt32(tb_vendas[vx].QTDE);
            }
            //fim do For 
            //insere os valores dos array no gráficos 

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

        protected void GridViewVendas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewVendas.PageIndex = e.NewPageIndex;
            CarregaGridViewVendas();
            PovoaGrafico();
        }
    }
}
