using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Net.Mail;
using System.Net;

namespace Relatorios
{
    public partial class FechamentoCaixaNovo : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    ZerarValores();

                    USUARIO usuario = (USUARIO)Session["USUARIO"];

                    string codigoPerfil = usuario.CODIGO_PERFIL.ToString();

                    if (codigoPerfil == "8")
                    {
                        labTitulo.Text = "Caixa de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento Caixa";
                        hrefVoltar.HRef = "DefaultCaixaFilial.aspx";
                    }
                    else
                    {
                        if (codigoPerfil != "2" && codigoPerfil != "3")
                        {
                            labTitulo.Text = "Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento Caixa";
                            hrefVoltar.HRef = "DefaultFinanceiro.aspx";
                        }
                    }

                    USUARIOLOJA usuarioLoja = baseController.BuscaUsuarioLoja(usuario.CODIGO_USUARIO);

                    if (usuarioLoja != null)
                    {
                        txtFilial.Text = baseController.BuscaFilialCodigoInt(Convert.ToInt32(usuarioLoja.CODIGO_LOJA)).FILIAL;

                        decimal valorFundo = 0;

                        FUNDO_FIXO_CONTA_RECEITA fundoReceita = baseController.BuscaLimiteFundoFilial(Convert.ToInt32(usuarioLoja.CODIGO_LOJA));

                        if (fundoReceita != null)
                        {
                            valorFundo = (fundoReceita.VALOR_FUNDO_LIMITE == null) ? Convert.ToDecimal(fundoReceita.VALOR_FUNDO) : Convert.ToDecimal(fundoReceita.VALOR_FUNDO_LIMITE);

                            decimal saldoAtual = 0;

                            FUNDO_FIXO_HISTORICO_FECHAMENTO fundoSaldo = baseController.BuscaUltimoFechamento(Convert.ToInt32(usuarioLoja.CODIGO_LOJA));

                            if (fundoSaldo != null)
                            {
                                saldoAtual = fundoSaldo.SALDO_ATUAL;

                                decimal valorRetiradaAcumulada = 0;

                                List<FUNDO_FIXO_HISTORICO_RECEITA> receitas = baseController.BuscaReceitasFundoFixo(Convert.ToInt32(usuarioLoja.CODIGO_LOJA), fundoSaldo.DATA, DateTime.Now);

                                if (receitas != null)
                                {
                                    foreach (FUNDO_FIXO_HISTORICO_RECEITA itemReceita in receitas)
                                    {
                                        valorRetiradaAcumulada += itemReceita.VALOR;
                                    }

                                    decimal saldoRetirada = valorFundo - (saldoAtual + valorRetiradaAcumulada);

                                    Session["SaldoRetirada"] = saldoRetirada;

                                    txtValorFundoFixo.Text = saldoRetirada.ToString();
                                }
                            }
                        }
                    }
                    else
                        LabelFeedBack.Text = "Usuário não tem loja vinculada !!!";
                }
            }
        }

        protected void CalendarDataFechamento_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFechamento.Text = CalendarDataFechamento.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFechamento_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date > DateTime.Now.AddDays(0))
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        }

        protected void ButtonAtualizar_Click(object sender, EventArgs e)
        {
            LabelFeedBack.Text = "";

            try
            {

                //Validar perfil do usuario, nao pode ser com senha de supervisor
                if (!ValidarPerfilCaixa())
                {
                    LabelFeedBack.Text = "Não é permitido Fechar o Caixa com este Usuário.";
                    return;
                }

                decimal faturamento = Convert.ToDecimal(txtDinheiroDeposito.Text) +
                                      Convert.ToDecimal(txtChequeDeposito.Text) +
                                      Convert.ToDecimal(txtChequePreRetorno.Text) +
                                      Convert.ToDecimal(txtCartaoRetorno.Text);

                decimal diferenca = faturamento - Convert.ToDecimal(txtComanda.Text);

                if ((diferenca < -1 || diferenca > 1))
                {
                    LabelFeedBack.Text = "Valor comanda diferente do faturamento !!!";

                    return;
                }

                if (Convert.ToDecimal(txtRetirada.Text) > Convert.ToDecimal(txtDinheiroDeposito.Text))
                {
                    LabelFeedBack.Text = "Valor da Retirada maior que o Valor em Dinheiro !!!";

                    return;
                }

                if (Session["SaldoRetirada"] != null)
                {
                    decimal saldoRetirada = (decimal)Session["SaldoRetirada"];

                    if (Convert.ToDecimal(txtRetirada.Text) > saldoRetirada & Convert.ToDecimal(txtRetirada.Text) > 0)
                    {
                        LabelFeedBack.Text = "Valor da Retirada maior que 0 e maior que Saldo da Retirada !!!";

                        return;
                    }
                }

                FILIAI filial = baseController.BuscaFilial(txtFilial.Text);

                if (filial != null)
                {
                    Session["CODIGO_FILIAL"] = filial.COD_FILIAL;

                    List<FECHAMENTO_CAIXA> contCaixa = baseController.ExisteMovimentoCaixa(CalendarDataFechamento.SelectedDate, Convert.ToInt32(filial.COD_FILIAL));

                    if (contCaixa.Count > 0)
                    {
                        LabelFeedBack.Text = "Movimento de Caixa já Transmitido !!!";

                        return;
                    }

                    DateTime dataAtual = DateTime.Now;

                    TimeSpan date = dataAtual - CalendarDataFechamento.SelectedDate;

                    int totalDias = date.Days;

                    if (dataAtual != null)
                    {
                        if (Convert.ToDecimal(txtRetirada.Text) > 0 & totalDias > 1)
                        {
                            LabelFeedBack.Text = "Fechamento c/ data anterior não permite retirada p/ fundo Fixo !!!";

                            return;
                        }
                    }
                }
                else
                {
                    LabelFeedBack.Text = "Filial não Cadastrada !!!";

                    return;
                }

                decimal cheque = 0;

                cheque = Convert.ToDecimal(txtChequeDeposito.Text) + Convert.ToDecimal(txtChequePreRetorno.Text);

                txtDinheiroVenda.Text = txtDinheiroDeposito.Text;
                txtChequeVenda.Text = cheque.ToString();
                txtCartaoVenda.Text = txtCartaoRetorno.Text;
                txtTotal.Text = (Convert.ToDecimal(txtDinheiroVenda.Text) + Convert.ToDecimal(txtChequeVenda.Text) + Convert.ToDecimal(txtCartaoVenda.Text)).ToString();

                if (Convert.ToDecimal(txtTotal.Text) > Convert.ToDecimal(txtComanda.Text))
                    txtSobra.Text = (Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtComanda.Text)).ToString();
                if (Convert.ToDecimal(txtTotal.Text) < Convert.ToDecimal(txtComanda.Text))
                    txtFalta.Text = (Convert.ToDecimal(txtComanda.Text) - Convert.ToDecimal(txtTotal.Text)).ToString();

                if (txtRetirada.Text.Equals(""))
                    txtRetirada.Text = "0";

                txtDinheiroDeposito.Text = (Convert.ToDecimal(txtDinheiroDeposito.Text) - (Convert.ToDecimal(txtRetirada.Text) + Convert.ToDecimal(txtDevolucao.Text))).ToString();

                ButtonEnviar.Enabled = true;

                txtDinheiroDeposito.Enabled = false;
                txtChequeDeposito.Enabled = false;
                txtChequePreRetorno.Enabled = false;
                txtCartaoRetorno.Enabled = false;
                txtComanda.Enabled = false;
                txtRetirada.Enabled = false;
                txtDevolucao.Enabled = false;

            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = ex.Message;
            }
        }

        protected void ButtonEnviar_Click(object sender, EventArgs e)
        {

            LabelFeedBack.Text = "";
            try
            {
                //Obter Usuario
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario == null || usuario.CODIGO_USUARIO <= 0)
                {
                    LabelFeedBack.Text = "Sessão expirada. Por favor, faça o LOGIN novamente.";
                    return;
                }

                //Validar perfil do usuario, nao pode ser com senha de supervisor
                if (!ValidarPerfilCaixa())
                {
                    LabelFeedBack.Text = "Não é permitido Fechar o Caixa com este Usuário.";
                    return;
                }

                FECHAMENTO_CAIXA caixa = new FECHAMENTO_CAIXA();
                caixa.CODIGO_FILIAL = Convert.ToInt32(Session["CODIGO_FILIAL"]);
                caixa.DATA_FECHAMENTO = CalendarDataFechamento.SelectedDate;
                caixa.VALOR_DINHEIRO = Convert.ToDecimal(txtDinheiroVenda.Text);
                caixa.VALOR_CHEQUE = Convert.ToDecimal(txtChequeDeposito.Text);
                caixa.VALOR_CHEQUE_PRE = Convert.ToDecimal(txtChequePreRetorno.Text);
                caixa.VALOR_CARTAO_CREDITO = Convert.ToDecimal(txtCartaoRetorno.Text);
                caixa.VALOR_COMANDA = Convert.ToDecimal(txtComanda.Text);
                caixa.VALOR_RETIRADA = Convert.ToDecimal(txtRetirada.Text);
                caixa.VALOR_DEVOLUCAO = Convert.ToDecimal(txtDevolucao.Text);
                caixa.RESPONSAVEL = txtResponsavel.Text;
                caixa.VISA = Convert.ToDecimal(txtVisa.Text);
                caixa.V_CRED_VISTA = Convert.ToDecimal(txtVisaCredVista.Text);
                caixa.V_DEB_VISTA = Convert.ToDecimal(txtVisaDebVista.Text);
                caixa.V_PARC_ADMIN = Convert.ToDecimal(txtVisaParcAdmin.Text);
                caixa.V_PARC_ESTAB_2X = Convert.ToDecimal(txtVisaParcEstab2x.Text);
                caixa.V_PARC_ESTAB_3X = Convert.ToDecimal(txtVisaParcEstab3x.Text);
                caixa.V_PARC_ESTAB_4X = Convert.ToDecimal(txtVisaParcEstab4x.Text);
                caixa.V_PARC_ESTAB_5X = Convert.ToDecimal(txtVisaParcEstab5x.Text);
                caixa.V_PARC_ESTAB_6x = Convert.ToDecimal(txtVisaParcEstab6x.Text);
                caixa.V_PARC_ESTAB_7X = Convert.ToDecimal(txtVisaParcEstab7x.Text);
                caixa.V_PARC_ESTAB_8X = Convert.ToDecimal(txtVisaParcEstab8x.Text);
                caixa.V_PARC_ESTAB_9X = Convert.ToDecimal(txtVisaParcEstab9x.Text);
                caixa.V_PARC_ESTAB_10X = Convert.ToDecimal(txtVisaParcEstab10x.Text);
                caixa.MASTERCARD = Convert.ToDecimal(txtMaster.Text);
                caixa.M_CRED_VISTA = Convert.ToDecimal(txtMasterCredVista.Text);
                caixa.M_DEB_VISTA = Convert.ToDecimal(txtMasterDebVista.Text);
                caixa.M_PARC_ADMIN = Convert.ToDecimal(txtMasterParcAdmin.Text);
                caixa.M_PARC_ESTAB_2X = Convert.ToDecimal(txtMasterParcEstab2x.Text);
                caixa.M_PARC_ESTAB_3X = Convert.ToDecimal(txtMasterParcEstab3x.Text);
                caixa.M_PARC_ESTAB_4X = Convert.ToDecimal(txtMasterParcEstab4x.Text);
                caixa.M_PARC_ESTAB_5X = Convert.ToDecimal(txtMasterParcEstab5x.Text);
                caixa.M_PARC_ESTAB_6X = Convert.ToDecimal(txtMasterParcEstab6x.Text);
                caixa.M_PARC_ESTAB_7X = Convert.ToDecimal(txtMasterParcEstab7x.Text);
                caixa.M_PARC_ESTAB_8X = Convert.ToDecimal(txtMasterParcEstab8x.Text);
                caixa.M_PARC_ESTAB_9X = Convert.ToDecimal(txtMasterParcEstab9x.Text);
                caixa.M_PARC_ESTAB_10X = Convert.ToDecimal(txtMasterParcEstab10x.Text);
                caixa.AMERICAN = Convert.ToDecimal(txtAmex.Text);
                caixa.A_CRED_VISTA = Convert.ToDecimal(txtAmexCredVista.Text);
                caixa.A_DEB_VISTA = Convert.ToDecimal(txtAmexDebVista.Text);
                caixa.A_PARC_ADMIN = Convert.ToDecimal(txtAmexParcAdmin.Text);
                caixa.A_PARC_ESTAB_2X = Convert.ToDecimal(txtAmexParcEstab2x.Text);
                caixa.A_PARC_ESTAB_3X = Convert.ToDecimal(txtAmexParcEstab3x.Text);
                caixa.A_PARC_ESTAB_4X = Convert.ToDecimal(txtAmexParcEstab4x.Text);
                caixa.A_PARC_ESTAB_5X = Convert.ToDecimal(txtAmexParcEstab5x.Text);
                caixa.A_PARC_ESTAB_6X = Convert.ToDecimal(txtAmexParcEstab6x.Text);
                caixa.A_PARC_ESTAB_7X = Convert.ToDecimal(txtAmexParcEstab7x.Text);
                caixa.A_PARC_ESTAB_8X = Convert.ToDecimal(txtAmexParcEstab8x.Text);
                caixa.A_PARC_ESTAB_9X = Convert.ToDecimal(txtAmexParcEstab9x.Text);
                caixa.A_PARC_ESTAB_10X = Convert.ToDecimal(txtAmexParcEstab10x.Text);
                caixa.HIPERCARD = Convert.ToDecimal(txtHiper.Text);
                caixa.H_CRED_VISTA = Convert.ToDecimal(txtHiperCredVista.Text);
                caixa.H_DEB_VISTA = Convert.ToDecimal(txtHiperDebVista.Text);
                caixa.H_PARC_ADMIN = Convert.ToDecimal(txtHiperParcAdmin.Text);
                caixa.H_PARC_ESTAB_2X = Convert.ToDecimal(txtHiperParcEstab2x.Text);
                caixa.H_PARC_ESTAB_3X = Convert.ToDecimal(txtHiperParcEstab3x.Text);
                caixa.H_PARC_ESTAB_4X = Convert.ToDecimal(txtHiperParcEstab4x.Text);
                caixa.H_PARC_ESTAB_5X = Convert.ToDecimal(txtHiperParcEstab5x.Text);
                caixa.H_PARC_ESTAB_6X = Convert.ToDecimal(txtHiperParcEstab6x.Text);
                caixa.H_PARC_ESTAB_7X = Convert.ToDecimal(txtHiperParcEstab7x.Text);
                caixa.H_PARC_ESTAB_8X = Convert.ToDecimal(txtHiperParcEstab8x.Text);
                caixa.H_PARC_ESTAB_9X = Convert.ToDecimal(txtHiperParcEstab9x.Text);
                caixa.H_PARC_ESTAB_10X = Convert.ToDecimal(txtHiperParcEstab10x.Text);
                caixa.OUTROS_CARTOES = Convert.ToDecimal(txtOutrosCartoes.Text);
                caixa.OBS = txtObsGeral.Text;
                caixa.VALOR_DESPESAS = 0;
                caixa.USUARIO_FECHAMENTO = usuario.CODIGO_USUARIO;

                usuarioController.InsereCaixa(caixa);

                AtualizaFundoFixo(caixa);

                try
                {

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(usuarioController.BuscaPorCodigoUsuario(usuario.CODIGO_USUARIO).EMAIL);
                    mail.To.Add(usuarioController.BuscaPorCodigoUsuario(usuario.CODIGO_USUARIO).EMAIL);
                    mail.Bcc.Add(usuarioController.BuscaPorCodigoUsuario(usuario.CODIGO_USUARIO).EMAIL);
                    mail.Subject = "Confirmação de Envio de Fechamento de Caixa";
                    //mail.Attachments.Add(new Attachment(nomeArquivo));

                    string msg = "Email de Confirmação de Envio do Fechamento de Caixa no dia " + CalendarDataFechamento.SelectedDate.ToString("dd/MM/yyyy") + " pelo usuário: " + usuario.NOME_USUARIO + ".";
                    mail.Body = msg;
                    //mail.BodyEncoding = System.Text.Encoding.Default;

                    //mail.ReplyTo = new MailAddress(this.EmailUsuario);

                    SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
                    smtp.Port = 587;
                    smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

                    smtp.Send(mail);

                    Response.Redirect("~/FinalizadoComSucesso.aspx");
                }
                catch (Exception ex)
                {
                    LabelFeedBack.Text = "Erro : " + ex.Message;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void AtualizaFundoFixo(FECHAMENTO_CAIXA caixa)
        {
            if (caixa != null)
            {
                // Gravar Receita
                if (caixa.VALOR_RETIRADA > 0)
                {
                    //Atualizar receita (Valor da Retirada)
                    usuarioController.InsereHistoricoReceita(caixa);
                }

                // Gravar Deposito
                usuarioController.InsereHistoricoDeposito(caixa);
            }
        }

        private void ZerarValores()
        {
            txtDinheiroDeposito.Text = "0";
            txtChequeDeposito.Text = "0";
            txtChequePreRetorno.Text = "0";
            txtCartaoRetorno.Text = "0";
            txtComanda.Text = "0";
            txtRetirada.Text = "0";
            txtDevolucao.Text = "0";
            txtVisa.Text = "0";
            txtVisaCredVista.Text = "0";
            txtVisaDebVista.Text = "0";
            txtVisaParcAdmin.Text = "0";
            txtVisaParcEstab2x.Text = "0";
            txtVisaParcEstab3x.Text = "0";
            txtVisaParcEstab4x.Text = "0";
            txtVisaParcEstab5x.Text = "0";
            txtVisaParcEstab6x.Text = "0";
            txtVisaParcEstab7x.Text = "0";
            txtVisaParcEstab8x.Text = "0";
            txtVisaParcEstab9x.Text = "0";
            txtVisaParcEstab10x.Text = "0";
            txtMaster.Text = "0";
            txtMasterCredVista.Text = "0";
            txtMasterDebVista.Text = "0";
            txtMasterParcAdmin.Text = "0";
            txtMasterParcEstab2x.Text = "0";
            txtMasterParcEstab3x.Text = "0";
            txtMasterParcEstab4x.Text = "0";
            txtMasterParcEstab5x.Text = "0";
            txtMasterParcEstab6x.Text = "0";
            txtMasterParcEstab7x.Text = "0";
            txtMasterParcEstab8x.Text = "0";
            txtMasterParcEstab9x.Text = "0";
            txtMasterParcEstab10x.Text = "0";
            txtAmex.Text = "0";
            txtAmexCredVista.Text = "0";
            txtAmexDebVista.Text = "0";
            txtAmexParcAdmin.Text = "0";
            txtAmexParcEstab2x.Text = "0";
            txtAmexParcEstab3x.Text = "0";
            txtAmexParcEstab4x.Text = "0";
            txtAmexParcEstab5x.Text = "0";
            txtAmexParcEstab6x.Text = "0";
            txtAmexParcEstab7x.Text = "0";
            txtAmexParcEstab8x.Text = "0";
            txtAmexParcEstab9x.Text = "0";
            txtAmexParcEstab10x.Text = "0";
            txtHiper.Text = "0";
            txtHiperCredVista.Text = "0";
            txtHiperDebVista.Text = "0";
            txtHiperParcAdmin.Text = "0";
            txtHiperParcEstab2x.Text = "0";
            txtHiperParcEstab3x.Text = "0";
            txtHiperParcEstab4x.Text = "0";
            txtHiperParcEstab5x.Text = "0";
            txtHiperParcEstab6x.Text = "0";
            txtHiperParcEstab7x.Text = "0";
            txtHiperParcEstab8x.Text = "0";
            txtHiperParcEstab9x.Text = "0";
            txtHiperParcEstab10x.Text = "0";
            txtOutrosCartoes.Text = "0";
        }

        private bool ValidarPerfilCaixa()
        {
            USUARIO _usuario = ((USUARIO)Session["USUARIO"]);
            if (_usuario == null)
                return false;

            if (_usuario.CODIGO_PERFIL == 3)
                return false;
            return true;
        }

    }
}