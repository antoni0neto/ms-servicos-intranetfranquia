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
    public partial class cont_estoque_loja : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataContagem.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarFilial();

                txtDataContagem.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }

            //Evitar duplo clique no botão
            btImportar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImportar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                        ddlFilial.SelectedIndex = 1;


                }
            }
        }
        #endregion

        protected void btImportar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            hidCodigoEstoqueLojaCont.Value = "";
            btAbrirRelatorio.Visible = false;

            if (ddlFilial.SelectedValue == "")
            {
                labErro.Text = "Selecione uma Filial.";
                return;
            }

            if (txtDescricao.Text.Trim() == "")
            {
                labErro.Text = "Informe uma Descrição.";
                return;
            }

            if (txtDataContagem.Text.Trim() == "")
            {
                labErro.Text = "Informe a Data da Contagem.";
                return;
            }

            //Validar arquivo inserido
            if (!upEstoqueTxt.HasFile)
            {
                labErro.Text = "Selecione um arquivo TXT.";
                return;
            }

            //Validar Tamanho do Arquivo
            if (upEstoqueTxt.PostedFile.ContentLength < 0)
            {
                labErro.Text = "Selecione um arquivo com o tamanho maior que zero.";
                return;
            }

            //Validar Extensão
            string fileExtension = System.IO.Path.GetExtension(upEstoqueTxt.PostedFile.FileName);
            if (fileExtension.ToUpper() != ".TXT")
            {
                labErro.Text = "Selecione um arquivo com extensão \".txt\".";
                return;
            }

            int codigoEstoqueLojaCont = 0;
            int totalLinha = 0;
            try
            {
                labErro.Text = "Carregando arquivo...";

                //inserir cabecalho
                ESTOQUE_LOJA_CONT estoqueLojaCont = new ESTOQUE_LOJA_CONT();
                estoqueLojaCont.CODIGO_FILIAL = ddlFilial.SelectedValue;
                estoqueLojaCont.DATA_CONTAGEM = Convert.ToDateTime(txtDataContagem.Text);
                estoqueLojaCont.DESCRICAO = txtDescricao.Text.Trim().ToUpper();
                estoqueLojaCont.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                estoqueLojaCont.DATA_INCLUSAO = DateTime.Now;
                codigoEstoqueLojaCont = contController.InserirEstoqueLojaContagem(estoqueLojaCont);

                Stream arquivo = upEstoqueTxt.PostedFile.InputStream;
                using (StreamReader sr = new StreamReader(arquivo))
                {
                    //Variáveis do arquivo
                    string produto = "";
                    string cor = "";
                    string tamanho = "";
                    int quantidade = 0;


                    string linha = "";
                    string[] linhaArquivo;
                    string linhaAux = "";

                    ESTOQUE_LOJA_CONT_ARQ estoqueArq = null;
                    while ((linha = sr.ReadLine()) != null)
                    {
                        //Conta linhas
                        totalLinha += 1;

                        //inicializa variaveis
                        produto = "";
                        cor = "";
                        tamanho = "";
                        quantidade = 0;

                        linhaArquivo = linha.Split(',');

                        if (linhaArquivo[0].Trim() != "")
                        {
                            linhaAux = linhaArquivo[0].Trim();

                            var produtoBarra = baseController.BuscaProdutoBarra(linhaAux);
                            if (produtoBarra != null)
                            {
                                produto = produtoBarra.PRODUTO.Trim();
                                cor = produtoBarra.COR_PRODUTO.Trim();

                                var tamanhoAux = ("00" + produtoBarra.TAMANHO.ToString());
                                tamanhoAux = tamanhoAux.Substring(tamanhoAux.Length - 2);
                                tamanho = tamanhoAux;
                            }
                            else
                            {
                                produto = linhaAux.Substring(0, 5);
                                tamanho = linhaAux.Substring(linhaAux.Length - 2, 2);

                                //cor sempre por último
                                linhaAux = linhaAux.Remove(0, 5);
                                linhaAux = linhaAux.Remove(linhaAux.Length - 2, 2);
                                cor = linhaAux;
                            }
                        }

                        if (linhaArquivo[1].Trim() != "")
                            quantidade = Convert.ToInt32(linhaArquivo[1].Trim());

                        //insere linha
                        estoqueArq = new ESTOQUE_LOJA_CONT_ARQ();
                        estoqueArq.ESTOQUE_LOJA_CONT = codigoEstoqueLojaCont;
                        estoqueArq.PRODUTO = produto;
                        estoqueArq.COR = cor;
                        estoqueArq.TAMANHO = tamanho;
                        estoqueArq.QTDE = quantidade;
                        contController.InserirEstoqueLojaContagemArq(estoqueArq);
                    }
                }
                totalLinha = -1;

                //INSERE SALDO ESTOQUE PA
                if (contController.GerarSaldoEstoquePA(Convert.ToDateTime(txtDataContagem.Text), ddlFilial.SelectedItem.Text, codigoEstoqueLojaCont))
                {
                    labErro.Text = "Arquivo importado com sucesso.";
                    hidCodigoEstoqueLojaCont.Value = codigoEstoqueLojaCont.ToString();
                    btAbrirRelatorio.Visible = true;

                    btAbrirRelatorio_Click(btAbrirRelatorio, null);
                }
                else
                {
                    labErro.Text = "Ocorreu um erro inesperado. Entre em contato com o suporte.";
                }

            }
            catch (Exception ex)
            {
                if (totalLinha > 0)
                    labErro.Text = "Erro ao importar arquivo: Linha " + totalLinha.ToString();
                else if (totalLinha == -1)
                    labErro.Text = "Erro ao gerar saldo do estoque do linx.";
                else
                    labErro.Text = ex.Message;


                if (codigoEstoqueLojaCont > 0)
                    contController.ExcluirEstoqueLojaContagem(codigoEstoqueLojaCont);

            }
            finally
            {

            }
        }

        protected void btAbrirRelatorio_Click(object sender, EventArgs e)
        {
            string url = "";
            string msg = "";
            try
            {
                string codigoEstoqueLojaCont = hidCodigoEstoqueLojaCont.Value;

                //Abrir pop-up
                url = "fnAbrirTelaCadastroMaior('cont_estoque_loja_rel_detalhes.aspx?p=" + codigoEstoqueLojaCont + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);
            }
            catch (Exception)
            {
                msg = "Erro ao abrir o Relatório.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }
        }

    }
}
