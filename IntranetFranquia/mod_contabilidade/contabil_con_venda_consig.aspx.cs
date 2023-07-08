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
    public partial class contabil_con_venda_consig : System.Web.UI.Page
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

                CarregarFornecedores();
                CarregarJquery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {
            var f = contabilController.ObterVendaConsigFornecedor();
            f.Insert(0, new SP_OBTER_VC_FORNECEDORResult { FORNECEDOR = "" });
            ddlFornecedor.DataSource = f;
            ddlFornecedor.DataBind();
        }
        #endregion

        private void CarregarJquery()
        {
            //GRID gvControleConsignado
            if (gvControleConsignado.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID gvPagamento
            if (gvPagamento.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                DateTime? dataIni = Convert.ToDateTime("2015-01-01");
                DateTime? dataFim = DateTime.Now;

                if (ddlFornecedor.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione o Fornecedor";
                    return;
                }

                if (txtDataIni.Text.Trim() != "")
                    dataIni = Convert.ToDateTime(txtDataIni.Text.Trim());
                if (txtDataFim.Text.Trim() != "")
                    dataFim = Convert.ToDateTime(txtDataFim.Text.Trim());

                CarregarControleConsignado(ddlFornecedor.SelectedValue.Trim(), ddlNatureza.SelectedValue, dataIni, dataFim);
                CarregarControlePagamento(ddlFornecedor.SelectedValue.Trim(), ddlNatureza.SelectedValue, dataIni, dataFim);

                CarregarJquery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        private void CarregarControleConsignado(string fornecedor, string natureza, DateTime? dataIni, DateTime? dataFim)
        {
            var conAll = contabilController.ObterVendaConsigControleRemessa(fornecedor);

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
                    saldofinal.ENTRADA = Convert.ToDecimal(0.00);
                    saldofinal.SAIDA = Convert.ToDecimal(0.00);
                    con.Insert(0, saldofinal);
                }
            }

            gvControleConsignado.DataSource = con.OrderBy(x => x.COL);
            gvControleConsignado.DataBind();


        }
        protected void gvControleConsignado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VC_CTRL_REMESSAResult remessa = e.Row.DataItem as SP_OBTER_VC_CTRL_REMESSAResult;

                    if (remessa != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(remessa.DATA).ToString("dd/MM/yyyy");

                        Literal _litEntrada = e.Row.FindControl("litEntrada") as Literal;
                        if (_litEntrada != null)
                            _litEntrada.Text = Convert.ToDecimal(remessa.ENTRADA).ToString("###,###,###,##0.00");

                        Literal _litSaida = e.Row.FindControl("litSaida") as Literal;
                        if (_litSaida != null)
                            _litSaida.Text = Convert.ToDecimal(remessa.SAIDA).ToString("###,###,###,##0.00");

                        Literal _litSaldo = e.Row.FindControl("litSaldo") as Literal;
                        if (_litSaldo != null)
                            _litSaldo.Text = Convert.ToDecimal(remessa.SALDO).ToString("###,###,###,##0.00");
                    }
                }
            }
        }
        protected void gvControleConsignado_DataBound(object sender, EventArgs e)
        {

        }

        private void CarregarControlePagamento(string fornecedor, string natureza, DateTime? dataIni, DateTime? dataFim)
        {
            var pagAll = contabilController.ObterVendaConsigControlePGTO(fornecedor);

            if (natureza != "")
                pagAll = pagAll.Where(p => p.NATUREZA_COD == natureza).ToList();

            //Filtrar pela data
            var pag = pagAll.Where(p => p.DATA >= Convert.ToDateTime(dataIni) && p.DATA <= Convert.ToDateTime(dataFim)).ToList();

            //Obter Linha do Saldo 
            var linha = pagAll.Where(p => p.DATA <= Convert.ToDateTime(dataIni).AddDays(-1));
            if (linha != null && linha.Count() > 0)
            {
                var saldofinal = linha.Where(p => p.COL == linha.Max(x => x.COL)).SingleOrDefault();
                if (saldofinal != null)
                {
                    saldofinal.COL = 0;
                    saldofinal.LANCAMENTO = 0;
                    saldofinal.NATUREZA = "SALDO FINAL";
                    saldofinal.NOTA_FISCAL = "-";
                    saldofinal.ENTRADA = Convert.ToDecimal(0.00);
                    saldofinal.SAIDA = Convert.ToDecimal(0.00);
                    pag.Insert(0, saldofinal);
                }
            }

            gvPagamento.DataSource = pag.OrderBy(x => x.COL);
            gvPagamento.DataBind();
        }
        protected void gvPagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VC_CTRL_PGTOResult pgto = e.Row.DataItem as SP_OBTER_VC_CTRL_PGTOResult;

                    if (pgto != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(pgto.DATA).ToString("dd/MM/yyyy");

                        Literal _litEntrada = e.Row.FindControl("litEntrada") as Literal;
                        if (_litEntrada != null)
                            _litEntrada.Text = Convert.ToDecimal(pgto.ENTRADA).ToString("###,###,###,##0.00");

                        Literal _litSaida = e.Row.FindControl("litSaida") as Literal;
                        if (_litSaida != null)
                            _litSaida.Text = Convert.ToDecimal(pgto.SAIDA).ToString("###,###,###,##0.00");

                        Literal _litSaldo = e.Row.FindControl("litSaldo") as Literal;
                        if (_litSaldo != null)
                            _litSaldo.Text = Convert.ToDecimal(pgto.SALDO).ToString("###,###,###,##0.00");
                    }
                }
            }
        }
        protected void gvPagamento_DataBound(object sender, EventArgs e)
        {

        }




    }
}
