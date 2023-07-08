using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;
using System.Text;

namespace Relatorios
{
    public partial class prod_con_compra_aviamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        List<PROD_HB_PROD_AVIAMENTO> lstAviamento = new List<PROD_HB_PROD_AVIAMENTO>();

        int colunaAviamento = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarAviamentos();
                CarregarFornecedores();
                CarregarColecoes();

                divFinalizarCompra.Visible = false;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void CarregarAviamentos()
        {
            List<PROD_AVIAMENTO> _aviamento = prodController.ObterAviamento();
            if (_aviamento != null)
            {
                _aviamento = _aviamento.Where(p => p.STATUS == 'A').ToList();
                _aviamento.Insert(0, new PROD_AVIAMENTO { CODIGO = 0, DESCRICAO = "" });

                ddlAviamento.DataSource = _aviamento;
                ddlAviamento.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'A') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }
        }
        #endregion

        #region "PESQUISA"
        protected void btBuscarAviamentos_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroImpressao.Text = "";
                labMsgSucesso.Text = "";

                if (ddlColecoesBuscar.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                CarregarAviamentosOrcado();
                divFinalizarCompra.Visible = false;

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }
        private void CarregarAviamentosOrcado()
        {
            List<SP_OBTER_AVIAMENTO_ORCADOResult> aviamentoOrcado = prodController.ObterAviamentoOrcado("", ddlAviamento.SelectedValue, ddlFornecedor.SelectedValue, txtHB.Text, ddlColecoesBuscar.SelectedValue.Trim()).Where(p => p.DATA_CANCELAMENTO == null && p.COMPRADO == "N").ToList();

            if (ddlAviamento.SelectedValue != "0")
                aviamentoOrcado = aviamentoOrcado.Where(p => p.PROD_AVIAMENTO.ToString() == ddlAviamento.SelectedValue).ToList();

            if (ddlFornecedor.SelectedValue != "")
                aviamentoOrcado = aviamentoOrcado.Where(p => p.FORNECEDOR.Trim().ToUpper() == ddlFornecedor.SelectedValue.Trim().ToUpper()).ToList();

            if (txtHB.Text.Trim() != "")
                aviamentoOrcado = aviamentoOrcado.Where(p => p.HB.ToString() == txtHB.Text.Trim()).ToList();

            List<SP_OBTER_AVIAMENTO_ORCADOResult> distinctAviamento = aviamentoOrcado.GroupBy(p => new
                                                                                                    {
                                                                                                        CODIGO = p.CODIGO,
                                                                                                        FORNECEDOR = p.FORNECEDOR,
                                                                                                        QTDE_PACOTE = p.QTDE_PACOTE,
                                                                                                        QTDE_POR_PACOTE = p.QTDE_POR_PACOTE,
                                                                                                        PRECO_ORCAMENTO = p.PRECO_ORCAMENTO,
                                                                                                        DESCONTO_ORCAMENTO = p.DESCONTO_ORCAMENTO,
                                                                                                        OBSERVACAO = p.OBSERVACAO
                                                                                                    }).Select(f => new SP_OBTER_AVIAMENTO_ORCADOResult
                                                                                                    {
                                                                                                        CODIGO = f.Key.CODIGO,
                                                                                                        FORNECEDOR = f.Key.FORNECEDOR,
                                                                                                        QTDE_PACOTE = f.Key.QTDE_PACOTE,
                                                                                                        QTDE_POR_PACOTE = f.Key.QTDE_POR_PACOTE,
                                                                                                        PRECO_ORCAMENTO = f.Key.PRECO_ORCAMENTO,
                                                                                                        DESCONTO_ORCAMENTO = f.Key.DESCONTO_ORCAMENTO,
                                                                                                        OBSERVACAO = f.Key.OBSERVACAO
                                                                                                    }).ToList();


            labErroImpressao.Text = "";
            if (distinctAviamento != null)
            {
                gvAviamento.DataSource = distinctAviamento;
                gvAviamento.DataBind();

            }
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_AVIAMENTO_ORCADOResult _aviamento = e.Row.DataItem as SP_OBTER_AVIAMENTO_ORCADOResult;

                    colunaAviamento += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaAviamento.ToString();

                        Literal _litQtdeTotal = e.Row.FindControl("litQtdeTotal") as Literal;
                        if (_litQtdeTotal != null)
                        {
                            decimal qtde = 0;
                            qtde = (_aviamento.QTDE_PACOTE * _aviamento.QTDE_POR_PACOTE);
                            _litQtdeTotal.Text = qtde.ToString("###,###,###,##0.00");
                        }

                        Literal _litPrecoOrcamento = e.Row.FindControl("litPrecoOrcamento") as Literal;
                        if (_litPrecoOrcamento != null)
                            _litPrecoOrcamento.Text = "R$ " + _aviamento.PRECO_ORCAMENTO.ToString("##0.00");

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                        {
                            decimal valor = 0;
                            decimal desconto = 0;

                            valor = (_aviamento.QTDE_PACOTE * _aviamento.PRECO_ORCAMENTO);
                            desconto = (_aviamento.DESCONTO_ORCAMENTO / 100) * valor;

                            _litValorTotal.Text = "R$ " + (valor - desconto).ToString("###,###,###,##0.00");
                        }

                        TextBox txtValorReal = e.Row.FindControl("txtValorReal") as TextBox;
                        if (txtValorReal != null)
                            txtValorReal.Text = _aviamento.PRECO_ORCAMENTO.ToString("##0.00");

                        TextBox txtDescontoReal = e.Row.FindControl("txtDescontoReal") as TextBox;
                        if (txtDescontoReal != null)
                            txtDescontoReal.Text = _aviamento.DESCONTO_ORCAMENTO.ToString("##0.00");


                        Button _btComprar = e.Row.FindControl("btComprar") as Button;
                        if (_btComprar != null)
                            _btComprar.CommandArgument = _aviamento.CODIGO.ToString();

                        Button _btCancelar = e.Row.FindControl("btCancelar") as Button;
                        if (_btCancelar != null)
                            _btCancelar.CommandArgument = _aviamento.CODIGO.ToString();

                        //Popular GRID VIEW FILHO
                        GridView _gvAviamentoHB = e.Row.FindControl("gvAviamentoHB") as GridView;
                        if (_gvAviamentoHB != null)
                        {
                            List<SP_OBTER_AVIAMENTO_ORCADOResult> aviamentoOrcado = prodController.ObterAviamentoOrcado(_aviamento.CODIGO.ToString(), "0", "", "", ddlColecoesBuscar.SelectedValue.Trim()).Where(p => p.COMPRADO == "N" && p.DATA_CANCELAMENTO == null).ToList();
                            if (aviamentoOrcado != null)
                            {
                                _gvAviamentoHB.DataSource = aviamentoOrcado;
                                _gvAviamentoHB.DataBind();
                            }
                        }
                    }
                }
            }
        }
        protected void gvAviamentoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_AVIAMENTO_ORCADOResult _aviamento = e.Row.DataItem as SP_OBTER_AVIAMENTO_ORCADOResult;
                    if (_aviamento != null)
                    {
                        Literal _litCompraExtra = e.Row.FindControl("litCompraExtra") as Literal;
                        if (_litCompraExtra != null)
                        {
                            _litCompraExtra.Text = (_aviamento.COMPRA_EXTRA == 'S') ? "Sim" : "Não";
                        }
                    }
                }
        }
        #endregion

        #region "BAIXAR"
        protected void btComprar_Click(object sender, EventArgs e)
        {
            int codigoOrcamento = 0;
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {

                    labErroImpressao.Text = "";
                    labErro.Text = "";
                    codigoOrcamento = Convert.ToInt32(bt.CommandArgument);
                    if (codigoOrcamento > 0)
                    {
                        GridViewRow row = (GridViewRow)bt.NamingContainer;
                        if (row != null)
                        {
                            decimal descontoReal = 0;
                            decimal valorReal = 0;
                            // hidCodigoOrcamento.Value = codigoOrcamento.ToString();

                            TextBox txtValorReal = row.FindControl("txtValorReal") as TextBox;
                            if (txtValorReal != null && txtValorReal.Text.Trim() != "")
                            {
                                valorReal = Convert.ToDecimal(txtValorReal.Text.Trim());
                            }
                            else
                            {
                                labErro.Text = "Informe o Valor Real";
                                return;
                            }

                            TextBox txtDescontoReal = row.FindControl("txtDescontoReal") as TextBox;
                            if (txtDescontoReal != null && txtDescontoReal.Text.Trim() != "")
                            {
                                descontoReal = Convert.ToDecimal(txtDescontoReal.Text.Trim());
                            }
                            else
                            {
                                labErro.Text = "Informe o Desconto Real";
                                return;
                            }

                            if (codigoOrcamento > 0)
                            {
                                PROD_HB_AVIAMENTO_ORCAMENTO avi = new PROD_HB_AVIAMENTO_ORCAMENTO();
                                avi.CODIGO = codigoOrcamento;
                                avi.PRECO_REAL = valorReal;
                                avi.DESCONTO_REAL = descontoReal;
                                avi.USUARIO_COMPRA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                avi.DATA_COMPRA = DateTime.Now;
                                prodController.ComprarAviamentoOrcamento(avi);

                                //divFinalizarCompra.Visible = false;
                                CarregarAviamentosOrcado();

                                labMsgSucesso.Text = "Compra efetuada com sucesso.";
                            }

                            //divFinalizarCompra.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            labMsgSucesso.Text = "";
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            int codigoOrcamento = 0;
            Button bt = (Button)sender;
            if (bt != null)
            {
                try
                {
                    codigoOrcamento = Convert.ToInt32(bt.CommandArgument);
                    if (codigoOrcamento > 0)
                    {
                        PROD_HB_AVIAMENTO_ORCAMENTO cancelar = new PROD_HB_AVIAMENTO_ORCAMENTO();

                        cancelar.CODIGO = codigoOrcamento;
                        cancelar.USUARIO_CANCELAMENTO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        cancelar.DATA_CANCELAMENTO = DateTime.Now;
                        prodController.CancelarAviamentoOrcamento(cancelar);

                        CarregarAviamentosOrcado();
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message + "........ " + ex.StackTrace.ToString();
                }
            }

            labMsgSucesso.Text = "";
        }
        protected void btFinalizar_Click(object sender, EventArgs e)
        {
            int codigoOrcamento = 0;
            labErroFinalizarCompra.Text = "";
            try
            {
                if (txtPrecoReal.Text.Trim() == "" || Convert.ToDecimal(txtPrecoReal.Text) <= 0)
                {
                    labErroFinalizarCompra.Text = "Informe o preço pago POR PACOTE.";
                    return;
                }
                if (txtDescontoReal.Text.Trim() == "" || Convert.ToDecimal(txtDescontoReal.Text) < 0)
                {
                    labErroFinalizarCompra.Text = "Informe o desconto.";
                    return;
                }

                codigoOrcamento = Convert.ToInt32(hidCodigoOrcamento.Value);
                if (codigoOrcamento > 0)
                {
                    PROD_HB_AVIAMENTO_ORCAMENTO avi = new PROD_HB_AVIAMENTO_ORCAMENTO();
                    avi.CODIGO = codigoOrcamento;
                    avi.PRECO_REAL = Convert.ToDecimal(txtPrecoReal.Text);
                    avi.DESCONTO_REAL = Convert.ToDecimal(txtDescontoReal.Text);
                    avi.USUARIO_COMPRA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    avi.DATA_COMPRA = DateTime.Now;
                    prodController.ComprarAviamentoOrcamento(avi);

                    divFinalizarCompra.Visible = false;
                    CarregarAviamentosOrcado();

                    labMsgSucesso.Text = "Compra efetuada com sucesso.";
                }
            }
            catch (Exception ex)
            {
                labErroFinalizarCompra.Text = ex.Message;
            }
        }
        #endregion

    }
}
