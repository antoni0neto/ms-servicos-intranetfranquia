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
    public partial class pacab_pedido_produto_grade_cadastrov2 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrade();
                CarregarGrades();

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        private void CarregarGrades()
        {
            var grades = desenvController.ObterGradeProduto();
            gvGrade.DataSource = grades;
            gvGrade.DataBind();
        }

        #region "DADOS INICIAIS"
        private void CarregarGrade()
        {
            var prodTam = baseController.BuscaProdutosTamanho().Where(p =>
                            p.GRADE.Trim() == "34-36-38-40-42-44" ||
                            p.GRADE.Trim() == "36-38-40-42" ||
                            p.GRADE.Trim() == "38-40-42-44-46" ||
                            p.GRADE.Trim() == "38-40-42-44-46-48" ||
                            p.GRADE.Trim() == "PP-PQ-MD-GD" ||
                            p.GRADE.Trim() == "PP-PQ-MD-GD-GG" ||
                            p.GRADE.Trim() == "PQ-MD-GD" ||
                            p.GRADE.Trim() == "PQ-MD-GD-GG" ||
                            p.GRADE.Trim() == "XP-PP-PQ-MD-GD" ||
                            p.GRADE.Trim() == "XP-PP-PQ-MD-GD-GG" ||
                            p.GRADE.Trim() == "UNICO"
                            ).OrderBy(p => p.GRADE).ToList();

            prodTam.Insert(0, new PRODUTOS_TAMANHO { GRADE = "Selecione" });
            ddlGrade.DataSource = prodTam;
            ddlGrade.DataBind();
        }
        protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGrade.SelectedValue != "Selecione")
            {
                var tam = baseController.BuscaProdutosTamanho().Where(p => p.GRADE == ddlGrade.SelectedValue).FirstOrDefault();
                if (tam != null)
                {
                    labGrade1.Text = tam.TAMANHO_1.Replace(".", "").Trim();
                    labGrade2.Text = tam.TAMANHO_2.Replace(".", "").Trim();
                    labGrade3.Text = tam.TAMANHO_3.Replace(".", "").Trim();
                    labGrade4.Text = tam.TAMANHO_4.Replace(".", "").Trim();
                    labGrade5.Text = tam.TAMANHO_5.Replace(".", "").Trim();
                    labGrade6.Text = tam.TAMANHO_6.Replace(".", "").Trim();
                    labGrade7.Text = tam.TAMANHO_7.Replace(".", "").Trim();
                    labGrade8.Text = tam.TAMANHO_8.Replace(".", "").Trim();
                    labGrade9.Text = tam.TAMANHO_9.Replace(".", "").Trim();
                    labGrade10.Text = tam.TAMANHO_10.Replace(".", "").Trim();
                    labGrade11.Text = tam.TAMANHO_11.Replace(".", "").Trim();
                    labGrade12.Text = tam.TAMANHO_12.Replace(".", "").Trim();
                    labGrade13.Text = tam.TAMANHO_13.Replace(".", "").Trim();
                    labGrade14.Text = tam.TAMANHO_14.Replace(".", "").Trim();

                    CarregarControleGrade(tam);
                }
            }
        }
        private void CarregarControleGrade(PRODUTOS_TAMANHO tamProduto)
        {
            txtGrade1.Enabled = (tamProduto.TAMANHO_1.Replace(".", "").Trim() != "");
            txtGrade2.Enabled = (tamProduto.TAMANHO_2.Replace(".", "").Trim() != "");
            txtGrade3.Enabled = (tamProduto.TAMANHO_3.Replace(".", "").Trim() != "");
            txtGrade4.Enabled = (tamProduto.TAMANHO_4.Replace(".", "").Trim() != "");
            txtGrade5.Enabled = (tamProduto.TAMANHO_5.Replace(".", "").Trim() != "");
            txtGrade6.Enabled = (tamProduto.TAMANHO_6.Replace(".", "").Trim() != "");
            txtGrade7.Enabled = (tamProduto.TAMANHO_7.Replace(".", "").Trim() != "");
            txtGrade8.Enabled = (tamProduto.TAMANHO_8.Replace(".", "").Trim() != "");
            txtGrade9.Enabled = (tamProduto.TAMANHO_9.Replace(".", "").Trim() != "");
            txtGrade10.Enabled = (tamProduto.TAMANHO_10.Replace(".", "").Trim() != "");
            txtGrade11.Enabled = (tamProduto.TAMANHO_11.Replace(".", "").Trim() != "");
            txtGrade12.Enabled = (tamProduto.TAMANHO_12.Replace(".", "").Trim() != "");
            txtGrade13.Enabled = (tamProduto.TAMANHO_13.Replace(".", "").Trim() != "");
            txtGrade14.Enabled = (tamProduto.TAMANHO_14.Replace(".", "").Trim() != "");
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labGrade.ForeColor = _OK;
            if (ddlGrade.SelectedValue == "Selecione")
            {
                labGrade.ForeColor = _notOK;
                retorno = false;
            }

            labNome.ForeColor = _OK;
            if (txtNome.Text.Trim() == "")
            {
                labNome.ForeColor = _notOK;
                retorno = false;
            }

            labGradeTitulo.ForeColor = _OK;
            if (txtGradeTotal.Text.Trim() == "" || txtGradeTotal.Text.Trim() == "0")
            {
                labGradeTitulo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }

        #endregion

        protected void gvGrade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO_GRADE grade = e.Row.DataItem as DESENV_PRODUTO_GRADE;

                    Button btAlterarGrade = e.Row.FindControl("btAlterarGrade") as Button;
                    btAlterarGrade.CommandArgument = grade.CODIGO.ToString();

                    Button btExcluirGrade = e.Row.FindControl("btExcluirGrade") as Button;
                    btExcluirGrade.CommandArgument = grade.CODIGO.ToString();
                }
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            int grade1 = 0;
            int grade2 = 0;
            int grade3 = 0;
            int grade4 = 0;
            int grade5 = 0;
            int grade6 = 0;
            int grade7 = 0;
            int grade8 = 0;
            int grade9 = 0;
            int grade10 = 0;
            int grade11 = 0;
            int grade12 = 0;
            int grade13 = 0;
            int grade14 = 0;

            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho.";
                    return;
                }


                if (txtGrade1.Text.Trim() != "")
                    grade1 = Convert.ToInt32(txtGrade1.Text);

                if (txtGrade2.Text.Trim() != "")
                    grade2 = Convert.ToInt32(txtGrade2.Text);

                if (txtGrade3.Text.Trim() != "")
                    grade3 = Convert.ToInt32(txtGrade3.Text);

                if (txtGrade4.Text.Trim() != "")
                    grade4 = Convert.ToInt32(txtGrade4.Text);

                if (txtGrade5.Text.Trim() != "")
                    grade5 = Convert.ToInt32(txtGrade5.Text);

                if (txtGrade6.Text.Trim() != "")
                    grade6 = Convert.ToInt32(txtGrade6.Text);

                if (txtGrade7.Text.Trim() != "")
                    grade7 = Convert.ToInt32(txtGrade7.Text);

                if (txtGrade8.Text.Trim() != "")
                    grade8 = Convert.ToInt32(txtGrade8.Text);

                if (txtGrade9.Text.Trim() != "")
                    grade9 = Convert.ToInt32(txtGrade9.Text);

                if (txtGrade10.Text.Trim() != "")
                    grade10 = Convert.ToInt32(txtGrade10.Text);

                if (txtGrade11.Text.Trim() != "")
                    grade11 = Convert.ToInt32(txtGrade11.Text);

                if (txtGrade12.Text.Trim() != "")
                    grade12 = Convert.ToInt32(txtGrade12.Text);

                if (txtGrade13.Text.Trim() != "")
                    grade13 = Convert.ToInt32(txtGrade13.Text);

                if (txtGrade14.Text.Trim() != "")
                    grade14 = Convert.ToInt32(txtGrade14.Text);

                DESENV_PRODUTO_GRADE grade = null;
                if (hidCodigo.Value != "" && hidCodigo.Value != "0")
                    grade = desenvController.ObterGradeProduto(Convert.ToInt32(hidCodigo.Value));
                else
                    grade = new DESENV_PRODUTO_GRADE();

                grade.GRADE1 = grade1;
                grade.GRADE2 = grade2;
                grade.GRADE3 = grade3;
                grade.GRADE4 = grade4;
                grade.GRADE5 = grade5;
                grade.GRADE6 = grade6;
                grade.GRADE7 = grade7;
                grade.GRADE8 = grade8;
                grade.GRADE9 = grade9;
                grade.GRADE10 = grade10;
                grade.GRADE11 = grade11;
                grade.GRADE12 = grade12;
                grade.GRADE13 = grade13;
                grade.GRADE14 = grade14;
                grade.NOME = txtNome.Text.Trim().ToUpper();
                grade.GRADE = ddlGrade.SelectedValue;

                if (hidCodigo.Value != "" && hidCodigo.Value != "0")
                    desenvController.AtualizarGradeProduto(grade);
                else
                    desenvController.InserirGradeProduto(grade);

                CarregarGrades();

                labErro.Text = "Grade Atualizada com Sucesso.";
                Limpar();


            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btAlterarGrade_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                int codigoGrade = Convert.ToInt32(bt.CommandArgument);

                var grade = desenvController.ObterGradeProduto(codigoGrade);
                if (grade != null)
                {
                    hidCodigo.Value = codigoGrade.ToString();

                    ddlGrade.SelectedValue = grade.GRADE;
                    txtNome.Text = grade.NOME;
                    txtGrade1.Text = grade.GRADE1.ToString();
                    txtGrade2.Text = grade.GRADE2.ToString();
                    txtGrade3.Text = grade.GRADE3.ToString();
                    txtGrade4.Text = grade.GRADE4.ToString();
                    txtGrade5.Text = grade.GRADE5.ToString();
                    txtGrade6.Text = grade.GRADE6.ToString();
                    txtGrade7.Text = grade.GRADE7.ToString();
                    txtGrade8.Text = grade.GRADE8.ToString();
                    txtGrade9.Text = grade.GRADE9.ToString();
                    txtGrade10.Text = grade.GRADE10.ToString();
                    txtGrade11.Text = grade.GRADE11.ToString();
                    txtGrade12.Text = grade.GRADE12.ToString();
                    txtGrade13.Text = grade.GRADE13.ToString();
                    txtGrade14.Text = grade.GRADE14.ToString();

                    txtGradeTotal.Text = (
                        grade.GRADE1 +
                        grade.GRADE2 +
                        grade.GRADE3 +
                        grade.GRADE4 +
                        grade.GRADE5 +
                        grade.GRADE6 +
                        grade.GRADE7 +
                        grade.GRADE8 +
                        grade.GRADE9 +
                        grade.GRADE10 +
                        grade.GRADE11 +
                        grade.GRADE12 +
                        grade.GRADE13 +
                        grade.GRADE14).ToString();

                    btCancelar.Visible = true;

                    ddlGrade_SelectedIndexChanged(ddlGrade, null);
                }
            }
            catch (Exception)
            {
            }
        }
        protected void btExcluirGrade_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                int codigoGrade = Convert.ToInt32(bt.CommandArgument);

                desenvController.ExcluirGradeProduto(codigoGrade);
                CarregarGrades();
            }
            catch (Exception)
            {
            }
        }

        protected void btCancelar_Click(object sender, EventArgs e)
        {
            Limpar();
        }
        private void Limpar()
        {
            ddlGrade.SelectedValue = "Selecione";
            txtNome.Text = "";
            txtGrade1.Text = "";
            txtGrade2.Text = "";
            txtGrade3.Text = "";
            txtGrade4.Text = "";
            txtGrade5.Text = "";
            txtGrade6.Text = "";
            txtGrade7.Text = "";
            txtGrade8.Text = "";
            txtGrade9.Text = "";
            txtGrade10.Text = "";
            txtGrade11.Text = "";
            txtGrade12.Text = "";
            txtGrade13.Text = "";
            txtGrade14.Text = "";

            txtGradeTotal.Text = "";

            hidCodigo.Value = "0";
            btCancelar.Visible = false;
        }



    }
}
