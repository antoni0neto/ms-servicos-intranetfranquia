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
    public partial class VerificarFechamentoCaixa : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();

                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();
                if (codigoPerfil == "8")
                {
                    labTitulo.Text = "Caixa de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Fechamento Caixa";
                    hrefVoltar.HRef = "DefaultCaixaFilial.aspx";
                }
                else if (codigoPerfil == "27")
                {
                    labTitulo.Text = "Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Fechamento Caixa";
                    hrefVoltar.HRef = "mod_financeiro/fin_prod_menu.aspx";
                }
                else
                {
                    if (codigoPerfil != "2" && codigoPerfil != "3")
                    {
                        labTitulo.Text = "Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Fechamento Caixa";
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

                //ddlFilial.DataSource = baseController.BuscaFiliais_Intermediario(usuario);
                //ddlFilial.DataBind();

                List<FILIAI> listFiliais = baseController.BuscaFiliais_Intermediario(usuario);

                ddlFilial.DataSource = listFiliais;
                ddlFilial.DataBind();

                if (listFiliais != null && listFiliais.Count() == 1)
                    ddlFilial.SelectedValue = listFiliais.First().COD_FILIAL;
            }
        }

        private void CarregaGridViewFechamento()
        {
            GridViewFechamento.DataSource = baseController.BuscaFechamentosCaixa(CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate, Convert.ToInt32(ddlFilial.SelectedValue));
            GridViewFechamento.DataBind();
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

        protected void ButtonPesquisarFechamento_Click(object sender, EventArgs e)
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

            CarregaGridViewFechamento();
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            Button buttonPesquisar = sender as Button;

            if (buttonPesquisar != null)
                Response.Redirect(string.Format("AlterarFechamentoCaixa.aspx?CodigoCaixa={0}", buttonPesquisar.CommandArgument));
        }
        /*
                protected void ButtonDeletar_Click(object sender, EventArgs e)
                {
                    Button buttonDeletar = sender as Button;

                    if (buttonDeletar != null)
                    {
                        FECHAMENTO_CAIXA caixa = baseController.BuscaCaixa(Convert.ToInt32(buttonDeletar.CommandArgument));

                        if (caixa != null)
                        {
                            usuarioController.ExcluiCaixa(caixa.CODIGO_CAIXA);
                            usuarioController.ExcluiHistoricoDeposito(caixa);

                            CarregaGridViewFechamento();
                        }
                    }
                }
                */
        protected void GridViewFechamento_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewFechamento.Rows)
            {
                if ((Convert.ToDecimal(item.Cells[2].Text) - Convert.ToDecimal(item.Cells[4].Text)) < Convert.ToDecimal(item.Cells[3].Text))
                {
                    item.Cells[0].BackColor = System.Drawing.Color.Red;
                    item.Cells[1].BackColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void GridViewFechamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            FECHAMENTO_CAIXA caixa = e.Row.DataItem as FECHAMENTO_CAIXA;

            if (caixa != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                    literalFilial.Text = baseController.BuscaFilialCodigoInt(caixa.CODIGO_FILIAL).FILIAL;

                Literal literalSaldoDinheiro = e.Row.FindControl("LiteralSaldoDinheiro") as Literal;

                if (literalSaldoDinheiro != null)
                    literalSaldoDinheiro.Text = (caixa.VALOR_DINHEIRO - caixa.VALOR_RETIRADA).ToString();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button buttonPesquisar = e.Row.FindControl("ButtonPequisar") as Button;

                if (buttonPesquisar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        FECHAMENTO_CAIXA itemCaixa = e.Row.DataItem as FECHAMENTO_CAIXA;

                        buttonPesquisar.CommandArgument = itemCaixa.CODIGO_CAIXA.ToString();
                    }
                }

                Button buttonDeletar = e.Row.FindControl("ButtonDeletar") as Button;

                if (buttonDeletar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        FECHAMENTO_CAIXA itemCaixa = e.Row.DataItem as FECHAMENTO_CAIXA;

                        buttonDeletar.CommandArgument = itemCaixa.CODIGO_CAIXA.ToString();
                    }
                }
            }
        }
    }
}
