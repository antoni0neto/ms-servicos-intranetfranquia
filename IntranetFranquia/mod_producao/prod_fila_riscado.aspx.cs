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

namespace Relatorios
{
    public partial class prod_fila_riscado : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int colunaPrincipal, colunaDetalhe = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPesquisa.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

            if (!Page.IsPostBack)
            {
                txtDataPesquisa.Text = DateTime.Now.ToString("dd/MM/yyyy");
                CarregarPrincipal(DateTime.Now.Date);
                CarregarDetalhe(DateTime.Now.Date);
            }
        }

        #region "PRINCIPAL"
        private void CarregarPrincipal(DateTime dataPesquisa)
        {
            List<PROD_HB> _hb = prodController.ObterHBFilaRiscado(dataPesquisa.Date).Where(x => x.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
            if (_hb != null)
            {
                gvPrincipal.DataSource = _hb;
                gvPrincipal.DataBind();
                if (_hb.Count <= 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            }
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _HB = e.Row.DataItem as PROD_HB;

                    colunaPrincipal += 1;
                    if (_HB != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaPrincipal.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_HB.COLECAO)).DESC_COLECAO;

                        Literal _litTipo = e.Row.FindControl("litTipo") as Literal;
                        if (_litTipo != null)
                            _litTipo.Text = (_HB.MOSTRUARIO == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO";

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                        {
                            PROD_HB_GRADE _grade = prodController.ObterGradeHB(_HB.CODIGO, 2);
                            if (_grade != null)
                                _qtde.Text = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG)).ToString();
                        }

                        Literal _dataIni = e.Row.FindControl("litDataInicial") as Literal;
                        if (_dataIni != null)
                        {
                            int processo = 0;
                            if (_HB.TIPO == 'A')
                                processo = 1;
                            else if (_HB.TIPO == 'R')
                                processo = 2;
                            else
                                processo = 3;

                            _dataIni.Text = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, processo).DATA_INICIO).ToString("dd/MM/yyyy HH:mm");
                        }
                        Literal _dataFim = e.Row.FindControl("litDataFinal") as Literal;
                        if (_dataFim != null)
                        {
                            _dataFim.Text = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 2).DATA_FIM).ToString("dd/MM/yyyy HH:mm");
                        }

                        Literal _tempo = e.Row.FindControl("litTempo") as Literal;
                        if (_tempo != null)
                        {
                            TimeSpan date = Convert.ToDateTime(_dataFim.Text) - Convert.ToDateTime(_dataIni.Text);
                            if (date.Days < 1)
                                _tempo.Text = date.Hours.ToString() + " horas";
                            else if (date.Days == 1)
                                _tempo.Text = date.Days.ToString() + " dia " + date.Hours.ToString() + "h";
                            else
                                _tempo.Text = date.Days.ToString() + " dias " + date.Hours.ToString() + "h";
                        }

                    }
                }
            }
        }
        protected void gvPrincipal_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPrincipal.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvPrincipal.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[6].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        #endregion

        #region "DETALHE"
        private void CarregarDetalhe(DateTime dataPesquisa)
        {
            List<PROD_HB> _hb = prodController.ObterHBFilaRiscadoDetalhe(dataPesquisa.Date).Where(x => x.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.DATA_INCLUSAO).ThenBy(p => p.PROD_DETALHE1.DESCRICAO).ToList();

            if (_hb != null)
            {
                _hb = _hb.Where(i => i.PROD_DETALHE != 4 && i.PROD_DETALHE != 5).ToList();
                gvDetalhe.DataSource = _hb;
                gvDetalhe.DataBind();
                if (_hb.Count <= 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionD').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionD').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            }
        }
        protected void gvDetalhe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _HB = e.Row.DataItem as PROD_HB;

                    colunaDetalhe += 1;
                    if (_HB != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaDetalhe.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_HB.COLECAO)).DESC_COLECAO;

                        Literal _litTipo = e.Row.FindControl("litTipo") as Literal;
                        if (_litTipo != null)
                            _litTipo.Text = (_HB.MOSTRUARIO == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO";

                        Literal _detalhe = e.Row.FindControl("litDetalhe") as Literal;
                        if (_detalhe != null)
                            _detalhe.Text = _HB.PROD_DETALHE1.DESCRICAO;

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                        {
                            PROD_HB_GRADE _grade = prodController.ObterGradeHB(Convert.ToInt32(_HB.CODIGO), 2);
                            if (_grade != null)
                                _qtde.Text = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG)).ToString();
                        }

                        Literal _dataIni = e.Row.FindControl("litDataInicial") as Literal;
                        if (_dataIni != null)
                        {
                            _dataIni.Text = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 2).DATA_INICIO).ToString("dd/MM/yyyy HH:mm");
                        }
                        Literal _dataFim = e.Row.FindControl("litDataFinal") as Literal;
                        if (_dataFim != null)
                        {
                            _dataFim.Text = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 2).DATA_FIM).ToString("dd/MM/yyyy HH:mm");
                        }

                        Literal _tempo = e.Row.FindControl("litTempo") as Literal;
                        if (_tempo != null)
                        {
                            TimeSpan date = Convert.ToDateTime(_dataFim.Text) - Convert.ToDateTime(_dataIni.Text);
                            if (date.Days < 1)
                                _tempo.Text = date.Hours.ToString() + " horas";
                            else if (date.Days == 1)
                                _tempo.Text = date.Days.ToString() + " dia " + date.Hours.ToString() + "h";
                            else
                                _tempo.Text = date.Days.ToString() + " dias " + date.Hours.ToString() + "h";
                        }

                    }
                }
            }
        }
        protected void gvDetalhe_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvDetalhe.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvDetalhe.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[7].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        #endregion

        #region "BUSCAR"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_data;

            labMsg.Text = "";
            labErro.Text = "";
            if (!DateTime.TryParse(txtDataPesquisa.Text, out v_data))
            {
                labMsg.Text = "INFORME UMA DATA VÁLIDA.";
                return;
            }

            try
            {
                CarregarPrincipal(v_data);
                CarregarDetalhe(v_data);
            }
            catch (Exception ex)
            {
                labMsg.Text = "ERRO: " + ex.Message;
            }
        }
        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            int cont = 0;
            DateTime dataPesquisa = DateTime.Now;
            bool bMostruario = false;

            labMsg.Text = "";
            labErro.Text = "";
            if (!DateTime.TryParse(txtDataPesquisa.Text, out dataPesquisa))
            {
                labMsg.Text = "INFORME UMA DATA VÁLIDA.";
                return;
            }

            try
            {
                //Nao traz mostruario
                List<PROD_HB> _principal = prodController.ObterHBFilaRiscado(dataPesquisa.Date).Where(x => x.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
                List<PROD_HB> _detalhe = prodController.ObterHBFilaRiscadoDetalhe(dataPesquisa.Date).Where(x => x.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.DATA_INCLUSAO).ThenBy(p => p.PROD_DETALHE1.DESCRICAO).ToList();
                // 12/12/2014 - ALTERACAO - NÃO MOSTRAR GALAO
                _detalhe = _detalhe.Where(i => i.PROD_DETALHE != 4 && i.PROD_DETALHE != 5).ToList();

                bMostruario = (chkMostruario.Checked) ? true : false;

                List<Fila> _filaPrincipal = new List<Fila>();
                foreach (PROD_HB _prod_hb in _principal)
                {
                    _filaPrincipal.Add(new Fila
                    {
                        HB = Convert.ToInt32(_prod_hb.HB),
                        COLECAO = (new BaseController().BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO,
                        GRUPO = _prod_hb.GRUPO,
                        NOME = _prod_hb.NOME,
                        TIPO = "PRINCIPAL",
                        COR = prodController.ObterCoresBasicas(_prod_hb.COR).DESC_COR,
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 2).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(_prod_hb.CODIGO, 2),
                        TITULO = "PRINCIPAL"
                    });
                    cont += 1;
                }

                List<Fila> _filaDetalhe = new List<Fila>();
                foreach (PROD_HB _prod_hb in _detalhe)
                {
                    _filaDetalhe.Add(new Fila
                    {
                        HB = Convert.ToInt32(_prod_hb.HB),
                        COLECAO = (new BaseController().BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO,
                        GRUPO = _prod_hb.GRUPO,
                        NOME = _prod_hb.NOME,
                        TIPO = _prod_hb.PROD_DETALHE1.DESCRICAO,
                        COR = prodController.ObterCoresBasicas(_prod_hb.COR).DESC_COR,
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 2).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(Convert.ToInt32(_prod_hb.CODIGO), 2),
                        TITULO = "DETALHES"
                    });
                    cont += 1;
                }

                if (cont > 0)
                {
                    GerarRelatorio(_filaPrincipal, _filaDetalhe, dataPesquisa, bMostruario);
                    labErro.Text = "Relatório gerado com sucesso.";
                }
                else
                {
                    labErro.Text = "Não existem HBs cortados na data escolhida.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            CarregarPrincipal(dataPesquisa.Date);
            CarregarDetalhe(dataPesquisa.Date);
        }
        private void GerarRelatorio(List<Fila> _filaPrincipal, List<Fila> _filaDetalhe, DateTime dataPesquisa, bool bMostruario)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FILA_RISCADO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_filaPrincipal, _filaDetalhe, dataPesquisa, bMostruario));
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
        private StringBuilder MontarRelatorioHTML(List<Fila> _filaPrincipal, List<Fila> _filaDetalhe, DateTime dataPesquisa, bool bMostruario)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, dataPesquisa, bMostruario);
            _texto = MontarRiscado(_texto, _filaPrincipal);
            _texto = MontarRiscado(_texto, _filaDetalhe);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto, DateTime dataPesquisa, bool bMostruario)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            string dia_da_semana = dtfi.GetDayName(dataPesquisa.DayOfWeek);

            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Riscado</title>   ");
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
            _texto.Append("     <div id='fichaRisco' align='center' style='border: 0px solid #000;'>");
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
            _texto.Append("                         " + ((bMostruario) ? "MOSTRUÁRIO RISCADO" : "PRODUÇÃO RISCADO") + " - " + dataPesquisa.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana) + "</h2>");
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
        private StringBuilder MontarRiscado(StringBuilder _texto, List<Fila> _fila)
        {
            int total = 0;

            if (_fila.Count > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                _texto.Append("        <span style='font-size: 15px;'>#" + _fila[0].TITULO + "</span>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='7'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                foreach (Fila fila in _fila)
                {
                    _texto.Append("            <tr>");
                    _texto.Append("                <td style='width: 123px'>");
                    _texto.Append("                    " + fila.COLECAO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 115px'>");
                    if (fila.TITULO == "DETALHES")
                        _texto.Append("                    HB " + fila.HB.ToString() + " - " + fila.TIPO);
                    else
                        _texto.Append("                    HB " + fila.HB.ToString());
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 100px'>");
                    _texto.Append("                    " + fila.GRUPO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.NOME);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 105px'>");
                    _texto.Append("                    " + fila.COR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 70px'>");
                    _texto.Append("                    " + fila.DATA);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right; width: 35px;'>");
                    _texto.Append("                    " + fila.QUANTIDADE);
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");

                    total += fila.QUANTIDADE;
                }

                _texto.Append("            <tr>");
                _texto.Append("                <td style='width: 123px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 115px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 100px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 105px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 70px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right; width: 35px;''>");
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

