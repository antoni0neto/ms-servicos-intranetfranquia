using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class ecom_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EcomController eController = new EcomController();

            if (!Page.IsPostBack)
            {

                if (Session["USUARIO"] != null)
                {
                    var usuario = (USUARIO)Session["USUARIO"];

                    if (usuario.CODIGO_PERFIL == 55)
                        Response.Redirect("ecom_menu_externo.aspx");

                    // MENU ESTOQUE
                    var estoqueLib = eController.ObterProdutoLinxLiberacaoCount();
                    mnuEstoque.Items[0].Text = "1. Liberação Estoque <font color='red'>(" + estoqueLib.ToString() + ")</font>";

                    var estoqueDev = eController.ObterProdutoLinxDevolucaoCount();
                    mnuEstoque.Items[1].Text = "2. Devolução Estoque <font color='red'>(" + estoqueDev.ToString() + ")</font>";

                    //MENU CONTROLE DE PEDIDO
                    var lib = eController.ObterPedidoExpedicao("", 'E');
                    mnuControle.Items[0].Text = "1. Aprovação de Pedido <font color='red'>(" + lib.Count().ToString() + ")</font>";

                    var dest = eController.ObterPedidoExpedicao("", 'N');
                    mnuControle.Items[1].Text = "2. Destino do Pedido <font color='red'>(" + dest.Count().ToString() + ")</font>";

                    var exp = eController.ObterPedidoExpedicao("", 'A');
                    mnuControle.Items[2].Text = "3. Expedição <font color='red'>(" + exp.Count().ToString() + ")</font>";

                    var fat = eController.ObterPedidoFaturamento("");
                    mnuControle.Items[3].Text = "4. Faturamento <font color='red'>(" + fat.Count().ToString() + ")</font>";

                    //MENU CONTROLE DE CLIENTES
                    var bol = eController.ObterPedidosMagBoleto("", DateTime.MinValue, DateTime.MaxValue, "", "", "", "payment_review", "");
                    mnuCRM.Items[1].Text = "2. Controle de Boletos <font color='red'>(" + bol.Count().ToString() + ")</font>";

                }
            }
        }
    }
}
