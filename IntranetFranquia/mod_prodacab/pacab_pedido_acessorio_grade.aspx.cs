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
    public partial class pacab_pedido_acessorio_grade : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoCarrinho = 0;
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("pacab_menu.aspx");

                codigoCarrinho = Convert.ToInt32(Request.QueryString["c"].ToString());

                DESENV_ACESSORIO_CARRINHO carrinho = desenvController.ObterCarrinhoAcessorio(codigoCarrinho);
                if (carrinho == null)
                    Response.Redirect("pacab_menu.aspx");

                hidCodigoCarrinho.Value = carrinho.CODIGO.ToString();

                var desenvAcessorio = desenvController.ObterAcessorio(carrinho.DESENV_ACESSORIO);

                txtColecao.Text = new BaseController().BuscaColecaoAtual(desenvAcessorio.COLECAO).DESC_COLECAO.Trim();
                txtGrupoProduto.Text = desenvAcessorio.GRUPO;
                txtProduto.Text = desenvAcessorio.PRODUTO;
                txtNome.Text = desenvAcessorio.DESCRICAO_SUGERIDA;
                txtCor.Text = prodController.ObterCoresBasicas(desenvAcessorio.COR).DESC_COR.Trim();
                txtGriffe.Text = desenvAcessorio.GRIFFE;
                txtQtde.Text = desenvAcessorio.QTDE.ToString();

                txtCusto.Text = (desenvAcessorio.CUSTO == null) ? "" : desenvAcessorio.CUSTO.ToString();

                CarregarControleGrade(baseController.ObterProdutoTamanho(desenvAcessorio.PRODUTO));
                CarregarGrade(carrinho);

                if (carrinho.GRADE_TOTAL <= 0)
                {
                    var produto = baseController.BuscaProduto(desenvAcessorio.PRODUTO);
                    if (produto.GRADE.Trim() == "UNICO")
                    {
                        txtGrade1.Text = desenvAcessorio.QTDE.ToString();
                        txtGradeTotal.Text = desenvAcessorio.QTDE.ToString();
                    }
                }

            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        private void CarregarControleGrade(SP_OBTER_PRODUTO_TAMANHOResult tamProduto)
        {
            labGrade1.Text = tamProduto.TAMANHO_1;
            labGrade2.Text = tamProduto.TAMANHO_2;
            labGrade3.Text = tamProduto.TAMANHO_3;
            labGrade4.Text = tamProduto.TAMANHO_4;
            labGrade5.Text = tamProduto.TAMANHO_5;
            labGrade6.Text = tamProduto.TAMANHO_6;
            labGrade7.Text = tamProduto.TAMANHO_7;
            labGrade8.Text = tamProduto.TAMANHO_8;
            labGrade9.Text = tamProduto.TAMANHO_9;
            labGrade10.Text = tamProduto.TAMANHO_10;
            labGrade11.Text = tamProduto.TAMANHO_11;
            labGrade12.Text = tamProduto.TAMANHO_12;
            labGrade13.Text = tamProduto.TAMANHO_13;
            labGrade14.Text = tamProduto.TAMANHO_14;

            txtGrade1.Enabled = (tamProduto.TAMANHO_1 != "");
            txtGrade2.Enabled = (tamProduto.TAMANHO_2 != "");
            txtGrade3.Enabled = (tamProduto.TAMANHO_3 != "");
            txtGrade4.Enabled = (tamProduto.TAMANHO_4 != "");
            txtGrade5.Enabled = (tamProduto.TAMANHO_5 != "");
            txtGrade6.Enabled = (tamProduto.TAMANHO_6 != "");
            txtGrade7.Enabled = (tamProduto.TAMANHO_7 != "");
            txtGrade8.Enabled = (tamProduto.TAMANHO_8 != "");
            txtGrade9.Enabled = (tamProduto.TAMANHO_9 != "");
            txtGrade10.Enabled = (tamProduto.TAMANHO_10 != "");
            txtGrade11.Enabled = (tamProduto.TAMANHO_11 != "");
            txtGrade12.Enabled = (tamProduto.TAMANHO_12 != "");
            txtGrade13.Enabled = (tamProduto.TAMANHO_13 != "");
            txtGrade14.Enabled = (tamProduto.TAMANHO_14 != "");
        }
        private void CarregarGrade(DESENV_ACESSORIO_CARRINHO carrinho)
        {
            txtGrade1.Text = carrinho.GRADE1.ToString();
            txtGrade2.Text = carrinho.GRADE2.ToString();
            txtGrade3.Text = carrinho.GRADE3.ToString();
            txtGrade4.Text = carrinho.GRADE4.ToString();
            txtGrade5.Text = carrinho.GRADE5.ToString();
            txtGrade6.Text = carrinho.GRADE6.ToString();
            txtGrade7.Text = carrinho.GRADE7.ToString();
            txtGrade8.Text = carrinho.GRADE8.ToString();
            txtGrade9.Text = carrinho.GRADE9.ToString();
            txtGrade10.Text = carrinho.GRADE10.ToString();
            txtGrade11.Text = carrinho.GRADE11.ToString();
            txtGrade12.Text = carrinho.GRADE12.ToString();
            txtGrade13.Text = carrinho.GRADE13.ToString();
            txtGrade14.Text = carrinho.GRADE14.ToString();

            txtGradeTotal.Text = (
                carrinho.GRADE1
                + carrinho.GRADE2
                + carrinho.GRADE3
                + carrinho.GRADE4
                + carrinho.GRADE5
                + carrinho.GRADE6
                + carrinho.GRADE7
                + carrinho.GRADE8
                + carrinho.GRADE9
                + carrinho.GRADE10
                + carrinho.GRADE11
                + carrinho.GRADE12
                + carrinho.GRADE13
                + carrinho.GRADE14
                ).ToString();


        }

        #region "DADOS INICIAIS"

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labGradeTitulo.ForeColor = _OK;
            if (txtGradeTotal.Text.Trim() == "" || txtGradeTotal.Text.Trim() == "0")
            {
                labGradeTitulo.ForeColor = _notOK;
                retorno = false;
            }

            labCusto.ForeColor = _OK;
            if (txtCusto.Text.Trim() == "" || txtCusto.Text.Trim() == "0")
            {
                labCusto.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }

        #endregion

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

                var carrinho = desenvController.ObterCarrinhoAcessorio(Convert.ToInt32(hidCodigoCarrinho.Value));

                var desenvAcessorio = desenvController.ObterAcessorio(carrinho.DESENV_ACESSORIO);
                if (desenvAcessorio != null)
                {
                    desenvAcessorio.CUSTO = Convert.ToDecimal(txtCusto.Text.Trim());
                    desenvController.AtualizarAcessorio(desenvAcessorio);
                }

                carrinho.DESENV_ACESSORIO_GRADE = null;
                carrinho.GRADE1 = grade1;
                carrinho.GRADE2 = grade2;
                carrinho.GRADE3 = grade3;
                carrinho.GRADE4 = grade4;
                carrinho.GRADE5 = grade5;
                carrinho.GRADE6 = grade6;
                carrinho.GRADE7 = grade7;
                carrinho.GRADE8 = grade8;
                carrinho.GRADE9 = grade9;
                carrinho.GRADE10 = grade10;
                carrinho.GRADE11 = grade11;
                carrinho.GRADE12 = grade12;
                carrinho.GRADE13 = grade13;
                carrinho.GRADE14 = grade14;

                carrinho.GRADE_TOTAL =
                     carrinho.GRADE1
                     + carrinho.GRADE2
                     + carrinho.GRADE3
                     + carrinho.GRADE4
                     + carrinho.GRADE5
                     + carrinho.GRADE6
                     + carrinho.GRADE7
                     + carrinho.GRADE8
                     + carrinho.GRADE9
                     + carrinho.GRADE10
                     + carrinho.GRADE11
                     + carrinho.GRADE12
                     + carrinho.GRADE13
                     + carrinho.GRADE14;
                desenvController.AtualizarCarrinhoAcessorio(carrinho);

                labErro.Text = "Grade Atualizada com Sucesso.";


            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

    }
}
