using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using Relatorios.mod_ecom.mag;


namespace Relatorios
{
    public partial class ecom_dev_estoque_motivo : System.Web.UI.Page
    {
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);


            if (!Page.IsPostBack)
            {
                CarregarJQuery();
            }

        }

        #region "DADOS INICIAIS"

        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        private List<SP_OBTER_ECOM_PRODUTO_DEV_MOTIVOResult> ObterProdutoLinxDevolucao()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;

            if (txtDataIni.Text != "")
                dataIni = Convert.ToDateTime(txtDataIni.Text.Trim());

            if (txtDataFim.Text != "")
                dataFim = Convert.ToDateTime(txtDataFim.Text.Trim());

            return eController.ObterProdutoLinxDevolucaoMotivo(txtProduto.Text.Trim(), txtNFEntrada.Text.Trim(), ddlMotivo.SelectedValue.Trim(), dataIni, dataFim);
        }
        private void CarregarProdutoDevolvidos()
        {
            gvProduto.DataSource = ObterProdutoLinxDevolucao();
            gvProduto.DataBind();

            CarregarJQuery();
        }

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_DEV_MOTIVOResult dev = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_DEV_MOTIVOResult;

                    Literal _litRecebimento = e.Row.FindControl("litRecebimento") as Literal;
                    if (_litRecebimento != null)
                        _litRecebimento.Text = dev.RECEBIMENTO.ToString("dd/MM/yyyy");

                    DropDownList ddlMotivo = e.Row.FindControl("ddlMotivo") as DropDownList;
                    if (dev.MOTIVO != "")
                        ddlMotivo.SelectedValue = dev.MOTIVO;

                }
            }
        }

        #endregion

        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
        }

        protected void ddlMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var ddl = (DropDownList)sender;
                GridViewRow row = (GridViewRow)ddl.NamingContainer;

                if (row != null)
                {
                    int codigo = Convert.ToInt32(gvProduto.DataKeys[row.RowIndex].Value);
                    if (codigo > 0)
                    {
                        var dev = eController.ObterProdutoEstoqueDevolvido(codigo);
                        if (dev != null)
                        {
                            dev.MOTIVO = ddl.SelectedValue;

                            eController.AtualizarMagentoEstoqueDevolvido(dev);
                        }
                    }
                }

                CarregarJQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarProdutoDevolvidos();
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}

