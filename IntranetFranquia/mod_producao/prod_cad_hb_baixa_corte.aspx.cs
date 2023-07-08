using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using DAL;
using System.Text;
using System.Collections;

namespace Relatorios.mod_producao
{
    public partial class prod_cad_hb_baixa_corte : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        FaccaoController faccController = new FaccaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int colunaComposicao, colunaAviamento, colunaDetalhe, colunagradesub, colunagradecortehb, colunagradecortehbbaixa = 0;

        List<UNIDADE_MEDIDA> _unidadeMedida = null;

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
                    litConteudoDiv.Text = "NÃO ENCONTRADO";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Ok: function () { window.open('prod_menu.aspx', '_self'); } } }); });", true);
                    return;
                }

                hidMostruario.Value = prod_hb.MOSTRUARIO.ToString();

                if (hidTela.Value == "3") //ACESSANDO ESTA TELA DA BAIXA DO CORTE
                {
                    divBaixaCorte.Visible = true;
                    hrefVoltar.HRef = "prod_fila_corte.aspx";
                    trGradeReal.Visible = true;
                    fsRisco.Visible = false;
                    fsCorteBaixa.Visible = true;

                    if (hidMostruario.Value == "S")
                        hrefVoltar.HRef = "prod_fila_mostruario.aspx";

                }

                //Carregar Combos
                CarregarGrupo();
                CarregarCores();
                CarregarColecoes();
                CarregarFornecedores();
                CarregarUnidadeMedida();

                if (prod_hb != null)
                {
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
                    cbLiquidar.Checked = (prod_hb.LIQUIDAR == 'S') ? true : false;
                    cbLiquidar.Enabled = false;
                    txtModelagem.Text = prod_hb.MODELAGEM;
                    txtModelagem.ReadOnly = true;
                    txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                    txtCustoTecido.ReadOnly = true;
                    imgFotoTecido.ImageUrl = prod_hb.FOTO_TECIDO;

                    txtCodigoLinx.Text = prod_hb.CODIGO_PRODUTO_LINX.ToString();
                    txtCodigoLinx.ReadOnly = true;

                    txtPedidoNumero.Text = prod_hb.NUMERO_PEDIDO.ToString();
                    txtPedidoNumero.ReadOnly = true;

                    cbAtacado.Checked = (prod_hb.ATACADO == 'S') ? true : false;
                    cbAtacado.Enabled = false;

                    if (prod_hb.GABARITO != null)
                        ddlGabarito.SelectedValue = prod_hb.GABARITO.ToString();

                    if (prod_hb.ESTAMPARIA != null && prod_hb.ESTAMPARIA == 'S')
                        cbEstamparia.Checked = true;

                    if (prod_hb.MOSTRUARIO == 'S')
                    {
                        txtVolume.Text = "0";
                        txtVolume.Enabled = false;
                    }

                    if (cbAtacado.Checked && hidTela.Value == "3")
                    {
                        fsGradeAtacado.Visible = true;
                        PreencherGradeAtacado(txtCodigoLinx.Text.Trim());
                    }

                    gvComposicao.DataSource = prodController.ObterComposicaoHB(Convert.ToInt32(codigo));
                    gvComposicao.DataBind();

                    CarregarNomeGrade(prod_hb);

                    PROD_HB_GRADE _grade = prodController.ObterGradeHB(Convert.ToInt32(codigo), 2);
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

                        int totalGrade = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG));
                        txtGradeTotal.Text = totalGrade.ToString();
                        txtGradeTotal.Enabled = false;

                        PreencherGradeReal(prod_hb.CODIGO);
                        ValidarDiferencaGradesReal();
                    }

                    txtGastoPorCorte.Attributes.Add("readonly", "readonly");
                    txtGastoPorPeca.Attributes.Add("readonly", "readonly");
                    txtGradeTotal_R.Attributes.Add("readonly", "readonly");

                    List<PROD_HB> detalhes;
                    detalhes = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));
                    gvDetalhe.DataSource = detalhes;
                    gvDetalhe.DataBind();

                    var rotaExiste = new FaccaoController().ObterTabelaOperacao(prod_hb.CODIGO_PRODUTO_LINX);
                    //if (((detalhes == null || (detalhes.Where(p => p.PROD_DETALHE != 4 && p.PROD_DETALHE != 5).Count() <= 0)) || prod_hb.MOSTRUARIO == 'S') && rotaExiste == null)
                    if (rotaExiste == null)
                    {
                        fsFaccaoSemRota.Visible = true;
                    }
                    else
                    {
                        fsFaccaoComRota.Visible = true;
                        List<PRODUTO_OPERACOES_ROTA> rotas = new ProducaoController().ObterRotaFases(rotaExiste.TABELA_OPERACOES);
                        if (rotas != null)
                        {
                            foreach (PRODUTO_OPERACOES_ROTA rota in rotas)
                            {
                                var fase = new ProducaoController().ObterFase(rota.FASE_PRODUCAO);
                                if (fase != null)
                                {
                                    if (fase.DESC_FASE_PRODUCAO.Trim() == "FACCAO")
                                    {
                                        lblFaccao.Text = "SIM";
                                    }
                                    else if (fase.DESC_FASE_PRODUCAO.Trim() == "ESTAMPARIA")
                                    {
                                        lblEstamparia.Text = "SIM";
                                    }
                                    else if (fase.DESC_FASE_PRODUCAO.Trim() == "LAVANDERIA")
                                    {
                                        lblLavanderia.Text = "SIM";
                                    }
                                    else if (fase.DESC_FASE_PRODUCAO.Trim() == "ACABAMENTO")
                                    {
                                        lblAcabamento.Text = "SIM";
                                    }
                                }
                            }
                        }
                    }


                    //Se nao for mostruario, esconde as colunas dos detalhe
                    if (hidMostruario.Value == "N")
                    {
                        gvDetalhe.Columns[3].Visible = false;
                        gvDetalhe.Columns[4].Visible = false;
                        gvDetalhe.Columns[5].Visible = false;
                        gvDetalhe.Columns[6].Visible = false;
                        gvDetalhe.Columns[7].Visible = false;
                        gvDetalhe.Columns[8].Visible = false;

                        labGastoPorPecaCusto.Visible = false;
                        txtGastoPorPecaCusto.Visible = false;
                        labPecaGalaoCusto.Visible = false;
                        txtGastoPorPecaGalaoCusto.Visible = false;
                        labPecaGalaoCustoProprio.Visible = false;
                        txtGastoPorPecaGalaoCustoProprio.Visible = false;
                    }

                    //if (prod_hb.LIQUIDAR == 'S')
                    //{
                    //    var galao = detalhes.Where(i => i.PROD_DETALHE == 4);
                    //    if ((galao != null && galao.Count() > 0) && hidMostruario.Value == "N")
                    //    {
                    //        pnlGastoDetalhe.Visible = true;
                    //        labGastoGalao.Text = "Gasto Galão";

                    //        ddlUnidadeGalao.SelectedValue = RetornarUnidadeMedidaMaterial(galao.SingleOrDefault().GRUPO_TECIDO.Trim(), galao.SingleOrDefault().TECIDO.Trim());
                    //    }

                    //    var galaoProprio = detalhes.Where(i => i.PROD_DETALHE == 5);
                    //    if ((galaoProprio != null && galaoProprio.Count() > 0) && hidMostruario.Value == "N")
                    //    {
                    //        pnlGastoDetalheGalaoProprio.Visible = true;
                    //        labGastoGalaoProprio.Text = "Gasto Galão Próprio";

                    //        ddlUnidadeGalaoProprio.SelectedValue = RetornarUnidadeMedidaMaterial(galaoProprio.SingleOrDefault().GRUPO_TECIDO.Trim(), galaoProprio.SingleOrDefault().TECIDO.Trim());
                    //    }
                    //}

                    CarregarAviamentos();

                    imgFotoPeca.ImageUrl = prod_hb.FOTO_PECA;
                    txtObservacao.Text = prod_hb.OBSERVACAO;

                    RecarregarGradeSub();
                    RecarregarGradeCorteHB();

                    ddlUnidade.SelectedValue = RetornarUnidadeMedidaMaterial(prod_hb.GRUPO_TECIDO.Trim(), prod_hb.TECIDO.Trim());
                    ddlUnidade.Enabled = false;

                    dialogPai.Visible = false;
                }

                //Tem que fazer independente do POST BACK
                RecarregarGVCorteBaixaDropDownList();

                /*Carregar dados do mostruario na baixa, se ja foi salvo a primeira vez*/
                if (hidMostruario.Value == "S")
                    PreencherDadosMostruarioBaixa(prod_hb);
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
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
                    list.Add(new ListItem(corteF.NOME, corteF.NOME.ToString()));
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
        private void CarregarTipoRisco()
        {
            List<PROD_HB> riscoDetalhe = new List<PROD_HB>();
            riscoDetalhe = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));

            List<ListItem> lstItem = new List<ListItem>();
            lstItem.Add(new ListItem { Value = hidHB.Value, Text = "PRINCIPAL" });

            ListItem item = null;
            foreach (PROD_HB risco in riscoDetalhe)
            {
                item = new ListItem();
                item.Value = risco.CODIGO.ToString();
                item.Text = risco.PROD_DETALHE1.DESCRICAO;
                lstItem.Add(item);
            }

            ddlRisco.DataSource = lstItem;
            ddlRisco.DataBind();
        }
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

                ddlUnidadeGalao.DataSource = _unidadeMedida;
                ddlUnidadeGalao.DataBind();

                ddlUnidadeGalaoProprio.DataSource = _unidadeMedida;
                ddlUnidadeGalaoProprio.DataBind();
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

                txtGradeP_A.Enabled = (_gradeNome.GRADE_P == "-") ? false : true;
                txtGradeM_A.Enabled = (_gradeNome.GRADE_M == "-") ? false : true;
                txtGradeG_A.Enabled = (_gradeNome.GRADE_G == "-") ? false : true;
                txtGradeGG_A.Enabled = (_gradeNome.GRADE_GG == "-") ? false : true;
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
                ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_P, Text = _gradeNome.GRADE_P });
                ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_M, Text = _gradeNome.GRADE_M });
                ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_G, Text = _gradeNome.GRADE_G });
                ddlGradeNumero.Items.Add(new ListItem { Value = _gradeNome.GRADE_GG, Text = _gradeNome.GRADE_GG });
                ddlGradeNumero.DataBind();
            }

        }
        private string RetornarUnidadeMedidaMaterial(string grupo, string subGrupo)
        {
            string retorno = "0";
            //PREENCHER UNIDADE DE MEDIDA AUTOMATICAMENTE
            var material = new DesenvolvimentoController().ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                                                                      p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();
            if (material != null && material.Count() > 0)
            {
                if (material[0].UNIDADE.UNIDADE1.Trim() == "KG")
                    retorno = "3";
                else if (material[0].UNIDADE.UNIDADE1.Trim() == "MT")
                    retorno = "1";
                else if (material[0].UNIDADE.UNIDADE1.Trim() == "UN")
                    retorno = "2";
            }

            return retorno;
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
                            _DetalheDesc.Text = (_hb.PROD_DETALHE1 == null) ? "PRINCIPAL" : _hb.PROD_DETALHE1.DESCRICAO;

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
        private void AtualizarGrade(PROD_HB_GRADE _grade, int processo)
        {
            prodController.AtualizarGrade(_grade, processo);
        }
        private void AtualizarGradeDetalhe(int codigo)
        {
            PROD_HB_GRADE _grade = null;

            //Atualizar grade
            if (codigo > 0)
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(codigo);
                _grade.PROD_PROCESSO = 2; //GRADE DO RISCO
                _grade.GRADE_EXP = (txtGradeEXP_R.Text != "") ? Convert.ToInt32(txtGradeEXP_R.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP_R.Text != "") ? Convert.ToInt32(txtGradeXP_R.Text) : 0;
                _grade.GRADE_PP = (txtGradePP_R.Text != "") ? Convert.ToInt32(txtGradePP_R.Text) : 0;
                _grade.GRADE_P = (txtGradeP_R.Text != "") ? Convert.ToInt32(txtGradeP_R.Text) : 0;
                _grade.GRADE_M = (txtGradeM_R.Text != "") ? Convert.ToInt32(txtGradeM_R.Text) : 0;
                _grade.GRADE_G = (txtGradeG_R.Text != "") ? Convert.ToInt32(txtGradeG_R.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG_R.Text != "") ? Convert.ToInt32(txtGradeGG_R.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, 2);
            }
        }
        #endregion

        #region "AVIAMENTOS"
        private void CarregarAviamentos()
        {
            gvAviamento.DataSource = prodController.ObterAviamentoHB(Convert.ToInt32(hidHB.Value));
            gvAviamento.DataBind();
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal qtdeExtra = 0;
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
                            _labQtde.Text = _aviamento.QTDE.ToString().Replace(",000", "");

                        Label _labQtdePorPeca = e.Row.FindControl("labQtdePorPeca") as Label;
                        if (_labQtdePorPeca != null)
                            _labQtdePorPeca.Text = _aviamento.QTDE_POR_PECA.ToString().Replace(",000", "");

                        TextBox _txtQtdeExtra = e.Row.FindControl("txtQtdeExtra") as TextBox;
                        if (_txtQtdeExtra != null)
                        {
                            if (_aviamento.QTDE_POR_PECA != null)
                            {
                                if (_aviamento.GRUPO.Trim().ToUpper().Contains("ZIPPER") ||
                                    _aviamento.GRUPO.Trim().ToUpper().Contains("ZIPER") ||
                                    _aviamento.GRUPO.Trim().ToUpper().Contains("ZIPP") ||
                                    _aviamento.GRUPO.Trim().ToUpper().Contains("BOJO")
                                    )
                                {
                                    qtdeExtra = 0;
                                    e.Row.BackColor = Color.Pink;
                                }
                                else
                                {
                                    if (txtGradeTotal_R.Text.Trim() != "")
                                    {
                                        int gradeTotal = Convert.ToInt32(txtGradeTotal_R.Text.Trim());
                                        qtdeExtra = ((gradeTotal * Convert.ToDecimal(_aviamento.QTDE_POR_PECA)) - _aviamento.QTDE);

                                        string unidadeMedida = RetornarUnidadeMedidaMaterial(_aviamento.GRUPO, _aviamento.SUBGRUPO);
                                        if (qtdeExtra > 0 && (unidadeMedida != "2" && unidadeMedida != "0"))
                                            qtdeExtra = qtdeExtra + (qtdeExtra * 5 / 100);

                                    }
                                }
                            }
                            else
                            {
                                qtdeExtra = (_aviamento.QTDE_EXTRA != null) ? Convert.ToDecimal(_aviamento.QTDE_EXTRA) : 0;
                            }

                            _txtQtdeExtra.Text = qtdeExtra.ToString();
                        }

                        Label _labQtdeTotal = e.Row.FindControl("labQtdeTotal") as Label;
                        if (_labQtdeTotal != null)
                            _labQtdeTotal.Text = (_aviamento.QTDE + qtdeExtra).ToString("###,###,##0.00");


                    }
                }
            }
        }

        protected void txtQtdeExtra_TextChanged(object sender, EventArgs e)
        {
            TextBox _txt = (TextBox)sender;
            if (_txt != null)
            {
                GridViewRow row = (GridViewRow)_txt.NamingContainer;
                if (row != null)
                {
                    Label _labQtde = row.FindControl("labQtde") as Label;
                    Label _labQtdeTotal = row.FindControl("labQtdeTotal") as Label;

                    _labQtdeTotal.Text = (((_txt.Text != "") ? Convert.ToDecimal(_txt.Text.Trim()) : 0) + ((_labQtde.Text != "") ? Convert.ToDecimal(_labQtde.Text.Trim()) : 0)).ToString();

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
            if (ddlRisco.SelectedValue != "")
            {
                List<PROD_HB_GRADE_SUB> _list = new List<PROD_HB_GRADE_SUB>();
                _list = prodController.ObterGradeSubHB(Convert.ToInt32(ddlRisco.SelectedValue));
                if (_list != null)
                {
                    gvGradeSub.DataSource = _list;
                    gvGradeSub.DataBind();
                }
            }
        }
        protected void ddlRisco_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecarregarGradeSub();
        }
        protected void btGradeSubIncluir_Click(object sender, EventArgs e)
        {
            try
            {

                labErroGradeSub.Text = "";
                var validaGrade = prodController.ObterGradeSubHB(Convert.ToInt32(ddlRisco.SelectedValue));
                if (validaGrade != null && validaGrade.Where(p => p.GRADE.Trim() == ddlGradeNumero.SelectedValue.Trim() && p.LETRA.Trim() == ddlGradeLetra.SelectedValue.Trim()).Count() > 0)
                {
                    labErroGradeSub.Text = "Não é possível inserir mais de uma grade igual.";
                    return;
                }

                PROD_HB_GRADE_SUB _novo = new PROD_HB_GRADE_SUB();

                _novo.GRADE = ddlGradeNumero.SelectedValue;
                _novo.LETRA = ddlGradeLetra.SelectedValue;
                _novo.NUMERO = txtGradeNumeroDetalhe.Text.Trim();
                _novo.PROD_HB = Convert.ToInt32(ddlRisco.SelectedValue);

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
            //Obter Pai
            List<SP_OBTER_GRADE_CORTEResult> _list = prodController.ObterGradeCorteHB(Convert.ToInt32(hidHB.Value));

            if (_list != null)
            {
                gvGradeCorteHBBaixa.DataSource = _list;
                gvGradeCorteHBBaixa.DataBind();

                //Obter Filhos
                if (hidMostruario.Value == "S")
                {
                    List<PROD_HB> detalhes = new List<PROD_HB>();
                    detalhes = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));
                    foreach (PROD_HB d in detalhes)
                        _list.AddRange(prodController.ObterGradeCorteHB(d.CODIGO));
                }

                gvGradeCorteHB.DataSource = _list;
                gvGradeCorteHB.DataBind();
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

            try
            {
                var _corte = new PROD_HB_CORTE();
                _corte.PROD_HB = Convert.ToInt32(ddlRisco.SelectedValue);
                _corte.GASTO = Convert.ToDecimal(txtCorteGasto.Text.Trim());
                _corte.FOLHA = Convert.ToInt32(txtCorteFolha.Text.Trim());
                _corte.GASTO_KGS = CalcularGastoKG(_corte.PROD_HB, Convert.ToDecimal(_corte.GASTO));
                codigoCorte = IncluirGradeCorte(_corte);

                List<PROD_HB_GRADE_SUB> _list = new List<PROD_HB_GRADE_SUB>();
                _list = prodController.ObterGradeSubHB(Convert.ToInt32(ddlRisco.SelectedValue));

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
                RecarregarGradeSub();
                RecarregarGradeCorteHB();

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
                    ExcluirGradeCorteHB(Convert.ToInt32(b.CommandArgument));
                    RecarregarGradeCorteHB();

                    //Tem que fazer independente do POST BACK
                    RecarregarGVCorteBaixaDropDownList();
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

                        Literal _litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        if (_litDetalhe != null)
                            _litDetalhe.Text = _gradeCorte.DESCRICAO;

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

        protected void chkGradeCorte_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton cb = (RadioButton)sender;
            if (cb != null)
            {
                GridViewRow row = (GridViewRow)cb.NamingContainer;
                if (row != null)
                {
                    //limpando marcacoes antigas
                    foreach (GridViewRow oldrow in gvGradeCorteHBBaixa.Rows)
                        ((RadioButton)oldrow.FindControl("rdbGradeCorte")).Checked = false;

                    int codigoGradeCorte = 0;
                    codigoGradeCorte = Convert.ToInt32(gvGradeCorteHBBaixa.DataKeys[row.RowIndex].Value);
                    if (codigoGradeCorte > 0)
                    {
                        decimal gasto = 0;
                        var hbCorte = prodController.ObterGradeCorte(codigoGradeCorte);
                        if (hbCorte != null)
                            gasto = Convert.ToDecimal(hbCorte.GASTO);

                        int totalGrade = 0;
                        var hbGradeSub = prodController.ObterGradeCorteSubPorCorte(codigoGradeCorte);
                        if (hbGradeSub != null && hbGradeSub.Count() > 0)
                            totalGrade = hbGradeSub.Count();

                        if (totalGrade > 0)
                            txtGastoPorFolha.Text = (gasto / totalGrade).ToString("###,###,###,##0.000");

                        ((RadioButton)row.FindControl("rdbGradeCorte")).Checked = true;
                    }
                }
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

                            PreencherGradeReal(corte.PROD_HB);
                            CarregarAviamentos();

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
        private void ValidarDiferencaGradesReal()
        {
            txtGradeEXP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeEXP_R.Text) < Convert.ToInt32(txtGradeEXP.Text))
                txtGradeEXP_R.BackColor = Color.Orange;

            txtGradeXP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeXP_R.Text) < Convert.ToInt32(txtGradeXP.Text))
                txtGradeXP_R.BackColor = Color.Orange;

            txtGradePP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradePP_R.Text) < Convert.ToInt32(txtGradePP.Text))
                txtGradePP_R.BackColor = Color.Orange;

            txtGradeP_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeP_R.Text) < Convert.ToInt32(txtGradeP.Text))
                txtGradeP_R.BackColor = Color.Orange;

            txtGradeM_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeM_R.Text) < Convert.ToInt32(txtGradeM.Text))
                txtGradeM_R.BackColor = Color.Orange;

            txtGradeG_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeG_R.Text) < Convert.ToInt32(txtGradeG.Text))
                txtGradeG_R.BackColor = Color.Orange;

            txtGradeGG_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeGG_R.Text) < Convert.ToInt32(txtGradeGG.Text))
                txtGradeGG_R.BackColor = Color.Orange;

            txtGradeTotal_R.BackColor = Color.LightGray;
            if (Convert.ToInt32(txtGradeTotal_R.Text) < Convert.ToInt32(txtGradeTotal.Text))
                txtGradeTotal_R.BackColor = Color.Orange;
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
        private void AtualizarProdutoProducao(PROD_HB prod_hb, bool bMostruario, int gradeVarejo, int gradeAtacado)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();

            //OBTER PRODUTO
            var _produto = desenvController.ObterProduto(prod_hb.COLECAO).Where(p => p.MODELO.Trim() == prod_hb.CODIGO_PRODUTO_LINX.Trim() && p.COR.Trim() == prod_hb.COR.Trim()).FirstOrDefault();

            if (_produto != null)
            {

                //OBTER PRODUTO PRODUCAO LIBERADO
                var _produtoProducao = desenvController.ObterProdutoProducao(_produto.CODIGO).Where(p => (p.STATUS == 'L' || p.STATUS == 'P') && p.PROD_DETALHE == null && p.PROD_HB == null);

                if (bMostruario)
                {
                    //MOSTRUARIO
                    foreach (var pp in _produtoProducao.Where(p => p.TIPO == 'M'))
                    {
                        var _ppNovo = desenvController.ObterProdutoProducaoCodigo(pp.CODIGO);
                        if (_ppNovo != null)
                        {
                            _ppNovo.PROD_HB = prod_hb.CODIGO;
                            desenvController.AtualizarProdutoProducao(_ppNovo);
                        }
                    }
                }
                else
                {
                    //VAREJO
                    if (gradeVarejo > 0)
                    {
                        var v = _produtoProducao.Where(p => p.TIPO == 'V');
                        if (v != null && v.Count() > 0)
                        {
                            foreach (var pp in v)
                            {
                                var _ppNovo = desenvController.ObterProdutoProducaoCodigo(pp.CODIGO);
                                if (_ppNovo != null)
                                {
                                    _ppNovo.PROD_HB = prod_hb.CODIGO;
                                    desenvController.AtualizarProdutoProducao(_ppNovo);
                                }
                            }
                        }
                        else
                        {
                            _produto.QTDE = gradeVarejo;
                            _produto.QTDE_ATACADO = 0;
                            InserirQtdeProdutoProducao(_produto, prod_hb.CODIGO);
                        }
                    }

                    //ATACADO
                    if (prod_hb.ATACADO == 'S' && gradeAtacado > 0)
                    {
                        var a = _produtoProducao.Where(p => p.TIPO == 'A');
                        if (a != null && a.Count() > 0)
                        {
                            foreach (var pp in a)
                            {
                                var _ppNovo = desenvController.ObterProdutoProducaoCodigo(pp.CODIGO);
                                if (_ppNovo != null)
                                {
                                    _ppNovo.PROD_HB = prod_hb.CODIGO;
                                    desenvController.AtualizarProdutoProducao(_ppNovo);
                                }
                            }
                        }
                        else
                        {
                            _produto.QTDE = 0;
                            _produto.QTDE_ATACADO = gradeAtacado;
                            InserirQtdeProdutoProducao(_produto, prod_hb.CODIGO);
                        }
                    }
                }
            }
        }

        private void InserirQtdeProdutoProducao(DESENV_PRODUTO _produto, int codigoHB)
        {
            List<DESENV_PRODUTO_FICTEC> _fichaTecnica = null;
            _fichaTecnica = desenvController.ObterFichaTecnica(_produto.CODIGO);

            DESENV_PRODUTO_PRODUCAO _produtoProducao = null;
            int codigoProdutoProducao = 0;

            foreach (DESENV_PRODUTO_FICTEC ft in _fichaTecnica)
            {
                if (_produto.QTDE != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'V';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    _produtoProducao.STATUS = 'L';
                    _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                    _produtoProducao.USUARIO_LIBERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    _produtoProducao.PROD_HB = codigoHB;
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                }

                if (_produto.QTDE_ATACADO != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'A';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE_ATACADO);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    _produtoProducao.STATUS = 'L';
                    _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                    _produtoProducao.USUARIO_LIBERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    _produtoProducao.PROD_HB = codigoHB;
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                }
            }
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
        private void PreencherGradeAtacado(string codigoLinx) //txtCodigoLinx.Text
        {
            string griffe = "";
            PRODUTO produto = (new BaseController().BuscaProduto(codigoLinx));
            if (produto != null)
            {
                griffe = produto.GRIFFE.Trim().ToUpper();
                if (griffe != "")
                {
                    var gradeAtacado = prodController.ObterGradeAtacado().Where(p =>
                                                                                    p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() &&
                                                                                    p.GRUPO.Trim().ToUpper() == ddlGrupo.SelectedValue.Trim().ToUpper() &&
                                                                                    p.GRIFFE.Trim().ToUpper() == griffe).SingleOrDefault();
                    if (gradeAtacado != null)
                    {
                        txtGradeEXP_A.Text = gradeAtacado.GRADE_EXP.ToString();
                        txtGradeXP_A.Text = gradeAtacado.GRADE_XP.ToString();
                        txtGradePP_A.Text = gradeAtacado.GRADE_PP.ToString();
                        txtGradeP_A.Text = gradeAtacado.GRADE_P.ToString();
                        txtGradeM_A.Text = gradeAtacado.GRADE_M.ToString();
                        txtGradeG_A.Text = gradeAtacado.GRADE_G.ToString();
                        txtGradeGG_A.Text = gradeAtacado.GRADE_GG.ToString();

                        //Preencher total
                        txtGradeTotal_A.Text = Convert.ToInt32(gradeAtacado.GRADE_EXP + gradeAtacado.GRADE_XP + gradeAtacado.GRADE_PP + gradeAtacado.GRADE_P + gradeAtacado.GRADE_M + gradeAtacado.GRADE_G + gradeAtacado.GRADE_GG).ToString();
                    }
                }
            }
        }

        private PROD_HB_SAIDA GerarFaccaoInterno()
        {
            // obter grade para gerar faccao interno
            var faccaoSaida = new PROD_HB_SAIDA();
            faccaoSaida.PROD_HB = Convert.ToInt32(hidHB.Value);
            faccaoSaida.PROD_PROCESSO = 20;
            faccaoSaida.GRADE_EXP = (txtGradeEXP_F.Text != "") ? Convert.ToInt32(txtGradeEXP_F.Text) : 0;
            faccaoSaida.GRADE_XP = (txtGradeXP_F.Text != "") ? Convert.ToInt32(txtGradeXP_F.Text) : 0;
            faccaoSaida.GRADE_PP = (txtGradePP_F.Text != "") ? Convert.ToInt32(txtGradePP_F.Text) : 0;
            faccaoSaida.GRADE_P = (txtGradeP_F.Text != "") ? Convert.ToInt32(txtGradeP_F.Text) : 0;
            faccaoSaida.GRADE_M = (txtGradeM_F.Text != "") ? Convert.ToInt32(txtGradeM_F.Text) : 0;
            faccaoSaida.GRADE_G = (txtGradeG_F.Text != "") ? Convert.ToInt32(txtGradeG_F.Text) : 0;
            faccaoSaida.GRADE_GG = (txtGradeGG_F.Text != "") ? Convert.ToInt32(txtGradeGG_F.Text) : 0;
            faccaoSaida.GRADE_TOTAL = faccaoSaida.GRADE_EXP + faccaoSaida.GRADE_XP + faccaoSaida.GRADE_PP + faccaoSaida.GRADE_P + faccaoSaida.GRADE_M + faccaoSaida.GRADE_G + faccaoSaida.GRADE_GG;

            if (faccaoSaida.GRADE_TOTAL > 0)
            {
                faccaoSaida.CNPJ = "10680869000854";
                faccaoSaida.FORNECEDOR = "HANDBOOK";
                faccaoSaida.EMISSAO = DateTime.Now;
                faccaoSaida.CODIGO_FILIAL = "";
                faccaoSaida.NF_SAIDA = "";
                faccaoSaida.SERIE_NF = "";
                faccaoSaida.USUARIO_EMISSAO = 1144;
                faccaoSaida.VOLUME = 1;
                faccaoSaida.CUSTO = 0;
                faccaoSaida.PRECO = 0;
                faccaoSaida.PROD_SERVICO = 1;
                faccaoSaida.TIPO = 'I';
                faccaoSaida.USUARIO_LIBERACAO = 1144;
                faccaoSaida.DATA_LIBERACAO = DateTime.Now;
                faccaoSaida.DATA_INCLUSAO = DateTime.Now;
                faccaoSaida.SALDO = 'S';
                faccController.InserirSaidaHB(faccaoSaida);

                var faccaoEntrada = new PROD_HB_ENTRADA();
                faccaoEntrada.PROD_HB_SAIDA = faccaoSaida.CODIGO;
                faccaoEntrada.GRADE_EXP = 0;
                faccaoEntrada.GRADE_XP = 0;
                faccaoEntrada.GRADE_PP = 0;
                faccaoEntrada.GRADE_P = 0;
                faccaoEntrada.GRADE_M = 0;
                faccaoEntrada.GRADE_G = 0;
                faccaoEntrada.GRADE_GG = 0;
                faccaoEntrada.GRADE_TOTAL = 0;
                faccaoEntrada.STATUS = 'A';
                faccaoEntrada.DATA_INCLUSAO = DateTime.Now;
                faccController.InserirEntradaHB(faccaoEntrada);
            }

            return faccaoSaida;
        }
        private bool BaixarCorte(string tela)
        {
            //Validação de nulos
            if (!ValidarCampos())
            {
                throw new Exception("Preencha corretamente os campos em <strong>vermelho</strong>.");
            }

            PROD_HB principal = new PROD_HB();
            DESENV_PRODUTO produtoPrincipal = null;

            principal = prodController.ObterHB(Convert.ToInt32(hidHB.Value));
            if (principal != null)
            {
                //BUscar produto cortado na desenv_produto
                if (principal.DESENV_PRODUTO_ORIGEM != null)
                    produtoPrincipal = desenvController.ObterProduto(principal.COLECAO, principal.CODIGO_PRODUTO_LINX, principal.COR, Convert.ToInt32(principal.DESENV_PRODUTO_ORIGEM));
                else
                    produtoPrincipal = desenvController.ObterProduto(principal.COLECAO, principal.CODIGO_PRODUTO_LINX, principal.COR);

                if (principal.STATUS != 'B')
                {
                    //Inserir grade
                    var gradeExiste = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 3);
                    if (gradeExiste == null)
                    {
                        var grade = new PROD_HB_GRADE();
                        grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                        grade.PROD_PROCESSO = 3; //GRAVA GRADE
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

                    //ATUALIZAR AVIAMENTO
                    foreach (GridViewRow r in gvAviamento.Rows)
                    {
                        PROD_HB_PROD_AVIAMENTO aviamento = new PROD_HB_PROD_AVIAMENTO();
                        string codigoAvia = gvAviamento.DataKeys[r.RowIndex].Value.ToString();

                        TextBox _qtdeExtra = r.FindControl("txtQtdeExtra") as TextBox;
                        if (_qtdeExtra != null)
                        {
                            if (_qtdeExtra.Text != "")
                            {
                                aviamento.CODIGO = Convert.ToInt32(codigoAvia);
                                aviamento.QTDE_EXTRA = Convert.ToDecimal(_qtdeExtra.Text);
                                AtualizarAviamentoHB(aviamento);
                            }
                        }
                    }

                    principal.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);
                    principal.GASTO_FOLHA = Convert.ToDecimal(txtGastoPorFolha.Text) * 1; //Convert.ToDecimal(1.07);
                    if (txtGastoPorPecaCusto.Text.Trim() != "")
                        principal.GASTO_PECA_CUSTO = Convert.ToDecimal(txtGastoPorPecaCusto.Text);
                    principal.RETALHOS = Convert.ToDecimal(txtGastoRetalhos.Text);
                    principal.VOLUME = Convert.ToInt32(txtVolume.Text);
                    principal.GABARITO = Convert.ToChar(ddlGabarito.SelectedValue);
                    principal.DATA_ENVIO_ETI_COMP = DateTime.Now;
                    principal.STATUS = 'B'; //BAIXADO

                    if (principal.MOSTRUARIO == 'S')
                        principal.DATA_IMP_FIC_LOGISTICA = DateTime.Now;
                    AtualizarBaixaHB(principal);

                    //Baixa terceiro processo
                    AtualizarProcessoDataFim(principal.CODIGO, ObterProcessoOrdem(3), Convert.ToDateTime(hidData.Value));
                    //FIM ATUALIZAÇÃO

                    CriarFichaTecnica(produtoPrincipal, principal);
                }

                List<PROD_HB> lstBaixaDetalhe = new List<PROD_HB>();
                List<PROD_HB> detalhes = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));
                if (hidMostruario.Value == "N" && principal.LIQUIDAR == 'S')
                {

                    /******************************************************************************************************/
                    foreach (PROD_HB det in detalhes.Where(p => p.PROD_DETALHE != 4 && p.PROD_DETALHE != 5).ToList()) //RETIRAR GALAO
                    {
                        //INCLUIR PROCESSO
                        var processoDetExiste = prodController.ObterProcessoHB(det.CODIGO, 2);
                        if (processoDetExiste == null)
                        {
                            var processoDet = new PROD_HB_PROD_PROCESSO();
                            processoDet.PROD_HB = det.CODIGO;
                            processoDet.PROD_PROCESSO = ObterProcessoOrdem(2); //Obter o primeiro processo
                            processoDet.DATA_INICIO = DateTime.Now;
                            IncluirProcesso(processoDet);
                        }

                        //Atualizar grade
                        AtualizarGradeDetalhe(det.CODIGO);
                    }
                    /******************************************************************************************************/

                }
                else if (hidMostruario.Value == "N" && principal.LIQUIDAR == 'N')
                {
                    // SO ATUALIZAR GRADE DE GALAO E GALAO PROPRIO

                    //AQUI TEM QUE MONTAR O GALAO NORMAL TAMBEM
                    foreach (PROD_HB det in detalhes.Where(p => p.PROD_DETALHE == 4 || p.PROD_DETALHE == 5).ToList())
                    {
                        prodController.InserirDadosGalao(Convert.ToInt32(det.CODIGO_PAI), Convert.ToInt32(det.PROD_DETALHE));
                    }

                }
                else // É mostruário
                {
                    int codigoHB = 0;
                    foreach (GridViewRow row in gvDetalhe.Rows)
                    {
                        codigoHB = Convert.ToInt32(gvDetalhe.DataKeys[row.RowIndex].Value);

                        PROD_HB det = prodController.ObterHB(codigoHB);
                        if (det != null)
                        {
                            if (det.STATUS != 'B')
                            {
                                DropDownList _ddlUnidadeMedida = row.FindControl("ddlUnidadeMedida") as DropDownList;
                                TextBox _txtPorFolha = row.FindControl("txtGastoPorFolha") as TextBox;
                                TextBox _txtRetalho = row.FindControl("txtGastoRetalhos") as TextBox;
                                TextBox _txtGastoPorPecaCusto = row.FindControl("txtGastoPorPecaCusto") as TextBox;

                                if (_ddlUnidadeMedida.SelectedValue != "0" && _txtPorFolha.Text.Trim() != "" && _txtRetalho.Text.Trim() != "")
                                {
                                    //Inserir grade
                                    var gradeExiste = prodController.ObterGradeHB(det.CODIGO, 3);
                                    if (gradeExiste == null)
                                    {
                                        var grade = new PROD_HB_GRADE();
                                        grade.PROD_HB = det.CODIGO;
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

                                    var _prod_hb = new PROD_HB();
                                    _prod_hb.CODIGO = det.CODIGO;
                                    _prod_hb.UNIDADE_MEDIDA = Convert.ToInt32(_ddlUnidadeMedida.SelectedValue);
                                    _prod_hb.GASTO_FOLHA = Convert.ToDecimal(_txtPorFolha.Text) * 1; //Convert.ToDecimal(1.07);
                                    if (_txtGastoPorPecaCusto != null && _txtGastoPorPecaCusto.Text.Trim() != "")
                                        _prod_hb.GASTO_PECA_CUSTO = Convert.ToDecimal(_txtGastoPorPecaCusto.Text);
                                    _prod_hb.RETALHOS = Convert.ToDecimal(_txtRetalho.Text);
                                    _prod_hb.GABARITO = Convert.ToChar(ddlGabarito.SelectedValue);
                                    _prod_hb.STATUS = 'B'; //BAIXADO

                                    AtualizarBaixaHB(_prod_hb);

                                    //Baixa primeiro processo
                                    AtualizarProcessoDataFim(det.CODIGO, ObterProcessoOrdem(3), Convert.ToDateTime(hidData.Value));

                                    det.UNIDADE_MEDIDA = _prod_hb.UNIDADE_MEDIDA;
                                    det.GASTO_FOLHA = _prod_hb.GASTO_FOLHA;
                                    det.GASTO_PECA_CUSTO = _prod_hb.GASTO_PECA_CUSTO;
                                    det.RETALHOS = _prod_hb.RETALHOS;
                                    det.CODIGO_PRODUTO_LINX = principal.CODIGO_PRODUTO_LINX;
                                    CriarFichaTecnica(produtoPrincipal, det);

                                }
                            }
                        }
                    }
                }
            }
            return true;
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

                            var ficTecExiste = desenvController.ObterFichaTecnicaPorMaterial(_produto.CODIGO, materialCodigo, a.COR);
                            if (ficTecExiste != null)
                            {
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
                            }

                            prodDet += 1;
                        }
                    }



                }
            }
        }
        private void PreencherDadosMostruarioBaixa(PROD_HB principal)
        {
            PROD_HB_GRADE grade = new PROD_HB_GRADE();
            grade = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 3);

            int totalGrade = 0;
            if (grade != null)
            {
                txtGradeEXP_R.Text = grade.GRADE_EXP.ToString();
                txtGradeXP_R.Text = grade.GRADE_XP.ToString();
                txtGradePP_R.Text = grade.GRADE_PP.ToString();
                txtGradeP_R.Text = grade.GRADE_P.ToString();
                txtGradeM_R.Text = grade.GRADE_M.ToString();
                txtGradeG_R.Text = grade.GRADE_G.ToString();
                txtGradeGG_R.Text = grade.GRADE_GG.ToString();

                totalGrade = Convert.ToInt32(grade.GRADE_EXP + grade.GRADE_XP + grade.GRADE_PP + grade.GRADE_P + grade.GRADE_M + grade.GRADE_G + grade.GRADE_GG);
                txtGradeTotal_R.Text = totalGrade.ToString();
            }

            ddlUnidade.SelectedValue = (principal.UNIDADE_MEDIDA != null) ? principal.UNIDADE_MEDIDA.ToString() : ddlUnidadeGalao.SelectedValue = RetornarUnidadeMedidaMaterial(principal.GRUPO_TECIDO.Trim(), principal.TECIDO.Trim());
            ddlUnidade.Enabled = false;
            txtGastoPorFolha.Text = principal.GASTO_FOLHA.ToString();
            txtGastoRetalhos.Text = principal.RETALHOS.ToString();

            if (totalGrade > 0 && ddlUnidade.SelectedValue != "0" && txtGastoPorFolha.Text.Trim() != "" && txtGastoRetalhos.Text.Trim() != "")
            {
                decimal sobraMetro = 1;
                txtGastoPorCorte.Text = ((Convert.ToDecimal(txtGastoPorFolha.Text) * totalGrade)).ToString("####0.000");
                txtGastoPorCorte.ReadOnly = true;
                txtGastoPorPeca.Text = (((Convert.ToDecimal(txtGastoPorCorte.Text) + Convert.ToDecimal(txtGastoRetalhos.Text)) / totalGrade) * sobraMetro).ToString("####0.000");
                txtGastoPorPeca.ReadOnly = true;
            }

            if (principal.GASTO_PECA_CUSTO != null)
                txtGastoPorPecaCusto.Text = principal.GASTO_PECA_CUSTO.ToString();

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

                    _ddlUnidadeMedida.SelectedValue = (det.UNIDADE_MEDIDA != null) ? det.UNIDADE_MEDIDA.ToString() : (RetornarUnidadeMedidaMaterial(det.GRUPO_TECIDO.Trim(), det.TECIDO.Trim()));
                    _txtGastoPorFolha.Text = det.GASTO_FOLHA.ToString();
                    _txtGastoRetalhos.Text = det.RETALHOS.ToString();
                    if (det.GASTO_PECA_CUSTO != null)
                        _txtGastoPorPecaCusto.Text = det.GASTO_PECA_CUSTO.ToString();

                    txtGasto_TextChanged(_txtGastoPorFolha, null);
                }
            }

            foreach (GridViewRow row in gvGradeCorteHBBaixa.Rows)
            {
                int codigoCorte = 0;
                codigoCorte = Convert.ToInt32(gvGradeCorteHBBaixa.DataKeys[row.RowIndex].Value);
                PROD_HB_CORTE _corte = prodController.ObterGradeCorte(codigoCorte);
                if (_corte != null)
                {
                    if (_corte.ENFESTO != null)
                    {
                        DropDownList ddlEnfesto1 = row.FindControl("ddlEnfesto1") as DropDownList;
                        DropDownList ddlEnfesto2 = row.FindControl("ddlEnfesto2") as DropDownList;
                        DropDownList ddlEnfesto3 = row.FindControl("ddlEnfesto3") as DropDownList;

                        DropDownList ddlCorte1 = row.FindControl("ddlCorte1") as DropDownList;
                        DropDownList ddlCorte2 = row.FindControl("ddlCorte2") as DropDownList;

                        DropDownList ddlAmarrador1 = row.FindControl("ddlAmarrador1") as DropDownList;
                        DropDownList ddlAmarrador2 = row.FindControl("ddlAmarrador2") as DropDownList;

                        string[] enfesto;
                        enfesto = _corte.ENFESTO.Split('/');
                        if (enfesto.Count() > 0 && enfesto[0] != "")
                            ddlEnfesto1.SelectedValue = enfesto[0];
                        if (enfesto.Count() > 1 && enfesto[1] != "")
                            ddlEnfesto2.SelectedValue = enfesto[1];
                        if (enfesto.Count() > 2 && enfesto[2] != "")
                            ddlEnfesto3.SelectedValue = enfesto[2];

                        string[] cortador;
                        cortador = _corte.CORTE.Split('/');
                        if (cortador.Count() > 0 && cortador[0] != "")
                            ddlCorte1.SelectedValue = cortador[0];
                        if (cortador.Count() > 1 && cortador[1] != "")
                            ddlCorte2.SelectedValue = cortador[1];

                        string[] amarrador;
                        amarrador = _corte.AMARRADOR.Split('/');
                        if (amarrador.Count() > 0 && amarrador[0] != "")
                            ddlAmarrador1.SelectedValue = amarrador[0];
                        if (amarrador.Count() > 1 && amarrador[1] != "")
                            ddlAmarrador2.SelectedValue = amarrador[1];

                    }
                }
            }
        }
        private string GerarPedidoCompra(int prod_hb, string produto, string corProduto, char atacado, string aprovadoPor)
        {
            string numeroPedido = "";

            var pedidoCompra = prodController.GerarPedidoCompra(prod_hb, produto, corProduto, atacado, aprovadoPor);
            if (pedidoCompra != null)
                numeroPedido = pedidoCompra.NUMERO_PEDIDO.Trim().ToString();

            return numeroPedido;
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
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            bool bAviamentoSemOrcamento = false;
            string nomeUsuario = "";
            string pcAtacado = "";
            string pcVarejo = "";
            bool bMostruario = false;
            int gradeVarejo = 0;
            int gradeTotal = 0;
            int gradeAtacado = 0;
            //bool gradeErrada = false;
            int gradeSaida = 0;

            try
            {


                PROD_HB prod_hb = prodController.ObterHB(Convert.ToInt32(hidHB.Value));
                if (prod_hb != null)
                    bMostruario = (prod_hb.MOSTRUARIO == 'S') ? true : false;

                var _detalhes = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value));

                //Baixar CORTE
                BaixarCorte(hidTela.Value);
                prod_hb.STATUS = 'B';
                nomeUsuario = "INTRA-" + ((USUARIO)Session["USUARIO"]).NOME_USUARIO;

                //Validar se o atacado está marcado e existe a grade de atacado e não é mostruario
                if (cbAtacado.Checked && !bMostruario)
                {
                    //INSERIR GRADE do ATACADO
                    var gradeAtacadoExiste = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 99);
                    if (gradeAtacadoExiste == null)
                    {
                        PROD_HB_GRADE _grade = new PROD_HB_GRADE();
                        _grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                        _grade.PROD_PROCESSO = 99; //GRAVA GRADE 99 - ATACADO
                        _grade.GRADE_EXP = (txtGradeEXP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeEXP_A.Text);
                        _grade.GRADE_XP = (txtGradeXP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeXP_A.Text);
                        _grade.GRADE_PP = (txtGradePP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradePP_A.Text);
                        _grade.GRADE_P = (txtGradeP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeP_A.Text);
                        _grade.GRADE_M = (txtGradeM_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeM_A.Text);
                        _grade.GRADE_G = (txtGradeG_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeG_A.Text);
                        _grade.GRADE_GG = (txtGradeGG_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeGG_A.Text);
                        _grade.DATA_INCLUSAO = DateTime.Now;
                        _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        IncluirGrade(_grade);
                    }

                    //INSERIR GRADE do ATACADO CORTE - HISTORICO DO QUE FOI CORTADO PQ A ANA (LOGISTICA) REDEFINI AS FICHAS
                    var gradeAtacadoCorteExiste = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 4);
                    if (gradeAtacadoCorteExiste == null)
                    {
                        PROD_HB_GRADE _grade = new PROD_HB_GRADE();
                        _grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                        _grade.PROD_PROCESSO = 4; //GRAVA GRADE 4 - CORTE ATACADO
                        _grade.GRADE_EXP = (txtGradeEXP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeEXP_A.Text);
                        _grade.GRADE_XP = (txtGradeXP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeXP_A.Text);
                        _grade.GRADE_PP = (txtGradePP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradePP_A.Text);
                        _grade.GRADE_P = (txtGradeP_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeP_A.Text);
                        _grade.GRADE_M = (txtGradeM_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeM_A.Text);
                        _grade.GRADE_G = (txtGradeG_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeG_A.Text);
                        _grade.GRADE_GG = (txtGradeGG_A.Text.Trim() == "") ? 0 : Convert.ToInt32(txtGradeGG_A.Text);
                        _grade.DATA_INCLUSAO = DateTime.Now;
                        _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        IncluirGrade(_grade);
                    }

                    int totalGradeAtacado = prodController.ObterQtdeGradeHB(Convert.ToInt32(hidHB.Value), 99);

                    //GERAR PEDIDO DE COMPRA
                    if (Constante.gerarPedidoCompra && totalGradeAtacado > 0)
                        pcAtacado = GerarPedidoCompra(Convert.ToInt32(hidHB.Value), txtCodigoLinx.Text, ddlCor.SelectedValue, 'S', nomeUsuario);
                }

                //Varejo deve ser gerado depois do atacado SEMPRE
                if (Constante.gerarPedidoCompra && !bMostruario)
                    pcVarejo = GerarPedidoCompra(Convert.ToInt32(hidHB.Value), txtCodigoLinx.Text, ddlCor.SelectedValue, 'N', nomeUsuario);

                PROD_HB _prod_hb_pc = new PROD_HB();
                _prod_hb_pc.CODIGO = Convert.ToInt32(hidHB.Value);
                _prod_hb_pc.PC_ATACADO = pcAtacado;
                _prod_hb_pc.PC_VAREJO = pcVarejo;
                AtualizarPedidoCompra(_prod_hb_pc);

                gradeTotal = prodController.ObterQtdeGradeHB(prod_hb.CODIGO, 3);
                gradeAtacado = prodController.ObterQtdeGradeHB(prod_hb.CODIGO, 99);
                gradeVarejo = gradeTotal - gradeAtacado;

                var custoSimulacao = prodController.ObterCustoSimulacao(prod_hb.CODIGO_PRODUTO_LINX, Convert.ToChar(prod_hb.MOSTRUARIO));
                if (custoSimulacao == null || custoSimulacao.Count() <= 0)
                {
                    if (prod_hb.MOSTRUARIO == 'S')
                    {
                        GerarCustoZerado(prod_hb, 'S');
                        GerarCustoZerado(prod_hb, 'N');
                    }
                    else
                    {
                        GerarCustoZerado(prod_hb, 'N');
                    }
                }

                // Gerar registro de Facção
                // 29/02/2016
                // SE PEDIDO FOR 2028, não gera controle de produção, pois será feito em outro lugar
                if (prod_hb.NUMERO_PEDIDO != 2028)
                {
                    var validaSaida = new FaccaoController().ObterSaidaHB(prod_hb.CODIGO.ToString());
                    if (validaSaida == null || validaSaida.Count() <= 0)
                    {

                        var faccaoInterno = GerarFaccaoInterno();

                        PROD_HB_SAIDA _saida = new PROD_HB_SAIDA();
                        _saida.PROD_HB = prod_hb.CODIGO;
                        _saida.DATA_INCLUSAO = DateTime.Now;
                        _saida.PROD_PROCESSO = 20;
                        _saida.GRADE_EXP = ((txtGradeEXP_R.Text != "") ? Convert.ToInt32(txtGradeEXP_R.Text) : 0) - faccaoInterno.GRADE_EXP;
                        _saida.GRADE_XP = ((txtGradeXP_R.Text != "") ? Convert.ToInt32(txtGradeXP_R.Text) : 0) - faccaoInterno.GRADE_XP;
                        _saida.GRADE_PP = ((txtGradePP_R.Text != "") ? Convert.ToInt32(txtGradePP_R.Text) : 0) - faccaoInterno.GRADE_PP;
                        _saida.GRADE_P = ((txtGradeP_R.Text != "") ? Convert.ToInt32(txtGradeP_R.Text) : 0) - faccaoInterno.GRADE_P;
                        _saida.GRADE_M = ((txtGradeM_R.Text != "") ? Convert.ToInt32(txtGradeM_R.Text) : 0) - faccaoInterno.GRADE_M;
                        _saida.GRADE_G = ((txtGradeG_R.Text != "") ? Convert.ToInt32(txtGradeG_R.Text) : 0) - faccaoInterno.GRADE_G;
                        _saida.GRADE_GG = ((txtGradeGG_R.Text != "") ? Convert.ToInt32(txtGradeGG_R.Text) : 0) - faccaoInterno.GRADE_GG;
                        _saida.GRADE_TOTAL = (_saida.GRADE_EXP + _saida.GRADE_XP + _saida.GRADE_PP + _saida.GRADE_P + _saida.GRADE_M + _saida.GRADE_G + _saida.GRADE_GG);
                        _saida.SALDO = 'S';
                        gradeSaida = Convert.ToInt32(_saida.GRADE_TOTAL);
                        faccController.InserirSaidaHB(_saida);

                        ////TESTE PARA ENCONTRAR O ERRO
                        //if (gradeTotal != _saida.GRADE_TOTAL)
                        //    gradeErrada = true;

                        try
                        {
                            var retornoRota = faccController.GerarRotaModeloProduto(prod_hb.CODIGO_PRODUTO_LINX, prod_hb.COR, true, false, cbEstamparia.Checked, cbLavanderia.Checked, cbFaccao.Checked, cbAcabamento.Checked, true, true, false, false, false, false, false);

                            if (retornoRota.MENSAGEM_ERRO.Trim() != "")
                            {
                                throw new Exception("ERRO (GerarRotaModeloProduto): COD. " + retornoRota.CODIGO_ERRO + ", MSG. " + retornoRota.MENSAGEM_ERRO);
                            }

                            var retornoFacc = faccController.GerarOrdemProducao(prod_hb.CODIGO);

                            if (retornoFacc == null || retornoFacc.MENSAGEM_ERRO.Trim() != "")
                            {
                                throw new Exception("ERRO (GerarOrdemProducao): COD. " + retornoFacc.CODIGO_ERRO + ", MSG. " + retornoFacc.MENSAGEM_ERRO);
                            }

                            prod_hb.ORDEM_PRODUCAO = retornoFacc.CODIGO_ERRO.ToString();
                            prodController.AtualizarHB(prod_hb);

                            //Manutenção das Rotas
                            PROD_HB_ROTA rotaNova = new PROD_HB_ROTA();
                            var rotaExiste = new FaccaoController().ObterTabelaOperacao(prod_hb.CODIGO_PRODUTO_LINX);
                            List<PRODUTO_OPERACOES_ROTA> rotas = new ProducaoController().ObterRotaFases(rotaExiste.TABELA_OPERACOES);
                            if (rotas != null && rotas.Count > 0)
                            {
                                rotaNova.ORDEM_PRODUCAO = retornoFacc.CODIGO_ERRO.ToString();
                                rotaNova.PROD_HB = prod_hb.CODIGO;
                                rotaNova.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                rotaNova.DATA_INCLUSAO = DateTime.Now;
                                rotaNova.FACCAO = false;
                                rotaNova.ESTAMPARIA = false;
                                rotaNova.LAVANDERIA = false;
                                rotaNova.ACABAMENTO = false;

                                foreach (PRODUTO_OPERACOES_ROTA rota in rotas)
                                {
                                    var fase = new ProducaoController().ObterFase(rota.FASE_PRODUCAO);
                                    if (fase != null)
                                    {
                                        if (fase.DESC_FASE_PRODUCAO.Trim() == "FACCAO")
                                        {
                                            rotaNova.FACCAO = true;
                                        }
                                        else if (fase.DESC_FASE_PRODUCAO.Trim() == "ESTAMPARIA")
                                        {
                                            rotaNova.ESTAMPARIA = true;
                                        }
                                        else if (fase.DESC_FASE_PRODUCAO.Trim() == "LAVANDERIA")
                                        {
                                            rotaNova.LAVANDERIA = true;
                                        }
                                        else if (fase.DESC_FASE_PRODUCAO.Trim() == "ACABAMENTO")
                                        {
                                            rotaNova.ACABAMENTO = true;
                                        }
                                    }
                                }

                                prodController.InserirRota(rotaNova);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }
                }

                //Atualizar registros liberados do produto principal
                AtualizarProdutoProducao(prod_hb, bMostruario, gradeVarejo, gradeAtacado);

                //Enviar Email
                prod_hb.PC_ATACADO = pcAtacado;
                prod_hb.PC_VAREJO = pcVarejo;

                EnviarEmail(prod_hb);

                EnviarEmailEtiquetaComposicao(prod_hb);

                labErroEnvio.Text = "<strong>Aguarde enquanto o relatório está sendo gerado...</strong>";
                //GERAR RELATORIO
                RelatorioHB _relatorio = new RelatorioHB();
                _relatorio.CODIGO = prod_hb.CODIGO;
                _relatorio.HB = txtHB.Text.ToString();
                _relatorio.COLECAO = ddlColecoes.SelectedItem.Text.Trim();
                _relatorio.COD_COR = ddlCor.SelectedValue.Trim();
                _relatorio.DESC_COR = ddlCor.SelectedItem.Text.Trim();
                _relatorio.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                _relatorio.GRUPO = ddlGrupo.SelectedItem.Text.Trim();
                _relatorio.NOME = txtNome.Text.ToString().Trim().ToUpper();
                _relatorio.DATA = Convert.ToDateTime(txtData.Text).ToString("dd/MM/yyyy");
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
                _relatorio.DETALHE = _detalhes;
                _relatorio.AVIAMENTO = prodController.ObterAviamentoHB(Convert.ToInt32(hidHB.Value));
                _relatorio.GRADE_CORTE = prodController.ObterGradeCorteHB(Convert.ToInt32(hidHB.Value));
                _relatorio.CODIGO_PRODUTO_LINX = txtCodigoLinx.Text.Trim();
                _relatorio.PC_VAREJO = pcVarejo;
                _relatorio.PC_ATACADO = pcAtacado;
                _relatorio.MOLDE = (prod_hb.MOLDE == null) ? "" : prod_hb.MOLDE.ToString();
                _relatorio.MOSTRUARIO = prod_hb.MOSTRUARIO.ToString();
                _relatorio.PROD_GRADE = prod_hb.PROD_GRADE.ToString();
                _relatorio.ORDEM_PRODUCAO = prod_hb.ORDEM_PRODUCAO;
                _relatorio.VOLUME = txtVolume.Text.Trim().ToUpper();
                _relatorio.GABARITO = ddlGabarito.SelectedValue;
                _relatorio.DATA_IMP_FIC_LOGISTICA = "";

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

                if (fsFaccaoSemRota.Visible == true)
                {
                    if (cbEstamparia.Checked == true)
                    {
                        _relatorio.FaseEstamparia = "SIM";
                    }
                    else
                    {
                        _relatorio.FaseEstamparia = "NÃO";
                    }
                    if (cbLavanderia.Checked == true)
                    {
                        _relatorio.FaseLavanderia = "SIM";
                    }
                    else
                    {
                        _relatorio.FaseLavanderia = "NÃO";
                    }
                }
                if (fsFaccaoComRota.Visible == true)
                {
                    if (lblEstamparia.Text == "SIM")
                    {
                        _relatorio.FaseEstamparia = "SIM";
                    }
                    else
                    {
                        _relatorio.FaseEstamparia = "NÃO";
                    }
                    if (lblLavanderia.Text == "SIM")
                    {
                        _relatorio.FaseLavanderia = "SIM";
                    }
                    else
                    {
                        _relatorio.FaseLavanderia = "NÃO";
                    }
                }

                //OBTER PROCESSO
                _relatorio.PROCESSO = prodController.ObterProcessoHB(Convert.ToInt32(hidHB.Value), 3); //CORTE

                _relatorio.GRADE = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 3); //GERAR RELATORIO COM A GRADE FINAL DO CORTE
                _relatorio.GRADEATACADO = prodController.ObterGradeHB(Convert.ToInt32(hidHB.Value), 99); //GERAR RELATORIO COM A GRADE ATACADO
                GerarRelatorio(_relatorio, 3, bMostruario);
                if (bAviamentoSemOrcamento)
                    litAviamento.Visible = true;

                labErroEnvio.Text = "<strong>OK</strong>";
                labHBPopUp.Text = txtHB.Text;

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, Risco: function () { window.open('prod_fila_risco.aspx', '_self'); }, Corte: function () { window.open('prod_fila_corte.aspx', '_self'); }, 'Rel. Aviamento': function () { window.open('prod_rel_aviamento.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;

                ////TESTE PARA ENCONTRAR ERRO
                //if (gradeErrada)
                //{
                //    EnviarEmailErro(prod_hb.CODIGO, gradeTotal, gradeSaida);
                //}
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

            labFotoTecido.ForeColor = _OK;
            if (imgFotoTecido.ImageUrl == "")
            {
                labFotoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labVolume.ForeColor = _OK;
            if (txtVolume.Text.Trim() == "" || Convert.ToDecimal(txtVolume.Text.Trim()) < 0)
            {
                labVolume.ForeColor = _notOK;
                retorno = false;
            }

            labGabarito.ForeColor = _OK;
            if (ddlGabarito.SelectedValue == "")
            {
                labGabarito.ForeColor = _notOK;
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
            if (ValidarGrade("V") <= 0)
            {
                labGrade.ForeColor = _notOK;
                labGradeReal.ForeColor = _notOK;
                retorno = false;
            }

            labGradeAtacado.ForeColor = _OK;
            if (cbAtacado.Checked && ValidarGrade("A") <= 0)
            {
                labGradeAtacado.ForeColor = _notOK;
                retorno = false;
            }

            labGradeCorteBaixa.ForeColor = _OK;
            if (gvGradeCorteHBBaixa.Rows.Count <= 0)
            {
                labGradeCorteBaixa.ForeColor = _notOK;
                retorno = false;
            }

            labGradeCorteBaixa.ForeColor = _OK;
            if (ValidarGridCorte() <= 0)
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

            /*labGastoPorPecaCusto.ForeColor = _OK;
            if (txtGastoPorPecaCusto.Text.Trim() == "" || Convert.ToDecimal(txtGastoPorPecaCusto.Text.Trim()) <= 0)
            {
                labGastoPorPecaCusto.ForeColor = _notOK;
                retorno = false;
            }*/

            labGastoRetalhos.ForeColor = _OK;
            if (txtGastoRetalhos.Text.Trim() == "" || Convert.ToDecimal(txtGastoRetalhos.Text.Trim()) < 0)
            {
                labGastoRetalhos.ForeColor = _notOK;
                retorno = false;
            }

            /* GASTO GALAO*/
            // 12/12/2014 - ALTERACAO - INCLUSAO DE GASTOS DE GALAO E GALAO PROPRIO NO PRINCIPAL
            if (pnlGastoDetalhe.Visible)
            {
                labGastoUnidadeGalao.ForeColor = _OK;
                if (ddlUnidadeGalao.SelectedValue == "" || ddlUnidadeGalao.SelectedValue == "0" || ddlUnidadeGalao.SelectedValue == "Selecione")
                {
                    labGastoUnidadeGalao.ForeColor = _notOK;
                    retorno = false;
                }

                labGastoPorFolhaGalao.ForeColor = _OK;
                if (txtGastoPorFolhaGalao.Text.Trim() == "" || Convert.ToDecimal(txtGastoPorFolhaGalao.Text.Trim()) <= 0)
                {
                    labGastoPorFolhaGalao.ForeColor = _notOK;
                    retorno = false;
                }

                labGastoRetalhosGalao.ForeColor = _OK;
                if (txtGastoRetalhosGalao.Text.Trim() == "" || Convert.ToDecimal(txtGastoRetalhosGalao.Text.Trim()) < 0)
                {
                    labGastoRetalhosGalao.ForeColor = _notOK;
                    retorno = false;
                }
            }
            /**/

            /* GASTO GALAO PROPRIO*/
            // 12/12/2014 - ALTERACAO - INCLUSAO DE GASTOS DE GALAO E GALAO PROPRIO NO PRINCIPAL
            if (pnlGastoDetalheGalaoProprio.Visible)
            {
                labGastoUnidadeGalaoProprio.ForeColor = _OK;
                if (ddlUnidadeGalaoProprio.SelectedValue == "" || ddlUnidadeGalaoProprio.SelectedValue == "0" || ddlUnidadeGalaoProprio.SelectedValue == "Selecione")
                {
                    labGastoUnidadeGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }

                labGastoPorFolhaGalaoProprio.ForeColor = _OK;
                if (txtGastoPorFolhaGalaoProprio.Text.Trim() == "" || Convert.ToDecimal(txtGastoPorFolhaGalaoProprio.Text.Trim()) <= 0)
                {
                    labGastoPorFolhaGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }

                labGastoRetalhosGalaoProprio.ForeColor = _OK;
                if (txtGastoRetalhosGalaoProprio.Text.Trim() == "" || Convert.ToDecimal(txtGastoRetalhosGalaoProprio.Text.Trim()) < 0)
                {
                    labGastoRetalhosGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
            }
            /**/

            labFotoPeca.ForeColor = _OK;
            if (imgFotoPeca.ImageUrl == "")
            {
                labFotoPeca.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private int ValidarGrade(string tipo)
        {
            string _exp = "";
            string _xp = "";
            string _pp = "";
            string _p = "";
            string _m = "";
            string _g = "";
            string _gg = "";

            if (tipo == "V")
            {
                _exp = txtGradeEXP_R.Text.Trim();
                _xp = txtGradeXP_R.Text.Trim();
                _pp = txtGradePP_R.Text.Trim();
                _p = txtGradeP_R.Text.Trim();
                _m = txtGradeM_R.Text.Trim();
                _g = txtGradeG_R.Text.Trim();
                _gg = txtGradeGG_R.Text.Trim();
            }
            else
            {
                _exp = txtGradeEXP_A.Text.Trim();
                _xp = txtGradeXP_A.Text.Trim();
                _pp = txtGradePP_A.Text.Trim();
                _p = txtGradeP_A.Text.Trim();
                _m = txtGradeM_A.Text.Trim();
                _g = txtGradeG_A.Text.Trim();
                _gg = txtGradeGG_A.Text.Trim();
            }

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

        private void EnviarEmailErro(int codigoPai, int gradeTotal, int gradeSaida)
        {
            email_envio email = new email_envio();
            email.ASSUNTO = "ERRO PROD_HB: " + codigoPai.ToString();
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = MontarCorpoEmailErro(codigoPai, gradeTotal, gradeSaida);

            List<string> destinatario = new List<string>();
            destinatario.Add("leandro.bevilaqua@hbf.com.br");
            destinatario.Add("le.bevilaqua@gmail.com");

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmailErro(int codigoPai, int gradeTotal, int gradeSaida)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("");
            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>ERRO</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divErro' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <h2>ERRO</h2>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    CODIGO PAI:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + codigoPai.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    GRADE TOTAL:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + gradeTotal.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    GRADE SAIDA:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + gradeSaida.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

        private void GerarRelatorio(RelatorioHB _relatorio, int tela, bool bMostruario)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "";
                if (tela == 2)
                    nomeArquivo = "HB_BAIXADO_R_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
                else
                    nomeArquivo = "HB_BAIXADO_C_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";

                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_relatorio, tela, bMostruario));
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
        private StringBuilder MontarRelatorioHTML(RelatorioHB _relatorio, int tela, bool bMostruario)
        {
            StringBuilder _texto = new StringBuilder();

            if (tela == 2) //BAIXA RISCO
            {
                //Cabecalho
                _texto = _relatorio.MontarCabecalho(_texto, "HB - Baixa Risco");
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
            }
            else //BAIXA CORTE
            {
                //Cabecalho
                _texto = _relatorio.MontarCabecalho(_texto, "HB - Baixa Corte");
                //Montar Baixa Corte
                _texto = _relatorio.MontarFaccaoAviamento(_texto, _relatorio, "1", "breakafter"); //Facção

                //Controle
                if (!bMostruario)
                {
                    _texto = _relatorio.MontarFaccaoAviamento(_texto, _relatorio, "2", "breakafter"); //Com quebra
                    _texto = _relatorio.MontarFaccaoAviamento(_texto, _relatorio, "2", "breakafter"); //Com quebra
                }
                else
                {
                    _texto = _relatorio.MontarFaccaoAviamento(_texto, _relatorio, "2", "breakafter"); //Sem quebra
                }

                //Retirar Galão
                _relatorio.DETALHE = _relatorio.DETALHE.Where(p => p.PROD_DETALHE != 4 && p.PROD_DETALHE != 5).ToList();

                if (bMostruario)
                    _texto = _relatorio.MontarLogisticaV2(_texto, _relatorio, "breakafter", Server.MapPath("~")); // Logística

                //Detalhes
                if (_relatorio.LIQUIDAR == "Sim" && _relatorio.DETALHE.Count > 0 && !bMostruario)
                    _texto = _relatorio.MontarDetalhe(_texto, _relatorio, "breakafter");
                //Rodape
                _texto = _relatorio.MontarRodape(_texto);
            }

            return _texto;
        }

        private void EnviarEmail(PROD_HB prod_hb)
        {
            string _tipo = "";

            var gradeTotal = prodController.ObterGradeHB(prod_hb.CODIGO, 3);
            var gradeAtacado = prodController.ObterGradeHB(prod_hb.CODIGO, 99);

            int totalGrade = 0;
            totalGrade = (Convert.ToInt32(gradeTotal.GRADE_EXP) +
                                Convert.ToInt32(gradeTotal.GRADE_XP) +
                                Convert.ToInt32(gradeTotal.GRADE_PP) +
                                Convert.ToInt32(gradeTotal.GRADE_P) +
                                Convert.ToInt32(gradeTotal.GRADE_M) +
                                Convert.ToInt32(gradeTotal.GRADE_G) +
                                Convert.ToInt32(gradeTotal.GRADE_GG));

            int totalGradeAtacado = 0;
            if (gradeAtacado != null && gradeAtacado.CODIGO > 0)
            {
                totalGradeAtacado = (Convert.ToInt32(gradeAtacado.GRADE_EXP) +
                                            Convert.ToInt32(gradeAtacado.GRADE_XP) +
                                            Convert.ToInt32(gradeAtacado.GRADE_PP) +
                                            Convert.ToInt32(gradeAtacado.GRADE_P) +
                                            Convert.ToInt32(gradeAtacado.GRADE_M) +
                                            Convert.ToInt32(gradeAtacado.GRADE_G) +
                                            Convert.ToInt32(gradeAtacado.GRADE_GG));
            }

            if (prod_hb.MOSTRUARIO == 'S')
            {
                _tipo = "M";
            }
            else
            {
                if (totalGrade == totalGradeAtacado)
                    _tipo = "A";

                if (totalGrade > 0 && totalGradeAtacado == 0)
                    _tipo = "V";

                if (totalGrade > 0 && totalGradeAtacado > 0 && totalGrade != totalGradeAtacado)
                    _tipo = "A/V";
            }

            email_envio email = new email_envio();
            email.ASSUNTO = "INTRANET: PEDIDO DE COMPRA - PRODUTO: " + prod_hb.CODIGO_PRODUTO_LINX.Trim() + " - COR: " + prod_hb.COR.Trim() + " - CORTE: " + _tipo;
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = MontarCorpoEmail(prod_hb, gradeTotal, (totalGrade - totalGradeAtacado), gradeAtacado, totalGradeAtacado);

            List<string> destinatario = new List<string>();
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(4, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(PROD_HB prod_hb, PROD_HB_GRADE gradeTotal, int totalVarejo, PROD_HB_GRADE gradeAtacado, int totalAtacado)
        {
            BaseController baseController = new BaseController();
            StringBuilder sb = new StringBuilder();

            var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(prod_hb.PROD_GRADE));

            sb.Append("");
            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>PEDIDO DE COMPRA</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divHB' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <h2>GERAÇÃO DE PEDIDO DE COMPRA</h2>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 517pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    COLEÇÃO:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaColecaoAtual(prod_hb.COLECAO).DESC_COLECAO.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    HB:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + prod_hb.HB.ToString());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    PRODUTO:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + prod_hb.CODIGO_PRODUTO_LINX.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    GRUPO:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + prod_hb.GRUPO.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    NOME:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + prod_hb.NOME.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    COR:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + prodController.ObterCoresBasicas(prod_hb.COR).DESC_COR.Trim());
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    COR FORNECEDOR:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + prod_hb.COR_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");

            if (prod_hb.PC_VAREJO != null && prod_hb.PC_VAREJO.Trim() != "" && Convert.ToInt32(prod_hb.PC_VAREJO.Trim()) > 0)
            {
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        Pedido de Compra VAREJO: <span style='color:red;'>" + prod_hb.PC_VAREJO + "</span>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
            }
            if (totalVarejo > 0)
            {
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        <table cellpadding='0' cellspacing='0' style='width: 550pt; padding: 0px; color: black;");
                sb.Append("                            font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
                sb.Append("                            border: 1px solid #ccc;'>");
                sb.Append("                            <tr style='background-color: #ccc;'>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Varejo");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                     " + (gradeTotal.GRADE_EXP - ((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_EXP)));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                     " + (gradeTotal.GRADE_XP - ((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_XP)));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                     " + (gradeTotal.GRADE_PP - ((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_PP)));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                     " + (gradeTotal.GRADE_P - ((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_P)));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                     " + (gradeTotal.GRADE_M - ((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_M)));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                     " + (gradeTotal.GRADE_G - ((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_G)));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                     " + (gradeTotal.GRADE_GG - ((gradeAtacado == null) ? 0 : gradeAtacado.GRADE_GG)));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + totalVarejo.ToString());
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                sb.Append("                        </table>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        &nbsp;");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        &nbsp;");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
            }
            if (prod_hb.PC_ATACADO != null && prod_hb.PC_ATACADO.Trim() != "" && Convert.ToInt32(prod_hb.PC_ATACADO.Trim()) > 0)
            {
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        Pedido de Compra ATACADO: <span style='color:red;'>" + prod_hb.PC_ATACADO + "</span>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
            }
            if (totalAtacado > 0)
            {
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        <table cellpadding='0' cellspacing='0' style='width: 550pt; padding: 0px; color: black;");
                sb.Append("                            font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
                sb.Append("                            border: 1px solid #ccc;'>");
                sb.Append("                            <tr style='background-color: #ccc;'>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center;'>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Atacado");
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + ((gradeAtacado == null) ? "&nbsp;" : gradeAtacado.GRADE_EXP.ToString()));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + ((gradeAtacado == null) ? "&nbsp;" : gradeAtacado.GRADE_XP.ToString()));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + ((gradeAtacado == null) ? "&nbsp;" : gradeAtacado.GRADE_PP.ToString()));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + ((gradeAtacado == null) ? "&nbsp;" : gradeAtacado.GRADE_P.ToString()));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + ((gradeAtacado == null) ? "&nbsp;" : gradeAtacado.GRADE_M.ToString()));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + ((gradeAtacado == null) ? "&nbsp;" : gradeAtacado.GRADE_G.ToString()));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + ((gradeAtacado == null) ? "&nbsp;" : gradeAtacado.GRADE_GG.ToString()));
                sb.Append("                                </td>");
                sb.Append("                                <td style='text-align: center; width: 75px;'>");
                sb.Append("                                    " + totalAtacado.ToString());
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                sb.Append("                        </table>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
            }
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO.ToUpper() + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        #region "EMAIL"
        private void EnviarEmailEtiquetaComposicao(PROD_HB hb)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Etiqueta de Composição - " + hb.CODIGO_PRODUTO_LINX;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmailEtiquetaComposicao(hb);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(10, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmailEtiquetaComposicao(PROD_HB hb)
        {
            string material = "";
            string ncm = "";
            string composicao = "";
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(hb.PROD_GRADE));

            var materialList = desenvController.ObterMaterial(hb.GRUPO_TECIDO, hb.TECIDO);
            foreach (var m in materialList)
            {
                var matCor = desenvController.ObterMaterialCor(m.MATERIAL, hb.COR, hb.COR_FORNECEDOR);
                if (matCor != null)
                {
                    var matComp = desenvController.ObterMaterialComposicao(m.MATERIAL);
                    if (matComp != null && matComp.Count() > 0)
                    {
                        material = m.MATERIAL;
                        break;
                    }
                }
            }

            var compNCM = new ControleProdutoController().ObterComposicaoNCM(material);
            if (compNCM != null)
                composicao = compNCM.COMPOSICAO;


            var desenvProduto = desenvController.ObterProduto(hb.COLECAO, hb.CODIGO_PRODUTO_LINX, hb.COR);
            if (desenvProduto != null && desenvProduto.SUBGRUPO_PRODUTO != null)
            {
                var ncmProduto = new ControleProdutoController().ObterProdutoNCM(desenvProduto.SUBGRUPO_PRODUTO, desenvProduto.GRIFFE, composicao, hb.GRUPO).Select(p => new PRODUTO_NCM
                {
                    CLASSIF_FISCAL = p.CLASSIF_FISCAL.Trim()
                }).Distinct().ToList();

                if (ncmProduto != null && ncmProduto.Count() > 0)
                    ncm = ncmProduto[0].CLASSIF_FISCAL;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Etiqueta de Composicao</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divEtiqueta' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>Etiqueta de Composição " + " - " + ((hb.MOSTRUARIO == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO") + " - Módulo de Produção</h3>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 600pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr>");
            sb.Append("                                <td style='width: 165px;'>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + hb.CODIGO_PRODUTO_LINX);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Nome:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + hb.NOME);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + hb.COR.Trim() + " - " + prodController.ObterCoresBasicas(hb.COR).DESC_COR);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    NCM:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + ncm);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            var comp = prodController.ObterListaComposicaoHB(Convert.ToInt32(hb.CODIGO)).Where(p => p.ETIQUETA_COMPOSICAO == 'S');
            var compAviamento = prodController.ObterListaComposicaoHBAviamento(Convert.ToInt32(hb.CODIGO)).Where(p => p.ETIQUETA_COMPOSICAO == 'S');

            List<SP_OBTER_COMPOSICAO_HBResult> compList = new List<SP_OBTER_COMPOSICAO_HBResult>();
            compList.AddRange(comp);
            foreach (var a in compAviamento)
                compList.Add(new SP_OBTER_COMPOSICAO_HBResult { COMPOSICAO = a.COMPOSICAO, DESCRICAO = a.DESCRICAO, ETIQUETA_COMPOSICAO = a.ETIQUETA_COMPOSICAO });

            if (compList != null && compList.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Composição:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + compList[0].DESCRICAO + " - " + compList[0].COMPOSICAO);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                foreach (var c in compList.Where(p => p.DESCRICAO != compList[0].DESCRICAO))
                {
                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    &nbsp;");
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");

                    if (c.DESCRICAO.Trim().ToUpper() == "COR 2" || c.DESCRICAO.Trim().ToUpper() == "COR 3")
                        sb.Append("                                    DETALHE - " + c.COMPOSICAO);
                    else
                        sb.Append("                                    " + c.DESCRICAO + " - " + c.COMPOSICAO);

                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    &nbsp;");
                    sb.Append("                                </td>");
                    sb.Append("                            </tr>");
                }
            }
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                             <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Grade:");
            sb.Append("                                </td>");
            sb.Append("                                 <td colspan='2'>");
            sb.Append("                                     <table cellpadding='0' cellspacing='0' style='width: 400pt; padding: 0px; color: black;");
            sb.Append("                                         font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            sb.Append("                                         border: 1px solid #ccc;'>");
            sb.Append("                                         <tr style='background-color: #ccc;'>");
            sb.Append("                                             <td>");
            sb.Append("                                                 &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center;'>");
            sb.Append("                                                  &nbsp;");
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");

            var _grade = prodController.ObterGradeHB(hb.CODIGO, 3);
            sb.Append("                                         <tr>");
            sb.Append("                                             <td>");
            sb.Append("                                                 " + prodController.ObterCoresBasicas(hb.COR.Trim()).DESC_COR.Trim());
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_EXP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_XP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_PP)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_P)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_M)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_G)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_GG)));
            sb.Append("                                             </td>");
            sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            sb.Append("                                                  " + (((_grade == null) ? 0 : (_grade.GRADE_EXP + _grade.GRADE_XP + _grade.GRADE_PP + _grade.GRADE_P + _grade.GRADE_M + _grade.GRADE_G + _grade.GRADE_GG))));
            sb.Append("                                             </td>");
            sb.Append("                                         </tr>");
            sb.Append("                                     </table>");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string mat = "";
            var materialLinx = desenvController.ObterMaterial(material);
            if (materialLinx != null)
                mat = "TECIDO: " + materialLinx.GRUPO + " / " + materialLinx.SUBGRUPO + " - FORNECEDOR: " + materialLinx.FABRICANTE;

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Lavagem:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                     " + mat);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            foreach (var lav in desenvController.ObterMaterialLavagem(material))
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                     <img alt='' Width='30px' Height='30px' src='http://cmaxweb.dnsalias.com:8585" + lav.IMAGEM.Replace("~", "") + "' />&nbsp;");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                 </td>");
            sb.Append("                             </tr>");
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

    }
}