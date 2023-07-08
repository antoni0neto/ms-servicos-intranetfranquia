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
    public partial class ListaAluguel : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();
        
        decimal totalValor = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListFilial();
        }

        private void CarregaGridViewAlugueis()
        {
            List<ACOMPANHAMENTO_ALUGUEL> lista = baseController.BuscaAlugueis(Convert.ToInt32(ddlFilial.SelectedValue), CalendarData.SelectedDate);

            if (lista != null)
            {
                GridViewAlugueis.DataSource = lista;
                GridViewAlugueis.DataBind();
            }
        }

        private void CarregaDropDownListFilial()
        {
            ddlFilial.DataSource = baseController.BuscaFiliais();
            ddlFilial.DataBind();
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void CalendarData_SelectionChanged(object sender, EventArgs e)
        {
            txtData.Text = CalendarData.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button btAlterar = sender as Button;

            if (btAlterar != null)
                Response.Redirect(string.Format("CadastroAluguel.aspx?CodigoAluguel={0}", btAlterar.CommandArgument));
        }

        protected void btBuscaAlugueis_Click(object sender, EventArgs e)
        {
            if (txtData.Text.Equals("") ||
                txtData.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewAlugueis();
        }

        protected void GridViewAlugueis_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewAlugueis.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalValor.ToString("###,###.##");
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void GridViewAlugueis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ACOMPANHAMENTO_ALUGUEL aluguel = e.Row.DataItem as ACOMPANHAMENTO_ALUGUEL;

            if (aluguel != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigo(aluguel.CODIGO_FILIAL);

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }

                Literal literalReferencia = e.Row.FindControl("LiteralReferencia") as Literal;

                if (literalReferencia != null)
                {
                    ACOMPANHAMENTO_ALUGUEL_MESANO referencia = baseController.BuscaReferencia(aluguel.CODIGO_MESANO);

                    if (referencia != null)
                        literalReferencia.Text = referencia.DESCRICAO;
                }

                Literal literalValor = e.Row.FindControl("LiteralValor") as Literal;

                if (literalValor != null)
                {
                    literalValor.Text = aluguel.VALOR.ToString("###,###.##");

                    totalValor += aluguel.VALOR;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btAlterar = e.Row.FindControl("btAlterar") as Button;

                if (btAlterar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        ACOMPANHAMENTO_ALUGUEL aluguelItem = e.Row.DataItem as ACOMPANHAMENTO_ALUGUEL;

                        if (aluguelItem != null)
                            btAlterar.CommandArgument = aluguelItem.CODIGO_ACOMPANHAMENTO_ALUGUEL.ToString();
                    }
                }
            }
        }
    }
}
