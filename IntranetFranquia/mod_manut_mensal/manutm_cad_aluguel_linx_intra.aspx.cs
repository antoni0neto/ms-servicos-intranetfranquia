using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;

namespace Relatorios
{
    public partial class manutm_cad_aluguel_linx_intra : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        decimal totalBoleto = 0;
        decimal totalDesconto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarCompentencia();

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btEnviar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarCompentencia()
        {
            ddlCompetencia.DataSource = ObterCompetencia();
            ddlCompetencia.DataBind();
        }

        private bool ValidarCampos(bool inclusao)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;


            labLancamento.ForeColor = _OK;
            if (txtLancamento.Text.Trim() == "")
            {
                labLancamento.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarContasIntranet()
        {
            bool valido = true;

            foreach (GridViewRow r in gvBoletoDespesa.Rows)
            {
                var ddlContaIntranet = ((DropDownList)r.FindControl("ddlContaIntranet"));
                if (ddlContaIntranet.SelectedValue == "0")
                    return false;
            }

            return valido;
        }

        private List<ACOMPANHAMENTO_ALUGUEL_DESPESA> ObterContasIntranet()
        {
            var c = baseController.BuscaAluguelDespesas().OrderBy(p => p.DESCRICAO).ToList();
            c.Insert(0, new ACOMPANHAMENTO_ALUGUEL_DESPESA { CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA = 0, DESCRICAO = "Selecione" });
            return c;
        }
        private List<ACOMPANHAMENTO_ALUGUEL_MESANO> ObterCompetencia()
        {
            var v = baseController.BuscaReferencias().OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).ToList();
            v.Insert(0, new ACOMPANHAMENTO_ALUGUEL_MESANO { CODIGO_ACOMPANHAMENTO_MESANO = 0, DESCRICAO = "Selecione" });
            return v;
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroBusca.Text = "";

                if (!ValidarCampos(true))
                {
                    labErroBusca.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                CarregarDespesas();
            }
            catch (Exception ex)
            {
                labErroBusca.Text = ex.Message;
            }
        }

        private void CarregarDespesas()
        {
            var boletoDespesas = lojaController.ObterBoletoLancamentoDespesas(Convert.ToInt32(txtLancamento.Text.Trim()));

            gvBoletoDespesa.DataSource = boletoDespesas;
            gvBoletoDespesa.DataBind();
        }

        #region "INCLUSAO"
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroBusca.Text = "";

                if (ddlCompetencia.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Competência.";
                    return;
                }

                if (!ValidarContasIntranet())
                {
                    labErro.Text = "Preencha corretamente as <strong>Contas da Intranet</strong>.";
                    return;
                }

