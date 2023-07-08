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
    public partial class VerificarPendenciasFechamentoCaixa : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();
                if (codigoPerfil == "27")
                {
                    hrefVoltar.HRef = "mod_financeiro/fin_prod_menu.aspx";
                }
                else
                {
                    hrefVoltar.HRef = "DefaultFinanceiro.aspx";
                }
            }
        }

        private void CarregaGridViewFechamento()
        {
            List<DivergenciaFechamento> lista = baseController.BuscaDivergenciasFechamento(CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);

            lista = lista.OrderBy(z => z.Data).ToList();

            GridViewFechamento.DataSource = lista;
            GridViewFechamento.DataBind();
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            Button buttonPesquisar = sender as Button;

            if (buttonPesquisar != null)
                Response.Redirect(string.Format("AlterarFechamentoCaixa.aspx?CodigoCaixa={0}", buttonPesquisar.CommandArgument));
        }

        protected void btBuscaDivergencias_Click(object sender, EventArgs e)
        {
            CarregaGridViewFechamento();
        }

        protected void GridViewFechamento_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewFechamento.Rows)
            {
                //if ((Convert.ToDecimal(item.Cells[2].Text) - Convert.ToDecimal(item.Cells[4].Text)) < Convert.ToDecimal(item.Cells[3].Text))
                //{
                //    item.Cells[0].BackColor = System.Drawing.Color.Red;
                //    item.Cells[1].BackColor = System.Drawing.Color.Red;
                //}
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

        protected void GridViewFechamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DivergenciaFechamento divergenciaFechamento = e.Row.DataItem as DivergenciaFechamento;

            if (divergenciaFechamento != null)
            {
                Literal literalTotalLinx = e.Row.FindControl("LiteralTotalLinx") as Literal;

                if (literalTotalLinx != null)
                    literalTotalLinx.Text = (divergenciaFechamento.ValorCartaoLinx + divergenciaFechamento.ValorDinheiroLinx + divergenciaFechamento.ValorOutrosLinx).ToString();

                Literal literalTotalIntranet = e.Row.FindControl("LiteralTotalIntranet") as Literal;

                if (literalTotalIntranet != null)
                    literalTotalIntranet.Text = (divergenciaFechamento.ValorCartaoIntranet + divergenciaFechamento.ValorDinheiroIntranet + divergenciaFechamento.ValorOutrosIntranet).ToString();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button buttonPesquisar = e.Row.FindControl("btPequisar") as Button;

                if (buttonPesquisar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        DivergenciaFechamento divergencia = e.Row.DataItem as DivergenciaFechamento;

                        if (divergencia != null)
                        {
                            FILIAI filial = baseController.BuscaFilial(divergencia.Filial);

                            if (filial != null)
                            {
                                FECHAMENTO_CAIXA caixa = baseController.BuscaFechamentoCaixa(divergencia.Data, Convert.ToInt32(filial.COD_FILIAL));

                                if (caixa != null)
                                    buttonPesquisar.CommandArgument = caixa.CODIGO_CAIXA.ToString();
                                else
                                    buttonPesquisar.CommandArgument = "0";
                            }
                        }
                    }
                }
            }
        }
    }
}
