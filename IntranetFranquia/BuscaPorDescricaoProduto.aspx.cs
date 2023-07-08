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
    public partial class BuscaPorDescricaoProduto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListColecao();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecao();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListProdutos(string colecao)
        {
            ddlProdutoNelProforma.DataSource = baseController.BuscaProdutos(colecao);
            ddlProdutoNelProforma.DataBind();
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals(""))
                return;

            CarregaDropDownListProdutos(ddlColecao.SelectedValue.ToString());

            btProdutoNelProforma.Enabled = true;
        }

        protected void btProdutoNelProforma_Click(object sender, EventArgs e)
        {
            if (ddlProdutoNelProforma.SelectedValue.ToString().Equals("0") ||
                ddlProdutoNelProforma.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewNelProforma();
        }

        private void CarregaGridViewNelProforma()
        {
            GridViewProdutoNel.DataSource = baseController.BuscaNelProdutoProduto(Convert.ToInt32(ddlProdutoNelProforma.SelectedValue));
            GridViewProdutoNel.DataBind();

            GridViewProdutoProforma.DataSource = baseController.BuscaProformaProdutoProduto(Convert.ToInt32(ddlProdutoNelProforma.SelectedValue));
            GridViewProdutoProforma.DataBind();
        }

        protected void ddlProdutoNelProforma_DataBound(object sender, EventArgs e)
        {
            ddlProdutoNelProforma.Items.Add(new ListItem("Selecione", "0"));
            ddlProdutoNelProforma.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void GridViewProdutoNel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_NEL_PRODUTO nelProduto = e.Row.DataItem as IMPORTACAO_NEL_PRODUTO;

            if (nelProduto != null)
            {
                Literal literalNel = e.Row.FindControl("LiteralNel") as Literal;

                if (literalNel != null)
                    literalNel.Text = baseController.BuscaNelPeloCodigo(nelProduto.CODIGO_NEL).DESCRICAO_NEL;

                Literal literalJanela = e.Row.FindControl("LiteralJanela") as Literal;

                if (literalJanela != null)
                    literalJanela.Text = baseController.BuscaJanela(nelProduto.CODIGO_JANELA).DESCRICAO;

                Literal literalDescricao = e.Row.FindControl("LiteralDescricao") as Literal;

                if (literalDescricao != null)
                    literalDescricao.Text = baseController.BuscaProduto(nelProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                Literal literalCor = e.Row.FindControl("LiteralCor") as Literal;

                if (literalCor != null)
                    literalCor.Text = baseController.BuscaProdutoCor(nelProduto.CODIGO_PRODUTO.ToString(), nelProduto.CODIGO_PRODUTO_COR.ToString()).DESC_COR_PRODUTO;
            }
        }

        protected void GridViewProdutoProforma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_PROFORMA_PRODUTO proformaProduto = e.Row.DataItem as IMPORTACAO_PROFORMA_PRODUTO;

            if (proformaProduto != null)
            {
                Literal literalProforma = e.Row.FindControl("LiteralProforma") as Literal;

                if (literalProforma != null)
                    literalProforma.Text = baseController.BuscaProformaPeloCodigo(proformaProduto.CODIGO_PROFORMA).DESCRICAO_PROFORMA;

                Literal literalJanela = e.Row.FindControl("LiteralJanela") as Literal;

                if (literalJanela != null)
                    literalJanela.Text = baseController.BuscaJanela(proformaProduto.CODIGO_JANELA).DESCRICAO;

                Literal literalDescricao = e.Row.FindControl("LiteralDescricao") as Literal;

                if (literalDescricao != null)
                    literalDescricao.Text = baseController.BuscaProduto(proformaProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                Literal literalCor = e.Row.FindControl("LiteralCor") as Literal;

                if (literalCor != null)
                    literalCor.Text = baseController.BuscaProdutoCor(proformaProduto.CODIGO_PRODUTO.ToString(), proformaProduto.CODIGO_PRODUTO_COR.ToString()).DESC_COR_PRODUTO;
            }
        }
    }
}
