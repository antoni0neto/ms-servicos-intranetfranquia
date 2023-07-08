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
    public partial class desp_loja_grafico : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                CarregaDropDownListContas();
                CarregarDataAno();
            }
        }

        private void CarregaDropDownListContas()
        {
            ddlContas.DataSource = baseController.BuscaAluguelDespesas();
            ddlContas.DataBind();
        }
        private void CarregaDropDownListFilial()
        {
            ddlFilial.DataSource = baseController.BuscaFiliais();
            ddlFilial.DataBind();
        }
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

        protected void ddlContas_DataBound(object sender, EventArgs e)
        {
            ddlContas.Items.Add(new ListItem("Todas", ""));
            ddlContas.SelectedValue = "";
        }
        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void btCarregaGrafico_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
            {
                labErro.Text = "Selecione uma Filial.";
                return;
            }

            try
            {
                Chart _chart = null;
                List<ACOMPANHAMENTO_ALUGUEL_DESPESA> _despesa = new List<ACOMPANHAMENTO_ALUGUEL_DESPESA>();

                if (ddlContas.SelectedValue != "" && ddlContas.SelectedValue != "0")
                    _despesa.Add(baseController.BuscaAluguelDespesa(Convert.ToInt32(ddlContas.SelectedValue)));
                else
                    _despesa = baseController.BuscaAluguelDespesas();

                //Adicionar chart performance de loja
                _chart = new Chart();
                _chart = CarregarGraficoPerformance(ddlFilial.SelectedValue, 0, ddlAno.SelectedValue, "Performance de Loja");
                if (_chart != null)
                {
                    pnl1.Controls.Add(_chart);
                    pnl1.Controls.Add(new LiteralControl("<br />"));
                }

                //Adicionar chart boleto (TOTAL DE DESPESAS)
                _chart = new Chart();
                _chart = CarregarGraficoDinamico(ddlFilial.SelectedValue, 0, ddlAno.SelectedValue, "Boleto");
                if (_chart != null)
                {
                    pnl1.Controls.Add(_chart);
                    pnl1.Controls.Add(new LiteralControl("<br />"));
                }

                foreach (ACOMPANHAMENTO_ALUGUEL_DESPESA d in _despesa)
                {
                    _chart = new Chart();
                    _chart = CarregarGraficoDinamico(ddlFilial.SelectedValue, d.CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA, ddlAno.SelectedValue, d.DESCRICAO);
                    if (_chart != null)
                    {
                        pnl1.Controls.Add(_chart);
                        pnl1.Controls.Add(new LiteralControl("<br />"));
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private Chart CarregarGraficoDinamico(string filial, int conta, string ano, string nome)
        {
            string _serie = "Series" + conta;
            string _chartArea = "chartArea" + conta;

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

                List<SP_BUSCAR_DESPESA_ALUGUEL_ANUALResult> _aluguel = baseController.BuscarDespesaAluguelAnual(filial, conta, ano);

                int qtde_linhas = _aluguel.Count;

                if (qtde_linhas > 0)
                {
                    string[] valoresx = new string[qtde_linhas + 1]; // + 1 para TOTAL
                    decimal[] valoresy = new decimal[qtde_linhas + 1];// + 1 para TOTAL

                    int vx = 0;
                    decimal ySomatorioAluguel = 0;
                    for (vx = 0; vx < qtde_linhas; vx++)
                    {
                        valoresx[vx] = Convert.ToString(_aluguel[vx].MES_EXTENSO);
                        valoresy[vx] = Convert.ToDecimal(_aluguel[vx].VALOR);

                        //Somatorio
                        ySomatorioAluguel += valoresy[vx];
                    }

                    valoresx[vx] = "Média Total";
                    valoresy[vx] = (ySomatorioAluguel / qtde_linhas);

                    _c.Series[_serie].XValueMember = "MES_EXTENSO";
                    _c.Series[_serie].YValueMembers = "VALOR";

                    _c.Series[_serie].Points.DataBindXY(valoresx, valoresy);
                    _c.Series[_serie].ChartType = SeriesChartType.Line;
                    _c.Series[_serie].IsValueShownAsLabel = true;
                    _c.Series[_serie].Label = "#VALY{R$#.###}";
                    _c.Series[_serie].MarkerSize = 4;
                    _c.Series[_serie].MarkerStyle = MarkerStyle.Cross;
                    _c.Series[_serie].Color = Color.DarkBlue;

                    _c.Width = Unit.Pixel(1425);
                    if (conta == 7 || conta == 8 || conta == 10 || conta == 0)
                    {
                        _c.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(valoresy.Min() - 1000);
                        _c.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(valoresy.Max() + 1000);
                        _c.ChartAreas[0].AxisY.Interval = 1000;
                        _c.Height = Unit.Pixel(400);
                    }
                    else
                    {
                        _c.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(valoresy.Min() - 1000);
                        _c.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(valoresy.Max() + 1000);
                        _c.ChartAreas[0].AxisY.Interval = 200;
                        _c.Height = Unit.Pixel(300);
                    }

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
        private Chart CarregarGraficoPerformance(string filial, int conta, string ano, string nome)
        {
            string _serie = "SeriesP" + conta;
            string _chartArea = "chartAreaP" + conta;

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

                List<SP_BUSCAR_DESPESA_ALUGUEL_ANUALResult> _aluguel = baseController.BuscarDespesaAluguelAnual(filial, conta, ano);

                int qtde_linhas = _aluguel.Count;

                if (qtde_linhas > 0)
                {
                    string[] valoresx = new string[qtde_linhas + 1]; // + 1 para o TOTAL
                    decimal[] valoresy = new decimal[qtde_linhas + 1]; // + 1 para o TOTAL

                    List<SP_BUSCAR_FATURAMENTO_ANUALResult> _faturamento = baseController.BuscarFaturamentoAnual(filial, ano);
                    SP_BUSCAR_FATURAMENTO_ANUALResult f = null;
                    decimal? valorFilialAntiga = 0;
                    decimal ySomatorioAluguel = 0;
                    decimal ySomatorioFaturamento = 0;
                    int vx = 0;
                    for (vx = 0; vx < qtde_linhas; vx++)
                    {
                        f = new SP_BUSCAR_FATURAMENTO_ANUALResult();
                        f = _faturamento.Where(p => p.COMP == _aluguel[vx].COMP).SingleOrDefault();

                        FILIAIS_DE_PARA _dePara = baseController.BuscaFilialDePara(filial);
                        if (_dePara != null)
                        {
                            var _fatFilialAntiga = baseController.BuscarFaturamentoAnual(_dePara.DE, ano).Where(p => p.COMP == _aluguel[vx].COMP).SingleOrDefault();
                            if (_fatFilialAntiga != null)
                            {
                                valorFilialAntiga = _fatFilialAntiga.VALOR;
                            }
                        }

                        valoresx[vx] = Convert.ToString(_aluguel[vx].MES_EXTENSO);
                        if (f != null || valorFilialAntiga > 0)
                        {
                            valorFilialAntiga += ((f != null) ? f.VALOR : 0);
                            valoresy[vx] = Math.Round((Convert.ToDecimal(_aluguel[vx].VALOR / ((valorFilialAntiga > 0) ? valorFilialAntiga : 1)) * 100), 2);
                        }
                        else
                        {
                            valoresy[vx] = Math.Round((Convert.ToDecimal(_aluguel[vx].VALOR / (_aluguel[vx].VALOR)) * 100), 2);
                        }

                        //total
                        if (f != null)
                        {
                            ySomatorioAluguel += Convert.ToDecimal(_aluguel[vx].VALOR);
                            ySomatorioFaturamento += Convert.ToDecimal(valorFilialAntiga);
                        }

                        valorFilialAntiga = 0;
                    }

                    valoresx[vx] = "Média Total";
                    valoresy[vx] = Math.Round(((ySomatorioAluguel / ((ySomatorioFaturamento > 0) ? ySomatorioFaturamento : 1)) * 100), 2);

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
