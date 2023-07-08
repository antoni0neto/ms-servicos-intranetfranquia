using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroContainer : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();
        LojaController lojaController = new LojaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListJanela();
                CarregaGridViewContainer();
            }
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = usuarioController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        protected void ddlJanela_DataBound(object sender, EventArgs e)
        {
            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";
        }

        private void CarregaDropDownListJanela()
        {
            ddlJanela.DataSource = baseController.BuscaJanela();
            ddlJanela.DataBind();
        }

        private void CarregaContainer(int codigoContainer)
        {
            IMPORTACAO_CONTAINER container = usuarioController.BuscaPorCodigoContainer(codigoContainer);

            if (container != null)
            {
                txtCodigo.Text = container.CODIGO_CONTAINER.ToString();
                txtDescricao.Text = container.DESCRICAO;

                ddlColecao.SelectedValue = container.CODIGO_COLECAO.ToString();
                ddlJanela.SelectedValue = container.CODIGO_JANELA.ToString();

                HiddenFieldCodigoContainer.Value = container.CODIGO_CONTAINER.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                IMPORTACAO_CONTAINER container;

                if (Convert.ToInt32(HiddenFieldCodigoContainer.Value) > 0)
                    container = usuarioController.BuscaPorCodigoContainer(Convert.ToInt32(HiddenFieldCodigoContainer.Value));
                else
                {
                    IMPORTACAO_CONTAINER containerOld = usuarioController.BuscaContainerPelaDescricao(txtDescricao.Text);

                    if (containerOld != null)
                        throw new Exception("Container já existe!");

                    container = new IMPORTACAO_CONTAINER();
                }

                container.CODIGO_CONTAINER = Convert.ToInt32(txtCodigo.Text);
                container.DESCRICAO = txtDescricao.Text;

                if (ddlColecao != null)
                    container.CODIGO_COLECAO = Convert.ToInt32(ddlColecao.SelectedValue);

                if (ddlJanela != null)
                    container.CODIGO_JANELA = Convert.ToInt32(ddlJanela.SelectedValue);

                if (Convert.ToInt32(HiddenFieldCodigoContainer.Value) > 0)
                    usuarioController.AtualizaContainer(container);
                else
                    usuarioController.InsereContainer(container);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewContainer();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewContainer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_CONTAINER container = e.Row.DataItem as IMPORTACAO_CONTAINER;

            if (container != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = container.CODIGO_CONTAINER.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = container.CODIGO_CONTAINER.ToString();
            }
        }

        private void CarregaGridViewContainer()
        {
            GridViewContainer.DataSource = usuarioController.BuscaContainers();
            GridViewContainer.DataBind();
        }

        protected void ButtonExcluirContainer_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                IMPORTACAO_CONTAINER container = usuarioController.BuscaPorCodigoContainer(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (container != null)
                {
                    usuarioController.ExcluiContainer(container);

                    CarregaGridViewContainer();

                    LimpaTela();
                }
            }
        }

        protected void ButtonEditarContainer_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaContainer(Convert.ToInt32(buttonEditar.CommandArgument));
            
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

            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";

            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";

            HiddenFieldCodigoContainer.Value = "0";
        }
    }
}