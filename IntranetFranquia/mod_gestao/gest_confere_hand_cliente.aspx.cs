using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class gest_confere_hand_cliente : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        decimal totalPontoCampanha = 0;

        decimal totalValorBruto = 0;
        decimal totalValorPago = 0;
        decimal totalPonto = 0;
        decimal totalDesconto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "gest_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        private void CarregarDados(string codigoCliente)
        {
            var d = DateTime.Now;

            var dataIniResgate = new DateTime(d.Year, d.Month, 1);
            var dataFimResgate = dataIniResgate.AddMonths(1).AddDays(-1);

            var dataIniAcumulado = dataIniResgate.AddMonths(-1);
            var dataFimAcumulado = dataIniResgate.AddDays(-1);

            var mesAcumulado = lojaController.ObterVendaPorClienteEData(codigoCliente, dataIniAcumulado, dataFimAcumulado).Where(p => p.DATA_HORA_CANCELAMENTO == null);
            gvPontoAcumulado.DataSource = mesAcumulado;
            gvPontoAcumulado.DataBind();

            totalValorBruto = 0;
            totalValorPago = 0;
            totalPonto = 0;
            totalDesconto = 0;

            var mesResgatado = lojaController.ObterVendaPorClienteEData(codigoCliente, dataIniResgate, dataFimResgate).Where(p => p.DATA_HORA_CANCELAMENTO == null);
            gvPontoResgatado.DataSource = mesResgatado;
            gvPontoResgatado.DataBind();

            var pontoCampanha = lojaController.ObterPontosCampanha(codigoCliente, dataIniResgate, dataFimResgate);
            gvCampanha.DataSource = pontoCampanha;
            gvCampanha.DataBind();

            var saldo = lojaController.ObterSaldoPontos(codigoCliente, dataIniResgate, dataFimResgate);
            txtSaldoPonto.Text = "";
            if (saldo != null)
            {
                txtSaldoPonto.Text = saldo.SALDO_PONTOS.ToString();
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (txtCPFFiltro.Text.Trim() == "")
                {
                    labErro.Text = "Informe o CPF do Cliente";
                    return;
                }

                var cliente = baseController.BuscaClienteCPF(txtCPFFiltro.Text.Trim());
                if (cliente == null)
                {
                    labErro.Text = "Cliente não encontrado.";
                    return;
                }

                txtCodigoCliente.Text = cliente.CODIGO_CLIENTE;
                txtCPF.Text = cliente.CPF_CGC;
                txtCliente.Text = cliente.CLIENTE_VAREJO;
                txtDDD.Text = cliente.DDD;
                txtTelefone.Text = cliente.TELEFONE;
                txtEmail.Text = cliente.EMAIL;

                CarregarDados(cliente.CODIGO_CLIENTE);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvPonto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LOJA_VENDA lv = e.Row.DataItem as LOJA_VENDA;

                    Literal litFilial = e.Row.FindControl("litFilial") as Literal;
                    litFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(lv.CODIGO_FILIAL)).FILIAL;

                    Literal litTicket = e.Row.FindControl("litTicket") as Literal;
                    litTicket.Text = lv.TICKET;

                    Literal litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                    litDataVenda.Text = lv.DATA_VENDA.ToString("dd/MM/yyyy");

                    Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                    litValorPago.Text = Convert.ToDecimal(lv.VALOR_PAGO).ToString("###,###,###,##0.00");

                    Literal litPonto = e.Row.FindControl("litPonto") as Literal;
                    if (litPonto != null)
                        litPonto.Text = Convert.ToDecimal(lv.QTDE_PONTOS_ACUMULADOS).ToString("###,###,###,##0.00");

                    Literal litValorBruto = e.Row.FindControl("litValorBruto") as Literal;
                    if (litValorBruto != null)
                        litValorBruto.Text = Convert.ToDecimal(lv.VALOR_VENDA_BRUTA).ToString("###,###,###,##0.00");

                    Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                    if (litDesconto != null)
                        litDesconto.Text = Convert.ToDecimal(lv.DESCONTO).ToString("###,###,###,##0.00");


                    totalValorBruto += Convert.ToDecimal(lv.VALOR_VENDA_BRUTA);
                    totalValorPago += Convert.ToDecimal(lv.VALOR_PAGO);
                    totalPonto += Convert.ToDecimal(lv.QTDE_PONTOS_ACUMULADOS);
                    totalDesconto += Convert.ToDecimal(lv.DESCONTO);

                }
            }
        }
        protected void gvCampanha_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    LJ_FIDELIDADE_PONTO_CAMPANHA lv = e.Row.DataItem as LJ_FIDELIDADE_PONTO_CAMPANHA;

                    Literal litCodigoPrograma = e.Row.FindControl("litCodigoPrograma") as Literal;
                    litCodigoPrograma.Text = lv.COD_FIDELIDADE_PROGRAMA.ToString();

                    Literal litCodigoCampanha = e.Row.FindControl("litCodigoCampanha") as Literal;
                    litCodigoCampanha.Text = lv.CODIGO_CAMPANHA.ToString();

                    Literal litCampanha = e.Row.FindControl("litCampanha") as Literal;
                    litCampanha.Text = lojaController.ObterCampanhaHandclub(lv.CODIGO_CAMPANHA).CAMPANHA;

                    Literal litPonto = e.Row.FindControl("litPonto") as Literal;
                    litPonto.Text = lv.PONTOS.ToString("###,###,###,##0.00");

                    totalPontoCampanha += lv.PONTOS;

                }
            }
        }

        protected void gvPontoResgatado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPontoResgatado.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = totalValorBruto.ToString();
                footer.Cells[5].Text = totalValorPago.ToString();
                footer.Cells[6].Text = totalDesconto.ToString();
            }
        }

        protected void gvPontoAcumulado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPontoAcumulado.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[5].Text = totalValorPago.ToString();
                footer.Cells[6].Text = totalPonto.ToString();
            }
        }

        protected void gvCampanha_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCampanha.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = totalPontoCampanha.ToString();
            }
        }



    }
}
