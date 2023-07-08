using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class acomp_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                USUARIO usuario = Session["USUARIO"] as USUARIO;
                if (usuario != null)
                {
                    var dreService = new DREController();
                    var usuarioTemPermissao = dreService.ObterDREPermissao(usuario.CODIGO_USUARIO);

                    if (usuarioTemPermissao != null)
                    {
                        mDRE.Visible = true;
                    }
                }
            }

        }
    }
}
