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
    public partial class AlteraQtdeNel : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListJanela();
            }
        }

        private void CarregaDropDownListNel()
        {
            ddlNel.DataSource = baseController.BuscaNel(Convert.ToInt32(ddlColecao.SelectedValue), Convert.ToInt32(ddlJanela.SelectedValue));
            ddlNel.DataBind();
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
        
        private void CarregaGridViewProdutos()
        {
            GridViewProdutos.DataSource = baseController.BuscaNelProdutos(Convert.ToInt32(ddlNel.SelectedValue));
            GridViewProdutos.DataBind();
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

            CarregaGridViewProdutos();

            btGravar.Enabled = true;
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            int contProduto = 0;

            IMPORTACAO_NEL_PRODUTO nelProduto = new IMPORTACAO_NEL_PRODUTO();

            int qtTotal;
            string qtXp;
            string qtPp;
            string qtPq;
            string qtMd;
            string qtGd;
            string qtGg;
            string fob;

            foreach (GridViewRow item in GridViewProdutos.Rows)
            {
                qtTotal = 0;
                qtXp = "0";
                qtPp = "0";
                qtPq = "0";
                qtMd = "0";
                qtGd = "0";
                qtGg = "0";
                fob = "0";

                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        TextBox txtQtdeXp = item.FindControl("txtQtdeXp") as TextBox;

                        if (txtQtdeXp != null)
                        {
                            if (!txtQtdeXp.Text.Equals(""))
                            {
                                qtXp = txtQtdeXp.Text;
                                qtTotal += Convert.ToInt32(txtQtdeXp.Text);
                            }
                        }

                        TextBox txtQtdePp = item.FindControl("txtQtdePp") as TextBox;

                        if (txtQtdePp != null)
                        {
                            if (!txtQtdePp.Text.Equals(""))
                            {
                                qtPp = txtQtdePp.Text;
                                qtTotal += Convert.ToInt32(txtQtdePp.Text);
                            }
                        }

                        TextBox txtQtdePq = item.FindControl("txtQtdePq") as TextBox;

                        if (txtQtdePq != null)
                        {
                            if (!txtQtdePq.Text.Equals(""))
                            {
                                qtPq = txtQtdePq.Text;
                                qtTotal += Convert.ToInt32(txtQtdePq.Text);
                            }
                        }

                        TextBox txtQtdeMd = item.FindControl("txtQtdeMd") as TextBox;

                        if (txtQtdeMd != null)
                        {
                            if (!txtQtdeMd.Text.Equals(""))
                            {
                                qtMd = txtQtdeMd.Text;
                                qtTotal += Convert.ToInt32(txtQtdeMd.Text);
                            }
                        }

                        TextBox txtQtdeGd = item.FindControl("txtQtdeGd") as TextBox;

                        if (txtQtdeGd != null)
                        {
                            if (!txtQtdeGd.Text.Equals(""))
                            {
                                qtGd = txtQtdeGd.Text;
                                qtTotal += Convert.ToInt32(txtQtdeGd.Text);
                            }
                        }

                        TextBox txtQtdeGg = item.FindControl("txtQtdeGg") as TextBox;

                        if (txtQtdeGg != null)
                        {
                            if (!txtQtdeGg.Text.Equals(""))
                            {
                                qtGg = txtQtdeGg.Text;
                                qtTotal += Convert.ToInt32(txtQtdeGg.Text);
                            }
                        }

                        TextBox txtFob = item.FindControl("txtFob") as TextBox;

                        if (txtFob != null)
                        {
                            if (!txtFob.Text.Equals(""))
                                fob = txtFob.Text;
                        }

                        baseController.AtualizaNelProduto(GridViewProdutos.DataKeys[item.RowIndex].Value.ToString(), qtTotal.ToString(), qtXp, qtPp, qtPq, qtMd, qtGd, qtGg, fob);

                        contProduto++;

                        CarregaGridViewProdutos();
                    }
                }
            }

            LabelFeedBack.Text = "Foram gravados " + contProduto + " Produtos !!!";
        }

        protected void GridViewProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_NEL_PRODUTO nelProduto = e.Row.DataItem as IMPORTACAO_NEL_PRODUTO;

            if (nelProduto != null)
            {
                Literal literalDescricao = e.Row.FindControl("LiteralDescricao") as Literal;

                if (literalDescricao != null)
                    literalDescricao.Text = baseController.BuscaProduto(nelProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                Literal literalCor = e.Row.FindControl("LiteralCor") as Literal;

                if (literalCor != null)
                    literalCor.Text = baseController.BuscaProdutoCor(nelProduto.CODIGO_PRODUTO.ToString(), nelProduto.CODIGO_PRODUTO_COR.ToString()).DESC_COR_PRODUTO;

                TextBox txtQtdeTotal = e.Row.FindControl("txtQtdeTotal") as TextBox;

                if (txtQtdeTotal != null)
                    txtQtdeTotal.Text = nelProduto.QTDE_TOTAL.ToString();

                TextBox txtQtdeXp = e.Row.FindControl("txtQtdeXp") as TextBox;

                if (txtQtdeXp != null)
                    txtQtdeXp.Text = nelProduto.QTDE_XP.ToString();

                TextBox txtQtdePp = e.Row.FindControl("txtQtdePp") as TextBox;

                if (txtQtdePp != null)
                    txtQtdePp.Text = nelProduto.QTDE_PP.ToString();

                TextBox txtQtdePq = e.Row.FindControl("txtQtdePq") as TextBox;

                if (txtQtdePq != null)
                    txtQtdePq.Text = nelProduto.QTDE_PQ.ToString();

                TextBox txtQtdeMd = e.Row.FindControl("txtQtdeMd") as TextBox;

                if (txtQtdeMd != null)
                    txtQtdeMd.Text = nelProduto.QTDE_MD.ToString();

                TextBox txtQtdeGd = e.Row.FindControl("txtQtdeGd") as TextBox;

                if (txtQtdeGd != null)
                    txtQtdeGd.Text = nelProduto.QTDE_GD.ToString();

                TextBox txtQtdeGg = e.Row.FindControl("txtQtdeGg") as TextBox;

                if (txtQtdeGg != null)
                    txtQtdeGg.Text = nelProduto.QTDE_GG.ToString();

                TextBox txtFob = e.Row.FindControl("txtFob") as TextBox;

                if (txtFob != null)
                    txtFob.Text = nelProduto.FOB.ToString();
            }
        }
    }
}
