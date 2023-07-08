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
    public partial class EntradaProduto : System.Web.UI.Page
    {
        EntradaProdutoController entradaProdutoController = new EntradaProdutoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListLinha();
                CarregaDropDownListGriffe();
                CarregaDropDownListTipo();
                CarregaDropDownListSubGrupo();
                CarregaDropDownListGrupo();
            }
        }

        private void CarregaDropDownListTipo()
        {
            ddlTipo.DataSource = baseController.BuscaTipos();
            ddlTipo.DataBind();
        }
        
        private void CarregaDropDownListGrupo()
        {
            ddlGrupo.DataSource = baseController.BuscaGrupos();
            ddlGrupo.DataBind();
        }
        
        private void CarregaDropDownListSubGrupo()
        {
            ddlSubGrupo.DataSource = baseController.BuscaSubGrupos();
            ddlSubGrupo.DataBind();
        }

        private void CarregaDropDownListLinha()
        {
            ddlLinha.DataSource = baseController.BuscaLinhas();
            ddlLinha.DataBind();
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffes();
            ddlGriffe.DataBind();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaGridViewEntrada()
        {
            GridViewEntrada.DataSource = entradaProdutoController.BuscaEntradasProduto(TextBoxDataInicio.Text, TextBoxDataFim.Text, Session["CODIGO_PRODUTO"].ToString());
            //GridViewEntrada.DataSource = entradaProdutoController.BuscaEntradasProduto("11/01/2012", "30126");
            GridViewEntrada.DataBind();
        }

        private void CarregaDropDownListProduto(string grupo, string subgrupo, string griffe)
        {
            ddlProduto.DataSource = entradaProdutoController.BuscaProdutos(grupo, subgrupo, griffe);
            ddlProduto.DataBind();
        }

        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        protected void ddlLinha_DataBound(object sender, EventArgs e)
        {
            ddlLinha.Items.Add(new ListItem("Selecione", "0"));
            ddlLinha.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }
        
        protected void ddlTipo_DataBound(object sender, EventArgs e)
        {
            ddlTipo.Items.Add(new ListItem("Selecione", "0"));
            ddlTipo.SelectedValue = "0";
        }
        
        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";
        }

        protected void ddlSubGrupo_DataBound(object sender, EventArgs e)
        {
            ddlSubGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlSubGrupo.SelectedValue = "0";
        }

        protected void ddlProduto_DataBound(object sender, EventArgs e)
        {
            ddlProduto.Items.Add(new ListItem("Selecione", "0"));
            ddlProduto.SelectedValue = "0";
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void ButtonPesquisarProduto_Click(object sender, EventArgs e)
        {
            CarregaDropDownListProduto(ddlGrupo.SelectedItem.ToString(), ddlSubGrupo.SelectedItem.ToString(), ddlGriffe.SelectedItem.ToString());
        }

        protected void ButtonPesquisarEntrada_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
                return;

            if ((TextBoxCodigoProduto.Text.Equals("") || TextBoxCodigoProduto.Text == null) && ddlProduto.SelectedValue.ToString().Equals("0"))
                return;

            if (ddlProduto.SelectedValue.ToString().Equals("0") || ddlProduto.SelectedValue.ToString().Equals(""))
                Session["CODIGO_PRODUTO"] = TextBoxCodigoProduto.Text;
            else
                Session["CODIGO_PRODUTO"] = ddlProduto.SelectedValue.ToString();

            CarregaGridViewEntrada();

            PovoaGrafico();
        }

        private void PovoaGrafico()
        {
            List<SP_Entradas_Produto_GraficoResult> tb_vendas = entradaProdutoController.BuscaEntradasProdutoGrafico(TextBoxDataInicio.Text, TextBoxDataFim.Text, Session["CODIGO_PRODUTO"].ToString()); //conta a quantidade de itens no datatble 
            int qtde_linhas = tb_vendas.Count; 
            //cria os arrays para alimentar o gráfico 
            string[] valoresx = new string[qtde_linhas];
            int[] valoresy = new int[qtde_linhas];
            for (int vx = 0; qtde_linhas > vx; vx++)
            {
                valoresx[vx] = Convert.ToString(tb_vendas[vx].FILIAL);
                valoresy[vx] = Convert.ToInt32(tb_vendas[vx].TOTAL_QTDE_ENTRADA);
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

        private void LimpaFeedBack()
        {
            lblMensagemProduto.Text = string.Empty;
        }

        private void LimpaTela()
        {
            TextBoxDataInicio.Text = string.Empty;
            ddlColecao.Text = string.Empty;
            ddlLinha.Text = string.Empty;
            ddlGriffe.Text = string.Empty;
        }
        
        protected void GridViewEntrada_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewEntrada.PageIndex = e.NewPageIndex;
            CarregaGridViewEntrada();
            PovoaGrafico();
        }
    }
}
