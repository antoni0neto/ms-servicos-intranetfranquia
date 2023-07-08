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
using System.Text;

namespace Relatorios
{
    public partial class cont_resultado_grafico : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();

        int tValorAceitavel = 0;
        int tResultadoPeca = 0;
        decimal tPorcValorAceitavel = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarSupervisor();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btEnviarEmail.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviarEmail, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarSupervisor()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            if (usuario != null)
            {
                var super = baseController.BuscaUsuarioPerfil(3);
                if (usuario.CODIGO_PERFIL == 3)
                {
                    super = super.Where(p => p.CODIGO_USUARIO == usuario.CODIGO_USUARIO).ToList();
                }

                super.Insert(0, new USUARIO { CODIGO_USUARIO = 0, NOME_USUARIO = "Selecione" });
                ddlSupervisor.DataSource = super;
                ddlSupervisor.DataBind();

                if (super != null && super.Count() == 2)
                {
                    ddlSupervisor.SelectedIndex = 1;
                    ddlSupervisor.Enabled = false;
                    ddlSupervisor_SelectedIndexChanged(null, null);
                }
                else
                {
                    CarregarFilial("0");
                }
            }

        }
        private void CarregarFilial(string codigoSupervisor)
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            if (codigoSupervisor != "0")
            {
                var usu = baseController.BuscaUsuario(Convert.ToInt32(codigoSupervisor));
                filial = baseController.BuscaFiliais(usu).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").OrderBy(p => p.FILIAL).ToList();
            }
            else
            {
                filial = baseController.BuscaFiliais();
            }

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarDataParaContagem(string codigoFilial)
        {
            var dataContagem = contController.ObterResultado(codigoFilial);

            List<ListItem> items = new List<ListItem>();
            foreach (var i in dataContagem)
                items.Add(new ListItem { Text = Convert.ToDateTime(i.DATA).ToString("dd/MM/yyyy"), Value = Convert.ToDateTime(i.DATA).ToString("dd/MM/yyyy") });

            items.Insert(0, new ListItem { Text = "Selecione", Value = "" });
            ddlDataContagemIni.DataSource = items;
            ddlDataContagemIni.DataBind();

            ddlDataContagemFim.DataSource = items;
            ddlDataContagemFim.DataBind();

            if (items.Count() >= 2)
            {
                ddlDataContagemIni.SelectedIndex = 1;
                ddlDataContagemFim.SelectedIndex = items.Count() - 1;
            }

        }
        protected void ddlSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFilial(ddlSupervisor.SelectedValue);
        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarDataParaContagem(ddlFilial.SelectedValue);
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoFilial = "";
                labErro.Text = "";

                if (ddlDataContagemIni.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Período Inicial.";
                    return;
                }

                if (ddlDataContagemFim.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Período Final.";
                    return;
                }


                codigoFilial = ddlFilial.SelectedValue;
                var resultadoContagem = CarregarResultadoContagem(codigoFilial, Convert.ToDateTime(ddlDataContagemIni.SelectedValue), Convert.ToDateTime(ddlDataContagemFim.SelectedValue));

                if (resultadoContagem != null)
                {
                    Chart chart = new Chart();
                    chart = ObterResultadoGrafico(resultadoContagem, ddlFilial.SelectedItem.Text.Trim(), cbRede.Checked);
                    if (chart != null)
                    {
                        pnlResultado.Controls.Add(chart);
                        //pnlResultado.Controls.Add(new LiteralControl("<br />"));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "GRID"
        private List<SP_OBTER_CONT_RESULT_LOJAResult> CarregarResultadoContagem(string codigoFilial, DateTime periodoIni, DateTime periodoFim)
        {
            var resultadoLoja = contController.ObterResultadoPorLoja(codigoFilial, periodoIni, periodoFim);
            gvResultadoContagem.DataSource = resultadoLoja;
            gvResultadoContagem.DataBind();

            if (resultadoLoja == null || resultadoLoja.Count() <= 0)
            {
                labErro.Text = "Nenhum resultado de Contagem Encontrado.";
                return null;
            }

            return resultadoLoja;
        }
        protected void gvResultadoContagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CONT_RESULT_LOJAResult resultadoCont = e.Row.DataItem as SP_OBTER_CONT_RESULT_LOJAResult;

                    if (resultadoCont != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = resultadoCont.DATA_CONTAGEM.ToString("dd/MM/yyyy");

                        Literal _litDiaUltimaContagem = e.Row.FindControl("litDiaUltimaContagem") as Literal;
                        if (_litDiaUltimaContagem != null)
                            _litDiaUltimaContagem.Text = resultadoCont.DIAS_ULTIMA_CONTAGEM.ToString() + " dias";

                        if (resultadoCont.LEGENDA.ToLower() == "boa")
                            e.Row.BackColor = Color.LightGreen;
                        else if (resultadoCont.LEGENDA.ToLower() == "preocupante")
                            e.Row.BackColor = Color.Gold;
                        else if (resultadoCont.LEGENDA.ToLower() == "ruim")
                            e.Row.BackColor = Color.LightCoral;
                        else if (resultadoCont.LEGENDA.ToLower() == "crítica")
                        {
                            e.Row.BackColor = Color.MediumPurple;
                            e.Row.ForeColor = Color.White;
                        }

                        tValorAceitavel += resultadoCont.VALOR_ACEITAVEL;
                        tResultadoPeca += resultadoCont.RESULTADO_PECAS;

                        tPorcValorAceitavel += resultadoCont.VALOR_ACEITAVEL_PORC;

                    }
                }
            }

        }
        protected void gvResultadoContagem_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvResultadoContagem.FooterRow;
            if (footer != null)
            {

                footer.Cells[1].Text = "Total";
                footer.Cells[3].Text = tValorAceitavel.ToString();
                footer.Cells[4].Text = tResultadoPeca.ToString();

                decimal mediaValorAceitavelPorc = tPorcValorAceitavel / gvResultadoContagem.Rows.Count;

                footer.Cells[5].Text = ((tResultadoPeca * mediaValorAceitavelPorc) / tValorAceitavel).ToString("##0.00") + "%";

            }
        }
        #endregion

        private Chart ObterResultadoGrafico(List<SP_OBTER_CONT_RESULT_LOJAResult> resultadoContagem, string filial, bool rede)
        {
            string nomeSerieLoja = filial;
            string nomeSerieAceitavel = "Valor Aceitável";
            string nomeSerieRede = "Rede";
            string _chartArea = "chartArea" + "1";

            try
            {
                Chart chart = new Chart();

                chart.Width = Unit.Pixel(1425);
                //chart.Height = Unit.Pixel(350);

                chart.Titles.Add(new Title(
                                    filial,
                                    Docking.Top,
                                    new Font("Verdana", 10f, FontStyle.Bold),
                                    Color.Black
                ));
                chart.Titles[0].BackColor = Color.Gainsboro;

                Series serieLoja = new Series(nomeSerieLoja);
                serieLoja.ChartType = SeriesChartType.Line;
                chart.Series.Add(serieLoja);
                if (rede)
                {
                    Series serieRede = new Series(nomeSerieRede);
                    serieRede.ChartType = SeriesChartType.Line;
                    chart.Series.Add(serieRede);
                }

                Series serieAceitavel = new Series(nomeSerieAceitavel);
                serieAceitavel.ChartType = SeriesChartType.Line;
                chart.Series.Add(serieAceitavel);

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

                int qtde_linhas = resultadoContagem.Count;

                if (qtde_linhas > 0)
                {
                    //SERIE LOJA
                    DateTime[] valoresx = new DateTime[qtde_linhas];
                    decimal[] valoresy = new decimal[qtde_linhas];
                    //SERIE VALOR ACEITAVEL
                    decimal[] valoresyA = new decimal[qtde_linhas];

                    //SERIE REDE
                    decimal[] valoresyRede = new decimal[qtde_linhas];

                    int vx = 0;
                    for (vx = 0; vx < qtde_linhas; vx++)
                    {
                        valoresx[vx] = resultadoContagem[vx].DATA_CONTAGEM;
                        valoresy[vx] = Convert.ToDecimal(resultadoContagem[vx].PORC_PERDA.Replace("%", "").Replace(".", ","));

                        //OBTER VALORES ACEITAVEIS
                        valoresyA[vx] = resultadoContagem[vx].VALOR_ACEITAVEL_PORC;

                        if (rede)
                        {
                            //OBTER VALOR DA REDE POR MES
                            valoresyRede[vx] = ObterResultadoRede(resultadoContagem[vx].DATA_CONTAGEM);
                        }
                    }

                    //SERIE DA LOJA
                    chart.Series[nomeSerieLoja].Points.DataBindXY(valoresx, valoresy);
                    chart.Series[nomeSerieLoja].ChartType = SeriesChartType.Line;
                    chart.Series[nomeSerieLoja].IsValueShownAsLabel = true;
                    chart.Series[nomeSerieLoja].Label = "#VALY" + "%";
                    chart.Series[nomeSerieLoja].MarkerSize = 8;
                    chart.Series[nomeSerieLoja].BorderWidth = 4;
                    chart.Series[nomeSerieLoja].MarkerStyle = MarkerStyle.Circle;
                    chart.Series[nomeSerieLoja].Color = Color.DarkBlue;
                    chart.Series[nomeSerieLoja].XValueType = ChartValueType.DateTime;
                    chart.Series[nomeSerieLoja].Font = new Font("Verdana", 9f, FontStyle.Bold);

                    //SERIE ACEITAVEL
                    chart.Series[nomeSerieAceitavel].Points.DataBindXY(valoresx, valoresyA);
                    chart.Series[nomeSerieAceitavel].ChartType = SeriesChartType.Line;
                    chart.Series[nomeSerieAceitavel].BorderWidth = 2;
                    chart.Series[nomeSerieAceitavel].MarkerSize = 9;
                    chart.Series[nomeSerieAceitavel].IsValueShownAsLabel = false;
                    chart.Series[nomeSerieAceitavel].MarkerStyle = MarkerStyle.Triangle;
                    chart.Series[nomeSerieAceitavel].Color = Color.DarkGreen;

                    if (rede)
                    {
                        //SERIE REDE
                        chart.Series[nomeSerieRede].Points.DataBindXY(valoresx, valoresyRede);
                        chart.Series[nomeSerieRede].ChartType = SeriesChartType.Line;
                        chart.Series[nomeSerieRede].BorderWidth = 2;
                        chart.Series[nomeSerieRede].MarkerSize = 9;
                        chart.Series[nomeSerieRede].MarkerStyle = MarkerStyle.Diamond;
                        chart.Series[nomeSerieRede].IsValueShownAsLabel = false;
                        chart.Series[nomeSerieRede].Color = Color.DarkRed;
                    }
                    //EIXO X
                    LabelStyle styleX = new LabelStyle();
                    styleX.Font = new Font("Verdana", 9f, FontStyle.Regular);
                    styleX.Format = "dd/MMM";
                    chart.ChartAreas[0].AxisX.LabelStyle = styleX;
                    chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chart.ChartAreas[0].AxisX.Interval = 1;
                    chart.ChartAreas[0].AxisX.Title = "Tempo em Meses";
                    chart.ChartAreas[0].AxisX.TitleFont = new Font("Verdana", 9f, FontStyle.Bold);
                    chart.ChartAreas[0].AxisX.IntervalOffset = 1;
                    chart.ChartAreas[0].AxisX.Minimum = resultadoContagem[0].DATA_CONTAGEM.AddMonths(-1).ToOADate();
                    chart.ChartAreas[0].AxisX.Maximum = resultadoContagem[resultadoContagem.Count() - 1].DATA_CONTAGEM.AddMonths(1).ToOADate();

                    //EIXO Y
                    LabelStyle styleY = new LabelStyle();
                    styleY.Font = new Font("Verdana", 9f, FontStyle.Regular);
                    styleY.Format = "{0.00}%";
                    chart.ChartAreas[0].AxisY.LabelStyle = styleY;
                    chart.ChartAreas[0].AxisY.Title = "% de Perda";
                    chart.ChartAreas[0].AxisY.TitleFont = new Font("Verdana", 9f, FontStyle.Bold);
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(valoresy.Max()) + 1.5;
                    chart.ChartAreas[0].AxisY.Interval = 0.5;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = true;

                    //LEGENDA
                    chart.Legends.Add(new Legend("LegendCont"));
                    chart.Legends["LegendCont"].Alignment = StringAlignment.Center;
                    chart.Series[nomeSerieLoja].Legend = "LegendCont";
                    chart.Series[nomeSerieAceitavel].Legend = "LegendCont";
                    chart.Series[nomeSerieLoja].IsVisibleInLegend = true;
                    chart.Series[nomeSerieAceitavel].IsVisibleInLegend = true;

                    if (rede)
                    {
                        chart.Series[nomeSerieRede].Legend = "LegendCont";
                        chart.Series[nomeSerieRede].IsVisibleInLegend = true;
                    }

                    //OUTROS
                    chart.Series[nomeSerieLoja]["PieLabelStyle"] = "inside";
                    chart.Series[nomeSerieAceitavel]["PieLabelStyle"] = "inside";
                    if (rede)
                        chart.Series[nomeSerieRede]["PieLabelStyle"] = "inside";

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

        private decimal ObterResultadoRede(DateTime dataFiltro)
        {
            decimal resp = 0;

            int valorAceitavel = 0;
            int resultadoPeca = 0;
            decimal valorAceitavelPorc = 0;
            int cont = 0;


            var listaResultado = contController.ObterResultado(dataFiltro.Year, dataFiltro.Month);

            foreach (var v in listaResultado)
            {
                var r = contController.ObterResultadoPorLoja(v.CODIGO_FILIAL)
                    .Where(p => p.DATA_CONTAGEM.Year == dataFiltro.Year && p.DATA_CONTAGEM.Month == dataFiltro.Month);

                foreach (var t in r)
                {
                    valorAceitavel += t.VALOR_ACEITAVEL;
                    resultadoPeca += t.RESULTADO_PECAS;
                    valorAceitavelPorc += t.VALOR_ACEITAVEL_PORC;

                    cont += 1;
                }

            }

            resp = (resultadoPeca * (valorAceitavelPorc / cont)) / valorAceitavel;


            return resp;
        }

        #region "EMAIL"
        protected void btEnviarEmail_Click(object sender, EventArgs e)
        {
            if (Constante.enviarEmail)
            {
                var resultadoContagem = CarregarResultadoContagem(ddlFilial.SelectedValue, Convert.ToDateTime(ddlDataContagemIni.SelectedValue), Convert.ToDateTime(ddlDataContagemFim.SelectedValue));
                if (resultadoContagem != null)
                {
                    Chart chart = new Chart();
                    chart = ObterResultadoGrafico(resultadoContagem, ddlFilial.SelectedItem.Text.Trim(), cbRede.Checked);
                    if (chart != null)
                    {
                        Guid g = Guid.NewGuid();
                        string nomeArquivo = "\\chart\\" + g + ".jpg";
                        chart.SaveImage(Server.MapPath("") + nomeArquivo, ChartImageFormat.Jpeg);
                        //pnlResultado.Controls.Add(new LiteralControl("<br />"));
                        EnviarEmail(resultadoContagem, nomeArquivo);
                    }
                }


            }

        }

        private void EnviarEmail(List<SP_OBTER_CONT_RESULT_LOJAResult> resultadoContagem, string nomeArquivo)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Resultado de Inventário - " + ddlFilial.SelectedItem.Text.Trim();
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(resultadoContagem, nomeArquivo);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(11, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            var supervisor = baseController.BuscaSupervisorLoja(ddlFilial.SelectedValue);
            if (supervisor != null)
            {
                var usuarioSupervisor = baseController.BuscaUsuario(supervisor.codigo_usuario);
                if (usuarioSupervisor != null)
                {
                    destinatario.Add(usuarioSupervisor.EMAIL);
                }
            }

            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(List<SP_OBTER_CONT_RESULT_LOJAResult> resultadoContagem, string nomeArquivo)
        {

            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Inventário</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divInvet' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>Contagem de Estoque " + " - " + ddlFilial.SelectedItem.Text.Trim() + "</h3>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 600pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                     <table cellpadding='0' cellspacing='0' style='width: 100%; padding: 0px; color: black;");
            sb.Append("                                         font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            sb.Append("                                         border: 1px solid #ccc;'>");
            sb.Append("                                         <tr style='background-color: #ccc;'>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Data");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Última Contagem");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Valor Aceitável");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Resultado de Peças");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 % Perda");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Peças/Dia");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                 Gerente");
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");

            string backColor = "";
            string color = "";
            foreach (var r in resultadoContagem)
            {
                // 630c8b ROXO
                // 50a438 VERDE
                // ffd800 AMARELHO
                // d04141 VERMELHO
                if (r.LEGENDA.ToLower() == "boa")
                {
                    backColor = "50a438";
                    color = "";
                }
                else if (r.LEGENDA.ToLower() == "preocupante")
                {
                    backColor = "ffd800";
                    color = "";
                }
                else if (r.LEGENDA.ToLower() == "ruim")
                {
                    backColor = "d04141";
                    color = "";
                }
                else if (r.LEGENDA.ToLower() == "crítica")
                {
                    backColor = "630c8b";
                    color = "white";
                }

                sb.Append("                                         <tr style='background-color: #" + backColor + "; color:" + color + ";'>");
                sb.Append("                                             <td style='text-align: center; width: 200px; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                sb.Append("                                                 " + r.DATA_CONTAGEM.ToString("dd/MM/yyyy"));
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='width: 200px; text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                sb.Append("                                                 " + r.DIAS_ULTIMA_CONTAGEM.ToString() + " dias");
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='width: 200px; text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                sb.Append("                                                 " + r.VALOR_ACEITAVEL.ToString());
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='width: 200px; text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                sb.Append("                                                 " + r.RESULTADO_PECAS.ToString());
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='width: 200px; text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                sb.Append("                                                 " + r.PORC_PERDA.Replace(".", ","));
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='width: 200px; text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                sb.Append("                                                  " + r.PECAS_POR_DIA.Replace(".", ","));
                sb.Append("                                             </td>");
                sb.Append("                                             <td style='width: 200px; text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                sb.Append("                                                  " + r.GERENTE);
                sb.Append("                                             </td>");
                sb.Append("                                         </tr>");
            }
            sb.Append("                                         <tr style='background-color: #ccc;'>");
            sb.Append("                                             <td colspan='7' style='text-align: center;'>");
            sb.Append("                                                 &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                     </table>");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                    <img alt='' src='http://cmaxweb.dnsalias.com:8585" + nomeArquivo.Replace("\\", "/") + "' />&nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        #endregion



    }
}
