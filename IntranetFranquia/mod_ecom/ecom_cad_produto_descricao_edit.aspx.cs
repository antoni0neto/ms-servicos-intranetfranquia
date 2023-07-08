using DAL;
using Relatorios.mod_ecom.mag;
using Relatorios.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_descricao_edit : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                string produto = "";

                produto = Request.QueryString["p"].ToString();

                var plinxdesc = eController.ObterProdutoLinxParaDescricao("", "", produto, "").FirstOrDefault();
                if (plinxdesc != null)
                {
                    txtProduto.Text = plinxdesc.PRODUTO;
                    txtGrupoProduto.Text = plinxdesc.GRUPO_PRODUTO;
                    txtNome.Text = plinxdesc.DESC_PRODUTO;
                    imgFoto.ImageUrl = plinxdesc.FOTO;

                    var pdesc = eController.ObterProdutoDescricao(produto);
                    if (pdesc != null)
                    {
                        txtDescTecido.Text = pdesc.PRODUTO_DESC;
                        txtDescPeca.Text = pdesc.PECA_DESC;
                        txtOcasiaoUso.Text = pdesc.USO_DESC;
                    }

                }

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labDescTecido.ForeColor = _OK;
            if (txtDescTecido.Text.Trim() == "")
            {
                labDescTecido.ForeColor = _notOK;
                retorno = false;
            }

            labDescPeca.ForeColor = _OK;
            if (txtDescPeca.Text.Trim() == "")
            {
                labDescPeca.ForeColor = _notOK;
                retorno = false;
            }

            labOcasiaoUso.ForeColor = _OK;
            if (txtOcasiaoUso.Text.Trim() == "")
            {
                labOcasiaoUso.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Informe a Descrição do Produto...";
                    return;
                }

                var pdesc = eController.ObterProdutoDescricao(txtProduto.Text.Trim());
                if (pdesc == null)
                {
                    pdesc = new ECOM_PRODUTO_DESCRICAO();
                    pdesc.PRODUTO = txtProduto.Text.Trim();
                    pdesc.PRODUTO_DESC = txtDescTecido.Text;
                    pdesc.PECA_DESC = txtDescPeca.Text;
                    pdesc.USO_DESC = txtOcasiaoUso.Text;

                    eController.InserirProdutoDescricao(pdesc);
                }
                else
                {
                    pdesc.PRODUTO_DESC = txtDescTecido.Text;
                    pdesc.PECA_DESC = txtDescPeca.Text;
                    pdesc.USO_DESC = txtOcasiaoUso.Text;
                    eController.AtualizarProdutoDescricao(pdesc);
                }


                // cadastrar produtos com mesmo tecido
                // se cor listrado, ou estampado, por tecido e cor fornecedor
                var produto = desenvController.ObterProdutoPorProduto(txtProduto.Text.Trim()).FirstOrDefault();
                if (produto != null)
                {
                    var tecido = produto.TECIDO_POCKET;
                    var corFornecedor = produto.FORNECEDOR_COR;

                    if (tecido != null && tecido.Trim() != "")
                    {
                        if (produto.COR.Trim() == "036" || produto.COR.Trim() == "265")
                        {
                            var produtos = desenvController.ObterProdutoPorTecidoECorFornecedor(tecido, corFornecedor);
                            foreach (var p in produtos)
                            {
                                var pdescaux = eController.ObterProdutoDescricao(p.MODELO.Trim());
                                if (pdescaux == null)
                                {
                                    pdescaux = new ECOM_PRODUTO_DESCRICAO();
                                    pdescaux.PRODUTO = p.MODELO.Trim();
                                    pdescaux.PRODUTO_DESC = txtDescTecido.Text;
                                    //pdescaux.PECA_DESC = txtDescPeca.Text;
                                    //pdescaux.USO_DESC = txtOcasiaoUso.Text;
                                    eController.InserirProdutoDescricao(pdescaux);
                                }
                                else
                                {
                                    pdescaux.PRODUTO_DESC = txtDescTecido.Text;
                                    //pdescaux.PECA_DESC = txtDescPeca.Text;
                                    //pdescaux.USO_DESC = txtOcasiaoUso.Text;
                                    eController.AtualizarProdutoDescricao(pdescaux);
                                }
                            }
                        }
                        else
                        {
                            var produtos = desenvController.ObterProdutoPorTecido(tecido);
                            foreach (var p in produtos)
                            {
                                var pdescaux = eController.ObterProdutoDescricao(p.MODELO.Trim());
                                if (pdescaux == null)
                                {
                                    pdescaux = new ECOM_PRODUTO_DESCRICAO();
                                    pdescaux.PRODUTO = p.MODELO.Trim();
                                    pdescaux.PRODUTO_DESC = txtDescTecido.Text;
                                    //pdescaux.PECA_DESC = txtDescPeca.Text;
                                    //pdescaux.USO_DESC = txtOcasiaoUso.Text;
                                    eController.InserirProdutoDescricao(pdescaux);
                                }
                                else
                                {
                                    pdescaux.PRODUTO_DESC = txtDescTecido.Text;
                                    //pdescaux.PECA_DESC = txtDescPeca.Text;
                                    //pdescaux.USO_DESC = txtOcasiaoUso.Text;
                                    eController.AtualizarProdutoDescricao(pdescaux);
                                }
                            }
                        }
                    }
                }

                btSalvar.Enabled = false;

                labErro.Text = "Descrição de Produto cadastrado com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }




    }
}


