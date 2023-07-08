using DAL;
using System;
using System.Web.UI;

namespace Relatorios
{
    public partial class pacab_pedido_acessorio_grade_linx : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtEntregaNova.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {

                int codigoCarrinho = 0;
                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("pacab_menu.aspx");

                codigoCarrinho = Convert.ToInt32(Request.QueryString["c"].ToString());

                DESENV_ACESSORIO_CARRINHO_LINX carrinho = desenvController.ObterCarrinhoLinxAcessorio(codigoCarrinho);
                if (carrinho == null)
                    Response.Redirect("pacab_menu.aspx");

                hidCodigoCarrinho.Value = carrinho.CODIGO.ToString();

                var desenvAcessorio = desenvController.ObterAcessorio(carrinho.PRODUTO, carrinho.COR_PRODUTO);

                txtColecao.Text = new BaseController().BuscaColecaoAtual(desenvAcessorio.COLECAO).DESC_COLECAO.Trim();
                txtGrupoProduto.Text = desenvAcessorio.GRUPO;
                txtProduto.Text = desenvAcessorio.PRODUTO;
                txtNome.Text = desenvAcessorio.DESCRICAO_SUGERIDA;
                txtCor.Text = prodController.ObterCoresBasicas(desenvAcessorio.COR).DESC_COR.Trim();
                txtGriffe.Text = desenvAcessorio.GRIFFE;
                txtQtde.Text = desenvAcessorio.QTDE.ToString();
                txtCustoNota.Text = (desenvAcessorio.CUSTO_NOTA == null) ? "" : desenvAcessorio.CUSTO_NOTA.ToString();

                CarregarControleGrade(baseController.ObterProdutoTamanho(desenvAcessorio.PRODUTO));
                CarregarGrade(carrinho);

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

            txtGradeNova1.Enabled = (tamProduto.TAMANHO_1 != "");
            txtGradeNova2.Enabled = (tamProduto.TAMANHO_2 != "");
            txtGradeNova3.Enabled = (tamProduto.TAMANHO_3 != "");
            txtGradeNova4.Enabled = (tamProduto.TAMANHO_4 != "");
            txtGradeNova5.Enabled = (tamProduto.TAMANHO_5 != "");
            txtGradeNova6.Enabled = (tamProduto.TAMANHO_6 != "");
            txtGradeNova7.Enabled = (tamProduto.TAMANHO_7 != "");
            txtGradeNova8.Enabled = (tamProduto.TAMANHO_8 != "");
            txtGradeNova9.Enabled = (tamProduto.TAMANHO_9 != "");
            txtGradeNova10.Enabled = (tamProduto.TAMANHO_10 != "");
        }
        private void CarregarGrade(DESENV_ACESSORIO_CARRINHO_LINX carrinho)
        {

            var comprasProdutoPre = desenvController.ObterComprasProdutoPrePedido(carrinho.PEDIDO, carrinho.PRODUTO, carrinho.COR_PRODUTO, carrinho.ENTREGA);

            txtCusto.Text = (comprasProdutoPre.CUSTO1 == null) ? "" : comprasProdutoPre.CUSTO1.ToString();
            txtNotaFiscal.Text = comprasProdutoPre.NOTA_FISCAL;

            if (comprasProdutoPre.LIMITE_ENTREGA != null)
                txtEntrega.Text = comprasProdutoPre.LIMITE_ENTREGA.ToString("dd/MM/yyyy");

            txtGradeO1.Text = comprasProdutoPre.CO1.ToString();
            txtGradeO2.Text = comprasProdutoPre.CO2.ToString();
            txtGradeO3.Text = comprasProdutoPre.CO3.ToString();
            txtGradeO4.Text = comprasProdutoPre.CO4.ToString();
            txtGradeO5.Text = comprasProdutoPre.CO5.ToString();
            txtGradeO6.Text = comprasProdutoPre.CO6.ToString();
            txtGradeO7.Text = comprasProdutoPre.CO7.ToString();
            txtGradeO8.Text = comprasProdutoPre.CO8.ToString();
            txtGradeO9.Text = comprasProdutoPre.CO9.ToString();
            txtGradeO10.Text = comprasProdutoPre.CO10.ToString();

            txtGrade1.Text = comprasProdutoPre.CO1.ToString();
            txtGrade2.Text = comprasProdutoPre.CO2.ToString();
            txtGrade3.Text = comprasProdutoPre.CO3.ToString();
            txtGrade4.Text = comprasProdutoPre.CO4.ToString();
            txtGrade5.Text = comprasProdutoPre.CO5.ToString();
            txtGrade6.Text = comprasProdutoPre.CO6.ToString();
            txtGrade7.Text = comprasProdutoPre.CO7.ToString();
            txtGrade8.Text = comprasProdutoPre.CO8.ToString();
            txtGrade9.Text = comprasProdutoPre.CO9.ToString();
            txtGrade10.Text = comprasProdutoPre.CO10.ToString();

            var gradeTotal = (
                comprasProdutoPre.CO1
                + comprasProdutoPre.CO2
                + comprasProdutoPre.CO3
                + comprasProdutoPre.CO4
                + comprasProdutoPre.CO5
                + comprasProdutoPre.CO6
                + comprasProdutoPre.CO7
                + comprasProdutoPre.CO8
                + comprasProdutoPre.CO9
                + comprasProdutoPre.CO10
                + comprasProdutoPre.CO11
                + comprasProdutoPre.CO12
                + comprasProdutoPre.CO13
                + comprasProdutoPre.CO14
                );

            txtGradeTotal.Text = gradeTotal.ToString();
            txtGradeOTot.Text = gradeTotal.ToString();

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

            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho.";
                    return;
                }

