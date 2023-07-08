using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;

namespace Relatorios
{
    public partial class AlteraItemNotaDefeito : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CodigoNotaRetirada"] != null)
                {
                    int codigoNotaRetirada = Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"].ToString());

                    CarregaGridViewNotaRetiradaItens(codigoNotaRetirada);
                }
            }
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            string destino = "";
            string corProduto = "";
            string descricaoProduto = "";
            int codigoProduto = 0;
            int totalRegistros = 0;

            foreach (GridViewRow item in GridViewNotaRetiradaItem.Rows)
            {
                CheckBox cbAlterado = item.FindControl("cbAlterado") as CheckBox;

                if (cbAlterado != null)
                {
                    if (cbAlterado.Checked)
                    {
                        TextBox txtCodigoProduto = item.FindControl("txtCodigoProduto") as TextBox;

                        if (txtCodigoProduto != null)
                        {
                            if (!txtCodigoProduto.Text.Equals(""))
                            {
                                codigoProduto = Convert.ToInt32(txtCodigoProduto.Text);

                                PRODUTO produto = baseController.BuscaProduto(txtCodigoProduto.Text);

                                if (produto != null)
                                    descricaoProduto = produto.DESC_PRODUTO;
                            }
                        }

                        DropDownList ddlCorProduto = item.FindControl("ddlCorProduto") as DropDownList;

                        if (ddlCorProduto != null)
                            corProduto = ddlCorProduto.SelectedValue;

                        DropDownList ddlDestino = item.FindControl("ddlDestino") as DropDownList;

                        if (ddlDestino != null)
                            destino = ddlDestino.SelectedValue;

                        usuarioController.AtualizaNotaRetiradaItem(Convert.ToInt32(GridViewNotaRetiradaItem.DataKeys[item.RowIndex].Value), destino, codigoProduto, descricaoProduto, corProduto);

                        totalRegistros++;
                    }
                }
            }

            lblMensagem.Text = totalRegistros + " Gravados com sucesso !!!";

            CarregaGridViewNotaRetiradaItens(Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"]));
        }

        protected void GridViewNotaRetiradaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            if (notaRetiradaItem != null)
            {
                Button btExcluir = e.Row.FindControl("btExcluir") as Button;

                if (btExcluir != null)
                    btExcluir.CommandArgument = notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM.ToString();

                TextBox txtCodigoProduto = e.Row.FindControl("txtCodigoProduto") as TextBox;

                if (txtCodigoProduto != null)
                    txtCodigoProduto.Text = notaRetiradaItem.CODIGO_PRODUTO.ToString();

                Literal literalOrigemDefeito = e.Row.FindControl("LiteralOrigemDefeito") as Literal;

                if (literalOrigemDefeito != null)
                {
                    NOTA_RETIRADA_ORIGEM_DEFEITO origemDefeito = baseController.BuscaOrigemDefeito(notaRetiradaItem.CODIGO_ORIGEM_DEFEITO);

                    if (origemDefeito != null)
                        literalOrigemDefeito.Text = origemDefeito.DESCRICAO_ORIGEM_DEFEITO;
                }

                Literal literalDefeito = e.Row.FindControl("LiteralDefeito") as Literal;

                if (literalDefeito != null)
                {
                    NOTA_RETIRADA_DEFEITO defeito = baseController.BuscaDefeito(notaRetiradaItem.CODIGO_ORIGEM_DEFEITO);

                    if (defeito != null)
                        literalDefeito.Text = defeito.DESCRICAO_DEFEITO;
                }

                DropDownList ddlCorProduto = e.Row.FindControl("ddlCorProduto") as DropDownList;

                if (ddlCorProduto != null)
                {
                    ddlCorProduto.DataSource = baseController.BuscaProdutoCores(notaRetiradaItem.CODIGO_PRODUTO.ToString());
                    ddlCorProduto.DataBind();
                    ddlCorProduto.Items.Add(new ListItem("Selecione", "0"));

                    if (notaRetiradaItem.COR_PRODUTO != null)
                        ddlCorProduto.SelectedValue = notaRetiradaItem.COR_PRODUTO;
                    else
                        ddlCorProduto.SelectedValue = "0";
                }

                DropDownList ddlDestino = e.Row.FindControl("ddlDestino") as DropDownList;

                if (ddlDestino != null)
                {
                    ddlDestino.DataSource = baseController.BuscaDestinos();
                    ddlDestino.DataBind();
                    ddlDestino.Items.Add(new ListItem("Selecione", "0"));

                    if (notaRetiradaItem.CODIGO_DESTINO != null)
                        ddlDestino.SelectedValue = notaRetiradaItem.CODIGO_DESTINO.ToString();
                    else
                        ddlDestino.SelectedValue = "0";
                }

                Literal literalColecao = e.Row.FindControl("LiteralDescricaoColecao") as Literal;

                if (literalColecao != null)
                {
                    COLECOE colecoes = baseController.BuscaColecaoPorProduto(notaRetiradaItem.CODIGO_PRODUTO);

                    if (colecoes != null)
                        literalColecao.Text = colecoes.DESC_COLECAO;
                }
            }
        }

        private void CarregaGridViewNotaRetiradaItens(int codigoNotaRetirada)
        {
            List<NOTA_RETIRADA_ITEM> listaNotaRetiradaItem = usuarioController.BuscaNotaRetiradaItens(codigoNotaRetirada);

            GridViewNotaRetiradaItem.DataSource = listaNotaRetiradaItem;
            GridViewNotaRetiradaItem.DataBind();
        }

        private void LimpaFeedBack()
        {
            lblMensagem.Text = string.Empty;
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button btDeletar = sender as Button;

            if (btDeletar != null)
            {
                NOTA_RETIRADA_ITEM notaRetiradaItem = baseController.BuscaNotaRetiradaItem(Convert.ToInt32(btDeletar.CommandArgument));

                if (notaRetiradaItem != null)
                {
                    usuarioController.ExcluiNotaRetiradaItem(notaRetiradaItem.CODIGO_NOTA_RETIRADA_ITEM);

                    CarregaGridViewNotaRetiradaItens(notaRetiradaItem.CODIGO_NOTA_RETIRADA);
                }
            }
        }
    }
}