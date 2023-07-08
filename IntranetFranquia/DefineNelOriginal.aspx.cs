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
    public partial class DefineNelOriginal : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListJanela();
            }
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

        private void CarregaGridViewNelProdutos()
        {
            GridViewNelProdutos.DataSource = baseController.BuscaNelProdutos(Convert.ToInt32(ddlNel.SelectedValue));
            GridViewNelProdutos.DataBind();
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

        protected void btBuscarNel_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("") ||
                ddlJanela.SelectedValue.ToString().Equals("0") ||
                ddlJanela.SelectedValue.ToString().Equals(""))
                return;

            CarregaDropDownListNel();

            btBuscarProdutos.Enabled = true;
        }

        protected void btBuscarProdutos_Click(object sender, EventArgs e)
        {
            if (ddlNel.SelectedValue.ToString().Equals("0") ||
                ddlNel.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewNelProdutos();

            btReplica.Enabled = true;
        }

        protected void btReplica_Click(object sender, EventArgs e)
        {
            int contProduto = 0;

            IMPORTACAO_NEL nel = baseController.BuscaNelPeloCodigo(Convert.ToInt32(ddlNel.SelectedValue));

            if (nel != null)
            {
                if (nel.ORIGINAL != null)
                {
                    if (Convert.ToBoolean(nel.ORIGINAL))
                    {
                        LabelFeedBack.Text = "NeL já é Original !!!";
                        return;
                    }
                    else
                    {
                        IMPORTACAO_NEL_PRODUTO nelProduto = new IMPORTACAO_NEL_PRODUTO();

                        foreach (GridViewRow item in GridViewNelProdutos.Rows)
                        {
                            baseController.AtualizaNelOriginalProduto(item.Cells[0].Text, item.Cells[5].Text, item.Cells[6].Text, item.Cells[7].Text, item.Cells[8].Text, item.Cells[9].Text, item.Cells[10].Text, item.Cells[11].Text);

                            contProduto++;
                        }

                        LabelFeedBack.Text = "Foram vinculados " + contProduto + " Produtos !!!";

                        nel.ORIGINAL = true;

                        usuarioController.AtualizaNel(nel);
                    }
                }
            }
        }

        protected void GridViewNelProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_NEL_PRODUTO nelProduto;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literalDescricaoProduto = e.Row.FindControl("LiteralDescricaoProduto") as Literal;

                if (literalDescricaoProduto != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        nelProduto = e.Row.DataItem as IMPORTACAO_NEL_PRODUTO;

                        literalDescricaoProduto.Text = baseController.BuscaProduto(nelProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                        Literal literalDescricaoProdutoCor = e.Row.FindControl("LiteralDescricaoProdutoCor") as Literal;

                        if (literalDescricaoProdutoCor != null)
                            literalDescricaoProdutoCor.Text = baseController.BuscaProdutoCor(nelProduto.CODIGO_PRODUTO.ToString(), nelProduto.CODIGO_PRODUTO_COR.ToString()).DESC_COR_PRODUTO;
                    }
                }
            }
        }
    }
}
