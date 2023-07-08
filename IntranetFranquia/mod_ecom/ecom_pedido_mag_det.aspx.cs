using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using Relatorios.mod_ecom.mag;

namespace Relatorios
{
    public partial class ecom_pedido_mag_det : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();

        decimal valPago = 0;
        decimal precoProduto = 0;
        decimal descontoProduto = 0;
        decimal totalProduto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                string pedidoExterno = "";

                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                pedidoExterno = Request.QueryString["p"].ToString();
                hidPedidoExterno.Value = pedidoExterno;

                CarregarDados(pedidoExterno);

            }

            //Evitar duplo clique no botão
            btGerarCupom.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarCupom, null) + ";");

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

        }

        #region "DADOS INICIAIS"
        private void MoverParaAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        #endregion

        private void CarregarDados(string pedidoExterno)
        {
            var pedidoMag = ecomController.ObterPedidoMag(pedidoExterno);

            if (pedidoMag != null)
            {
                if (pedidoMag.FRAUDE)
                {
                    pnlFraude.Visible = true;
                    btFraude.Enabled = false;
                }

                CarregarPedido(pedidoMag);
                CarregarPagto(pedidoMag);
                CarregarEndereco(pedidoMag);
                CarregarProdutos(pedidoMag);
                CarregarPedidosCliente(pedidoMag);
                CarregarHistorico(pedidoMag);

                CarregarPagarme(pedidoMag);

                CarregarEstorno(pedidoMag);

                CarregarCupom(pedidoMag);

                CarregarEnderecoEtiqueta(pedidoMag);

            }
        }

        private void TratarListaNegra(ECOM_PEDIDO_MAG pedidoMag)
        {

            var numeroTel1 = pedidoMag.TELEFONE.Trim().Replace("-", "").Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "").Replace("  ", "").Trim();
            var numeroTel2 = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE1_COBRANCA.Trim().Replace("-", "").Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "").Replace("  ", "").Trim();
            var numeroTel3 = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE2_COBRANCA.Trim().Replace("-", "").Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "").Replace("  ", "").Trim();

            var telefone1 = ecomController.ObterListaNegraPorValor(numeroTel1);
            if (telefone1 != null)
            {
                txtTelefone1WA.BackColor = Color.LightCoral;
                btTelefone1WA.Visible = false;
                btTelefone1WABloq.Visible = false;
            }

            var telefone2 = ecomController.ObterListaNegraPorValor(numeroTel2);
            if (telefone2 != null)
            {
                txtTelefone2WA.BackColor = Color.LightCoral;
                btTelefone2WA.Visible = false;
                btTelefone2WABloq.Visible = false;
            }

            var telefone3 = ecomController.ObterListaNegraPorValor(numeroTel3);
            if (telefone3 != null)
            {
                txtTelefone3WA.BackColor = Color.LightCoral;
                btTelefone3WA.Visible = false;
                btTelefone3WABloq.Visible = false;
            }

        }
        private void CarregarPedido(ECOM_PEDIDO_MAG pedidoMag)
        {

            txtTelefone1WA.Text = pedidoMag.TELEFONE;
            txtTelefone2WA.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE1_COBRANCA;
            txtTelefone3WA.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE2_COBRANCA;

            TratarListaNegra(pedidoMag);

            txtPedido.Text = ObterPedidoLinx(pedidoMag.PEDIDO_EXTERNO);
            txtPedidoExterno.Text = pedidoMag.PEDIDO_EXTERNO;
            txtNomeCliente.Text = pedidoMag.CLIENTE;
            txtData.Text = pedidoMag.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

            txtCPF.Text = pedidoMag.CPF;
            txtEmail.Text = pedidoMag.EMAIL;
            txtSexo.Text = (pedidoMag.SEXO == "M") ? "Masculino" : ((pedidoMag.SEXO == "F") ? "FEMININO" : "-");
            txtDataNascimento.Text = (pedidoMag.DATA_NASCIMENTO == null) ? "-" : Convert.ToDateTime(pedidoMag.DATA_NASCIMENTO).ToString("dd/MM/yyyy");

            txtStatusPedido.Text = ecomController.ObterPedidoMagStatus(pedidoMag.STATUS_MAGENTO).DESCRICAO;

            var ecomPagto = ecomController.ObterPedidoPgto(txtPedido.Text.Trim());
            if (ecomPagto != null)
                txtZerarPonto.Text = (ecomPagto.PONTOS_HANDCLUB == null) ? "0,00" : ecomPagto.PONTOS_HANDCLUB.ToString();

            txtValorSubTotal.Text = "R$ " + Convert.ToDecimal(pedidoMag.VALOR_SUBTOTAL).ToString("###,###,##0.00");
            txtValorFrete.Text = "R$ " + Convert.ToDecimal(pedidoMag.VALOR_FRETE).ToString("###,###,##0.00");
            txtValorDesconto.Text = (pedidoMag.DESCONTO != null && pedidoMag.DESCONTO > 0) ? ("R$ " + Convert.ToDecimal(pedidoMag.DESCONTO).ToString("###,###,##0.00") + " - C: (" + pedidoMag.CUPOM + ")") : "-";
            txtValorTotal.Text = "R$ " + Convert.ToDecimal(((pedidoMag.VALOR_SUBTOTAL + pedidoMag.VALOR_FRETE) - pedidoMag.DESCONTO)).ToString("###,###,##0.00");
        }

        private string ObterPedidoLinx(string pedidoExterno)
        {
            var pedidoVenda = baseController.ObterPedidoPorPedidoExterno(pedidoExterno);
            if (pedidoVenda != null)
                return pedidoVenda.PEDIDO;

            return "-";
        }
        private void CarregarEndereco(ECOM_PEDIDO_MAG pedidoMag)
        {
            //COBRANCA
            txtNomeCob.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.NOME_COBRANCA;
            txtEnderecoCob.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.RUA_COBRANCA;

            txtCidadeCob.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.CIDADE_COBRANCA;
            txtEstadoCob.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.ESTADO_COBRANCA;
            txtCEPCob.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.CEP_COBRANCA;

            txtTel1Cob.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE1_COBRANCA;
            txtTel2Cob.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE2_COBRANCA;

            //ENTREGA
            txtNomeEnt.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.NOME_ENTREGA;
            txtEnderecoEnt.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.RUA_ENTREGA;

            txtCidadeEnt.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.CIDADE_ENTREGA;
            txtEstadoEnt.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.ESTADO_ENTREGA;
            txtCEPEnt.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.CEP_ENTREGA;

            txtTel1Ent.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE1_ENTREGA;
            txtTel2Ent.Text = pedidoMag.ECOM_PEDIDO_MAG_ENDERECO.TELEFONE2_ENTREGA;
        }
        private void CarregarPagto(ECOM_PEDIDO_MAG pedidoMag)
        {
            var pgto = ecomController.ObterPedidoMagPagto(pedidoMag.PEDIDO_EXTERNO);
            if (pgto != null)
            {
                txtMetodo.Text = (pgto.METODO.ToLower().Contains("bol") || pgto.METODO.ToLower().Contains("billet")) ? "BOLETO" : ((pgto.METODO.ToLower().Contains("free")) ? "SEM COBRANÇA" : "CARTÃO DE CRÉDITO");
                txtValorPago.Text = (pgto.VALOR_PGTO_TOT == null) ? "-" : "R$ " + pgto.VALOR_PGTO_TOT.ToString("###,###,##0.00");

                txtNomeCartao.Text = (pgto.NOME_CARTAO == null) ? "-" : pgto.NOME_CARTAO;
                txtBandeira.Text = (pgto.BANDEIRA == null) ? "-" : pgto.BANDEIRA.ToUpper();
                txtUltDigito.Text = (pgto.ULT_DIG_CARTAO == null) ? "-" : pgto.ULT_DIG_CARTAO;
            }
        }
        private void CarregarProdutos(ECOM_PEDIDO_MAG pedidoMag)
        {
            gvProdutos.DataSource = ecomController.ObterPedidoMagProduto(pedidoMag.PEDIDO_EXTERNO);
            gvProdutos.DataBind();
        }
        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_PEDIDO_MAG_PRODUTO produto = e.Row.DataItem as ECOM_PEDIDO_MAG_PRODUTO;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    var foto = ecomController.ObterMagentoProdutoId(produto.ID_PRODUTO_MAG);
                    if (foto != null)
                        _imgProduto.ImageUrl = foto.FOTO_FRENTE_CAB;

                    Literal litPreco = e.Row.FindControl("litPreco") as Literal;
                    litPreco.Text = "R$ " + Convert.ToDecimal(produto.PRECO).ToString("###,###,###,##0.00");

                    Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                    litDesconto.Text = "R$ " + Convert.ToDecimal(produto.DESCONTO).ToString("###,###,###,##0.00");

                    Literal litTotal = e.Row.FindControl("litTotal") as Literal;
                    litTotal.Text = "R$ " + Convert.ToDecimal(produto.PRECO - produto.DESCONTO).ToString("###,###,###,##0.00");

                    precoProduto += Convert.ToDecimal(produto.PRECO);
                    descontoProduto += Convert.ToDecimal(produto.DESCONTO);
                    totalProduto += Convert.ToDecimal((produto.PRECO - produto.DESCONTO));

                }
            }
        }
        protected void gvProdutos_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProdutos.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[5].Text = "R$ " + precoProduto.ToString("###,###,###0.00");
                _footer.Cells[6].Text = "R$ " + descontoProduto.ToString("###,###,###0.00");
                _footer.Cells[7].Text = "R$ " + totalProduto.ToString("###,###,###0.00");
            }
        }

        private void CarregarHistorico(ECOM_PEDIDO_MAG pedidoMag)
        {
            gvHistorico.DataSource = ecomController.ObterPedidoMagHistorico(pedidoMag.PEDIDO_EXTERNO);
            gvHistorico.DataBind();
        }
        protected void gvHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_PEDIDO_MAG_HIST hist = e.Row.DataItem as ECOM_PEDIDO_MAG_HIST;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = hist.DATA.ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litStatus = e.Row.FindControl("litStatus") as Literal;
                    litStatus.Text = (hist.STATUS_MAGENTO == null || hist.STATUS_MAGENTO == "") ? "" : ecomController.ObterPedidoMagStatus(hist.STATUS_MAGENTO).DESCRICAO;

                    Literal litComentario = e.Row.FindControl("litComentario") as Literal;
                    litComentario.Text = hist.COMENTARIO;
                }
            }
        }

        private void CarregarPagarme(ECOM_PEDIDO_MAG pedidoMag)
        {
            var pagarME = ecomController.ObterPedidoMagPagarme(pedidoMag.PEDIDO_EXTERNO);
            if (pagarME != null)
            {
                txtPMIdTransacao.Text = pagarME.ID.ToString();
                txtPMTID.Text = pagarME.TID.ToString();
                txtPMData.Text = pagarME.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

                txtPMValorPagar.Text = (pagarME.VALOR_A_PAGAR == null) ? "" : pagarME.VALOR_A_PAGAR.ToString();
                txtPMValorPago.Text = (pagarME.VALOR_PAGO == null) ? "" : pagarME.VALOR_PAGO.ToString();
                txtPMValorEstorno.Text = (pagarME.VALOR_ESTORNO == null) ? "" : pagarME.VALOR_ESTORNO.ToString();

                txtPMStatus.Text = pagarME.ECOM_PEDIDO_PAGARME_STA.MOTIVO;

                var statusRazao = "-";
                if (pagarME.STATUS_RAZAO == "antifraud")
                    statusRazao = "Antifraude";
                else if (pagarME.STATUS_RAZAO == "acquirer")
                    statusRazao = "-";
                txtPMStatusRazao.Text = statusRazao;

                var statusRecusa = "-";
                if (pagarME.RECUSA_RAZAO == "antifraud")
                    statusRecusa = "Antifraude";
                txtPMRecusa.Text = statusRecusa;

                txtPMBoletoURL.Text = pagarME.BOLETO_URL;
                txtPMBoletoCODBarra.Text = pagarME.BOLETO_COD_BARRA;
                txtPMBoletoVencimento.Text = (pagarME.BOLETO_EXPIRACAO == null) ? "-" : Convert.ToDateTime(pagarME.BOLETO_EXPIRACAO).ToString("dd/MM/yyyy");

                txtPMCCNome.Text = pagarME.CC_NOME;
                txtPMCCNumero.Text = pagarME.CC_NUMERO;
                txtPMCCParcela.Text = (pagarME.PARCELAS == null) ? "-" : pagarME.PARCELAS.ToString();
                txtPMCCBandeira.Text = pagarME.CC_BANDEIRA;

            }
        }

        private void CarregarPedidosCliente(ECOM_PEDIDO_MAG pedidoMag)
        {
            var pedidosCliente = ecomController.ObterPedidoMagPorClienteAtacado(pedidoMag.CLIENTE_ATACADO).Where(p => p.PEDIDO_EXTERNO != pedidoMag.PEDIDO_EXTERNO);

            gvPedidoCliente.DataSource = pedidosCliente;
            gvPedidoCliente.DataBind();
        }
        protected void gvPedidoCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_PEDIDO_MAG pedido = e.Row.DataItem as ECOM_PEDIDO_MAG;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = pedido.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litMetodoPgto = e.Row.FindControl("litMetodoPgto") as Literal;
                    var pgto = ecomController.ObterPedidoMagPagto(pedido.PEDIDO_EXTERNO);
                    if (pgto != null)
                    {
                        var metodo = (pgto.METODO.ToLower().Contains("bol") || pgto.METODO.ToLower().Contains("billet")) ? "Boleto" : "Cartão de Crédito";
                        litMetodoPgto.Text = metodo;
                    }

                    Literal litValPago = e.Row.FindControl("litValPago") as Literal;
                    litValPago.Text = "R$ " + pedido.VALOR_PAGO.ToString("###,###,###,##0.00");

                    Literal litStatus = e.Row.FindControl("litStatus") as Literal;
                    litStatus.Text = ecomController.ObterPedidoMagStatus(pedido.STATUS_MAGENTO).DESCRICAO;

                    ImageButton btPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                    btPesquisar.CommandArgument = pedido.PEDIDO_EXTERNO;

                    valPago += pedido.VALOR_PAGO;

                    if (pedido.STATUS_MAGENTO == "canceled")
                        e.Row.BackColor = Color.LightCoral;
                }
            }
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPedidoCliente_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPedidoCliente.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[4].Text = "R$ " + valPago.ToString("###,###,###0.00");
            }
        }
        protected void btPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                ImageButton bt = (ImageButton)sender;
                string pedidoExterno = bt.CommandArgument;
                var link = "ecom_pedido_mag_det.aspx?p=" + pedidoExterno.Trim();
                Response.Redirect(link);
            }
            catch (Exception)
            {
            }
        }

        protected void btTelefoneWA_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            var numero = "55";
            var nome = txtNomeCliente.Text.Trim().ToLower();

            if (bt.ID.Contains("1"))
                numero = numero + txtTelefone1WA.Text.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("  ", "").Trim();
            else if (bt.ID.Contains("2"))
                numero = numero + txtTelefone2WA.Text.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("  ", "").Trim();
            else if (bt.ID.Contains("3"))
                numero = numero + txtTelefone3WA.Text.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("  ", "").Trim();

            var nomes = nome.Split(' ');
            if (nomes.Length > 0)
            {
                nome = nomes[0];
                nome = Utils.WebControls.AlterarPrimeiraLetraMaiscula(nome);
            }
            else
            {
                nome = "";
            }

            if (numero.Length == 13) //é celular
                Response.Redirect("https://api.whatsapp.com/send?phone=" + numero + "&text=Oi " + nome, "_blank", null);
        }
        protected void btTelefoneWABloq_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            var numero = "";

            if (bt.ID.Contains("1"))
            {
                numero = txtTelefone1WA.Text.Trim().Replace("-", "").Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "").Replace("  ", "").Trim();
                txtTelefone1WA.BackColor = Color.LightCoral;
                btTelefone1WA.Visible = false;
            }
            else if (bt.ID.Contains("2"))
            {
                numero = txtTelefone2WA.Text.Trim().Replace("-", "").Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "").Replace("  ", "").Trim();
                txtTelefone2WA.BackColor = Color.LightCoral;
                btTelefone2WA.Visible = false;
            }
            else if (bt.ID.Contains("3"))
            {
                numero = txtTelefone3WA.Text.Trim().Replace("-", "").Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "").Replace("  ", "").Trim();
                txtTelefone3WA.BackColor = Color.LightCoral;
                btTelefone3WA.Visible = false;
            }

            var ln = new ECOM_LISTANEGRA { VALOR = numero };
            ecomController.InserirListaNegra(ln);

            var link = "ecom_pedido_mag_det.aspx?p=" + hidPedidoExterno.Value.Trim();
            Response.Redirect(link);

        }
        protected void btFraude_Click(object sender, ImageClickEventArgs e)
        {
            var pedidoMag = ecomController.ObterPedidoMag(hidPedidoExterno.Value);
            if (pedidoMag != null)
            {
                pedidoMag.FRAUDE = true;
                ecomController.AtualizarPedidoMag(pedidoMag);

                //adicionar cliente no blacklist
                var bl = new ECOM_LISTANEGRA { VALOR = pedidoMag.CLIENTE_ATACADO };
                ecomController.InserirListaNegra(bl);
                bl = new ECOM_LISTANEGRA { VALOR = pedidoMag.EMAIL };
                ecomController.InserirListaNegra(bl);
                bl = new ECOM_LISTANEGRA { VALOR = pedidoMag.CPF };
                ecomController.InserirListaNegra(bl);

                var link = "ecom_pedido_mag_det.aspx?p=" + hidPedidoExterno.Value.Trim();
                Response.Redirect(link);
            }
        }
        protected void btZerarHC_Click(object sender, ImageClickEventArgs e)
        {
            if (txtZerarPonto.Text.Trim() != "")
            {
                var ecomPagto = ecomController.ObterPedidoPgto(txtPedido.Text.Trim());
                if (ecomPagto != null)
                {
                    ecomPagto.PONTOS_HANDCLUB = Convert.ToDecimal(txtZerarPonto.Text.Trim());
                    ecomController.AtualizarPedidoPgto(ecomPagto);
                    txtZerarPonto.BackColor = Color.PaleGreen;
                }
            }
        }

        private void CarregarEstorno(ECOM_PEDIDO_MAG pedidoMag)
        {

            txtEstornoSubTotal.Text = "R$ " + Convert.ToDecimal(pedidoMag.VALOR_SUBTOTAL).ToString("###,###,##0.00");
            txtEstornoFrete.Text = "R$ " + Convert.ToDecimal(pedidoMag.VALOR_FRETE).ToString("###,###,##0.00");
            txtEstornoDesconto.Text = (pedidoMag.DESCONTO != null && pedidoMag.DESCONTO > 0) ? ("R$ " + Convert.ToDecimal(pedidoMag.DESCONTO).ToString("###,###,##0.00") + " - C: (" + pedidoMag.CUPOM + ")") : "-";
            txtEstornoTotal.Text = "R$ " + Convert.ToDecimal(((pedidoMag.VALOR_SUBTOTAL + pedidoMag.VALOR_FRETE) - pedidoMag.DESCONTO)).ToString("###,###,##0.00");

            btImprimir.Enabled = false;
            btEnviarEmail.Enabled = false;

            //if (pedidoMag.STATUS_MAGENTO == "closed")
            //    btEstorno.Enabled = false;


            var magEstorno = ecomController.ObterPedidoMagEstorno(pedidoMag.PEDIDO_EXTERNO);
            if (magEstorno != null)
            {
                txtEstornoValor.Text = magEstorno.VALOR_REEMBOLSO.ToString();
                txtEstornoValorAjuste.Text = "R$ " + magEstorno.AJUSTE_MANUTENCAO.ToString();
                txtEstornoValReembolsado.Text = "R$ " + magEstorno.VALOR_REEMBOLSO.ToString();
                txtEstornoNome.Text = magEstorno.NOME;
                txtEstornoCPF.Text = magEstorno.CPF;
                txtEstornoTipoConta.Text = magEstorno.TIPO_CONTA;
                txtEstornoBanco.Text = magEstorno.BANCO;
                txtEstornoAgencia.Text = magEstorno.AGENCIA;
                txtEstornoNumeroConta.Text = magEstorno.CONTA;
                txtEstornoMotivo.Text = magEstorno.MOTIVO;

                btImprimir.Enabled = true;
                btEnviarEmail.Enabled = true;
            }
            else
            {
                txtEstornoNome.Text = pedidoMag.CLIENTE;
                txtEstornoCPF.Text = pedidoMag.CPF;
            }
        }
        protected void txtEstornoValor_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (txtEstornoValor.Text.Trim() != "")
                {
                    var valorEstorno = Convert.ToDecimal(txtEstornoValor.Text.Replace("R$ ", ""));
                    var valorTotal = Convert.ToDecimal(txtEstornoTotal.Text.Replace("R$ ", ""));

                    var valorAjuste = valorTotal - valorEstorno;

                    txtEstornoValorAjuste.Text = "R$ " + valorAjuste.ToString("###,###,##0.00");
                    txtEstornoValReembolsado.Text = "R$ " + valorEstorno.ToString("###,###,##0.00");
                }
            }
            catch (Exception)
            {
                txtEstornoValor.Text = "Erro";
            }

            MoverParaAba("3");
        }

        protected void btEstorno_Click(object sender, EventArgs e)
        {
            try
            {
                labErroEstorno.Text = "";

                var boleto = (txtMetodo.Text.Trim().ToLower() == "boleto") ? true : false;

                if (boleto)
                {
                    if (txtEstornoNome.Text.Trim() == "")
                    {
                        labErroEstorno.Text = "Informe o Nome do cliente.";
                        MoverParaAba("3");
                        return;
                    }
                    if (txtEstornoCPF.Text.Trim() == "")
                    {
                        labErroEstorno.Text = "Informe o CPF do cliente.";
                        MoverParaAba("3");
                        return;
                    }
                    if (txtEstornoTipoConta.Text.Trim() == "")
                    {
                        labErroEstorno.Text = "Informe o Tipo de Conta.";
                        MoverParaAba("3");
                        return;
                    }
                    if (txtEstornoBanco.Text.Trim() == "")
                    {
                        labErroEstorno.Text = "Informe o Banco do cliente.";
                        MoverParaAba("3");
                        return;
                    }
                    if (txtEstornoAgencia.Text.Trim() == "")
                    {
                        labErroEstorno.Text = "Informe a Agência do cliente.";
                        MoverParaAba("3");
                        return;
                    }
                    if (txtEstornoNumeroConta.Text.Trim() == "")
                    {
                        labErroEstorno.Text = "Informe o Número da Conta do cliente.";
                        MoverParaAba("3");
                        return;
                    }
                }

                if (txtEstornoValor.Text.Trim() == "")
                {
                    labErroEstorno.Text = "Informe o Valor do Estorno.";
                    MoverParaAba("3");
                    return;
                }

                if (Convert.ToDecimal(txtEstornoValor.Text.Replace("R$ ", "").Trim()) > Convert.ToDecimal(txtEstornoTotal.Text.Replace("R$ ", "").Trim()))
                {
                    labErroEstorno.Text = "Valor do Estorno não pode ser maios que o Valor Total.";
                    MoverParaAba("3");
                    return;
                }

                if (txtEstornoMotivo.Text.Trim() == "")
                {
                    labErroEstorno.Text = "Informe o Motivo do Estorno.";
                    MoverParaAba("3");
                    return;
                }

                var magEstorno = ecomController.ObterPedidoMagEstorno(txtPedidoExterno.Text.Trim());
                if (magEstorno == null)
                {
                    var estorno = new ECOM_PEDIDO_MAG_ESTORNO();
                    estorno.PEDIDO_EXTERNO = txtPedidoExterno.Text.Trim();
                    estorno.SUBTOTAL = Convert.ToDecimal(txtEstornoSubTotal.Text.Replace("R$ ", ""));
                    estorno.FRETE = Convert.ToDecimal(txtEstornoFrete.Text.Replace("R$ ", ""));
                    estorno.AJUSTE_MANUTENCAO = Convert.ToDecimal(txtEstornoValorAjuste.Text.Replace("R$ ", ""));
                    estorno.VALOR_REEMBOLSO = Convert.ToDecimal(txtEstornoValor.Text.Replace("R$ ", ""));
                    estorno.NOME = txtEstornoNome.Text;
                    estorno.CPF = txtEstornoCPF.Text;
                    estorno.BANCO = txtEstornoBanco.Text;
                    estorno.AGENCIA = txtEstornoAgencia.Text;
                    estorno.CONTA = txtEstornoNumeroConta.Text;
                    estorno.TIPO_CONTA = txtEstornoTipoConta.Text;
                    estorno.DATA_SOLICITACAO = DateTime.Now;
                    estorno.MOTIVO = txtEstornoMotivo.Text.Trim();

                    ecomController.InserirPedidoMagEstorno(estorno);
                }
                else
                {
                    magEstorno.AJUSTE_MANUTENCAO = Convert.ToDecimal(txtEstornoValorAjuste.Text.Replace("R$ ", ""));
                    magEstorno.VALOR_REEMBOLSO = Convert.ToDecimal(txtEstornoValor.Text.Replace("R$ ", ""));
                    magEstorno.NOME = txtEstornoNome.Text;
                    magEstorno.CPF = txtEstornoCPF.Text;
                    magEstorno.BANCO = txtEstornoBanco.Text;
                    magEstorno.AGENCIA = txtEstornoAgencia.Text;
                    magEstorno.CONTA = txtEstornoNumeroConta.Text;
                    magEstorno.TIPO_CONTA = txtEstornoTipoConta.Text;
                    magEstorno.MOTIVO = txtEstornoMotivo.Text.Trim();
                    ecomController.AtualizarPedidoMagEstorno(magEstorno);

                }

                labErroEstorno.Text = "Solicitação atualizada com sucesso.";
                btImprimir.Enabled = true;
                btEnviarEmail.Enabled = true;

            }
            catch (Exception ex)
            {
                labErroEstorno.Text = ex.Message;
            }

            MoverParaAba("3");
        }
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErroEstorno.Text = "";


                var pedidoMag = ecomController.ObterPedidoMag(txtPedidoExterno.Text.Trim());
                if (pedidoMag != null && pedidoMag.ECOM_PEDIDO_MAG_ESTORNO != null)
                {
                    GerarRelatorio(pedidoMag);
                }

            }
            catch (Exception ex)
            {
                labErroEstorno.Text = ex.Message;
            }

            MoverParaAba("3");
        }
        private void GerarRelatorio(ECOM_PEDIDO_MAG pedidoMag)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PEDIDOMAG_ESTORNO" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(pedidoMag));
                wr.Flush();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(ECOM_PEDIDO_MAG pedidoMag)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarFormulario(pedidoMag);

            return _texto;
        }
        private StringBuilder MontarFormulario(ECOM_PEDIDO_MAG pedidoMag)
        {
            StringBuilder _texto = new StringBuilder();
            _texto.AppendLine("");
            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Estorno Ecommerce - Handbook Online</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload='window.print();'>");
            _texto.AppendLine("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("        background: white; white-space: nowrap;'>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <div id='divHB' align='left'>");
            _texto.AppendLine("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                background: white; white-space: nowrap;'>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <h2>Solicitação de Estorno Ecommerce - Handbook Online</h2>");
            _texto.AppendLine("                        <h3>" + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.DATA_SOLICITACAO + "</h3>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                            background: white; white-space: nowrap;'>");
            _texto.AppendLine("                            <tr style='text-align: left;'>");
            _texto.AppendLine("                                <td style='width: 235px;'>");
            _texto.AppendLine("                                    Motivo do Estorno:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.MOTIVO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;<font size='3'>Informações do Pedido</font>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='line-height: 20px;'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                            background: white; white-space: nowrap;'>");
            _texto.AppendLine("                            <tr style='text-align: left;'>");
            _texto.AppendLine("                                <td style='width: 235px;'>");
            _texto.AppendLine("                                    Número do Pedido:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.PEDIDO_EXTERNO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Nome do Cliente:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                   " + pedidoMag.CLIENTE);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Data da Compra:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.DATA_VENDA.ToString("dd/MM/yyyy"));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Valor da Compra:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    R$ " + pedidoMag.VALOR_PAGO.ToString("###,###,##0.00"));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Valor a ser Estornado:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    R$ " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.VALOR_REEMBOLSO.ToString("###,###,##0.00"));
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Valor Ajuste de Manutenção (Magento):");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    <font color='red'>R$ " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.AJUSTE_MANUTENCAO + "</font>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            var pedidoPgto = ecomController.ObterPedidoPgtoPorPedidoExterno(pedidoMag.PEDIDO_EXTERNO);
            if (pedidoPgto != null)
            {
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Operadora:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    " + pedidoPgto.ADQUIRENTE);
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Método de Pagamento:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    " + ((pedidoPgto.BANDEIRA.ToLower() == "boleto") ? "Boleto" : ("Cartão de Crédito - " + pedidoPgto.BANDEIRA)));
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    TID:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    " + pedidoPgto.TID);
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
            }
            else
            {
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Operadora:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Não encontrado");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Método de Pagamento:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Não encontrado");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    TID:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Não encontrado");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
            }

            var faturamento = ecomController.ObterEcomNFporPedido(txtPedido.Text.Trim());
            if (faturamento != null)
            {
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    NF Saída:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    " + faturamento.NF_SAIDA);
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
                _texto.AppendLine("                            <tr>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    Data de Emissão:");
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                                <td>");
                _texto.AppendLine("                                    " + Convert.ToDateTime(faturamento.EMISSAO).ToString("dd/MM/yyyy"));
                _texto.AppendLine("                                </td>");
                _texto.AppendLine("                            </tr>");
            }
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
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        &nbsp;<font size='3'>Dados Bancários</font>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("                            background: white; white-space: nowrap;'>");
            _texto.AppendLine("                            <tr style='text-align: left;'>");
            _texto.AppendLine("                                <td style='width: 235px;'>");
            _texto.AppendLine("                                    Nome:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.NOME);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    CPF:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.CPF);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Banco:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.BANCO);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Agência:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.AGENCIA);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Número da Conta:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.CONTA);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    Tipo de Conta:");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td>");
            _texto.AppendLine("                                    " + pedidoMag.ECOM_PEDIDO_MAG_ESTORNO.TIPO_CONTA);
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("                <tr>");
            _texto.AppendLine("                    <td style='text-align:center;'>");
            _texto.AppendLine("                        <hr />");
            _texto.AppendLine("                    </td>");
            _texto.AppendLine("                </tr>");
            _texto.AppendLine("            </table>");
            _texto.AppendLine("        </div>");
            _texto.AppendLine("        <br />");
            _texto.AppendLine("        <br />");

            var nomeUsuario = "Intranet";
            var usuario = (USUARIO)Session["USUARIO"];
            if (usuario != null)
                nomeUsuario = usuario.NOME_USUARIO;

            _texto.AppendLine("        <span>Enviado por: " + usuario.NOME_USUARIO + "</span>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");

            return _texto;
        }

        protected void btEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                labErroEstorno.Text = "";

                var pedidoMag = ecomController.ObterPedidoMag(txtPedidoExterno.Text.Trim());
                if (pedidoMag != null && pedidoMag.ECOM_PEDIDO_MAG_ESTORNO != null)
                {
                    EnviarEmail(pedidoMag);
                    labErroEstorno.Text = "E-mail enviado com sucesso.";
                }

            }
            catch (Exception ex)
            {
                labErroEstorno.Text = ex.Message;
            }

            MoverParaAba("3");
        }
        private void EnviarEmail(ECOM_PEDIDO_MAG pedidoMag)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            string assunto = "";

            assunto = "Handbook Online: Estorno Pedido " + pedidoMag.PEDIDO_EXTERNO;
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarFormulario(pedidoMag).ToString();

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(71, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }

        protected void btAbrirGoogleMaps1_Click(object sender, ImageClickEventArgs e)
        {
            var url = "https://www.google.com/maps/place/";
            var enderero = txtEnderecoCob.Text.Trim().Replace("  ", " ").Replace(" ", "+").Trim();
            url = url + enderero;
            Response.Redirect(url, "_blank", null);
        }
        protected void btAbrirGoogleMaps2_Click(object sender, ImageClickEventArgs e)
        {
            var url = "https://www.google.com/maps/place/";
            var enderero = txtEnderecoEnt.Text.Trim().Replace("  ", " ").Replace(" ", "+").Trim();
            url = url + enderero;
            Response.Redirect(url, "_blank", null);
        }

        private void CarregarCupom(ECOM_PEDIDO_MAG pedidoMag)
        {
            var pedidoCupom = ecomController.ObterPedidoCupom(pedidoMag.PEDIDO_EXTERNO);
            gvCupom.DataSource = pedidoCupom;
            gvCupom.DataBind();
        }
        protected void btGerarCupom_Click(object sender, EventArgs e)
        {
            try
            {
                labCupomErro.Text = "";

                if (ddlCupomTipo.SelectedValue == "")
                {
                    labCupomErro.Text = "Selecione o tipo de cupom.";
                    MoverParaAba("4");
                    return;
                }

                if (ddlFreteGratis.SelectedValue == "")
                {
                    labCupomErro.Text = "Selecione o tipo de frete.";
                    MoverParaAba("4");
                    return;
                }

                if (txtCupomVal.Text == "")
                {
                    labCupomErro.Text = "Informe o valor do cupom.";
                    MoverParaAba("4");
                    return;
                }

                if (txtNFEntrada.Text == "")
                {
                    labCupomErro.Text = "Informe o Número da NF Entrada.";
                    MoverParaAba("4");
                    return;
                }

                var valorCupom = Convert.ToDecimal(txtCupomVal.Text);

                if (valorCupom <= 0)
                {
                    labCupomErro.Text = "Informe o valor do cupom maior que zero.";
                    MoverParaAba("4");
                    return;
                }

                var cupomAUX = "";
                var cupom = "";
                if (ddlCupomTipo.SelectedValue == "T")
                {
                    cupomAUX = "D" + ObterAuxCupom() + "-" + txtNomeCliente.Text.Trim().Split(' ')[0].ToUpper().Substring(0, 3);
                    cupom = cupomAUX + "-" + "$" + valorCupom.ToString().Replace(",", "").Replace(".", "");
                }
                else if (ddlCupomTipo.SelectedValue == "J")
                {
                    cupomAUX = "J" + ObterAuxCupom() + "-" + txtNomeCliente.Text.Trim().Split(' ')[0].ToUpper().Substring(0, 3);
                    cupom = cupomAUX + "-" + "$" + valorCupom.ToString().Replace(",", "").Replace(".", "");
                }
                else if (ddlCupomTipo.SelectedValue == "H")
                {
                    cupomAUX = "HC" + ObterAuxCupom() + "-" + txtNomeCliente.Text.Trim().Split(' ')[0].ToUpper().Substring(0, 3);
                    cupom = cupomAUX + "-" + "$" + valorCupom.ToString().Replace(",", "").Replace(".", "");
                }

                var freteGratis = (ddlFreteGratis.SelectedValue == "1") ? true : false;

                Magento mag = new Magento();
                var idCupom = mag.GerarCupom(cupom, valorCupom, freteGratis);

                if (idCupom > 0)
                {
                    var pedidoCupom = new ECOM_PEDIDO_MAG_CUPOM();
                    pedidoCupom.PEDIDO_EXTERNO = txtPedidoExterno.Text.Trim();
                    pedidoCupom.CUPOM = cupom;
                    pedidoCupom.FRETE_GRATIS = freteGratis;
                    pedidoCupom.VALOR = valorCupom;
                    pedidoCupom.DATA_CRIACAO = DateTime.Now;
                    pedidoCupom.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    pedidoCupom.ID_CUPOM = idCupom;
                    pedidoCupom.NF_ENTRADA = txtNFEntrada.Text.Trim();

                    ecomController.InserirPedidoCupom(pedidoCupom);

                    var pedidoMag = ecomController.ObterPedidoMag(pedidoCupom.PEDIDO_EXTERNO);
                    CarregarCupom(pedidoMag);
                    labCupomErro.Text = "Cupom criado com sucesso: " + cupom;
                }
                else { labCupomErro.Text = "Erro ao gerar cupom"; }
            }
            catch (Exception ex)
            {
                labCupomErro.Text = ex.Message;
            }
            finally
            {
                MoverParaAba("4");
            }

        }
        private string ObterAuxCupom()
        {
            Random randNum = new Random();
            var res = randNum.Next(10, 99);
            var dia = DateTime.Now.Day;
            var trackMonth = (dia < 10) ? ("0" + dia.ToString()) : dia.ToString();
            return res.ToString() + trackMonth;
        }
        protected void gvCupom_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_PEDIDO_MAG_CUPOM cupom = e.Row.DataItem as ECOM_PEDIDO_MAG_CUPOM;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = cupom.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litValCupom = e.Row.FindControl("litValCupom") as Literal;
                    litValCupom.Text = cupom.VALOR.ToString();

                    Literal litFreteGratis = e.Row.FindControl("litFreteGratis") as Literal;
                    litFreteGratis.Text = ((cupom.FRETE_GRATIS == true) ? "Sim" : ((cupom.FRETE_GRATIS == null) ? "-" : "Não"));

                    Literal litDataExc = e.Row.FindControl("litDataExc") as Literal;
                    litDataExc.Text = (cupom.DATA_EXCLUSAO == null) ? "-" : Convert.ToDateTime(cupom.DATA_EXCLUSAO).ToString("dd/MM/yyyy HH:mm:ss");

                    ImageButton btExcluirCupom = e.Row.FindControl("btExcluirCupom") as ImageButton;
                    btExcluirCupom.CommandArgument = cupom.CUPOM.ToString();
                }
            }
        }
        protected void gvCupom_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCupom.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "";
            }
        }
        protected void btExcluirCupom_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labCupomErro.Text = "";

                ImageButton bt = (ImageButton)sender;
                var cupom = bt.CommandArgument;

                var pedidoCupom = ecomController.ObterPedidoCupomPorCodigo(txtPedidoExterno.Text, cupom);
                if (pedidoCupom != null)
                {
                    //excluir do magento
                    Magento mag = new Magento();
                    mag.ExcluirCupom(pedidoCupom.ID_CUPOM.ToString());

                    //excluir da intranet
                    pedidoCupom.DATA_EXCLUSAO = DateTime.Now;
                    pedidoCupom.MOTIVO = "";
                    ecomController.AtualizarPedidoCupom(pedidoCupom);

                    //recarregar grid
                    var pedidoMag = ecomController.ObterPedidoMag(txtPedidoExterno.Text.Trim());
                    CarregarCupom(pedidoMag);
                    labCupomErro.Text = "Cupom excluído com sucesso.";
                }
                else { labCupomErro.Text = "Cupom não foi encontrado."; }
            }
            catch (Exception ex)
            {
                labCupomErro.Text = ex.Message;
            }

            MoverParaAba("4");
        }

        private void CarregarEnderecoEtiqueta(ECOM_PEDIDO_MAG pedidoMag)
        {
            var endEtiq = ecomController.ObterPedidoEnderecoEtiqueta(pedidoMag.PEDIDO_EXTERNO);
            if (endEtiq != null)
            {
                txtEtiCEP.Text = endEtiq.CEP;
                txtEtiEndereco.Text = endEtiq.ENDERECO;
                txtEtiNumero.Text = endEtiq.NUMERO;
                txtEtiComplemento.Text = endEtiq.COMPLEMENTO;
                txtEtiBairro.Text = endEtiq.BAIRRO;
                txtEtiCidade.Text = endEtiq.CIDADE;
                txtEtiUF.Text = endEtiq.UF;
            }

            if (pedidoMag.STATUS_MAGENTO == "" ||
                pedidoMag.STATUS_MAGENTO == "complete" ||
                pedidoMag.STATUS_MAGENTO == "closed" ||
                pedidoMag.STATUS_MAGENTO == "complete_shipped" ||
                pedidoMag.STATUS_MAGENTO == "canceled" ||
                pedidoMag.STATUS_MAGENTO == "shipment_exception")
            {
                txtEtiCEP.Enabled = false;
                txtEtiEndereco.Enabled = false;
                txtEtiNumero.Enabled = false;
                txtEtiComplemento.Enabled = false;
                txtEtiBairro.Enabled = false;
                txtEtiCidade.Enabled = false;
                txtEtiUF.Enabled = false;
            }
        }
        protected void btSalvarEndEti_Click(object sender, EventArgs e)
        {
            try
            {
                labErroEndEti.Text = "";
                if (txtEtiCEP.Text.Trim() == "")
                {
                    labErroEndEti.Text = "Informe o CEP do endereço da Etiqueta.";
                    MoverParaAba("1");
                    return;
                }
                if (txtEtiEndereco.Text.Trim() == "")
                {
                    labErroEndEti.Text = "Informe o Endreço da Etiqueta.";
                    MoverParaAba("1");
                    return;
                }

                if (txtEtiNumero.Text.Trim() == "")
                {
                    labErroEndEti.Text = "Informe o Número do endereço da Etiqueta.";
                    MoverParaAba("1");
                    return;
                }

                if (txtEtiBairro.Text.Trim() == "")
                {
                    labErroEndEti.Text = "Informe o Bairro do endereço da Etiqueta.";
                    MoverParaAba("1");
                    return;
                }

                if (txtEtiCidade.Text.Trim() == "")
                {
                    labErroEndEti.Text = "Informe o Cidade do endereço da Etiqueta.";
                    MoverParaAba("1");
                    return;
                }

                if (txtEtiUF.Text.Trim() == "")
                {
                    labErroEndEti.Text = "Informe o UF do endereço da Etiqueta.";
                    MoverParaAba("1");
                    return;
                }

                var endEtiq = ecomController.ObterPedidoEnderecoEtiqueta(txtPedidoExterno.Text.Trim());
                if (endEtiq != null)
                {
                    endEtiq.CEP = txtEtiCEP.Text.Trim();
                    endEtiq.ENDERECO = txtEtiEndereco.Text.Trim();
                    endEtiq.NUMERO = txtEtiNumero.Text.Trim();
                    endEtiq.COMPLEMENTO = txtEtiComplemento.Text.Trim();
                    endEtiq.BAIRRO = txtEtiBairro.Text.Trim();
                    endEtiq.CIDADE = txtEtiCidade.Text.Trim();
                    endEtiq.UF = txtEtiUF.Text.Trim();
                    endEtiq.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    endEtiq.DATA_ALTERACAO = DateTime.Now;
                    ecomController.AtualizarPedidoEnderecoEtiqueta(endEtiq);
                    labErroEndEti.Text = "Endereço da Etiqueta atualizado com sucesso.";
                }
            }
            catch (Exception ex)
            {
                labErroEndEti.Text = ex.Message;
            }
            finally
            {
                MoverParaAba("1");
            }
        }
    }
}
