using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Data.OleDb;
using GemBox.Spreadsheet;
using Relatorios.mod_ecom.mag;

namespace Relatorios
{
    public partial class ecom_produto_preco : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }

            //Evitar duplo clique no botão
            btImportar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImportar, null) + ";");
        }

        protected void btImportar_Click(object sender, EventArgs e)
        {
            int totalLinha = 0;
            try
            {
                labErro.Text = "Carregando arquivo...";

                //Validar arquivo inserido
                if (!uploadArquivo.HasFile)
                {
                    labErro.Text = "Selecione um arquivo .TXT.";
                    return;
                }

                //Validar Tamanho do Arquivo
                if (uploadArquivo.PostedFile.ContentLength < 0)
                {
                    labErro.Text = "Selecione um arquivo com o tamanho maior que zero.";
                    return;
                }

                //Validar Extensão
                string fileExtension = System.IO.Path.GetExtension(uploadArquivo.PostedFile.FileName);
                if (fileExtension.ToUpper() != ".TXT")
                {
                    labErro.Text = "Selecione um arquivo com extensão \".TXT\".";
                    return;
                }

                using (StreamReader reader = new StreamReader(uploadArquivo.PostedFile.InputStream, Encoding.Default))
                {

                    while (!reader.EndOfStream)
                    {
                        var linha = reader.ReadLine();

                        if (linha != null && linha.Trim() != "")
                        {
                            var produto = linha.Substring(0, 5);
                            var preco = Convert.ToDecimal(linha.Substring(5));

                            // procurar produto na intranet
                            if (preco > 0)
                            {
                                var ecomProdutos = ecomController.ObterMagentoTodosProdutos(produto).Where(p => p.ID_PRODUTO_MAG > 0);
                                foreach (var eprod in ecomProdutos)
                                {
                                    Magento mag = new Magento();
                                    var respPrice = mag.AtualizarPrecoPromo(eprod.ID_PRODUTO_MAG.ToString(), preco);
                                    if (respPrice)
                                    {
                                        eprod.PRECO_PROMO = preco;
                                        ecomController.AtualizarMagentoProduto(eprod);
                                    }
                                }
                            }
                        }

                    }
                }

                labErro.Text = "Preços atualizados com sucesso.";
            }
            catch (Exception ex)
            {
                if (totalLinha > 0)
                    labErro.Text = "Erro ao importar arquivo: Linha " + totalLinha.ToString();
                else if (totalLinha == -1)
                    labErro.Text = "Erro ao iniciar a leitura do arquivo.";
                else
                    labErro.Text = ex.Message;

            }
            finally
            {
            }
        }

    }
}
