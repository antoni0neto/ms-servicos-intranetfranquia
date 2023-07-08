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
    public partial class AtualizaDefeitoDestino : System.Web.UI.Page
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
                catch (Exception ex)
                {
                }
            }

            LerArquivo();
            
            int totalRegistros = 0;

            if (Request.QueryString["CodigoNotaRetirada"] == null)
                return;

            List<NOTA_RETIRADA_ITEM> listaNotaRetiradaItem = baseController.BuscaNotaRetiradaItens(Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"].ToString()));

            if (listaNotaRetiradaItem != null)
            {
                string retorno = "";

                foreach (NOTA_RETIRADA_ITEM item in listaNotaRetiradaItem)
                {
                    if (item.DATA_DESTINO == null)
                    {
                        PRODUTO_CORE produtoCor = baseController.BuscaProdutoCores(item.CODIGO_PRODUTO.ToString().Trim(), item.COR_PRODUTO.Trim());

                        if (produtoCor != null)
                        {
                            PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarra(item.CODIGO_PRODUTO.ToString().Trim(), produtoCor.COR_PRODUTO.Trim(), item.TAMANHO.Trim());

                            if (produtoBarra != null)
                            {
                                retorno = AtualizaListaProduto(produtoBarra.CODIGO_BARRA.Trim(), item.CODIGO_DESTINO);

                                if (!retorno.Equals(""))
                                {
                                    usuarioController.AtualizaNotaRetiradaItem(item.CODIGO_NOTA_RETIRADA_ITEM, CalendarData.SelectedDate, retorno.Substring(0, 1));

                                    totalRegistros++;
                                }
                            }
                        }
                    }
                }

                lblMensagem.Text = totalRegistros + " Gravados com sucesso !!!";

                CarregaGridViewNotaRetiradaItem(Convert.ToInt32(Request.QueryString["CodigoNotaRetirada"].ToString()));
            }
        }
        
        private string AtualizaListaProduto(string produto, string destino)
        {
            string produtoDefeito = "";

            foreach (ProdutoDestino item in listaProdutoDestino)
            {
                if (item.CodigoProduto.Equals(produto) & item.Destino.Equals(destino))
                {
                    if (!item.FlagBaixado)
                    {
                        item.FlagBaixado = true;

                        produtoDefeito = item.Destino;

                        break;
                    }
                }
            }

            return produtoDefeito;
        }
        
        protected void GridViewNotaRetiradaItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NOTA_RETIRADA_ITEM notaRetiradaItem = e.Row.DataItem as NOTA_RETIRADA_ITEM;

            Literal literalItem = e.Row.FindControl("LiteralItem") as Literal;

            if (literalItem != null)
                literalItem.Text = i++.ToString();
        }

        private void CarregaGridViewNotaRetiradaItens(int codigoNotaRetirada)
        {
            GridViewNotaRetiradaItem.DataSource = usuarioController.BuscaNotaRetiradaItens(codigoNotaRetirada);
            GridViewNotaRetiradaItem.DataBind();
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