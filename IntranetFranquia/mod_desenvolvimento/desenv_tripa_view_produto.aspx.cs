using DAL;
using Relatorios.mod_desenvolvimento.modelo_pocket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_tripa_view_produto : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoPrePedido = 0;
                int codigoOrigem = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null
                    || Request.QueryString["o"] == null || Request.QueryString["o"] == "")
                    Response.Redirect("desenv_menu.aspx");

                codigoPrePedido = Convert.ToInt32(Request.QueryString["p"].ToString());
                codigoOrigem = Convert.ToInt32(Request.QueryString["o"].ToString());
                var prePedido = desenvController.ObterPrePedido(codigoPrePedido);
                if (prePedido == null)
                    Response.Redirect("desenv_menu.aspx");

                //CARREGAR FILTROS
                CarregarGrupo();
                CarregarOrigem(prePedido.COLECAO, codigoOrigem);
                CarregarGriffe();
                CarregarCores();
                CarregarFornecedores();
                CarregarLinhas();

                hidCodigoPrePedido.Value = codigoPrePedido.ToString();
                hidColecao.Value = prePedido.COLECAO;
                hidProduto.Value = prePedido.PRODUTO;
                hidMarca.Value = prePedido.MARCA;

                //TRATAMENTO DO PRODUTO
                if (prePedido.PRODUTO == null)
                {
                    prePedido.PRODUTO = "";
                    btHandbook.Visible = true;
                    btCmax.Visible = true;
                }
                //Obter o produto no desenvolvimento de Produto
                var desenvProduto = desenvController.ObterProduto(prePedido.COLECAO, prePedido.PRODUTO, prePedido.COR);
                if (desenvProduto != null)
                {
                    pnlFoto.Visible = true;

                    hidFoto2.Value = (desenvProduto.FOTO2 == null) ? "" : desenvProduto.FOTO2;
                    ddlQtdeFoto_SelectedIndexChanged(null, null);

                    CarregarProduto(desenvProduto.CODIGO);
                    hidCodigoProduto.Value = desenvProduto.CODIGO.ToString();
                }
                else
                {
                    CarregarProdutoPrePedido(prePedido);
                }

                Session["DROP_IMAGE"] = null;
            }

            btAlterar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAlterar, null) + ";");

            if (Request.Files.Keys.Count > 0)
                btCarregarFoto_Click(sender, e);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "if (window.File && window.FileList && window.FileReader) { var dropZone = document.getElementById('drop_zone'); dropZone.addEventListener('dragover', handleDragOver, false); dropZone.addEventListener('drop', handleDnDFileSelect, false); } else { alert('Sorry! this browser does not support HTML5 File APIs.'); }", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            var grupo = prodController.ObterGrupoProduto("01");
            if (grupo != null)
            {
                grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col, int codigoOrigem)
        {
            var origem = desenvController.ObterProdutoOrigem().Where(i => i.COLECAO.Trim() == col.Trim() && i.STATUS == 'A').OrderBy(i => i.DESCRICAO).ToList();
            if (origem != null)
            {
                origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = origem;
                ddlOrigem.DataBind();

                if (origem.Count == 2)
                    ddlOrigem.SelectedValue = origem[1].CODIGO.ToString();
                else
                    ddlOrigem.SelectedValue = codigoOrigem.ToString();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }
        private void CarregarCores()
        {
            var coresBasicas = prodController.ObterCoresBasicas().ToList();
            coresBasicas = coresBasicas.GroupBy(p => new { COR = p.COR.Trim(), DESC_COR = p.DESC_COR.Trim() }).Select(x => new CORES_BASICA { COR = x.Key.COR.Trim(), DESC_COR = x.Key.DESC_COR.Trim() }).ToList();
            if (coresBasicas != null)
            {
                coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

                ddlCor.DataSource = coresBasicas;
                ddlCor.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            var fornecedores = prodController.ObterFornecedor().ToList();
            fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            fornecedores = fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'T') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (fornecedores != null)
            {
                ddlFornecedor.DataSource = fornecedores;
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

        private bool ValidarCampos()
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
            if (txtModelo.Text.Trim() == "")
            {
                labProduto.ForeColor = _notOK;
                retorno = false;
            }

            labGriffe.ForeColor = _OK;
            if (ddlGriffe.SelectedValue == "")
            {
                labGriffe.ForeColor = _notOK;
                retorno = false;
            }

            labTecido.ForeColor = _OK;
            if (txtTecido.Text.Trim() == "")
            {
                labTecido.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedor.ForeColor = _OK;
            if (ddlFornecedor.SelectedValue.Trim() == "")
            {
                labFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupo.ForeColor = _OK;
            if (ddlSubGrupo.SelectedValue.Trim() == "")
            {
                labSubGrupo.ForeColor = _notOK;
                retorno = false;
            }


            //labNomeProduto.ForeColor = _OK;
            //if (txtNome.Text.Trim() == "")
            //{
            //    labNomeProduto.ForeColor = _notOK;
            //    retorno = false;
            //}

            labPrecoVendaAtacado.ForeColor = _OK;
            if (txtPrecoVendaAtacado.Text == "")
            {
                labPrecoVendaAtacado.ForeColor = _notOK;
                retorno = false;
            }

            labPrecoVendaVarejo.ForeColor = _OK;
            if (txtPrecoVendaVarejo.Text == "")
            {
                labPrecoVendaVarejo.ForeColor = _notOK;
                retorno = false;
            }

            labSignedNome.ForeColor = _OK;
            if (ddlSigned.SelectedValue == "S" && txtSignedNome.Text.Trim() == "")
            {
                labSignedNome.ForeColor = _notOK;
                retorno = false;
            }

            labCorLinx.ForeColor = _OK;
            if (ddlCor.SelectedValue == "")
            {
                labCorLinx.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedor.ForeColor = _OK;
            if (txtCorFornecedor.Text.Trim() == "")
            {
                labCorFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            labModDesign.ForeColor = _OK;
            if (ddlModDesign.SelectedValue.Trim() == "")
            {
                labModDesign.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        private void CarregarProduto(int codigoProduto)
        {
            var desenvProduto = desenvController.ObterProduto(codigoProduto);
            if (desenvProduto != null)
            {
                ddlOrigem.SelectedValue = desenvProduto.DESENV_PRODUTO_ORIGEM.ToString();
                ddlGrupo.SelectedValue = prodController.ObterGrupoProduto("01").Where(p => p.GRUPO_PRODUTO.Trim() == desenvProduto.GRUPO).SingleOrDefault().GRUPO_PRODUTO;
                txtModelo.Text = desenvProduto.MODELO;
                ddlGriffe.SelectedValue = baseController.BuscaGriffes().Where(p => p.GRIFFE.Trim() == desenvProduto.GRIFFE.Trim()).SingleOrDefault().GRIFFE;
                txtSegmento.Text = desenvProduto.SEGMENTO;
                txtNome.Text = desenvProduto.DESC_MODELO;
                txtTecido.Text = desenvProduto.TECIDO_POCKET;
                ddlFornecedor.SelectedValue = desenvProduto.FORNECEDOR;
                txtREFModelagem.Text = desenvProduto.REF_MODELAGEM;
                txtPrecoVendaVarejo.Text = desenvProduto.PRECO.ToString();
                txtPrecoVendaAtacado.Text = desenvProduto.PRECO_ATACADO.ToString();
                ddlBrinde.SelectedValue = desenvProduto.BRINDE.ToString();
                ddlProdutoAcabado.SelectedValue = desenvProduto.PRODUTO_ACABADO.ToString();
                ddlCor.SelectedValue = desenvProduto.COR;
                ddlLinha.SelectedValue = desenvProduto.LINHA;
                txtCorFornecedor.Text = desenvProduto.FORNECEDOR_COR;
                txtQtdeMostruario.Text = desenvProduto.QTDE_MOSTRUARIO.ToString();
                hidQtdeMostruario.Value = desenvProduto.QTDE_MOSTRUARIO.ToString();
                txtQtdeVarejo.Text = desenvProduto.QTDE.ToString();
                hidQtdeVarejo.Value = desenvProduto.QTDE.ToString();
                txtQtdeAtacado.Text = desenvProduto.QTDE_ATACADO.ToString();
                hidQtdeAtacado.Value = desenvProduto.QTDE_ATACADO.ToString();
                if (desenvProduto.SUBGRUPO_PRODUTO != null)
                    ddlSubGrupo.SelectedValue = desenvProduto.SUBGRUPO_PRODUTO;
                if (desenvProduto.MODDESIGN != null)
                    ddlModDesign.SelectedValue = desenvProduto.MODDESIGN.ToString();

                imgFoto.ImageUrl = (desenvProduto.FOTO == null) ? "" : desenvProduto.FOTO;
                imgFoto2.ImageUrl = (desenvProduto.FOTO2 == null) ? "" : desenvProduto.FOTO2;

                txtObservacao.Text = desenvProduto.OBSERVACAO;
                txtObsImpressao.Text = desenvProduto.OBS_IMPRESSAO;

                hidCodigo.Value = codigoProduto.ToString();

                ddlSigned.SelectedValue = desenvProduto.SIGNED.ToString();
                txtSignedNome.Text = desenvProduto.SIGNED_NOME;

                CarregarPocket(desenvProduto);
            }
        }
        private void CarregarProdutoPrePedido(DESENV_PREPEDIDO prePedido)
        {
            var grupoDePara = desenvController.ObterGrupoProdutoPrePedidoDePara(prePedido.GRUPO_PRODUTO);

            ddlOrigem.SelectedValue = prePedido.DESENV_PRODUTO_ORIGEM.ToString();
            ddlGrupo.SelectedValue = prodController.ObterGrupoProduto("01").Where(p => p.GRUPO_PRODUTO.Trim() == grupoDePara.GRUPO_PRODUTO).SingleOrDefault().GRUPO_PRODUTO;
            ddlGriffe.SelectedValue = baseController.BuscaGriffes().Where(p => p.GRIFFE.Trim() == prePedido.GRIFFE.Trim()).SingleOrDefault().GRIFFE;
            ddlCor.SelectedValue = prePedido.COR;
            txtCorFornecedor.Text = prePedido.COR_FORNECEDOR;
            txtQtdeVarejo.Text = prePedido.QTDE_VAREJO.ToString();
            txtQtdeAtacado.Text = prePedido.QTDE_ATACADO.ToString();

            ddlFornecedor.SelectedValue = prePedido.FORNECEDOR;
            txtPrecoVendaAtacado.Text = Math.Round((prePedido.PRECO_VENDA / 2), 0).ToString();
            txtPrecoVendaVarejo.Text = prePedido.PRECO_VENDA.ToString();

            txtREFModelagem.Text = prePedido.REF_MODELAGEM;


            //Controle de Tecido
            if (prePedido.MATERIAL == "")
            {
                txtTecido.Text = prePedido.TECIDO;
            }
            else
            {
                var materialLinx = desenvController.ObterMaterial(prePedido.MATERIAL);
                txtTecido.Text = materialLinx.SUBGRUPO.Trim();
            }

            //SIGNED
            if (prePedido.SIGNED_NOME != null && prePedido.SIGNED_NOME != "")
            {
                ddlSigned.SelectedValue = "S";
                txtSignedNome.Text = prePedido.SIGNED_NOME;
            }

        }

        private int ObterCodigoREF(string colecao)
        {
            var produto = desenvController.ObterProduto(colecao);
            if (produto != null)
            {
                int codigoMax = 0;
                if (produto.Count > 0)
                    codigoMax = produto.Max(i => i.CODIGO_REF);

                return (codigoMax + 1);
            }

            return 1;
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "";
                labErroAlteracao.Text = "";

                if (Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                if (!ValidarCampos())
                {
                    labErroAlteracao.Text = "Preencha os campos em vermelho corretamente.";
                    return;
                }

                var prePedido = desenvController.ObterPrePedido(hidColecao.Value, hidProduto.Value, ddlCor.SelectedValue);
                if (prePedido != null)
                {
                    var prePedidoInc = desenvController.ObterPrePedido(hidColecao.Value, txtModelo.Text, ddlCor.SelectedValue);
                    if (prePedidoInc != null && prePedidoInc.CODIGO != Convert.ToInt32(hidCodigoPrePedido.Value))
                    {
                        labErroAlteracao.Text = "Já existe um produto inserido com este Modelo e Cor.";
                        return;
                    }
                }
                else
                {
                    var prePedidoInc = desenvController.ObterPrePedido(hidColecao.Value, txtModelo.Text, ddlCor.SelectedValue);
                    if (prePedidoInc != null)
                    {
                        labErroAlteracao.Text = "Já existe um produto inserido com este Modelo e Cor.";
                        return;
                    }

                }

                var desenvProduto = SalvarDESENVPRODUTO();
                SalvarPREPEDIDO(desenvProduto);

                //Controle de Foto
                pnlFoto.Visible = true;
                hidFoto2.Value = (desenvProduto.FOTO2 == null) ? "" : desenvProduto.FOTO2;
                ddlQtdeFoto_SelectedIndexChanged(null, null);

                CarregarProduto(desenvProduto.CODIGO);
                hidCodigoProduto.Value = desenvProduto.CODIGO.ToString();

                labErroAlteracao.Text = "Produto criado com sucesso.\n" + msg;

            }
            catch (Exception ex)
            {
                labErroAlteracao.Text = ex.Message;
            }
        }

        private DESENV_PRODUTO SalvarDESENVPRODUTO()
        {
            int codigoProduto = 0;
            var desenvProduto = new DESENV_PRODUTO();
            if (hidCodigoProduto.Value != "")
            {
                codigoProduto = Convert.ToInt32(hidCodigoProduto.Value);
                desenvProduto = desenvController.ObterProduto(codigoProduto);
            }

            if (txtQtdeMostruario.Text.Trim() == "")
                txtQtdeMostruario.Text = "0";

            if (txtQtdeVarejo.Text.Trim() == "")
                txtQtdeVarejo.Text = "0";

            if (txtQtdeAtacado.Text.Trim() == "")
                txtQtdeAtacado.Text = "0";

            desenvProduto.MARCA = hidMarca.Value;
            desenvProduto.COLECAO = hidColecao.Value;
            desenvProduto.CODIGO_REF = (desenvProduto.CODIGO_REF > 0) ? desenvProduto.CODIGO_REF : ObterCodigoREF(desenvProduto.COLECAO);
            desenvProduto.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(ddlOrigem.SelectedValue.Trim());
            desenvProduto.GRUPO = ddlGrupo.SelectedValue.Trim();
            desenvProduto.MODELO = txtModelo.Text.Trim().ToUpper();
            desenvProduto.GRIFFE = ddlGriffe.SelectedValue.Trim();
            desenvProduto.SUBGRUPO_PRODUTO = ddlSubGrupo.SelectedValue.Trim();
            desenvProduto.MODDESIGN = Convert.ToInt32(ddlModDesign.SelectedValue);
            desenvProduto.DATA_MODDESIGN = DateTime.Now;

            desenvProduto.SEGMENTO = txtSegmento.Text.Trim().ToUpper();
            desenvProduto.TECIDO_POCKET = txtTecido.Text.Trim().ToUpper();
            desenvProduto.FORNECEDOR = ddlFornecedor.SelectedValue.ToUpper();

            desenvProduto.CORTE_PETIT = "";

            desenvProduto.DESC_MODELO = txtNome.Text.Trim().ToUpper();
            desenvProduto.REF_MODELAGEM = txtREFModelagem.Text.Trim().ToUpper();

            if (txtPrecoVendaVarejo.Text != "")
                desenvProduto.PRECO = Convert.ToDecimal(txtPrecoVendaVarejo.Text);
            else
                desenvProduto.PRECO = 0;
            if (txtPrecoVendaAtacado.Text != "")
                desenvProduto.PRECO_ATACADO = Convert.ToDecimal(txtPrecoVendaAtacado.Text);
            else
                desenvProduto.PRECO_ATACADO = 0;

            if (ddlBrinde.SelectedValue != "")
                desenvProduto.BRINDE = Convert.ToChar(ddlBrinde.SelectedValue);
            if (ddlProdutoAcabado.SelectedValue != "")
                desenvProduto.PRODUTO_ACABADO = Convert.ToChar(ddlProdutoAcabado.SelectedValue);

            desenvProduto.COR = ddlCor.SelectedValue.Trim().ToUpper();
            desenvProduto.FORNECEDOR_COR = txtCorFornecedor.Text.Trim().ToUpper();

            desenvProduto.LINHA = ddlLinha.SelectedValue;

            if (txtQtdeMostruario.Text != "")
                desenvProduto.QTDE_MOSTRUARIO = Convert.ToInt32(txtQtdeMostruario.Text);
            else
                desenvProduto.QTDE_MOSTRUARIO = 0;

            desenvProduto.CORTE_VAREJO = "";
            if (txtQtdeVarejo.Text != "")
            {
                desenvProduto.QTDE = Convert.ToInt32(txtQtdeVarejo.Text);
                if (txtQtdeVarejo.Text != "0")
                    desenvProduto.CORTE_VAREJO = "OK";
            }
            else
                desenvProduto.QTDE = 0;

            desenvProduto.CORTE_ATACADO = "";
            if (txtQtdeAtacado.Text != "")
            {
                desenvProduto.QTDE_ATACADO = Convert.ToInt32(txtQtdeAtacado.Text);
                if (txtQtdeAtacado.Text != "0")
                    desenvProduto.CORTE_ATACADO = "OK";
            }
            else
                desenvProduto.QTDE_ATACADO = 0;

            desenvProduto.OBSERVACAO = txtObservacao.Text.Trim().ToUpper();
            desenvProduto.OBS_IMPRESSAO = txtObsImpressao.Text;

            desenvProduto.PLAN_VAREJO = 'N';
            desenvProduto.PLAN_ATACADO = 'N';

            desenvProduto.DATA_INCLUSAO = DateTime.Now;
            desenvProduto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            desenvProduto.STATUS = 'A';

            if (ddlSigned.SelectedValue != "")
                desenvProduto.SIGNED = Convert.ToChar(ddlSigned.SelectedValue);

            desenvProduto.SIGNED_NOME = txtSignedNome.Text.Trim().ToUpper();

            desenvProduto.DATA_APROVACAO = DateTime.Now;

            if (codigoProduto > 0)
                desenvController.AtualizarProduto(desenvProduto);
            else
                desenvController.InserirProduto(desenvProduto);

            CarregarPocket(desenvProduto);

            return desenvProduto;
        }
        private void SalvarPREPEDIDO(DESENV_PRODUTO desenvProduto)
        {
            var prePedido = desenvController.ObterPrePedido(Convert.ToInt32(hidCodigoPrePedido.Value));
            prePedido.DESENV_PRODUTO_ORIGEM = desenvProduto.DESENV_PRODUTO_ORIGEM;
            prePedido.COR = desenvProduto.COR;
            prePedido.COR_FORNECEDOR = desenvProduto.FORNECEDOR_COR;
            prePedido.GRIFFE = desenvProduto.GRIFFE;
            prePedido.PRODUTO = desenvProduto.MODELO;
            prePedido.QTDE_ATACADO = Convert.ToInt32(desenvProduto.QTDE_ATACADO);
            prePedido.QTDE_VAREJO = Convert.ToInt32(desenvProduto.QTDE);
            prePedido.TECIDO = desenvProduto.TECIDO_POCKET;
            prePedido.REF_MODELAGEM = desenvProduto.REF_MODELAGEM;
            prePedido.FORNECEDOR = desenvProduto.FORNECEDOR;

            prePedido.CONSUMO_TOTAL = ((prePedido.QTDE_ATACADO + prePedido.QTDE_VAREJO) * prePedido.CONSUMO);
            prePedido.VALOR_TOTAL_ATACADO = ((prePedido.PRECO_VENDA / 2) * prePedido.QTDE_ATACADO);
            prePedido.VALOR_TOTAL_VAREJO = ((prePedido.PRECO_VENDA) * prePedido.QTDE_VAREJO);

            prePedido.DATA_CRIACAO = DateTime.Now;
            prePedido.USUARIO_CRIACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            prePedido.SIGNED_NOME = txtSignedNome.Text.Trim().ToUpper();

            desenvController.AtualizarPrePedido(prePedido);

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

                if (hidCodigoProduto.Value != "")
                {
                    var _produto = desenvController.ObterProduto(Convert.ToInt32(hidCodigoProduto.Value));
                    if (_produto != null)
                        CarregarPocket(_produto);
                }
            }
            catch (Exception ex)
            {
                labErroAlteracao.Text = ex.Message;
            }

        }

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

                        var produtoHB = prodController.ObterHBPorDesenvProduto(_produto.MODELO, _produto.COR);
                        foreach (var phb in produtoHB)
                        {
                            var hb = prodController.ObterHB(phb.CODIGO);
                            hb.FOTO_PECA = imgFoto.ImageUrl.Replace("Image_POCKET", "Image_HB");
                            hb.FOTO_TECIDO = imgFoto.ImageUrl.Replace("Image_POCKET", "Image_HB");
                            prodController.AtualizarHB(hb);
                        }

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
            string _path2 = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(_pathFile);
                _fileName = Guid.NewGuid() + "_FOTO" + _ext;
                _path = Server.MapPath("~/Image_POCKET/") + _fileName;
                _path2 = Server.MapPath("~/Image_HB/") + _fileName;

                //Obter stream da imagem
                if (_stream != null)
                    GenerateThumbnails(1, _stream, _path, _path2);

                retornoImagem = "~/Image_POCKET/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath, string targetPath2)
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
                thumbnailImg.Save(targetPath2, image.RawFormat);
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

        protected void btProdutoSeq_Click(object sender, EventArgs e)
        {
            var bt = (Button)sender;

            labErroAlteracao.Text = "";

            bool cmax = true;

            if (bt.ID.ToLower().Contains("handbook"))
                cmax = false;

            var produtoSeq = desenvController.ObterProdutoSequencia(cmax);
            if (produtoSeq != null)
            {
                txtModelo.Text = produtoSeq.PRODUTO.ToString();

                produtoSeq.DATA_UTILIZACAO = DateTime.Now;
                desenvController.AtualizarProdutoSequencia(produtoSeq);
            }
            else
            {
                txtModelo.Text = "";
                labErroAlteracao.Text = "Não existe Sequência cadastrada. Entre em contato com TI.";
            }

        }

        protected void ddlModDesign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProdutoAcabado.SelectedValue = "N";
            if (ddlModDesign.SelectedValue == "4")
                ddlProdutoAcabado.SelectedValue = "S";
        }

        protected void ddlProdutoAcabado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProdutoAcabado.SelectedValue == "S")
                ddlModDesign.SelectedValue = "4";
        }
    }
}
