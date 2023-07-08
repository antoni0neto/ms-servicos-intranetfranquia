using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class fisc_retirada_devtecido_baixar : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {


                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null)
                    Response.Redirect("facc_menu.aspx");

                var codigoPedidoQtde = Convert.ToInt32(Request.QueryString["p"].ToString());


                var pedidoQtde = desenvController.ObterPedidoQtde(codigoPedidoQtde);
                if (pedidoQtde == null)
                    Response.Redirect("facc_menu.aspx");

                if (pedidoQtde.DATA_BAIXA_RETIRADA != null)
                {
                    Response.Write("DEVOLUÇÃO JA FOI BAIXADA.");
                    Response.End();
                }

                hidCodigoPedidoQtde.Value = codigoPedidoQtde.ToString();

                txtFornecedor.Text = pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR;

                string cnpjCPF = pedidoQtde.CNPJ.Trim();
                if (cnpjCPF.Length == 11)
                    cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"000\.000\.000\-00");
                else
                    cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"00\.000\.000\/0000\-00");
                txtCNPJ.Text = cnpjCPF;
                txtNFOrigem.Text = pedidoQtde.NOTA_FISCAL;
                txtTransportadora.Text = pedidoQtde.TRANSPORTADORA;
                txtFrete.Text = pedidoQtde.FRETE;
                txtTecido.Text = pedidoQtde.DESENV_PEDIDO1.SUBGRUPO;
                txtCor.Text = pedidoQtde.DESENV_PEDIDO1.COR.Trim() + " - " + prodController.ObterCoresBasicas(pedidoQtde.DESENV_PEDIDO1.COR).DESC_COR;
                txtCorFornecedor.Text = pedidoQtde.DESENV_PEDIDO1.COR_FORNECEDOR;
                txtProdutoFornecedor.Text = pedidoQtde.PRODUTO_FORNECEDOR;
                txtNatureza.Text = pedidoQtde.NATUREZA_OPERACAO;
                txtQtde.Text = (pedidoQtde.QTDE * -1.00M).ToString();
                txtPreco.Text = "R$ " + pedidoQtde.VALOR_MATERIAL.ToString();
                txtVolume.Text = pedidoQtde.VOLUME.ToString();
                txtPesoLiq.Text = pedidoQtde.PESOLIQ_KG.ToString();
                txtPesoBruto.Text = pedidoQtde.PESO_KG.ToString();

                ddlFilial.SelectedValue = pedidoQtde.COD_FILIAL_DEV;
                txtNF.Text = pedidoQtde.NF_DEV;
                txtSerie.Text = pedidoQtde.SERIE_DEV;
                txtEmissao.Text = Convert.ToDateTime(pedidoQtde.EMISSAO).ToString("dd/MM/yyyy");
                txtDataRetirada.Text = Convert.ToDateTime(pedidoQtde.DATA_RETIRADA).ToString("dd/MM/yyyy");
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            return retorno;
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                var pedidoQtde = desenvController.ObterPedidoQtde(Convert.ToInt32(hidCodigoPedidoQtde.Value));
                if (pedidoQtde == null)
                {
                    labErro.Text = "Pedido não foi encontrado... Entre em contato com suporte.";
                    return;
                }

                pedidoQtde.DATA_BAIXA_RETIRADA = DateTime.Now;

                desenvController.AtualizarPedidoQtde(pedidoQtde);

                labErro.Text = "Tecido baixado com sucesso.";

                btSalvar.Enabled = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }
}
