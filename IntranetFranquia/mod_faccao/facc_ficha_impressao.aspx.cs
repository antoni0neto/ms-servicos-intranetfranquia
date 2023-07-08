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
    public partial class facc_ficha_impressao : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
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
        #endregion

        protected void btImprimir_Click(object sender, EventArgs e)
        {
            labImpressao.Text = "";
            if (ddlColecoesBuscar.SelectedValue == "" || ddlColecoesBuscar.SelectedValue == "0")
            {
                labImpressao.Text = "Selecione a Coleção.";
                return;
            }
            if (txtHBBuscar.Text == "" || Convert.ToInt32(txtHBBuscar.Text) <= 0)
            {
                labImpressao.Text = "Informe o número do HB.";
                return;
            }
            if (ddlFicha.SelectedValue == "" || ddlFicha.SelectedValue == "0")
            {
                labImpressao.Text = "Selecione a Ficha.";
                return;
            }

            try
            {
                List<PROD_HB> lstProdHB = new List<PROD_HB>();
                lstProdHB = new ProducaoController().ObterNumeroHB(ddlColecoesBuscar.SelectedValue, Convert.ToInt32(txtHBBuscar.Text));

                PROD_HB prod_hb = null;
                prod_hb = lstProdHB.Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).FirstOrDefault();

                if (prod_hb == null)
                {
                    labImpressao.Text = "Nenhum HB encontrado. Refaça a sua pesquisa.";
                    return;
                }

                if (prod_hb.STATUS.ToString() == "X" || prod_hb.STATUS.ToString() == "E")
                {
                    labImpressao.Text = "Nenhum HB encontrado. Refaça a sua pesquisa.";
                    return;
                }

                if (ddlFicha.SelectedValue == "1") //FACCAOFICHA
                {
                    GerarRelatorioRisco(prod_hb);
                }

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labImpressao.Text = "ERRO: " + ex.Message;
            }
        }

        #region "FICHA FACCAO"
        private void GerarRelatorioRisco(PROD_HB prod_hb)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FIC_FACC_" + prod_hb.HB.ToString() + "_COL_" + prod_hb.COLECAO.Trim() + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(prod_hb));
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
        private StringBuilder MontarRelatorioHTML(PROD_HB prod_hb)
        {
            StringBuilder _texto = new StringBuilder();
            _texto = MontarFaccao(prod_hb, _texto);
            return _texto;
        }
        private StringBuilder MontarFaccao(PROD_HB prod_hb, StringBuilder _texto)
        {
            ProducaoController prodController = new ProducaoController();

            _texto.Append(" ");
            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Ficha Facção</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("    <style type='text/css'>");
            _texto.Append("        @media print {");
            _texto.Append("            .background-force {");
            _texto.Append("                -webkit-print-color-adjust: exact;");
            _texto.Append("            }");
            _texto.Append("        }");
            _texto.Append(" ");
            _texto.Append("        .break {");
            _texto.Append("            page-break-before: always;");
            _texto.Append("        }");
            _texto.Append("    </style>");
            _texto.Append("</head>");
            _texto.Append("<body onload='window.print();' style='padding: 0; margin: 0;'>");
            _texto.Append("    <div id='divFaccao' align='center' class='' style='font: Calibri;'>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' style='padding: 0px; color: black;");
            _texto.Append("        font-size: 9.0pt; font-weight: 700; font-family: Calibri; background: white; white-space: nowrap;'>");
            _texto.Append("            <tr>");
            _texto.Append("                <td valign='top' style='height:750px;'>");
            _texto.Append("                    <div style='font: Calibri; border: 1px solid #696969; height: 750px; '>");
            _texto.Append("                        <table border='0' cellpadding='0' cellspacing='0' width='412px' style='padding: 0px;");
            _texto.Append("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
            _texto.Append("                            <tr style='line-height: 32px;'>");
            _texto.Append("                                <td rowspan='2' class='background-force' style='text-align: center; border-bottom: 1px solid #696969; width: 120px; background-color: #EEE9E9;' valign='middle'>");
            _texto.Append("                                    <span style='font-size: 16px;'>PRODUTO</span>");
            _texto.Append("                                </td>");
            _texto.Append("                                <td colspan='3' valign='middle' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    <span style='font-size: 15px;'>");
            _texto.Append("                                        " + prod_hb.CODIGO_PRODUTO_LINX + " - " + prod_hb.NOME.Trim());
            _texto.Append("                                    </span>");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 32px;'>");
            _texto.Append("                                <td colspan='4' valign='middle' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    <span style='font-size: 13px;'>");
            _texto.Append("                                        " + new BaseController().BuscaColecaoAtual(prod_hb.COLECAO).DESC_COLECAO.Trim() + " - HB " + prod_hb.HB.ToString() + " - " + ((prod_hb.MOSTRUARIO == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO"));
            _texto.Append("                                    </span>");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 32px; '>");
            _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;&nbsp;QUANTIDADE TOTAL");
            _texto.Append("                                </td>");
            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + prodController.ObterQtdeGradeHB(prod_hb.CODIGO, 3).ToString());
            _texto.Append("                                </td>");
            _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + prodController.ObterCoresBasicas(prod_hb.COR).DESC_COR.Trim());
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");

            int cont = 1;
            var saidaFornecedor = faccController.ObterSaidaFornecedor(prod_hb.CODIGO);
            if (saidaFornecedor == null || saidaFornecedor.Count() <= 0)
            {
                _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
                _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
                _texto.Append("                                    &nbsp;&nbsp;FACÇÃO");
                _texto.Append("                                </td>");
                _texto.Append("                                <td colspan='3' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                _texto.Append("                                    SEM ENCAIXE");
                _texto.Append("                                </td>");
            }
            else
            {
                foreach (var p in saidaFornecedor)
                {
                    _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
                    _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
                    _texto.Append("                                    &nbsp;&nbsp;FACÇÃO " + cont.ToString());
                    _texto.Append("                                </td>");
                    _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                    _texto.Append("                                    " + p.FORNECEDOR.Trim() + " " + p.FORNECEDOR_SUB);
                    _texto.Append("                                </td>");
                    _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                    _texto.Append("                                    " + p.GRADE_TOTAL.ToString());
                    _texto.Append("                                </td>");

                    cont += 1;
                }
            }
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 12px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td class='background-force' style='text-align: center; border-bottom: 1px solid #696969; background-color: #EEE9E9;' colspan='4' valign='middle'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td style='text-align: center; border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    SAÍDA");
            _texto.Append("                                </td>");
            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    RETORNO");
            _texto.Append("                                </td>");
            _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    DIAS ÚTEIS");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td style=' border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;&nbsp;ORDEM DE CORTE");
            _texto.Append("                                </td>");
            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + prod_hb.DATA_INCLUSAO.ToString("dd/MM/yyyy"));
            _texto.Append("                                </td>");
            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    -");
            _texto.Append("                                </td>");
            _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    -");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;&nbsp;CAD");
            _texto.Append("                                </td>");

            PROD_HB_PROD_PROCESSO _processo = null;
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            _processo = prodController.ObterProcessoHB(prod_hb.CODIGO, 1); //AMPLIACAO
            if (_processo != null)
                dataIni = _processo.DATA_INICIO;

            _processo = prodController.ObterProcessoHB(prod_hb.CODIGO, 2); //RISCO
            if (_processo != null)
            {
                if (dataIni == null)
                    dataIni = _processo.DATA_INICIO;
                dataFim = _processo.DATA_FIM;
            }

            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + ((dataIni == null) ? "" : Convert.ToDateTime(dataIni).ToString("dd/MM/yyyy")));
            _texto.Append("                                </td>");
            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + ((dataFim == null) ? "" : Convert.ToDateTime(dataFim).ToString("dd/MM/yyyy")));
            _texto.Append("                                </td>");
            _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + ((dataIni == null || dataFim == null) ? "" : (Utils.WebControls.ObterTotalDiasUteis(Convert.ToDateTime(dataIni).Date, Convert.ToDateTime(dataFim).Date)).ToString()));
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;&nbsp;CORTE");
            _texto.Append("                                </td>");

            _processo = null;
            dataIni = null;
            dataFim = null;
            _processo = prodController.ObterProcessoHB(prod_hb.CODIGO, 3); //CORTE
            if (_processo != null)
            {
                dataIni = _processo.DATA_INICIO;
                dataFim = _processo.DATA_FIM;
            }

            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + ((dataIni == null) ? "" : Convert.ToDateTime(dataIni).ToString("dd/MM/yyyy")));
            _texto.Append("                                </td>");
            _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + ((dataFim == null) ? "" : Convert.ToDateTime(dataFim).ToString("dd/MM/yyyy")));
            _texto.Append("                                </td>");
            _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    " + ((dataIni == null || dataFim == null) ? "" : (Utils.WebControls.ObterTotalDiasUteis(Convert.ToDateTime(dataIni).Date, Convert.ToDateTime(dataFim).Date)).ToString()));
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 24px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td class='background-force' style='text-align: left; border-bottom: 1px solid #696969; background-color: #EEE9E9;' colspan='4' valign='middle'>");
            _texto.Append("                                    &nbsp;DETALHES CORTE");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");

            foreach (var det in prodController.ObterDetalhesHB(prod_hb.CODIGO).Where(p => p.PROD_DETALHE != 5))
            {
                _processo = null;
                dataIni = null;
                dataFim = null;
                _processo = prodController.ObterProcessoHB(det.CODIGO, 3); //CORTE DETALHES
                if (_processo != null)
                {
                    dataIni = _processo.DATA_INICIO;
                    dataFim = _processo.DATA_FIM;
                }

                _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
                _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
                _texto.Append("                                    &nbsp;&nbsp;" + det.PROD_DETALHE1.DESCRICAO.ToUpper());
                _texto.Append("                                </td>");
                _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                _texto.Append("                                    " + ((dataIni == null) ? "" : Convert.ToDateTime(dataIni).ToString("dd/MM/yyyy")));
                _texto.Append("                                </td>");
                _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                _texto.Append("                                    " + ((dataFim == null) ? "" : Convert.ToDateTime(dataFim).ToString("dd/MM/yyyy")));
                _texto.Append("                                </td>");
                _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                _texto.Append("                                    " + ((dataIni == null || dataFim == null) ? "" : (Utils.WebControls.ObterTotalDiasUteis(Convert.ToDateTime(dataIni).Date, Convert.ToDateTime(dataFim).Date)).ToString()));
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");
            }
            _texto.Append("                            <tr style='line-height: 12px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td class='background-force' style='text-align: left; border-bottom: 1px solid #696969; background-color: #EEE9E9' colspan='4' valign='middle'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            cont = 1;
            var entradaFornecedor = faccController.ObterEntradaFornecedor(prod_hb.CODIGO);
            foreach (var e in entradaFornecedor)
            {
                _texto.Append("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
                _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
                _texto.Append("                                    &nbsp;&nbsp;FACÇÃO " + cont.ToString());
                _texto.Append("                                </td>");
                _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                _texto.Append("                                    " + ((e.DATA_SAIDA == null) ? "" : Convert.ToDateTime(e.DATA_SAIDA).ToString("dd/MM/yyyy")));
                _texto.Append("                                </td>");
                _texto.Append("                                <td style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                _texto.Append("                                    " + ((e.DATA_ENTRADA == null) ? "" : Convert.ToDateTime(e.DATA_ENTRADA).ToString("dd/MM/yyyy")));
                _texto.Append("                                </td>");
                _texto.Append("                                <td colspan='2' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
                _texto.Append("                                    " + ((e.DATA_SAIDA == null || e.DATA_ENTRADA == null) ? "" : (Utils.WebControls.ObterTotalDiasUteis(Convert.ToDateTime(e.DATA_SAIDA).Date, Convert.ToDateTime(e.DATA_ENTRADA).Date)).ToString()));
                _texto.Append("                                </td>");
                _texto.Append("                            </tr>");

                cont += 1;
            }
            _texto.Append("                            <tr style='line-height: 12px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td class='background-force' style='text-align: left; border-bottom: 1px solid #696969; background-color: #EEE9E9; ' colspan='4' valign='middle'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 40px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td style='border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;&nbsp;FALTA AVIAMENTO");
            _texto.Append("                                </td>");
            _texto.Append("                                <td colspan='3' style='text-align: center; border-left: 1px solid #696969; border-bottom: 1px solid #696969;'>");
            _texto.Append("                                    &nbsp;");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 45px; border-top: 1px solid #696969;'>");
            _texto.Append("                                <td colspan='4' style='border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;&nbsp; PREVISÃO 1:");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr style='line-height: 45px; border-top: 1px solid #696969; '>");
            _texto.Append("                                <td colspan='4' style='border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.Append("                                    &nbsp;&nbsp; PREVISÃO 2:");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                            <tr>");
            _texto.Append("                                <td colspan='4' style='border-bottom: none;'>");
            _texto.Append("                                    <br />");
            _texto.Append("                                    &nbsp;&nbsp; OBS.:");
            _texto.Append("                                </td>");
            _texto.Append("                            </tr>");
            _texto.Append("                        </table>");
            _texto.Append("                    </div>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");
            _texto.Append(" ");

            return _texto;
        }
        #endregion

    }
}
