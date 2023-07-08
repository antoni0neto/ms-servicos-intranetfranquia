using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Mail;
using System.Net;

namespace Relatorios
{
    public partial class EscanearProdutoNotaTransfer : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtCodigoProduto.Attributes.Add("onKeyPress", "doClick('" + btGravar.ClientID + "',event)");
            txtCodigoProduto.Focus();

            if (Request.QueryString["CodigoNotaTransfer"] != null & Convert.ToInt32(Session["CONTADOR"]) == 0)
            {
                int codigoNotaTransfer = Convert.ToInt32(Request.QueryString["CodigoNotaTransfer"].ToString());

                CarregaGridViewProdutos(codigoNotaTransfer);

                Session["CONTADOR"] = 1;

                List<NOTA_TRANSFERENCIA_ITEM> listaNotaTransferenciaItem = baseController.BuscaNotasTransferenciaItem(Convert.ToInt32(Request.QueryString["CodigoNotaTransfer"]));

                if (listaNotaTransferenciaItem != null)
                {
                    List<Produto> listaProduto = new List<Produto>();

                    int i = 0;

                    foreach (NOTA_TRANSFERENCIA_ITEM item in listaNotaTransferenciaItem)
                    {
                        i++;

                        Produto xProduto = new Produto();

                        xProduto.CodigoBarra = item.CODIGO_BARRA;

                        PRODUTO_CORE xProdutoCor = baseController.BuscaProdutoCorCodigo(item.CODIGO_BARRA.Substring(0, 5), item.CODIGO_BARRA.Substring(5, 3));

                        if (xProdutoCor != null)
                            xProduto.CorProduto = xProdutoCor.DESC_COR_PRODUTO;

                        PRODUTO xxProduto = baseController.BuscaProduto(item.CODIGO_BARRA.Substring(0, 5));

                        if (xxProduto != null)
                            xProduto.DescricaoProduto = xxProduto.DESC_PRODUTO;

                        xProduto.GradeProduto = xxProduto.GRADE;

                        xProduto.IndiceProduto = i;

                        listaProduto.Add(xProduto);
                    }

                    Session["PRODUTO"] = listaProduto;
                }
            }
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            Produto produtoGrid = null;

            Button btGravar = sender as Button;

