using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class desenv_pedido_altera : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) { }

        }

        protected void btBuscarPedido_Click(object sender, EventArgs e)
        {
            labPedido.Text = "";
            if (txtNumeroPedido.Text == "" || Convert.ToInt32(txtNumeroPedido.Text) <= 0)
            {
                labPedido.Text = "Informe o número do Pedido.";
                return;
            }

            try
            {
                DESENV_PEDIDO _pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtNumeroPedido.Text));

                if (_pedido == null)
                {
                    labPedido.Text = "Nenhum Pedido encontrado. Refaça a sua pesquisa.";
                    return;
                }

                /*
                if (_pedido.STATUS == 'F')
                {
                    labPedido.Text = "Este Pedido já foi ENTREGUE. Refaça a sua pesquisa.";
                    return;
                }*/

                if (_pedido.STATUS == 'E')
                {
                    labPedido.Text = "Este Pedido já foi EXCLUÍDO. Refaça a sua pesquisa.";
                    return;
                }

                Response.Redirect("desenv_pedido_cad.aspx?p=" + _pedido.CODIGO.ToString());

                labPedido.Text = "OK";
            }
            catch (Exception ex)
            {
                labPedido.Text = "ERRO: " + ex.Message;
            }
        }

        #region "DADOS INICIAIS"
        #endregion
    }
}
