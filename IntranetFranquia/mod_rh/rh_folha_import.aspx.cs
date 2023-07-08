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
    public partial class rh_folha_import : System.Web.UI.Page
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
        }

        #region "DADOS INICIAIS"
        private void CarregarCompetencia()
        {
            var comp = baseController.BuscaReferencias().Where(p => (p.ANO >= 2017 && p.MES >= 11) || p.ANO >= 2018).OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).ToList();

            comp.Insert(0, new ACOMPANHAMENTO_ALUGUEL_MESANO { CODIGO_ACOMPANHAMENTO_MESANO = 0, DESCRICAO = "Selecione" });
            ddlCompetencia.DataSource = comp;
            ddlCompetencia.DataBind();
        }
        #endregion

        protected void btImportar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";

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

            int totalLinha = 0;
            try
            {
                labErro.Text = "Carregando arquivo...";

                using (StreamReader reader = new StreamReader(uploadArquivoFolha.PostedFile.InputStream))
                {
                    //le a primeira linha para retirar do loop
                    var linha = reader.ReadLine();

                    var competencia = Convert.ToDateTime("01/" + ddlCompetencia.SelectedItem.Text.Trim());

                    rhController.ExcluirProPayFolhaDRE(competencia);

                    while (!reader.EndOfStream)
                    {
                        linha = reader.ReadLine();
                        var valores = linha.Split(';');

                        var pp = new PROPAY_DRE_FOLHA();
                        pp.COMPETENCIA = competencia;
                        pp.COD_EMPRESA = valores[0].Split('-')[0].Trim();

                        var empresaArr = valores[0].Split('-');
                        if (empresaArr.Length == 2)
                            pp.EMPRESA = empresaArr[1];
                        else
                            pp.EMPRESA = empresaArr[1] + " " + empresaArr[2];
                        pp.CPF = valores[1].Replace(".", "").Replace("-", "").Trim();
                        pp.NOME = valores[2].Trim();

                        var cc = valores[3].Split('-');
                        if (cc != null)
                        {
                            pp.COD_CENTRO_CUSTO = cc[0].Trim();
                            pp.CENTRO_CUSTO = "";
                            if (cc.Length == 2)
                                pp.CENTRO_CUSTO = cc[1].Trim();
                        }
                        else
                        {
                            pp.COD_CENTRO_CUSTO = "";
                            pp.CENTRO_CUSTO = "";
                        }

                        pp.CNPJ = valores[4].Trim().Replace(".", "").Replace("-", "").Replace("/", "").Trim();
                        pp.MATRICULA = valores[5].Trim();
                        pp.CARGO = valores[6].Trim();
                        pp.DESC_COMPETENCIA = valores[7].ToUpper();
                        pp.SALARIO_BRUTO = Convert.ToDecimal(valores[8].Replace("R$", ""));
                        pp.TOT_PROVENTOS = Convert.ToDecimal(valores[9].Replace("R$", ""));
                        pp.SALARIO_LIQUIDO = Convert.ToDecimal(valores[10].Replace("R$", ""));
                        pp.INSS_EMPRESA = Convert.ToDecimal(valores[11].Replace("R$", ""));
                        pp.INSS_FUNCIONARIO = Convert.ToDecimal(valores[12].Replace("R$", ""));
                        pp.IRPF = Convert.ToDecimal(valores[13].Replace("R$", ""));
                        pp.FGTS = Convert.ToDecimal(valores[14].Replace("R$", ""));
                        pp.FGTS50 = Convert.ToDecimal(valores[15].Replace("R$", ""));
                        pp.AVISO_PREVIO = Convert.ToDecimal(valores[16].Replace("R$", ""));
                        pp.VT_EMPRESA = Convert.ToDecimal(valores[17].Replace("R$", ""));
                        pp.VT_FUNCIONARIO = Convert.ToDecimal(valores[18].Replace("R$", ""));

                        pp.VR_EMPRESA = Convert.ToDecimal(valores[19].Replace("R$", ""));
                        pp.VR_FUNCIONARIO = Convert.ToDecimal(valores[20].Replace("R$", ""));

                        pp.VA_EMPRESA = Convert.ToDecimal(valores[21].Replace("R$", ""));
                        pp.VA_FUNCIONARIO = Convert.ToDecimal(valores[22].Replace("R$", ""));

                        pp.MULTICASH_EMPRESA = Convert.ToDecimal(valores[23].Replace("R$", ""));
                        pp.MULTICASH_FUNCIONARIO = Convert.ToDecimal(valores[24].Replace("R$", ""));

                        pp.PROV_13SALARIO = Convert.ToDecimal(valores[25].Replace("R$", ""));
                        pp.PROV_FERIAS = Convert.ToDecimal(valores[26].Replace("R$", ""));
                        pp.DATA_INCLUSAO = DateTime.Now;
                        pp.USUARIO_INCLUSAO = 1144;

                        rhController.InserirProPayFolhaDRE(pp);
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
                    labErro.Text = "Erro ao gerar saldo do estoque do linx.";
                else
                    labErro.Text = ex.Message;

            }
            finally
            {

            }
        }

    }
}
