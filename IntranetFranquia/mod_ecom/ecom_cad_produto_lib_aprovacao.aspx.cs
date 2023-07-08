using DAL;
using System;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_lib_aprovacao : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        USUARIO usuario = new USUARIO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    return;

                CarregarProdutosAprovacao();
            }
        }

        #region "APROVACAO"
        private void CarregarProdutosAprovacao()
        {
            var exp = eController.ObterPedidoExpedicao("", 'E');

            usuario = ((USUARIO)Session["USUARIO"]);

            gvAprovacao.DataSource = exp;
            gvAprovacao.DataBind();
        }
        protected void gvAprovacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult magExpedicao = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_EXPEDICAOResult;

                    if (magExpedicao != null)
                    {
                        Literal _litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        if (_litDataPedido != null)
                            _litDataPedido.Text = Convert.ToDateTime(magExpedicao.EMISSAO).ToString("dd/MM/yyyy");

                        Literal _litQtdeProduto = e.Row.FindControl("litQtdeProduto") as Literal;
                        if (_litQtdeProduto != null)
                            _litQtdeProduto.Text = Convert.ToInt32(magExpedicao.QTDE_PRODUTO).ToString();

                        Literal _litValorPedido = e.Row.FindControl("litValorPedido") as Literal;
                        _litValorPedido.Text = "R$ " + Convert.ToDecimal(magExpedicao.VALOR_PEDIDO).ToString("###,###,##0.00");

                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        _btExcluir.CommandArgument = magExpedicao.PEDIDO.ToString();

                        ImageButton _btBaixar = e.Row.FindControl("btBaixar") as ImageButton;
                        _btBaixar.CommandArgument = magExpedicao.PEDIDO.ToString();

                        _btBaixar.Enabled = false;
                        _btExcluir.Enabled = false;
                        if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 43)
                        {
                            _btBaixar.Enabled = true;
                            _btExcluir.Enabled = true;
                        }

                        Literal litAbrirPedido = e.Row.FindControl("litAbrirPedido") as Literal;
                        litAbrirPedido.Text = CriarLinkPedido(magExpedicao.PEDIDO_EXTERNO);

                        Literal litAbrirHistorico = e.Row.FindControl("litAbrirHistorico") as Literal;
                        litAbrirHistorico.Text = CriarLinkPedidoHist(magExpedicao.PEDIDO_EXTERNO);

                        var hist = eController.ObterEcomPedidoMagClienteHist(magExpedicao.PEDIDO_EXTERNO);
                        if (hist != null && hist.Count() > 0)
                        {
                            e.Row.BackColor = Color.LightYellow;
                            Literal litQtdeTentativa = e.Row.FindControl("litQtdeTentativa") as Literal;
                            litQtdeTentativa.Text = hist.Count().ToString();
                        }

                    }
                }
            }
        }
        #endregion

        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    string pedido = bt.CommandArgument;
                    //aprovar
                    var pedidoVenda = baseController.ObterPedido(pedido);
                    if (pedidoVenda != null)
                    {
                        InserirAprovacaoHist(pedido, "A");
                        //atualizar para no aguardo
                        //necessario decidie se vai para expedicao, ou somente faturar pq foi retirado em loja
                        baseController.AtualizarStatusPedido(pedido, 'N');
                    }

                    CarregarProdutosAprovacao();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void InserirAprovacaoHist(string pedido, string acao)
        {

            var exp = eController.ObterPedidoExpedicao(pedido, 'E').FirstOrDefault();

            var aprovacaohist = new ECOM_PEDIDO_APROVACAO_HIST();
            aprovacaohist.PEDIDO = exp.PEDIDO;
            aprovacaohist.PEDIDO_EXTERNO = exp.PEDIDO_EXTERNO;
            aprovacaohist.EMISSAO = Convert.ToDateTime(exp.EMISSAO);
            aprovacaohist.CLIENTE_ATACADO = exp.CLIENTE_ATACADO;
            aprovacaohist.QTDE_PRODUTO = Convert.ToInt32(exp.QTDE_PRODUTO);
            aprovacaohist.VALOR_PEDIDO = Convert.ToDecimal(exp.VALOR_PEDIDO);
            aprovacaohist.ACAO = acao;
            aprovacaohist.USUARIO_ACAO = ObterUsuario().CODIGO_USUARIO;
            aprovacaohist.DATA_ACAO = DateTime.Now;

            eController.InserirEcomAprovacaoHist(aprovacaohist);
        }
        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    string pedido = bt.CommandArgument;
                    //excluir
                    InserirAprovacaoHist(pedido, "E");
                    eController.ExcluirPedidoEcom(pedido);


                    CarregarProdutosAprovacao();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string CriarLinkPedido(string pedidoExterno)
        {
            var link = "../mod_ecomv2/ecomv2_pedido_mag_det.aspx?p=" + pedidoExterno.Trim();
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }
        private string CriarLinkPedidoHist(string pedidoExterno)
        {
            var link = "ecom_pedido_mag_histbol_cli.aspx?p=" + pedidoExterno.Trim();
            var linkOk = "<a href=\"javascript: openwindowhist('" + link + "')\"><img alt='' src='../../Image/email.png' width='13px' /></a>";
            return linkOk;
        }


    }
}