using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class pacab_entrada_produto_baixar : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        PROD_GRADE gradeNome = new PROD_GRADE();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                string produto = "";
                string cor = "";
                if (
                        Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                        Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                        Session["USUARIO"] == null
                    )
                    Response.Redirect("pacab_menu.aspx");

                produto = Request.QueryString["p"].ToString();
                cor = Request.QueryString["c"].ToString();

                var produtoLinx = baseController.BuscaProduto(produto);
                if (produtoLinx == null)
                    Response.Redirect("pacab_menu.aspx");

                hidColecao.Value = produtoLinx.COLECAO.Trim();
                hidProduto.Value = produto.Trim();
                hidCor.Value = cor.Trim();

                labColecao.Text = baseController.BuscaColecaoAtual(produtoLinx.COLECAO.Trim()).DESC_COLECAO;
                labProduto.Text = produto;
                labNome.Text = produtoLinx.DESC_PRODUTO;
                labCor.Text = prodController.ObterCoresBasicas(cor).DESC_COR.Trim();

                CarregarGradeCompra(hidColecao.Value, produto, cor);
                CarregarGradeVendaAtacado(hidColecao.Value, produto, cor);

                labGrade01.Text = gradeNome.GRADE_EXP;
                txtGradeEXP_R.Enabled = (gradeNome.GRADE_EXP == "") ? false : true;
                labGrade02.Text = gradeNome.GRADE_XP;
                txtGradeXP_R.Enabled = (gradeNome.GRADE_XP == "") ? false : true;
                labGrade03.Text = gradeNome.GRADE_PP;
                txtGradePP_R.Enabled = (gradeNome.GRADE_PP == "") ? false : true;
                labGrade04.Text = gradeNome.GRADE_P;
                txtGradeP_R.Enabled = (gradeNome.GRADE_P == "") ? false : true;
                labGrade05.Text = gradeNome.GRADE_M;
                txtGradeM_R.Enabled = (gradeNome.GRADE_M == "") ? false : true;
                labGrade06.Text = gradeNome.GRADE_G;
                txtGradeG_R.Enabled = (gradeNome.GRADE_G == "") ? false : true;
                labGrade07.Text = gradeNome.GRADE_GG;
                txtGradeGG_R.Enabled = (gradeNome.GRADE_GG == "") ? false : true;

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

            labTituloGradeNova.ForeColor = _OK;
            if (txtGradeEXP_R.Text == "" ||
                txtGradeXP_R.Text == "" ||
                txtGradePP_R.Text == "" ||
                txtGradeP_R.Text == "" ||
                txtGradeM_R.Text == "" ||
                txtGradeG_R.Text == "" ||
                txtGradeGG_R.Text == "")
            {
                labTituloGradeNova.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "VENDA ATACADO"
        private void CarregarGradeVendaAtacado(string colecao, string produto, string cor)
        {
            var _gradeVendaAtacado = new List<SP_OBTER_VENDA_ATACADO_PRODUTOResult>();
            var gradeVendaAtacado = desenvController.ObterVendaAtacadoProduto(colecao.Trim(), produto.Trim(), cor.Trim()).FirstOrDefault();
            if (gradeVendaAtacado != null)
            {
                _gradeVendaAtacado.Add(gradeVendaAtacado);
                gvGradeVendaAtacado.DataSource = _gradeVendaAtacado;
                gvGradeVendaAtacado.DataBind();
            }
        }
        protected void gvGradeVendaAtacado_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    SP_OBTER_VENDA_ATACADO_PRODUTOResult _gradeVendaAtacado = e.Row.DataItem as SP_OBTER_VENDA_ATACADO_PRODUTOResult;

                    if (_gradeVendaAtacado != null)
                    {
                    }
                }
            }
        }
        #endregion

        #region "COMPRA"
        private void CarregarGradeCompra(string colecao, string produto, string cor)
        {
            var gradeCompra = ObterGradeCompra(colecao, produto, cor);

            var grade = gradeCompra.FirstOrDefault();
            gradeNome.GRADE_EXP = grade.TAMANHO_1;
            gradeNome.GRADE_XP = grade.TAMANHO_2;
            gradeNome.GRADE_PP = grade.TAMANHO_3;
            gradeNome.GRADE_P = grade.TAMANHO_4;
            gradeNome.GRADE_M = grade.TAMANHO_5;
            gradeNome.GRADE_G = grade.TAMANHO_6;
            gradeNome.GRADE_GG = grade.TAMANHO_7;


            gvGradeProducao.DataSource = gradeCompra;
            gvGradeProducao.DataBind();
        }

        private List<SP_OBTER_PRODUTO_COMPRAResult> ObterGradeCompra(string colecao, string produto, string cor)
        {
            var gradeCompra = new List<SP_OBTER_PRODUTO_COMPRAResult>();

            var p = desenvController.ObterProduto(colecao, produto, cor);
            if (p != null)
            {
                var pPedido = desenvController.ObterProdutoPedidoLinx(p.CODIGO.ToString(), 'N');

                var gradeAtacado = new SP_OBTER_PRODUTO_COMPRAResult();
                var pedidoAtacado = pPedido.Where(x => x.FILIAL.Trim() == "ATACADO HANDBOOK").SingleOrDefault();
                if (pedidoAtacado != null)
                {
                    gradeAtacado = desenvController.ObterProdutoAcabado(produto, cor, pedidoAtacado.PEDIDO);
                    gradeAtacado.PRODUTO = "1";
                }

                var gradeVarejo = new SP_OBTER_PRODUTO_COMPRAResult();
                var pedidoVarejo = pPedido.Where(x => x.FILIAL.Trim() == "CD - LUGZY").SingleOrDefault();
                if (pedidoVarejo != null)
                {
                    gradeVarejo = desenvController.ObterProdutoAcabado(produto, cor, pedidoVarejo.PEDIDO);
                    gradeVarejo.PRODUTO = "2";
                }

                gradeCompra.Add(gradeAtacado);
                gradeCompra.Add(gradeVarejo);

            }

            return gradeCompra;
        }
        protected void gvGradeProducao_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (gradeNome != null)
                {
                    e.Row.Cells[2].Text = gradeNome.GRADE_EXP;    //EXP
                    e.Row.Cells[3].Text = gradeNome.GRADE_XP;     //XP
                    e.Row.Cells[4].Text = gradeNome.GRADE_PP;     //PP
                    e.Row.Cells[5].Text = gradeNome.GRADE_P;      //P
                    e.Row.Cells[6].Text = gradeNome.GRADE_M;      //M
                    e.Row.Cells[7].Text = gradeNome.GRADE_G;      //G
                    e.Row.Cells[8].Text = gradeNome.GRADE_GG;     //GG
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_COMPRAResult _gradeProducao = e.Row.DataItem as SP_OBTER_PRODUTO_COMPRAResult;

                    if (_gradeProducao != null)
                    {

                        Literal _litGrade = e.Row.FindControl("litGrade") as Literal;
                        if (_litGrade != null)
                        {
                            //fake
                            if (_gradeProducao.PRODUTO == "1")
                                _litGrade.Text = "ATACADO";
                            else
                                _litGrade.Text = "VAREJO";
                        }

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = (_gradeProducao.QTDE_ORIGINAL).ToString();
                    }
                }
            }
        }

        #endregion
        protected void btCopiar_Click(object sender, EventArgs e)
        {

            var gradeAtacado = desenvController.ObterVendaAtacadoProduto(hidColecao.Value, hidProduto.Value, hidCor.Value).FirstOrDefault();
            if (gradeAtacado != null)
            {
                txtGradeEXP_R.Text = gradeAtacado.VE1.ToString();
                txtGradeXP_R.Text = gradeAtacado.VE2.ToString();
                txtGradePP_R.Text = gradeAtacado.VE3.ToString();
                txtGradeP_R.Text = gradeAtacado.VE4.ToString();
                txtGradeM_R.Text = gradeAtacado.VE5.ToString();
                txtGradeG_R.Text = gradeAtacado.VE6.ToString();
                txtGradeGG_R.Text = gradeAtacado.VE7.ToString();

                txtGradeTotal_R.Text = (gradeAtacado.VE1 +
                                        gradeAtacado.VE2 +
                                        gradeAtacado.VE3 +
                                        gradeAtacado.VE4 +
                                        gradeAtacado.VE5 +
                                        gradeAtacado.VE6 +
                                        gradeAtacado.VE7).ToString();
            }

        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                var gradeCompra = ObterGradeCompra(hidColecao.Value, hidProduto.Value, hidCor.Value);

                int grade01 = Convert.ToInt32(txtGradeEXP_R.Text);
                int grade02 = Convert.ToInt32(txtGradeXP_R.Text);
                int grade03 = Convert.ToInt32(txtGradePP_R.Text);
                int grade04 = Convert.ToInt32(txtGradeP_R.Text);
                int grade05 = Convert.ToInt32(txtGradeM_R.Text);
                int grade06 = Convert.ToInt32(txtGradeG_R.Text);
                int grade07 = Convert.ToInt32(txtGradeGG_R.Text);

                if (grade01 > gradeCompra.Sum(p => p.CO1).Value)
                {
                    labErro.Text = "Grade 01 não pode ser maior que a grade de Produção";
                    return;
                }

                if (grade02 > gradeCompra.Sum(p => p.CO2).Value)
                {
                    labErro.Text = "Grade 02 não pode ser maior que a grade de Produção";
                    return;
                }

                if (grade03 > gradeCompra.Sum(p => p.CO3).Value)
                {
                    labErro.Text = "Grade 03 não pode ser maior que a grade de Produção";
                    return;
                }

                if (grade04 > gradeCompra.Sum(p => p.CO4).Value)
                {
                    labErro.Text = "Grade 04 não pode ser maior que a grade de Produção";
                    return;
                }

                if (grade05 > gradeCompra.Sum(p => p.CO5).Value)
                {
                    labErro.Text = "Grade 05 não pode ser maior que a grade de Produção";
                    return;
                }

                if (grade06 > gradeCompra.Sum(p => p.CO6).Value)
                {
                    labErro.Text = "Grade 06 não pode ser maior que a grade de Produção";
                    return;
                }

                if (grade07 > gradeCompra.Sum(p => p.CO7).Value)
                {
                    labErro.Text = "Grade 07 não pode ser maior que a grade de Produção";
                    return;
                }

                var pedidoAtacado = "00000";
                var pedidoVarejo = "00000";

                //obter pedido atacado
                var pa = gradeCompra.Where(p => p.PRODUTO == "1").FirstOrDefault();
                if (pa != null)
                    pedidoAtacado = pa.PEDIDO.Trim();
                //obter pedido varejo
                var pv = gradeCompra.Where(p => p.PRODUTO == "2").FirstOrDefault();
                if (pv != null)
                    pedidoVarejo = pv.PEDIDO.Trim();

                var x = desenvController.AtualizarPedidoCompraProdutoAcabado(pedidoAtacado, pedidoVarejo, hidProduto.Value, hidCor.Value, grade01, grade02, grade03, grade04, grade05, grade06, grade07);

                //recarregar grade
                CarregarGradeCompra(hidColecao.Value, hidProduto.Value, hidCor.Value);

                labErro.Text = "Grade de Compra definida com sucesso.";
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
