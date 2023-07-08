using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace Relatorios
{
    public partial class NFeCompararNotas : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();

                btExcelPortal.Enabled = false;
                btExcelRetaguarda.Enabled = false;
            }
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
            {

                Response.Redirect("~/Login.aspx");
            }

            ddlFilial.DataSource = baseController.BuscaTodasFiliais();
            ddlFilial.DataBind();
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btBuscarNotas_Click(object sender, EventArgs e)
        {
            labMSG.Text = "";
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null)
            {
                labMSG.Text = "Por favor, informe a Data Inicial.";
                return;
            }
            if (TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
            {
                labMSG.Text = "Por favor, informe a Data Final.";
                return;
            }

            if (ddlFilial.SelectedValue == "0" || ddlFilial.SelectedValue == "" || ddlFilial.SelectedValue == null)
            {
                labMSG.Text = "Por favor, informe a Filial.";
                return;
            }

            TimeSpan _time = new TimeSpan();
            _time = Convert.ToDateTime(TextBoxDataFim.Text).Subtract(Convert.ToDateTime(TextBoxDataInicio.Text));

            if (_time.Days >= 31)
            {
                labMSG.Text = "Por favor, informe um período inferior ou igual a 31 dias.";
                return;
            }

            CarregaGridViewRetaguarda();
            CarregaGridViewPortal();

        }

        private void CarregaGridViewRetaguarda()
        {
            List<SP_BUSCAR_NF_RETAGUARDAResult> _notasRetaguarda = baseController.BuscaNotasRetaguarda(ddlFilial.SelectedValue.ToString().Trim(),
                                                               CalendarDataInicio.SelectedDate.ToString(),
                                                               CalendarDataFim.SelectedDate.ToString());
            btExcelRetaguarda.Enabled = false;
            if (_notasRetaguarda != null)
            {
                //Alterar o index em todos os lugares que utilizam este comando
                GridViewRetaguarda.Columns[10].Visible = true;

                GridViewRetaguarda.DataSource = _notasRetaguarda;
                GridViewRetaguarda.DataBind();
                if (_notasRetaguarda.Count > 0)
                    btExcelRetaguarda.Enabled = true;
            }
        }

        private void CarregaGridViewPortal()
        {
            List<SP_BUSCAR_NF_PORTALResult> _notasPortal = baseController.BuscaNotasPortal(ddlFilial.SelectedValue.ToString().Trim(),
                                                               CalendarDataInicio.SelectedDate.ToString(),
                                                               CalendarDataFim.SelectedDate.ToString());
            btExcelPortal.Enabled = false;
            if (_notasPortal != null)
            {
                //Alterar o index em todos os lugares que utilizam este comando
                GridViewPortal.Columns[10].Visible = true;

                GridViewPortal.DataSource = _notasPortal;
                GridViewPortal.DataBind();

                if (_notasPortal.Count > 0)
                    btExcelPortal.Enabled = true;
            }
        }

        protected void GridViewRetaguarda_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridViewPortal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void GridViewRetaguarda_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow r in GridViewRetaguarda.Rows)
            {
                //Alterar o index em todos os lugares que utilizam este comando
                if (r.Cells[10].Text == "X")
                {
                    r.BackColor = System.Drawing.Color.Moccasin;
                    r.ForeColor = System.Drawing.Color.Black;
                }

                //se nota de entrada, verifica se tem uma nota de saida
                if (r.Cells[7].Text == "E" && r.Cells[10].Text == "X")
                {
                    SP_OBTER_NF_SAIDAResult nf_saida = baseController.ObterNFSaida(r.Cells[3].Text, r.Cells[5].Text, r.Cells[4].Text);
                    if (nf_saida != null)
                    {
                        if (nf_saida.FILIAL.Trim() != ddlFilial.SelectedItem.Text.Trim())
                        {
                            r.BackColor = System.Drawing.Color.LightGray;
                            r.ForeColor = System.Drawing.Color.Black;
                            r.ToolTip = nf_saida.FILIAL;
                        }
                    }
                    nf_saida = null;
                }

            }

            GridViewRetaguarda.Columns[10].Visible = false;
        }

        protected void GridViewPortal_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow r in GridViewPortal.Rows)
            {
                //Alterar o index em todos os lugares que utilizam este comando
                if (r.Cells[10].Text == "X")
                {
                    r.BackColor = System.Drawing.Color.Moccasin;
                    r.ForeColor = System.Drawing.Color.Black;
                }
            }

            GridViewPortal.Columns[10].Visible = false;
        }

        protected void GridViewRetaguarda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRetaguarda.PageIndex = e.NewPageIndex;
            CarregaGridViewRetaguarda();
        }

        protected void GridViewPortal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPortal.PageIndex = e.NewPageIndex;
            CarregaGridViewPortal();
        }

        protected void btExcelRetaguarda_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "NF_Retaguarda_" + ddlFilial.SelectedItem.Text.Trim() + "_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GridViewRetaguarda.AllowPaging = false;

                GridViewRetaguarda.HeaderRow.Style.Add("background-color", "#FFFFFF");

                for (int i = 0; i < GridViewRetaguarda.HeaderRow.Cells.Count; i++)
                {
                    GridViewRetaguarda.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                }
                GridViewRetaguarda.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();

            }
            catch (Exception ex)
            {
            }
        }

        protected void btExcelPortal_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "NF_Portal_" + ddlFilial.SelectedItem.Text.Trim() + "_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GridViewPortal.AllowPaging = false;

                GridViewPortal.HeaderRow.Style.Add("background-color", "#FFFFFF");

                for (int i = 0; i < GridViewPortal.HeaderRow.Cells.Count; i++)
                {
                    GridViewPortal.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                }
                GridViewPortal.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }
}
