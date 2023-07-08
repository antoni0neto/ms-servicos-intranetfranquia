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

namespace Relatorios
{
    public partial class admloj_con_cupomfiscal : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataVenda.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            if (!Page.IsPostBack)
            {
                CarregarFilial();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                DateTime? dataVenda = null;

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                if (txtDataVenda.Text.Trim() != "")
                    dataVenda = Convert.ToDateTime(txtDataVenda.Text.Trim());

                var cupomFiscal = baseController.ObterCupomFiscal(txtTicket.Text.Trim(), ddlFilial.SelectedValue.Trim(), txtProduto.Text.Trim(), dataVenda);

                gvCupomFiscal.DataSource = cupomFiscal;
                gvCupomFiscal.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvCupomFiscal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CUPOM_FISCALResult cupomFiscal = e.Row.DataItem as SP_OBTER_CUPOM_FISCALResult;

                    if (cupomFiscal != null)
                    {

                        Literal _litDataVenda = e.Row.FindControl("litDataVenda") as Literal;
                        if (_litDataVenda != null)
                            _litDataVenda.Text = cupomFiscal.DATA_VENDA.ToString("dd/MM/yyyy");

                        Literal _litValorPago = e.Row.FindControl("litValorPago") as Literal;
                        if (_litValorPago != null)
                            _litValorPago.Text = "R$ " + ((cupomFiscal.PRECO_LIQUIDO == null) ? "-" : Convert.ToDecimal(cupomFiscal.PRECO_LIQUIDO).ToString("###,###,###,##0.00"));

                        Literal _litCancelado = e.Row.FindControl("litCancelado") as Literal;
                        if (_litCancelado != null)
                            _litCancelado.Text = (Convert.ToBoolean(cupomFiscal.NOTA_CANCELADA)) ? "SIM" : "NÃO";
                    }
                }
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

        private bool ValidarCampos()
        {
            bool retorno = true;
            System.Drawing.Color _OK = Color.Gray;
            System.Drawing.Color _NOK = Color.Red;

            labTicket.ForeColor = _OK;
            if (txtTicket.Text.Trim() == "")
            {
                labTicket.ForeColor = _NOK;
                retorno = false;
            }

            labFilial.ForeColor = _OK;
            if (ddlFilial.SelectedValue == "")
            {
                labFilial.ForeColor = _NOK;
                retorno = false;
            }

            labDataVenda.ForeColor = _OK;
            if (txtDataVenda.Text.Trim() == "")
            {
                labDataVenda.ForeColor = _NOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion
    }
}
