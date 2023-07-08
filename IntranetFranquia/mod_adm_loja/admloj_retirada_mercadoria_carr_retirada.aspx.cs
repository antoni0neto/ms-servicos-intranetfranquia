using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class admloj_retirada_mercadoria_carr_retirada : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarFilial();
            }

            btImportar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImportar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            var filial = baseController.BuscaFiliais_Intermediario();
            var filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }

        }
        private void CriarIdTransferencia()
        {
            var usuario = (USUARIO)Session["USUARIO"];
            var codigoUsuario = usuario.CODIGO_USUARIO;

            var ptNovo = new PRODUTO_TRANSFERENCIA();
            ptNovo.CODIGO_FILIAL = ddlFilial.SelectedValue;
            ptNovo.QTDE_ITEM = 0;
            ptNovo.VOLUME = 0;
            ptNovo.USUARIO_INCLUSAO = codigoUsuario;
            ptNovo.DATA_INCLUSAO = DateTime.Now;
            ptNovo.PERMUTA = 'N';
            ptNovo.MOTIVO = 'R';
            ptNovo.OBSERVACAO = txtDescricao.Text.ToUpper().Trim();
            ptNovo.SOL_POR_ARQUIVO = true;
            var codigoTransferencia = baseController.InserirProdutoTransferencia(ptNovo);
            hidCodigoTransferencia.Value = codigoTransferencia.ToString();
        }
        #endregion

        #region "PRODUTO"
        private void CarregarItens(string codigoTransferencia)
        {
            var listaItem = baseController.ObterProdutoTransferenciaSol(codigoTransferencia);

            gvProduto.DataSource = listaItem;
            gvProduto.DataBind();
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PRODUTO_TRANSFERENCIA_SOL produtoSol = e.Row.DataItem as PRODUTO_TRANSFERENCIA_SOL;

                    if (produtoSol != null)
                    {
                        var foto1 = "";
                        var foto2 = "";
                        var foto3 = "";
                        var desenvProduto = desenvController.ObterProduto(produtoSol.COLECAO, produtoSol.PRODUTO, produtoSol.COR);
                        if (desenvProduto != null)
                        {
                            foto1 = "/Fotos/" + produtoSol.PRODUTO.Trim() + produtoSol.COR.Trim() + ".png";
                            foto2 = "/Fotos/" + produtoSol.PRODUTO.Trim() + produtoSol.COR.Trim() + ".jpg";
                            foto3 = desenvProduto.FOTO;
                        }

                        ImageButton _imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                        if (File.Exists(Server.MapPath(foto1)))
                            _imgProduto.ImageUrl = foto1;
                        else if (File.Exists(Server.MapPath(foto2)))
                            _imgProduto.ImageUrl = foto2;
                        else if (File.Exists(Server.MapPath(foto3)))
                            _imgProduto.ImageUrl = foto3;
                        else
                            _imgProduto.ImageUrl = "/Fotos/sem_foto.png";


                        Label labCor = e.Row.FindControl("labCor") as Label;
                        labCor.Text = prodController.ObterCoresBasicas(produtoSol.COR).DESC_COR;

                    }
                }
            }
        }

        private void Excluir()
        {
            var listaItem = baseController.ObterProdutoTransferenciaSol(hidCodigoTransferencia.Value);
            foreach (var l in listaItem)
                baseController.ExcluirProdutoTransferenciaSol(l.CODIGO);

            baseController.ExcluirProdutoTransferencia(Convert.ToInt32(hidCodigoTransferencia.Value));

        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErroImport.Text = "";

                Excluir();
                CarregarItens(hidCodigoTransferencia.Value);

                btImportar.Enabled = true;

            }
            catch (Exception ex)
            {
                labErroImport.Text = ex.Message;
            }
        }
        #endregion

        protected void btImportar_Click(object sender, EventArgs e)
        {

            labErroImport.Text = "";

            if (txtDescricao.Text.Trim() == "")
            {
                labErroImport.Text = "Informe uma descrição para retirada.";
                return;
            }

            if (ddlFilial.SelectedValue == "")
            {
                labErroImport.Text = "Selecione uma Filial.";
                return;
            }

            //Validar arquivo inserido
            if (!uploadArquivoProduto.HasFile)
            {
                labErroImport.Text = "Selecione um arquivo de produto .TXT.";
                return;
            }

            //Validar Tamanho do Arquivo
            if (uploadArquivoProduto.PostedFile.ContentLength <= 0)
            {
                labErroImport.Text = "Selecione um arquivo com o tamanho maior que zero.";
                return;
            }

            //Validar Extensão
            string fileExtension = System.IO.Path.GetExtension(uploadArquivoProduto.PostedFile.FileName);
            if (fileExtension.ToUpper() != ".TXT")
            {
                labErroImport.Text = "Selecione um arquivo com extensão \".txt\".";
                return;
            }


            int totalLinha = 0;
            try
            {
                labErroImport.Text = "Carregando arquivo...";

                CriarIdTransferencia();

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
                                var produtoTransferenciaSol = new PRODUTO_TRANSFERENCIA_SOL();
                                produtoTransferenciaSol.CODIGO_BARRA = produtoBarra.CODIGO_BARRA;
                                produtoTransferenciaSol.COR = produtoBarra.COR_PRODUTO.Trim();
                                produtoTransferenciaSol.PRODUTO = produtoBarra.PRODUTO.Trim();

                                var p = baseController.BuscaProduto(produtoTransferenciaSol.PRODUTO);
                                produtoTransferenciaSol.NOME = p.DESC_PRODUTO;
                                produtoTransferenciaSol.COLECAO = p.COLECAO;

                                produtoTransferenciaSol.TAMANHO = produtoBarra.GRADE;

                                produtoTransferenciaSol.PRODUTO_TRANSFERENCIA = Convert.ToInt32(hidCodigoTransferencia.Value);
                                produtoTransferenciaSol.DATA_INCLUSAO = DateTime.Now;
                                produtoTransferenciaSol.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                                baseController.InserirProdutoTransferenciaSol(produtoTransferenciaSol);

                            }

                            totalLinha += 1;
                        }

                        qtdeCount = 0;
                    }

                    //// atualizar cabecalho
                    ////FINALIZA O REGISTRO
                    //var produtoTransf = baseController.ObterProdutoTransferencia(Convert.ToInt32(hidCodigoTransferencia.Value));
                    //if (produtoTransf != null)
                    //{

                    //    baseController.AtualizarProdutoTransferencia(produtoTransf);
                    //}

                }

                CarregarItens(hidCodigoTransferencia.Value);

                labErroImport.Text = "Arquivo importado com sucesso.";
                btImportar.Enabled = false;

                var listaSol = baseController.ObterProdutoTransferenciaQtde(Convert.ToInt32(hidCodigoTransferencia.Value), "", "", "", 2);

                EnviarEmail(listaSol);
                labErroImport.Text = "Email enviado com sucesso.";

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

        #region "EMAIL"
        private void EnviarEmail(List<SP_OBTER_PRODUTOTRANSF_QTDEResult> produtoTransSolLis)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();

            var produtoTransf = baseController.ObterProdutoTransferencia(Convert.ToInt32(hidCodigoTransferencia.Value));

            var filial = baseController.ObterFilialIntranet(produtoTransf.CODIGO_FILIAL);

            email.ASSUNTO = "Intranet: Solicitação de Retirada - " + produtoTransf.OBSERVACAO.ToUpper().Trim() + " - " + filial.FILIAL.Trim();
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(produtoTransSolLis.OrderBy(p => p.PRODUTO).ThenBy(p => p.COR).ThenBy(p => p.TAMANHO).ToList());

            List<string> destinatario = new List<string>();
            //Adiciona e-mails 
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(88, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);
            destinatario.Add(filial.email);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }

        private string MontarCorpoEmail(List<SP_OBTER_PRODUTOTRANSF_QTDEResult> produtoTransSolLis)
        {
            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            StringBuilder txtBuilder = new StringBuilder();
            txtBuilder.AppendLine("");
            txtBuilder.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            txtBuilder.AppendLine("<html>");
            txtBuilder.AppendLine("<head>");
            txtBuilder.AppendLine("    <title>Solicitação de Retirada Loja</title>");
            txtBuilder.AppendLine("    <meta charset='UTF-8' />");
            txtBuilder.AppendLine("</head>");
            txtBuilder.AppendLine("<body>");
            txtBuilder.AppendLine("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            txtBuilder.AppendLine("        background: white; white-space: nowrap;'>");
            txtBuilder.AppendLine("        <br />");
            txtBuilder.AppendLine("        <div id='divLoja' align='left'>");

            txtBuilder.AppendLine("             <table cellpadding='0' cellspacing='0' style='width: 800pt; padding: 0px; color: black;");
            txtBuilder.AppendLine("                 font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            txtBuilder.AppendLine("                 border: 1px solid #ccc;'>");
            txtBuilder.AppendLine("                 <tr style='background-color: #ccc;'>");
            txtBuilder.AppendLine("                     <td>");
            txtBuilder.AppendLine("                         &nbsp;");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: center;'>");
            txtBuilder.AppendLine("                          &nbsp;");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: center;'>");
            txtBuilder.AppendLine("                          Produto");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: left;'>");
            txtBuilder.AppendLine("                          Nome");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: left;'>");
            txtBuilder.AppendLine("                          Cor");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: center;'>");
            txtBuilder.AppendLine("                          Grade");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: center;'>");
            txtBuilder.AppendLine("                          Qtde Solicitada");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                 </tr>");

            var coluna = 1;
            var qtde = 0;
            foreach (var pp in produtoTransSolLis)
            {
                txtBuilder.AppendLine("                 <tr style='background-color: #fff;'>");
                txtBuilder.AppendLine("                     <td style='text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                txtBuilder.AppendLine("                         " + coluna.ToString());
                txtBuilder.AppendLine("                     </td>");
                txtBuilder.AppendLine("                     <td style='text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");

                var imgSrc = "http://intranetweb.ddns.net:8585/Fotos/" + pp.PRODUTO.Trim() + pp.COR.Trim() + ".jpg";

                txtBuilder.AppendLine("                          <img src='" + imgSrc + "' width='30%' alt=''>");
                txtBuilder.AppendLine("                     </td>");
                txtBuilder.AppendLine("                     <td style='text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                txtBuilder.AppendLine("                          " + pp.PRODUTO.Trim());
                txtBuilder.AppendLine("                     </td>");
                txtBuilder.AppendLine("                     <td style='text-align: left; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                txtBuilder.AppendLine("                          " + pp.NOME);
                txtBuilder.AppendLine("                     </td>");
                txtBuilder.AppendLine("                     <td style='text-align: left; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                txtBuilder.AppendLine("                          " + prodController.ObterCoresBasicas(pp.COR).DESC_COR.Trim());
                txtBuilder.AppendLine("                     </td>");
                txtBuilder.AppendLine("                     <td style='text-align: center; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;'>");
                txtBuilder.AppendLine("                          " + pp.TAMANHO);
                txtBuilder.AppendLine("                     </td>");
                txtBuilder.AppendLine("                     <td style='text-align: center; border-bottom: 1px solid #ccc;'>");
                txtBuilder.AppendLine("                          " + pp.QTDE_SOL.ToString());
                txtBuilder.AppendLine("                     </td>");
                txtBuilder.AppendLine("                 </tr>");

                coluna += 1;
                qtde += pp.QTDE_SOL;
            }

            txtBuilder.AppendLine("                 <tr style='background-color: #fff;'>");
            txtBuilder.AppendLine("                     <td>");
            txtBuilder.AppendLine("                          &nbsp;");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: left; border-right: 1px solid #ccc;'>");
            txtBuilder.AppendLine("                          Total");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: left; border-right: 1px solid #ccc;'>");
            txtBuilder.AppendLine("                          &nbsp;");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: left; border-right: 1px solid #ccc;'>");
            txtBuilder.AppendLine("                          &nbsp;");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: left; border-right: 1px solid #ccc;'>");
            txtBuilder.AppendLine("                          &nbsp;");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: left; border-right: 1px solid #ccc;'>");
            txtBuilder.AppendLine("                          &nbsp;");
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                     <td style='text-align: center; border-right: 1px solid #ccc;'>");
            txtBuilder.AppendLine("                          " + (qtde).ToString());
            txtBuilder.AppendLine("                     </td>");
            txtBuilder.AppendLine("                 </tr>");

            txtBuilder.AppendLine("             </table>");

            txtBuilder.AppendLine("        </div>");
            txtBuilder.AppendLine("        <br />");
            txtBuilder.AppendLine("        <br />");
            txtBuilder.AppendLine("        <span>Enviado por: " + (baseController.BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            txtBuilder.AppendLine("    </div>");
            txtBuilder.AppendLine("</body>");
            txtBuilder.AppendLine("</html>");

            return txtBuilder.ToString();
        }
        #endregion



    }
}