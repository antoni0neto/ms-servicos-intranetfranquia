using DAL;
using System;
using System.Web.UI;

namespace Relatorios
{
    public partial class ecomv2_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EcomController eController = new EcomController();

            if (!Page.IsPostBack)
            {

                if (Session["USUARIO"] != null)
                {
                    var usuario = (USUARIO)Session["USUARIO"];
                }
            }
        }
    }
}
