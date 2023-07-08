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
    public partial class ImportaProdutoNel : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

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

        private void CarregaDropDownListJanela()
        {
            ddlJanela.DataSource = baseController.BuscaJanela();
            ddlJanela.DataBind();
        }

        private void CarregaDropDownListProforma()
        {
            ddlProforma.DataSource = baseController.BuscaProformasColecao(Convert.ToInt32(ddlColecao.SelectedValue));
            ddlProforma.DataBind();
        }

        private void CarregaDropDownListFornecedor()
        {
            ddlFornecedor.DataSource = baseController.BuscaFornecedor();
            ddlFornecedor.DataBind();
        }

        private void CarregaGridViewProdutos()
        {
            GridViewProdutos.DataSource = baseController.BuscaProdutosProforma(Convert.ToInt32(ddlProforma.SelectedValue));
            GridViewProdutos.DataBind();
        }

        private void CarregaDropDownListNel()
        {
            ddlNel.DataSource = baseController.BuscaNel(Convert.ToInt32(ddlColecao.SelectedValue), Convert.ToInt32(ddlJanela.SelectedValue));
            ddlNel.DataBind();
        }

        protected void ddlNel_DataBound(object sender, EventArgs e)
        {
            ddlNel.Items.Add(new ListItem("Selecione", "0"));
            ddlNel.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlJanela_DataBound(object sender, EventArgs e)
        {
            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";
        }

        protected void ddlProforma_DataBound(object sender, EventArgs e)
        {
            ddlProforma.Items.Add(new ListItem("Selecione", "0"));
            ddlProforma.SelectedValue = "0";
        }
        
        protected void ddlFornecedor_DataBound(object sender, EventArgs e)
        {
            ddlFornecedor.Items.Add(new ListItem("Selecione", "0"));
            ddlFornecedor.SelectedValue = "0";
        }

        protected void btProforma_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals(""))
                return;

            CarregaDropDownListJanela();
            CarregaDropDownListProforma();
            CarregaDropDownListFornecedor();

            btBuscarNel.Enabled = true;
        }

        protected void btBuscarNel_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("") ||
                ddlProforma.SelectedValue.ToString().Equals("0") ||
                ddlProforma.SelectedValue.ToString().Equals("") ||
                ddlJanela.SelectedValue.ToString().Equals("0") ||
                ddlJanela.SelectedValue.ToString().Equals("") ||
                ddlFornecedor.SelectedValue.ToString().Equals("0") ||
                ddlFornecedor.SelectedValue.ToString().Equals(""))
                return;

            CarregaDropDownListNel();

            btBuscarProdutos.Enabled = true;
        }

        protected void btBuscarProdutos_Click(object sender, EventArgs e)
        {
            if (ddlNel.SelectedValue.ToString().Equals("0") ||
                ddlNel.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewProdutos();

            btImportar.Enabled = true;
        }

        protected void btImportar_Click(object sender, EventArgs e)
        {
            int contProduto = 0;

            IMPORTACAO_NEL_PRODUTO nelProduto = new IMPORTACAO_NEL_PRODUTO();

            foreach (GridViewRow item in GridViewProdutos.Rows)
            {
                CheckBox cbImporta = item.FindControl("cbImporta") as CheckBox;

                if (cbImporta != null)
                {
                    if (cbImporta.Checked)
                    {
                        if (baseController.ExisteProdutoJanelaNel(Convert.ToInt32(item.Cells[2].Text), 
                                                                  Convert.ToInt32(item.Cells[4].Text), 
                                                                  Convert.ToInt32(ddlJanela.SelectedValue),
                                                                  Convert.ToInt32(ddlProforma.SelectedValue),
                                                                  Convert.ToInt32(ddlColecao.SelectedValue),
                                                                  Convert.ToInt32(ddlNel.SelectedValue)).Count == 0)
                        {
                            nelProduto.CODIGO_COLECAO = Convert.ToInt32(ddlColecao.SelectedValue);
                            nelProduto.CODIGO_JANELA = Convert.ToInt32(ddlJanela.SelectedValue);
                            nelProduto.CODIGO_PROFORMA = Convert.ToInt32(ddlProforma.SelectedValue);
                            nelProduto.CODIGO_FORNECEDOR = Convert.ToInt32(ddlFornecedor.SelectedValue);
                            nelProduto.CODIGO_PROFORMA = Convert.ToInt32(item.Cells[0].Text);
                            nelProduto.CODIGO_GRIFFE = Convert.ToInt32(item.Cells[1].Text);
                            nelProduto.CODIGO_PRODUTO = Convert.ToInt32(item.Cells[2].Text);
                            nelProduto.CODIGO_PRODUTO_COR = Convert.ToInt32(item.Cells[4].Text);
                            nelProduto.GRUPO_PRODUTO = item.Cells[6].Text;
                            nelProduto.FOB = Convert.ToDecimal(item.Cells[7].Text);
                            nelProduto.CODIGO_NEL = Convert.ToInt32(ddlNel.SelectedValue);

                            baseController.IncluirNelProduto(nelProduto);

                            contProduto++;
                        }
                    }
                }
            }

            LabelFeedBack.Text = "Formam vinculados " + contProduto + " Produtos !!!";
        }

        protected void GridViewProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_PROFORMA_PRODUTO proformaProduto;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literalDescricaoProduto = e.Row.FindControl("LiteralDescricaoProduto") as Literal;

                if (literalDescricaoProduto != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        proformaProduto = e.Row.DataItem as IMPORTACAO_PROFORMA_PRODUTO;

                        literalDescricaoProduto.Text = baseController.BuscaProduto(proformaProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                        Literal literalDescricaoProdutoCor = e.Row.FindControl("LiteralDescricaoProdutoCor") as Literal;

                        if (literalDescricaoProdutoCor != null)
                            literalDescricaoProdutoCor.Text = baseController.BuscaProdutoCor(proformaProduto.CODIGO_PRODUTO.ToString(), proformaProduto.CODIGO_PRODUTO_COR.ToString()).DESC_COR_PRODUTO;
                    }
                }
            }
        }
    }
}
