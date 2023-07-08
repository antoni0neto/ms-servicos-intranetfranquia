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
    public partial class desenv_pedido_baixa : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;
        bool updated = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RecarregarPedido();
            }
        }

        #region "PEDIDO"
        private void RecarregarPedido()
        {
            int codigoPedido = 0;
            try
            {
                if (txtPedidoNumero.Text != "")
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
            List<DESENV_PEDIDO> _pedido = desenvController.ObterPedido();

            //Filtro pro status ATIVO E PENDENTE DE BAIXA
            _pedido = _pedido.Where(i => i.STATUS == 'A').Where(j => j.DATA_ENTREGA_REAL == null).OrderBy(g => g.DATA_PEDIDO).ToList();

            if (_numeroPedido > 0)
                _pedido = _pedido.Where(i => i.NUMERO_PEDIDO == _numeroPedido).ToList();

            if (_pedido != null)
            {
                gvPedido.DataSource = _pedido;
                gvPedido.DataBind();

                if (coluna > 0)
                    btBaixar.Visible = true;
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

                        TextBox t = e.Row.FindControl("txtDataBaixa") as TextBox;
                        if (t != null)
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + t.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

                        //Qtde de PRODUTO
                        Literal _qtdeProduto = e.Row.FindControl("litQtdeProduto") as Literal;
                        if (_qtdeProduto != null)
                        {
                            total = desenvController.ObterProdutoPedidoPedido(_pedido.CODIGO).Count();
                            _qtdeProduto.Text = total.ToString();
                        }

                        //Qtde INICIAL
                        e.Row.Cells[6].Text = Convert.ToDecimal(_pedido.QTDE).ToString("######0.00").Replace(",00", "") + " " + ((_pedido.UNIDADE_MEDIDA1 == null) ? "" : _pedido.UNIDADE_MEDIDA1.DESCRICAO + "s");

                        //Qtde em casa
                        Literal _litQtdeEmCasa = e.Row.FindControl("litQtdeEmCasa") as Literal;
                        if (_litQtdeEmCasa != null)
                        {
                            List<DESENV_PEDIDO_QTDE> _lstPedidoQtde = desenvController.ObterPedidoQtdePedido(_pedido.CODIGO);
                            var _lstTotal = _lstPedidoQtde.GroupBy(g => g.DESENV_PEDIDO).Select(i => new { QTDE = i.Sum(w => w.QTDE) }).SingleOrDefault();
                            if (_lstTotal != null)
                                _litQtdeEmCasa.Text = Convert.ToDecimal(_lstTotal.QTDE).ToString("######0.00").Replace(",00", "") + " " + ((_pedido.UNIDADE_MEDIDA1 == null) ? "" : _pedido.UNIDADE_MEDIDA1.DESCRICAO + "s");
                            else
                                _litQtdeEmCasa.Text = "0";
                        }

                        Button _btAtualizarQtde = e.Row.FindControl("btAtualizarQtde") as Button;
                        if (_btAtualizarQtde != null)
                            _btAtualizarQtde.CommandArgument = _pedido.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvPedido_DataBound(object sender, EventArgs e)
        {
        }
        protected void btAtualizarQtde_Click(object sender, EventArgs e)
        {
            string msg = "";
            DateTime _databaixa = DateTime.Now;
            decimal _qtdeentregue = 0;
            Button b = (Button)sender;
            if (b != null)
            {
                int codigoPedido = Convert.ToInt32(b.CommandArgument);
                GridViewRow _row = (GridViewRow)b.NamingContainer;
                if (_row != null)
                {
                    TextBox _txtDataEntrega = _row.FindControl("txtDataBaixa") as TextBox;
                    if (_txtDataEntrega.Text == "" || !DateTime.TryParse(_txtDataEntrega.Text, out _databaixa))
                        msg = "Data inválida. Informe uma Data.";

                    TextBox _txtQtdeEntrega = _row.FindControl("txtQtdeEntregue") as TextBox;
                    if (_txtQtdeEntrega.Text == "" || !Decimal.TryParse(_txtQtdeEntrega.Text, out _qtdeentregue) || _qtdeentregue == 0)
                        msg = "Quantidade inválida. Informe uma Quantidade.";

                    if (msg != "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2000, theme: 'redError', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                    else
                    {
                        //Inserir QTDE
                        DESENV_PEDIDO_QTDE _qtdePedido = new DESENV_PEDIDO_QTDE();
                        _qtdePedido.DESENV_PEDIDO = codigoPedido;
                        _qtdePedido.QTDE = _qtdeentregue;
                        _qtdePedido.DATA = _databaixa;
                        desenvController.InserirPedidoQtde(_qtdePedido);
                        msg = "Pedido <b>" + _row.Cells[1].Text.ToString() + "</b><br /> Quantidade Atualizada.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2000, theme: 'redError', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }

                    RecarregarPedido();
                }
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labErro.Text = "";
                RecarregarPedido();
            }
            catch (Exception ex)
            {
                labMsg.Text = "ERRO: " + ex.Message;
            }
        }
        protected void btVoltarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labErro.Text = "";
                labErroVoltar.Text = "";
                if (txtPedidoNumeroVoltar.Text.Trim() == "")
                {
                    RecarregarPedido();
                    labErroVoltar.Text = "Informe o Número do Pedido.";
                    return;
                }

                DESENV_PEDIDO pedido = new DESENV_PEDIDO();
                pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoNumeroVoltar.Text.Trim()));
                if (pedido == null)
                {
                    RecarregarPedido();
                    labErroVoltar.Text = "PEDIDO NÃO ENCONTRADO.";
                    return;
                }

                pedido.DATA_ENTREGA_REAL = null;
                pedido.STATUS = 'A';
                desenvController.BaixarPedido(pedido);

                labErroVoltar.Text = "Pedido restaurado com sucesso.";
                txtPedidoNumeroVoltar.Text = "";
                RecarregarPedido();
            }
            catch (Exception ex)
            {
                labErroVoltar.Text = "ERRO: " + ex.Message;
            }
        }
        #endregion

        #region "BAIXAR"
        private void BaixarPedido(GridView _grid)
        {
            string codigo;
            string msg = "";
            DateTime dataBaixa;
            try
            {
                DESENV_PEDIDO _pedido = null;
                foreach (GridViewRow g in _grid.Rows)
                {
                    codigo = _grid.DataKeys[g.RowIndex].Value.ToString();
                    if (codigo != "")
                    {
                        CheckBox _cb = g.FindControl("cbBaixar") as CheckBox;
                        if (_cb != null)
                            if (_cb.Checked)
                                if (DateTime.TryParse(((TextBox)g.FindControl("txtDataBaixa")).Text, out dataBaixa))
                                {
                                    Literal litQtdeEmCasa = (Literal)g.FindControl("litQtdeEmCasa");
                                    if (litQtdeEmCasa != null)
                                    {
                                        if (litQtdeEmCasa.Text != "" && litQtdeEmCasa.Text != "0")
                                        {
                                            _pedido = new DESENV_PEDIDO();
                                            _pedido.CODIGO = Convert.ToInt32(codigo);
                                            _pedido.DATA_ENTREGA_REAL = dataBaixa;
                                            _pedido.STATUS = 'B';
                                            desenvController.BaixarPedido(_pedido);

                                            updated = true;
                                        }
                                        else
                                        {
                                            msg = "QUANTIDADE INVÁLIDA!";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2000, theme: 'redError', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                        }
                                    }
                                }
                                else
                                {
                                    msg = "DATA ENTREGA INVÁLIDA!";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2000, theme: 'redError', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                updated = false;

                BaixarPedido(gvPedido);

                RecarregarPedido();

                if (updated)
                    labErro.Text = "Pedidos baixados com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion


    }
}

