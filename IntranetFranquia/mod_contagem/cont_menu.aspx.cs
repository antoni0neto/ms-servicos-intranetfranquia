using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class cont_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                USUARIO usuario = null;
                pnlLoja.Visible = false;
                pnlRetaguarda.Visible = false;

                if (Session["USUARIO"] != null)
                {
                    usuario = new USUARIO();
                    usuario = (USUARIO)Session["USUARIO"];

                    if (usuario != null)
                    {
                        int codigoPerfil = usuario.CODIGO_PERFIL;

                        if (codigoPerfil == 2 || codigoPerfil == 36 || codigoPerfil == 1 || codigoPerfil == 3 || codigoPerfil == 40)
                            pnlLoja.Visible = true;
                        if (codigoPerfil == 36 || codigoPerfil == 1 || codigoPerfil == 40)
                            pnlRetaguarda.Visible = true;

                        if (codigoPerfil == 3)
                        {
                            MenuItem item = new MenuItem();
                            item.Text = "4. Gráfico de Resultado";
                            item.NavigateUrl = "~/mod_contagem/cont_resultado_grafico.aspx";
                            mnuContagemLoja.Items.Add(item);
                        }

                        /************************************/
                    }
                }
            }
        }
    }
}
