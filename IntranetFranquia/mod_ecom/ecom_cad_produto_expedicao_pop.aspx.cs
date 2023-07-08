using DAL;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_expedicao_pop : System.Web.UI.Page
    {
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        int qtdeProduto = 0;
        int qtdeProdutoCaixa = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            txtProdutoBarra.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "', event)");
            txtProdutoBarra.Focus();

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["pedido"] == null || Request.QueryString["pedido"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                string pedido = "";

                pedido = Request.QueryString["pedido"].ToString();

                txtPedido.Text = pedido;
                hidPedido.Value = pedido;

                var exp = eController.ObterPedidoExpedicao(pedido, 'A').FirstOrDefault();
                if (exp != null)
                {
                    txtTipo.Text = exp.TIPO;
                }

                var produtos = eController.ObterProdutoPedidoExpedicao(pedido, "", "", "");
                if (produtos != null)
                {
                    gvProdutoPedido.DataSource = produtos;
                    gvProdutoPedido.DataBind();
                }

                CarregarProdutoCaixa();
                CarregarCaixaDimensao();
                CarregarVolumes();

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarCaixaDimensao()
        {
            var caixaDim = eController.ObterMagentoCaixaDimensao();

            caixaDim.Insert(0, new ECOM_CAIXA_DIMENSAO { CODIGO = 0, DESCRICAO = "Selecione" });
            ddlCaixa.DataSource = caixaDim;
            ddlCaixa.DataBind();
        }
        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }
        #endregion

        protected void gvProdutoPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_EXPEDICAO_PRODUTOResult produtoPedido = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_EXPEDICAO_PRODUTOResult;

                    if (produtoPedido != null)
                    {
                        qtdeProduto += Convert.ToInt32(produtoPedido.QTDE);
                    }
                }
            }
        }
        protected void gvProdutoPedido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProdutoPedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = qtdeProduto.ToString();
                footer.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                hidQtdeProduto.Value = qtdeProduto.ToString();
            }
        }

        private void CarregarProdutoCaixa()
        {
            var produtos = eController.ObterProdutoPedidoExpedicao(txtPedido.Text, "", "", "");

            gvProdutoCaixa.DataSource = produtos;
            gvProdutoCaixa.DataBind();
        }
        protected void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroBarra.Text = "";

                string pedido = txtPedido.Text.Trim();
                string codigoBarraProduto = txtProdutoBarra.Text.Trim();

                var produtoBarra = baseController.BuscaProdutoBarra(codigoBarraProduto);
                if (produtoBarra != null)
                {
                    string produto = produtoBarra.PRODUTO.Trim();
                    string cor = produtoBarra.COR_PRODUTO.Trim();
                    string tamanho = produtoBarra.GRADE.Trim();

                    var exp = eController.ObterProdutoPedidoExpedicao(pedido, produto, cor, tamanho);
                    if (exp != null && exp.Count() > 0)
                    {
                        var expQtde = exp.Where(p => p.QTDE_FALTANTE > 0);
                        if (expQtde != null && expQtde.Count() > 0)
                        {

                            var expProduto = new ECOM_EXPEDICAO_PRODUTO();
                            expProduto.PEDIDO = pedido;
                            expProduto.PRODUTO = produto;
                            expProduto.COR_PRODUTO = cor;
                            expProduto.TAMANHO = tamanho;
                            expProduto.QTDE = 1;

                            eController.InserirMagentoExpedicaoProduto(expProduto);
                        }
                        else
                        {
                            labErroBarra.Text = "Todas as quantidades deste Produto já foram adicionadas na Caixa...";
                        }
                    }
                    else
                    {
                        labErroBarra.Text = "Este produto não foi adicionado ao Pedido...";
                    }

                }
                else
                {
                    labErroBarra.Text = "Produto não foi encontrado...";
                }

                CarregarProdutoCaixa();

                txtProdutoBarra.Text = "";
            }
            catch (Exception ex)
            {
                labErroBarra.Text = ex.Message;
            }
        }

        protected void gvProdutoCaixa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_EXPEDICAO_PRODUTOResult produtoPedido = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_EXPEDICAO_PRODUTOResult;

                    if (produtoPedido != null)
                    {
                        qtdeProdutoCaixa += Convert.ToInt32(produtoPedido.QTDE_CAIXA);
                    }
                }
            }
        }
        protected void gvProdutoCaixa_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProdutoCaixa.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = qtdeProdutoCaixa.ToString();
                footer.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                hidQtdeProdutoCaixa.Value = qtdeProdutoCaixa.ToString();
            }
        }

        private void CarregarVolumes()
        {
            var volumesPedido = eController.ObterMagentoExpedicaoVolumePorPedido(txtPedido.Text);

            gvVolume.DataSource = volumesPedido;
            gvVolume.DataBind();
        }
        protected void btIncluirVolume_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlCaixa.SelectedValue == "0")
                {
                    labErro.Text = "Selecione uma caixa...";
                    return;
                }
                if (txtPeso.Text.Trim() == "" || Convert.ToDecimal(txtPeso.Text.Trim()) <= 0)
                {
                    labErro.Text = "Informe o peso da Caixa maior que zero...";
                    return;
                }

                if (Convert.ToDecimal(txtPeso.Text.Trim()) > 10)
                {
                    labErro.Text = "Peso máximo permitido: 10KGs...";
                    return;
                }

                var volCaixa = new ECOM_EXPEDICAO_VOLUME();
                volCaixa.PEDIDO = hidPedido.Value;
                volCaixa.PESO_KG = Convert.ToDecimal(txtPeso.Text.Trim());
                volCaixa.ECOM_CAIXA_DIMENSAO = Convert.ToInt32(ddlCaixa.SelectedValue);

                var caixaDimensao = eController.ObterMagentoCaixaDimensaoPorCodigo(volCaixa.ECOM_CAIXA_DIMENSAO);

                volCaixa.COMPRIMENTO_CM = caixaDimensao.COMPRIMENTO_CM;
                volCaixa.LARGURA_CM = caixaDimensao.LARGURA_CM;
                volCaixa.ALTURA_CM = caixaDimensao.ALTURA_CM;

                eController.InserirMagentoExpedicaoVolume(volCaixa);

                txtPeso.Text = "";
                ddlCaixa.SelectedValue = "0";

                CarregarVolumes();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void gvVolume_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_EXPEDICAO_VOLUME volume = e.Row.DataItem as ECOM_EXPEDICAO_VOLUME;

                    if (volume != null)
                    {
                        Literal litCaixa = e.Row.FindControl("litCaixa") as Literal;
                        litCaixa.Text = volume.ECOM_CAIXA_DIMENSAO1.DESCRICAO;

                        Literal litPesoKG = e.Row.FindControl("litPesoKG") as Literal;
                        litPesoKG.Text = volume.PESO_KG.ToString("###,###,##0.000");

                        Literal litComprimentoCM = e.Row.FindControl("litComprimentoCM") as Literal;
                        litComprimentoCM.Text = volume.COMPRIMENTO_CM.ToString("###,###,##0.00");

                        Literal litLarguraCM = e.Row.FindControl("litLarguraCM") as Literal;
                        litLarguraCM.Text = volume.LARGURA_CM.ToString("###,###,##0.00");

                        Literal litAlturaCM = e.Row.FindControl("litAlturaCM") as Literal;
                        litAlturaCM.Text = volume.ALTURA_CM.ToString("###,###,##0.00");

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        _btExcluir.CommandArgument = volume.CODIGO.ToString();
                    }
                }
            }

        }
        protected void gvVolume_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVolume.FooterRow;
            if (footer != null)
            {
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //SALVAR
                var u = ObterUsuario();
                if (u == null)
                {
                    labErro.Text = "Favor realizar o Login novamente...";
                    return;
                }

                if (gvVolume.Rows.Count <= 0)
                {
                    labErro.Text = "Informe a quantidade de Volumes.";
                    return;
                }

                if (hidQtdeProduto.Value != hidQtdeProdutoCaixa.Value)
                {
                    labErro.Text = "Qtde de Produtos na caixa é DIFERENTE da Qtde de Produtos no PEDIDO...";
                    return;
                }

                var pesoVolumes = eController.ObterMagentoPesoVolumePorPedido(hidPedido.Value);
                if (pesoVolumes <= 0)
                {
                    labErro.Text = "Não foi possível calcular o PESO. Entre em contato com TI.";
                    return;
                }

                var pedidoFaturado = eController.ObterMagentoFaturamento(txtPedido.Text.Trim());
                if (pedidoFaturado != null)
                {
                    labErro.Text = "Pedido já foi faturado. AVISAR O LEO!";
                    return;
                }

                //MARCA EXPEDIÇÃO
                var magExpedicao = new ECOM_EXPEDICAO();
                magExpedicao.PEDIDO = txtPedido.Text;
                magExpedicao.VOLUME = gvVolume.Rows.Count.ToString();
                magExpedicao.PESO_KG = pesoVolumes;
                magExpedicao.DATA_BAIXA = DateTime.Now;
                magExpedicao.USUARIO_BAIXA = u.CODIGO_USUARIO;
                eController.InserirMagentoExpedicao(magExpedicao);

                // SE FOR TROCA, VALIDA FORMA DE ENTREGA
                if (txtTipo.Text == "TROCAS")
                    InserirFormaEntrega(magExpedicao.PEDIDO, magExpedicao.PESO_KG);

                btSalvar.Enabled = false;

                labErro.Text = "Pedido atualizado com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                Button bt = (Button)sender;
                string codigo = bt.CommandArgument;

                eController.ExcluirMagentoExpedicaoVolume(Convert.ToInt32(codigo));

                CarregarVolumes();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void InserirFormaEntrega(string pedido, decimal pesoKG)
        {
            var frete = new ECOM_PEDIDO_FRETE();
            frete.PEDIDO = pedido;
            frete.PESO_KG = pesoKG;
            frete.TIPO_FRETE = "C";
            frete.FORMA_ENTREGA = "Economico";
            frete.VALOR_FRETE = 0;
            eController.InserirFrete(frete);
        }

    }
}


