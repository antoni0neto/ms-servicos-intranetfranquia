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
    public partial class ecom_rel_config_rel_lista : System.Web.UI.Page
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

        protected void ddlFiltroCaract_SelectedIndexChanged(object sender, EventArgs e)
        {
            var griffe = ddlGriffe.SelectedValue.Trim();
            var grupoProduto = ddlGrupo.SelectedValue.Trim();

            CarregarTipoModelagem(griffe, grupoProduto);
            CarregarTipoTecido(griffe, grupoProduto);
            CarregarTipoManga(griffe, grupoProduto);
            CarregarTipoGola(griffe, grupoProduto);
            CarregarTipoComprimento(griffe, grupoProduto);
            CarregarTipoEstilo(griffe, grupoProduto);
        }

        private void CarregarTipoModelagem(string griffe, string grupoProduto)
        {
            ddlTipoModelagem.Enabled = true;
            var tipoModelagem = eController.ObterMagentoTipoModelagem(griffe, grupoProduto);
            if (tipoModelagem != null && tipoModelagem.Count() > 0)
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = 0, TIPO_MODELAGEM = "" });
            }
            else
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = 0, TIPO_MODELAGEM = "-" });
                ddlTipoModelagem.Enabled = false;
            }

            ddlTipoModelagem.DataSource = tipoModelagem;
            ddlTipoModelagem.DataBind();
        }
        private void CarregarTipoTecido(string griffe, string grupoProduto)
        {
            ddlTipoTecido.Enabled = true;
            var tipoTecido = eController.ObterMagentoTipoTecido(griffe, grupoProduto);
            if (tipoTecido != null && tipoTecido.Count() > 0)
            {
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = 0, TIPO_TECIDO = "" });
            }
            else
            {
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = 0, TIPO_TECIDO = "-" });
                ddlTipoTecido.Enabled = false;
            }

            ddlTipoTecido.DataSource = tipoTecido;
            ddlTipoTecido.DataBind();
        }
        private void CarregarTipoManga(string griffe, string grupoProduto)
        {
            ddlTipoManga.Enabled = true;
            var tipoManga = eController.ObterMagentoTipoManga(griffe, grupoProduto);
            if (tipoManga != null && tipoManga.Count() > 0)
            {
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = 0, TIPO_MANGA = "" });
            }
            else
            {
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = 0, TIPO_MANGA = "-" });
                ddlTipoManga.Enabled = false;
            }

            ddlTipoManga.DataSource = tipoManga;
            ddlTipoManga.DataBind();
        }
        private void CarregarTipoGola(string griffe, string grupoProduto)
        {
            ddlTipoGola.Enabled = true;
            var tipoGola = eController.ObterMagentoTipoGola(griffe, grupoProduto);
            if (tipoGola != null && tipoGola.Count() > 0)
            {
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = 0, TIPO_GOLA = "" });
            }
            else
            {
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = 0, TIPO_GOLA = "-" });
                ddlTipoGola.Enabled = false;
            }

            ddlTipoGola.DataSource = tipoGola;
            ddlTipoGola.DataBind();
        }
        private void CarregarTipoComprimento(string griffe, string grupoProduto)
        {
            ddlTipoComprimento.Enabled = true;
            var tipoComprimento = eController.ObterMagentoTipoComprimento(griffe, grupoProduto);
            if (tipoComprimento != null && tipoComprimento.Count() > 0)
            {
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = 0, TIPO_COMPRIMENTO = "" });
            }
            else
            {
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = 0, TIPO_COMPRIMENTO = "-" });
                ddlTipoComprimento.Enabled = false;
            }

            ddlTipoComprimento.DataSource = tipoComprimento;
            ddlTipoComprimento.DataBind();
        }
        private void CarregarTipoEstilo(string griffe, string grupoProduto)
        {
            ddlTipoEstilo.Enabled = true;
            var tipoEstilo = eController.ObterMagentoTipoEstilo(griffe, grupoProduto);
            if (tipoEstilo != null && tipoEstilo.Count() > 0)
            {
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = 0, TIPO_ESTILO = "" });
            }
            else
            {
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = 0, TIPO_ESTILO = "-" });
                ddlTipoEstilo.Enabled = false;
            }

            ddlTipoEstilo.DataSource = tipoEstilo;
            ddlTipoEstilo.DataBind();
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


            if (ddlEstoque.SelectedValue != "")
            {
                if (ddlEstoque.SelectedValue == "S")
                    produtos = produtos.Where(p => p.ESTOQUE > 0).ToList();
                else
                    produtos = produtos.Where(p => p.ESTOQUE <= 0).ToList();
            }

            if (ddlRelacionado.SelectedValue != "")
            {
                if (ddlRelacionado.SelectedValue == "S")
                    produtos = produtos.Where(p => p.TEM_RELACIONADO > 0).ToList();
                else
                    produtos = produtos.Where(p => p.TEM_RELACIONADO == 0).ToList();
            }

            //caracteristicas
            var codigoModelagem = ddlTipoModelagem.SelectedValue;
            if (codigoModelagem != null && codigoModelagem != "" && codigoModelagem != "0")
                produtos = produtos.Where(p => p.ECOM_TIPO_MODELAGEM == Convert.ToInt32(codigoModelagem)).ToList();

            var codigoTecido = ddlTipoTecido.SelectedValue;
            if (codigoTecido != null && codigoTecido != "" && codigoTecido != "0")
                produtos = produtos.Where(p => p.ECOM_TIPO_TECIDO == Convert.ToInt32(codigoTecido)).ToList();

            var codigoManga = ddlTipoManga.SelectedValue;
            if (codigoManga != null && codigoManga != "" && codigoManga != "0")
                produtos = produtos.Where(p => p.ECOM_TIPO_MANGA == Convert.ToInt32(codigoManga)).ToList();

            var codigoGola = ddlTipoGola.SelectedValue;
            if (codigoGola != null && codigoGola != "" && codigoGola != "0")
                produtos = produtos.Where(p => p.ECOM_TIPO_GOLA == Convert.ToInt32(codigoGola)).ToList();

            var codigoComprimento = ddlTipoComprimento.SelectedValue;
            if (codigoComprimento != null && codigoComprimento != "" && codigoComprimento != "0")
                produtos = produtos.Where(p => p.ECOM_TIPO_COMPRIMENTO == Convert.ToInt32(codigoComprimento)).ToList();

            var codigoEstilo = ddlTipoEstilo.SelectedValue;
            if (codigoEstilo != null && codigoEstilo != "" && codigoEstilo != "0")
                produtos = produtos.Where(p => p.ECOM_TIPO_ESTILO == Convert.ToInt32(codigoEstilo)).ToList();


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

            Response.Redirect("~/mod_ecomv2/ecomv2_rel_config_rel_criar_random.aspx?ec=" + codigo, "_blank", "");

            CarregarJQuery();
        }

        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecao.SelectedValue.Trim();

            CarregarTecido(colecao);
            CarregarCorFornecedor(colecao);

            CarregarJQuery();
        }

    }
}

