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
    public partial class contabil_det_despesa_ffc : System.Web.UI.Page
    {

        BaseController baseController = new BaseController();
        private decimal g_total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarFiltrosDropDown();
            }
        }

        #region "Metodos Fixos"
        //controle de datas
        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        //inicializar dropdownlists
        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }
        protected void ddlDespesa_DataBound(object sender, EventArgs e)
        {
            ddlDespesa.Items.Add(new ListItem("Selecione", ""));
            ddlDespesa.SelectedValue = "";
        }

        #endregion

        #region "Carregar dropdown lists"
        //Chamada principal de dropdown
        private void CarregarFiltrosDropDown()
        {
            CarregaDropDownListDespesa();
            CarregaDropDownListFilial();
        }

        //Carregar Filial
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

        //Carregar Despesa
        private void CarregaDropDownListDespesa()
        {
            ddlDespesa.DataSource = baseController.BuscaContas();
            ddlDespesa.DataBind();
        }

        #endregion

        private void CarregaGridViewDespesas()
        {
            List<SP_BUSCAR_DESPESAS_DETALHEResult> listaDespesaDetalhe = new List<SP_BUSCAR_DESPESAS_DETALHEResult>();

            try
            {
                listaDespesaDetalhe = baseController.BuscarDespesasDetalhe(ddlFilial.SelectedValue, TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlDespesa.SelectedValue);

                foreach (SP_BUSCAR_DESPESAS_DETALHEResult lista in listaDespesaDetalhe)
                {
                    g_total = g_total + lista.VALOR;
                }

                if (listaDespesaDetalhe != null && listaDespesaDetalhe.Count > 0)
                {
                    labErro.Visible = false;
                    GridViewDespesas.DataSource = listaDespesaDetalhe;
                    GridViewDespesas.DataBind();
                }
                else
                {
                    GridViewDespesas.DataSource = null;
                    GridViewDespesas.DataBind();
                    labErro.Text = "DADOS NÃO ENCONTRADOS. REALIZE UMA NOVA CONSULTA.";
                    labErro.Visible = true;
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
                labErro.Visible = true;
            }
        }

        protected void btBuscarDespesas_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;

            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
            {
                labErro.Text = "SELECIONE UMA FILIAL.";
                labErro.Visible = true;
                return;
            }

            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataFim.Text.Equals(""))
            {
                labErro.Text = "INFORME UM PERÍODO PARA CONSULTA.";
                labErro.Visible = true;
                return;
            }

            if ((!DateTime.TryParse(TextBoxDataInicio.Text, out v_dataini)) || (!DateTime.TryParse(TextBoxDataFim.Text, out v_datafim)))
            {
                labErro.Text = "INFORME UMA DATA VÁLIDA.";
                labErro.Visible = true;
                return;
            }

            labErro.Visible = false;
            CarregaGridViewDespesas();
        }

        protected void GridViewDespesas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDespesas.FooterRow;

            if (footer != null)
            {
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = g_total.ToString("##.00");
            }
        }

    }
}
