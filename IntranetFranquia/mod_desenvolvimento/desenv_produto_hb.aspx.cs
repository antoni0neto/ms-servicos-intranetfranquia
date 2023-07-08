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
    public partial class desenv_produto_hb : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;
        int colunaFilho = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                RecarregarProduto();
        }

        protected void btPedidoFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                RecarregarProduto();
            }
            catch (Exception ex)
            {
                labPedido.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    int numeroPedido = 0;
                    SP_OBTER_HB_CORTADOResult _produto = e.Row.DataItem as SP_OBTER_HB_CORTADOResult;

                    coluna += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_produto.COLECAO)).DESC_COLECAO;

                        Literal _litNumeroPedido = e.Row.FindControl("litNumeroPedido") as Literal;
                        if (_litNumeroPedido != null)
                        {
                            numeroPedido = desenvController.ObterProdutoPedidoProduto(_produto.CODIGO).DESENV_PEDIDO1.NUMERO_PEDIDO;
                            _litNumeroPedido.Text = numeroPedido.ToString();
                        }

                        //Popular GRID VIEW FILHO
                        GridView gvHB = e.Row.FindControl("gvHB") as GridView;
                        if (gvHB != null)
                        {
                            if (numeroPedido > 0)
                            {
                                List<PROD_HB> _lstHB = (new ProducaoController().ObterHBPedidoNumero(numeroPedido));
                                _lstHB = _lstHB.Where(p => p.COLECAO.Trim() == _produto.COLECAO.Trim()).Where(g => g.GRUPO.Trim() == _produto.GRUPO.Trim()).ToList();

                                colunaFilho = 0;

                                //Só mostra linha se tiver filhos disponíveis
                                if (_lstHB.Count > 0)
                                {
                                    gvHB.DataSource = _lstHB;
                                    gvHB.DataBind();
                                }
                                else
                                {
                                    e.Row.Visible = false;
                                    coluna -= 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void gvHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _hb = e.Row.DataItem as PROD_HB;

                    colunaFilho += 1;
                    if (_hb != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaFilho.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_hb.COLECAO)).DESC_COLECAO;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = (new ProducaoController().ObterCoresBasicas(_hb.COR)).DESC_COR;

                        Button _btRelacionar = e.Row.FindControl("btRelacionar") as Button;
                        if (_btRelacionar != null)
                        {
                            _btRelacionar.CommandArgument = _hb.CODIGO.ToString();
                        }

                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _hb.FOTO_PECA;
                    }
                }
            }
        }
        protected void btRelacionar_Click(object sender, EventArgs e)
        {
            int codigoHB = 0;
            int codigoProduto = 0;
            DESENV_PRODUTO_HB _produtoHB = null;

            try
            {
                Button b = (Button)sender;

                if (b != null)
                {
                    //Obter Codigo HB
                    codigoHB = Convert.ToInt32(b.CommandArgument);

                    //Obter referencia para Obter o Codigo do Produto
                    GridViewRow gv = (GridViewRow)b.NamingContainer;
                    HiddenField hid = ((HiddenField)gv.Parent.Parent.Parent.FindControl("hidCodigoProduto"));
                    if (hid != null)
                        codigoProduto = Convert.ToInt32(hid.Value);

                    //Gravar registro na tabela
                    if (codigoHB > 0 && codigoProduto > 0)
                    {
                        //INSERIR RELACIONAMENTO
                        _produtoHB = new DESENV_PRODUTO_HB();
                        _produtoHB.PROD_HB = codigoHB;
                        _produtoHB.DESENV_PRODUTO = codigoProduto;
                        _produtoHB.DATA_INCLUSAO = DateTime.Now;
                        _produtoHB.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        desenvController.InserirProdutoHB(_produtoHB);

                        RecarregarProduto();
                    }
                }
            }
            catch (Exception ex)
            {
                labPedido.Text = ex.Message;
            }
        }

        #region "DADOS INICIAIS"
        private void RecarregarProduto()
        {
            //Obter lista de produtos
            List<SP_OBTER_HB_CORTADOResult> _produto = null;

            if (txtPedidoNumero.Text.Trim() != "" && Convert.ToInt32(txtPedidoNumero.Text.Trim()) > 0)
                _produto = desenvController.ObterProdutoHBCortado(Convert.ToInt32(txtPedidoNumero.Text));
            else
                _produto = desenvController.ObterProdutoHBCortado(0);

            gvProduto.DataSource = _produto;
            gvProduto.DataBind();
        }
        #endregion
    }
}
