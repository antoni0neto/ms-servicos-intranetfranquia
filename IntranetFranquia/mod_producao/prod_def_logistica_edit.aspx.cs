using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class prod_def_logistica_edit : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        PROD_GRADE gradeNome = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoHB = 0;
                if (
                    Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("prod_menu.aspx");

                codigoHB = Convert.ToInt32(Request.QueryString["c"].ToString());

                PROD_HB prodHB = prodController.ObterHB(codigoHB);
                if (prodHB == null)
                    Response.Redirect("prod_menu.aspx");

                hidCodigoHB.Value = prodHB.CODIGO.ToString();

                hidColecao.Value = prodHB.COLECAO.Trim();
                hidProduto.Value = prodHB.CODIGO_PRODUTO_LINX.Trim();
                hidCor.Value = prodHB.COR.Trim();

                labColecao.Text = baseController.BuscaColecaoAtual(prodHB.COLECAO).DESC_COLECAO;
                labProduto.Text = prodHB.CODIGO_PRODUTO_LINX;
                labNome.Text = prodHB.NOME;
                labHB.Text = prodHB.HB.ToString();
                labCor.Text = prodController.ObterCoresBasicas(prodHB.COR).DESC_COR.Trim();

                if (prodHB.PROD_GRADE != null)
                    gradeNome = prodController.ObterGradeNome(Convert.ToInt32(prodHB.PROD_GRADE));

                CarregarGradeVendaAtacado(prodHB);
                CarregarGradeProducao(prodHB);
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
        private void CarregarGradeVendaAtacado(PROD_HB prodHB)
        {
            List<SP_OBTER_VENDA_ATACADO_PRODUTOResult> _gradeVendaAtacado = new List<SP_OBTER_VENDA_ATACADO_PRODUTOResult>();
            SP_OBTER_VENDA_ATACADO_PRODUTOResult gradeVendaAtacado = desenvController.ObterVendaAtacadoProduto(prodHB.COLECAO.Trim(), prodHB.CODIGO_PRODUTO_LINX.Trim(), prodHB.COR.Trim()).FirstOrDefault();
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

        #region "PRODUCAO"
        private void CarregarGradeProducao(PROD_HB prodHB)
        {
            List<PROD_HB_GRADE> _gradeProducao = new List<PROD_HB_GRADE>();

            //Obter produção atacado
            PROD_HB_GRADE gradeProducaoAtacado = prodController.ObterGradeHB(prodHB.CODIGO, 99);
            if (gradeProducaoAtacado != null)
            {
                //Adiciona produção atacado
                gradeProducaoAtacado.CODIGO = 1;
                _gradeProducao.Add(gradeProducaoAtacado);

                //Obter produção total
                var gradeProducaoTotal = prodController.ObterGradeHB(prodHB.CODIGO, 3);
                if (gradeProducaoTotal != null)
                {
                    //Obter Varejo
                    PROD_HB_GRADE gradeVarejo = new PROD_HB_GRADE();
                    gradeVarejo.GRADE_EXP = gradeProducaoTotal.GRADE_EXP - gradeProducaoAtacado.GRADE_EXP;
                    gradeVarejo.GRADE_XP = gradeProducaoTotal.GRADE_XP - gradeProducaoAtacado.GRADE_XP;
                    gradeVarejo.GRADE_PP = gradeProducaoTotal.GRADE_PP - gradeProducaoAtacado.GRADE_PP;
                    gradeVarejo.GRADE_P = gradeProducaoTotal.GRADE_P - gradeProducaoAtacado.GRADE_P;
                    gradeVarejo.GRADE_M = gradeProducaoTotal.GRADE_M - gradeProducaoAtacado.GRADE_M;
                    gradeVarejo.GRADE_G = gradeProducaoTotal.GRADE_G - gradeProducaoAtacado.GRADE_G;
                    gradeVarejo.GRADE_GG = gradeProducaoTotal.GRADE_GG - gradeProducaoAtacado.GRADE_GG;

                    //Se existir varejo, adiciona na lista
                    if (gradeVarejo.GRADE_EXP > 0 ||
                        gradeVarejo.GRADE_XP > 0 ||
                        gradeVarejo.GRADE_PP > 0 ||
                        gradeVarejo.GRADE_P > 0 ||
                        gradeVarejo.GRADE_M > 0 ||
                        gradeVarejo.GRADE_G > 0 ||
                        gradeVarejo.GRADE_GG > 0
                        )
                    {
                        gradeVarejo.CODIGO = 2;
                        _gradeProducao.Add(gradeVarejo);
                    }
                }

            }

            gvGradeProducao.DataSource = _gradeProducao;
            gvGradeProducao.DataBind();
        }
        protected void gvGradeProducao_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    PROD_HB_GRADE _gradeProducao = e.Row.DataItem as PROD_HB_GRADE;

                    if (_gradeProducao != null)
                    {

                        Literal _litGrade = e.Row.FindControl("litGrade") as Literal;
                        if (_litGrade != null)
                        {
                            if (_gradeProducao.CODIGO == 1)
                                _litGrade.Text = "ATACADO";
                            else
                                _litGrade.Text = "VAREJO";
                        }

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = (_gradeProducao.GRADE_EXP +
                                                _gradeProducao.GRADE_XP +
                                                _gradeProducao.GRADE_PP +
                                                _gradeProducao.GRADE_P +
                                                _gradeProducao.GRADE_M +
                                                _gradeProducao.GRADE_G +
                                                _gradeProducao.GRADE_GG).ToString();
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
                txtGradeEXP_R.Text = gradeAtacado.VE9.ToString();
                txtGradeXP_R.Text = gradeAtacado.VE1.ToString();
                txtGradePP_R.Text = gradeAtacado.VE2.ToString();
                txtGradeP_R.Text = gradeAtacado.VE3.ToString();
                txtGradeM_R.Text = gradeAtacado.VE4.ToString();
                txtGradeG_R.Text = gradeAtacado.VE5.ToString();
                txtGradeGG_R.Text = gradeAtacado.VE6.ToString();

                txtGradeTotal_R.Text = (gradeAtacado.VE9 +
                                        gradeAtacado.VE1 +
                                        gradeAtacado.VE2 +
                                        gradeAtacado.VE3 +
                                        gradeAtacado.VE4 +
                                        gradeAtacado.VE5 +
                                        gradeAtacado.VE6).ToString();
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

                var gradeProducao = prodController.ObterGradeHB(Convert.ToInt32(hidCodigoHB.Value), 3);

                int gradeEXP = Convert.ToInt32(txtGradeEXP_R.Text);
                int gradeXP = Convert.ToInt32(txtGradeXP_R.Text);
                int gradePP = Convert.ToInt32(txtGradePP_R.Text);
                int gradeP = Convert.ToInt32(txtGradeP_R.Text);
                int gradeM = Convert.ToInt32(txtGradeM_R.Text);
                int gradeG = Convert.ToInt32(txtGradeG_R.Text);
                int gradeGG = Convert.ToInt32(txtGradeGG_R.Text);

                if (gradeEXP > gradeProducao.GRADE_EXP)
                {
                    labErro.Text = "Grade EXP não pode ser maior que a grade de Produção (EXP: " + gradeProducao.GRADE_EXP.ToString() + ")";
                    return;
                }

                if (gradeXP > gradeProducao.GRADE_XP)
                {
                    labErro.Text = "Grade XP não pode ser maior que a grade de Produção (XP: " + gradeProducao.GRADE_XP.ToString() + ")";
                    return;
                }

                if (gradePP > gradeProducao.GRADE_PP)
                {
                    labErro.Text = "Grade PP não pode ser maior que a grade de Produção (PP: " + gradeProducao.GRADE_PP.ToString() + ")";
                    return;
                }

                if (gradeP > gradeProducao.GRADE_P)
                {
                    labErro.Text = "Grade P não pode ser maior que a grade de Produção (P: " + gradeProducao.GRADE_P.ToString() + ")";
                    return;
                }

                if (gradeM > gradeProducao.GRADE_M)
                {
                    labErro.Text = "Grade M não pode ser maior que a grade de Produção (M: " + gradeProducao.GRADE_M.ToString() + ")";
                    return;
                }

                if (gradeG > gradeProducao.GRADE_G)
                {
                    labErro.Text = "Grade G não pode ser maior que a grade de Produção (G: " + gradeProducao.GRADE_G.ToString() + ")";
                    return;
                }

                if (gradeGG > gradeProducao.GRADE_GG)
                {
                    labErro.Text = "Grade GG não pode ser maior que a grade de Produção (GG: " + gradeProducao.GRADE_GG.ToString() + ")";
                    return;
                }

                var gradeAtacado = prodController.ObterGradeHB(Convert.ToInt32(hidCodigoHB.Value), 99);
                gradeAtacado.GRADE_EXP = gradeEXP;
                gradeAtacado.GRADE_XP = gradeXP;
                gradeAtacado.GRADE_PP = gradePP;
                gradeAtacado.GRADE_P = gradeP;
                gradeAtacado.GRADE_M = gradeM;
                gradeAtacado.GRADE_G = gradeG;
                gradeAtacado.GRADE_GG = gradeGG;
                prodController.AtualizarGrade(gradeAtacado, 99);

                var hb = prodController.ObterHB(Convert.ToInt32(hidCodigoHB.Value));
                hb.DATA_GRADE_ATACADO = DateTime.Now;
                hb.USUARIO_GRADE_ATACADO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                prodController.AtualizarHB(hb);

                labErro.Text = "Grade Atacado definida com sucesso.";
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
