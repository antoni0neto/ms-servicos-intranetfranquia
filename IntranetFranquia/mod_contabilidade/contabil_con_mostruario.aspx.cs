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
    public partial class contabil_con_mostruario : System.Web.UI.Page
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

                CarregarRepresentantes();
                CarregarJquery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarRepresentantes()
        {
            var f = contabilController.ObterMostruarioRepresentante();
            f.Insert(0, new SP_OBTER_MOSTR_REPREResult { REPRESENTANTE = "" });
            ddlRepresentante.DataSource = f;
            ddlRepresentante.DataBind();
        }
        #endregion

        private void CarregarJquery()
        {
            //GRID gvControleRemessa
            if (gvControleRemessa.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID gvBonificacao
            if (gvBonificacao.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionB').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionB').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID gvRecebimento
            if (gvRecebimento.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                DateTime? dataIni = Convert.ToDateTime("1900-01-01");
                DateTime? dataFim = DateTime.Now.AddDays(800);

                if (ddlRepresentante.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione o Representante";
                    return;
                }

                if (txtDataIni.Text.Trim() != "")
                    dataIni = Convert.ToDateTime(txtDataIni.Text.Trim());
                if (txtDataFim.Text.Trim() != "")
                    dataFim = Convert.ToDateTime(txtDataFim.Text.Trim());

                CarregarControleRemessa(ddlRepresentante.SelectedValue.Trim(), ddlNatureza.SelectedValue, dataIni, dataFim);
                CarregarBonificacao(ddlRepresentante.SelectedValue.Trim(), ddlNatureza.SelectedValue, dataIni, dataFim);
                CarregarRecebimento(ddlRepresentante.SelectedValue.Trim(), ddlNatureza.SelectedValue, dataIni, dataFim);

                CarregarJquery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        private void CarregarControleRemessa(string representante, string natureza, DateTime? dataIni, DateTime? dataFim)
        {
            var remessaAll = contabilController.ObterMostruarioRemessa(representante);

            if (natureza != "")
                remessaAll = remessaAll.Where(p => p.NATUREZA_COD == natureza).ToList();

            //Filtrar pela data
            var con = remessaAll.Where(p => p.DATA >= Convert.ToDateTime(dataIni) && p.DATA <= Convert.ToDateTime(dataFim)).ToList();

            //Obter Linha do Saldo 
            var linha = remessaAll.Where(p => p.DATA <= Convert.ToDateTime(dataIni).AddDays(-1));
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

            gvControleRemessa.DataSource = con.OrderBy(x => x.COL);
            gvControleRemessa.DataBind();


        }
        protected void gvControleRemessa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MOSTR_REPRE_REMESSAResult remessa = e.Row.DataItem as SP_OBTER_MOSTR_REPRE_REMESSAResult;

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
        protected void gvControleRemessa_DataBound(object sender, EventArgs e)
        {

        }

        private void CarregarBonificacao(string representante, string natureza, DateTime? dataIni, DateTime? dataFim)
        {
            var bonificacaoAll = contabilController.ObterMostruarioBonificacao(representante);

            if (natureza != "")
                bonificacaoAll = bonificacaoAll.Where(p => p.NATUREZA_COD == natureza).ToList();

            //Filtrar pela data
            var bon = bonificacaoAll.Where(p => p.DATA >= Convert.ToDateTime(dataIni) && p.DATA <= Convert.ToDateTime(dataFim)).ToList();

            //Obter Linha do Saldo 
            var linha = bonificacaoAll.Where(p => p.DATA <= Convert.ToDateTime(dataIni).AddDays(-1));
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
                    bon.Insert(0, saldofinal);
                }
            }

            gvBonificacao.DataSource = bon.OrderBy(x => x.COL);
            gvBonificacao.DataBind();
        }
        protected void gvBonificacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MOSTR_REPRE_BONIFICACAOResult bon = e.Row.DataItem as SP_OBTER_MOSTR_REPRE_BONIFICACAOResult;

                    if (bon != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(bon.DATA).ToString("dd/MM/yyyy");

                        Literal _litEntrada = e.Row.FindControl("litEntrada") as Literal;
                        if (_litEntrada != null)
                            _litEntrada.Text = Convert.ToDecimal(bon.ENTRADA).ToString("###,###,###,##0.00");

                        Literal _litSaida = e.Row.FindControl("litSaida") as Literal;
                        if (_litSaida != null)
                            _litSaida.Text = Convert.ToDecimal(bon.SAIDA).ToString("###,###,###,##0.00");

                        Literal _litSaldo = e.Row.FindControl("litSaldo") as Literal;
                        if (_litSaldo != null)
                            _litSaldo.Text = Convert.ToDecimal(bon.SALDO).ToString("###,###,###,##0.00");
                    }
                }
            }
        }
        protected void gvBonificacao_DataBound(object sender, EventArgs e)
        {

        }

        private void CarregarRecebimento(string representante, string natureza, DateTime? dataIni, DateTime? dataFim)
        {
            var recebimentoAll = contabilController.ObterMostruarioRecebimento(representante);

            if (natureza != "")
                recebimentoAll = recebimentoAll.Where(p => p.NATUREZA_COD == natureza).ToList();

            //Filtrar pela data
            var rec = recebimentoAll.Where(p => p.DATA >= Convert.ToDateTime(dataIni) && p.DATA <= Convert.ToDateTime(dataFim)).ToList();

            //Obter Linha do Saldo 
            var linha = recebimentoAll.Where(p => p.DATA <= Convert.ToDateTime(dataIni).AddDays(-1));
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
                    rec.Insert(0, saldofinal);
                }
            }

            gvRecebimento.DataSource = rec.OrderBy(x => x.COL);
            gvRecebimento.DataBind();
        }
        protected void gvRecebimento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MOSTR_REPRE_RECEBIMENTOResult rec = e.Row.DataItem as SP_OBTER_MOSTR_REPRE_RECEBIMENTOResult;

                    if (rec != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(rec.DATA).ToString("dd/MM/yyyy");

                        Literal _litEntrada = e.Row.FindControl("litEntrada") as Literal;
                        if (_litEntrada != null)
                            _litEntrada.Text = Convert.ToDecimal(rec.ENTRADA).ToString("###,###,###,##0.00");

                        Literal _litSaida = e.Row.FindControl("litSaida") as Literal;
                        if (_litSaida != null)
                            _litSaida.Text = Convert.ToDecimal(rec.SAIDA).ToString("###,###,###,##0.00");

                        Literal _litSaldo = e.Row.FindControl("litSaldo") as Literal;
                        if (_litSaldo != null)
                            _litSaldo.Text = Convert.ToDecimal(rec.SALDO).ToString("###,###,###,##0.00");
                    }
                }
            }
        }
        protected void gvRecebimento_DataBound(object sender, EventArgs e)
        {

        }




    }
}
