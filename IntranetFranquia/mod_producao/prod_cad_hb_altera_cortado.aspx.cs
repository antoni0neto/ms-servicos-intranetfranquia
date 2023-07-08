using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Collections;

namespace Relatorios
{
    public partial class prod_cad_hb_altera_cortado : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int colunaComposicao, colunaAviamento, colunaDetalhe, colunagradesub, colunagradecortehb, colunagradecortehbbaixa = 0;

        List<UNIDADE_MEDIDA> _unidadeMedida = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "")
                    Response.Redirect("prod_menu.aspx");

                bool hbValido = true;
                string codigo = Request.QueryString["c"].ToString();

                hidHB.Value = codigo;

                //Valida HB
                PROD_HB prod_hb = ObterHB(Convert.ToInt32(codigo));
                if (prod_hb == null)
                    hbValido = false;

                if (!hbValido)
                {
                    labConteudoDiv.Text = "NÃO ENCONTRADO";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Ok: function () { window.open('prod_menu.aspx', '_self'); } } }); });", true);
                    return;
                }

                hrefVoltar.HRef = "prod_cad_hb_altera_filtro.aspx?d=2";

                //Carregar Combos
                CarregarGrupo();
                CarregarCores();
                CarregarColecoes();
                CarregarFornecedores();
                CarregarUnidadeMedida();
                CarregarGrupoMaterial("");
                CarregarCores("0");

                if (prod_hb != null)
                {
                    ddlColecoes.SelectedValue = prod_hb.COLECAO;
                    ddlColecoes.Enabled = false;
                    txtHB.Text = prod_hb.HB.ToString();
                    txtHB.ReadOnly = true;
                    ddlGrupo.SelectedValue = (new BaseController().BuscaGrupoProduto(prod_hb.GRUPO)).GRUPO_PRODUTO;
                    ddlGrupo.Enabled = false;
                    txtNome.Text = prod_hb.NOME;
                    txtNome.ReadOnly = true;
                    txtData.Text = prod_hb.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                    txtData.ReadOnly = true;
                    txtGrupoTecido.Text = prod_hb.GRUPO_TECIDO;
                    txtGrupoTecido.ReadOnly = true;
                    txtSubGrupoTecido.Text = prod_hb.TECIDO;
                    txtSubGrupoTecido.ReadOnly = true;
                    ddlCor.SelectedValue = prod_hb.COR;
                    ddlCor.Enabled = false;
                    txtCorFornecedor.Text = prod_hb.COR_FORNECEDOR;
                    txtCorFornecedor.ReadOnly = true;
                    txtFornecedor.Text = prod_hb.FORNECEDOR;
                    txtFornecedor.ReadOnly = true;
                    txtLargura.Text = prod_hb.LARGURA.ToString();
                    txtLargura.ReadOnly = true;
                    cbLiquidar.Checked = (prod_hb.LIQUIDAR == 'S') ? true : false;
                    cbLiquidar.Enabled = false;
                    txtModelagem.Text = prod_hb.MODELAGEM;
                    txtModelagem.ReadOnly = true;
                    txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                    txtCustoTecido.ReadOnly = true;
                    imgFotoTecido.ImageUrl = prod_hb.FOTO_TECIDO;

                    txtCodigoLinx.Text = prod_hb.CODIGO_PRODUTO_LINX.ToString();
                    txtCodigoLinx.ReadOnly = true;

                    txtPCVarejo.Text = (prod_hb.PC_VAREJO != null) ? prod_hb.PC_VAREJO.Trim() : "";
                    txtPCAtacado.Text = (prod_hb.PC_ATACADO != null) ? prod_hb.PC_ATACADO.Trim() : "";

                    txtPedidoNumero.Text = prod_hb.NUMERO_PEDIDO.ToString();
                    txtPedidoNumero.ReadOnly = true;

                    bool custoProdutoSimulado = false;
                    var custoSimulacao = prodController.ObterCustoSimulacao(prod_hb.CODIGO_PRODUTO_LINX, Convert.ToChar(prod_hb.MOSTRUARIO));
                    if (custoSimulacao != null && custoSimulacao.Count() > 0 && custoSimulacao[0].FINALIZADO == 'S')
                        custoProdutoSimulado = true;

                    ddlUnidade.SelectedValue = prod_hb.UNIDADE_MEDIDA.ToString();
                    //ddlUnidade.Enabled = !custoProdutoSimulado;
                    txtGastoPorFolha.Text = Convert.ToDecimal((prod_hb.GASTO_FOLHA / 1)).ToString("###,###,###,##0.000"); // Convert.ToDecimal(1.07))).ToString("###,###,###,##0.000");
                    //txtGastoPorFolha.Enabled = !custoProdutoSimulado;
                    txtGastoRetalhos.Text = prod_hb.RETALHOS.ToString();
                    //txtGastoRetalhos.Enabled = !custoProdutoSimulado;
                    //txtGastoPorPecaCusto.Enabled = !custoProdutoSimulado;
                    if (prod_hb.GASTO_PECA_CUSTO != null)
                        txtGastoPorPecaCusto.Text = prod_hb.GASTO_PECA_CUSTO.ToString();

                    gvComposicao.DataSource = prodController.ObterComposicaoHB(Convert.ToInt32(codigo));
                    gvComposicao.DataBind();

                    CarregarNomeGrade(prod_hb);
                    CarregarGradeCorte(prod_hb);
                    CarregarRota(prod_hb);

                    PROD_HB_GRADE _grade = prodController.ObterGradeHB(Convert.ToInt32(codigo), 2);
                    int totalGrade = 0;
                    if (_grade != null)
                    {
                        txtGradeEXP.Text = _grade.GRADE_EXP.ToString();
                        txtGradeEXP.Enabled = false;
                        txtGradeXP.Text = _grade.GRADE_XP.ToString();
                        txtGradeXP.Enabled = false;
                        txtGradePP.Text = _grade.GRADE_PP.ToString();
                        txtGradePP.Enabled = false;
                        txtGradeP.Text = _grade.GRADE_P.ToString();
                        txtGradeP.Enabled = false;
                        txtGradeM.Text = _grade.GRADE_M.ToString();
                        txtGradeM.Enabled = false;
                        txtGradeG.Text = _grade.GRADE_G.ToString();
                        txtGradeG.Enabled = false;
                        txtGradeGG.Text = _grade.GRADE_GG.ToString();
                        txtGradeGG.Enabled = false;

                        totalGrade = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG));
                        txtGradeTotal.Text = totalGrade.ToString();
                        txtGradeTotal.Enabled = false;
                    }

                    _grade = new PROD_HB_GRADE();
                    _grade = prodController.ObterGradeHB(Convert.ToInt32(codigo), 3);
                    if (_grade != null)
                    {
                        txtGradeEXP_R.Text = _grade.GRADE_EXP.ToString();
                        txtGradeXP_R.Text = _grade.GRADE_XP.ToString();
                        txtGradePP_R.Text = _grade.GRADE_PP.ToString();
                        txtGradeP_R.Text = _grade.GRADE_P.ToString();
                        txtGradeM_R.Text = _grade.GRADE_M.ToString();
                        txtGradeG_R.Text = _grade.GRADE_G.ToString();
                        txtGradeGG_R.Text = _grade.GRADE_GG.ToString();

                        totalGrade = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG));
                        txtGradeTotal_R.Text = totalGrade.ToString();

                        //Grava grade Inicial
                        hidGradeTotal.Value = totalGrade.ToString();
                    }

                    txtGastoPorCorte.Attributes.Add("readonly", "readonly");
                    txtGastoPorPeca.Attributes.Add("readonly", "readonly");
                    txtGradeTotal_R.Attributes.Add("readonly", "readonly");

                    decimal sobraMetro = 0;
                    if (totalGrade > 0)
                    {
                        txtGastoPorCorte.Text = ((Convert.ToDecimal(prod_hb.GASTO_FOLHA) * totalGrade)).ToString("####0.000");
                        txtGastoPorCorte.ReadOnly = true;

                        sobraMetro = 1;
                        txtGastoPorPeca.Text = (((Convert.ToDecimal(txtGastoPorCorte.Text) + Convert.ToDecimal(prod_hb.RETALHOS)) / totalGrade) * sobraMetro).ToString("####0.000");
                        txtGastoPorPeca.ReadOnly = true;
                    }

                    cbAtacado.Checked = (prod_hb.ATACADO == 'S') ? true : false;
                    cbAtacado.Enabled = false;
                    if (cbAtacado.Checked)
                        fsGradeAtacado.Visible = true;

                    _grade = new PROD_HB_GRADE();
                    _grade = prodController.ObterGradeHB(Convert.ToInt32(codigo), 99);
                    if (_grade != null)
                    {
                        int totalGradeAtacado = 0;
                        txtGradeEXP_A.Text = _grade.GRADE_EXP.ToString();
                        txtGradeXP_A.Text = _grade.GRADE_XP.ToString();
                        txtGradePP_A.Text = _grade.GRADE_PP.ToString();
                        txtGradeP_A.Text = _grade.GRADE_P.ToString();
                        txtGradeM_A.Text = _grade.GRADE_M.ToString();
                        txtGradeG_A.Text = _grade.GRADE_G.ToString();
                        txtGradeGG_A.Text = _grade.GRADE_GG.ToString();

                        totalGradeAtacado = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG));
                        txtGradeTotal_A.Text = totalGradeAtacado.ToString();
                    }

                    List<PROD_HB> detalhes;
                    detalhes = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));
                    gvDetalhe.DataSource = detalhes;
                    gvDetalhe.DataBind();

                    foreach (GridViewRow row in gvDetalhe.Rows)
                    {
                        int codigoHB = 0;
                        codigoHB = Convert.ToInt32(gvDetalhe.DataKeys[row.RowIndex].Value);
                        PROD_HB det = prodController.ObterHB(codigoHB);
                        if (det != null)
                        {
                            DropDownList _ddlUnidadeMedida = row.FindControl("ddlUnidadeMedida") as DropDownList;
                            TextBox _txtGastoPorFolha = row.FindControl("txtGastoPorFolha") as TextBox;
                            TextBox _txtGastoRetalhos = row.FindControl("txtGastoRetalhos") as TextBox;
                            TextBox _txtGastoPorPecaCusto = row.FindControl("txtGastoPorPecaCusto") as TextBox;

                            //_ddlUnidadeMedida.Enabled = !custoProdutoSimulado;
                            //_txtGastoPorFolha.Enabled = !custoProdutoSimulado;
                            _txtGastoRetalhos.Enabled = !custoProdutoSimulado;
                            _txtGastoPorPecaCusto.Enabled = !custoProdutoSimulado;

                            _ddlUnidadeMedida.SelectedValue = (det.UNIDADE_MEDIDA != null) ? det.UNIDADE_MEDIDA.ToString() : "";
                            _txtGastoPorFolha.Text = Convert.ToDecimal((det.GASTO_FOLHA / ((det.GASTO_FOLHA < Convert.ToDecimal(0.01)) ? 1 : 1))).ToString("###,###,###,##0.000");// Convert.ToDecimal(1.07)))).ToString("###,###,###,##0.000");
                            _txtGastoRetalhos.Text = det.RETALHOS.ToString();
                            if (det.GASTO_PECA_CUSTO != null)
                                _txtGastoPorPecaCusto.Text = det.GASTO_PECA_CUSTO.ToString();

                            txtGasto_TextChanged(_txtGastoPorFolha, null);
                        }
                    }

                    if (prod_hb.MOSTRUARIO == 'N')
                    {
                        labGastoPorPecaCusto.Visible = false;
                        txtGastoPorPecaCusto.Visible = false;

                        gvDetalhe.Columns[8].Visible = false;
                    }

                    gvAviamento.DataSource = prodController.ObterAviamentoHB(Convert.ToInt32(hidHB.Value));
                    gvAviamento.DataBind();

                    imgFotoPeca.ImageUrl = prod_hb.FOTO_PECA;
                    txtObservacao.Text = prod_hb.OBSERVACAO;

                    RecarregarGradeSub();
                    RecarregarGradeCorteHB();
                    RecarregarGVCorteBaixaDropDownList();

                    var enxaixe = new FaccaoController().ObterSaidaHB(prod_hb.CODIGO.ToString()).Where(p => p.DATA_LIBERACAO != null);
                    if (enxaixe != null && enxaixe.Count() > 0)
                        HabilitarGrade(false);

                    dialogPai.Visible = false;
                    labAviamento.Visible = false;
                }

            }
        }

        private static List<PROD_CORTE_FUNCIONARIO> ObterListaCorteFuncionario()
        {
            return (new ProducaoController().ObterCorteFuncionario());
        }
        [System.Web.Services.WebMethod]
        public static ArrayList CarregarCorteFuncionario()
        {
            ArrayList list = new ArrayList();

            List<PROD_CORTE_FUNCIONARIO> _prodCorte = ObterListaCorteFuncionario();
            if (_prodCorte != null)
            {
                _prodCorte = _prodCorte.Where(p => p.STATUS == 'A').ToList();

                foreach (PROD_CORTE_FUNCIONARIO corteF in _prodCorte)
                {
                    list.Add(new ListItem(corteF.NOME, corteF.CODIGO.ToString()));
                }

                list.Insert(0, new ListItem("", ""));
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
        private void RecarregarGVCorteBaixaDropDownList()
        {
            try
            {

                ArrayList func = CarregarCorteFuncionario();

                ddlEnfesto1.Items.Clear();
                CarregarDropDownList(func, ddlEnfesto1);
                ddlEnfesto2.Items.Clear();
                CarregarDropDownList(func, ddlEnfesto2);
                ddlEnfesto3.Items.Clear();
                CarregarDropDownList(func, ddlEnfesto3);

                ddlCorte1.Items.Clear();
                CarregarDropDownList(func, ddlCorte1);
                ddlCorte2.Items.Clear();
                CarregarDropDownList(func, ddlCorte2);

                ddlAmarrador1.Items.Clear();
                CarregarDropDownList(func, ddlAmarrador1);
                ddlAmarrador2.Items.Clear();
                CarregarDropDownList(func, ddlAmarrador2);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "Selecione" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarCores()
        {
            List<CORES_BASICA> _cores = prodController.ObterCoresBasicas();
            if (_cores != null)
            {
                _cores.Insert(0, new CORES_BASICA { COR = "0", DESC_COR = "Selecione" });
                ddlCor.DataSource = _cores;
                ddlCor.DataBind();
            }
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
            }
        }
        private void CarregarFornecedores()
        {
            /*
            List<SP_OBTER_FORNECEDORESResult> _fornecedores = prodController.ObterFornecedores("TECIDOS");

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new SP_OBTER_FORNECEDORESResult { FORNECEDOR = "Selecione", CGC_CPF = "0", TIPO = "0" });

                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
             * */
        }
        private void CarregarUnidadeMedida()
        {
            _unidadeMedida = prodController.ObterUnidadeMedida();

            _unidadeMedida.Add(new UNIDADE_MEDIDA { CODIGO = 0, DESCRICAO = "Selecione", STATUS = 'A' });

            _unidadeMedida = _unidadeMedida.OrderBy(l => l.CODIGO).ToList();

            if (_unidadeMedida != null)
            {
                ddlUnidade.DataSource = _unidadeMedida;
                ddlUnidade.DataBind();
            }
        }

        private void CarregarGrupoMaterial(string tipo)
        {
            List<MATERIAIS_GRUPO> _matGrupo = new DesenvolvimentoController().ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
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
        }
        private void CarregarCores(string codigoProduto)
        {
            BaseController baseController = new BaseController();
            List<PRODUTO_CORE> produtoCor = baseController.BuscaProdutoCores(codigoProduto);
            if (produtoCor == null || produtoCor.Count <= 0 || codigoProduto == "0")
            {
                List<CORES_BASICA> _cores = prodController.ObterCoresBasicas();

                foreach (CORES_BASICA c in _cores)
                    produtoCor.Add(new PRODUTO_CORE { COR_PRODUTO = c.COR, DESC_COR_PRODUTO = c.DESC_COR });

                produtoCor = produtoCor.OrderBy(p => p.DESC_COR_PRODUTO).ToList();
                produtoCor.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "0", DESC_COR_PRODUTO = "Selecione" });

                ddlCorAviamento.DataSource = produtoCor;
                ddlCorAviamento.DataBind();
            }
            if (produtoCor != null)
            {
                try
                {
                    produtoCor.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "0", DESC_COR_PRODUTO = "Selecione" });
                    ddlCor.SelectedIndex = -1;
                    ddlCor.Items.Clear();
                    ddlCor.ClearSelection();
                    ddlCor.DataSource = produtoCor;
                    ddlCor.DataTextField = "DESC_COR_PRODUTO";
                    ddlCor.DataValueField = "COR_PRODUTO";
                    ddlCor.DataBind();
                }
                catch (Exception)
                {
                }

                if (produtoCor.Count == 2)
                    ddlCor.SelectedValue = produtoCor[1].COR_PRODUTO;

            }
        }
        private void CarregarSubGrupo(string grupo, string tipo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = new DesenvolvimentoController().ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).OrderBy(p => p.SUBGRUPO.Trim()).ToList();
            if (_matSubGrupo != null)
            {
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "Selecione" });

                if (tipo == "A")
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
                if (_ddl.ID == "ddlSubGrupoAviamento")
                    CarregarCorFornecedor(ddlGrupoAviamento.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlCorFornecedorAviamento);
            }
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, DropDownList _ddlCorFornecedor)
        {
            //Obter materiais
            var _material = new DesenvolvimentoController().ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim());
            //Obter cores do material
            var _matCores = new DesenvolvimentoController().ObterMaterialCor();
            _matCores = _matCores.Where(p => _material.Any(c => c.MATERIAL.Trim() == p.MATERIAL.Trim())).OrderBy(x => x.DESC_COR_MATERIAL).ToList();

            List<MATERIAIS_CORE> _cores = new List<MATERIAIS_CORE>();
            MATERIAIS_CORE cor = null;
            foreach (MATERIAIS_CORE c in _matCores)
            {
                cor = new MATERIAIS_CORE();
                cor.REFER_FABRICANTE = c.REFER_FABRICANTE.Trim();
                cor.DESC_COR_MATERIAL = c.DESC_COR_MATERIAL.Trim();
                _cores.Add(cor);
            }

            _cores = _cores.OrderBy(p => p.REFER_FABRICANTE.Trim()).ToList();
            _cores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "Selecione" });

            if (_cores != null && _cores.Count() <= 1)
                _cores.Add(new MATERIAIS_CORE { REFER_FABRICANTE = "OUTROS" });

            _ddlCorFornecedor.DataSource = null;
            _ddlCorFornecedor.DataBind();

            _ddlCorFornecedor.DataSource = _cores;
            _ddlCorFornecedor.DataBind();
        }

        private void CarregarNomeGrade(PROD_HB prod_hb)
        {
            var _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(prod_hb.PROD_GRADE));
            if (_gradeNome != null)
            {
                labGradeEXP.Text = _gradeNome.GRADE_EXP;
                labGradeXP.Text = _gradeNome.GRADE_XP;
                labGradePP.Text = _gradeNome.GRADE_PP;
                labGradeP.Text = _gradeNome.GRADE_P;
                labGradeM.Text = _gradeNome.GRADE_M;
                labGradeG.Text = _gradeNome.GRADE_G;
                labGradeGG.Text = _gradeNome.GRADE_GG;

                labGradeEXP_A.Text = _gradeNome.GRADE_EXP;
                labGradeXP_A.Text = _gradeNome.GRADE_XP;
                labGradePP_A.Text = _gradeNome.GRADE_PP;
                labGradeP_A.Text = _gradeNome.GRADE_P;
                labGradeM_A.Text = _gradeNome.GRADE_M;
                labGradeG_A.Text = _gradeNome.GRADE_G;
                labGradeGG_A.Text = _gradeNome.GRADE_GG;


                txtGradeP.Enabled = (_gradeNome.GRADE_P != "-") ? true : false;
                txtGradeM.Enabled = (_gradeNome.GRADE_M != "-") ? true : false;
                txtGradeG.Enabled = (_gradeNome.GRADE_G != "-") ? true : false;
                txtGradeGG.Enabled = (_gradeNome.GRADE_GG != "-") ? true : false;
                txtGradeP_R.Enabled = (_gradeNome.GRADE_P != "-") ? true : false;
                txtGradeM_R.Enabled = (_gradeNome.GRADE_M != "-") ? true : false;
                txtGradeG_R.Enabled = (_gradeNome.GRADE_G != "-") ? true : false;
                txtGradeGG_R.Enabled = (_gradeNome.GRADE_GG != "-") ? true : false;
                txtGradeP_A.Enabled = (_gradeNome.GRADE_P != "-") ? true : false;
                txtGradeM_A.Enabled = (_gradeNome.GRADE_M != "-") ? true : false;
                txtGradeG_A.Enabled = (_gradeNome.GRADE_G != "-") ? true : false;
                txtGradeGG_A.Enabled = (_gradeNome.GRADE_GG != "-") ? true : false;
            }
        }
        private void CarregarGradeCorte(PROD_HB prod_hb)
        {
            var _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(prod_hb.PROD_GRADE));
            if (_gradeNome != null)
            {
                ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_EXP, Text = _gradeNome.GRADE_EXP });
                ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_XP, Text = _gradeNome.GRADE_XP });
                ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_PP, Text = _gradeNome.GRADE_PP });
                if (_gradeNome.GRADE_P != "-")
                    ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_P, Text = _gradeNome.GRADE_P });
                if (_gradeNome.GRADE_M != "-")
                    ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_M, Text = _gradeNome.GRADE_M });
                if (_gradeNome.GRADE_G != "-")
                    ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_G, Text = _gradeNome.GRADE_G });
                if (_gradeNome.GRADE_GG != "-")
                    ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_GG, Text = _gradeNome.GRADE_GG });
                ddlGradeNumero.DataBind();
            }

        }
        private void CarregarRota(PROD_HB prod_hb)
        {
            var rota = prodController.ObterRotaHB(prod_hb.CODIGO);
            if (rota != null)
            {
                cbFaccao.Checked = rota.FACCAO;
                cbAcabamento.Checked = rota.ACABAMENTO;
                cbLavanderia.Checked = rota.LAVANDERIA;
                cbEstamparia.Checked = rota.ESTAMPARIA;
            }
        }
        #endregion

        #region "COMPOSICAO"
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
                    }
                }
            }
        }
        #endregion

        #region "DETALHE"
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

                        DropDownList _ddlUnidadeMedida = e.Row.FindControl("ddlUnidadeMedida") as DropDownList;
                        if (_ddlUnidadeMedida != null)
                        {
                            _ddlUnidadeMedida.DataSource = _unidadeMedida;
                            _ddlUnidadeMedida.DataBind();
                        }
                    }
                }
            }
        }
        private void AtualizarGradeDetalhe(int codigo, int processo)
        {
            PROD_HB_GRADE _grade = null;

            //Atualizar grade
            if (codigo > 0)
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(codigo);
                _grade.PROD_PROCESSO = processo; //GRADE DO CORTE
                _grade.GRADE_EXP = (txtGradeEXP_R.Text != "") ? Convert.ToInt32(txtGradeEXP_R.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP_R.Text != "") ? Convert.ToInt32(txtGradeXP_R.Text) : 0;
                _grade.GRADE_PP = (txtGradePP_R.Text != "") ? Convert.ToInt32(txtGradePP_R.Text) : 0;
                _grade.GRADE_P = (txtGradeP_R.Text != "") ? Convert.ToInt32(txtGradeP_R.Text) : 0;
                _grade.GRADE_M = (txtGradeM_R.Text != "") ? Convert.ToInt32(txtGradeM_R.Text) : 0;
                _grade.GRADE_G = (txtGradeG_R.Text != "") ? Convert.ToInt32(txtGradeG_R.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG_R.Text != "") ? Convert.ToInt32(txtGradeGG_R.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, processo);
            }
        }
        #endregion

        #region "AVIAMENTOS"
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
        protected void btAviamentoIncluir_Click(object sender, EventArgs e)
        {
            labErroAviamento.Text = "";

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
            if (ddlCorAviamento.SelectedValue.Trim() == "0")
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

            try
            {
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
                ddlCorAviamento.SelectedValue = "0";
                ddlCorFornecedorAviamento.SelectedValue = "Selecione";
                txtAviamentoQtde.Text = "";
                txtAviamentoQtdePorPeca.Text = "";
                ddlEtiquetaComposicaoAviamento.SelectedValue = "";

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

                        Label _labQtde = e.Row.FindControl("labQtde") as Label;
                        if (_labQtde != null)
                            _labQtde.Text = (((_aviamento.QTDE_EXTRA == null) ? 0 : _aviamento.QTDE_EXTRA) + _aviamento.QTDE).ToString().Replace(",000", "");

                        Label _labQtdePorPeca = e.Row.FindControl("labQtdePorPeca") as Label;
                        if (_labQtdePorPeca != null)
                            _labQtdePorPeca.Text = _aviamento.QTDE_POR_PECA.ToString().Replace(",000", "");

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

        #region "GRADE SUB"
        private void IncluirGradeSub(PROD_HB_GRADE_SUB _grade_sub)
        {
            prodController.InserirGradeSub(_grade_sub);
        }
        private void ExcluirGradeSub(int _Codigo)
        {
            prodController.ExcluirGradeSub(_Codigo);
        }
        private void RecarregarGradeSub()
        {
            List<PROD_HB_GRADE_SUB> _list = new List<PROD_HB_GRADE_SUB>();
            _list = prodController.ObterGradeSubHB(Convert.ToInt32(hidHB.Value));
            if (_list != null)
            {
                gvGradeSub.DataSource = _list;
                gvGradeSub.DataBind();
            }
        }
        protected void btGradeSubIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                PROD_HB_GRADE_SUB _novo = new PROD_HB_GRADE_SUB();

                _novo.GRADE = ddlGradeNumero.SelectedValue;
                _novo.LETRA = ddlGradeLetra.SelectedValue;
                _novo.NUMERO = txtGradeNumeroDetalhe.Text.Trim();
                _novo.PROD_HB = Convert.ToInt32(hidHB.Value);

                IncluirGradeSub(_novo);

                RecarregarGradeSub();

                ddlGradeLetra.SelectedValue = "";
                txtGradeNumeroDetalhe.Text = "";
            }
            catch (Exception)
            {
            }
        }
        protected void btGradeSubExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    ExcluirGradeSub(Convert.ToInt32(b.CommandArgument));
                    RecarregarGradeSub();
                }
                catch (Exception ex)
                {
                    //labErroComposicao.Text = "ERRO (btComposicaoExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void gvGradeSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_GRADE_SUB _grade_hb = e.Row.DataItem as PROD_HB_GRADE_SUB;

                    colunagradesub += 1;
                    if (_grade_hb != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunagradesub.ToString();

                        Button _btExcluir = e.Row.FindControl("btGradeSubExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _grade_hb.CODIGO.ToString();
                        }

                    }
                }
            }
        }
        #endregion

        #region "GRADE CORTE"
        private int IncluirGradeCorte(PROD_HB_CORTE _corte)
        {
            return prodController.InserirGradeCorte(_corte);
        }
        private void AtualizarGradeCorte(PROD_HB_CORTE _corte)
        {
            prodController.AtualizarGradeCorte(_corte);
        }
        private void ExcluirGradeCorte(int _Codigo)
        {
            prodController.ExcluirGradeCorte(_Codigo);
        }
        private void IncluirGradeCorteSub(PROD_HB_CORTE_GRADE_SUB _corteSub)
        {
            prodController.InserirGradeCorteSub(_corteSub);
        }
        private void ExcluirGradeCorteSub(int _Codigo)
        {
            prodController.ExcluirGradeCorteSub(_Codigo);
        }
        private void RecarregarGradeCorteHB()
        {
            List<SP_OBTER_GRADE_CORTEResult> _list = prodController.ObterGradeCorteHB(Convert.ToInt32(hidHB.Value));
            if (_list != null)
            {
                gvGradeCorteHB.DataSource = _list;
                gvGradeCorteHB.DataBind();

                //gvGradeCorteHBBaixa.DataSource = _list;
                //gvGradeCorteHBBaixa.DataBind();
            }
        }
        private void ExcluirGradeCorteHB(int _Codigo)
        {
            prodController.ExcluirGradeCorteHB(_Codigo);
        }
        protected void btIncluirCorte_Click(object sender, EventArgs e)
        {
            int codigoCorte = 0;

            labErroGradeSub.Text = "";
            if (txtCorteGasto.Text.Trim() == "")
            {
                labErroGradeSub.Text = "Informe o Gasto do Corte.";
                return;
            }
            if (gvGradeSub.Rows.Count <= 0)
            {
                labErroGradeSub.Text = "Informe pelo menos uma Grade.";
                return;
            }
            if (txtCorteFolha.Text.Trim() == "" || Convert.ToDecimal(txtCorteFolha.Text) <= 0)
            {
                labErroGradeSub.Text = "Informe o número de Folhas.";
                return;
            }
            if (txtRiscoNome.Text.Trim() == "")
            {
                labErroGradeSub.Text = "Informe o Nome do Risco.";
                return;
            }
            /*
            if (ddlEnfesto1.SelectedValue.Trim() == "")
            {
                labErroGradeSub.Text = "Informe pelo menos um Enfestador.";
                return;
            }
            if (ddlCorte1.SelectedValue.Trim() == "")
            {
                labErroGradeSub.Text = "Informe pelo menos um Cortador.";
                return;
            }
            if (ddlAmarrador1.SelectedValue.Trim() == "")
            {
                labErroGradeSub.Text = "Informe pelo menos um Amarrador.";
                return;
            }*/

            try
            {
                PROD_HB_CORTE _corte = new PROD_HB_CORTE();
                _corte.PROD_HB = Convert.ToInt32(hidHB.Value);
                _corte.GASTO = Convert.ToDecimal(txtCorteGasto.Text.Trim());
                _corte.FOLHA = Convert.ToInt32(txtCorteFolha.Text.Trim());
                _corte.RISCO_NOME = txtRiscoNome.Text.Trim().ToUpper();

                _corte.ENFESTO = ddlEnfesto1.SelectedItem.Text;
                _corte.ENFESTO = _corte.ENFESTO + ((ddlEnfesto2.SelectedItem.Text == "") ? "" : ("/" + ddlEnfesto2.SelectedItem.Text));
                _corte.ENFESTO = _corte.ENFESTO + ((ddlEnfesto3.SelectedItem.Text == "") ? "" : ("/" + ddlEnfesto3.SelectedItem.Text));


                _corte.CORTE = ddlCorte1.SelectedItem.Text;
                _corte.CORTE = _corte.CORTE + ((ddlCorte2.SelectedItem.Text == "") ? "" : ("/" + ddlCorte2.SelectedItem.Text));

                _corte.AMARRADOR = ddlAmarrador1.SelectedItem.Text;
                _corte.AMARRADOR = _corte.AMARRADOR + ((ddlAmarrador2.SelectedItem.Text == "") ? "" : ("/" + ddlAmarrador2.SelectedItem.Text));

                codigoCorte = IncluirGradeCorte(_corte);

                List<PROD_HB_GRADE_SUB> _list = new List<PROD_HB_GRADE_SUB>();
                _list = prodController.ObterGradeSubHB(Convert.ToInt32(hidHB.Value));

                PROD_HB_CORTE_GRADE_SUB _corteSub = null;
                foreach (PROD_HB_GRADE_SUB sub in _list)
                {
                    _corteSub = new PROD_HB_CORTE_GRADE_SUB();
                    _corteSub.PROD_HB_CORTE = codigoCorte;
                    _corteSub.PROD_HB_GRADE_SUB = sub.CODIGO;
                    IncluirGradeCorteSub(_corteSub);
                }

                txtCorteGasto.Text = "";
                txtCorteFolha.Text = "";
                txtRiscoNome.Text = "";
                ddlEnfesto1.SelectedValue = "";
                ddlEnfesto2.SelectedValue = "";
                ddlEnfesto3.SelectedValue = "";

                ddlCorte1.SelectedValue = "";
                ddlCorte2.SelectedValue = "";

                ddlAmarrador1.SelectedValue = "";
                ddlAmarrador2.SelectedValue = "";

                RecarregarGradeSub();
                RecarregarGradeCorteHB();
                PreencherGradeReal(Convert.ToInt32(hidHB.Value));
            }
            catch (Exception ex)
            {
                labErroGradeSub.Text = "ERRO (btIncluirCorte_Click): " + ex.Message;
            }

        }
        protected void btGradeCorteHBExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    ExcluirGradeCorteHB(Convert.ToInt32(b.CommandArgument));
                    RecarregarGradeCorteHB();
                    PreencherGradeReal(Convert.ToInt32(hidHB.Value));
                }
                catch (Exception ex)
                {
                    labErroGradeSub.Text = "ERRO (btComposicaoExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        protected void gvGradeCorteHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_GRADE_CORTEResult _gradeCorte = e.Row.DataItem as SP_OBTER_GRADE_CORTEResult;

                    colunagradecortehb += 1;
                    if (_gradeCorte != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunagradecortehb.ToString();

                        Button _btExcluir = e.Row.FindControl("btGradeCorteHBExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _gradeCorte.PROD_HB_CORTE.ToString();
                        }
                    }
                }
            }
        }
        protected void gvGradeCorteHBBaixa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_GRADE_CORTEResult _gradeCorte = e.Row.DataItem as SP_OBTER_GRADE_CORTEResult;

                    colunagradecortehbbaixa += 1;
                    if (_gradeCorte != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunagradecortehbbaixa.ToString();

                        TextBox _folha = e.Row.FindControl("txtFolha") as TextBox;
                        if (_folha != null)
                            _folha.Text = _gradeCorte.FOLHA.ToString();

                        List<PROD_CORTE_FUNCIONARIO> _corteFunc = prodController.ObterCorteFuncionario();

                        DropDownList _ddlEnfesto1 = e.Row.FindControl("ddlEnfesto1") as DropDownList;
                        if (_ddlEnfesto1 != null)
                        {
                            _ddlEnfesto1.DataSource = _corteFunc;
                            _ddlEnfesto1.DataBind();
                        }
                        DropDownList _ddlEnfesto2 = e.Row.FindControl("ddlEnfesto2") as DropDownList;
                        if (_ddlEnfesto2 != null)
                        {
                            _ddlEnfesto2.DataSource = _corteFunc;
                            _ddlEnfesto2.DataBind();
                        }
                        DropDownList _ddlEnfesto3 = e.Row.FindControl("ddlEnfesto3") as DropDownList;
                        if (_ddlEnfesto3 != null)
                        {
                            _ddlEnfesto3.DataSource = _corteFunc;
                            _ddlEnfesto3.DataBind();
                        }

                        DropDownList _ddlCorte1 = e.Row.FindControl("ddlCorte1") as DropDownList;
                        if (_ddlCorte1 != null)
                        {
                            _ddlCorte1.DataSource = _corteFunc;
                            _ddlCorte1.DataBind();
                        }
                        DropDownList _ddlCorte2 = e.Row.FindControl("ddlCorte2") as DropDownList;
                        if (_ddlCorte2 != null)
                        {
                            _ddlCorte2.DataSource = _corteFunc;
                            _ddlCorte2.DataBind();
                        }

                        DropDownList _ddlAmarrador1 = e.Row.FindControl("ddlAmarrador1") as DropDownList;
                        if (_ddlAmarrador1 != null)
                        {
                            _ddlAmarrador1.DataSource = _corteFunc;
                            _ddlAmarrador1.DataBind();
                        }
                        DropDownList _ddlAmarrador2 = e.Row.FindControl("ddlAmarrador2") as DropDownList;
                        if (_ddlAmarrador2 != null)
                        {
                            _ddlAmarrador2.DataSource = _corteFunc;
                            _ddlAmarrador2.DataBind();
                        }

                        Button _btExcluir = e.Row.FindControl("btGradeCorteHBExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _gradeCorte.PROD_HB_CORTE.ToString();
                        }

                    }
                }
            }
        }
        #endregion

        #region "HB"
        private void HabilitarGrade(bool encaixado)
        {
            txtGradeEXP.Enabled = encaixado;
            txtGradeEXP_R.Enabled = encaixado;
            txtGradeEXP_A.Enabled = encaixado;

            txtGradeXP.Enabled = encaixado;
            txtGradeXP_R.Enabled = encaixado;
            txtGradeXP_A.Enabled = encaixado;

            txtGradePP.Enabled = encaixado;
            txtGradePP_R.Enabled = encaixado;
            txtGradePP_A.Enabled = encaixado;

            txtGradeP.Enabled = encaixado;
            txtGradeP_R.Enabled = encaixado;
            txtGradeP_A.Enabled = encaixado;

            txtGradeM.Enabled = encaixado;
            txtGradeM_R.Enabled = encaixado;
            txtGradeM_A.Enabled = encaixado;

            txtGradeG.Enabled = encaixado;
            txtGradeG_R.Enabled = encaixado;
            txtGradeG_A.Enabled = encaixado;

            txtGradeGG.Enabled = encaixado;
            txtGradeGG_R.Enabled = encaixado;
            txtGradeGG_A.Enabled = encaixado;

            txtGradeTotal.Enabled = encaixado;
            txtGradeTotal_R.Enabled = encaixado;
            txtGradeTotal_A.Enabled = encaixado;
        }
        private void PreencherGradeReal(int prod_hb)
        {
            var gradeReal = prodController.CalcularGradeReal(prod_hb);
            if (gradeReal != null)
            {
                //Preenche grade real
                txtGradeEXP_R.Text = gradeReal.GRADE_EXP.ToString();
                txtGradeXP_R.Text = gradeReal.GRADE_XP.ToString();
                txtGradePP_R.Text = gradeReal.GRADE_PP.ToString();
                txtGradeP_R.Text = gradeReal.GRADE_P.ToString();
                txtGradeM_R.Text = gradeReal.GRADE_M.ToString();
                txtGradeG_R.Text = gradeReal.GRADE_G.ToString();
                txtGradeGG_R.Text = gradeReal.GRADE_GG.ToString();
                int totalGrade = (Convert.ToInt32(gradeReal.GRADE_EXP) + Convert.ToInt32(gradeReal.GRADE_XP) + Convert.ToInt32(gradeReal.GRADE_PP) + Convert.ToInt32(gradeReal.GRADE_P) + Convert.ToInt32(gradeReal.GRADE_M) + Convert.ToInt32(gradeReal.GRADE_G) + Convert.ToInt32(gradeReal.GRADE_GG));
                txtGradeTotal_R.Text = totalGrade.ToString();
            }
        }
        private PROD_HB ObterHB(int codigo)
        {
            return prodController.ObterHB(codigo);
        }
        private void AtualizarHB(PROD_HB _hb)
        {
            prodController.AtualizarHB(_hb);
        }
        private void AtualizarAviamentoHB(PROD_HB_PROD_AVIAMENTO _aviamento)
        {
            prodController.AtualizarAviamentoHB(_aviamento);
        }
        private void AtualizarBaixaHB(PROD_HB _hb)
        {
            prodController.AtualizarBaixaHB(_hb);
        }
        private void AtualizarPedidoCompra(PROD_HB _hb)
        {
            prodController.AtualizarPedidoCompra(_hb);
        }
        private void AtualizarProcessoDataFim(int _prod_hb, int _processo, DateTime _dataBaixa)
        {
            prodController.AtualizarProcessoDataFim(_prod_hb, _processo, _dataBaixa);
        }
        private void AtualizarGrade(PROD_HB_GRADE _grade, int _processo)
        {
            prodController.AtualizarGrade(_grade, _processo);
        }
        private void IncluirProcesso(PROD_HB_PROD_PROCESSO _processo)
        {
            prodController.InserirProcesso(_processo);
        }
        private int ObterProcessoOrdem(int _ordem)
        {
            return prodController.ObterProcessoOrdem(_ordem).CODIGO;
        }
        protected void txtGasto_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                GridViewRow row = (GridViewRow)txt.NamingContainer;
                if (row != null)
                {
                    DropDownList _ddlUnidadeMedida = row.FindControl("ddlUnidadeMedida") as DropDownList;
                    TextBox _txtPorFolha = row.FindControl("txtGastoPorFolha") as TextBox;
                    TextBox _txtRetalho = row.FindControl("txtGastoRetalhos") as TextBox;
                    TextBox _txtPorCorte = row.FindControl("txtGastoPorCorte") as TextBox;
                    TextBox _txtPorPeca = row.FindControl("txtGastoPorPeca") as TextBox;

                    if (_ddlUnidadeMedida.SelectedValue != "0" && _txtPorFolha.Text.Trim() != "" && _txtRetalho.Text.Trim() != "")
                    {
                        if (txtGradeTotal_R.Text != "")
                        {
                            int totalGrade = Convert.ToInt32(txtGradeTotal_R.Text);
                            if (totalGrade > 0)
                            {
                                decimal sobraMetro = 1;
                                _txtPorCorte.Text = ((Convert.ToDecimal(_txtPorFolha.Text) * totalGrade)).ToString("####0.000");
                                _txtPorCorte.ReadOnly = true;
                                _txtPorPeca.Text = (((Convert.ToDecimal(_txtPorCorte.Text) + Convert.ToDecimal(_txtRetalho.Text)) / totalGrade) * sobraMetro).ToString("####0.000");
                                _txtPorPeca.ReadOnly = true;
                            }
                        }
                    }
                }
            }
        }
        private bool AtualizarCorte()
        {
            //Validação de nulos
            if (!ValidarCampos())
            {
                throw new Exception("Preencha corretamente os campos em <strong>vermelho</strong>.");
            }

            //Atualizar grade
            PROD_HB_GRADE _grade = new PROD_HB_GRADE();
            _grade.PROD_HB = Convert.ToInt32(hidHB.Value);
            _grade.PROD_PROCESSO = 3; //GRAVA GRADE 3 - CORTE
            _grade.GRADE_EXP = (txtGradeEXP_R.Text != "") ? Convert.ToInt32(txtGradeEXP_R.Text) : 0;
            _grade.GRADE_XP = (txtGradeXP_R.Text != "") ? Convert.ToInt32(txtGradeXP_R.Text) : 0;
            _grade.GRADE_PP = (txtGradePP_R.Text != "") ? Convert.ToInt32(txtGradePP_R.Text) : 0;
            _grade.GRADE_P = (txtGradeP_R.Text != "") ? Convert.ToInt32(txtGradeP_R.Text) : 0;
            _grade.GRADE_M = (txtGradeM_R.Text != "") ? Convert.ToInt32(txtGradeM_R.Text) : 0;
            _grade.GRADE_G = (txtGradeG_R.Text != "") ? Convert.ToInt32(txtGradeG_R.Text) : 0;
            _grade.GRADE_GG = (txtGradeGG_R.Text != "") ? Convert.ToInt32(txtGradeGG_R.Text) : 0;
            _grade.DATA_INCLUSAO = DateTime.Now;
            _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            AtualizarGrade(_grade, 3);

            //Atualizar grade atacado
            if (cbAtacado.Checked)
            {
                bool _insert = false;
                _grade = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 99);
                if (_grade == null)
                {
                    _grade = new PROD_HB_GRADE();
                    _insert = true;
                }
                _grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                _grade.PROD_PROCESSO = 99; //GRAVA GRADE 99 - ATACADO
                _grade.GRADE_EXP = (txtGradeEXP_A.Text != "") ? Convert.ToInt32(txtGradeEXP_A.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP_A.Text != "") ? Convert.ToInt32(txtGradeXP_A.Text) : 0;
                _grade.GRADE_PP = (txtGradePP_A.Text != "") ? Convert.ToInt32(txtGradePP_A.Text) : 0;
                _grade.GRADE_P = (txtGradeP_A.Text != "") ? Convert.ToInt32(txtGradeP_A.Text) : 0;
                _grade.GRADE_M = (txtGradeM_A.Text != "") ? Convert.ToInt32(txtGradeM_A.Text) : 0;
                _grade.GRADE_G = (txtGradeG_A.Text != "") ? Convert.ToInt32(txtGradeG_A.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG_A.Text != "") ? Convert.ToInt32(txtGradeGG_A.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                if (_insert)
                    prodController.InserirGrade(_grade);
                else
                    AtualizarGrade(_grade, 99);
            }

            var _prod_hb = new PROD_HB();
            _prod_hb.CODIGO = Convert.ToInt32(hidHB.Value);
            _prod_hb.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);
            _prod_hb.GASTO_FOLHA = Convert.ToDecimal(txtGastoPorFolha.Text) * 1; //Convert.ToDecimal(1.07);
            if (txtGastoPorPecaCusto.Text != "")
                _prod_hb.GASTO_PECA_CUSTO = Convert.ToDecimal(txtGastoPorPecaCusto.Text);
            _prod_hb.RETALHOS = Convert.ToDecimal(txtGastoRetalhos.Text);
            _prod_hb.STATUS = 'B'; //BAIXADO
            AtualizarBaixaHB(_prod_hb);

            _prod_hb = new PROD_HB();
            _prod_hb.CODIGO = Convert.ToInt32(hidHB.Value);
            _prod_hb.PC_VAREJO = txtPCVarejo.Text.Trim();
            _prod_hb.PC_ATACADO = txtPCAtacado.Text.Trim();

            AtualizarPedidoCompra(_prod_hb);

            AtualizarRota(_prod_hb);

            int codigoHB = 0;
            foreach (GridViewRow row in gvDetalhe.Rows)
            {
                codigoHB = Convert.ToInt32(gvDetalhe.DataKeys[row.RowIndex].Value);

                PROD_HB det = prodController.ObterHB(codigoHB);
                if (det != null)
                {
                    //Atualizar grade
                    AtualizarGradeDetalhe(det.CODIGO, 2); //Risco
                    AtualizarGradeDetalhe(det.CODIGO, 3); //Corte

                    if (det.STATUS == 'B')
                    {
                        DropDownList _ddlUnidadeMedida = row.FindControl("ddlUnidadeMedida") as DropDownList;
                        TextBox _txtPorFolha = row.FindControl("txtGastoPorFolha") as TextBox;
                        TextBox _txtRetalho = row.FindControl("txtGastoRetalhos") as TextBox;
                        TextBox _txtGastoPorPecaCusto = row.FindControl("txtGastoPorPecaCusto") as TextBox;

                        if (_ddlUnidadeMedida.SelectedValue != "0" && _txtPorFolha.Text.Trim() != "" && _txtRetalho.Text.Trim() != "")
                        {
                            PROD_HB prod_hb_det = new PROD_HB();
                            prod_hb_det.CODIGO = det.CODIGO;
                            prod_hb_det.UNIDADE_MEDIDA = Convert.ToInt32(_ddlUnidadeMedida.SelectedValue);
                            prod_hb_det.GASTO_FOLHA = Convert.ToDecimal(_txtPorFolha.Text) * 1; //Convert.ToDecimal(1.07);
                            if (_txtGastoPorPecaCusto != null && _txtGastoPorPecaCusto.Text.Trim() != "")
                                prod_hb_det.GASTO_PECA_CUSTO = Convert.ToDecimal(_txtGastoPorPecaCusto.Text);
                            prod_hb_det.RETALHOS = Convert.ToDecimal(_txtRetalho.Text);
                            prod_hb_det.STATUS = det.STATUS;
                            AtualizarBaixaHB(prod_hb_det);
                        }
                    }
                }
            }

            return true;
        }

        private void AtualizarRota(PROD_HB prod_hb)
        {
            var rota = prodController.ObterRotaHB(prod_hb.CODIGO);
            if (rota != null)
            {
                rota.FACCAO = cbFaccao.Checked;
                rota.ESTAMPARIA = cbEstamparia.Checked;
                rota.LAVANDERIA = cbLavanderia.Checked;
                rota.ACABAMENTO = cbAcabamento.Checked;
                rota.DATA_INCLUSAO = DateTime.Now;

                prodController.AtualizarRota(rota);
            }
        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            int totalGradeAnterior = 0;

            try
            {
                AtualizarCorte();

                totalGradeAnterior = (hidGradeTotal.Value.Trim() == "") ? 0 : Convert.ToInt32(hidGradeTotal.Value);

                int gexp = (txtGradeEXP_R.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeEXP_R.Text.Trim());
                int gxp = (txtGradeXP_R.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeXP_R.Text.Trim());
                int gpp = (txtGradePP_R.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradePP_R.Text.Trim());
                int gp = (txtGradeP_R.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeP_R.Text.Trim());
                int gm = (txtGradeM_R.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeM_R.Text.Trim());
                int gg = (txtGradeG_R.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeG_R.Text.Trim());
                int ggg = (txtGradeGG_R.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeGG_R.Text.Trim());

                int totalGrade = gexp + gxp + gpp + gp + gm + gg + ggg;

                if (totalGradeAnterior != totalGrade)
                    labAviamento.Visible = true;

                labErroEnvio.Text = "<strong>OK</strong>";
                labHBPopUp.Text = txtHB.Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, Risco: function () { window.open('prod_fila_risco.aspx', '_self'); }, Corte: function () { window.open('prod_fila_corte.aspx', '_self'); }, 'Rel. Aviamento': function () { window.open('prod_rel_aviamento.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErroEnvio.Text = "Erro (btEnviar_Click): " + ex.Message;
            }
        }
        #endregion

        #region "OUTROS"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labHB.ForeColor = _OK;
            if (txtHB.Text == "" || Convert.ToInt32(txtHB.Text) <= 0)
            {
                labHB.ForeColor = _notOK;
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

            labGrupoTecido.ForeColor = _OK;
            if (txtGrupoTecido.Text.Trim() == "")
            {
                labGrupoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupoTecido.ForeColor = _OK;
            if (txtSubGrupoTecido.Text.Trim() == "")
            {
                labSubGrupoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue == "" || ddlCor.SelectedValue == "0" || ddlCor.SelectedValue == "Selecione")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (txtFornecedor.Text.Trim() == "")
            {
                labFornecedor.ForeColor = _notOK;
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

            labFotoTecido.ForeColor = _OK;
            if (imgFotoTecido.ImageUrl == "")
            {
                labFotoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labComposicaoTecido.ForeColor = _OK;
            if (gvComposicao.Rows.Count <= 0)
            {
                labComposicaoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labGrade.ForeColor = _OK;
            labGradeReal.ForeColor = _OK;
            if (ValidarGradeReal() <= 0)
            {
                labGrade.ForeColor = _notOK;
                labGradeReal.ForeColor = _notOK;
                retorno = false;
            }

            labGastoUnidade.ForeColor = _OK;
            if (ddlUnidade.SelectedValue == "" || ddlUnidade.SelectedValue == "0" || ddlUnidade.SelectedValue == "Selecione")
            {
                labGastoUnidade.ForeColor = _notOK;
                retorno = false;
            }

            labGastoPorFolha.ForeColor = _OK;
            if (txtGastoPorFolha.Text.Trim() == "" || Convert.ToDecimal(txtGastoPorFolha.Text.Trim()) <= 0)
            {
                labGastoPorFolha.ForeColor = _notOK;
                retorno = false;
            }

            labGastoRetalhos.ForeColor = _OK;
            if (txtGastoRetalhos.Text.Trim() == "" || Convert.ToDecimal(txtGastoRetalhos.Text.Trim()) < 0)
            {
                labGastoRetalhos.ForeColor = _notOK;
                retorno = false;
            }

            labFotoPeca.ForeColor = _OK;
            if (imgFotoPeca.ImageUrl == "")
            {
                labFotoPeca.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private int ValidarGradeReal()
        {
            string _exp = txtGradeEXP_R.Text.Trim();
            string _xp = txtGradeXP_R.Text.Trim();
            string _pp = txtGradePP_R.Text.Trim();
            string _p = txtGradeP_R.Text.Trim();
            string _m = txtGradeM_R.Text.Trim();
            string _g = txtGradeG_R.Text.Trim();
            string _gg = txtGradeGG_R.Text.Trim();

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
        /*
        private int ValidarGridCorte()
        {
            int retorno = 1;
            foreach (GridViewRow r in gvGradeCorteHBBaixa.Rows)
            {
                TextBox _folha = r.FindControl("txtFolha") as TextBox;
                if (_folha == null || _folha.Text.Trim() == "")
                    retorno = 0;

                DropDownList _enfesto1 = r.FindControl("ddlEnfesto1") as DropDownList;
                if (_enfesto1 == null || _enfesto1.SelectedValue.Trim() == "" || _enfesto1.SelectedValue.Trim() == "0")
                    retorno = 0;

                DropDownList _corte1 = r.FindControl("ddlCorte1") as DropDownList;
                if (_corte1 == null || _corte1.SelectedValue.Trim() == "" || _corte1.SelectedValue.Trim() == "0")
                    retorno = 0;

                DropDownList _amarrador1 = r.FindControl("ddlAmarrador1") as DropDownList;
                if (_amarrador1 == null || _amarrador1.SelectedValue.Trim() == "" || _amarrador1.SelectedValue.Trim() == "0")
                    retorno = 0;
            }

            return retorno;
        }
        */
        #endregion

    }
}
