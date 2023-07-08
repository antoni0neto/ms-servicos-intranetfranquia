using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class DefaultManutencaoMensal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] != null)
                {
                    USUARIO usuario = (USUARIO)Session["USUARIO"];
                    if (usuario != null)
                    {

                        if (usuario.CODIGO_USUARIO == 1144 || usuario.CODIGO_USUARIO == 1155 || usuario.CODIGO_USUARIO == 1142 || usuario.CODIGO_USUARIO == 18)
                        {
                            mnuAluguel.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
