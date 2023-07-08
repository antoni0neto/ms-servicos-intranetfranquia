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
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class desenv_pocket_cad : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        int coluna = 0, colunaModelo = 0;
        bool _erro = false;
        int _linha = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                divModelo.Visible = false;
                pnlModelo.Visible = false;
            }
        }

        protected void btImportar_Click(object sender, EventArgs e)
        {
            DataSet _dsProduto = new DataSet();
            string fileExtension = "";
            string foto = "";

            labErro.Text = "";
            //Validar Layout excel
            if (!upPocketExcel.HasFile)
            {
                labErro.Text = "Selecione o MODELO EXCEL.";
                return;
            }
            //Validar Tamanho do Arquivo
            if (upPocketExcel.PostedFile.ContentLength < 0)
            {
                labErro.Text = "Selecione um MODELO EXCEL com o tamanho maior que zero.";
                return;
            }
            //Validar Extensão
            fileExtension = System.IO.Path.GetExtension(upPocketExcel.PostedFile.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                labErro.Text = "Selecione um MODELO EXCEL com extensão \".xls\" ou \".xlsx\".";
                return;
            }

            //Validar foto do modelo
            if (!upPocketFoto.HasFile)
            {
                labErro.Text = "Selecione a FOTO do modelo.";
                return;
            }
            //Validar Tamanho do Arquivo
            if (upPocketFoto.PostedFile.ContentLength < 0)
            {
                labErro.Text = "Selecione uma FOTO DO MODELO com o tamanho maior que zero.";
                return;
            }
            //Validar Extensão
            fileExtension = System.IO.Path.GetExtension(upPocketFoto.PostedFile.FileName);
            if (fileExtension != ".jpg" && fileExtension != ".png")
            {
                labErro.Text = "Selecione uma FOTO DO MODELO com extensão \".jpg\" ou \".png\".";
                return;
            }

            foto = GravarImagem();
            if (foto.Contains("ERRO"))
                throw new Exception(foto);

            List<ERRO> _lstErro = new List<ERRO>();
            DESENV_PRODUTO _produto = null;
            List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();
            DESENV_PRODUTO_ORIGEM _origem = null;
            DESENV_CORE _cor = null;

            try
            {
                //Obter dataset com os dados da planilha
                _dsProduto = CarregarDados();

                int codigoRef = 0;
                var produtos = desenvController.ObterProduto().Where(p => p.COLECAO == _dsProduto.Tables[0].Rows[0][0].ToString());
                if (produtos != null && produtos.Count() > 0)
                    codigoRef = produtos.Max(i => i.CODIGO_REF);

                _erro = false;
                foreach (DataRow r in _dsProduto.Tables[0].Rows)
                {
                    _linha += 1;

                    _origem = desenvController.ObterProdutoOrigem(r["COLECAO"].ToString().Trim()).Where(p => p.DESCRICAO.Trim() == r["ORIGEM"].ToString().Trim().ToUpper() && p.STATUS == 'A').SingleOrDefault();
                    _cor = desenvController.ObterCorPocket(r["COR"].ToString());

                    if (r["COLECAO"].ToString().Trim() != "")
                    {
                        if (baseController.BuscaColecaoAtual(r["COLECAO"].ToString()) == null)
                        {
                            _lstErro.Add(new ERRO
                            {
                                LINHA = _linha.ToString(),
                                COLECAO = r["COLECAO"].ToString(),
                                ORIGEM = r["ORIGEM"].ToString(),
                                GRUPO = r["GRUPO"].ToString(),
                                MODELO = r["MODELO"].ToString(),
                                COR = r["COR"].ToString(),
                                OBS = "COL NÃO EXISTE. INFORME UMA COL EXISTENTE."
                            });
                            _erro = true;
                        }
                        else if (baseController.BuscaGrupoProduto(r["GRUPO"].ToString()) == null)
                        {
                            _lstErro.Add(new ERRO
                            {
                                LINHA = _linha.ToString(),
                                COLECAO = r["COLECAO"].ToString(),
                                ORIGEM = r["ORIGEM"].ToString(),
                                GRUPO = r["GRUPO"].ToString(),
                                MODELO = r["MODELO"].ToString(),
                                COR = r["COR"].ToString(),
                                OBS = "GRUPO NÃO ENCONTRADO. INFORME UM GRUPO."
                            });
                            _erro = true;
                        }
                        else if (_origem == null)
                        {
                            _lstErro.Add(new ERRO
                            {
                                LINHA = _linha.ToString(),
                                COLECAO = r["COLECAO"].ToString(),
                                ORIGEM = r["ORIGEM"].ToString(),
                                GRUPO = r["GRUPO"].ToString(),
                                MODELO = r["MODELO"].ToString(),
                                COR = r["COR"].ToString(),
                                OBS = "ORIGEM NÃO ENCONTRADA. INFORME UMA ORIGEM."
                            });
                            _erro = true;
                        }
                        else if (_cor == null)
                        {
                            _lstErro.Add(new ERRO
                            {
                                LINHA = _linha.ToString(),
                                COLECAO = r["COLECAO"].ToString(),
                                ORIGEM = r["ORIGEM"].ToString(),
                                GRUPO = r["GRUPO"].ToString(),
                                MODELO = r["MODELO"].ToString(),
                                COR = r["COR"].ToString(),
                                OBS = "COR NÃO ENCONTRADA. INFORME UMA COR EXISTENTE."
                            });
                            _erro = true;
                        }

                        if (!_erro)
                        {
                            int codigo = 0;
                            if (r["CODIGO"].ToString().Trim() != "")
                                codigo = desenvController.ObterProdutoCODRef(Convert.ToInt32(r["CODIGO"].ToString()), r["COLECAO"].ToString());

                            _produto = new DESENV_PRODUTO();
                            if (codigo > 0)
                                _produto.CODIGO = codigo;
                            _produto.COLECAO = r["COLECAO"].ToString().Trim();
                            _produto.DESENV_PRODUTO_ORIGEM = _origem.CODIGO;
                            _produto.GRUPO = r["GRUPO"].ToString().Trim().ToUpper();
                            _produto.MODELO = r["MODELO"].ToString().Trim().ToUpper();
                            _produto.COR = r["COR"].ToString().Trim().ToUpper();
                            _produto.TECIDO_POCKET = r["TECIDO"].ToString().Trim().ToUpper();
                            _produto.OBSERVACAO = r["OBS"].ToString().Trim().ToUpper();
                            _produto.FOTO = foto;

                            if (codigo > 0)
                            {
                                _produto.CODIGO_REF = Convert.ToInt32(r["CODIGO"].ToString().Trim());
                            }
                            else
                            {
                                codigoRef = codigoRef + 1;
                                _produto.CODIGO_REF = codigoRef;
                            }

                            if (r["VAREJO"].ToString() != "")
                                _produto.QTDE = Convert.ToInt32(r["VAREJO"].ToString());
                            else
                                _produto.QTDE = 0;
                            if (r["ATACADO"].ToString() != "")
                                _produto.QTDE_ATACADO = Convert.ToInt32(r["ATACADO"].ToString());
                            else
                                _produto.QTDE_ATACADO = 0;

                            if (r["PRECO_VAREJO"].ToString() != "")
                                _produto.PRECO = Convert.ToDecimal(r["PRECO_VAREJO"].ToString().Replace("R$", "").Trim());
                            else
                                _produto.PRECO = 0;
                            if (r["PRECO_ATACADO"].ToString() != "")
                                _produto.PRECO_ATACADO = Convert.ToDecimal(r["PRECO_ATACADO"].ToString().Replace("R$", "").Trim());
                            else
                                _produto.PRECO_ATACADO = 0;
                            _produto.DATA_INCLUSAO = DateTime.Now;
                            _produto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            _produto.STATUS = 'A';

                            listaProdutos.Add(_produto);
                        }
                    }
                }

                if (!_erro)
                {
                    if (desenvController.InserirAtualizarProduto(listaProdutos))
                    {
                        divPocket.InnerHtml = Pocket.MontarPocket(listaProdutos, false).ToString();
                        gvModelo.DataSource = listaProdutos;
                        gvModelo.DataBind();
                        divErro.Visible = false;
                        pnlModelo.Visible = true;
                        divModelo.Visible = true;
                        pnlImportacao.Visible = false;
                    }
                    else
                    {
                        throw new Exception("ERRO AO INSERIR MODELOS. ENTRE EM CONTATO COM A ÁREA DE TI.");
                    }
                }
                else
                    Carregar_gvErro(_lstErro);
            }
            catch (Exception ex)
            {
                labErro.Text = "(btImportar_Click): Exception \\n " + ex.Message;
            }
            finally
            {

            }
        }
        protected void btNovo_Click(object sender, EventArgs e)
        {
            pnlImportacao.Visible = true;
            pnlModelo.Visible = false;
        }

        private string GravarImagem()
        {
            string _path = string.Empty;
            string _fileName = string.Empty;
            string _ext = string.Empty;
            Stream _stream = null;
            string retornoImagem = "";

            try
            {
                //Obter variaveis do arquivo
                _ext = System.IO.Path.GetExtension(upPocketFoto.PostedFile.FileName);
                _fileName = Guid.NewGuid() + "_FOTO" + _ext;
                _path = Server.MapPath("~/Image_POCKET/") + _fileName;

                //Obter stream da imagem
                _stream = upPocketFoto.PostedFile.InputStream;
                if (_stream != null)
                    GenerateThumbnails(1, _stream, _path);

                retornoImagem = "~/Image_POCKET/" + _fileName;
            }
            catch (Exception ex)
            {
                retornoImagem = "ERRO" + ex.Message;
            }

            return retornoImagem;
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                // width 300
                // height 320
                var newWidth = 0;
                var newHeight = 0;

                newWidth = image.Width;
                newHeight = image.Height;
                while (newWidth > 300 || newHeight > 320)
                {
                    newWidth = (int)((newWidth) * 0.95);
                    newHeight = (int)((newHeight) * 0.95);
                }

                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        protected void gvErro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ERRO _erro = e.Row.DataItem as ERRO;

                    coluna += 1;
                    if (_erro != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();
                    }
                }
            }
        }
        private void Carregar_gvErro(List<ERRO> _erro)
        {
            if (_erro != null)
            {
                gvErro.DataSource = _erro;
                gvErro.DataBind();
            }
        }

        protected void gvModelo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    colunaModelo += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaModelo.ToString();
                    }
                }
            }
        }

        #region "DADOS INICIAIS"
        [Serializable]
        public class ERRO
        {
            public string LINHA { get; set; }
            public string ORIGEM { get; set; }
            public string COLECAO { get; set; }
            public string GRUPO { get; set; }
            public string MODELO { get; set; }
            public string COR { get; set; }
            public string OBS { get; set; }
        }
        public DataSet CarregarDados()
        {
            DataSet ds = new DataSet();
            OleDbConnection excelConnection = null;
            OleDbConnection excelConnection1 = null;

            try
            {
                //Verifica se existe arquivo
                string fileLocation = Server.MapPath("~/Excel/") + upPocketExcel.PostedFile.FileName;

                if (!System.IO.Directory.Exists(Server.MapPath("~/Excel/")))
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Excel/"));

                if (System.IO.File.Exists(fileLocation))
                    System.IO.File.Delete(fileLocation);

                //Salvar arquivo
                upPocketExcel.PostedFile.SaveAs(fileLocation);

                //Obter extensão do arquivo
                string fileExtension = System.IO.Path.GetExtension(upPocketExcel.PostedFile.FileName);
                string excelConnectionString = string.Empty;
                if (fileExtension == ".xls")
                {
                    //excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=YES\"";
                }
                else if (fileExtension == ".xlsx")
                {
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
                }

                //Criar conexão com excel
                excelConnection = new OleDbConnection(excelConnectionString);
                excelConnection.Open();
                DataTable dt = new DataTable();

                //Valida workbook do excel
                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                    return null;

                //Obter total de planilhas
                String[] excelSheets = new String[dt.Rows.Count];
                int t = 0;
                //excel data saves in temp file here.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[t] = row["TABLE_NAME"].ToString();
                    t++;
                }

                //Abrir conexão para buscar a primeira planilha
                excelConnection1 = new OleDbConnection(excelConnectionString);

                string query = " SELECT * FROM [{0}] " +
                                " WHERE " +
                                " COLECAO IS NOT NULL AND COLECAO <> ''''";

                string selectCommand = string.Format(query, excelSheets[1]);
                //Preencher DATASET com retorno da query
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(selectCommand, excelConnection1))
                    dataAdapter.Fill(ds);

                if (ds.Tables[0].Columns.Count != 12)
                    throw new Exception("Layout do arquivo excel incorreto.");

                return ds;
            }
            catch (Exception ex)
            {
                labErroOleDB.Text = ex.Message;
                throw ex;
            }
            finally
            {
                //Fecha primeira conexão
                if (excelConnection.State == ConnectionState.Open)
                    excelConnection.Close();
                //Fecha segunda conexão
                if (excelConnection1.State == ConnectionState.Open)
                    excelConnection1.Close();
            }
        }
        #endregion
    }
}
