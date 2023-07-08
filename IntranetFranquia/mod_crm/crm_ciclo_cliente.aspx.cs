using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class crm_ciclo_cliente : System.Web.UI.Page
    {
        CRMController crmController = new CRMController();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "crm_menu.aspx";

                CarregarTrimestre();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarTrimestre()
        {
            var tri = crmController.ObterTrimestre().Where(p => p.ATIVO == true).ToList();

            tri.Insert(0, new TRIMESTRE { DATA = new DateTime(1900, 1, 1), TRI = "Selecione" });
            ddlTri.DataSource = tri;
            ddlTri.DataBind();
        }

        private List<SP_OBTER_CRM_CICLOCLIENTEResult> ObterCicloCliente()
        {
            var ciclo = crmController.ObterCRMCicloCliente(Convert.ToDateTime(ddlTri.SelectedValue));
            return ciclo;
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlTri.SelectedValue == "01/01/1900 00:00:00")
                {
                    labErro.Text = "Selecione o trimestre para comparação...";
                    return;
                }

                var ciclos = new List<SP_OBTER_CRM_CICLOCLIENTEResult>();
                var ciclo = ObterCicloCliente();

                var cicloAtivo = ciclo.Where(p => p.COL != 5).ToList();
                var cicloInativo = ciclo.Where(p => p.COL == 5).SingleOrDefault();


                cicloAtivo.Add(new SP_OBTER_CRM_CICLOCLIENTEResult { COL = 99 });
                cicloAtivo.AddRange(cicloAtivo.GroupBy(p => new { CICLO = "CLIENTES ATIVOS" }).Select(x => new SP_OBTER_CRM_CICLOCLIENTEResult
                {
                    COL = 66,
                    CICLO = "Clientes Ativos",
                    QTDE_TRI5 = x.Sum(j => j.QTDE_TRI5),
                    PORC_TRI5 = (100.00M - cicloInativo.PORC_TRI5),
                    QTDE_TRI4 = x.Sum(j => j.QTDE_TRI4),
                    PORC_TRI4 = (100.00M - cicloInativo.PORC_TRI4),
                    QTDE_TRI3 = x.Sum(j => j.QTDE_TRI3),
                    PORC_TRI3 = (100.00M - cicloInativo.PORC_TRI3),
                    QTDE_TRI2 = x.Sum(j => j.QTDE_TRI2),
                    PORC_TRI2 = (100.00M - cicloInativo.PORC_TRI2),
                    QTDE_TRI1 = x.Sum(j => j.QTDE_TRI1),
                    PORC_TRI1 = (100.00M - cicloInativo.PORC_TRI1)
                }).ToList());

                ciclos.AddRange(cicloAtivo);

                ciclos.Add(new SP_OBTER_CRM_CICLOCLIENTEResult { COL = 99 });
                ciclos.Add(cicloInativo);
                ciclos.Add(new SP_OBTER_CRM_CICLOCLIENTEResult { COL = 99 });

                ciclos.Add(new SP_OBTER_CRM_CICLOCLIENTEResult
                {
                    COL = 101,
                    CICLO = "Total",
                    QTDE_TRI5 = (cicloAtivo.Where(p => p.COL == 66).Sum(p => p.QTDE_TRI5) + cicloInativo.QTDE_TRI5),
                    PORC_TRI5 = 100,
                    QTDE_TRI4 = (cicloAtivo.Where(p => p.COL == 66).Sum(p => p.QTDE_TRI4) + cicloInativo.QTDE_TRI4),
                    PORC_TRI4 = 100,
                    QTDE_TRI3 = (cicloAtivo.Where(p => p.COL == 66).Sum(p => p.QTDE_TRI3) + cicloInativo.QTDE_TRI3),
                    PORC_TRI3 = 100,
                    QTDE_TRI2 = (cicloAtivo.Where(p => p.COL == 66).Sum(p => p.QTDE_TRI2) + cicloInativo.QTDE_TRI2),
                    PORC_TRI2 = 100,
                    QTDE_TRI1 = (cicloAtivo.Where(p => p.COL == 66).Sum(p => p.QTDE_TRI1) + cicloInativo.QTDE_TRI1),
                    PORC_TRI1 = 100

                });
                ciclos.Add(new SP_OBTER_CRM_CICLOCLIENTEResult { COL = 99 });

                gvCiclo.DataSource = ciclos;
                gvCiclo.DataBind();

                var cicloDiff = ciclos.Where(p => p.COL == 101).SingleOrDefault();

                labDiffP.Text = ((((cicloDiff.QTDE_TRI1 * 1.000M) / (cicloDiff.QTDE_TRI5 * 1.000M)) - 1) * 100.00M).ToString("###,###,###,##0.00") + "%";

                var trimestre = crmController.ObterTrimestre().Where(p => p.DATA <= Convert.ToDateTime(ddlTri.SelectedValue)).OrderByDescending(p => p.DATA).Take(5).ToList();
                int i = 1;
                foreach (var tri in trimestre)
                {
                    if (i == 1) labTri1.Text = tri.TRI;
                    else if (i == 2) labTri2.Text = tri.TRI;
                    else if (i == 3) labTri3.Text = tri.TRI;
                    else if (i == 4) labTri4.Text = tri.TRI;
                    else if (i == 5) labTri5.Text = tri.TRI;

                    i = i + 1;
                }

                GerarGrafico(ciclos.Where(p => p.COL != 99 && p.COL != 101).ToList(), trimestre);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void gvCiclo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CRM_CICLOCLIENTEResult ciclo = e.Row.DataItem as SP_OBTER_CRM_CICLOCLIENTEResult;

                    if (ciclo.COL != 99)
                    {
                        Literal litStatus = e.Row.FindControl("litStatus") as Literal;
                        litStatus.Text = ciclo.CICLO;

                        Literal litQtdeCliente5 = e.Row.FindControl("litQtdeCliente5") as Literal;
                        litQtdeCliente5.Text = ciclo.QTDE_TRI5.ToString("###,###,###,###,###");
                        Literal litQtdeCliente5Porc = e.Row.FindControl("litQtdeCliente5Porc") as Literal;
                        litQtdeCliente5Porc.Text = Convert.ToDecimal(ciclo.PORC_TRI5).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente4 = e.Row.FindControl("litQtdeCliente4") as Literal;
                        litQtdeCliente4.Text = ciclo.QTDE_TRI4.ToString("###,###,###,###,###");
                        Literal litQtdeCliente4Porc = e.Row.FindControl("litQtdeCliente4Porc") as Literal;
                        litQtdeCliente4Porc.Text = Convert.ToDecimal(ciclo.PORC_TRI4).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente3 = e.Row.FindControl("litQtdeCliente3") as Literal;
                        litQtdeCliente3.Text = ciclo.QTDE_TRI3.ToString("###,###,###,###,###");
                        Literal litQtdeCliente3Porc = e.Row.FindControl("litQtdeCliente3Porc") as Literal;
                        litQtdeCliente3Porc.Text = Convert.ToDecimal(ciclo.PORC_TRI3).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente2 = e.Row.FindControl("litQtdeCliente2") as Literal;
                        litQtdeCliente2.Text = ciclo.QTDE_TRI2.ToString("###,###,###,###,###");
                        Literal litQtdeCliente2Porc = e.Row.FindControl("litQtdeCliente2Porc") as Literal;
                        litQtdeCliente2Porc.Text = Convert.ToDecimal(ciclo.PORC_TRI2).ToString("###,##0.00") + "%";

                        Literal litQtdeCliente1 = e.Row.FindControl("litQtdeCliente1") as Literal;
                        litQtdeCliente1.Text = ciclo.QTDE_TRI1.ToString("###,###,###,###,###");
                        Literal litQtdeCliente1Porc = e.Row.FindControl("litQtdeCliente1Porc") as Literal;
                        litQtdeCliente1Porc.Text = Convert.ToDecimal(ciclo.PORC_TRI1).ToString("###,##0.00") + "%";
                    }

                    if (ciclo.COL == 99)
                    {
                        e.Row.Height = Unit.Pixel(6);
                        e.Row.BackColor = Color.LightGray;
                    }


                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCiclo_DataBound(object sender, EventArgs e)
        {
        }

        private void GerarGrafico(List<SP_OBTER_CRM_CICLOCLIENTEResult> ciclos, List<TRIMESTRE> tri)
        {
            Chart chart = new Chart();
            chart = ObterResultadoGrafico(ciclos, tri);
            if (chart != null)
            {
                pnlGraficoCiclo.Controls.Add(chart);
            }
        }
        private Chart ObterResultadoGrafico(List<SP_OBTER_CRM_CICLOCLIENTEResult> ciclos, List<TRIMESTRE> tri)
        {

            string _chartArea = "chartArea" + "1";

            try
            {
                Chart chart = new Chart();

                chart.Width = Unit.Pixel(1425);
                chart.Height = Unit.Pixel(500);

                chart.Titles.Add(new Title(
                                    "Comparação por Trimestre",
                                    Docking.Top,
                                    new System.Drawing.Font("Verdana", 10f, FontStyle.Bold),
                                    System.Drawing.Color.Black
                ));
                chart.Titles[0].BackColor = System.Drawing.Color.Gainsboro;

                ChartArea _ca = new ChartArea(_chartArea);
                _ca.AxisY.LineColor = System.Drawing.Color.Black;
                _ca.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisY.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisY.MinorGrid.Enabled = false;

                _ca.AxisX.LineColor = System.Drawing.Color.Black;
                _ca.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
                _ca.AxisX.MinorGrid.Enabled = false;

                chart.ChartAreas.Add(_ca);

                int qtde_linhas = ciclos.Count;

                if (qtde_linhas > 0)
                {

                    string[] valoresx = new string[qtde_linhas];
                    //SERIE ATUAL
                    decimal[] valoresyTRI5 = new decimal[qtde_linhas];
                    decimal[] valoresyTRI4 = new decimal[qtde_linhas];
                    decimal[] valoresyTRI3 = new decimal[qtde_linhas];
                    decimal[] valoresyTRI2 = new decimal[qtde_linhas];
                    decimal[] valoresyTRI1 = new decimal[qtde_linhas];

                    int vx = 0;
                    for (vx = 0; vx < qtde_linhas; vx++)
                    {
                        valoresx[vx] = ciclos[vx].CICLO;
                        valoresyTRI5[vx] = Convert.ToDecimal(ciclos[vx].PORC_TRI5.ToString().Replace("%", "").Replace(".", ","));
                        valoresyTRI4[vx] = Convert.ToDecimal(ciclos[vx].PORC_TRI4.ToString().Replace("%", "").Replace(".", ","));
                        valoresyTRI3[vx] = Convert.ToDecimal(ciclos[vx].PORC_TRI3.ToString().Replace("%", "").Replace(".", ","));
                        valoresyTRI2[vx] = Convert.ToDecimal(ciclos[vx].PORC_TRI2.ToString().Replace("%", "").Replace(".", ","));
                        valoresyTRI1[vx] = Convert.ToDecimal(ciclos[vx].PORC_TRI1.ToString().Replace("%", "").Replace(".", ","));
                    }

                    //LEGENDA
                    chart.Legends.Add(new Legend("LegendCont"));
                    chart.Legends["LegendCont"].Alignment = StringAlignment.Center;

                    //SERIE VALORES ATUAIS
                    int i = 1;
                    foreach (var t in tri)
                    {
                        Series serieAtual = new Series(t.TRI);
                        serieAtual.ChartType = SeriesChartType.Line;
                        chart.Series.Add(serieAtual);

                        if (i == 1)
                        {
                            chart.Series[t.TRI].Points.DataBindXY(valoresx, valoresyTRI1);
                            chart.Series[t.TRI].Color = System.Drawing.Color.LightCyan;
                        }
                        else if (i == 2)
                        {
                            chart.Series[t.TRI].Points.DataBindXY(valoresx, valoresyTRI2);
                            chart.Series[t.TRI].Color = System.Drawing.Color.PaleTurquoise;
                        }
                        else if (i == 3)
                        {
                            chart.Series[t.TRI].Points.DataBindXY(valoresx, valoresyTRI3);
                            chart.Series[t.TRI].Color = System.Drawing.Color.LightSteelBlue;
                        }
                        else if (i == 4)
                        {
                            chart.Series[t.TRI].Points.DataBindXY(valoresx, valoresyTRI4);
                            chart.Series[t.TRI].Color = System.Drawing.Color.SteelBlue;
                        }
                        else if (i == 5)
                        {
                            chart.Series[t.TRI].Points.DataBindXY(valoresx, valoresyTRI5);
                            chart.Series[t.TRI].Color = System.Drawing.Color.DarkBlue;
                        }

                        chart.Series[t.TRI].ChartType = SeriesChartType.Column;
                        chart.Series[t.TRI].IsValueShownAsLabel = true;
                        chart.Series[t.TRI].Label = "" + "#VALY" + "%";
                        chart.Series[t.TRI].MarkerSize = 8;
                        chart.Series[t.TRI].BorderWidth = 4;
                        chart.Series[t.TRI].MarkerStyle = MarkerStyle.Circle;
                        chart.Series[t.TRI].XValueType = ChartValueType.Double;
                        chart.Series[t.TRI].Font = new System.Drawing.Font("Verdana", 6f, FontStyle.Bold);
                        chart.Series[t.TRI].Legend = "LegendCont";
                        chart.Series[t.TRI].IsVisibleInLegend = true;

                        i = i + 1;
                    }

                    //EIXO X
                    LabelStyle styleX = new LabelStyle();
                    styleX.Font = new System.Drawing.Font("Verdana", 9f, FontStyle.Regular);
                    //styleX.Format = "dd/MMM";
                    chart.ChartAreas[0].AxisX.LabelStyle = styleX;
                    chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto;
                    chart.ChartAreas[0].AxisX.Title = "Status";
                    chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Verdana", 9f, FontStyle.Bold);
                    chart.ChartAreas[0].AxisX.IntervalOffset = 1;


                    //EIXO Y
                    LabelStyle styleY = new LabelStyle();
                    styleY.Font = new System.Drawing.Font("Verdana", 9f, FontStyle.Regular);
                    styleY.Format = "{###,###,##0.00}%";
                    chart.ChartAreas[0].AxisY.LabelStyle = styleY;
                    chart.ChartAreas[0].AxisY.Title = "Valores em % ";
                    chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 9f, FontStyle.Bold);
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 80;
                    chart.ChartAreas[0].AxisY.Interval = 5;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;

                    chart.ChartAreas[_chartArea].Area3DStyle.Enable3D = false;
                    chart.DataBind();

                    return chart;
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
