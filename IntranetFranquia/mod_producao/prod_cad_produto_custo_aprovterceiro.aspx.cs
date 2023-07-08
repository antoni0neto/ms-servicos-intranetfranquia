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
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class prod_cad_produto_custo_aprovterceiro : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        ProdutoCusto pc = new ProdutoCusto();

        decimal totalAviamento = 0;
        int coluna = 0;
        int colunaReal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                CarregarColecoes();

                dialogPai.Visible = false;
                divGridReal.Visible = false;
                btAprovar.Visible = false;

                string colecao = "";
                string produto = "";
                string mostruario = "";
                if (Request.QueryString["c"] != null && Request.QueryString["c"].ToString() != "")
                    colecao = Request.QueryString["c"].ToString();

                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != "")
                    produto = Request.QueryString["p"].ToString();

                if (Request.QueryString["m"] != null && Request.QueryString["m"].ToString() != "")
                    mostruario = Request.QueryString["m"].ToString();

                if (colecao != "0" && produto != "0" && mostruario != "0")
                {
                    ddlColecoes.SelectedValue = colecao;
                    txtProdutoLinxFiltro.Text = produto;
                    btBuscar_Click(null, null);
                }

                btDesaprovarCusto.Visible = false;

            }

            //Evitar duplo clique no botão
            btValidar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btValidar, null) + ";");
            btAprovar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAprovar, null) + ";");
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "INICIALIZAR DADOS"
        private void CarregarColecoes()
        {
            var _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private SP_OBTER_PRODUTO_ACABADO_PRECOResult ObterProduto()
        {
            char mostruario = 'N'; //deprecated
            var precoTerceiro = prodController.ObterProdutoAcabadoTerceiroPreco(txtProdutoLinxFiltro.Text, txtNomeFiltro.Text, mostruario);

            return precoTerceiro;
        }
        #endregion

        #region "AÇÕES"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                pnlCusto.Visible = false;
                divGridReal.Visible = false;
                txtPrecoAprovado.Text = "";

                if (ddlColecoes.SelectedValue.Trim() == "" || ddlColecoes.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }
                if (txtProdutoLinxFiltro.Text.Trim() == "" && txtNomeFiltro.Text.Trim() == "")
                {
                    labErro.Text = "Informe Código e/ou Nome do Produto.";
                    return;
                }

                var produto = ObterProduto();

                if (produto == null)
                {
                    labErro.Text = "PRODUTO NÃO ESTÁ CADASTRADO NO LINX COMO REVENDA.";
                    return;
                }

                txtProduto.Text = produto.PRODUTO.Trim();
                txtGrupo.Text = produto.GRUPO_PRODUTO.Trim();
                txtNome.Text = produto.DESC_PRODUTO.Trim();
                imgFotoPeca.ImageUrl = produto.FOTO;

                if (produto.CUSTO_PRODUTO > 0)
                    txtValorCusto.Text = produto.CUSTO_PRODUTO.ToString("###,###,##0.00");
                else
                    txtValorCusto.Text = "";

                if (produto.PRECO_SUGERIDO > 0)
                    txtPrecoSugerido.Text = Convert.ToDecimal(produto.PRECO_SUGERIDO).ToString("###,###,##0.00");
                else
                    txtPrecoSugerido.Text = "";

                CarregarAviamentos(produto);
                CarregarPrecos(produto);

                var precoAprovado = prodController.ObterCustoTerceiro(produto.PRODUTO, 'N');

                if (precoAprovado != null)
                {
                    txtValorCusto.Text = precoAprovado.CUSTO.ToString("###,###,##0.00");
                    btnCalcular_Click(btnCalcular, null);

                    txtPrecoAprovado.Text = precoAprovado.PRECO.ToString("###,###,##0.00");
                    btValidar_Click(btValidar, null);
                }

                pnlCusto.Visible = true;
                Session["COLECAO"] = ddlColecoes.SelectedValue;

                //VALIDAR SE CUSTO JA EXISTE - NAO PODE CALCULAR NOVAMENTE SE O PRECO JA FOI APROVADO
                btValidar.Visible = true;
                btValidar.Enabled = true;
                btAprovar.Enabled = true;


                if (produto.PRECO_APROVADO != null)
                {
                    labErro.Text = "ATENÇÃO: PRODUTO JÁ FOI APROVADO.";
                    btValidar.Visible = false;
                    btValidar.Enabled = false;
                    btAprovar.Visible = false;
                    btAprovar.Enabled = false;

                    btDesaprovarCusto.Visible = true;
                }

            }
            catch (Exception ex)
            {
                labErro.Text = "" + ex.Message;
            }
        }
        #endregion

        #region "APROVACAO"

        //PRECO REAL DE CUSTO
        private bool InserirCustoOrigem(string codigoProduto, string colecao, int codOrigem, decimal custoTotal, char mostruario, char tipoCusto)
        {
            int codigo = 0;

            try
            {
                codigo = prodController.ObterCustoOrigem().Where(p => p.PRODUTO.Trim() == codigoProduto.Trim() &&
                                                                        p.COLECAO.Trim() == colecao.Trim() &&
                                                                        p.COD_CUSTO_ORIGEM == codOrigem &&
                                                                        p.MOSTRUARIO == mostruario &&
                                                                        p.CUSTO_MOSTRUARIO == tipoCusto).SingleOrDefault().CODIGO;
            }
            catch (Exception)
            {
                codigo = 0;
            }

            PRODUTO_CUSTO_ORIGEM custoOrigem = new PRODUTO_CUSTO_ORIGEM();
            custoOrigem.CODIGO = codigo;
            custoOrigem.PRODUTO = codigoProduto;
            custoOrigem.COLECAO = colecao;
            custoOrigem.COD_CUSTO_ORIGEM = codOrigem;
            custoOrigem.CUSTO_TOTAL = Math.Round(custoTotal, 2);
            custoOrigem.MOSTRUARIO = mostruario;
            custoOrigem.CUSTO_MOSTRUARIO = tipoCusto;

            if (codigo <= 0)
                prodController.InserirCustoOrigem(custoOrigem);
            else
                prodController.AtualizarCustoOrigem(custoOrigem);

            return true;
        }
        private decimal CalcularCMV()
        {
            decimal vAviamento = 0;
            decimal vCusto = 0;

            foreach (GridViewRow rowA in gvAviamentoNovo.Rows)
                vAviamento += (((Literal)rowA.FindControl("litTotal")).Text == "") ? 0 : Convert.ToDecimal(((Literal)rowA.FindControl("litTotal")).Text);

            vCusto = Convert.ToDecimal(txtValorCusto.Text.Trim());

            return (vAviamento + vCusto);
        }

        private bool InserirPrecoProduto(string codigoProduto, string codigoTabela, decimal preco)
        {
            PRODUTOS_PRECO _produtoPreco = new PRODUTOS_PRECO();

            _produtoPreco.PRODUTO = codigoProduto;
            _produtoPreco.CODIGO_TAB_PRECO = codigoTabela;
            _produtoPreco.PRECO1 = preco;
            _produtoPreco.PRECO2 = 0;
            _produtoPreco.PRECO3 = 0;
            _produtoPreco.PRECO4 = 0;
            _produtoPreco.LIMITE_DESCONTO = 0;
            _produtoPreco.PROMOCAO_DESCONTO = 0;
            _produtoPreco.MARK_UP_PREVISTO = 0;
            _produtoPreco.PROMOCAO_ATACADO = 0;
            _produtoPreco.PRECO_LIQUIDO1 = preco;
            _produtoPreco.PRECO_LIQUIDO2 = 0;
            _produtoPreco.PRECO_LIQUIDO3 = 0;
            _produtoPreco.PRECO_LIQUIDO4 = 0;
            _produtoPreco.ULT_ATUALIZACAO = DateTime.Now.Date;
            baseController.InserirPrecoProduto(_produtoPreco);
            return true;
        }
        private bool AtualizarPrecoProduto(string codigoProduto, string codigoTabela, decimal preco)
        {
            baseController.AtualizarPrecoProduto(codigoProduto, codigoTabela, preco);
            return true;
        }

        private PRODUTO_CUSTO_SIMULACAO CalcularPrecoAprovadoTerceiro()
        {
            //char mostruario = 'N';
            //if (txtMostruario.Text.Trim().ToUpper() == "SIM")
            //    mostruario = 'S';

            //Obter valores para calculo do custo
            var valoresCalculo = prodController.ObterCustoSimulacaoValoresCalculo();

            decimal CMV = CalcularCMV();
            var precoAprovado = pc.CalcularCustoProdutoTerceiro(CMV, 0, Convert.ToDecimal(txtPrecoAprovado.Text.Trim()), valoresCalculo);
            //precoAprovado = pc.CalcularLucro(precoAprovado, valoresCalculo);

            return precoAprovado;
        }

        protected void btValidar_Click(object sender, EventArgs e)
        {
            labErroValidacao.Text = "";
            try
            {

                if (txtPrecoAprovado.Text.Trim() == "" || Convert.ToDecimal(txtPrecoAprovado.Text.Trim()) <= 0)
                {
                    labErroValidacao.Text = "Informe o Preço Aprovado.";
                    return;
                }

                int colecao = 0;
                if (!int.TryParse(ddlColecoes.SelectedValue, out colecao))
                {
                    labErroValidacao.Text = "Erro ao obter Coleção. Entre em contato com TI.";
                    return;
                }

                if (txtValorCusto.Text == "")
                {
                    labErroCalculo.Text = "Informe o valor do Custo do Produto.";
                    return;
                }

                var precoAprovado = CalcularPrecoAprovadoTerceiro();

                if (precoAprovado != null)
                {
                    //Transformar coluna em linha
                    List<ProdutoCusto> lstAUX = new List<ProdutoCusto>();
                    lstAUX.Add(new ProdutoCusto
                    {
                        COLUNA_NOME = "MKUP",
                        COLUNA_A = precoAprovado.MARKUP
                    });
                    lstAUX.Add(new ProdutoCusto
                    {
                        COLUNA_NOME = "Lucro",
                        COLUNA_A = precoAprovado.LUCRO_PORC
                    });
                    lstAUX.Add(new ProdutoCusto
                    {
                        COLUNA_NOME = "Preço",
                        COLUNA_A = precoAprovado.PRECO_PRODUTO
                    });

                    gvCustoReal.DataSource = lstAUX;
                    gvCustoReal.DataBind();

                    divGridReal.Visible = true;
                    btAprovar.Visible = true;
                }
            }
            catch (Exception ex)
            {
                labErroValidacao.Text = ex.Message;
                divGridReal.Visible = false;
                btAprovar.Visible = false;
            }
        }
        protected void btAprovar_Click(object sender, EventArgs e)
        {
            try
            {
                bool enviarEmailProduto = false;

                labErroValidacao.Text = "";

                char mostruario = 'N';

                if (txtPrecoAprovado.Text.Trim() == "" || Convert.ToDecimal(txtPrecoAprovado.Text.Trim()) <= 0)
                {
                    labErroValidacao.Text = "Informe o Preço Aprovado.";
                    return;
                }

                var produto = ObterProduto();
                if (produto == null)
                {
                    labErroValidacao.Text = "PRODUTO NÃO ENCONTRADO.";
                    return;
                }

                int colecao = 0;
                if (!int.TryParse(ddlColecoes.SelectedValue, out colecao))
                {
                    labErroValidacao.Text = "Erro ao obter Coleção. Entre em contato com TI.";
                    return;
                }

                var precoAprovado = CalcularPrecoAprovadoTerceiro();

                var custoTerceiro = prodController.ObterCustoTerceiro(txtProduto.Text.Trim(), mostruario);
                if (custoTerceiro == null)
                {
                    custoTerceiro = new PRODUTO_CUSTO_TERCEIRO();
                }
                custoTerceiro.PRODUTO = txtProduto.Text.Trim();
                custoTerceiro.MOSTRUARIO = mostruario;
                custoTerceiro.PRECO = precoAprovado.PRECO_PRODUTO;
                custoTerceiro.CUSTO = Convert.ToDecimal(txtValorCusto.Text.Trim());
                if (custoTerceiro.CODIGO > 0)
                    prodController.AtualizarCustoTerceiro(custoTerceiro);
                else
                    prodController.InserirCustoTerceiro(custoTerceiro);

                // Inserir custo origem do imposto
                decimal CMV = CalcularCMV();
                InserirCustoOrigem(produto.PRODUTO.Trim(), ddlColecoes.SelectedValue.Trim(), 8, (CMV), mostruario, 'N');
                //InserirCustoOrigem(produto.PRODUTO.Trim(), "", 5, (precoAprovado.CUSTO_COM_IMPOSTO - CMV), mostruario, 'N');


                //Obter valores para calculo do custo
                var valoresCalculo = prodController.ObterCustoSimulacaoValoresCalculo();

                //decimal custo12 = (CMV * Convert.ToDecimal(valoresCalculo.VALOR_PORC_AMAIS25));

                ///// SOMENTE INSERE QUQUANDO EH SIMULAAO
                // REMOVIDO - 19/09/2018
                //if (produto.MOSTRUARIO == 'S') // && QUANDO FOR APROVACAO SIMULAÇÂO
                //{

                //}

                // ATUALIZAR PRECO NO LINX - TABELA = PRODUTOS_PRECOS
                //ATUALIZAR SOMENTE QDO FOR CORTE
                if (produto.MOSTRUARIO == 'N')
                {
                    // ADICIONADO PARA APROVAR SOMENTE UMA VEZ
                    // 19/09/2018
                    decimal precoTACalc = Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2);

                    //decimal precoTRCalc = Math.Round((precoTACalc / 2), 0);
                    decimal precoTRCalc;
                    int fracao = Convert.ToInt32((Math.Abs(precoTACalc / 2) % 1).ToString().Substring(2));
                    if (fracao == 50 || fracao == 5)
                    {
                        precoTRCalc = Math.Abs((precoTACalc / 2)) - Math.Abs((precoTACalc / 2)) % 1 + 1;
                    }
                    else
                    {
                        precoTRCalc = Math.Round((precoTACalc / 2), 0);
                    }

                    // TA = TO/2 = MATH.ROUND(2)
                    //INSERIR TA SE NAO EXISTIR
                    var precoTA = baseController.BuscaPrecoProduto(produto.PRODUTO, "TA");
                    if (precoTA == null || precoTA.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TA", Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2));
                    else if (precoTA.Count == 1 && precoTA[0].PRECO1 <= 0)
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TA", Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2));

                    // TL = PRECO LOJA
                    var precoTL = baseController.BuscaPrecoProduto(produto.PRODUTO, "TL");
                    if (precoTL == null || precoTL.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);

                    // TR = PRECO REPRESENTANTE
                    var precoTR = baseController.BuscaPrecoProduto(produto.PRODUTO, "TR");
                    if (precoTR == null || precoTR.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TR", precoTRCalc);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TR", precoTRCalc);

                    // TP = TABELA PARA
                    var precoTP = baseController.BuscaPrecoProduto(produto.PRODUTO, "TP");
                    var precoValor = (Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2) + (Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2) * (8.00M / 100.00M)));
                    if (precoTP == null || precoTP.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TP", Math.Round((precoValor), 0));
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TP", Math.Round((precoValor), 0));


                    //// TL = PRECO LOJA
                    //var precoTL = baseController.BuscaPrecoProduto(produto.PRODUTO, "TL");
                    //if (precoTL == null || precoTL.Count() <= 0)
                    //    InserirPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);
                    //else
                    //    AtualizarPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);

                    // TO = PRECO ORIGINAL
                    var precoTO = baseController.BuscaPrecoProduto(produto.PRODUTO, "TO");
                    if (precoTO == null || precoTO.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TO", precoAprovado.PRECO_PRODUTO);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TO", precoAprovado.PRECO_PRODUTO);

                    // APENAS CRIA, SE HOUVER ALTERACAO ENVIAR E-MAIL
                    // SC = SAO CAETANO
                    var precoSC = baseController.BuscaPrecoProduto(produto.PRODUTO, "SC");
                    if (precoSC == null || precoSC.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "SC", precoAprovado.PRECO_PRODUTO);
                    else
                        enviarEmailProduto = true;

                    // TD = TABELA DESCONTO
                    var precoTD = baseController.BuscaPrecoProduto(produto.PRODUTO, "TD");
                    if (precoTD == null || precoTD.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TD", precoAprovado.PRECO_PRODUTO);
                    else
                        enviarEmailProduto = true;

                    //RETIRADO 07/11/2017 - NOVO CUSTO
                    //// TN = TABELA NORTE
                    //var precoTN = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TN");
                    //if (precoTN == null || precoTN.Count() <= 0)
                    //    InserirPrecoProduto(txtProduto.Text.Trim(), "TN", precoAprovado.PRECO_PRODUTO);
                    //else
                    //    enviarEmailProduto = true;

                    var custoProduto = prodController.ObterCustoOrigem(produto.PRODUTO, Convert.ToChar(produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                p.COD_CUSTO_ORIGEM == 8 && p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim()).FirstOrDefault();

                    // TX = CUSTO CMAX
                    decimal custoTX = Convert.ToDecimal(custoProduto.CUSTO_TOTAL);
                    var precoTX = baseController.BuscaPrecoProduto(produto.PRODUTO, "TX");
                    if (precoTX == null || precoTX.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TX", custoTX);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TX", custoTX);

                    var valCustoTM = ((custoTX) - (custoTX * 30.00M / 100.00M));
                    var precoTM = baseController.BuscaPrecoProduto(produto.PRODUTO, "TM");
                    if (precoTM == null || precoTM.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TM", valCustoTM);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TM", valCustoTM);

                    // TT = PRECO TT
                    var precoTT = baseController.BuscaPrecoProduto(produto.PRODUTO, "TT");
                    if (precoTT == null || precoTT.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TT", custoTX);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TT", custoTX);

                    var precoTS = baseController.BuscaPrecoProduto(produto.PRODUTO, "TS");
                    if (precoTS == null || precoTS.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TS", custoTX);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TS", custoTX);

                    // TH = PRECO TH
                    var precoTH = baseController.BuscaPrecoProduto(produto.PRODUTO, "TH");
                    if (precoTH == null || precoTH.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TH", custoTX);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TH", custoTX);

                    var precoTC = baseController.BuscaPrecoProduto(produto.PRODUTO, "TC");
                    if (precoTC == null || precoTC.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TC", (custoTX));
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TC", (custoTX));

                    var precoTU = baseController.BuscaPrecoProduto(produto.PRODUTO, "TU");
                    if (precoTU == null || precoTU.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TU", (custoTX));
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TU", (custoTX));

                    PRODUTO _produto = baseController.ObterProdutoLinx(txtProduto.Text.Trim());
                    if (_produto != null)
                    {

                        _produto.PRODUTO1 = txtProduto.Text.Trim();
                        _produto.CUSTO_REPOSICAO1 = (custoTX);
                        baseController.AtualizarProdutoLinx(_produto);
                    }

                }


                //Enviar Email
                if (Constante.enviarEmail)
                {
                    EnviarEmail(produto);

                    if (enviarEmailProduto)
                        EnviarEmailAlteracao(produto);
                }

                labProduto.Text = txtProduto.Text.Trim();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, 'Aprovação Preço Terceiro': function () { window.open('prod_cad_produto_custo_aprovterceiro.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErroValidacao.Text = ex.Message;
            }
        }
        protected void gvCustoReal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ProdutoCusto _simulacao = e.Row.DataItem as ProdutoCusto;

                    colunaReal += 1;
                    if (_simulacao != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaReal.ToString();

                        Literal _litColunaNome = e.Row.FindControl("litColunaNome") as Literal;
                        if (_litColunaNome != null)
                            _litColunaNome.Text = _simulacao.COLUNA_NOME;

                        Literal _litColunaA = e.Row.FindControl("litColunaA") as Literal;
                        if (_litColunaA != null)
                        {
                            if (colunaReal == 1) // MARKUP
                                _litColunaA.Text = _simulacao.COLUNA_A.ToString("0.0#");
                            if (colunaReal == 2) // LUCRO
                                _litColunaA.Text = _simulacao.COLUNA_A.ToString("##0.00").Replace(",00", "") + "%";
                            if (colunaReal == 3) // PREÇO
                                _litColunaA.Text = "R$ " + _simulacao.COLUNA_A.ToString("###,###,##0.00");
                        }
                    }
                }
            }
        }
        #endregion

        #region "GRIDS"
        /*AVIAMENTOS*/
        private void CarregarAviamentos(SP_OBTER_PRODUTO_ACABADO_PRECOResult produto)
        {
            if (produto != null)
            {
                var lstAviamentos = prodController.ObterMaterialExtraTerceiro(produto.PRODUTO, Convert.ToChar(produto.MOSTRUARIO));

                if (lstAviamentos == null || lstAviamentos.Count <= 0)
                {
                    lstAviamentos.Insert(0, new PRODUTO_MATERIAL_EXTRA { CODIGO = 0 });
                }

                gvAviamentoNovo.DataSource = lstAviamentos;
                gvAviamentoNovo.DataBind();
            }
        }
        protected void gvAviamentoNovo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PRODUTO_MATERIAL_EXTRA _aviamento = e.Row.DataItem as PRODUTO_MATERIAL_EXTRA;

                    if (_aviamento != null)
                    {
                        if (_aviamento.CODIGO == 0)
                        {
                            e.Row.Visible = false;
                            return;
                        }

                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                            _litPreco.Text = _aviamento.PRECO.ToString("#########0.00");

                        Literal _litConsumo = e.Row.FindControl("litConsumo") as Literal;
                        if (_litConsumo != null)
                            _litConsumo.Text = _aviamento.CONSUMO.ToString("###,###,##0.00");

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                        {
                            decimal total = 0;
                            total = (_aviamento.PRECO * _aviamento.CONSUMO);
                            _litTotal.Text = total.ToString("#########0.00");

                            totalAviamento += total;
                        }
                    }
                }
            }
        }
        protected void gvAviamentoNovo_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAviamentoNovo.FooterRow;
            if (footer != null)
            {
            }
        }
        protected void btIncluirAviamento_Click(object sender, EventArgs e)
        {
            labErroAviamento.Text = "";
            try
            {
                GridViewRow row = gvAviamentoNovo.FooterRow;
                if (row != null)
                {
                    TextBox _txtAviamento = row.FindControl("txtAviamento") as TextBox;
                    TextBox _txtPreco = row.FindControl("txtPreco") as TextBox;
                    TextBox _txtConsumo = row.FindControl("txtConsumo") as TextBox;

                    if (_txtAviamento.Text.Trim() == "")
                    {
                        labErroAviamento.Text = "Informe o Aviamento.";
                        return;
                    }

                    if (_txtPreco.Text.Trim() == "")
                    {
                        labErroAviamento.Text = "Informe o Preço do Tecido.";
                        return;
                    }

                    if (_txtConsumo.Text.Trim() == "")
                    {
                        labErroAviamento.Text = "Informe o Consumo.";
                        return;
                    }

                    var matExtra = new PRODUTO_MATERIAL_EXTRA();
                    matExtra.PRODUTO = txtProduto.Text.Trim();
                    matExtra.MOSTRUARIO = 'N';
                    matExtra.MATERIAL = _txtAviamento.Text.Trim().ToUpper();
                    matExtra.PRECO = Convert.ToDecimal(_txtPreco.Text);
                    matExtra.CONSUMO = Convert.ToDecimal(_txtConsumo.Text);

                    prodController.InserirMaterialExtraTerceiro(matExtra);

                    var produto = ObterProduto();
                    CarregarAviamentos(produto);

                }
            }
            catch (Exception ex)
            {
                labErroAviamento.Text = ex.Message;
            }
        }
        protected void btExcluirAviamento_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    GridViewRow row = (GridViewRow)bt.NamingContainer;
                    if (row != null)
                    {
                        int codigoMaterial = Convert.ToInt32(gvAviamentoNovo.DataKeys[row.RowIndex].Value);
                        if (codigoMaterial > 0)
                        {
                            prodController.ExcluirMaterialExtraTerceiro(codigoMaterial);

                            var produto = ObterProduto();
                            CarregarAviamentos(produto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErroAviamento.Text = ex.Message;
            }
        }

        #endregion

        private void CarregarPrecos(SP_OBTER_PRODUTO_ACABADO_PRECOResult produto)
        {
            var precoTO = baseController.BuscaPrecoProduto(produto.PRODUTO, "TO");
            if (precoTO != null && precoTO.Count() > 0)
                txtPrecoTO.Text = Convert.ToDecimal(precoTO[0].PRECO1).ToString("###,###,##0.00");
            else
                txtPrecoTO.Text = "";

            var precoTA = baseController.BuscaPrecoProduto(produto.PRODUTO, "TA");
            if (precoTA != null && precoTA.Count() > 0)
                txtPrecoTA.Text = Convert.ToDecimal(precoTA[0].PRECO1).ToString("###,###,##0.00");
            else
                txtPrecoTA.Text = "";

            var precoTL = baseController.BuscaPrecoProduto(produto.PRODUTO, "TL");
            if (precoTL != null && precoTL.Count() > 0)
                txtPrecoTL.Text = Convert.ToDecimal(precoTL[0].PRECO1).ToString("###,###,##0.00");
            else
                txtPrecoTL.Text = "";

        }

        private void EnviarEmail(SP_OBTER_PRODUTO_ACABADO_PRECOResult produto)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Preço de Produto Terceiro - " + produto.PRODUTO;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(produto, false);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(4, 2).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private void EnviarEmailAlteracao(SP_OBTER_PRODUTO_ACABADO_PRECOResult produto)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet: ALTERAÇÃO DE PREÇO TERCEIRO - Produto: " + produto.PRODUTO;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(produto, true);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(4, 3).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(SP_OBTER_PRODUTO_ACABADO_PRECOResult produto, bool _alteracao)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>E-mail Preço do Produto</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divPreco' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 300pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>Preço de Produto TERCEIRO " + ((!_alteracao) ? "Aprovado" : "ALTERADO") + " - " + ((produto.MOSTRUARIO == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO") + "</h3>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 300pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produto.PRODUTO.Trim());
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Grupo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produto.GRUPO_PRODUTO.Trim());
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
            sb.Append("                                    " + produto.DESC_PRODUTO.Trim());
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            var precoTC = baseController.BuscaPrecoProduto(produto.PRODUTO, "TC");
            if (precoTC != null && precoTC.Count() > 0 && produto.MOSTRUARIO == 'N')
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Custo - TC:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTC[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTO = baseController.BuscaPrecoProduto(produto.PRODUTO, "TO");
            if (precoTO != null && precoTO.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - TO:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTO[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            var precoSC = baseController.BuscaPrecoProduto(produto.PRODUTO, "SC");
            if (precoSC != null && precoSC.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - SC:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoSC[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTL = baseController.BuscaPrecoProduto(produto.PRODUTO, "TL");
            if (precoTL != null && precoTL.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - TL:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTL[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTD = baseController.BuscaPrecoProduto(produto.PRODUTO, "TD");
            if (precoTD != null && precoTD.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - TD:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTD[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTN = baseController.BuscaPrecoProduto(produto.PRODUTO, "TN");
            if (precoTN != null && precoTN.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - TN:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTN[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTX = baseController.BuscaPrecoProduto(produto.PRODUTO, "TX");
            if (precoTX != null && precoTX.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - TX:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTX[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTA = baseController.BuscaPrecoProduto(produto.PRODUTO, "TA");
            if (precoTA != null && precoTA.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - TA:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTA[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTT = baseController.BuscaPrecoProduto(produto.PRODUTO, "TT");
            if (precoTT != null && precoTT.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Preço - TT:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + precoTT[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            //var comp = prodController.ObterComposicaoHB(produto.CODIGO);

            //if (comp != null && comp.Count() > 0)
            //{
            //    sb.Append("                            <tr>");
            //    sb.Append("                                <td>");
            //    sb.Append("                                    Composição:");
            //    sb.Append("                                </td>");
            //    sb.Append("                                <td>");
            //    sb.Append("                                    " + comp[0].QTDE + "% " + comp[0].DESCRICAO);
            //    sb.Append("                                </td>");
            //    sb.Append("                                <td>");
            //    sb.Append("                                    &nbsp;");
            //    sb.Append("                                </td>");
            //    sb.Append("                            </tr>");
            //    foreach (var c in comp.Where(p => p.CODIGO != comp[0].CODIGO))
            //    {
            //        sb.Append("                            <tr>");
            //        sb.Append("                                <td>");
            //        sb.Append("                                    &nbsp;");
            //        sb.Append("                                </td>");
            //        sb.Append("                                <td>");
            //        sb.Append("                                    " + c.QTDE + "% " + c.DESCRICAO);
            //        sb.Append("                                </td>");
            //        sb.Append("                                <td>");
            //        sb.Append("                                    &nbsp;");
            //        sb.Append("                                </td>");
            //        sb.Append("                            </tr>");
            //    }

            //    sb.Append("                            <tr>");
            //    sb.Append("                                <td colspan='3'>");
            //    sb.Append("                                    &nbsp;");
            //    sb.Append("                                </td>");
            //    sb.Append("                            </tr>");
            //}
            //sb.Append("                             <tr>");
            //sb.Append("                                 <td colspan='3'>");
            //sb.Append("                                     <table cellpadding='0' cellspacing='0' style='width: 400pt; padding: 0px; color: black;");
            //sb.Append("                                         font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
            //sb.Append("                                         border: 1px solid #ccc;'>");
            //sb.Append("                                         <tr style='background-color: #ccc;'>");
            //sb.Append("                                             <td>");
            //sb.Append("                                                 &nbsp;");
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center;'>");
            //sb.Append("                                                  &nbsp;");
            //sb.Append("                                             </td>");
            //sb.Append("                                         </tr>");

            //var _grade = prodController.ObterGradeHB(produto.CODIGO, 3);
            //sb.Append("                                         <tr>");
            //sb.Append("                                             <td>");
            //sb.Append("                                                 " + prodController.ObterCoresBasicas(produto.COR.Trim()).DESC_COR.Trim());
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_EXP)));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_XP)));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_PP)));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_P)));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_M)));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_G)));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : _grade.GRADE_GG)));
            //sb.Append("                                             </td>");
            //sb.Append("                                             <td style='text-align: center; width: 55px;'>");
            //sb.Append("                                                  " + (((_grade == null) ? 0 : (_grade.GRADE_EXP + _grade.GRADE_XP + _grade.GRADE_PP + _grade.GRADE_P + _grade.GRADE_M + _grade.GRADE_G + _grade.GRADE_GG))));
            //sb.Append("                                             </td>");
            //sb.Append("                                         </tr>");
            //sb.Append("                                     </table>");
            //sb.Append("                                 </td>");
            //sb.Append("                             </tr>");
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

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                decimal CMV = 0;
                labErroCalculo.Text = "";

                if (txtValorCusto.Text == "")
                {
                    labErroCalculo.Text = "Informe o valor do Custo do Produto.";
                    return;
                }

                CMV = CalcularCMV();

                CarregarSimulacao(CMV);
            }
            catch (Exception ex)
            {
                labErroValidacao.Text = ex.Message;
            }


        }
        private void CarregarSimulacao(decimal CMV)
        {
            var lstCustoSimulacao = new List<PRODUTO_CUSTO_SIMULACAO>();


            var valoresCalculo = prodController.ObterCustoSimulacaoValoresCalculo();
            decimal mkup = Convert.ToDecimal(3.0);
            PRODUTO_CUSTO_SIMULACAO novoCusto = null;
            for (int i = 0; i < 5; i++)
            {
                novoCusto = new PRODUTO_CUSTO_SIMULACAO();

                novoCusto = pc.CalcularCustoProdutoTerceiro(CMV, mkup, 0, valoresCalculo);
                lstCustoSimulacao.Add(novoCusto);

                mkup += Convert.ToDecimal(0.5);
            }


            List<decimal> precoProduto = new List<decimal>();
            List<decimal> markUp = new List<decimal>();
            List<decimal> lucro = new List<decimal>();
            foreach (PRODUTO_CUSTO_SIMULACAO custoS in lstCustoSimulacao)
            {
                if (custoS != null)
                {
                    precoProduto.Add(custoS.PRECO_PRODUTO);
                    markUp.Add(custoS.MARKUP);
                    lucro.Add(custoS.LUCRO_PORC);
                }
            }

            //Transformar coluna em linha
            List<ProdutoCusto> lstAUX = new List<ProdutoCusto>();
            lstAUX.Add(new ProdutoCusto
            {
                COLUNA_NOME = "MKUP",
                COLUNA_A = markUp[0],
                COLUNA_B = markUp[1],
                COLUNA_C = markUp[2],
                COLUNA_D = markUp[3],
                COLUNA_E = markUp[4]
            });
            lstAUX.Add(new ProdutoCusto
            {
                COLUNA_NOME = "Lucro",
                COLUNA_A = lucro[0],
                COLUNA_B = lucro[1],
                COLUNA_C = lucro[2],
                COLUNA_D = lucro[3],
                COLUNA_E = lucro[4]
            });
            lstAUX.Add(new ProdutoCusto
            {
                COLUNA_NOME = "Preço",
                COLUNA_A = precoProduto[0],
                COLUNA_B = precoProduto[1],
                COLUNA_C = precoProduto[2],
                COLUNA_D = precoProduto[3],
                COLUNA_E = precoProduto[4]
            });

            gvCustoSimulacao.DataSource = lstAUX;
            gvCustoSimulacao.DataBind();
        }
        protected void gvCustoSimulacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ProdutoCusto _simulacao = e.Row.DataItem as ProdutoCusto;

                    coluna += 1;
                    if (_simulacao != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litColunaA = e.Row.FindControl("litColunaA") as Literal;
                        if (_litColunaA != null)
                        {
                            if (coluna == 1) // MARKUP
                                _litColunaA.Text = _simulacao.COLUNA_A.ToString("0.0#");
                            if (coluna == 2) // LUCRO
                                _litColunaA.Text = _simulacao.COLUNA_A.ToString("##0.00").Replace(",00", "") + "%";
                            if (coluna == 3) // PREÇO
                                _litColunaA.Text = "R$ " + _simulacao.COLUNA_A.ToString("###,###,##0.00");
                        }
                        Literal _litColunaB = e.Row.FindControl("litColunaB") as Literal;
                        if (_litColunaB != null)
                        {
                            if (coluna == 1) // MARKUP
                                _litColunaB.Text = _simulacao.COLUNA_B.ToString("0.0#");
                            if (coluna == 2) // LUCRO
                                _litColunaB.Text = _simulacao.COLUNA_B.ToString("##0.00").Replace(",00", "") + "%";
                            if (coluna == 3) // PREÇO
                                _litColunaB.Text = "R$ " + _simulacao.COLUNA_B.ToString("###,###,##0.00");
                        }
                        Literal _litColunaC = e.Row.FindControl("litColunaC") as Literal;
                        if (_litColunaC != null)
                        {
                            if (coluna == 1) // MARKUP
                                _litColunaC.Text = _simulacao.COLUNA_C.ToString("0.0#");
                            if (coluna == 2) // LUCRO
                                _litColunaC.Text = _simulacao.COLUNA_C.ToString("##0.00").Replace(",00", "") + "%";
                            if (coluna == 3) // PREÇO
                                _litColunaC.Text = "R$ " + _simulacao.COLUNA_C.ToString("###,###,##0.00");
                        }
                        Literal _litColunaD = e.Row.FindControl("litColunaD") as Literal;
                        if (_litColunaD != null)
                        {
                            if (coluna == 1) // MARKUP
                                _litColunaD.Text = _simulacao.COLUNA_D.ToString("0.0#");
                            if (coluna == 2) // LUCRO
                                _litColunaD.Text = _simulacao.COLUNA_D.ToString("##0.00").Replace(",00", "") + "%";
                            if (coluna == 3) // PREÇO
                                _litColunaD.Text = "R$ " + _simulacao.COLUNA_D.ToString("###,###,##0.00");
                        }
                        Literal _litColunaE = e.Row.FindControl("litColunaE") as Literal;
                        if (_litColunaE != null)
                        {
                            if (coluna == 1) // MARKUP
                                _litColunaE.Text = _simulacao.COLUNA_E.ToString("0.0#");
                            if (coluna == 2) // LUCRO
                                _litColunaE.Text = _simulacao.COLUNA_E.ToString("##0.00").Replace(",00", "") + "%";
                            if (coluna == 3) // PREÇO
                                _litColunaE.Text = "R$ " + _simulacao.COLUNA_E.ToString("###,###,##0.00");
                        }
                    }
                }
            }
        }

        protected void btDesaprovarCusto_Click(object sender, EventArgs e)
        {
            try
            {
                string produto = txtProduto.Text;
                char mostruario = 'N';

                var custoTerceiro = prodController.ObterCustoTerceiro(produto.Trim(), mostruario);
                prodController.ExcluirCustoTerceiro(custoTerceiro.CODIGO);

                labProduto.Text = txtProduto.Text.Trim();
                labMensagem.Text = "PREÇO DESAPROVADO COM SUCESSO.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, 'Aprovação Preço Terceiro': function () { window.open('prod_cad_produto_custo_aprovterceiro.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
