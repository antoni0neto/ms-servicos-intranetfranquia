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
using System.Collections;
using Relatorios.mod_ecom.mag;


namespace Relatorios
{
    public partial class fin_altera_forma_pgto_funcionario : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy' });});", true);

            if (!Page.IsPostBack)
            {
                CarregarFiliais();
                CarregarFormaPgtoCliente();
                CarregarFormaPgtoValeMercadoria();

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btAtualizar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFiliais()
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
        private void CarregarFormaPgtoCliente()
        {
            var formaPgto = baseController.ObterFormaPgtoLojaCliente();

            formaPgto.Insert(0, new LOJA_FORMAS_PGTO { COD_FORMA_PGTO = "", FORMA_PGTO = "" });
            ddlFormaPgtoCliente.DataSource = formaPgto;
            ddlFormaPgtoCliente.DataBind();
        }
        private void CarregarFormaPgtoValeMercadoria()
        {
            var formaPgto = baseController.ObterFormaPgtoLojaValeMercadoria();

            formaPgto.Insert(0, new LOJA_FORMAS_PGTO { COD_FORMA_PGTO = "", FORMA_PGTO = "Selecione" });
            ddlFormaPgtoValeFuncionario.DataSource = formaPgto;
            ddlFormaPgtoValeFuncionario.DataBind();
        }

        private void CarregarJQuery()
        {
            //GRID TICKET
            if (gvTicket.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        private List<SP_OBTER_VENDA_VALE_FUNCIONARIO_ERRADOResult> ObterTickets()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;

            if (txtDataInicial.Text.Trim() != "")
                dataIni = Convert.ToDateTime(txtDataInicial.Text.Trim());

            if (txtDataFinal.Text.Trim() != "")
                dataFim = Convert.ToDateTime(txtDataFinal.Text.Trim());

            var tickets = contabilController.ObterTicketValeFuncionario(ddlFilial.SelectedValue, dataIni, dataFim, ddlFormaPgtoCliente.SelectedValue, txtTicket.Text.Trim());

            return tickets;
        }
        private void CarregarTickets()
        {
            gvTicket.DataSource = ObterTickets();
            gvTicket.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CarregarTickets();
                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID TICKET"
        protected void gvTicket_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_VALE_FUNCIONARIO_ERRADOResult ticket = e.Row.DataItem as SP_OBTER_VENDA_VALE_FUNCIONARIO_ERRADOResult;

                    Literal _litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                    if (_litDataVenda != null)
                        _litDataVenda.Text = ticket.DATA_VENDA.ToString("dd/MM/yyyy");

                    Literal _litValVendaBruta = e.Row.FindControl("litValVendaBruta") as Literal;
                    if (_litValVendaBruta != null)
                        _litValVendaBruta.Text = "R$ " + Convert.ToDecimal(ticket.VALOR_VENDA_BRUTA).ToString("###,###,##0.00");

                }
            }
        }

        #endregion

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroBaixa.Text = "";

                string codigoFilial = "";
                string ticket = "";
                DateTime dataVenda = DateTime.Now;

                string codFormaPgto = "";
                codFormaPgto = ddlFormaPgtoValeFuncionario.SelectedValue;

                if (codFormaPgto == "" || codFormaPgto == "0")
                {
                    labErroBaixa.Text = "Selecione a Forma de Pagamento para Vale Mercadoria.";
                    return;
                }

                foreach (GridViewRow row in gvTicket.Rows)
                {
                    codigoFilial = gvTicket.DataKeys[row.RowIndex].Values[0].ToString().Trim();
                    ticket = gvTicket.DataKeys[row.RowIndex].Values[1].ToString().Trim();
                    dataVenda = Convert.ToDateTime(gvTicket.DataKeys[row.RowIndex].Values[2].ToString());

                    //Atualizar Formas de Pagamentos
                    contabilController.AtualizarFormaPgtoTicket(codigoFilial, ticket, dataVenda, codFormaPgto);

                }

                CarregarTickets();
                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErroBaixa.Text = ex.Message;
            }
        }



    }
}

