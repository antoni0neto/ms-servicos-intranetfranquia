using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class fin_prod_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProducaoController prodController = new ProducaoController();
            ControleProdutoController cprodController = new ControleProdutoController();
            USUARIO usuario = null;

            if (Session["USUARIO"] != null)
            {
                usuario = new USUARIO();
                usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                {
                    var etiquetaPreco = prodController.ObterHBSemEtiquetaBaixada();
                    mnuProducao.Items[0].Text = "1. Liberação Etiqueta de Preço <font color='red'>(" + etiquetaPreco.Count().ToString() + ")</font>";

                    var etiquetaComp = cprodController.ObterProdutoCortado(null, null, "", "", "");
                    mnuProducao.Items[1].Text = "2. Liberação Etiqueta de Composição <font color='red'>(" + etiquetaComp.Count().ToString() + ")</font>";
                }
            }
        }
    }
}