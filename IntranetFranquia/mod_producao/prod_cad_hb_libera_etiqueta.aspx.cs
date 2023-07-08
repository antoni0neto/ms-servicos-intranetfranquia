using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Relatorios
{
    public partial class prod_cad_hb_libera_etiqueta : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        PROD_GRADE gradeNome = new PROD_GRADE();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                var tipo = Request.QueryString["t"].ToString();

                var codigoHB = 0;
                var pedido = "";
                var produto = "";
                var cor = "";
                var entrega = DateTime.Now;
                var grupoProduto = "";
                var nomeProduto = "";
                var mostruario = "";
                int? codigoGrade = null;

                if (tipo == "00")
                {
                    codigoHB = Convert.ToInt32(Request.QueryString["chb"].ToString());
                }
                else
                {
                    pedido = Request.QueryString["p"].ToString();
                    produto = Request.QueryString["prod"].ToString();
                    cor = Request.QueryString["c"].ToString();
                    entrega = Convert.ToDateTime(Request.QueryString["ent"].ToString());
                }


                if (codigoHB > 0)
                {
                    PROD_HB prod_hb = prodController.ObterHB(codigoHB);
                    if (prod_hb != null)
                    {
                        hidCodigoHB.Value = codigoHB.ToString();

                        produto = prod_hb.CODIGO_PRODUTO_LINX;
                        cor = prod_hb.COR;
                        grupoProduto = prod_hb.GRUPO;
                        nomeProduto = prod_hb.NOME;
                        mostruario = prod_hb.MOSTRUARIO.ToString();
                        codigoGrade = Convert.ToInt32(prod_hb.PROD_GRADE);

                        if (prod_hb.DATA_BAIXA_ETIQUETA != null)
                        {
                            Response.Write("ETIQUETA DESTE PRODUTO JÁ FOI BAIXADA.");
                            Response.End();
                        }
                    }
                }
                else
                {
                    var produtoLinx = baseController.BuscaProduto(produto);
                    grupoProduto = produtoLinx.GRUPO_PRODUTO.Trim();
                    nomeProduto = produtoLinx.DESC_PRODUTO.Trim();
                    mostruario = "N";
                    codigoGrade = 0;

                    hidPedido.Value = pedido;
                    hidProduto.Value = produto;
                    hidCor.Value = cor;
                    hidEntrega.Value = entrega.ToString("yyyy-MM-dd");
                }



                labProduto.Text = produto;
                labTipo.Text = (tipo == "00") ? "CORTE" : ((tipo == "01") ? "PRODUTO ACABADO" : "ACESSÓRIO");
                labNome.Text = nomeProduto;
                labCor.Text = cor + " - " + prodController.ObterCoresBasicas(cor).DESC_COR;



                var custoTC = prodController.ObterCustoOrigem(produto, Convert.ToChar(mostruario), 'N').Where(p =>
                                                                                            p.COD_CUSTO_ORIGEM == 1 ||
                                                                                            p.COD_CUSTO_ORIGEM == 2 ||
                                                                                            p.COD_CUSTO_ORIGEM == 3 ||
                                                                                            p.COD_CUSTO_ORIGEM == 4 ||
                                                                                            p.COD_CUSTO_ORIGEM == 5 ||
                                                                                            p.COD_CUSTO_ORIGEM == 6 ||
                                                                                            p.COD_CUSTO_ORIGEM == 8);

                if ((custoTC != null) && (custoTC.Count() > 0) && (custoTC.Where(p => p.COD_CUSTO_ORIGEM == 5).Count() > 0) && mostruario == "N")
                {
                    decimal custo = custoTC.Sum(p => p.CUSTO_TOTAL).Value;
                    labCustoTC.Text = "R$ " + custo.ToString("###,###,###,##0.00");
                }
                else
                {
                    labCustoTC.Text = "Não Informado";
                }

                var precoTO = baseController.BuscaPrecoProduto(produto, "TO");
                if (precoTO != null && precoTO.Count() > 0)
                    labPrecoTO.Text = "R$ " + precoTO[0].PRECO1;
                else
                    labPrecoTO.Text = "Não Informado";

                var precoTL = baseController.BuscaPrecoProduto(produto, "TL");
                if (precoTL != null && precoTL.Count() > 0)
                    labPrecoTL.Text = "R$ " + precoTL[0].PRECO1;
                else
                    labPrecoTL.Text = "Não Informado";

                var precoSC = baseController.BuscaPrecoProduto(produto, "SC");
                if (precoSC != null && precoSC.Count() > 0)
                    labPrecoSC.Text = "R$ " + precoSC[0].PRECO1;
                else
                    labPrecoSC.Text = "Não Informado";

                var precoTD = baseController.BuscaPrecoProduto(produto, "TD");
                if (precoTD != null && precoTD.Count() > 0)
                    labPrecoTD.Text = "R$ " + precoTD[0].PRECO1;
                else
                    labPrecoTD.Text = "Não Informado";

                var precoTN = baseController.BuscaPrecoProduto(produto, "TN");
                if (precoTN != null && precoTN.Count() > 0)
                    labPrecoTN.Text = "R$ " + precoTN[0].PRECO1;
                else
                    labPrecoTN.Text = "Não Informado";

                var precoTA = baseController.BuscaPrecoProduto(produto, "TA");
                if (precoTA != null && precoTA.Count() > 0)
                    labPrecoTA.Text = "R$ " + precoTA[0].PRECO1;
                else
                    labPrecoTA.Text = "Não Informado";

                var precoTT = baseController.BuscaPrecoProduto(produto, "TT");
                if (precoTT != null && precoTT.Count() > 0)
                    labPrecoTT.Text = "R$ " + precoTT[0].PRECO1;
                else
                    labPrecoTT.Text = "Não Informado";

                var precoTS = baseController.BuscaPrecoProduto(produto, "TS");
                if (precoTS != null && precoTS.Count() > 0)
                    labPrecoTS.Text = "R$ " + precoTS[0].PRECO1;
                else
                    labPrecoTS.Text = "Não Informado";

                var precoTX = baseController.BuscaPrecoProduto(produto, "TX");
                if (precoTX != null && precoTX.Count() > 0)
                    labPrecoTX.Text = "R$ " + precoTX[0].PRECO1;
                else
                    labPrecoTX.Text = "Não Informado";


                List<PROD_HB_GRADE> _grade = new List<PROD_HB_GRADE>();
                if (codigoHB > 0) // SE CORTE
                {
                    var composicao = prodController.ObterComposicaoHB(codigoHB);
                    gvComposicao.DataSource = composicao;
                    gvComposicao.DataBind();

                    var grade = prodController.ObterGradeHB(codigoHB, 3);
                    if (grade != null)
                    {

                        if (codigoGrade != null)
                            gradeNome = prodController.ObterGradeNome(Convert.ToInt32(codigoGrade));

                        _grade.Add(grade);
                        gvGrade.DataSource = _grade;
                        gvGrade.DataBind();
                    }
                }
                else
                {
                    CarregarControleGrade(baseController.ObterProdutoTamanho(produto));

                    _grade.Add(CarregarGrade(pedido, produto, cor, entrega));
                    gvGrade.DataSource = _grade;
                    gvGrade.DataBind();
                }


            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            return retorno;
        }
        #endregion

        private void CarregarControleGrade(SP_OBTER_PRODUTO_TAMANHOResult tamProduto)
        {
            gradeNome.GRADE_EXP = tamProduto.TAMANHO_1;
            gradeNome.GRADE_XP = tamProduto.TAMANHO_2;
            gradeNome.GRADE_PP = tamProduto.TAMANHO_3;
            gradeNome.GRADE_P = tamProduto.TAMANHO_4;
            gradeNome.GRADE_M = tamProduto.TAMANHO_5;
            gradeNome.GRADE_G = tamProduto.TAMANHO_6;
            gradeNome.GRADE_GG = tamProduto.TAMANHO_7;

        }
        private PROD_HB_GRADE CarregarGrade(string pedido, string produto, string corProduto, DateTime entrega)
        {

            var grade = new PROD_HB_GRADE();

            var comprasProdutoPre = (new DesenvolvimentoController()).ObterComprasProdutoPrePedido(pedido, produto, corProduto, entrega);

            grade.GRADE_EXP = comprasProdutoPre.CO1;
            grade.GRADE_XP = comprasProdutoPre.CO2;
            grade.GRADE_PP = comprasProdutoPre.CO3;
            grade.GRADE_P = comprasProdutoPre.CO4;
            grade.GRADE_M = comprasProdutoPre.CO5;
            grade.GRADE_G = comprasProdutoPre.CO6;
            grade.GRADE_GG = comprasProdutoPre.CO7;

            return grade;
        }

        protected void gvGrade_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (gradeNome != null)
                {
                    e.Row.Cells[1].Text = gradeNome.GRADE_EXP;    //EXP
                    e.Row.Cells[2].Text = gradeNome.GRADE_XP;     //XP
                    e.Row.Cells[3].Text = gradeNome.GRADE_PP;     //PP
                    e.Row.Cells[4].Text = gradeNome.GRADE_P;      //P
                    e.Row.Cells[5].Text = gradeNome.GRADE_M;      //M
                    e.Row.Cells[6].Text = gradeNome.GRADE_G;      //G
                    e.Row.Cells[7].Text = gradeNome.GRADE_GG;     //GG
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_GRADE grade = e.Row.DataItem as PROD_HB_GRADE;

                    if (grade != null)
                    {

                        Literal litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (litTotal != null)
                            litTotal.Text = (grade.GRADE_EXP + grade.GRADE_XP + grade.GRADE_PP + grade.GRADE_P + grade.GRADE_M + grade.GRADE_G + grade.GRADE_GG).ToString();
                    }
                }
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //if (!ValidarCampos())
                //{
                //    labErro.Text = "Preencha os campos em vermelho corretamente.";
                //    return;
                //}

                if (hidCodigoHB.Value != "" && hidCodigoHB.Value != "0")
                {
                    int codigoHB = 0;

                    codigoHB = Convert.ToInt32(hidCodigoHB.Value);
                    var prod_hb = prodController.ObterHB(codigoHB);
                    if (prod_hb == null)
                    {
                        labErro.Text = "HB não encontrado. Entre em contato com TI.";
                        return;
                    }

                    prod_hb.DATA_BAIXA_ETIQUETA = DateTime.Now;
                    prod_hb.USUARIO_BAIXA_ETIQUETA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    prodController.AtualizarHB(prod_hb);
                }
                else
                {
                    var pedido = hidPedido.Value;
                    var produto = hidProduto.Value;
                    var cor = hidCor.Value;
                    var entrega = Convert.ToDateTime(hidEntrega.Value);


                    var comprasProdutoPre = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                    if (comprasProdutoPre == null)
                    {
                        labErro.Text = "Pedido do Produto não encontrado. Entre em contato com TI.";
                        return;
                    }

                    comprasProdutoPre.DATA_BAIXA_ETI_BARRA = DateTime.Now;
                    comprasProdutoPre.USUARIO_BAIXA_ETI_BARRA = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    desenvController.AtualizarComprasProdutoPrePedido(comprasProdutoPre);

                }

                labErro.Text = "Etiqueta de Preço baixada com sucesso.";
                labErro.Font.Bold = true;
                btSalvar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
    }
}
