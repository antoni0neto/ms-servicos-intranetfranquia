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
    public partial class DepositoBanco : System.Web.UI.Page
    {
        ImagemController imagemController = new ImagemController();
        BaseController baseController = new BaseController();

        decimal totalDeposito = 0;

        private void calcularDiferenca()
        {
            if (txtValorDepositadoCalculado.Text.Equals(""))
                txtValorDepositadoCalculado.Text = "0";
            if (txtValorDepositado.Text.Equals(""))
                txtValorDepositado.Text = "0";
            txtDiferenca.Text = (Convert.ToDecimal(txtValorDepositadoCalculado.Text) - Convert.ToDecimal(txtValorDepositado.Text)).ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();

                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();
                if (codigoPerfil != "2" && codigoPerfil != "3")
                {
                    labTitulo.Text = "Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Depósito em Banco";
                    hrefVoltar.HRef = "DefaultFinanceiro.aspx";
                }

                txtDataDeposito.Attributes.Add("onkeyup", "formataData(this, event);");
                txtHoraDeposito.Attributes.Add("onkeyup", "formataHora(this, event);");
            }

            btGravar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGravar, null) + ";");
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataDeposito_SelectionChanged(object sender, EventArgs e)
        {
            txtDataDeposito.Text = CalendarDataDeposito.SelectedDate.ToString("dd/MM/yyyy");
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

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Insert(0, new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        private void CarregaGridViewMovimento()
        {
            GridViewMovimento.DataSource = baseController.BuscaDepositoSemana(Convert.ToInt32(ddlFilial.SelectedValue), CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);
            GridViewMovimento.DataBind();

            gridALancar.DataSource = baseController.BuscaDepositoSemana(Convert.ToInt32(ddlFilial.SelectedValue), CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);
            gridALancar.DataBind();

            txtValorADepositar.Text = totalDeposito.ToString();
        }

        protected void btMovimento_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null)
            {
                lblValidaBusca.Text = "Necessário informar as datas de início e fim para efetuar a busca dos movimentos em dinheiro.";
                return;
            }
            if (TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
            {
                lblValidaBusca.Text = "Necessário informar as datas de início e fim para efetuar a busca dos movimentos em dinheiro.";
                return;
            }
            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
            {
                lblValidaBusca.Text = "Necessário informar uma filial para efetuar a busca dos movimentos em dinheiro.";
                return;
            }

            // Ver falha nas datas depósito

            DEPOSITO_HISTORICO depositoHistorico = baseController.BuscaDepositoRealizado(Convert.ToInt32(ddlFilial.SelectedValue), CalendarDataInicio.SelectedDate).OrderByDescending(p => p.DATA).Take(1).SingleOrDefault();

            if (depositoHistorico != null)
            {
                if (depositoHistorico.CODIGO_SALDO == 0)
                {
                    lblValidaBusca.Text = "Existe depósito a ser realizado em dias anteriores. Se não tiver, por favor, entre em contato com TI! ";

                    GridViewMovimento.DataSource = null;
                    GridViewMovimento.DataBind();

                    return;
                }
                else
                    lblValidaBusca.Text = "";
            }
            else
                lblValidaBusca.Text = "";

            btAtualizar1.Enabled = true;

            Session["1A_IMAGEM"] = false;
            Session["2A_IMAGEM"] = false;
            Session["3A_IMAGEM"] = false;
            Session["4A_IMAGEM"] = false;

            CarregaGridViewMovimento();

            DEPOSITO_SALDO saldo = baseController.BuscaUltimoDepositoSaldo(Convert.ToInt32(ddlFilial.SelectedValue));

            //if (saldo != null)
            //    txtDiferencaAnterior.Text = saldo.VALOR_SALDO.ToString("###0.00");
            //else
            txtDiferencaAnterior.Text = "-";

            List<DEPOSITO_HISTORICO> depositosRealizados = baseController.BuscaDepositosRealizados(Convert.ToInt32(ddlFilial.SelectedValue), DateTime.Now.AddDays(-30), DateTime.Now);

            foreach (DEPOSITO_HISTORICO depositoRealizado in depositosRealizados)
            {
                CalendarDepositosRealizados.SelectedDates.Add(depositoRealizado.DATA.Date);
            }

            //Carregar banco da filial
            var _depositoBanco = baseController.BuscarBancoPorFilial(ddlFilial.SelectedValue);
            if (_depositoBanco != null)
            {
                txtBancoDepositado.Text = _depositoBanco.BANCO;
                txtAgenciaConta.Text = _depositoBanco.AGENCIA + '.' + _depositoBanco.CONTA;
            }

        }

        protected void btGravar_Click(object sender, EventArgs e)
        {

            DEPOSITO_SEMANA depositoSemana = new DEPOSITO_SEMANA();

            depositoSemana.TIPO_DEPOSITO = 1;

            Gravar(depositoSemana);
        }

        private void Gravar(DEPOSITO_SEMANA depositoSemana)
        {
            DateTime dtInicio = DateTime.Now.Date;
            DateTime dtFim = DateTime.Now.Date;

            DateTime dtLancamento = DateTime.Now.Date;

            Boolean bTemLancamento = false;
            Char cTemBuraco = 'N';
            Char cPrimeiro = 'S';

            Boolean bPrimeiroLancamento = false;

            lblMensagem.Text = "";

            foreach (GridViewRow row in gridALancar.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLancar");

                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        bTemLancamento = true;

                        if (bPrimeiroLancamento == false)
                        {
                            bPrimeiroLancamento = true;
                            dtInicio = Convert.ToDateTime(row.Cells[1].Text);
                        }

                        dtFim = Convert.ToDateTime(row.Cells[1].Text);

                        if (cTemBuraco == 'T')
                        {
                            cTemBuraco = 'S';
                            break;
                        }
                    }
                    else
                    {
                        if (cPrimeiro == 'S')
                        {
                            break;
                        }

                        if (bTemLancamento == true)
                        {
                            cTemBuraco = 'T';
                        }
                    }

                    cPrimeiro = 'N';
                }
            }

            //foreach (GridViewRow row in gridALancar.Rows)
            //{
            //    CheckBox chk = (CheckBox)row.FindControl("chkLancar");

            //    if (chk != null)
            //    {
            //        if (chk.Checked)
            //        {
            //            cPrimeiro = 'N';
            //            bTemLancamento = true;

            //            dtInicio = Convert.ToDateTime(row.Cells[1].Text);
            //            dtFim = Convert.ToDateTime(row.Cells[1].Text);

            //            break;
            //        }
            //    }
            //}


            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
            {
                lblMensagem.Text = "Informe uma filial para os lançamentos.";
                return;
            }

            if (gridALancar.Rows.Count == 0)
            {
                lblMensagem.Text = "Não existem movimentos a serem lançados. Certifique-se de ter clicado em 'Buscar Movimento Dinheiro' após escolher um período e uma filial.";
                return;
            }

            if (cPrimeiro == 'S')
            {
                lblMensagem.Text = "A seleção dos dias para depósito devem sempre iniciar marcando a primeira data da lista ao lado para o lançamento.";
                return;
            }

            if (bTemLancamento == false)
            {
                lblMensagem.Text = "Informe as datas de lançamento clicando nas seleções da lista ao lado.\n Caso estejam ocultas, escolha um período, uma filial e clique em 'Buscar Movimento Dinheiro'.";
                return;
            }

            if (cTemBuraco == 'S')
            {
                lblMensagem.Text = String.Format("Existem datas faltando na seleção de lançamentos entre a data inicial marcada {0} e a data seguinte/final marcada {1}.", dtInicio.ToString("dd/MM/yyyy"), dtFim.ToString("dd/MM/yyyy"));
                return;
            }

            if (txtValorDepositado.Text.Equals("") || txtValorDepositado.Text == null)
            {
                lblMensagem.Text = "Informe o valor a ser depositado.";
                return;
            }

            if (Convert.ToDouble(txtValorDepositado.Text) <= 0)
            {
                lblMensagem.Text = "Informe um valor a ser depositado maior que 0 (zero).";
                return;
            }

            //if (Convert.ToDouble(txtValorDepositadoCalculado.Text) < Convert.ToDouble(txtValorDepositado.Text))
            //{
            //    lblMensagem.Text = "Valor depositado é maior que o valor a depositar.";
            //    return;
            //}

            if (Convert.ToDouble(txtValorDepositadoCalculado.Text) - Convert.ToDouble(txtValorDepositado.Text) > 5)
            {
                lblMensagem.Text = "Diferença do valor a depositar e o valor depositado é superior a R$5,00.";
                return;
            }

            if (Convert.ToDouble(txtValorDepositado.Text) - Convert.ToDouble(txtValorDepositadoCalculado.Text) > 10)
            {
                lblMensagem.Text = "Diferença do valor depositado e o valor a depositar é superior a R$10,00.";
                return;
            }

            if (txtAssinatura.Text.Equals("") || txtAssinatura.Text == null)
            {
                lblMensagem.Text = "Informe uma assinatura.";
                return;
            }

            if (txtDataDeposito.Text.Equals("") || txtDataDeposito.Text == null)
            {
                lblMensagem.Text = "Informe a data o depósito.";
                return;
            }

            DateTime dtValida = DateTime.MinValue;
            if (!DateTime.TryParse(txtDataDeposito.Text, out dtValida))
            {
                lblMensagem.Text = "Informe uma data de depósito válida.";
                return;
            }

            if (Convert.ToDateTime(txtDataDeposito.Text) < dtFim)
            {
                lblMensagem.Text = "A data do depósito não pode ser menor que a última data selecionada para lançamento.";
                return;
            }

            if (Convert.ToDateTime(txtDataDeposito.Text) > baseController.BuscaDataBanco().data)
            {
                lblMensagem.Text = "A data do depósito não pode ser maior que a data atual.";
                return;
            }

            if (txtHoraDeposito.Text.Trim() == "")
            {
                lblMensagem.Text = "Informe uma hora de depósito válida.";
                return;
            }

            if (txtHoraDeposito.Text.Trim() == "00:00")
            {
                lblMensagem.Text = "Informe uma hora de depósito válida.";
                return;
            }

            string[] saHoraDeposito = txtHoraDeposito.Text.Split(':');
            string s_Hora = saHoraDeposito[0].ToString().Trim();
            string s_Minuto = saHoraDeposito[1].ToString().Trim();

            if (s_Hora.Trim() == "")
            {
                lblMensagem.Text = "Informe uma hora de depósito válida.";
                return;
            }

            if (Convert.ToInt16(s_Hora) < 0 && Convert.ToInt16(s_Hora) > 23)
            {
                lblMensagem.Text = "Informe uma hora de depósito válida.";
                return;
            }

            if (s_Minuto.Trim() == "")
            {
                lblMensagem.Text = "Informe o minuto da hora de depósito válida.";
                return;
            }

            if (Convert.ToInt16(s_Minuto) < 0 && Convert.ToInt16(s_Minuto) > 59)
            {
                lblMensagem.Text = "Informe o minuto da hora de depósito válida.";
                return;
            }

            if ((Convert.ToBoolean(Session["1A_IMAGEM"]) == false) && (Convert.ToBoolean(Session["2A_IMAGEM"]) == false) && (Convert.ToBoolean(Session["3A_IMAGEM"]) == false) && (Convert.ToBoolean(Session["4A_IMAGEM"]) == false))
            {
                lblMensagem.Text = "Favor inserir pelo menos uma imagem de comprovante de depósito.";
                return;
            }

            //List<DEPOSITO_HISTORICO> listDepHist = baseController.BuscaDepositoSemana(Convert.ToInt32(ddlFilial.SelectedValue), dtInicio, dtFim);
            //if (listDepHist != null)
            //{
            //    if (listDepHist.Count() > 1)
            //    {
            //        dtInicio = listDepHist.First().DATA;
            //        dtFim = listDepHist.Last().DATA;
            //    }
            //    else if (listDepHist.Count() == 1)
            //    {
            //        dtInicio = dtFim = listDepHist.First().DATA;
            //    }
            //    else if (listDepHist.Count() == 0)
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    return;
            //}

            depositoSemana.AGENCIA = "0";
            depositoSemana.ANO_SEMANA = 0;
            depositoSemana.ASSINATURA = txtAssinatura.Text;
            depositoSemana.BANCO = 0;
            depositoSemana.CODIGO_FILIAL = Convert.ToInt32(ddlFilial.SelectedValue);
            depositoSemana.COMPROVANTE = "";
            depositoSemana.CONTA = "0";
            depositoSemana.DATA_DEPOSITO = Convert.ToDateTime(txtDataDeposito.Text.ToString()); //baseController.BuscaDataBanco().data;
            depositoSemana.HORA_DEPOSITO = txtHoraDeposito.Text.ToString();
            depositoSemana.DATA_INICIO = dtInicio;
            depositoSemana.DATA_FIM = dtFim;
            calcularDiferenca();
            depositoSemana.DIFERENCA = Convert.ToDecimal(txtDiferenca.Text);
            depositoSemana.VALOR_A_DEPOSITAR = Convert.ToDecimal(txtValorDepositadoCalculado.Text);
            depositoSemana.VALOR_DEPOSITADO = Convert.ToDecimal(txtValorDepositado.Text);
            depositoSemana.CODIGO_BAIXA = 1;

            baseController.IncluirDepositoBanco(depositoSemana);

            DEPOSITO_SALDO depositoSaldo = new DEPOSITO_SALDO();

            depositoSaldo.CODIGO_FILIAL = Convert.ToInt32(ddlFilial.SelectedValue);
            depositoSaldo.DATA_DEPOSITO = Convert.ToDateTime(txtDataDeposito.Text.ToString());

            DEPOSITO_SALDO saldo = baseController.BuscaUltimoDepositoSaldo(Convert.ToInt32(ddlFilial.SelectedValue));

            if (saldo != null)
                depositoSaldo.VALOR_SALDO = saldo.VALOR_SALDO + depositoSemana.DIFERENCA;
            else
                depositoSaldo.VALOR_SALDO = depositoSemana.DIFERENCA;

            baseController.IncluirDepositoSaldo(depositoSaldo);

            saldo = baseController.BuscaUltimoDepositoSaldo(Convert.ToInt32(ddlFilial.SelectedValue));

            if (saldo != null)
            {
                foreach (GridViewRow row in gridALancar.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkLancar");

                    if (chk != null)
                    {
                        if (chk.Checked)
                        {
                            dtLancamento = Convert.ToDateTime(row.Cells[1].Text);

                            List<DEPOSITO_HISTORICO> depositos = baseController.BuscaDepositoSemana(Convert.ToInt32(ddlFilial.SelectedValue), dtLancamento, dtLancamento);

                            if (depositos != null)
                            {
                                foreach (DEPOSITO_HISTORICO item in depositos)
                                {
                                    item.CODIGO_SALDO = saldo.CODIGO_SALDO;

                                    baseController.AtualizaHistoricoDeposito(item);
                                }
                            }
                        }
                    }
                }
            }

            IMAGEM imagem;

            if (Convert.ToBoolean(Session["1A_IMAGEM"]))
            {
                imagem = new IMAGEM();

                imagem.NOME_IMAGEM = (ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_1").Replace(" ", "_");

                if (Session["ARQUIVO1"] != null)
                    imagem.LOCAL_IMAGEM = Session["ARQUIVO1"].ToString();
                else
                    imagem.LOCAL_IMAGEM = "";

                imagem.ATIVO = true;
                imagem.CODIGO_LOJA = Convert.ToInt32(ddlFilial.SelectedValue);
                imagem.DATA_DIGITADA = Convert.ToDateTime(txtDataDeposito.Text.ToString());

                try
                {
                    imagemController.Insere(imagem);
                }
                catch (Exception ex)
                {
                    LabelFeedBack1.Text = string.Format("Erro: {0}", ex.Message);
                }
            }

            if (Convert.ToBoolean(Session["2A_IMAGEM"]))
            {
                imagem = new IMAGEM();

                imagem.NOME_IMAGEM = (ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_2").Replace(" ", "_");

                if (Session["ARQUIVO2"] != null)
                    imagem.LOCAL_IMAGEM = Session["ARQUIVO2"].ToString();
                else
                    imagem.LOCAL_IMAGEM = "";

                imagem.ATIVO = true;
                imagem.CODIGO_LOJA = Convert.ToInt32(ddlFilial.SelectedValue);
                imagem.DATA_DIGITADA = Convert.ToDateTime(txtDataDeposito.Text.ToString());

                try
                {
                    imagemController.Insere(imagem);
                }
                catch (Exception ex)
                {
                    LabelFeedBack2.Text = string.Format("Erro: {0}", ex.Message);
                }
            }

            if (Convert.ToBoolean(Session["3A_IMAGEM"]))
            {
                imagem = new IMAGEM();

                imagem.NOME_IMAGEM = (ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_3").Replace(" ", "_");

                if (Session["ARQUIVO3"] != null)
                    imagem.LOCAL_IMAGEM = Session["ARQUIVO3"].ToString();
                else
                    imagem.LOCAL_IMAGEM = "";

                imagem.ATIVO = true;
                imagem.CODIGO_LOJA = Convert.ToInt32(ddlFilial.SelectedValue);
                imagem.DATA_DIGITADA = Convert.ToDateTime(txtDataDeposito.Text.ToString());

                try
                {
                    imagemController.Insere(imagem);
                }
                catch (Exception ex)
                {
                    LabelFeedBack3.Text = string.Format("Erro: {0}", ex.Message);
                }
            }

            if (Convert.ToBoolean(Session["4A_IMAGEM"]))
            {
                imagem = new IMAGEM();

                imagem.NOME_IMAGEM = (ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_4").Replace(" ", "_");

                if (Session["ARQUIVO4"] != null)
                    imagem.LOCAL_IMAGEM = Session["ARQUIVO4"].ToString();
                else
                    imagem.LOCAL_IMAGEM = "";

                imagem.ATIVO = true;
                imagem.CODIGO_LOJA = Convert.ToInt32(ddlFilial.SelectedValue);
                imagem.DATA_DIGITADA = Convert.ToDateTime(txtDataDeposito.Text.ToString());

                try
                {
                    imagemController.Insere(imagem);
                }
                catch (Exception ex)
                {
                    LabelFeedBack4.Text = string.Format("Erro: {0}", ex.Message);
                }
            }

            Response.Redirect("~/FinalizadoComSucesso.aspx");
        }

        protected void btAtualizar1_Click(object sender, EventArgs e)
        {
            txtDiferenca.Text = (Convert.ToDecimal(Session["DEPOSITO"]) - Convert.ToDecimal(txtValorDepositado.Text)).ToString();

            Session["1A_IMAGEM"] = true;
            btAtualizar2.Enabled = true;
            btGravar.Enabled = true;
        }

        protected void btAtualizar2_Click(object sender, EventArgs e)
        {
            txtDiferenca.Text = (Convert.ToDecimal(Session["DEPOSITO"]) - Convert.ToDecimal(txtValorDepositado.Text)).ToString();

            Session["2A_IMAGEM"] = true;
            btAtualizar3.Enabled = true;
            btGravar.Enabled = true;
        }

        protected void btAtualizar3_Click(object sender, EventArgs e)
        {
            txtDiferenca.Text = (Convert.ToDecimal(Session["DEPOSITO"]) - Convert.ToDecimal(txtValorDepositado.Text)).ToString();

            Session["3A_IMAGEM"] = true;
            btAtualizar4.Enabled = true;
            btGravar.Enabled = true;
        }

        protected void btAtualizar4_Click(object sender, EventArgs e)
        {
            txtDiferenca.Text = (Convert.ToDecimal(Session["DEPOSITO"]) - Convert.ToDecimal(txtValorDepositado.Text)).ToString();

            Session["4A_IMAGEM"] = true;
            btGravar.Enabled = true;
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
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalDeposito.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
            }

            //Session["DEPOSITO"] = totalDeposito;
        }

        protected void UploadButton1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    if (FileUpload1.PostedFile.ContentType == "image/jpeg" || FileUpload1.PostedFile.ContentType == "image/pjpeg")
                    {
                        if (FileUpload1.PostedFile.ContentLength < 500000)
                        {
                            string savePath = Server.MapPath("~/Upload/");

                            String[] Arquivo;
                            String NomeArquivo;
                            char[] cSeparador = { '.' };
                            Arquivo = FileUpload1.FileName.Split(cSeparador, StringSplitOptions.RemoveEmptyEntries);
                            NomeArquivo = Arquivo[0].ToString();

                            String fileName = FileUpload1.FileName.Replace(NomeArquivo, ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_1").Replace(" ", "_");

                            savePath += fileName;

                            FileUpload1.SaveAs(savePath);

                            LabelFeedBack1.Text = "Arquivo carregado com sucesso !!!";

                            Session["ARQUIVO1"] = fileName;
                        }
                        else
                            LabelFeedBack1.Text = "Arquivo não carregado -> maior que 500 kb !!!";
                    }
                    else
                        LabelFeedBack1.Text = "Arquivo não carregado -> não é uma Imagem !!!";
                }
                catch (Exception ex)
                {
                    LabelFeedBack1.Text = ex.Message;
                }
            }
        }

        protected void UploadButton2_Click(object sender, EventArgs e)
        {
            if (FileUpload2.HasFile)
            {
                try
                {
                    if (FileUpload2.PostedFile.ContentType == "image/jpeg" || FileUpload2.PostedFile.ContentType == "image/pjpeg")
                    {
                        if (FileUpload2.PostedFile.ContentLength < 500000)
                        {
                            string savePath = Server.MapPath("~/Upload/");

                            String[] Arquivo;
                            String NomeArquivo;
                            char[] cSeparador = { '.' };
                            Arquivo = FileUpload2.FileName.Split(cSeparador, StringSplitOptions.RemoveEmptyEntries);
                            NomeArquivo = Arquivo[0].ToString();

                            String fileName = FileUpload2.FileName.Replace(NomeArquivo, ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_2").Replace(" ", "_");

                            savePath += fileName;

                            FileUpload2.SaveAs(savePath);

                            LabelFeedBack2.Text = "Arquivo carregado com sucesso !!!";

                            Session["ARQUIVO2"] = fileName;
                        }
                        else
                            LabelFeedBack2.Text = "Arquivo não carregado -> maior que 500 kb !!!";
                    }
                    else
                        LabelFeedBack2.Text = "Arquivo não carregado -> não é uma Imagem !!!";
                }
                catch (Exception ex)
                {
                    LabelFeedBack2.Text = ex.Message;
                }
            }
        }

        protected void UploadButton3_Click(object sender, EventArgs e)
        {
            if (FileUpload3.HasFile)
            {
                try
                {
                    if (FileUpload3.PostedFile.ContentType == "image/jpeg" || FileUpload3.PostedFile.ContentType == "image/pjpeg")
                    {
                        if (FileUpload3.PostedFile.ContentLength < 500000)
                        {
                            string savePath = Server.MapPath("~/Upload/");

                            String[] Arquivo;
                            String NomeArquivo;
                            char[] cSeparador = { '.' };
                            Arquivo = FileUpload3.FileName.Split(cSeparador, StringSplitOptions.RemoveEmptyEntries);
                            NomeArquivo = Arquivo[0].ToString();

                            String fileName = FileUpload3.FileName.Replace(NomeArquivo, ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_3").Replace(" ", "_");

                            savePath += fileName;

                            FileUpload3.SaveAs(savePath);

                            LabelFeedBack3.Text = "Arquivo carregado com sucesso !!!";

                            Session["ARQUIVO3"] = fileName;
                        }
                        else
                            LabelFeedBack3.Text = "Arquivo não carregado -> maior que 500 kb !!!";
                    }
                    else
                        LabelFeedBack3.Text = "Arquivo não carregado -> não é uma Imagem !!!";
                }
                catch (Exception ex)
                {
                    LabelFeedBack3.Text = ex.Message;
                }
            }
        }

        protected void UploadButton4_Click(object sender, EventArgs e)
        {
            if (FileUpload4.HasFile)
            {
                try
                {
                    if (FileUpload4.PostedFile.ContentType == "image/jpeg" || FileUpload4.PostedFile.ContentType == "image/pjpeg")
                    {
                        if (FileUpload4.PostedFile.ContentLength < 500000)
                        {
                            string savePath = Server.MapPath("~/Upload/");

                            String[] Arquivo;
                            String NomeArquivo;
                            char[] cSeparador = { '.' };
                            Arquivo = FileUpload4.FileName.Split(cSeparador, StringSplitOptions.RemoveEmptyEntries);
                            NomeArquivo = Arquivo[0].ToString();

                            String fileName = FileUpload4.FileName.Replace(NomeArquivo, ddlFilial.SelectedItem.ToString().Trim() + "_" + Convert.ToDateTime(txtDataDeposito.Text).ToString("ddMMyyyy").Trim() + "_4").Replace(" ", "_");

                            savePath += fileName;

                            FileUpload4.SaveAs(savePath);

                            LabelFeedBack4.Text = "Arquivo carregado com sucesso !!!";

                            Session["ARQUIVO4"] = fileName;
                        }
                        else
                            LabelFeedBack4.Text = "Arquivo não carregado -> maior que 500 kb !!!";
                    }
                    else
                        LabelFeedBack4.Text = "Arquivo não carregado -> não é uma Imagem !!!";
                }
                catch (Exception ex)
                {
                    LabelFeedBack4.Text = ex.Message;
                }
            }
        }

        protected void chkLancar_CheckedChanged(object sender, EventArgs e)
        {
            decimal valorADepositar = 0;

            foreach (GridViewRow row in gridALancar.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLancar");

                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        valorADepositar += Convert.ToDecimal(row.Cells[2].Text);
                    }
                }
            }

            txtValorDepositadoCalculado.Text = valorADepositar.ToString("0.00");
            calcularDiferenca();

            Session["DEPOSITO"] = valorADepositar;
        }

        protected void txtValorDepositadoCalculado_TextChanged(object sender, EventArgs e)
        {
            calcularDiferenca();
        }

        protected void txtValorDepositado_TextChanged(object sender, EventArgs e)
        {
            calcularDiferenca();
        }
    }
}
