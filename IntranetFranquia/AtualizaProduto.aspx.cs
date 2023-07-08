using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class AtualizaProduto : System.Web.UI.Page
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

        protected void btGravaMovimento_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals(""))
                return;

            lblMensagem.Text = "Aguarde ...";

            Sp_Pega_Data_BancoResult dt = baseController.BuscaDataBanco();

            Sp_Busca_Semana_454Result bs = baseController.BuscaSemana454(dt.data.ToString());

            string semana = bs.ano_semana.ToString();

            if (semana.Substring(4, 2).Equals("01"))
                semana = (dt.data.Year - 1).ToString()+"52";
            else
                semana = (bs.ano_semana - 1).ToString();

            //Deleta Movimento Atual e Movimentos Anteriores

            baseController.DeletaMovimentoProduto(semana);

            // Gera Movimento de Produtos

            List<PRODUTO> listProduto = baseController.BuscaProdutos(ddlColecao.SelectedValue);
            
            foreach (PRODUTO item in listProduto)
            {
                List<MovimentoProduto> listMovimentoProduto = baseController.BuscaMovimentoProduto(baseController.BuscaDataInicio(semana), baseController.BuscaDataFim(semana), item.PRODUTO1);

                if (listMovimentoProduto != null)
                {
                    int itemAnterior = 0;
                    PRODUTO_MOVIMENTO_PRODUTO mp = null;

                    foreach (MovimentoProduto item2 in listMovimentoProduto)
                    {
                        // Quebrar e gravar totais

                        if (item2.Estoque > 0 ||
                            item2.Recebido_semana > 0 ||
                            item2.Venda_semana > 0)
                        {
                            if (item2.Produto != itemAnterior)
                            {
                                itemAnterior = item2.Produto;

                                mp = new PRODUTO_MOVIMENTO_PRODUTO();

                                mp.colecao = Convert.ToInt32(item2.Colecao);
                                //mp.cor = produto.PRODUTO_COREs
                                mp.data = baseController.AjustaData(baseController.BuscaDataFim(semana));
                                mp.estoque = 0;
                                mp.filial = "AAA-" + item2.Produto.ToString()+"-"+item2.Colecao + "-" + item2.Grupo.Trim() + "-" + item2.Modelo.Trim() + "-" + item2.Preco.Trim();
                                mp.grupo = item2.Grupo;
                                mp.modelo = item2.Modelo;
                                mp.produto = item2.Produto;
                                mp.recebido_semana = 0;
                                mp.venda_semana = 0;
                                mp.recebido_geral = 0;
                                mp.venda_geral = 0;
                                mp.preco = Convert.ToDecimal(item2.Preco);

                                baseController.IncluirMovimentoProduto(mp);
                            }

                            mp = new PRODUTO_MOVIMENTO_PRODUTO();

                            mp.colecao = Convert.ToInt32(item2.Colecao);
                            //mp.cor = produto.PRODUTO_COREs
                            mp.data = baseController.AjustaData(baseController.BuscaDataFim(semana));
                            mp.estoque = item2.Estoque;
                            mp.filial = item2.Filial;
                            mp.grupo = item2.Grupo;
                            mp.modelo = item2.Modelo;
                            mp.produto = item2.Produto;
                            mp.recebido_semana = item2.Recebido_semana;
                            mp.venda_semana = item2.Venda_semana;
                            mp.recebido_geral = item2.Recebido_geral;
                            mp.venda_geral = item2.Venda_geral;
                            mp.preco = Convert.ToDecimal(item2.Preco);

                            baseController.IncluirMovimentoProduto(mp);
                        }
                    }
                }
            }
            
            lblMensagem.Text = "Movimento de Produtos - Gravado com Sucesso !!!";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }
    }
}