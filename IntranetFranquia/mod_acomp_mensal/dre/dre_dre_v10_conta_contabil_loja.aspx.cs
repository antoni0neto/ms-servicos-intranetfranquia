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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Text;

namespace Relatorios
{
    public partial class dre_dre_v10_conta_contabil_loja : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        DREController dreController = new DREController();
        int coluna = 0;

        decimal debito = 0;
        decimal credito = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["cu"] == null || Request.QueryString["cu"] == "" ||
                    Request.QueryString["a"] == null || Request.QueryString["a"] == "" ||
                    Request.QueryString["m"] == null || Request.QueryString["m"] == "" ||
                    Request.QueryString["procedure"] == null || Request.QueryString["procedure"] == "")
                {
                    Response.Write("CONTA CONTABIL e/ou LINHA não foi configurada para abertura dos Lançamentos...");
                    Response.End();
                }

                string codCentroCusto = (Request.QueryString["cu"] == null) ? "" : Request.QueryString["cu"].ToString(); // codigo centro custo;
                string contaContabil = Request.QueryString["cc"].ToString().Replace("@", "."); // conta contabil
                string unidadeNegocio = Request.QueryString["un"].ToString(); // unidade de negocio
                string codigoFilial = Request.QueryString["ff"].ToString(); // filial
                string mes = Request.QueryString["m"].ToString(); // mes
                string ano = Request.QueryString["a"].ToString(); // ano
                string query = Request.QueryString["query"].ToString(); // qual query
                string procedure = Request.QueryString["procedure"].ToString(); // qual procedure;
                string rateio = Request.QueryString["rat"].ToString(); // rateado?;

                hidCodCentroCusto.Value = codCentroCusto;
                hidContaContabil.Value = contaContabil;
                hidUnidadeNegocio.Value = unidadeNegocio;
                hidCodigoFilial.Value = codigoFilial;
                hidMes.Value = mes;
                hidAno.Value = ano;
                hidRateio.Value = rateio;

                CarregarLancamentos(codCentroCusto, contaContabil, unidadeNegocio, codigoFilial, ano, mes, rateio);
            }
        }

        #region "ACOES"
        private List<SP_OBTER_DREV10_LANCAMENTO_LOJAResult> ObterLancamentos(string codCentroCusto, string contaContabil, string unidadeNegocio, string codigoFilial, string ano, string mes, string rateio)
        {
            DateTime dataInicio;
            DateTime dataFim;

            int ultimoDiaMes = DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes));

            dataInicio = Convert.ToDateTime(ano + "-" + mes + "-" + "01");
            dataFim = Convert.ToDateTime(ano + "-" + mes + "-" + ultimoDiaMes.ToString());

            if (codigoFilial == "0")
                codigoFilial = "";

            var dreCC = dreController.ObterDREV10LancamentoLoja(codCentroCusto, contaContabil, unidadeNegocio, codigoFilial, dataInicio, dataFim, rateio);

            return dreCC;
        }
        private void CarregarLancamentos(string codCentroCusto, string contaContabil, string unidadeNegocio, string codigoFilial, string ano, string mes, string rateio)
        {

            var dreCC = ObterLancamentos(codCentroCusto, contaContabil, unidadeNegocio, codigoFilial, ano, mes, rateio);

            if (dreCC != null)
            {
                var descConta = (contaContabil == null || contaContabil == "") ? "-" : desenvController.ObterContaContabil(contaContabil).DESC_CONTA.Trim();
                var descCentroCusto = dreController.ObterCentroCusto().Where(p => p.CENTRO_CUSTO.Trim() == codCentroCusto.Trim()).FirstOrDefault().DESC_CENTRO_CUSTO.Trim();

                var valRateio = 0M;
                var rateioFilial = dreController.ObterDRERateioFilial(codigoFilial);
                if (rateioFilial != null)
                    valRateio = rateioFilial.VAL_RATEIO;

                txtCentroCusto.Text = codCentroCusto.Trim() + " - " + descCentroCusto.Trim();
                txtContaContabil.Text = contaContabil.Trim() + " - " + descConta.Trim();
                txtCompetencia.Text = ((mes.Length == 1) ? "0" + mes : mes) + "/" + ano;
                txtValorRateio.Text = (valRateio * 100.00M).ToString("###,###,##0.00") + "%";
                if (hidRateio.Value == "R")
                    txtValorRateio.BackColor = Color.PaleGreen;

                hidRateioVal.Value = valRateio.ToString();

                //Controle de ordenação Inicial
                if (contaContabil != null && contaContabil != "")
                {
                    if (contaContabil.Substring(0, 2) == "3.")
                        dreCC = dreCC.OrderByDescending(p => p.DEBITO).ToList();
                    if (contaContabil.Substring(0, 2) == "4.")
                        dreCC = dreCC.OrderByDescending(p => p.CREDITO).ToList();
                }

                gvLancamento.DataSource = dreCC;
                gvLancamento.DataBind();
            }
        }

        protected void gvLancamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DREV10_LANCAMENTO_LOJAResult dreCC = e.Row.DataItem as SP_OBTER_DREV10_LANCAMENTO_LOJAResult;

                    coluna += 1;
                    if (dreCC != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = "<font size='2' face='Calibri'>" + coluna.ToString() + "</font> ";

                        debito += Convert.ToDecimal(dreCC.DEBITO);
                        credito += Convert.ToDecimal(dreCC.CREDITO);
                    }
                }
            }
        }
        protected void gvLancamento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvLancamento.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[4].Text = credito.ToString("###,###,###,###,##0.00");
                footer.Cells[5].Text = debito.ToString("###,###,###,###,##0.00");

                decimal total = credito - debito;

                decimal valRateio = 1M;

                if (hidRateio.Value == "R")
                    valRateio = Convert.ToDecimal(hidRateioVal.Value);

                txtValor.Text = (total * valRateio).ToString("###,###,###,###,##0.00");

            }
        }
        protected void gvLancamento_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_DREV10_LANCAMENTO_LOJAResult> dreCC = ObterLancamentos(hidCodCentroCusto.Value, hidContaContabil.Value, hidUnidadeNegocio.Value, hidCodigoFilial.Value, hidAno.Value, hidMes.Value, hidRateio.Value);

            //string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            dreCC = dreCC.OrderBy(e.SortExpression + sortDirection);
            gvLancamento.DataSource = dreCC;
            gvLancamento.DataBind();
        }
        #endregion

        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            if (valor < 0)
                tagCor = "#CD2626";

            return "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + "</font> ";
        }

    }
}

