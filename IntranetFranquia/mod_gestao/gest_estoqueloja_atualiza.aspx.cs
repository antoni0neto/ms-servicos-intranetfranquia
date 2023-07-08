using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class gest_estoqueloja_atualiza : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListSemana454();
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListSemana454()
        {
            ddlSemana454.DataSource = baseController.BuscaDatas(2010);
            ddlSemana454.DataBind();
        }

        protected void btGravaMovimento_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("") ||
                ddlSemana454.SelectedValue.ToString().Equals("0") ||
                ddlSemana454.SelectedValue.ToString().Equals(""))
                return;

            lblMensagem.Text = "Aguarde ...";
            
            ESTOQUE_DATA_LOJA edl = new ESTOQUE_DATA_LOJA();

            List<Sp_Estoque_Data_Produto_LojaResult> sp_Estoque_Data_Produto_LojaResult = null;

            List<FILIAI> filiais = baseController.BuscaFiliais();

            if (filiais != null)
            {
                foreach (FILIAI filial in filiais)
                {
                    sp_Estoque_Data_Produto_LojaResult = baseController.BuscaEstoqueDataProdutoLoja(baseController.AjustaData(baseController.BuscaDataInicio(ddlSemana454.SelectedValue)),
                                                                                                    baseController.AjustaData(baseController.BuscaDataFim(ddlSemana454.SelectedValue)),
                                                                                                    ddlColecao.SelectedValue,
                                                                                                    filial.FILIAL);
                    if (sp_Estoque_Data_Produto_LojaResult != null)
                    {
                        foreach (Sp_Estoque_Data_Produto_LojaResult item in sp_Estoque_Data_Produto_LojaResult)
                        {
                            edl.CODIGO_FILIAL = Convert.ToInt32(filial.COD_FILIAL);
                            edl.ANO_SEMANA = Convert.ToInt32(ddlSemana454.SelectedValue);
                            edl.CODIGO_CATEGORIA = Convert.ToInt32(item.categoria);
                            edl.CODIGO_COLECAO = Convert.ToInt32(ddlColecao.SelectedValue);
                            edl.CODIGO_PRODUTO = Convert.ToInt32(item.produto);
                            edl.GRIFFE = item.griffe;
                            edl.GRUPO = item.grupo;
                            edl.SUBGRUPO = item.subgrupo;
                            edl.ESTOQUE_INICIAL = Convert.ToInt32(item.estoque_inicial);
                            edl.ESTOQUE_FINAL = Convert.ToInt32(item.estoque_final);

                            baseController.IncluirMovimentoEstoqueLoja(edl);
                        }
                    }
                }

                lblMensagem.Text = "Movimento de Estoque Loja Gravado com Sucesso!";
            }
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlSemana454_DataBound(object sender, EventArgs e)
        {
            ddlSemana454.Items.Add(new ListItem("Selecione", "0"));
            ddlSemana454.SelectedValue = "0";
        }
    }
}