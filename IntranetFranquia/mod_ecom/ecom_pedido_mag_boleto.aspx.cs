using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_pedido_mag_boleto : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        decimal valBoleto = 0;

        const string PEDIDO_MAG_BOL_SESSION = "PEDIDO_MAG_BOL_SESSION";

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
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
                    hrefVoltar.HRef = "ecom_menu.aspx";

                CarregarStatusMag();
                CarregarStatusCliente();

                Session[PEDIDO_MAG_BOL_SESSION] = null;

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btLimpar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btLimpar, null) + ";");
        }

        private List<SP_OBTER_ECOM_PEDIDO_MAG_BOLETOResult> ObterPedidosMagBoleto(string pedidoExterno, DateTime dataIni, DateTime dataFim, string nome, string email, string cpf, string statusMag, string statusCliente)
        {
            var pMag = ecomController.ObterPedidosMagBoleto(pedidoExterno, dataIni, dataFim, nome, email, cpf, statusMag, statusCliente);

            if (ddlTotalContato.SelectedValue != "")
            {
                var totalContato = Convert.ToInt32(ddlTotalContato.SelectedValue);
                if (totalContato >= 0 && totalContato <= 4)
                    pMag = pMag.Where(p => p.TOTAL_CONTATO == totalContato).ToList();
                else
                    pMag = pMag.Where(p => p.TOTAL_CONTATO >= 5).ToList();
            }

            return pMag;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                DateTime dataIni = DateTime.MinValue;
                DateTime dataFim = DateTime.MaxValue;

                if (txtDataIni.Text != "" && !DateTime.TryParse(txtDataIni.Text, out dataIni))
                {
                    labErro.Text = "Informe uma data válida para o Período Inicial.";
                    return;
                }

                if (txtDataFim.Text != "" && !DateTime.TryParse(txtDataFim.Text, out dataFim))
                {
                    labErro.Text = "Informe uma data válida para o Período Fim.";
                    return;
                }

                CarregarPedidos(txtPedido.Text.Trim(), dataIni, dataFim, txtNome.Text.Trim(), txtEmail.Text.Trim(), txtCPF.Text.Trim(), ddlStatusMag.SelectedValue, ddlStatusCliente.SelectedValue);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void CarregarPedidos(string pedidoExterno, DateTime dataIni, DateTime dataFim, string nome, string email, string cpf, string statusMag, string statusCliente)
        {
            var pedidos = ObterPedidosMagBoleto(pedidoExterno, dataIni, dataFim, nome, email, cpf, statusMag, statusCliente);

            gvPedido.DataSource = pedidos;
            gvPedido.DataBind();

            Session[PEDIDO_MAG_BOL_SESSION] = pedidos;
        }

        protected void gvPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_MAG_BOLETOResult pedido = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_MAG_BOLETOResult;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = pedido.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litDataBolExp = e.Row.FindControl("litDataBolExp") as Literal;
                    litDataBolExp.Text = (pedido.BOLETO_EXPIRACAO == null) ? "-" : Convert.ToDateTime(pedido.BOLETO_EXPIRACAO).ToString("dd/MM/yyyy");

                    Literal litValBoleto = e.Row.FindControl("litValBoleto") as Literal;
                    litValBoleto.Text = "R$ " + Convert.ToDecimal(pedido.VALOR_BOLETO).ToString("###,###,###,##0.00");

                    Literal litUltimoContato = e.Row.FindControl("litUltimoContato") as Literal;
                    litUltimoContato.Text = (pedido.ULTIMO_CONTATO == null) ? "-" : Convert.ToDateTime(pedido.ULTIMO_CONTATO).ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litAbrirPedido = e.Row.FindControl("litAbrirPedido") as Literal;
                    litAbrirPedido.Text = CriarLinkPedido(pedido.PEDIDO_EXTERNO);

                    Literal litAbrirHistorico = e.Row.FindControl("litAbrirHistorico") as Literal;
                    litAbrirHistorico.Text = CriarLinkPedidoHist(pedido.PEDIDO_EXTERNO);

                    valBoleto += Convert.ToDecimal(pedido.VALOR_BOLETO);

                    if (pedido.TOTAL_CONTATO <= 0 && (pedido.STATUS_MAGENTO == "pending_payment" || pedido.STATUS_MAGENTO == "pending"))
                        e.Row.BackColor = Color.LavenderBlush;

                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPedido_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPedido.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[6].Text = "R$ " + valBoleto.ToString("###,###,###0.00");

            }
        }
        protected void gvPedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PEDIDO_MAG_BOL_SESSION] != null)
            {
                IEnumerable<SP_OBTER_ECOM_PEDIDO_MAG_BOLETOResult> pedidos = ((IEnumerable<SP_OBTER_ECOM_PEDIDO_MAG_BOLETOResult>)Session[PEDIDO_MAG_BOL_SESSION]);

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                pedidos = pedidos.OrderBy(e.SortExpression + sortDirection);
                gvPedido.DataSource = pedidos;
                gvPedido.DataBind();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarStatusMag()
        {
            var statusMag = ecomController.ObterEcomPedidoMagStatus();
            statusMag.Insert(0, new ECOM_PEDIDO_MAG_STATUS { CODIGO = "", DESCRICAO = "" });
            ddlStatusMag.DataSource = statusMag;
            ddlStatusMag.DataBind();

            //ddlStatusMag.SelectedValue = "pending_payment";

        }
        private void CarregarStatusCliente()
        {
            var statusMagCliente = ecomController.ObterEcomPedidoMagClienteHistStatus();
            statusMagCliente.Insert(0, new ECOM_PEDIDO_CLIENTE_HIST_STA { CODIGO = 0, MOTIVO = "" });
            ddlStatusCliente.DataSource = statusMagCliente;
            ddlStatusCliente.DataBind();
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            txtPedido.Text = "";
            txtDataIni.Text = "";
            txtDataFim.Text = "";
            txtNome.Text = "";
            txtCPF.Text = "";
            txtEmail.Text = "";
            ddlStatusMag.SelectedValue = "";
            ddlStatusCliente.SelectedValue = "0";
            ddlTotalContato.SelectedValue = "";
            Session[PEDIDO_MAG_BOL_SESSION] = null;

            gvPedido.DataSource = new List<SP_OBTER_ECOM_PEDIDO_MAGResult>();
            gvPedido.DataBind();
        }
        #endregion

        private string CriarLinkPedido(string pedidoExterno)
        {
            var link = "ecom_pedido_mag_det.aspx?p=" + pedidoExterno.Trim();
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }
        private string CriarLinkPedidoHist(string pedidoExterno)
        {
            var link = "ecom_pedido_mag_histbol_cli.aspx?p=" + pedidoExterno.Trim();
            var linkOk = "<a href=\"javascript: openwindowhist('" + link + "')\"><img alt='' src='../../Image/email.png' width='13px' /></a>";
            return linkOk;
        }
    }
}
