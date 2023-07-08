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
    public partial class MaisVendidos : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListSemana454();
                CarregaDropDownListFilial();
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListSemana454()
        {
            ddlSemana454.DataSource = baseController.BuscaDatas(2012);
            ddlSemana454.DataBind();
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

        private void CarregaGridViewColecaoProduto()
        {
            List<ProdutoVendido> listaProdutoVendido = new List<ProdutoVendido>();

            List<p_busca_mais_vendidosResult> listaMaisVendidos = baseController.BuscaMaisVendidos(ddlSemana454.SelectedValue,
                                                                                                   "01",
                                                                                                   ddlColecao.SelectedValue,
                                                                                                   ddlFilial.SelectedValue);
            if (listaMaisVendidos != null)
            {
                ProdutoVendido produtoVendido = null;

                int indice = 1;

                foreach (p_busca_mais_vendidosResult item in listaMaisVendidos)
                {
                    if (indice == 1 || indice == 6 || indice == 11 || indice == 16 || indice == 21 || indice == 26)
                    {
                        produtoVendido = new ProdutoVendido();

                        produtoVendido.CodigoProduto_1 = item.produto;
                        produtoVendido.CorProduto_1 = item.cor;
                        produtoVendido.DescricaoCorProduto_1 = item.descricao_cor;
                        produtoVendido.DescricaoProduto_1 = item.descricao;
                        produtoVendido.QtdeVendida_1 = item.qtde_loja;
                        produtoVendido.QtdeVendidaRede_1 = item.qtde_geral;

                        ESTOQUE_PRODUTO estoqueProduto = baseController.BuscaEstoqueLoja(item.produto, item.cor, ddlFilial.SelectedItem.ToString().Trim());

                        if (estoqueProduto != null)
                        {
                            if (estoqueProduto.ESTOQUE != null)
                                produtoVendido.QtdeEstoque_1 = Convert.ToInt32(estoqueProduto.ESTOQUE);
                            else
                                produtoVendido.QtdeEstoque_1 = 0;
                        }
                    }

                    if (indice == 2 || indice == 7 || indice == 12 || indice == 17 || indice == 22 || indice == 27)
                    {
                        produtoVendido.CodigoProduto_2 = item.produto;
                        produtoVendido.CorProduto_2 = item.cor;
                        produtoVendido.DescricaoCorProduto_2 = item.descricao_cor;
                        produtoVendido.DescricaoProduto_2 = item.descricao;
                        produtoVendido.QtdeVendida_2 = item.qtde_loja;
                        produtoVendido.QtdeVendidaRede_2 = item.qtde_geral;

                        ESTOQUE_PRODUTO estoqueProduto = baseController.BuscaEstoqueLoja(item.produto, item.cor, ddlFilial.SelectedItem.ToString().Trim());

                        if (estoqueProduto != null)
                        {
                            if (estoqueProduto.ESTOQUE != null)
                                produtoVendido.QtdeEstoque_2 = Convert.ToInt32(estoqueProduto.ESTOQUE);
                            else
                                produtoVendido.QtdeEstoque_2 = 0;
                        }
                    }

                    if (indice == 3 || indice == 8 || indice == 13 || indice == 18 || indice == 23 || indice == 28)
                    {
                        produtoVendido.CodigoProduto_3 = item.produto;
                        produtoVendido.CorProduto_3 = item.cor;
                        produtoVendido.DescricaoCorProduto_3 = item.descricao_cor;
                        produtoVendido.DescricaoProduto_3 = item.descricao;
                        produtoVendido.QtdeVendida_3 = item.qtde_loja;
                        produtoVendido.QtdeVendidaRede_3 = item.qtde_geral;

                        ESTOQUE_PRODUTO estoqueProduto = baseController.BuscaEstoqueLoja(item.produto, item.cor, ddlFilial.SelectedItem.ToString().Trim());

                        if (estoqueProduto != null)
                        {
                            if (estoqueProduto.ESTOQUE != null)
                                produtoVendido.QtdeEstoque_3 = Convert.ToInt32(estoqueProduto.ESTOQUE);
                            else
                                produtoVendido.QtdeEstoque_3 = 0;
                        }
                    }

                    if (indice == 4 || indice == 9 || indice == 14 || indice == 19 || indice == 24 || indice == 29)
                    {
                        produtoVendido.CodigoProduto_4 = item.produto;
                        produtoVendido.CorProduto_4 = item.cor;
                        produtoVendido.DescricaoCorProduto_4 = item.descricao_cor;
                        produtoVendido.DescricaoProduto_4 = item.descricao;
                        produtoVendido.QtdeVendida_4 = item.qtde_loja;
                        produtoVendido.QtdeVendidaRede_4 = item.qtde_geral;

                        ESTOQUE_PRODUTO estoqueProduto = baseController.BuscaEstoqueLoja(item.produto, item.cor, ddlFilial.SelectedItem.ToString().Trim());

                        if (estoqueProduto != null)
                        {
                            if (estoqueProduto.ESTOQUE != null)
                                produtoVendido.QtdeEstoque_4 = Convert.ToInt32(estoqueProduto.ESTOQUE);
                            else
                                produtoVendido.QtdeEstoque_4 = 0;
                        }
                    }

                    if (indice == 5 || indice == 10 || indice == 15 || indice == 20 || indice == 25 || indice == 30)
                    {
                        produtoVendido.CodigoProduto_5 = item.produto;
                        produtoVendido.CorProduto_5 = item.cor;
                        produtoVendido.DescricaoCorProduto_5 = item.descricao_cor;
                        produtoVendido.DescricaoProduto_5 = item.descricao;
                        produtoVendido.QtdeVendida_5 = item.qtde_loja;
                        produtoVendido.QtdeVendidaRede_5 = item.qtde_geral;

                        ESTOQUE_PRODUTO estoqueProduto = baseController.BuscaEstoqueLoja(item.produto, item.cor, ddlFilial.SelectedItem.ToString().Trim());

                        if (estoqueProduto != null)
                        {
                            if (estoqueProduto.ESTOQUE != null)
                                produtoVendido.QtdeEstoque_5 = Convert.ToInt32(estoqueProduto.ESTOQUE);
                            else
                                produtoVendido.QtdeEstoque_5 = 0;
                        }

                        listaProdutoVendido.Add(produtoVendido);
                    }

                    indice++;
                }
            }

            Repeater1.DataSource = listaProdutoVendido;
            Repeater1.DataBind();

            List<p_busca_mais_vendidos_lojaResult> listaMaisVendidosLoja = baseController.BuscaMaisVendidosLoja(ddlSemana454.SelectedValue,
                                                                                                                "01",
                                                                                                                ddlColecao.SelectedValue,
                                                                                                                ddlFilial.SelectedValue);

            GridViewMaisVendidos.DataSource = listaMaisVendidosLoja;
            GridViewMaisVendidos.DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ProdutoVendido produtoVendido = e.Item.DataItem as ProdutoVendido;

            if (produtoVendido != null)
            {
                Literal literalCodigoProduto_1 = e.Item.FindControl("LiteralCodigoProduto_1") as Literal;

                if (literalCodigoProduto_1 != null)
                    literalCodigoProduto_1.Text = produtoVendido.CodigoProduto_1;

                Literal literalDescricaoProduto_1 = e.Item.FindControl("LiteralDescricaoProduto_1") as Literal;

                if (literalDescricaoProduto_1 != null)
                    literalDescricaoProduto_1.Text = produtoVendido.DescricaoProduto_1;

                Literal literalDescricaoCor_1 = e.Item.FindControl("LiteralDescricaoCor_1") as Literal;

                if (literalDescricaoCor_1 != null)
                    literalDescricaoCor_1.Text = produtoVendido.DescricaoCorProduto_1;

                Literal literalQtdeVendida_1 = e.Item.FindControl("LiteralQtdeVendida_1") as Literal;

                if (literalQtdeVendida_1 != null)
                    literalQtdeVendida_1.Text = produtoVendido.QtdeVendida_1.ToString();

                Literal literalQtdeVendidaRede_1 = e.Item.FindControl("LiteralQtdeVendidaRede_1") as Literal;

                if (literalQtdeVendidaRede_1 != null)
                    literalQtdeVendidaRede_1.Text = produtoVendido.QtdeVendidaRede_1.ToString();

                Literal literalQtdeEstoque_1 = e.Item.FindControl("LiteralQtdeEstoque_1") as Literal;

                if (literalQtdeEstoque_1 != null)
                    literalQtdeEstoque_1.Text = produtoVendido.QtdeEstoque_1.ToString();

                Image imageProduto_1 = e.Item.FindControl("ImageProduto_1") as Image;

                if (imageProduto_1 != null)
                {
                    if (ddlColecao.SelectedValue.Contains("16"))
                        imageProduto_1.ImageUrl = "~/Image/COLECAO_16/" + produtoVendido.CodigoProduto_1 + produtoVendido.CorProduto_1 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("17"))
                        imageProduto_1.ImageUrl = "~/Image/COLECAO_17/" + produtoVendido.CodigoProduto_1 + produtoVendido.CorProduto_1 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("18"))
                        imageProduto_1.ImageUrl = "~/Image/COLEÇÃO 18_VERÃO/" + produtoVendido.CodigoProduto_1 + produtoVendido.CorProduto_1 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("19"))
                        imageProduto_1.ImageUrl = "~/Image/COLECAO_19/" + produtoVendido.CodigoProduto_1 + produtoVendido.CorProduto_1 + ".jpg";
                }

                Literal literalCodigoProduto_2 = e.Item.FindControl("LiteralCodigoProduto_2") as Literal;

                if (literalCodigoProduto_2 != null)
                    literalCodigoProduto_2.Text = produtoVendido.CodigoProduto_2;

                Literal literalDescricaoProduto_2 = e.Item.FindControl("LiteralDescricaoProduto_2") as Literal;

                if (literalDescricaoProduto_2 != null)
                    literalDescricaoProduto_2.Text = produtoVendido.DescricaoProduto_2;

                Literal literalDescricaoCor_2 = e.Item.FindControl("LiteralDescricaoCor_2") as Literal;

                if (literalDescricaoCor_2 != null)
                    literalDescricaoCor_2.Text = produtoVendido.DescricaoCorProduto_2;

                Literal literalQtdeVendida_2 = e.Item.FindControl("LiteralQtdeVendida_2") as Literal;

                if (literalQtdeVendida_2 != null)
                    literalQtdeVendida_2.Text = produtoVendido.QtdeVendida_2.ToString();

                Literal literalQtdeVendidaRede_2 = e.Item.FindControl("LiteralQtdeVendidaRede_2") as Literal;

                if (literalQtdeVendidaRede_2 != null)
                    literalQtdeVendidaRede_2.Text = produtoVendido.QtdeVendidaRede_2.ToString();

                Literal literalQtdeEstoque_2 = e.Item.FindControl("LiteralQtdeEstoque_2") as Literal;

                if (literalQtdeEstoque_2 != null)
                    literalQtdeEstoque_2.Text = produtoVendido.QtdeEstoque_2.ToString();

                Image imageProduto_2 = e.Item.FindControl("ImageProduto_2") as Image;

                if (imageProduto_2 != null)
                {
                    if (ddlColecao.SelectedValue.Contains("16"))
                        imageProduto_2.ImageUrl = "~/Image/COLECAO_16/" + produtoVendido.CodigoProduto_2 + produtoVendido.CorProduto_2 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("17"))
                        imageProduto_2.ImageUrl = "~/Image/COLECAO_17/" + produtoVendido.CodigoProduto_2 + produtoVendido.CorProduto_2 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("18"))
                        imageProduto_2.ImageUrl = "~/Image/COLEÇÃO 18_VERÃO/" + produtoVendido.CodigoProduto_2 + produtoVendido.CorProduto_2 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("19"))
                        imageProduto_2.ImageUrl = "~/Image/COLECAO_19/" + produtoVendido.CodigoProduto_2 + produtoVendido.CorProduto_2 + ".jpg";
                }

                Literal literalCodigoProduto_3 = e.Item.FindControl("LiteralCodigoProduto_3") as Literal;

                if (literalCodigoProduto_3 != null)
                    literalCodigoProduto_3.Text = produtoVendido.CodigoProduto_3;

                Literal literalDescricaoProduto_3 = e.Item.FindControl("LiteralDescricaoProduto_3") as Literal;

                if (literalDescricaoProduto_3 != null)
                    literalDescricaoProduto_3.Text = produtoVendido.DescricaoProduto_3;

                Literal literalDescricaoCor_3 = e.Item.FindControl("LiteralDescricaoCor_3") as Literal;

                if (literalDescricaoCor_3 != null)
                    literalDescricaoCor_3.Text = produtoVendido.DescricaoCorProduto_3;

                Literal literalQtdeVendida_3 = e.Item.FindControl("LiteralQtdeVendida_3") as Literal;

                if (literalQtdeVendida_3 != null)
                    literalQtdeVendida_3.Text = produtoVendido.QtdeVendida_3.ToString();

                Literal literalQtdeVendidaRede_3 = e.Item.FindControl("LiteralQtdeVendidaRede_3") as Literal;

                if (literalQtdeVendidaRede_3 != null)
                    literalQtdeVendidaRede_3.Text = produtoVendido.QtdeVendidaRede_3.ToString();

                Literal literalQtdeEstoque_3 = e.Item.FindControl("LiteralQtdeEstoque_3") as Literal;

                if (literalQtdeEstoque_3 != null)
                    literalQtdeEstoque_3.Text = produtoVendido.QtdeEstoque_3.ToString();

                Image imageProduto_3 = e.Item.FindControl("ImageProduto_3") as Image;

                if (imageProduto_3 != null)
                {
                    if (ddlColecao.SelectedValue.Contains("16"))
                        imageProduto_3.ImageUrl = "~/Image/COLECAO_16/" + produtoVendido.CodigoProduto_3 + produtoVendido.CorProduto_3 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("17"))
                        imageProduto_3.ImageUrl = "~/Image/COLECAO_17/" + produtoVendido.CodigoProduto_3 + produtoVendido.CorProduto_3 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("18"))
                        imageProduto_3.ImageUrl = "~/Image/COLEÇÃO 18_VERÃO/" + produtoVendido.CodigoProduto_3 + produtoVendido.CorProduto_3 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("19"))
                        imageProduto_3.ImageUrl = "~/Image/COLECAO_19/" + produtoVendido.CodigoProduto_3 + produtoVendido.CorProduto_3 + ".jpg";
                }

                Literal literalCodigoProduto_4 = e.Item.FindControl("LiteralCodigoProduto_4") as Literal;

                if (literalCodigoProduto_4 != null)
                    literalCodigoProduto_4.Text = produtoVendido.CodigoProduto_4;

                Literal literalDescricaoProduto_4 = e.Item.FindControl("LiteralDescricaoProduto_4") as Literal;

                if (literalDescricaoProduto_4 != null)
                    literalDescricaoProduto_4.Text = produtoVendido.DescricaoProduto_4;

                Literal literalDescricaoCor_4 = e.Item.FindControl("LiteralDescricaoCor_4") as Literal;

                if (literalDescricaoCor_4 != null)
                    literalDescricaoCor_4.Text = produtoVendido.DescricaoCorProduto_4;

                Literal literalQtdeVendida_4 = e.Item.FindControl("LiteralQtdeVendida_4") as Literal;

                if (literalQtdeVendida_4 != null)
                    literalQtdeVendida_4.Text = produtoVendido.QtdeVendida_4.ToString();

                Literal literalQtdeVendidaRede_4 = e.Item.FindControl("LiteralQtdeVendidaRede_4") as Literal;

                if (literalQtdeVendidaRede_4 != null)
                    literalQtdeVendidaRede_4.Text = produtoVendido.QtdeVendidaRede_4.ToString();

                Literal literalQtdeEstoque_4 = e.Item.FindControl("LiteralQtdeEstoque_4") as Literal;

                if (literalQtdeEstoque_4 != null)
                    literalQtdeEstoque_4.Text = produtoVendido.QtdeEstoque_4.ToString();

                Image imageProduto_4 = e.Item.FindControl("ImageProduto_4") as Image;

                if (imageProduto_4 != null)
                {
                    if (ddlColecao.SelectedValue.Contains("16"))
                        imageProduto_4.ImageUrl = "~/Image/COLECAO_16/" + produtoVendido.CodigoProduto_4 + produtoVendido.CorProduto_4 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("17"))
                        imageProduto_4.ImageUrl = "~/Image/COLECAO_17/" + produtoVendido.CodigoProduto_4 + produtoVendido.CorProduto_4 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("18"))
                        imageProduto_4.ImageUrl = "~/Image/COLEÇÃO 18_VERÃO/" + produtoVendido.CodigoProduto_4 + produtoVendido.CorProduto_4 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("19"))
                        imageProduto_4.ImageUrl = "~/Image/COLECAO_19/" + produtoVendido.CodigoProduto_4 + produtoVendido.CorProduto_4 + ".jpg";
                }

                Literal literalCodigoProduto_5 = e.Item.FindControl("LiteralCodigoProduto_5") as Literal;

                if (literalCodigoProduto_5 != null)
                    literalCodigoProduto_5.Text = produtoVendido.CodigoProduto_5;

                Literal literalDescricaoProduto_5 = e.Item.FindControl("LiteralDescricaoProduto_5") as Literal;

                if (literalDescricaoProduto_5 != null)
                    literalDescricaoProduto_5.Text = produtoVendido.DescricaoProduto_5;

                Literal literalDescricaoCor_5 = e.Item.FindControl("LiteralDescricaoCor_5") as Literal;

                if (literalDescricaoCor_5 != null)
                    literalDescricaoCor_5.Text = produtoVendido.DescricaoCorProduto_5;

                Literal literalQtdeVendida_5 = e.Item.FindControl("LiteralQtdeVendida_5") as Literal;

                if (literalQtdeVendida_5 != null)
                    literalQtdeVendida_5.Text = produtoVendido.QtdeVendida_5.ToString();

                Literal literalQtdeVendidaRede_5 = e.Item.FindControl("LiteralQtdeVendidaRede_5") as Literal;

                if (literalQtdeVendidaRede_5 != null)
                    literalQtdeVendidaRede_5.Text = produtoVendido.QtdeVendidaRede_5.ToString();

                Literal literalQtdeEstoque_5 = e.Item.FindControl("LiteralQtdeEstoque_5") as Literal;

                if (literalQtdeEstoque_5 != null)
                    literalQtdeEstoque_5.Text = produtoVendido.QtdeEstoque_5.ToString();

                Image imageProduto_5 = e.Item.FindControl("ImageProduto_5") as Image;

                if (imageProduto_5 != null)
                {
                    if (ddlColecao.SelectedValue.Contains("16"))
                        imageProduto_5.ImageUrl = "~/Image/COLECAO_16/" + produtoVendido.CodigoProduto_5 + produtoVendido.CorProduto_5 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("17"))
                        imageProduto_5.ImageUrl = "~/Image/COLECAO_17/" + produtoVendido.CodigoProduto_5 + produtoVendido.CorProduto_5 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("18"))
                        imageProduto_5.ImageUrl = "~/Image/COLEÇÃO 18_VERÃO/" + produtoVendido.CodigoProduto_5 + produtoVendido.CorProduto_5 + ".jpg";
                    if (ddlColecao.SelectedValue.Contains("19"))
                        imageProduto_5.ImageUrl = "~/Image/COLECAO_19/" + produtoVendido.CodigoProduto_5 + produtoVendido.CorProduto_5 + ".jpg";
                }
            }
        }

        protected void ddlSemana454_DataBound(object sender, EventArgs e)
        {
            ddlSemana454.Items.Add(new ListItem("Selecione", "0"));
            ddlSemana454.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void GridViewMaisVendidos_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewMaisVendidos.Rows)
            {
                Literal literalCor = item.FindControl("LiteralCor") as Literal;

                if (literalCor != null)
                {
                    Sp_Busca_Produto_CorResult cor = baseController.BuscaProdutoCor(item.Cells[0].Text, item.Cells[2].Text);

                    if (cor != null)
                        literalCor.Text = cor.DESC_COR_PRODUTO;
                }
            }
        }

        protected void ButtonPesquisarMovimento_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("") ||
                ddlSemana454.SelectedValue.ToString().Equals("0") ||
                ddlSemana454.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewColecaoProduto();
        }
    }
}
