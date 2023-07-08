using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class desenv_pedido_con : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;
        int colunaFilho = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Carregar dados iniciais
                CalendarDataInicio.SelectedDate = DateTime.Now;
                txtDataInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
                CalendarDataFim.SelectedDate = DateTime.Now;
                txtDataFim.Text = DateTime.Now.ToString("dd/MM/yyyy");

                //Chamar a primeira vez
                btBuscarPedido_Click(null, null);
            }
        }

        protected void btBuscarPedido_Click(object sender, EventArgs e)
        {
            DateTime v_dataini = DateTime.Now;
            DateTime v_datafim = DateTime.Now;
            bool _bPedido = false;

            labErro.Text = "";
            if (txtPedidoNumero.Text != "" && Convert.ToInt32(txtPedidoNumero.Text) > 0)
                _bPedido = true;

            if (!_bPedido)
            {
                if (!DateTime.TryParse(txtDataInicio.Text, out v_dataini))
                {
                    labErro.Text = "INFORME UMA DATA INICIAL VÁLIDA.";
                    return;
                }
                if (!DateTime.TryParse(txtDataFim.Text, out v_datafim))
                {
                    labErro.Text = "INFORME UMA DATA FINAL VÁLIDA.";
                    return;
                }
            }

            try
            {
                List<DESENV_PEDIDO> _lstPedido = null;
                //BUSCAR POR PEDIDO
                if (_bPedido)
                {
                    DESENV_PEDIDO _pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoNumero.Text));
                    if (_pedido == null)
                    {
                        labErro.Text = "Nenhum Pedido encontrado. Refaça a sua pesquisa.";
                        return;
                    }

                    _lstPedido = new List<DESENV_PEDIDO>();
                    _lstPedido.Add(_pedido);
                }
                else //BUSCAR POR DATA
                {
                    _lstPedido = desenvController.ObterPedido().Where(i => i.DATA_ENTREGA_REAL >= v_dataini && i.DATA_ENTREGA_REAL <= v_datafim).ToList();
                }

                gvPedido.DataSource = _lstPedido;
                gvPedido.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
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

                        Literal _qtdeProduto = e.Row.FindControl("litQtdeProduto") as Literal;
                        if (_qtdeProduto != null)
                        {
                            total = desenvController.ObterProdutoPedidoPedido(_pedido.CODIGO).Count;
                            _qtdeProduto.Text = total.ToString();
                        }

                        string _status = string.Empty;
                        decimal? qtdeTecido = 0;
                        qtdeTecido = desenvController.ObterPedidoQtdePedido(_pedido.CODIGO).Sum(i => i.QTDE);
                        if (qtdeTecido == null)
                            qtdeTecido = 0;

                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                        {
                            if (_pedido.STATUS == 'A' && qtdeTecido <= 0)
                                _status = "AGUARDANDO TECIDO";
                            else if ((_pedido.STATUS == 'B') || (_pedido.STATUS == 'A' && qtdeTecido > 0))
                                _status = "EM CASA";
                            else if (_pedido.STATUS == 'E')
                                _status = "EXCLUÍDO";
                            else
                                _status = "DESCONHECIDO";

                            _litStatus.Text = _status;
                        }

                        //Popular GRID VIEW FILHO
                        GridView gvProduto = e.Row.FindControl("gvProduto") as GridView;
                        if (gvProduto != null)
                        {
                            List<DESENV_PRODUTO> _lstProduto = new List<DESENV_PRODUTO>();
                            DESENV_PRODUTO _tecido = null;
                            foreach (DESENV_PRODUTO_PEDIDO tp in desenvController.ObterProdutoPedidoPedido(_pedido.CODIGO))
                            {
                                _tecido = new DESENV_PRODUTO();
                                _tecido = desenvController.ObterProduto(tp.DESENV_PRODUTO);
                                if (_tecido != null)
                                    _lstProduto.Add(_tecido);
                            }

                            colunaFilho = 0;
                            gvProduto.DataSource = _lstProduto;
                            gvProduto.DataBind();
                        }
                    }
                }
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    colunaFilho += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaFilho.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_produto.COLECAO)).DESC_COLECAO;

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                        {
                            var prod_hb = desenvController.ObterProdutoHBPorProduto(_produto.CODIGO);
                            if (prod_hb != null)
                                _litNome.Text = prod_hb.NOME.Trim();
                            else
                                _litNome.Text = _produto.MODELO.Trim();
                        }
                    }
                }
            }
        }

        #region "DADOS INICIAIS"
        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }
        #endregion
    }
}
