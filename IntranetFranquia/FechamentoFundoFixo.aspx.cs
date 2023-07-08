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
    public partial class FechamentoFundoFixo : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    USUARIO usuario = (USUARIO)Session["USUARIO"];

                    if (usuario != null)
                    {
                        string codigoPerfil = usuario.CODIGO_PERFIL.ToString();
                        if (codigoPerfil != "2" && codigoPerfil != "3")
                        {
                            labTitulo.Text = "Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento Fundo Fixo";
                            hrefVoltar.HRef = "DefaultFinanceiro.aspx";
                        }

                        USUARIOLOJA usuarioLoja = baseController.BuscaUsuarioLoja(usuario.CODIGO_USUARIO);

                        if (usuarioLoja != null)
                        {
                            txtFilial.Text = baseController.BuscaFilialCodigoInt(Convert.ToInt32(usuarioLoja.CODIGO_LOJA)).FILIAL;

                            CarregaDropDownListContas();
                            CarregaGridViewDespesas();
                        }
                        else
                        {
                            LabelFeedBack.Text = "Usuário não tem loja vinculada !!!";

                            return;
                        }
                    }
                }
            }
        }

        private void CarregaGridViewDespesas()
        {
            FILIAI filial = baseController.BuscaFilial(txtFilial.Text);

            if (filial != null)
            {
                List<FUNDO_FIXO_HISTORICO_DESPESA> listaHistoricoDespesa = baseController.BuscaDespesasEmAberto(Convert.ToInt32(filial.COD_FILIAL));

                if (listaHistoricoDespesa != null)
                {
                    GridViewDespesa.DataSource = listaHistoricoDespesa;
                    GridViewDespesa.DataBind();

                    decimal totalDespesas = 0;

                    foreach (FUNDO_FIXO_HISTORICO_DESPESA itemDespesas in listaHistoricoDespesa)
                    {
                        totalDespesas += itemDespesas.VALOR;
                    }

                    FUNDO_FIXO_HISTORICO_FECHAMENTO ultimoFechamento = baseController.BuscaUltimoFechamento(Convert.ToInt32(filial.COD_FILIAL));

                    if (ultimoFechamento != null)
                    {
                        List<FUNDO_FIXO_HISTORICO_RECEITA> receitas = baseController.BuscaReceitasFundoFixo(Convert.ToInt32(filial.COD_FILIAL), ultimoFechamento.DATA, DateTime.Now);

                        if (receitas != null)
                        {
                            decimal totalReceitas = 0;

                            foreach (FUNDO_FIXO_HISTORICO_RECEITA itemReceita in receitas)
                            {
                                totalReceitas += itemReceita.VALOR;
                            }

                            txtSaldoInicial.Text = (ultimoFechamento.SALDO_ATUAL + totalReceitas).ToString("#,##0.00");
                            txtTotalDespesas.Text = totalDespesas.ToString("#,##0.00");
                            txtSaldoFinal.Text = ((ultimoFechamento.SALDO_ATUAL + totalReceitas) - totalDespesas).ToString("#,##0.00");
                        }
                    }
                }
            }
        }

        private void CarregaDropDownListContas()
        {
            List<FUNDO_FIXO_CONTA_DESPESA> contaDespesa = new List<FUNDO_FIXO_CONTA_DESPESA>();

            contaDespesa = baseController.BuscaContas().OrderBy(p => p.DESCRICAO).ToList();
            contaDespesa.Insert(0, new FUNDO_FIXO_CONTA_DESPESA { CODIGO = "", DESCRICAO = "Selecione" });
            ddlContas.DataSource = contaDespesa;
            ddlContas.DataBind();
        }

        protected void ddlContas_DataBound(object sender, EventArgs e)
        {
        }

        private void CarregaHistoricoDespesas(int codigoDespesa)
        {
            FUNDO_FIXO_HISTORICO_DESPESA despesa = baseController.BuscaDespesaPorCodigo(codigoDespesa);

            if (despesa != null)
            {
                ddlContas.SelectedValue = despesa.CODIGO_DESPESA_LOJA.ToString();
                txtValor.Text = despesa.VALOR.ToString();

                HiddenFieldCodigoDespesa.Value = despesa.CODIGO.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (ddlContas.SelectedValue.ToString().Equals("0") ||
                ddlContas.SelectedValue.ToString().Equals("") ||
                ddlContas.SelectedValue.ToString().Equals("0") ||
                ddlContas.SelectedValue.ToString().Equals("") ||
                txtValor.Text.Equals(""))
                return;

            FILIAI filial = baseController.BuscaFilial(txtFilial.Text);

            if (filial != null)
            {
                List<FUNDO_FIXO_HISTORICO_DESPESA> listaHistoricoDespesa = baseController.BuscaDespesasEmAberto(Convert.ToInt32(filial.COD_FILIAL));

                if (listaHistoricoDespesa != null)
                {
                    decimal totalDespesas = 0;

                    foreach (FUNDO_FIXO_HISTORICO_DESPESA itemDespesas in listaHistoricoDespesa)
                    {
                        totalDespesas += itemDespesas.VALOR;
                    }

                    if ((totalDespesas + Convert.ToDecimal(txtValor.Text)) > Convert.ToDecimal(txtSaldoInicial.Text))
                    {
                        LabelFeedBack.Text = "Saldo de Despesas maior que Saldo Inicial !!!";

                        LimpaTela();

                        return;
                    }

                    try
                    {
                        FUNDO_FIXO_HISTORICO_DESPESA historicoDespesa;

                        if (Convert.ToInt32(HiddenFieldCodigoDespesa.Value) > 0)
                            historicoDespesa = baseController.BuscaDespesaPorCodigo(Convert.ToInt32(HiddenFieldCodigoDespesa.Value));
                        else
                            historicoDespesa = new FUNDO_FIXO_HISTORICO_DESPESA();

                        FUNDO_FIXO_CONTA_DESPESA contaDespesa = baseController.BuscaContaPorCodigoContaLoja(ddlContas.SelectedValue);

                        if (contaDespesa != null)
                        {
                            historicoDespesa.CODIGO_CONTA_DESPESA = contaDespesa.CODIGO;
                            historicoDespesa.DATA = DateTime.Now;
                            historicoDespesa.VALOR = Convert.ToDecimal(txtValor.Text);
                            historicoDespesa.CODIGO_FECHAMENTO = 0;
                            historicoDespesa.CODIGO_DESPESA_LOJA = 0;
                            historicoDespesa.CODIGO_FILIAL = Convert.ToInt32(filial.COD_FILIAL);
                            historicoDespesa.DESC_DESPESA = txtDescricaoDespesa.Text.ToUpper().Trim();

                            if (Convert.ToInt32(HiddenFieldCodigoDespesa.Value) > 0)
                                usuarioController.AtualizaHistoricoDespesas(historicoDespesa);
                            else
                                usuarioController.InsereHistoricoDespesas(historicoDespesa);

                            LabelFeedBack.Text = "Gravado com sucesso!";

                            CarregaGridViewDespesas();

                            LimpaTela();
                        }
                    }
                    catch (Exception ex)
                    {
                        LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
                    }
                }
            }
        }

        protected void GridViewDespesa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            FUNDO_FIXO_HISTORICO_DESPESA historicoDespesa = e.Row.DataItem as FUNDO_FIXO_HISTORICO_DESPESA;

            if (historicoDespesa != null)
            {
                Button buttonExcluirDespesa = e.Row.FindControl("ButtonExcluirDespesa") as Button;

                if (buttonExcluirDespesa != null)
                    buttonExcluirDespesa.CommandArgument = historicoDespesa.CODIGO.ToString();

                Literal literalDespesa = e.Row.FindControl("LiteralDespesa") as Literal;
                if (literalDespesa != null)
                {
                    FUNDO_FIXO_CONTA_DESPESA contaDespesa = baseController.BuscaContaPorCodigoContaLoja(historicoDespesa.CODIGO_CONTA_DESPESA);
                    if (contaDespesa != null)
                        literalDespesa.Text = contaDespesa.DESCRICAO.ToString();
                }

                Literal literalData = e.Row.FindControl("LiteralData") as Literal;
                if (literalData != null)
                    literalData.Text = historicoDespesa.DATA.Date.ToString("dd/MM/yyyy");

                Literal literalDespesaDescricao = e.Row.FindControl("LiteralDespesaDescricao") as Literal;
                if (literalDespesaDescricao != null)
                    literalDespesaDescricao.Text = historicoDespesa.DESC_DESPESA;

            }
        }

        protected void ButtonExcluirDespesa_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                usuarioController.ExcluiHistoricoDespesa(Convert.ToInt32(buttonExcluir.CommandArgument));

                CarregaGridViewDespesas();

                LimpaTela();
            }
        }

        protected void btFechar_Click(object sender, EventArgs e)
        {
            decimal saldoAnterior = 0;
            decimal totalDespesas = 0;
            decimal totalReceitas = 0;
            decimal saldoAtual = 0;

            Button btFechar = sender as Button;

            if (btFechar != null)
            {
                FILIAI filial = baseController.BuscaFilial(txtFilial.Text);

                if (filial != null)
                {
                    FUNDO_FIXO_HISTORICO_FECHAMENTO ultimoFechamento = baseController.BuscaUltimoFechamento(Convert.ToInt32(filial.COD_FILIAL));

                    if (ultimoFechamento == null)
                    {
                        FUNDO_FIXO_HISTORICO_FECHAMENTO historicoFechamentoInicial = new FUNDO_FIXO_HISTORICO_FECHAMENTO();

                        historicoFechamentoInicial.CODIGO_FILIAL = Convert.ToInt32(filial.COD_FILIAL);
                        historicoFechamentoInicial.DATA = DateTime.Now;
                        historicoFechamentoInicial.SALDO_ANTERIOR = 0;
                        historicoFechamentoInicial.SALDO_ATUAL = 0;

                        FUNDO_FIXO_CONTA_RECEITA fundoReceita = baseController.BuscaLimiteFundoFilial(Convert.ToInt32(filial.COD_FILIAL));

                        if (fundoReceita != null)
                        {
                            historicoFechamentoInicial.SALDO_ATUAL = (fundoReceita.VALOR_FUNDO_LIMITE == null) ? Convert.ToDecimal(fundoReceita.VALOR_FUNDO) : Convert.ToDecimal(fundoReceita.VALOR_FUNDO_LIMITE);
                        }

                        historicoFechamentoInicial.TOTAL_CREDITO = 0;
                        historicoFechamentoInicial.TOTAL_DEBITO = 0;
                        historicoFechamentoInicial.CODIGO_BAIXA = 2; //A primeira vez já insere como CONFERIDO

                        usuarioController.InsereHistoricoFechamento(historicoFechamentoInicial);

                        Response.Redirect("~/FundoFixoInicializado.aspx");
                    }
                    else
                        saldoAnterior = ultimoFechamento.SALDO_ATUAL;

                    List<FUNDO_FIXO_HISTORICO_RECEITA> receitas = baseController.BuscaReceitasFundoFixo(Convert.ToInt32(filial.COD_FILIAL), ultimoFechamento.DATA, DateTime.Now);

                    if (receitas != null)
                    {
                        foreach (FUNDO_FIXO_HISTORICO_RECEITA itemReceita in receitas)
                        {
                            totalReceitas += itemReceita.VALOR;
                        }
                    }

                    List<FUNDO_FIXO_HISTORICO_DESPESA> listaHistoricoDespesa = baseController.BuscaDespesasEmAberto(Convert.ToInt32(filial.COD_FILIAL));

                    if (listaHistoricoDespesa != null)
                    {
                        foreach (FUNDO_FIXO_HISTORICO_DESPESA itemDespesas in listaHistoricoDespesa)
                        {
                            totalDespesas += itemDespesas.VALOR;
                        }
                    }

                    saldoAtual = (saldoAnterior + totalReceitas) - totalDespesas;

                    FUNDO_FIXO_HISTORICO_FECHAMENTO historicoFechamento = new FUNDO_FIXO_HISTORICO_FECHAMENTO();

                    historicoFechamento.CODIGO_FILIAL = Convert.ToInt32(filial.COD_FILIAL);
                    historicoFechamento.DATA = DateTime.Now;
                    historicoFechamento.SALDO_ANTERIOR = saldoAnterior;
                    historicoFechamento.SALDO_ATUAL = saldoAtual;
                    historicoFechamento.TOTAL_CREDITO = totalReceitas;
                    historicoFechamento.TOTAL_DEBITO = totalDespesas;
                    historicoFechamento.CODIGO_BAIXA = 1; //Insere o fechamento como EM ABERTO

                    usuarioController.InsereHistoricoFechamento(historicoFechamento);

                    ultimoFechamento = baseController.BuscaUltimoFechamento(Convert.ToInt32(filial.COD_FILIAL));

                    if (ultimoFechamento != null)
                    {
                        if (listaHistoricoDespesa != null)
                        {
                            foreach (FUNDO_FIXO_HISTORICO_DESPESA item in listaHistoricoDespesa)
                            {
                                item.CODIGO_FECHAMENTO = ultimoFechamento.CODIGO;

                                usuarioController.AtualizaHistoricoDespesas(item);
                            }
                        }

                        if (receitas != null)
                        {
                            foreach (FUNDO_FIXO_HISTORICO_RECEITA item in receitas)
                            {
                                item.CODIGO_FECHAMENTO = ultimoFechamento.CODIGO;

                                usuarioController.AtualizaHistoricoReceitas(item);
                            }
                        }
                    }

                    USUARIO usuario = (USUARIO)Session["USUARIO"];

                    if (usuario != null)
                    {
                        string nomeArquivo;

                        try
                        {
                            string data = baseController.BuscaDataBanco().data.ToString("dd") + baseController.BuscaDataBanco().data.ToString("MM") + baseController.BuscaDataBanco().data.ToString("yyyy");
                            string hora = baseController.BuscaDataBanco().data.ToString("hh") + baseController.BuscaDataBanco().data.ToString("mm") + baseController.BuscaDataBanco().data.ToString("ss");

                            nomeArquivo = "C:\\bkp\\" + txtFilial.Text.Trim() + "_" + data + "_" + hora + ".txt";

                            string codigoControle = txtFilial.Text.Trim() + "_" + data;

                            Session["CodigoControle"] = codigoControle;

                            if (System.IO.File.Exists(nomeArquivo))
                                System.IO.File.Delete(nomeArquivo);

                            System.IO.File.Create(nomeArquivo).Close();
                            System.IO.TextWriter arquivo = System.IO.File.AppendText(nomeArquivo);

                            arquivo.WriteLine("Fechamento de Fundo Fixo - " + txtFilial.Text);
                            arquivo.WriteLine("");
                            arquivo.WriteLine("Saldo Anterior : " + saldoAnterior.ToString());
                            arquivo.WriteLine("");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("Retiradas:");

                            decimal totalRetirada = 0;

                            foreach (FUNDO_FIXO_HISTORICO_RECEITA itemReceita in receitas)
                            {
                                arquivo.WriteLine(itemReceita.CODIGO_CONTA_RECEITA + " " + itemReceita.DATA + " " + itemReceita.VALOR);

                                totalRetirada += itemReceita.VALOR;
                            }

                            arquivo.WriteLine("");
                            arquivo.WriteLine("Total das Retiradas : " + totalRetirada);

                            arquivo.WriteLine("");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("Despesas:");

                            decimal totalDespesa = 0;

                            foreach (FUNDO_FIXO_HISTORICO_DESPESA itemDespesa in listaHistoricoDespesa)
                            {
                                arquivo.WriteLine(itemDespesa.CODIGO_CONTA_DESPESA + " " + itemDespesa.DATA + " " + itemDespesa.VALOR + ((itemDespesa.DESC_DESPESA == null || itemDespesa.DESC_DESPESA.Trim() == "") ? "" : (" - " + itemDespesa.DESC_DESPESA)));

                                totalDespesa += itemDespesa.VALOR;
                            }

                            arquivo.WriteLine("");
                            arquivo.WriteLine("Total das Despesas : " + totalDespesa);

                            arquivo.WriteLine("");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("Saldo Atual : " + saldoAtual.ToString());

                            arquivo.Close();
                            arquivo.Dispose();
                        }
                        catch (Exception ex)
                        {
                            LabelFeedBack.Text = "Erro : " + ex.Message;
                            return;
                        }

                        try
                        {

                            //MailMessage mail = new MailMessage();
                            //mail.From = new MailAddress(usuarioController.BuscaPorCodigoUsuario(usuario.CODIGO_USUARIO).EMAIL);
                            ////mail.To.Add("luana.lima@hbf.com.br");
                            ////mail.To.Add("selma.barreto@hbf.com.br");
                            ////mail.To.Add("leandro.bevilaqua@hbf.com.br");
                            //mail.Subject = "Fechamento de Despesas";
                            //mail.Attachments.Add(new Attachment(nomeArquivo));

                            //string msg = "Arquivo em anexo enviado pela filial: " + usuario.NOME_USUARIO;
                            //mail.Body = msg;
                            ////mail.BodyEncoding = System.Text.Encoding.Default;

                            ////mail.ReplyTo = new MailAddress(this.EmailUsuario);

                            //SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
                            //smtp.Port = 587;
                            //smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

                            //smtp.Send(mail);

                            Response.Redirect("~/FundoFixoEncerrado.aspx");
                        }
                        catch (Exception ex)
                        {
                            LabelFeedBack.Text = "Erro : " + ex.Message;
                            return;
                        }
                    }
                }
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            HiddenFieldCodigoDespesa.Value = "0";
            txtValor.Text = string.Empty;
            txtDescricaoDespesa.Text = "";

        }
    }
}