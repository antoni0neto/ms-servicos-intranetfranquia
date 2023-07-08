using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class zold_admloj_cad_vitrine_foto : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "admloj_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_marketing/mkt_menu.aspx";

                CarregarFilial();
                CarregarAnoSemana();
                //CarregarVitrineSetor();

            }
        }

        private void CarregarVitrineFotos(int anoSemana)
        {
            //var vitrineFoto = lojaController.ObterVitrineFoto(ddlFilial.SelectedValue, anoSemana);

            //gvVitrineFoto.DataSource = vitrineFoto;
            //gvVitrineFoto.DataBind();
        }
        protected void gvVitrineFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.DataItem != null)
            //    {
            //        MKT_VITRINE_FOTO vitrine = e.Row.DataItem as MKT_VITRINE_FOTO;

            //        if (vitrine != null)
            //        {

            //            System.Web.UI.WebControls.Image _imgFotoVitrine = e.Row.FindControl("imgFotoVitrine") as System.Web.UI.WebControls.Image;
            //            _imgFotoVitrine.ImageUrl = vitrine.FOTO;

            //            Button _btExcluirFoto = e.Row.FindControl("btExcluirFoto") as Button;
            //            _btExcluirFoto.CommandArgument = vitrine.CODIGO.ToString();

            //        }
            //    }
            //}
        }

        protected void btIncluirFotoVitrine_Click(object sender, EventArgs e)
        {
            try
            {
                imgFotoVitrine.ImageUrl = GravarImagem();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: (btIncluirFotoVitrine_Click) \n\n" + ex.Message;
            }
        }
        private string GravarImagem()
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
                    SalvarImagem(_stream, _path, false);

                    string pathZoom = _path.Replace("Image_VITRINE", "Image_VITRINE/Zoom");
                    SalvarImagem(_stream, pathZoom, true);
                }

                retornoImagem = "~/Image_VITRINE/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void SalvarImagem(Stream sourcePath, string targetPath, bool zoom)
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
                    while (newWidth > 800 || newHeight > 800)
                    {
                        newWidth = (int)((newWidth) * 0.95);
                        newHeight = (int)((newHeight) * 0.95);
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

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                string codigoFilial = "";
                int anoSemana = 0;


                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em vermelho.";
                    return;
                }

                //codigoFilial = ddlFilial.SelectedValue;
                //anoSemana = Convert.ToInt32(ddlAnoSemana.SelectedValue);

                //var l = lojaController.ObterVitrineFotoQtdeSemana(codigoFilial, anoSemana);
                //var lMax = lojaController.ObterVitrineFotoMax(codigoFilial);

                //if (l >= lMax)
                //{
                //    labErro.Text = "O número Máximo de Fotos para esta semana foi atingido...";
                //    return;
                //}

                //var dias = baseController.ObterAnoSemana(anoSemana).FirstOrDefault();

                //var vitrineFoto = new MKT_VITRINE_FOTO();
                //vitrineFoto.CODIGO_FILIAL = codigoFilial;
                //vitrineFoto.FILIAL = ddlFilial.SelectedItem.Text.Trim();
                //vitrineFoto.ANOSEMANA = anoSemana;
                //vitrineFoto.DIA_INICIAL = Convert.ToDateTime(dias.DIA_INICIAL);
                //vitrineFoto.DIA_FINAL = Convert.ToDateTime(dias.DIA_FINAL);
                //vitrineFoto.DESCRICAO = ddlSetorTipo.SelectedItem.Text;
                //vitrineFoto.FOTO = imgFotoVitrine.ImageUrl;
                //vitrineFoto.MKT_VITRINE_SETOR = Convert.ToInt32(ddlSetor.SelectedValue);

                //lojaController.InserirVitrineFoto(vitrineFoto);

                //CarregarVitrineFotos(anoSemana);

                //imgFotoVitrine.ImageUrl = "";
                //ddlSetorTipo.SelectedValue = "0";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void ddlAnoSemana_TextChanged(object sender, EventArgs e)
        {
            //if (ddlAnoSemana.SelectedValue != "")
            //{
            //    CarregarVitrineFotos(Convert.ToInt32(ddlAnoSemana.SelectedValue));

            //    var setorSemana = lojaController.ObterVitrineSetorData(Convert.ToInt32(ddlAnoSemana.SelectedValue));

            //    ddlSetor.SelectedValue = setorSemana.MKT_VITRINE_SETOR.ToString();
            //    ddlSetor_SelectedIndexChanged(ddlSetor, null);

            //}
            //else
            //{
            //    ddlSetor.SelectedValue = "0";

            //    gvVitrineFoto.DataSource = new List<MKT_VITRINE_FOTO>();
            //    gvVitrineFoto.DataBind();
            //}
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            var usuario = (USUARIO)Session["USUARIO"];

            if (usuario.CODIGO_PERFIL == 3 || usuario.CODIGO_PERFIL == 1)
                filial = baseController.BuscaFiliais();
            else
                filial = baseController.BuscaFiliais(usuario);

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }
        private void CarregarAnoSemana()
        {
            var anoSemana = baseController.ObterAnoSemana(null);

            anoSemana.Insert(0, new SP_OBTER_ANOSEMANAResult { ANOSEMANA = "", DESC_ANOSEMANA = "Selecione" });
            ddlAnoSemana.DataSource = anoSemana;
            ddlAnoSemana.DataBind();
        }

        private bool ValidarCampos()
        {
            bool retorno = true;
            System.Drawing.Color _OK = Color.Gray;
            System.Drawing.Color _NOK = Color.Red;

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _NOK;
                retorno = false;
            }

            labAnoSemana.ForeColor = _OK;
            if (ddlAnoSemana.SelectedValue == "")
            {
                labAnoSemana.ForeColor = _NOK;
                retorno = false;
            }

            labSetor.ForeColor = _OK;
            if (ddlSetor.SelectedValue == "0")
            {
                labSetor.ForeColor = _NOK;
                retorno = false;
            }

            labVitrine.ForeColor = _OK;
            if (ddlSetorTipo.SelectedValue == "0" || ddlSetorTipo.SelectedValue == "" || ddlSetorTipo.SelectedValue == null)
            {
                labVitrine.ForeColor = _NOK;
                retorno = false;
            }

            labFotoVitrineTitulo.ForeColor = _OK;
            if (imgFotoVitrine.ImageUrl == "")
            {
                labFotoVitrineTitulo.ForeColor = _NOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btExcluirFoto_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    labErro.Text = "";

            //    Button bt = (Button)sender;

            //    int codigoVitrineFoto = 0;

            //    codigoVitrineFoto = Convert.ToInt32(bt.CommandArgument);

            //    lojaController.ExcluirVitrineFoto(codigoVitrineFoto);

            //    if (ddlAnoSemana.SelectedValue != "")
            //        CarregarVitrineFotos(Convert.ToInt32(ddlAnoSemana.SelectedValue));

            //}
            //catch (Exception ex)
            //{
            //    labErro.Text = ex.Message;
            //}
        }

        protected void ddlSetor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // CarregarVitrineSetorTipo(Convert.ToInt32(ddlSetor.SelectedValue));
        }
    }
}
