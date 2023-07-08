using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DAL;

namespace Relatorios
{
    public partial class contabil_analise_ffc_imp : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        StringWriter _sw = new StringWriter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");

                try
                {
                    string _numeroFechamento = Request.QueryString["nfecx"];
                    if (_numeroFechamento != "" && Convert.ToInt32(_numeroFechamento) > 0)
                    {
                        MontarArquivoFechamento(_numeroFechamento.Trim());
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Erro ao abrir arquivo. Envie esta tela para o TI.");
                    Response.Write("<br><br>");
                    Response.Write(ex.Message);
                    Response.Write("<br><br>");
                    Response.Write(ex.StackTrace);
                }
            }
        }

        private void MontarArquivoFechamento(string numeroFechamento)
        {
            FUNDO_FIXO_HISTORICO_FECHAMENTO _ff = new FUNDO_FIXO_HISTORICO_FECHAMENTO();
            FILIAI _filial = new FILIAI();


            try
            {
                _ff = baseController.BuscaFundoFixoHistoricoFechamento(Convert.ToInt32(numeroFechamento));
            }
            catch (Exception)
            {
                throw;
            }

            if (_ff != null)
            {
                _filial = baseController.BuscaFilialCodigo(Convert.ToInt32(_ff.CODIGO_FILIAL));


                //Escrevendo o arquivo
                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("Fechamento de Fundo Fixo - Nº " + numeroFechamento + " - " + ((_filial != null) ? _filial.FILIAL : "FILIAL NÃO ENCONTRADA"));
                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("Saldo Anterior : " + ((_ff.SALDO_ANTERIOR > 0) ? _ff.SALDO_ANTERIOR.ToString("####0.00") : "0"));
                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("Retiradas:");
                Response.Write("<br>");
                //Buscar histórico de receitas do Fechamento
                decimal totalReceitas = 0;
                List<FUNDO_FIXO_HISTORICO_RECEITA> receitas = baseController.BuscaReceitasFechamento(numeroFechamento);
                if (receitas != null)
                    foreach (FUNDO_FIXO_HISTORICO_RECEITA itemReceita in receitas)
                    {
                        Response.Write(itemReceita.CODIGO_CONTA_RECEITA.Trim() + "\t&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemReceita.DATA + "\t&nbsp;" + itemReceita.VALOR.ToString("#####0.00"));
                        Response.Write("<br>");
                        totalReceitas += itemReceita.VALOR;
                    }
                //FIM

                Response.Write("<br>");
                Response.Write("Total das Retiradas : " + ((totalReceitas > 0) ? totalReceitas.ToString("####0.00") : "0"));

                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("Despesas:");
                Response.Write("<br>");
                //Buscar histórico de despesas do Fechamento
                decimal totalDespesas = 0;
                List<FUNDO_FIXO_HISTORICO_DESPESA> listaHistoricoDespesa = baseController.BuscaDespesasFechamento(numeroFechamento);
                if (listaHistoricoDespesa != null)
                    foreach (FUNDO_FIXO_HISTORICO_DESPESA itemDespesa in listaHistoricoDespesa)
                    {
                        Response.Write(itemDespesa.CODIGO_CONTA_DESPESA.Trim() + "\t&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemDespesa.DATA + "\t&nbsp;" + itemDespesa.VALOR.ToString("#####0.00") + "&nbsp;-&nbsp;");
                        Response.Write(((itemDespesa.DESC_DESPESA == null || itemDespesa.DESC_DESPESA.Trim() == "") ? "" : (itemDespesa.DESC_DESPESA)));
                        Response.Write("<br>");
                        totalDespesas += itemDespesa.VALOR;
                    }
                //FIM

                Response.Write("<br>");
                Response.Write("Total das Despesas : " + ((totalDespesas > 0) ? totalDespesas.ToString("####0.00") : "0"));

                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("<br>");
                Response.Write("Saldo Atual : " + ((_ff.SALDO_ATUAL > 0) ? _ff.SALDO_ATUAL.ToString("####0.00") : "0"));
                Response.Write("<br>");

                Response.Write("<script>window.print();</script>");
                //Response.Write("<script>window.close();</script>");
            }
            else
            {
                Response.Write("<br>");
                Response.Write("FUNDO FIXO Nº " + numeroFechamento + " NÃO ENCONTRADO...");
                Response.Write("<br>");
            }
        }
    }
}