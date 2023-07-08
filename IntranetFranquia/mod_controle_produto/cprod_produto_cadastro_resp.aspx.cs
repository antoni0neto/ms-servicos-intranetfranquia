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
using System.Drawing;

namespace Relatorios
{
    public partial class cprod_produto_cadastro_resp : System.Web.UI.Page
    {
        ControleProdutoController cprodController = new ControleProdutoController();

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
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "cprod_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                CarregarProdutos("", null, "");

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarJQuery()
        {
            if (gvProduto.Rows.Count <= 0)
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

            gvProduto.DataSource = libEtiqueta;
            gvProduto.DataBind();

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
                    SP_OBTER_PRODUTO_CORTADOResult _prodCortado = e.Row.DataItem as SP_OBTER_PRODUTO_CORTADOResult;

                    if (_prodCortado != null)
                    {

                        Literal _litEnviado = e.Row.FindControl("litEnviado") as Literal;
                        if (_litEnviado != null)
                            _litEnviado.Text = (_prodCortado.DATA_ENVIO_ETI_COMP == null) ? "-" : Convert.ToDateTime(_prodCortado.DATA_ENVIO_ETI_COMP).ToString("dd/MM/yyyy");

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _prodCortado.PROD_HB.ToString();

                        if (_prodCortado.DATA_ENVIO_ETI_COMP != null)
                            e.Row.BackColor = Color.Lavender;
                    }
                }
            }
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
                    string codigoHB = "";
                    codigoHB = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('cprod_produto_cadastro_resp_edit.aspx?p=" + codigoHB + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);

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
            var prodCortado = cprodController.ObterProdutoCortado(null, hb, "", produto, "").Where(p => p.TIPO == "00").ToList();

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

