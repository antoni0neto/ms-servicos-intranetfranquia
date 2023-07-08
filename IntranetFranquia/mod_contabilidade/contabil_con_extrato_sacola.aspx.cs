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
    public partial class contabil_con_extrato_sacola : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

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
                    hrefVoltar.HRef = "contabil_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_fiscal/fisc_menu.aspx";

                CarregarJquery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"

        #endregion

        private void CarregarJquery()
        {
            //GRID gvExtratoSacola
            if (gvExtratoSacola.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);


        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                DateTime? dataIni = Convert.ToDateTime("2018-01-01");
                DateTime? dataFim = DateTime.Now;

                if (ddlSacola.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione a Sacola";
                    return;
                }

                if (txtDataIni.Text.Trim() != "")
                    dataIni = Convert.ToDateTime(txtDataIni.Text.Trim());
                if (txtDataFim.Text.Trim() != "")
                    dataFim = Convert.ToDateTime(txtDataFim.Text.Trim());

                CarregarExtratoSacola(ddlSacola.SelectedValue.Trim(), ddlNatureza.SelectedValue, dataIni, dataFim);

                CarregarJquery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        private void CarregarExtratoSacola(string sacolaItem, string natureza, DateTime? dataIni, DateTime? dataFim)
        {
            var conAll = contabilController.ObterExtratoSacola(sacolaItem, "");

            if (natureza != "")
                conAll = conAll.Where(p => p.NATUREZA_COD == natureza).ToList();

            //Filtrar pela data
            var con = conAll.Where(p => p.DATA >= Convert.ToDateTime(dataIni) && p.DATA <= Convert.ToDateTime(dataFim)).ToList();

            //Obter Linha do Saldo 
            var linha = conAll.Where(p => p.DATA <= Convert.ToDateTime(dataIni).AddDays(-1));
            if (linha != null && linha.Count() > 0)
            {
                var saldofinal = linha.Where(p => p.COL == linha.Max(x => x.COL)).SingleOrDefault();
                if (saldofinal != null)
                {
                    saldofinal.COL = 0;
                    saldofinal.LANCAMENTO = 0;
                    saldofinal.NATUREZA = "SALDO FINAL";
                    saldofinal.NOTA_FISCAL = "";
                    saldofinal.QTDE_ENTRADA = Convert.ToDecimal(0.00);
                    saldofinal.QTDE_SAIDA = Convert.ToDecimal(0.00);
                    saldofinal.VALOR_ENTRADA = Convert.ToDecimal(0.00);
                    saldofinal.VALOR_SAIDA = Convert.ToDecimal(0.00);
                    con.Insert(0, saldofinal);
                }
            }

            gvExtratoSacola.DataSource = con.OrderBy(x => x.COL);
            gvExtratoSacola.DataBind();

        }
        protected void gvExtratoSacola_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_EXTRATO_SACOLAResult sacola = e.Row.DataItem as SP_OBTER_EXTRATO_SACOLAResult;

                    if (sacola != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(sacola.DATA).ToString("dd/MM/yyyy");

                        Literal _litQtdeEntrada = e.Row.FindControl("litQtdeEntrada") as Literal;
                        if (_litQtdeEntrada != null)
                            _litQtdeEntrada.Text = Convert.ToDecimal(sacola.QTDE_ENTRADA).ToString("###,###,###,##0");

                        Literal _litQtdeSaida = e.Row.FindControl("litQtdeSaida") as Literal;
                        if (_litQtdeSaida != null)
                            _litQtdeSaida.Text = Convert.ToDecimal(sacola.QTDE_SAIDA).ToString("###,###,###,##0");

                        Literal _litQtdeSaldo = e.Row.FindControl("litQtdeSaldo") as Literal;
                        if (_litQtdeSaldo != null)
                            _litQtdeSaldo.Text = Convert.ToDecimal(sacola.QTDE_SALDO).ToString("###,###,###,##0");

                        Literal _litValorEntrada = e.Row.FindControl("litValorEntrada") as Literal;
                        if (_litValorEntrada != null)
                            _litValorEntrada.Text = Convert.ToDecimal(sacola.VALOR_ENTRADA).ToString("###,###,###,##0.00");

                        Literal _litValorSaida = e.Row.FindControl("litValorSaida") as Literal;
                        if (_litValorSaida != null)
                            _litValorSaida.Text = Convert.ToDecimal(sacola.VALOR_SAIDA).ToString("###,###,###,##0.00");

                        Literal _litValorSaldo = e.Row.FindControl("litValorSaldo") as Literal;
                        if (_litValorSaldo != null)
                            _litValorSaldo.Text = Convert.ToDecimal(sacola.VALOR_SALDO).ToString("###,###,###,##0.00");


                    }
                }
            }
        }
        protected void gvExtratoSacola_DataBound(object sender, EventArgs e)
        {

        }



    }
}
