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
    public partial class prod_fila_mostruario : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        int colunaAmpliacao, colunaRisco, colunaCorte = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarAmpliacao();
                CarregarRisco();
                CarregarCorte();
            }
        }

        #region "AMPLIACAO"
        private void CarregarAmpliacao()
        {
            List<PROD_HB> _hb = prodController.ObterHBFila(1, 'S');
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

                    colunaAmpliacao += 1;
                    if (_HB != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaAmpliacao.ToString();

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
        #endregion

        #region "RISCO"
        private void CarregarRisco()
        {
            List<PROD_HB> _hb = prodController.ObterHBFila(2, 'S');
            if (_hb != null)
            {
                // 12/12/2014 - ALTERACAO - NÃO MOSTRAR GALAO E GALAO PROPRIO
                _hb = _hb.Where(i => i.PROD_DETALHE != 5).ToList();

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

                    colunaRisco += 1;
                    if (_HB != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaRisco.ToString();

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

        #region "CORTE"
        private void CarregarCorte()
        {
            List<PROD_HB> _hb = prodController.ObterHBFila(3, 'S');
            List<PROD_HB> _hbdetalhe = prodController.ObterHBFilaDetalhe(3, 'S');

            List<PROD_HB> lista = new List<PROD_HB>();
            lista.AddRange(_hb);
            lista.AddRange(_hbdetalhe);

            List<PROD_HB> corte = new List<PROD_HB>();
            foreach (PROD_HB prod_hb in lista)
            {
                if (corte.Find(p => p.COLECAO.Trim() == prod_hb.COLECAO.Trim() && p.HB == prod_hb.HB && p.MOSTRUARIO == 'S') == null)
                {
                    corte.Add(prod_hb);
                }
            }

            if (corte != null)
            {
                gvCorte.DataSource = corte;
                gvCorte.DataBind();
                if (corte.Count <= 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionC').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionC').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            }
        }
        protected void gvCorte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _HB = e.Row.DataItem as PROD_HB;

                    colunaCorte += 1;
                    if (_HB != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaCorte.ToString();

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
                        {
                            _btBaixar.CommandArgument = (_HB.CODIGO_PAI == null) ? _HB.CODIGO.ToString() : _HB.CODIGO_PAI.ToString();
                        }
                    }
                }
            }
        }
        protected void gvCorte_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvCorte.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvCorte.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[5].Text = total.ToString();
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
                                    Response.Redirect("prod_cad_hb_baixa.aspx?c=" + codigo + "&d=" + data.ToString("dd/MM/yyyy").Replace("/", "@") + "&t=3");
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
            int cont = 0;
            try
            {
                List<PROD_HB> _Ampliacao = prodController.ObterHBFila(1, 'S').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
                List<PROD_HB> _Risco = prodController.ObterHBFila(2, 'S').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();

                /* -- OBTER CORTE COM DETALHE -- */
                List<PROD_HB> _Corte = prodController.ObterHBFila(3, 'S').OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
                List<PROD_HB> _CorteDetalhe = prodController.ObterHBFilaDetalhe(3, 'S').Where(p => p.PROD_DETALHE != 4 && p.PROD_DETALHE != 5).OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
                List<PROD_HB> lista = new List<PROD_HB>();
                lista.AddRange(_Corte);
                lista.AddRange(_CorteDetalhe);
                List<PROD_HB> corte = new List<PROD_HB>();
                foreach (PROD_HB prod_hb in lista)
                    if (corte.Find(p => p.COLECAO.Trim() == prod_hb.COLECAO.Trim() && p.HB == prod_hb.HB && p.MOSTRUARIO == 'S') == null)
                        corte.Add(prod_hb);
                /* -- FIM -- */

                List<Fila> _filaAmpliacao = new List<Fila>();
                foreach (PROD_HB _prod_hb in _Ampliacao)
                {
                    _filaAmpliacao.Add(new Fila
                    {
                        HB = Convert.ToInt32(_prod_hb.HB),
                        COLECAO = (baseController.BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO,
                        GRUPO = _prod_hb.GRUPO,
                        NOME = _prod_hb.NOME,
                        TIPO = "PRINCIPAL",
                        COR = prodController.ObterCoresBasicas(_prod_hb.COR).DESC_COR,
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 1).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(_prod_hb.CODIGO, 1),
                        TITULO = "AMPLIAÇÃO"
                    });
                    cont += 1;
                }
                List<Fila> _filaRisco = new List<Fila>();
                foreach (PROD_HB _prod_hb in _Risco)
                {
                    _filaRisco.Add(new Fila
                    {
                        HB = Convert.ToInt32(_prod_hb.HB),
                        COLECAO = (baseController.BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO,
                        GRUPO = _prod_hb.GRUPO,
                        NOME = _prod_hb.NOME,
                        TIPO = "PRINCIPAL",
                        COR = prodController.ObterCoresBasicas(_prod_hb.COR).DESC_COR,
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 2).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(_prod_hb.CODIGO, 2),
                        TITULO = "RISCO"
                    });
                    cont += 1;
                }
                List<Fila> _filaCorte = new List<Fila>();
                foreach (PROD_HB _prod_hb in corte)
                {
                    _filaCorte.Add(new Fila
                    {
                        HB = Convert.ToInt32(_prod_hb.HB),
                        COLECAO = (baseController.BuscaColecaoAtual(_prod_hb.COLECAO)).DESC_COLECAO,
                        GRUPO = _prod_hb.GRUPO,
                        NOME = _prod_hb.NOME,
                        TIPO = "PRINCIPAL",
                        COR = prodController.ObterCoresBasicas(_prod_hb.COR).DESC_COR,
                        DATA = Convert.ToDateTime(prodController.ObterProcessoHB(_prod_hb.CODIGO, 2).DATA_INICIO).ToString("dd/MM/yyyy HH:mm"),
                        QUANTIDADE = prodController.ObterQtdeGradeHB(_prod_hb.CODIGO, 2),
                        TITULO = "CORTE"
                    });
                    cont += 1;
                }

                if (cont > 0)
                {
                    GerarRelatorio(_filaAmpliacao, _filaRisco, _filaCorte);
                    labErro.Text = "Relatório gerado com sucesso.";
                }
                else
                {
                    labErro.Text = "Não existem HBs para o Mostruário.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            //Recarregar GRIDS
            CarregarAmpliacao();
            CarregarRisco();
            CarregarCorte();
        }
        private void GerarRelatorio(List<Fila> _filaAmpliacao, List<Fila> _filaRisco, List<Fila> _filaCorte)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FILA_MOSTRUARIO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_filaAmpliacao, _filaRisco, _filaCorte));
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
        private StringBuilder MontarRelatorioHTML(List<Fila> _filaAmpliacao, List<Fila> _filaRisco, List<Fila> _filaCorte)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarCorte(_texto, _filaAmpliacao);
            _texto = MontarCorte(_texto, _filaRisco);
            _texto = MontarCorte(_texto, _filaCorte);
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
            _texto.Append("         <title>Mostruário</title>   ");
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
            _texto.Append("     <div id='fichaMostruario' align='center' style='border: 0px solid #000;'>");
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
            _texto.Append("                         HBF - MOSTRUÁRIO - " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana) + "</h2>");
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
        private StringBuilder MontarCorte(StringBuilder _texto, List<Fila> _fila)
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

    }
}

