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
    public partial class prod_cad_produto_custo_simulacao_novo : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        ProdutoCusto pc = new ProdutoCusto();
        int coluna = 0;
        int colunaReal = 0;
        decimal totalCMV = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                GerarGrids();
            }

            CalcularCustosVariavelCol("");

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btValidar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btValidar, null) + ";");
        }

        private void GerarGrids()
        {
            var p = prodController.ObterCustoCodigoOrigem().Take(6);

            gvTecido.DataSource = p;
            gvTecido.DataBind();

            gvAviamento.DataSource = p;
            gvAviamento.DataBind();

            gvServico.DataSource = p;
            gvServico.DataBind();
        }

        #region "AÇÕES"
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            decimal vTecido = 0;
            decimal vAviamento = 0;
            decimal vServico = 0;

            hidTecido.Value = "0";
            hidAviamento.Value = "0";
            hidServico.Value = "0";

            foreach (GridViewRow r in gvTecido.Rows)
            {
                var txt = ((TextBox)r.FindControl("valor")).Text;
                var qtde = ((TextBox)r.FindControl("qtde")).Text;
                if (txt != "")
                    vTecido += (Convert.ToDecimal(txt) * Convert.ToDecimal(qtde));
            }
            foreach (GridViewRow r in gvAviamento.Rows)
            {
                var txt = ((TextBox)r.FindControl("valor")).Text;
                var qtde = ((TextBox)r.FindControl("qtde")).Text;
                if (txt != "")
                    vAviamento += (Convert.ToDecimal(txt) * Convert.ToDecimal(qtde));
            }
            foreach (GridViewRow r in gvServico.Rows)
            {
                var txt = ((TextBox)r.FindControl("valor")).Text;
                if (txt != "")
                    vServico += Convert.ToDecimal(txt);
            }

            var vEtiqueta = (txtValorEtiqueta.Text == "") ? 0 : Convert.ToDecimal(txtValorEtiqueta.Text);
            var vTAG = (txtTAG.Text == "") ? 0 : Convert.ToDecimal(txtTAG.Text);

            var vOperacional = vServico + (vServico * Convert.ToDecimal(txtOperacionalPorc.Text.Replace("%", "")) / 100.00M);

            decimal custoUnidade = vTecido + vAviamento + vServico + vOperacional;
            var vMargem = (custoUnidade / 0.9500M) - custoUnidade;

            decimal custoUnidadeMargem = custoUnidade + vMargem;
            var vImposto = (custoUnidadeMargem / 0.9075M) - custoUnidadeMargem;

            List<PRODUTO_CUSTO_SIMULACAO> lstProdutoCusto = new List<PRODUTO_CUSTO_SIMULACAO>();
            //Primeiro realizar a simulação de todos os MARKUPS informado: (3, 3.5, 4, 4.5, 5)
            decimal mkup = Convert.ToDecimal(3.0);

            List<PRODUTO_CUSTO_SIMULACAO> lstCustoSimulacao = new List<PRODUTO_CUSTO_SIMULACAO>();
            PRODUTO_CUSTO_SIMULACAO novoCusto = null;

            var custoTX = vTecido + vAviamento + vServico + vOperacional + vEtiqueta + vTAG + vMargem + vImposto;
            for (int i = 0; i < 5; i++)
            {
                novoCusto = new PRODUTO_CUSTO_SIMULACAO();

                novoCusto = pc.CalcularCustoProduto27(custoTX, mkup, 0);

                lstProdutoCusto.Add(novoCusto);

                mkup += Convert.ToDecimal(0.5);
            }

            CarregarCustoSimulacao(lstProdutoCusto);

            hidTecido.Value = vTecido.ToString();
            hidAviamento.Value = vAviamento.ToString();
            hidServico.Value = vServico.ToString();
            hidOperacional.Value = vOperacional.ToString();
            hidEtiqueta.Value = vEtiqueta.ToString();
            hidTAG.Value = vTAG.ToString();
            hidMargem.Value = vMargem.ToString();
            hidImposto.Value = vImposto.ToString();

            CarregarItens(vTecido, vAviamento, vServico, vOperacional, vEtiqueta, vTAG, vMargem, vImposto);
        }

        private void CarregarItens(decimal vTecido, decimal vAviamento, decimal vServico, decimal vOperacional, decimal vEtiqueta, decimal vTAG, decimal vMargem, decimal vImposto)
        {

            var cmvX = new List<PRODUTO_CUSTO_ORIGEM>();

            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Tecido", CUSTO_TOTAL = vTecido });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Aviamento", CUSTO_TOTAL = vAviamento });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Serviço/Facção", CUSTO_TOTAL = vServico });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Operacional", CUSTO_TOTAL = vOperacional });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Etiqueta", CUSTO_TOTAL = vEtiqueta });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "TAG", CUSTO_TOTAL = vTAG });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Margem", CUSTO_TOTAL = vMargem });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Imposto", CUSTO_TOTAL = vImposto });

            gvTotalItem.DataSource = cmvX.OrderBy(p => p.PRODUTO);
            gvTotalItem.DataBind();
        }

        private void CarregarCustoSimulacao(List<PRODUTO_CUSTO_SIMULACAO> lstCustoSimulacao)
        {

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
                            _litDesc.Text = _custo.PRODUTO;

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

        #endregion

        #region "APROVACAO"
        //PRECO REAL DE CUSTO
        private PRODUTO_CUSTO_SIMULACAO CalcularPrecoAprovado()
        {

            decimal custoTX = Convert.ToDecimal(hidTecido.Value) +
                                Convert.ToDecimal(hidAviamento.Value) +
                                Convert.ToDecimal(hidServico.Value) +
                                Convert.ToDecimal(hidOperacional.Value) +
                                Convert.ToDecimal(hidEtiqueta.Value) +
                                Convert.ToDecimal(hidTAG.Value) +
                                Convert.ToDecimal(hidMargem.Value) +
                                Convert.ToDecimal(hidImposto.Value);

            var precoAprovado = pc.CalcularCustoProduto27(custoTX, 0, Convert.ToDecimal(txtPrecoAprovado.Text.Trim()));

            return precoAprovado;
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
        protected void btValidar_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtPrecoAprovado.Text.Trim() == "" || Convert.ToDecimal(txtPrecoAprovado.Text.Trim()) <= 0)
                {
                    return;
                }

                var precoAprovado = CalcularPrecoAprovado();

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

                    //CarregarItens(Convert.ToDecimal(hidTecido.Value), Convert.ToDecimal(hidAviamento.Value), Convert.ToDecimal(hidServico.Value), Convert.ToDecimal(hidOperacional.Value), Convert.ToDecimal(hidEtiqueta.Value), 0);
                }
            }
            catch (Exception ex)
            {
                divGridReal.Visible = false;
            }
        }

        #endregion

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            Response.Redirect("prod_cad_produto_custo_simulacao_novo.aspx");
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

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }

        private void CalcularCustosVariavelCol(string colecao)
        {
            //var varColecao = prodController.ObterCustoVariavelPorColecao(colecao);

            //if (varColecao == null)
            //    return;

            txtValorEtiqueta.Text = "0,26";
            txtTAG.Text = "1,00";
            txtOperacionalPorc.Text = "42,00%";

        }

        #region "COPIAR"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                hidCodigoHB.Value = "";
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


                PROD_HB produto = ObterProduto();

                if (produto == null)
                    throw new Exception("PRODUTO NÃO POSSUI HB.");


                PROD_HB_ROTA rotaHB = null;
                if (produto.ORDEM_PRODUCAO != null)
                {
                    rotaHB = prodController.ObterRotaOP(produto.ORDEM_PRODUCAO);
                    //VERIFICAR ROTAS
                    if (rotaHB == null)
                        throw new Exception("HB " + produto.HB.ToString() + " não possui ROTA. Entre em contato com TI.");
                }
                else
                {
                    throw new Exception("HB " + produto.HB.ToString() + " não possui OP. Entre em contato com TI.");
                }

                CarregarTecidos(produto);
                CarregarAviamentos(produto);
                CarregarServicos(produto);

                hidCodigoHB.Value = produto.CODIGO.ToString();
                btBuscar.Enabled = false;

            }
            catch (Exception ex)
            {
                labErro.Text = "" + ex.Message;
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
                    if (det != null)
                        lstTecidos.Add(det);

                int i = 0;
                foreach (GridViewRow r in gvTecido.Rows)
                {
                    var txt = ((TextBox)r.FindControl("txt"));
                    var qtde = ((TextBox)r.FindControl("qtde"));
                    var valor = ((TextBox)r.FindControl("valor"));

                    if (i < lstTecidos.Count())
                    {

                        txt.Text = lstTecidos[i].TECIDO;
                        valor.Text = lstTecidos[i].CUSTO_TECIDO.ToString();

                        decimal gastoPorCorte = 0;
                        decimal consumoTecidoPreco = 0;
                        int totalGrade = 0;
                        totalGrade = prodController.ObterQtdeGradeHB(lstTecidos[i].CODIGO, 3);
                        if (lstTecidos[i].GASTO_PECA_CUSTO == null)
                        {
                            gastoPorCorte = Convert.ToDecimal(lstTecidos[i].GASTO_FOLHA * totalGrade);
                            consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte + lstTecidos[i].RETALHOS) / totalGrade));
                        }
                        else
                        {
                            gastoPorCorte = Convert.ToDecimal(lstTecidos[i].GASTO_PECA_CUSTO * totalGrade);
                            consumoTecidoPreco = (totalGrade <= 0) ? 0 : Convert.ToDecimal(((gastoPorCorte) / totalGrade));
                        }

                        qtde.Text = consumoTecidoPreco.ToString();
                    }

                    i = i + 1;

                }

            }
        }
        private void CarregarAviamentos(PROD_HB produto)
        {
            if (produto != null)
            {
                var lstAviamentos = prodController.ObterAviamentoCusto(produto.CODIGO);

                int i = 0;


                foreach (GridViewRow r in gvAviamento.Rows)
                {
                    var txt = ((TextBox)r.FindControl("txt"));
                    var qtde = ((TextBox)r.FindControl("qtde"));
                    var valor = ((TextBox)r.FindControl("valor"));

                    if (i < lstAviamentos.Count())
                    {
                        txt.Text = lstAviamentos[i].AVIAMENTO;
                        valor.Text = lstAviamentos[i].PRECO.ToString();
                        qtde.Text = (lstAviamentos[i].CONSUMO == null) ? "0" : lstAviamentos[i].CONSUMO.ToString();
                    }

                    i = i + 1;
                }

            }
        }
        private void CarregarServicos(PROD_HB produto)
        {
            List<PROD_HB_CUSTO_SERVICO> lstServicosFinal = new List<PROD_HB_CUSTO_SERVICO>();
            List<PROD_HB_CUSTO_SERVICO> lstServicos = new List<PROD_HB_CUSTO_SERVICO>();
            lstServicos = prodController.ObterCustoServico().Where(p => p.PRODUTO.Trim() == produto.CODIGO_PRODUTO_LINX.ToString() && p.MOSTRUARIO == produto.MOSTRUARIO).ToList();

            char mostruario = Convert.ToChar(produto.MOSTRUARIO);

            //obter rota do produto
            var rotaHB = prodController.ObterRotaHB(produto.CODIGO);
            int i = 1;
            if (rotaHB != null)
            {

                for (; i <= 4; i++)
                {
                    var f = lstServicos.Where(p => p.SERVICO == ((p.MOSTRUARIO == 'S' && i == 1) ? 6 : i)).FirstOrDefault();
                    if (f != null)
                        lstServicosFinal.Add(f);
                    else if ((rotaHB.FACCAO && (i == 1 || i == 6)) || (rotaHB.ESTAMPARIA && i == 2) || (rotaHB.LAVANDERIA && i == 3))
                        //else if (rotaHB.FACCAO && (i == 1 || i == 6))
                        lstServicosFinal.Add(new PROD_HB_CUSTO_SERVICO
                        {
                            CODIGO = 0,
                            MOSTRUARIO = mostruario,
                            CUSTO_PECA = ((mostruario == 'S' && i == 1) ? produto.PRECO_FACC_MOSTRUARIO : 0),
                            SERVICO = ((mostruario == 'S' && i == 1) ? 6 : i)
                        });
                }

                for (i = lstServicosFinal.Count; i < 6; i++)
                    lstServicosFinal.Add(new PROD_HB_CUSTO_SERVICO { CODIGO = 0, MOSTRUARIO = mostruario });
            }

            i = 0;
            foreach (GridViewRow r in gvServico.Rows)
            {
                var txt = ((TextBox)r.FindControl("txt"));
                var valor = ((TextBox)r.FindControl("valor"));

                if (i < lstServicosFinal.Count())
                {
                    if (lstServicosFinal[i].SERVICO > 0)
                    {
                        txt.Text = prodController.ObterServicoProducao(lstServicosFinal[i].SERVICO).DESCRICAO;
                        valor.Text = lstServicosFinal[i].CUSTO_PECA.ToString();
                    }
                }

                i = i + 1;
            }

        }

        #endregion


    }
}
