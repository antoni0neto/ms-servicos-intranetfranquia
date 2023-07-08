using DAL;
using Relatorios.mod_ecom.mag;
using Relatorios.mod_ecomv2.mag2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class ecomv2_produto_magento_estoque_tamanho : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarGrupo();
                CarregarGriffe();
                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            var grupos = prodController.ObterGrupoProduto("");

            grupos.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
            ddlGrupo.DataSource = grupos;
            ddlGrupo.DataBind();

        }
        private void CarregarGriffe()
        {
            var griffes = baseController.BuscaGriffes();

            griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
            ddlGriffe.DataSource = griffes;
            ddlGriffe.DataBind();

        }
        private void CarregarJQuery()
        {
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        private List<SP_OBTER_ESTOQUE_TEMP_VALV2Result> ObterEstoqueProdutos(int codigoUsuario)
        {
            var usuario = baseController.BuscaUsuario(codigoUsuario);
            var mag = new MagentoV2();
            mag.ObterESalvarEstoque(codigoUsuario);

            var produtosEstoque = eController.ObterEstoqueMagentoTempV2ParaValidar(codigoUsuario);

            if (ddlGrupo.SelectedValue != "")
                produtosEstoque = produtosEstoque.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).ToList();

            if (txtProduto.Text != "")
                produtosEstoque = produtosEstoque.Where(p => p.PRODUTO == txtProduto.Text.Trim()).ToList();

            if (ddlGriffe.SelectedValue != "")
                produtosEstoque = produtosEstoque.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlMagentoMaiorLinx.SelectedValue != "")
            {
                if (ddlMagentoMaiorLinx.SelectedValue == "S")
                    produtosEstoque = produtosEstoque.Where(p => p.ESTOQUE_ONLINE > p.ESTOQUE_LINX).ToList();
            }

            return produtosEstoque;
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

                var produtosEstoque = ObterEstoqueProdutos(codigoUsuario);

                gvProduto.DataSource = produtosEstoque;
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
                    SP_OBTER_ESTOQUE_TEMP_VALV2Result estoque = e.Row.DataItem as SP_OBTER_ESTOQUE_TEMP_VALV2Result;

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
                    var mag = new MagentoV2();
                    mag.AtualizarEstoque(ecom, qty);
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

