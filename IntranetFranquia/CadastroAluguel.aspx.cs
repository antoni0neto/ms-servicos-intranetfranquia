using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Mail;
using System.Net;

namespace Relatorios
{
    public partial class CadastroAluguel : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                CarregaDropDownListReferencias();
                CarregaDropDownListContas();
                divItem.Visible = false;
                divGrid.Visible = false;
            }
        }

        private void CarregaGridViewAluguel(GridView _grid, int codigoFilial, int codigoReferencia, int status)
        {
            List<ACOMPANHAMENTO_ALUGUEL> listaAluguelItem = baseController.BuscaAlugueis(codigoFilial, codigoReferencia, status);

            if (listaAluguelItem != null)
            {
                _grid.DataSource = listaAluguelItem;
                _grid.DataBind();
                if (listaAluguelItem.Count > 0 && status == 1)
                    btConferir.Visible = true;
                else
                    btConferir.Visible = false;
            }
        }

        protected void CalendarDataVencimento_SelectionChanged(object sender, EventArgs e)
        {
            txtDataVencimento.Text = CalendarDataVencimento.SelectedDate.ToString("dd/MM/yyyy");
        }

        private void CarregaDropDownListContas()
        {
            ddlContas.DataSource = baseController.BuscaAluguelDespesas();
            ddlContas.DataBind();
        }

        private void CarregaDropDownListReferencias()
        {
            ddlReferencia.DataSource = baseController.BuscaReferencias().OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES);
            ddlReferencia.DataBind();
        }

        private void CarregaDropDownListFilial()
        {
            //Obter Filiais
            List<FILIAI> _filiais = ObterFiliais();
            ddlFilial.DataSource = _filiais;
            ddlFilial.DataBind();
        }

        private List<FILIAI> ObterFiliais()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliaisAtivaInativa();
            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            return filial;
        }

        protected void ddlContas_DataBound(object sender, EventArgs e)
        {
            ddlContas.Items.Add(new ListItem("Selecione", "0"));
            ddlContas.SelectedValue = "0";
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void ddlReferencia_DataBound(object sender, EventArgs e)
        {
            ddlReferencia.Items.Insert(0, new ListItem("Selecione", "0"));

            ddlReferencia.SelectedValue = "0";
        }

        protected void btBuscarAluguel_Click(object sender, EventArgs e)
        {
            ButtonSalvar.Enabled = true;
            divItem.Visible = true;
            divGrid.Visible = true;
            txtDataVencimento.Text = "";
            LabelFeedBack.Text = "";

            try
            {
                ControleCalendario();

                //Carregar Aluguel com Status = 0 (Aprovados)
                //CarregaGridViewAluguel(GridViewAluguel, Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);

                //Carregar Aluguel com Status = 1 (Pendentes)
                CarregaGridViewAluguel(GridViewAluguelPendente, Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }

        }

        private void ControleCalendario()
        {
            int mes;
            int ano;

            try
            {
                //Obter mes e ano da competencia selecionado
                mes = Convert.ToInt32(ddlReferencia.SelectedItem.Text.Split('/').GetValue(0));
                ano = Convert.ToInt32(ddlReferencia.SelectedItem.Text.Split('/').GetValue(1));

                CalendarDataVencimento.VisibleDate = new DateTime(ano, mes, 1);
                CalendarDataVencimento.ShowNextPrevMonth = false;
                CalendarDataVencimento.SelectedDates.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btConferir_Click(object sender, EventArgs e)
        {
            List<ACOMPANHAMENTO_ALUGUEL> _aluguel = new List<ACOMPANHAMENTO_ALUGUEL>();

            try
            {
                _aluguel = baseController.BuscarAluguelEmAberto(ddlFilial.SelectedValue, ddlReferencia.SelectedValue);

                foreach (ACOMPANHAMENTO_ALUGUEL _item in _aluguel)
                {
                    baseController.AtualizaAcompanhamentoAluguel(_item);
                }

                //CarregaGridViewAluguel(GridViewAluguel, Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);
                CarregaGridViewAluguel(GridViewAluguelPendente, Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = "Erro_Conferir. Message: " + ex.Message;
            }

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

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (txtDataVencimento.Text.Equals("") || txtDataVencimento.Text == null)
            {
                LabelFeedBack.Text = "Informe a Data de Vencimento.";
                return;
            }

            if (!ValidarData(ddlReferencia.SelectedItem.Text, Convert.ToDateTime(txtDataVencimento.Text)))
            {
                LabelFeedBack.Text = "Mês do Vencimento diferente da Competência. Informe a Data do Vencimento dentro da Competência.";
                return;
            }

            try
            {
                ACOMPANHAMENTO_ALUGUEL_DESPESA contaDespesa = baseController.BuscaContaAluguelPorCodigo(Convert.ToInt32(ddlContas.SelectedValue));

                if (contaDespesa != null)
                {
                    ACOMPANHAMENTO_ALUGUEL aluguel = new ACOMPANHAMENTO_ALUGUEL();

                    aluguel.CODIGO_ALUGUEL_DESPESA = contaDespesa.CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA;
                    aluguel.VALOR = Convert.ToDecimal(txtValor.Text);
                    aluguel.CODIGO_FILIAL = Convert.ToInt32(ddlFilial.SelectedValue);
                    aluguel.CODIGO_MESANO = Convert.ToInt32(ddlReferencia.SelectedValue);
                    aluguel.DATA_VENCIMENTO = CalendarDataVencimento.SelectedDate;
                    aluguel.STATUS = 0;
                    aluguel.VALOR_BOLETO = Convert.ToChar(ddlValorBoleto.SelectedValue);

                    lojaController.InserirBoletoAluguelDespesa(aluguel);

                    LabelFeedBack.Text = "Incluído com sucesso!";

                    //Carrega apenas status 0
                    CarregaGridViewAluguel(GridViewAluguelPendente, Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);

                    LimpaTela();

                    CalendarDataVencimento.SelectedDates.Clear();
                    txtDataVencimento.Text = "";
                }
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewAluguel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ACOMPANHAMENTO_ALUGUEL aluguel = e.Row.DataItem as ACOMPANHAMENTO_ALUGUEL;

            if (aluguel != null)
            {
                Button btExcluirAluguel = e.Row.FindControl("btExcluirAluguel") as Button;

                if (btExcluirAluguel != null)
                    btExcluirAluguel.CommandArgument = aluguel.CODIGO_ACOMPANHAMENTO_ALUGUEL.ToString();

                Literal literalAluguel = e.Row.FindControl("LiteralAluguel") as Literal;

                if (literalAluguel != null)
                {
                    ACOMPANHAMENTO_ALUGUEL_DESPESA aluguelDespesa = baseController.BuscaAluguelDespesa(aluguel.CODIGO_ALUGUEL_DESPESA);

                    if (aluguelDespesa != null)
                        literalAluguel.Text = aluguelDespesa.DESCRICAO.ToString();
                }

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

                Literal literalValorBoleto = e.Row.FindControl("LiteralValorBoleto") as Literal;
                if (literalValorBoleto != null)
                {
                    literalValorBoleto.Text = (aluguel.VALOR_BOLETO == 'S') ? "SIM" : "NÃO";
                }

            }
        }

        protected void GridViewAluguelPendente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ACOMPANHAMENTO_ALUGUEL aluguel = e.Row.DataItem as ACOMPANHAMENTO_ALUGUEL;

            if (aluguel != null)
            {
                Button btExcluirAluguel = e.Row.FindControl("btExcluirAluguelPend") as Button;

                if (btExcluirAluguel != null)
                    btExcluirAluguel.CommandArgument = aluguel.CODIGO_ACOMPANHAMENTO_ALUGUEL.ToString();

                Literal literalAluguel = e.Row.FindControl("LiteralAluguel") as Literal;

                if (literalAluguel != null)
                {
                    ACOMPANHAMENTO_ALUGUEL_DESPESA aluguelDespesa = baseController.BuscaAluguelDespesa(aluguel.CODIGO_ALUGUEL_DESPESA);

                    if (aluguelDespesa != null)
                        literalAluguel.Text = aluguelDespesa.DESCRICAO.ToString();
                }

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

                Literal literalValorBoleto = e.Row.FindControl("LiteralValorBoleto") as Literal;
                if (literalValorBoleto != null)
                {
                    literalValorBoleto.Text = (aluguel.VALOR_BOLETO == 'S') ? "SIM" : "NÃO";
                }
            }
        }

        protected void btExcluirAluguel_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                lojaController.ExcluirBoletoAluguelDespesa(Convert.ToInt32(buttonExcluir.CommandArgument));

                CarregaGridViewAluguel(GridViewAluguel, Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);

                LimpaTela();
            }
        }

        protected void btExcluirAluguelPend_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                lojaController.ExcluirBoletoAluguelDespesa(Convert.ToInt32(buttonExcluir.CommandArgument));

                CarregaGridViewAluguel(GridViewAluguelPendente, Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlReferencia.SelectedValue), 0);

                LimpaTela();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            HiddenFieldCodigoAluguelItem.Value = "0";
            txtValor.Text = string.Empty;

            ddlContas.SelectedValue = "0";
        }

        protected void CalendarDataVencimento_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "";
            }
        }

        protected void GridViewAluguel_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = GridViewAluguel.FooterRow;
            decimal _total = 0;

            if (_footer != null)
            {
                foreach (GridViewRow r in GridViewAluguel.Rows)
                {
                    _total += Convert.ToDecimal(r.Cells[3].Text);
                }

                _footer.Cells[0].Text = "Total";
                _footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                _footer.Cells[0].Font.Bold = true;
                _footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                _footer.Cells[3].Text = _total.ToString("######.#0");
                _footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                _footer.Cells[3].Font.Bold = true;
                _footer.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void GridViewAluguelPendente_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = GridViewAluguelPendente.FooterRow;
            decimal _total = 0;

            if (_footer != null)
            {

                foreach (GridViewRow r in GridViewAluguelPendente.Rows)
                {
                    _total += Convert.ToDecimal(r.Cells[3].Text);
                }

                _footer.HorizontalAlign = HorizontalAlign.Center;
                _footer.Cells[0].Text = "Total";
                _footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                _footer.Cells[0].Font.Bold = true;
                _footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                _footer.Cells[3].Text = _total.ToString("######.#0");
                _footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                _footer.Cells[3].Font.Bold = true;
                _footer.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
}