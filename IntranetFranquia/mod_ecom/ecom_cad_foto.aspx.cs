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
    public partial class ecom_cad_foto : System.Web.UI.Page
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
            var dir = eController.ObterProdutoFotoSemSelecao();
            dir.Insert(0, new SP_OBTER_ECOM_FOTO_SEM_SELECAOResult { DIRETORIO_ORIGEM = "", DIRETORIO_DESC = "Selecione" });
            ddlProduto.DataSource = dir;
            ddlProduto.DataBind();
        }
        private void CarregarDiretoriosOld()
        {
            string pathSource = Server.MapPath("~/FotosHandbookOnlineGeral");
            var dirSource = new DirectoryInfo(pathSource).GetDirectories();

            string pathDest = Server.MapPath("~/FotosHandbookTratamento");
            var dirDest = new DirectoryInfo(pathDest).GetDirectories();

            List<ListItem> directories = new List<ListItem>();
            string nomeDiretorio = "";
            string nomeDiretorioDestino = "";
            string produto = "";

            directories.Add(new ListItem("", ""));
            foreach (var d in dirSource)
            {
                nomeDiretorio = d.Name;
                produto = d.Name.Substring(0, 5);
                var prodLinx = baseController.BuscaProduto(produto);
                if (prodLinx != null)
                {
                    nomeDiretorioDestino = prodLinx.GRUPO_PRODUTO.Trim() + "-" + nomeDiretorio;

                    var dirExists = dirDest.Where(p => p.Name == nomeDiretorioDestino).SingleOrDefault();
                    if (dirExists == null)
                    {
                        if (prodLinx != null)
                            nomeDiretorio = nomeDiretorio + "-" + prodLinx.DESC_PRODUTO.Trim();

                        var c = prodController.ObterCoresBasicas(d.Name.Substring(5));
                        if (c != null)
                            nomeDiretorio = nomeDiretorio + "-" + c.DESC_COR.Trim();

                        directories.Add(new ListItem(nomeDiretorio, d.Name));
                    }
                    else
                    {
                        var fileExists = dirExists.GetFiles();

                        if (fileExists == null || fileExists.Count() <= 0 || (fileExists.Count() == 1 && fileExists[0].FullName.ToLower().Contains("thumbs.db")))
                        {

                            if (prodLinx != null)
                                nomeDiretorio = nomeDiretorio + "-" + prodLinx.DESC_PRODUTO.Trim();

                            var c = prodController.ObterCoresBasicas(d.Name.Substring(5));
                            if (c != null)
                                nomeDiretorio = nomeDiretorio + "-" + c.DESC_COR.Trim();

                            directories.Add(new ListItem(nomeDiretorio, d.Name));
                        }
                    }
                }
            }

            ddlProduto.DataSource = directories;
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
            gvProduto.Columns[3].ItemStyle.BackColor = Color.White;
            gvProduto.Columns[4].ItemStyle.BackColor = Color.White;
            gvProduto.Columns[5].ItemStyle.BackColor = Color.White;
            gvProduto.Columns[6].ItemStyle.BackColor = Color.White;
            gvProduto.Columns[7].ItemStyle.BackColor = Color.White;

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

                    CheckBox _cbLook = e.Row.FindControl("cbLook") as CheckBox;
                    CheckBox _cbFrenteCabeca = e.Row.FindControl("cbFrenteCabeca") as CheckBox;
                    CheckBox _cbFrenteSemCabeca = e.Row.FindControl("cbFrenteSemCabeca") as CheckBox;
                    CheckBox _cbCostas = e.Row.FindControl("cbCostas") as CheckBox;
                    CheckBox _cbDetalhe = e.Row.FindControl("cbDetalhe") as CheckBox;
                    CheckBox _cbLado = e.Row.FindControl("cbLado") as CheckBox;

                    if (gCodCategoria == "02")
                    {
                        _cbLook.Enabled = false;
                        _cbFrenteSemCabeca.Enabled = false;
                        _cbLado.Enabled = false;
                    }
                    else
                    {
                        _cbLook.Enabled = true;
                        _cbFrenteSemCabeca.Enabled = true;
                        _cbLado.Enabled = true;
                    }

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
            else if (cb.ID.ToLower().Contains("frentecabeca"))
                coluna = 3;
            else if (cb.ID.ToLower().Contains("frentesemcabeca"))
                coluna = 4;
            else if (cb.ID.ToLower().Contains("costas"))
                coluna = 5;
            else if (cb.ID.ToLower().Contains("detalhe"))
                coluna = 6;
            else if (cb.ID.ToLower().Contains("lado"))
                coluna = 7;

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

                string path = Server.MapPath("~/FotosHandbookOnlineGeral") + "\\";

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
                        files.Add(new ListItem(filePath, "~/FotosHandbookOnlineGeral/" + produto + "/small/" + Path.GetFileName(filePath)));
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
                string griffe = produtoLinx.GRIFFE.Trim();
                string codCategoria = produtoLinx.COD_CATEGORIA.Trim();

                if (grupoProduto == null || grupoProduto == "")
                {
                    labErro.Text = "Produto não encontrado... Verifique os 5 primeiros dígitos...";
                    return;
                }

                if (!VerificarCheckbok(codCategoria))
                {
                    labErro.Text = "Selecione as fotos...";
                    return;
                }

                // CONTROLE DE DIRETORIOS

                //string pathBaseBKP = @"\\192.168.1.25\Marketing\Marketing " + DateTime.Now.Year.ToString();
                //if (!Directory.Exists(pathBaseBKP))
                //    Directory.CreateDirectory(pathBaseBKP);

                //string pathBackup = pathBaseBKP + @"\ECOMMERCE\BKP_TRATAMENTO\";
                //if (!Directory.Exists(pathBackup))
                //    Directory.CreateDirectory(pathBackup);

                string diretorioPadrao = Server.MapPath("~/FotosHandbookTratamento") + "\\";

                string diretorioDestino = "";
                //string diretorioDestinoBKP = "";
                if (!Directory.Exists(diretorioPadrao + griffe + "_" + grupoProduto + "-" + produtocor))
                {
                    var diretorioCriado = Directory.CreateDirectory(diretorioPadrao + griffe + "_" + grupoProduto + "-" + produtocor);

                    diretorioDestino = diretorioCriado.FullName;
                }
                else
                {
                    diretorioDestino = (diretorioPadrao + griffe + "_" + grupoProduto + "-" + produtocor);
                }

                bool frenteCabeca = false;
                bool frenteSemCabeca = false;
                bool costas = false;
                bool look = false;
                bool detalhe = false;
                bool lado = false;

                foreach (GridViewRow row in gvProduto.Rows)
                {
                    string file = "";

                    file = ((Literal)row.FindControl("litDiretorio")).Text.Trim();

                    look = ((CheckBox)row.FindControl("cbLook")).Checked;
                    frenteCabeca = ((CheckBox)row.FindControl("cbFrenteCabeca")).Checked;
                    frenteSemCabeca = ((CheckBox)row.FindControl("cbFrenteSemCabeca")).Checked;
                    costas = ((CheckBox)row.FindControl("cbCostas")).Checked;
                    detalhe = ((CheckBox)row.FindControl("cbDetalhe")).Checked;
                    lado = ((CheckBox)row.FindControl("cbLado")).Checked;

                    if (File.Exists(file))
                    {
                        if (look)
                        {
                            File.Copy(file, diretorioDestino + "\\" + produtocor + "L.jpg", true);
                            //File.Copy(diretorioDestino + "\\" + produtocor + "L.jpg", diretorioDestinoBKP + "\\" + produtocor + "L.jpg", true);
                        }
                        if (frenteCabeca)
                        {
                            File.Copy(file, diretorioDestino + "\\" + produtocor + "F.jpg", true);
                            //File.Copy(diretorioDestino + "\\" + produtocor + "F.jpg", diretorioDestinoBKP + "\\" + produtocor + "F.jpg", true);
                        }
                        if (frenteSemCabeca)
                        {
                            File.Copy(file, diretorioDestino + "\\" + produtocor + "G.jpg", true);
                            //File.Copy(diretorioDestino + "\\" + produtocor + "G.jpg", diretorioDestinoBKP + "\\" + produtocor + "G.jpg", true);
                        }
                        if (costas)
                        {
                            File.Copy(file, diretorioDestino + "\\" + produtocor + "C.jpg", true);
                            //File.Copy(diretorioDestino + "\\" + produtocor + "C.jpg", diretorioDestinoBKP + "\\" + produtocor + "C.jpg", true);
                        }
                        if (detalhe)
                        {
                            File.Copy(file, diretorioDestino + "\\" + produtocor + "D.jpg", true);
                            // File.Copy(diretorioDestino + "\\" + produtocor + "D.jpg", diretorioDestinoBKP + "\\" + produtocor + "D.jpg", true);
                        }
                        if (lado)
                        {
                            File.Copy(file, diretorioDestino + "\\" + produtocor + "A.jpg", true);
                            //File.Copy(diretorioDestino + "\\" + produtocor + "A.jpg", diretorioDestinoBKP + "\\" + produtocor + "A.jpg", true);
                        }
                    }

                    frenteCabeca = false;
                    frenteSemCabeca = false;
                    costas = false;
                    look = false;
                    detalhe = false;
                    lado = false;
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            Limpar();
        }
        private bool VerificarCheckbok(string codCategoria)
        {
            bool frenteCabeca = false;
            bool frenteSemCabeca = false;
            bool costas = false;
            bool look = false;
            bool detalhe = false;
            bool lado = false;

            int foto5 = 0;

            foreach (GridViewRow row in gvProduto.Rows)
            {

                look = ((CheckBox)row.FindControl("cbLook")).Checked;
                frenteCabeca = ((CheckBox)row.FindControl("cbFrenteCabeca")).Checked;
                frenteSemCabeca = ((CheckBox)row.FindControl("cbFrenteSemCabeca")).Checked;
                costas = ((CheckBox)row.FindControl("cbCostas")).Checked;
                detalhe = ((CheckBox)row.FindControl("cbDetalhe")).Checked;
                lado = ((CheckBox)row.FindControl("cbLado")).Checked;

                if (look)
                    foto5 += 1;
                if (frenteCabeca)
                    foto5 += 1;
                if (frenteSemCabeca)
                    foto5 += 1;
                if (costas)
                    foto5 += 1;
                if (detalhe)
                    foto5 += 1;
                if (lado)
                    foto5 += 1;
            }

            if (foto5 >= 5 && codCategoria == "01")
                return true;
            else if (foto5 >= 3 && codCategoria == "02")
                return true;

            return false;
        }
    }
}

