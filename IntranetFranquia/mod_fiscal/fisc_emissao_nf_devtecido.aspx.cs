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
    public partial class fisc_emissao_nf_devtecido : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                CarregarEmissaoNF();

                CarregarJQuery();
            }

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
        private void CarregarEmissaoNF()
        {
            var devEmissao = desenvController.ObterPedidoQtdeEmissaoNF();

            gvPrincipal.DataSource = devEmissao;
            gvPrincipal.DataBind();
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_QTDE pedidoQtde = e.Row.DataItem as DESENV_PEDIDO_QTDE;

                    if (pedidoQtde != null)
                    {
                        Literal litFornecedor = e.Row.FindControl("litFornecedor") as Literal;
                        litFornecedor.Text = pedidoQtde.DESENV_PEDIDO_SUB1.FORNECEDOR;

                        Literal litTecido = e.Row.FindControl("litTecido") as Literal;
                        litTecido.Text = pedidoQtde.DESENV_PEDIDO1.SUBGRUPO;

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = pedidoQtde.DESENV_PEDIDO1.COR.Trim() + " - " + prodController.ObterCoresBasicas(pedidoQtde.DESENV_PEDIDO1.COR.Trim()).DESC_COR.Trim();

                        Literal litCorFornecedor = e.Row.FindControl("litCorFornecedor") as Literal;
                        litCorFornecedor.Text = pedidoQtde.DESENV_PEDIDO1.COR_FORNECEDOR;

                        Literal litNFOrigem = e.Row.FindControl("litNFOrigem") as Literal;
                        litNFOrigem.Text = pedidoQtde.NOTA_FISCAL;

                        Literal litQtde = e.Row.FindControl("litQtde") as Literal;
                        litQtde.Text = (pedidoQtde.QTDE * -1.00M).ToString("###,###,###,##0.000");

                        Literal litDataNota = e.Row.FindControl("litDataNota") as Literal;
                        litDataNota.Text = (pedidoQtde.DATA_NOTA == null) ? "-" : Convert.ToDateTime(pedidoQtde.DATA_NOTA).ToString("dd/MM/yyyy");

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

        #endregion

        #region "BAIXA"
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            var msg = "";
            try
            {
                Button b = (Button)sender;

                if (b != null)
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        var codigoDesenvQtde = gvPrincipal.DataKeys[row.RowIndex].Value.ToString();

                        //Abrir pop-up
                        var url = "fnAbrirTelaCadastroMaior('fisc_emissao_nf_devtecido_baixar.aspx?p=" + codigoDesenvQtde + "&t=" + "" + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
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
        #endregion

    }
}

