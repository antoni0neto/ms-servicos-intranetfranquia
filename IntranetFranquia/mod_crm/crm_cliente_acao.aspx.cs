using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class crm_cliente_acao : System.Web.UI.Page
    {
        CRMController crmController = new CRMController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "crm_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";

                CarregarAcoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarAcoes()
        {
            var acoes = crmController.ObterAcao();
            acoes.Insert(0, new CRM_ACAO { CODIGO = 0, ACAO = "" });
            ddlAcao.DataSource = acoes;
            ddlAcao.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlAcao.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Ação.";
                    return;
                }

                CarregarClientes();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private List<CRM_CLIENTE_ACAO> ObterClienteAcao()
        {
            var clientes = crmController.ObterClienteAcaoPorCodigoAcao(Convert.ToInt32(ddlAcao.SelectedValue));
            if (txtCPF.Text != "")
                clientes = clientes.Where(p => p.CPF == txtCPF.Text.Trim()).ToList();

            if (txtNome.Text != "")
                clientes = clientes.Where(p => p.NOME.Contains(txtNome.Text.ToUpper().Trim())).ToList();

            if (ddlAberto.SelectedValue != "")
            {
                if (ddlAberto.SelectedValue == "S")
                    clientes = clientes.Where(p => p.DATA_BAIXA == null).ToList();
                else
                    clientes = clientes.Where(p => p.DATA_BAIXA != null).ToList();
            }

            return clientes;
        }
        private void CarregarClientes()
        {
            var clientes = ObterClienteAcao();
            gvCliente.DataSource = clientes;
            gvCliente.DataBind();
        }
        protected void gvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CRM_CLIENTE_ACAO cli = e.Row.DataItem as CRM_CLIENTE_ACAO;

                    Literal litAbrirCliente = e.Row.FindControl("litAbrirCliente") as Literal;
                    litAbrirCliente.Text = CriarLinkCliente(cli.CPF.Trim());

                    Literal litFilial = e.Row.FindControl("litFilial") as Literal;
                    litFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(cli.CODIGO_FILIAL)).FILIAL.Trim();

                    Literal litDataBaixa = e.Row.FindControl("litDataBaixa") as Literal;
                    litDataBaixa.Text = (cli.DATA_BAIXA == null) ? "-" : Convert.ToDateTime(cli.DATA_BAIXA).ToString("dd/MM/yyyy HH:mm");

                    ImageButton btWapp1 = e.Row.FindControl("btWapp1") as ImageButton;
                    if (cli.TELEFONE == "")
                        btWapp1.Visible = false;

                    ImageButton btWapp2 = e.Row.FindControl("btWapp2") as ImageButton;
                    if (cli.CELULAR == "")
                        btWapp2.Visible = false;

                    Button btBaixar = e.Row.FindControl("btBaixar") as Button;
                    btBaixar.CommandArgument = cli.CPF.Trim() + "|" + cli.CRM_ACAO.ToString();
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCliente_DataBound(object sender, EventArgs e)
        {
            var footer = gvCliente.FooterRow;
            if (footer != null)
            {
                //footer.Cells[2].Text = "Total";
            }
        }
        protected void gvCliente_Sorting(object sender, GridViewSortEventArgs e)
        {

            IEnumerable<CRM_CLIENTE_ACAO> clientes = ((IEnumerable<CRM_CLIENTE_ACAO>)ObterClienteAcao());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            clientes = clientes.OrderBy(e.SortExpression + sortDirection);
            gvCliente.DataSource = clientes.ToList();
            gvCliente.DataBind();

        }
        private string CriarLinkCliente(string cpf)
        {
            var link = "crm_cliente_det.aspx?t=1&ca=" + cpf;
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }

        protected void btWapp1_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            var numero = bt.CommandArgument;
            Response.Redirect("https://api.whatsapp.com/send?phone=" + numero + "&text=", "_blank", null);
        }
        protected void btWapp2_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            var numero = bt.CommandArgument;
            Response.Redirect("https://api.whatsapp.com/send?phone=" + numero + "&text=", "_blank", null);
        }

        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;

                var vals = bt.CommandArgument.Split('|');
                var cpf = vals[0].Trim();
                var codigoAcao = vals[1].Trim();

                var url = "fnAbrirTelaCadastroMaior('crm_cliente_acao_pop.aspx?cpf=" + cpf + "&ca=" + codigoAcao + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);

            }
            catch (Exception)
            {
            }
        }

    }
}