                foreach (GridViewRow r in gvBoletoDespesa.Rows)
                {
                    var marked = ((CheckBox)r.FindControl("cbMarcar")).Checked;

                    if (marked)
                    {
                        var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());

                        int codigoDespesa = Convert.ToInt32(((DropDownList)r.FindControl("ddlContaIntranet")).SelectedValue);
                        decimal valor = Convert.ToDecimal(((Literal)r.FindControl("litValor")).Text.Replace("R$", "").Trim());
                        decimal desconto = Convert.ToDecimal(((TextBox)r.FindControl("txtDesconto")).Text);
                        int item = Convert.ToInt32(((Literal)r.FindControl("litItem")).Text.Replace("R$", "").Trim());
                        var txtVencimento = (TextBox)r.FindControl("txtVencimento");
                        DateTime vencimento = competencia;
                        if (txtVencimento.Text != "")
                            vencimento = Convert.ToDateTime(txtVencimento.Text);

                        var codigoFilial = gvBoletoDespesa.DataKeys[r.RowIndex][0];
                        var codigoBoletoDespesa = Convert.ToInt32(gvBoletoDespesa.DataKeys[r.RowIndex][1]);

                        if (codigoBoletoDespesa == 0)
                        {
                            var acompAlu = new ACOMPANHAMENTO_ALUGUEL();
                            acompAlu.CODIGO_ALUGUEL_DESPESA = codigoDespesa;
                            acompAlu.VALOR = valor;
                            acompAlu.CODIGO_FILIAL = Convert.ToInt32(codigoFilial);
                            acompAlu.CODIGO_MESANO = Convert.ToInt32(ddlCompetencia.SelectedValue);
                            acompAlu.STATUS = 0;
                            acompAlu.VALOR_BOLETO = 'S';
                            acompAlu.LANCAMENTO = Convert.ToInt32(txtLancamento.Text);
                            acompAlu.ITEM = item;
                            acompAlu.DESCONTO = desconto;
                            acompAlu.COMPETENCIA = competencia;
                            acompAlu.DATA_VENCIMENTO = vencimento;

                            lojaController.InserirBoletoAluguelDespesa(acompAlu);
                        }
                        else
                        {
                            var acompAlu = lojaController.ObterBoletoAluguelDespesa(codigoBoletoDespesa);
                            if (acompAlu != null)
                            {
                                acompAlu.CODIGO_ALUGUEL_DESPESA = codigoDespesa;
                                acompAlu.VALOR = valor;
                                acompAlu.DATA_VENCIMENTO = vencimento;
                                acompAlu.COMPETENCIA = competencia;
                                acompAlu.DESCONTO = desconto;
                                acompAlu.CODIGO_FILIAL = Convert.ToInt32(codigoFilial);

                                lojaController.AtualizarBoletoAluguelDespesa(acompAlu);
                            }
                        }
                    }
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('manutm_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('manutm_menu.aspx', '_self'); }, Cadastro: function () { window.open('manutm_cad_aluguel_linx_intra.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        #endregion

        #region "GRID DESPESAS"
        protected void gvBoletoDespesa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_BOLETO_LANCAMENTO_DESPESASResult bol = e.Row.DataItem as SP_OBTER_BOLETO_LANCAMENTO_DESPESASResult;

                    if (bol != null)
                    {
                        CheckBox cbMarcar = e.Row.FindControl("cbMarcar") as CheckBox;
                        cbMarcar.Checked = (bol.CODIGO_ACOMPANHAMENTO_ALUGUEL > 0);

                        Literal litValor = e.Row.FindControl("litValor") as Literal;
                        litValor.Text = "R$ " + Convert.ToDecimal(bol.DEBITO).ToString("###,###,###,##0.00");

                        TextBox txtDesconto = e.Row.FindControl("txtDesconto") as TextBox;
                        txtDesconto.Text = bol.DESCONTO.ToString();

                        TextBox txtVencimento = e.Row.FindControl("txtVencimento") as TextBox;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimento.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);
                        txtVencimento.Text = (bol.DATA_VENCIMENTO == null) ? "" : Convert.ToDateTime(bol.DATA_VENCIMENTO).ToString("dd/MM/yyyy");

                        Literal litCompetencia = e.Row.FindControl("litCompetencia") as Literal;
                        litCompetencia.Text = (bol.COMPETENCIA == null) ? "-" : Convert.ToDateTime(bol.COMPETENCIA).ToString("dd/MM/yyyy");

                        DropDownList ddlContaIntranet = e.Row.FindControl("ddlContaIntranet") as DropDownList;
                        ddlContaIntranet.DataSource = ObterContasIntranet();
                        ddlContaIntranet.DataBind();

                        if (bol.CODIGO_ALUGUEL_DESPESA <= 0)
                        {
                            var dePara = baseController.ObterAluguelDespesaPorConta(bol.CONTA_CONTABIL);
                            if (dePara != null)
                                ddlContaIntranet.SelectedValue = dePara.CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA.ToString();
                        }
                        else
                        {
                            ddlContaIntranet.SelectedValue = bol.CODIGO_ALUGUEL_DESPESA.ToString();
                        }

                        totalBoleto += Convert.ToDecimal(bol.DEBITO);
                        totalDesconto += Convert.ToDecimal(bol.DESCONTO);

                    }
                }
            }

        }
        protected void gvBoletoDespesa_DataBound(object sender, EventArgs e)
        {

            GridViewRow footer = gvBoletoDespesa.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";

                footer.Cells[6].Text = "R$ " + totalBoleto.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = "R$ " + totalDesconto.ToString("###,###,###,##0.00");
            }
        }
        #endregion

    }
}
