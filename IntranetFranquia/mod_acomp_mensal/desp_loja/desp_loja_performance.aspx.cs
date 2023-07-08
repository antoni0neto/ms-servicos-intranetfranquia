using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.Drawing;

namespace Relatorios
{
    public partial class desp_loja_performance : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        int coluna = 0;
        decimal totalBol = 0;
        decimal totalFat = 0;
        List<SP_BUSCAR_FATURAMENTO_ANUALResult> _faturamentoFilial;
        bool g_orderAsc = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaVencimento();

                DateTime dtAtual = DateTime.Now;
                ListItem l = ddlVencimento.Items.FindByText(dtAtual.Month.ToString("0#") + "/" + dtAtual.Year.ToString());
                if (l != null)
                    ddlVencimento.SelectedValue = l.Value;

                ViewState["sortDirection"] = true;
                if (ddlVencimento.SelectedValue != "")
                    CarregarPerformanceLoja("", (bool)ViewState["sortDirection"]);
            }
        }

        private void CarregaVencimento()
        {
            ddlVencimento.DataSource = baseController.BuscaReferencias().OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES);
            ddlVencimento.DataBind();
        }

        protected void ddlVencimento_DataBound(object sender, EventArgs e)
        {
            ddlVencimento.Items.Insert(0, new ListItem("Selecione", ""));
        }

        protected void btPerformance_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            if (ddlVencimento.SelectedValue.ToString().Equals("0") || ddlVencimento.SelectedValue.ToString().Equals(""))
            {
                labErro.Text = "Selecione um Vencimento.";
                return;
            }

            try
            {
                ViewState["sortDirection"] = true;
                CarregarPerformanceLoja("", (bool)ViewState["sortDirection"]);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        [Serializable]
        public class PERFORMANCE
        {
            public PERFORMANCE()
            { }
            public string FILIAL { get; set; }
            public string FATURAMENTO { get; set; }
            public string BOLETO { get; set; }
            public string PERF { get; set; }
        }

        private void CarregarPerformanceLoja(string col, bool orderAsc)
        {
            //Obter faturamento de todas as filiais
            _faturamentoFilial = baseController.BuscarFaturamentoAnual("0", ddlVencimento.SelectedItem.Text.Replace("/", ""));

            //Agrupar por filiais //CASO A FILIAL TENHA SIDO TROCADA NO MEIO DO MÊS
            var _groupFaturamentoFilialAtiva = _faturamentoFilial.GroupBy(c => new { CODIGO_FILIAL = c.CODIGO_FILIAL, COMP = c.COMP }).Select(g =>
                                                                                                                                            new
                                                                                                                                            {
                                                                                                                                                CODIGO_FILIAL = g.Key.CODIGO_FILIAL,
                                                                                                                                                COMP = g.Key.COMP,
                                                                                                                                                VALOR = g.Sum(w => w.VALOR)
                                                                                                                                            }
                                                                                                                                            ).ToList();
            List<PERFORMANCE> _list = new List<PERFORMANCE>();
            foreach (var f in _groupFaturamentoFilialAtiva)
            {
                //Obter Filial
                string filial = baseController.BuscaFilialCodigo(Convert.ToInt32(f.CODIGO_FILIAL)).FILIAL;

                //Obter Valor Faturamento
                string _valFaturamento = "0";
                _valFaturamento = (Convert.ToDecimal(f.VALOR)).ToString("###,###,###,###,###0.00");
                totalFat = totalFat + Convert.ToDecimal(_valFaturamento);

                //Obter Total Boleto
                string _valBoleto = "0";
                List<SP_BUSCAR_DESPESA_ALUGUEL_ANUALResult> _boletoAno = baseController.BuscarDespesaAluguelAnual(f.CODIGO_FILIAL, 0, f.COMP.Remove(0, 2));
                if (_boletoAno != null)
                {
                    SP_BUSCAR_DESPESA_ALUGUEL_ANUALResult _boletoMes = _boletoAno.Where(p => p.COMP == f.COMP).SingleOrDefault();

                    if (_boletoMes != null)
                    {
                        _valBoleto = Convert.ToDecimal(_boletoMes.VALOR).ToString("###,###,###,###,###0.00");
                        totalBol = totalBol + Convert.ToDecimal(_valBoleto);
                    }
                }

                //Obter Performance
                string _valPerformance = "0%";
                decimal total = 0;
                if (_valBoleto != "")
                {
                    total = (Convert.ToDecimal(_valBoleto) / Convert.ToDecimal(_valFaturamento)) * 100;
                    _valPerformance = total.ToString("###0.00") + "%";
                }

                _list.Add(new PERFORMANCE { FILIAL = filial, FATURAMENTO = _valFaturamento, BOLETO = _valBoleto, PERF = _valPerformance });
            }

            if (_list != null)
            {
                //CASO ORDENAR POR OUTRAS COLUNAS, ALTERAR...
                if (orderAsc)
                    _list = _list.OrderBy(p => Convert.ToDecimal(p.PERF.Replace("%", ""))).ToList();
                else
                    _list = _list.OrderByDescending(p => Convert.ToDecimal(p.PERF.Replace("%", ""))).ToList();

                gvPerformance.DataSource = _list;
                gvPerformance.DataBind();
            }
        }

        protected void gvPerformance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PERFORMANCE _PERFOMANCE = e.Row.DataItem as PERFORMANCE;

                    coluna += 1;
                    if (_PERFOMANCE != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        decimal total = 0;
                        if (_PERFOMANCE.PERF != null && _PERFOMANCE.PERF != "")
                        {
                            total = Convert.ToDecimal(_PERFOMANCE.PERF.Replace("%", ""));
                            if (total > Convert.ToDecimal("20"))
                            {
                                e.Row.Cells[4].Text = "<span style='color:red'> " + e.Row.Cells[4].Text + " </span>";
                            }
                        }

                        e.Row.Cells[2].Text = "R$ " + e.Row.Cells[2].Text;
                        e.Row.Cells[3].Text = "R$ " + e.Row.Cells[3].Text;
                    }
                }
            }
        }

        protected void gvPerformance_DataBound(object sender, EventArgs e)
        {
            GridViewRow foo = gvPerformance.FooterRow;
            if (foo != null)
            {
                decimal total = 0;

                foo.Cells[1].Text = "Total";
                foo.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                foo.Cells[2].Text = "R$ " + totalFat.ToString("###,###,###,###,###0.00");
                foo.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                foo.Cells[3].Text = "R$ " + totalBol.ToString("###,###,###,###,###0.00");
                foo.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                if (totalBol > 0 && totalFat > 0)
                {
                    total = (totalBol / totalFat) * 100;
                    foo.Cells[4].Text = total.ToString("####0.00") + "%";
                    foo.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void gvPerformance_Sorting(object sender, GridViewSortEventArgs e)
        {
            if ((bool)ViewState["sortDirection"])
                ViewState["sortDirection"] = false;
            else
                ViewState["sortDirection"] = true;

            CarregarPerformanceLoja(e.SortExpression, (bool)ViewState["sortDirection"]);
        }
    }
}
