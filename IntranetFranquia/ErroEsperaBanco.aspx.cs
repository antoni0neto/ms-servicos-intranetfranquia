using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class ErroEsperaBanco : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["erro"] != null)
                LiteralErro.Text = Request.QueryString["erro"].ToString();
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario.CODIGO_PERFIL == 1)
                    Response.Redirect("~/DefaultGeral.aspx");
                else if (usuario.CODIGO_PERFIL == 2)
                    Response.Redirect("~/DefaultLoja.aspx");
                else if (usuario.CODIGO_PERFIL == 3)
                    Response.Redirect("~/DefaultSuperLoja.aspx");
                else if (usuario.CODIGO_PERFIL == 4)
                    Response.Redirect("~/DefaultAtacado.aspx");
                else if (usuario.CODIGO_PERFIL == 5)
                    Response.Redirect("~/DefaultAdministrativo.aspx");
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}
