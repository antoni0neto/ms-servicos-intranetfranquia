using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Relatorios.mod_desenvolvimento
{
    public partial class desenv_material : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        NotificacaoController notController = new NotificacaoController();
        int colunaCores = 0, colunaTodos = 0, colunaComposicao = 0, colunaNotificacao = 0;

        const string cAbaTodos = "6";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);

            if (!Page.IsPostBack)
            {
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

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                var tecido = "";
                if (Request.QueryString["tec"] != null && Request.QueryString["tec"] != "")
                    tecido = Request.QueryString["tec"].ToString().Trim();

                CarregarColecoes();
                CarregarGrupos();
                CarregarCores("");
                CarregarMaterialTipos();
                CarregarUnidadeLinx();
                CarregarFornecedores();
                CarregarClassifFiscal();
                CarregarTributOrigem();
                CarregarContaContabil();
                CarregarCaractContabil();
                CarregarTipoSped();
                CarregarLavagem();
                CarregarComposicao(txtMaterial.Text.Trim());
                CarregarNotificacao();

                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtLimpar.Visible = false;
                gvLavagem.Visible = false;
                gvLavagem.Enabled = false;
                btComposicaoIncluir.Visible = false;

                Session["MATERIAL_COR"] = null;
                Session["MATERIAL_COMP"] = null;
                Session["FOTO_TECIDO_PRINCIPAL"] = null;

                if (tecido != "")
                {
                    txtMaterialDescricao.Text = tecido;
                    //ibtPesquisar_Click(null, null);
                }

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoesMateriais();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }
        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo().Where(p => p.CODIGO_GRUPO.Trim() != "01").OrderBy(o => o.GRUPO.Trim()).ToList();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMaterialGrupo.SelectedValue.Trim() != "")
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).OrderBy(o => o.SUBGRUPO.Trim()).ToList();
            if (_matSubGrupo != null)
            {
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "Selecione" });
                ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                ddlMaterialSubGrupo.DataBind();
            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMaterialSubGrupo.SelectedValue.Trim() != "")
            {
                var _subGrupo = desenvController.ObterMaterialSubGrupo(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
                if (_subGrupo != null)
                {
                    ddlUnidadeMedida.SelectedValue = _subGrupo.UNIDADE_ESTOQUE;
                }
            }
        }
        private void CarregarMaterialTipos()
        {
            List<MATERIAIS_TIPO> _matTipo = desenvController.ObterMaterialTipo();
            if (_matTipo != null)
            {
                _matTipo.Insert(0, new MATERIAIS_TIPO { TIPO = "Selecione" });
                ddlTipoMaterial.DataSource = _matTipo;
                ddlTipoMaterial.DataBind();
            }
        }
        private void CarregarUnidadeLinx()
        {
            List<UNIDADE> _unidade = desenvController.ObterUnidadeMedidaLinx().Where(p => p.UNIDADE1.Trim() == "KG" || p.UNIDADE1.Trim() == "UN" || p.UNIDADE1.Trim() == "MT").OrderBy(o => o.UNIDADE1.Trim()).ToList();
            if (_unidade != null)
            {
                _unidade.Insert(0, new UNIDADE { UNIDADE1 = "", DESC_UNIDADE = "Selecione" });
                ddlUnidadeMedida.DataSource = _unidade;
                ddlUnidadeMedida.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().Where(p => (p.STATUS == 'A' && (p.TIPO == 'T' || p.TIPO == 'A')) || p.STATUS == 'S').ToList();

            if (_fornecedores != null)
            {
                _fornecedores = _fornecedores.GroupBy(p => new { FORNECEDOR = p.FORNECEDOR }).Select(x => new PROD_FORNECEDOR { FORNECEDOR = x.Key.FORNECEDOR }).OrderBy(p => p.FORNECEDOR).ToList();
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarClassifFiscal()
        {
            List<CLASSIF_FISCAL> _classifFiscal = desenvController.ObterClassificacaoFiscal();
            if (_classifFiscal != null)
            {
                List<CLASSIF_FISCAL> _classifFiscalAux = new List<CLASSIF_FISCAL>();
                foreach (CLASSIF_FISCAL cf in _classifFiscal)
                    if (cf != null)
                        _classifFiscalAux.Add(new CLASSIF_FISCAL { CLASSIF_FISCAL1 = cf.CLASSIF_FISCAL1, DESC_CLASSIFICACAO = cf.CLASSIF_FISCAL1.Trim() + " - " + cf.DESC_CLASSIFICACAO.Trim() });

                _classifFiscalAux.Insert(0, new CLASSIF_FISCAL { CLASSIF_FISCAL1 = "", DESC_CLASSIFICACAO = "Selecione" });

                ddlClassifFiscal.DataSource = _classifFiscalAux;
                ddlClassifFiscal.DataBind();

                ddlClassifFiscal.SelectedValue = "0000000000";
            }
        }
        private void CarregarTributOrigem()
        {
            List<TRIBUT_ORIGEM> _tributFiscal = desenvController.ObterTributacaoOrigem();
            if (_tributFiscal != null)
            {
                ddlOrigem.DataSource = _tributFiscal;
                ddlOrigem.DataBind();

                ddlOrigem.SelectedValue = "0  ";
            }
        }
        private void CarregarContaContabil()
        {
            List<CTB_CONTA_PLANO> _contaContabil = ObterContaContabil();
            if (_contaContabil != null)
            {

                ddlContaContabilEstoque1.DataSource = _contaContabil.Where(p => p.CONTA_CONTABIL.Trim() == "1.1.5.01.0002" || p.CONTA_CONTABIL.Trim() == "1.1.5.01.0003");
                ddlContaContabilEstoque1.DataBind();
                //ddlContaContabilEstoque1.SelectedValue = "11401003            ";

                ddlContaContabilCompra1.DataSource = _contaContabil.Where(p => p.CONTA_CONTABIL.Trim() == "1.1.5.01.0002" || p.CONTA_CONTABIL.Trim() == "1.1.5.01.0003");
                ddlContaContabilCompra1.DataBind();
                //ddlContaContabilCompra1.SelectedValue = "11401003            ";

                ddlContaContabilCompra2.DataSource = _contaContabil.Where(p => p.CONTA_CONTABIL.Trim() == "1.1.5.01.0002" || p.CONTA_CONTABIL.Trim() == "1.1.5.01.0003");
                ddlContaContabilCompra2.DataBind();
                //ddlContaContabilCompra2.SelectedValue = "11401003            ";

                ddlContaContabilVenda1.DataSource = _contaContabil;
                ddlContaContabilVenda1.DataBind();
                ddlContaContabilVenda1.SelectedValue = "4.1.1.01.0002       ";

                ddlContaContabilVenda2.DataSource = _contaContabil;
                ddlContaContabilVenda2.DataBind();
                ddlContaContabilVenda2.SelectedValue = "4.1.2.01.0001       ";

                RecarregarContaContabil();
            }
        }
        private void RecarregarContaContabil()
        {
            ddlContaContabil(ddlContaContabilEstoque1, null);
            ddlContaContabil(ddlContaContabilCompra1, null);
            ddlContaContabil(ddlContaContabilCompra2, null);
            ddlContaContabil(ddlContaContabilVenda1, null);
            ddlContaContabil(ddlContaContabilVenda2, null);
        }
        protected void ddlContaContabil(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                string conta_contabil = "";
                conta_contabil = ddl.SelectedValue.Trim();

                var ctbConta = desenvController.ObterContaContabil(conta_contabil);
                if (ctbConta != null)
                {
                    //ESTOQUE
                    if (ddl.ID == "ddlContaContabilEstoque1")
                        txtContaContabilEstoque1.Text = ctbConta.DESC_CONTA.Trim();
                    //COMPRA 1
                    if (ddl.ID == "ddlContaContabilCompra1")
                        txtContaContabilCompra1.Text = ctbConta.DESC_CONTA.Trim();
                    //VENDA 1
                    if (ddl.ID == "ddlContaContabilVenda1")
                        txtContaContabilVenda1.Text = ctbConta.DESC_CONTA.Trim();
                    //COMPRA 2
                    if (ddl.ID == "ddlContaContabilCompra2")
                        txtContaContabilCompra2.Text = ctbConta.DESC_CONTA.Trim();
                    //VENDA 2
                    if (ddl.ID == "ddlContaContabilVenda2")
                        txtContaContabilVenda2.Text = ctbConta.DESC_CONTA.Trim();
                }
            }
        }
        private void CarregarCaractContabil()
        {
            List<CTB_LX_INDICADOR_CFOP> _caractContabil = desenvController.ObterCaractContabil();
            if (_caractContabil != null)
            {
                List<CTB_LX_INDICADOR_CFOP> _caractContabilAux = new List<CTB_LX_INDICADOR_CFOP>();
                foreach (CTB_LX_INDICADOR_CFOP cc in _caractContabil)
                    if (cc != null)
                        _caractContabilAux.Add(new CTB_LX_INDICADOR_CFOP { INDICADOR_CFOP = cc.INDICADOR_CFOP, DESCRICAO_INDICADOR_CFOP = cc.INDICADOR_CFOP.ToString() + " - " + cc.DESCRICAO_INDICADOR_CFOP.Trim() });

                ddlCaractContabil.DataSource = _caractContabilAux;
                ddlCaractContabil.DataBind();

                ddlCaractContabil.SelectedValue = "12";
            }
        }
        private void CarregarTipoSped()
        {
            List<LCF_LX_ITEM_TIPO> _tipoSped = desenvController.ObterTipoSped();
            if (_tipoSped != null)
            {
                ddlTipoSped.DataSource = _tipoSped;
                ddlTipoSped.DataBind();

                ddlTipoSped.SelectedValue = "01";
            }
        }

        private List<CTB_CONTA_PLANO> ObterContaContabil()
        {
            List<CTB_CONTA_PLANO> _contaContabil = new List<CTB_CONTA_PLANO>();
            _contaContabil = desenvController.ObterContaContabil();
            return _contaContabil;
        }
        private List<CORES_BASICA> ObterCoresBasicas()
        {
            List<CORES_BASICA> _cores = new List<CORES_BASICA>();
            _cores = prodController.ObterCoresBasicas();
            return _cores;
        }
        #endregion

        #region "AÇÕES DA TELA"
        protected void ibtIncluir_Click(object sender, EventArgs e)
        {
            //controle de visibilidade dos botões da tela
            ibtIncluir.Visible = false;
            ibtEditar.Visible = false;
            ibtExcluir.Visible = false;
            ibtPesquisar.Visible = false;
            ibtLimpar.Visible = false;
            gvLavagem.Visible = false;
            gvLavagem.Enabled = false;

            //Controle de campos
            HabilitarCampos(true);
            LimparCampos();

            hidAcao.Value = "I";
            txtMaterial.Text = "";
            ibtSalvar.Visible = true;
            ibtCancelar.Visible = true;
            txtMaterial.Enabled = false;

            CarregarCores(txtMaterial.Text.Trim());
            CarregarComposicao(txtMaterial.Text.Trim());
            CarregarContaContabil();

            MoverAba("0");
            Session["MATERIAL_COR"] = null;
            Session["MATERIAL_COMP"] = null;
        }
        protected void ibtEditar_Click(object sender, EventArgs e)
        {
            try
            {
                Session["MATERIAL_COR"] = null;
                Session["MATERIAL_COMP"] = null;

                //controle de visibilidade dos botões da tela
                ibtIncluir.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtPesquisar.Visible = false;
                ibtLimpar.Visible = false;
                gvLavagem.Visible = true;
                gvLavagem.Enabled = true;

                ibtSalvar.Visible = true;
                ibtCancelar.Visible = true;

                labErro.Text = "";
                HabilitarCampos(true);
                txtMaterial.Enabled = false;
                ddlUnidadeMedida.Enabled = false;
                ddlMaterialGrupo.Enabled = false;
                ddlMaterialSubGrupo.Enabled = false;
                hidAcao.Value = "A";

                gvCores_DataBound(null, null);
                CarregarLavagemSelecionada(txtMaterial.Text.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            Session["MATERIAL_COR"] = null;
            Session["MATERIAL_COMP"] = null;
            txtComposicaoQtde.Text = "";
            txtComposicaoDescricao.Text = "";

            labErro.Text = "";
            try
            {
                /*if (txtMaterial.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Código do Material.";
                    return;
                }*/

                string material = txtMaterial.Text.Trim();

                MATERIAI _material = desenvController.ObterMaterial(material);
                if (_material != null)
                {

                    //if (Convert.ToInt32(material.Substring(0, 2)) <= 5)
                    //{
                    //    labErro.Text = "Este material não pode ser alterado. Material Antigo.";
                    //    return;
                    //}

                    CarregarMaterial(_material);
                    CarregarCores(material);

                    ibtIncluir.Visible = true;
                    ibtPesquisar.Visible = false;
                    ibtSalvar.Visible = false;
                    ibtCancelar.Visible = false;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;
                    gvLavagem.Enabled = false;
                    gvLavagem.Visible = true;
                    CarregarLavagem();
                    CarregarLavagemSelecionada(material);

                    ibtExcluir.Visible = true;
                    hidAcao.Value = "C";

                    HabilitarCampos(false);
                    gvCores_DataBound(null, null);

                    MoverAba("0");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + cAbaTodos + "');", true);
                    hidTabSelected.Value = cAbaTodos;
                    CarregarMaterialTodos();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ibtPesquisar.Visible = true;
                ibtEditar.Visible = false;
                ibtLimpar.Visible = false;
                CarregarLavagem();
                gvLavagem.Visible = false;
                gvLavagem.Enabled = false;
                ibtExcluir.Visible = false;
                hidAcao.Value = "";

                HabilitarCampos(true);
                LimparCampos();

                Session["MATERIAL_COR"] = null;
                CarregarCores(txtMaterial.Text.Trim());
                Session["MATERIAL_COMP"] = null;
                CarregarComposicao(txtMaterial.Text.Trim());
                //CarregarMaterialTodos();
                CarregarContaContabil();
                MoverAba("0");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtSalvar_Click(object sender, EventArgs e)
        {
            //Validar campos obrigatórios
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return;
                }

                gvLavagem.Enabled = true;

                MATERIAI _material = new MATERIAI();
                _material = desenvController.ObterMaterial(txtMaterial.Text.Trim());
                if (_material == null)
                {
                    _material = new MATERIAI();
                    _material.MATERIAL = ObterMaterialCodigo(ddlMaterialGrupo.SelectedValue.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim(), "");
                }
                _material.DESC_MATERIAL = txtMaterialDescricao.Text.Trim().ToUpper();
                _material.SUBGRUPO = ddlMaterialSubGrupo.SelectedItem.Text.Trim().ToUpper();
                _material.GRUPO = ddlMaterialGrupo.SelectedItem.Text.Trim().ToUpper();
                _material.TIPO = ddlTipoMaterial.SelectedValue.Trim();
                _material.FASE_PRODUCAO = "01";
                _material.SETOR_PRODUCAO = "01";
                _material.TRIBUT_ICMS = "00";
                _material.TRIBUT_ORIGEM = ddlOrigem.SelectedValue;
                _material.CLASSIF_FISCAL = ddlClassifFiscal.SelectedValue;
                _material.FABRICANTE = ddlFornecedor.SelectedItem.Text.Trim().ToUpper();
                _material.COLECAO = ddlColecoes.SelectedValue.Trim();
                _material.FATOR_CONVERSAO = 1;
                _material.UNID_ESTOQUE = ddlUnidadeMedida.SelectedValue.Trim();
                _material.UNID_FICHA_TEC = ddlUnidadeMedida.SelectedValue.Trim();

                var condicaoFornecedor = prodController.ObterFornecedores("T").Where(p => p.FORNECEDOR.Trim() == ddlFornecedor.SelectedValue.Trim()).Take(1).SingleOrDefault();
                if (condicaoFornecedor != null)
                    _material.CONDICAO_PGTO = condicaoFornecedor.CONDICAO_PGTO;
                else
                    _material.CONDICAO_PGTO = "01";
                _material.CTRL_UNID_AUX = false;
                _material.COD_MATERIAL_TAMANHO = "";
                _material.VARIA_MATERIAL_TAMANHO = false;
                _material.CTRL_PARTIDAS = false;
                _material.CTRL_PECAS = false;
                _material.CTRL_PECAS_PARCIAL = false;
                _material.DATA_REPOSICAO = DateTime.Now;
                _material.CUSTO_A_VISTA = 0;
                _material.TAXA_JUROS_CUSTO = 0;
                _material.MATERIAL_INDIRETO = false;
                _material.RESERVA_MATERIAL_OP = false;
                _material.ABATER_RESERVA_QTDE = false;
                _material.CONTA_CONTABIL = ddlContaContabilEstoque1.SelectedValue;
                _material.COMPOSICAO = "000000";
                _material.DATA_CADASTRAMENTO = DateTime.Now;
                _material.INDICADOR_CFOP = Convert.ToByte(ddlCaractContabil.SelectedValue);
                _material.MRP_AGRUPAR_NECESSIDADE_TIPO = 0;
                _material.CONTA_CONTABIL_COMPRA = ddlContaContabilCompra1.SelectedValue;
                _material.CONTA_CONTABIL_VENDA = ddlContaContabilVenda1.SelectedValue;
                _material.CONTA_CONTABIL_DEV_COMPRA = ddlContaContabilCompra2.SelectedValue;
                _material.CONTA_CONTABIL_DEV_VENDA = ddlContaContabilVenda2.SelectedValue;
                _material.TIPO_ITEM_SPED = ddlTipoSped.SelectedValue;

                if (hidAcao.Value == "I")
                    desenvController.InserirMaterial(_material);
                else
                    desenvController.AtualizarMaterial(_material);

                //CORES E FORNECEDORES
                if (Session["MATERIAL_COR"] != null)
                {
                    txtMaterial.Text = _material.MATERIAL;

                    MATERIAIS_FORNECEDOR _materialFornecedor = null;
                    MATERIAIS_CORE _materialCor = null;

                    //LOOP NA SESSION PARA INSERIR APENAS OS NOVOS//
                    List<MATERIAIS_CORE> listaMaterialCor = new List<MATERIAIS_CORE>();
                    listaMaterialCor = (List<MATERIAIS_CORE>)Session["MATERIAL_COR"];
                    foreach (MATERIAIS_CORE mat in listaMaterialCor)
                    {
                        //Label _labCor = row.FindControl("labCor") as Label;
                        //Label _labRefCor = row.FindControl("labRefCor") as Label;
                        //string _codigoCor = gvCores.DataKeys[row.RowIndex].Value.ToString();
                        if (mat != null)
                        {
                            _materialCor = new MATERIAIS_CORE();
                            _materialCor.MATERIAL = _material.MATERIAL;
                            _materialCor.COR_MATERIAL = mat.COR_MATERIAL;
                            _materialCor.DESC_COR_MATERIAL = mat.DESC_COR_MATERIAL;
                            _materialCor.REFER_FABRICANTE = mat.REFER_FABRICANTE;
                            _materialCor.CUSTO_REPOSICAO = 0;
                            _materialCor.CUSTO_A_VISTA = 0;
                            _materialCor.VARIANTE_DESENHO = "";
                            _materialCor.COMPOSICAO = "000000";
                            _materialCor.CLASSIF_FISCAL = "0000000000"; // PENDENTE TRIBUTACAO
                            _materialCor.VERSAO_FICHA = "00001";
                            desenvController.InserirMaterialCor(_materialCor);

                            _materialFornecedor = new MATERIAIS_FORNECEDOR();
                            _materialFornecedor.FORNECEDOR = ddlFornecedor.SelectedValue.Trim();
                            _materialFornecedor.MATERIAL = _material.MATERIAL;
                            _materialFornecedor.COR_MATERIAL = mat.COR_MATERIAL;
                            _materialFornecedor.REF_FORNECEDOR = "";
                            _materialFornecedor.DESC_FORNECEDOR = txtMaterialDescricao.Text.Trim().ToUpper();
                            _materialFornecedor.REF_COR_FORNECEDOR = " ";
                            _materialFornecedor.DESC_COR_FORNECEDOR = mat.DESC_COR_MATERIAL;
                            _materialFornecedor.UNIDADE = ddlUnidadeMedida.SelectedValue.Trim();
                            _materialFornecedor.FATOR_CONVERSAO_FORNECEDOR = 1;
                            desenvController.InserirMaterialFornecedor(_materialFornecedor);
                        }
                    }
                }
                /************************************************************************************/

                //COMPOSICAO
                if (Session["MATERIAL_COMP"] != null)
                {
                    txtMaterial.Text = _material.MATERIAL;

                    MATERIAL_COMPOSICAO _materialComposicao = null;

                    //LOOP NA SESSION PARA INSERIR APENAS OS NOVOS//
                    List<MATERIAL_COMPOSICAO> listaMaterialComposicao = new List<MATERIAL_COMPOSICAO>();
                    listaMaterialComposicao = (List<MATERIAL_COMPOSICAO>)Session["MATERIAL_COMP"];
                    foreach (MATERIAL_COMPOSICAO comp in listaMaterialComposicao)
                    {
                        if (comp != null)
                        {
                            _materialComposicao = new MATERIAL_COMPOSICAO();
                            _materialComposicao.QTDE = comp.QTDE;
                            _materialComposicao.DESCRICAO = comp.DESCRICAO;
                            _materialComposicao.MATERIAL = txtMaterial.Text.Trim();
                            desenvController.InserirMaterialComposicao(_materialComposicao);
                        }
                    }
                }
                /************************************************************************************/

                //FOTO
                bool _inserirFoto = false;
                MATERIAL_FOTO _materialFoto = null;
                _materialFoto = desenvController.ObterMaterialFotoIntranet(_material.MATERIAL);
                if (_materialFoto == null)
                {
                    _materialFoto = new MATERIAL_FOTO();
                    _inserirFoto = true;
                }
                _materialFoto.MATERIAL = _material.MATERIAL;
                _materialFoto.FOTO = imgFotoTecido.ImageUrl;
                if (_inserirFoto)
                    desenvController.InserirMaterialFotoIntranet(_materialFoto);
                else
                    desenvController.AtualizarMaterialFotoIntranet(_materialFoto);
                /************************************************************************************/

                //Entrar em modo de CONSULTA
                ibtPesquisar_Click(null, null);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos() && hidAcao.Value == "A")
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return;
                }

                //controle de visibilidade dos botões da tela
                ibtIncluir.Visible = true;
                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                btComposicaoIncluir.Visible = false;
                CarregarLavagem();
                gvLavagem.Enabled = false;

                System.Drawing.Color _OK = System.Drawing.Color.Gray;
                labMaterialDescricao.ForeColor = _OK;
                labColecao.ForeColor = _OK;
                labGrupo.ForeColor = _OK;
                labSubGrupo.ForeColor = _OK;
                labTipoMaterial.ForeColor = _OK;
                labUnidadeMedida.ForeColor = _OK;
                labFornecedor.ForeColor = _OK;
                labCores.ForeColor = _OK;
                labClassifFiscal.ForeColor = _OK;

                if (hidAcao.Value == "I")
                {
                    ibtLimpar_Click(null, null);
                    txtMaterial.Text = "";
                    txtMaterial.Enabled = true;

                    ibtPesquisar.Visible = true;

                    CarregarContaContabil();
                }

                if (hidAcao.Value == "A")
                {
                    ibtPesquisar_Click(null, null);
                    ibtPesquisar.Visible = false;
                    ibtExcluir.Visible = true;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;

                    gvCores_DataBound(null, null);
                }
                CarregarLavagemSelecionada(txtMaterial.Text.Trim());
                hidAcao.Value = "";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (txtMaterial.Text.Trim() == "")
                {
                    labErro.Text = "Material não informado. Refaça sua pesquisa.";
                }
                else
                {
                    MATERIAI _material = null;
                    _material = desenvController.ObterMaterial(txtMaterial.Text.Trim());

                    //Validar exclusao do material
                    var _estoqueMaterial = desenvController.ObterMaterialEstoque(txtMaterial.Text.Trim()).Where(p => p.QTDE_ESTOQUE != 0 && p.QTDE_ESTOQUE_AUX != 0);
                    if (_estoqueMaterial != null && _estoqueMaterial.Count() > 0)
                    {
                        labErro.Text = "Material não pode ser excluído. Possui movimento em Estoque.";
                        return;
                    }

                    var _fichaTecnica = desenvController.ObterFichaTecnica().Where(p => p.MATERIAL.Trim() == txtMaterial.Text.Trim());
                    if (_fichaTecnica != null && _fichaTecnica.Count() > 0)
                    {
                        labErro.Text = "Material não pode ser excluído. Possui Ficha Técnica.";
                        return;
                    }

                    List<MATERIAIS_CORE> _matCores = desenvController.ObterMaterialCor().Where(p => p.MATERIAL.Trim() == _material.MATERIAL.Trim()).ToList();
                    foreach (MATERIAIS_CORE matCor in _matCores)
                    {
                        if (matCor != null)
                        {
                            desenvController.ExcluirMaterialFornecedor(_material.MATERIAL, matCor.COR_MATERIAL);
                            desenvController.ExcluirMaterialCor(_material.MATERIAL, matCor.COR_MATERIAL);
                        }
                    }
                    List<MATERIAL_COMPOSICAO> _matComposicao = desenvController.ObterMaterialComposicao(_material.MATERIAL.Trim());
                    foreach (MATERIAL_COMPOSICAO matComp in _matComposicao)
                    {
                        if (matComp != null)
                        {
                            desenvController.ExcluirMaterialComposicao(matComp.CODIGO);
                        }
                    }

                    desenvController.ExcluirMaterialFotoIntranet(_material.MATERIAL);
                    desenvController.ExcluirMaterial(_material.MATERIAL);

                    ibtLimpar_Click(null, null);
                    MoverAba("0");
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        private void CarregarMaterial(MATERIAI _material)
        {
            txtMaterialDescricao.Text = _material.DESC_MATERIAL.Trim();

            //MATÉRIA-PRIMA
            ddlColecoes.SelectedValue = _material.COLECAO;
            ddlMaterialGrupo.SelectedValue = desenvController.ObterMaterialGrupo(_material.GRUPO).CODIGO_GRUPO;
            CarregarSubGrupos(_material.GRUPO);
            ddlMaterialSubGrupo.SelectedValue = desenvController.ObterMaterialSubGrupo(_material.GRUPO, _material.SUBGRUPO).CODIGO_SUBGRUPO;
            ddlTipoMaterial.SelectedValue = _material.TIPO.Trim();
            ddlUnidadeMedida.SelectedValue = _material.UNID_ESTOQUE;
            ddlFornecedor.SelectedValue = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == _material.FABRICANTE.Trim()).Take(1).SingleOrDefault().FORNECEDOR;

            //TRIBUTAÇÕES
            ddlClassifFiscal.SelectedValue = _material.CLASSIF_FISCAL;
            ddlOrigem.SelectedValue = _material.TRIBUT_ORIGEM;
            ddlContaContabilEstoque1.SelectedValue = desenvController.ObterContaContabil(_material.CONTA_CONTABIL).CONTA_CONTABIL;
            ddlContaContabilCompra1.SelectedValue = desenvController.ObterContaContabil(_material.CONTA_CONTABIL_COMPRA).CONTA_CONTABIL;
            ddlContaContabilCompra2.SelectedValue = desenvController.ObterContaContabil(_material.CONTA_CONTABIL_DEV_COMPRA).CONTA_CONTABIL;
            ddlContaContabilVenda1.SelectedValue = desenvController.ObterContaContabil(_material.CONTA_CONTABIL_VENDA).CONTA_CONTABIL;
            ddlContaContabilVenda2.SelectedValue = desenvController.ObterContaContabil(_material.CONTA_CONTABIL_DEV_VENDA).CONTA_CONTABIL;
            ddlCaractContabil.SelectedValue = _material.INDICADOR_CFOP.ToString();
            ddlTipoSped.SelectedValue = _material.TIPO_ITEM_SPED.Trim();
            RecarregarContaContabil();

            //COMPOSICAO
            CarregarComposicao(txtMaterial.Text.Trim());
            hidTotalComp.Value = "";
            if (gvComposicao.FooterRow != null)
                hidTotalComp.Value = gvComposicao.FooterRow.Cells[1].Text.ToString();

            //FOTO
            CarregarMaterialFoto(_material.MATERIAL);

        }
        private string ObterMaterialCodigo(string grupo, string subgrupo, string sequencial)
        {
            string retorno = grupo + "." + subgrupo + ".";

            if (sequencial == "")
            {
                sequencial = "0001";
                var _material = desenvController.ObterMaterial().Where(p => p.MATERIAL.Trim().Substring(0, 5) == (grupo + "." + subgrupo)).OrderByDescending(x => x.MATERIAL);
                if (_material != null && _material.Count() > 0)
                {
                    var _materialSeq = _material.Take(1).SingleOrDefault();
                    int seqAux = Convert.ToInt32(_materialSeq.MATERIAL.Substring(6)) + 1;
                    sequencial = seqAux.ToString().PadLeft(4, '0');
                }
            }

            return (retorno + sequencial);
        }
        #endregion

        #region "MATERIA-PRIMA"
        //CORES
        private decimal ObterMaterialEstoqueQtde(string material, string cor)
        {
            decimal retorno = 0;
            var qtdeEstoque = desenvController.ObterMaterialEstoque(material, cor);
            if (qtdeEstoque != null && qtdeEstoque.Count() > 0)
            {
                retorno = qtdeEstoque.Sum(p => p.QTDE_ESTOQUE).Value;
            }

            return retorno;
        }
        private void CarregarCores(string material)
        {
            List<MATERIAIS_CORE> _matCores = desenvController.ObterMaterialCor().Where(p => p.MATERIAL.Trim() == material.ToUpper().Trim()).ToList();

            if (Session["MATERIAL_COR"] != null)
                _matCores.AddRange((List<MATERIAIS_CORE>)Session["MATERIAL_COR"]);

            if (_matCores == null || _matCores.Count <= 0)
                _matCores.Add(new MATERIAIS_CORE { MATERIAL = "", COR_MATERIAL = "", REFER_FABRICANTE = "" });

            gvCores.DataSource = _matCores.OrderBy(p => p.DESC_COR_MATERIAL);
            gvCores.DataBind();
        }
        protected void gvCores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MATERIAIS_CORE _matCor = e.Row.DataItem as MATERIAIS_CORE;

                    colunaCores += 1;
                    if (_matCor != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaCores.ToString();

                        DropDownList _ddlCor = e.Row.FindControl("ddlCor") as DropDownList;
                        if (_ddlCor != null)
                        {
                            _ddlCor.DataSource = ObterCoresBasicas();
                            _ddlCor.DataBind();

                            _ddlCor.SelectedValue = prodController.ObterCoresBasicas(_matCor.COR_MATERIAL).COR;
                        }

                        TextBox _txtRefCor = e.Row.FindControl("txtRefCor") as TextBox;
                        if (_txtRefCor != null)
                        {
                            _txtRefCor.Text = _matCor.REFER_FABRICANTE.Trim();
                        }

                        Label _labQtdeEstoque = e.Row.FindControl("labQtdeEstoque") as Label;
                        if (_labQtdeEstoque != null)
                        {
                            decimal totalEstoque = 0;
                            if (_matCor.MATERIAL != null && _matCor.COR_MATERIAL != null)
                                totalEstoque = ObterMaterialEstoqueQtde(_matCor.MATERIAL, _matCor.COR_MATERIAL);
                            _labQtdeEstoque.Text = totalEstoque.ToString("###,###,###,##0.00");
                        }

                        ImageButton _ibtEditar = e.Row.FindControl("btEditarCor") as ImageButton;
                        ImageButton _ibtSalvarCor = e.Row.FindControl("btSalvarCor") as ImageButton;
                        ImageButton _ibtSair = e.Row.FindControl("btSairCor") as ImageButton;
                        ImageButton _ibtExcluir = e.Row.FindControl("btExcluirCor") as ImageButton;
                        _ibtEditar.Visible = false;
                        _ibtSalvarCor.Visible = false;
                        _ibtSair.Visible = false;
                        _ibtExcluir.Visible = false;
                        if (_matCor.COR_MATERIAL.Trim() != "")
                        {
                            _ibtExcluir.Visible = true;
                            _ibtExcluir.CommandArgument = _matCor.MATERIAL + "|" + _matCor.COR_MATERIAL;

                            if (e.Row.RowState.ToString().ToUpper().Contains("EDIT"))
                            {
                                _ibtSalvarCor.Visible = true;
                                _ibtSair.Visible = true;
                            }
                            else
                            {
                                _ibtEditar.Visible = true;
                            }
                        }
                    }
                }
            }

        }
        protected void gvCores_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCores.FooterRow;
            if (footer != null)
            {
                DropDownList _ddlCorFooter = footer.FindControl("ddlCorFooter") as DropDownList;
                if (_ddlCorFooter != null)
                {
                    _ddlCorFooter.DataSource = ObterCoresBasicas();
                    _ddlCorFooter.DataBind();
                }

                ImageButton ibtIncluirCor = footer.FindControl("btIncluirCor") as ImageButton;
                if (ibtIncluirCor != null)
                {
                    ibtIncluirCor.Visible = false;
                    if (hidAcao.Value == "I" || hidAcao.Value == "A")
                        ibtIncluirCor.Visible = true;
                }
            }
        }
        protected void btEditarCor_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {

                        //Obter código da COR
                        string codigoCor = gvCores.DataKeys[row.RowIndex].Value.ToString();
                        var _fichaTecnica = desenvController.ObterFichaTecnicaPorMaterial(txtMaterial.Text.Trim(), codigoCor.Trim());
                        if (_fichaTecnica != null && _fichaTecnica.Count() > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui movimento. Não será possível Alterar.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        gvCores.EditIndex = row.RowIndex;
                        CarregarCores(txtMaterial.Text.Trim());

                        //Botão acionado é escondido
                        bt.Visible = false;

                        ImageButton _btSairCor = row.FindControl("btSairCor") as ImageButton;
                        if (_btSairCor != null)
                            _btSairCor.Visible = true;

                        ImageButton _btSalvarCor = row.FindControl("btSalvarCor") as ImageButton;
                        if (_btSalvarCor != null)
                            _btSalvarCor.Visible = true;

                        Label _labMaterialCor = row.FindControl("labCor") as Label;
                        Label _labRefCor = row.FindControl("labRefCor") as Label;

                        hidDescricaoCor.Value = _labMaterialCor.Text.Trim();
                        hidReferFab.Value = _labRefCor.Text.Trim();

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btEditarCor_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSalvarCor_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {

                        string codigoCor = "";
                        DropDownList _ddlMaterialCor = row.FindControl("ddlCor") as DropDownList;
                        TextBox _txtRefCor = row.FindControl("txtRefCor") as TextBox;

                        if (!ValidarCores(_ddlMaterialCor.SelectedItem.Text.Trim()))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já foi inserida neste Material', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (_txtRefCor == null || _txtRefCor.Text.Trim() == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Informe a Descrição do Fornecedor.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        List<MATERIAIS_CORE> matCores = new List<MATERIAIS_CORE>();
                        if (Session["MATERIAL_COR"] != null)
                            matCores = (List<MATERIAIS_CORE>)Session["MATERIAL_COR"];

                        //Obter código da COR
                        codigoCor = gvCores.DataKeys[row.RowIndex].Value.ToString();

                        MATERIAIS_CORE matCor = new MATERIAIS_CORE();
                        matCor = desenvController.ObterMaterialCor(txtMaterial.Text.Trim(), codigoCor);
                        if (matCor == null)
                        {
                            int _index = matCores.FindIndex(p => p.DESC_COR_MATERIAL.Trim() == hidDescricaoCor.Value.Trim() && p.REFER_FABRICANTE.Trim() == hidReferFab.Value.Trim());
                            matCores.RemoveAt(_index);
                        }
                        else
                        {
                            //valida movimento linx
                            if (desenvController.ValidarExclusaoMaterial(txtMaterial.Text.Trim(), codigoCor) == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui movimento de Nota Fiscal. Não será possível Alterar.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }

                            //valida movimento intranet
                            var _fichaTecnica = desenvController.ObterFichaTecnicaPorMaterial(txtMaterial.Text.Trim(), codigoCor.Trim());
                            if (_fichaTecnica != null && _fichaTecnica.Count() > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR possui Ficha Técnica. Não será possível Alterar.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }

                            //VALIDAR HB - PENDENTE
                            var _prodHB = prodController.ObterHB().Where(p => p.GRUPO_TECIDO.Trim() == ddlMaterialGrupo.SelectedItem.Text.Trim() &&
                                                                                p.TECIDO.Trim() == ddlMaterialSubGrupo.SelectedItem.Text.Trim() &&
                                                                                p.COR.Trim() == codigoCor.Trim() &&
                                                                                p.COR_FORNECEDOR.Trim() == _txtRefCor.Text.Trim());
                            if (_prodHB != null && _prodHB.Count() > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui HB. Não será possível Alterar.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }

                            //VALIDAR PEDIDO
                            var _pedido = desenvController.ObterPedido1000().Where(p => p.GRUPO.Trim() == ddlMaterialGrupo.SelectedItem.Text.Trim() &&
                                                                                p.SUBGRUPO.Trim() == ddlMaterialSubGrupo.SelectedItem.Text.Trim() &&
                                                                                p.COR.Trim() == codigoCor.Trim() &&
                                                                                p.COR_FORNECEDOR.Trim() == _txtRefCor.Text.Trim());
                            if (_pedido != null && _pedido.Count() > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui PEDIDO. Não será possível Alterar.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }

                            desenvController.ExcluirMaterialFornecedor(txtMaterial.Text.Trim(), codigoCor);
                            desenvController.ExcluirMaterialCor(txtMaterial.Text.Trim(), codigoCor);
                        }

                        matCor = new MATERIAIS_CORE();
                        matCor.COR_MATERIAL = _ddlMaterialCor.SelectedValue.Trim();
                        matCor.DESC_COR_MATERIAL = _ddlMaterialCor.SelectedItem.Text.Trim();
                        matCor.REFER_FABRICANTE = _txtRefCor.Text.Trim().ToUpper();
                        matCores.Add(matCor);

                        Session["MATERIAL_COR"] = matCores;
                        //CarregarCores(txtMaterial.Text.Trim());

                        ImageButton ibtSair = row.FindControl("btSairCor") as ImageButton;
                        btSairCor_Click(ibtSair, null);
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btSalvarCor_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSairCor_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {
                        gvCores.EditIndex = -1;
                        CarregarCores(txtMaterial.Text.Trim());

                        //Botão acionado é escondido
                        bt.Visible = false;

                        hidDescricaoCor.Value = "";
                        hidReferFab.Value = "";

                        ImageButton _btEditar = row.FindControl("btEditar") as ImageButton;
                        if (_btEditar != null)
                            _btEditar.Visible = true;

                        ImageButton _btSalvarCor = row.FindControl("btSalvarCor") as ImageButton;
                        if (_btSalvarCor != null)
                            _btSalvarCor.Visible = false;

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btSairCor_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btExcluirCor_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {
                        string codigoCor = "";
                        string materialCor = "";
                        string refFabricante = "";

                        #region "Validação de Campos"
                        Label _labMaterialCor = row.FindControl("labCor") as Label;
                        if (_labMaterialCor == null)
                        {
                            DropDownList _ddlMaterialCor = row.FindControl("ddlCor") as DropDownList;
                            materialCor = _ddlMaterialCor.SelectedItem.Text.Trim();
                        }
                        else
                        {
                            materialCor = _labMaterialCor.Text.Trim();
                        }

                        Label _labRefCor = row.FindControl("labRefCor") as Label;
                        if (_labRefCor == null)
                        {
                            TextBox _txtRefCor = row.FindControl("txtRefCor") as TextBox;
                            refFabricante = _txtRefCor.Text.Trim();
                        }
                        else
                        {
                            refFabricante = _labRefCor.Text.Trim();
                        }
                        #endregion

                        MATERIAIS_CORE matCor = new MATERIAIS_CORE();

                        codigoCor = gvCores.DataKeys[row.RowIndex].Value.ToString();

                        matCor = desenvController.ObterMaterialCor(txtMaterial.Text.Trim(), codigoCor);
                        if (matCor == null)
                        {
                            List<MATERIAIS_CORE> matCores = new List<MATERIAIS_CORE>();
                            if (Session["MATERIAL_COR"] != null)
                                matCores = (List<MATERIAIS_CORE>)Session["MATERIAL_COR"];

                            int _index = matCores.FindIndex(p => p.DESC_COR_MATERIAL.Trim() == materialCor && p.REFER_FABRICANTE.Trim() == refFabricante);
                            matCores.RemoveAt(_index);

                            Session["MATERIAL_COR"] = matCores;
                        }
                        else
                        {

                            //Obter código da COR
                            var _fichaTecnica = desenvController.ObterFichaTecnica().Where(p => p.MATERIAL.Trim() == txtMaterial.Text.Trim() && p.COR_MATERIAL.Trim() == codigoCor.Trim());
                            if (_fichaTecnica != null && _fichaTecnica.Count() > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR possui Ficha Ténica. Não será possível Excluir.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }

                            if (desenvController.ValidarExclusaoMaterial(txtMaterial.Text.Trim(), codigoCor) == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui movimento de Nota Fiscal. Não será possível Excluir', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }

                            desenvController.ExcluirMaterialFornecedor(txtMaterial.Text.Trim(), codigoCor);
                            desenvController.ExcluirMaterialCor(txtMaterial.Text.Trim(), codigoCor);
                        }

                        gvCores.EditIndex = -1;
                        CarregarCores(txtMaterial.Text.Trim());

                        hidDescricaoCor.Value = "";
                        hidReferFab.Value = "";
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btExcluirCor_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }

        }
        protected void btIncluirCor_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {
                        string COR_MATERIAL = "";
                        string DESC_COR_MATERIAL = "";
                        string REFER_FABRICANTE = "";

                        DropDownList _ddlCorFooter = row.FindControl("ddlCorFooter") as DropDownList;
                        TextBox _txtRefCorFooter = row.FindControl("txtRefCorFooter") as TextBox;

                        if (_ddlCorFooter != null && _ddlCorFooter.SelectedValue.Trim() != "")
                        {
                            COR_MATERIAL = _ddlCorFooter.SelectedValue;
                            DESC_COR_MATERIAL = _ddlCorFooter.SelectedItem.Text;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Selecione uma COR.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (_txtRefCorFooter != null && _txtRefCorFooter.Text.Trim() != "")
                        {
                            REFER_FABRICANTE = _txtRefCorFooter.Text.Trim().ToUpper();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Informe a Descrição do Fornecedor.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        if (!ValidarCores(DESC_COR_MATERIAL))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já foi inserida neste Material.', { header: 'Erro', life: 2500, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                            return;
                        }

                        List<MATERIAIS_CORE> matCores = new List<MATERIAIS_CORE>();
                        MATERIAIS_CORE matCor = new MATERIAIS_CORE();
                        matCor.COR_MATERIAL = COR_MATERIAL;
                        matCor.DESC_COR_MATERIAL = DESC_COR_MATERIAL;
                        matCor.REFER_FABRICANTE = REFER_FABRICANTE;

                        if (Session["MATERIAL_COR"] != null)
                            matCores = (List<MATERIAIS_CORE>)Session["MATERIAL_COR"];

                        matCores.Add(matCor);
                        Session["MATERIAL_COR"] = matCores;

                        //Carregar cores
                        CarregarCores(txtMaterial.Text.Trim());

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btIncluirCor_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }



        }
        #endregion

        #region "COMPOSICAO"
        private void CarregarComposicao(string material)
        {
            List<MATERIAL_COMPOSICAO> _matComposicao = new List<MATERIAL_COMPOSICAO>();
            _matComposicao = desenvController.ObterMaterialComposicao(material);

            if (Session["MATERIAL_COMP"] != null)
                _matComposicao.AddRange((List<MATERIAL_COMPOSICAO>)Session["MATERIAL_COMP"]);

            gvComposicao.DataSource = _matComposicao;
            gvComposicao.DataBind();
        }
        protected void btComposicaoIncluir_Click(object sender, EventArgs e)
        {
            labErroComposicao.Text = "";
            try
            {
                if (txtComposicaoQtde.Text.Trim() == "" || Convert.ToDecimal(txtComposicaoQtde.Text) <= 0)
                {
                    labErroComposicao.Text = "Informe a Quantidade da composição.";
                    return;
                }
                if (txtComposicaoDescricao.Text.Trim() == "")
                {
                    labErroComposicao.Text = "Informe a Descrição da composição.";
                    return;
                }

                if (hidTotalComp.Value == "" || Convert.ToDecimal(hidTotalComp.Value) == 0)
                {
                    if (Convert.ToDecimal(txtComposicaoQtde.Text.Trim()) > 100)
                    {
                        labErroComposicao.Text = "Quantidade da Composição não pode ser maior que 100%.";
                        return;
                    }
                    else
                    {
                        hidTotalComp.Value = txtComposicaoQtde.Text.Trim();
                    }
                }
                else
                {
                    if (Convert.ToDecimal(hidTotalComp.Value) + Convert.ToDecimal(txtComposicaoQtde.Text.Trim()) > 100)
                    {
                        labErroComposicao.Text = "Quantidade da Composição não pode ser maior que 100%. Ainda é permitido o valor de " + ((Convert.ToDecimal(100.00)) - Convert.ToDecimal(hidTotalComp.Value)).ToString() + "%.";
                        return;
                    }
                    else
                    {
                        hidTotalComp.Value = (Convert.ToDecimal(hidTotalComp.Value) + Convert.ToDecimal(txtComposicaoQtde.Text.Trim())).ToString();
                    }
                }

                List<MATERIAL_COMPOSICAO> matComposicao = new List<MATERIAL_COMPOSICAO>();
                MATERIAL_COMPOSICAO _novo = new MATERIAL_COMPOSICAO();
                _novo.QTDE = Convert.ToDecimal(txtComposicaoQtde.Text);
                _novo.DESCRICAO = txtComposicaoDescricao.Text.Trim().ToUpper();
                _novo.MATERIAL = txtMaterial.Text.Trim();

                if (Session["MATERIAL_COMP"] != null)
                    matComposicao = (List<MATERIAL_COMPOSICAO>)Session["MATERIAL_COMP"];

                matComposicao.Add(_novo);
                Session["MATERIAL_COMP"] = matComposicao;

                txtComposicaoQtde.Text = "";
                txtComposicaoDescricao.Text = "";

                CarregarComposicao(txtMaterial.Text.Trim());
            }
            catch (Exception ex)
            {
                labErroComposicao.Text = "ERRO (btComposicaoIncluir_Click). \n\n" + ex.Message;
            }
        }
        protected void btComposicaoExcluir_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            if (b != null)
            {
                try
                {
                    labErroComposicao.Text = "";
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                        if (row.Cells[1].Text != "")
                            hidTotalComp.Value = (Convert.ToDecimal(hidTotalComp.Value) - Convert.ToDecimal(row.Cells[1].Text)).ToString();

                    var _matComposicao = desenvController.ObterMaterialComposicao(Convert.ToInt32(b.CommandArgument));
                    if (_matComposicao == null)
                    {
                        List<MATERIAL_COMPOSICAO> _lstMaterialComposicao = new List<MATERIAL_COMPOSICAO>();
                        if (Session["MATERIAL_COMP"] != null)
                            _lstMaterialComposicao = (List<MATERIAL_COMPOSICAO>)Session["MATERIAL_COMP"];

                        int _index = _lstMaterialComposicao.FindIndex(p => p.QTDE == Convert.ToDecimal(row.Cells[1].Text) && p.DESCRICAO.Trim() == row.Cells[2].Text);
                        _lstMaterialComposicao.RemoveAt(_index);

                        Session["MATERIAL_COMP"] = _lstMaterialComposicao;
                    }
                    else
                    {
                        desenvController.ExcluirMaterialComposicao(_matComposicao.CODIGO);
                    }

                    CarregarComposicao(txtMaterial.Text.Trim());
                }
                catch (Exception ex)
                {
                    labErroComposicao.Text = "ERRO (btComposicaoExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void gvComposicao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MATERIAL_COMPOSICAO _composicao = e.Row.DataItem as MATERIAL_COMPOSICAO;

                    colunaComposicao += 1;
                    if (_composicao != null)
                    {
                        Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaComposicao.ToString();

                        ImageButton _btExcluir = e.Row.FindControl("btComposicaoExcluir") as ImageButton;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _composicao.CODIGO.ToString();
                        }

                    }
                }
            }
        }
        protected void gvComposicao_DataBound(object sender, EventArgs e)
        {
            GridViewRow foo = gvComposicao.FooterRow;
            if (foo != null)
            {
                decimal total = 0;
                foreach (GridViewRow r in gvComposicao.Rows)
                    total = total + Convert.ToDecimal(r.Cells[1].Text);

                foo.Cells[1].Text = total.ToString();
                foo.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        #endregion

        #region "FOTO"
        private void CarregarMaterialFoto(string material)
        {
            var _materialFoto = desenvController.ObterMaterialFotoIntranet(material);
            if (_materialFoto != null)
                imgFotoTecido.ImageUrl = _materialFoto.FOTO;
        }
        protected void btFotoTecidoIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecido, imgFotoTecido, "FOTO_TECIDO_PRINCIPAL");
            }
            catch (Exception ex)
            {
                labErroFotoTecido.Text = "ERRO: (btFotoTecidoIncluir_OnClick) \n\n" + ex.Message;
            }
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 300
                // height 320
                var newWidth = 0;
                var newHeight = 0;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > 300 || newHeight > 320)
                {
                    newWidth = (int)((newWidth) * 0.95);
                    newHeight = (int)((newHeight) * 0.95);
                }

                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
        private bool GravarFoto(FileUpload _upload, System.Web.UI.WebControls.Image _image, string _session)
        {
            /*
            _session["FOTO_PECA"]
            _session["FOTO_TECIDO_PRINCIPAL"]
            _session["FOTO_TECIDO_DETALHE"]
             */
            bool ret = false;
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            Stream _stream = null;
            double _escale = 0;

            //AO ALTERAR AQUI/ ALTERAR NA TELA DE ALTERAÇÃO
            try
            {
                if (_upload.HasFile)
                {
                    //Obter variaveis do arquivo
                    _ext = System.IO.Path.GetExtension(_upload.PostedFile.FileName);
                    _fileName = Guid.NewGuid() + _session.Replace("FOTO", "") + _ext;
                    _path = Server.MapPath("~/Image_HB/") + _fileName;

                    //Obter stream da imagem
                    _stream = _upload.PostedFile.InputStream;

                    //Definir escala de redimensionamento
                    _escale = 0.50;
                }
                else
                {
                    if (Session[_session] != null)
                    {
                        //Obter variaveis do arquivo
                        _ext = ".png";
                        _fileName = Guid.NewGuid() + _session.Replace("FOTO", "") + _ext;
                        _path = Server.MapPath("~/Image_HB/") + _fileName;

                        //Obter stream da imagem
                        byte[] _foto = null;
                        _foto = (byte[])Session[_session];
                        _stream = new MemoryStream(_foto);

                        //Definir escala de redimensionamento
                        _escale = 0.35;
                    }
                }

                if (_stream != null)
                {
                    //Redimensionar imagem e salvar
                    GenerateThumbnails(_escale, _stream, _path);
                    //Carregar imagem
                    if (_fileName != "")
                    {
                        _image.ImageUrl = "~/Image_HB/" + _fileName;
                        Session[_session] = null;
                    }
                }

                ret = true;
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }
        #endregion

        #region "NOTIFICACAO"
        private void CarregarNotificacao()
        {
            List<NOTIFICACAO> _notificacao = new List<NOTIFICACAO>();
            _notificacao = new NotificacaoController().ObterNotificacao(3).Where(p => p.DATA_LEITURA == null).ToList();

            gvNotificacao.DataSource = _notificacao;
            gvNotificacao.DataBind();

            labNotificacao.ForeColor = Color.Gray;
            if (_notificacao != null && _notificacao.Count > 0)
                labNotificacao.ForeColor = Color.Red;
        }
        protected void gvNotificacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    NOTIFICACAO _notificacao = e.Row.DataItem as NOTIFICACAO;

                    colunaNotificacao += 1;
                    if (_notificacao != null)
                    {
                        DESENV_PRODUTO_FICTEC _fichaTecnica = desenvController.ObterFichaTecnicaCodigo(_notificacao.CODIGO_TABELA);

                        if (_fichaTecnica != null)
                        {
                            Literal _col = e.Row.FindControl("litColuna") as Literal;
                            if (_col != null)
                                _col.Text = colunaNotificacao.ToString();

                            Label _labGrupo = e.Row.FindControl("labGrupo") as Label;
                            if (_labGrupo != null)
                                _labGrupo.Text = _fichaTecnica.GRUPO;

                            Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            if (_labSubGrupo != null)
                                _labSubGrupo.Text = _fichaTecnica.SUBGRUPO;

                            Label _labCor = e.Row.FindControl("labCor") as Label;
                            if (_labCor != null)
                                _labCor.Text = prodController.ObterCoresBasicas(_fichaTecnica.COR_MATERIAL).DESC_COR;

                            Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            if (_labCorFornecedor != null)
                                _labCorFornecedor.Text = _fichaTecnica.COR_FORNECEDOR;

                            Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                            if (_labFornecedor != null)
                                _labFornecedor.Text = _fichaTecnica.FORNECEDOR;

                            ImageButton _ibtMarcarLido = e.Row.FindControl("btMarcarLido") as ImageButton;
                            if (_ibtMarcarLido != null)
                                _ibtMarcarLido.CommandArgument = _notificacao.CODIGO.ToString();

                        }
                    }
                }
            }
        }
        protected void btMarcarLido_Click(object sender, EventArgs e)
        {
            ImageButton ibt = (ImageButton)sender;
            if (ibt != null)
            {
                try
                {
                    int codigoNotificacao = Convert.ToInt32(ibt.CommandArgument);

                    NOTIFICACAO notificacao = notController.ObterNotificacaoCodigo(codigoNotificacao);
                    if (notificacao != null)
                    {
                        notificacao.DATA_LEITURA = DateTime.Now;
                        notificacao.USUARIO_LEITURA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        notController.AtualizarNotificacao(notificacao);
                    }

                    CarregarNotificacao();
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }

        #endregion

        #region "TODOS"
        private void CarregarMaterialTodos()
        {
            List<MATERIAI> _material = new List<MATERIAI>();

            List<COLECOE> colAuxiliar = new List<COLECOE>();
            colAuxiliar = (new BaseController()).BuscaColecoesMateriais();
            _material = desenvController.ObterMaterial();
            _material = _material.Where(p => colAuxiliar.Any(x => x.COLECAO == p.COLECAO)).ToList();

            if (txtMaterial.Text.Trim() != "")
                _material = _material.Where(p => p.MATERIAL.Trim().Contains(txtMaterial.Text.Trim().ToUpper())).ToList();

            if (txtMaterialDescricao.Text.Trim() != "")
                _material = _material.Where(p => p.DESC_MATERIAL.Trim().Contains(txtMaterialDescricao.Text.Trim().ToUpper())).ToList();

            if (ddlColecoes.SelectedValue.Trim() != "")
                _material = _material.Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim()).ToList();

            if (ddlMaterialGrupo.SelectedValue.Trim() != "")
                _material = _material.Where(p => p.GRUPO.Trim() == ddlMaterialGrupo.SelectedItem.Text.Trim()).ToList();

            if (ddlMaterialSubGrupo.SelectedValue.Trim() != "")
                _material = _material.Where(p => p.SUBGRUPO.Trim() == ddlMaterialSubGrupo.SelectedItem.Text.Trim()).ToList();

            if (ddlFornecedor.SelectedValue.Trim() != "Selecione")
                _material = _material.Where(p => p.FABRICANTE.Trim() == ddlFornecedor.SelectedItem.Text.Trim()).ToList();

            gvTodos.DataSource = _material.Where(p => p.GRUPO.Trim() != "TECIDOS" &&
                                                      p.GRUPO.Trim() != "AVIAMENTOS" &&
                                                      p.GRUPO.Trim() != "UTENSILIOS" &&
                                                      p.GRUPO.Trim() != "TECIDO" &&
                                                      p.GRUPO.Trim() != "AVIAMENTO"
                                                      ).OrderBy(o => o.GRUPO.Trim()).ThenBy(k => k.SUBGRUPO);
            gvTodos.DataBind();
        }
        protected void gvTodos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MATERIAI _material = e.Row.DataItem as MATERIAI;

                    colunaTodos += 1;
                    if (_material != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaTodos.ToString();

                        ImageButton _ibtPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                        if (_ibtPesquisar != null)
                        {
                            _ibtPesquisar.CommandArgument = _material.MATERIAL;
                        }
                    }
                }
            }
        }
        protected void btPesquisarTodos_Click(object sender, EventArgs e)
        {
            ImageButton ibt = (ImageButton)sender;
            if (ibt != null)
            {
                try
                {
                    string material = ibt.CommandArgument;

                    txtMaterial.Text = material;
                    ibtPesquisar_Click(null, null);

                    MoverAba("0");
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        #endregion

        #region "VALIDACAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labMaterialDescricao.ForeColor = _OK;
            if (txtMaterialDescricao.Text.Trim() == "")
            {
                labMaterialDescricao.ForeColor = _notOK;
                retorno = false;
            }

            labColecao.ForeColor = _OK;
            if (ddlColecoes.SelectedValue.Trim() == "")
            {
                labColecao.ForeColor = _notOK;
                retorno = false;
            }

            labGrupo.ForeColor = _OK;
            if (ddlMaterialGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupo.ForeColor = _OK;
            if (ddlMaterialSubGrupo.SelectedValue.Trim() == "")
            {
                labSubGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labTipoMaterial.ForeColor = _OK;
            if (ddlTipoMaterial.SelectedValue.Trim() == "Selecione")
            {
                labTipoMaterial.ForeColor = _notOK;
                retorno = false;
            }

            labUnidadeMedida.ForeColor = _OK;
            if (ddlUnidadeMedida.SelectedValue.Trim() == "")
            {
                labUnidadeMedida.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labCores.ForeColor = _OK;
            if (gvCores.Rows.Count > 0)
            {
                Label _labCor = gvCores.Rows[0].FindControl("labCor") as Label;
                if (_labCor.Text.Trim() == "")
                {
                    labCores.ForeColor = _notOK;
                    retorno = false;
                }
            }

            labClassifFiscal.ForeColor = _OK;
            if (ddlClassifFiscal.SelectedValue.Trim() == "")
            {
                labClassifFiscal.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private bool ValidarCores(string descCor)
        {
            bool retorno = true;

            foreach (GridViewRow row in gvCores.Rows)
            {
                if (row != null)
                {
                    Label labMateriaCor = row.FindControl("labCor") as Label;
                    if (labMateriaCor != null)
                    {
                        if (labMateriaCor.Text.Trim() == descCor.Trim())
                        {
                            retorno = false;
                            break;
                        }
                    }
                }
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {
            txtMaterial.Enabled = enable;
            txtMaterialDescricao.Enabled = enable;

            //MATERIA PRIMA
            ddlColecoes.Enabled = enable;
            ddlMaterialGrupo.Enabled = enable;
            ddlMaterialSubGrupo.Enabled = enable;
            ddlTipoMaterial.Enabled = enable;
            ddlUnidadeMedida.Enabled = enable;
            ddlFornecedor.Enabled = enable;

            foreach (GridViewRow row in gvCores.Rows)
                if (row != null)
                {
                    ImageButton _btEditarCor = row.FindControl("btEditarCor") as ImageButton;
                    if (_btEditarCor != null)
                        _btEditarCor.Visible = enable;
                    ImageButton _btExcluirCor = row.FindControl("btExcluirCor") as ImageButton;
                    if (_btExcluirCor != null)
                        _btExcluirCor.Visible = enable;
                }

            //COMPOSICAO
            btComposicaoIncluir.Enabled = enable;
            btComposicaoIncluir.Visible = enable;
            foreach (GridViewRow row in gvComposicao.Rows)
                if (row != null)
                {
                    ImageButton _btComposicaoExcluir = row.FindControl("btComposicaoExcluir") as ImageButton;
                    if (_btComposicaoExcluir != null)
                        _btComposicaoExcluir.Visible = enable;
                }

            //FOTO
            btFotoTecidoIncluir.Enabled = enable;
            uploadFotoTecido.Enabled = enable;

            //TRIBUT
            ddlClassifFiscal.Enabled = enable;
            ddlOrigem.Enabled = enable;
            ddlContaContabilEstoque1.Enabled = enable;
            ddlContaContabilCompra1.Enabled = enable;
            ddlContaContabilCompra2.Enabled = enable;
            ddlContaContabilVenda1.Enabled = enable;
            ddlContaContabilVenda2.Enabled = enable;
            ddlCaractContabil.Enabled = enable;
            ddlTipoSped.Enabled = enable;
        }
        private void LimparCampos()
        {
            //Recarregar novamente, não me pergunte porque...
            CarregarGrupos();
            CarregarFornecedores();
            //***********************************************

            txtMaterial.Text = "";
            txtMaterialDescricao.Text = "";
            labErro.Text = "";

            //MATERIA PRIMA
            ddlColecoes.SelectedValue = "";
            ddlMaterialGrupo.SelectedValue = "";
            ddlMaterialSubGrupo.Items.Clear();
            ddlTipoMaterial.SelectedValue = "Selecione";
            ddlUnidadeMedida.SelectedValue = "";
            ddlFornecedor.SelectedValue = "Selecione";

            //COMPOSICAO
            hidTotalComp.Value = "";
            labErroComposicao.Text = "";
            txtComposicaoQtde.Text = "";
            txtComposicaoDescricao.Text = "";
            CarregarComposicao(txtMaterial.Text.Trim());

            //FOTO
            imgFotoTecido.ImageUrl = "";
            labErroFotoTecido.Text = "";

            //TRIBUTACOES
            ddlClassifFiscal.SelectedValue = "0000000000";
            ddlCaractContabil.SelectedValue = "12";
            ddlTipoSped.SelectedValue = "01";

        }
        #endregion

        #region "LAVAGEM"
        private void CarregarLavagemSelecionada(string material)
        {
            pnlLavagem.Controls.Clear();

            var matLavSel = desenvController.ObterMaterialLavagem(material).OrderBy(p => p.ORDEM);

            System.Web.UI.WebControls.Image img = null;

            foreach (var m in matLavSel)
            {
                img = new System.Web.UI.WebControls.Image();
                img.ID = m.IMAGEM;
                img.ImageUrl = m.IMAGEM;
                img.Width = Unit.Pixel(35);
                img.Height = Unit.Pixel(35);
                pnlLavagem.Controls.Add(img);
            }

            //
        }
        private void CarregarLavagem()
        {
            List<SP_OBTER_LAVAGEMResult> lav = new List<SP_OBTER_LAVAGEMResult>();
            lav = desenvController.ObterLavagemGrid();

            gvLavagem.DataSource = lav;
            gvLavagem.DataBind();
        }
        protected void gvLavagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_LAVAGEMResult _lav = e.Row.DataItem as SP_OBTER_LAVAGEMResult;

                    if (_lav != null)
                    {
                        ImageButton _imgColuna1 = e.Row.FindControl("imgColuna1") as ImageButton;
                        if (_imgColuna1 != null)
                            _imgColuna1.ImageUrl = _lav._1;

                        ImageButton _imgColuna2 = e.Row.FindControl("imgColuna2") as ImageButton;
                        if (_imgColuna2 != null)
                            _imgColuna2.ImageUrl = _lav._2;

                        ImageButton _imgColuna3 = e.Row.FindControl("imgColuna3") as ImageButton;
                        if (_imgColuna3 != null)
                            _imgColuna3.ImageUrl = _lav._3;

                        ImageButton _imgColuna4 = e.Row.FindControl("imgColuna4") as ImageButton;
                        if (_imgColuna4 != null)
                            _imgColuna4.ImageUrl = _lav._4;

                        ImageButton _imgColuna5 = e.Row.FindControl("imgColuna5") as ImageButton;
                        if (_imgColuna5 != null)
                            _imgColuna5.ImageUrl = _lav._5;

                        ImageButton _imgColuna6 = e.Row.FindControl("imgColuna6") as ImageButton;
                        if (_imgColuna6 != null)
                            _imgColuna6.ImageUrl = _lav._6;

                        ImageButton _imgColuna7 = e.Row.FindControl("imgColuna7") as ImageButton;
                        if (_imgColuna7 != null)
                            _imgColuna7.ImageUrl = _lav._7;

                        ImageButton _imgColuna8 = e.Row.FindControl("imgColuna8") as ImageButton;
                        if (_imgColuna8 != null)
                            _imgColuna8.ImageUrl = _lav._8;

                        ImageButton _imgColuna9 = e.Row.FindControl("imgColuna9") as ImageButton;
                        if (_imgColuna9 != null)
                            _imgColuna9.ImageUrl = _lav._9;

                        ImageButton _imgColuna10 = e.Row.FindControl("imgColuna10") as ImageButton;
                        if (_imgColuna10 != null)
                            _imgColuna10.ImageUrl = _lav._10;

                        ImageButton _imgColuna11 = e.Row.FindControl("imgColuna11") as ImageButton;
                        if (_imgColuna11 != null)
                        {
                            _imgColuna11.ImageUrl = _lav._11;
                            if (_lav._11 == null)
                                _imgColuna11.Visible = false;
                        }

                        ImageButton _imgColuna12 = e.Row.FindControl("imgColuna12") as ImageButton;
                        if (_imgColuna12 != null)
                        {
                            _imgColuna12.ImageUrl = _lav._12;
                            if (_lav._12 == null)
                                _imgColuna12.Visible = false;
                        }

                        ImageButton _imgColuna13 = e.Row.FindControl("imgColuna13") as ImageButton;
                        if (_imgColuna13 != null)
                        {
                            _imgColuna13.ImageUrl = _lav._13;
                            if (_lav._13 == null)
                                _imgColuna13.Visible = false;
                        }

                        ImageButton _imgColuna14 = e.Row.FindControl("imgColuna14") as ImageButton;
                        if (_imgColuna14 != null)
                        {
                            _imgColuna14.ImageUrl = _lav._14;
                            if (_lav._14 == null)
                                _imgColuna14.Visible = false;
                        }
                    }
                }
            }
        }
        protected void gvLavagem_DataBound(object sender, EventArgs e)
        {
            int contLav = 0;
            MATERIAL_LAVAGEM matLav = null;
            foreach (GridViewRow row in gvLavagem.Rows)
            {
                for (int i = 0; i < 14; i++)
                {
                    matLav = desenvController.ObterMaterialLavagem(txtMaterial.Text.Trim(), ((ImageButton)row.FindControl("imgColuna" + ((i + 1).ToString()))).ImageUrl);
                    if (matLav != null)
                        gvLavagem.Rows[contLav].Cells[i].BackColor = Color.LightBlue;
                }
                contLav += 1;
            }

        }

        protected void imgColuna_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";
                ImageButton ib = (ImageButton)sender;
                if (ib != null)
                {
                    GridViewRow row = (GridViewRow)ib.NamingContainer;
                    if (row != null)
                    {
                        int linha = 0;
                        int coluna = 0;
                        int qtdeMaterial = 0;

                        linha = row.RowIndex;
                        coluna = Convert.ToInt32(ib.ID.Replace("imgColuna", "")) - 1;

                        var img = desenvController.ObterMaterialLavagem(txtMaterial.Text, ib.ImageUrl);

                        var imgTotal = desenvController.ObterMaterialLavagem(txtMaterial.Text).OrderByDescending(p => p.CODIGO).FirstOrDefault();
                        if (imgTotal != null)
                            qtdeMaterial = Convert.ToInt32(imgTotal.ORDEM) + 1;

                        MATERIAL_LAVAGEM lav = null;
                        if (img == null)
                        {
                            lav = new MATERIAL_LAVAGEM();
                            lav.MATERIAL = txtMaterial.Text.Trim();
                            lav.IMAGEM = ib.ImageUrl;
                            lav.ORDEM = qtdeMaterial;
                            desenvController.InserirMaterialLavagem(lav);
                            gvLavagem.Rows[linha].Cells[coluna].BackColor = Color.LightBlue;
                        }
                        else
                        {
                            desenvController.ExcluirMaterialLavagem(txtMaterial.Text.Trim(), ib.ImageUrl);
                            gvLavagem.Rows[linha].Cells[coluna].BackColor = Color.White;
                        }

                        CarregarLavagemSelecionada(txtMaterial.Text.Trim());

                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion
    }
}