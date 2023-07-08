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
    public partial class GerirNotaTransferencia : System.Web.UI.Page
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

                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btNotasTransfer_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") ||
                TextBoxDataInicio.Text == null ||
                TextBoxDataFim.Text.Equals("") ||
                TextBoxDataFim.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue == null)
                return;

            Session["FILIAL"] = ddlFilial.SelectedValue;

            CarregaGridViewNotas();

            btIncluirNota.Visible = true;
            GridViewResumo.Visible = false;
        }

        protected void btGerarArquivo_Click(object sender, EventArgs e)
        {
            Button btGerarArquivo = sender as Button;

            if (btGerarArquivo != null)
            {
                string nome_arquivo_cmax = "";
                string nome_arquivo_hbf = "";
                string nome_arquivo_calcados = "";
                string nome_arquivo_outros = "";
                string nome_arquivo_lugzi = "";
                string data = "";
                string hora = "";

                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                {
                    Button btSalvar = sender as Button;

                    bool flagCmax = false;
                    bool flagHbf = false;
                    bool flagCalcados = false;
                    bool flagOutros = false;
                    bool flagLugzi = false;

                    if (btSalvar != null)
                    {
                        List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];

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

                            string tipoProduto = "";

                            List<Sp_Busca_Nota_Transferencia_ItemResult> listaNotaTransferenciaItem = baseController.BuscaNotasTransferenciaItemAgrupado(Convert.ToInt32(btGerarArquivo.CommandArgument));

                            if (listaNotaTransferenciaItem != null)
                            {
                                foreach (Sp_Busca_Nota_Transferencia_ItemResult item in listaNotaTransferenciaItem)
                                {
                                    tipoProduto = baseController.BuscaOrigemProduto(item.barra);

                                    if (tipoProduto.Equals("cmax"))
                                    {
                                        flagCmax = true;

                                        arquivo_cmax.WriteLine(item.barra + "," + item.qtde);
                                    }

                                    if (tipoProduto.Equals("hbf"))
                                    {
                                        flagHbf = true;

                                        arquivo_hbf.WriteLine(item.barra + "," + item.qtde);
                                    }

                                    if (tipoProduto.Equals("calcados"))
                                    {
                                        flagCalcados = true;

                                        arquivo_calcados.WriteLine(item.barra + "," + item.qtde);
                                    }

                                    if (tipoProduto.Equals("outros"))
                                    {
                                        flagOutros = true;

                                        arquivo_outros.WriteLine(item.barra + "," + item.qtde);
                                    }

                                    if (tipoProduto.Equals("lugzi"))
                                    {
                                        flagLugzi = true;

                                        arquivo_lugzi.WriteLine(item.barra + "," + item.qtde);
                                    }
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
                            LabelFeedBack.Text = "Erro : " + ex.Message;
                            return;
                        }

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(usuario.EMAIL);

                            if (usuario.USUARIO1.Trim().Equals("edi"))
                            {
                                mail.To.Add("fernanda.sampaio@hbf.com.br");
                                mail.Bcc.Add("fernanda.sampaio@hbf.com.br");
                            }
                            else
                            {
                                mail.To.Add("rodrigo.garcia@hbf.com.br");
                                mail.Bcc.Add("rodrigo.garcia@hbf.com.br");
                            }

                            mail.Subject = "Produtos para transferência";

                            if (flagCmax)
                                mail.Attachments.Add(new Attachment(nome_arquivo_cmax));

                            if (flagHbf)
                                mail.Attachments.Add(new Attachment(nome_arquivo_hbf));

                            if (flagCalcados)
                                mail.Attachments.Add(new Attachment(nome_arquivo_calcados));

                            if (flagOutros)
                                mail.Attachments.Add(new Attachment(nome_arquivo_outros));

                            if (flagLugzi)
                                mail.Attachments.Add(new Attachment(nome_arquivo_lugzi));

                            string msg = "Arquivo em anexo enviado pela filial: " + usuario.NOME_USUARIO;

                            mail.Body = msg;
                            //mail.BodyEncoding = System.Text.Encoding.Default;

                            //mail.ReplyTo = new MailAddress(this.EmailUsuario);

                            SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
                            smtp.Port = 587;
                            smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

                            smtp.Send(mail);

                            NOTA_TRANSFERENCIA notaTransferencia = usuarioController.BuscaNotaTransferencia(Convert.ToInt32(btGerarArquivo.CommandArgument));

                            if (notaTransferencia != null)
                            {
                                notaTransferencia.DATA_FIM = DateTime.Now;

                                usuarioController.AtualizaNotaTransferencia(notaTransferencia);
                            }

                            Response.Redirect("~/FinalizadoComSucesso.aspx");
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

        protected void btIncluirNota_Click(object sender, EventArgs e)
        {
            Session["PRODUTO"] = new List<Produto>();

            Response.Redirect("~/EscanearProdutoNotaTransfer.aspx");
        }

        protected void btAlterarNota_Click(object sender, EventArgs e)
        {
            Button btAlterarNota = sender as Button;

            if (btAlterarNota != null)
            {
                Session["PRODUTO"] = new List<Produto>();

                Session["CONTADOR"] = 0;

                Response.Redirect(string.Format("EscanearProdutoNotaTransfer.aspx?CodigoNotaTransfer={0}", btAlterarNota.CommandArgument));
            }
        }

        protected void btResumoNota_Click(object sender, EventArgs e)
        {
            List<ResumoNotaTransfer> listaResumoNotaTransfer = new List<ResumoNotaTransfer>();

            Button btResumoNota = sender as Button;

            if (btResumoNota != null)
            {
                List<Sp_Busca_Nota_Transferencia_ItemResult> listaNotaTransferenciaItem = baseController.BuscaNotasTransferenciaItemAgrupado(Convert.ToInt32(btResumoNota.CommandArgument));

                if (listaNotaTransferenciaItem != null)
                {
                    foreach (Sp_Busca_Nota_Transferencia_ItemResult item in listaNotaTransferenciaItem)
                    {
                        ResumoNotaTransfer resumoNotaTransfer = new ResumoNotaTransfer();

                        resumoNotaTransfer.Barra = item.barra;
                        resumoNotaTransfer.QtEscaneada = Convert.ToInt32(item.qtde);

                        PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorCodigo(item.barra.Substring(0, 5), item.barra.Substring(5, 3));

                        if (produtoCor != null)
                            resumoNotaTransfer.Cor = produtoCor.DESC_COR_PRODUTO;

                        PRODUTO produto = baseController.BuscaProduto(item.barra.Substring(0, 5));

                        if (produto != null)
                            resumoNotaTransfer.Produto = produto.DESC_PRODUTO;

                        ESTOQUE_PRODUTO estoqueProduto = baseController.BuscaEstoqueLoja(item.barra.Substring(0, 5), item.barra.Substring(5, 3), ddlFilial.SelectedItem.ToString().Trim());

                        if (estoqueProduto != null)
                        {
                            if (estoqueProduto.ESTOQUE != null)
                                resumoNotaTransfer.QtEstoque = Convert.ToInt32(estoqueProduto.ESTOQUE);
                            else
                                resumoNotaTransfer.QtEstoque = 0;
                        }

                        listaResumoNotaTransfer.Add(resumoNotaTransfer);
                    }

                    GridViewResumo.DataSource = listaResumoNotaTransfer;
                    GridViewResumo.DataBind();

                    GridViewResumo.Visible = true;
                }
            }
        }

        private void CarregaGridViewNotas()
        {
            GridViewNotas.DataSource = baseController.BuscaNotasTransferencia(Convert.ToInt32(ddlFilial.SelectedValue), CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);
            GridViewNotas.DataBind();
        }

        protected void btExcluirNotas_Click(object sender, EventArgs e)
        {
            Button btExcluirNotas = sender as Button;

            if (btExcluirNotas != null)
            {
                usuarioController.ExcluirNotaTransferencia(Convert.ToInt32(btExcluirNotas.CommandArgument));
                usuarioController.ExcluirNotaTransferenciaItem(Convert.ToInt32(btExcluirNotas.CommandArgument));

                CarregaGridViewNotas();
            }
        }

        protected void GridViewNotas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_TRANSFERENCIA notaTransferencia = e.Row.DataItem as NOTA_TRANSFERENCIA;

            if (notaTransferencia != null)
            {
                Button btExcluirNotas = e.Row.FindControl("btExcluirNotas") as Button;

                if (btExcluirNotas != null)
                    btExcluirNotas.CommandArgument = notaTransferencia.CODIGO.ToString();

                Button btGerarArquivo = e.Row.FindControl("btGerarArquivo") as Button;

                if (btGerarArquivo != null)
                    btGerarArquivo.CommandArgument = notaTransferencia.CODIGO.ToString();

                Button btResumoNota = e.Row.FindControl("btResumoNota") as Button;

                if (btResumoNota != null)
                    btResumoNota.CommandArgument = notaTransferencia.CODIGO.ToString();

                Button btAlterarNota = e.Row.FindControl("btAlterarNota") as Button;

                if (btAlterarNota != null)
                    btAlterarNota.CommandArgument = notaTransferencia.CODIGO.ToString();
            }
        }
    }
}