using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class photo_register_transfer : System.Web.UI.Page
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

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btGerarFoto.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarFoto, null) + ";");
        }

        private void CarregarDataTransferencia()
        {
            var dataTransferencia = eController.ObterFotoMaquinaTransferencia();

            dataTransferencia.Sort((a, b) => a.CompareTo(b));

            var d = new List<ListItem>();
            foreach (var dataT in dataTransferencia)
            {
                d.Add(new ListItem { Value = dataT.ToString(), Text = dataT.ToString("dd/MM/yyyy") });
            }

            ddlDataFoto.DataSource = d;
            ddlDataFoto.DataBind();
        }

        #region "FOTOS DA MAQUINA"
        private List<ECOM_FOTO_MAQUINA> ObterFotoMaquinaPorData()
        {
            return eController.ObterFotoMaquinaPorData(Convert.ToDateTime(ddlDataFoto.SelectedValue));
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
                        gvMaqProduto.DataSource = eController.ObterFotoMaquinaProdutoPorMaquina(fotoMaq.CODIGO);
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
                    ECOM_FOTO_MAQUINA_PROD fotoMaqProd = e.Row.DataItem as ECOM_FOTO_MAQUINA_PROD;

                    if (fotoMaqProd != null)
                    {


                        var histFoto = eController.ObterFotoMaquinaHistorico(fotoMaqProd.PRODUTO, fotoMaqProd.COR);
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
                var fMaq = new ECOM_FOTO_MAQUINA();
                fMaq.DATA_FOTO = Convert.ToDateTime(ddlDataFoto.SelectedValue);
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
                fMaq.DATA_FOTO = Convert.ToDateTime(ddlDataFoto.SelectedValue);
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
            }
            catch (Exception)
            {
            }
        }

        protected void btGerarFoto_Click(object sender, EventArgs e)
        {

            string fotoArquivoDest = "";
            string fotoArquivoDestSmall = "";

            string diretorioFoto = "";
            string diretorioFotoSmall = "";

            try
            {
                labErro.Text = "";


                if (ddlDiretorio.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Diretório....";
                    return;
                }

                string basePathOrigem = Server.MapPath("~/FotosHandbookOnlineGeralMaquina") + "\\";

                string basePathDest = "";
                if (ddlDiretorio.SelectedValue == "1")
                    basePathDest = Server.MapPath("~/FotosHandbookOnlineGeral") + "\\";
                else if (ddlDiretorio.SelectedValue == "2")
                    basePathDest = Server.MapPath("~/FotoLookBookGeral") + "\\";

                DateTime dataFoto = Convert.ToDateTime(ddlDataFoto.SelectedValue);

                string pathOrigem = basePathOrigem + dataFoto.ToString("ddMMyyyy");
                string pathOrigemSmall = basePathOrigem + dataFoto.ToString("ddMMyyyy") + "\\small";

                if (!Directory.Exists(pathOrigem))
                {
                    labErro.Text = "Diretório de Origem não existe para o Dia selecionado...";
                    return;
                }
                if (!Directory.Exists(pathOrigemSmall))
                {
                    labErro.Text = "Diretório de Origem (Small) não existe para o Dia selecionado...";
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

                string[] fotosPathSmall = Directory.GetFiles(pathOrigemSmall);
                if (fotosPathSmall == null || fotosPathSmall.Count() <= 0)
                {
                    labErro.Text = "Diretório Small não possui fotos. (" + pathOrigem + ")";
                    return;
                }
                if (fotosPathSmall.Count() == 1 && fotosPathSmall[0].ToLower().Contains("thumbs.db"))
                {
                    labErro.Text = "Diretório Small não possui fotos. (" + pathOrigem + ")";
                    return;
                }

                var fotoMatSeq = eController.ObterFotoMaquinaSequencia(dataFoto);

                foreach (var fotoSeq in fotoMatSeq)
                {
                    // big
                    diretorioFoto = basePathDest + fotoSeq.DIRETORIO + @"\";
                    if (!Directory.Exists(diretorioFoto))
                        Directory.CreateDirectory(diretorioFoto);
                    // small
                    diretorioFotoSmall = basePathDest + fotoSeq.DIRETORIO + @"\small\";
                    if (!Directory.Exists(diretorioFotoSmall))
                        Directory.CreateDirectory(diretorioFotoSmall);

                    var seqFoto4 = "0000" + fotoSeq.SEQ_FOTO.ToString();
                    seqFoto4 = seqFoto4.Substring(seqFoto4.Length - 4);
                    var fotoArquivo = Directory.GetFiles(pathOrigem, ("*" + seqFoto4 + ".jpg"), SearchOption.TopDirectoryOnly).FirstOrDefault();
                    var fotoArquivoSmall = Directory.GetFiles(pathOrigemSmall, ("*" + seqFoto4 + ".jpg"), SearchOption.TopDirectoryOnly).FirstOrDefault();

                    if ((fotoArquivo != null && File.Exists(fotoArquivo)) && (fotoArquivoSmall != null && File.Exists(fotoArquivoSmall)))
                    {

                        fotoArquivoDest = diretorioFoto + Path.GetFileName(fotoArquivo);
                        fotoArquivoDestSmall = diretorioFotoSmall + Path.GetFileName(fotoArquivoSmall);

                        if (!File.Exists(fotoArquivoDest))
                            File.Copy(fotoArquivo, fotoArquivoDest, true);
                        if (!File.Exists(fotoArquivoDestSmall))
                            File.Copy(fotoArquivoSmall, fotoArquivoDestSmall, true);

                        var fotoHist = new ECOM_FOTO_MAQUINA_HIST();
                        fotoHist.PRODUTO = fotoSeq.PRODUTO;
                        fotoHist.COR = fotoSeq.COR;
                        fotoHist.DIRETORIO = fotoSeq.DIRETORIO;
                        fotoHist.FOTO_NOME = Path.GetFileName(fotoArquivo);
                        fotoHist.DATA_TRANSFER = DateTime.Now;
                        eController.InserirFotoMaquinaHist(fotoHist);

                    }

                }

                labErro.Text = "Fotos Tranferidas com Sucesso...";

            }
            catch (Exception ex)
            {
                labErro.Text = "Erro ao copiar Foto... " + ex.Message;

                fotoArquivoDest = "";
                fotoArquivoDestSmall = "";
                diretorioFoto = "";
                diretorioFotoSmall = "";

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