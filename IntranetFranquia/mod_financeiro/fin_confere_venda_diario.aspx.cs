using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class fin_confere_venda_diario : System.Web.UI.Page
    {

        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

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
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        private List<SP_OBTER_TICKET_DIARIO_CLIENTEResult> ObterTicketDiarioPorCliente()
        {
            DateTime dataIni;
            DateTime dataFim;
            int qtdeTickets = 0;

            dataIni = Convert.ToDateTime(txtPeriodoInicial.Text.Trim());
            dataFim = Convert.ToDateTime(txtPeriodoFinal.Text.Trim());

            qtdeTickets = Convert.ToInt32(txtQtdeTickets.Text.Trim());

            return lojaController.ObterTicketDiarioPorCliente(dataIni, dataFim, ddlFilial.SelectedValue, txtCPF.Text.Trim(), qtdeTickets);
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                /*VALIDA DATA*/
                if (txtPeriodoInicial.Text == "")
                {
                    labErro.Text = "Informe a Data Inicial.";
                    return;
                }

                if (txtPeriodoFinal.Text == "")
                {
                    labErro.Text = "Informe a Data Final.";
                    return;
                }

                if (txtQtdeTickets.Text == "")
                {
                    labErro.Text = "Informe a Qtde de Tickets.";
                    return;
                }

                gvClientes.DataSource = ObterTicketDiarioPorCliente();
                gvClientes.DataBind();
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
                    SP_OBTER_TICKET_DIARIO_CLIENTEResult tkt = e.Row.DataItem as SP_OBTER_TICKET_DIARIO_CLIENTEResult;

                    Button btHistHandclub = e.Row.FindControl("btHistHandclub") as Button;
                    btHistHandclub.CommandArgument = tkt.CPF.Trim();
                }
            }
        }
        protected void gvClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_TICKET_DIARIO_CLIENTEResult> tkt = ObterTicketDiarioPorCliente();

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
        #endregion

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
