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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class facc_resumo_atacadovarejo : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        List<SP_OBTER_FACCAO_RESUMOResult> faccaoResumoAux = new List<SP_OBTER_FACCAO_RESUMOResult>();
        int qtdeAtacado = 0;
        int contagemAtacado = 0;
        int qtdeVarejo = 0;
        int contagemVarejo = 0;
        int qtdeTotal = 0;

        int qtdeFaltante = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

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

                CarregarColecoes();
                CarregarServicosProducao();
                CarregarFornecedores();
                CarregarStatus(0);
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
        }

        private List<SP_OBTER_FACCAO_RESUMOResult> ObterResumoFaccao()
        {
            List<SP_OBTER_FACCAO_RESUMOResult> listaRetorno = new List<SP_OBTER_FACCAO_RESUMOResult>();
            char? mostruario = null;
            int? numeroHB = null;
            int? prodServico = null;

            if (txtHB.Text.Trim() != "")
                numeroHB = Convert.ToInt32(txtHB.Text.Trim());

            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            if (ddlServico.SelectedValue.Trim() != "0")
                prodServico = Convert.ToInt32(ddlServico.SelectedValue);

            var resumoHBF = faccController.ObterFaccaoResumo(ddlColecao.SelectedValue.Trim(), numeroHB, mostruario, ddlFornecedor.SelectedValue.Trim(), prodServico, ddlStatus.SelectedValue);
            listaRetorno.AddRange(resumoHBF);

            if (ddlStatus.SelectedValue == "")
                listaRetorno = listaRetorno.Where(p => p.STATUS.Trim() == "AG. ENTRADA" || p.STATUS.Trim() == "AG. ENTRADA ACAB").ToList();

            if (ddlSubStatus.SelectedValue != "")
                listaRetorno = listaRetorno.Where(p => p.SUBSTATUS.Trim() == ddlSubStatus.SelectedValue.Trim()).ToList();

            if (ddlFornecedorSub.SelectedValue.Trim() != "")
                listaRetorno = listaRetorno.Where(p => p.FORNECEDOR_SUB.Trim() == ddlFornecedorSub.SelectedValue.Trim()).ToList();

            //AUXILIAR PARA CONTAGEM DE ATACADO E VAREJO (REMOVE REGISTROS DUPLICADOS)
            faccaoResumoAux.AddRange(listaRetorno);
            faccaoResumoAux = faccaoResumoAux.GroupBy(p => new { COLECAO = p.COLECAO, HB = p.HB, MOSTRUARIO = p.MOSTRUARIO, QTDE_ATACADO = p.QTDE_ATACADO, QTDE_VAREJO = p.QTDE_VAREJO }).Select(x => new SP_OBTER_FACCAO_RESUMOResult
            {
                COLECAO = x.Key.COLECAO,
                HB = x.Key.HB,
                MOSTRUARIO = x.Key.MOSTRUARIO,
                QTDE_ATACADO = x.Key.QTDE_ATACADO,
                QTDE_VAREJO = x.Key.QTDE_VAREJO
            }).ToList();

            return listaRetorno.OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.MOSTRUARIO).ThenBy(p => p.PROD_PROCESSO).ThenBy(p => p.SERVICO).ToList();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvFaccaoHB.DataSource = ObterResumoFaccao();
                gvFaccaoHB.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            ddlServico.SelectedValue = "0";
            ddlFornecedor.SelectedValue = "";
            ddlColecao.SelectedValue = "";
            txtHB.Text = "";
            ddlMostruario.SelectedValue = "";
            ddlStatus.SelectedValue = "";
            ddlSubStatus.SelectedValue = "";
            ddlSubStatus.Visible = false;
            labSubStatus.Visible = false;

            labErro.Text = "";

            gvFaccaoHB.DataSource = new List<SP_OBTER_FACCAO_RESUMOResult>();
            gvFaccaoHB.DataBind();

        }

        protected void gvFaccaoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_RESUMOResult faccaoHB = e.Row.DataItem as SP_OBTER_FACCAO_RESUMOResult;

                    if (faccaoHB != null)
                    {
                        qtdeFaltante += Convert.ToInt32(faccaoHB.QTDE_FALTANTE);

                        Label _labColecao = e.Row.FindControl("labColecao") as Label;
                        if (_labColecao != null)
                            _labColecao.Text = faccaoHB.COLECAO.Trim() + faccaoHB.MOSTRUARIO.ToString();

                        Label _labEmissao = e.Row.FindControl("labEmissao") as Label;
                        if (_labEmissao != null)
                            if (faccaoHB.EMISSAO != null)
                                _labEmissao.Text = Convert.ToDateTime(faccaoHB.EMISSAO).ToString("dd/MM/yyyy");

                        Label _labTotal = e.Row.FindControl("labTotal") as Label;
                        if (_labTotal != null)
                            _labTotal.Text = (Convert.ToInt32(faccaoHB.QTDE_VAREJO) + Convert.ToInt32(faccaoHB.QTDE_ATACADO)).ToString();
                    }
                }
            }
        }
        protected void gvFaccaoHB_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFaccaoHB.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                foreach (var f in faccaoResumoAux)
                {
                    qtdeAtacado += Convert.ToInt32(f.QTDE_ATACADO);
                    qtdeVarejo += Convert.ToInt32(f.QTDE_VAREJO);
                    qtdeTotal += (Convert.ToInt32(f.QTDE_ATACADO) + Convert.ToInt32(f.QTDE_VAREJO));

                    if (f.QTDE_ATACADO != "" && Convert.ToInt32(f.QTDE_ATACADO) > 0)
                        contagemAtacado += 1;

                    if (f.QTDE_VAREJO != "" && Convert.ToInt32(f.QTDE_VAREJO) > 0)
                        contagemVarejo += 1;
                }

                labTotalAtacado.Text = contagemAtacado.ToString();
                labTotalVarejo.Text = contagemVarejo.ToString();

                _footer.Cells[6].Text = qtdeAtacado.ToString();
                _footer.Cells[7].Text = qtdeVarejo.ToString();
                _footer.Cells[8].Text = qtdeTotal.ToString();

                _footer.Cells[10].Text = qtdeFaltante.ToString();
            }
        }
        protected void gvFaccaoHB_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_RESUMOResult> _faccResumo = ObterResumoFaccao();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _faccResumo = _faccResumo.OrderBy(e.SortExpression + sortDirection);
            gvFaccaoHB.DataSource = _faccResumo;
            gvFaccaoHB.DataBind();
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }

        }
        private void CarregarFornecedoresSub(string fornecedor)
        {

            List<PROD_FORNECEDOR_SUB> _fornecedoresSub = faccController.ObterFornecedorSub(fornecedor);

            if (_fornecedoresSub != null)
            {
                _fornecedoresSub.Insert(0, new PROD_FORNECEDOR_SUB { FORNECEDOR_SUB = "" });

                ddlFornecedorSub.DataSource = _fornecedoresSub;
                ddlFornecedorSub.DataBind();
            }

        }
        private void CarregarServicosProducao()
        {
            List<PROD_SERVICO> _servico = new List<PROD_SERVICO>();
            _servico = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A' && p.CODIGO != 5).ToList();
            if (_servico != null)
            {
                _servico.Insert(0, new PROD_SERVICO { CODIGO = 0, DESCRICAO = "", STATUS = 'A' });
                ddlServico.DataSource = _servico;
                ddlServico.DataBind();
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string status = "";
            status = ddlStatus.SelectedValue.Trim();
            if (status == "AG. ENTRADA" || status == "AG. ENTRADA ACAB")
            {
                ddlSubStatus.Visible = true;
                labSubStatus.Visible = true;
            }
            else
            {
                ddlSubStatus.SelectedValue = "";
                ddlSubStatus.Visible = false;
                labSubStatus.Visible = false;
            }
        }
        protected void ddlServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codigoServico = 0;
            codigoServico = Convert.ToInt32(ddlServico.SelectedValue);
            CarregarStatus(codigoServico);
            ddlStatus_SelectedIndexChanged(null, null);
        }
        protected void ddlFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fornecedor = "";
            fornecedor = ddlFornecedor.SelectedValue.Trim();
            if (fornecedor == "HANDBOOK")
            {
                CarregarFornecedoresSub(fornecedor);
                ddlFornecedorSub.Visible = true;
                labFornecedorSub.Visible = true;
            }
            else
            {
                ddlFornecedorSub.SelectedValue = "";
                ddlFornecedorSub.Visible = false;
                labFornecedorSub.Visible = false;
            }
        }
        private void CarregarStatus(int codigoServico)
        {
            List<ListItem> lstStatus = new List<ListItem>();

            lstStatus.Add(new ListItem { Value = "", Text = "", Selected = true }); // OK

            if (codigoServico != 4)
                lstStatus.Add(new ListItem { Value = "AG. ENTRADA", Text = "AG. ENTRADA" });

            if (codigoServico == 0 || codigoServico == 4)
                lstStatus.Add(new ListItem { Value = "AG. ENTRADA ACAB", Text = "AG. ENTRADA ACAB" });

            ddlStatus.DataSource = lstStatus;
            ddlStatus.DataBind();

            if (lstStatus.Count() == 2)
                ddlStatus.SelectedIndex = 1;
        }
        #endregion

        #region "RELATÓRIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                List<SP_OBTER_FACCAO_RESUMOResult> faccaoResumo = ObterResumoFaccao();

                if (faccaoResumo.Count() > 0)
                {
                    GerarRelatorio(faccaoResumo);
                    labErro.Text = "Relatório gerado com sucesso.";
                }
                else
                {
                    labErro.Text = "Não existem registros para Facção.";
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void GerarRelatorio(List<SP_OBTER_FACCAO_RESUMOResult> faccaoResumo)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FACC_RESUMO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(faccaoResumo));
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
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_FACCAO_RESUMOResult> faccaoResumo)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarResumoFaccao(_texto, faccaoResumo);
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
            _texto.Append("         <title>Resumo Facção</title>   ");
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
            _texto.Append("     <div id='fichaResumoFacc' align='center' style='border: 0px solid #000;'>");
            _texto.Append("        <br />");
            _texto.Append("        <br />");
            _texto.Append("        <div align='center' style='border: 2px solid #000; background-color: transparent;");
            _texto.Append("            width: 720pt;'>");
            _texto.Append("            <table cellpadding='0' cellspacing='0' style='width: 721pt; padding: 0px; color: black;");
            _texto.Append("                font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white;");
            _texto.Append("                white-space: nowrap; border: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.Append("                <tr>");
            _texto.Append("                    <td colspan='2'>");
            _texto.Append("                        &nbsp;");
            _texto.Append("                    </td>");
            _texto.Append("                </tr>");
            _texto.Append("                <tr style='line-height:15px; text-align:center;'>");
            _texto.Append("                    <td style='width: 150px; text-align:center;'>");
            _texto.Append("                         <img alt='HBF' Width='50px' Height='50px' src='../../Image/hbf_branco.png' />");
            _texto.Append("                    </td>");
            _texto.Append("                    <td>");
            _texto.Append("                         &nbsp;&nbsp;&nbsp;&nbsp;<h2>");
            _texto.Append("                            HBF - FACÇÃO ATACADO/VAREJO - " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana));
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
        private StringBuilder MontarResumoFaccao(StringBuilder _texto, List<SP_OBTER_FACCAO_RESUMOResult> faccaoResumo)
        {
            _texto.Append("<tr style='line-height: 19px; text-align:center;'>");
            _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000; text-align:center;'>");
            _texto.Append("        <span style='font-size: 15px;'>&nbsp;</span>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='padding: 0px;");
            _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("            background: white; white-space: nowrap;'>");
            _texto.Append("            <tr style='line-height: 5px;'>");
            _texto.Append("                <td colspan='15'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr style='font-size:11px;'>");
            _texto.Append("                <td>Serviço</td>");
            _texto.Append("                <td>Fornecedor</td>");
            _texto.Append("                <td>Coleção</td>");
            _texto.Append("                <td>HB</td>");
            _texto.Append("                <td>Nome</td>");
            _texto.Append("                <td>Atacado</td>");
            _texto.Append("                <td>Varejo</td>");
            _texto.Append("                <td>Total</td>");
            _texto.Append("                <td>Emissão</td>");
            _texto.Append("                <td>Faltante</td>");
            _texto.Append("                <td>Status</td>");
            _texto.Append("            </tr>");

            foreach (var f in faccaoResumo)
            {
                _texto.Append("        <tr style='font-size:10px; text-align:center;'>");
                _texto.Append("            <td>" + f.SERVICO + "</td>");
                _texto.Append("            <td>" + f.FORNECEDOR + "</td>");
                _texto.Append("            <td>" + f.COLECAO + f.MOSTRUARIO.ToString() + "</td>");
                _texto.Append("            <td>" + f.HB + "</td>");
                _texto.Append("            <td>" + f.NOME + "</td>");
                _texto.Append("            <td>" + f.QTDE_ATACADO + "</td>");
                _texto.Append("            <td>" + f.QTDE_VAREJO + "</td>");
                _texto.Append("            <td>" + (Convert.ToInt32(f.QTDE_ATACADO) + Convert.ToInt32(f.QTDE_VAREJO)).ToString() + "</td>");
                _texto.Append("            <td>" + ((f.EMISSAO == null) ? "" : Convert.ToDateTime(f.EMISSAO).ToString("dd/MM/yyyy")) + "</td>");
                _texto.Append("            <td>" + f.QTDE_FALTANTE + "</td>");
                _texto.Append("            <td>" + f.STATUS_ATACADOVAREJO + "</td>");
                _texto.Append("        </tr>");
            }

            _texto.Append("        </table>");
            _texto.Append("    </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        #endregion



    }
}
