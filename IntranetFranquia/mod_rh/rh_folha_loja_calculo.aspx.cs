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
    public partial class rh_folha_loja_calculo : System.Web.UI.Page
    {
        RHController rhController = new RHController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarCompetencia();
            }

            //Evitar duplo clique no botão
            btImportar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImportar, null) + ";");
            btCalcularComissao.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btCalcularComissao, null) + ";");
            btExportar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btExportar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarCompetencia()
        {
            var comp = baseController.BuscaReferencias().Where(p => p.ANO >= 2018).OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).ToList();

            comp.Insert(0, new ACOMPANHAMENTO_ALUGUEL_MESANO { CODIGO_ACOMPANHAMENTO_MESANO = 0, DESCRICAO = "Selecione" });
            ddlCompetencia.DataSource = comp;
            ddlCompetencia.DataBind();
        }
        #endregion

        protected void btImportar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            labMsg.Text = "";

            if (ddlCompetencia.SelectedValue == "0")
            {
                labErro.Text = "Selecione uma Competência.";
                return;
            }

            //Validar arquivo inserido
            if (!uploadArquivoFolha.HasFile)
            {
                labErro.Text = "Selecione um arquivo de folha .CSV.";
                return;
            }

            //Validar Tamanho do Arquivo
            if (uploadArquivoFolha.PostedFile.ContentLength < 0)
            {
                labErro.Text = "Selecione um arquivo com o tamanho maior que zero.";
                return;
            }

            //Validar Extensão
            string fileExtension = System.IO.Path.GetExtension(uploadArquivoFolha.PostedFile.FileName);
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

            var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());
            var folha = rhController.ObterCalculoComissao(competencia, "", "", "");
            if (folha != null && folha.Count() > 0)
            {
                labErro.Text = "Esta competência já foi calculada. Para importar um novo arquivo entre em contato com TI.";
                return;
            }

            int totalLinha = 0;
            try
            {
                labErro.Text = "Carregando arquivo...";

                using (StreamReader reader = new StreamReader(uploadArquivoFolha.PostedFile.InputStream, Encoding.Default))
                {
                    //le a primeira linha para retirar do loop
                    var linha = reader.ReadLine();

                    rhController.ExcluirFolhaLoja(competencia);

                    while (!reader.EndOfStream)
                    {
                        linha = reader.ReadLine();
                        var valores = linha.Split(';');

                        var ff = new RH_FOLHA_LOJA();

                        ff.COMPETENCIA = competencia;
                        ff.MATRICULA = valores[2].Trim();
                        ff.NOME = valores[3].Trim();

                        var cpf = ("00000000000" + valores[4].Trim());
                        ff.CPF = cpf.Substring(cpf.Length - 11);
                        ff.CARGO = valores[5].Trim();

                        ff.LOJA = valores[6].ToUpper();

                        ff.SAL_MINIMO_GARA = Convert.ToDecimal(valores[8].Replace("R$", ""));
                        ff.COMISSAO = 0;
                        ff.PREMIO_PONTA = 0;
                        ff.PREMIO_COTA = 0;
                        ff.PREMIO_VENDEDOR = 0;
                        ff.PREMIO_CONVCOLET = 0;
                        ff.SUPERVISOR = "";
                        ff.GERENTE = "";
                        ff.EMPRESA = valores[0].Trim();
                        ff.VALOR_VENDIDOCOM = 0;
                        ff.VALOR_VENDIDOCOTA = 0;
                        ff.PREMIO_VENDEDOR = 0;
                        ff.COMISSAO_PORC = 0;
                        ff.COMISSAO_COTA = 0;
                        ff.COTA_SEM1 = 0;
                        ff.COTA_SEM2 = 0;
                        ff.COTA_SEM3 = 0;
                        ff.COTA_SEM4 = 0;
                        ff.COTA_TOTAL = 0;

                        ff.VALOR_VENDIDO_ECOM = 0;

                        ff.DATA_INCLUSAO = DateTime.Now;
                        ff.USUARIO_INCLUSAO = codigoUsuario;

                        rhController.InserirFolhaLoja(ff);

                        rhController.GerarFolhaFuncionarioHist(ff.CPF, ff.COMPETENCIA);

                        totalLinha += 1;
                    }
                }

                labErro.Text = "Arquivo de Funcionários importado com sucesso.";
                btCalcularComissao.Enabled = true;
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

        protected void btCalcularComissao_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labMsg.Text = "";

                if (ddlCompetencia.SelectedValue == "0")
                {
                    labErro.Text = "Selecione uma Competência.";
                    return;
                }

                var usuario = ((USUARIO)Session["USUARIO"]);
                var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());
                var ret = rhController.GerarFolha(competencia, usuario.CODIGO_USUARIO);

                if (ret.OK == 1)
                    labMsg.Text = "Folha calculada com sucesso. Clique em Exportar para obter o Relatório.";
                else
                    labMsg.Text = "Por favor, importar o arquivo da competência antes de solicitar o Cálculo da Comissão.";
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }
        }

        protected void btExportar_Click(object sender, EventArgs e)
        {
            try
            {

                labErro.Text = "";
                labMsg.Text = "";

                if (ddlCompetencia.SelectedValue == "0")
                {
                    labErro.Text = "Selecione uma Competência.";
                    return;
                }

                var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());

                var folha = rhController.ObterCalculoComissao(competencia, "", "", "");
                if (folha == null || folha.Count() <= 0)
                {
                    labMsg.Text = "Não existe cálculo para esta competência.";
                    return;
                }

                gvExcel.DataSource = folha;
                gvExcel.DataBind();

                if (folha != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "FOLHA_" + ddlCompetencia.SelectedItem.Text.Replace("/", "-") + "_" + DateTime.Now.ToString("dd_MM_HH_mm") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    //Abaixo codifica os caracteres para o alfabeto latino
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                    Response.Charset = "ISO-8859-1";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvExcel.AllowPaging = false;
                    gvExcel.PageSize = 1000;
                    gvExcel.DataSource = folha;
                    gvExcel.DataBind();

                    gvExcel.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvExcel.HeaderRow.Cells.Count; i++)
                    {
                        gvExcel.HeaderRow.Cells[i].Style.Add("background-color", "#CCCCCC");
                        gvExcel.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }

                    for (int x = 0; x < gvExcel.Rows.Count; x++)
                    {
                        gvExcel.Rows[x].Cells[2].Style.Add("mso-number-format", "\\@");
                    }

                    labMsg.Text = "Arquivo exportado com sucesso";

                    gvExcel.RenderControl(htw);
                    Response.Write(sw.ToString());
                    Response.End();
                }

                labMsg.Text = "Arquivo exportado com sucesso";
            }
            catch (Exception ex)
            {

                labMsg.Text = ex.Message;
            }


        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }
}
