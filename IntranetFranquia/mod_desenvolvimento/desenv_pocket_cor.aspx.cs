using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Data.OleDb;

namespace Relatorios
{
    public partial class desenv_pocket_cor : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //Carregar e gv de cores
                CarregarCoresTripa();
                CarregarCores();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        #region "INICIALIZAR DADOS"
        private void CarregarCores()
        {
            List<DESENV_CORE> _cores = new List<DESENV_CORE>();

            try
            {
                _cores = desenvController.ObterCorPocket().OrderBy(p => p.COR).ToList();

                _cores = _cores.GroupBy(x => new { COR = x.COR, IMAGEM = x.IMAGEM }).Select(p =>
                                                                                        new DESENV_CORE
                                                                                        {
                                                                                            COR = p.Key.COR,
                                                                                            IMAGEM = p.Key.IMAGEM
                                                                                        }).ToList();
                if (_cores != null)
                {
                    gvCores.DataSource = _cores;
                    gvCores.DataBind();
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvCores): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labCor.ForeColor = Color.Black;
            labFotoTecido.ForeColor = Color.Black;
            btCancelar.Visible = false;
            ddlCorFornecedor.SelectedValue = "Selecione";

            CarregarCores();
        }
        private void CarregarCoresTripa()
        {
            try
            {

                List<DESENV_PRODUTO> coresTripa = new List<DESENV_PRODUTO>();
                coresTripa = desenvController.ObterProduto().ToList();
                coresTripa = coresTripa.Where(k => k.FORNECEDOR_COR != null && k.FORNECEDOR_COR.Trim() != "").GroupBy(
                                                p => new { FORNECEDOR_COR = p.FORNECEDOR_COR.Trim() }).Select(j => new DESENV_PRODUTO { FORNECEDOR_COR = j.Key.FORNECEDOR_COR.Trim().ToUpper() }).OrderBy(p => p.FORNECEDOR_COR).ToList();

                if (coresTripa != null && coresTripa.Count() > 0)
                {
                    coresTripa.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "Selecione" });
                    ddlCorFornecedor.DataSource = coresTripa;
                    ddlCorFornecedor.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region "CRUD"
        private void Incluir(DESENV_CORE _cor)
        {
            desenvController.InserirCorPocket(_cor);
        }
        private void Editar(DESENV_CORE _cor)
        {
            desenvController.AtualizarCorPocket(_cor);
        }
        private void Excluir(string _cor)
        {
            desenvController.ExcluirCorPocket(_cor);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                bool _Inclusao;

                labErro.Text = "";
                if (ddlCorFornecedor.SelectedValue.Trim() == "" || ddlCorFornecedor.SelectedValue == "Selecione")
                {
                    labErro.Text = "Selecione a descrição da COR.";
                    return;
                }

                if (!upFoto.HasFile)
                {
                    labErro.Text = "Selecione uma foto para esta COR.";
                    return;
                }


                _Inclusao = true;
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                string retorno = GravarImagem().ToUpper();
                if (retorno.Contains("ERRO"))
                    throw new Exception(retorno);

                DESENV_CORE _novo = new DESENV_CORE();
                _novo.COR = ddlCorFornecedor.SelectedValue.ToUpper().Trim();
                _novo.IMAGEM = retorno;
                _novo.DATA_ALTERACAO = DateTime.Now;

                if (_Inclusao)
                {
                    Incluir(_novo);
                }
                else
                {
                    _novo.COR = hidCodigo.Value.ToUpper().Trim();
                    Editar(_novo);
                }

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string cor = b.CommandArgument;
                    DESENV_CORE _cor = desenvController.ObterCorPocket(cor);
                    if (_cor != null)
                    {
                        hidCodigo.Value = _cor.COR;
                        ddlCorFornecedor.SelectedValue = _cor.COR.ToUpper().Trim();
                        labCor.ForeColor = Color.Red;
                        labFotoTecido.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                    else
                    {
                        btCancelar.Visible = false;
                        labCor.ForeColor = Color.Gray;
                        labFotoTecido.ForeColor = Color.Gray;
                        hidCodigo.Value = "";
                        ddlCorFornecedor.SelectedValue = cor.ToUpper().Trim();
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            if (b != null)
            {
                labErro.Text = "";
                try
                {
                    try
                    {
                        Excluir(b.CommandArgument);
                        RecarregarTela();
                    }
                    catch (Exception)
                    {
                        labErro.Text = "A COR não pode ser excluída. Esta COR já está cadastrada em um Produto.";
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion

        private string GravarImagem()
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            Stream _stream = null;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(upFoto.PostedFile.FileName);
                _fileName = Guid.NewGuid() + "_COR" + _ext;
                _path = Server.MapPath("~/Image_POCKET/") + _fileName;

                //Obter stream da imagem
                _stream = upFoto.PostedFile.InputStream;
                if (_stream != null)
                    GenerateThumbnails(1, _stream, _path);

                retornoImagem = "~/Image_POCKET/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 220
                // height 130
                var newWidth = 0;
                var newHeight = 0;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > 220 || newHeight > 130)
                {
                    newWidth = (int)((newWidth) * 0.95);
                    newHeight = (int)((newHeight) * 0.95);
                }

                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        protected void gvCores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_CORE _cores = e.Row.DataItem as DESENV_CORE;

                    coluna += 1;
                    if (_cores != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        System.Web.UI.WebControls.Image _foto = e.Row.FindControl("imgFoto") as System.Web.UI.WebControls.Image;
                        if (_foto != null)
                            _foto.ImageUrl = _cores.IMAGEM;

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = _cores.COR.ToUpper().Trim();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _cores.COR.ToUpper().Trim();
                    }
                }
            }
        }
    }

}
