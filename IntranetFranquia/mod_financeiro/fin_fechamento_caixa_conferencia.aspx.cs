using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios.mod_financeiro
{
    public partial class fin_fechamento_caixa_conferencia : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();

                if (codigoPerfil == "27")
                    hrefVoltar.HRef = "";

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "../DefaultFinanceiro.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";



                CarregaDropDownListFilial();
            }
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> listFiliais = baseController.BuscaFiliais_Intermediario(usuario);

                ddlFilial.DataSource = listFiliais;
                ddlFilial.DataBind();

                if (listFiliais != null && listFiliais.Count() == 1)
                    ddlFilial.SelectedValue = listFiliais.First().COD_FILIAL;
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                CalendarDataInicio.SelectedDate = DateTime.Now;
                CalendarDataFim.SelectedDate = DateTime.Now;
                ddlFilial.SelectedIndex = 0;

                gvFechamentos.DataSource = null;
                gvFechamentos.DataBind();
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregaGridViewFechamento()
        {
            gvFechamentos.DataSource = baseController.BuscaFechamentosCaixa(CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate, Convert.ToInt32(ddlFilial.SelectedValue));
            gvFechamentos.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "Informe um período válido.";
                return;
            }

            if (ddlFilial.SelectedValue == "")
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "Selecione a Filial.";
                return;
            }

            CarregaGridViewFechamento();
        }

        protected void gvFechamentos_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in gvFechamentos.Rows)
            {
                if ((Convert.ToDecimal(item.Cells[3].Text) - Convert.ToDecimal(item.Cells[5].Text)) < Convert.ToDecimal(item.Cells[4].Text))
                {
                    item.Cells[0].BackColor = System.Drawing.Color.Red;
                    item.Cells[1].BackColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void gvFechamentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            FECHAMENTO_CAIXA caixa = e.Row.DataItem as FECHAMENTO_CAIXA;

            if (caixa != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField _hdnCodigoFechamentogrd = e.Row.FindControl("hdnCodigoFechamentogrd") as HiddenField;

                    if (_hdnCodigoFechamentogrd != null)
                        _hdnCodigoFechamentogrd.Value = caixa.CODIGO_CAIXA.ToString();

                    Button _btnVer = e.Row.FindControl("btnVer") as Button;
                    _btnVer.Attributes.Add("OnClick", "AbrirJanela(" + _hdnCodigoFechamentogrd.Value.ToString() + ");");

                    Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                    if (literalFilial != null)
                        literalFilial.Text = baseController.BuscaFilialCodigoInt(caixa.CODIGO_FILIAL).FILIAL;

                    Literal literalSaldoDinheiro = e.Row.FindControl("LiteralSaldoDinheiro") as Literal;

                    var saldoDin = (caixa.VALOR_DINHEIRO - (caixa.VALOR_RETIRADA + caixa.VALOR_DEVOLUCAO));

                    if (literalSaldoDinheiro != null)
                        literalSaldoDinheiro.Text = saldoDin.ToString();



                    Button _btnReabrir = e.Row.FindControl("btnReabrir") as Button;
                    Literal _litDepositado = e.Row.FindControl("litDepositado") as Literal;
                    Literal _litLancado = e.Row.FindControl("litLancado") as Literal;

                    List<DEPOSITO_HISTORICO> historicoDeposito = baseController.BuscaHistoricoDeposito(caixa);

                    if (historicoDeposito != null)
                    {
                        foreach (DEPOSITO_HISTORICO hist in historicoDeposito)
                        {
                            if (hist.CODIGO_SALDO != 0)
                            {
                                _btnReabrir.Visible = false;

                                _litDepositado.Visible = true;
                                _litDepositado.Text = "<font size='2' face='Calibri' color='Red'>" + _litDepositado.Text.Trim() + "</font>";
                            }
                        }
                    }

                    List<FUNDO_FIXO_HISTORICO_RECEITA> historicoReceita = usuarioController.BuscaHistoricoReceitas(Convert.ToInt32(_hdnCodigoFechamentogrd.Value));

                    if (historicoReceita != null)
                    {
                        foreach (FUNDO_FIXO_HISTORICO_RECEITA hist in historicoReceita)
                        {
                            if (hist.NUMERO_LANCAMENTO != null)
                            {
                                _btnReabrir.Visible = false;
                                _litDepositado.Visible = false;

                                _litLancado.Visible = true;
                                _litLancado.Text = "<font size='2' face='Calibri' color='Red'>" + _litLancado.Text.Trim() + "</font>";
                            }
                        }
                    }

                    if (saldoDin <= 0)
                    {
                        _btnReabrir.Visible = false;

                        _litDepositado.Visible = true;
                        _litDepositado.Text = "<font size='2' face='Calibri' color='Red'>-</font>";
                    }

                }
            }
        }

        protected void gvFechamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument.ToString().Trim() == "")
            { return; }

            int resultado;
            if (!int.TryParse(e.CommandArgument.ToString(), out resultado))
            { return; }

            Int32 iIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow dgItem = gvFechamentos.Rows[iIndex];

            if (e.CommandName == "reabrir_Fechamento")
            {
                Button _btnReabrir = (Button)(dgItem.FindControl("btnReabrir"));
                HiddenField _hdnCodigoFechamentogrd = (HiddenField)(dgItem.FindControl("hdnCodigoFechamentogrd"));

                FECHAMENTO_CAIXA caixa = baseController.BuscaCaixa(Convert.ToInt32(_hdnCodigoFechamentogrd.Value));

                if (caixa != null)
                {
                    usuarioController.ExcluiCaixa(caixa.CODIGO_CAIXA);
                    usuarioController.ExcluiHistoricoDeposito(caixa);
                    usuarioController.ExcluiHistoricoReceita(caixa);

                    CarregaGridViewFechamento();
                }
            }
            else if (e.CommandName == "ver_Fechamento")
            {
                return;
                //Button _btnVer = (Button)(dgItem.FindControl("btnVer"));
                //HiddenField _hdnCodigoFechamentogrd = (HiddenField)(dgItem.FindControl("hdnCodigoFechamentogrd"));

                //Response.Redirect(string.Format("AlterarFechamentoCaixa.aspx?CodigoCaixa={0}", _hdnCodigoFechamentogrd.Value.ToString()));
            }
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
            ddlFilial.Items.Add(new ListItem("Todas", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void btnFecharMensagem_Click(object sender, EventArgs e)
        {
            txtMensagemErro.Text = "";
            panMensagem.Visible = false;
            panPrincipal.Enabled = true;
        }
    }
}