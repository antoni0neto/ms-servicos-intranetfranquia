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
    public partial class atac_cad_cliente_bloqueio : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                labTitulo.Text = "Liberação Financeiro";
                labMeioTitulo.Text = "Controle";
                labSubTitulo.Text = "Liberação Financeiro";

                Page.Title = labTitulo.Text;
                CarregarUF();
                CarregarJQuery();

                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {

            var colecao = baseController.ObterColecaoAtacado();
            if (colecao != null)
            {
                colecao.Insert(0, new SP_OBTER_COLECAO_ATACADOResult { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = colecao;
                ddlColecao.DataBind();
            }

        }

        private string ObterColecaoAtual()
        {
            var colecao = baseController.ObterParametroLinx("COLECAO_PADRAO");
            if (colecao != null)
            {
                var descColecao = baseController.BuscaColecaoAtual(colecao.VALOR_PARAM);
                if (descColecao != null)
                {
                    return descColecao.COLECAO.Trim();
                }
            }
            return "";
        }
        private void CarregarUF()
        {
            var uf = baseController.ObterUF();
            if (uf != null)
            {
                uf.Insert(0, new LCF_LX_UF { UF = "" });
                ddlUF.DataSource = uf;
                ddlUF.DataBind();
            }
        }
        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvCliente.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        #region "CLIENTES"
        private List<SP_OBTER_CLIENTE_ATC_BLOQResult> ObterClienteAtacadoBloq()
        {
            char? fatBloqueado = null;
            char? semCredito = null;

            if (ddlFatBloqueado.SelectedValue != "")
                fatBloqueado = Convert.ToChar(ddlFatBloqueado.SelectedValue);

            if (ddlSemCredito.SelectedValue != "")
                semCredito = Convert.ToChar(ddlSemCredito.SelectedValue);

            var cliBloq = atacController.ObterClienteAtacadoBloq(ddlColecao.SelectedValue.Trim(), txtCodClifor.Text.Trim(), txtNomeCliente.Text.Trim().ToUpper(), txtCNPJ.Text.Trim(), ddlUF.SelectedValue, fatBloqueado, semCredito);

            if (ddlTipoBloqueio.SelectedValue != "")
                cliBloq = cliBloq.Where(p => p.TIPO_BLOQUEIO == ddlTipoBloqueio.SelectedValue).ToList();

            if (ddlClienteVerificado.SelectedValue != "")
            {
                if (ddlClienteVerificado.SelectedValue == "S")
                    cliBloq = cliBloq.Where(p => p.CODIGO_ULTIMO_LOG != null).ToList();
                else
                    cliBloq = cliBloq.Where(p => p.CODIGO_ULTIMO_LOG == null).ToList();
            }

            return cliBloq;
        }
        private void CarregarClientes()
        {
            gvCliente.DataSource = ObterClienteAtacadoBloq();
            gvCliente.DataBind();

        }
        protected void gvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CLIENTE_ATC_BLOQResult cliBloq = e.Row.DataItem as SP_OBTER_CLIENTE_ATC_BLOQResult;

                    if (cliBloq != null)
                    {

                        Literal _litCNPJ = e.Row.FindControl("litCNPJ") as Literal;
                        if (_litCNPJ != null)
                            _litCNPJ.Text = Convert.ToUInt64(cliBloq.CGC_CPF).ToString(@"00\.000\.000\/0000\-00");

                        Literal _litSemCredito = e.Row.FindControl("litSemCredito") as Literal;
                        if (_litSemCredito != null)
                            _litSemCredito.Text = (cliBloq.SEM_CREDITO) ? "Sim" : "Não";

                        Literal _litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        if (_litDataPedido != null)
                            _litDataPedido.Text = (cliBloq.ULTIMO_PEDIDO_DATA == null) ? "" : Convert.ToDateTime(cliBloq.ULTIMO_PEDIDO_DATA).ToString("dd/MM/yyyy");

                        Literal _litUltimoMov = e.Row.FindControl("litUltimoMov") as Literal;
                        if (_litUltimoMov != null)
                            _litUltimoMov.Text = (cliBloq.ULTIMO_MOV == null) ? "" : Convert.ToDateTime(cliBloq.ULTIMO_MOV).ToString("dd/MM/yyyy");

                        // tem algum problema, precisa conferir
                        if (cliBloq.TIPO_BLOQUEIO == "CONFERIR")
                        {
                            e.Row.BackColor = Color.LightYellow;
                        }
                        // tem atraso e nao foi conferido
                        else if (cliBloq.ATR_MAIOR_15 == "S" && cliBloq.CODIGO_ULTIMO_LOG == null)
                        {
                            e.Row.BackColor = Color.IndianRed;
                            e.Row.ForeColor = Color.White;
                        }
                        // tem atraso e foi conferido ou observação
                        else if ((cliBloq.ATR_MAIOR_15 == "S" && cliBloq.CODIGO_ULTIMO_LOG != null) || cliBloq.TIPO_BLOQUEIO == "OBSERVACAO")
                        {
                            e.Row.BackColor = Color.LightSkyBlue;
                        }
                        //branco, os outros

                        if (cliBloq.TIPO_BLOQUEIO != "CONFERIR" && cliBloq.PRIMEIRO_VENCIMENTO_REAL != null && cliBloq.PRIMEIRO_VENCIMENTO_REAL > cliBloq.ULTIMO_MOV)
                        {
                            e.Row.BackColor = Color.Plum;
                        }
                    }
                }
            }
        }
        protected void gvCliente_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvCliente.FooterRow;
            if (_footer != null)
            {
            }
        }
        protected void gvCliente_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_CLIENTE_ATC_BLOQResult> cliBloq = ObterClienteAtacadoBloq();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            cliBloq = cliBloq.OrderBy(e.SortExpression + sortDirection);
            gvCliente.DataSource = cliBloq;
            gvCliente.DataBind();

            CarregarJQuery();
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarClientes();

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        protected void btAbrirCliente_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    string colecao = "";
                    string clifor = "";

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        colecao = ddlColecao.SelectedValue.Trim();
                        clifor = gvCliente.DataKeys[row.RowIndex].Value.ToString();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior2('atac_cad_cliente_bloqueio_log.aspx?col=" + colecao + "&clifor=" + clifor + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                    CarregarJQuery();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

    }
}

