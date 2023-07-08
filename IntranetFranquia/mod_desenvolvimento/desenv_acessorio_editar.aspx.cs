using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using Relatorios.mod_desenvolvimento.modelo_pocket;
using System.Text;

namespace Relatorios
{
    public partial class desenv_acessorio_editar : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        int qtdeOriginal = 0;
        decimal valorTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                int codigoAcessorio = 0;
                if (Request.QueryString["a"] == null || Request.QueryString["a"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                codigoAcessorio = Convert.ToInt32(Request.QueryString["a"].ToString());
                DESENV_ACESSORIO _acessorio = desenvController.ObterAcessorio(codigoAcessorio);
                if (_acessorio == null)
                    Response.Redirect("desenv_menu.aspx");

                hidFoto2.Value = (_acessorio.FOTO2 == null) ? "" : _acessorio.FOTO2;
                ddlQtdeFoto_SelectedIndexChanged(null, null);

                CarregarAcessorio(codigoAcessorio);
                hidCodigoAcessorio.Value = codigoAcessorio.ToString();
                Session["DROP_IMAGE"] = null;
            }

            if (Request.Files.Keys.Count > 0)
                btCarregarFoto_Click(sender, e);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if (window.File && window.FileList && window.FileReader) { var dropZone = document.getElementById('drop_zone'); dropZone.addEventListener('dragover', handleDragOver, false); dropZone.addEventListener('drop', handleDnDFileSelect, false); } else { alert('Sorry! this browser does not support HTML5 File APIs.'); }", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("02");
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem().Where(i => i.COLECAO.Trim() == col.Trim() && i.STATUS == 'A').OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarCores()
        {
            List<CORES_BASICA> _coresBasicas = prodController.ObterCoresBasicas().ToList();
            _coresBasicas = _coresBasicas.GroupBy(p => new { COR = p.COR.Trim(), DESC_COR = p.DESC_COR.Trim() }).Select(x => new CORES_BASICA { COR = x.Key.COR.Trim(), DESC_COR = x.Key.DESC_COR.Trim() }).ToList();
            if (_coresBasicas != null)
            {
                _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

                ddlCor.DataSource = _coresBasicas;
                ddlCor.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
        }

        private void CarregarAcessorio(int codigoAcessorio)
        {
            DESENV_ACESSORIO _acessorio = new DESENV_ACESSORIO();
            _acessorio = desenvController.ObterAcessorio(codigoAcessorio);
            if (_acessorio != null)
            {
                CarregarGrupo();
                CarregarOrigem(_acessorio.COLECAO);
                CarregarGriffe();
                CarregarCores();
                CarregarFornecedores();

                // SE JA EXISTES ALGUM PEDIDO, NAO PODE ALTERAR
                bool enabled = true;
                var pedidoAcessorio = desenvController.ObterAcessorioPedido(_acessorio.CODIGO.ToString());
                if (pedidoAcessorio != null && pedidoAcessorio.Count() > 0)
                    enabled = false;

                ddlOrigem.SelectedValue = _acessorio.DESENV_PRODUTO_ORIGEM.ToString();
                ddlOrigem.Enabled = enabled;
                ddlGrupo.SelectedValue = prodController.ObterGrupoProduto("02").Where(p => p.GRUPO_PRODUTO.Trim() == _acessorio.GRUPO).SingleOrDefault().GRUPO_PRODUTO;
                ddlGrupo.Enabled = enabled;
                txtProduto.Text = _acessorio.PRODUTO;
                txtProduto.Enabled = enabled;
                if (_acessorio.GRIFFE != null && _acessorio.GRIFFE.Trim() != "")
                    ddlGriffe.SelectedValue = new BaseController().BuscaGriffes().Where(p => p.GRIFFE.Trim() == _acessorio.GRIFFE.Trim()).SingleOrDefault().GRIFFE;
                ddlGriffe.Enabled = enabled;
                ddlFornecedor.SelectedValue = _acessorio.FORNECEDOR;
                ddlFornecedor.Enabled = enabled;

                ddlCor.SelectedValue = _acessorio.COR;
                ddlCor.Enabled = enabled;
                txtCorFornecedor.Text = _acessorio.COR_FORNECEDOR;
                txtCorFornecedor.Enabled = enabled;

                imgFoto.ImageUrl = (_acessorio.FOTO1 == null) ? "" : _acessorio.FOTO1;
                imgFoto2.ImageUrl = (_acessorio.FOTO2 == null) ? "" : _acessorio.FOTO2;
                txtObservacao.Text = _acessorio.OBS;

                txtPreco.Text = Convert.ToString(_acessorio.PRECO);
                txtCusto.Text = Convert.ToString(_acessorio.CUSTO);
                txtQuantidade.Text = Convert.ToString(_acessorio.QTDE);
                txtDescricaoSugerida.Text = _acessorio.DESCRICAO_SUGERIDA;
                txtReferFabricante.Text = _acessorio.REFER_FABRICANTE;
                txtDataPrevEntrega.Text = _acessorio.DATA_PREVISAO_ENTREGA;

                hidCodigo.Value = codigoAcessorio.ToString();
                CarregarHistoricoPedido(_acessorio);
                CarregarPocket(_acessorio);
            }
        }
        private bool ValidarCampos(bool validaPedido)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labOrigem.ForeColor = _OK;
            if (ddlOrigem.SelectedValue.Trim() == "" || ddlOrigem.SelectedValue.Trim() == "0")
            {
                labOrigem.ForeColor = _notOK;
                retorno = false;
            }

            labGrupo.ForeColor = _OK;
            if (ddlGrupo.SelectedValue.Trim() == "Selecione" || ddlGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labProduto.ForeColor = _OK;
            if (txtProduto.Text.Trim() == "")
            {
                labProduto.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue.Trim() == "")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labQuantidade.ForeColor = _OK;
            if (txtQuantidade.Text.Trim() == "" && validaPedido)
            {
                labQuantidade.ForeColor = _notOK;
                retorno = false;
            }

            labPreco.ForeColor = _OK;
            if (txtPreco.Text.Trim() == "" && validaPedido)
            {
                labPreco.ForeColor = _notOK;
                retorno = false;
            }

            labCusto.ForeColor = _OK;
            if (txtCusto.Text.Trim() == "" && validaPedido)
            {
                labCusto.ForeColor = _notOK;
                retorno = false;
            }

            labDescricaoSugerida.ForeColor = _OK;
            if (txtDescricaoSugerida.Text.Trim() == "" && validaPedido)
            {
                labDescricaoSugerida.ForeColor = _notOK;
                retorno = false;
            }

            labPrevEntrega.ForeColor = _OK;
            if (txtDataPrevEntrega.Text.Trim() == "" && validaPedido)
            {
                labPrevEntrega.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoAcessorio = 0;
                labErroAlteracao.Text = "";

                bool validaPedido = false;
                if (txtQuantidade.Text.Trim() == "" || txtPreco.Text.Trim() == "" || txtDataPrevEntrega.Text.Trim() == "")
                    validaPedido = true;

                if (!ValidarCampos(validaPedido))
                {
                    labErroAlteracao.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                    return;
                }

                codigoAcessorio = Convert.ToInt32(hidCodigoAcessorio.Value);

                DESENV_ACESSORIO _acessorio = null;
                _acessorio = desenvController.ObterAcessorio(codigoAcessorio);

                _acessorio.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue.Trim());
                _acessorio.GRUPO = ddlGrupo.SelectedValue.Trim();
                _acessorio.PRODUTO = txtProduto.Text.Trim().ToUpper();
                _acessorio.GRIFFE = ddlGriffe.SelectedValue.Trim();

                _acessorio.FORNECEDOR = ddlFornecedor.SelectedValue.ToUpper();
                _acessorio.COR = ddlCor.SelectedValue.Trim().ToUpper();
                _acessorio.COR_FORNECEDOR = txtCorFornecedor.Text.Trim().ToUpper();

                _acessorio.PRECO = Convert.ToDecimal(txtPreco.Text.Trim());
                _acessorio.CUSTO = Convert.ToDecimal(txtCusto.Text.Trim());
                _acessorio.QTDE = Convert.ToInt32(txtQuantidade.Text.Trim());
                _acessorio.DESCRICAO_SUGERIDA = txtDescricaoSugerida.Text.Trim().ToUpper();
                _acessorio.REFER_FABRICANTE = txtReferFabricante.Text.Trim().ToUpper();
                _acessorio.DATA_PREVISAO_ENTREGA = txtDataPrevEntrega.Text.Trim().ToUpper();

                _acessorio.OBS = txtObservacao.Text.Trim().ToUpper();

                _acessorio.DATA_INCLUSAO = DateTime.Now;
                _acessorio.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _acessorio.STATUS = 'A';

                desenvController.AtualizarAcessorio(_acessorio);

                //string pedido = "";
                //string msgErroCadastro = "";
                //if (validaPedido)
                //{

                //    //validar produto cadastrado
                //    var produtoCor = new BaseController().BuscaProdutoCor(_acessorio.PRODUTO, _acessorio.COR);
                //    if (produtoCor != null)
                //    {
                //        DESENV_ACESSORIO_PEDIDO pedidoAcessorio = new DESENV_ACESSORIO_PEDIDO();
                //        pedidoAcessorio.DESENV_ACESSORIO = codigoAcessorio;
                //        pedidoAcessorio.PRECO = Convert.ToDecimal(txtPreco.Text.Trim());
                //        pedidoAcessorio.QTDE_ORIGINAL = Convert.ToInt32(txtQuantidade.Text.Trim());
                //        pedidoAcessorio.QTDE_O1 = 0;
                //        pedidoAcessorio.QTDE_O2 = 0;
                //        pedidoAcessorio.QTDE_O3 = 0;
                //        pedidoAcessorio.QTDE_O4 = 0;
                //        pedidoAcessorio.QTDE_O5 = 0;
                //        pedidoAcessorio.QTDE_O6 = 0;
                //        pedidoAcessorio.QTDE_O7 = 0;
                //        pedidoAcessorio.QTDE_O8 = 0;
                //        pedidoAcessorio.QTDE_O9 = 0;
                //        pedidoAcessorio.QTDE_O10 = 0;
                //        pedidoAcessorio.DATA_PEDIDO = DateTime.Now;
                //        pedidoAcessorio.USUARIO_PEDIDO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                //        pedidoAcessorio.DATA_PREV_ENTREGA = Convert.ToDateTime(txtDataPrevEntrega.Text.Trim());
                //        desenvController.InserirAcessorioPedido(pedidoAcessorio);

                //        //GERAR PEDIDO DE COMPRA NO LINX
                //        pedido = GerarPedidoCompraAcessorio(pedidoAcessorio);

                //        pedidoAcessorio.PEDIDO = pedido;
                //        desenvController.AtualizarAcessorioPedido(pedidoAcessorio);
                //    }
                //    else
                //    {
                //        msgErroCadastro = "<strong>Erro</strong> ao gerar PEDIDO DE COMPRA. <strong>PRODUTO/COR não cadastrado</strong>.";
                //    }

                //    CarregarHistoricoPedido(_acessorio);
                //}

                //string msgResposta = "";
                //if (validaPedido)
                //{
                //    if (pedido != null && pedido != "")
                //        msgResposta = "PEDIDO DE COMPRA \"<strong>" + pedido.ToUpper().Trim() + "</strong>\" gerado com Sucesso.";
                //    else
                //        msgResposta = "Erro ao gerar PEDIDO DE COMPRA. Entre em contato com TI.";

                //    if (msgErroCadastro != "")
                //        msgResposta = msgErroCadastro;
                //}
                //else
                //{
                //    msgResposta = "Acessório atualizado com Sucesso.";
                //}
                //labErroAlteracao.Text = msgResposta;

                labErroAlteracao.Text = "Acessório atualizado com Sucesso.";

                if (Constante.enviarEmail)
                    EnviarEmail(_acessorio);

                txtQuantidade.Text = "";
                txtPreco.Text = "";
                txtDataPrevEntrega.Text = "";
            }
            catch (Exception ex)
            {
                labErroAlteracao.Text = ex.Message;
            }
        }
        //private string GerarPedidoCompraAcessorio(DESENV_ACESSORIO_PEDIDO pedidoAcessorio)
        //{
        //    string pedido = "";
        //    string aprovadoPor = "";

        //    aprovadoPor = "INTRANET-" + ((USUARIO)Session["USUARIO"]).NOME_USUARIO;

        //    var pedidoCompra = desenvController.GerarPedidoCompraAcessorio(
        //        pedidoAcessorio.DESENV_ACESSORIO1.PRODUTO,
        //        pedidoAcessorio.DESENV_ACESSORIO1.COR,
        //        pedidoAcessorio.DESENV_ACESSORIO1.FORNECEDOR,
        //        pedidoAcessorio.QTDE_ORIGINAL,
        //        aprovadoPor,
        //        pedidoAcessorio.DATA_PREV_ENTREGA
        //        );

        //    if (pedidoCompra != null)
        //        pedido = pedidoCompra.NUMERO_PEDIDO;

        //    return pedido;
        //}
        private void CarregarHistoricoPedido(DESENV_ACESSORIO _acessorio)
        {
            var pedidoAcessorio = desenvController.ObterAcessorioPedido(_acessorio.CODIGO.ToString());
            pedidoAcessorio = pedidoAcessorio.Where(p => p.PEDIDO != null && p.PEDIDO.Trim() != "").ToList();

            gvHistoricoPedido.DataSource = pedidoAcessorio.OrderBy(p => p.DATA_PEDIDO);
            gvHistoricoPedido.DataBind();
        }
        protected void gvHistoricoPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_ACESSORIO_PEDIDO _pedidoAcessorio = e.Row.DataItem as DESENV_ACESSORIO_PEDIDO;

                    if (_pedidoAcessorio != null)
                    {
                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                            _litPreco.Text = "R$ " + _pedidoAcessorio.PRECO.ToString("###,###,###,##0.00");

                        Literal _litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        if (_litDataPedido != null)
                            _litDataPedido.Text = _pedidoAcessorio.DATA_PEDIDO.ToString("dd/MM/yyyy");

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                            _litValorTotal.Text = "R$ " + (_pedidoAcessorio.QTDE_ORIGINAL * _pedidoAcessorio.PRECO).ToString("###,###,###,##0.00");

                        qtdeOriginal += _pedidoAcessorio.QTDE_ORIGINAL;
                        valorTotal += (_pedidoAcessorio.QTDE_ORIGINAL * _pedidoAcessorio.PRECO);

                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                        {
                            if (_pedidoAcessorio.PEDIDO == null || _pedidoAcessorio.PEDIDO.Trim() == "")
                            {
                                _litStatus.Text = "Erro ao gerar pedido";
                                e.Row.ForeColor = Color.Red;
                            }
                            else
                            {
                                _litStatus.Text = "OK";
                            }
                        }

                    }
                }
            }
        }
        protected void gvHistoricoPedido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvHistoricoPedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = qtdeOriginal.ToString();
                footer.Cells[5].Text = "R$ " + valorTotal.ToString("###,###,###,##0.00");
            }
        }

        #region "FOTO"
        protected void ddlQtdeFoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdbFoto1.Checked = true;
            if (hidFoto2.Value == "" && ddlQtdeFoto.SelectedValue == "1")
            {
                rdbFoto2.Visible = false;
                imgFoto2.Visible = false;
                imgFoto2.ImageUrl = "";
            }
            else
            {
                rdbFoto2.Visible = true;
                imgFoto2.Visible = true;
                ddlQtdeFoto.SelectedValue = "2";
            }
        }
        protected void btCarregarFoto_Click(object sender, EventArgs e)
        {
            bool dropped = false;

            labErro.Text = "";
            if (Session["DROP_IMAGE"] != null)
            {
                if (rdbFoto1.Checked)
                    imgFoto.ImageUrl = Session["DROP_IMAGE"].ToString();
                if (rdbFoto2.Checked)
                    imgFoto2.ImageUrl = Session["DROP_IMAGE"].ToString();
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
                        for (int i = 1; i < fileCollection.Count; i++)
                        {
                            dropped = true;
                            HttpPostedFile upload = fileCollection[i];
                            streamFoto = upload.InputStream;
                            pathFile = fileCollection.Keys[1].ToString();
                        }
                    }

                    if (streamFoto != null)
                    {

                        urlFoto = GravarImagem(streamFoto, pathFile);
                        if (urlFoto.Contains("ERRO"))
                            throw new Exception(urlFoto);
                        if (dropped)
                            Session["DROP_IMAGE"] = urlFoto;

                        if (rdbFoto1.Checked)
                            imgFoto.ImageUrl = urlFoto;
                        if (rdbFoto2.Checked)
                            imgFoto2.ImageUrl = urlFoto;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
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

                int newWidth2Image = 300;
                if (ddlQtdeFoto.SelectedValue == "2")
                    newWidth2Image = 149;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > newWidth2Image || newHeight > 320)
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

        #region "ACOES"
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labSalvar.Text = "";
                labExclusao.Text = "";

                if (imgFoto.ImageUrl.Trim() == "")
                {
                    labSalvar.Text = "Selecione a primeira IMAGEM para o Acessório.";
                    return;
                }
                if (imgFoto2.ImageUrl.Trim() == "" && ddlQtdeFoto.SelectedValue == "2")
                {
                    labSalvar.Text = "Selecione a segunda IMAGEM para o Acessório.";
                    return;
                }

                int codigoAcessorio = 0;
                codigoAcessorio = (hidCodigo.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigo.Value);

                if (codigoAcessorio > 0)
                {
                    DESENV_ACESSORIO _acessorio = new DESENV_ACESSORIO();
                    _acessorio = desenvController.ObterAcessorio(codigoAcessorio);
                    if (_acessorio != null)
                    {
                        //BUSCAR LISTA
                        List<DESENV_ACESSORIO> listaAcessoriosFiltro = new List<DESENV_ACESSORIO>();
                        listaAcessoriosFiltro = desenvController.ObterAcessorio(_acessorio.COLECAO).Where(p =>
                                                                                        p.PRODUTO != null &&
                                                                                        p.PRODUTO.Trim().ToUpper() == _acessorio.PRODUTO.Trim().ToUpper()
                                                                                        ).ToList();
                        foreach (DESENV_ACESSORIO acess in listaAcessoriosFiltro)
                        {
                            if (acess != null)
                            {
                                acess.FOTO1 = imgFoto.ImageUrl;
                                if (ddlQtdeFoto.SelectedValue == "2")
                                    acess.FOTO2 = imgFoto2.ImageUrl;
                                desenvController.AtualizarAcessorio(acess);
                            }
                        }

                        labSalvar.Text = "Foto incluída com sucesso.";

                        //Carregar Pocket
                        _acessorio.FOTO1 = imgFoto.ImageUrl;
                        _acessorio.FOTO2 = imgFoto2.ImageUrl;
                        CarregarPocket(_acessorio);
                    }
                }
            }
            catch (Exception ex)
            {
                labSalvar.Text = ex.Message;
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labSalvar.Text = "";
                labExclusao.Text = "";

                int codigoAcessorio = 0;
                codigoAcessorio = (hidCodigo.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigo.Value);

                if (codigoAcessorio > 0)
                {
                    DESENV_ACESSORIO _acessorio = new DESENV_ACESSORIO();
                    _acessorio = desenvController.ObterAcessorio(codigoAcessorio);
                    if (_acessorio != null)
                    {
                        //BUSCAR LISTA
                        List<DESENV_ACESSORIO> listaAcessoriosFiltro = new List<DESENV_ACESSORIO>();
                        listaAcessoriosFiltro = desenvController.ObterAcessorio(_acessorio.COLECAO).Where(p =>
                                                                                        p.PRODUTO != null &&
                                                                                        p.PRODUTO.Trim().ToUpper() == _acessorio.PRODUTO.Trim().ToUpper()
                                                                                        ).ToList();
                        foreach (DESENV_ACESSORIO acess in listaAcessoriosFiltro)
                        {
                            if (acess != null)
                            {
                                Button btExc = (Button)sender;
                                if (btExc != null)
                                {
                                    if (btExc.CommandArgument == "1")
                                    {
                                        acess.FOTO1 = "";
                                        acess.FOTO2 = "";
                                        imgFoto.ImageUrl = "";
                                        imgFoto2.ImageUrl = "";
                                    }
                                    desenvController.AtualizarAcessorio(acess);
                                }
                            }
                        }

                        labExclusao.Text = "Foto excluída com sucesso.";

                        //Carregar foto no pocket
                        //_produto.FOTO = imgFoto.ImageUrl;
                        //_produto.FOTO2 = imgFoto.ImageUrl;
                        CarregarPocket(_acessorio);
                    }
                }
            }
            catch (Exception ex)
            {
                labExclusao.Text = ex.Message;
            }
        }
        #endregion

        #region "POCKET"
        private void CarregarPocket(DESENV_ACESSORIO acessorio)
        {
            HtmlGenericControl divPocket;
            try
            {
                List<DESENV_ACESSORIO> listaAcessorios = new List<DESENV_ACESSORIO>();

                listaAcessorios.Add(acessorio);
                if (listaAcessorios.Count > 0)
                {
                    //Descarregar hMTL na DIV
                    divPocket = new HtmlGenericControl();
                    divPocket.InnerHtml = Pocket.MontarPocketAcessorio(listaAcessorios, false).ToString();
                    pnlPocket.Controls.Add(divPocket);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region "EMAIL"
        private void EnviarEmail(DESENV_ACESSORIO produtos)
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Alteração de Cadastro de Acessórios - " + produtos.PRODUTO;
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail(produtos);

            List<string> destinatario = new List<string>();
            //Adiciona e-mails
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(37, 13).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(DESENV_ACESSORIO produtos)
        {
            int codigoUsuario = 0;
            codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            BaseController baseController = new BaseController();

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Cadastro de Acessório</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divAcessorio' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 600pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:left;'>");
            sb.Append("                        <h3>ALTERAÇÃO de Cadastro de Acessório - " + produtos.PRODUTO + "</h3>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='text-align:center;'>");
            sb.Append("                        <hr />");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='3' style='width: 600pt; padding: 0px;");
            sb.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                            background: white; white-space: nowrap;'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 235px;'>");
            sb.Append("                                    Coleção:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + baseController.BuscaColecaoAtual(produtos.COLECAO).DESC_COLECAO.Trim());
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Produto:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.PRODUTO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            PRODUTO produto = baseController.BuscaProduto(produtos.PRODUTO);
            if (produto != null)
            {
                if (produto.DESC_PRODUTO != null)
                {
                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    Nome:");
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    " + produto.DESC_PRODUTO);
                    sb.Append("                                </td>");
                    sb.Append("                                <td>");
                    sb.Append("                                    &nbsp;");
                    sb.Append("                                </td>");
                    sb.Append("                            </tr>");
                }
            }

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Grupo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.GRUPO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Griffe:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.GRIFFE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            string cores = "";
            ProducaoController prodController = new ProducaoController();
            cores = produtos.COR + " - " + prodController.ObterCoresBasicas(produtos.COR).DESC_COR.Trim();
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + cores);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.COR_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$" + produtos.PRECO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Custo:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    R$" + produtos.CUSTO);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Quantidade:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.QTDE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Previsão de Entrega:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.DATA_PREVISAO_ENTREGA);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Descrição Sugerida:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.DESCRICAO_SUGERIDA);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Referência Fabricante:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.REFER_FABRICANTE);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Observação:");
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    " + produtos.OBS);
            sb.Append("                                </td>");
            sb.Append("                                <td>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                            <tr>");
            sb.Append("                                <td colspan='3'>");
            sb.Append("                                    &nbsp;");
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(codigoUsuario).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

    }
}
