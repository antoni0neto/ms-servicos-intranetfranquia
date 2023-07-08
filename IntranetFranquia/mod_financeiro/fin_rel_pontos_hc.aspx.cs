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
    public partial class fin_rel_pontos_hc : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        const string REL_PONTO_HC_WAPP = "REL_PONTO_HC_WAPP";

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
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";

                hidTela.Value = tela;

                CarregarFilial();
                CarregarDataAno();

                Session[REL_PONTO_HC_WAPP] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        private List<SP_OBTER_WAPP_SALDOHCResult> ObterClientesSaldoHC()
        {
            decimal valorSaldo = Convert.ToInt32(txtSaldoHC.Text.Trim());

            var clientes = lojaController.ObterSaldoWAPP(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), ddlFilial.SelectedValue, txtCPF.Text.Trim(), valorSaldo);

            if (ddlMsgEnviada.SelectedValue != "")
                clientes = clientes.Where(p => p.MSG_ENVIADA == ddlMsgEnviada.SelectedValue).ToList();

            return clientes;
        }

        private void CarregarClientes()
        {
            var saldoHC = ObterClientesSaldoHC();

            gvClientes.DataSource = saldoHC;
            gvClientes.DataBind();

            Session[REL_PONTO_HC_WAPP] = saldoHC;
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

                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                if (txtSaldoHC.Text == "")
                {
                    labErro.Text = "Informe o valor mínimo para o Saldo Handclub.";
                    return;
                }

                if (Convert.ToInt32(txtSaldoHC.Text) < 1)
                {
                    labErro.Text = "Informe um valor maior que R$1,00.";
                    return;
                }

                CarregarClientes();
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
                    SP_OBTER_WAPP_SALDOHCResult saldoHC = e.Row.DataItem as SP_OBTER_WAPP_SALDOHCResult;

                    Literal litSaldo = e.Row.FindControl("litSaldo") as Literal;
                    litSaldo.Text = "R$ " + saldoHC.SALDO_PONTOS.ToString("###,###,##0.00");

                    Literal litDataEnvio = e.Row.FindControl("litDataEnvio") as Literal;
                    litDataEnvio.Text = (saldoHC.DATA_ENVIO == null) ? "-" : Convert.ToDateTime(saldoHC.DATA_ENVIO).ToString("dd/MM/yyyy HH:mm");

                    Button btHistHandclub = e.Row.FindControl("btHistHandclub") as Button;
                    btHistHandclub.CommandArgument = saldoHC.CPF.Trim();

                }
            }
        }
        protected void gvClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[REL_PONTO_HC_WAPP] != null)
            {
                IEnumerable<SP_OBTER_WAPP_SALDOHCResult> saldoHC = (IEnumerable<SP_OBTER_WAPP_SALDOHCResult>)Session[REL_PONTO_HC_WAPP];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                saldoHC = saldoHC.OrderBy(e.SortExpression + sortDirection);
                gvClientes.DataSource = saldoHC;
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
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
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
                        ddlAno.Enabled = false;

                        var mes = DateTime.Now.Month.ToString();
                        ddlMes.SelectedValue = mes;
                        ddlMes.Enabled = false;

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
