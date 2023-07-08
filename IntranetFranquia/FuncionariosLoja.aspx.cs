using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Web.UI.DataVisualization.Charting;

namespace Relatorios
{
    public partial class FuncionariosLoja : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        FuncionarioLojaController funcionarioLojaController = new FuncionarioLojaController();

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

        private void CarregaGridViewFuncionarios()
        {
            GridViewFuncionario.DataSource = funcionarioLojaController.BuscaVendedores(ddlFilial.SelectedValue.ToString());
            GridViewFuncionario.DataBind();
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void ButtonPesquisarFuncionarios_Click(object sender, EventArgs e)
        {
            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewFuncionarios();

            PovoaGrafico();
        }

        private void PovoaGrafico()
        {
            List<SP_Funcionarios_Loja_GraficoResult> tb_vendas = funcionarioLojaController.BuscaVendedoresCincoMais(ddlFilial.SelectedValue.ToString());
            int qtde_linhas = tb_vendas.Count; 
            //cria os arrays para alimentar o gráfico 
            string[] valoresx = new string[qtde_linhas];
            int[] valoresy = new int[qtde_linhas];
            for (int vx = 0; qtde_linhas > vx; vx++)
            {
                valoresx[vx] = Convert.ToString(tb_vendas[vx].NOME_VENDEDOR);
                valoresy[vx] = Convert.ToInt32(tb_vendas[vx].MESES);
            }
            //fim do For //insere os valores dos array no gráficos 

            Chart1.Series["Series1"].Points.DataBindXY(valoresx, valoresy);
            //setando o tipo de gráfico a ser usado. Neste caso o gráfico de pizza 
            Chart1.Series["Series1"].ChartType = SeriesChartType.Area;
            //setando os labels denrto de cada fatia do grafico 
            Chart1.Series["Series1"]["PieLabelStyle"] = "inside";
            //Defininto estilo 3D para o gráfico 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

            Chart1.DataBind();

            //LimpaTela();
        }

        protected void GridViewFuncionario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewFuncionario.PageIndex = e.NewPageIndex;
            CarregaGridViewFuncionarios();
            PovoaGrafico();
        }
    }
}