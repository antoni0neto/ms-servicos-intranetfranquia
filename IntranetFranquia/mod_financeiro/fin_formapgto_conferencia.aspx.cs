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
    public partial class fin_formapgto_conferencia : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarFilial();
            }
        }

        private void CarregarConferenciaFormaPgto()
        {

            gvFormaPagamento.DataSource = null;
            gvFormaPagamento.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini = DateTime.Now;
            DateTime v_datafim = DateTime.Now;

            if (!DateTime.TryParse(txtDataInicio.Text, out v_dataini))
            {
                labErro.Text = "INFORME UMA DATA INICIAL VÁLIDA.";
                return;
            }
            if (!DateTime.TryParse(txtDataFim.Text, out v_datafim))
            {
                labErro.Text = "INFORME UMA DATA FINAL VÁLIDA.";
                return;
            }

            try
            {
                CarregarConferenciaFormaPgto();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvFormaPagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO _pedido = e.Row.DataItem as DESENV_PEDIDO;

                    coluna += 1;
                    if (_pedido != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                    }
                }
            }
        }

        #region "DADOS INICIAIS"
        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                //lstFilial = baseController.BuscaFiliais(usuario);
                lstFilial = baseController.BuscaFiliais();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    //lstFilial.Add(new FILIAI { COD_FILIAL = "999999", FILIAL = "GERAL" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();
                }
            }
        }
        #endregion
    }
}
