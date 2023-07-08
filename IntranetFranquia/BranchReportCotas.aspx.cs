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
    public partial class BranchReportCotas : System.Web.UI.Page
    {
        EstoqueController estoqueController = new EstoqueController();
        EntradaProdutoController entradaProdutoController = new EntradaProdutoController();
        SaidaProdutoController saidaProdutoController = new SaidaProdutoController();
        HistoricoProdutoController historicoProdutoController = new HistoricoProdutoController();
        BaseController baseController = new BaseController();
        List<GrupoProduto> grupo = new List<GrupoProduto>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListSemana454();
        }

        private void CarregaDropDownListSemana454()
        {
            ddlSemana454.DataSource = baseController.BuscaDatas(2012);
            ddlSemana454.DataBind();
        }

        private void CarregaGridViewVendaLoja()
        {
            USUARIO usuario = null;

            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
                usuario = (USUARIO)Session["USUARIO"];

            GridViewVendaLoja.DataSource = historicoProdutoController.BuscaVendasLoja(ddlSemana454.SelectedValue, usuario);
            GridViewVendaLoja.DataBind();
        }

        private void CarregaGridViewEstoque(string filial)
        {
            GridViewEstoque.Visible = true;
            GridViewEstoque.DataSource = historicoProdutoController.BuscaEstoqueLoja(ddlSemana454.SelectedValue, filial);
            GridViewEstoque.DataBind();
        }

        protected void ddlSemana454_DataBound(object sender, EventArgs e)
        {
            ddlSemana454.Items.Add(new ListItem("Selecione", "0"));
            ddlSemana454.SelectedValue = "0";
        }
        
        protected void ButtonPesquisarMovimento_Click(object sender, EventArgs e)
        {
            GridViewEstoque.Visible = false;

            if (ddlSemana454.SelectedValue.ToString().Equals("0") || 
                ddlSemana454.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewVendaLoja();
        }

        protected void GridViewVendaLoja_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewVendaLoja.PageIndex = e.NewPageIndex;
            CarregaGridViewVendaLoja();
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            Button buttonPesquisar = sender as Button;

            if (buttonPesquisar != null)
                CarregaGridViewEstoque(buttonPesquisar.CommandArgument);
        }

        protected void GridViewVendaLoja_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button buttonPesquisar = e.Row.FindControl("ButtonPequisar") as Button;
                if (buttonPesquisar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        BranchReport branchReport = e.Row.DataItem as BranchReport;
                        buttonPesquisar.CommandArgument = branchReport.Descricao;
                    }
                }
            }
        }
    }
}
