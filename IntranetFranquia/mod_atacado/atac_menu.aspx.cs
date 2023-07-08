using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class atac_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");

                var usuario = ((USUARIO)Session["USUARIO"]);
                if (usuario.CODIGO_PERFIL == 4)
                    mnuModFinanceiro.Visible = false;

                if (usuario.CODIGO_PERFIL == 56)
                    Response.Redirect("atac_menu_externo.aspx");

            }
        }
    }
}
