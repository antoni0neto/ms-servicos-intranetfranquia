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
    public partial class gerloja_venda_gincana2019 : System.Web.UI.Page
    {
        DesempenhoController desempenhoController = new DesempenhoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);


            if (!IsPostBack)
            {
                CarregarFilial();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });

                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais.Count() == 2)
                    ddlFilial.SelectedIndex = 1;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                DateTime dataIni = DateTime.Today;
                DateTime dataFim = DateTime.Today;

                if (txtPeriodoInicial.Text.Trim() == "" || !DateTime.TryParse(txtPeriodoInicial.Text.Trim(), out dataIni))
                {
                    labErro.Text = "Informe uma Data Inicial.";
                    return;
                }

                if (txtPeriodoFinal.Text.Trim() == "" || !DateTime.TryParse(txtPeriodoFinal.Text.Trim(), out dataFim))
                {
                    labErro.Text = "Informe uma Data Final.";
                    return;
                }

                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma Filial.";
                    return;
                }

                CarregarVendas(dataIni, dataFim, ddlFilial.SelectedValue.Trim());

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void CarregarVendas(DateTime dataIni, DateTime dataFim, string codigoFilial)
        {
            var gincana = desempenhoController.ObterDesempenhoGincana2019(dataIni, dataFim, codigoFilial);

            gvVendas.DataSource = gincana;
            gvVendas.DataBind();
        }

        protected void gvVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_GINCANA_NAT2019_PERIODOResult vendas = e.Row.DataItem as SP_OBTER_GINCANA_NAT2019_PERIODOResult;

            if (vendas != null)
            {

            }
        }

    }
}
