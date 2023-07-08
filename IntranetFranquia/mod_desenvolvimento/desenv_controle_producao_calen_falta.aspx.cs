using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;

namespace Relatorios
{
    public partial class desenv_controle_producao_calen_falta : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        int qtdeDiasUteis = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //p=" + produto.Trim() + "&c=" + cor + "&di=" + dataIni + "&df=" + dataFim + "";


                if (Request.QueryString["qtfal"] == null || Request.QueryString["qtfal"] == "" ||
                    Request.QueryString["diame"] == null || Request.QueryString["diame"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("desenv_menu.aspx");

                var qtdeFaltante = Request.QueryString["qtfal"].ToString();
                var mediaCorteDia = Request.QueryString["diame"].ToString();

                CarregarMeses(Convert.ToInt32(qtdeFaltante), Convert.ToInt32(mediaCorteDia));

            }

        }


        #region "DIAS"


        private void CarregarMeses(int qtdeFaltante, int mediaCorteDia)
        {
            var falt = desenvController.ObterCalendarioControleProducaoFaltante(qtdeFaltante, mediaCorteDia);

            gvProducaoFalta.DataSource = falt;
            gvProducaoFalta.DataBind();
        }
        protected void gvProducaoFalta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CALENDARIO_PRODUCAO_FALTAResult prod = e.Row.DataItem as SP_OBTER_CALENDARIO_PRODUCAO_FALTAResult;

                    if (prod != null)
                    {
                        qtdeDiasUteis += prod.UTIL;
                    }
                }
            }
        }
        protected void gvProducaoFalta_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProducaoFalta.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[3].Text = qtdeDiasUteis.ToString("###,###,##0");
            }
        }

        #endregion

    }
}

