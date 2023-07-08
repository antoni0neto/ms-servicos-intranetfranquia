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


namespace Relatorios
{
    public partial class gerloja_camp_guarulhos : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                string tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";


                CarregarFilialCompra();

                CarregarJQuery();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilialCompra()
        {
            var filialCompra = lojaController.ObterCampGuarulhos("", "", "").GroupBy(p => new { p.COD_FILIAL_COMPRA, p.FILIAL_COMPRA }).Select(
                x => new SP_OBTER_CAMP_GUARULHOSResult
                {
                    COD_FILIAL_COMPRA = x.Key.COD_FILIAL_COMPRA.Trim(),
                    FILIAL_COMPRA = x.Key.FILIAL_COMPRA.Trim()
                }).OrderBy(p => p.FILIAL_COMPRA).ToList();

            filialCompra.Insert(0, new SP_OBTER_CAMP_GUARULHOSResult { COD_FILIAL_COMPRA = "", FILIAL_COMPRA = "" });
            ddlFilialCompra.DataSource = filialCompra;
            ddlFilialCompra.DataBind();

            if (filialCompra != null && filialCompra.Count() == 2)
                ddlFilialCompra.SelectedIndex = 1;

        }

        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvClientes.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion


        private List<SP_OBTER_CAMP_GUARULHOSResult> ObterCampGuarulhos()
        {

            var clientes = lojaController.ObterCampGuarulhos(ddlFilialCompra.SelectedValue, txtCPF.Text.Trim(), txtNome.Text.Trim());

            if (ddlSexo.SelectedValue != "")
                clientes = clientes.Where(p => p.SEXO == ddlSexo.SelectedValue).ToList();

            return clientes;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvClientes.DataSource = ObterCampGuarulhos();
                gvClientes.DataBind();

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID CLIENTES"
        protected void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CAMP_GUARULHOSResult gru = e.Row.DataItem as SP_OBTER_CAMP_GUARULHOSResult;

                    Literal litAniversario = e.Row.FindControl("litAniversario") as Literal;
                    litAniversario.Text = (gru.ANIVERSARIO == null) ? "" : Convert.ToDateTime(gru.ANIVERSARIO).ToString("dd/MM/yyyy");

                    Literal litSexo = e.Row.FindControl("litSexo") as Literal;
                    litSexo.Text = (gru.SEXO == "M") ? "MASCULINO" : "FEMININO";

                }
            }
        }
        protected void gvClientes_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvClientes.FooterRow;
            if (_footer != null)
            {
            }
        }
        protected void gvClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_CAMP_GUARULHOSResult> clientes = ObterCampGuarulhos();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            clientes = clientes.OrderBy(e.SortExpression + sortDirection);
            gvClientes.DataSource = clientes;
            gvClientes.DataBind();

            CarregarJQuery();
        }

        #endregion


    }
}

