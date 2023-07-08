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
    public partial class fisc_emissao_nf_devtecido_baixar : System.Web.UI.Page
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

                if (pedidoQtde.EMISSAO != null)
                {
                    Response.Write("NOTA FISCAL DESTE PEDIDO JÁ SALVA.");
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

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue.Trim() == "")
            {
                labFilial.ForeColor = _notOK;
                retorno = false;
            }

            labNF.ForeColor = _OK;
            if (txtNF.Text.Trim() == "")
            {
                labNF.ForeColor = _notOK;
                retorno = false;
            }

            labSerie.ForeColor = _OK;
            if (txtSerie.Text.Trim() == "")
            {
                labSerie.ForeColor = _notOK;
                retorno = false;
            }

            labEmissao.ForeColor = _OK;
            if (txtEmissao.Text.Trim() == "")
            {
                labEmissao.ForeColor = _notOK;
                retorno = false;
            }

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

                pedidoQtde.EMISSAO = Convert.ToDateTime(txtEmissao.Text.Trim());
                pedidoQtde.NF_DEV = txtNF.Text.Trim();
                pedidoQtde.SERIE_DEV = txtSerie.Text.Trim();
                pedidoQtde.COD_FILIAL_DEV = ddlFilial.SelectedValue;

                desenvController.AtualizarPedidoQtde(pedidoQtde);

                if (Constante.enviarEmail)
                    EnviarEmail(pedidoQtde);

                labErro.Text = "Emissão da Nota Fiscal salva com Sucesso.";

                btSalvar.Enabled = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "EMAIL"
        private void EnviarEmail(DESENV_PEDIDO_QTDE pedidoQtde)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();

            var assunto = "Intranet: NOTA FISCAL EMITIDA - " + pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR.Trim() + " - " + DateTime.Today.ToString("dd/MM/yyyy");
            email.ASSUNTO = assunto;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(pedidoQtde);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(45, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(DESENV_PEDIDO_QTDE pedidoQtde)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>NOTA FISCAL EMITIDA</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divSaida' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <h2>NF DEV Emitida - " + pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR + " </h2>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cnpjCPF = pedidoQtde.CNPJ.Trim();
            if (cnpjCPF.Length == 11)
                cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"000\.000\.000\-00");
            else
                cnpjCPF = Convert.ToUInt64(cnpjCPF).ToString(@"00\.000\.000\/0000\-00");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    CNPJ:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + cnpjCPF);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Tecido:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.DESENV_PEDIDO1.SUBGRUPO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.DESENV_PEDIDO1.COR_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto Fornecedor");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.PRODUTO_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Qtde:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + (pedidoQtde.QTDE * -1.00M).ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Volume:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.VOLUME.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Transportadora:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.TRANSPORTADORA);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Nota Fiscal:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.NF_DEV);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Série:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + pedidoQtde.SERIE_DEV);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Emissão:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + Convert.ToDateTime(pedidoQtde.EMISSAO).ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <span>Emitida por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");


            return sb.ToString();
        }
        #endregion

    }
}
