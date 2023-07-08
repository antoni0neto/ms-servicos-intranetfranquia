using DAL;
using Relatorios.mod_ecom.correio;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_faturamento_pop_trocas : System.Web.UI.Page
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
                    ddlFormaEntrega.SelectedValue = pedidoFat.FORMA_ENTREGA;
                    txtPeso.Text = pedidoFat.PESO_KG.ToString();
                    txtTipoFrete.Text = pedidoFat.TIPO_FRETE + " - " + pedidoFat.FORMA_ENTREGA;
                    txtValorFrete.Text = pedidoFat.VALOR_FRETE.ToString();

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

                //SALVAR
                var u = ObterUsuario();
                if (u == null)
                {
                    labErro.Text = "Favor realizar o Login novamente...";
                    return;
                }

                if (txtNF.Text.Trim() == "")
                {
                    labErro.Text = "Informe a NOTA FISCAL...";
                    return;
                }

                if (txtSerie.Text.Trim() == "")
                {
                    labErro.Text = "Informe a SÉRIE da nota fiscal...";
                    return;
                }

                if (txtEmissao.Text.Trim() == "")
                {
                    labErro.Text = "Informe a EMISSÃO da nota fiscal...";
                    return;
                }

                string nfSaida = "";
                nfSaida = "000000000" + txtNF.Text.Trim();
                nfSaida = nfSaida.Substring(nfSaida.Length - 9, 9);

                var pedidoFaturado = eController.ObterPedidoFaturado(txtPedido.Text, ddlFilial.SelectedItem.Text, nfSaida, txtSerie.Text, Convert.ToDateTime(txtEmissao.Text));
                if (pedidoFaturado == null || pedidoFaturado.Count() <= 0)
                {
                    labErro.Text = "Nota Fiscal informada não foi encontrada no LINX. Verifique os dados informados...";
                    return;
                }

                //Atualizar Forma de Entrega ****************************************************************/
                if (ddlFormaEntrega.Enabled)
                {
                    var ecomFrete = eController.ObterFrete(pedido);
                    if (ecomFrete == null)
                    {
                        labErro.Text = "Forma de Entrega do Pedido não encontrada. Entrar em contato com TI.";
                        return;
                    }
                    ecomFrete.FORMA_ENTREGA = ddlFormaEntrega.SelectedValue;
                    eController.AtualizarFrete(ecomFrete);
                }
                /********************************************************************************************/

                // POSTAR CORREIOS
                List<string> trackNumbers = new List<string>();

                //CorreioVipp correioVipp = new CorreioVipp();
                //trackNumbers = correioVipp.PostarObjeto(pedidoFaturado);

                //CorreioPortal correioPortal = new CorreioPortal();
                //trackNumbers = correioPortal.PostarObjeto(pedidoFaturado);

                //List<string> urlEtiquetas = new List<string>();
                //foreach (var er in trackNumbers)
                //{
                //    if (er.Length != 13)
                //    {
                //        labErro.Text = er;
                //        ddlFormaEntrega.Enabled = true;
                //        return;
                //    }
                //    else
                //    {
                //        urlEtiquetas.Add(correioPortal.GerarURLEtiqueta(er));
                //    }
                //}

                Mandae mandae = new Mandae();
                trackNumbers = mandae.PostarObjeto(pedidoFaturado);

                List<string> etiquetas = new List<string>();
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
                        etiquetas.Add(er);
                    }
                }


                //Enviar codigos de rastreio para o cliente
                // ENVIAR E-MAIL CLIENTE

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

                ////retirar do estoque magento
                //RetirarProdutoEstoqueMagento(pedido);

                ////imprimir etiquetas
                //foreach (string etiq in urlEtiquetas)
                //{
                //    Response.Redirect(etiq, "_blank", "");
                //}

                foreach (string eti in etiquetas)
                {
                    var etiqueta = eController.ObterEtiquetaImpressao(pedidoFaturado[0].CLIENTE_ATACADO, eti, pedido);
                    ImprimirEtiquetas(etiqueta);
                }

                btSalvar.Enabled = false;

                labErro.Text = "Pedido atualizado com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void RetirarProdutoEstoqueMagento(string pedido)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            string mapPath = Server.MapPath("..");

            Magento mag = new Magento(usuario.CODIGO_USUARIO, mapPath);

            var expProduto = eController.ObterMagentoExpedicaoProduto(pedido);
            foreach (var exp in expProduto)
            {
                var produtoConfig = eController.ObterMagentoProdutoConfig(exp.PRODUTO, exp.COR_PRODUTO);

                mag.RetirarEstoqueMagentoPorTroca(Convert.ToInt32(produtoConfig.ID_PRODUTO_MAG), exp.TAMANHO, exp.QTDE);
            }

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


