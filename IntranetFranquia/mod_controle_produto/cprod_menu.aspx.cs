using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class cprod_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                ControleProdutoController cprodController = new ControleProdutoController();

                // PRIMEIRO MENU
                var prodSemCad = cprodController.ObterProdutoSemCadastro("", "");
                mnuCadastro.Items[0].Text = "1. Cadastro de Produto <font color='red'>(" + prodSemCad.Count().ToString() + ")</font>";

                // SEGUNDO MENU
                var prodCortado = cprodController.ObterProdutoCortado(null, null, "", "", "").Where(p => p.TIPO == "00");
                mnuEtiqueta.Items[0].Text = "1. Etiqueta de Composição <font color='red'>(" + prodCortado.Count().ToString() + ")</font>";
            }
        }
    }
}
