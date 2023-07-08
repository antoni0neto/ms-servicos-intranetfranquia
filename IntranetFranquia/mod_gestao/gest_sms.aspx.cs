using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;


namespace Relatorios
{
    public partial class gest_sms : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gest_menu.aspx";

                CarregarCampanha();

                CarregarJQuery();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btEnviarTodos.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviarTodos, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarCampanha()
        {
            var c = lojaController.ObterSMSCampanha();
            if (c != null)
            {
                c.Insert(0, new SMS_CAMPANHA { CODIGO = "", CAMPANHA = "Selecione" });
                ddlCampanha.DataSource = c;
                ddlCampanha.DataBind();
            }
        }

        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvCliente.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion


        private List<SP_OBTER_SMS_CLIENTEResult> ObterSMSClientePorCampanha()
        {

            var clientes = lojaController.ObterSMSClientePorCampanha(ddlCampanha.SelectedValue, "");

            if (ddlEnviado.SelectedValue != "")
            {
                if (ddlEnviado.SelectedValue == "S")
                    clientes = clientes.Where(p => p.QTDE_MSG > 0).ToList();
                else
                    clientes = clientes.Where(p => p.QTDE_MSG <= 0).ToList();
            }

            return clientes;
        }
        private void CarregarClientes()
        {
            gvCliente.DataSource = ObterSMSClientePorCampanha();
            gvCliente.DataBind();

            var c = lojaController.ObterSMSCampanha(ddlCampanha.SelectedValue);
            txtMensagem.Text = c.MSG;

            CarregarJQuery();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                txtMensagem.Text = "";

                if (ddlCampanha.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Campanha";
                    CarregarJQuery();
                    return;
                }

                CarregarClientes();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID CLIENTES"
        protected void gvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_SMS_CLIENTEResult cli = e.Row.DataItem as SP_OBTER_SMS_CLIENTEResult;

                    Literal litUltimoEnvio = e.Row.FindControl("litUltimoEnvio") as Literal;
                    litUltimoEnvio.Text = (cli.DATA_ENVIO == null) ? "" : Convert.ToDateTime(cli.DATA_ENVIO).ToString("dd/MM/yyyy");

                    if (cli.QTDE_MSG > 0)
                        e.Row.BackColor = Color.LightYellow;
                }
            }
        }
        protected void gvCliente_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvCliente.FooterRow;
            if (_footer != null)
            {
            }
        }

        #endregion

        private string TratarMensagemHandclubMensal(string codigoCliente, string msg)
        {
            //var pontoCliente = lojaController.ObterPontosCampanhaHandclubSMS(codigoCliente);
            //if (pontoCliente == null || pontoCliente.VALOR <= 20)
            //    return "";

            //msg = msg.Replace("{{FILIAL}}", pontoCliente.FILIAL_FANTASIA.Trim());
            //msg = msg.Replace("{{VALOR}}", Convert.ToDecimal(pontoCliente.VALOR).ToString("###,###,##0.00"));
            //msg = msg.Replace("{{DATA_VENCIMENTO}}", Convert.ToDateTime(pontoCliente.DATA_VENCIMENTO).ToString("dd-MM-yyyy"));

            return msg;
        }
        private SMS_CLIENTE_HISTORICO EnviarSMS(string smsCampanha, string codigoCliente)
        {
            var cliCamp = lojaController.ObterSMSClientePorCampanha(smsCampanha, codigoCliente).FirstOrDefault();

            if (cliCamp.ERRO != null && cliCamp.ERRO.ToLower().Contains("is not a mobile number"))
                return null;

            var ddd = Convert.ToInt32(cliCamp.DDD);
            var telefone = Convert.ToInt32(cliCamp.TELEFONE.Replace("-", ""));

            if (txtMensagem.Text.Trim() == "")
                throw new Exception("Informe a mensagem para Envio.");

            var mensagem = TratarMensagemHandclubMensal(codigoCliente, txtMensagem.Text.Trim());

            if (mensagem != "")
            {
                var resp = lojaController.EnviarSMSCampanha(ddd, telefone, mensagem);
                resp.CODIGO_CLIENTE = codigoCliente;
                resp.SMS_CAMPANHA = smsCampanha;
                lojaController.InserirSMSClienteHist(resp);
                return resp;
            }

            return null;
        }
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            try
            {
                labErro.Text = "";
                GridViewRow row = (GridViewRow)b.NamingContainer;
                b.Enabled = false;

                string codigoCliente = gvCliente.DataKeys[row.RowIndex][0].ToString();
                string smsCampanha = gvCliente.DataKeys[row.RowIndex][1].ToString();

                EnviarSMS(smsCampanha, codigoCliente);

                CarregarClientes();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btEnviarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var smsCampanha = ddlCampanha.SelectedValue;
                var clientes = ObterSMSClientePorCampanha();

                foreach (var c in clientes)
                {
                    var cliSMSHist = EnviarSMS(smsCampanha, c.CODIGO_CLIENTE);
                    if (cliSMSHist != null && (cliSMSHist.ERRO.ToLower().Contains("authenticate") || cliSMSHist.ERRO.ToLower().Contains("is inactive")))
                    {
                        labErro.Text = cliSMSHist.ERRO;
                        btEnviarTodos.BackColor = Color.Red;
                        return;
                    }
                }

                CarregarClientes();

                labErro.Text = "SMSs enviados com sucesso.";
                btEnviarTodos.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }


    }
}


