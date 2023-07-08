using DAL;
using Relatorios.mod_ecom.emailMKT;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_email_criacao : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";

                if (Request.QueryString["email"] == null || Request.QueryString["email"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                var codigoEmail = Convert.ToInt32(Request.QueryString["email"].ToString());

                CarregarBlocos();

                if (codigoEmail > 0)
                {
                    var email = ecomController.ObterEmailPorCodigo(codigoEmail);
                    if (email != null)
                    {
                        txtTitulo.Text = email.NOME;
                        CarregarCorpoEmail(codigoEmail);
                    }
                }


            }

            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarBlocos()
        {
            var blocos = ecomController.ObterEmailBlocoPorFixo('N');

            blocos.Insert(0, new ECOM_EMAIL_BLOCO { CODIGO = 0, NOME = "Selecione" });
            ddlBloco.DataSource = blocos;
            ddlBloco.DataBind();
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (txtTitulo.Text.Trim() == "")
                {
                    labErro.Text = "Informe o título do email.";
                    return;
                }

                //criar email
                var email = new ECOM_EMAIL();
                email.NOME = txtTitulo.Text.Trim();
                email.DATA_CRIACAO = DateTime.Now;

                var codigoEmail = ecomController.InserirEmail(email);

                // criar fixos
                var blocosFixos = ecomController.ObterEmailBlocoPorFixo('S');
                foreach (var b in blocosFixos)
                {
                    var emailCorpo = new ECOM_EMAIL_CORPO();
                    emailCorpo.ECOM_EMAIL = codigoEmail;
                    emailCorpo.ECOM_EMAIL_BLOCO = b.CODIGO;
                    emailCorpo.ORDEM = b.ORDEM;
                    ecomController.InserirEmailCorpo(emailCorpo);
                }

                CarregarCorpoEmail(codigoEmail);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }

        private void CarregarCorpoEmail(int codigoEmail)
        {
            var corpoEmail = ecomController.ObterEmailCorpoPorCodigoEmail(codigoEmail);

            gvCorpoBloco.DataSource = corpoEmail;
            gvCorpoBloco.DataBind();

            hidCodigoEmail.Value = codigoEmail.ToString();
            pnlCorpo.Visible = true;
            txtTitulo.Enabled = false;
            btSalvar.Enabled = false;

            var ultOrdem = ecomController.ObterEmailCorpoUltimaOrdem(codigoEmail);
            txtPosicao.Text = ultOrdem.ToString();
        }

        protected void btAddBloco_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlBloco.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o bloco do email.";
                    return;
                }

                if (txtPosicao.Text.Trim() == "")
                {
                    labErro.Text = "Informe a posição do bloco.";
                    return;
                }

                var codigoEmail = Convert.ToInt32(hidCodigoEmail.Value);

                var emailCorpo = new ECOM_EMAIL_CORPO();
                emailCorpo.ECOM_EMAIL = codigoEmail;
                emailCorpo.ECOM_EMAIL_BLOCO = Convert.ToInt32(ddlBloco.SelectedValue);
                emailCorpo.ORDEM = Convert.ToInt32(txtPosicao.Text);
                var codigoEmailCorpo = ecomController.InserirEmailCorpo(emailCorpo);

                if (emailCorpo.ECOM_EMAIL_BLOCO == 6 || emailCorpo.ECOM_EMAIL_BLOCO == 7 || emailCorpo.ECOM_EMAIL_BLOCO == 8 || emailCorpo.ECOM_EMAIL_BLOCO == 9)
                {
                    var emailCorpoLink = new ECOM_EMAIL_CORPO_LINK();
                    emailCorpoLink.ECOM_EMAIL_CORPO = codigoEmailCorpo;
                    emailCorpoLink.ORDEM = 1;
                    ecomController.InserirEmailCorpoLink(emailCorpoLink);
                }


                CarregarCorpoEmail(codigoEmail);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvCorpoBloco_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_EMAIL_CORPO emailCorpo = e.Row.DataItem as ECOM_EMAIL_CORPO;

                    Literal litBloco = e.Row.FindControl("litBloco") as Literal;
                    litBloco.Text = emailCorpo.ECOM_EMAIL_BLOCO1.NOME;

                    Button btAtualizar = e.Row.FindControl("btAtualizar") as Button;

                    Button btAlterar = e.Row.FindControl("btAlterar") as Button;
                    btAlterar.CommandArgument = emailCorpo.CODIGO.ToString();

                    Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                    btExcluir.CommandArgument = emailCorpo.CODIGO.ToString();

                    ImageButton btUp = e.Row.FindControl("btUp") as ImageButton;
                    btUp.CommandArgument = emailCorpo.CODIGO.ToString();

                    ImageButton btDown = e.Row.FindControl("btDown") as ImageButton;
                    btDown.CommandArgument = emailCorpo.CODIGO.ToString();

                    Panel pnlConteudo = e.Row.FindControl("pnlConteudo") as Panel;

                    if (emailCorpo.ECOM_EMAIL_BLOCO1.FIXO == 'S')
                    {
                        btAtualizar.Visible = false;
                        btAlterar.Visible = false;
                        btExcluir.Visible = false;
                        btUp.Visible = false;
                        btDown.Visible = false;
                        pnlConteudo.Visible = false;
                    }

                    var html = (emailCorpo.ECOM_EMAIL_BLOCO1.HTML_INTRANET == null) ? emailCorpo.ECOM_EMAIL_BLOCO1.HTML : emailCorpo.ECOM_EMAIL_BLOCO1.HTML_INTRANET;
                    var conteudo = MontarConteudo(emailCorpo.CODIGO, emailCorpo.ECOM_EMAIL_BLOCO, html);
                    pnlConteudo.Controls.Add(conteudo);

                }
            }
        }
        protected void gvCorpoBloco_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvCorpoBloco.FooterRow;
            if (_footer != null)
            {
            }
        }
        protected void btUp_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            try
            {
                labErro.Text = "";
                var codigoEmailCorpo = Convert.ToInt32(bt.CommandArgument);
                var codigoEmail = Convert.ToInt32(hidCodigoEmail.Value);

                var emailCorpo = ecomController.ObterEmailCorpoPorCodigo(codigoEmailCorpo);
                if (emailCorpo != null)
                {
                    var emailCorpoUp = ecomController.ObterEmailCorpoPorOrdem(codigoEmail, (emailCorpo.ORDEM - 1));
                    if (emailCorpoUp != null)
                    {
                        emailCorpoUp.ORDEM = emailCorpoUp.ORDEM + 1;
                        ecomController.AtualizarEmailCorpo(emailCorpoUp);
                    }

                    emailCorpo.ORDEM = emailCorpo.ORDEM - 1;
                    ecomController.AtualizarEmailCorpo(emailCorpo);
                }

                CarregarCorpoEmail(codigoEmail);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btDown_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            try
            {
                labErro.Text = "";
                var codigoEmailCorpo = Convert.ToInt32(bt.CommandArgument);
                var codigoEmail = Convert.ToInt32(hidCodigoEmail.Value);

                var emailCorpo = ecomController.ObterEmailCorpoPorCodigo(codigoEmailCorpo);
                if (emailCorpo != null)
                {
                    var emailCorpoDown = ecomController.ObterEmailCorpoPorOrdem(codigoEmail, (emailCorpo.ORDEM + 1));
                    if (emailCorpoDown != null)
                    {
                        emailCorpoDown.ORDEM = emailCorpoDown.ORDEM - 1;
                        ecomController.AtualizarEmailCorpo(emailCorpoDown);
                    }

                    emailCorpo.ORDEM = emailCorpo.ORDEM + 1;
                    ecomController.AtualizarEmailCorpo(emailCorpo);
                }

                CarregarCorpoEmail(codigoEmail);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";

                var codigoEmailCorpo = Convert.ToInt32(bt.CommandArgument);
                ecomController.ExcluirEmailCorpo(codigoEmailCorpo);

                CarregarCorpoEmail(Convert.ToInt32(hidCodigoEmail.Value));

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";
                var codigoEmailCorpo = Convert.ToInt32(bt.CommandArgument);

                //abrir para alteracao
                var url = "fnAbrirTelaCadastroMaior('ecom_email_criacao_conteudo.aspx?emcorp=" + codigoEmailCorpo.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);

                CarregarCorpoEmail(Convert.ToInt32(hidCodigoEmail.Value));
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btCancelar_Click(object sender, EventArgs e)
        {
            hidCodigoEmail.Value = "";
            pnlCorpo.Visible = false;

            txtTitulo.Text = "";
            txtTitulo.Enabled = true;

            txtPosicao.Text = "1";
            ddlBloco.SelectedValue = "0";

            gvCorpoBloco.DataSource = new List<ECOM_EMAIL_CORPO>();
            gvCorpoBloco.DataBind();

            btSalvar.Enabled = true;

            labErro.Text = "";
        }
        private HtmlGenericControl MontarConteudo(int codigoEmailCorpo, int codigoEmailBloco, string templateHTML)
        {
            HtmlGenericControl divConteudo = new HtmlGenericControl();
            var conteudo = emailMKT.MontarConteudo(codigoEmailCorpo, codigoEmailBloco, templateHTML, false);
            divConteudo.InnerHtml = conteudo;
            return divConteudo;
        }

        protected void btVisualizar_Click(object sender, EventArgs e)
        {
            try
            {

                GerarEmailMKT(Convert.ToInt32(hidCodigoEmail.Value));

                CarregarCorpoEmail(Convert.ToInt32(hidCodigoEmail.Value));
            }
            catch (Exception)
            {
            }
        }

        private void GerarEmailMKT(int codigoEmail)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "ECOM_EMAIL_MKT_" + codigoEmail.ToString() + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarEmailMKT(codigoEmail));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private string MontarEmailMKT(int codigoEmail)
        {
            var texto = new StringBuilder();

            var emailCorpo = ecomController.ObterEmailCorpoPorCodigoEmail(codigoEmail);

            foreach (var em in emailCorpo)
            {
                var conteudo = emailMKT.MontarConteudo(em.CODIGO, em.ECOM_EMAIL_BLOCO, em.ECOM_EMAIL_BLOCO1.HTML, true);
                texto.AppendLine(conteudo);
            }

            return texto.ToString();
        }

        protected void btGerarHTML_Click(object sender, EventArgs e)
        {
            try
            {

                if (hidCodigoEmail.Value != "")
                {
                    //Abrir pop-up
                    var url = "fnAbrirTelaCadastroMaior('ecom_email_criacao_html.aspx?em=" + hidCodigoEmail.Value + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }

                CarregarCorpoEmail(Convert.ToInt32(hidCodigoEmail.Value));
            }
            catch (Exception)
            {
            }
        }

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarCorpoEmail(Convert.ToInt32(hidCodigoEmail.Value));
            }
            catch (Exception)
            {
            }
        }


    }
}
