using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class crm_cliente : System.Web.UI.Page
    {
        CRMController crmController = new CRMController();

        const string CRM_CLIENTE_LIST = "CRM_CLIENTE_LIST";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                var tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "crm_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";

                CarregarTrimestre();
                CarregarAcoes();
                CarregarCRMTipo();

                Session[CRM_CLIENTE_LIST] = "";
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarTrimestre()
        {
            var tri = crmController.ObterTrimestre().Where(p => p.ATIVO == true).ToList();

            tri.Insert(0, new TRIMESTRE { DATA = new DateTime(1900, 1, 1), TRI = "Selecione" });
            ddlTri.DataSource = tri;
            ddlTri.DataBind();

            ddlTri.SelectedIndex = (tri.Count() - 1);

        }
        private void CarregarAcoes()
        {
            var acoes = crmController.ObterAcao();
            acoes.Insert(0, new CRM_ACAO { CODIGO = 0, ACAO = "" });
            ddlAcao.DataSource = acoes;
            ddlAcao.DataBind();
        }
        private void CarregarCRMTipo()
        {
            var tipos = crmController.ObterCRMTipo();
            tipos.Insert(0, new CRM_TIPO { CODIGO = 0, TIPO = "" });
            ddlTipoCliente.DataSource = tipos;
            ddlTipoCliente.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlTri.SelectedValue == "01/01/1900 00:00:00")
                {
                    labErro.Text = "Selecione o trimestre...";
                    return;
                }

                CarregarClientes();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private List<SP_OBTER_CRM_CLIENTEResult> ObterCRMCliente()
        {

            char? status = null;
            if (ddlStatus.SelectedValue != "")
                status = Convert.ToChar(ddlStatus.SelectedValue);

            var clientes = crmController.ObterCRMCliente(Convert.ToDateTime(ddlTri.SelectedValue), txtCPF.Text.Trim(), txtNome.Text.Trim(), status, Convert.ToInt32(ddlRecencia.SelectedValue),
                                Convert.ToInt32(ddlFrequencia.SelectedValue), Convert.ToInt32(ddlValor.SelectedValue), ddlCicloAtual.SelectedValue, ddlCicloAnterior.SelectedValue);

            if (ddlCLI4F.SelectedValue != "")
            {
                if (ddlCLI4F.SelectedValue == "S")
                    clientes = clientes.Where(p => p.CLI4F == true).ToList();
                else
                    clientes = clientes.Where(p => p.CLI4F == false).ToList();
            }

            if (ddlCompraOnline.SelectedValue != "")
            {
                if (ddlCompraOnline.SelectedValue == "S")
                    clientes = clientes.Where(p => p.TICKETS_ONLINE > 0).ToList();
                else
                    clientes = clientes.Where(p => p.TICKETS_ONLINE <= 0).ToList();
            }

            if (ddlTipoCliente.SelectedValue != "0")
                clientes = clientes.Where(p => p.CODIGO_TIPO_CLIENTE == Convert.ToInt32(ddlTipoCliente.SelectedValue)).ToList();

            if (ddlTeveAcao.SelectedValue != "")
            {
                if (ddlTeveAcao.SelectedValue == "S")
                    clientes = clientes.Where(p => p.ULTIMA_ACAO != null).ToList();
                else
                    clientes = clientes.Where(p => p.ULTIMA_ACAO == null).ToList();
            }

            if (ddlComprouUltimaAcao.SelectedValue != "")
            {
                if (ddlComprouUltimaAcao.SelectedValue == "S")
                    clientes = clientes.Where(p => p.ULTIMA_ACAO != null && (p.ULTIMA_COMPRA > p.ULTIMA_ACAO || p.ULTIMA_COMPRA_ONLINE > p.ULTIMA_ACAO)).ToList();
                else
                    clientes = clientes.Where(p => (p.ULTIMA_ACAO == null) || (p.ULTIMA_ACAO != null && (p.ULTIMA_COMPRA < p.ULTIMA_ACAO && p.ULTIMA_COMPRA_ONLINE < p.ULTIMA_ACAO))).ToList();
            }

            if (ddlAcao.SelectedValue != "0")
                clientes = clientes.Where(p => p.CODIGO_ACAO != null && p.CODIGO_ACAO == Convert.ToInt32(ddlAcao.SelectedValue)).ToList();

            return clientes;
        }
        private void CarregarClientes()
        {
            var clientes = ObterCRMCliente();
            gvCliente.DataSource = clientes;
            gvCliente.DataBind();

            Session[CRM_CLIENTE_LIST] = clientes;

            labQtdeCliente.Text = clientes.Count().ToString();
        }
        protected void gvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CRM_CLIENTEResult cli = e.Row.DataItem as SP_OBTER_CRM_CLIENTEResult;

                    Literal litAbrirCliente = e.Row.FindControl("litAbrirCliente") as Literal;
                    litAbrirCliente.Text = CriarLinkCliente(cli.CPF.Trim());

                    Literal litUltimaAcao = e.Row.FindControl("litUltimaAcao") as Literal;
                    litUltimaAcao.Text = (cli.ULTIMA_ACAO == null) ? "-" : Convert.ToDateTime(cli.ULTIMA_ACAO).ToString("dd/MM/yyyy");

                    Literal litAcao = e.Row.FindControl("litAcao") as Literal;
                    litAcao.Text = (cli.CODIGO_ACAO == null || cli.CODIGO_ACAO == 0) ? "-" : (crmController.ObterAcaoPorCodigo(Convert.ToInt32(cli.CODIGO_ACAO)).ACAO);

                    Literal litUltimaCompra = e.Row.FindControl("litUltimaCompra") as Literal;
                    litUltimaCompra.Text = (cli.ULTIMA_COMPRA == null) ? "-" : Convert.ToDateTime(cli.ULTIMA_COMPRA).ToString("dd/MM/yyyy");

                    Literal litUltimaCompraOnline = e.Row.FindControl("litUltimaCompraOnline") as Literal;
                    litUltimaCompraOnline.Text = (cli.ULTIMA_COMPRA_ONLINE == null || cli.ULTIMA_COMPRA_ONLINE.ToString() == "01/01/1900 00:00:00") ? "-" : Convert.ToDateTime(cli.ULTIMA_COMPRA_ONLINE).ToString("dd/MM/yyyy");

                    Literal litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                    litValorTotal.Text = "R$ " + cli.VALOR_TOTAL.ToString("###,###,###,##0.00");

                    Literal litValorTotalOnline = e.Row.FindControl("litValorTotalOnline") as Literal;
                    litValorTotalOnline.Text = (cli.VALOR_TOTAL_ONLINE == null) ? "-" : ("R$ " + Convert.ToDecimal(cli.VALOR_TOTAL_ONLINE).ToString("###,###,###,##0.00"));

                    Literal litCLI4F = e.Row.FindControl("litCLI4F") as Literal;
                    litCLI4F.Text = (cli.CLI4F == true) ? "Sim" : "Não";

                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCliente_DataBound(object sender, EventArgs e)
        {
            var footer = gvCliente.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
            }
        }
        protected void gvCliente_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[CRM_CLIENTE_LIST] != null)
            {
                IEnumerable<SP_OBTER_CRM_CLIENTEResult> clientes = ((IEnumerable<SP_OBTER_CRM_CLIENTEResult>)Session[CRM_CLIENTE_LIST]);

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                clientes = clientes.OrderBy(e.SortExpression + sortDirection);
                gvCliente.DataSource = clientes.ToList();
                gvCliente.DataBind();
            }
        }
        protected void gvCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCliente.PageIndex = e.NewPageIndex;
            CarregarClientes();
        }
        protected void ddlPaginaNumero_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageSize = Convert.ToInt32(ddlPaginaNumero.SelectedValue);
            gvCliente.PageSize = pageSize;
            CarregarClientes();
        }
        private string CriarLinkCliente(string cpf)
        {
            var link = "crm_cliente_det.aspx?t=1&ca=" + cpf;
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }

    }
}
