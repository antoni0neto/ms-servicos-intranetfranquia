using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_pedido_cliente_crm : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();


        const string PEDIDO_MAG_CLIENTECRM = "PEDIDO_MAG_CLIENTECRM";
        string callGridViewScroll = "$(function () { gridviewScroll(); });";

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
                    hrefVoltar.HRef = "ecom_menu.aspx";

                CarregarDataAno();

                Session[PEDIDO_MAG_CLIENTECRM] = null;

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_ECOM_CLIENTE_CRMResult> ObterEcomClienteCRM(int ano, int mesDe, int mesAte, string cliente, string cpf, string email)
        {
            var pMag = ecomController.ObterEcomClienteCRM(ano, mesDe, mesAte, cliente, cpf, email);

            return pMag;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";

                CarregarClientes(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMesDe.SelectedValue), Convert.ToInt32(ddlMesAte.SelectedValue), txtNome.Text, txtCPF.Text, txtEmail.Text);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void CarregarClientes(int ano, int mesDe, int mesAte, string cliente, string cpf, string email)
        {
            var cliCRM = ecomController.ObterEcomClienteCRM(ano, mesDe, mesAte, cliente, cpf, email);

            if (txtQtdeMeses.Text != "")
                cliCRM = cliCRM.Where(p => p.QTDE_MESES >= Convert.ToInt32(txtQtdeMeses.Text.Trim())).ToList();

            if (txtQtdePedidos.Text != "")
                cliCRM = cliCRM.Where(p => p.TOT_QTDE >= Convert.ToInt32(txtQtdePedidos.Text.Trim())).ToList();

            if (ddlAcao.SelectedValue != "")
            {
                //if (ddlAcao.SelectedValue == "S")
                //    cliCRM = cliCRM.Where(p => p. >= Convert.ToInt32(txtQtdePedidos.Text.Trim())).ToList();
            }


            gvCliente.DataSource = cliCRM;
            gvCliente.DataBind();

            Session[PEDIDO_MAG_CLIENTECRM] = cliCRM;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
        }

        protected void gvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_CLIENTE_CRMResult cliCRM = e.Row.DataItem as SP_OBTER_ECOM_CLIENTE_CRMResult;

                    Literal litAbrirCliente = e.Row.FindControl("litAbrirCliente") as Literal;
                    litAbrirCliente.Text = CriarLinkClienteAtacado(cliCRM.CLIENTE_ATACADO);

                    Literal litDataUltimoAcao = e.Row.FindControl("litDataUltimoAcao") as Literal;
                    litDataUltimoAcao.Text = "26/10/1987";// pedido.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm:ss");

                    Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                    litValTotal.Text = cliCRM.TOT_VAL.ToString("###,###,###,##0.00");

                    Literal litValJAN = e.Row.FindControl("litValJAN") as Literal;
                    litValJAN.Text = cliCRM.JAN_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.JAN_VAL > 0)
                    {
                        e.Row.Cells[7].BackColor = Color.PaleGreen;
                        e.Row.Cells[8].BackColor = Color.PaleGreen;
                    }

                    Literal litValFEV = e.Row.FindControl("litValFEV") as Literal;
                    litValFEV.Text = cliCRM.FEV_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.FEV_VAL > 0)
                    {
                        e.Row.Cells[9].BackColor = Color.PaleGreen;
                        e.Row.Cells[10].BackColor = Color.PaleGreen;
                    }

                    Literal litValMAR = e.Row.FindControl("litValMAR") as Literal;
                    litValMAR.Text = cliCRM.MAR_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.MAR_VAL > 0)
                    {
                        e.Row.Cells[11].BackColor = Color.PaleGreen;
                        e.Row.Cells[12].BackColor = Color.PaleGreen;
                    }

                    Literal litValABR = e.Row.FindControl("litValABR") as Literal;
                    litValABR.Text = cliCRM.ABR_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.ABR_VAL > 0)
                    {
                        e.Row.Cells[13].BackColor = Color.PaleGreen;
                        e.Row.Cells[14].BackColor = Color.PaleGreen;
                    }

                    Literal litValMAI = e.Row.FindControl("litValMAI") as Literal;
                    litValMAI.Text = cliCRM.MAI_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.MAI_VAL > 0)
                    {
                        e.Row.Cells[15].BackColor = Color.PaleGreen;
                        e.Row.Cells[16].BackColor = Color.PaleGreen;
                    }

                    Literal litValJUN = e.Row.FindControl("litValJUN") as Literal;
                    litValJUN.Text = cliCRM.JUN_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.JUN_VAL > 0)
                    {
                        e.Row.Cells[17].BackColor = Color.PaleGreen;
                        e.Row.Cells[18].BackColor = Color.PaleGreen;
                    }

                    Literal litValJUL = e.Row.FindControl("litValJUL") as Literal;
                    litValJUL.Text = cliCRM.JUL_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.JUL_VAL > 0)
                    {
                        e.Row.Cells[19].BackColor = Color.PaleGreen;
                        e.Row.Cells[20].BackColor = Color.PaleGreen;
                    }

                    Literal litValAGO = e.Row.FindControl("litValAGO") as Literal;
                    litValAGO.Text = cliCRM.AGO_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.AGO_VAL > 0)
                    {
                        e.Row.Cells[21].BackColor = Color.PaleGreen;
                        e.Row.Cells[22].BackColor = Color.PaleGreen;
                    }

                    Literal litValSET = e.Row.FindControl("litValSET") as Literal;
                    litValSET.Text = cliCRM.SET_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.SET_VAL > 0)
                    {
                        e.Row.Cells[23].BackColor = Color.PaleGreen;
                        e.Row.Cells[24].BackColor = Color.PaleGreen;
                    }

                    Literal litValOUT = e.Row.FindControl("litValOUT") as Literal;
                    litValOUT.Text = cliCRM.OUT_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.OUT_VAL > 0)
                    {
                        e.Row.Cells[25].BackColor = Color.PaleGreen;
                        e.Row.Cells[26].BackColor = Color.PaleGreen;
                    }

                    Literal litValNOV = e.Row.FindControl("litValNOV") as Literal;
                    litValNOV.Text = cliCRM.NOV_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.NOV_VAL > 0)
                    {
                        e.Row.Cells[27].BackColor = Color.PaleGreen;
                        e.Row.Cells[28].BackColor = Color.PaleGreen;
                    }

                    Literal litValDEZ = e.Row.FindControl("litValDEZ") as Literal;
                    litValDEZ.Text = cliCRM.DEZ_VAL.ToString("###,###,###,##0.00");
                    if (cliCRM.DEZ_VAL > 0)
                    {
                        e.Row.Cells[29].BackColor = Color.PaleGreen;
                        e.Row.Cells[30].BackColor = Color.PaleGreen;
                    }

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
            GridViewRow _footer = gvCliente.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[2].Text = "Total";
                _footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;


            }
        }
        protected void gvCliente_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PEDIDO_MAG_CLIENTECRM] != null)
            {
                IEnumerable<SP_OBTER_ECOM_CLIENTE_CRMResult> pedidos = ((IEnumerable<SP_OBTER_ECOM_CLIENTE_CRMResult>)Session[PEDIDO_MAG_CLIENTECRM]);

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                pedidos = pedidos.OrderBy(e.SortExpression + sortDirection);
                gvCliente.DataSource = pedidos;
                gvCliente.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno().Where(p => p.STATUS == 'A').ToList();
            if (dataAno != null)
            {
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

        private string CriarLinkClienteAtacado(string clienteAtacado)
        {
            var link = "ecom_pedido_cliente_crm_det.aspx?t=1&ca=" + clienteAtacado.Trim();
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }

    }
}
