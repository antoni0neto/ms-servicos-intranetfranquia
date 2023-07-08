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
    public partial class gerloja_venda_cota_loja_semvale : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        decimal totalVendas = 0;
        decimal totalCotas = 0;
        decimal totalCotasRealizado = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!IsPostBack)
            {
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

                filiais.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "" });
                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();
            }
        }

        #endregion

        private void CarregarCotasVendas()
        {

            List<FILIAI> filiais = new List<FILIAI>();
            if (ddlFilial.SelectedValue == "0")
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                filiais = baseController.BuscaFiliais(usuario);
            }
            else
            {
                filiais.Add(new FILIAI { COD_FILIAL = ddlFilial.SelectedValue, FILIAL = ddlFilial.SelectedItem.Text });
            }

            if (filiais != null && filiais.Count > 0)
            {
                List<SP_OBTER_VENDASCOTAS_SEMVALEResult> vendasCotas = null;

                vendasCotas = baseController.ObterVendasCotasSemVale("", Convert.ToDateTime(txtPeriodoInicial.Text), Convert.ToDateTime(txtPeriodoFinal.Text));
                vendasCotas = vendasCotas.Where(p => filiais.Any(x => x.COD_FILIAL.Trim() == p.CODIGO_FILIAL.Trim())).ToList();

                if (vendasCotas != null && vendasCotas.Count > 0)
                {
                    vendasCotas = vendasCotas.GroupBy(p => new { DATA_VENDA = p.DATA_VENDA }).Select(x => new SP_OBTER_VENDASCOTAS_SEMVALEResult
                    {
                        DATA_VENDA = x.Key.DATA_VENDA,
                        VALOR_VENDA = x.Sum(g => g.VALOR_VENDA),
                        VALOR_COTA = x.Sum(g => g.VALOR_COTA)
                    }).ToList();

                    if (vendasCotas != null)
                    {
                        foreach (SP_OBTER_VENDASCOTAS_SEMVALEResult item in vendasCotas)
                        {
                            totalVendas += Convert.ToDecimal(item.VALOR_VENDA);
                            totalCotas += Convert.ToDecimal(item.VALOR_COTA);

                            if (item.VALOR_VENDA > 0)
                                totalCotasRealizado += Convert.ToDecimal(item.VALOR_COTA);
                        }
                    }

                    SP_OBTER_VENDASCOTAS_SEMVALEResult vendaCota = null;

                    vendaCota = new SP_OBTER_VENDASCOTAS_SEMVALEResult();
                    vendaCota.DATA_VENDA = "No Realizado";
                    vendaCota.VALOR_VENDA = totalVendas;
                    vendaCota.VALOR_COTA = totalCotasRealizado;

                    vendasCotas.Add(vendaCota);

                    vendaCota = new SP_OBTER_VENDASCOTAS_SEMVALEResult();
                    vendaCota.DATA_VENDA = "No Período";
                    vendaCota.VALOR_VENDA = totalVendas;
                    vendaCota.VALOR_COTA = totalCotas;

                    vendasCotas.Add(vendaCota);

                    gvVendas.DataSource = vendasCotas.OrderBy(p => p.DATA_VENDA);
                    gvVendas.DataBind();
                }
            }
        }
        protected void btBuscarVendas_Click(object sender, EventArgs e)
        {
            try
            {

                labErro.Text = "";
                if (txtPeriodoInicial.Text.Trim() == "" || txtPeriodoFinal.Text.Trim() == "")
                {
                    labErro.Text = "Informe uma Data de Início e uma Data Fim.";
                    return;
                }

                CarregarCotasVendas();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SP_OBTER_VENDASCOTAS_SEMVALEResult vendasCota = e.Row.DataItem as SP_OBTER_VENDASCOTAS_SEMVALEResult;

            if (vendasCota != null)
            {
                Literal literalValorVendas = e.Row.FindControl("LiteralValorVendas") as Literal;

                if (literalValorVendas != null)
                    literalValorVendas.Text = Convert.ToDecimal(vendasCota.VALOR_VENDA).ToString("N2");

                Literal literalValorCotas = e.Row.FindControl("LiteralValorCotas") as Literal;

                if (literalValorCotas != null)
                    literalValorCotas.Text = Convert.ToDecimal(vendasCota.VALOR_COTA).ToString("N2");

                Literal literalPercAtingido = e.Row.FindControl("LiteralPercAtingido") as Literal;

                if (literalPercAtingido != null)
                {
                    if (vendasCota.VALOR_COTA > 0)
                        literalPercAtingido.Text = Convert.ToDecimal((vendasCota.VALOR_VENDA / vendasCota.VALOR_COTA) * 100).ToString("N2") + "%";
                    else
                        literalPercAtingido.Text = "0,00%";
                }
            }
        }

    }
}
