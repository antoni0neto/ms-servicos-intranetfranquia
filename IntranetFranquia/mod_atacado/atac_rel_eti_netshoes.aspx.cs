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

namespace Relatorios
{
    public partial class atac_rel_eti_netshoes : System.Web.UI.Page
    {
        AtacadoController atacadoController = new AtacadoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }

            //Evitar duplo clique no botão
            btImportar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImportar, null) + ";");
        }

        #region "DADOS INICIAIS"
        #endregion

        protected void btImportar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";

            if (ddlGriffe.SelectedValue == "")
            {
                labErro.Text = "Selecione uma Griffe.";
                return;
            }

            //Validar arquivo inserido
            if (!uploadArquivo.HasFile)
            {
                labErro.Text = "Selecione um arquivo .CSV.";
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
            if (fileExtension.ToUpper() != ".CSV")
            {
                labErro.Text = "Selecione um arquivo com extensão \".CSV\".";
                return;
            }

            var codigoUsuario = 0;
            if (Session["USUARIO"] == null)
            {
                labErro.Text = "Sua sessão expirou. Faça o Login novamente";
                return;
            }
            else
            {
                codigoUsuario = Convert.ToInt32(((USUARIO)(Session["USUARIO"])).CODIGO_USUARIO);
            }

            int totalLinha = 0;
            try
            {
                labErro.Text = "Carregando arquivo...";

                using (StreamReader reader = new StreamReader(uploadArquivo.PostedFile.InputStream, Encoding.Default))
                {
                    //le a primeira linha para retirar do loop
                    var linha = reader.ReadLine();

                    var griffe = ddlGriffe.SelectedValue;

                    while (!reader.EndOfStream)
                    {
                        linha = reader.ReadLine();
                        var valores = linha.Split(';');

                        int qtde = Convert.ToInt32(valores[5].Trim());

                        int i = 0;
                        for (; i < qtde; i++)
                        {
                            var ns = new NETSHOES_ETIQUETA();

                            ns.EAN = valores[0].Trim();
                            ns.EAN12 = valores[0].Trim().Substring(0, 12);
                            ns.SKU = valores[1].Trim();
                            ns.MODELO = valores[2].Trim();
                            ns.COD_FABRICANTE = valores[3].Trim();
                            ns.TAMANHO = valores[4].Trim();
                            ns.GRIFFE = griffe;
                            ns.QTDE = 1;

                            atacadoController.InserirNetshoesEtiqueta(ns);
                        }

                        totalLinha += 1;
                    }
                }

                labErro.Text = "Arquivo importado com sucesso.";

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
