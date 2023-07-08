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

namespace Relatorios
{
    public partial class prod_con_hb : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int colunaComposicao, colunaAviamento, colunaDetalhe, colunagradecortehb = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "")
                    Response.Redirect("prod_menu.aspx");

                string tela = Request.QueryString["t"].ToString();
                hrefVoltar.HRef = "prod_con_hb_filtro.aspx?d=1&t=" + tela + "";

                bool hbValido = true;
                string codigo = Request.QueryString["c"].ToString();

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

                labDataInicial.Text = prod_hb.DATA_INCLUSAO.ToString("dd/MM/yyyy HH:mm:ss");

                PROD_HB_PROD_PROCESSO _processo = null;
                _processo = prodController.ObterProcessoHB(Convert.ToInt32(codigo), 3); //CORTE
                if (_processo == null)
                    _processo = prodController.ObterProcessoHB(Convert.ToInt32(codigo), 2); //RISCO
                if (_processo == null)
                    _processo = prodController.ObterProcessoHB(Convert.ToInt32(codigo), 1); //AMPLIAÇÂO

                if (_processo != null)
                {
                    DateTime? dataFim = _processo.DATA_FIM;
                    if (dataFim != null)
                        labDataFinal.Text = Convert.ToDateTime(dataFim).ToString("dd/MM/yyyy HH:mm:ss");
                }

                //Carregar Combos
                CarregarGrupo();
                CarregarCores();
                CarregarColecoes();
                CarregarFornecedores();
                CarregarUnidadeMedida();

                if (prod_hb != null)
                {
                    hidHB.Value = codigo;

                    ddlColecoes.SelectedValue = prod_hb.COLECAO;
                    ddlColecoes.Enabled = false;
                    txtHB.Text = prod_hb.HB.ToString();
                    txtHB.ReadOnly = true;
                    ddlGrupo.SelectedValue = prod_hb.GRUPO;
                    ddlGrupo.Enabled = false;
                    txtNome.Text = prod_hb.NOME;
                    txtNome.ReadOnly = true;
                    txtData.Text = prod_hb.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                    txtData.ReadOnly = true;
                    txtTecido.Text = prod_hb.TECIDO;
                    txtTecido.ReadOnly = true;
                    txtPedidoNumero.Text = prod_hb.NUMERO_PEDIDO.ToString();
                    txtPedidoNumero.ReadOnly = true;
                    ddlCor.SelectedValue = prod_hb.COR;
                    ddlCor.Enabled = false;
                    txtCorFornecedor.Text = prod_hb.COR_FORNECEDOR;
                    txtCorFornecedor.ReadOnly = true;
                    txtFornecedor.Text = prod_hb.FORNECEDOR;
                    txtFornecedor.ReadOnly = true;
                    txtLargura.Text = prod_hb.LARGURA.ToString();
                    txtLargura.ReadOnly = true;
                    txtModelagem.Text = prod_hb.MODELAGEM;
                    txtModelagem.ReadOnly = true;
                    cbLiquidar.Checked = (prod_hb.LIQUIDAR == 'S') ? true : false;
                    cbLiquidar.Enabled = false;
                    txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                    txtCustoTecido.ReadOnly = true;
                    txtPrecoFaccaoMostruairo.Text = (prod_hb.PRECO_FACC_MOSTRUARIO == null) ? "" : prod_hb.PRECO_FACC_MOSTRUARIO.ToString();
                    txtPrecoFaccaoMostruairo.ReadOnly = true;
                    imgFotoTecido.ImageUrl = prod_hb.FOTO_TECIDO;

                    txtCodigoLinx.Text = prod_hb.CODIGO_PRODUTO_LINX.ToString();
                    txtCodigoLinx.ReadOnly = true;

                    txtMolde.Text = (prod_hb.MOLDE != null) ? prod_hb.MOLDE.ToString() : "";
                    txtMolde.ReadOnly = false;

                    cbAtacado.Checked = (prod_hb.ATACADO == 'S') ? true : false;
                    cbAtacado.Enabled = false;
                    if (cbAtacado.Checked)
                    {
                        fsGradeAtacado.Visible = true;
                        PreencherGradeAtacado(codigo);
                    }

                    gvComposicao.DataSource = prodController.ObterComposicaoHB(Convert.ToInt32(codigo));
                    gvComposicao.DataBind();

                    CarregarNomeGrade(prod_hb);

                    PROD_HB_GRADE _gradeP = null;
                    _gradeP = prodController.ObterGradeHB(Convert.ToInt32(codigo), 2); // RISCO
                    if (_gradeP == null)
                        _gradeP = prodController.ObterGradeHB(Convert.ToInt32(codigo), 1); //AMPLIAÇÃO

                    int totalGrade = 0;
                    if (_gradeP != null)
                    {
                        txtGradeEXP.Text = _gradeP.GRADE_EXP.ToString();
                        txtGradeEXP.ReadOnly = true;
                        txtGradeXP.Text = _gradeP.GRADE_XP.ToString();
                        txtGradeXP.ReadOnly = true;
                        txtGradePP.Text = _gradeP.GRADE_PP.ToString();
                        txtGradePP.ReadOnly = true;
                        txtGradeP.Text = _gradeP.GRADE_P.ToString();
                        txtGradeP.ReadOnly = true;
                        txtGradeM.Text = _gradeP.GRADE_M.ToString();
                        txtGradeM.ReadOnly = true;
                        txtGradeG.Text = _gradeP.GRADE_G.ToString();
                        txtGradeG.ReadOnly = true;
                        txtGradeGG.Text = _gradeP.GRADE_GG.ToString();
                        txtGradeGG.ReadOnly = true;

                        totalGrade = (Convert.ToInt32(_gradeP.GRADE_EXP) + Convert.ToInt32(_gradeP.GRADE_XP) + Convert.ToInt32(_gradeP.GRADE_PP) + Convert.ToInt32(_gradeP.GRADE_P) + Convert.ToInt32(_gradeP.GRADE_M) + Convert.ToInt32(_gradeP.GRADE_G) + Convert.ToInt32(_gradeP.GRADE_GG));
                        txtGradeTotal.Text = totalGrade.ToString();
                        txtGradeTotal.ReadOnly = true;
                    }

                    PROD_HB_GRADE _gradeR = prodController.ObterGradeHB(Convert.ToInt32(codigo), 3);
                    totalGrade = 0;
                    if (_gradeR != null)
                    {
                        txtGradeEXP_R.Text = _gradeR.GRADE_EXP.ToString();
                        txtGradeEXP_R.ReadOnly = true;
                        txtGradeXP_R.Text = _gradeR.GRADE_XP.ToString();
                        txtGradeXP_R.ReadOnly = true;
                        txtGradePP_R.Text = _gradeR.GRADE_PP.ToString();
                        txtGradePP_R.ReadOnly = true;
                        txtGradeP_R.Text = _gradeR.GRADE_P.ToString();
                        txtGradeP_R.ReadOnly = true;
                        txtGradeM_R.Text = _gradeR.GRADE_M.ToString();
                        txtGradeM_R.ReadOnly = true;
                        txtGradeG_R.Text = _gradeR.GRADE_G.ToString();
                        txtGradeG_R.ReadOnly = true;
                        txtGradeGG_R.Text = _gradeR.GRADE_GG.ToString();
                        txtGradeGG_R.ReadOnly = true;

                        totalGrade = (Convert.ToInt32(_gradeR.GRADE_EXP) + Convert.ToInt32(_gradeR.GRADE_XP) + Convert.ToInt32(_gradeR.GRADE_PP) + Convert.ToInt32(_gradeR.GRADE_P) + Convert.ToInt32(_gradeR.GRADE_M) + Convert.ToInt32(_gradeR.GRADE_G) + Convert.ToInt32(_gradeR.GRADE_GG));
                        txtGradeTotal_R.Text = totalGrade.ToString();
                        txtGradeTotal_R.ReadOnly = true;
                    }

                    txtGastoPorCorte.Attributes.Add("readonly", "readonly");
                    txtGastoPorPeca.Attributes.Add("readonly", "readonly");
                    txtGradeTotal_R.Attributes.Add("readonly", "readonly");

                    gvDetalhe.DataSource = prodController.ObterDetalhesHB(Convert.ToInt32(codigo));
                    gvDetalhe.DataBind();

                    gvAviamento.DataSource = prodController.ObterAviamentoHB(Convert.ToInt32(codigo));
                    gvAviamento.DataBind();

                    ddlUnidade.SelectedValue = prod_hb.UNIDADE_MEDIDA.ToString();
                    ddlUnidade.Enabled = false;
                    txtGastoPorFolha.Text = prod_hb.GASTO_FOLHA.ToString();
                    txtGastoPorFolha.ReadOnly = true;


                    if (prod_hb.MOSTRUARIO == 'N')
                    {
                        labGastoPorPecaCusto.Visible = false;
                        txtGastoPorPecaCusto.Visible = false;
                    }
                    else
                    {
                        if (prod_hb.GASTO_PECA_CUSTO != null)
                        {
                            txtGastoPorPecaCusto.Text = prod_hb.GASTO_PECA_CUSTO.ToString();
                            txtGastoPorPecaCusto.ReadOnly = true;
                        }
                    }

                    txtGastoRetalhos.Text = prod_hb.RETALHOS.ToString();
                    txtGastoRetalhos.ReadOnly = true;

                    if (totalGrade > 0)
                    {
                        decimal sobraMetro = 1;
                        txtGastoPorCorte.Text = ((Convert.ToDecimal(prod_hb.GASTO_FOLHA) * totalGrade)).ToString("####0.000");
                        txtGastoPorCorte.ReadOnly = true;
                        txtGastoPorPeca.Text = (((Convert.ToDecimal(txtGastoPorCorte.Text) + Convert.ToDecimal(prod_hb.RETALHOS)) / totalGrade) * sobraMetro).ToString("####0.000");
                        txtGastoPorPeca.ReadOnly = true;
                    }

                    imgFotoPeca.ImageUrl = prod_hb.FOTO_PECA;

                    var rotaHB = prodController.ObterRotaHB(prod_hb.CODIGO);
                    if (rotaHB != null)
                    {

                        if (rotaHB.LAVANDERIA)
                            lblLavanderia.Text = "SIM";
                        if (rotaHB.ESTAMPARIA)
                            lblEstamparia.Text = "SIM";
                        if (rotaHB.FACCAO)
                            lblFaccao.Text = "SIM";
                        if (rotaHB.ACABAMENTO)
                            lblAcabamento.Text = "SIM";
                    }

                    txtObservacao.Text = prod_hb.OBSERVACAO;

                    RecarregarGradeCorteHB(Convert.ToInt32(codigo));

                    dialogPai.Visible = false;
                }

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
            List<UNIDADE_MEDIDA> _unidadeMedida = prodController.ObterUnidadeMedida();

            _unidadeMedida.Add(new UNIDADE_MEDIDA { CODIGO = 0, DESCRICAO = "Selecione", STATUS = 'A' });

            _unidadeMedida = _unidadeMedida.OrderBy(l => l.CODIGO).ToList();

            if (_unidadeMedida != null)
            {
                ddlUnidade.DataSource = _unidadeMedida;
                ddlUnidade.DataBind();
            }
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

                        Literal _litNumeroPedido = e.Row.FindControl("litNumeroPedido") as Literal;
                        if (_litNumeroPedido != null)
                            _litNumeroPedido.Text = (_hb.NUMERO_PEDIDO == null) ? "" : _hb.NUMERO_PEDIDO.ToString();
                    }
                }
            }
        }
        #endregion

        #region "AVIAMENTOS"
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

                        Label _labQtdePorPeca = e.Row.FindControl("labQtdePorPeca") as Label;
                        if (_labQtdePorPeca != null)
                            _labQtdePorPeca.Text = _aviamento.QTDE_POR_PECA.ToString().Replace(",000", "");

                        Literal _litQtde = e.Row.FindControl("litQtde") as Literal;
                        if (_litQtde != null)
                            _litQtde.Text = (_aviamento.QTDE + ((_aviamento.QTDE_EXTRA == null) ? 0 : _aviamento.QTDE_EXTRA)).ToString().Replace(",000", "");
                    }
                }
            }
        }
        #endregion

        #region "GRADE CORTE"
        private void RecarregarGradeCorteHB(int codigo)
        {
            List<SP_OBTER_GRADE_CORTEResult> _list = prodController.ObterGradeCorteHB(codigo);
            if (_list != null)
            {
                gvGradeCorteHB.DataSource = _list;
                gvGradeCorteHB.DataBind();
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
                    }
                }
            }
        }
        #endregion

        #region "HB"
        private PROD_HB ObterHB(int codigo)
        {
            return prodController.ObterHB(codigo);
        }
        private void PreencherGradeAtacado(string prod_hb)
        {
            PROD_HB_GRADE _gradeA = prodController.ObterGradeHB(Convert.ToInt32(prod_hb), 99);
            int totalGrade = 0;
            if (_gradeA != null)
            {
                txtGradeEXP_A.Text = _gradeA.GRADE_EXP.ToString();
                txtGradeEXP_A.ReadOnly = true;
                txtGradeXP_A.Text = _gradeA.GRADE_XP.ToString();
                txtGradeXP_A.ReadOnly = true;
                txtGradePP_A.Text = _gradeA.GRADE_PP.ToString();
                txtGradePP_A.ReadOnly = true;
                txtGradeP_A.Text = _gradeA.GRADE_P.ToString();
                txtGradeP_A.ReadOnly = true;
                txtGradeM_A.Text = _gradeA.GRADE_M.ToString();
                txtGradeM_A.ReadOnly = true;
                txtGradeG_A.Text = _gradeA.GRADE_G.ToString();
                txtGradeG_A.ReadOnly = true;
                txtGradeGG_A.Text = _gradeA.GRADE_GG.ToString();
                txtGradeGG_A.ReadOnly = true;

                totalGrade = (Convert.ToInt32(_gradeA.GRADE_EXP) + Convert.ToInt32(_gradeA.GRADE_XP) + Convert.ToInt32(_gradeA.GRADE_PP) + Convert.ToInt32(_gradeA.GRADE_P) + Convert.ToInt32(_gradeA.GRADE_M) + Convert.ToInt32(_gradeA.GRADE_G) + Convert.ToInt32(_gradeA.GRADE_GG));
                txtGradeTotal_A.Text = totalGrade.ToString();
                txtGradeTotal_A.ReadOnly = true;
            }
        }

        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            int totalGrade = 0;
            try
            {
                if (hidHB.Value != null)
                {
                    //RELATORIO
                    var prod_hb = prodController.ObterHB(Convert.ToInt32(hidHB.Value));
                    RelatorioHB _relatorio = new RelatorioHB();
                    _relatorio.HB = txtHB.Text.ToString();
                    _relatorio.COLECAO = ddlColecoes.SelectedItem.Text.Trim();
                    _relatorio.COD_COR = ddlCor.SelectedValue.Trim();
                    _relatorio.DESC_COR = ddlCor.SelectedItem.Text.Trim();
                    _relatorio.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                    _relatorio.GRUPO = ddlGrupo.SelectedItem.Text.Trim();
                    _relatorio.NOME = txtNome.Text.ToString().Trim().ToUpper();
                    _relatorio.DATA = Convert.ToDateTime(txtData.Text).ToString("dd/MM/yyyy");
                    _relatorio.GRUPO_TECIDO = prod_hb.GRUPO_TECIDO.ToString();
                    _relatorio.TECIDO = txtTecido.Text.Trim().ToUpper();
                    _relatorio.FORNECEDOR = txtFornecedor.Text.Trim();
                    _relatorio.LARGURA = txtLargura.Text.Trim();
                    _relatorio.LIQUIDAR = (cbLiquidar.Checked) ? "Sim" : "Não";
                    //_relatorio.ATACADO = (cbAtacado.Checked) ? "Sim" : "Não";
                    _relatorio.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
                    _relatorio.FOTO_PECA = imgFotoPeca.ImageUrl;
                    _relatorio.FOTO_TECIDO = imgFotoTecido.ImageUrl;
                    _relatorio.CUSTO_TECIDO = txtCustoTecido.Text;
                    _relatorio.OBS = txtObservacao.Text.Trim().ToUpper();
                    _relatorio.COMPOSICAO = prodController.ObterComposicaoHB(Convert.ToInt32(hidHB.Value));
                    _relatorio.DETALHE = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));
                    _relatorio.AVIAMENTO = prodController.ObterAviamentoHB(Convert.ToInt32(hidHB.Value));
                    _relatorio.GRADE_CORTE = prodController.ObterGradeCorteHB(Convert.ToInt32(hidHB.Value));
                    _relatorio.CODIGO_PRODUTO_LINX = txtCodigoLinx.Text.ToString();
                    _relatorio.NUMERO_PEDIDO = txtPedidoNumero.Text.ToString();
                    _relatorio.PROD_GRADE = prod_hb.PROD_GRADE.ToString();

                    DesenvolvimentoController desenvController = new DesenvolvimentoController();
                    List<DESENV_PRODUTO> _produto = desenvController.ObterProdutoRel(ddlColecoes.SelectedValue, txtCodigoLinx.Text.ToString(), ddlCor.SelectedValue.Trim());
                    if (_produto != null && _produto.Count > 0)
                    {
                        if (_produto[0].GRIFFE != null)
                            _relatorio.GRIFFE = _produto[0].GRIFFE;

                        _relatorio.SIGNED = "N";
                        if (_produto[0].SIGNED != null)
                            _relatorio.SIGNED = _produto[0].SIGNED.ToString();

                        _relatorio.SIGNED_NOME = "";
                        if (_produto[0].SIGNED_NOME != null)
                            _relatorio.SIGNED_NOME = _produto[0].SIGNED_NOME;

                    }

                    PROD_HB_ROTA _rota = new PROD_HB_ROTA();
                    _rota = prodController.ObterRotaOP(prod_hb.ORDEM_PRODUCAO);
                    if (_rota != null)
                    {
                        if (_rota.ESTAMPARIA == true)
                        {
                            _relatorio.FaseEstamparia = "SIM";
                        }
                        else
                        {
                            _relatorio.FaseEstamparia = "NÃO";
                        }
                        if (_rota.ESTAMPARIA == true)
                        {
                            _relatorio.FaseLavanderia = "SIM";
                        }
                        else
                        {
                            _relatorio.FaseLavanderia = "NÃO";
                        }
                    }
                    else
                    {
                        _relatorio.FaseEstamparia = "NÃO";
                        _relatorio.FaseLavanderia = "NÃO";
                    }

                    //GRADE REAL
                    PROD_HB_GRADE gradeReal = null;
                    gradeReal = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 3); //CORTE
                    _relatorio.GRADEREAL = gradeReal;
                    if (gradeReal != null)
                        totalGrade = Convert.ToInt32((gradeReal.GRADE_EXP + gradeReal.GRADE_XP + gradeReal.GRADE_PP + gradeReal.GRADE_P + gradeReal.GRADE_M + gradeReal.GRADE_G + gradeReal.GRADE_GG));

                    //GRADE PREVISTA
                    PROD_HB_GRADE grade = null;
                    grade = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 2); //RISCO
                    if (grade == null)
                        grade = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 1); //AMPLIACAO
                    _relatorio.GRADE = grade;
                    if (totalGrade <= 0)
                        totalGrade = Convert.ToInt32((grade.GRADE_EXP + grade.GRADE_XP + grade.GRADE_PP + grade.GRADE_P + grade.GRADE_M + grade.GRADE_G + grade.GRADE_GG));

                    //RETALHOS
                    if (txtGastoRetalhos.Text != "" && txtGastoPorFolha.Text != "")
                    {
                        _relatorio.UNIDADE_MEDIDA = ddlUnidade.SelectedValue.ToString();

                        _relatorio.RETALHO = Convert.ToDecimal(txtGastoRetalhos.Text).ToString("####0.00");
                        //(GASTO_FOLHA * TOTAL GRADE)
                        _relatorio.PORCORTE = Convert.ToDecimal((((Convert.ToDecimal(txtGastoPorFolha.Text) > 0) ? Convert.ToDecimal(txtGastoPorFolha.Text) : 0) * totalGrade)).ToString("####0.000").Replace(",000", "");
                        //(POR CORTE + RETALHOS / TOTAL GRADE)
                        _relatorio.PORPECA = Convert.ToDecimal(((Convert.ToDecimal(_relatorio.PORCORTE) + Convert.ToDecimal(_relatorio.RETALHO)) / totalGrade)).ToString("####0.000").Replace(",000", "");
                    }

                    if (_relatorio != null)
                        GerarRelatorio(_relatorio);
                }
            }
            catch (Exception ex)
            {
                labErroEnvio.Text = ex.Message;
            }
        }
        private void GerarRelatorio(RelatorioHB _relatorio)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "HB_PRINC_RE" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_relatorio));
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
        private StringBuilder MontarRelatorioHTML(RelatorioHB _relatorio)
        {
            StringBuilder _texto = new StringBuilder();

            //Cabecalho
            _texto = _relatorio.MontarCabecalho(_texto, "HB - Consulta");
            //Montar ORDEM DE CORTE
            _texto = _relatorio.MontarTecido(_texto, _relatorio);
            _texto = _relatorio.MontarComposicao(_texto, _relatorio);
            _texto = _relatorio.MontarGrade(_texto, _relatorio);
            _texto = _relatorio.MontarGradeCorte(_texto, _relatorio);
            _texto = _relatorio.MontarLinhasEmBranco(_texto);
            _texto = _relatorio.MontarDetalhes(_texto, _relatorio);
            _texto = _relatorio.MontarModelagem(_texto, _relatorio);
            _texto = _relatorio.MontarPeca(_texto, _relatorio);
            _texto = _relatorio.MontarOBS(_texto, _relatorio);
            //Rodape
            _texto = _relatorio.MontarRodape(_texto);


            return _texto;
        }
        #endregion

    }
}
