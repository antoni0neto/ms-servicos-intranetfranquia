using DAL;
using Relatorios.mod_ecomv2.mag2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class ecomv2_ordem_produtobloco_catfinal : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                CarregarCategoriaMag();

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btAtualizarMagento.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizarMagento, null) + ";");
        }


        private void CarregarCategoriaMag()
        {
            var catMag = ecomController.ObterMagentoGrupoProdutoAberto();
            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();
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

                CarregarProdutoCategoria(Convert.ToInt32(ddlCategoriaMag.SelectedValue));

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarProdutoCategoria(int ecomGrupoProduto)
        {

            var bloco = ecomController.ObterBlocoPorCategoriaMag(ecomGrupoProduto);

            var produtos = new List<SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult>();
            foreach (var b in bloco)
            {
                var prods = ecomController.ObterProdutoBlocoOrdem(b.CODIGO);
                produtos.AddRange(prods);
            }

            repProdutos.DataSource = produtos.OrderBy(p => p.ORDEM_BLOCO).ThenByDescending(x => x.ORDEM_PRODUTO);
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

                Label labBloco = e.Item.FindControl("labBloco") as Label;
                labBloco.Text = produto.BLOCO;

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

        protected void btAtualizarMagento_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";


                var bloco = ecomController.ObterBlocoPorCategoriaMag(Convert.ToInt32(ddlCategoriaMag.SelectedValue));
                var produtos = new List<SP_OBTER_ECOM_PRODUTO_BLOCO_ORDEMResult>();
                foreach (var b in bloco)
                {
                    var prods = ecomController.ObterProdutoBlocoOrdem(b.CODIGO);
                    produtos.AddRange(prods);
                }
                produtos = produtos.OrderBy(p => p.ORDEM_BLOCO).ThenByDescending(p => p.ORDEM_PRODUTO).ToList();

                var mag = new MagentoV2();
                int counter = 1;
                foreach (var p in produtos)
                {
                    if (p.ID_PRODUTO_MAG > 0)
                    {
                        mag.AtualizarCategoria(p.ECOM_GRUPO_PRODUTO, p.SKU, counter);

                        // gerar log ultima atualizacao
                        var bProdOrd = ecomController.ObterBlocoProdutoOrdemPorCodigo(p.CODIGO_ORDEM);
                        bProdOrd.DATA_ULT_ATUALIZACAO = DateTime.Now;
                        ecomController.AtualizarBlocoProdutoOrdem(bProdOrd);

                        var hist = new ECOM_BLOCO_PRODUTO_ORDEM_HIST();
                        hist.ECOM_PRODUTO = p.ECOM_PRODUTO;
                        hist.ECOM_GRUPO_PRODUTO = p.ECOM_GRUPO_PRODUTO;
                        hist.ECOM_BLOCO_PRODUTO = p.ECOM_BLOCO_PRODUTO;
                        hist.ORDEM = counter;
                        hist.DATA_ATUALIZACAO = DateTime.Now;
                        ecomController.InserirBlocoProdutoOrdemHist(hist);

                        counter = counter + 1;
                    }
                }

                labMsg.Text = "Ordenação realizada com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('../mod_ecom/ecom_menu.aspx', '_self'); } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message + ex.InnerException + ex.StackTrace;
            }
        }

        protected void btRandomBloco_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategoriaMag.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione a Categoria Magento.";
                    return;
                }

                ecomController.AtualizarEcomBlocoProdutoRandom(0, Convert.ToInt32(ddlCategoriaMag.SelectedValue), 2);

                CarregarProdutoCategoria(Convert.ToInt32(ddlCategoriaMag.SelectedValue));

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btRandomTudo_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategoriaMag.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione a Categoria Magento.";
                    return;
                }

                ecomController.AtualizarEcomBlocoProdutoRandom(0, Convert.ToInt32(ddlCategoriaMag.SelectedValue), 3);

                CarregarProdutoCategoria(Convert.ToInt32(ddlCategoriaMag.SelectedValue));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
