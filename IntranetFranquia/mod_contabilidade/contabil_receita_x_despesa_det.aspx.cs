using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class contabil_receita_x_despesa_det : System.Web.UI.Page
    {
        ContabilidadeController contabController = new ContabilidadeController();

        decimal debito = 0;
        decimal credito = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["cc"] == null || Request.QueryString["cc"] == "" ||
                    Request.QueryString["mc"] == null || Request.QueryString["mc"] == "" ||
                    Request.QueryString["a"] == null || Request.QueryString["a"] == "" ||
                    Request.QueryString["m"] == null || Request.QueryString["m"] == "")
                {
                    Response.Write("CONTA CONTABIL e/ou LINHA não foi configurada para abertura dos Lançamentos...");
                    Response.End();
                }

                string contaContabil = Request.QueryString["cc"].ToString().Replace("@", ".");
                string matrizContabil = Request.QueryString["mc"].ToString();
                string mes = Request.QueryString["m"].ToString();
                string ano = Request.QueryString["a"].ToString();

                hidContaContabil.Value = contaContabil;
                hidMatrizContabil.Value = matrizContabil;
                hidMes.Value = mes;
                hidAno.Value = ano;

                CarregarLancamentos(contaContabil, matrizContabil, mes, ano);
            }
        }

        #region "ACOES"
        private List<SP_OBTER_LANCAMENTO_RECEITADESPESAResult> ObterLancamentos(string contaContabil, string matrizContabil, string mes, string ano)
        {
            DateTime dataInicio;
            DateTime dataFim;

            int ultimoDiaMes = DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes));

            dataInicio = Convert.ToDateTime(ano + "-" + mes + "-" + "01");
            dataFim = Convert.ToDateTime(ano + "-" + mes + "-" + ultimoDiaMes.ToString());

            var lancamentoItens = contabController.ObterReceitaXDespesaLancamentos(contaContabil, matrizContabil, dataInicio, dataFim, "");

            return lancamentoItens;
        }
        private void CarregarLancamentos(string contaContabil, string matrizContabil, string mes, string ano)
        {
            var lancamentoItens = ObterLancamentos(contaContabil, matrizContabil, mes, ano);

            if (lancamentoItens != null)
            {
                var descConta = lancamentoItens.FirstOrDefault().DESC_CONTA;

                txtContaContabil.Text = contaContabil.Trim() + " - " + descConta.Trim();
                txtCompetencia.Text = ((mes.Length == 1) ? "0" + mes : mes) + "/" + ano;

                //Controle de ordenação Inicial
                if (contaContabil.Substring(0, 2) == "3.")
                    lancamentoItens = lancamentoItens.OrderByDescending(p => p.DEBITO).ToList();
                if (contaContabil.Substring(0, 2) == "4.")
                    lancamentoItens = lancamentoItens.OrderByDescending(p => p.CREDITO).ToList();

                gvLancamento.DataSource = lancamentoItens;
                gvLancamento.DataBind();
            }
        }


        protected void gvLancamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_LANCAMENTO_RECEITADESPESAResult lancamentoItem = e.Row.DataItem as SP_OBTER_LANCAMENTO_RECEITADESPESAResult;

                    if (lancamentoItem != null)
                    {
                        debito += Convert.ToDecimal(lancamentoItem.DEBITO);
                        credito += Convert.ToDecimal(lancamentoItem.CREDITO);
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

                footer.Cells[5].Text = credito.ToString("###,###,###,###,##0.00");
                footer.Cells[6].Text = debito.ToString("###,###,###,###,##0.00");

                decimal total = credito - debito;
                txtValor.Text = total.ToString("###,###,###,###,##0.00");

            }
        }
        protected void gvLancamento_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_LANCAMENTO_RECEITADESPESAResult> lancamentoItens = ObterLancamentos(hidContaContabil.Value, hidMatrizContabil.Value, hidMes.Value, hidAno.Value);

            //string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            lancamentoItens = lancamentoItens.OrderBy(e.SortExpression + sortDirection);
            gvLancamento.DataSource = lancamentoItens;
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

