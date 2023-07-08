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
    public partial class prod_con_hb_det : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int colunaComposicao, colunagradecortehb = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "")
                    Response.Redirect("prod_menu.aspx");

                string tela = Request.QueryString["t"].ToString();

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
                _processo = prodController.ObterProcessoHB(Convert.ToInt32(codigo), 3);
                if (_processo == null)
                    _processo = prodController.ObterProcessoHB(Convert.ToInt32(codigo), 2);

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

                    hidDetalheHB.Value = codigo;

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
                    ddlCor.SelectedValue = prod_hb.COR;
                    ddlCor.Enabled = false;
                    txtCorFornecedor.Text = prod_hb.COR_FORNECEDOR;
                    txtCorFornecedor.ReadOnly = true;
                    txtFornecedor.Text = prod_hb.FORNECEDOR;
                    txtFornecedor.ReadOnly = false;
                    txtLargura.Text = prod_hb.LARGURA.ToString();
                    txtLargura.ReadOnly = true;
                    txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                    txtCustoTecido.ReadOnly = true;
                    imgFotoTecido.ImageUrl = prod_hb.FOTO_TECIDO;
                    txtNumeroPedido.Text = (prod_hb.NUMERO_PEDIDO == null) ? "" : prod_hb.NUMERO_PEDIDO.ToString();
                    txtNumeroPedido.ReadOnly = true;

                    txtMolde.Text = (prod_hb.MOLDE == null) ? "" : prod_hb.MOLDE.ToString();
                    txtMolde.ReadOnly = true;


                    gvComposicao.DataSource = prodController.ObterComposicaoHB(Convert.ToInt32(codigo));
                    gvComposicao.DataBind();

                    CarregarNomeGrade(prodController.ObterHB(Convert.ToInt32(prod_hb.CODIGO_PAI)));

                    labTituloDetalhe.Text = prod_hb.PROD_DETALHE1.DESCRICAO.ToUpper();

                    PROD_HB_GRADE _gradeP = prodController.ObterGradeHB(Convert.ToInt32(codigo), 2); // RISCO
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

                    totalGrade = 0;
                    PROD_HB_GRADE _gradeR = prodController.ObterGradeHB(Convert.ToInt32(codigo), 3);//CORTE
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
                        txtGastoPorCorte.Text = ((Convert.ToDecimal(prod_hb.GASTO_FOLHA) * totalGrade)).ToString("####0.000");
                        txtGastoPorCorte.ReadOnly = true;

                        //Controle de estoque de metro, somar 4%
                        decimal sobraMetro = 1;
                        txtGastoPorPeca.Text = (((Convert.ToDecimal(txtGastoPorCorte.Text) + Convert.ToDecimal(prod_hb.RETALHOS)) / totalGrade) * sobraMetro).ToString("####0.000");
                        txtGastoPorPeca.ReadOnly = true;
                    }

                    txtObservacao.Text = prod_hb.OBSERVACAO;

                    RecarregarGradeCorteHB(Convert.ToInt32(codigo));

                    hrefVoltar.HRef = "prod_con_hb_filtro.aspx?t=" + tela + "&d=2&col=" + prod_hb.COLECAO + "&h=" + prod_hb.HB.ToString() + "";
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
            */
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
        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            int totalGrade = 0;
            try
            {
                if (hidDetalheHB.Value != null)
                {
                    List<PROD_HB> _prod_HB = new List<PROD_HB>();

                    //RELATORIO
                    RelatorioHB _relatorio = new RelatorioHB();

                    //DETALHE
                    List<PROD_HB> detalhe = new List<PROD_HB>();
                    detalhe.Add(prodController.ObterHB(Convert.ToInt32(hidDetalheHB.Value)));

                    _prod_HB.Add(prodController.ObterHB(Convert.ToInt32(detalhe[0].CODIGO_PAI)));

                    _relatorio.DETALHE = detalhe;
                    _relatorio.PROD_GRADE = prodController.ObterHB(Convert.ToInt32(detalhe[0].CODIGO_PAI)).PROD_GRADE.ToString();

                    //GRADE REAL
                    PROD_HB_GRADE gradeReal = null;
                    gradeReal = prodController.ObterGradeHB(Convert.ToInt32(hidDetalheHB.Value), 3); //CORTE
                    _relatorio.GRADEREAL = gradeReal;
                    if (gradeReal != null)
                        totalGrade = Convert.ToInt32((gradeReal.GRADE_EXP + gradeReal.GRADE_XP + gradeReal.GRADE_PP + gradeReal.GRADE_P + gradeReal.GRADE_M + gradeReal.GRADE_G + gradeReal.GRADE_GG));

                    //GRADE PREVISTA
                    PROD_HB_GRADE grade = null;
                    grade = prodController.ObterGradeHB(Convert.ToInt32(hidDetalheHB.Value), 2); //RISCO
                    if (grade == null)
                        grade = prodController.ObterGradeHB(Convert.ToInt32(hidDetalheHB.Value), 1); //AMPLIACAO
                    _relatorio.GRADE = grade;
                    if (totalGrade <= 0)
                        totalGrade = Convert.ToInt32((grade.GRADE_EXP + grade.GRADE_XP + grade.GRADE_PP + grade.GRADE_P + grade.GRADE_M + grade.GRADE_G + grade.GRADE_GG));

                    if (txtGastoRetalhos.Text != "" && txtGastoPorFolha.Text != "")
                    {
                        _relatorio.UNIDADE_MEDIDA = detalhe[0].UNIDADE_MEDIDA1.DESCRICAO;
                        _relatorio.RETALHO = Convert.ToDecimal(detalhe[0].RETALHOS).ToString("####0.00");

                        //(GASTO_FOLHA * TOTAL GRADE)
                        _relatorio.PORCORTE = Convert.ToDecimal((((detalhe[0].GASTO_FOLHA > 0) ? detalhe[0].GASTO_FOLHA : 0) * totalGrade)).ToString("####0.000").Replace(",000", "");
                        //(POR CORTE + RETALHOS / TOTAL GRADE)
                        _relatorio.PORPECA = Convert.ToDecimal(((Convert.ToDecimal(_relatorio.PORCORTE) + Convert.ToDecimal(_relatorio.RETALHO)) / totalGrade)).ToString("####0.000").Replace(",000", "");
                    }

                    DesenvolvimentoController desenvController = new DesenvolvimentoController();
                    List<DESENV_PRODUTO> _produto = desenvController.ObterProdutoRel(_prod_HB[0].COLECAO, _prod_HB[0].CODIGO_PRODUTO_LINX.ToString(), _prod_HB[0].COR);
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
                    _rota = prodController.ObterRotaOP(_prod_HB[0].ORDEM_PRODUCAO);
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
                string nomeArquivo = "HB_DETALHE_" + _relatorio.DETALHE[0].PROD_DETALHE.ToString() + "_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
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

            _texto = _relatorio.MontarCabecalho(_texto, "HB Detalhe - Consulta");
            _texto = _relatorio.MontarDetalhe(_texto, _relatorio, "");
            _texto = _relatorio.MontarRodape(_texto);

            return _texto;
        }
        #endregion

    }
}
