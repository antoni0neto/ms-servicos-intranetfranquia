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
    public partial class facc_resumo_grupo_faltante : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

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
                CarregarGrupo();
                CarregarServicosProducao();
                CarregarFornecedores();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
        }

        private List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> ObterFaccaoGrupo()
        {
            List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> listaRetorno = new List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult>();
            char? mostruario = null;
            int? prodServico = null;

            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            if (ddlServico.SelectedValue.Trim() != "0")
                prodServico = Convert.ToInt32(ddlServico.SelectedValue);

            var grupoHBF = faccController.ObterFaccaoGrupo(ddlGrupo.SelectedValue.Trim(), ddlColecao.SelectedValue, prodServico, ddlFornecedor.SelectedValue.Trim(), ddlFornecedorSub.SelectedValue.Trim(), mostruario);

            if (ddlEmpresa.SelectedValue == "" || ddlEmpresa.SelectedValue == "HANDBOOK")
                listaRetorno.AddRange(grupoHBF);

            return listaRetorno;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvFaccaoGrupo.DataSource = ObterFaccaoGrupo();
                gvFaccaoGrupo.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvFaccaoGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_GRUPO_PRODUTOResult faccaoGrupo = e.Row.DataItem as SP_OBTER_FACCAO_GRUPO_PRODUTOResult;

                    if (faccaoGrupo != null)
                    {
                        qtdeFaltante += Convert.ToInt32(faccaoGrupo.QTDE_FALTANTE);
                    }
                }
            }
        }
        protected void gvFaccaoGrupo_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFaccaoGrupo.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[3].Text = qtdeFaltante.ToString();
            }
        }
        protected void gvFaccaoGrupo_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> _faccGrupo = ObterFaccaoGrupo();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _faccGrupo = _faccGrupo.OrderBy(e.SortExpression + sortDirection);
            gvFaccaoGrupo.DataSource = _faccGrupo;
            gvFaccaoGrupo.DataBind();
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
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
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
        protected void ddlFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fornecedor = "";
            fornecedor = ddlFornecedor.SelectedItem.Text.Trim();
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
        #endregion

        #region "RELATÓRIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> faccaoGrupo = ObterFaccaoGrupo();

                if (faccaoGrupo.Count() > 0)
                {
                    GerarRelatorio(faccaoGrupo);
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
        private void GerarRelatorio(List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> faccaoGrupo)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FACC_GRUPO_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(faccaoGrupo));
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
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> faccaoGrupo)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarFaccaoGrupo(_texto, faccaoGrupo);
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
            _texto.Append("         <title>Grupo Facção</title>   ");
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
            _texto.Append("     <div id='fichaGrupoFacc' align='center' style='border: 0px solid #000;'>");
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
            _texto.Append("                            HBF - FACÇÃO por GRUPO- " + DateTime.Now.ToString("dd/MM/yyyy") + " - " + Utils.WebControls.AlterarPrimeiraLetraMaiscula(dia_da_semana));
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
        private StringBuilder MontarFaccaoGrupo(StringBuilder _texto, List<SP_OBTER_FACCAO_GRUPO_PRODUTOResult> faccaoGrupo)
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
            _texto.Append("                <td>Grupo</td>");
            _texto.Append("                <td>Empresa</td>");
            _texto.Append("                <td>Quantidade Faltante</td>");
            _texto.Append("            </tr>");

            foreach (var f in faccaoGrupo)
            {
                _texto.Append("        <tr style='font-size:10px; text-align:center;'>");
                _texto.Append("            <td>" + f.GRUPO + "</td>");
                _texto.Append("            <td>" + f.EMPRESA + "</td>");
                _texto.Append("            <td>" + f.QTDE_FALTANTE + "</td>");
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
