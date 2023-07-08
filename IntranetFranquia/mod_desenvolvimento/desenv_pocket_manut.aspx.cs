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
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class desenv_pocket_manut : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        List<SP_OBTER_GRUPOResult> g_grupo = null;
        List<DESENV_PRODUTO_ORIGEM> g_origem = null;
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Request.Files.Keys.Count > 0)
                {
                    btCarregarFoto_Click(sender, e);
                }
                else
                {
                    //Carregar combos
                    CarregarColecoes();
                    CarregarGrupo();
                    //CarregarCores();

                    //Foco no coleção
                    ddlColecoes.Focus();

                    hidAcao.Value = "N";
                    btCancelar.Visible = false;

                    Session["DROP_IMAGE"] = null;
                    btCarregarFoto.Enabled = false;
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if (window.File && window.FileList && window.FileReader) { var dropZone = document.getElementById('drop_zone'); dropZone.addEventListener('dragover', handleDragOver, false); dropZone.addEventListener('drop', handleDnDFileSelect, false); } else { alert('Sorry! this browser does not support HTML5 File APIs.'); }", true);
        }

        #region "MANUTENÇÃO"
        protected void btBuscarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                labSalvar.Text = "";
                labProduto.Text = "";
                if (ddlColecoesBuscar.SelectedValue.Trim() == "" || ddlColecoesBuscar.SelectedValue.Trim() == "0")
                {
                    labProduto.Text = "Selecione a Coleção.";
                    return;
                }
                if (!cbModeloFiltro.Checked)
                {
                    if ((ddlGrupoBuscar.SelectedValue.Trim() == ""
                        || ddlGrupoBuscar.SelectedValue.Trim() == "0"
                        || ddlGrupoBuscar.SelectedValue.Trim() == "Selecione") &&
                        (txtModeloBuscar.Text.Trim() == ""))
                    {
                        labProduto.Text = "Selecione um GRUPO e/ou informe um MODELO.";
                        return;
                    }
                }

                RecarregarProduto();
                btCancelar_Click(null, null);
                btCarregarFoto.Enabled = false;
                labSalvar.Text = "";
                Session["DROP_IMAGE"] = null;

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labProduto.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    coluna += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();


                        Literal _litGrupo = e.Row.FindControl("litGrupo") as Literal;
                        if (_litGrupo != null)
                            _litGrupo.Text = _produto.GRUPO.Trim();

                        Literal _litOrigem = e.Row.FindControl("litOrigem") as Literal;
                        if (_litOrigem != null)
                            if (_produto.DESENV_PRODUTO_ORIGEM > 0)
                            {
                                string _produtoOrigem = desenvController.ObterProdutoOrigem(Convert.ToInt32(_produto.DESENV_PRODUTO_ORIGEM)).DESCRICAO;
                                _litOrigem.Text = _produtoOrigem.ToString();
                            }

                        Literal _litModelo = e.Row.FindControl("litModelo") as Literal;
                        if (_litModelo != null)
                            _litModelo.Text = (_produto.MODELO == null) ? "" : _produto.MODELO.Trim();

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = (_produto.COR == null) ? "" : _produto.COR.Trim();

                        Literal _litQtdeVarejo = e.Row.FindControl("litQtdeVarejo") as Literal;
                        if (_litQtdeVarejo != null)
                            _litQtdeVarejo.Text = (_produto.QTDE == null) ? "0" : _produto.QTDE.ToString();

                        Literal _litQtdeAtacado = e.Row.FindControl("litQtdeAtacado") as Literal;
                        if (_litQtdeAtacado != null)
                            _litQtdeAtacado.Text = (_produto.QTDE_ATACADO == null) ? "0" : _produto.QTDE_ATACADO.ToString();

                        System.Web.UI.WebControls.Image _imgFoto = e.Row.FindControl("imgFoto") as System.Web.UI.WebControls.Image;
                        if (_imgFoto != null)
                            _imgFoto.ImageUrl = (_produto.FOTO == null) ? "" : _produto.FOTO.ToString();

                        Button _btProdutoSalvar = e.Row.FindControl("btProdutoSalvar") as Button;
                        if (_btProdutoSalvar != null)
                            _btProdutoSalvar.CommandArgument = _produto.CODIGO.ToString();

                        Button _btProdutoExcluir = e.Row.FindControl("btProdutoExcluir") as Button;
                        if (_btProdutoExcluir != null)
                            _btProdutoExcluir.CommandArgument = _produto.CODIGO.ToString();

                        Button _btProdutoAprovar = e.Row.FindControl("btProdutoAprovar") as Button;
                        if (_btProdutoAprovar != null)
                            _btProdutoAprovar.CommandArgument = _produto.CODIGO.ToString();
                    }
                }
            }
        }
        protected void btProdutoEditar_Click(object sender, EventArgs e)
        {
            BaseController baseController = new BaseController();
            string msg = "";

            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    labSalvar.Text = "";

                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(Convert.ToInt32(b.CommandArgument));

                    if (_produto != null && _produto.CODIGO > 0)
                    {
                        GridViewRow _row = (GridViewRow)b.NamingContainer;
                        if (_row != null)
                        {
                            //Preencher campos
                            ddlColecoes.SelectedValue = baseController.BuscaColecaoAtual(_produto.COLECAO).COLECAO;
                            ddlColecoes_SelectedIndexChanged(null, null);

                            if (_produto.DESENV_PRODUTO_ORIGEM > 0)
                                ddlOrigem.SelectedValue = _produto.DESENV_PRODUTO_ORIGEM.ToString();
                            txtCodigoRef.Text = _produto.CODIGO_REF.ToString();

                            var grupo = baseController.BuscaGrupoProduto(_produto.GRUPO);
                            if (grupo != null)
                                ddlGrupo.SelectedValue = grupo.GRUPO_PRODUTO;

                            txtModelo.Text = _produto.MODELO.ToString();
                            txtCor.Text = _produto.COR.ToString();
                            txtCorFornecedor.Text = (_produto.FORNECEDOR_COR == null) ? "" : _produto.FORNECEDOR_COR;
                            txtTecido.Text = (_produto.TECIDO_POCKET == null) ? "" : _produto.TECIDO_POCKET;
                            txtQtdeVarejo.Text = (_produto.QTDE == null) ? "0" : _produto.QTDE.ToString();
                            txtPreco.Text = (_produto.PRECO == null) ? "0" : Convert.ToDecimal(_produto.PRECO).ToString("###,###,##0.00");
                            txtQtdeAtacado.Text = (_produto.QTDE_ATACADO == null) ? "0" : _produto.QTDE_ATACADO.ToString();
                            txtPrecoAtacado.Text = (_produto.PRECO_ATACADO == null) ? "0" : Convert.ToDecimal(_produto.PRECO_ATACADO).ToString("###,###,##0.00");
                            txtObservacao.Text = _produto.OBSERVACAO;
                            imgFoto.ImageUrl = _produto.FOTO;

                            labAcao.Text = "Alterar Produto";
                            labAcao.ForeColor = Color.Red;
                            hidAcao.Value = "A";
                            btCancelar.Visible = true;
                            btCarregarFoto.Enabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 4000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btProdutoExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            if (b != null)
            {
                try
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto.CODIGO = Convert.ToInt32(b.CommandArgument);

                    if (desenvController.ObterProdutoPedidoProduto(_produto.CODIGO) != null)
                    {
                        msg = "Não será possível excluir este Produto. Produto possui Produto relacionado.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                    else
                    {
                        _produto.STATUS = 'E'; //Excluido
                        desenvController.ExcluirProduto(_produto);

                        msg = "Produto excluído com sucesso.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Exclusão', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);

                        RecarregarProduto();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            hidAcao.Value = "N";
            labAcao.Text = "Novo Produto";
            labAcao.ForeColor = Color.Gray;
            ddlColecoes.SelectedValue = "0";
            ddlOrigem.SelectedValue = "0";
            txtCodigoRef.Text = "";
            ddlGrupo.SelectedValue = "";
            txtModelo.Text = "";
            txtCor.Text = "";
            txtCorFornecedor.Text = "";
            txtTecido.Text = "";
            txtQtdeVarejo.Text = "";
            txtPreco.Text = "";
            txtQtdeAtacado.Text = "";
            txtPrecoAtacado.Text = "";
            txtObservacao.Text = "";
            imgFoto.ImageUrl = "";
            labSalvar.Text = "";

            btCancelar.Visible = false;
            btCarregarFoto.Enabled = false;
            Session["DROP_IMAGE"] = null;
        }
        private void RecarregarProduto()
        {
            labProduto.Text = "";

            //Obter lista de produtos
            List<DESENV_PRODUTO> _lstProduto = new List<DESENV_PRODUTO>();
            _lstProduto = desenvController.ObterProduto(ddlColecoesBuscar.SelectedValue).Where(p => p.STATUS == 'A' && p.PRODUTO_ACABADO == 'N').ToList();

            if (ddlGrupoBuscar.SelectedValue.Trim() != "")
                _lstProduto = _lstProduto.Where(i => i.GRUPO.Trim() == ddlGrupoBuscar.SelectedValue.Trim()).ToList();

            if (txtModeloBuscar.Text.Trim() != "")
                _lstProduto = _lstProduto.Where(i => i.MODELO.Trim().ToUpper().Contains(txtModeloBuscar.Text.Trim().ToUpper())).ToList();

            if (cbModeloFiltro.Checked)
                _lstProduto = _lstProduto.Where(i => i.MODELO == null || i.MODELO.Trim() == "").ToList();

            if (_lstProduto.Count <= 0)
                labProduto.Text = "Nenhum produto encontrado. Refaça sua pesquisa.";

            gvProduto.DataSource = _lstProduto;
            gvProduto.DataBind();
        }
        #endregion

        #region "INCLUSAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labColecao.ForeColor = _OK;
            if (ddlColecoes.SelectedValue.Trim() == "" || ddlColecoes.SelectedValue.Trim() == "0")
            {
                labColecao.ForeColor = _notOK;
                retorno = false;
            }

            labOrigem.ForeColor = _OK;
            if (ddlOrigem.SelectedValue.Trim() == "" || ddlOrigem.SelectedValue.Trim() == "0")
            {
                labOrigem.ForeColor = _notOK;
                retorno = false;
            }

            labCodigo.ForeColor = _OK;
            if (txtCodigoRef.Text == "" || Convert.ToInt32(txtCodigoRef.Text) <= 0)
            {
                labCodigo.ForeColor = _notOK;
                retorno = false;
            }

            labGrupo.ForeColor = _OK;
            if (ddlGrupo.SelectedValue.Trim() == "Selecione" || ddlGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            int codigo = 0;
            DESENV_PRODUTO _produto = null;
            string sFoto = "";
            string sObservacao = "";

            try
            {
                labSalvar.Text = "";

                //Validação de nulos
                if (!ValidarCampos())
                {
                    labSalvar.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                _produto = new DESENV_PRODUTO();
                codigo = desenvController.ObterProdutoCODRef(Convert.ToInt32(txtCodigoRef.Text), ddlColecoes.SelectedValue.Trim());
                if (codigo > 0 && hidAcao.Value != "A")
                {
                    labSalvar.Text = "Não será possível inserir este PRODUTO. Este CÓDIGO já existe para esta Coleção.";
                    return;
                }
                else if (codigo > 0 && hidAcao.Value == "A")
                {
                    _produto.CODIGO = codigo;
                }
                else
                {
                    _produto.CODIGO = 0;
                }

                _produto.COLECAO = ddlColecoes.Text.Trim();
                //_produto.CODIGO_REF = Convert.ToInt32(txtCodigoRef.Text);
                //_produto.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue.Trim());
                //_produto.GRUPO = ddlGrupo.SelectedValue.Trim();
                _produto.MODELO = txtModelo.Text.Trim().ToUpper();
                //_produto.COR = txtCor.Text.Trim();
                //_produto.TECIDO_POCKET = txtTecido.Text.Trim().ToUpper();
                sObservacao = txtObservacao.Text.Trim().ToUpper();

                if (imgFoto.ImageUrl.Trim() != "")
                {
                    sFoto = imgFoto.ImageUrl;
                }
                else
                {
                    var fotoProduto = desenvController.ObterProduto(_produto.COLECAO).Where(p => p.MODELO != null
                                                                            && p.MODELO.Trim().ToUpper() == _produto.MODELO.Trim().ToUpper()
                                                                            && p.FOTO != null && p.FOTO.Trim() != "").Take(1).SingleOrDefault();
                    if (fotoProduto != null)
                        sFoto = (fotoProduto.FOTO == null) ? "" : fotoProduto.FOTO;
                }

                /*
                if (txtQtdeVarejo.Text != "")
                    _produto.QTDE = Convert.ToInt32(txtQtdeVarejo.Text);
                else
                    _produto.QTDE = 0;

                if (txtPreco.Text != "")
                    _produto.PRECO = Convert.ToDecimal(txtPreco.Text);
                else
                    _produto.PRECO = 0;

                if (txtQtdeAtacado.Text != "")
                    _produto.QTDE_ATACADO = Convert.ToInt32(txtQtdeAtacado.Text);
                else
                    _produto.QTDE_ATACADO = 0;

                if (txtPrecoAtacado.Text != "")
                    _produto.PRECO_ATACADO = Convert.ToDecimal(txtPrecoAtacado.Text);
                else
                    _produto.PRECO_ATACADO = 0;

                 * */
                _produto.DATA_INCLUSAO = DateTime.Now;
                _produto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _produto.STATUS = 'A';

                //Lista NOVA
                List<DESENV_PRODUTO> listaProdutosNova = new List<DESENV_PRODUTO>();
                //BUSCAR LISTA
                List<DESENV_PRODUTO> listaProdutosFiltro = new List<DESENV_PRODUTO>();
                listaProdutosFiltro = desenvController.ObterProduto(ddlColecoes.SelectedValue.Trim());
                listaProdutosFiltro = listaProdutosFiltro.Where(p => p.MODELO.Trim().ToUpper() == txtModelo.Text.ToUpper().Trim()).ToList();
                foreach (DESENV_PRODUTO prod in listaProdutosFiltro)
                {
                    if (prod != null)
                    {
                        prod.FOTO = sFoto;
                        prod.OBSERVACAO = sObservacao;
                        listaProdutosNova.Add(prod);
                    }
                }
                //listaProdutos.Add(_produto);
                desenvController.InserirAtualizarProduto(listaProdutosNova);

                //Abrir pop-up do pocket baseado no modelo
                listaProdutosNova = listaProdutosNova.Where(p => p.STATUS == 'A').ToList();
                GerarPocket(listaProdutosNova);

                //Limpar tela
                ddlColecoes_SelectedIndexChanged(null, null);
                ddlGrupo.SelectedValue = "";
                txtModelo.Text = "";
                txtCor.Text = "";
                txtCorFornecedor.Text = "";
                txtQtdeVarejo.Text = "";
                txtPreco.Text = "";
                txtQtdeAtacado.Text = "";
                txtPrecoAtacado.Text = "";
                txtTecido.Text = "";
                txtObservacao.Text = "";
                imgFoto.ImageUrl = "";
                txtCodigoRef.Focus();

                labSalvar.Text = "Produto Cadastrado com sucesso.";
                if (hidAcao.Value == "A")
                    labSalvar.Text = "Produto Alterado com sucesso.";

                labAcao.Text = "Novo Produto";
                labAcao.ForeColor = Color.Gray;
                hidAcao.Value = "N";
                btCancelar.Visible = false;

                Session["COLECAO"] = ddlColecoes.SelectedValue;
                btBuscarProduto_Click(null, null);

            }
            catch (Exception ex)
            {
                labSalvar.Text = ex.Message;
            }
        }
        #endregion

        #region "APROVAÇÃO"
        protected void btProdutoAprovar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            if (b != null)
            {
                try
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(Convert.ToInt32(b.CommandArgument));
                    _produto.DATA_APROVACAO = DateTime.Now;
                    desenvController.AprovarProduto(_produto);

                    msg = "Produto APROVADO com sucesso.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Aprovação Produto', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);

                    RecarregarProduto();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        #endregion

        #region "FOTO
        private void GerarPocket(List<DESENV_PRODUTO> listaProdutos)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "POCKET_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(Pocket.MontarPocket(listaProdutos, false).ToString());
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        protected void btCarregarFoto_Click(object sender, EventArgs e)
        {
            bool dropped = false;

            labSalvar.Text = "";
            if (Session["DROP_IMAGE"] != null)
            {
                imgFoto.ImageUrl = Session["DROP_IMAGE"].ToString();
                Session["DROP_IMAGE"] = null;
            }
            else
            {

                string urlFoto = "";
                Stream streamFoto = null;
                string pathFile = "";
                try
                {
                    if (upFoto.HasFile)
                    {
                        dropped = false;
                        streamFoto = upFoto.PostedFile.InputStream;
                        pathFile = upFoto.PostedFile.FileName;
                        Session["DROP_IMAGE"] = null;
                    }
                    else
                    {
                        HttpFileCollection fileCollection = Request.Files;
                        for (int i = 0; i < fileCollection.Count; i++)
                        {
                            dropped = true;
                            HttpPostedFile upload = fileCollection[i];
                            streamFoto = upload.InputStream;
                            pathFile = fileCollection.Keys[0].ToString();
                        }
                    }

                    if (streamFoto != null)
                    {

                        urlFoto = GravarImagem(streamFoto, pathFile);
                        if (urlFoto.Contains("ERRO"))
                            throw new Exception(urlFoto);
                        if (dropped)
                            Session["DROP_IMAGE"] = urlFoto;
                        imgFoto.ImageUrl = urlFoto;
                    }
                }
                catch (Exception ex)
                {
                    labSalvar.Text = ex.Message;
                }
            }
        }
        private string GravarImagem(Stream _stream, string _pathFile)
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(_pathFile);
                _fileName = Guid.NewGuid() + "_FOTO" + _ext;
                _path = Server.MapPath("~/Image_POCKET/") + _fileName;

                //Obter stream da imagem
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
                // width 300
                // height 320
                var newWidth = 0;
                var newHeight = 0;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > 300 || newHeight > 320)
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
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });

                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                    ddlColecoes_SelectedIndexChanged(null, null);
                }
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = RetornarGrupo();
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }

            List<SP_OBTER_GRUPOResult> _grupoAux = (prodController.ObterGrupoProduto("01"));
            if (_grupoAux != null)
            {
                _grupoAux.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoBuscar.DataSource = _grupoAux;
                ddlGrupoBuscar.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = RetornarOrigem();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "Selecione" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        /*private void CarregarCores()
        {
            List<DESENV_CORE> _cores = desenvController.ObterCorPocket();
            if (_cores != null)
            {
                _cores.Insert(0, new DESENV_CORE { COR = "" });

                ddlCor.DataSource = _cores;
                ddlCor.DataBind();

            }
        }*/
        private List<SP_OBTER_GRUPOResult> RetornarGrupo()
        {
            return (prodController.ObterGrupoProduto("01"));
        }
        private List<DESENV_PRODUTO_ORIGEM> RetornarOrigem()
        {
            return (desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList());
        }
        #endregion

        protected void gvProduto_Load(object sender, EventArgs e)
        {
            g_grupo = RetornarGrupo();
            g_origem = RetornarOrigem();
        }
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                CarregarOrigem(colecao);

                var _produto = desenvController.ObterProduto().ToList();
                _produto = _produto.Where(p => p.COLECAO == colecao).ToList();
                if (_produto != null)
                {
                    int _codigoMax = 0;
                    if (_produto.Count > 0)
                        _codigoMax = _produto.Max(i => i.CODIGO_REF);

                    txtCodigoRef.Text = (_codigoMax + 1).ToString();
                }

            }
        }

    }
}