            if (btGravar != null)
            {
                PRODUTOS_BARRA itemProduto = baseController.BuscaProdutoBarra(txtCodigoProduto.Text);

                if (itemProduto != null)
                {
                    produtoGrid = new Produto();

                    produtoGrid.CodigoBarra = txtCodigoProduto.Text;

                    PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorCodigo(itemProduto.PRODUTO, itemProduto.COR_PRODUTO);

                    if (produtoCor != null)
                        produtoGrid.CorProduto = produtoCor.DESC_COR_PRODUTO;

                    PRODUTO produto = baseController.BuscaProduto(itemProduto.PRODUTO);

                    if (produto != null)
                        produtoGrid.DescricaoProduto = produto.DESC_PRODUTO;

                    produtoGrid.GradeProduto = itemProduto.GRADE;

                    List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];

                    produtoGrid.IndiceProduto = listaProduto.Count() + 1;

                    listaProduto.Add(produtoGrid);

                    CarregaGridViewProdutos(listaProduto);

                    Session["PRODUTO"] = listaProduto;

                    LimpaTela();
                }
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];

            if (listaProduto == null || listaProduto.Count == 0 || txtDescricaoNota.Text.Equals(""))
                return;

            int codigoUltimaNotaTransfer = 0;

            if (Request.QueryString["CodigoNotaTransfer"] == null)
            {
                NOTA_TRANSFERENCIA notaTranferencia = new NOTA_TRANSFERENCIA();

                notaTranferencia.CODIGO_FILIAL = Convert.ToInt32(Session["FILIAL"]);
                notaTranferencia.DATA_INICIO = DateTime.Now;
                notaTranferencia.DESCRICAO = txtDescricaoNota.Text;

                usuarioController.InsereNotaTransferencia(notaTranferencia);

                codigoUltimaNotaTransfer = usuarioController.BuscaUltimaNotaTransferencia(Convert.ToInt32(Session["FILIAL"]));
            }

            if (codigoUltimaNotaTransfer == 0) // Alteração
            {
                codigoUltimaNotaTransfer = Convert.ToInt32(Request.QueryString["CodigoNotaTransfer"]);

                List<NOTA_TRANSFERENCIA_ITEM> listaNotaTransferenciaItem = baseController.BuscaNotasTransferenciaItem(Convert.ToInt32(Request.QueryString["CodigoNotaTransfer"]));

                if (listaNotaTransferenciaItem != null)
                {
                    foreach (NOTA_TRANSFERENCIA_ITEM item in listaNotaTransferenciaItem)
                    {
                        usuarioController.ExcluiNotaTransferenciaItem(item.CODIGO);
                    }
                }
            }

            foreach (Produto item in listaProduto)
            {
                NOTA_TRANSFERENCIA_ITEM notaTransferenciaItem = new NOTA_TRANSFERENCIA_ITEM();

                notaTransferenciaItem.CODIGO_BARRA = item.CodigoBarra;
                notaTransferenciaItem.CODIGO_NOTA_TRANSFER = codigoUltimaNotaTransfer;

                usuarioController.InsereNotaTransferenciaItem(notaTransferenciaItem);

                LabelFeedBack.Text = "Nota de Transferência Gravada com Sucesso !!!";
            }
        }

        private void CarregaGridViewProdutos(List<Produto> produtos)
        {
            GridViewProdutos.DataSource = produtos;
            GridViewProdutos.DataBind();
        }

        private void CarregaGridViewProdutos(int codigoNotaTransfer)
        {
            List<Produto> produtos = new List<Produto>();

            NOTA_TRANSFERENCIA nota = baseController.BuscaNotaTransferencia(codigoNotaTransfer);

            if (nota != null)
            {
                txtDescricaoNota.Text = nota.DESCRICAO;

                List<NOTA_TRANSFERENCIA_ITEM> itens = baseController.BuscaNotasTransferenciaItem(codigoNotaTransfer);

                if (itens != null)
                {
                    foreach (NOTA_TRANSFERENCIA_ITEM item in itens)
                    {
                        Produto produtoGrid = new Produto();

                        produtoGrid.CodigoBarra = item.CODIGO_BARRA;

                        PRODUTO_CORE produtoCor = baseController.BuscaProdutoCorCodigo(item.CODIGO_BARRA.Substring(0, 5), item.CODIGO_BARRA.Substring(5, 3));

                        if (produtoCor != null)
                            produtoGrid.CorProduto = produtoCor.DESC_COR_PRODUTO;

                        PRODUTO produto = baseController.BuscaProduto(item.CODIGO_BARRA.Substring(0, 5));

                        if (produto != null)
                            produtoGrid.DescricaoProduto = produto.DESC_PRODUTO;

                        produtoGrid.GradeProduto = produto.GRADE;

                        List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];

                        produtoGrid.IndiceProduto = listaProduto.Count() + 1;

                        listaProduto.Add(produtoGrid);

                        produtos = listaProduto;
                    }

                    GridViewProdutos.DataSource = produtos;
                    GridViewProdutos.DataBind();
                }
            }
        }

        private void LimpaTela()
        {
            txtCodigoProduto.Text = string.Empty;
        }

        protected void btExcluirProduto_Click(object sender, EventArgs e)
        {
            List<Produto> listaProduto = (List<Produto>)Session["PRODUTO"];

            Button btExcluirProduto = sender as Button;

            if (btExcluirProduto != null)
            {
                foreach (Produto item in listaProduto)
                {
                    if (item.IndiceProduto == Convert.ToInt32(btExcluirProduto.CommandArgument))
                    {
                        listaProduto.Remove(item);

                        CarregaGridViewProdutos(listaProduto);

                        break;
                    }
                }
            }
        }

        protected void GridViewProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Produto produto = e.Row.DataItem as Produto;

            if (produto != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btExcluirProduto = e.Row.FindControl("btExcluirProduto") as Button;

                    if (btExcluirProduto != null)
                    {
                        if (e.Row.DataItem != null)
                        {
                            Produto itemProduto = e.Row.DataItem as Produto;

                            btExcluirProduto.CommandArgument = itemProduto.IndiceProduto.ToString();
                        }
                    }
                }
            }
        }
    }
}