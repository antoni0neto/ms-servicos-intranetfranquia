using DAL;
using Relatorios.mod_ecom.correio;
using Relatorios.mod_ecom.mag;
using Relatorios.mod_ecomv2.mag2;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecomv2_rastreio_entrega : System.Web.UI.Page
    {
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                pnlMandae.Visible = false;
            }

            //Evitar duplo clique no botão
            btAtualizarMagento.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizarMagento, null) + ";");
            btAtualizarPedido.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizarPedido, null) + ";");
        }

        #region "RASTREIO"
        protected void gvRastreioPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    RASTREIO_MANDAE evento = e.Row.DataItem as RASTREIO_MANDAE;

                    if (evento != null)
                    {

                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = evento.DATA.ToString("dd/MM/yyyy HH:mm");

                    }
                }
            }
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                pnlMandae.Visible = false;
                btAtualizarPedido.Enabled = false;

                string trackNumber = "";
                string pedidoLinx = "";
                string pedidoExterno = "";
                string operadora = "";
                string cliente = "";

                trackNumber = txtTrackNumberFiltro.Text.Trim().ToUpper();
                pedidoLinx = txtPedidoLinxFiltro.Text.Trim().ToUpper();
                pedidoExterno = txtPedidoExternoFiltro.Text.Trim().ToUpper();
                if (trackNumber == "" && pedidoLinx == "" && pedidoExterno == "")
                {
                    labErro.Text = "Informe o Código de Rastreio, Pedido Linx ou Pedido Externo";
                    return;
                }

                if (trackNumber != "")
                {
                    var correioPost = eController.ObterCorreiosPostTrackNumber(trackNumber);
                    if (correioPost != null)
                    {
                        var pedidoAux = correioPost.PEDIDO;

                        // Sim, atualizar pedido no Magento
                        if (correioPost.PEDIDO.Contains("-"))
                            pedidoAux = correioPost.PEDIDO.Substring(0, correioPost.PEDIDO.IndexOf('-'));

                        pedidoLinx = pedidoAux;
                        operadora = correioPost.SHIP_SERVICE_MANDAE;
                        if (operadora == null)
                            operadora = "CORREIOS";

                        var entregaMag = eController.ObterPedidoEntregaPorPedidoLinx(pedidoLinx);
                        if (entregaMag != null)
                            pedidoExterno = entregaMag.PEDIDO_EXTERNO;
                    }
                }
                else if (pedidoLinx != "")
                {
                    var correioPost = eController.ObterCorreiosPost(pedidoLinx);
                    if (correioPost != null && correioPost.Count() > 0)
                    {
                        trackNumber = correioPost.FirstOrDefault().TRACK_NUMBER;
                        operadora = correioPost.FirstOrDefault().SHIP_SERVICE_MANDAE;
                        if (operadora == null)
                            operadora = "CORREIOS";
                        var entregaMag = eController.ObterPedidoEntregaPorPedidoLinx(pedidoLinx);
                        if (entregaMag != null)
                            pedidoExterno = entregaMag.PEDIDO_EXTERNO;
                    }
                }
                else if (pedidoExterno != "")
                {
                    var entregaMag = eController.ObterPedidoEntregaPorPedidoExterno(pedidoExterno);
                    if (entregaMag != null)
                    {
                        var pedidoAux = entregaMag.PEDIDO;

                        // Sim, atualizar pedido no Magento
                        if (entregaMag.PEDIDO.Contains("-"))
                            pedidoAux = entregaMag.PEDIDO.Substring(0, entregaMag.PEDIDO.IndexOf('-'));

                        pedidoLinx = pedidoAux;

                        var correioPost = eController.ObterCorreiosPost(pedidoLinx);
                        if (correioPost != null && correioPost.Count() > 0)
                        {
                            trackNumber = correioPost.FirstOrDefault().TRACK_NUMBER;
                            operadora = correioPost.FirstOrDefault().SHIP_SERVICE_MANDAE;
                            if (operadora == null)
                                operadora = "CORREIOS";
                        }
                    }
                }

                var c = new BaseController().ObterPedido(pedidoLinx);
                if (c != null)
                    cliente = c.CLIENTE_ATACADO;

                txtTrackNumber.Text = trackNumber;
                txtPedidoLinx.Text = pedidoLinx;
                txtPedidoExterno.Text = pedidoExterno;
                txtServico.Text = operadora;
                txtCliente.Text = cliente;

                var trackingId = trackNumber;

                btAtualizarPedido.Enabled = false;
                hidEtiquetaMandae.Value = "0";
                if (trackingId != "")
                {
                    if (trackingId.ToLower().Contains("hand"))
                    {
                        hidEtiquetaMandae.Value = "1";

                        Mandae mandae = new Mandae();
                        var eventoRastreio = mandae.ObterRastreioPedido(trackingId);

                        btAtualizarPedido.Enabled = true;
                        if (eventoRastreio == null || eventoRastreio.Count() <= 0)
                        {
                            labErro.Text = "Código de Rastreio não foi encontrado na Mandaê.";
                            btAtualizarPedido.Enabled = false;
                        }

                        gvRastreioPedido.DataSource = eventoRastreio;
                        gvRastreioPedido.DataBind();
                    }
                    else if (trackingId.ToLower().Substring(0, 2) == "lg")
                    {
                        hidEtiquetaMandae.Value = "2";
                        btAtualizarPedido.Enabled = false;
                        btImprimir.Enabled = false;
                    }
                    else
                    {
                        hidEtiquetaMandae.Value = "0";

                        CorreioPortal pp = new CorreioPortal();
                        var eventoRastreio = pp.ObterRastreioPedido(trackingId);

                        btAtualizarPedido.Enabled = true;
                        if (eventoRastreio == null || eventoRastreio.Count() <= 0)
                        {
                            labErro.Text = "Código de Rastreio não foi encontrado no Correio.";
                            btAtualizarPedido.Enabled = false;
                        }

                        gvRastreioPedido.DataSource = eventoRastreio;
                        gvRastreioPedido.DataBind();
                    }
                }

                pnlMandae.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btRastreio_Click(object sender, EventArgs e)
        {

            if (hidEtiquetaMandae.Value == "1")
            {
                var url = $"https://rastreae.com.br/resultado/{txtTrackNumber.Text}";
                Response.Redirect(url, "_blank", "");
            }
            if (hidEtiquetaMandae.Value == "2")
            {
                var url = $"https://www.loggi.com/rastreador/7e3b2131be/{txtTrackNumber.Text}";
                Response.Redirect(url, "_blank", "");
            }
            else
            {
                var url = $"https://portalpostal.com.br/sro.jsp?sro={txtTrackNumber.Text}";
                Response.Redirect(url, "_blank", "");
            }
        }

        protected void btAtualizarMagento_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                var erros = "";

                var mag = new MagentoV2();
                Mandae mandae = new Mandae();
                CorreioPortal pp = new CorreioPortal();

                var pedidoNaoEntregue = eController.ObterCorreiosPostNaoEntregue();

                foreach (var p in pedidoNaoEntregue)
                {
                    // Pedido está com Status Entregue na Mandae?

                    var pedidoEntregue = false;
                    if (p.TRACK_NUMBER.Trim().ToLower().Contains("hand"))
                        pedidoEntregue = mandae.VerificarEntregaRealizada(p.TRACK_NUMBER);
                    else
                        pedidoEntregue = pp.VerificarEntregaRealizada(p.TRACK_NUMBER);

                    if (pedidoEntregue)
                    {
                        var pedidoAux = p.PEDIDO;

                        // Sim, atualizar pedido no Magento
                        if (p.PEDIDO.Contains("-"))
                            pedidoAux = p.PEDIDO.Substring(0, p.PEDIDO.IndexOf('-'));

                        var entregaMag = eController.ObterPedidoEntregaPorPedidoLinx(pedidoAux);
                        if (entregaMag != null)
                        {
                            var retStatus = mag.AtualizarPedidoCompleto(entregaMag.PEDIDO_EXTERNO);

                            if (retStatus)
                            {
                                p.ENTREGA_REALIZADA = true;
                                eController.AtualizarCorreiosPost(p);
                            }
                            else
                            {
                                erros = erros + "Err Update Magento Status: " + entregaMag.PEDIDO_EXTERNO + "; ";
                            }
                        }
                    }
                }

                if (erros != "")
                    labErro.Text = "Erro em alguns pedidos: " + erros;
                else
                    labErro.Text = "Pedidos atualizados com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btAtualizarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var correioPost = eController.ObterCorreiosPostTrackNumber(txtTrackNumber.Text.Trim());
                if (correioPost != null)
                {
                    if (correioPost.ENTREGA_REALIZADA == true)
                    {
                        labErro.Text = "Pedido já foi marcado como entregue.";
                        return;
                    }


                    var erros = "";

                    var pedidoEntregue = false;
                    if (hidEtiquetaMandae.Value == "1")
                    {
                        Mandae mandae = new Mandae();
                        pedidoEntregue = mandae.VerificarEntregaRealizada(txtTrackNumber.Text.Trim());
                    }
                    else
                    {
                        CorreioPortal pp = new CorreioPortal();
                        pedidoEntregue = pp.VerificarEntregaRealizada(txtTrackNumber.Text.Trim());
                    }

                    if (pedidoEntregue)
                    {
                        // Sim, atualizar pedido no Magento
                        var entregaMag = eController.ObterPedidoEntregaPorPedidoLinx(txtPedidoLinx.Text.Trim());
                        if (entregaMag != null)
                        {
                            var mag = new MagentoV2();
                            var retStatus = mag.AtualizarPedidoCompleto(entregaMag.PEDIDO_EXTERNO);

                            if (retStatus)
                            {
                                correioPost.ENTREGA_REALIZADA = true;
                                eController.AtualizarCorreiosPost(correioPost);
                            }
                            else
                            {
                                erros = erros + "Err Update Magento Status: " + entregaMag.PEDIDO_EXTERNO + "; ";
                            }
                        }
                    }
                    else
                    {
                        labErro.Text = "Pedido ainda não foi entregue.";
                        return;
                    }


                    if (erros != "")
                        labErro.Text = "Erro no pedido: " + erros;
                    else
                        labErro.Text = "Pedido atualizado com sucesso.";
                }
                else
                {
                    labErro.Text = "Pedido não encontrado em CORREIOS_POST.";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {

                if (hidEtiquetaMandae.Value == "1") // mandae
                {
                    var etiqueta = eController.ObterEtiquetaImpressao(txtCliente.Text.Trim(), txtTrackNumber.Text.Trim(), txtPedidoLinx.Text.Trim());
                    ImprimirEtiquetas(etiqueta);
                }
                if (hidEtiquetaMandae.Value == "2") // loggi
                {
                    // do nothing for now
                }
                else
                {
                    Response.Redirect((GerarURLEtiqueta(txtTrackNumber.Text.Trim())), "_blank", "");
                }

            }
            catch (Exception ex)
            {
            }
        }
        public string GerarURLEtiqueta(string codigoRastreio)
        {
            string urlEtiqueta = "";

            var loginCorreio = eController.ObterLoginCorreios();

            if (loginCorreio == null)
                throw new Exception("ECOM_CORREIOS_LOGIN não existe...");

            var _login = loginCorreio.PP_USUARIO;
            var _senha = loginCorreio.PP_SENHA;
            var _codAgencia = loginCorreio.COD_AGENCIA;
            var _numeroCartao = loginCorreio.CARTAO_POSTAGEM;
            var _urlEtiqueta = loginCorreio.URL_ETIQUETA;
            var _urlEtiquetaFormato = loginCorreio.URL_ETIQUETA_FORMATO;

            urlEtiqueta = _urlEtiqueta;

            urlEtiqueta = urlEtiqueta.Replace("##codAgencia##", _codAgencia.ToString());
            urlEtiqueta = urlEtiqueta.Replace("##login##", _login);
            urlEtiqueta = urlEtiqueta.Replace("##senha##", _senha);
            urlEtiqueta = urlEtiqueta.Replace("##sro##", codigoRastreio);
            urlEtiqueta = urlEtiqueta.Replace("##formato##", _urlEtiquetaFormato);

            return urlEtiqueta;
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