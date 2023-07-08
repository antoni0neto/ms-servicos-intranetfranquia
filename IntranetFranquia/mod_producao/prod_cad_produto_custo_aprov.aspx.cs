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
    public partial class prod_cad_produto_custo_aprov : System.Web.UI.Page
    {

        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        ProdutoCusto pc = new ProdutoCusto();

        int colTecido = 0;
        int colAviamento = 0;
        int colServico = 0;

        decimal totalTecidoCusto = 0;
        decimal totalTecidoPreco = 0;
        decimal totalAviamento = 0;
        decimal totalServicoCusto = 0;
        decimal totalServicoPreco = 0;

        decimal totalCMV = 0;

        int coluna = 0, colunaReal = 0;

        int faccaoC = 0;
        int acabamentoC = 0;

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
                    chkMostruario.Checked = (mostruario == "S") ? true : false;
                    btBuscar_Click(null, null);
                }

                btDesaprovarCusto.Visible = false;
            }

            //Evitar duplo clique no botão
            btValidar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btValidar, null) + ";");
            btAprovar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAprovar, null) + ";");
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btDesaprovarCusto.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btDesaprovarCusto, null) + ";");
        }

        #region "INICIALIZAR DADOS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private PROD_HB ObterProduto()
        {
            PROD_HB produto = new PROD_HB();
            produto = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() && p.CODIGO_PRODUTO_LINX.Trim() == txtProdutoLinxFiltro.Text.Trim() && p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).Take(1).SingleOrDefault();
            if (produto == null)
                produto = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim() && p.NOME.Trim().ToUpper() == txtNomeFiltro.Text.Trim().ToUpper() && p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).Take(1).SingleOrDefault();

            return produto;
        }
        #endregion

        #region "AÇÕES"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                btDesaprovarCusto.Visible = false;
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
                if (!ValidarCodigoLinx())
                {
                    labErro.Text = "Produto informado INVÁLIDO para geração de CUSTO.";
                    return;
                }
                var colecao = ddlColecoes.SelectedValue.Trim();
                if (colecao == "27" || colecao == "28" || colecao == "29" || colecao == "30" || colecao == "31" || colecao == "32")
                {
                    labErro.Text = "Utilize esta tela para aprovar preços da Coleção 26-.";
                    return;
                }

                PROD_HB produto = ObterProduto();

                if (produto == null)
                    throw new Exception("PRODUTO NÃO POSSUI HB.");

                List<PRODUTO_CUSTO_SIMULACAO> simulacao = prodController.ObterCustoSimulacao(produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(produto.MOSTRUARIO)).Where(p => p.CUSTO_MOSTRUARIO == 'N').ToList();
                if (simulacao == null || simulacao.Count <= 0)
                    throw new Exception("PRODUTO NÃO POSSUI SIMULAÇÃO.");

                if (simulacao != null && simulacao.Count > 0 && simulacao[0].FINALIZADO == 'N')
                    throw new Exception("PRODUTO NÃO FOI FINALIZADO NA SIMULAÇÃO DE CUSTO.");

                var desenvProduto = desenvController.ObterProduto(produto.COLECAO, produto.CODIGO_PRODUTO_LINX, produto.COR);

                txtHB.Text = produto.HB.ToString();
                txtProduto.Text = produto.CODIGO_PRODUTO_LINX;
                txtGrupo.Text = produto.GRUPO.Trim();
                txtNome.Text = produto.NOME.Trim();
                txtGriffe.Text = (desenvProduto == null) ? "DesenvProduto Não Encontrado" : desenvProduto.GRIFFE;
                txtMostruario.Text = (produto.MOSTRUARIO == 'N') ? "Não" : "Sim";
                imgFotoPeca.ImageUrl = produto.FOTO_PECA;

                CarregarTecidos(produto);
                CarregarAviamentos(produto);
                CarregarServicos(produto);
                CarregarCustoSimulacao(produto);
                CarregarPrecos(produto);

                var precoAprovado = prodController.ObterCustoSimulacaoPreco(produto.CODIGO_PRODUTO_LINX.Trim(), 'N', 'N').OrderBy(x => x.DATA_INCLUSAO).ToList();

                if (precoAprovado != null && precoAprovado.Count() > 0)
                {
                    txtPrecoAprovado.Text = precoAprovado[precoAprovado.Count() - 1].PRECO_PRODUTO.ToString("###,###,##0.00");
                    btValidar_Click(null, null);
                }

                pnlCusto.Visible = true;
                Session["COLECAO"] = ddlColecoes.SelectedValue;

                //VALIDAR SE CUSTO JA EXISTE - NAO PODE CALCULAR NOVAMENTE SE O PRECO JA FOI APROVADO
                btValidar.Visible = true;
                btValidar.Enabled = true;
                btAprovar.Enabled = true;

                if (!ValidarProdutoPrecoAprovado(produto))
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
        //SIMULAÇÃO DE CUSTO
        private void CarregarCustoSimulacao(PROD_HB produto)
        {

            var lstCustoSimulacao = prodController.ObterCustoSimulacao(produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(produto.MOSTRUARIO)).Where(p => p.CUSTO_MOSTRUARIO == 'N').ToList();

            if (lstCustoSimulacao.Count > 0)
            {
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

                var CMV = prodController.ObterCustoOrigem(produto.CODIGO_PRODUTO_LINX.Trim(), Convert.ToChar(produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                                            p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 3 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                                            p.COD_CUSTO_ORIGEM == 6
                                                                                                                            );
                gvTotalItem.DataSource = CMV;
                gvTotalItem.DataBind();

            }
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
            char mostruario = 'N';
            if (txtMostruario.Text.Trim().ToUpper() == "SIM")
                mostruario = 'S';

            List<PRODUTO_CUSTO_ORIGEM> custoOrigem = new List<PRODUTO_CUSTO_ORIGEM>();
            custoOrigem = prodController.ObterCustoOrigem(txtProduto.Text.Trim(), mostruario, 'N');
            //vTecido + vAviamento + vServico + vOperacional + vEtiqueta;
            decimal CMV = Convert.ToDecimal(custoOrigem.Where(p => p.COD_CUSTO_ORIGEM == 1 ||
                                                                                            p.COD_CUSTO_ORIGEM == 2 ||
                                                                                            p.COD_CUSTO_ORIGEM == 3).Sum(d => d.CUSTO_TOTAL));
            return CMV;
        }
        private decimal CalcularOpeEtiqueta()
        {
            char mostruario = 'N';
            if (txtMostruario.Text.Trim().ToUpper() == "SIM")
                mostruario = 'S';

            List<PRODUTO_CUSTO_ORIGEM> custoOrigem = new List<PRODUTO_CUSTO_ORIGEM>();
            custoOrigem = prodController.ObterCustoOrigem(txtProduto.Text.Trim(), mostruario, 'N');
            //vTecido + vAviamento + vServico + vOperacional + vEtiqueta;
            decimal CMV = Convert.ToDecimal(custoOrigem.Where(p => p.COD_CUSTO_ORIGEM == 4 ||
                                                                                            p.COD_CUSTO_ORIGEM == 6).Sum(d => d.CUSTO_TOTAL));
            return CMV;
        }

        private void CarregarCustoReal(PROD_HB produto)
        {
            List<PRODUTO_CUSTO_SIMULACAO> lstCustoSimulacao = new List<PRODUTO_CUSTO_SIMULACAO>();
            lstCustoSimulacao = prodController.ObterCustoSimulacao().Where(p => p.PRODUTO.Trim() == produto.CODIGO_PRODUTO_LINX.Trim() && p.MOSTRUARIO == produto.MOSTRUARIO).ToList();

            if (lstCustoSimulacao.Count > 0)
            {
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
                });
                lstAUX.Add(new ProdutoCusto
                {
                    COLUNA_NOME = "Lucro",
                    COLUNA_A = lucro[0],
                });
                lstAUX.Add(new ProdutoCusto
                {
                    COLUNA_NOME = "Preço",
                    COLUNA_A = precoProduto[0],
                });

                gvCustoReal.DataSource = lstAUX;
                gvCustoReal.DataBind();
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
        private bool ValidarCodigoLinx()
        {
            PRODUTO _produto = null;
            string _codigoProduto = "";

            //CODIGO LINX EXISTE?
            _produto = (new BaseController().BuscaProduto(txtProdutoLinxFiltro.Text.Trim()));
            if (_produto == null)
            {
                var _produtoNome = (new BaseController().BuscaProdutosDescricao(txtNomeFiltro.Text.Trim().ToUpper()));
                if (_produtoNome == null || _produtoNome.Count() <= 0)
                    return false;

                _produto = _produtoNome[0];
            }

            //CODIGO LINX CATEGORIA = "01" ?
            if (_produto.COD_CATEGORIA != "01")
                return false;

            //CODIGO LINX INICIA COM NUMERO PAR? //PRODUTO NACIONAL
            _codigoProduto = _produto.PRODUTO1.Substring(0, 1);
            if (_codigoProduto != "" && Convert.ToInt32(_codigoProduto) > 0)
            {
                if ((Convert.ToInt32(_codigoProduto) % 2) != 0)
                    return false;
            }
            else
                return false;

            return true;
        }
        private bool ValidarProdutoPrecoAprovado(PROD_HB produto)
        {
            string codigoProduto = "";
            codigoProduto = produto.CODIGO_PRODUTO_LINX.Trim();

            if (codigoProduto != "")
            {
                try
                {
                    if (prodController.ObterCustoSimulacao(codigoProduto, Convert.ToChar(produto.MOSTRUARIO), 'N', 'N', 'S').Count() > 0)
                        return false;
                }
                catch (Exception)
                {
                }
            }

            return true;
        }
        private void LimparDataAtualizacaoTripa(string produto)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();

            var produtos = desenvController.ObterProduto().Where(p => p.MODELO.Trim() == produto.Trim() && p.STATUS == 'A');

            foreach (var p in produtos)
                desenvController.AtualizarProduto(p);
        }

        private PRODUTO_CUSTO_SIMULACAO CalcularPrecoAprovado()
        {
            ////Obter valores para calculo do custo
            var valoresCalculo = prodController.ObterCustoSimulacaoValoresCalculo();

            decimal CMV = CalcularCMV();
            decimal opeEti = CalcularOpeEtiqueta();

            var precoAprovado = pc.CalcularCustoProdutoX(CMV, 0, Convert.ToDecimal(txtPrecoAprovado.Text.Trim()), opeEti);
            precoAprovado = pc.CalcularLucro(precoAprovado, valoresCalculo);

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

                var precoAprovado = new PRODUTO_CUSTO_SIMULACAO();
                //if (colecao > 24)
                //    precoAprovado = CalcularPrecoAprovado25();
                //else
                precoAprovado = CalcularPrecoAprovado();

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
                if (txtMostruario.Text.Trim().ToUpper() == "SIM")
                    mostruario = 'S';

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

                var precoAprovado = new PRODUTO_CUSTO_SIMULACAO();
                //if (colecao > 24)
                //    precoAprovado = CalcularPrecoAprovado25();
                //else
                precoAprovado = CalcularPrecoAprovado();

                precoAprovado.PRODUTO = txtProduto.Text.Trim();
                precoAprovado.MOSTRUARIO = mostruario;
                precoAprovado.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                precoAprovado.DATA_INCLUSAO = DateTime.Now;
                precoAprovado.SIMULACAO = 'N';
                precoAprovado.CUSTO_MOSTRUARIO = 'N';
                precoAprovado.FINALIZADO = 'S';
                prodController.InserirCustoSimulacao(precoAprovado);

                // Inserir custo origem do imposto
                decimal CMV = CalcularCMV();

                //retirado 26/06/2018 -- solicitado por tata
                //InserirCustoOrigem(produto.CODIGO_PRODUTO_LINX.ToString(), produto.COLECAO.Trim(), 5, (precoAprovado.CUSTO_COM_IMPOSTO - CMV), mostruario, 'N'); //Tecido

                //Obter valores para calculo do custo
                var valoresCalculo = prodController.ObterCustoSimulacaoValoresCalculo();

                ///// SOMENTE INSERE QUQUANDO EH SIMULAAO
                if (produto.MOSTRUARIO == 'S') // && QUANDO FOR APROVACAO SIMULAÇÂO
                {
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
                    var precoTA = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TA");
                    if (precoTA == null || precoTA.Count <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TA", Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2));
                    else if (precoTA.Count == 1 && precoTA[0].PRECO1 <= 0)
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TA", Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2));

                    // TL = PRECO LOJA
                    var precoTL = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TL");
                    if (precoTL == null || precoTL.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);

                    // TR = PRECO REPRESENTANTE
                    var precoTR = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TR");
                    if (precoTR == null || precoTR.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TR", precoTRCalc);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TR", precoTRCalc);

                    // TP = TABELA PARA
                    var precoTP = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TP");
                    var precoValor = (Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2) + (Math.Round((precoAprovado.PRECO_PRODUTO / 2), 2) * (8.00M / 100.00M)));
                    if (precoTP == null || precoTP.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TP", Math.Round((precoValor), 0));
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TP", Math.Round((precoValor), 0));
                }

                // ATUALIZAR PRECO NO LINX - TABELA = PRODUTOS_PRECOS
                //ATUALIZAR SOMENTE QDO FOR CORTE
                if (produto.MOSTRUARIO == 'N')
                {
                    // TL = PRECO LOJA
                    var precoTL = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TL");
                    if (precoTL == null || precoTL.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TL", precoAprovado.PRECO_PRODUTO);

                    // TO = PRECO ORIGINAL
                    var precoTO = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TO");
                    if (precoTO == null || precoTO.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TO", precoAprovado.PRECO_PRODUTO);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TO", precoAprovado.PRECO_PRODUTO);

                    // APENAS CRIA, SE HOUVER ALTERACAO ENVIAR E-MAIL
                    // SC = SAO CAETANO
                    var precoSC = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "SC");
                    if (precoSC == null || precoSC.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "SC", precoAprovado.PRECO_PRODUTO);
                    else
                        enviarEmailProduto = true;

                    // TD = TABELA DESCONTO
                    var precoTD = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TD");
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

                    var custo = prodController.ObterCustoOrigem(produto.CODIGO_PRODUTO_LINX, Convert.ToChar(produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                p.COD_CUSTO_ORIGEM == 3 ||
                                                                                                p.COD_CUSTO_ORIGEM == 4 ||
                                                                                                p.COD_CUSTO_ORIGEM == 6);

                    // alterado 26/06/2018 -- solicitado por tata
                    // custo 1 = tecido, aviamento e serviço
                    var custo1 = prodController.ObterCustoOrigem(produto.CODIGO_PRODUTO_LINX, Convert.ToChar(produto.MOSTRUARIO), 'N').Where(p =>
                                                                                                p.COD_CUSTO_ORIGEM == 1 ||
                                                                                                p.COD_CUSTO_ORIGEM == 2 ||
                                                                                                p.COD_CUSTO_ORIGEM == 3);

                    // TX = CUSTO CMAX
                    var custoSoma = (custo.Sum(p => p.CUSTO_TOTAL).Value);
                    var custo1soma = (custo1.Sum(p => p.CUSTO_TOTAL).Value);

                    //decimal custoTX = Math.Round((custo1soma * 2.50M), 2);

                    // alterado dia 16/05 - Marcia solicitou aumento de 5%
                    decimal custoTX = Math.Round((custo1soma * 2.65M), 2);

                    var precoTX = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TX");
                    if (precoTX == null || precoTX.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TX", custoTX);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TX", custoTX);

                    var valCustoTM = ((custoTX) - (custoTX * 30.00M / 100.00M));
                    var precoTM = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TM");
                    if (precoTM == null || precoTM.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TM", valCustoTM);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TM", valCustoTM);

                    // TT = IGUAL TX
                    var precoTT = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TT");
                    if (precoTT == null || precoTT.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TT", custoTX);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TT", custoTX);

                    // TH = IGUAL TX
                    var precoTH = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TH");
                    if (precoTH == null || precoTH.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TH", custoTX);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TH", custoTX);

                    // alterado 26/06/2018 -- solicitado por tata
                    var precoTC = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TC");
                    if (precoTC == null || precoTC.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TC", custo1soma);
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TC", custo1soma);

                    // alterado 23/07/2018 -- solicitado por tata/marcia/fabio/reginaldo
                    // retirar 1,30 de acabamento
                    var precoTU = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TU");
                    if (precoTU == null || precoTU.Count() <= 0)
                        InserirPrecoProduto(txtProduto.Text.Trim(), "TU", (custo1soma));
                    else
                        AtualizarPrecoProduto(txtProduto.Text.Trim(), "TU", (custo1soma));

                    PRODUTO _produto = baseController.ObterProdutoLinx(txtProduto.Text.Trim());
                    if (_produto != null)
                    {
                        _produto.PRODUTO1 = txtProduto.Text.Trim();
                        _produto.CUSTO_REPOSICAO1 = (custo1soma);
                        baseController.AtualizarProdutoLinx(_produto);
                    }

                }

                /*LIMPAR DATA DE ATUALIZACAO DO PRECO DO PRODUTO NA TRIPA*/
                LimparDataAtualizacaoTripa(txtProduto.Text.Trim());

                //Enviar Email
                if (Constante.enviarEmail)
                {
                    EnviarEmail(produto);

                    if (enviarEmailProduto)
                        EnviarEmailAlteracao(produto);
                }

                labProduto.Text = txtProduto.Text.Trim();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, 'Custo/Preço': function () { window.open('prod_cad_produto_custo.aspx', '_self'); }, 'Aprovação Preço': function () { window.open('prod_cad_produto_custo_aprov.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErroValidacao.Text = ex.Message;
            }
        }
        #endregion

        #region "GRIDS"
        /*TECIDOS*/
        private void CarregarTecidos(PROD_HB produto)
        {
            List<PROD_HB> lstTecidos = new List<PROD_HB>();

            if (produto != null)
            {
                //Adiciona pai na lista
                lstTecidos.Add(produto);

                List<PROD_HB> tecidoDetalhe = new List<PROD_HB>();
                tecidoDetalhe = prodController.ObterDetalhesHB(produto.CODIGO);
                foreach (PROD_HB det in tecidoDetalhe)
                {
                    if (det != null)
                        lstTecidos.Add(det);
                }

                gvTecido.DataSource = lstTecidos;
                gvTecido.DataBind();

                var tecidoExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'T');

                if (tecidoExtra == null || tecidoExtra.Count() <= 0)
                    tecidoExtra.Insert(0, new PROD_HB_MATERIAL_EXTRA { CODIGO = 0 });

                gvTecidoNovo.DataSource = tecidoExtra;
                gvTecidoNovo.DataBind();
            }
        }
        protected void gvTecido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal precoTecido = 0;
            decimal consumoTecidoCusto = 0;
            decimal consumoTecidoPreco = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _tecido = e.Row.DataItem as PROD_HB;

                    colTecido += 1;
                    if (_tecido != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colTecido.ToString();

                        int totalGrade = 0;
                        totalGrade = prodController.ObterQtdeGradeHB(_tecido.CODIGO, 3);

                        Literal _litTecido = e.Row.FindControl("litTecido") as Literal;
                        if (_litTecido != null)
                            _litTecido.Text = _tecido.TECIDO.Trim() + " -  " + ((_tecido.UNIDADE_MEDIDA == null) ? "" : (_tecido.UNIDADE_MEDIDA1.DESCRICAO + "s"));

                        Literal _litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        if (_litDetalhe != null)
                            _litDetalhe.Text = (_tecido.PROD_DETALHE == null) ? "PRINCIPAL" : _tecido.PROD_DETALHE1.DESCRICAO.ToUpper();

                        Literal _grade = e.Row.FindControl("litGrade") as Literal;
                        if (_grade != null)
                            _grade.Text = totalGrade.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = prodController.ObterFornecedor().Where(p =>
                                                                                    p.FORNECEDOR.Trim().ToUpper() == _tecido.FORNECEDOR.Trim().ToUpper() &&
                                                                                    p.TIPO == 'T').SingleOrDefault().PORC_ICMS.ToString();

                        Literal _precoTecido = e.Row.FindControl("litPrecoTecido") as Literal;
                        if (_precoTecido != null)
                        {
                            precoTecido = Convert.ToDecimal(_tecido.CUSTO_TECIDO);
                            _precoTecido.Text = precoTecido.ToString("###,###,##0.00");
                        }

                        Literal _litConsumoCusto = e.Row.FindControl("litConsumoCusto") as Literal;
                        if (_litConsumoCusto != null)
                        {
                            decimal gastoPorCorte = 0;
                            gastoPorCorte = Convert.ToDecimal(_tecido.GASTO_FOLHA * totalGrade);
                            consumoTecidoCusto = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + _tecido.RETALHOS) / totalGrade));

                            _litConsumoCusto.Text = consumoTecidoCusto.ToString("###,###,##0.00");
                        }

                        Literal _litConsumoPreco = e.Row.FindControl("litConsumoPreco") as Literal;
                        if (_litConsumoPreco != null)
                        {
                            decimal gastoPorCorte = 0;
                            if (_tecido.GASTO_PECA_CUSTO == null)
                            {
                                gastoPorCorte = Convert.ToDecimal(_tecido.GASTO_FOLHA * totalGrade);
                                consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + _tecido.RETALHOS) / totalGrade));
                            }
                            else
                            {
                                gastoPorCorte = Convert.ToDecimal(_tecido.GASTO_PECA_CUSTO * totalGrade);
                                consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte) / totalGrade));
                            }

                            _litConsumoPreco.Text = consumoTecidoPreco.ToString("###,###,##0.00");
                        }

                        Literal _litTotalCusto = e.Row.FindControl("litTotalCusto") as Literal;
                        if (_litTotalCusto != null)
                        {
                            decimal total = 0;
                            total = (precoTecido * consumoTecidoCusto);

                            if (chkMostruario.Checked)
                                total = total + (total * Convert.ToDecimal(10.000 / 100.000));

                            _litTotalCusto.Text = total.ToString("###,###,##0.00");

                            //somatorio
                            totalTecidoCusto += total;
                        }

                        Literal _litTotalPreco = e.Row.FindControl("litTotalPreco") as Literal;
                        if (_litTotalPreco != null)
                        {
                            decimal total = 0;
                            total = (precoTecido * consumoTecidoPreco);

                            if (chkMostruario.Checked)
                                total = total + (total * Convert.ToDecimal(10.000 / 100.000));

                            _litTotalPreco.Text = total.ToString("###,###,##0.00");

                            //somatorio
                            totalTecidoPreco += total;
                        }
                    }
                }
            }
        }
        protected void gvTecido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTecido.FooterRow;
            if (footer != null)
            {

            }
        }
        protected void gvTecidoNovo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_MATERIAL_EXTRA _tecido = e.Row.DataItem as PROD_HB_MATERIAL_EXTRA;

                    colTecido += 1;
                    if (_tecido != null)
                    {

                        if (_tecido.CODIGO == 0)
                        {
                            e.Row.Visible = false;
                            return;
                        }

                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colTecido.ToString();

                        Literal _litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        if (_litDetalhe != null)
                            _litDetalhe.Text = "";

                        Literal _medida = e.Row.FindControl("litMedida") as Literal;
                        if (_medida != null)
                            _medida.Text = "";

                        int totalGrade = 0;
                        totalGrade = prodController.ObterQtdeGradeHB(_tecido.PROD_HB, 3);
                        Literal _grade = e.Row.FindControl("litGrade") as Literal;
                        if (_grade != null)
                            _grade.Text = totalGrade.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = prodController.ObterFornecedor().Where(p =>
                                                                                    p.FORNECEDOR.Trim().ToUpper() == _tecido.FORNECEDOR.Trim().ToUpper() &&
                                                                                    p.TIPO == 'T').SingleOrDefault().PORC_ICMS.ToString();

                        Literal _precoTecido = e.Row.FindControl("litPrecoTecido") as Literal;
                        if (_precoTecido != null)
                            _precoTecido.Text = _tecido.PRECO.ToString("###,###,##0.00");

                        Literal _litConsumoCusto = e.Row.FindControl("litConsumoCusto") as Literal;
                        if (_litConsumoCusto != null)
                            _litConsumoCusto.Text = _tecido.CONSUMO_CUSTO.ToString("###,###,##0.00");

                        Literal _litConsumoPreco = e.Row.FindControl("litConsumoPreco") as Literal;
                        if (_litConsumoPreco != null)
                            _litConsumoPreco.Text = _tecido.CONSUMO_PRECO.ToString("###,###,##0.00");

                        Literal _litTotalCusto = e.Row.FindControl("litTotalCusto") as Literal;
                        if (_litTotalCusto != null)
                            _litTotalCusto.Text = (_tecido.PRECO * _tecido.CONSUMO_CUSTO).ToString("###,###,##0.00");

                        Literal _litTotalPreco = e.Row.FindControl("litTotalPreco") as Literal;
                        if (_litTotalPreco != null)
                        {
                            decimal total = 0;
                            total = (_tecido.PRECO * _tecido.CONSUMO_PRECO);
                            _litTotalPreco.Text = total.ToString("###,###,##0.00");

                            //somatorio
                            totalTecidoPreco += total;
                        }
                    }
                }
            }
        }
        protected void gvTecidoNovo_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTecidoNovo.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[9].Text = totalTecidoPreco.ToString("###,###,###,##0.00");
            }
        }

        /*AVIAMENTOS*/
        private void CarregarAviamentos(PROD_HB produto)
        {
            List<SP_OBTER_HB_AVIAMENTO_CUSTOResult> lstAviamentos = new List<SP_OBTER_HB_AVIAMENTO_CUSTOResult>();

            if (produto != null)
            {
                lstAviamentos = prodController.ObterAviamentoCusto(produto.CODIGO);

                gvAviamentoNovo.ShowHeader = false;
                if (lstAviamentos == null || lstAviamentos.Count <= 0)
                    gvAviamentoNovo.ShowHeader = true;

                gvAviamento.DataSource = lstAviamentos;
                gvAviamento.DataBind();

                var aviamentoExtra = prodController.ObterMaterialExtra(produto.CODIGO, 'A');

                if (aviamentoExtra == null || aviamentoExtra.Count() <= 0)
                    aviamentoExtra.Insert(0, new PROD_HB_MATERIAL_EXTRA { CODIGO = 0 });

                gvAviamentoNovo.DataSource = aviamentoExtra;
                gvAviamentoNovo.DataBind();
            }
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_AVIAMENTO_CUSTOResult _aviamento = e.Row.DataItem as SP_OBTER_HB_AVIAMENTO_CUSTOResult;

                    colAviamento += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAviamento.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = _aviamento.PORC_ICMS.ToString();

                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                            _litPreco.Text = Convert.ToDecimal(_aviamento.PRECO).ToString("###,###,##0.00");

                        Literal _litConsumo = e.Row.FindControl("litConsumo") as Literal;
                        if (_litConsumo != null)
                            _litConsumo.Text = (_aviamento.CONSUMO == null) ? "0" : Convert.ToDecimal(_aviamento.CONSUMO).ToString("#########0.00");

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                        {
                            _litTotal.Text = ((Convert.ToDecimal(_aviamento.PRECO) * ((_aviamento.CONSUMO == null) ? 0 : Convert.ToDecimal(_aviamento.CONSUMO)))).ToString("#########0.00");
                            totalAviamento += (_litTotal.Text.Trim() != "") ? Convert.ToDecimal(_litTotal.Text) : 0;
                        }
                    }
                }
            }
        }
        protected void gvAviamento_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAviamento.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[6].Text = totalAviamento.ToString("###,###,###,##0.00");
            }
        }
        protected void gvAviamentoNovo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_MATERIAL_EXTRA _aviamento = e.Row.DataItem as PROD_HB_MATERIAL_EXTRA;

                    colAviamento += 1;
                    if (_aviamento != null)
                    {
                        if (_aviamento.CODIGO == 0)
                        {
                            e.Row.Visible = false;
                            return;
                        }

                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colAviamento.ToString();

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = prodController.ObterFornecedor().Where(p =>
                                                                                    p.FORNECEDOR.Trim().ToUpper() == _aviamento.FORNECEDOR.Trim().ToUpper() &&
                                                                                    p.TIPO == 'A').SingleOrDefault().PORC_ICMS.ToString();

                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                            _litPreco.Text = _aviamento.PRECO.ToString("#########0.00");

                        Literal _litConsumo = e.Row.FindControl("litConsumo") as Literal;
                        if (_litConsumo != null)
                            _litConsumo.Text = _aviamento.CONSUMO_PRECO.ToString("###,###,##0.00");

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                        {
                            decimal total = 0;
                            total = (_aviamento.PRECO * _aviamento.CONSUMO_PRECO);
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
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[6].Text = totalAviamento.ToString("###,###,###,##0.00");
            }
        }

        /*SERVICOS*/
        private void CarregarServicos(PROD_HB produto)
        {
            List<PROD_HB_CUSTO_SERVICO> lstServicos = new List<PROD_HB_CUSTO_SERVICO>();
            lstServicos = prodController.ObterCustoServico().Where(p => p.PRODUTO.Trim() == produto.CODIGO_PRODUTO_LINX.ToString() && p.MOSTRUARIO == produto.MOSTRUARIO).ToList();

            gvServico.DataSource = lstServicos;
            gvServico.DataBind();
        }
        protected void gvServico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_CUSTO_SERVICO _servico = e.Row.DataItem as PROD_HB_CUSTO_SERVICO;

                    colServico += 1;
                    if (_servico != null)
                    {

                        if (_servico.SERVICO == 1 || _servico.SERVICO == 6)
                            faccaoC += 1;
                        if (_servico.SERVICO == 4 || _servico.SERVICO == 6)
                            acabamentoC += 1;

                        if (((_servico.SERVICO == 1 && _servico.SERVICO == 6) && faccaoC > 1) || ((_servico.SERVICO == 4 && _servico.SERVICO == 6) && acabamentoC > 1))
                            e.Row.BackColor = Color.PeachPuff;

                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colServico.ToString();

                        Literal _litFornecedor = e.Row.FindControl("litFornecedor") as Literal;
                        if (_litFornecedor != null)
                            _litFornecedor.Text = _servico.FORNECEDOR;

                        Literal _litServico = e.Row.FindControl("litServico") as Literal;
                        if (_litServico != null)
                            if (_servico.SERVICO > 0)
                                _litServico.Text = prodController.ObterServicoProducao(_servico.SERVICO).DESCRICAO;

                        Literal _litValorServico = e.Row.FindControl("litValorServico") as Literal;
                        if (_litValorServico != null)
                        {
                            _litValorServico.Text = _servico.CUSTO.ToString("########0.00");
                            totalServicoCusto += _servico.CUSTO;
                        }

                        Literal _litValorPreco = e.Row.FindControl("litValorPreco") as Literal;
                        if (_litValorPreco != null)
                        {
                            _litValorPreco.Text = Convert.ToDecimal(_servico.CUSTO_PECA).ToString("########0.00");
                            totalServicoPreco += Convert.ToDecimal(_servico.CUSTO_PECA);
                        }
                    }
                }
            }
        }
        protected void gvServico_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvServico.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = totalServicoCusto.ToString("###,###,###,##0.00");
                footer.Cells[4].Text = totalServicoPreco.ToString("###,###,###,##0.00");
            }
        }

        #endregion

        protected void gvTotalItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PRODUTO_CUSTO_ORIGEM _custo = e.Row.DataItem as PRODUTO_CUSTO_ORIGEM;

                    if (_custo != null)
                    {
                        Literal _litDesc = e.Row.FindControl("litDesc") as Literal;
                        if (_litDesc != null)
                            _litDesc.Text = _custo.PRODUTO_CUSTO_COD_ORIGEM.DESCRICAO;

                        Literal _litValor = e.Row.FindControl("litValor") as Literal;
                        if (_litValor != null)
                        {
                            _litValor.Text = "R$ " + Convert.ToDecimal(_custo.CUSTO_TOTAL).ToString("###,###,##0.00");
                            totalCMV += Convert.ToDecimal(_custo.CUSTO_TOTAL);
                        }

                    }
                }
            }
        }
        protected void gvTotalItem_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTotalItem.FooterRow;
            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[1].Text = "R$ " + totalCMV.ToString("###,###,###,##0.00");
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        private void CarregarPrecos(PROD_HB produto)
        {
            var precoTO = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TO");
            if (precoTO != null && precoTO.Count() > 0)
                txtPrecoTO.Text = Convert.ToDecimal(precoTO[0].PRECO1).ToString("###,###,##0.00");
            else
                txtPrecoTO.Text = "";

            var precoTA = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TA");
            if (precoTA != null && precoTA.Count() > 0)
                txtPrecoTA.Text = Convert.ToDecimal(precoTA[0].PRECO1).ToString("###,###,##0.00");
            else
                txtPrecoTA.Text = "";

            var precoTL = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TL");
            if (precoTL != null && precoTL.Count() > 0)
                txtPrecoTL.Text = Convert.ToDecimal(precoTL[0].PRECO1).ToString("###,###,##0.00");
            else
                txtPrecoTL.Text = "";

        }

        protected void btDesaprovarCusto_Click(object sender, EventArgs e)
        {
            try
            {
                string produto = txtProduto.Text;
                char mostruario = (txtMostruario.Text.Trim().ToUpper() == "NÃO") ? 'N' : 'S';

                var produtoSimulacao = prodController.ObterCustoSimulacao(produto, mostruario).Where(p => p.SIMULACAO == 'N').FirstOrDefault();
                prodController.ExcluirCustoSimulacao(produtoSimulacao.CODIGO);

                labProduto.Text = txtProduto.Text.Trim();
                labMensagem.Text = "PREÇO DESAPROVADO COM SUCESSO.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('prod_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('prod_menu.aspx', '_self'); }, 'Custo/Preço': function () { window.open('prod_cad_produto_custo.aspx', '_self'); }, 'Aprovação Preço': function () { window.open('prod_cad_produto_custo_aprov.aspx', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


        private void EnviarEmail(PROD_HB produto)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Preço de Produto - " + produto.CODIGO_PRODUTO_LINX;
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
        private void EnviarEmailAlteracao(PROD_HB produto)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet: ALTERAÇÃO DE PREÇO - Produto: " + produto.CODIGO_PRODUTO_LINX;
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
        private string MontarCorpoEmail(PROD_HB produto, bool _alteracao)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(produto.PROD_GRADE));

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
            sb.Append("                        <h3>Preço de Produto " + ((!_alteracao) ? "Aprovado" : "ALTERADO") + " - " + ((produto.MOSTRUARIO == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO") + "</h3>");
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
            sb.Append("                                    " + produto.CODIGO_PRODUTO_LINX.Trim());
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
            sb.Append("                                    " + produto.GRUPO.Trim());
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
            sb.Append("                                    " + produto.NOME.Trim());
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
            sb.Append("                                    " + produto.COR.Trim() + " - " + prodController.ObterCoresBasicas(produto.COR.Trim()).DESC_COR);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            //var custoTC = prodController.ObterCustoOrigem(produto.CODIGO_PRODUTO_LINX, Convert.ToChar(produto.MOSTRUARIO), 'N').Where(p =>
            //                                                                            p.COD_CUSTO_ORIGEM == 1 ||
            //                                                                            p.COD_CUSTO_ORIGEM == 2 ||
            //                                                                            p.COD_CUSTO_ORIGEM == 3 ||
            //                                                                            p.COD_CUSTO_ORIGEM == 4 ||
            //                                                                            p.COD_CUSTO_ORIGEM == 5 ||
            //                                                                            p.COD_CUSTO_ORIGEM == 6);

            var custoTC = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TC");

            if (custoTC != null && custoTC.Count() > 0 && produto.MOSTRUARIO == 'N')
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Custo - TC:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    R$ " + custoTC[0].PRECO1);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }

            var precoTO = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TO");
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
            var precoSC = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "SC");
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

            var precoTL = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TL");
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

            var precoTD = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TD");
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

            var precoTN = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TN");
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

            var precoTX = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TX");
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

            var precoTA = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TA");
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

            var precoTT = baseController.BuscaPrecoProduto(produto.CODIGO_PRODUTO_LINX, "TT");
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

            var comp = prodController.ObterComposicaoHB(produto.CODIGO);

            if (comp != null && comp.Count() > 0)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Composição:");
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    " + comp[0].QTDE + "% " + comp[0].DESCRICAO);
                sb.Append("                                </td>");
                sb.Append("                                <td>");
                sb.Append("                                    &nbsp;");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                foreach (var c in comp.Where(p => p.CODIGO != comp[0].CODIGO))
                {
                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    &nbsp;");
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    " + c.QTDE + "% " + c.DESCRICAO);
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
            }
            sb.Append("                             <tr>");
            sb.Append("                                 <td colspan='3'>");
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

            var _grade = prodController.ObterGradeHB(produto.CODIGO, 3);
            sb.Append("                                         <tr>");
            sb.Append("                                             <td>");
            sb.Append("                                                 " + prodController.ObterCoresBasicas(produto.COR.Trim()).DESC_COR.Trim());
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



    }
}
