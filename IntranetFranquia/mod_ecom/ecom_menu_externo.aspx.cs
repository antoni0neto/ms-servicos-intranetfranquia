using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class ecom_menu_externo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EcomController eController = new EcomController();

            if (!Page.IsPostBack)
            {
                USUARIO usuario = null;

                if (Session["USUARIO"] != null)
                {
                    usuario = new USUARIO();
                    usuario = (USUARIO)Session["USUARIO"];

                }
            }
        }
    }
}
