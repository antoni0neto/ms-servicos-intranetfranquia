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
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class facc_avaliacao_prod : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        int entFaltante = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarFornecedores();

                //Carregar Grid Inicial
                var aval = faccController.ObterFaccaoAvaliacaoRel("", null, "", "").Where(p => p.NOTA1 == null && p.GRADE_RECEBIDA > 0);
                gvAvaliacao.DataSource = aval;
                gvAvaliacao.DataBind();


                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }
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
        private void CarregarJQuery()
        {
            //GRID PRINCIPAL
            if (gvAvaliacao.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        #region "PRINCIPAL"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                int? hb = null;
                if (txtHB.Text.Trim() != "")
                    hb = Convert.ToInt32(txtHB.Text.Trim());

                CarregarAvaliacao(ddlColecoes.SelectedValue.Trim(), hb, txtProduto.Text.Trim(), ddlFornecedor.SelectedValue.Trim());

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarAvaliacao(string colecao, int? hb, string produto, string fornecedor)
        {
            gvAvaliacao.DataSource = ObterAvaliacao(colecao, hb, produto, fornecedor);
            gvAvaliacao.DataBind();
        }

        private List<SP_OBTER_FACCAO_AVALResult> ObterAvaliacao(string colecao, int? hb, string produto, string fornecedor)
        {
            var aval = faccController.ObterFaccaoAvaliacaoRel(colecao, hb, produto, fornecedor);

            if (cbSemAvaliacao.Checked)
                aval = aval.Where(p => p.NOTA1 == null).ToList();

            if (ddlTemEntrada.SelectedValue != "")
            {
                if (ddlTemEntrada.SelectedValue == "S")
                    aval = aval.Where(p => p.GRADE_RECEBIDA > 0).ToList();
                else
                    aval = aval.Where(p => p.GRADE_RECEBIDA == 0).ToList();
            }

            return aval;
        }
        protected void gvAvaliacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_AVALResult aval = e.Row.DataItem as SP_OBTER_FACCAO_AVALResult;

                    if (aval != null)
                    {
                        Literal _litEnvio = e.Row.FindControl("litEnvio") as Literal;
                        if (_litEnvio != null)
                            _litEnvio.Text = Convert.ToDateTime(aval.ENVIO).ToString("dd/MM/yyyy");

                        Literal _litDataAvaliacao = e.Row.FindControl("litDataAvaliacao") as Literal;
                        if (_litDataAvaliacao != null)
                            _litDataAvaliacao.Text = (aval.DATA_AVALIACAO == null) ? "" : Convert.ToDateTime(aval.DATA_AVALIACAO).ToString("dd/MM/yyyy");

                        entFaltante += Convert.ToInt32(aval.QTDE_FALTANTE);
                    }
                }
            }
        }
        protected void gvAvaliacao_Sorting(object sender, GridViewSortEventArgs e)
        {
            int? hb = null;
            if (txtHB.Text.Trim() != "")
                hb = Convert.ToInt32(txtHB.Text.Trim());

            IEnumerable<SP_OBTER_FACCAO_AVALResult> avaliacao = ObterAvaliacao(ddlColecoes.SelectedValue.Trim(), hb, txtProduto.Text.Trim(), ddlFornecedor.SelectedValue.Trim());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            avaliacao = avaliacao.OrderBy(e.SortExpression + sortDirection);
            gvAvaliacao.DataSource = avaliacao;
            gvAvaliacao.DataBind();

            CarregarJQuery();
        }
        protected void gvAvaliacao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAvaliacao.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[8].Text = entFaltante.ToString();
                footer.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        #endregion


        #region "BAIXA"
        protected void btAvaliar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string codigoAvaliacao = "";

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        codigoAvaliacao = gvAvaliacao.DataKeys[row.RowIndex].Value.ToString();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('facc_avaliacao_prod_baixa.aspx?p=" + codigoAvaliacao + "');";
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
        #endregion


    }
}

