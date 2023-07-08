using DAL;
using Relatorios.mod_ecom.correio;
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
    public partial class ecomv2_cad_produto_faturamento_pop : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEmissao.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["pedido"] == null || Request.QueryString["pedido"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                string pedido = "";

                pedido = Request.QueryString["pedido"].ToString();

                var pedidoFat = eController.ObterPedidoFaturamento(pedido).SingleOrDefault();
                if (pedidoFat != null)
                {
                    txtPedido.Text = pedido;
                    txtCLIFOR.Text = pedidoFat.CLIFOR;
                    txtCliente.Text = pedidoFat.CLIENTE_ATACADO;
                    txtDataPedido.Text = Convert.ToDateTime(pedidoFat.EMISSAO).ToString("dd/MM/yyyy");

                    txtPedidoExterno.Text = pedidoFat.PEDIDO_EXTERNO;
                    txtTransportadora.Text = pedidoFat.TRANSPORTADORA;
                    txtQtde.Text = Convert.ToInt32(pedidoFat.QTDE_PRODUTO).ToString();

                    txtVolume.Text = pedidoFat.VOLUME;
                    txtPeso.Text = pedidoFat.PESO_KG.ToString();
                    txtTipoFrete.Text = pedidoFat.TIPO_FRETE;
                    txtValorFrete.Text = pedidoFat.VALOR_FRETE.ToString();
                    txtObsRetLoja.Text = pedidoFat.OBS_RET_LOJA;
                    txtRastreioLoggi.Enabled = false;

                    if (pedidoFat.PEDIDO_ORIGEM.ToLower() == "b2w")
                    {
                        cbGerarEtiqueta.Checked = false;
                        cbGerarEtiqueta.Enabled = false;

                        ddlFormaEntrega.SelectedValue = "B2W";
                        txtRastreioMP.Text = pedidoFat.TRACK_NUMBER_MP;
                        txtPedidoMP.Text = pedidoFat.ORDER_KEY ?? "-";
                    }
                    else
                    {
                        if (pedidoFat.RETIRADA_LOJA == 'S')
                        {
                            ddlFormaEntrega.SelectedValue = "L";
                            cbGerarEtiqueta.Checked = false;
                            cbGerarEtiqueta.Enabled = false;
                        }
                        else if (pedidoFat.TRANSPORTADORA.ToLower().Contains("log"))
                        {
                            cbGerarEtiqueta.Checked = false;
                            cbGerarEtiqueta.Enabled = false;
                            txtRastreioLoggi.Enabled = true;
                            ddlFormaEntrega.SelectedValue = pedidoFat.FORMA_ENTREGA;
                        }
                        else
                        {
                            ddlFormaEntrega.SelectedValue = pedidoFat.FORMA_ENTREGA;
                        }
                    }

                    CarregarVolumes();
                }

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

        }

        #region "DADOS INICIAIS"

        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }
        #endregion


        private void CarregarVolumes()
        {
            var volumesPedido = eController.ObterMagentoExpedicaoVolumePorPedido(txtPedido.Text);

            gvVolume.DataSource = volumesPedido;
            gvVolume.DataBind();
        }
        protected void gvVolume_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_EXPEDICAO_VOLUME volume = e.Row.DataItem as ECOM_EXPEDICAO_VOLUME;

                    if (volume != null)
                    {
                        Literal litCaixa = e.Row.FindControl("litCaixa") as Literal;
                        litCaixa.Text = volume.ECOM_CAIXA_DIMENSAO1.DESCRICAO;

                        Literal litPesoKG = e.Row.FindControl("litPesoKG") as Literal;
                        litPesoKG.Text = volume.PESO_KG.ToString("###,###,##0.000");

                        Literal litComprimentoCM = e.Row.FindControl("litComprimentoCM") as Literal;
                        litComprimentoCM.Text = volume.COMPRIMENTO_CM.ToString("###,###,##0.00");

                        Literal litLarguraCM = e.Row.FindControl("litLarguraCM") as Literal;
                        litLarguraCM.Text = volume.LARGURA_CM.ToString("###,###,##0.00");

                        Literal litAlturaCM = e.Row.FindControl("litAlturaCM") as Literal;
                        litAlturaCM.Text = volume.ALTURA_CM.ToString("###,###,##0.00");

                    }
                }
            }

        }
        protected void gvVolume_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVolume.FooterRow;
            if (footer != null)
            {
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {

            try
            {

                labErro.Text = "";

                string pedido = txtPedido.Text.Trim();
                string pedidoExterno = txtPedidoExterno.Text.Trim();
                string chaveNFE = "";

                //SALVAR
                var u = ObterUsuario();
                if (u == null)
                {
                    labErro.Text = "Favor realizar o Login novamente.";
                    return;
                }

                if (txtNF.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Nota Fiscal.";
                    return;
                }

                if (txtSerie.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Série da Nota Fiscal.";
                    return;
                }

                if (txtEmissao.Text.Trim() == "")
                {
                    labErro.Text = "Informe a Emissão da Nota Fiscal.";
                    return;
                }

                if (ddlFormaEntrega.SelectedValue.ToLower().Contains("loggi") && txtRastreioLoggi.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Código de Rastreio gerado na plataforma da Loggi.";
                    return;
                }

                string nfSaida = "";
                nfSaida = "000000000" + txtNF.Text.Trim();
                nfSaida = nfSaida.Substring(nfSaida.Length - 9, 9);

                var pedidoFaturado = eController.ObterPedidoFaturado(pedido, ddlFilial.SelectedItem.Text, nfSaida, txtSerie.Text, Convert.ToDateTime(txtEmissao.Text));
                if (pedidoFaturado == null || pedidoFaturado.Count() <= 0)
                {
                    labErro.Text = "Nota Fiscal informada não foi encontrada no LINX. Verifique os dados informados...";
                    return;
                }

                if (pedidoFaturado[0].CHAVE_NFE == null || pedidoFaturado[0].CHAVE_NFE == "")
                {
                    labErro.Text = "Chave na NFE não foi encontrada no LINX. Verifique os dados informados ou entre em contato com suporte...";
                    return;
                }

                chaveNFE = pedidoFaturado[0].CHAVE_NFE.Trim();

                // POSTAR CORREIOS
                List<string> trackNumbers = new List<string>();

                //CorreioVipp correioVipp = new CorreioVipp();
                //trackNumbers = correioVipp.PostarObjeto(pedidoFaturado);

                var mktplace = false;
                CarrierCode carrierCode = CarrierCode.custom;
                List<string> etiquetas = new List<string>();

                //PEDIDO DO SITE
                if (cbGerarEtiqueta.Checked)
                {
                    //CORREIOS PORTAL POSTAL
                    if (ddlFormaEntrega.SelectedValue == "PAC" || ddlFormaEntrega.SelectedValue == "SEDEX")
                    {
                        carrierCode = CarrierCode.correios;

                        CorreioPortal correioPortal = new CorreioPortal();
                        trackNumbers = correioPortal.PostarObjeto(pedidoFaturado);
                        foreach (var er in trackNumbers)
                        {
                            if (er.Length != 13)
                            {
                                labErro.Text = er;
                                ddlFormaEntrega.Enabled = true;
                                return;
                            }
                            else
                            {
                                etiquetas.Add(correioPortal.GerarURLEtiqueta(er));
                            }
                        }
                    }
                    else
                    {
                        carrierCode = CarrierCode.mandae;

                        Mandae mandae = new Mandae();
                        trackNumbers = mandae.PostarObjeto(pedidoFaturado);

                        foreach (var er in trackNumbers)
                        {
                            if (er.Length != 13)
                            {
                                labErro.Text = er;
                                return;
                            }
                            else
                            {
                                etiquetas.Add(er);
                            }
                        }
                    }
                }
                else
                {
                    //PEDIDO B2W
                    if (ddlFormaEntrega.SelectedValue == "B2W")
                    {
                        mktplace = true;
                        trackNumbers.Add(txtRastreioMP.Text);
                        carrierCode = CarrierCode.custom;

                        var correiosPost = eController.ObterCorreiosPostTrackNumber(txtRastreioMP.Text);
                        if (correiosPost == null)
                        {
                            correiosPost = new ECOM_CORREIOS_POST();
                            correiosPost.TRACK_NUMBER = txtRastreioMP.Text;
                            correiosPost.PEDIDO = pedido;
                            correiosPost.ECOM_EXPEDICAO_VOLUME = pedidoFaturado[0].ECOM_EXPEDICAO_VOLUME;
                            correiosPost.VALOR_TARIFA = 0;
                            correiosPost.ENTREGA_DOMICILIAR = "";
                            correiosPost.PRAZO_DIAS_UTEIS = "";
                            correiosPost.PESO_GRAMA = 0;
                            correiosPost.DATA_ENVIO = DateTime.Now;
                            correiosPost.SHIP_SERVICE_MANDAE = "B2W";
                            correiosPost.ENTREGA_REALIZADA = false;
                            eController.InserirCorreiosPost(correiosPost);
                        }
                    }
                    else if (ddlFormaEntrega.SelectedValue.ToLower() == "loggi")
                    {
                        carrierCode = CarrierCode.frenetshipping;

                        Loggi loggi = new Loggi();
                        trackNumbers = loggi.PostarObjeto(pedidoFaturado, txtRastreioLoggi.Text.ToUpper().Trim());
                    }
                    else
                    {
                        // PEDIDO INTERNO OU LOJA
                        carrierCode = CarrierCode.custom;
                        trackNumbers.Add("RET. LOJA");
                    }
                }

                //Enviar codigos de rastreio para o cliente
                //ENVIAR E-MAIL CLIENTE
                var magentoOK = SalvarEntregaMagento(pedido, pedidoExterno, trackNumbers, carrierCode, cbGerarEtiqueta.Checked, mktplace, chaveNFE, txtMsgCliente.Text.Trim());

                if (magentoOK)
                {
                    var magFaturamento = new ECOM_FATURAMENTO();
                    magFaturamento.PEDIDO = pedido;
                    magFaturamento.PEDIDO_EXTERNO = pedidoExterno;
                    magFaturamento.FILIAL = ddlFilial.SelectedItem.Text.Trim();
                    magFaturamento.NF_SAIDA = nfSaida;
                    magFaturamento.SERIE_NF = txtSerie.Text.Trim();
                    magFaturamento.EMISSAO = Convert.ToDateTime(txtEmissao.Text.Trim());
                    magFaturamento.VALOR_FRETE = Convert.ToDecimal(txtValorFrete.Text.Trim());
                    magFaturamento.EMAIL_ENVIADO = true;
                    magFaturamento.DATA_BAIXA = DateTime.Now;
                    magFaturamento.USUARIO_BAIXA = u.CODIGO_USUARIO;

                    eController.InserirMagentoFaturamento(magFaturamento);

                    ////imprimir etiquetas

                    if (carrierCode == CarrierCode.mandae)
                    {
                        foreach (string eti in etiquetas)
                        {
                            var etiqueta = eController.ObterEtiquetaImpressao(pedidoFaturado[0].CLIENTE_ATACADO, eti, pedido);
                            ImprimirEtiquetas(etiqueta);
                        }
                    }
                    else if (carrierCode == CarrierCode.correios)
                    {
                        foreach (string etiq in etiquetas)
                        {
                            Response.Redirect(etiq, "_blank", "");
                        }
                    }

                    btSalvar.Enabled = false;

                    labErro.Text = "Pedido atualizado com sucesso.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                    dialogPai.Visible = true;

                }
                else
                {
                    labErro.Text = trackNumbers[0];
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private bool SalvarEntregaMagento(string pedido, string pedidoExterno, List<string> trackNumbers, CarrierCode carrierCode, bool gerarEtiqueta, bool mktplace, string chaveNFE, string msgParaCliente)
        {
            var mag = new MagentoV2();
            return mag.SalvarEntrega(pedido, pedidoExterno, trackNumbers, carrierCode, gerarEtiqueta, mktplace, chaveNFE, msgParaCliente);
        }

        private void ImprimirEtiquetas(SP_OBTER_ECOM_ETIQUETA_IMPResult etiqueta)
        {

            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "ETIMANDAE_" + etiqueta.TRACK_NUMBER + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(Magento.GerarEtiquetaMandae(etiqueta, Server.MapPath("~")));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
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


    }
}


