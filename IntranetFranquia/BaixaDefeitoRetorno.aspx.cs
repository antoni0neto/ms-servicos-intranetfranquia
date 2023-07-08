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
    public partial class BaixaDefeitoRetorno : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListGriffe();
                CarregaDropDownListGrupo();
            }
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffes();
            ddlGriffe.DataBind();
        }

        private void CarregaDropDownListGrupo()
        {
            ddlGrupo.DataSource = baseController.BuscaGrupos();
            ddlGrupo.DataBind();
        }
        
        private void CarregaGridViewProdutos()
        {
            GridViewProdutos.DataSource = baseController.BuscaDefeitoRetorno(ddlGriffe.SelectedItem.ToString(), ddlGrupo.SelectedItem.ToString());
            GridViewProdutos.DataBind();
        }

        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";
        }

        protected void btDefeitoRetorno_Click(object sender, EventArgs e)
        {
            CarregaGridViewProdutos();
        }

        protected void btBaixar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewProdutos.Rows)
            {
                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        TextBox txtQtdeBaixa = item.FindControl("txtQtdeBaixa") as TextBox;

                        if (txtQtdeBaixa != null)
                        {
                            if (!txtQtdeBaixa.Text.Equals(""))
                            {
                                int i = atualizar(Convert.ToInt32(txtQtdeBaixa.Text), item);

                                Literal literalBaixado = item.FindControl("LiteralBaixado") as Literal;

                                if (literalBaixado != null)
                                    literalBaixado.Text = i.ToString();
                            }
                        }
                    }
                }
            }

            //LabelFeedBack.Text = "Foram Atualizados " + contProduto + " Produtos !!!";

            //CarregaGridViewProdutos();
        }

        public int atualizar(int qtde, GridViewRow item)
        {
            int i = 0;

            List<NOTA_DEFEITO_RETORNO> listaNotaDefeitoRetorno = baseController.BuscaNotaDefeitoRetorno(item.Cells[0].Text, item.Cells[2].Text, item.Cells[3].Text, item.Cells[4].Text, item.Cells[6].Text, item.Cells[8].Text);

            if (listaNotaDefeitoRetorno != null)
            {
                if (listaNotaDefeitoRetorno.Count < qtde)
                    return i;

                foreach (NOTA_DEFEITO_RETORNO itemLista in listaNotaDefeitoRetorno)
                {
                    i++;

                    baseController.AtualizaDefeitoRetorno(itemLista);

                    if (i == qtde)
                        break;
                }
            }

            return i;
        }

        protected void GridViewProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Busca_Defeito_RetornoResult defeitoRetorno = e.Row.DataItem as Sp_Busca_Defeito_RetornoResult;

            if (defeitoRetorno != null)
            {
                Literal literalDestino = e.Row.FindControl("literalDestino") as Literal;

                if (literalDestino != null)
                {
                    NOTA_RETIRADA_DESTINO destino = baseController.BuscaNotaRetiradaDestino(defeitoRetorno.destino);

                    if (destino != null)
                        literalDestino.Text = destino.DESCRICAO_DESTINO;
                }

                Literal literalProduto = e.Row.FindControl("literalProduto") as Literal;

                if (literalProduto != null)
                {
                    PRODUTO produto = baseController.BuscaProduto(defeitoRetorno.produto);

                    if (produto != null)
                        literalProduto.Text = produto.DESC_PRODUTO;
                }

                Literal literalCor = e.Row.FindControl("literalCor") as Literal;

                if (literalCor != null)
                {
                    Sp_Busca_Produto_CorResult produtoCor = baseController.BuscaProdutoCor(defeitoRetorno.produto, defeitoRetorno.cor);

                    if (produtoCor != null)
                        literalCor.Text = produtoCor.DESC_COR_PRODUTO;
                }

                Literal literalTamanho = e.Row.FindControl("literalTamanho") as Literal;

                if (literalTamanho != null)
                {
                    PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarraGradeTamanho(defeitoRetorno.produto, defeitoRetorno.cor, defeitoRetorno.tamanho);

                    if (produtoBarra != null)
                        literalTamanho.Text = produtoBarra.GRADE;
                }
            }
        }
    }
}
