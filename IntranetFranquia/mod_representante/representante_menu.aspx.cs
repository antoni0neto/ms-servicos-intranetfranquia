using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class representante_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            USUARIO usuario = null;
            pnlVendas.Visible = false;

            if (Session["USUARIO"] != null)
            {
                usuario = new USUARIO();
                usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                {
                    int codigoPerfil = usuario.CODIGO_PERFIL;

                    //if (codigoPerfil == 2 || codigoPerfil == 11 || codigoPerfil == 1 || codigoPerfil == 3)
                        pnlVendas.Visible = true;
                }
            }
        }
    }
}