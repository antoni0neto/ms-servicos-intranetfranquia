using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class GeraCustoProduto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
 
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btMovimento_Click(object sender, EventArgs e)
        {
            int contador=0;
            int contaalteracao = 0;

            List<PRODUTO_CUSTO> listaCustos = baseController.BuscaCustoProduto();

            if (listaCustos != null)
            {
                foreach (PRODUTO_CUSTO item in listaCustos)
                {
                    try
                    {
                        PRODUTO produto = baseController.BuscaProduto(item.CODIGO_PRODUTO.ToString());

                        if (produto.CUSTO_REPOSICAO1 != item.CUSTO_PRODUTO)
                            contaalteracao++;

                        produto.CUSTO_REPOSICAO1 = item.CUSTO_PRODUTO;

                        //baseController.AtualizaCustoProduto(produto);
                        contador++;
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }

                lblMensagem.Text = "Produtos Lidos: " + listaCustos.Count + " -> Produtos Alterados: " + contador;
            }
        }
    }
}
