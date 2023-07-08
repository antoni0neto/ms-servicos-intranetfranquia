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
    public partial class facc_resumo_time : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();
        ProducaoController prodController = new ProducaoController();

        List<SP_OBTER_FACCAO_TIMEResult> faccaoResumoAux = new List<SP_OBTER_FACCAO_TIMEResult>();
        List<SP_OBTER_FACCAO_TIME_ENCAIXEResult> faccaoEncaixeAux = new List<SP_OBTER_FACCAO_TIME_ENCAIXEResult>();
        int qtdeAtacado = 0;
        int qtdeVarejo = 0;
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

                btBuscar_Click(null, null);
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_FACCAO_TIMEResult> ObterFaccaoTime()
        {
            List<SP_OBTER_FACCAO_TIMEResult> listaRetorno = new List<SP_OBTER_FACCAO_TIMEResult>();
            char? mostruario = null;
            int? numeroHB = null;
            int? prodServico = null;

            if (txtHB.Text.Trim() != "")
                numeroHB = Convert.ToInt32(txtHB.Text.Trim());

            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            if (ddlServico.SelectedValue.Trim() != "0")
                prodServico = Convert.ToInt32(ddlServico.SelectedValue);

            var resumoHBF = faccController.ObterFaccaoTime(ddlColecao.SelectedValue.Trim(), numeroHB, mostruario, ddlFornecedor.SelectedValue.Trim(), ddlFornecedorSub.SelectedValue.Trim(), prodServico, ddlStatus.SelectedValue);
            listaRetorno.AddRange(resumoHBF);

            //FILTRAR 
            listaRetorno = listaRetorno.Where(p => p.SUBSTATUS.Trim() == "CORTE INTEIRO" && p.STATUS_SEMANA != "OK").ToList();

            //AUXILIAR PARA CONTAGEM DE ATACADO E VAREJO (REMOVE REGISTROS DUPLICADOS)
            faccaoResumoAux.AddRange(listaRetorno);
            faccaoResumoAux = faccaoResumoAux.GroupBy(p => new { COLECAO = p.COLECAO, HB = p.HB, MOSTRUARIO = p.MOSTRUARIO, QTDE_ATACADO = p.QTDE_ATACADO, QTDE_VAREJO = p.QTDE_VAREJO }).Select(x => new SP_OBTER_FACCAO_TIMEResult
            {
                COLECAO = x.Key.COLECAO,
                HB = x.Key.HB,
                MOSTRUARIO = x.Key.MOSTRUARIO,
                QTDE_ATACADO = x.Key.QTDE_ATACADO,
                QTDE_VAREJO = x.Key.QTDE_VAREJO
            }).ToList();

            return listaRetorno.OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.MOSTRUARIO).ThenBy(p => p.PROD_PROCESSO).ThenBy(p => p.SERVICO).ToList();
        }
        private List<SP_OBTER_FACCAO_TIME_ENCAIXEResult> ObterFaccaoTimeEncaixe()
        {
            List<SP_OBTER_FACCAO_TIME_ENCAIXEResult> listaRetorno = new List<SP_OBTER_FACCAO_TIME_ENCAIXEResult>();
            char? mostruario = null;
            int? numeroHB = null;
            int? prodServico = null;
            int? prodProcesso = null;

            if (txtHB.Text.Trim() != "")
                numeroHB = Convert.ToInt32(txtHB.Text.Trim());

            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            var resumoHBF = faccController.ObterFaccaoTimeEncaixe(ddlColecao.SelectedValue.Trim(), numeroHB, mostruario, ddlFornecedor.SelectedValue.Trim(), prodServico, prodProcesso, ddlStatus.SelectedValue);
            listaRetorno.AddRange(resumoHBF);

            //AUXILIAR PARA CONTAGEM DE ATACADO E VAREJO (REMOVE REGISTROS DUPLICADOS)
            faccaoEncaixeAux.AddRange(listaRetorno);
            faccaoResumoAux = faccaoResumoAux.GroupBy(p => new { COLECAO = p.COLECAO, HB = p.HB, MOSTRUARIO = p.MOSTRUARIO, QTDE_ATACADO = p.QTDE_ATACADO, QTDE_VAREJO = p.QTDE_VAREJO }).Select(x => new SP_OBTER_FACCAO_TIMEResult
            {
                COLECAO = x.Key.COLECAO,
                HB = x.Key.HB,
                MOSTRUARIO = x.Key.MOSTRUARIO,
                QTDE_ATACADO = x.Key.QTDE_ATACADO,
                QTDE_VAREJO = x.Key.QTDE_VAREJO
            }).ToList();

            return listaRetorno.OrderBy(p => p.COLECAO).ThenBy(p => p.HB).ThenBy(p => p.MOSTRUARIO).ThenBy(p => p.PROD_PROCESSO).ToList();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvFaccaoHB.DataSource = ObterFaccaoTime();
                gvFaccaoHB.DataBind();

                gvFaccaoEncaixe.DataSource = ObterFaccaoTimeEncaixe();
                gvFaccaoEncaixe.DataBind();
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
            //ddlStatus.SelectedValue = "";

            labErro.Text = "";

            gvFaccaoHB.DataSource = new List<SP_OBTER_FACCAO_TIMEResult>();
            gvFaccaoHB.DataBind();

            gvFaccaoEncaixe.DataSource = new List<SP_OBTER_FACCAO_TIME_ENCAIXEResult>();
            gvFaccaoEncaixe.DataBind();

        }

        protected void gvFaccaoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_TIMEResult faccaoHB = e.Row.DataItem as SP_OBTER_FACCAO_TIMEResult;

                    if (faccaoHB != null)
                    {
                        Label _labColecao = e.Row.FindControl("labColecao") as Label;
                        if (_labColecao != null)
                            _labColecao.Text = faccaoHB.DESC_COLECAO;

                        Label _labTotal = e.Row.FindControl("labTotal") as Label;
                        if (_labTotal != null)
                            _labTotal.Text = faccaoHB.TOTAL.ToString();

                        Label _labEmissao = e.Row.FindControl("labEmissao") as Label;
                        if (_labEmissao != null)
                            if (faccaoHB.EMISSAO != null)
                                _labEmissao.Text = Convert.ToDateTime(faccaoHB.EMISSAO).ToString("dd/MM/yyyy");

                        Label _labMostruario = e.Row.FindControl("labMostruario") as Label;
                        if (_labMostruario != null)
                            _labMostruario.Text = (faccaoHB.MOSTRUARIO == 'N') ? "Não" : "Sim";

                        Label _labFaltante = e.Row.FindControl("labFaltante") as Label;
                        if (_labFaltante != null)
                        {
                            _labFaltante.Text = (faccaoHB.QTDE_FALTANTE).ToString();
                            qtdeFaltante += Convert.ToInt32(faccaoHB.QTDE_FALTANTE);
                        }

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

                    /*if (f.QTDE_ATACADO != "" && Convert.ToInt32(f.QTDE_ATACADO) > 0)
                        contagemAtacado += 1;

                    if (f.QTDE_VAREJO != "" && Convert.ToInt32(f.QTDE_VAREJO) > 0)
                        contagemVarejo += 1;*/
                }

                //labTotalAtacado.Text = contagemAtacado.ToString();
                //labTotalVarejo.Text = contagemVarejo.ToString();

                _footer.Cells[7].Text = qtdeAtacado.ToString();
                _footer.Cells[8].Text = qtdeVarejo.ToString();
                _footer.Cells[9].Text = qtdeTotal.ToString();

                _footer.Cells[11].Text = qtdeFaltante.ToString();
            }
        }
        protected void gvFaccaoHB_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_TIMEResult> _faccResumo = ObterFaccaoTime();

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

        protected void gvFaccaoEncaixe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_TIME_ENCAIXEResult faccaoEncaixe = e.Row.DataItem as SP_OBTER_FACCAO_TIME_ENCAIXEResult;

                    if (faccaoEncaixe != null)
                    {
                        Label _labColecao = e.Row.FindControl("labColecao") as Label;
                        if (_labColecao != null)
                            _labColecao.Text = faccaoEncaixe.DESC_COLECAO;

                        Label _labTotal = e.Row.FindControl("labTotal") as Label;
                        if (_labTotal != null)
                            _labTotal.Text = faccaoEncaixe.TOTAL.ToString();

                        Label _labMostruario = e.Row.FindControl("labMostruario") as Label;
                        if (_labMostruario != null)
                            _labMostruario.Text = (faccaoEncaixe.MOSTRUARIO == 'N') ? "Não" : "Sim";

                        Label _labLiberado = e.Row.FindControl("labLiberado") as Label;
                        if (_labLiberado != null)
                            _labLiberado.Text = faccaoEncaixe.DATA_INCLUSAO.ToString("dd/MM/yyyy");

                        Label _labServico = e.Row.FindControl("labServico") as Label;
                        if (_labServico != null)
                            _labServico.Text = (faccaoEncaixe.PROD_PROCESSO == 20) ? "Facção" : "Acabamento";

                    }
                }
            }
        }
        protected void gvFaccaoEncaixe_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvFaccaoEncaixe.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                qtdeAtacado = 0;
                qtdeVarejo = 0;
                qtdeTotal = 0;
                foreach (var f in faccaoEncaixeAux)
                {
                    qtdeAtacado += Convert.ToInt32(f.QTDE_ATACADO);
                    qtdeVarejo += Convert.ToInt32(f.QTDE_VAREJO);
                    qtdeTotal += (Convert.ToInt32(f.QTDE_ATACADO) + Convert.ToInt32(f.QTDE_VAREJO));
                }

                _footer.Cells[6].Text = qtdeAtacado.ToString();
                _footer.Cells[7].Text = qtdeVarejo.ToString();
                _footer.Cells[8].Text = qtdeTotal.ToString();
            }
        }
        protected void gvFaccaoEncaixe_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_TIME_ENCAIXEResult> _faccEncaixe = ObterFaccaoTimeEncaixe();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _faccEncaixe = _faccEncaixe.OrderBy(e.SortExpression + sortDirection);
            gvFaccaoEncaixe.DataSource = _faccEncaixe;
            gvFaccaoEncaixe.DataBind();
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
        private void CarregarServicosProducao()
        {
            List<PROD_SERVICO> _servico = new List<PROD_SERVICO>();
            _servico = prodController.ObterServicoProducao().Where(p => p.STATUS == 'A' && p.CODIGO != 5 && p.CODIGO != 4).ToList();
            if (_servico != null)
            {
                _servico.Insert(0, new PROD_SERVICO { CODIGO = 0, DESCRICAO = "", STATUS = 'A' });
                ddlServico.DataSource = _servico;
                ddlServico.DataBind();
            }
        }
        protected void ddlServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codigoServico = 0;
            codigoServico = Convert.ToInt32(ddlServico.SelectedValue);
            CarregarStatus(codigoServico);
        }
        private void CarregarStatus(int codigoServico)
        {
            List<ListItem> lstStatus = new List<ListItem>();

            lstStatus.Add(new ListItem { Value = "", Text = "", Selected = true }); // OK
            //lstStatus.Add(new ListItem { Value = "BAIXADO", Text = "BAIXADO" }); // OK

            /*if (codigoServico == 0) // OK
                lstStatus.Add(new ListItem { Value = "AG. ENCAIXE", Text = "AG. ENCAIXE" });*/

            /*if (codigoServico == 0) // OK
                lstStatus.Add(new ListItem { Value = "AG. ENC ACAB", Text = "AG. ENC ACAB" });*/

            /*if (codigoServico != 4)
                lstStatus.Add(new ListItem { Value = "AG. EMISSÃO", Text = "AG. EMISSÃO" });*/

            /*if (codigoServico == 0 || codigoServico == 4)
                lstStatus.Add(new ListItem { Value = "AG. EMISSÃO ACAB", Text = "AG. EMISSÃO ACAB" });*/

            if (codigoServico != 4)
                lstStatus.Add(new ListItem { Value = "AG. ENTRADA", Text = "AG. ENTRADA", Selected = true });

            //if (codigoServico == 0 || codigoServico == 4)
            //lstStatus.Add(new ListItem { Value = "AG. ENTRADA ACAB", Text = "AG. ENTRADA ACAB" });

            ddlStatus.DataSource = lstStatus;
            ddlStatus.DataBind();

            if (lstStatus.Count() == 2)
                ddlStatus.SelectedIndex = 1;
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

    }
}
