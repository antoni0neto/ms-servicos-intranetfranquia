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
    public partial class prod_cad_produto_custo_simulacao : System.Web.UI.Page
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
                gerarGrids();
            }

            //Evitar duplo clique no botão
            btValidar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btValidar, null) + ";");
        }

        private void gerarGrids()
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
            decimal vOperacional = 6.90M;
            decimal vEtiqueta = 0.26M;

            hidICMSCredito.Value = "0";
            hidTecido.Value = "0";
            hidAviamento.Value = "0";
            hidServico.Value = "0";
            hidOperacional.Value = "0";
            hidEtiqueta.Value = "0";

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

            //Calcular ICMS Credito
            //decimal icmsCredito = 0;
            //icmsCredito = ((vTecido * 12 / 100) + (vAviamento * 12 / 100));

            //Inserir Valores Custo de Simulação
            decimal CMV = 0;

            CMV = vTecido + vAviamento + vServico;

            //decimal custoRotativo = (CMV - icmsCredito);

            var valoresCalculo = prodController.ObterCustoSimulacaoValoresCalculo();

            List<PRODUTO_CUSTO_SIMULACAO> lstProdutoCusto = new List<PRODUTO_CUSTO_SIMULACAO>();
            //Primeiro realizar a simulação de todos os MARKUPS informado: (3, 3.5, 4, 4.5, 5)
            decimal mkup = Convert.ToDecimal(3.0);
            int codigo = 0;
            List<PRODUTO_CUSTO_SIMULACAO> lstCustoSimulacao = new List<PRODUTO_CUSTO_SIMULACAO>();
            PRODUTO_CUSTO_SIMULACAO novoCusto = null;
            for (int i = 0; i < 5; i++)
            {
                novoCusto = new PRODUTO_CUSTO_SIMULACAO();

                novoCusto = pc.CalcularCustoProdutoX(CMV, mkup, 0, (vOperacional + vEtiqueta));
                novoCusto = pc.CalcularLucro(novoCusto, valoresCalculo);
                novoCusto.CODIGO = codigo;
                lstProdutoCusto.Add(novoCusto);

                mkup += Convert.ToDecimal(0.5);
            }

            CarregarCustoSimulacao(lstProdutoCusto);

            //hidICMSCredito.Value = icmsCredito.ToString();
            hidTecido.Value = vTecido.ToString();
            hidAviamento.Value = vAviamento.ToString();
            hidServico.Value = vServico.ToString();
            hidOperacional.Value = vOperacional.ToString();
            hidEtiqueta.Value = vEtiqueta.ToString();

            CarregarItens(vTecido, vAviamento, vServico, vOperacional, vEtiqueta, 0);
        }

        private void CarregarItens(decimal vTecido, decimal vAviamento, decimal vServico, decimal vOperacional, decimal vEtiqueta, decimal vImpostoFabrica)
        {

            var cmvX = new List<PRODUTO_CUSTO_ORIGEM>();

            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Tecido", CUSTO_TOTAL = vTecido });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Aviamento", CUSTO_TOTAL = vAviamento });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Serviço", CUSTO_TOTAL = vServico });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Operacional", CUSTO_TOTAL = vOperacional });
            cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Etiqueta", CUSTO_TOTAL = vEtiqueta });
            //cmvX.Add(new PRODUTO_CUSTO_ORIGEM { PRODUTO = "Imp Fábrica", CUSTO_TOTAL = vImpostoFabrica });

            gvTotalItem.DataSource = cmvX;
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
            //Obter valores para calculo do custo
            var valoresCalculo = prodController.ObterCustoSimulacaoValoresCalculo();

            decimal CMV = CalcularCMV();
            decimal opeEti = Convert.ToDecimal(hidEtiqueta.Value) + Convert.ToDecimal(hidOperacional.Value);

            var precoAprovado = pc.CalcularCustoProdutoX(CMV, 0, Convert.ToDecimal(txtPrecoAprovado.Text.Trim()), opeEti);
            //precoAprovado = pc.CalcularLucro(precoAprovado, valoresCalculo);

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

                    CarregarItens(Convert.ToDecimal(hidTecido.Value), Convert.ToDecimal(hidAviamento.Value), Convert.ToDecimal(hidServico.Value), Convert.ToDecimal(hidOperacional.Value), Convert.ToDecimal(hidEtiqueta.Value), 0);
                }
            }
            catch (Exception ex)
            {
                divGridReal.Visible = false;
            }
        }

        private decimal CalcularCMV()
        {

            //vTecido + vAviamento + vServico + vOperacional + vEtiqueta;
            decimal CMV = (Convert.ToDecimal(hidTecido.Value) + Convert.ToDecimal(hidAviamento.Value) + Convert.ToDecimal(hidServico.Value));
            return CMV;
        }

        #endregion

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            Response.Redirect("prod_cad_produto_custo_simulacao.aspx");
        }





    }
}
