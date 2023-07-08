using DAL;
using GemBox.Spreadsheet;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class gest_loja_compara_venda_super : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        public decimal totalVendaAtual = 0;
        public decimal totalVendaAnterior = 0;
        public decimal totalVendaMaurilhoAtual = 0;
        public decimal totalVendaMaurilhoAnterior = 0;
        public decimal totalVendaDirceuAtual = 0;
        public decimal totalVendaDirceuAnterior = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void CarregaGridViewVendas()
        {
            List<Vendas> listaVendasMaurilho = new List<Vendas>();
            List<Vendas> listaVendasDirceu = new List<Vendas>();
            List<Vendas> listaGeral = new List<Vendas>();

            string dataInicioAtual = baseController.AjustaData(txtDataInicioAtual.Text);
            string dataFimAtual = baseController.AjustaData(txtDataFimAtual.Text);

            int anoAnterior = Convert.ToInt32(dataInicioAtual.Substring(0, 4)) - 1;

            string dataInicioAnterior = anoAnterior.ToString() + dataInicioAtual.Substring(4, 4);
            string dataFimAnterior = anoAnterior.ToString() + dataFimAtual.Substring(4, 4);

            List<Vendas> listaVendas = desempenhoController.BuscaVendasAtualAnterior(dataInicioAtual, dataFimAtual, dataInicioAnterior, dataFimAnterior, true);

            if (listaVendas != null)
            {
                listaVendas = listaVendas.Where(p => p.ValorVendaAtual > 0 || p.ValorVendaAnterior > 0).OrderByDescending(z => z.ValorVendaAtual).ToList();

                GridViewVendas.DataSource = listaVendas;
                GridViewVendas.DataBind();

                USUARIO usuario = usuarioController.ValidaUsuarioSuper("maurilio");

                if (usuario != null)
                {
                    List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

                    if (filiais != null)
                    {
                        foreach (FILIAI filial in filiais)
                        {
                            foreach (Vendas item in listaVendas)
                            {
                                if (item.Filial.Trim().Equals(filial.FILIAL.Trim()))
                                {
                                    totalVendaMaurilhoAnterior += item.ValorVendaAnterior;
                                    totalVendaMaurilhoAtual += item.ValorVendaAtual;

                                    listaVendasMaurilho.Add(item);
                                }
                            }

                            listaVendasMaurilho = listaVendasMaurilho.OrderByDescending(c => c.ValorVendaAtual).ToList();

                            GridViewMaurilho.DataSource = listaVendasMaurilho;
                            GridViewMaurilho.DataBind();
                        }
                    }
                }

                usuario = usuarioController.ValidaUsuarioSuper("dirceu");

                if (usuario != null)
                {
                    List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

                    if (filiais != null)
                    {
                        foreach (FILIAI filial in filiais)
                        {
                            foreach (Vendas item in listaVendas)
                            {
                                if (item.Filial.Trim().Equals(filial.FILIAL.Trim()))
                                {
                                    totalVendaDirceuAnterior += item.ValorVendaAnterior;
                                    totalVendaDirceuAtual += item.ValorVendaAtual;

                                    listaVendasDirceu.Add(item);
                                }
                            }

                            listaVendasDirceu = listaVendasDirceu.OrderByDescending(d => d.ValorVendaAtual).ToList();

                            GridViewDirceu.DataSource = listaVendasDirceu;
                            GridViewDirceu.DataBind();
                        }
                    }
                }


                foreach (Vendas item in listaVendas)
                {
                    listaGeral.Add(item);
                }

                Vendas itemTotal = new Vendas();
                itemTotal.Filial = "";
                listaGeral.Add(itemTotal);

                itemTotal = new Vendas();
                itemTotal.Filial = "TOTAL REDE";
                itemTotal.ValorVendaAtual = totalVendaMaurilhoAtual + totalVendaDirceuAtual;
                itemTotal.ValorVendaAnterior = totalVendaMaurilhoAnterior + totalVendaDirceuAnterior;
                itemTotal.Diferenca = itemTotal.ValorVendaAtual - itemTotal.ValorVendaAnterior;
                if (itemTotal.ValorVendaAnterior > 0)
                    itemTotal.PercAtingido = Convert.ToInt32(((itemTotal.ValorVendaAtual / itemTotal.ValorVendaAnterior) - 1) * 100);
                else
                    itemTotal.PercAtingido = 0;
                listaGeral.Add(itemTotal);

                //Maurilho
                itemTotal = new Vendas();
                itemTotal.Filial = "";
                listaGeral.Add(itemTotal);

                foreach (Vendas item in listaVendasMaurilho)
                {
                    listaGeral.Add(item);
                }

                itemTotal = new Vendas();
                itemTotal.Filial = "";
                listaGeral.Add(itemTotal);

                itemTotal = new Vendas();
                itemTotal.Filial = "TOTAL MAURILHO";
                itemTotal.ValorVendaAtual = totalVendaMaurilhoAtual;
                itemTotal.ValorVendaAnterior = totalVendaMaurilhoAnterior;
                itemTotal.Diferenca = totalVendaMaurilhoAtual - totalVendaMaurilhoAnterior;
                if (totalVendaMaurilhoAnterior > 0)
                    itemTotal.PercAtingido = Convert.ToInt32(((totalVendaMaurilhoAtual / totalVendaMaurilhoAnterior) - 1) * 100);
                else
                    itemTotal.PercAtingido = 0;
                listaGeral.Add(itemTotal);

                //Dirceu
                itemTotal = new Vendas();
                itemTotal.Filial = "";
                listaGeral.Add(itemTotal);

                foreach (Vendas item in listaVendasDirceu)
                {
                    listaGeral.Add(item);
                }

                itemTotal = new Vendas();
                itemTotal.Filial = "";
                listaGeral.Add(itemTotal);

                itemTotal = new Vendas();
                itemTotal.Filial = "TOTAL DIRCEU";
                itemTotal.ValorVendaAtual = totalVendaDirceuAtual;
                itemTotal.ValorVendaAnterior = totalVendaDirceuAnterior;
                itemTotal.Diferenca = totalVendaDirceuAtual - totalVendaDirceuAnterior;
                if (totalVendaDirceuAnterior > 0)
                    itemTotal.PercAtingido = Convert.ToInt32(((totalVendaDirceuAtual / totalVendaDirceuAnterior) - 1) * 100);
                else
                    itemTotal.PercAtingido = 0;
                listaGeral.Add(itemTotal);

                Session["TotalRede"] = listaGeral;
                Session["Excel"] = false;
            }
            else
                Session["TotalRede"] = null;

        }

        protected void CalendarDataInicioAtual_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicioAtual.Text = CalendarDataInicioAtual.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFimAtual_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFimAtual.Text = CalendarDataFimAtual.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (txtDataInicioAtual.Text.Equals("") ||
                txtDataInicioAtual.Text == null)
                return;

            btGeraExcel.Enabled = true;

            CarregaGridViewVendas();
        }

        protected void GridViewVendas_DataBound(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["Excel"]))
            {
                GridViewRow footer = GridViewVendas.FooterRow;

                if (footer != null)
                {
                    footer.Cells[0].Text = "Totais";
                    footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                    footer.Cells[1].Text = totalVendaAtual.ToString("N2");
                    footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                    footer.Cells[2].Text = totalVendaAnterior.ToString("N2");
                    footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                    footer.Cells[3].Text = (totalVendaAtual - totalVendaAnterior).ToString("N2");
                    footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                    if (totalVendaAnterior > 0)
                        footer.Cells[4].Text = (((totalVendaAtual / totalVendaAnterior) - 1) * 100).ToString("N0") + "%";
                    footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;

                    if (footer.Cells[4].Text.Substring(0, 1).Equals("-"))
                        footer.Cells[4].BackColor = System.Drawing.Color.Red;
                    else
                        footer.Cells[4].BackColor = System.Drawing.Color.AliceBlue;
                }
            }

            foreach (GridViewRow item in GridViewVendas.Rows)
            {
                Literal literalPercentual = item.FindControl("LiteralPercentual") as Literal;

                if (!literalPercentual.Text.Equals(""))
                {
                    if (literalPercentual.Text.Substring(0, 1).Equals("-"))
                        item.Cells[4].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[4].BackColor = System.Drawing.Color.AliceBlue;
                }
            }
        }
        protected void GridViewVendas_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    literalPercentual.Text = Convert.ToDecimal(((vendas.ValorVendaAtual / vendas.ValorVendaAnterior) - 1) * 100).ToString("N0") + "%";

                totalVendaAtual += vendas.ValorVendaAtual;
                totalVendaAnterior += vendas.ValorVendaAnterior;

                if (vendas.Filial.ToLower() == "handbook online")
                    e.Row.BackColor = System.Drawing.Color.FloralWhite;
            }
        }

        protected void GridViewMaurilho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewMaurilho.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalVendaMaurilhoAtual.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = totalVendaMaurilhoAnterior.ToString("N2");
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = (totalVendaMaurilhoAtual - totalVendaMaurilhoAnterior).ToString("N2");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;

                if (totalVendaMaurilhoAnterior > 0)
                    footer.Cells[4].Text = (((totalVendaMaurilhoAtual / totalVendaMaurilhoAnterior) - 1) * 100).ToString("N0") + "%";

                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;

                if (footer.Cells[4].Text.Substring(0, 1).Equals("-"))
                    footer.Cells[4].BackColor = System.Drawing.Color.Red;
                else
                    footer.Cells[4].BackColor = System.Drawing.Color.AliceBlue;
            }

            foreach (GridViewRow item in GridViewMaurilho.Rows)
            {
                Literal literalPercentual = item.FindControl("LiteralPercentual") as Literal;

                if (!literalPercentual.Text.Equals(""))
                {
                    if (literalPercentual.Text.Substring(0, 1).Equals("-"))
                        item.Cells[4].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[4].BackColor = System.Drawing.Color.AliceBlue;
                }
            }
        }
        protected void GridViewDirceu_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDirceu.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalVendaDirceuAtual.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = totalVendaDirceuAnterior.ToString("N2");
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = (totalVendaDirceuAtual - totalVendaDirceuAnterior).ToString("N2");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                if (totalVendaDirceuAnterior > 0)
                    footer.Cells[4].Text = (((totalVendaDirceuAtual / totalVendaDirceuAnterior) - 1) * 100).ToString("N0") + "%";
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;

                if (footer.Cells[4].Text.Substring(0, 1).Equals("-"))
                    footer.Cells[4].BackColor = System.Drawing.Color.Red;
                else
                    footer.Cells[4].BackColor = System.Drawing.Color.AliceBlue;
            }

            foreach (GridViewRow item in GridViewDirceu.Rows)
            {
                Literal literalPercentual = item.FindControl("LiteralPercentual") as Literal;

                if (!literalPercentual.Text.Equals(""))
                {
                    if (literalPercentual.Text.Substring(0, 1).Equals("-"))
                        item.Cells[4].BackColor = System.Drawing.Color.Red;
                    else
                        item.Cells[4].BackColor = System.Drawing.Color.AliceBlue;
                }
            }
        }

        protected void btGeraExcel_Click(object sender, EventArgs e)
        {
            try
            {
                labMsgEmail.Text = "";

                var diretorioArquivo = GerarExcel();

                //Enviar Email
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();

                var assunto = "[Intranet] - TOTAL REDE - " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " - " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.ANEXO = diretorioArquivo;
                email.MENSAGEM = "Anexo total rede lojas no período de " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " à " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(54, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
                foreach (var usu in usuarioEmail)
                    if (usu != null)
                        destinatario.Add(usu.EMAIL);
                //Adicionar remetente
                destinatario.Add(usuario.EMAIL);

                email.DESTINATARIOS = destinatario;

                if (destinatario.Count > 0)
                    email.EnviarEmail();


                labMsgEmail.Text = "E-Mail Enviado com Sucesso.";
            }
            catch (Exception ex)
            {
                labMsgEmail.Text = ex.Message;
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
            xlWorkSheet.Cells[3, 2].Value = dataIni.Year;
            xlWorkSheet.Cells[3, 3].Value = (dataIni.Year - 1);
            xlWorkSheet.Cells[3, 4].Value = "Diferença";
            xlWorkSheet.Cells[3, 5].Value = "% Crescimento";

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
            xlWorkSheet.Columns[5].Width = 20 * 256;

            //INICIO PRIMEIRO QUADRO
            int linha = 4;
            int posicao = 1;

            decimal totVendaAtual = 0;
            decimal totVendaAnterior = 0;
            foreach (GridViewRow row in GridViewVendas.Rows)
            {
                Literal litVendaAtual = row.FindControl("LiteralVendasAtual") as Literal;
                Literal litVendaAnterior = row.FindControl("LiteralVendasAnterior") as Literal;
                Literal litDiff = row.FindControl("LiteralDiferenca") as Literal;
                Literal litPercentual = row.FindControl("LiteralPercentual") as Literal;

                if ((litVendaAtual.Text != "" && Convert.ToDecimal(litVendaAtual.Text) <= 0) && (litVendaAnterior.Text != "" && Convert.ToDecimal(litVendaAnterior.Text) <= 0))
                    continue;

                xlWorkSheet.Cells[linha, 0].Value = posicao.ToString();
                xlWorkSheet.Cells[linha, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 0].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 1].Value = row.Cells[0].Text;
                xlWorkSheet.Cells[linha, 1].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 2].Value = Convert.ToDecimal(litVendaAtual.Text.Trim());
                xlWorkSheet.Cells[linha, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 2].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 3].Value = Convert.ToDecimal(litVendaAnterior.Text.Trim());
                xlWorkSheet.Cells[linha, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 3].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 4].Value = Convert.ToDecimal(litDiff.Text.Trim());
                xlWorkSheet.Cells[linha, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 4].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litDiff.Text.Trim().Contains("-"))
                    xlWorkSheet.Cells[linha, 4].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                xlWorkSheet.Cells[linha, 5].Value = litPercentual.Text.Trim();
                xlWorkSheet.Cells[linha, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 5].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litPercentual.Text.Trim().Contains("-"))
                    xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                linha += 1;
                posicao += 1;

                totVendaAtual += Convert.ToDecimal(litVendaAtual.Text.Trim());
                totVendaAnterior += Convert.ToDecimal(litVendaAnterior.Text.Trim());
            }

            //FILTROS E CONGELAMENTO
            xlWorkSheet.Panes = new WorksheetPanes(PanesState.Frozen, 0, 4, "A5", PanePosition.BottomLeft);
            var filterRange = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 0, linha, 5);
            filterRange.Filter(true).Apply();

            xlWorkSheet.Cells[linha, 1].Value = "Totais";
            xlWorkSheet.Cells[linha, 2].Formula = "=SUBTOTAL(9, C5:C" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 3].Formula = "=SUBTOTAL(9, D5:D" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 4].Formula = "=SUBTOTAL(9, E5:E" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 5].Formula = "=(C" + (linha + 1) + "/D" + (linha + 1) + ")-1";
            xlWorkSheet.Cells[linha, 5].Style.NumberFormat = "###,##%";

            var footerRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 5);
            footerRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            footerRange.Style.Font.Weight = ExcelFont.BoldWeight;
            footerRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            footerRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            footerRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            if ((totVendaAtual / totVendaAnterior) - 1 > 0)
                xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.YellowGreen);
            else
                xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 2, linha, 2).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 3, linha, 3).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 4, linha, 4).Style.NumberFormat = "R$ ###,###,##0.00";

            //SEGUNDO QUADRO - 1 SUPERVISOR
            linha = linha + 2;

            xlWorkSheet.Cells[linha, 0].Value = "MAURILHO";
            xlWorkSheet.Cells[linha, 1].Value = "Filial";
            xlWorkSheet.Cells[linha, 2].Value = dataIni.Year;
            xlWorkSheet.Cells[linha, 3].Value = (dataIni.Year - 1);
            xlWorkSheet.Cells[linha, 4].Value = "Diferença";
            xlWorkSheet.Cells[linha, 5].Value = "% Crescimento";

            columnHeadingsRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 5);
            columnHeadingsRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            columnHeadingsRange.Style.Font.Weight = ExcelFont.BoldWeight;
            columnHeadingsRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            columnHeadingsRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            columnHeadingsRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            xlWorkSheet.Rows[linha].Height = 20 * 20;

            linha = linha + 1;
            int linhaIni = linha + 1;
            posicao = 1;
            totVendaAtual = 0;
            totVendaAnterior = 0;
            foreach (GridViewRow row in GridViewMaurilho.Rows)
            {
                Literal litVendaAtual = row.FindControl("LiteralVendasAtual") as Literal;
                Literal litVendaAnterior = row.FindControl("LiteralVendasAnterior") as Literal;
                Literal litDiff = row.FindControl("LiteralDiferenca") as Literal;
                Literal litPercentual = row.FindControl("LiteralPercentual") as Literal;

                if ((litVendaAtual.Text != "" && Convert.ToDecimal(litVendaAtual.Text) <= 0) && (litVendaAnterior.Text != "" && Convert.ToDecimal(litVendaAnterior.Text) <= 0))
                    continue;

                xlWorkSheet.Cells[linha, 0].Value = posicao.ToString();
                xlWorkSheet.Cells[linha, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 0].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 1].Value = row.Cells[0].Text;
                xlWorkSheet.Cells[linha, 1].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 2].Value = Convert.ToDecimal(litVendaAtual.Text.Trim());
                xlWorkSheet.Cells[linha, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 2].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 3].Value = Convert.ToDecimal(litVendaAnterior.Text.Trim());
                xlWorkSheet.Cells[linha, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 3].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 4].Value = Convert.ToDecimal(litDiff.Text.Trim());
                xlWorkSheet.Cells[linha, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 4].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litDiff.Text.Trim().Contains("-"))
                    xlWorkSheet.Cells[linha, 4].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                xlWorkSheet.Cells[linha, 5].Value = litPercentual.Text.Trim();
                xlWorkSheet.Cells[linha, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 5].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litPercentual.Text.Trim().Contains("-"))
                    xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                linha += 1;
                posicao += 1;

                totVendaAtual += Convert.ToDecimal(litVendaAtual.Text.Trim());
                totVendaAnterior += Convert.ToDecimal(litVendaAnterior.Text.Trim());
            }

            xlWorkSheet.Cells[linha, 1].Value = "Totais";
            xlWorkSheet.Cells[linha, 2].Formula = "=SUBTOTAL(9, C" + linhaIni + ":C" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 3].Formula = "=SUBTOTAL(9, D" + linhaIni + ":D" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 4].Formula = "=SUBTOTAL(9, E" + linhaIni + ":E" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 5].Formula = "=(C" + (linha + 1) + "/D" + (linha + 1) + ")-1";
            xlWorkSheet.Cells[linha, 5].Style.NumberFormat = "###,##%";

            footerRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 5);
            footerRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            footerRange.Style.Font.Weight = ExcelFont.BoldWeight;
            footerRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            footerRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            footerRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            if ((totVendaAtual / totVendaAnterior) - 1 > 0)
                xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.YellowGreen);
            else
                xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);

            xlWorkSheet.Cells.GetSubrangeAbsolute(linhaIni, 2, linha, 2).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(linhaIni, 3, linha, 3).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(linhaIni, 4, linha, 4).Style.NumberFormat = "R$ ###,###,##0.00";

            //TERCEIRO QUADRO - 2 SUPERVISOR
            linha = linha + 2;

            xlWorkSheet.Cells[linha, 0].Value = "DIRCEU";
            xlWorkSheet.Cells[linha, 1].Value = "Filial";
            xlWorkSheet.Cells[linha, 2].Value = dataIni.Year;
            xlWorkSheet.Cells[linha, 3].Value = (dataIni.Year - 1);
            xlWorkSheet.Cells[linha, 4].Value = "Diferença";
            xlWorkSheet.Cells[linha, 5].Value = "% Crescimento";

            columnHeadingsRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 5);
            columnHeadingsRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            columnHeadingsRange.Style.Font.Weight = ExcelFont.BoldWeight;
            columnHeadingsRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            columnHeadingsRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            columnHeadingsRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            xlWorkSheet.Rows[linha].Height = 20 * 20;
            linha = linha + 1;
            linhaIni = linha + 1;
            posicao = 1;
            totVendaAtual = 0;
            totVendaAnterior = 0;
            foreach (GridViewRow row in GridViewDirceu.Rows)
            {
                Literal litVendaAtual = row.FindControl("LiteralVendasAtual") as Literal;
                Literal litVendaAnterior = row.FindControl("LiteralVendasAnterior") as Literal;
                Literal litDiff = row.FindControl("LiteralDiferenca") as Literal;
                Literal litPercentual = row.FindControl("LiteralPercentual") as Literal;

                if ((litVendaAtual.Text != "" && Convert.ToDecimal(litVendaAtual.Text) <= 0) && (litVendaAnterior.Text != "" && Convert.ToDecimal(litVendaAnterior.Text) <= 0))
                    continue;

                xlWorkSheet.Cells[linha, 0].Value = posicao.ToString();
                xlWorkSheet.Cells[linha, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 0].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 1].Value = row.Cells[0].Text;
                xlWorkSheet.Cells[linha, 1].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 2].Value = Convert.ToDecimal(litVendaAtual.Text.Trim());
                xlWorkSheet.Cells[linha, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 2].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 3].Value = Convert.ToDecimal(litVendaAnterior.Text.Trim());
                xlWorkSheet.Cells[linha, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 3].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 4].Value = Convert.ToDecimal(litDiff.Text.Trim());
                xlWorkSheet.Cells[linha, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 4].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litDiff.Text.Trim().Contains("-"))
                    xlWorkSheet.Cells[linha, 4].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                xlWorkSheet.Cells[linha, 5].Value = litPercentual.Text.Trim();
                xlWorkSheet.Cells[linha, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
                xlWorkSheet.Cells[linha, 5].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litPercentual.Text.Trim().Contains("-"))
                    xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.Pink);

                linha += 1;
                posicao += 1;

                totVendaAtual += Convert.ToDecimal(litVendaAtual.Text.Trim());
                totVendaAnterior += Convert.ToDecimal(litVendaAnterior.Text.Trim());
            }

            xlWorkSheet.Cells[linha, 1].Value = "Totais";
            xlWorkSheet.Cells[linha, 2].Formula = "=SUBTOTAL(9, C" + linhaIni + ":C" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 3].Formula = "=SUBTOTAL(9, D" + linhaIni + ":D" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 4].Formula = "=SUBTOTAL(9, E" + linhaIni + ":E" + (linha).ToString() + ")";
            xlWorkSheet.Cells[linha, 5].Formula = "=(C" + (linha + 1) + "/D" + (linha + 1) + ")-1";
            xlWorkSheet.Cells[linha, 5].Style.NumberFormat = "###,##%";

            footerRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 5);
            footerRange.Style.FillPattern.SetSolid(System.Drawing.Color.PeachPuff);
            footerRange.Style.Font.Weight = ExcelFont.BoldWeight;
            footerRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            footerRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            footerRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

            if ((totVendaAtual / totVendaAnterior) - 1 > 0)
                xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.YellowGreen);
            else
                xlWorkSheet.Cells[linha, 5].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);

            xlWorkSheet.Cells.GetSubrangeAbsolute(linhaIni, 2, linha, 2).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(linhaIni, 3, linha, 3).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(linhaIni, 4, linha, 4).Style.NumberFormat = "R$ ###,###,##0.00";

            string nomeArquivo = dataIni.Day.ToString() + "-A-" + dataFim.Day.ToString() + "-" + mesExtenso + "-TOTAL_REDE.xlsx";
            string diretorioArquivo = Server.MapPath("..") + "\\Excel\\" + nomeArquivo;

            ef.Save(diretorioArquivo);

            return diretorioArquivo;
        }


    }
}
