using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;
using System.Text;
using System.Globalization;

namespace Relatorios
{
    public partial class prod_con_hb_finalizado : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0, qtde_total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarGrupo();
                CarregarColecoes();
            }
        }

        #region "DADOS INICIAIS"
        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }
        #endregion

        #region "ACOES"
        private List<SP_OBTER_HB_PROCESSOResult> ObterFinalizado()
        {
            List<SP_OBTER_HB_PROCESSOResult> _finalizado = new List<SP_OBTER_HB_PROCESSOResult>();
            _finalizado = prodController.ObterHBProcesso(txtDataInicio.Text, txtDataFim.Text, ddlGrupo.SelectedValue);

            if (_finalizado != null)
            {
                if (ddlColecoes.SelectedValue.Trim() != "")
                    _finalizado = _finalizado.Where(p => p.COLECAO.Trim().ToUpper() == ddlColecoes.SelectedValue.Trim().ToUpper()).ToList();

                if (ddlMostruario.SelectedValue != "")
                    _finalizado = _finalizado.Where(p => p.MOSTRUARIO == Convert.ToChar(ddlMostruario.SelectedValue)).ToList();

                // - 18/01/2015 - TATHIANA - TRAZER SOMENTE OS PRINCIPAIS
                _finalizado = _finalizado.Where(p => p.DETALHE == "PRINCIPAL").ToList();
            }

            return _finalizado;
        }
        private void CarregarFinalizados()
        {
            List<SP_OBTER_HB_PROCESSOResult> _finalizado = new List<SP_OBTER_HB_PROCESSOResult>();
            _finalizado = ObterFinalizado();

            if (_finalizado != null)
            {
                gvFinalizado.DataSource = _finalizado;
                gvFinalizado.DataBind();

                btImprimir.Visible = false;
                if (_finalizado.Count > 0)
                    btImprimir.Visible = true;
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;

            labErro.Text = "";
            if (txtDataInicio.Text.Trim() != "" && !DateTime.TryParse(txtDataInicio.Text, out v_dataini))
            {
                labErro.Text = "INFORME UMA DATA INICIAL VÁLIDA.";
                return;
            }
            if (txtDataFim.Text.Trim() != "" && !DateTime.TryParse(txtDataFim.Text, out v_datafim))
            {
                labErro.Text = "INFORME UMA DATA FINAL VÁLIDA.";
                return;
            }

            try
            {
                CarregarFinalizados();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }
        protected void gvFinalizado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_PROCESSOResult _finalizado = e.Row.DataItem as SP_OBTER_HB_PROCESSOResult;

                    coluna += 1;
                    if (_finalizado != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_finalizado.COLECAO)).DESC_COLECAO;

                        qtde_total += Convert.ToInt32(_finalizado.QTDE);
                    }
                }
            }
        }
        protected void gvFinalizado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvFinalizado.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].Font.Bold = true;
                footer.Cells[10].Text = qtde_total.ToString();
                footer.Cells[10].Font.Bold = true;
            }
        }
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErroImpressao.Visible = false;

                List<SP_OBTER_HB_PROCESSOResult> _finalizadoLista = new List<SP_OBTER_HB_PROCESSOResult>();
                _finalizadoLista = ObterFinalizado();

                List<Fila> _fila = new List<Fila>();
                foreach (SP_OBTER_HB_PROCESSOResult _finalizado in _finalizadoLista)
                {
                    _fila.Add(new Fila
                    {
                        HB = Convert.ToInt32(_finalizado.HB),
                        MOSTRUARIO = ((_finalizado.MOSTRUARIO == 'S') ? true : false),
                        GRUPO = _finalizado.GRUPO,
                        NOME = _finalizado.NOME,
                        TIPO = _finalizado.DETALHE,
                        COR = prodController.ObterCoresBasicas(_finalizado.COR).DESC_COR,
                        DATA = _finalizado.DT_FINALIZADO,
                        QUANTIDADE = Convert.ToInt32(_finalizado.QTDE),
                        TITULO = ((_finalizado.DETALHE == "PRINCIPAL") ? "PRINCIPAL" : "DETALHES")
                    });
                }

                //IMPRIMIR RELATÓRIO
                if (_fila != null && _fila.Count > 0)
                    GerarRelatorio(_fila.Where(p => p.TITULO == "PRINCIPAL").ToList(), _fila.Where(p => p.TITULO == "DETALHES").ToList());
            }
            catch (Exception ex)
            {
                labErroImpressao.Text = "ERRO: " + ex.Message;
                labErroImpressao.Visible = true;
            }
        }
        #endregion

        #region "RELATORIO"
        private void GerarRelatorio(List<Fila> _filaPrincipal, List<Fila> _filaDetalhe)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FILA_FINALIZADO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_filaPrincipal, _filaDetalhe));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
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
        private StringBuilder MontarRelatorioHTML(List<Fila> _filaPrincipal, List<Fila> _filaDetalhe)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarFinalizado(_texto, _filaPrincipal);
            _texto = MontarFinalizado(_texto, _filaDetalhe);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            //DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            //string dia_da_semana = dtfi.GetDayName(dataCortado.DayOfWeek);

            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Cortado</title>   ");
            _texto.Append("         <meta charset='UTF-8'>          ");
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
            _texto.Append("     <div id='fichaCortado' align='center' style='border: 0px solid #000;'>");
            _texto.Append("        <br />");
            _texto.Append("        <br />");
            _texto.Append("        <div align='center' style='border: 2px solid #000; background-color: transparent;");
            _texto.Append("            width: 517pt;'>");
            _texto.Append("            <h2>");
            _texto.Append("                FINALIZADO - " + txtDataInicio.Text + " - " + txtDataFim.Text + "</h2>");
            _texto.Append("            <table cellpadding='0' cellspacing='0' style='width: 518pt; padding: 0px; color: black;");
            _texto.Append("                font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white;");
            _texto.Append("                white-space: nowrap; border: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");

            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td>");
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
        private StringBuilder MontarFinalizado(StringBuilder _texto, List<Fila> _fila)
        {
            int total = 0;

            if (_fila.Count > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                _texto.Append("        <span style='font-size: 15px;'>#" + _fila[0].TITULO + "</span>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='6'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                foreach (Fila fila in _fila)
                {
                    _texto.Append("            <tr>");
                    _texto.Append("                <td style='width: 130px'>");
                    if (fila.TITULO == "DETALHES")
                        _texto.Append("                    HB " + fila.HB.ToString() + " - " + fila.TIPO + ((fila.MOSTRUARIO) ? " - M" : ""));
                    else
                        _texto.Append("                    HB " + fila.HB.ToString() + ((fila.MOSTRUARIO) ? " - M" : ""));
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 130px'>");
                    _texto.Append("                    " + fila.GRUPO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 150px'>");
                    _texto.Append("                    " + fila.NOME);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 100px'>");
                    _texto.Append("                    " + fila.COR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 80px'>");
                    _texto.Append("                    " + fila.DATA);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right;'>");
                    _texto.Append("                    " + fila.QUANTIDADE);
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");

                    total += fila.QUANTIDADE;
                }

                _texto.Append("            <tr>");
                _texto.Append("                <td style='width: 130px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 130px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 150px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 100px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 80px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right;'>");
                _texto.Append("                    " + total.ToString());
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
