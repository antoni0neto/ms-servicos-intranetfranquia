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
    public partial class desenv_tripa_view_foto_acessorio : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                var codigoAcessorio = Convert.ToInt32(Request.QueryString["p"].ToString());
                var acessorio = desenvController.ObterAcessorio(codigoAcessorio);
                if (acessorio == null)
                    Response.Redirect("desenv_menu.aspx");

                CarregarAcessorio(acessorio);
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarAcessorio(DESENV_ACESSORIO acessorio)
        {
            acessorio.FOTO1 = ObterFoto(acessorio);
            acessorio.FOTO2 = null;
            CarregarPocket(acessorio);
        }
        #endregion

        #region "POCKET"
        private void CarregarPocket(DESENV_ACESSORIO acessorio)
        {
            HtmlGenericControl divPocket;
            try
            {
                List<DESENV_ACESSORIO> listaAcessorios = new List<DESENV_ACESSORIO>();

                listaAcessorios.Add(acessorio);
                if (listaAcessorios.Count > 0)
                {
                    //Descarregar hMTL na DIV
                    divPocket = new HtmlGenericControl();
                    divPocket.InnerHtml = Pocket.MontarPocketAcessorio(listaAcessorios, false).ToString();
                    pnlPocket.Controls.Add(divPocket);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        private string ObterFoto(DESENV_ACESSORIO acessorio)
        {
            var foto = acessorio.FOTO1;

            string fotoPNG = @"/Fotos/" + acessorio.PRODUTO.Trim() + acessorio.COR.Trim() + ".png";
            string fotoJPG = @"/Fotos/" + acessorio.PRODUTO.Trim() + acessorio.COR.Trim() + ".jpg";

            if (File.Exists(Server.MapPath(fotoPNG)))
                return fotoPNG;
            else if (File.Exists(Server.MapPath(fotoJPG)))
                return fotoJPG;
            return foto;
        }
    }
}
