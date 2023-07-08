using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Relatorios
{
    public partial class rh_cad_funcionario_foto : System.Web.UI.Page
    {
        RHController rhController = new RHController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                if (Request.QueryString["fot"] == null || Request.QueryString["fot"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("Login.aspx");

                var codigoFuncionario = Request.QueryString["fot"].ToString();

                hidCodigoFuncionario.Value = codigoFuncionario;
            }

        }

        protected void btIncluirFoto_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (uploadFoto.PostedFile == null)
                {
                    labErro.Text = "Por favor, selecione a foto...";
                    return;
                }


                var func = rhController.ObterFuncionario(Convert.ToInt32(hidCodigoFuncionario.Value));
                if (func != null)
                {
                    imgFoto.ImageUrl = GravarImagem(func);

                    func.FOTO = imgFoto.ImageUrl;
                    rhController.AtualizarFuncionario(func);
                    labErro.Text = "Foto incluída com sucesso.";
                }
                else
                {
                    labErro.Text = "O funcionário não foi encontrado.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: (btIncluirFoto_Click) \n\n" + ex.Message;
            }
        }
        private string GravarImagem(RH_FUNCIONARIO func)
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            Stream _stream = null;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(uploadFoto.PostedFile.FileName);
                _fileName = "FF_" + func.VENDEDOR.Trim() + "_" + Guid.NewGuid().ToString().Substring(1, 8) + "" + _ext;
                _path = Server.MapPath("~/Image_RH/") + _fileName;

                //Obter stream da imagem
                _stream = uploadFoto.PostedFile.InputStream;
                if (_stream != null)
                {
                    SalvarImagem(_stream, _path);
                }

                retornoImagem = "~/Image_RH/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void SalvarImagem(Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 220
                // height 130
                var newWidth = 0;
                var newHeight = 0;

                newWidth = image.Width;
                newHeight = image.Height;

                while (newWidth > 250 || newHeight > 250)
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


    }
}
