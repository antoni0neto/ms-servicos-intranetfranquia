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
    public partial class DivergenciaDeposito : System.Web.UI.Page
    {
        ImagemController imagemController = new ImagemController();
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();
        LojaController lojaController = new LojaController();

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
        //Chamada principal de dropdown
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

                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
        }

        private void CarregaDropDownListBaixa()
        {
            ddlBaixa.DataSource = baseController.BuscaDepositoBaixas();
            ddlBaixa.DataBind();
        }

        private void CarregaGridViewDepositos()
        {
            List<DEPOSITO_SEMANA> listaDepositosSemana = baseController.BuscaDepositosRealizados(Convert.ToInt32(ddlBaixa.SelectedValue), ddlFilial.SelectedValue, CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);

            if (listaDepositosSemana != null)
            {
                GridViewDepositos.DataSource = listaDepositosSemana;
                GridViewDepositos.DataBind();
            }
        }

        protected void ddlBaixa_DataBound(object sender, EventArgs e)
        {
            ddlBaixa.Items.Add(new ListItem("Selecione", "0"));
            ddlBaixa.SelectedValue = "0";
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
            DEPOSITO_SEMANA depositoSemana = e.Row.DataItem as DEPOSITO_SEMANA;

            if (depositoSemana != null)
            {
                Literal literalBaixa = e.Row.FindControl("LiteralBaixa") as Literal;

                if (literalBaixa != null)
                    literalBaixa.Text = lojaController.BuscaDepositoBaixa(depositoSemana.CODIGO_BAIXA).DESCRICAO;

                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigoInt(depositoSemana.CODIGO_FILIAL);

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
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
