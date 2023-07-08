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
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class prod_fila_riscocorte : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int auxHB = 0;
        Color corGrid = Color.White;
        bool trocouHB = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                int prod_processo = 0;

                prod_processo = Convert.ToInt32(ddlProducao.SelectedValue);

                CarregarRiscoCorte(prod_processo);

            }
            catch (Exception ex)
            {

                labMsg.Text = ex.Message;
            }
        }
        private List<SP_OBTER_HB_FILAResult> ObterHbFila(int prod_processo, char mostruario)
        {
            var filaHB = prodController.ObterHBFilaPrincipalDetalhe(prod_processo, mostruario);

            if (prod_processo == 2) //RISCO
                filaHB = filaHB.Where(p => p.PROD_DETALHE != 4 && p.PROD_DETALHE != 5).ToList();

            return filaHB;
        }

        #region "GRID"
        private void CarregarRiscoCorte(int prod_processo)
        {
            List<SP_OBTER_HB_FILAResult> _hb = ObterHbFila(prod_processo, 'N');
            if (_hb != null)
            {
                gvRiscoCorte.DataSource = _hb;
                gvRiscoCorte.DataBind();
                if (_hb.Count <= 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            }
        }
        protected void gvRiscoCorte_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_FILAResult _HB = e.Row.DataItem as SP_OBTER_HB_FILAResult;

                    if (_HB != null)
                    {
                        if (auxHB == 0)
                            auxHB = Convert.ToInt32(_HB.HB);

                        if (auxHB != Convert.ToInt32(_HB.HB))
                        {
                            if (trocouHB)
                            {
                                corGrid = Color.White;
                                trocouHB = false;
                            }
                            else
                            {
                                corGrid = Color.LightGray;
                                trocouHB = true;
                            }
                        }

                        e.Row.BackColor = corGrid;
                        auxHB = Convert.ToInt32(_HB.HB);

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                        {
                            _qtde.Text = _HB.GRADE_TOTAL.ToString();
                        }

                        Literal _data = e.Row.FindControl("litData") as Literal;
                        DateTime _DataInicial = DateTime.Now;
                        if (_data != null)
                        {
                            _DataInicial = Convert.ToDateTime(_HB.DATA_INICIO_PROCESSO);
                            _data.Text = _DataInicial.ToString("dd/MM/yyyy HH:mm");
                        }

                        Literal _tempo = e.Row.FindControl("litTempo") as Literal;
                        if (_tempo != null)
                        {
                            TimeSpan date = DateTime.Now - _DataInicial;
                            if (date.Days < 1)
                                _tempo.Text = date.Hours.ToString() + " horas";
                            else if (date.Days == 1)
                                _tempo.Text = date.Days.ToString() + " dia " + date.Hours.ToString() + "h";
                            else
                                _tempo.Text = date.Days.ToString() + " dias " + date.Hours.ToString() + "h";
                        }

                        Literal _liquidar = e.Row.FindControl("litLiquidar") as Literal;
                        if (_liquidar != null)
                        {
                            _liquidar.Text = ((_HB.LIQUIDAR == 'S') ? "Sim" : "Não");
                        }


                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#FFEFD5';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvRiscoCorte_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvRiscoCorte.FooterRow;

            if (_footer != null)
            {
            }
        }
        protected void gvRiscoCorte_Sorting(object sender, GridViewSortEventArgs e)
        {
            int prod_processo = 0;
            prod_processo = Convert.ToInt32(ddlProducao.SelectedValue);

            IEnumerable<SP_OBTER_HB_FILAResult> filaHB = ObterHbFila(prod_processo, 'N');

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            string ordem = "";

            if (e.SortExpression == "HB")
                ordem = e.SortExpression + sortDirection + ", PROD_DETALHE";
            else
                ordem = e.SortExpression + sortDirection + ", HB, PROD_DETALHE";

            filaHB = filaHB.OrderBy(ordem);
            gvRiscoCorte.DataSource = filaHB;
            gvRiscoCorte.DataBind();

            hidOrdem.Value = e.SortExpression;
            hidOrdenacao.Value = sortDirection;

            if (filaHB.Count() <= 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        }
        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {

                int prod_processo = 0;

                prod_processo = Convert.ToInt32(ddlProducao.SelectedValue);

                IEnumerable<SP_OBTER_HB_FILAResult> filaHB = ObterHbFila(prod_processo, 'N');

                string ordem = "";

                if (hidOrdem.Value == "HB")
                    ordem = hidOrdem.Value + hidOrdenacao.Value + ", PROD_DETALHE";
                else
                    ordem = hidOrdem.Value + hidOrdenacao.Value + ", HB, PROD_DETALHE";


                if (filaHB.Count() > 0)
                {
                    filaHB = filaHB.OrderBy(ordem);

                    GerarRelatorio(filaHB, prod_processo);
                    labErro.Text = "Relatório gerado com sucesso.";
                }
                else
                {
                    labErro.Text = "Não existem HBs para o Corte.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }


            CarregarRiscoCorte(0);
        }
        private void GerarRelatorio(IEnumerable<SP_OBTER_HB_FILAResult> filaHB, int prod_processo)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FILA_RISCOCORTE_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(filaHB, prod_processo));
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
        private StringBuilder MontarRelatorioHTML(IEnumerable<SP_OBTER_HB_FILAResult> filaHB, int prod_processo)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, prod_processo);
            _texto = MontarRiscoCorte(_texto, filaHB);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto, int prod_processo)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            string dia_da_semana = dtfi.GetDayName(DateTime.Now.DayOfWeek);

            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Corte</title>   ");
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
            _texto.Append("     <div id='fichaRiscoCorte' align='center' style='border: 0px solid #000;'>");
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
            _texto.Append("                            HBF - " + ((prod_processo == 2) ? "RISCO" : "CORTE") + " - " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana));
            _texto.Append("                         </h2>");
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
        private StringBuilder MontarRiscoCorte(StringBuilder _texto, IEnumerable<SP_OBTER_HB_FILAResult> filaHB)
        {
            int total = 0;
            auxHB = 0;
            string corGrid = "#ffffff";


            if (filaHB.Count() > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                //_texto.Append("        <span style='font-size: 15px;'>#" + filaHB[0].TITULO + "</span>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='7'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                foreach (SP_OBTER_HB_FILAResult fila in filaHB)
                {

                    if (auxHB == 0)
                        auxHB = Convert.ToInt32(fila.HB);

                    if (auxHB != Convert.ToInt32(fila.HB))
                    {
                        if (trocouHB)
                        {
                            corGrid = "#ffffff";
                            trocouHB = false;
                        }
                        else
                        {
                            corGrid = "#cccccc";
                            trocouHB = true;
                        }
                    }

                    auxHB = Convert.ToInt32(fila.HB);


                    _texto.Append("            <tr class='" + ((corGrid == "#cccccc") ? "tdback" : "") + "'>");
                    _texto.Append("                <td style='width: 200px'>");
                    _texto.Append("                    HB " + fila.HB.ToString() + " - " + fila.DESC_DETALHE);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 100px'>");
                    _texto.Append("                    " + fila.GRUPO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.NOME);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 105px'>");
                    _texto.Append("                    " + prodController.ObterCoresBasicas(fila.COR).DESC_COR.Trim());
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 70px'>");
                    _texto.Append("                    " + fila.DATA_INICIO_PROCESSO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 70px'>");
                    _texto.Append("                    " + fila.GRADE_TOTAL);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right; width: 50px;'>");
                    _texto.Append("                    " + ((fila.LIQUIDAR == 'S') ? "LIQ" : "-"));
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");

                    total += Convert.ToInt32(fila.GRADE_TOTAL);
                }

                _texto.Append("            <tr>");
                _texto.Append("                <td style=''>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style=''>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style=''>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style=''>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style=''>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style=''>");
                _texto.Append("                    &nbsp;");
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

