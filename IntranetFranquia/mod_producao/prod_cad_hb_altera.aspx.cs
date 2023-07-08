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
    public partial class prod_cad_hb_altera : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        int colunaComposicao, colunaAviamento = 0;
        int colunaComposicaoDetCor2, colunaComposicaoDetCor3, colunaComposicaoDetColante, colunaComposicaoDetForro, colunaComposicaoDetGalao, colunaComposicaoDetGalaoProprio = 0;

        PROD_GRADE _gradeNome = new PROD_GRADE();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida queryString
            if (Request.QueryString["c"] == null || Request.QueryString["c"] == "")
                Response.Redirect("prod_menu.aspx");

            //RecarregarAviamentoDropDownList();

            if (!Page.IsPostBack)
            {
                bool hbValido = true;
                string codigo = Request.QueryString["c"].ToString();
                hidHB.Value = codigo;

                //Valida HB
                PROD_HB prod_hb = ObterHB(Convert.ToInt32(codigo));
                if (prod_hb == null)
                {
                    hbValido = false;
                    labMensagem.Text = "NÃO ENCONTRADO";
                }

                if (prod_hb.STATUS.ToString() == "E" || prod_hb.STATUS.ToString() == "X") //EXCLUIDO e //PERDIDO
                {
                    hbValido = false;
                    labMensagem.Text = "STATUS NÃO PERMITIDO";
                }

                if (!hbValido)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Ok: function () { window.open('prod_menu.aspx', '_self'); } } }); });", true);
                    return;
                }

                //Carregar Combos
                CarregarGrupo();
                CarregarGrupoMaterial("");
                CarregarColecoes();
                CarregarFornecedores();
                //CarregarSubGrupo("");
                CarregarGrades();
                CarregarHB(prod_hb);
                CarregarCores("0");


                hidStatus.Value = prod_hb.STATUS.ToString();

                cbCor2_CheckedChanged(cbCor2Alt, null);
                cbCor3_CheckedChanged(cbCor3Alt, null);
                cbColante_CheckedChanged(cbColanteAlt, null);
                cbForro_CheckedChanged(cbForroAlt, null);
                cbGalao_CheckedChanged(cbGalaoAlt, null);
                cbGalaoProprio_CheckedChanged(cbGalaoProprioAlt, null);

                divEscondeGalaoProprio.Visible = false;
                dialogPai.Visible = false;

                Session["COMP_COR2"] = null;
                Session["COMP_COR3"] = null;
                Session["COMP_COLANTE"] = null;
                Session["COMP_FORRO"] = null;
                Session["COMP_GALAO"] = null;
                Session["COMP_GALAOPROPRIO"] = null;

                if (prod_hb.MOSTRUARIO == 'S' && prod_hb.STATUS == 'B')
                    fsDetalhes.Visible = false;
            }

            //Evitar duplo clique no botão
            btEnviar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviar, null) + ";");
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

        #region "DADOS INICIAIS"
        private PROD_HB ObterHB(int codigo)
        {
            return prodController.ObterHB(codigo);
        }
        private void CarregarHB(PROD_HB prod_hb)
        {
            try
            {
                ddlColecoes.SelectedValue = prod_hb.COLECAO;
                txtHB.Text = prod_hb.HB.ToString();
                ddlGrupo.SelectedValue = prod_hb.GRUPO;
                txtNome.Text = prod_hb.NOME;
                txtData.Text = prod_hb.DATA_INCLUSAO.ToString("dd/MM/yyyy");
                ddlGrupoTecido.SelectedValue = desenvController.ObterMaterialGrupo(prod_hb.GRUPO_TECIDO).GRUPO;
                CarregarSubGrupo(prod_hb.GRUPO_TECIDO, "T");
                ddlSubGrupoTecido.SelectedValue = prod_hb.TECIDO;
                CarregarCorFornecedor(prod_hb.GRUPO_TECIDO, prod_hb.TECIDO, ddlCorFornecedor);
                ddlCorFornecedor.SelectedValue = prod_hb.COR_FORNECEDOR;

                txtPedidoNumero.Text = prod_hb.NUMERO_PEDIDO.ToString();

                var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == prod_hb.FORNECEDOR.Trim()).Take(1).SingleOrDefault();
                if (f != null)
                    ddlFornecedor.SelectedValue = f.FORNECEDOR;
                txtLargura.Text = prod_hb.LARGURA.ToString();
                txtModelagem.Text = prod_hb.MODELAGEM;
                cbLiquidar.Checked = (prod_hb.LIQUIDAR == 'S') ? true : false;
                cbAtacado.Checked = (prod_hb.ATACADO == 'S') ? true : false;
                txtCustoTecido.Text = prod_hb.CUSTO_TECIDO.ToString();
                imgFotoTecido.ImageUrl = prod_hb.FOTO_TECIDO;
                txtCodigoLinx.Text = prod_hb.CODIGO_PRODUTO_LINX.ToString().Trim();
                txtMolde.Text = prod_hb.MOLDE.ToString();
                if (prod_hb.GASTO_FOLHA_SIMULACAO != null)
                    txtGastoPorFolha.Text = prod_hb.GASTO_FOLHA_SIMULACAO.ToString();

                txtPrecoFaccMostruario.Text = (prod_hb.PRECO_FACC_MOSTRUARIO == null) ? "" : prod_hb.PRECO_FACC_MOSTRUARIO.ToString();

                CarregarCores(txtCodigoLinx.Text);
                ddlCor.SelectedValue = prod_hb.COR;

                if (prod_hb.TIPO.ToString() == "A")
                    rdbAmpliacao.Checked = true;
                else if (prod_hb.TIPO.ToString() == "R")
                    rdbRisco.Checked = true;

                cbMostruario.Checked = (prod_hb.MOSTRUARIO == 'S') ? true : false;

                if (prod_hb.GABARITO != null)
                    ddlGabarito.SelectedValue = prod_hb.GABARITO.ToString();
                if (prod_hb.ESTAMPARIA != null)
                    ddlEstamparia.SelectedValue = prod_hb.ESTAMPARIA.ToString();

                RecarregarComposicao();

                if (prod_hb.PROD_GRADE == null)
                    prod_hb.PROD_GRADE = 1;
                ddlGrade.SelectedValue = prod_hb.PROD_GRADE.ToString();
                ddlGrade_SelectedIndexChanged(ddlGrade, null);

                PROD_HB_GRADE _grade = null;
                _grade = prodController.ObterGradeHB(prod_hb.CODIGO, 2); //RISCO
                if (_grade == null)
                    _grade = prodController.ObterGradeHB(prod_hb.CODIGO, 1);//AMPLIAÇÃO

                if (_grade != null)
                {
                    txtGradeEXP.Text = _grade.GRADE_EXP.ToString();
                    txtGradeXP.Text = _grade.GRADE_XP.ToString();
                    txtGradePP.Text = _grade.GRADE_PP.ToString();
                    txtGradeP.Text = _grade.GRADE_P.ToString();
                    txtGradeM.Text = _grade.GRADE_M.ToString();
                    txtGradeG.Text = _grade.GRADE_G.ToString();
                    txtGradeGG.Text = _grade.GRADE_GG.ToString();

                    int totalGrade = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG));
                    txtGradeTotal.Text = totalGrade.ToString();
                }

                CarregarNomeGrade(prod_hb);

                RecarregarAviamento();

                imgFotoPeca.ImageUrl = prod_hb.FOTO_PECA;
                txtObservacao.Text = prod_hb.OBSERVACAO;

                //Controle de Detalhes
                cbCor2Inc.Visible = true;
                cbCor2Alt.Visible = false;
                cbCor3Inc.Visible = true;
                cbCor3Alt.Visible = false;
                cbColanteInc.Visible = true;
                cbColanteAlt.Visible = false;
                cbForroInc.Visible = true;
                cbForroAlt.Visible = false;
                cbGalaoInc.Visible = true;
                cbGalaoAlt.Visible = false;
                cbGalaoProprioInc.Visible = true;
                cbGalaoProprioAlt.Visible = false;

                List<PROD_HB> _det = prodController.ObterDetalhesHB(prod_hb.CODIGO);
                List<PROD_HB_COMPOSICAO> comp = null;
                foreach (PROD_HB p in _det)
                {
                    if (p.PROD_DETALHE == 1) //COR 2
                    {
                        fsCor2.Visible = true;
                        ddlDetalheGrupoTecidoCor2.SelectedValue = p.GRUPO_TECIDO;
                        CarregarSubGrupo(p.GRUPO_TECIDO, "COR2");
                        ddlDetalheSubGrupoTecidoCor2.SelectedValue = p.TECIDO.Trim();
                        ddlDetalheCorCor2.SelectedValue = p.COR;
                        CarregarCorFornecedor(p.GRUPO_TECIDO.Trim(), p.TECIDO.Trim(), ddlDetalheCorFornecedorCor2);
                        ddlDetalheCorFornecedorCor2.SelectedValue = p.COR_FORNECEDOR;
                        txtDetalheNumeroPedidoCor2.Text = p.NUMERO_PEDIDO.ToString();
                        ddlDetalheFornecedorCor2.SelectedValue = p.FORNECEDOR;
                        txtDetalheMoldeCor2.Text = p.MOLDE.ToString();
                        txtDetalheLarguraCor2.Text = p.LARGURA.ToString();
                        txtDetalheCustoTecidoCor2.Text = p.CUSTO_TECIDO.ToString();
                        if (p.GASTO_FOLHA_SIMULACAO != null)
                            txtDetalheGastoPorFolhaCor2.Text = p.GASTO_FOLHA_SIMULACAO.ToString();

                        if (p.ETIQUETA_COMPOSICAO != null)
                            ddlEtiquetaComposicaoCor2.SelectedValue = p.ETIQUETA_COMPOSICAO.ToString();

                        imgFotoTecidoDetalheCor2.ImageUrl = p.FOTO_TECIDO;
                        txtDetalheObservacaoCor2.Text = p.OBSERVACAO;
                        hidCor2.Value = p.CODIGO.ToString();
                        cbCor2Alt.Visible = true;
                        cbCor2Inc.Visible = false;
                        RecarregarComp(comp, gvComposicaoDetalheCor2, p.CODIGO, hidDetalheCompTotalCor2, null);
                    }

                    if (p.PROD_DETALHE == 6) //COR 3
                    {
                        fsCor3.Visible = true;
                        ddlDetalheGrupoTecidoCor3.SelectedValue = p.GRUPO_TECIDO;
                        CarregarSubGrupo(p.GRUPO_TECIDO, "COR3");
                        ddlDetalheSubGrupoTecidoCor3.SelectedValue = p.TECIDO.Trim();
                        ddlDetalheCorCor3.SelectedValue = p.COR;
                        CarregarCorFornecedor(p.GRUPO_TECIDO.Trim(), p.TECIDO.Trim(), ddlDetalheCorFornecedorCor3);
                        ddlDetalheCorFornecedorCor3.SelectedValue = p.COR_FORNECEDOR;
                        txtDetalheNumeroPedidoCor3.Text = p.NUMERO_PEDIDO.ToString();
                        ddlDetalheFornecedorCor3.SelectedValue = p.FORNECEDOR;
                        txtDetalheMoldeCor3.Text = p.MOLDE.ToString();
                        txtDetalheLarguraCor3.Text = p.LARGURA.ToString();
                        txtDetalheCustoTecidoCor3.Text = p.CUSTO_TECIDO.ToString();
                        if (p.GASTO_FOLHA_SIMULACAO != null)
                            txtDetalheGastoPorFolhaCor3.Text = p.GASTO_FOLHA_SIMULACAO.ToString();

                        if (p.ETIQUETA_COMPOSICAO != null)
                            ddlEtiquetaComposicaoCor3.SelectedValue = p.ETIQUETA_COMPOSICAO.ToString();

                        imgFotoTecidoDetalheCor3.ImageUrl = p.FOTO_TECIDO;
                        txtDetalheObservacaoCor3.Text = p.OBSERVACAO;
                        hidCor3.Value = p.CODIGO.ToString();
                        cbCor3Alt.Visible = true;
                        cbCor3Inc.Visible = false;
                        RecarregarComp(comp, gvComposicaoDetalheCor3, p.CODIGO, hidDetalheCompTotalCor3, null);
                    }

                    if (p.PROD_DETALHE == 2) //COLANTE
                    {
                        fsColante.Visible = true;
                        ddlDetalheGrupoTecidoColante.SelectedValue = p.GRUPO_TECIDO;
                        CarregarSubGrupo(p.GRUPO_TECIDO, "COLANTE");
                        ddlDetalheSubGrupoTecidoColante.SelectedValue = p.TECIDO.Trim();
                        ddlDetalheCorColante.SelectedValue = p.COR;
                        CarregarCorFornecedor(p.GRUPO_TECIDO.Trim(), p.TECIDO.Trim(), ddlDetalheCorFornecedorColante);
                        ddlDetalheCorFornecedorColante.SelectedValue = p.COR_FORNECEDOR;
                        txtDetalheNumeroPedidoColante.Text = p.NUMERO_PEDIDO.ToString();
                        ddlDetalheFornecedorColante.SelectedValue = p.FORNECEDOR;
                        txtDetalheMoldeColante.Text = p.MOLDE.ToString();
                        txtDetalheLarguraColante.Text = p.LARGURA.ToString();
                        txtDetalheCustoTecidoColante.Text = p.CUSTO_TECIDO.ToString();
                        if (p.GASTO_FOLHA_SIMULACAO != null)
                            txtDetalheGastoPorFolhaColante.Text = p.GASTO_FOLHA_SIMULACAO.ToString();

                        if (p.ETIQUETA_COMPOSICAO != null)
                            ddlEtiquetaComposicaoColante.SelectedValue = p.ETIQUETA_COMPOSICAO.ToString();

                        imgFotoTecidoDetalheColante.ImageUrl = p.FOTO_TECIDO;
                        txtDetalheObservacaoColante.Text = p.OBSERVACAO;
                        hidColante.Value = p.CODIGO.ToString();
                        cbColanteAlt.Visible = true;
                        cbColanteInc.Visible = false;
                        RecarregarComp(comp, gvComposicaoDetalheColante, p.CODIGO, hidDetalheCompTotalColante, null);
                    }

                    if (p.PROD_DETALHE == 3) //FORRO
                    {
                        fsForro.Visible = true;
                        ddlDetalheGrupoTecidoForro.SelectedValue = p.GRUPO_TECIDO;
                        CarregarSubGrupo(p.GRUPO_TECIDO, "FORRO");
                        ddlDetalheSubGrupoTecidoForro.SelectedValue = p.TECIDO.Trim();
                        ddlDetalheCorForro.SelectedValue = p.COR;
                        CarregarCorFornecedor(p.GRUPO_TECIDO.Trim(), p.TECIDO.Trim(), ddlDetalheCorFornecedorForro);
                        ddlDetalheCorFornecedorForro.SelectedValue = p.COR_FORNECEDOR;
                        txtDetalheNumeroPedidoForro.Text = p.NUMERO_PEDIDO.ToString();
                        ddlDetalheFornecedorForro.SelectedValue = p.FORNECEDOR;
                        txtDetalheMoldeForro.Text = p.MOLDE.ToString();
                        txtDetalheLarguraForro.Text = p.LARGURA.ToString();
                        txtDetalheCustoTecidoForro.Text = p.CUSTO_TECIDO.ToString();
                        if (p.GASTO_FOLHA_SIMULACAO != null)
                            txtDetalheGastoPorFolhaForro.Text = p.GASTO_FOLHA_SIMULACAO.ToString();

                        if (p.ETIQUETA_COMPOSICAO != null)
                            ddlEtiquetaComposicaoForro.SelectedValue = p.ETIQUETA_COMPOSICAO.ToString();

                        imgFotoTecidoDetalheForro.ImageUrl = p.FOTO_TECIDO;
                        txtDetalheObservacaoForro.Text = p.OBSERVACAO;
                        hidForro.Value = p.CODIGO.ToString();
                        cbForroAlt.Visible = true;
                        cbForroInc.Visible = false;
                        RecarregarComp(comp, gvComposicaoDetalheForro, p.CODIGO, hidDetalheCompTotalForro, null);
                    }

                    if (p.PROD_DETALHE == 4) //GALAO
                    {
                        fsGalao.Visible = true;
                        ddlDetalheGrupoTecidoGalao.SelectedValue = p.GRUPO_TECIDO;
                        CarregarSubGrupo(p.GRUPO_TECIDO, "GALAO");
                        ddlDetalheSubGrupoTecidoGalao.SelectedValue = p.TECIDO.Trim();
                        ddlDetalheCorGalao.SelectedValue = p.COR;
                        CarregarCorFornecedor(p.GRUPO_TECIDO.Trim(), p.TECIDO.Trim(), ddlDetalheCorFornecedorGalao);
                        ddlDetalheCorFornecedorGalao.SelectedValue = p.COR_FORNECEDOR;
                        txtDetalheNumeroPedidoGalao.Text = p.NUMERO_PEDIDO.ToString();
                        ddlDetalheFornecedorGalao.SelectedValue = p.FORNECEDOR;
                        txtDetalheMoldeGalao.Text = p.MOLDE.ToString();
                        txtDetalheLarguraGalao.Text = p.LARGURA.ToString();
                        txtDetalheCustoTecidoGalao.Text = p.CUSTO_TECIDO.ToString();
                        if (p.GASTO_FOLHA_SIMULACAO != null)
                            txtDetalheGastoPorFolhaGalao.Text = p.GASTO_FOLHA_SIMULACAO.ToString();

                        if (p.ETIQUETA_COMPOSICAO != null)
                            ddlEtiquetaComposicaoGalao.SelectedValue = p.ETIQUETA_COMPOSICAO.ToString();

                        imgFotoTecidoDetalheGalao.ImageUrl = p.FOTO_TECIDO;
                        txtDetalheObservacaoGalao.Text = p.OBSERVACAO;
                        hidGalao.Value = p.CODIGO.ToString();
                        cbGalaoAlt.Visible = true;
                        cbGalaoInc.Visible = false;
                        RecarregarComp(comp, gvComposicaoDetalheGalao, p.CODIGO, hidDetalheCompTotalGalao, null);
                    }

                    if (p.PROD_DETALHE == 5) //GALAO PRÓPRIO
                    {
                        fsGalaoProprio.Visible = true;
                        ddlDetalheGrupoTecidoGalaoProprio.SelectedValue = p.GRUPO_TECIDO;
                        //CarregarSubGrupo(p.GRUPO_TECIDO, "GALAOPROPRIO");
                        ddlDetalheSubGrupoTecidoGalaoProprio.SelectedValue = p.TECIDO.Trim();
                        ddlDetalheCorGalaoProprio.SelectedValue = p.COR;
                        CarregarCorFornecedor(p.GRUPO_TECIDO.Trim(), p.TECIDO.Trim(), ddlDetalheCorFornecedorGalaoProprio);
                        ddlDetalheCorFornecedorGalaoProprio.SelectedValue = p.COR_FORNECEDOR;
                        txtDetalheNumeroPedidoGalaoProprio.Text = p.NUMERO_PEDIDO.ToString();
                        ddlDetalheFornecedorGalaoProprio.SelectedValue = p.FORNECEDOR;
                        txtDetalheMoldeGalaoProprio.Text = p.MOLDE.ToString();
                        txtDetalheLarguraGalaoProprio.Text = p.LARGURA.ToString();
                        txtDetalheCustoTecidoGalaoProprio.Text = p.CUSTO_TECIDO.ToString();
                        if (p.GASTO_FOLHA_SIMULACAO != null)
                            txtDetalheGastoPorFolhaGalaoProprio.Text = p.GASTO_FOLHA_SIMULACAO.ToString();

                        if (p.ETIQUETA_COMPOSICAO != null)
                            ddlEtiquetaComposicaoGalaoProprio.SelectedValue = p.ETIQUETA_COMPOSICAO.ToString();

                        imgFotoTecidoDetalheGalaoProprio.ImageUrl = p.FOTO_TECIDO;
                        txtDetalheObservacaoGalaoProprio.Text = p.OBSERVACAO;
                        hidGalaoProprio.Value = p.CODIGO.ToString();
                        cbGalaoProprioAlt.Visible = true;
                        cbGalaoProprioInc.Visible = false;
                        RecarregarComp(comp, gvComposicaoDetalheGalaoProprio, p.CODIGO, hidDetalheCompTotalGalaoProprio, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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

                    ddlDetalheGrupoTecidoCor2.DataSource = _matGrupo;
                    ddlDetalheGrupoTecidoCor2.DataBind();

                    ddlDetalheGrupoTecidoCor3.DataSource = _matGrupo;
                    ddlDetalheGrupoTecidoCor3.DataBind();

                    ddlDetalheGrupoTecidoColante.DataSource = _matGrupo;
                    ddlDetalheGrupoTecidoColante.DataBind();

                    ddlDetalheGrupoTecidoForro.DataSource = _matGrupo;
                    ddlDetalheGrupoTecidoForro.DataBind();

                    ddlDetalheGrupoTecidoGalao.DataSource = _matGrupo;
                    ddlDetalheGrupoTecidoGalao.DataBind();

                    ddlDetalheGrupoTecidoGalaoProprio.DataSource = _matGrupo;
                    ddlDetalheGrupoTecidoGalaoProprio.DataBind();

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

            if (ddl.ID.Contains("ddlDetalheGrupoTecido"))
            {
                if (ddl.ID.ToUpper().Contains("COR2"))
                    CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "COR2");

                if (ddl.ID.ToUpper().Contains("COR3"))
                    CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "COR3");

                if (ddl.ID.ToUpper().Contains("FORRO"))
                    CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "FORRO");

                if (ddl.ID.ToUpper().Contains("COLANTE"))
                    CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "COLANTE");

                if (ddl.ID.ToUpper().Contains("GALAO"))
                    CarregarSubGrupo(ddl.SelectedItem.Text.Trim(), "GALAO");
            }
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

                ddlDetalheCorCor2.DataSource = produtoCor;
                ddlDetalheCorCor2.DataBind();

                ddlDetalheCorCor3.DataSource = produtoCor;
                ddlDetalheCorCor3.DataBind();

                ddlDetalheCorColante.DataSource = produtoCor;
                ddlDetalheCorColante.DataBind();

                ddlDetalheCorForro.DataSource = produtoCor;
                ddlDetalheCorForro.DataBind();

                ddlDetalheCorGalao.DataSource = produtoCor;
                ddlDetalheCorGalao.DataBind();

                ddlDetalheCorGalaoProprio.DataSource = produtoCor;
                ddlDetalheCorGalaoProprio.DataBind();

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

                if (tipo == "T")
                {
                    ddlSubGrupoTecido.DataSource = _matSubGrupo;
                    ddlSubGrupoTecido.DataBind();

                    ddlDetalheSubGrupoTecidoGalaoProprio.DataSource = _matSubGrupo;
                    ddlDetalheSubGrupoTecidoGalaoProprio.DataBind();
                }
                else if (tipo == "COR2")
                {
                    ddlDetalheSubGrupoTecidoCor2.DataSource = _matSubGrupo;
                    ddlDetalheSubGrupoTecidoCor2.DataBind();
                }
                else if (tipo == "COR3")
                {
                    ddlDetalheSubGrupoTecidoCor3.DataSource = _matSubGrupo;
                    ddlDetalheSubGrupoTecidoCor3.DataBind();
                }
                else if (tipo == "FORRO")
                {
                    ddlDetalheSubGrupoTecidoForro.DataSource = _matSubGrupo;
                    ddlDetalheSubGrupoTecidoForro.DataBind();
                }
                else if (tipo == "COLANTE")
                {
                    ddlDetalheSubGrupoTecidoColante.DataSource = _matSubGrupo;
                    ddlDetalheSubGrupoTecidoColante.DataBind();
                }
                else if (tipo == "GALAO")
                {
                    ddlDetalheSubGrupoTecidoGalao.DataSource = _matSubGrupo;
                    ddlDetalheSubGrupoTecidoGalao.DataBind();
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
                else if (_ddl.ID.ToUpper().Contains("COR2"))
                    CarregarCorFornecedor(ddlDetalheGrupoTecidoCor2.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlDetalheCorFornecedorCor2);
                else if (_ddl.ID.ToUpper().Contains("COR3"))
                    CarregarCorFornecedor(ddlDetalheGrupoTecidoCor3.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlDetalheCorFornecedorCor3);
                else if (_ddl.ID.ToUpper().Contains("FORRO"))
                    CarregarCorFornecedor(ddlDetalheGrupoTecidoForro.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlDetalheCorFornecedorForro);
                else if (_ddl.ID.ToUpper().Contains("COLANTE"))
                    CarregarCorFornecedor(ddlDetalheGrupoTecidoColante.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlDetalheCorFornecedorColante);
                else if (_ddl.ID.ToUpper().Contains("GALAO"))
                    CarregarCorFornecedor(ddlDetalheGrupoTecidoGalao.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlDetalheCorFornecedorGalao);
                else if (_ddl.ID == "ddlSubGrupoAviamento")
                    CarregarCorFornecedor(ddlGrupoAviamento.SelectedItem.Text.Trim(), _ddl.SelectedItem.Text.Trim(), ddlCorFornecedorAviamento);
            }
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, DropDownList _ddlCorFornecedor)
        {
            //Obter materiais
            //var _material = new DesenvolvimentoController().ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim());
            ////Obter cores do material
            //var _matCores = new DesenvolvimentoController().ObterMaterialCor();
            //_matCores = _matCores.Where(p => _material.Any(c => c.MATERIAL.Trim() == p.MATERIAL.Trim())).OrderBy(x => x.DESC_COR_MATERIAL).ToList();

            var _matCores = desenvController.ObterMaterialCorV2(grupo.Trim(), subGrupo.Trim(), "");

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

            _cores = _cores.OrderBy(p => p.REFER_FABRICANTE.Trim()).ToList();
            _cores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "Selecione" });

            if (_cores != null && _cores.Count() <= 1)
                _cores.Add(new MATERIAIS_CORE { REFER_FABRICANTE = "OUTROS" });

            _ddlCorFornecedor.DataSource = _cores;
            _ddlCorFornecedor.DataBind();
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
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "Selecione", STATUS = 'S' });

                var tecidoFornecedor = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S');

                if (tecidoFornecedor != null && tecidoFornecedor.Count() > 0)
                {
                    ddlFornecedor.DataSource = tecidoFornecedor;
                    ddlFornecedor.DataBind();

                    ddlDetalheFornecedorCor2.DataSource = tecidoFornecedor;
                    ddlDetalheFornecedorCor2.DataBind();

                    ddlDetalheFornecedorCor3.DataSource = tecidoFornecedor;
                    ddlDetalheFornecedorCor3.DataBind();

                    ddlDetalheFornecedorForro.DataSource = tecidoFornecedor;
                    ddlDetalheFornecedorForro.DataBind();

                    ddlDetalheFornecedorColante.DataSource = tecidoFornecedor;
                    ddlDetalheFornecedorColante.DataBind();

                    ddlDetalheFornecedorGalao.DataSource = tecidoFornecedor;
                    ddlDetalheFornecedorGalao.DataBind();
                }

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
        private void CarregarDropDownList(ArrayList _list, DropDownList _ddl)
        {
            _ddl.DataSource = _list;
            _ddl.DataTextField = "Text";
            _ddl.DataValueField = "Value";
            _ddl.DataBind();
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
        private void CarregarGrades()
        {
            List<PROD_GRADE> _grade = prodController.ObterGradeNome();

            ddlGrade.DataSource = _grade;
            ddlGrade.DataBind();
        }

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
        protected void btFotoTecidoDetalheIncluirCor2_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecidoDetalheCor2, imgFotoTecidoDetalheCor2, "");
            }
            catch (Exception ex)
            {
                labErroFotoTecidoDetalheCor2.Text = "ERRO: (btFotoTecidoDetalheIncluirCor2_Click) \n\n" + ex.Message;
            }
        }
        protected void btFotoTecidoDetalheIncluirCor3_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecidoDetalheCor3, imgFotoTecidoDetalheCor3, "");
            }
            catch (Exception ex)
            {
                labErroFotoTecidoDetalheCor3.Text = "ERRO: (btFotoTecidoDetalheIncluirCor3_Click) \n\n" + ex.Message;
            }
        }
        protected void btFotoTecidoDetalheIncluirForro_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecidoDetalheForro, imgFotoTecidoDetalheForro, "");
            }
            catch (Exception ex)
            {
                labErroFotoTecidoDetalheForro.Text = "ERRO: (btFotoTecidoDetalheIncluirForro_Click) \n\n" + ex.Message;
            }
        }
        protected void btFotoTecidoDetalheIncluirColante_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecidoDetalheColante, imgFotoTecidoDetalheColante, "");
            }
            catch (Exception ex)
            {
                labErroFotoTecidoDetalheColante.Text = "ERRO: (btFotoTecidoDetalheIncluirColante_Click) \n\n" + ex.Message;
            }
        }
        protected void btFotoTecidoDetalheIncluirGalao_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecidoDetalheGalao, imgFotoTecidoDetalheGalao, "");
            }
            catch (Exception ex)
            {
                labErroFotoTecidoDetalheGalao.Text = "ERRO: (btFotoTecidoDetalheIncluirGalão_Click) \n\n" + ex.Message;
            }
        }
        protected void btFotoTecidoDetalheIncluirGalaoProprio_Click(object sender, EventArgs e)
        {
            try
            {
                GravarFoto(uploadFotoTecidoDetalheGalaoProprio, imgFotoTecidoDetalheGalaoProprio, "");
            }
            catch (Exception ex)
            {
                labErroFotoTecidoDetalheGalaoProprio.Text = "ERRO: (btFotoTecidoDetalheIncluirGalaoProprio_Click) \n\n" + ex.Message;
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
                decimal total = 0;
                foreach (PROD_HB_COMPOSICAO c in _list)
                {
                    total = total + c.QTDE;
                }
                hidTotalComp.Value = Convert.ToDecimal(total).ToString();
            }
        }
        protected void btComposicaoIncluir_Click(object sender, EventArgs e)
        {
            labErroComposicao.Text = "";
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

            try
            {
                PROD_HB_COMPOSICAO _novo = null;
                _novo = new PROD_HB_COMPOSICAO();
                _novo.QTDE = Convert.ToDecimal(txtComposicaoQtde.Text);
                _novo.DESCRICAO = txtComposicaoDescricao.Text.Trim().ToUpper();
                _novo.PROD_HB = Convert.ToInt32(hidHB.Value);
                IncluirComposicao(_novo);

                //Verificar se tem GALAO PROPRIO
                PROD_HB galaoProprio = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value), 5);
                if (galaoProprio != null)
                {
                    _novo = new PROD_HB_COMPOSICAO();
                    _novo.QTDE = Convert.ToDecimal(txtComposicaoQtde.Text);
                    _novo.DESCRICAO = txtComposicaoDescricao.Text.Trim().ToUpper();
                    _novo.PROD_HB = galaoProprio.CODIGO;
                    IncluirComposicao(_novo);
                }

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

                    //Verificar se tem GALAO PROPRIO
                    PROD_HB galaoProprio = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value), 5);
                    if (galaoProprio != null)
                    {
                        List<PROD_HB_COMPOSICAO> comp = prodController.ObterComposicaoHB(galaoProprio.CODIGO);
                        PROD_HB_COMPOSICAO _comp = comp.Find(p => p.QTDE == Convert.ToDecimal(g.Cells[1].Text) && p.DESCRICAO == g.Cells[2].Text.Trim().ToUpper());
                        if (_comp != null)
                            ExcluirComposicao(_comp.CODIGO);
                    }

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
                            _labQtde.Text = _aviamento.QTDE.ToString().Replace(",000", "");

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
        private void CriarHB()
        {
            PROD_HB _prod = new PROD_HB();
            _prod.DATA_INCLUSAO = DateTime.Now;
            _prod.STATUS = 'X';
            hidHB.Value = prodController.CriarHB(_prod).ToString();
        }
        private int AtualizarHB(PROD_HB _hb)
        {
            return prodController.AtualizarHB(_hb);
        }
        private void AtualizarGrade(PROD_HB_GRADE _grade, int processo)
        {
            prodController.AtualizarGrade(_grade, processo);
        }
        private void IncluirProcesso(PROD_HB_PROD_PROCESSO _processo)
        {
            prodController.InserirProcesso(_processo);
        }
        private int ObterProcessoOrdem(int _ordem)
        {
            return prodController.ObterProcessoOrdem(_ordem).CODIGO;
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
                    labCodigoLinx.ForeColor = System.Drawing.Color.Gray;
                    if (txtCodigoLinx.Text.Trim() != "")
                    {
                        codigoProduto = txtCodigoLinx.Text.Trim();
                        //CarregarCores(codigoProduto);

                        PRODUTO _produto = new PRODUTO();
                        _produto = (new BaseController().BuscaProduto(codigoProduto));
                        if (_produto != null)
                        {
                            txtNome.Text = _produto.DESC_PRODUTO.Trim();
                            ddlGrupo.SelectedValue = _produto.GRUPO_PRODUTO;
                            CarregarCores(codigoProduto);
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
        protected void txtNumeroPedido_TextChanged(object sender, EventArgs e)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();

            try
            {

                TextBox _txtNumeroPedido = (TextBox)sender;
                if (_txtNumeroPedido != null && _txtNumeroPedido.Text != "")
                {
                    int numeroPedido = Convert.ToInt32(_txtNumeroPedido.Text);
                    var pedido = desenvController.ObterPedidoNumero(numeroPedido);
                    if (pedido != null)
                    {
                        if (_txtNumeroPedido.ID == "txtPedidoNumero")
                        {
                            PreencherTecido(pedido, "T", txtCustoTecido, ddlGrupoTecido, ddlSubGrupoTecido, ddlCor, ddlCorFornecedor, ddlFornecedor);
                        }
                        else if (_txtNumeroPedido.ID == "txtDetalheNumeroPedidoCor2")
                        {
                            PreencherTecido(pedido, "COR2", txtDetalheCustoTecidoCor2, ddlDetalheGrupoTecidoCor2, ddlDetalheSubGrupoTecidoCor2, ddlDetalheCorCor2, ddlDetalheCorFornecedorCor2, ddlDetalheFornecedorCor2);
                        }
                        else if (_txtNumeroPedido.ID == "txtDetalheNumeroPedidoCor3")
                        {
                            PreencherTecido(pedido, "COR3", txtDetalheCustoTecidoCor3, ddlDetalheGrupoTecidoCor3, ddlDetalheSubGrupoTecidoCor3, ddlDetalheCorCor3, ddlDetalheCorFornecedorCor3, ddlDetalheFornecedorCor3);
                        }
                        else if (_txtNumeroPedido.ID == "txtDetalheNumeroPedidoForro")
                        {
                            PreencherTecido(pedido, "FORRO", txtDetalheCustoTecidoForro, ddlDetalheGrupoTecidoForro, ddlDetalheSubGrupoTecidoForro, ddlDetalheCorForro, ddlDetalheCorFornecedorForro, ddlDetalheFornecedorForro);
                        }
                        else if (_txtNumeroPedido.ID == "txtDetalheNumeroPedidoColante")
                        {
                            PreencherTecido(pedido, "COLANTE", txtDetalheCustoTecidoColante, ddlDetalheGrupoTecidoColante, ddlDetalheSubGrupoTecidoColante, ddlDetalheCorColante, ddlDetalheCorFornecedorColante, ddlDetalheFornecedorColante);
                        }
                        else if (_txtNumeroPedido.ID == "txtDetalheNumeroPedidoGalao")
                        {
                            PreencherTecido(pedido, "GALAO", txtDetalheCustoTecidoGalao, ddlDetalheGrupoTecidoGalao, ddlDetalheSubGrupoTecidoGalao, ddlDetalheCorGalao, ddlDetalheCorFornecedorGalao, ddlDetalheFornecedorGalao);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void PreencherTecido(DESENV_PEDIDO pedido, string tipo, TextBox _txtCustoTecido, DropDownList _ddlGrupo, DropDownList _ddlSubGrupo, DropDownList _ddlCor, DropDownList _ddlCorFornecedor, DropDownList _ddlFornecedor)
        {
            _txtCustoTecido.Text = "";
            _ddlGrupo.SelectedValue = "Selecione";
            _ddlSubGrupo.SelectedValue = "Selecione";
            _ddlCor.SelectedValue = "0";
            _ddlCorFornecedor.SelectedValue = "Selecione";
            _ddlFornecedor.SelectedValue = "Selecione";

            _txtCustoTecido.Text = pedido.VALOR.ToString();
            if (pedido.VALOR == 0)
            {
                // obter custo do subpedido
                var subPedido = desenvController.ObterPedidoSubPorDesenvPedido(pedido.CODIGO).LastOrDefault();
                if (subPedido != null)
                {
                    _txtCustoTecido.Text = subPedido.VALOR.ToString();
                }
            }


            _ddlGrupo.SelectedValue = desenvController.ObterMaterialGrupo(pedido.GRUPO).GRUPO;
            CarregarSubGrupo(pedido.GRUPO, tipo);
            _ddlSubGrupo.SelectedValue = pedido.SUBGRUPO;
            _ddlCor.SelectedValue = prodController.ObterCoresBasicas(pedido.COR).COR;
            CarregarCorFornecedor(pedido.GRUPO, pedido.SUBGRUPO, _ddlCorFornecedor);
            _ddlCorFornecedor.SelectedValue = pedido.COR_FORNECEDOR;
            _ddlFornecedor.SelectedValue = prodController.ObterFornecedor(pedido.FORNECEDOR, 'T').FORNECEDOR;// pedido.FORNECEDOR;

        }
        protected void btEnviar_Click(object sender, EventArgs e)
        {
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
                //if (!ValidarHB(labErroEnvio, true))
                //return;

                labErroEnvio.Text = "<strong>Aguarde enquanto o relatório está sendo gerado...</strong>";

                //ATUALIZAR PRINCIPAL

                PROD_HB _prod_hb = new PROD_HB();
                _prod_hb = prodController.ObterHB(Convert.ToInt32(hidHB.Value));
                _prod_hb.HB = Convert.ToInt32(txtHB.Text);
                _prod_hb.COLECAO = ddlColecoes.SelectedValue;
                _prod_hb.GRUPO = ddlGrupo.SelectedValue;
                _prod_hb.NOME = txtNome.Text.Trim().ToUpper();
                _prod_hb.GRUPO_TECIDO = ddlGrupoTecido.SelectedValue.ToUpper();
                _prod_hb.TECIDO = ddlSubGrupoTecido.SelectedValue.Trim().ToUpper();
                _prod_hb.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
                _prod_hb.COR = ddlCor.SelectedValue;
                _prod_hb.COR_FORNECEDOR = ddlCorFornecedor.SelectedItem.Text.Trim();
                _prod_hb.FORNECEDOR = ddlFornecedor.SelectedValue;
                _prod_hb.LARGURA = Convert.ToDecimal(txtLargura.Text);
                _prod_hb.LIQUIDAR = (cbLiquidar.Checked) ? 'S' : 'N';
                _prod_hb.ATACADO = (cbAtacado.Checked) ? 'S' : 'N';
                _prod_hb.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
                _prod_hb.CUSTO_TECIDO = Convert.ToDecimal(txtCustoTecido.Text);
                _prod_hb.FOTO_TECIDO = imgFotoTecido.ImageUrl;
                _prod_hb.TIPO = (rdbAmpliacao.Checked) ? 'A' : 'R';
                _prod_hb.FOTO_PECA = imgFotoPeca.ImageUrl;
                _prod_hb.OBSERVACAO = txtObservacao.Text.Trim().ToUpper();
                _prod_hb.MOLDE = txtMolde.Text.Trim().ToUpper();
                //_prod_hb.STATUS = 'P';
                _prod_hb.CODIGO_PRODUTO_LINX = txtCodigoLinx.Text.Trim();
                _prod_hb.MOSTRUARIO = (cbMostruario.Checked) ? 'S' : 'N';
                if (txtGastoPorFolha.Text.Trim() != "")
                    _prod_hb.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtGastoPorFolha.Text.Trim());

                _prod_hb.PRECO_FACC_MOSTRUARIO = Convert.ToDecimal(txtPrecoFaccMostruario.Text.Trim());
                _prod_hb.PROD_GRADE = Convert.ToInt32(ddlGrade.SelectedValue);
                if (_prod_hb.PROD_GRADE == 2)
                    _prod_hb.KIDS = 'S';
                else
                    _prod_hb.KIDS = 'N';

                _prod_hb.ESTAMPARIA = Convert.ToChar(ddlEstamparia.SelectedValue);
                _prod_hb.GABARITO = Convert.ToChar(ddlGabarito.SelectedValue);

                //validar peso do tecido para calcular estoque
                var rendimento = prodController.ObterRendimentoTecidoKG(_prod_hb.NUMERO_PEDIDO.ToString());
                if (rendimento != null)
                    _prod_hb.RENDIMENTO_1MT = rendimento.RENDIMENTO_1MT;

                //Atualizar
                AtualizarHB(_prod_hb);

                if (hidStatus.Value != "B") //DIFERENTE DE BAIXADO
                {
                    //Atualizar GRADE PRINCIPAL
                    PROD_HB_GRADE _grade;
                    _grade = new PROD_HB_GRADE();
                    _grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                    _grade.PROD_PROCESSO = 2;
                    _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                    _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                    _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                    _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                    _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                    _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                    _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                    _grade.DATA_INCLUSAO = DateTime.Now;
                    _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    AtualizarGrade(_grade, 2); //ALTERACAO - ATUALIZAR GRADE RISCO

                    if (rdbAmpliacao.Checked) //AMPLIACAO, ATUALIZA AMPLIACAO
                    {
                        _grade = new PROD_HB_GRADE();
                        _grade.PROD_HB = Convert.ToInt32(hidHB.Value);
                        _grade.PROD_PROCESSO = 1;
                        _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                        _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                        _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                        _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                        _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                        _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                        _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                        _grade.DATA_INCLUSAO = DateTime.Now;
                        _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        AtualizarGrade(_grade, 1); //ALTERACAO - ATUALIZAR GRADE AMPLIACAO
                    }

                    //Atualizar GRADE DETALHE
                    AtualizarGradeDetalhe();
                }

                //ATUALIZAR DETALHES
                if (hidCor2.Value != "")
                    AtualizarDetalhe(prodController.ObterHB(Convert.ToInt32(hidCor2.Value)));
                if (hidCor3.Value != "")
                    AtualizarDetalhe(prodController.ObterHB(Convert.ToInt32(hidCor3.Value)));
                if (hidForro.Value != "")
                    AtualizarDetalhe(prodController.ObterHB(Convert.ToInt32(hidForro.Value)));
                if (hidColante.Value != "")
                    AtualizarDetalhe(prodController.ObterHB(Convert.ToInt32(hidColante.Value)));
                if (hidGalao.Value != "")
                    AtualizarDetalhe(prodController.ObterHB(Convert.ToInt32(hidGalao.Value)));
                if (hidGalaoProprio.Value != "")
                    AtualizarDetalhe(prodController.ObterHB(Convert.ToInt32(hidGalaoProprio.Value)));

                //INCLUIR DETALHES E GRADE
                if (cbCor2Inc.Checked)
                    IncluirDetalhe(1, ddlDetalheGrupoTecidoCor2, ddlDetalheSubGrupoTecidoCor2, txtDetalheNumeroPedidoCor2, ddlDetalheFornecedorCor2, txtDetalheLarguraCor2,
                        txtDetalheCustoTecidoCor2, imgFotoTecidoDetalheCor2, txtDetalheObservacaoCor2, gvComposicaoDetalheCor2, hidStatus.Value, txtDetalheMoldeCor2, ddlDetalheCorCor2, ddlDetalheCorFornecedorCor2, txtDetalheGastoPorFolhaCor2,
                        ddlEtiquetaComposicaoCor2);
                if (cbCor3Inc.Checked)
                    IncluirDetalhe(6, ddlDetalheGrupoTecidoCor3, ddlDetalheSubGrupoTecidoCor3, txtDetalheNumeroPedidoCor3, ddlDetalheFornecedorCor3, txtDetalheLarguraCor3,
                        txtDetalheCustoTecidoCor3, imgFotoTecidoDetalheCor3, txtDetalheObservacaoCor3, gvComposicaoDetalheCor3, hidStatus.Value, txtDetalheMoldeCor3, ddlDetalheCorCor3, ddlDetalheCorFornecedorCor3, txtDetalheGastoPorFolhaCor3,
                        ddlEtiquetaComposicaoCor3);
                if (cbColanteInc.Checked)
                    IncluirDetalhe(2, ddlDetalheGrupoTecidoColante, ddlDetalheSubGrupoTecidoColante, txtDetalheNumeroPedidoColante, ddlDetalheFornecedorColante, txtDetalheLarguraColante,
                        txtDetalheCustoTecidoColante, imgFotoTecidoDetalheColante, txtDetalheObservacaoColante, gvComposicaoDetalheColante, hidStatus.Value, txtDetalheMoldeColante, ddlDetalheCorColante, ddlDetalheCorFornecedorColante, txtDetalheGastoPorFolhaColante,
                        ddlEtiquetaComposicaoColante);
                if (cbForroInc.Checked)
                    IncluirDetalhe(3, ddlDetalheGrupoTecidoForro, ddlDetalheSubGrupoTecidoForro, txtDetalheNumeroPedidoForro, ddlDetalheFornecedorForro, txtDetalheLarguraForro,
                        txtDetalheCustoTecidoForro, imgFotoTecidoDetalheForro, txtDetalheObservacaoForro, gvComposicaoDetalheForro, hidStatus.Value, txtDetalheMoldeForro, ddlDetalheCorForro, ddlDetalheCorFornecedorForro, txtDetalheGastoPorFolhaForro,
                        ddlEtiquetaComposicaoForro);
                if (cbGalaoInc.Checked)
                    IncluirDetalhe(4, ddlDetalheGrupoTecidoGalao, ddlDetalheSubGrupoTecidoGalao, txtDetalheNumeroPedidoGalao, ddlDetalheFornecedorGalao, txtDetalheLarguraGalao,
                        txtDetalheCustoTecidoGalao, imgFotoTecidoDetalheGalao, txtDetalheObservacaoGalao, gvComposicaoDetalheGalao, hidStatus.Value, txtDetalheMoldeGalao, ddlDetalheCorGalao, ddlDetalheCorFornecedorGalao, txtDetalheGastoPorFolhaGalao,
                        ddlEtiquetaComposicaoGalao);
                if (cbGalaoProprioInc.Checked)
                    IncluirDetalhe(5, ddlGrupoTecido, ddlSubGrupoTecido, txtPedidoNumero, ddlFornecedor, txtLargura,
                        txtCustoTecido, imgFotoTecido, txtDetalheObservacaoGalaoProprio, gvComposicao, hidStatus.Value, txtMolde, ddlCor, ddlCorFornecedor, txtDetalheGastoPorFolhaGalaoProprio,
                        ddlEtiquetaComposicaoGalaoProprio);

                //Excluir DETALHE
                if (cbCor2Exc.Checked && hidCor2.Value != "")
                    prodController.ExcluirDetalheHB(Convert.ToInt32(hidCor2.Value));
                if (cbCor3Exc.Checked && hidCor3.Value != "")
                    prodController.ExcluirDetalheHB(Convert.ToInt32(hidCor3.Value));
                if (cbColanteExc.Checked && hidColante.Value != "")
                    prodController.ExcluirDetalheHB(Convert.ToInt32(hidColante.Value));
                if (cbForroExc.Checked && hidForro.Value != "")
                    prodController.ExcluirDetalheHB(Convert.ToInt32(hidForro.Value));
                if (cbGalaoExc.Checked && hidGalao.Value != "")
                    prodController.ExcluirDetalheHB(Convert.ToInt32(hidGalao.Value));
                if (cbGalaoProprioExc.Checked && hidGalaoProprio.Value != "")
                    prodController.ExcluirDetalheHB(Convert.ToInt32(hidGalaoProprio.Value));

                //FIM ATUALIZAÇÃO
                //************************************************************************************************************************

                labErroEnvio.Text = "<strong>OK...</strong>";

                labHBPopUp.Text = txtHB.Text;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, Risco: function () { window.open('prod_fila_risco.aspx', '_self'); }, Corte: function () { window.open('prod_fila_corte.aspx', '_self'); }, 'Rel. Aviamento': function () { window.open('prod_rel_aviamento.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErroEnvio.Text = "Erro (btEnviar_Click): " + ex.Message;
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                //Atualizar
                prodController.AtualizarStatusHB(Convert.ToInt32(hidHB.Value), 'E');

                //Mostrar POP-UP
                labHBPopUp.Text = txtHB.Text;
                labMensagem.Text = "EXCLUÍDO COM SUCESSO";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, Risco: function () { window.open('prod_fila_risco.aspx', '_self'); }, Corte: function () { window.open('prod_fila_corte.aspx', '_self'); }, 'Rel. Aviamento': function () { window.open('prod_rel_aviamento.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {

                labErroEnvio.Text = "Erro (btExcluir_Click): " + ex.Message;
            }

        }
        #endregion

        #region "OUTROS"
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

            //AO ALTERAR AQUI/ ALTERAR NA TELA DE CADASTRO
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

            labHB.ForeColor = _OK;
            if (txtHB.Text == "" || Convert.ToInt32(txtHB.Text) <= 0)
            {
                labHB.ForeColor = _notOK;
                retorno = false;
            }

            labCodigoLinx.ForeColor = _OK;
            if (txtCodigoLinx.Text.Trim() == "" || Convert.ToInt32(txtCodigoLinx.Text) <= 0)
            {
                labCodigoLinx.ForeColor = _notOK;
                retorno = false;
            }

            labGrupo.ForeColor = _OK;
            if (ddlGrupo.SelectedValue.Trim() == "" || ddlGrupo.SelectedValue == "Selecione")
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
            if (ddlGrupoTecido.SelectedValue.Trim() == "Selecione")
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
            if (txtPedidoNumero.Text == "" || Convert.ToInt32(txtPedidoNumero.Text) <= 0)
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
            if (ddlCorFornecedor.SelectedValue.Trim() == "" || ddlCorFornecedor.SelectedValue.Trim() == "Selecione")
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
            if (txtMolde.Text.Trim() == "")
            {
                labMolde.ForeColor = _notOK;
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

            labFotoTecido.ForeColor = _OK;
            if (imgFotoTecido.ImageUrl == "")
            {
                labFotoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labComposicaoTecido.ForeColor = _OK;
            if (gvComposicao.Rows.Count <= 0 || Convert.ToDecimal(hidTotalComp.Value) < 100)
            {
                labComposicaoTecido.ForeColor = _notOK;
                retorno = false;
            }

            labGrade.ForeColor = _OK;
            if (ValidarGrade() <= 0)
            {
                labGrade.ForeColor = _notOK;
                retorno = false;
            }

            labFotoPeca.ForeColor = _OK;
            if (imgFotoPeca.ImageUrl == "")
            {
                labFotoPeca.ForeColor = _notOK;
                retorno = false;
            }
            //COR2
            if (cbCor2Alt.Checked || cbCor2Inc.Checked)
            {
                labDetGrupoTecidoCor2.ForeColor = _OK;
                if (ddlDetalheGrupoTecidoCor2.SelectedValue.Trim() == "Selecione")
                {
                    labDetGrupoTecidoCor2.ForeColor = _notOK;
                    retorno = false;
                }

                labDetSubGrupoTecidoCor2.ForeColor = _OK;
                if (ddlDetalheSubGrupoTecidoCor2.SelectedValue.Trim() == "Selecione")
                {
                    labDetSubGrupoTecidoCor2.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorCor2.ForeColor = _OK;
                if (ddlDetalheCorCor2.SelectedValue.Trim() == "0")
                {
                    labDetCorCor2.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorFornecedorCor2.ForeColor = _OK;
                if (ddlDetalheCorFornecedorCor2.SelectedValue.Trim() == "Selecione")
                {
                    labDetCorFornecedorCor2.ForeColor = _notOK;
                    retorno = false;
                }

                labDetNumeroPedidoCor2.ForeColor = _OK;
                if (txtDetalheNumeroPedidoCor2.Text.Trim() == "")
                {
                    labDetNumeroPedidoCor2.ForeColor = _notOK;
                    retorno = false;
                }
                labDetFornecedorCor2.ForeColor = _OK;
                if (ddlDetalheFornecedorCor2.SelectedValue == "" || ddlDetalheFornecedorCor2.SelectedValue == "Selecione")
                {
                    labDetFornecedorCor2.ForeColor = _notOK;
                    retorno = false;
                }
                labDetMoldeCor2.ForeColor = _OK;
                if (txtDetalheMoldeCor2.Text.Trim() == "")
                {
                    labDetMoldeCor2.ForeColor = _notOK;
                    retorno = false;
                }
                labDetLarguraCor2.ForeColor = _OK;
                if (txtDetalheLarguraCor2.Text.Trim() == "" || Convert.ToDecimal(txtDetalheLarguraCor2.Text) <= 0)
                {
                    labDetLarguraCor2.ForeColor = _notOK;
                    retorno = false;
                }
                labDetCustoTecidoCor2.ForeColor = _OK;
                if (txtDetalheCustoTecidoCor2.Text.Trim() == "" || Convert.ToDecimal(txtDetalheCustoTecidoCor2.Text) <= 0)
                {
                    labDetCustoTecidoCor2.ForeColor = _notOK;
                    retorno = false;
                }

                labEtiquetaComposicaoCor2.ForeColor = _OK;
                if (ddlEtiquetaComposicaoCor2.SelectedValue == "")
                {
                    labEtiquetaComposicaoCor2.ForeColor = _notOK;
                    retorno = false;
                }

                //Controle da Composição
                labComposicaoDetalheCor2.ForeColor = _OK;
                if (gvComposicaoDetalheCor2.Rows.Count <= 0 || Convert.ToDecimal(hidDetalheCompTotalCor2.Value) < 100)
                {
                    labComposicaoDetalheCor2.ForeColor = _notOK;
                    retorno = false;
                }
            }
            //COR3
            if (cbCor3Alt.Checked || cbCor3Inc.Checked)
            {
                labDetGrupoTecidoCor3.ForeColor = _OK;
                if (ddlDetalheGrupoTecidoCor3.SelectedValue.Trim() == "Selecione")
                {
                    labDetGrupoTecidoCor3.ForeColor = _notOK;
                    retorno = false;
                }

                labDetSubGrupoTecidoCor3.ForeColor = _OK;
                if (ddlDetalheSubGrupoTecidoCor3.SelectedValue.Trim() == "Selecione")
                {
                    labDetSubGrupoTecidoCor3.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorCor3.ForeColor = _OK;
                if (ddlDetalheCorCor3.SelectedValue.Trim() == "0")
                {
                    labDetCorCor3.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorFornecedorCor3.ForeColor = _OK;
                if (ddlDetalheCorFornecedorCor3.SelectedValue.Trim() == "Selecione")
                {
                    labDetCorFornecedorCor3.ForeColor = _notOK;
                    retorno = false;
                }

                labDetNumeroPedidoCor3.ForeColor = _OK;
                if (txtDetalheNumeroPedidoCor3.Text.Trim() == "")
                {
                    labDetNumeroPedidoCor3.ForeColor = _notOK;
                    retorno = false;
                }
                labDetFornecedorCor3.ForeColor = _OK;
                if (ddlDetalheFornecedorCor3.SelectedValue == "" || ddlDetalheFornecedorCor3.SelectedValue == "Selecione")
                {
                    labDetFornecedorCor3.ForeColor = _notOK;
                    retorno = false;
                }
                labDetMoldeCor3.ForeColor = _OK;
                if (txtDetalheMoldeCor3.Text.Trim() == "")
                {
                    labDetMoldeCor3.ForeColor = _notOK;
                    retorno = false;
                }
                labDetLarguraCor3.ForeColor = _OK;
                if (txtDetalheLarguraCor3.Text.Trim() == "" || Convert.ToDecimal(txtDetalheLarguraCor3.Text) <= 0)
                {
                    labDetLarguraCor3.ForeColor = _notOK;
                    retorno = false;
                }
                labDetCustoTecidoCor3.ForeColor = _OK;
                if (txtDetalheCustoTecidoCor3.Text.Trim() == "" || Convert.ToDecimal(txtDetalheCustoTecidoCor3.Text) <= 0)
                {
                    labDetCustoTecidoCor3.ForeColor = _notOK;
                    retorno = false;
                }

                labEtiquetaComposicaoCor3.ForeColor = _OK;
                if (ddlEtiquetaComposicaoCor3.SelectedValue == "")
                {
                    labEtiquetaComposicaoCor3.ForeColor = _notOK;
                    retorno = false;
                }

                //Controle da Composição
                labComposicaoDetalheCor3.ForeColor = _OK;
                if (gvComposicaoDetalheCor3.Rows.Count <= 0 || Convert.ToDecimal(hidDetalheCompTotalCor3.Value) < 100)
                {
                    labComposicaoDetalheCor3.ForeColor = _notOK;
                    retorno = false;
                }
            }
            //FORRO
            if (cbForroAlt.Checked || cbForroInc.Checked)
            {
                labDetGrupoTecidoForro.ForeColor = _OK;
                if (ddlDetalheGrupoTecidoForro.SelectedValue.Trim() == "Selecione")
                {
                    labDetGrupoTecidoForro.ForeColor = _notOK;
                    retorno = false;
                }
                labDetSubGrupoTecidoForro.ForeColor = _OK;
                if (ddlDetalheSubGrupoTecidoForro.SelectedValue.Trim() == "Selecione")
                {
                    labDetSubGrupoTecidoForro.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorForro.ForeColor = _OK;
                if (ddlDetalheCorForro.SelectedValue.Trim() == "0")
                {
                    labDetCorForro.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorFornecedorForro.ForeColor = _OK;
                if (ddlDetalheCorFornecedorForro.SelectedValue.Trim() == "Selecione")
                {
                    labDetCorFornecedorForro.ForeColor = _notOK;
                    retorno = false;
                }

                labDetNumeroPedidoForro.ForeColor = _OK;
                if (txtDetalheNumeroPedidoForro.Text.Trim() == "")
                {
                    labDetNumeroPedidoForro.ForeColor = _notOK;
                    retorno = false;
                }
                labDetFornecedorForro.ForeColor = _OK;
                if (ddlDetalheFornecedorForro.SelectedValue == "" || ddlDetalheFornecedorForro.SelectedValue == "Selecione")
                {
                    labDetFornecedorForro.ForeColor = _notOK;
                    retorno = false;
                }
                labDetMoldeForro.ForeColor = _OK;
                if (txtDetalheMoldeForro.Text.Trim() == "")
                {
                    labDetMoldeForro.ForeColor = _notOK;
                    retorno = false;
                }
                labDetLarguraForro.ForeColor = _OK;
                if (txtDetalheLarguraForro.Text.Trim() == "" || Convert.ToDecimal(txtDetalheLarguraForro.Text) <= 0)
                {
                    labDetLarguraForro.ForeColor = _notOK;
                    retorno = false;
                }
                labDetCustoTecidoForro.ForeColor = _OK;
                if (txtDetalheCustoTecidoForro.Text.Trim() == "" || Convert.ToDecimal(txtDetalheCustoTecidoForro.Text) <= 0)
                {
                    labDetCustoTecidoForro.ForeColor = _notOK;
                    retorno = false;
                }
                labEtiquetaComposicaoForro.ForeColor = _OK;
                if (ddlEtiquetaComposicaoForro.SelectedValue == "")
                {
                    labEtiquetaComposicaoForro.ForeColor = _notOK;
                    retorno = false;
                }
                //Controle da Composição
                labComposicaoDetalheForro.ForeColor = _OK;
                if (gvComposicaoDetalheForro.Rows.Count <= 0 || Convert.ToDecimal(hidDetalheCompTotalForro.Value) < 100)
                {
                    labComposicaoDetalheForro.ForeColor = _notOK;
                    retorno = false;
                }
            }
            //COLANTE
            if (cbColanteAlt.Checked || cbColanteInc.Checked)
            {
                labDetGrupoTecidoColante.ForeColor = _OK;
                if (ddlDetalheGrupoTecidoColante.SelectedValue.Trim() == "Selecione")
                {
                    labDetGrupoTecidoColante.ForeColor = _notOK;
                    retorno = false;
                }

                labDetSubGrupoTecidoColante.ForeColor = _OK;
                if (ddlDetalheSubGrupoTecidoColante.SelectedValue.Trim() == "Selecione")
                {
                    labDetSubGrupoTecidoColante.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorColante.ForeColor = _OK;
                if (ddlDetalheCorColante.SelectedValue.Trim() == "0")
                {
                    labDetCorColante.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorFornecedorColante.ForeColor = _OK;
                if (ddlDetalheCorFornecedorColante.SelectedValue.Trim() == "Selecione")
                {
                    labDetCorFornecedorColante.ForeColor = _notOK;
                    retorno = false;
                }

                labDetNumeroPedidoColante.ForeColor = _OK;
                if (txtDetalheNumeroPedidoColante.Text.Trim() == "")
                {
                    labDetNumeroPedidoColante.ForeColor = _notOK;
                    retorno = false;
                }
                labDetFornecedorColante.ForeColor = _OK;
                if (ddlDetalheFornecedorColante.SelectedValue == "" || ddlDetalheFornecedorColante.SelectedValue == "Selecione")
                {
                    labDetFornecedorColante.ForeColor = _notOK;
                    retorno = false;
                }
                labDetMoldeColante.ForeColor = _OK;
                if (txtDetalheMoldeColante.Text.Trim() == "")
                {
                    labDetMoldeColante.ForeColor = _notOK;
                    retorno = false;
                }
                labDetLarguraColante.ForeColor = _OK;
                if (txtDetalheLarguraColante.Text.Trim() == "" || Convert.ToDecimal(txtDetalheLarguraColante.Text) <= 0)
                {
                    labDetLarguraColante.ForeColor = _notOK;
                    retorno = false;
                }
                labDetCustoTecidoColante.ForeColor = _OK;
                if (txtDetalheCustoTecidoColante.Text.Trim() == "" || Convert.ToDecimal(txtDetalheCustoTecidoColante.Text) <= 0)
                {
                    labDetCustoTecidoColante.ForeColor = _notOK;
                    retorno = false;
                }
                labEtiquetaComposicaoColante.ForeColor = _OK;
                if (ddlEtiquetaComposicaoColante.SelectedValue == "")
                {
                    labEtiquetaComposicaoColante.ForeColor = _notOK;
                    retorno = false;
                }
                //Controle da Composição
                labComposicaoDetalheColante.ForeColor = _OK;
                if (gvComposicaoDetalheColante.Rows.Count <= 0 || Convert.ToDecimal(hidDetalheCompTotalColante.Value) < 100)
                {
                    labComposicaoDetalheColante.ForeColor = _notOK;
                    retorno = false;
                }
            }
            //GALAO
            if (cbGalaoAlt.Checked || cbGalaoInc.Checked)
            {
                labDetGrupoTecidoGalao.ForeColor = _OK;
                if (ddlDetalheGrupoTecidoGalao.SelectedValue.Trim() == "Selecione")
                {
                    labDetGrupoTecidoGalao.ForeColor = _notOK;
                    retorno = false;
                }

                labDetSubGrupoTecidoGalao.ForeColor = _OK;
                if (ddlDetalheSubGrupoTecidoGalao.SelectedValue.Trim() == "Selecione")
                {
                    labDetSubGrupoTecidoGalao.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorGalao.ForeColor = _OK;
                if (ddlDetalheCorGalao.SelectedValue.Trim() == "0")
                {
                    labDetCorGalao.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorFornecedorGalao.ForeColor = _OK;
                if (ddlDetalheCorFornecedorGalao.SelectedValue.Trim() == "Selecione")
                {
                    labDetCorFornecedorGalao.ForeColor = _notOK;
                    retorno = false;
                }

                labDetNumeroPedidoGalao.ForeColor = _OK;
                if (txtDetalheNumeroPedidoGalao.Text.Trim() == "")
                {
                    labDetNumeroPedidoGalao.ForeColor = _notOK;
                    retorno = false;
                }
                labDetFornecedorGalao.ForeColor = _OK;
                if (ddlDetalheFornecedorGalao.SelectedValue == "" || ddlDetalheFornecedorGalao.SelectedValue == "Selecione")
                {
                    labDetFornecedorGalao.ForeColor = _notOK;
                    retorno = false;
                }
                labDetMoldeGalao.ForeColor = _OK;
                if (txtDetalheMoldeGalao.Text.Trim() == "")
                {
                    labDetMoldeGalao.ForeColor = _notOK;
                    retorno = false;
                }
                labDetLarguraGalao.ForeColor = _OK;
                if (txtDetalheLarguraGalao.Text.Trim() == "" || Convert.ToDecimal(txtDetalheLarguraGalao.Text) <= 0)
                {
                    labDetLarguraGalao.ForeColor = _notOK;
                    retorno = false;
                }
                labDetCustoTecidoGalao.ForeColor = _OK;
                if (txtDetalheCustoTecidoGalao.Text.Trim() == "" || Convert.ToDecimal(txtDetalheCustoTecidoGalao.Text) <= 0)
                {
                    labDetCustoTecidoGalao.ForeColor = _notOK;
                    retorno = false;
                }
                labEtiquetaComposicaoGalao.ForeColor = _OK;
                if (ddlEtiquetaComposicaoGalao.SelectedValue == "")
                {
                    labEtiquetaComposicaoGalao.ForeColor = _notOK;
                    retorno = false;
                }
                //Controle da Composição
                labComposicaoDetalheGalao.ForeColor = _OK;
                if (gvComposicaoDetalheGalao.Rows.Count <= 0 || Convert.ToDecimal(hidDetalheCompTotalGalao.Value) < 100)
                {
                    labComposicaoDetalheGalao.ForeColor = _notOK;
                    retorno = false;
                }
            }
            //GALAO PROPRIO
            /*
            if (cbGalaoProprioAlt.Checked || cbGalaoProprioInc.Checked)
            {
                labDetGrupoTecidoGalaoProprio.ForeColor = _OK;
                if (ddlDetalheGrupoTecidoGalaoProprio.SelectedValue.Trim() == "Selecione")
                {
                    labDetGrupoTecidoGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
             
                labDetSubGrupoTecidoGalaoProprio.ForeColor = _OK;
                if (ddlDetalheSubGrupoTecidoGalaoProprio.SelectedValue.Trim() == "Selecione")
                {
                    labDetSubGrupoTecidoGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                } 

                labDetCorGalaoProprio.ForeColor = _OK;
                if (ddlDetalheCorGalaoProprio.SelectedValue.Trim() == "0")
                {
                    labDetCorGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }

                labDetCorFornecedorGalaoProprio.ForeColor = _OK;
                if (ddlDetalheCorFornecedorGalaoProprio.SelectedValue.Trim() == "Selecione")
                {
                    labDetCorFornecedorGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
             
                labDetNumeroPedidoGalaoProprio.ForeColor = _OK;
                if (txtDetalheNumeroPedidoGalaoProprio.Text.Trim() == "")
                {
                    labDetNumeroPedidoGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
                labDetFornecedorGalaoProprio.ForeColor = _OK;
                if (txtDetalheFornecedorGalaoProprio.Text == "")
                {
                    labDetFornecedorGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
                labDetMoldeGalaoProprio.ForeColor = _notOK;
                if (txtDetalheMoldeGalaoProprio.Text.Trim() == "")
                {
                    labDetMoldeGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
                labDetLarguraGalaoProprio.ForeColor = _OK;
                if (txtDetalheLarguraGalaoProprio.Text.Trim() == "" || Convert.ToDecimal(txtDetalheLarguraGalaoProprio.Text) <= 0)
                {
                    labDetLarguraGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
                labDetCustoTecidoGalaoProprio.ForeColor = _OK;
                if (txtDetalheCustoTecidoGalaoProprio.Text.Trim() == "" || Convert.ToDecimal(txtDetalheCustoTecidoGalaoProprio.Text) <= 0)
                {
                    labDetCustoTecidoGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }

                //Controle da Composição
                labComposicaoDetalheGalaoProprio.ForeColor = _OK;
                if (gvComposicaoDetalheGalaoProprio.Rows.Count <= 0 || Convert.ToDecimal(hidDetalheCompTotalGalaoProprio.Value) < 100)
                {
                    labComposicaoDetalheGalaoProprio.ForeColor = _notOK;
                    retorno = false;
                }
            }*/

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
                    if (_hb.Where(p => p.MOSTRUARIO == 'S').Count() > 0)
                    {
                        msg = "O número do HB informado já existe nesta Coleção para MOSTRUÁRIO. Informe outro número.";
                        _label.Text = msg;
                        return msg;
                    }
                    if (_hb.Where(p => p.MOSTRUARIO == 'N').Count() > 0)
                    {
                        msg = "O número do HB informado já existe nesta Coleção para VENDA. Informe outro número.";
                        _label.Text = msg;
                        return msg;
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

        #region "RELATORIO"
        private void GerarRelatorio(RelatorioHB _relatorio)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "HB_" + _relatorio.HB + "_COL_" + _relatorio.COLECAO + ".html";
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
            _texto = MontarCabecalho(_texto);
            //Montar ORDEM DE CORTE
            _texto = MontarTecido(_texto, _relatorio);
            _texto = MontarComposicao(_texto, _relatorio);
            _texto = MontarGrade(_texto, _relatorio);
            _texto = MontarGradeCorte(_texto, _relatorio);
            _texto = MontarLinhasEmBranco(_texto);
            _texto = MontarDetalhes(_texto, _relatorio);
            _texto = MontarModelagem(_texto, _relatorio);
            _texto = MontarPeca(_texto, _relatorio);
            _texto = MontarOBS(_texto, _relatorio);
            //Montar DETALHES
            if (_relatorio.DETALHE.Count > 0)
                _texto = MontarDetalhe(_texto, _relatorio);

            //Rodape
            _texto = MontarRodape(_texto);
            return _texto;
        }

        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Ordem de Corte</title>   ");
            _texto.Append("         <meta charset='UTF-8'>          ");
            _texto.Append("         <style type='text/css'>");
            _texto.Append("             .break");
            _texto.Append("             {");
            _texto.Append("                 page-break-after: always;");
            _texto.Append("             }");
            _texto.Append("         </style>");
            _texto.Append("     </head>");
            _texto.Append("");
            _texto.Append("<body onLoad='window.print();'>");
            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("</body></html>");
            return _texto;
        }

        private StringBuilder MontarTecido(StringBuilder _texto, RelatorioHB _relatorio)
        {
            if (_relatorio.DETALHE.Count > 0)
                _texto.Append("<div id='fichaCorte' align='center' class='break'>");
            else
                _texto.Append("<div id='fichaCorte' align='center'>");
            _texto.Append("<table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt; padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white; white-space: nowrap;'>");
            _texto.Append("<tr>");
            _texto.Append("     <td style='line-height: 16px;'>");
            _texto.Append("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("     width: 517pt'>");
            _texto.Append("         <tr style='border-right: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.Append("             <td style='width: 70px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.Append("                 HB");
            _texto.Append("             </td>");
            _texto.Append("             <td style='width: 150px; border-right: 1px solid #000;'>");
            _texto.Append("                 GRUPO");
            _texto.Append("             </td>");
            _texto.Append("             <td style='width: 202px; border-right: 1px solid #000;'>");
            _texto.Append("                 NOME");
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000;'>");
            _texto.Append("                 DATA");
            _texto.Append("             </td>");
            _texto.Append("             <td>");
            _texto.Append("                 MOLDE");
            _texto.Append("             </td>");
            _texto.Append("             </tr>");
            _texto.Append("         <tr style='border: 1px solid #000; border-top: none;'>");
            _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                 " + _relatorio.HB);
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                 " + _relatorio.GRUPO);
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                 " + _relatorio.NOME);
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                 " + _relatorio.DATA);
            _texto.Append("             </td>");
            _texto.Append("             <td style='text-align: center'>");
            _texto.Append("                 &nbsp;");
            _texto.Append("             </td>");
            _texto.Append("         </tr>");
            _texto.Append("     </table>");
            _texto.Append(" </td>");
            _texto.Append("</tr>");
            _texto.Append("<tr>");
            _texto.Append(" <td style='line-height: 18px;'>");
            _texto.Append("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("     width: 517pt'>");
            _texto.Append("         <tr>");
            _texto.Append("             <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.Append("                 TECIDO");
            _texto.Append("             </td>");
            _texto.Append("             <td style='width: 100px; border-right: 1px solid #000;'>");
            _texto.Append("                 COR");
            _texto.Append("             </td>");
            _texto.Append("             <td style='width: 253px; border-right: 1px solid #000;'>");
            _texto.Append("                 FORNECEDOR");
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000;'>");
            _texto.Append("                 LARGURA");
            _texto.Append("             </td>");
            _texto.Append("         </tr>");
            _texto.Append("         <tr style='border-right: 1px solid #000;'>");
            _texto.Append("             <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: center;'>");
            _texto.Append("                 " + _relatorio.TECIDO);
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                 " + _relatorio.DESC_COR);
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                 " + _relatorio.FORNECEDOR);
            _texto.Append("             </td>");
            _texto.Append("             <td style='text-align: center'>");
            _texto.Append("                 " + _relatorio.LARGURA);
            _texto.Append("             </td>");
            _texto.Append("         </tr>");
            _texto.Append("     </table>");
            _texto.Append(" </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        private StringBuilder MontarComposicao(StringBuilder _texto, RelatorioHB _relatorio)
        {
            _texto.Append("<tr>");
            _texto.Append("     <td style='line-height: 16px;'>");
            _texto.Append("         <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("         width: 517pt'>");
            _texto.Append("             <tr style='border-top: 2px solid #000;'>");
            _texto.Append("                 <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                     COMPOSIÇÃO");
            _texto.Append("                 </td>");
            _texto.Append("             </tr>");
            foreach (PROD_HB_COMPOSICAO comp in _relatorio.COMPOSICAO)
            {
                _texto.Append("             <tr style='border: 1px solid #000'>");
                _texto.Append("                 <td style='text-align: center;'>");
                _texto.Append("                     " + comp.QTDE.ToString() + "% " + comp.DESCRICAO);
                _texto.Append("                 </td>");
                _texto.Append("             </tr>");
            }
            _texto.Append("         </table>");
            _texto.Append("     </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        private StringBuilder MontarGrade(StringBuilder _texto, RelatorioHB _relatorio)
        {
            _texto.Append("<tr>");
            _texto.Append("    <td style='line-height: 23px;'>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("            width: 517pt'>");
            _texto.Append("            <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.Append("                <td style='width: 130px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.Append("                    EXP");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.Append("                    XP");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.Append("                    PP");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.Append("                    P");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.Append("                    M");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.Append("                    G");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.Append("                    GG");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    TOTAL");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.Append("                <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.Append("                    GRADE PREVISTA");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.GRADE.GRADE_EXP);
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.GRADE.GRADE_XP);
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.GRADE.GRADE_PP);
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.GRADE.GRADE_P);
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.GRADE.GRADE_M);
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.GRADE.GRADE_G);
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.GRADE.GRADE_GG);
            _texto.Append("                </td>");
            _texto.Append("                <td>");
            _texto.Append("                    " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr style='border-bottom: 1px solid #000;'>");
            _texto.Append("                <td style='text-align: left; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                    GRADE REAL");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        private StringBuilder MontarGradeCorte(StringBuilder _texto, RelatorioHB _relatorio)
        {
            _texto.Append("<tr>");
            _texto.Append("    <td style='line-height: 23px;'>");
            _texto.Append("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("            width: 517pt'>");
            _texto.Append("            <tr style='line-height: 12px;'>");
            _texto.Append("                <td style='border-left: 1px solid #000; border: 1px solid #000;'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr>");
            _texto.Append("                <td>");
            _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
            _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[0].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 1) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[1].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[2].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td rowspan='13' style='text-align: center; width: 82px;'>");
            _texto.Append("                                 <img alt='Foto Tecido' src='..\\.." + _relatorio.FOTO_TECIDO.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                1ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 0) ? (_relatorio.GRADE_CORTE[0].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                2ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 1) ? (_relatorio.GRADE_CORTE[1].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                3ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 2) ? (_relatorio.GRADE_CORTE[2].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 0) ? Convert.ToInt32(_relatorio.GRADE_CORTE[0].FOLHA).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 1) ? Convert.ToInt32(_relatorio.GRADE_CORTE[1].FOLHA).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 2) ? Convert.ToInt32(_relatorio.GRADE_CORTE[2].FOLHA).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none; border-bottom: 2px solid #000;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='line-height: 12px;'>");
            _texto.Append("                            <td colspan='6' style='border-bottom: 1px solid #000;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.Append("                                GASTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 2px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 3) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[3].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 4) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[4].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 5) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[5].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                4ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 3) ? (_relatorio.GRADE_CORTE[3].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                5ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 4) ? (_relatorio.GRADE_CORTE[4].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                6ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 5) ? (_relatorio.GRADE_CORTE[5].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 3) ? Convert.ToInt32((_relatorio.GRADE_CORTE[3].FOLHA)).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 4) ? Convert.ToInt32((_relatorio.GRADE_CORTE[4].FOLHA)).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 5) ? Convert.ToInt32((_relatorio.GRADE_CORTE[5].FOLHA)).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none; border-bottom: 1px solid #000;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        private StringBuilder MontarLinhasEmBranco(StringBuilder _texto)
        {
            _texto.Append("<tr>");
            _texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
            _texto.Append("        &nbsp;");
            _texto.Append("    </td>");
            _texto.Append("</tr>");
            _texto.Append("<tr>");
            _texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
            _texto.Append("        &nbsp;");
            _texto.Append("    </td>");
            _texto.Append("</tr>");
            _texto.Append("<tr>");
            _texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
            _texto.Append("        &nbsp;");
            _texto.Append("    </td>");
            _texto.Append("</tr>");
            _texto.Append("<tr>");
            _texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
            _texto.Append("        &nbsp;");
            _texto.Append("    </td>");
            _texto.Append("</tr>");
            _texto.Append("<tr>");
            _texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
            _texto.Append("        &nbsp;");
            _texto.Append("    </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        private StringBuilder MontarDetalhes(StringBuilder _texto, RelatorioHB _relatorio)
        {
            _texto.Append("<tr>");
            _texto.Append("     <td style='line-height: 18px;'>");
            _texto.Append("         <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("         width: 517pt'>");
            _texto.Append("             <tr style='border-top: 1px solid #000;'>");
            _texto.Append("                 <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                     DETALHES");
            _texto.Append("                 </td>");
            _texto.Append("             </tr>");
            foreach (PROD_HB hb in _relatorio.DETALHE)
            {
                _texto.Append("             <tr style='border: 1px solid #000'>");
                _texto.Append("                 <td style='text-align: center;'>");
                _texto.Append("                     " + hb.PROD_DETALHE1.DESCRICAO);
                _texto.Append("                 </td>");
                _texto.Append("             </tr>");
            }
            _texto.Append("             <tr style='border: 1px solid #000'>");
            _texto.Append("                 <td style='text-align: center;'>");
            _texto.Append("                     &nbsp;");
            _texto.Append("                 </td>");
            _texto.Append("             </tr>");
            _texto.Append("         </table>");
            _texto.Append("     </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        private StringBuilder MontarModelagem(StringBuilder _texto, RelatorioHB _relatorio)
        {
            _texto.Append("<tr>");
            _texto.Append("    <td style='line-height: 17px;'>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("            width: 517pt'>");
            _texto.Append("            <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.Append("                <td style='width: 200px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.Append("                    MODELAGEM");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 130px; border-right: 1px solid #000;'>");
            _texto.Append("                    TOTAL DE PARTES");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 50px; border-right: 1px solid #000;'>");
            _texto.Append("                    RISCO");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 130px; border-right: 1px solid #000;'>");
            _texto.Append("                    CONFERIDO");
            _texto.Append("                </td>");
            _texto.Append("                <td>");
            _texto.Append("                    LIQUIDAR");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr style='border: 1px solid #000; border-top: none;'>");
            _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                    " + _relatorio.MODELAGEM);
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("                <td style='text-align: center'>");
            _texto.Append("                    " + _relatorio.LIQUIDAR);
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </td>");
            _texto.Append("</tr>");


            return _texto;
        }
        private StringBuilder MontarPeca(StringBuilder _texto, RelatorioHB _relatorio)
        {
            _texto.Append("<tr>");
            _texto.Append("    <td style='line-height: 17px;'>");
            _texto.Append("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("            width: 517pt'>");
            _texto.Append("            <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.Append("                <td style='width: 252px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.Append("                    <table cellpadding='0' cellspacing='0' width='100%'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                CONSUMO PARA CUSTO");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='border-bottom: 1px solid #000;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                TECIDO PRINCIPAL $");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='border-bottom: 1px solid #000;'>");
            _texto.Append("                                " + _relatorio.CUSTO_TECIDO);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td>");
            _texto.Append("                                <table cellpadding='0' cellspacing='0' width='100%'>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                            POR PEÇA");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; text-align: center;'>");
            _texto.Append("                                            POR CORTE");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            KILOS");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            METROS");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            RETALHOS");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
            _texto.Append("                                </table>");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("                <td style='text-align: center; width: 200px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.Append("                     <img alt='Foto Peça' src='..\\.." + _relatorio.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        private StringBuilder MontarOBS(StringBuilder _texto, RelatorioHB _relatorio)
        {
            _texto.Append("<tr>");
            _texto.Append("     <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.Append("         OBSERVAÇÃO");
            _texto.Append("     </td>");
            _texto.Append("</tr>");
            _texto.Append("<tr style='line-height: 25px; vertical-align: top;' valign='top'>");
            _texto.Append("     <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.Append("                    " + _relatorio.OBS.Replace("\r", "<br/>").Replace("\n", "<br/>"));
            _texto.Append("     </td>");
            _texto.Append("</tr>");
            _texto.Append("</table></div>");
            return _texto;
        }
        private StringBuilder MontarDetalhe(StringBuilder _texto, RelatorioHB _relatorio)
        {
            int count = 0;
            foreach (PROD_HB d in _relatorio.DETALHE)
            {
                count += 1;
                _texto.Append("    <br />");
                if (count != _relatorio.DETALHE.Count())
                    _texto.Append("    <div id='divDetalhe" + d.PROD_DETALHE.ToString() + "' align='center' class='break'>");
                else
                    _texto.Append("    <div id='divDetalhe" + d.PROD_DETALHE1.DESCRICAO + "' align='center'>");

                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
                _texto.Append("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='line-height: 16px;'>");
                _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
                _texto.Append("                        width: 517pt'>");
                _texto.Append("                        <tr style='border-right: 1px solid #000; border-top: 2px solid #000;'>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
                _texto.Append("                                HB");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 150px; border-right: 1px solid #000;'>");
                _texto.Append("                                GRUPO");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 202px; border-right: 1px solid #000;'>");
                _texto.Append("                                NOME");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                DATA");
                _texto.Append("                            </td>");
                _texto.Append("                            <td>");
                _texto.Append("                                MOLDE");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border: 1px solid #000; border-top: none;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center'>");
                _texto.Append("                                " + d.HB);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center'>");
                _texto.Append("                                " + d.GRUPO);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center'>");
                _texto.Append("                                " + d.NOME);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center'>");
                _texto.Append("                                " + d.DATA_INCLUSAO);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align: center'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                    </table>");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='line-height: 18px;'>");
                _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
                _texto.Append("                        width: 517pt'>");
                _texto.Append("                        <tr style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
                _texto.Append("                            <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
                _texto.Append("                                DETALHE");
                _texto.Append("                            </td>");
                _texto.Append("                            <td>");
                _texto.Append("                                COMPOSIÇÃO");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border: 1px solid #000; border-top: none;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center'>");
                _texto.Append("                                <i><b>" + d.PROD_DETALHE1.DESCRICAO.ToUpper() + "</b></i>");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align:center;'>");
                _texto.Append("                                <table width='100%'>");
                foreach (PROD_HB_COMPOSICAO comp in prodController.ObterComposicaoHB(d.CODIGO))
                {
                    _texto.Append("                                    <tr>");
                    _texto.Append("                                        <td>");
                    _texto.Append("                                         " + comp.QTDE.ToString() + "% " + comp.DESCRICAO);
                    _texto.Append("                                         </td>");
                    _texto.Append("                                    </tr>");
                }
                _texto.Append("                                </table>");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                    </table>");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='line-height: 18px;'>");
                _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
                _texto.Append("                        width: 517pt'>");
                _texto.Append("                        <tr>");
                _texto.Append("                            <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
                _texto.Append("                                TECIDO");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 100px; border-right: 1px solid #000;'>");
                _texto.Append("                                T. PARTES");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 253px; border-right: 1px solid #000;'>");
                _texto.Append("                                FORNECEDOR");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                LARGURA");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border-right: 1px solid #000;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + d.TECIDO);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + d.FORNECEDOR.Trim());
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='text-align: center'>");
                _texto.Append("                                " + d.LARGURA);
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                    </table>");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='line-height: 23px;'>");
                _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
                _texto.Append("                        width: 517pt'>");
                _texto.Append("                        <tr style='text-align: center; border-top: 2px solid #000;'>");
                _texto.Append("                            <td style='width: 130px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000;'>");
                _texto.Append("                                XP");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000;'>");
                _texto.Append("                                EXP");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000;'>");
                _texto.Append("                                PP");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000;'>");
                _texto.Append("                                P");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000;'>");
                _texto.Append("                                M");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000;'>");
                _texto.Append("                                G");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='width: 70px; border-right: 1px solid #000;'>");
                _texto.Append("                                GG");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                TOTAL");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.Append("                            <td style='text-align: left; border-right: 1px solid #000;'>");
                _texto.Append("                                GRADE PREVISTA");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + _relatorio.GRADE.GRADE_EXP);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + _relatorio.GRADE.GRADE_XP);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + _relatorio.GRADE.GRADE_PP);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + _relatorio.GRADE.GRADE_P);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + _relatorio.GRADE.GRADE_M);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + _relatorio.GRADE.GRADE_G);
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + _relatorio.GRADE.GRADE_GG);
                _texto.Append("                            </td>");
                _texto.Append("                            <td>");
                _texto.Append("                                " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border-bottom: 1px solid #000;'>");
                _texto.Append("                            <td style='text-align: left; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                _texto.Append("                                GRADE REAL");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                    </table>");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='line-height: 23px;'>");
                _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
                _texto.Append("                        width: 517pt'>");
                _texto.Append("                        <tr style='line-height: 12px;'>");
                _texto.Append("                            <td style='border-left: 1px solid #000; border: 1px solid #000;'>");
                _texto.Append("                                &nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr>");
                _texto.Append("                            <td>");
                _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
                _texto.Append("                                    <tr style='border-top: 1px solid #000;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            RISCO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            RISCO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            RISCO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            RISCO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td rowspan='9' style='text-align: center; width: 82px;'>");
                _texto.Append("                                            <img alt='Foto Tecido' src='..\\.." + d.FOTO_TECIDO.Replace("~", "").Replace("/", "\\") + "' />");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='border: 0px solid #000; border-left: none;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-bottom:none;'>");
                _texto.Append("                                            GASTO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-bottom:0px solid #ccc;'>");
                _texto.Append("                                            GASTO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            GASTO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            GASTO");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='line-height: 12px;'>");
                _texto.Append("                                        <td colspan='6' style='border-bottom: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                </table>");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                    </table>");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='border: 1px solid #000; border-top: none;'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='border: 1px solid #000; border-top: none;'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='border: 1px solid #000; border-top: none;'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='line-height: 17px;'>");
                _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
                _texto.Append("                        width: 517pt'>");
                _texto.Append("                        <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
                _texto.Append("                                <table cellpadding='0' cellspacing='0' width='100%'>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px;'>");
                _texto.Append("                                            CONSUMO PARA CUSTO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
                _texto.Append("                                            POR PEÇA");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
                _texto.Append("                                            POR CORTE");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
                _texto.Append("                                            R$");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px; text-align:center;'>");
                _texto.Append("                                            " + ((d.CUSTO_TECIDO != null) ? Convert.ToDecimal(d.CUSTO_TECIDO).ToString("###0.00") : "&nbsp;"));
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
                _texto.Append("                                            KILO");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
                _texto.Append("                                            METROS");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
                _texto.Append("                                            RETALHOS");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                    <tr style='' valign='top'>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
                _texto.Append("                                    </tr>");
                _texto.Append("                                </table>");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                    </table>");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr>");
                _texto.Append("                <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
                _texto.Append("                    OBSERVAÇÃO");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr style='line-height: 25px; vertical-align: top;' valign='top'>");
                _texto.Append("                <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
                _texto.Append("                    " + d.OBSERVACAO.Replace("\r", "<br/>").Replace("\n", "<br/>"));
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("        </table>");
                _texto.Append("    </div>");
            }

            return _texto;
        }

        #endregion

        #region "DETALHE"
        private int IncluirDetalheHB(PROD_HB _detalhe)
        {
            return prodController.InserirDetalheHB(_detalhe);
        }
        private void IncluirGrade(PROD_HB_GRADE _grade)
        {
            prodController.InserirGrade(_grade);
        }
        private void IncluirDetalhe(
                int detalhe, DropDownList DetalheGrupoTecido, DropDownList DetalheSubGrupoTecido, TextBox DetalheNumeroPedido, DropDownList DetalheFornecedor,
                TextBox DetalheLargura, TextBox DetalheCustoTecido, System.Web.UI.WebControls.Image DetalheFoto,
                TextBox Observacao, GridView GridComposicao, string statusPrincipal, TextBox DetalheMolde, DropDownList DetalheCor, DropDownList DetalheCorFornecedor, TextBox DetalheGastoFolha, DropDownList DetalheEtiquetaComposicao)
        {
            int codigo;
            PROD_HB _novoDetalhe = new PROD_HB();

            _novoDetalhe.CODIGO_PAI = Convert.ToInt32(hidHB.Value);
            _novoDetalhe.HB = Convert.ToInt32(txtHB.Text);
            _novoDetalhe.PROD_DETALHE = detalhe;
            _novoDetalhe.COLECAO = ddlColecoes.SelectedValue;
            _novoDetalhe.COR = DetalheCor.SelectedValue;
            _novoDetalhe.COR_FORNECEDOR = DetalheCorFornecedor.SelectedValue.Trim();
            _novoDetalhe.GRUPO = ddlGrupo.SelectedValue;
            _novoDetalhe.NOME = txtNome.Text.Trim().ToUpper();
            _novoDetalhe.DATA_INCLUSAO = DateTime.Now;
            _novoDetalhe.GRUPO_TECIDO = DetalheGrupoTecido.SelectedValue.ToUpper();
            _novoDetalhe.TECIDO = DetalheSubGrupoTecido.SelectedValue.Trim().ToUpper();
            _novoDetalhe.FORNECEDOR = DetalheFornecedor.SelectedValue;
            _novoDetalhe.LARGURA = Convert.ToDecimal(DetalheLargura.Text);
            _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(DetalheCustoTecido.Text);
            _novoDetalhe.LIQUIDAR = (cbLiquidar.Checked) ? 'S' : 'N';
            _novoDetalhe.ATACADO = (cbAtacado.Checked) ? 'S' : 'N';
            _novoDetalhe.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
            _novoDetalhe.FOTO_TECIDO = DetalheFoto.ImageUrl;
            _novoDetalhe.TIPO = 'R';
            _novoDetalhe.OBSERVACAO = Observacao.Text.Trim().ToUpper();
            _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(DetalheNumeroPedido.Text);
            _novoDetalhe.STATUS = 'P';
            _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(DetalheEtiquetaComposicao.SelectedValue);
            _novoDetalhe.MOSTRUARIO = (cbMostruario.Checked) ? 'S' : 'N';
            if (DetalheMolde.Text.Trim() != "")
                _novoDetalhe.MOLDE = DetalheMolde.Text.Trim().ToUpper();
            if (DetalheGastoFolha.Text.Trim() != "")
                _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(DetalheGastoFolha.Text.Trim());

            //validar peso do tecido para calcular estoque
            var rendimento = prodController.ObterRendimentoTecidoKG(_novoDetalhe.NUMERO_PEDIDO.ToString());
            if (rendimento != null)
                _novoDetalhe.RENDIMENTO_1MT = rendimento.RENDIMENTO_1MT;

            codigo = IncluirDetalheHB(_novoDetalhe);

            //CONTROLE DE GRADE
            //Inserir grade
            var gradeExiste = prodController.ObterGradeHB(codigo, 2);
            if (gradeExiste == null)
            {
                var grade = new PROD_HB_GRADE();
                grade.PROD_HB = codigo;
                grade.PROD_PROCESSO = 2; //PROCESSO DO DETALHE SEMPRE INICIA NO RISCO-NAO TEM AMPLIAÇÃO
                grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                grade.DATA_INCLUSAO = DateTime.Now;
                grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                IncluirGrade(grade);
            }

            PROD_HB_COMPOSICAO _novo = null;
            foreach (GridViewRow r in GridComposicao.Rows)
            {
                _novo = new PROD_HB_COMPOSICAO();
                _novo.PROD_HB = codigo;
                _novo.QTDE = Convert.ToDecimal(r.Cells[1].Text);
                _novo.DESCRICAO = r.Cells[2].Text.Trim().ToUpper();
                IncluirComposicao(_novo);
            }

            //CONTROLE DE PROCESSO
            //if (statusPrincipal != "B")
            //{
            //    if (_novoDetalhe.MOSTRUARIO == 'S' || (_novoDetalhe.MOSTRUARIO == 'N' && (detalhe == 4 || detalhe == 5)))
            //    {
            //        PROD_HB_PROD_PROCESSO _processo;
            //        PROD_HB _detalhe = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value), detalhe);
            //        if (_detalhe != null)
            //        {
            //            _processo = new PROD_HB_PROD_PROCESSO();
            //            _processo.PROD_HB = _detalhe.CODIGO;
            //            _processo.PROD_PROCESSO = ObterProcessoOrdem(2);
            //            _processo.DATA_INICIO = DateTime.Now;
            //            IncluirProcesso(_processo);
            //        }
            //    }
            //}

            var detalheInclusao = prodController.ObterDetalhesHB(Convert.ToInt32(hidHB.Value), detalhe);
            if (detalheInclusao != null)
            {
                int proc = 2;
                if (detalhe == 4 || detalhe == 5)
                    proc = 3;

                var processoExiste = prodController.ObterProcessoHB(detalheInclusao.CODIGO, ObterProcessoOrdem(proc));
                if (processoExiste == null)
                {
                    var processo = new PROD_HB_PROD_PROCESSO();
                    processo.PROD_HB = detalheInclusao.CODIGO;
                    processo.PROD_PROCESSO = ObterProcessoOrdem(proc);
                    processo.DATA_INICIO = DateTime.Now;
                    IncluirProcesso(processo);
                }
            }


        }
        private void AtualizarDetalhe(PROD_HB prod_hb)
        {
            try
            {
                PROD_HB _novoDetalhe = new PROD_HB();
                _novoDetalhe.CODIGO = prod_hb.CODIGO;
                _novoDetalhe.CODIGO_PAI = Convert.ToInt32(hidHB.Value);
                _novoDetalhe.PROD_DETALHE = prod_hb.PROD_DETALHE;
                _novoDetalhe.HB = Convert.ToInt32(txtHB.Text);
                _novoDetalhe.COLECAO = ddlColecoes.SelectedValue;
                _novoDetalhe.GRUPO = ddlGrupo.SelectedValue;
                _novoDetalhe.NOME = txtNome.Text.Trim().ToUpper();
                _novoDetalhe.DATA_INCLUSAO = DateTime.Now;
                _novoDetalhe.TIPO = 'R';
                _novoDetalhe.LIQUIDAR = (cbLiquidar.Checked) ? 'S' : 'N';
                _novoDetalhe.ATACADO = (cbAtacado.Checked) ? 'S' : 'N';
                _novoDetalhe.MODELAGEM = txtModelagem.Text.Trim().ToUpper();
                _novoDetalhe.STATUS = prod_hb.STATUS;
                _novoDetalhe.MOSTRUARIO = (cbMostruario.Checked) ? 'S' : 'N';

                if (prod_hb.PROD_DETALHE == 1) //COR 2
                {
                    if (cbCor2Alt.Checked)
                    {
                        _novoDetalhe.GRUPO_TECIDO = ddlDetalheGrupoTecidoCor2.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.TECIDO = ddlDetalheSubGrupoTecidoCor2.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.COR = ddlDetalheCorCor2.SelectedValue;
                        _novoDetalhe.COR_FORNECEDOR = ddlDetalheCorFornecedorCor2.SelectedValue;
                        _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(txtDetalheNumeroPedidoCor2.Text.Trim());
                        _novoDetalhe.FORNECEDOR = ddlDetalheFornecedorCor2.SelectedValue;
                        _novoDetalhe.LARGURA = Convert.ToDecimal(txtDetalheLarguraCor2.Text);
                        _novoDetalhe.MOLDE = txtDetalheMoldeCor2.Text.Trim().ToUpper();
                        _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(txtDetalheCustoTecidoCor2.Text);
                        if (txtDetalheGastoPorFolhaCor2.Text.Trim() != "")
                            _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtDetalheGastoPorFolhaCor2.Text);

                        if (ddlEtiquetaComposicaoCor2.SelectedValue.Trim() != "")
                            _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicaoCor2.SelectedValue);

                        _novoDetalhe.FOTO_TECIDO = imgFotoTecidoDetalheCor2.ImageUrl;
                        _novoDetalhe.OBSERVACAO = txtDetalheObservacaoCor2.Text.Trim().ToUpper();
                    }
                    else
                    {
                        _novoDetalhe.GRUPO_TECIDO = prod_hb.GRUPO_TECIDO;
                        _novoDetalhe.TECIDO = prod_hb.TECIDO;
                        _novoDetalhe.COR = prod_hb.COR;
                        _novoDetalhe.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                        _novoDetalhe.NUMERO_PEDIDO = prod_hb.NUMERO_PEDIDO;
                        _novoDetalhe.FORNECEDOR = prod_hb.FORNECEDOR;
                        _novoDetalhe.LARGURA = prod_hb.LARGURA;
                        _novoDetalhe.MOLDE = prod_hb.MOLDE;
                        _novoDetalhe.CUSTO_TECIDO = prod_hb.CUSTO_TECIDO;
                        _novoDetalhe.GASTO_FOLHA_SIMULACAO = prod_hb.GASTO_FOLHA_SIMULACAO;
                        _novoDetalhe.FOTO_TECIDO = prod_hb.FOTO_TECIDO;
                        _novoDetalhe.OBSERVACAO = prod_hb.OBSERVACAO;
                        _novoDetalhe.ETIQUETA_COMPOSICAO = prod_hb.ETIQUETA_COMPOSICAO;
                    }
                }

                if (prod_hb.PROD_DETALHE == 6) //COR 3
                {
                    if (cbCor3Alt.Checked)
                    {
                        _novoDetalhe.GRUPO_TECIDO = ddlDetalheGrupoTecidoCor3.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.TECIDO = ddlDetalheSubGrupoTecidoCor3.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.COR = ddlDetalheCorCor3.SelectedValue;
                        _novoDetalhe.COR_FORNECEDOR = ddlDetalheCorFornecedorCor3.SelectedValue;
                        _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(txtDetalheNumeroPedidoCor3.Text.Trim());
                        _novoDetalhe.FORNECEDOR = ddlDetalheFornecedorCor3.SelectedValue;
                        _novoDetalhe.LARGURA = Convert.ToDecimal(txtDetalheLarguraCor3.Text);
                        _novoDetalhe.MOLDE = txtDetalheMoldeCor3.Text.Trim().ToUpper();
                        _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(txtDetalheCustoTecidoCor3.Text);
                        if (txtDetalheGastoPorFolhaCor3.Text.Trim() != "")
                            _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtDetalheGastoPorFolhaCor3.Text);
                        if (ddlEtiquetaComposicaoCor3.SelectedValue.Trim() != "")
                            _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicaoCor3.SelectedValue);
                        _novoDetalhe.FOTO_TECIDO = imgFotoTecidoDetalheCor3.ImageUrl;
                        _novoDetalhe.OBSERVACAO = txtDetalheObservacaoCor3.Text.Trim().ToUpper();
                    }
                    else
                    {
                        _novoDetalhe.GRUPO_TECIDO = prod_hb.GRUPO_TECIDO;
                        _novoDetalhe.TECIDO = prod_hb.TECIDO;
                        _novoDetalhe.COR = prod_hb.COR;
                        _novoDetalhe.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                        _novoDetalhe.NUMERO_PEDIDO = prod_hb.NUMERO_PEDIDO;
                        _novoDetalhe.FORNECEDOR = prod_hb.FORNECEDOR;
                        _novoDetalhe.LARGURA = prod_hb.LARGURA;
                        _novoDetalhe.MOLDE = prod_hb.MOLDE;
                        _novoDetalhe.CUSTO_TECIDO = prod_hb.CUSTO_TECIDO;
                        _novoDetalhe.GASTO_FOLHA_SIMULACAO = prod_hb.GASTO_FOLHA_SIMULACAO;
                        _novoDetalhe.FOTO_TECIDO = prod_hb.FOTO_TECIDO;
                        _novoDetalhe.OBSERVACAO = prod_hb.OBSERVACAO;
                        _novoDetalhe.ETIQUETA_COMPOSICAO = prod_hb.ETIQUETA_COMPOSICAO;
                    }
                }

                if (prod_hb.PROD_DETALHE == 2) //COLANTE
                {
                    if (cbColanteAlt.Checked)
                    {
                        _novoDetalhe.GRUPO_TECIDO = ddlDetalheGrupoTecidoColante.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.TECIDO = ddlDetalheSubGrupoTecidoColante.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.COR = ddlDetalheCorColante.SelectedValue;
                        _novoDetalhe.COR_FORNECEDOR = ddlDetalheCorFornecedorColante.SelectedValue;
                        _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(txtDetalheNumeroPedidoColante.Text.Trim());
                        _novoDetalhe.FORNECEDOR = ddlDetalheFornecedorColante.SelectedValue;
                        _novoDetalhe.LARGURA = Convert.ToDecimal(txtDetalheLarguraColante.Text);
                        _novoDetalhe.MOLDE = txtDetalheMoldeColante.Text.Trim().ToUpper();
                        _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(txtDetalheCustoTecidoColante.Text);
                        if (txtDetalheGastoPorFolhaColante.Text.Trim() != "")
                            _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtDetalheGastoPorFolhaColante.Text);
                        if (ddlEtiquetaComposicaoColante.SelectedValue.Trim() != "")
                            _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicaoColante.SelectedValue);
                        _novoDetalhe.FOTO_TECIDO = imgFotoTecidoDetalheColante.ImageUrl;
                        _novoDetalhe.OBSERVACAO = txtDetalheObservacaoColante.Text.Trim().ToUpper();
                    }
                    else
                    {
                        _novoDetalhe.GRUPO_TECIDO = prod_hb.GRUPO_TECIDO;
                        _novoDetalhe.TECIDO = prod_hb.TECIDO;
                        _novoDetalhe.COR = prod_hb.COR;
                        _novoDetalhe.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                        _novoDetalhe.NUMERO_PEDIDO = prod_hb.NUMERO_PEDIDO;
                        _novoDetalhe.FORNECEDOR = prod_hb.FORNECEDOR;
                        _novoDetalhe.LARGURA = prod_hb.LARGURA;
                        _novoDetalhe.MOLDE = prod_hb.MOLDE;
                        _novoDetalhe.CUSTO_TECIDO = prod_hb.CUSTO_TECIDO;
                        _novoDetalhe.GASTO_FOLHA_SIMULACAO = prod_hb.GASTO_FOLHA_SIMULACAO;
                        _novoDetalhe.FOTO_TECIDO = prod_hb.FOTO_TECIDO;
                        _novoDetalhe.OBSERVACAO = prod_hb.OBSERVACAO;
                        _novoDetalhe.ETIQUETA_COMPOSICAO = prod_hb.ETIQUETA_COMPOSICAO;
                    }
                }

                if (prod_hb.PROD_DETALHE == 3) //FORRO
                {
                    if (cbForroAlt.Checked)
                    {
                        _novoDetalhe.GRUPO_TECIDO = ddlDetalheGrupoTecidoForro.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.TECIDO = ddlDetalheSubGrupoTecidoForro.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.COR = ddlDetalheCorForro.SelectedValue;
                        _novoDetalhe.COR_FORNECEDOR = ddlDetalheCorFornecedorForro.SelectedValue;
                        _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(txtDetalheNumeroPedidoForro.Text.Trim());
                        _novoDetalhe.FORNECEDOR = ddlDetalheFornecedorForro.SelectedValue;
                        _novoDetalhe.LARGURA = Convert.ToDecimal(txtDetalheLarguraForro.Text);
                        _novoDetalhe.MOLDE = txtDetalheMoldeForro.Text.Trim().ToUpper();
                        _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(txtDetalheCustoTecidoForro.Text);
                        if (txtDetalheGastoPorFolhaForro.Text.Trim() != "")
                            _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtDetalheGastoPorFolhaForro.Text);
                        if (ddlEtiquetaComposicaoForro.SelectedValue.Trim() != "")
                            _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicaoForro.SelectedValue);
                        _novoDetalhe.FOTO_TECIDO = imgFotoTecidoDetalheForro.ImageUrl;
                        _novoDetalhe.OBSERVACAO = txtDetalheObservacaoForro.Text.Trim().ToUpper();
                    }
                    else
                    {
                        _novoDetalhe.GRUPO_TECIDO = prod_hb.GRUPO_TECIDO;
                        _novoDetalhe.TECIDO = prod_hb.TECIDO;
                        _novoDetalhe.COR = prod_hb.COR;
                        _novoDetalhe.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                        _novoDetalhe.NUMERO_PEDIDO = prod_hb.NUMERO_PEDIDO;
                        _novoDetalhe.FORNECEDOR = prod_hb.FORNECEDOR;
                        _novoDetalhe.LARGURA = prod_hb.LARGURA;
                        _novoDetalhe.MOLDE = prod_hb.MOLDE;
                        _novoDetalhe.CUSTO_TECIDO = prod_hb.CUSTO_TECIDO;
                        _novoDetalhe.GASTO_FOLHA_SIMULACAO = prod_hb.GASTO_FOLHA_SIMULACAO;
                        _novoDetalhe.FOTO_TECIDO = prod_hb.FOTO_TECIDO;
                        _novoDetalhe.OBSERVACAO = prod_hb.OBSERVACAO;
                        _novoDetalhe.ETIQUETA_COMPOSICAO = prod_hb.ETIQUETA_COMPOSICAO;
                    }
                }

                if (prod_hb.PROD_DETALHE == 4) //GALÃO
                {
                    if (cbGalaoAlt.Checked)
                    {
                        _novoDetalhe.GRUPO_TECIDO = ddlDetalheGrupoTecidoGalao.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.TECIDO = ddlDetalheSubGrupoTecidoGalao.SelectedValue.Trim().ToUpper();
                        _novoDetalhe.COR = ddlDetalheCorGalao.SelectedValue;
                        _novoDetalhe.COR_FORNECEDOR = ddlDetalheCorFornecedorGalao.SelectedValue;
                        _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(txtDetalheNumeroPedidoGalao.Text.Trim());
                        _novoDetalhe.FORNECEDOR = ddlDetalheFornecedorGalao.SelectedValue;
                        _novoDetalhe.LARGURA = Convert.ToDecimal(txtDetalheLarguraGalao.Text);
                        _novoDetalhe.MOLDE = txtDetalheMoldeGalao.Text.Trim().ToUpper();
                        _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(txtDetalheCustoTecidoGalao.Text);
                        if (txtDetalheGastoPorFolhaGalao.Text.Trim() != "")
                            _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtDetalheGastoPorFolhaGalao.Text);
                        if (ddlEtiquetaComposicaoGalao.SelectedValue.Trim() != "")
                            _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicaoGalao.SelectedValue);
                        _novoDetalhe.FOTO_TECIDO = imgFotoTecidoDetalheGalao.ImageUrl;
                        _novoDetalhe.OBSERVACAO = txtDetalheObservacaoGalao.Text.Trim().ToUpper();
                    }
                    else
                    {
                        _novoDetalhe.GRUPO_TECIDO = prod_hb.GRUPO_TECIDO;
                        _novoDetalhe.TECIDO = prod_hb.TECIDO;
                        _novoDetalhe.COR = prod_hb.COR;
                        _novoDetalhe.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                        _novoDetalhe.NUMERO_PEDIDO = prod_hb.NUMERO_PEDIDO;
                        _novoDetalhe.FORNECEDOR = prod_hb.FORNECEDOR;
                        _novoDetalhe.LARGURA = prod_hb.LARGURA;
                        _novoDetalhe.MOLDE = prod_hb.MOLDE;
                        _novoDetalhe.CUSTO_TECIDO = prod_hb.CUSTO_TECIDO;
                        _novoDetalhe.GASTO_FOLHA_SIMULACAO = prod_hb.GASTO_FOLHA_SIMULACAO;
                        _novoDetalhe.FOTO_TECIDO = prod_hb.FOTO_TECIDO;
                        _novoDetalhe.OBSERVACAO = prod_hb.OBSERVACAO;
                        _novoDetalhe.ETIQUETA_COMPOSICAO = prod_hb.ETIQUETA_COMPOSICAO;
                    }
                }

                if (prod_hb.PROD_DETALHE == 5) //GALÃO PRÓPRIO
                {
                    if (cbGalaoProprioAlt.Checked)
                    {
                        _novoDetalhe.GRUPO_TECIDO = ddlGrupoTecido.SelectedValue.Trim();
                        _novoDetalhe.TECIDO = ddlSubGrupoTecido.SelectedValue.Trim();
                        _novoDetalhe.COR = ddlCor.SelectedValue;
                        _novoDetalhe.COR_FORNECEDOR = ddlCorFornecedor.SelectedValue;
                        _novoDetalhe.NUMERO_PEDIDO = Convert.ToInt32(txtDetalheNumeroPedidoGalaoProprio.Text.Trim());
                        _novoDetalhe.FORNECEDOR = ddlFornecedor.SelectedValue;
                        _novoDetalhe.LARGURA = Convert.ToDecimal(txtLargura.Text);
                        _novoDetalhe.MOLDE = txtMolde.Text.Trim().ToUpper();
                        _novoDetalhe.CUSTO_TECIDO = Convert.ToDecimal(txtCustoTecido.Text);
                        if (txtDetalheGastoPorFolhaGalaoProprio.Text.Trim() != "")
                            _novoDetalhe.GASTO_FOLHA_SIMULACAO = Convert.ToDecimal(txtDetalheGastoPorFolhaGalaoProprio.Text);
                        if (ddlEtiquetaComposicaoGalaoProprio.SelectedValue.Trim() != "")
                            _novoDetalhe.ETIQUETA_COMPOSICAO = Convert.ToChar(ddlEtiquetaComposicaoGalaoProprio.SelectedValue);
                        _novoDetalhe.FOTO_TECIDO = imgFotoTecido.ImageUrl;
                        _novoDetalhe.OBSERVACAO = txtDetalheObservacaoGalaoProprio.Text.Trim().ToUpper();
                    }
                    else
                    {
                        _novoDetalhe.GRUPO_TECIDO = prod_hb.GRUPO_TECIDO;
                        _novoDetalhe.TECIDO = prod_hb.TECIDO;
                        _novoDetalhe.COR = prod_hb.COR;
                        _novoDetalhe.COR_FORNECEDOR = prod_hb.COR_FORNECEDOR;
                        _novoDetalhe.NUMERO_PEDIDO = prod_hb.NUMERO_PEDIDO;
                        _novoDetalhe.FORNECEDOR = prod_hb.FORNECEDOR;
                        _novoDetalhe.LARGURA = prod_hb.LARGURA;
                        _novoDetalhe.MOLDE = prod_hb.MOLDE;
                        _novoDetalhe.CUSTO_TECIDO = prod_hb.CUSTO_TECIDO;
                        _novoDetalhe.GASTO_FOLHA_SIMULACAO = prod_hb.GASTO_FOLHA_SIMULACAO;
                        _novoDetalhe.FOTO_TECIDO = prod_hb.FOTO_TECIDO;
                        _novoDetalhe.OBSERVACAO = prod_hb.OBSERVACAO;
                        _novoDetalhe.ETIQUETA_COMPOSICAO = prod_hb.ETIQUETA_COMPOSICAO;
                    }
                }

                //validar peso do tecido para calcular estoque
                var rendimento = prodController.ObterRendimentoTecidoKG(_novoDetalhe.NUMERO_PEDIDO.ToString());
                if (rendimento != null)
                    _novoDetalhe.RENDIMENTO_1MT = rendimento.RENDIMENTO_1MT;

                AtualizarHB(_novoDetalhe);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void AtualizarGradeDetalhe()
        {
            PROD_HB_GRADE _grade = null;

            //Atualizar grade Cor2
            if (hidCor2.Value != "")
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(hidCor2.Value);
                _grade.PROD_PROCESSO = 2;
                _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, 2);
            }

            //Atualizar grade Cor3
            if (hidCor3.Value != "")
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(hidCor3.Value);
                _grade.PROD_PROCESSO = 2;
                _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, 2);
            }

            //Atualizar grade Forro
            if (hidForro.Value != "")
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(hidForro.Value);
                _grade.PROD_PROCESSO = 2;
                _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, 2);
            }

            //Atualizar grade COLANTE
            if (hidColante.Value != "")
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(hidColante.Value);
                _grade.PROD_PROCESSO = 2;
                _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, 2);
            }

            //Atualizar grade GALAO
            if (hidGalao.Value != "")
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(hidGalao.Value);
                _grade.PROD_PROCESSO = 2;
                _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, 2);
            }

            //Atualizar grade GALAO PROPRIO
            if (hidGalaoProprio.Value != "")
            {
                _grade = new PROD_HB_GRADE();
                _grade.PROD_HB = Convert.ToInt32(hidGalaoProprio.Value);
                _grade.PROD_PROCESSO = 2;
                _grade.GRADE_EXP = (txtGradeEXP.Text != "") ? Convert.ToInt32(txtGradeEXP.Text) : 0;
                _grade.GRADE_XP = (txtGradeXP.Text != "") ? Convert.ToInt32(txtGradeXP.Text) : 0;
                _grade.GRADE_PP = (txtGradePP.Text != "") ? Convert.ToInt32(txtGradePP.Text) : 0;
                _grade.GRADE_P = (txtGradeP.Text != "") ? Convert.ToInt32(txtGradeP.Text) : 0;
                _grade.GRADE_M = (txtGradeM.Text != "") ? Convert.ToInt32(txtGradeM.Text) : 0;
                _grade.GRADE_G = (txtGradeG.Text != "") ? Convert.ToInt32(txtGradeG.Text) : 0;
                _grade.GRADE_GG = (txtGradeGG.Text != "") ? Convert.ToInt32(txtGradeGG.Text) : 0;
                _grade.DATA_INCLUSAO = DateTime.Now;
                _grade.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                AtualizarGrade(_grade, 2);
            }
        }
        protected void cbCor2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c != null)
            {
                BloqueiaCampos(c.Checked, ddlDetalheGrupoTecidoCor2, ddlDetalheSubGrupoTecidoCor2, ddlDetalheFornecedorCor2, txtDetalheLarguraCor2,
                    txtDetalheCustoTecidoCor2, uploadFotoTecidoDetalheCor2, txtDetalheObservacaoCor2, btFotoTecidoDetalheIncluirCor2,
                    btDetalheComposicaoIncluirCor2, txtDetalheQtdeComposicaoCor2, txtDetalheDescricaoComposicaoCor2,
                    gvComposicaoDetalheCor2, txtDetalheNumeroPedidoCor2, txtDetalheMoldeCor2, ddlDetalheCorCor2, ddlDetalheCorFornecedorCor2, txtDetalheGastoPorFolhaCor2, ddlEtiquetaComposicaoCor2);
            }
        }
        protected void cbCor3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c != null)
            {
                BloqueiaCampos(c.Checked, ddlDetalheGrupoTecidoCor3, ddlDetalheSubGrupoTecidoCor3, ddlDetalheFornecedorCor3, txtDetalheLarguraCor3,
                    txtDetalheCustoTecidoCor3, uploadFotoTecidoDetalheCor3, txtDetalheObservacaoCor3, btFotoTecidoDetalheIncluirCor3,
                    btDetalheComposicaoIncluirCor3, txtDetalheQtdeComposicaoCor3, txtDetalheDescricaoComposicaoCor3,
                    gvComposicaoDetalheCor3, txtDetalheNumeroPedidoCor3, txtDetalheMoldeCor3, ddlDetalheCorCor3, ddlDetalheCorFornecedorCor3, txtDetalheGastoPorFolhaCor3, ddlEtiquetaComposicaoCor3);
            }
        }
        protected void cbForro_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c != null)
                BloqueiaCampos(c.Checked, ddlDetalheGrupoTecidoForro, ddlDetalheSubGrupoTecidoForro, ddlDetalheFornecedorForro, txtDetalheLarguraForro,
                    txtDetalheCustoTecidoForro, uploadFotoTecidoDetalheForro, txtDetalheObservacaoForro, btFotoTecidoDetalheIncluirForro,
                    btDetalheComposicaoIncluirForro, txtDetalheQtdeComposicaoForro, txtDetalheDescricaoComposicaoForro,
                    gvComposicaoDetalheForro, txtDetalheNumeroPedidoForro, txtDetalheMoldeForro, ddlDetalheCorForro, ddlDetalheCorFornecedorForro, txtDetalheGastoPorFolhaForro, ddlEtiquetaComposicaoForro);
        }
        protected void cbGalao_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c != null)
                BloqueiaCampos(c.Checked, ddlDetalheGrupoTecidoGalao, ddlDetalheSubGrupoTecidoGalao, ddlDetalheFornecedorGalao, txtDetalheLarguraGalao,
                    txtDetalheCustoTecidoGalao, uploadFotoTecidoDetalheGalao, txtDetalheObservacaoGalao, btFotoTecidoDetalheIncluirGalao,
                    btDetalheComposicaoIncluirGalao, txtDetalheQtdeComposicaoGalao, txtDetalheDescricaoComposicaoGalao,
                    gvComposicaoDetalheGalao, txtDetalheNumeroPedidoGalao, txtDetalheMoldeGalao, ddlDetalheCorGalao, ddlDetalheCorFornecedorGalao, txtDetalheGastoPorFolhaGalao, ddlEtiquetaComposicaoGalao);
        }
        protected void cbColante_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c != null)
                BloqueiaCampos(c.Checked, ddlDetalheGrupoTecidoColante, ddlDetalheSubGrupoTecidoColante, ddlDetalheFornecedorColante, txtDetalheLarguraColante,
                    txtDetalheCustoTecidoColante, uploadFotoTecidoDetalheColante, txtDetalheObservacaoColante, btFotoTecidoDetalheIncluirColante,
                    btDetalheComposicaoIncluirColante, txtDetalheQtdeComposicaoColante, txtDetalheDescricaoComposicaoColante,
                    gvComposicaoDetalheColante, txtDetalheNumeroPedidoColante, txtDetalheMoldeColante, ddlDetalheCorColante, ddlDetalheCorFornecedorColante, txtDetalheGastoPorFolhaColante, ddlEtiquetaComposicaoColante);
        }
        protected void cbGalaoProprio_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c != null)
            {
                txtDetalheObservacaoGalaoProprio.Enabled = c.Checked;
                txtDetalheGastoPorFolhaGalaoProprio.Enabled = c.Checked;
            }
        }

        private void BloqueiaCampos(bool _bool, DropDownList _d0, DropDownList _d00, DropDownList _d1, TextBox _t2, TextBox _t3, FileUpload _f1,
                                    TextBox _t4, Button _b1, Button _b2, TextBox _t5, TextBox _t6,
                                    GridView _g1, TextBox _t7, TextBox _t8, DropDownList _d2, DropDownList _d3, TextBox _t9, DropDownList _d4)
        {
            _f1.Enabled = _bool;

            _d0.Enabled = _bool;
            _d00.Enabled = _bool;
            _d1.Enabled = _bool;
            _t2.Enabled = _bool;
            _t3.Enabled = _bool;
            _t4.Enabled = _bool;
            _t5.Enabled = _bool;
            _t6.Enabled = _bool;
            _t7.Enabled = _bool;
            _t8.Enabled = _bool;
            _t9.Enabled = _bool;

            _d2.Enabled = _bool;
            _d3.Enabled = _bool;
            _d4.Enabled = _bool;

            _b1.Enabled = _bool;
            _b2.Enabled = _bool;

            foreach (GridViewRow r in _g1.Rows)
            {
                Button b = (Button)r.FindControl("btComposicaoDetalheExcluir");
                if (b != null)
                    b.Enabled = _bool;
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
        private void DataBoundComp(GridView comp)
        {
            GridViewRow foo = comp.FooterRow;
            if (foo != null)
            {
                decimal total = 0;
                foreach (GridViewRow r in comp.Rows)
                    total = total + Convert.ToDecimal(r.Cells[1].Text);

                foo.Cells[1].Text = total.ToString();
                foo.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void btComposicaoDetalheExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            List<ComposicaoGrade> comp = null;
            PROD_HB_COMPOSICAO c = new PROD_HB_COMPOSICAO();
            if (b != null)
            {
                try
                {
                    HiddenField _hidTotalAux = null;
                    GridView _gridAux = null;
                    CheckBox _cbAux = null;
                    string _detalhe = "";
                    if (b.CommandName == "COR2")
                    {
                        labErroDetalheComposicaoCor2.Text = "";
                        _hidTotalAux = hidDetalheCompTotalCor2;
                        _gridAux = gvComposicaoDetalheCor2;
                        _cbAux = cbCor2Alt;
                        _detalhe = "COMP_COR2";
                    }
                    if (b.CommandName == "COR3")
                    {
                        labErroDetalheComposicaoCor3.Text = "";
                        _hidTotalAux = hidDetalheCompTotalCor3;
                        _gridAux = gvComposicaoDetalheCor3;
                        _cbAux = cbCor3Alt;
                        _detalhe = "COMP_COR3";
                    }
                    if (b.CommandName == "COLANTE")
                    {
                        labErroDetalheComposicaoColante.Text = "";
                        _hidTotalAux = hidDetalheCompTotalColante;
                        _gridAux = gvComposicaoDetalheColante;
                        _cbAux = cbColanteAlt;
                        _detalhe = "COMP_COLANTE";
                    }
                    if (b.CommandName == "FORRO")
                    {
                        labErroDetalheComposicaoForro.Text = "";
                        _hidTotalAux = hidDetalheCompTotalForro;
                        _gridAux = gvComposicaoDetalheForro;
                        _cbAux = cbForroAlt;
                        _detalhe = "COMP_FORRO";
                    }
                    if (b.CommandName == "GALAO")
                    {
                        labErroDetalheComposicaoGalao.Text = "";
                        _hidTotalAux = hidDetalheCompTotalGalao;
                        _gridAux = gvComposicaoDetalheGalao;
                        _cbAux = cbGalaoAlt;
                        _detalhe = "COMP_GALAO";
                    }
                    if (b.CommandName == "GALAOPROPRIO")
                    {
                        labErroDetalheComposicaoGalaoProprio.Text = "";
                        _hidTotalAux = hidDetalheCompTotalGalaoProprio;
                        _gridAux = gvComposicaoDetalheGalaoProprio;
                        _cbAux = cbGalaoProprioAlt;
                        _detalhe = "COMP_GALAOPROPRIO";
                    }

                    GridViewRow g = (GridViewRow)b.NamingContainer;
                    if (g != null)
                        if (g.Cells[1].Text != "")
                        {
                            if (_cbAux.Checked)
                            {
                                //Obter HB
                                c = prodController.ObterComposicao(Convert.ToInt32(b.CommandArgument));
                                if (c != null)
                                    ExcluirComposicao(c.CODIGO);
                            }
                            else
                            {
                                comp = new List<ComposicaoGrade>();
                                if (Session[_detalhe] != null)
                                    comp = (List<ComposicaoGrade>)Session[_detalhe];

                                int _index = comp.FindIndex(p => p.QTDE == Convert.ToDecimal(g.Cells[1].Text) && p.DESCRICAO == g.Cells[2].Text.Trim().ToUpper());

                                comp.RemoveAt(_index);

                                Session[_detalhe] = comp;
                            }

                            _hidTotalAux.Value = (Convert.ToDecimal(_hidTotalAux.Value) - Convert.ToDecimal(g.Cells[1].Text)).ToString();
                        }
                    RecarregarComp(null, _gridAux, c.PROD_HB, _hidTotalAux, comp);

                }
                catch (Exception ex)
                {
                    labErroComposicao.Text = "ERRO (btComposicaoExcluir_Click). \n\n" + ex.Message;
                }
            }
        }
        private bool IncluirComposicaoDetalhe(Label _lalErro, TextBox _txtQtde, TextBox _txtDescricao,
                                                HiddenField _hidTotalComp, HiddenField _hidHB, GridView _gridComp,
                                                string _detalhe, bool alteracao)
        {

            _lalErro.Text = "";
            if (_txtQtde.Text.Trim() == "" || Convert.ToDecimal(_txtQtde.Text) <= 0)
            {
                _lalErro.Text = "Informe a Quantidade da composição.";
                return false;
            }
            if (_txtDescricao.Text.Trim() == "")
            {
                _lalErro.Text = "Informe a Descrição da composição.";
                return false;
            }

            if (_hidTotalComp.Value == "" || Convert.ToDecimal(_hidTotalComp.Value) == 0)
            {
                if (Convert.ToDecimal(_txtQtde.Text.Trim()) > 100)
                {
                    _lalErro.Text = "Quantidade da Composição não pode ser maior que 100%.";
                    return false;
                }
                else
                {
                    _hidTotalComp.Value = _txtQtde.Text.Trim();
                }
            }
            else
            {
                if (Convert.ToDecimal(_hidTotalComp.Value) + Convert.ToDecimal(_txtQtde.Text.Trim()) > 100)
                {
                    _lalErro.Text = "Quantidade da Composição não pode ser maior que 100%. Ainda é permitido o valor de " + ((Convert.ToDecimal(100.00)) - Convert.ToDecimal(_hidTotalComp.Value)).ToString() + "%.";
                    return false;
                }
                else
                {
                    _hidTotalComp.Value = (Convert.ToDecimal(_hidTotalComp.Value) + Convert.ToDecimal(_txtQtde.Text.Trim())).ToString();
                }
            }


            List<ComposicaoGrade> compSession = null;
            try
            {
                if (alteracao)
                {
                    PROD_HB_COMPOSICAO _novo = new PROD_HB_COMPOSICAO();
                    _novo.QTDE = Convert.ToDecimal(_txtQtde.Text);
                    _novo.DESCRICAO = _txtDescricao.Text.Trim().ToUpper();
                    _novo.PROD_HB = Convert.ToInt32(_hidHB.Value);

                    IncluirComposicao(_novo);
                }
                else
                {
                    ComposicaoGrade _compGrade = new ComposicaoGrade();
                    _compGrade.QTDE = Convert.ToDecimal(_txtQtde.Text);
                    _compGrade.DESCRICAO = _txtDescricao.Text.Trim().ToUpper();

                    compSession = new List<ComposicaoGrade>();
                    if (Session[_detalhe] != null)
                        compSession = (List<ComposicaoGrade>)Session[_detalhe];

                    compSession.Add(_compGrade);
                    Session[_detalhe] = compSession;
                }

                _txtQtde.Text = "";
                _txtDescricao.Text = "";

                RecarregarComp(null, _gridComp, ((_hidHB.Value != "") ? Convert.ToInt32(_hidHB.Value) : 0), _hidTotalComp, compSession);

                return true;
            }
            catch (Exception ex)
            {
                _lalErro.Text = "ERRO (btComposicaoIncluir_Click). \n\n" + ex.Message;
                return false;
            }
        }
        #endregion

        #region "COMPOSICAO DETALHE"
        //COR 2
        protected void btDetalheComposicaoIncluirCor2_Click(object sender, EventArgs e)
        {
            IncluirComposicaoDetalhe(labErroDetalheComposicaoCor2,
                                txtDetalheQtdeComposicaoCor2,
                                txtDetalheDescricaoComposicaoCor2,
                                hidDetalheCompTotalCor2,
                                hidCor2,
                                gvComposicaoDetalheCor2,
                                "COMP_COR2", cbCor2Alt.Checked);

        }
        protected void gvComposicaoDetalheCor2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_COMPOSICAO _composicao = e.Row.DataItem as PROD_HB_COMPOSICAO;

                    colunaComposicaoDetCor2 += 1;
                    Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                    if (_col != null)
                        _col.Text = colunaComposicaoDetCor2.ToString();

                    if (_composicao != null)
                    {
                        Button _btExcluir = e.Row.FindControl("btComposicaoDetalheExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _composicao.CODIGO.ToString();
                            if (cbCor2Inc.Checked || cbCor2Alt.Checked)
                                _btExcluir.Enabled = true;
                            else
                                _btExcluir.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void gvComposicaoDetalheCor2_DataBound(object sender, EventArgs e)
        {
            DataBoundComp(gvComposicaoDetalheCor2);
        }

        //COR 3
        protected void btDetalheComposicaoIncluirCor3_Click(object sender, EventArgs e)
        {
            IncluirComposicaoDetalhe(labErroDetalheComposicaoCor3,
                                txtDetalheQtdeComposicaoCor3,
                                txtDetalheDescricaoComposicaoCor3,
                                hidDetalheCompTotalCor3,
                                hidCor3,
                                gvComposicaoDetalheCor3,
                                "COMP_COR3", cbCor3Alt.Checked);

        }
        protected void gvComposicaoDetalheCor3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_COMPOSICAO _composicao = e.Row.DataItem as PROD_HB_COMPOSICAO;

                    colunaComposicaoDetCor3 += 1;
                    Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                    if (_col != null)
                        _col.Text = colunaComposicaoDetCor3.ToString();

                    if (_composicao != null)
                    {
                        Button _btExcluir = e.Row.FindControl("btComposicaoDetalheExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _composicao.CODIGO.ToString();
                            if (cbCor3Inc.Checked || cbCor3Alt.Checked)
                                _btExcluir.Enabled = true;
                            else
                                _btExcluir.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void gvComposicaoDetalheCor3_DataBound(object sender, EventArgs e)
        {
            DataBoundComp(gvComposicaoDetalheCor3);
        }

        //COLANTE
        protected void btDetalheComposicaoIncluirColante_Click(object sender, EventArgs e)
        {
            IncluirComposicaoDetalhe(labErroDetalheComposicaoColante,
                                    txtDetalheQtdeComposicaoColante,
                                    txtDetalheDescricaoComposicaoColante,
                                    hidDetalheCompTotalColante,
                                    hidColante,
                                    gvComposicaoDetalheColante,
                                "COMP_COLANTE", cbColanteAlt.Checked);
        }
        protected void gvComposicaoDetalheColante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_COMPOSICAO _composicao = e.Row.DataItem as PROD_HB_COMPOSICAO;

                    colunaComposicaoDetColante += 1;
                    Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                    if (_col != null)
                        _col.Text = colunaComposicaoDetColante.ToString();

                    if (_composicao != null)
                    {
                        Button _btExcluir = e.Row.FindControl("btComposicaoDetalheExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _composicao.CODIGO.ToString();
                            if (cbColanteInc.Checked || cbColanteAlt.Checked)
                                _btExcluir.Enabled = true;
                            else
                                _btExcluir.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void gvComposicaoDetalheColante_DataBound(object sender, EventArgs e)
        {
            DataBoundComp(gvComposicaoDetalheColante);
        }

        //FORRO
        protected void btDetalheComposicaoIncluirForro_Click(object sender, EventArgs e)
        {
            IncluirComposicaoDetalhe(labErroDetalheComposicaoForro,
                        txtDetalheQtdeComposicaoForro,
                        txtDetalheDescricaoComposicaoForro,
                        hidDetalheCompTotalForro,
                        hidForro,
                        gvComposicaoDetalheForro,
                                "COMP_FORRO", cbForroAlt.Checked);
        }
        protected void gvComposicaoDetalheForro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_COMPOSICAO _composicao = e.Row.DataItem as PROD_HB_COMPOSICAO;

                    colunaComposicaoDetForro += 1;
                    Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                    if (_col != null)
                        _col.Text = colunaComposicaoDetForro.ToString();

                    if (_composicao != null)
                    {
                        Button _btExcluir = e.Row.FindControl("btComposicaoDetalheExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _composicao.CODIGO.ToString();
                            if (cbForroInc.Checked || cbForroAlt.Checked)
                                _btExcluir.Enabled = true;
                            else
                                _btExcluir.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void gvComposicaoDetalheForro_DataBound(object sender, EventArgs e)
        {
            DataBoundComp(gvComposicaoDetalheForro);
        }

        //GALAO
        protected void btDetalheComposicaoIncluirGalao_Click(object sender, EventArgs e)
        {
            IncluirComposicaoDetalhe(labErroDetalheComposicaoGalao,
                        txtDetalheQtdeComposicaoGalao,
                        txtDetalheDescricaoComposicaoGalao,
                        hidDetalheCompTotalGalao,
                        hidGalao,
                        gvComposicaoDetalheGalao,
                                "COMP_GALAO", cbGalaoAlt.Checked);
        }
        protected void gvComposicaoDetalheGalao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_COMPOSICAO _composicao = e.Row.DataItem as PROD_HB_COMPOSICAO;

                    colunaComposicaoDetGalao += 1;
                    Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                    if (_col != null)
                        _col.Text = colunaComposicaoDetGalao.ToString();

                    if (_composicao != null)
                    {
                        Button _btExcluir = e.Row.FindControl("btComposicaoDetalheExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _composicao.CODIGO.ToString();

                        if (cbGalaoInc.Checked || cbGalaoAlt.Checked)
                            _btExcluir.Enabled = true;
                        else
                            _btExcluir.Enabled = false;
                    }
                }
            }
        }
        protected void gvComposicaoDetalheGalao_DataBound(object sender, EventArgs e)
        {
            DataBoundComp(gvComposicaoDetalheGalao);
        }

        //GALAO PROPRIO
        protected void btDetalheComposicaoIncluirGalaoProprio_Click(object sender, EventArgs e)
        {
            IncluirComposicaoDetalhe(labErroDetalheComposicaoGalaoProprio,
                        txtDetalheQtdeComposicaoGalaoProprio,
                        txtDetalheDescricaoComposicaoGalaoProprio,
                        hidDetalheCompTotalGalaoProprio,
                        hidGalaoProprio,
                        gvComposicaoDetalheGalaoProprio,
                                "COMP_GALAOPROPRIO", cbGalaoProprioAlt.Checked);
        }
        protected void gvComposicaoDetalheGalaoProprio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_COMPOSICAO _composicao = e.Row.DataItem as PROD_HB_COMPOSICAO;

                    colunaComposicaoDetGalaoProprio += 1;
                    Literal _col = e.Row.FindControl("litComposicaoColuna") as Literal;
                    if (_col != null)
                        _col.Text = colunaComposicaoDetGalaoProprio.ToString();

                    if (_composicao != null)
                    {
                        Button _btExcluir = e.Row.FindControl("btComposicaoDetalheExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _composicao.CODIGO.ToString();
                            if (cbGalaoProprioInc.Checked || cbGalaoProprioAlt.Checked)
                                _btExcluir.Enabled = true;
                            else
                                _btExcluir.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void gvComposicaoDetalheGalaoProprio_DataBound(object sender, EventArgs e)
        {
            DataBoundComp(gvComposicaoDetalheGalaoProprio);
        }

        #endregion

    }
}
