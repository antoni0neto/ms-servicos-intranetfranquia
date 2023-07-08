using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class AlterarFechamentoCaixa : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USUARIO"] != null)
                {
                    USUARIO usuario = (USUARIO)Session["USUARIO"];

                    if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 5 || usuario.CODIGO_PERFIL == 27) // Administrador ou Financeiro
                        txtObsMatriz.Enabled = true;

                    int codigoCaixa = 0;

                    if (Request.QueryString["CodigoCaixa"] != null)
                        codigoCaixa = Convert.ToInt32(Request.QueryString["CodigoCaixa"].ToString());

                    FECHAMENTO_CAIXA caixa = baseController.BuscaCaixa(codigoCaixa);

                    if (caixa != null)
                    {
                        txtDinheiroVenda.Text = "";
                        txtChequeVenda.Text = "";
                        txtCartaoVenda.Text = "";
                        txtSobra.Text = "";
                        txtFalta.Text = "";
                        txtTotal.Text = "";
                        txtLoja.Text = baseController.BuscaFilialCodigoInt(caixa.CODIGO_FILIAL).FILIAL;
                        txtData.Text = caixa.DATA_FECHAMENTO.ToString();
                        txtCartaoRetorno.Text = caixa.VALOR_CARTAO_CREDITO.ToString();
                        txtChequeDeposito.Text = caixa.VALOR_CHEQUE.ToString();
                        txtDinheiroDeposito.Text = caixa.VALOR_DINHEIRO.ToString();
                        txtChequePreRetorno.Text = caixa.VALOR_CHEQUE_PRE.ToString();
                        txtComanda.Text = caixa.VALOR_COMANDA.ToString();
                        txtRetirada.Text = caixa.VALOR_RETIRADA.ToString();
                        if (caixa.VALOR_DEVOLUCAO == null)
                            txtDevolucao.Text = "0";
                        else
                            txtDevolucao.Text = caixa.VALOR_DEVOLUCAO.ToString();

                        txtChequeVenda.Text = (Convert.ToDecimal(txtChequeDeposito.Text) + Convert.ToDecimal(txtChequePreRetorno.Text)).ToString();
                        //txtDinheiroDeposito.Text = (Convert.ToDecimal(txtDinheiroDeposito.Text) - (Convert.ToDecimal(txtRetirada.Text) + Convert.ToDecimal(txtDevolucao.Text))).ToString();
                        txtDinheiroVenda.Text = caixa.VALOR_DINHEIRO.ToString();
                        txtCartaoVenda.Text = txtCartaoRetorno.Text;

                        txtTotal.Text = (Convert.ToDecimal(txtDinheiroVenda.Text) + Convert.ToDecimal(txtChequeVenda.Text) + Convert.ToDecimal(txtCartaoVenda.Text)).ToString();

                        if (Convert.ToDecimal(txtTotal.Text) > Convert.ToDecimal(txtComanda.Text))
                            txtSobra.Text = (Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtComanda.Text)).ToString();
                        if (Convert.ToDecimal(txtTotal.Text) < Convert.ToDecimal(txtComanda.Text))
                            txtFalta.Text = (Convert.ToDecimal(txtComanda.Text) - Convert.ToDecimal(txtTotal.Text)).ToString();

                        if (txtRetirada.Text.Equals(""))
                            txtRetirada.Text = "0";

                        txtVisa.Text = caixa.VISA.ToString();
                        txtVisaCredVista.Text = caixa.V_CRED_VISTA.ToString();
                        txtVisaDebVista.Text = caixa.V_DEB_VISTA.ToString();
                        txtVisaParcAdmin.Text = caixa.V_PARC_ADMIN.ToString();
                        txtVisaParcEstab2x.Text = caixa.V_PARC_ESTAB_2X.ToString();
                        txtVisaParcEstab3x.Text = caixa.V_PARC_ESTAB_3X.ToString();
                        txtVisaParcEstab4x.Text = caixa.V_PARC_ESTAB_4X.ToString();
                        txtVisaParcEstab5x.Text = caixa.V_PARC_ESTAB_5X.ToString();
                        txtVisaParcEstab6x.Text = caixa.V_PARC_ESTAB_6x.ToString();

                        txtVisaParcEstab7x.Text = caixa.V_PARC_ESTAB_7X.ToString();
                        txtVisaParcEstab8x.Text = caixa.V_PARC_ESTAB_8X.ToString();
                        txtVisaParcEstab9x.Text = caixa.V_PARC_ESTAB_9X.ToString();
                        txtVisaParcEstab10x.Text = caixa.V_PARC_ESTAB_10X.ToString();

                        txtMaster.Text = caixa.MASTERCARD.ToString();
                        txtMasterCredVista.Text = caixa.M_CRED_VISTA.ToString();
                        txtMasterDebVista.Text = caixa.M_DEB_VISTA.ToString();
                        txtMasterParcAdmin.Text = caixa.M_PARC_ADMIN.ToString();
                        txtMasterParcEstab2x.Text = caixa.M_PARC_ESTAB_2X.ToString();
                        txtMasterParcEstab3x.Text = caixa.M_PARC_ESTAB_3X.ToString();
                        txtMasterParcEstab4x.Text = caixa.M_PARC_ESTAB_4X.ToString();
                        txtMasterParcEstab5x.Text = caixa.M_PARC_ESTAB_5X.ToString();
                        txtMasterParcEstab6x.Text = caixa.M_PARC_ESTAB_6X.ToString();

                        txtMasterParcEstab7x.Text = caixa.M_PARC_ESTAB_7X.ToString();
                        txtMasterParcEstab8x.Text = caixa.M_PARC_ESTAB_8X.ToString();
                        txtMasterParcEstab9x.Text = caixa.M_PARC_ESTAB_9X.ToString();
                        txtMasterParcEstab10x.Text = caixa.M_PARC_ESTAB_10X.ToString();

                        txtAmex.Text = caixa.AMERICAN.ToString();
                        txtAmexCredVista.Text = caixa.A_CRED_VISTA.ToString();
                        txtAmexDebVista.Text = caixa.A_DEB_VISTA.ToString();
                        txtAmexParcAdmin.Text = caixa.A_PARC_ADMIN.ToString();
                        txtAmexParcEstab2x.Text = caixa.A_PARC_ESTAB_2X.ToString();
                        txtAmexParcEstab3x.Text = caixa.A_PARC_ESTAB_3X.ToString();
                        txtAmexParcEstab4x.Text = caixa.A_PARC_ESTAB_4X.ToString();
                        txtAmexParcEstab5x.Text = caixa.A_PARC_ESTAB_5X.ToString();
                        txtAmexParcEstab6x.Text = caixa.A_PARC_ESTAB_6X.ToString();

                        txtAmexParcEstab7x.Text = caixa.A_PARC_ESTAB_7X.ToString();
                        txtAmexParcEstab8x.Text = caixa.A_PARC_ESTAB_8X.ToString();
                        txtAmexParcEstab9x.Text = caixa.A_PARC_ESTAB_9X.ToString();
                        txtAmexParcEstab10x.Text = caixa.A_PARC_ESTAB_10X.ToString();

                        txtHiper.Text = caixa.HIPERCARD.ToString();
                        txtHiperCredVista.Text = caixa.H_CRED_VISTA.ToString();
                        txtHiperDebVista.Text = caixa.H_DEB_VISTA.ToString();
                        txtHiperParcAdmin.Text = caixa.H_PARC_ADMIN.ToString();
                        txtHiperParcEstab2x.Text = caixa.H_PARC_ESTAB_2X.ToString();
                        txtHiperParcEstab3x.Text = caixa.H_PARC_ESTAB_3X.ToString();
                        txtHiperParcEstab4x.Text = caixa.H_PARC_ESTAB_4X.ToString();
                        txtHiperParcEstab5x.Text = caixa.H_PARC_ESTAB_5X.ToString();
                        txtHiperParcEstab6x.Text = caixa.H_PARC_ESTAB_6X.ToString();

                        txtHiperParcEstab7x.Text = caixa.H_PARC_ESTAB_7X.ToString();
                        txtHiperParcEstab8x.Text = caixa.H_PARC_ESTAB_8X.ToString();
                        txtHiperParcEstab9x.Text = caixa.H_PARC_ESTAB_9X.ToString();
                        txtHiperParcEstab10x.Text = caixa.H_PARC_ESTAB_10X.ToString();

                        txtOutrosCartoes.Text = caixa.OUTROS_CARTOES.ToString();
                        txtResponsavel.Text = caixa.RESPONSAVEL;
                        txtObsGeral.Text = caixa.OBS;
                        txtObsMatriz.Text = caixa.OBS_MATRIZ;

                        if (usuario.CODIGO_PERFIL == 5 || usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 27)
                        {
                            btConferir.Enabled = true;
                        }
                    }
                }
            }
        }

        protected void btConferir_Click(object sender, EventArgs e)
        {
            try
            {
                FECHAMENTO_CAIXA caixa = new FECHAMENTO_CAIXA();

                caixa.CODIGO_FILIAL = Convert.ToInt32(baseController.BuscaFilial(txtLoja.Text).COD_FILIAL);
                caixa.DATA_FECHAMENTO = Convert.ToDateTime(txtData.Text);
                caixa.VALOR_DINHEIRO = Convert.ToDecimal(txtDinheiroDeposito.Text);
                if (txtChequeDeposito.Text.Equals(""))
                    txtChequeDeposito.Text = "0";
                caixa.VALOR_CHEQUE = Convert.ToDecimal(txtChequeDeposito.Text);
                if (txtChequePreRetorno.Text.Equals(""))
                    txtChequePreRetorno.Text = "0";
                caixa.VALOR_CHEQUE_PRE = Convert.ToDecimal(txtChequePreRetorno.Text);
                if (txtCartaoRetorno.Text.Equals(""))
                    txtCartaoRetorno.Text = "0";
                caixa.VALOR_CARTAO_CREDITO = Convert.ToDecimal(txtCartaoRetorno.Text);
                caixa.VALOR_COMANDA = Convert.ToDecimal(txtComanda.Text);
                caixa.VALOR_RETIRADA = Convert.ToDecimal(txtRetirada.Text);
                caixa.VALOR_DEVOLUCAO = Convert.ToDecimal(txtDevolucao.Text);
                if (txtVisa.Text.Equals(""))
                    txtVisa.Text = "0";
                caixa.VISA = Convert.ToDecimal(txtVisa.Text);
                if (txtVisaCredVista.Text.Equals(""))
                    txtVisaCredVista.Text = "0";
                caixa.V_CRED_VISTA = Convert.ToDecimal(txtVisaCredVista.Text);
                if (txtVisaDebVista.Text.Equals(""))
                    txtVisaDebVista.Text = "0";
                caixa.V_DEB_VISTA = Convert.ToDecimal(txtVisaDebVista.Text);
                if (txtVisaParcAdmin.Text.Equals(""))
                    txtVisaParcAdmin.Text = "0";
                caixa.V_PARC_ADMIN = Convert.ToDecimal(txtVisaParcAdmin.Text);
                if (txtVisaParcEstab2x.Text.Equals(""))
                    txtVisaParcEstab2x.Text = "0";
                caixa.V_PARC_ESTAB_2X = Convert.ToDecimal(txtVisaParcEstab2x.Text);
                if (txtVisaParcEstab3x.Text.Equals(""))
                    txtVisaParcEstab3x.Text = "0";
                caixa.V_PARC_ESTAB_3X = Convert.ToDecimal(txtVisaParcEstab3x.Text);
                if (txtVisaParcEstab4x.Text.Equals(""))
                    txtVisaParcEstab4x.Text = "0";
                caixa.V_PARC_ESTAB_4X = Convert.ToDecimal(txtVisaParcEstab4x.Text);
                if (txtVisaParcEstab5x.Text.Equals(""))
                    txtVisaParcEstab5x.Text = "0";
                caixa.V_PARC_ESTAB_5X = Convert.ToDecimal(txtVisaParcEstab5x.Text);
                if (txtVisaParcEstab6x.Text.Equals(""))
                    txtVisaParcEstab6x.Text = "0";
                caixa.V_PARC_ESTAB_6x = Convert.ToDecimal(txtVisaParcEstab6x.Text);
                if (txtVisaParcEstab7x.Text.Equals(""))
                    txtVisaParcEstab7x.Text = "0";
                caixa.V_PARC_ESTAB_7X = Convert.ToDecimal(txtVisaParcEstab7x.Text);
                if (txtVisaParcEstab8x.Text.Equals(""))
                    txtVisaParcEstab8x.Text = "0";
                caixa.V_PARC_ESTAB_8X = Convert.ToDecimal(txtVisaParcEstab8x.Text);
                if (txtVisaParcEstab9x.Text.Equals(""))
                    txtVisaParcEstab9x.Text = "0";
                caixa.V_PARC_ESTAB_9X = Convert.ToDecimal(txtVisaParcEstab9x.Text);
                if (txtVisaParcEstab10x.Text.Equals(""))
                    txtVisaParcEstab10x.Text = "0";
                caixa.V_PARC_ESTAB_10X = Convert.ToDecimal(txtVisaParcEstab10x.Text);

                if (txtMaster.Text.Equals(""))
                    txtMaster.Text = "0";
                caixa.MASTERCARD = Convert.ToDecimal(txtMaster.Text);
                if (txtMasterCredVista.Text.Equals(""))
                    txtMasterCredVista.Text = "0";
                caixa.M_CRED_VISTA = Convert.ToDecimal(txtMasterCredVista.Text);
                if (txtMasterDebVista.Text.Equals(""))
                    txtMasterDebVista.Text = "0";
                caixa.M_DEB_VISTA = Convert.ToDecimal(txtMasterDebVista.Text);
                if (txtMasterParcAdmin.Text.Equals(""))
                    txtMasterParcAdmin.Text = "0";
                caixa.M_PARC_ADMIN = Convert.ToDecimal(txtMasterParcAdmin.Text);
                if (txtMasterParcEstab2x.Text.Equals(""))
                    txtMasterParcEstab2x.Text = "0";
                caixa.M_PARC_ESTAB_2X = Convert.ToDecimal(txtMasterParcEstab2x.Text);
                if (txtMasterParcEstab3x.Text.Equals(""))
                    txtMasterParcEstab3x.Text = "0";
                caixa.M_PARC_ESTAB_3X = Convert.ToDecimal(txtMasterParcEstab3x.Text);
                if (txtMasterParcEstab4x.Text.Equals(""))
                    txtMasterParcEstab4x.Text = "0";
                caixa.M_PARC_ESTAB_4X = Convert.ToDecimal(txtMasterParcEstab4x.Text);
                if (txtMasterParcEstab5x.Text.Equals(""))
                    txtMasterParcEstab5x.Text = "0";
                caixa.M_PARC_ESTAB_5X = Convert.ToDecimal(txtMasterParcEstab5x.Text);
                if (txtMasterParcEstab6x.Text.Equals(""))
                    txtMasterParcEstab6x.Text = "0";
                caixa.M_PARC_ESTAB_6X = Convert.ToDecimal(txtMasterParcEstab6x.Text);
                if (txtMasterParcEstab7x.Text.Equals(""))
                    txtMasterParcEstab7x.Text = "0";
                caixa.M_PARC_ESTAB_7X = Convert.ToDecimal(txtMasterParcEstab7x.Text);
                if (txtMasterParcEstab8x.Text.Equals(""))
                    txtMasterParcEstab8x.Text = "0";
                caixa.M_PARC_ESTAB_8X = Convert.ToDecimal(txtMasterParcEstab8x.Text);
                if (txtMasterParcEstab9x.Text.Equals(""))
                    txtMasterParcEstab9x.Text = "0";
                caixa.M_PARC_ESTAB_9X = Convert.ToDecimal(txtMasterParcEstab9x.Text);
                if (txtMasterParcEstab10x.Text.Equals(""))
                    txtMasterParcEstab10x.Text = "0";
                caixa.M_PARC_ESTAB_10X = Convert.ToDecimal(txtMasterParcEstab10x.Text);

                if (txtAmex.Text.Equals(""))
                    txtAmex.Text = "0";
                caixa.AMERICAN = Convert.ToDecimal(txtAmex.Text);
                if (txtAmexCredVista.Text.Equals(""))
                    txtAmexCredVista.Text = "0";
                caixa.A_CRED_VISTA = Convert.ToDecimal(txtAmexCredVista.Text);
                if (txtAmexDebVista.Text.Equals(""))
                    txtAmexDebVista.Text = "0";
                caixa.A_DEB_VISTA = Convert.ToDecimal(txtAmexDebVista.Text);
                if (txtAmexParcAdmin.Text.Equals(""))
                    txtAmexParcAdmin.Text = "0";
                caixa.A_PARC_ADMIN = Convert.ToDecimal(txtAmexParcAdmin.Text);
                if (txtAmexParcEstab2x.Text.Equals(""))
                    txtAmexParcEstab2x.Text = "0";
                caixa.A_PARC_ESTAB_2X = Convert.ToDecimal(txtAmexParcEstab2x.Text);
                if (txtAmexParcEstab3x.Text.Equals(""))
                    txtAmexParcEstab3x.Text = "0";
                caixa.A_PARC_ESTAB_3X = Convert.ToDecimal(txtAmexParcEstab3x.Text);
                if (txtAmexParcEstab4x.Text.Equals(""))
                    txtAmexParcEstab4x.Text = "0";
                caixa.A_PARC_ESTAB_4X = Convert.ToDecimal(txtAmexParcEstab4x.Text);
                if (txtAmexParcEstab5x.Text.Equals(""))
                    txtAmexParcEstab5x.Text = "0";
                caixa.A_PARC_ESTAB_5X = Convert.ToDecimal(txtAmexParcEstab5x.Text);
                if (txtAmexParcEstab6x.Text.Equals(""))
                    txtAmexParcEstab6x.Text = "0";
                caixa.A_PARC_ESTAB_6X = Convert.ToDecimal(txtAmexParcEstab6x.Text);
                if (txtAmexParcEstab7x.Text.Equals(""))
                    txtAmexParcEstab7x.Text = "0";
                caixa.A_PARC_ESTAB_7X = Convert.ToDecimal(txtAmexParcEstab7x.Text);
                if (txtAmexParcEstab8x.Text.Equals(""))
                    txtAmexParcEstab8x.Text = "0";
                caixa.A_PARC_ESTAB_8X = Convert.ToDecimal(txtAmexParcEstab8x.Text);
                if (txtAmexParcEstab9x.Text.Equals(""))
                    txtAmexParcEstab9x.Text = "0";
                caixa.A_PARC_ESTAB_9X = Convert.ToDecimal(txtAmexParcEstab9x.Text);
                if (txtAmexParcEstab10x.Text.Equals(""))
                    txtAmexParcEstab10x.Text = "0";
                caixa.A_PARC_ESTAB_10X = Convert.ToDecimal(txtAmexParcEstab10x.Text);

                if (txtHiper.Text.Equals(""))
                    txtHiper.Text = "0";
                caixa.HIPERCARD = Convert.ToDecimal(txtHiper.Text);
                if (txtHiperCredVista.Text.Equals(""))
                    txtHiperCredVista.Text = "0";
                caixa.H_CRED_VISTA = Convert.ToDecimal(txtHiperCredVista.Text);
                if (txtHiperDebVista.Text.Equals(""))
                    txtHiperDebVista.Text = "0";
                caixa.H_DEB_VISTA = Convert.ToDecimal(txtHiperDebVista.Text);
                if (txtHiperParcAdmin.Text.Equals(""))
                    txtHiperParcAdmin.Text = "0";
                caixa.H_PARC_ADMIN = Convert.ToDecimal(txtHiperParcAdmin.Text);
                if (txtHiperParcEstab2x.Text.Equals(""))
                    txtHiperParcEstab2x.Text = "0";
                caixa.H_PARC_ESTAB_2X = Convert.ToDecimal(txtHiperParcEstab2x.Text);
                if (txtHiperParcEstab3x.Text.Equals(""))
                    txtHiperParcEstab3x.Text = "0";
                caixa.H_PARC_ESTAB_3X = Convert.ToDecimal(txtHiperParcEstab3x.Text);
                if (txtHiperParcEstab4x.Text.Equals(""))
                    txtHiperParcEstab4x.Text = "0";
                caixa.H_PARC_ESTAB_4X = Convert.ToDecimal(txtHiperParcEstab4x.Text);
                if (txtHiperParcEstab5x.Text.Equals(""))
                    txtHiperParcEstab5x.Text = "0";
                caixa.H_PARC_ESTAB_5X = Convert.ToDecimal(txtHiperParcEstab5x.Text);
                if (txtHiperParcEstab6x.Text.Equals(""))
                    txtHiperParcEstab6x.Text = "0";
                caixa.H_PARC_ESTAB_6X = Convert.ToDecimal(txtHiperParcEstab6x.Text);
                if (txtHiperParcEstab7x.Text.Equals(""))
                    txtHiperParcEstab7x.Text = "0";
                caixa.H_PARC_ESTAB_7X = Convert.ToDecimal(txtHiperParcEstab7x.Text);
                if (txtHiperParcEstab8x.Text.Equals(""))
                    txtHiperParcEstab8x.Text = "0";
                caixa.H_PARC_ESTAB_8X = Convert.ToDecimal(txtHiperParcEstab8x.Text);
                if (txtHiperParcEstab9x.Text.Equals(""))
                    txtHiperParcEstab9x.Text = "0";
                caixa.H_PARC_ESTAB_9X = Convert.ToDecimal(txtHiperParcEstab9x.Text);
                if (txtHiperParcEstab10x.Text.Equals(""))
                    txtHiperParcEstab10x.Text = "0";
                caixa.H_PARC_ESTAB_10X = Convert.ToDecimal(txtHiperParcEstab10x.Text);

                if (txtOutrosCartoes.Text.Equals(""))
                    txtOutrosCartoes.Text = "0";
                caixa.OUTROS_CARTOES = Convert.ToDecimal(txtOutrosCartoes.Text);
                caixa.RESPONSAVEL = txtResponsavel.Text;
                caixa.OBS = txtObsGeral.Text;
                caixa.OBS_MATRIZ = txtObsMatriz.Text;
                caixa.CONFERIDO = true;

                caixa.VALOR_DESPESAS = 0;

                if (Request.QueryString["CodigoCaixa"] != null)
                {
                    int codigoCaixa = Convert.ToInt32(Request.QueryString["CodigoCaixa"].ToString());

                    usuarioController.AtualizaCaixa(caixa, codigoCaixa);

                    //usuarioController.ExcluiHistoricoDeposito(caixa);
                    //usuarioController.InsereHistoricoDeposito(caixa);
                }
                else
                {
                    usuarioController.InsereCaixa(caixa);
                    usuarioController.InsereHistoricoDeposito(caixa);
                }

                Response.Redirect("~/FinalizadoComSucesso.aspx");
            }
            catch (Exception ex)
            {
            }
        }

    }
}