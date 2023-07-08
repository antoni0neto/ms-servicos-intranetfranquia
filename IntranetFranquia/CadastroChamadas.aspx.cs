using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class CadastroChamadas : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListRequisitante();
                CarregaDropDownListTipoTelefone();
                CarregaDropDownListTipoDestino();
                CarregaGridViewChamadas();
            }
        }

        private void CarregaDropDownListRequisitante()
        {
            DropDownListRequisitante.DataSource = usuarioController.BuscaUsuarios();
            DropDownListRequisitante.DataBind();
        }

        private void CarregaDropDownListTipoTelefone()
        {
            DropDownListTipoTelefone.DataSource = usuarioController.BuscaTiposTelefone();
            DropDownListTipoTelefone.DataBind();
        }

        private void CarregaDropDownListTipoDestino()
        {
            DropDownListTipoDestino.DataSource = usuarioController.BuscaTiposDestino();
            DropDownListTipoDestino.DataBind();
        }

        protected void CalendarDataChamada_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataChamada.Text = CalendarDataChamada.SelectedDate.ToString("dd/MM/yyyy");
        }

        private void CarregaChamada(int codigoChamada)
        {
            TELEFONIA_CONTROLE_CHAMADA controleChamada = usuarioController.BuscaPorCodigoChamada(codigoChamada);

            if (controleChamada != null)
            {
                DropDownListRequisitante.SelectedValue = controleChamada.CODIGO_USUARIO_LIGACAO.ToString();
                TextBoxDataChamada.Text = controleChamada.DATA_OCORRENCIA.ToString("dd/MM/yyyy");
                TextBoxDDD.Text = controleChamada.DDD.ToString();
                TextBoxTelefone.Text = controleChamada.TELEFONE;
                TextBoxDestino.Text = controleChamada.DESTINO;
                DropDownListTipoTelefone.SelectedValue = controleChamada.CODIGO_TIPO_TELEFONE.ToString();
                DropDownListTipoDestino.SelectedValue = controleChamada.CODIGO_TIPO_DESTINO.ToString();
                HiddenFieldCodigoChamada.Value = controleChamada.CODIGO.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                TELEFONIA_CONTROLE_CHAMADA controleChamada;

                if (Convert.ToInt32(HiddenFieldCodigoChamada.Value) > 0)
                    controleChamada = usuarioController.BuscaPorCodigoChamada(Convert.ToInt32(HiddenFieldCodigoChamada.Value));
                else
                    controleChamada = new TELEFONIA_CONTROLE_CHAMADA();

                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario != null)
                    controleChamada.CODIGO_USUARIO = usuario.CODIGO_USUARIO;

                controleChamada.CODIGO_USUARIO_LIGACAO = Convert.ToInt32(DropDownListRequisitante.SelectedValue);
                controleChamada.DATA = DateTime.Now;
                controleChamada.DATA_OCORRENCIA = Convert.ToDateTime(TextBoxDataChamada.Text);
                
                if (!TextBoxDDD.Text.Equals(""))
                    controleChamada.DDD = Convert.ToInt32(TextBoxDDD.Text);

                controleChamada.TELEFONE = TextBoxTelefone.Text;
                controleChamada.DESTINO = TextBoxDestino.Text;
                controleChamada.CODIGO_TIPO_TELEFONE = Convert.ToInt32(DropDownListTipoTelefone.SelectedValue);
                controleChamada.CODIGO_TIPO_DESTINO = Convert.ToInt32(DropDownListTipoDestino.SelectedValue);

                if (Convert.ToInt32(HiddenFieldCodigoChamada.Value) > 0)
                    usuarioController.AtualizaChamada(controleChamada);
                else
                    usuarioController.InsereChamada(controleChamada);

                LabelFeedBack.Text = "Gravado com sucesso!";

                CarregaGridViewChamadas();

                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewChamadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TELEFONIA_CONTROLE_CHAMADA controleChamada = e.Row.DataItem as TELEFONIA_CONTROLE_CHAMADA;

            if (controleChamada != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;

                if (buttonEditar != null)
                    buttonEditar.CommandArgument = controleChamada.CODIGO.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = controleChamada.CODIGO.ToString();

                Literal literalRequisitante = e.Row.FindControl("LiteralRequisitante") as Literal;

                if (literalRequisitante != null)
                    literalRequisitante.Text = usuarioController.BuscaPorCodigoUsuario(controleChamada.CODIGO_USUARIO_LIGACAO).NOME_USUARIO;

                Literal literalTipoTelefone = e.Row.FindControl("LiteralTipoTelefone") as Literal;

                if (literalTipoTelefone != null)
                    literalTipoTelefone.Text = usuarioController.BuscaTipoTelefone(controleChamada.CODIGO_TIPO_TELEFONE).DESCRICAO;

                Literal literalTipoDestino = e.Row.FindControl("LiteralTipoDestino") as Literal;

                if (literalTipoDestino != null)
                    literalTipoDestino.Text = usuarioController.BuscaTipoDestino(controleChamada.CODIGO_TIPO_DESTINO).DESCRICAO;
            }
        }

        private void CarregaGridViewChamadas()
        {
            GridViewChamadas.DataSource = usuarioController.BuscaChamadas();
            GridViewChamadas.DataBind();
        }

        protected void ButtonExcluirChamada_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;

            if (buttonExcluir != null)
            {
                usuarioController.ExcluiChamada(Convert.ToInt32(buttonExcluir.CommandArgument));

                CarregaGridViewChamadas();

                LimpaTela();
            }
        }

        protected void ButtonEditarChamada_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;

            if (buttonEditar != null)
            {
                CarregaChamada(Convert.ToInt32(buttonEditar.CommandArgument));

                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            HiddenFieldCodigoChamada.Value = "0";
            TextBoxDataChamada.Text = string.Empty;
            TextBoxDDD.Text = string.Empty;
            TextBoxTelefone.Text = string.Empty;
            TextBoxDestino.Text = string.Empty;
        }

        protected void DropDownListRequisitante_DataBound(object sender, EventArgs e)
        {
            DropDownListRequisitante.Items.Add(new ListItem("Selecione", "0"));
            DropDownListRequisitante.SelectedValue = "0";
        }

        protected void DropDownListTipoTelefone_DataBound(object sender, EventArgs e)
        {
            DropDownListTipoTelefone.Items.Add(new ListItem("Selecione", "0"));
            DropDownListTipoTelefone.SelectedValue = "0";
        }

        protected void DropDownListTipoDestino_DataBound(object sender, EventArgs e)
        {
            DropDownListTipoDestino.Items.Add(new ListItem("Selecione", "0"));
            DropDownListTipoDestino.SelectedValue = "0";
        }
    }
}