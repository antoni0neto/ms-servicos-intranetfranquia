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
    public partial class rh_rel_valor_venda : System.Web.UI.Page
    {
        RHController rhController = new RHController();

        decimal valorVendido = 0;
        decimal valorVendidoEcom = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_VALOR_VENDA_VENDEDORResult> ObterValorVendaVendedor(DateTime dataIni, DateTime dataFim, string codigoFilial, string cpf, string vendedor)
        {
            return rhController.ObterValorVendaVendedor(dataIni, dataFim, codigoFilial, cpf, vendedor);
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                DateTime dataIni;
                DateTime dataFim;

                if (txtDataIni.Text == "" || !DateTime.TryParse(txtDataIni.Text, out dataIni))
                {
                    labErro.Text = "Informe uma data válida para o Período Inicial.";
                    return;
                }

                if (txtDataFim.Text == "" || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                {
                    labErro.Text = "Informe uma data válida para o Período Fim.";
                    return;
                }

                if (!cbFilial.Checked && !cbVendedor.Checked)
                {
                    labErro.Text = "Selecione um filtro por Filial ou Vendedor.";
                    return;
                }

                if (cbFilial.Checked)
                {
                    if (ddlFilial.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecione uma filial.";
                        return;
                    }
                }

                if (cbVendedor.Checked)
                {
                    if (txtCPF.Text.Trim() == "" && txtVendedor.Text.Trim() == "")
                    {
                        labErro.Text = "Informe o CPF ou Código do Vendedor.";
                        return;
                    }
                }

                CarregarValoresVendedor(dataIni, dataFim, ddlFilial.SelectedValue.Trim(), txtCPF.Text.Trim(), txtVendedor.Text.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private void CarregarValoresVendedor(DateTime dataIni, DateTime dataFim, string codigoFilial, string cpf, string vendedor)
        {
            var vendedores = ObterValorVendaVendedor(dataIni, dataFim, codigoFilial, cpf, vendedor);

            gvVendedor.DataSource = vendedores;
            gvVendedor.DataBind();

            if (vendedores == null || vendedores.Count() <= 0)
            {
                labErro.Text = "Nenhum registro encontrado.";
            }
        }

        protected void gvVendedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VALOR_VENDA_VENDEDORResult venda = e.Row.DataItem as SP_OBTER_VALOR_VENDA_VENDEDORResult;

                    Literal litComissao = e.Row.FindControl("litComissao") as Literal;
                    litComissao.Text = "" + Convert.ToDecimal(venda.COMISSAO).ToString("0.00") + "%";

                    Literal litValorVendido = e.Row.FindControl("litValorVendido") as Literal;
                    litValorVendido.Text = "R$ " + Convert.ToDecimal(venda.VALOR_VENDA).ToString("###,###,###,##0.00");

                    Literal litValorComissao = e.Row.FindControl("litValorComissao") as Literal;
                    litValorComissao.Text = "R$ " + Convert.ToDecimal(venda.VALOR_COMISSAO).ToString("###,###,###,##0.00");


                    Literal litComissaoEcom = e.Row.FindControl("litComissaoEcom") as Literal;
                    litComissaoEcom.Text = "" + Convert.ToDecimal(venda.COMISSAO_ECOM).ToString("0.00") + "%";

                    Literal litValorVendidoEcom = e.Row.FindControl("litValorVendidoEcom") as Literal;
                    litValorVendidoEcom.Text = "R$ " + Convert.ToDecimal(venda.VALOR_VENDA_ECOM).ToString("###,###,###,##0.00");

                    Literal litValorComissaoEcom = e.Row.FindControl("litValorComissaoEcom") as Literal;
                    litValorComissaoEcom.Text = "R$ " + Convert.ToDecimal(venda.VALOR_COMISSAO_ECOM).ToString("###,###,###,##0.00");

                    Literal litValorVendaComissaoTot = e.Row.FindControl("litValorVendaComissaoTot") as Literal;
                    litValorVendaComissaoTot.Text = "R$ " + (Convert.ToDecimal(venda.VALOR_VENDA) + Convert.ToDecimal(venda.VALOR_VENDA_ECOM)).ToString("###,###,###,##0.00");

                    Literal litValorComissaoTot = e.Row.FindControl("litValorComissaoTot") as Literal;
                    litValorComissaoTot.Text = "R$ " + (Convert.ToDecimal(venda.VALOR_COMISSAO) + Convert.ToDecimal(venda.VALOR_COMISSAO_ECOM)).ToString("###,###,###,##0.00");

                    valorVendido += Convert.ToDecimal(venda.VALOR_VENDA);
                    valorVendidoEcom += Convert.ToDecimal(venda.VALOR_VENDA_ECOM);

                }
            }
        }
        protected void gvVendedor_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvVendedor.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[7].Text = "R$ " + valorVendido.ToString("###,###,###0.00");
                _footer.Cells[10].Text = "R$ " + valorVendidoEcom.ToString("###,###,###0.00");

                _footer.Cells[12].Text = "R$ " + (valorVendido + valorVendidoEcom).ToString("###,###,###0.00");
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = new BaseController().BuscaFiliais_Intermediario(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA" || p.TIPO_FILIAL.Trim() == "INATIVA").ToList();
                //lstFilial = baseController.BuscaFiliais();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                    }
                }
            }
        }
        #endregion

        private void HabilitarCampos(bool habilitaFilial)
        {
            ddlFilial.Enabled = habilitaFilial;
            ddlFilial.SelectedValue = "";
            txtCPF.Enabled = !habilitaFilial;
            txtVendedor.Enabled = !habilitaFilial;
            txtCPF.Text = "";
            txtVendedor.Text = "";
        }

        protected void cbFilial_CheckedChanged(object sender, EventArgs e)
        {
            labErro.Text = "";
            cbFilial.Checked = true;
            cbVendedor.Checked = true;
            if (cbFilial.Checked)
            {
                HabilitarCampos(true);
                cbVendedor.Checked = false;
            }
        }
        protected void cbVendedor_CheckedChanged(object sender, EventArgs e)
        {
            labErro.Text = "";
            cbFilial.Checked = true;
            cbVendedor.Checked = true;
            if (cbVendedor.Checked)
            {
                HabilitarCampos(false);
                cbFilial.Checked = false;
            }
        }

    }
}
