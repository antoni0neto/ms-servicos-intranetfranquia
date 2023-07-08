using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;


namespace Relatorios
{
    public partial class desenv_cad_molde : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_producao/prod_menu_cad.aspx";

                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_MOLDE_PRODUTOResult> ObterMoldeProduto()
        {

            var moldes = prodController.ObterMoldeProduto();

            //Filtrar por Colecoes
            if (ddlColecoes.SelectedValue.Trim() != "0" && ddlColecoes.SelectedValue.Trim() != "")
                moldes = moldes.Where(p => p.COLECAO != null && p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim()).ToList();

            //Filtrar por griffe
            var origemCon = "";
            if (lstOrigem.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstOrigem.GetSelectedIndices())
                {
                    var itemList = lstOrigem.Items[v].Value.Trim() + ",";
                    origemCon = origemCon + itemList;
                }

                origemCon = origemCon + ",";
                origemCon = origemCon.Replace(",,", "");
            }
            if (origemCon != "" && origemCon != "0")
                moldes = moldes.Where(p => p.DESENV_PRODUTO_ORIGEM != null && origemCon.Contains(p.DESENV_PRODUTO_ORIGEM.ToString())).ToList();

            //Filtrar por grupo
            if (ddlGrupo.SelectedValue.Trim() != "")
                moldes = moldes.Where(p => p.GRUPO != null && p.GRUPO.Trim() == ddlGrupo.SelectedValue.Trim()).ToList();

            //Filtrar por Modelo
            if (txtModelo.Text.Trim() != "")
                moldes = moldes.Where(p => p.PRODUTO != null && p.PRODUTO.Trim().ToUpper().Contains(txtModelo.Text.Trim().ToUpper())).ToList();

            //Filtrar por nome
            if (txtNome.Text.Trim() != "")
            {
                var produtoFiltro = baseController.BuscaProdutosDescricao(txtNome.Text.ToUpper().Trim());
                moldes = moldes.Where(p => p.PRODUTO != null && produtoFiltro.Any(x => x.PRODUTO1.Trim() == p.PRODUTO.Trim())).ToList();
            }

            //Filtrar por Tecido
            if (ddlTecido.SelectedValue.Trim() != "")
                moldes = moldes.Where(p => p.TECIDO_POCKET != null && p.TECIDO_POCKET.Trim().Contains(ddlTecido.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por Cor Fornecedor
            if (ddlCorFornecedor.SelectedValue.Trim() != "")
                moldes = moldes.Where(p => p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim().Contains(ddlCorFornecedor.SelectedValue.Trim().ToUpper())).ToList();

            //Filtrar por griffe
            var griffeCon = "";
            if (lstGriffe.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGriffe.GetSelectedIndices())
                {
                    var itemList = lstGriffe.Items[v].Value.Trim() + ",";
                    griffeCon = griffeCon + itemList;
                }

                griffeCon = griffeCon + ",";
                griffeCon = griffeCon.Replace(",,", "");
            }
            if (griffeCon != "")
                moldes = moldes.Where(p => p.GRIFFE != null && griffeCon.Contains(p.GRIFFE.Trim())).ToList();

            return moldes;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvMolde.DataSource = ObterMoldeProduto();
                gvMolde.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void gvMolde_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MOLDE_PRODUTOResult molde = e.Row.DataItem as SP_OBTER_MOLDE_PRODUTOResult;

                    if (molde != null)
                    {

                        Image imgProduto = e.Row.FindControl("imgProduto") as Image;
                        imgProduto.ImageUrl = molde.FOTO;
                    }
                }
            }
        }


        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
                CarregarTecido(colecao, "");
                CarregarCorFornecedor(colecao, "", "");
            }
        }
        protected void ddlTecido_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCorFornecedor(ddlColecoes.SelectedValue, "", ddlTecido.SelectedValue);
        }
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecoes.DataSource = colecoes;
                ddlColecoes.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            var grupoProduto = (prodController.ObterGrupoProduto("01"));
            if (grupoProduto != null)
            {
                grupoProduto.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = grupoProduto;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            var origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
            origem = origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (origem != null)
            {
                origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                lstOrigem.DataSource = origem;
                lstOrigem.DataBind();

            }
        }
        private void CarregarTecido(string colecao, string origem)
        {
            var tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.TECIDO_POCKET != null && p.STATUS == 'A');

            if (origem != "")
                _tecido = _tecido.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == origem).ToList();

            var _tecidoAux = _tecido.Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();

            foreach (var item in _tecidoAux)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor(string colecao, string origem, string tecido)
        {
            var corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao(colecao).Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');

            if (origem != "" && origem != "0")
                _corFornecedor = _corFornecedor.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == origem).ToList();

            if (tecido != "")
                _corFornecedor = _corFornecedor.Where(p => p.TECIDO_POCKET != null && p.TECIDO_POCKET.Trim() == tecido.Trim()).ToList();

            var _corFornecedorAux = _corFornecedor.Select(s => s.FORNECEDOR_COR.Trim()).Distinct().ToList();

            foreach (var item in _corFornecedorAux)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR_COR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.FORNECEDOR_COR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
            ddlCorFornecedor.DataSource = corFornecedorFiltro;
            ddlCorFornecedor.DataBind();
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (baseController.BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                lstGriffe.DataSource = griffe;
                lstGriffe.DataBind();
            }
        }
        #endregion


    }
}
