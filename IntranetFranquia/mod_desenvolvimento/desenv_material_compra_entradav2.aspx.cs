using DAL;
using Relatorios.mod_desenvolvimento.relatorios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_material_compra_entradav2 : System.Web.UI.Page
    {

        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdeRecebida = 0;
        decimal qtdeDevolvida = 0;
        decimal valorNF = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataEntrada.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPrevFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataNota.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataRetirada.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto1.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto2.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto3.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto4.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto5.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPgto6.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                int codigoSubPedido = 0;
                if (Request.QueryString["sub"] == null || Request.QueryString["sub"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                codigoSubPedido = Convert.ToInt32(Request.QueryString["sub"].ToString());
                var subPedido = desenvController.ObterPedidoSub(codigoSubPedido);
                if (subPedido == null)
                    Response.Redirect("desenv_menu.aspx");

                hidCodigoSubPedido.Value = codigoSubPedido.ToString();

                CarregarPedido(subPedido);
                CarregarPagamento(subPedido);
                CarregarEntradas();
                ControleCampos(false);
            }


            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
            btIncluirEntrada.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluirEntrada, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarPedido(DESENV_PEDIDO_SUB subPedido)
        {
            txtNumeroPedido.Text = subPedido.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString() + ((subPedido.ITEM == null) ? "" : (" - " + subPedido.ITEM));
            txtGrupoTecido.Text = subPedido.DESENV_PEDIDO1.GRUPO;
            txtSubGrupoTecido.Text = subPedido.DESENV_PEDIDO1.SUBGRUPO;
            txtCor.Text = prodController.ObterCoresBasicas(subPedido.DESENV_PEDIDO1.COR).DESC_COR;
            txtCorFornecedor.Text = subPedido.DESENV_PEDIDO1.COR_FORNECEDOR;
            txtFornecedor.Text = subPedido.FORNECEDOR;
            CarregarCNPJ(subPedido.FORNECEDOR);
            txtQtdePedido.Text = subPedido.QTDE.ToString();
        }
        private void CarregarCNPJ(string fornecedor)
        {
            var cnpj = baseController.ObterCadastroCLIFOR(fornecedor);
            if (cnpj != null)
                txtCNPJ.Text = cnpj.CGC_CPF.Trim();
        }

        private void ControleCampos(bool devol)
        {
            txtRendimento.Enabled = false;
            txtLargura.Enabled = false;
            txtValorNF.Enabled = false;
            txtDataNota.Enabled = false;
            txtTransportadora.Enabled = false;
            ddlFrete.Enabled = false;
            txtCNPJ.Enabled = false;
            txtNatOpe.Enabled = false;
            txtValProduto.Enabled = false;
            txtPesoBrutoKG.Enabled = false;
            txtPesoLiqKG.Enabled = false;
            txtDataRetirada.Enabled = false;

            if (devol)
            {
                txtDataNota.Enabled = true;
                txtDataRetirada.Enabled = true;
                txtTransportadora.Enabled = true;
                ddlFrete.Enabled = true;
                txtCNPJ.Enabled = true;
                txtNatOpe.Enabled = true;
                txtValProduto.Enabled = true;
                txtPesoBrutoKG.Enabled = true;
                txtPesoLiqKG.Enabled = true;
                txtDataRetirada.Enabled = true;
            }
            else
            {
                txtRendimento.Enabled = true;
                txtLargura.Enabled = true;
                txtValorNF.Enabled = true;
            }
        }

        #endregion

        protected void ddlParcela_SelectedIndexChanged(object sender, EventArgs e)
        {
            var qtdeParcela = Convert.ToInt32(ddlParcela.SelectedValue);

            var i = 1;
            while (i <= 6)
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
        protected void txtNFPgto_TextChanged(object sender, EventArgs e)
        {
            txtNF.Text = txtNFPgto.Text;
        }
        protected void cbPago_CheckedChanged(object sender, EventArgs e)
        {
            RecalcularPagamento();
        }
        private void CarregarPagamento(DESENV_PEDIDO_SUB subPedido)
        {

            txtEntregaIni.Text = (subPedido.DATA_ENTREGA_PREV == null) ? "-" : Convert.ToDateTime(subPedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");
            txtPrevFinal.Text = (subPedido.DATA_PREVISAO_FINAL == null) ? "" : Convert.ToDateTime(subPedido.DATA_PREVISAO_FINAL).ToString("dd/MM/yyyy");
            txtValor.Text = "R$" + Convert.ToDecimal((subPedido.QTDE * subPedido.VALOR)).ToString("###,###,##0.00");

            var subPedidoPgto = desenvController.ObterPedidoSubPgtoPorDesenvPedidoSub(subPedido.CODIGO);

            if (subPedidoPgto != null && subPedidoPgto.Count() > 0)
            {
                if (subPedidoPgto[0].EMISSAO != null)
                    txtEmissao.Text = Convert.ToDateTime(subPedidoPgto[0].EMISSAO).ToString("dd/MM/yyyy");
                txtNFPgto.Text = subPedidoPgto[0].NOTA_FISCAL;
            }

            int i = 1;
            foreach (var c in subPedidoPgto)
            {
                TextBox txtParc = this.FindControl("txtParc" + i.ToString()) as TextBox;
                txtParc.Text = i.ToString() + "x";

                TextBox txtPgto = this.FindControl("txtPgto" + i.ToString()) as TextBox;
                txtPgto.Text = ((c.DATA_PAGAMENTO == null) ? "-" : Convert.ToDateTime(c.DATA_PAGAMENTO).ToString("dd/MM/yyyy"));

                TextBox txtVal = this.FindControl("txtVal" + i.ToString()) as TextBox;
                txtVal.Text = c.VALOR.ToString("###,###,##0.00");

                TextBox txtPorc = this.FindControl("txtPorc" + i.ToString()) as TextBox;
                txtPorc.Text = c.PORC.ToString("##0.00");

                CheckBox cbPago = this.FindControl("cbPago" + i.ToString()) as CheckBox;
                cbPago.Checked = (c.DATA_BAIXA == null) ? false : true;

                i = i + 1;
            }

            var qtdeParcelas = subPedidoPgto.Count;
            if (qtdeParcelas > 0)
            {
                ddlParcela.SelectedValue = qtdeParcelas.ToString();
                ddlParcela_SelectedIndexChanged(ddlParcela, null);
                RecalcularPagamento();
            }
        }
        private void RecalcularPagamento()
        {
            var valorPago = 0.00M;
            var valorTotal = 0.00M;
            var valorTotalPagto = 0.00M;

            var i = 1;
            while (i <= 6)
            {
                TextBox txtVal = this.FindControl("txtVal" + i.ToString()) as TextBox;
                if (txtVal.Text != "" && txtVal.Text != "0")
                    valorTotalPagto = valorTotalPagto + Convert.ToDecimal(txtVal.Text);

                CheckBox cbPago = this.FindControl("cbPago" + i.ToString()) as CheckBox;
                if (cbPago.Checked)
                {
                    if (txtVal.Text != "" && txtVal.Text != "0")
                        valorPago = valorPago + Convert.ToDecimal(txtVal.Text);
                }

                i = i + 1;
            }

            if (valorTotalPagto <= 0)
                valorTotal = Convert.ToDecimal(txtValor.Text.Replace("R$", "").Trim());
            else
                valorTotal = valorTotalPagto;

            var valorFalt = (valorTotal - valorPago);
            txtValorTotalFal.Text = "R$ " + valorFalt.ToString("###,###,###,##0.00");
            txtValorTotalPago.Text = "R$ " + valorPago.ToString("###,###,###,##0.00");

            if (valorPago > valorTotal)
                txtValorTotalPago.BackColor = Color.Red;
            else
                txtValorTotalPago.BackColor = Color.White;
        }
        private bool ValidarCamposPgto()
        {
            bool retorno = true;

            if (ddlParcela.SelectedValue == "" || ddlParcela.SelectedValue == "0")
                return false;

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
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labDataEntrada.ForeColor = _OK;
            if (txtDataEntrada.Text == "")
            {
                labDataEntrada.ForeColor = _notOK;
                retorno = false;
            }

            labQtdeEntrada.ForeColor = _OK;
            if (txtQuantidade.Text == "" || txtQuantidade.Text == "0")
            {
                labQtdeEntrada.ForeColor = _notOK;
                retorno = false;
            }

            labNFEntrada.ForeColor = _OK;
            if (txtNF.Text == "")
            {
                labNFEntrada.ForeColor = _notOK;
                retorno = false;
            }

            labValorNota.ForeColor = _OK;
            if (txtValorNF.Text == "")
            {
                labValorNota.ForeColor = _notOK;
                retorno = false;
            }

            labRendimentoMT.ForeColor = _OK;
            if (txtRendimento.Text == "" || txtRendimento.Text == "0")
            {
                labRendimentoMT.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarCamposDev()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labDataEntrada.ForeColor = _OK;
            if (txtDataEntrada.Text == "")
            {
                labDataEntrada.ForeColor = _notOK;
                retorno = false;
            }

            labQtdeEntrada.ForeColor = _OK;
            if (txtQuantidade.Text == "" || txtQuantidade.Text == "0")
            {
                labQtdeEntrada.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarCamposEmissao()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labDataEntrada.ForeColor = _OK;
            if (txtDataEntrada.Text == "")
            {
                labDataEntrada.ForeColor = _notOK;
                retorno = false;
            }

            labQtdeEntrada.ForeColor = _OK;
            if (txtQuantidade.Text == "" || txtQuantidade.Text == "0")
            {
                labQtdeEntrada.ForeColor = _notOK;
                retorno = false;
            }

            labNFEntrada.ForeColor = _OK;
            if (txtNF.Text == "" || txtNF.Text == "0")
            {
                labNFEntrada.ForeColor = _notOK;
                retorno = false;
            }

            labProdutoFornecedor.ForeColor = _OK;
            if (txtProdutoFornecedor.Text == "")
            {
                labProdutoFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labVolume.ForeColor = _OK;
            if (txtVolume.Text == "")
            {
                labVolume.ForeColor = _notOK;
                retorno = false;
            }

            labDataNota.ForeColor = _OK;
            if (txtDataNota.Text == "")
            {
                labDataNota.ForeColor = _notOK;
                retorno = false;
            }

            labTransportadora.ForeColor = _OK;
            if (txtTransportadora.Text == "")
            {
                labTransportadora.ForeColor = _notOK;
                retorno = false;
            }

            labFrete.ForeColor = _OK;
            if (ddlFrete.SelectedValue == "")
            {
                labFrete.ForeColor = _notOK;
                retorno = false;
            }

            labCNPJ.ForeColor = _OK;
            if (txtCNPJ.Text == "")
            {
                labCNPJ.ForeColor = _notOK;
                retorno = false;
            }

            labNatOpe.ForeColor = _OK;
            if (txtNatOpe.Text == "")
            {
                labNatOpe.ForeColor = _notOK;
                retorno = false;
            }

            labPrecoProduto.ForeColor = _OK;
            if (txtValProduto.Text == "")
            {
                labPrecoProduto.ForeColor = _notOK;
                retorno = false;
            }

            labPesoBrutoKG.ForeColor = _OK;
            if (txtPesoBrutoKG.Text == "")
            {
                labPesoBrutoKG.ForeColor = _notOK;
                retorno = false;
            }

            labPesoLiqKG.ForeColor = _OK;
            if (txtPesoLiqKG.Text == "")
            {
                labPesoLiqKG.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";


                if (!ValidarCamposPgto())
                {
                    labErro.Text = "Informe os campos de pagamento.";
                    return;
                }

                var codigoSubPedido = Convert.ToInt32(hidCodigoSubPedido.Value);

                //Atualizar pagamento
                if (!SalvarPagamento(codigoSubPedido))
                {
                    labErro.Text = "Erro ao salvar pagamento. Verifique os campos informados...";
                    return;
                }

                //atualizar previsao final
                var subPedido = desenvController.ObterPedidoSub(codigoSubPedido);
                subPedido.DATA_PREVISAO_FINAL = Convert.ToDateTime(txtPrevFinal.Text);
                desenvController.AtualizarPedidoSub(subPedido);

                labErro.Text = "Pagamento atualizado com sucesso.";

                CarregarEntradas();

                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private bool SalvarPagamento(int codigoSubPedido)
        {
            bool ret = true;

            var parcelas = Convert.ToInt32(ddlParcela.SelectedValue);
            //excluir parcelas antigas para incluir novas
            var parcelasEx = desenvController.ObterPedidoSubPgtoPorDesenvPedidoSub(codigoSubPedido);
            var i = 1;
            foreach (var par in parcelasEx)
            {
                desenvController.ExcluirPedidoSubPgto(par.DESENV_PEDIDO_SUB, i);
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
                var pagtoParcela = desenvController.ObterPedidoSubPgtoPorParcela(codigoSubPedido, i);
                if (pagtoParcela == null)
                {
                    pagtoParcela = new DESENV_PEDIDO_SUB_PGTO();
                    pagtoParcela.DATA_INCLUSAO = DateTime.Now;
                    inc = true;
                }

                pagtoParcela.DESENV_PEDIDO_SUB = codigoSubPedido;
                pagtoParcela.PARCELA = i;
                pagtoParcela.VALOR = (txtVal.Text == "") ? 0.00M : Convert.ToDecimal(txtVal.Text);
                pagtoParcela.PORC = (txtPorc.Text == "") ? 0.00M : Convert.ToDecimal(txtPorc.Text);
                pagtoParcela.DATA_PAGAMENTO = (txtPgto.Text == "") ? DateTime.Now : Convert.ToDateTime(txtPgto.Text);
                if (txtEmissao.Text != "")
                    pagtoParcela.EMISSAO = Convert.ToDateTime(txtEmissao.Text);
                pagtoParcela.NOTA_FISCAL = txtNFPgto.Text;

                if (cbPago.Checked)
                    pagtoParcela.DATA_BAIXA = DateTime.Now;
                else
                    pagtoParcela.DATA_BAIXA = null;
                pagtoParcela.OBS = "";

                if (inc)
                    desenvController.InserirPedidoSubPgto(pagtoParcela);
                else
                    desenvController.AtualizarPedidoSubPgto(pagtoParcela);

                i = i + 1;
            }


            return ret;
        }


        private void CarregarEntradas()
        {
            var entradas = desenvController.ObterPedidoQtdePedidoPorSub(Convert.ToInt32(hidCodigoSubPedido.Value));

            gvEntrada.DataSource = entradas.Where(p => p.QTDE > 0);
            gvEntrada.DataBind();

            gvDevolucao.DataSource = entradas.Where(p => p.QTDE < 0);
            gvDevolucao.DataBind();
        }
        protected void gvEntrada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_QTDE pedidoQtde = e.Row.DataItem as DESENV_PEDIDO_QTDE;

                    if (pedidoQtde != null)
                    {
                        Label labDataEntrada = e.Row.FindControl("labDataEntrada") as Label;
                        labDataEntrada.Text = pedidoQtde.DATA.ToString("dd/MM/yyyy");

                        Label labValorNota = e.Row.FindControl("labValorNota") as Label;
                        labValorNota.Text = (pedidoQtde.VALOR_NOTA == null) ? "-" : Convert.ToDecimal(pedidoQtde.VALOR_NOTA).ToString("###,###,###,##0.00");

                        Label labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                        labQtdeEntregue.Text = pedidoQtde.QTDE.ToString("###,###,###,##0.000");

                        Label labRendimentoMT = e.Row.FindControl("labRendimentoMT") as Label;
                        labRendimentoMT.Text = (pedidoQtde.RENDIMENTO_MT == null) ? "-" : pedidoQtde.RENDIMENTO_MT.ToString();

                        ImageButton btImprimir = e.Row.FindControl("btImprimir") as ImageButton;
                        btImprimir.CommandArgument = pedidoQtde.CODIGO.ToString();

                        ImageButton btEditarEntrada = e.Row.FindControl("btEditarEntrada") as ImageButton;
                        btEditarEntrada.CommandArgument = pedidoQtde.CODIGO.ToString();

                        ImageButton btExcluirEntrada = e.Row.FindControl("btExcluirEntrada") as ImageButton;
                        btExcluirEntrada.CommandArgument = pedidoQtde.CODIGO.ToString();

                        qtdeRecebida += pedidoQtde.QTDE;
                        valorNF += ((pedidoQtde.VALOR_NOTA == null) ? 0 : Convert.ToDecimal(pedidoQtde.VALOR_NOTA));
                    }
                }
            }
        }
        protected void gvEntrada_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvEntrada.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[2].Text = qtdeRecebida.ToString("###,###,###,##0.000");
                footer.Cells[5].Text = "R$ " + valorNF.ToString("###,###,##0.00");

                txtQtdeRecebimento.Text = (qtdeRecebida + qtdeDevolvida).ToString();
            }
        }
        protected void gvDevolucao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_QTDE pedidoQtde = e.Row.DataItem as DESENV_PEDIDO_QTDE;

                    if (pedidoQtde != null)
                    {
                        Label labDataEntrada = e.Row.FindControl("labDataEntrada") as Label;
                        labDataEntrada.Text = pedidoQtde.DATA.ToString("dd/MM/yyyy");

                        Label labEmissao = e.Row.FindControl("labEmissao") as Label;
                        labEmissao.Text = (pedidoQtde.EMISSAO == null) ? "-" : Convert.ToDateTime(pedidoQtde.EMISSAO).ToString("dd/MM/yyyy");

                        Label labQtdeDev = e.Row.FindControl("labQtdeDev") as Label;
                        labQtdeDev.Text = pedidoQtde.QTDE.ToString("###,###,###,##0.000");

                        Label labNFDev = e.Row.FindControl("labNFDev") as Label;
                        labNFDev.Text = pedidoQtde.NF_DEV;

                        Label labDataNota = e.Row.FindControl("labDataNota") as Label;
                        labDataNota.Text = (pedidoQtde.DATA_NOTA == null) ? "-" : Convert.ToDateTime(pedidoQtde.DATA_NOTA).ToString("dd/MM/yyyy");

                        Label labDataRetirada = e.Row.FindControl("labDataRetirada") as Label;
                        labDataRetirada.Text = (pedidoQtde.DATA_RETIRADA == null) ? "-" : Convert.ToDateTime(pedidoQtde.DATA_RETIRADA).ToString("dd/MM/yyyy");

                        ImageButton btEditarDevol = e.Row.FindControl("btEditarDevol") as ImageButton;
                        btEditarDevol.CommandArgument = pedidoQtde.CODIGO.ToString();

                        ImageButton btExcluirEntrada = e.Row.FindControl("btExcluirEntrada") as ImageButton;
                        btExcluirEntrada.CommandArgument = pedidoQtde.CODIGO.ToString();

                        if (pedidoQtde.EMISSAO != null)
                            btExcluirEntrada.Visible = false;
                        if (pedidoQtde.DATA_RETIRADA != null)
                            btEditarDevol.Visible = false;

                        qtdeDevolvida += pedidoQtde.QTDE;
                    }
                }
            }
        }
        protected void gvDevolucao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvDevolucao.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[2].Text = qtdeDevolvida.ToString("###,###,###,##0.000");

                txtQtdeRecebimento.Text = (qtdeRecebida + qtdeDevolvida).ToString();
            }
        }

        protected void btExcluirEntrada_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";
                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    int codigo = 0;
                    codigo = Convert.ToInt32(b.CommandArgument);
                    desenvController.ExcluirPedidoQtde(codigo);
                    CarregarEntradas();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btImprimir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";
                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    int codigo = 0;
                    codigo = Convert.ToInt32(b.CommandArgument);
                    var desenvQtdePedido = desenvController.ObterPedidoQtde(codigo);

                    ImprimirCartelinha(desenvQtdePedido);
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void ImprimirCartelinha(DESENV_PEDIDO_QTDE desenvPedidoQtde)
        {
            StreamWriter wr = null;
            try
            {
                labErro.Text = "";

                string nomeArquivo = "ENT_MAT_CARTE" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(Cartelinha.MontarCartelinha(desenvPedidoQtde));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
            finally
            {
                wr.Close();
            }

        }

        protected void btIncluirEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                labEntrada.Text = "";

                if (txtQuantidade.Text == "" || txtQuantidade.Text == "0")
                {
                    labEntrada.Text = "Informe a quantidade do Material.";
                    return;
                }

                var qtdeMaterial = Convert.ToDecimal(txtQuantidade.Text);

                if (qtdeMaterial > 0)
                {
                    if (!ValidarCampos())
                    {
                        labEntrada.Text = "Informe os campos obrigatórios para recebimento do material.";
                        return;
                    }
                }
                else
                {
                    if (!ValidarCamposDev())
                    {
                        labEntrada.Text = "Informe os campos obrigatórios para devolução do material.";
                        return;
                    }
                }

                var pedidoSub = desenvController.ObterPedidoSub(Convert.ToInt32(hidCodigoSubPedido.Value));
                if (pedidoSub == null)
                {
                    labEntrada.Text = "Pedido não foi encontrado. Tente novamente...";
                    return;
                }

                var inc = true;
                var pedidoQtde = new DESENV_PEDIDO_QTDE();
                if (hidCodigoPedidoQtde.Value != "")
                {
                    pedidoQtde = desenvController.ObterPedidoQtde(Convert.ToInt32(hidCodigoPedidoQtde.Value));
                    if (pedidoQtde != null)
                    {
                        inc = false;

                        if (pedidoQtde.QTDE < 0 && txtDataNota.Text != "")
                            if (!ValidarCamposEmissao())
                            {
                                labEntrada.Text = "Informe os campos obrigatórios para emissão da NF de Devolução.";
                                return;
                            }

                    }
                }

                pedidoQtde.DATA = Convert.ToDateTime(txtDataEntrada.Text);
                pedidoQtde.QTDE = Convert.ToDecimal(txtQuantidade.Text);
                pedidoQtde.NOTA_FISCAL = txtNF.Text.Trim();

                if (txtValorNF.Text != "") pedidoQtde.VALOR_NOTA = Convert.ToDecimal(txtValorNF.Text);
                if (txtRendimento.Text != "") pedidoQtde.RENDIMENTO_MT = Convert.ToDecimal(txtRendimento.Text);
                pedidoQtde.PRODUTO_FORNECEDOR = txtProdutoFornecedor.Text.Trim().ToUpper();
                pedidoQtde.LARGURA = (txtLargura.Text == "") ? 0 : Convert.ToDecimal(txtLargura.Text);
                pedidoQtde.VOLUME = (txtVolume.Text == "") ? 0 : Convert.ToInt32(txtVolume.Text);
                pedidoQtde.DESENV_PEDIDO_SUB = Convert.ToInt32(hidCodigoSubPedido.Value);
                pedidoQtde.DESENV_PEDIDO = pedidoSub.DESENV_PEDIDO;
                pedidoQtde.OBS = txtObs.Text.Trim();

                if (txtDataNota.Text != "") pedidoQtde.DATA_NOTA = Convert.ToDateTime(txtDataNota.Text);

                pedidoQtde.TRANSPORTADORA = txtTransportadora.Text.Trim().ToUpper();
                pedidoQtde.FRETE = ddlFrete.SelectedValue;

                pedidoQtde.CNPJ = txtCNPJ.Text.Trim();
                pedidoQtde.NATUREZA_OPERACAO = txtNatOpe.Text.Trim().ToUpper();
                if (txtValProduto.Text != "") pedidoQtde.VALOR_MATERIAL = Convert.ToDecimal(txtValProduto.Text);
                if (txtPesoBrutoKG.Text != "") pedidoQtde.PESO_KG = Convert.ToDecimal(txtPesoBrutoKG.Text);
                if (txtPesoLiqKG.Text != "") pedidoQtde.PESOLIQ_KG = Convert.ToDecimal(txtPesoLiqKG.Text);

                // limpar para atualizar
                pedidoQtde.VENCIMENTO1 = null;
                pedidoQtde.VENCIMENTO2 = null;
                pedidoQtde.VENCIMENTO3 = null;
                pedidoQtde.VENCIMENTO4 = null;
                pedidoQtde.VENCIMENTO5 = null;
                pedidoQtde.VENCIMENTO6 = null;

                var pedidoPgto = desenvController.ObterPedidoSubPgtoPorDesenvPedidoSub(pedidoSub.CODIGO);
                int i = 1;
                foreach (var p in pedidoPgto)
                {
                    if (i == 1) pedidoQtde.VENCIMENTO1 = p.DATA_PAGAMENTO;
                    if (i == 2) pedidoQtde.VENCIMENTO2 = p.DATA_PAGAMENTO;
                    if (i == 3) pedidoQtde.VENCIMENTO3 = p.DATA_PAGAMENTO;
                    if (i == 4) pedidoQtde.VENCIMENTO4 = p.DATA_PAGAMENTO;
                    if (i == 5) pedidoQtde.VENCIMENTO5 = p.DATA_PAGAMENTO;
                    if (i == 6) pedidoQtde.VENCIMENTO6 = p.DATA_PAGAMENTO;
                    i += 1;
                }

                //email para emissao de nota fiscal de devolucao
                if (pedidoQtde.QTDE < 0 && pedidoQtde.DATA_NOTA != null && pedidoQtde.DATA_IMPRESSAO == null)
                {
                    EnviarEmailEmissaoDEV(pedidoQtde);
                    pedidoQtde.DATA_IMPRESSAO = DateTime.Now;
                }

                //email para informar retirada
                if (pedidoQtde.QTDE < 0 && pedidoQtde.EMISSAO != null && txtDataRetirada.Text != "" && pedidoQtde.DATA_RETIRADA == null)
                {
                    EnviarEmailRetirada(pedidoQtde);
                    pedidoQtde.DATA_RETIRADA = DateTime.Now;
                }

                if (inc)
                    desenvController.InserirPedidoQtde(pedidoQtde);
                else
                    desenvController.AtualizarPedidoQtde(pedidoQtde);

                if (qtdeMaterial > 0)
                    labEntrada.Text = "Entrada de material atualizada com sucesso.";
                else
                    labEntrada.Text = "Devolução de material atualizada com sucesso.";

                btCancelarEntrada_Click(btCancelarEntrada, null);
                CarregarEntradas();
            }
            catch (Exception ex)
            {
                labEntrada.Text = ex.Message;
            }
        }
        protected void btEditarEntrada_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labEntrada.Text = "";

                ImageButton bt = (ImageButton)sender;
                var codigoPedidoQtde = Convert.ToInt32(bt.CommandArgument);

                var pedidoQtde = desenvController.ObterPedidoQtde(codigoPedidoQtde);
                if (pedidoQtde == null)
                {
                    labEntrada.Text = "Entrada de pedido não encontrada.";
                    return;
                }

                //validar click do botao
                var botID = bt.ID.ToLower();
                var devol = false;
                if (botID.Contains("devol"))
                    devol = true;

                txtDataEntrada.Text = pedidoQtde.DATA.ToString("dd/MM/yyyy");
                txtQuantidade.Text = pedidoQtde.QTDE.ToString();
                txtNF.Text = pedidoQtde.NOTA_FISCAL;
                txtValorNF.Text = (pedidoQtde.VALOR_NOTA == null) ? "" : pedidoQtde.VALOR_NOTA.ToString();
                txtProdutoFornecedor.Text = pedidoQtde.PRODUTO_FORNECEDOR;
                txtLargura.Text = (pedidoQtde.LARGURA == null) ? "" : pedidoQtde.LARGURA.ToString();
                txtVolume.Text = (pedidoQtde.VOLUME == null) ? "" : pedidoQtde.VOLUME.ToString();
                txtRendimento.Text = (pedidoQtde.RENDIMENTO_MT == null) ? "" : pedidoQtde.RENDIMENTO_MT.ToString();
                txtObs.Text = pedidoQtde.OBS;

                txtDataNota.Text = (pedidoQtde.DATA_NOTA == null) ? "" : Convert.ToDateTime(pedidoQtde.DATA_NOTA).ToString("dd/MM/yyyy");
                txtDataRetirada.Text = (pedidoQtde.DATA_RETIRADA == null) ? "" : Convert.ToDateTime(pedidoQtde.DATA_RETIRADA).ToString("dd/MM/yyyy");

                txtTransportadora.Text = pedidoQtde.TRANSPORTADORA;
                ddlFrete.SelectedValue = (pedidoQtde.FRETE == null) ? "" : pedidoQtde.FRETE;
                txtCNPJ.Text = pedidoQtde.CNPJ;
                txtNatOpe.Text = pedidoQtde.NATUREZA_OPERACAO;
                txtValProduto.Text = (pedidoQtde.VALOR_MATERIAL == null) ? Convert.ToDecimal(pedidoQtde.DESENV_PEDIDO_SUB1.VALOR).ToString("###,###,###,##0.00") : Convert.ToDecimal(pedidoQtde.VALOR_MATERIAL).ToString("###,###,###,##0.00");
                txtPesoBrutoKG.Text = (pedidoQtde.PESO_KG == null) ? "" : Convert.ToDecimal(pedidoQtde.PESO_KG).ToString("###,###,###,##0.000");
                txtPesoLiqKG.Text = (pedidoQtde.PESOLIQ_KG == null) ? "" : Convert.ToDecimal(pedidoQtde.PESOLIQ_KG).ToString("###,###,###,##0.000");

                CarregarCNPJ(txtFornecedor.Text.Trim());

                hidCodigoPedidoQtde.Value = pedidoQtde.CODIGO.ToString();
                btCancelarEntrada.Visible = true;

                ControleCampos(devol);

            }
            catch (Exception)
            {
            }
        }
        protected void btCancelarEntrada_Click(object sender, EventArgs e)
        {
            txtDataEntrada.Text = "";
            txtQuantidade.Text = "";
            txtNF.Text = "";
            txtValorNF.Text = "";
            txtProdutoFornecedor.Text = "";
            txtLargura.Text = "";
            txtVolume.Text = "";
            txtRendimento.Text = "";
            txtObs.Text = "";
            hidCodigoPedidoQtde.Value = "";
            btCancelarEntrada.Visible = false;

            txtDataNota.Text = "";
            txtDataRetirada.Text = "";

            txtTransportadora.Text = "";
            ddlFrete.SelectedValue = "";
            txtPesoBrutoKG.Text = "";
            txtPesoLiqKG.Text = "";
            txtNatOpe.Text = "";
            txtValProduto.Text = "";

            ControleCampos(false);

        }
        protected void txtQuantidade_TextChanged(object sender, EventArgs e)
        {
            txtQuantidade.BackColor = Color.White;
            if (txtQuantidade.Text != "")
            {
                var qtdePedido = Convert.ToDecimal(txtQtdePedido.Text);
                var qtdeRecebida = (txtQtdeRecebimento.Text == "") ? 0 : Convert.ToDecimal(txtQtdeRecebimento.Text);
                var qtdeDigitada = Convert.ToDecimal(txtQuantidade.Text);
                if (qtdePedido > 0)
                {
                    var qtdeFalta = qtdePedido - qtdeRecebida;
                    if (qtdeDigitada > qtdeFalta)
                        txtQuantidade.BackColor = Color.Coral;
                }
            }
        }

        private void EnviarEmailEmissaoDEV(DESENV_PEDIDO_QTDE pedidoQtde)
        {

            try
            {
                var relDev = MontarDevolucaoTecido(pedidoQtde);

                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();
                var assunto = "Intranet: Emissão NF de Devolução de Tecido: " + pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR.Trim() + " - " + DateTime.Today.ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.MENSAGEM = relDev.ToString();

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(45, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);

                email.DESTINATARIOS = destinatario;

                if (destinatario.Count > 0)
                    email.EnviarEmail();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }
        public StringBuilder MontarDevolucaoTecido(DESENV_PEDIDO_QTDE pedidoQtde)
        {
            StringBuilder _texto = new StringBuilder();

            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Emissao de NF de Devolução de Tecido</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body>");
            _texto.AppendLine("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("        background: white; white-space: nowrap;'>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <div id='divHB' align='left'>");
            _texto.AppendLine("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                background: white; white-space: nowrap;'>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <h2>Emissao de NF de Devolução de Tecido</h2>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='line-height: 20px;'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                            background: white; white-space: nowrap;'>");
            _texto.AppendLine("                            <tr style='text-align: left;'>");
            _texto.AppendLine("                                <td style='width: 235px;'>");
            _texto.AppendLine("                                    Nome do Destinatário:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            string cnpjCPF = pedidoQtde.CNPJ.Trim();
            if (cnpjCPF.Length == 11)
                cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"000\.000\.000\-00");
            else
                cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"00\.000\.000\/0000\-00");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    CNPJ:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + cnpjCPF);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Natureza de Operação:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.NATUREZA_OPERACAO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Produto Fornecedor:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.PRODUTO_FORNECEDOR);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Tecido:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.DESENV_PEDIDO1.SUBGRUPO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Cor/Estampa:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.DESENV_PEDIDO1.COR_FORNECEDOR);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Quantidade:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + (pedidoQtde.QTDE * -1.00M).ToString() + " " + pedidoQtde.DESENV_PEDIDO1.UNIDADE_MEDIDA1.DESCRICAO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Preço do Produto:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    R$ " + ((pedidoQtde.VALOR_MATERIAL == null) ? "N/I" : Convert.ToDecimal(pedidoQtde.VALOR_MATERIAL).ToString("###,###,###,##0.00")));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Volume:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.VOLUME.ToString());
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Peso Líquido:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + ((pedidoQtde.PESOLIQ_KG == null) ? "-" : (pedidoQtde.PESOLIQ_KG.ToString() + " KGs")));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Peso Bruto:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + ((pedidoQtde.PESO_KG == null) ? "-" : (pedidoQtde.PESO_KG.ToString() + " KGs")));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Transportadora:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.TRANSPORTADORA);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Frete:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + ((pedidoQtde.FRETE == null || pedidoQtde.FRETE == "") ? "N/I" : pedidoQtde.FRETE));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    NF Origem: ");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.NOTA_FISCAL);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("            </table>");
            _texto.AppendLine("        </div>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <span>Solicitado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");

            _texto.AppendLine("");
            return _texto;
        }


        private void EnviarEmailRetirada(DESENV_PEDIDO_QTDE pedidoQtde)
        {

            try
            {
                var relDev = MontarRetiradaTecido(pedidoQtde);

                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();
                var assunto = "Intranet: Tecido Entregue para " + pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR.Trim() + " - " + DateTime.Today.ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.MENSAGEM = relDev.ToString();

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(45, 3).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);

                email.DESTINATARIOS = destinatario;

                if (destinatario.Count > 0)
                    email.EnviarEmail();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }
        public StringBuilder MontarRetiradaTecido(DESENV_PEDIDO_QTDE pedidoQtde)
        {
            StringBuilder _texto = new StringBuilder();

            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Retirada de Tecido pelo Fornecedor</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body>");
            _texto.AppendLine("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("        background: white; white-space: nowrap;'>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <div id='divHB' align='left'>");
            _texto.AppendLine("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                background: white; white-space: nowrap;'>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <h2>Retirada de Tecido pelo Fornecedor</h2>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='line-height: 20px;'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                            background: white; white-space: nowrap;'>");
            _texto.AppendLine("                            <tr style='text-align: left;'>");
            _texto.AppendLine("                                <td style='width: 235px;'>");
            _texto.AppendLine("                                    Nome do Destinatário:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            string cnpjCPF = pedidoQtde.CNPJ.Trim();
            if (cnpjCPF.Length == 11)
                cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"000\.000\.000\-00");
            else
                cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"00\.000\.000\/0000\-00");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    CNPJ:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + cnpjCPF);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Natureza de Operação:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.NATUREZA_OPERACAO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Produto Fornecedor:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.PRODUTO_FORNECEDOR);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Tecido:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.DESENV_PEDIDO1.SUBGRUPO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Cor/Estampa:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.DESENV_PEDIDO1.COR_FORNECEDOR);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Quantidade:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + (pedidoQtde.QTDE * -1.00M).ToString() + " " + pedidoQtde.DESENV_PEDIDO1.UNIDADE_MEDIDA1.DESCRICAO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Preço do Produto:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    R$ " + ((pedidoQtde.VALOR_MATERIAL == null) ? "N/I" : Convert.ToDecimal(pedidoQtde.VALOR_MATERIAL).ToString("###,###,###,##0.00")));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Volume:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.VOLUME.ToString());
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Peso Líquido:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + ((pedidoQtde.PESOLIQ_KG == null) ? "-" : (pedidoQtde.PESOLIQ_KG.ToString() + " KGs")));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Peso Bruto:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + ((pedidoQtde.PESO_KG == null) ? "-" : (pedidoQtde.PESO_KG.ToString() + " KGs")));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Transportadora:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.TRANSPORTADORA);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Frete:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + ((pedidoQtde.FRETE == null || pedidoQtde.FRETE == "") ? "N/I" : pedidoQtde.FRETE));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    NF Origem: ");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.NOTA_FISCAL);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    NF Dev: ");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoQtde.NF_DEV);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("            </table>");
            _texto.AppendLine("        </div>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <span>Enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");

            _texto.AppendLine("");
            return _texto;
        }


    }
}
