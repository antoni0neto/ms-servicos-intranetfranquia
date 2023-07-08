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
    public partial class desp_loja_performance_rede : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        #endregion

        protected void btCarregaGrafico_Click(object sender, EventArgs e)
        {
            labErro.Text = "";

            try
            {
                Chart _chart = null;

                //Adicionar chart performance de loja
                _chart = new Chart();
                _chart = CarregarGraficoPerformance(ddlAno.SelectedValue, "Performance da Rede");
                if (_chart != null)
                {
                    pnl1.Controls.Add(_chart);
                    pnl1.Controls.Add(new LiteralControl("<br />"));
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private Chart CarregarGraficoPerformance(string ano, string nome)
        {
            string _serie = "SeriesRede";
            string _chartArea = "chartAreaRede";

            try
            {
                Chart _c = new Chart();

                _c.Titles.Add(new Title(
                                    nome,
                                    Docking.Top,
                                    new Font("Verdana", 10f, FontStyle.Bold),
                                    Color.Black
                ));
                _c.Titles[0].BackColor = Color.Gainsboro;

                Series _s = new Series(_serie);
                _s.ChartType = SeriesChartType.Line;
                _c.Series.Add(_s);

                ChartArea _ca = new ChartArea(_chartArea);
                _ca.AxisY.LineColor = System.Drawing.Color.Black;
                _ca.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisY.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisY.MinorGrid.Enabled = false;

                _ca.AxisX.LineColor = System.Drawing.Color.Black;
                _ca.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisX.MinorGrid.Enabled = false;

                _c.ChartAreas.Add(_ca);

                List<SP_BUSCAR_DESPESA_ALUGUEL_REDEResult> _aluguel = baseController.BuscarDespesaAluguelRede(ano);

                int qtde_linhas = _aluguel.Count;

                if (qtde_linhas > 0)
                {
                    string[] valoresx = new string[qtde_linhas + 1]; // + 1 para o TOTAL
                    decimal[] valoresy = new decimal[qtde_linhas + 1]; // + 1 para o TOTAL

                    List<SP_BUSCAR_FATURAMENTO_REDEResult> _faturamento = baseController.BuscarFaturamentoRede(ano);
                    SP_BUSCAR_FATURAMENTO_REDEResult f = null;

                    int vx = 0;
                    decimal ySomatorioAluguel = 0;
                    decimal ySomatorioFaturamento = 0;
                    for (vx = 0; vx < qtde_linhas; vx++)
                    {
                        f = new SP_BUSCAR_FATURAMENTO_REDEResult();
                        f = _faturamento.Where(p => p.COMP == _aluguel[vx].COMP).SingleOrDefault();

                        valoresx[vx] = Convert.ToString(_aluguel[vx].MES_EXTENSO);
                        if (f != null)
                            valoresy[vx] = Math.Round((Convert.ToDecimal(_aluguel[vx].VALOR / ((f.VALOR > 0) ? f.VALOR : 1)) * 100), 2);
                        else
                            valoresy[vx] = Math.Round((Convert.ToDecimal(_aluguel[vx].VALOR / (_aluguel[vx].VALOR)) * 100), 2);

                        if (f != null)
                        {
                            ySomatorioAluguel += Convert.ToDecimal(_aluguel[vx].VALOR);
                            ySomatorioFaturamento += Convert.ToDecimal(f.VALOR);
                        }
                    }

                    valoresx[vx] = "Média Total";
                    valoresy[vx] = Math.Round(((ySomatorioAluguel / ySomatorioFaturamento) * 100), 2);

                    _c.Series[_serie].XValueMember = "MES_EXTENSO";
                    _c.Series[_serie].YValueMembers = "VALOR";

                    _c.Series[_serie].Points.DataBindXY(valoresx, valoresy);
                    _c.Series[_serie].ChartType = SeriesChartType.Line;
                    _c.Series[_serie].IsValueShownAsLabel = true;
                    _c.Series[_serie].Label = "#VALY" + "%";
                    _c.Series[_serie].MarkerSize = 4;
                    _c.Series[_serie].MarkerStyle = MarkerStyle.Cross;
                    _c.Series[_serie].Color = Color.DarkBlue;

                    _c.Width = Unit.Pixel(1425);
                    _c.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(0);
                    decimal valY = 0;
                    valY = valoresy.Where(p => p > 100).Take(1).SingleOrDefault();

                    if (valY <= 100)
                        _c.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(100);
                    else
                        _c.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(valY);
                    _c.ChartAreas[0].AxisY.Interval = 10;
                    _c.ChartAreas[0].AxisY.LabelStyle.Format = "{#'%'}";
                    _c.Height = Unit.Pixel(300);

                    _c.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;

                    _c.ChartAreas[0].AxisX.Interval = 1;

                    LabelStyle _style = new LabelStyle();
                    _style.Font = new Font("Verdana", 9f, FontStyle.Regular);
                    _c.ChartAreas[0].AxisX.LabelStyle = _style;

                    _c.Series[_serie]["PieLabelStyle"] = "inside";

                    _c.ChartAreas[_chartArea].Area3DStyle.Enable3D = false;

                    _c.DataBind();
                    return _c;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
