using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_fila_aviamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //CarregarColecoes();

                CarregarPrincipal("", "");

                CarregarJQuery();

                //string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();
                //if (codigoPerfil == "27")
                //    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"

        private void CarregarJQuery()
        {
            if (gvPrincipal.Rows.Count <= 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        }
        #endregion

        #region "PRINCIPAL"
        private void CarregarPrincipal(string hb, string produto)
        {
            var produtoLib = ObterProdutoLiberacao(hb, produto);
            if (produtoLib != null)
            {
                gvPrincipal.DataSource = produtoLib;
                gvPrincipal.DataBind();
            }
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_SEMTAGAVIResult lib = e.Row.DataItem as SP_OBTER_PRODUTO_SEMTAGAVIResult;

                    if (lib != null)
                    {
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
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
            IEnumerable<SP_OBTER_PRODUTO_SEMTAGAVIResult> prodLib = ObterProdutoLiberacao(txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prodLib = prodLib.OrderBy(e.SortExpression + sortDirection);
            gvPrincipal.DataSource = prodLib;
            gvPrincipal.DataBind();

            CarregarJQuery();
        }
        #endregion

        #region "BAIXAR"
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
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
                        //TIPO, CODIGO, PEDIDO, PRODUTO, COR

                        var tipo = gvPrincipal.DataKeys[row.RowIndex][0].ToString();

                        //Abrir pop-up
                        if (tipo == "00")
                        {
                            //var codigoHB = gvPrincipal.DataKeys[row.RowIndex][1].ToString();
                            //_url = "fnAbrirTelaCadastroMaior('prod_cad_hb_libera_etiqueta.aspx?t=" + tipo + "&chb=" + codigoHB + "');";
                        }
                        else
                        {
                            var pedido = gvPrincipal.DataKeys[row.RowIndex][1].ToString().Trim();
                            var produto = gvPrincipal.DataKeys[row.RowIndex][2].ToString().Trim();
                            var cor = gvPrincipal.DataKeys[row.RowIndex][3].ToString().Trim();
                            var entrega = Convert.ToDateTime(gvPrincipal.DataKeys[row.RowIndex][4].ToString()).ToString("yyyy-MM-dd").Trim();

                            _url = "fnAbrirTelaCadastroMaior('desenv_fila_tagavi_baixa.aspx?t=" + tipo + "&p=" + pedido + "&prod=" + produto + "&c=" + cor + "&ent=" + entrega + "&eti=a');";
                        }

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

        private List<SP_OBTER_PRODUTO_SEMTAGAVIResult> ObterProdutoLiberacao(string hb, string produto)
        {
            var lib = prodController.ObterObterProdutoSemTAGAVI().Where(p => p.DATA_BAIXA_AVIAMENTO == null).ToList();

            if (hb.Trim() != "")
                lib = lib.Where(p => p.HB == hb).ToList();

            if (produto.Trim() != "")
                lib = lib.Where(p => p.PRODUTO.Trim().ToUpper() == produto).ToList();

            return lib;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarPrincipal(txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }
}

