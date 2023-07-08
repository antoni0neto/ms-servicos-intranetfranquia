using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class cprod_produto_cadastro : System.Web.UI.Page
    {
        ControleProdutoController cprodController = new ControleProdutoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();


                if (tela == "1")
                    hrefVoltar.HRef = "cprod_menu.aspx";
                else
                    hrefVoltar.HRef = "../mod_financeiro/fin_prod_menu.aspx";


                CarregarColecoes();
                CarregarGriffe();
                CarregarGrupo();
                CarregarProdutos("", "");

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
            ddlColecoes.DataSource = colecoes;
            ddlColecoes.DataBind();
        }
        private void CarregarGrupo()
        {
            var _grupo = new ProducaoController().ObterGrupoProduto("01");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarJQuery()
        {
            if (gvProduto.Rows.Count <= 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        }
        #endregion

        #region "PRODUTO"
        private void CarregarProdutos(string colecao, string produto)
        {
            var prodSemCad = ObterProdutoSemCadastro(colecao, produto);

            gvProduto.DataSource = prodSemCad;
            gvProduto.DataBind();

        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_SEM_CADASTROResult _prodsemCad = e.Row.DataItem as SP_OBTER_PRODUTO_SEM_CADASTROResult;

                    if (_prodsemCad != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_prodsemCad.COLECAO)).DESC_COLECAO;

                        Literal _litProdutoAcabado = e.Row.FindControl("litProdutoAcabado") as Literal;
                        if (_litProdutoAcabado != null)
                            _litProdutoAcabado.Text = (_prodsemCad.PRODUTO_ACABADO == 'S') ? "SIM" : "NÃO";

                        Literal _litEnviado = e.Row.FindControl("litEnviado") as Literal;
                        if (_litEnviado != null)
                            _litEnviado.Text = (_prodsemCad.DATA_CADASTRO_LIB == null) ? "" : Convert.ToDateTime(_prodsemCad.DATA_CADASTRO_LIB).ToString("dd/MM/yyyy");

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _prodsemCad.COLECAO.Trim().ToString();

                        if (_prodsemCad.DATA_CADASTRO_LIB != null)
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
                    string colecao = "";
                    string produto = "";

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        colecao = gvProduto.DataKeys[row.RowIndex].Value.ToString();
                        produto = ((Literal)row.FindControl("litModelo")).Text.Trim();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('cprod_produto_cadastro_edit.aspx?c=" + colecao + "&p=" + produto + "');";
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
        #endregion

        private List<SP_OBTER_PRODUTO_SEM_CADASTROResult> ObterProdutoSemCadastro(string colecao, string produto)
        {
            var prodSemCad = cprodController.ObterProdutoSemCadastro(colecao, produto);

            if (ddlGriffe.SelectedValue != "")
                prodSemCad = prodSemCad.Where(x => x.GRIFFE.Trim() == ddlGriffe.SelectedValue.Trim()).ToList();

            if (ddlGrupo.SelectedValue != "")
                prodSemCad = prodSemCad.Where(x => x.GRUPO.Trim() == ddlGrupo.SelectedValue.Trim()).ToList();

            return prodSemCad;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarProdutos(ddlColecoes.SelectedValue.Trim(), txtProduto.Text.Trim().ToUpper());

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }
}

