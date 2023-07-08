using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Mail;
using System.Net;

namespace Relatorios
{
    public partial class ConsultaValeMercadoria : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtCodigoProduto.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "',event)");
            txtCodigoProduto.Focus();
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            Button btGravar = sender as Button;

            if (btGravar != null)
            {
                PRODUTO itemProduto = baseController.BuscaProduto(txtCodigoProduto.Text);

                if (itemProduto != null)
                {
                    Produto produtoGrid = new Produto();

                    produtoGrid.CodigoBarra = txtCodigoProduto.Text;
                    produtoGrid.DescricaoProduto = itemProduto.DESC_PRODUTO;
                    produtoGrid.GradeProduto = itemProduto.GRADE;

                    PRODUTOS_PRECO produtoPreco = baseController.BuscaPrecoOriginalProduto(itemProduto.PRODUTO1);

                    if (produtoPreco != null)
                    {
                        produtoGrid.PrecoOriginal = Convert.ToDecimal(produtoPreco.PRECO1);
                        produtoGrid.Desconto = (Convert.ToDecimal(produtoPreco.PRECO1) * 40) / 100;
                        produtoGrid.PrecoLiquido = produtoGrid.PrecoOriginal - produtoGrid.Desconto;

                        string texto = produtoGrid.PrecoLiquido.ToString().Substring(0, produtoGrid.Desconto.ToString().Length - 1);

                        produtoGrid.PrecoLiquido = Convert.ToDecimal(texto);

                        texto = produtoGrid.Desconto.ToString().Substring(0, produtoGrid.Desconto.ToString().Length-1);

                        produtoGrid.Desconto = Convert.ToDecimal(texto);

                        PRODUTOS_PRECO produtoPrecoLoja = baseController.BuscaPrecoLojaProduto(itemProduto.PRODUTO1);

                        if (produtoPrecoLoja != null)
                            produtoGrid.PrecoLoja = Convert.ToDecimal(produtoPrecoLoja.PRECO1);
                        else
                            produtoGrid.PrecoLoja = 0;
                    }
                    else
                    {
                        produtoGrid.PrecoOriginal = 0;
                        produtoGrid.PrecoLiquido = 0;
                        produtoGrid.PrecoLoja = 0;
                        produtoGrid.Desconto = 0;
                    }

                    List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];

                    produtoGrid.IndiceProduto = listaProduto.Count() + 1;

                    listaProduto.Add(produtoGrid);

                    CarregaGridViewProduto(listaProduto);

                    Session["PRODUTO"] = listaProduto;

                    LimpaTela();
                }
            }
        }

        protected void btSomar_Click(object sender, EventArgs e)
        {
            Button btSalvar = sender as Button;

            if (btSalvar != null)
            {
                List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];
                    
                try
                {
                    decimal desconto = 0;

                    foreach (Produto item in listaProduto)
                    {
                        if (item.PrecoLoja > item.PrecoLiquido)
                            desconto += item.PrecoLoja - item.PrecoLiquido;
                    }

                    txtTotalDesconto.Text = desconto.ToString("#,###.#0");
                }
                catch (Exception ex)
                {
                    LabelFeedBack.Text = "Erro : " + ex.Message;
                    return;
                }
            }
        }

        private void CarregaGridViewProduto(List<Produto> produto)
        {
            GridViewProduto.DataSource = produto;
            GridViewProduto.DataBind();
        }

        private void LimpaTela()
        {
            txtCodigoProduto.Text = string.Empty;
        }

        protected void ButtonDeletar_Click(object sender, EventArgs e)
        {
            List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];

            Button buttonDeletar = sender as Button;

            if (buttonDeletar != null)
            {
                foreach (Produto item in listaProduto)
                {
                    if (item.IndiceProduto == Convert.ToInt32(buttonDeletar.CommandArgument))
                    {
                        listaProduto.Remove(item);

                        CarregaGridViewProduto(listaProduto);

                        break;
                    }
                }
            }
        }

        protected void GridViewProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Produto produto = e.Row.DataItem as Produto;

            if (produto != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button buttonDeletar = e.Row.FindControl("ButtonDeletar") as Button;

                    if (buttonDeletar != null)
                    {
                        if (e.Row.DataItem != null)
                        {
                            Produto itemProduto = e.Row.DataItem as Produto;

                            buttonDeletar.CommandArgument = itemProduto.IndiceProduto.ToString();
                        }
                    }
                }
            }
        }
    }
}