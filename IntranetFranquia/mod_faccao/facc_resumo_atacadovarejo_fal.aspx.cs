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
    public partial class facc_resumo_atacadovarejo_fal : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        List<PROD_HB_FACCAO_PROBLEMA> gFaccProblema = new List<PROD_HB_FACCAO_PROBLEMA>();
        List<SP_OBTER_FACCAO_ATAC_VAR_FALResult> faccaoResumoAux = new List<SP_OBTER_FACCAO_ATAC_VAR_FALResult>();
        //int qtdeAtacado = 0;
        //int contagemAtacado = 0;
        int qtdeVarejo = 0;
        int contagemVarejo = 0;

        //int qtdeFaltanteAtacado = 0;
        int qtdeFaltanteVarejo = 0;

        const string FACC_REL_ATAC_VAR_FAL = "FACC_REL_ATAC_VAR_FAL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataPrevFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "facc_menu_relatorio.aspx";
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_prodacab/pacab_menu.aspx";

                string codigoServico = (Request.QueryString["s"] == null) ? "" : Request.QueryString["s"].ToString();
                string fornecedor = (Request.QueryString["f"] == null) ? "" : Request.QueryString["f"].ToString();

                CarregarColecoes();
                CarregarServicosProducao(codigoServico);
                CarregarFornecedores(fornecedor);
                CarregarFaccaoProblema();
                CarregarGriffe();
                CarregarGrupo();

                Session[FACC_REL_ATAC_VAR_FAL] = null;

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btImprimir.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btImprimir, null) + ";");
            btExcel.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btExcel, null) + ";");
        }

        private List<SP_OBTER_FACCAO_ATAC_VAR_FALResult> ObterResumoAtacVarejoFaltante()
        {
            char? mostruario = null;
            int? numeroHB = null;
            int? prodServico = null;
            string fornecedor = "";

            if (txtHB.Text.Trim() != "")
                numeroHB = Convert.ToInt32(txtHB.Text.Trim());

            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            if (ddlServico.SelectedValue.Trim() != "0")
                prodServico = Convert.ToInt32(ddlServico.SelectedValue);

            if (ddlFornecedor.SelectedValue.Trim() != "0")
                fornecedor = ddlFornecedor.SelectedItem.Text.Trim();

            var faccaoFaltante = faccController.ObterFaccaoAtacadoVarejoFaltante(ddlColecao.SelectedValue.Trim(), numeroHB, mostruario, prodServico, fornecedor, Convert.ToInt32(ddlFaccaoProblema.SelectedValue)).Where(p => p.QTDE_ATACADO_FALTANTE > 0 || p.QTDE_VAREJO_FALTANTE > 0 || p.QTDE_FALTANTE > 0).AsQueryable();

            if (ddlFornecedorSub.SelectedValue.Trim() != "")
                faccaoFaltante = faccaoFaltante.Where(p => p.FORNECEDOR_SUB == ddlFornecedorSub.SelectedValue.Trim()).AsQueryable();

            if (ddlGriffe.SelectedValue.Trim() != "")
                faccaoFaltante = faccaoFaltante.Where(p => p.GRIFFE == ddlGriffe.SelectedValue.Trim()).AsQueryable();

            if (ddlGrupo.SelectedValue.Trim() != "")
                faccaoFaltante = faccaoFaltante.Where(p => p.GRUPO_PRODUTO == ddlGrupo.SelectedValue.Trim()).AsQueryable();

            if (txtDataPrevIni.Text.Trim() != "")
                faccaoFaltante = faccaoFaltante.Where(p => p.DATA_PREV_ENTREGA != null && p.DATA_PREV_ENTREGA >= Convert.ToDateTime(txtDataPrevIni.Text.Trim())).AsQueryable();

            if (txtDataPrevFim.Text.Trim() != "")
                faccaoFaltante = faccaoFaltante.Where(p => p.DATA_PREV_ENTREGA != null && p.DATA_PREV_ENTREGA <= Convert.ToDateTime(txtDataPrevFim.Text.Trim())).AsQueryable();

            if (ddlRemoverHandbook.SelectedValue != "")
                faccaoFaltante = faccaoFaltante.Where(p => p.FORNECEDOR.ToLower().Trim() != "handbook").AsQueryable();

            //AUXILIAR PARA CONTAGEM DE ATACADO E VAREJO (REMOVE REGISTROS DUPLICADOS)
            faccaoResumoAux.AddRange(faccaoFaltante);
            faccaoResumoAux = faccaoResumoAux.GroupBy(p => new { COLECAO = p.COLECAO, HB = p.HB, MOSTRUARIO = p.MOSTRUARIO, QTDE_ATACADO = p.QTDE_ATACADO, QTDE_VAREJO = p.QTDE_VAREJO, QTDE_TOTAL = p.QTDE_TOTAL }).Select(x => new SP_OBTER_FACCAO_ATAC_VAR_FALResult
            {
                COLECAO = x.Key.COLECAO,
                HB = x.Key.HB,
                MOSTRUARIO = x.Key.MOSTRUARIO,
                QTDE_ATACADO = x.Key.QTDE_ATACADO,
                QTDE_VAREJO = x.Key.QTDE_VAREJO,
                QTDE_TOTAL = x.Key.QTDE_TOTAL
            }).ToList();

            return faccaoFaltante.ToList();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gFaccProblema = ObterFaccaoProblema();


                var faccaoFal = ObterResumoAtacVarejoFaltante();
                gvFaccaoHB.DataSource = faccaoFal;
                gvFaccaoHB.DataBind();

                Session[FACC_REL_ATAC_VAR_FAL] = faccaoFal;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            ddlServico.SelectedValue = "0";
            ddlColecao.SelectedValue = "";
            txtHB.Text = "";
            ddlMostruario.SelectedValue = "";
            ddlGriffe.SelectedValue = "";
            ddlGrupo.SelectedValue = "";
            ddlFornecedor.SelectedValue = "0";
            txtDataPrevIni.Text = "";
            txtDataPrevFim.Text = "";

            labErro.Text = "";

            gvFaccaoHB.DataSource = new List<SP_OBTER_FACCAO_ATAC_VAR_FALResult>();
            gvFaccaoHB.DataBind();

        }

        protected void gvFaccaoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_ATAC_VAR_FALResult faccaoHB = e.Row.DataItem as SP_OBTER_FACCAO_ATAC_VAR_FALResult;

                    if (faccaoHB != null)
                    {
                        ImageButton imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                        if (File.Exists(Server.MapPath(faccaoHB.FOTO1)))
                            imgProduto.ImageUrl = faccaoHB.FOTO1;
                        else if (File.Exists(Server.MapPath(faccaoHB.FOTO2)))
                            imgProduto.ImageUrl = faccaoHB.FOTO2;
                        else if (File.Exists(Server.MapPath(faccaoHB.FOTO3)))
                            imgProduto.ImageUrl = faccaoHB.FOTO3;
                        else
                            imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                        qtdeFaltanteVarejo += Convert.ToInt32(faccaoHB.QTDE_VAREJO_FALTANTE);

                        Label labFaltanteTotal = e.Row.FindControl("labFaltanteTotal") as Label;
                        if (labFaltanteTotal != null)
                            labFaltanteTotal.Text = (faccaoHB.QTDE_ATACADO_FALTANTE + faccaoHB.QTDE_VAREJO_FALTANTE).ToString();

                        Label labDataPrevEntrega = e.Row.FindControl("labDataPrevEntrega") as Label;
                        if (labDataPrevEntrega != null)
                            if (faccaoHB.DATA_PREV_ENTREGA != null)
                                labDataPrevEntrega.Text = Convert.ToDateTime(faccaoHB.DATA_PREV_ENTREGA).ToString("dd/MM/yyyy");

                        Label labPrazoEntrega = e.Row.FindControl("labPrazoEntrega") as Label;
                        var prazoEntrega = 0;
                        if (faccaoHB.DATA_PREV_ENTREGA != null)
                        {
                            prazoEntrega = faccaoHB.DATA_PREV_ENTREGA.Value.Subtract(DateTime.Now).Days;
                        }
                        labPrazoEntrega.Text = prazoEntrega.ToString();


                        DropDownList ddlFaccaoProblema = e.Row.FindControl("ddlFaccaoProblema") as DropDownList;
                        ddlFaccaoProblema.DataSource = gFaccProblema;
                        ddlFaccaoProblema.DataBind();
                        if (faccaoHB.PROD_HB_FACCAO_PROBLEMA != null)
                            ddlFaccaoProblema.SelectedValue = faccaoHB.PROD_HB_FACCAO_PROBLEMA.ToString();

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
                    //qtdeAtacado += Convert.ToInt32(f.QTDE_ATACADO);
                    qtdeVarejo += Convert.ToInt32(f.QTDE_VAREJO);
                    //qtdeTotal += Convert.ToInt32(f.QTDE_TOTAL);

                    //if (f.QTDE_ATACADO != null && Convert.ToInt32(f.QTDE_ATACADO) > 0)
                    //    contagemAtacado += 1;

                    if (f.QTDE_VAREJO != null && Convert.ToInt32(f.QTDE_VAREJO) > 0)
                        contagemVarejo += 1;
                }

                //labTotalAtacado.Text = contagemAtacado.ToString();
                labTotalVarejo.Text = contagemVarejo.ToString();

                _footer.Cells[7].Text = qtdeVarejo.ToString();

                _footer.Cells[8].Text = qtdeFaltanteVarejo.ToString();

            }
        }
        protected void gvFaccaoHB_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_ATAC_VAR_FALResult> _faccResumo = ObterResumoAtacVarejoFaltante();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            //carregar dropddown problemas faccao
            gFaccProblema = ObterFaccaoProblema();

            _faccResumo = _faccResumo.OrderBy(e.SortExpression + sortDirection);
            gvFaccaoHB.DataSource = _faccResumo;
            gvFaccaoHB.DataBind();
        }

        protected void txtObsFaltante_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        string codigoHB = gvFaccaoHB.DataKeys[row.RowIndex].Value.ToString();

                        var hb = prodController.ObterHB(Convert.ToInt32(codigoHB));
                        if (hb != null)
                        {
                            hb.OBS_REL_FALTANTE = txt.Text.Trim();
                            prodController.AtualizarHB(hb);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

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
        private void CarregarServicosProducao(string codigoServico)
        {

            var servico = prodController.ObterServicoProducao().Where(p => (p.CODIGO == 1 || p.CODIGO == 4) && p.STATUS == 'A').ToList();
            if (servico != null)
            {
                servico.Insert(0, new PROD_SERVICO { CODIGO = 0, DESCRICAO = "", STATUS = 'A' });
                ddlServico.DataSource = servico;
                ddlServico.DataBind();

                if (codigoServico != "")
                    ddlServico.SelectedValue = codigoServico;
            }
        }
        private List<PROD_HB_FACCAO_PROBLEMA> ObterFaccaoProblema()
        {
            var faccProblema = faccController.ObterFaccaoProblema();
            return faccProblema;
        }
        private void CarregarFaccaoProblema()
        {
            ddlFaccaoProblema.DataSource = ObterFaccaoProblema();
            ddlFaccaoProblema.DataBind();
        }
        private void CarregarFornecedores(string fornecedor)
        {

            var fornecedores = prodController.ObterFornecedor().Where(f => f.STATUS == 'A' && f.TIPO == 'S').ToList();
            if (fornecedores != null)
            {
                fornecedores.Insert(0, new PROD_FORNECEDOR { CODIGO = 0, FORNECEDOR = "", PORC_ICMS = 0, TIPO = ' ', STATUS = ' ', EMAIL = null });
                ddlFornecedor.DataSource = fornecedores;
                ddlFornecedor.DataBind();

                if (fornecedor != "")
                    ddlFornecedor.SelectedValue = fornecedores.Where(p => p.FORNECEDOR.Trim() == fornecedor.Trim()).FirstOrDefault().CODIGO.ToString();

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
        private void CarregarGrupo()
        {
            var _grupo = (prodController.ObterGrupoProduto("01"));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        #endregion

        #region "RELATÓRIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                var faccaoResumo = ObterResumoAtacVarejoFaltante().OrderBy(p => p.FORNECEDOR).ThenBy(p => p.HB).ToList();

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
        private void GerarRelatorio(List<SP_OBTER_FACCAO_ATAC_VAR_FALResult> faccaoResumo)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "FACC_RESUMO_ATAC_VAR_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
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
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_FACCAO_ATAC_VAR_FALResult> faccaoResumo)
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
        private StringBuilder MontarResumoFaccao(StringBuilder _texto, List<SP_OBTER_FACCAO_ATAC_VAR_FALResult> faccaoResumo)
        {
            _texto.Append("<tr style='line-height: 19px; text-align:center;'>");
            _texto.Append("    <td colspan='2' style='padding: 0px 0px 0px 10px; border:0px solid #000; text-align:center;'>");
            _texto.Append("        <span style='font-size: 15px;'>&nbsp;</span>");
            _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='padding: 0px;");
            _texto.Append("            color: black; font-size: 11px; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.Append("            background: white; white-space: nowrap;'>");
            _texto.Append("            <tr style='line-height: 5px;'>");
            _texto.Append("                <td colspan='12'>");
            _texto.Append("                    &nbsp;");
            _texto.Append("                </td>");
            _texto.Append("            </tr>");
            _texto.Append("            <tr style='font-size:11px;'>");
            _texto.Append("                <td>Serviço</td>");
            _texto.Append("                <td>Coleção</td>");
            _texto.Append("                <td>HB</td>");
            _texto.Append("                <td>Nome</td>");
            _texto.Append("                <td>Varejo</td>");
            _texto.Append("                <td>Varejo Faltante</td>");
            _texto.Append("                <td>Fornecedor</td>");
            _texto.Append("                <td>Obs</td>");
            _texto.Append("                <td>Previsão</td>");
            _texto.Append("            </tr>");

            foreach (var f in faccaoResumo)
            {
                _texto.Append("        <tr style='font-size:10px; text-align:center;'>");
                _texto.Append("            <td>" + f.SERVICO + "</td>");
                _texto.Append("            <td>" + f.COLECAO + f.MOSTRUARIO.ToString() + "</td>");
                _texto.Append("            <td>" + f.HB + "</td>");
                _texto.Append("            <td>" + f.NOME + "</td>");
                _texto.Append("            <td>" + f.QTDE_VAREJO + "</td>");
                _texto.Append("            <td>" + f.QTDE_VAREJO_FALTANTE + "</td>");
                _texto.Append("            <td>" + f.FORNECEDOR + "</td>");
                _texto.Append("            <td>" + f.OBS_REL_FALTANTE + "</td>");
                _texto.Append("            <td>" + ((f.DATA_PREV_ENTREGA == null) ? "-" : Convert.ToDateTime(f.DATA_PREV_ENTREGA).ToString("dd/MM/yyyy")) + "</td>");
                _texto.Append("        </tr>");
            }

            _texto.Append("        </table>");
            _texto.Append("    </td>");
            _texto.Append("</tr>");

            return _texto;
        }
        #endregion

        protected void ddlFaccaoProblema_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = (DropDownList)sender;
                if (ddl != null)
                {
                    GridViewRow row = (GridViewRow)ddl.NamingContainer;
                    if (row != null)
                    {
                        string codigoHB = gvFaccaoHB.DataKeys[row.RowIndex].Value.ToString();

                        var hb = prodController.ObterHB(Convert.ToInt32(codigoHB));
                        if (hb != null)
                        {
                            hb.PROD_HB_FACCAO_PROBLEMA = Convert.ToInt32(ddl.SelectedValue);
                            prodController.AtualizarHB(hb);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected void gvExcel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_ATAC_VAR_FALResult faccaoHB = e.Row.DataItem as SP_OBTER_FACCAO_ATAC_VAR_FALResult;

                    if (faccaoHB != null)
                    {
                        Literal litPrazoEntrega = e.Row.FindControl("litPrazoEntrega") as Literal;
                        litPrazoEntrega.Text = "";
                        var prazoEntrega = 0;
                        if (faccaoHB.DATA_PREV_ENTREGA != null)
                            prazoEntrega = faccaoHB.DATA_PREV_ENTREGA.Value.Subtract(DateTime.Now).Days;
                        litPrazoEntrega.Text = prazoEntrega.ToString();
                    }
                }
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {

                if (Session[FACC_REL_ATAC_VAR_FAL] != null)
                {
                    var faccaoFal = (List<SP_OBTER_FACCAO_ATAC_VAR_FALResult>)Session[FACC_REL_ATAC_VAR_FAL];
                    gvExcel.DataSource = faccaoFal;
                    gvExcel.DataBind();

                    if (faccaoFal != null)
                    {
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "EXCEL_FACC_FAL" + DateTime.Today.Day + ".xls"));
                        Response.ContentType = "application/ms-excel";
                        //Abaixo codifica os caracteres para o alfabeto latino
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                        Response.Charset = "ISO-8859-1";
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter htw = new HtmlTextWriter(sw);
                        gvExcel.AllowPaging = false;
                        gvExcel.PageSize = 1000;
                        gvExcel.DataSource = faccaoFal;
                        gvExcel.DataBind();

                        gvExcel.HeaderRow.Style.Add("background-color", "#FFFFFF");

                        for (int i = 0; i < gvExcel.HeaderRow.Cells.Count; i++)
                        {
                            gvExcel.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                            gvExcel.HeaderRow.Cells[i].Style.Add("color", "#333333");
                        }
                        gvExcel.RenderControl(htw);
                        Response.Write(sw.ToString());
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }




    }
}
