using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;

namespace Relatorios
{
    public partial class gerloja_mais_vendidos : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        int vendaLoja = 0;
        int estoqueLoja = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";


                CarregarColecoes();
                CarregarSemanas();
                CarregarFiliais();
                CarregarGriffe();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();
        }
        private void CarregarSemanas()
        {
            var semana = lojaController.ObterSemanaVenda();

            semana.Insert(0, new LOJA_VENDA_SEMANA { CODIGO = 0, SEMANA = "Selecione" });

            ddlSemana.DataSource = semana;
            ddlSemana.DataBind();
        }
        private void CarregarFiliais()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais_Intermediario();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlSemana.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Semana...";
                    return;
                }

                CarregarMaisVendidos();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }
        private void CarregarMaisVendidos()
        {
            var listaProdutoVendido = new List<ProdutoVendido>();

            var semanaSel = lojaController.ObterSemanaVenda(Convert.ToInt32(ddlSemana.SelectedValue));

            var listaMaisVendidos = baseController.ObterMaisVendidos(semanaSel.DATA_INI, semanaSel.DATA_FIM, ddlColecao.SelectedValue, ddlFilial.SelectedValue, ddlGriffe.SelectedValue);

            if (ddlGriffe.SelectedValue != "")
            {
                listaMaisVendidos = listaMaisVendidos.Where(x => x.GRIFFE.Trim() == ddlGriffe.SelectedValue.Trim()).ToList();
            }

            if (listaMaisVendidos != null)
            {
                ProdutoVendido produtoVendido = null;

                int indice = 1;

                foreach (SP_OBTER_MAIS_VENDIDOSResult item in listaMaisVendidos)
                {
                    if (indice == 1 || indice == 6 || indice == 11 || indice == 16 || indice == 21 || indice == 26
                        || indice == 31 || indice == 36 || indice == 41 || indice == 46 || indice == 51 || indice == 56)
                    {
                        produtoVendido = new ProdutoVendido();
                        produtoVendido.CodigoProduto_1 = item.PRODUTO;
                        produtoVendido.CorProduto_1 = item.COR_PRODUTO;
                        produtoVendido.DescricaoCorProduto_1 = item.DESC_COR;
                        produtoVendido.DescricaoProduto_1 = item.NOME;
                        produtoVendido.QtdeVendida_1 = item.QTDE_LOJA;
                        produtoVendido.QtdeVendidaRede_1 = item.QTDE_REDE;
                        produtoVendido.QtdeEstoque_1 = item.ESTOQUE_LOJA;
                        produtoVendido.FotoPNG_1 = item.FOTOPNG;
                        produtoVendido.FotoJPG_1 = item.FOTOJPG;
                        produtoVendido.FotoDP_1 = item.FOTODP;
                    }
                    else if (indice == 2 || indice == 7 || indice == 12 || indice == 17 || indice == 22 || indice == 27
                        || indice == 32 || indice == 37 || indice == 42 || indice == 47 || indice == 52 || indice == 57)
                    {
                        produtoVendido.CodigoProduto_2 = item.PRODUTO;
                        produtoVendido.CorProduto_2 = item.COR_PRODUTO;
                        produtoVendido.DescricaoCorProduto_2 = item.DESC_COR;
                        produtoVendido.DescricaoProduto_2 = item.NOME;
                        produtoVendido.QtdeVendida_2 = item.QTDE_LOJA;
                        produtoVendido.QtdeVendidaRede_2 = item.QTDE_REDE;
                        produtoVendido.QtdeEstoque_2 = item.ESTOQUE_LOJA;
                        produtoVendido.FotoPNG_2 = item.FOTOPNG;
                        produtoVendido.FotoJPG_2 = item.FOTOJPG;
                        produtoVendido.FotoDP_2 = item.FOTODP;
                    }
                    else if (indice == 3 || indice == 8 || indice == 13 || indice == 18 || indice == 23 || indice == 28
                        || indice == 33 || indice == 38 || indice == 43 || indice == 48 || indice == 53 || indice == 58)
                    {
                        produtoVendido.CodigoProduto_3 = item.PRODUTO;
                        produtoVendido.CorProduto_3 = item.COR_PRODUTO;
                        produtoVendido.DescricaoCorProduto_3 = item.DESC_COR;
                        produtoVendido.DescricaoProduto_3 = item.NOME;
                        produtoVendido.QtdeVendida_3 = item.QTDE_LOJA;
                        produtoVendido.QtdeVendidaRede_3 = item.QTDE_REDE;
                        produtoVendido.QtdeEstoque_3 = item.ESTOQUE_LOJA;
                        produtoVendido.FotoPNG_3 = item.FOTOPNG;
                        produtoVendido.FotoJPG_3 = item.FOTOJPG;
                        produtoVendido.FotoDP_3 = item.FOTODP;
                    }
                    else if (indice == 4 || indice == 9 || indice == 14 || indice == 19 || indice == 24 || indice == 29
                        || indice == 34 || indice == 39 || indice == 44 || indice == 49 || indice == 54 || indice == 59)
                    {
                        produtoVendido.CodigoProduto_4 = item.PRODUTO;
                        produtoVendido.CorProduto_4 = item.COR_PRODUTO;
                        produtoVendido.DescricaoCorProduto_4 = item.DESC_COR;
                        produtoVendido.DescricaoProduto_4 = item.NOME;
                        produtoVendido.QtdeVendida_4 = item.QTDE_LOJA;
                        produtoVendido.QtdeVendidaRede_4 = item.QTDE_REDE;
                        produtoVendido.QtdeEstoque_4 = item.ESTOQUE_LOJA;
                        produtoVendido.FotoPNG_4 = item.FOTOPNG;
                        produtoVendido.FotoJPG_4 = item.FOTOJPG;
                        produtoVendido.FotoDP_4 = item.FOTODP;
                    }
                    else if (indice == 5 || indice == 10 || indice == 15 || indice == 20 || indice == 25 || indice == 30
                        || indice == 35 || indice == 40 || indice == 45 || indice == 50 || indice == 55 || indice == 60)
                    {
                        produtoVendido.CodigoProduto_5 = item.PRODUTO;
                        produtoVendido.CorProduto_5 = item.COR_PRODUTO;
                        produtoVendido.DescricaoCorProduto_5 = item.DESC_COR;
                        produtoVendido.DescricaoProduto_5 = item.NOME;
                        produtoVendido.QtdeVendida_5 = item.QTDE_LOJA;
                        produtoVendido.QtdeVendidaRede_5 = item.QTDE_REDE;
                        produtoVendido.QtdeEstoque_5 = item.ESTOQUE_LOJA;
                        produtoVendido.FotoPNG_5 = item.FOTOPNG;
                        produtoVendido.FotoJPG_5 = item.FOTOJPG;
                        produtoVendido.FotoDP_5 = item.FOTODP;

                        listaProdutoVendido.Add(produtoVendido);
                    }

                    indice++;
                }
            }

            repVendidos.DataSource = listaProdutoVendido;
            repVendidos.DataBind();

            var listaMaisVendidosLoja = baseController.ObterMaisVendidosLoja(semanaSel.DATA_INI, semanaSel.DATA_FIM, ddlColecao.SelectedValue, ddlFilial.SelectedValue, ddlGriffe.SelectedValue);

            gvMaisVendidos.DataSource = listaMaisVendidosLoja;
            gvMaisVendidos.DataBind();
        }

        protected void repVendidos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ProdutoVendido produtoVendido = e.Item.DataItem as ProdutoVendido;

            if (produtoVendido != null)
            {
                Literal litProduto1 = e.Item.FindControl("litProduto1") as Literal;
                litProduto1.Text = produtoVendido.CodigoProduto_1;
                Literal litNome1 = e.Item.FindControl("litNome1") as Literal;
                litNome1.Text = produtoVendido.DescricaoProduto_1;
                Literal litCor1 = e.Item.FindControl("litCor1") as Literal;
                litCor1.Text = produtoVendido.DescricaoCorProduto_1;
                Literal litQtdeVendidaRede1 = e.Item.FindControl("litQtdeVendidaRede1") as Literal;
                litQtdeVendidaRede1.Text = produtoVendido.QtdeVendidaRede_1.ToString();
                Literal litQtdeVendidaLoja1 = e.Item.FindControl("litQtdeVendidaLoja1") as Literal;
                litQtdeVendidaLoja1.Text = produtoVendido.QtdeVendida_1.ToString();
                Literal litEstoqueLoja1 = e.Item.FindControl("litEstoqueLoja1") as Literal;
                litEstoqueLoja1.Text = produtoVendido.QtdeEstoque_1.ToString();
                Image imgProduto1 = e.Item.FindControl("imgProduto1") as Image;
                if (File.Exists(Server.MapPath(produtoVendido.FotoPNG_1)))
                    imgProduto1.ImageUrl = produtoVendido.FotoPNG_1;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoJPG_1)))
                    imgProduto1.ImageUrl = produtoVendido.FotoJPG_1;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoDP_1)))
                    imgProduto1.ImageUrl = produtoVendido.FotoDP_1;
                else
                    imgProduto1.ImageUrl = "/Fotos/sem_foto.png";

                Literal litProduto2 = e.Item.FindControl("litProduto2") as Literal;
                litProduto2.Text = produtoVendido.CodigoProduto_2;
                Literal litNome2 = e.Item.FindControl("litNome2") as Literal;
                litNome2.Text = produtoVendido.DescricaoProduto_2;
                Literal litCor2 = e.Item.FindControl("litCor2") as Literal;
                litCor2.Text = produtoVendido.DescricaoCorProduto_2;
                Literal litQtdeVendidaRede2 = e.Item.FindControl("litQtdeVendidaRede2") as Literal;
                litQtdeVendidaRede2.Text = produtoVendido.QtdeVendidaRede_2.ToString();
                Literal litQtdeVendidaLoja2 = e.Item.FindControl("litQtdeVendidaLoja2") as Literal;
                litQtdeVendidaLoja2.Text = produtoVendido.QtdeVendida_2.ToString();
                Literal litEstoqueLoja2 = e.Item.FindControl("litEstoqueLoja2") as Literal;
                litEstoqueLoja2.Text = produtoVendido.QtdeEstoque_2.ToString();
                Image imgProduto2 = e.Item.FindControl("imgProduto2") as Image;
                if (File.Exists(Server.MapPath(produtoVendido.FotoPNG_2)))
                    imgProduto2.ImageUrl = produtoVendido.FotoPNG_2;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoJPG_2)))
                    imgProduto2.ImageUrl = produtoVendido.FotoJPG_2;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoDP_2)))
                    imgProduto2.ImageUrl = produtoVendido.FotoDP_2;
                else
                    imgProduto2.ImageUrl = "/Fotos/sem_foto.png";

                Literal litProduto3 = e.Item.FindControl("litProduto3") as Literal;
                litProduto3.Text = produtoVendido.CodigoProduto_3;
                Literal litNome3 = e.Item.FindControl("litNome3") as Literal;
                litNome3.Text = produtoVendido.DescricaoProduto_3;
                Literal litCor3 = e.Item.FindControl("litCor3") as Literal;
                litCor3.Text = produtoVendido.DescricaoCorProduto_3;
                Literal litQtdeVendidaRede3 = e.Item.FindControl("litQtdeVendidaRede3") as Literal;
                litQtdeVendidaRede3.Text = produtoVendido.QtdeVendidaRede_3.ToString();
                Literal litQtdeVendidaLoja3 = e.Item.FindControl("litQtdeVendidaLoja3") as Literal;
                litQtdeVendidaLoja3.Text = produtoVendido.QtdeVendida_3.ToString();
                Literal litEstoqueLoja3 = e.Item.FindControl("litEstoqueLoja3") as Literal;
                litEstoqueLoja3.Text = produtoVendido.QtdeEstoque_3.ToString();
                Image imgProduto3 = e.Item.FindControl("imgProduto3") as Image;
                if (File.Exists(Server.MapPath(produtoVendido.FotoPNG_3)))
                    imgProduto3.ImageUrl = produtoVendido.FotoPNG_3;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoJPG_3)))
                    imgProduto3.ImageUrl = produtoVendido.FotoJPG_3;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoDP_3)))
                    imgProduto3.ImageUrl = produtoVendido.FotoDP_3;
                else
                    imgProduto3.ImageUrl = "/Fotos/sem_foto.png";

                Literal litProduto4 = e.Item.FindControl("litProduto4") as Literal;
                litProduto4.Text = produtoVendido.CodigoProduto_4;
                Literal litNome4 = e.Item.FindControl("litNome4") as Literal;
                litNome4.Text = produtoVendido.DescricaoProduto_4;
                Literal litCor4 = e.Item.FindControl("litCor4") as Literal;
                litCor4.Text = produtoVendido.DescricaoCorProduto_4;
                Literal litQtdeVendidaRede4 = e.Item.FindControl("litQtdeVendidaRede4") as Literal;
                litQtdeVendidaRede4.Text = produtoVendido.QtdeVendidaRede_4.ToString();
                Literal litQtdeVendidaLoja4 = e.Item.FindControl("litQtdeVendidaLoja4") as Literal;
                litQtdeVendidaLoja4.Text = produtoVendido.QtdeVendida_4.ToString();
                Literal litEstoqueLoja4 = e.Item.FindControl("litEstoqueLoja4") as Literal;
                litEstoqueLoja4.Text = produtoVendido.QtdeEstoque_4.ToString();
                Image imgProduto4 = e.Item.FindControl("imgProduto4") as Image;
                if (File.Exists(Server.MapPath(produtoVendido.FotoPNG_4)))
                    imgProduto4.ImageUrl = produtoVendido.FotoPNG_4;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoJPG_4)))
                    imgProduto4.ImageUrl = produtoVendido.FotoJPG_4;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoDP_4)))
                    imgProduto4.ImageUrl = produtoVendido.FotoDP_4;
                else
                    imgProduto4.ImageUrl = "/Fotos/sem_foto.png";

                Literal litProduto5 = e.Item.FindControl("litProduto5") as Literal;
                litProduto5.Text = produtoVendido.CodigoProduto_5;
                Literal litNome5 = e.Item.FindControl("litNome5") as Literal;
                litNome5.Text = produtoVendido.DescricaoProduto_5;
                Literal litCor5 = e.Item.FindControl("litCor5") as Literal;
                litCor5.Text = produtoVendido.DescricaoCorProduto_5;
                Literal litQtdeVendidaRede5 = e.Item.FindControl("litQtdeVendidaRede5") as Literal;
                litQtdeVendidaRede5.Text = produtoVendido.QtdeVendidaRede_5.ToString();
                Literal litQtdeVendidaLoja5 = e.Item.FindControl("litQtdeVendidaLoja5") as Literal;
                litQtdeVendidaLoja5.Text = produtoVendido.QtdeVendida_5.ToString();
                Literal litEstoqueLoja5 = e.Item.FindControl("litEstoqueLoja5") as Literal;
                litEstoqueLoja5.Text = produtoVendido.QtdeEstoque_5.ToString();
                Image imgProduto5 = e.Item.FindControl("imgProduto5") as Image;
                if (File.Exists(Server.MapPath(produtoVendido.FotoPNG_5)))
                    imgProduto5.ImageUrl = produtoVendido.FotoPNG_5;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoJPG_5)))
                    imgProduto5.ImageUrl = produtoVendido.FotoJPG_5;
                else if (File.Exists(Server.MapPath(produtoVendido.FotoDP_5)))
                    imgProduto5.ImageUrl = produtoVendido.FotoDP_5;
                else
                    imgProduto5.ImageUrl = "/Fotos/sem_foto.png";
            }
        }

        protected void gvMaisVendidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_MAIS_VENDIDOS_LOJAResult produtoVendido = e.Row.DataItem as SP_OBTER_MAIS_VENDIDOS_LOJAResult;
            if (produtoVendido != null)
            {

                Image imgProduto = e.Row.FindControl("imgProduto") as Image;
                if (File.Exists(Server.MapPath(produtoVendido.FOTOPNG)))
                    imgProduto.ImageUrl = produtoVendido.FOTOPNG;
                else if (File.Exists(Server.MapPath(produtoVendido.FOTOJPG)))
                    imgProduto.ImageUrl = produtoVendido.FOTOJPG;
                else if (File.Exists(Server.MapPath(produtoVendido.FOTODP)))
                    imgProduto.ImageUrl = produtoVendido.FOTODP;
                else
                    imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                vendaLoja += Convert.ToInt32(produtoVendido.QTDE_LOJA);
                estoqueLoja += produtoVendido.ESTOQUE_LOJA;
            }

        }
        protected void gvMaisVendidos_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvMaisVendidos.FooterRow;
            if (footer != null)
            {
                footer.Cells[5].Text = vendaLoja.ToString();
                footer.Cells[6].Text = estoqueLoja.ToString();

            }
        }



    }
}
