using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class prod_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }

                //ProducaoController prodController = new ProducaoController();
                // ControleProdutoController cprodController = new ControleProdutoController();
                var usuario = (USUARIO)Session["USUARIO"];

                //pnlProducao1.Visible = false;
                //pnlProducao2.Visible = false;
                //if (usuario.CODIGO_PERFIL == 42)
                //{
                //    pnlProducao2.Visible = true;
                //}
                //else
                //{
                //var produtoSimuladoN = prodController.ObterProdutoCustoSimulado("", "", "", ' ', 'N');
                //menuCusto.Items[2].Text = "3. Lista Simulação de Custo/Preço <font color='red'>(" + produtoSimuladoN.Count.ToString() + ")</font>";

                //var produtoSimuladoS = prodController.ObterProdutoCustoSimulado("", "", "", ' ', 'S');
                //menuCusto.Items[4].Text = "5. Lista Aprovação de Preço <font color='red'>(" + produtoSimuladoS.Count.ToString() + ")</font>";

                //var produtoSemPreco = prodController.ObterProdutoSemCusto("", "", "", ' ').Where(p => p.CORTADO == true).ToList();
                //menuCusto.Items[6].Text = "7. Produtos Sem Preço <font color='red'>(" + produtoSemPreco.Count.ToString() + ")</font>";

                //var etiquetaComp = cprodController.ObterProdutoCortado(null, "", null, "");
                //menuCusto.Items[8].Text = "9. Etiqueta de Composição <font color='red'>(" + etiquetaComp.Count().ToString() + ")</font>";

                //var defLogistica = prodController.ObterDefinicaoGradeAtacado("", null, "");
                //menuLogistica.Items[0].Text = "1. Definição Logística <font color='red'>(" + defLogistica.Count().ToString() + ")</font>";

                //var defLogisticaImp = prodController.ObterDefinicaoGradeAtacadoImpressao("", null, "");
                //menuLogistica.Items[1].Text = "2. Impressão Ficha Logística <font color='red'>(" + defLogisticaImp.Count().ToString() + ")</font>";

                pnlProducao1.Visible = true;

            }

        }
    }
}
