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
    public partial class desenv_fila_tagavi_baixa : System.Web.UI.Page
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

                var pedido = "";
                var produto = "";
                var cor = "";
                var entrega = DateTime.Now;
                var eti = "";

                pedido = Request.QueryString["p"].ToString();
                produto = Request.QueryString["prod"].ToString();
                cor = Request.QueryString["c"].ToString();
                entrega = Convert.ToDateTime(Request.QueryString["ent"].ToString());
                eti = Request.QueryString["eti"].ToString();

                var produtoLinx = baseController.BuscaProduto(produto);
                var grupoProduto = produtoLinx.GRUPO_PRODUTO.Trim();
                var nomeProduto = produtoLinx.DESC_PRODUTO.Trim();

                hidPedido.Value = pedido;
                hidProduto.Value = produto;
                hidCor.Value = cor;
                hidEntrega.Value = entrega.ToString("yyyy-MM-dd");
                hidEti.Value = eti;

                if (eti == "a")
                    labTitulo.Text = "Baixar Aviamento";
                else if (eti == "t")
                    labTitulo.Text = "Baixar TAG";

                labProduto.Text = produto;
                labTipo.Text = (tipo == "00") ? "CORTE" : ((tipo == "01") ? "PRODUTO ACABADO" : "ACESSÓRIO");
                labNome.Text = nomeProduto;
                labCor.Text = cor + " - " + prodController.ObterCoresBasicas(cor).DESC_COR;

                List<PROD_HB_GRADE> grade = new List<PROD_HB_GRADE>();

                CarregarControleGrade(baseController.ObterProdutoTamanho(produto));

                grade.Add(CarregarGrade(pedido, produto, cor, entrega));
                gvGrade.DataSource = grade;
                gvGrade.DataBind();
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

            var comprasProdutoPre = desenvController.ObterComprasProdutoPrePedido(pedido, produto, corProduto, entrega);

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

                if (hidEti.Value == "a")
                {
                    comprasProdutoPre.DATA_BAIXA_AVIAMENTO = DateTime.Now;
                    comprasProdutoPre.USUARIO_BAIXA_AVIAMENTO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                }
                else
                {
                    comprasProdutoPre.DATA_BAIXA_TAG = DateTime.Now;
                    comprasProdutoPre.USUARIO_BAIXA_TAG = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                }

                desenvController.AtualizarComprasProdutoPrePedido(comprasProdutoPre);



                labErro.Text = ((hidEti.Value == "a") ? "Aviamento" : "TAG") + " baixado com sucesso.";
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
