using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class gerloja_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                USUARIO usuario = null;
                pnlLoja.Visible = false;
                pnlEscritorio.Visible = false;

                if (Session["USUARIO"] != null)
                {
                    usuario = new USUARIO();
                    usuario = (USUARIO)Session["USUARIO"];

                    if (usuario != null)
                    {
                        if (usuario.CODIGO_PERFIL == 2 || usuario.CODIGO_PERFIL == 3)
                        {
                            pnlLoja.Visible = true;
                            if (usuario.CODIGO_PERFIL == 2)
                            {
                                fsHandclub.Visible = false;
                                fsSupervisorLoja.Visible = false;
                            }
                            else
                            {
                                fsGerenteLoja.Visible = false;
                            }
                        }
                        else
                        {
                            pnlEscritorio.Visible = true;
                        }
                    }
                }


            }
        }
    }
}
