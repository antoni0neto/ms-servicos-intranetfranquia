using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;

namespace Relatorios
{
    public partial class contabil_analise_ffc : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        decimal valorDebito = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                CarregaDropDownListBaixa();
                fsFechamento.Visible = false;
                btConferir.Visible = false;

                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();
                if (codigoPerfil != "2" && codigoPerfil != "3")
                {
                    labTitulo.Text = "Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Análise Fundo Fixo";
                    hrefVoltar.HRef = "contabil_menu.aspx";
                }

            }

            //Evitar duplo clique no botão
            btConferir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btConferir, null) + ";");
        }

        private void CarregaDropDownListBaixa()
        {
            ddlBaixa.DataSource = baseController.BuscarFundoFixoBaixa();
            ddlBaixa.DataBind();
        }
        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> listFiliais = baseController.BuscaFiliais_Intermediario(usuario);

                listFiliais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilial.DataSource = listFiliais;
                ddlFilial.DataBind();

                if (listFiliais != null && listFiliais.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }

        }
        private void CarregaGridView()
        {
            GridViewSaldo.DataSource = baseController.BuscaSaldosFundoFixo(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToDateTime(TextBoxDataInicio.Text + " 00:00:00"), Convert.ToDateTime(TextBoxDataFim.Text + " 23:59:59"), Convert.ToInt32(ddlBaixa.SelectedValue));
            GridViewSaldo.DataBind();

            GridViewDebitos.DataSource = null;
            GridViewDebitos.DataBind();

            GridViewCreditos.DataSource = null;
            GridViewCreditos.DataBind();
        }
        private void CarregaGridViewAnaliseFF(string codigo)
        {

            try
            {

                GridViewDebitos.DataSource = baseController.BuscaDespesasFechamento(codigo);
                GridViewDebitos.DataBind();

                GridViewCreditos.DataSource = baseController.BuscaReceitasFechamento(codigo);
                GridViewCreditos.DataBind();

                labNumeroFechamento.Text = codigo;
                hidNumeroFechamento.Value = codigo;
                lblErroGravar.Text = "";

                FUNDO_FIXO_HISTORICO_FECHAMENTO _ff = new FUNDO_FIXO_HISTORICO_FECHAMENTO();
                _ff = baseController.BuscaFundoFixoHistoricoFechamento(Convert.ToInt32(codigo));

                fsFechamento.Visible = true;

                bool _conferir = false;

                if (((USUARIO)Session["USUARIO"]).CODIGO_PERFIL == 24 ||
                    ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL == 25 ||
                    ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL == 1)
                    _conferir = true;

                if (_ff != null && _ff.CODIGO_BAIXA > 0)
                {
                    if (_ff.CODIGO_BAIXA == 1 && _conferir)
                    {
                        btConferir.Enabled = true;
                        btConferir.Visible = true;
                        GridViewDebitos.Enabled = true;
                        GridViewCreditos.Enabled = true;
                    }
                    else
                    {
                        btConferir.Enabled = false;
                        btConferir.Visible = false;
                        GridViewDebitos.Enabled = false;
                        GridViewCreditos.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblErroGravar.Text = ex.Message + " - StackTrace: " + ex.StackTrace.ToString();
            }
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }
        protected void ddlBaixa_DataBound(object sender, EventArgs e)
        {
            ddlBaixa.Items.Add(new ListItem("Todos", "0"));
            ddlBaixa.SelectedValue = "0";
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btDocumentos_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") ||
                TextBoxDataInicio.Text == null ||
                TextBoxDataFim.Text.Equals("") ||
                TextBoxDataFim.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            fsFechamento.Visible = false;
            btConferir.Enabled = false;
            btConferir.Visible = false;

            CarregaGridView();
        }

        protected void GridViewSaldo_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {

                    FUNDO_FIXO_HISTORICO_FECHAMENTO saldo = e.Row.DataItem as FUNDO_FIXO_HISTORICO_FECHAMENTO;
                    if (saldo != null)
                    {
                        Button btAnalisarFF = e.Row.FindControl("btAnalisarFF") as Button;

                        if (btAnalisarFF != null)
                        {
                            btAnalisarFF.CommandArgument = saldo.CODIGO.ToString();
                        }

                        Literal _Status = e.Row.FindControl("LiteralStatus") as Literal;

                        if (_Status != null)
                        {
                            FUNDO_FIXO_BAIXA _ff = new FUNDO_FIXO_BAIXA();
                            _ff = baseController.BuscarFundoFixoBaixa(Convert.ToInt32(saldo.CODIGO_BAIXA));

                            if (_ff != null)
                                _Status.Text = _ff.DESCRICAO;
                        }
                    }
                }
            }
        }

        protected void btAnalisarFF_Click(object sender, EventArgs e)
        {
            Button btAnalisarFF = sender as Button;
            labErro.Text = "";

            if (btAnalisarFF != null)
            {
                CarregaGridViewAnaliseFF(btAnalisarFF.CommandArgument);
            }
        }

        protected void GridViewDebitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            FUNDO_FIXO_HISTORICO_DESPESA despesa = e.Row.DataItem as FUNDO_FIXO_HISTORICO_DESPESA;

            if (despesa != null)
            {
                Literal litDespesa = e.Row.FindControl("LiteralDespesa") as Literal;
                litDespesa.Text = baseController.BuscaDespesa(despesa.CODIGO_CONTA_DESPESA.ToString()).DESCRICAO + ((despesa.DESC_DESPESA != null && despesa.DESC_DESPESA.Trim() != "") ? (" - " + despesa.DESC_DESPESA) : "");

                TextBox txtValor = e.Row.FindControl("txtValor") as TextBox;
                txtValor.Text = despesa.VALOR.ToString();

                valorDebito += despesa.VALOR;

            }
        }
        protected void GridViewCreditos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            FUNDO_FIXO_HISTORICO_RECEITA receita = e.Row.DataItem as FUNDO_FIXO_HISTORICO_RECEITA;

            if (receita != null)
            {
                Literal literalReceita = e.Row.FindControl("LiteralReceita") as Literal;

                if (literalReceita != null)
                    literalReceita.Text = baseController.BuscaReceita(receita.CODIGO_CONTA_RECEITA.ToString()).DESCRICAO;
            }
        }
        protected void GridViewCreditos_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = GridViewCreditos.FooterRow;

            if (_footer != null)
            {
                _footer.Cells[0].Text = "Total";

                decimal _total = 0;
                foreach (GridViewRow r in GridViewCreditos.Rows)
                {
                    if (r.Cells[2].Text != "")
                        _total += Convert.ToDecimal(r.Cells[2].Text);
                }
                _footer.Cells[2].Text = _total.ToString("###,###,##0.00");
            }
        }
        protected void GridViewDebitos_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = GridViewDebitos.FooterRow;

            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[2].Text = valorDebito.ToString("###,###,##0.00");
            }
        }

        protected void btConferir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                GerarLancamentoReceitaLINX();
                GerarLancamentoDespesaLINX(labNumeroFechamento.Text);

                //Atualizar fechamento
                AtualizarFechamento(2);

                //Recarrega grid fechamento
                btDocumentos_Click(null, null);
                //Recarrega grid despesas/receitas
                CarregaGridViewAnaliseFF(labNumeroFechamento.Text);

                lblErroGravar.Text = "Fechamento Nº " + labNumeroFechamento.Text + " fechado com sucesso.";
                //btConferir.Visible = false;
            }
            catch (Exception ex)
            {
                AtualizarFechamento(1);
                lblErroGravar.Text = "Message: " + ex.Message;
            }
        }

        private void AtualizarFechamento(int codigoBaixa)
        {

            //Atualizar fechamento
            FUNDO_FIXO_HISTORICO_FECHAMENTO _ff = new FUNDO_FIXO_HISTORICO_FECHAMENTO();
            if (_ff != null)
            {
                _ff.CODIGO = Convert.ToInt32(labNumeroFechamento.Text);
                _ff.CODIGO_BAIXA = codigoBaixa;
                _ff.DATA_CONF = DateTime.Now;

                //Atualiza FUNDO FIXO HISTORICO DESPESA
                usuarioController.AtualizaHistoricoFechamento(_ff);
            }

        }
        private void GerarLancamentoDespesaLINX(string numeroFechamento)
        {
            LancamentoContabilEntity _lancamento = null;
            SP_INTRANET_REGISTRA_LANCAMENTO_LINXResult sp = null;
            FUNDO_FIXO_CONTA_DESPESA _ffDespesa = new FUNDO_FIXO_CONTA_DESPESA();
            TIPO_LANCAMENTO tpEntrada = null;
            TIPO_LANCAMENTO tpSaida = null;

            try
            {
                var _listReceita = baseController.BuscaDespesasFechamentoSemLancamento(labNumeroFechamento.Text);

                tpSaida = usuarioController.BuscarTipoLancamento(2);

                foreach (var _list in _listReceita)
                {
                    if (_list.VALOR > 0)
                    {
                        _ffDespesa = baseController.BuscaContaPorCodigoContaLoja(_list.CODIGO_CONTA_DESPESA);
                        if (_ffDespesa != null)
                        {
                            tpEntrada = usuarioController.BuscarTipoLancamento(Convert.ToInt32(_ffDespesa.TIPO_LANCAMENTO));
                        }
                        else
                        {
                            throw new Exception("Nenhuma Conta/Tipo de Lançamento cadastrado para esta Despesa. Por favor, entre em contato com TI.");
                        }

                        _lancamento = new LancamentoContabilEntity();
                        _lancamento.Empresa = 1; //FIXO
                        _lancamento.Filial = ddlFilial.SelectedValue;
                        _lancamento.Data_Lancamento = (DateTime)_list.DATA;

                        var _contaReceita = baseController.BuscaContaReceita(Convert.ToInt32(_list.CODIGO_FILIAL));
                        if (_contaReceita != null)
                        {
                            _lancamento.Conta_Saida = _contaReceita.CODIGO; //CONTA DO FUNDO FIXO DA LOJA
                        }
                        else
                        {
                            throw new Exception("Nenhuma conta cadastrada para esta Filial. Por favor, entre em contato com TI.");
                        }

                        if (tpSaida != null)
                            _lancamento.Tipo_Lancamento_Saida = tpSaida.TIPO;

                        _lancamento.Conta_Entrada = _list.CODIGO_CONTA_DESPESA; //CONTA DA DESPESA

                        if (tpEntrada != null)
                            _lancamento.Tipo_Lancamento_Entrada = tpEntrada.TIPO;

                        _lancamento.Valor_Deposito = _list.VALOR;

                        _lancamento.Desc_Despesa = _list.DESC_DESPESA;


                        var ffHistorico = baseController.BuscaFundoFixoHistoricoFechamento(Convert.ToInt32(numeroFechamento));
                        if (ffHistorico != null)
                            _lancamento.Historico_Lancamento = ffHistorico.DATA.ToString("ddMMyyyy");

                        //Chamar procedure de processamento contábil
                        sp = baseController.ProcessarLancamentoContabil(_lancamento);

                        if (sp != null)
                        {
                            if (sp.CODIGO_ERRO == 0)
                            {
                                FUNDO_FIXO_HISTORICO_DESPESA _despesa = new FUNDO_FIXO_HISTORICO_DESPESA();

                                if (_despesa != null)
                                {
                                    _despesa.CODIGO = _list.CODIGO;
                                    _despesa.CODIGO_CONTA_DESPESA = _list.CODIGO_CONTA_DESPESA;
                                    _despesa.CODIGO_FECHAMENTO = _list.CODIGO_FECHAMENTO;
                                    _despesa.CODIGO_FILIAL = _list.CODIGO_FILIAL;
                                    _despesa.DATA = _list.DATA;
                                    _despesa.VALOR = _list.VALOR;
                                    _despesa.CODIGO_DESPESA_LOJA = _list.CODIGO_DESPESA_LOJA;
                                    _despesa.DESC_DESPESA = _list.DESC_DESPESA;
                                    _despesa.NUMERO_LANCAMENTO = sp.NUMERO_LANCAMENTO;

                                    //Atualiza FUNDO FIXO HISTORICO DESPESA
                                    usuarioController.AtualizaHistoricoDespesas(_despesa);
                                }
                            }
                            else
                            {
                                throw new Exception("Erro Procedure(Despesa). Código: " + sp.CODIGO_ERRO.ToString() + " - Mensagem: " + sp.MENSAGEM_ERRO);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sp = null;
            }
        }
        private void GerarLancamentoReceitaLINX()
        {
            LancamentoContabilEntity _lancamento = null;
            SP_INTRANET_REGISTRA_LANCAMENTO_LINXResult sp = null;
            TIPO_LANCAMENTO tpEntrada = null;
            TIPO_LANCAMENTO tpSaida = null;

            try
            {
                var _listReceita = baseController.BuscaReceitasFechamentoSemLancamento(labNumeroFechamento.Text);
                tpEntrada = usuarioController.BuscarTipoLancamento(1);
                tpSaida = usuarioController.BuscarTipoLancamento(2);

                foreach (var _list in _listReceita)
                {
                    if (_list.VALOR > 0)
                    {
                        _lancamento = new LancamentoContabilEntity();
                        _lancamento.Empresa = 1; //FIXO
                        _lancamento.Filial = ddlFilial.SelectedValue;
                        _lancamento.Data_Lancamento = (DateTime)_list.DATA;
                        _lancamento.Conta_Saida = "1.1.1.01.0001";
                        if (tpSaida != null)
                            _lancamento.Tipo_Lancamento_Saida = tpSaida.TIPO;
                        else
                            _lancamento.Tipo_Lancamento_Saida = "LSC";
                        _lancamento.Conta_Entrada = _list.CODIGO_CONTA_RECEITA;
                        if (tpEntrada != null)
                            _lancamento.Tipo_Lancamento_Entrada = tpEntrada.TIPO;
                        else
                            _lancamento.Tipo_Lancamento_Entrada = "LEC";
                        _lancamento.Valor_Deposito = _list.VALOR;

                        //Chamar procedure de processamento contábil
                        sp = baseController.ProcessarLancamentoContabil(_lancamento);

                        if (sp != null)
                        {
                            if (sp.CODIGO_ERRO == 0)
                            {
                                FUNDO_FIXO_HISTORICO_RECEITA _receita = new FUNDO_FIXO_HISTORICO_RECEITA();

                                if (_receita != null)
                                {
                                    _receita.CODIGO = _list.CODIGO;
                                    _receita.CODIGO_CONTA_RECEITA = _list.CODIGO_CONTA_RECEITA;
                                    _receita.CODIGO_FECHAMENTO = _list.CODIGO_FECHAMENTO;
                                    _receita.CODIGO_FILIAL = _list.CODIGO_FILIAL;
                                    _receita.DATA = _list.DATA;
                                    _receita.VALOR = _list.VALOR;
                                    _receita.NUMERO_LANCAMENTO = sp.NUMERO_LANCAMENTO;

                                    //Atualiza FUNDO FIXO HISTORICO RECEITA
                                    usuarioController.AtualizaHistoricoReceitas(_receita);
                                }
                            }
                            else
                            {
                                throw new Exception("Erro Procedure(Receita). Código: " + sp.CODIGO_ERRO.ToString() + " - Mensagem: " + sp.MENSAGEM_ERRO);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sp = null;
            }

        }

        private void GerarTxtFechamentoFundoFixo(StringWriter _writer)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "XXXXX.txt"));
            Response.ContentType = "text/plain";
            //StringWriter sw = new StringWriter();
            //sw.Write("EXEMPLO");
            //sw.Write(_writer);
            Response.Write(_writer.ToString());
            Response.End();
        }

        protected void txtValor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var codigoFechamento = labNumeroFechamento.Text.Trim();
                var codigoFilial = ddlFilial.SelectedValue.Trim();

                if (codigoFechamento == "" || Convert.ToInt32(codigoFechamento) <= 0)
                {
                    labErro.Text = "Erro ao obter numero do lancamento.";
                    return;
                }

                if (codigoFilial == "" || codigoFilial == "0")
                {
                    labErro.Text = "Erro ao obter filial.";
                    return;
                }

                TextBox _txtValor = ((TextBox)sender);

                if (_txtValor.Text == "" || _txtValor.Text == "0")
                {
                    labErro.Text = "Informe o valor para alterar.";
                    return;
                }

                GridViewRow row = (GridViewRow)_txtValor.NamingContainer;
                string codigoDespesa = GridViewDebitos.DataKeys[row.RowIndex].Value.ToString();
                var despesa = baseController.BuscaDespesaPorCodigo(Convert.ToInt32(codigoDespesa));
                despesa.VALOR = Convert.ToDecimal(_txtValor.Text.Trim());

                baseController.AtualizarDespesaFundoFixo(despesa);

                AtualizarHistoricoFFC(codigoFilial, codigoFechamento);



            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                Button btExcluir = (Button)sender;

                var codigoFechamento = labNumeroFechamento.Text.Trim();
                var codigoFilial = ddlFilial.SelectedValue.Trim();

                if (codigoFechamento == "" || Convert.ToInt32(codigoFechamento) <= 0)
                {
                    labErro.Text = "Erro ao obter numero do lancamento.";
                    return;
                }

                if (codigoFilial == "" || codigoFilial == "0")
                {
                    labErro.Text = "Erro ao obter filial.";
                    return;
                }

                GridViewRow row = (GridViewRow)btExcluir.NamingContainer;
                string codigoDespesa = GridViewDebitos.DataKeys[row.RowIndex].Value.ToString();
                baseController.ExcluirDespesaFundoFixo(Convert.ToInt32(codigoDespesa));

                AtualizarHistoricoFFC(codigoFilial, codigoFechamento);

                GridViewDebitos.DataSource = baseController.BuscaDespesasFechamento(codigoFechamento);
                GridViewDebitos.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void AtualizarHistoricoFFC(string codigoFilial, string codigoFechamento)
        {
            var ret = baseController.AtualizarFundoFixo(codigoFilial, Convert.ToInt32(codigoFechamento));
            if (ret != null && ret.OK == 1)
            {
                labErro.Text = "Atualizado com sucesso.";

                GridViewDebitos.DataSource = baseController.BuscaDespesasFechamento(codigoFechamento);
                GridViewDebitos.DataBind();
            }
            else
            {
                labErro.Text = "Erro ao recalcular saldo de FFC.";
            }
        }

    }
}
