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

namespace Relatorios
{
    public partial class desenv_produto_cad : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { }
        }

        protected void btImportar_Click(object sender, EventArgs e)
        {
            DataSet _dsProduto = new DataSet();

            labErro.Text = "";
            //Validar arquivo inserido
            if (!upTecidoExcel.HasFile)
            {
                labErro.Text = "Selecione um arquivo Excel.";
                return;
            }

            //Validar Tamanho do Arquivo
            if (upTecidoExcel.PostedFile.ContentLength < 0)
            {
                labErro.Text = "Selecione um arquivo com o tamanho maior que zero.";
                return;
            }

            //Validar Extensão
            string fileExtension = System.IO.Path.GetExtension(upTecidoExcel.PostedFile.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                labErro.Text = "Selecione um arquivo com extensão \".xls\" ou \".xlsx\".";
                return;
            }

            List<ERRO> _lstErro = new List<ERRO>();
            DESENV_PRODUTO _produto = null;
            DESENV_PRODUTO_ORIGEM _origem = null;
            bool _erro = false;
            int _linha = 1;
            bool _insert = true;
            try
            {
                labErro.Text = "Carregando planejamento...";

                //Obter dataset com os dados da planilha
                _dsProduto = CarregarDados();
                foreach (DataRow r in _dsProduto.Tables[0].Rows)
                {
                    _linha += 1;
                    _erro = false;
                    _insert = true;

                    _origem = desenvController.ObterProdutoOrigem(r["COLECAO"].ToString().Trim()).Where(p => p.DESCRICAO.Trim() == r["ORIGEM"].ToString().Trim().ToUpper() && p.STATUS == 'A').SingleOrDefault();

                    if (r["CODIGO"].ToString() == "")
                    {
                        _lstErro.Add(new ERRO
                        {
                            LINHA = _linha.ToString(),
                            CODREF = r["CODIGO"].ToString(),
                            ORIGEM = r["ORIGEM"].ToString(),
                            COLECAO = r["COLECAO"].ToString(),
                            GRUPO = r["GRUPO"].ToString(),
                            MODELO = r["MODELO"].ToString(),
                            OBS = "CÓDIGO REF NÃO INFORMADO. INFORME UM CÓDIGO."
                        });
                        _erro = true;
                    }
                    else if (baseController.BuscaColecaoAtual(r["COLECAO"].ToString()) == null)
                    {
                        _lstErro.Add(new ERRO
                        {
                            LINHA = _linha.ToString(),
                            CODREF = r["CODIGO"].ToString(),
                            ORIGEM = r["ORIGEM"].ToString(),
                            COLECAO = r["COLECAO"].ToString(),
                            GRUPO = r["GRUPO"].ToString(),
                            MODELO = r["MODELO"].ToString(),
                            OBS = "COL NÃO EXISTE. INFORME UMA COL EXISTENTE."
                        });
                        _erro = true;
                    }
                    else if (baseController.BuscaGrupoProduto(r["GRUPO"].ToString()) == null)
                    {
                        _lstErro.Add(new ERRO
                        {
                            LINHA = _linha.ToString(),
                            CODREF = r["CODIGO"].ToString(),
                            ORIGEM = r["ORIGEM"].ToString(),
                            COLECAO = r["COLECAO"].ToString(),
                            GRUPO = r["GRUPO"].ToString(),
                            MODELO = r["MODELO"].ToString(),
                            OBS = "GRUPO NÃO ENCONTRADO. INFORME UM GRUPO."
                        });
                        _erro = true;
                    }
                    else if (_origem == null)
                    {
                        _lstErro.Add(new ERRO
                        {
                            LINHA = _linha.ToString(),
                            CODREF = r["CODIGO"].ToString(),
                            ORIGEM = r["ORIGEM"].ToString(),
                            COLECAO = r["COLECAO"].ToString(),
                            GRUPO = r["GRUPO"].ToString(),
                            MODELO = r["MODELO"].ToString(),
                            OBS = "ORIGEM NÃO ENCONTRADA. INFORME UMA ORIGEM."
                        });
                        _erro = true;
                    }

                    if (!_erro)
                    {
                        int codigo = 0;
                        codigo = desenvController.ObterProdutoCODRef(Convert.ToInt32(r["CODIGO"].ToString()), r["COLECAO"].ToString());
                        if (codigo > 0)
                            _insert = false;

                        _produto = new DESENV_PRODUTO();
                        _produto.CODIGO_REF = Convert.ToInt32(r["CODIGO"].ToString());
                        _produto.COLECAO = r["COLECAO"].ToString().Trim();
                        _produto.DESENV_PRODUTO_ORIGEM = _origem.CODIGO;
                        _produto.GRUPO = r["GRUPO"].ToString().Trim().ToUpper();
                        _produto.MODELO = r["MODELO"].ToString().Trim().ToUpper();
                        _produto.COR = r["COR"].ToString().Trim().ToUpper();
                        if (r["VAREJO"].ToString() != "")
                            _produto.QTDE = Convert.ToInt32(r["VAREJO"].ToString());
                        else
                            _produto.QTDE = 0;
                        if (r["PRECO"].ToString() != "")
                            _produto.PRECO = Convert.ToDecimal(r["PRECO"].ToString());
                        else
                            _produto.PRECO = 0;
                        _produto.DATA_INCLUSAO = DateTime.Now;
                        _produto.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                        _produto.DATA_APROVACAO = DateTime.Now;
                        _produto.STATUS = 'A';

                        List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();
                        listaProdutos.Add(_produto);
                        if (_insert)
                            _produto.CODIGO = 0;
                        else
                            _produto.CODIGO = codigo;
                        desenvController.InserirAtualizarProduto(listaProdutos);
                    }
                }

                //Carregar grid de erros
                Carregar_gvErro(_lstErro);
                labErro.Text = "Arquivo importado com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = "Erro ao importar planilha(): " + ex.Message;
            }
            finally
            {

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

        #region "DADOS INICIAIS"
        [Serializable]
        public class ERRO
        {
            public string LINHA { get; set; }
            public string CODREF { get; set; }
            public string ORIGEM { get; set; }
            public string COLECAO { get; set; }
            public string GRUPO { get; set; }
            public string MODELO { get; set; }
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
                string fileLocation = Server.MapPath("~/Excel/") + upTecidoExcel.PostedFile.FileName;

                if (!System.IO.Directory.Exists(Server.MapPath("~/Excel/")))
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Excel/"));

                if (System.IO.File.Exists(fileLocation))
                    System.IO.File.Delete(fileLocation);

                //Salvar arquivo
                upTecidoExcel.PostedFile.SaveAs(fileLocation);

                //Obter extensão do arquivo
                string fileExtension = System.IO.Path.GetExtension(upTecidoExcel.PostedFile.FileName);
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
                {
                    return null;
                }

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
                string query = string.Format("SELECT * FROM [{0}] WHERE CODIGO IS NOT NULL", excelSheets[0]);
                //Preencher DATASET com retorno da query
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    dataAdapter.Fill(ds);

                if (ds.Tables[0].Columns.Count != 8)
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
