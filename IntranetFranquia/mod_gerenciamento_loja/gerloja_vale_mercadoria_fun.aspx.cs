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
    public partial class gerloja_vale_mercadoria_fun : System.Web.UI.Page
    {
        SaidaProdutoController saidaProdutoController = new SaidaProdutoController();
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListFilial();
        }

        #region "dados inicias"
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
        #endregion

        protected void ButtonPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
                return;

            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewVendas();

        }

        private void CarregaGridViewVendas()
        {
            GridViewVendas.DataSource = saidaProdutoController.BuscaVendasConsumidor(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
            GridViewVendas.DataBind();
        }
        protected void GridViewVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Vendas_ConsumidorResult vendas = e.Row.DataItem as Sp_Vendas_ConsumidorResult;

            if (vendas != null)
            {
                Literal literalValor = e.Row.FindControl("LiteralValor") as Literal;

                if (literalValor != null)
                    literalValor.Text = Convert.ToDecimal(vendas.VALOR).ToString("##,###.##");

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


    }
}
