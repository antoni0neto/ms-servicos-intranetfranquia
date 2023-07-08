using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;


namespace Relatorios
{
    public partial class ecom_rel_config_rel_criar_random : System.Web.UI.Page
    {
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                var codigoEcom = Request.QueryString["ec"].ToString();

                var ecomProduto = eController.ObterMagentoProduto(Convert.ToInt32(codigoEcom));

                CarregarDadosProduto(ecomProduto);

                hidProduto.Value = ecomProduto.PRODUTO;
                hidCor.Value = ecomProduto.COR;

                hidCodigoEcom.Value = ecomProduto.CODIGO.ToString();
                hidIdMagento.Value = ecomProduto.ID_PRODUTO_MAG.ToString();

                //verifica se existe relacionados OK
                var produtoRelOK = eController.ObterEcomRelacionadoPorCodigoPai(ecomProduto.CODIGO);
                if (produtoRelOK != null && produtoRelOK.Count() > 0)
                {
                    PreencherFotosRelOK(produtoRelOK);
                }
                else
                {
                    //senao existe, cria um radndom
                    var erro = AtualizarProdutosRandom(hidProduto.Value, hidCor.Value);
                    labErro.Text = erro;
                }

                btApagarHistExc_Click(btApagarHistExc, null);
                btRandom_Click(btRandom, null);
            }

