using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
using System.Text;
using System.IO;

namespace Relatorios
{
    public partial class ecom_rel_config_aux : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupoProduto();
                CarregarGriffe();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

                ddlColecao.DataSource = colecoes;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
                }
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarGrupoProduto()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoProduto.DataSource = _grupo;
                ddlGrupoProduto.DataBind();
            }
        }

        #endregion


        #region "PRODUTO"

        protected void btBuscarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                CarregarProduto();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_AUXResult> ObterProdutoConfigCadastrado()
        {
            string colecao = ddlColecao.SelectedValue.Trim();
            string griffe = ddlGriffe.SelectedValue.Trim();
            string grupoProduto = ddlGrupoProduto.SelectedValue.Trim();

            var produtos = ecomController.ObterMagentoProdutoConfigCadastrado(colecao, griffe, grupoProduto);

            if (ddlEstoque.SelectedValue != "")
            {
                if (ddlEstoque.SelectedValue == "S")
                    produtos = produtos.Where(p => p.ESTOQUE > 0).ToList();
                else
                    produtos = produtos.Where(p => p.ESTOQUE <= 0).ToList();
            }

            return produtos;
        }
        private void CarregarProduto()
        {
            var produtos = ObterProdutoConfigCadastrado();

            gvProduto.DataSource = produtos;
            gvProduto.DataBind();
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_RELACIONADO_AUXResult produto = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_RELACIONADO_AUXResult;

                    if (produto != null)
                    {
                        System.Web.UI.WebControls.Image imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                        imgProduto.ImageUrl = produto.FOTO_FRENTE_CAB;
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_PRODUTO_RELACIONADO_AUXResult> produto = ObterProdutoConfigCadastrado();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(gvProduto, e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(gvProduto, e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(gvProduto, e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produto = produto.OrderBy(e.SortExpression + sortDirection);
            gvProduto.DataSource = produto;
            gvProduto.DataBind();

        }

        #endregion

        protected void ddlGrupoProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CarregarProduto();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }






    }
}

