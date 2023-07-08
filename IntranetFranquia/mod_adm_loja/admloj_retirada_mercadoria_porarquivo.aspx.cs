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
using System.Text;
using System.IO;
using System.Collections;

namespace Relatorios
{
    public partial class admloj_retirada_mercadoria_porarquivo : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarFilial();
                CriarIdTransferencia();
                CarregarItens(hidCodigoTransferencia.Value);

                dialogPai.Visible = false;
            }

            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {


            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais_Intermediario();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "1029", FILIAL = "ATACADO HANDBOOK" });
                filial.Insert(2, new FILIAI { COD_FILIAL = "000041", FILIAL = "CD MOSTRUARIO" });
                filial.Insert(3, new FILIAI { COD_FILIAL = "000029", FILIAL = "CD - LUGZY" });
                filial.Insert(4, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE" });
                filial.Insert(5, new FILIAI { COD_FILIAL = "000003", FILIAL = "CD CRIARI" });

                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }

        }
        private void CriarIdTransferencia()
        {

            if (Session["USUARIO"] == null)
            {
                labErro.Text = "SESSÃO EXPIRADA. FAÇA O LOGIN NOVAMENTE.";
                return;
            }

            var usuario = (USUARIO)Session["USUARIO"];

            var codigoUsuario = usuario.CODIGO_USUARIO.ToString();
            int codigoTransferencia = 0;
            var pt = baseController.ObterProdutoTransferencia(codigoUsuario).Where(p => p.DATA_ENVIO == null && (p.SOL_POR_ARQUIVO == null || p.SOL_POR_ARQUIVO == false)).FirstOrDefault();
            if (pt == null)
            {
                PRODUTO_TRANSFERENCIA ptNovo = new PRODUTO_TRANSFERENCIA();
                ptNovo.CODIGO_FILIAL = codigoUsuario;
                ptNovo.QTDE_ITEM = 0;
                ptNovo.VOLUME = 0;
                ptNovo.USUARIO_INCLUSAO = usuario.CODIGO_USUARIO;
                ptNovo.DATA_INCLUSAO = DateTime.Now;
                ptNovo.PERMUTA = 'N';
                ptNovo.MOTIVO = 'R';
                ptNovo.SOL_POR_ARQUIVO = false;

                codigoTransferencia = baseController.InserirProdutoTransferencia(ptNovo);
            }
            else
            {
                codigoTransferencia = pt.CODIGO;
            }

            hidCodigoTransferencia.Value = codigoTransferencia.ToString();
        }
        #endregion

        protected void btImportar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            labErroImport.Text = "";

            if (ddlFilial.SelectedValue == "")
            {
                labErro.Text = "Selecione uma Filial.";
                return;
            }

            //Validar arquivo inserido
            if (!uploadArquivoProduto.HasFile)
            {
                labErro.Text = "Selecione um arquivo de produto .TXT.";
                return;
            }

            //Validar Tamanho do Arquivo
            if (uploadArquivoProduto.PostedFile.ContentLength <= 0)
            {
                labErro.Text = "Selecione um arquivo com o tamanho maior que zero.";
                return;
            }

            //Validar Extensão
            string fileExtension = System.IO.Path.GetExtension(uploadArquivoProduto.PostedFile.FileName);
            if (fileExtension.ToUpper() != ".TXT")
            {
                labErro.Text = "Selecione um arquivo com extensão \".txt\".";
                return;
            }


            int totalLinha = 0;
            try
            {
                labErroImport.Text = "Carregando arquivo...";


                using (StreamReader reader = new StreamReader(uploadArquivoProduto.PostedFile.InputStream))
                {

                    int qtdeCount = 0;
                    while (!reader.EndOfStream)
                    {
                        var linha = reader.ReadLine();
                        var valores = linha.Split(',');

                        //var produto = valores[0].Substring(0, 5);
                        //var cor = valores[0].Substring(5, valores[0].Substring(5).Length - 2);
                        //var tamanho = valores[0].Substring(valores[0].Length - 2, 2);

                        qtdeCount = Convert.ToInt32(valores[1]);

                        for (int i = 1; i <= qtdeCount; i++)
                        {
                            var codigoBarraProduto = valores[0].Trim();

                            var produtoBarra = baseController.BuscaProdutoBarra(codigoBarraProduto);

                            if (produtoBarra != null)
                            {
                                var produtoTransferenciaItem = new PRODUTO_TRANSFERENCIA_ITEM();
                                produtoTransferenciaItem.CODIGO_BARRA = produtoBarra.CODIGO_BARRA;
                                produtoTransferenciaItem.COR = produtoBarra.COR_PRODUTO.Trim();
                                produtoTransferenciaItem.PRODUTO = produtoBarra.PRODUTO.Trim();

                                var p = baseController.BuscaProduto(produtoTransferenciaItem.PRODUTO);
                                produtoTransferenciaItem.NOME = p.DESC_PRODUTO;
                                produtoTransferenciaItem.COLECAO = p.COLECAO;

                                produtoTransferenciaItem.TAMANHO = produtoBarra.GRADE;

                                produtoTransferenciaItem.PRODUTO_TRANSFERENCIA = Convert.ToInt32(hidCodigoTransferencia.Value);
                                produtoTransferenciaItem.DATA_INCLUSAO = DateTime.Now;
                                produtoTransferenciaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;




                                var saldo = baseController.ObterSaldoLojaDevInterno(produtoBarra.PRODUTO, produtoBarra.COR_PRODUTO, Convert.ToInt32(produtoBarra.TAMANHO), ddlFilial.SelectedItem.Text.Trim(), produtoTransferenciaItem.USUARIO_INCLUSAO.ToString());

                                if (saldo == null)
                                {
                                    //Excluir();
                                    //labErro.Text = "O produto " + codigoBarraProduto + " não tem saldo de devolução na Filial selecionada.";
                                    //return;

                                    produtoTransferenciaItem.FILIAL = ddlFilial.SelectedItem.Text.Trim();
                                    produtoTransferenciaItem.FILIAL_ORIGEM = "SEM SALDO";
                                    produtoTransferenciaItem.NF_ENTRADA = "999999";
                                    produtoTransferenciaItem.SERIE_NF_ENTRADA = "99";
                                    produtoTransferenciaItem.ITEM_IMPRESSAO = "";
                                    produtoTransferenciaItem.CUSTO = 0;
                                    produtoTransferenciaItem.PEDIDO = "";

                                }
                                else
                                {
                                    produtoTransferenciaItem.FILIAL = saldo.FILIAL;
                                    produtoTransferenciaItem.FILIAL_ORIGEM = saldo.FILIAL_ORIGEM;
                                    produtoTransferenciaItem.NF_ENTRADA = saldo.NF_ENTRADA;
                                    produtoTransferenciaItem.SERIE_NF_ENTRADA = saldo.SERIE_NF_ENTRADA;
                                    produtoTransferenciaItem.ITEM_IMPRESSAO = saldo.ITEM_IMPRESSAO;
                                    produtoTransferenciaItem.CUSTO = saldo.CUSTO;
                                    produtoTransferenciaItem.PEDIDO = saldo.PEDIDO;
                                }

                                produtoTransferenciaItem.ENL1 = (produtoBarra.TAMANHO == 1) ? 1 : 0;
                                produtoTransferenciaItem.ENL2 = (produtoBarra.TAMANHO == 2) ? 1 : 0;
                                produtoTransferenciaItem.ENL3 = (produtoBarra.TAMANHO == 3) ? 1 : 0;
                                produtoTransferenciaItem.ENL4 = (produtoBarra.TAMANHO == 4) ? 1 : 0;
                                produtoTransferenciaItem.ENL5 = (produtoBarra.TAMANHO == 5) ? 1 : 0;
                                produtoTransferenciaItem.ENL6 = (produtoBarra.TAMANHO == 6) ? 1 : 0;
                                produtoTransferenciaItem.ENL7 = (produtoBarra.TAMANHO == 7) ? 1 : 0;
                                produtoTransferenciaItem.ENL8 = (produtoBarra.TAMANHO == 8) ? 1 : 0;
                                produtoTransferenciaItem.ENL9 = (produtoBarra.TAMANHO == 9) ? 1 : 0;
                                produtoTransferenciaItem.ENL10 = (produtoBarra.TAMANHO == 10) ? 1 : 0;
                                produtoTransferenciaItem.ENL11 = (produtoBarra.TAMANHO == 11) ? 1 : 0;
                                produtoTransferenciaItem.ENL12 = (produtoBarra.TAMANHO == 12) ? 1 : 0;
                                produtoTransferenciaItem.ENL13 = (produtoBarra.TAMANHO == 13) ? 1 : 0;
                                produtoTransferenciaItem.ENL14 = (produtoBarra.TAMANHO == 14) ? 1 : 0;
                                produtoTransferenciaItem.ENL15 = (produtoBarra.TAMANHO == 15) ? 1 : 0;
                                produtoTransferenciaItem.ENL16 = (produtoBarra.TAMANHO == 16) ? 1 : 0;

                                baseController.InserirProdutoTransferenciaItem(produtoTransferenciaItem);
                            }

                            totalLinha += 1;
                        }

                        qtdeCount = 0;
                    }
                }

                labErroImport.Text = "Arquivo importado com sucesso.";

                CarregarItens(hidCodigoTransferencia.Value);

            }
            catch (Exception ex)
            {
                if (totalLinha > 0)
                    labErroImport.Text = "Erro ao importar arquivo: Linha " + totalLinha.ToString();
                else
                    labErroImport.Text = ex.Message;

                Excluir();
            }
            finally
            {
            }

        }

        #region "PRODUTO"

        private void CarregarItens(string codigoTransferencia)
        {
            var listaItem = baseController.ObterProdutoTransferenciaItem(codigoTransferencia);

            gvProduto.DataSource = listaItem;
            gvProduto.DataBind();
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PRODUTO_TRANSFERENCIA_ITEM produtoItem = e.Row.DataItem as PRODUTO_TRANSFERENCIA_ITEM;

                    if (produtoItem != null)
                    {
                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                            _labCor.Text = new ProducaoController().ObterCoresBasicas(produtoItem.COR).DESC_COR;

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = produtoItem.CODIGO.ToString();

                    }
                }
            }
        }

        private void Excluir()
        {
            var listaItem = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value);
            foreach (var l in listaItem)
            {
                baseController.ExcluirProdutoTransferenciaItem(l.CODIGO);
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErroImport.Text = "";
                labErro.Text = "";

                Excluir();
                CarregarItens(hidCodigoTransferencia.Value);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (Session["USUARIO"] == null)
                {
                    labErro.Text = "SESSÃO EXPIRADA. FAÇA O LOGIN NOVAMENTE.";
                    return;
                }

                if (gvProduto.Rows.Count <= 0)
                {
                    labErro.Text = "Não existem produtos para gerar romaneios.";
                    return;
                }

                var codigoFilial = ddlFilial.SelectedValue.Trim();
                if (codigoFilial == "1029" || codigoFilial == "000041" || codigoFilial == "000029" || codigoFilial == "1054" || codigoFilial == "000003")
                    ProcessarDevInterna();
                else
                    ProcessarProdutoLoja();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('admloj_retirada_mercadoria_porarquivo.aspx', '_self'); } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        private void ProcessarDevInterna()
        {
            string htmlBody = "";
            var romaneiosSaida = new Dictionary<string, string>();


            try
            {

                var usuario = (USUARIO)Session["USUARIO"];

                var listaCmax = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value);
                var notas = listaCmax.GroupBy(p => new { NF_ENTRADA = p.NF_ENTRADA.Trim(), SERIE_NF_ENTRADA = p.SERIE_NF_ENTRADA.Trim(), FILIAL = p.FILIAL.Trim(), FILIAL_ORIGEM = p.FILIAL_ORIGEM.Trim() }).Select(
                    x => new PRODUTO_TRANSFERENCIA_ITEM
                    {
                        NF_ENTRADA = x.Key.NF_ENTRADA.Trim(),
                        SERIE_NF_ENTRADA = x.Key.SERIE_NF_ENTRADA.Trim(),
                        FILIAL = x.Key.FILIAL.Trim(),
                        FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM.Trim(),
                        USUARIO_INCLUSAO = usuario.CODIGO_USUARIO
                    });

                foreach (var n in notas)
                {
                    var rom = InserirRomaneioSaida(n);
                    romaneiosSaida.Add(rom, n.FILIAL_ORIGEM);
                }

                htmlBody = MontarCorpoEmail(romaneiosSaida);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(usuario.EMAIL);

                mail.To.Add("sueli.ponta@hbf.com.br");
                mail.To.Add("rodrigo.garcia@hbf.com.br");
                mail.To.Add("leandro.bevilaqua@hbf.com.br");
                mail.To.Add("anapaula@hbf.com.br");

                mail.Subject = "[Intranet] - Produtos para Devolução Interna";
                mail.IsBodyHtml = true;

                mail.Body = htmlBody;
                //mail.BodyEncoding = System.Text.Encoding.Default;

                //mail.ReplyTo = new MailAddress(this.EmailUsuario);

                SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
                //smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                smtp.Port = 587;
                smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);
                smtp.EnableSsl = false;

                smtp.Send(mail);

                //FINALIZA O REGISTRO
                var produtoTransf = baseController.ObterProdutoTransferencia(Convert.ToInt32(hidCodigoTransferencia.Value));
                if (produtoTransf != null)
                {
                    produtoTransf.VOLUME = 0;
                    produtoTransf.QTDE_ITEM = 0;
                    produtoTransf.DATA_ENVIO = DateTime.Now;
                    produtoTransf.OBSERVACAO = "";


                    baseController.AtualizarProdutoTransferencia(produtoTransf);
                }

            }
            catch (Exception)
            {
                foreach (var r in romaneiosSaida)
                    baseController.ExcluirRomaneioEstoqueSaida(r.Key, ddlFilial.SelectedItem.Text);

                throw;
            }


        }

        private string InserirRomaneioSaida(PRODUTO_TRANSFERENCIA_ITEM nota)
        {
            var romaneioSaida = baseController.GerarRomaneioSaidaEstoqueInterno(nota.FILIAL.Trim(), nota.FILIAL_ORIGEM.Trim(), nota.NF_ENTRADA.Trim(), nota.SERIE_NF_ENTRADA.Trim(), nota.USUARIO_INCLUSAO.ToString()).ROMANEIO_PRODUTO;
            return romaneioSaida;
        }

        private string MontarCorpoEmail(Dictionary<string, string> romaneiosSaida)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("");
            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Devolução de Produtos Interno</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <br />");

            if (romaneiosSaida != null && romaneiosSaida.Count() > 0)
            {
                sb.Append("    <div id='divDev' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Romaneio de Saída");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Filial Origem");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");
                foreach (var rom in romaneiosSaida)
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + rom.Key);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + rom.Value);
                    sb.Append("                            </td>");
                    sb.Append("                        </tr>");
                }
                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
                sb.Append("    <br />");
            }
            sb.Append("    <br />");
            sb.Append("    <br /><br /><span>E-mail enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }


        #region "LOJA"

        private void ProcessarProdutoLoja()
        {

            int qtdeItem = 0;
            Hashtable hashRomaneios = new Hashtable();
            List<PRODUTO_TRANSFERENCIA_ITEM> listas = new List<PRODUTO_TRANSFERENCIA_ITEM>();

            var erroNota = "";


            try
            {
                string htmlBody = "";

                var codigoFilial = ddlFilial.SelectedValue.Trim();
                var usuario = (USUARIO)Session["USUARIO"];
                var arquivos = new List<string>();

                var listaItens = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value);

                var filiaisOrigem = listaItens.GroupBy(p => new { FILIAL_ORIGEM = p.FILIAL_ORIGEM.Trim() }).Select(
                    x => new PRODUTO_TRANSFERENCIA_ITEM
                    {
                        FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM.Trim()
                    });


                foreach (var fOrigem in filiaisOrigem)
                {
                    var notas = listaItens.Where(p => p.FILIAL_ORIGEM.Trim() == fOrigem.FILIAL_ORIGEM.Trim()).OrderBy(p => p.NF_ENTRADA).GroupBy(p => new { NF_ENTRADA = p.NF_ENTRADA.Trim(), SERIE_NF_ENTRADA = p.SERIE_NF_ENTRADA.Trim(), FILIAL = p.FILIAL.Trim(), FILIAL_ORIGEM = p.FILIAL_ORIGEM.Trim() }).Select(
                        x => new PRODUTO_TRANSFERENCIA_ITEM
                        {
                            NF_ENTRADA = x.Key.NF_ENTRADA.Trim(),
                            SERIE_NF_ENTRADA = x.Key.SERIE_NF_ENTRADA.Trim(),
                            FILIAL = x.Key.FILIAL.Trim(),
                            FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM.Trim(),
                            USUARIO_INCLUSAO = usuario.CODIGO_USUARIO
                        });

                    //gerar romaneios
                    foreach (var n in notas.Where(p => p.NF_ENTRADA != "999999"))
                    {
                        erroNota = n.NF_ENTRADA;

                        var rom = InserirRomaneioSaida(n);
                        hashRomaneios.Add(rom, fOrigem.FILIAL_ORIGEM.Trim());
                    }

                    var nomeArquivo = "C:\\bkp\\" + fOrigem.FILIAL_ORIGEM.Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "_").Trim() + "_" + usuario.NOME_USUARIO.Trim() + "_" + "devinterna" + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                    var listaItem = GerarArquivo(nomeArquivo, fOrigem.FILIAL_ORIGEM.Trim());
                    if (listaItem != null && listaItem.Count() > 0)
                    {
                        arquivos.Add(nomeArquivo);
                        qtdeItem = qtdeItem + listaItem.Count();
                        listas.AddRange(listaItem);
                    }

                }

                //gerar arquivos outros
                var nomeArquivoSemSaldo = "C:\\bkp\\sem_saldo_" + usuario.NOME_USUARIO.Trim().Replace(" ", "_") + "_" + "devinterna" + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                var listaItemSemSaldo = GerarArquivoSemSaldo(nomeArquivoSemSaldo);
                if (listaItemSemSaldo != null && listaItemSemSaldo.Count() > 0)
                {
                    qtdeItem = qtdeItem + listaItemSemSaldo.Count();
                    arquivos.Add(nomeArquivoSemSaldo);
                }

                //htmlBody = MontarCorpoEmailLoja(listaItemHandbook, romaneiosSaida, "");
                htmlBody = MontarCorpoEmailFilialOrigem(hashRomaneios, listas, listaItemSemSaldo);


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(usuario.EMAIL);

                mail.To.Add("sueli.ponta@hbf.com.br");
                mail.To.Add("rodrigo.garcia@hbf.com.br");
                mail.To.Add("leandro.bevilaqua@hbf.com.br");
                mail.To.Add("anapaula@hbf.com.br");
                mail.To.Add(usuario.EMAIL.Trim().ToLower());

                mail.Subject = "[Intranet] - Produtos para Transferência/Devolução Interna Arquivo";
                mail.IsBodyHtml = true;

                foreach (var ar in arquivos)
                    mail.Attachments.Add(new Attachment(ar));

                mail.Body = htmlBody;
                //mail.BodyEncoding = System.Text.Encoding.Default;

                //mail.ReplyTo = new MailAddress(this.EmailUsuario);

                SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
                //smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                smtp.Port = 587;
                smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);
                smtp.EnableSsl = false;

                smtp.Send(mail);

                //FINALIZA O REGISTRO
                var produtoTransf = baseController.ObterProdutoTransferencia(Convert.ToInt32(hidCodigoTransferencia.Value));
                if (produtoTransf != null)
                {
                    produtoTransf.VOLUME = -1;
                    produtoTransf.QTDE_ITEM = (qtdeItem);
                    produtoTransf.DATA_ENVIO = DateTime.Now;
                    produtoTransf.OBSERVACAO = "DEV INTERNA";
                    produtoTransf.PERMUTA = 'N';

                    baseController.AtualizarProdutoTransferencia(produtoTransf);
                }

            }
            catch (Exception ex)
            {
                foreach (DictionaryEntry entry in hashRomaneios)
                    baseController.ExcluirRomaneioEstoqueSaida(entry.Key.ToString(), ddlFilial.SelectedItem.Text);

                labErro.Text = "NOTA : " + erroNota + " --------- " + ex.Message;
                return;
            }
        }

        private string InserirRomaneioSaidaLoja(PRODUTO_TRANSFERENCIA_ITEM nota)
        {
            var romaneioSaida = baseController.GerarRomaneioSaidaEstoqueInterno(nota.FILIAL.Trim(), nota.FILIAL_ORIGEM.Trim(), nota.NF_ENTRADA.Trim(), nota.SERIE_NF_ENTRADA.Trim(), "").ROMANEIO_PRODUTO;
            return romaneioSaida;
        }


        private List<PRODUTO_TRANSFERENCIA_ITEM> GerarArquivo(string nomeArquivo, string filialOrigem)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var lista = baseController.ObterProdutoTransferenciaItemPorFilial(hidCodigoTransferencia.Value, filialOrigem).Where(p => p.NF_ENTRADA != "999999").ToList();
            foreach (var item in lista)
            {
                arquivoW.WriteLine(item.CODIGO_BARRA.Trim() + ",01");
            }
            arquivoW.Close();
            arquivoW.Dispose();

            if (lista == null || lista.Count() <= 0)
                System.IO.File.Delete(nomeArquivo);

            return lista;
        }
        private List<PRODUTO_TRANSFERENCIA_ITEM> GerarArquivoSemSaldo(string nomeArquivo)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var listaCmax = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value, true).Where(p => p.NF_ENTRADA == "999999").ToList();
            var listaHandbook = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value, false).Where(p => p.NF_ENTRADA == "999999").ToList();
            var lista = new List<PRODUTO_TRANSFERENCIA_ITEM>();
            lista.AddRange(listaCmax);
            lista.AddRange(listaHandbook);
            foreach (var item in lista)
            {
                arquivoW.WriteLine(item.CODIGO_BARRA.Trim() + ",01");
            }
            arquivoW.Close();
            arquivoW.Dispose();

            if (lista == null || lista.Count() <= 0)
                System.IO.File.Delete(nomeArquivo);

            return lista;
        }

        private List<PRODUTO_TRANSFERENCIA_ITEM> GerarArquivoHandbookLoja(string nomeArquivo)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var lista = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value, false);
            foreach (var item in lista)
            {
                arquivoW.WriteLine(item.CODIGO_BARRA.Trim() + ",01");
            }
            arquivoW.Close();
            arquivoW.Dispose();

            return lista;
        }
        private List<PRODUTO_TRANSFERENCIA_ITEM> GerarArquivoCmaxLoja(string nomeArquivo)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var lista = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value, true);
            foreach (var item in lista)
            {
                arquivoW.WriteLine(item.CODIGO_BARRA.Trim() + ",01");
            }
            arquivoW.Close();
            arquivoW.Dispose();

            return lista;
        }

        private string MontarCorpoEmailLoja(List<PRODUTO_TRANSFERENCIA_ITEM> listaHandbook, List<string> romaneiosSaida, string obs)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            sb.Append("");
            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Transferência/Devolução de Produtos</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <br />");

            if (obs != null && obs != "")
            {
                sb.Append("    <div id='divObs' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Observação");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");

                sb.Append("                        <tr style='border: 1px solid #000;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                sb.Append("                                " + obs);
                sb.Append("                            </td>");
                sb.Append("                        </tr>");

                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
                sb.Append("    <br />");
            }

            if (romaneiosSaida != null && romaneiosSaida.Count() > 0)
            {
                sb.Append("    <div id='divDev' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Romaneio de Saída");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");
                foreach (var rom in romaneiosSaida)
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + rom);
                    sb.Append("                            </td>");
                    sb.Append("                        </tr>");
                }
                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
                sb.Append("    <br />");
            }
            sb.Append("    <br />");
            if (listaHandbook != null && listaHandbook.Count() > 0)
            {
                sb.Append("    <div id='divTransf' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Produto");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                sb.Append("                                Descrição");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 100px; border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Cor");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Descrição Cor");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Tamanho");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000; text-align: right;' valign='top' >");
                sb.Append("                                Qtde");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");
                foreach (PRODUTO_TRANSFERENCIA_ITEM p in listaHandbook)
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.CODIGO_BARRA.Substring(0, 5));
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.NOME);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + p.COR);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + (new ProducaoController().ObterCoresBasicas(p.COR).DESC_COR));
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + p.TAMANHO);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000; text-align:right;'>");
                    sb.Append("                                1");
                    sb.Append("                            </td>");
                    sb.Append("                        </tr>");

                    count += 1;
                }
                sb.Append("                        <tr style='border: 1px solid #000;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                sb.Append("                                Total");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000; text-align:right;'>");
                sb.Append("                                " + count.ToString());
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
            }
            sb.Append("    <br /><br /><span>E-mail enviado pela Filial: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        private string MontarCorpoEmailFilialOrigem(Hashtable hashRomaneios, List<PRODUTO_TRANSFERENCIA_ITEM> listaItens, List<PRODUTO_TRANSFERENCIA_ITEM> listaSemsaldo)
        {
            ProducaoController prodController = new ProducaoController();

            StringBuilder sb = new StringBuilder();
            int count = 0;
            sb.Append("");
            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Transferência/Devolução de Produtos</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <br />");


            if (hashRomaneios != null && hashRomaneios.Count > 0)
            {
                sb.Append("    <div id='divDev' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Romaneio de Saída");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Filial");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");

                foreach (DictionaryEntry entry in hashRomaneios)
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + entry.Key.ToString());
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + entry.Value.ToString());
                    sb.Append("                            </td>");
                    sb.Append("                        </tr>");
                }
                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
                sb.Append("    <br />");


            }


            sb.Append("    <br />");
            if (listaItens != null && listaItens.Count() > 0)
            {
                sb.Append("    <div id='divTransf' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Filial Origem");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                sb.Append("                                Produto");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                sb.Append("                                Descrição");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 100px; border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Cor");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Descrição Cor");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Tamanho");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000; text-align: right;' valign='top' >");
                sb.Append("                                Qtde");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");
                foreach (PRODUTO_TRANSFERENCIA_ITEM p in listaItens.OrderBy(p => p.FILIAL_ORIGEM))
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.FILIAL_ORIGEM);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.CODIGO_BARRA.Substring(0, 5));
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.NOME);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + p.COR);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + (prodController.ObterCoresBasicas(p.COR).DESC_COR));
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + p.TAMANHO);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000; text-align:right;'>");
                    sb.Append("                                1");
                    sb.Append("                            </td>");
                    sb.Append("                        </tr>");

                    count += 1;
                }
                sb.Append("                        <tr style='border: 1px solid #000;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                sb.Append("                                Total");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000; text-align:right;'>");
                sb.Append("                                " + count.ToString());
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
            }


            sb.Append("    <br />");
            sb.Append("    <br />");


            count = 0;
            if (listaSemsaldo != null && listaSemsaldo.Count() > 0)
            {
                sb.Append("    <div id='divSemSaldo' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                SEM SALDO");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
                sb.Append("    <div id='divSemSaldoProdutos' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Produto");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                sb.Append("                                Descrição");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 100px; border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Cor");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Descrição Cor");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                Tamanho");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000; text-align: right;' valign='top' >");
                sb.Append("                                Qtde");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");
                foreach (PRODUTO_TRANSFERENCIA_ITEM p in listaSemsaldo)
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.CODIGO_BARRA.Substring(0, 5));
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.NOME);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + p.COR);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + (prodController.ObterCoresBasicas(p.COR).DESC_COR));
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                    sb.Append("                                " + p.TAMANHO);
                    sb.Append("                            </td>");
                    sb.Append("                            <td style='border-right: 1px solid #000; text-align:right;'>");
                    sb.Append("                                1");
                    sb.Append("                            </td>");
                    sb.Append("                        </tr>");

                    count += 1;
                }
                sb.Append("                        <tr style='border: 1px solid #000;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                sb.Append("                                Total");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 200px; border-right: 1px solid #000;'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
                sb.Append("                                &nbsp;");
                sb.Append("                            </td>");
                sb.Append("                            <td style='border-right: 1px solid #000; text-align:right;'>");
                sb.Append("                                " + count.ToString());
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
            }
            sb.Append("    <br /><br /><span>E-mail enviado pela Filial: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion


    }
}