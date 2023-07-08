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
    public partial class facc_emissao_nf_n : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["a"].ToString();
                if (tela == "1")
                {
                    hidTela.Value = "20";
                    labTitulo.Text = "Emissão Nota Fiscal Lote";
                    labMeioTitulo.Text = "Controle de Facção Lote";
                    labSubTitulo.Text = "Emissão Nota Fiscal Lote Fiscal";
                }
                else
                {
                    hidTela.Value = "21";
                    labTitulo.Text = "Emissão Nota Fiscal Lote Acabamento";
                    labMeioTitulo.Text = "Controle de Facção Lote Acabamento";
                    labSubTitulo.Text = "Emissão Nota Fiscal Lote Acabamento";
                }

                CarregarEmissaoNFLote("");

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarJQuery()
        {
            //GRID PRINCIPAL
            if (gvPrincipal.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        #region "EMISSAO"
        private void CarregarEmissaoNFLote(string lote)
        {
            List<PROD_HB_SAIDA_LOTE> _saidaLote = new List<PROD_HB_SAIDA_LOTE>();
            _saidaLote = ObterListaSaidaLote(lote);

            if (_saidaLote != null)
            {
                gvPrincipal.DataSource = _saidaLote;
                gvPrincipal.DataBind();
            }
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_SAIDA_LOTE _saidaLote = e.Row.DataItem as PROD_HB_SAIDA_LOTE;

                    if (_saidaLote != null)
                    {

                        Literal _litFornecedor = e.Row.FindControl("litFornecedor") as Literal;
                        if (_litFornecedor != null)
                            _litFornecedor.Text = faccController.ObterListaSaidaEmissaoLote(Convert.ToInt32(hidTela.Value)).FirstOrDefault().PROD_HB_SAIDA1.FORNECEDOR;

                        Literal _litLiberado = e.Row.FindControl("litLiberado") as Literal;
                        if (_litLiberado != null)
                            _litLiberado.Text = Convert.ToDateTime(_saidaLote.DATA_ENCAIXE).ToString("dd/MM/yyyy");

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _saidaLote.LOTE.ToString();

                    }
                }
            }
        }
        protected void gvPrincipal_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPrincipal.FooterRow;
            if (_footer != null)
            {
            }
        }
        protected void gvPrincipal_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<PROD_HB_SAIDA_LOTE> _saidaLote = ObterListaSaidaLote(txtLote.Text.Trim());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _saidaLote = _saidaLote.OrderBy(e.SortExpression + sortDirection);
            gvPrincipal.DataSource = _saidaLote;
            gvPrincipal.DataBind();

            CarregarJQuery();
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CarregarEmissaoNFLote(txtLote.Text.Trim());

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private List<PROD_HB_SAIDA_LOTE> ObterListaSaidaLote(string lote)
        {
            List<PROD_HB_SAIDA_LOTE> _saidaLote = new List<PROD_HB_SAIDA_LOTE>();

            int tela = Convert.ToInt32(hidTela.Value);

            _saidaLote.AddRange(faccController.ObterListaSaidaEmissaoLote(tela));

            if (lote.Trim() != "")
                _saidaLote = _saidaLote.Where(p => p.LOTE == Convert.ToInt32(lote)).ToList();

            _saidaLote = _saidaLote.GroupBy(p => new { LOTE = p.LOTE }).Select(g => new PROD_HB_SAIDA_LOTE
            {
                LOTE = g.Key.LOTE,
                DATA_ENCAIXE = g.Max(x => x.DATA_ENCAIXE)
            }).ToList();

            return _saidaLote.OrderBy(p => p.LOTE).ToList();
        }

        #region "BAIXA"
        protected void btBaixarLote_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string lote = b.CommandArgument;
                    int tela = Convert.ToInt32(hidTela.Value);

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('facc_emissao_nf_n_baixar.aspx?p=" + lote + "&t=" + tela.ToString() + "');";
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

