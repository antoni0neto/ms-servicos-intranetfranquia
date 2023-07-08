using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;


namespace Relatorios
{
    public partial class gest_historico_hand_cliente : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();
        EcomController ecomController = new EcomController();

        decimal totalPontoCampanha = 0;

        decimal totalValorBruto = 0;
        decimal totalValorPago = 0;
        decimal totalPonto = 0;
        decimal totalDesconto = 0;
        decimal totalValorTroca = 0;

        int histTotalVendas = 0;
        decimal histTotalPontoAcumulado = 0;
        decimal histTotalSaldo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();

                if (tela == "2")
                    hrefVoltar.HRef = "gest_menu.aspx";
                else if (tela == "1")
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "../mod_gerenciamento_loja/gerloja_menu.aspx";
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";
                else if (tela == "5")
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";

                var cpf = Request.QueryString["cpf"];
                if (cpf != null && cpf.ToString() != "")
                {
                    txtCPFFiltro.Text = cpf;
                    btBuscar_Click(btBuscar, null);
                }
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btEnviarSMS.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviarSMS, null) + ";");

        }

        private void LimparCampos()
        {
            txtCodigoCliente.Text = "";
            txtCPF.Text = "";
            txtCliente.Text = "";
            txtDDD.Text = "";
            txtTelefone.Text = "";
            txtEmail.Text = "";
            txtSaldoMesAtual.Text = "";
            txtTextoSMS.Text = "";
            txtTextoWhatsApp.Text = "";
            btEnviarSMS.Enabled = true;
            btEnviarWhatsApp.Enabled = true;
            btBloquearTelefone.Enabled = true;
            hidCodigoProgFidelidade.Value = "";

            gvHistHand.DataSource = new List<SP_OBTER_HISTORICO_CLIENTE_HANDResult>();
            gvHistHand.DataBind();

        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                LimparCampos();

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

                CarregarHistoricoClienteHand(cliente.CPF_CGC.Trim());

                var dataIniResgate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var dataFimResgate = dataIniResgate.AddMonths(1).AddDays(-1);
                var saldo = lojaController.ObterSaldoPontos(cliente.CODIGO_CLIENTE, dataIniResgate, dataFimResgate);
                if (saldo != null)
                {
                    txtSaldoMesAtual.Text = saldo.SALDO_PONTOS.ToString("###,###,###,##0.00");
                    hidCodigoProgFidelidade.Value = saldo.COD_FIDELIDADE_PROGRAMA.ToString();
                }
                else
                {
                    txtSaldoMesAtual.Text = "0,00";
                    hidCodigoProgFidelidade.Value = "";
                }

                var primeiroNome = "Handlover";
                var mensagem = "";
                try
                {
                    var arrayNome = cliente.CLIENTE_VAREJO.ToLower().Trim().Split(' ');
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

                if (saldo != null)
                {
                    mensagem = "Handbook: Ei, " + primeiroNome + ", passando aqui rapidinho pra te avisar que voce tem R$" + saldo.SALDO_PONTOS.ToString("###,###,###,##0.00") + " em creditos HANDCLUB para utilizar em uma de nossas lojas ate " + dataFimResgate.ToString("dd-MM-yyyy") + ". Venha resgatar seus pontos!";
                    txtTextoSMS.Text = mensagem;

                    var filial = "Shopping";
                    if (saldo.FILIAL_ULT_MOVTO_PONTUACAO != null)
                    {
                        filial = baseController.ObterFilialIntranet(saldo.FILIAL_ULT_MOVTO_PONTUACAO).SHOPPING.Trim();
                    }

                    var mensagemWhats = "Oi, *" + primeiroNome + "*! " + Environment.NewLine +
                        "A Handbook do " + filial + " passou para avisar que você tem *R$" + saldo.SALDO_PONTOS.ToString("###,###,###,##0.00") + "* de bônus " +
                        "Handclub, pronto para ser utilizado em nossas lojas físicas." + Environment.NewLine +
                        "" + Environment.NewLine +
                        "Esse benefício é válido até dia *" + dataFimResgate.ToString("dd-MM-yyyy") + "*, então não perde tempo. " + Environment.NewLine +
                        "O benefício só pode ser utilizado uma única vez no mês. Esta é uma mensagem automática, caso você não queira mais receber, nos avise enviando ”SAIR”";
                    txtTextoWhatsApp.Text = mensagemWhats;
                }

                TratarListaNegra(cliente.DDD, cliente.TELEFONE);


            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private void TratarListaNegra(string ddd, string telefone)
        {
            if (ddd != "" && telefone != "")
            {
                var numero = Convert.ToInt32(ddd).ToString().Trim() + telefone;
                var telefoneListaNegra = ecomController.ObterListaNegraPorValor(numero);

                btEnviarWhatsApp.Enabled = true;
                btBloquearTelefone.Enabled = true;
                txtTextoSMS.BackColor = Color.Gainsboro;
                txtTextoWhatsApp.BackColor = Color.Gainsboro;

                if (telefoneListaNegra != null)
                {
                    btEnviarSMS.Enabled = false;
                    btEnviarWhatsApp.Enabled = false;
                    btBloquearTelefone.Enabled = false;
                    txtTextoSMS.BackColor = Color.LightCoral;
                    txtTextoWhatsApp.BackColor = Color.LightCoral;
                }
            }
        }
        private void CarregarHistoricoClienteHand(string cpf)
        {
            var hist = lojaController.ObterHistoricoClienteHand(cpf);
            gvHistHand.DataSource = hist;
            gvHistHand.DataBind();
        }

        protected void gvHistHand_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HISTORICO_CLIENTE_HANDResult lv = e.Row.DataItem as SP_OBTER_HISTORICO_CLIENTE_HANDResult;

                    Literal litCodigoPrograma = e.Row.FindControl("litCodigoPrograma") as Literal;
                    litCodigoPrograma.Text = lv.COD_FIDELIDADE_PROGRAMA.ToString();

                    Literal litPrograma = e.Row.FindControl("litPrograma") as Literal;
                    litPrograma.Text = lv.DESC_PROGRAMA.ToString();

                    Literal litTotVenda = e.Row.FindControl("litTotVenda") as Literal;
                    litTotVenda.Text = lv.TOTAL_VENDAS.ToString();

                    Literal litPontosAcumulados = e.Row.FindControl("litPontosAcumulados") as Literal;
                    litPontosAcumulados.Text = Convert.ToDecimal(lv.PONTOS_ACUMULADOS).ToString("###,###,###,##0.00");

                    Literal litSaldo = e.Row.FindControl("litSaldo") as Literal;
                    litSaldo.Text = lv.SALDO_PONTOS.ToString("###,###,###,##0.00");

                    histTotalVendas += Convert.ToInt32(lv.TOTAL_VENDAS);
                    histTotalPontoAcumulado += Convert.ToDecimal(lv.PONTOS_ACUMULADOS);
                    histTotalSaldo += lv.SALDO_PONTOS;

                    //Carregar Dados dos Programas
                    GridView gvPontoAcumulado = e.Row.FindControl("gvPontoAcumulado") as GridView;
                    GridView gvPontoResgatado = e.Row.FindControl("gvPontoResgatado") as GridView;
                    GridView gvCampanha = e.Row.FindControl("gvCampanha") as GridView;
                    TextBox txtSaldoPonto = e.Row.FindControl("txtSaldoPonto") as TextBox;
                    CarregarDadosPrograma(lv.CODIGO_CLIENTE, lv.COD_FIDELIDADE_PROGRAMA, gvPontoAcumulado, gvPontoResgatado, gvCampanha, txtSaldoPonto);



                }
            }
        }
        protected void gvHistHand_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvHistHand.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[4].Text = histTotalVendas.ToString();
                footer.Cells[5].Text = histTotalPontoAcumulado.ToString();
                footer.Cells[6].Text = histTotalSaldo.ToString();
            }
        }

        private void CarregarDadosPrograma(string codigoCliente, int codigoPrograma, GridView gvPontoAcumulado, GridView gvPontoResgatado, GridView gvCampanha, TextBox txtSaldoPonto)
        {

            int ano = Convert.ToInt32(codigoPrograma.ToString().Substring(0, 4));
            int mes = Convert.ToInt32(codigoPrograma.ToString().Substring(4, 2));

            var dataIniResgate = new DateTime(ano, mes, 1).AddMonths(1);
            var dataFimResgate = dataIniResgate.AddMonths(1).AddDays(-1);

            var dataIniAcumulado = dataIniResgate.AddMonths(-1);
            var dataFimAcumulado = dataIniResgate.AddDays(-1);

            totalValorBruto = 0;
            totalValorPago = 0;
            totalPonto = 0;
            totalDesconto = 0;
            totalValorTroca = 0;
            totalPontoCampanha = 0;

            var mesAcumulado = lojaController.ObterVendaPorClienteEData(codigoCliente, dataIniAcumulado, dataFimAcumulado);
            gvPontoAcumulado.DataSource = mesAcumulado;
            gvPontoAcumulado.DataBind();

            totalValorBruto = 0;
            totalValorPago = 0;
            totalPonto = 0;
            totalDesconto = 0;
            totalValorTroca = 0;
            totalPontoCampanha = 0;

            var mesResgatado = lojaController.ObterVendaPorClienteEData(codigoCliente, dataIniResgate, dataFimResgate);
            gvPontoResgatado.DataSource = mesResgatado;
            gvPontoResgatado.DataBind();

            totalValorBruto = 0;
            totalValorPago = 0;
            totalPonto = 0;
            totalDesconto = 0;
            totalValorTroca = 0;
            totalPontoCampanha = 0;

            var pontoCampanha = lojaController.ObterPontosCampanha(codigoCliente, dataIniResgate, dataFimResgate);
            gvCampanha.DataSource = pontoCampanha;
            gvCampanha.DataBind();

            var saldo = lojaController.ObterSaldoPontos(codigoCliente, dataIniResgate, dataFimResgate);
            txtSaldoPonto.Text = "";
            if (saldo != null)
                txtSaldoPonto.Text = saldo.SALDO_PONTOS.ToString();

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

                    Literal litVendedor = e.Row.FindControl("litVendedor") as Literal;
                    litVendedor.Text = baseController.BuscaVendedorPorCodigo(lv.VENDEDOR).NOME_VENDEDOR;

                    Literal litTicket = e.Row.FindControl("litTicket") as Literal;
                    litTicket.Text = lv.TICKET;

                    Literal litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                    litDataVenda.Text = (lv.DATA_DIGITACAO == null) ? Convert.ToDateTime(lv.DATA_VENDA).ToString("dd/MM/yyyy HH:mm:ss") : Convert.ToDateTime(lv.DATA_DIGITACAO).ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                    litValorPago.Text = Convert.ToDecimal(lv.VALOR_PAGO).ToString("###,###,###,##0.00");

                    Literal litPonto = e.Row.FindControl("litPonto") as Literal;
                    if (litPonto != null)
                        litPonto.Text = Convert.ToDecimal(lv.QTDE_PONTOS_ACUMULADOS).ToString("###,###,###,##0.00");

                    Literal litValorBruto = e.Row.FindControl("litValorBruto") as Literal;
                    if (litValorBruto != null)
                        litValorBruto.Text = Convert.ToDecimal(lv.VALOR_VENDA_BRUTA).ToString("###,###,###,##0.00");

                    Literal litValorTroca = e.Row.FindControl("litValorTroca") as Literal;
                    if (litValorTroca != null)
                        litValorTroca.Text = Convert.ToDecimal(lv.VALOR_TROCA).ToString("###,###,###,##0.00");

                    Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                    if (litDesconto != null)
                        litDesconto.Text = Convert.ToDecimal(lv.DESCONTO).ToString("###,###,###,##0.00");

                    if (lv.DATA_HORA_CANCELAMENTO != null)
                    {
                        e.Row.BackColor = Color.LightCoral;
                    }
                    else
                    {
                        totalValorBruto += Convert.ToDecimal(lv.VALOR_VENDA_BRUTA);
                        totalValorPago += Convert.ToDecimal(lv.VALOR_PAGO);
                        totalPonto += Convert.ToDecimal(lv.QTDE_PONTOS_ACUMULADOS);
                        totalDesconto += Convert.ToDecimal(lv.DESCONTO);
                        totalValorTroca += Convert.ToDecimal(lv.VALOR_TROCA);
                    }

                    var histVenda = lojaController.ObterVendaCanceladaHist(lv.CODIGO_FILIAL, lv.TICKET, lv.CODIGO_CLIENTE);
                    if (histVenda != null && histVenda.Count() > 0)
                    {
                        GridView gvHistCancelamento = e.Row.FindControl("gvHistCancelamento") as GridView;
                        gvHistCancelamento.DataSource = histVenda;
                        gvHistCancelamento.DataBind();
                    }

                    Literal litAbrirProduto = e.Row.FindControl("litAbrirProduto") as Literal;
                    var link = "gest_historico_hand_cliente_produto.aspx?t=" + lv.TICKET.Trim() + "&cf=" + lv.CODIGO_FILIAL.Trim() + "&dv=" + lv.DATA_VENDA + "";
                    var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
                    litAbrirProduto.Text = linkOk;

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
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[6].Text = totalValorBruto.ToString();
                footer.Cells[7].Text = totalValorTroca.ToString();
                footer.Cells[8].Text = totalValorPago.ToString();
                footer.Cells[9].Text = totalDesconto.ToString();
            }
        }
        protected void gvPontoAcumulado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[6].Text = totalValorBruto.ToString();
                footer.Cells[7].Text = totalValorTroca.ToString();
                footer.Cells[8].Text = totalValorPago.ToString();
                footer.Cells[9].Text = totalPonto.ToString();
            }
        }
        protected void gvCampanha_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = totalPontoCampanha.ToString();
            }
        }

        protected void gvHistCancelamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HIST_VENDA_CANCELADAResult tkt = e.Row.DataItem as SP_OBTER_HIST_VENDA_CANCELADAResult;

                    if (tkt != null)
                    {
                        Literal litDataEvento = e.Row.FindControl("litDataEvento") as Literal;
                        litDataEvento.Text = Convert.ToDateTime(tkt.DATA_EVENTO).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                }
            }
        }
        protected void gvHistCancelamento_DataBound(object sender, EventArgs e)
        {
        }

        protected void btAbrirProduto_Click(object sender, EventArgs e)
        {
            //"<a href=\"javascript: openwindow('')\" class='adre' title='Janeiro'>NOME</a>";
        }

        protected void btEnviarSMS_Click(object sender, EventArgs e)
        {

            var saldo = txtSaldoMesAtual.Text.Trim();
            var dddTexto = txtDDD.Text.Trim();
            var telefoneTexto = txtTelefone.Text.Trim();

            labSMS.Text = "";

            if (saldo == "")
            {
                labSMS.Text = "Este cliente não possui Saldo no mês Atual.";
                return;
            }

            if (Convert.ToDecimal(saldo) <= 0)
            {
                labSMS.Text = "Este cliente não possui Saldo no mês Atual.";
                return;
            }

            if (dddTexto == "")
            {
                labSMS.Text = "Informe o DDD.";
                return;
            }

            if (Convert.ToInt32(dddTexto).ToString().Length != 2)
            {
                labSMS.Text = "Informe o DDD válido.";
                return;
            }

            if (telefoneTexto == "")
            {
                labSMS.Text = "Informe o Telefone.";
                return;
            }

            if (Convert.ToInt32(telefoneTexto).ToString().Length != 9)
            {
                labSMS.Text = "Informe o Telefone válido.";
                return;
            }

            if (txtTextoSMS.Text.Trim() == "")
            {
                labSMS.Text = "Erro ao gerar mensagem de SMS. Entre em contato com TI.";
                return;
            }


            var resp = lojaController.EnviarSMSHandclubHist(Convert.ToInt32(dddTexto), Convert.ToInt32(telefoneTexto), txtTextoSMS.Text.Trim());
            labSMS.Text = resp;
            if (!resp.ToLower().Contains("erro"))
                btEnviarSMS.Enabled = false;

        }

        protected void btEnviarWhatsApp_Click(object sender, EventArgs e)
        {
            var saldo = txtSaldoMesAtual.Text.Trim();
            var dddTexto = txtDDD.Text.Replace(" ", "").Trim();
            var telefoneTexto = txtTelefone.Text.Replace(" ", "").Trim();

            labWhatsApp.Text = "";

            if (saldo == "")
            {
                labWhatsApp.Text = "Este cliente não possui Saldo no mês Atual.";
                return;
            }

            if (Convert.ToDecimal(saldo) <= 0)
            {
                labWhatsApp.Text = "Este cliente não possui Saldo no mês Atual.";
                return;
            }

            if (dddTexto == "")
            {
                labWhatsApp.Text = "Informe o DDD.";
                return;
            }

            if (Convert.ToInt32(dddTexto).ToString().Length != 2)
            {
                labWhatsApp.Text = "Informe o DDD válido.";
                return;
            }

            if (telefoneTexto == "")
            {
                labWhatsApp.Text = "Informe o Telefone.";
                return;
            }

            if (Convert.ToInt32(telefoneTexto).ToString().Length != 9)
            {
                labWhatsApp.Text = "Informe o Telefone válido.";
                return;
            }

            if (txtTextoWhatsApp.Text.Trim() == "")
            {
                labWhatsApp.Text = "Erro ao gerar mensagem de WhatsApp. Entre em contato com TI.";
                return;
            }

            var numeroAux = Convert.ToInt32(dddTexto).ToString().Trim() + telefoneTexto;
            var telefoneListaNegra = ecomController.ObterListaNegraPorValor(numeroAux);

            if (telefoneListaNegra != null)
            {
                btEnviarSMS.Enabled = false;
                btEnviarWhatsApp.Enabled = false;
                btBloquearTelefone.Enabled = false;
                txtTextoSMS.BackColor = Color.LightCoral;
                txtTextoWhatsApp.BackColor = Color.LightCoral;

                labWhatsApp.Text = "Este número está na Lista Negra. Não é possível enviar mensagem.";
                return;
            }

            if (hidCodigoProgFidelidade.Value != "")
            {
                var wapp = new CLIENTE_WHATSAPP_HC();
                wapp.COD_FIDELIDADE_PROGRAMA = Convert.ToInt32(hidCodigoProgFidelidade.Value);
                wapp.CODIGO_CLIENTE = txtCodigoCliente.Text.Trim();
                wapp.DATA_ENVIO = DateTime.Now;
                wapp.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                lojaController.InserirWhatsAppHC(wapp);
            }
            var numero = "55";
            numero = numero + Convert.ToInt32(dddTexto).ToString() + telefoneTexto.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("  ", "").Trim();
            Response.Redirect("https://api.whatsapp.com/send?phone=" + numero + "&text=", "_blank", null);

        }

        protected void btBloquearTelefone_Click(object sender, EventArgs e)
        {

            if (txtDDD.Text.Trim() != "" && txtTelefone.Text.Trim() != "")
            {
                var ddd = Convert.ToInt32(txtDDD.Text.Trim()).ToString();
                var numero = ddd + txtTelefone.Text.Trim();

                var ln = new ECOM_LISTANEGRA { VALOR = numero };
                ecomController.InserirListaNegra(ln);

                btEnviarWhatsApp.Enabled = false;
                btEnviarSMS.Enabled = false;
                btBloquearTelefone.Enabled = false;
                txtTextoWhatsApp.BackColor = Color.LightCoral;
                txtTextoSMS.BackColor = Color.LightCoral;
            }
        }




    }
}
