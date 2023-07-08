using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class desenv_produto_foto : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoProduto = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                codigoProduto = Convert.ToInt32(Request.QueryString["p"].ToString());
                DESENV_PRODUTO _produto = desenvController.ObterProduto(codigoProduto);
                if (_produto == null)
                    Response.Redirect("desenv_menu.aspx");

                hidFoto2.Value = (_produto.FOTO2 == null) ? "" : _produto.FOTO2;
                ddlQtdeFoto_SelectedIndexChanged(null, null);

                CarregarProduto(codigoProduto);
                Session["DROP_IMAGE"] = null;
            }

            if (Request.Files.Keys.Count > 0)
                btCarregarFoto_Click(sender, e);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if (window.File && window.FileList && window.FileReader) { var dropZone = document.getElementById('drop_zone'); dropZone.addEventListener('dragover', handleDragOver, false); dropZone.addEventListener('drop', handleDnDFileSelect, false); } else { alert('Sorry! this browser does not support HTML5 File APIs.'); }", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarProduto(int codigoProduto)
        {
            DESENV_PRODUTO _produto = new DESENV_PRODUTO();
            _produto = desenvController.ObterProduto(codigoProduto);
            if (_produto != null)
            {
                imgFoto.ImageUrl = (_produto.FOTO == null) ? "" : _produto.FOTO;
                imgFoto2.ImageUrl = (_produto.FOTO2 == null) ? "" : _produto.FOTO2;

                hidCodigo.Value = codigoProduto.ToString();

                CarregarPocket(_produto);
            }
        }
        #endregion

        #region "FOTO"
        protected void ddlQtdeFoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdbFoto1.Checked = true;
            if (hidFoto2.Value == "" && ddlQtdeFoto.SelectedValue == "1")
            {
                rdbFoto2.Visible = false;
                imgFoto2.Visible = false;
                imgFoto2.ImageUrl = "";
            }
            else
            {
                rdbFoto2.Visible = true;
                imgFoto2.Visible = true;
                ddlQtdeFoto.SelectedValue = "2";
            }
        }
        protected void btCarregarFoto_Click(object sender, EventArgs e)
        {
            bool dropped = false;

            labErro.Text = "";
            if (Session["DROP_IMAGE"] != null)
            {
                if (rdbFoto1.Checked)
                    imgFoto.ImageUrl = Session["DROP_IMAGE"].ToString();
                if (rdbFoto2.Checked)
                    imgFoto2.ImageUrl = Session["DROP_IMAGE"].ToString();
                Session["DROP_IMAGE"] = null;
            }
            else
            {

                string urlFoto = "";
                Stream streamFoto = null;
                string pathFile = "";
                try
                {
                    if (upFoto.HasFile)
                    {
                        dropped = false;
                        streamFoto = upFoto.PostedFile.InputStream;
                        pathFile = upFoto.PostedFile.FileName;
                        Session["DROP_IMAGE"] = null;
                    }
                    else
                    {
                        HttpFileCollection fileCollection = Request.Files;
                        for (int i = 1; i < fileCollection.Count; i++)
                        {
                            dropped = true;
                            HttpPostedFile upload = fileCollection[i];
                            streamFoto = upload.InputStream;
                            pathFile = fileCollection.Keys[1].ToString();
                        }
                    }

                    if (streamFoto != null)
                    {

                        urlFoto = GravarImagem(streamFoto, pathFile);
                        if (urlFoto.Contains("ERRO"))
                            throw new Exception(urlFoto);
                        if (dropped)
                            Session["DROP_IMAGE"] = urlFoto;

                        if (rdbFoto1.Checked)
                            imgFoto.ImageUrl = urlFoto;
                        if (rdbFoto2.Checked)
                            imgFoto2.ImageUrl = urlFoto;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        private string GravarImagem(Stream _stream, string _pathFile)
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(_pathFile);
                _fileName = Guid.NewGuid() + "_FOTO" + _ext;
                _path = Server.MapPath("~/Image_POCKET/") + _fileName;

                //Obter stream da imagem
                if (_stream != null)
                    GenerateThumbnails(1, _stream, _path);

                retornoImagem = "~/Image_POCKET/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 300
                // height 320
                var newWidth = 0;
                var newHeight = 0;

                int newWidth2Image = 300;
                if (ddlQtdeFoto.SelectedValue == "2")
                    newWidth2Image = 149;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > newWidth2Image || newHeight > 320)
                {
                    newWidth = (int)((newWidth) * 0.95);
                    newHeight = (int)((newHeight) * 0.95);
                }

                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
        #endregion

        #region "ACOES"
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labSalvar.Text = "";
                labExclusao.Text = "";

                if (imgFoto.ImageUrl.Trim() == "")
                {
                    labSalvar.Text = "Selecione a primeira IMAGEM para o Produto.";
                    return;
                }
                if (imgFoto2.ImageUrl.Trim() == "" && ddlQtdeFoto.SelectedValue == "2")
                {
                    labSalvar.Text = "Selecione a segunda IMAGEM para o Produto.";
                    return;
                }

                int codigoProduto = 0;
                codigoProduto = (hidCodigo.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigo.Value);

                if (codigoProduto > 0)
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(codigoProduto);
                    if (_produto != null)
                    {
                        //Lista NOVA
                        List<DESENV_PRODUTO> listaProdutosNova = new List<DESENV_PRODUTO>();
                        //BUSCAR LISTA
                        List<DESENV_PRODUTO> listaProdutosFiltro = new List<DESENV_PRODUTO>();
                        listaProdutosFiltro = desenvController.ObterProduto().Where(p =>
                                                                                        p.COLECAO == _produto.COLECAO &&
                                                                                        p.MODELO != null &&
                                                                                        p.MODELO.Trim().ToUpper() == _produto.MODELO.Trim().ToUpper()
                                                                                        ).ToList();
                        foreach (DESENV_PRODUTO prod in listaProdutosFiltro)
                        {
                            if (prod != null)
                            {
                                prod.FOTO = imgFoto.ImageUrl;
                                if (ddlQtdeFoto.SelectedValue == "2")
                                    prod.FOTO2 = imgFoto2.ImageUrl;
                                listaProdutosNova.Add(prod);
                            }
                        }
                        //listaProdutos.Add(_produto);
                        desenvController.InserirAtualizarProduto(listaProdutosNova);

                        labSalvar.Text = "Foto incluída com sucesso.";

                        //Carregar Pocket
                        _produto.FOTO = imgFoto.ImageUrl;
                        _produto.FOTO2 = imgFoto2.ImageUrl;
                        CarregarPocket(_produto);
                    }
                }
            }
            catch (Exception ex)
            {
                labSalvar.Text = ex.Message;
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labSalvar.Text = "";
                labExclusao.Text = "";

                int codigoProduto = 0;
                codigoProduto = (hidCodigo.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigo.Value);

                if (codigoProduto > 0)
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(codigoProduto);
                    if (_produto != null)
                    {
                        //Lista NOVA
                        List<DESENV_PRODUTO> listaProdutosNova = new List<DESENV_PRODUTO>();
                        //BUSCAR LISTA
                        List<DESENV_PRODUTO> listaProdutosFiltro = new List<DESENV_PRODUTO>();
                        listaProdutosFiltro = desenvController.ObterProduto().Where(p =>
                                                                                        p.COLECAO == _produto.COLECAO &&
                                                                                        p.MODELO != null &&
                                                                                        p.MODELO.Trim().ToUpper() == _produto.MODELO.Trim().ToUpper()
                                                                                        ).ToList();
                        foreach (DESENV_PRODUTO prod in listaProdutosFiltro)
                        {
                            if (prod != null)
                            {
                                Button btExc = (Button)sender;
                                if (btExc != null)
                                {
                                    if (btExc.CommandArgument == "1")
                                    {
                                        prod.FOTO = "";
                                        prod.FOTO2 = "";
                                        imgFoto.ImageUrl = "";
                                        imgFoto2.ImageUrl = "";
                                    }
                                    listaProdutosNova.Add(prod);
                                }
                            }
                        }
                        //listaProdutos.Add(_produto);
                        desenvController.InserirAtualizarProduto(listaProdutosNova);

                        labExclusao.Text = "Foto excluída com sucesso.";

                        //Carregar foto no pocket
                        //_produto.FOTO = imgFoto.ImageUrl;
                        //_produto.FOTO2 = imgFoto.ImageUrl;
                        CarregarPocket(_produto);
                    }
                }
            }
            catch (Exception ex)
            {
                labExclusao.Text = ex.Message;
            }
        }
        #endregion

        #region "POCKET"
        private void CarregarPocket(DESENV_PRODUTO produto)
        {
            HtmlGenericControl divPocket;
            try
            {
                List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();

                listaProdutos.Add(produto);
                if (listaProdutos.Count > 0)
                {
                    //Descarregar hMTL na DIV
                    divPocket = new HtmlGenericControl();
                    divPocket.InnerHtml = Pocket.MontarPocket(listaProdutos, false).ToString();
                    pnlPocket.Controls.Add(divPocket);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
