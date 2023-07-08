using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class gest_estoque_atualiza : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListCategoria();
                CarregaDropDownListColecao();
                CarregaDropDownListSemana454();
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListCategoria()
        {
            ddlCategoria.DataSource = baseController.BuscaCategorias();
            ddlCategoria.DataBind();
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
                ddlCategoria.SelectedValue.ToString().Equals("0") ||
                ddlCategoria.SelectedValue.ToString().Equals(""))
                return;

            lblMensagem.Text = "Aguarde ...                                                        ";

            baseController.DeletaMovimentoEstoque(ddlSemana454.SelectedValue, ddlCategoria.SelectedValue, ddlColecao.SelectedValue);

            ESTOQUE_DATA ed = new ESTOQUE_DATA();

            Sp_Estoque_Semana_Fechada_Grupo_ProdutoResult sp_LineReportEstoqueSemanaFechadaResult = null;
            Sp_Estoque_Semana_Fechada_Grupo_Produto_SubResult sp_LineReportEstoqueSemanaFechadaSubResult = null;

            List<Sp_Busca_Grupos_ProdutoResult> listaGrupoProduto = null;
            List<Sp_Busca_SubGrupos_ProdutoResult> listaSubGrupoProduto = null;

            baseController.BuscaEstoqueSemanaBase(baseController.AjustaData(baseController.BuscaDataInicio(ddlSemana454.SelectedValue)),
                                                  baseController.AjustaData(baseController.BuscaDataFim(ddlSemana454.SelectedValue)));

            if (ddlCategoria.SelectedValue.Equals("01"))
            {
                listaGrupoProduto = baseController.BuscaGruposProduto(ddlCategoria.SelectedValue, ddlColecao.SelectedValue);

                if (listaGrupoProduto != null)
                {
                    foreach (Sp_Busca_Grupos_ProdutoResult item in listaGrupoProduto)
                    {
                        sp_LineReportEstoqueSemanaFechadaResult = baseController.BuscaEstoqueSemanaFechada(item.grupo,
                                                                                                           item.griffe,
                                                                                                           ddlColecao.SelectedValue,
                                                                                                           ddlCategoria.SelectedValue);
                        if (sp_LineReportEstoqueSemanaFechadaResult != null)
                        {
                            ed.ANO_SEMANA = Convert.ToInt32(ddlSemana454.SelectedValue);
                            ed.CATEGORIA = ddlCategoria.SelectedValue;
                            ed.COLECAO = ddlColecao.SelectedValue;
                            ed.GRIFFE = item.griffe;
                            ed.GRUPO = item.grupo;
                            ed.ESTOQUE_INICIAL = Convert.ToInt32(sp_LineReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL);
                            ed.ESTOQUE_FINAL = Convert.ToInt32(sp_LineReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL);
                            ed.VALOR_ESTOQUE_FINAL = Convert.ToDecimal(sp_LineReportEstoqueSemanaFechadaResult.VALOR_ESTOQUE_FINAL);

                            baseController.IncluirMovimentoEstoque(ed);
                        }
                    }

                    lblMensagem.Text = "Movimento de Estoque de Peças - Gravado com Sucesso !!!";
                }
            }
            else
            {
                listaSubGrupoProduto = baseController.BuscaSubGruposProduto(ddlCategoria.SelectedValue, ddlColecao.SelectedValue);

                if (listaSubGrupoProduto != null)
                {
                    foreach (Sp_Busca_SubGrupos_ProdutoResult item in listaSubGrupoProduto)
                    {
                        sp_LineReportEstoqueSemanaFechadaSubResult = baseController.BuscaEstoqueSemanaFechadaSub(item.grupo,
                                                                                                                 item.griffe,
                                                                                                                 ddlColecao.SelectedValue,
                                                                                                                 ddlCategoria.SelectedValue);
                        if (sp_LineReportEstoqueSemanaFechadaSubResult != null)
                        {
                            ed.ANO_SEMANA = Convert.ToInt32(ddlSemana454.SelectedValue);
                            ed.CATEGORIA = ddlCategoria.SelectedValue;
                            ed.COLECAO = ddlColecao.SelectedValue;
                            ed.GRIFFE = item.griffe;
                            ed.GRUPO = item.grupo;
                            ed.ESTOQUE_INICIAL = Convert.ToInt32(sp_LineReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL);
                            ed.ESTOQUE_FINAL = Convert.ToInt32(sp_LineReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL);
                            ed.VALOR_ESTOQUE_FINAL = Convert.ToDecimal(sp_LineReportEstoqueSemanaFechadaSubResult.VALOR_ESTOQUE_FINAL);

                            baseController.IncluirMovimentoEstoque(ed);
                        }
                    }

                    lblMensagem.Text = "Movimento de Estoque de Acessórios - Gravado com Sucesso !!!";
                }
            }

        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlCategoria_DataBound(object sender, EventArgs e)
        {
            ddlCategoria.Items.Add(new ListItem("Selecione", "0"));
            ddlCategoria.SelectedValue = "0";
        }

        protected void ddlSemana454_DataBound(object sender, EventArgs e)
        {
            ddlSemana454.Items.Add(new ListItem("Selecione", "0"));
            ddlSemana454.SelectedValue = "0";
        }
    }
}