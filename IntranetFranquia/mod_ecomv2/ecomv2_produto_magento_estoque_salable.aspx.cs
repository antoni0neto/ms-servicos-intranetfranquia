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
    public partial class ecomv2_produto_magento_estoque_salable : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var _colecoes = baseController.BuscaColecoes();

            _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
            ddlColecao.DataSource = _colecoes;
            ddlColecao.DataBind();

        }
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

        private List<SP_OBTER_ESTOQUE_TEMP_VALSALABLEV2Result> ObterEstoqueProdutos(int codigoUsuario)
        {
            if (chkAtualizarSalable.Checked)
            {
                var usuario = baseController.BuscaUsuario(codigoUsuario);
                var mag = new MagentoV2();
                mag.ObterESalvarEstoqueSalable(codigoUsuario, ddlColecao.SelectedValue.Trim());
            }

            var produtosEstoque = eController.ObterEstoqueMagentoTempV2SalableParaValidar(codigoUsuario);

            if (ddlColecao.SelectedValue != "")
                produtosEstoque = produtosEstoque.Where(p => p.COLECAO == ddlColecao.SelectedValue.Trim()).ToList();

            if (ddlGrupo.SelectedValue != "")
                produtosEstoque = produtosEstoque.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).ToList();

            if (txtProduto.Text != "")
                produtosEstoque = produtosEstoque.Where(p => p.PRODUTO == txtProduto.Text.Trim()).ToList();

            if (ddlGriffe.SelectedValue != "")
                produtosEstoque = produtosEstoque.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlDivergente.SelectedValue != "")
            {
                if (ddlDivergente.SelectedValue == "S")
                    produtosEstoque = produtosEstoque.Where(p => p.ESTOQUE_SALABLE > p.ESTOQUE_ONLINE).ToList();
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

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

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
                    SP_OBTER_ESTOQUE_TEMP_VALSALABLEV2Result estoque = e.Row.DataItem as SP_OBTER_ESTOQUE_TEMP_VALSALABLEV2Result;
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
        }


        #endregion

    }
}

