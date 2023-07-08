using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Collections;
using Relatorios.mod_ecom.mag;

namespace Relatorios
{
    public partial class photo_register : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            //txtCodigoBarra.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "', event)");
            txtCodigoBarra.Focus();

            if (!Page.IsPostBack)
            {
                txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");

                CarregarFotoMaquina();
            }
        }

        #region "FOTOS DA MAQUINA"
        private List<ECOM_FOTO_MAQUINA> ObterFotoMaquinaPorData()
        {
            return eController.ObterFotoMaquinaPorData(DateTime.Today);
        }
        private void CarregarFotoMaquina()
        {
            var fotoMaquina = ObterFotoMaquinaPorData();

            gvFotoMaquina.DataSource = fotoMaquina;
            gvFotoMaquina.DataBind();
        }
        protected void gvFotoMaquina_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_FOTO_MAQUINA fotoMaq = e.Row.DataItem as ECOM_FOTO_MAQUINA;

                    if (fotoMaq != null)
                    {

                        TextBox _txtSeqInicial = e.Row.FindControl("txtSeqInicial") as TextBox;
                        if (_txtSeqInicial != null)
                        {
                            if (fotoMaq.SEQ_INICIAL != null)
                            {
                                var seqIni = ("0000" + fotoMaq.SEQ_INICIAL.ToString());
                                _txtSeqInicial.Text = seqIni.Substring(seqIni.Length - 4, 4);
                            }
                        }

                        TextBox _txtSeqFinal = e.Row.FindControl("txtSeqFinal") as TextBox;
                        if (_txtSeqFinal != null)
                        {
                            if (fotoMaq.SEQ_FINAL != null)
                            {
                                var seqFim = ("0000" + fotoMaq.SEQ_FINAL.ToString());
                                _txtSeqFinal.Text = seqFim.Substring(seqFim.Length - 4, 4);
                            }
                        }

                        Button _btSalvar = e.Row.FindControl("btSalvar") as Button;
                        if (_btSalvar != null)
                        {
                            _btSalvar.CommandArgument = fotoMaq.CODIGO.ToString();
                        }

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = fotoMaq.CODIGO.ToString();
                        }

                        GridView gvMaqProduto = e.Row.FindControl("gvFotoMaquinaProduto") as GridView;
                        gvMaqProduto.DataSource = eController.ObterFotoMaquinaProdutoPorMaquinaFOTO(fotoMaq.CODIGO);
                        gvMaqProduto.DataBind();

                    }
                }
            }
        }
        protected void gvFotoMaquinaProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FOTO_MAQUINA_PROD_FOTOResult fotoMaqProd = e.Row.DataItem as SP_OBTER_FOTO_MAQUINA_PROD_FOTOResult;

                    if (fotoMaqProd != null)
                    {
                        ImageButton imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                        if (fotoMaqProd.FOTO == null)
                        {
                            imgProduto.Height = Unit.Pixel(25);
                            imgProduto.Width = Unit.Pixel(25);
                            imgProduto.ImageUrl = "~/Image/no_image.png";
                        }
                        else
                        {
                            imgProduto.Width = Unit.Pixel(40);
                            imgProduto.ImageUrl = fotoMaqProd.FOTO;
                        }
                    }
                }
            }
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                string codigoBarraProduto = "";
                string descOutro = "";
                string produtoFiltro = "";
                string corFiltro = "";

                if (ObterUsuario() == null)
                {
                    Response.Redirect("../Login.aspx");
                    return;
                }

                codigoBarraProduto = txtCodigoBarra.Text.Trim();
                descOutro = txtDescOutro.Text.Trim();
                produtoFiltro = txtProduto.Text.Trim();
                corFiltro = ddlCor.SelectedValue.Trim();

                if (codigoBarraProduto == "" && descOutro == "" && produtoFiltro == "" && corFiltro == "")
                {
                    labErro.Text = "Informe o Código de Barras do Produto, Produto e Cor ou descreva o Produto sem Etiqueta...";
                    return;
                }

                if (codigoBarraProduto == "" && descOutro == "" && (produtoFiltro == "" || corFiltro == ""))
                {
                    labErro.Text = "Informe o Produto e Cor...";
                    return;
                }


                var produtoBarra = baseController.BuscaProdutoBarra(codigoBarraProduto);
                if (produtoBarra != null)
                {
                    var produto = produtoBarra.PRODUTO.Trim();
                    var descProduto = baseController.BuscaProduto(produto).DESC_PRODUTO.Trim();
                    var cor = produtoBarra.COR_PRODUTO.Trim();
                    var descCor = prodController.ObterCoresBasicas(cor).DESC_COR.Trim();

                    var fMaqProduto = eController.ObterFotoMaquinaProdutoUltimoAberto();
                    if (fMaqProduto != null)
                    {
                        fMaqProduto.PRODUTO = produto;
                        fMaqProduto.DESC_PRODUTO = descProduto;
                        fMaqProduto.COR = cor;
                        fMaqProduto.DESC_COR = descCor;
                        fMaqProduto.DESC_OUTRO = "";

                        eController.AtualizarFotoMaquinaProduto(fMaqProduto);

                        CarregarFotoMaquina();
                    }
                    else
                    {
                        labErro.Text = "Nenhum Produto em Aberto. Favor abrir os produtos, clicando em \"1 Peça\" ou \"2 Peças\" ou \"3 Peças\"...";
                        return;
                    }
                }
                else
                {
                    if (descOutro != "")
                    {
                        var fMaqProduto = eController.ObterFotoMaquinaProdutoUltimoAberto();
                        if (fMaqProduto != null)
                        {
                            fMaqProduto.PRODUTO = produtoFiltro;
                            fMaqProduto.DESC_PRODUTO = "";
                            fMaqProduto.COR = corFiltro;
                            fMaqProduto.DESC_COR = "";
                            fMaqProduto.DESC_OUTRO = descOutro;

                            eController.AtualizarFotoMaquinaProduto(fMaqProduto);

                            CarregarFotoMaquina();
                        }
                        else
                        {
                            labErro.Text = "Nenhum Produto em Aberto. Favor abrir os produtos, clicando em \"1 Peça\" ou \"2 Peças\" ou \"3 Peças\"...";
                            return;
                        }
                    }
                    else if (produtoFiltro != "" && corFiltro != "")
                    {
                        var fMaqProduto = eController.ObterFotoMaquinaProdutoUltimoAberto();
                        if (fMaqProduto != null)
                        {
                            var descProduto = baseController.BuscaProduto(produtoFiltro).DESC_PRODUTO.Trim();
                            var descCor = prodController.ObterCoresBasicas(corFiltro).DESC_COR.Trim();

                            fMaqProduto.PRODUTO = produtoFiltro;
                            fMaqProduto.DESC_PRODUTO = descProduto;
                            fMaqProduto.COR = corFiltro;
                            fMaqProduto.DESC_COR = descCor;
                            fMaqProduto.DESC_OUTRO = descOutro;

                            eController.AtualizarFotoMaquinaProduto(fMaqProduto);

                            CarregarFotoMaquina();
                        }
                        else
                        {
                            labErro.Text = "Nenhum Produto em Aberto. Favor abrir os produtos, clicando em \"1 Peça\" ou \"2 Peças\" ou \"3 Peças\"...";
                            return;
                        }
                    }
                    else
                    {
                        labErro.Text = "Produto não cadastrado no Linx...";
                    }
                }

                txtCodigoBarra.Text = "";
                txtDescOutro.Text = "";
                txtProduto.Text = "";
                ddlCor.SelectedValue = "";

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }

        protected void btProduto1_Click(object sender, EventArgs e)
        {
            try
            {
                var fMaq = new ECOM_FOTO_MAQUINA();
                fMaq.DATA_FOTO = Convert.ToDateTime(txtData.Text);
                fMaq.DATA_INCLUSAO = DateTime.Now;

                var codigoFotoMaq = eController.InserirFotoMaquina(fMaq);

                var fMaqProd1 = new ECOM_FOTO_MAQUINA_PROD();
                fMaqProd1.ECOM_FOTO_MAQUINA = codigoFotoMaq;
                eController.InserirFotoMaquinaProduto(fMaqProd1);

                CarregarFotoMaquina();
            }
            catch (Exception)
            {
            }
        }
        protected void btProduto2_Click(object sender, EventArgs e)
        {
            try
            {
                var fMaq = new ECOM_FOTO_MAQUINA();
                fMaq.DATA_FOTO = Convert.ToDateTime(txtData.Text);
                fMaq.DATA_INCLUSAO = DateTime.Now;

                var codigoFotoMaq = eController.InserirFotoMaquina(fMaq);

                var fMaqProd1 = new ECOM_FOTO_MAQUINA_PROD();
                fMaqProd1.ECOM_FOTO_MAQUINA = codigoFotoMaq;
                eController.InserirFotoMaquinaProduto(fMaqProd1);

                var fMaqProd2 = new ECOM_FOTO_MAQUINA_PROD();
                fMaqProd2.ECOM_FOTO_MAQUINA = codigoFotoMaq;
                eController.InserirFotoMaquinaProduto(fMaqProd2);

                CarregarFotoMaquina();
            }
            catch (Exception)
            {
            }
        }
        protected void btProduto3_Click(object sender, EventArgs e)
        {
            try
            {
                var fMaq = new ECOM_FOTO_MAQUINA();
                fMaq.DATA_FOTO = Convert.ToDateTime(txtData.Text);
                fMaq.DATA_INCLUSAO = DateTime.Now;

                var codigoFotoMaq = eController.InserirFotoMaquina(fMaq);

                var fMaqProd1 = new ECOM_FOTO_MAQUINA_PROD();
                fMaqProd1.ECOM_FOTO_MAQUINA = codigoFotoMaq;
                eController.InserirFotoMaquinaProduto(fMaqProd1);

                var fMaqProd2 = new ECOM_FOTO_MAQUINA_PROD();
                fMaqProd2.ECOM_FOTO_MAQUINA = codigoFotoMaq;
                eController.InserirFotoMaquinaProduto(fMaqProd2);

                var fMaqProd3 = new ECOM_FOTO_MAQUINA_PROD();
                fMaqProd3.ECOM_FOTO_MAQUINA = codigoFotoMaq;
                eController.InserirFotoMaquinaProduto(fMaqProd3);

                CarregarFotoMaquina();
            }
            catch (Exception)
            {
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                Button btSal = (Button)sender;

                GridViewRow row = (GridViewRow)btSal.NamingContainer;
                if (row != null)
                {
                    string codigoFotoMaquina = btSal.CommandArgument;

                    string seqIni = "";
                    string seqFim = "";

                    TextBox txtSeqIni = (TextBox)row.FindControl("txtSeqInicial");
                    TextBox txtSeqFim = (TextBox)row.FindControl("txtSeqFinal");

                    seqIni = txtSeqIni.Text.Trim();
                    seqFim = txtSeqFim.Text.Trim();

                    var fMaq = eController.ObterFotoMaquinaPorCodigo(Convert.ToInt32(codigoFotoMaquina));
                    if (fMaq != null)
                    {
                        if (seqIni != "")
                            fMaq.SEQ_INICIAL = Convert.ToInt32(seqIni);
                        if (seqFim != "")
                            fMaq.SEQ_FINAL = Convert.ToInt32(seqFim);

                        eController.AtualizarFotoMaquina(fMaq);

                        btSal.Enabled = false;
                        //CarregarFotoMaquina();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                Button btExc = (Button)sender;
                string codigoFoto = btExc.CommandArgument;
                eController.ExcluirFotoMaquina(Convert.ToInt32(codigoFoto));
                CarregarFotoMaquina();
                txtCodigoBarra.Focus();
            }
            catch (Exception)
            {
            }
        }

        protected void imgProduto_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton img = (ImageButton)sender;

            string dirImagem = img.ImageUrl;

            string url = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "") + dirImagem.Replace("~", "").Replace("\"", "/");
            Response.Redirect(url, "_blank", "");
        }

        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            CarregarCores(txtProduto.Text.Trim());
        }
        private void CarregarCores(string codigoProduto)
        {
            var produtoCor = baseController.BuscaProdutoCores(codigoProduto);
            if (produtoCor != null)
            {
                produtoCor = produtoCor.OrderBy(p => p.DESC_COR_PRODUTO).ToList();
                produtoCor.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "", DESC_COR_PRODUTO = "Selecione" });
                ddlCor.DataSource = produtoCor;
                ddlCor.DataBind();

                if (produtoCor.Count == 2)
                    ddlCor.SelectedValue = produtoCor[1].COR_PRODUTO;
            }
            else
            {
                ddlCor.DataSource = new List<PRODUTO_CORE>();
                ddlCor.DataBind();
            }
        }


    }
}