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
    public partial class prod_fila_etiqueta : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //CarregarColecoes();

                CarregarPrincipal("", "", "");
                CarregarMostruario("", "", "");

                CarregarJQuery();

                string codigoPerfil = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();
                if (codigoPerfil == "27")
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";

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

            if (gvMostruario.Rows.Count <= 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionM').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionM').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        }
        #endregion

        #region "PRINCIPAL"
        private void CarregarPrincipal(string colecao, string hb, string produto)
        {
            var hbEtiqueta = ObterHBEtiqueta("N", colecao, hb, produto);
            if (hbEtiqueta != null)
            {
                gvPrincipal.DataSource = hbEtiqueta;
                gvPrincipal.DataBind();
            }
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_SEM_ETIQUETAResult _HB = e.Row.DataItem as SP_OBTER_HB_SEM_ETIQUETAResult;

                    if (_HB != null)
                    {

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _HB.CODIGO.ToString();

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
            IEnumerable<SP_OBTER_HB_SEM_ETIQUETAResult> _hb = ObterHBEtiqueta("N", "", txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _hb = _hb.OrderBy(e.SortExpression + sortDirection);
            gvPrincipal.DataSource = _hb;
            gvPrincipal.DataBind();

            CarregarJQuery();
        }
        #endregion

        #region "MOSTRUARIO"
        private void CarregarMostruario(string colecao, string hb, string produto)
        {
            var hbEtiqueta = ObterHBEtiqueta("S", colecao, hb, produto);

            if (hbEtiqueta != null)
            {
                gvMostruario.DataSource = hbEtiqueta;
                gvMostruario.DataBind();
            }
        }
        protected void gvMostruario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_SEM_ETIQUETAResult _HB = e.Row.DataItem as SP_OBTER_HB_SEM_ETIQUETAResult;

                    if (_HB != null)
                    {
                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _HB.CODIGO.ToString();

                    }
                }
            }
        }
        protected void gvMostruario_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvMostruario.FooterRow;
            if (_footer != null)
            {
            }
        }
        protected void gvMostruario_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_HB_SEM_ETIQUETAResult> _hb = ObterHBEtiqueta("S", "", txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _hb = _hb.OrderBy(e.SortExpression + sortDirection);
            gvMostruario.DataSource = _hb;
            gvMostruario.DataBind();

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
                            var codigoHB = gvPrincipal.DataKeys[row.RowIndex][1].ToString();
                            _url = "fnAbrirTelaCadastroMaior('prod_cad_hb_libera_etiqueta.aspx?t=" + tipo + "&chb=" + codigoHB + "');";
                        }
                        else
                        {
                            var pedido = gvPrincipal.DataKeys[row.RowIndex][2].ToString();
                            var produto = gvPrincipal.DataKeys[row.RowIndex][3].ToString();
                            var cor = gvPrincipal.DataKeys[row.RowIndex][4].ToString();
                            var entrega = Convert.ToDateTime(gvPrincipal.DataKeys[row.RowIndex][5].ToString()).ToString("yyyy-MM-dd");

                            _url = "fnAbrirTelaCadastroMaior('prod_cad_hb_libera_etiqueta.aspx?t=" + tipo + "&p=" + pedido + "&prod=" + produto + "&c=" + cor + "&ent=" + entrega + "');";
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
        protected void btBaixarMostruario_Click(object sender, EventArgs e)
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
                        var tipo = gvMostruario.DataKeys[row.RowIndex][0].ToString();

                        //Abrir pop-up
                        if (tipo == "00")
                        {
                            var codigoHB = gvMostruario.DataKeys[row.RowIndex][1].ToString();
                            _url = "fnAbrirTelaCadastroMaior('prod_cad_hb_libera_etiqueta.aspx?t=" + tipo + "&chb=" + codigoHB + "');";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                        }

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

        private List<SP_OBTER_HB_SEM_ETIQUETAResult> ObterHBEtiqueta(string mostruario, string colecao, string hb, string produto)
        {
            List<SP_OBTER_HB_SEM_ETIQUETAResult> prod_hb = new List<SP_OBTER_HB_SEM_ETIQUETAResult>();

            var etiqueta = prodController.ObterHBSemEtiquetaBaixada();

            if (mostruario == "" || mostruario == "S")
                prod_hb.AddRange(etiqueta.Where(p => p.MOSTRUARIO == "S").ToList());

            if (mostruario == "" || mostruario == "N")
                prod_hb.AddRange(etiqueta.Where(p => p.MOSTRUARIO == "N"));

            //if (colecao.Trim() != "")
            //    prod_hb = prod_hb.Where(p => p.COLECAO.Trim() == colecao.Trim()).ToList();

            if (hb.Trim() != "")
                prod_hb = prod_hb.Where(p => p.HB == hb).ToList();

            if (produto.Trim() != "")
                prod_hb = prod_hb.Where(p => p.PRODUTO.Trim().ToUpper() == produto).ToList();

            return prod_hb;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarPrincipal("", txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());
                CarregarMostruario("", txtHB.Text.Trim(), txtProduto.Text.Trim().ToUpper());

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }
}

