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
    public partial class ecom_pedido_mag : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        decimal valPago = 0;
        decimal valFrete = 0;
        decimal valSubTotal = 0;

        const string PEDIDO_MAG_SESSION = "PEDIDO_MAG_SESSION";
        string callGridViewScroll = "$(function () { gridviewScroll(); });";

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

                Session[PEDIDO_MAG_SESSION] = null;

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btLimpar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btLimpar, null) + ";");
        }

        private List<SP_OBTER_ECOM_PEDIDO_MAGResult> ObterPedidosMag(string pedidoExterno, DateTime dataIni, DateTime dataFim, string nome, string metodo, string statusMag, decimal valPagoIni, decimal valPagoFim, string email, string cpf)
        {
            var pMag = ecomController.ObterPedidosMag(pedidoExterno, dataIni, dataFim, nome, metodo, statusMag, valPagoIni, valPagoFim, email, cpf);

            if (ddlFraude.SelectedValue != "")
            {
                if (ddlFraude.SelectedValue == "S")
                    pMag = pMag.Where(p => p.FRAUDE == true).ToList();
                else
                    pMag = pMag.Where(p => p.FRAUDE == false).ToList();
            }

            if (txtCEP.Text.Trim() != "")
                pMag = pMag.Where(p => p.CEP_ENTREGA.Contains(txtCEP.Text.Trim())).ToList();

            if (txtCupom.Text.Trim() != "")
                pMag = pMag.Where(p => p.CUPOM.ToLower().Trim() == txtCupom.Text.ToLower().Trim()).ToList();


            return pMag;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                DateTime dataIni = DateTime.MinValue;
                DateTime dataFim = DateTime.MaxValue;
                decimal valIni = 0;
                decimal valFim = 0;

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

                if (txtValIni.Text != "" && !decimal.TryParse(txtValIni.Text, out valIni))
                {
                    labErro.Text = "Informe um valor inicial válido.";
                    return;
                }

                if (txtValFim.Text != "" && !decimal.TryParse(txtValFim.Text, out valFim))
                {
                    labErro.Text = "Informe um valor final válido.";
                    return;
                }

                CarregarPedidos(txtPedido.Text.Trim(), dataIni, dataFim, txtNome.Text.Trim(), ddlMetodo.SelectedValue, ddlStatus.SelectedValue, valIni, valFim, txtEmail.Text.Trim(), txtCPF.Text.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void CarregarPedidos(string pedidoExterno, DateTime dataIni, DateTime dataFim, string nome, string metodo, string statusMag, decimal valPagoIni, decimal valPagoFim, string email, string cpf)
        {
            var pedidos = ObterPedidosMag(pedidoExterno, dataIni, dataFim, nome, metodo, statusMag, valPagoIni, valPagoFim, email, cpf);

            gvPedido.DataSource = pedidos;
            gvPedido.DataBind();

            Session[PEDIDO_MAG_SESSION] = pedidos;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
        }

        protected void gvPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_MAGResult pedido = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_MAGResult;

                    Literal litAbrirPedido = e.Row.FindControl("litAbrirPedido") as Literal;
                    litAbrirPedido.Text = CriarLinkPedido(pedido.PEDIDO_EXTERNO);

                    Literal litAbrirHistorico = e.Row.FindControl("litAbrirHistorico") as Literal;
                    litAbrirHistorico.Text = CriarLinkPedidoHist(pedido.PEDIDO_EXTERNO);

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = pedido.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litValSubTotal = e.Row.FindControl("litValSubTotal") as Literal;
                    litValSubTotal.Text = "R$ " + pedido.VALOR_SUBTOTAL.ToString("###,###,###,##0.00");

                    Literal litValPago = e.Row.FindControl("litValPago") as Literal;
                    litValPago.Text = "R$ " + pedido.VALOR_PAGO.ToString("###,###,###,##0.00");

                    Literal litValFrete = e.Row.FindControl("litValFrete") as Literal;
                    litValFrete.Text = "R$ " + pedido.VALOR_FRETE.ToString("###,###,###,##0.00");

                    Literal litEndereco = e.Row.FindControl("litEndereco") as Literal;
                    litEndereco.Text = pedido.RUA_ENTREGA;

                    Literal litCidadeEstado = e.Row.FindControl("litCidadeEstado") as Literal;
                    litCidadeEstado.Text = pedido.CIDADE_ENTREGA + "/" + pedido.ESTADO_ENTREGA;

                    Literal litCEP = e.Row.FindControl("litCEP") as Literal;
                    litCEP.Text = pedido.CEP_ENTREGA;

                    valPago += pedido.VALOR_PAGO;
                    valFrete += pedido.VALOR_FRETE;
                    valSubTotal += pedido.VALOR_SUBTOTAL;

                    if (pedido.FRAUDE)
                        e.Row.BackColor = Color.SeaShell;

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

                _footer.Cells[8].Text = "R$ " + valSubTotal.ToString("###,###,###0.00");
                _footer.Cells[9].Text = "R$ " + valFrete.ToString("###,###,###0.00");
                _footer.Cells[10].Text = "R$ " + valPago.ToString("###,###,###0.00");

            }
        }
        protected void gvPedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PEDIDO_MAG_SESSION] != null)
            {
                IEnumerable<SP_OBTER_ECOM_PEDIDO_MAGResult> pedidos = ((IEnumerable<SP_OBTER_ECOM_PEDIDO_MAGResult>)Session[PEDIDO_MAG_SESSION]);

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

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarStatusMag()
        {
            var statusMag = ecomController.ObterEcomPedidoMagStatus();
            statusMag.Insert(0, new ECOM_PEDIDO_MAG_STATUS { CODIGO = "", DESCRICAO = "" });
            ddlStatus.DataSource = statusMag;
            ddlStatus.DataBind();
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            txtPedido.Text = "";
            txtDataIni.Text = "";
            txtDataFim.Text = "";
            txtNome.Text = "";
            txtCPF.Text = "";
            txtEmail.Text = "";
            ddlMetodo.SelectedValue = "";
            txtValIni.Text = "";
            txtValFim.Text = "";
            ddlStatus.SelectedValue = "";
            txtCEP.Text = "";
            Session[PEDIDO_MAG_SESSION] = null;

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
