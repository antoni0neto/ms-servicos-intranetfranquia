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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class rh_rel_folha : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        RHController rhController = new RHController();

        decimal gMinino = 0;

        decimal gComVendaTot = 0;
        decimal gComVendaLoja = 0;
        decimal gComVendaEcom = 0;

        decimal gComCota = 0;

        decimal gPremioCota = 0;
        decimal gPremioVendedor = 0;
        decimal gPremioPonta = 0;

        string callGridViewScroll = "$(function () { gridviewScroll(); });";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();

                if (tela == "1")
                    hrefVoltar.HRef = "rh_menu.aspx";

                CarregarCompetencia();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_CALCULO_COMISSAOResult> ObterCalculoComissao()
        {
            var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());
            var cpf = txtCPF.Text.Trim().Replace(".", "").Replace("-", "");

            return rhController.ObterCalculoComissao(competencia, ddlFilial.SelectedValue.Trim(), cpf, txtVendedor.Text.Trim());
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlCompetencia.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a competência.";
                    return;
                }

                gvFolha.DataSource = ObterCalculoComissao();
                gvFolha.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvFolha_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CALCULO_COMISSAOResult folha = e.Row.DataItem as SP_OBTER_CALCULO_COMISSAOResult;

                    Literal litMinino = e.Row.FindControl("litMinino") as Literal;
                    litMinino.Text = "R$ " + Convert.ToDecimal(folha.MINIMO_GARANTIDO).ToString("###,###,##0.00");

                    Literal litComissaoPorc = e.Row.FindControl("litComissaoPorc") as Literal;
                    litComissaoPorc.Text = Convert.ToDecimal(folha.COMISSAO_PORC).ToString("###,###,##0.00") + "%";

                    Literal litVALOR_VENDA_TOTAL_2120 = e.Row.FindControl("litVALOR_VENDA_TOTAL_2120") as Literal;
                    litVALOR_VENDA_TOTAL_2120.Text = "R$ " + Convert.ToDecimal(folha.VALOR_VENDA_TOTAL_2120).ToString("###,###,##0.00");
                    Literal litCOMISSAO_VENDA_TOTAL_2120 = e.Row.FindControl("litCOMISSAO_VENDA_TOTAL_2120") as Literal;
                    litCOMISSAO_VENDA_TOTAL_2120.Text = "R$ " + Convert.ToDecimal(folha.COMISSAO_VENDA_TOTAL_2120).ToString("###,###,##0.00");

                    Literal litVALOR_VENDIDOCOM_2120 = e.Row.FindControl("litVALOR_VENDIDOCOM_2120") as Literal;
                    litVALOR_VENDIDOCOM_2120.Text = "R$ " + Convert.ToDecimal(folha.VALOR_VENDIDOCOM_2120).ToString("###,###,##0.00");
                    Literal litCOMISSAO_VENDA_2120 = e.Row.FindControl("litCOMISSAO_VENDA_2120") as Literal;
                    litCOMISSAO_VENDA_2120.Text = "R$ " + Convert.ToDecimal(folha.COMISSAO_VENDA_2120).ToString("###,###,##0.00");

                    Literal litVALOR_VENDIDO_ECOM_2120 = e.Row.FindControl("litVALOR_VENDIDO_ECOM_2120") as Literal;
                    litVALOR_VENDIDO_ECOM_2120.Text = "R$ " + Convert.ToDecimal(folha.VALOR_VENDIDO_ECOM_2120).ToString("###,###,##0.00");
                    Literal litCOMISSAO_ECOM_2120 = e.Row.FindControl("litCOMISSAO_ECOM_2120") as Literal;
                    litCOMISSAO_ECOM_2120.Text = "R$ " + Convert.ToDecimal(folha.COMISSAO_ECOM_2120).ToString("###,###,##0.00");

                    Literal litVALOR_VENDIDOCOTA_0130 = e.Row.FindControl("litVALOR_VENDIDOCOTA_0130") as Literal;
                    litVALOR_VENDIDOCOTA_0130.Text = "R$ " + Convert.ToDecimal(folha.VALOR_VENDIDOCOTA_0130).ToString("###,###,##0.00");

                    Literal litCOMISSAO_COTA_0130 = e.Row.FindControl("litCOMISSAO_COTA_0130") as Literal;
                    litCOMISSAO_COTA_0130.Text = "R$ " + Convert.ToDecimal(folha.COMISSAO_COTA_0130).ToString("###,###,##0.00");

                    Literal litPREMIO_COTA = e.Row.FindControl("litPREMIO_COTA") as Literal;
                    litPREMIO_COTA.Text = "R$ " + Convert.ToDecimal(folha.PREMIO_COTA).ToString("###,###,##0.00");

                    Literal litPREMIO_VENDEDOR = e.Row.FindControl("litPREMIO_VENDEDOR") as Literal;
                    litPREMIO_VENDEDOR.Text = "R$ " + Convert.ToDecimal(folha.PREMIO_VENDEDOR).ToString("###,###,##0.00");

                    Literal litPREMIO_PONTA = e.Row.FindControl("litPREMIO_PONTA") as Literal;
                    litPREMIO_PONTA.Text = "R$ " + Convert.ToDecimal(folha.PREMIO_PONTA).ToString("###,###,##0.00");

                    Literal litCOTA_SEM1 = e.Row.FindControl("litCOTA_SEM1") as Literal;
                    litCOTA_SEM1.Text = Convert.ToDecimal(folha.COTA_SEM1).ToString("###,###,##0.00") + "%";

                    Literal litCOTA_SEM2 = e.Row.FindControl("litCOTA_SEM2") as Literal;
                    litCOTA_SEM2.Text = Convert.ToDecimal(folha.COTA_SEM2).ToString("###,###,##0.00") + "%";

                    Literal litCOTA_SEM3 = e.Row.FindControl("litCOTA_SEM3") as Literal;
                    litCOTA_SEM3.Text = Convert.ToDecimal(folha.COTA_SEM3).ToString("###,###,##0.00") + "%";

                    Literal litCOTA_SEM4 = e.Row.FindControl("litCOTA_SEM4") as Literal;
                    litCOTA_SEM4.Text = Convert.ToDecimal(folha.COTA_SEM4).ToString("###,###,##0.00") + "%";

                    Literal litCOTA_TOTAL = e.Row.FindControl("litCOTA_TOTAL") as Literal;
                    litCOTA_TOTAL.Text = Convert.ToDecimal(folha.COTA_TOTAL).ToString("###,###,##0.00") + "%";

                    gMinino += Convert.ToDecimal(folha.MINIMO_GARANTIDO);
                    gComVendaTot += Convert.ToDecimal(folha.COMISSAO_VENDA_TOTAL_2120);
                    gComVendaLoja += Convert.ToDecimal(folha.COMISSAO_VENDA_2120);
                    gComVendaEcom += Convert.ToDecimal(folha.COMISSAO_ECOM_2120);

                    gComCota += Convert.ToDecimal(folha.COMISSAO_COTA_0130);

                    gPremioCota += Convert.ToDecimal(folha.PREMIO_COTA);
                    gPremioVendedor += Convert.ToDecimal(folha.PREMIO_VENDEDOR);
                    gPremioPonta += Convert.ToDecimal(folha.PREMIO_PONTA);
                }
            }
        }
        protected void gvFolha_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvFolha.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[4].Text = "R$" + gMinino.ToString("###,###,##0.00");


                footer.Cells[7].Text = "R$" + gComVendaTot.ToString("###,###,##0.00");
                footer.Cells[9].Text = "R$" + gComVendaLoja.ToString("###,###,##0.00");
                footer.Cells[11].Text = "R$" + gComVendaEcom.ToString("###,###,##0.00");

                footer.Cells[13].Text = "R$" + gComCota.ToString("###,###,##0.00");

                footer.Cells[14].Text = "R$" + gPremioCota.ToString("###,###,##0.00");
                footer.Cells[15].Text = "R$" + gPremioVendedor.ToString("###,###,##0.00");
                footer.Cells[16].Text = "R$" + gPremioPonta.ToString("###,###,##0.00");
            }
        }
        protected void gvFolha_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_CALCULO_COMISSAOResult> folha = ObterCalculoComissao();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            folha = folha.OrderBy(e.SortExpression + sortDirection);
            gvFolha.DataSource = folha;
            gvFolha.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial(DateTime competencia)
        {
            var filiais = rhController.ObterCalculoComissao(competencia, "", "", "");
            var lstFilial = new List<FILIAI>();
            foreach (var f in filiais)
            {
                if (lstFilial.Where(p => p.COD_FILIAL.Trim().Contains(f.CODIGO_FILIAL)).Count() <= 0)
                    lstFilial.Add(new FILIAI { COD_FILIAL = f.CODIGO_FILIAL, FILIAL = f.FILIAL });
            }

            //
            lstFilial = lstFilial.OrderBy(p => p.FILIAL).ToList();
            lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
            ddlFilial.DataSource = lstFilial;
            ddlFilial.DataBind();
        }
        private void CarregarCompetencia()
        {
            var comp = baseController.BuscaReferencias().Where(p => p.ANO >= 2018).OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).ToList();

            comp.Insert(0, new ACOMPANHAMENTO_ALUGUEL_MESANO { CODIGO_ACOMPANHAMENTO_MESANO = 0, DESCRICAO = "Selecione" });
            ddlCompetencia.DataSource = comp;
            ddlCompetencia.DataBind();
        }
        #endregion

        protected void ddlCompetencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());
            CarregarFilial(competencia);

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);
        }

    }
}
