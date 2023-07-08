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
    public partial class ecom_cad_foto_lookbook_transfer_old : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }

            btGerarFoto.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarFoto, null) + ";");
        }

        #region "FOTOS DA MAQUINA"
        private List<ECOM_FOTO_LOOKBOOK> ObterFotoLookbook()
        {
            return eController.ObterFotoLookbook();
        }
        private void CarregarFotoLookbook(List<ECOM_FOTO_LOOKBOOK> fotos)
        {
            gvFotoLookbook.DataSource = fotos;
            gvFotoLookbook.DataBind();
        }
        protected void gvFotoLookbook_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_FOTO_LOOKBOOK foto = e.Row.DataItem as ECOM_FOTO_LOOKBOOK;

                    if (foto != null)
                    {
                        System.Web.UI.WebControls.Image imgFoto = e.Row.FindControl("imgFoto") as System.Web.UI.WebControls.Image;
                        imgFoto.ImageUrl = "~/FotoLookBookApp/" + foto.NOME_FOTO.Trim() + ".jpg";
                    }
                }
            }
        }

        #endregion

        protected void btGerarFoto_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";

                var fotosLookbook = new List<ECOM_FOTO_LOOKBOOK>();

                string basePathOrigem = Server.MapPath("~/FotosHandbookTratamento") + "\\";
                string basePathDest1 = Server.MapPath("~/FotoLookBook") + "\\";
                string basePathDest2 = Server.MapPath("~/FotoLookBookApp") + "\\";

                var foto = ObterFotoLookbook();

                foreach (var f in foto)
                {

                    var codigoProduto = f.NOME_FOTO.Substring(0, 5);
                    var cor = f.NOME_FOTO.Substring(5, (f.NOME_FOTO.Length - 6));

                    var produto = baseController.BuscaProduto(codigoProduto);
                    if (produto != null)
                    {
                        var nomeDiretorio = produto.GRUPO_PRODUTO.Trim() + "-" + codigoProduto + cor;
                        var diretorioCompleto = basePathOrigem + nomeDiretorio;

                        if (Directory.Exists(diretorioCompleto))
                        {
                            string[] fotosPath = Directory.GetFiles(diretorioCompleto);
                            if ((fotosPath == null || fotosPath.Count() <= 0) || (fotosPath.Count() == 1 && fotosPath[0].ToLower().Contains("thumbs.db")))
                                continue;

                            var fotoArquivo = Directory.GetFiles(diretorioCompleto, (f.NOME_FOTO + ".jpg"), SearchOption.TopDirectoryOnly).FirstOrDefault();
                            if (fotoArquivo != null && File.Exists(fotoArquivo))
                            {
                                var fotoArquivoDest1 = basePathDest1 + Path.GetFileName(fotoArquivo);
                                if (!File.Exists(fotoArquivoDest1))
                                    File.Copy(fotoArquivo, fotoArquivoDest1, true);

                                var fotoArquivoDest2 = basePathDest2 + Path.GetFileName(fotoArquivo);
                                if (!File.Exists(fotoArquivoDest2))
                                    File.Copy(fotoArquivo, fotoArquivoDest2, true);

                                var fotoLookbook = eController.ObterFotoLookbook(f.NOME_FOTO);
                                if (fotoLookbook != null)
                                {
                                    fotoLookbook.DATA_TRANSF = DateTime.Now;
                                    eController.AtualizarFotoLookbook(fotoLookbook);

                                    fotosLookbook.Add(new ECOM_FOTO_LOOKBOOK { NOME_FOTO = fotoLookbook.NOME_FOTO });
                                }

                            }

                        }

                    }

                }

                CarregarFotoLookbook(fotosLookbook);

                labErro.Text = "Fotos Tranferidas com Sucesso...";

            }
            catch (Exception ex)
            {
                labErro.Text = "Erro ao copiar Foto... " + ex.Message;
            }
        }

    }
}