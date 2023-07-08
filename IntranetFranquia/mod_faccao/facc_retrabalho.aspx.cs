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
using System.Globalization;


namespace Relatorios
{
    public partial class facc_retrabalho : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btGerar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerar, null) + ";");
        }


        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                hidCodigoHB.Value = "";

                if (ddlColecao.SelectedValue.Trim() == "" || ddlColecao.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                if (txtHB.Text.Trim() == "")
                {
                    labErro.Text = "Informe o número do HB.";
                    return;
                }


                var hb = prodController.ObterNumeroHB(ddlColecao.SelectedValue.Trim(), Convert.ToInt32(txtHB.Text.Trim())).Where(p => p.MOSTRUARIO == 'N' && p.CODIGO_PAI == null).FirstOrDefault();
                if (hb != null)
                {
                    imgFoto.ImageUrl = hb.FOTO_PECA;
                    imgFoto.Visible = true;

                    txtProduto.Text = hb.CODIGO_PRODUTO_LINX;
                    txtNome.Text = hb.NOME;
                    txtCor.Text = prodController.ObterCoresBasicas(hb.COR).DESC_COR;


                    var entrada = faccController.ObterFaccaoHistorico(hb.COLECAO, Convert.ToInt16(hb.HB), Convert.ToChar(hb.MOSTRUARIO), 20, 1).Where(p => p.CABECALHO == 2).ToList();

                    txtGradeEXP_O.Text = entrada.Sum(p => p.GRADE_EXP).ToString();
                    txtGradeXP_O.Text = entrada.Sum(p => p.GRADE_XP).ToString();
                    txtGradePP_O.Text = entrada.Sum(p => p.GRADE_PP).ToString();
                    txtGradeP_O.Text = entrada.Sum(p => p.GRADE_P).ToString();
                    txtGradeM_O.Text = entrada.Sum(p => p.GRADE_M).ToString();
                    txtGradeG_O.Text = entrada.Sum(p => p.GRADE_G).ToString();
                    txtGradeGG_O.Text = entrada.Sum(p => p.GRADE_GG).ToString();
                    txtGradeTotal_O.Text = entrada.Sum(p => p.GRADE_TOTAL).ToString();

                    hidCodigoHB.Value = hb.CODIGO.ToString();

                    txtGradeEXP_E.Enabled = true;
                    if (Convert.ToInt32(txtGradeEXP_O.Text) <= 0)
                        txtGradeEXP_E.Enabled = false;

                    txtGradeXP_E.Enabled = true;
                    if (Convert.ToInt32(txtGradeXP_O.Text) <= 0)
                        txtGradeXP_E.Enabled = false;

                    txtGradePP_E.Enabled = true;
                    if (Convert.ToInt32(txtGradePP_O.Text) <= 0)
                        txtGradePP_E.Enabled = false;

                    txtGradeP_E.Enabled = true;
                    if (Convert.ToInt32(txtGradeP_O.Text) <= 0)
                        txtGradeP_E.Enabled = false;

                    txtGradeM_E.Enabled = true;
                    if (Convert.ToInt32(txtGradeM_O.Text) <= 0)
                        txtGradeM_E.Enabled = false;

                    txtGradeG_E.Enabled = true;
                    if (Convert.ToInt32(txtGradeG_O.Text) <= 0)
                        txtGradeG_E.Enabled = false;

                    txtGradeGG_E.Enabled = true;
                    if (Convert.ToInt32(txtGradeGG_O.Text) <= 0)
                        txtGradeGG_E.Enabled = false;

                    txtGradeTotal_E.Enabled = false;

                    txtGradeEXP_E.Text = "0";
                    txtGradeXP_E.Text = "0";
                    txtGradePP_E.Text = "0";
                    txtGradeP_E.Text = "0";
                    txtGradeM_E.Text = "0";
                    txtGradeG_E.Text = "0";
                    txtGradeGG_E.Text = "0";

                    btGerar.Enabled = true;
                    labMsg.Text = "";

                    CarregarNomeGrade(hb);

                }
                else { labErro.Text = "Nenhum HB encontrado. Refaça a sua pesquisa."; }

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
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
            }
        }
        #endregion

        protected void btGerar_Click(object sender, EventArgs e)
        {

            try
            {
                labMsg.Text = "";

                if (hidCodigoHB.Value == "")
                {
                    labMsg.Text = "Informe o HB para Retrabalho.";
                    return;
                }

                // validar grade
                if (txtGradeTotal_E.Text == "" || Convert.ToInt32(txtGradeTotal_E.Text) <= 0)
                    labMsg.Text = "Informe a grade para Retrabalho.";

                if (txtGradeEXP_E.Text != "" && Convert.ToInt32(txtGradeEXP_E.Text) > Convert.ToInt32(txtGradeEXP_O.Text))
                    labMsg.Text = "A Grade EXP de Retrabalho não pode ser maior que a Grade Original.";
                else if (txtGradeXP_E.Text != "" && Convert.ToInt32(txtGradeXP_E.Text) > Convert.ToInt32(txtGradeXP_O.Text))
                    labMsg.Text = "A Grade XP de Retrabalho não pode ser maior que a Grade Original.";
                else if (txtGradePP_E.Text != "" && Convert.ToInt32(txtGradePP_E.Text) > Convert.ToInt32(txtGradePP_O.Text))
                    labMsg.Text = "A Grade PP de Retrabalho não pode ser maior que a Grade Original.";
                else if (txtGradeP_E.Text != "" && Convert.ToInt32(txtGradeP_E.Text) > Convert.ToInt32(txtGradeP_O.Text))
                    labMsg.Text = "A Grade P de Retrabalho não pode ser maior que a Grade Original.";
                else if (txtGradeM_E.Text != "" && Convert.ToInt32(txtGradeM_E.Text) > Convert.ToInt32(txtGradeM_O.Text))
                    labMsg.Text = "A Grade M de Retrabalho não pode ser maior que a Grade Original.";
                else if (txtGradeG_E.Text != "" && Convert.ToInt32(txtGradeG_E.Text) > Convert.ToInt32(txtGradeG_O.Text))
                    labMsg.Text = "A Grade G de Retrabalho não pode ser maior que a Grade Original.";
                else if (txtGradeGG_E.Text != "" && Convert.ToInt32(txtGradeGG_E.Text) > Convert.ToInt32(txtGradeGG_O.Text))
                    labMsg.Text = "A Grade GG de Retrabalho não pode ser maior que a Grade Original.";
                if (labMsg.Text != "")
                    return;


                // gerar saida de retrabalho para facção
                PROD_HB_SAIDA _saida = new PROD_HB_SAIDA();
                _saida.PROD_HB = Convert.ToInt32(hidCodigoHB.Value);
                _saida.DATA_INCLUSAO = DateTime.Now;
                _saida.PROD_PROCESSO = 20;
                _saida.GRADE_EXP = Convert.ToInt32(txtGradeEXP_E.Text);
                _saida.GRADE_XP = Convert.ToInt32(txtGradeXP_E.Text);
                _saida.GRADE_PP = Convert.ToInt32(txtGradePP_E.Text);
                _saida.GRADE_P = Convert.ToInt32(txtGradeP_E.Text);
                _saida.GRADE_M = Convert.ToInt32(txtGradeM_E.Text);
                _saida.GRADE_G = Convert.ToInt32(txtGradeG_E.Text);
                _saida.GRADE_GG = Convert.ToInt32(txtGradeGG_E.Text);
                _saida.GRADE_TOTAL = (_saida.GRADE_EXP + _saida.GRADE_XP + _saida.GRADE_PP + _saida.GRADE_P + _saida.GRADE_M + _saida.GRADE_G + _saida.GRADE_GG);
                _saida.SALDO = 'R';
                _saida.CODIGO = faccController.InserirSaidaHB(_saida);

                // verificar acabamento e retirar saldo
                var saldoAcabamento = faccController.ObterSaidaHB(_saida.PROD_HB1.CODIGO.ToString()).Where(p => p.PROD_PROCESSO == 21 && p.SALDO == 'S').FirstOrDefault();
                if (saldoAcabamento != null)
                {
                    saldoAcabamento.GRADE_EXP = saldoAcabamento.GRADE_EXP - _saida.GRADE_EXP;
                    saldoAcabamento.GRADE_XP = saldoAcabamento.GRADE_XP - _saida.GRADE_XP;
                    saldoAcabamento.GRADE_PP = saldoAcabamento.GRADE_PP - _saida.GRADE_PP;
                    saldoAcabamento.GRADE_P = saldoAcabamento.GRADE_P - _saida.GRADE_P;
                    saldoAcabamento.GRADE_M = saldoAcabamento.GRADE_M - _saida.GRADE_M;
                    saldoAcabamento.GRADE_G = saldoAcabamento.GRADE_G - _saida.GRADE_G;
                    saldoAcabamento.GRADE_GG = saldoAcabamento.GRADE_GG - _saida.GRADE_GG;
                    saldoAcabamento.GRADE_TOTAL = saldoAcabamento.GRADE_TOTAL - _saida.GRADE_TOTAL;

                    faccController.AtualizarSaidaHB(saldoAcabamento);
                }

                btGerar.Enabled = false;
                labMsg.Text = "Grade de Retrabalho criada com sucesso.";

            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

    }
}
