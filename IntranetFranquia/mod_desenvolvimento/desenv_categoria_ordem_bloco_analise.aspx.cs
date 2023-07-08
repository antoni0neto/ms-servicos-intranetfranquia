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
using System.Web.UI.HtmlControls;

namespace Relatorios
{
    public partial class desenv_categoria_ordem_bloco_analise : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "desenv_menu.aspx";
                CarregarColecoes();
                CarregarGriffe();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();
        }
        private void CarregarGriffe()
        {
            var griffes = baseController.BuscaGriffes();
            griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "Selecione" });

            ddlGriffe.DataSource = griffes;
            ddlGriffe.DataBind();
        }
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCategorias(ddlColecao.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim());
        }
        protected void ddlGriffe_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCategorias(ddlColecao.SelectedValue.Trim(), ddlGriffe.SelectedValue.Trim());
        }
        private void CarregarCategorias(string colecao, string griffe)
        {
            var categorias = desenvController.ObterCategoria(colecao, griffe).OrderBy(p => p.NOME).ToList();
            categorias.Insert(0, new DESENV_CATEGORIA { CODIGO = 0, NOME = "" });
            lstCategoria.DataSource = categorias;
            lstCategoria.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                if (ddlColecao.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Coleção.";
                    return;
                }

                if (ddlGriffe.SelectedValue == "Selecione")
                {
                    labMsg.Text = "Selecione a Griffe.";
                    return;
                }

                CarregarProdutoCategorias();

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        private void CarregarProdutoCategorias()
        {
            var produtos = desenvController.ObterCarrinhoProdutoAnalise(ddlColecao.SelectedValue, ddlGriffe.SelectedValue, 0);

            // lista de categorias
            var categoriasCon = "";
            if (lstCategoria.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstCategoria.GetSelectedIndices())
                {
                    var itemList = lstCategoria.Items[v].Value.Trim() + ",";
                    categoriasCon = categoriasCon + itemList;
                }

                categoriasCon = categoriasCon + ",";
                categoriasCon = categoriasCon.Replace(",,", "");
            }
            if (categoriasCon != "" && categoriasCon != "0")
                produtos = produtos.Where(p => categoriasCon.Contains(p.CODIGO_CATEGORIA.ToString().Trim())).ToList();

            repProdutos.DataSource = produtos;
            repProdutos.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { sortBlocoProduto(); });", true);
        }
        protected void repProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SP_OBTER_DESENV_ANALISEPRODUTOResult produto = e.Item.DataItem as SP_OBTER_DESENV_ANALISEPRODUTOResult;

            if (produto != null)
            {
                HiddenField hidCodigoProduto = e.Item.FindControl("hidCodigoProduto") as HiddenField;
                hidCodigoProduto.Value = produto.DESENV_PRODUTO.ToString();

                Label labOrigem = e.Item.FindControl("labOrigem") as Label;
                labOrigem.Text = produto.CATEGORIA + " - " + produto.ORIGEM.ToString();

                Label labProdutoTitulo = e.Item.FindControl("labProdutoTitulo") as Label;
                labProdutoTitulo.Text = produto.PRODUTO.Trim() + " " + produto.DESC_PRODUTO.Trim();

                Label labTecido = e.Item.FindControl("labTecido") as Label;
                labTecido.Text = produto.TECIDO;

                Label labCor = e.Item.FindControl("labCor") as Label;
                labCor.Text = produto.DESC_COR;

                Label labCorFornecedor = e.Item.FindControl("labCorFornecedor") as Label;
                labCorFornecedor.Text = produto.COR_FORNECEDOR;

                Label labQtdeVarejo = e.Item.FindControl("labQtdeVarejo") as Label;
                labQtdeVarejo.Text = produto.QTDE_VAREJO.ToString();

                HtmlTableCell tdBack = e.Item.FindControl("tdBack") as HtmlTableCell;
                tdBack.Style.Add("background", "url('" + produto.COR_TECIDO.Replace("~", "..") + "')");


                System.Web.UI.WebControls.Image imgProduto = e.Item.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                if (File.Exists(Server.MapPath(produto.FOTO1)))
                    imgProduto.ImageUrl = produto.FOTO1;
                else if (File.Exists(Server.MapPath(produto.FOTO2)))
                    imgProduto.ImageUrl = produto.FOTO2;
                else
                    imgProduto.ImageUrl = "/Fotos/sem_foto.png";



            }
        }


    }
}


