using DAL;
using Relatorios.mod_ecom.mag;
using Relatorios.mod_ecomv2.mag2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class crm_cliente_det : System.Web.UI.Page
    {
        CRMController crmController = new CRMController();
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        int qtdeTickets = 0;
        decimal valBruto = 0;
        decimal valFrete = 0;
        decimal valDesconto = 0;
        decimal valPago = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["ca"] == null || Request.QueryString["ca"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("crm_menu.aspx");

                var cpf = Request.QueryString["ca"].ToString();
                CarregarDados(cpf);
                CarregarCRMTipo();

                hidCPF.Value = cpf;
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
            btZerarHandClub.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btZerarHandClub, null) + ";");
            btGerarCupom.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarCupom, null) + ";");
            btCarregarHistorico.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCarregarHistorico, null) + ";");

        }

        private void CarregarCRMTipo()
        {
            var tipos = crmController.ObterCRMTipo();
            tipos.Insert(0, new CRM_TIPO { CODIGO = 0, TIPO = "Selecione" });
            ddlTipoCliente.DataSource = tipos;
            ddlTipoCliente.DataBind();
        }
        private void CarregarDados(string cpf)
        {
            ObterPrimeiroNome(cpf);
            CarregarCliente(cpf);
            CarregarCupom(cpf);
            CarregarAcoes(cpf);
            CarregarUltimasCompras(cpf);
            CarregarMaisCompra(cpf);
            CarregarHistoricoClienteCRM(cpf);
            CarregarHistoricoTipoCliente(cpf);
        }
        private void CarregarSaldoMes(string codigoCliente)
        {
            btZerarHandClub.Enabled = false;
            var dataIniResgate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dataFimResgate = dataIniResgate.AddMonths(1).AddDays(-1);

            var codigoPrograma = lojaController.ObterProgramaHandclub(dataIniResgate, dataFimResgate).COD_FIDELIDADE_PROGRAMA;
            hidCodigoPrograma.Value = codigoPrograma.ToString();

            var saldo = lojaController.ObterSaldoPontos(codigoCliente, dataIniResgate, dataFimResgate);
            if (saldo != null)
            {
                txtSaldoAtual.Text = saldo.SALDO_PONTOS.ToString("###,###,###,##0.00");
                if (saldo.SALDO_PONTOS > 0)
                    btZerarHandClub.Enabled = true;
            }
            else
            {
                txtSaldoAtual.Text = "0,00";
            }
        }

        private void ObterPrimeiroNome(string cpf)
        {
            var primeiroNome = "Handlover";
            try
            {
                var cliente = baseController.BuscaClienteCPF(cpf);
                if (cliente != null && cliente.CLIENTE_VAREJO != null)
                {
                    primeiroNome = cliente.CLIENTE_VAREJO.Trim().ToLower().ToString().Split(' ')[0].ToString();
                    primeiroNome = Utils.WebControls.AlterarPrimeiraLetraMaiscula(primeiroNome);
                }
            }
            catch (Exception)
            {
                primeiroNome = "Handlover";
            }

            labNomeClienteTit1.Text = primeiroNome;
            labNomeClienteTit2.Text = primeiroNome;
        }

        private void CarregarCliente(string cpf)
        {
            var dados = crmController.ObterCRMClienteDET(cpf);
            if (dados != null)
            {
                txtCodigoCliente.Text = dados.CODIGO_CLIENTE;
                txtCPF.Text = dados.CPF;
                txtCliente.Text = dados.CLIENTE;
                txtEmail.Text = dados.EMAIL;
                txtTelefone.Text = dados.TELEFONE;
                txtObs.Text = dados.OBS;
                ddlTipoCliente.SelectedValue = (dados.CODIGO_TIPO_CLIENTE == null) ? "0" : dados.CODIGO_TIPO_CLIENTE.ToString();

                CarregarSaldoMes(dados.CODIGO_CLIENTE);
            }
        }
        protected void btWapp_Click(object sender, ImageClickEventArgs e)
        {
            var numero = txtTelefone.Text.Trim();
            var nome = labNomeClienteTit1.Text;

            if (numero.Length == 13) //é celular
                Response.Redirect("https://api.whatsapp.com/send?phone=" + numero + "&text=Oi " + nome, "_blank", null);
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlTipoCliente.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o tipo de cliente para atualizar.";
                    return;
                }

                var tipoCliente = new CRM_CLIENTE_TIPO();
                tipoCliente.CPF = txtCPF.Text.Trim();
                tipoCliente.CRM_TIPO = Convert.ToInt32(ddlTipoCliente.SelectedValue);
                tipoCliente.DATA = DateTime.Now;
                tipoCliente.OBS = txtObs.Text.Trim();

                crmController.InserirClienteTipoHist(tipoCliente);

                labErro.Text = "Cliente atualizado com sucesso!";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void CarregarUltimasCompras(string cpf)
        {
            var ultimasCompras = crmController.ObterCRMUltimosProdutos(cpf);

            if (ultimasCompras != null && ultimasCompras.Count() > 0)
            {
                if (ultimasCompras.Count() > 0) { imgProduto1.ImageUrl = ultimasCompras[0].FOTO; imgProduto1.Visible = true; labProduto1.Text = ultimasCompras[0].SKU + " - " + ultimasCompras[0].DESC_PRODUTO; }
                if (ultimasCompras.Count() > 1) { imgProduto2.ImageUrl = ultimasCompras[1].FOTO; imgProduto2.Visible = true; labProduto2.Text = ultimasCompras[1].SKU + " - " + ultimasCompras[1].DESC_PRODUTO; }
                if (ultimasCompras.Count() > 2) { imgProduto3.ImageUrl = ultimasCompras[2].FOTO; imgProduto3.Visible = true; labProduto3.Text = ultimasCompras[2].SKU + " - " + ultimasCompras[2].DESC_PRODUTO; }
                if (ultimasCompras.Count() > 3) { imgProduto4.ImageUrl = ultimasCompras[3].FOTO; imgProduto4.Visible = true; labProduto4.Text = ultimasCompras[3].SKU + " - " + ultimasCompras[3].DESC_PRODUTO; }
                if (ultimasCompras.Count() > 4) { imgProduto5.ImageUrl = ultimasCompras[4].FOTO; imgProduto5.Visible = true; labProduto5.Text = ultimasCompras[4].SKU + " - " + ultimasCompras[4].DESC_PRODUTO; }
                if (ultimasCompras.Count() > 5) { imgProduto6.ImageUrl = ultimasCompras[5].FOTO; imgProduto6.Visible = true; labProduto6.Text = ultimasCompras[5].SKU + " - " + ultimasCompras[5].DESC_PRODUTO; }
            }

        }
        private void CarregarMaisCompra(string cpf)
        {
            var maisCompra = crmController.ObterCRMMaisCompra(cpf);
            gvMaisCompra.DataSource = maisCompra;
            gvMaisCompra.DataBind();
        }
        protected void gvMaisCompra_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CRM_MAISCOMPRAResult prod = e.Row.DataItem as SP_OBTER_CRM_MAISCOMPRAResult;


                }
            }
        }

        protected void btCarregarHistorico_Click(object sender, EventArgs e)
        {
            labHistoricoMensal.Text = "Histórico Geral de " + labNomeClienteTit1.Text;

            var hist = crmController.ObterCRMHistMensal(txtCPF.Text.Trim());
            gvHistCliente.DataSource = hist;
            gvHistCliente.DataBind();
        }
        private void CarregarHistoricoClienteCRM(string cpf)
        {
            var anoAtual = DateTime.Now.Year;
            labHistoricoMensal.Text = "Histórico de " + anoAtual.ToString() + " de " + labNomeClienteTit1.Text;

            var hist = crmController.ObterCRMHistMensal(cpf).Where(p => p.ANO == anoAtual);
            gvHistCliente.DataSource = hist;
            gvHistCliente.DataBind();
        }
        protected void gvHistCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CRM_HISTMENSALResult cliCRM = e.Row.DataItem as SP_OBTER_CRM_HISTMENSALResult;

                    Literal litTipo = e.Row.FindControl("litTipo") as Literal;
                    litTipo.Text = (cliCRM.TIPO == "E") ? "Online" : "Loja Física";

                    Literal litValorBruto = e.Row.FindControl("litValorBruto") as Literal;
                    litValorBruto.Text = Convert.ToDecimal(cliCRM.VALOR_SUBTOTAL).ToString("###,###,###,##0.00");

                    Literal litValorFrete = e.Row.FindControl("litValorFrete") as Literal;
                    litValorFrete.Text = Convert.ToDecimal(cliCRM.VALOR_FRETE).ToString("###,###,###,##0.00");

                    Literal litDesconto = e.Row.FindControl("litDesconto") as Literal;
                    litDesconto.Text = Convert.ToDecimal(cliCRM.DESCONTO).ToString("###,###,###,##0.00");

                    Literal litValorPago = e.Row.FindControl("litValorPago") as Literal;
                    litValorPago.Text = Convert.ToDecimal(cliCRM.VALOR_PAGO).ToString("###,###,###,##0.00");

                    qtdeTickets += Convert.ToInt32(cliCRM.TOT_PEDIDO);
                    valBruto += Convert.ToDecimal(cliCRM.VALOR_SUBTOTAL);
                    valFrete += Convert.ToDecimal(cliCRM.VALOR_FRETE);
                    valDesconto += Convert.ToDecimal(cliCRM.DESCONTO);
                    valPago += Convert.ToDecimal(cliCRM.VALOR_PAGO);

                    //Historico
                    var cliCRMPROD = crmController.ObterCRMHistMensalProduto(cliCRM.CPF, cliCRM.TIPO, Convert.ToInt32(cliCRM.ANO), Convert.ToInt32(cliCRM.MES));
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

                footer.Cells[5].Text = qtdeTickets.ToString();
                footer.Cells[6].Text = valBruto.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = valFrete.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = valDesconto.ToString("###,###,###,##0.00");
                footer.Cells[9].Text = valPago.ToString("###,###,###,##0.00");
            }
        }
        protected void gvHistClienteProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CRM_HISTMENSAL_PRODUTOResult prod = e.Row.DataItem as SP_OBTER_CRM_HISTMENSAL_PRODUTOResult;

                    System.Web.UI.WebControls.ImageButton _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.ImageButton;
                    _imgProduto.ImageUrl = prod.FOTO;

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

        private void CarregarAcoes(string cpf)
        {
            var acoes = crmController.ObterClienteAcao(cpf);
            gvAcao.DataSource = acoes;
            gvAcao.DataBind();
        }
        protected void gvAcao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CRM_CLIENTE_ACAO acao = e.Row.DataItem as CRM_CLIENTE_ACAO;

                    Literal litAcao = e.Row.FindControl("litAcao") as Literal;
                    litAcao.Text = acao.CRM_ACAO1.ACAO;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = acao.DATA_ACAO.ToString("dd/MM/yyyy");
                }
            }
        }

        private void CarregarHistoricoTipoCliente(string cpf)
        {
            var histTipo = crmController.ObterClienteTipoHist(cpf);
            gvTipoCliente.DataSource = histTipo;
            gvTipoCliente.DataBind();
        }
        protected void gvTipoCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CRM_CLIENTE_TIPO tipo = e.Row.DataItem as CRM_CLIENTE_TIPO;

                    Literal litTipoCliente = e.Row.FindControl("litTipoCliente") as Literal;
                    litTipoCliente.Text = tipo.CRM_TIPO1.TIPO;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = tipo.DATA.ToString("dd/MM/yyyy");
                }
            }

        }

        protected void btZerarHandClub_Click(object sender, EventArgs e)
        {
            try
            {
                if (hidCodigoPrograma.Value == "")
                {
                    labErro.Text = "Erro ao carregar codigo do programa...";
                    return;
                }

                var codigoCliente = txtCodigoCliente.Text.Trim();
                var codigoPrograma = Convert.ToInt32(hidCodigoPrograma.Value);

                var clienteZerado = crmController.ObterClienteHandZerado(codigoPrograma, codigoCliente);
                if (clienteZerado != null)
                {
                    labErro.Text = "Os pontos deste clientes já foram zerados...";
                    return;
                }
                else
                {
                    clienteZerado = new CRM_HANDCLUB_ZERADO();
                    clienteZerado.COD_FIDELIDADE_PROGRAMA = codigoPrograma;
                    clienteZerado.CODIGO_CLIENTE = codigoCliente;
                    clienteZerado.DATA_INCLUSAO = DateTime.Now;

                    crmController.InserirClienteHandZerado(clienteZerado);

                    //zerar clientes
                    crmController.ZerarHandclub();
                }

                CarregarCliente(txtCPF.Text.Trim());

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }



        }
        private void CarregarCupom(string cpf)
        {
            var cliCupom = crmController.ObterClienteCupom(cpf);
            gvCupom.DataSource = cliCupom;
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
                    return;
                }

                if (ddlFreteGratis.SelectedValue == "")
                {
                    labCupomErro.Text = "Selecione o tipo de frete.";
                    return;
                }

                if (txtCupomVal.Text == "")
                {
                    labCupomErro.Text = "Informe o valor do cupom.";
                    return;
                }

                var valorCupom = Convert.ToDecimal(txtCupomVal.Text);
                if (valorCupom <= 0)
                {
                    labCupomErro.Text = "Informe o valor do cupom maior que zero.";
                    return;
                }

                var cupomAUX = "HC" + ObterAuxCupom() + "-" + txtCliente.Text.Trim().Split(' ')[0].ToUpper().Substring(0, 3);
                var cupom = cupomAUX + "-" + "$" + valorCupom.ToString().Replace(",", "").Replace(".", "");

                var freteGratis = (ddlFreteGratis.SelectedValue == "1") ? true : false;

                var magV2 = new MagentoV2();
                var cupomResult = magV2.GerarCupom(cupom, Convert.ToDouble(valorCupom), freteGratis);

                if (cupomResult.CouponId > 0)
                {
                    var cliCupom = new CRM_CLIENTE_CUPOM();
                    cliCupom.CPF = txtCPF.Text.Trim();
                    cliCupom.CUPOM = cupom;
                    cliCupom.FRETE_GRATIS = freteGratis;
                    cliCupom.VALOR = valorCupom;
                    cliCupom.DATA_CRIACAO = DateTime.Now;
                    cliCupom.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    cliCupom.ID_CUPOM = cupomResult.CouponId;
                    cliCupom.ID_RULE = Convert.ToInt32(cupomResult.RuleId);
                    cliCupom.VERSAO = 2;

                    crmController.InserirClienteCupom(cliCupom);

                    CarregarCupom(cliCupom.CPF);

                    txtCupomVal.Text = "";
                    labCupomErro.Text = "Cupom criado com sucesso: " + cupom;
                }
                else { labCupomErro.Text = "Erro ao gerar cupom"; }
            }
            catch (Exception ex)
            {
                labCupomErro.Text = ex.Message;
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
                    CRM_CLIENTE_CUPOM cupom = e.Row.DataItem as CRM_CLIENTE_CUPOM;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = cupom.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litFreteGratis = e.Row.FindControl("litFreteGratis") as Literal;
                    litFreteGratis.Text = ((cupom.FRETE_GRATIS == true) ? "Sim" : ((cupom.FRETE_GRATIS == null) ? "-" : "Não"));

                    Literal litValCupom = e.Row.FindControl("litValCupom") as Literal;
                    litValCupom.Text = cupom.VALOR.ToString();

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

                var cliCupom = crmController.ObterClienteCupomPorCodigo(txtCPF.Text, cupom, 2);
                if (cliCupom != null)
                {
                    //excluir do magento
                    var mag = new MagentoV2();
                    mag.ExcluirCupom(cliCupom.ID_CUPOM, Convert.ToInt32(cliCupom.ID_RULE));

                    //excluir da intranet
                    cliCupom.DATA_EXCLUSAO = DateTime.Now;
                    cliCupom.MOTIVO = "";
                    crmController.AtualizarClienteCupom(cliCupom);

                    //recarregar grid
                    CarregarCupom(txtCPF.Text);
                    labCupomErro.Text = "Cupom excluído com sucesso.";
                }
                else { labCupomErro.Text = "Cupom não foi encontrado."; }
            }
            catch (Exception ex)
            {
                labCupomErro.Text = ex.Message;
            }
        }


    }
}


