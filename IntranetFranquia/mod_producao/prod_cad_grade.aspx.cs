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
using System.Data.OleDb;

namespace Relatorios
{
    public partial class prod_cad_grade : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Carregar gv de Grade
                CarregarGrades();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarGrades()
        {
            List<PROD_GRADE> _grade = new List<PROD_GRADE>();

            try
            {
                _grade = prodController.ObterGradeNome();

                if (_grade != null)
                {
                    gvGrade.DataSource = _grade;
                    gvGrade.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvGrade): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labNomeGrade.ForeColor = Color.Black;
            labGradeEXP.ForeColor = Color.Black;
            labGradeXP.ForeColor = Color.Black;
            labGradePP.ForeColor = Color.Black;
            labGradeP.ForeColor = Color.Black;
            labGradeM.ForeColor = Color.Black;
            labGradeG.ForeColor = Color.Black;
            labGradeGG.ForeColor = Color.Black;
            btCancelar.Visible = false;

            txtNomeGrade.Text = "";
            txtGradeEXP.Text = "";
            txtGradeXP.Text = "";
            txtGradePP.Text = "";
            txtGradeP.Text = "";
            txtGradeM.Text = "";
            txtGradeG.Text = "";
            txtGradeGG.Text = "";

            CarregarGrades();
        }
        #endregion

        #region "CRUD"
        private void Incluir(PROD_GRADE _grade)
        {
            prodController.InserirGradeNome(_grade);
        }
        private void Editar(PROD_GRADE _grade)
        {
            prodController.AtualizarGradeNome(_grade);
        }
        private void Excluir(int _codigo)
        {
            prodController.ExcluirGradeNome(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao;

            labErro.Text = "";

            if (txtNomeGrade.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade.";
                return;
            }

            if (txtGradeEXP.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade 1.";
                return;
            }
            if (txtGradeXP.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade 2.";
                return;
            }
            if (txtGradePP.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade 3.";
                return;
            }
            if (txtGradeP.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade 4.";
                return;
            }
            if (txtGradeM.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade 5.";
                return;
            }
            if (txtGradeG.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade 6.";
                return;
            }
            if (txtGradeGG.Text.Trim() == "")
            {
                labErro.Text = "Informe o Nome da Grade 7.";
                return;
            }

            try
            {
                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                PROD_GRADE _novo = new PROD_GRADE();

                _novo.GRADE = txtNomeGrade.Text.Trim().ToUpper();
                _novo.GRADE_EXP = txtGradeEXP.Text.Trim().ToUpper();
                _novo.GRADE_XP = txtGradeXP.Text.Trim().ToUpper();
                _novo.GRADE_PP = txtGradePP.Text.Trim().ToUpper();
                _novo.GRADE_P = txtGradeP.Text.Trim().ToUpper();
                _novo.GRADE_M = txtGradeM.Text.Trim().ToUpper();
                _novo.GRADE_G = txtGradeG.Text.Trim().ToUpper();
                _novo.GRADE_GG = txtGradeGG.Text.Trim().ToUpper();

                if (_Inclusao)
                {
                    Incluir(_novo);
                }
                else
                {
                    _novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    Editar(_novo);
                }

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    PROD_GRADE _grade = prodController.ObterGradeNome(Convert.ToInt32(b.CommandArgument));
                    if (_grade != null)
                    {
                        hidCodigo.Value = _grade.CODIGO.ToString();

                        txtNomeGrade.Text = _grade.GRADE;

                        txtGradeEXP.Text = _grade.GRADE_EXP.ToString();
                        txtGradeXP.Text = _grade.GRADE_XP.ToString();
                        txtGradePP.Text = _grade.GRADE_PP.ToString();
                        txtGradeP.Text = _grade.GRADE_P.ToString();
                        txtGradeM.Text = _grade.GRADE_M.ToString();
                        txtGradeG.Text = _grade.GRADE_G.ToString();
                        txtGradeGG.Text = _grade.GRADE_GG.ToString();

                        labNomeGrade.ForeColor = Color.Red;
                        labGradeEXP.ForeColor = Color.Red;
                        labGradeXP.ForeColor = Color.Red;
                        labGradePP.ForeColor = Color.Red;
                        labGradeP.ForeColor = Color.Red;
                        labGradeM.ForeColor = Color.Red;
                        labGradeG.ForeColor = Color.Red;
                        labGradeGG.ForeColor = Color.Red;

                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            if (b != null)
            {
                labErro.Text = "";
                try
                {
                    try
                    {
                        Excluir(Convert.ToInt32(b.CommandArgument));
                        RecarregarTela();
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = ex.Message;
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion

        protected void gvGrade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_GRADE _grade = e.Row.DataItem as PROD_GRADE;

                    coluna += 1;
                    if (_grade != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _grade.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _grade.CODIGO.ToString();
                    }
                }
            }
        }
    }

}
