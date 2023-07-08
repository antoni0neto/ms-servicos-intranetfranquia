using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class estoque_menu : System.Web.UI.Page
    {
        EstoqueController estoqueController = new EstoqueController();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Page.IsPostBack)
            {
                USUARIO usuario = null;

                if (Session["USUARIO"] != null)
                {
                    usuario = new USUARIO();
                    usuario = (USUARIO)Session["USUARIO"];

                    if (usuario.CODIGO_PERFIL == 2)
                    {
                        pnlLoja.Visible = true;
                    }
                    else
                    {
                        pnlRetaguarda.Visible = true;

                        var dataIni = Convert.ToDateTime("2017-10-27");
                        var dataFim = Convert.ToDateTime("2099-10-27");
                        var nfEntradaProdAcabado = estoqueController.ObterNFEntradaProdutoAcabado(dataIni, dataFim, "", "");
                        mnuBaixaEntrada.Items[0].Text = "1. Baixa NF Entrada Produto Acabado <font color='red'>(" + (nfEntradaProdAcabado.Count()).ToString() + ")</font>";
                    }

                }
            }
        }
    }
}
