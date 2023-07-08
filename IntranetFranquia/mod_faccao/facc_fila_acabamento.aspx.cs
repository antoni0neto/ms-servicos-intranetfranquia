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
    public partial class facc_fila_acabamento : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

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
                CarregarEnviado(DateTime.Now.Date, 'N');
                CarregarRecebido(DateTime.Now.Date, 'N');

                CarregarJQuery();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private void CarregarJQuery()
        {
            //GRID Enviado
            if (gvEnviado.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionE').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);

            //GRID Recebido
            if (gvRecebido.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionR').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }

        private List<SP_OBTER_FACCAO_DIARIOResult> ObterAcabamentoDiario(DateTime dataPesquisa, char mostruario, int query)
        {
            int prodServico = 4;
            if (mostruario == 'S')
                prodServico = 6;

            var faccaoDiario = faccController.ObterFaccaoDiario(dataPesquisa, prodServico, mostruario, query);

            if (ddlFornecedor.SelectedValue.Trim() != "")
            {
                if (ddlFornecedor.SelectedValue.Trim() == "S")
                    faccaoDiario = faccaoDiario.Where(p => p.FORNECEDOR.Trim().Contains("HANDBOOK")).ToList();
                else
                    faccaoDiario = faccaoDiario.Where(p => !p.FORNECEDOR.Trim().Contains("HANDBOOK")).ToList();
            }

            return faccaoDiario;
        }

        #region "ENVIADO"
        private void CarregarEnviado(DateTime dataPesquisa, char mostruario)
        {
            var _enviado = ObterAcabamentoDiario(dataPesquisa, mostruario, 1);
            if (_enviado != null)
            {
                gvEnviado.DataSource = _enviado;
                gvEnviado.DataBind();
            }
        }
        protected void gvEnviado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_DIARIOResult _enviado = e.Row.DataItem as SP_OBTER_FACCAO_DIARIOResult;

                    if (_enviado != null)
                    {
                    }
                }
            }
        }
        protected void gvEnviado_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvEnviado.FooterRow;
            int total;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow g in gvEnviado.Rows)
                    total += (((Literal)g.FindControl("litQtde")).Text != "") ? Convert.ToInt32(((Literal)g.FindControl("litQtde")).Text) : 0;

                _footer.Cells[1].Text = "Total";
                _footer.Cells[7].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvEnviado_Sorting(object sender, GridViewSortEventArgs e)
        {
            char mostruario = 'N';
            if (chkMostruario.Checked)
                mostruario = 'S';

            IEnumerable<SP_OBTER_FACCAO_DIARIOResult> faccaoDiario = ObterAcabamentoDiario(Convert.ToDateTime(txtDataPesquisa.Text), mostruario, 1);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            faccaoDiario = faccaoDiario.OrderBy(e.SortExpression + sortDirection);
            gvEnviado.DataSource = faccaoDiario;
            gvEnviado.DataBind();

            CarregarJQuery();

        }

        #endregion

        #region "RECEBIDO"

        private List<SP_OBTER_FACCAO_DIARIO_ATACVARResult> ObterAcabamentoDiarioAtacadoVarejo(DateTime dataPesquisa, char mostruario)
        {
            int prodServico = 4;
            if (mostruario == 'S')
                prodServico = 6;

            var faccaoDiario = faccController.ObterFaccaoDiarioAtacadoVarejo(dataPesquisa, prodServico, mostruario);

            if (ddlFornecedor.SelectedValue.Trim() != "")
            {
                if (ddlFornecedor.SelectedValue.Trim() == "S")
                    faccaoDiario = faccaoDiario.Where(p => p.FORNECEDOR.Trim().Contains("HANDBOOK")).ToList();
                else
                    faccaoDiario = faccaoDiario.Where(p => !p.FORNECEDOR.Trim().Contains("HANDBOOK")).ToList();
            }

            return faccaoDiario;
        }

        private void CarregarRecebido(DateTime dataPesquisa, char mostruario)
        {
            var _recebido = ObterAcabamentoDiarioAtacadoVarejo(dataPesquisa, mostruario);

            if (_recebido != null)
            {
                gvRecebido.DataSource = _recebido;
                gvRecebido.DataBind();
            }
        }
        protected void gvRecebido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_DIARIO_ATACVARResult _recebido = e.Row.DataItem as SP_OBTER_FACCAO_DIARIO_ATACVARResult;

                    if (_recebido != null)
                    {

                        CheckBox cbLinx = e.Row.FindControl("cbLinx") as CheckBox;
                        if (_recebido.ENTRADA_LINX == 'S')
                        {
                            cbLinx.Checked = true;
                            e.Row.BackColor = Color.PaleGreen;
                        }
                        else
                        {
                            cbLinx.Checked = false;
                            e.Row.BackColor = Color.White;
                        }

                        CheckBox cbLoja = e.Row.FindControl("cbLoja") as CheckBox;
                        if (_recebido.LIBERADO_LOJA == 'S')
                        {
                            cbLoja.Checked = true;
                            e.Row.Cells[18].BackColor = Color.HotPink;
                        }
                        else
                        {
                            cbLoja.Checked = false;
                            e.Row.Cells[18].BackColor = Color.White;
                        }


                    }
                }
            }
        }
        protected void gvRecebido_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvRecebido.FooterRow;

            int exp = 0;
            int xp = 0;
            int pp = 0;
            int p = 0;
            int m = 0;
            int g = 0;
            int gg = 0;
            int total = 0;
            if (_footer != null)
            {
                total = 0;
                foreach (GridViewRow row in gvRecebido.Rows)
                {
                    exp += (((Literal)row.FindControl("litQtdeEXP")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdeEXP")).Text) : 0;
                    xp += (((Literal)row.FindControl("litQtdeXP")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdeXP")).Text) : 0;
                    pp += (((Literal)row.FindControl("litQtdePP")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdePP")).Text) : 0;
                    p += (((Literal)row.FindControl("litQtdeP")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdeP")).Text) : 0;
                    m += (((Literal)row.FindControl("litQtdeM")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdeM")).Text) : 0;
                    g += (((Literal)row.FindControl("litQtdeM")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdeM")).Text) : 0;
                    gg += (((Literal)row.FindControl("litQtdeGG")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdeGG")).Text) : 0;
                    total += (((Literal)row.FindControl("litQtdeTotal")).Text != "") ? Convert.ToInt32(((Literal)row.FindControl("litQtdeTotal")).Text) : 0;
                }

                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[9].Text = exp.ToString();
                _footer.Cells[10].Text = xp.ToString();
                _footer.Cells[11].Text = pp.ToString();
                _footer.Cells[12].Text = p.ToString();
                _footer.Cells[13].Text = m.ToString();
                _footer.Cells[14].Text = g.ToString();
                _footer.Cells[15].Text = gg.ToString();
                _footer.Cells[16].Text = total.ToString();
                _footer.Font.Bold = true;
            }
        }
        protected void gvRecebido_Sorting(object sender, GridViewSortEventArgs e)
        {
            char mostruario = 'N';
            if (chkMostruario.Checked)
                mostruario = 'S';

            IEnumerable<SP_OBTER_FACCAO_DIARIO_ATACVARResult> faccaoDiario = ObterAcabamentoDiarioAtacadoVarejo(Convert.ToDateTime(txtDataPesquisa.Text), mostruario);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            faccaoDiario = faccaoDiario.OrderBy(e.SortExpression + sortDirection);
            gvRecebido.DataSource = faccaoDiario;
            gvRecebido.DataBind();

            CarregarJQuery();
        }
        #endregion

        #region "BUSCAR"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_data;
            char mostruario = 'N';

            try
            {
                labMsg.Text = "";
                labErro.Text = "";
                if (!DateTime.TryParse(txtDataPesquisa.Text, out v_data))
                {
                    labMsg.Text = "INFORME UMA DATA VÁLIDA.";
                    return;
                }

                if (chkMostruario.Checked)
                    mostruario = 'S';

                CarregarEnviado(v_data, mostruario);
                CarregarRecebido(v_data, mostruario);

                CarregarJQuery();
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
            DateTime dataPesquisa = DateTime.Now;
            char mostruario = 'N';

            labMsg.Text = "";
            labErro.Text = "";

            if (!DateTime.TryParse(txtDataPesquisa.Text, out dataPesquisa))
            {
                labMsg.Text = "INFORME UMA DATA VÁLIDA.";
                return;
            }

            if (chkMostruario.Checked)
                mostruario = 'S';

            try
            {
                var faccaoEnviado = ObterAcabamentoDiario(dataPesquisa, mostruario, 1);
                var faccaoRecebido = ObterAcabamentoDiarioAtacadoVarejo(dataPesquisa, mostruario);

                GerarRelatorio(faccaoEnviado, faccaoRecebido, dataPesquisa, ((mostruario == 'S') ? true : false));
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

            CarregarEnviado(dataPesquisa.Date, mostruario);
            CarregarRecebido(dataPesquisa.Date, mostruario);
        }
        private void GerarRelatorio(List<SP_OBTER_FACCAO_DIARIOResult> faccaoEnviado, List<SP_OBTER_FACCAO_DIARIO_ATACVARResult> faccaoRecebido, DateTime dataPesquisa, bool bMostruario)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "ACABAMENTO_DIARIO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(faccaoEnviado, faccaoRecebido, dataPesquisa, bMostruario));
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
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_FACCAO_DIARIOResult> faccaoEnviado, List<SP_OBTER_FACCAO_DIARIO_ATACVARResult> faccaoRecebido, DateTime dataPesquisa, bool bMostruario)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, dataPesquisa, bMostruario);
            _texto = MontarFaccaoDiario(_texto, faccaoEnviado);
            _texto = MontarEspaco(_texto);
            _texto = MontarFaccaoDiarioAtacVar(_texto, faccaoRecebido);
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
            _texto.Append("         <title>Acabamento Diário</title>   ");
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
            _texto.Append("                         " + ((bMostruario) ? "MOSTRUÁRIO ACABAMENTO DIÁRIO" : "PRODUÇÃO ACABAMENTO DIÁRIO") + " - " + dataPesquisa.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana) + "</h2>");
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
        private StringBuilder MontarFaccaoDiario(StringBuilder _texto, List<SP_OBTER_FACCAO_DIARIOResult> _fila)
        {
            int total = 0;
            int cont = 1;

            if (_fila.Count > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                _texto.Append("        <span style='font-size: 15px;'># " + _fila[0].TIPO + "</span>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='7'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                foreach (SP_OBTER_FACCAO_DIARIOResult fila in _fila)
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
                    _texto.Append("                <td style='width: 70px'>");
                    _texto.Append("                    HB " + fila.HB);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.PRODUTO + " - " + fila.NOME);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.DESC_COR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right; width: 50px;'>");
                    _texto.Append("                    " + fila.GRADE_TOTAL);
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");

                    cont += 1;
                    total += Convert.ToInt32(fila.GRADE_TOTAL);
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
            }
            return _texto;
        }
        private StringBuilder MontarFaccaoDiarioAtacVar(StringBuilder _texto, List<SP_OBTER_FACCAO_DIARIO_ATACVARResult> _fila)
        {
            int total = 0;
            int cont = 1;

            if (_fila.Count > 0)
            {
                _texto.Append("<tr style='line-height: 19px;'>");
                _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000;'>");
                _texto.Append("        <span style='font-size: 15px;'># " + _fila[0].TIPO + "</span>");
                _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("            background: white; white-space: nowrap;'>");
                _texto.Append("            <tr style='line-height: 5px;'>");
                _texto.Append("                <td colspan='8'>");
                _texto.Append("                    &nbsp;");
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                foreach (SP_OBTER_FACCAO_DIARIO_ATACVARResult fila in _fila)
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
                    _texto.Append("                <td style='width: 70px'>");
                    _texto.Append("                    HB " + fila.HB);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.PRODUTO + " - " + fila.NOME);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.DESC_COR);
                    _texto.Append("                </td>");
                    _texto.Append("                <td>");
                    _texto.Append("                    " + fila.TIPO);
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='text-align: right; width: 50px;'>");
                    _texto.Append("                    " + fila.GRADE_TOTAL);
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");

                    cont += 1;
                    total += Convert.ToInt32(fila.GRADE_TOTAL);
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
            }
            return _texto;
        }
        #endregion

        protected void cbLinx_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labErro.Text = "";

                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;

                int codigoEntradaQtde = Convert.ToInt32(gvRecebido.DataKeys[row.RowIndex].Value);

                var ent = faccController.ObterEntradaQtdeCodigo(codigoEntradaQtde);
                if (ent != null)
                {
                    ent.ENTRADA_LINX = (cb.Checked) ? 'S' : 'N';

                    faccController.AtualizarEntradaQtde(ent);

                    if (cb.Checked)
                        row.BackColor = Color.PaleGreen;
                    else
                        row.BackColor = Color.White;

                }

                CarregarJQuery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void cbLoja_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labErro.Text = "";

                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;

                int codigoEntradaQtde = Convert.ToInt32(gvRecebido.DataKeys[row.RowIndex].Value);

                var ent = faccController.ObterEntradaQtdeCodigo(codigoEntradaQtde);
                if (ent != null)
                {
                    ent.LIBERADO_LOJA = (cb.Checked) ? 'S' : 'N';

                    faccController.AtualizarEntradaQtde(ent);

                    if (cb.Checked)
                        row.Cells[18].BackColor = Color.HotPink;
                    else
                        row.Cells[18].BackColor = Color.White;

                }

                CarregarJQuery();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
    }
}

