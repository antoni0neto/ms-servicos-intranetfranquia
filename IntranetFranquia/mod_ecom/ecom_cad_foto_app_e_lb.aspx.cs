using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class ecom_cad_foto_app_e_lb : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        EcomController eController = new EcomController();

        string gCodCategoria = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarDiretorios();
                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarDiretorios()
        {
            var dir = eController.ObterProdutoFotoSemSelecaoAppLB();
            dir.Insert(0, new SP_OBTER_ECOM_FOTO_SEM_SELECAO_APPLBResult { DIRETORIO_ORIGEM = "", DIRETORIO_DESC = "Selecione" });
            ddlProduto.DataSource = dir;
            ddlProduto.DataBind();
        }

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }
        private void Limpar()
        {
            ddlProduto.SelectedValue = "";
            ddlProduto.Enabled = true;

            gvProduto.DataSource = new List<ListItem>();
            gvProduto.DataBind();

            btSalvar.Enabled = false;

            labErro.Text = "";
            labErroDir.Text = "";

            gvProduto.Columns[2].ItemStyle.BackColor = Color.White;

            imgProdutoDesenv.ImageUrl = "";

            CarregarDiretorios();

            CarregarJQuery();
        }

        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ListItem file = e.Row.DataItem as ListItem;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = file.Value;

                    Literal _litDiretorio = e.Row.FindControl("litDiretorio") as Literal;
                    _litDiretorio.Text = file.Text;

                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvProduto.FooterRow;
            if (_footer != null)
            {
            }
        }
        #endregion

        protected void cbLook_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (CheckBox)sender;
            int coluna = 0;

            if (cb.ID.ToLower().Contains("look"))
                coluna = 2;

            if (cb.Checked)
                gvProduto.Columns[coluna].ItemStyle.BackColor = Color.AliceBlue;
            else
                gvProduto.Columns[coluna].ItemStyle.BackColor = Color.White;

            CarregarJQuery();
        }

        protected void btAbrirDiretorio_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                labErroDir.Text = "";

                string path = Server.MapPath("~/FotoLookBookGeral") + "\\";

                string produto = ddlProduto.SelectedValue.Trim();

                string filesDirectory = path + produto;
                string filesDirectorySmall = path + produto + "\\small";

                //DIRETORIO BIG
                if (!Directory.Exists(filesDirectory))
                {
                    labErroDir.Text = "Diretório não encontrado...";
                    return;
                }
                string[] filePaths = Directory.GetFiles(filesDirectory);
                if (filePaths == null || filePaths.Count() <= 0)
                {
                    labErroDir.Text = "Diretório não possui fotos. (" + filesDirectory + ")";
                    return;
                }
                if (filePaths.Count() == 1 && filePaths[0].ToLower().Contains("thumbs.db"))
                {
                    labErroDir.Text = "Diretório não possui fotos. (" + filesDirectory + ")";
                    return;
                }

                //DIRETORIO SMALL
                if (!Directory.Exists(filesDirectorySmall))
                {
                    labErroDir.Text = "Diretório Small não encontrado...";
                    return;
                }
                string[] filePathsSmall = Directory.GetFiles(filesDirectorySmall);
                if (filePathsSmall == null || filePathsSmall.Count() <= 0)
                {
                    labErroDir.Text = "Diretório não possui fotos. (" + filesDirectorySmall + ")";
                    return;
                }
                if (filePathsSmall.Count() == 1 && filePathsSmall[0].ToLower().Contains("thumbs.db"))
                {
                    labErroDir.Text = "Diretório não possui fotos. (" + filesDirectorySmall + ")";
                    return;
                }

                List<ListItem> files = new List<ListItem>();
                foreach (string filePath in filePaths)
                {
                    if (filePath.Contains(".jpg"))
                    {
                        files.Add(new ListItem(filePath, "~/FotoLookBookGeral/" + produto + "/small/" + Path.GetFileName(filePath)));
                    }
                }

                gCodCategoria = baseController.BuscaProduto(produto.Substring(0, 5)).COD_CATEGORIA.Trim();

                gvProduto.DataSource = files;
                gvProduto.DataBind();

                gCodCategoria = "";

                //carregar foto do desenv_produto
                var desenvProduto = desenvController.ObterProdutoFoto(produto.Substring(0, 5), produto.Substring(5));
                if (desenvProduto != null)
                    imgProdutoDesenv.ImageUrl = desenvProduto.FOTO;
                else
                    imgProdutoDesenv.ImageUrl = "";

                btSalvar.Enabled = true;
                ddlProduto.Enabled = false;

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErroDir.Text = ex.Message;
            }


        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroDir.Text = "";

                CarregarJQuery();

                string produtocor = ddlProduto.SelectedValue.Trim();
                string produto = produtocor.Substring(0, 5);

                var produtoLinx = baseController.BuscaProduto(produto);
                string grupoProduto = produtoLinx.GRUPO_PRODUTO.Trim();
                string codCategoria = produtoLinx.COD_CATEGORIA.Trim();

                if (grupoProduto == null || grupoProduto == "")
                {
                    labErro.Text = "Produto não encontrado... Verifique os 5 primeiros dígitos...";
                    return;
                }

                if (!VerificarCheckbok())
                {
                    labErro.Text = "Selecione apenas uma foto...";
                    return;
                }

                string diretorioPadraoLB = Server.MapPath("~/FotoLookBook/FotoTratamento") + "\\";
                string diretorioPadraoApp = Server.MapPath("~/FotoLookBookApp/") + "\\";

                bool look = false;

                foreach (GridViewRow row in gvProduto.Rows)
                {
                    string file = "";

                    file = ((Literal)row.FindControl("litDiretorio")).Text.Trim();

                    look = ((CheckBox)row.FindControl("cbLook")).Checked;

                    if (File.Exists(file))
                    {
                        if (look)
                        {
                            File.Copy(file, diretorioPadraoLB + "\\" + produtocor + ".jpg", true);
                            File.Copy(file, diretorioPadraoApp + "\\" + produtocor + ".jpg", true);
                            //File.Copy(diretorioDestino + "\\" + produtocor + "L.jpg", diretorioDestinoBKP + "\\" + produtocor + "L.jpg", true);
                        }

                    }

                    look = false;
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            Limpar();
        }
        private bool VerificarCheckbok()
        {
            bool look = false;

            int fotoCount = 0;

            foreach (GridViewRow row in gvProduto.Rows)
            {
                look = ((CheckBox)row.FindControl("cbLook")).Checked;

                if (look)
                    fotoCount += 1;
            }

            if (fotoCount == 1)
                return true;

            return false;
        }
    }
}

