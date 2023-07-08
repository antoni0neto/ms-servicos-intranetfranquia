using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.IO;
using System.Xml;
using System.Web.Services;
using System.Text.RegularExpressions;

namespace Relatorios
{
    public partial class admfis_leitura_xmlnfe_moverv2 : System.Web.UI.Page
    {
        private readonly static BaseController baseController = new BaseController();

        private readonly static string fromPath = Constante.pathFromXML;
        private readonly static string toPath = Constante.pathToXML;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        [WebMethod]
        public static string TransformarArquivos()
        {
            var ffAux = "";

            try
            {
                var fromFiles = Directory.GetFiles(fromPath, "*.xml");
                foreach (var ff in fromFiles)
                {
                    ffAux = ff;

                    var fullFileName = Path.GetFileName(ff);

                    var fileNameChaveNfe = new String(fullFileName.Where(Char.IsDigit).ToArray());

                    var dataEmissao = Convert.ToDateTime("20" + fileNameChaveNfe.Substring(2, 2) + "-" + fileNameChaveNfe.Substring(4, 2) + "-01");

                    var ano = dataEmissao.Year.ToString();
                    var mes = dataEmissao.Month.ToString();
                    var CNPJ = fileNameChaveNfe.Substring(6, 14);

                    if (!Directory.Exists(toPath + CNPJ))
                        Directory.CreateDirectory(toPath + CNPJ);

                    if (!Directory.Exists(toPath + CNPJ + "\\" + ano))
                        Directory.CreateDirectory(toPath + CNPJ + "\\" + ano);

                    if (!Directory.Exists(toPath + CNPJ + "\\" + ano + "\\" + mes))
                        Directory.CreateDirectory(toPath + CNPJ + "\\" + ano + "\\" + mes);

                    var pathTO = toPath + CNPJ + "\\" + ano + "\\" + mes + "\\";

                    // se arquivo existe, exclui o arquivo
                    if (File.Exists(pathTO + fullFileName))
                        File.Delete(pathTO + fullFileName);

                    //mover o arquivo
                    File.Move(ff, pathTO + fullFileName);

                }

            }
            catch (Exception ex)
            {
                return "Arquivo: " + ffAux + " Mensagem: " + ex.Message + " - Stack: " + ex.StackTrace;
            }

            return "Arquivos Movidos com sucesso.";
        }


    }
}