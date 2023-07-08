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
using Relatorios.mod_desenvolvimento.relatorios;

namespace Relatorios
{
    public partial class desenv_material_compra_entrada : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataEntrada.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimento1.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimento2.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimento3.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimento4.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimento5.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtVencimento6.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                int codigoPedido = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                codigoPedido = Convert.ToInt32(Request.QueryString["p"].ToString());
                DESENV_PEDIDO _pedido = desenvController.ObterPedido(codigoPedido);
                if (_pedido == null)
                    Response.Redirect("desenv_menu.aspx");

                CarregarPedido(_pedido);
                hidCodigoPedido.Value = codigoPedido.ToString();
                Session["DROP_IMAGE"] = null;
            }

            if (Request.Files.Keys.Count > 0)
                btCarregarFoto_Click(sender, e);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if (window.File && window.FileList && window.FileReader) { var dropZone = document.getElementById('drop_zone'); dropZone.addEventListener('dragover', handleDragOver, false); dropZone.addEventListener('drop', handleDnDFileSelect, false); } else { alert('Sorry! this browser does not support HTML5 File APIs.'); }", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarPedido(DESENV_PEDIDO pedido)
        {
            if (pedido != null)
            {
                txtNumeroPedido.Text = pedido.NUMERO_PEDIDO.ToString();
                txtGrupoTecido.Text = pedido.GRUPO;
                txtSubGrupoTecido.Text = pedido.SUBGRUPO;
                txtCor.Text = prodController.ObterCoresBasicas(pedido.COR).DESC_COR;
                txtDescricaoCor.Text = pedido.COR_FORNECEDOR;
                txtFornecedor.Text = pedido.FORNECEDOR;

                imgFoto.ImageUrl = (pedido.FOTO_TECIDO == null) ? "" : pedido.FOTO_TECIDO;
            }
        }

        private void LimparCampos()
        {
            txtDataEntrada.Text = "";
            txtQuantidade.Text = "";
            txtNF.Text = "";
            txtValorNF.Text = "";
            txtProdutoFornecedor.Text = "";
            txtLargura.Text = "";
            txtVolume.Text = "";
            txtVencimento1.Text = "";
            txtVencimento2.Text = "";
            txtVencimento3.Text = "";
            txtVencimento4.Text = "";
            txtVencimento5.Text = "";
            txtVencimento6.Text = "";
        }

        #endregion

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "";
                int codigoPedido = 0;
                labErroAlteracao.Text = "";

                if (txtDataEntrada.Text == "")
                {
                    labErroAlteracao.Text = "Informe a Data de Entrada.";
                    return;
                }
                if (txtQuantidade.Text == "")
                {
                    labErroAlteracao.Text = "Informe a quantidade de Entrada";
                    return;
                }
                if (txtNF.Text == "")
                {
                    labErroAlteracao.Text = "Informe o Número da NF.";
                    return;
                }

                if (txtRendimento.Text.Trim() == "")
                {
                    labErroAlteracao.Text = "Informe o rendimento do Tecido.";
                    return;
                }

                codigoPedido = Convert.ToInt32(hidCodigoPedido.Value);

                DESENV_PEDIDO_QTDE desenvPedidoQtde = new DESENV_PEDIDO_QTDE();
                desenvPedidoQtde.DESENV_PEDIDO = codigoPedido;
                desenvPedidoQtde.DATA = Convert.ToDateTime(txtDataEntrada.Text);
                desenvPedidoQtde.QTDE = Convert.ToDecimal(txtQuantidade.Text);
                desenvPedidoQtde.NOTA_FISCAL = txtNF.Text.Trim();

                if (txtValorNF.Text.Trim() != "")
                    desenvPedidoQtde.VALOR_NOTA = Convert.ToDecimal(txtValorNF.Text);
                desenvPedidoQtde.PRODUTO_FORNECEDOR = txtProdutoFornecedor.Text.Trim().ToUpper();
                if (txtLargura.Text.Trim() != "")
                    desenvPedidoQtde.LARGURA = Convert.ToDecimal(txtLargura.Text);
                if (txtVolume.Text.Trim() != "")
                    desenvPedidoQtde.VOLUME = Convert.ToInt32(txtVolume.Text);

                if (txtVencimento1.Text.Trim() != "")
                    desenvPedidoQtde.VENCIMENTO1 = Convert.ToDateTime(txtVencimento1.Text.Trim());

                if (txtVencimento2.Text.Trim() != "")
                    desenvPedidoQtde.VENCIMENTO2 = Convert.ToDateTime(txtVencimento2.Text.Trim());

                if (txtVencimento3.Text.Trim() != "")
                    desenvPedidoQtde.VENCIMENTO3 = Convert.ToDateTime(txtVencimento3.Text.Trim());

                if (txtVencimento4.Text.Trim() != "")
                    desenvPedidoQtde.VENCIMENTO4 = Convert.ToDateTime(txtVencimento4.Text.Trim());

                if (txtVencimento5.Text.Trim() != "")
                    desenvPedidoQtde.VENCIMENTO5 = Convert.ToDateTime(txtVencimento5.Text.Trim());

                if (txtVencimento6.Text.Trim() != "")
                    desenvPedidoQtde.VENCIMENTO6 = Convert.ToDateTime(txtVencimento6.Text.Trim());

                if (txtRendimento.Text.Trim() != "")
                    desenvPedidoQtde.RENDIMENTO_MT = Convert.ToDecimal(txtRendimento.Text);


                desenvController.InserirPedidoQtde(desenvPedidoQtde);

                if (desenvPedidoQtde.QTDE > 0)
                    ImprimirCartelinha(desenvPedidoQtde);

                labErroAlteracao.Text = "Entrada de Material realizada com sucesso.\n" + msg;

                LimparCampos();
                btAlterar.Enabled = false;
            }
            catch (Exception ex)
            {
                labErroAlteracao.Text = ex.Message;
            }
        }

        #region "FOTO"
        protected void btCarregarFoto_Click(object sender, EventArgs e)
        {
            bool dropped = false;

            labErro.Text = "";
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

                        Session["DROP_IMAGE"] = urlFoto;
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
                _fileName = Guid.NewGuid() + "_TECIDO" + _ext;
                _path = Server.MapPath("~/Image_TECIDO/") + _fileName;

                //Obter stream da imagem
                if (_stream != null)
                    GenerateThumbnails(1, _stream, _path);

                retornoImagem = "~/Image_TECIDO/" + _fileName;
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
                    labSalvar.Text = "Selecione a Foto para o Material.";
                    return;
                }

                int codigoPedido = 0;
                codigoPedido = (hidCodigoPedido.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigoPedido.Value);

                if (codigoPedido > 0)
                {
                    DESENV_PEDIDO _pedido = new DESENV_PEDIDO();
                    _pedido = desenvController.ObterPedido(codigoPedido);
                    if (_pedido != null)
                    {
                        _pedido.FOTO_TECIDO = imgFoto.ImageUrl;
                        desenvController.AtualizarPedido(_pedido);

                        labSalvar.Text = "Foto Material incluída com sucesso.";
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

                int codigoPedido = 0;
                codigoPedido = (hidCodigoPedido.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigoPedido.Value);

                if (codigoPedido > 0)
                {
                    DESENV_PEDIDO pedido = new DESENV_PEDIDO();
                    pedido = desenvController.ObterPedido(codigoPedido);
                    if (pedido != null)
                    {
                        pedido.FOTO_TECIDO = "";
                        desenvController.AtualizarPedido(pedido);
                        labExclusao.Text = "Foto Material excluída com sucesso.";
                        imgFoto.ImageUrl = "";
                    }
                }
            }
            catch (Exception ex)
            {
                labExclusao.Text = ex.Message;
            }
        }
        #endregion


        #region "IMPRESSAO"
        private void ImprimirCartelinha(DESENV_PEDIDO_QTDE desenvPedidoQtde)
        {
            StreamWriter wr = null;
            try
            {
                labErroAlteracao.Text = "";

                string nomeArquivo = "ENT_MAT_CARTE" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(Cartelinha.MontarCartelinha(desenvPedidoQtde));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

            }
            catch (Exception ex)
            {
                labErroAlteracao.Text = "ERRO: " + ex.Message;
            }
            finally
            {
                wr.Close();
            }

        }
        #endregion
    }
}
