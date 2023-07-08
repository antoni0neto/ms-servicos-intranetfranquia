using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class prod_rel_aviamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarAviamentos();
                CarregarFornecedores();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void CarregarAviamentos()
        {
            List<PROD_AVIAMENTO> _aviamento = prodController.ObterAviamento();
            if (_aviamento != null)
            {
                _aviamento = _aviamento.Where(p => p.STATUS == 'A').ToList();
                _aviamento.Insert(0, new PROD_AVIAMENTO { CODIGO = 0, DESCRICAO = "" });

                ddlAviamento.DataSource = _aviamento;
                ddlAviamento.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'A') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }
        }
        #endregion

        #region "AVIAMENTOS"
        private List<SP_OBTER_AVIAMENTO_ORCADOResult> RetornarAviamentos()
        {
            List<SP_OBTER_AVIAMENTO_ORCADOResult> aviamentoOrcado = new List<SP_OBTER_AVIAMENTO_ORCADOResult>();
            aviamentoOrcado = prodController.ObterAviamentoOrcado(txtNumeroOrcamento.Text, ddlAviamento.SelectedValue, ddlFornecedor.SelectedValue, txtHBBuscar.Text, ddlColecoesBuscar.SelectedValue.Trim()).Where(p => p.DATA_CANCELAMENTO == null).ToList();

            if (ddlComprado.SelectedValue != "")
                aviamentoOrcado = aviamentoOrcado.Where(i => i.COMPRADO == ddlComprado.SelectedValue).ToList();

            var distinctAviamento = aviamentoOrcado.GroupBy(p => new
            {
                CODIGO = p.CODIGO,
                HB = p.HB,
                GRUPO = p.GRUPO,
                NOME = p.NOME,
                DESC_AVIAMENTO = p.DESC_AVIAMENTO,
                QTDE = p.QTDE,
                DESCRICAO = p.DESCRICAO,
                COMPRADO = p.COMPRADO,
                COMPRA_EXTRA = p.COMPRA_EXTRA

            }).Select(k => new SP_OBTER_AVIAMENTO_ORCADOResult
            {
                CODIGO = k.Key.CODIGO,
                HB = k.Key.HB,
                GRUPO = k.Key.GRUPO,
                NOME = k.Key.NOME,
                DESC_AVIAMENTO = k.Key.DESC_AVIAMENTO,
                QTDE = k.Key.QTDE,
                DESCRICAO = k.Key.DESCRICAO,
                COMPRADO = k.Key.COMPRADO,
                COMPRA_EXTRA = k.Key.COMPRA_EXTRA
            }).ToList();

            distinctAviamento = distinctAviamento.OrderBy(p => p.HB).ToList();
            return distinctAviamento;
        }
        private void CarregarAviamentosOrcado()
        {
            var lstAviamento = RetornarAviamentos();

            if (lstAviamento != null)
            {
                lstAviamento = lstAviamento.OrderBy(p => p.HB).ToList();
                gvAviamento.DataSource = lstAviamento;
                gvAviamento.DataBind();
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labErro.Text = "";
            try
            {
                if (ddlColecoesBuscar.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                CarregarAviamentosOrcado();

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_AVIAMENTO_ORCADOResult _aviamento = e.Row.DataItem as SP_OBTER_AVIAMENTO_ORCADOResult;

                    coluna += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litGrupoNome = e.Row.FindControl("litGrupoNome") as Literal;
                        if (_litGrupoNome != null)
                            _litGrupoNome.Text = (_aviamento.GRUPO + _aviamento.NOME);

                        Literal _litCompraExtra = e.Row.FindControl("litCompraExtra") as Literal;
                        if (_litCompraExtra != null)
                            _litCompraExtra.Text = (_aviamento.COMPRA_EXTRA == 'S') ? "Sim" : "Não";

                        Literal _litComprado = e.Row.FindControl("litComprado") as Literal;
                        if (_litComprado != null)
                            _litComprado.Text = (_aviamento.COMPRADO == "S") ? "Sim" : "Não";

                    }
                }
        }
        #endregion

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            int contProduto;
            try
            {
                labErroImpressao.Text = "";
                contProduto = 0;

                List<RelatorioAviamento> _relatorio = new List<RelatorioAviamento>();
                var lstAviamento = RetornarAviamentos();
                foreach (var item in lstAviamento)
                {
                    contProduto++;

                    _relatorio.Add(new RelatorioAviamento
                    {
                        N_ORCAMENTO = item.CODIGO.ToString(),
                        HB = item.HB.ToString(),
                        NOME = (item.GRUPO.Trim() + " " + item.NOME.Trim()),
                        QUANTIDADE = item.QTDE.ToString(),
                        AVIAMENTO = item.DESC_AVIAMENTO,
                        DESCRICAO = item.DESCRICAO,
                        COMPRA_EXTRA = ((item.COMPRA_EXTRA == 'S') ? "Sim" : "Não")
                    });

                }

                labErroImpressao.Visible = true;
                if (contProduto == 1)
                    labErroImpressao.Text = "Foi impresso " + contProduto.ToString() + " aviamento!";
                else if (contProduto <= 0)
                    labErroImpressao.Text = "Nenhum aviamento impresso!";
                else
                    labErroImpressao.Text = "Foram impressos " + contProduto.ToString() + " aviamentos!";

                //IMPRIMIR RELATÓRIO
                if (_relatorio != null && _relatorio.Count > 0)
                    GerarRelatorio(_relatorio);
            }
            catch (Exception ex)
            {
                labErroImpressao.Text = "ERRO: " + ex.Message;
            }
        }
        private void GerarRelatorio(List<RelatorioAviamento> _relatorio)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "ORCAMENTO_AVIAMENTO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_relatorio));
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
        private StringBuilder MontarRelatorioHTML(List<RelatorioAviamento> _relatorio)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarConteudo(_texto, _relatorio);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Orçamento Aviamento</title>   ");
            _texto.Append("         <meta charset='UTF-8'>          ");
            _texto.Append("         <style type='text/css'>");
            _texto.Append("             @media print");
            _texto.Append("             {");
            _texto.Append("                 .tdback");
            _texto.Append("                 {");
            _texto.Append("                     background-color: WindowFrame !important;");
            _texto.Append("                     -webkit-print-color-adjust: exact;");
            _texto.Append("                 }");
            _texto.Append("                 .trback");
            _texto.Append("                 {");
            _texto.Append("                     background-color: #E6E6FA;");
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
            _texto.Append("            <h3>");
            _texto.Append("                RELATÓRIO AVIAMENTO - " + DateTime.Now.ToString("dd/MM/yyyy") + "</h3>");
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
        private StringBuilder MontarConteudo(StringBuilder _texto, List<RelatorioAviamento> _relatorio)
        {
            string hb = "";
            int totalAviamento = 0;
            int count = 0;

            foreach (RelatorioAviamento rel in _relatorio)
            {
                count += 1;
                if (hb != rel.HB)
                {
                    totalAviamento = _relatorio.Where(p => p.HB == rel.HB).Count();

                    _texto.Append("<tr style='line-height: 21px;'>");
                    _texto.Append("    <td style='padding: 5px 5px 5px 5px;'>");
                    _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                    _texto.Append("            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                    _texto.Append("            background: white; white-space: nowrap;'>");

                    _texto.Append("            <tr style='line-height:20px;'>");
                    _texto.Append("                <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center; background-color: WindowFrame !important;");
                    _texto.Append("                    width: 60px;'>");
                    _texto.Append("                    HB");
                    _texto.Append("                </td>");
                    _texto.Append("                <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center; background-color: WindowFrame !important;");
                    _texto.Append("                    width: 130px;'>");
                    _texto.Append("                    NOME");
                    _texto.Append("                </td>");
                    _texto.Append("                <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center; background-color: WindowFrame !important;");
                    _texto.Append("                    width: 94px;'>");
                    _texto.Append("                    QUANTIDADE");
                    _texto.Append("                </td>");
                    _texto.Append("                <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center; background-color: WindowFrame !important;");
                    _texto.Append("                    width: 150px;'>");
                    _texto.Append("                    AVIAMENTO");
                    _texto.Append("                </td>");
                    _texto.Append("                <td class='tdback' style='border: 1px solid #000; text-align: center; background-color: WindowFrame !important;");
                    _texto.Append("                    width: 110px;'>");
                    _texto.Append("                    No. ORÇAMENTO");
                    _texto.Append("                </td>");
                    _texto.Append("                <td style='width: 5px;'>");
                    _texto.Append("                    &nbsp;");
                    _texto.Append("                </td>");
                    _texto.Append("                <td rowspan='" + (totalAviamento * 3) + "' style='border: 1px solid #000; text-align: center;'>");
                    _texto.Append("                    TECIDO<br />");
                    _texto.Append("                    (Cole Aqui)");
                    _texto.Append("                </td>");
                    _texto.Append("            </tr>");
                }

                _texto.Append("            <tr class='trback' style='line-height:20px;'>");
                _texto.Append("                <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                _texto.Append("                    " + rel.HB);
                _texto.Append("                </td>");
                _texto.Append("                <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                _texto.Append("                    " + rel.NOME);
                _texto.Append("                </td>");
                _texto.Append("                <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                _texto.Append("                    " + rel.QUANTIDADE);
                _texto.Append("                </td>");
                _texto.Append("                <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                _texto.Append("                    " + rel.AVIAMENTO);
                _texto.Append("                </td>");
                _texto.Append("                <td style='border: 1px solid #000; border-top: none; text-align: center;'>");
                _texto.Append("                    " + rel.N_ORCAMENTO);
                _texto.Append("                </td>");
                _texto.Append("            </tr>");
                _texto.Append("            <tr class='trback' style='line-height:20px;'>");
                _texto.Append("                <td colspan='5' style='border: 1px solid #000; border-right: 1px solid #000; border-top: none; text-align: left;'>");
                _texto.Append("                    &nbsp;&nbsp;" + rel.DESCRICAO);
                _texto.Append("                </td>");
                _texto.Append("            </tr>");

                if (count == totalAviamento)
                {
                    _texto.Append("        </table>");
                    _texto.Append("    </td>");
                    _texto.Append("</tr>");

                    count = 0;
                }

                hb = rel.HB;
            }

            return _texto;
        }
        #endregion
    }
}
