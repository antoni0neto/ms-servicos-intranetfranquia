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
    public partial class LineReportColection : System.Web.UI.Page
    {
        EstoqueController estoqueController = new EstoqueController();
        EntradaProdutoController entradaProdutoController = new EntradaProdutoController();
        HistoricoProdutoController historicoProdutoController = new HistoricoProdutoController();
        BaseController baseController = new BaseController();
        List<GrupoProduto> grupo = new List<GrupoProduto>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListCategoria();
                CarregaDropDownListColecao();
                CarregaDropDownListSemana454();
                CarregaDropDownListGriffe();
            }
        }

        private void CarregaDropDownListCategoria()
        {
            ddlCategoria.DataSource = baseController.BuscaCategorias();
            ddlCategoria.DataBind();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListSemana454()
        {
            ddlSemana454.DataSource = baseController.BuscaDatas(2010);
            ddlSemana454.DataBind();
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffes();
            ddlGriffe.DataBind();
        }

        private void CarregaGridViewColecaoProduto()
        {
            string colecaoAnoAnterior = (Convert.ToInt32(ddlColecao.SelectedValue) - 2).ToString();

            GridViewMovimentoProduto.DataSource = historicoProdutoController.BuscaMovimentoColecao(ddlCategoria.SelectedValue, 
                                                                                                   ddlColecao.SelectedValue, 
                                                                                                   ddlSemana454.SelectedValue, 
                                                                                                   ddlGriffe.SelectedItem.ToString(),
                                                                                                   colecaoAnoAnterior);
            GridViewMovimentoProduto.DataBind();
        }

        protected void ddlSemana454_DataBound(object sender, EventArgs e)
        {
            ddlSemana454.Items.Add(new ListItem("Selecione", "0"));
            ddlSemana454.SelectedValue = "0";
        }
        
        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        protected void ddlCategoria_DataBound(object sender, EventArgs e)
        {
            ddlCategoria.Items.Add(new ListItem("Selecione", "0"));
            ddlCategoria.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ButtonPesquisarMovimento_Click(object sender, EventArgs e)
        {
            if (ddlGriffe.SelectedValue.ToString().Equals("0")    || 
                ddlGriffe.SelectedValue.ToString().Equals("")     ||
                ddlCategoria.SelectedValue.ToString().Equals("0") || 
                ddlCategoria.SelectedValue.ToString().Equals("")  ||
                ddlColecao.SelectedValue.ToString().Equals("0")   || 
                ddlColecao.SelectedValue.ToString().Equals("")    ||
                ddlSemana454.SelectedValue.ToString().Equals("0") || 
                ddlSemana454.SelectedValue.ToString().Equals(""))
                 return;

            CarregaGridViewColecaoProduto();
        }
    }
}
