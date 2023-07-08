using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class manutm_cad_aluguel : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarFiliais();
                CarregarCompetencias();
            }
        }

        private void CarregarAlugueis(int codigoFilial, DateTime competencia)
        {
            var listaAluguelItem = baseController.BuscaAlugueisPorCompetencia(codigoFilial, competencia);
            gvAluguel.DataSource = listaAluguelItem;
            gvAluguel.DataBind();
        }

        private void CarregarCompetencias()
        {
            var refs = baseController.BuscaReferencias().OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).ToList();
            refs.Insert(0, new ACOMPANHAMENTO_ALUGUEL_MESANO { CODIGO_ACOMPANHAMENTO_MESANO = 0, DESCRICAO = "Selecione" });

            ddlCompetencia.DataSource = refs;
            ddlCompetencia.DataBind();
        }
        private void CarregarFiliais()
        {
            ddlFilial.DataSource = ObterFiliais();
            ddlFilial.DataBind();
        }
        private List<FILIAI> ObterFiliais()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliaisAtivaInativa();
            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

            filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });

            return filial;
        }

        protected void btBuscarAluguel_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());

                CarregarAlugueis(Convert.ToInt32(ddlFilial.SelectedValue), competencia);
            }
            catch (Exception ex)
            {
                labErro.Text = string.Format("Erro: {0}", ex.Message);
            }

        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;
            if (buttonExcluir != null)
            {
                var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());

                lojaController.ExcluirBoletoAluguelDespesa(Convert.ToInt32(buttonExcluir.CommandArgument));
                CarregarAlugueis(Convert.ToInt32(ddlFilial.SelectedValue), competencia);
            }
        }
        protected void gvAluguel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ACOMPANHAMENTO_ALUGUEL aluguel = e.Row.DataItem as ACOMPANHAMENTO_ALUGUEL;

            if (aluguel != null)
            {
                Literal litAluguel = e.Row.FindControl("litAluguel") as Literal;
                var aluguelDespesa = baseController.BuscaAluguelDespesa(aluguel.CODIGO_ALUGUEL_DESPESA);
                if (aluguelDespesa != null)
                    litAluguel.Text = aluguelDespesa.DESCRICAO.ToString();

                Literal litTotal = e.Row.FindControl("litTotal") as Literal;
                var desconto = (aluguel.DESCONTO == null) ? 0 : Convert.ToDecimal(aluguel.DESCONTO);
                //valor positivo no desconto é pra descontar
                //valor negativo no desconto é pra acrescentar
                litTotal.Text = (aluguel.VALOR + (desconto * -1)).ToString("###,###,##0.00");

                Literal litBoleto = e.Row.FindControl("litBoleto") as Literal;
                litBoleto.Text = (aluguel.VALOR_BOLETO == 'S') ? "Sim" : "Não";

                Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                btExcluir.CommandArgument = aluguel.CODIGO_ACOMPANHAMENTO_ALUGUEL.ToString();
            }
        }
        protected void gvAluguel_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAluguel.FooterRow;
            decimal totValor = 0;
            decimal totDesconto = 0;

            if (footer != null)
            {
                foreach (GridViewRow r in gvAluguel.Rows)
                {
                    totValor += Convert.ToDecimal(r.Cells[3].Text);
                    totDesconto += Convert.ToDecimal(r.Cells[4].Text);
                }

                footer.Cells[1].Text = "Total";

                footer.Cells[3].Text = totValor.ToString("#####0.00");
                footer.Cells[4].Text = totDesconto.ToString("#####0.00");
                footer.Cells[5].Text = (totValor - totDesconto).ToString("#####0.00");
            }
        }

    }
}