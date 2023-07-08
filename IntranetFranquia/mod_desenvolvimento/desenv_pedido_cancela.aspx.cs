using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;

namespace Relatorios
{
    public partial class desenv_pedido_cancela : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //RecarregarPedido();
            }
        }

        #region "PEDIDO"
        private void RecarregarPedido()
        {
            int codigoPedido = 0;
            try
            {
                if (txtPedidoNumero.Text == "" || Convert.ToInt32(txtPedidoNumero.Text) <= 0)
                {
                    labMsg.Text = "Informe o número do Pedido.";
                    return;
                }

                codigoPedido = Convert.ToInt32(txtPedidoNumero.Text);
                CarregarPedido(codigoPedido);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void CarregarPedido(int _numeroPedido)
        {
            DESENV_PEDIDO _pedido = desenvController.ObterPedidoNumero(_numeroPedido);

            if (_pedido == null)
            {
                labMsg.Text = "Pedido não encontrado. Refaça sua pesquisa.";
                return;
            }

            if (_pedido.STATUS == 'E')
            {
                labMsg.Text = "Pedido já foi Cancelado. Refaça sua pesquisa.";
                return;
            }

            if (_pedido.DATA_ENTREGA_REAL != null || _pedido.STATUS == 'B')
            {
                labMsg.Text = "Pedido já foi entregue. Refaça sua pesquisa.";
                return;
            }

            List<DESENV_PEDIDO> _lstPedido = new List<DESENV_PEDIDO>();
            _lstPedido.Add(_pedido);
            gvPedido.DataSource = _lstPedido;
            gvPedido.DataBind();
        }
        protected void gvPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    int total = 0;
                    DESENV_PEDIDO _pedido = e.Row.DataItem as DESENV_PEDIDO;

                    coluna += 1;
                    if (_pedido != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        //Qtde de PRODUTO
                        Literal _qtdeProduto = e.Row.FindControl("litQtdeProduto") as Literal;
                        if (_qtdeProduto != null)
                        {
                            total = desenvController.ObterProdutoPedidoPedido(_pedido.CODIGO).Count();
                            _qtdeProduto.Text = total.ToString();
                        }

                        //Qtde INICIAL
                        e.Row.Cells[6].Text = Convert.ToDecimal(_pedido.QTDE).ToString("######0.00").Replace(",00", "") + " " + _pedido.UNIDADE_MEDIDA1.DESCRICAO + "s";

                        //Qtde em casa
                        Literal _litQtdeEmCasa = e.Row.FindControl("litQtdeEmCasa") as Literal;
                        if (_litQtdeEmCasa != null)
                        {
                            List<DESENV_PEDIDO_QTDE> _lstPedidoQtde = desenvController.ObterPedidoQtdePedido(_pedido.CODIGO);
                            var _lstTotal = _lstPedidoQtde.GroupBy(g => g.DESENV_PEDIDO).Select(i => new { QTDE = i.Sum(w => w.QTDE) }).SingleOrDefault();
                            if (_lstTotal != null)
                                _litQtdeEmCasa.Text = Convert.ToDecimal(_lstTotal.QTDE).ToString("######0.00").Replace(",00", "") + " " + _pedido.UNIDADE_MEDIDA1.DESCRICAO + "s";
                            else
                                _litQtdeEmCasa.Text = "0";
                        }

                        Button _btCancelar = e.Row.FindControl("btCancelar") as Button;
                        if (_btCancelar != null)
                            _btCancelar.CommandArgument = _pedido.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvPedido_DataBound(object sender, EventArgs e)
        {
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            DESENV_PEDIDO _pedido = null;
            if (b != null)
            {
                int codigoPedido = Convert.ToInt32(b.CommandArgument);
                GridViewRow _row = (GridViewRow)b.NamingContainer;
                if (_row != null)
                {
                    //CANCELAR PEDIDO
                    _pedido = new DESENV_PEDIDO();
                    _pedido.CODIGO = codigoPedido;
                    _pedido.DATA_ENTREGA_REAL = DateTime.Now;
                    _pedido.STATUS = 'E';
                    desenvController.BaixarPedido(_pedido);

                    //Remover Produtos do PEDIDO
                    foreach (DESENV_PRODUTO_PEDIDO pp in desenvController.ObterProdutoPedidoPedido(codigoPedido))
                        desenvController.ExcluirProdutoPedidoProduto(pp.DESENV_PRODUTO);

                    txtPedidoNumero.Text = "";
                    gvPedido.DataSource = null;
                    gvPedido.DataBind();
                }
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                RecarregarPedido();
            }
            catch (Exception ex)
            {
                labMsg.Text = "ERRO: " + ex.Message;
            }
        }
        #endregion

    }
}
