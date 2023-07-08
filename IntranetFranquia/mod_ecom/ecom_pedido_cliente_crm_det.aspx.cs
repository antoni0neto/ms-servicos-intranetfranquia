using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;


namespace Relatorios
{
    public partial class ecom_pedido_cliente_crm_det : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        EcomController ecomController = new EcomController();
        LojaController lojaController = new LojaController();

        int totQtdePedido = 0;
        decimal totValSubtotal = 0;
        decimal totValFrete = 0;
        decimal totValDesconto = 0;
        decimal totValorPago = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"];


                var clienteAtacado = Request.QueryString["ca"];
                if (clienteAtacado != null && clienteAtacado.ToString() != "")
                {
                    hidClienteAtacado.Value = clienteAtacado;

                    ObterPrimeiroNome(clienteAtacado);

                    CarregarDados(clienteAtacado);
                    CarregarHistoricoClienteCRM(clienteAtacado);
                    CarregarUltimasCompras(clienteAtacado);
                    CarregarMaisCompra(clienteAtacado);
                }
            }

            //Evitar duplo clique no botão
            btEnviarSMS.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviarSMS, null) + ";");

        }

        private void ObterPrimeiroNome(string clienteAtacado)
        {
            var primeiroNome = "Handlover";
            try
            {
                var arrayNome = clienteAtacado.ToLower().Trim().Split(' ');
                if (arrayNome != null && arrayNome.Count() > 0)
                {
                    primeiroNome = arrayNome[0].Trim();
                    primeiroNome = Utils.WebControls.AlterarPrimeiraLetraMaiscula(primeiroNome);
                }
            }
            catch (Exception)
            {
                primeiroNome = "Handlover";
            }

            txtSMS.Text = "Oi " + primeiroNome;
            labNomeClienteTit1.Text = primeiroNome;
            labNomeClienteTit2.Text = primeiroNome;
        }

        private void CarregarDados(string clienteAtacado)
        {
            var dados = ecomController.ObterEcomClienteCRMDados(clienteAtacado);
            if (dados != null)
            {
                txtClienteAtacado.Text = dados.CLIENTE_ATACADO;
                txtCPF.Text = dados.CPF;
                txtCliente.Text = dados.CLIENTE;
                txtEmail.Text = dados.EMAIL;
                txtQtdePedido.Text = dados.QTDE_PEDIDO.ToString();
                txtValorTotal.Text = (dados.VALOR_PAGO == null) ? "-" : "R$ " + Convert.ToDecimal(dados.VALOR_PAGO).ToString("###,###,###,##0.00");

                txtTelefone1WA.Text = dados.TELEFONE1;
                txtTelefone2WA.Text = dados.TELEFONE2;
                txtTelefone3WA.Text = dados.TELEFONE3;

                TratarListaNegra(dados);
            }
        }
        private void TratarListaNegra(SP_OBTER_ECOM_CLIENTE_CRM_DADOSResult dados)
        {
            var telefone1 = ecomController.ObterListaNegraPorValor(dados.TELEFONE1);
            if (telefone1 != null)
            {
                txtTelefone1WA.BackColor = Color.LightCoral;
                btTelefone1WA.Visible = false;
                btTelefone1WABloq.Visible = false;
            }

            var telefone2 = ecomController.ObterListaNegraPorValor(dados.TELEFONE2);
            if (telefone2 != null)
            {
                txtTelefone2WA.BackColor = Color.LightCoral;
                btTelefone2WA.Visible = false;
                btTelefone2WABloq.Visible = false;
            }

            var telefone3 = ecomController.ObterListaNegraPorValor(dados.TELEFONE3);
            if (telefone3 != null)
            {
                txtTelefone3WA.BackColor = Color.LightCoral;
                btTelefone3WA.Visible = false;
                btTelefone3WABloq.Visible = false;
            }

        }

        protected void btEnviarSMS_Click(object sender, EventArgs e)
        {

            try
            {

                var numero = "55";
                var nome = hidClienteAtacado.Value.Trim().ToLower();

                labSMS.Text = "";
                var sms = txtSMS.Text.Trim();

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


                // pega numero 1
                numero = numero + txtTelefone1WA.Text.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("  ", "").Trim();
                if (numero.Length != 13) //nao é celular?
                {
                    //pega numero 2
                    numero = numero + txtTelefone2WA.Text.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("  ", "").Trim();
                    if (numero.Length != 13) //nao é celular?
                    {
                        //pega numero 3
                        numero = numero + txtTelefone3WA.Text.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("  ", "").Trim();
                    }
                }

                if (numero.Length != 13) //nao é celular?
                {
                    labSMS.Text = "Nenhum número de celular encontrado.";
                    return;
                }

                if (sms == "")
                {
                    labSMS.Text = "Informe uma mensagem para envio.";
                    return;
                }

                if (sms.Length > 150)
                {
                    labSMS.Text = "Tamanho máximo permitido 150 caracteres.";
                    return;
                }

                sms = RemoverAcentos(sms);

                var resp = lojaController.EnviarSMSHandclubHist(numero, sms);
                labSMS.Text = resp;
                if (!resp.ToLower().Contains("erro"))
                    btEnviarSMS.Enabled = false;

            }
            catch (Exception ex)
            {
                labSMS.Text = ex.Message;
            }
        }
        private string RemoverAcentos(string sms)
        {
            return RemoverAcento(sms);
        }

        public string RemoverAcento(string text)
        {
            if (text == null)
                return "";

            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        protected void btTelefoneWA_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            var numero = "55";
            var nome = hidClienteAtacado.Value.Trim().ToLower();

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
                numero = txtTelefone1WA.Text.Trim();
                txtTelefone1WA.BackColor = Color.LightCoral;
                btTelefone1WA.Visible = false;
            }
            else if (bt.ID.Contains("2"))
            {
                numero = txtTelefone2WA.Text.Trim();
                txtTelefone2WA.BackColor = Color.LightCoral;
                btTelefone2WA.Visible = false;
            }
            else if (bt.ID.Contains("3"))
            {
                numero = txtTelefone3WA.Text.Trim();
                txtTelefone3WA.BackColor = Color.LightCoral;
                btTelefone3WA.Visible = false;
            }

            var ln = new ECOM_LISTANEGRA { VALOR = numero };
            ecomController.InserirListaNegra(ln);

        }

        private void CarregarUltimasCompras(string clienteAtacado)
        {
            var ultimasCompras = ecomController.ObterEcomClienteCRMUltimasCompras(clienteAtacado);

            if (ultimasCompras != null && ultimasCompras.Count() > 0)
            {
                if (ultimasCompras.Count() > 0) { imgProduto1.ImageUrl = ultimasCompras[0].FOTO_FRENTE_CAB; imgProduto1.Visible = true; labProduto1.Text = ultimasCompras[0].PRODUTO + " - " + ultimasCompras[0].NOME; }
                if (ultimasCompras.Count() > 1) { imgProduto2.ImageUrl = ultimasCompras[1].FOTO_FRENTE_CAB; imgProduto2.Visible = true; labProduto2.Text = ultimasCompras[1].PRODUTO + " - " + ultimasCompras[1].NOME; }
                if (ultimasCompras.Count() > 2) { imgProduto3.ImageUrl = ultimasCompras[2].FOTO_FRENTE_CAB; imgProduto3.Visible = true; labProduto3.Text = ultimasCompras[2].PRODUTO + " - " + ultimasCompras[2].NOME; }
                if (ultimasCompras.Count() > 3) { imgProduto4.ImageUrl = ultimasCompras[3].FOTO_FRENTE_CAB; imgProduto4.Visible = true; labProduto4.Text = ultimasCompras[3].PRODUTO + " - " + ultimasCompras[3].NOME; }
                if (ultimasCompras.Count() > 4) { imgProduto5.ImageUrl = ultimasCompras[4].FOTO_FRENTE_CAB; imgProduto5.Visible = true; labProduto5.Text = ultimasCompras[4].PRODUTO + " - " + ultimasCompras[4].NOME; }
                if (ultimasCompras.Count() > 5) { imgProduto6.ImageUrl = ultimasCompras[5].FOTO_FRENTE_CAB; imgProduto6.Visible = true; labProduto6.Text = ultimasCompras[5].PRODUTO + " - " + ultimasCompras[5].NOME; }
            }

        }
        private void CarregarMaisCompra(string clienteAtacado)
        {
            var maisCompra = ecomController.ObterEcomClienteCRMMaisCompra(clienteAtacado);
            gvMaisCompra.DataSource = maisCompra;
            gvMaisCompra.DataBind();
        }

        private void CarregarHistoricoClienteCRM(string clienteAtacado)
        {
            var hist = ecomController.ObterEcomClienteCRMDET(clienteAtacado);
            gvHistCliente.DataSource = hist;
            gvHistCliente.DataBind();
        }

        protected void gvHistCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_CLIENTE_CRM_DETResult cliCRM = e.Row.DataItem as SP_OBTER_ECOM_CLIENTE_CRM_DETResult;

                    Literal litValorSubTotal = e.Row.FindControl("litValorSubTotal") as Literal;
                    litValorSubTotal.Text = Convert.ToDecimal(cliCRM.VALOR_SUBTOTAL).ToString("###,###,###,##0.00");

                    Literal litValorFrete = e.Row.FindControl("litValorFrete") as Literal;
                    litValorFrete.Text = Convert.ToDecimal(cliCRM.VALOR_FRETE).ToString("###,###,###,##0.00");

                    Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                    litDesconto.Text = Convert.ToDecimal(cliCRM.DESCONTO).ToString("###,###,###,##0.00");

                    Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                    litValorPago.Text = Convert.ToDecimal(cliCRM.VALOR_PAGO).ToString("###,###,###,##0.00");

                    totQtdePedido += Convert.ToInt32(cliCRM.TOT_PEDIDO);
                    totValSubtotal += Convert.ToDecimal(cliCRM.VALOR_SUBTOTAL);
                    totValFrete += Convert.ToDecimal(cliCRM.VALOR_FRETE);
                    totValDesconto += Convert.ToDecimal(cliCRM.DESCONTO);
                    totValorPago += Convert.ToDecimal(cliCRM.VALOR_PAGO);

                    //Historico
                    var cliCRMPROD = ecomController.ObterEcomClienteCRMDETPROD(cliCRM.CLIENTE_ATACADO, Convert.ToInt32(cliCRM.ANO), Convert.ToInt32(cliCRM.MES));
                    if (cliCRMPROD != null && cliCRMPROD.Count() > 0)
                    {
                        GridView gvHistClienteProduto = e.Row.FindControl("gvHistClienteProduto") as GridView;
                        gvHistClienteProduto.DataSource = cliCRMPROD;
                        gvHistClienteProduto.DataBind();
                    }
                    else
                    {
                        System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                        if (img != null)
                            img.Visible = false;
                    }


                }
            }
        }
        protected void gvHistCliente_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvHistCliente.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";

                footer.Cells[4].Text = totQtdePedido.ToString();
                footer.Cells[5].Text = totValSubtotal.ToString("###,###,###,##0.00");
                footer.Cells[6].Text = totValFrete.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = totValDesconto.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = totValorPago.ToString("###,###,###,##0.00");
            }
        }

        protected void gvHistClienteProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_CLIENTE_CRM_DET_PRODResult prod = e.Row.DataItem as SP_OBTER_ECOM_CLIENTE_CRM_DET_PRODResult;

                    System.Web.UI.WebControls.ImageButton _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.ImageButton;
                    _imgProduto.ImageUrl = prod.FOTO_FRENTE_CAB;

                    Literal litPreco = e.Row.FindControl("litPreco") as Literal;
                    litPreco.Text = "R$ " + Convert.ToDecimal(prod.PRECO).ToString("###,###,###,##0.00");

                    Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                    litDesconto.Text = "R$ " + Convert.ToDecimal(prod.DESCONTO).ToString("###,###,###,##0.00");

                    Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                    litValTotal.Text = "R$ " + Convert.ToDecimal(prod.VALOR_TOTAL).ToString("###,###,###,##0.00");

                }
            }
        }
        protected void gvHistClienteProduto_DataBound(object sender, EventArgs e)
        {

        }

        protected void gvMaisCompra_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_CLIENTE_CRM_MAISCOMPRAResult prod = e.Row.DataItem as SP_OBTER_ECOM_CLIENTE_CRM_MAISCOMPRAResult;


                }
            }
        }

    }
}
