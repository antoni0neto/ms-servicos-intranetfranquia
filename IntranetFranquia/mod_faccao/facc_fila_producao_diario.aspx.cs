using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;

namespace Relatorios
{
    public partial class facc_fila_producao_diario : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        int totalCorte = 0;
        int totalEntrou = 0;
        int totalFalta = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPesquisa.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "facc_menu_relatorio.aspx";

                txtDataPesquisa.Text = DateTime.Now.ToString("dd/MM/yyyy");
                CarregarServicos();
                CarregarColecoes();

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labErro.Text = "";

                DateTime dataProducao;
                if (!DateTime.TryParse(txtDataPesquisa.Text, out dataProducao))
                {
                    labMsg.Text = "INFORME UMA DATA VÁLIDA.";
                    return;
                }

                if (ddlServico.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione o Serviço";
                    return;
                }

                int hb = 0;
                if (txtHB.Text.Trim() != "")
                    hb = Convert.ToInt32(txtHB.Text.Trim());

                CarregarRelatorio(dataProducao, ddlColecoes.SelectedValue.Trim(), Convert.ToInt32(ddlServico.SelectedValue), hb);


                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = "ERRO: " + ex.Message;
            }
        }
        private List<SP_OBTER_FACCAO_PRODUCAO_DIARIOResult> ObterProducaoDiario(DateTime dataPesquisa, string colecao, int codigoServico, int hb)
        {

            var producaoDiario = faccController.ObterFaccaoProducaoDiario(dataPesquisa, colecao, codigoServico, hb);

            if (txtProduto.Text.Trim() != "")
                producaoDiario = producaoDiario.Where(p => p.PRODUTO.Trim() == txtProduto.Text.Trim()).ToList();

            return producaoDiario;
        }


        #region "DADOS INICIAIS"
        private void CarregarServicos()
        {
            var servicos = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A' && p.CODIGO != 5).ToList();

            servicos.Insert(0, new PROD_SERVICO { CODIGO = 0, DESCRICAO = "Selecione", STATUS = 'A' });
            ddlServico.DataSource = servicos;
            ddlServico.DataBind();

        }
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = colecoes;
                ddlColecoes.DataBind();
            }
        }
        private void CarregarJQuery()
        {
            //GRID Enviado
            if (gvCorte.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID Recebido
            if (gvPonta.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        #region "CORTES/PONTAS"
        private void CarregarRelatorio(DateTime dataPesquisa, string colecao, int codigoServico, int hb)
        {
            var producaoDiario = ObterProducaoDiario(dataPesquisa, colecao, codigoServico, hb);

            gvCorte.DataSource = producaoDiario.Where(p => p.TIPO == "C");
            gvCorte.DataBind();

            totalCorte = 0;
            totalEntrou = 0;
            totalFalta = 0;

            gvPonta.DataSource = producaoDiario.Where(p => p.TIPO == "P");
            gvPonta.DataBind();

        }
        protected void gvCorte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_PRODUCAO_DIARIOResult corte = e.Row.DataItem as SP_OBTER_FACCAO_PRODUCAO_DIARIOResult;

                    if (corte != null)
                    {

                        totalCorte += Convert.ToInt32(corte.QTDE_A_PRODUZIR);
                        totalEntrou += Convert.ToInt32(corte.QTDE_RECEBIDA_HOJE);
                        totalFalta += Convert.ToInt32(corte.QTDE_FALTA);
                    }
                }
            }
        }
        protected void gvCorte_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[7].Text = totalCorte.ToString();
                footer.Cells[8].Text = totalEntrou.ToString();
                footer.Cells[9].Text = totalFalta.ToString();

            }
        }


        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            DateTime dataPesquisa = DateTime.Now;

            labMsg.Text = "";
            labErro.Text = "";

            if (!DateTime.TryParse(txtDataPesquisa.Text, out dataPesquisa))
            {
                labMsg.Text = "INFORME UMA DATA VÁLIDA.";
                return;
            }

            if (ddlServico.SelectedValue == "0")
            {
                labMsg.Text = "Selecione o Serviço";
                return;
            }

            int hb = 0;
            if (txtHB.Text.Trim() != "")
                hb = Convert.ToInt32(txtHB.Text.Trim());

            try
            {
                var faccaoEnviado = ObterProducaoDiario(dataPesquisa, ddlColecoes.SelectedValue.Trim(), Convert.ToInt32(ddlServico.SelectedValue), hb);

                GerarRelatorio(faccaoEnviado, dataPesquisa);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void GerarRelatorio(List<SP_OBTER_FACCAO_PRODUCAO_DIARIOResult> producaoDiario, DateTime dataPesquisa)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PRODUCAO_MAR_DIARIO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(producaoDiario, dataPesquisa));
                wr.Flush();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_FACCAO_PRODUCAO_DIARIOResult> producaoDiario, DateTime dataPesquisa)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, dataPesquisa);
            _texto = MontarFaccaoDiario(_texto, producaoDiario.Where(p => p.TIPO == "C").ToList(), "CORTES");
            _texto = MontarEspaco(_texto);
            _texto = MontarFaccaoDiario(_texto, producaoDiario.Where(p => p.TIPO == "P").ToList(), "PONTAS");
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto, DateTime dataPesquisa)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            string dia_da_semana = dtfi.GetDayName(dataPesquisa.DayOfWeek);

            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Produção Diário</title>");
            _texto.Append("         <meta charset='UTF-8'>");
            _texto.Append("         <style type='text/css'>");
            _texto.Append("             @media print");
            _texto.Append("             {");
            _texto.Append("                 .tdback");
            _texto.Append("                 {");
            _texto.Append("                     background-color: WindowFrame !important;");
            _texto.Append("                     -webkit-print-color-adjust: exact;");
            _texto.Append("                 }");
            _texto.Append("             }");
            _texto.Append("         </style>");
            _texto.Append("     </head>");
            _texto.Append("");
            _texto.Append("<body onLoad='window.print();'>");
            _texto.Append("     <div id='fichaAcabamento' align='center' style='border: 0px solid #000;'>");
            _texto.Append("        <br />");
            _texto.Append("        <br />");
            _texto.Append("        <div align='center' style='border: 2px solid #000; background-color: transparent;");
            _texto.Append("            width: 517pt;'>");
            _texto.Append("            <table cellpadding='0' cellspacing='0' style='width: 518pt; padding: 0px; color: black;");
            _texto.Append("                font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white;");
            _texto.Append("                white-space: nowrap; border: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr style='line-height:15px;'>");
            _texto.Append("                    <td style='width: 150px; text-align:center;'>");
            _texto.Append("                         <img alt='HBF' Width='50px' Height='50px' src='../../Image/hbf_branco.png' />");
            _texto.Append("                    </td>");
            _texto.Append("                    <td>");
            _texto.Append("                         &nbsp;&nbsp;&nbsp;&nbsp;<h2>");
            _texto.Append("                         " + dataPesquisa.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana) + " Produção do Dia</h2>");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");

            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("            </table>");
            _texto.Append("        </div>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }
        private StringBuilder MontarEspaco(StringBuilder _texto)
        {
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");

            return _texto;
        }
        private StringBuilder MontarFaccaoDiario(StringBuilder _texto, List<SP_OBTER_FACCAO_PRODUCAO_DIARIOResult> _fila, string titulo)
        {
            int cont = 1;

            if (_fila.Count > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                _texto.Append("        <span style='font-size: 15px;'># " + titulo + "</span>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 10px; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='7'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                _texto.Append("            <tr>");
                _texto.Append("                <td style='width: 15px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    Fornecedor");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    Coleção");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    Produto");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    Cor");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 70px'>");
                _texto.Append("                    HB");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right; width: 50px;'>");
                _texto.Append("                    Corte");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right; width: 50px;'>");
                _texto.Append("                    Entrou");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right; width: 50px;'>");
                _texto.Append("                    Falta");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                var totalCorte = 0;
                var totalEntrou = 0;
                var totalFalta = 0;

                foreach (SP_OBTER_FACCAO_PRODUCAO_DIARIOResult fila in _fila)
                {
                    _texto.Append("            <tr>");
                    _texto.Append("                <td style='width: 15px'>");
                    _texto.Append("                    " + cont.ToString());
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.FORNECEDOR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.DESC_COLECAO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.PRODUTO + " - " + fila.DESC_PRODUTO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.DESC_COR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 70px'>");
                    _texto.Append("                    " + fila.HB);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right; width: 50px;'>");
                    _texto.Append("                    " + fila.QTDE_A_PRODUZIR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right; width: 50px;'>");
                    _texto.Append("                    " + fila.QTDE_RECEBIDA_HOJE);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right; width: 50px;'>");
                    _texto.Append("                    " + fila.QTDE_FALTA);
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");

                    cont += 1;
                    totalCorte += Convert.ToInt32(fila.QTDE_A_PRODUZIR);
                    totalEntrou += Convert.ToInt32(fila.QTDE_RECEBIDA_HOJE);
                    totalFalta += Convert.ToInt32(fila.QTDE_FALTA);
                }

                _texto.Append("            <tr>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right; width: 50px;'>");
                _texto.Append("                    " + totalCorte.ToString());
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right; width: 50px;'>");
                _texto.Append("                    " + totalEntrou.ToString());
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right; width: 50px;'>");
                _texto.Append("                    " + totalFalta.ToString());
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("        </table>");
                _texto.Append("    </td>");
                _texto.Append("</tr>");
            }
            return _texto;
        }
        #endregion

    }
}

