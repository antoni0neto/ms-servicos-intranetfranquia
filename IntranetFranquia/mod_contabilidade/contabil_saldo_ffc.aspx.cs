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

namespace Relatorios
{
    public partial class contabil_saldo_ffc : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CarregaDropDownListFilial();
                btExcelSaldo.Enabled = false;
            }
        }

        private void CarregaDropDownListFilial()
        {
            var _filial = baseController.BuscaFiliais();
            var filialDePara = baseController.BuscaFilialDePara();
            _filial = _filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

            _filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
            ddlFilial.DataSource = _filial;
            ddlFilial.DataBind();
        }

        private void CarregaGridView()
        {
            List<SP_BUSCAR_SALDO_FUNDO_FIXOResult> _ff = new List<SP_BUSCAR_SALDO_FUNDO_FIXOResult>();

            _ff = baseController.BuscarSaldoFundoFixo(ddlFilial.SelectedValue, Convert.ToDateTime(TextBoxDataInicio.Text), Convert.ToDateTime(TextBoxDataFim.Text));

            GridViewSaldoFundoFixo.DataSource = _ff;
            GridViewSaldoFundoFixo.DataBind();

            btExcelSaldo.Enabled = false;
            Session["SALDO"] = null;
            if (_ff != null && _ff.Count > 0)
            {
                Session["SALDO"] = _ff;
                btExcelSaldo.Enabled = true;
            }
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

        protected void btSaldoFundoFixo_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") ||
                TextBoxDataInicio.Text == null ||
                TextBoxDataFim.Text.Equals("") ||
                TextBoxDataFim.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridView();
        }

        protected void GridViewSaldoFundoFixo_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void GridViewSaldoFundoFixo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewSaldoFundoFixo.PageIndex = e.NewPageIndex;
            CarregaGridView();
        }

        protected void btExcelSaldo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["SALDO"] != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SALDO_" + ddlFilial.SelectedItem.Text.Trim() + "_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    GridViewSaldoFundoFixo.AllowPaging = false;
                    GridViewSaldoFundoFixo.PageSize = 1000;
                    GridViewSaldoFundoFixo.DataSource = (Session["SALDO"] as List<SP_BUSCAR_SALDO_FUNDO_FIXOResult>);
                    GridViewSaldoFundoFixo.DataBind();


                    GridViewSaldoFundoFixo.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < GridViewSaldoFundoFixo.HeaderRow.Cells.Count; i++)
                    {
                        GridViewSaldoFundoFixo.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                    }
                    GridViewSaldoFundoFixo.RenderControl(htw);
                    Response.Write(sw.ToString());
                    Response.End();
                }
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
