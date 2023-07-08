using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Services;

namespace Relatorios
{
    public partial class ecom_ordem_produtobloco : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";


                CarregarProdutoBloco();

            }
        }

        [WebMethod]
        public static string SalvarOrdem(string[] produtos, int codigoBloco)
        {
            EcomController ecomController = new EcomController();

            int ordem = 1;
            foreach (var p in produtos)
            {
                var blocoProduto = ecomController.ObterBlocoProdutoOrdemPorIdMagProduto(codigoBloco, Convert.ToInt32(p));
                if (blocoProduto != null)
                {
                    blocoProduto.ORDEM = ordem;
                    ecomController.AtualizarBlocoProdutoOrdem(blocoProduto);

                    ordem += 1;
                }
            }

            return "";
        }

        private void CarregarProdutoBloco()
        {
            var produtos = ecomController.ObterProdutoBlocoOrdem(3);

            var produtoBloco1 = new List<SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult>();
            var produtoBloco2 = new List<SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult>();
            var produtoBloco3 = new List<SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult>();
            int i = 0;
            foreach (var p in produtos)
            {
                i = i + 1;

                if (i == 1)
                {
                    produtoBloco1.Add(p);
                }
                else if (i == 2)
                {
                    produtoBloco2.Add(p);
                }
                else if (i == 3)
                {
                    produtoBloco3.Add(p);
                    i = 0;
                }
            }

            gvProduto1.DataSource = produtoBloco1;
            gvProduto1.DataBind();

            gvProduto2.DataSource = produtoBloco2;
            gvProduto2.DataBind();

            gvProduto3.DataSource = produtoBloco3;
            gvProduto3.DataBind();

        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult bloco = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult;

                    Label labProdutoTitulo1 = e.Row.FindControl("labProdutoTitulo1") as Label;
                    labProdutoTitulo1.Text = bloco.PRODUTO + " " + bloco.NOME;

                    System.Web.UI.WebControls.Image imgFrenteCab1 = e.Row.FindControl("imgFrenteCab1") as System.Web.UI.WebControls.Image;
                    imgFrenteCab1.ImageUrl = bloco.FOTO_FRENTE_CAB;

                }
            }
        }


    }
}


