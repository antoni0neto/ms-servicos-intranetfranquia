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
    public partial class ecom_cad_produto_relacionamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarCorMagento();
                CarregarGrupoMacro();

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            var _grupo = prodController.ObterGrupoProduto("");

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
        private void CarregarCorMagento()
        {
            var corMagento = eController.ObterMagentoCor();
            if (corMagento != null)
            {
                corMagento.Insert(0, new ECOM_COR { CODIGO = 0, COR = "" });
                ddlCorMagento.DataSource = corMagento;
                ddlCorMagento.DataBind();
            }
        }
        private void CarregarTecido(string colecao)
        {
            var tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.TECIDO_POCKET != null && p.STATUS == 'A').Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();
            foreach (var item in _tecido)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor(string colecao)
        {
            List<DESENV_PRODUTO> corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');
            var _corFornecedorAux = _corFornecedor.Select(s => s.FORNECEDOR_COR.Trim()).Distinct().ToList();

            foreach (var item in _corFornecedorAux)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR_COR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.FORNECEDOR_COR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
            ddlCorFornecedor.DataSource = corFornecedorFiltro;
            ddlCorFornecedor.DataBind();
        }
        private void CarregarGrupoMacro()
        {
            var grupoMacro = eController.ObterMagentoGrupoMacro();
            if (grupoMacro != null)
            {
                grupoMacro.Insert(0, new ECOM_GRUPO_MACRO { CODIGO = 0, GRUPO_MACRO = "" });
                ddlGrupoMacro.DataSource = grupoMacro;
                ddlGrupoMacro.DataBind();
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

        private List<SP_OBTER_ECOM_PRODUTO_RELACIONADOResult> ObterProdutoRelacionado()
        {
            var produtos = eController.ObterProdutoRelacionado(ddlColecao.SelectedValue, ddlGrupo.SelectedValue, txtProduto.Text, ddlGriffe.SelectedValue, Convert.ToInt32(ddlCorMagento.SelectedValue), Convert.ToInt32(ddlGrupoMacro.SelectedValue), ddlTecido.SelectedValue.Trim(), ddlCorFornecedor.SelectedValue.Trim(), 0);

            if (ddlRelacionado.SelectedValue != "")
            {
                if (ddlRelacionado.SelectedValue == "S")
                    produtos = produtos.Where(p => p.TEM_RELACIONADO > 0).ToList();
                else
                    produtos = produtos.Where(p => p.TEM_RELACIONADO == 0).ToList();
            }

            return produtos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a coleção.";
                    return;
                }

                gvProduto.DataSource = ObterProdutoRelacionado();
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
                    SP_OBTER_ECOM_PRODUTO_RELACIONADOResult prod = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_RELACIONADOResult;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = (prod.FOTO_FRENTE_CAB == null) ? "" : prod.FOTO_FRENTE_CAB;

                    Button _btAbrir = e.Row.FindControl("btAbrir") as Button;
                    _btAbrir.CommandArgument = prod.CODIGO.ToString();

                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_PRODUTO_RELACIONADOResult> produtos = ObterProdutoRelacionado();

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

        #endregion

        protected void btAbrir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            string codigo = bt.CommandArgument;

            Response.Redirect("ecom_cad_produto_relacionamento_edit.aspx?codp=" + codigo, "_blank", "");

            CarregarJQuery();
        }

        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecao.SelectedValue.Trim();

            CarregarTecido(colecao);
            CarregarCorFornecedor(colecao);
        }



    }
}

