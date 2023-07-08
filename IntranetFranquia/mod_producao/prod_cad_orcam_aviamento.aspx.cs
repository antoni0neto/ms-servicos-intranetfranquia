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
    public partial class prod_cad_orcam_aviamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int colunaAviamentoSel = 0;
        decimal qtdeSel = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                /*
                int codigoOrcamento = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "window.close();", true);

                codigoOrcamento = Convert.ToInt32(Request.QueryString["p"].ToString());
                */
                CarregarFornecedores();
                CarregarAviamentoParaOrcamento();
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
        #endregion

        #region "SALVAR ORCAMENTO"
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

                PROD_HB_AVIAMENTO_ORCAMENTO_AVI aviOrcamentoAviamento = null;
                if (codigoAviamentoOrcamento > 0)
                {
                    //Relacionar orcamento com aviamento
                    foreach (GridViewRow row in gvAviamentoSelecionado.Rows)
                    {
                        aviOrcamentoAviamento = new PROD_HB_AVIAMENTO_ORCAMENTO_AVI();
                        aviOrcamentoAviamento.PROD_HB_AVIAMENTO_ORCAMENTO = codigoAviamentoOrcamento;
                        aviOrcamentoAviamento.PROD_HB_PROD_AVIAMENTO = Convert.ToInt32(gvAviamentoSelecionado.DataKeys[row.RowIndex].Value); // row
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
        private void CarregarAviamentoParaOrcamento()
        {
            if (Session["AVIAMENTO"] != null)
            {
                List<PROD_HB_PROD_AVIAMENTO> _aviamento = Session["AVIAMENTO"] as List<PROD_HB_PROD_AVIAMENTO>;
                _aviamento = _aviamento.OrderBy(p => p.PROD_HB1.HB).OrderBy(i => i.PROD_AVIAMENTO1.DESCRICAO).OrderBy(u => u.DESCRICAO).ToList();

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
