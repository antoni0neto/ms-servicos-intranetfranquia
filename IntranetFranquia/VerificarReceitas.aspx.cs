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
    public partial class VerificarReceitas : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        public double totalDinheiro = 0;
        public double totalCheque = 0;
        public double totalChequePre = 0;
        public double totalCartaoCredito = 0;
        public double totalComanda = 0;
        public double totalRetirada = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();

                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();

                if (codigoPerfil == "8")
                {
                    labTitulo.Text = "Caixa de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Receitas Caixa";
                    hrefVoltar.HRef = "DefaultCaixaFilial.aspx";
                }
                else
                {
                    if (codigoPerfil != "2" && codigoPerfil != "3")
                    {
                        labTitulo.Text = "Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Receitas Caixa";
                        hrefVoltar.HRef = "DefaultFinanceiro.aspx";
                    }
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

                //ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                //ddlFilial.DataBind();

                List<FILIAI> listFiliais = baseController.BuscaFiliais_Intermediario(usuario);

                ddlFilial.DataSource = listFiliais;
                ddlFilial.DataBind();

                if (listFiliais != null && listFiliais.Count() == 1)
                    ddlFilial.SelectedValue = listFiliais.First().COD_FILIAL;
            
            
            }
        }

        private void CarregaGridViewReceitas()
        {
            GridViewReceitas.DataSource = baseController.BuscaReceitasCaixa(CalendarDataInicio.SelectedDate.Date,
                                                                            CalendarDataFim.SelectedDate.Date,
                                                                            Convert.ToInt32(ddlFilial.SelectedValue));
            GridViewReceitas.DataBind();
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

        protected void btBuscarReceitas_Click(object sender, EventArgs e)
        {
            labErroFilial.Text = "";

            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
            {
                labErroFilial.Text = "Informe um período válido.";
                return;
            }

            if (ddlFilial.SelectedValue == "" || ddlFilial.SelectedValue == "0")
            {
                labErroFilial.Text = "Selecione a Filial.";
                return;
            }

            CarregaGridViewReceitas();
        }

        protected void GridViewReceitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            FECHAMENTO_CAIXA caixa = e.Row.DataItem as FECHAMENTO_CAIXA;

            if (caixa != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                    literalFilial.Text = baseController.BuscaFilialCodigoInt(caixa.CODIGO_FILIAL).FILIAL;
            }
        }

        protected void GridViewReceitas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewReceitas.FooterRow;

            foreach (GridViewRow item in GridViewReceitas.Rows)
            {
                totalDinheiro += Convert.ToDouble(item.Cells[2].Text);
                totalCheque += Convert.ToDouble(item.Cells[3].Text);
                totalChequePre += Convert.ToDouble(item.Cells[4].Text);
                totalCartaoCredito += Convert.ToDouble(item.Cells[5].Text);
                totalComanda += Convert.ToDouble(item.Cells[6].Text);
                totalRetirada += Convert.ToDouble(item.Cells[7].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = totalDinheiro.ToString("N2");
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalCheque.ToString("N2");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[4].Text = totalChequePre.ToString("N2");
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[5].Text = totalCartaoCredito.ToString("N2");
                footer.Cells[5].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[6].Text = totalComanda.ToString("N2");
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[7].Text = totalRetirada.ToString("N2");
                footer.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
    }
}
