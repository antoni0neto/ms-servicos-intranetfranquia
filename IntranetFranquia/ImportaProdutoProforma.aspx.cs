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
    public partial class ImportaProdutoProforma : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListColecao();
        }

        private void CarregaDropDownListGrupo()
        {
            ddlGrupo.DataSource = baseController.BuscaGruposProduto("01", ddlColecao.SelectedValue);
            ddlGrupo.DataBind();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecao();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListJanela()
        {
            ddlJanela.DataSource = baseController.BuscaJanela();
            ddlJanela.DataBind();
        }

        private void CarregaDropDownListProforma()
        {
            ddlProforma.DataSource = baseController.BuscaProformasProvisoria();
            ddlProforma.DataBind();
        }

        private void CarregaDropDownListFornecedor()
        {
            ddlFornecedor.DataSource = baseController.BuscaFornecedor();
            ddlFornecedor.DataBind();
        }
        
        private void CarregaGridViewProdutos()
        {
            //List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto = baseController.BuscaProformaProduto(Convert.ToInt32(ddlColecao.SelectedValue), Convert.ToInt32(ddlJanela.SelectedValue), Convert.ToInt32(ddlProforma.SelectedValue));

            List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto = null;  

            if (Convert.ToInt32(ddlProforma.SelectedValue) == 1 ||
                Convert.ToInt32(ddlProforma.SelectedValue) == 3)
            {
                listaProformaProduto = baseController.BuscaProformaProduto(Convert.ToInt32(ddlColecao.SelectedValue), 1, ddlGrupo.SelectedItem.ToString(), Convert.ToInt32(ddlProforma.SelectedValue));
                GridViewProdutos.DataSource = baseController.BuscaProdutosPecasColecao(ddlColecao.SelectedValue.ToString(), "MASCULINO", ddlGrupo.SelectedItem.ToString(), listaProformaProduto);
            }

            if (Convert.ToInt32(ddlProforma.SelectedValue) == 5)
                GridViewProdutos.DataSource = baseController.BuscaProdutosAcessoriosColecao(ddlColecao.SelectedValue.ToString(), "MASCULINO", ddlGrupo.SelectedItem.ToString());

            if (Convert.ToInt32(ddlProforma.SelectedValue) == 2 ||
                Convert.ToInt32(ddlProforma.SelectedValue) == 4)
            {
                listaProformaProduto = baseController.BuscaProformaProduto(Convert.ToInt32(ddlColecao.SelectedValue), 2, ddlGrupo.SelectedItem.ToString(), Convert.ToInt32(ddlProforma.SelectedValue));
                GridViewProdutos.DataSource = baseController.BuscaProdutosPecasColecao(ddlColecao.SelectedValue.ToString(), "FEMININO", ddlGrupo.SelectedItem.ToString(), listaProformaProduto);
            }

            if (Convert.ToInt32(ddlProforma.SelectedValue) == 6)
                GridViewProdutos.DataSource = baseController.BuscaProdutosAcessoriosColecao(ddlColecao.SelectedValue.ToString(), "FEMININO", ddlGrupo.SelectedItem.ToString());
            
            GridViewProdutos.DataBind();
        }

        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlJanela_DataBound(object sender, EventArgs e)
        {
            ddlJanela.Items.Add(new ListItem("Selecione", "0"));
            ddlJanela.SelectedValue = "0";
        }

        protected void ddlProforma_DataBound(object sender, EventArgs e)
        {
            ddlProforma.Items.Add(new ListItem("Selecione", "0"));
            ddlProforma.SelectedValue = "0";
        }
        
        protected void ddlFornecedor_DataBound(object sender, EventArgs e)
        {
            ddlFornecedor.Items.Add(new ListItem("Selecione", "0"));
            ddlFornecedor.SelectedValue = "0";
        }


        protected void btGrupos_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals(""))
                return;

            CarregaDropDownListJanela();
            CarregaDropDownListProforma();
            CarregaDropDownListFornecedor();
            CarregaDropDownListGrupo();

            btBuscarProdutos.Enabled = true;
        }

        protected void btBuscarProdutos_Click(object sender, EventArgs e)
        {
            if (ddlProforma.SelectedValue.ToString().Equals("0") ||
                ddlProforma.SelectedValue.ToString().Equals("") ||
                ddlJanela.SelectedValue.ToString().Equals("0") ||
                ddlJanela.SelectedValue.ToString().Equals("") ||
                ddlGrupo.SelectedValue.ToString().Equals("0") ||
                ddlGrupo.SelectedValue.ToString().Equals("") ||
                ddlFornecedor.SelectedValue.ToString().Equals("0") ||
                ddlFornecedor.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewProdutos();

            btImportar.Enabled = true;
        }

        protected void btImportar_Click(object sender, EventArgs e)
        {
            int contProduto = 0;

            IMPORTACAO_PROFORMA_PRODUTO proformaProduto = new IMPORTACAO_PROFORMA_PRODUTO();

            foreach (GridViewRow item in GridViewProdutos.Rows)
            {
                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        if (baseController.ExisteProdutoJanela(Convert.ToInt32(item.Cells[0].Text),
                                                                Convert.ToInt32(item.Cells[4].Text),
                                                                Convert.ToInt32(ddlJanela.SelectedValue),
                                                                Convert.ToInt32(ddlProforma.SelectedValue),
                                                                Convert.ToInt32(ddlColecao.SelectedValue)).Count == 0)
                        {
                            proformaProduto.CODIGO_COLECAO = Convert.ToInt32(ddlColecao.SelectedValue);
                            proformaProduto.CODIGO_JANELA = Convert.ToInt32(ddlJanela.SelectedValue);
                            proformaProduto.CODIGO_PROFORMA = Convert.ToInt32(ddlProforma.SelectedValue);
                            proformaProduto.CODIGO_FORNECEDOR = Convert.ToInt32(ddlFornecedor.SelectedValue);

                            if (Convert.ToInt32(ddlProforma.SelectedValue) == 1 ||
                                Convert.ToInt32(ddlProforma.SelectedValue) == 3 ||
                                Convert.ToInt32(ddlProforma.SelectedValue) == 5)
                                proformaProduto.CODIGO_GRIFFE = 1; //Masculino

                            if (Convert.ToInt32(ddlProforma.SelectedValue) == 2 ||
                                Convert.ToInt32(ddlProforma.SelectedValue) == 4 ||
                                Convert.ToInt32(ddlProforma.SelectedValue) == 6)
                                proformaProduto.CODIGO_GRIFFE = 2; //Feminino

                            proformaProduto.CODIGO_PRODUTO = Convert.ToInt32(item.Cells[0].Text);
                            proformaProduto.CODIGO_PRODUTO_COR = Convert.ToInt32(item.Cells[4].Text);
                            proformaProduto.GRUPO_PRODUTO = item.Cells[5].Text;
                            proformaProduto.FOB = Convert.ToDecimal(item.Cells[3].Text);
                            proformaProduto.CODIGO_ARMARIO = 0;
                            proformaProduto.CODIGO_PACK_GRADE = 0;
                            proformaProduto.CODIGO_PACK_GROUP = 0;

                            // por enquanto, gravo número da proforma no código da nel

                            proformaProduto.CODIGO_NEL = Convert.ToInt32(ddlProforma.SelectedValue);

                            baseController.IncluirProformaProduto(proformaProduto);

                            contProduto++;
                        }
                    }
                }
            }

            LabelFeedBack.Text = "Formam importados " + contProduto + " Produtos !!!";
        }

        protected void GridViewProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Convert.ToInt32(ddlProforma.SelectedValue) == 1 ||
                Convert.ToInt32(ddlProforma.SelectedValue) == 2 ||
                Convert.ToInt32(ddlProforma.SelectedValue) == 3 ||
                Convert.ToInt32(ddlProforma.SelectedValue) == 4)
            {
                Sp_Busca_Produtos_Pecas_ColecaoResult resultado = e.Row.DataItem as Sp_Busca_Produtos_Pecas_ColecaoResult;

                if (resultado != null)
                {
                    DropDownList ddlPackGrade = e.Row.FindControl("ddlPackGrade") as DropDownList;

                    if (ddlPackGrade != null)
                    {
                        ddlPackGrade.DataSource = baseController.BuscaPackGrade();
                        ddlPackGrade.DataBind();
                    }
                }
            }

            if (Convert.ToInt32(ddlProforma.SelectedValue) == 5 ||
                Convert.ToInt32(ddlProforma.SelectedValue) == 6)
            {
                Sp_Busca_Produtos_Acessorios_ColecaoResult resultado = e.Row.DataItem as Sp_Busca_Produtos_Acessorios_ColecaoResult;

                if (resultado != null)
                {
                    DropDownList ddlPackGrade = e.Row.FindControl("ddlPackGrade") as DropDownList;

                    if (ddlPackGrade != null)
                    {
                        ddlPackGrade.DataSource = baseController.BuscaPackGrade();
                        ddlPackGrade.DataBind();
                    }
                }
            }
        }
    }
}
