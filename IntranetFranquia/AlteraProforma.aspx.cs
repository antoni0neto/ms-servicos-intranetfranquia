using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class AlteraProforma : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListPerfil();
                CarregaGridViewUsuario();
            }
        }

        private void CarregaDropDownListPerfil()
        {
            DropDownListPerfil.DataSource = perfilController.BuscaPerfil();
            DropDownListPerfil.DataBind();
        }

        private void CarregaFuncionario(int codigoFuncionario)
        {
            USUARIO usuario = usuarioController.BuscaPorCodigoUsuario(codigoFuncionario);
            {
                TextBoxNomeFuncionario.Text = usuario.NOME_USUARIO;
                DropDownListPerfil.SelectedValue = usuario.CODIGO_PERFIL.ToString();
                TextBoxUsuario.Text = usuario.USUARIO1;
                TextBoxSenha.Text = usuario.SENHA;
                TextBoxEmail.Text = usuario.EMAIL;
                HiddenFieldCodigofuncionario.Value = usuario.CODIGO_USUARIO.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                USUARIO usuario;
                if (Convert.ToInt32(HiddenFieldCodigofuncionario.Value) > 0)
                    usuario = usuarioController.BuscaPorCodigoUsuario(Convert.ToInt32(HiddenFieldCodigofuncionario.Value));
                else
                {
                    USUARIO userOld = usuarioController.BuscaPorUsuario(TextBoxUsuario.Text);

                    if (userOld != null)
                        throw new Exception("Usuário já existe!");

                    usuario = new USUARIO();
                }

                usuario.NOME_USUARIO = TextBoxNomeFuncionario.Text;
                usuario.CODIGO_PERFIL = Convert.ToInt32(DropDownListPerfil.SelectedValue);
                usuario.USUARIO1 = TextBoxUsuario.Text;
                usuario.SENHA = TextBoxSenha.Text;
                usuario.EMAIL = TextBoxEmail.Text;

                if (Convert.ToInt32(HiddenFieldCodigofuncionario.Value) > 0)
                    usuarioController.Atualiza(usuario);
                else
                    usuarioController.Insere(usuario);

                LabelFeedBack.Text = "Gravado com sucesso!";
                CarregaGridViewUsuario();
                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            USUARIO usuario = e.Row.DataItem as USUARIO;

            if (usuario != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;
                if (buttonEditar != null)
                    buttonEditar.CommandArgument = usuario.CODIGO_USUARIO.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;
                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = usuario.CODIGO_USUARIO.ToString();

                Literal literalPerfil = e.Row.FindControl("LiteralPerfil") as Literal;

                if (literalPerfil != null)
                    literalPerfil.Text = perfilController.BuscaPorCodigoPerfil(usuario.CODIGO_PERFIL).DESCRICAO_PERFIL.ToString();
            }
        }

        private void CarregaGridViewUsuario()
        {
            GridViewUsuario.DataSource = usuarioController.BuscaUsuarios();
            GridViewUsuario.DataBind();
        }

        protected void ButtonExcluirUsuario_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;
            if (buttonExcluir != null)
            {
                int retorno = usuarioController.Exclui(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (retorno == 1)
                    LabelFeedBack.Text = "Existe Loja Vinculado a este Staff !!!";

                CarregaGridViewUsuario();
                LimpaTela();
            }
        }

        protected void ButtonEditarUsuario_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;
            if (buttonEditar != null)
            {
                CarregaFuncionario(Convert.ToInt32(buttonEditar.CommandArgument));
                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            TextBoxNomeFuncionario.Text = string.Empty;
            HiddenFieldCodigofuncionario.Value = "0";
            TextBoxUsuario.Text = string.Empty;
            TextBoxSenha.Text = string.Empty;
            TextBoxEmail.Text = string.Empty;
        }

        protected void DropDownListPerfil_DataBound(object sender, EventArgs e)
        {
            DropDownListPerfil.Items.Add(new ListItem("Selecione", "0"));
            DropDownListPerfil.SelectedValue = "0";
        }
    }
}