using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_ajuda_video : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";
                CarregarVideos();

            }

        }

        #region "DADOS INICIAIS"
        private void CarregarVideos()
        {
            var videos = ObterAjudaVideo();

            videos.Insert(0, new ECOM_AJUDA_VIDEO { CODIGO = 0, TITULO = "Selecione", ATIVO = true });
            ddlVideos.DataSource = videos;
            ddlVideos.DataBind();
        }
        #endregion


        private List<ECOM_AJUDA_VIDEO> ObterAjudaVideo()
        {
            return ecomController.ObterAjudaVideo().Where(p => p.ATIVO).ToList();
        }

        protected void ddlVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVideos.SelectedValue != "0")
            {
                var video = ecomController.ObterAjudaVideoPorCodigo(Convert.ToInt32(ddlVideos.SelectedValue));

                frmVideo.Attributes.Add("src", video.LINK);

                txtDescricao.Text = video.DESCRICAO;

            }
            else
            {
                frmVideo.Attributes.Remove("src");
                txtDescricao.Text = "";
            }
        }
    }
}
