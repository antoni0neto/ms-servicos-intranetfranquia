using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class ListaDepositoPendencias : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        decimal valorDepositoRealizado = 0;
        decimal valorDepositoPendente = 0;
        decimal valorComprovanteRealizado = 0;
        decimal valorComprovantePendente = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            if (txtDataInicio.Text.Equals("") ||
                txtDataInicio.Text == null ||
                txtDataFim.Text.Equals("") ||
                txtDataFim.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewComprovantesRealizados();
            CarregaGridViewComprovantesPendentes();
            CarregaGridViewDepositosRealizados();
            CarregaGridViewDepositosPendentes();
        }

        protected void GridViewComprovantesRealizados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DEPOSITO_COMPROVANTE depositoComprovante = e.Row.DataItem as DEPOSITO_COMPROVANTE;

            if (depositoComprovante != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigoInt(depositoComprovante.CODIGO_FILIAL);

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }
            }
        }

        protected void GridViewComprovantesRealizados_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewComprovantesRealizados.FooterRow;

            foreach (GridViewRow item in GridViewComprovantesRealizados.Rows)
            {
                valorComprovanteRealizado += Convert.ToDecimal(item.Cells[3].Text);
            }

            if (footer != null)
            {
                footer.Cells[3].Text = valorComprovanteRealizado.ToString("N2");
            }
        }

        protected void GridViewComprovantesPendentes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DEPOSITO_COMPROVANTE depositoComprovante = e.Row.DataItem as DEPOSITO_COMPROVANTE;

            if (depositoComprovante != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigoInt(depositoComprovante.CODIGO_FILIAL);

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }
            }
        }

        protected void GridViewComprovantesPendentes_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewComprovantesPendentes.FooterRow;

            foreach (GridViewRow item in GridViewComprovantesPendentes.Rows)
            {
                valorComprovantePendente += Convert.ToDecimal(item.Cells[3].Text);
            }

            if (footer != null)
            {
                footer.Cells[3].Text = valorComprovantePendente.ToString("N2");
            }
        }

        protected void GridViewDepositosRealizados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DEPOSITO_HISTORICO depositoHistorico = e.Row.DataItem as DEPOSITO_HISTORICO;

            if (depositoHistorico != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigoInt(depositoHistorico.CODIGO_FILIAL);

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }
            }
        }

        protected void GridViewDepositosRealizados_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDepositosRealizados.FooterRow;

            foreach (GridViewRow item in GridViewDepositosRealizados.Rows)
            {
                valorDepositoRealizado += Convert.ToDecimal(item.Cells[2].Text);
            }

            if (footer != null)
            {
                footer.Cells[2].Text = valorDepositoRealizado.ToString("N2");
            }
        }

        protected void GridViewDepositosPendentes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DEPOSITO_HISTORICO depositoHistorico = e.Row.DataItem as DEPOSITO_HISTORICO;

            if (depositoHistorico != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigoInt(depositoHistorico.CODIGO_FILIAL);

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }
            }
        }

        protected void GridViewDepositosPendentes_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDepositosPendentes.FooterRow;

            foreach (GridViewRow item in GridViewDepositosPendentes.Rows)
            {
                valorDepositoPendente += Convert.ToDecimal(item.Cells[2].Text);
            }

            if (footer != null)
            {
                footer.Cells[2].Text = valorDepositoPendente.ToString("N2");
            }
        }

        private void CarregaGridViewComprovantesRealizados()
        {
            GridViewComprovantesRealizados.DataSource = usuarioController.BuscaComprovantesRealizados(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToDateTime(txtDataInicio.Text), Convert.ToDateTime(txtDataFim.Text));
            GridViewComprovantesRealizados.DataBind();
        }

        private void CarregaGridViewComprovantesPendentes()
        {
            GridViewComprovantesPendentes.DataSource = usuarioController.BuscaComprovantesPendentes(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToDateTime(txtDataInicio.Text), Convert.ToDateTime(txtDataFim.Text));
            GridViewComprovantesPendentes.DataBind();
        }

        private void CarregaGridViewDepositosRealizados()
        {
            GridViewDepositosRealizados.DataSource = usuarioController.BuscaDepositosRealizados(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToDateTime(txtDataInicio.Text), Convert.ToDateTime(txtDataFim.Text));
            GridViewDepositosRealizados.DataBind();
        }

        private void CarregaGridViewDepositosPendentes()
        {
            GridViewDepositosPendentes.DataSource = usuarioController.BuscaDepositosPendentes(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToDateTime(txtDataInicio.Text), Convert.ToDateTime(txtDataFim.Text));
            GridViewDepositosPendentes.DataBind();
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            txtDataInicio.Text = string.Empty;
            txtDataFim.Text = string.Empty;
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }
    }
}