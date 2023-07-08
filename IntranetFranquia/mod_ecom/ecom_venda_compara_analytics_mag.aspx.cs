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
    public partial class ecom_venda_compara_analytics_mag : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        decimal valAds = 0;
        decimal valAnalytics = 0;
        decimal valBruto = 0;
        decimal valBoletoPen = 0;
        decimal valCartaoPen = 0;
        decimal valCancelado = 0;
        decimal valPago = 0;
        int qtdeAnalytics = 0;
        int qtdeMagento = 0;
        decimal valCota;

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
                else if (tela == "2")
                    hrefVoltar.HRef = "ecom_menu_externo.aspx";

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_ECOM_VENDA_COM_ANALYTICSResult> ObterVendaComparativoAnalyticsMag(DateTime dataIni, DateTime dataFim)
        {
            return ecomController.ObterVendaComparativoAnalyticsMag(dataIni, dataFim);
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                DateTime dataIni;
                DateTime dataFim;

                if (txtDataIni.Text == "" || !DateTime.TryParse(txtDataIni.Text, out dataIni))
                {
                    labErro.Text = "Informe uma data válida para o Período Inicial";
                    return;
                }

                if (txtDataFim.Text == "" || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                {
                    labErro.Text = "Informe uma data válida para o Período Fim";
                    return;
                }

                CarregarPedidos(dataIni, dataFim);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void CarregarPedidos(DateTime dataIni, DateTime dataFim)
        {
            var pedidos = ObterVendaComparativoAnalyticsMag(dataIni, dataFim);

            gvPedido.DataSource = pedidos;
            gvPedido.DataBind();
        }

        protected void gvPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_VENDA_COM_ANALYTICSResult venda = e.Row.DataItem as SP_OBTER_ECOM_VENDA_COM_ANALYTICSResult;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = venda.DATA.ToString("dd/MM/yyyy");

                    Literal litValAds = e.Row.FindControl("litValAds") as Literal;
                    litValAds.Text = (venda.VALOR_ADS == null) ? "N/I" : ("R$ " + Convert.ToDecimal(venda.VALOR_ADS).ToString("###,###,###,##0.00"));

                    Literal litValAnalytics = e.Row.FindControl("litValAnalytics") as Literal;
                    litValAnalytics.Text = "R$ " + venda.VAL_ANALYTICS.ToString("###,###,###,##0.00");

                    Literal litValBruto = e.Row.FindControl("litValBruto") as Literal;
                    litValBruto.Text = "R$ " + venda.VAL_BRUTO.ToString("###,###,###,##0.00");

                    Literal litValBoletoPen = e.Row.FindControl("litValBoletoPen") as Literal;
                    litValBoletoPen.Text = "R$ " + venda.VAL_BOLETO_PEN.ToString("###,###,###,##0.00");

                    Literal litValCartaPen = e.Row.FindControl("litValCartaPen") as Literal;
                    litValCartaPen.Text = "R$ " + venda.VAL_CARTAO_PEN.ToString("###,###,###,##0.00");

                    Literal litValCancelado = e.Row.FindControl("litValCancelado") as Literal;
                    litValCancelado.Text = "R$ " + venda.VAL_CANCELADO.ToString("###,###,###,##0.00");

                    Literal litValPago = e.Row.FindControl("litValPago") as Literal;
                    litValPago.Text = "R$ " + venda.VAL_PAGO.ToString("###,###,###,##0.00");

                    Literal litValCota = e.Row.FindControl("litValCota") as Literal;
                    litValCota.Text = "R$ " + venda.COTAS.ToString("###,###,###,##0.00");

                    Literal litAtingidoBruto = e.Row.FindControl("litAtingidoBruto") as Literal;
                    litAtingidoBruto.Text = venda.ATINGIDO_BRUTO.ToString("N2") + "%";

                    Literal litAtingidoPago = e.Row.FindControl("litAtingidoPago") as Literal;
                    litAtingidoPago.Text = venda.ATINGIDO_PAGO.ToString("N2") + "%";


                    valAds += (venda.VALOR_ADS == null) ? 0 : Convert.ToDecimal(venda.VALOR_ADS);
                    valAnalytics += venda.VAL_ANALYTICS;
                    valBruto += venda.VAL_BRUTO;
                    valBoletoPen += venda.VAL_BOLETO_PEN;
                    valCartaoPen += venda.VAL_CARTAO_PEN;
                    valCancelado += venda.VAL_CANCELADO;
                    valPago += venda.VAL_PAGO;
                    qtdeAnalytics += venda.QTDE_PED_ANALYTICS;
                    qtdeMagento += venda.QTDE_PED_MAGENTO;

                    valCota += venda.COTAS;

                }
            }
        }
        protected void gvPedido_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPedido.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[2].Text = "R$ " + valAds.ToString("###,###,###0.00");
                _footer.Cells[3].Text = qtdeAnalytics.ToString();
                _footer.Cells[4].Text = "R$ " + valAnalytics.ToString("###,###,###0.00");
                _footer.Cells[5].Text = qtdeMagento.ToString();

                _footer.Cells[6].Text = "R$ " + valBruto.ToString("###,###,###0.00");
                _footer.Cells[7].Text = "R$ " + valBoletoPen.ToString("###,###,###0.00");
                _footer.Cells[8].Text = "R$ " + valCartaoPen.ToString("###,###,###0.00");
                _footer.Cells[9].Text = "R$ " + valCancelado.ToString("###,###,###0.00");
                _footer.Cells[10].Text = "R$ " + valPago.ToString("###,###,###0.00");

                _footer.Cells[11].Text = "R$ " + valCota.ToString("###,###,###0.00");

                var atingidoBruto = 0.00M;
                var atingidoPago = 0.00M;
                if (valCota > 0)
                {
                    atingidoBruto = (valBruto / valCota * 100.00M);
                    atingidoPago = (valPago / valCota * 100.00M);
                }

                _footer.Cells[12].Text = atingidoBruto.ToString("N2") + "%";
                _footer.Cells[13].Text = atingidoPago.ToString("N2") + "%";
            }
        }

        protected void btMes_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime agora = DateTime.Today;
                var mes = (agora.Month.ToString().Length == 1) ? "0" + agora.Month.ToString() : agora.Month.ToString();
                DateTime dataIni = Convert.ToDateTime(agora.Year.ToString() + "-" + mes + "-01");
                var ultimoDia = DateTime.DaysInMonth(agora.Year, agora.Month).ToString();
                DateTime dataFim = Convert.ToDateTime(agora.Year.ToString() + "-" + mes + "-" + ultimoDia);
                txtDataIni.Text = dataIni.ToString("dd/MM/yyyy");
                txtDataFim.Text = dataFim.ToString("dd/MM/yyyy");
                CarregarPedidos(dataIni, dataFim);
            }
            catch (Exception)
            {
            }
        }

        protected void btSemana_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime agora = DateTime.Today;
                DateTime dataIni = agora.AddDays(-7);
                DateTime dataFim = agora;
                txtDataIni.Text = dataIni.ToString("dd/MM/yyyy");
                txtDataFim.Text = dataFim.ToString("dd/MM/yyyy");
                CarregarPedidos(dataIni, dataFim);
            }
            catch (Exception)
            {
            }
        }

        protected void btDia_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime agora = DateTime.Today;
                DateTime dataIni = agora;
                DateTime dataFim = agora;
                txtDataIni.Text = dataIni.ToString("dd/MM/yyyy");
                txtDataFim.Text = dataFim.ToString("dd/MM/yyyy");
                CarregarPedidos(dataIni, dataFim);
            }
            catch (Exception)
            {
            }
        }

        #region "DADOS INICIAIS"

        #endregion

    }
}
