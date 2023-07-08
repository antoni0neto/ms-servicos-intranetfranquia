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
    public partial class AtualizaDefeitoAtacado : System.Web.UI.Page
    {
        UsuarioController usuarioController = new UsuarioController();
        BaseController baseController = new BaseController();

        List<ProdutoDestino> listaProdutoDestino = new List<ProdutoDestino>();

        int i = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void CalendarData_SelectionChanged(object sender, EventArgs e)
        {
            txtData.Text = CalendarData.SelectedDate.ToString("yyyy-MM-dd");
        }

        private void CarregaGridViewNotaRetiradaItem(int codigoNotaRetirada)
        {
            List<NOTA_RETIRADA_ITEM> listaNotaRetiradaItem = baseController.BuscaNotaRetiradaItens(codigoNotaRetirada);

            if (listaNotaRetiradaItem != null)
            {
                GridViewNotaRetiradaItem.DataSource = listaNotaRetiradaItem;
                GridViewNotaRetiradaItem.DataBind();
            }
        }

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            if (txtData.Text.Equals(""))
                //|| txtArquivo.Text.Equals(""))
                return;

            if (FileUpload1.HasFile)
            {
                try
                {
                    txtDiretorio.Text = System.IO.Path.GetDirectoryName(FileUpload1.PostedFile.FileName) + "\\";

                    txtArquivo.Text = FileUpload1.FileName;
                }
                catch (Exception)
                {
                }
            }

            LerArquivo();

            int totalRegistros = 0;

            NOTA_RETIRADA notaRetirada = new NOTA_RETIRADA();

            notaRetirada.CODIGO_FILIAL = 7;
            notaRetirada.NUMERO_NOTA_CMAX = "0";
            notaRetirada.BAIXADO = false;

            usuarioController.InsereNotaRetirada(notaRetirada);

            int ultimaNotaRetirada = usuarioController.BuscaUltimaNotaRetirada(7);

            int i = 0;

            foreach (ProdutoDestino item in listaProdutoDestino)
            {

                NOTA_RETIRADA_ITEM notaRetiradaItem = new NOTA_RETIRADA_ITEM();

                notaRetiradaItem.CODIGO_PRODUTO = Convert.ToInt32(item.CodigoProduto.Substring(0, 5));

                Sp_Busca_Produto_CorResult produtoCores = baseController.BuscaProdutoCor(item.CodigoProduto.Substring(0, 5), item.CodigoProduto.Substring(5, 3));

                if (produtoCores != null)
                    notaRetiradaItem.COR_PRODUTO = produtoCores.DESC_COR_PRODUTO;

                PRODUTO produto = baseController.BuscaProduto(item.CodigoProduto.Substring(0, 5));

                if (produto != null)
                    notaRetiradaItem.DESCRICAO_PRODUTO = produto.DESC_PRODUTO;

                PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CodigoProduto);

                if (produtoBarra != null)
                    notaRetiradaItem.TAMANHO = produtoBarra.GRADE;

                i++;

                notaRetiradaItem.CODIGO_ORIGEM_DEFEITO = "3";
                notaRetiradaItem.CODIGO_DEFEITO = item.Defeito;
                notaRetiradaItem.DATA_LANCAMENTO = CalendarData.SelectedDate;
                notaRetiradaItem.CODIGO_NOTA_RETIRADA = ultimaNotaRetirada;
                notaRetiradaItem.CODIGO_DESTINO = item.Destino;
                notaRetiradaItem.TIPO_NOTA = baseController.BuscaOrigemProduto(produtoBarra.CODIGO_BARRA);
                notaRetiradaItem.ITEM_PRODUTO = i;

                usuarioController.InsereNotaRetiradaItem(notaRetiradaItem);

                totalRegistros++;
            }

            lblMensagem.Text = totalRegistros + " Gravados com sucesso !!!";

            CarregaGridViewNotaRetiradaItem(ultimaNotaRetirada);
        }

        protected void GridViewNotaRetiradaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            Literal literalItem = e.Row.FindControl("LiteralItem") as Literal;

            if (literalItem != null)
                literalItem.Text = i++.ToString();
        }

        private void LimpaFeedBack()
        {
            lblMensagem.Text = string.Empty;
        }

        private void LerArquivo()
        {
            Stream fileContent = null;

            if (FileUpload1.HasFile)
            {
                try
                {
                    txtDiretorio.Text = System.IO.Path.GetDirectoryName(FileUpload1.PostedFile.FileName) + "\\";

                    txtArquivo.Text = FileUpload1.FileName;
                    fileContent = FileUpload1.FileContent;
                }
                catch (Exception ex)
                {
                }
            }
            else
                Response.Write("<script>alert('Não foi possível encontrar o arquivo!');</script>");

            string line;
            string texto = "";

            // Read the file and display it line by line.
            //System.IO.StreamReader file = new System.IO.StreamReader(txtDiretorio.Text+txtArquivo.Text);
            if (fileContent != null)
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileContent);

                while ((line = file.ReadLine()) != null)
                {
                    if (!line.Trim().Substring(0, 8).Equals("11111111") &
                        !line.Trim().Substring(0, 8).Equals("22222222") &
                        !line.Trim().Substring(0, 8).Equals("33333333"))
                    {
                        ProdutoDestino produtoDestino = new ProdutoDestino();

                        produtoDestino.Destino = texto.Substring(0, 1);
                        produtoDestino.Defeito = texto.Substring(8, 2);
                        produtoDestino.CodigoProduto = line.Substring(0, 10);
                        produtoDestino.FlagBaixado = false;

                        listaProdutoDestino.Add(produtoDestino);
                    }
                    else
                    {
                        texto = line.Trim();
                    }
                }

                file.Close();
            }
            else
                Response.Write("<script>alert('Não foi possível ler o arquivo!');</script>");
        }
    }
}