using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Net.Mail;
using System.Net;
using DAL;

namespace Relatorios
{
    public partial class GerarNotasDefeito : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListFilial();
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                {
                    ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                    ddlFilial.DataBind();
                }
            }
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void btNotaRetirada_Click(object sender, EventArgs e)
        {
            CarregaGridViewNotaRetirada();
        }

        protected void GridViewNotaRetirada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA notaRetirada = e.Row.DataItem as NOTA_RETIRADA;

            if (notaRetirada != null)
            {
                Button buttonGravarArquivo = e.Row.FindControl("btGerarArquivo") as Button;

                if (buttonGravarArquivo != null)
                {
                    buttonGravarArquivo.CommandArgument = notaRetirada.CODIGO_NOTA_RETIRADA.ToString();

                    if (notaRetirada.FLAG_ARQUIVO == 1)
                        buttonGravarArquivo.Enabled = false;
                    else
                        buttonGravarArquivo.Enabled = true;
                }

                Button buttonRegistrarNota = e.Row.FindControl("btRegistrarNota") as Button;

                if (buttonRegistrarNota != null)
                    buttonRegistrarNota.CommandArgument = notaRetirada.CODIGO_NOTA_RETIRADA.ToString();

                if (notaRetirada.FLAG_ARQUIVO == 1)
                    buttonRegistrarNota.Enabled = true;
                else
                    buttonRegistrarNota.Enabled = false;

                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                {
                    FILIAI filial = baseController.BuscaFilialCodigo(Convert.ToInt32(ddlFilial.SelectedValue));

                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }
            }
        }

        private void CarregaGridViewNotaRetirada()
        {
            GridViewNotaRetirada.DataSource = baseController.BuscaNotasRetiradaEmAberto(Convert.ToInt32(ddlFilial.SelectedValue));
            GridViewNotaRetirada.DataBind();
        }

        protected void btRegistrarNota_Click(object sender, EventArgs e)
        {
            Button buttonRegistrarNota = sender as Button;

            if (buttonRegistrarNota != null)
                Response.Redirect(string.Format("RegistrarNotasDefeito.aspx?CodigoNotaRetirada={0}", buttonRegistrarNota.CommandArgument));
        }

        protected void btGerarArquivo_Click(object sender, EventArgs e)
        {
            string tipoNota = "";

            USUARIO usuario = (USUARIO)Session["USUARIO"];

            Button btGerarArquivo = sender as Button;

            if (btGerarArquivo != null)
            {
                List<NOTA_RETIRADA_ITEM> itens = usuarioController.BuscaNotaRetiradaItens(Convert.ToInt32(btGerarArquivo.CommandArgument));

                if (itens != null)
                {
                    foreach (NOTA_RETIRADA_ITEM item in itens)
                    {
                        PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorDescricao(item.CODIGO_PRODUTO.ToString(), item.COR_PRODUTO.Trim());

                        if (produtoCor != null)
                        {
                            PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CODIGO_PRODUTO.ToString(), produtoCor.COR_PRODUTO.Trim(), item.TAMANHO);

                            if (produtoBarra != null)
                            {
                                tipoNota = baseController.BuscaOrigemProduto(produtoBarra.CODIGO_BARRA);

                                if (tipoNota.Equals("cmax"))
                                    item.TIPO_NOTA = "cmax";

                                if (tipoNota.Equals("hbf"))
                                    item.TIPO_NOTA = "hbf";

                                if (tipoNota.Equals("calcados"))
                                    item.TIPO_NOTA = "calcados";

                                if (tipoNota.Equals("outros") || tipoNota.Equals(""))
                                    item.TIPO_NOTA = "outros";

                                if (tipoNota.Equals("lugzi"))
                                    item.TIPO_NOTA = "lugzi";
                            }
                            else
                            {
                                item.TIPO_NOTA = "outros";
                            }

                            usuarioController.AtualizaNotaRetiradaItem(item);
                        }
                    }
                }

                List<NOTA_RETIRADA_ITEM> itensNota = usuarioController.BuscaNotaRetiradaCmax(Convert.ToInt32(btGerarArquivo.CommandArgument));

                GridViewCmax.DataSource = itensNota;
                GridViewCmax.DataBind();

                itensNota = usuarioController.BuscaNotaRetiradaHbf(Convert.ToInt32(btGerarArquivo.CommandArgument));

                GridViewHbf.DataSource = itensNota;
                GridViewHbf.DataBind();

                itensNota = usuarioController.BuscaNotaRetiradaCalcados(Convert.ToInt32(btGerarArquivo.CommandArgument));

                GridViewCalcados.DataSource = itensNota;
                GridViewCalcados.DataBind();

                itensNota = usuarioController.BuscaNotaRetiradaOutros(Convert.ToInt32(btGerarArquivo.CommandArgument));

                GridViewOutros.DataSource = itensNota;
                GridViewOutros.DataBind();

                itensNota = usuarioController.BuscaNotaRetiradaLugzi(Convert.ToInt32(btGerarArquivo.CommandArgument));

                GridViewLugzi.DataSource = itensNota;
                GridViewLugzi.DataBind();

                string data = "";
                string hora = "";
                string nome_arquivo_cmax = "";
                string nome_arquivo_hbf = "";
                string nome_arquivo_calcados = "";
                string nome_arquivo_outros = "";
                string nome_arquivo_lugzi = "";

                try
                {
                    data = baseController.BuscaDataBanco().data.ToString("dd") + baseController.BuscaDataBanco().data.ToString("MM") + baseController.BuscaDataBanco().data.ToString("yyyy");
                    hora = baseController.BuscaDataBanco().data.ToString("hh") + baseController.BuscaDataBanco().data.ToString("mm") + baseController.BuscaDataBanco().data.ToString("ss");

                    nome_arquivo_cmax = "C:\\bkp\\" + usuario.NOME_USUARIO + "_" + "cmax" + "_" + data + "_" + hora + ".txt";
                    nome_arquivo_hbf = "C:\\bkp\\" + usuario.NOME_USUARIO + "_" + "hbf" + "_" + data + "_" + hora + ".txt";
                    nome_arquivo_calcados = "C:\\bkp\\" + usuario.NOME_USUARIO + "_" + "calcados" + "_" + data + "_" + hora + ".txt";
                    nome_arquivo_outros = "C:\\bkp\\" + usuario.NOME_USUARIO + "_" + "outros" + "_" + data + "_" + hora + ".txt";
                    nome_arquivo_lugzi = "C:\\bkp\\" + usuario.NOME_USUARIO + "_" + "lugzi" + "_" + data + "_" + hora + ".txt";

                    if (System.IO.File.Exists(nome_arquivo_cmax))
                        System.IO.File.Delete(nome_arquivo_cmax);

                    if (System.IO.File.Exists(nome_arquivo_hbf))
                        System.IO.File.Delete(nome_arquivo_hbf);

                    if (System.IO.File.Exists(nome_arquivo_calcados))
                        System.IO.File.Delete(nome_arquivo_calcados);

                    if (System.IO.File.Exists(nome_arquivo_outros))
                        System.IO.File.Delete(nome_arquivo_outros);

                    if (System.IO.File.Exists(nome_arquivo_lugzi))
                        System.IO.File.Delete(nome_arquivo_lugzi);

                    System.IO.File.Create(nome_arquivo_cmax).Close();
                    System.IO.File.Create(nome_arquivo_hbf).Close();
                    System.IO.File.Create(nome_arquivo_calcados).Close();
                    System.IO.File.Create(nome_arquivo_outros).Close();
                    System.IO.File.Create(nome_arquivo_lugzi).Close();

                    System.IO.TextWriter arquivo_cmax = System.IO.File.AppendText(nome_arquivo_cmax);
                    System.IO.TextWriter arquivo_hbf = System.IO.File.AppendText(nome_arquivo_hbf);
                    System.IO.TextWriter arquivo_calcados = System.IO.File.AppendText(nome_arquivo_calcados);
                    System.IO.TextWriter arquivo_outros = System.IO.File.AppendText(nome_arquivo_outros);
                    System.IO.TextWriter arquivo_lugzi = System.IO.File.AppendText(nome_arquivo_lugzi);

                    itens = usuarioController.BuscaNotaRetiradaItens(Convert.ToInt32(btGerarArquivo.CommandArgument));

                    if (itens != null)
                    {
                        foreach (NOTA_RETIRADA_ITEM item in itens)
                        {
                            PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorDescricao(item.CODIGO_PRODUTO.ToString(), item.COR_PRODUTO.Trim());

                            if (produtoCor != null)
                            {
                                PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CODIGO_PRODUTO.ToString(), produtoCor.COR_PRODUTO, item.TAMANHO);

                                if (produtoBarra != null)
                                {
                                    if (item.TIPO_NOTA.Trim().Equals("cmax"))
                                        arquivo_cmax.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
                                    if (item.TIPO_NOTA.Trim().Equals("hbf"))
                                        arquivo_hbf.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
                                    if (item.TIPO_NOTA.Trim().Equals("calcados"))
                                        arquivo_calcados.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
                                    if (item.TIPO_NOTA.Trim().Equals("outros"))
                                        arquivo_outros.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
                                    if (item.TIPO_NOTA.Trim().Equals("lugzi"))
                                        arquivo_lugzi.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
                                }
                                else
                                {
                                    arquivo_outros.WriteLine(item.CODIGO_PRODUTO.ToString() + ",01");
                                }
                            }
                        }

                        NOTA_RETIRADA notaRetirada = baseController.BuscaNotaRetirada(Convert.ToInt32(btGerarArquivo.CommandArgument));

                        if (notaRetirada != null)
                        {
                            notaRetirada.FLAG_ARQUIVO = 1;

                            usuarioController.AtualizaNotaRetirada(notaRetirada);
                        }

                        arquivo_cmax.Close();
                        arquivo_hbf.Close();
                        arquivo_calcados.Close();
                        arquivo_outros.Close();
                        arquivo_lugzi.Close();

                        arquivo_cmax.Dispose();
                        arquivo_hbf.Dispose();
                        arquivo_calcados.Dispose();
                        arquivo_outros.Dispose();
                        arquivo_lugzi.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    LabelFeedBack.Text = ex.Message + " Falta Código de Barra ou Cor ou TipoNota do produto !!!";
                    return;
                }

                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(usuario.EMAIL);
                    mail.To.Add("rodrigo.garcia@hbf.com.br");
                    mail.To.Add("sueli.ponta@hbf.com.br");
                    mail.Bcc.Add("rodrigo.garcia@hbf.com.br");
                    mail.Subject = "Produtos para transferência";
                    mail.Attachments.Add(new Attachment(nome_arquivo_cmax));
                    mail.Attachments.Add(new Attachment(nome_arquivo_hbf));
                    mail.Attachments.Add(new Attachment(nome_arquivo_calcados));
                    mail.Attachments.Add(new Attachment(nome_arquivo_outros));
                    mail.Attachments.Add(new Attachment(nome_arquivo_lugzi));

                    string msg = "Arquivo em anexo enviado pela filial: " + usuario.NOME_USUARIO;
                    mail.Body = msg;
                    //mail.BodyEncoding = System.Text.Encoding.Default;

                    //mail.ReplyTo = new MailAddress(this.EmailUsuario);

                    SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
                    smtp.Port = 587;
                    smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    LabelFeedBack.Text = ex.Message + " Erro no envio de email !!!";
                    return;
                }

                btGerarArquivo.Enabled = false;
            }
        }
    }
}
