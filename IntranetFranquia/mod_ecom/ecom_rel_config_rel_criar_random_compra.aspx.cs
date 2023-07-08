using DAL;
using Relatorios.mod_ecom.correio;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_rel_config_rel_criar_random_compra : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["prod"] == null || Request.QueryString["prod"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");


                var produto = Request.QueryString["prod"].ToString();
                var cor = Request.QueryString["cor"].ToString();

                CarregarClientesCompraraTB(produto, cor);
            }
        }

        private void CarregarClientesCompraraTB(string produto, string cor)
        {
            var listaProdutoVendido = new List<ProdutoVendido>();

            var compraramTB = eController.ObterProdutoClientesCompraramTB(produto, cor);

            if (compraramTB != null)
            {
                ProdutoVendido produtoVendido = null;

                int indice = 1;

                foreach (var item in compraramTB)
                {
                    if (indice == 1 || indice == 4 || indice == 7 || indice == 10 || indice == 13 || indice == 16
                        || indice == 19 || indice == 22 || indice == 25 || indice == 28 || indice == 31)
                    {
                        produtoVendido = new ProdutoVendido();
                        produtoVendido.CodigoProduto_1 = item.SKU;
                        produtoVendido.FotoPNG_1 = item.FOTO_FRENTE_CAB;
                        produtoVendido.QtdeEstoque_1 = item.ESTOQUE;
                    }
                    else if (indice == 2 || indice == 5 || indice == 8 || indice == 11 || indice == 14 || indice == 17
                        || indice == 20 || indice == 23 || indice == 26 || indice == 29 || indice == 32)
                    {
                        produtoVendido.CodigoProduto_2 = item.SKU;
                        produtoVendido.FotoPNG_2 = item.FOTO_FRENTE_CAB;
                        produtoVendido.QtdeEstoque_2 = item.ESTOQUE;
                    }
                    else if (indice == 3 || indice == 6 || indice == 9 || indice == 12 || indice == 15 || indice == 18
                        || indice == 21 || indice == 24 || indice == 27 || indice == 30 || indice == 33)
                    {
                        produtoVendido.CodigoProduto_3 = item.SKU;
                        produtoVendido.FotoPNG_3 = item.FOTO_FRENTE_CAB;
                        produtoVendido.QtdeEstoque_3 = item.ESTOQUE;

                        listaProdutoVendido.Add(produtoVendido);
                    }

                    indice++;
                }
            }

            repCompraramTB.DataSource = listaProdutoVendido;
            repCompraramTB.DataBind();
        }

        protected void repCompraramTB_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ProdutoVendido produtoVendido = e.Item.DataItem as ProdutoVendido;

            if (produtoVendido != null)
            {
                Literal litSKU1 = e.Item.FindControl("litSKU1") as Literal;
                litSKU1.Text = produtoVendido.CodigoProduto_1;
                Literal litEstoque1 = e.Item.FindControl("litEstoque1") as Literal;
                litEstoque1.Text = produtoVendido.QtdeEstoque_1.ToString();
                Image imgProduto1 = e.Item.FindControl("imgProduto1") as Image;
                imgProduto1.ImageUrl = produtoVendido.FotoPNG_1;

                Literal litSKU2 = e.Item.FindControl("litSKU2") as Literal;
                litSKU2.Text = produtoVendido.CodigoProduto_2;
                Literal litEstoque2 = e.Item.FindControl("litEstoque2") as Literal;
                litEstoque2.Text = produtoVendido.QtdeEstoque_2.ToString();
                Image imgProduto2 = e.Item.FindControl("imgProduto2") as Image;
                imgProduto2.ImageUrl = produtoVendido.FotoPNG_2;

                Literal litSKU3 = e.Item.FindControl("litSKU3") as Literal;
                litSKU3.Text = produtoVendido.CodigoProduto_3;
                Literal litEstoque3 = e.Item.FindControl("litEstoque3") as Literal;
                litEstoque3.Text = produtoVendido.QtdeEstoque_3.ToString();
                Image imgProduto3 = e.Item.FindControl("imgProduto3") as Image;
                imgProduto3.ImageUrl = produtoVendido.FotoPNG_3;


            }
        }
    }
}


