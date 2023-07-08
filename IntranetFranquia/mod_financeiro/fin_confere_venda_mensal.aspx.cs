using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class fin_confere_venda_mensal : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        const string S_VENDA_MES_CLIENTE = "S_VENDA_MES_CLIENTE";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "fin_prod_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_gestao/gest_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "../mod_gerenciamento_loja/gerloja_menu.aspx";

                hidTela.Value = tela;

                CarregarFilial();
                CarregarDataAno();

                Session[S_VENDA_MES_CLIENTE] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        private List<SP_OBTER_TICKET_MENSAL_CLIENTEResult> ObterTicketMensalPorCliente()
        {
            int qtdeTickets = 0;
            qtdeTickets = Convert.ToInt32(txtQtdeTickets.Text.Trim());

            var clientes = lojaController.ObterTicketMensalPorCliente(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), ddlFilial.SelectedValue, txtCPF.Text.Trim(), qtdeTickets);

            if (ddlFuncionario.SelectedValue != "")
                clientes = clientes.Where(p => p.FUNCIONARIO == ddlFuncionario.SelectedValue).ToList();

            return clientes;
        }

        private void CarregarVendas()
        {
            var tkt = ObterTicketMensalPorCliente();

            gvClientes.DataSource = tkt;
            gvClientes.DataBind();

            Session[S_VENDA_MES_CLIENTE] = tkt;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlMes.SelectedValue == "")
                {
                    labErro.Text = "Informe o Mês.";
                    return;
                }

                if (txtQtdeTickets.Text == "")
                {
                    labErro.Text = "Informe a Qtde de Tickets.";
                    return;
                }

                if (Convert.ToInt32(txtQtdeTickets.Text) < 3)
                {
                    labErro.Text = "Informe a Qtde de Tickets maior ou igual a 3.";
                    return;
                }

                CarregarVendas();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_TICKET_MENSAL_CLIENTEResult tkt = e.Row.DataItem as SP_OBTER_TICKET_MENSAL_CLIENTEResult;

                    CheckBox cb = e.Row.FindControl("cbAcompanhar") as CheckBox;
                    if (tkt.MARCADO != null && tkt.MARCADO != "")
                    {
                        e.Row.BackColor = Color.LightSalmon;
                        cb.Checked = true;
                    }
                    else
                    {
                        cb.Checked = false;
                        if (tkt.MAIS_2_COMPRAS_NODIA == "S")
                            e.Row.BackColor = Color.Wheat;
                        else
                            e.Row.BackColor = Color.White;
                    }

                    Button btAbrir = e.Row.FindControl("btAbrir") as Button;
                    btAbrir.CommandArgument = tkt.CPF.Trim();

                    Button btHistHandclub = e.Row.FindControl("btHistHandclub") as Button;
                    btHistHandclub.CommandArgument = tkt.CPF.Trim();

                }
            }
        }
        protected void gvClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[S_VENDA_MES_CLIENTE] != null)
            {
                IEnumerable<SP_OBTER_TICKET_MENSAL_CLIENTEResult> tkt = (IEnumerable<SP_OBTER_TICKET_MENSAL_CLIENTEResult>)Session[S_VENDA_MES_CLIENTE];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                tkt = tkt.OrderBy(e.SortExpression + sortDirection);
                gvClientes.DataSource = tkt;
                gvClientes.DataBind();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        #endregion

        protected void btAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;

                var ano = ddlAno.SelectedValue.Trim();
                var mes = ddlMes.SelectedValue.Trim();
                var cpf = b.CommandArgument;

                //Abrir pop-up
                var _url = "fnAbrirTelaCadastroMaior('fin_confere_venda_mensal_tickets.aspx?a=" + ano + "&m=" + mes + "&cpf=" + cpf + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
            }
            catch (Exception)
            {

                throw;
            }


        }

        protected void cbAcompanhar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;
                var cpf = gvClientes.DataKeys[row.RowIndex].Value.ToString();

                if (cb.Checked)
                {
                    var cli = new CLIENTE_VENDA_ACOMP();
                    cli.CPF = cpf;
                    cli.DATA_INCLUSAO = DateTime.Now;

                    lojaController.InserirClienteVendaAcompanhado(cli);
                }
                else
                {
                    lojaController.ExcluirClienteVendaAcompanhado(cpf);
                }

                CarregarVendas();

            }
            catch (Exception)
            {
            }
        }

        protected void btHistHandclub_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;

                if (b != null)
                {
                    string cpf = b.CommandArgument;
                    string tela = hidTela.Value;
                    Response.Redirect(("../mod_gestao/gest_historico_hand_cliente.aspx?t=" + tela + "&cpf=" + cpf), "_blank", "");
                }
            }
            catch (Exception ex)
            {
            }
        }




    }
}
