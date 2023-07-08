using DAL;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class ecom_produto_magento_estoque_difff : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarJQuery();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarJQuery()
        {
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        private List<SP_OBTER_ESTOQUE_TEMP_VALResult> ObterProduto(int codigoUsuario)
        {
            var produtos = eController.ObterMagentoProdutoSimplesCadastrado();

            var idsMagento = new List<string>();

            foreach (var prod in produtos)
            {
                idsMagento.Add(prod.ID_PRODUTO_MAG.ToString());
            }

            var usuario = baseController.BuscaUsuario(codigoUsuario);

            Magento mag = new Magento(usuario.USER_MAG_API, usuario.PASS_MAG_API);
            mag.ObterEstoqueMagento(idsMagento, codigoUsuario);

            var estoqueMagento = eController.ObterEstoqueMagentoTempParaValidar(codigoUsuario);

            return estoqueMagento;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                var estoqueMag = ObterProduto(codigoUsuario);

                gvProduto.DataSource = estoqueMag;
                gvProduto.DataBind();

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ESTOQUE_TEMP_VALResult estoque = e.Row.DataItem as SP_OBTER_ESTOQUE_TEMP_VALResult;

                    ImageButton imgAtual = e.Row.FindControl("imgAtualizarEst") as ImageButton;
                    if (imgAtual != null)
                    {
                        imgAtual.CommandArgument = estoque.ID_PRODUTO_MAG.ToString() + "|" + estoque.ESTOQUE_LINX.ToString();
                    }

                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
        }


        #endregion

        protected void imgAtualizarEst_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgAtual = (ImageButton)sender;
                GridViewRow row = (GridViewRow)imgAtual.NamingContainer;

                var values = imgAtual.CommandArgument.Split('|');
                var idProdutoMag = Convert.ToInt32(values[0].ToString());
                var qty = Convert.ToInt32(values[1].ToString());
                var ecom = eController.ObterMagentoProdutoId(idProdutoMag);
                if (ecom != null)
                {
                    int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    var usuario = baseController.BuscaUsuario(codigoUsuario);
                    Magento mag = new Magento(usuario.USER_MAG_API, usuario.PASS_MAG_API);
                    mag.AtualizarEstoque(ecom, qty, true);
                    row.BackColor = Color.PaleGreen;

                    CarregarJQuery();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

