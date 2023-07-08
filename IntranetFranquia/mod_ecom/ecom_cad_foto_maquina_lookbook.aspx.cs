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
    public partial class ecom_cad_foto_maquina_lookbook : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        int sequencia = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtCodigoBarra.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "', event)");
            txtCodigoBarra.Focus();

            if (!Page.IsPostBack)
            {
                txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");

                CarregarFotoMaquina();
            }
        }

        #region "FOTOS DA MAQUINA"
        private List<ECOM_FOTO_MAQUINA_LOOKBOOK> ObterFotoMaquinaPorData()
        {
            return eController.ObterFotoMaquinaLookBookPorData(DateTime.Today);
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
                    ECOM_FOTO_MAQUINA_LOOKBOOK fotoMaq = e.Row.DataItem as ECOM_FOTO_MAQUINA_LOOKBOOK;

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
                        gvMaqProduto.DataSource = eController.ObterFotoMaquinaProdutoLookBookPorMaquina(fotoMaq.CODIGO);
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
                    ECOM_FOTO_MAQUINA_PROD_LOOKBOOK fotoMaqProd = e.Row.DataItem as ECOM_FOTO_MAQUINA_PROD_LOOKBOOK;

                    if (fotoMaqProd != null)
                    {
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

                if (ObterUsuario() == null)
                {
                    Response.Redirect("../Login.aspx");
                    return;
                }

                codigoBarraProduto = txtCodigoBarra.Text.Trim();
                descOutro = txtDescOutro.Text.Trim();

                if (codigoBarraProduto == "" && descOutro == "")
                {
                    labErro.Text = "Informe o Código de Barras do Produto ou descreva o Produto sem Etiqueta...";
                    return;
                }

                var produtoBarra = baseController.BuscaProdutoBarra(codigoBarraProduto);
                if (produtoBarra != null)
                {
                    var produto = produtoBarra.PRODUTO.Trim();
                    var descProduto = baseController.BuscaProduto(produto).DESC_PRODUTO.Trim();
                    var cor = produtoBarra.COR_PRODUTO.Trim();
                    var descCor = prodController.ObterCoresBasicas(cor).DESC_COR.Trim();

                    var fMaqProduto = eController.ObterFotoMaquinaProdutoLookBookUltimoAberto();
                    if (fMaqProduto != null)
                    {
                        fMaqProduto.PRODUTO = produto;
                        fMaqProduto.DESC_PRODUTO = descProduto;
                        fMaqProduto.COR = cor;
                        fMaqProduto.DESC_COR = descCor;
                        fMaqProduto.DESC_OUTRO = "";

                        eController.AtualizarFotoMaquinaProdutoLookBook(fMaqProduto);

                        CarregarFotoMaquina();
                    }
                    else
                    {
                        labErro.Text = "Nenhum Produto em Aberto. Favor abrir os produtos, clicando em \"1 Peça\" ou \"2 Peças\" ou \"3 peças\"...";
                        return;
                    }
                }
                else
                {
                    if (descOutro != "")
                    {
                        var fMaqProduto = eController.ObterFotoMaquinaProdutoLookBookUltimoAberto();
                        if (fMaqProduto != null)
                        {
                            fMaqProduto.PRODUTO = "";
                            fMaqProduto.DESC_PRODUTO = "";
                            fMaqProduto.COR = "";
                            fMaqProduto.DESC_COR = "";
                            fMaqProduto.DESC_OUTRO = descOutro;

                            eController.AtualizarFotoMaquinaProdutoLookBook(fMaqProduto);

                            CarregarFotoMaquina();
                        }
                        else
                        {
                            labErro.Text = "Nenhum Produto em Aberto. Favor abrir os produtos, clicando em \"1 Peça\" ou \"2 Peças\" ou \"3 peças\"...";
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
                var fMaq = new ECOM_FOTO_MAQUINA_LOOKBOOK();
                fMaq.DATA_FOTO = Convert.ToDateTime(txtData.Text);
                fMaq.DATA_INCLUSAO = DateTime.Now;

                var codigoFotoMaq = eController.InserirFotoMaquinaLookBook(fMaq);

                var fMaqProd1 = new ECOM_FOTO_MAQUINA_PROD_LOOKBOOK();
                fMaqProd1.ECOM_FOTO_MAQUINA_LOOKBOOK = codigoFotoMaq;
                eController.InserirFotoMaquinaProdutoLookBook(fMaqProd1);

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
                var fMaq = new ECOM_FOTO_MAQUINA_LOOKBOOK();
                fMaq.DATA_FOTO = Convert.ToDateTime(txtData.Text);
                fMaq.DATA_INCLUSAO = DateTime.Now;

                var codigoFotoMaq = eController.InserirFotoMaquinaLookBook(fMaq);

                var fMaqProd1 = new ECOM_FOTO_MAQUINA_PROD_LOOKBOOK();
                fMaqProd1.ECOM_FOTO_MAQUINA_LOOKBOOK = codigoFotoMaq;
                eController.InserirFotoMaquinaProdutoLookBook(fMaqProd1);

                var fMaqProd2 = new ECOM_FOTO_MAQUINA_PROD_LOOKBOOK();
                fMaqProd2.ECOM_FOTO_MAQUINA_LOOKBOOK = codigoFotoMaq;
                eController.InserirFotoMaquinaProdutoLookBook(fMaqProd2);

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
                var fMaq = new ECOM_FOTO_MAQUINA_LOOKBOOK();
                fMaq.DATA_FOTO = Convert.ToDateTime(txtData.Text);
                fMaq.DATA_INCLUSAO = DateTime.Now;

                var codigoFotoMaq = eController.InserirFotoMaquinaLookBook(fMaq);

                var fMaqProd1 = new ECOM_FOTO_MAQUINA_PROD_LOOKBOOK();
                fMaqProd1.ECOM_FOTO_MAQUINA_LOOKBOOK = codigoFotoMaq;
                eController.InserirFotoMaquinaProdutoLookBook(fMaqProd1);

                var fMaqProd2 = new ECOM_FOTO_MAQUINA_PROD_LOOKBOOK();
                fMaqProd2.ECOM_FOTO_MAQUINA_LOOKBOOK = codigoFotoMaq;
                eController.InserirFotoMaquinaProdutoLookBook(fMaqProd2);

                var fMaqProd3 = new ECOM_FOTO_MAQUINA_PROD_LOOKBOOK();
                fMaqProd3.ECOM_FOTO_MAQUINA_LOOKBOOK = codigoFotoMaq;
                eController.InserirFotoMaquinaProdutoLookBook(fMaqProd3);

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

                    var fMaq = eController.ObterFotoMaquinaLookBookPorCodigo(Convert.ToInt32(codigoFotoMaquina));
                    if (fMaq != null)
                    {
                        if (seqIni != "")
                            fMaq.SEQ_INICIAL = Convert.ToInt32(seqIni);
                        if (seqFim != "")
                            fMaq.SEQ_FINAL = Convert.ToInt32(seqFim);

                        eController.AtualizarFotoMaquinaLookBook(fMaq);

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
                eController.ExcluirFotoMaquinaLookBook(Convert.ToInt32(codigoFoto));
                CarregarFotoMaquina();
                txtCodigoBarra.Focus();
            }
            catch (Exception)
            {
            }
        }


    }
}