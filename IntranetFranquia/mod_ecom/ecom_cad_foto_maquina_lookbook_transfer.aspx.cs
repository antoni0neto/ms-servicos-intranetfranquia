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
    public partial class ecom_cad_foto_maquina_lookbook_transfer : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarDataTransferencia();
            }

            btGerarFoto.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarFoto, null) + ";");
        }

        private void CarregarDataTransferencia()
        {
            var dataTransferencia = eController.ObterFotoMaquinaLookBookTransferencia();

            var d = new List<ListItem>();
            foreach (var dataT in dataTransferencia)
            {
                d.Add(new ListItem { Value = dataT.ToString(), Text = dataT.ToString("dd/MM/yyyy") });
            }

            ddlDataFoto.DataSource = d;
            ddlDataFoto.DataBind();
        }

        #region "FOTOS DA MAQUINA"
        private List<ECOM_FOTO_MAQUINA_LOOKBOOK> ObterFotoMaquinaPorData()
        {
            return eController.ObterFotoMaquinaLookBookPorData(Convert.ToDateTime(ddlDataFoto.SelectedValue));
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


                        var histFoto = eController.ObterFotoMaquinaLookBookHistorico(fotoMaqProd.PRODUTO, fotoMaqProd.COR);
                        if (histFoto != null)
                            e.Row.BackColor = Color.Lavender;

                    }
                }
            }
        }

        //protected void btGravar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        labErro.Text = "";

        //        string codigoBarraProduto = "";
        //        string descOutro = "";

        //        if (ObterUsuario() == null)
        //        {
        //            Response.Redirect("../Login.aspx");
        //            return;
        //        }


        //        if (codigoBarraProduto == "" && descOutro == "")
        //        {
        //            labErro.Text = "Informe o Código de Barras do Produto ou descreva o Produto sem Etiqueta...";
        //            return;
        //        }

        //        var produtoBarra = baseController.BuscaProdutoBarra(codigoBarraProduto);
        //        if (produtoBarra != null)
        //        {
        //            var produto = produtoBarra.PRODUTO.Trim();
        //            var descProduto = baseController.BuscaProduto(produto).DESC_PRODUTO.Trim();
        //            var cor = produtoBarra.COR_PRODUTO.Trim();
        //            var descCor = prodController.ObterCoresBasicas(cor).DESC_COR.Trim();

        //            var fMaqProduto = eController.ObterFotoMaquinaProdutoUltimoAberto();
        //            if (fMaqProduto != null)
        //            {
        //                fMaqProduto.PRODUTO = produto;
        //                fMaqProduto.DESC_PRODUTO = descProduto;
        //                fMaqProduto.COR = cor;
        //                fMaqProduto.DESC_COR = descCor;
        //                fMaqProduto.DESC_OUTRO = "";

        //                eController.AtualizarFotoMaquinaProduto(fMaqProduto);

        //                CarregarFotoMaquina();
        //            }
        //            else
        //            {
        //                labErro.Text = "Nenhum Produto em Aberto. Favor abrir os produtos, clicando em \"1 Peça\" ou \"2 Peças\"...";
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            if (descOutro != "")
        //            {
        //                var fMaqProduto = eController.ObterFotoMaquinaProdutoUltimoAberto();
        //                if (fMaqProduto != null)
        //                {
        //                    fMaqProduto.PRODUTO = "";
        //                    fMaqProduto.DESC_PRODUTO = "";
        //                    fMaqProduto.COR = "";
        //                    fMaqProduto.DESC_COR = "";
        //                    fMaqProduto.DESC_OUTRO = descOutro;

        //                    eController.AtualizarFotoMaquinaProduto(fMaqProduto);

        //                    CarregarFotoMaquina();
        //                }
        //                else
        //                {
        //                    labErro.Text = "Nenhum Produto em Aberto. Favor abrir os produtos, clicando em \"1 Peça\" ou \"2 Peças\"";
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                labErro.Text = "Produto não cadastrado no Linx...";
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
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
                fMaq.DATA_FOTO = Convert.ToDateTime(ddlDataFoto.SelectedValue);
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
                fMaq.DATA_FOTO = Convert.ToDateTime(ddlDataFoto.SelectedValue);
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
            }
            catch (Exception)
            {
            }
        }

        protected void btGerarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                string basePathOrigem = Server.MapPath("~/FotoLookBookMaquina") + "\\";
                string basePathDest = Server.MapPath("~/FotoLookBook") + "\\";

                DateTime dataFoto = Convert.ToDateTime(ddlDataFoto.SelectedValue);

                string pathOrigem = "";
                pathOrigem = basePathOrigem + dataFoto.ToString("ddMMyyyy");

                if (!Directory.Exists(pathOrigem))
                {
                    labErro.Text = "Diretório de Origem não existe para o Dia selecionado...";
                    return;
                }

                string[] fotosPath = Directory.GetFiles(pathOrigem);

                if (fotosPath == null || fotosPath.Count() <= 0)
                {
                    labErro.Text = "Diretório não possui fotos. (" + pathOrigem + ")";
                    return;
                }

                if (fotosPath.Count() == 1 && fotosPath[0].ToLower().Contains("thumbs.db"))
                {
                    labErro.Text = "Diretório não possui fotos. (" + pathOrigem + ")";
                    return;
                }

                var fotoMatSeq = eController.ObterFotoMaquinaLookBookSequencia(dataFoto);

                foreach (var fotoSeq in fotoMatSeq)
                {
                    var diretorioFoto = basePathDest + fotoSeq.DIRETORIO + @"\";
                    if (!Directory.Exists(diretorioFoto))
                        Directory.CreateDirectory(diretorioFoto);

                    var seqFoto4 = "0000" + fotoSeq.SEQ_FOTO.ToString();
                    seqFoto4 = seqFoto4.Substring(seqFoto4.Length - 4);
                    var fotoArquivo = Directory.GetFiles(pathOrigem, ("*" + seqFoto4 + ".jpg"), SearchOption.TopDirectoryOnly).FirstOrDefault();

                    if (fotoArquivo != null && File.Exists(fotoArquivo))
                    {
                        File.Copy(fotoArquivo, diretorioFoto + Path.GetFileName(fotoArquivo), true);

                        var fotoHist = new ECOM_FOTO_MAQUINA_LOOKBOOK_HIST();
                        fotoHist.PRODUTO = fotoSeq.PRODUTO;
                        fotoHist.COR = fotoSeq.COR;
                        fotoHist.DIRETORIO = fotoSeq.DIRETORIO;
                        fotoHist.FOTO_NOME = Path.GetFileName(fotoArquivo);
                        fotoHist.DATA_TRANSFER = DateTime.Now;
                        eController.InserirFotoMaquinaLookBookHist(fotoHist);

                    }

                }

                labErro.Text = "Fotos Tranferidas com Sucesso...";

            }
            catch (Exception ex)
            {
                labErro.Text = "Erro ao copiar Foto... " + ex.Message;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarFotoMaquina();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}