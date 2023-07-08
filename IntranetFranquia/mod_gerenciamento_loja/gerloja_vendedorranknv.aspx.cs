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

namespace Relatorios
{
    public partial class gerloja_vendedorranknv : System.Web.UI.Page
    {
        LojaController lojaController = new LojaController();

        string semana = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                CarregarPontos();

            }
        }

        private void CarregarPontos()
        {
            CarregarPontoAcumulado();
            CarregarPontoSemana();
        }

        #region "PONTO ACUMULADO"
        private void CarregarPontoAcumulado()
        {
            var rankAcum = lojaController.ObterVendedorRankingAcum2020();

            gvPontoAcumulado.DataSource = rankAcum;
            gvPontoAcumulado.DataBind();
            if (rankAcum == null || rankAcum.Count <= 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionA').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionA').accordion({ collapsible: true, heightStyle: 'content' });});", true);

        }
        protected void gvPontoAcumulado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDEDOR_RANKACUM2020Result rank = e.Row.DataItem as SP_OBTER_VENDEDOR_RANKACUM2020Result;

                    if (rank != null)
                    {
                        System.Web.UI.WebControls.Image imgCaricatura = e.Row.FindControl("imgCaricatura") as System.Web.UI.WebControls.Image;
                        imgCaricatura.ImageUrl = (rank.FOTO == null || rank.FOTO.Trim() == "") ? "~/Image/fun_sfoto.jpg" : rank.FOTO;

                        if (rank.POS > 20)
                            imgCaricatura.CssClass = "imgGray";
                    }
                }
            }
        }
        protected void gvPontoAcumulado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPontoAcumulado.FooterRow;
            if (footer != null)
            {
            }
        }
        protected void gvPontoAcumulado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPontoAcumulado.PageIndex = e.NewPageIndex;
            CarregarPontos();
        }

        #endregion

        #region "PONTO SEMANA"
        private void CarregarPontoSemana()
        {
            var rankSemana = lojaController.ObterVendedorRankingSemana2020();

            gvPontoSemana.DataSource = rankSemana;
            gvPontoSemana.DataBind();
            if (rankSemana == null || rankSemana.Count <= 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content' });});", true);

        }
        protected void gvPontoSemana_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDEDOR_RANKSEMANA2020Result rank = e.Row.DataItem as SP_OBTER_VENDEDOR_RANKSEMANA2020Result;
                    if (rank != null)
                    {
                        System.Web.UI.WebControls.Image imgCaricatura = e.Row.FindControl("imgCaricatura") as System.Web.UI.WebControls.Image;
                        imgCaricatura.ImageUrl = (rank.FOTO == null || rank.FOTO.Trim() == "") ? "~/Image/fun_sfoto.jpg" : rank.FOTO;

                        if (semana == "")
                        {
                            semana = "Pontos da Semana - " + rank.DATA_INI + " à " + rank.DATA_FIM;
                        }

                    }
                }
            }
        }
        protected void gvPontoSemana_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPontoSemana.FooterRow;
            if (footer != null)
            {
            }

            labPontoSemana.Text = semana;
        }
        #endregion

    }
}

