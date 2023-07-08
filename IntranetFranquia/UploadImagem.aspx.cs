using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class UploadImagem : System.Web.UI.Page
    {
        ImagemController imagemController = new ImagemController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaGridViewImagem();
            }
        }

        private void CarregaGridViewImagem()
        {
            GridViewImagem.DataSource = imagemController.BuscaImagens(Convert.ToInt32(Session["CODIGO_LOJA"]));
            GridViewImagem.DataBind();
        }

        private void CarregaImagem(int codigoImagem)
        {
            IMAGEM imagem = imagemController.BuscaPorCodigoImagem(codigoImagem);
            {
                TextBoxNomeImagem.Text = imagem.NOME_IMAGEM;

                if (imagem.ATIVO == true)
                    CheckBoxAtivo.Checked = true;
                else
                    CheckBoxAtivo.Checked = false;

                HiddenFieldCodigoimagem.Value = imagem.CODIGO_IMAGEM.ToString();
            }
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            IMAGEM imagem;

            if (Convert.ToInt32(HiddenFieldCodigoimagem.Value) > 0)
                imagem = imagemController.BuscaPorCodigoImagem(Convert.ToInt32(HiddenFieldCodigoimagem.Value));
            else
                imagem = new IMAGEM();

            imagem.NOME_IMAGEM = TextBoxNomeImagem.Text;

            if (FileUpload1.HasFile)
                imagem.LOCAL_IMAGEM = FileUpload1.FileName;
            else
                imagem.LOCAL_IMAGEM = "";

            imagem.ATIVO = CheckBoxAtivo.Checked;
            imagem.CODIGO_LOJA = Convert.ToInt32(Session["CODIGO_LOJA"]);

            try
            {
                if (Convert.ToInt32(HiddenFieldCodigoimagem.Value) > 0)
                    imagemController.Atualiza(imagem);
                else
                    imagemController.Insere(imagem);

                LabelFeedBack.Text = "Gravado com sucesso!";
                CarregaGridViewImagem();
                LimpaTela();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewImagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMAGEM imagem = e.Row.DataItem as IMAGEM;
            if (imagem != null)
            {
                Button buttonEditar = e.Row.FindControl("ButtonEditar") as Button;
                if (buttonEditar != null)
                    buttonEditar.CommandArgument = imagem.CODIGO_IMAGEM.ToString();

                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;
                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = imagem.CODIGO_IMAGEM.ToString();
            }
        }

        protected void ButtonExcluir_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;
            if (buttonExcluir != null)
            {
                imagemController.Exclui(Convert.ToInt32(buttonExcluir.CommandArgument));

                CarregaGridViewImagem();
                LimpaTela();
            }
        }

        protected void ButtonEditar_Click(object sender, EventArgs e)
        {
            Button buttonEditar = sender as Button;
            if (buttonEditar != null)
            {
                CarregaImagem(Convert.ToInt32(buttonEditar.CommandArgument));
                LimpaFeedBack();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
        }

        private void LimpaTela()
        {
            TextBoxNomeImagem.Text = string.Empty;
            CheckBoxAtivo.Checked = false;
            HiddenFieldCodigoimagem.Value = "0";
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            string savePath = Server.MapPath("~/Upload/");

            if (FileUpload1.HasFile)
            {
                String fileName = FileUpload1.FileName;

                savePath += fileName;

                FileUpload1.SaveAs(savePath);

                LabelFeedBack.Text = "Arquivo Carregado " + fileName;
            }
        }
    }
}