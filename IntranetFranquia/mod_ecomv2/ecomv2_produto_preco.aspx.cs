using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using System.IO;
using DAL;
using System.Text;
using Relatorios.mod_ecom.mag;
using Relatorios.mod_ecomv2.mag2;

namespace Relatorios
{
    public partial class ecomv2_produto_preco : System.Web.UI.Page
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
                    var mag = new MagentoV2();

                    while (!reader.EndOfStream)
                    {
                        var linha = reader.ReadLine();

                        if (!string.IsNullOrWhiteSpace(linha))
                        {
                            var produto = linha.Substring(0, 5);
                            var preco = Convert.ToDecimal(linha.Substring(5));
                            if (preco > 0)
                            {
                                var ecomProdutos = ecomController.ObterMagentoTodosProdutos(produto).Where(p => p.ID_PRODUTO_MAG > 0);
                                if (ecomProdutos.Any())
                                {
                                    var respPrice = mag.AtualizarPrecoPromo(ecomProdutos.Where(p => p.ID_PRODUTO_MAG_CONFIG != 0).Select(x => x.SKU).ToList(), preco, 0);
                                    if (respPrice)
                                    {
                                        foreach (var eprod in ecomProdutos)
                                        {
                                            eprod.PRECO_PROMO = preco;
                                            ecomController.AtualizarMagentoProduto(eprod);
                                        }
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
