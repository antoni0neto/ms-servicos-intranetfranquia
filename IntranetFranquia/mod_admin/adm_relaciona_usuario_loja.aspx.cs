using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class adm_relaciona_usuario_loja : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListLoja();
                CarregaDropDownListUsuario();
                CarregaGridViewUsuarioLoja();
            }
        }

        private void CarregaDropDownListLoja()
        {
            DropDownListLoja.DataSource = lojaController.BuscaLojas();
            DropDownListLoja.DataBind();
        }

        private void CarregaDropDownListUsuario()
        {
            DropDownListUsuario.DataSource = usuarioController.BuscaUsuarios().Where(p => p.DATA_EXCLUSAO == null);
            DropDownListUsuario.DataBind();
        }

        private void CarregaGridViewUsuarioLoja()
        {
            USUARIO usuario = null;

            if (Session["USUARIO"] != null)
                usuario = (USUARIO)Session["USUARIO"];
            else
                Response.Redirect("~/Login.aspx");

            var usuarioLoja = lojaController.BuscaLojas(0).OrderBy(p => p.USUARIO.NOME_USUARIO);

            GridViewLoja.DataSource = usuarioLoja;
            GridViewLoja.DataBind();
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            USUARIOLOJA usuarioLoja;

            if (Convert.ToInt32(HiddenFieldCodigoUsuarioLoja.Value) > 0)
            {
                usuarioLoja = lojaController.BuscaStaffPorCodigoLoja(HiddenFieldCodigoUsuarioLoja.Value);

                if (usuarioLoja == null)
                {
                    usuarioLoja = new USUARIOLOJA();
                    usuarioLoja.CODIGO_USUARIOLOJA = Convert.ToInt32(HiddenFieldCodigoUsuarioLoja.Value);
                }
            }
            else
                usuarioLoja = new USUARIOLOJA();

            usuarioLoja.CODIGO_USUARIO = Convert.ToInt32(DropDownListUsuario.SelectedValue);
            usuarioLoja.CODIGO_LOJA = DropDownListLoja.SelectedValue;

            try
            {
                if (Convert.ToInt32(HiddenFieldCodigoUsuarioLoja.Value) > 0)
                    lojaController.Atualiza(usuarioLoja);
                else
                    lojaController.Insere(usuarioLoja);

                LabelFeedBack.Text = "Gravado com sucesso!";
                CarregaGridViewUsuarioLoja();
                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewLoja_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                USUARIOLOJA usuarioLoja = e.Row.DataItem as USUARIOLOJA;

                if (usuarioLoja != null)
                {
                    Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;

                    if (literalFilial != null)
                    {
                        FILIAI filial = lojaController.BuscaPorCodigoLoja(usuarioLoja.CODIGO_LOJA);

                        if (filial != null)
                            literalFilial.Text = filial.FILIAL;
                    }

                    Literal literalUsuario = e.Row.FindControl("LiteralUsuario") as Literal;

                    if (literalUsuario != null)
                    {
                        USUARIO usuario = usuarioController.BuscaPorCodigoUsuario(usuarioLoja.CODIGO_USUARIO);

                        if (usuario != null)
                            literalUsuario.Text = usuario.NOME_USUARIO;
                    }

                    Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;
                    if (buttonEditar != null)
                        buttonEditar.CommandArgument = usuarioLoja.CODIGO_USUARIOLOJA.ToString();

                    Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;
                    if (buttonExcluir != null)
                        buttonExcluir.CommandArgument = usuarioLoja.CODIGO_USUARIOLOJA.ToString();
                }
            }
        }

        protected void ButtonExcluir_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;
            if (buttonExcluir != null)
            {
                int retorno = lojaController.Exclui(Convert.ToInt32(buttonExcluir.CommandArgument));

                if (retorno == 1)
                    LabelFeedBack.Text = "Existe Funcionário de Loja Vinculado a esta Loja !!!";

                if (retorno == 2)
                    LabelFeedBack.Text = "Existe Candidato Vinculado a esta Loja !!!";

                CarregaGridViewUsuarioLoja();
                LimpaTela();
            }
        }

        protected void ButtonEditar_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;
            if (buttonEditar != null)
            {
                CarregaLoja(Convert.ToInt32(buttonEditar.CommandArgument));
                LimpaFeedBack();
            }
        }

        private void CarregaLoja(int codigoUsuarioLoja)
        {
            USUARIOLOJA usuarioLoja = lojaController.BuscaPorCodigoUsuarioLoja(codigoUsuarioLoja);

            if (usuarioLoja != null)
            {
                DropDownListUsuario.SelectedValue = usuarioLoja.CODIGO_USUARIO.ToString();
                DropDownListLoja.SelectedValue = usuarioLoja.CODIGO_LOJA;

                HiddenFieldCodigoUsuarioLoja.Value = usuarioLoja.CODIGO_USUARIOLOJA.ToString();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            HiddenFieldCodigoUsuarioLoja.Value = "0";
        }

        protected void DropDownListUsuario_DataBound(object sender, EventArgs e)
        {
            DropDownListUsuario.Items.Add(new ListItem("Selecione", "0"));
            DropDownListUsuario.SelectedValue = "0";
        }

        protected void DropDownListLoja_DataBound(object sender, EventArgs e)
        {
            DropDownListLoja.Items.Add(new ListItem("Selecione", "0"));
            DropDownListLoja.SelectedValue = "0";
        }
    }
}