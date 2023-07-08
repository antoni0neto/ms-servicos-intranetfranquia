using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Text;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Transactions;

namespace Relatorios.mod_financeiro
{
    public partial class fin_Deposito_Banco_Conferencia : System.Web.UI.Page
    {
        ContabilidadeController contabilidadeController = new ContabilidadeController();
        BaseController baseController = new BaseController();
        ImagemController imagemController = new ImagemController();
        UsuarioController usuarioController = new UsuarioController();

        decimal dTotalDepositado = 0;
        Int16 iTotalDepositado = 0;
        decimal dValorParaDeposito = 0;
        decimal dValorParaDeposito2 = 0;

        decimal dValorTotalImagensVinculadas = 0;
        decimal dValorTotalImagensLancadas = 0;

        private Boolean arquivoExiste(String caminho)
        {
            Boolean retorno = false;

            if (File.Exists(caminho))
            {
                retorno = true;
            }

            return retorno;
        }

        private void CarregarDepositos(FiltroDepositos filtro)
        {
            try
            {
                List<SP_OBTER_FILIAIS_DEPOSITO_BANCOResult> gvFiliaisDepositos = new List<SP_OBTER_FILIAIS_DEPOSITO_BANCOResult>();

                gvFiliaisDepositos = contabilidadeController.BuscaFiliaisDepositos(filtro.DataDeposito.ToShortDateString(), filtro.TipoDataDeposito, filtro.CodigoFilial, filtro.NomeBanco, filtro.CodigoBanco, filtro.Status, filtro.IgnorarData);

                //gvCentroCustoFuncionario.Columns[0].SortExpression = "DESC_GRUPO_CENTRO_CUSTO";
                gvDepositos.DataSource = gvFiliaisDepositos;
                gvDepositos.DataBind();
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarFechamentosDeposito(int codigoSaldo)
        {
            List<SP_OBTER_FECHAMENTOS_DEPOSITO_BANCOResult> gvFechamentosDeposito = new List<SP_OBTER_FECHAMENTOS_DEPOSITO_BANCOResult>();

            gvFechamentosDeposito = contabilidadeController.BuscaFechamentosDepositos(codigoSaldo);

            gvReferencia.DataSource = gvFechamentosDeposito;
            gvReferencia.DataBind();

            gvImagensReferencias.DataSource = gvFechamentosDeposito;
            gvImagensReferencias.DataBind();
        }

        private void CarregarImagensDocumento(int codigoDocuemnto)
        {
            List<DEPOSITO_DOCUMENTO> gvDepositoDocumento = new List<DEPOSITO_DOCUMENTO>();

            gvDepositoDocumento = baseController.BuscaDepositoDocumento(codigoDocuemnto);

            gvImagensVinculadas.DataSource = gvDepositoDocumento;
            gvImagensVinculadas.DataBind();
        }

        private void CarregarImagensDeposito(int codigoLoja, DateTime dataDigitada)
        {
            List<IMAGEM> gvImagem = new List<IMAGEM>();

            gvImagem = imagemController.BuscaImagensDeposito(codigoLoja, dataDigitada);

            gvImagensDeposito.DataSource = gvImagem;
            gvImagensDeposito.DataBind();
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

        private void CarregaDropDownListBanco()
        {
            List<DEPOSITO_BANCO> listBancos = baseController.ListarNomeBanco();

            List<NOME_BANCOS> listBancosDistinct = new List<NOME_BANCOS>();
            listBancosDistinct = listBancos.Select(b => new NOME_BANCOS
            {
                NOME = b.NOME.Trim()
            }).Distinct().OrderBy(b => b.NOME).ToList();

            ddlBanco.DataSource = listBancosDistinct.GroupBy(b => b.NOME).Select(b => b.First());
            ddlBanco.DataBind();
        }

        private void CarregaDropDownListContaCorrente(String NomeBanco)
        {
            List<SP_OBTER_CONTAS_CORRENTESResult> listContasCorrentes = contabilidadeController.ListarContasCorrentes(NomeBanco);

            ddlContaCorrente.DataSource = listContasCorrentes;
            ddlContaCorrente.DataBind();
        }

        private void CarregaDropDownListStatus()
        {
            ddlStatus.DataSource = baseController.BuscaDepositoBaixas();
            ddlStatus.DataBind();
        }

        private void alterarDeposito(DEPOSITO_SEMANA depositoSelecionado)
        {
            DEPOSITO_SEMANA depositoSemana = new DEPOSITO_SEMANA();

            depositoSemana.CODIGO_DEPOSITO = depositoSelecionado.CODIGO_DEPOSITO;
            depositoSemana.AGENCIA = "0";
            depositoSemana.ANO_SEMANA = 0;
            depositoSemana.ASSINATURA = depositoSelecionado.ASSINATURA;
            depositoSemana.BANCO = 0;
            depositoSemana.CODIGO_FILIAL = depositoSelecionado.CODIGO_FILIAL;
            depositoSemana.COMPROVANTE = "";
            depositoSemana.CONTA = "0";
            depositoSemana.DATA_DEPOSITO = depositoSelecionado.DATA_DEPOSITO;
            depositoSemana.VALOR_A_DEPOSITAR = depositoSelecionado.VALOR_A_DEPOSITAR;
            depositoSemana.VALOR_DEPOSITADO = depositoSelecionado.VALOR_DEPOSITADO;
            depositoSemana.DIFERENCA = depositoSelecionado.DIFERENCA;
            depositoSemana.OBS = depositoSelecionado.OBS;
            depositoSemana.SALDO_INICIAL = 0;

            SP_OBTER_CODIGO_SALDO_DO_DEPOSITOResult codigoSaldoDesteDeposito = new SP_OBTER_CODIGO_SALDO_DO_DEPOSITOResult();
            codigoSaldoDesteDeposito = baseController.ObterCodigoSaldoDoDeposito(depositoSelecionado.CODIGO_FILIAL.ToString(), depositoSelecionado.DATA_DEPOSITO.ToShortDateString(), depositoSelecionado.CODIGO_DEPOSITO);

            List<DEPOSITO_SALDO> listaSaldo = baseController.BuscaUltimosSaldos(Convert.ToInt32(depositoSelecionado.CODIGO_FILIAL));

            if (listaSaldo != null)
            {
                int i = 0;

                foreach (DEPOSITO_SALDO item in listaSaldo)
                {
                    if (item.CODIGO_SALDO <= codigoSaldoDesteDeposito.CODIGO_SALDO)
                    {
                        i++;
                    }

                    if (i == 2)
                    {
                        depositoSemana.SALDO_INICIAL = item.VALOR_SALDO;
                        break;
                    }
                }
            }

            depositoSemana.SALDO_FINAL = depositoSemana.SALDO_INICIAL + depositoSemana.DIFERENCA;
            depositoSemana.CODIGO_BAIXA = 2;

            baseController.AtualizaDepositoSemana(depositoSemana);
        }

        private LancamentoLinha gravarLancamento(DEPOSITO_SEMANA depositoSelecionado, String sCodigoFilialSelecionada)
        {
            LancamentoLinha lancamento = new LancamentoLinha();
            lancamento.OK = false;

            try
            {
                alterarDeposito(depositoSelecionado);

                LancamentoContabilEntity _lancamento = null;
                SP_INTRANET_REGISTRA_LANCAMENTO_LINXResult sp = null;
                TIPO_LANCAMENTO tpEntrada = null;
                TIPO_LANCAMENTO tpSaida = null;
                DateTime _dataInicio = Convert.ToDateTime("2014-09-16 00:00:00");
                DateTime _dataInicial = Convert.ToDateTime("2014-09-16 00:00:00");

                try
                {

                    DEPOSITO_SEMANA _dS = baseController.BuscaDepositoSemana(depositoSelecionado.CODIGO_DEPOSITO);
                    if (_dS != null)
                        _dataInicio = _dS.DATA_INICIO;

                    if (_dataInicio >= _dataInicial)
                    {

                        var _listDocumento = usuarioController.BuscaDocumentosParaLancamento(depositoSelecionado.CODIGO_DEPOSITO);
                        tpEntrada = usuarioController.BuscarTipoLancamento(3);
                        tpSaida = usuarioController.BuscarTipoLancamento(2);

                        foreach (var _list in _listDocumento)
                        {
                            //Obter Conta Banco
                            var _contaBanco = baseController.BuscarDadosBanco((int)_list.CODIGO_BANCO);

                            _lancamento = new LancamentoContabilEntity();
                            _lancamento.Empresa = 1; //FIXO
                            _lancamento.Filial = sCodigoFilialSelecionada;
                            _lancamento.Data_Lancamento = (DateTime)_list.DATA_LANCAMENTO;
                            _lancamento.Conta_Saida = "1.1.1.01.0001";
                            if (tpSaida != null)
                                _lancamento.Tipo_Lancamento_Saida = tpSaida.TIPO;
                            else
                                _lancamento.Tipo_Lancamento_Saida = "LSC";
                            _lancamento.Conta_Entrada = _contaBanco.CONTA_CTB;
                            if (tpEntrada != null)
                                _lancamento.Tipo_Lancamento_Entrada = tpEntrada.TIPO;
                            else
                                _lancamento.Tipo_Lancamento_Entrada = "ECC";
                            _lancamento.Valor_Deposito = _list.VALOR;

                            //Chamar procedure de processamento contábil
                            sp = baseController.ProcessarLancamentoContabil(_lancamento);

                            if (sp != null)
                            {
                                if (sp.CODIGO_ERRO == 0)
                                {
                                    DEPOSITO_DOCUMENTO _deposito = new DEPOSITO_DOCUMENTO();
                                    USUARIO usuario = (USUARIO)Session["USUARIO"];

                                    if (usuario != null)
                                    {
                                        _deposito.CODIGO_DOCUMENTO = _list.CODIGO_DOCUMENTO;
                                        _deposito.NUMERO_LANCAMENTO = sp.NUMERO_LANCAMENTO;
                                        _deposito.USUARIO_CONF = usuario.CODIGO_USUARIO;
                                        _deposito.DATA_CONF = DateTime.Now;

                                        //Atualiza DEPOSITO_DOCUMENTO
                                        usuarioController.AtualizaDocumento(_deposito);

                                        DEPOSITO_DOCUMENTO depositoData = usuarioController.BuscaDepositoDocumento(_list.CODIGO_DOCUMENTO);
                                        if (depositoData != null)
                                        {
                                            lancamento.DataLancamento = depositoData.DATA_LANCAMENTO.ToString();
                                        }

                                        lancamento.Lancamentos += _deposito.NUMERO_LANCAMENTO + ", ";
                                        lancamento.Usuario = usuario.NOME_USUARIO;
                                        lancamento.DataUsuario = _deposito.DATA_CONF.ToString();
                                    }
                                }
                                else
                                {
                                    panMensagem.Visible = true;
                                    txtMensagemErro.Text = "Erro Procedure(2). Código: " + sp.CODIGO_ERRO.ToString() + " - Mensagem: " + sp.MENSAGEM_ERRO;
                                    return lancamento;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = ex.Message;
                    return lancamento;
                }
                finally
                {
                    sp = null;
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = ex.Message;
                return lancamento;
            }

            if (lancamento.Lancamentos.Trim() != "")
                lancamento.Lancamentos = lancamento.Lancamentos.Trim().Substring(0, lancamento.Lancamentos.Trim().Length - 1);

            lancamento.OK = true;

            return lancamento;
        }

        private FiltroDepositos validarFiltros()
        {
            DateTime resultado;

            FiltroDepositos filtro = new FiltroDepositos();

            try
            {
                filtro.OK = false;

                if (txtDeposito.Text.Equals("") || txtDeposito.Text == null)
                {
                    panPrincipal.Enabled = false;
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Necessário informar uma data de depósito para efetuar a busca dos lançamentos em dinheiro.";
                    return filtro;
                }

                if (!DateTime.TryParse(this.txtDeposito.Text.Trim(), out resultado))
                {
                    panPrincipal.Enabled = false;
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Necessário informar uma data de depósito válida para efetuar a busca dos lançamentos em dinheiro.";
                    return filtro;
                }

                filtro.DataDeposito = Convert.ToDateTime(txtDeposito.Text);

                if (optDataBanco.Checked == true)
                {
                    filtro.TipoDataDeposito = 'B';
                }
                else if (optDataFechamento.Checked == true)
                {
                    filtro.TipoDataDeposito = 'F';
                }
                else
                {
                    filtro.TipoDataDeposito = 'D';
                }

                if (ddlFilial.Text == "0")
                {
                    filtro.CodigoFilial = "";
                }
                else
                {
                    filtro.CodigoFilial = ddlFilial.SelectedValue;
                }

                if (ddlBanco.Text == "0")
                {
                    filtro.NomeBanco = "";
                }
                else
                {
                    filtro.NomeBanco = ddlBanco.Text;
                }

                if (ddlContaCorrente.Text == "0")
                {
                    filtro.CodigoBanco = 0;
                }
                else
                {
                    filtro.CodigoBanco = Convert.ToInt32(ddlContaCorrente.SelectedValue);
                }

                if (ddlStatus.Text == "0")
                {
                    filtro.Status = 0;
                }
                else
                {
                    filtro.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                }

                if (chkIgnorarData.Checked == true)
                {
                    filtro.IgnorarData = 'S';
                }
                else
                {
                    filtro.IgnorarData = 'N';
                }

                filtro.OK = true;
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }

            return filtro;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CalendarDeposito.SelectedDate = DateTime.Now;
                txtDeposito.Text = DateTime.Now.ToShortDateString();

                CarregaDropDownListFilial();
                CarregaDropDownListBanco();
                CarregaDropDownListContaCorrente("");
                CarregaDropDownListStatus();
            }

            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            Page.MaintainScrollPositionOnPostBack = true;

            txtValorImgAvulsa.Attributes.Add("onkeyup", "formataValor(this, event);");
            txtValorReal.Attributes.Add("onkeyup", "formataValor(this, event);");
            txtDeposito.Attributes.Add("onkeyup", "formataData(this, event);");
            txtDtLctoImgAvulsa.Attributes.Add("onkeyup", "formataData(this, event);");

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        protected void CalendarDeposito_SelectionChanged(object sender, EventArgs e)
        {
            txtDeposito.Text = CalendarDeposito.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btnDiaAnterior_Click(object sender, EventArgs e)
        {
            CalendarDeposito.SelectedDate = CalendarDeposito.SelectedDate.AddDays(-1);
            txtDeposito.Text = CalendarDeposito.SelectedDate.ToString("dd/MM/yyyy");

            try
            {
                FiltroDepositos filtro = new FiltroDepositos();

                filtro = validarFiltros();

                if (filtro.OK == true)
                {
                    CarregarDepositos(filtro);
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void btnDiaSeguinte_Click(object sender, EventArgs e)
        {
            CalendarDeposito.SelectedDate = CalendarDeposito.SelectedDate.AddDays(1);
            txtDeposito.Text = CalendarDeposito.SelectedDate.ToString("dd/MM/yyyy");

            try
            {
                FiltroDepositos filtro = new FiltroDepositos();

                filtro = validarFiltros();

                if (filtro.OK == true)
                {
                    CarregarDepositos(filtro);
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                FiltroDepositos filtro = new FiltroDepositos();

                filtro = validarFiltros();

                if (filtro.OK == true)
                {
                    CarregarDepositos(filtro);
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void gvDepositos_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvDepositos.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].ColumnSpan = 2;
                footer.Cells[0].Text = "Localizado  " + Convert.ToString(iTotalDepositado) + "  Depósitos";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[2].Text = "Total:";
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = dTotalDepositado.ToString("N2");
                footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void gvDepositos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        HiddenField _hdnCodigoDepositogrd = e.Row.FindControl("hdnCodigoDepositogrd") as HiddenField;
                        Literal _litDataDeposito = e.Row.FindControl("litDataDeposito") as Literal;
                        Literal _litHoraDeposito = e.Row.FindControl("litHoraDeposito") as Literal;

                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;

                        Literal _litImagens = e.Row.FindControl("litImagens") as Literal;
                        Button _btnImagens = e.Row.FindControl("btnImagens") as Button;

                        Literal _litDTsReferencia = e.Row.FindControl("litDTsReferencia") as Literal;
                        Button _btnReferencia = e.Row.FindControl("btnReferencia") as Button;

                        Literal _litLancamentos = e.Row.FindControl("litLancamentos") as Literal;
                        Literal _litDataLancamento = e.Row.FindControl("litDataLancamento") as Literal;
                        Button _btnObs = e.Row.FindControl("btnObs") as Button;
                        Button _btnFecharObs = e.Row.FindControl("btnFecharObs") as Button;
                        Literal _litLblObs = e.Row.FindControl("litLblObs") as Literal;
                        Literal _litObs = e.Row.FindControl("litObs") as Literal;
                        Literal _litSep3 = e.Row.FindControl("litSep3") as Literal;
                        Literal _litSep4 = e.Row.FindControl("litSep4") as Literal;
                        Literal _litConferido = e.Row.FindControl("litConferido") as Literal;
                        Literal _litEmAberto = e.Row.FindControl("litEmAberto") as Literal;

                        Literal _litBanco = e.Row.FindControl("litBanco") as Literal;
                        Literal _litAgencia = e.Row.FindControl("litAgencia") as Literal;
                        Literal _litContaCorrente = e.Row.FindControl("litContaCorrente") as Literal;
                        Literal _litSep1 = e.Row.FindControl("litSep1") as Literal;
                        Literal _litSep2 = e.Row.FindControl("litSep2") as Literal;

                        Literal _litDataPendente = e.Row.FindControl("litDataPendente") as Literal;
                        Button _btnPendenteAvancar = e.Row.FindControl("btnPendenteAvancar") as Button;
                        Button _btnPendenteRetornar = e.Row.FindControl("btnPendenteRetornar") as Button;
                        Button _btnPendenteRemover = e.Row.FindControl("btnPendenteRemover") as Button;

                        Button _btnExcluir = e.Row.FindControl("btnExcluir") as Button;

                        if (_litFilial != null)
                        {
                            _litFilial.Text = "<font size='1' face='Calibri'>" + _litFilial.Text.Trim() + "</font>";
                        }

                        if (_litDataDeposito != null)
                        {
                            if (_litDataDeposito.Text.Trim() == "")
                            {
                                _btnImagens.Visible = false;
                                _btnReferencia.Visible = false;
                                _litEmAberto.Visible = false;
                                _btnPendenteAvancar.Visible = false;
                                _btnPendenteRetornar.Visible = false;
                                _btnPendenteRemover.Visible = false;
                                _btnExcluir.Visible = false;
                                _btnObs.Visible = false;
                                _btnFecharObs.Visible = false;
                                _litLblObs.Visible = false;
                                _litSep1.Visible = false;
                                _litSep2.Visible = false;
                                _litSep3.Visible = false;
                                _litSep4.Visible = false;

                                e.Row.BackColor = Color.LightYellow;
                            }
                            else
                            {
                                if (_litHoraDeposito != null)
                                {
                                    _litDataDeposito.Text = "<font size='2' face='Calibri'>" + _litDataDeposito.Text.Trim() + "</font>";
                                    _litHoraDeposito.Text = "<font size='2' face='Calibri'>" + _litHoraDeposito.Text.Trim() + "</font>";
                                }

                                if (_litDTsReferencia != null)
                                {
                                    if (Convert.ToInt16(_litDTsReferencia.Text.Trim().Length) <= 20)
                                    {
                                        _litDTsReferencia.Text = "(" + _litDTsReferencia.Text.Trim() + ")";
                                    }
                                    else
                                    {
                                        _litDTsReferencia.Text = "<font size='1' face='Calibri'>(" + _litDTsReferencia.Text.Trim() + ")</font>";
                                    }
                                }

                                if (_litBanco != null)
                                {
                                    if (_litBanco.Text.Trim() == "")
                                    {
                                        _litSep1.Visible = false;
                                        _litSep2.Visible = false;
                                    }

                                    _litBanco.Text = "<font size='1' face='Calibri'>" + _litBanco.Text.Trim() + "</font>";
                                    _litAgencia.Text = "<font size='1' face='Calibri'>" + _litAgencia.Text.Trim() + "</font>";
                                    _litContaCorrente.Text = "<font size='1' face='Calibri'>" + _litContaCorrente.Text.Trim() + "</font>";
                                    _litSep1.Text = "<font size='1' face='Calibri'>" + _litSep1.Text.Trim() + "</font>";
                                    _litSep2.Text = "<font size='1' face='Calibri'>" + _litSep2.Text.Trim() + "</font>";
                                }

                                if (_litImagens != null && _btnImagens != null)
                                {
                                    _btnImagens.Text = _litImagens.Text;
                                    if (_litImagens.Text == "0")
                                    {
                                        _btnImagens.Enabled = false;
                                    }
                                    _litImagens.Visible = false;
                                }

                                if (_litLancamentos != null && _litDataLancamento != null && _litObs != null && _litEmAberto != null && _litConferido != null && _btnPendenteAvancar != null)
                                {
                                    if (_litLancamentos.Text.Replace(",", "").Trim() == "")
                                    {
                                        _litLancamentos.Visible = false;
                                        _litDataLancamento.Visible = false;
                                        _btnObs.Visible = false;
                                        _btnFecharObs.Visible = false;
                                        _litObs.Visible = false;
                                        _litLblObs.Visible = false;
                                        _litSep3.Visible = false;
                                        _litConferido.Visible = false;
                                        _litSep4.Visible = false;
                                        _litEmAberto.Visible = true;
                                        e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Left;
                                        _litLancamentos.Text = "<font size='1' face='Calibri'>" + _litLancamentos.Text.Trim() + "</font>";
                                        _litDataLancamento.Text = "<font size='1' face='Calibri'>" + _litDataLancamento.Text.Trim() + "</font>";

                                        if (_litObs.Text.Trim() == "")
                                        {
                                            _btnObs.Visible = false;
                                        }
                                        _litObs.Visible = false;
                                        _litLblObs.Visible = false;
                                        _btnFecharObs.Visible = false;
                                        _litEmAberto.Visible = false;
                                        _btnExcluir.Visible = false;

                                        if (_litConferido.Text.Trim() == "")
                                        {
                                            _litSep4.Visible = false;
                                            _litConferido.Visible = false;
                                        }
                                        else
                                        {
                                            _litConferido.Text = "<font size='1' face='Calibri'>" + _litConferido.Text.Trim() + "</font>";
                                        }
                                    }
                                }

                                if (_litDataPendente.Text.Trim() == "")
                                {
                                    if (_litLancamentos.Text.Replace(",", "").Trim() == "")
                                    {
                                        _btnPendenteAvancar.Visible = true;
                                    }
                                    else
                                    {
                                        _btnPendenteAvancar.Visible = false;
                                    }
                                    _btnPendenteRetornar.Visible = false;
                                    _btnPendenteRemover.Visible = false;
                                    _litDataPendente.Visible = false;
                                }
                                else
                                {
                                    e.Row.Cells[1].ForeColor = Color.Red;
                                    _litDataPendente.Visible = true;
                                    _litDataPendente.Text = "<font size='1' face='Calibri'>" + _litDataPendente.Text.Trim() + "</font>";
                                    _btnPendenteRemover.Visible = true;
                                    _btnPendenteRemover.Font.Bold = true;

                                    if (_litDataDeposito.Text.IndexOf(CalendarDeposito.SelectedDate.ToShortDateString().Trim(), 0, _litDataDeposito.Text.Length) >= 0)
                                    {
                                        _btnPendenteAvancar.Visible = false;
                                        _btnPendenteRetornar.Visible = false;
                                    }
                                    else if (_litDataDeposito.Text.IndexOf(CalendarDeposito.SelectedDate.AddDays(-1).ToShortDateString().Trim(), 0, _litDataDeposito.Text.Length) >= 0)
                                    {
                                        _btnPendenteAvancar.Visible = true;
                                        _btnPendenteRetornar.Visible = false;
                                    }
                                    else
                                    {
                                        _btnPendenteAvancar.Visible = true;
                                        _btnPendenteRetornar.Visible = true;
                                    }
                                }

                                Literal _litValorDepositado = e.Row.FindControl("litValorDepositado") as Literal;
                                if (_litValorDepositado != null)
                                {
                                    if (Convert.ToDecimal(_litValorDepositado.Text) > 0)
                                    {
                                        dTotalDepositado += (string.IsNullOrEmpty(_litValorDepositado.Text.ToString().Trim()) ? 0 : Convert.ToDecimal(_litValorDepositado.Text));
                                        iTotalDepositado += 1;
                                    }
                                }

                                if (hdnCodigoDeposito_Consultas.Value.ToString().Trim() != "")
                                {
                                    if (Convert.ToInt32(_hdnCodigoDepositogrd.Value) == Convert.ToInt32(hdnCodigoDeposito_Consultas.Value))
                                    {
                                        e.Row.BackColor = Color.AliceBlue;

                                        hdnCodigoDeposito_Consultas.Value = String.Empty;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void mtd_gvDepositos_Buscar_Referencia(GridViewRow dgItem)
        {
            desmarcarRow(-1);
            marcarRow(dgItem);

            HiddenField _hdnCodigoSaldo = (HiddenField)(dgItem.FindControl("hdnCodigoSaldo"));
            Literal _litFilial = (Literal)(dgItem.FindControl("litFilial"));
            Literal _litDataDeposito = (Literal)(dgItem.FindControl("litDataDeposito"));
            Literal _litHoraDeposito = (Literal)(dgItem.FindControl("litHoraDeposito"));
            Literal _litDataBanco = (Literal)(dgItem.FindControl("litDataBanco"));
            Literal _litLancamentos = (Literal)(dgItem.FindControl("litLancamentos"));
            Literal _litValorDepositado = (Literal)(dgItem.FindControl("litValorDepositado"));

            panReferencia.Visible = true;
            panPrincipal.Enabled = false;

            lblRefFilial.Text = _litFilial.Text;
            lblRefDataDeposito.Text = _litDataDeposito.Text + " " + _litHoraDeposito.Text;
            lblRefDataBanco.Text = _litDataBanco.Text;
            lblRefValor.Text = _litValorDepositado.Text;

            lblRefFilial.Text = "<font size='2' face='Calibri'><B>" + lblRefFilial.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";
            lblRefDataDeposito.Text = "<font size='2' face='Calibri'><B>" + lblRefDataDeposito.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";
            lblRefDataBanco.Text = "<font size='2' face='Calibri'><B>" + lblRefDataBanco.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";
            lblRefValor.Text = "<font size='2' face='Calibri'><B>" + lblRefValor.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";

            CarregarFechamentosDeposito(Convert.ToInt32(_hdnCodigoSaldo.Value));
        }

        private void mtd_gvDepositos_Buscar_Imagens(GridViewRow dgItem, Int32 iIndex)
        {
            desmarcarRow(-1);
            marcarRow(dgItem);

            hdnLinha.Value = iIndex.ToString();

            HiddenField _hdnCodigoDeposito = (HiddenField)(dgItem.FindControl("hdnCodigoDepositogrd"));
            HiddenField _hdnCodigoSaldo = (HiddenField)(dgItem.FindControl("hdnCodigoSaldo"));
            HiddenField _hdnFilial = (HiddenField)(dgItem.FindControl("hdnFilial"));
            Literal _litFilial = (Literal)(dgItem.FindControl("litFilial"));
            Literal _litDataDeposito = (Literal)(dgItem.FindControl("litDataDeposito"));
            Literal _litHoraDeposito = (Literal)(dgItem.FindControl("litHoraDeposito"));
            Literal _litDataBanco = (Literal)(dgItem.FindControl("litDataBanco"));
            Literal _litLancamentos = (Literal)(dgItem.FindControl("litLancamentos"));
            Literal _litValorDepositado = (Literal)(dgItem.FindControl("litValorDepositado"));
            Literal _litEmAberto = (Literal)(dgItem.FindControl("litEmAberto"));
            Literal _litObs = (Literal)(dgItem.FindControl("litObs"));

            panImagens.Visible = true;
            panPrincipal.Enabled = false;

            lblImgFilial.Text = _litFilial.Text;
            lblImgDataDeposito.Text = _litDataDeposito.Text + " " + _litHoraDeposito.Text;
            lblImgDataBanco.Text = _litDataBanco.Text;
            lblImgValor.Text = _litValorDepositado.Text;
            hdnImgCodFilial.Value = _hdnFilial.Value;

            lblImgFilial.Text = "<font size='2' face='Calibri'><B>" + lblImgFilial.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";
            lblImgDataDeposito.Text = "<font size='2' face='Calibri'><B>" + lblImgDataDeposito.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";
            lblImgDataBanco.Text = "<font size='2' face='Calibri'><B>" + lblImgDataBanco.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";
            lblImgValor.Text = "<font size='2' face='Calibri'><B>" + lblImgValor.Text.Trim().Replace("<font size='1' face='Calibri'>", "").Replace("</font>", "") + "</B></font>";

            hdnCodigoDeposito_Consultas.Value = _hdnCodigoDeposito.Value;
            hdnCodigoFilial_Consultas.Value = _hdnFilial.Value;
            hdnDataDeposito_Consultas.Value = _litDataDeposito.Text.Replace("<font size='2' face='Calibri'>", "").Replace("</font>", "");

            CarregarImagensDocumento(Convert.ToInt32(_hdnCodigoDeposito.Value));
            CarregarImagensDeposito(Convert.ToInt32(_hdnFilial.Value), Convert.ToDateTime(hdnDataDeposito_Consultas.Value.ToString()));
            CarregaGridViewDocumento(Convert.ToInt32(_hdnCodigoDeposito.Value));
            CarregarFechamentosDeposito(Convert.ToInt32(_hdnCodigoSaldo.Value));

            if (_litEmAberto.Visible == true)
            {
                panImagensVinculadasNaoLancadas.Visible = true;
                panImagensVinculadasLancadas.Visible = false;

                btnGravarLancamento.Visible = true;
                txtValorReal.Text = "";
                txtValorReal.ReadOnly = false;
                txtObs.Text = "";
                txtObs.ReadOnly = false;
                txtObs.Height = 107;

                btSalvarImagemAvulsa.Visible = true;
            }
            else
            {
                panImagensVinculadasNaoLancadas.Visible = false;
                panImagensVinculadasLancadas.Visible = true;

                btnGravarLancamento.Visible = false;
                txtValorReal.Text = _litValorDepositado.Text;
                txtValorReal.ReadOnly = true;
                txtObs.Text = _litObs.Text;
                txtObs.ReadOnly = true;
                txtObs.Height = 127;

                btSalvarImagemAvulsa.Visible = false;
            }

            List<DEPOSITO_BANCO> _list = new List<DEPOSITO_BANCO>();
            List<DEPOSITO_BANCO> _listAux = new List<DEPOSITO_BANCO>();

            _list = baseController.BuscarDadosBanco();

            foreach (DEPOSITO_BANCO d in _list)
            {
                _listAux.Add(new DEPOSITO_BANCO
                {
                    CODIGO_BANCO = d.CODIGO_BANCO,
                    NOME = d.NOME + " - " + d.CONTA_CTB
                });
            }

            ddlCCImgAvulsa.DataSource = _listAux;
            ddlCCImgAvulsa.DataBind();

            //Carregar banco da filial
            var _depositoBanco = baseController.BuscarBancoPorFilial(_hdnFilial.Value.ToString());
            if (_depositoBanco != null)
            {
                ddlCCImgAvulsa.SelectedValue = _depositoBanco.CODIGO_BANCO.ToString();
            }

            txtDiferenca.Text = (dValorTotalImagensLancadas - dValorParaDeposito).ToString("N2");
        }
        private void CarregaGridViewDocumento(int codigoDeposito)
        {
            gvImagensLancadas.DataSource = usuarioController.BuscaDocumentos(codigoDeposito);
            gvImagensLancadas.DataBind();
        }

        private void mtd_gvDepositos_Ver_Obs(GridViewRow dgItem)
        {
            desmarcarRow(-1);
            marcarRow(dgItem);

            Button _btnObs = (Button)(dgItem.FindControl("btnObs"));
            Button _btnFecharObs = (Button)(dgItem.FindControl("btnFecharObs"));
            Literal _litLblObs = (Literal)(dgItem.FindControl("litLblObs"));
            Literal _litObs = (Literal)(dgItem.FindControl("litObs"));
            _litLblObs.Visible = true;
            _litObs.Visible = true;
            _litObs.Text = "<font size='1' face='Calibri'>" + _litObs.Text.Trim() + "</font>";
            _litLblObs.Text = "<br />" + ("<br /><font size='1' face='Calibri'>" + _litLblObs.Text.Trim() + "</font>").Replace("<br />", "");
            _btnObs.Visible = false;
            _btnFecharObs.Visible = true;
        }

        private void mtd_gvDepositos_Fechar_Obs(GridViewRow dgItem)
        {
            Button _btnObs = (Button)(dgItem.FindControl("btnObs"));
            Button _btnFecharObs = (Button)(dgItem.FindControl("btnFecharObs"));
            Literal _litLblObs = (Literal)(dgItem.FindControl("litLblObs"));
            Literal _litObs = (Literal)(dgItem.FindControl("litObs"));
            _litLblObs.Visible = false;
            _litObs.Visible = false;
            _btnObs.Visible = true;
            _btnFecharObs.Visible = false;
        }

        private void bloquearControles(GridViewRow dgItem, Boolean OK)
        {
            Button _btnReferencia = (Button)(dgItem.FindControl("btnReferencia"));
            Button _btnImagens = (Button)(dgItem.FindControl("btnImagens"));
            Button _btnLancar = (Button)(dgItem.FindControl("btnLancar"));
            Button _btnExcluir = (Button)(dgItem.FindControl("btnExcluir"));

            if (_btnReferencia != null)
            {
                _btnReferencia.Visible = OK;
            }

            if (_btnImagens != null)
            {
                _btnImagens.Visible = OK;
            }

            if (_btnLancar != null)
            {
                _btnLancar.Visible = OK;
            }

            if (_btnExcluir != null)
            {
                _btnExcluir.Visible = OK;
            }
        }

        private void mtd_gvDepositos_Deposito_Pendente_Avancar(GridViewRow dgItem)
        {
            DEPOSITO_PENDENTE_LANCAMENTO deposito = new DEPOSITO_PENDENTE_LANCAMENTO();

            HiddenField _hdnCodigoDepositogrd = (HiddenField)(dgItem.FindControl("hdnCodigoDepositogrd"));
            baseController.ExcluirDepositoPendenteLancamento(Convert.ToInt32(_hdnCodigoDepositogrd.Value));

            Button _btnPendenteAvancar = (Button)(dgItem.FindControl("btnPendenteAvancar"));
            Button _btnPendenteRetornar = (Button)(dgItem.FindControl("btnPendenteRetornar"));
            Button _btnPendenteRemover = (Button)(dgItem.FindControl("btnPendenteRemover"));
            Literal _litDataPendente = (Literal)(dgItem.FindControl("litDataPendente"));

            deposito.CODIGO_DEPOSITO = Convert.ToInt32(_hdnCodigoDepositogrd.Value);
            deposito.DATA_PENDENTE = CalendarDeposito.SelectedDate.AddDays(1);
            baseController.IncluirDepositoPendenteLancamento(deposito);

            _litDataPendente.Text = "<font size='1' face='Calibri'>" + String.Format("{0:dd/MM/yyyy}", deposito.DATA_PENDENTE.ToString().Replace("00:00:00", "").Trim()) + "</font>";
            _litDataPendente.Visible = true;
            _btnPendenteRemover.Visible = true;
            _btnPendenteAvancar.Visible = false;
            _btnPendenteRetornar.Visible = false;
            dgItem.Cells[1].ForeColor = Color.Empty;
            dgItem.BackColor = Color.Red;
            bloquearControles(dgItem, false);
        }

        private void mtd_gvDepositos_Deposito_Pendente_Retornar(GridViewRow dgItem)
        {
            HiddenField _hdnCodigoDepositogrd = (HiddenField)(dgItem.FindControl("hdnCodigoDepositogrd"));
            baseController.ExcluirDepositoPendenteLancamento(Convert.ToInt32(_hdnCodigoDepositogrd.Value));

            Button _btnPendenteAvancar = (Button)(dgItem.FindControl("btnPendenteAvancar"));
            Button _btnPendenteRetornar = (Button)(dgItem.FindControl("btnPendenteRetornar"));
            Button _btnPendenteRemover = (Button)(dgItem.FindControl("btnPendenteRemover"));
            Literal _litDataPendente = (Literal)(dgItem.FindControl("litDataPendente"));
            Literal _litDataDeposito = (Literal)(dgItem.FindControl("litDataDeposito"));

            DEPOSITO_PENDENTE_LANCAMENTO deposito = new DEPOSITO_PENDENTE_LANCAMENTO();

            deposito.CODIGO_DEPOSITO = Convert.ToInt32(_hdnCodigoDepositogrd.Value);
            deposito.DATA_PENDENTE = CalendarDeposito.SelectedDate.AddDays(-1);
            baseController.IncluirDepositoPendenteLancamento(deposito);

            _litDataPendente.Text = "<font size='1' face='Calibri'>" + String.Format("{0:dd/MM/yyyy}", deposito.DATA_PENDENTE.ToString().Replace("00:00:00", "").Trim()) + "</font>";
            _litDataPendente.Visible = true;
            _btnPendenteAvancar.Visible = false;
            _btnPendenteRetornar.Visible = false;
            _btnPendenteRemover.Visible = true;
            dgItem.Cells[1].ForeColor = Color.Empty;
            dgItem.BackColor = Color.Red;
            bloquearControles(dgItem, false);
        }

        private void mtd_gvDepositos_Deposito_Pendente_Remover(GridViewRow dgItem)
        {
            DEPOSITO_PENDENTE_LANCAMENTO deposito = new DEPOSITO_PENDENTE_LANCAMENTO();

            HiddenField _hdnCodigoDepositogrd = (HiddenField)(dgItem.FindControl("hdnCodigoDepositogrd"));

            Button _btnPendenteAvancar = (Button)(dgItem.FindControl("btnPendenteAvancar"));
            Button _btnPendenteRetornar = (Button)(dgItem.FindControl("btnPendenteRetornar"));
            Button _btnPendenteRemover = (Button)(dgItem.FindControl("btnPendenteRemover"));
            Literal _litDataPendente = (Literal)(dgItem.FindControl("litDataPendente"));
            Literal _litDataDeposito = (Literal)(dgItem.FindControl("litDataDeposito"));

            baseController.ExcluirDepositoPendenteLancamento(Convert.ToInt32(_hdnCodigoDepositogrd.Value));

            _litDataPendente.Text = "";
            _litDataPendente.Visible = false;
            _btnPendenteRemover.Visible = false;

            //Mesmo Dia
            if (_litDataDeposito.Text.IndexOf(CalendarDeposito.SelectedDate.ToShortDateString().Trim(), 0, _litDataDeposito.Text.Length) >= 0)
            {
                _btnPendenteAvancar.Visible = true;
                _btnPendenteRetornar.Visible = false;
                dgItem.Cells[1].ForeColor = Color.Empty;
                dgItem.BackColor = Color.Empty;
                bloquearControles(dgItem, true);
            }//Dia Depósito Anterior ao Dia Pendente
            else if (_litDataDeposito.Text.IndexOf(CalendarDeposito.SelectedDate.AddDays(-1).ToShortDateString().Trim(), 0, _litDataDeposito.Text.Length) >= 0)
            {
                _btnPendenteAvancar.Visible = false;
                _btnPendenteRetornar.Visible = false;
                dgItem.Cells[1].ForeColor = Color.Empty;
                dgItem.BackColor = Color.Red;
                bloquearControles(dgItem, false);
            }
            else//Dias Posteriores ao Dia Pendente
            {
                _btnPendenteAvancar.Visible = false;
                _btnPendenteRetornar.Visible = false;
                dgItem.Cells[1].ForeColor = Color.Empty;
                dgItem.BackColor = Color.Red;
                bloquearControles(dgItem, false);
            }
        }

        private void mtd_gvDepositos_Excluir_Deposito(GridViewRow dgItem)
        {
            desmarcarRow(-1);
            marcarRow(dgItem);

            Panel _panExluir = (Panel)(dgItem.FindControl("panExluir"));
            Button _btnExcluir = (Button)(dgItem.FindControl("btnExcluir"));

            Button _btnExcluirDeposito = (Button)(dgItem.FindControl("btnExcluirDeposito"));
            if (_btnExcluirDeposito != null)
            {
                _btnExcluirDeposito.Attributes.Add("OnClick", "AbrirAguarde();");
            }

            _btnExcluir.Visible = false;

            _panExluir.Visible = true;
        }

        private void mtd_gvDepositos_Excluir_Deposito_Confirmar(GridViewRow dgItem)
        {
            HiddenField _hdnCodigoDepositogrd = (HiddenField)(dgItem.FindControl("hdnCodigoDepositogrd"));
            Panel _panExluir = (Panel)(dgItem.FindControl("panExluir"));
            Button _btnExcluir = (Button)(dgItem.FindControl("btnExcluir"));
            TextBox _txtMotivoExclusao = (TextBox)(dgItem.FindControl("txtMotivoExclusao"));

            Button _btnPendenteAvancar = (Button)(dgItem.FindControl("btnPendenteAvancar"));
            Button _btnPendenteRetornar = (Button)(dgItem.FindControl("btnPendenteRetornar"));
            Button _btnPendenteRemover = (Button)(dgItem.FindControl("btnPendenteRemover"));

            Panel _panProcessando = (Panel)(dgItem.FindControl("panProcessando"));

            if (_txtMotivoExclusao != null)
            {
                if (_txtMotivoExclusao.Text.Trim() == "")
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Informe o motivo da exclusão deste depósito.";
                    txtMensagemErro.Focus();
                    return;
                }
                if (_txtMotivoExclusao.Text.Trim().Length <= 5)
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Informe um motivo de exclusão do depósito com pelo menos 5 caracteres.";
                    txtMensagemErro.Focus();
                    return;
                }
            }

            try
            {
                DEPOSITO_SEMANA depositoSelecionado = new DEPOSITO_SEMANA();
                DEPOSITO_SEMANA_LOG depositoLOG = new DEPOSITO_SEMANA_LOG();
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                depositoSelecionado = baseController.BuscaDepositoSemana(Convert.ToInt32(_hdnCodigoDepositogrd.Value));

                depositoLOG.DATA_LOG = DateTime.Now;
                depositoLOG.USUARIO_LOG = usuario.NOME_USUARIO;
                depositoLOG.MOTIVO = _txtMotivoExclusao.Text;
                depositoLOG.CODIGO_DEPOSITO = depositoSelecionado.CODIGO_DEPOSITO;
                depositoLOG.ANO_SEMANA = depositoSelecionado.ANO_SEMANA;
                depositoLOG.DATA_INICIO = depositoSelecionado.DATA_INICIO;
                depositoLOG.DATA_FIM = depositoSelecionado.DATA_FIM;
                depositoLOG.DATA_DEPOSITO = depositoSelecionado.DATA_DEPOSITO;
                depositoLOG.VALOR_A_DEPOSITAR = depositoSelecionado.VALOR_A_DEPOSITAR;
                depositoLOG.VALOR_DEPOSITADO = depositoSelecionado.VALOR_DEPOSITADO;
                depositoLOG.DIFERENCA = depositoSelecionado.DIFERENCA;
                depositoLOG.COMPROVANTE = depositoSelecionado.COMPROVANTE;
                depositoLOG.ASSINATURA = depositoSelecionado.ASSINATURA;
                depositoLOG.BANCO = depositoSelecionado.BANCO;
                depositoLOG.AGENCIA = depositoSelecionado.AGENCIA;
                depositoLOG.CONTA = depositoSelecionado.CONTA;
                depositoLOG.CODIGO_FILIAL = depositoSelecionado.CODIGO_FILIAL;
                depositoLOG.TIPO_DEPOSITO = depositoSelecionado.TIPO_DEPOSITO;
                depositoLOG.OBS = depositoSelecionado.OBS;
                depositoLOG.SALDO_INICIAL = depositoSelecionado.SALDO_INICIAL;
                depositoLOG.SALDO_FINAL = depositoSelecionado.SALDO_FINAL;
                depositoLOG.CODIGO_BAIXA = depositoSelecionado.CODIGO_BAIXA;

                baseController.IncluirDepositoLOG(depositoLOG);

                SP_OBTER_CODIGO_SALDO_DO_DEPOSITOResult codigoSaldo = new SP_OBTER_CODIGO_SALDO_DO_DEPOSITOResult();
                codigoSaldo = baseController.ObterCodigoSaldoDoDeposito(depositoSelecionado.CODIGO_FILIAL.ToString(), depositoSelecionado.DATA_DEPOSITO.ToShortDateString(), depositoSelecionado.CODIGO_DEPOSITO);

                if (codigoSaldo != null)
                {
                    baseController.AtualizaCodigoSaldoHistoricoDepositos(codigoSaldo.CODIGO_SALDO);
                    baseController.ExcluirDepositoSaldo(codigoSaldo.CODIGO_SALDO);
                }

                baseController.ExcluirDepositoBanco(depositoSelecionado);

                _btnExcluir.Visible = false;
                _panExluir.Visible = false;
                dgItem.Cells[1].ForeColor = Color.Empty;
                dgItem.BackColor = Color.Red;
                bloquearControles(dgItem, false);
                _btnPendenteAvancar.Visible = false;
                _btnPendenteRetornar.Visible = false;
                _btnPendenteRemover.Visible = false;
                dgItem.Cells[12].Text = "<font size='2' face='Calibri' color='white'><b>EXCLUÍDO</b></font>";

                System.Threading.Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
            finally
            {
                panProcessando.Visible = false;
            }
        }

        private void mtd_gvDepositos_Excluir_Deposito_Cancelar(GridViewRow dgItem)
        {
            Panel _panExluir = (Panel)(dgItem.FindControl("panExluir"));
            TextBox _txtMotivoExclusao = (TextBox)(dgItem.FindControl("txtMotivoExclusao"));

            Button _btnExcluir = (Button)(dgItem.FindControl("btnExcluir"));

            _txtMotivoExclusao.Text = "";
            _panExluir.Visible = false;
            _btnExcluir.Visible = true;
        }

        protected void gvDepositos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 iIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow dgItem = gvDepositos.Rows[iIndex];

            if (e.CommandName == "Buscar_Referencia")
            {
                mtd_gvDepositos_Buscar_Referencia(dgItem);
            }
            else if (e.CommandName == "Buscar_Imagens")
            {
                mtd_gvDepositos_Buscar_Imagens(dgItem, iIndex);
            }
            else if (e.CommandName == "Ver_Obs")
            {
                mtd_gvDepositos_Ver_Obs(dgItem);
            }
            else if (e.CommandName == "Fechar_Obs")
            {
                mtd_gvDepositos_Fechar_Obs(dgItem);
            }
            else if (e.CommandName == "Deposito_Pendente_Avancar")
            {
                mtd_gvDepositos_Deposito_Pendente_Avancar(dgItem);
            }
            else if (e.CommandName == "Deposito_Pendente_Retornar")
            {
                mtd_gvDepositos_Deposito_Pendente_Retornar(dgItem);
            }
            else if (e.CommandName == "Deposito_Pendente_Remover")
            {
                mtd_gvDepositos_Deposito_Pendente_Remover(dgItem);
            }
            else if (e.CommandName == "Excluir_Deposito")
            {
                mtd_gvDepositos_Excluir_Deposito(dgItem);
            }
            else if (e.CommandName == "Excluir_Deposito_Confirmar")
            {
                mtd_gvDepositos_Excluir_Deposito_Confirmar(dgItem);
            }
            else if (e.CommandName == "Excluir_Deposito_Cancelar")
            {
                mtd_gvDepositos_Excluir_Deposito_Cancelar(dgItem);
            }
        }

        private void marcarRow(GridViewRow dgItem)
        {
            dgItem.BackColor = Color.AliceBlue;
        }

        private void desmarcarRow(Int32 iLinha)
        {
            foreach (GridViewRow gvItem in gvDepositos.Rows)
            {
                Literal _litDataDeposito = (Literal)gvItem.FindControl("litDataDeposito");

                if (gvItem.RowIndex != iLinha)
                {
                    if (_litDataDeposito != null)
                    {
                        if (_litDataDeposito.Text.Trim() == "")
                        {
                            gvItem.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            gvItem.BackColor = Color.Empty;
                        }
                    }
                }
                else
                {
                    gvItem.BackColor = Color.AliceBlue;
                }
            }
        }

        protected void btnFecharReferencia_Click(object sender, EventArgs e)
        {
            panReferencia.Visible = false;
            panPrincipal.Enabled = true;
            //desmarcarRow(-1);
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Insert(0, new ListItem("Todos", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void ddlBanco_DataBound(object sender, EventArgs e)
        {
            ddlBanco.Items.Insert(0, new ListItem("Todos", "0"));
            ddlBanco.SelectedValue = "0";
        }

        protected void ddlContaCorrente_DataBound(object sender, EventArgs e)
        {
            ddlContaCorrente.Items.Insert(0, new ListItem("Todos", "0"));
            ddlContaCorrente.SelectedValue = "0";
        }

        protected void ddlBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sNomeBanco;

            if (ddlBanco.SelectedValue.ToString() == "0")
            {
                sNomeBanco = "";
            }
            else
            {
                sNomeBanco = ddlBanco.SelectedItem.ToString();
            }

            CarregaDropDownListContaCorrente(sNomeBanco);
        }

        protected void ddlStatus_DataBound(object sender, EventArgs e)
        {
            ddlStatus.Items.Add(new ListItem("Todos", "0"));
            ddlStatus.SelectedValue = "0";
        }

        protected void gvReferencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvReferencia_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvReferencia.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].ColumnSpan = 2;
                footer.Cells[0].Text = "Localizado  " + Convert.ToString(gvReferencia.Rows.Count) + "  Fechamentos";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[2].Text = "Total:";
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = dValorParaDeposito.ToString("N2");
                footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void gvReferencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        Literal _litDataFechamento = e.Row.FindControl("litDataFechamento") as Literal;

                        if (_litDataFechamento != null)
                        {
                            _litDataFechamento.Text = String.Format("{0:dd/MM/yyyy}", _litDataFechamento.Text.Trim()).Replace(" 00:00:00", "");
                        }

                        Literal _litValorDeposito = e.Row.FindControl("litValorDeposito") as Literal;
                        if (_litValorDeposito != null)
                        {
                            if (Convert.ToDecimal(_litValorDeposito.Text) > 0)
                            {
                                dValorParaDeposito += (string.IsNullOrEmpty(_litValorDeposito.Text.ToString().Trim()) ? 0 : Convert.ToDecimal(_litValorDeposito.Text));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void btnFecharImagens_Click(object sender, EventArgs e)
        {
            GridViewRow row = gvDepositos.Rows[Convert.ToInt32(hdnLinha.Value)];

            Button _btnImagens = (Button)row.FindControl("btnImagens");
            if (_btnImagens != null)
            {
                _btnImagens.Text = gvImagensVinculadas.Rows.Count.ToString().Trim() + " - " + gvImagensDeposito.Rows.Count.ToString().Trim();
            }

            panImagens.Visible = false;
            panPrincipal.Enabled = true;

            if (gvImagensVinculadas.Rows.Count == 0)
            {
                try
                {
                    FiltroDepositos filtro = new FiltroDepositos();

                    filtro = validarFiltros();

                    if (filtro.OK == true)
                    {
                        CarregarDepositos(filtro);
                    }

                    panImagens.Visible = false;
                }
                catch (Exception ex)
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
                }
            }
        }

        protected void CalendarDtLctoImgAvulsa_SelectionChanged(object sender, EventArgs e)
        {
            txtDtLctoImgAvulsa.Text = CalendarDtLctoImgAvulsa.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btSalvarImagemAvulsa_Click(object sender, EventArgs e)
        {
            DEPOSITO_DOCUMENTO depositoDocumento = new DEPOSITO_DOCUMENTO();
            lblErroImg.Text = "";

            try
            {
                if (FileUpload1.HasFile)
                {
                    depositoDocumento.CAMINHO_IMAGEM = System.IO.Path.GetDirectoryName(FileUpload1.PostedFile.FileName);
                    depositoDocumento.NOME_IMAGEM = FileUpload1.FileName;

                    depositoDocumento.CODIGO_DEPOSITO = Convert.ToInt32(hdnCodigoDeposito_Consultas.Value);
                }
                else
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Selecione uma imagem.";
                    return;
                }

                if (!txtValorImgAvulsa.Text.Equals(""))
                {
                    depositoDocumento.VALOR = Convert.ToDecimal(txtValorImgAvulsa.Text);
                }
                else
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Informe o valor do lançamento.";
                    return;
                }

                depositoDocumento.CODIGO_BANCO = Convert.ToInt32(ddlCCImgAvulsa.SelectedValue);
                if (!txtDtLctoImgAvulsa.Text.Equals(""))
                {
                    depositoDocumento.DATA_LANCAMENTO = Convert.ToDateTime(txtDtLctoImgAvulsa.Text);
                }
                else
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Informe a data de lançamento.";
                    return;
                }

                usuarioController.InsereDocumento(depositoDocumento);

                CarregarImagensDocumento(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

                if (FileUpload1.PostedFile.ContentType == "image/jpeg" || FileUpload1.PostedFile.ContentType == "image/pjpeg")
                {
                    if (FileUpload1.PostedFile.ContentLength < 500000)
                    {
                        string savePath = Server.MapPath("~/Upload/");

                        String fileName = FileUpload1.FileName;

                        savePath += fileName;

                        FileUpload1.SaveAs(savePath);
                    }
                    else
                    {
                        panMensagem.Visible = true;
                        txtMensagemErro.Text = "Arquivo não carregado -> maior que 500 kb !!!";
                        return;
                    }
                }
                else
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Arquivo não carregado -> não é uma Imagem !!!";
                    return;
                }

                txtValorImgAvulsa.Text = string.Empty;

                recalcularValorReal(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

                lblErroImg.Text = "Gravado com sucesso!";
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        private void mtd_gvImagensVinculadas_Excluir_Imagem(GridViewRow dgItem)
        {
            SP_INTRANET_ESTORNA_LANCAMENTO_LINXResult sp = null;
            HiddenField _hdnCodigoDocumento = (HiddenField)(dgItem.FindControl("hdnCodigoDocumento"));
            int _numeroLancamento = 0;

            var _depositoDocumento = usuarioController.BuscaDepositoDocumento(Convert.ToInt32(_hdnCodigoDocumento.Value));

            if (_depositoDocumento != null)
                if (_depositoDocumento.NUMERO_LANCAMENTO != null)
                    _numeroLancamento = (int)_depositoDocumento.NUMERO_LANCAMENTO;

            if (_numeroLancamento > 0)
            {
                //Chamar procedure de processamento contábil
                sp = baseController.ProcessarEstornoLancamentoContabil(1, _numeroLancamento);

                if (sp != null)
                {
                    if (sp.CODIGO_ERRO != 0)
                    {
                        panMensagem.Visible = true;
                        txtMensagemErro.Text = "Erro Procedure (1). Código: " + sp.CODIGO_ERRO.ToString() + " - Mensagem: " + sp.MENSAGEM_ERRO;
                        return;
                    }
                }
            }

            usuarioController.ExcluiDocumento(Convert.ToInt32(_hdnCodigoDocumento.Value));

            IMAGEM_VINCULO_DEPOSITO_DOCUMENTO imagemVinculo = new IMAGEM_VINCULO_DEPOSITO_DOCUMENTO();

            imagemVinculo = imagemController.BuscaImagemVinculoDepositoDocumento(0, Convert.ToInt32(_hdnCodigoDocumento.Value), Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

            if (imagemVinculo != null)
            {
                imagemController.ExcluiImagemVinculoDepositoDocumento(imagemVinculo.CODIGO_IMAGEM, imagemVinculo.CODIGO_DOCUMENTO, imagemVinculo.CODIGO_DEPOSITO);
            }

            CarregarImagensDocumento(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));
            CarregarImagensDeposito(Convert.ToInt32(hdnCodigoFilial_Consultas.Value), Convert.ToDateTime(hdnDataDeposito_Consultas.Value));
            CarregaGridViewDocumento(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

            recalcularValorReal(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));
        }

        private void mtd_gvImagensLancadas_Excluir_Imagem(GridViewRow dgItem)
        {
            SP_INTRANET_ESTORNA_LANCAMENTO_LINXResult sp = null;
            HiddenField _hdnCodigoDocumento = (HiddenField)(dgItem.FindControl("hdnCodigoDocumento"));
            int _numeroLancamento = 0;

            var _depositoDocumento = usuarioController.BuscaDepositoDocumento(Convert.ToInt32(_hdnCodigoDocumento.Value));

            if (_depositoDocumento != null)
                if (_depositoDocumento.NUMERO_LANCAMENTO != null)
                    _numeroLancamento = (int)_depositoDocumento.NUMERO_LANCAMENTO;

            if (_numeroLancamento > 0)
            {
                //Chamar procedure de processamento contábil
                sp = baseController.ProcessarEstornoLancamentoContabil(1, _numeroLancamento);

                if (sp != null)
                {
                    if (sp.CODIGO_ERRO != 0)
                    {
                        panMensagem.Visible = true;
                        txtMensagemErro.Text = "Erro Procedure (1). Código: " + sp.CODIGO_ERRO.ToString() + " - Mensagem: " + sp.MENSAGEM_ERRO;
                        return;
                    }
                }
            }

            usuarioController.ExcluiDocumento(Convert.ToInt32(_hdnCodigoDocumento.Value));

            IMAGEM_VINCULO_DEPOSITO_DOCUMENTO imagemVinculo = new IMAGEM_VINCULO_DEPOSITO_DOCUMENTO();

            imagemVinculo = imagemController.BuscaImagemVinculoDepositoDocumento(0, Convert.ToInt32(_hdnCodigoDocumento.Value), Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

            if (imagemVinculo != null)
            {
                imagemController.ExcluiImagemVinculoDepositoDocumento(imagemVinculo.CODIGO_IMAGEM, imagemVinculo.CODIGO_DOCUMENTO, imagemVinculo.CODIGO_DEPOSITO);
            }

            CarregarImagensDocumento(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));
            CarregarImagensDeposito(Convert.ToInt32(hdnCodigoFilial_Consultas.Value), Convert.ToDateTime(hdnDataDeposito_Consultas.Value));
            CarregaGridViewDocumento(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

            recalcularValorReal(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));
        }

        protected void gvImagensVinculadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 iIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow dgItem = gvImagensVinculadas.Rows[iIndex];

            if (e.CommandName == "Excluir_Imagem")
            {
                mtd_gvImagensVinculadas_Excluir_Imagem(dgItem);
            }
        }

        protected void gvImagensVinculadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        Literal _litVlrDepositadoImgVinculada = e.Row.FindControl("litVlrDepositadoImgVinculada") as Literal;
                        if (_litVlrDepositadoImgVinculada != null)
                        {
                            if (Convert.ToDecimal(_litVlrDepositadoImgVinculada.Text) > 0)
                            {
                                dValorTotalImagensVinculadas += (string.IsNullOrEmpty(_litVlrDepositadoImgVinculada.Text.ToString().Trim()) ? 0 : Convert.ToDecimal(_litVlrDepositadoImgVinculada.Text));
                            }
                        }

                        Literal _litDataLancamento = e.Row.FindControl("litDataLancamento") as Literal;
                        if (_litDataLancamento != null)
                        {
                            _litDataLancamento.Text = String.Format("{0:dd/MM/yyyy}", _litDataLancamento.Text.Trim()).Replace(" 00:00:00", "");
                        }

                        HiddenField _hdnCodigoBanco = e.Row.FindControl("hdnCodigoBanco") as HiddenField;
                        Literal _litBancoDepositado = e.Row.FindControl("litBancoDepositado") as Literal;
                        if (_litBancoDepositado != null)
                        {
                            var banco = baseController.BuscarDadosBanco(Convert.ToInt32(_hdnCodigoBanco.Value));
                            if (banco != null)
                                _litBancoDepositado.Text = banco.NOME + " - " + banco.CONTA_CTB;
                        }


                        HiddenField _hdnCaminhoImagem = e.Row.FindControl("hdnCaminhoImagem") as HiddenField;
                        Literal _litNomeImagemVinculada = e.Row.FindControl("litNomeImagemVinculada") as Literal;
                        HyperLink _linkNomeImagemVinculada = e.Row.FindControl("linkNomeImagemVinculada") as HyperLink;
                        HiddenField _hdnNomeImagemVinculada = e.Row.FindControl("hdnNomeImagemVinculada") as HiddenField;

                        if (_litNomeImagemVinculada != null && _linkNomeImagemVinculada != null && _hdnNomeImagemVinculada != null && _hdnCaminhoImagem != null)
                        {
                            FILIAIS_CONFIGURACAO nomeDiretorio = new FILIAIS_CONFIGURACAO();
                            nomeDiretorio = baseController.BuscarFiliaisConfiguracao(hdnCodigoFilial_Consultas.Value.ToString());

                            String arquivo = "";
                            String diretorio = "";

                            if (nomeDiretorio != null)
                            {
                                diretorio = nomeDiretorio.PASTA_IMAGENS_DEPOSITO.Trim() + "\\";
                            }
                            else
                            {
                                diretorio = "~/Upload/";
                            }

                            _litNomeImagemVinculada.Visible = false;

                            if (_hdnCaminhoImagem.Value.ToString().Trim() != "")
                            {
                                arquivo = diretorio.Trim() + _hdnCaminhoImagem.Value.ToString().Trim();

                                if (arquivoExiste(arquivo) == true)
                                {
                                    _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                }
                                else
                                {
                                    arquivo = "~/Upload/" + _hdnCaminhoImagem.Value.ToString().Trim();

                                    if (arquivoExiste(arquivo) == true)
                                    {
                                        _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                    }
                                    else
                                    {
                                        arquivo = "";
                                    }
                                }
                            }

                            if (_hdnNomeImagemVinculada.Value.ToString().Trim() != "" && arquivo == "")
                            {
                                arquivo = diretorio.Trim() + _hdnNomeImagemVinculada.Value.ToString().Trim();

                                if (arquivoExiste(arquivo) == true)
                                {
                                    _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                }
                                else
                                {
                                    arquivo = "~/Upload/" + _hdnNomeImagemVinculada.Value.ToString().Trim();

                                    if (arquivoExiste(arquivo) == true)
                                    {
                                        _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                    }
                                    else
                                    {
                                        _linkNomeImagemVinculada.Visible = false;
                                        _litNomeImagemVinculada.Visible = true;
                                    }
                                }
                            }

                            //if (_hdnCaminhoImagem.Value.ToString().Trim() == "")
                            //{
                            //    if (_hdnNomeImagemVinculada.Value.ToString().Trim() != "")
                            //    {
                            //        _linkNomeImagemVinculada.NavigateUrl = string.Format("~/Upload/{0}", _hdnNomeImagemVinculada.Value.ToString().Trim());
                            //    }
                            //}
                            //else
                            //{
                            //    _linkNomeImagemVinculada.NavigateUrl = string.Format("~/Upload/{0}", _hdnCaminhoImagem.Value.ToString().Trim());
                            //}

                            _linkNomeImagemVinculada.Text = "<font size='2' face='Calibri'>" + _linkNomeImagemVinculada.Text.Trim() + "</font>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void gvImagensVinculadas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvImagensVinculadas.FooterRow;

            if (footer != null)
            {
                footer.Cells[2].Text = dValorTotalImagensVinculadas.ToString("N2");
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
            }

            if (gvImagensVinculadas.Visible == true)
            {
                //txtValorReal.Text = dValorTotalImagensVinculadas.ToString("N2");
                //txtDiferenca.Text = (Convert.ToDecimal(lblImgValor.Text.Replace("<font size='2' face='Calibri'><B>", "").Replace("</B></font>", "")) - dValorTotalImagensVinculadas).ToString("N2");
                txtDiferenca.Text = (dValorTotalImagensVinculadas - Convert.ToDecimal(lblImgValor.Text.Replace("<font size='2' face='Calibri'><B>", "").Replace("</B></font>", ""))).ToString("N2");
            }
        }

        private void mtd_gvImagensDeposito_Vincular_Imagem(GridViewRow dgItem)
        {
            Panel _panVincular = (Panel)(dgItem.FindControl("panVincular"));

            Button _btnVincular = (Button)(dgItem.FindControl("btnVincular"));
            DropDownList _ddlddlCCVinculo = (DropDownList)(dgItem.FindControl("ddlCCVinculo"));
            TextBox _txtValLctoVinculo = (TextBox)(dgItem.FindControl("txtValLctoVinculo"));
            TextBox _txtDtLctoVinculo = (TextBox)(dgItem.FindControl("txtDtLctoVinculo"));

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + _txtDtLctoVinculo.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            _btnVincular.Visible = false;
            _panVincular.Visible = true;

            if (_txtValLctoVinculo != null)
            {
                if (_txtValLctoVinculo.Text.Trim() != "")
                {
                    _txtValLctoVinculo.Text = "";
                }
            }

            if (_txtDtLctoVinculo != null)
            {
                _txtDtLctoVinculo.Text = "";
            }

            List<DEPOSITO_BANCO> _list = new List<DEPOSITO_BANCO>();
            List<DEPOSITO_BANCO> _listAux = new List<DEPOSITO_BANCO>();

            _list = baseController.BuscarDadosBanco();

            foreach (DEPOSITO_BANCO d in _list)
            {
                _listAux.Add(new DEPOSITO_BANCO
                {
                    CODIGO_BANCO = d.CODIGO_BANCO,
                    NOME = d.NOME + " - " + d.CONTA_CTB
                });
            }

            _ddlddlCCVinculo.DataSource = _listAux;
            _ddlddlCCVinculo.DataBind();

            //Carregar banco da filial
            var _depositoBanco = baseController.BuscarBancoPorFilial(hdnImgCodFilial.Value.ToString());
            if (_depositoBanco != null)
            {
                _ddlddlCCVinculo.SelectedValue = _depositoBanco.CODIGO_BANCO.ToString();
            }
        }

        private void mtd_gvImagensDeposito_Confirmar_Vinculo(GridViewRow dgItem)
        {
            DEPOSITO_DOCUMENTO depositoDocumento = new DEPOSITO_DOCUMENTO();

            Panel _panVincular = (Panel)(dgItem.FindControl("panVincular"));

            HiddenField _hdnNomeImagem = (HiddenField)(dgItem.FindControl("hdnNomeImagem"));
            HiddenField _hdnLocalImagem = (HiddenField)(dgItem.FindControl("hdnLocalImagem"));
            HiddenField _hdnCodigoImagem = (HiddenField)(dgItem.FindControl("hdnCodigoImagem"));
            DropDownList _ddlddlCCVinculo = (DropDownList)(dgItem.FindControl("ddlCCVinculo"));
            TextBox _txtValLctoVinculo = (TextBox)(dgItem.FindControl("txtValLctoVinculo"));
            TextBox _txtDtLctoVinculo = (TextBox)(dgItem.FindControl("txtDtLctoVinculo"));

            try
            {
                if (!_txtValLctoVinculo.Text.Equals(""))
                {
                    depositoDocumento.VALOR = Convert.ToDecimal(_txtValLctoVinculo.Text);
                }
                else
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Informe o valor do lançamento.";
                    return;
                }

                depositoDocumento.CAMINHO_IMAGEM = _hdnLocalImagem.Value.ToString();
                depositoDocumento.NOME_IMAGEM = _hdnNomeImagem.Value.ToString();

                depositoDocumento.CODIGO_DEPOSITO = Convert.ToInt32(hdnCodigoDeposito_Consultas.Value);

                depositoDocumento.CODIGO_BANCO = Convert.ToInt32(_ddlddlCCVinculo.SelectedValue);

                if (!_txtDtLctoVinculo.Text.Equals(""))
                {
                    depositoDocumento.DATA_LANCAMENTO = Convert.ToDateTime(_txtDtLctoVinculo.Text);
                }
                else
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Informe a data de lançamento";
                    return;
                }

                usuarioController.InsereDocumento(depositoDocumento);

                IMAGEM_VINCULO_DEPOSITO_DOCUMENTO imagem = new IMAGEM_VINCULO_DEPOSITO_DOCUMENTO();
                imagem.CODIGO_IMAGEM = Convert.ToInt32(_hdnCodigoImagem.Value);
                imagem.CODIGO_DOCUMENTO = depositoDocumento.CODIGO_DOCUMENTO;
                imagem.CODIGO_DEPOSITO = depositoDocumento.CODIGO_DEPOSITO;
                imagemController.InsereImagemVinculoDepositoDocumento(imagem);

                CarregarImagensDocumento(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

                recalcularValorReal(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

                _panVincular.Visible = false;
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        private void recalcularValorReal(int codigoDeposito)
        {
            List<DEPOSITO_DOCUMENTO> depositoDocumentoValor = new List<DEPOSITO_DOCUMENTO>();
            decimal dValorDepositoDocumento = 0;
            depositoDocumentoValor = baseController.BuscaDepositoDocumento(codigoDeposito);

            foreach (DEPOSITO_DOCUMENTO d in depositoDocumentoValor)
            {
                dValorDepositoDocumento += d.VALOR;
            }

            if (dValorDepositoDocumento > 0)
            {
                txtValorReal.Text = dValorDepositoDocumento.ToString("N2");
            }
            else
            {
                GridViewRow row = gvDepositos.Rows[Convert.ToInt32(hdnLinha.Value)];

                Literal _litValorDepositado = (Literal)row.FindControl("litValorDepositado");
                if (_litValorDepositado != null)
                {
                    txtValorReal.Text = _litValorDepositado.Text;
                }
                else
                {
                    txtValorReal.Text = "0";
                }
            }
        }

        private void mtd_gvImagensDeposito_Cancelar_Vinculo(GridViewRow dgItem)
        {
            Panel _panVincular = (Panel)(dgItem.FindControl("panVincular"));

            Button _btnVincular = (Button)(dgItem.FindControl("btnVincular"));

            _btnVincular.Visible = true;

            _panVincular.Visible = false;
        }

        private void mtd_gvImagensDeposito_Abrir_Calendario_Vinculo(GridViewRow dgItem)
        {
            Panel _panVincular = (Panel)(dgItem.FindControl("panVincular"));
            Panel _panImgCalendarioVinculo = (Panel)(dgItem.FindControl("panImgCalendarioVinculo"));
            Calendar _CalendarDtLctoVinculo = (Calendar)(dgItem.FindControl("CalendarDtLctoVinculo"));
            _panVincular.Visible = false;
            _panImgCalendarioVinculo.Visible = true;
        }

        private void mtd_gvImagensDeposito_Fechar_Calendario_Vinculo(GridViewRow dgItem)
        {
            Panel _panVincular = (Panel)(dgItem.FindControl("panVincular"));
            Panel _panImgCalendarioVinculo = (Panel)(dgItem.FindControl("panImgCalendarioVinculo"));
            _panVincular.Visible = true;
            _panImgCalendarioVinculo.Visible = false;
        }

        protected void gvImagensDeposito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 iIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow dgItem = gvImagensDeposito.Rows[iIndex];

            if (e.CommandName == "Vincular_Imagem")
            {
                mtd_gvImagensDeposito_Vincular_Imagem(dgItem);
            }
            else if (e.CommandName == "Confirmar_Vinculo")
            {
                mtd_gvImagensDeposito_Confirmar_Vinculo(dgItem);
            }
            else if (e.CommandName == "Cancelar_Vinculo")
            {
                mtd_gvImagensDeposito_Cancelar_Vinculo(dgItem);
            }
            else if (e.CommandName == "Abrir_Calendario_Vinculo")
            {
                mtd_gvImagensDeposito_Abrir_Calendario_Vinculo(dgItem);
            }
            else if (e.CommandName == "Fechar_Calendario_Vinculo")
            {
                mtd_gvImagensDeposito_Fechar_Calendario_Vinculo(dgItem);
            }
        }

        protected void gvImagensDeposito_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        HiddenField _hdnCodigoImagem = e.Row.FindControl("hdnCodigoImagem") as HiddenField;
                        Button _btnVincular = e.Row.FindControl("btnVincular") as Button;

                        GridViewRow row = gvDepositos.Rows[Convert.ToInt32(hdnLinha.Value)];

                        Literal _litEmAberto = (Literal)row.FindControl("litEmAberto");
                        if (_litEmAberto != null)
                        {
                            if (_litEmAberto.Visible == false)
                            {
                                _btnVincular.Visible = false;
                            }
                        }

                        if (_hdnCodigoImagem != null && _btnVincular != null)
                        {
                            IMAGEM_VINCULO_DEPOSITO_DOCUMENTO imagemVinculo = new IMAGEM_VINCULO_DEPOSITO_DOCUMENTO();

                            imagemVinculo = imagemController.BuscaImagemVinculoDepositoDocumento(Convert.ToInt32(_hdnCodigoImagem.Value), 0, 0);

                            if (imagemVinculo != null)
                            {
                                _btnVincular.Visible = false;
                            }
                        }

                        Literal _litDataDigitada = e.Row.FindControl("litDataDigitada") as Literal;
                        if (_litDataDigitada != null)
                        {
                            _litDataDigitada.Text = String.Format("{0:dd/MM/yyyy}", _litDataDigitada.Text.Trim()).Replace(" 00:00:00", "");
                        }

                        HyperLink _linkNomeImagemDeposito = e.Row.FindControl("linkNomeImagemDeposito") as HyperLink;
                        HiddenField _hdnLocalImagem = e.Row.FindControl("hdnLocalImagem") as HiddenField;
                        if (_linkNomeImagemDeposito != null && _hdnLocalImagem != null)
                        {
                            _linkNomeImagemDeposito.NavigateUrl = string.Format("~/Upload/{0}", _hdnLocalImagem.Value.ToString().Trim());
                            _linkNomeImagemDeposito.Text = "<font size='2' face='Calibri'>" + _linkNomeImagemDeposito.Text.Trim() + "</font>";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void CalendarDtLctoVinculo_SelectionChanged(object sender, EventArgs e)
        {
            Calendar _calendar = (Calendar)sender;
            GridViewRow rowgrid = (GridViewRow)_calendar.NamingContainer;

            Panel _panImgCalendarioVinculo = (Panel)rowgrid.FindControl("panImgCalendarioVinculo");
            Panel _panVincular = (Panel)rowgrid.FindControl("panVincular");
            Calendar _CalendarDtLctoVinculo = (Calendar)rowgrid.FindControl("CalendarDtLctoVinculo");
            TextBox _txtDtLctoVinculo = (TextBox)rowgrid.FindControl("txtDtLctoVinculo");

            _panVincular.Visible = true;
            _txtDtLctoVinculo.Text = _CalendarDtLctoVinculo.SelectedDate.ToString("dd/MM/yyyy");
            //_CalendarDtLctoVinculo.Visible = false;
            //_panVincular.Height = 140;
            _panImgCalendarioVinculo.Visible = false;
        }

        protected void btnFecharMensagem_Click(object sender, EventArgs e)
        {
            txtMensagemErro.Text = "";
            panMensagem.Visible = false;
            panPrincipal.Enabled = true;
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                CalendarDeposito.SelectedDate = DateTime.Now;
                txtDeposito.Text = DateTime.Now.ToShortDateString();
                optDataDeposito.Checked = true;
                optDataBanco.Checked = false;
                optDataFechamento.Checked = false;
                ddlFilial.SelectedIndex = 0;
                ddlBanco.SelectedIndex = 0;
                ddlContaCorrente.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 3;

                gvDepositos.DataSource = null;
                gvDepositos.DataBind();
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void btnGravarLancamento_Click(object sender, EventArgs e)
        {
            if (txtValorReal.Text.Trim() == "" || Convert.ToDecimal(txtValorReal.Text) <= 0)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "Informe o valor do lançamento.";
                return;
            }

            try
            {

                var _dados_bco_filial = baseController.BuscarBancoPorFilial(hdnCodigoFilial_Consultas.Value.ToString());

                //Validacoes
                if (_dados_bco_filial.EMP_STATUS != 'A') //Empresa Ativa
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "A empresa não está ATIVA. Favor entrar em contato com TI.";
                    return;
                }

                if (_dados_bco_filial.BCO_STATUS != 'A') //Banco Ativo
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "O banco não está ATIVO. Favor entrar em contato com TI.";
                    return;
                }

            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = ex.Message;
                return;
            }

            DEPOSITO_SEMANA depositoSelecionado = new DEPOSITO_SEMANA();

            depositoSelecionado = baseController.BuscaDepositoSemana(Convert.ToInt32(hdnCodigoDeposito_Consultas.Value));

            depositoSelecionado.VALOR_DEPOSITADO = Convert.ToDecimal(txtValorReal.Text);
            depositoSelecionado.DIFERENCA = depositoSelecionado.VALOR_A_DEPOSITAR - depositoSelecionado.VALOR_DEPOSITADO;
            depositoSelecionado.OBS = txtObs.Text;

            LancamentoLinha lancamento = new LancamentoLinha();
            lancamento = gravarLancamento(depositoSelecionado, hdnCodigoFilial_Consultas.Value.ToString());

            if (lancamento.OK == true)
            {
                try
                {
                    FiltroDepositos filtro = new FiltroDepositos();

                    filtro = validarFiltros();

                    if (filtro.OK == true)
                    {
                        CarregarDepositos(filtro);
                    }

                    panImagens.Visible = false;
                    panPrincipal.Enabled = true;
                }
                catch (Exception ex)
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
                }
            }

        }

        protected void gvImagensLancadas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvImagensLancadas.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = dValorTotalImagensLancadas.ToString("N2");
                footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
            }

            if (gvImagensVinculadas.Visible == true)
            {
                //txtValorReal.Text = dValorTotalImagensVinculadas.ToString("N2");
                //txtDiferenca.Text = (Convert.ToDecimal(lblImgValor.Text.Replace("<font size='2' face='Calibri'><B>", "").Replace("</B></font>", "")) - dValorTotalImagensLancadas).ToString("N2");
                txtDiferenca.Text = (dValorTotalImagensLancadas - Convert.ToDecimal(lblImgValor.Text.Replace("<font size='2' face='Calibri'><B>", "").Replace("</B></font>", ""))).ToString("N2");
            }
        }

        protected void gvImagensLancadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        dValorTotalImagensLancadas += Convert.ToDecimal(e.Row.Cells[3].Text);

                        DEPOSITO_DOCUMENTO depositoDocumento = e.Row.DataItem as DEPOSITO_DOCUMENTO;
                        DEPOSITO_BANCO depositoBanco = new DEPOSITO_BANCO();

                        if (depositoDocumento != null)
                        {
                            Literal _LiteralBanco = e.Row.FindControl("LiteralBanco") as Literal;

                            if (_LiteralBanco != null)
                            {
                                depositoBanco = baseController.BuscarDadosBanco(Convert.ToInt32(depositoDocumento.CODIGO_BANCO));

                                if (depositoBanco != null)
                                {
                                    _LiteralBanco.Text = depositoBanco.NOME + " - " + depositoBanco.CONTA_CTB;

                                    _LiteralBanco.Text = "<font size='2' face='Calibri'>" + _LiteralBanco.Text.Trim() + "</font>";
                                }
                            }
                        }


                        HiddenField _hdnCaminhoImagem = e.Row.FindControl("hdnCaminhoImagem") as HiddenField;
                        Literal _litNomeImagemVinculada = e.Row.FindControl("litNomeImagemVinculada") as Literal;
                        HyperLink _linkNomeImagemVinculada = e.Row.FindControl("linkNomeImagemVinculada") as HyperLink;
                        HiddenField _hdnNomeImagemVinculada = e.Row.FindControl("hdnNomeImagemVinculada") as HiddenField;

                        if (_litNomeImagemVinculada != null && _linkNomeImagemVinculada != null && _hdnNomeImagemVinculada != null && _hdnCaminhoImagem != null)
                        {
                            FILIAIS_CONFIGURACAO nomeDiretorio = new FILIAIS_CONFIGURACAO();
                            nomeDiretorio = baseController.BuscarFiliaisConfiguracao(hdnCodigoFilial_Consultas.Value.ToString());

                            String arquivo = "";

                            _litNomeImagemVinculada.Visible = false;

                            if (_hdnCaminhoImagem.Value.ToString().Trim() != "")
                            {
                                arquivo = nomeDiretorio.PASTA_IMAGENS_DEPOSITO + "\\" + _hdnCaminhoImagem.Value.ToString().Trim();

                                if (arquivoExiste(arquivo) == true)
                                {
                                    _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                }
                                else
                                {
                                    arquivo = "~/Upload/" + _hdnCaminhoImagem.Value.ToString().Trim();

                                    if (arquivoExiste(arquivo) == true)
                                    {
                                        _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                    }
                                    else
                                    {
                                        arquivo = "";
                                    }
                                }
                            }

                            if (_hdnNomeImagemVinculada.Value.ToString().Trim() != "" && arquivo == "")
                            {
                                arquivo = nomeDiretorio.PASTA_IMAGENS_DEPOSITO + "\\" + _hdnNomeImagemVinculada.Value.ToString().Trim();

                                if (arquivoExiste(arquivo) == true)
                                {
                                    _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                }
                                else
                                {
                                    arquivo = "~/Upload/" + _hdnNomeImagemVinculada.Value.ToString().Trim();

                                    if (arquivoExiste(arquivo) == true)
                                    {
                                        _linkNomeImagemVinculada.NavigateUrl = string.Format(arquivo);
                                    }
                                    else
                                    {
                                        _linkNomeImagemVinculada.Visible = false;
                                        _litNomeImagemVinculada.Visible = true;
                                    }
                                }
                            }

                            //if (_hdnCaminhoImagem.Value.ToString().Trim() == "")
                            //{
                            //    if (_hdnNomeImagemVinculada.Value.ToString().Trim() != "")
                            //    {
                            //        _linkNomeImagemVinculada.NavigateUrl = string.Format("~/Upload/{0}", _hdnNomeImagemVinculada.Value.ToString().Trim());
                            //    }
                            //}
                            //else
                            //{
                            //    _linkNomeImagemVinculada.NavigateUrl = string.Format("~/Upload/{0}", _hdnCaminhoImagem.Value.ToString().Trim());
                            //}

                            _linkNomeImagemVinculada.Text = "<font size='2' face='Calibri'>" + _linkNomeImagemVinculada.Text.Trim() + "</font>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void gvImagensLancadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 iIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow dgItem = gvImagensLancadas.Rows[iIndex];

            if (e.CommandName == "Excluir_Imagem")
            {
                mtd_gvImagensLancadas_Excluir_Imagem(dgItem);
            }
        }

        protected void gvImagensDeposito_DataBound(object sender, EventArgs e)
        {
            if (gvImagensDeposito.EditIndex != -1)
            {
                GridViewRow editRow = gvImagensDeposito.Rows[gvImagensDeposito.EditIndex];

                TextBox _txtValLctoVinculo = (TextBox)editRow.FindControl("txtValLctoVinculo");
                _txtValLctoVinculo.Attributes.Add("onkeyup", "formataValor(this, event);");

                TextBox _txtDtLctoVinculo = (TextBox)editRow.FindControl("txtDtLctoVinculo");
                _txtDtLctoVinculo.Attributes.Add("onkeyup", "formataData(this, event);");
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue == "1")
            {
                chkIgnorarData.Visible = true;
                chkIgnorarData.Checked = true;
            }
            else
            {
                chkIgnorarData.Visible = false;
            }
        }

        protected void gvImagensReferencias_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvImagensReferencias.FooterRow;

            if (footer != null)
            {
                //footer.Cells[0].ColumnSpan = 2;
                footer.Cells[0].Text = "Localizado  " + Convert.ToString(gvImagensReferencias.Rows.Count) + "  Fechamentos";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = "Total:";
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[4].Text = dValorParaDeposito2.ToString("N2");
                footer.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void gvImagensReferencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        Literal _litDataFechamento = e.Row.FindControl("litDataFechamento") as Literal;

                        if (_litDataFechamento != null)
                        {
                            _litDataFechamento.Text = String.Format("{0:dd/MM/yyyy}", _litDataFechamento.Text.Trim()).Replace(" 00:00:00", "");
                        }

                        Literal _litValorDeposito = e.Row.FindControl("litValorDeposito") as Literal;
                        if (_litValorDeposito != null)
                        {
                            if (Convert.ToDecimal(_litValorDeposito.Text) > 0)
                            {
                                dValorParaDeposito2 += (string.IsNullOrEmpty(_litValorDeposito.Text.ToString().Trim()) ? 0 : Convert.ToDecimal(_litValorDeposito.Text));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
    }

    public class LancamentoLinha
    {
        public String Lancamentos { get; set; }
        public String DataLancamento { get; set; }
        public String Usuario { get; set; }
        public String DataUsuario { get; set; }
        public Boolean OK { get; set; }
    }

    public class FiltroDepositos
    {
        public DateTime DataDeposito { get; set; }
        public Char TipoDataDeposito { get; set; }
        public String CodigoFilial { get; set; }
        public String NomeBanco { get; set; }
        public Int32 CodigoBanco { get; set; }
        public Int32 Status { get; set; }
        public Char IgnorarData { get; set; }
        public Boolean OK { get; set; }
    }
}