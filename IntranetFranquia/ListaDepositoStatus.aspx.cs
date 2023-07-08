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
    public partial class ListaDepositoStatus : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregarFiltrosDropDown();
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }
        private void CarregarFiltrosDropDown()
        {
            CarregaDropDownListBaixa();
            CarregaDropDownListFilial();
        }
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
        private void CarregaDropDownListBaixa()
        {
            ddlBaixa.DataSource = baseController.BuscaDepositoBaixas();
            ddlBaixa.DataBind();
        }
        protected void ddlBaixa_DataBound(object sender, EventArgs e)
        {
            ddlBaixa.Items.Add(new ListItem("Selecione", "0"));
            ddlBaixa.SelectedValue = "0";
        }

        private void CarregaGridViewDepositos()
        {
            List<DEPOSITO_DOCUMENTO> listaLancamentoLinx = baseController.BuscaLancamentosLinx(Convert.ToInt32(ddlBaixa.SelectedValue), ddlFilial.SelectedValue, CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);

            if (listaLancamentoLinx != null)
            {
                GridViewDepositos.DataSource = listaLancamentoLinx;
                GridViewDepositos.DataBind();
            }
        }
        protected void btBuscarDepositos_Click(object sender, EventArgs e)
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

            if (ddlBaixa.SelectedValue.ToString().Equals("0") || ddlBaixa.SelectedValue.ToString().Equals(""))
            {
                labErro.Text = "INFORME UM TIPO DE BAIXA.";
                labErro.Visible = true;
                return;
            }

            labErro.Visible = false;
            CarregaGridViewDepositos();
        }
        protected void GridViewDepositos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DEPOSITO_DOCUMENTO dpDocumento = e.Row.DataItem as DEPOSITO_DOCUMENTO;

            if (dpDocumento != null)
            {

                DEPOSITO_SEMANA dp = new DEPOSITO_SEMANA();
                dp = baseController.BuscaDepositoSemana(dpDocumento.CODIGO_DEPOSITO);

                if (dp != null)
                {
                    Literal _litDataInicio = e.Row.FindControl("litDataInicio") as Literal;
                    if (_litDataInicio != null)
                        _litDataInicio.Text = dp.DATA_INICIO.ToString("dd/MM/yyyy");

                    Literal _litDataFim = e.Row.FindControl("litDataFim") as Literal;
                    if (_litDataFim != null)
                        _litDataFim.Text = dp.DATA_FIM.ToString("dd/MM/yyyy");

                    Literal _litValorDepositar = e.Row.FindControl("litValorDepositar") as Literal;
                    if (_litValorDepositar != null)
                        _litValorDepositar.Text = dp.VALOR_A_DEPOSITAR.ToString("###,###,###,###,##0.00");

                    Literal _litValorDepositado = e.Row.FindControl("litValorDepositado") as Literal;
                    if (_litValorDepositado != null)
                        _litValorDepositado.Text = dp.VALOR_DEPOSITADO.ToString("###,###,###,###,##0.00");
                }

            }
        }
        protected void GridViewDepositos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDepositos.PageIndex = e.NewPageIndex;
            CarregaGridViewDepositos();
        }
    }
}
