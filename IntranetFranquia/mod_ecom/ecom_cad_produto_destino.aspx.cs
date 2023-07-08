using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_destino : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarPedidos();
            }
        }

        #region "EXPEDICAO"
        private List<SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult> ObterDestino()
        {
            var exp = eController.ObterPedidoExpedicao("", 'N');

            return exp;
        }
        private void CarregarPedidos()
        {
            var exp = ObterDestino();

            gvDestino.DataSource = exp;
            gvDestino.DataBind();
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarPedidos();
            }
            catch (Exception)
            {
            }
        }
        protected void gvDestino_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult magExpedicao = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult;

                    if (magExpedicao != null)
                    {
                        Literal litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        litDataPedido.Text = Convert.ToDateTime(magExpedicao.EMISSAO).ToString("dd/MM/yyyy");

                        Literal litQtdeProduto = e.Row.FindControl("litQtdeProduto") as Literal;
                        if (litQtdeProduto != null)
                            litQtdeProduto.Text = Convert.ToInt32(magExpedicao.QTDE_PRODUTO).ToString();

                        Literal litAbrirPedido = e.Row.FindControl("litAbrirPedido") as Literal;
                        litAbrirPedido.Text = CriarLinkPedido(magExpedicao.PEDIDO_EXTERNO);

                        Literal litDataAprovacao = e.Row.FindControl("litDataAprovacao") as Literal;
                        litDataAprovacao.Text = (magExpedicao.DATA_ACAO == null) ? "-" : Convert.ToDateTime(magExpedicao.DATA_ACAO).ToString("dd/MM/yyyy HH:mm");

                        Button btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (btBaixar != null)
                            btBaixar.CommandArgument = magExpedicao.PEDIDO;

                        ImageButton btVoltar = e.Row.FindControl("btVoltar") as ImageButton;
                        btVoltar.CommandArgument = magExpedicao.PEDIDO.Trim();

                        TextBox txtObs = e.Row.FindControl("txtObs") as TextBox;
                        if (txtObs != null)
                            txtObs.Text = magExpedicao.OBS_RET_LOJA;

                    }
                }
            }
        }

        protected void btVoltar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var pedido = ((ImageButton)sender).CommandArgument;

                var pedidoVenda = baseController.ObterPedido(pedido);
                if (pedidoVenda != null)
                {
                    baseController.AtualizarStatusPedido(pedido, 'E');
                    eController.ExcluirEcomAprovacaoHist(pedido);
                    var expedicao = eController.ObterMagentoExpedicaoPorPedido(pedido.Trim());
                    if (expedicao != null)
                    {
                        eController.ExcluirMagentoExpedicao(expedicao.CODIGO);
                    }
                    CarregarPedidos();
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        private string CriarLinkPedido(string pedidoExterno)
        {
            var link = "../mod_ecomv2/ecomv2_pedido_mag_det.aspx?p=" + pedidoExterno.Trim();
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    string pedido = bt.CommandArgument;

                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    string destino = ((DropDownList)(row.FindControl("ddlDestino"))).SelectedValue;

                    var pedidoVenda = baseController.ObterPedido(pedido);
                    if (pedidoVenda != null)
                    {
                        //aprovar
                        baseController.AtualizarStatusPedido(pedido);
                        var frete = eController.ObterFrete(pedido);

                        if (destino == "F")
                        {
                            //se for faturamento, incluir linha de expedicao
                            var magExpedicao = new ECOM_EXPEDICAO();
                            magExpedicao.PEDIDO = pedido;
                            magExpedicao.VOLUME = "1";
                            magExpedicao.PESO_KG = 0;
                            magExpedicao.DATA_BAIXA = DateTime.Now;
                            magExpedicao.USUARIO_BAIXA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            eController.InserirMagentoExpedicao(magExpedicao);

                            //marcar frete como loja
                            frete.RETIRADA_LOJA = 'S';
                        }
                        else
                        {
                            var expedicao = eController.ObterMagentoExpedicaoPorPedido(pedido.Trim());
                            if (expedicao != null)
                                eController.ExcluirMagentoExpedicao(expedicao.CODIGO);
                            frete.RETIRADA_LOJA = 'N';
                        }

                        eController.AtualizarFrete(frete);

                        CarregarPedidos();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void txtObs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var txtObs = ((TextBox)sender);

                GridViewRow row = (GridViewRow)txtObs.NamingContainer;
                if (row != null)
                {
                    ((DropDownList)row.FindControl("ddlDestino")).SelectedValue = "F";

                    string pedidoExterno = gvDestino.DataKeys[row.RowIndex].Value.ToString();
                    var pedidoMag = eController.ObterPedidoMag(pedidoExterno);
                    if (pedidoMag != null)
                    {
                        pedidoMag.OBS_RET_LOJA = txtObs.Text;
                        eController.AtualizarPedidoMag(pedidoMag);
                    }

                }
            }
            catch (Exception)
            {
            }
        }
    }
}