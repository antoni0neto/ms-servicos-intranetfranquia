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
    public partial class fisc_declaracao_faturamento : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        decimal tvalor = 0;
        decimal tvalorInter = 0;

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
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
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
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                txtDescTribut.Text = "";
                var empresa = baseController.ObterEmpresa(ddlEmpresa.SelectedValue);
                if (empresa != null)
                    txtDescTribut.Text = empresa.DESC_TRIBUTACAO;
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais_Intermediario();
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

            return retorno;
        }
        #endregion

        #region "DECLARACAO FAT"
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

                DateTime data = DateTime.Now;
                string mes = ddlMes.SelectedValue;
                string ano = ddlAno.SelectedValue;

                data = Convert.ToDateTime(ano + "-" + mes + "-01");

                CarregarDeclaracaoFAT(ddlEmpresa.SelectedValue.Trim(), data, ddlFilial.SelectedValue);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarDeclaracaoFAT(string empresa, DateTime data, string codFilial)
        {
            //Obter Declaração de FATURAMENTO
            var decFAT = contabilController.ObterDeclaracaoFAT(empresa, data, codFilial);

            gvDeclaracaoFAT.DataSource = decFAT;
            gvDeclaracaoFAT.DataBind();
        }
        protected void gvDeclaracaoFAT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DEC_FATURAMENTOResult decFAT = e.Row.DataItem as SP_OBTER_DEC_FATURAMENTOResult;

                    if (decFAT != null)
                    {
                        Literal _litValor = e.Row.FindControl("litValor") as Literal;
                        if (_litValor != null)
                        {
                            _litValor.Text = "R$  " + decFAT.VALOR.ToString("###,###,###,###,##0.00");
                            tvalor += decFAT.VALOR;
                        }

                        Literal _litValorIntercompany = e.Row.FindControl("litValorIntercompany") as Literal;
                        if (_litValorIntercompany != null)
                        {
                            _litValorIntercompany.Text = "R$  " + decFAT.VALOR_INTER.ToString("###,###,###,###,##0.00");
                            tvalorInter += decFAT.VALOR_INTER;
                        }
                    }
                }
            }
        }
        protected void gvDeclaracaoFAT_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvDeclaracaoFAT.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells[2].Text = "R$  " + tvalor.ToString("###,###,###,###,##0.00");
                footer.Cells[3].Text = "R$  " + tvalorInter.ToString("###,###,###,###,##0.00");
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

                DateTime data = DateTime.Now;
                string mes = ddlMes.SelectedValue;
                string ano = ddlAno.SelectedValue;
                string empresa = ddlEmpresa.SelectedValue.Trim();
                string codFilial = ddlFilial.SelectedValue;

                data = Convert.ToDateTime(ano + "-" + mes + "-01");

                var decFAT = contabilController.ObterDeclaracaoFAT(empresa, data, codFilial);

                //IMPRESSAO
                StreamWriter wr = null;
                string nomeArquivo = "DECLAR_FAT_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorio(decFAT).ToString());
                wr.Flush();
                wr.Close();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
        private StringBuilder MontarRelatorio(List<SP_OBTER_DEC_FATURAMENTOResult> decFAT)
        {
            StringBuilder _texto = new StringBuilder();
            _texto = MontarDeclaracaoFAT(_texto, decFAT);
            return _texto;
        }
        private StringBuilder MontarDeclaracaoFAT(StringBuilder _texto, List<SP_OBTER_DEC_FATURAMENTOResult> decFAT)
        {
            var empresa = baseController.ObterEmpresa(ddlEmpresa.SelectedValue.Trim());
            var empresaAssinante = baseController.ObterEmpresaAssinante(empresa.CODIGO_EMPRESA);

            string periodoIni = RetornarMesAnoExtenso(decFAT[0].COMPETENCIA);
            string pediodoFim = RetornarMesAnoExtenso(decFAT[decFAT.Count() - 1].COMPETENCIA);

            _texto.Append("");
            _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append("<html>");
            _texto.Append("<head>");
            _texto.Append("    <title>Declaração de Faturamento</title>");
            _texto.Append("    <meta charset='UTF-8' />");
            _texto.Append("    <style type='text/css'>");
            _texto.Append("        @media print {");
            _texto.Append("            .background-force {");
            _texto.Append("                -webkit-print-color-adjust: exact;");
            _texto.Append("           }");
            _texto.Append("        }");
            _texto.Append("    </style>");
            _texto.Append("</head>");
            _texto.Append("<body onload='window.print();'>");
            _texto.Append("    <div id='divFaturamento' align='center'>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt; padding: 0px; color: black;");
            _texto.Append("            font-size: 11.0pt; font-weight: 600; font-family: Arial; background: white;'>");
            _texto.Append("            <tr>");
            _texto.Append("                <td style='line-height: 23px;'>");
            _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt'>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3' style='border:none;'>");
            _texto.Append("                                <div style=''>");
            _texto.Append("                                    <img alt='' width='200' src='http://cmaxweb.dnsalias.com:8585/image/logo.jpg' />");
            _texto.Append("                                </div>");
            _texto.Append("                                <h1>&nbsp;</h1>");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3' style='line-height: 15px; border: none; '>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='border: none;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border: none; text-align:justify; '>");
            _texto.Append("                                <p style='font-size:medium;'>");
            _texto.Append("                                    Declaramos para os devidos fins de direito e informamos que com base nos valores apurados, a empresa");
            _texto.Append("                                    denominada " + empresa.NOME.Trim() + ", inscrita no CNPJ sob Nº " + empresa.CNPJ + ", teve o seguinte faturamento no");
            _texto.Append("                                    período de " + periodoIni + " à " + pediodoFim + "");
            if (txtDescTribut.Text.Trim() != "")
                _texto.Append("                                     , no qual optou por ser tributado pelo " + txtDescTribut.Text.Trim().ToUpper() + ".");
            else
                _texto.Append("                                     .");

            _texto.Append("                                </p>");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border: none;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3' style='line-height: 40px; border: none; '>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='border: none;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align:center; width:100%;'>");
            _texto.Append("                                    <div style='margin-left:25%;'>");
            _texto.Append("                                        <table border='1' cellpadding='0' cellspacing='0' style='width: 350px;'>");
            _texto.Append("                                            <tr style='background-color: #FAF0E6;' class='background-force'>");
            _texto.Append("                                                <td style='width: 150px; border: none;'>");
            _texto.Append("                                                    MÊS");
            _texto.Append("                                                </td>");
            _texto.Append("                                                <td colspan='2' style='border: none; border-left: solid 1px #000;'>");
            _texto.Append("                                                    VALOR");
            _texto.Append("                                                </td>");
            _texto.Append("                                            </tr>");
            foreach (var d in decFAT)
            {
                _texto.Append("                                            <tr>");
                _texto.Append("                                                <td style='border-bottom: none; border-left: none; border-right: none; '>");
                _texto.Append("                                                    " + d.COMPETENCIA);
                _texto.Append("                                                </td>");
                _texto.Append("                                                <td style='text-align:left; border-top: 1px solid #000; border-bottom:none; border-right: none;'>");
                _texto.Append("                                                    &nbsp;R$");
                _texto.Append("                                                </td>");
                _texto.Append("                                                <td style='text-align: right; border: 1px solid #000; border-left: none; border-bottom: none; border-right: none; '>");
                _texto.Append("                                                    " + d.VALOR.ToString("###,###,###,###,##0.00") + "&nbsp;");
                _texto.Append("                                                </td>");
                _texto.Append("                                            </tr>");

                tvalor += d.VALOR;
            }

            _texto.Append("                                            <tr style='background-color: #FAF0E6;' class='background-force'>");
            _texto.Append("                                                <td style='border-bottom: none; border-left: none; border-right: none; '>");
            _texto.Append("                                                    TOTAL");
            _texto.Append("                                                </td>");
            _texto.Append("                                                <td style='text-align:left; border-top: 1px solid #000; border-bottom:none; border-right: none;'>");
            _texto.Append("                                                    &nbsp;R$");
            _texto.Append("                                                </td>");
            _texto.Append("                                                <td style='text-align: right; border: 1px solid #000; border-left: none; border-bottom: none; border-right: none; '>");
            _texto.Append("                                                    " + tvalor.ToString("###,###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                                                </td>");
            _texto.Append("                                            </tr>");

            _texto.Append("                                        </table>");
            _texto.Append("                                    </div>");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border: none;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3' style='line-height: 45px; border: none; '>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='text-align:center'>");
            _texto.Append("                            <td colspan='3' style='line-height: 40px; border: none; '>");
            _texto.Append("                                <span style='font-size:medium;'> " + ((empresa.NOME.Trim().ToUpper().Contains("GABAZEITU COMERCIO DE CONFECCOES")) ? "Uberlândia (MG)" : "São Paulo") + ", " + DateTime.Now.Day + " de " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month)) + " de " + DateTime.Now.Year + "</span>");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3' style='line-height: 70px; border: none; '>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td style='border: none;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border: none; '>");
            _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' width='100%'>");
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td width='50px;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");

            var ass1 = empresaAssinante.FirstOrDefault();
            if (ass1 != null)
            {
                _texto.Append("                                        <td>");
                _texto.Append("                                            ___________________________<br />");
                _texto.Append("                                            <b>" + ass1.ASSINANTE + "</b><br />");
                _texto.Append("                                            " + ass1.ASSINANTE_COD + "<br />");
                _texto.Append("                                            CPF: " + ass1.CPF + "<br />");
                _texto.Append("                                            Tel.: " + ass1.TELEFONE + "<br />");
                _texto.Append("                                        </td>");
            }
            else
            {
                _texto.Append("                                        <td>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
            }


            _texto.Append("                                        <td  style='width:150px;'>");
            _texto.Append("                                            &nbsp;");
            _texto.Append("                                        </td>");

            var ass2 = empresaAssinante.LastOrDefault();
            if (ass2 != null)
            {
                _texto.Append("                                        <td>");
                _texto.Append("                                            ___________________________<br />");
                _texto.Append("                                            <b>" + ass2.ASSINANTE + "</b><br />");
                _texto.Append("                                            " + ass2.ASSINANTE_COD + "<br />");
                _texto.Append("                                            CPF: " + ass2.CPF + "<br />");
                _texto.Append("                                            &nbsp;" + ass2.TELEFONE + "<br />");
                _texto.Append("                                        </td>");
            }
            else
            {
                _texto.Append("                                        <td>");
                _texto.Append("                                            &nbsp;");
                _texto.Append("                                        </td>");
            }
            _texto.Append("                                    </tr>");
            _texto.Append("                                </table>");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border: none;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr>");
            _texto.Append("                            <td colspan='3' style='line-height: 20px; border-top: none; border-bottom: none; '>");
            _texto.Append("                                &nbsp;");
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

        private string RetornarMesAnoExtenso(string competencia)
        {
            string mes = "";
            string ano = "";

            string[] comp = competencia.Split('/');

            mes = Utils.WebControls.AlterarPrimeiraLetraMaiscula(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(comp[0])));
            ano = comp[1];

            return mes + " de " + ano;
        }
        #endregion


    }
}
