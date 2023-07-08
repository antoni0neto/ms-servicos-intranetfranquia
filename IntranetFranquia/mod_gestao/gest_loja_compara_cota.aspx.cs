using DAL;
using GemBox.Spreadsheet;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class gest_loja_compara_cota : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        public decimal totalAtingidoAtual = 0;
        public decimal totalAtingidoAnterior = 0;
        public decimal v_totalAtingidoAtual = 0;
        public decimal v_totalCota = 0;
        public int contaLinha = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gest_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_gerenciamento_loja/gerloja_menu.aspx";
            }

        }

        protected void CalendarDataInicioAtual_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicioAtual.Text = CalendarDataInicioAtual.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFimAtual_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFimAtual.Text = CalendarDataFimAtual.SelectedDate.ToString("dd/MM/yyyy");
        }

        private void CarregaGridViewVendas()
        {
            List<VendasCotasLoja> listaVendas = baseController.BuscaVendasCotasLoja(CalendarDataInicioAtual.SelectedDate, CalendarDataFimAtual.SelectedDate);

            if (listaVendas != null)
            {
                listaVendas = listaVendas.OrderByDescending(z => z.PercAtingido).ToList();

                GridViewVendas.DataSource = listaVendas;
                GridViewVendas.DataBind();

                GridViewFarol.DataSource = baseController.BuscaApoioFarol();
                GridViewFarol.DataBind();

                List<ResumoSupervisor> listaResumoSupervisor = new List<ResumoSupervisor>();

                int qtde_verde_maurilio = 0;
                int qtde_amarelo_maurilio = 0;
                int qtde_vermelho_maurilio = 0;
                int qtde_roxo_maurilio = 0;

                int qtde_verde_dirceu = 0;
                int qtde_amarelo_dirceu = 0;
                int qtde_vermelho_dirceu = 0;
                int qtde_roxo_dirceu = 0;

                decimal vendaMaurilio = 0;
                decimal cotaMaurilio = 0;
                decimal vendaDirceu = 0;
                decimal cotaDirceu = 0;

                foreach (VendasCotasLoja item in listaVendas)
                {
                    if (item.Supervisor != null)
                    {
                        if (item.Supervisor.Equals("Maurilio", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (item.PercAtingido >= 90)
                                qtde_verde_maurilio++;

                            if (item.PercAtingido < 90 && item.PercAtingido >= 80)
                                qtde_amarelo_maurilio++;

                            if (item.PercAtingido < 80 && item.PercAtingido >= 70)
                                qtde_vermelho_maurilio++;

                            if (item.PercAtingido < 70)
                                qtde_roxo_maurilio++;

                            vendaMaurilio += item.ValAtingido;
                            cotaMaurilio += item.ValCota;
                        }

                        if (item.Supervisor.Equals("Dirceu de Lima", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (item.PercAtingido >= 90)
                                qtde_verde_dirceu++;

                            if (item.PercAtingido < 90 && item.PercAtingido >= 80)
                                qtde_amarelo_dirceu++;

                            if (item.PercAtingido < 80 && item.PercAtingido >= 70)
                                qtde_vermelho_dirceu++;

                            if (item.PercAtingido < 70)
                                qtde_roxo_dirceu++;

                            vendaDirceu += item.ValAtingido;
                            cotaDirceu += item.ValCota;
                        }


                    }
                }

                ResumoSupervisor resumoSupervisor = null;
                if ((qtde_verde_maurilio + qtde_amarelo_maurilio + qtde_vermelho_maurilio + qtde_roxo_maurilio) > 0)
                {
                    resumoSupervisor = new ResumoSupervisor();
                    resumoSupervisor.Supervisor = "Maurilio";
                    resumoSupervisor.Qtde_verde = qtde_verde_maurilio;
                    resumoSupervisor.Qtde_amarelo = qtde_amarelo_maurilio;
                    resumoSupervisor.Qtde_vermelho = qtde_vermelho_maurilio;
                    resumoSupervisor.Qtde_roxo = qtde_roxo_maurilio;
                    resumoSupervisor.Atingido = (cotaMaurilio == 0) ? 0 : (vendaMaurilio / cotaMaurilio);
                    listaResumoSupervisor.Add(resumoSupervisor);
                }

                if ((qtde_verde_dirceu + qtde_amarelo_dirceu + qtde_vermelho_dirceu + qtde_roxo_dirceu) > 0)
                {
                    resumoSupervisor = new ResumoSupervisor();
                    resumoSupervisor.Supervisor = "Dirceu de Lima";
                    resumoSupervisor.Qtde_verde = qtde_verde_dirceu;
                    resumoSupervisor.Qtde_amarelo = qtde_amarelo_dirceu;
                    resumoSupervisor.Qtde_vermelho = qtde_vermelho_dirceu;
                    resumoSupervisor.Qtde_roxo = qtde_roxo_dirceu;
                    resumoSupervisor.Atingido = (cotaDirceu == 0) ? 0 : (vendaDirceu / cotaDirceu);
                    listaResumoSupervisor.Add(resumoSupervisor);
                }

                GridViewResumo.DataSource = listaResumoSupervisor.OrderByDescending(p => p.Atingido).ToList();
                GridViewResumo.DataBind();

                Session["Farol"] = listaVendas;
            }
            else
                Session["Farol"] = null;
        }

        protected void btPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (txtDataInicioAtual.Text.Equals("") ||
                txtDataInicioAtual.Text == null ||
                txtDataInicioAtual.Text.Equals("") ||
                txtDataInicioAtual.Text == null)
                return;

            CarregaGridViewVendas();

            btGeraExcel.Enabled = true;
        }

        protected void GridViewFarol_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewFarol.Rows)
            {
                if (item.Cells[0].Text.Equals("Verde"))
                {
                    item.Cells[0].Text = "";
                    item.Cells[0].BackColor = System.Drawing.Color.Green;
                }

                if (item.Cells[0].Text.Equals("Amarelo"))
                {
                    item.Cells[0].Text = "";
                    item.Cells[0].BackColor = System.Drawing.Color.Yellow;
                }

                if (item.Cells[0].Text.Equals("Vermelho "))
                {
                    item.Cells[0].Text = "";
                    item.Cells[0].BackColor = System.Drawing.Color.Red;
                }

                if (item.Cells[0].Text.Equals("Roxo"))
                {
                    item.Cells[0].Text = "";
                    item.Cells[0].BackColor = System.Drawing.Color.MediumPurple;
                    item.Cells[0].ForeColor = System.Drawing.Color.Black;
                }
            }
        }
        protected void GridViewResumo_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewResumo.Rows)
            {
                item.Cells[2].BackColor = System.Drawing.Color.Green;
                item.Cells[3].BackColor = System.Drawing.Color.Yellow;
                item.Cells[4].BackColor = System.Drawing.Color.Red;
                item.Cells[5].BackColor = System.Drawing.Color.MediumPurple;
                item.Cells[5].ForeColor = System.Drawing.Color.Black;
            }
        }
        protected void GridViewResumo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal atingido = 0;
            ResumoSupervisor _Resumo = e.Row.DataItem as ResumoSupervisor;
            if (_Resumo != null)
            {
                atingido = (_Resumo.Atingido * (100));
                if (atingido >= 90)
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Green;
                if (atingido < 90 && atingido >= 80)
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Yellow;
                if (atingido < 80 && atingido >= 70)
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Red;
                if (atingido < 70)
                    e.Row.Cells[1].BackColor = System.Drawing.Color.MediumPurple;

                Literal _litAtingido = e.Row.FindControl("litAtingido") as Literal;
                if (_litAtingido != null)
                    _litAtingido.Text = atingido.ToString("###,###,###,##0.00") + "%";
            }
        }
        protected void GridViewVendas_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendas.Rows)
            {
                Literal literalPercentual = item.FindControl("LiteralPercentual") as Literal;

                if (literalPercentual != null)
                {
                    int inteiro = Convert.ToInt32(literalPercentual.Text.Substring(0, literalPercentual.Text.Length - 5));

                    if (inteiro >= 90)
                    {
                        item.Cells[0].BackColor = System.Drawing.Color.Green;
                        item.Cells[1].BackColor = System.Drawing.Color.Green;
                        item.Cells[2].BackColor = System.Drawing.Color.Green;
                        item.Cells[3].BackColor = System.Drawing.Color.Green;
                    }

                    if (inteiro < 90 && inteiro >= 80)
                    {
                        item.Cells[0].BackColor = System.Drawing.Color.Yellow;
                        item.Cells[1].BackColor = System.Drawing.Color.Yellow;
                        item.Cells[2].BackColor = System.Drawing.Color.Yellow;
                        item.Cells[3].BackColor = System.Drawing.Color.Yellow;
                    }

                    if (inteiro < 80 && inteiro >= 70)
                    {
                        item.Cells[0].BackColor = System.Drawing.Color.Red;
                        item.Cells[1].BackColor = System.Drawing.Color.Red;
                        item.Cells[2].BackColor = System.Drawing.Color.Red;
                        item.Cells[3].BackColor = System.Drawing.Color.Red;
                    }

                    if (inteiro < 70)
                    {
                        item.Cells[0].BackColor = System.Drawing.Color.MediumPurple;
                        item.Cells[1].BackColor = System.Drawing.Color.MediumPurple;
                        item.Cells[2].BackColor = System.Drawing.Color.MediumPurple;
                        item.Cells[3].BackColor = System.Drawing.Color.MediumPurple;
                        item.Cells[0].ForeColor = System.Drawing.Color.Black;
                        item.Cells[1].ForeColor = System.Drawing.Color.Black;
                        item.Cells[2].ForeColor = System.Drawing.Color.Black;
                        item.Cells[3].ForeColor = System.Drawing.Color.Black;
                    }
                }

                Literal literalSobe = item.FindControl("LiteralSobe") as Literal;

                if (literalSobe != null)
                {
                    if (literalSobe.Text.Equals("+"))
                        item.Cells[4].BackColor = System.Drawing.Color.LightSkyBlue;
                    else
                        item.Cells[4].BackColor = System.Drawing.Color.Red;
                }
            }

            GridViewRow footer = GridViewVendas.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;

                if (contaLinha > 0)
                {
                    if (v_totalCota > 0)
                        footer.Cells[1].Text = (Convert.ToDecimal(v_totalAtingidoAtual / v_totalCota) * 100).ToString("N2") + " %";
                }

                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;

                if (totalAtingidoAtual > totalAtingidoAnterior)
                {
                    footer.Cells[4].Text = "+";
                    footer.Cells[4].BackColor = System.Drawing.Color.LightSkyBlue;
                }
                else
                {
                    footer.Cells[4].Text = "-";
                    footer.Cells[4].BackColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void GridViewVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            VendasCotasLoja vendas = e.Row.DataItem as VendasCotasLoja;

            if (vendas != null)
            {
                Literal literalPercentual = e.Row.FindControl("LiteralPercentual") as Literal;

                if (literalPercentual != null)
                    literalPercentual.Text = vendas.PercAtingido.ToString("##0.00") + " %";

                Literal literalSobe = e.Row.FindControl("LiteralSobe") as Literal;

                if (literalSobe != null)
                {
                    if (vendas.Sobe)
                        literalSobe.Text = "+";
                    else
                        literalSobe.Text = "-";
                }

                totalAtingidoAtual += vendas.PercAtingido;
                totalAtingidoAnterior += vendas.PercAtingidoAnoAnterior;

                /*
                 ALTERAÇÃO SOLICITADA - MARIA CAVACCINI
                 */
                v_totalAtingidoAtual += vendas.ValAtingido;
                v_totalCota += vendas.ValCota;
                //

                contaLinha++;
            }
        }

        protected void btGeraExcel_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                var diretorioArquivo = GerarExcel();

                //Enviar Email
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();

                var assunto = "[Intranet] - FAROL - " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " - " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.ANEXO = diretorioArquivo;
                email.MENSAGEM = "Anexo farol das lojas no período de " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " à " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(52, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);
                destinatario.Add("grupo-lojas@hbf.com.br");

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
            var titleRange = xlWorkSheet.Cells.GetSubrangeAbsolute(2, 0, 2, 5);
            titleRange.Merged = true;
            titleRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            titleRange.Style.Font.Weight = ExcelFont.BoldWeight;

            xlWorkSheet.Cells[3, 0].Value = "";
            xlWorkSheet.Cells[3, 1].Value = "Filial";
            xlWorkSheet.Cells[3, 2].Value = "Atingido";
            xlWorkSheet.Cells[3, 3].Value = "Gerente";
            xlWorkSheet.Cells[3, 4].Value = "Supervisor";
            xlWorkSheet.Cells[3, 5].Value = "";

            var columnHeadingsRange = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 0, 3, 5);
            columnHeadingsRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            columnHeadingsRange.Style.Font.Weight = ExcelFont.BoldWeight;
            columnHeadingsRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            columnHeadingsRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            columnHeadingsRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            xlWorkSheet.Rows[3].Height = 20 * 20;
            xlWorkSheet.Columns[0].Width = 15 * 256;
            xlWorkSheet.Columns[1].Width = 35 * 256;
            xlWorkSheet.Columns[2].Width = 20 * 256;
            xlWorkSheet.Columns[3].Width = 20 * 256;
            xlWorkSheet.Columns[4].Width = 20 * 256;
            xlWorkSheet.Columns[5].Width = 10 * 256;


            //INICIO PRIMEIRO QUADRO

            int linha = 4;
            int posicao = 1;
            int inteiroAtingido = 0;
            foreach (GridViewRow row in GridViewVendas.Rows)
            {
                Literal litAtingido = row.FindControl("LiteralPercentual") as Literal;
                Literal litSobe = row.FindControl("LiteralSobe") as Literal;

                inteiroAtingido = Convert.ToInt32(litAtingido.Text.Substring(0, litAtingido.Text.Length - 5));

                var cellBackColor = System.Drawing.Color.White;
                if (inteiroAtingido >= 90)
                    cellBackColor = System.Drawing.Color.Green;
                else if (inteiroAtingido < 90 && inteiroAtingido >= 80)
                    cellBackColor = System.Drawing.Color.Yellow;
                else if (inteiroAtingido < 80 && inteiroAtingido >= 70)
                    cellBackColor = System.Drawing.Color.Red;
                else if (inteiroAtingido < 70)
                    cellBackColor = System.Drawing.Color.MediumPurple;


                xlWorkSheet.Cells[linha, 0].Value = posicao.ToString();
                xlWorkSheet.Cells[linha, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 0].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 0].Style.FillPattern.SetSolid(cellBackColor);

                xlWorkSheet.Cells[linha, 1].Value = row.Cells[0].Text;
                xlWorkSheet.Cells[linha, 1].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 1].Style.FillPattern.SetSolid(cellBackColor);


                xlWorkSheet.Cells[linha, 2].Value = litAtingido.Text.Trim();
                xlWorkSheet.Cells[linha, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 2].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 2].Style.FillPattern.SetSolid(cellBackColor);

                xlWorkSheet.Cells[linha, 3].Value = row.Cells[2].Text.Trim().Replace("&nbsp;", "");
                xlWorkSheet.Cells[linha, 3].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 3].Style.FillPattern.SetSolid(cellBackColor);

                xlWorkSheet.Cells[linha, 4].Value = row.Cells[3].Text.Trim();
                xlWorkSheet.Cells[linha, 4].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 4].Style.FillPattern.SetSolid(cellBackColor);

                xlWorkSheet.Cells[linha, 5].Value = litSobe.Text.Trim();
                xlWorkSheet.Cells[linha, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 5].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litSobe.Text.Trim() == "+")
                    xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.PaleGreen);
                else
                    xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                linha += 1;
                posicao += 1;
            }


            //FILTROS E CONGELAMENTO
            xlWorkSheet.Panes = new WorksheetPanes(PanesState.Frozen, 0, 4, "A5", PanePosition.BottomLeft);
            var filterRange = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 0, linha, 5);
            filterRange.Filter(true).Apply();

            string atingidoFooter = GridViewVendas.FooterRow.Cells[1].Text.Replace("%", "").Trim();

            xlWorkSheet.Cells[linha, 1].Value = "Totais";
            xlWorkSheet.Cells[linha, 2].Value = atingidoFooter + "%";

            var footerRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 5);
            footerRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            footerRange.Style.Font.Weight = ExcelFont.BoldWeight;
            footerRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            footerRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            footerRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            //Controle sobe do footer
            var cellFooterBackColor = System.Drawing.Color.White;
            int atingidoIntFooter = Convert.ToInt32(Convert.ToDecimal(atingidoFooter));
            if (atingidoIntFooter >= 90)
                cellFooterBackColor = System.Drawing.Color.Green;
            else if (atingidoIntFooter < 90 && atingidoIntFooter >= 80)
                cellFooterBackColor = System.Drawing.Color.Yellow;
            else if (atingidoIntFooter < 80 && atingidoIntFooter >= 70)
                cellFooterBackColor = System.Drawing.Color.Red;
            else if (atingidoIntFooter < 70)
                cellFooterBackColor = System.Drawing.Color.MediumPurple;

            xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(cellFooterBackColor);

            //INICIO SEGUNDO QUADRO
            xlWorkSheet.Cells[3, 7].Value = "Cor";
            xlWorkSheet.Cells[3, 8].Value = "Descrição";
            xlWorkSheet.Cells[3, 9].Value = "Descrição Completa";

            var columnHeadings2Range = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 7, 7, 9);
            columnHeadings2Range.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            columnHeadings2Range.Style.Font.Weight = ExcelFont.BoldWeight;
            columnHeadings2Range.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            columnHeadings2Range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            columnHeadings2Range.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            xlWorkSheet.Columns[7].Width = 15 * 256;
            xlWorkSheet.Columns[8].Width = 30 * 256;
            xlWorkSheet.Columns[9].Width = 30 * 256;


            xlWorkSheet.Cells[4, 7].Style.FillPattern.SetSolid(System.Drawing.Color.Green);
            xlWorkSheet.Cells[4, 8].Value = "Situação de Meta";
            xlWorkSheet.Cells[4, 9].Value = "Superior a 90% da Meta";

            xlWorkSheet.Cells[5, 7].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            xlWorkSheet.Cells[5, 8].Value = "Situação de Atenção";
            xlWorkSheet.Cells[5, 9].Value = "Entre 81% e 90%";

            xlWorkSheet.Cells[6, 7].Style.FillPattern.SetSolid(System.Drawing.Color.Red);
            xlWorkSheet.Cells[6, 8].Value = "Situação de Alerta Máximo";
            xlWorkSheet.Cells[6, 9].Value = "Entre 70% e 80%";

            xlWorkSheet.Cells[7, 7].Style.FillPattern.SetSolid(System.Drawing.Color.MediumPurple);
            xlWorkSheet.Cells[7, 8].Value = "Situação Crítica";
            xlWorkSheet.Cells[7, 9].Value = "Abaixo de 70%";

            //INICIO TERCEIRO QUADRO
            xlWorkSheet.Cells[10, 7].Value = "Supervisor";
            xlWorkSheet.Cells[10, 8].Value = "Atingido";
            xlWorkSheet.Cells[10, 9].Value = "";
            xlWorkSheet.Cells[10, 10].Value = "";
            xlWorkSheet.Cells[10, 11].Value = "";
            xlWorkSheet.Cells[10, 12].Value = "";

            var columnHeadings3Range = xlWorkSheet.Cells.GetSubrangeAbsolute(10, 7, 10, 12);
            columnHeadings3Range.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            columnHeadings3Range.Style.Font.Weight = ExcelFont.BoldWeight;
            columnHeadings3Range.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            columnHeadings3Range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            columnHeadings3Range.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            linha = 11;
            foreach (GridViewRow row in GridViewResumo.Rows)
            {
                xlWorkSheet.Cells[linha, 7].Value = row.Cells[0].Text;
                xlWorkSheet.Cells[linha, 7].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                Literal litAtingido = row.FindControl("litAtingido") as Literal;
                xlWorkSheet.Cells[linha, 8].Value = litAtingido.Text.Trim();
                xlWorkSheet.Cells[linha, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 8].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                var cellResumoBackColor = System.Drawing.Color.White;
                int atingidoIntResumo = Convert.ToInt32(Convert.ToDecimal(litAtingido.Text.Replace("%", "").Trim()));
                if (atingidoIntResumo >= 90)
                    cellResumoBackColor = System.Drawing.Color.Green;
                else if (atingidoIntResumo < 90 && atingidoIntResumo >= 80)
                    cellResumoBackColor = System.Drawing.Color.Yellow;
                else if (atingidoIntResumo < 80 && atingidoIntResumo >= 70)
                    cellResumoBackColor = System.Drawing.Color.Red;
                else if (atingidoIntResumo < 70)
                    cellResumoBackColor = System.Drawing.Color.MediumPurple;
                xlWorkSheet.Cells[linha, 8].Style.FillPattern.SetSolid(cellResumoBackColor);

                xlWorkSheet.Cells[linha, 9].Value = row.Cells[2].Text;
                xlWorkSheet.Cells[linha, 9].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 9].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 9].Style.FillPattern.SetSolid(System.Drawing.Color.Green);

                xlWorkSheet.Cells[linha, 10].Value = row.Cells[3].Text;
                xlWorkSheet.Cells[linha, 10].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 10].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 10].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);

                xlWorkSheet.Cells[linha, 11].Value = row.Cells[4].Text;
                xlWorkSheet.Cells[linha, 11].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 11].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 11].Style.FillPattern.SetSolid(System.Drawing.Color.Red);

                xlWorkSheet.Cells[linha, 12].Value = row.Cells[5].Text;
                xlWorkSheet.Cells[linha, 12].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 12].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                xlWorkSheet.Cells[linha, 12].Style.FillPattern.SetSolid(System.Drawing.Color.MediumPurple);

                linha += 1;
            }


            string nomeArquivo = dataIni.Day.ToString() + "-A-" + dataFim.Day.ToString() + "-" + mesExtenso + "-FAROL.xlsx";
            string diretorioArquivo = Server.MapPath("..") + "\\Excel\\" + nomeArquivo;

            ef.Save(diretorioArquivo);

            return diretorioArquivo;
        }


    }
}
