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
    public partial class facc_encaixe : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["a"].ToString();
                string perfil = Request.QueryString["p"].ToString();
                if (tela == "1")
                {
                    hidTela.Value = "20";
                    labTitulo.Text = "Encaixe";
                    labMeioTitulo.Text = "Controle de Facção";
                    labSubTitulo.Text = "Encaixe";
                }
                else
                {
                    hidTela.Value = "21";
                    labTitulo.Text = "Encaixe Acabamento";
                    labMeioTitulo.Text = "Controle de Facção Acabamento";
                    labSubTitulo.Text = "Encaixe Acabamento";
                }

                if (perfil == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";
                else
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";


                CarregarColecoes();

                CarregarEncaixe("", "");
                CarregarEncaixeMostruario("", "");

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

        #region "PRINCIPAL"
        private void CarregarEncaixe(string colecao, string hb)
        {
            List<SP_OBTER_LISTA_SAIDAResult> _saida = ObterListaSaida("N", colecao, hb);

            if (_saida != null)
            {
                gvPrincipal.DataSource = _saida;
                gvPrincipal.DataBind();
            }
        }
        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_LISTA_SAIDAResult _saida = e.Row.DataItem as SP_OBTER_LISTA_SAIDAResult;

                    if (_saida != null)
                    {
                        //if (_saida.DATA_IMP_FIC_LOGISTICA == null && _saida.MOSTRUARIO == 'N' && hidTela.Value == "21")
                        //    e.Row.BackColor = Color.AntiqueWhite;

                        if (_saida.CORTADO == true && _saida.MOSTRUARIO == 'N' && hidTela.Value == "20")
                            e.Row.BackColor = Color.YellowGreen;

                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_saida.COLECAO)).DESC_COLECAO;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_saida.COR).DESC_COR.Trim();

                        Literal _litCortado = e.Row.FindControl("litCortado") as Literal;
                        if (_litCortado != null)
                            _litCortado.Text = _saida.DATA_INCLUSAO.ToString("dd/MM/yyyy");

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
                _footer.Cells[6].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvPrincipal_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_LISTA_SAIDAResult> _saida = ObterListaSaida("N", ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _saida = _saida.OrderBy(e.SortExpression + sortDirection);
            gvPrincipal.DataSource = _saida;
            gvPrincipal.DataBind();

            CarregarJQuery();
        }
        #endregion

        #region "MOSTRUARIO"
        private void CarregarEncaixeMostruario(string colecao, string hb)
        {
            List<SP_OBTER_LISTA_SAIDAResult> _saida = ObterListaSaida("S", colecao, hb);

            if (_saida != null)
            {
                gvMostruario.DataSource = _saida;
                gvMostruario.DataBind();
            }
        }
        protected void gvMostruario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_LISTA_SAIDAResult _saida = e.Row.DataItem as SP_OBTER_LISTA_SAIDAResult;

                    if (_saida != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = (new BaseController().BuscaColecaoAtual(_saida.COLECAO)).DESC_COLECAO;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(_saida.COR).DESC_COR.Trim();

                        Literal _litCortado = e.Row.FindControl("litCortado") as Literal;
                        if (_litCortado != null)
                            _litCortado.Text = _saida.DATA_INCLUSAO.ToString("dd/MM/yyyy");
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
                _footer.Cells[6].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvMostruario_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_LISTA_SAIDAResult> _saida = ObterListaSaida("S", ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim());

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _saida = _saida.OrderBy(e.SortExpression + sortDirection);
            gvMostruario.DataSource = _saida;
            gvMostruario.DataBind();

            CarregarJQuery();
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                CarregarEncaixe(ddlColecoes.SelectedValue.Trim(), txtHB.Text);
                CarregarEncaixeMostruario(ddlColecoes.SelectedValue.Trim(), txtHB.Text);

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }
        private List<SP_OBTER_LISTA_SAIDAResult> ObterListaSaida(string mostruario, string colecao, string hb)
        {
            List<SP_OBTER_LISTA_SAIDAResult> _saida = new List<SP_OBTER_LISTA_SAIDAResult>();

            int tela = Convert.ToInt32(hidTela.Value);

            if (mostruario == "" || mostruario == "S")
                _saida.AddRange(faccController.ObterListaSaida(tela, 1).Where(p => p.MOSTRUARIO == 'S'));

            if (mostruario == "" || mostruario == "N")
                _saida.AddRange(faccController.ObterListaSaida(tela, 1).Where(p => p.MOSTRUARIO == 'N'));

            if (colecao.Trim() != "")
                _saida = _saida.Where(p => p.COLECAO.Trim() == colecao.Trim()).ToList();

            if (hb.Trim() != "")
                _saida = _saida.Where(p => p.HB == Convert.ToInt32(hb)).ToList();

            return _saida.OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ToList();
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
                    string codigoSaida = "";
                    int tela = Convert.ToInt32(hidTela.Value);

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        codigoSaida = gvPrincipal.DataKeys[row.RowIndex].Value.ToString();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('facc_encaixe_baixar.aspx?p=" + codigoSaida + "&t=" + tela.ToString() + "');";
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
                    string codigoSaida = "";
                    int tela = Convert.ToInt32(hidTela.Value);

                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        codigoSaida = gvMostruario.DataKeys[row.RowIndex].Value.ToString();

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('facc_encaixe_baixar.aspx?p=" + codigoSaida + "&t=" + tela.ToString() + "');";
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
                List<SP_OBTER_LISTA_SAIDAResult> _saida = ObterListaSaida("", ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim());

                if (_saida.Count() > 0)
                {
                    GerarRelatorio(_saida);
                    labErro.Text = "Relatório gerado com sucesso.";
                }
                else
                {
                    labErro.Text = "Não existem encaixes para Impressão.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            CarregarJQuery();
        }
        private void GerarRelatorio(List<SP_OBTER_LISTA_SAIDAResult> _saida)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FACC_ENC_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_saida));
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
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_LISTA_SAIDAResult> _saida)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarEncaixe(_texto, _saida);
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
            _texto.Append("         <title>Encaixe</title>   ");
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
            _texto.Append("     <div id='fichaEncaixe' align='center' style='border: 0px solid #000;'>");
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
            _texto.Append("                            HBF - ENCAIXE- " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana));
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
        private StringBuilder MontarEncaixe(StringBuilder _texto, List<SP_OBTER_LISTA_SAIDAResult> _saida)
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
            _texto.Append("                <td style='width: 120px'>");
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
            _texto.Append("                <td>");
            _texto.Append("                    Mostruário");
            _texto.Append("                </td>");
            _texto.Append("                <td style='width: 80px; text-align: right;'>");
            _texto.Append("                    Quantidade");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");


            BaseController baseController = new BaseController();
            int qtde = 0;
            foreach (SP_OBTER_LISTA_SAIDAResult s in _saida)
            {
                qtde = (Convert.ToInt32(s.GRADE_EXP) + Convert.ToInt32(s.GRADE_XP) + Convert.ToInt32(s.GRADE_PP) + Convert.ToInt32(s.GRADE_P) + Convert.ToInt32(s.GRADE_M) + Convert.ToInt32(s.GRADE_G) + Convert.ToInt32(s.GRADE_GG));

                _texto.Append("            <tr>");
                _texto.Append("                <td>");
                _texto.Append("                    " + baseController.BuscaColecaoAtual(s.COLECAO).DESC_COLECAO.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + s.HB.ToString());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + s.CODIGO_PRODUTO_LINX.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + s.NOME.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + prodController.ObterCoresBasicas(s.COR).DESC_COR.Trim());
                _texto.Append("                </td>");
                _texto.Append("                <td>");
                _texto.Append("                    " + ((s.MOSTRUARIO == 'S') ? "Sim" : "Não"));
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

