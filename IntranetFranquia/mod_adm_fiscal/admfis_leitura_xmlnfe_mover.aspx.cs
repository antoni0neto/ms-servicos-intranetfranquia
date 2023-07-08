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

namespace Relatorios
{
    public partial class admfis_leitura_xmlnfe_mover : System.Web.UI.Page
    {
        private readonly static BaseController baseController = new BaseController();

        private readonly static string fromPath = @"D:\Intranet\XMLNFe\";
        private readonly static string toPath = @"\\HBFSTORAGE\handbook\Backup_XML\";
        private readonly static string toErrorPath = @"D:\Intranet\XMLNFe\Erros\";

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

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(ff);

                    //Verifica evento nota OK
                    XmlNodeList nfeProc = xmlDocument.GetElementsByTagName("nfeProc");
                    if (nfeProc != null && nfeProc.Count > 0)
                    {
                        var newPath = GetNFeOk(nfeProc);
                        if (newPath != "")
                            MoveFile(ff, newPath);
                        else
                            MoveFile(ff, "");
                    }
                    else
                    {
                        //Nota Inutilizada
                        XmlNodeList procInutNFe = xmlDocument.GetElementsByTagName("ProcInutNFe");
                        if (procInutNFe != null && procInutNFe.Count > 0)
                        {
                            var newPath = GetNFeInut(procInutNFe);
                            if (newPath != "")
                                MoveFile(ff, newPath);
                            else
                                MoveFile(ff, "");
                        }
                        else
                        {
                            //Cartao Correcao/Cancelamento
                            XmlNodeList procEventoNFe = xmlDocument.GetElementsByTagName("procEventoNFe");
                            if (procEventoNFe != null && procEventoNFe.Count > 0)
                            {
                                var newPath = GetNFeCancCC(procEventoNFe);
                                if (newPath != "")
                                    MoveFile(ff, newPath);
                                else
                                    MoveFile(ff, "");
                            }
                            else
                            {

                                //Cancelamento V2
                                XmlNodeList procCancNFe = xmlDocument.GetElementsByTagName("procCancNFe");
                                if (procCancNFe != null && procCancNFe.Count > 0)
                                {
                                    var newPath = GetNFeCancCCV2(procCancNFe);
                                    if (newPath != "")
                                        MoveFile(ff, newPath);
                                    else
                                        MoveFile(ff, "");
                                }
                                else
                                {
                                    MoveFile(ff, "");
                                }

                            }
                        }
                    }

