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
    public partial class gest_compara_intra_linx : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtData.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btFechar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btFechar, null) + ";");
        }

        private List<SP_OBTER_VALOR_LINX_INTRANETResult> ObterValorLinxIntranet(DateTime data)
        {
            return lojaController.ObterValorLinxIntranet(data);
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";

                if (txtData.Text.Trim() == "")
                {
                    labErro.Text = "Por favor, informe uma data válida.";
                    return;
                }

                var dataMovimento = Convert.ToDateTime(txtData.Text.Trim());

                gvIntranetLinx.DataSource = ObterValorLinxIntranet(dataMovimento);
                gvIntranetLinx.DataBind();

                txtData.BackColor = Color.White;
                var movimento = lojaController.ObterLinxMovimento(dataMovimento);
                if (movimento != null)
                {
                    txtData.BackColor = Color.GreenYellow;
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvIntranetLinx_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VALOR_LINX_INTRANETResult valLinxIntra = e.Row.DataItem as SP_OBTER_VALOR_LINX_INTRANETResult;

                    if (valLinxIntra != null)
                    {

                        Literal litValorLinx = e.Row.FindControl("litValorLinx") as Literal;
                        litValorLinx.Text = (valLinxIntra.VALOR == null) ? "" : "R$ " + Convert.ToDecimal(valLinxIntra.VALOR).ToString("###,###,###,##0.00");

                        Literal litValorIntranet = e.Row.FindControl("litValorIntranet") as Literal;
                        litValorIntranet.Text = (valLinxIntra.VALOR_INTRANET == null) ? "" : "R$ " + Convert.ToDecimal(valLinxIntra.VALOR_INTRANET).ToString("###,###,###,##0.00");

                        Literal litValorDiff = e.Row.FindControl("litValorDiff") as Literal;
                        litValorDiff.Text = (valLinxIntra.DIFERENCA == null) ? "" : "R$ " + Convert.ToDecimal(valLinxIntra.DIFERENCA).ToString("###,###,###,##0.00");

                        Literal litUltimaVenda = e.Row.FindControl("litUltimaVenda") as Literal;
                        litUltimaVenda.Text = (valLinxIntra.ULTIMA_VENDA == null) ? "" : Convert.ToDateTime(valLinxIntra.ULTIMA_VENDA).ToString("dd/MM/yyyy HH:mm");

                        if (valLinxIntra.STATUS_FILIAL.ToLower().Contains("erro"))
                        {
                            e.Row.BackColor = Color.IndianRed;
                            e.Row.ForeColor = Color.White;
                        }
                        else
                        {
                            e.Row.BackColor = Color.White;
                            e.Row.ForeColor = Color.Black;
                        }

                    }
                }
            }
        }

        protected void btFechar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (txtData.Text.Trim() == "")
                {
                    labErro.Text = "Informe a data do movimento.";
                    return;
                }

                var dataMovimento = Convert.ToDateTime(txtData.Text);

                var movimento = lojaController.ObterLinxMovimento(dataMovimento);
                if (movimento != null)
                {
                    labErro.Text = "Movimento já foi fechado.";
                    return;
                }

                var linxMovimento = new LINX_MOVIMENTO() { DATA_MOVIMENTO = dataMovimento };
                lojaController.InserirLinxMovimento(linxMovimento);

                labErro.Text = "Movimento fechado com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


        }

        #region "DADOS INICIAIS"

        #endregion


    }
}
