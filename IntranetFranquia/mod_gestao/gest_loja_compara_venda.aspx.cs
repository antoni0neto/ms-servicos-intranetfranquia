using DAL;
using GemBox.Spreadsheet;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class gest_loja_compara_venda : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        public decimal totalVendaAtual = 0;
        public decimal totalVendaAnterior = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region "DADOS INICIAIS"
        protected void CalendarDataInicioAtual_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicioAtual.Text = CalendarDataInicioAtual.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFimAtual_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFimAtual.Text = CalendarDataFimAtual.SelectedDate.ToString("dd/MM/yyyy");
        }
        #endregion  

        private void CarregarVendas()
        {
            string dataInicioAtual = baseController.AjustaData(txtDataInicioAtual.Text);
            string dataFimAtual = baseController.AjustaData(txtDataFimAtual.Text);

            int anoAnterior = Convert.ToInt32(dataInicioAtual.Substring(0, 4)) - 1;

            string dataInicioAnterior = anoAnterior.ToString() + dataInicioAtual.Substring(4, 4);
            string dataFimAnterior = anoAnterior.ToString() + dataFimAtual.Substring(4, 4);

            List<Vendas> listaVendas = desempenhoController.BuscaVendasAtualAnterior(dataInicioAtual, dataFimAtual, dataInicioAnterior, dataFimAnterior, false);

            List<Vendas> listaVendasValor = new List<Vendas>();
            List<Vendas> listaVendasSemValor = new List<Vendas>();

            foreach (Vendas item in listaVendas)
            {
                if (item.ValorVendaAnterior == 0 || item.ValorVendaAtual == 0)
                    listaVendasSemValor.Add(item);
                else
                    listaVendasValor.Add(item);
            }

            listaVendasValor = listaVendasValor.OrderByDescending(z => z.Filial).ToList();
            listaVendasSemValor = listaVendasSemValor.OrderByDescending(z => z.Filial).ToList();

            gvVendas.DataSource = listaVendasValor;
            gvVendas.DataBind();

            gvFilialSemVenda.DataSource = listaVendasSemValor;
            gvFilialSemVenda.DataBind();

        }

        protected void btPesquisarVendas_Click(object sender, EventArgs e)
        {

            try
            {
                labMsg.Text = "";

                if (txtDataInicioAtual.Text.Equals("") || txtDataFimAtual.Text == "")
                {
                    labMsg.Text = "Informe o período.";
                    return;
                }

                CarregarVendas();

                btGerarExcel.Enabled = true;

            }
            catch (Exception ex)
            {

                labMsg.Text = ex.Message;
            }

        }
        protected void gvVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Vendas vendas = e.Row.DataItem as Vendas;

            if (vendas != null)
            {
                Literal literalVendasAtual = e.Row.FindControl("LiteralVendasAtual") as Literal;
                literalVendasAtual.Text = Convert.ToDecimal(vendas.ValorVendaAtual).ToString("N2");

                Literal literalVendasAnterior = e.Row.FindControl("LiteralVendasAnterior") as Literal;
                literalVendasAnterior.Text = Convert.ToDecimal(vendas.ValorVendaAnterior).ToString("N2");

                Literal literalDiferenca = e.Row.FindControl("LiteralDiferenca") as Literal;
                literalDiferenca.Text = Convert.ToDecimal(vendas.ValorVendaAtual - vendas.ValorVendaAnterior).ToString("N2");

                Literal literalPercentual = e.Row.FindControl("LiteralPercentual") as Literal;
                if (vendas.ValorVendaAnterior > 0)
                {
                    literalPercentual.Text = Convert.ToDecimal(((vendas.ValorVendaAtual / vendas.ValorVendaAnterior) - 1) * 100).ToString("N0") + "%";

                    totalVendaAtual += vendas.ValorVendaAtual;
                    totalVendaAnterior += vendas.ValorVendaAnterior;
                }

                if (vendas.Filial.ToLower() == "handbook online")
                    e.Row.BackColor = System.Drawing.Color.FloralWhite;

                GridView gvGrafico = e.Row.FindControl("gvGrafico") as GridView;
                if (gvGrafico != null)
                {

                    List<CHARTCOMPVENDA> chartVenda = new List<CHARTCOMPVENDA>();

                    var dataIniAtu = baseController.AjustaData(txtDataInicioAtual.Text);
                    var dataFimAtu = baseController.AjustaData(txtDataFimAtual.Text);
                    var anoAnterior = Convert.ToInt32(dataIniAtu.Substring(0, 4)) - 1;

                    var dataIniAnt = anoAnterior.ToString() + dataIniAtu.Substring(4, 4);
                    var dataFimAnt = anoAnterior.ToString() + dataFimAtu.Substring(4, 4);

                    chartVenda.Add(new CHARTCOMPVENDA
                    {
                        codigoFilial = vendas.CodigoFilial,
                        dataIniAtual = dataIniAtu,
                        dataFimAtual = dataFimAtu,
                        dataIniAnt = dataIniAnt,
                        dataFimAnt = dataFimAnt
                    });

                    gvGrafico.DataSource = chartVenda;
                    gvGrafico.DataBind();
                }

            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }

        }
        protected void gvVendas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVendas.FooterRow;

            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[3].Text = totalVendaAtual.ToString("N2");
                footer.Cells[4].Text = totalVendaAnterior.ToString("N2");
                footer.Cells[5].Text = (totalVendaAtual - totalVendaAnterior).ToString("N2");

                if (totalVendaAnterior > 0)
                    footer.Cells[6].Text = (((totalVendaAtual / totalVendaAnterior) - 1) * 100).ToString("N0") + "%";

                if (footer.Cells[6].Text.Substring(0, 1).Equals("-"))
                    footer.Cells[6].BackColor = System.Drawing.Color.Red;
                else
                    footer.Cells[6].BackColor = System.Drawing.Color.AliceBlue;
            }

            foreach (GridViewRow item in gvVendas.Rows)
            {
                Literal literalPercentual = item.FindControl("LiteralPercentual") as Literal;

                if (!literalPercentual.Text.Equals(""))
                {
                    if (literalPercentual.Text.Substring(0, 1).Equals("-"))
                        item.Cells[6].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[6].BackColor = System.Drawing.Color.AliceBlue;
                }
            }
        }

        protected void btGerarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                var diretorioArquivo = GerarExcel();

                //Enviar Email
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();

                var assunto = "[Intranet] - COMPARÁVEIS - " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " - " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.ANEXO = diretorioArquivo;
                email.MENSAGEM = "Anexo comparáveis das lojas no período de " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " à " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(51, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);

                email.DESTINATARIOS = destinatario;

                if (destinatario.Count > 0)
                    email.EnviarEmail();


                labMsg.Text = "E-Mail Enviado com Sucesso.";
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }
        private string GerarExcel()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile ef = new ExcelFile();
            ExcelWorksheet xlWorkSheet = ef.Worksheets.Add("FINAL");

            DateTime dataIni = Convert.ToDateTime(txtDataInicioAtual.Text);
            DateTime dataFim = Convert.ToDateTime(txtDataFimAtual.Text);
            string mesExtenso = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(dataIni.Month).ToUpper();

            xlWorkSheet.Cells[2, 0].Value = dataIni.Day + " A " + dataFim.Day + " DE " + mesExtenso;
            xlWorkSheet.Cells[2, 0].Row.Height = 34 * 20;
            xlWorkSheet.Cells[2, 0].Style.Font.Size = 22 * 20;
            var titleRange = xlWorkSheet.Cells.GetSubrangeAbsolute(2, 0, 2, 4);
            titleRange.Merged = true;
            titleRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            titleRange.Style.Font.Weight = ExcelFont.BoldWeight;

            xlWorkSheet.Cells[3, 0].Value = "Filial";
            xlWorkSheet.Cells[3, 1].Value = dataIni.Year.ToString();
            xlWorkSheet.Cells[3, 2].Value = (dataIni.Year - 1).ToString();
            xlWorkSheet.Cells[3, 3].Value = "Diferença";
            xlWorkSheet.Cells[3, 4].Value = "% Crescimento";

            var columnHeadingsRange = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 0, 3, 4);
            columnHeadingsRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            columnHeadingsRange.Style.Font.Weight = ExcelFont.BoldWeight;
            columnHeadingsRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            columnHeadingsRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            columnHeadingsRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            xlWorkSheet.Rows[3].Height = 50 * 20;
            xlWorkSheet.Columns[0].Width = 35 * 256;
            xlWorkSheet.Columns[1].Width = 20 * 256;
            xlWorkSheet.Columns[2].Width = 20 * 256;
            xlWorkSheet.Columns[3].Width = 20 * 256;
            xlWorkSheet.Columns[4].Width = 20 * 256;


            int linha = 4;
            decimal vendaAtual = 0;
            decimal vendaAnterior = 0;
            foreach (GridViewRow row in gvVendas.Rows)
            {

                //Filial
                xlWorkSheet.Cells[linha, 0].Value = row.Cells[2].Text;
                xlWorkSheet.Cells[linha, 0].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                Literal litVendaAtual = row.FindControl("LiteralVendasAtual") as Literal;
                xlWorkSheet.Cells[linha, 1].Value = Convert.ToDecimal(litVendaAtual.Text);
                xlWorkSheet.Cells[linha, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                xlWorkSheet.Cells[linha, 1].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                Literal litVendaAnterior = row.FindControl("LiteralVendasAnterior") as Literal;
                xlWorkSheet.Cells[linha, 2].Value = Convert.ToDecimal(litVendaAnterior.Text);
                xlWorkSheet.Cells[linha, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 2].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                xlWorkSheet.Cells[linha, 2].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                Literal litDiff = row.FindControl("LiteralDiferenca") as Literal;
                xlWorkSheet.Cells[linha, 3].Value = Convert.ToDecimal(litDiff.Text);
                xlWorkSheet.Cells[linha, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 3].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                xlWorkSheet.Cells[linha, 3].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litDiff.Text.Contains("-"))
                    xlWorkSheet.Cells[linha, 3].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                Literal litCrescimento = row.FindControl("LiteralPercentual") as Literal;
                xlWorkSheet.Cells[linha, 4].Value = litCrescimento.Text;
                xlWorkSheet.Cells[linha, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 4].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                xlWorkSheet.Cells[linha, 4].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litCrescimento.Text.Contains("-"))
                    xlWorkSheet.Cells[linha, 4].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                linha += 1;
                vendaAtual = vendaAtual + Convert.ToDecimal(litVendaAtual.Text);
                vendaAnterior = vendaAnterior + Convert.ToDecimal(litVendaAnterior.Text);

            }

            //FILTROS E CONGELAMENTO
            xlWorkSheet.Panes = new WorksheetPanes(PanesState.Frozen, 0, 4, "A5", PanePosition.BottomLeft);
            var filterRange = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 0, linha, 4);
            filterRange.Filter(true).SortBy(0, false).Apply();

            //CONTROLE DE TOTAL
            xlWorkSheet.Cells[linha, 0].Value = "Totais";
            xlWorkSheet.Cells[linha, 1].Formula = "=SUBTOTAL(9, B5:B" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 2].Formula = "=SUBTOTAL(9, C5:C" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 3].Formula = "=SUBTOTAL(9, D5:D" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 4].Formula = "=(B" + (linha + 1) + "/C" + (linha + 1) + ")-1";
            xlWorkSheet.Cells[linha, 4].Style.NumberFormat = "###,##%";

            var footerRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 4);
            footerRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            footerRange.Style.Font.Weight = ExcelFont.BoldWeight;
            footerRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            footerRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            footerRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            string pCrescimento = (((vendaAtual / vendaAnterior) - 1) * 100).ToString("N0") + "%";
            if (pCrescimento.Contains("-"))
                xlWorkSheet.Cells[linha, 4].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            else
                xlWorkSheet.Cells[linha, 4].Style.FillPattern.SetSolid(System.Drawing.Color.YellowGreen);

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 1, linha, 1).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 2, linha, 2).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 3, linha, 3).Style.NumberFormat = "R$ ###,###,##0.00";

            string nomeArquivo = dataIni.Day.ToString() + "-A-" + dataFim.Day.ToString() + "-" + mesExtenso + "-COMPARAVEIS.xlsx";
            string diretorioArquivo = Server.MapPath("..") + "\\Excel\\" + nomeArquivo;

            ef.Save(diretorioArquivo);

            return diretorioArquivo;
        }

        protected void gvGrafico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CHARTCOMPVENDA chartVenda = e.Row.DataItem as CHARTCOMPVENDA;

                    Panel pnlGraficoResultado = e.Row.FindControl("pnlGraficoResultado") as Panel;
                    GerarGrafico(pnlGraficoResultado, chartVenda.codigoFilial, chartVenda.dataIniAtual, chartVenda.dataFimAtual, chartVenda.dataIniAnt, chartVenda.dataFimAnt);

                }
            }
        }
        private void GerarGrafico(Panel pnlGraficoResultado, string codigoFilial, string dataIniAtual, string dataFimAutal, string dataIniAnt, string dataFimAnt)
        {
            Chart chart = new Chart();
            var vendaComp = desempenhoController.ObterVendasComparaGrafico(codigoFilial, dataIniAtual, dataFimAutal, dataIniAnt, dataFimAnt);
            chart = ObterResultadoGrafico(vendaComp);
            if (chart != null)
            {
                pnlGraficoResultado.Controls.Add(chart);
                //pnlResultado.Controls.Add(new LiteralControl("<br />"));
            }
        }
        private Chart ObterResultadoGrafico(List<SP_OBTER_VENDA_COMPARAANTResult> vendaComp)
        {
            string nomeSerieAtual = "Valores em R$ (Período Atual) - Total: R$ " + vendaComp.Sum(p => p.VALOR_ATU).ToString();
            string nomeSerieAnterior = "Valores em R$ (Período Anterior) - Total: R$ " + vendaComp.Sum(p => p.VALOR_ANT).ToString();

            string _chartArea = "chartArea" + "1";

            try
            {
                Chart chart = new Chart();

                chart.Width = Unit.Pixel(1425);
                //chart.Height = Unit.Pixel(350);

                chart.Titles.Add(new Title(
                                    "Comparação do Período por Dia",
                                    Docking.Top,
                                    new System.Drawing.Font("Verdana", 10f, FontStyle.Bold),
                                    System.Drawing.Color.Black
                ));
                chart.Titles[0].BackColor = System.Drawing.Color.Gainsboro;

                Series serieAtual = new Series(nomeSerieAtual);
                serieAtual.ChartType = SeriesChartType.Line;
                chart.Series.Add(serieAtual);


                Series serieAnterior = new Series(nomeSerieAnterior);
                serieAnterior.ChartType = SeriesChartType.Line;
                chart.Series.Add(serieAnterior);

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

                int qtde_linhas = vendaComp.Count;

                if (qtde_linhas > 0)
                {

                    DateTime[] valoresx = new DateTime[qtde_linhas];
                    //SERIE ATUAL
                    decimal[] valoresy = new decimal[qtde_linhas];
                    //SERIE ANTERIOR
                    decimal[] valoresyA = new decimal[qtde_linhas];


                    int vx = 0;
                    for (vx = 0; vx < qtde_linhas; vx++)
                    {
                        valoresx[vx] = vendaComp[vx].DATA;
                        valoresy[vx] = Convert.ToDecimal(vendaComp[vx].VALOR_ATU.ToString().Replace("%", "").Replace(".", ","));

                        //OBTER VALORES ACEITAVEIS
                        valoresyA[vx] = vendaComp[vx].VALOR_ANT;
                    }

                    //SERIE VALORES ATUAIS
                    chart.Series[nomeSerieAtual].Points.DataBindXY(valoresx, valoresy);
                    chart.Series[nomeSerieAtual].ChartType = SeriesChartType.Line;
                    chart.Series[nomeSerieAtual].IsValueShownAsLabel = true;
                    chart.Series[nomeSerieAtual].Label = "" + "#VALY" + "";
                    chart.Series[nomeSerieAtual].MarkerSize = 8;
                    chart.Series[nomeSerieAtual].BorderWidth = 4;
                    chart.Series[nomeSerieAtual].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[nomeSerieAtual].Color = System.Drawing.Color.DarkBlue;
                    chart.Series[nomeSerieAtual].XValueType = ChartValueType.DateTime;
                    chart.Series[nomeSerieAtual].Font = new System.Drawing.Font("Verdana", 9f, FontStyle.Bold);

                    //SERIE VALORES ANTERIORES
                    chart.Series[nomeSerieAnterior].Points.DataBindXY(valoresx, valoresyA);
                    chart.Series[nomeSerieAnterior].ChartType = SeriesChartType.Line;
                    chart.Series[nomeSerieAnterior].BorderWidth = 2;
                    chart.Series[nomeSerieAnterior].MarkerSize = 9;
                    chart.Series[nomeSerieAnterior].IsValueShownAsLabel = false;
                    chart.Series[nomeSerieAnterior].MarkerStyle = MarkerStyle.Triangle;
                    chart.Series[nomeSerieAnterior].Color = System.Drawing.Color.Tomato;

                    //EIXO X
                    LabelStyle styleX = new LabelStyle();
                    styleX.Font = new System.Drawing.Font("Verdana", 9f, FontStyle.Regular);
                    styleX.Format = "dd/MMM";
                    chart.ChartAreas[0].AxisX.LabelStyle = styleX;
                    chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    chart.ChartAreas[0].AxisX.Interval = 1;
                    chart.ChartAreas[0].AxisX.Title = "Dias";
                    chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Verdana", 9f, FontStyle.Bold);
                    chart.ChartAreas[0].AxisX.IntervalOffset = 1;
                    chart.ChartAreas[0].AxisX.Minimum = vendaComp[0].DATA.AddDays(-1).ToOADate();
                    chart.ChartAreas[0].AxisX.Maximum = vendaComp[vendaComp.Count() - 1].DATA.AddDays(1).ToOADate();

                    //EIXO Y
                    LabelStyle styleY = new LabelStyle();
                    styleY.Font = new System.Drawing.Font("Verdana", 9f, FontStyle.Regular);
                    styleY.Format = "{###,###,##0.00}";
                    chart.ChartAreas[0].AxisY.LabelStyle = styleY;
                    chart.ChartAreas[0].AxisY.Title = "Valores em R$ ";
                    chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 9f, FontStyle.Bold);
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(valoresy.Max()) + 1000;
                    //chart.ChartAreas[0].AxisY.Interval = 0.5;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;

                    //LEGENDA
                    chart.Legends.Add(new Legend("LegendCont"));
                    chart.Legends["LegendCont"].Alignment = StringAlignment.Center;
                    chart.Series[nomeSerieAtual].Legend = "LegendCont";
                    chart.Series[nomeSerieAnterior].Legend = "LegendCont";
                    chart.Series[nomeSerieAtual].IsVisibleInLegend = true;
                    chart.Series[nomeSerieAnterior].IsVisibleInLegend = true;


                    //OUTROS
                    chart.Series[nomeSerieAtual]["PieLabelStyle"] = "inside";
                    chart.Series[nomeSerieAnterior]["PieLabelStyle"] = "inside";


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
public class CHARTCOMPVENDA
{
    public string codigoFilial { get; set; }
    public string dataIniAtual { get; set; }
    public string dataFimAtual { get; set; }
    public string dataIniAnt { get; set; }
    public string dataFimAnt { get; set; }
}