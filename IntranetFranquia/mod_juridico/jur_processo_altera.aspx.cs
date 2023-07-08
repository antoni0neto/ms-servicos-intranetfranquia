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
    public partial class jur_processo_altera : System.Web.UI.Page
    {
        JuridicoController jurController = new JuridicoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarTipoProcesso();
            }
        }

        protected void btBuscarProcesso_Click(object sender, EventArgs e)
        {
            try
            {

                labProcesso.Text = "";
                if (!ValidarCampos())
                {
                    labProcesso.Text = "Informe pelo menos um Filtro";
                    return;
                }

                CarregarProcesso();
            }
            catch (Exception ex)
            {
                labProcesso.Text = "ERRO: " + ex.Message;
            }
        }

        #region "GRID"
        private void CarregarProcesso()
        {
            List<JUR_PROCESSO> processo = new List<JUR_PROCESSO>();

            processo = jurController.ObterProcesso();

            if (txtNumeroProcesso.Text.Trim() != "")
                processo = processo.Where(p => p.NUMERO.Trim().ToUpper().Contains(txtNumeroProcesso.Text.Trim().ToUpper())).ToList();

            if (txtRequerente.Text.Trim() != "")
                processo = processo.Where(p => p.REQUERENTE.Trim().ToUpper().Contains(txtRequerente.Text.Trim().ToUpper())).ToList();

            if (ddlTipoProcesso.SelectedValue != "0" && ddlTipoProcesso.SelectedValue != "")
                processo = processo.Where(p => p.JUR_TIPO_PROCESSO.ToString() == ddlTipoProcesso.SelectedValue).ToList();

            gvProcesso.DataSource = processo.OrderBy(p => p.REQUERENTE).OrderBy(x => x.CARGO);
            gvProcesso.DataBind();
        }
        protected void gvProcesso_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    JUR_PROCESSO _processo = e.Row.DataItem as JUR_PROCESSO;

                    coluna += 1;
                    if (_processo != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litTipoProcesso = e.Row.FindControl("litTipoProcesso") as Literal;
                        if (_litTipoProcesso != null)
                            _litTipoProcesso.Text = _processo.JUR_TIPO_PROCESSO1.DESCRICAO;

                        Button _btBuscar = e.Row.FindControl("btBuscar") as Button;
                        if (_btBuscar != null)
                            _btBuscar.CommandArgument = _processo.CODIGO.ToString();
                    }
                }
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                int codigoProcesso = 0;
                codigoProcesso = Convert.ToInt32(bt.CommandArgument);
                if (codigoProcesso > 0)
                    Response.Redirect("jur_processo_cad.aspx?p=" + codigoProcesso.ToString());
            }
        }
        #endregion


        #region "DADOS INICIAIS"
        private void CarregarTipoProcesso()
        {
            List<JUR_TIPO_PROCESSO> _processo = jurController.ObterTipoProcesso().Where(p => p.STATUS == 'A').ToList();

            _processo.Insert(0, new JUR_TIPO_PROCESSO { CODIGO = 0, DESCRICAO = "" });

            if (_processo != null)
            {
                ddlTipoProcesso.DataSource = _processo;
                ddlTipoProcesso.DataBind();
            }
        }
        private bool ValidarCampos()
        {
            bool retorno = true;

            if (txtNumeroProcesso.Text.Trim() == "" && txtRequerente.Text.Trim() == "" && ddlTipoProcesso.SelectedValue.Trim() == "0")
                retorno = false;

            return retorno;
        }
        #endregion
    }
}
