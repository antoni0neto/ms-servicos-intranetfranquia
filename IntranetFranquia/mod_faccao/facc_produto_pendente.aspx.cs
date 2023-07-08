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
    public partial class facc_produto_pendente : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["a"].ToString();
                if (tela == "1")
                {
                    hidTela.Value = "20";
                    labTitulo.Text = "Pendentes";
                    labMeioTitulo.Text = "Controle de Facção";
                    labSubTitulo.Text = "Pendentes";
                }
                else
                {
                    hidTela.Value = "21";
                    labTitulo.Text = "Acabamentos Pendentes";
                    labMeioTitulo.Text = "Controle de Facção Acabamento";
                    labSubTitulo.Text = "Acabamentos Pendentes";
                }

                Page.Title = labTitulo.Text;

                CarregarColecoes();

                CarregarEntradaProdutoPendente("", "", "");
                CarregarEntradaProdutoMostruarioPendente("", "", "");

                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
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
        private void CarregarJQuery()
        {
            //GRID PRINCIPAL
            if (gvPrincipal.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID MOSTRUÁRIO
            if (gvMostruario.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionM').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionM').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        #region "ENTRADA"
        private void CarregarEntradaProdutoPendente(string colecao, string hb, string produto)
        {
            List<SP_OBTER_LISTA_ENTRADAResult> _entrada = ObterListaEntrada("N", colecao, hb);

            if (produto.Trim() != "")
                _entrada = _entrada.Where(p => p.CODIGO_PRODUTO_LINX.Trim() == produto.Trim()).ToList();

            gvPrincipal.DataSource = _entrada;
            gvPrincipal.DataBind();

        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_LISTA_ENTRADAResult _entrada = e.Row.DataItem as SP_OBTER_LISTA_ENTRADAResult;

                    if (_entrada != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_entrada.COLECAO)).DESC_COLECAO;

                        Literal _litHB = e.Row.FindControl("litHB") as Literal;
                        if (_litHB != null)
                            _litHB.Text = _entrada.HB.ToString();

                        Literal _litProduto = e.Row.FindControl("litProduto") as Literal;
                        if (_litProduto != null)
                            _litProduto.Text = _entrada.CODIGO_PRODUTO_LINX.ToString();

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = _entrada.NOME;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_entrada.COR).DESC_COR.Trim();

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                            _qtde.Text = (Convert.ToInt32(_entrada.GRADE_EXP_S) + Convert.ToInt32(_entrada.GRADE_XP_S) + Convert.ToInt32(_entrada.GRADE_PP_S) + Convert.ToInt32(_entrada.GRADE_P_S) + Convert.ToInt32(_entrada.GRADE_M_S) + Convert.ToInt32(_entrada.GRADE_G_S) + Convert.ToInt32(_entrada.GRADE_GG_S)).ToString();

                        Literal _litEnviado = e.Row.FindControl("litEnviado") as Literal;
                        if (_litEnviado != null)
                            _litEnviado.Text = Convert.ToDateTime(_entrada.DATA_INCLUSAO).ToString("dd/MM/yyyy");

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_entrada.RECEBIMENTO != null && _entrada.GRADE_TOTAL <= 0)
                        {
                            e.Row.BackColor = Color.OldLace;
                            _btBaixar.Text = "Grade";
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
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                _footer.Cells[7].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvPrincipal_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_LISTA_ENTRADAResult> _entrada = ObterListaEntrada("N", ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _entrada = _entrada.OrderBy(e.SortExpression + sortDirection);
            gvPrincipal.DataSource = _entrada;
            gvPrincipal.DataBind();

            CarregarJQuery();
        }
        #endregion

        #region "ENTRADA MOSTURARIO"
        private void CarregarEntradaProdutoMostruarioPendente(string colecao, string hb, string produto)
        {
            List<SP_OBTER_LISTA_ENTRADAResult> _entrada = ObterListaEntrada("S", colecao, hb);

            if (produto.Trim() != "")
                _entrada = _entrada.Where(p => p.CODIGO_PRODUTO_LINX.Trim() == produto.Trim()).ToList();

            gvMostruario.DataSource = _entrada;
            gvMostruario.DataBind();
        }
        protected void gvMostruario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_LISTA_ENTRADAResult _entrada = e.Row.DataItem as SP_OBTER_LISTA_ENTRADAResult;

                    if (_entrada != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_entrada.COLECAO)).DESC_COLECAO;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_entrada.COR).DESC_COR.Trim();

                        Literal _qtde = e.Row.FindControl("litQtde") as Literal;
                        if (_qtde != null)
                            _qtde.Text = (Convert.ToInt32(_entrada.GRADE_EXP_S) + Convert.ToInt32(_entrada.GRADE_XP_S) + Convert.ToInt32(_entrada.GRADE_PP_S) + Convert.ToInt32(_entrada.GRADE_P_S) + Convert.ToInt32(_entrada.GRADE_M_S) + Convert.ToInt32(_entrada.GRADE_G_S) + Convert.ToInt32(_entrada.GRADE_GG_S)).ToString();

                        Literal _litEnviado = e.Row.FindControl("litEnviado") as Literal;
                        if (_litEnviado != null)
                            _litEnviado.Text = Convert.ToDateTime(_entrada.DATA_INCLUSAO).ToString("dd/MM/yyyy");

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_entrada.RECEBIMENTO != null && _entrada.GRADE_TOTAL <= 0)
                        {
                            e.Row.BackColor = Color.OldLace;
                            _btBaixar.Text = "Grade";
                        }
                    }
                }
            }
        }
        protected void gvMostruario_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvMostruario.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvMostruario.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                _footer.Cells[7].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvMostruario_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_LISTA_ENTRADAResult> _entrada = ObterListaEntrada("S", ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _entrada = _entrada.OrderBy(e.SortExpression + sortDirection);
            gvMostruario.DataSource = _entrada;
            gvMostruario.DataBind();

            CarregarJQuery();
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarEntradaProdutoPendente(ddlColecoes.SelectedValue.Trim(), txtHB.Text, txtProduto.Text);
                CarregarEntradaProdutoMostruarioPendente(ddlColecoes.SelectedValue.Trim(), txtHB.Text, txtProduto.Text);

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }
        private List<SP_OBTER_LISTA_ENTRADAResult> ObterListaEntrada(string mostruario, string colecao, string hb)
        {
            List<SP_OBTER_LISTA_ENTRADAResult> _entrada = new List<SP_OBTER_LISTA_ENTRADAResult>();

            int tela = Convert.ToInt32(hidTela.Value);

            if (mostruario == "" || mostruario == "S")
                _entrada.AddRange(faccController.ObterListaEntrada(tela, "", 'S').Where(p => (p.STATUS == 'P')));

            if (mostruario == "" || mostruario == "N")
                _entrada.AddRange(faccController.ObterListaEntrada(tela, "", 'N').Where(p => (p.STATUS == 'P')));

            if (colecao.Trim() != "")
                _entrada = _entrada.Where(p => p.COLECAO.Trim() == colecao.Trim()).ToList();

            if (hb.Trim() != "")
                _entrada = _entrada.Where(p => p.HB == Convert.ToInt32(hb)).ToList();

            return _entrada.Where(p => p.GRADE_TOTAL_SAIDA > 0).OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
        }

        #region "BAIXA"
        protected void btBaixarProducao_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string codigoEntrada = "";
                    int tela = Convert.ToInt32(hidTela.Value);

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        codigoEntrada = gvPrincipal.DataKeys[row.RowIndex].Value.ToString();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('facc_produto_pendente_baixar.aspx?p=" + codigoEntrada + "&t=" + tela.ToString() + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                    CarregarJQuery();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        protected void btBaixarMostruario_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string codigoEntrada = "";
                    int tela = Convert.ToInt32(hidTela.Value);

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        codigoEntrada = gvMostruario.DataKeys[row.RowIndex].Value.ToString();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('facc_produto_pendente_baixar.aspx?p=" + codigoEntrada + "&t=" + tela.ToString() + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                    CarregarJQuery();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }
        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {

            try
            {
                List<SP_OBTER_LISTA_ENTRADAResult> _entrada = ObterListaEntrada("", ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim());

                if (_entrada.Count() > 0)
                {
                    GerarRelatorio(_entrada);
                    labErro.Text = "Relatório gerado com sucesso.";
                }
                else
                {
                    labErro.Text = "Não existem retornos pendentes para Impressão.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            CarregarJQuery();
        }
        private void GerarRelatorio(List<SP_OBTER_LISTA_ENTRADAResult> _entrada)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FACC_ENT_PROD_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_entrada));
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
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_LISTA_ENTRADAResult> _entrada)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarEntradaPendente(_texto, _entrada);
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
            _texto.Append("         <title>Entrada de Produto</title>   ");
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
            _texto.Append("     <div id='fichaEntrada' align='center' style='border: 0px solid #000;'>");
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
            _texto.Append("                            HBF - ENTRADA DE PRODUTO PENDENTE - " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana));
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
        private StringBuilder MontarEntradaPendente(StringBuilder _texto, List<SP_OBTER_LISTA_ENTRADAResult> _entrada)
        {
            int total = 0;

            _texto.Append("<tr style='line-height: 19px;'>");
            _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
            _texto.Append("        <span style='font-size: 15px;'>&nbsp;</span>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
            _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("            background: white; white-space: nowrap;'>");
            _texto.Append("            <tr style='line-height: 5px;'>");
            _texto.Append("                <td colspan='7'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr>");
            _texto.Append("                <td>");
            _texto.Append("                    Fornecedor");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 75px'>");
            _texto.Append("                    Coleção");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 75px'>");
            _texto.Append("                    HB");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 75px'>");
            _texto.Append("                    Produto");
            _texto.Append("                </td>");
            _texto.Append("                <td>");
            _texto.Append("                    Nome");
            _texto.Append("                </td>");
            _texto.Append("                <td>");
            _texto.Append("                    Cor");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 80px; text-align: right;'>");
            _texto.Append("                    Quantidade");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");

            BaseController baseController = new BaseController();
            int qtde = 0;
            foreach (SP_OBTER_LISTA_ENTRADAResult e in _entrada)
            {
                qtde = Convert.ToInt32(e.GRADE_EXP_S) + Convert.ToInt32(e.GRADE_XP_S) + Convert.ToInt32(e.GRADE_PP_S) + Convert.ToInt32(e.GRADE_P_S) + Convert.ToInt32(e.GRADE_M_S) + Convert.ToInt32(e.GRADE_G_S) + Convert.ToInt32(e.GRADE_GG_S);

                _texto.Append("            <tr>");
                _texto.Append("                <td>");
                _texto.Append("                    " + e.FORNECEDOR);
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + baseController.BuscaColecaoAtual(e.COLECAO).DESC_COLECAO.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + e.HB.ToString());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + e.CODIGO_PRODUTO_LINX.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + e.NOME.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + prodController.ObterCoresBasicas(e.COR).DESC_COR.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td style='text-align: right;'>");
                _texto.Append("                    " + qtde.ToString());
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                total += qtde;
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
            _texto.Append("                <td style='text-align: right;'>");
            _texto.Append("                    " + total.ToString());
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        #endregion

    }
}

