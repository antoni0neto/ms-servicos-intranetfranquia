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
    public partial class prod_con_compra_aviamento_extra : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int colunaAviamentoSel = 0;
        decimal qtdeSel = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarFornecedores();
                CarregarColecoes();

                Session["AVIAMENTO_EXTRA"] = null;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'A') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();

                if (ddlColecoes.SelectedValue.Trim() != "0")
                    ddlColecoes_SelectedIndexChanged(ddlColecoes, null);
            }
        }

        #endregion

        #region "FILTROS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlColecoes.SelectedValue.Trim() != "0")
            {
                try
                {
                    labErroFiltro.Text = "";
                    string _colecao = ddlColecoes.SelectedValue.Trim().ToUpper();

                    List<PROD_HB> _hb = new List<PROD_HB>();
                    _hb = prodController.ObterHB().Where(p => p.STATUS != 'X' && p.COLECAO.Trim().ToUpper() == _colecao && p.CODIGO_PAI == null).ToList();
                    if (_hb != null)
                    {

                        _hb.Insert(0, new PROD_HB
                        {
                            CODIGO = 0,
                            HB = 0
                        });

                        ddlHB.DataSource = _hb;
                        ddlHB.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    labErroFiltro.Text = ex.Message;
                }
            }
        }
        protected void ddlHB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prod_hb = ddlHB.SelectedValue;
            if (prod_hb != "")
            {
                try
                {
                    labErroFiltro.Text = "";
                    List<PROD_HB_PROD_AVIAMENTO> aviamentoHB = new List<PROD_HB_PROD_AVIAMENTO>();
                    aviamentoHB = prodController.ObterAviamentoHB(Convert.ToInt32(prod_hb));
                    if (aviamentoHB != null && aviamentoHB.Count > 0)
                    {
                        List<PROD_HB_PROD_AVIAMENTO> novaAviamentoHB = new List<PROD_HB_PROD_AVIAMENTO>();

                        novaAviamentoHB = aviamentoHB.Where(g => g.COMPRA_EXTRA == 'N').Select(p => new PROD_HB_PROD_AVIAMENTO
                        {
                            CODIGO = p.CODIGO,
                            DESCRICAO = (p.PROD_AVIAMENTO1.DESCRICAO + " - " + p.DESCRICAO)
                        }).Distinct().ToList();

                        novaAviamentoHB.Insert(0, new PROD_HB_PROD_AVIAMENTO
                        {
                            CODIGO = 0,
                            DESCRICAO = ""
                        });

                        ddlAviamento.DataSource = novaAviamentoHB;
                        ddlAviamento.DataBind();
                    }
                }
                catch (Exception ex)
                {

                    labErroFiltro.Text = ex.Message;
                }
            }
        }
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            labErroFiltro.Text = "";
            try
            {
                if (ddlColecoes.SelectedValue == "0")
                {
                    labErroFiltro.Text = "Selecione a Coleção.";
                    return;
                }
                if (ddlHB.SelectedValue == "0")
                {
                    labErroFiltro.Text = "Selecione o Número do HB.";
                    return;
                }
                if (ddlAviamento.SelectedValue == "0")
                {
                    labErroFiltro.Text = "Selecione o Aviamento.";
                    return;
                }
                if (txtAviamentoQtde.Text.Trim() == "" || Convert.ToDecimal(txtAviamentoQtde.Text) <= 0)
                {
                    labErroFiltro.Text = "Informe uma Quantidade Válida.";
                    return;
                }

                List<PROD_HB_PROD_AVIAMENTO> _aviamento = Session["AVIAMENTO_EXTRA"] as List<PROD_HB_PROD_AVIAMENTO>;

                if (_aviamento == null)
                    _aviamento = new List<PROD_HB_PROD_AVIAMENTO>();

                PROD_HB_PROD_AVIAMENTO _novo = new PROD_HB_PROD_AVIAMENTO();
                if (ddlAviamento.SelectedValue.Trim() != "" && ddlAviamento.SelectedValue.Trim() != "0")
                    _novo.PROD_AVIAMENTO = prodController.ObterAviamentoHBCodigo(Convert.ToInt32(ddlAviamento.SelectedValue)).PROD_AVIAMENTO;
                if (_novo.PROD_AVIAMENTO != null)
                    _novo.PROD_AVIAMENTO1 = prodController.ObterAviamento(Convert.ToInt32(_novo.PROD_AVIAMENTO));
                _novo.QTDE = Convert.ToDecimal(txtAviamentoQtde.Text);
                _novo.DESCRICAO = prodController.ObterAviamentoHBCodigo(Convert.ToInt32(ddlAviamento.SelectedValue)).DESCRICAO;
                _novo.PROD_HB = Convert.ToInt32(ddlHB.SelectedValue);
                _novo.PROD_HB1 = prodController.ObterHB(Convert.ToInt32(ddlHB.SelectedValue));
                _novo.DATA_INCLUSAO = DateTime.Now.Date;
                _novo.COMPRA_EXTRA = 'S';
                _aviamento.Add(_novo);
                Session["AVIAMENTO_EXTRA"] = _aviamento;

                CarregarAviamentoParaOrcamento();

                txtAviamentoQtde.Text = "";
                ddlAviamento.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                labErroFiltro.Text = ex.Message;
                Session["AVIAMENTO_EXTRA"] = null;
            }
        }
        #endregion

        #region "SALVAR ORCAMENTO"
        private int IncluirAviamento(PROD_HB_PROD_AVIAMENTO _aviamento)
        {
            return prodController.InserirAviamentoHB(_aviamento);
        }
        private int InserirAviamentoOrcamento(PROD_HB_AVIAMENTO_ORCAMENTO _novo)
        {
            return prodController.InserirAviamentoOrcamento(_novo);
        }
        private int RelacionarAviamentoOrcamento(PROD_HB_AVIAMENTO_ORCAMENTO_AVI _novo)
        {
            return prodController.RelacionarAviamentoOrcamento(_novo);
        }
        protected void btSalvarOrcamento_Click(object sender, EventArgs e)
        {
            labErro.Text = "";

            if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labErro.Text = "Selecione o Fornecedor.";
                return;
            }
            if (txtQtdePorPacote.Text.Trim() == "" || Convert.ToDecimal(txtQtdePorPacote.Text.Trim()) <= 0)
            {
                labErro.Text = "Informe a quantidade POR PACOTE.";
                return;
            }
            if (txtQtdePacote.Text.Trim() == "" || Convert.ToDecimal(txtQtdePacote.Text.Trim()) <= 0)
            {
                labErro.Text = "Informe a quantidade DE PACOTE.";
                return;
            }
            if (txtPreco.Text.Trim() == "" || Convert.ToDecimal(txtPreco.Text.Trim()) <= 0)
            {
                labErro.Text = "Informe o preço DO PACOTE.";
                return;
            }
            if (txtDesconto.Text.Trim() == "" || Convert.ToDecimal(txtDesconto.Text.Trim()) < 0 || Convert.ToDecimal(txtDesconto.Text.Trim()) > 100)
            {
                labErro.Text = "Informe um Valor Válido para o DESCONTO em %.";
                return;
            }

            try
            {

                int codigoAviamentoOrcamento = 0;
                PROD_HB_AVIAMENTO_ORCAMENTO _novo = new PROD_HB_AVIAMENTO_ORCAMENTO();
                _novo.FORNECEDOR = ddlFornecedor.SelectedValue;
                _novo.QTDE_POR_PACOTE = Convert.ToDecimal(txtQtdePorPacote.Text);
                _novo.QTDE_PACOTE = Convert.ToDecimal(txtQtdePacote.Text);
                _novo.PRECO_ORCAMENTO = Convert.ToDecimal(txtPreco.Text);
                _novo.DESCONTO_ORCAMENTO = Convert.ToDecimal(txtDesconto.Text);
                _novo.OBSERVACAO = txtObs.Text.Trim();
                _novo.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _novo.DATA_INCLUSAO = DateTime.Now;
                _novo.PRECO_REAL = 0;
                _novo.DESCONTO_REAL = 0;
                codigoAviamentoOrcamento = InserirAviamentoOrcamento(_novo);

                //INSERIR AVIAMENTO
                string HB = "";
                string Aviamento = "";
                string Descricao = "";
                string Qtde = "";
                int codigoAviamento = 0;
                PROD_HB_PROD_AVIAMENTO aviamento = null;
                foreach (GridViewRow row in gvAviamentoSelecionado.Rows)
                {
                    HB = ((Literal)row.FindControl("litHB")).Text;
                    Aviamento = ((Literal)row.FindControl("litAviamento")).Text.Trim().ToUpper();
                    Descricao = row.Cells[3].Text;
                    Qtde = row.Cells[4].Text;

                    if (hidAviamentoInserido.Value == "")
                    {
                        aviamento = new PROD_HB_PROD_AVIAMENTO();
                        aviamento.PROD_AVIAMENTO = prodController.ObterAviamento().Where(p => p.DESCRICAO.ToUpper().Trim() == Aviamento).SingleOrDefault().CODIGO;
                        aviamento.QTDE = Convert.ToDecimal(Qtde);
                        aviamento.DESCRICAO = Descricao;
                        aviamento.PROD_HB = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() && p.HB.ToString() == HB && p.CODIGO_PAI == null).SingleOrDefault().CODIGO;
                        aviamento.DATA_INCLUSAO = DateTime.Now.Date;
                        aviamento.COMPRA_EXTRA = 'S';
                        codigoAviamento = IncluirAviamento(aviamento);
                    }
                    else
                    {
                        PROD_HB_PROD_AVIAMENTO aviamentoExistente = new PROD_HB_PROD_AVIAMENTO();
                        aviamentoExistente = prodController.ObterAviamentoHB().Where(p => p.PROD_HB1.HB.ToString() == HB &&
                                                    p.PROD_AVIAMENTO1.DESCRICAO == Aviamento &&
                                                    p.DESCRICAO == Descricao &&
                                                    p.QTDE.ToString() == Convert.ToDecimal(Qtde).ToString("0.000")).SingleOrDefault();
                        codigoAviamento = aviamentoExistente.CODIGO;
                    }

                    PROD_HB_AVIAMENTO_ORCAMENTO_AVI aviOrcamentoAviamento = null;
                    if (codigoAviamentoOrcamento > 0)
                    {
                        aviOrcamentoAviamento = new PROD_HB_AVIAMENTO_ORCAMENTO_AVI();
                        aviOrcamentoAviamento.PROD_HB_AVIAMENTO_ORCAMENTO = codigoAviamentoOrcamento;
                        aviOrcamentoAviamento.PROD_HB_PROD_AVIAMENTO = codigoAviamento;
                        RelacionarAviamentoOrcamento(aviOrcamentoAviamento);
                    }
                }

                ddlFornecedor.SelectedValue = "Selecione";
                txtQtdePorPacote.Text = "";
                txtQtdePacote.Text = "";
                txtPreco.Text = "";
                txtDesconto.Text = "";
                txtObs.Text = "";

                labErro.Text = "Orçamento inserido com Sucesso.";

                hidAviamentoInserido.Value = "INSERIDO";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message + "......." + ex.StackTrace;
            }
        }
        protected void txtQtdePorPacote_TextChanged(object sender, EventArgs e)
        {
            decimal qtdeTotal = 0;
            decimal qtdePacote = 0;
            if (txtQtdePorPacote.Text.Trim() != "" && Convert.ToDecimal(txtQtdePorPacote.Text.Trim()) > 0)
            {
                GridViewRow footer = gvAviamentoSelecionado.FooterRow;
                if (footer != null)
                {
                    qtdeTotal = (footer.Cells[4].Text.Trim() == "") ? 0 : Convert.ToDecimal(footer.Cells[4].Text.Trim());
                    if (qtdeTotal > 0)
                    {
                        qtdePacote = qtdeTotal / Convert.ToDecimal(txtQtdePorPacote.Text.Trim());
                        txtQtdePacote.Text = qtdePacote.ToString();
                    }
                }
            }
        }

        #endregion

        #region "AVIAMENTOS SELECIONADOS PARA ORCAMENTO"
        protected void gvAviamentoSelecionado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_PROD_AVIAMENTO _aviamento = e.Row.DataItem as PROD_HB_PROD_AVIAMENTO;

                    colunaAviamentoSel += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaAviamentoSel.ToString();

                        Literal _HB = e.Row.FindControl("litHB") as Literal;
                        if (_HB != null)
                            _HB.Text = _aviamento.PROD_HB1.HB.ToString();

                        Literal _aviamentoDesc = e.Row.FindControl("litAviamento") as Literal;
                        if (_aviamentoDesc != null)
                            _aviamentoDesc.Text = _aviamento.PROD_AVIAMENTO1.DESCRICAO;

                        qtdeSel += _aviamento.QTDE;
                    }
                }
            }
        }
        protected void gvAviamentoSelecionado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAviamentoSelecionado.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = qtdeSel.ToString("###,###,###,##0.000");
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            List<PROD_HB_PROD_AVIAMENTO> lstAviamento = new List<PROD_HB_PROD_AVIAMENTO>();

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    if (Session["AVIAMENTO_EXTRA"] != null)
                        lstAviamento = Session["AVIAMENTO_EXTRA"] as List<PROD_HB_PROD_AVIAMENTO>;

                    if (lstAviamento != null)
                    {
                        string HB = ((Literal)row.FindControl("litHB")).Text;
                        string Aviamento = ((Literal)row.FindControl("litAviamento")).Text;
                        string Descricao = row.Cells[3].Text;
                        string Qtde = row.Cells[4].Text;

                        lstAviamento.RemoveAll(p => p.PROD_HB1.HB.ToString() == HB &&
                                                    p.PROD_AVIAMENTO1.DESCRICAO == Aviamento &&
                                                    p.DESCRICAO == Descricao &&
                                                    p.QTDE.ToString() == Qtde);
                    }
                    Session["AVIAMENTO_EXTRA"] = lstAviamento;

                    CarregarAviamentoParaOrcamento();
                }
            }
        }
        private void CarregarAviamentoParaOrcamento()
        {
            if (Session["AVIAMENTO_EXTRA"] != null)
            {
                List<PROD_HB_PROD_AVIAMENTO> _aviamento = Session["AVIAMENTO_EXTRA"] as List<PROD_HB_PROD_AVIAMENTO>;

                if (_aviamento != null)
                {
                    gvAviamentoSelecionado.DataSource = _aviamento;
                    gvAviamentoSelecionado.DataBind();
                }
            }
        }
        #endregion
    }
}
