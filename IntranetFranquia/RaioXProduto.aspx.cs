using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class RaioXProduto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListColecao();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListGrupo(string colecao)
        {
            ddlGrupo.DataSource = baseController.BuscaGruposProduto("01", colecao);
            ddlGrupo.DataBind();
        }

        private void CarregaDropDownListProduto(string grupo)
        {
            ddlProduto.DataSource = baseController.BuscaProdutos(ddlColecao.SelectedValue.ToString().Trim(), grupo);
            ddlProduto.DataBind();
        }

        private void CarregaDropDownListCor(string codigoProduto)
        {
            ddlCor.DataSource = baseController.BuscaProdutoCores(codigoProduto);
            ddlCor.DataBind();
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlProduto_DataBound(object sender, EventArgs e)
        {
            ddlProduto.Items.Add(new ListItem("Selecione", "0"));
            ddlProduto.SelectedValue = "0";
        }

        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";
        }

        protected void ddlCor_DataBound(object sender, EventArgs e)
        {
            ddlCor.Items.Add(new ListItem("Selecione", "0"));
            ddlCor.SelectedValue = "0";
        }

        protected void btMovimento_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("Selecione") ||
                ddlGrupo.SelectedValue.ToString().Equals("Selecione") ||
                ddlProduto.SelectedValue.ToString().Equals("Selecione"))
                return;

            int codigoProduto;

            if (ddlCor.SelectedValue.ToString().Equals("Selecione"))
                codigoProduto = Convert.ToInt32(ddlProduto.SelectedValue.Trim());
            else
                codigoProduto = Convert.ToInt32(ddlProduto.SelectedValue.Trim() + ddlCor.SelectedValue.Trim());

            Sp_Busca_Inicio_Fim_VendaResult dataInicioFimVenda = baseController.BuscaDataInicioFimVenda(ddlColecao.SelectedValue);

            if (dataInicioFimVenda != null)
            {
                LiteralDataInicioVenda.Text = dataInicioFimVenda.dataInicioVenda.ToString();
                LiteralDataFinalVenda.Text = dataInicioFimVenda.dataFimVenda.ToString();
                LiteralNumeroDias.Text = dataInicioFimVenda.dias.ToString();
            }

            LiteralColecao.Text = ddlColecao.SelectedItem.ToString();

            if (!ddlCor.SelectedItem.ToString().Equals("Selecione"))
                LiteralCor.Text = ddlCor.SelectedItem.ToString();

            PRODUTO produto = baseController.BuscaProduto(ddlProduto.SelectedValue.ToString());

            if (produto != null)
            {
                IMAGEM_PRODUTO imagemProduto = baseController.BuscaImagemProduto(codigoProduto);

                if (imagemProduto != null)
                    ImageProduto.ImageUrl = "~/Upload/" + imagemProduto.LOCAL_IMAGEM_PRODUTO;
                else
                    ImageProduto.ImageUrl = "~/Upload/logo.jpg";

                LiteralCodigoProduto.Text = produto.PRODUTO1;
                LiteralDescricaoProduto.Text = produto.DESC_PRODUTO;
                LiteralGrupo.Text = produto.GRUPO_PRODUTO;
                LiteralSubGrupo.Text = produto.SUBGRUPO_PRODUTO;
                LiteralTipo.Text = produto.TIPO_PRODUTO;
                LiteralLinha.Text = produto.LINHA;

                PRODUTOS_PRECO produtoPreco = baseController.BuscaPrecoTLProduto(produto.PRODUTO1);

                if (produtoPreco != null)
                {
                    LiteralPrecoFinal.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");
                    LiteralPrecoAtualVarejo.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");
                }

                PRODUTOS_PRECO produtoTAPreco = baseController.BuscaPrecoTAProduto(produto.PRODUTO1);

                if (produtoTAPreco != null)
                    LiteralPrecoAtacado.Text = Convert.ToDecimal(produtoTAPreco.PRECO1).ToString("N2");

                PRODUTOS_PRECO produtoTFPreco = baseController.BuscaPrecoTFProduto(produto.PRODUTO1);

                if (produtoTFPreco != null)
                    LiteralPrecoFranquia.Text = Convert.ToDecimal(produtoTFPreco.PRECO1).ToString("N2");

                Sp_Busca_Vendas_VarejoResult vendasVarejo = baseController.BuscaVendasVarejo(dataInicioFimVenda.dataInicioVenda.ToString(),
                                                                                             dataInicioFimVenda.dataFimVenda.ToString(), 
                                                                                             produto.PRODUTO1);
                if (vendasVarejo != null)
                {
                    LiteralEntrada.Text = vendasVarejo.qtde_entrada.ToString("#,###");
                    LiteralVendasVarejo.Text = Convert.ToInt32(vendasVarejo.qtde_venda_varejo).ToString("#,###");
                    LiteralQtdeVendasVarejo.Text = Convert.ToInt32(vendasVarejo.qtde_venda_varejo).ToString("#,###");
                    LiteralValorVendasVarejo.Text = Convert.ToDecimal(vendasVarejo.valor_venda_varejo).ToString("#,##0.00");
                    LiteralSegundaPerda.Text = vendasVarejo.qtde_2a_Qualidade.ToString("#,##0");
                    LiteralPrecoInicialVarejo.Text = Convert.ToDecimal(vendasVarejo.valor_unitario_max).ToString("#,##0.00");
                }

                Sp_Busca_Vendas_FranquiaResult vendasFranquia = baseController.BuscaVendasFranquia(dataInicioFimVenda.dataInicioVenda.ToString(),
                                                                                                   dataInicioFimVenda.dataFimVenda.ToString(),
                                                                                                   produto.PRODUTO1);
                if (vendasFranquia != null)
                {
                    LiteralTransferidoFranquia.Text = Convert.ToInt32(vendasFranquia.qtde_venda_franquia).ToString("#.###");
                    LiteralQtdeVendasFranquia.Text = Convert.ToInt32(vendasFranquia.qtde_venda_franquia).ToString("#.###");
                    LiteralValorVendasFranquia.Text = Convert.ToDecimal(vendasFranquia.valor_venda_franquia).ToString("#,##0.00");
                }

                LiteralEstoqueFinal.Text = Convert.ToInt32((vendasVarejo.qtde_entrada - (vendasVarejo.qtde_venda_varejo + vendasVarejo.qtde_2a_Qualidade + vendasFranquia.qtde_venda_franquia))).ToString("#.###");
                LiteralQtdeEstoqueTotal.Text = Convert.ToInt32((vendasVarejo.qtde_entrada - (vendasVarejo.qtde_venda_varejo + vendasVarejo.qtde_2a_Qualidade + vendasFranquia.qtde_venda_franquia))).ToString("#.###");
                LiteralQtdeVendasTotal.Text = Convert.ToInt32(vendasVarejo.qtde_venda_varejo+vendasFranquia.qtde_venda_franquia).ToString("#.###");
                LiteralValorVendasTotal.Text = Convert.ToDecimal(vendasVarejo.valor_venda_varejo + vendasFranquia.valor_venda_franquia).ToString("#,##0.00");

                Sp_Busca_Vendas_AtacadoResult vendasAtacado = baseController.BuscaVendasAtacado(dataInicioFimVenda.dataInicioVenda.ToString(),
                                                                                                dataInicioFimVenda.dataFimVenda.ToString(),
                                                                                                produto.PRODUTO1);
                if (vendasAtacado != null)
                {
                    LiteralEstoqueInicialAtacado.Text = vendasAtacado.TOTAL_QTDE_ORIGINAL.ToString("#.###"); ;
                    LiteralEntregarAtacado.Text = vendasAtacado.TOTAL_QTDE_ENTREGAR.ToString("#.###"); ;
                    LiteralEntregueAtacado.Text = vendasAtacado.TOTAL_QTDE_ENTREGUE.ToString("#.###"); ;
                    LiteralEstoqueTransferidoAtacado.Text = vendasAtacado.TOTAL_QTDE_CANCELADA.ToString("#.###"); ;
                    LiteralEstoqueDisponivelAtacado.Text = Convert.ToInt32(vendasAtacado.ESTOQUE_HBF).ToString("#.###"); ;
                }

                LiteralPercVenda.Text = (((Convert.ToDecimal(vendasVarejo.qtde_venda_varejo) + Convert.ToDecimal(vendasFranquia.qtde_venda_franquia)) / (Convert.ToDecimal(vendasVarejo.qtde_entrada) - (Convert.ToDecimal(vendasVarejo.qtde_venda_varejo) + Convert.ToDecimal(vendasVarejo.qtde_2a_Qualidade) + Convert.ToDecimal(vendasFranquia.qtde_venda_franquia)))) * 100).ToString("N2") + "%";

                string dataInicio = "";
                string dataFim = "";
                int totalVendas = 0;
                int totalVendasJaneiro = 0;
                int totalVendasFevereiro = 0;
                int totalVendasMarco = 0;
                int totalVendasAbril = 0;
                int totalVendasMaio = 0;
                int totalVendasJunho = 0;
                int totalVendasJulho = 0;
                int totalVendasAgosto = 0;
                int totalVendasSetembro = 0;
                int totalVendasOutubro = 0;
                int totalVendasNovembro = 0;
                int totalVendasDezembro = 0;

                Sp_Busca_Vendas_VarejoResult vendas;
                int? estoque;

                //Janeiro

                dataInicio = baseController.BuscaDataInicio("201144");
                dataFim = baseController.BuscaDataFim("201144");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201144, Convert.ToInt32(produto.PRODUTO1));

                LiteralJaneiroPcVarejo44.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJaneiroQtde44.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJaneiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJaneiroQtde44.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJaneiroSemanaVd44.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJaneiroSemanaVd44.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201145");
                dataFim = baseController.BuscaDataFim("201145");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201145, Convert.ToInt32(produto.PRODUTO1));

                LiteralJaneiroPcVarejo45.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJaneiroQtde45.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJaneiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJaneiroQtde45.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJaneiroSemanaVd45.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJaneiroSemanaVd45.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201146");
                dataFim = baseController.BuscaDataFim("201146");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201146, Convert.ToInt32(produto.PRODUTO1));

                LiteralJaneiroPcVarejo46.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJaneiroQtde46.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJaneiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJaneiroQtde46.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJaneiroSemanaVd46.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJaneiroSemanaVd46.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201147");
                dataFim = baseController.BuscaDataFim("201147");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201147, Convert.ToInt32(produto.PRODUTO1));

                LiteralJaneiroPcVarejo47.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJaneiroQtde47.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJaneiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJaneiroQtde47.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJaneiroSemanaVd47.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJaneiroSemanaVd47.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201148");
                dataFim = baseController.BuscaDataFim("201148");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201148, Convert.ToInt32(produto.PRODUTO1));

                LiteralJaneiroPcVarejo48.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJaneiroQtde48.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJaneiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJaneiroQtde48.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJaneiroSemanaVd48.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJaneiroSemanaVd48.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralJaneiroQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Fevereiro

                dataInicio = baseController.BuscaDataInicio("201149");
                dataFim = baseController.BuscaDataFim("201149");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201149, Convert.ToInt32(produto.PRODUTO1));

                LiteralFevereiroPcVarejo49.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralFevereiroQtde49.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasFevereiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralFevereiroQtde49.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralFevereiroSemanaVd49.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralFevereiroSemanaVd49.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201150");
                dataFim = baseController.BuscaDataFim("201150");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201150, Convert.ToInt32(produto.PRODUTO1));

                LiteralFevereiroPcVarejo50.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralFevereiroQtde50.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasFevereiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralFevereiroQtde50.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralFevereiroSemanaVd50.Text = "0";
                
                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralFevereiroSemanaVd50.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201151");
                dataFim = baseController.BuscaDataFim("201151");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201151, Convert.ToInt32(produto.PRODUTO1));

                LiteralFevereiroPcVarejo51.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralFevereiroQtde51.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasFevereiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralFevereiroQtde51.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralFevereiroSemanaVd51.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralFevereiroSemanaVd51.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201152");
                dataFim = baseController.BuscaDataFim("201152");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201152, Convert.ToInt32(produto.PRODUTO1));

                LiteralFevereiroPcVarejo52.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralFevereiroQtde52.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasFevereiro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralFevereiroQtde52.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralFevereiroSemanaVd52.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralFevereiroSemanaVd52.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                //Março

                dataInicio = baseController.BuscaDataInicio("201201");
                dataFim = baseController.BuscaDataFim("201201");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201201, Convert.ToInt32(produto.PRODUTO1));

                LiteralMarcoPcVarejo1.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMarcoQtde1.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMarco += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMarcoQtde1.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMarcoSemanaVd1.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMarcoSemanaVd1.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201202");
                dataFim = baseController.BuscaDataFim("201202");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201202, Convert.ToInt32(produto.PRODUTO1));

                LiteralMarcoPcVarejo2.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMarcoQtde2.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMarco += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMarcoQtde2.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMarcoSemanaVd2.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMarcoSemanaVd2.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201203");
                dataFim = baseController.BuscaDataFim("201203");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201203, Convert.ToInt32(produto.PRODUTO1));

                LiteralMarcoPcVarejo3.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMarcoQtde3.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMarco += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMarcoQtde3.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMarcoSemanaVd3.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMarcoSemanaVd3.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201204");
                dataFim = baseController.BuscaDataFim("201204");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201204, Convert.ToInt32(produto.PRODUTO1));

                LiteralMarcoPcVarejo4.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMarcoQtde4.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMarco += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMarcoQtde4.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMarcoSemanaVd4.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMarcoSemanaVd4.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralMarcoQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Abril

                dataInicio = baseController.BuscaDataInicio("201205");
                dataFim = baseController.BuscaDataFim("201205");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201205, Convert.ToInt32(produto.PRODUTO1));

                LiteralAbrilPcVarejo5.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAbrilQtde5.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAbril += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAbrilQtde5.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAbrilSemanaVd5.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAbrilSemanaVd5.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201206");
                dataFim = baseController.BuscaDataFim("201206");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201206, Convert.ToInt32(produto.PRODUTO1));

                LiteralAbrilPcVarejo6.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAbrilQtde6.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAbril += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAbrilQtde6.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAbrilSemanaVd6.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAbrilSemanaVd6.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201207");
                dataFim = baseController.BuscaDataFim("201207");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201207, Convert.ToInt32(produto.PRODUTO1));

                LiteralAbrilPcVarejo7.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAbrilQtde7.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAbril += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAbrilQtde7.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAbrilSemanaVd7.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAbrilSemanaVd7.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201208");
                dataFim = baseController.BuscaDataFim("201208");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201208, Convert.ToInt32(produto.PRODUTO1));

                LiteralAbrilPcVarejo8.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAbrilQtde8.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAbril += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAbrilQtde8.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAbrilSemanaVd8.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAbrilSemanaVd8.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201209");
                dataFim = baseController.BuscaDataFim("201209");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201209, Convert.ToInt32(produto.PRODUTO1));

                LiteralAbrilPcVarejo9.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAbrilQtde9.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAbril += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAbrilQtde9.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAbrilSemanaVd9.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAbrilSemanaVd9.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralAbrilQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Maio

                dataInicio = baseController.BuscaDataInicio("201210");
                dataFim = baseController.BuscaDataFim("201210");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201210, Convert.ToInt32(produto.PRODUTO1));

                LiteralMaioPcVarejo10.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMaioQtde10.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMaio += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMaioQtde10.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMaioSemanaVd10.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMaioSemanaVd10.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201211");
                dataFim = baseController.BuscaDataFim("201211");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201211, Convert.ToInt32(produto.PRODUTO1));

                LiteralMaioPcVarejo11.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMaioQtde11.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMaio += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMaioQtde11.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMaioSemanaVd11.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMaioSemanaVd11.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201212");
                dataFim = baseController.BuscaDataFim("201212");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201212, Convert.ToInt32(produto.PRODUTO1));

                LiteralMaioPcVarejo12.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMaioQtde12.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMaio += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMaioQtde12.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMaioSemanaVd12.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMaioSemanaVd12.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201213");
                dataFim = baseController.BuscaDataFim("201213");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201213, Convert.ToInt32(produto.PRODUTO1));

                LiteralMaioPcVarejo13.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralMaioQtde13.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasMaio += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralMaioQtde13.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralMaioSemanaVd13.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralMaioSemanaVd13.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralMaioQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Junho

                dataInicio = baseController.BuscaDataInicio("201214");
                dataFim = baseController.BuscaDataFim("201214");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201214, Convert.ToInt32(produto.PRODUTO1));

                LiteralJunhoPcVarejo14.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJunhoQtde14.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJunho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJunhoQtde14.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJunhoSemanaVd14.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJunhoSemanaVd14.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201215");
                dataFim = baseController.BuscaDataFim("201215");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201215, Convert.ToInt32(produto.PRODUTO1));

                LiteralJunhoPcVarejo15.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJunhoQtde15.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJunho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJunhoQtde15.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJunhoSemanaVd15.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJunhoSemanaVd15.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201216");
                dataFim = baseController.BuscaDataFim("201216");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201216, Convert.ToInt32(produto.PRODUTO1));

                LiteralJunhoPcVarejo16.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJunhoQtde16.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJunho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJunhoQtde16.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJunhoSemanaVd16.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJunhoSemanaVd16.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201217");
                dataFim = baseController.BuscaDataFim("201217");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201217, Convert.ToInt32(produto.PRODUTO1));

                LiteralJunhoPcVarejo17.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJunhoQtde17.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJunho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJunhoQtde17.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJunhoSemanaVd17.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJunhoSemanaVd17.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralJunhoQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Julho

                dataInicio = baseController.BuscaDataInicio("201218");
                dataFim = baseController.BuscaDataFim("201218");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201218, Convert.ToInt32(produto.PRODUTO1));

                LiteralJulhoPcVarejo18.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJulhoQtde18.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJulho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJulhoQtde18.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJulhoSemanaVd18.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJulhoSemanaVd18.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201219");
                dataFim = baseController.BuscaDataFim("201219");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201219, Convert.ToInt32(produto.PRODUTO1));

                LiteralJulhoPcVarejo19.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJulhoQtde19.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJulho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJulhoQtde19.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJulhoSemanaVd19.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJulhoSemanaVd19.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201220");
                dataFim = baseController.BuscaDataFim("201220");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201220, Convert.ToInt32(produto.PRODUTO1));

                LiteralJulhoPcVarejo20.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJulhoQtde20.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJulho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJulhoQtde20.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJulhoSemanaVd20.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJulhoSemanaVd20.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201221");
                dataFim = baseController.BuscaDataFim("201221");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201221, Convert.ToInt32(produto.PRODUTO1));

                LiteralJulhoPcVarejo21.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJulhoQtde21.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJulho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJulhoQtde21.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJulhoSemanaVd21.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJulhoSemanaVd21.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201222");
                dataFim = baseController.BuscaDataFim("201222");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201222, Convert.ToInt32(produto.PRODUTO1));

                LiteralJulhoPcVarejo22.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralJulhoQtde22.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasJulho += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralJulhoQtde22.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralJulhoSemanaVd22.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralJulhoSemanaVd22.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralJulhoQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Agosto

                dataInicio = baseController.BuscaDataInicio("201223");
                dataFim = baseController.BuscaDataFim("201223");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201223, Convert.ToInt32(produto.PRODUTO1));

                LiteralAgostoPcVarejo23.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAgostoQtde23.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAgosto += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAgostoQtde23.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAgostoSemanaVd23.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAgostoSemanaVd23.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201224");
                dataFim = baseController.BuscaDataFim("201224");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201224, Convert.ToInt32(produto.PRODUTO1));

                LiteralAgostoPcVarejo24.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAgostoQtde24.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAgosto += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAgostoQtde24.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAgostoSemanaVd24.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAgostoSemanaVd24.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201225");
                dataFim = baseController.BuscaDataFim("201225");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201225, Convert.ToInt32(produto.PRODUTO1));

                LiteralAgostoPcVarejo25.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAgostoQtde25.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAgosto += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAgostoQtde25.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAgostoSemanaVd25.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAgostoSemanaVd25.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201226");
                dataFim = baseController.BuscaDataFim("201226");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201226, Convert.ToInt32(produto.PRODUTO1));

                LiteralAgostoPcVarejo26.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralAgostoQtde26.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasAgosto += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralAgostoQtde26.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralAgostoSemanaVd26.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralAgostoSemanaVd26.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralAgostoQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Setembro

                dataInicio = baseController.BuscaDataInicio("201227");
                dataFim = baseController.BuscaDataFim("201227");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201227, Convert.ToInt32(produto.PRODUTO1));

                LiteralSetembroPcVarejo27.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralSetembroQtde27.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasSetembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralSetembroQtde27.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralSetembroSemanaVd27.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralSetembroSemanaVd27.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201228");
                dataFim = baseController.BuscaDataFim("201228");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201228, Convert.ToInt32(produto.PRODUTO1));

                LiteralSetembroPcVarejo28.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralSetembroQtde28.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasSetembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralSetembroQtde28.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralSetembroSemanaVd28.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralSetembroSemanaVd28.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201229");
                dataFim = baseController.BuscaDataFim("201229");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201229, Convert.ToInt32(produto.PRODUTO1));

                LiteralSetembroPcVarejo29.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralSetembroQtde29.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasSetembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralSetembroQtde29.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralSetembroSemanaVd29.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralSetembroSemanaVd29.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201230");
                dataFim = baseController.BuscaDataFim("201230");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201230, Convert.ToInt32(produto.PRODUTO1));

                LiteralSetembroPcVarejo30.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralSetembroQtde30.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasSetembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralSetembroQtde30.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralSetembroSemanaVd30.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralSetembroSemanaVd30.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralSetembroQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Outubro

                dataInicio = baseController.BuscaDataInicio("201231");
                dataFim = baseController.BuscaDataFim("201231");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201231, Convert.ToInt32(produto.PRODUTO1));

                LiteralOutubroPcVarejo31.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralOutubroQtde31.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasOutubro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralOutubroQtde31.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralOutubroSemanaVd31.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralOutubroSemanaVd31.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201232");
                dataFim = baseController.BuscaDataFim("201232");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201232, Convert.ToInt32(produto.PRODUTO1));

                LiteralOutubroPcVarejo32.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralOutubroQtde32.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasOutubro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralOutubroQtde32.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralOutubroSemanaVd32.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralOutubroSemanaVd32.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201233");
                dataFim = baseController.BuscaDataFim("201233");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201233, Convert.ToInt32(produto.PRODUTO1));

                LiteralOutubroPcVarejo33.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralOutubroQtde33.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasOutubro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralOutubroQtde33.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralOutubroSemanaVd33.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralOutubroSemanaVd33.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201234");
                dataFim = baseController.BuscaDataFim("201234");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201234, Convert.ToInt32(produto.PRODUTO1));

                LiteralOutubroPcVarejo34.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralOutubroQtde34.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasOutubro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralOutubroQtde34.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralOutubroSemanaVd34.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralOutubroSemanaVd34.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201235");
                dataFim = baseController.BuscaDataFim("201235");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201235, Convert.ToInt32(produto.PRODUTO1));

                LiteralOutubroPcVarejo35.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralOutubroQtde35.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasOutubro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralOutubroQtde35.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralOutubroSemanaVd35.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralOutubroSemanaVd35.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralOutubroQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Novembro

                dataInicio = baseController.BuscaDataInicio("201236");
                dataFim = baseController.BuscaDataFim("201236");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201236, Convert.ToInt32(produto.PRODUTO1));

                LiteralNovembroPcVarejo36.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralNovembroQtde36.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasNovembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralNovembroQtde36.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralNovembroSemanaVd36.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralNovembroSemanaVd36.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201237");
                dataFim = baseController.BuscaDataFim("201237");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201237, Convert.ToInt32(produto.PRODUTO1));

                LiteralNovembroPcVarejo37.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralNovembroQtde37.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasNovembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralNovembroQtde37.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralNovembroSemanaVd37.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralNovembroSemanaVd37.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201238");
                dataFim = baseController.BuscaDataFim("201238");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201238, Convert.ToInt32(produto.PRODUTO1));

                LiteralNovembroPcVarejo38.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralNovembroQtde38.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasNovembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralNovembroQtde38.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralNovembroSemanaVd38.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralNovembroSemanaVd38.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201239");
                dataFim = baseController.BuscaDataFim("201239");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201239, Convert.ToInt32(produto.PRODUTO1));

                LiteralNovembroPcVarejo39.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralNovembroQtde39.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasNovembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralNovembroQtde39.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralNovembroSemanaVd39.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralNovembroSemanaVd39.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralNovembroQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                //Dezembro

                dataInicio = baseController.BuscaDataInicio("201240");
                dataFim = baseController.BuscaDataFim("201240");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201240, Convert.ToInt32(produto.PRODUTO1));

                LiteralDezembroPcVarejo40.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralDezembroQtde40.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasDezembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralDezembroQtde40.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralDezembroSemanaVd40.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralDezembroSemanaVd40.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201241");
                dataFim = baseController.BuscaDataFim("201241");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201241, Convert.ToInt32(produto.PRODUTO1));

                LiteralDezembroPcVarejo41.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralDezembroQtde41.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasDezembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralDezembroQtde41.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralDezembroSemanaVd41.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralDezembroSemanaVd41.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201242");
                dataFim = baseController.BuscaDataFim("201242");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201242, Convert.ToInt32(produto.PRODUTO1));

                LiteralDezembroPcVarejo42.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralDezembroQtde42.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasDezembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralDezembroQtde42.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralDezembroSemanaVd42.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralDezembroSemanaVd42.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                dataInicio = baseController.BuscaDataInicio("201243");
                dataFim = baseController.BuscaDataFim("201243");

                vendas = baseController.BuscaVendasVarejo(dataInicio, dataFim, produto.PRODUTO1);
                estoque = baseController.BuscaEstoqueFinalSemanaProduto(201243, Convert.ToInt32(produto.PRODUTO1));

                LiteralDezembroPcVarejo43.Text = Convert.ToDecimal(produtoPreco.PRECO1).ToString("N2");

                LiteralDezembroQtde43.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                {
                    totalVendas += Convert.ToInt32(vendas.qtde_venda_varejo);
                    totalVendasDezembro += Convert.ToInt32(vendas.qtde_venda_varejo);
                    LiteralDezembroQtde43.Text = Convert.ToInt32(vendas.qtde_venda_varejo).ToString("###0");
                }

                LiteralDezembroSemanaVd43.Text = "0";

                if (vendas != null && vendas.qtde_venda_varejo > 0)
                    LiteralDezembroSemanaVd43.Text = (Convert.ToDecimal(estoque) / Convert.ToDecimal(vendas.qtde_venda_varejo)).ToString("N2");

                LiteralDezembroQtdeTotal.Text = (Convert.ToInt32(totalVendas)).ToString("###0");
                totalVendas = 0;

                LiteralJaneiro.Text = totalVendasJaneiro.ToString("###0");
                LiteralFevereiro.Text = totalVendasFevereiro.ToString("###0");
                LiteralMarco.Text = totalVendasMarco.ToString("###0");
                LiteralAbril.Text = totalVendasAbril.ToString("###0");
                LiteralMaio.Text = totalVendasMaio.ToString("###0");
                LiteralJunho.Text = totalVendasJunho.ToString("###0");
                LiteralJulho.Text = totalVendasJulho.ToString("###0");
                LiteralAgosto.Text = totalVendasAgosto.ToString("###0");
                LiteralSetembro.Text = totalVendasSetembro.ToString("###0");
                LiteralOutubro.Text = totalVendasOutubro.ToString("###0");
                LiteralNovembro.Text = totalVendasNovembro.ToString("###0");
                LiteralDezembro.Text = totalVendasDezembro.ToString("###0");

                LiteralTotalVarejoMensal.Text = (totalVendasJaneiro+totalVendasFevereiro+totalVendasMarco+totalVendasAbril+totalVendasMaio+totalVendasJunho+totalVendasJulho+totalVendasAgosto+totalVendasSetembro+totalVendasOutubro+totalVendasNovembro+totalVendasDezembro).ToString("##.###");
            }
        }

        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaDropDownListGrupo(ddlColecao.SelectedValue.ToString().Trim());
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaDropDownListProduto(ddlGrupo.SelectedValue.ToString().Trim());
        }

        protected void ddlProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaDropDownListCor(ddlProduto.SelectedValue.ToString().Trim());
        }
    }
}