                if (cbGerarOutroPedido.Checked)
                {
                    if (txtEntregaNova.Text == "")
                    {
                        labErro.Text = "Informe a nova Data de Entrega.";
                        return;
                    }

                    if (txtEntregaNova.Text == txtEntrega.Text)
                    {
                        labErro.Text = "A nova data de entrega deve ser diferente da data de entrega atual.";
                        return;
                    }

                    if (txtGradeNovaTot.Text == "" || txtGradeNovaTot.Text == "0")
                    {
                        labErro.Text = "Informe a nova grade.";
                        return;
                    }

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


                var carrinho = desenvController.ObterCarrinhoLinxAcessorio(Convert.ToInt32(hidCodigoCarrinho.Value));

                var comprasProdutoPre = desenvController.ObterComprasProdutoPrePedido(carrinho.PEDIDO, carrinho.PRODUTO, carrinho.COR_PRODUTO, carrinho.ENTREGA);


                if (txtEntrega.Text.Trim() != "")
                {
                    comprasProdutoPre.LIMITE_ENTREGA = Convert.ToDateTime(txtEntrega.Text.Trim());
                }

                comprasProdutoPre.CUSTO1 = Convert.ToDecimal(txtCusto.Text.Trim());

                comprasProdutoPre.QTDE_ENTREGAR = Convert.ToInt32(txtGradeTotal.Text);
                comprasProdutoPre.QTDE_ORIGINAL = Convert.ToInt32(txtGradeTotal.Text);

                comprasProdutoPre.VALOR_ENTREGAR = (comprasProdutoPre.QTDE_ENTREGAR * comprasProdutoPre.CUSTO1);
                comprasProdutoPre.VALOR_ORIGINAL = (comprasProdutoPre.QTDE_ENTREGAR * comprasProdutoPre.CUSTO1);

                comprasProdutoPre.NOTA_FISCAL = txtNotaFiscal.Text.Trim();

                comprasProdutoPre.CO1 = grade1;
                comprasProdutoPre.CO2 = grade2;
                comprasProdutoPre.CO3 = grade3;
                comprasProdutoPre.CO4 = grade4;
                comprasProdutoPre.CO5 = grade5;
                comprasProdutoPre.CO6 = grade6;
                comprasProdutoPre.CO7 = grade7;
                comprasProdutoPre.CO8 = grade8;
                comprasProdutoPre.CO9 = grade9;
                comprasProdutoPre.CO10 = grade10;
                comprasProdutoPre.CO11 = 0;
                comprasProdutoPre.CO12 = 0;
                comprasProdutoPre.CO13 = 0;
                comprasProdutoPre.CO14 = 0;

                comprasProdutoPre.CE1 = grade1;
                comprasProdutoPre.CE2 = grade2;
                comprasProdutoPre.CE3 = grade3;
                comprasProdutoPre.CE4 = grade4;
                comprasProdutoPre.CE5 = grade5;
                comprasProdutoPre.CE6 = grade6;
                comprasProdutoPre.CE7 = grade7;
                comprasProdutoPre.CE8 = grade8;
                comprasProdutoPre.CE9 = grade9;
                comprasProdutoPre.CE10 = grade10;
                comprasProdutoPre.CE11 = 0;
                comprasProdutoPre.CE12 = 0;
                comprasProdutoPre.CE13 = 0;
                comprasProdutoPre.CE14 = 0;

                desenvController.AtualizarComprasProdutoPrePedido(comprasProdutoPre);

                var desenvAcessorios = desenvController.ObterAcessorioPorProduto(carrinho.PRODUTO);
                foreach (var des in desenvAcessorios)
                {
                    var d = desenvController.ObterAcessorio(des.CODIGO);
                    d.CUSTO = Convert.ToDecimal(txtCusto.Text.Trim());
                    if (txtCustoNota.Text != "")
                        d.CUSTO_NOTA = Convert.ToDecimal(txtCustoNota.Text.Trim());
                    d.QTDE = comprasProdutoPre.QTDE_ORIGINAL;
                    desenvController.AtualizarAcessorio(d);
                }

                if (cbGerarOutroPedido.Checked)
                    GerarOutroPedido(comprasProdutoPre);

                labErro.Text = "Acessório Atualizado com Sucesso.";

                btSalvar.Enabled = false;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void GerarOutroPedido(COMPRAS_PRODUTO_PREPEDIDO compraProduto)
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


            if (txtGradeNova1.Text.Trim() != "")
                grade1 = Convert.ToInt32(txtGradeNova1.Text);

            if (txtGradeNova2.Text.Trim() != "")
                grade2 = Convert.ToInt32(txtGradeNova2.Text);

            if (txtGradeNova3.Text.Trim() != "")
                grade3 = Convert.ToInt32(txtGradeNova3.Text);

            if (txtGradeNova4.Text.Trim() != "")
                grade4 = Convert.ToInt32(txtGradeNova4.Text);

            if (txtGradeNova5.Text.Trim() != "")
                grade5 = Convert.ToInt32(txtGradeNova5.Text);

            if (txtGradeNova6.Text.Trim() != "")
                grade6 = Convert.ToInt32(txtGradeNova6.Text);

            if (txtGradeNova7.Text.Trim() != "")
                grade7 = Convert.ToInt32(txtGradeNova7.Text);

            if (txtGradeNova8.Text.Trim() != "")
                grade8 = Convert.ToInt32(txtGradeNova8.Text);

            if (txtGradeNova9.Text.Trim() != "")
                grade9 = Convert.ToInt32(txtGradeNova9.Text);

            if (txtGradeNova10.Text.Trim() != "")
                grade10 = Convert.ToInt32(txtGradeNova10.Text);

            var compraProdutoDiff = new COMPRAS_PRODUTO_PREPEDIDO();

            compraProdutoDiff.PEDIDO = compraProduto.PEDIDO;
            compraProdutoDiff.PRODUTO = compraProduto.PRODUTO;
            compraProdutoDiff.COR_PRODUTO = compraProduto.COR_PRODUTO;
            compraProdutoDiff.ENTREGA = Convert.ToDateTime(txtEntregaNova.Text);
            compraProdutoDiff.LIMITE_ENTREGA = compraProdutoDiff.ENTREGA;
            compraProdutoDiff.CUSTO1 = compraProduto.CUSTO1;
            compraProdutoDiff.CUSTO2 = compraProduto.CUSTO2;
            compraProdutoDiff.CUSTO3 = compraProduto.CUSTO3;
            compraProdutoDiff.CUSTO4 = compraProduto.CUSTO4;
            compraProdutoDiff.IPI = compraProduto.IPI;

            compraProdutoDiff.QTDE_ORIGINAL = Convert.ToInt32(txtGradeNovaTot.Text.Trim());
            compraProdutoDiff.QTDE_ENTREGAR = compraProdutoDiff.QTDE_ORIGINAL;

            compraProdutoDiff.VALOR_ENTREGAR = (compraProdutoDiff.QTDE_ENTREGAR * compraProduto.CUSTO1);
            compraProdutoDiff.VALOR_ORIGINAL = (compraProdutoDiff.QTDE_ENTREGAR * compraProduto.CUSTO1);

            compraProdutoDiff.NOTA_FISCAL = "";
            compraProdutoDiff.COD_CATEGORIA = compraProduto.COD_CATEGORIA;

            compraProdutoDiff.DATA_BAIXA_ETI_COMP = compraProduto.DATA_BAIXA_ETI_COMP;
            compraProdutoDiff.DATA_BAIXA_ETI_BARRA = compraProduto.DATA_BAIXA_ETI_BARRA;
            compraProdutoDiff.DATA_BAIXA_TAG = compraProduto.DATA_BAIXA_TAG;
            compraProdutoDiff.DATA_BAIXA_AVIAMENTO = compraProduto.DATA_BAIXA_AVIAMENTO;

            compraProdutoDiff.USUARIO_BAIXA_ETI_COMP = compraProduto.USUARIO_BAIXA_ETI_COMP;
            compraProdutoDiff.USUARIO_BAIXA_ETI_BARRA = compraProduto.USUARIO_BAIXA_ETI_BARRA;
            compraProdutoDiff.USUARIO_BAIXA_TAG = compraProduto.USUARIO_BAIXA_TAG;
            compraProdutoDiff.USUARIO_BAIXA_AVIAMENTO = compraProduto.USUARIO_BAIXA_AVIAMENTO;

            compraProdutoDiff.OBS = compraProduto.OBS;

            compraProdutoDiff.CO1 = grade1;
            compraProdutoDiff.CO2 = grade2;
            compraProdutoDiff.CO3 = grade3;
            compraProdutoDiff.CO4 = grade4;
            compraProdutoDiff.CO5 = grade5;
            compraProdutoDiff.CO6 = grade6;
            compraProdutoDiff.CO7 = grade7;
            compraProdutoDiff.CO8 = grade8;
            compraProdutoDiff.CO9 = grade9;
            compraProdutoDiff.CO10 = grade10;
            compraProdutoDiff.CO11 = 0;
            compraProdutoDiff.CO12 = 0;
            compraProdutoDiff.CO13 = 0;
            compraProdutoDiff.CO14 = 0;

            compraProdutoDiff.CE1 = grade1;
            compraProdutoDiff.CE2 = grade2;
            compraProdutoDiff.CE3 = grade3;
            compraProdutoDiff.CE4 = grade4;
            compraProdutoDiff.CE5 = grade5;
            compraProdutoDiff.CE6 = grade6;
            compraProdutoDiff.CE7 = grade7;
            compraProdutoDiff.CE8 = grade8;
            compraProdutoDiff.CE9 = grade9;
            compraProdutoDiff.CE10 = grade10;
            compraProdutoDiff.CE11 = 0;
            compraProdutoDiff.CE12 = 0;
            compraProdutoDiff.CE13 = 0;
            compraProdutoDiff.CE14 = 0;


            desenvController.InserirComprasProdutoPrePedido(compraProdutoDiff);


        }

    }
}
