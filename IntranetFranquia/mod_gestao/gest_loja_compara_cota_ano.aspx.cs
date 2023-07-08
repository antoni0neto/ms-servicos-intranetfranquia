using DAL;
using GemBox.Spreadsheet;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class gest_loja_compara_cota_ano : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        public decimal totalVendaAtual = 0;
        public decimal totalVendaAnterior = 0;
        public decimal totalValorCota = 0;
        public decimal totalGeralVendaAtual = 0;
        public decimal totalGeralVendaAnterior = 0;
        public decimal totalGeralValorCota = 0;

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

        protected void btPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (txtDataInicioAtual.Text.Equals("") ||
                txtDataInicioAtual.Text == null)
                return;

            CarregaGridViewVendas();

            btGeraExcel.Enabled = true;
        }

        private void CarregaGridViewVendas()
        {
            string dataInicioAtual = baseController.AjustaData(txtDataInicioAtual.Text);
            string dataFimAtual = baseController.AjustaData(txtDataFimAtual.Text);

            int anoAnterior = Convert.ToInt32(dataInicioAtual.Substring(0, 4)) - 1;

            string dataInicioAnterior = anoAnterior.ToString() + dataInicioAtual.Substring(4, 4);
            string dataFimAnterior = anoAnterior.ToString() + dataFimAtual.Substring(4, 4);

            DifereVendaCota difereVendaCota = null;

            List<Vendas> listaVendas = desempenhoController.BuscaVendasAtualAnterior(dataInicioAtual, dataFimAtual, dataInicioAnterior, dataFimAnterior, false);

            if (listaVendas != null)
            {
                listaVendas = listaVendas.OrderByDescending(z => z.PercAtingido).ToList();

                List<DifereVendaCota> listaDifereVendaCota = new List<DifereVendaCota>();

                List<USUARIO> supervisores = baseController.BuscaSupervisores();

                if (supervisores != null)
                {
                    foreach (USUARIO supervisor in supervisores)
                    {
                        List<FILIAI> filiais = baseController.BuscaFiliais(supervisor);

                        if (filiais != null && filiais.Count > 0)
                        {
                            totalVendaAtual = 0;
                            totalVendaAnterior = 0;
                            totalValorCota = 0;

                            foreach (FILIAI filial in filiais)
                            {
                                foreach (Vendas item in listaVendas)
                                {
                                    if (item.Filial.Trim().Equals(filial.FILIAL.Trim()))
                                    {
                                        difereVendaCota = new DifereVendaCota();

                                        difereVendaCota.Supervisor = supervisor.NOME_USUARIO.ToUpper().Trim();
                                        difereVendaCota.Filial = item.Filial.Trim();
                                        difereVendaCota.ValorVendaAtual = item.ValorVendaAtual;
                                        difereVendaCota.ValorVendaAnterior = item.ValorVendaAnterior;
                                        difereVendaCota.ValorCota = baseController.BuscaTotalCotasPeriodo(filial.COD_FILIAL, CalendarDataInicioAtual.SelectedDate, CalendarDataFimAtual.SelectedDate);

                                        if (difereVendaCota.ValorCota != null)
                                        {
                                            difereVendaCota.Diferenca = difereVendaCota.ValorVendaAtual - Convert.ToDecimal(difereVendaCota.ValorCota);

                                            if (difereVendaCota.ValorCota > 0)
                                                difereVendaCota.PercAtingido = Convert.ToInt32((difereVendaCota.ValorVendaAtual / Convert.ToDecimal(difereVendaCota.ValorCota)) * 100);

                                            if (difereVendaCota.ValorVendaAnterior > 0)
                                                difereVendaCota.DiferencaAtingido = Convert.ToInt32((difereVendaCota.ValorVendaAtual / difereVendaCota.ValorVendaAnterior) * 100) - 100;
                                        }

                                        totalVendaAnterior += difereVendaCota.ValorVendaAnterior;
                                        totalVendaAtual += difereVendaCota.ValorVendaAtual;
                                        totalValorCota += Convert.ToDecimal(difereVendaCota.ValorCota);

                                        totalGeralVendaAnterior += difereVendaCota.ValorVendaAnterior;
                                        totalGeralVendaAtual += difereVendaCota.ValorVendaAtual;
                                        totalGeralValorCota += Convert.ToDecimal(difereVendaCota.ValorCota);

                                        listaDifereVendaCota.Add(difereVendaCota);
                                    }
                                }
                            }

                            difereVendaCota = new DifereVendaCota();

                            difereVendaCota.Supervisor = "Sub-Total";
                            difereVendaCota.Filial = "";
                            difereVendaCota.ValorVendaAtual = totalVendaAtual;
                            difereVendaCota.ValorVendaAnterior = totalVendaAnterior;
                            difereVendaCota.ValorCota = totalValorCota;

                            if (difereVendaCota.ValorCota != null)
                            {
                                difereVendaCota.Diferenca = difereVendaCota.ValorVendaAtual - Convert.ToDecimal(difereVendaCota.ValorCota);

                                if (difereVendaCota.ValorCota > 0)
                                    difereVendaCota.PercAtingido = Convert.ToInt32((difereVendaCota.ValorVendaAtual / Convert.ToDecimal(difereVendaCota.ValorCota)) * 100);

                                if (difereVendaCota.ValorVendaAnterior > 0)
                                    difereVendaCota.DiferencaAtingido = Convert.ToInt32((difereVendaCota.ValorVendaAtual / difereVendaCota.ValorVendaAnterior) * 100) - 100;
                            }

                            listaDifereVendaCota.Add(difereVendaCota);
                        }
                    }
                }

                GridViewVendas.DataSource = listaDifereVendaCota;
                GridViewVendas.DataBind();

                Session["Cotas"] = listaDifereVendaCota;
            }
            else
                Session["Cotas"] = null;
        }
        protected void GridViewVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DifereVendaCota vendas = e.Row.DataItem as DifereVendaCota;

            if (vendas != null)
            {
                Literal literalCotas = e.Row.FindControl("LiteralCotas") as Literal;
                if (literalCotas != null)
                    literalCotas.Text = Convert.ToDecimal(vendas.ValorCota).ToString("N2");

                Literal literalVendas = e.Row.FindControl("LiteralVendas") as Literal;
                if (literalVendas != null)
                    literalVendas.Text = Convert.ToDecimal(vendas.ValorVendaAtual).ToString("N2");

                Literal literalAtingido = e.Row.FindControl("LiteralAtingido") as Literal;
                if (literalAtingido != null)
                    literalAtingido.Text = vendas.PercAtingido.ToString() + "%";

                Literal literalDiferenca = e.Row.FindControl("LiteralDiferenca") as Literal;
                if (literalDiferenca != null)
                    literalDiferenca.Text = Convert.ToDecimal(vendas.Diferenca).ToString("N2");

                Literal literalVendasAnoAnterior = e.Row.FindControl("LiteralVendasAnoAnterior") as Literal;
                if (literalVendasAnoAnterior != null)
                    literalVendasAnoAnterior.Text = Convert.ToDecimal(vendas.ValorVendaAnterior).ToString("N2");

                Literal literalDiferencaAtingido = e.Row.FindControl("LiteralDiferencaAtingido") as Literal;
                if (literalDiferencaAtingido != null)
                    literalDiferencaAtingido.Text = vendas.DiferencaAtingido.ToString("") + "%";
            }
        }
        protected void GridViewVendas_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendas.Rows)
            {
                Literal literalDiferenca = item.FindControl("LiteralDiferenca") as Literal;
                if (literalDiferenca.Text.Substring(0, 1).Equals("-"))
                    item.Cells[5].BackColor = System.Drawing.Color.Pink;

                Literal literalDiferencaAtingido = item.FindControl("LiteralDiferencaAtingido") as Literal;
                if (literalDiferencaAtingido.Text.Substring(0, 1).Equals("-"))
                    item.Cells[7].BackColor = System.Drawing.Color.Pink;

                if (item.Cells[0].Text.Equals("Sub-Total"))
                    item.BackColor = System.Drawing.Color.LightGray;

            }

            GridViewRow footer = GridViewVendas.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total Rede";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = "";
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = totalGeralValorCota.ToString("N2");
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalGeralVendaAtual.ToString("N2");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                if (totalGeralValorCota > 0)
                {
                    footer.Cells[4].Text = ((totalGeralVendaAtual / totalGeralValorCota) * 100).ToString("N0") + "%";
                    footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                }

                footer.Cells[5].Text = (totalGeralVendaAtual - totalGeralValorCota).ToString("N2");
                if (footer.Cells[5].Text.Substring(0, 1).Equals("-"))
                    footer.Cells[5].BackColor = System.Drawing.Color.Pink;
                else
                    footer.Cells[5].BackColor = System.Drawing.Color.AliceBlue;
                footer.Cells[6].Text = totalGeralVendaAnterior.ToString("N2");
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;

                if (totalGeralVendaAnterior > 0)
                {
                    var porcTotal = (totalGeralVendaAtual / totalGeralVendaAnterior - 1);
                    footer.Cells[7].Text = (porcTotal * 100).ToString("N0") + "%";
                    footer.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
                    if (footer.Cells[7].Text.Substring(0, 1).Equals("-"))
                        footer.Cells[7].BackColor = System.Drawing.Color.Pink;
                    else
                        footer.Cells[7].BackColor = System.Drawing.Color.AliceBlue;
                }
            }
        }

        #region "RELATORIO/EXCEL"
        //protected void btRelatorio_Click(object sender, EventArgs e)
        //{
        //    if (GridViewVendas.Rows.Count > 0)
        //    {
        //        Document document = new Document(PageSize.A4.Rotate(), 30, 30, 30, 30);

        //        document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Width, iTextSharp.text.PageSize.LETTER.Height));

        //        try
        //        {
        //            PdfWriter.GetInstance(document, new FileStream("c:\\bkp\\vendacota.pdf", FileMode.Create));
        //            document.AddAuthor("xxx");
        //            document.AddSubject("xxxx");
        //            document.Open();
        //            iTextSharp.text.Table datatable = new iTextSharp.text.Table(8);
        //            datatable.Padding = 4;
        //            datatable.Spacing = 0;
        //            float[] headerwidths = { 20, 30, 20, 20, 10, 20, 20, 10 };
        //            datatable.Widths = headerwidths;
        //            Cell cell = new Cell(new Phrase(@"Venda x Cota - Início: " + txtDataInicioAtual.Text + " - Fim: " + txtDataFimAtual.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            cell.Leading = 5;
        //            cell.Colspan = 8;
        //            cell.Border = Rectangle.NO_BORDER;
        //            cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //            datatable.AddCell(cell);
        //            datatable.DefaultCellBorderWidth = 2;
        //            datatable.DefaultHorizontalAlignment = 1;
        //            datatable.DefaultRowspan = 2;
        //            datatable.AddCell(new Phrase("Supervisor", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("Filial", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("Cotas", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("Vendas", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("Atingido", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("Diferença", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("Venda Anterior", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("%", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.DefaultColspan = 1;
        //            datatable.EndHeaders();
        //            datatable.DefaultCellBorderWidth = 1;
        //            datatable.DefaultRowspan = 1;

        //            decimal vendaAtual = 0;
        //            decimal vendaAnterior = 0;
        //            decimal valorCota = 0;

        //            foreach (GridViewRow item in GridViewVendas.Rows)
        //            {
        //                datatable.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
        //                if (item.Cells[0].Text.Equals("Sub-Total"))
        //                {
        //                    cell = new Cell(new Phrase(item.Cells[0].Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                    cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                    datatable.AddCell(cell);
        //                }
        //                else
        //                    datatable.AddCell(new Phrase(item.Cells[0].Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));

        //                if (item.Cells[0].Text.Equals("Sub-Total"))
        //                {
        //                    cell = new Cell(new Phrase(item.Cells[1].Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                    cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                    datatable.AddCell(cell);
        //                }
        //                else
        //                    datatable.AddCell(new Phrase(item.Cells[1].Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                datatable.DefaultHorizontalAlignment = Element.ALIGN_CENTER;

        //                Literal literalCotas = item.FindControl("LiteralCotas") as Literal;

        //                if (literalCotas != null)
        //                {
        //                    if (item.Cells[0].Text.Equals("Sub-Total"))
        //                    {
        //                        cell = new Cell(new Phrase(literalCotas.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                        cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                        datatable.AddCell(cell);
        //                    }
        //                    else
        //                        datatable.AddCell(new Phrase(literalCotas.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));

        //                    if (!item.Cells[0].Text.Equals("Sub-Total"))
        //                        valorCota += Convert.ToDecimal(literalCotas.Text);
        //                }

        //                Literal literalVendas = item.FindControl("LiteralVendas") as Literal;

        //                if (literalVendas != null)
        //                {
        //                    if (item.Cells[0].Text.Equals("Sub-Total"))
        //                    {
        //                        cell = new Cell(new Phrase(literalVendas.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                        cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                        datatable.AddCell(cell);
        //                    }
        //                    else
        //                        datatable.AddCell(new Phrase(literalVendas.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));

        //                    if (!item.Cells[0].Text.Equals("Sub-Total"))
        //                        vendaAtual += Convert.ToDecimal(literalVendas.Text);
        //                }

        //                Literal literalAtingido = item.FindControl("LiteralAtingido") as Literal;

        //                if (literalAtingido != null)
        //                {
        //                    if (item.Cells[0].Text.Equals("Sub-Total"))
        //                    {
        //                        cell = new Cell(new Phrase(literalAtingido.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                        cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                        datatable.AddCell(cell);
        //                    }
        //                    else
        //                        datatable.AddCell(new Phrase(literalAtingido.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                }
        //                Literal literalDiferenca = item.FindControl("LiteralDiferenca") as Literal;

        //                if (literalDiferenca != null)
        //                {
        //                    if (literalDiferenca.Text.Substring(0, 1).Equals("-"))
        //                    {
        //                        if (item.Cells[0].Text.Equals("Sub-Total"))
        //                        {
        //                            cell = new Cell(new Phrase(literalDiferenca.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                            cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                            datatable.AddCell(cell);
        //                        }
        //                        else
        //                            datatable.AddCell(new Phrase(literalDiferenca.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.RED)));
        //                    }
        //                    else
        //                    {
        //                        if (item.Cells[0].Text.Equals("Sub-Total"))
        //                        {
        //                            cell = new Cell(new Phrase(literalDiferenca.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                            cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                            datatable.AddCell(cell);
        //                        }
        //                        else
        //                            datatable.AddCell(new Phrase(literalDiferenca.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.BLUE)));
        //                    }
        //                }

        //                Literal literalVendasAnoAnterior = item.FindControl("LiteralVendasAnoAnterior") as Literal;

        //                if (literalVendasAnoAnterior != null)
        //                {
        //                    if (item.Cells[0].Text.Equals("Sub-Total"))
        //                    {
        //                        cell = new Cell(new Phrase(literalVendasAnoAnterior.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                        cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                        datatable.AddCell(cell);
        //                    }
        //                    else
        //                        datatable.AddCell(new Phrase(literalVendasAnoAnterior.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));

        //                    if (!item.Cells[0].Text.Equals("Sub-Total"))
        //                        vendaAnterior += Convert.ToDecimal(literalVendasAnoAnterior.Text);
        //                }

        //                Literal literalDiferencaAtingido = item.FindControl("LiteralDiferencaAtingido") as Literal;

        //                if (literalDiferencaAtingido != null)
        //                {
        //                    if (literalDiferencaAtingido.Text.Substring(0, 1).Equals("-"))
        //                    {
        //                        if (item.Cells[0].Text.Equals("Sub-Total"))
        //                        {
        //                            cell = new Cell(new Phrase(literalDiferencaAtingido.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                            cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                            datatable.AddCell(cell);
        //                        }
        //                        else
        //                            datatable.AddCell(new Phrase(literalDiferencaAtingido.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.RED)));
        //                    }
        //                    else
        //                    {
        //                        if (item.Cells[0].Text.Equals("Sub-Total"))
        //                        {
        //                            cell = new Cell(new Phrase(literalDiferencaAtingido.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //                            cell.BackgroundColor = new Color(0xC0, 0xC0, 0xC0);
        //                            datatable.AddCell(cell);
        //                        }
        //                        else
        //                            datatable.AddCell(new Phrase(literalDiferencaAtingido.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.BLUE)));
        //                    }
        //                }
        //            }

        //            datatable.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
        //            datatable.AddCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase("Total Rede", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
        //            datatable.AddCell(new Phrase(valorCota.ToString("N2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase(vendaAtual.ToString("N2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));

        //            if (valorCota > 0)
        //            {
        //                if (((vendaAtual / valorCota) * 100).ToString("N0").Substring(0, 1).Equals("-"))
        //                    datatable.AddCell(new Phrase(((vendaAtual / valorCota) * 100).ToString("N0") + "%", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.RED)));
        //                else
        //                    datatable.AddCell(new Phrase(((vendaAtual / valorCota) * 100).ToString("N0") + "%", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.BLUE)));
        //            }

        //            datatable.AddCell(new Phrase((vendaAtual - valorCota).ToString("N2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));
        //            datatable.AddCell(new Phrase(vendaAnterior.ToString("N2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)));

        //            if (valorCota > 0)
        //            {
        //                decimal atingidoAtual = (vendaAtual / valorCota) * 100;
        //                decimal atingidoAnterior = (vendaAnterior / valorCota) * 100;

        //                if ((atingidoAtual - atingidoAnterior).ToString("N0").Substring(0, 1).Equals("-"))
        //                    datatable.AddCell(new Phrase((atingidoAtual - atingidoAnterior).ToString("N0") + "%", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.RED)));
        //                else
        //                    datatable.AddCell(new Phrase((atingidoAtual - atingidoAnterior).ToString("N0") + "%", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.BLUE)));
        //            }

        //            document.Add(datatable);
        //            document.Close();

        //            string msg = "";

        //            MailMessage mail = new MailMessage();

        //            USUARIO usuario = (USUARIO)Session["USUARIO"];

        //            if (usuario != null)
        //            {
        //                msg = "Arquivo em anexo enviado pelo usuário: " + usuario.NOME_USUARIO;
        //                mail.From = new MailAddress(usuario.EMAIL);
        //            }
        //            else
        //            {
        //                msg = "Arquivo em anexo enviado pelo TI";
        //                mail.From = new MailAddress("ti@hbf.com.br");
        //            }

        //            //+ Supervisores
        //            mail.To.Add("dennis@hbf.com.br");
        //            mail.To.Add("tathiana@hbf.com.br");
        //            mail.To.Add("luciana@hbf.com.br");
        //            mail.To.Add("sergio@hbf.com.br");
        //            mail.To.Add("marcio.campos@hbf.com.br");
        //            mail.To.Add("leandro.bevilaqua@hbf.com.br");
        //            mail.Subject = "Compara Vendas x Cotas ";
        //            mail.Attachments.Add(new Attachment("c:\\bkp\\vendacota.pdf"));
        //            mail.Body = msg;

        //            SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");

        //            smtp.Port = 587;
        //            smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

        //            smtp.Send(mail);

        //            Response.Redirect("~/FinalizadoComSucesso.aspx");
        //        }
        //        catch (IOException ex)
        //        {
        //            document.Close();
        //            throw ex;
        //        }
        //        catch (Exception ex)
        //        {
        //            document.Close();
        //            throw ex;
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensagem", @"window.alert('Sem Clientes\Profissionais!');", true);
        //    }
        //}

        protected void btGeraExcel_Click(object sender, EventArgs e)
        {
            try
            {
                labMsgEmail.Text = "";

                var diretorioArquivo = GerarExcel();

                //Enviar Email
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                email_envio email = new email_envio();

                var assunto = "[Intranet] - COTA X VENDA - ANO ANTERIOR - " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " - " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");
                email.ASSUNTO = assunto;
                email.REMETENTE = usuario;
                email.ANEXO = diretorioArquivo;
                email.MENSAGEM = "Anexo cota x venda do ano anterior das lojas no período de " + Convert.ToDateTime(txtDataInicioAtual.Text).ToString("dd/MM/yyyy") + " à " + Convert.ToDateTime(txtDataFimAtual.Text).ToString("dd/MM/yyyy");

                List<string> destinatario = new List<string>();
                //Adiciona e-mails 
                var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(53, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
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
            var titleRange = xlWorkSheet.Cells.GetSubrangeAbsolute(2, 0, 2, 7);
            titleRange.Merged = true;
            titleRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            titleRange.Style.Font.Weight = ExcelFont.BoldWeight;

            xlWorkSheet.Cells[3, 0].Value = "Supervisor";
            xlWorkSheet.Cells[3, 1].Value = "Filial";
            xlWorkSheet.Cells[3, 2].Value = "Cotas";
            xlWorkSheet.Cells[3, 3].Value = "Venda Atual";
            xlWorkSheet.Cells[3, 4].Value = "Atingido";
            xlWorkSheet.Cells[3, 5].Value = "Diferença";
            xlWorkSheet.Cells[3, 6].Value = "Venda Ano Anterior";
            xlWorkSheet.Cells[3, 7].Value = "%";

            var columnHeadingsRange = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 0, 3, 7);
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
            xlWorkSheet.Columns[6].Width = 30 * 256;
            xlWorkSheet.Columns[7].Width = 15 * 256;


            int linhaSubTotal = 5;
            int linha = 4;

            decimal valorTotalCota = 0;
            decimal valorTotalVendaAtual = 0;
            decimal valorTotalDiff = 0;
            decimal valorTotalVendaAnterior = 0;

            foreach (GridViewRow row in GridViewVendas.Rows)
            {
                Literal litCotas = row.FindControl("LiteralCotas") as Literal;
                Literal litVendaAtual = row.FindControl("LiteralVendas") as Literal;
                Literal litAtingido = row.FindControl("LiteralAtingido") as Literal;
                Literal litDiferenca = row.FindControl("LiteralDiferenca") as Literal;
                Literal litVendaAnoAnterior = row.FindControl("LiteralVendasAnoAnterior") as Literal;
                Literal litDiferencaAtingido = row.FindControl("LiteralDiferencaAtingido") as Literal;

                if (litVendaAtual.Text != "" && Convert.ToDecimal(litVendaAtual.Text) <= 0)
                    continue;

                if (row.Cells[0].Text.Trim() == "Sub-Total")
                {
                    //CONTROLE DE TOTAL
                    xlWorkSheet.Cells[linha, 0].Value = "Sub-Total";
                    xlWorkSheet.Cells[linha, 1].Value = "";
                    xlWorkSheet.Cells[linha, 2].Formula = "=SUBTOTAL(9, C" + linhaSubTotal.ToString() + ":C" + (linha).ToString() + ")";
                    xlWorkSheet.Cells[linha, 3].Formula = "=SUBTOTAL(9, D" + linhaSubTotal.ToString() + ":D" + (linha).ToString() + ")";
                    xlWorkSheet.Cells[linha, 4].Formula = "=(D" + (linha + 1) + "/C" + (linha + 1) + ")";
                    xlWorkSheet.Cells[linha, 4].Style.NumberFormat = "###,##%";
                    xlWorkSheet.Cells[linha, 5].Formula = "=SUBTOTAL(9, F" + linhaSubTotal.ToString() + ":F" + (linha).ToString() + ")";
                    xlWorkSheet.Cells[linha, 6].Formula = "=SUBTOTAL(9, G" + linhaSubTotal.ToString() + ":G" + (linha).ToString() + ")";
                    xlWorkSheet.Cells[linha, 7].Formula = "=(D" + (linha + 1) + "/G" + (linha + 1) + ")-1";
                    xlWorkSheet.Cells[linha, 7].Style.NumberFormat = "###,##%";

                    var subTotalRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 7);
                    subTotalRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                    subTotalRange.Style.FillPattern.SetSolid(System.Drawing.Color.SandyBrown);
                    subTotalRange.Style.Font.Weight = ExcelFont.BoldWeight;


                    linha += 1;
                    linhaSubTotal = linha + 1;

                    continue;
                }

                xlWorkSheet.Cells[linha, 0].Value = row.Cells[0].Text;
                xlWorkSheet.Cells[linha, 0].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 1].Value = row.Cells[1].Text;
                xlWorkSheet.Cells[linha, 1].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);


                xlWorkSheet.Cells[linha, 2].Value = Convert.ToDecimal(litCotas.Text);
                xlWorkSheet.Cells[linha, 2].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 3].Value = Convert.ToDecimal(litVendaAtual.Text);
                xlWorkSheet.Cells[linha, 3].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 4].Value = litAtingido.Text.Trim();
                xlWorkSheet.Cells[linha, 4].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 5].Value = Convert.ToDecimal(litDiferenca.Text);
                xlWorkSheet.Cells[linha, 5].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 6].Value = Convert.ToDecimal(litVendaAnoAnterior.Text);
                xlWorkSheet.Cells[linha, 6].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);

                xlWorkSheet.Cells[linha, 7].Value = litDiferencaAtingido.Text.Trim();
                xlWorkSheet.Cells[linha, 7].Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
                if (litDiferencaAtingido.Text.Trim().Contains("-"))
                    xlWorkSheet.Cells[linha, 7].Style.FillPattern.SetSolid(System.Drawing.Color.Red);

                valorTotalCota += Convert.ToDecimal(litCotas.Text);
                valorTotalVendaAtual += Convert.ToDecimal(litVendaAtual.Text);
                valorTotalDiff += Convert.ToDecimal(litDiferenca.Text);
                valorTotalVendaAnterior += Convert.ToDecimal(litVendaAnoAnterior.Text);


                linha += 1;
            }

            //FILTROS E CONGELAMENTO
            xlWorkSheet.Panes = new WorksheetPanes(PanesState.Frozen, 0, 4, "A5", PanePosition.BottomLeft);
            var filterRange = xlWorkSheet.Cells.GetSubrangeAbsolute(3, 0, linha, 7);
            filterRange.Filter(true).Apply();

            //CONTROLE DE TOTAL
            xlWorkSheet.Cells[linha, 0].Value = "Total Rede";
            xlWorkSheet.Cells[linha, 1].Value = "";
            xlWorkSheet.Cells[linha, 2].Value = valorTotalCota;
            xlWorkSheet.Cells[linha, 3].Value = valorTotalVendaAtual;
            xlWorkSheet.Cells[linha, 4].Value = (valorTotalVendaAtual / valorTotalCota);
            xlWorkSheet.Cells[linha, 4].Style.NumberFormat = "###,##%";
            xlWorkSheet.Cells[linha, 5].Value = valorTotalDiff;
            xlWorkSheet.Cells[linha, 6].Value = valorTotalVendaAnterior;
            xlWorkSheet.Cells[linha, 7].Formula = "=(D" + (linha + 1) + "/G" + (linha + 1) + ")-1";
            xlWorkSheet.Cells[linha, 7].Style.NumberFormat = "###,##%";

            var footerRange = xlWorkSheet.Cells.GetSubrangeAbsolute(linha, 0, linha, 7);
            footerRange.Style.FillPattern.SetSolid(System.Drawing.Color.SandyBrown);
            footerRange.Style.Font.Weight = ExcelFont.BoldWeight;
            footerRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;
            footerRange.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            footerRange.Style.Borders.SetBorders(MultipleBorders.All, System.Drawing.Color.Black, LineStyle.Thin);
            //Primeira Coluna alinhar esquerda
            xlWorkSheet.Cells[linha, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 2, linha + 1, 2).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 2, linha + 1, 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 3, linha + 1, 3).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 3, linha + 1, 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 4, linha + 1, 4).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 5, linha + 1, 5).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 5, linha + 1, 5).Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 6, linha + 1, 6).Style.NumberFormat = "R$ ###,###,##0.00";
            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 6, linha + 1, 6).Style.HorizontalAlignment = HorizontalAlignmentStyle.CenterAcross;

            xlWorkSheet.Cells.GetSubrangeAbsolute(4, 7, linha + 1, 7).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            string nomeArquivo = dataIni.Day.ToString() + "-A-" + dataFim.Day.ToString() + "-" + mesExtenso + "-COTAxVENDAxANO-ANTERIOR.xlsx";
            string diretorioArquivo = Server.MapPath("..") + "\\Excel\\" + nomeArquivo;

            ef.Save(diretorioArquivo);

            return diretorioArquivo;
        }


        #endregion
    }
}
