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
    public partial class prod_cad_hb_baixa_det : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        FaccaoController faccController = new FaccaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int colunagradecortehbbaixa, colunagradecortehb, colunagradesub, colunaComposicao = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Request.QueryString["d"] == null || Request.QueryString["d"] == "" ||
                    Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                    Response.Redirect("prod_menu.aspx");

                DateTime validaData;
                bool hbValido = true;
                string codigo = Request.QueryString["c"].ToString();
                string data = Request.QueryString["d"].ToString().Replace("@", "/");
                string tela = Request.QueryString["t"].ToString();

                hidHB.Value = codigo;
                hidData.Value = data;
                hidTela.Value = tela;

                //Valida data
                if (!DateTime.TryParse(data, out validaData))
                    hbValido = false;

                //Valida HB
                PROD_HB prod_hb = ObterHB(Convert.ToInt32(codigo));
                if (prod_hb == null)
                    hbValido = false;

                if (!hbValido || (tela != "2" && tela != "3" && tela != "4"))
                {
                    labConteudoDiv.Text = "NÃO ENCONTRADO";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Ok: function () { window.open('prod_menu.aspx', '_self'); } } }); });", true);
                    return;
                }

                if (hidTela.Value == "2") //ACESSANDO ESTA TELA DA BAIXA DO RISCO
                {
                    divBaixaCorte.Visible = false;
                    hrefVoltar.HRef = "prod_fila_risco.aspx";
                    trGradeReal.Visible = false;
                    fsRisco.Visible = true;
                    fsCorteBaixa.Visible = false;

                    CarregarGradeCorte(prodController.ObterHB(Convert.ToInt32(prod_hb.CODIGO_PAI)));
                }
                if (hidTela.Value == "3") //ACESSANDO ESTA TELA DA BAIXA DO CORTE
                {
                    divBaixaCorte.Visible = true;
                    hrefVoltar.HRef = "prod_fila_corte.aspx";
                    trGradeReal.Visible = true;
                    trRiscoPrevisto.Visible = false;
                    fsRisco.Visible = false;
                    fsCorteBaixa.Visible = false;
                    if (prod_hb.PROD_DETALHE != 4 && prod_hb.PROD_DETALHE != 5)
                        fsCorteBaixa.Visible = true;

                    if (prod_hb.PROD_DETALHE == 4 || prod_hb.PROD_DETALHE == 5)
                        prodController.InserirDadosGalao(Convert.ToInt32(prod_hb.CODIGO_PAI), Convert.ToInt32(prod_hb.PROD_DETALHE));
                }
                if (hidTela.Value == "4") //ACESSANDO ESTA TELA DA BAIXA DO MOSTRUARIO
                {
                    divBaixaCorte.Visible = true;
                    hrefVoltar.HRef = "prod_fila_mostruario.aspx";
                    trGradeReal.Visible = true;
                    fsRisco.Visible = true;
                    fsCorteBaixa.Visible = true;
                }

                //Carregar Combos
                CarregarGrupo();
                CarregarCores();
                CarregarColecoes();
                CarregarFornecedores();
                CarregarUnidadeMedida();

                if (prod_hb != null)
                {
                    //TITULO
                    labDetalheTitulo.Text = prod_hb.PROD_DETALHE1.DESCRICAO.Trim().ToUpper();

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
                    txtCorFornecedor.Text = prod_hb.COR_FORNECEDOR;
                    txtCorFornecedor.ReadOnly = true;
                    ddlCor.SelectedValue = prod_hb.COR;
                    ddlCor.Enabled = false;
                    txtFornecedor.Text = prod_hb.FORNECEDOR;
                    txtFornecedor.ReadOnly = true;
                    txtLargura.Text = prod_hb.LARGURA.ToString();
                    txtLargura.ReadOnly = true;
                    txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                    txtCustoTecido.ReadOnly = true;
                    imgFotoTecido.ImageUrl = prod_hb.FOTO_TECIDO;

                    gvComposicao.DataSource = prodController.ObterComposicaoHB(Convert.ToInt32(codigo));
                    gvComposicao.DataBind();

                    var principal = prodController.ObterHB(Convert.ToInt32(prod_hb.CODIGO_PAI));

                    CarregarNomeGrade(principal);

                    if (prod_hb.MOSTRUARIO == 'N')
                    {
                        labGastoPorPecaCusto.Visible = false;
                        txtGastoPorPecaCusto.Visible = false;
                    }

                    PROD_HB_GRADE _grade = prodController.ObterGradeHB(Convert.ToInt32(codigo), 2);
                    if (_grade != null)
                    {
                        txtGradeEXPIni.Text = _grade.GRADE_EXP.ToString();
                        txtGradeEXPIni.Enabled = false;
                        txtGradeXPIni.Text = _grade.GRADE_XP.ToString();
                        txtGradeXPIni.Enabled = false;
                        txtGradePPIni.Text = _grade.GRADE_PP.ToString();
                        txtGradePPIni.Enabled = false;
                        txtGradePIni.Text = _grade.GRADE_P.ToString();
                        txtGradePIni.Enabled = false;
                        txtGradeMIni.Text = _grade.GRADE_M.ToString();
                        txtGradeMIni.Enabled = false;
                        txtGradeGIni.Text = _grade.GRADE_G.ToString();
                        txtGradeGIni.Enabled = false;
                        txtGradeGGIni.Text = _grade.GRADE_GG.ToString();
                        txtGradeGGIni.Enabled = false;

                        int totalGrade = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG));
                        txtGradeTotalIni.Text = totalGrade.ToString();
                        txtGradeTotalIni.Enabled = false;

                        //grade sera alterada pelo risco adicionado
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

                        PreencherGradeReal(prod_hb.CODIGO, true);
                        PreencherGradeReal(prod_hb.CODIGO, false);

                    }

                    txtGastoPorCorte.Attributes.Add("readonly", "readonly");
                    txtGastoPorPeca.Attributes.Add("readonly", "readonly");
                    txtGradeTotal_R.Attributes.Add("readonly", "readonly");

                    txtObservacao.Text = prod_hb.OBSERVACAO;

                    RecarregarGradeSub();
                    RecarregarGradeCorteHB();

                    //PREENCHER UNIDADE DE MEDIDA AUTOMATICAMENTE
                    var material = new DesenvolvimentoController().ObterMaterial().Where(p => p.GRUPO.Trim() == prod_hb.GRUPO_TECIDO.Trim() &&
                                                                                              p.SUBGRUPO.Trim() == prod_hb.TECIDO.Trim()).ToList();
                    if (material != null && material.Count() > 0)
                    {
                        if (material[0].UNIDADE.UNIDADE1.Trim() == "KG")
                            ddlUnidade.SelectedValue = "3";
                        else if (material[0].UNIDADE.UNIDADE1.Trim() == "MT")
                            ddlUnidade.SelectedValue = "1";
                        else if (material[0].UNIDADE.UNIDADE1.Trim() == "UN")
                            ddlUnidade.SelectedValue = "2";
                        else
                            ddlUnidade.SelectedValue = "0";
                    }

                    var _detalhes = prodController.ObterDetalhesHB(Convert.ToInt32(prod_hb.CODIGO_PAI));
                    bool detBaixado = true;
                    foreach (var d in _detalhes.Where(p => p.CODIGO != prod_hb.CODIGO))
                        if (d != null && d.STATUS != 'B')
                            detBaixado = false;

                    var rotaExiste = faccController.ObterTabelaOperacao(principal.CODIGO_PRODUTO_LINX);
                    if (detBaixado && rotaExiste == null)
                        fsFaccao.Visible = true;

                    MontarRiscoNome();

                    dialogPai.Visible = false;
                }

            }

            //Tem que fazer independente do POST BACK
            RecarregarGVCorteBaixaDropDownList();
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
            string strEnfesto = "";
            string strCorte = "";
            string strAmarrador = "";
            try
            {

                foreach (GridViewRow r in gvGradeCorteHBBaixa.Rows)
                {
                    DropDownList _enfesto1 = r.FindControl("ddlEnfesto1") as DropDownList;
                    if (_enfesto1 != null)
                    {
                        _enfesto1.Items.Clear();
                        strEnfesto = Request.Form[_enfesto1.UniqueID];
                        CarregarDropDownList(CarregarCorteFuncionario(), _enfesto1);

                        //Set valor no client no server
                        if (strEnfesto != null)
                            _enfesto1.Items.FindByValue(strEnfesto).Selected = true;
                    }
                    DropDownList _enfesto2 = r.FindControl("ddlEnfesto2") as DropDownList;
                    if (_enfesto2 != null)
                    {
                        _enfesto2.Items.Clear();
                        strEnfesto = Request.Form[_enfesto2.UniqueID];
                        CarregarDropDownList(CarregarCorteFuncionario(), _enfesto2);

                        //Set valor no client no server
                        if (strEnfesto != null)
                            _enfesto2.Items.FindByValue(strEnfesto).Selected = true;
                    }
                    DropDownList _enfesto3 = r.FindControl("ddlEnfesto3") as DropDownList;
                    if (_enfesto3 != null)
                    {
                        _enfesto3.Items.Clear();
                        strEnfesto = Request.Form[_enfesto3.UniqueID];
                        CarregarDropDownList(CarregarCorteFuncionario(), _enfesto3);

                        //Set valor no client no server
                        if (strEnfesto != null)
                            _enfesto3.Items.FindByValue(strEnfesto).Selected = true;
                    }

                    DropDownList _corte1 = r.FindControl("ddlCorte1") as DropDownList;
                    if (_corte1 != null)
                    {
                        _corte1.Items.Clear();
                        strCorte = Request.Form[_corte1.UniqueID];
                        CarregarDropDownList(CarregarCorteFuncionario(), _corte1);

                        //Set valor no client no server
                        if (strCorte != null)
                            _corte1.Items.FindByValue(strCorte).Selected = true;
                    }
                    DropDownList _corte2 = r.FindControl("ddlCorte2") as DropDownList;
                    if (_corte2 != null)
                    {
                        _corte2.Items.Clear();
                        strCorte = Request.Form[_corte2.UniqueID];
                        CarregarDropDownList(CarregarCorteFuncionario(), _corte2);

                        //Set valor no client no server
                        if (strCorte != null)
                            _corte2.Items.FindByValue(strCorte).Selected = true;
                    }

                    DropDownList _amarrador1 = r.FindControl("ddlAmarrador1") as DropDownList;
                    if (_amarrador1 != null)
                    {
                        _amarrador1.Items.Clear();
                        strAmarrador = Request.Form[_amarrador1.UniqueID];
                        CarregarDropDownList(CarregarCorteFuncionario(), _amarrador1);

                        //Set valor no client no server
                        if (strAmarrador != null)
                            _amarrador1.Items.FindByValue(strAmarrador).Selected = true;
                    }
                    DropDownList _amarrador2 = r.FindControl("ddlAmarrador2") as DropDownList;
                    if (_amarrador2 != null)
                    {
                        _amarrador2.Items.Clear();
                        strAmarrador = Request.Form[_amarrador2.UniqueID];
                        CarregarDropDownList(CarregarCorteFuncionario(), _amarrador2);

                        //Set valor no client no server
                        if (strAmarrador != null)
                            _amarrador2.Items.FindByValue(strAmarrador).Selected = true;
                    }

                }

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

                txtGradeP_R.Enabled = (_gradeNome.GRADE_P == "-") ? false : true;
                txtGradeM_R.Enabled = (_gradeNome.GRADE_M == "-") ? false : true;
                txtGradeG_R.Enabled = (_gradeNome.GRADE_G == "-") ? false : true;
                txtGradeGG_R.Enabled = (_gradeNome.GRADE_GG == "-") ? false : true;
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

                labErroGradeSub.Text = "";
                var validaGrade = prodController.ObterGradeSubHB(Convert.ToInt32(hidHB.Value));
                if (validaGrade != null && validaGrade.Where(p => p.GRADE.Trim() == ddlGradeNumero.SelectedValue.Trim() && p.LETRA.Trim() == ddlGradeLetra.SelectedValue.Trim()).Count() > 0)
                {
                    labErroGradeSub.Text = "Não é possível inserir mais de uma grade igual.";
                    return;
                }

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
        private void MontarRiscoNome()
        {
            string riscoPosicao = "1";
            string riscoLetra = "R";

            int codigoHB = Convert.ToInt32(hidHB.Value);
            var p = prodController.ObterHB(codigoHB);
            if (p != null)
            {
                if (p.PROD_DETALHE == null) // PRINCIPAL
                    riscoLetra = "R";
                else if (p.PROD_DETALHE == 1) //COR 2
                    riscoLetra = "CR";
                else if (p.PROD_DETALHE == 2) //COLANTE
                    riscoLetra = "E";
                else if (p.PROD_DETALHE == 3) //FORRO
                    riscoLetra = "F";
                else if (p.PROD_DETALHE == 6) //COR 3
                    riscoLetra = "CC";

                var c = prodController.ObterGradeCorteHB(codigoHB);
                if (c != null && c.Count() > 0)
                    riscoPosicao = (c.Count() + 1).ToString();

                txtRiscoNome.Text = txtHB.Text.Trim() + "" + p.MOSTRUARIO.ToString() + "" + ddlColecoes.SelectedValue.Trim() + "" + riscoLetra + riscoPosicao;
            }
        }
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

                gvGradeCorteHBBaixa.DataSource = _list;
                gvGradeCorteHBBaixa.DataBind();
            }
        }
        private void ExcluirGradeCorteHB(int _Codigo)
        {
            prodController.ExcluirGradeCorteHB(_Codigo);
        }
        protected void btIncluirCorte_Click(object sender, EventArgs e)
        {
            int codigoCorte = 0;
            int codigoHB = 0;

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
            if (txtCorteEnfesto.Text.Trim() == "")
            {
                labErroGradeSub.Text = "Informe o Enfestador.";
                return;
            }
            if (txtCorteCorte.Text.Trim() == "")
            {
                labErroGradeSub.Text = "Informe o Cortador.";
                return;
            }
            if (txtCorteAmarrador.Text.Trim() == "")
            {
                labErroGradeSub.Text = "Informe o Amarrador.";
                return;
            }
             * */
            try
            {

                codigoHB = Convert.ToInt32(hidHB.Value);


                var _corte = new PROD_HB_CORTE();
                _corte.PROD_HB = codigoHB;
                _corte.GASTO = Convert.ToDecimal(txtCorteGasto.Text.Trim());
                _corte.FOLHA = Convert.ToInt32(txtCorteFolha.Text.Trim());
                _corte.RISCO_NOME = txtRiscoNome.Text.Trim().ToUpper();
                _corte.GASTO_KGS = CalcularGastoKG(_corte.PROD_HB, Convert.ToDecimal(_corte.GASTO));

                codigoCorte = IncluirGradeCorte(_corte);

                List<PROD_HB_GRADE_SUB> _list = new List<PROD_HB_GRADE_SUB>();
                _list = prodController.ObterGradeSubHB(codigoHB);

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
                //txtCorteEnfesto.Text = "";
                //txtCorteCorte.Text = "";
                //txtCorteAmarrador.Text = "";
                RecarregarGradeSub();
                RecarregarGradeCorteHB();

                PreencherGradeReal(codigoHB, false);

                MontarRiscoNome();

                //Tem que fazer independente do POST BACK
                RecarregarGVCorteBaixaDropDownList();
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
                    int codigoHB = 0;

                    codigoHB = Convert.ToInt32(b.CommandArgument);

                    ExcluirGradeCorteHB(codigoHB);
                    RecarregarGradeCorteHB();

                    PreencherGradeReal(codigoHB, false);

                    MontarRiscoNome();
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
                    }
                }
            }
        }

        protected void txtNomeRisco_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox _txt = (TextBox)sender;
                if (_txt != null)
                {
                    string nomeRisco = _txt.Text.Trim().ToUpper();

                    GridViewRow row = (GridViewRow)_txt.NamingContainer;
                    if (row != null)
                    {
                        int codigo = Convert.ToInt32(gvGradeCorteHB.DataKeys[row.RowIndex].Value);

                        var corte = prodController.ObterGradeCorte(codigo);
                        if (corte != null)
                        {
                            corte.RISCO_NOME = nomeRisco;
                            prodController.AtualizarGradeCorte(corte);

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void txtFolha_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox _txt = (TextBox)sender;
                if (_txt != null)
                {
                    int _folhas = Convert.ToInt32(_txt.Text);

                    GridViewRow row = (GridViewRow)_txt.NamingContainer;
                    if (row != null)
                    {
                        int codigo = Convert.ToInt32(gvGradeCorteHBBaixa.DataKeys[row.RowIndex].Value);

                        var corte = prodController.ObterGradeCorte(codigo);
                        if (corte != null)
                        {
                            corte.FOLHA = _folhas;
                            prodController.AtualizarGradeCorte(corte);

                            PreencherGradeReal(corte.PROD_HB, true);

                            ValidarDiferencaGradesReal();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private decimal CalcularGastoKG(int codigoHB, decimal gastoMT)
        {
            var phb = prodController.ObterHB(codigoHB);
            if (phb != null && phb.RENDIMENTO_1MT != null)
                return Convert.ToDecimal(phb.RENDIMENTO_1MT) * gastoMT;

            return 0;
        }
        #endregion

        #region "HB"
        private void ValidarDiferencaGrades()
        {
            txtGradeEXP.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeEXP.Text) < Convert.ToInt32(txtGradeEXPIni.Text))
                txtGradeEXP.BackColor = Color.Orange;

            txtGradeXP.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeXP.Text) < Convert.ToInt32(txtGradeXPIni.Text))
                txtGradeXP.BackColor = Color.Orange;

            txtGradePP.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradePP.Text) < Convert.ToInt32(txtGradePPIni.Text))
                txtGradePP.BackColor = Color.Orange;

            txtGradeP.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeP.Text) < Convert.ToInt32(txtGradePIni.Text))
                txtGradeP.BackColor = Color.Orange;

            txtGradeM.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeM.Text) < Convert.ToInt32(txtGradeMIni.Text))
                txtGradeM.BackColor = Color.Orange;

            txtGradeG.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeG.Text) < Convert.ToInt32(txtGradeGIni.Text))
                txtGradeG.BackColor = Color.Orange;

            txtGradeGG.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeGG.Text) < Convert.ToInt32(txtGradeGGIni.Text))
                txtGradeGG.BackColor = Color.Orange;

            txtGradeTotal.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeTotal.Text) < Convert.ToInt32(txtGradeTotalIni.Text))
                txtGradeTotal.BackColor = Color.Orange;
        }
        private void ValidarDiferencaGradesReal()
        {
            txtGradeEXP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeEXP_R.Text) < Convert.ToInt32(txtGradeEXPIni.Text))
                txtGradeEXP_R.BackColor = Color.Orange;

            txtGradeXP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeXP_R.Text) < Convert.ToInt32(txtGradeXPIni.Text))
                txtGradeXP_R.BackColor = Color.Orange;

            txtGradePP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradePP_R.Text) < Convert.ToInt32(txtGradePPIni.Text))
                txtGradePP_R.BackColor = Color.Orange;

            txtGradeP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeP_R.Text) < Convert.ToInt32(txtGradePIni.Text))
                txtGradeP_R.BackColor = Color.Orange;

            txtGradeM_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeM_R.Text) < Convert.ToInt32(txtGradeMIni.Text))
                txtGradeM_R.BackColor = Color.Orange;

            txtGradeG_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeG_R.Text) < Convert.ToInt32(txtGradeGIni.Text))
                txtGradeG_R.BackColor = Color.Orange;

            txtGradeGG_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeGG_R.Text) < Convert.ToInt32(txtGradeGGIni.Text))
                txtGradeGG_R.BackColor = Color.Orange;

            txtGradeTotal_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeTotal_R.Text) < Convert.ToInt32(txtGradeTotalIni.Text))
                txtGradeTotal_R.BackColor = Color.Orange;
        }
        private void PreencherGradeReal(int prod_hb, bool gradeReal)
        {
            var gReal = prodController.CalcularGradeReal(prod_hb);
            if (gReal != null)
            {
                int totalGrade = (Convert.ToInt32(gReal.GRADE_EXP) + Convert.ToInt32(gReal.GRADE_XP) + Convert.ToInt32(gReal.GRADE_PP) + Convert.ToInt32(gReal.GRADE_P) + Convert.ToInt32(gReal.GRADE_M) + Convert.ToInt32(gReal.GRADE_G) + Convert.ToInt32(gReal.GRADE_GG));

                if (!gradeReal)
                {
                    txtGradeEXP.Text = gReal.GRADE_EXP.ToString();
                    txtGradeXP.Text = gReal.GRADE_XP.ToString();
                    txtGradePP.Text = gReal.GRADE_PP.ToString();
                    txtGradeP.Text = gReal.GRADE_P.ToString();
                    txtGradeM.Text = gReal.GRADE_M.ToString();
                    txtGradeG.Text = gReal.GRADE_G.ToString();
                    txtGradeGG.Text = gReal.GRADE_GG.ToString();
                    txtGradeTotal.Text = totalGrade.ToString();
                }
                else
                {
                    //Preenche grade real
                    txtGradeEXP_R.Text = gReal.GRADE_EXP.ToString();
                    txtGradeXP_R.Text = gReal.GRADE_XP.ToString();
                    txtGradePP_R.Text = gReal.GRADE_PP.ToString();
                    txtGradeP_R.Text = gReal.GRADE_P.ToString();
                    txtGradeM_R.Text = gReal.GRADE_M.ToString();
                    txtGradeG_R.Text = gReal.GRADE_G.ToString();
                    txtGradeGG_R.Text = gReal.GRADE_GG.ToString();
                    txtGradeTotal_R.Text = totalGrade.ToString();

                    ValidarDiferencaGradesReal();
                }
            }
            else
            {
                if (!gradeReal)
                {
                    txtGradeEXP.Text = "0";
                    txtGradeXP.Text = "0";
                    txtGradePP.Text = "0";
                    txtGradeP.Text = "0";
                    txtGradeM.Text = "0";
                    txtGradeG.Text = "0";
                    txtGradeGG.Text = "0";
                    txtGradeTotal.Text = "0";
                }
            }

            ValidarDiferencaGrades();
        }
        private PROD_HB ObterHB(int codigo)
        {
            return prodController.ObterHB(codigo);
        }
        private void AtualizarHB(PROD_HB _hb)
        {
            prodController.AtualizarHB(_hb);
        }
        private void AtualizarBaixaHB(PROD_HB _hb)
        {
            prodController.AtualizarBaixaHB(_hb);
        }
        private void AtualizarProcessoDataFim(int _prod_hb, int _processo, DateTime _dataBaixa)
        {
            prodController.AtualizarProcessoDataFim(_prod_hb, _processo, _dataBaixa);
        }
        private void IncluirGrade(PROD_HB_GRADE _grade)
        {
            prodController.InserirGrade(_grade);
        }
        private void IncluirProcesso(PROD_HB_PROD_PROCESSO _processo)
        {
            prodController.InserirProcesso(_processo);
        }
        private int ObterProcessoOrdem(int _ordem)
        {
            return prodController.ObterProcessoOrdem(_ordem).CODIGO;
        }
        private void BaixarRisco()
        {
            if (gvGradeCorteHB.Rows.Count <= 0)
                throw new Exception("Informe pelo menos uma Grade.");

            var gradePrevista = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 2);
            if (gradePrevista != null)
            {
                gradePrevista.GRADE_EXP = Convert.ToInt32(txtGradeEXP.Text);
                gradePrevista.GRADE_XP = Convert.ToInt32(txtGradeXP.Text);
                gradePrevista.GRADE_PP = Convert.ToInt32(txtGradePP.Text);
                gradePrevista.GRADE_P = Convert.ToInt32(txtGradeP.Text);
                gradePrevista.GRADE_M = Convert.ToInt32(txtGradeM.Text);
                gradePrevista.GRADE_G = Convert.ToInt32(txtGradeG.Text);
                gradePrevista.GRADE_GG = Convert.ToInt32(txtGradeGG.Text);

                prodController.AtualizarGrade(gradePrevista, 2);
            }

            //Baixa primeiro processo
            AtualizarProcessoDataFim(Convert.ToInt32(hidHB.Value), ObterProcessoOrdem(2), Convert.ToDateTime(hidData.Value));

            //Iniciar segundo processo
            var processoExiste = prodController.ObterProcessoHB(Convert.ToInt32(hidHB.Value), 3);
            if (processoExiste == null)
            {
                var processo = new PROD_HB_PROD_PROCESSO();
                processo.PROD_HB = Convert.ToInt32(hidHB.Value);
                processo.PROD_PROCESSO = ObterProcessoOrdem(3); //Obter o terceiro processo
                processo.DATA_INICIO = Convert.ToDateTime(Convert.ToDateTime(hidData.Value).ToString("yyyy-MM-dd ") + DateTime.Now.ToString("HH:mm:ss"));
                IncluirProcesso(processo);
            }
        }
        private bool BaixarCorte()
        {

            var detHB = prodController.ObterHB(Convert.ToInt32(hidHB.Value));

            //Validação de nulos
            if (!ValidarCampos(detHB))
                throw new Exception("Preencha corretamente os campos em <strong>vermelho</strong>.");

            //Inserir grade
            var gradeExiste = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 3);
            if (gradeExiste == null)
            {
                var grade = new PROD_HB_GRADE();
                grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                grade.PROD_PROCESSO = 3; //GRADE DO RISCO
                grade.GRADE_EXP = (txtGradeEXP_R.Text != "") ? Convert.ToInt32(txtGradeEXP_R.Text) : 0;
                grade.GRADE_XP = (txtGradeXP_R.Text != "") ? Convert.ToInt32(txtGradeXP_R.Text) : 0;
                grade.GRADE_PP = (txtGradePP_R.Text != "") ? Convert.ToInt32(txtGradePP_R.Text) : 0;
                grade.GRADE_P = (txtGradeP_R.Text != "") ? Convert.ToInt32(txtGradeP_R.Text) : 0;
                grade.GRADE_M = (txtGradeM_R.Text != "") ? Convert.ToInt32(txtGradeM_R.Text) : 0;
                grade.GRADE_G = (txtGradeG_R.Text != "") ? Convert.ToInt32(txtGradeG_R.Text) : 0;
                grade.GRADE_GG = (txtGradeGG_R.Text != "") ? Convert.ToInt32(txtGradeGG_R.Text) : 0;
                grade.DATA_INCLUSAO = DateTime.Now;
                grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                IncluirGrade(grade);
            }

            //Atualizar grade corte
            PROD_HB_CORTE corte = null;
            foreach (GridViewRow r in gvGradeCorteHBBaixa.Rows)
            {
                string codigo = gvGradeCorteHBBaixa.DataKeys[r.RowIndex].Value.ToString();

                corte = new PROD_HB_CORTE();
                corte.CODIGO = Convert.ToInt32(codigo);
                corte.PROD_HB = Convert.ToInt32(hidHB.Value);

                TextBox _folha = r.FindControl("txtFolha") as TextBox;
                if (_folha != null)
                    corte.FOLHA = Convert.ToInt32(_folha.Text);

                DropDownList _enfesto1 = r.FindControl("ddlEnfesto1") as DropDownList;
                if (_enfesto1 != null)
                    corte.ENFESTO = _enfesto1.SelectedItem.Text.Trim().ToUpper();
                DropDownList _enfesto2 = r.FindControl("ddlEnfesto2") as DropDownList;
                if (_enfesto2 != null)
                    corte.ENFESTO = corte.ENFESTO + ((_enfesto2.SelectedItem.Text == "") ? "" : ("/" + _enfesto2.SelectedItem.Text));
                DropDownList _enfesto3 = r.FindControl("ddlEnfesto3") as DropDownList;
                if (_enfesto3 != null)
                    corte.ENFESTO = corte.ENFESTO + ((_enfesto3.SelectedItem.Text == "") ? "" : ("/" + _enfesto3.SelectedItem.Text));

                DropDownList _corte1 = r.FindControl("ddlCorte1") as DropDownList;
                if (_corte1 != null)
                    corte.CORTE = _corte1.SelectedItem.Text.Trim().ToUpper();
                DropDownList _corte2 = r.FindControl("ddlCorte2") as DropDownList;
                if (_corte2 != null)
                    corte.CORTE = corte.CORTE + ((_corte2.SelectedItem.Text == "") ? "" : ("/" + _corte2.SelectedItem.Text));

                DropDownList _amarrador1 = r.FindControl("ddlAmarrador1") as DropDownList;
                if (_amarrador1 != null)
                    corte.AMARRADOR = _amarrador1.SelectedItem.Text.Trim().ToUpper();
                DropDownList _amarrador2 = r.FindControl("ddlAmarrador2") as DropDownList;
                if (_amarrador2 != null)
                    corte.AMARRADOR = corte.AMARRADOR + ((_amarrador2.SelectedItem.Text == "") ? "" : ("/" + _amarrador2.SelectedItem.Text));

                AtualizarGradeCorte(corte);
            }

            PROD_HB _prod_hb = new PROD_HB();
            _prod_hb.CODIGO = Convert.ToInt32(hidHB.Value);
            _prod_hb.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);
            _prod_hb.GASTO_FOLHA = Convert.ToDecimal(txtGastoPorFolha.Text) * Convert.ToDecimal(1.07);
            if (txtGastoPorPecaCusto.Text.Trim() != "")
                _prod_hb.GASTO_PECA_CUSTO = Convert.ToDecimal(txtGastoPorPecaCusto.Text);
            _prod_hb.RETALHOS = Convert.ToDecimal(txtGastoRetalhos.Text);
            _prod_hb.VOLUME = Convert.ToInt32(txtVolume.Text);
            _prod_hb.STATUS = 'B'; //BAIXADO

            AtualizarBaixaHB(_prod_hb);

            //Baixa primeiro processo
            AtualizarProcessoDataFim(_prod_hb.CODIGO, ObterProcessoOrdem(3), Convert.ToDateTime(hidData.Value));
            //FIM ATUALIZAÇÃO

            //OBTER DETALHE ATUAL
            var _det = prodController.ObterHB(Convert.ToInt32(hidHB.Value));
            //OBTER PAI DOS DETALHES
            var prod_hb = prodController.ObterHB(Convert.ToInt32(_det.CODIGO_PAI));
            //OBTER OUTROS DETALHES
            //var _detalhes = prodController.ObterDetalhesHB(Convert.ToInt32(_det.CODIGO_PAI));

            //CRIAR FICHA TECNICA DO DETALHE
            var produtoPrincipal = desenvController.ObterProduto(prod_hb.COLECAO, prod_hb.CODIGO_PRODUTO_LINX, prod_hb.COR);
            CriarFichaTecnica(produtoPrincipal, _det);

            //bool detBaixado = true;
            //foreach (var d in _detalhes.Where(p => p.CODIGO != _det.CODIGO))
            //    if (d != null && d.STATUS != 'B')
            //        detBaixado = false;

            //if (detBaixado)
            //{

            //    // SE PEDIDO DO PRINCIPAL FOR 2028, não gera controle de produção, pois será feito em outro lugar
            //    if (prod_hb.NUMERO_PEDIDO != 2028)
            //    {
            //        //Gerar registro de Facção
            //        var validaSaida = new FaccaoController().ObterSaidaHB(_det.CODIGO_PAI.ToString());
            //        if (validaSaida == null || validaSaida.Count() <= 0)
            //        {
            //            PROD_HB_SAIDA _saida = new PROD_HB_SAIDA();
            //            _saida.PROD_HB = Convert.ToInt32(_det.CODIGO_PAI);
            //            _saida.DATA_INCLUSAO = DateTime.Now;
            //            _saida.PROD_PROCESSO = 20;
            //            var gradePrincipal = prodController.ObterGradeHB(Convert.ToInt32(_det.CODIGO_PAI), 3);
            //            _saida.GRADE_EXP = (gradePrincipal != null) ? gradePrincipal.GRADE_EXP : 0;
            //            _saida.GRADE_XP = (gradePrincipal != null) ? gradePrincipal.GRADE_XP : 0;
            //            _saida.GRADE_PP = (gradePrincipal != null) ? gradePrincipal.GRADE_PP : 0;
            //            _saida.GRADE_P = (gradePrincipal != null) ? gradePrincipal.GRADE_P : 0;
            //            _saida.GRADE_M = (gradePrincipal != null) ? gradePrincipal.GRADE_M : 0;
            //            _saida.GRADE_G = (gradePrincipal != null) ? gradePrincipal.GRADE_G : 0;
            //            _saida.GRADE_GG = (gradePrincipal != null) ? gradePrincipal.GRADE_GG : 0;
            //            _saida.GRADE_TOTAL = (_saida.GRADE_EXP + _saida.GRADE_XP + _saida.GRADE_PP + _saida.GRADE_P + _saida.GRADE_M + _saida.GRADE_G + _saida.GRADE_GG);
            //            _saida.SALDO = 'S';
            //            new FaccaoController().InserirSaidaHB(_saida);

            //            try
            //            {
            //                var principal = prodController.ObterHB(Convert.ToInt32(_det.CODIGO_PAI));
            //                if (principal != null)
            //                {
            //                    var retornoRota = faccController.GerarRotaModeloProduto(principal.CODIGO_PRODUTO_LINX, principal.COR, true, false, cbEstamparia.Checked, cbLavanderia.Checked, cbFaccao.Checked, cbAcabamento.Checked, true, true, false, false, false, false, false);

            //                    if (retornoRota.MENSAGEM_ERRO.Trim() != "")
            //                    {
            //                        throw new Exception("ERRO (GerarRotaModeloProduto): COD. " + retornoRota.CODIGO_ERRO + ", MSG. " + retornoRota.MENSAGEM_ERRO);
            //                    }

            //                    var retornoFacc = faccController.GerarOrdemProducao(principal.CODIGO);

            //                    if (retornoFacc == null || retornoFacc.MENSAGEM_ERRO.Trim() != "")
            //                    {
            //                        throw new Exception("ERRO (GerarOrdemProducao): COD. " + retornoFacc.CODIGO_ERRO + ", MSG. " + retornoFacc.MENSAGEM_ERRO);
            //                    }

            //                    principal.ORDEM_PRODUCAO = retornoFacc.CODIGO_ERRO.ToString();
            //                    prodController.AtualizarHB(principal);
            //                }

            //            }
            //            catch (Exception ex)
            //            {
            //                throw ex;
            //            }

            //        }
            //    }

            //    //GERAR CUSTO PARA SIMULACAO
            //    if (prod_hb != null)
            //    {
            //        var custoSimulacao = prodController.ObterCustoSimulacao(prod_hb.CODIGO_PRODUTO_LINX, Convert.ToChar(prod_hb.MOSTRUARIO));
            //        if (custoSimulacao == null || custoSimulacao.Count() <= 0)
            //        {
            //            if (prod_hb.MOSTRUARIO == 'S')
            //            {
            //                GerarCustoZerado(prod_hb, 'S');
            //                GerarCustoZerado(prod_hb, 'N');
            //            }
            //            else
            //            {
            //                GerarCustoZerado(prod_hb, 'N');
            //            }
            //        }
            //    }
            //}

            return true;
        }
        private void GerarCustoZerado(PROD_HB prod_hb, char tipo)
        {

            //Gerar custo
            PRODUTO_CUSTO_SIMULACAO simulacao = new PRODUTO_CUSTO_SIMULACAO();
            simulacao.PRODUTO = prod_hb.CODIGO_PRODUTO_LINX;
            simulacao.PRECO_PRODUTO = 0;
            simulacao.MARKUP = 3;
            simulacao.TT_PORC = 0;
            simulacao.TT_IMPOSTO = 0;
            simulacao.ICMS = 0;
            simulacao.CUSTO_COM_IMPOSTO = 0;
            simulacao.LUCRO_PORC = 0;
            simulacao.DATA_INCLUSAO = DateTime.Now;
            simulacao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            simulacao.SIMULACAO = 'S';
            simulacao.MOSTRUARIO = Convert.ToChar(prod_hb.MOSTRUARIO);
            simulacao.CUSTO_MOSTRUARIO = tipo;
            simulacao.FINALIZADO = 'N';

            prodController.InserirCustoSimulacao(simulacao);

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

                }
            }
        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int totalGrade = 0;

                if (hidTela.Value == "2")
                {
                    BaixarRisco();
                    labConteudoDiv.Text = "RISCADO COM SUCESSO";

                    var p = prodController.ObterHB(Convert.ToInt32(hidHB.Value));

                    if (p.LIQUIDAR == 'N')
                    {
                        List<PROD_HB> _prod_HB = new List<PROD_HB>();

                        //RELATORIO
                        RelatorioHB _relatorio = new RelatorioHB();

                        //DETALHE
                        List<PROD_HB> detalhe = new List<PROD_HB>();
                        detalhe.Add(p);

                        _prod_HB.Add(prodController.ObterHB(Convert.ToInt32(detalhe[0].CODIGO_PAI)));

                        _relatorio.DETALHE = detalhe;
                        _relatorio.PROD_GRADE = prodController.ObterHB(Convert.ToInt32(detalhe[0].CODIGO_PAI)).PROD_GRADE.ToString();

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

                        GerarRelatorio(_relatorio);
                    }
                }
                else
                {
                    BaixarCorte();
                    labConteudoDiv.Text = "CORTADO COM SUCESSO";
                }

                labHBPopUp.Text = txtHB.Text;
                labConteudoDetalhe.Text = labDetalheTitulo.Text;
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
        private bool ValidarCampos(PROD_HB det)
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

            labTecido.ForeColor = _OK;
            if (txtTecido.Text.Trim() == "")
            {
                labTecido.ForeColor = _notOK;
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

            labVolume.ForeColor = _OK;
            if (txtVolume.Text.Trim() == "" || Convert.ToDecimal(txtVolume.Text.Trim()) < 0)
            {
                labVolume.ForeColor = _notOK;
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

            labGradeCorteBaixa.ForeColor = _OK;
            if (gvGradeCorteHBBaixa.Rows.Count <= 0 && det.PROD_DETALHE != 4 && det.PROD_DETALHE != 5)
            {
                labGradeCorteBaixa.ForeColor = _notOK;
                retorno = false;
            }

            labGradeCorteBaixa.ForeColor = _OK;
            if (ValidarGridCorte() <= 0 && det.PROD_DETALHE != 4 && det.PROD_DETALHE != 5)
            {
                labGradeCorteBaixa.ForeColor = _notOK;
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
        #endregion

        #region "RELATORIO"
        private void GerarRelatorio(RelatorioHB _relatorio)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "HB_DETRISCO" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
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

            // Cabecalho
            _texto = _relatorio.MontarCabecalho(_texto, "Detalhes - Ficha Risco");
            // Detalhes
            _texto = _relatorio.MontarDetalhe(_texto, _relatorio, "breakafter");
            // Rodape
            _texto = _relatorio.MontarRodape(_texto);

            return _texto;
        }
        #endregion


    }
}
