using DAL;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class crm_xxx : System.Web.UI.Page
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
            //btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
            //btZerarHandClub.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btZerarHandClub, null) + ";");
            //btGerarCupom.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerarCupom, null) + ";");
            //btCarregarHistorico.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCarregarHistorico, null) + ";");

        }

        private void CarregarCRMTipo()
        {
            //var tipos = crmController.ObterCRMTipo();
            //tipos.Insert(0, new CRM_TIPO { CODIGO = 0, TIPO = "Selecione" });
            //ddlTipoCliente.DataSource = tipos;
            //ddlTipoCliente.DataBind();
        }
        private void CarregarDados(string cpf)
        {
            ObterPrimeiroNome(cpf);
            CarregarCliente(cpf);
            CarregarUltimasCompras(cpf);
            CarregarMaisCompra(cpf);
        }
        private void CarregarSaldoMes(string codigoCliente)
        {
            //btZerarHandClub.Enabled = false;
            //var dataIniResgate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            //var dataFimResgate = dataIniResgate.AddMonths(1).AddDays(-1);

            //var codigoPrograma = lojaController.ObterProgramaHandclub(dataIniResgate, dataFimResgate).COD_FIDELIDADE_PROGRAMA;
            //hidCodigoPrograma.Value = codigoPrograma.ToString();

            //var saldo = lojaController.ObterSaldoPontos(codigoCliente, dataIniResgate, dataFimResgate);
            //if (saldo != null)
            //{
            //    txtSaldoAtual.Text = saldo.SALDO_PONTOS.ToString("###,###,###,##0.00");
            //    if (saldo.SALDO_PONTOS > 0)
            //        btZerarHandClub.Enabled = true;
            //}
            //else
            //{
            //    txtSaldoAtual.Text = "0,00";
            //}
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
                //txtTelefone.Text = dados.TELEFONE;
                //txtObs.Text = dados.OBS;
                //ddlTipoCliente.SelectedValue = (dados.CODIGO_TIPO_CLIENTE == null) ? "0" : dados.CODIGO_TIPO_CLIENTE.ToString();

                CarregarSaldoMes(dados.CODIGO_CLIENTE);
            }
        }
        protected void btWapp_Click(object sender, ImageClickEventArgs e)
        {
            //var numero = txtTelefone.Text.Trim();
            //var nome = labNomeClienteTit1.Text;

            //if (numero.Length == 13) //é celular
            //    Response.Redirect("https://api.whatsapp.com/send?phone=" + numero + "&text=Oi " + nome, "_blank", null);
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
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


    }
}


