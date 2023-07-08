using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using Relatorios.mod_desenvolvimento.modelo_pocket;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios
{
    public partial class cont_estoque_loja_rel_detalhes : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();

        int tQtdeCont = 0;
        int tSaldo = 0;
        int tDiff = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoEstoqueLojaCont = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("estoque_menu.aspx");

                codigoEstoqueLojaCont = Convert.ToInt32(Request.QueryString["p"].ToString());
                var estLojaCont = contController.ObterEstoqueLojaContagem(codigoEstoqueLojaCont);
                if (estLojaCont != null)
                {
                    txtFilial.Text = new BaseController().ObterFilialIntranet(estLojaCont.CODIGO_FILIAL).FILIAL;
                    txtDescricao.Text = estLojaCont.DESCRICAO;
                    txtDataContagem.Text = estLojaCont.DATA_CONTAGEM.ToString("dd/MM/yyyy");
                }


                var estoqueLojaContRel = contController.ObterEstoqueLojaContRel(codigoEstoqueLojaCont);

                gvEstoqueLojaCont.DataSource = estoqueLojaContRel;
                gvEstoqueLojaCont.DataBind();

                Session["ESTOQUE_LOJA_CONTAGEM"] = estoqueLojaContRel;

            }
        }

        protected void gvEstoqueLojaCont_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_REL_DIFF_EST_CONTAGEMResult estLojaCont = e.Row.DataItem as SP_OBTER_REL_DIFF_EST_CONTAGEMResult;

                    if (estLojaCont != null)
                    {

                        tQtdeCont += Convert.ToInt32(estLojaCont.TOTAL_CONTAGEM);
                        tSaldo += Convert.ToInt32(estLojaCont.TOTAL_ESTOQUE);
                        tDiff += Convert.ToInt32(estLojaCont.DIFERENCA);

                    }
                }
            }
        }
        protected void gvEstoqueLojaCont_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["ESTOQUE_LOJA_CONTAGEM"] != null)
            {
                IEnumerable<SP_OBTER_REL_DIFF_EST_CONTAGEMResult> estLojaCont = (IEnumerable<SP_OBTER_REL_DIFF_EST_CONTAGEMResult>)Session["ESTOQUE_LOJA_CONTAGEM"];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection((GridView)sender, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName((GridView)sender, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName((GridView)sender, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                estLojaCont = estLojaCont.OrderBy(e.SortExpression + sortDirection);
                gvEstoqueLojaCont.DataSource = estLojaCont;
                gvEstoqueLojaCont.DataBind();

            }
        }

        protected void gvEstoqueLojaCont_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[4].Text = tQtdeCont.ToString();
                footer.Cells[5].Text = tSaldo.ToString();
                footer.Cells[6].Text = tDiff.ToString();
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ESTOQUE_LOJA_CONTAGEM"] != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "EST_LOJA_CONTA_" + txtFilial.Text.Trim() + "_" + Convert.ToDateTime(txtDataContagem.Text).ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvEstoqueLojaCont.AllowPaging = false;
                    gvEstoqueLojaCont.PageSize = 1000;
                    gvEstoqueLojaCont.DataSource = (Session["ESTOQUE_LOJA_CONTAGEM"] as List<SP_OBTER_REL_DIFF_EST_CONTAGEMResult>);
                    gvEstoqueLojaCont.DataBind();


                    gvEstoqueLojaCont.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvEstoqueLojaCont.HeaderRow.Cells.Count; i++)
                    {
                        gvEstoqueLojaCont.HeaderRow.Cells[i].Attributes.Remove("href");
                        gvEstoqueLojaCont.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                        gvEstoqueLojaCont.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvEstoqueLojaCont.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }
                    gvEstoqueLojaCont.RenderControl(htw);
                    Response.Write(sw.ToString().Replace("<a", "<p").Replace("</a>", "</p>"));
                    Response.End();
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