            //Evitar duplo clique no botão
            btRandom.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btRandom, null) + ";");
            btApagarHistExc.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btApagarHistExc, null) + ";");
            btAtualizarMagento.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizarMagento, null) + ";");
        }

        private void PreencherFotosRelOK(List<ECOM_PRODUTO_RELACIONADO> produtoRelOK)
        {
            // Se tudo certo
            var i = 1;
            foreach (var pr in produtoRelOK)
            {
                if (i == 1 && (pr.PRIORIDADE == 1 || pr.PRIORIDADE == 2))
                    imgUpSell1.ImageUrl = eController.ObterMagentoProduto(pr.ECOM_PRODUTO_REL).FOTO_FRENTE_CAB;
                else if (i == 2 && pr.PRIORIDADE == 2)
                    imgUpSell2.ImageUrl = eController.ObterMagentoProduto(pr.ECOM_PRODUTO_REL).FOTO_FRENTE_CAB;
                else if (i == 3)
                    imgRelated1.ImageUrl = eController.ObterMagentoProduto(pr.ECOM_PRODUTO_REL).FOTO_FRENTE_CAB;
                else if (i == 4)
                    imgRelated2.ImageUrl = eController.ObterMagentoProduto(pr.ECOM_PRODUTO_REL).FOTO_FRENTE_CAB;
                else if (i == 5)
                    imgRelated3.ImageUrl = eController.ObterMagentoProduto(pr.ECOM_PRODUTO_REL).FOTO_FRENTE_CAB;
                else if (i == 6)
                    imgRelated4.ImageUrl = eController.ObterMagentoProduto(pr.ECOM_PRODUTO_REL).FOTO_FRENTE_CAB;

                i = i + 1;
            }
        }

        private void CarregarDadosProduto(ECOM_PRODUTO ecomProduto)
        {
            txtProduto.Text = ecomProduto.PRODUTO;
            txtNome.Text = ecomProduto.NOME;
            txtCor.Text = ecomProduto.ECOM_COR1.COR.ToUpper();
            txtGriffe.Text = ecomProduto.GRIFFE;
            txtGrupoProduto.Text = ecomProduto.GRUPO_PRODUTO;
            txtSKU.Text = ecomProduto.SKU;

            imgFotoLook.ImageUrl = ecomProduto.FOTO_LOOK;
            imgFotoFrenteCabeca.ImageUrl = ecomProduto.FOTO_FRENTE_CAB;
            imgFotoFrenteSemCabeca.ImageUrl = ecomProduto.FOTO_FRENTE_SEMCAB;
            imgFotoCostas.ImageUrl = ecomProduto.FOTO_COSTAS;
            imgFotoLado.ImageUrl = ecomProduto.FOTO_LADO;
            imgFotoDetalhe.ImageUrl = ecomProduto.FOTO_DETALHE;

        }

        protected void btRandom_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroMagento.Text = "";

                var erro = AtualizarProdutosRandom(hidProduto.Value, hidCor.Value);

                labErro.Text = erro;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private string AtualizarProdutosRandom(string produto, string cor)
        {
            var configProduto = eController.ObterConfigDoProduto(produto, cor);
            if (configProduto == null || configProduto.CODIGO_CONFIG_OK <= 0)
                return "Não foi encontrada nenhuma REGRA DE ENTRADA para este produto. Crie pelo menos uma regra.";

            var produtoRel = eController.ObterProdutoRelacionadoPorCodigoConfig(produto, cor, configProduto.CODIGO_CONFIG_OK);
            if (produtoRel == null || produtoRel.Count() <= 0)
                return "Não foi encontrada nenhuma REGRA DE SAÍDA para este produto. Verifique as regras relacionadas.";

            if (produtoRel.Count() < 6)
                return "Não existem produtos suficientes para a REGRA DE ENTRADA deste produto. Relacione mais regras ou altere a quantidade de produtos.";


            hidUpSell1.Value = "";
            hidUpSell2.Value = "";
            hidRelated1.Value = "";
            hidRelated2.Value = "";
            hidRelated3.Value = "";
            hidRelated4.Value = "";

            foreach (var pr in produtoRel)
            {
                if (hidUpSell1.Value == "" && (pr.PRIORIDADE == 1 || pr.PRIORIDADE == 2) && pr.TIPO == "upsell" && (pr.POSICAO == 1 || pr.POSICAO == 0)) //up_sell
                {
                    imgUpSell1.ImageUrl = pr.FOTO_FRENTE_CAB;
                    labTotRelUp1.Text = pr.TOT_REL.ToString();
                    hidUpSell1.Value = pr.CODIGO.ToString();
                    txtUpsell1.Text = pr.SKU_MANUAL;

                    //cbUpSell1.Enabled = true;
                    //txtUpsell1.Enabled = true;
                    //if (pr.PRIORIDADE == 1)
                    //{
                    //    cbUpSell1.Checked = true;
                    //    cbUpSell1.Enabled = false;
                    //    txtUpsell1.Enabled = false;
                    //}
                    //else
                    //{
                    cbUpSell1.Checked = (pr.FIXO == "N") ? false : true;
                    if (cbUpSell1.Checked && txtUpsell1.Text == "")
                        txtUpsell1.Enabled = false;
                    if (cbUpSell1.Checked && txtUpsell1.Text != "")
                        cbUpSell1.Enabled = false;

                }
                else if (hidUpSell2.Value == "" && pr.PRIORIDADE == 2 && pr.TIPO == "upsell" && (pr.POSICAO == 2 || pr.POSICAO == 0)) //up_sell
                {
                    imgUpSell2.ImageUrl = pr.FOTO_FRENTE_CAB;
                    labTotRelUp2.Text = pr.TOT_REL.ToString();
                    hidUpSell2.Value = pr.CODIGO.ToString();
                    txtUpsell2.Text = pr.SKU_MANUAL;
                    cbUpSell2.Checked = (pr.FIXO == "N") ? false : true;
                    if (cbUpSell2.Checked && txtUpsell2.Text == "")
                        txtUpsell2.Enabled = false;

                    if (cbUpSell2.Checked && txtUpsell2.Text != "")
                        cbUpSell2.Enabled = false;
                }
                else if (hidRelated1.Value == "" && pr.PRIORIDADE == 99 && pr.TIPO == "related" && (pr.POSICAO == 3 || pr.POSICAO == 0))
                {
                    imgRelated1.ImageUrl = pr.FOTO_FRENTE_CAB;
                    labTotRel1.Text = pr.TOT_REL.ToString();
                    hidRelated1.Value = pr.CODIGO.ToString();
                    txtRelated1.Text = pr.SKU_MANUAL;
                    cbRelated1.Checked = (pr.FIXO == "N") ? false : true;
                    if (cbRelated1.Checked && txtRelated1.Text == "")
                        txtRelated1.Enabled = false;

                    if (cbRelated1.Checked && txtRelated1.Text != "")
                        cbRelated1.Enabled = false;
                }
                else if (hidRelated2.Value == "" && pr.PRIORIDADE == 99 && pr.TIPO == "related" && (pr.POSICAO == 4 || pr.POSICAO == 0))
                {
                    imgRelated2.ImageUrl = pr.FOTO_FRENTE_CAB;
                    labTotRel2.Text = pr.TOT_REL.ToString();
                    hidRelated2.Value = pr.CODIGO.ToString();
                    txtRelated2.Text = pr.SKU_MANUAL;
                    cbRelated2.Checked = (pr.FIXO == "N") ? false : true;
                    if (cbRelated2.Checked && txtRelated2.Text == "")
                        txtRelated2.Enabled = false;

                    if (cbRelated2.Checked && txtRelated2.Text != "")
                        cbRelated2.Enabled = false;
                }
                else if (hidRelated3.Value == "" && pr.PRIORIDADE == 99 && pr.TIPO == "related" && (pr.POSICAO == 5 || pr.POSICAO == 0))
                {
                    imgRelated3.ImageUrl = pr.FOTO_FRENTE_CAB;
                    labTotRel3.Text = pr.TOT_REL.ToString();
                    hidRelated3.Value = pr.CODIGO.ToString();
                    txtRelated3.Text = pr.SKU_MANUAL;
                    cbRelated3.Checked = (pr.FIXO == "N") ? false : true;
                    if (cbRelated3.Checked && txtRelated3.Text == "")
                        txtRelated3.Enabled = false;

                    if (cbRelated3.Checked && txtRelated3.Text != "")
                        cbRelated3.Enabled = false;

                }
                else if (hidRelated4.Value == "" && pr.PRIORIDADE == 99 && pr.TIPO == "related" && (pr.POSICAO == 6 || pr.POSICAO == 0))
                {
                    imgRelated4.ImageUrl = pr.FOTO_FRENTE_CAB;
                    labTotRel4.Text = pr.TOT_REL.ToString();
                    hidRelated4.Value = pr.CODIGO.ToString();
                    txtRelated4.Text = pr.SKU_MANUAL;
                    cbRelated4.Checked = (pr.FIXO == "N") ? false : true;
                    if (cbRelated4.Checked && txtRelated4.Text == "")
                        txtRelated4.Enabled = false;

                    if (cbRelated4.Checked && txtRelated4.Text != "")
                        cbRelated4.Enabled = false;
                }
            }

            return "";
        }

        protected void btAtualizarMagento_Click(object sender, EventArgs e)
        {
            EcomMySqlController mysqlController = null;

            try
            {
                labErro.Text = "";
                labErroMagento.Text = "";

                mysqlController = new EcomMySqlController();

                if (hidIdMagento.Value == "" || hidIdMagento.Value == "0")
                {
                    labErroMagento.Text = "Não foi possível encontrar o Id do Magento. Entre em contato com TI.";
                    return;
                }

                //obter relacionados
                var produtos = eController.ObterEcomRelacionadoTempPorCodigoPai(Convert.ToInt32(hidCodigoEcom.Value));
                if (produtos == null || produtos.Count() < 6)
                {
                    labErroMagento.Text = "Não existem produtos suficientes para relacionar no Magento. Verifique as regras configuradas.";
                    return;
                }

                // excluir toda relação de produtos do magento
                mysqlController.ExcluirProdutoRelacionado(hidIdMagento.Value);
                // excluir toda relação de produtos da intranet
                eController.ExcluirEcomRelacionadoPorCodigoPai(Convert.ToInt32(hidCodigoEcom.Value));

                // inserir relacionados
                ECOM_PRODUTO_RELACIONADO prodRel = null;
                foreach (var p in produtos)
                {
                    var type = (p.TIPO.ToLower() == "upsell") ? "4" : "1";

                    //inserir relacionado no magento
                    mysqlController.InserirProdutoRelacionado(p.ECOM_PRODUTO1.ID_PRODUTO_MAG.ToString(), p.ECOM_PRODUTO2.ID_PRODUTO_MAG.ToString(), type, p.PRIORIDADE.ToString());

                    //inserir relacionado na intranet
                    prodRel = new ECOM_PRODUTO_RELACIONADO();
                    prodRel.ECOM_PRODUTO = p.ECOM_PRODUTO;
                    prodRel.ECOM_PRODUTO_REL = p.ECOM_PRODUTO_REL;
                    prodRel.PRIORIDADE = p.PRIORIDADE;
                    prodRel.TIPO = p.TIPO;
                    eController.InserirEcomRelacionado(prodRel);
                }

                labErroMagento.Text = "Produtos relacionados no magento com sucesso.";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
            finally
            {
                if (mysqlController != null)
                    mysqlController.FecharConexao();
            }
        }

        protected void btCriarRegra_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                Response.Redirect("ecom_rel_config_cad.aspx?gp=" + txtGrupoProduto.Text + "&gg=" + txtGriffe.Text, "_blank", "");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btRelacionarRegra_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                Response.Redirect("ecom_rel_config_entrada_saida.aspx", "_blank", "");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void AtualizarRelFIXO(string codigoTemp, bool fixo, int posicao, string skuManual, int codigoEcomRel = 0)
        {
            var rel = eController.ObterEcomRelacionadoTempPorCodigo(Convert.ToInt32(codigoTemp));
            rel.FIXO = (fixo) ? 'S' : 'N';
            rel.POSICAO = posicao;
            rel.SKU_MANUAL = skuManual;
            if (codigoEcomRel > 0)
                rel.ECOM_PRODUTO_REL = codigoEcomRel;
            eController.AtualizarEcomRelacionadoTemp(rel);
        }

        protected void cbUpSell1_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarRelFIXO(hidUpSell1.Value, cbUpSell1.Checked, 1, "");
            txtUpsell1.Enabled = !cbUpSell1.Checked;
        }
        protected void cbUpSell2_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarRelFIXO(hidUpSell2.Value, cbUpSell2.Checked, 2, "");
            txtUpsell2.Enabled = !cbUpSell2.Checked;
        }
        protected void cbRelated1_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarRelFIXO(hidRelated1.Value, cbRelated1.Checked, 3, "");
            txtRelated1.Enabled = !cbRelated1.Checked;
        }
        protected void cbRelated2_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarRelFIXO(hidRelated2.Value, cbRelated2.Checked, 4, "");
            txtRelated2.Enabled = !cbRelated2.Checked;
        }
        protected void cbRelated3_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarRelFIXO(hidRelated3.Value, cbRelated3.Checked, 5, "");
            txtRelated3.Enabled = !cbRelated3.Checked;
        }
        protected void cbRelated4_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarRelFIXO(hidRelated4.Value, cbRelated4.Checked, 6, "");
            txtRelated4.Enabled = !cbRelated4.Checked;
        }

        protected void txtUpsell1_TextChanged(object sender, EventArgs e)
        {
            var p = eController.ObterMagentoProdutoConfigPorSKU(txtUpsell1.Text.Trim());
            if (p != null)
            {
                var codigoTemp = "0";

                var temp = eController.ObterEcomRelacionadoTempParaSKU(Convert.ToInt32(hidCodigoEcom.Value), "upsell", 2, 1);
                if (temp != null)
                {
                    codigoTemp = temp.CODIGO.ToString();
                    AtualizarRelFIXO(codigoTemp, true, 1, p.SKU, p.CODIGO);
                    hidUpSell1.Value = codigoTemp;
                    cbUpSell1.Enabled = false;
                    cbUpSell1.Checked = true;
                    imgUpSell1.ImageUrl = p.FOTO_FRENTE_CAB;
                }
            }
            else
            {
                if (hidUpSell1.Value != "")
                    AtualizarRelFIXO(hidUpSell1.Value, false, 0, "");

                txtUpsell1.Text = "";
                txtUpsell1.Enabled = true;
                cbUpSell1.Enabled = true;
                cbUpSell1.Checked = false;
            }
        }
        protected void txtUpsell2_TextChanged(object sender, EventArgs e)
        {
            var p = eController.ObterMagentoProdutoConfigPorSKU(txtUpsell2.Text.Trim());
            if (p != null)
            {
                var codigoTemp = "0";

                var temp = eController.ObterEcomRelacionadoTempParaSKU(Convert.ToInt32(hidCodigoEcom.Value), "upsell", 2, 2);
                if (temp != null)
                {
                    codigoTemp = temp.CODIGO.ToString();
                    AtualizarRelFIXO(codigoTemp, true, 2, p.SKU, p.CODIGO);
                    hidUpSell2.Value = codigoTemp;
                    cbUpSell2.Enabled = false;
                    cbUpSell2.Checked = true;
                    imgUpSell2.ImageUrl = p.FOTO_FRENTE_CAB;
                }
            }
            else
            {
                if (hidUpSell2.Value != "")
                    AtualizarRelFIXO(hidUpSell2.Value, false, 0, "");

                txtUpsell2.Text = "";
                txtUpsell2.Enabled = true;
                cbUpSell2.Enabled = true;
                cbUpSell2.Checked = false;
            }
        }

        protected void txtRelated1_TextChanged(object sender, EventArgs e)
        {
            var p = eController.ObterMagentoProdutoConfigPorSKU(txtRelated1.Text.Trim());
            if (p != null)
            {
                var codigoTemp = "0";

                var temp = eController.ObterEcomRelacionadoTempParaSKU(Convert.ToInt32(hidCodigoEcom.Value), "related", 99, 3);
                if (temp != null)
                {
                    codigoTemp = temp.CODIGO.ToString();
                    AtualizarRelFIXO(codigoTemp, true, 3, p.SKU, p.CODIGO);
                    hidRelated1.Value = codigoTemp;
                    cbRelated1.Enabled = false;
                    cbRelated1.Checked = true;
                    imgRelated1.ImageUrl = p.FOTO_FRENTE_CAB;
                }
            }
            else
            {
                if (hidRelated1.Value != "")
                    AtualizarRelFIXO(hidRelated1.Value, false, 0, "");

                txtRelated1.Text = "";
                txtRelated1.Enabled = true;
                cbRelated1.Enabled = true;
                cbRelated1.Checked = false;
            }
        }
        protected void txtRelated2_TextChanged(object sender, EventArgs e)
        {
            var p = eController.ObterMagentoProdutoConfigPorSKU(txtRelated2.Text.Trim());
            if (p != null)
            {
                var codigoTemp = "0";

                var temp = eController.ObterEcomRelacionadoTempParaSKU(Convert.ToInt32(hidCodigoEcom.Value), "related", 99, 4);
                if (temp != null)
                {
                    codigoTemp = temp.CODIGO.ToString();
                    AtualizarRelFIXO(codigoTemp, true, 4, p.SKU, p.CODIGO);
                    hidRelated2.Value = codigoTemp;
                    cbRelated2.Enabled = false;
                    cbRelated2.Checked = true;
                    imgRelated2.ImageUrl = p.FOTO_FRENTE_CAB;
                }
            }
            else
            {
                if (hidRelated2.Value != "")
                    AtualizarRelFIXO(hidRelated2.Value, false, 0, "");

                txtRelated2.Text = "";
                txtRelated2.Enabled = true;
                cbRelated2.Enabled = true;
                cbRelated2.Checked = false;
            }
        }
        protected void txtRelated3_TextChanged(object sender, EventArgs e)
        {
            var p = eController.ObterMagentoProdutoConfigPorSKU(txtRelated3.Text.Trim());
            if (p != null)
            {
                var codigoTemp = "0";

                var temp = eController.ObterEcomRelacionadoTempParaSKU(Convert.ToInt32(hidCodigoEcom.Value), "related", 99, 5);
                if (temp != null)
                {
                    codigoTemp = temp.CODIGO.ToString();
                    AtualizarRelFIXO(codigoTemp, true, 5, p.SKU, p.CODIGO);
                    hidRelated3.Value = codigoTemp;
                    cbRelated3.Enabled = false;
                    cbRelated3.Checked = true;
                    imgRelated3.ImageUrl = p.FOTO_FRENTE_CAB;
                }
            }
            else
            {
                if (hidRelated3.Value != "")
                    AtualizarRelFIXO(hidRelated3.Value, false, 0, "");

                txtRelated3.Text = "";
                txtRelated3.Enabled = true;
                cbRelated3.Enabled = true;
                cbRelated3.Checked = false;
            }
        }
        protected void txtRelated4_TextChanged(object sender, EventArgs e)
        {
            var p = eController.ObterMagentoProdutoConfigPorSKU(txtRelated4.Text.Trim());
            if (p != null)
            {
                var codigoTemp = "0";

                var temp = eController.ObterEcomRelacionadoTempParaSKU(Convert.ToInt32(hidCodigoEcom.Value), "related", 99, 6);
                if (temp != null)
                {
                    codigoTemp = temp.CODIGO.ToString();
                    AtualizarRelFIXO(codigoTemp, true, 6, p.SKU, p.CODIGO);
                    hidRelated4.Value = codigoTemp;
                    cbRelated4.Enabled = false;
                    cbRelated4.Checked = true;
                    imgRelated4.ImageUrl = p.FOTO_FRENTE_CAB;
                }
            }
            else
            {
                if (hidRelated4.Value != "")
                    AtualizarRelFIXO(hidRelated4.Value, false, 0, "");

                txtRelated4.Text = "";
                txtRelated4.Enabled = true;
                cbRelated4.Enabled = true;
                cbRelated4.Checked = false;
            }
        }

        protected void btApagarHistExc_Click(object sender, EventArgs e)
        {
            try
            {
                var codigoEcom = Convert.ToInt32(hidCodigoEcom.Value);
                eController.ExcluirEcomRelacionadoTempExc(codigoEcom);
            }
            catch (Exception)
            {
            }
        }

        protected void btAbrirProdutosComprados_Click(object sender, EventArgs e)
        {
            try
            {
                var sku = txtSKU.Text.Trim().Split('-');

                var produto = sku[0].ToString();
                var cor = sku[1].ToString();

                //Abrir pop-up
                var _url = "fnAbrirTelaCadastro('ecom_rel_config_rel_criar_random_compra.aspx?prod=" + produto + "&cor=" + cor + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);

            }
            catch (Exception)
            {

            }
        }
    }
}

