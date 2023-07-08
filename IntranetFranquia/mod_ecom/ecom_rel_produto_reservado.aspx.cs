using DAL;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class ecom_rel_produto_reservado : System.Web.UI.Page
    {
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarProdutoReservado();
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

        private List<SP_OBTER_ECOM_PRODUTO_RESERVADOResult> ObterProdutoReservado()
        {

            var produtoReservado = eController.ObterProdutoReservado();

            if (txtProduto.Text != "")
                produtoReservado = produtoReservado.Where(p => p.SKU.Contains(txtProduto.Text.Trim())).ToList();

            return produtoReservado;
        }
        private void CarregarProdutoReservado()
        {
            gvProduto.DataSource = ObterProdutoReservado();
            gvProduto.DataBind();
        }

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_RESERVADOResult reserva = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_RESERVADOResult;

                    Literal litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                    if (litDataVenda != null)
                        litDataVenda.Text = reserva.DATA_VENDA.ToString("dd/MM/yyyy");

                }
            }
        }

        #endregion

        protected void gvProduto_DataBound(object sender, EventArgs e)
        {

        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarProdutoReservado();
            }
            catch (Exception)
            {


            }
        }


    }
}

