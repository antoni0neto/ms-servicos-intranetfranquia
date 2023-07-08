using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Relatorios
{
    public partial class pacab_pedido_produto_linx_pgto : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();


        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto1.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto2.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto3.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto4.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto5.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto6.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto7.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto8.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                var pedidoIntra = Request.QueryString["ped"].ToString();
                var produto = Request.QueryString["prod"].ToString();
                var cor = Request.QueryString["cor"].ToString();
                var entrega = Convert.ToDateTime(Request.QueryString["en"].ToString());
                var tipo = Request.QueryString["tip"].ToString();

                var compraProduto = desenvController.ObterComprasProdutoPrePedido(pedidoIntra, produto, cor, entrega);
                if (compraProduto != null)
                {
                    txtEntregaIni.Text = compraProduto.ENTREGA.ToString("dd/MM/yyyy");
                    txtPrevFinal.Text = compraProduto.LIMITE_ENTREGA.ToString("dd/MM/yyyy");
                    txtValor.Text = "R$" + Convert.ToDecimal((compraProduto.QTDE_ORIGINAL * compraProduto.CUSTO1)).ToString("###,###,##0.00");
                }

                var compraProdutoPgto = desenvController.ObterComprasProdutoPrePedidoPagto(pedidoIntra, produto, cor, entrega);

                int i = 1;
                foreach (var c in compraProdutoPgto)
                {
                    TextBox txtParc = this.FindControl("txtParc" + i.ToString()) as TextBox;
                    txtParc.Text = i.ToString() + "x";

                    TextBox txtPgto = this.FindControl("txtPgto" + i.ToString()) as TextBox;
                    txtPgto.Text = c.DATA_PAGAMENTO.ToString("dd/MM/yyyy");

                    TextBox txtVal = this.FindControl("txtVal" + i.ToString()) as TextBox;
                    txtVal.Text = c.VALOR.ToString("###,###,##0.00");

                    TextBox txtPorc = this.FindControl("txtPorc" + i.ToString()) as TextBox;
                    txtPorc.Text = c.PORC.ToString("##0.00");

                    CheckBox cbPago = this.FindControl("cbPago" + i.ToString()) as CheckBox;
                    cbPago.Checked = (c.DATA_BAIXA == null) ? false : true;

                    i = i + 1;
                }

                var qtdeParcelas = compraProdutoPgto.Count();
                if (qtdeParcelas > 0)
                {
                    ddlParcela.SelectedValue = qtdeParcelas.ToString();
                    ddlParcela_SelectedIndexChanged(ddlParcela, null);
                    RecalcularPagamento();
                }

                hidPedidoIntra.Value = pedidoIntra;
                hidProduto.Value = produto;
                hidCor.Value = cor;
                hidEntrega.Value = entrega.ToString("yyyy-MM-dd");

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        protected void ddlParcela_SelectedIndexChanged(object sender, EventArgs e)
        {
            var qtdeParcela = Convert.ToInt32(ddlParcela.SelectedValue);

            var i = 1;
            while (i <= 8)
            {
                TextBox txtParc = this.FindControl("txtParc" + i.ToString()) as TextBox;
                if (txtParc != null && i > qtdeParcela) { txtParc.Enabled = false; txtParc.Text = ""; }
                if (txtParc != null && i <= qtdeParcela) { txtParc.Text = (i.ToString() + "x"); }
                TextBox txtPgto = this.FindControl("txtPgto" + i.ToString()) as TextBox;
                txtPgto.Enabled = true;
                if (txtPgto != null && i > qtdeParcela) { txtPgto.Enabled = false; txtPgto.Text = ""; }
                TextBox txtVal = this.FindControl("txtVal" + i.ToString()) as TextBox;
                txtVal.Enabled = true;
                if (txtVal != null && i > qtdeParcela) { txtVal.Enabled = false; txtVal.Text = ""; }
                TextBox txtPorc = this.FindControl("txtPorc" + i.ToString()) as TextBox;
                txtPorc.Enabled = true;
                if (txtPorc != null && i > qtdeParcela) { txtPorc.Enabled = false; txtPorc.Text = ""; }
                CheckBox cbPago = this.FindControl("cbPago" + i.ToString()) as CheckBox;
                cbPago.Enabled = true;
                if (cbPago != null && i > qtdeParcela) { cbPago.Enabled = false; cbPago.Checked = false; }

                i = i + 1;
            }

            RecalcularPagamento();
        }

        protected void txtPorc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtPorc = (TextBox)sender;

                if (txtPorc.Text != "")
                {
                    var valorTotal = Convert.ToDecimal(txtValor.Text.Replace("R$", "").Trim());
                    var valorParcela = valorTotal * (Convert.ToDecimal(txtPorc.Text) / 100.00M);

                    TextBox txtValorParcela = this.FindControl(txtPorc.ID.Replace("txtPorc", "txtVal")) as TextBox;
                    txtValorParcela.Text = valorParcela.ToString("###,###,###,##0.00");

                    RecalcularPagamento();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void txtVal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtVal = (TextBox)sender;

                if (txtVal.Text != "")
                {
                    var valorTotal = Convert.ToDecimal(txtValor.Text.Replace("R$", "").Trim());
                    var porcParcela = (Convert.ToDecimal(txtVal.Text) / valorTotal) * 100.00M;

                    TextBox txtPorcParcela = this.FindControl(txtVal.ID.Replace("txtVal", "txtPorc")) as TextBox;
                    txtPorcParcela.Text = porcParcela.ToString("###,##0.00");

                    RecalcularPagamento();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void cbPago_CheckedChanged(object sender, EventArgs e)
        {
            RecalcularPagamento();
        }

        private void RecalcularPagamento()
        {
            var valorTotal = Convert.ToDecimal(txtValor.Text.Replace("R$", "").Trim());
            var valorPago = 0.00M;

            var i = 1;
            while (i <= 8)
            {
                CheckBox cbPago = this.FindControl("cbPago" + i.ToString()) as CheckBox;
                if (cbPago.Checked)
                {
                    TextBox txtVal = this.FindControl("txtVal" + i.ToString()) as TextBox;
                    if (txtVal != null)
                    {
                        if (txtVal.Text != "" && txtVal.Text != "0")
                            valorPago = valorPago + Convert.ToDecimal(txtVal.Text);
                    }

                }

                i = i + 1;
            }

            var valorFalt = (valorTotal - valorPago);
            txtValorTotalFal.Text = "R$ " + valorFalt.ToString("###,###,###,##0.00");
            txtValorTotalPago.Text = "R$ " + valorPago.ToString("###,###,###,##0.00");

            if (valorPago > valorTotal)
                txtValorTotalPago.BackColor = Color.Red;
            else
                txtValorTotalPago.BackColor = Color.White;
        }

        private bool ValidarCampos()
        {
            bool retorno = true;

            TextBox txtPgto = null;
            TextBox txtVal = null;
            TextBox txtPorc = null;

            var parcelas = Convert.ToInt32(ddlParcela.SelectedValue);
            var i = 1;
            while (i <= parcelas)
            {
                txtPgto = this.FindControl("txtPgto" + i.ToString()) as TextBox;
                if (txtPgto.Text == "") return false;
                txtVal = this.FindControl("txtVal" + i.ToString()) as TextBox;
                if (txtVal.Text == "") return false;
                txtPorc = this.FindControl("txtPorc" + i.ToString()) as TextBox;
                if (txtPorc.Text == "") return false;

                i = i + 1;
            }

            return retorno;
        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";

                if (ddlParcela.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o número de parcelas.";
                    return;
                }

                var parcelas = Convert.ToInt32(ddlParcela.SelectedValue);

                //validar dados das parcelas
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha todos os campos das " + parcelas.ToString() + " parcelas.";
                    return;
                }

                var pedidoIntra = hidPedidoIntra.Value;
                var produto = hidProduto.Value;
                var cor = hidCor.Value;
                var entrega = Convert.ToDateTime(hidEntrega.Value);

                //excluir parcelas antigas para incluir novas
                var parcelasEx = desenvController.ObterComprasProdutoPrePedidoPagto(pedidoIntra, produto, cor, entrega);
                var i = 1;
                foreach (var par in parcelasEx)
                {
                    desenvController.ExcluirComprasProdutoPrePedidoPagto(par.PEDIDO, par.PRODUTO, par.COR_PRODUTO, par.ENTREGA, i);
                    i = i + 1;
                }


                i = 1;
                while (i <= parcelas)
                {
                    TextBox txtPgto = this.FindControl("txtPgto" + i.ToString()) as TextBox;
                    TextBox txtVal = this.FindControl("txtVal" + i.ToString()) as TextBox;
                    TextBox txtPorc = this.FindControl("txtPorc" + i.ToString()) as TextBox;
                    CheckBox cbPago = this.FindControl("cbPago" + i.ToString()) as CheckBox;

                    var inc = false;
                    var pagtoParcela = desenvController.ObterComprasProdutoPrePedidoPagto(pedidoIntra, produto, cor, entrega, i);
                    if (pagtoParcela == null)
                    {
                        pagtoParcela = new COMPRAS_PRODUTO_PREPEDIDO_PGTO();
                        pagtoParcela.DATA_INCLUSAO = DateTime.Now;
                        inc = true;
                    }

                    pagtoParcela.PEDIDO = pedidoIntra;
                    pagtoParcela.PRODUTO = produto;
                    pagtoParcela.COR_PRODUTO = cor;
                    pagtoParcela.ENTREGA = entrega;
                    pagtoParcela.PARCELA = i;
                    pagtoParcela.VALOR = (txtVal.Text == "") ? 0.00M : Convert.ToDecimal(txtVal.Text);
                    pagtoParcela.PORC = (txtPorc.Text == "") ? 0.00M : Convert.ToDecimal(txtPorc.Text);
                    pagtoParcela.DATA_PAGAMENTO = (txtPgto.Text == "") ? DateTime.Now : Convert.ToDateTime(txtPgto.Text);
                    if (cbPago.Checked)
                        pagtoParcela.DATA_BAIXA = DateTime.Now;
                    else
                        pagtoParcela.DATA_BAIXA = null;
                    pagtoParcela.OBS = "";

                    if (inc)
                        desenvController.InserirComprasProdutoPrePedidoPagto(pagtoParcela);
                    else
                        desenvController.AtualizarComprasProdutoPrePedidoPagto(pagtoParcela);

                    i = i + 1;
                }


                labErro.Text = "Pagamento atualizado com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


    }
}
