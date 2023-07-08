using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class manutm_cad_aluguel_antigo : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarFiliais();
                CarregarReferencias();
                CarregarContas();
            }
        }

        private void CarregarAlugueis(int codigoFilial, int codigoReferencia, int status)
        {
            var listaAluguelItem = baseController.BuscaAlugueis(codigoFilial, codigoReferencia, status);
            gvAluguel.DataSource = listaAluguelItem;
            gvAluguel.DataBind();
        }

        protected void calVencimento_SelectionChanged(object sender, EventArgs e)
        {
            txtDataVencimento.Text = calVencimento.SelectedDate.ToString("dd/MM/yyyy");
        }
        private void CarregarContas()
        {
            var despesas = baseController.BuscaAluguelDespesas();
            despesas.Insert(0, new ACOMPANHAMENTO_ALUGUEL_DESPESA { CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA = 0, DESCRICAO = "Selecione" });
            ddlContas.DataSource = despesas;
            ddlContas.DataBind();
        }
        private void CarregarReferencias()
        {
            var refs = baseController.BuscaReferencias().OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).ToList();
            refs.Insert(0, new ACOMPANHAMENTO_ALUGUEL_MESANO { CODIGO_ACOMPANHAMENTO_MESANO = 0, DESCRICAO = "Selecione" });

            ddlReferencia.DataSource = refs;
            ddlReferencia.DataBind();
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
                btSalvar.Enabled = true;
                txtDataVencimento.Text = "";
                labErro.Text = "";

                ControleCalendario();
                CarregarAlugueis(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);
            }
            catch (Exception ex)
            {
                labErro.Text = string.Format("Erro: {0}", ex.Message);
            }

        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtDataVencimento.Text.Equals("") || txtDataVencimento.Text == null)
                {
                    labErro.Text = "Informe a Data de Vencimento.";
                    return;
                }

                if (!ValidarData(ddlReferencia.SelectedItem.Text, Convert.ToDateTime(txtDataVencimento.Text)))
                {
                    labErro.Text = "Mês do Vencimento diferente da Competência. Informe a Data do Vencimento dentro da Competência.";
                    return;
                }

                ACOMPANHAMENTO_ALUGUEL_DESPESA contaDespesa = baseController.BuscaContaAluguelPorCodigo(Convert.ToInt32(ddlContas.SelectedValue));

                if (contaDespesa != null)
                {
                    var aluguel = new ACOMPANHAMENTO_ALUGUEL();
                    aluguel.CODIGO_ALUGUEL_DESPESA = contaDespesa.CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA;
                    aluguel.VALOR = Convert.ToDecimal(txtValor.Text);
                    aluguel.DESCONTO = (txtDesconto.Text == "") ? 0 : Convert.ToDecimal(txtDesconto.Text);
                    aluguel.CODIGO_FILIAL = Convert.ToInt32(ddlFilial.SelectedValue);
                    aluguel.CODIGO_MESANO = Convert.ToInt32(ddlReferencia.SelectedValue);
                    aluguel.DATA_VENCIMENTO = calVencimento.SelectedDate;
                    aluguel.STATUS = 0;
                    aluguel.VALOR_BOLETO = Convert.ToChar(ddlValorBoleto.SelectedValue);

                    lojaController.InserirBoletoAluguelDespesa(aluguel);

                    labErro.Text = "Incluído com sucesso!";

                    CarregarAlugueis(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);

                    LimparTela();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        private void ControleCalendario()
        {
            try
            {
                //Obter mes e ano da competencia selecionado
                var mes = Convert.ToInt32(ddlReferencia.SelectedItem.Text.Split('/').GetValue(0));
                var ano = Convert.ToInt32(ddlReferencia.SelectedItem.Text.Split('/').GetValue(1));

                calVencimento.VisibleDate = new DateTime(ano, mes, 1);
                calVencimento.ShowNextPrevMonth = false;
                calVencimento.SelectedDates.Clear();
            }
            catch (Exception ex)
            {
            }
        }
        protected void calVencimento_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
                e.Cell.Text = "";
        }
        private bool ValidarData(string competencia, DateTime data)
        {
            int mesComp = 0;
            int anoComp = 0;

            mesComp = Convert.ToInt32(ddlReferencia.SelectedItem.Text.Split('/').GetValue(0));
            anoComp = Convert.ToInt32(ddlReferencia.SelectedItem.Text.Split('/').GetValue(1));

            int mesData = 0;
            int anoData = 0;

            mesData = data.Month;
            anoData = data.Year;

            if ((mesData.ToString() + anoData.ToString()) == (mesComp.ToString() + anoComp.ToString()))
                return true;

            return false;
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;
            if (buttonExcluir != null)
            {
                lojaController.ExcluirBoletoAluguelDespesa(Convert.ToInt32(buttonExcluir.CommandArgument));
                CarregarAlugueis(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);
                LimparTela();
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
                litTotal.Text = (aluguel.VALOR - desconto).ToString("###,###,##0.00");

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

        private void LimparTela()
        {
            hidCodigoAluguelItem.Value = "0";
            txtValor.Text = "";
            txtDesconto.Text = "";
            ddlContas.SelectedValue = "0";
            calVencimento.SelectedDates.Clear();
            txtDataVencimento.Text = "";
        }
    }
}