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
    public partial class facc_analise_faccao_mensal : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        int tSaldo = 0;
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

                CarregarFornecedores();
                CarregarColecoes();

                Session["ANALISE_DESEMPENHO_FACCAO"] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CarregarFaccaoDesempenho();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }


        #region "ANALISE DE DESEMPENHO"
        private List<SP_OBTER_FACCAO_ANALISE_DESEMPENHOResult> ObterFaccaoDesempenho()
        {
            char? mostruario = null;
            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            return faccController.ObterFaccaoAnaliseDesempenho(ddlFornecedor.SelectedValue, ddlColecao.SelectedValue, mostruario);
        }

        private void CarregarFaccaoDesempenho()
        {
            Session["ANALISE_DESEMPENHO_FACCAO"] = null;

            var analiseDesempenho = ObterFaccaoDesempenho();

            gvFaccaoAnaliseDesempenho.DataSource = analiseDesempenho;
            gvFaccaoAnaliseDesempenho.DataBind();

            Session["ANALISE_DESEMPENHO_FACCAO"] = analiseDesempenho;
        }
        protected void gvFaccaoAnaliseDesempenho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_ANALISE_DESEMPENHOResult analiseDesempenho = e.Row.DataItem as SP_OBTER_FACCAO_ANALISE_DESEMPENHOResult;

                    if (analiseDesempenho != null)
                    {
                        Literal _litNota = e.Row.FindControl("litNota") as Literal;
                        if (_litNota != null)
                            _litNota.Text = analiseDesempenho.NOTA.ToString("0.0");

                        ImageButton _ibtPesquisar = e.Row.FindControl("ibtPesquisar") as ImageButton;
                        if (_ibtPesquisar != null)
                            _ibtPesquisar.CommandArgument = analiseDesempenho.FORNECEDOR;

                        tSaldo += analiseDesempenho.SALDO;

                    }
                }
            }
        }
        protected void gvFaccaoAnaliseDesempenho_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["ANALISE_DESEMPENHO_FACCAO"] != null)
            {
                IEnumerable<SP_OBTER_FACCAO_ANALISE_DESEMPENHOResult> _faccGrupo = ((IEnumerable<SP_OBTER_FACCAO_ANALISE_DESEMPENHOResult>)Session["ANALISE_DESEMPENHO_FACCAO"]);

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                _faccGrupo = _faccGrupo.OrderBy(e.SortExpression + sortDirection);
                ((GridView)sender).DataSource = _faccGrupo;
                ((GridView)sender).DataBind();
            }
        }

        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton b = (ImageButton)sender;

                if (b != null)
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        string fornecedor = b.CommandArgument.Trim();
                        string colecao = ddlColecao.SelectedValue.Trim();
                        string mostruario = ddlMostruario.SelectedValue;

                        Response.Redirect(("facc_analise_faccao_mensal_media.aspx?c=" + colecao + "&f=" + fornecedor + "&m=" + mostruario), "_blank", "");

                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarFornecedores()
        {

            List<PROD_FORNECEDOR> _fornecedores = new ProducaoController().ObterFornecedor().Where(p => p.CODIGO != 94).OrderBy(p => p.FORNECEDOR).ToList();

            if (_fornecedores != null)
            {
                _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });

                ddlFornecedor.DataSource = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'S') || p.STATUS == 'S');
                ddlFornecedor.DataBind();
            }

        }
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
        #endregion

        protected void gvFaccaoAnaliseDesempenho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvFaccaoAnaliseDesempenho.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[7].Text = tSaldo.ToString();

            }
        }





    }
}
