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
    public partial class desenv_ficha_tecnica : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        NotificacaoController notController = new NotificacaoController();
        int colunaMaterial = 0, colunaTodos = 0, colunaCopia = 0, colunaNotificacao = 0;

        List<DESENV_PRODUTO> g_ProdutoFiltro = new List<DESENV_PRODUTO>();

        const string cAbaTodos = "5";

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
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                CarregarGrupos(ddlMaterialGrupo);
                CarregarDetalhes();
                CarregarCoresBasicas("", "", ddlCor);
                CarregarFornecedores("", "");
                CarregarFiltros();
                //CarregarNotificacao();

                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtLimpar.Visible = false;
                btSalvarMaterial.Visible = false;
                btCancelarMaterial.Visible = false;
                ibtAprovarFichaTecnica.Visible = false;

                pnlCopiaModelo.Visible = false;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "5" : hidTabSelected.Value) + "');", true);

            //Evitar duplo clique no botão
            ibtAprovarFichaTecnica.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(ibtAprovarFichaTecnica, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupos(DropDownList _ddlMaterialGrupo)
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
                _ddlMaterialGrupo.DataSource = _matGrupo;
                _ddlMaterialGrupo.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddlMaterialGrupo = (DropDownList)sender;

            if (_ddlMaterialGrupo.ID == "ddlMaterialGrupo")
            {
                if (ddlMaterialGrupo.SelectedValue.Trim() != "")
                    CarregarSubGrupos(_ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo);
            }
            else
            {
                CarregarSubGrupos(_ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupoFiltro);
            }
        }
        private void CarregarSubGrupos(string grupo, DropDownList _ddlMaterialSubGrupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).OrderBy(p => p.SUBGRUPO.Trim()).ToList();
            if (_matSubGrupo != null)
            {
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "Selecione" });
                _ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                _ddlMaterialSubGrupo.DataBind();
            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddlMaterialSubGrupo = (DropDownList)sender;

            if (_ddlMaterialSubGrupo.ID == "ddlMaterialSubGrupo")
            {
                if (ddlMaterialSubGrupo.SelectedValue.Trim() != "")
                    CarregarCoresBasicas(ddlMaterialGrupo.SelectedItem.Text.Trim(), _ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor);
            }
            else
            {
                CarregarCoresBasicas(ddlMaterialGrupoFiltro.SelectedItem.Text.Trim(), _ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlMaterialCorFiltro);
            }

        }
        private void CarregarCoresBasicas(string grupo, string subGrupo, DropDownList _ddlCorMaterial)
        {
            List<CORES_BASICA> coresBasicas = new List<CORES_BASICA>();

            if (grupo.Trim() != "" && subGrupo.Trim() != "")
            {
                List<MATERIAI> filtroMaterial = new List<MATERIAI>();
                filtroMaterial = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();

                List<MATERIAIS_CORE> materialCores = desenvController.ObterMaterialCor();
                materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

                CORES_BASICA _corBasica = null;
                foreach (MATERIAIS_CORE c in materialCores)
                {
                    _corBasica = new CORES_BASICA();
                    _corBasica.COR = c.COR_MATERIAL.Trim() + "-" + c.REFER_FABRICANTE.Trim();
                    _corBasica.DESC_COR = (c.DESC_COR_MATERIAL.Trim() + ((c.REFER_FABRICANTE.Trim() != "") ? " - " + c.REFER_FABRICANTE.Trim() : ""));
                    coresBasicas.Add(_corBasica);
                }

                coresBasicas = coresBasicas.OrderBy(p => p.DESC_COR.Trim()).ToList();
            }
            else
            {
                coresBasicas = ObterCoresBasicas();
            }

            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            _ddlCorMaterial.DataSource = coresBasicas;
            _ddlCorMaterial.DataBind();

        }
        protected void ddlCor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCor.SelectedValue.Trim() != "")
                CarregarFornecedores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarFornecedores(string grupo, string subGrupo)
        {
            List<MATERIAI> filtroMaterial = new List<MATERIAI>();
            if (grupo.Trim() != "" && subGrupo.Trim() != "")
                filtroMaterial = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();

            List<PROD_FORNECEDOR> _fornecedor = prodController.ObterFornecedor().Where(p => p.STATUS == 'A').GroupBy(g => new { FORNECEDOR = g.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();
            if (_fornecedor != null)
            {
                if (filtroMaterial != null && filtroMaterial.Count > 0)
                    _fornecedor = _fornecedor.Where(i => filtroMaterial.Where(j => j.FABRICANTE != null).Any(g => g.FABRICANTE.Trim() == i.FORNECEDOR.Trim())).ToList();

                _fornecedor.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });
                ddlFornecedor.DataSource = _fornecedor;
                ddlFornecedor.DataBind();

                if (_fornecedor.Count() == 2)
                    ddlFornecedor.SelectedIndex = 1;
            }
        }
        private void CarregarDetalhes()
        {
            List<PROD_DETALHE> detalhes = prodController.ObterDetalhes().OrderBy(p => p.DESCRICAO).ToList();

            detalhes.Insert(0, new PROD_DETALHE { CODIGO = 99, DESCRICAO = "PRINCIPAL" });
            ddlDetalhe.DataSource = detalhes;
            ddlDetalhe.DataBind();
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
            ibtEditar.Visible = false;
            ibtExcluir.Visible = false;
            ibtPesquisar.Visible = false;
            ibtLimpar.Visible = false;

            //Controle de campos
            HabilitarCampos(true);
            LimparCampos(true);

            hidAcao.Value = "I";
            txtModelo.Text = "";
            ibtSalvar.Visible = true;
            ibtCancelar.Visible = true;
            txtModelo.Enabled = false;

            MoverAba("0");

            pnlCopiaModelo.Visible = false;
        }
        protected void ibtEditar_Click(object sender, EventArgs e)
        {
            try
            {

                DESENV_PRODUTO _produto = desenvController.ObterProduto(Convert.ToInt32(hidCodigoProduto.Value));
                if (_produto != null)
                {
                    if (_produto.DATA_FICHATECNICA != null)
                    {
                        labErro.Text = "Ficha Técnica não pode ser alterada. Já está Aprovada.";
                        return;
                    }
                }

                //controle de visibilidade dos botões da tela
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtLimpar.Visible = false;

                ibtSalvar.Visible = true;
                ibtCancelar.Visible = true;

                labErro.Text = "";
                HabilitarCampos(true);
                txtModelo.Enabled = false;
                txtModeloDescricao.Enabled = false;
                txtCor.Enabled = false;
                txtCorFornecedor.Enabled = false;
                txtFornecedor.Enabled = false;
                txtTecido.Enabled = false;

                hidAcao.Value = "A";

                pnlCopiaModelo.Visible = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                List<DESENV_PRODUTO> _produto = new List<DESENV_PRODUTO>();
                if (hidCodigoProduto.Value != "")
                {
                    _produto.Add(desenvController.ObterProduto(Convert.ToInt32(hidCodigoProduto.Value)));
                }
                else
                {
                    _produto = desenvController.ObterProduto().Where(p => p.MODELO != null && p.MODELO.Trim() != "" &&
                                                                                         p.MODELO.Trim() == txtModelo.Text.ToUpper().Trim() &&
                                                                                         p.COR.Trim().Contains(txtCor.Text.Trim().ToUpper()) &&
                                                                                         p.STATUS == 'A').ToList();
                    if (txtCorFornecedor.Text.Trim() != "")
                        _produto = _produto.Where(p => p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim().Contains(txtCorFornecedor.Text.Trim().ToUpper())).ToList();
                }

                if (_produto != null && _produto.Count == 1)
                {
                    if (_produto[0].DATA_APROVACAO == null)
                    {
                        hidCodigoProduto.Value = "";
                        labErro.Text = "Produto não está Aprovado para criar uma Ficha Técnica.";
                        return;
                    }

                    CarregarProduto(_produto[0]);

                    hidCodigoProduto.Value = _produto[0].CODIGO.ToString();
                    ibtPesquisar.Visible = false;
                    ibtSalvar.Visible = false;
                    ibtCancelar.Visible = false;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;
                    ibtCopiarHB.Visible = true;

                    ibtAprovarFichaTecnica.Visible = false;
                    ibtAbrirFichaTecnica.Visible = false;
                    if (_produto[0].DATA_FICHATECNICA == null)
                        ibtAprovarFichaTecnica.Visible = true;
                    else
                        ibtAbrirFichaTecnica.Visible = true;
                    hidAcao.Value = "C";

                    HabilitarCampos(false);
                    MoverAba("0");

                    pnlCopiaModelo.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + cAbaTodos + "');", true);
                    hidTabSelected.Value = cAbaTodos;
                    CarregarProdutoTodos(_produto);
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
                ibtExcluir.Visible = false;
                ibtCopiarHB.Visible = false;
                hidAcao.Value = "";
                hidAcaoMaterial.Value = "";
                hidDetalhe.Value = "";

                hidCodigoProduto.Value = "";
                txtModelo.Text = "";
                txtModeloDescricao.Text = "";

                HabilitarCampos(true);
                LimparCampos(true);

                CarregarProdutoTodos(null);

                pnlCopiaModelo.Visible = false;
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
                /*if (!ValidarCampos() && hidAcao.Value == "A")
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return;
                }*/

                //controle de visibilidade dos botões da tela
                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;

                System.Drawing.Color _OK = System.Drawing.Color.Gray;
                labGrupo.ForeColor = _OK;
                labSubGrupo.ForeColor = _OK;
                labFornecedor.ForeColor = _OK;
                labCor.ForeColor = _OK;
                labDetalhe.ForeColor = _OK;
                labConsumo.ForeColor = _OK;
                hidAcaoMaterial.Value = "";
                hidDetalhe.Value = "";

                if (hidAcao.Value == "I")
                {
                    ibtLimpar_Click(null, null);
                    ibtPesquisar.Visible = true;
                    hidAcao.Value = "";
                }

                if (hidAcao.Value == "A")
                {
                    ibtPesquisar_Click(null, null);
                    ibtPesquisar.Visible = false;
                    //ibtExcluir.Visible = true;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;
                    hidAcao.Value = "C";
                }

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
                if (txtModelo.Text.Trim() == "")
                {
                    labErro.Text = "Produto não informado. Refaça sua pesquisa.";
                    return;
                }
                else
                {
                    DESENV_PRODUTO _produto = null;
                    _produto = desenvController.ObterProduto(Convert.ToInt32(hidCodigoProduto.Value));

                    //Validar exclusao do PRODUTO
                    _produto.STATUS = 'E';
                    desenvController.ExcluirProduto(_produto);

                    ibtLimpar_Click(null, null);
                    MoverAba("0");
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtAprovarFichaTecnica_Click(object sender, EventArgs e)
        {
            try
            {
                bool ok = true;

                labErro.Text = "";
                if (hidCodigoProduto.Value.Trim() == "")
                {
                    labErro.Text = "Produto não informado. Refaça sua pesquisa.";
                    return;
                }
                else
                {
                    //Obter botao
                    string msg = "";
                    ImageButton _ibt = (ImageButton)sender;

                    DESENV_PRODUTO _produto = null;
                    _produto = desenvController.ObterProduto(Convert.ToInt32(hidCodigoProduto.Value));

                    var ftExiste = desenvController.ObterFichaTecnica(Convert.ToInt32(hidCodigoProduto.Value));
                    if (ftExiste == null || ftExiste.Count() <= 0)
                    {
                        ok = false;
                        labErro.Text = "Produto não possui Ficha Técnica. Antes de aprovar, crie uma Ficha Técnica.";
                        return;
                    }

                    if (_ibt.ID == "ibtAprovarFichaTecnica")
                    {
                        _produto.DATA_FICHATECNICA = DateTime.Now;
                        InserirQtdeProdutoProducao(_produto);
                        ibtAprovarFichaTecnica.Visible = false;
                        msg = "Ficha Técnica Aprovada.";

                        //LerNotificacao(_produto.CODIGO);
                    }
                    else
                    {
                        ////Verifica se produto ja foi cortado
                        //var producao = desenvController.ObterProdutoProducao(_produto.CODIGO).Where(p => p.PROD_HB != null);
                        //if (producao == null || producao.Count() <= 0)
                        //{
                        _produto.DATA_FICHATECNICA = null;
                        ibtAbrirFichaTecnica.Visible = false;
                        msg = "Ficha Técnica Aberta.";

                        var _produtoProducao = desenvController.ObterProdutoProducao(_produto.CODIGO);
                        foreach (DESENV_PRODUTO_PRODUCAO pp in _produtoProducao)
                            if (pp != null)
                                desenvController.ExcluirProdutoProducao(pp.CODIGO);
                        //}
                        //else
                        //{
                        //    ok = false;
                        //    msg = "Não será possível abrir esta Ficha. Produto CORTADO.";
                        //}
                    }

                    _produto.USUARIO_FICHATECNICA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    if (ok)
                        desenvController.AprovarProduto(_produto);

                    ibtPesquisar_Click(null, null);
                    MoverAba("0");

                    labErro.Text = msg;
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
        private void CarregarProduto(DESENV_PRODUTO _produto)
        {
            txtModelo.Text = _produto.MODELO;
            var produtoLinx = (new BaseController().BuscaProduto(_produto.MODELO.Trim()));
            if (produtoLinx != null)
                txtModeloDescricao.Text = produtoLinx.DESC_PRODUTO.Trim();
            else
                txtModeloDescricao.Text = "PRODUTO NÃO CADASTRADO.";

            var _cor = prodController.ObterCoresBasicas(_produto.COR);
            if (_cor != null)
                txtCor.Text = _cor.DESC_COR.Trim();
            txtCorFornecedor.Text = _produto.FORNECEDOR_COR;
            txtFornecedor.Text = _produto.FORNECEDOR;
            txtTecido.Text = _produto.TECIDO_POCKET;
            chkGradeado.Checked = (_produto.GRADEADO == true) ? true : false;

            //MATERIAIS
            CarregarMateriais(_produto.CODIGO);

            //FOTO
            CarregarProdutoFoto(_produto);
        }
        private void InserirQtdeProdutoProducao(DESENV_PRODUTO _produto)
        {
            List<DESENV_PRODUTO_FICTEC> _fichaTecnica = null;
            _fichaTecnica = desenvController.ObterFichaTecnica(_produto.CODIGO);

            DESENV_PRODUTO_PRODUCAO _produtoProducao = null;
            int codigoProdutoProducao = 0;
            int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            foreach (DESENV_PRODUTO_FICTEC ft in _fichaTecnica)
            {
                if (_produto.QTDE_MOSTRUARIO != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'M';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE_MOSTRUARIO);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = codigoUsuario;
                    _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                    _produtoProducao.USUARIO_LIBERACAO = codigoUsuario;
                    _produtoProducao.STATUS = 'L';

                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    //notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS MOSTRUÁRIO - ADICIONADO"));
                }

                if (_produto.QTDE != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'V';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = codigoUsuario;
                    _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                    _produtoProducao.USUARIO_LIBERACAO = codigoUsuario;
                    _produtoProducao.STATUS = 'L';

                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    //notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS VAREJO - ADICIONADO"));
                }

                if (_produto.QTDE_ATACADO != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'A';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE_ATACADO);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = codigoUsuario;
                    _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                    _produtoProducao.USUARIO_LIBERACAO = codigoUsuario;
                    _produtoProducao.STATUS = 'L';

                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    //notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS ATACADO - ADICIONADO"));
                }
            }
        }
        #endregion

        #region "MATERIAIS"
        private void CarregarMateriais(int codigoProduto)
        {
            List<DESENV_PRODUTO_FICTEC> _fichaTecnica = desenvController.ObterFichaTecnica(codigoProduto);

            if (_fichaTecnica == null || _fichaTecnica.Count <= 0)
                _fichaTecnica.Add(new DESENV_PRODUTO_FICTEC { CODIGO = 0, GRUPO = "", SUBGRUPO = "", DESENV_PRODUTO = 0, PROD_DETALHE = 0, MATERIAL = "", COR_MATERIAL = "", FORNECEDOR = "", CONSUMO = 0 });

            gvMateriais.DataSource = _fichaTecnica;
            gvMateriais.DataBind();
        }
        protected void gvMateriais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO_FICTEC _fichaTecnica = e.Row.DataItem as DESENV_PRODUTO_FICTEC;

                    colunaMaterial += 1;
                    if (_fichaTecnica != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaMaterial.ToString();

                        if (_fichaTecnica.GRUPO.Trim() != "")
                        {
                            Label _labGrupo = e.Row.FindControl("labGrupo") as Label;
                            if (_labGrupo != null)
                                _labGrupo.Text = _fichaTecnica.GRUPO.Trim();

                            Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            if (_labSubGrupo != null)
                                _labSubGrupo.Text = _fichaTecnica.SUBGRUPO.Trim();

                            Label _labDetalhe = e.Row.FindControl("labDetalhe") as Label;
                            if (_labDetalhe != null)
                                _labDetalhe.Text = (_fichaTecnica.PROD_DETALHE1 == null) ? "PRINCIPAL" : _fichaTecnica.PROD_DETALHE1.DESCRICAO;

                            Label _labCor = e.Row.FindControl("labCor") as Label;
                            if (_labCor != null)
                                _labCor.Text = prodController.ObterCoresBasicas(_fichaTecnica.COR_MATERIAL).DESC_COR.Trim();

                            Label _labRefCor = e.Row.FindControl("labRefCor") as Label;
                            if (_labRefCor != null)
                            {
                                var corRef = desenvController.ObterMaterialCor(_fichaTecnica.MATERIAL, _fichaTecnica.COR_MATERIAL);
                                if (corRef != null)
                                    _labRefCor.Text = corRef.REFER_FABRICANTE;
                                else
                                    _labRefCor.Text = _fichaTecnica.COR_FORNECEDOR;
                            }

                            Label _labConsumo = e.Row.FindControl("labConsumo") as Label;
                            if (_labConsumo != null)
                                _labConsumo.Text = _fichaTecnica.CONSUMO.ToString("###,###,###,##0.000");

                            ImageButton _btEditarMaterial = e.Row.FindControl("btEditarMaterial") as ImageButton;
                            ImageButton _btExcluirMaterial = e.Row.FindControl("btExcluirMaterial") as ImageButton;
                            _btEditarMaterial.Visible = false;
                            _btExcluirMaterial.Visible = false;
                            if (_fichaTecnica.CODIGO.ToString().Trim() != "")
                            {
                                _btExcluirMaterial.Visible = true;
                                _btExcluirMaterial.CommandArgument = _fichaTecnica.CODIGO.ToString();
                                _btEditarMaterial.Visible = true;
                            }
                        }
                    }
                }
            }

        }
        protected void btEditarMaterial_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {
                        int codigoFichaTecnica = 0;
                        codigoFichaTecnica = Convert.ToInt32(gvMateriais.DataKeys[row.RowIndex].Value.ToString());

                        hidCodigoFichaTecnica.Value = codigoFichaTecnica.ToString();
                        hidAcaoMaterial.Value = "A";

                        var fichaTecnica = desenvController.ObterFichaTecnicaCodigo(codigoFichaTecnica);
                        if (fichaTecnica != null)
                        {
                            hidDetalhe.Value = (fichaTecnica.PROD_DETALHE == null) ? "99" : fichaTecnica.PROD_DETALHE.ToString();

                            if (fichaTecnica.MATERIAL.Trim().Length == 10)
                                txtMaterialFiltro.Text = fichaTecnica.MATERIAL.Trim();
                            else
                                txtMaterialFiltro.Text = "00.00.0000";
                            txtMaterialFiltro_TextChanged(txtMaterialFiltro, null);
                        }

                        MoverAba("1");
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btEditarMaterial_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btExcluirMaterial_Click(object sender, EventArgs e)
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
                        int codigoFichaTecnica = 0;
                        codigoFichaTecnica = Convert.ToInt32(gvMateriais.DataKeys[row.RowIndex].Value.ToString());

                        var fichaTecnica = desenvController.ObterFichaTecnicaCodigo(codigoFichaTecnica);
                        if (fichaTecnica != null)
                        {
                            //VALIDAR EXCLUSAO DE MATERIAL PRODUTO
                            /*
                            if (desenvController.ValidarExclusaoMaterial(txtModelo.Text.Trim(), codigoCor) == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('Esta COR já possui movimento.', { header: 'Aviso', life: 3000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                                return;
                            }*/

                            desenvController.ExcluirFichaTecnica(codigoFichaTecnica);

                            CarregarMateriais(Convert.ToInt32(hidCodigoProduto.Value));
                        }

                        hidAcaoMaterial.Value = "";
                        hidDetalhe.Value = "";

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btExcluirMaterial_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btIncluirMaterial_Click(object sender, EventArgs e)
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
                        LimparCampos(false);

                        hidAcaoMaterial.Value = "I";
                        hidDetalhe.Value = "";

                        MoverAba("1");
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btIncluirMaterial_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSalvarMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                string material = "";
                labErro.Text = "";
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho.";
                    return;
                }

                if (hidAcaoMaterial.Value != "A")
                {
                    if (!ValidarFichaTecnica(Convert.ToInt32(hidCodigoProduto.Value), ddlDetalhe.SelectedValue))
                    {
                        labErro.Text = "Não será possível inserir este Material. Já existe um Material inserido com este Detalhe.";
                        return;
                    }
                }
                else
                {
                    if (hidDetalhe.Value != "" && hidDetalhe.Value != ddlDetalhe.SelectedValue)
                    {
                        if (!ValidarFichaTecnica(Convert.ToInt32(hidCodigoProduto.Value), ddlDetalhe.SelectedValue))
                        {
                            labErro.Text = "Não será possível inserir este Material. Já existe um Material inserido com este Detalhe.";
                            return;
                        }
                    }
                }

                DESENV_PRODUTO_FICTEC _fichaTecnica = new DESENV_PRODUTO_FICTEC();

                //Procurar codigo do Material
                var _material = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == ddlMaterialGrupo.SelectedItem.Text.Trim() &&
                                                                            p.SUBGRUPO.Trim() == ddlMaterialSubGrupo.SelectedItem.Text.Trim());


                if (_material == null || _material.Count() <= 0)
                {
                    labErro.Text = "Material não encontrado. Entre em contato com TI.";
                    return;
                }

                string descricaoFornecedor = "";
                string[] auxDescFornecedor;
                auxDescFornecedor = ddlCor.SelectedItem.Text.Trim().Split('-');
                if (auxDescFornecedor != null && auxDescFornecedor.Count() > 1)
                    descricaoFornecedor = auxDescFornecedor[1].Trim();

                string cor = "";
                string[] auxCor;
                auxCor = ddlCor.SelectedValue.Trim().Split('-');
                if (auxCor != null && auxCor.Count() > 1)
                    cor = auxCor[0].Trim();

                foreach (MATERIAI m in _material)
                {
                    var _materialCor = desenvController.ObterMaterialCor().Where(p => p.MATERIAL.Trim() == m.MATERIAL.Trim() && p.COR_MATERIAL.Trim() == cor && p.REFER_FABRICANTE.Trim().Contains(descricaoFornecedor.Trim()));
                    if (_materialCor != null && _materialCor.Count() > 0)
                    {
                        material = _materialCor.Take(1).SingleOrDefault().MATERIAL;
                        break;
                    }
                }

                _fichaTecnica.DESENV_PRODUTO = Convert.ToInt32(hidCodigoProduto.Value);
                _fichaTecnica.MATERIAL = material;
                if (ddlDetalhe.SelectedValue != "99")
                    _fichaTecnica.PROD_DETALHE = Convert.ToInt32(ddlDetalhe.SelectedValue);
                _fichaTecnica.GRUPO = ddlMaterialGrupo.SelectedItem.Text;
                _fichaTecnica.SUBGRUPO = ddlMaterialSubGrupo.SelectedItem.Text;
                _fichaTecnica.COR_MATERIAL = cor;
                _fichaTecnica.COR_FORNECEDOR = descricaoFornecedor;
                _fichaTecnica.FORNECEDOR = ddlFornecedor.SelectedValue;
                _fichaTecnica.CONSUMO = Convert.ToDecimal(txtConsumo.Text);
                _fichaTecnica.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _fichaTecnica.DATA_INCLUSAO = DateTime.Now;

                if (hidCodigoFichaTecnica.Value == "" || hidCodigoFichaTecnica.Value == "0")
                    desenvController.InserirFichaTecnica(_fichaTecnica);
                else
                {
                    _fichaTecnica.CODIGO = Convert.ToInt32(hidCodigoFichaTecnica.Value);
                    desenvController.AtualizarFichaTecnica(_fichaTecnica);
                }

                /*if (material.Trim() != "" && material.Trim() != "00.00.0000")
                {
                    var corExiste = desenvController.ObterMaterialCor(material, ddlCor.SelectedValue);
                    if (corExiste == null)
                    {
                        MATERIAIS_CORE _materialCor = new MATERIAIS_CORE();
                        _materialCor.MATERIAL = material;
                        _materialCor.COR_MATERIAL = ddlCor.SelectedValue;
                        _materialCor.DESC_COR_MATERIAL = ddlCor.SelectedItem.Text;
                        _materialCor.REFER_FABRICANTE = txtRefCor.Text;
                        _materialCor.CUSTO_REPOSICAO = 0;
                        _materialCor.CUSTO_A_VISTA = 0;
                        _materialCor.VARIANTE_DESENHO = "";
                        _materialCor.COMPOSICAO = "000000";
                        _materialCor.CLASSIF_FISCAL = "0000000000"; // PENDENTE TRIBUTACAO
                        _materialCor.VERSAO_FICHA = "00001";
                        desenvController.InserirMaterialCor(_materialCor);
                    }
                }*/

                //Recarregar
                CarregarMateriais(Convert.ToInt32(hidCodigoProduto.Value));
                LimparCampos(false);

                hidAcaoMaterial.Value = "";
                hidDetalhe.Value = "";
                MoverAba("0");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btCancelarMaterial_Click(object sender, EventArgs e)
        {
            LimparCampos(false);

            hidAcaoMaterial.Value = "";
            hidDetalhe.Value = "";
            MoverAba("0");
        }
        protected void txtMaterialFiltro_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                try
                {
                    labErro.Text = "";
                    if (txt.Text.Trim().Length == 10)
                    {
                        string material = txt.Text.Trim();
                        string codigoGrupo = "";
                        string grupo = "";
                        string codigoSubGrupo = "";
                        string subGrupo = "";
                        string fornecedor = "";
                        string cor = "";
                        string corRef = "";
                        string detalhe = "99";
                        string consumo = "";

                        DESENV_PRODUTO_FICTEC _produtoFichaTecnica = null;
                        if (hidCodigoFichaTecnica.Value != "")
                        {
                            _produtoFichaTecnica = desenvController.ObterFichaTecnicaCodigo(Convert.ToInt32(hidCodigoFichaTecnica.Value));
                            if (_produtoFichaTecnica != null)
                            {
                                grupo = _produtoFichaTecnica.GRUPO;
                                subGrupo = _produtoFichaTecnica.SUBGRUPO;
                                fornecedor = _produtoFichaTecnica.FORNECEDOR.Trim();
                                cor = _produtoFichaTecnica.COR_MATERIAL.Trim() + "-" + _produtoFichaTecnica.COR_FORNECEDOR.Trim();
                                corRef = _produtoFichaTecnica.COR_FORNECEDOR;
                                detalhe = (_produtoFichaTecnica.PROD_DETALHE == null) ? "99" : _produtoFichaTecnica.PROD_DETALHE.ToString();
                                consumo = _produtoFichaTecnica.CONSUMO.ToString();
                            }
                        }

                        var _material = desenvController.ObterMaterial(material);
                        if (_material != null)
                        {
                            codigoGrupo = desenvController.ObterMaterialGrupo(_material.GRUPO).CODIGO_GRUPO;
                            codigoSubGrupo = desenvController.ObterMaterialSubGrupo(_material.GRUPO, _material.SUBGRUPO).CODIGO_SUBGRUPO;
                        }

                        //PREENCHER CAMPOS
                        ddlMaterialGrupo.SelectedValue = codigoGrupo;
                        ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupo, null);

                        ddlMaterialSubGrupo.SelectedValue = codigoSubGrupo;
                        ddlMaterialSubGrupo_SelectedIndexChanged(ddlMaterialSubGrupo, null);

                        var _fornecedor = prodController.ObterFornecedor().Where(p => p.FORNECEDOR != null && p.FORNECEDOR.Trim() == fornecedor.Trim());
                        if (_fornecedor != null && _fornecedor.Count() > 0)
                        {
                            CarregarFornecedores("", "");
                            ddlFornecedor.SelectedValue = _fornecedor.Take(1).SingleOrDefault().FORNECEDOR.Trim();
                        }

                        if (_produtoFichaTecnica != null && (_produtoFichaTecnica.MATERIAL.Trim() == txt.Text.Trim()))
                            ddlCor.SelectedValue = cor;

                        //txtRefCor.Text = corRef;
                        ddlDetalhe.SelectedValue = detalhe;

                        txtConsumo.Text = consumo;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }

        }
        #endregion

        #region "FOTO"
        private void CarregarProdutoFoto(DESENV_PRODUTO _produto)
        {
            if (_produto != null)
            {
                imgFotoPeca.ImageUrl = _produto.FOTO;
                imgFotoPeca2.ImageUrl = _produto.FOTO2;
            }
        }
        #endregion

        #region "COPIA"
        private void CarregarModeloCopia(string modelo)
        {
            List<DESENV_PRODUTO> _produto = new List<DESENV_PRODUTO>();
            _produto = desenvController.ObterProduto().Where(p => p.MODELO != null &&
                                                                    p.MODELO != "" &&
                                                                    p.MODELO.Contains(modelo)).ToList();

            gvModeloCopia.DataSource = _produto;
            gvModeloCopia.DataBind();
        }
        protected void ibtPesquisarModelo_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (txtModeloCopia.Text.Trim() == "")
                {
                    labErro.Text = "Informe um Modelo para cópia.";
                    return;
                }

                CarregarModeloCopia(txtModeloCopia.Text.ToUpper().Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvModeloCopia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    colunaCopia += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaCopia.ToString();

                        Label _labNome = e.Row.FindControl("labNome") as Label;
                        if (_labNome != null)
                        {
                            var produtoLinx = new BaseController().BuscaProduto(_produto.MODELO);
                            if (produtoLinx != null)
                                _labNome.Text = produtoLinx.DESC_PRODUTO.Trim();
                        }

                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                        {
                            var _cor = prodController.ObterCoresBasicas(_produto.COR);
                            if (_cor != null)
                                _labCor.Text = _cor.DESC_COR.Trim();
                        }

                        //Popular GRID VIEW FILHO
                        if (_produto.FOTO != null && _produto.FOTO.Trim() != "")
                        {
                            GridView gvFotoCopia = e.Row.FindControl("gvFotoCopia") as GridView;
                            if (gvFotoCopia != null)
                            {
                                List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = _produto.CODIGO, FOTO = _produto.FOTO, FOTO2 = _produto.FOTO2 });
                                gvFotoCopia.DataSource = _fotoProduto;
                                gvFotoCopia.DataBind();
                            }
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }

                        ImageButton _btCopiar = e.Row.FindControl("btCopiar") as ImageButton;
                        if (_btCopiar != null)
                            _btCopiar.CommandArgument = _produto.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvFotoCopia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO;

                        System.Web.UI.WebControls.Image _imgFotoPeca2 = e.Row.FindControl("imgFotoPeca2") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca2 != null)
                            _imgFotoPeca2.ImageUrl = _produto.FOTO2;
                    }
                }
            }
        }
        protected void btCopiar_Click(object sender, EventArgs e)
        {
            ImageButton ibt = (ImageButton)sender;
            if (ibt != null)
            {
                try
                {
                    labErro.Text = "";

                    int codigoProduto = Convert.ToInt32(ibt.CommandArgument);

                    var ftExiste = desenvController.ObterFichaTecnica(Convert.ToInt32(hidCodigoProduto.Value));
                    if (ftExiste != null && ftExiste.Count() > 0)
                    {
                        labErro.Text = "Este Modelo já possui Ficha Técnica.";
                        return;
                    }

                    List<DESENV_PRODUTO_FICTEC> _fichaTecnicaCopia = new List<DESENV_PRODUTO_FICTEC>();
                    _fichaTecnicaCopia = desenvController.ObterFichaTecnica(codigoProduto);
                    if (_fichaTecnicaCopia == null || _fichaTecnicaCopia.Count() <= 0)
                    {
                        labErro.Text = "O Modelo de Cópia não possui Ficha Técnica.";
                        return;
                    }

                    DESENV_PRODUTO_FICTEC fichaTecnica = null;
                    foreach (DESENV_PRODUTO_FICTEC ft in _fichaTecnicaCopia)
                    {
                        fichaTecnica = new DESENV_PRODUTO_FICTEC();
                        fichaTecnica.DESENV_PRODUTO = Convert.ToInt32(hidCodigoProduto.Value);
                        fichaTecnica.PROD_DETALHE = ft.PROD_DETALHE;
                        fichaTecnica.MATERIAL = ft.MATERIAL;
                        fichaTecnica.GRUPO = ft.GRUPO;
                        fichaTecnica.SUBGRUPO = ft.SUBGRUPO;
                        fichaTecnica.COR_MATERIAL = ft.COR_MATERIAL;
                        fichaTecnica.COR_FORNECEDOR = ft.COR_FORNECEDOR;
                        fichaTecnica.FORNECEDOR = ft.FORNECEDOR;
                        fichaTecnica.CONSUMO = ft.CONSUMO;
                        fichaTecnica.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        fichaTecnica.DATA_INCLUSAO = DateTime.Now;
                        desenvController.InserirFichaTecnica(fichaTecnica);
                    }

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

        #region "FILTROS"
        private void CarregarColecaoFiltro()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecaoFiltro.DataSource = _colecoes;
                ddlColecaoFiltro.DataBind();
            }
        }
        protected void ddlColecaoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_ProdutoFiltro = ObterProduto();
            CarregarOrigemFiltro();
        }
        private void CarregarOrigemFiltro()
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = null;

            if (ddlColecaoFiltro.SelectedValue.Trim() != "")
                _origem = desenvController.ObterProdutoOrigem(ddlColecaoFiltro.SelectedValue);
            else
                _origem = desenvController.ObterProdutoOrigem();

            if (_origem != null)
            {
                _origem = _origem.Where(i => g_ProdutoFiltro.Any(g => g.DESENV_PRODUTO_ORIGEM == i.CODIGO)).ToList();
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigemFiltro.DataSource = _origem;
                ddlOrigemFiltro.DataBind();
            }
        }
        private void CarregarGrupoProdutoFiltro()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");
            if (_grupo != null)
            {
                _grupo = _grupo.Where(i => g_ProdutoFiltro.Any(g => g.GRUPO.Trim() == i.GRUPO_PRODUTO.Trim())).ToList();
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoProdutoFiltro.DataSource = _grupo;
                ddlGrupoProdutoFiltro.DataBind();
            }
        }
        private void CarregarTecidoFiltro()
        {
            List<DESENV_PRODUTO> tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = g_ProdutoFiltro.Where(p => p.TECIDO_POCKET != null).Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();
            foreach (var item in _tecido)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecidoFiltro.DataSource = tecidoFiltro;
            ddlTecidoFiltro.DataBind();
        }
        private void CarregarFornecedorFiltro()
        {
            List<PROD_FORNECEDOR> _fornecedor = prodController.ObterFornecedor().Where(p => p.STATUS == 'A' && p.TIPO == 'T').ToList();
            if (_fornecedor != null)
            {
                _fornecedor = _fornecedor.Where(i => g_ProdutoFiltro.Where(j => j.FORNECEDOR_COR != null).Any(g => g.FORNECEDOR.Trim() == i.FORNECEDOR.Trim())).ToList();
                _fornecedor.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });
                ddlFornecedorFiltro.DataSource = _fornecedor;
                ddlFornecedorFiltro.DataBind();
            }
        }
        private void CarregarGriffeFiltro()
        {
            List<DESENV_PRODUTO> griffeFiltro = new List<DESENV_PRODUTO>();

            var _griffe = g_ProdutoFiltro.Where(p => p.GRIFFE != null).Select(s => s.GRIFFE.Trim()).Distinct().ToList();
            foreach (var item in _griffe)
                if (item.Trim() != "")
                    griffeFiltro.Add(new DESENV_PRODUTO { GRIFFE = item.Trim() });

            griffeFiltro = griffeFiltro.OrderBy(p => p.GRIFFE).ToList();
            griffeFiltro.Insert(0, new DESENV_PRODUTO { GRIFFE = "" });
            ddlGriffeFiltro.DataSource = griffeFiltro;
            ddlGriffeFiltro.DataBind();
        }
        private void CarregarFiltros()
        {
            g_ProdutoFiltro = ObterProduto();

            CarregarColecaoFiltro();
            CarregarOrigemFiltro();
            CarregarGrupoProdutoFiltro();
            CarregarTecidoFiltro();
            CarregarFornecedorFiltro();
            CarregarGriffeFiltro();

            CarregarGrupos(ddlMaterialGrupoFiltro);
        }
        private List<DESENV_PRODUTO> ObterProduto()
        {
            return desenvController.ObterProduto();
        }
        #endregion

        #region "NOTIFICACAO"
        private void CarregarNotificacao()
        {
            List<NOTIFICACAO> _notificacao = new List<NOTIFICACAO>();
            _notificacao = new NotificacaoController().ObterNotificacao(1).Where(p => p.DATA_LEITURA == null).ToList();

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
                        DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                        _produto = desenvController.ObterProduto(_notificacao.CODIGO_TABELA);
                        if (_produto != null)
                        {
                            Literal _col = e.Row.FindControl("litColuna") as Literal;
                            if (_col != null)
                                _col.Text = colunaNotificacao.ToString();

                            Label _labModelo = e.Row.FindControl("labModelo") as Label;
                            if (_labModelo != null)
                                _labModelo.Text = _produto.MODELO;

                            Label _labNome = e.Row.FindControl("labNome") as Label;
                            if (_labNome != null)
                            {
                                var produtoLinx = new BaseController().BuscaProduto(_produto.MODELO);
                                if (produtoLinx != null)
                                    _labNome.Text = produtoLinx.DESC_PRODUTO.Trim();
                            }

                            Label _labCor = e.Row.FindControl("labCor") as Label;
                            if (_labCor != null)
                            {
                                var _cor = prodController.ObterCoresBasicas(_produto.COR);
                                if (_cor != null)
                                    _labCor.Text = _cor.DESC_COR.Trim();
                            }

                            Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            if (_labCorFornecedor != null)
                                _labCorFornecedor.Text = _produto.FORNECEDOR_COR;

                            Label _labMsg = e.Row.FindControl("labMsg") as Label;
                            if (_labMsg != null)
                            {
                                _labMsg.Text = _notificacao.MENSAGEM;
                                if (_notificacao.MENSAGEM.ToUpper().Contains("DESAPROVADO"))
                                    e.Row.ForeColor = Color.Red;
                            }

                            //Popular GRID VIEW FILHO
                            if (_produto.FOTO != null && _produto.FOTO.Trim() != "")
                            {
                                GridView gvFoto = e.Row.FindControl("gvNotificacaoFoto") as GridView;
                                if (gvFoto != null)
                                {
                                    List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                    _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = _produto.CODIGO, FOTO = _produto.FOTO, FOTO2 = _produto.FOTO2 });
                                    gvFoto.DataSource = _fotoProduto;
                                    gvFoto.DataBind();
                                }
                            }
                            else
                            {
                                System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                                if (img != null)
                                    img.Visible = false;
                            }

                            ImageButton _ibtMarcarLido = e.Row.FindControl("btMarcarLido") as ImageButton;
                            if (_ibtMarcarLido != null)
                                _ibtMarcarLido.CommandArgument = _notificacao.CODIGO.ToString();

                            ImageButton _ibtPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                            if (_ibtPesquisar != null)
                                _ibtPesquisar.CommandArgument = _produto.CODIGO.ToString();
                        }
                    }
                }
            }
        }
        protected void gvNotificacaoFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO;

                        System.Web.UI.WebControls.Image _imgFotoPeca2 = e.Row.FindControl("imgFotoPeca2") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca2 != null)
                            _imgFotoPeca2.ImageUrl = _produto.FOTO2;
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
        private void CarregarProdutoTodos(List<DESENV_PRODUTO> _listaProduto)
        {
            List<DESENV_PRODUTO> _produto = new List<DESENV_PRODUTO>();

            labErro.Text = "";
            if (_listaProduto == null)
            {
                gvTodos.DataSource = _produto.OrderBy(p => p.MODELO).ThenBy(x => x.COR).ThenBy(x => x.FORNECEDOR_COR);
                gvTodos.DataBind();
                return;
            }

            if (_listaProduto.Count <= 0)
                _produto = desenvController.ObterProduto().Where(p => p.MODELO != null && p.MODELO.Trim() != "" && p.STATUS == 'A').ToList();
            else
                _produto = _listaProduto;

            if (txtModelo.Text.Trim() != "")
                _produto = _produto.Where(p => p.MODELO.Trim().Contains(txtModelo.Text.Trim())).ToList();

            //Filtrar por nome
            if (txtModeloDescricao.Text.Trim() != "")
            {
                var produtoFiltro = new BaseController().BuscaProdutosDescricao(txtModeloDescricao.Text.Trim().ToUpper());
                _produto = _produto.Where(p => produtoFiltro.Any(x => x.PRODUTO1.Trim() == p.MODELO.Trim())).ToList();
            }

            if (ddlColecaoFiltro.SelectedValue.Trim() != "")
                _produto = _produto.Where(p => p.COLECAO.Trim() == ddlColecaoFiltro.SelectedValue.Trim()).ToList();

            if (ddlOrigemFiltro.SelectedValue.Trim() != "" && ddlOrigemFiltro.SelectedValue.Trim() != "0")
                _produto = _produto.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == ddlOrigemFiltro.SelectedValue.Trim()).ToList();

            if (ddlGrupoProdutoFiltro.SelectedValue.Trim() != "")
                _produto = _produto.Where(p => p.GRUPO.Trim() == ddlGrupoProdutoFiltro.SelectedValue.Trim()).ToList();

            if (ddlTecidoFiltro.SelectedValue.Trim() != "")
                _produto = _produto.Where(p => p.TECIDO_POCKET != null && p.TECIDO_POCKET.Trim() == ddlTecidoFiltro.SelectedValue.Trim()).ToList();

            if (ddlFornecedorFiltro.SelectedValue.Trim() != "")
                _produto = _produto.Where(p => p.FORNECEDOR != null && p.FORNECEDOR.Trim() == ddlFornecedorFiltro.SelectedValue.Trim()).ToList();

            if (ddlGriffeFiltro.SelectedValue.Trim() != "")
                _produto = _produto.Where(p => p.GRIFFE != null && p.GRIFFE.Trim() == ddlGriffeFiltro.SelectedValue.Trim()).ToList();

            if (chkSemFicha.Checked)
                _produto = _produto.Where(p => p.DATA_FICHATECNICA == null).ToList();

            //Filtros LIKE
            //if (txtCor.Text.Trim() != "")
            //    _produto = _produto.Where(p => p.COR != null && p.COR.Contains(txtCor.Text.Trim().ToUpper())).ToList();

            if (txtCorFornecedor.Text.Trim() != "")
                _produto = _produto.Where(p => p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Contains(txtCorFornecedor.Text.Trim().ToUpper())).ToList();

            if (txtFornecedor.Text.Trim() != "")
                _produto = _produto.Where(p => p.FORNECEDOR != null && p.FORNECEDOR.Contains(txtFornecedor.Text.Trim().ToUpper())).ToList();

            if (txtTecido.Text.Trim() != "")
                _produto = _produto.Where(p => p.TECIDO_POCKET != null && p.TECIDO_POCKET.Contains(txtTecido.Text.Trim().ToUpper())).ToList();

            //FILTRAR POR FICHA TECNICA

            if (ddlMaterialGrupoFiltro.SelectedValue.Trim() != "" || ddlMaterialSubGrupoFiltro.SelectedValue.Trim() != "" || ddlMaterialCorFiltro.SelectedValue.Trim() != "")
            {
                var fichaTecnica = desenvController.ObterFichaTecnica();

                if (ddlMaterialGrupoFiltro.SelectedValue.Trim() != "")
                    fichaTecnica = fichaTecnica.Where(p => p.GRUPO.Trim() == ddlMaterialGrupoFiltro.SelectedItem.Text.Trim()).ToList();

                if (ddlMaterialSubGrupoFiltro.SelectedValue.Trim() != "")
                    fichaTecnica = fichaTecnica.Where(p => p.SUBGRUPO.Trim() == ddlMaterialSubGrupoFiltro.SelectedItem.Text.Trim()).ToList();

                if (ddlMaterialCorFiltro.SelectedValue.Trim() != "")
                    fichaTecnica = fichaTecnica.Where(p => (p.COR_MATERIAL.Trim() + "-" + p.COR_FORNECEDOR.Trim().ToUpper()) == ddlMaterialCorFiltro.SelectedValue.Trim()).ToList();

                _produto = _produto.Where(p => fichaTecnica.Any(x => p.CODIGO == x.DESENV_PRODUTO)).ToList();
            }

            if (_produto == null || _produto.Count <= 0)
                labErro.Text = "Nenhum produto encontrado. Refaça sua pesquisa.";

            gvTodos.DataSource = _produto.OrderBy(p => p.MODELO).ThenBy(x => x.COR).ThenBy(x => x.FORNECEDOR_COR);
            gvTodos.DataBind();
        }
        protected void gvTodos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    colunaTodos += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaTodos.ToString();

                        Label _labNome = e.Row.FindControl("labNome") as Label;
                        if (_labNome != null)
                        {
                            var produtoLinx = new BaseController().BuscaProduto(_produto.MODELO);
                            if (produtoLinx != null)
                                _labNome.Text = produtoLinx.DESC_PRODUTO.Trim();
                        }

                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                        {
                            var _cor = prodController.ObterCoresBasicas(_produto.COR);
                            if (_cor != null)
                                _labCor.Text = _cor.DESC_COR.Trim();
                        }

                        //Popular GRID VIEW FILHO
                        if (_produto.FOTO != null && _produto.FOTO.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = _produto.CODIGO, FOTO = _produto.FOTO, FOTO2 = _produto.FOTO2 });
                                gvFoto.DataSource = _fotoProduto;
                                gvFoto.DataBind();
                            }
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }

                        ImageButton _ibtPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                        if (_ibtPesquisar != null)
                            _ibtPesquisar.CommandArgument = _produto.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO;

                        System.Web.UI.WebControls.Image _imgFotoPeca2 = e.Row.FindControl("imgFotoPeca2") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca2 != null)
                            _imgFotoPeca2.ImageUrl = _produto.FOTO2;
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
                    //CarregarProdutoTodos(null);

                    string codigoProduto = ibt.CommandArgument;
                    hidCodigoProduto.Value = codigoProduto.ToString();
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
        private bool ValidarFichaTecnica(int desenv_produto, string detalhe)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;


            var _fichaTecnica = new List<DESENV_PRODUTO_FICTEC>();
            if (detalhe == "99")//PRINCIPAL
            {
                _fichaTecnica = desenvController.ObterFichaTecnica(desenv_produto).Where(p => p.PROD_DETALHE == null).ToList();
            }
            else
            {
                _fichaTecnica = desenvController.ObterFichaTecnica(desenv_produto).Where(p => p.PROD_DETALHE.ToString() == detalhe).ToList();
            }

            if (_fichaTecnica != null && _fichaTecnica.Count() > 0)
                retorno = false;

            return retorno;
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

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

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue.Trim() == "")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labDetalhe.ForeColor = _OK;
            if (ddlDetalhe.SelectedValue.Trim() == "")
            {
                labDetalhe.ForeColor = _notOK;
                retorno = false;
            }

            labConsumo.ForeColor = _OK;
            if (txtConsumo.Text.Trim() == "" || Convert.ToDecimal(txtConsumo.Text) <= 0)
            {
                labConsumo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {
            txtModelo.Enabled = enable;
            txtModeloDescricao.Enabled = enable;
            txtCor.Enabled = enable;
            txtCorFornecedor.Enabled = enable;
            txtFornecedor.Enabled = enable;
            txtTecido.Enabled = enable;
            chkGradeado.Enabled = enable;

            //MATERIAIS
            txtMaterialFiltro.Enabled = enable;
            ddlMaterialGrupo.Enabled = enable;
            ddlMaterialSubGrupo.Enabled = enable;
            ddlFornecedor.Enabled = enable;
            ddlCor.Enabled = enable;
            txtCorFornecedor.Enabled = enable;
            ddlDetalhe.Enabled = enable;
            txtConsumo.Enabled = enable;
            btSalvarMaterial.Visible = enable;
            btCancelarMaterial.Visible = enable;

            foreach (GridViewRow row in gvMateriais.Rows)
                if (row != null)
                {
                    ImageButton _btEditarMaterial = row.FindControl("btEditarMaterial") as ImageButton;
                    if (_btEditarMaterial != null)
                        _btEditarMaterial.Visible = enable;
                    ImageButton _btExcluirMaterial = row.FindControl("btExcluirMaterial") as ImageButton;
                    if (_btExcluirMaterial != null)
                        _btExcluirMaterial.Visible = enable;
                }

            GridViewRow footer = gvMateriais.FooterRow;
            if (footer != null)
            {
                ImageButton _btIncluirMaterial = footer.FindControl("btIncluirMaterial") as ImageButton;
                if (_btIncluirMaterial != null)
                    _btIncluirMaterial.Visible = enable;
            }

        }
        private void LimparCampos(bool _cabecalho)
        {
            //Recarregar novamente, não me pergunte porque...
            CarregarGrupos(ddlMaterialGrupo);
            //***********************************************

            hidCodigoFichaTecnica.Value = "";
            labErro.Text = "";

            //MATERIAL
            txtMaterialFiltro.Text = "";
            ddlMaterialGrupo.SelectedValue = "";
            ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupo, null);
            ddlMaterialGrupoFiltro.SelectedValue = "";
            ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupoFiltro, null);

            ddlMaterialSubGrupo.SelectedValue = "";
            ddlMaterialSubGrupo_SelectedIndexChanged(ddlMaterialSubGrupo, null);
            ddlMaterialSubGrupoFiltro.SelectedValue = "";
            ddlMaterialSubGrupo_SelectedIndexChanged(ddlMaterialSubGrupoFiltro, null);

            ddlFornecedor.SelectedValue = "";
            ddlCor.SelectedValue = "";
            ddlMaterialCorFiltro.SelectedValue = "";
            txtRefCor.Text = "";
            ddlDetalhe.SelectedValue = "99";
            txtConsumo.Text = "";

            //FILTROS
            ddlColecaoFiltro.SelectedValue = "";
            ddlOrigemFiltro.SelectedValue = "0";
            ddlGrupoProdutoFiltro.SelectedValue = "";
            ddlTecidoFiltro.SelectedValue = "";
            ddlFornecedorFiltro.SelectedValue = "";
            ddlGriffeFiltro.SelectedValue = "";
            chkSemFicha.Checked = false;

            //COPIA
            txtModeloCopia.Text = "";
            CarregarModeloCopia("00000");

            //FOTO
            if (_cabecalho)
            {
                txtModelo.Text = "";
                txtModeloDescricao.Text = "";
                txtCor.Text = "";
                txtCorFornecedor.Text = "";
                txtFornecedor.Text = "";
                txtTecido.Text = "";
                chkGradeado.Checked = false;

                imgFotoPeca.ImageUrl = "";
                imgFotoPeca2.ImageUrl = "";

                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                btSalvarMaterial.Visible = false;
                btCancelarMaterial.Visible = false;
                ibtAprovarFichaTecnica.Visible = false;
                ibtAbrirFichaTecnica.Visible = false;
                CarregarMateriais(0);
            }
        }
        private void LerNotificacao(int codigoProduto)
        {
            var not = notController.ObterNotificacao(1).Where(p => p.CODIGO_TABELA == codigoProduto);
            foreach (NOTIFICACAO n in not)
            {
                n.USUARIO_LEITURA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                n.DATA_LEITURA = DateTime.Now;
                notController.AtualizarNotificacao(n);
            }

            CarregarNotificacao();
        }
        #endregion

        protected void ibtCopiarHB_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                labErro.Text = "";
                if (hidCodigoProduto.Value.Trim() == "")
                {
                    labErro.Text = "Produto não informado. Refaça sua pesquisa.";
                    return;
                }

                var desenvProduto = desenvController.ObterProduto(Convert.ToInt32(hidCodigoProduto.Value));
                if (desenvProduto != null)
                {
                    var hbPAI = prodController.ObterHB(desenvProduto.MODELO, desenvProduto.COR);
                    if (hbPAI != null)
                    {
                        CriarFichaTecnica(desenvProduto, hbPAI);

                        var detalhes = prodController.ObterDetalhesHB(hbPAI.CODIGO);
                        foreach (var hb in detalhes)
                        {
                            CriarFichaTecnica(desenvProduto, hb);
                        }

                        ibtPesquisar_Click(null, null);
                        labErro.Text = "Ficha Técnica criada com sucesso.";
                    }
                    else
                    {
                        labErro.Text = "HB não encontrado. Verifique se existe o HB (Produto + Cor) ou insira os dados manualmente.";
                        return;
                    }
                }
                else
                {
                    labErro.Text = "Produto não encontrado. Refaça sua pesquisa.";
                    return;
                }


            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CriarFichaTecnica(DESENV_PRODUTO _produto, PROD_HB prod_hb)
        {
            if (_produto != null)
            {
                //Se nao existe ficha tecnica, cria
                var ficTecDetalhe = desenvController.ObterFichaTecnica(_produto.CODIGO).Where(p => p.PROD_DETALHE == prod_hb.PROD_DETALHE);
                if (ficTecDetalhe == null || ficTecDetalhe.Count() == 0)
                {
                    string materialCodigo = "";
                    var material = desenvController.ObterMaterial(prod_hb.GRUPO_TECIDO, prod_hb.TECIDO);
                    foreach (var m in material)
                    {
                        var materialCor = desenvController.ObterMaterialCor(m.MATERIAL, prod_hb.COR, prod_hb.COR_FORNECEDOR);
                        if (materialCor != null)
                        {
                            materialCodigo = materialCor.MATERIAL;
                            break;
                        }
                    }

                    DESENV_PRODUTO_FICTEC fictec = new DESENV_PRODUTO_FICTEC();
                    fictec.DESENV_PRODUTO = _produto.CODIGO;
                    fictec.PROD_DETALHE = prod_hb.PROD_DETALHE;
                    fictec.MATERIAL = materialCodigo;
                    fictec.GRUPO = prod_hb.GRUPO_TECIDO;
                    fictec.SUBGRUPO = prod_hb.TECIDO;
                    fictec.COR_MATERIAL = prod_hb.COR;
                    fictec.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                    fictec.FORNECEDOR = prod_hb.FORNECEDOR;
                    fictec.CONSUMO = (prod_hb.GASTO_FOLHA == null) ? 0 : Convert.ToDecimal(prod_hb.GASTO_FOLHA);
                    fictec.USUARIO_INCLUSAO = ((USUARIO)(Session["USUARIO"])).CODIGO_USUARIO;
                    fictec.DATA_INCLUSAO = DateTime.Now;
                    desenvController.InserirFichaTecnica(fictec);

                    //se for principal, cria aviamentos
                    if (prod_hb.PROD_DETALHE == null)
                    {
                        int prodDet = 7;
                        var avi = prodController.ObterAviamentoHB(prod_hb.CODIGO);
                        foreach (var a in avi)
                        {
                            materialCodigo = "";
                            string fornecedor = "";
                            var materialAvia = desenvController.ObterMaterial(a.GRUPO, a.SUBGRUPO);
                            foreach (var m in materialAvia)
                            {
                                var materialCor = desenvController.ObterMaterialCor(m.MATERIAL, a.COR, a.COR_FORNECEDOR);
                                if (materialCor != null)
                                {
                                    materialCodigo = materialCor.MATERIAL;
                                    fornecedor = m.FABRICANTE;
                                    break;
                                }
                            }

                            fictec = new DESENV_PRODUTO_FICTEC();
                            fictec.DESENV_PRODUTO = _produto.CODIGO;
                            fictec.PROD_DETALHE = prodDet;
                            fictec.MATERIAL = materialCodigo;
                            fictec.GRUPO = a.GRUPO;
                            fictec.SUBGRUPO = a.SUBGRUPO;
                            fictec.COR_MATERIAL = a.COR;
                            fictec.COR_FORNECEDOR = a.COR_FORNECEDOR;
                            fictec.FORNECEDOR = fornecedor;
                            fictec.CONSUMO = (a.QTDE_POR_PECA == null) ? 0 : Convert.ToDecimal(a.QTDE_POR_PECA);
                            fictec.USUARIO_INCLUSAO = ((USUARIO)(Session["USUARIO"])).CODIGO_USUARIO;
                            fictec.DATA_INCLUSAO = DateTime.Now;
                            desenvController.InserirFichaTecnica(fictec);

                            prodDet += 1;
                        }
                    }

                }
            }
        }

        protected void chkGradeado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string codigoDesenvProduto = hidCodigoProduto.Value;

                var desenvProduto = desenvController.ObterProduto(Convert.ToInt32(codigoDesenvProduto));
                if (desenvProduto != null)
                {
                    desenvProduto.GRADEADO = chkGradeado.Checked;
                    desenvController.AtualizarProduto(desenvProduto);
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}