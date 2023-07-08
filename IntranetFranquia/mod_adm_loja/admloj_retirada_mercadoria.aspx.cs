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
using System.Collections;
using System.IO;
using System.Drawing;

namespace Relatorios
{
    public partial class admloj_retirada_mercadoria : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        int gQtde = 0;
        int gQtdeSol = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtCodigoProduto.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "', event)");
            txtCodigoProduto.Focus();

            if (!Page.IsPostBack)
            {
                var solPorArquivo = "";
                var codigoTransferencia = 0;
                if (Request.QueryString["sol"] == null || Request.QueryString["sol"] == "")
                    solPorArquivo = "0";
                else
                    solPorArquivo = Request.QueryString["sol"].ToString();

                if (Request.QueryString["ct"] == null || Request.QueryString["ct"] == "")
                    codigoTransferencia = 0;
                else
                    codigoTransferencia = Convert.ToInt32(Request.QueryString["ct"].ToString());

                hidSolArquivo.Value = solPorArquivo;
                hidCodigoTransferencia.Value = codigoTransferencia.ToString();

                CarregarDadosIni();

                dialogPai.Visible = false;
            }

            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        private void CarregarDadosIni()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");

            var codigoTransferencia = 0;
            if (hidSolArquivo.Value == "0")
            {
                var usuario = (USUARIO)Session["USUARIO"];

                //Obter Loja
                string codigoFilial = "";
                USUARIOLOJA usuarioLoja = baseController.BuscaUsuarioLoja(usuario.CODIGO_USUARIO);
                if (usuarioLoja == null || usuario.CODIGO_PERFIL == 1)
                    codigoFilial = usuario.CODIGO_USUARIO.ToString();
                else
                    codigoFilial = usuarioLoja.CODIGO_LOJA;

                var pt = baseController.ObterProdutoTransferencia(codigoFilial).Where(p => p.DATA_ENVIO == null && (p.SOL_POR_ARQUIVO == null || p.SOL_POR_ARQUIVO == false)).FirstOrDefault();
                if (pt == null)
                {
                    PRODUTO_TRANSFERENCIA ptNovo = new PRODUTO_TRANSFERENCIA();
                    ptNovo.CODIGO_FILIAL = codigoFilial;
                    ptNovo.QTDE_ITEM = 0;
                    ptNovo.VOLUME = 0;
                    ptNovo.USUARIO_INCLUSAO = usuario.CODIGO_USUARIO;
                    ptNovo.DATA_INCLUSAO = DateTime.Now;
                    ptNovo.SOL_POR_ARQUIVO = false;

                    codigoTransferencia = baseController.InserirProdutoTransferencia(ptNovo);
                }
                else
                {
                    codigoTransferencia = pt.CODIGO;
                }

                hidCodigoTransferencia.Value = codigoTransferencia.ToString();
            }


            CarregarItens(hidCodigoTransferencia.Value);
            CarregarFilial();
            CarregarFiliaisPara();
        }


        #region "DADOS INICIAIS"
        private void CarregarFiliaisPara()
        {

            var filial = baseController.BuscaFiliais_Agora();

            filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
            ddlParaFilial.DataSource = filial;
            ddlParaFilial.DataBind();
        }

        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 2 || usuario.CODIGO_PERFIL == 3)
                {
                    var lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();

                    if (lstFilial.Count > 0)
                    {
                        lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                        if (usuario.CODIGO_PERFIL == 1)
                            lstFilial.Insert(1, new FILIAI { COD_FILIAL = "000000", FILIAL = "ESCRITÓRIO" });

                        ddlFilial.DataSource = lstFilial;
                        ddlFilial.DataBind();

                        if (lstFilial.Count == 2)
                        {
                            ddlFilial.SelectedIndex = 1;
                            ddlFilial.Enabled = false;
                        }

                        if (hidSolArquivo.Value == "1")
                        {
                            var produtoTrans = baseController.ObterProdutoTransferencia(Convert.ToInt32(hidCodigoTransferencia.Value));
                            if (produtoTrans != null)
                            {
                                ddlFilial.SelectedValue = produtoTrans.CODIGO_FILIAL;
                                ddlFilial.Enabled = false;
                                txtObservacao.Text = produtoTrans.OBSERVACAO;
                                txtObservacao.Enabled = false;
                                ddlMotivo.SelectedValue = "R";
                                ddlMotivo.Enabled = false;

                                if (produtoTrans.DATA_ENVIO != null)
                                {
                                    btSalvar.Enabled = false;
                                    btGravar.Enabled = false;
                                    gvProduto.Enabled = false;
                                    txtVolume.Text = produtoTrans.VOLUME.ToString();
                                    txtVolume.Enabled = false;
                                    txtCodigoProduto.Enabled = false;
                                }

                            }
                        }
                        else
                        {
                            ddlFilial.SelectedValue = "000000";
                            ddlFilial.Enabled = false;
                        }

                    }
                }
                else
                {
                    var lstFilial = new List<FILIAI>();
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "000000", FILIAL = "ESCRITÓRIO" });

                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();
                }
            }
        }
        private void LimparCampos()
        {
            txtCodigoProduto.Text = string.Empty;
        }
        #endregion

        #region "PRODUTO"
        private void CarregarItens(string codigoTransferencia)
        {
            if (hidSolArquivo.Value == "0")
            {
                var listaItem = baseController.ObterProdutoTransferenciaQtde(Convert.ToInt32(codigoTransferencia), "", "", "", 1);

                gvProduto.DataSource = listaItem.OrderBy(p => p.PRODUTO).ThenBy(p => p.COR).ThenBy(p => p.TAMANHO);
                gvProduto.DataBind();

                fsRetirada.Visible = true;
            }
            else
            {
                var transCab = baseController.ObterProdutoTransferencia(Convert.ToInt32(codigoTransferencia));
                if (transCab != null && transCab.DATA_ENVIO != null)
                {
                    gvProdutoNaoSol.Enabled = false;
                    gvProdutoSol.Enabled = false;
                }

                var listaSol = baseController.ObterProdutoTransferenciaQtde(Convert.ToInt32(codigoTransferencia), "", "", "", 2);


                //<asp:ListItem Value="T" Text="MOSTRAR TODOS"></asp:ListItem>
                //<asp:ListItem Value="D" Text="MOSTRAR PRODUTOS DIVERGENTES"></asp:ListItem>
                //<asp:ListItem Value="K" Text="MOSTRAR PRODUTOS OK"></asp:ListItem>
                if (ddlMostrar.SelectedValue == "K")
                    listaSol = listaSol.Where(p => p.QTDE == p.QTDE_SOL).ToList();
                else if (ddlMostrar.SelectedValue == "D")
                    listaSol = listaSol.Where(p => p.QTDE != p.QTDE_SOL).ToList();

                gQtde = 0;
                gQtdeSol = 0;
                gvProdutoSol.DataSource = listaSol.OrderBy(p => p.PRODUTO).ThenBy(p => p.COR).ThenBy(p => p.TAMANHO);
                gvProdutoSol.DataBind();

                var listaNaoSol = baseController.ObterProdutoTransferenciaQtde(Convert.ToInt32(codigoTransferencia), "", "", "", 3);

                gQtde = 0;
                gQtdeSol = 0;
                gvProdutoNaoSol.DataSource = listaNaoSol.OrderBy(p => p.PRODUTO).ThenBy(p => p.COR).ThenBy(p => p.TAMANHO);
                gvProdutoNaoSol.DataBind();

                fsSolicitado.Visible = true;
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTOTRANSF_QTDEResult produtoQtde = e.Row.DataItem as SP_OBTER_PRODUTOTRANSF_QTDEResult;

                    if (produtoQtde != null)
                    {
                        Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (btExcluir != null)
                            btExcluir.CommandArgument = produtoQtde.PRODUTO + "|" + produtoQtde.COR + "|" + produtoQtde.TAMANHO;

                        gQtde += Convert.ToInt32(produtoQtde.QTDE);


                    }
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[5].Text = gQtde.ToString();
            }
        }

        protected void gvProdutoSol_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTOTRANSF_QTDEResult produtoQtde = e.Row.DataItem as SP_OBTER_PRODUTOTRANSF_QTDEResult;

                    if (produtoQtde != null)
                    {
                        Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (btExcluir != null)
                            btExcluir.CommandArgument = produtoQtde.PRODUTO + "|" + produtoQtde.COR + "|" + produtoQtde.TAMANHO;

                        if (produtoQtde.QTDE > 0)
                        {
                            if (produtoQtde.QTDE != produtoQtde.QTDE_SOL)
                                e.Row.BackColor = Color.LightYellow;
                            else
                                e.Row.BackColor = Color.PaleGreen;
                        }

                        gQtdeSol += Convert.ToInt32(produtoQtde.QTDE_SOL);
                        gQtde += Convert.ToInt32(produtoQtde.QTDE);

                    }
                }
            }
        }
        protected void gvProdutoSol_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[5].Text = gQtdeSol.ToString();
                footer.Cells[6].Text = gQtde.ToString();
            }
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroBip.Text = "";


                if (ddlFilial.SelectedValue == "")
                {
                    labErroBip.Text = "Selecione a Filial";
                    return;
                }

                //if (gvProduto.Rows.Count > 80)
                //{
                //    labErroBip.Text = "Limite de produtos atingido. Por favor, salve os produtos atuais para continuar inserindo outros produtos.";
                //    return;
                //}

                var codigoBarraProduto = txtCodigoProduto.Text.Trim();
                var codigoFilial = ddlFilial.SelectedValue.Trim();

                var produtoBarra = baseController.BuscaProdutoBarra(codigoBarraProduto);

                if (produtoBarra != null)
                {
                    var produtoTransferenciaItem = new PRODUTO_TRANSFERENCIA_ITEM();
                    produtoTransferenciaItem.CODIGO_BARRA = produtoBarra.CODIGO_BARRA;
                    produtoTransferenciaItem.COR = produtoBarra.COR_PRODUTO.Trim();
                    produtoTransferenciaItem.PRODUTO = produtoBarra.PRODUTO.Trim();

                    var produto = baseController.BuscaProduto(produtoTransferenciaItem.PRODUTO);
                    produtoTransferenciaItem.NOME = produto.DESC_PRODUTO;
                    produtoTransferenciaItem.COLECAO = produto.COLECAO;

                    produtoTransferenciaItem.TAMANHO = produtoBarra.GRADE;

                    produtoTransferenciaItem.PRODUTO_TRANSFERENCIA = Convert.ToInt32(hidCodigoTransferencia.Value);
                    produtoTransferenciaItem.DATA_INCLUSAO = DateTime.Now;
                    produtoTransferenciaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    var geraRomaneio = baseController.ObterFilialIntranet(codigoFilial);
                    if ((ddlFilial.SelectedValue == "000000" && produtoBarra.PRODUTO.Substring(0, 1) == "6") ||
                        (geraRomaneio.GERA_ROMANEIO_CMAX && produtoBarra.PRODUTO.Substring(0, 1) == "6") ||
                        (geraRomaneio.GERA_ROMANEIO_HANDBOOK && produtoBarra.PRODUTO.Substring(0, 1) != "6")
                    )
                    {
                        if (ddlFilial.SelectedValue == "000000")
                        {
                            produtoTransferenciaItem.FILIAL = "ESCRITORIO";
                            produtoTransferenciaItem.FILIAL_ORIGEM = "ESCRITORIO";
                            produtoTransferenciaItem.NF_ENTRADA = "000000";
                            produtoTransferenciaItem.SERIE_NF_ENTRADA = "00";
                        }
                        else
                        {
                            var saldo = baseController.ObterSaldoLojaDev(produtoBarra.PRODUTO, produtoBarra.COR_PRODUTO, Convert.ToInt32(produtoBarra.TAMANHO), ddlFilial.SelectedItem.Text.Trim());
                            if (saldo == null)
                            {
                                //labErro.Text = "O produto informado não tem saldo de devolução. Entre em contato com a Logística.";
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
                                produtoTransferenciaItem.FILIAL = saldo.FILIAL.Trim();
                                produtoTransferenciaItem.FILIAL_ORIGEM = saldo.FILIAL_ORIGEM.Trim();
                                produtoTransferenciaItem.NF_ENTRADA = saldo.NF_ENTRADA.Trim();
                                produtoTransferenciaItem.SERIE_NF_ENTRADA = saldo.SERIE_NF_ENTRADA.Trim();
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
                        }

                    }

                    baseController.InserirProdutoTransferenciaItem(produtoTransferenciaItem);
                }

                LimparCampos();

                CarregarItens(hidCodigoTransferencia.Value);
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
                Button bt = (Button)sender;
                if (bt != null)
                {
                    //produtoQtde.PRODUTO + "|" + produtoQtde.COR + "|" + produtoQtde.TAMANHO;

                    var vals = bt.CommandArgument.Split('|');
                    var produto = vals[0];
                    var cor = vals[1];
                    var tamanho = vals[2];

                    baseController.ExcluirProdutoTransferenciaItem(Convert.ToInt32(hidCodigoTransferencia.Value), produto, cor, tamanho);
                    CarregarItens(hidCodigoTransferencia.Value);
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
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

                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                if (txtVolume.Text.Trim() == "")
                {
                    labErro.Text = "Informe a quantidade de Volumes.";
                    return;
                }

                if (ddlMotivo.SelectedValue == "")
                {
                    labErro.Text = "Selecione o Motivo.";
                    return;
                }

                if (ddlMotivo.SelectedValue == "T" && ddlParaFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Filial de transferência.";
                    return;
                }

                int volume = 0;
                if (!int.TryParse(txtVolume.Text.Trim(), out volume))
                {
                    labErro.Text = "Informe apenas o NÚMERO no campo Volume.";
                    return;
                }

                if (ddlFilial.SelectedValue != "000000")
                {
                    ProcessarProdutoLoja();
                }
                else
                {
                    ProcessarProdutoEscritorio();
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('~/DefaultFilial.aspx', '_self'); } }); });", true);
                dialogPai.Visible = true;
                txtVolume.Text = "";
                txtObservacao.Text = "";
                txtCodigoProduto.Text = "";
                ddlMotivo.SelectedValue = "";

                btSalvar.Enabled = false;
                btGravar.Enabled = false;
                txtCodigoProduto.Enabled = false;

                //CarregarDadosIni();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void ProcessarProdutoEscritorio()
        {
            try
            {
                string htmlBody = "";

                var usuario = (USUARIO)Session["USUARIO"];
                var arquivos = new List<string>();

                // gerar arquivo handbook
                var nomeArquivo = "C:\\bkp\\criatiff_" + usuario.NOME_USUARIO.Trim() + "_" + "transferencia" + "_vol-" + txtVolume.Text.Trim() + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                var listaItemHandbook = GerarArquivoHandbook(nomeArquivo);
                if (listaItemHandbook != null && listaItemHandbook.Count() > 0)
                    arquivos.Add(nomeArquivo);

                //gerar arquivos cmax
                var listaCmax = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value, true);
                var notas = listaCmax.GroupBy(p => new { NF_ENTRADA = p.NF_ENTRADA, SERIE_NF_ENTRADA = p.SERIE_NF_ENTRADA, FILIAL = p.FILIAL, FILIAL_ORIGEM = p.FILIAL_ORIGEM }).Select(
                    x => new PRODUTO_TRANSFERENCIA_ITEM
                    {
                        NF_ENTRADA = x.Key.NF_ENTRADA,
                        SERIE_NF_ENTRADA = x.Key.SERIE_NF_ENTRADA,
                        FILIAL = x.Key.FILIAL,
                        FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM
                    });

                int qtdeItemCmax = 0;

                foreach (var n in notas)
                {
                    var nomeArquivoCmax = "C:\\bkp\\cmax_" + usuario.NOME_USUARIO.Trim() + "_" + "transferencia" + "_vol-" + txtVolume.Text.Trim() + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                    var listaItemCmax = GerarArquivoCmax(nomeArquivoCmax);

                    qtdeItemCmax = qtdeItemCmax + listaItemCmax.Count();
                    arquivos.Add(nomeArquivoCmax);
                }

                htmlBody = MontarCorpoEmailEscritorio(listaItemHandbook, listaCmax, txtObservacao.Text.Trim());

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(usuario.EMAIL);

                mail.To.Add("sueli.ponta@hbf.com.br");
                mail.To.Add("rodrigo.garcia@hbf.com.br");
                mail.To.Add("leandro.bevilaqua@hbf.com.br");
                mail.To.Add("anapaula@hbf.com.br");
                if (ddlFilial.SelectedValue == "000000")
                {
                    mail.To.Add("acabamentointerno@hbf.com.br");
                }

                mail.Subject = "[Intranet] - Produtos para Transferência/Devolução";
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
                    produtoTransf.VOLUME = Convert.ToInt32(txtVolume.Text);
                    produtoTransf.QTDE_ITEM = (listaItemHandbook.Count() + qtdeItemCmax);
                    produtoTransf.DATA_ENVIO = DateTime.Now;
                    produtoTransf.OBSERVACAO = txtObservacao.Text.Trim();
                    produtoTransf.PERMUTA = 'N';
                    produtoTransf.MOTIVO = Convert.ToChar(ddlMotivo.SelectedValue);

                    baseController.AtualizarProdutoTransferencia(produtoTransf);
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "Erro : " + ex.Message;
                return;
            }
        }
        private void ProcessarProdutoLoja()
        {

            int qtdeItem = 0;
            Hashtable hashRomaneios = new Hashtable();
            List<PRODUTO_TRANSFERENCIA_ITEM> listas = new List<PRODUTO_TRANSFERENCIA_ITEM>();

            try
            {
                string htmlBody = "";

                var codigoFilial = ddlFilial.SelectedValue.Trim();
                var usuario = (USUARIO)Session["USUARIO"];
                var arquivos = new List<string>();

                //var geraRomaneio = baseController.ObterFilialIntranet(codigoFilial);

                var listaItens = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value);

                var filiaisOrigem = listaItens.GroupBy(p => new { FILIAL_ORIGEM = p.FILIAL_ORIGEM.Trim().ToUpper() }).Select(
                    x => new PRODUTO_TRANSFERENCIA_ITEM
                    {
                        FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM.Trim()
                    });

                foreach (var fOrigem in filiaisOrigem)
                {
                    var notas = listaItens.Where(p => p.FILIAL_ORIGEM.Trim() == fOrigem.FILIAL_ORIGEM.Trim()).GroupBy(p => new { NF_ENTRADA = p.NF_ENTRADA.Trim(), SERIE_NF_ENTRADA = p.SERIE_NF_ENTRADA.Trim(), FILIAL = p.FILIAL.Trim(), FILIAL_ORIGEM = p.FILIAL_ORIGEM.Trim().ToUpper() }).Select(
                        x => new PRODUTO_TRANSFERENCIA_ITEM
                        {
                            NF_ENTRADA = x.Key.NF_ENTRADA.Trim(),
                            SERIE_NF_ENTRADA = x.Key.SERIE_NF_ENTRADA.Trim(),
                            FILIAL = x.Key.FILIAL.Trim(),
                            FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM.Trim()
                        });

                    //gerar romaneios
                    foreach (var n in notas.Where(p => p.NF_ENTRADA != "999999"))
                    {
                        var rom = InserirRomaneioSaida(n);
                        hashRomaneios.Add(rom, fOrigem.FILIAL_ORIGEM.Trim());
                    }

                    var nomeArquivo = "C:\\bkp\\" + fOrigem.FILIAL_ORIGEM.Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "_").Trim() + "_" + usuario.NOME_USUARIO.Trim() + "_" + "transferencia" + "_vol-" + txtVolume.Text.Trim() + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                    var listaItem = GerarArquivo(nomeArquivo, fOrigem.FILIAL_ORIGEM);
                    if (listaItem != null && listaItem.Count() > 0)
                    {
                        arquivos.Add(nomeArquivo);
                        qtdeItem = qtdeItem + listaItem.Count();
                        listas.AddRange(listaItem);
                    }

                }

                //gerar arquivos outros
                var nomeArquivoSemSaldo = "C:\\bkp\\sem_saldo_" + usuario.NOME_USUARIO.Trim().Replace(" ", "_") + "_" + "transferencia" + "_vol-" + txtVolume.Text.Trim() + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                var listaItemSemSaldo = GerarArquivoSemSaldo(nomeArquivoSemSaldo);
                if (listaItemSemSaldo != null && listaItemSemSaldo.Count() > 0)
                {
                    qtdeItem = qtdeItem + listaItemSemSaldo.Count();
                    arquivos.Add(nomeArquivoSemSaldo);
                }

                var codigoMotivo = ddlMotivo.SelectedValue;
                htmlBody = MontarCorpoEmailFilialOrigem(hashRomaneios, listas, txtObservacao.Text.Trim(), listaItemSemSaldo, codigoMotivo);

                // SE FOR PERMUTA. NAO GERA ROMANEIO (NO CASO EU EXCLUO)
                if (codigoMotivo == "P" || codigoMotivo == "T")
                {
                    foreach (DictionaryEntry entry in hashRomaneios)
                        baseController.ExcluirRomaneioEstoqueSaida(entry.Key.ToString(), ddlFilial.SelectedItem.Text);
                }


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(usuario.EMAIL);

                mail.To.Add("sueli.ponta@hbf.com.br");
                mail.To.Add("rodrigo.garcia@hbf.com.br");
                mail.To.Add("leandro.bevilaqua@hbf.com.br");
                mail.To.Add("anapaula@hbf.com.br");
                if (ddlFilial.SelectedValue == "000000")
                {
                    //mail.To.Add("wilderlan.duarte@hbf.com.br");
                    mail.To.Add("acabamentointerno@hbf.com.br");
                }

                var assuntoPermuta = (codigoMotivo == "R") ? " Retirada" : ((codigoMotivo == "T") ? (" Transferência " + ddlParaFilial.SelectedItem.Text.Trim()) : "Permuta");


                mail.Subject = "[Intranet] - Produtos para Transferência/Devolução" + assuntoPermuta;
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

                //throw new Exception("erro controlado");

                //FINALIZA O REGISTRO
                var produtoTransf = baseController.ObterProdutoTransferencia(Convert.ToInt32(hidCodigoTransferencia.Value));
                if (produtoTransf != null)
                {
                    produtoTransf.VOLUME = Convert.ToInt32(txtVolume.Text);
                    produtoTransf.QTDE_ITEM = (qtdeItem);
                    produtoTransf.DATA_ENVIO = DateTime.Now;
                    produtoTransf.OBSERVACAO = txtObservacao.Text.Trim();
                    produtoTransf.PERMUTA = (codigoMotivo == "P") ? 'S' : 'N';
                    produtoTransf.MOTIVO = Convert.ToChar(codigoMotivo);
                    produtoTransf.CODIGO_FILIAL_TRANSF = ddlParaFilial.SelectedValue;

                    baseController.AtualizarProdutoTransferencia(produtoTransf);
                }

            }
            catch (Exception ex)
            {

                foreach (DictionaryEntry entry in hashRomaneios)
                    baseController.ExcluirRomaneioEstoqueSaida(entry.Key.ToString(), ddlFilial.SelectedItem.Text);

                //foreach (var r in romaneiosSaidaCmax)
                //    baseController.ExcluirRomaneioEstoqueSaida(r, ddlFilial.SelectedItem.Text);

                //foreach (var r in romaneiosSaidaHandbook)
                //    baseController.ExcluirRomaneioEstoqueSaida(r, ddlFilial.SelectedItem.Text);

                labErro.Text = "Erro : " + ex.Message;
                return;
            }
        }

        private string InserirRomaneioSaida(PRODUTO_TRANSFERENCIA_ITEM nota)
        {
            var romaneioSaida = baseController.GerarRomaneioSaidaEstoque(nota.FILIAL.Trim(), nota.FILIAL_ORIGEM.Trim(), nota.NF_ENTRADA.Trim(), nota.SERIE_NF_ENTRADA.Trim(), "").ROMANEIO_PRODUTO;
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

        private List<PRODUTO_TRANSFERENCIA_ITEM> GerarArquivoHandbook(string nomeArquivo)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var lista = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value, false).Where(p => p.NF_ENTRADA != "999999").ToList();
            foreach (var item in lista)
            {
                arquivoW.WriteLine(item.CODIGO_BARRA.Trim() + ",01");
            }
            arquivoW.Close();
            arquivoW.Dispose();

            return lista;
        }
        private List<PRODUTO_TRANSFERENCIA_ITEM> GerarArquivoCmax(string nomeArquivo)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var lista = baseController.ObterProdutoTransferenciaItem(hidCodigoTransferencia.Value, true).Where(p => p.NF_ENTRADA != "999999").ToList();
            foreach (var item in lista)
            {
                arquivoW.WriteLine(item.CODIGO_BARRA.Trim() + ",01");
            }
            arquivoW.Close();
            arquivoW.Dispose();

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

        #region "EMAIL"
        private string MontarCorpoEmailFilialOrigem(Hashtable hashRomaneios, List<PRODUTO_TRANSFERENCIA_ITEM> listaItens, string obs, List<PRODUTO_TRANSFERENCIA_ITEM> listaSemsaldo, string permuta)
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
                if (ddlParaFilial.SelectedValue != "")
                {
                    sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                    sb.Append("                                Para Filial");
                    sb.Append("                            </td>");
                }
                sb.Append("                        </tr>");
                sb.Append("");

                sb.Append("                        <tr style='border: 1px solid #000;'>");
                sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                sb.Append("                                " + obs);
                sb.Append("                            </td>");
                if (ddlParaFilial.SelectedValue != "")
                {
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + ddlParaFilial.SelectedItem.Text.Trim());
                    sb.Append("                            </td>");
                }
                sb.Append("                        </tr>");

                sb.Append("                    </table>");
                sb.Append("                </td>");
                sb.Append("            </tr>");
                sb.Append("        </table>");
                sb.Append("    </div>");
                sb.Append("    <br />");
            }
            sb.Append("    <br />");



            if (hashRomaneios != null && hashRomaneios.Count > 0 && permuta == "N")
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
                    sb.Append("                                " + p.PRODUTO);
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
                    sb.Append("                                " + p.PRODUTO);
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
        private string MontarCorpoEmail(List<PRODUTO_TRANSFERENCIA_ITEM> listaHandbook, List<PRODUTO_TRANSFERENCIA_ITEM> listaCmax, List<string> romaneiosSaidaHandbook, List<string> romaneiosSaidaCmax, string obs, List<PRODUTO_TRANSFERENCIA_ITEM> listaSemsaldo)
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
            sb.Append("    <br />");
            if (romaneiosSaidaCmax != null && romaneiosSaidaCmax.Count() > 0)
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
                sb.Append("                                Romaneio de Saída CMAX");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");
                foreach (var rom in romaneiosSaidaCmax)
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
            if (listaCmax != null && listaCmax.Count() > 0)
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
                foreach (PRODUTO_TRANSFERENCIA_ITEM p in listaCmax)
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.PRODUTO);
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


            sb.Append("    <br />");
            sb.Append("    <br />");

            count = 0;

            if (romaneiosSaidaHandbook != null && romaneiosSaidaHandbook.Count() > 0)
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
                sb.Append("                                Romaneio de Saída Handbook");
                sb.Append("                            </td>");
                sb.Append("                        </tr>");
                sb.Append("");
                foreach (var rom in romaneiosSaidaHandbook)
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
                    sb.Append("                                " + p.PRODUTO);
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
                    sb.Append("                                " + p.PRODUTO);
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
        private string MontarCorpoEmailEscritorio(List<PRODUTO_TRANSFERENCIA_ITEM> listaHandbook, List<PRODUTO_TRANSFERENCIA_ITEM> listaCmax, string obs)
        {

            var listaGeral = new List<PRODUTO_TRANSFERENCIA_ITEM>();
            listaGeral.AddRange(listaHandbook);
            listaGeral.AddRange(listaCmax);

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

            sb.Append("    <br />");
            if (listaGeral != null && listaGeral.Count() > 0)
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
                foreach (PRODUTO_TRANSFERENCIA_ITEM p in listaGeral)
                {
                    sb.Append("                        <tr style='border: 1px solid #000;'>");
                    sb.Append("                            <td style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                    sb.Append("                                " + p.PRODUTO);
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
        #endregion

        protected void ddlMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            labParaFilial.Visible = false;
            ddlParaFilial.Visible = false;
            if (ddlMotivo.SelectedValue == "T")
            {
                labParaFilial.Visible = true;
                ddlParaFilial.Visible = true;
            }
        }

        protected void ddlMostrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarItens(hidCodigoTransferencia.Value);
        }
    }
}