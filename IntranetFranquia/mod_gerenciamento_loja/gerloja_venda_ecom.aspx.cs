using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class gerloja_venda_ecom : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        decimal vTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!IsPostBack)
            {

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";



                CarregarFiliais();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFiliais()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais != null && filiais.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }

            }
        }

        #endregion

        private List<SP_OBTER_ECOM_VENDA_VENDEDORESResult> ObterVendaOnlineEcom()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            string codigoFilial = ddlFilial.SelectedValue.Trim();

            if (txtPeriodoInicial.Text.Trim() != "")
                dataIni = Convert.ToDateTime(txtPeriodoInicial.Text.Trim());

            if (txtPeriodoFinal.Text.Trim() != "")
                dataFim = Convert.ToDateTime(txtPeriodoFinal.Text.Trim());

            // se informar pelo codigo, buscar todas as vendas
            if (txtCODVendedor.Text.Trim() != "")
                codigoFilial = "";

            var venda = lojaController.ObterVendaOnlineEcom(dataIni, dataFim, txtCODVendedor.Text.Trim(), txtNome.Text.Trim(), codigoFilial);

            return venda;
        }
        private void CarregarVendas()
        {
            var venda = ObterVendaOnlineEcom();

            gvVendas.DataSource = venda;
            gvVendas.DataBind();
        }
        protected void btBuscarVendas_Click(object sender, EventArgs e)
        {
            try
            {

                labErro.Text = "";
                if (txtPeriodoInicial.Text.Trim() == "" && txtPeriodoFinal.Text.Trim() == "" && ddlFilial.SelectedValue.Trim() == ""
                    && txtNome.Text.Trim() == "" && txtCODVendedor.Text.Trim() == "")
                {
                    labErro.Text = "Informe pelo menos um filtro...";
                    return;
                }

                CarregarVendas();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_VENDA_VENDEDORESResult venda = e.Row.DataItem as SP_OBTER_ECOM_VENDA_VENDEDORESResult;

                    if (venda != null)
                    {

                        Literal litDataPagamento = e.Row.FindControl("litDataPagamento") as Literal;
                        litDataPagamento.Text = venda.DATA_PAGAMENTO.ToString("dd/MM/yyyy");

                        Literal litValorPedido = e.Row.FindControl("litValorPedido") as Literal;
                        litValorPedido.Text = "R$ " + Convert.ToDecimal(venda.VALOR_PEDIDO).ToString("###,###,##0.00");

                        Button btRastrearPedido = e.Row.FindControl("btRastrearPedido") as Button;
                        btRastrearPedido.CommandArgument = venda.TRACK_NUMBER.ToString();

                        vTotal += Convert.ToDecimal(venda.VALOR_PEDIDO);
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvVendas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVendas.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[6].Text = "R$ " + vTotal.ToString("###,###,##0.00");
            }
        }
        protected void gvVendas_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_VENDA_VENDEDORESResult> prods = ObterVendaOnlineEcom();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prods = prods.OrderBy(e.SortExpression + sortDirection);
            gvVendas.DataSource = prods;
            gvVendas.DataBind();
        }

        protected void btRastrearPedido_Click(object sender, EventArgs e)
        {
            try
            {
                var bt = (Button)sender;

                var trackNumber = bt.CommandArgument.Trim();

                var url = "";
                if (trackNumber.ToLower().Contains("hand"))
                    url = "https://rastreae.com.br/resultado/";
                else
                    url = "https://portalpostal.com.br/sro.jsp?sro=";

                url = url + trackNumber;
                Response.Redirect(url, "_blank", "");
            }
            catch (Exception ex)
            {
            }

        }

    }
}
