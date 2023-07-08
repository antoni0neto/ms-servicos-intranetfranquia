using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroColecao : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaGridViewColecao();
        }

        private void CarregaColecao(int codigoColecao)
        {
            IMPORTACAO_COLECAO colecao = usuarioController.BuscaPorCodigoColecao(codigoColecao);

            if (colecao != null)
            {
                txtCodigo.Text = colecao.CODIGO_COLECAO.ToString();
                txtDescricao.Text = colecao.DESCRICAO;
                txtTaxa.Text = colecao.TAXA.ToString();
                
                HiddenFieldCodigoColecao.Value = colecao.CODIGO_COLECAO.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                IMPORTACAO_COLECAO colecao;

                if (Convert.ToInt32(HiddenFieldCodigoColecao.Value) > 0)
                    colecao = usuarioController.BuscaPorCodigoColecao(Convert.ToInt32(HiddenFieldCodigoColecao.Value));
                else
                {
                    IMPORTACAO_COLECAO colecaoOld = usuarioController.BuscaColecaoPelaDescricao(txtDescricao.Text);

                    if (colecaoOld != null)
                        throw new Exception("Coleção já existe!");

                    colecao = new IMPORTACAO_COLECAO();
                }

                colecao.CODIGO_COLECAO = Convert.ToInt32(txtCodigo.Text);
                colecao.DESCRICAO = txtDescricao.Text;
                colecao.TAXA = Convert.ToInt32(txtTaxa.Text);

                if (Convert.ToInt32(HiddenFieldCodigoColecao.Value) > 0)
                    usuarioController.AtualizaColecao(colecao);
                else
                    usuarioController.InsereColecao(colecao);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewColecao();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewColecao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_COLECAO colecao = e.Row.DataItem as IMPORTACAO_COLECAO;

            if (colecao != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = colecao.CODIGO_COLECAO.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = colecao.CODIGO_COLECAO.ToString();
            }
        }

        private void CarregaGridViewColecao()
        {
            GridViewColecao.DataSource = usuarioController.BuscaColecoes();
            GridViewColecao.DataBind();
        }

        protected void ButtonExcluirColecao_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                IMPORTACAO_COLECAO colecao = usuarioController.BuscaPorCodigoColecao(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (colecao != null)
                {
                    usuarioController.ExcluiColecao(colecao);

                    CarregaGridViewColecao();

                    LimpaTela();
                }
            }
        }

        protected void ButtonEditarColecao_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaColecao(Convert.ToInt32(buttonEditar.CommandArgument));
            
                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            txtCodigo.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtTaxa.Text = string.Empty;
            HiddenFieldCodigoColecao.Value = "0";
        }
    }
}