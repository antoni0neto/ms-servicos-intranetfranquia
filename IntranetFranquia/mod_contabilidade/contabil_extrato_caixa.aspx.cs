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
    public partial class contabil_extrato_caixa : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CarregaDropDownListFilial();
                btExcelExtrato.Enabled = false;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais_Intermediario(usuario);
                ddlFilial.DataBind();
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
        #endregion

        private void CarregarExtrato()
        {
            List<SP_BUSCAR_EXTRATO_LOJAResult> _extrato = new List<SP_BUSCAR_EXTRATO_LOJAResult>();
            SP_BUSCAR_EXTRATO_LOJAResult _saldoInicial = new SP_BUSCAR_EXTRATO_LOJAResult();
            bool _entrou = false;

            _extrato = baseController.BuscarExtratoLoja(ddlFilial.SelectedValue, Convert.ToDateTime(TextBoxDataInicio.Text), Convert.ToDateTime(TextBoxDataFim.Text));

            //Obter Linha do Saldo 
            var linha = _extrato.Where(p => p.DATA == Convert.ToDateTime(TextBoxDataInicio.Text).AddDays(-1));
            if (linha != null && linha.Count() > 0)
            {
                _saldoInicial = _extrato.Where(p => p.COL == linha.Max(x => x.COL)).SingleOrDefault();
                _entrou = true;
            }

            //Filtrar pela data
            _extrato = _extrato.Where(p => p.DATA >= Convert.ToDateTime(TextBoxDataInicio.Text) && p.DATA <= Convert.ToDateTime(TextBoxDataFim.Text)).ToList();

            //Adicionar saldo a data
            if (_saldoInicial != null && _entrou)
            {
                _saldoInicial.COL = 0;
                _saldoInicial.DESCRICAO = "SALDO FINAL";
                _saldoInicial.SAIDAS = Convert.ToDecimal(0.00);
                _saldoInicial.ENTRADAS = Convert.ToDecimal(0.00);
                _extrato.Insert(0, _saldoInicial);
            }

            gvExtrato.DataSource = _extrato.OrderBy(x => x.COL);
            gvExtrato.DataBind();

            btExcelExtrato.Enabled = false;
            Session["EXTRATO"] = null;
            if (_extrato != null && _extrato.Count > 0)
            {
                Session["EXTRATO"] = _extrato;
                btExcelExtrato.Enabled = true;
            }
        }

        protected void btExtratoLoja_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") ||
                TextBoxDataInicio.Text == null ||
                TextBoxDataFim.Text.Equals("") ||
                TextBoxDataFim.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregarExtrato();
        }
        protected void gvExtrato_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gvExtrato_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvExtrato.PageIndex = e.NewPageIndex;
            CarregarExtrato();
        }

        protected void btExcelExtrato_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["EXTRATO"] != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "EXTRATO_" + ddlFilial.SelectedItem.Text.Trim() + "_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvExtrato.AllowPaging = false;
                    gvExtrato.PageSize = 1000;
                    gvExtrato.DataSource = (Session["EXTRATO"] as List<SP_BUSCAR_EXTRATO_LOJAResult>);
                    gvExtrato.DataBind();


                    gvExtrato.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvExtrato.HeaderRow.Cells.Count; i++)
                    {
                        gvExtrato.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                    }
                    gvExtrato.RenderControl(htw);
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
