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
    public partial class prod_fila_risco : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Recarregar();
            }
        }

        private void Recarregar()
        {
            CarregarAmpliacao();
            CarregarRisco();
            CarregarDetalhe();

            //CarregarRiscoProducao();
        }

        #region "AMPLIAÇÃO"
        private void CarregarAmpliacao()
        {
            List<PROD_HB> _hb = prodController.ObterHBFila(1, 'N');
            if (_hb != null)
            {
                gvAmpliacao.DataSource = _hb;
                gvAmpliacao.DataBind();
                if (_hb.Count <= 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionA').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionA').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            }
        }
        protected void gvAmpliacao_RowDataBound(object sender, GridViewRowEventArgs e)
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

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                        {
                            PROD_HB_GRADE _grade = prodController.ObterGradeHB(_HB.CODIGO, 1);
                            if (_grade != null)
                                _qtde.Text = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG)).ToString();
                        }

                        Literal _litDataInicial = e.Row.FindControl("litDataInicial") as Literal;
                        DateTime _DataInicial = DateTime.Now;
                        if (_litDataInicial != null)
                        {
                            _DataInicial = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 1).DATA_INICIO);
                            _litDataInicial.Text = _DataInicial.ToString("dd/MM/yyyy HH:mm");
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

                        Button _btRisco = e.Row.FindControl("btRisco") as Button;
                        if (_btRisco != null)
                            _btRisco.CommandArgument = _HB.CODIGO.ToString();
                    }
                }
            }
        }
        protected void gvAmpliacao_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvAmpliacao.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvAmpliacao.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[5].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void btRisco_Click(object sender, EventArgs e)
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
                            /*
                            string codigo = b.CommandArgument;
                            AtualizarProcessoDataFim(Convert.ToInt32(codigo), 1, data);

                            //Inserir no processo do PRINCIPAL
                            PROD_HB_PROD_PROCESSO _processo = null;
                            _processo = new PROD_HB_PROD_PROCESSO();
                            _processo.PROD_HB = Convert.ToInt32(codigo);
                            _processo.PROD_PROCESSO = ObterProcessoOrdem(2); //Vai da Ampliação para o RISCO

                            string horaAtual = DateTime.Now.ToString("HH:mm:ss");
                            string dataBaixa = data.ToString("yyyy-MM-dd ");
                            _processo.DATA_INICIO = Convert.ToDateTime(dataBaixa + horaAtual);
                            IncluirProcesso(_processo);

                            //Obter grade do processo ampliação
                            PROD_HB_GRADE grade = prodController.ObterGradeHB(Convert.ToInt32(codigo), 1);
                            //Inserir grade
                            PROD_HB_GRADE _novo = new PROD_HB_GRADE();
                            _novo.PROD_HB = Convert.ToInt32(codigo);
                            _novo.PROD_PROCESSO = 2;
                            _novo.GRADE_EXP = grade.GRADE_EXP;
                            _novo.GRADE_XP = grade.GRADE_XP;
                            _novo.GRADE_PP = grade.GRADE_PP;
                            _novo.GRADE_P = grade.GRADE_P;
                            _novo.GRADE_M = grade.GRADE_M;
                            _novo.GRADE_G = grade.GRADE_G;
                            _novo.GRADE_GG = grade.GRADE_GG;
                            _novo.DATA_INCLUSAO = DateTime.Now;
                            _novo.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            IncluirGrade(_novo);
                            */

                            string codigo = b.CommandArgument;
                            string sData = data.ToString("dd/MM/yyyy").Replace("/", "@");
                            //Redirecionar para tela de baixa
                            if (codigo != "" && sData != "")
                                Response.Redirect("prod_cad_hb.aspx?t=3&c=" + codigo + "&d=" + sData);
                            else
                                labErro.Text = "Não foi possível localizar o HB. Entre em contato com TI.";

                        }

                        //Recarregar();
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
        private void IncluirGrade(PROD_HB_GRADE _grade)
        {
            prodController.InserirGrade(_grade);
        }
        #endregion

        #region "RISCO"
        private void CarregarRisco()
        {
            List<PROD_HB> _hb = prodController.ObterHBFila(2, 'N');
            if (_hb != null)
            {
                gvRisco.DataSource = _hb;
                gvRisco.DataBind();
                if (_hb.Count <= 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            }
        }
        protected void gvRisco_RowDataBound(object sender, GridViewRowEventArgs e)
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

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                        {
                            PROD_HB_GRADE _grade = prodController.ObterGradeHB(_HB.CODIGO, 2);
                            if (_grade != null)
                                _qtde.Text = (Convert.ToInt32(_grade.GRADE_EXP) + Convert.ToInt32(_grade.GRADE_XP) + Convert.ToInt32(_grade.GRADE_PP) + Convert.ToInt32(_grade.GRADE_P) + Convert.ToInt32(_grade.GRADE_M) + Convert.ToInt32(_grade.GRADE_G) + Convert.ToInt32(_grade.GRADE_GG)).ToString();
                        }

                        Literal _litDataInicial = e.Row.FindControl("litDataInicial") as Literal;
                        DateTime _DataInicial = DateTime.Now;
                        if (_litDataInicial != null)
                        {
                            _DataInicial = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 2).DATA_INICIO);
                            _litDataInicial.Text = _DataInicial.ToString("dd/MM/yyyy HH:mm");
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

                        Button _btBaixar = e.Row.FindControl("btBaixarRisco") as Button;
                        if (_btBaixar != null)
                        {
                            _btBaixar.CommandArgument = _HB.CODIGO.ToString();
                        }
                    }
                }
            }
        }
        protected void gvRisco_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvRisco.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvRisco.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[5].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void btBaixarRisco_Click(object sender, EventArgs e)
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
                        string codigo = b.CommandArgument;
                        string data = t.Text.Replace("/", "@");
                        //Redirecionar para tela de baixa
                        if (codigo != "" && data != "")
                            Response.Redirect("prod_cad_hb_baixa.aspx?c=" + codigo + "&d=" + data + "&t=2");
                        else
                            labErro.Text = "Não foi possível localizar o HB. Entre em contato com TI.";
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

        #region "DETALHE"
        private void CarregarDetalhe()
        {
            List<PROD_HB> _hb = prodController.ObterHBFilaDetalhe(2, 'N');
            if (_hb != null)
            {
                // 12/12/2014 - ALTERACAO - NÃO MOSTRAR GALAO
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

                        Literal _litDataInicial = e.Row.FindControl("litDataInicial") as Literal;
                        DateTime _DataInicial = DateTime.Now;
                        if (_litDataInicial != null)
                        {
                            _DataInicial = Convert.ToDateTime(prodController.ObterProcessoHB(_HB.CODIGO, 2).DATA_INICIO);
                            _litDataInicial.Text = _DataInicial.ToString("dd/MM/yyyy HH:mm");
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

                        Button _btBaixar = e.Row.FindControl("btBaixarDetalhe") as Button;
                        if (_btBaixar != null)
                            _btBaixar.CommandArgument = _HB.CODIGO.ToString();
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
                _footer.Cells[6].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void btBaixarDetalhe_Click(object sender, EventArgs e)
        {
            DateTime dataValidador;
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
                    if (t != null && DateTime.TryParse(t.Text, out dataValidador))
                    {
                        string codigo = b.CommandArgument;
                        string data = t.Text.Replace("/", "@");
                        //Redirecionar para tela de baixa
                        if (codigo != "" && data != "")
                            Response.Redirect("prod_cad_hb_baixa_det.aspx?c=" + codigo + "&d=" + data + "&t=2");
                        else
                            labErro.Text = "Não foi possível localizar o HB. Entre em contato com TI.";
                    }
                    else
                    {
                        labErro.Text = "Não foi possível obter a DATA.";
                    }
                }
                else
                {
                    labErro.Text = "Não foi possível obter o CÓDIGO.";
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
                List<PROD_HB> _ampliacao = prodController.ObterHBFila(1, 'N').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
                List<PROD_HB> _risco = prodController.ObterHBFila(2, 'N').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
                List<PROD_HB> _detalhe = prodController.ObterHBFilaDetalhe(2, 'N').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.PROD_DETALHE1.DESCRICAO).ToList();
                // 12/12/2014 - ALTERACAO - NÃO MOSTRAR GALAO
                _detalhe = _detalhe.Where(i => i.PROD_DETALHE != 4 && i.PROD_DETALHE != 5).ToList();

                int cont = 0;

                List<Fila> _filaAmpliacao = new List<Fila>();
                foreach (PROD_HB _prod_hb in _ampliacao)
                {
                    _filaAmpliacao.Add(new Fila
                    {
                        HB = Convert.ToInt32(_prod_hb.HB),
                        COLECAO = (new BaseController().BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO,
                        GRUPO = _prod_hb.GRUPO,
                        NOME = _prod_hb.NOME,
                        TIPO = "AMPLIAÇÃO",
                        COR = prodController.ObterCoresBasicas(_prod_hb.COR).DESC_COR,
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 1).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(_prod_hb.CODIGO, 1),
                        TITULO = "AMPLIAÇÃO"
                    });
                    cont += 1;
                }

                List<Fila> _filaRisco = new List<Fila>();
                foreach (PROD_HB _prod_hb in _risco)
                {
                    _filaRisco.Add(new Fila
                    {
                        HB = Convert.ToInt32(_prod_hb.HB),
                        COLECAO = (new BaseController().BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO,
                        GRUPO = _prod_hb.GRUPO,
                        NOME = _prod_hb.NOME,
                        TIPO = "RISCO",
                        COR = prodController.ObterCoresBasicas(_prod_hb.COR).DESC_COR,
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 2).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(_prod_hb.CODIGO, 2),
                        TITULO = "RISCO"
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
                        QUANTIDADE = prodController.ObterQtdeGradeHB(_prod_hb.CODIGO, 2),
                        TITULO = "DETALHES"
                    });
                    cont += 1;
                }



                if (cont > 0)
                {

                    GerarRelatorio(_filaAmpliacao, _filaRisco, _filaDetalhe);
                    labErro.Text = "Relatório gerado com sucesso.";
                }
                else
                {
                    labErro.Text = "Não existem HBs para Ampliação ou Risco.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            Recarregar();
        }
        private void GerarRelatorio(List<Fila> filaAmpliacao, List<Fila> filaRisco, List<Fila> filaRiscoDetalhe)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FILA_RISCO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(filaAmpliacao, filaRisco, filaRiscoDetalhe));
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
        private StringBuilder MontarRelatorioHTML(List<Fila> filaAmpliacao, List<Fila> filaRisco, List<Fila> filaRiscoDetalhe)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarRisco(_texto, filaAmpliacao);
            _texto = MontarRisco(_texto, filaRisco);
            _texto = MontarRisco(_texto, filaRiscoDetalhe);
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
            _texto.Append("         <title>Risco</title>   ");
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
            _texto.Append("                            HBF - RISCO PENDENTE - " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana));
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
        private StringBuilder MontarRisco(StringBuilder _texto, List<Fila> filaHB)
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

                    _texto.Append("            <tr style=''>");
                    _texto.Append("                <td style='width: 110px'>");
                    _texto.Append("                    " + fila.COLECAO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 145px'>");
                    if (fila.TITULO == "DETALHES")
                        _texto.Append("                    HB " + fila.HB.ToString() + " - " + fila.TIPO);
                    else
                        _texto.Append("                    HB " + fila.HB.ToString());
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 85px'>");
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
                _texto.Append("                <td style='width: 110px'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("                <td style='width: 145px'>");
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
        //    var filaRisco = prodController.ObterHBFilaPrincipalDetalhe(prod_processo, mostruario);

        //    filaRisco = filaRisco.Where(p => p.PROD_DETALHE != 4 && p.PROD_DETALHE != 5).ToList();

        //    return filaRisco;
        //}
        //private void CarregarRiscoProducao()
        //{
        //    List<SP_OBTER_HB_FILAResult> _hb = ObterHbFila(2, 'N');
        //    if (_hb != null)
        //    {
        //        gvRiscoProducao.DataSource = _hb;
        //        gvRiscoProducao.DataBind();
        //        if (_hb.Count <= 0)
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        //        else
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        //    }
        //}
        //protected void gvRiscoProducao_RowDataBound(object sender, GridViewRowEventArgs e)
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
        //            }
        //        }
        //    }

        //    if (e.Row.DataItemIndex != -1)
        //    {
        //        e.Row.Attributes.Add("onMouseover", "this.style.background='#FFEFD5';this.style.cursor='hand'");
        //        e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
        //    }
        //}
        //protected void gvRiscoProducao_DataBound(object sender, EventArgs e)
        //{
        //    GridViewRow _footer = gvRiscoProducao.FooterRow;
        //    int total;
        //    string detalhe = "";
        //    if (_footer != null)
        //    {
        //        total = 0;
        //        foreach (GridViewRow g in gvRiscoProducao.Rows)
        //        {
        //            detalhe = g.Cells[3].Text.Trim().Trim();
        //            if (detalhe != "GALÃO PRÓPRIO" && detalhe != "GALÃO" && detalhe != "GAL&#195;O" && detalhe != "GAL&#195;O PR&#211;PRIO")
        //            {
        //                total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;
        //            }
        //        }
        //        _footer.Cells[1].Text = "Total";
        //        _footer.Cells[6].Text = total.ToString();
        //        _footer.Font.Bold = true;
        //    }
        //}
        //protected void gvRiscoProducao_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    IEnumerable<SP_OBTER_HB_FILAResult> filaHB = ObterHbFila(2, 'N');

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
        //    gvRiscoProducao.DataSource = filaHB;
        //    gvRiscoProducao.DataBind();

        //    hidOrdem.Value = e.SortExpression;
        //    hidOrdenacao.Value = sortDirection;

        //    if (filaHB.Count() <= 0)
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        //    else
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionT').accordion({ collapsible: true, heightStyle: 'content' });});", true);
        //}

    }
}

