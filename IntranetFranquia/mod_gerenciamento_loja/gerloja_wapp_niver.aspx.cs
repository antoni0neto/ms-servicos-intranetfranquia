using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;

namespace Relatorios
{
    public partial class gerloja_wapp_niver : System.Web.UI.Page
    {
        DesempenhoController desempenhoController = new DesempenhoController();
        CRMController crmController = new CRMController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);


            if (!IsPostBack)
            {
                try
                {
                    var usuario = (USUARIO)Session["USUARIO"];
                    txtUsuario.Text = usuario.USUARIO1.Trim();
                }
                catch (Exception)
                {
                }
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        public List<QUAR> ObterClientePorUsuario()
        {
            var cli = desempenhoController.ObterQUARUsuario(txtUsuario.Text.Trim());

            if (ddlDia.SelectedValue != "")
                cli = cli.Where(p => p.DIA == Convert.ToInt32(ddlDia.SelectedValue)).ToList();

            if (ddlMes.SelectedValue != "")
                cli = cli.Where(p => p.MES == Convert.ToInt32(ddlMes.SelectedValue)).ToList();

            if (txtPeriodoInicial.Text != "")
                cli = cli.Where(p => p.ULTIMA_COMPRA >= Convert.ToDateTime(txtPeriodoInicial.Text)).ToList();

            if (txtPeriodoInicial.Text != "")
                cli = cli.Where(p => p.ULTIMA_COMPRA >= Convert.ToDateTime(txtPeriodoInicial.Text)).ToList();

            if (txtPeriodoFinal.Text != "")
                cli = cli.Where(p => p.ULTIMA_COMPRA <= Convert.ToDateTime(txtPeriodoFinal.Text)).ToList();

            if (txtValorIni.Text != "")
                cli = cli.Where(p => p.VALOR_PAGO >= Convert.ToDecimal(txtValorIni.Text)).ToList();
            if (txtValorFim.Text != "")
                cli = cli.Where(p => p.VALOR_PAGO <= Convert.ToDecimal(txtValorFim.Text)).ToList();

            if (txtCPF.Text != "")
                cli = cli.Where(p => p.CPF.Trim() == txtCPF.Text.Trim()).ToList();

            if (ddlJaFalei.SelectedValue != "")
            {
                if (ddlJaFalei.SelectedValue == "S")
                    cli = cli.Where(p => p.DATA_BAIXA != null).ToList();
                else
                    cli = cli.Where(p => p.DATA_BAIXA == null).ToList();
            }

            if (ddJaFizContato.SelectedValue != "")
            {
                if (ddJaFizContato.SelectedValue == "S")
                    cli = cli.Where(p => p.CONTATO_FEITO == 'S').ToList();
                else
                    cli = cli.Where(p => p.CONTATO_FEITO != 'S').ToList();
            }

            if (ddlTemHandclub.SelectedValue != "")
            {
                if (ddlTemHandclub.SelectedValue == "S")
                    cli = cli.Where(p => p.HANDCLUB > 0).ToList();
                else
                    cli = cli.Where(p => p.HANDCLUB == null || p.HANDCLUB == 0).ToList();
            }

            return cli;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarVendas();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void CarregarVendas()
        {
            var cli = ObterClientePorUsuario();

            gvVendas.DataSource = cli;
            gvVendas.DataBind();
        }

        protected void gvVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    QUAR q = e.Row.DataItem as QUAR;

                    Literal litAbrirCliente = e.Row.FindControl("litAbrirCliente") as Literal;
                    litAbrirCliente.Text = CriarLinkCliente(q.CPF.Trim());

                    Literal litAbrirWapp = e.Row.FindControl("litAbrirWapp") as Literal;
                    litAbrirWapp.Text = CriarLinkWapp(q.CELULAR.Trim(), q.CLIENTE_VAREJO.Trim(), q.NOME_VENDEDOR, q.FILIAL, (q.HANDCLUB > 0), ((q.HANDCLUB > 0) ? Convert.ToDecimal(q.HANDCLUB) : 0));

                    Literal litUltimaCompra = e.Row.FindControl("litUltimaCompra") as Literal;
                    litUltimaCompra.Text = q.ULTIMA_COMPRA.ToString("dd/MM/yyyy");

                    Literal litValPago = e.Row.FindControl("litValPago") as Literal;
                    litValPago.Text = "R$ " + q.VALOR_PAGO.ToString("###,###,###,##0.00");

                    Literal litHandclub = e.Row.FindControl("litHandclub") as Literal;
                    litHandclub.Text = (q.HANDCLUB == null) ? "-" : "R$ " + Convert.ToDecimal(q.HANDCLUB).ToString("###,###,###,##0.00");

                    TextBox txtObs = e.Row.FindControl("txtObs") as TextBox;
                    txtObs.Text = q.OBS;

                    Literal litFaleiCliente = e.Row.FindControl("litFaleiCliente") as Literal;
                    litFaleiCliente.Text = (q.DATA_BAIXA == null) ? "-" : Convert.ToDateTime(q.DATA_BAIXA).ToString("dd/MM/yyyy HH:mm");

                    Button btJaFalei = e.Row.FindControl("btJaFalei") as Button;
                    btJaFalei.CommandArgument = q.CODIGO.ToString();
                    if (q.DATA_BAIXA != null)
                        btJaFalei.Visible = false;

                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        private string CriarLinkCliente(string cpf)
        {
            var link = "../mod_crm/crm_xxx.aspx?t=1&ca=" + cpf;
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }
        private string CriarLinkWapp(string celular, string cliente, string vendedor, string filial, bool temHandclub, decimal valorHandclub)
        {
            var msg = "";
            if (temHandclub)
                msg = "Oi " + cliente + ", tudo bem?%0aPassando para avisar que você tem handclub para resgatar no valor de R$" + valorHandclub.ToString("###,###,##0.00") + ". Você pode resgatar no site, no drive ou aqui com a gente na loja física. Bora resgatar, chama aqui que eu te explico como.";
            else
                msg = "Oi " + cliente + ", tudo bem?%0aPassando para avisar que estamos abertos, estamos atendendo por drive, nosso site e aqui na loja física. Me chama aqui, porque estamos cheios de novidades e com muitas saudades.";

            var link = "https://api.whatsapp.com/send?phone=" + celular + "&text=" + msg;
            var linkOk = "<a href=\"" + link + "\" target=\"_blank\" ><img alt='' src='../../Image/whatsapp.png' width='15px' /></a>";
            return linkOk;
        }

        protected void btJaFalei_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                var codigo = bt.CommandArgument;

                var quar = desempenhoController.ObterQUAR(Convert.ToInt32(codigo));
                if (quar != null)
                {
                    quar.DATA_BAIXA = DateTime.Now;
                    desempenhoController.AtualizarQUAR(quar);

                    bt.Visible = false;
                }

            }
            catch (Exception)
            {
            }
        }
        protected void btWapp_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                var vals = bt.CommandArgument.Split('|');
                var numero = vals[0];
                var cliente = vals[1];

                if (numero.Length == 13) //é celular
                    Response.Redirect("https://api.whatsapp.com/send?phone=" + numero + "&text=Oi " + cliente, "_blank", null);
            }
            catch (Exception)
            {
            }
        }

        protected void gvVendas_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<QUAR> cli = ObterClientePorUsuario();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            cli = cli.OrderBy(e.SortExpression + sortDirection);
            gvVendas.DataSource = cli;
            gvVendas.DataBind();
        }

        protected void txtObs_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = null;
            try
            {
                TextBox txtObs = (TextBox)sender;
                row = (GridViewRow)txtObs.NamingContainer;

                var codigo = Convert.ToInt32(gvVendas.DataKeys[row.RowIndex][0]);

                var quar = desempenhoController.ObterQUAR(Convert.ToInt32(codigo));
                if (quar != null)
                {
                    quar.OBS = txtObs.Text.Trim();
                    desempenhoController.AtualizarQUAR(quar);

                    row.BackColor = Color.FloralWhite;
                    row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");
                }
            }
            catch (Exception)
            {
                if (row != null)
                {
                    row.BackColor = Color.Red;
                    row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");
                }
            }
        }
    }
}