                    ffAux = "";
                }

            }
            catch (Exception ex)
            {
                if (ffAux != "")
                    MoveFile(ffAux, "");
                //return "Message: " + ex.Message + " - Stack: " + ex.StackTrace;
            }

            return "Arquivos Movidos com sucesso. Verifique a pasta de erros.";
        }

        private static void MoveFile(string file, string newPath)
        {
            //obter nome do arquivo
            string fileName = Path.GetFileName(file);
            // mover para diretorios
            if (newPath == "") //erro
            {
                File.Move(file, toErrorPath + fileName);
            }
            else
            {
                File.Move(file, newPath + fileName);
            }
        }

        private static string GetNFeOk(XmlNodeList xmlnfeProc)
        {
            var nfe = "";
            var serie = "";
            var emissao = "";
            var CNPJ = "";
            var chaveNfe = "";

            XmlNodeList xmlNFe = ((XmlElement)xmlnfeProc[0]).GetElementsByTagName("NFe");
            XmlNodeList xmlinfNFe = ((XmlElement)xmlNFe[0]).GetElementsByTagName("infNFe");
            XmlNodeList xmlide = ((XmlElement)xmlinfNFe[0]).GetElementsByTagName("ide");
            // Info da nota
            foreach (XmlElement nodo in xmlide)
            {
                XmlNodeList xmlnNF = nodo.GetElementsByTagName("nNF");
                nfe = xmlnNF[0].InnerText;
                XmlNodeList xmlserie = nodo.GetElementsByTagName("serie");
                serie = xmlserie[0].InnerText;
                XmlNodeList xmldhEmi = nodo.GetElementsByTagName("dhEmi");
                if (xmldhEmi.Count > 0)
                {
                    emissao = xmldhEmi[0].InnerText;
                }
                else
                {
                    XmlNodeList xmldEmi = nodo.GetElementsByTagName("dEmi");
                    emissao = xmldEmi[0].InnerText;
                }
            }

            XmlNodeList xmlemit = ((XmlElement)xmlinfNFe[0]).GetElementsByTagName("emit");
            foreach (XmlElement nodo in xmlemit)
            {
                XmlNodeList xmlCNPJ = nodo.GetElementsByTagName("CNPJ");
                CNPJ = xmlCNPJ[0].InnerText;
            }

            XmlNodeList xmlprotNFe = ((XmlElement)xmlnfeProc[0]).GetElementsByTagName("protNFe");
            XmlNodeList xmlinfProt = ((XmlElement)xmlprotNFe[0]).GetElementsByTagName("infProt");
            foreach (XmlElement nodo in xmlinfProt)
            {
                XmlNodeList xmlchNFe = nodo.GetElementsByTagName("chNFe");
                chaveNfe = xmlchNFe[0].InnerText;
            }

            DateTime dataEmissao = DateTime.Today;
            if (emissao == "" || !DateTime.TryParse(emissao, out dataEmissao))
                return "";

            string ano = dataEmissao.Year.ToString();
            string mes = dataEmissao.Month.ToString();

            if (!Directory.Exists(toPath + CNPJ))
                Directory.CreateDirectory(toPath + CNPJ);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano + "\\" + mes))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano + "\\" + mes);

            return toPath + CNPJ + "\\" + ano + "\\" + mes + "\\";
        }
        private static string GetNFeInut(XmlNodeList xmlprocInutNFe)
        {
            var nfe = "";
            var serie = "";
            var CNPJ = "";
            var protocoloNfe = "";
            var recebimento = "";

            XmlNodeList xmlinutNFe = ((XmlElement)xmlprocInutNFe[0]).GetElementsByTagName("inutNFe");
            XmlNodeList xmlinfInut = ((XmlElement)xmlinutNFe[0]).GetElementsByTagName("infInut");
            // Info da nota
            foreach (XmlElement nodo in xmlinfInut)
            {
                XmlNodeList xmlnNFIni = nodo.GetElementsByTagName("nNFIni");
                nfe = xmlnNFIni[0].InnerText;
                XmlNodeList xmlserie = nodo.GetElementsByTagName("serie");
                serie = xmlserie[0].InnerText;
                XmlNodeList xmlCNPJ = nodo.GetElementsByTagName("CNPJ");
                CNPJ = xmlCNPJ[0].InnerText;
            }

            XmlNodeList xmlretInutNFe = ((XmlElement)xmlprocInutNFe[0]).GetElementsByTagName("retInutNFe");
            xmlinfInut = ((XmlElement)xmlretInutNFe[0]).GetElementsByTagName("infInut");
            foreach (XmlElement nodo in xmlinfInut)
            {
                XmlNodeList xmlnProt = nodo.GetElementsByTagName("nProt");
                protocoloNfe = xmlnProt[0].InnerText;

                XmlNodeList xmldhRecbto = nodo.GetElementsByTagName("dhRecbto");
                recebimento = xmldhRecbto[0].InnerText;
            }

            DateTime dataRecebimento = DateTime.Today;
            if (recebimento == "" || !DateTime.TryParse(recebimento, out dataRecebimento))
                return "";

            string ano = dataRecebimento.Year.ToString();
            string mes = dataRecebimento.Month.ToString();

            if (!Directory.Exists(toPath + CNPJ))
                Directory.CreateDirectory(toPath + CNPJ);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano + "\\" + mes))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano + "\\" + mes);

            return toPath + CNPJ + "\\" + ano + "\\" + mes + "\\";

        }
        private static string GetNFeCancCC(XmlNodeList xmlprocEventoNFe)
        {
            var CNPJ = "";
            var chaveNFe = "";
            var tpEvento = "";
            var evento = "";

            XmlNodeList xmlevento = ((XmlElement)xmlprocEventoNFe[0]).GetElementsByTagName("evento");
            XmlNodeList xmlinfEvento = ((XmlElement)xmlevento[0]).GetElementsByTagName("infEvento");
            // Info da nota
            foreach (XmlElement nodo in xmlinfEvento)
            {
                XmlNodeList xmlCNPJ = nodo.GetElementsByTagName("CNPJ");
                CNPJ = xmlCNPJ[0].InnerText;
                XmlNodeList xmlchNFe = nodo.GetElementsByTagName("chNFe");
                chaveNFe = xmlchNFe[0].InnerText;
                XmlNodeList xmltpEvento = nodo.GetElementsByTagName("tpEvento");
                tpEvento = xmltpEvento[0].InnerText;
                XmlNodeList xmldhEvento = nodo.GetElementsByTagName("dhEvento");
                evento = xmldhEvento[0].InnerText;
            }

            //obter protocolo

            if (tpEvento != "110110" && tpEvento != "110111")
                return "";

            DateTime dataEvento = DateTime.Today;
            if (evento == "" || !DateTime.TryParse(evento, out dataEvento))
                return "";

            string ano = dataEvento.Year.ToString();
            string mes = dataEvento.Month.ToString();

            if (!Directory.Exists(toPath + CNPJ))
                Directory.CreateDirectory(toPath + CNPJ);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano + "\\" + mes))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano + "\\" + mes);

            return toPath + CNPJ + "\\" + ano + "\\" + mes + "\\";

        }
        private static string GetNFeCancCCV2(XmlNodeList xmlprocCancNFe)
        {
            var CNPJ = "";
            var chaveNFe = "";
            var tpEvento = "";
            var evento = "";

            XmlNodeList xmlevento = ((XmlElement)xmlprocCancNFe[0]).GetElementsByTagName("evento");
            XmlNodeList xmlinfEvento = ((XmlElement)xmlevento[0]).GetElementsByTagName("infEvento");
            // Info da nota
            foreach (XmlElement nodo in xmlinfEvento)
            {
                XmlNodeList xmlCNPJ = nodo.GetElementsByTagName("CNPJ");
                CNPJ = xmlCNPJ[0].InnerText;
                XmlNodeList xmlchNFe = nodo.GetElementsByTagName("chNFe");
                chaveNFe = xmlchNFe[0].InnerText;
                XmlNodeList xmltpEvento = nodo.GetElementsByTagName("tpEvento");
                tpEvento = xmltpEvento[0].InnerText;
                XmlNodeList xmldhEvento = nodo.GetElementsByTagName("dhEvento");
                evento = xmldhEvento[0].InnerText;
            }

            //obter protocolo

            if (tpEvento != "110110" && tpEvento != "110111")
                return "";

            DateTime dataEvento = DateTime.Today;
            if (evento == "" || !DateTime.TryParse(evento, out dataEvento))
                return "";

            string ano = dataEvento.Year.ToString();
            string mes = dataEvento.Month.ToString();

            if (!Directory.Exists(toPath + CNPJ))
                Directory.CreateDirectory(toPath + CNPJ);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano);

            if (!Directory.Exists(toPath + CNPJ + "\\" + ano + "\\" + mes))
                Directory.CreateDirectory(toPath + CNPJ + "\\" + ano + "\\" + mes);

            return toPath + CNPJ + "\\" + ano + "\\" + mes + "\\";

        }

    }
}