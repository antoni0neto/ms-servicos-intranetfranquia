using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;
using System.Text;

namespace Relatorios
{
    public partial class fisc_resumo_imposto : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        decimal tPIS = 0;
        decimal tCOFINS = 0;
        decimal tICMS = 0;
        decimal tINSS = 0;
        decimal tIRPJ = 0;
        decimal tCSLL = 0;
        decimal tSN = 0;
        decimal tTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "fisc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_contabilidade/contabil_menu.aspx";

                CarregarDataAno();
                CarregarEmpresa();
                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAnoDe.DataSource = dataAno;
                ddlAnoDe.DataBind();

                ddlAnoAte.DataSource = dataAno;
                ddlAnoAte.DataBind();

                if (ddlAnoDe.Items.Count > 0)
                {
                    try
                    {
                        ddlAnoDe.SelectedValue = DateTime.Now.Year.ToString();
                        ddlAnoAte.SelectedValue = DateTime.Now.Year.ToString();

                        ddlMesDe.SelectedValue = "";
                        ddlMesAte.SelectedValue = "";
                    }
                    catch (Exception) { }
                }
            }
        }
        private void CarregarEmpresa()
        {
            var empresa = baseController.ObterEmpresa().OrderBy(p => p.NOME).ToList();

            empresa.Insert(0, new EMPRESA { NOME = "" });
            ddlEmpresa.DataSource = empresa;
            ddlEmpresa.DataBind();
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labAnoDe.ForeColor = _OK;
            if (ddlAnoDe.SelectedValue.Trim() == "")
            {
                labAnoDe.ForeColor = _notOK;
                retorno = false;
            }

            labMesDe.ForeColor = _OK;
            if (ddlMesDe.SelectedValue.Trim() == "")
            {
                labMesDe.ForeColor = _notOK;
                retorno = false;
            }

            labAnoAte.ForeColor = _OK;
            if (ddlAnoAte.SelectedValue.Trim() == "")
            {
                labAnoAte.ForeColor = _notOK;
                retorno = false;
            }

            labMesAte.ForeColor = _OK;
            if (ddlMesAte.SelectedValue.Trim() == "")
            {
                labMesAte.ForeColor = _notOK;
                retorno = false;
            }


            //labEmpresa.ForeColor = _OK;
            //if (ddlEmpresa.SelectedValue.Trim() == "Selecione")
            //{
            //    labEmpresa.ForeColor = _notOK;
            //    retorno = false;
            //}

            //labFilial.ForeColor = _OK;
            //if (ddlFilial.SelectedValue.Trim() == "Selecione")
            //{
            //    labFilial.ForeColor = _notOK;
            //    retorno = false;
            //}

            return retorno;
        }
        #endregion

        #region "RESUMO IMPOSTO"
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho.";
                    return;
                }

                int anoDe = Convert.ToInt32(ddlAnoDe.SelectedValue);
                int mesDe = Convert.ToInt32(ddlMesDe.SelectedValue);

                int anoAte = Convert.ToInt32(ddlAnoAte.SelectedValue);
                int mesAte = Convert.ToInt32(ddlMesAte.SelectedValue);

                CarregarResumoImposto(anoDe, mesDe, anoAte, mesAte, ddlEmpresa.SelectedValue.Trim(), ddlFilial.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarResumoImposto(int anoDe, int mesDe, int anoAte, int mesAte, string empresa, string codigoFilial)
        {
            //Obter Resumo Imposto
            var resumoImposto = contabilController.ObterResumoImposto(anoDe, mesDe, anoAte, mesAte, empresa, codigoFilial);

            gvResumoImposto.DataSource = resumoImposto;
            gvResumoImposto.DataBind();
        }
        protected void gvResumoImposto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_RESUMO_IMPOSTOResult resumoImposto = e.Row.DataItem as SP_OBTER_RESUMO_IMPOSTOResult;

                    if (resumoImposto != null)
                    {
                        Literal _litCompetencia = e.Row.FindControl("litCompetencia") as Literal;
                        if (_litCompetencia != null)
                            _litCompetencia.Text = Convert.ToDateTime(resumoImposto.COMPETENCIA).ToString("dd/MM/yyy");

                        Literal _litPIS = e.Row.FindControl("litPIS") as Literal;
                        if (_litPIS != null)
                            _litPIS.Text = "R$ " + resumoImposto.VAL_PIS.ToString("###,###,###,##0.00");

                        Literal _litCOFINS = e.Row.FindControl("litCOFINS") as Literal;
                        if (_litCOFINS != null)
                            _litCOFINS.Text = "R$ " + resumoImposto.VAL_COFINS.ToString("###,###,###,##0.00");

                        Literal _litICMS = e.Row.FindControl("litICMS") as Literal;
                        if (_litICMS != null)
                            _litICMS.Text = "R$ " + resumoImposto.VAL_ICMS.ToString("###,###,###,##0.00");

                        Literal _litINSS = e.Row.FindControl("litINSS") as Literal;
                        if (_litINSS != null)
                            _litINSS.Text = "R$ " + resumoImposto.VAL_INSS.ToString("###,###,###,##0.00");

                        Literal _litIRPJ = e.Row.FindControl("litIRPJ") as Literal;
                        if (_litIRPJ != null)
                            _litIRPJ.Text = "R$ " + resumoImposto.VAL_IRPJ.ToString("###,###,###,##0.00");

                        Literal _litCSLL = e.Row.FindControl("litCSLL") as Literal;
                        if (_litCSLL != null)
                            _litCSLL.Text = "R$ " + resumoImposto.VAL_CSLL.ToString("###,###,###,##0.00");

                        Literal _litSN = e.Row.FindControl("litSN") as Literal;
                        if (_litSN != null)
                            _litSN.Text = "R$ " + resumoImposto.VAL_SN.ToString("###,###,###,##0.00");

                        Literal _litTotal = e.Row.FindControl("litTotal") as Literal;
                        if (_litTotal != null)
                            _litTotal.Text = "R$ " + resumoImposto.TOTAL.ToString("###,###,###,##0.00");

                        tPIS += resumoImposto.VAL_PIS;
                        tCOFINS += resumoImposto.VAL_COFINS;
                        tICMS += resumoImposto.VAL_ICMS;
                        tINSS += resumoImposto.VAL_INSS;
                        tIRPJ += resumoImposto.VAL_IRPJ;
                        tCSLL += resumoImposto.VAL_CSLL;
                        tSN += resumoImposto.VAL_SN;
                        tTotal += resumoImposto.TOTAL;

                    }
                }
            }
        }
        protected void gvResumoImposto_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvResumoImposto.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                footer.Cells[2].Text = "R$ " + tPIS.ToString("###,###,###,##0.00");
                footer.Cells[3].Text = "R$ " + tCOFINS.ToString("###,###,###,##0.00");
                footer.Cells[4].Text = "R$ " + tICMS.ToString("###,###,###,##0.00");
                footer.Cells[5].Text = "R$ " + tINSS.ToString("###,###,###,##0.00");
                footer.Cells[6].Text = "R$ " + tIRPJ.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = "R$ " + tCSLL.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = "R$ " + tSN.ToString("###,###,###,##0.00");
                footer.Cells[9].Text = "R$ " + tTotal.ToString("###,###,###,##0.00");
            }
        }
        #endregion

        #region "IMPRESSAO REL"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho.";
                    return;
                }

                int anoDe = Convert.ToInt32(ddlAnoDe.SelectedValue);
                int mesDe = Convert.ToInt32(ddlMesDe.SelectedValue);

                int anoAte = Convert.ToInt32(ddlAnoAte.SelectedValue);
                int mesAte = Convert.ToInt32(ddlMesAte.SelectedValue);

                var resumoImposto = contabilController.ObterResumoImposto(anoDe, mesDe, anoAte, mesAte, ddlEmpresa.SelectedValue.Trim(), ddlFilial.SelectedValue.Trim());

                //IMPRESSAO
                StreamWriter wr = null;
                string nomeArquivo = "RESUMO_IMPOSTO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorio(resumoImposto).ToString());
                wr.Flush();
                wr.Close();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
        private StringBuilder MontarRelatorio(List<SP_OBTER_RESUMO_IMPOSTOResult> impostoResumo)
        {
            StringBuilder _texto = new StringBuilder();
            _texto = MontarResumoImposto(_texto, impostoResumo);
            return _texto;
        }
        private StringBuilder MontarResumoImposto(StringBuilder _texto, List<SP_OBTER_RESUMO_IMPOSTOResult> impostoResumo)
        {
            string titulo = "";

            tPIS = 0;
            tCOFINS = 0;
            tICMS = 0;
            tINSS = 0;
            tIRPJ = 0;
            tCSLL = 0;
            tSN = 0;
            tTotal = 0;

            _texto.Append("");
            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Resumo dos Impostos</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("    <style type='text/css'>");
            _texto.Append("        @media print {");
            _texto.Append("            .background-force {");
            _texto.Append("                -webkit-print-color-adjust: exact;");
            _texto.Append("            }");
            _texto.Append("        }");
            _texto.Append("    </style>");
            _texto.Append("</head>");
            _texto.Append("<body onload='window.print();'>");
            _texto.Append("    <div id='divImposto' align='center'>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
            _texto.Append("            padding: 0px; color: black; font-size: 8.0pt; font-weight: 600; font-family: Arial;");
            _texto.Append("            background: white; white-space: nowrap;'>");
            _texto.Append("            <tr>");
            _texto.Append("                <td style='line-height: 23px;'>");
            _texto.Append("                    <table border='1' cellpadding='1' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.Append("                        width: 1000pt'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='17' style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000; border-bottom:none;'>");
            _texto.Append("                                <div style='position:absolute;'>");
            _texto.Append("                                    <img alt='' width='200' src='http://cmaxweb.dnsalias.com:8585/image/logo.jpg' />");
            _texto.Append("                                </div>");
            _texto.Append("                                <h1>");
            _texto.Append("                                    RESUMO DE IMPOSTOS<br />");

            titulo = ddlMesDe.SelectedValue + "/" + ddlAnoDe.SelectedValue + "  à  " + ddlMesAte.SelectedValue + "/" + ddlAnoAte.SelectedValue;
            if (ddlFilial.SelectedValue.Trim() != "")
                titulo = ddlFilial.SelectedItem.Text.Trim() + " - " + titulo;

            if (ddlEmpresa.SelectedValue.Trim() != "")
                titulo = ddlEmpresa.SelectedValue.Trim() + " - " + titulo;

            _texto.Append("                                    " + titulo);
            _texto.Append("                                </h1>");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='17' style='line-height: 40px; border-top: none; border-bottom: none; '>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000; border-bottom: 1px solid #000; background-color: #FAF0E6;' class='background-force'>");
            _texto.Append("                            <td style='width:100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                COMPETÊNCIA");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 120px; border-right: 1px solid #000;' >");
            _texto.Append("                                PIS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 150px; border-right: 1px solid #000; '>");
            _texto.Append("                                COFINS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 150px; border-right: 1px solid #000; '>");
            _texto.Append("                                ICMS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 150px; border-right: 1px solid #000; '>");
            _texto.Append("                                INSS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 150px; border-right: 1px solid #000; '>");
            _texto.Append("                                IRPJ");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 150px; border-right: 1px solid #000; '>");
            _texto.Append("                                CSLL");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 130px; border-right: 1px solid #000; '>");
            _texto.Append("                                Simples Nacional");
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='width: 150px; border-right: 1px solid #000; '>");
            _texto.Append("                                Total");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            foreach (var imp in impostoResumo)
            {
                tPIS += imp.VAL_PIS;
                tCOFINS += imp.VAL_COFINS;
                tICMS += imp.VAL_ICMS;
                tINSS += imp.VAL_INSS;
                tIRPJ += imp.VAL_IRPJ;
                tCSLL += imp.VAL_CSLL;
                tSN += imp.VAL_SN;
                tTotal += imp.TOTAL;

                _texto.Append("                        <tr style='text-align: left; border-bottom: 1px solid #000;'>");
                _texto.Append("                            <td style='text-align: center; border-left: 1px solid #000; border-right: 1px solid #000;'>");
                _texto.Append("                                &nbsp;" + Convert.ToDateTime(imp.COMPETENCIA).ToString("dd/MM/yyyy"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.Append("                                " + imp.VAL_PIS.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.Append("                                " + imp.VAL_COFINS.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.Append("                                " + imp.VAL_ICMS.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.Append("                                " + imp.VAL_INSS.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.Append("                                " + imp.VAL_IRPJ.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.Append("                                " + imp.VAL_CSLL.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.Append("                                " + imp.VAL_SN.ToString("###,##0.00"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align: right; '>");
                _texto.Append("                               " + imp.TOTAL.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
            }

            _texto.Append("                        <tr style='text-align: left; border-bottom: 1px solid #000; background-color: #FAF0E6;' class='background-force'>");
            _texto.Append("                            <td style='text-align: center; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                &nbsp;TOTAL");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.Append("                                " + tPIS.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.Append("                                " + tCOFINS.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.Append("                                " + tICMS.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.Append("                                " + tINSS.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.Append("                                " + tIRPJ.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.Append("                                " + tCSLL.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.Append("                                " + tSN.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.Append("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align: right; '>");
            _texto.Append("                                " + tTotal.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                    </table>");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("        </table>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;

        }
        #endregion
    }
}
