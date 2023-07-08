using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_compra_tecidofluxo : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
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
                    hrefVoltar.HRef = "desenv_menu.aspx";

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
            var fornecedores = prodController.ObterFornecedor().Where(p => p.STATUS == 'A').ToList();
            if (fornecedores != null)
            {
                var fornecedoresAux = fornecedores.Where(p => p.FORNECEDOR != null).Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

                fornecedores = new List<PROD_FORNECEDOR>();
                foreach (var item in fornecedoresAux)
                    if (item.Trim() != "")
                        fornecedores.Add(new PROD_FORNECEDOR { FORNECEDOR = item.Trim() });

                fornecedores.OrderBy(p => p.FORNECEDOR);
                fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione" });

                ddlFornecedor.DataSource = fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        #endregion

        private void CarregarJquery()
        {
            //GRID gvExtratoSacola
            if (gvPgtoTecido.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);


        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                DateTime? dataIni = Convert.ToDateTime("2019-01-01");
                DateTime? dataFim = DateTime.Now.AddYears(2);

                if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
                {
                    labErro.Text = "Selecione o Fornecedor.";
                    return;
                }

                if (txtDataIni.Text.Trim() != "")
                    dataIni = Convert.ToDateTime(txtDataIni.Text.Trim());
                if (txtDataFim.Text.Trim() != "")
                    dataFim = Convert.ToDateTime(txtDataFim.Text.Trim());

                CarregarFluxoFornecedor(ddlFornecedor.SelectedValue.Trim(), dataIni, dataFim);

                CarregarJquery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        private void CarregarFluxoFornecedor(string fornecedor, DateTime? dataIni, DateTime? dataFim)
        {
            var fluxoAll = desenvController.ObterFluxoPgtoTecido(fornecedor);

            //Filtrar pela data
            var con = fluxoAll.Where(p => p.DATA >= Convert.ToDateTime(dataIni) && p.DATA <= Convert.ToDateTime(dataFim)).ToList();

            //Obter Linha do Saldo 
            var linha = fluxoAll.Where(p => p.DATA <= Convert.ToDateTime(dataIni).AddDays(-1));
            if (linha != null && linha.Count() > 0)
            {
                var saldofinal = linha.Where(p => p.COL == linha.Max(x => x.COL)).SingleOrDefault();
                if (saldofinal != null)
                {
                    saldofinal.COL = 0;
                    saldofinal.FORNECEDOR = "";
                    saldofinal.TECIDO = "SALDO DATA";
                    saldofinal.COR_FORNECEDOR = "-";
                    saldofinal.NOTA_FISCAL = "-";
                    saldofinal.VALOR_ENTRADA = Convert.ToDecimal(0.00);
                    saldofinal.VALOR_SAIDA = Convert.ToDecimal(0.00);
                    con.Insert(0, saldofinal);
                }
            }

            gvPgtoTecido.DataSource = con.OrderBy(x => x.COL);
            gvPgtoTecido.DataBind();

        }
        protected void gvPgtoTecido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FLUXOPGTO_TECIDOResult fluxo = e.Row.DataItem as SP_OBTER_FLUXOPGTO_TECIDOResult;

                    if (fluxo != null)
                    {
                        Literal litData = e.Row.FindControl("litData") as Literal;
                        litData.Text = Convert.ToDateTime(fluxo.DATA).ToString("dd/MM/yyyy");

                        Literal litParcela = e.Row.FindControl("litParcela") as Literal;
                        litParcela.Text = (fluxo.PARCELA == 0) ? "-" : fluxo.PARCELA.ToString();

                        Literal litValorEntrada = e.Row.FindControl("litValorEntrada") as Literal;
                        litValorEntrada.Text = Convert.ToDecimal(fluxo.VALOR_ENTRADA).ToString("###,###,###,##0.00");

                        Literal litValorSaida = e.Row.FindControl("litValorSaida") as Literal;
                        litValorSaida.Text = Convert.ToDecimal(fluxo.VALOR_SAIDA).ToString("###,###,###,##0.00");

                        Literal litValorSaldo = e.Row.FindControl("litValorSaldo") as Literal;
                        litValorSaldo.Text = Convert.ToDecimal(fluxo.VALOR_SALDO).ToString("###,###,###,##0.00");
                    }
                }
            }
        }
        protected void gvPgtoTecido_DataBound(object sender, EventArgs e)
        {

        }



    }
}
