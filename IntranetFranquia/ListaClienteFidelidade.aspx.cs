using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class ListaClienteFidelidade : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        int totalFiel = 0;
        int totalNaoFiel = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void CarregaGridViewFidelidade()
        {
            GridViewFidelidade.DataSource = baseController.BuscaClienteFidelidade(TextBoxDataInicio.Text, TextBoxDataFim.Text);
            GridViewFidelidade.DataBind();
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void ButtonPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || 
                TextBoxDataInicio.Text == null || 
                TextBoxDataFim.Text.Equals("") || 
                TextBoxDataFim.Text == null)
                return;

            CarregaGridViewFidelidade();
        }

        protected void GridViewFidelidade_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewFidelidade.FooterRow;

            foreach (GridViewRow item in GridViewFidelidade.Rows)
            {
                if (item.Cells[1].Text.Equals(""))
                    item.Cells[1].Text = "0";

                if (item.Cells[3].Text.Equals(""))
                    item.Cells[3].Text = "0";

                totalFiel += Convert.ToInt32(item.Cells[1].Text);
                totalNaoFiel += Convert.ToInt32(item.Cells[3].Text);

                if (!item.Cells[1].Text.Equals("0") || !item.Cells[3].Text.Equals("0"))
                {
                    item.Cells[2].Text = Convert.ToInt32((Convert.ToDecimal(item.Cells[1].Text) / (Convert.ToDecimal(item.Cells[3].Text) + Convert.ToDecimal(item.Cells[1].Text)) * 100)).ToString() + " %";
                    item.Cells[4].Text = Convert.ToInt32((Convert.ToDecimal(item.Cells[3].Text) / (Convert.ToDecimal(item.Cells[3].Text) + Convert.ToDecimal(item.Cells[1].Text)) * 100)).ToString() + " %";
                }
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[1].Text = totalFiel.ToString();
                footer.Cells[3].Text = totalNaoFiel.ToString();
            }
        }
    }
}
