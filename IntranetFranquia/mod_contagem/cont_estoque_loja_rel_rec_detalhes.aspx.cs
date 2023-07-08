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
    public partial class cont_estoque_loja_rel_rec_detalhes : System.Web.UI.Page
    {
        ContagemController contController = new ContagemController();

        int tSaldo = 0;
        int tContagem = 0;

        private bool isEditMode = false;

        protected bool IsInEditMode
        {
            get { return this.isEditMode; }
            set { this.isEditMode = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int codigoEstoqueLojaCont = 0;
                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("estoque_menu.aspx");

                codigoEstoqueLojaCont = Convert.ToInt32(Request.QueryString["p"].ToString());
                isEditMode = ((Request.QueryString["e"].ToString() == "1") ? true : false);
                var estLojaCont = contController.ObterEstoqueLojaContagem(codigoEstoqueLojaCont);
                if (estLojaCont != null)
                {
                    txtFilial.Text = new BaseController().ObterFilialIntranet(estLojaCont.CODIGO_FILIAL).FILIAL;
                    txtDescricao.Text = estLojaCont.DESCRICAO;
                    txtDataContagem.Text = estLojaCont.DATA_CONTAGEM.ToString("dd/MM/yyyy");
                }

                hidEditMode.Value = (isEditMode) ? "1" : "0";

                if (isEditMode)
                    btExcel.Visible = false;

                var recEstoque = contController.ObterEstoqueLojaRecontRel(codigoEstoqueLojaCont);

                gvEstoqueLojaContRec.DataSource = recEstoque;
                gvEstoqueLojaContRec.DataBind();

                Session["ESTOQUE_LOJA_RECONTAGEM_REC"] = recEstoque;
            }
        }

        protected void gvEstoqueLojaContRec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_REL_DIFF_EST_RECONTAGEMResult estLojaCont = e.Row.DataItem as SP_OBTER_REL_DIFF_EST_RECONTAGEMResult;

                    if (estLojaCont != null)
                    {
                        tSaldo += Convert.ToInt32(estLojaCont.TOTAL_ESTOQUE);
                        tContagem += Convert.ToInt32(estLojaCont.ESTOQUE_REC);

                        //Popular GRID VIEW FILHO
                        if (estLojaCont.FOTO != null && estLojaCont.FOTO.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { FOTO = estLojaCont.FOTO });
                                gvFoto.DataSource = _fotoProduto;
                                gvFoto.DataBind();
                            }
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }


                    }
                }
            }
        }
        protected void gvEstoqueLojaContRec_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["ESTOQUE_LOJA_RECONTAGEM_REC"] != null)
            {
                IEnumerable<SP_OBTER_REL_DIFF_EST_RECONTAGEMResult> estLojaCont = (IEnumerable<SP_OBTER_REL_DIFF_EST_RECONTAGEMResult>)Session["ESTOQUE_LOJA_RECONTAGEM_REC"];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection((GridView)sender, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName((GridView)sender, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName((GridView)sender, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                isEditMode = (hidEditMode.Value == "1") ? true : false;

                estLojaCont = estLojaCont.OrderBy(e.SortExpression + sortDirection);
                gvEstoqueLojaContRec.DataSource = estLojaCont;
                gvEstoqueLojaContRec.DataBind();

            }
        }
        protected void gvEstoqueLojaContRec_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[5].Text = tSaldo.ToString();
                footer.Cells[6].Text = tContagem.ToString();
            }
        }

        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO;
                    }
                }
            }
        }

        protected void txtContagem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox _txt = (TextBox)sender;
                if (_txt != null && _txt.Text != "")
                {
                    GridViewRow row = ((GridViewRow)_txt.NamingContainer);
                    if (row != null)
                    {
                        int codigoEstoqueRet = Convert.ToInt32(gvEstoqueLojaContRec.DataKeys[row.RowIndex].Value);
                        var estLojaContRet = contController.ObterEstoqueLojaContagemRetCodigo(codigoEstoqueRet);
                        estLojaContRet.ESTOQUE_REC = Convert.ToInt32(_txt.Text);
                        contController.AtualizarEstoqueLojaContagemRet(estLojaContRet);

                    }
                }
            }
            catch (Exception ex)
            {
                //Erro
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ESTOQUE_LOJA_RECONTAGEM_REC"] != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "EST_LOJA_CONTA_REC" + txtFilial.Text.Trim() + "_" + Convert.ToDateTime(txtDataContagem.Text).ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvEstoqueLojaContRec.AllowPaging = false;
                    gvEstoqueLojaContRec.PageSize = 1000;
                    gvEstoqueLojaContRec.DataSource = (Session["ESTOQUE_LOJA_RECONTAGEM_REC"] as List<SP_OBTER_REL_DIFF_EST_RECONTAGEMResult>);
                    gvEstoqueLojaContRec.DataBind();


                    gvEstoqueLojaContRec.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvEstoqueLojaContRec.HeaderRow.Cells.Count; i++)
                    {
                        gvEstoqueLojaContRec.HeaderRow.Cells[i].Attributes.Remove("href");
                        gvEstoqueLojaContRec.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                        gvEstoqueLojaContRec.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvEstoqueLojaContRec.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }
                    gvEstoqueLojaContRec.RenderControl(htw);
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
