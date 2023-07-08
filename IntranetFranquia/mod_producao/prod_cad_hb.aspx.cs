using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class prod_cad_hb : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int colunaComposicao, colunaAviamento, colunaDetalhe, colunaComposicaoDet = 0;
        int colAmpliacaoElastico, colAmpliacaoGalao, colAmpliacaoAlcaPronta = 0;

        PROD_GRADE _gradeNome = new PROD_GRADE();

        protected void Page_Load(object sender, EventArgs e)
        {
            //RecarregarAviamentoDropDownList();

            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                    Response.Redirect("prod_menu.aspx");

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                    Response.Redirect("prod_menu.aspx");

                CarregarGrupo();
                CarregarGrupoMaterial("");
                CarregarColecoes();
                CarregarFornecedores();
                CarregarCores("0");

                hidTela.Value = tela;

                if (tela == "1")
                {
                    //Criar HBs
                    CriarHB();
                    txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    ddlCorFornecedor.Items.Add("");
                }

                //Mostra opção de Cópia
                if (tela == "2")
                {
                    CriarHB();
                    pnlHBCopia.Visible = true;
                    //rdbAmpliacao.Visible = false;
                    rdbRisco.Checked = true;
                    pnlCabecalho.Visible = false;

                    //Esconder controle de mostruário
                    labMostruario.Visible = false;
                    cbMostruario.Visible = false;
                }

                //Baixa Ampliacao
                if (tela == "3")
                {
                    string codigo = Request.QueryString["c"].ToString();
                    string data = Request.QueryString["d"].ToString();

                    rdbRisco.Checked = true;
                    rdbAmpliacao.Visible = false;
                    hidHB.Value = codigo;
                    hidData.Value = data.Replace("@", "/");

                    PROD_HB prod_hb = prodController.ObterHB(Convert.ToInt32(codigo));
                    if (prod_hb != null)
                    {
                        cbMostruario.Checked = (prod_hb.MOSTRUARIO == 'S') ? true : false;
                        cbMostruario.Enabled = false;
                        rdbRisco_CheckedChanged(null, null);

                        ddlColecoes.SelectedValue = prod_hb.COLECAO;
                        CarregarOrigem(prod_hb.COLECAO);
                        ddlColecoes.Enabled = false;
                        ddlColecoes_SelectedIndexChanged(ddlColecoes, null);
                        txtHB.Text = prod_hb.HB.ToString();
                        txtHB.Enabled = false;
                        txtCodigoLinx.Text = prod_hb.CODIGO_PRODUTO_LINX.ToString();
                        ddlGrupo.SelectedValue = prod_hb.GRUPO;
                        txtNome.Text = prod_hb.NOME;
                        txtData.Text = prod_hb.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                        ddlGrupoTecido.SelectedValue = prod_hb.GRUPO_TECIDO;
                        CarregarSubGrupo(prod_hb.GRUPO_TECIDO, "T");
                        ddlSubGrupoTecido.SelectedValue = prod_hb.TECIDO;

                        CarregarCorFornecedor(prod_hb.GRUPO_TECIDO, prod_hb.TECIDO, ddlCorFornecedor);
                        ddlCorFornecedor.SelectedValue = prod_hb.COR_FORNECEDOR;

                        txtPedidoNumero.Text = prod_hb.NUMERO_PEDIDO.ToString();
                        txtMolde.Text = (prod_hb.MOLDE == null) ? "0" : prod_hb.MOLDE.ToString();
                        txtObservacao.Text = prod_hb.OBSERVACAO;

                        //AMPLIACAO
                        txtOutro.Text = prod_hb.AMPLIACAO_OUTRO;

                        if (prod_hb.GABARITO != null)
                            ddlGabarito.SelectedValue = prod_hb.GABARITO.ToString();

                        if (prod_hb.ESTAMPARIA != null)
                            ddlEstamparia.SelectedValue = prod_hb.ESTAMPARIA.ToString();

                        if (prod_hb.GASTO_FOLHA_SIMULACAO != null)
                            txtGastoPorFolha.Text = prod_hb.GASTO_FOLHA_SIMULACAO.ToString();
                        if (prod_hb.PRECO_FACC_MOSTRUARIO != null)
                            txtPrecoFaccMostruario.Text = prod_hb.PRECO_FACC_MOSTRUARIO.ToString();

                        if (prod_hb.DESENV_PRODUTO_ORIGEM != null)
                            ddlOrigem.SelectedValue = prod_hb.DESENV_PRODUTO_ORIGEM.ToString();

                        ddlFornecedor.SelectedValue = prod_hb.FORNECEDOR;
                        txtModelagem.Text = prod_hb.MODELAGEM;
                        txtLargura.Text = prod_hb.LARGURA.ToString();
                        txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                        cbLiquidar.Checked = (prod_hb.LIQUIDAR == 'S') ? true : false;
                        cbLiquidarPqno.Checked = (prod_hb.LIQUIDAR_PQNO == 'S') ? true : false;
                        cbAtacado.Checked = (prod_hb.ATACADO == 'S') ? true : false;
                        imgFotoTecido.ImageUrl = (prod_hb.FOTO_TECIDO == null) ? "" : prod_hb.FOTO_TECIDO;
                        imgFotoPeca.ImageUrl = (prod_hb.FOTO_PECA == null) ? "" : prod_hb.FOTO_PECA;

                        CarregarCores(txtCodigoLinx.Text);
                        ddlCor.SelectedValue = prod_hb.COR;

                        if (prod_hb.PROD_GRADE != null)
                        {
                            ddlGrade.SelectedValue = prod_hb.PROD_GRADE.ToString();
                            ddlGrade_SelectedIndexChanged(ddlGrade, null);
                        }

                        RecarregarComposicao();
                        hidTotalComp.Value = "";
                        if (gvComposicao.FooterRow != null)
                            hidTotalComp.Value = gvComposicao.FooterRow.Cells[1].Text.ToString();

                        RecarregarDetalhe();
                        RecarregarAviamento();
                    }

                    hrefVoltar.HRef = "prod_fila_risco.aspx";
                }

                //Carregar Combos dependentes do pai
                CarregarDetalhes();
                CarregarAmpliacaoMedidas();

                //Esconder painel de detalhes
                pnlDetalhe.Visible = false;

                dialogPai.Visible = false;
                txtHB.Enabled = false;
            }

            //Evitar duplo clique no botão
            btEnviar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "Selecione" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarCores(string codigoProduto)
        {
            BaseController baseController = new BaseController();
            List<PRODUTO_CORE> produtoCor = new List<PRODUTO_CORE>();
            produtoCor = baseController.BuscaProdutoCores(codigoProduto);
            if (produtoCor == null || produtoCor.Count <= 0 || codigoProduto == "0")
            {
                List<CORES_BASICA> _cores = prodController.ObterCoresBasicas();

                foreach (CORES_BASICA c in _cores)
                    produtoCor.Add(new PRODUTO_CORE { COR_PRODUTO = c.COR, DESC_COR_PRODUTO = c.DESC_COR });

                produtoCor = produtoCor.OrderBy(p => p.DESC_COR_PRODUTO).ToList();
                produtoCor.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "0", DESC_COR_PRODUTO = "Selecione" });
                ddlDetalheCor.DataSource = produtoCor;
                ddlDetalheCor.DataBind();
            }

            if (produtoCor != null)
            {
                produtoCor = produtoCor.OrderBy(p => p.DESC_COR_PRODUTO).ToList();
                produtoCor.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "0", DESC_COR_PRODUTO = "Selecione" });
                ddlCor.DataSource = produtoCor;
                ddlCor.DataBind();

                if (produtoCor.Count == 2)
                    ddlCor.SelectedValue = produtoCor[1].COR_PRODUTO;
            }
        }
        private void CarregarCoresAviamento(string grupo, string subGrupo)
        {
            List<MATERIAI> filtroMaterial = new List<MATERIAI>();
            filtroMaterial = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();

            List<MATERIAIS_CORE> materialCores = desenvController.ObterMaterialCor();
            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

            List<CORES_BASICA> coresBasicas = new List<CORES_BASICA>();
            CORES_BASICA _corBasica = null;
            foreach (MATERIAIS_CORE c in materialCores)
            {
                _corBasica = new CORES_BASICA();
                _corBasica.COR = c.COR_MATERIAL.Trim();
                _corBasica.DESC_COR = c.DESC_COR_MATERIAL.Trim();
                coresBasicas.Add(_corBasica);
            }

            coresBasicas = coresBasicas.OrderBy(p => p.DESC_COR.Trim()).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "Selecione" });

            ddlCorAviamento.DataSource = coresBasicas;
            ddlCorAviamento.DataBind();

            if (coresBasicas.Count == 2)
            {
                ddlCorAviamento.SelectedIndex = 1;
                CarregarCorFornecedor(ddlGrupoAviamento.SelectedItem.Text.Trim(), ddlSubGrupoAviamento.SelectedItem.Text.Trim(), ddlCorFornecedorAviamento);
            }
        }
        protected void ddlCorAviamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCorAviamento.SelectedValue.Trim() != "")
            {
                CarregarCorFornecedor(ddlGrupoAviamento.SelectedItem.Text.Trim(), ddlSubGrupoAviamento.SelectedItem.Text.Trim(), ddlCorFornecedorAviamento);
            }
        }
        private void CarregarGrupoMaterial(string tipo)
        {
            List<MATERIAIS_GRUPO> _matGrupo = new DesenvolvimentoController().ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
                if (tipo == "" || tipo == "T")
                {
                    ddlGrupoTecido.DataSource = _matGrupo;
                    ddlGrupoTecido.DataBind();

                    ddlDetalheGrupoTecido.DataSource = _matGrupo;
                    ddlDetalheGrupoTecido.DataBind();
                }
                if (tipo == "" || tipo == "A")
                {
                    ddlGrupoAviamento.DataSource = _matGrupo;
                    ddlGrupoAviamento.DataBind();
                }
            }

        }
        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "ddlGrupoAviamento")
                CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "A");

            if (ddl.ID == "ddlGrupoTecido")
                CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "T");

            if (ddl.ID == "ddlDetalheGrupoTecido")
                CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "D");
        }
        private void CarregarSubGrupo(string grupo, string tipo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = new DesenvolvimentoController().ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).OrderBy(p => p.SUBGRUPO.Trim()).ToList();
            if (_matSubGrupo != null)
            {
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "Selecione" });

                if (tipo == "T")
                {
                    ddlSubGrupoTecido.DataSource = _matSubGrupo;
                    ddlSubGrupoTecido.DataBind();
                }
                else if (tipo == "D")
                {
                    ddlDetalheSubGrupoTecido.DataSource = _matSubGrupo;
                    ddlDetalheSubGrupoTecido.DataBind();
                }
                else
                {
                    ddlSubGrupoAviamento.DataSource = _matSubGrupo;
                    ddlSubGrupoAviamento.DataBind();
                }
            }

        }
        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddl = (DropDownList)sender;
            if (_ddl != null)
            {
                if (_ddl.ID == "ddlSubGrupoTecido")
                    CarregarCorFornecedor(ddlGrupoTecido.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlCorFornecedor);
                else if (_ddl.ID == "ddlDetalheSubGrupoTecido")
                    CarregarCorFornecedor(ddlDetalheGrupoTecido.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlDetalheCorFornecedor);
                else if (_ddl.ID == "ddlSubGrupoAviamento")
                    CarregarCoresAviamento(ddlGrupoAviamento.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim());
            }
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, DropDownList _ddlCorFornecedor)
        {
            //Obter materiais
            //var _material = desenvController.ObterMaterial(grupo.Trim(), subGrupo.Trim());
            //Obter cores do material
            var _matCores = desenvController.ObterMaterialCorV2(grupo.Trim(), subGrupo.Trim(), "");
            //_matCores = _matCores.Where(p => _material.Any(c => c.MATERIAL.Trim() == p.MATERIAL.Trim())).OrderBy(x => x.DESC_COR_MATERIAL).ToList();


            List<MATERIAIS_CORE> _cores = new List<MATERIAIS_CORE>();
            MATERIAIS_CORE cor = null;
            foreach (SP_OBTER_MATERIAL_CORESResult c in _matCores)
            {
                cor = new MATERIAIS_CORE();
                cor.COR_MATERIAL = c.COR_MATERIAL;
                cor.REFER_FABRICANTE = c.REFER_FABRICANTE.Trim();
                cor.DESC_COR_MATERIAL = c.DESC_COR_MATERIAL.Trim();
                _cores.Add(cor);
            }

            if (_ddlCorFornecedor.ID == "ddlCorFornecedorAviamento")
            {
                _cores = _cores.Where(p => p.COR_MATERIAL.Trim() == ddlCorAviamento.SelectedValue.Trim()).ToList();
            }

            _cores = _cores.OrderBy(p => p.REFER_FABRICANTE.Trim()).ToList();
            _cores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "Selecione" });

            _ddlCorFornecedor.DataSource = null;
            _ddlCorFornecedor.DataBind();

            _ddlCorFornecedor.DataSource = _cores;
            _ddlCorFornecedor.DataBind();

            if (_cores.Count == 2)
                _ddlCorFornecedor.SelectedIndex = 1;

        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                ddlColecaoCopia.DataSource = _colecoes;
                ddlColecaoCopia.DataBind();

                ddlColecoes_SelectedIndexChanged(ddlColecoes, null);
            }
        }
        private void CarregarOrigem(string colecao)
        {
            var origem = desenvController.ObterProdutoOrigem(colecao);

            origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "Selecione" });
            ddlOrigem.DataSource = origem;
            ddlOrigem.DataBind();
        }
        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S');
                ddlFornecedor.DataBind();

                ddlDetalheFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S');
                ddlDetalheFornecedor.DataBind();
            }

        }
        private void CarregarDetalhes()
        {
            List<PROD_DETALHE> _detalhe = prodController.ObterDetalhes().Where(x => !x.DESCRICAO.Trim().Contains("AVIAMENTO")).OrderBy(p => p.DESCRICAO).ToList();

            if (_detalhe != null)
            {
                _detalhe.Insert(0, new PROD_DETALHE { CODIGO = 0, DESCRICAO = "Selecione" });
                ddlDetalhe.DataSource = _detalhe;
                ddlDetalhe.DataBind();
            }
        }
        private static List<PROD_AVIAMENTO> ObterListaAviamentos()
        {
            return (new ProducaoController().ObterAviamento());
            /*
            List<PROD_AVIAMENTO> _aviamento = prodController.ObterAviamento();
            if (_aviamento != null)
            {
                _aviamento = _aviamento.Where(p => p.STATUS == 'A').ToList();
                _aviamento.Insert(0, new PROD_AVIAMENTO { CODIGO = 0, DESCRICAO = "Selecione" });
                ddlAviamento.DataSource = _aviamento;
                ddlAviamento.DataBind();
            }*/
        }
        [System.Web.Services.WebMethod]
        public static ArrayList CarregarAviamentos()
        {
            ArrayList list = new ArrayList();

            List<PROD_AVIAMENTO> _aviamento = ObterListaAviamentos();
            if (_aviamento != null)
            {
                _aviamento = _aviamento.Where(p => p.STATUS == 'A').ToList();
                _aviamento.Insert(0, new PROD_AVIAMENTO { CODIGO = 0, DESCRICAO = "Selecione" });

                foreach (PROD_AVIAMENTO avia in _aviamento)
                {
                    list.Add(new ListItem(avia.DESCRICAO, avia.CODIGO.ToString()));
                }
            }

            return list;
        }
        private void CarregarDropDownList(ArrayList _list, DropDownList _ddl)
        {
            _ddl.DataSource = _list;
            _ddl.DataTextField = "Text";
            _ddl.DataValueField = "Value";
            _ddl.DataBind();
        }
        private void CarregarGrades(string colecao)
        {
            var grades = prodController.ObterGradeNome();

            int colecaoNumero = 0;
            if (int.TryParse(colecao, out colecaoNumero))
            {
                if (colecaoNumero >= 70 && colecaoNumero <= 90)
                    grades = grades.Where(x => x.GRADE.ToUpper().Contains("CRIATIFF")).ToList();
                else
                    grades = grades.Where(x => !x.GRADE.ToUpper().Contains("CRIATIFF")).ToList();
            }

            grades.Insert(0, new PROD_GRADE { CODIGO = 0, GRADE = "Selecione" });
            ddlGrade.DataSource = grades;
            ddlGrade.DataBind();

            ddlGrade_SelectedIndexChanged(ddlGrade, null);
        }
        #endregion

        #region "CONTROLE DO NOME DAS GRADES"
        private void BloquearCamposGrade(PROD_GRADE gradeNome)
        {
            txtGradeP.Enabled = (gradeNome.GRADE_P == "-") ? false : true;
            txtGradeM.Enabled = (gradeNome.GRADE_M == "-") ? false : true;
            txtGradeG.Enabled = (gradeNome.GRADE_G == "-") ? false : true;
            txtGradeGG.Enabled = (gradeNome.GRADE_GG == "-") ? false : true;
        }
        protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codigoGrade = 0;

            if (ddlGrade.SelectedValue != "" && ddlGrade.SelectedValue != "0")
            {
                codigoGrade = Convert.ToInt32(ddlGrade.SelectedValue);

                _gradeNome = prodController.ObterGradeNome(codigoGrade);
                if (_gradeNome != null)
                {
                    labGradeEXP.Text = _gradeNome.GRADE_EXP;
                    labGradeXP.Text = _gradeNome.GRADE_XP;
                    labGradePP.Text = _gradeNome.GRADE_PP;
                    labGradeP.Text = _gradeNome.GRADE_P;
                    labGradeM.Text = _gradeNome.GRADE_M;
                    labGradeG.Text = _gradeNome.GRADE_G;
                    labGradeGG.Text = _gradeNome.GRADE_GG;

                    BloquearCamposGrade(_gradeNome);
                    CarregarAmpliacaoMedidas();
                }
            }
        }
        #endregion

        #region "FOTOS"
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
        protected void btFotoPecaIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoPeca, imgFotoPeca, "FOTO_PECA");
            }
            catch (Exception ex)
            {
                labErroFotoPeca.Text = "ERRO: (btFotoPecaIncluir_Click) \n\n" + ex.Message;
            }
        }
        protected void btFotoTecidoDetalheIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecidoDetalhe, imgFotoTecidoDetalhe, "FOTO_TECIDO_DETALHE");
            }
            catch (Exception ex)
            {
                labErroFotoPeca.Text = "ERRO: (btFotoTecidoDetalheIncluir_Click) \n\n" + ex.Message;
            }
        }
        #endregion

        #region "COMPOSICAO"
        private void IncluirComposicao(PROD_HB_COMPOSICAO _composicao)
        {
            prodController.InserirComposicao(_composicao);
        }
        private void ExcluirComposicao(int _Codigo)
        {
            prodController.ExcluirComposicao(_Codigo);
        }
        private void RecarregarComposicao()
        {
            List<PROD_HB_COMPOSICAO> _list = new List<PROD_HB_COMPOSICAO>();
            _list = prodController.ObterComposicaoHB(Convert.ToInt32(hidHB.Value));
            if (_list != null)
            {
                gvComposicao.DataSource = _list;
                gvComposicao.DataBind();
            }
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

                PROD_HB_COMPOSICAO _novo = new PROD_HB_COMPOSICAO();
                _novo.QTDE = Convert.ToDecimal(txtComposicaoQtde.Text);
                _novo.DESCRICAO = txtComposicaoDescricao.Text.Trim().ToUpper();
                _novo.PROD_HB = Convert.ToInt32(hidHB.Value);

                IncluirComposicao(_novo);

                txtComposicaoQtde.Text = "";
                txtComposicaoDescricao.Text = "";

                RecarregarComposicao();
            }
            catch (Exception ex)
            {
                labErroComposicao.Text = "ERRO (btComposicaoIncluir_Click). \n\n" + ex.Message;
            }
        }
        protected void btComposicaoExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    labErroComposicao.Text = "";
                    GridViewRow g = (GridViewRow)b.NamingContainer;
                    if (g != null)
                        if (g.Cells[1].Text != "")
                            hidTotalComp.Value = (Convert.ToDecimal(hidTotalComp.Value) - Convert.ToDecimal(g.Cells[1].Text)).ToString();

                    ExcluirComposicao(Convert.ToInt32(b.CommandArgument));
                    RecarregarComposicao();
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
                    PROD_HB_COMPOSICAO _composicao = e.Row.DataItem as PROD_HB_COMPOSICAO;

                    colunaComposicao += 1;
                    if (_composicao != null)
                    {
                        Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaComposicao.ToString();

                        Button _btExcluir = e.Row.FindControl("btComposicaoExcluir") as Button;
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

        protected void btObterComposicao_Click(object sender, EventArgs e)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            try
            {
                labErroComposicao.Text = "";

                //var materiais = desenvController.ObterMaterial().Where(p =>
                //                                                        p.GRUPO.Trim() == ddlGrupoTecido.SelectedValue.Trim() &&
                //                                                        p.SUBGRUPO.Trim() == ddlSubGrupoTecido.SelectedValue.Trim()).ToList();

                var materiais = desenvController.ObterMaterialCorV2(ddlGrupoTecido.SelectedValue.Trim(), ddlSubGrupoTecido.SelectedValue.Trim(), "")
                    .Where(p => p.COR_MATERIAL.Trim() == ddlCor.SelectedValue.Trim() && p.REFER_FABRICANTE.Trim() == ddlCorFornecedor.SelectedValue.Trim()).ToList();

                if (materiais == null || materiais.Count() <= 0)
                {
                    labErroComposicao.Text = "Nenhum material encontrado. Por favor, adicione a composição.";
                    return;
                }

                var composicaoMaterial = desenvController.ObterMaterialComposicao(materiais[0].MATERIAL.Substring(0, 10).Trim());
                if (composicaoMaterial == null || composicaoMaterial.Count() <= 0)
                {
                    labErroComposicao.Text = "Nenhuma composição encontrada. Por favor, adicione a composição.";
                    return;
                }

                if (gvComposicao.Rows.Count > 0)
                {
                    labErroComposicao.Text = "Para obter a composição do cadastro do tecido, exclua a composição atual.";
                    return;
                }

                PROD_HB_COMPOSICAO composicao = null;
                decimal totalComp = 0;
                foreach (var c in composicaoMaterial)
                {
                    if (c != null)
                    {
                        composicao = new PROD_HB_COMPOSICAO();
                        composicao.PROD_HB = Convert.ToInt32(hidHB.Value);
                        composicao.QTDE = c.QTDE;
                        composicao.DESCRICAO = c.DESCRICAO;

                        IncluirComposicao(composicao);

                        totalComp += c.QTDE;
                    }
                }

                hidTotalComp.Value = totalComp.ToString();

                RecarregarComposicao();
            }
            catch (Exception ex)
            {
                labErroComposicao.Text = ex.Message;
            }
        }
        #endregion

        #region "DETALHE"
        private int IncluirDetalheHB(PROD_HB _detalhe)
        {
            return prodController.InserirDetalheHB(_detalhe);
        }
        private void ExcluirDetalheHB(int _codigo)
        {
            prodController.ExcluirDetalheHB(_codigo);
        }
        protected void ddlDetalhe_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigoDetalhe = ddlDetalhe.SelectedValue;

            if (codigoDetalhe != "0")
            {
                pnlDetalhe.Visible = true;
                //ifrmFotoTecidoDetalhe.Attributes["src"] = "/Silverlight/FotoTecidoDetalhe.aspx";

                if (hidCodigoDetalhe.Value == "")
                    fsDetalheComposicao.Visible = true;
                else
                    fsDetalheComposicao.Visible = false;

                if (codigoDetalhe == "5") //GALÃO PRÓPRIO
                {
                    ddlDetalheGrupoTecido.SelectedValue = ddlGrupoTecido.SelectedValue.ToUpper();
                    CarregarSubGrupo(ddlGrupoTecido.SelectedValue, "D");
                    ddlDetalheSubGrupoTecido.SelectedValue = ddlSubGrupoTecido.SelectedValue.ToUpper();
                    ddlDetalheCor.SelectedValue = ddlCor.SelectedValue;
                    CarregarCorFornecedor(ddlGrupoTecido.SelectedItem.Text.Trim(), ddlSubGrupoTecido.SelectedValue.Trim().ToUpper(), ddlDetalheCorFornecedor);
                    ddlDetalheCorFornecedor.SelectedValue = ddlCorFornecedor.SelectedValue;
                    ddlDetalheFornecedor.SelectedValue = ddlFornecedor.SelectedValue;
                    txtDetalheNumeroPedido.Text = txtPedidoNumero.Text;
                    txtDetalheMolde.Text = txtMolde.Text;
                    txtDetalheLargura.Text = txtLargura.Text;
                    txtDetalheCustoTecido.Text = txtCustoTecido.Text;
                    imgFotoTecidoDetalhe.ImageUrl = imgFotoTecido.ImageUrl;

                    List<ComposicaoGrade> comp = new List<ComposicaoGrade>();
                    foreach (GridViewRow g in gvComposicao.Rows)
                        comp.Add(new ComposicaoGrade { QTDE = Convert.ToDecimal(g.Cells[1].Text), DESCRICAO = g.Cells[2].Text.ToString() });

                    if (comp != null && comp.Count > 0)
                    {
                        gvComposicaoDetalhe.DataSource = comp;
                        gvComposicaoDetalhe.DataBind();
                        Session["COMP_GRADE"] = comp;
                        hidDetalheCompTotal.Value = hidTotalComp.Value;
                    }
                }
                else
                {
                    if (hidCodigoDetalhe.Value == "")
                    {
                        ddlDetalheGrupoTecido.SelectedValue = "Selecione";
                        ddlDetalheSubGrupoTecido.SelectedValue = "Selecione";
                        ddlDetalheCor.SelectedValue = "0";
                        ddlDetalheCorFornecedor.Items.Clear();
                        ddlDetalheCorFornecedor.Items.Add("");
                        ddlDetalheCorFornecedor.SelectedValue = "";
                        txtDetalheNumeroPedido.Text = "";
                        ddlDetalheFornecedor.SelectedValue = "Selecione";
                        txtDetalheMolde.Text = "";
                        txtDetalheLargura.Text = "";
                        txtDetalheCustoTecido.Text = "";
                        txtDetalheGastoPorFolha.Text = "";
                        imgFotoTecidoDetalhe.ImageUrl = "";
                        gvComposicaoDetalhe.DataSource = null;
                        gvComposicaoDetalhe.DataBind();
                        Session["COMP_GRADE"] = null;
                        hidDetalheCompTotal.Value = "0";
                    }
                }

                if (codigoDetalhe == "1" || codigoDetalhe == "6") //COR2 e COR3 -- SELECIONE
                {
                    ddlEtiquetaComposicao.SelectedValue = "";
                    ddlEtiquetaComposicao.Enabled = true;
                }
                else if (codigoDetalhe == "3") //FORRO -- SEMPRE SIM
                {
                    ddlEtiquetaComposicao.SelectedValue = "S";
                    ddlEtiquetaComposicao.Enabled = false;
                }
                else
                {
                    ddlEtiquetaComposicao.SelectedValue = "N";
                    ddlEtiquetaComposicao.Enabled = false;
                }



            }
            else
            {
                pnlDetalhe.Visible = false;
                //ifrmFotoTecidoDetalhe.Attributes["src"] = "";
                Session["COMP_GRADE"] = null;
                hidDetalheCompTotal.Value = "0";
            }

        }
        protected void txtDetalheNumeroPedido_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                GridViewRow row = (GridViewRow)txt.NamingContainer;
                if (row != null)
                {
                    if (txt.Text.Trim() != "")
                    {
                        int codigoHB = 0;
                        codigoHB = Convert.ToInt32(gvDetalhe.DataKeys[row.RowIndex].Value);
                        if (codigoHB > 0)
                        {
                            int numeroPedido = Convert.ToInt32(txt.Text);
                            PROD_HB prod_hb = prodController.ObterHB(codigoHB);
                            prod_hb.NUMERO_PEDIDO = numeroPedido;

                            var pedido = (new DesenvolvimentoController().ObterPedidoNumero(numeroPedido));
                            if (pedido != null)
                                prod_hb.FOTO_TECIDO = (pedido.FOTO_TECIDO == null) ? "" : pedido.FOTO_TECIDO;

                            prodController.AtualizarHB(prod_hb);
                        }
                    }
                }
            }
        }
        private void RecarregarDetalhe()
        {
            List<PROD_HB> _list = new List<PROD_HB>();
            _list = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));
            if (_list != null)
            {
                gvDetalhe.DataSource = _list;
                gvDetalhe.DataBind();
            }
        }
        protected void btDetalheIncluir_Click(object sender, EventArgs e)
        {
            labErroDetalhe.Text = "";
            try
            {
                //Validar se o HB informado já existe nesta coleção
                if (ValidarHB(labErroDetalhe) != "" && hidData.Value.Trim() == "")
                    return;
                else
                    labErroDetalhe.Text = "";

                if (ddlDetalhe.SelectedValue == "" || ddlDetalhe.SelectedValue == "0")
                {
                    labErroDetalhe.Text = "Selecione um Detalhe.";
                    return;
                }
                if (ddlDetalheGrupoTecido.SelectedValue.Trim() == "Selecione")
                {
                    labErroDetalhe.Text = "Selecione o Tecido Grupo para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (ddlDetalheSubGrupoTecido.SelectedValue.Trim() == "Selecione")
                {
                    labErroDetalhe.Text = "Selecione o Tecido SubGrupo para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (ddlDetalheCor.SelectedValue.Trim() == "0")
                {
                    labErroDetalhe.Text = "Selecione a Cor para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (ddlDetalheCorFornecedor.SelectedValue.Trim() == "Selecione")
                {
                    labErroDetalhe.Text = "Selecione a Descrição do Fornecedor para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (ddlDetalheFornecedor.SelectedValue.Trim() == "" || ddlDetalheFornecedor.SelectedValue.Trim() == "Selecione")
                {
                    labErroDetalhe.Text = "Selecione o Fornecedor para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (txtDetalheNumeroPedido.Text.Trim() == "")
                {
                    labErroDetalhe.Text = "Informe o Número do Pedido para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (txtDetalheMolde.Text.Trim() == "")
                {
                    labErroDetalhe.Text = "Informe o Molde para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (txtDetalheLargura.Text.Trim() == "" || Convert.ToDecimal(txtDetalheLargura.Text) <= 0)
                {
                    labErroDetalhe.Text = "Informe a Largura para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }
                if (txtDetalheCustoTecido.Text.Trim() == "" || Convert.ToDecimal(txtDetalheCustoTecido.Text) <= 0)
                {
                    labErroDetalhe.Text = "Informe o Custo do Tecido para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }

                if (ddlEtiquetaComposicao.SelectedValue.Trim() == "")
                {
                    labErroDetalhe.Text = "Selecione se este Tecido irá para a Etiqueta de Composição.";
                    return;
                }

                if (gvComposicaoDetalhe.Rows.Count <= 0 || Convert.ToDecimal(hidDetalheCompTotal.Value) < 100)
                {
                    labErroDetalhe.Text = "Informe a Composição do Tecido para \"" + ddlDetalhe.SelectedItem.Text + "\".";
                    return;
                }

                int codigo = 0;
                PROD_HB _novoDetalhe = new PROD_HB();

                if (hidCodigoDetalhe.Value != "")
                {
                    codigo = Convert.ToInt32(hidCodigoDetalhe.Value);
                    _novoDetalhe = prodController.ObterHB(codigo);
                }

                _novoDetalhe.CODIGO_PAI = Convert.ToInt32(hidHB.Value);
                _novoDetalhe.HB = Convert.ToInt32(txtHB.Text);
                _novoDetalhe.PROD_DETALHE = Convert.ToInt32(ddlDetalhe.SelectedValue);
                _novoDetalhe.COLECAO = ddlColecoes.SelectedValue;
                _novoDetalhe.GRUPO_TECIDO = ddlDetalheGrupoTecido.SelectedItem.Text.ToUpper();
                _novoDetalhe.TECIDO = ddlDetalheSubGrupoTecido.SelectedItem.Text.Trim().ToUpper();
                _novoDetalhe.COR = ddlDetalheCor.SelectedValue;
                _novoDetalhe.COR_FORNECEDOR = ddlDetalheCorFornecedor.SelectedValue.Trim().ToUpper();
                _novoDetalhe.GRUPO = ddlGrupo.SelectedValue;
                _novoDetalhe.NOME = txtNome.Text.Trim().ToUpper();
                _novoDetalhe.DATA_INCLUSAO = DateTime.Now;
                _novoDetalhe.FORNECEDOR = ddlDetalheFornecedor.SelectedValue.ToUpper();
                _novoDetalhe.LARGURA = Convert.ToDecimal(txtDetalheLargura.Text);
                _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(txtDetalheCustoTecido.Text);
                _novoDetalhe.LIQUIDAR = (cbLiquidar.Checked) ? 'S' : 'N';
                _novoDetalhe.LIQUIDAR_PQNO = (cbLiquidarPqno.Checked) ? 'S' : 'N';
                _novoDetalhe.ATACADO = (cbAtacado.Checked) ? 'S' : 'N';
                _novoDetalhe.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
                _novoDetalhe.FOTO_TECIDO = imgFotoTecidoDetalhe.ImageUrl;
                _novoDetalhe.TIPO = (rdbAmpliacao.Checked) ? 'A' : 'R';
                _novoDetalhe.MOSTRUARIO = (cbMostruario.Checked) ? 'S' : 'N';
                _novoDetalhe.OBSERVACAO = txtDetalheObservacao.Text.Trim().ToUpper();
                _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(txtDetalheNumeroPedido.Text.Trim());
                _novoDetalhe.MOLDE = txtDetalheMolde.Text.Trim().ToUpper();
                _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicao.SelectedValue);
                if (txtDetalheGastoPorFolha.Text.Trim() != "")
                    _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtDetalheGastoPorFolha.Text.Trim());
                _novoDetalhe.STATUS = 'P';
                _novoDetalhe.CORTE_SIMULACAO = (cbSimulacaoCorte.Checked) ? 'S' : 'N';

                //validar peso do tecido para calcular estoque
                var rendimento = prodController.ObterRendimentoTecidoKG(_novoDetalhe.NUMERO_PEDIDO.ToString());
                if (rendimento != null)
                    _novoDetalhe.RENDIMENTO_1MT = rendimento.RENDIMENTO_1MT;


                //Atualizacao
                if (hidCodigoDetalhe.Value != "")
                    prodController.AtualizarHB(_novoDetalhe);
                else //Inclusao
                {
                    codigo = IncluirDetalheHB(_novoDetalhe);

                    PROD_HB_COMPOSICAO _novo = null;
                    foreach (GridViewRow r in gvComposicaoDetalhe.Rows)
                    {
                        _novo = new PROD_HB_COMPOSICAO();
                        _novo.PROD_HB = codigo;
                        _novo.QTDE = Convert.ToDecimal(r.Cells[1].Text);
                        _novo.DESCRICAO = r.Cells[2].Text.Trim().ToUpper();
                        IncluirComposicao(_novo);
                    }
                }

                LimparCamposDetalhe();

                RecarregarDetalhe();
                pnlDetalhe.Visible = false;
            }
            catch (Exception ex)
            {
                labErroDetalhe.Text = "ERRO (btDetalheIncluir_Click). \n\n" + ex.Message;
            }
        }
        protected void btDetalheAlterar_Click(object sender, EventArgs e)
        {
            Button _btDetalheAlterar = (Button)sender;
            if (_btDetalheAlterar != null)
            {
                GridViewRow row = (GridViewRow)_btDetalheAlterar.NamingContainer;
                if (row != null)
                {
                    string codigoDetalhe = gvDetalhe.DataKeys[row.RowIndex].Value.ToString();

                    PROD_HB prod_hb = prodController.ObterHB(Convert.ToInt32(codigoDetalhe));
                    if (prod_hb != null)
                    {
                        btDetalheIncluir.Text = "Alterar Detalhe";

                        ddlDetalhe.SelectedValue = prod_hb.PROD_DETALHE.ToString();
                        ddlDetalhe_SelectedIndexChanged(ddlDetalhe, null);

                        ddlDetalheGrupoTecido.SelectedValue = prod_hb.GRUPO_TECIDO;
                        CarregarSubGrupo(prod_hb.GRUPO_TECIDO, "D");
                        ddlDetalheSubGrupoTecido.SelectedValue = prod_hb.TECIDO;
                        CarregarCorFornecedor(ddlDetalheGrupoTecido.SelectedItem.Text.Trim(), ddlDetalheSubGrupoTecido.SelectedItem.Text.Trim(), ddlDetalheCorFornecedor);
                        ddlDetalheFornecedor.SelectedValue = prod_hb.FORNECEDOR;
                        ddlDetalheCor.SelectedValue = prod_hb.COR;
                        ddlDetalheCorFornecedor.SelectedValue = prod_hb.COR_FORNECEDOR;
                        txtDetalheNumeroPedido.Text = prod_hb.NUMERO_PEDIDO.ToString();
                        txtDetalheMolde.Text = prod_hb.MOLDE.ToString();
                        txtDetalheLargura.Text = prod_hb.LARGURA.ToString();
                        txtDetalheCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                        imgFotoTecidoDetalhe.ImageUrl = prod_hb.FOTO_TECIDO;
                        txtDetalheObservacao.Text = prod_hb.OBSERVACAO;
                        if (prod_hb.GASTO_FOLHA_SIMULACAO != null)
                            txtDetalheGastoPorFolha.Text = prod_hb.GASTO_FOLHA_SIMULACAO.ToString();

                        if (prod_hb.ETIQUETA_COMPOSICAO != null)
                            ddlEtiquetaComposicao.SelectedValue = prod_hb.ETIQUETA_COMPOSICAO.ToString();

                        //Controle temporario para nao alterar a composicao do detalhe
                        fsDetalheComposicao.Visible = false;
                        RecarregarComp(null, gvComposicaoDetalhe, Convert.ToInt32(codigoDetalhe), hidDetalheCompTotal, null);

                        hidCodigoDetalhe.Value = codigoDetalhe;
                    }
                }
            }
        }
        protected void btDetalheExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    ExcluirDetalheHB(Convert.ToInt32(b.CommandArgument));
                    RecarregarDetalhe();
                    LimparCamposDetalhe();
                    pnlDetalhe.Visible = false;
                }
                catch (Exception ex)
                {
                    labErroDetalhe.Text = "ERRO (btDetalheExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void btDetalheLimpar_Click(object sender, EventArgs e)
        {
            LimparCamposDetalhe();

            var _compDetalhe = prodController.ObterComposicao(0);
            gvComposicaoDetalhe.DataSource = _compDetalhe;
            gvComposicaoDetalhe.DataBind();

            pnlDetalhe.Visible = false;
        }
        protected void gvDetalhe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _hb = e.Row.DataItem as PROD_HB;

                    colunaDetalhe += 1;
                    if (_hb != null)
                    {
                        Literal _col = e.Row.FindControl("litDetalheColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaDetalhe.ToString();

                        Literal _HB = e.Row.FindControl("litHB") as Literal;
                        if (_HB != null)
                            _HB.Text = _hb.HB.ToString();

                        Literal _DetalheDesc = e.Row.FindControl("litDetalheDesc") as Literal;
                        if (_DetalheDesc != null)
                            _DetalheDesc.Text = _hb.PROD_DETALHE1.DESCRICAO;

                        TextBox _txtDetalheNumeroPedido = e.Row.FindControl("txtDetalheNumeroPedido") as TextBox;
                        if (_txtDetalheNumeroPedido != null)
                            _txtDetalheNumeroPedido.Text = (_hb.NUMERO_PEDIDO == null) ? "" : _hb.NUMERO_PEDIDO.ToString();

                        Button _btExcluir = e.Row.FindControl("btDetalheExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _hb.CODIGO.ToString();
                    }
                }
            }
        }
        private void LimparCamposDetalhe()
        {
            hidCodigoDetalhe.Value = "";
            ddlDetalhe.SelectedValue = "0";
            txtDetalheNumeroPedido.Text = "";
            ddlDetalheGrupoTecido.SelectedValue = "Selecione";
            ddlDetalheSubGrupoTecido.SelectedValue = "Selecione";
            ddlDetalheCor.SelectedValue = "0";
            ddlDetalheCorFornecedor.SelectedValue = "Selecione";
            ddlDetalheFornecedor.SelectedValue = "Selecione";
            txtDetalheMolde.Text = "";
            txtDetalheLargura.Text = "";
            txtDetalheCustoTecido.Text = "";
            txtDetalheObservacao.Text = "";
            txtDetalheQtdeComposicao.Text = "";
            txtDetalheDescricaoComposicao.Text = "";
            txtDetalheGastoPorFolha.Text = "";
            imgFotoTecidoDetalhe.ImageUrl = "";
            ddlEtiquetaComposicao.SelectedValue = "";
            Session["COMP_GRADE"] = null;
            hidDetalheCompTotal.Value = "0";

            btDetalheIncluir.Text = "Incluir Detalhe";
        }
        private void RecarregarComposicaoDetalhe(List<ComposicaoGrade> _list)
        {
            if (_list != null)
            {
                gvComposicaoDetalhe.DataSource = _list;
                gvComposicaoDetalhe.DataBind();
            }
        }
        private void RecarregarComp(List<PROD_HB_COMPOSICAO> _list, GridView comp, int prod_hb, HiddenField hidCompTotal, List<ComposicaoGrade> _listSession)
        {
            if (_listSession != null)
            {
                comp.DataSource = _listSession;
                comp.DataBind();
            }
            else
            {
                if (_list == null)
                {
                    _list = new List<PROD_HB_COMPOSICAO>();
                    _list = prodController.ObterComposicaoHB(prod_hb);
                }
                comp.DataSource = _list;
                comp.DataBind();
                decimal total = 0;
                foreach (PROD_HB_COMPOSICAO c in _list)
                    total = total + c.QTDE;
                hidCompTotal.Value = Convert.ToDecimal(total).ToString();
            }
        }
        protected void btDetalheComposicaoIncluir_Click(object sender, EventArgs e)
        {
            labErroDetalheComposicao.Text = "";
            try
            {
                if (txtDetalheQtdeComposicao.Text.Trim() == "" || Convert.ToDecimal(txtDetalheQtdeComposicao.Text) <= 0)
                {
                    labErroDetalheComposicao.Text = "Informe a Quantidade da composição.";
                    return;
                }
                if (txtDetalheDescricaoComposicao.Text.Trim() == "")
                {
                    labErroDetalheComposicao.Text = "Informe a Descrição da composição.";
                    return;
                }

                if (hidDetalheCompTotal.Value == "" || Convert.ToDecimal(hidDetalheCompTotal.Value) == 0)
                {
                    if (Convert.ToDecimal(txtDetalheQtdeComposicao.Text.Trim()) > 100)
                    {
                        labErroDetalheComposicao.Text = "Quantidade da Composição não pode ser maior que 100%.";
                        return;
                    }
                    else
                    {
                        hidDetalheCompTotal.Value = txtDetalheQtdeComposicao.Text.Trim();
                    }
                }
                else
                {
                    if (Convert.ToDecimal(hidDetalheCompTotal.Value) + Convert.ToDecimal(txtDetalheQtdeComposicao.Text.Trim()) > 100)
                    {
                        labErroDetalheComposicao.Text = "Quantidade da Composição não pode ser maior que 100%. Ainda é permitido o valor de " + ((Convert.ToDecimal(100.00)) - Convert.ToDecimal(hidDetalheCompTotal.Value)).ToString() + "%.";
                        return;
                    }
                    else
                    {
                        hidDetalheCompTotal.Value = (Convert.ToDecimal(hidDetalheCompTotal.Value) + Convert.ToDecimal(txtDetalheQtdeComposicao.Text.Trim())).ToString();
                    }
                }

                ComposicaoGrade _compGrade = new ComposicaoGrade();
                //PROD_HB_COMPOSICAO _novo = new PROD_HB_COMPOSICAO();
                _compGrade.QTDE = Convert.ToDecimal(txtDetalheQtdeComposicao.Text);
                _compGrade.DESCRICAO = txtDetalheDescricaoComposicao.Text.Trim().ToUpper();

                List<ComposicaoGrade> comp = new List<ComposicaoGrade>();
                if (Session["COMP_GRADE"] != null)
                    comp = (List<ComposicaoGrade>)Session["COMP_GRADE"];

                comp.Add(_compGrade);
                Session["COMP_GRADE"] = comp;

                txtDetalheQtdeComposicao.Text = "";
                txtDetalheDescricaoComposicao.Text = "";

                RecarregarComposicaoDetalhe(comp);
            }
            catch (Exception ex)
            {
                labErroDetalheComposicao.Text = "ERRO (btDetalheComposicaoIncluir_Click). \n\n" + ex.Message;
            }
        }
        protected void btDetalheComposicaoExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            List<ComposicaoGrade> comp = null;
            if (b != null)
            {
                try
                {
                    labErroDetalheComposicao.Text = "";
                    GridViewRow g = (GridViewRow)b.NamingContainer;
                    if (g != null)
                        if (g.Cells[1].Text != "")
                        {
                            hidDetalheCompTotal.Value = (Convert.ToDecimal(hidDetalheCompTotal.Value) - Convert.ToDecimal(g.Cells[1].Text)).ToString();

                            comp = new List<ComposicaoGrade>();
                            if (Session["COMP_GRADE"] != null)
                                comp = (List<ComposicaoGrade>)Session["COMP_GRADE"];

                            int _index = comp.FindIndex(p => p.QTDE == Convert.ToDecimal(g.Cells[1].Text) && p.DESCRICAO == g.Cells[2].Text.Trim().ToUpper());

                            comp.RemoveAt(_index);

                            Session["COMP_GRADE"] = comp;
                        }

                    RecarregarComposicaoDetalhe(comp);
                }
                catch (Exception ex)
                {
                    labErroDetalheComposicao.Text = "ERRO (btDetalheComposicaoExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void gvComposicaoDetalhe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ComposicaoGrade _composicao = e.Row.DataItem as ComposicaoGrade;

                    colunaComposicaoDet += 1;
                    if (_composicao != null)
                    {
                        Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaComposicaoDet.ToString();
                    }
                }
            }
        }
        protected void gvComposicaoDetalhe_DataBound(object sender, EventArgs e)
        {
            GridViewRow foo = gvComposicaoDetalhe.FooterRow;
            if (foo != null)
            {
                decimal total = 0;
                foreach (GridViewRow r in gvComposicaoDetalhe.Rows)
                    total = total + Convert.ToDecimal(r.Cells[1].Text);

                foo.Cells[1].Text = total.ToString();
                foo.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void btDetalheObterComposicao_Click(object sender, EventArgs e)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            try
            {
                labErroDetalheComposicao.Text = "";

                //var materiais = desenvController.ObterMaterial().Where(p =>
                //                                                        p.GRUPO.Trim() == ddlDetalheGrupoTecido.SelectedValue.Trim() &&
                //                                                        p.SUBGRUPO.Trim() == ddlDetalheSubGrupoTecido.SelectedValue.Trim()).ToList();

                var materiais = desenvController.ObterMaterialCorV2(ddlDetalheGrupoTecido.SelectedValue.Trim(), ddlDetalheSubGrupoTecido.SelectedValue.Trim(), "")
                     .Where(p => p.COR_MATERIAL.Trim() == ddlDetalheCor.SelectedValue.Trim() && p.REFER_FABRICANTE.Trim() == ddlDetalheCorFornecedor.SelectedValue.Trim()).ToList();

                if (materiais == null || materiais.Count() <= 0)
                {
                    labErroDetalheComposicao.Text = "Nenhum material encontrado. Por favor, adicione a composição.";
                    return;
                }

                var composicaoMaterial = desenvController.ObterMaterialComposicao(materiais[0].MATERIAL.Substring(0, 10));
                if (composicaoMaterial == null || composicaoMaterial.Count() <= 0)
                {
                    labErroDetalheComposicao.Text = "Nenhuma composição encontrada. Por favor, adicione a composição.";
                    return;
                }

                if (gvComposicaoDetalhe.Rows.Count > 0)
                {
                    labErroDetalheComposicao.Text = "Para obter a composição do cadastro do tecido, exclua a composição atual.";
                    return;
                }

                List<ComposicaoGrade> comp = new List<ComposicaoGrade>();
                if (Session["COMP_GRADE"] != null)
                    comp = (List<ComposicaoGrade>)Session["COMP_GRADE"];

                ComposicaoGrade composicao = null;
                decimal totalComp = 0;
                foreach (var c in composicaoMaterial)
                {
                    if (c != null)
                    {
                        composicao = new ComposicaoGrade();
                        composicao.QTDE = c.QTDE;
                        composicao.DESCRICAO = c.DESCRICAO;

                        comp.Add(composicao);
                        Session["COMP_GRADE"] = comp;

                        totalComp += c.QTDE;
                    }
                }

                hidDetalheCompTotal.Value = totalComp.ToString();

                RecarregarComposicaoDetalhe(comp);
            }
            catch (Exception ex)
            {
                labErroDetalheComposicao.Text = ex.Message;
            }
        }

        #endregion

        #region "AVIAMENTOS"
        protected void btAviamentoAtualizar_Click(object sender, EventArgs e)
        {
            CarregarAviamentos();
        }
        private void IncluirAviamento(PROD_HB_PROD_AVIAMENTO _aviamento)
        {
            prodController.InserirAviamentoHB(_aviamento);
        }
        private void ExcluirAviamento(int _Codigo)
        {
            prodController.ExcluirAviamentoHB(_Codigo);
        }
        private void RecarregarAviamento()
        {
            List<PROD_HB_PROD_AVIAMENTO> _list = new List<PROD_HB_PROD_AVIAMENTO>();
            _list = prodController.ObterAviamentoHB(Convert.ToInt32(hidHB.Value));
            if (_list != null)
            {
                gvAviamento.DataSource = _list;
                gvAviamento.DataBind();
            }
        }
        /*private void RecarregarAviamentoDropDownList()
        {
            string CodigoAviamento = "";
            try
            {
                //Obtem do id do Cliente
                CodigoAviamento = Request.Form[ddlAviamento.UniqueID];

                //Carrega novamento no lado server
                ddlAviamento.Items.Clear();
                CarregarDropDownList(CarregarAviamentos(), ddlAviamento);

                //Set valor no client no server
                if (CodigoAviamento != null)
                    ddlAviamento.Items.FindByValue(CodigoAviamento).Selected = true;
            }
            catch (Exception)
            {
                throw;
            }
        }*/
        protected void txtQtde_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                GridViewRow row = (GridViewRow)txt.NamingContainer;
                if (row != null)
                {
                    if (txt.Text.Trim() != "")
                    {
                        int codigoAviamento = 0;
                        codigoAviamento = Convert.ToInt32(gvAviamento.DataKeys[row.RowIndex].Value);
                        if (codigoAviamento > 0)
                        {
                            decimal qtde = Convert.ToDecimal(txt.Text);
                            PROD_HB_PROD_AVIAMENTO aviamento = prodController.ObterAviamentoHBCodigo(codigoAviamento);
                            aviamento.QTDE = qtde;
                            prodController.AtualizarAviamentoHB(aviamento);
                        }
                    }
                }
            }
        }
        protected void btAviamentoIncluir_Click(object sender, EventArgs e)
        {
            try
            {

                labErroAviamento.Text = "";
                /*if (ddlAviamento.SelectedValue == "" || ddlAviamento.SelectedValue == "0")
                {
                    labErroAviamento.Text = "Selecione um Aviamento.";
                    return;
                }*/
                if (ddlGrupoAviamento.SelectedValue.Trim() == "")
                {
                    labErroAviamento.Text = "Informe o Grupo do Aviamento.";
                    return;
                }

                if (ddlSubGrupoAviamento.SelectedValue.Trim() == "")
                {
                    labErroAviamento.Text = "Informe o SubGrupo do Aviamento.";
                    return;
                }
                if (ddlCorAviamento.SelectedValue.Trim() == "")
                {
                    labErroAviamento.Text = "Informe a Cor do Aviamento.";
                    return;
                }
                if (ddlCorFornecedorAviamento.SelectedValue.Trim() == "Selecione")
                {
                    labErroAviamento.Text = "Informe a Cor do Fornecedor do Aviamento.";
                    return;
                }
                if (txtAviamentoQtde.Text.Trim() == "" || Convert.ToDecimal(txtAviamentoQtde.Text.Trim()) <= 0)
                {
                    labErroAviamento.Text = "Informe a Qtde Total do Aviamento.";
                    return;
                }
                if (txtAviamentoQtdePorPeca.Text.Trim() == "" || Convert.ToDecimal(txtAviamentoQtdePorPeca.Text.Trim()) <= 0)
                {
                    labErroAviamento.Text = "Informe a Qtde por Peça do Aviamento.";
                    return;
                }
                if (txtAviamentoDescricao.Text.Trim() == "")
                {
                    labErroAviamento.Text = "Informe a Descrição do Aviamento.";
                    return;
                }

                if (ddlEtiquetaComposicaoAviamento.SelectedValue == "")
                {
                    labErroAviamento.Text = "Selecione se este Aviamento irá para a Etiqueta de Composição.";
                    return;
                }

                if (txtGradeTotal.Text == "" || Convert.ToInt32(txtGradeTotal.Text) <= 0)
                {
                    labErroAviamento.Text = "Informa a grade da Ordem de Corte.";
                    return;
                }

                PROD_HB_PROD_AVIAMENTO _novo = new PROD_HB_PROD_AVIAMENTO();

                _novo.GRUPO = ddlGrupoAviamento.SelectedItem.Text.Trim();
                _novo.SUBGRUPO = ddlSubGrupoAviamento.SelectedItem.Text.Trim();
                _novo.COR = ddlCorAviamento.SelectedValue.Trim();
                _novo.COR_FORNECEDOR = ddlCorFornecedorAviamento.SelectedValue.Trim();
                _novo.PROD_AVIAMENTO = null; // Convert.ToInt32(ddlAviamento.SelectedValue);
                _novo.QTDE = Convert.ToDecimal(txtAviamentoQtde.Text);
                _novo.QTDE_POR_PECA = Convert.ToDecimal(txtAviamentoQtdePorPeca.Text);
                _novo.DESCRICAO = txtAviamentoDescricao.Text.Trim().ToUpper();
                _novo.PROD_HB = Convert.ToInt32(hidHB.Value);
                _novo.DATA_INCLUSAO = DateTime.Now.Date;
                _novo.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicaoAviamento.SelectedValue);
                _novo.COMPRA_EXTRA = 'N';

                IncluirAviamento(_novo);

                ddlGrupoAviamento.SelectedValue = "";
                ddlSubGrupoAviamento.SelectedValue = "";
                ddlCorAviamento.SelectedValue = "";
                ddlCorFornecedorAviamento.SelectedValue = "Selecione";
                txtAviamentoQtde.Text = "";
                txtAviamentoQtdePorPeca.Text = "";
                ddlEtiquetaComposicaoAviamento.SelectedValue = "";
                //txtAviamentoDescricao.Text = "";

                RecarregarAviamento();
            }
            catch (Exception ex)
            {
                labErroAviamento.Text = "ERRO (btAviamentoIncluir_Click). \n\n" + ex.Message;
            }
        }
        protected void btAviamentoExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    ExcluirAviamento(Convert.ToInt32(b.CommandArgument));
                    RecarregarAviamento();
                }
                catch (Exception ex)
                {
                    labErroAviamento.Text = "ERRO (btAviamentoExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_PROD_AVIAMENTO _aviamento = e.Row.DataItem as PROD_HB_PROD_AVIAMENTO;

                    colunaAviamento += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litAviamentoColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaAviamento.ToString();

                        Literal _litGrupo = e.Row.FindControl("litGrupo") as Literal;
                        if (_litGrupo != null)
                            _litGrupo.Text = (_aviamento.PROD_AVIAMENTO1 == null) ? _aviamento.GRUPO : _aviamento.PROD_AVIAMENTO1.DESCRICAO;

                        Literal _litSubGrupo = e.Row.FindControl("litSubGrupo") as Literal;
                        if (_litSubGrupo != null)
                            _litSubGrupo.Text = (_aviamento.PROD_AVIAMENTO1 == null) ? _aviamento.SUBGRUPO : _aviamento.PROD_AVIAMENTO1.DESCRICAO;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            if (_aviamento.COR != null)
                            {
                                _litCor.Text = prodController.ObterCoresBasicas(_aviamento.COR).DESC_COR;
                            }

                        TextBox _txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                        if (_txtQtde != null)
                            _txtQtde.Text = _aviamento.QTDE.ToString().Replace(",000", "");

                        Label _labQtdePorPeca = e.Row.FindControl("labQtdePorPeca") as Label;
                        if (_labQtdePorPeca != null)
                            _labQtdePorPeca.Text = _aviamento.QTDE_POR_PECA.ToString().Replace(",000", "");

                        /*Literal _litMedida = e.Row.FindControl("litMedida") as Literal;
                        if (_litMedida != null)
                            _litMedida.Text = _aviamento.PROD_AVIAMENTO1.UNIDADE_MEDIDA1.DESCRICAO;*/

                        Button _btExcluir = e.Row.FindControl("btAviamentoExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _aviamento.CODIGO.ToString();
                        }
                    }
                }
            }
        }
        #endregion

        #region "HB"
        protected void cbLiquidarPqno_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLiquidarPqno.Checked)
            {
                cbLiquidar.Enabled = false;
                cbLiquidar.Checked = false;
            }
            else
            {
                cbLiquidar.Enabled = true;
            }
        }
        protected void cbLiquidar_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLiquidar.Checked)
            {
                cbLiquidarPqno.Enabled = false;
                cbLiquidarPqno.Checked = false;
            }
            else
            { cbLiquidarPqno.Enabled = true; }
        }
        private void ObterUltimoHB()
        {
            string _colecao = ddlColecoes.SelectedValue;
            char cMostruario;
            int HB = 0;
            txtHB.Text = "";
            if (_colecao != "" && _colecao != "0")
            {
                cMostruario = (cbMostruario.Checked) ? 'S' : 'N';
                HB = prodController.ObterUltimoHB(_colecao, cMostruario);
                txtHB.Text = HB.ToString();
            }
        }
        private void CriarHB()
        {
            PROD_HB _prod = new PROD_HB();
            _prod.DATA_INCLUSAO = DateTime.Now;
            _prod.STATUS = 'X';
            hidHB.Value = prodController.CriarHB(_prod).ToString();
        }
        private void AtualizarHB(PROD_HB _hb)
        {
            prodController.AtualizarHB(_hb);
        }
        private void IncluirGrade(PROD_HB_GRADE _grade)
        {
            prodController.InserirGrade(_grade);
        }
        private void IncluirProcesso(PROD_HB_PROD_PROCESSO _processo)
        {
            prodController.InserirProcesso(_processo);
        }
        private void AtualizarProcessoDataFim(int _prod_hb, int _processo, DateTime _dataBaixa)
        {
            prodController.AtualizarProcessoDataFim(_prod_hb, _processo, _dataBaixa);
        }
        private int ObterProcessoOrdem(int _ordem)
        {
            return prodController.ObterProcessoOrdem(_ordem).CODIGO;
        }
        protected void txtHB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHB.Text != "")
                {
                    string msg = ValidarHB(labErroEnvio);

                    if (msg != "")
                    {
                        txtHB.Text = "";
                        labHB.ForeColor = System.Drawing.Color.Red;
                        labHB.ToolTip = msg;
                        txtHB.Focus();
                    }
                    else
                    {
                        labHB.ForeColor = System.Drawing.Color.Gray;
                        labHB.ToolTip = "";
                        labErroEnvio.Text = "";
                        ddlGrupo.Focus();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void txtCodigoLinx_TextChanged(object sender, EventArgs e)
        {
            string codigoProduto = "";
            try
            {
                //Validação se CODIGO DO LINX ESTÁ OK
                if (!ValidarCodigoLinx())
                {
                    labCodigoLinx.ForeColor = System.Drawing.Color.Red;
                    labCodigoLinx.ToolTip = "Código do produto no LINX não encontrado.";
                    labErroEnvio.Text = "Código do produto no LINX incorreto.";
                    txtCodigoLinx.Text = "";
                    txtCodigoLinx.Focus();
                    return;
                }
                else
                {
                    if (txtCodigoLinx.Text.Trim() != "")
                    {
                        codigoProduto = txtCodigoLinx.Text.Trim();
                        CarregarCores(codigoProduto);

                        PRODUTO _produto = new PRODUTO();
                        _produto = baseController.BuscaProduto(codigoProduto);
                        if (_produto != null)
                        {
                            txtNome.Text = _produto.DESC_PRODUTO.Trim();
                            ddlGrupo.SelectedValue = _produto.GRUPO_PRODUTO;

                            var m = prodController.ObterHB(codigoProduto);
                            if (m != null && m.Count() > 0)
                            {
                                txtMolde.Text = (m[0].MOLDE == null) ? "" : m[0].MOLDE;
                                txtModelagem.Text = (m[0].MODELAGEM == null) ? "" : m[0].MODELAGEM;

                                var most = m.Where(p => p.MOSTRUARIO == 'S' && p.PRECO_FACC_MOSTRUARIO > 0).FirstOrDefault();
                                if (most != null)
                                {
                                    txtPrecoFaccMostruario.Text = most.PRECO_FACC_MOSTRUARIO.ToString();
                                }
                            }

                            CarregarFoto(codigoProduto, ddlCor.SelectedValue.Trim());

                            CarregarOrigemProduto(ddlColecoes.SelectedValue.Trim(), codigoProduto);
                        }

                        labErroEnvio.Text = "";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObterUltimoHB();
            CarregarOrigem(ddlColecoes.SelectedValue.Trim());
            CarregarGrades(ddlColecoes.SelectedValue.Trim());
        }
        protected void txtNumeroPedido_TextChanged(object sender, EventArgs e)
        {

            try
            {
                TextBox _txtNumeroPedido = (TextBox)sender;
                if (_txtNumeroPedido != null && _txtNumeroPedido.Text != "" && Convert.ToInt32(_txtNumeroPedido.Text) >= 1000)
                {
                    int numeroPedido = Convert.ToInt32(_txtNumeroPedido.Text);
                    var pedido = desenvController.ObterPedidoNumero(numeroPedido);
                    if (pedido != null)
                    {
                        if (_txtNumeroPedido.ID == "txtDetalheNumeroPedido")
                        {

                            txtDetalheCustoTecido.Text = "";
                            ddlDetalheGrupoTecido.SelectedValue = "Selecione";
                            ddlDetalheSubGrupoTecido.SelectedValue = "Selecione";
                            ddlDetalheCor.SelectedValue = "0";
                            if (ddlDetalheCorFornecedor.SelectedValue.Trim() != "")
                                ddlDetalheCorFornecedor.SelectedValue = "Selecione";
                            ddlDetalheFornecedor.SelectedValue = "Selecione";

                            // obter custo do subpedido
                            var subPedido = desenvController.ObterPedidoSubPorDesenvPedido(pedido.CODIGO)
                                    .OrderByDescending(p => p.DATA_PEDIDO)
                                    .FirstOrDefault();
                            if (subPedido != null)
                            {
                                txtDetalheCustoTecido.Text = subPedido.VALOR.ToString();
                            }
                            else
                            {
                                txtDetalheCustoTecido.Text = pedido.VALOR.ToString();
                            }

                            ddlDetalheGrupoTecido.SelectedValue = desenvController.ObterMaterialGrupo(pedido.GRUPO).GRUPO;
                            CarregarSubGrupo(pedido.GRUPO, "D");
                            ddlDetalheSubGrupoTecido.SelectedValue = pedido.SUBGRUPO;
                            ddlDetalheCor.SelectedValue = prodController.ObterCoresBasicas(pedido.COR).COR;
                            CarregarCorFornecedor(pedido.GRUPO, pedido.SUBGRUPO, ddlDetalheCorFornecedor);
                            ddlDetalheCorFornecedor.SelectedValue = pedido.COR_FORNECEDOR;
                            ddlDetalheFornecedor.SelectedValue = prodController.ObterFornecedor(pedido.FORNECEDOR, 'T').FORNECEDOR;// pedido.FORNECEDOR;
                        }
                        else
                        {
                            txtCustoTecido.Text = "";
                            ddlGrupoTecido.SelectedValue = "Selecione";
                            ddlSubGrupoTecido.SelectedValue = "Selecione";
                            ddlCor.SelectedValue = "0";
                            if (ddlCorFornecedor.SelectedValue.Trim() != "")
                                ddlCorFornecedor.SelectedValue = "Selecione";
                            ddlFornecedor.SelectedValue = "Selecione";

                            // obter custo do subpedido
                            var subPedido = desenvController.ObterPedidoSubPorDesenvPedido(pedido.CODIGO)
                                    .OrderByDescending(p => p.DATA_PEDIDO)
                                    .FirstOrDefault();
                            if (subPedido != null)
                            {
                                txtCustoTecido.Text = subPedido.VALOR.ToString();
                            }
                            else
                            {
                                txtCustoTecido.Text = pedido.VALOR.ToString();
                            }

                            ddlGrupoTecido.SelectedValue = desenvController.ObterMaterialGrupo(pedido.GRUPO).GRUPO;
                            CarregarSubGrupo(pedido.GRUPO, "T");
                            ddlSubGrupoTecido.SelectedValue = pedido.SUBGRUPO;
                            ddlCor.SelectedValue = prodController.ObterCoresBasicas(pedido.COR).COR;
                            CarregarCorFornecedor(pedido.GRUPO, pedido.SUBGRUPO, ddlCorFornecedor);
                            ddlCorFornecedor.SelectedValue = pedido.COR_FORNECEDOR;
                            ddlFornecedor.SelectedValue = prodController.ObterFornecedor(pedido.FORNECEDOR, 'T').FORNECEDOR;// pedido.FORNECEDOR;

                            CarregarFoto(txtCodigoLinx.Text.Trim(), ddlCor.SelectedValue.Trim());
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        protected void cbMostruario_CheckedChanged(object sender, EventArgs e)
        {
            ObterUltimoHB();
        }
        protected void rdbAmpliacao_CheckedChanged(object sender, EventArgs e)
        {
            ObterUltimoHB();

            if (rdbAmpliacao.Checked)
            {
                if (hidTela.Value != "2")
                {
                    fsAmpliacao.Visible = true;
                    pnlRisco.Visible = false;
                }
            }
        }
        protected void rdbRisco_CheckedChanged(object sender, EventArgs e)
        {
            ObterUltimoHB();

            if (rdbRisco.Checked)
            {
                if (hidTela.Value != "2")
                {
                    fsAmpliacao.Visible = false;
                    pnlRisco.Visible = true;
                }
            }
        }
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            bool bMostruario = false;

            try
            {
                //Validação de nulos
                if (!ValidarCampos())
                {
                    labErroEnvio.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                //Validação se PEDIDO EXISTE
                if (!ValidarPedido())
                {
                    labErroEnvio.Text = "Número do Pedido não existe.";
                    return;
                }

                //Validação se CODIGO DO LINX ESTÁ OK
                if (!ValidarCodigoLinx())
                {
                    labErroEnvio.Text = "Código do produto no LINX incorreto.";
                    return;
                }

                //Validar se o HB informado já existe nesta coleção
                if (ValidarHB(labErroEnvio) != "" && hidData.Value.Trim() == "")
                    return;

                if (ddlDetalhe.SelectedValue != "" && ddlDetalhe.SelectedValue != "0")
                {
                    labErroEnvio.Text = "Existe Detalhe selecionado que não foi inserido, por favor, VERIFICAR.";
                    return;
                }

                labErroEnvio.Text = "<strong>Aguarde enquanto o relatório está sendo gerado...</strong>";

                //Se for tela 3 e data é preenchimento de risco vindo da ampliação
                DateTime dataBaixaAmpliacao = DateTime.Now;
                if (hidData.Value.Trim() != "")
                {
                    string horaAtual = DateTime.Now.ToString("HH:mm:ss");
                    string dataBaixa = Convert.ToDateTime(hidData.Value).ToString("yyyy-MM-dd ");
                    dataBaixaAmpliacao = Convert.ToDateTime(dataBaixa + horaAtual);
                    AtualizarProcessoDataFim(Convert.ToInt32(hidHB.Value), 1, dataBaixaAmpliacao);
                }

                //Inserir no processo do PRINCIPAL
                var processoExiste = prodController.ObterProcessoHB(Convert.ToInt32(hidHB.Value), (rdbAmpliacao.Checked) ? 1 : 2);
                if (processoExiste == null)
                {
                    PROD_HB_PROD_PROCESSO _processo = null;
                    _processo = new PROD_HB_PROD_PROCESSO();
                    _processo.PROD_HB = Convert.ToInt32(hidHB.Value);
                    _processo.PROD_PROCESSO = ObterProcessoOrdem((rdbAmpliacao.Checked) ? 1 : 2); //se tiver amplicação, inserir processo de ampliação, senão, iniciar no processo do risco
                    _processo.DATA_INICIO = dataBaixaAmpliacao;
                    IncluirProcesso(_processo);
                }

                //Atualizar HB
                PROD_HB _prod_hb = new PROD_HB();
                _prod_hb.CODIGO = Convert.ToInt32(hidHB.Value);
                _prod_hb.HB = Convert.ToInt32(txtHB.Text);
                _prod_hb.COLECAO = ddlColecoes.SelectedValue;
                _prod_hb.GRUPO = ddlGrupo.SelectedValue;
                _prod_hb.NOME = txtNome.Text.Trim().ToUpper();
                _prod_hb.GRUPO_TECIDO = ddlGrupoTecido.SelectedItem.Text.ToUpper();
                _prod_hb.TECIDO = ddlSubGrupoTecido.SelectedItem.Text.Trim().ToUpper();
                _prod_hb.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
                _prod_hb.COR = ddlCor.SelectedValue;
                _prod_hb.COR_FORNECEDOR = ddlCorFornecedor.SelectedValue.Trim().ToUpper();
                _prod_hb.FORNECEDOR = ddlFornecedor.SelectedValue;
                _prod_hb.LARGURA = Convert.ToDecimal(txtLargura.Text);
                _prod_hb.LIQUIDAR = (cbLiquidar.Checked) ? 'S' : 'N';
                _prod_hb.LIQUIDAR_PQNO = (cbLiquidarPqno.Checked) ? 'S' : 'N';
                _prod_hb.ATACADO = (cbAtacado.Checked) ? 'S' : 'N';
                _prod_hb.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
                _prod_hb.CUSTO_TECIDO = Convert.ToDecimal(txtCustoTecido.Text);
                if (txtOutro.Text.Trim() != "")
                    _prod_hb.AMPLIACAO_OUTRO = txtOutro.Text.Trim();
                _prod_hb.PROD_GRADE = Convert.ToInt32(ddlGrade.SelectedValue);
                if (_prod_hb.PROD_GRADE == 2)
                    _prod_hb.KIDS = 'S';
                else
                    _prod_hb.KIDS = 'N';

                _prod_hb.FOTO_TECIDO = imgFotoTecido.ImageUrl;
                _prod_hb.TIPO = (rdbAmpliacao.Checked) ? 'A' : 'R';
                _prod_hb.FOTO_PECA = imgFotoPeca.ImageUrl;
                _prod_hb.OBSERVACAO = txtObservacao.Text.Trim().ToUpper();
                _prod_hb.STATUS = 'P';
                _prod_hb.AMPLIACAO_OUTRO = txtOutro.Text.Trim().ToUpper();
                _prod_hb.MOSTRUARIO = (cbMostruario.Checked) ? 'S' : 'N';
                if (txtMolde.Text.Trim() != "")
                    _prod_hb.MOLDE = txtMolde.Text.Trim().ToUpper();
                if (txtCodigoLinx.Text.Trim() != "")
                    _prod_hb.CODIGO_PRODUTO_LINX = txtCodigoLinx.Text.Trim();
                if (txtHBCopia.Text.Trim() != "")
                    _prod_hb.HB_COPIADO = Convert.ToInt32(txtHBCopia.Text.Trim());

                if (txtGastoPorFolha.Text.Trim() != "")
                    _prod_hb.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtGastoPorFolha.Text.Trim());
                if (txtPrecoFaccMostruario.Text.Trim() != "")
                    _prod_hb.PRECO_FACC_MOSTRUARIO = Convert.ToDecimal(txtPrecoFaccMostruario.Text.Trim());

                _prod_hb.ETIQUETA_COMPOSICAO = 'S';
                _prod_hb.GABARITO = Convert.ToChar(ddlGabarito.SelectedValue);
                _prod_hb.ESTAMPARIA = Convert.ToChar(ddlEstamparia.SelectedValue);
                _prod_hb.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue);
                _prod_hb.CORTE_SIMULACAO = (cbSimulacaoCorte.Checked) ? 'S' : 'N';

                //validar peso do tecido para calcular estoque
                var rendimento = prodController.ObterRendimentoTecidoKG(_prod_hb.NUMERO_PEDIDO.ToString());
                if (rendimento != null)
                    _prod_hb.RENDIMENTO_1MT = rendimento.RENDIMENTO_1MT;


                AtualizarHB(_prod_hb);

                PROD_HB_GRADE _grade = null;
                if (rdbAmpliacao.Checked)
                {
                    InserirMedidasAmpliacao();
                }

                if (rdbRisco.Checked)
                {
                    //Inserir grade
                    _grade = new PROD_HB_GRADE();
                    _grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                    _grade.PROD_PROCESSO = ObterProcessoOrdem((rdbAmpliacao.Checked) ? 1 : 2); //Se tiver amplicação, inserir processo de ampliação, senão, iniciar no processo do risco
                    _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                    _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                    _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                    _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                    _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                    _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                    _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                    _grade.DATA_INCLUSAO = DateTime.Now;
                    _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    IncluirGrade(_grade);

                    //DETALHES
                    foreach (PROD_HB d in prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value)))
                    {
                        //Inserir grade
                        _grade = new PROD_HB_GRADE();
                        _grade.PROD_HB = d.CODIGO;
                        _grade.PROD_PROCESSO = 2; // COMECA SEMPRE NO RISCO = 2
                        _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                        _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                        _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                        _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                        _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                        _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                        _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                        _grade.DATA_INCLUSAO = DateTime.Now;
                        _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        IncluirGrade(_grade);

                        //if (d.PROD_DETALHE == 4 || d.PROD_DETALHE == 5 || cbMostruario.Checked)
                        //{
                        //    _processo = new PROD_HB_PROD_PROCESSO();
                        //    _processo.PROD_HB = d.CODIGO;
                        //    _processo.PROD_PROCESSO = ObterProcessoOrdem(2); //DETALHE É SEMPRE RISCO
                        //    _processo.DATA_INICIO = DateTime.Now;
                        //    IncluirProcesso(_processo);
                        //}

                        if (cbMostruario.Checked || !cbLiquidar.Checked)
                        {
                            var processoDetExiste = prodController.ObterProcessoHB(d.CODIGO, 2);
                            if (processoDetExiste == null)
                            {
                                var processoDet = new PROD_HB_PROD_PROCESSO();
                                processoDet.PROD_HB = d.CODIGO;
                                processoDet.PROD_PROCESSO = ObterProcessoOrdem(2); //DETALHE É SEMPRE RISCO
                                processoDet.DATA_INICIO = DateTime.Now;
                                IncluirProcesso(processoDet);
                            }
                        }

                    }
                }

                //GERAR RELATORIO
                RelatorioHB _relatorio = new RelatorioHB();
                _relatorio.CODIGO = _prod_hb.CODIGO;
                _relatorio.HB = _prod_hb.HB.ToString();
                _relatorio.COLECAO = ddlColecoes.SelectedItem.Text.Trim();
                _relatorio.COD_COR = ddlCor.SelectedValue.Trim();
                _relatorio.DESC_COR = ddlCor.SelectedItem.Text.Trim();
                _relatorio.COR_FORNECEDOR = ddlCorFornecedor.SelectedItem.Text.Trim();
                _relatorio.GRUPO = ddlGrupo.SelectedItem.Text.Trim();
                _relatorio.NOME = _prod_hb.NOME.Trim();
                _relatorio.DATA = DateTime.Now.ToString("dd/MM/yyyy");
                _relatorio.GRUPO_TECIDO = _prod_hb.GRUPO_TECIDO.Trim();
                _relatorio.TECIDO = _prod_hb.TECIDO.Trim();
                _relatorio.FORNECEDOR = ddlFornecedor.SelectedValue.Trim().ToUpper();
                _relatorio.LARGURA = _prod_hb.LARGURA.ToString();
                _relatorio.LIQUIDAR = (cbLiquidar.Checked) ? "Sim" : "Não";
                _relatorio.LIQUIDAR_PQNO = (cbLiquidarPqno.Checked) ? "Sim" : "Não";
                //_relatorio.ATACADO = (cbAtacado.Checked) ? "Sim" : "Não";
                _relatorio.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
                _relatorio.CUSTO_TECIDO = txtCustoTecido.Text;
                _relatorio.FOTO_PECA = _prod_hb.FOTO_PECA;
                _relatorio.FOTO_TECIDO = _prod_hb.FOTO_TECIDO;
                _relatorio.COMPOSICAO = prodController.ObterComposicaoHB(_prod_hb.CODIGO);
                _relatorio.GRADE = _grade;
                _relatorio.DETALHE = prodController.ObterDetalhesHB(_prod_hb.CODIGO);
                _relatorio.OBS = _prod_hb.OBSERVACAO.Trim().ToUpper();
                _relatorio.AMPLIACAOMEDIDA = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == _prod_hb.CODIGO).ToList();
                _relatorio.AMPLIACAO_OUTRO = _prod_hb.AMPLIACAO_OUTRO;
                _relatorio.MOLDE = txtMolde.Text.Trim();
                _relatorio.PROD_GRADE = ddlGrade.SelectedValue;
                _relatorio.CODIGO_PRODUTO_LINX = _prod_hb.CODIGO_PRODUTO_LINX;
                _relatorio.MOSTRUARIO = (cbMostruario.Checked) ? "S" : "N";
                _relatorio.AVIAMENTO = prodController.ObterAviamentoHB(_prod_hb.CODIGO);
                _relatorio.GABARITO = ddlGabarito.SelectedValue;
                _relatorio.ESTAMPARIA = ddlEstamparia.SelectedValue;
                _relatorio.MOSTRUARIO = (cbMostruario.Checked) ? "S" : "N";
                _relatorio.PROCESSO = new PROD_HB_PROD_PROCESSO();
                _relatorio.PROCESSO.DATA_INICIO = DateTime.Now;

                var desenvProduto = desenvController.ObterProduto(ddlColecoes.SelectedValue, txtCodigoLinx.Text, _relatorio.COD_COR);
                if (desenvProduto != null)
                {
                    if (desenvProduto.GRIFFE != null)
                        _relatorio.GRIFFE = desenvProduto.GRIFFE;

                    _relatorio.SIGNED = "N";
                    if (desenvProduto.SIGNED != null)
                        _relatorio.SIGNED = desenvProduto.SIGNED.ToString();

                    _relatorio.SIGNED_NOME = "";
                    if (desenvProduto.SIGNED_NOME != null)
                        _relatorio.SIGNED_NOME = desenvProduto.SIGNED_NOME;

                }

                bMostruario = (cbMostruario.Checked) ? true : false;
                GerarRelatorio(_relatorio, bMostruario);

                labErroEnvio.Text = "<strong>OK...</strong>";

                Session["COLECAO"] = ddlColecoes.SelectedValue;

                labHBPopUp.Text = txtHB.Text;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, Risco: function () { window.open('prod_fila_risco.aspx', '_self'); }, Corte: function () { window.open('prod_fila_corte.aspx', '_self'); }, 'Rel. Aviamento': function () { window.open('prod_rel_aviamento.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErroEnvio.Text = "Erro (btEnviar_Click): " + ex.Message;
            }
        }
        #endregion

        #region "OUTROS"
        private void CarregarOrigemProduto(string colecao, string codigoProduto)
        {
            var desenvProduto = desenvController.ObterProduto(colecao, codigoProduto).FirstOrDefault();
            if (desenvProduto != null)
            {
                //if (desenvProduto.DESENV_PRODUTO_ORIGEM != null)
                ddlOrigem.SelectedValue = desenvProduto.DESENV_PRODUTO_ORIGEM.ToString();
            }

        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
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
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;
            bool ampliacao = rdbAmpliacao.Checked;

            labHB.ForeColor = _OK;
            if (txtHB.Text == "" || Convert.ToInt32(txtHB.Text) <= 0)
            {
                labHB.ForeColor = _notOK;
                retorno = false;
            }

            labCodigoLinx.ForeColor = _OK;
            if (txtCodigoLinx.Text == "" || Convert.ToInt32(txtCodigoLinx.Text) <= 0)
            {
                labCodigoLinx.ForeColor = _notOK;
                retorno = false;
            }

            labGrupo.ForeColor = _OK;
            if (ddlGrupo.SelectedValue == "" || ddlGrupo.SelectedValue == "Selecione")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labNome.ForeColor = _OK;
            if (txtNome.Text.Trim() == "")
            {
                labNome.ForeColor = _notOK;
                retorno = false;
            }

            labColecao.ForeColor = _OK;
            if (ddlColecoes.SelectedValue == "" || ddlColecoes.SelectedValue == "0" || ddlColecoes.SelectedValue == "Selecione")
            {
                labColecao.ForeColor = _notOK;
                retorno = false;
            }

            labOrigem.ForeColor = _OK;
            if (ddlOrigem.SelectedValue == "" || ddlOrigem.SelectedValue == "0")
            {
                labOrigem.ForeColor = _notOK;
                retorno = false;
            }

            labGrupoTecido.ForeColor = _OK;
            if (ddlGrupoTecido.SelectedValue.Trim() == "")
            {
                labGrupoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupoTecido.ForeColor = _OK;
            if (ddlSubGrupoTecido.SelectedValue.Trim() == "Selecione")
            {
                labSubGrupoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labPedidoNumero.ForeColor = _OK;
            if (txtPedidoNumero.Text.Trim() == "" || Convert.ToInt32(txtPedidoNumero.Text) <= 0)
            {
                labPedidoNumero.ForeColor = _notOK;
                retorno = false;
            }

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue == "" || ddlCor.SelectedValue == "0" || ddlCor.SelectedValue == "Selecione")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedor.ForeColor = _OK;
            if (ddlCorFornecedor.SelectedItem.Text.Trim() == "")
            {
                labCorFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "" || ddlFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labModelagem.ForeColor = _OK;
            if (txtModelagem.Text.Trim() == "")
            {
                labModelagem.ForeColor = _notOK;
                retorno = false;
            }

            labLargura.ForeColor = _OK;
            if (txtLargura.Text.Trim() == "" || Convert.ToDecimal(txtLargura.Text.Trim()) <= 0)
            {
                labLargura.ForeColor = _notOK;
                retorno = false;
            }

            labCustoTecido.ForeColor = _OK;
            if (txtCustoTecido.Text.Trim() == "" || Convert.ToDecimal(txtCustoTecido.Text.Trim()) <= 0)
            {
                labCustoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labMolde.ForeColor = _OK;
            if (txtMolde.Text.Trim() == "" && !ampliacao)
            {
                labMolde.ForeColor = _notOK;
                retorno = false;
            }

            labNomeGrade.ForeColor = _OK;
            if (ddlGrade.SelectedValue == "" || ddlGrade.SelectedValue == "0")
            {
                labNomeGrade.ForeColor = _notOK;
                retorno = false;
            }

            labFotoTecido.ForeColor = _OK;
            if (imgFotoTecido.ImageUrl == "" && !ampliacao)
            {
                labFotoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labComposicaoTecido.ForeColor = _OK;
            if ((gvComposicao.Rows.Count <= 0 || Convert.ToDecimal(hidTotalComp.Value) < 100) && !ampliacao)
            {
                labComposicaoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labGrade.ForeColor = _OK;
            if ((ValidarGrade() <= 0) && !ampliacao)
            {
                labGrade.ForeColor = _notOK;
                retorno = false;
            }

            labTipo.ForeColor = _OK;
            if (!rdbAmpliacao.Checked && !rdbRisco.Checked)
            {
                labTipo.ForeColor = _notOK;
                retorno = false;
            }

            labFotoPeca.ForeColor = _OK;
            if ((imgFotoPeca.ImageUrl == "") && !ampliacao)
            {
                labFotoPeca.ForeColor = _notOK;
                retorno = false;
            }

            labPrecoFaccMostruario.ForeColor = _OK;
            if (txtPrecoFaccMostruario.Text.Trim() == "")
            {
                labPrecoFaccMostruario.ForeColor = _notOK;
                retorno = false;
            }

            labGabarito.ForeColor = _OK;
            if (ddlGabarito.SelectedValue == "")
            {
                labGabarito.ForeColor = _notOK;
                retorno = false;
            }

            labEstamparia.ForeColor = _OK;
            if (ddlEstamparia.SelectedValue == "")
            {
                labEstamparia.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private int ValidarGrade()
        {
            string _exp = txtGradeEXP.Text.Trim();
            string _xp = txtGradeXP.Text.Trim();
            string _pp = txtGradePP.Text.Trim();
            string _p = txtGradeP.Text.Trim();
            string _m = txtGradeM.Text.Trim();
            string _g = txtGradeG.Text.Trim();
            string _gg = txtGradeGG.Text.Trim();

            int total = 0;

            if (_exp != "" && Convert.ToInt32(_exp) > 0)
                total += Convert.ToInt32(_exp);
            if (_xp != "" && Convert.ToInt32(_xp) > 0)
                total += Convert.ToInt32(_xp);
            if (_pp != "" && Convert.ToInt32(_pp) > 0)
                total += Convert.ToInt32(_pp);
            if (_p != "" && Convert.ToInt32(_p) > 0)
                total += Convert.ToInt32(_p);
            if (_m != "" && Convert.ToInt32(_m) > 0)
                total += Convert.ToInt32(_m);
            if (_g != "" && Convert.ToInt32(_g) > 0)
                total += Convert.ToInt32(_g);
            if (_gg != "" && Convert.ToInt32(_gg) > 0)
                total += Convert.ToInt32(_gg);

            return total;
        }
        private string ValidarHB(Label _label)
        {
            string msg = "";
            try
            {
                if (txtHB.Text == "")
                {
                    msg = "Informe o número do HB.";
                    _label.Text = msg;
                    return msg;
                }
                if (ddlColecoes.SelectedValue == "" || ddlColecoes.SelectedValue == "0" || ddlColecoes.SelectedValue == "Selecione")
                {
                    msg = "Selecione a Coleção.";
                    _label.Text = msg;
                    return msg;
                }

                List<PROD_HB> _hb = prodController.ValidarNumeroHB(Convert.ToInt32(txtHB.Text), ddlColecoes.SelectedValue);
                if (_hb != null)
                {
                    if (cbMostruario.Checked)
                    {
                        if (_hb.Where(p => p.MOSTRUARIO == 'S').Count() > 0)
                        {
                            msg = "O número do HB informado já existe nesta Coleção para MOSTRUÁRIO. Informe outro número.";
                            _label.Text = msg;
                            return msg;
                        }
                    }
                    else
                    {
                        if (_hb.Where(p => p.MOSTRUARIO == 'N').Count() > 0)
                        {
                            msg = "O número do HB informado já existe nesta Coleção para VENDA. Informe outro número.";
                            _label.Text = msg;
                            return msg;
                        }
                    }
                }

                return msg;
            }
            catch (Exception ex)
            {
                msg = "ERRO ao Consultar HB existente para esta coleção. \n\n Message: " + ex.Message;
                _label.Text = msg;
                return msg;
            }
        }
        private bool ValidarPedido()
        {
            bool retorno = true;

            //PEDIDO EXISTE?
            DESENV_PEDIDO _pedido = null;
            _pedido = (new DesenvolvimentoController().ObterPedidoNumero(Convert.ToInt32(txtPedidoNumero.Text)));
            if (_pedido == null)
                retorno = false;

            return retorno;
        }
        private bool ValidarCodigoLinx()
        {
            PRODUTO _produto = null;
            string _codigoProduto = "";

            //CODIGO LINX EXISTE?
            _produto = (new BaseController().BuscaProduto(txtCodigoLinx.Text));
            if (_produto == null)
                return false;

            // LAINY - 16/06/2016 - ABRIR ORDEM DE CORTE DE ACESSÓRIOS
            //CODIGO LINX CATEGORIA = "01" ?
            //if (_produto.COD_CATEGORIA != "01")
            //    return false;

            //CODIGO LINX INICIA COM NUMERO PAR? //PRODUTO NACIONAL
            _codigoProduto = _produto.PRODUTO1.Substring(0, 1);
            if (_codigoProduto != "" && Convert.ToInt32(_codigoProduto) > 0)
                if ((Convert.ToInt32(_codigoProduto) % 2) != 0)
                    return false;

            return true;
        }

        #endregion

        #region "COPIAR HB"
        private void CriarCopiaHB(PROD_HB prod_hb)
        {
            //Campos FIXOS
            ddlColecoes.SelectedValue = prod_hb.COLECAO;
            //Obter ultimo HB
            ddlColecoes_SelectedIndexChanged(null, null);
            //txtHB.Text = prod_hb.HB.ToString();
            txtCodigoLinx.Text = prod_hb.CODIGO_PRODUTO_LINX.ToString();
            txtCodigoLinx_TextChanged(null, null);
            ddlGrupo.SelectedValue = prod_hb.GRUPO;
            txtNome.Text = prod_hb.NOME.Trim();
            ddlGrupoTecido.SelectedValue = prod_hb.GRUPO_TECIDO.Trim();
            CarregarSubGrupo(prod_hb.GRUPO_TECIDO.Trim(), "T");
            ddlSubGrupoTecido.SelectedValue = prod_hb.TECIDO.Trim();
            CarregarCorFornecedor(prod_hb.GRUPO_TECIDO, prod_hb.TECIDO, ddlCorFornecedor);
            txtPedidoNumero.Text = prod_hb.NUMERO_PEDIDO.ToString();
            ddlCor.SelectedValue = prod_hb.COR;
            ddlCorFornecedor.SelectedValue = prod_hb.COR_FORNECEDOR;
            ddlFornecedor.SelectedValue = prod_hb.FORNECEDOR;
            txtModelagem.Text = prod_hb.MODELAGEM;
            txtLargura.Text = prod_hb.LARGURA.ToString();
            txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
            imgFotoTecido.ImageUrl = prod_hb.FOTO_TECIDO;
            imgFotoPeca.ImageUrl = prod_hb.FOTO_PECA;
            txtMolde.Text = (prod_hb.MOLDE == null) ? "0" : prod_hb.MOLDE.ToString();
            txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtOutro.Text = prod_hb.AMPLIACAO_OUTRO;
            if (prod_hb.PROD_GRADE != null)
                ddlGrade.SelectedValue = prod_hb.PROD_GRADE.ToString();
            ddlGrade_SelectedIndexChanged(ddlGrade, null);

            //Preencher composição
            List<PROD_HB_COMPOSICAO> composicao = new List<PROD_HB_COMPOSICAO>();
            composicao = prodController.ObterComposicaoHB(prod_hb.CODIGO);
            PROD_HB_COMPOSICAO novaComposicao = null;
            decimal totalComposicao = 0;
            foreach (PROD_HB_COMPOSICAO c in composicao)
            {
                if (c != null)
                {
                    novaComposicao = new PROD_HB_COMPOSICAO();
                    novaComposicao.PROD_HB = Convert.ToInt32(hidHB.Value);
                    novaComposicao.QTDE = c.QTDE;
                    novaComposicao.DESCRICAO = c.DESCRICAO;
                    IncluirComposicao(novaComposicao);

                    totalComposicao += novaComposicao.QTDE;
                }
            }
            hidTotalComp.Value = totalComposicao.ToString();
            RecarregarComposicao();

            //Preencher aviamento
            List<PROD_HB_PROD_AVIAMENTO> aviamento = new List<PROD_HB_PROD_AVIAMENTO>();
            aviamento = prodController.ObterAviamentoHB(prod_hb.CODIGO);
            PROD_HB_PROD_AVIAMENTO novoAviamento = null;
            foreach (PROD_HB_PROD_AVIAMENTO a in aviamento)
            {
                if (a != null)
                {
                    novoAviamento = new PROD_HB_PROD_AVIAMENTO();
                    novoAviamento.PROD_HB = Convert.ToInt32(hidHB.Value);
                    novoAviamento.DESCRICAO = a.DESCRICAO;
                    novoAviamento.PROD_AVIAMENTO = a.PROD_AVIAMENTO;
                    novoAviamento.QTDE = a.QTDE;
                    novoAviamento.MATERIAL = a.MATERIAL;
                    novoAviamento.GRUPO = a.GRUPO;
                    novoAviamento.SUBGRUPO = a.SUBGRUPO;
                    novoAviamento.COR = a.COR;
                    novoAviamento.COR_FORNECEDOR = a.COR_FORNECEDOR;
                    novoAviamento.QTDE_POR_PECA = a.QTDE_POR_PECA;
                    novoAviamento.DATA_INCLUSAO = DateTime.Now;
                    IncluirAviamento(novoAviamento);
                }
            }
            RecarregarAviamento();

            //Preencher Ampliacao
            List<PROD_HB_AMPLIACAO_MEDIDA> ampliacaoMedida = new List<PROD_HB_AMPLIACAO_MEDIDA>();
            ampliacaoMedida = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == Convert.ToInt32(prod_hb.CODIGO)).ToList();
            PROD_HB_AMPLIACAO_MEDIDA novaAmpliacaoMedida = null;
            foreach (PROD_HB_AMPLIACAO_MEDIDA am in ampliacaoMedida)
            {
                if (am != null)
                {
                    novaAmpliacaoMedida = new PROD_HB_AMPLIACAO_MEDIDA();
                    novaAmpliacaoMedida.PROD_HB = Convert.ToInt32(hidHB.Value);
                    novaAmpliacaoMedida.PROD_HB_AMPLIACAO = am.PROD_HB_AMPLIACAO;
                    novaAmpliacaoMedida.LOCAL = am.LOCAL;
                    novaAmpliacaoMedida.QTDE = am.QTDE;
                    novaAmpliacaoMedida.COMPRIMENTO = am.COMPRIMENTO;
                    novaAmpliacaoMedida.LARGURA = am.LARGURA;
                    novaAmpliacaoMedida.DESCRICAO = am.DESCRICAO;
                    novaAmpliacaoMedida.GRADE_EXP = am.GRADE_EXP;
                    novaAmpliacaoMedida.GRADE_XP = am.GRADE_XP;
                    novaAmpliacaoMedida.GRADE_PP = am.GRADE_PP;
                    novaAmpliacaoMedida.GRADE_P = am.GRADE_P;
                    novaAmpliacaoMedida.GRADE_M = am.GRADE_M;
                    novaAmpliacaoMedida.GRADE_G = am.GRADE_G;
                    novaAmpliacaoMedida.GRADE_GG = am.GRADE_GG;
                    novaAmpliacaoMedida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    novaAmpliacaoMedida.DATA_INCLUSAO = DateTime.Now;
                    prodController.InserirAmpliacaoMedidasHB(novaAmpliacaoMedida);
                }
            }
            //PreencherCampos
            PreencherAmpliacaoMedidas();

            //Preencher os detalhes
            CriarCopiaHBDetalhe(prod_hb.CODIGO);

        }
        private void CriarCopiaHBDetalhe(int codigoHB)
        {
            List<PROD_HB> detalhes = new List<PROD_HB>();
            detalhes = prodController.ObterDetalhesHB(codigoHB);

            PROD_HB _novoDetalhe = null;
            foreach (PROD_HB d in detalhes)
            {
                int codigo;
                _novoDetalhe = new PROD_HB();
                _novoDetalhe.CODIGO_PAI = Convert.ToInt32(hidHB.Value);
                _novoDetalhe.HB = Convert.ToInt32(txtHB.Text);
                _novoDetalhe.PROD_DETALHE = d.PROD_DETALHE;
                _novoDetalhe.COLECAO = d.COLECAO;
                _novoDetalhe.COR = d.COR;
                _novoDetalhe.COR_FORNECEDOR = d.COR_FORNECEDOR;
                _novoDetalhe.GRUPO = d.GRUPO;
                _novoDetalhe.NOME = d.NOME;
                _novoDetalhe.DATA_INCLUSAO = DateTime.Now;
                _novoDetalhe.GRUPO_TECIDO = d.GRUPO_TECIDO;
                _novoDetalhe.TECIDO = d.TECIDO;
                _novoDetalhe.FORNECEDOR = d.FORNECEDOR;
                _novoDetalhe.LARGURA = d.LARGURA;
                _novoDetalhe.CUSTO_TECIDO = d.CUSTO_TECIDO;
                _novoDetalhe.LIQUIDAR = 'N';
                _novoDetalhe.ATACADO = null;
                _novoDetalhe.MODELAGEM = d.MODELAGEM;
                _novoDetalhe.MOLDE = d.MOLDE;
                _novoDetalhe.FOTO_TECIDO = d.FOTO_TECIDO;
                _novoDetalhe.TIPO = 'R';
                _novoDetalhe.OBSERVACAO = d.OBSERVACAO;
                _novoDetalhe.STATUS = 'P';
                _novoDetalhe.NUMERO_PEDIDO = d.NUMERO_PEDIDO;
                _novoDetalhe.MOSTRUARIO = 'N';

                codigo = IncluirDetalheHB(_novoDetalhe);

                PROD_HB_COMPOSICAO _novo = null;
                foreach (PROD_HB_COMPOSICAO c in prodController.ObterComposicaoHB(d.CODIGO))
                {
                    _novo = new PROD_HB_COMPOSICAO();
                    _novo.PROD_HB = codigo;
                    _novo.QTDE = c.QTDE;
                    _novo.DESCRICAO = c.DESCRICAO;
                    IncluirComposicao(_novo);
                }
            }

            RecarregarDetalhe();
        }
        protected void btCopiarHB_Click(object sender, EventArgs e)
        {
            try
            {
                labErroCopia.Text = "";
                //Validar Coleção e HB
                if (ddlColecaoCopia.SelectedValue == "" || ddlColecaoCopia.SelectedValue == "0" || ddlColecaoCopia.SelectedValue == "Selecione")
                {
                    labErroCopia.Text = "Selecione a Coleção.";
                    return;
                }
                if (txtHBCopia.Text.Trim() == "")
                {
                    labErroCopia.Text = "Informe o número do HB.";
                    return;
                }

                List<PROD_HB> prod_hb = new List<PROD_HB>();
                //Obter HB MOSTRUARIO PARA CÓPIA
                prod_hb = prodController.ObterNumeroHB(ddlColecaoCopia.SelectedValue.Trim(), Convert.ToInt32(txtHBCopia.Text.Trim())).Where(
                    p => p.CODIGO_PAI == null && p.STATUS != 'X' && p.MOSTRUARIO == 'S').ToList();

                //Validar se existe Mostruário do HB
                if (prod_hb == null || prod_hb.Count <= 0)
                {
                    labErroCopia.Text = "HB informado não existe. Informe outro HB.";
                    return;
                }

                /*
                if (prodController.ObterHB().Where(p => p.HB_COPIADO == Convert.ToInt32(txtHBCopia.Text.Trim())).Count() > 0)
                {
                    labErroCopia.Text = "HB informado já foi copiado. Informe outro HB.";
                    return;
                }*/

                //Controle de paineis e fieldsets
                pnlCabecalho.Visible = true;
                if (rdbRisco.Checked)
                {
                    pnlRisco.Visible = true;
                }
                else
                {
                    fsAmpliacao.Visible = true;
                }

                //Criar Copia do HB Informado e Detalhes
                CriarCopiaHB(prod_hb.Where(p => p.MOSTRUARIO == 'S').SingleOrDefault());

                //Bloquear campos de cópia
                ddlColecoes.Enabled = false;
                txtHB.Enabled = false;
                txtCodigoLinx.Enabled = false;
                ddlGrupo.Enabled = false;
                txtNome.Enabled = false;

                btCopiarHB.Enabled = false;
            }
            catch (Exception ex)
            {
                labErroCopia.Text = ex.Message;
            }
        }
        #endregion

        #region "AMPLIACAO"
        private void InserirMedidasAmpliacao()
        {
            int codigo = 0;
            bool bInserir = false;

            PROD_HB_AMPLIACAO_MEDIDA ampliacaoMedida = null;

            //ELASTICO
            foreach (GridViewRow row in gvElastico.Rows)
            {
                if (row != null)
                {
                    TextBox _txtLocal = row.FindControl("txtLocal") as TextBox;
                    TextBox _txtGradeEXP = row.FindControl("txtGradeEXP") as TextBox;
                    TextBox _txtGradeXP = row.FindControl("txtGradeXP") as TextBox;
                    TextBox _txtGradePP = row.FindControl("txtGradePP") as TextBox;
                    TextBox _txtGradeP = row.FindControl("txtGradeP") as TextBox;
                    TextBox _txtGradeM = row.FindControl("txtGradeM") as TextBox;
                    TextBox _txtGradeG = row.FindControl("txtGradeG") as TextBox;
                    TextBox _txtGradeGG = row.FindControl("txtGradeGG") as TextBox;
                    TextBox _txtLargura = row.FindControl("txtLargura") as TextBox;

                    ampliacaoMedida = new PROD_HB_AMPLIACAO_MEDIDA();

                    //Obter codigo tabela PROD_HB_AMPLIACAO_MEDIDA
                    codigo = Convert.ToInt32(gvElastico.DataKeys[row.RowIndex].Value);
                    ampliacaoMedida.CODIGO = codigo;
                    ampliacaoMedida.PROD_HB = Convert.ToInt32(hidHB.Value);
                    ampliacaoMedida.PROD_HB_AMPLIACAO = 1; //ELASTICO
                    ampliacaoMedida.LOCAL = _txtLocal.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_EXP = _txtGradeEXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_XP = _txtGradeXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_PP = _txtGradePP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_P = _txtGradeP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_M = _txtGradeM.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_G = _txtGradeG.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_GG = _txtGradeGG.Text.Trim().ToUpper();
                    ampliacaoMedida.LARGURA = _txtLargura.Text.Trim().ToUpper();
                    ampliacaoMedida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    ampliacaoMedida.DATA_INCLUSAO = DateTime.Now;

                    bInserir = false;
                    if ((_txtLocal.Text.Trim() != "") || (_txtGradeEXP.Text.Trim() != "") || (_txtGradeXP.Text.Trim() != "") || (_txtGradePP.Text.Trim() != "") || (_txtGradeP.Text.Trim() != "") || (_txtGradeM.Text.Trim() != "") || (_txtGradeG.Text.Trim() != "") || (_txtGradeGG.Text.Trim() != "") || (_txtLargura.Text.Trim() != ""))
                        bInserir = true;

                    if (bInserir) //Inserir/Alterar
                    {
                        if (codigo <= 0)
                            prodController.InserirAmpliacaoMedidasHB(ampliacaoMedida);
                        else
                            prodController.AtualizarAmpliacaoMedidasHB(ampliacaoMedida);
                    }
                    else
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(codigo);
                    }

                }
            }

            //GALÃO
            foreach (GridViewRow row in gvGalao.Rows)
            {
                if (row != null)
                {
                    TextBox _txtQtde = row.FindControl("txtQtde") as TextBox;
                    TextBox _txtLocal = row.FindControl("txtLocal") as TextBox;
                    TextBox _txtComprimento = row.FindControl("txtComprimento") as TextBox;
                    TextBox _txtLargura = row.FindControl("txtLargura") as TextBox;

                    ampliacaoMedida = new PROD_HB_AMPLIACAO_MEDIDA();

                    //Obter codigo tabela PROD_HB_AMPLIACAO_MEDIDA
                    codigo = Convert.ToInt32(gvGalao.DataKeys[row.RowIndex].Value);
                    ampliacaoMedida.CODIGO = codigo;
                    ampliacaoMedida.PROD_HB = Convert.ToInt32(hidHB.Value);
                    ampliacaoMedida.PROD_HB_AMPLIACAO = 2; // GALAO
                    ampliacaoMedida.QTDE = _txtQtde.Text.Trim().ToUpper();
                    ampliacaoMedida.LOCAL = _txtLocal.Text.Trim().ToUpper();
                    ampliacaoMedida.COMPRIMENTO = _txtComprimento.Text.Trim().ToUpper();
                    ampliacaoMedida.LARGURA = _txtLargura.Text.Trim().ToUpper();
                    ampliacaoMedida.DESCRICAO = txtGalaoDescricao.Text.Trim().ToUpper();
                    ampliacaoMedida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    ampliacaoMedida.DATA_INCLUSAO = DateTime.Now;

                    bInserir = false;
                    if ((_txtQtde.Text.Trim() != "") || (_txtLocal.Text.Trim() != "") || (_txtComprimento.Text.Trim() != "") || (_txtLargura.Text.Trim() != ""))
                        bInserir = true;

                    if (bInserir) //Inserir/Alterar
                    {
                        if (codigo <= 0)
                            prodController.InserirAmpliacaoMedidasHB(ampliacaoMedida);
                        else
                            prodController.AtualizarAmpliacaoMedidasHB(ampliacaoMedida);
                    }
                    else
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(codigo);
                    }

                }
            }

            //ALCA PRONTA
            foreach (GridViewRow row in gvAlcaPronta.Rows)
            {
                if (row != null)
                {
                    TextBox _txtGradeEXP = row.FindControl("txtGradeEXP") as TextBox;
                    TextBox _txtGradeXP = row.FindControl("txtGradeXP") as TextBox;
                    TextBox _txtGradePP = row.FindControl("txtGradePP") as TextBox;
                    TextBox _txtGradeP = row.FindControl("txtGradeP") as TextBox;
                    TextBox _txtGradeM = row.FindControl("txtGradeM") as TextBox;
                    TextBox _txtGradeG = row.FindControl("txtGradeG") as TextBox;
                    TextBox _txtGradeGG = row.FindControl("txtGradeGG") as TextBox;

                    ampliacaoMedida = new PROD_HB_AMPLIACAO_MEDIDA();

                    ///Obter codigo tabela PROD_HB_AMPLIACAO_MEDIDA
                    codigo = Convert.ToInt32(gvAlcaPronta.DataKeys[row.RowIndex].Value);
                    ampliacaoMedida.CODIGO = codigo;
                    ampliacaoMedida.PROD_HB = Convert.ToInt32(hidHB.Value);
                    ampliacaoMedida.PROD_HB_AMPLIACAO = 3; // ALÇA PRONTA
                    ampliacaoMedida.GRADE_EXP = _txtGradeEXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_XP = _txtGradeXP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_PP = _txtGradePP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_P = _txtGradeP.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_M = _txtGradeM.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_G = _txtGradeG.Text.Trim().ToUpper();
                    ampliacaoMedida.GRADE_GG = _txtGradeGG.Text.Trim().ToUpper();
                    ampliacaoMedida.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    ampliacaoMedida.DATA_INCLUSAO = DateTime.Now;

                    bInserir = false;
                    if ((_txtGradeEXP.Text.Trim() != "") || (_txtGradeXP.Text.Trim() != "") || (_txtGradePP.Text.Trim() != "") || (_txtGradeP.Text.Trim() != "") || (_txtGradeM.Text.Trim() != "") || (_txtGradeG.Text.Trim() != "") || (_txtGradeGG.Text.Trim() != ""))
                        bInserir = true;

                    if (bInserir) //Inserir/Alterar
                    {
                        if (codigo <= 0)
                            prodController.InserirAmpliacaoMedidasHB(ampliacaoMedida);
                        else
                            prodController.AtualizarAmpliacaoMedidasHB(ampliacaoMedida);
                    }
                    else
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(codigo);
                    }

                }
            }
        }
        private void CarregarAmpliacaoMedidas()
        {
            List<PROD_HB_AMPLIACAO_MEDIDA> lstAmpliacaoMedida = new List<PROD_HB_AMPLIACAO_MEDIDA>();

            if (lstAmpliacaoMedida.Count < 6)
            {
                int i = lstAmpliacaoMedida.Count;
                for (; i < 6; i++)
                    lstAmpliacaoMedida.Add(new PROD_HB_AMPLIACAO_MEDIDA { CODIGO = 0 });
            }

            gvElastico.DataSource = lstAmpliacaoMedida;
            gvElastico.DataBind();

            gvGalao.DataSource = lstAmpliacaoMedida;
            gvGalao.DataBind();

            gvAlcaPronta.DataSource = lstAmpliacaoMedida;
            gvAlcaPronta.DataBind();
        }
        private void PreencherAmpliacaoMedidas()
        {
            List<PROD_HB_AMPLIACAO_MEDIDA> lstAmpliacaoMedidaElastico = new List<PROD_HB_AMPLIACAO_MEDIDA>();
            List<PROD_HB_AMPLIACAO_MEDIDA> lstAmpliacaoMedidaGalao = new List<PROD_HB_AMPLIACAO_MEDIDA>();
            List<PROD_HB_AMPLIACAO_MEDIDA> lstAmpliacaoMedidaAlcaPronta = new List<PROD_HB_AMPLIACAO_MEDIDA>();

            /*ELASTICO*/
            lstAmpliacaoMedidaElastico = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == Convert.ToInt32(hidHB.Value) && p.PROD_HB_AMPLIACAO == 1).ToList();
            if (lstAmpliacaoMedidaElastico.Count < 6)
            {
                int i = 6 - lstAmpliacaoMedidaElastico.Count;
                for (; i > 0; i--)
                    lstAmpliacaoMedidaElastico.Add(new PROD_HB_AMPLIACAO_MEDIDA { CODIGO = 0 });
            }
            gvElastico.DataSource = lstAmpliacaoMedidaElastico;
            gvElastico.DataBind();
            /************************************************************************************/

            /*GALAO*/
            lstAmpliacaoMedidaGalao = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == Convert.ToInt32(hidHB.Value) && p.PROD_HB_AMPLIACAO == 2).ToList();
            if (lstAmpliacaoMedidaGalao.Count < 6)
            {
                int i = 6 - lstAmpliacaoMedidaGalao.Count;
                for (; i > 0; i--)
                    lstAmpliacaoMedidaGalao.Add(new PROD_HB_AMPLIACAO_MEDIDA { CODIGO = 0 });
            }
            gvGalao.DataSource = lstAmpliacaoMedidaGalao;
            gvGalao.DataBind();

            txtGalaoDescricao.Text = lstAmpliacaoMedidaGalao[0].DESCRICAO;
            /************************************************************************************/

            /*ALÇA PRONTA*/
            lstAmpliacaoMedidaAlcaPronta = prodController.ObterAmpliacaoMedidasHB().Where(p => p.PROD_HB == Convert.ToInt32(hidHB.Value) && p.PROD_HB_AMPLIACAO == 3).ToList();
            if (lstAmpliacaoMedidaAlcaPronta.Count < 6)
            {
                int i = 6 - lstAmpliacaoMedidaAlcaPronta.Count;
                for (; i > 0; i--)
                    lstAmpliacaoMedidaAlcaPronta.Add(new PROD_HB_AMPLIACAO_MEDIDA { CODIGO = 0 });
            }
            gvAlcaPronta.DataSource = lstAmpliacaoMedidaAlcaPronta;
            gvAlcaPronta.DataBind();
            /************************************************************************************/
        }
        protected void gvElastico_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[2].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[3].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[4].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[5].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[6].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[8].Text = _gradeNome.GRADE_GG;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_AMPLIACAO_MEDIDA _ampliacaoElastico = e.Row.DataItem as PROD_HB_AMPLIACAO_MEDIDA;

                    colAmpliacaoElastico += 1;
                    if (_ampliacaoElastico != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAmpliacaoElastico.ToString();

                        TextBox _txtLocal = e.Row.FindControl("txtLocal") as TextBox;
                        if (_txtLocal != null)
                            _txtLocal.Text = _ampliacaoElastico.LOCAL;

                        TextBox _txtGradeEXP = e.Row.FindControl("txtGradeEXP") as TextBox;
                        if (_txtGradeEXP != null)
                            _txtGradeEXP.Text = _ampliacaoElastico.GRADE_EXP;

                        TextBox _txtGradeXP = e.Row.FindControl("txtGradeXP") as TextBox;
                        if (_txtGradeXP != null)
                            _txtGradeXP.Text = _ampliacaoElastico.GRADE_XP;

                        TextBox _txtGradePP = e.Row.FindControl("txtGradePP") as TextBox;
                        if (_txtGradePP != null)
                            _txtGradePP.Text = _ampliacaoElastico.GRADE_PP;

                        TextBox _txtGradeP = e.Row.FindControl("txtGradeP") as TextBox;
                        if (_txtGradeP != null)
                        {
                            _txtGradeP.Text = _ampliacaoElastico.GRADE_P;
                            if (_gradeNome.GRADE_P == "-")
                                _txtGradeP.Enabled = false;
                        }

                        TextBox _txtGradeM = e.Row.FindControl("txtGradeM") as TextBox;
                        if (_txtGradeM != null)
                        {
                            _txtGradeM.Text = _ampliacaoElastico.GRADE_M;
                            if (_gradeNome.GRADE_M == "-")
                                _txtGradeM.Enabled = false;
                        }

                        TextBox _txtGradeG = e.Row.FindControl("txtGradeG") as TextBox;
                        if (_txtGradeG != null)
                        {
                            _txtGradeG.Text = _ampliacaoElastico.GRADE_G;
                            if (_gradeNome.GRADE_G == "-")
                                _txtGradeG.Enabled = false;
                        }

                        TextBox _txtGradeGG = e.Row.FindControl("txtGradeGG") as TextBox;
                        if (_txtGradeGG != null)
                        {
                            _txtGradeGG.Text = _ampliacaoElastico.GRADE_GG;
                            if (_gradeNome.GRADE_GG == "-")
                                _txtGradeGG.Enabled = false;
                        }

                        TextBox _txtLargura = e.Row.FindControl("txtLargura") as TextBox;
                        if (_txtLargura != null)
                            _txtLargura.Text = _ampliacaoElastico.LARGURA;
                    }
                }
            }
        }
        protected void gvGalao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_AMPLIACAO_MEDIDA _ampliacaoGalao = e.Row.DataItem as PROD_HB_AMPLIACAO_MEDIDA;

                    colAmpliacaoGalao += 1;
                    if (_ampliacaoGalao != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAmpliacaoGalao.ToString();

                        TextBox _txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                        if (_txtQtde != null)
                            _txtQtde.Text = _ampliacaoGalao.QTDE;

                        TextBox _txtLocal = e.Row.FindControl("txtLocal") as TextBox;
                        if (_txtLocal != null)
                            _txtLocal.Text = _ampliacaoGalao.LOCAL;

                        TextBox _txtComprimento = e.Row.FindControl("txtComprimento") as TextBox;
                        if (_txtComprimento != null)
                            _txtComprimento.Text = _ampliacaoGalao.COMPRIMENTO;

                        TextBox _txtLargura = e.Row.FindControl("txtLargura") as TextBox;
                        if (_txtLargura != null)
                            _txtLargura.Text = _ampliacaoGalao.LARGURA;
                    }
                }
            }
        }
        protected void gvAlcaPronta_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (_gradeNome != null)
                {
                    e.Row.Cells[1].Text = _gradeNome.GRADE_EXP;
                    e.Row.Cells[2].Text = _gradeNome.GRADE_XP;
                    e.Row.Cells[3].Text = _gradeNome.GRADE_PP;
                    e.Row.Cells[4].Text = _gradeNome.GRADE_P;
                    e.Row.Cells[5].Text = _gradeNome.GRADE_M;
                    e.Row.Cells[6].Text = _gradeNome.GRADE_G;
                    e.Row.Cells[7].Text = _gradeNome.GRADE_GG;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_AMPLIACAO_MEDIDA _ampliacaoAlcaPronta = e.Row.DataItem as PROD_HB_AMPLIACAO_MEDIDA;

                    colAmpliacaoAlcaPronta += 1;
                    if (_ampliacaoAlcaPronta != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAmpliacaoAlcaPronta.ToString();

                        TextBox _txtGradeEXP = e.Row.FindControl("txtGradeEXP") as TextBox;
                        if (_txtGradeEXP != null)
                            _txtGradeEXP.Text = _ampliacaoAlcaPronta.GRADE_EXP;

                        TextBox _txtGradeXP = e.Row.FindControl("txtGradeXP") as TextBox;
                        if (_txtGradeXP != null)
                            _txtGradeXP.Text = _ampliacaoAlcaPronta.GRADE_XP;

                        TextBox _txtGradePP = e.Row.FindControl("txtGradePP") as TextBox;
                        if (_txtGradePP != null)
                            _txtGradePP.Text = _ampliacaoAlcaPronta.GRADE_PP;

                        TextBox _txtGradeP = e.Row.FindControl("txtGradeP") as TextBox;
                        if (_txtGradeP != null)
                        {
                            _txtGradeP.Text = _ampliacaoAlcaPronta.GRADE_P;
                            if (_gradeNome.GRADE_P == "-")
                                _txtGradeP.Enabled = false;
                        }

                        TextBox _txtGradeM = e.Row.FindControl("txtGradeM") as TextBox;
                        if (_txtGradeM != null)
                        {
                            _txtGradeM.Text = _ampliacaoAlcaPronta.GRADE_M;
                            if (_gradeNome.GRADE_M == "-")
                                _txtGradeM.Enabled = false;
                        }

                        TextBox _txtGradeG = e.Row.FindControl("txtGradeG") as TextBox;
                        if (_txtGradeG != null)
                        {
                            _txtGradeG.Text = _ampliacaoAlcaPronta.GRADE_G;
                            if (_gradeNome.GRADE_G == "-")
                                _txtGradeG.Enabled = false;
                        }

                        TextBox _txtGradeGG = e.Row.FindControl("txtGradeGG") as TextBox;
                        if (_txtGradeGG != null)
                        {
                            _txtGradeGG.Text = _ampliacaoAlcaPronta.GRADE_GG;
                            if (_gradeNome.GRADE_GG == "-")
                                _txtGradeGG.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    if (b.CommandArgument.Trim().ToString() != "")
                    {
                        prodController.ExcluirAmpliacaoMedidasHB(Convert.ToInt32(b.CommandArgument));
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region "RELATORIO"
        private void GerarRelatorio(RelatorioHB _relatorio, bool bMostruario)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "HB_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_relatorio, bMostruario));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(RelatorioHB _relatorio, bool bMostruario)
        {
            StringBuilder _texto = new StringBuilder();

            if (rdbAmpliacao.Checked)
            {
                _texto = _relatorio.MontarCabecalho(_texto, "HB - Ampliação");
                _texto = _relatorio.MontarAmpliacao(_texto, _relatorio, "");
            }
            else if (rdbRisco.Checked && bMostruario)
            {
                _texto = _relatorio.MontarCabecalho(_texto, "HB - SKU Mostruário");
                _texto = _relatorio.MontarTecido(_texto, _relatorio);
                _texto = _relatorio.MontarComposicao(_texto, _relatorio);
                _texto = _relatorio.MontarGrade(_texto, _relatorio);
                _texto = _relatorio.MontarGradeCorte(_texto, _relatorio);
                _texto = _relatorio.MontarLinhasEmBranco(_texto);
                _texto = _relatorio.MontarDetalhes(_texto, _relatorio);
                _texto = _relatorio.MontarModelagem(_texto, _relatorio);
                _texto = _relatorio.MontarPeca(_texto, _relatorio);

                _texto = _relatorio.MontarOBS(_texto, _relatorio);
                _texto = _relatorio.MontarDetalheRiscoCAD(_texto, _relatorio, "breakbefore");

            }
            else if (rdbRisco.Checked && !bMostruario)
            {
                _texto = _relatorio.MontarCabecalho(_texto, "HB - SKU CAD");
                _texto = _relatorio.MontarRiscoCAD(_texto, _relatorio);
                _texto = _relatorio.MontarReciboFaccao(_texto, _relatorio, "breakafter");

                if (_relatorio.LIQUIDAR == "Sim" && !bMostruario)// E LIQUIDAR PEQUENO NAO
                {
                    _relatorio.DETALHE = _relatorio.DETALHE.Where(p => p.PROD_DETALHE == 4 || p.PROD_DETALHE == 5).ToList();
                    //_texto = _relatorio.MontarDetalhe(_texto, _relatorio, "breakbefore");
                }

                // Detalhes
                _texto = _relatorio.MontarDetalheRiscoCAD(_texto, _relatorio, "breakbefore");

            }
            _texto = _relatorio.MontarRodape(_texto);

            return _texto;
        }
        #endregion

        protected void txtMolde_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtModelagem.Text = "";
                if (txtMolde.Text.Trim() != "")
                {
                    var molde = prodController.ObterMolde(Convert.ToInt32(txtMolde.Text.Trim()));
                    if (molde != null)
                    {
                        txtModelagem.Text = molde.MODELAGEM;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void CarregarFoto(string produto, string cor)
        {
            if (produto != "" && cor != "")
            {
                string fotoPNG = @"/Fotos/" + produto.Trim() + cor.Trim() + ".png";
                string fotoJPG = @"/Fotos/" + produto.Trim() + cor.Trim() + ".jpg";

                if (File.Exists(Server.MapPath(fotoPNG)))
                {
                    imgFotoPeca.ImageUrl = fotoPNG;
                    imgFotoTecido.ImageUrl = fotoPNG;
                }
                else if (File.Exists(Server.MapPath(fotoJPG)))
                {
                    imgFotoPeca.ImageUrl = fotoJPG;
                    imgFotoTecido.ImageUrl = fotoJPG;
                }
                else
                {
                    var desenvProduto = desenvController.ObterProduto(ddlColecoes.SelectedValue.Trim(), produto, cor);
                    if (desenvProduto != null && desenvProduto.FOTO != null)
                    {
                        imgFotoPeca.ImageUrl = desenvProduto.FOTO;
                        imgFotoTecido.ImageUrl = desenvProduto.FOTO;
                    }
                }
            }
        }


    }
}
