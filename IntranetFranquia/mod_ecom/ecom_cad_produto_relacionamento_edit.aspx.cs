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
using Relatorios.mod_ecom.mag;


namespace Relatorios
{
    public partial class ecom_cad_produto_relacionamento_edit : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        Magento mag = new Magento();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string codigoEcomProduto = "0";
                if (Request.QueryString["codp"] == null || Request.QueryString["codp"] == "" ||
                    Session["USUARIO"] == null)
                    Response.Redirect("ecom_menu.aspx");

                codigoEcomProduto = Request.QueryString["codp"].ToString();

                var ecomProduto = eController.ObterMagentoProduto(Convert.ToInt32(codigoEcomProduto));

                if (ecomProduto != null)
                {
                    Page.Title = ecomProduto.NOME + " - Produto Relacionado";
                    labProduto.Text = ecomProduto.PRODUTO + " - " + ecomProduto.NOME;
                    imgProduto.ImageUrl = ecomProduto.FOTO_FRENTE_CAB;

                    hidCodEcomProduto.Value = ecomProduto.CODIGO.ToString();
                    hidGriffe.Value = ecomProduto.GRIFFE.Trim();
                    hidColecao.Value = ecomProduto.COLECAO.Trim();

                    ddlLook.Visible = false;
                    var ecomProdutoRelLook = eController.ObterProdutoRelacionadoLook(ecomProduto.PRODUTO, ecomProduto.COR);
                    if (ecomProdutoRelLook != null)
                    {
                        if (ecomProdutoRelLook.LOOK_CAD == true)
                        {
                            var prodRelLook = eController.ObterMagentoProdutoConfig(ecomProdutoRelLook.PRODUTO, ecomProdutoRelLook.COR);
                            if (prodRelLook != null)
                            {
                                labProdutoLook.Text = prodRelLook.PRODUTO + " - " + prodRelLook.NOME;
                                imgProdutoLook.ImageUrl = prodRelLook.FOTO_FRENTE_CAB;

                                hidCodEcomProdutoLook.Value = prodRelLook.CODIGO.ToString();

                                ddlLook.Visible = true;

                                //Preencher produto LOOK
                                VerificarProdutoLookRelacionado(ecomProduto.CODIGO, prodRelLook.CODIGO);
                            }
                        }
                    }

                    CarregarRelacionados();

                    //Carregar por tecido
                    var desenvProduto = desenvController.ObterProduto(ecomProduto.COLECAO, ecomProduto.PRODUTO, ecomProduto.COR);
                    if (desenvProduto != null)
                    {
                        var produtoGeral = eController.ObterProdutoRelacionadoDET(Convert.ToInt32(hidCodEcomProduto.Value), ecomProduto.COLECAO, "", 0, "", "", desenvProduto.TECIDO_POCKET.Trim(), desenvProduto.FORNECEDOR_COR.Trim(), ecomProduto.GRIFFE, "");
                        CarregarTecidoCor(produtoGeral, ecomProduto.PRODUTO, ecomProduto.COR);
                    }

                }

                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarCorLinx();
                CarregarGrupoMagento();

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btBuscarCaract.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscarCaract, null) + ";");

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
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecao.SelectedValue.Trim();

            CarregarTecido(colecao);
            CarregarCorFornecedor(colecao);
            CarregarJQuery();
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoLinx.DataSource = _grupo;
                ddlGrupoLinx.DataBind();

                ddlGrupoProdutoFiltroCaract.DataSource = _grupo;
                ddlGrupoProdutoFiltroCaract.DataBind();

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
        private void CarregarCorLinx()
        {
            var corLinx = prodController.ObterCoresBasicas();
            if (corLinx != null)
            {
                corLinx.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
                ddlCorLinx.DataSource = corLinx;
                ddlCorLinx.DataBind();
            }
        }
        private void CarregarGrupoMagento()
        {
            var grupoMagento = eController.ObterMagentoGrupoProduto();
            if (grupoMagento != null)
            {
                grupoMagento = grupoMagento.OrderBy(p => p.GRUPO).ToList();
                grupoMagento.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "" });
                ddlGrupoMagento.DataSource = grupoMagento;
                ddlGrupoMagento.DataBind();
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

        private void CarregarJQuery()
        {
            //GRID Produtos Geral
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //tecido Cor
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionTC').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            if (gvCaracteristicas.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionCA').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionCA').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

        }

        private void CarregarTipoModelagem(string griffe, string grupoProduto)
        {
            ddlTipoModelagem.Enabled = true;
            var tipoModelagem = eController.ObterMagentoTipoModelagem(griffe, grupoProduto);
            if (tipoModelagem != null && tipoModelagem.Count() > 0)
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = 0, TIPO_MODELAGEM = "Selecione" });
            }
            else
            {
                tipoModelagem.Insert(0, new ECOM_TIPO_MODELAGEM { CODIGO = -1, TIPO_MODELAGEM = "-" });
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
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = 0, TIPO_TECIDO = "Selecione" });
            }
            else
            {
                tipoTecido.Insert(0, new ECOM_TIPO_TECIDO { CODIGO = -1, TIPO_TECIDO = "-" });
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
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = 0, TIPO_MANGA = "Selecione" });
            }
            else
            {
                tipoManga.Insert(0, new ECOM_TIPO_MANGA { CODIGO = -1, TIPO_MANGA = "-" });
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
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = 0, TIPO_GOLA = "Selecione" });
            }
            else
            {
                tipoGola.Insert(0, new ECOM_TIPO_GOLA { CODIGO = -1, TIPO_GOLA = "-" });
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
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = 0, TIPO_COMPRIMENTO = "Selecione" });
            }
            else
            {
                tipoComprimento.Insert(0, new ECOM_TIPO_COMPRIMENTO { CODIGO = -1, TIPO_COMPRIMENTO = "-" });
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
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = 0, TIPO_ESTILO = "Selecione" });
            }
            else
            {
                tipoEstilo.Insert(0, new ECOM_TIPO_ESTILO { CODIGO = -1, TIPO_ESTILO = "-" });
                ddlTipoEstilo.Enabled = false;
            }

            ddlTipoEstilo.DataSource = tipoEstilo;
            ddlTipoEstilo.DataBind();
        }
        #endregion

        private void CarregarRelacionados()
        {
            gvProdutoRelacionado.DataSource = ObterProdutoRelacionadoProduto();
            gvProdutoRelacionado.DataBind();
        }

        private void VerificarProdutoLookRelacionado(int codigoPai, int codigoRel)
        {
            //var proRel = eController.ObterProdutoRelacionado(codigoPai, codigoRel);
            //if (proRel != null)
            //{
            //    ddlLook.SelectedValue = proRel.TIPO;
            //    labProdutoLook.ForeColor = Color.Red;
            //    hidTipoLook.Value = ddlLook.SelectedValue;
            //}
        }

        private List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_PRODUTOResult> ObterProdutoRelacionadoProduto()
        {
            return eController.ObterProdutoRelacionadoProduto(Convert.ToInt32(hidCodEcomProduto.Value));
        }
        protected void gvProdutoRelacionado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_RELACIONADO_PRODUTOResult prodRel = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_RELACIONADO_PRODUTOResult;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = prodRel.FOTO_FRENTE_CAB;

                    Label _labTipo = e.Row.FindControl("labTipo") as Label;
                    if (prodRel.TIPO == "related")
                    {
                        _labTipo.Text = "Relacionado";
                        e.Row.BackColor = Color.Lavender;
                    }
                    else if (prodRel.TIPO == "up_sell")
                    {
                        e.Row.BackColor = Color.Beige;
                        _labTipo.Text = "2 Produtos";
                    }
                }
            }
        }
        protected void gvProdutoRelacionado_DataBound(object sender, EventArgs e)
        {

        }

        private List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_DETResult> ObterProdutoRelacionadoDET()
        {
            int? codEcomGrupoProduto = null;

            if (ddlGrupoMagento.SelectedValue != "")
                codEcomGrupoProduto = Convert.ToInt32(ddlGrupoMagento.SelectedValue);

            return eController.ObterProdutoRelacionadoDET(Convert.ToInt32(hidCodEcomProduto.Value), ddlColecao.SelectedValue, ddlGrupoLinx.SelectedValue, codEcomGrupoProduto, txtProduto.Text, ddlCorLinx.SelectedValue, ddlTecido.SelectedValue.Trim(), ddlCorFornecedor.SelectedValue.Trim(), ddlGriffe.SelectedValue, ddlCodCategoria.SelectedValue);
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //Apagar grid de caracteristicas
                gvCaracteristicas.DataSource = new List<PROD_REL3>();
                gvCaracteristicas.DataBind();

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção...";

                    CarregarJQuery();
                    return;
                }

                var relProduto = ObterProdutoRelacionadoDET();
                var relProduto3 = TransformarProdutoGrid(relProduto);

                gvProduto.DataSource = relProduto3;
                gvProduto.DataBind();

                CarregarJQuery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private List<PROD_REL3> TransformarProdutoGrid(List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_DETResult> relProduto)
        {
            var relProduto3 = new List<PROD_REL3>();
            var relProd3 = new PROD_REL3();
            int cont = 1;
            foreach (var rel in relProduto)
            {
                if (cont == 1)
                {
                    relProd3.CodigoProduto1 = rel.COD_ECOM_PRODUTO;
                    relProd3.ProdutoLinx1 = rel.PRODUTO.Trim();
                    relProd3.Titulo1 = rel.PRODUTO + " - " + rel.NOME_MAG;
                    relProd3.Grupo1 = rel.GRUPO_MAGENTO;
                    relProd3.Linha1 = rel.LINHA;
                    relProd3.Griffe1 = rel.GRIFFE;
                    relProd3.CodigoProdutoRel1 = rel.COD_ECOM_PRODUTO_REL;
                    relProd3.Foto1 = rel.FOTO_FRENTE_CAB;
                    relProd3.Tipo1 = rel.TIPO;

                }

                else if (cont == 2)
                {
                    relProd3.CodigoProduto2 = rel.COD_ECOM_PRODUTO;
                    relProd3.ProdutoLinx2 = rel.PRODUTO.Trim();
                    relProd3.Titulo2 = rel.PRODUTO + " - " + rel.NOME_MAG;
                    relProd3.Grupo2 = rel.GRUPO_MAGENTO;
                    relProd3.Linha2 = rel.LINHA;
                    relProd3.Griffe2 = rel.GRIFFE;
                    relProd3.CodigoProdutoRel2 = rel.COD_ECOM_PRODUTO_REL;
                    relProd3.Foto2 = rel.FOTO_FRENTE_CAB;
                    relProd3.Tipo2 = rel.TIPO;
                }

                else if (cont == 3)
                {
                    relProd3.CodigoProduto3 = rel.COD_ECOM_PRODUTO;
                    relProd3.ProdutoLinx3 = rel.PRODUTO.Trim();
                    relProd3.Titulo3 = rel.PRODUTO + " - " + rel.NOME_MAG;
                    relProd3.Grupo3 = rel.GRUPO_MAGENTO;
                    relProd3.Linha3 = rel.LINHA;
                    relProd3.Griffe3 = rel.GRIFFE;
                    relProd3.CodigoProdutoRel3 = rel.COD_ECOM_PRODUTO_REL;
                    relProd3.Foto3 = rel.FOTO_FRENTE_CAB;
                    relProd3.Tipo3 = rel.TIPO;

                    relProduto3.Add(relProd3);

                    cont = 0;
                    relProd3 = new PROD_REL3();
                }

                cont = cont + 1;
            }

            if ((cont == 1) || (cont == 2) || (cont == 3 && relProd3.ProdutoLinx3 == null))
                relProduto3.Add(relProd3);

            return relProduto3;
        }

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_REL3 rel3 = e.Row.DataItem as PROD_REL3;

                    /*Produto 1*/
                    HiddenField hidTipo1 = e.Row.FindControl("hidTipo1") as HiddenField;
                    hidTipo1.Value = rel3.Tipo1;

                    HiddenField hidCodEcom1 = e.Row.FindControl("hidCodEcom1") as HiddenField;
                    hidCodEcom1.Value = rel3.CodigoProduto1.ToString();

                    Label labProdutoTitulo1 = e.Row.FindControl("labProdutoTitulo1") as Label;
                    labProdutoTitulo1.Text = rel3.Titulo1;

                    Label labGrupo1 = e.Row.FindControl("labGrupo1") as Label;
                    labGrupo1.Text = rel3.Grupo1;

                    Label labCor1 = e.Row.FindControl("labLinha1") as Label;
                    labCor1.Text = rel3.Linha1;

                    Label labGriffe1 = e.Row.FindControl("labGriffe1") as Label;
                    labGriffe1.Text = rel3.Griffe1;

                    System.Web.UI.WebControls.Image imgFrenteCab1 = e.Row.FindControl("imgFrenteCab1") as System.Web.UI.WebControls.Image;
                    imgFrenteCab1.ImageUrl = rel3.Foto1;

                    DropDownList ddl1 = e.Row.FindControl("ddl1") as DropDownList;
                    ddl1.Visible = (rel3.CodigoProduto1 > 0);
                    /*******************************************************************************************************************************/

                    /*Produto 2*/
                    HiddenField hidTipo2 = e.Row.FindControl("hidTipo2") as HiddenField;
                    hidTipo2.Value = rel3.Tipo2;

                    HiddenField hidCodEcom2 = e.Row.FindControl("hidCodEcom2") as HiddenField;
                    hidCodEcom2.Value = rel3.CodigoProduto2.ToString();

                    Label labProdutoTitulo2 = e.Row.FindControl("labProdutoTitulo2") as Label;
                    labProdutoTitulo2.Text = rel3.Titulo2;

                    Label labGrupo2 = e.Row.FindControl("labGrupo2") as Label;
                    labGrupo2.Text = rel3.Grupo2;

                    Label labCor2 = e.Row.FindControl("labLinha2") as Label;
                    labCor2.Text = rel3.Linha2;

                    Label labGriffe2 = e.Row.FindControl("labGriffe2") as Label;
                    labGriffe2.Text = rel3.Griffe2;

                    System.Web.UI.WebControls.Image imgFrenteCab2 = e.Row.FindControl("imgFrenteCab2") as System.Web.UI.WebControls.Image;
                    imgFrenteCab2.ImageUrl = rel3.Foto2;

                    DropDownList ddl2 = e.Row.FindControl("ddl2") as DropDownList;
                    ddl2.Visible = (rel3.CodigoProduto2 > 0);
                    /*******************************************************************************************************************************/

                    /*Produto 3*/
                    HiddenField hidTipo3 = e.Row.FindControl("hidTipo3") as HiddenField;
                    hidTipo3.Value = rel3.Tipo3;

                    HiddenField hidCodEcom3 = e.Row.FindControl("hidCodEcom3") as HiddenField;
                    hidCodEcom3.Value = rel3.CodigoProduto3.ToString();

                    Label labProdutoTitulo3 = e.Row.FindControl("labProdutoTitulo3") as Label;
                    labProdutoTitulo3.Text = rel3.Titulo3;

                    Label labGrupo3 = e.Row.FindControl("labGrupo3") as Label;
                    labGrupo3.Text = rel3.Grupo3;

                    Label labCor3 = e.Row.FindControl("labLinha3") as Label;
                    labCor3.Text = rel3.Linha3;

                    Label labGriffe3 = e.Row.FindControl("labGriffe3") as Label;
                    labGriffe3.Text = rel3.Griffe3;

                    System.Web.UI.WebControls.Image imgFrenteCab3 = e.Row.FindControl("imgFrenteCab3") as System.Web.UI.WebControls.Image;
                    imgFrenteCab3.ImageUrl = rel3.Foto3;

                    DropDownList ddl3 = e.Row.FindControl("ddl3") as DropDownList;
                    ddl3.Visible = (rel3.CodigoProduto3 > 0);
                    /*******************************************************************************************************************************/
                    // CONTROLE DE MOMENTO
                    if (rel3.CodigoProdutoRel1 != null && rel3.CodigoProdutoRel1 != 0)
                    {
                        e.Row.Cells[0].BackColor = Color.WhiteSmoke;
                        ddl1.SelectedValue = rel3.Tipo1;
                        labProdutoTitulo1.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[0].BackColor = Color.White;
                        ddl1.SelectedValue = "";
                        labProdutoTitulo1.ForeColor = Color.Gray;
                    }

                    if (rel3.CodigoProdutoRel2 != null && rel3.CodigoProdutoRel2 != 0)
                    {
                        e.Row.Cells[1].BackColor = Color.WhiteSmoke;
                        ddl2.SelectedValue = rel3.Tipo2;
                        labProdutoTitulo2.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[1].BackColor = Color.White;
                        ddl2.SelectedValue = "";
                        labProdutoTitulo2.ForeColor = Color.Gray;
                    }

                    if (rel3.CodigoProdutoRel3 != null && rel3.CodigoProdutoRel3 != 0)
                    {
                        e.Row.Cells[2].BackColor = Color.WhiteSmoke;
                        ddl3.SelectedValue = rel3.Tipo3;
                        labProdutoTitulo3.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[2].BackColor = Color.White;
                        ddl3.SelectedValue = rel3.Tipo3;
                        labProdutoTitulo3.ForeColor = Color.Gray;
                    }

                    //CONTROLE DE VISUALIZACAO COLUNA
                    Panel pnl1 = e.Row.FindControl("pnl1") as Panel;
                    Panel pnl2 = e.Row.FindControl("pnl2") as Panel;
                    Panel pnl3 = e.Row.FindControl("pnl3") as Panel;
                    pnl1.Visible = false;
                    if (rel3.ProdutoLinx1 != "" && rel3.ProdutoLinx1 != null)
                        pnl1.Visible = true;
                    pnl2.Visible = false;
                    if (rel3.ProdutoLinx2 != "" && rel3.ProdutoLinx2 != null)
                        pnl2.Visible = true;
                    pnl3.Visible = false;
                    if (rel3.ProdutoLinx3 != "" && rel3.ProdutoLinx3 != null)
                        pnl3.Visible = true;

                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProduto.FooterRow;
            if (_footer != null)
            {
            }
        }

        #endregion

        protected void ddlLook_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codigoEcom = Convert.ToInt32(hidCodEcomProduto.Value);
            int codigoProdutoRel = Convert.ToInt32(hidCodEcomProdutoLook.Value);

            if (ddlLook.SelectedValue != "")
            {
                InserirProdutoRel(codigoEcom, codigoProdutoRel, hidTipoLook.Value, ddlLook.SelectedValue);
                labProdutoLook.ForeColor = Color.Red;
            }
            else
            {
                ExcluirProdutoRel(codigoEcom, codigoProdutoRel, hidTipoLook.Value);
                labProdutoLook.ForeColor = Color.Gray;
            }

            hidTipoLook.Value = ddlLook.SelectedValue;

            CarregarRelacionados();
            CarregarJQuery();
        }

        protected void ddl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl1 = (DropDownList)sender;
            if (ddl1 != null)
            {
                GridViewRow row = (GridViewRow)ddl1.NamingContainer;
                HiddenField hidCodEcom1 = row.FindControl("hidCodEcom1") as HiddenField;
                HiddenField hidTipo1 = row.FindControl("hidTipo1") as HiddenField;
                Label labProdutoTitulo1 = row.FindControl("labProdutoTitulo1") as Label;

                int codigoEcom = Convert.ToInt32(hidCodEcomProduto.Value);
                int codigoProdutoRel = Convert.ToInt32(hidCodEcom1.Value);


                if (ddl1.SelectedValue != "")
                {
                    InserirProdutoRel(codigoEcom, codigoProdutoRel, hidTipo1.Value, ddl1.SelectedValue);
                    row.Cells[0].BackColor = Color.WhiteSmoke;
                    labProdutoTitulo1.ForeColor = Color.Red;
                }
                else
                {
                    ExcluirProdutoRel(codigoEcom, codigoProdutoRel, hidTipo1.Value);
                    row.Cells[0].BackColor = Color.White;
                    labProdutoTitulo1.ForeColor = Color.Gray;
                }

                hidTipo1.Value = ddl1.SelectedValue;
                CarregarRelacionados();
            }

            CarregarJQuery();
        }
        protected void ddl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl2 = (DropDownList)sender;
            if (ddl2 != null)
            {
                GridViewRow row = (GridViewRow)ddl2.NamingContainer;
                HiddenField hidCodEcom2 = row.FindControl("hidCodEcom2") as HiddenField;
                HiddenField hidTipo2 = row.FindControl("hidTipo2") as HiddenField;
                Label labProdutoTitulo2 = row.FindControl("labProdutoTitulo2") as Label;

                int codigoEcom = Convert.ToInt32(hidCodEcomProduto.Value);
                int codigoProdutoRel = Convert.ToInt32(hidCodEcom2.Value);

                if (ddl2.SelectedValue != "")
                {
                    InserirProdutoRel(codigoEcom, codigoProdutoRel, hidTipo2.Value, ddl2.SelectedValue);
                    row.Cells[0].BackColor = Color.WhiteSmoke;
                    labProdutoTitulo2.ForeColor = Color.Red;
                }
                else
                {
                    ExcluirProdutoRel(codigoEcom, codigoProdutoRel, hidTipo2.Value);
                    row.Cells[0].BackColor = Color.White;
                    labProdutoTitulo2.ForeColor = Color.Gray;
                }


                hidTipo2.Value = ddl2.SelectedValue;
                CarregarRelacionados();
            }

            CarregarJQuery();
        }
        protected void ddl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl3 = (DropDownList)sender;
            if (ddl3 != null)
            {
                GridViewRow row = (GridViewRow)ddl3.NamingContainer;
                HiddenField hidCodEcom3 = row.FindControl("hidCodEcom3") as HiddenField;
                HiddenField hidTipo3 = row.FindControl("hidTipo3") as HiddenField;
                Label labProdutoTitulo3 = row.FindControl("labProdutoTitulo3") as Label;

                int codigoEcom = Convert.ToInt32(hidCodEcomProduto.Value);
                int codigoProdutoRel = Convert.ToInt32(hidCodEcom3.Value);

                if (ddl3.SelectedValue != "")
                {
                    InserirProdutoRel(codigoEcom, codigoProdutoRel, hidTipo3.Value, ddl3.SelectedValue);
                    row.Cells[0].BackColor = Color.WhiteSmoke;
                    labProdutoTitulo3.ForeColor = Color.Red;
                }
                else
                {
                    ExcluirProdutoRel(codigoEcom, codigoProdutoRel, hidTipo3.Value);
                    row.Cells[0].BackColor = Color.White;
                    labProdutoTitulo3.ForeColor = Color.Gray;
                }

                hidTipo3.Value = ddl3.SelectedValue;
                CarregarRelacionados();
            }

            CarregarJQuery();
        }

        private bool InserirProdutoRel(int codigoPai, int codigoRel, string tipoAnterior, string tipoNovo)
        {

            //var tipoProdutoSelect = Magento.PRODUTORELACIONADO.RELACIONADO;
            //if (tipoNovo == "related")
            //    tipoProdutoSelect = Magento.PRODUTORELACIONADO.RELACIONADO;
            //else if (tipoNovo == "up_sell")
            //    tipoProdutoSelect = Magento.PRODUTORELACIONADO.AGREGADO;

            //var tipoProdutoAnterior = Magento.PRODUTORELACIONADO.RELACIONADO;
            //if (tipoAnterior == "related")
            //    tipoProdutoAnterior = Magento.PRODUTORELACIONADO.RELACIONADO;
            //else if (tipoAnterior == "up_sell")
            //    tipoProdutoAnterior = Magento.PRODUTORELACIONADO.AGREGADO;


            ////Obter produtos
            //var a = eController.ObterMagentoProduto(codigoPai);
            //var b = eController.ObterMagentoProduto(codigoRel);

            ////INSERIR
            //if (tipoNovo != "")
            //{
            //    //var prExiste = eController.ObterProdutoRelacionado(codigoPai, codigoRel, tipoNovo);
            //    //if (prExiste == null)
            //    //{
            //    //    mag.InserirProdutoRelacionado(a.ID_PRODUTO_MAG.ToString(), b.ID_PRODUTO_MAG.ToString(), tipoProdutoSelect);

            //    //    var pR = new ECOM_PRODUTO_RELACIONADO();
            //    //    pR.ECOM_PRODUTO = codigoPai;
            //    //    pR.ECOM_PRODUTO_REL = codigoRel;
            //    //    pR.TIPO = tipoNovo;
            //    //    eController.InserirProdutoRelacionado(pR);
            //    //}

            //    //if (tipoAnterior != "")
            //    //{
            //    //    var prAntExiste = eController.ObterProdutoRelacionado(codigoPai, codigoRel, tipoAnterior);
            //    //    if (prAntExiste != null)
            //    //    {
            //    //        mag.RemoverProdutoRelacionado(a.ID_PRODUTO_MAG.ToString(), b.ID_PRODUTO_MAG.ToString(), tipoProdutoAnterior);
            //    //        eController.ExcluirProdutoRelacionado(prAntExiste.CODIGO);
            //    //    }
            //    //}

            //    return true;
            //}

            return false;

        }
        private bool ExcluirProdutoRel(int codigoPai, int codigoRel, string tipoAnterior)
        {

            //var tipoProdutoAnterior = Magento.PRODUTORELACIONADO.RELACIONADO;
            //if (tipoAnterior == "related")
            //    tipoProdutoAnterior = Magento.PRODUTORELACIONADO.RELACIONADO;
            //else if (tipoAnterior == "up_sell")
            //    tipoProdutoAnterior = Magento.PRODUTORELACIONADO.AGREGADO;

            ////Obter produtos
            //var a = eController.ObterMagentoProduto(codigoPai);
            //var b = eController.ObterMagentoProduto(codigoRel);

            //mag.RemoverProdutoRelacionado(a.ID_PRODUTO_MAG.ToString(), b.ID_PRODUTO_MAG.ToString(), tipoProdutoAnterior);

            //var pR = eController.ObterProdutoRelacionado(codigoPai, codigoRel, tipoAnterior);
            //eController.ExcluirProdutoRelacionado(pR.CODIGO);

            return true;
        }

        private void CarregarTecidoCor(List<SP_OBTER_ECOM_PRODUTO_RELACIONADO_DETResult> produtoRel, string produto, string cor)
        {
            //remove o proprio produto
            produtoRel = produtoRel.Where(p => p.PRODUTO != produto).ToList();

            var produtoRel3 = TransformarProdutoGrid(produtoRel);
            gvTecidoCor.DataSource = produtoRel3;
            gvTecidoCor.DataBind();
        }

        protected void ddlGrupoProdutoFiltroCaract_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGrupoProdutoFiltroCaract.SelectedValue != "")
            {
                var grupoProduto = ddlGrupoProdutoFiltroCaract.SelectedValue;

                CarregarTipoModelagem(hidGriffe.Value, grupoProduto);
                CarregarTipoTecido(hidGriffe.Value, grupoProduto);
                CarregarTipoManga(hidGriffe.Value, grupoProduto);
                CarregarTipoGola(hidGriffe.Value, grupoProduto);
                CarregarTipoComprimento(hidGriffe.Value, grupoProduto);
                CarregarTipoEstilo(hidGriffe.Value, grupoProduto);
            }

            CarregarJQuery();
        }

        protected void btBuscarCaract_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlTipoModelagem.SelectedValue) > 0 ||
                Convert.ToInt32(ddlTipoTecido.SelectedValue) > 0 ||
                Convert.ToInt32(ddlTipoManga.SelectedValue) > 0 ||
                Convert.ToInt32(ddlTipoGola.SelectedValue) > 0 ||
                Convert.ToInt32(ddlTipoComprimento.SelectedValue) > 0 ||
                Convert.ToInt32(ddlTipoEstilo.SelectedValue) > 0
            )
            {
                //buscar todos produtos possiveis para relacionar
                var produtoGeral = eController.ObterProdutoRelacionadoDET(Convert.ToInt32(hidCodEcomProduto.Value), hidColecao.Value, ddlGrupoProdutoFiltroCaract.SelectedValue.Trim(), 0, "", "", "", "", hidGriffe.Value, "");

                if (Convert.ToInt32(ddlTipoModelagem.SelectedValue) > 0)
                    produtoGeral = produtoGeral.Where(p => p.ECOM_TIPO_MODELAGEM == Convert.ToInt32(ddlTipoModelagem.SelectedValue)).ToList();

                if (Convert.ToInt32(ddlTipoTecido.SelectedValue) > 0)
                    produtoGeral = produtoGeral.Where(p => p.ECOM_TIPO_TECIDO == Convert.ToInt32(ddlTipoTecido.SelectedValue)).ToList();

                if (Convert.ToInt32(ddlTipoManga.SelectedValue) > 0)
                    produtoGeral = produtoGeral.Where(p => p.ECOM_TIPO_MANGA == Convert.ToInt32(ddlTipoManga.SelectedValue)).ToList();

                if (Convert.ToInt32(ddlTipoGola.SelectedValue) > 0)
                    produtoGeral = produtoGeral.Where(p => p.ECOM_TIPO_GOLA == Convert.ToInt32(ddlTipoGola.SelectedValue)).ToList();

                if (Convert.ToInt32(ddlTipoComprimento.SelectedValue) > 0)
                    produtoGeral = produtoGeral.Where(p => p.ECOM_TIPO_COMPRIMENTO == Convert.ToInt32(ddlTipoComprimento.SelectedValue)).ToList();

                if (Convert.ToInt32(ddlTipoEstilo.SelectedValue) > 0)
                    produtoGeral = produtoGeral.Where(p => p.ECOM_TIPO_ESTILO == Convert.ToInt32(ddlTipoEstilo.SelectedValue)).ToList();

                var produtoRel3 = TransformarProdutoGrid(produtoGeral);
                gvCaracteristicas.DataSource = produtoRel3;
                gvCaracteristicas.DataBind();
            }
            else
            {
                gvCaracteristicas.DataSource = new List<PROD_REL3>();
                gvCaracteristicas.DataBind();
            }

            CarregarJQuery();
        }

    }
}

public class PROD_REL3
{
    public int CodigoProduto1 { get; set; }
    public string ProdutoLinx1 { get; set; }
    public string Titulo1 { get; set; }
    public string Grupo1 { get; set; }
    public string Linha1 { get; set; }
    public string Griffe1 { get; set; }
    public int? CodigoProdutoRel1 { get; set; }
    public string Foto1 { get; set; }
    public string Tipo1 { get; set; }

    public int CodigoProduto2 { get; set; }
    public string ProdutoLinx2 { get; set; }
    public string Titulo2 { get; set; }
    public string Grupo2 { get; set; }
    public string Linha2 { get; set; }
    public string Griffe2 { get; set; }
    public int? CodigoProdutoRel2 { get; set; }
    public string Foto2 { get; set; }
    public string Tipo2 { get; set; }

    public int CodigoProduto3 { get; set; }
    public string ProdutoLinx3 { get; set; }
    public string Titulo3 { get; set; }
    public string Grupo3 { get; set; }
    public string Linha3 { get; set; }
    public string Griffe3 { get; set; }
    public int? CodigoProdutoRel3 { get; set; }
    public string Foto3 { get; set; }
    public string Tipo3 { get; set; }
}

