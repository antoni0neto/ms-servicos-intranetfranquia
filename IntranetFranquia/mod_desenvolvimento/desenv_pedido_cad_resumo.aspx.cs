using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;

namespace Relatorios
{
    public partial class desenv_pedido_cad_resumo : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController _base;
        int coluna = 0;
        int _totalColunaQtde = 0;
        decimal _totalColunaPreco = 0;
        decimal _totalColunaConsumo = 0;
        decimal _totalColunaTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int codigoPedido = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                codigoPedido = Convert.ToInt32(Request.QueryString["p"].ToString());
                DESENV_PEDIDO _pedido = desenvController.ObterPedido(codigoPedido);
                if (_pedido == null)
                    Response.Redirect("desenv_menu.aspx");

                CarregarPedido(_pedido);


            }
        }

        #region "DADOS INICIAIS"
        private void CarregarPedido(DESENV_PEDIDO _pedido)
        {
            _base = new BaseController();

            labColecao.Text = (_base.BuscaColecaoAtual(_pedido.COLECAO)).DESC_COLECAO;
            labPedidoNumero.Text = _pedido.NUMERO_PEDIDO.ToString();
            labFornecedor.Text = _pedido.FORNECEDOR;
            labDataPedido.Text = _pedido.DATA_PEDIDO.ToString("dd/MM/yyyy");
            if (_pedido.DATA_ENTREGA_PREV != null)
                labPrevisaoEntrega.Text = Convert.ToDateTime(_pedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");
            labUnidadeMedida.Text = (_pedido.UNIDADE_MEDIDA == null) ? "SEM UNIDADE DE MEDIDA" : (((new ProducaoController()).ObterUnidadeMedida(Convert.ToInt32(_pedido.UNIDADE_MEDIDA))).DESCRICAO);
            labQtde.Text = _pedido.QTDE.ToString();
            labPreco.Text = _pedido.VALOR.ToString("#######0.00");
            labFormaPgto.Text = (_base.ObterFormaPgto(_pedido.CONDICAO_PGTO)).DESC_COND_PGTO;

            List<DESENV_PRODUTO> _lstProduto = desenvController.ObterProdutoNoCarrinho(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO, _pedido.CODIGO);
            if (_lstProduto != null && _lstProduto.Count > 0)
            {
                gvProduto.DataSource = _lstProduto;
                gvProduto.DataBind();
            }

        }
        #endregion

        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    coluna += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        decimal _consumoTotal = 0;
                        Literal _litQtdeConsumo = e.Row.FindControl("litQtdeConsumo") as Literal;
                        if (_litQtdeConsumo != null)
                        {
                            _consumoTotal = Convert.ToDecimal(((_produto.QTDE == null) ? 0 : _produto.QTDE) * _produto.CONSUMO);
                            _litQtdeConsumo.Text = _consumoTotal.ToString("#######0.00").Replace(",00", "");
                            _totalColunaTotal += _consumoTotal;
                        }

                        _totalColunaQtde += Convert.ToInt32(_produto.QTDE);
                        _totalColunaPreco += Convert.ToDecimal(_produto.PRECO);
                        _totalColunaConsumo += Convert.ToDecimal(_produto.CONSUMO);
                    }
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProduto.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                //Preço
                _footer.Cells[4].Text = _totalColunaPreco.ToString("######0.00").Replace(",00", "");
                _footer.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                //Qtde Varejo
                _footer.Cells[5].Text = _totalColunaQtde.ToString();
                _footer.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                //Consumo
                _footer.Cells[6].Text = _totalColunaConsumo.ToString("######0.00").Replace(",00", "");
                _footer.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                //Total
                _footer.Cells[7].Text = _totalColunaTotal.ToString("######0.00").Replace(",00", "");
                _footer.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            }

            decimal _sobra = 0;
            if (Convert.ToDecimal(labQtde.Text) > _totalColunaTotal)
            {
                _sobra = Convert.ToDecimal(labQtde.Text) - _totalColunaTotal;
                labAtencao.Text = "ATENÇÃO: FALTAM " + _sobra.ToString() + " " + labUnidadeMedida.Text + "s";
            }
            else if (Convert.ToDecimal(labQtde.Text) == _totalColunaTotal)
            { labAtencao.Text = ""; }
            else
            {
                decimal _porc = 0;
                _porc = (((_totalColunaTotal / Convert.ToDecimal(labQtde.Text) * 100)) - 100);
                labAtencao.Text = "Quantidade TOTAL DE CONSUMO dos produtos ultrapassou a quantidade do PEDIDO em " + _porc.ToString("#####0.00") + "%";
            }

        }
    }
}
