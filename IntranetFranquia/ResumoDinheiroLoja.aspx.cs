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
    public partial class ResumoDinheiroLoja : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        decimal saldoBancoADepositar = 0;
        decimal saldoBancoDepositado = 0;
        decimal saldoCofre = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListFilial();
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


        private void CarregaGridView()
        {
            List<DEPOSITO_SEMANA> depositoSemana = new List<DEPOSITO_SEMANA>();

            depositoSemana = baseController.BuscaDepositoNoPeriodo(Convert.ToInt32(ddlFilial.SelectedValue), CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);

            GridViewBanco.DataSource = depositoSemana;
            GridViewBanco.DataBind();

            btExcel.Enabled = false;
            Session["MOVIMENTO_DINHEIRO"] = null;
            if (depositoSemana != null && depositoSemana.Count > 0)
            {
                Session["MOVIMENTO_DINHEIRO"] = depositoSemana;
                btExcel.Enabled = true;
            }

            GridViewCofre.DataSource = baseController.BuscaCofreNoPeriodo(Convert.ToInt32(ddlFilial.SelectedValue), CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);
            GridViewCofre.DataBind();
        }
        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") ||
                TextBoxDataInicio.Text == null ||
                TextBoxDataFim.Text.Equals("") ||
                TextBoxDataFim.Text == null)
                return;

            CarregaGridView();
        }
        protected void GridViewBanco_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewBanco.FooterRow;

            foreach (GridViewRow item in GridViewBanco.Rows)
            {
                saldoBancoADepositar += Convert.ToDecimal(item.Cells[3].Text);
                saldoBancoDepositado += Convert.ToDecimal(item.Cells[4].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Saldo";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = saldoBancoADepositar.ToString("N2");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[4].Text = saldoBancoDepositado.ToString("N2");
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
        protected void GridViewCofre_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewCofre.FooterRow;

            foreach (GridViewRow item in GridViewCofre.Rows)
            {
                saldoCofre += Convert.ToDecimal(item.Cells[1].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Saldo";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = saldoCofre.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["MOVIMENTO_DINHEIRO"] != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "MOVIMENTO_DINHEIRO_" + ddlFilial.SelectedItem.Text.Trim() + "_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    GridViewBanco.AllowPaging = false;
                    GridViewBanco.PageSize = 1000;
                    GridViewBanco.DataSource = (Session["MOVIMENTO_DINHEIRO"] as List<DEPOSITO_SEMANA>);
                    GridViewBanco.DataBind();


                    GridViewBanco.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < GridViewBanco.HeaderRow.Cells.Count; i++)
                    {
                        GridViewBanco.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                    }
                    GridViewBanco.RenderControl(htw);
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
