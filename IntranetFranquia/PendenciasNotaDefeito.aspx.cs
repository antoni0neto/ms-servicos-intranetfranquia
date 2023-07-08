using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class PendenciasNotaDefeito : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregarTotalNotaFiscal();
        }

        private void CarregarTotalNotaFiscal()
        {
            gvTotalNotaDefeito.DataSource = baseController.BuscaItensNotaDefeito();
            gvTotalNotaDefeito.DataBind();
        }
    }
}