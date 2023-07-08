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
    public partial class pacab_entrada_produto : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();

                CarregarEntradaProduto("", "", "");

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
        #endregion

        #region "ENTRADA"
        private List<SP_OBTER_PRODUTO_ACABADO_ENTRADAResult> ObterProdutoAcabadoEntrada(string colecao, string produto, string nome)
        {
            var produtoAcabado = desenvController.ObterProdutoAcabadoEntrada(colecao, produto, nome);
            return produtoAcabado;
        }
        private void CarregarEntradaProduto(string colecao, string produto, string nome)
        {
            var _entrada = ObterProdutoAcabadoEntrada(colecao, produto, nome);

            gvPrincipal.DataSource = _entrada;
            gvPrincipal.DataBind();

        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADO_ENTRADAResult _entrada = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADO_ENTRADAResult;

                    if (_entrada != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_entrada.COLECAO)).DESC_COLECAO;

                    }
                }
            }
        }
        protected void gvPrincipal_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPrincipal.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvPrincipal.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                _footer.Cells[6].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvPrincipal_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PRODUTO_ACABADO_ENTRADAResult> _entrada = ObterProdutoAcabadoEntrada(ddlColecoes.SelectedValue.Trim(), txtProduto.Text.Trim(), txtNome.Text.Trim().ToUpper());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _entrada = _entrada.OrderBy(e.SortExpression + sortDirection);
            gvPrincipal.DataSource = _entrada;
            gvPrincipal.DataBind();
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarEntradaProduto(ddlColecoes.SelectedValue.Trim(), txtProduto.Text.Trim(), txtNome.Text.Trim().ToUpper());
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }


        protected void btDefinirGrade_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;

                    string produto = gvPrincipal.DataKeys[row.RowIndex][0].ToString().Trim();
                    string cor = gvPrincipal.DataKeys[row.RowIndex][1].ToString().Trim();

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('pacab_entrada_produto_baixar.aspx?p=" + produto + "&c=" + cor + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);

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

