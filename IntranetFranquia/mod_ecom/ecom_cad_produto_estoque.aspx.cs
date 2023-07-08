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
using System.Collections;
using Relatorios.mod_ecom.mag;


namespace Relatorios
{
    public partial class ecom_cad_produto_estoque : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

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
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
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

        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        private List<SP_OBTER_ECOM_PRODUTO_ESTOQUEResult> ObterProduto()
        {
            var produtos = eController.ObterProdutoEstoqueMagento(ddlColecao.SelectedValue, ddlGrupo.SelectedValue, txtProduto.Text.Trim(), "", ddlGriffe.SelectedValue, ddlTipo.SelectedValue);

            if (ddlTemEstoque.SelectedValue != "")
            {
                if (ddlTemEstoque.SelectedValue == "S")
                    produtos = produtos.Where(p => p.QTDE_ESTOQUE > 0).ToList();
                else
                    produtos = produtos.Where(p => p.QTDE_ESTOQUE <= 0).ToList();
            }

            return produtos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvProduto.DataSource = ObterProduto();
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
                    SP_OBTER_ECOM_PRODUTO_ESTOQUEResult prod = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_ESTOQUEResult;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = (prod.FOTO == null) ? "" : prod.FOTO;



                    //Popular GRID VIEW FILHO
                    if (prod.QTDE_ESTOQUE > 0)
                    {
                        GridView gvEstoque = e.Row.FindControl("gvEstoque") as GridView;
                        if (gvEstoque != null)
                        {
                            List<SP_OBTER_ECOM_PRODUTO_ESTOQUEResult> produtoEstoque = new List<SP_OBTER_ECOM_PRODUTO_ESTOQUEResult>();
                            produtoEstoque.Add(new SP_OBTER_ECOM_PRODUTO_ESTOQUEResult
                            {
                                TAM1 = prod.TAM1,
                                TAM2 = prod.TAM2,
                                TAM3 = prod.TAM3,
                                TAM4 = prod.TAM4,
                                TAM5 = prod.TAM5,
                                TAM6 = prod.TAM6,
                                TAM7 = prod.TAM7,
                                TAM8 = prod.TAM8,
                                TAM9 = prod.TAM9,
                                TAM10 = prod.TAM10,
                                TAM11 = prod.TAM11,
                                TAM12 = prod.TAM12,
                                TAM13 = prod.TAM13,
                                TAM14 = prod.TAM14
                            });
                            gvEstoque.DataSource = produtoEstoque;
                            gvEstoque.DataBind();
                        }
                    }
                    else
                    {
                        System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                        if (img != null)
                            img.Visible = false;
                    }



                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_PRODUTO_ESTOQUEResult> produtos = ObterProduto();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produtos = produtos.OrderBy(e.SortExpression + sortDirection);
            gvProduto.DataSource = produtos;
            gvProduto.DataBind();

            CarregarJQuery();
        }

        protected void gvEstoque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_ESTOQUEResult _produto = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_ESTOQUEResult;
                    if (_produto != null)
                    {
                    }
                }
            }
        }
        #endregion


    }
}

