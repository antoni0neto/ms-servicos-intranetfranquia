using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class mkt_gin_natal2021 : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        string semana = "Início da Pontuação - 01/12/2021";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarPrincipal();
            }
        }

        #region "PRINCIPAL"
        private void CarregarPrincipal()
        {
            var gincana = lojaController.ObterGincanaNAT2021();
            if (gincana != null)
            {
                gvPrincipal.DataSource = gincana;
                gvPrincipal.DataBind();
                if (gincana.Count <= 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            }
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_GINCANA_NAT2021Result gin = e.Row.DataItem as SP_OBTER_GINCANA_NAT2021Result;

                    if (gin != null)
                    {

                        if (semana == "Início da Pontuação - 01/12/2021")
                        {
                            semana = gin.SEMANA + " - " + gin.DATA_INICIAL + " à " + gin.DATA_FIM;
                        }

                    }
                }
            }
        }
        protected void gvPrincipal_DataBound(object sender, EventArgs e)
        {
            labTitulo.Text = semana;
        }
        #endregion

        protected void btRegra_Click(object sender, EventArgs e)
        {

        }

    }
}

