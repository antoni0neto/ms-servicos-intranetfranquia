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
    public partial class ecom_ordem_produtoblocov2 : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";

                CarregarCategoriaMag();
            }
        }

        [WebMethod]
        public static string SalvarOrdem(string[] produtos, int codigoBloco)
        {
            EcomController ecomController = new EcomController();

            int ordem = 9999;
            foreach (var p in produtos)
            {
                var blocoProduto = ecomController.ObterBlocoProdutoOrdemPorIdMagProduto(codigoBloco, Convert.ToInt32(p));
                if (blocoProduto != null)
                {
                    blocoProduto.ORDEM = ordem;
                    ecomController.AtualizarBlocoProdutoOrdem(blocoProduto);

                    ordem -= 1;
                }
            }

            return "";
        }

        private void CarregarCategoriaMag()
        {
            var catMag = ecomController.ObterMagentoGrupoProdutoAberto();
            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();
        }
        protected void ddlCategoriaMag_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarBloco(Convert.ToInt32(ddlCategoriaMag.SelectedValue));
        }
        private void CarregarBloco(int ecomGrupoProduto)
        {
            var bloco = ecomController.ObterBlocoPorCategoriaMag(ecomGrupoProduto);
            bloco.Insert(0, new ECOM_BLOCO_PRODUTO { CODIGO = 0, BLOCO = "Selecione" });

            ddlBloco.DataSource = bloco;
            ddlBloco.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {


            try
            {
                labMsg.Text = "";

                if (ddlCategoriaMag.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione a Categoria Magento.";
                    return;
                }

                if (ddlBloco.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione o Bloco.";
                    return;
                }

                CarregarProdutoBloco(Convert.ToInt32(ddlBloco.SelectedValue));

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarProdutoBloco(int codigoBloco)
        {
            var produtos = ecomController.ObterProdutoBlocoOrdem(codigoBloco);

            repProdutos.DataSource = produtos;
            repProdutos.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { sortBlocoProduto(); });", true);
        }
        protected void repProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult produto = e.Item.DataItem as SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult;

            if (produto != null)
            {
                HiddenField hidIdMag = e.Item.FindControl("hidIdMag") as HiddenField;
                hidIdMag.Value = produto.ID_PRODUTO_MAG.ToString();

                Label labIdMag = e.Item.FindControl("labIdMag") as Label;
                labIdMag.Text = produto.ID_PRODUTO_MAG.ToString();

                Label labProdutoTitulo = e.Item.FindControl("labProdutoTitulo") as Label;
                labProdutoTitulo.Text = produto.PRODUTO.Trim() + " " + produto.NOME.Trim();

                Label labEstoque = e.Item.FindControl("labEstoque") as Label;
                labEstoque.Text = "ESTOQUE " + produto.ESTOQUE.ToString();
                if (produto.ESTOQUE <= 1)
                    labEstoque.ForeColor = Color.Red;

                Label labUltimaAtu = e.Item.FindControl("labUltimaAtu") as Label;
                labUltimaAtu.Text = (produto.DATA_ULT_ATUALIZACAO == null) ? "-" : Convert.ToDateTime(produto.DATA_ULT_ATUALIZACAO).ToString("dd/MM/yyyy");

                System.Web.UI.WebControls.Image imgFrenteCab = e.Item.FindControl("imgFrenteCab") as System.Web.UI.WebControls.Image;
                if (File.Exists(Server.MapPath(produto.FOTO_FRENTE_CAB)))
                    imgFrenteCab.ImageUrl = produto.FOTO_FRENTE_CAB;

                var imgOver = "this.src='" + produto.FOTO_COSTAS.Replace("~", "..") + "'";
                var imgOut = "this.src='" + produto.FOTO_FRENTE_CAB.Replace("~", "..") + "'";

                imgFrenteCab.Attributes.Add("onmouseover", imgOver);
                imgFrenteCab.Attributes.Add("onmouseout", imgOut);
            }
        }

        protected void btRandom_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlBloco.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione o Bloco.";
                    return;
                }

                ecomController.AtualizarEcomBlocoProdutoRandom(Convert.ToInt32(ddlBloco.SelectedValue), 0, 1);

                CarregarProdutoBloco(Convert.ToInt32(ddlBloco.SelectedValue));

            }
            catch (Exception)
            {

                throw;
            }
        }






    }
}


