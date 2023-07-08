using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class UploadImagemProduto : System.Web.UI.Page
    {
        ImagemController imagemController = new ImagemController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaGridViewImagem();
                CarregaDropDownListColecao();
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListGrupo(string colecao)
        {
            ddlGrupo.DataSource = baseController.BuscaGruposProduto("01", colecao);
            ddlGrupo.DataBind();
        }

        private void CarregaDropDownListProduto(string grupo)
        {
            ddlProduto.DataSource = baseController.BuscaProdutos(ddlColecao.SelectedValue.ToString().Trim(), grupo);
            ddlProduto.DataBind();
        }

        private void CarregaDropDownListCor(string codigoProduto)
        {
            ddlCor.DataSource = baseController.BuscaProdutoCores(codigoProduto);
            ddlCor.DataBind();
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlProduto_DataBound(object sender, EventArgs e)
        {
            ddlProduto.Items.Add(new ListItem("Selecione", "0"));
            ddlProduto.SelectedValue = "0";
        }

        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";
        }

        protected void ddlCor_DataBound(object sender, EventArgs e)
        {
            ddlCor.Items.Add(new ListItem("Selecione", "0"));
            ddlCor.SelectedValue = "0";
        }

        private void CarregaGridViewImagem()
        {
            GridViewImagem.DataSource = imagemController.BuscaImagensProduto();
            GridViewImagem.DataBind();
        }

        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaDropDownListGrupo(ddlColecao.SelectedValue.ToString().Trim());
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaDropDownListProduto(ddlGrupo.SelectedValue.ToString().Trim());
        }

        protected void ddlProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaDropDownListCor(ddlProduto.SelectedValue.ToString().Trim());
        }

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("Selecione") ||
                ddlGrupo.SelectedValue.ToString().Equals("Selecione") ||
                ddlProduto.SelectedValue.ToString().Equals("Selecione"))
                return;

            IMAGEM_PRODUTO imagem = new IMAGEM_PRODUTO();

            if (ddlCor.SelectedValue.ToString().Equals("Selecione"))
                imagem.CODIGO_PRODUTO = Convert.ToInt32(ddlProduto.SelectedValue);
            else
                imagem.CODIGO_PRODUTO = Convert.ToInt32(ddlProduto.SelectedValue.Trim() + ddlCor.SelectedValue.Trim());

            imagem.DESCRICAO_PRODUTO = ddlProduto.SelectedItem.ToString();

            if (FileUpload1.HasFile)
                imagem.LOCAL_IMAGEM_PRODUTO = FileUpload1.FileName;
            else
                imagem.LOCAL_IMAGEM_PRODUTO = "";

            imagem.ATIVO = CheckBoxAtivo.Checked;

            try
            {
                imagemController.InsereImagemProduto(imagem);

                LabelFeedBack.Text = "Gravado com sucesso!";
                CarregaGridViewImagem();
            }
            catch (Exception ex)
            {
                LabelFeedBack.Text = string.Format("Erro: {0}", ex.Message);
            }
        }

        protected void GridViewImagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMAGEM_PRODUTO imagem = e.Row.DataItem as IMAGEM_PRODUTO;

            if (imagem != null)
            {
                Button buttonExcluir = e.Row.FindControl("ButtonExcluir") as Button;

                if (buttonExcluir != null)
                    buttonExcluir.CommandArgument = imagem.CODIGO_IMAGEM_PRODUTO.ToString();
            }
        }

        protected void ButtonExcluir_Click(object sender, EventArgs e)
        {
            Button buttonExcluir = sender as Button;
            if (buttonExcluir != null)
            {
                imagemController.ExcluirProduto(Convert.ToInt32(buttonExcluir.CommandArgument));

                CarregaGridViewImagem();
            }
        }

        private void LimpaFeedBack()
        {
            LabelFeedBack.Text = string.Empty;
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