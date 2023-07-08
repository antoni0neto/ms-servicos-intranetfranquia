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
    public partial class AnaliseVenda : System.Web.UI.Page
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
            ddlSemana454.DataSource = baseController.BuscaDatas(2010);
            ddlSemana454.DataBind();
        }

        private void CarregaGridViewVendaPeriodo()
        {
            USUARIO usuario = null;

            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
                usuario = (USUARIO)Session["USUARIO"];

            GridViewVendaPeriodo.DataSource = historicoProdutoController.BuscaVendasPeriodo(ddlSemana454.SelectedValue, usuario);
            GridViewVendaPeriodo.DataBind();
        }

        private void CarregaGridViewVendaPeriodoGrupo()
        {
            USUARIO usuario = null;

            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
                usuario = (USUARIO)Session["USUARIO"];

            GridViewVendaPeriodoGrupo.DataSource = historicoProdutoController.BuscaVendasPeriodoGrupo(usuario);
            GridViewVendaPeriodoGrupo.DataBind();
        }

        private void CarregaGridViewVendaPeriodoLoja(string loja)
        {
            GridViewVendaPeriodoLoja.DataSource = historicoProdutoController.BuscaVendasPeriodoLoja(loja);
            GridViewVendaPeriodoLoja.DataBind();
        }
        
        protected void ddlSemana454_DataBound(object sender, EventArgs e)
        {
            ddlSemana454.Items.Add(new ListItem("Selecione", "0"));
            ddlSemana454.SelectedValue = "0";
        }
        
        protected void ButtonPesquisarMovimento_Click(object sender, EventArgs e)
        {
            if (ddlSemana454.SelectedValue.ToString().Equals("0") || 
                ddlSemana454.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewVendaPeriodo();
            CarregaGridViewVendaPeriodoGrupo();
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            Button buttonPesquisar = sender as Button;

            if (buttonPesquisar != null)
                CarregaGridViewVendaPeriodoLoja(buttonPesquisar.CommandArgument);
        }

        protected void GridViewVendaPeriodo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button buttonPesquisar = e.Row.FindControl("ButtonPequisar") as Button;
                if (buttonPesquisar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        ReportVendas vendaReport = e.Row.DataItem as ReportVendas;
                        buttonPesquisar.CommandArgument = baseController.BuscaFilial(vendaReport.Item).COD_FILIAL;
                    }
                }

                Literal literalPa = e.Row.FindControl("LiteralPa") as Literal;

                if (literalPa != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        ReportVendas vendaReport = e.Row.DataItem as ReportVendas;

                        if (vendaReport.Pa == null || vendaReport.Pa.Equals(""))
                            literalPa.Text = "0";
                        else
                            literalPa.Text = vendaReport.Pa;
                    }
                }

                Literal literalPa_2 = e.Row.FindControl("LiteralPa_2") as Literal;

                if (literalPa_2 != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        ReportVendas vendaReport = e.Row.DataItem as ReportVendas;

                        if (vendaReport.Pa_2 == null || vendaReport.Pa_2.Equals(""))
                            literalPa_2.Text = "0";
                        else
                            literalPa_2.Text = vendaReport.Pa_2;
                    }
                }

                Literal literalDifer = e.Row.FindControl("LiteralDifer") as Literal;

                if (literalDifer != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        ReportVendas vendaReport = e.Row.DataItem as ReportVendas;

                        if (vendaReport.Difer_ticket == null || vendaReport.Difer_ticket.Equals(""))
                            literalDifer.Text = "0";
                        else
                            literalDifer.Text = vendaReport.Difer_ticket;
                    }
                }
            }
        }

        protected void GridViewVendaPeriodo_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendaPeriodo.Rows)
            {
                Literal literalPa = item.FindControl("LiteralPa") as Literal;
                Literal literalPa_2 = item.FindControl("LiteralPa_2") as Literal;

                if (Convert.ToDecimal(literalPa.Text) > Convert.ToDecimal(literalPa_2.Text))
                    item.Cells[13].BackColor = System.Drawing.Color.Red;
                else
                    item.Cells[13].BackColor = System.Drawing.Color.Green;

                Literal literalDifer = item.FindControl("LiteralDifer") as Literal;

                if (Convert.ToDecimal(literalDifer.Text) < 0)
                    item.Cells[10].ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
