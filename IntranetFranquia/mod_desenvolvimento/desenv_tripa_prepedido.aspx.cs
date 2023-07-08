using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_tripa_prepedido : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        List<SP_OBTER_PREPEDIDOResult> gPrePedido = new List<SP_OBTER_PREPEDIDOResult>();

        enum ORIGEM_PREPEDIDO
        {
            BUSCA = 1,
            INCLUSAO = 2,
            FILTRO = 3,
        }

        int gQtdeAtacado = 0;
        int gQtdeVarejo = 0;
        decimal gTotalAtacado = 0;
        decimal gTotalVarejo = 0;
        decimal gTotConsumo = 0;
        decimal gTotConsumoTotal = 0;
        decimal gTotTecido = 0;

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

                //string tela = Request.QueryString["t"].ToString();
                //if (tela != "1" && tela != "2")
                //{
                //    Session["USUARIO"] = null;
                //    Response.Redirect("~/Login.aspx");
                //}

                //if (tela == "1")
                //    hrefVoltar.HRef = "desenv_menu.aspx";


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
                ddlOrigemFiltro.DataSource = origem;
                ddlOrigemFiltro.DataBind();

                ddlOrigem.DataSource = origem;
                ddlOrigem.DataBind();

                if (origem.Count == 2)
                {
                    ddlOrigemFiltro.SelectedValue = origem[1].CODIGO.ToString();
                    ddlOrigem.SelectedValue = origem[1].CODIGO.ToString();
                }
            }
        }
        private void CarregarGriffe()
        {
            var griffe = RetornarGriffe();

            if (griffe != null)
            {
                ddlGriffeFiltro.DataSource = griffe;
                ddlGriffeFiltro.DataBind();

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

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;


            labOrigem.ForeColor = _OK;
            if (ddlOrigem.SelectedValue == "0")
            {
                labOrigem.ForeColor = _notOK;
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
            txtPrecoTecido.Text = "";
            txtQtdeAtacado.Text = "";
            txtQtdeVarejo.Text = "";
            txtPrecoVenda.Text = "";
            txtConsumoTotal.Text = "";
            txtValorTecido.Text = "";
            txtTotalAtacado.Text = "";
            txtTotalVarejo.Text = "";

            labErro.Text = "";
            labErroInclusao.Text = "";

            LimparAlteracao();

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
            txtValorTecido.Text = "";
            txtTotalAtacado.Text = "";
            txtTotalVarejo.Text = "";
            txtSignedNome.Text = "";

            ddlFornecedor.SelectedValue = "";
            txtTecidoFiltro.Text = "";
            txtCorFornecedor.Text = "";
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
        private void LimparAlteracao()
        {
            hidCodigo.Value = "";
            btIncluirPrePedido.Text = "Incluir";
            btLimparTodos.Text = "Limpar Campos";
        }
        protected void btLimparTodos_Click(object sender, EventArgs e)
        {
            LimparTodosCampos();
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroInclusao.Text = "";

                if (ddlMarca.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Marca.";
                    pnlPrePedido.Visible = false;
                    pnlResumo.Visible = false;
                    return;
                }

                if (ddlColecao.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    pnlPrePedido.Visible = false;
                    pnlResumo.Visible = false;
                    return;
                }

                pnlPrePedido.Visible = true;
                pnlResumo.Visible = true;
                if (ddlGriffeFiltro.SelectedValue != "")
                    ddlGriffe.SelectedValue = ddlGriffeFiltro.SelectedValue;

                CarregarPrePedido(ORIGEM_PREPEDIDO.BUSCA);

                LimparAlteracao();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "FILTROS"
        private void CarregarFiltros(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            CarregarFiltroOrigem(prePedido);
            CarregarFiltroProduto(prePedido);
            CarregarFiltroTecido(prePedido);
            CarregarFiltroFornecedor(prePedido);
            CarregarFiltroCorFornecedor(prePedido);
            CarregarFiltroCorLinx(prePedido);
            CarregarFiltroGrupoProduto(prePedido);
            CarregarFiltroGriffe(prePedido);
            CarregarFiltroMedida(prePedido);
            CarregarFiltroConsumo(prePedido);
            CarregarFiltroPrecoTecido(prePedido);
            CarregarFiltroQtdeAtacado(prePedido);
            CarregarFiltroQtdeVarejo(prePedido);
            CarregarFiltroProdParcial(prePedido);
            CarregarFiltroPrecoVenda(prePedido);
            CarregarFiltroTotConsumo(prePedido);
            CarregarFiltroSigned(prePedido);
            CarregarFiltroPedido(prePedido);

        }
        private void CarregarFiltroPedido(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Where(p => p.STATUS_DESC != null).Select(p => p.STATUS_DESC).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            list.Add(new ListItem { Value = "Vazio", Text = "Vazio" });
            ddlFiltroPedido.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroPedido.DataBind();
        }
        private void CarregarFiltroOrigem(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.ORIGEM).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroOrigem.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroOrigem.DataBind();
        }
        private void CarregarFiltroProduto(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Where(p => p.PRODUTO != null).Select(p => p.PRODUTO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroProduto.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroProduto.DataBind();
        }
        private void CarregarFiltroTecido(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.TECIDO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroTecido.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroTecido.DataBind();
        }
        private void CarregarFiltroFornecedor(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.FORNECEDOR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroFornecedor.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroFornecedor.DataBind();
        }
        private void CarregarFiltroCorFornecedor(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.COR_FORNECEDOR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroCorFornecedor.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroCorFornecedor.DataBind();
        }
        private void CarregarFiltroCorLinx(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.DESC_COR).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroCorLinx.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroCorLinx.DataBind();
        }
        private void CarregarFiltroGrupoProduto(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.GRUPO_PRODUTO).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroGrupoProduto.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroGrupoProduto.DataBind();
        }
        private void CarregarFiltroGriffe(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.GRIFFE.Trim()).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroGriffe.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroGriffe.DataBind();
        }
        private void CarregarFiltroMedida(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.UNIDADE_MEDIDA).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x, Text = x });

            ddlFiltroMedida.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroMedida.DataBind();
        }
        private void CarregarFiltroConsumo(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.CONSUMO).Distinct();

            var list = new List<ListItem>();
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            list = list.OrderBy(p => Convert.ToDecimal(p.Text)).ToList();
            list.Insert(0, new ListItem { Value = "", Text = "" });
            ddlFiltroConsumo.DataSource = list;
            ddlFiltroConsumo.DataBind();
        }
        private void CarregarFiltroPrecoTecido(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.PRECO_TECIDO).Distinct();

            var list = new List<ListItem>();
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            list = list.OrderBy(p => Convert.ToDecimal(p.Text)).ToList();
            list.Insert(0, new ListItem { Value = "", Text = "" });
            ddlFiltroPrecoTecido.DataSource = list;
            ddlFiltroPrecoTecido.DataBind();
        }
        private void CarregarFiltroQtdeAtacado(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.QTDE_ATACADO).Distinct();

            var list = new List<ListItem>();
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            list = list.OrderBy(p => Convert.ToDecimal(p.Text)).ToList();
            list.Insert(0, new ListItem { Value = "", Text = "" });
            ddlFiltroQtdeAtacado.DataSource = list;
            ddlFiltroQtdeAtacado.DataBind();
        }
        private void CarregarFiltroQtdeVarejo(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.QTDE_VAREJO).Distinct();

            var list = new List<ListItem>();
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            list = list.OrderBy(p => Convert.ToDecimal(p.Text)).ToList();
            list.Insert(0, new ListItem { Value = "", Text = "" });
            ddlFiltroQtdeVarejo.DataSource = list;
            ddlFiltroQtdeVarejo.DataBind();
        }
        private void CarregarFiltroProdParcial(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.PROD_PARCIAL).Distinct();

            var list = new List<ListItem>();
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            list.Insert(0, new ListItem { Value = "", Text = "" });
            ddlFiltroProdParcial.DataSource = list;
            ddlFiltroProdParcial.DataBind();
        }
        private void CarregarFiltroPrecoVenda(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.PRECO_VENDA).Distinct();

            var list = new List<ListItem>();
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            list = list.OrderBy(p => Convert.ToDecimal(p.Text)).ToList();
            list.Insert(0, new ListItem { Value = "", Text = "" });
            ddlFiltroPrecoVenda.DataSource = list;
            ddlFiltroPrecoVenda.DataBind();
        }
        private void CarregarFiltroTotConsumo(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Select(p => p.CONSUMO_TOTAL).Distinct();

            var list = new List<ListItem>();

            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            list = list.OrderBy(p => Convert.ToDecimal(p.Text)).ToList();
            list.Insert(0, new ListItem { Value = "", Text = "" });
            ddlFiltroTotalConsumo.DataSource = list;
            ddlFiltroTotalConsumo.DataBind();
        }
        private void CarregarFiltroSigned(List<SP_OBTER_PREPEDIDOResult> prePedido)
        {
            var y = prePedido.Where(p => p.SIGNED_NOME != null && p.SIGNED_NOME != "").Select(p => p.SIGNED_NOME).Distinct();

            var list = new List<ListItem>();
            list.Add(new ListItem { Value = "", Text = "" });
            foreach (var x in y)
                list.Add(new ListItem { Value = x.ToString(), Text = x.ToString() });

            ddlFiltroSigned.DataSource = list.OrderBy(p => p.Text);
            ddlFiltroSigned.DataBind();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarPrePedido(ORIGEM_PREPEDIDO.FILTRO);

            LimparAlteracao();
        }

        #endregion
        private List<SP_OBTER_PREPEDIDOResult> ObterPrePedido()
        {
            var prePedido = desenvController.ObterPrePedidoMarca(ddlMarca.SelectedValue, ddlColecao.SelectedValue, ddlGriffeFiltro.SelectedValue);

            if (ddlOrigemFiltro.SelectedValue != "0")
                prePedido = prePedido.Where(p => p.DESENV_PRODUTO_ORIGEM == Convert.ToInt32(ddlOrigemFiltro.SelectedValue)).ToList();

            gPrePedido.AddRange(prePedido);
            //FILTROSSSSSSSSS
            var prePedidoFiltro = prePedido.AsEnumerable();

            if (ddlFiltroOrigem.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.ORIGEM == ddlFiltroOrigem.SelectedValue);

            if (ddlFiltroProduto.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.PRODUTO == ddlFiltroProduto.SelectedValue);

            if (ddlFiltroTecido.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.TECIDO == ddlFiltroTecido.SelectedValue);

            if (ddlFiltroFornecedor.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.FORNECEDOR == ddlFiltroFornecedor.SelectedValue);

            if (ddlFiltroCorFornecedor.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.COR_FORNECEDOR == ddlFiltroCorFornecedor.SelectedValue);

            if (ddlFiltroCorLinx.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.DESC_COR == ddlFiltroCorLinx.SelectedValue);

            if (ddlFiltroGrupoProduto.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.GRUPO_PRODUTO == ddlFiltroGrupoProduto.SelectedValue);

            if (ddlFiltroGriffe.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.GRIFFE.Trim() == ddlFiltroGriffe.SelectedValue.Trim());

            if (ddlFiltroMedida.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.UNIDADE_MEDIDA == ddlFiltroMedida.SelectedValue);

            if (ddlFiltroConsumo.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.CONSUMO.ToString() == ddlFiltroConsumo.SelectedValue);

            if (ddlFiltroPrecoTecido.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.PRECO_TECIDO.ToString() == ddlFiltroPrecoTecido.SelectedValue);

            if (ddlFiltroQtdeAtacado.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.QTDE_ATACADO.ToString() == ddlFiltroQtdeAtacado.SelectedValue);

            if (ddlFiltroQtdeVarejo.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.QTDE_VAREJO.ToString() == ddlFiltroQtdeVarejo.SelectedValue);

            if (ddlFiltroProdParcial.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.PROD_PARCIAL.ToString() == ddlFiltroProdParcial.SelectedValue);

            if (ddlFiltroPrecoVenda.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.PRECO_VENDA.ToString() == ddlFiltroPrecoVenda.SelectedValue);

            if (ddlFiltroTotalConsumo.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.CONSUMO_TOTAL.ToString() == ddlFiltroTotalConsumo.SelectedValue);

            if (ddlFiltroSigned.SelectedValue != "")
                prePedidoFiltro = prePedidoFiltro.Where(p => p.SIGNED_NOME == ddlFiltroSigned.SelectedValue);

            if (ddlFiltroPedido.SelectedValue != "")
            {
                if (ddlFiltroPedido.SelectedValue == "Vazio")
                {
                    prePedidoFiltro = prePedidoFiltro.Where(p => p.STATUS_DESC == "");
                }
                else
                {
                    prePedidoFiltro = prePedidoFiltro.Where(p => p.STATUS_DESC == ddlFiltroPedido.SelectedValue);
                }
            }

            return prePedidoFiltro.ToList();
        }

        private void CarregarPrePedido(ORIGEM_PREPEDIDO o)
        {
            var prePedido = ObterPrePedido();

            gvPrePedido.DataSource = prePedido;
            gvPrePedido.DataBind();

            if (o != ORIGEM_PREPEDIDO.FILTRO)
                CarregarFiltros(gPrePedido);

        }
        protected void gvPrePedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDOResult pre = e.Row.DataItem as SP_OBTER_PREPEDIDOResult;

                    Literal litConsumo = e.Row.FindControl("litConsumo") as Literal;
                    litConsumo.Text = pre.CONSUMO.ToString("###,###,##0.000");

                    Literal litPrecoTecido = e.Row.FindControl("litPrecoTecido") as Literal;
                    litPrecoTecido.Text = "R$ " + pre.PRECO_TECIDO.ToString("###,###,###,##0.00");

                    Literal litPrecoVenda = e.Row.FindControl("litPrecoVenda") as Literal;
                    litPrecoVenda.Text = "R$ " + pre.PRECO_VENDA.ToString("###,###,###,##0.00");

                    Literal litConsumoTotal = e.Row.FindControl("litConsumoTotal") as Literal;
                    litConsumoTotal.Text = pre.CONSUMO_TOTAL.ToString("###,###,###,##0.000");

                    Literal litValorTecido = e.Row.FindControl("litValorTecido") as Literal;
                    litValorTecido.Text = "R$ " + (pre.PRECO_TECIDO * pre.CONSUMO_TOTAL).ToString("###,###,###,##0.00");

                    Literal litValorTotalAtacado = e.Row.FindControl("litValorTotalAtacado") as Literal;
                    litValorTotalAtacado.Text = "R$ " + pre.VALOR_TOTAL_ATACADO.ToString("###,###,###,##0.00");

                    Literal litValorTotalVarejo = e.Row.FindControl("litValorTotalVarejo") as Literal;
                    litValorTotalVarejo.Text = "R$ " + pre.VALOR_TOTAL_VAREJO.ToString("###,###,###,##0.00");

                    Literal litPedido = e.Row.FindControl("litPedido") as Literal;
                    litPedido.Text = (pre.NUMERO_PEDIDO == null) ? "-" : (pre.NUMERO_PEDIDO.ToString() + "-" + pre.ITEM.ToString());

                    ImageButton imgBtnAlterar = e.Row.FindControl("imgBtnAlterar") as ImageButton;
                    imgBtnAlterar.CommandArgument = pre.CODIGO.ToString();

                    ImageButton imgBtnCopiar = e.Row.FindControl("imgBtnCopiar") as ImageButton;
                    imgBtnCopiar.CommandArgument = pre.CODIGO.ToString();

                    ImageButton imgBtnExcluir = e.Row.FindControl("imgBtnExcluir") as ImageButton;
                    imgBtnExcluir.CommandArgument = pre.CODIGO.ToString();

                    if (pre.PRODUTO != "")
                        imgBtnExcluir.Visible = false;

                    gQtdeAtacado += pre.QTDE_ATACADO;
                    gQtdeVarejo += pre.QTDE_VAREJO;
                    gTotalAtacado += Convert.ToDecimal(pre.VALOR_TOTAL_ATACADO);
                    gTotalVarejo += Convert.ToDecimal(pre.VALOR_TOTAL_VAREJO);

                    gTotConsumo += Convert.ToDecimal(pre.CONSUMO);
                    gTotConsumoTotal += Convert.ToDecimal(pre.CONSUMO_TOTAL);

                    gTotTecido += (pre.PRECO_TECIDO * pre.CONSUMO_TOTAL);
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
                footer.Cells[4].Text = "Total";
                footer.Cells[14].Text = gTotConsumo.ToString("###,###,###,##0.000");
                footer.Cells[16].Text = gQtdeAtacado.ToString();
                footer.Cells[17].Text = gQtdeVarejo.ToString();
                footer.Cells[20].Text = gTotConsumoTotal.ToString("###,###,###,##0.000");
                footer.Cells[21].Text = "R$ " + gTotTecido.ToString("###,###,###,##0.00");
                footer.Cells[22].Text = "R$ " + gTotalAtacado.ToString("###,###,###,##0.00");
                footer.Cells[23].Text = "R$ " + gTotalVarejo.ToString("###,###,###,##0.00");
            }
        }
        protected void gvPrePedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PREPEDIDOResult> prePedido = ObterPrePedido();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prePedido = prePedido.OrderBy(e.SortExpression + sortDirection);
            gvPrePedido.DataSource = prePedido.ToList();
            gvPrePedido.DataBind();

            LimparAlteracao();
        }
        protected void gvPrePedido_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrePedido.PageIndex = e.NewPageIndex;
            CarregarPrePedido(ORIGEM_PREPEDIDO.FILTRO);

            LimparAlteracao();
        }
        protected void ddlPaginaNumero_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageSize = Convert.ToInt32(ddlPaginaNumero.SelectedValue);
            gvPrePedido.PageSize = pageSize;
            CarregarPrePedido(ORIGEM_PREPEDIDO.FILTRO);

            LimparAlteracao();
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

                //ddlFornecedor.Enabled = true;
                //ddlFornecedor.SelectedValue = "";
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
                //ddlFornecedor.Enabled = true;

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

                        //ddlFornecedor.SelectedValue = materialLinx.FORNECEDOR.Trim();
                        //ddlFornecedor.Enabled = false;

                        imgTecido.ImageUrl = materialLinx.FOTO_TECIDO;
                    }
                }
                else
                {
                    txtCorFornecedor.Text = "";
                    ddlCor.SelectedValue = "";
                    ddlUnidadeMedida.SelectedValue = "";
                    ddlFornecedor.SelectedValue = "";

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

                var corAnterior = "";
                var prePedido = new DESENV_PREPEDIDO();
                string codigo = hidCodigo.Value;
                bool inc = true;
                if (codigo != "")
                {
                    prePedido = desenvController.ObterPrePedido(Convert.ToInt32(codigo));
                    if (prePedido == null)
                    {
                        labErroInclusao.Text = "Produto não encontrado. Entrar em contato com TI.";
                        return;
                    }
                    inc = false;
                    corAnterior = prePedido.COR;
                }

                prePedido.MARCA = ddlMarca.SelectedValue;
                prePedido.COLECAO = ddlColecao.SelectedValue;

                prePedido.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue);

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
                prePedido.PROD_PARCIAL = Convert.ToChar(ddlProdParcial.SelectedValue);

                if (inc)
                {
                    desenvController.InserirPrePedido(prePedido);
                }
                else
                {
                    desenvController.AtualizarPrePedido(prePedido);
                    SalvarDESENVPRODUTO(prePedido, corAnterior);
                }

                CarregarPrePedido(ORIGEM_PREPEDIDO.FILTRO);

                LimparCampos();
                labErroInclusao.Text = "Produto incluído com Sucesso.";
            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }
        }
        private void SalvarDESENVPRODUTO(DESENV_PREPEDIDO prePedido, string corAnterior)
        {
            if (prePedido.PRODUTO != null)
            {
                var desenvProduto = desenvController.ObterProduto(prePedido.COLECAO, prePedido.PRODUTO, corAnterior);
                if (desenvProduto != null)
                {
                    desenvProduto.DESENV_PRODUTO_ORIGEM = prePedido.DESENV_PRODUTO_ORIGEM;
                    desenvProduto.TECIDO_POCKET = prePedido.TECIDO;
                    desenvProduto.FORNECEDOR = prePedido.FORNECEDOR;
                    desenvProduto.FORNECEDOR_COR = prePedido.COR_FORNECEDOR;
                    desenvProduto.COR = prePedido.COR;

                    var grupoDePara = desenvController.ObterGrupoProdutoPrePedidoDePara(prePedido.GRUPO_PRODUTO);
                    desenvProduto.GRUPO = grupoDePara.GRUPO_PRODUTO;
                    desenvProduto.GRIFFE = prePedido.GRIFFE;
                    desenvProduto.QTDE = prePedido.QTDE_VAREJO;
                    desenvProduto.QTDE_ATACADO = prePedido.QTDE_ATACADO;
                    desenvProduto.CONSUMO = prePedido.CONSUMO;
                    desenvProduto.MARCA = prePedido.MARCA;
                    if (prePedido.SIGNED_NOME != "")
                    {
                        desenvProduto.SIGNED = 'S';
                        desenvProduto.SIGNED_NOME = prePedido.SIGNED_NOME;
                    }

                    desenvController.AtualizarProduto(desenvProduto);
                }
            }
        }

        protected void imgBtnAlterar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroInclusao.Text = "";

                ImageButton b = (ImageButton)sender;
                string codigo = b.CommandArgument;

                var prePedido = desenvController.ObterPrePedido(Convert.ToInt32(codigo));
                if (prePedido != null)
                {
                    txtTecidoFiltro.Text = prePedido.TECIDO;
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

                    ddlFornecedor.SelectedValue = prePedido.FORNECEDOR;
                    ddlGriffe.SelectedValue = baseController.BuscaGriffes().Where(p => p.GRIFFE.Trim() == prePedido.GRIFFE.Trim()).SingleOrDefault().GRIFFE;
                    ddlGrupoPedido.SelectedValue = prePedido.GRUPO_PRODUTO;

                    ddlOrigem.SelectedValue = prePedido.DESENV_PRODUTO_ORIGEM.ToString();

                    txtConsumo.Text = prePedido.CONSUMO.ToString();
                    txtPrecoTecido.Text = prePedido.PRECO_TECIDO.ToString();
                    txtQtdeAtacado.Text = prePedido.QTDE_ATACADO.ToString();
                    txtQtdeVarejo.Text = prePedido.QTDE_VAREJO.ToString();
                    txtPrecoVenda.Text = prePedido.PRECO_VENDA.ToString();
                    txtConsumoTotal.Text = prePedido.CONSUMO_TOTAL.ToString();
                    txtValorTecido.Text = "R$ " + (prePedido.CONSUMO_TOTAL * prePedido.PRECO_TECIDO).ToString("###,###,###,###,##0.00");
                    txtTotalAtacado.Text = "R$ " + prePedido.VALOR_TOTAL_ATACADO.ToString();
                    txtTotalVarejo.Text = "R$ " + prePedido.VALOR_TOTAL_VAREJO.ToString();

                    txtSignedNome.Text = prePedido.SIGNED_NOME;
                    ddlProdParcial.SelectedValue = prePedido.PROD_PARCIAL.ToString();

                    hidCodigo.Value = codigo.ToString();
                    btIncluirPrePedido.Text = "Alterar";
                    btLimparTodos.Text = "Cancelar";

                }
            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }

        }
        protected void imgBtnCopiar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
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
                    txtValorTecido.Text = "R$ " + (prePedido.CONSUMO_TOTAL * prePedido.PRECO_TECIDO).ToString("###,###,###,###,##0.00");
                    txtTotalAtacado.Text = "R$ " + prePedido.VALOR_TOTAL_ATACADO.ToString();
                    txtTotalVarejo.Text = "R$ " + prePedido.VALOR_TOTAL_VAREJO.ToString();

                    txtSignedNome.Text = prePedido.SIGNED_NOME;

                    ddlProdParcial.SelectedValue = prePedido.PROD_PARCIAL.ToString();

                }

                LimparAlteracao();
            }
            catch (Exception)
            {
            }
        }

        protected void imgBtnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                labErro.Text = "";
                labErroInclusao.Text = "";

                ImageButton b = (ImageButton)sender;
                string codigo = b.CommandArgument;

                desenvController.ExcluirPrePedido(Convert.ToInt32(codigo));
                CarregarPrePedido(ORIGEM_PREPEDIDO.FILTRO);

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btResumo_Click(object sender, EventArgs e)
        {
            try
            {
                labErroInclusao.Text = "";

                Button b = (Button)sender;

                string marca = ddlMarca.SelectedValue;
                string colecao = ddlColecao.SelectedValue.Trim();
                string griffe = b.CommandArgument;
                string desenvOrigem = ddlOrigemFiltro.SelectedValue;

                //Abrir pop-up
                var url = "fnAbrirTelaCadastroMaior2('desenv_tripa_prepedido_resumo.aspx?m=" + marca + "&g=" + griffe + "&c=" + colecao + "&d=" + desenvOrigem + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);

            }
            catch (Exception ex)
            {
                labErroInclusao.Text = ex.Message;
            }
        }

    }
}

