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
    public partial class VerificarDespesas : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        public double totalDespesas = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                CarregaDropDownListDespesa();

                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();
                if (codigoPerfil != "2" && codigoPerfil != "3")
                {
                    labTitulo.Text = "Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Despesas Caixa";
                    hrefVoltar.HRef = "DefaultFinanceiro.aspx";
                }
            }
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
        }

        private void CarregaDropDownListDespesa()
        {
            ddlDespesa.DataSource = baseController.BuscaContasAntigas();
            ddlDespesa.DataBind();
        }        

        private void CarregaGridViewDespesas()
        {
            GridViewDespesas.DataSource = baseController.BuscaDespesasCaixa(baseController.AjustaData(TextBoxDataInicio.Text),
                                                                            baseController.AjustaData(TextBoxDataFim.Text), 
                                                                            ddlFilial.SelectedValue, 
                                                                            ddlDespesa.SelectedValue);
            GridViewDespesas.DataBind();
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void ddlDespesa_DataBound(object sender, EventArgs e)
        {
            ddlDespesa.Items.Add(new ListItem("Selecione", "0"));
            ddlDespesa.SelectedValue = "0";
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void ButtonPesquisarDespesas_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
                return;

            CarregaGridViewDespesas();
        }

        protected void GridViewDespesas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Depesas_CaixaResult caixa = e.Row.DataItem as Sp_Depesas_CaixaResult;

            if (caixa != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                    literalFilial.Text = baseController.BuscaFilialCodigoInt(caixa.Filial).FILIAL;
            }
        }

        protected void GridViewDespesas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDespesas.FooterRow;

            foreach (GridViewRow item in GridViewDespesas.Rows)
            {
                totalDespesas += Convert.ToDouble(item.Cells[3].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalDespesas.ToString("N2");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
    }
}
