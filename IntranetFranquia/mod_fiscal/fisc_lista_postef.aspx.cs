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
    public partial class fisc_lista_postef : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaVendasPOSTEF();
        }

        private void CarregaVendasPOSTEF()
        {
            List<p_valida_pos_tef_relatorio_intranetResult> listaPosTef = baseController.BuscaPosTef();

            gvVendas.DataSource = listaPosTef;
            gvVendas.DataBind();
        }

    }
}
