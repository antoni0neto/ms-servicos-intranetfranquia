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
using System.Text.RegularExpressions;


namespace Relatorios
{
    public partial class facc_resumo_status : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        int corteGrande = 0;
        int cortePequeno = 0;
        int corteGradeATR = 0;
        int cortePequenoATR = 0;
        int corteMostruario = 0;
        int corteGrandeTotal = 0;
        int cortePequenoTotal = 0;
        int corteTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "facc_menu_relatorio.aspx";

                CarregarFornecedores();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_FACCAO_STATUSResult> ObterFaccaoStatus()
        {
            var faccaoStatus = faccController.ObterFaccaoStatus(ddlFornecedor.SelectedValue.Trim(), ddlFornecedorSub.SelectedValue.Trim());

            return faccaoStatus;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var status = ObterFaccaoStatus();

                gvFaccaoStatus.DataSource = status.OrderBy(p => p.FORNECEDOR);
                gvFaccaoStatus.DataBind();

                if (status == null || status.Count() <= 0)
                    labErro.Text = "Nenhum registro encontrado. Refaça sua pesquisa.";
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvFaccaoStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_STATUSResult faccaoStatus = e.Row.DataItem as SP_OBTER_FACCAO_STATUSResult;

                    if (faccaoStatus != null)
                    {
                        Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                        if (_labFornecedor != null)
                            _labFornecedor.Text = faccaoStatus.FORNECEDOR + " " + faccaoStatus.FORNECEDOR_SUB;


                        corteGrande += faccaoStatus.CORTE_GRANDE;
                        cortePequeno += faccaoStatus.CORTE_PEQUENO;
                        corteGradeATR += faccaoStatus.CORTE_GRANDE_ATR;
                        cortePequenoATR += faccaoStatus.CORTE_PEQUENO_ATR;
                        corteMostruario += faccaoStatus.MOSTRUARIO;
                        corteGrandeTotal += faccaoStatus.TOTAL_GRANDE;
                        cortePequenoTotal += faccaoStatus.TOTAL_PEQUENO;
                        corteTotal += faccaoStatus.TOTAL;
                    }
                }
            }
        }
        protected void gvFaccaoStatus_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFaccaoStatus.FooterRow;
            if (_footer != null)
            {

                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[3].Text = corteGrande.ToString();
                _footer.Cells[4].Text = cortePequeno.ToString();
                _footer.Cells[5].Text = corteGradeATR.ToString();
                _footer.Cells[6].Text = cortePequenoATR.ToString();
                _footer.Cells[7].Text = corteMostruario.ToString();
                _footer.Cells[8].Text = corteGrandeTotal.ToString();
                _footer.Cells[9].Text = cortePequenoTotal.ToString();
                _footer.Cells[10].Text = corteTotal.ToString();
            }
        }
        protected void gvFaccaoStatus_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_STATUSResult> _faccStatus = ObterFaccaoStatus();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _faccStatus = _faccStatus.OrderBy(e.SortExpression + sortDirection);
            gvFaccaoStatus.DataSource = _faccStatus;
            gvFaccaoStatus.DataBind();
        }

        protected void btAbrir_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";

                    GridViewRow row = (GridViewRow)b.NamingContainer;

                    if (row != null)
                    {
                        string fornecedor = ((HiddenField)row.FindControl("hidFornecedor")).Value.Trim();
                        string fornecedorSub = ((HiddenField)row.FindControl("hidFornecedorSub")).Value.Trim();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('facc_resumo_status_det.aspx?f=" + fornecedor + "&s=" + fornecedorSub + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }

        }
        private void CarregarFornecedoresSub(string fornecedor)
        {

            List<PROD_FORNECEDOR_SUB> _fornecedoresSub = faccController.ObterFornecedorSub(fornecedor);

            if (_fornecedoresSub != null)
            {
                _fornecedoresSub.Insert(0, new PROD_FORNECEDOR_SUB { FORNECEDOR_SUB = "" });

                ddlFornecedorSub.DataSource = _fornecedoresSub;
                ddlFornecedorSub.DataBind();
            }

        }
        protected void ddlFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fornecedor = "";
            fornecedor = ddlFornecedor.SelectedItem.Text.Trim();
            if (fornecedor == "HANDBOOK")
            {
                CarregarFornecedoresSub(fornecedor);
                ddlFornecedorSub.Visible = true;
                labFornecedorSub.Visible = true;
            }
            else
            {
                ddlFornecedorSub.SelectedValue = "";
                ddlFornecedorSub.Visible = false;
                labFornecedorSub.Visible = false;
            }
        }
        #endregion

    }
}
