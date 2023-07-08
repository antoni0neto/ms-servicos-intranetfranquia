using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

namespace Relatorios.mod_rh
{
    public partial class rh_ponto_rel_nao_conformidades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarFilial();
                CarregarMesesRereferencia();
                //btPDF.Enabled = false;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            RHController rhController = new RHController();

            try
            {
                if (!Validar())
                {
                    labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }
                else
                {
                    string filial = ddlFilial.SelectedValue;
                    if (ddlFilial.SelectedValue == "9999")
                    {
                        filial = "";
                    }

                    List<SP_OBTER_NAO_CONFORMIDADES_RHResult> naoConf = rhController.ObterNaoConformidadesRH(filial).Where(p => p.COMPETENCIA == ddlMesReferencia.SelectedValue).ToList();
                    gvNConformidades.DataSource = naoConf;
                    gvNConformidades.DataBind();

                    btBuscar.Enabled = true;
                    btPDF.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();

                var filialDePara = new BaseController().BuscaFilialDePara();
                if (lstFilial.Count > 0)
                {
                    lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    lstFilial.Insert(1, new FILIAI { COD_FILIAL = "9999", FILIAL = "Todas" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                    }
                }
            }
        }

        private void CarregarMesesRereferencia()
        {
            ddlMesReferencia.Items.Add(new System.Web.UI.WebControls.ListItem("Selecione", "0"));

            for (int ano = DateTime.Now.Year; ano >= 2015; ano--)
            {
                for (int mes = 12; mes >= 1; mes--)
                {
                    ddlMesReferencia.Items.Add(new System.Web.UI.WebControls.ListItem(mes + "/" + ano, mes + "/" + ano));
                }
            }
        }

        private bool Validar()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;

            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _notOK;
                return false;
            }

            if (ddlMesReferencia.SelectedValue == "0")
            {
                labMesReferencia.ForeColor = _notOK;
                return false;
            }

            return true;
        }

        protected void btPDF_Click(object sender, EventArgs e)
        {
            int noOfColumns = gvNConformidades.Columns.Count;
            int noOfRows = gvNConformidades.Rows.Count;

            float HeaderTextSize = 7;
            float ReportTextSize = 7;
            float ApplicationNameSize = 6;

            Document document = new Document(PageSize.A4.Rotate(), 0, 0, 10, 10);

            iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);
            mainTable.HeaderRows = 2;
            iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(2);
            Phrase phApplicationName = new Phrase("Não Conformidades", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));
            PdfPCell clApplicationName = new PdfPCell(phApplicationName);
            clApplicationName.Border = PdfPCell.NO_BORDER;
            clApplicationName.HorizontalAlignment = Element.ALIGN_LEFT;
            Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));
            PdfPCell clDate = new PdfPCell(phDate);
            clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
            clDate.Border = PdfPCell.NO_BORDER;

            mainTable.SetWidths(new float[] { 18f, 10f, 17f, 8f, 14f, 7f, 7f, 7f, 7f, 5f });

            headerTable.AddCell(clApplicationName);
            headerTable.AddCell(clDate);
            headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

            PdfPCell cellHeader = new PdfPCell(headerTable);
            cellHeader.Border = PdfPCell.NO_BORDER;
            cellHeader.Colspan = noOfColumns;
            mainTable.AddCell(cellHeader);

            for (int i = 0; i < noOfColumns; i++)
            {
                Phrase ph = null;
                ph = new Phrase(gvNConformidades.Columns[i].HeaderText, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                mainTable.AddCell(ph);
            }

            for (int rowNo = 0; rowNo < noOfRows; rowNo++)
            {
                for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                {
                    string s = gvNConformidades.Rows[rowNo].Cells[columnNo].Text.Trim();
                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                    mainTable.AddCell(ph);
                }
                mainTable.CompleteRow();
            }

            PdfWriter.GetInstance(document, Response.OutputStream);
            HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
            pdfFooter.Alignment = Element.ALIGN_CENTER;
            pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

            document.Footer = pdfFooter;
            document.Open();
            document.Add(mainTable);
            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=\"Não Conformidades - " + ddlFilial.SelectedItem.Text.Trim() + ".pdf\"");
            Response.End();
            
            //Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
            
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            
        }

        private void RefreshData()
        {
            RHController rhController = new RHController();

            try
            {
                string filial = ddlFilial.SelectedValue;
                if (ddlFilial.SelectedValue == "9999")
                {
                    filial = "";
                }

                List<SP_OBTER_NAO_CONFORMIDADES_RHResult> naoConf = rhController.ObterNaoConformidadesRH(filial).Where(p => p.COMPETENCIA == ddlMesReferencia.SelectedValue).ToList();
                gvNConformidades.DataSource = naoConf;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
    }
}