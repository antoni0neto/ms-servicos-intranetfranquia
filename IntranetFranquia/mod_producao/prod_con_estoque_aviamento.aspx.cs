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
    public partial class prod_con_estoque_aviamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarAviamentos();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarAviamentos()
        {
            List<PROD_AVIAMENTO> _aviamento = prodController.ObterAviamento();
            if (_aviamento != null)
            {
                _aviamento = _aviamento.Where(p => p.STATUS == 'A').ToList();
                _aviamento.Insert(0, new PROD_AVIAMENTO { CODIGO = 0, DESCRICAO = "" });

                ddlAviamento.DataSource = _aviamento;
                ddlAviamento.DataBind();
            }
        }
        #endregion

        #region "AVIAMENTOS"
        private List<SP_OBTER_AVIAMENTO_ESTOQUEResult> RetornarAviamentoEstoque()
        {
            List<SP_OBTER_AVIAMENTO_ESTOQUEResult> aviamentoEstoque = new List<SP_OBTER_AVIAMENTO_ESTOQUEResult>();
            aviamentoEstoque = prodController.ObterAviamentoEstoque(ddlAviamento.SelectedValue).ToList();
            return aviamentoEstoque;
        }
        private void CarregarAviamentosEstoque()
        {
            var lstAviamento = RetornarAviamentoEstoque();

            if (lstAviamento != null)
            {
                gvAviamento.DataSource = lstAviamento;
                gvAviamento.DataBind();
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                CarregarAviamentosEstoque();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_AVIAMENTO_ESTOQUEResult _aviamento = e.Row.DataItem as SP_OBTER_AVIAMENTO_ESTOQUEResult;

                    coluna += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                    }
                }
        }
        #endregion

    }
}
