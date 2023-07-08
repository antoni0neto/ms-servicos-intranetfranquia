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
    public partial class GestaoDepositoBanco : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();
        LojaController lojaController = new LojaController();

        decimal totalDeposito = 0;
        decimal totalDocumento = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                CarregaDropDownListBaixa();

                txtAssinatura.Text = "";
                txtDataDeposito.Text = "";
                txtDiferenca.Text = "";
                //txtDiferencaAnterior.Text = "";
                txtObs.Text = "";
                txtValorADepositar.Text = "";
                txtValorDepositado.Text = "";
            }
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais_Intermediario(usuario);
                ddlFilial.DataBind();
            }
        }

        protected void CalendarDataLancamento_SelectionChanged(object sender, EventArgs e)
        {
            txtDataLancamento.Text = CalendarDataLancamento.SelectedDate.ToString("dd/MM/yyyy");
        }

        private void CarregaDropDownListBaixa()
        {
            ddlBaixa.DataSource = baseController.BuscaDepositoBaixas();
            ddlBaixa.DataBind();
        }

        private void CarregaDropDownListContaBanco()
        {

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

            ddlContaContabil.DataSource = _listAux;
            ddlContaContabil.DataBind();

            try
            {
                //Carregar banco da filial
                var _depositoBanco = baseController.BuscarBancoPorFilial(ddlFilial.SelectedValue);
                if (_depositoBanco != null)
                {
                    ddlContaContabil.SelectedValue = _depositoBanco.CODIGO_BANCO.ToString();
                }
            }
            catch (Exception)
            {
            }
        }

        private void CarregaGridViewDepositos()
        {
            txtAssinatura.Text = "";
            txtDataDeposito.Text = "";
            txtDiferenca.Text = "";
            //txtDiferencaAnterior.Text = "";
            txtObs.Text = "";
            txtValorADepositar.Text = "";
            txtValorDepositado.Text = "";

            GridViewMovimento.Visible = false;
            GridViewDocumento.Visible = false;

            LabelFeedBack.Text = "";

            List<DEPOSITO_SEMANA> listaDepositosSemana = null;
            listaDepositosSemana = baseController.BuscaDepositosRealizadosFilial(Convert.ToInt32(ddlFilial.SelectedValue), Convert.ToInt32(ddlBaixa.SelectedValue));

            if (listaDepositosSemana != null & listaDepositosSemana.Count > 0)
            {
                GridViewDepositos.DataSource = listaDepositosSemana;
                GridViewDepositos.DataBind();
            }
            else
            {
                GridViewDepositos.DataSource = null;
                GridViewDepositos.DataBind();
            }
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void ddlBaixa_DataBound(object sender, EventArgs e)
        {
            ddlBaixa.Items.Add(new ListItem("Selecione", "0"));
            ddlBaixa.SelectedValue = "0";
        }

        protected void btBuscarDepositos_Click(object sender, EventArgs e)
        {
            if (ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewDepositos();
            CarregaDropDownListContaBanco();
            divMovimento.Visible = false;
            divValores.Visible = false;
        }

        protected void btAlterar_Click(object sender, EventArgs e)
        {

            GridViewMovimento.Visible = true;
            GridViewDocumento.Visible = true;

            Button btAlterar = sender as Button;

            if (btAlterar != null)
                CarregaGridViewMovimento(btAlterar.CommandArgument);

            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (Convert.ToInt32(ddlBaixa.SelectedValue) != 2)
                {
                    btGravar.Enabled = true;
                }
                else
                {
                    btGravar.Enabled = false;

                    foreach (GridViewRow r in GridViewDocumento.Rows)
                    {
                        Button _btExcluir = r.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.Enabled = false;
                    }
                }

                if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_USUARIO == 1183) // Usuário Administrador
                    btGravar.Enabled = true;
            }

            divMovimento.Visible = true;
            divValores.Visible = true;
            if (ddlBaixa.SelectedValue == "2") //conferido
            {
                divMovimento.Visible = false;
            }
        }

        protected void GridViewDocumento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDocumento.FooterRow;

            foreach (GridViewRow item in GridViewDocumento.Rows)
            {
                totalDocumento += Convert.ToDecimal(item.Cells[2].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[2].Text = totalDocumento.ToString();
            }

            Session["DOCUMENTO"] = totalDocumento;

            txtValorDepositado.Text = totalDocumento.ToString();
            txtDiferenca.Text = (Convert.ToDecimal(txtValorADepositar.Text) - Convert.ToDecimal(txtValorDepositado.Text)).ToString();
        }

        protected void GridViewDepositos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDepositos.PageIndex = e.NewPageIndex;
            CarregaGridViewDepositos();
        }

        protected void GridViewDepositos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btAlterar = e.Row.FindControl("btAlterar") as Button;

                if (btAlterar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        DEPOSITO_SEMANA itemDepositoSemana = e.Row.DataItem as DEPOSITO_SEMANA;

                        btAlterar.CommandArgument = itemDepositoSemana.CODIGO_DEPOSITO.ToString();
                    }
                }
            }

            DEPOSITO_SEMANA depositoSemana = e.Row.DataItem as DEPOSITO_SEMANA;

            if (depositoSemana != null)
            {
                Literal literalBaixa = e.Row.FindControl("LiteralBaixa") as Literal;

                if (literalBaixa != null)
                    literalBaixa.Text = lojaController.BuscaDepositoBaixa(depositoSemana.CODIGO_BAIXA).DESCRICAO;
            }

        }

        private void CarregaGridViewMovimento(string codigo)
        {
            DEPOSITO_SEMANA depositoSemana = baseController.BuscaDepositoSemana(Convert.ToInt32(codigo));

            decimal totalDeposito = 0;

            if (depositoSemana != null)
            {
                List<DEPOSITO_HISTORICO> listaDeposito = baseController.BuscaDepositoSemanaEfetuado(depositoSemana.CODIGO_FILIAL, depositoSemana.DATA_INICIO, depositoSemana.DATA_FIM);

                if (listaDeposito != null)
                {
                    foreach (DEPOSITO_HISTORICO deposito in listaDeposito)
                    {
                        totalDeposito += deposito.VALOR_DEPOSITO;
                    }

                    GridViewMovimento.DataSource = listaDeposito;
                    GridViewMovimento.DataBind();
                }
            }

            txtValorADepositar.Text = totalDeposito.ToString();
            txtDiferenca.Text = depositoSemana.DIFERENCA.ToString();
            txtAssinatura.Text = depositoSemana.ASSINATURA;
            txtDataDeposito.Text = depositoSemana.DATA_DEPOSITO.ToString();
            txtCodigoDeposito.Text = codigo;
            txtObs.Text = depositoSemana.OBS;

            List<DEPOSITO_DOCUMENTO> listaDepositoDocumento = baseController.BuscaDepositoDocumento(Convert.ToInt32(codigo));

            if (listaDepositoDocumento != null)
            {
                GridViewDocumento.DataSource = listaDepositoDocumento;
                GridViewDocumento.DataBind();
            }

            if (Session["DOCUMENTO"] != null)
            {
                totalDocumento = Convert.ToDecimal(Session["DOCUMENTO"]);

                txtValorDepositado.Text = totalDocumento.ToString();
            }
        }

        protected void GridViewMovimento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewMovimento.FooterRow;

            foreach (GridViewRow item in GridViewMovimento.Rows)
            {
                totalDeposito += Convert.ToDecimal(item.Cells[1].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalDeposito.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
            }

            Session["DEPOSITO"] = totalDeposito;
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    txtDiretorio.Text = System.IO.Path.GetDirectoryName(FileUpload1.PostedFile.FileName);

                    txtArquivo.Text = FileUpload1.FileName;

                    Salvar();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                LabelFeedBack.Text = "Selecione uma imagem.";
            }
        }

        private void Salvar()
        {
            try
            {
                DEPOSITO_DOCUMENTO depositoDocumento = new DEPOSITO_DOCUMENTO();

                depositoDocumento.CAMINHO_IMAGEM = txtDiretorio.Text;
                depositoDocumento.NOME_IMAGEM = txtArquivo.Text;
                depositoDocumento.CODIGO_DEPOSITO = Convert.ToInt32(txtCodigoDeposito.Text);

                if (!txtValor.Text.Equals(""))
                {
                    depositoDocumento.VALOR = Convert.ToDecimal(txtValor.Text);
                }

                depositoDocumento.CODIGO_BANCO = Convert.ToInt32(ddlContaContabil.SelectedValue);
                if (!txtDataLancamento.Text.Equals(""))
                {
                    depositoDocumento.DATA_LANCAMENTO = Convert.ToDateTime(txtDataLancamento.Text);
                }
                else
                {
                    LabelFeedBack.Text = "Informe a data de lançamento";
                    return;
                }

                usuarioController.InsereDocumento(depositoDocumento);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewDocumento(depositoDocumento.CODIGO_DEPOSITO);

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void UploadButton1_Click(object sender, EventArgs e)
        {
        }
        private void CarregaGridViewDocumento(int codigoDeposito)
        {
            GridViewDocumento.DataSource = usuarioController.BuscaDocumentos(codigoDeposito);
            GridViewDocumento.DataBind();
        }
        private void LimpaTela()
        {
            txtValor.Text = string.Empty;
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button btExcluir = sender as Button;
            SP_INTRANET_ESTORNA_LANCAMENTO_LINXResult sp = null;
            int _numeroLancamento = 0;

            if (btExcluir != null)
            {

                var _depositoDocumento = usuarioController.BuscaDepositoDocumento(Convert.ToInt32(btExcluir.CommandArgument));

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
                            LabelFeedBack.Text = "Erro Procedure (1). Código: " + sp.CODIGO_ERRO.ToString() + " - Mensagem: " + sp.MENSAGEM_ERRO;
                            return;
                        }
                    }
                }

                usuarioController.ExcluiDocumento(Convert.ToInt32(btExcluir.CommandArgument));

                CarregaGridViewDocumento(Convert.ToInt32(txtCodigoDeposito.Text));
                LimpaTela();

            }
        }
        protected void GridViewDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DEPOSITO_DOCUMENTO depositoDocumento = e.Row.DataItem as DEPOSITO_DOCUMENTO;
            DEPOSITO_BANCO depositoBanco = new DEPOSITO_BANCO();

            if (depositoDocumento != null)
            {
                Button btExcluir = e.Row.FindControl("btExcluir") as Button;

                if (btExcluir != null)
                    btExcluir.CommandArgument = depositoDocumento.CODIGO_DOCUMENTO.ToString();

                Literal _LiteralBanco = e.Row.FindControl("LiteralBanco") as Literal;

                if (_LiteralBanco != null)
                {
                    depositoBanco = baseController.BuscarDadosBanco(Convert.ToInt32(depositoDocumento.CODIGO_BANCO));
                    if (depositoBanco != null)
                        _LiteralBanco.Text = depositoBanco.NOME + " - " + depositoBanco.CONTA_CTB;
                }
            }
        }
        protected void btGravar_Click(object sender, EventArgs e)
        {
            try
            {

                var _dados_bco_filial = baseController.BuscarBancoPorFilial(ddlFilial.SelectedValue);

                if (_dados_bco_filial == null)
                {
                    lblErroGravar.Text = "Não foi possível encontrar o Banco da Filial. Favor entrar em contato com TI.";
                    return;
                }
                //Validacoes
                if (_dados_bco_filial.EMP_STATUS != 'A') //Empresa Ativa
                {
                    lblErroGravar.Text = "A empresa não está ATIVA. Favor entrar em contato com TI.";
                    return;
                }

                if (_dados_bco_filial.BCO_STATUS != 'A') //Banco Ativo
                {
                    lblErroGravar.Text = "O banco não está ATIVO. Favor entrar em contato com TI.";
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErroGravar.Text = ex.Message;
            }

            DEPOSITO_SEMANA depositoSemana = new DEPOSITO_SEMANA();

            depositoSemana.CODIGO_DEPOSITO = Convert.ToInt32(txtCodigoDeposito.Text);
            depositoSemana.AGENCIA = "0";
            depositoSemana.ANO_SEMANA = 0;
            depositoSemana.ASSINATURA = txtAssinatura.Text;
            depositoSemana.BANCO = 0;
            depositoSemana.CODIGO_FILIAL = Convert.ToInt32(ddlFilial.SelectedValue);
            depositoSemana.COMPROVANTE = "";
            depositoSemana.CONTA = "0";
            depositoSemana.DATA_DEPOSITO = Convert.ToDateTime(txtDataDeposito.Text.ToString()); //baseController.BuscaDataBanco().data;

            if (txtValorADepositar.Text.Equals(""))
                txtValorADepositar.Text = "0";

            depositoSemana.VALOR_A_DEPOSITAR = Convert.ToDecimal(txtValorADepositar.Text);

            if (txtValorDepositado.Text.Equals(""))
                txtValorDepositado.Text = "0";

            depositoSemana.VALOR_DEPOSITADO = Convert.ToDecimal(txtValorDepositado.Text);
            depositoSemana.DIFERENCA = depositoSemana.VALOR_A_DEPOSITAR - depositoSemana.VALOR_DEPOSITADO;
            depositoSemana.OBS = txtObs.Text;
            depositoSemana.SALDO_INICIAL = 0;

            List<DEPOSITO_SALDO> listaSaldo = baseController.BuscaUltimosSaldos(Convert.ToInt32(ddlFilial.SelectedValue));

            if (listaSaldo != null)
            {
                int i = 0;

                foreach (DEPOSITO_SALDO item in listaSaldo)
                {
                    i++;

                    if (i == 2)
                        depositoSemana.SALDO_INICIAL = item.VALOR_SALDO;
                }
            }

            depositoSemana.SALDO_FINAL = depositoSemana.SALDO_INICIAL + depositoSemana.DIFERENCA;
            depositoSemana.CODIGO_BAIXA = Convert.ToInt32(ddlBaixa.SelectedValue);

            baseController.AtualizaDepositoSemana(depositoSemana);

            /*INICIO LANÇÁMENTO CONTÁBIL*/
            //Deposito conferido? 
            if (ddlBaixa.SelectedValue == "2")
            {
                LancamentoContabilEntity _lancamento = null;
                SP_INTRANET_REGISTRA_LANCAMENTO_LINXResult sp = null;
                TIPO_LANCAMENTO tpEntrada = null;
                TIPO_LANCAMENTO tpSaida = null;
                DateTime _dataInicio = Convert.ToDateTime("2014-09-16 00:00:00");
                DateTime _dataInicial = Convert.ToDateTime("2014-09-16 00:00:00");

                try
                {

                    DEPOSITO_SEMANA _dS = baseController.BuscaDepositoSemana(Convert.ToInt32(txtCodigoDeposito.Text));
                    if (_dS != null)
                        _dataInicio = _dS.DATA_INICIO;

                    if (_dataInicio >= _dataInicial)
                    {

                        var _listDocumento = usuarioController.BuscaDocumentosParaLancamento(Convert.ToInt32(txtCodigoDeposito.Text));
                        tpEntrada = usuarioController.BuscarTipoLancamento(3);
                        tpSaida = usuarioController.BuscarTipoLancamento(2);

                        foreach (var _list in _listDocumento)
                        {
                            //Obter Conta Banco
                            var _contaBanco = baseController.BuscarDadosBanco((int)_list.CODIGO_BANCO);

                            _lancamento = new LancamentoContabilEntity();
                            _lancamento.Empresa = 1; //FIXO
                            _lancamento.Filial = ddlFilial.SelectedValue;
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
                                    }
                                }
                                else
                                {
                                    lblErroGravar.Text = "Erro Procedure(2). Código: " + sp.CODIGO_ERRO.ToString() + " - Mensagem: " + sp.MENSAGEM_ERRO;
                                    return;
                                }
                            }
                        }
                    }
                    //Confere e volta para "Em Aberto"
                    ddlBaixa.SelectedValue = "1";
                    CarregaGridViewDepositos();
                }
                catch (Exception ex)
                {
                    lblErroGravar.Text = ex.Message;
                }
                finally
                {
                    sp = null;
                }

            }
            /*FIM LANÇÁMENTO CONTÁBIL*/
            else
            {
                CarregaGridViewDepositos();
            }
        }

    }
}
