using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class contabil_cad_ncm : System.Web.UI.Page
    {
        ControleProdutoController cprodController = new ControleProdutoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();

                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "contabil_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_fiscal/fisc_menu.aspx";

                CarregarGrupo();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoProduto.DataSource = _grupo;
                ddlGrupoProduto.DataBind();
            }
        }

        #endregion

        #region "NCM"

        private List<PRODUTO_NCM> ObterProdutoNCM(string grupoProduto, string subGrupoProduto, string griffe, string composicao)
        {
            var produtoNCM = cprodController.ObterProdutoNCM();

            if (grupoProduto.Trim() != "")
                produtoNCM = produtoNCM.Where(p => p.GRUPO_PRODUTO.Trim() == grupoProduto.Trim()).ToList();

            if (subGrupoProduto.Trim() != "")
                produtoNCM = produtoNCM.Where(p => p.SUBGRUPO_PRODUTO.Trim() == subGrupoProduto.Trim()).ToList();

            if (griffe.Trim() != "")
                produtoNCM = produtoNCM.Where(p => p.GRIFFE.Trim() == griffe.Trim()).ToList();

            if (composicao.Trim() != "")
                produtoNCM = produtoNCM.Where(p => p.COMPOSICAO.Trim() == composicao.Trim()).ToList();

            return produtoNCM;
        }
        private void CarregarProdutoNCM(string grupoProduto, string subGrupoProduto, string griffe, string composicao)
        {
            var produtoNCM = ObterProdutoNCM(grupoProduto, subGrupoProduto, griffe, composicao);

            gvNCM.DataSource = produtoNCM;
            gvNCM.DataBind();

        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarProdutoNCM(ddlGrupoProduto.SelectedValue, ddlSubGrupoProduto.SelectedValue, ddlGriffe.SelectedValue, ddlComposicao.SelectedValue);

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        protected void gvNCM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PRODUTO_NCM produtoNCM = e.Row.DataItem as PRODUTO_NCM;

                    if (produtoNCM != null)
                    {

                        TextBox _txtNCM = e.Row.FindControl("txtNCM") as TextBox;
                        if (_txtNCM != null)
                            _txtNCM.Text = produtoNCM.CLASSIF_FISCAL.Trim();

                        Button _btSalvar = e.Row.FindControl("btSalvar") as Button;
                        if (_btSalvar != null)
                            _btSalvar.CommandArgument = produtoNCM.CODIGO.ToString();

                    }
                }
            }
        }

        protected void gvNCM_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<PRODUTO_NCM> _produtoNCM = ObterProdutoNCM(ddlGrupoProduto.SelectedValue, ddlSubGrupoProduto.SelectedValue, ddlGriffe.SelectedValue, ddlComposicao.SelectedValue);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _produtoNCM = _produtoNCM.OrderBy(e.SortExpression + sortDirection);
            gvNCM.DataSource = _produtoNCM;
            gvNCM.DataBind();
        }

        #endregion

        #region "SALVAR"
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";

            if (b != null)
            {
                labMsg.Text = "";

                try
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        TextBox _txtNCM = row.FindControl("txtNCM") as TextBox;
                        if (_txtNCM != null)
                        {

                            if (_txtNCM.Text.Trim() == "" || _txtNCM.Text.Trim().Length != 8)
                            {
                                msg = "NCM inválido.";
                                labMsg.Text = msg;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Erro', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }

                            int codigoNCM = Convert.ToInt32(b.CommandArgument);
                            var produtoNCM = cprodController.ObterProdutoNCM(codigoNCM);
                            if (produtoNCM != null)
                            {
                                produtoNCM.CLASSIF_FISCAL = _txtNCM.Text.Trim();
                                produtoNCM.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                produtoNCM.DATA_ALTERACAO = DateTime.Now;
                                cprodController.AtualizarProdutoNCM(produtoNCM);

                                msg = "NCM alterado com sucesso.";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Sucesso', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        #endregion




    }
}

