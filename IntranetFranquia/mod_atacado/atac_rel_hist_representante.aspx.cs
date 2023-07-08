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

namespace Relatorios
{
    public partial class atac_rel_hist_representante : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();

        List<SP_OBTER_VENDA_CLI_REPResult> gVendaRepresentante = new List<SP_OBTER_VENDA_CLI_REPResult>();
        int tCol = 0;

        string callGridViewScroll = "$(function () { gridviewScroll(); });";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarRepresentantes();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            labErro.Text = "";
            try
            {
                CarregarHistoricoRep();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private List<SP_OBTER_VENDA_CLI_REPResult> ObterHistoricoRepresentante(string colecaoDe, string colecaoAte, string representante)
        {
            gVendaRepresentante = atacController.ObterVendaColecaoClienteRepresentante(colecaoDe, colecaoAte, representante, true);

            return gVendaRepresentante;
        }

        private void CarregarHistoricoRep()
        {

            try
            {
                if (gvHistRep.Columns.Count > 0)
                    gvHistRep.Columns.Clear();

                DataTable dt = new DataTable();

                var histRep = ObterHistoricoRepresentante(ddlColecaoDe.SelectedValue.Trim(), ddlColecaoAte.SelectedValue.Trim(), ddlRepresentante.SelectedValue);
                if (histRep != null && histRep.Count() > 0)
                {
                    var colColecao = gVendaRepresentante.GroupBy(p => new { COLECAO = p.COLECAO, DESC_COLECAO = p.DESC_COLECAO }).Select(x => new COLECOE
                    {
                        COLECAO = x.Key.COLECAO,
                        DESC_COLECAO = x.Key.DESC_COLECAO
                    }).OrderBy(c => c.COLECAO).ToList();

                    BoundField b = new BoundField();
                    b.DataField = "Coluna";
                    b.HeaderText = "";
                    b.HeaderStyle.Width = Unit.Pixel(30);
                    b.ItemStyle.Width = Unit.Pixel(30);
                    b.ItemStyle.CssClass = "corTD";
                    b.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    gvHistRep.Columns.Add(b);
                    dt.Columns.Add(new DataColumn("Coluna", typeof(string)));

                    b = new BoundField();
                    b.DataField = "Representante";
                    b.HeaderText = "Representante";
                    b.HeaderStyle.Width = Unit.Pixel(200);
                    b.ItemStyle.Width = Unit.Pixel(200);
                    b.ItemStyle.CssClass = "corTD";
                    b.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    gvHistRep.Columns.Add(b);
                    dt.Columns.Add(new DataColumn("Representante", typeof(string)));

                    List<OD_COL> ordemColecao = new List<OD_COL>();
                    int o = 1;
                    Color backColor = Color.White;
                    bool bColor = true;
                    foreach (var c in colColecao)
                    {
                        ordemColecao.Add(new OD_COL { COLUNA = o, COLECAO = c.COLECAO, DESC_COLECAO = c.DESC_COLECAO.Trim() });
                        o = o + 1;

                        b = new BoundField();
                        b.DataField = "Qtde Ent. " + c.DESC_COLECAO;
                        b.HeaderText = "Qtde Ent. " + c.DESC_COLECAO;
                        b.HeaderStyle.Width = Unit.Pixel(220);
                        b.ItemStyle.Width = Unit.Pixel(220);
                        b.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        gvHistRep.Columns.Add(b);
                        dt.Columns.Add(new DataColumn("Qtde Ent. " + c.DESC_COLECAO, typeof(string)));
                        b.ItemStyle.BackColor = backColor;

                        b = new BoundField();
                        b.DataField = "Val. Ent. " + c.DESC_COLECAO;
                        b.HeaderText = "Val. Ent. " + c.DESC_COLECAO;
                        b.HeaderStyle.Width = Unit.Pixel(220);
                        b.ItemStyle.Width = Unit.Pixel(220);
                        gvHistRep.Columns.Add(b);
                        dt.Columns.Add(new DataColumn("Val. Ent. " + c.DESC_COLECAO, typeof(string)));
                        b.ItemStyle.BackColor = backColor;

                        if (bColor)
                        {
                            backColor = Color.Bisque;
                            bColor = false;
                        }
                        else
                        {
                            backColor = Color.White;
                            bColor = true;
                        }
                    }

                    var rep = gVendaRepresentante.OrderBy(p => p.ITEM).Select(p => p.ITEM).Distinct();
                    int col = 0;
                    tCol = colColecao.Count();

                    DataRow dr = null;
                    int j = 0;
                    foreach (var r in rep)
                    {
                        dr = dt.NewRow();
                        dr[0] = (col + 1).ToString();
                        dr[1] = r;
                        j = 2;
                        for (int i = 0; i < (colColecao.Count()); i++)
                        {
                            var vr = gVendaRepresentante.Where(p => p.ITEM.Trim() == r.Trim() && p.COLECAO.Trim() == ordemColecao.Where(x => x.COLUNA == (i + 1)).FirstOrDefault().COLECAO.Trim()).FirstOrDefault();

                            dr[j] = (vr != null && vr.TOT_QTDE_ENTREGUE != null) ? Convert.ToInt32(vr.TOT_QTDE_ENTREGUE).ToString() : "-";
                            dr[j + 1] = ((vr != null && vr.TOT_VALOR_ENTREGUE != null) ? ("R$ " + Convert.ToDecimal(vr.TOT_VALOR_ENTREGUE).ToString("###,###,###,##0.00")) : "-");
                            j += 2;

                        }

                        dt.Rows.Add(dr);
                        col += 1;
                    }

                    //adiciona linha total
                    dr = dt.NewRow();
                    dr[0] = "-";
                    dr[1] = "";
                    j = 2;
                    for (int i = 0; i < (colColecao.Count()); i++)
                    {
                        dr[j] = "";
                        dr[j + 1] = "";
                    }
                    dt.Rows.Add(dr);


                    gvHistRep.Width = Unit.Pixel(1430);
                    gvHistRep.DataSource = dt;
                    gvHistRep.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);

                }
                else
                {
                    if (gvHistRep.Columns.Count > 0)
                        gvHistRep.Columns.Clear();
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message + "   -   " + ex.StackTrace;
            }
        }
        protected void gvHistRep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    string r = e.Row.Cells[1].Text;
                    string url_link = "atac_rel_hist_representante_cli.aspx?rep=" + r + "&cde=" + ddlColecaoDe.SelectedValue.Trim() + "&cate=" + ddlColecaoAte.SelectedValue.Trim();
                    e.Row.Cells[1].Text = "<a href='" + url_link + "' target='_blank' class='adre' title='" + r + "'>&nbsp;" + r + "</a>";

                }
            }
        }

        protected void gvHistRep_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvHistRep.FooterRow;
            if (footer != null)
            {
                //Soma a porra toda
                decimal[] d = new decimal[200];
                int j = 2;
                foreach (GridViewRow row in gvHistRep.Rows)
                {
                    j = 2;
                    for (int i = 1; i <= tCol; i++)
                    {
                        d[j] += ((row.Cells[j].Text != "&nbsp;" && row.Cells[j].Text != "-") ? (Convert.ToDecimal(row.Cells[j].Text)) : 0);
                        d[j + 1] += ((row.Cells[j + 1].Text != "&nbsp;" && row.Cells[j + 1].Text != "-") ? (Convert.ToDecimal(row.Cells[j + 1].Text.Replace("R$ ", ""))) : 0);

                        j += 2;
                    }

                }

                //adiciona linha total
                footer.Cells[1].Text = "Total";

                j = 2;
                for (int i = 1; i <= (tCol); i++)
                {
                    footer.Cells[j].Text = d[j].ToString();
                    footer.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                    footer.Cells[j + 1].Text = "R$ " + d[j + 1].ToString("###,###,###,##0.00");

                    j += 2;
                }

            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = atacController.ObterVendaColecaoClienteRepresentante("15", "40", "", true);

            var colAg = colecoes.GroupBy(p => new { COLECAO = p.COLECAO, DESC_COLECAO = p.DESC_COLECAO }).Select(x => new COLECOE
            {
                COLECAO = x.Key.COLECAO,
                DESC_COLECAO = x.Key.DESC_COLECAO
            }).OrderBy(c => c.COLECAO).ToList();

            ddlColecaoDe.DataSource = colAg;
            ddlColecaoDe.DataBind();

            ddlColecaoAte.DataSource = colAg;
            ddlColecaoAte.DataBind();

        }
        private void CarregarRepresentantes()
        {
            ddlRepresentante.DataSource = new BaseController().BuscaRepresentanteAtacado().OrderBy(p => p.REPRESENTANTE);
            ddlRepresentante.DataBind();
            ddlRepresentante.Items.Insert(0, new ListItem("", ""));
        }
        public class OD_COL
        {
            public int COLUNA { get; set; }
            public string COLECAO { get; set; }
            public string DESC_COLECAO { get; set; }
        }

        #endregion




    }
}
