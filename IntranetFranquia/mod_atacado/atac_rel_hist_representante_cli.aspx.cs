using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class atac_rel_hist_representante_cli : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();

        int tQtdeOriginal = 0;
        decimal tValorOriginal = 0;
        int tQtdeCanc = 0;
        decimal tValorCanc = 0;
        int tQtdeEntregue = 0;
        decimal tValorEntregue = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["rep"] == null || Request.QueryString["rep"] == "" ||
                                    Request.QueryString["cde"] == null || Request.QueryString["cde"] == "" ||
                                    Request.QueryString["cate"] == null || Request.QueryString["cate"] == "" ||
                                    Session["USUARIO"] == null)
                    Response.Redirect("facc_menu.aspx");

                string colecaoDe = Request.QueryString["cde"].ToString();
                string colecaoAte = Request.QueryString["cate"].ToString();
                string rep = Request.QueryString["rep"].ToString();

                CarregarClientes(colecaoDe, colecaoAte, rep);

                txtRepresentante.Text = rep;
                hidColecaoDe.Value = colecaoDe;
                hidColecaoAte.Value = colecaoAte;
            }
        }

        private List<SP_OBTER_VENDA_CLI_REPResult> ObterClienteRepresentante(string colecaoDe, string colecaoAte, string rep)
        {
            var repCliente = atacController.ObterVendaColecaoClienteRepresentante(colecaoDe, colecaoAte, rep, false);

            return repCliente;
        }

        #region "HISTORICO CLIENTE"
        private void CarregarClientes(string colecaoDe, string colecaoAte, string rep)
        {
            var repCliente = ObterClienteRepresentante(colecaoDe, colecaoAte, rep);

            gvHistoricoCliente.DataSource = repCliente;
            gvHistoricoCliente.DataBind();

        }
        protected void gvHistoricoCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_CLI_REPResult cli = e.Row.DataItem as SP_OBTER_VENDA_CLI_REPResult;

                    if (cli != null)
                    {
                        Literal _litQtdeOriginal = e.Row.FindControl("litQtdeOriginal") as Literal;
                        if (_litQtdeOriginal != null)
                            _litQtdeOriginal.Text = ((cli.TOT_QTDE_ORIGINAL == null || cli.TOT_QTDE_ORIGINAL == 0) ? "-" : Convert.ToInt32(cli.TOT_QTDE_ORIGINAL).ToString());

                        Literal _litValorOriginal = e.Row.FindControl("litValorOriginal") as Literal;
                        if (_litValorOriginal != null)
                            _litValorOriginal.Text = ((cli.TOT_VALOR_ORIGINAL == null || cli.TOT_VALOR_ORIGINAL == 0) ? "-" : ("R$ " + Convert.ToDecimal(cli.TOT_VALOR_ORIGINAL).ToString("###,###,###,##0.00")));

                        Literal _litQtdeCanc = e.Row.FindControl("litQtdeCanc") as Literal;
                        if (_litQtdeCanc != null)
                            _litQtdeCanc.Text = ((cli.TOT_QTDE_CANCELADA == null || cli.TOT_QTDE_CANCELADA == 0) ? "-" : Convert.ToInt32(cli.TOT_QTDE_CANCELADA).ToString());

                        Literal _litValorCanc = e.Row.FindControl("litValorCanc") as Literal;
                        if (_litValorCanc != null)
                            _litValorCanc.Text = ((cli.TOT_VALOR_CANCELADO == null || cli.TOT_VALOR_CANCELADO == 0) ? "-" : ("R$ " + Convert.ToDecimal(cli.TOT_VALOR_CANCELADO).ToString("###,###,###,##0.00")));

                        Literal _litQtdeEntregue = e.Row.FindControl("litQtdeEntregue") as Literal;
                        if (_litQtdeEntregue != null)
                            _litQtdeEntregue.Text = ((cli.TOT_QTDE_ENTREGUE == null || cli.TOT_QTDE_ENTREGUE == 0) ? "-" : Convert.ToInt32(cli.TOT_QTDE_ENTREGUE).ToString());

                        Literal _litValorEntregue = e.Row.FindControl("litValorEntregue") as Literal;
                        if (_litValorEntregue != null)
                            _litValorEntregue.Text = ((cli.TOT_VALOR_ENTREGUE == null || cli.TOT_VALOR_ENTREGUE == 0) ? "-" : ("R$ " + Convert.ToDecimal(cli.TOT_VALOR_ENTREGUE).ToString("###,###,###,##0.00")));


                        tQtdeOriginal += Convert.ToInt32(cli.TOT_QTDE_ORIGINAL);
                        tValorOriginal += Convert.ToDecimal(cli.TOT_VALOR_ORIGINAL);
                        tQtdeCanc += Convert.ToInt32(cli.TOT_QTDE_CANCELADA);
                        tValorCanc += Convert.ToDecimal(cli.TOT_VALOR_CANCELADO);
                        tQtdeEntregue += Convert.ToInt32(cli.TOT_QTDE_ENTREGUE);
                        tValorEntregue += Convert.ToDecimal(cli.TOT_VALOR_ENTREGUE);

                    }
                }
            }
        }
        protected void gvHistoricoCliente_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvHistoricoCliente.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = tQtdeOriginal.ToString();
                footer.Cells[4].Text = "R$ " + tValorOriginal.ToString("###,###,###,##0.00");
                footer.Cells[4].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[5].Text = tQtdeCanc.ToString();
                footer.Cells[6].Text = "R$ " + tValorCanc.ToString("###,###,###,##0.00");
                footer.Cells[6].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[7].Text = tQtdeEntregue.ToString();
                footer.Cells[8].Text = "R$ " + tValorEntregue.ToString("###,###,###,##0.00");
                footer.Cells[8].HorizontalAlign = HorizontalAlign.Left;

            }
        }

        protected void gvHistoricoCliente_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_VENDA_CLI_REPResult> cliRep = ObterClienteRepresentante(hidColecaoDe.Value, hidColecaoDe.Value, txtRepresentante.Text);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            cliRep = cliRep.OrderBy(e.SortExpression + sortDirection);
            gvHistoricoCliente.DataSource = cliRep;
            gvHistoricoCliente.DataBind();
        }

        #endregion





    }
}

