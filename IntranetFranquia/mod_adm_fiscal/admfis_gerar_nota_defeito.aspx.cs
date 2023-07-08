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
using System.Text;
using System.Collections;

namespace Relatorios
{
    public partial class admfis_gerar_nota_defeito : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarFiliais();
            }
        }

        private void CarregarFiliais()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                {
                    var filial = baseController.BuscaFiliais(usuario);

                    filial.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "Selecione" });
                    ddlFilial.DataSource = filial;
                    ddlFilial.DataBind();
                }
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarNotaRetirada();

                gvCDLugzy.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCDLugzy.DataBind();
                gvCDLugzyTransito.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCDLugzyTransito.DataBind();
                gvCmaxNova.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCmaxNova.DataBind();
                gvCmaxNovaMostruario.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCmaxNovaMostruario.DataBind();
                gvCmaxNovaTransito.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCmaxNovaTransito.DataBind();
                gvCDLucianaTransito.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCDLucianaTransito.DataBind();
                gvCDMostruario.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCDMostruario.DataBind();
                gvCDTagzy.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvCDTagzy.DataBind();
                gvOutros.DataSource = new List<NOTA_RETIRADA_ITEM>();
                gvOutros.DataBind();

                fsCDLugzy.Visible = false;
                fsCDLugzyTransito.Visible = false;
                fsCmaxNova.Visible = false;
                fsCmaxNovaMostruario.Visible = false;
                fsCmaxNovaTransito.Visible = false;
                fsCDLucianaTransito.Visible = false;
                fsCDMostruario.Visible = false;
                fsCDTagzy.Visible = false;
                fsSemSaldo.Visible = false;

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void CarregarNotaRetirada()
        {

            var notaRet = baseController.BuscaNotasRetiradaEmAberto(Convert.ToInt32(ddlFilial.SelectedValue));

            gvNotaRetirada.DataSource = notaRet;
            gvNotaRetirada.DataBind();

        }
        protected void gvNotaRetirada_RowDataBound(object sender, GridViewRowEventArgs e)
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


                Button btAbrirNota = e.Row.FindControl("btAbrirNota") as Button;
                if (btAbrirNota != null)
                {
                    btAbrirNota.CommandArgument = notaRetirada.CODIGO_NOTA_RETIRADA.ToString();
                    if (notaRetirada.FLAG_ARQUIVO == 1)
                        btAbrirNota.Enabled = true;
                    else
                        btAbrirNota.Enabled = false;
                }



                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;
                if (literalFilial != null)
                {
                    var filial = baseController.BuscaFilialCodigo(Convert.ToInt32(ddlFilial.SelectedValue));
                    if (filial != null)
                        literalFilial.Text = filial.FILIAL;
                }
            }
        }

        protected void btRegistrarNota_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var bt = (Button)sender;
                var codNotaRetirada = Convert.ToInt32(bt.CommandArgument);

                var notaRetirada = usuarioController.BuscaNotaRetirada(codNotaRetirada);
                if (notaRetirada != null)
                {

                    GridViewRow row = (GridViewRow)bt.NamingContainer;

                    if (row != null)
                    {

                        TextBox txtNFCDLugzy = (TextBox)row.FindControl("txtNFCDLugzy");
                        TextBox txtNFCDLugzyTransito = (TextBox)row.FindControl("txtNFCDLugzyTransito");
                        TextBox txtNFCmaxNova = (TextBox)row.FindControl("txtNFCmaxNova");
                        TextBox txtNFCmaxNovaMostruario = (TextBox)row.FindControl("txtNFCmaxNovaMostruario");
                        TextBox txtNFCmaxNovaTransito = (TextBox)row.FindControl("txtNFCmaxNovaTransito");
                        TextBox txtNFLucianaTransito = (TextBox)row.FindControl("txtNFLucianaTransito");
                        TextBox txtNFCDMostruario = (TextBox)row.FindControl("txtNFCDMostruario");
                        TextBox txtNFCDTagzy = (TextBox)row.FindControl("txtNFCDTagzy");


                        if (txtNFCDLugzy.Text.Equals("") &&
                            txtNFCDLugzyTransito.Text.Equals("") &&
                            txtNFCmaxNova.Text.Equals("") &&
                            txtNFCmaxNovaMostruario.Text.Equals("") &&
                            txtNFCmaxNovaTransito.Text.Equals("") &&
                            txtNFLucianaTransito.Text.Equals("") &&
                            txtNFCDMostruario.Text.Equals("") &&
                            txtNFCDTagzy.Text.Equals(""))
                        {
                            labErro.Text = "Informe pelo menos um número de Nota Fiscal...";
                            return;
                        }

                        notaRetirada.NUMERO_NOTA_LUGZI = "0";
                        notaRetirada.NUMERO_NOTA_LUGZI_TRAN = "0";
                        notaRetirada.NUMERO_NOTA_CMAX = "0";
                        notaRetirada.NUMERO_NOTA_CMAX_MOSTR = "0";
                        notaRetirada.NUMERO_NOTA_CMAX_TRAN = "0";
                        notaRetirada.NUMERO_NOTA_LUCIANA_TRAN = "0";
                        notaRetirada.NUMERO_NOTA_MOSTRUARIO = "0";
                        notaRetirada.NUMERO_NOTA_TAGZY = "0";

                        if (!txtNFCDLugzy.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_LUGZI = txtNFCDLugzy.Text;
                        if (!txtNFCDLugzyTransito.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_LUGZI_TRAN = txtNFCDLugzyTransito.Text;
                        if (!txtNFCmaxNova.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_CMAX = txtNFCmaxNova.Text;
                        if (!txtNFCmaxNovaMostruario.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_CMAX_MOSTR = txtNFCmaxNovaMostruario.Text;
                        if (!txtNFCmaxNovaTransito.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_CMAX_TRAN = txtNFCmaxNovaTransito.Text;
                        if (!txtNFLucianaTransito.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_LUCIANA_TRAN = txtNFLucianaTransito.Text;
                        if (!txtNFCDMostruario.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_MOSTRUARIO = txtNFCDMostruario.Text;
                        if (!txtNFCDTagzy.Text.Equals(""))
                            notaRetirada.NUMERO_NOTA_TAGZY = txtNFCDTagzy.Text;

                        notaRetirada.NUMERO_NOTA_HBF = "0";
                        notaRetirada.NUMERO_NOTA_CALCADOS = "0";
                        notaRetirada.NUMERO_NOTA_OUTROS = "0";

                        notaRetirada.DATA_NOTA_LUGZI = DateTime.Now;
                        notaRetirada.DATA_NOTA_LUGZI_TRAN = DateTime.Now;
                        notaRetirada.DATA_NOTA_CMAX = DateTime.Now;
                        notaRetirada.DATA_NOTA_CMAX_MOSTR = DateTime.Now;
                        notaRetirada.DATA_NOTA_CMAX_TRAN = DateTime.Now;
                        notaRetirada.DATA_NOTA_LUCIANA_TRAN = DateTime.Now;
                        notaRetirada.DATA_NOTA_MOSTRUARIO = DateTime.Now;
                        notaRetirada.DATA_NOTA_TAGZY = DateTime.Now;
                        notaRetirada.DATA_NOTA_HBF = DateTime.Now;
                        notaRetirada.DATA_NOTA_CALCADOS = DateTime.Now;
                        notaRetirada.DATA_NOTA_OUTROS = DateTime.Now;

                        notaRetirada.BAIXADO = false;

                        usuarioController.AtualizaNotaRetirada(notaRetirada);
                        labErro.Text = "Registro atualizado com Sucesso";
                    }
                }


            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM item = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            if (item != null)
            {
                Literal litOrigemDefeito = e.Row.FindControl("litOrigemDefeito") as Literal;
                litOrigemDefeito.Text = baseController.BuscaOrigemDefeito(item.CODIGO_ORIGEM_DEFEITO.ToString()).DESCRICAO_ORIGEM_DEFEITO;

                Literal litDefeito = e.Row.FindControl("litDefeito") as Literal;
                litDefeito.Text = baseController.BuscaDefeito(item.CODIGO_DEFEITO.ToString()).DESCRICAO_DEFEITO;
            }
        }
        //protected void btGerarArquivo_Click(object sender, EventArgs e)
        //{
        //    USUARIO usuario = (USUARIO)Session["USUARIO"];
        //    labErro.Text = "";

        //    Button btGerarArquivo = sender as Button;

        //    if (btGerarArquivo != null)
        //    {
        //        List<NOTA_RETIRADA_ITEM> itens = usuarioController.BuscaNotaRetiradaItens(Convert.ToInt32(btGerarArquivo.CommandArgument));

        //        if (itens != null)
        //        {
        //            foreach (NOTA_RETIRADA_ITEM item in itens)
        //            {
        //                PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorDescricao(item.CODIGO_PRODUTO.ToString(), item.COR_PRODUTO.Trim());

        //                if (produtoCor != null)
        //                {
        //                    PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CODIGO_PRODUTO.ToString(), produtoCor.COR_PRODUTO.Trim(), item.TAMANHO);

        //                    if (produtoBarra != null)
        //                    {
        //                        if (item.NF_ENTRADA == "999999") // produto sem saldo
        //                            item.TIPO_NOTA = "outros";
        //                        else
        //                        {
        //                            if (produtoBarra.PRODUTO.Substring(0, 1) == "6")
        //                                item.TIPO_NOTA = "cmax";
        //                            else
        //                                item.TIPO_NOTA = "lugzi";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        item.TIPO_NOTA = "outros";
        //                    }

        //                    usuarioController.AtualizaNotaRetiradaItem(item);
        //                }
        //            }
        //        }

        //        var itensNotaCmax = usuarioController.BuscaNotaRetiradaCmax(Convert.ToInt32(btGerarArquivo.CommandArgument));
        //        gvCmax.DataSource = itensNotaCmax;
        //        gvCmax.DataBind();

        //        var itensNotaLugzi = usuarioController.BuscaNotaRetiradaLugzi(Convert.ToInt32(btGerarArquivo.CommandArgument));
        //        gvLugzi.DataSource = itensNotaLugzi;
        //        gvLugzi.DataBind();

        //        var itensNotaOutros = usuarioController.BuscaNotaRetiradaOutros(Convert.ToInt32(btGerarArquivo.CommandArgument));
        //        gvOutros.DataSource = itensNotaOutros;
        //        gvOutros.DataBind();

        //        var codigoFilial = ddlFilial.SelectedValue.Trim();
        //        var romaneiosSaidaCmax = new List<string>();
        //        var romaneiosSaidaHandbook = new List<string>();

        //        string nome_arquivo_cmax = "";
        //        string nome_arquivo_outros = "";
        //        string nome_arquivo_lugzi = "";

        //        bool arquivoCmax = false;
        //        bool arquivoLugzi = false;
        //        bool arquivoOutro = false;

        //        try
        //        {
        //            var geraRomaneio = baseController.ObterFilialIntranet(codigoFilial);

        //            if (geraRomaneio.GERA_ROMANEIO_HANDBOOK)
        //            {
        //                //romaneios
        //                //gerar arquivos cmax
        //                var itensHandbook = new List<NOTA_RETIRADA_ITEM>();
        //                itensHandbook.AddRange(itensNotaLugzi);
        //                var notas = itensHandbook.GroupBy(p => new { NF_ENTRADA = p.NF_ENTRADA, SERIE_NF_ENTRADA = p.SERIE_NF_ENTRADA, FILIAL = p.FILIAL, FILIAL_ORIGEM = p.FILIAL_ORIGEM }).Select(
        //                    x => new NOTA_RETIRADA_ITEM
        //                    {
        //                        NF_ENTRADA = x.Key.NF_ENTRADA,
        //                        SERIE_NF_ENTRADA = x.Key.SERIE_NF_ENTRADA,
        //                        FILIAL = x.Key.FILIAL,
        //                        FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM
        //                    });

        //                foreach (var n in notas.Where(p => p.NF_ENTRADA != "999999"))
        //                {
        //                    var rom = InserirRomaneioSaida(n, "2");
        //                    romaneiosSaidaHandbook.Add(rom);
        //                }
        //            }

        //            if (geraRomaneio.GERA_ROMANEIO_CMAX)
        //            {
        //                //romaneios
        //                //gerar arquivos cmax
        //                var notas = itensNotaCmax.GroupBy(p => new { NF_ENTRADA = p.NF_ENTRADA, SERIE_NF_ENTRADA = p.SERIE_NF_ENTRADA, FILIAL = p.FILIAL, FILIAL_ORIGEM = p.FILIAL_ORIGEM }).Select(
        //                    x => new NOTA_RETIRADA_ITEM
        //                    {
        //                        NF_ENTRADA = x.Key.NF_ENTRADA,
        //                        SERIE_NF_ENTRADA = x.Key.SERIE_NF_ENTRADA,
        //                        FILIAL = x.Key.FILIAL,
        //                        FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM
        //                    });


        //                foreach (var n in notas.Where(p => p.NF_ENTRADA != "999999"))
        //                {
        //                    var rom = InserirRomaneioSaida(n, "1");
        //                    romaneiosSaidaCmax.Add(rom);
        //                }
        //            }

        //            string data = "";
        //            string hora = "";

        //            data = baseController.BuscaDataBanco().data.ToString("dd") + baseController.BuscaDataBanco().data.ToString("MM") + baseController.BuscaDataBanco().data.ToString("yyyy");
        //            hora = baseController.BuscaDataBanco().data.ToString("hh") + baseController.BuscaDataBanco().data.ToString("mm") + baseController.BuscaDataBanco().data.ToString("ss");

        //            nome_arquivo_cmax = "C:\\bkp\\" + ddlFilial.SelectedItem.Text.Trim().Replace(" ", "_") + "_" + "cmax" + "_" + data + "_" + hora + ".txt";
        //            nome_arquivo_lugzi = "C:\\bkp\\" + ddlFilial.SelectedItem.Text.Trim().Replace(" ", "_") + "_" + "lugzi" + "_" + data + "_" + hora + ".txt";
        //            nome_arquivo_outros = "C:\\bkp\\" + ddlFilial.SelectedItem.Text.Trim().Replace(" ", "_") + "_" + "outros-semsaldo" + "_" + data + "_" + hora + ".txt";

        //            if (System.IO.File.Exists(nome_arquivo_cmax))
        //                System.IO.File.Delete(nome_arquivo_cmax);

        //            if (System.IO.File.Exists(nome_arquivo_lugzi))
        //                System.IO.File.Delete(nome_arquivo_lugzi);

        //            if (System.IO.File.Exists(nome_arquivo_outros))
        //                System.IO.File.Delete(nome_arquivo_outros);

        //            System.IO.File.Create(nome_arquivo_cmax).Close();
        //            System.IO.File.Create(nome_arquivo_outros).Close();
        //            System.IO.File.Create(nome_arquivo_lugzi).Close();

        //            System.IO.TextWriter arquivo_cmax = System.IO.File.AppendText(nome_arquivo_cmax);
        //            System.IO.TextWriter arquivo_outros = System.IO.File.AppendText(nome_arquivo_outros);
        //            System.IO.TextWriter arquivo_lugzi = System.IO.File.AppendText(nome_arquivo_lugzi);

        //            itens = usuarioController.BuscaNotaRetiradaItens(Convert.ToInt32(btGerarArquivo.CommandArgument));

        //            if (itens != null)
        //            {
        //                foreach (NOTA_RETIRADA_ITEM item in itens)
        //                {
        //                    PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorDescricao(item.CODIGO_PRODUTO.ToString(), item.COR_PRODUTO.Trim());

        //                    if (produtoCor != null)
        //                    {
        //                        PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CODIGO_PRODUTO.ToString(), produtoCor.COR_PRODUTO, item.TAMANHO);

        //                        if (produtoBarra != null)
        //                        {
        //                            if (item.TIPO_NOTA.Trim().Equals("cmax"))
        //                            {
        //                                arquivo_cmax.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
        //                                arquivoCmax = true;
        //                            }
        //                            if (item.TIPO_NOTA.Trim().Equals("outros"))
        //                            {
        //                                arquivo_outros.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
        //                                arquivoOutro = true;

        //                            }
        //                            if (item.TIPO_NOTA.Trim().Equals("lugzi"))
        //                            {
        //                                arquivo_lugzi.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
        //                                arquivoLugzi = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            arquivo_outros.WriteLine(item.CODIGO_PRODUTO.ToString() + ",01");
        //                            arquivoOutro = true;
        //                        }
        //                    }
        //                }

        //                var notaRetirada = baseController.BuscaNotaRetirada(Convert.ToInt32(btGerarArquivo.CommandArgument));
        //                if (notaRetirada != null)
        //                {
        //                    notaRetirada.FLAG_ARQUIVO = 1;
        //                    usuarioController.AtualizaNotaRetirada(notaRetirada);
        //                }

        //                arquivo_cmax.Close();
        //                arquivo_outros.Close();
        //                arquivo_lugzi.Close();

        //                arquivo_cmax.Dispose();
        //                arquivo_outros.Dispose();
        //                arquivo_lugzi.Dispose();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            foreach (var r in romaneiosSaidaCmax)
        //                baseController.ExcluirRomaneioEstoqueSaida(r, ddlFilial.SelectedItem.Text);
        //            foreach (var r in romaneiosSaidaHandbook)
        //                baseController.ExcluirRomaneioEstoqueSaida(r, ddlFilial.SelectedItem.Text);

        //            labErro.Text = ex.Message;
        //            return;
        //        }

        //        try
        //        {
        //            MailMessage mail = new MailMessage();
        //            mail.From = new MailAddress(usuario.EMAIL);
        //            mail.To.Add("rodrigo.garcia@hbf.com.br");
        //            mail.To.Add("sueli.ponta@hbf.com.br");
        //            mail.To.Add("leandro.bevilaqua@hbf.com.br");

        //            mail.Subject = "[Intranet] - Produtos c/ Defeito para Transferência/Devolução";

        //            if (arquivoCmax)
        //                mail.Attachments.Add(new Attachment(nome_arquivo_cmax));
        //            if (arquivoLugzi)
        //                mail.Attachments.Add(new Attachment(nome_arquivo_lugzi));
        //            if (arquivoOutro)
        //                mail.Attachments.Add(new Attachment(nome_arquivo_outros));

        //            mail.IsBodyHtml = true;
        //            mail.Body = MontarCorpoEmail(ddlFilial.SelectedItem.Text.Trim(), romaneiosSaidaCmax, romaneiosSaidaHandbook);
        //            //mail.BodyEncoding = System.Text.Encoding.Default;

        //            //mail.ReplyTo = new MailAddress(this.EmailUsuario);

        //            SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
        //            smtp.Port = 587;
        //            smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

        //            smtp.Send(mail);
        //        }
        //        catch (Exception ex)
        //        {
        //            labErro.Text = ex.Message + " Erro no envio de email";
        //            return;
        //        }

        //        btGerarArquivo.Enabled = false;
        //    }
        //}

        protected void btGerarArquivo_Click(object sender, EventArgs e)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];
            labErro.Text = "";

            Button btGerarArquivo = sender as Button;

            if (btGerarArquivo != null)
            {
                List<NOTA_RETIRADA_ITEM> itens = usuarioController.BuscaNotaRetiradaItens(Convert.ToInt32(btGerarArquivo.CommandArgument));

                if (itens != null)
                {
                    foreach (NOTA_RETIRADA_ITEM item in itens)
                    {
                        if (item.NF_ENTRADA == "999999") // produto sem saldo
                            item.TIPO_NOTA = "outros";
                        else
                            item.TIPO_NOTA = item.FILIAL_ORIGEM.Trim();

                        usuarioController.AtualizaNotaRetiradaItem(item);
                    }
                }

                var codigoNotaRetirada = Convert.ToInt32(btGerarArquivo.CommandArgument);
                hidCodigoNotaRetirada.Value = codigoNotaRetirada.ToString();

                CarregarItensNotaRetirada(codigoNotaRetirada);

                var codigoFilial = ddlFilial.SelectedValue.Trim();
                Hashtable hashRomaneios = new Hashtable();
                var arquivos = new List<string>();


                try
                {

                    var listaItens = usuarioController.BuscaNotaRetiradaItens(codigoNotaRetirada);

                    var filiaisOrigem = listaItens.GroupBy(p => new { FILIAL_ORIGEM = p.FILIAL_ORIGEM.Trim().ToUpper() }).Select(
                        x => new NOTA_RETIRADA_ITEM
                        {
                            FILIAL_ORIGEM = x.Key.FILIAL_ORIGEM
                        });

                    foreach (var fOrigem in filiaisOrigem)
                    {
                        var notas = listaItens.Where(p => p.FILIAL_ORIGEM.Trim().ToUpper() == fOrigem.FILIAL_ORIGEM.Trim().ToUpper()).GroupBy(p => new { NF_ENTRADA = p.NF_ENTRADA.Trim(), SERIE_NF_ENTRADA = p.SERIE_NF_ENTRADA.Trim(), FILIAL = p.FILIAL.Trim().ToUpper(), FILIAL_ORIGEM = p.FILIAL_ORIGEM.Trim().ToUpper() }).Select(
                            x => new NOTA_RETIRADA_ITEM
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

                        var nomeArquivo = "C:\\bkp\\" + ddlFilial.SelectedItem.Text.Trim().Replace(" ", "_") + "--" + fOrigem.FILIAL_ORIGEM.Trim().Replace("(", "").Replace(")", "").Trim().Replace(" ", "_").Trim() + "_" + "retirada" + "" + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                        var listaItem = GerarArquivo(nomeArquivo, fOrigem.FILIAL_ORIGEM);
                        if (listaItem != null && listaItem.Count() > 0)
                        {
                            arquivos.Add(nomeArquivo);
                        }

                    }

                    //gerar arquivos outros
                    var nomeArquivoSemSaldo = "C:\\bkp\\sem_saldo_" + usuario.NOME_USUARIO.Trim().Replace(" ", "_") + "_" + "retirada" + "_" + DateTime.Today.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".txt";
                    var listaItemSemSaldo = GerarArquivoSemSaldo(nomeArquivoSemSaldo);
                    if (listaItemSemSaldo != null && listaItemSemSaldo.Count() > 0)
                    {
                        arquivos.Add(nomeArquivoSemSaldo);
                    }

                    var notaRetirada = baseController.BuscaNotaRetirada(codigoNotaRetirada);
                    if (notaRetirada != null)
                    {
                        notaRetirada.FLAG_ARQUIVO = 1;
                        usuarioController.AtualizaNotaRetirada(notaRetirada);
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

                    labErro.Text = ex.Message;
                    return;
                }

                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(usuario.EMAIL);
                    mail.To.Add("rodrigo.garcia@hbf.com.br");
                    mail.To.Add("sueli.ponta@hbf.com.br");
                    mail.To.Add("leandro.bevilaqua@hbf.com.br");

                    mail.Subject = "[Intranet] - Produtos c/ Defeito para Transferência/Devolução";

                    //if (arquivoCmax)
                    //    mail.Attachments.Add(new Attachment(nome_arquivo_cmax));
                    //if (arquivoLugzi)
                    //    mail.Attachments.Add(new Attachment(nome_arquivo_lugzi));
                    //if (arquivoOutro)
                    //    mail.Attachments.Add(new Attachment(nome_arquivo_outros));

                    foreach (var ar in arquivos)
                        mail.Attachments.Add(new Attachment(ar));

                    mail.IsBodyHtml = true;
                    mail.Body = MontarCorpoEmailPorFilial(ddlFilial.SelectedItem.Text.Trim(), hashRomaneios);
                    //mail.BodyEncoding = System.Text.Encoding.Default;

                    //mail.ReplyTo = new MailAddress(this.EmailUsuario);

                    SmtpClient smtp = new SmtpClient("smtp.hbf.com.br");
                    smtp.Port = 587;
                    smtp.Credentials = (ICredentialsByHost)new NetworkCredential(Constante.emailUser, Constante.emailPass);

                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message + " Erro no envio de email";
                    return;
                }

                btGerarArquivo.Enabled = false;
            }
        }

        private List<NOTA_RETIRADA_ITEM> GerarArquivo(string nomeArquivo, string filialOrigem)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var lista = usuarioController.BuscaNotaRetiradaPorFilial(Convert.ToInt32(hidCodigoNotaRetirada.Value), filialOrigem).Where(p => p.NF_ENTRADA != "999999").ToList();
            foreach (var item in lista)
            {
                PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorDescricao(item.CODIGO_PRODUTO.ToString(), item.COR_PRODUTO.Trim());
                if (produtoCor != null)
                {
                    PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CODIGO_PRODUTO.ToString(), produtoCor.COR_PRODUTO, item.TAMANHO.Trim().Replace("UNICO", "UN"));

                    if (produtoBarra != null)
                    {
                        arquivoW.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
                    }
                }
            }
            arquivoW.Close();
            arquivoW.Dispose();

            if (lista == null || lista.Count() <= 0)
                System.IO.File.Delete(nomeArquivo);

            return lista;
        }
        private List<NOTA_RETIRADA_ITEM> GerarArquivoSemSaldo(string nomeArquivo)
        {
            if (System.IO.File.Exists(nomeArquivo))
                System.IO.File.Delete(nomeArquivo);
            System.IO.File.Create(nomeArquivo).Close();
            System.IO.TextWriter arquivoW = System.IO.File.AppendText(nomeArquivo);

            var lista = usuarioController.BuscaNotaRetiradaOutros(Convert.ToInt32(hidCodigoNotaRetirada.Value));
            foreach (var item in lista)
            {
                PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorDescricao(item.CODIGO_PRODUTO.ToString(), item.COR_PRODUTO.Trim());
                if (produtoCor != null)
                {
                    PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CODIGO_PRODUTO.ToString(), produtoCor.COR_PRODUTO, item.TAMANHO.Trim().Replace("UNICO", "UN"));

                    if (produtoBarra != null)
                    {
                        arquivoW.WriteLine(produtoBarra.CODIGO_BARRA.Trim() + ",01");
                    }
                }
            }
            arquivoW.Close();
            arquivoW.Dispose();

            if (lista == null || lista.Count() <= 0)
                System.IO.File.Delete(nomeArquivo);

            return lista;
        }

        private string InserirRomaneioSaida(NOTA_RETIRADA_ITEM nota)
        {
            // verificar aqui
            var romaneioSaida = baseController.GerarRomaneioSaidaEstoqueDefeito(nota.FILIAL.Trim(), nota.FILIAL_ORIGEM.Trim(), nota.NF_ENTRADA.Trim(), nota.SERIE_NF_ENTRADA.Trim(), "").ROMANEIO_PRODUTO;
            return romaneioSaida;
        }

        private string MontarCorpoEmail(string filial, List<string> romaneiosSaidaCmax, List<string> romaneiosSaidaHandbook)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("");
            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Transferência/Devolução de Produtos Defeito</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <br />");
            if (romaneiosSaidaCmax != null && romaneiosSaidaCmax.Count() > 0)
            {
                sb.Append("    <div id='divDevDefeito1' align='left'>");
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
            if (romaneiosSaidaHandbook != null && romaneiosSaidaHandbook.Count() > 0)
            {
                sb.Append("    <div id='divDevDefeito2' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Romaneio de Saída HANDBOOK");
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
            sb.Append("    <br /><br /><span>E-mail enviado pela Filial: " + filial + "</span>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();

        }
        private string MontarCorpoEmailPorFilial(string filial, Hashtable hashRomaneios)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("");
            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Transferência/Devolução de Produtos Defeito</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <br />");
            if (hashRomaneios != null && hashRomaneios.Count > 0)
            {
                sb.Append("    <div id='divDevDefeito1' align='left'>");
                sb.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
                sb.Append("                        color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                sb.Append("                        background: white; white-space: nowrap;'>");
                sb.Append("            <tr>");
                sb.Append("                <td style='line-height: 20px;'>");
                sb.Append("                    <table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
                sb.Append("                                    width: 517pt'>");
                sb.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none; background-color:#B0C4DE;'>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Romaneios");
                sb.Append("                            </td>");
                sb.Append("                            <td style='width: 301px; border-left: 1px solid #000; border-right: 1px solid #000; font-weight:bold;'>");
                sb.Append("                                Filial Origem");
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

            sb.Append("    <br />");
            sb.Append("    <br /><br /><span>E-mail enviado pela Filial: " + filial + "</span>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();

        }


        protected void btAbrirNota_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            var codigoNotaRetirada = Convert.ToInt32(bt.CommandArgument);
            CarregarItensNotaRetirada(codigoNotaRetirada);
        }

        private void CarregarItensNotaRetirada(int codigoNotaRetirada)
        {
            var itensCDLugzy = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "CD - LUGZY");
            if (itensCDLugzy != null && itensCDLugzy.Count() > 0)
            {
                gvCDLugzy.DataSource = itensCDLugzy;
                gvCDLugzy.DataBind();
                fsCDLugzy.Visible = true;
            }

            var itensCDLugzyTran = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "CD LUGZY TRANSITO");
            if (itensCDLugzyTran != null && itensCDLugzyTran.Count() > 0)
            {
                gvCDLugzyTransito.DataSource = itensCDLugzyTran;
                gvCDLugzyTransito.DataBind();
                fsCDLugzyTransito.Visible = true;
            }

            var itensCmaxNova = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "C-MAX (NOVA)");
            if (itensCmaxNova != null && itensCmaxNova.Count() > 0)
            {
                gvCmaxNova.DataSource = itensCmaxNova;
                gvCmaxNova.DataBind();
                fsCmaxNova.Visible = true;
            }

            var itensCmaxNovaMostruario = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "C-MAX (NOVA) MOSTRUARIO");
            if (itensCmaxNovaMostruario != null && itensCmaxNovaMostruario.Count() > 0)
            {
                gvCmaxNovaMostruario.DataSource = itensCmaxNovaMostruario;
                gvCmaxNovaMostruario.DataBind();
                fsCmaxNovaMostruario.Visible = true;
            }

            var itensCmaxNovaTran = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "C-MAX (NOVA) TRANSITO");
            if (itensCmaxNovaTran != null && itensCmaxNovaTran.Count() > 0)
            {
                gvCmaxNovaTransito.DataSource = itensCmaxNovaTran;
                gvCmaxNovaTransito.DataBind();
                fsCmaxNovaTransito.Visible = true;
            }

            var itensCDLucianaTran = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "CD LUCIANA TRANSITO");
            if (itensCDLucianaTran != null && itensCDLucianaTran.Count() > 0)
            {
                gvCDLucianaTransito.DataSource = itensCDLucianaTran;
                gvCDLucianaTransito.DataBind();
                fsCDLucianaTransito.Visible = true;
            }

            var itensCDMostruario = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "CD MOSTRUARIO");
            if (itensCDMostruario != null && itensCDMostruario.Count() > 0)
            {
                gvCDMostruario.DataSource = itensCDMostruario;
                gvCDMostruario.DataBind();
                fsCDMostruario.Visible = true;
            }

            var itensCDTagzy = usuarioController.BuscaNotaRetiradaPorFilial(codigoNotaRetirada, "CD TAGZY");
            if (itensCDTagzy != null && itensCDTagzy.Count() > 0)
            {
                gvCDTagzy.DataSource = itensCDTagzy;
                gvCDTagzy.DataBind();
                fsCDTagzy.Visible = true;
            }

            var itensOutros = usuarioController.BuscaNotaRetiradaOutros(codigoNotaRetirada);
            if (itensOutros != null && itensOutros.Count() > 0)
            {
                gvOutros.DataSource = itensOutros;
                gvOutros.DataBind();
                fsSemSaldo.Visible = true;
            }
        }

    }
}
