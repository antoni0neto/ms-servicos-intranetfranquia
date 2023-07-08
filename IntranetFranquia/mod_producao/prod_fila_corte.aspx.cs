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
    public partial class prod_fila_corte : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarPrincipal();
                CarregarDetalhe();

                //CarregarCorte();
            }
        }

        protected void btVoltarRisco_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoHB = 0;

                ImageButton bt = (ImageButton)sender;

                if (bt != null)
                {
                    codigoHB = Convert.ToInt32(bt.CommandArgument);
                    //Voltar registro principal
                    prodController.VoltarProcessoRisco(codigoHB);

                    List<PROD_HB> _detalhes = new List<PROD_HB>();
                    //Obter Detalhes 
                    _detalhes.Add(prodController.ObterDetalhesHB(codigoHB, 4)); // Galão
                    _detalhes.Add(prodController.ObterDetalhesHB(codigoHB, 5)); // Galão Próprio

                    foreach (PROD_HB d in _detalhes)
                        if (d != null)
                            prodController.VoltarProcessoRisco(d.CODIGO);


                    CarregarPrincipal();
                    CarregarDetalhe();

                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "PRINCIPAL"
        private void CarregarPrincipal()
        {
            List<PROD_HB> _hb = prodController.ObterHBFila(3, 'N');
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

                    if (_HB != null)
                    {

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_HB.COLECAO)).DESC_COLECAO;

                        //HiddenField _hidPROD_DETALHE = e.Row.FindControl("hidPROD_DETALHE") as HiddenField;
                        //if (_hidPROD_DETALHE != null)
                        //{
                        //    if (_hidPROD_DETALHE.Value.ToString().Trim() == "")
                        //    {
                        //        sDetalhe = "0";
                        //    }
                        //    else
                        //    {
                        //        sDetalhe = _hidPROD_DETALHE.Value.ToString();
                        //    }
                        //}

                        //if (Convert.ToInt32(sDetalhe) == 5)
                        //{
                        //    e.Row.BackColor = Color.Lavender;
                        //}

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                        {
                            PROD_HB_GRADE _grade = prodController.ObterGradeHB(_HB.CODIGO, 2);
                            if (_grade != null)
                            {
                                _qtde.Text = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG)).ToString();
                            }
                        }

                        Literal _data = e.Row.FindControl("litData") as Literal;
                        DateTime _DataInicial = DateTime.Now;
                        if (_data != null)
                        {
                            _DataInicial = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 3).DATA_INICIO);
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

                        TextBox t = e.Row.FindControl("txtDataBaixa") as TextBox;
                        if (t != null)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + t.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date('" + _DataInicial.Year + "', '" + (_DataInicial.Month - 1).ToString() + "', '" + _DataInicial.Day + "'), maxDate: new Date() });});", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + t.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _HB.CODIGO.ToString();

                        ImageButton _btVoltarRisco = e.Row.FindControl("btVoltarRisco") as ImageButton;
                        if (_btVoltarRisco != null)
                            _btVoltarRisco.CommandArgument = _HB.CODIGO.ToString();
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

                _footer.Cells[2].Text = "Total";
                _footer.Cells[6].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        #endregion

        #region "DETALHE"
        private void CarregarDetalhe()
        {
            List<PROD_HB> _hb = prodController.ObterHBFilaDetalhe(3, 'N');
            if (_hb != null)
            {
                // 12/12/2014 - ALTERACAO - NÃO MOSTRAR GALAO
                //_hb = _hb.Where(i => i.PROD_DETALHE != 4 && i.PROD_DETALHE != 5).ToList();

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

                    if (_HB != null)
                    {

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_HB.COLECAO)).DESC_COLECAO;

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

                        Literal _data = e.Row.FindControl("litData") as Literal;
                        DateTime _DataInicial = DateTime.Now;
                        if (_data != null)
                        {
                            _DataInicial = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 3).DATA_INICIO);
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

                        TextBox t = e.Row.FindControl("txtDataBaixa") as TextBox;
                        if (t != null)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + t.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date('" + _DataInicial.Year + "', '" + (_DataInicial.Month - 1).ToString() + "', '" + _DataInicial.Day + "'), maxDate: new Date() });});", true);
                        // ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + t.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _HB.CODIGO.ToString();

                        ImageButton _btVoltarRisco = e.Row.FindControl("btVoltarRisco") as ImageButton;
                        if (_btVoltarRisco != null)
                            _btVoltarRisco.CommandArgument = _HB.CODIGO.ToString();

                    }
                }
            }
        }
        protected void gvDetalhe_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvDetalhe.FooterRow;
            int total;
            string det = "";

            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvDetalhe.Rows)
                {

                    det = ((Literal)g.FindControl("litDetalhe")).Text;

                    if (det != "GALÃO PRÓPRIO" && det != "GALÃO" && det != "" && det != "")
                        total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;
                }

                _footer.Cells[2].Text = "Total";
                _footer.Cells[7].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        #endregion

        #region "BAIXAR"
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                //Obter controle do botão
                Button b = (Button)sender;
                if (b != null)
                {
                    //Obter Linha do botão clicado
                    GridViewRow g = (GridViewRow)b.NamingContainer;
                    //Obter data informada no textbox
                    TextBox t = g.FindControl("txtDataBaixa") as TextBox;
                    if (t != null)
                    {
                        DateTime data;
                        if (DateTime.TryParse(t.Text, out data))
                        {
                            string codigo = b.CommandArgument;
                            //Redirecionar para tela de baixa
                            if (codigo != "")
                            {
                                Literal lit = g.FindControl("litDetalhe") as Literal;
                                if (lit == null)
                                    Response.Redirect("prod_cad_hb_baixa_corte.aspx?c=" + codigo + "&d=" + data.ToString("dd/MM/yyyy").Replace("/", "@") + "&t=3");
                                else
                                    Response.Redirect("prod_cad_hb_baixa_det.aspx?c=" + codigo + "&d=" + data.ToString("dd/MM/yyyy").Replace("/", "@") + "&t=3");

                            }
                            else
                            {
                                labErro.Text = "Não foi possível localizar o HB. Entre em contato com TI.";
                            }
                        }
                        else
                        {
                            labErro.Text = "Não foi possível converter a DATA. Entre em contato com TI.";
                        }
                    }
                    else
                    {
                        labErro.Text = "Não foi possível obter a DATA. Entre em contato com TI.";
                    }
                }
                else
                {
                    labErro.Text = "Não foi possível obter o CÓDIGO. Entre em contato com TI.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                List<PROD_HB> _principal = prodController.ObterHBFila(3, 'N').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
                List<PROD_HB> _detalhe = prodController.ObterHBFilaDetalhe(3, 'N').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.PROD_DETALHE1.DESCRICAO).ToList();
                //12/12/2014 - ALTERACAO - NÃO MOSTRAR GALAO
                _detalhe = _detalhe.Where(i => i.PROD_DETALHE != 4 && i.PROD_DETALHE != 5).ToList();

                int cont = 0;

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
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 3).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
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
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 3).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(Convert.ToInt32(_prod_hb.CODIGO), 2),
                        TITULO = "DETALHES"
                    });
                    cont += 1;
                }


                if (cont > 0)
                {
                    GerarRelatorio(_filaPrincipal, _filaDetalhe);
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
            CarregarPrincipal();
            CarregarDetalhe();
        }
        private void GerarRelatorio(List<Fila> filaPrincipal, List<Fila> filaDetalhe)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FILA_CORTE_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(filaPrincipal, filaDetalhe));
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
        private StringBuilder MontarRelatorioHTML(List<Fila> filaPrincipal, List<Fila> filaDetalhe)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarCorte(_texto, filaPrincipal);
            _texto = MontarCorte(_texto, filaDetalhe);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
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
            _texto.Append("     <div id='fichaCorte' align='center' style='border: 0px solid #000;'>");
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
            _texto.Append("                            HBF - CORTE - " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana));
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
        private StringBuilder MontarCorte(StringBuilder _texto, List<Fila> filaHB)
        {
            int total = 0;

            if (filaHB.Count() > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                _texto.Append("        <span style='font-size: 15px;'>#" + filaHB[0].TITULO + "</span>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='7'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                foreach (Fila fila in filaHB)
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

                    total += Convert.ToInt32(fila.QUANTIDADE);
                }

                _texto.Append("            <tr>");
                _texto.Append("                <td style='width: 123px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 115px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 85px'>");
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
                _texto.Append("                <td style='text-align: right; width: 35px;'>");
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

        //private List<SP_OBTER_HB_FILAResult> ObterHbFila(int prod_processo, char mostruario)
        //{
        //    return prodController.ObterHBFilaPrincipalDetalhe(prod_processo, mostruario);
        //}

        //private void CarregarCorte()
        //{
        //    List<SP_OBTER_HB_FILAResult> _hb = ObterHbFila(3, 'N');
        //    if (_hb != null)
        //    {
        //        gvCorte.DataSource = _hb;
        //        gvCorte.DataBind();
        //        if (_hb.Count <= 0)
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        //        else
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        //    }
        //}
        //protected void gvCorte_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.DataItem != null)
        //        {
        //            SP_OBTER_HB_FILAResult _HB = e.Row.DataItem as SP_OBTER_HB_FILAResult;

        //            if (_HB != null)
        //            {
        //                if (auxHB == 0)
        //                    auxHB = Convert.ToInt32(_HB.HB);

        //                if (auxHB != Convert.ToInt32(_HB.HB))
        //                {
        //                    if (trocouHB)
        //                    {
        //                        corGrid = Color.White;
        //                        trocouHB = false;
        //                    }
        //                    else
        //                    {
        //                        corGrid = Color.LightGray;
        //                        trocouHB = true;
        //                    }
        //                }

        //                e.Row.BackColor = corGrid;
        //                auxHB = Convert.ToInt32(_HB.HB);

        //                Literal _qtde = e.Row.FindControl("litQtde") as Literal;
        //                if (_qtde != null)
        //                {
        //                    _qtde.Text = _HB.GRADE_TOTAL.ToString();
        //                }

        //                Literal _data = e.Row.FindControl("litData") as Literal;
        //                DateTime _DataInicial = DateTime.Now;
        //                if (_data != null)
        //                {
        //                    _DataInicial = Convert.ToDateTime(_HB.DATA_INICIO_PROCESSO);
        //                    _data.Text = _DataInicial.ToString("dd/MM/yyyy HH:mm");
        //                }

        //                Literal _tempo = e.Row.FindControl("litTempo") as Literal;
        //                if (_tempo != null)
        //                {
        //                    TimeSpan date = DateTime.Now - _DataInicial;
        //                    if (date.Days < 1)
        //                        _tempo.Text = date.Hours.ToString() + " horas";
        //                    else if (date.Days == 1)
        //                        _tempo.Text = date.Days.ToString() + " dia " + date.Hours.ToString() + "h";
        //                    else
        //                        _tempo.Text = date.Days.ToString() + " dias " + date.Hours.ToString() + "h";
        //                }

        //                TextBox t = e.Row.FindControl("txtDataBaixa") as TextBox;
        //                if (t != null)
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + t.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', minDate: new Date('" + _DataInicial.Year + "', '" + (_DataInicial.Month - 1).ToString() + "', '" + _DataInicial.Day + "'), maxDate: new Date() });});", true);

        //                Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
        //                if (_btBaixar != null)
        //                    _btBaixar.CommandArgument = _HB.CODIGO.ToString();

        //                ImageButton _btVoltarRisco = e.Row.FindControl("btVoltarRisco") as ImageButton;
        //                if (_btVoltarRisco != null)
        //                    _btVoltarRisco.CommandArgument = _HB.CODIGO.ToString();
        //            }
        //        }
        //    }

        //    if (e.Row.DataItemIndex != -1)
        //    {
        //        e.Row.Attributes.Add("onMouseover", "this.style.background='#FFEFD5';this.style.cursor='hand'");
        //        e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
        //    }
        //}
        //protected void gvCorte_DataBound(object sender, EventArgs e)
        //{
        //    GridViewRow _footer = gvCorte.FooterRow;
        //    int total;
        //    string detalhe = "";

        //    if (_footer != null)
        //    {
        //        total = 0;
        //        foreach (GridViewRow g in gvCorte.Rows)
        //        {
        //            detalhe = g.Cells[4].Text.Trim().Trim();
        //            if (detalhe != "GALÃO PRÓPRIO" && detalhe != "GALÃO" && detalhe != "GAL&#195;O" && detalhe != "GAL&#195;O PR&#211;PRIO")
        //            {
        //                total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;
        //            }
        //        }

        //        _footer.Cells[2].Text = "Total";
        //        _footer.Cells[7].Text = total.ToString();
        //        _footer.Font.Bold = true;
        //    }
        //}
        //protected void gvCorte_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    IEnumerable<SP_OBTER_HB_FILAResult> filaHB = ObterHbFila(3, 'N');

        //    string sortExpression = e.SortExpression;
        //    SortDirection sort;

        //    Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

        //    if (sort == SortDirection.Ascending)
        //        Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
        //    else
        //        Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

        //    string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

        //    string ordem = "";

        //    if (e.SortExpression == "HB")
        //        ordem = e.SortExpression + sortDirection + ", PROD_DETALHE";
        //    else
        //        ordem = e.SortExpression + sortDirection + ", HB, PROD_DETALHE";

        //    filaHB = filaHB.OrderBy(ordem);
        //    gvCorte.DataSource = filaHB;
        //    gvCorte.DataBind();

        //    hidOrdem.Value = e.SortExpression;
        //    hidOrdenacao.Value = sortDirection;

        //    if (filaHB.Count() <= 0)
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        //    else
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        //}



    }
}

