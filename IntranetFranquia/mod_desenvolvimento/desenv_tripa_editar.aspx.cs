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

namespace Relatorios
{
    public partial class desenv_tripa_editar : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoProduto = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                codigoProduto = Convert.ToInt32(Request.QueryString["p"].ToString());
                DESENV_PRODUTO _produto = desenvController.ObterProduto(codigoProduto);
                if (_produto == null)
                    Response.Redirect("desenv_menu.aspx");

                hidFoto2.Value = (_produto.FOTO2 == null) ? "" : _produto.FOTO2;
                ddlQtdeFoto_SelectedIndexChanged(null, null);

                CarregarProduto(codigoProduto);
                hidCodigoProduto.Value = codigoProduto.ToString();
                Session["DROP_IMAGE"] = null;

            }

            if (Request.Files.Keys.Count > 0)
                btCarregarFoto_Click(sender, e);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if (window.File && window.FileList && window.FileReader) { var dropZone = document.getElementById('drop_zone'); dropZone.addEventListener('dragover', handleDragOver, false); dropZone.addEventListener('drop', handleDnDFileSelect, false); } else { alert('Sorry! this browser does not support HTML5 File APIs.'); }", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");
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
            List<PRODUTOS_GRIFFE> griffe = baseController.BuscaGriffes();

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
            _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }
        }
        private void CarregarLinhas()
        {
            var linhas = baseController.BuscaLinhas();

            linhas.Insert(0, new PRODUTOS_LINHA { LINHA = "" });
            ddlLinha.DataSource = linhas;
            ddlLinha.DataBind();
        }

        private void CarregarProduto(int codigoProduto)
        {
            DESENV_PRODUTO _produto = new DESENV_PRODUTO();
            _produto = desenvController.ObterProduto(codigoProduto);
            if (_produto != null)
            {
                CarregarGrupo();
                CarregarOrigem(_produto.COLECAO);
                CarregarGriffe();
                CarregarCores();
                CarregarFornecedores();
                CarregarLinhas();

                ddlOrigem.SelectedValue = _produto.DESENV_PRODUTO_ORIGEM.ToString();
                ddlGrupo.SelectedValue = prodController.ObterGrupoProduto("01").Where(p => p.GRUPO_PRODUTO.Trim() == _produto.GRUPO).SingleOrDefault().GRUPO_PRODUTO;
                txtModelo.Text = _produto.MODELO;
                ddlGriffe.SelectedValue = baseController.BuscaGriffes().Where(p => p.GRIFFE.Trim() == _produto.GRIFFE).SingleOrDefault().GRIFFE;
                txtSegmento.Text = _produto.SEGMENTO;
                txtNome.Text = _produto.DESC_MODELO;
                txtTecido.Text = _produto.TECIDO_POCKET;
                ddlFornecedor.SelectedValue = _produto.FORNECEDOR;
                txtREFModelagem.Text = _produto.REF_MODELAGEM;
                txtPrecoVendaVarejo.Text = _produto.PRECO.ToString();
                txtPrecoVendaAtacado.Text = _produto.PRECO_ATACADO.ToString();
                ddlBrinde.SelectedValue = _produto.BRINDE.ToString();
                ddlProdutoAcabado.SelectedValue = _produto.PRODUTO_ACABADO.ToString();
                ddlCor.SelectedValue = _produto.COR;
                ddlLinha.SelectedValue = _produto.LINHA;
                txtCorFornecedor.Text = _produto.FORNECEDOR_COR;
                txtQtdeMostruario.Text = _produto.QTDE_MOSTRUARIO.ToString();
                hidQtdeMostruario.Value = _produto.QTDE_MOSTRUARIO.ToString();
                txtQtdeVarejo.Text = _produto.QTDE.ToString();
                hidQtdeVarejo.Value = _produto.QTDE.ToString();
                txtQtdeAtacado.Text = _produto.QTDE_ATACADO.ToString();
                hidQtdeAtacado.Value = _produto.QTDE_ATACADO.ToString();

                imgFoto.ImageUrl = (_produto.FOTO == null) ? "" : _produto.FOTO;
                imgFoto2.ImageUrl = (_produto.FOTO2 == null) ? "" : _produto.FOTO2;

                txtObservacao.Text = _produto.OBSERVACAO;
                txtObsImpressao.Text = _produto.OBS_IMPRESSAO;

                hidCodigo.Value = codigoProduto.ToString();

                ddlSigned.SelectedValue = _produto.SIGNED.ToString();
                txtSignedNome.Text = _produto.SIGNED_NOME;

                CarregarPocket(_produto);
            }
        }
        #endregion

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "";
                int codigoProduto = 0;
                labErroAlteracao.Text = "";

                codigoProduto = Convert.ToInt32(hidCodigoProduto.Value);

                if (txtQtdeMostruario.Text.Trim() == "")
                    txtQtdeMostruario.Text = "0";

                if (txtQtdeVarejo.Text.Trim() == "")
                    txtQtdeVarejo.Text = "0";

                if (txtQtdeAtacado.Text.Trim() == "")
                    txtQtdeAtacado.Text = "0";

                //Validação de qtdes
                //if (!ValidarQtde(Convert.ToInt32(txtQtdeVarejo.Text), txtVarejo.Text, labQtdeVarejo, labVarejo) || !ValidarQtde(Convert.ToInt32(txtQtdeAtacado.Text), txtAtacado.Text, labQtdeAtacado, labAtacado))
                //{
                //    labErroAlteracao.Text = "Preencha corretamente os campos em <strong>vermelho</strong>.";
                //    return;
                //}

                /*if (!ValidarQtdeProducao(codigoProduto, Convert.ToInt32(txtQtdeMostruario.Text), "M"))
                {
                    txtQtdeMostruario.Text = hidQtdeMostruario.Value;
                    labErroAlteracao.Text = "Não será possível diminuir a quantidade de MOSTRUÁRIO. Qtde Liberada para o Corte.";
                    return;
                }
                if (!ValidarQtdeProducao(codigoProduto, Convert.ToInt32(txtQtdeVarejo.Text), "V"))
                {
                    txtQtdeVarejo.Text = hidQtdeVarejo.Value;
                    labErroAlteracao.Text = "Não será possível diminuir a quantidade de VAREJO. Qtde Liberada para o Corte.";
                    return;
                }
                if (!ValidarQtdeProducao(codigoProduto, Convert.ToInt32(txtQtdeAtacado.Text), "A"))
                {
                    txtQtdeAtacado.Text = hidQtdeAtacado.Value;
                    labErroAlteracao.Text = "Não será possível diminuir a quantidade de ATACADO. Qtde Liberada para o Corte.";
                    return;
                }*/

                string produtoAux = "";

                var _produto = desenvController.ObterProduto(codigoProduto);

                //guardar código do produto antes da alteração
                produtoAux = _produto.MODELO;

                //_produto.COLECAO = ddlColecoes.Text.Trim();
                //_produto.CODIGO_REF = Convert.ToInt32(txtCodigoRef.Text);
                _produto.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue.Trim());
                _produto.GRUPO = ddlGrupo.SelectedValue.Trim();
                _produto.MODELO = txtModelo.Text.Trim().ToUpper();
                _produto.GRIFFE = ddlGriffe.SelectedValue.Trim();

                _produto.SEGMENTO = txtSegmento.Text.Trim().ToUpper();
                _produto.TECIDO_POCKET = txtTecido.Text.Trim().ToUpper();
                _produto.FORNECEDOR = ddlFornecedor.SelectedValue.ToUpper();

                _produto.CORTE_PETIT = "";

                _produto.DESC_MODELO = txtNome.Text.Trim().ToUpper();
                _produto.REF_MODELAGEM = txtREFModelagem.Text.Trim().ToUpper();

                if (txtPrecoVendaVarejo.Text != "")
                    _produto.PRECO = Convert.ToDecimal(txtPrecoVendaVarejo.Text);
                else
                    _produto.PRECO = 0;
                if (txtPrecoVendaAtacado.Text != "")
                    _produto.PRECO_ATACADO = Convert.ToDecimal(txtPrecoVendaAtacado.Text);
                else
                    _produto.PRECO_ATACADO = 0;

                if (ddlBrinde.SelectedValue != "")
                    _produto.BRINDE = Convert.ToChar(ddlBrinde.SelectedValue);
                if (ddlProdutoAcabado.SelectedValue != "")
                    _produto.PRODUTO_ACABADO = Convert.ToChar(ddlProdutoAcabado.SelectedValue);

                _produto.COR = ddlCor.SelectedValue.Trim().ToUpper();
                _produto.FORNECEDOR_COR = txtCorFornecedor.Text.Trim().ToUpper();

                _produto.LINHA = ddlLinha.SelectedValue;

                if (txtQtdeMostruario.Text != "")
                    _produto.QTDE_MOSTRUARIO = Convert.ToInt32(txtQtdeMostruario.Text);
                else
                    _produto.QTDE_MOSTRUARIO = 0;

                _produto.CORTE_VAREJO = "";
                if (txtQtdeVarejo.Text != "")
                {
                    _produto.QTDE = Convert.ToInt32(txtQtdeVarejo.Text);
                    if (txtQtdeVarejo.Text != "0")
                        _produto.CORTE_VAREJO = "OK";
                }
                else
                    _produto.QTDE = 0;

                _produto.CORTE_ATACADO = "";
                if (txtQtdeAtacado.Text != "")
                {
                    _produto.QTDE_ATACADO = Convert.ToInt32(txtQtdeAtacado.Text);
                    if (txtQtdeAtacado.Text != "0")
                        _produto.CORTE_ATACADO = "OK";
                }
                else
                    _produto.QTDE_ATACADO = 0;

                _produto.OBSERVACAO = txtObservacao.Text.Trim().ToUpper();
                _produto.OBS_IMPRESSAO = txtObsImpressao.Text;

                _produto.PLAN_VAREJO = 'N';
                _produto.PLAN_ATACADO = 'N';

                _produto.DATA_INCLUSAO = DateTime.Now;
                _produto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _produto.STATUS = 'A';

                if (ddlSigned.SelectedValue != "")
                    _produto.SIGNED = Convert.ToChar(ddlSigned.SelectedValue);

                _produto.SIGNED_NOME = txtSignedNome.Text.Trim().ToUpper();

                desenvController.AtualizarProduto(_produto);

                AtualizarPrePedido(produtoAux, _produto);

                //Gerar qtde para produção do Modelo
                /*if (_produto.DATA_APROVACAO != null && _produto.DATA_FICHATECNICA != null)
                {
                    int qtdeVarejo = 0;
                    int qtdeAtacado = 0;
                    int qtdeMostruario = 0;

                    if (txtQtdeVarejo != null && txtQtdeVarejo.Text.Trim() != "")
                        if (txtQtdeVarejo.Text != hidQtdeVarejo.Value)
                        {
                            qtdeVarejo = Convert.ToInt32(txtQtdeVarejo.Text) - ((hidQtdeVarejo.Value != "") ? Convert.ToInt32(hidQtdeVarejo.Value) : 0);
                            hidQtdeVarejo.Value = txtQtdeVarejo.Text;
                        }

                    if (txtQtdeAtacado != null && txtQtdeAtacado.Text.Trim() != "")
                        if (txtQtdeAtacado.Text != hidQtdeAtacado.Value)
                        {
                            qtdeAtacado = Convert.ToInt32(txtQtdeAtacado.Text) - ((hidQtdeAtacado.Value != "") ? Convert.ToInt32(hidQtdeAtacado.Value) : 0);
                            hidQtdeAtacado.Value = txtQtdeAtacado.Text;
                        }

                    if (txtQtdeMostruario != null && txtQtdeMostruario.Text.Trim() != "")
                        if (txtQtdeMostruario.Text != hidQtdeMostruario.Value)
                        {
                            qtdeMostruario = Convert.ToInt32(txtQtdeMostruario.Text) - ((hidQtdeMostruario.Value != "") ? Convert.ToInt32(hidQtdeMostruario.Value) : 0);
                            hidQtdeMostruario.Value = txtQtdeMostruario.Text;
                        }

                    if (qtdeVarejo != 0 || qtdeAtacado != 0 || qtdeMostruario != 0)
                    {
                        _produto.QTDE = qtdeVarejo;
                        _produto.QTDE_ATACADO = qtdeAtacado;
                        _produto.QTDE_MOSTRUARIO = qtdeMostruario;
                        InserirQtdeProdutoProducao(_produto, true);

                        msg = "Gerado Registro de Compra.";
                    }
                }*/

                CarregarPocket(_produto);

                labErroAlteracao.Text = "Produto alterado com sucesso.\n" + msg;

            }
            catch (Exception ex)
            {
                labErroAlteracao.Text = ex.Message;
            }
        }

        private void AtualizarPrePedido(string produto, DESENV_PRODUTO desenvProduto)
        {
            var prePedido = desenvController.ObterPrePedido(desenvProduto.COLECAO, produto, desenvProduto.COR);
            if (prePedido != null)
            {
                prePedido.PRODUTO = desenvProduto.MODELO;
                prePedido.PRECO_VENDA = Convert.ToDecimal(desenvProduto.PRECO);
                prePedido.QTDE_VAREJO = Convert.ToInt32(desenvProduto.QTDE);
                prePedido.QTDE_ATACADO = Convert.ToInt32(desenvProduto.QTDE_ATACADO);

                prePedido.CONSUMO_TOTAL = ((prePedido.QTDE_ATACADO + prePedido.QTDE_VAREJO) * prePedido.CONSUMO);
                prePedido.VALOR_TOTAL_ATACADO = ((prePedido.PRECO_VENDA / 2) * prePedido.QTDE_ATACADO);
                prePedido.VALOR_TOTAL_VAREJO = ((prePedido.PRECO_VENDA) * prePedido.QTDE_VAREJO);

                desenvController.AtualizarPrePedido(prePedido);
            }
        }
        protected void txtPreco_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtPrecoVarejo = (TextBox)sender;
                if (txtPrecoVarejo != null)
                {
                    decimal precoAtacado = 0;
                    if (txtPrecoVarejo.Text.Trim() != "" && Convert.ToDecimal(txtPrecoVarejo.Text.Trim()) > 0) //PRECO VAREJO MAIOR QUE ZERO, GERA PRECO ATACADO AUTOMATICAMENTE
                    {
                        precoAtacado = Math.Round((Convert.ToDecimal(txtPrecoVarejo.Text.Trim()) / 2), 0); //PRECO VAREJO INFORMADO DIVIDIDO por 2
                        txtPrecoVendaAtacado.Text = precoAtacado.ToString();
                    }
                }

                var _produto = desenvController.ObterProduto(Convert.ToInt32(hidCodigoProduto.Value));
                if (_produto != null)
                    CarregarPocket(_produto);
            }
            catch (Exception ex)
            {
                labErroAlteracao.Text = ex.Message;
            }

        }
        private bool ValidarQtde(int qtde, string corte, Label labQtde, Label labCorte)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labQtde.ForeColor = _OK;
            labCorte.ForeColor = _OK;
            if ((qtde <= 0 && corte.Trim().ToUpper() == "OK") || (qtde > 0 && corte.Trim().ToUpper() != "OK"))
            {
                retorno = false;
                labQtde.ForeColor = _notOK;
                labCorte.ForeColor = _notOK;
            }

            return retorno;
        }
        private bool ValidarQtdeProducao(int desenv_produto, int qtde, string tipo)
        {
            bool retorno = true;

            var _produtoProducao = desenvController.ObterProdutoProducao(desenv_produto);
            if (_produtoProducao != null && _produtoProducao.Count() > 0)
            {
                var _producaoQtde = _produtoProducao.Where(p => p.TIPO.ToString() == tipo && p.PROD_DETALHE == null && p.STATUS != null).ToList().Sum(x => x.QTDE);
                if (_producaoQtde > 0)
                    if (qtde < _producaoQtde)
                        retorno = false;
            }

            return retorno;
        }
        /*private void InserirQtdeProdutoProducao(DESENV_PRODUTO _produto, bool _edit)
        {
            List<DESENV_PRODUTO_FICTEC> _fichaTecnica = null;
            _fichaTecnica = desenvController.ObterFichaTecnica(_produto.CODIGO);

            DESENV_PRODUTO_PRODUCAO _produtoProducao = null;
            int codigoProdutoProducao = 0;

            string msg = "ALTERADA";
            if (!_edit)
                msg = "EXCLUÍDA";

            foreach (DESENV_PRODUTO_FICTEC ft in _fichaTecnica)
            {
                if (_produto.QTDE_MOSTRUARIO != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'M';
                    _produtoProducao.QTDE = (_produto.QTDE_MOSTRUARIO == null) ? 0 : Convert.ToInt32(_produto.QTDE_MOSTRUARIO);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS MOSTRUÁRIO - " + msg));
                }

                if (_produto.QTDE != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'V';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS VAREJO - " + msg));
                }

                if (_produto.QTDE_ATACADO != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'A';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE_ATACADO);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS ATACADO - " + msg));
                }
            }
        }*/

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
                    labSalvar.Text = "Selecione a primeira IMAGEM para o Produto.";
                    return;
                }
                if (imgFoto2.ImageUrl.Trim() == "" && ddlQtdeFoto.SelectedValue == "2")
                {
                    labSalvar.Text = "Selecione a segunda IMAGEM para o Produto.";
                    return;
                }

                int codigoProduto = 0;
                codigoProduto = (hidCodigo.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigo.Value);

                if (codigoProduto > 0)
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(codigoProduto);
                    if (_produto != null)
                    {
                        //Lista NOVA
                        List<DESENV_PRODUTO> listaProdutosNova = new List<DESENV_PRODUTO>();
                        //BUSCAR LISTA
                        List<DESENV_PRODUTO> listaProdutosFiltro = new List<DESENV_PRODUTO>();
                        listaProdutosFiltro = desenvController.ObterProduto().Where(p =>
                                                                                        p.COLECAO == _produto.COLECAO &&
                                                                                        p.MODELO != null &&
                                                                                        p.MODELO.Trim().ToUpper() == _produto.MODELO.Trim().ToUpper()
                                                                                        ).ToList();
                        foreach (DESENV_PRODUTO prod in listaProdutosFiltro)
                        {
                            if (prod != null)
                            {
                                prod.FOTO = imgFoto.ImageUrl;
                                if (ddlQtdeFoto.SelectedValue == "2")
                                    prod.FOTO2 = imgFoto2.ImageUrl;
                                listaProdutosNova.Add(prod);
                            }
                        }
                        //listaProdutos.Add(_produto);
                        desenvController.InserirAtualizarProduto(listaProdutosNova);

                        labSalvar.Text = "Foto incluída com sucesso.";

                        //Carregar Pocket
                        _produto.FOTO = imgFoto.ImageUrl;
                        _produto.FOTO2 = imgFoto2.ImageUrl;
                        CarregarPocket(_produto);
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

                int codigoProduto = 0;
                codigoProduto = (hidCodigo.Value.Trim() == "") ? 0 : Convert.ToInt32(hidCodigo.Value);

                if (codigoProduto > 0)
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto = desenvController.ObterProduto(codigoProduto);
                    if (_produto != null)
                    {
                        //Lista NOVA
                        List<DESENV_PRODUTO> listaProdutosNova = new List<DESENV_PRODUTO>();
                        //BUSCAR LISTA
                        List<DESENV_PRODUTO> listaProdutosFiltro = new List<DESENV_PRODUTO>();
                        listaProdutosFiltro = desenvController.ObterProduto().Where(p =>
                                                                                        p.COLECAO == _produto.COLECAO &&
                                                                                        p.MODELO != null &&
                                                                                        p.MODELO.Trim().ToUpper() == _produto.MODELO.Trim().ToUpper()
                                                                                        ).ToList();
                        foreach (DESENV_PRODUTO prod in listaProdutosFiltro)
                        {
                            if (prod != null)
                            {
                                Button btExc = (Button)sender;
                                if (btExc != null)
                                {
                                    if (btExc.CommandArgument == "1")
                                    {
                                        prod.FOTO = "";
                                        prod.FOTO2 = "";
                                        imgFoto.ImageUrl = "";
                                        imgFoto2.ImageUrl = "";
                                    }
                                    listaProdutosNova.Add(prod);
                                }
                            }
                        }
                        //listaProdutos.Add(_produto);
                        desenvController.InserirAtualizarProduto(listaProdutosNova);

                        labExclusao.Text = "Foto excluída com sucesso.";

                        //Carregar foto no pocket
                        //_produto.FOTO = imgFoto.ImageUrl;
                        //_produto.FOTO2 = imgFoto.ImageUrl;
                        CarregarPocket(_produto);
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
        private void CarregarPocket(DESENV_PRODUTO produto)
        {
            HtmlGenericControl divPocket;
            try
            {
                List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();

                listaProdutos.Add(produto);
                if (listaProdutos.Count > 0)
                {
                    //Descarregar hMTL na DIV
                    divPocket = new HtmlGenericControl();
                    divPocket.InnerHtml = Pocket.MontarPocket(listaProdutos, false).ToString();
                    pnlPocket.Controls.Add(divPocket);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
