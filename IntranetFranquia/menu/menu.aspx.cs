using DAL;
using System;

namespace Relatorios
{
    public partial class menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["USUARIO"] != null)
            {
                var usuario = (USUARIO)Session["USUARIO"];

                if (usuario.CODIGO_PERFIL != 1)
                    Response.Redirect("/Login.aspx");
            }
            else
            {
                Response.Redirect("/Login.aspx");
            }
        }
    }
}
