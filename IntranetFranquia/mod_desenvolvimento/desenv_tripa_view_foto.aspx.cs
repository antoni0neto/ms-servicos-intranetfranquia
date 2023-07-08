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
    public partial class desenv_tripa_view_foto : System.Web.UI.Page
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
                var produto = desenvController.ObterProduto(codigoProduto);
                if (produto == null)
                    Response.Redirect("desenv_menu.aspx");

                CarregarProduto(produto);
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarProduto(DESENV_PRODUTO produto)
        {
            produto.FOTO = ObterFoto(produto);
            produto.FOTO2 = null;
            CarregarPocket(produto);
        }
        #endregion

        #region "POCKET"
        private void CarregarPocket(DESENV_PRODUTO produto)
        {
            HtmlGenericControl divPocket;
            try
            {
                var listaProdutos = new List<DESENV_PRODUTO>();

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

        private string ObterFoto(DESENV_PRODUTO produto)
        {
            var foto = produto.FOTO;

            string fotoPNG = @"/Fotos/" + produto.MODELO.Trim() + produto.COR.Trim() + ".png";
            string fotoJPG = @"/Fotos/" + produto.MODELO.Trim() + produto.COR.Trim() + ".jpg";

            if (File.Exists(Server.MapPath(fotoPNG)))
                return fotoPNG;
            else if (File.Exists(Server.MapPath(fotoJPG)))
                return fotoJPG;
            return foto;
        }
    }
}
