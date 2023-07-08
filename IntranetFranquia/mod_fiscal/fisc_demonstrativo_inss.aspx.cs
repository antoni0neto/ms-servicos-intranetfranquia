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
    public partial class fisc_demonstrativo_inss : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        decimal tfatOperacional = 0;
        decimal tfatNAOOperacional = 0;
        decimal tdevVenda = 0;
        decimal tretMercNAOEntregue = 0;
        decimal tbaseCalculo = 0;
        decimal tvalorINSS = 0;

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
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        private void CarregarEmpresa()
        {
            var empresa = baseController.ObterEmpresa().OrderBy(p => p.NOME).ToList();

            empresa.Insert(0, new EMPRESA { NOME = "Selecione" });
            ddlEmpresa.DataSource = empresa;
            ddlEmpresa.DataBind();
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labEmpresa.ForeColor = _OK;
            if (ddlEmpresa.SelectedValue.Trim() == "Selecione")
            {
                labEmpresa.ForeColor = _notOK;
                retorno = false;
            }

            labMes.ForeColor = _OK;
            if (ddlMes.SelectedValue.Trim() == "")
            {
                labMes.ForeColor = _notOK;
                retorno = false;
            }

            labAliquota.ForeColor = _OK;
            if (txtAliquota.Text.Trim() == "")
            {
                labAliquota.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "DESONERACAO INSS"
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

                CarregarFaturamentoDesoneracao();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private List<SP_OBTER_FAT_DESONERACAOResult> ObterFaturamentoDesoneracao()
        {

            DateTime dataIni = DateTime.Now;
            DateTime dataFim = DateTime.Now;
            string mes = ddlMes.SelectedValue;
            string ano = ddlAno.SelectedValue;
            string empresa = ddlEmpresa.SelectedValue.Trim();

            int diaFim = DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes));

            dataIni = Convert.ToDateTime(ano + "-" + mes + "-01");
            dataFim = Convert.ToDateTime(ano + "-" + mes + "-" + diaFim.ToString());

            var inss = contabilController.ObterFaturamentoDesoneracao(empresa, dataIni, dataFim, "").Where(p => p.FILIAL.Trim().ToUpper() != "SCS C-MAX").ToList();

            return inss;
        }
        private void CarregarFaturamentoDesoneracao()
        {
            //Obter Faturamento INSS
            var inss = ObterFaturamentoDesoneracao();

            gvINSS.DataSource = inss;
            gvINSS.DataBind();
        }
        protected void gvINSS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal baseCalculo = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FAT_DESONERACAOResult fatINSS = e.Row.DataItem as SP_OBTER_FAT_DESONERACAOResult;

                    if (fatINSS != null)
                    {
                        Literal _litCNPJ = e.Row.FindControl("litCNPJ") as Literal;
                        if (_litCNPJ != null)
                            _litCNPJ.Text = Convert.ToUInt64(fatINSS.CGC_CPF.Trim()).ToString(@"00\.000\.000\/0000\-00");

                        Literal _litFATOperacional = e.Row.FindControl("litFATOperacional") as Literal;
                        if (_litFATOperacional != null)
                        {
                            _litFATOperacional.Text = "R$  " + Convert.ToDecimal(fatINSS.FAT_OPERACIONAL).ToString("###,###,###,###,##0.00");
                            tfatOperacional += Convert.ToDecimal(fatINSS.FAT_OPERACIONAL);
                        }

                        Literal _litFATNAOOperacional = e.Row.FindControl("litFATNAOOperacional") as Literal;
                        if (_litFATNAOOperacional != null)
                        {
                            _litFATNAOOperacional.Text = "R$  " + Convert.ToDecimal(fatINSS.FAT_N_OPERACIONAL).ToString("###,###,###,###,##0.00");
                            tfatNAOOperacional += Convert.ToDecimal(fatINSS.FAT_N_OPERACIONAL);
                        }

                        Literal _litDEVVendas = e.Row.FindControl("litDEVVendas") as Literal;
                        if (_litDEVVendas != null)
                        {
                            _litDEVVendas.Text = "R$  " + Convert.ToDecimal(fatINSS.DEVOLUCAO_VENDA).ToString("###,###,###,###,##0.00");
                            tdevVenda += Convert.ToDecimal(fatINSS.DEVOLUCAO_VENDA);
                        }

                        Literal _litRETMercNAOEntregue = e.Row.FindControl("litRETMercNAOEntregue") as Literal;
                        if (_litRETMercNAOEntregue != null)
                        {
                            _litRETMercNAOEntregue.Text = "R$  " + Convert.ToDecimal(fatINSS.RET_MERC_N_ENTREGUE).ToString("###,###,###,###,##0.00");
                            tretMercNAOEntregue += Convert.ToDecimal(fatINSS.RET_MERC_N_ENTREGUE);
                        }

                        Literal _litBaseCalculo = e.Row.FindControl("litBaseCalculo") as Literal;
                        if (_litBaseCalculo != null)
                        {
                            baseCalculo = Convert.ToDecimal(fatINSS.FAT_OPERACIONAL + fatINSS.FAT_N_OPERACIONAL - fatINSS.DEVOLUCAO_VENDA - fatINSS.RET_MERC_N_ENTREGUE);
                            _litBaseCalculo.Text = "R$  " + baseCalculo.ToString("###,###,###,###,##0.00");
                            tbaseCalculo += baseCalculo;
                        }

                        Literal _litAliquota = e.Row.FindControl("litAliquota") as Literal;
                        if (_litAliquota != null)
                            _litAliquota.Text = txtAliquota.Text;

                        Literal _litValorINSS = e.Row.FindControl("litValorINSS") as Literal;
                        if (_litValorINSS != null)
                        {
                            _litValorINSS.Text = "R$  " + (baseCalculo * (Convert.ToDecimal(txtAliquota.Text) / 100)).ToString("###,###,###,###,##0.00");
                            tvalorINSS += (baseCalculo * (Convert.ToDecimal(txtAliquota.Text) / 100));
                        }
                    }
                }
            }
        }
        protected void gvINSS_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvINSS.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[3].Text = "R$  " + tfatOperacional.ToString("###,###,###,###,##0.00");
                footer.Cells[4].Text = "R$  " + tfatNAOOperacional.ToString("###,###,###,###,##0.00");
                footer.Cells[5].Text = "R$  " + tdevVenda.ToString("###,###,###,###,##0.00");
                footer.Cells[6].Text = "R$  " + tretMercNAOEntregue.ToString("###,###,###,###,##0.00");
                footer.Cells[7].Text = "R$  " + tbaseCalculo.ToString("###,###,###,###,##0.00");
                footer.Cells[9].Text = "R$  " + tvalorINSS.ToString("###,###,###,###,##0.00");
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

                var inss = ObterFaturamentoDesoneracao();

                //IMPRESSAO
                StreamWriter wr = null;
                string nomeArquivo = "DECLAR_FAT_INSS_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorio(inss).ToString());
                wr.Flush();
                wr.Close();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
        private StringBuilder MontarRelatorio(List<SP_OBTER_FAT_DESONERACAOResult> inss)
        {
            StringBuilder _texto = new StringBuilder();
            _texto = MontarDeclaracaoINSS(_texto, inss);
            return _texto;
        }
        private StringBuilder MontarDeclaracaoINSS(StringBuilder _texto, List<SP_OBTER_FAT_DESONERACAOResult> inss)
        {
            _texto.AppendLine("");
            _texto.AppendLine("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>Declaração de INSS</title>");
            _texto.AppendLine("    <meta charset='UTF-8' />");
            _texto.AppendLine("    <style type='text/css'>");
            _texto.AppendLine("        @media print {");
            _texto.AppendLine("            .background-force {");
            _texto.AppendLine("                -webkit-print-color-adjust: exact;");
            _texto.AppendLine("            }");
            _texto.AppendLine("        }");
            _texto.AppendLine("    </style>");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload='window.print();'>");
            _texto.AppendLine("    <div id='divINSS' align='center'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
            _texto.AppendLine("            padding: 0px; color: black; font-size: 8.0pt; font-weight: 600; font-family: Arial;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 23px;'>");
            _texto.AppendLine("                    <table border='1' cellpadding='1' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 800pt'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='15' style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000; border-bottom:none;'>");
            _texto.AppendLine("                                <div style='position:absolute;'>");
            _texto.AppendLine("                                    <img alt='' width='200' src='http://cmaxweb.dnsalias.com:8585/image/logo.jpg' />");
            _texto.AppendLine("                                </div>");
            _texto.AppendLine("                                <h1>");
            _texto.AppendLine("                                    DEMONSTRATIVO INSS<br />");
            _texto.AppendLine("                                    " + ddlEmpresa.SelectedValue.Trim() + " - " + ddlMes.SelectedValue + "/" + ddlAno.SelectedValue + "");
            _texto.AppendLine("                                </h1>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='15' style='line-height: 40px; border-top: none; border-bottom: none; '>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='text-align: center; border-bottom: 1px solid #000;' class='background-force'>");
            _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-top:none;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='4' style='border-right: 1px solid #000; background-color: #FFDAB9;'>");
            _texto.AppendLine("                                RECEITAS DE VENDAS");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='4' style='border-right: 1px solid #000; background-color: #FFDAB9;'>");
            _texto.AppendLine("                                VENDAS CANCELADAS");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='5' style='border-right: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000; border-bottom: 1px solid #000; background-color: #FAF0E6;' class='background-force'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                FILIAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 115px; border-right: 1px solid #000; text-align:center;'>");
            _texto.AppendLine("                                CNPJ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='width: 135px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                FAT. OPERACIONAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='width: 140px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                FAT. NÃO OPERACIONAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='width: 145px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                DEVOLUÇÕES DE VENDAS");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='width: 145px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                RET. MERC. NÃO ENTREGUE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='width: 135px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                BASE DE CÁLCULO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 80px; border-right: 1px solid #000; text-align:center;'>");
            _texto.AppendLine("                                ALÍQUOTA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='width: 130px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                VALOR DO INSS");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            decimal baseCalculo = 0;
            decimal valorINSS = 0;
            decimal aliquota = Convert.ToDecimal(txtAliquota.Text.Trim());
            foreach (var i in inss)
            {
                baseCalculo = (i.FAT_OPERACIONAL + i.FAT_N_OPERACIONAL - i.DEVOLUCAO_VENDA - i.RET_MERC_N_ENTREGUE);
                valorINSS = (baseCalculo * (aliquota / 100));

                tfatOperacional += i.FAT_OPERACIONAL;
                tfatNAOOperacional += i.FAT_N_OPERACIONAL;
                tdevVenda += i.DEVOLUCAO_VENDA;
                tretMercNAOEntregue += i.RET_MERC_N_ENTREGUE;
                tbaseCalculo += baseCalculo;
                tvalorINSS += valorINSS;

                _texto.AppendLine("                        <tr style='text-align: left; border-bottom: 1px solid #000;'>");
                _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;" + i.FILIAL);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align:center;'>");
                _texto.AppendLine("                                " + Convert.ToUInt64(i.CGC_CPF.Trim()).ToString(@"00\.000\.000\/0000\-00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.AppendLine("                                " + i.FAT_OPERACIONAL.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.AppendLine("                                " + i.FAT_N_OPERACIONAL.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.AppendLine("                                " + i.DEVOLUCAO_VENDA.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.AppendLine("                                " + i.RET_MERC_N_ENTREGUE.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
                _texto.AppendLine("                                " + baseCalculo.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000; font-size: 9pt; text-align: center;'>");
                _texto.AppendLine("                                " + aliquota.ToString("###,##0.00"));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
                _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align: right; '>");
                _texto.AppendLine("                               " + valorINSS.ToString("###,###,###,##0.00") + "&nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }

            _texto.AppendLine("                        <tr style='text-align: left; border-bottom: 1px solid #000; background-color: #FAF0E6;' class='background-force'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align:center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.AppendLine("                                " + tfatOperacional.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.AppendLine("                                " + tfatNAOOperacional.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.AppendLine("                                " + tdevVenda.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.AppendLine("                                " + tretMercNAOEntregue.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align:right;'>");
            _texto.AppendLine("                                " + tbaseCalculo.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; font-size: 9pt; text-align: center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right:none;'>&nbsp;R$</td>");
            _texto.AppendLine("                            <td style='border-left: none; border-right: 1px solid #000; font-size: 9pt; text-align: right; '>");
            _texto.AppendLine("                                " + tvalorINSS.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");

            return _texto;
        }
        #endregion
    }
}
