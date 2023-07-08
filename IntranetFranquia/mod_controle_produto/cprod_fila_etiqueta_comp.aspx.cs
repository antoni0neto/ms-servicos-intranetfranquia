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
    public partial class cprod_fila_etiqueta_comp : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ControleProdutoController cprodController = new ControleProdutoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                CarregarProdutos("", null, "");
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

        #region "PRODUTO"
        private void CarregarProdutos(string colecao, int? hb, string produto)
        {
            var libEtiqueta = ObterProdutoCortado(colecao, hb, produto).Where(p => p.MOSTRUARIO == "N");

            gvPrincipal.DataSource = libEtiqueta;
            gvPrincipal.DataBind();

            var libEtiquetaMostruario = ObterProdutoCortado(colecao, hb, produto).Where(p => p.MOSTRUARIO == "S");

            gvMostruario.DataSource = libEtiquetaMostruario;
            gvMostruario.DataBind();
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_CORTADOResult prodCortado = e.Row.DataItem as SP_OBTER_PRODUTO_CORTADOResult;

                    if (prodCortado != null)
                    {

                        Literal _litRecebido = e.Row.FindControl("litRecebido") as Literal;
                        if (_litRecebido != null)
                            _litRecebido.Text = (prodCortado.DATA_ENVIO_ETI_COMP == null) ? "-" : Convert.ToDateTime(prodCortado.DATA_ENVIO_ETI_COMP).ToString("dd/MM/yyyy");

                        Button btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (prodCortado.TIPO != "00")
                            btBaixar.Text = "Baixar";


                    }
                }
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            var mostruario = "S";
            int? hb = null;

            GridView gv = (GridView)sender;
            if (gv.ID == "gvPrincipal")
                mostruario = "N";

            if (txtHB.Text.Trim() != "")
                hb = Convert.ToInt32(txtHB.Text.Trim());

            IEnumerable<SP_OBTER_PRODUTO_CORTADOResult> _hb = ObterProdutoCortado("", hb, txtProduto.Text.Trim()).Where(p => p.MOSTRUARIO == mostruario);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gv, e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(gv, e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(gv, e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _hb = _hb.OrderBy(e.SortExpression + sortDirection);
            gv.DataSource = _hb;
            gv.DataBind();

            CarregarJQuery();
        }
        #endregion

        #region "BAIXAR"
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            string msg = "";
            string _url = "";
            if (bt != null)
            {
                try
                {
                    labErro.Text = "";

                    //TIPO, PEDIDO, PRODUTO, COR, PROD_HB
                    GridViewRow row = (GridViewRow)bt.NamingContainer;

                    var tipo = gvPrincipal.DataKeys[row.RowIndex][0].ToString();
                    var pedido = gvPrincipal.DataKeys[row.RowIndex][1].ToString();
                    var produto = gvPrincipal.DataKeys[row.RowIndex][2].ToString();
                    var cor = gvPrincipal.DataKeys[row.RowIndex][3].ToString();
                    var codigoHB = gvPrincipal.DataKeys[row.RowIndex][4].ToString();
                    var entrega = Convert.ToDateTime(gvPrincipal.DataKeys[row.RowIndex][5].ToString());

                    if (tipo == "00")
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('cprod_fila_etiqueta_comp_baixa.aspx?p=" + codigoHB + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }
                    else
                    {
                        var prePedidoProduto = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                        if (prePedidoProduto != null)
                        {
                            prePedidoProduto.DATA_BAIXA_ETI_COMP = DateTime.Now;
                            prePedidoProduto.USUARIO_BAIXA_ETI_COMP = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                            desenvController.AtualizarComprasProdutoPrePedido(prePedidoProduto);

                            bt.Enabled = false;
                            bt.Visible = false;
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

        protected void btBaixarMostruario_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            string msg = "";
            string _url = "";
            if (bt != null)
            {
                try
                {
                    labErro.Text = "";

                    //TIPO, PEDIDO, PRODUTO, COR, PROD_HB
                    GridViewRow row = (GridViewRow)bt.NamingContainer;

                    var tipo = gvMostruario.DataKeys[row.RowIndex][0].ToString();
                    var pedido = gvMostruario.DataKeys[row.RowIndex][1].ToString();
                    var produto = gvMostruario.DataKeys[row.RowIndex][2].ToString();
                    var cor = gvMostruario.DataKeys[row.RowIndex][3].ToString();
                    var codigoHB = gvMostruario.DataKeys[row.RowIndex][4].ToString();
                    var entrega = Convert.ToDateTime(gvMostruario.DataKeys[row.RowIndex][5].ToString());

                    if (tipo == "00")
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('cprod_fila_etiqueta_comp_baixa.aspx?p=" + codigoHB + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }
                    else
                    {
                        var prePedidoProduto = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                        if (prePedidoProduto != null)
                        {
                            prePedidoProduto.DATA_BAIXA_ETI_COMP = DateTime.Now;
                            prePedidoProduto.USUARIO_BAIXA_ETI_COMP = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                            desenvController.AtualizarComprasProdutoPrePedido(prePedidoProduto);

                            bt.Enabled = false;
                            bt.Visible = false;
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

        private List<SP_OBTER_PRODUTO_CORTADOResult> ObterProdutoCortado(string colecao, int? hb, string produto)
        {
            List<SP_OBTER_PRODUTO_CORTADOResult> prodCortado = new List<SP_OBTER_PRODUTO_CORTADOResult>();

            prodCortado = cprodController.ObterProdutoCortado(null, hb, "", produto, "");

            return prodCortado;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                int? hb = null;

                if (txtHB.Text.Trim() != "")
                    hb = Convert.ToInt32(txtHB.Text.Trim());

                CarregarProdutos("", hb, txtProduto.Text.Trim().ToUpper());

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }


    }
}

