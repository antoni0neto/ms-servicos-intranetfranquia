using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Relatorios
{
    public partial class admloj_cad_vitrine_fotomundoadd : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                if (Request.QueryString["fot"] == null || Request.QueryString["fot"] == "" ||
                    Request.QueryString["o"] == null || Request.QueryString["o"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("Login.aspx");

                var codigoVitrineMundo = Request.QueryString["fot"].ToString();
                var ordem = Request.QueryString["o"].ToString();

                hidCodigoVitrineMundo.Value = codigoVitrineMundo;
                hidOrdem.Value = ordem;

            }

        }

        protected void btIncluirFotoVitrine_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (uploadFotoVitrine.PostedFile == null)
                {
                    labErro.Text = "Por favor, selecione a foto...";
                    return;
                }


                bool principal = (hidOrdem.Value == "0") ? true : false;

                imgFotoVitrine.ImageUrl = GravarImagem(principal);

                var mundoFoto = lojaController.ObterVitrineMundoFotoPorMundoEOrdem(Convert.ToInt32(hidCodigoVitrineMundo.Value), Convert.ToInt32(hidOrdem.Value));
                if (mundoFoto == null)
                {
                    mundoFoto = new LOJA_VITRINE_MUNDO_FOTO();
                    mundoFoto.LOJA_VITRINE_MUNDO = Convert.ToInt32(hidCodigoVitrineMundo.Value);
                    mundoFoto.FOTO = imgFotoVitrine.ImageUrl;
                    mundoFoto.PRINCIPAL = (hidOrdem.Value == "0") ? true : false;
                    mundoFoto.FAVORITO = 'N';
                    mundoFoto.HARMONIA = 'N';
                    mundoFoto.POSICAO = 'N';
                    mundoFoto.MONTAGEM = 'N';
                    mundoFoto.CUBOS = 'N';
                    mundoFoto.ACESSORIOS = 'N';
                    mundoFoto.ORDEM = Convert.ToInt32(hidOrdem.Value);

                    lojaController.InserirVitrineMundoFoto(mundoFoto);

                }
                else
                {
                    mundoFoto.FOTO = imgFotoVitrine.ImageUrl;
                    lojaController.AtualizarVitrineMundoFoto(mundoFoto);
                }

                labErro.Text = "Foto incluída com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: (btIncluirFotoVitrine_Click) \n\n" + ex.Message;
            }
        }
        private string GravarImagem(bool fotoPrincipal)
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            Stream _stream = null;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(uploadFotoVitrine.PostedFile.FileName);
                _fileName = Guid.NewGuid() + "_VITRINE" + _ext;
                _path = Server.MapPath("~/Image_VITRINE/") + _fileName;

                //Obter stream da imagem
                _stream = uploadFotoVitrine.PostedFile.InputStream;
                if (_stream != null)
                {
                    SalvarImagem(_stream, _path, fotoPrincipal, false);

                    string pathZoom = _path.Replace("Image_VITRINE", "Image_VITRINE/Zoom");
                    SalvarImagem(_stream, pathZoom, fotoPrincipal, true);
                }

                retornoImagem = "~/Image_VITRINE/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void SalvarImagem(Stream sourcePath, string targetPath, bool fotoPrincipal, bool zoom)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 220
                // height 130
                var newWidth = 0;
                var newHeight = 0;

                newWidth = image.Width;
                newHeight = image.Height;

                if (!zoom)
                {
                    if (!fotoPrincipal)
                    {
                        while (newWidth > 350 || newHeight > 450)
                        {
                            newWidth = (int)((newWidth) * 0.95);
                            newHeight = (int)((newHeight) * 0.95);
                        }
                    }
                    else
                    {
                        while (newWidth > 800 || newHeight > 450)
                        {
                            newWidth = (int)((newWidth) * 0.95);
                            newHeight = (int)((newHeight) * 0.95);
                        }
                    }
                }
                else
                {
                    while (newWidth < 1500 || newHeight < 1500)
                    {
                        newWidth = (int)((newWidth) * 1.10);
                        newHeight = (int)((newHeight) * 1.10);
                    }
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


    }
}
