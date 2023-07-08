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
    public partial class AtualizaRetorno : System.Web.UI.Page
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

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            if (txtData.Text.Equals(""))
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

            if (txtDiretorio.Text.Equals("") ||
                txtArquivo.Text.Equals(""))
                return;

            LerArquivo();
            
            int i = 0;

            List<NOTA_RETIRADA_ITEM> gridNotaRetiradaItem = new List<NOTA_RETIRADA_ITEM>();

            List<NOTA_RETIRADA_ITEM> listaNotaRetiradaItem = baseController.BuscaNotaRetiradaItensSemRetorno();

            if (listaNotaRetiradaItem != null)
            {
                foreach (NOTA_RETIRADA_ITEM itemNota in listaNotaRetiradaItem)
                {
                    string codigo = "";

                    PRODUTO_CORE produtoCores = baseController.BuscaProdutoCores(itemNota.CODIGO_PRODUTO.ToString(), itemNota.COR_PRODUTO.Trim());

                    if (produtoCores != null)
                    {
                        codigo = itemNota.CODIGO_PRODUTO.ToString() + produtoCores.COR_PRODUTO.Trim();

                        PRODUTOS_BARRA produtoBarra = baseController.BuscaProdutoBarraGrade(itemNota.CODIGO_PRODUTO.ToString(), produtoCores.COR_PRODUTO.Trim(), itemNota.TAMANHO);

                        if (produtoBarra != null)
                        {
                            if (produtoBarra.TAMANHO.ToString().Length == 1)
                                codigo = codigo + "0" + produtoBarra.TAMANHO.ToString().Trim();
                            else
                                codigo = codigo + produtoBarra.TAMANHO.ToString().Trim();

                            PRODUTO produto = baseController.BuscaProduto(itemNota.CODIGO_PRODUTO.ToString());

                            if (produto != null)
                            {
                                foreach (ProdutoDestino item in listaProdutoDestino)
                                {
                                    if (codigo.Equals(item.CodigoProduto) & !item.FlagBaixado)
                                    {
                                        item.FlagBaixado = true;

                                        i++;

                                        itemNota.DATA_RETORNO = CalendarData.SelectedDate;

                                        usuarioController.AtualizaNotaRetiradaItem(itemNota);

                                        NOTA_DEFEITO_RETORNO notaDefeitoRetorno = new NOTA_DEFEITO_RETORNO();

                                        notaDefeitoRetorno.CODIGO_BARRA_PRODUTO = produtoBarra.CODIGO_BARRA;
                                        notaDefeitoRetorno.CODIGO_DESTINO = Convert.ToInt32(itemNota.CODIGO_DESTINO);
                                        notaDefeitoRetorno.CODIGO_STATUS_RETORNO = 1;
                                        notaDefeitoRetorno.DATA_LANCAMENTO = DateTime.Now;
                                        notaDefeitoRetorno.GRIFFE = produto.GRIFFE;
                                        notaDefeitoRetorno.GRUPO = produto.GRUPO_PRODUTO;
                                        notaDefeitoRetorno.FLAG_BAIXADO = false;

                                        usuarioController.InsereNotaDefeitoRetorno(notaDefeitoRetorno);

                                        gridNotaRetiradaItem.Add(itemNota);

                                        break;
                                    }
                                }

                                lblMensagem.Text = i + " Atualizados com Sucesso !!!";

                                if (gridNotaRetiradaItem != null)
                                {
                                    GridViewNotaRetiradaItem.DataSource = gridNotaRetiradaItem;
                                    GridViewNotaRetiradaItem.DataBind();
                                }
                            }
                        }
                    }
                }
            }
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
                        if (texto.Trim().Substring(0, 8).Equals("11111111"))
                        {
                            ProdutoDestino produtoDestino = new ProdutoDestino();

                            produtoDestino.Destino = texto.Substring(0, 1);
                            produtoDestino.Defeito = texto.Substring(8, 2);
                            produtoDestino.CodigoProduto = line.Substring(0, 10);
                            produtoDestino.FlagBaixado = false;

                            listaProdutoDestino.Add(produtoDestino);
                        }
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