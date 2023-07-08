using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class adm_cad_usuario : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        PerfilController perfilController = new PerfilController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarPerfil();
                CarregarUsuarios();
            }
        }

        protected void gvUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            USUARIO usuario = e.Row.DataItem as USUARIO;

            if (usuario != null)
            {

                Literal literalPerfil = e.Row.FindControl("litPerfil") as Literal;
                literalPerfil.Text = perfilController.BuscaPorCodigoPerfil(usuario.CODIGO_PERFIL).DESCRICAO_PERFIL;

                Button btEditar = e.Row.FindControl("btEditar") as Button;
                btEditar.CommandArgument = usuario.CODIGO_USUARIO.ToString();

            }
        }
        private void CarregarUsuarios()
        {
            var usuarios = usuarioController.BuscaUsuarios().Where(p => p.DATA_EXCLUSAO == null);
            gvUsuario.DataSource = usuarios;
            gvUsuario.DataBind();
        }

        #region "ACOES"
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                USUARIO usuario = new USUARIO();

                if (Convert.ToInt32(hidCodigoUsuario.Value) > 0)
                    usuario = usuarioController.BuscaPorCodigoUsuario(Convert.ToInt32(hidCodigoUsuario.Value));
                else
                {
                    USUARIO userOld = usuarioController.BuscaPorUsuario(txtUsuario.Text);

                    if (userOld != null)
                    {
                        labErro.Text = "O Usuário informado já está cadastrado.";
                        return;
                    }
                }

                usuario.NOME_USUARIO = txtNome.Text;
                usuario.CODIGO_PERFIL = Convert.ToInt32(ddlPerfil.SelectedValue);
                usuario.USUARIO1 = txtUsuario.Text;
                usuario.SENHA = Criptografia.Encrypt(txtSenha.Text);
                usuario.EMAIL = txtEmail.Text;

                if (Convert.ToInt32(hidCodigoUsuario.Value) > 0)
                    usuarioController.Atualiza(usuario);
                else
                    usuarioController.Insere(usuario);

                labErro.Text = "Usuário cadastrado com sucesso.";
                CarregarUsuarios();

                LimpaTela();
            }
            catch (Exception ex)
            {
                labErro.Text = string.Format("Erro: {0}", ex.Message);
            }
        }
        protected void btEditar_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (bt != null)
            {
                EditarUsuario(Convert.ToInt32(bt.CommandArgument));
                LimpaFeedBack();
            }
        }
        private void EditarUsuario(int codigoUsuario)
        {
            var usuario = usuarioController.BuscaPorCodigoUsuario(codigoUsuario);
            if (usuario != null)
            {
                txtNome.Text = usuario.NOME_USUARIO;
                ddlPerfil.SelectedValue = usuario.CODIGO_PERFIL.ToString();
                txtUsuario.Text = usuario.USUARIO1;
                txtSenha.Text = usuario.SENHA;
                txtEmail.Text = usuario.EMAIL;
                hidCodigoUsuario.Value = usuario.CODIGO_USUARIO.ToString();
            }
        }
        #endregion

        #region "DADOS INICIAS"
        private void CarregarPerfil()
        {
            var perfil = perfilController.BuscaPerfil();

            perfil.Insert(0, new USUARIO_PERFIL { CODIGO_PERFIL = 0, DESCRICAO_PERFIL = "Selecione" });
            ddlPerfil.DataSource = perfil;
            ddlPerfil.DataBind();
        }

        private void LimpaFeedBack()
        {
            labErro.Text = string.Empty;
        }
        private void LimpaTela()
        {
            txtNome.Text = string.Empty;
            hidCodigoUsuario.Value = "0";
            txtUsuario.Text = string.Empty;
            txtSenha.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
        #endregion

    }
}