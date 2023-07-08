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

namespace Relatorios
{
    public partial class desenv_pedido_cad : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        List<DESENV_PRODUTO> _produtoFiltro = null;
        DESENV_PRODUTO_CARRINHO _carrinho = null;
        DESENV_PRODUTO_PEDIDO _produtoPedido = null;
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPedidoData.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPedidoPrevEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                if (Request.Files.Keys.Count > 0)
                {
                    btCarregarFoto_Click(sender, e);
                }
                else
                {
                    int codigoPedido = 0;
                    if (Request.QueryString["p"] == null || Request.QueryString["p"] == "")
                        Response.Redirect("desenv_menu.aspx");

                    codigoPedido = Convert.ToInt32(Request.QueryString["p"].ToString());
                    DESENV_PEDIDO _pedido = desenvController.ObterPedido(codigoPedido);

                    //Carregar colecoes
                    CarregarColecoes();
                    CarregarUnidadeMedida();
                    CarregarFormaPgto();
                    CarregarGrupo();

                    pnlFormaPgtoOutro.Visible = false;
                    btExcluir.Visible = false;
                    if (_pedido != null) //Se tiver pedido em aberto, carregar o pedido do USUARIO do carrinho
                    {
                        hidPedido.Value = codigoPedido.ToString();

                        string colecao = "";
                        colecao = (new BaseController().BuscaColecaoAtual(_pedido.COLECAO)).COLECAO;

                        ddlColecoes.SelectedValue = colecao;
                        txtPedidoNumero.Text = _pedido.NUMERO_PEDIDO.ToString();
                        txtFornecedor.Text = _pedido.FORNECEDOR;
                        txtCorFornecedor.Text = _pedido.COR.ToUpper();
                        txtPedidoData.Text = _pedido.DATA_PEDIDO.ToString("dd/MM/yyyy");
                        if (_pedido.DATA_ENTREGA_PREV != null)
                            txtPedidoPrevEntrega.Text = Convert.ToDateTime(_pedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");
                        ddlUnidade.SelectedValue = _pedido.UNIDADE_MEDIDA.ToString();
                        txtQtde.Text = _pedido.QTDE.ToString("###,###,##0.000");
                        txtPreco.Text = _pedido.VALOR.ToString("###,###,##0.000");
                        ddlFormaPgto.SelectedValue = _pedido.CONDICAO_PGTO.ToString();
                        imgFoto.ImageUrl = _pedido.FOTO_TECIDO;

                        if (_pedido.CONDICAO_PGTO.Trim() == "##")
                        {
                            pnlFormaPgtoOutro.Visible = true;
                            txtFormaPgtoOutro.Text = _pedido.CONDICAO_PGTO_OUTRO.Trim().ToUpper();
                        }
                        HabilitarCampos(false);

                        hrefVoltar.HRef = "desenv_pedido_altera.aspx";

                        btExcluir.Visible = true;
                        btIncluirPedido.Visible = false;
                        pnlProduto.Visible = true;

                        //Preencher total do carrinho
                        PreencherTotalProdutoCarrinho();
                    }


                    dialogPai.Visible = false;
                    Session["DROP_IMAGE"] = null;
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFormaPgto()
        {
            List<FORMA_PGTO> _formaPGTO = (new BaseController().ObterFormaPgto());

            _formaPGTO.Insert(0, new FORMA_PGTO { CONDICAO_PGTO = "0", DESC_COND_PGTO = "Selecione" });

            if (_formaPGTO != null)
            {
                ddlFormaPgto.DataSource = _formaPGTO;
                ddlFormaPgto.DataBind();
            }
        }
        private void CarregarUnidadeMedida()
        {
            List<UNIDADE_MEDIDA> _unidadeMedida = (new ProducaoController().ObterUnidadeMedida());

            _unidadeMedida.Add(new UNIDADE_MEDIDA { CODIGO = 0, DESCRICAO = "Selecione", STATUS = 'A' });

            _unidadeMedida = _unidadeMedida.OrderBy(l => l.CODIGO).ToList();

            if (_unidadeMedida != null)
            {
                ddlUnidade.DataSource = _unidadeMedida;
                ddlUnidade.DataBind();
            }
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (new ProducaoController().ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void RecarregarProduto(int codigoPedido)
        {
            //Obter lista de produtos para exclusão
            List<DESENV_PRODUTO> _produtoSemPedido = null;
            List<DESENV_PRODUTO> _produtoComPedido = null;

            //_produtoSemPedido = desenvController.ObterProdutoSemPedido(ddlColecoes.SelectedValue.Trim());
            _produtoSemPedido = desenvController.ObterProdutoSemPedido().Where(p => p.DATA_APROVACAO != null).ToList();

            if (codigoPedido > 0)
            {
                DESENV_PEDIDO _pedido = desenvController.ObterPedido(codigoPedido);
                if (_pedido != null)
                {
                    _produtoComPedido = desenvController.ObterProdutoComPedido(_pedido.CODIGO).ToList();
                    _produtoSemPedido = _produtoSemPedido.Union(_produtoComPedido).ToList();
                }
            }
            else
            {
                gvProduto.Columns[9].Visible = false;
            }

            _produtoSemPedido = _produtoSemPedido.OrderBy(i => i.COLECAO).ThenBy(p => p.GRUPO).ThenBy(j => j.MODELO).ToList();

            //Filtro
            _produtoFiltro = _produtoSemPedido;
            gvProduto.DataSource = _produtoFiltro;
            gvProduto.DataBind();

            /*
            DESENV_PRODUTO _produto = null;
            int codigoProduto = 0;
            CheckBox cbIncluir = null;
            CheckBox cbExcluir = null;
            foreach (GridViewRow r in gvProduto.Rows)
            {
                //Obter controle
                cbExcluir = r.FindControl("cbPedidoExcluir") as CheckBox;
                cbIncluir = r.FindControl("cbPedidoIncluir") as CheckBox;

                //Desativar
                cbExcluir.Enabled = false;

                if (_produtoComPedido != null && _produtoComPedido.Count > 0)
                {
                    codigoProduto = Convert.ToInt32(gvProduto.DataKeys[r.RowIndex].Value);
                    _produto = _produtoComPedido.Find(i => i.CODIGO == codigoProduto);

                    if (_produto != null)
                    {
                        cbExcluir.Enabled = true;
                        cbIncluir.Enabled = false;
                        if (_produto.STATUS == 'F')
                        {
                            r.Enabled = false;
                            r.ForeColor = Color.Green;
                        }
                        else
                            r.ForeColor = Color.MidnightBlue;
                    }
                }
            }*/
        }
        private void PreencherTotalProdutoCarrinho()
        {
            int total = 0;
            string msg = "";
            int _usuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            List<DESENV_PRODUTO_CARRINHO> _carrinho = desenvController.ObterProdutoCarrinho();
            if (_carrinho != null && _carrinho.Count > 0 && _usuario > 0)
            {
                _carrinho = _carrinho.Where(i => i.USUARIO == _usuario).ToList();

                if (hidPedido.Value != "")
                    _carrinho = _carrinho.Where(j => j.DESENV_PEDIDO == Convert.ToInt32(hidPedido.Value)).ToList();

                total = _carrinho.Count;
            }

            if (total <= 0)
            {
                msg = "Nenhum produto adicionado";
                lnkProdutoResumo.Visible = false;
            }
            else if (total == 1)
            {
                msg = total.ToString() + " Produto adicionado";
                lnkProdutoResumo.Visible = true;
            }
            else
            {
                msg = total.ToString() + " Produtos adicionados";
                lnkProdutoResumo.Visible = true;
            }

            hidTotalCarrinho.Value = total.ToString();
            labTotalCarrinho.Text = msg;
        }
        private List<DESENV_PRODUTO> ObterCarrinhoUsuario()
        {
            List<DESENV_PRODUTO> _lstProdutoCarrinho = new List<DESENV_PRODUTO>();
            int? codPedido = null;

            if (hidPedido.Value != "")
                codPedido = Convert.ToInt32(hidPedido.Value);

            _lstProdutoCarrinho = desenvController.ObterProdutoNoCarrinho(((USUARIO)Session["USUARIO"]).CODIGO_USUARIO, codPedido);

            return _lstProdutoCarrinho;
        }
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

            labPedidoNumero.ForeColor = _OK;
            if (txtPedidoNumero.Text.Trim() == "" || Convert.ToInt32(txtPedidoNumero.Text.Trim()) <= 0)
            {
                labPedidoNumero.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (txtFornecedor.Text.Trim() == "")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedor.ForeColor = _OK;
            if (txtCorFornecedor.Text.Trim() == "")
            {
                labCorFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labDataPedido.ForeColor = _OK;
            if (txtPedidoData.Text.Trim() == "")
            {
                labDataPedido.ForeColor = _notOK;
                retorno = false;
            }

            labDataPrevisaoEntrega.ForeColor = _OK;
            if (txtPedidoPrevEntrega.Text.Trim() == "")
            {
                labDataPrevisaoEntrega.ForeColor = _notOK;
                retorno = false;
            }

            labUnidadeMedida.ForeColor = _OK;
            if (ddlUnidade.SelectedValue.Trim() == "" || ddlUnidade.SelectedValue.Trim() == "0")
            {
                labUnidadeMedida.ForeColor = _notOK;
                retorno = false;
            }

            labQtde.ForeColor = _OK;
            if (txtQtde.Text.Trim() == "" || Convert.ToDecimal(txtQtde.Text.Trim()) <= 0)
            {
                labQtde.ForeColor = _notOK;
                retorno = false;
            }

            labPreco.ForeColor = _OK;
            if (txtPreco.Text.Trim() == "" || Convert.ToDecimal(txtPreco.Text.Trim()) <= 0)
            {
                labPreco.ForeColor = _notOK;
                retorno = false;
            }

            labFormaPgto.ForeColor = _OK;
            if (ddlFormaPgto.SelectedValue.Trim() == "" || ddlFormaPgto.SelectedValue.Trim() == "0")
            {
                labFormaPgto.ForeColor = _notOK;
                retorno = false;
            }
            else if (ddlFormaPgto.SelectedValue.Trim().ToUpper() == "##" && txtFormaPgtoOutro.Text.Trim() == "")
            {
                labFormaPgto.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool _true)
        {
            //ddlColecoes.Enabled = _true;
            txtPedidoNumero.Enabled = _true;
            //txtPedidoData.Enabled = _true;
            //txtPedidoPrevEntrega.Enabled = _true;
            //ddlUnidade.Enabled = _true;
            //txtQtde.Enabled = _true;
            //txtPreco.Enabled = _true;
            //ddlFormaPgto.Enabled = _true;
            //txtFormaPgtoOutro.Enabled = _true;
        }
        #endregion

        #region "FOTO"
        protected void btCarregarFoto_Click(object sender, EventArgs e)
        {
            bool dropped = false;

            //labSalvar.Text = "";
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
                    //labpe.Text = ex.Message;
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
                _fileName = Guid.NewGuid() + "_TECIDO" + _ext;
                _path = Server.MapPath("~/Image_HB/") + _fileName;

                //Obter stream da imagem
                if (_stream != null)
                    GenerateThumbnails(1, _stream, _path);

                retornoImagem = "~/Image_HB/" + _fileName;
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

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_produto.COLECAO)).DESC_COLECAO;

                        TextBox _txtConsumo = e.Row.FindControl("txtConsumo") as TextBox;
                        if (_txtConsumo != null)
                            _txtConsumo.Text = _produto.CONSUMO.ToString();

                        Button _btIncluirCarrinho = e.Row.FindControl("btIncluirCarrinho") as Button;
                        if (_btIncluirCarrinho != null)
                            _btIncluirCarrinho.CommandArgument = _produto.CODIGO.ToString();

                        Button _btExcluirCarrinho = e.Row.FindControl("btExcluirCarrinho") as Button;
                        if (_btExcluirCarrinho != null)
                        {
                            //Verifica produto no carrinho
                            _btExcluirCarrinho.Enabled = false;
                            _carrinho = desenvController.ObterProdutoCarrinho().Where(j => j.DESENV_PRODUTO == _produto.CODIGO).SingleOrDefault();
                            if (_carrinho != null)
                            {
                                _btIncluirCarrinho.Enabled = false;
                                _btExcluirCarrinho.Enabled = true;
                                e.Row.ForeColor = Color.Red;
                                _btExcluirCarrinho.CommandArgument = _carrinho.CODIGO.ToString();
                            }
                            else
                            {
                                //Verifica produto ja adicionado
                                _produtoPedido = desenvController.ObterProdutoPedidoProduto(_produto.CODIGO);
                                if (_produtoPedido != null)
                                {
                                    _btIncluirCarrinho.Enabled = false;
                                    _btExcluirCarrinho.Enabled = true;
                                    e.Row.ForeColor = Color.Pink;
                                    _btExcluirCarrinho.CommandArgument = _produtoPedido.DESENV_PRODUTO.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void txtPedidoNumero_TextChanged(object sender, EventArgs e)
        {
            labPedidoNumero.ForeColor = Color.Gray;
            labPedidoNumero.ToolTip = "";
            if (txtPedidoNumero.Text != "")
                if (desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoNumero.Text)) != null)
                {
                    txtPedidoNumero.Text = "";
                    txtPedidoNumero.Focus();
                    labPedidoNumero.ForeColor = Color.Red;
                    labPedidoNumero.ToolTip = "Número do Pedido já existe. Informe um número novo.";
                }
                else
                {
                    txtFornecedor.Focus();
                }
        }
        protected void cbPedido_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb != null)
            {
                GridViewRow gv = (GridViewRow)cb.NamingContainer;
                if (gv != null)
                {
                    if (cb.Checked)
                        gv.ForeColor = Color.Red;
                    else
                        gv.ForeColor = Color.Gray;
                }
            }
        }
        protected void btIncluirCarrinho_Click(object sender, EventArgs e)
        {
            int codigoProduto = 0;
            int? codigoPedido = null;
            string msg = "";
            Button b = (Button)sender;
            if (b != null)
            {
                codigoProduto = Convert.ToInt32(b.CommandArgument);
                GridViewRow row = (GridViewRow)b.NamingContainer;
                if (row != null)
                {
                    TextBox _txtConsumo = row.FindControl("txtConsumo") as TextBox;
                    if (_txtConsumo != null && _txtConsumo.Text != "" && Convert.ToDecimal(_txtConsumo.Text) > 0)
                    {
                        Button bExcluir = row.FindControl("btExcluirCarrinho") as Button;

                        //Inserir no carrinho
                        DESENV_PRODUTO_CARRINHO _carrinho = new DESENV_PRODUTO_CARRINHO();
                        _carrinho.DESENV_PRODUTO = codigoProduto;
                        if (hidPedido.Value != "")
                            codigoPedido = Convert.ToInt32(hidPedido.Value);
                        _carrinho.DESENV_PEDIDO = codigoPedido;
                        _carrinho.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        int codigoCarrinho = desenvController.InserirProdutoCarrinho(_carrinho);

                        //Atualizar consumo
                        DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                        _produto.CODIGO = codigoProduto;
                        _produto.CONSUMO = Convert.ToDecimal(_txtConsumo.Text);
                        desenvController.AtualizarProdutoConsumo(_produto);

                        //Desabilitar Linha
                        b.Enabled = false;
                        row.ForeColor = Color.Red;
                        if (bExcluir != null)
                        {
                            bExcluir.Enabled = true;
                            bExcluir.CommandArgument = codigoCarrinho.ToString();
                        }

                        //Preencher total do carrinho
                        PreencherTotalProdutoCarrinho();

                        //Toast - Inserido com sucesso
                        msg = (row.Cells[2].Text) + " " + (row.Cells[3].Text) + " " + (row.Cells[4].Text) + " adicionado.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Produto', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                    else
                    {
                        msg = "Informe o <b>Consumo</b> do produto.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Atenção', life: 2000, theme: 'redError', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                    }
                }
            }
        }
        protected void btExcluirCarrinho_Click(object sender, EventArgs e)
        {
            int codigoCarrinho = 0;
            string msg = "";
            Button b = (Button)sender;
            if (b != null)
            {
                GridViewRow row = (GridViewRow)b.NamingContainer;
                if (row != null)
                {
                    Button bIncluir = row.FindControl("btIncluirCarrinho") as Button;

                    //excluir no carrinho
                    codigoCarrinho = Convert.ToInt32(b.CommandArgument);
                    desenvController.ExcluirProdutoCarrinho(codigoCarrinho);
                    desenvController.ExcluirProdutoPedidoProduto(codigoCarrinho);

                    //Habilitar Linha
                    b.Enabled = false;
                    row.ForeColor = Color.Gray;
                    if (bIncluir != null)
                        bIncluir.Enabled = true;

                    //Preencher total do carrinho
                    PreencherTotalProdutoCarrinho();

                    //Toast - Inserido com sucesso
                    msg = (row.Cells[2].Text) + " " + (row.Cells[3].Text) + " " + (row.Cells[4].Text) + " excluído.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'Produto', life: 2000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                labFiltro.Text = "";
                /*if (ddlColecoes.SelectedValue.Trim() == "" || ddlColecoes.SelectedValue.Trim() == "0")
                {
                    labFiltro.Text = "Selecione a coleção.";
                    return;
                }*/

                //Buscar Produtos sem Pedido
                if (!cbProdutoAdicionado.Checked)
                {
                    //_produtoFiltro = desenvController.ObterProdutoSemPedido(ddlColecoes.SelectedValue.Trim());
                    _produtoFiltro = desenvController.ObterProdutoSemPedido().Where(p => p.DATA_APROVACAO != null).ToList();

                    if (ddlGrupo.SelectedValue.Trim() != "")
                        _produtoFiltro = _produtoFiltro.Where(i => ((i.GRUPO == null) ? i.GRUPO : i.GRUPO.Trim()) == ddlGrupo.SelectedValue.Trim()).ToList();

                    if (txtModelo.Text.Trim() != "")
                        _produtoFiltro = _produtoFiltro.Where(i => i.MODELO.Trim().ToUpper().Contains(txtModelo.Text.Trim().ToUpper())).ToList();

                    if (txtCor.Text.Trim() != "")
                        _produtoFiltro = _produtoFiltro.Where(i => i.COR.Trim().ToUpper().Contains(txtCor.Text.Trim().ToUpper())).ToList();
                } //Buscar produtos do carrinho
                else
                {
                    _produtoFiltro = ObterCarrinhoUsuario();
                    if (hidPedido.Value != "")
                        _produtoFiltro = _produtoFiltro.Union(desenvController.ObterProdutoComPedido(Convert.ToInt32(hidPedido.Value))).ToList();
                }

                _produtoFiltro = _produtoFiltro.OrderBy(i => i.COLECAO).ThenBy(p => p.GRUPO).ThenBy(j => j.MODELO).ThenBy(k => k.COR).ToList();
                gvProduto.DataSource = _produtoFiltro;
                gvProduto.DataBind();
            }
            catch (Exception ex)
            {
                labFiltro.Text = ex.Message;
            }
        }
        protected void ddlFormaPgto_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFormaPgtoOutro.Text = "";
            if (ddlFormaPgto.SelectedValue.Trim().ToUpper() == "##")
                pnlFormaPgtoOutro.Visible = true;
            else
                pnlFormaPgtoOutro.Visible = false;
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            //int totalCarrinho = 0;
            labBuscar.Text = "";

            try
            {
                labBuscar.Text = "";
                if (!ValidarCampos())
                {
                    labFiltro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                /*
                totalCarrinho = (hidTotalCarrinho.Value == "") ? 0 : Convert.ToInt32(hidTotalCarrinho.Value);
                if (totalCarrinho <= 0)
                {
                    labFiltro.Text = "Por favor, adicionar produtos ao Pedido.";
                    return;
                }
                 * */

                SalvarPedido();

                labPopUp.Text = txtPedidoNumero.Text;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('desenv_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('desenv_menu.aspx', '_self'); }, 'Pedido Novo': function () { window.open('desenv_pedido_cad.aspx?p=0', '_self'); } } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labBuscar.Text = "ERRO: " + ex.Message;
            }
        }
        private void SalvarPedido()
        {
            int codigoPedido = 0;

            //Salvar PEDIDO
            DESENV_PEDIDO _pedido = null;

            if (hidPedido.Value == "")
                _pedido = new DESENV_PEDIDO();
            else
            {
                codigoPedido = Convert.ToInt32(hidPedido.Value);
                _pedido = desenvController.ObterPedido(codigoPedido);
            }

            _pedido.COLECAO = ddlColecoes.SelectedValue.Trim();
            _pedido.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
            _pedido.FORNECEDOR = txtFornecedor.Text.Trim().ToUpper();
            _pedido.COR = txtCorFornecedor.Text.Trim().ToUpper();
            _pedido.DATA_PEDIDO = Convert.ToDateTime(txtPedidoData.Text);
            _pedido.DATA_ENTREGA_PREV = Convert.ToDateTime(txtPedidoPrevEntrega.Text);
            _pedido.DATA_INCLUSAO = DateTime.Now;
            _pedido.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            _pedido.STATUS = 'A';
            _pedido.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);
            _pedido.QTDE = Convert.ToDecimal(txtQtde.Text);
            _pedido.VALOR = Convert.ToDecimal(txtPreco.Text);
            _pedido.CONDICAO_PGTO = ddlFormaPgto.SelectedValue;
            _pedido.CONDICAO_PGTO_OUTRO = txtFormaPgtoOutro.Text.Trim().ToUpper();
            _pedido.FOTO_TECIDO = imgFoto.ImageUrl;

            if (hidPedido.Value == "")
            {
                codigoPedido = desenvController.InserirPedido(_pedido);
            }
            else
            {
                desenvController.AtualizarPedido(_pedido);
            }

            List<DESENV_PRODUTO> _carrinho = ObterCarrinhoUsuario();
            DESENV_PRODUTO_PEDIDO _produtoPedido = null;
            //INSERIR PRODUTOS NO PEDIDO
            foreach (DESENV_PRODUTO p in _carrinho)
            {
                _produtoPedido = new DESENV_PRODUTO_PEDIDO();
                _produtoPedido.DESENV_PEDIDO = codigoPedido;
                _produtoPedido.DESENV_PRODUTO = p.CODIGO;
                desenvController.InserirProdutoPedido(_produtoPedido);
            }

            DESENV_PRODUTO_CARRINHO _produtoCarrinho = null;
            //EXCLUIR PRODUTOS DO CARRINHO NO PEDIDO
            foreach (DESENV_PRODUTO p in _carrinho)
            {
                _produtoCarrinho = new DESENV_PRODUTO_CARRINHO();
                _produtoCarrinho = desenvController.ObterProdutoCarrinho().Where(i => i.DESENV_PRODUTO == p.CODIGO).SingleOrDefault();
                desenvController.ExcluirProdutoCarrinho(_produtoCarrinho.CODIGO);
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            int codigoPedido = 0;
            DESENV_PEDIDO _pedido = null;

            if (hidPedido.Value != "")
            {
                codigoPedido = Convert.ToInt32(hidPedido.Value);

                List<DESENV_PRODUTO_PEDIDO> _lstTP = desenvController.ObterProdutoPedidoPedido(codigoPedido);
                if (_lstTP.FindIndex(i => i.DESENV_PEDIDO1.STATUS == 'F') >= 0)
                {
                    labBuscar.Text = "Não será possível excluir este Pedido. Pedido possui Produto entregue.";
                    return;
                }

                _pedido = new DESENV_PEDIDO();
                _pedido = desenvController.ObterPedido(codigoPedido);
                if (_pedido != null)
                {
                    List<PROD_HB> _hb = (new ProducaoController().ObterHB());
                    if (_hb.FindIndex(i => i.NUMERO_PEDIDO == _pedido.NUMERO_PEDIDO) >= 0)
                    {
                        labBuscar.Text = "Não será possível excluir este Pedido. Pedido possui HB relacionado.";
                        return;
                    }
                }

                _pedido = new DESENV_PEDIDO();
                _pedido.CODIGO = codigoPedido;
                _pedido.STATUS = 'E'; //EXCLUSAO
                desenvController.ExcluirPedido(_pedido);

                foreach (DESENV_PRODUTO_PEDIDO tp in _lstTP)
                {
                    desenvController.ExcluirProdutoPedidoProduto(tp.DESENV_PRODUTO);
                }

                labPopUp.Text = txtPedidoNumero.Text;
                labMensagem.Text = "EXCLUÍDO COM SUCESSO.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('desenv_menu.aspx', '_self'); }, buttons: { Menu: function () { window.open('desenv_menu.aspx', '_self'); }, 'Pedido Novo': function () { window.open('desenv_pedido_cad.aspx?p=0', '_self'); } } }); });", true);
                dialogPai.Visible = true;
            }
        }
        protected void lnkProdutoResumo_Click(object sender, EventArgs e)
        {
            string _url = "";
            int codigoPedido = 0;
            DateTime _dataPedido;
            DateTime _dataPrevEntrega;
            try
            {
                labFiltro.Text = "";
                if (!ValidarCampos())
                {
                    labFiltro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                if (!DateTime.TryParse(txtPedidoData.Text, out _dataPedido))
                {
                    labFiltro.Text = "Data do Pedido inválida.";
                    return;
                }
                if (!DateTime.TryParse(txtPedidoPrevEntrega.Text, out _dataPrevEntrega))
                {
                    labFiltro.Text = "Data de Previsão de Entrega inválida.";
                    return;
                }

                //Inserir PEDIDO
                DESENV_PEDIDO _pedido = new DESENV_PEDIDO();
                _pedido.COLECAO = ddlColecoes.SelectedValue.Trim();
                _pedido.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
                _pedido.FORNECEDOR = txtFornecedor.Text.Trim().ToUpper();
                _pedido.COR = txtCorFornecedor.Text.Trim().ToUpper();
                _pedido.DATA_PEDIDO = Convert.ToDateTime(txtPedidoData.Text);
                _pedido.DATA_ENTREGA_PREV = Convert.ToDateTime(txtPedidoPrevEntrega.Text);
                _pedido.DATA_INCLUSAO = DateTime.Now;
                _pedido.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _pedido.STATUS = 'A';
                _pedido.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);
                _pedido.QTDE = Convert.ToDecimal(txtQtde.Text);
                _pedido.VALOR = Convert.ToDecimal(txtPreco.Text);
                _pedido.CONDICAO_PGTO = ddlFormaPgto.SelectedValue;
                _pedido.CONDICAO_PGTO_OUTRO = txtFormaPgtoOutro.Text.Trim().ToUpper();

                if (hidPedido.Value == "")
                {
                    codigoPedido = desenvController.InserirPedido(_pedido);
                    hidPedido.Value = codigoPedido.ToString();
                }
                else
                {
                    codigoPedido = Convert.ToInt32(hidPedido.Value);
                    _pedido.CODIGO = codigoPedido;
                    desenvController.AtualizarPedido(_pedido);
                }

                //Abrir pop-up
                _url = "fnAbrirTelaCadastro('desenv_pedido_cad_resumo.aspx?p=" + codigoPedido + "');"; //AO CLICAR NO BOTAO PEDIDO OK - LIBERAR O BOTAO SALVAR
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
            }
            catch (Exception ex)
            {
                labFiltro.Text = ex.Message;
            }
        }
        protected void lnkLimparCarrinho_Click(object sender, EventArgs e)
        {

        }

        protected void btIncluirPedido_Click(object sender, EventArgs e)
        {
            int codigoPedido = 0;
            DateTime _dataPedido;
            DateTime _dataPrevEntrega;
            try
            {
                labFiltro.Text = "";
                if (!ValidarCampos())
                {
                    labFiltro.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                if (!DateTime.TryParse(txtPedidoData.Text, out _dataPedido))
                {
                    labFiltro.Text = "Data do Pedido inválida.";
                    return;
                }
                if (!DateTime.TryParse(txtPedidoPrevEntrega.Text, out _dataPrevEntrega))
                {
                    labFiltro.Text = "Data de Previsão de Entrega inválida.";
                    return;
                }

                //Inserir PEDIDO
                DESENV_PEDIDO _pedido = new DESENV_PEDIDO();
                _pedido.COLECAO = ddlColecoes.SelectedValue.Trim();
                _pedido.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
                _pedido.FORNECEDOR = txtFornecedor.Text.Trim().ToUpper();
                _pedido.COR = txtCorFornecedor.Text.Trim().ToUpper();
                _pedido.DATA_PEDIDO = Convert.ToDateTime(txtPedidoData.Text);
                _pedido.DATA_ENTREGA_PREV = Convert.ToDateTime(txtPedidoPrevEntrega.Text);
                _pedido.DATA_INCLUSAO = DateTime.Now;
                _pedido.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _pedido.STATUS = 'A';
                _pedido.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);
                _pedido.QTDE = Convert.ToDecimal(txtQtde.Text);
                _pedido.VALOR = Convert.ToDecimal(txtPreco.Text);
                _pedido.CONDICAO_PGTO = ddlFormaPgto.SelectedValue;
                _pedido.CONDICAO_PGTO_OUTRO = txtFormaPgtoOutro.Text.Trim().ToUpper();
                _pedido.FOTO_TECIDO = imgFoto.ImageUrl;
                codigoPedido = desenvController.InserirPedido(_pedido);

                hidPedido.Value = codigoPedido.ToString();

                pnlProduto.Visible = true;
                btIncluirPedido.Visible = false;
                HabilitarCampos(false);

                //Abrir pop-up
                //_url = "fnAbrirTelaCadastro('desenv_pedido_cad_resumo.aspx?p=" + codigoPedido + "');"; //AO CLICAR NO BOTAO PEDIDO OK - LIBERAR O BOTAO SALVAR
                //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
            }
            catch (Exception ex)
            {
                labFiltro.Text = ex.Message;
            }
        }

    }
}
