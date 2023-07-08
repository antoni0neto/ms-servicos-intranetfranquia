using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_tripa_view : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> gPrePedidoLib = new List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult>();
        List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> gPrePedidoLibFiltro = new List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult>();

        enum ORIGEM_PREPEDIDO
        {
            RECARREGARFILTRO = 1,
            MANTERFILTRO = 2
        }

        decimal gQtdeCompra = 0;
        decimal gQtdeConsumo = 0;
        decimal gQtdeSobra = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                CarregarColecoes();
                CarregarGriffe();
                CarregarCorLinx();
                CarregarGrupoPedido();
                CarregarUnidadeLinx();
                CarregarFornecedores();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btIncluirPrePedido.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btIncluirPrePedido, null) + ";");

        }

        #region "DADOS INI"
        private void CarregarColecoes()
        {
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(ddlColecao.SelectedValue);
                }
            }
        }
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarOrigem(ddlColecao.SelectedValue);
        }
        private void CarregarOrigem(string colecao)
        {
            var origem = desenvController.ObterProdutoOrigem(colecao);

            if (origem != null)
            {
                origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = origem;
                ddlOrigem.DataBind();

                ddlOrigemInc.DataSource = origem;
                ddlOrigemInc.DataBind();


                if (origem.Count == 2)
                {
                    ddlOrigem.SelectedValue = origem[1].CODIGO.ToString();
                    ddlOrigemInc.SelectedValue = origem[1].CODIGO.ToString();
                }

            }
        }

        private void CarregarGriffe()
        {
            var griffe = RetornarGriffe();

            if (griffe != null)
            {
                griffe.RemoveAt(0);
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "Selecione" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarCorLinx()
        {
            var cores = RetornarCorLinx();

            cores.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "Selecione" });
            ddlCor.DataSource = cores;
            ddlCor.DataBind();
        }
        private void CarregarGrupoPedido()
        {
            var grupoPedido = desenvController.ObterGrupoProdutoPrePedido();

            grupoPedido.Insert(0, new DESENV_GRUPO_PRODUTO { GRUPO = "Selecione" });
            ddlGrupoPedido.DataSource = grupoPedido;
            ddlGrupoPedido.DataBind();
        }
        private void CarregarUnidadeLinx()
        {
            var unidade = desenvController.ObterUnidadeMedidaLinx().Where(p => p.UNIDADE1.Trim() == "KG" || p.UNIDADE1.Trim() == "UN" || p.UNIDADE1.Trim() == "MT").OrderBy(o => o.UNIDADE1.Trim()).ToList();
            if (unidade != null)
            {
                unidade.Insert(0, new UNIDADE { UNIDADE1 = "", DESC_UNIDADE = "Selecione" });
                ddlUnidadeMedida.DataSource = unidade;
                ddlUnidadeMedida.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            var fornecedores = prodController.ObterFornecedor().ToList();
            fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            fornecedores = fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (fornecedores != null)
            {
                ddlFornecedor.DataSource = fornecedores;
                ddlFornecedor.DataBind();
            }
        }

        private List<PRODUTOS_GRIFFE> RetornarGriffe()
        {
            var griffes = baseController.BuscaGriffes();
            griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
            return griffes;
        }
        private List<CORES_BASICA> RetornarCorLinx()
        {
            var coresBasicas = prodController.ObterCoresBasicas();
            coresBasicas = coresBasicas.GroupBy(p => new { COR = p.COR.Trim(), DESC_COR = p.DESC_COR.Trim() }).Select(x => new CORES_BASICA { COR = x.Key.COR.Trim(), DESC_COR = x.Key.DESC_COR.Trim() }).ToList();
            return coresBasicas;
        }

        #endregion

        #region "FILTROS"
        private void CarregarFiltros(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            CarregarFiltroOrigem(prePedidoLib);
            CarregarFiltroFabricante(prePedidoLib);
            CarregarFiltroProduto(prePedidoLib);
            CarregarFiltroTecido(prePedidoLib);
            CarregarFiltroCorFornecedor(prePedidoLib);
            CarregarFiltroCorLinx(prePedidoLib);
            CarregarFiltroGrupoProduto(prePedidoLib);
            CarregarFiltroGriffe(prePedidoLib);
            CarregarFiltroLibAtacado(prePedidoLib);
            CarregarFiltroQtdeAtacado(prePedidoLib);
            CarregarFiltroLibVarejo(prePedidoLib);
            CarregarFiltroSigned(prePedidoLib);
        }

        private void CarregarFiltroOrigem(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.ORIGEM).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            list.Add(new ListItem { Value = "VAZIO", Text = "VAZIO" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroOrigem.DataSource = list.Distinct().OrderBy(p => p.Text);
            ddlFiltroOrigem.DataBind();
        }
        private void CarregarFiltroFabricante(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.FORNECEDOR.Trim()).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            list.Add(new ListItem { Value = "VAZIO", Text = "VAZIO" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroFabricante.DataSource = list.Distinct().OrderBy(p => p.Text);
            ddlFiltroFabricante.DataBind();
        }
        private void CarregarFiltroProduto(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.PRODUTO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            list.Add(new ListItem { Value = "VAZIO", Text = "VAZIO" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroProduto.DataSource = list.Distinct().OrderBy(p => p.Text);
            ddlFiltroProduto.DataBind();
        }
        private void CarregarFiltroLibAtacado(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.LIBERACAO_ATACADO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroLibAtacado.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroLibAtacado.DataBind();
        }
        private void CarregarFiltroQtdeAtacado(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.QTDE_ATACADO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            ddlFiltroQtdeAtacado.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroQtdeAtacado.DataBind();
        }
        private void CarregarFiltroLibVarejo(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.LIBERACAO_VAREJO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroLibVarejo.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroLibVarejo.DataBind();
        }
        private void CarregarFiltroSigned(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.SIGNED_NOME).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            list.Add(new ListItem { Value = "VAZIO", Text = "VAZIO" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroSigned.DataSource = list.Distinct().OrderBy(p => p.Text);
            ddlFiltroSigned.DataBind();
        }




        private void CarregarFiltroTecido(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.TECIDO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroTecido.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroTecido.DataBind();
        }
        private void CarregarFiltroCorFornecedor(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {

            if (ddlFiltroTecido.SelectedValue != "")
                prePedidoLib = prePedidoLib.Where(p => p.TECIDO.Trim().Contains(ddlFiltroTecido.SelectedValue.Trim())).ToList();

            var y = prePedidoLib.Select(p => p.COR_FORNECEDOR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            //pegar valor selecionado
            var corFornecedorSelected = ddlFiltroCorFornecedor.SelectedValue;

            ddlFiltroCorFornecedor.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroCorFornecedor.DataBind();

            ddlFiltroCorFornecedor.SelectedValue = corFornecedorSelected;
        }
        private void CarregarFiltroCorLinx(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            if (ddlFiltroTecido.SelectedValue != "")
                prePedidoLib = prePedidoLib.Where(p => p.TECIDO.Trim().Contains(ddlFiltroTecido.SelectedValue.Trim())).ToList();

            var y = prePedidoLib.Select(p => p.DESC_COR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            //pegar valor selecionado
            var corSelected = ddlFiltroCorLinx.SelectedValue;

            ddlFiltroCorLinx.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroCorLinx.DataBind();

            ddlFiltroCorLinx.SelectedValue = corSelected;
        }
        private void CarregarFiltroGrupoProduto(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.GRUPO_PRODUTO.Trim()).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroGrupoProduto.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroGrupoProduto.DataBind();
        }
        private void CarregarFiltroGriffe(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedidoLib)
        {
            var y = prePedidoLib.Select(p => p.GRIFFE.Trim()).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroGriffe.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroGriffe.DataBind();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList ddl = (DropDownList)sender;
            //if (ddl.SelectedValue.Trim() != "")
            CarregarPrePedido(ORIGEM_PREPEDIDO.MANTERFILTRO);

        }

        private void LimparFiltros()
        {
            ddlFiltroOrigem.SelectedValue = "";
            ddlFiltroFabricante.SelectedValue = "";
            ddlFiltroGriffe.SelectedValue = "";
            ddlFiltroGrupoProduto.SelectedValue = "";
            ddlFiltroProduto.SelectedValue = "";
            ddlFiltroTecido.SelectedValue = "";
            ddlFiltroCorLinx.SelectedValue = "";
            ddlFiltroCorFornecedor.SelectedValue = "";
            ddlFiltroLibVarejo.SelectedValue = "";
            ddlFiltroLibAtacado.SelectedValue = "";
            ddlFiltroQtdeAtacado.SelectedValue = "";
            ddlFiltroSigned.SelectedValue = "";
        }

        #endregion

        #region "INCLUSAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labOrigemInc.ForeColor = _OK;
            if (ddlOrigemInc.SelectedValue == "0")
            {
                labOrigemInc.ForeColor = _notOK;
                retorno = false;
            }

            labTecidoFiltro.ForeColor = _OK;
            if (txtTecidoFiltro.Text.Trim() == "")
            {
                labTecidoFiltro.ForeColor = _notOK;
                retorno = false;
            }

            labTecido.ForeColor = _OK;
            if (ddlTecido.SelectedValue == "-1")
            {
                labTecido.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue == "")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedor.ForeColor = _OK;
            if (txtCorFornecedor.Text.Trim() == "")
            {
                labCorFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue == "")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labGrupoPedido.ForeColor = _OK;
            if (ddlGrupoPedido.SelectedValue == "Selecione")
            {
                labGrupoPedido.ForeColor = _notOK;
                retorno = false;
            }

            labGriffe.ForeColor = _OK;
            if (ddlGriffe.SelectedValue == "Selecione")
            {
                labGriffe.ForeColor = _notOK;
                retorno = false;
            }

            labUnidadeMedida.ForeColor = _OK;
            if (ddlUnidadeMedida.SelectedValue == "")
            {
                labUnidadeMedida.ForeColor = _notOK;
                retorno = false;
            }

            labConsumo.ForeColor = _OK;
            if (txtConsumo.Text.Trim() == "")
            {
                labConsumo.ForeColor = _notOK;
                retorno = false;
            }

            //labPrecoTecido.ForeColor = _OK;
            //if (txtPrecoTecido.Text.Trim() == "")
            //{
            //    labPrecoTecido.ForeColor = _notOK;
            //    retorno = false;
            //}

            labQtdeAtacado.ForeColor = _OK;
            if (txtQtdeAtacado.Text.Trim() == "")
            {
                labQtdeAtacado.ForeColor = _notOK;
                retorno = false;
            }

            labQtdeVarejo.ForeColor = _OK;
            if (txtQtdeVarejo.Text.Trim() == "")
            {
                labQtdeVarejo.ForeColor = _notOK;
                retorno = false;
            }

            labPrecoVenda.ForeColor = _OK;
            if (txtPrecoVenda.Text.Trim() == "")
            {
                labPrecoVenda.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void LimparCampos()
        {

            ddlGrupoPedido.SelectedValue = "Selecione";

            txtConsumo.Text = "";
            //txtPrecoTecido.Text = "";
            txtQtdeAtacado.Text = "";
            txtQtdeVarejo.Text = "";
            txtPrecoVenda.Text = "";
            txtConsumoTotal.Text = "";
            txtTotalAtacado.Text = "";
            txtTotalVarejo.Text = "";

            labErro.Text = "";
            labErroInclusao.Text = "";

        }
        private void LimparTodosCampos()
        {

            ddlGrupoPedido.SelectedValue = "Selecione";

            txtConsumo.Text = "";
            txtPrecoTecido.Text = "";
            txtQtdeAtacado.Text = "";
            txtQtdeVarejo.Text = "";
            txtPrecoVenda.Text = "";
            txtConsumoTotal.Text = "";
            txtTotalAtacado.Text = "";
            txtTotalVarejo.Text = "";
            txtSignedNome.Text = "";
            txtRefModelagem.Text = "";

            txtTecidoFiltro.Text = "";
            txtCorFornecedor.Text = "";
            ddlFornecedor.SelectedValue = "";
            ddlCor.SelectedValue = "";
            ddlGriffe.SelectedValue = "Selecione";
            ddlUnidadeMedida.SelectedValue = "";
            if (ddlTecido.Enabled)
                ddlTecido.SelectedValue = "-1";
            else
                ddlTecido.SelectedValue = "";

            txtCorFornecedor.Enabled = true;
            ddlCor.Enabled = true;
            ddlUnidadeMedida.Enabled = true;

            imgTecido.ImageUrl = "";

            LimparCampos();

        }
        protected void btLimparTodos_Click(object sender, EventArgs e)
        {
            LimparTodosCampos();
        }
        protected void btnImgMostrarInclusao_Click(object sender, ImageClickEventArgs e)
        {
            pnlInc.Visible = true;
            btnImgEsconderInc.Visible = true;
            btnImgMostrarInc.Visible = false;
            LimparCampos();
        }
        protected void btnImgEsconderInc_Click(object sender, ImageClickEventArgs e)
        {
            pnlInc.Visible = false;
            btnImgEsconderInc.Visible = false;
            btnImgMostrarInc.Visible = true;
            LimparCampos();
        }

        protected void txtTecidoFiltro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroInclusao.Text = "";

                string pesquisa = txtTecidoFiltro.Text.Trim();

                var tecido = desenvController.ObterMaterialPesquisa(pesquisa);

                if (tecido != null && tecido.Count() > 0)
                {
                    ddlTecido.Enabled = true;
                    tecido.Insert(0, new SP_OBTER_MATERIALResult { MATERIAL = "-1", DESCRICAO_MATERIAL = "Selecione" });
                    tecido.Add(new SP_OBTER_MATERIALResult { MATERIAL = "00.00.0000", DESCRICAO_MATERIAL = "Não Encontrado" });
                }
                else
                {
                    ddlTecido.Enabled = false;
                    tecido.Insert(0, new SP_OBTER_MATERIALResult { MATERIAL = "", DESCRICAO_MATERIAL = "" });
                }

                txtCorFornecedor.Enabled = true;
                txtCorFornecedor.Text = "";
                ddlCor.Enabled = true;
                ddlCor.SelectedValue = "";
                ddlUnidadeMedida.Enabled = true;
                ddlUnidadeMedida.SelectedValue = "";

                imgTecido.ImageUrl = "";

                ddlTecido.DataSource = tecido;
                ddlTecido.DataBind();

            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }
        }
        protected void ddlTecido_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroInclusao.Text = "";

                txtCorFornecedor.Enabled = true;
                ddlCor.Enabled = true;
                ddlUnidadeMedida.Enabled = true;

                var materialCor = ddlTecido.SelectedValue;

                if (materialCor.Contains("|"))
                {
                    string material = materialCor.Split('|')[0];
                    string corMaterial = materialCor.Split('|')[1];

                    var materialLinx = desenvController.ObterMaterialPesquisa(material, corMaterial);
                    if (materialLinx != null)
                    {
                        txtCorFornecedor.Text = materialLinx.REFER_FABRICANTE;
                        txtCorFornecedor.Enabled = false;

                        ddlCor.SelectedValue = materialLinx.COR_MATERIAL;
                        ddlCor.Enabled = false;

                        ddlUnidadeMedida.SelectedValue = materialLinx.UNID_ESTOQUE;
                        ddlUnidadeMedida.Enabled = false;

                        imgTecido.ImageUrl = materialLinx.FOTO_TECIDO;
                    }
                }
                else
                {
                    txtCorFornecedor.Text = "";
                    ddlCor.SelectedValue = "";
                    ddlUnidadeMedida.SelectedValue = "";

                    imgTecido.ImageUrl = "";
                }
            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }
        }
        protected void imgBtnAbrirTecido_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                string tecido = txtTecidoFiltro.Text.Trim();

                Response.Redirect("desenv_material.aspx?t=1&tec=" + tecido, "_blank", "");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btIncluirPrePedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroInclusao.Text = "";

                if (!ValidarCampos())
                {
                    labErroInclusao.Text = "Preencha corretamente os campos em vermelho.";
                    return;
                }

                if (Session["USUARIO"] == null)
                {
                    labErroInclusao.Text = "Por favor, efetue seu Login novamente...";
                    return;
                }

                var material = "";
                if (ddlTecido.SelectedValue != "-1" && ddlTecido.SelectedValue != "")
                {
                    var materialCor = ddlTecido.SelectedValue;
                    material = materialCor.Split('|')[0];
                    if (material == "00.00.0000")
                        material = "";
                }

                var prePedido = new DESENV_PREPEDIDO();
                bool inc = true;
                //string codigo = hidCodigo.Value;
                //if (codigo != "")
                //{
                //    prePedido = desenvController.ObterPrePedido(Convert.ToInt32(codigo));
                //    if (prePedido == null)
                //    {
                //        labErroInclusao.Text = "Produto não encontrado. Entrar em contato com TI.";
                //        return;
                //    }
                //    inc = false;
                //}

                prePedido.MARCA = ddlMarca.SelectedValue;
                prePedido.COLECAO = ddlColecao.SelectedValue;

                prePedido.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigemInc.SelectedValue);

                prePedido.MATERIAL = material;
                prePedido.TECIDO = txtTecidoFiltro.Text.Trim().ToUpper();
                prePedido.FORNECEDOR = ddlFornecedor.SelectedValue;

                prePedido.COR_FORNECEDOR = txtCorFornecedor.Text.Trim().ToUpper();
                prePedido.COR = ddlCor.SelectedValue;
                prePedido.GRUPO_PRODUTO = ddlGrupoPedido.SelectedValue;
                prePedido.GRIFFE = ddlGriffe.SelectedValue;
                prePedido.UNIDADE_MEDIDA = ddlUnidadeMedida.SelectedValue;
                prePedido.CONSUMO = Convert.ToDecimal(txtConsumo.Text.Trim());

                prePedido.PRECO_TECIDO = 0;
                if (txtPrecoTecido.Text.Trim() != "")
                    prePedido.PRECO_TECIDO = Convert.ToDecimal(txtPrecoTecido.Text.Trim());
                prePedido.QTDE_ATACADO = Convert.ToInt32(txtQtdeAtacado.Text.Trim());
                prePedido.QTDE_VAREJO = Convert.ToInt32(txtQtdeVarejo.Text.Trim());
                prePedido.PRECO_VENDA = Convert.ToDecimal(txtPrecoVenda.Text.Trim());

                prePedido.CONSUMO_TOTAL = ((prePedido.QTDE_ATACADO + prePedido.QTDE_VAREJO) * prePedido.CONSUMO);
                prePedido.VALOR_TOTAL_ATACADO = ((prePedido.PRECO_VENDA / 2) * prePedido.QTDE_ATACADO);
                prePedido.VALOR_TOTAL_VAREJO = ((prePedido.PRECO_VENDA) * prePedido.QTDE_VAREJO);

                prePedido.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                prePedido.DATA_INCLUSAO = DateTime.Now;

                prePedido.SIGNED_NOME = txtSignedNome.Text.Trim().ToUpper();
                prePedido.REF_MODELAGEM = txtRefModelagem.Text.Trim().ToUpper();

                if (inc)
                    desenvController.InserirPrePedido(prePedido);
                else
                    desenvController.AtualizarPrePedido(prePedido);

                CarregarPrePedido(ORIGEM_PREPEDIDO.MANTERFILTRO);

                LimparCampos();
                labErroInclusao.Text = "Produto incluído com Sucesso.";
            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlMarca.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Marca.";
                    return;
                }

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                //if (ddlOrigem.SelectedValue == "0")
                //{
                //    labErro.Text = "Selecione a Origem.";
                //    return;
                //}

                ddlOrigemInc.SelectedValue = ddlOrigem.SelectedValue;
                //ddlOrigemInc.Enabled = false;

                pnlResumo.Visible = true;

                LimparFiltros();
                CarregarPrePedido(ORIGEM_PREPEDIDO.RECARREGARFILTRO);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> ObterPrePedidoLiberacaoProduto()
        {
            var prePedidoLibProdudo = desenvController.ObterPrePedidoLiberacaoProduto(ddlMarca.SelectedValue, ddlColecao.SelectedValue, Convert.ToInt32(ddlOrigem.SelectedValue), "", "");

            //FILTROSSSSSSSSS
            var prePedidoLibProdutoFiltro = prePedidoLibProdudo.AsEnumerable();

            gPrePedidoLib.AddRange(prePedidoLibProdutoFiltro);

            if (ddlFiltroOrigem.SelectedValue != "")
            {
                if (ddlFiltroOrigem.SelectedValue != "VAZIO")
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.ORIGEM.Trim() == ddlFiltroOrigem.SelectedItem.Text.Trim());
                else
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.ORIGEM.Trim() == "");
            }

            if (ddlFiltroFabricante.SelectedValue != "")
            {
                if (ddlFiltroFabricante.SelectedValue != "VAZIO")
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.FORNECEDOR.Trim() == ddlFiltroFabricante.SelectedItem.Text.Trim());
                else
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.FORNECEDOR.Trim() == "");
            }

            if (ddlFiltroProduto.SelectedValue != "")
            {
                if (ddlFiltroProduto.SelectedValue != "VAZIO")
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.PRODUTO.Trim() == ddlFiltroProduto.SelectedItem.Text.Trim());
                else
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.PRODUTO == "");
            }

            if (ddlFiltroLibAtacado.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.LIBERACAO_ATACADO == ddlFiltroLibAtacado.SelectedValue.Trim());

            if (ddlFiltroQtdeAtacado.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.QTDE_ATACADO == Convert.ToInt32(ddlFiltroQtdeAtacado.SelectedValue));

            if (ddlFiltroLibVarejo.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.LIBERACAO_VAREJO == ddlFiltroLibVarejo.SelectedValue.Trim());

            if (ddlFiltroQtdeVarejo.SelectedValue != "")
            {
                if (ddlFiltroQtdeVarejo.SelectedValue == "S")
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.QTDE_VAREJO > 0);
                else
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.QTDE_VAREJO <= 0);
            }

            if (ddlFiltroQtdeVarejoRepique.SelectedValue != "")
            {
                if (ddlFiltroQtdeVarejoRepique.SelectedValue == "S")
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.QTDE_VAREJO_REPIQUE > 0);
                else
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.QTDE_VAREJO_REPIQUE <= 0);
            }

            if (ddlFiltroSigned.SelectedValue != "")
            {
                if (ddlFiltroSigned.SelectedValue != "VAZIO")
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.SIGNED_NOME.Trim().Contains(ddlFiltroSigned.SelectedItem.Text.Trim()));
                else
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.SIGNED_NOME.Trim() == "");
            }

            if (ddlFiltroTecido.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.TECIDO.Trim() == (ddlFiltroTecido.SelectedValue.Trim()));


            if (ddlFiltroCorFornecedor.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.COR_FORNECEDOR.Trim().Contains(ddlFiltroCorFornecedor.SelectedValue.Trim()));

            if (ddlFiltroCorLinx.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.DESC_COR.Trim() == ddlFiltroCorLinx.SelectedValue.Trim());

            if (ddlFiltroGrupoProduto.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.GRUPO_PRODUTO.Trim() == ddlFiltroGrupoProduto.SelectedValue.Trim());

            if (ddlFiltroGriffe.SelectedValue != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.GRIFFE.Trim() == ddlFiltroGriffe.SelectedValue.Trim());

            if (ddlFiltroTecidoLib.SelectedValue != "")
            {
                if (ddlFiltroTecidoLib.SelectedValue == "S")
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.DATA_APROVACAO_TECIDO != null);
                else
                    prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.Where(p => p.DATA_APROVACAO_TECIDO == null);
            }


            PreencherTotais(prePedidoLibProdutoFiltro.ToList());

            if (hidOrdemCampo.Value.Trim() != "" && hidOrdem.Value.Trim() != "")
                prePedidoLibProdutoFiltro = prePedidoLibProdutoFiltro.OrderBy(hidOrdemCampo.Value + hidOrdem.Value);

            gPrePedidoLibFiltro.AddRange(prePedidoLibProdutoFiltro);

            return prePedidoLibProdutoFiltro.ToList();
        }
        private void CarregarPrePedido(ORIGEM_PREPEDIDO filtro)
        {
            var prePedidoLib = ObterPrePedidoLiberacaoProduto();

            gvPrePedido.DataSource = prePedidoLib;
            gvPrePedido.DataBind();


            var prePedidoTotal = new List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult>();
            prePedidoTotal.Add(new SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult
            {
                ORIGEM = "Total Geral",
                QTDE_VAREJO = gPrePedidoLib.Sum(p => p.QTDE_VAREJO),
                QTDE_VAREJO_REPIQUE = gPrePedidoLib.Sum(p => p.QTDE_VAREJO_REPIQUE),
                QTDE_ATACADO = gPrePedidoLib.Sum(p => p.QTDE_ATACADO),
                QTDE_VENDA_ATACADO = gPrePedidoLib.Sum(p => p.QTDE_VENDA_ATACADO),
                VALOR_VENDA_ATACADO = gPrePedidoLib.Sum(p => p.VALOR_VENDA_ATACADO),
                VALOR_VENDA_TOTAL_VAR = gPrePedidoLib.Sum(p => p.VALOR_VENDA_TOTAL_VAR),
                CONSUMO_TOTAL = gPrePedidoLib.Sum(p => p.CONSUMO_TOTAL)
            });

            gvPrePedidoTotal.DataSource = prePedidoTotal;
            gvPrePedidoTotal.DataBind();


            //Carregar Consumo Tecido
            CarregarTecidoConsumo();

            if (ORIGEM_PREPEDIDO.RECARREGARFILTRO == filtro)
                CarregarFiltros(gPrePedidoLib);

            CarregarFiltroCorLinx(gPrePedidoLib);
            CarregarFiltroCorFornecedor(gPrePedidoLib);
        }

        private void PreencherTotais(List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> lstPrePedidoProduto)
        {
            int totalModelo = 0;
            int totalEstilo = 0;

            var estilo = lstPrePedidoProduto.Where(p => p.PRODUTO != null && p.PRODUTO.Trim() != "" && p.PRODUTO.Trim().Length >= 4);
            if (estilo != null)
                totalEstilo = estilo.Count();

            var modelo = estilo.Select(j => j.PRODUTO).Distinct();
            if (modelo != null)
                totalModelo = modelo.Count();

            labModeloFiltroValor.Text = totalModelo.ToString();
            labEstiloFiltroValor.Text = totalEstilo.ToString();
        }

        protected void gvPrePedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult pre = e.Row.DataItem as SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult;


                    Literal _litValorAtacado = e.Row.FindControl("litValorAtacado") as Literal;
                    _litValorAtacado.Text = "R$ " + pre.VALOR_VENDA_ATACADO.ToString("###,###,###,##0.00");

                    Literal _litPrecoVenda = e.Row.FindControl("litPrecoVenda") as Literal;
                    _litPrecoVenda.Text = "R$ " + pre.PRECO_VENDA.ToString("###,###,###,##0.00");

                    Literal _litValorTotVarejo = e.Row.FindControl("litValorTotVarejo") as Literal;
                    _litValorTotVarejo.Text = "R$ " + Convert.ToDecimal(pre.VALOR_VENDA_TOTAL_VAR).ToString("###,###,##0.000");

                    Literal _litConsumo = e.Row.FindControl("litConsumo") as Literal;
                    _litConsumo.Text = Convert.ToDecimal(pre.CONSUMO).ToString("###,###,##0.000");

                    Literal _litConsumoTotal = e.Row.FindControl("litConsumoTotal") as Literal;
                    _litConsumoTotal.Text = Convert.ToDecimal(pre.CONSUMO_TOTAL).ToString("###,###,###,##0.000");

                    ImageButton _imgBtnFoto = e.Row.FindControl("imgBtnFoto") as ImageButton;
                    ImageButton _imgBtnFotoAux = e.Row.FindControl("imgBtnFotoAux") as ImageButton;

                    _imgBtnFoto.CommandArgument = pre.DESENV_PRODUTO.ToString();
                    _imgBtnFoto.ImageUrl = pre.FOTO;
                    _imgBtnFoto.Visible = true;

                    _imgBtnFotoAux.CommandArgument = pre.DESENV_PRODUTO.ToString();
                    _imgBtnFotoAux.ImageUrl = pre.FOTO;
                    _imgBtnFotoAux.Visible = true;
                    if (pre.FOTO == null || pre.FOTO == "")
                    {
                        _imgBtnFoto.Visible = false;
                        _imgBtnFotoAux.Visible = false;
                    }

                    ImageButton _imgBtnLiberar = e.Row.FindControl("imgBtnLiberar") as ImageButton;
                    _imgBtnLiberar.CommandArgument = pre.CODIGO.ToString();

                    ImageButton _imgBtnExcluir = e.Row.FindControl("imgBtnExcluir") as ImageButton;
                    _imgBtnExcluir.CommandArgument = pre.CODIGO.ToString();

                    ImageButton _imgBtnCopiar = e.Row.FindControl("imgBtnCopiar") as ImageButton;
                    _imgBtnCopiar.CommandArgument = pre.CODIGO.ToString();

                    ImageButton _imgBtnAprovarTecido = e.Row.FindControl("imgBtnAprovarTecido") as ImageButton;
                    _imgBtnAprovarTecido.CommandArgument = pre.CODIGO.ToString();
                    if (pre.DATA_APROVACAO_TECIDO == null)
                        _imgBtnAprovarTecido.ImageUrl = "~/Image/up_disabled.png";
                    else
                        _imgBtnAprovarTecido.ImageUrl = "~/Image/up.png";


                    TextBox _txtDEFVarejo = e.Row.FindControl("txtDEFVarejo") as TextBox;
                    if (pre.LIBERACAO_VAREJO.ToLower() == "corte")
                    {
                        e.Row.Cells[16].BackColor = Color.PeachPuff;
                        _txtDEFVarejo.Enabled = false;
                    }
                    else
                    {
                        e.Row.Cells[16].BackColor = Color.Honeydew;
                    }

                    TextBox _txtDEFAtacado = e.Row.FindControl("txtDEFAtacado") as TextBox;
                    if (pre.LIBERACAO_ATACADO.ToLower() == "corte")
                    {
                        e.Row.Cells[19].BackColor = Color.PeachPuff;
                        _txtDEFAtacado.Enabled = false;
                    }
                    else
                    {
                        e.Row.Cells[19].BackColor = Color.FloralWhite;
                    }

                    if (pre.LIBERACAO_ATACADO.ToLower() == "corte" || pre.LIBERACAO_VAREJO.ToLower() == "corte")
                        _imgBtnExcluir.Visible = false;
                    else _imgBtnExcluir.Visible = true;


                    if (pre.VAREJO_DEF == "S")
                        e.Row.Cells[28].BackColor = Color.LightBlue;
                    else
                        e.Row.Cells[28].BackColor = Color.White;

                    if (pre.CONSUMO_MOSTRUARIO_OK == "S")
                        e.Row.Cells[27].BackColor = Color.LightCyan;
                    else
                        e.Row.Cells[27].BackColor = Color.White;


                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPrePedido_DataBound(object sender, EventArgs e)
        {
            var footer = gvPrePedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[6].Text = "Total Filtro";

                footer.Cells[17].Text = gPrePedidoLibFiltro.Sum(p => p.QTDE_VAREJO).ToString();
                footer.Cells[18].Text = gPrePedidoLibFiltro.Sum(p => p.QTDE_VAREJO_REPIQUE).ToString();
                footer.Cells[21].Text = gPrePedidoLibFiltro.Sum(p => p.QTDE_ATACADO).ToString();

                footer.Cells[24].Text = gPrePedidoLibFiltro.Sum(p => p.QTDE_VENDA_ATACADO).ToString();
                footer.Cells[25].Text = "R$ " + gPrePedidoLibFiltro.Sum(p => p.VALOR_VENDA_ATACADO).ToString("###,###,###,##0.00");

                footer.Cells[27].Text = "R$ " + Convert.ToDecimal(gPrePedidoLibFiltro.Sum(p => p.VALOR_VENDA_TOTAL_VAR)).ToString("###,###,###,##0.00");
                footer.Cells[29].Text = gPrePedidoLibFiltro.Sum(p => p.CONSUMO_TOTAL).ToString();

            }
        }
        protected void gvPrePedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> prePedido = ObterPrePedidoLiberacaoProduto();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            hidOrdemCampo.Value = e.SortExpression;
            hidOrdem.Value = sortDirection;

            prePedido = prePedido.OrderBy(e.SortExpression + sortDirection);
            gvPrePedido.DataSource = prePedido.ToList();
            gvPrePedido.DataBind();

        }
        protected void gvPrePedido_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrePedido.PageIndex = e.NewPageIndex;
            CarregarPrePedido(ORIGEM_PREPEDIDO.MANTERFILTRO);
        }
        protected void ddlPaginaNumero_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageSize = Convert.ToInt32(ddlPaginaNumero.SelectedValue);
            gvPrePedido.PageSize = pageSize;
            CarregarPrePedido(ORIGEM_PREPEDIDO.MANTERFILTRO);
        }

        protected void txtDEF_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                TextBox txt = (TextBox)sender;

                int defValor = 0;
                if (txt.Text == "".Trim())
                    txt.Text = "0";
                defValor = Convert.ToInt32(txt.Text.Trim());

                //Obter Codigo PrePedido
                GridViewRow row = (GridViewRow)txt.NamingContainer;
                int codigo = Convert.ToInt32(gvPrePedido.DataKeys[row.RowIndex][0].ToString());
                string produto = gvPrePedido.DataKeys[row.RowIndex][2].ToString();


                //Atualizar PrePedido
                var prePedido = desenvController.ObterPrePedido(codigo);
                if (txt.ID.ToLower().Contains("varejo"))
                    prePedido.QTDE_VAREJO = defValor;
                else
                    prePedido.QTDE_ATACADO = defValor;


                prePedido.CONSUMO_TOTAL = ((prePedido.QTDE_ATACADO + prePedido.QTDE_VAREJO) * prePedido.CONSUMO);
                prePedido.VALOR_TOTAL_ATACADO = ((prePedido.PRECO_VENDA / 2) * prePedido.QTDE_ATACADO);
                prePedido.VALOR_TOTAL_VAREJO = ((prePedido.PRECO_VENDA) * prePedido.QTDE_VAREJO);
                desenvController.AtualizarPrePedido(prePedido);
                /*******************************************************/


                //Atualizar PrePedido Definição LOG
                var prePedidoDef = new DESENV_PREPEDIDO_DEF();
                prePedidoDef.DESENV_PREPEDIDO = codigo;
                if (txt.ID.ToLower().Contains("varejo"))
                {
                    prePedidoDef.QTDE_VAREJO = ((defValor == 0) ? -1 : defValor);
                    prePedidoDef.QTDE_ATACADO = 0;
                }
                else
                {
                    prePedidoDef.QTDE_VAREJO = 0;
                    prePedidoDef.QTDE_ATACADO = ((defValor == 0) ? -1 : defValor);
                }
                prePedidoDef.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                prePedidoDef.DATA_ALTERACAO = DateTime.Now;
                desenvController.InserirPrePedidoDEF(prePedidoDef);

                //ATUALIZAR DESENV_PRODUTO
                //PENDENTE
                int codigoDesenvProduto = Convert.ToInt32(gvPrePedido.DataKeys[row.RowIndex][1].ToString());
                var desenvProduto = desenvController.ObterProduto(codigoDesenvProduto);
                if (desenvProduto != null)
                {
                    if (txt.ID.ToLower().Contains("varejo"))
                        desenvProduto.QTDE = defValor;
                    else
                        desenvProduto.QTDE_ATACADO = defValor;

                    desenvController.AtualizarProduto(desenvProduto);
                }

                CarregarPrePedido(ORIGEM_PREPEDIDO.MANTERFILTRO);
            }
            catch (Exception)
            {
            }
        }

        protected void imgBtnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                int codigo = Convert.ToInt32(bt.CommandArgument);

                var prePedido = desenvController.ObterPrePedido(codigo);
                if (prePedido != null)
                {
                    //Verificar se existe DESENV_PRODUTO, EXCLUIR
                    if (prePedido.PRODUTO != null)
                        ExcluirDESENVPRODUTO(prePedido.COLECAO, prePedido.PRODUTO, prePedido.COR, prePedido.DESENV_PRODUTO_ORIGEM);

                    desenvController.ExcluirPrePedido(prePedido.CODIGO);
                }

                CarregarPrePedido(ORIGEM_PREPEDIDO.MANTERFILTRO);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void ExcluirDESENVPRODUTO(string colecao, string produto, string cor, int desenvProdutoOrigem)
        {

            var desenvProduto = desenvController.ObterProduto(colecao, produto, cor, desenvProdutoOrigem);
            if (desenvProduto != null)
            {
                desenvProduto.STATUS = 'E'; //Excluido
                desenvProduto.DATA_APROVACAO = null;

                //Obter Producao do Produto
                var _produtoProducao = desenvController.ObterProdutoProducao(desenvProduto.CODIGO);
                foreach (DESENV_PRODUTO_PRODUCAO pp in _produtoProducao)
                    if (pp != null)
                        desenvController.ExcluirProdutoProducao(pp.CODIGO);

                //Atualizar produto
                desenvController.ExcluirProduto(desenvProduto);
            }
        }

        #region "TECIDO"
        protected void gvTecido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDO_TECIDOResult tec = e.Row.DataItem as SP_OBTER_PREPEDIDO_TECIDOResult;

                    TextBox _txtQtdeComprada = e.Row.FindControl("txtQtdeComprada") as TextBox;
                    _txtQtdeComprada.Text = tec.QTDE_COMPRA.ToString("###,###,###,##0.00");

                    Literal _litQtdeTecidoConsumida = e.Row.FindControl("litQtdeTecidoConsumida") as Literal;
                    _litQtdeTecidoConsumida.Text = Convert.ToDecimal(tec.CONSUMO_TOTAL).ToString("###,###,###,##0.00");

                    Literal _litQtdeTecidoEstoque = e.Row.FindControl("litQtdeTecidoEstoque") as Literal;
                    _litQtdeTecidoEstoque.Text = Convert.ToDecimal(tec.QTDE_SOBRA).ToString("###,###,###,##0.00");

                    gQtdeCompra += tec.QTDE_COMPRA;
                    gQtdeConsumo += Convert.ToDecimal(tec.CONSUMO_TOTAL);
                    gQtdeSobra += Convert.ToDecimal(tec.QTDE_SOBRA);

                }
            }
        }
        protected void gvTecido_DataBound(object sender, EventArgs e)
        {
            var footer = gvTecido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[4].Text = gQtdeCompra.ToString("###,###,###,##0.00");
                footer.Cells[5].Text = gQtdeConsumo.ToString("###,###,###,##0.00");
                footer.Cells[6].Text = gQtdeSobra.ToString("###,###,###,##0.00");
            }
        }
        private void CarregarTecidoConsumo()
        {
            pnlTecido.Visible = false;

            if (ddlFiltroTecido.SelectedValue != "" || ddlFiltroCorLinx.SelectedValue != "" || ddlFiltroCorFornecedor.SelectedValue != "")
            {

                string marca = ddlMarca.SelectedValue;
                string colecao = ddlColecao.SelectedValue;
                int desenvProdutoOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);
                string corLinx = "";
                string corFornecedor = ddlFiltroCorFornecedor.SelectedValue;

                //CONTROLE DE MATERIAL
                string material = "";
                string tecido = "";
                string tecidoFiltro = ddlFiltroTecido.SelectedValue;
                if (ddlFiltroTecido.SelectedValue.Contains("-"))
                    material = tecidoFiltro.Split('-')[0].Trim();
                else
                    tecido = tecidoFiltro;

                //CONTROLE DE COR
                var corBasica = prodController.ObterCoresBasicasDescricao(ddlFiltroCorLinx.SelectedValue);
                if (corBasica != null)
                    corLinx = corBasica.COR;

                var tecidoConsumo = desenvController.ObterPrePedidoTecidoConsumo(marca, colecao, desenvProdutoOrigem, material, tecido, corLinx, corFornecedor);
                gvTecido.DataSource = tecidoConsumo;
                gvTecido.DataBind();

                //Se existir resultado, mostrar painel de tecido
                if (tecidoConsumo.Count() > 0)
                    pnlTecido.Visible = true;

            }
        }

        protected void txtQtdeComprada_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;
                if (row != null)
                {
                    string marca = ddlMarca.SelectedValue;
                    string material = gvTecido.DataKeys[row.RowIndex][0].ToString();
                    string tecido = gvTecido.DataKeys[row.RowIndex][1].ToString();
                    string corLinx = gvTecido.DataKeys[row.RowIndex][2].ToString();
                    string corFornecedor = gvTecido.DataKeys[row.RowIndex][3].ToString();

                    decimal qtdecompra = 0;
                    if (txt.Text != "")
                        qtdecompra = Convert.ToDecimal(txt.Text);

                    if (material != "")
                        tecido = "";

                    var tecidoConsumo = desenvController.ObterPrePedidoTecido(marca, material, tecido, corLinx, corFornecedor);
                    if (tecidoConsumo != null)
                    {
                        tecidoConsumo.QTDE_COMPRA = qtdecompra;
                        desenvController.AtualizarPrePedidoTecido(tecidoConsumo);
                    }
                    else
                    {
                        var t = new DESENV_PREPEDIDO_TECIDO();
                        t.MARCA = marca;
                        t.MATERIAL = material;
                        t.TECIDO = tecido;
                        t.COR_LINX = corLinx;
                        t.COR_FORNECEDOR = corFornecedor;
                        t.QTDE_COMPRA = qtdecompra;
                        desenvController.InserirPrePedidoTecido(t);
                    }

                    CarregarPrePedido(ORIGEM_PREPEDIDO.MANTERFILTRO);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        protected void imgBtnFoto_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    string codigoProduto = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaiorVert('desenv_tripa_view_foto.aspx?p=" + codigoProduto + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }

        }
        protected void imgBtnLiberar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            if (b != null)
            {
                try
                {
                    string marca = ddlMarca.SelectedValue;
                    string codigoProduto = b.CommandArgument;
                    string codigoOrigem = ddlOrigem.SelectedValue;

                    //Abrir pop-up
                    string url = "fnAbrirTelaCadastroMaior('desenv_tripa_view_produto.aspx?m=" + marca + "&p=" + codigoProduto + "&o=" + codigoOrigem + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
                }
                catch (Exception)
                {
                }
            }
        }

        protected void imgBtnCopiar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErroInclusao.Text = "";

                ImageButton b = (ImageButton)sender;
                string codigo = b.CommandArgument;

                var prePedido = desenvController.ObterPrePedido(Convert.ToInt32(codigo));
                if (prePedido != null)
                {
                    var tecidoFiltro = "";
                    if (prePedido.TECIDO.Contains("-"))
                    {
                        tecidoFiltro = prePedido.TECIDO.Split('-')[1].Trim();
                    }
                    else
                    {
                        tecidoFiltro = prePedido.TECIDO;
                    }

                    txtTecidoFiltro.Text = tecidoFiltro;
                    txtTecidoFiltro_TextChanged(txtTecidoFiltro, null);
                    if (prePedido.MATERIAL.Trim() != "")
                    {
                        ddlTecido.SelectedValue = prePedido.MATERIAL.Trim() + "|" + prePedido.COR.Trim();
                        ddlTecido_TextChanged(ddlTecido, null);
                    }
                    else
                    {
                        if (ddlTecido.Enabled)
                            ddlTecido.SelectedValue = "00.00.0000";

                        txtCorFornecedor.Text = prePedido.COR_FORNECEDOR;
                        ddlCor.SelectedValue = prePedido.COR;

                        if (prePedido.UNIDADE_MEDIDA != null && prePedido.UNIDADE_MEDIDA.Trim() != "")
                            ddlUnidadeMedida.SelectedValue = prePedido.UNIDADE_MEDIDA;
                    }

                    ddlOrigem.SelectedValue = prePedido.DESENV_PRODUTO_ORIGEM.ToString();

                    ddlFornecedor.SelectedValue = prePedido.FORNECEDOR;
                    txtPrecoTecido.Text = prePedido.PRECO_TECIDO.ToString();
                    ddlGriffe.SelectedValue = baseController.BuscaGriffes().Where(p => p.GRIFFE.Trim() == prePedido.GRIFFE.Trim()).SingleOrDefault().GRIFFE;
                    ddlGrupoPedido.SelectedValue = prePedido.GRUPO_PRODUTO;

                    txtConsumo.Text = prePedido.CONSUMO.ToString();
                    txtQtdeAtacado.Text = prePedido.QTDE_ATACADO.ToString();
                    txtQtdeVarejo.Text = prePedido.QTDE_VAREJO.ToString();
                    txtPrecoVenda.Text = prePedido.PRECO_VENDA.ToString();

                    txtConsumoTotal.Text = prePedido.CONSUMO_TOTAL.ToString();
                    txtTotalAtacado.Text = "R$ " + prePedido.VALOR_TOTAL_ATACADO.ToString();
                    txtTotalVarejo.Text = "R$ " + prePedido.VALOR_TOTAL_VAREJO.ToString();

                    txtSignedNome.Text = prePedido.SIGNED_NOME;
                    txtRefModelagem.Text = prePedido.REF_MODELAGEM;

                    pnlInc.Visible = true;
                    btnImgEsconderInc.Visible = true;
                    btnImgMostrarInc.Visible = false;
                }

                hidCodigo.Value = "0";
                btIncluirPrePedido.Text = "Incluir";
                btLimparTodos.Text = "Limpar Campos";

            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }
        }

        protected void btResumo_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                Button b = (Button)sender;

                string marca = ddlMarca.SelectedValue;
                string colecao = ddlColecao.SelectedValue.Trim();
                string griffe = b.CommandArgument;
                string desenvOrigem = ddlOrigem.SelectedValue;

                //Abrir pop-up
                var url = "fnAbrirTelaCadastroMaior2('desenv_tripa_prepedido_resumo.aspx?m=" + marca + "&g=" + griffe + "&c=" + colecao + "&d=" + desenvOrigem + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);

            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }
        }

        protected void gvPrePedidoTotal_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult pre = e.Row.DataItem as SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult;


                    Literal _litValorAtacado = e.Row.FindControl("litValorAtacado") as Literal;
                    _litValorAtacado.Text = "R$ " + pre.VALOR_VENDA_ATACADO.ToString("###,###,###,##0.00");

                    Literal _litValorTotVarejo = e.Row.FindControl("litValorTotVarejo") as Literal;
                    _litValorTotVarejo.Text = "R$ " + Convert.ToDecimal(pre.VALOR_VENDA_TOTAL_VAR).ToString("###,###,##0.000");

                    Literal _litConsumoTotal = e.Row.FindControl("litConsumoTotal") as Literal;
                    _litConsumoTotal.Text = Convert.ToDecimal(pre.CONSUMO_TOTAL).ToString("###,###,###,##0.000");

                }
            }
        }

        protected void imgBtnAprovarTecido_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgBtnAprovarTecido = (ImageButton)sender;

                var codigoPrePedido = Convert.ToInt32(imgBtnAprovarTecido.CommandArgument.ToString());

                var desenvPrePedido = desenvController.ObterPrePedido(codigoPrePedido);
                if (desenvPrePedido != null)
                {
                    // se nao esta aprovado
                    if (imgBtnAprovarTecido.ImageUrl.ToLower().Contains("disabled"))
                        desenvPrePedido.DATA_APROVACAO_TECIDO = DateTime.Now;
                    else
                        desenvPrePedido.DATA_APROVACAO_TECIDO = null;

                    desenvController.AtualizarPrePedido(desenvPrePedido);

                    // se nao esta aprovado
                    if (imgBtnAprovarTecido.ImageUrl.ToLower().Contains("disabled"))
                        imgBtnAprovarTecido.ImageUrl = "~/Image/up.png";
                    else
                        imgBtnAprovarTecido.ImageUrl = "~/Image/up_disabled.png";

                }

            }
            catch (Exception)
            {
            }
        }

    }
}

