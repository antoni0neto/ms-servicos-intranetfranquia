using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;
using System.IO;

namespace Relatorios
{
    public partial class atac_rel_faturamento : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();
        BaseController baseController = new BaseController();

        public int totalQtdeOriginal;
        public double totalValorOriginal;
        public int totalQtdeCancelada;
        public double totalValorCancelado;
        public int totalQtdeEntregue;
        public double totalValorEntregue;
        public int totalQtdeEmbalada;
        public double totalValorEmbalado;
        public int totalQtdeAEntregar;
        public double totalValorAEntregar;
        public int totalQtdeDevolvida;

        string callGridViewScroll = "$(function () { gridviewScroll(); });";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                USUARIO usuario = null;

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    usuario = (USUARIO)Session["USUARIO"];

                CarregaDropDownListColecao();
                CarregaDropDownListRepresentante();

                btExcelFaturamento.Enabled = false;
                Session["FATURAMENTO_ATC"] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAS"
        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
            ddlColecao.Items.Insert(0, new ListItem("Selecione", "0"));
        }
        private void CarregaDropDownListRepresentante()
        {
            ddlRepresentante.DataSource = baseController.BuscaRepresentanteAtacado().OrderBy(p => p.REPRESENTANTE);
            ddlRepresentante.DataBind();
            ddlRepresentante.Items.Insert(0, new ListItem("", "0"));
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            List<Utils.WebControls.ControlesValidacao> list = new List<Utils.WebControls.ControlesValidacao>();
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labColecao, CorPadrao = Color.Gray, CorAviso = Color.Red, Controle = ddlColecao, ValorPadrao = "0" });
            list.Add(new Utils.WebControls.ControlesValidacao() { Label = labSubColecao, CorPadrao = Color.Gray, CorAviso = Color.Red, Controle = ddlSubColecao, ValorPadrao = "0" });

            if (!Utils.WebControls.ValidarCampos(ref list))
            {
                labErro.Text = "Preencha corretamente os campos em vermelho.";
                return;
            }

            labErro.Text = "";

            CarregaGridViewAtacado(ddlColecao.SelectedValue.Trim(),
                ddlSubColecao.SelectedValue.Trim(),
                ddlStatus.SelectedItem.Value,
                ddlRepresentante.SelectedValue,
                txtNomeCliente.Text);

        }

        private void CarregaGridViewAtacado(string colecao, string subColecao, string status, string nomeRepresentante, string nomeCliente)
        {
            if (colecao.Equals("0") || colecao.Equals(""))
                return;

            Utils.WebControls.GetBoundFieldIndexByName(gvAtacado, " - >>");
            Utils.WebControls.GetBoundFieldIndexByName(gvAtacado, " - <<");

            List<SP_RELATORIO_FATURAMENTO_ATACADOResult> relAtacado = new List<SP_RELATORIO_FATURAMENTO_ATACADOResult>();
            relAtacado = atacController.ObterRelatorioFaturamentoAtacado(colecao, subColecao, status.Trim(), nomeRepresentante, nomeCliente);

            gvAtacado.DataSource = relAtacado;
            gvAtacado.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);

            btExcelFaturamento.Enabled = false;
            Session["FATURAMENTO_ATC"] = null;
            if (relAtacado != null && relAtacado.Count > 0)
            {
                Session["FATURAMENTO_ATC"] = relAtacado;
                btExcelFaturamento.Enabled = true;
            }

        }
        protected void gvAtacado_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }

            SP_RELATORIO_FATURAMENTO_ATACADOResult sp_Rel = e.Row.DataItem as SP_RELATORIO_FATURAMENTO_ATACADOResult;

            if (sp_Rel != null)
            {
                Literal literalColecao = e.Row.FindControl("LiteralColecao") as Literal;
                if (literalColecao != null)
                    literalColecao.Text = ddlColecao.SelectedValue;

                Literal _litQtdeEntregaPorc = e.Row.FindControl("litQtdeEntregaPorc") as Literal;
                if (_litQtdeEntregaPorc != null)
                    _litQtdeEntregaPorc.Text = (Convert.ToDouble(sp_Rel.QTDE_ENTREGUE_PORC)).ToString("###,##0.00") + "%";

                Literal literalQtdeOriginal = e.Row.FindControl("LiteralQtdeOriginal") as Literal;
                if (literalQtdeOriginal != null)
                    literalQtdeOriginal.Text = (Convert.ToDouble(sp_Rel.QTDE_ORIGINAL)).ToString("###,###,###");

                Literal literalValorOriginal = e.Row.FindControl("LiteralValorOriginal") as Literal;
                if (literalValorOriginal != null)
                    literalValorOriginal.Text = (Convert.ToDouble(sp_Rel.VALOR_ORIGINAL)).ToString("###,###,##0.00");

                Literal literalQtdeCancelada = e.Row.FindControl("LiteralQtdeCancelada") as Literal;
                if (literalQtdeCancelada != null)
                    literalQtdeCancelada.Text = (Convert.ToDouble(sp_Rel.QTDE_CANCELADA)).ToString("###,###,###");

                Literal literalValorCancelado = e.Row.FindControl("LiteralValorCancelado") as Literal;
                if (literalValorCancelado != null)
                    literalValorCancelado.Text = (Convert.ToDouble(sp_Rel.VALOR_CANCELADO)).ToString("###,###,##0.00");

                Literal literalQtdeEntregue = e.Row.FindControl("LiteralQtdeEntregue") as Literal;
                if (literalQtdeEntregue != null)
                    literalQtdeEntregue.Text = (Convert.ToInt32(sp_Rel.QTDE_ENTREGUE)).ToString("###,###,###");

                Literal literalValorEntregue = e.Row.FindControl("LiteralValorEntregue") as Literal;
                if (literalValorEntregue != null)
                    literalValorEntregue.Text = (Convert.ToInt32(sp_Rel.VALOR_ENTREGUE)).ToString("###,###,##0.00");

                Literal literalQtdeAEntregar = e.Row.FindControl("LiteralQtdeAEntregar") as Literal;
                if (literalQtdeAEntregar != null)
                    literalQtdeAEntregar.Text = (Convert.ToInt32(sp_Rel.QTDE_ENTREGAR)).ToString("###,###,###");

                Literal literalValorAEntregar = e.Row.FindControl("LiteralValorAEntregar") as Literal;
                if (literalValorAEntregar != null)
                    literalValorAEntregar.Text = (Convert.ToInt32(sp_Rel.VALOR_ENTREGAR)).ToString("###,###,##0.00");

                Literal literalQtdeEmbalada = e.Row.FindControl("LiteralQtdeEmbalada") as Literal;
                if (literalQtdeEmbalada != null)
                    literalQtdeEmbalada.Text = (Convert.ToInt32(sp_Rel.QTDE_EMBALADA)).ToString("###,###,###");

                Literal literalValorEmbalado = e.Row.FindControl("LiteralValorEmbalado") as Literal;
                if (literalValorEmbalado != null)
                    literalValorEmbalado.Text = (Convert.ToInt32(sp_Rel.VALOR_EMBALADO)).ToString("###,###,##0.00");

                Literal literalQtdeDevolvida = e.Row.FindControl("LiteralQtdeDevolvida") as Literal;
                if (literalQtdeDevolvida != null)
                    literalQtdeDevolvida.Text = (Convert.ToInt32(sp_Rel.QTDE_DEVOLVIDA)).ToString("###,###,###");

                Literal literalValorDevolvido = e.Row.FindControl("LiteralValorDevolvido") as Literal;
                if (literalValorDevolvido != null)
                    literalValorDevolvido.Text = (Convert.ToInt32(sp_Rel.VALOR_DEVOLVIDO)).ToString("###,###,##0.00");

                totalQtdeOriginal += Convert.ToInt32(sp_Rel.QTDE_ORIGINAL);
                totalValorOriginal += Convert.ToDouble(sp_Rel.VALOR_ORIGINAL);
                totalQtdeCancelada += Convert.ToInt32(sp_Rel.QTDE_CANCELADA);
                totalValorCancelado += Convert.ToDouble(sp_Rel.VALOR_CANCELADO);
                totalQtdeEntregue += Convert.ToInt32(sp_Rel.QTDE_ENTREGUE);
                totalValorEntregue += Convert.ToDouble(sp_Rel.VALOR_ENTREGUE);
                totalQtdeEmbalada += Convert.ToInt32(sp_Rel.QTDE_EMBALADA);
                totalValorEmbalado += Convert.ToInt32(sp_Rel.VALOR_EMBALADO);
                totalQtdeAEntregar += Convert.ToInt32(sp_Rel.QTDE_ENTREGAR);
                totalValorAEntregar += Convert.ToDouble(sp_Rel.VALOR_ENTREGAR);
                totalQtdeDevolvida += Convert.ToInt32(sp_Rel.QTDE_DEVOLVIDA);

            }
        }
        protected void gvAtacado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAtacado.FooterRow;

            foreach (GridViewRow item in gvAtacado.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    if (item.Cells[1].Text.Contains("NOVO"))
                        item.Cells[1].ForeColor = System.Drawing.Color.Red;

                    if (item.Cells[1].Text.Contains("CANCELADO"))
                        item.Cells[1].ForeColor = System.Drawing.Color.Pink;

                    if (item.Cells[1].Text.Contains("CONFERIR"))
                        item.Cells[1].ForeColor = System.Drawing.Color.Green;

                    if (item.Cells[1].Text.Contains("FAT. PARCIAL"))
                        item.Cells[1].ForeColor = System.Drawing.Color.Blue;

                    if (item.Cells[1].Text.Contains("FATURAMENTO"))
                        item.Cells[1].ForeColor = System.Drawing.Color.Orange;
                }
            }

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].Font.Bold = true;
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                if ((totalQtdeOriginal - totalQtdeCancelada) > 0)
                {
                    var porc = ((totalQtdeEntregue * 100.00) / ((totalQtdeOriginal - totalQtdeCancelada)));
                    footer.Cells[5].Text = porc.ToString("###,###,##0.00") + "%";
                    footer.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                }

                footer.Cells[8].Text = totalQtdeOriginal.ToString("###,###,##0");
                footer.Cells[9].Text = totalValorOriginal.ToString("###,###,##0.00");
                footer.Cells[10].Text = totalQtdeCancelada.ToString("###,###,##0");
                footer.Cells[11].Text = totalValorCancelado.ToString("###,###,##0.00");
                footer.Cells[12].Text = totalQtdeEntregue.ToString("###,###,##0");
                footer.Cells[13].Text = totalValorEntregue.ToString("###,###,##0.00");
                footer.Cells[14].Text = totalQtdeEmbalada.ToString("###,###,##0");
                footer.Cells[15].Text = totalValorEmbalado.ToString("###,###,##0");
                footer.Cells[16].Text = totalQtdeAEntregar.ToString("###,###,##0");
                footer.Cells[17].Text = totalValorAEntregar.ToString("###,###,##0.00");
                footer.Cells[18].Text = totalQtdeDevolvida.ToString("###,###,##0");

            }
        }
        protected void gvAtacado_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_RELATORIO_FATURAMENTO_ATACADOResult> listRelFaturamento = atacController.ObterRelatorioFaturamentoAtacado(ddlColecao.SelectedValue.Trim(), ddlSubColecao.SelectedValue.Trim(), ddlStatus.SelectedItem.Value, ddlRepresentante.SelectedValue, txtNomeCliente.Text);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            listRelFaturamento = listRelFaturamento.OrderBy(e.SortExpression + sortDirection);
            gvAtacado.DataSource = listRelFaturamento;
            gvAtacado.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), callGridViewScroll, true);

        }

        #region "FILTROS"
        protected void ddlColecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubColecao.DataSource = baseController.BuscaSubColecoes(ddlColecao.SelectedValue.Trim());
            ddlSubColecao.DataBind();
            ddlSubColecao.Items.Insert(0, new ListItem("", "0"));

            ddlSubColecao.SelectedIndex = -1;
            ddlRepresentante.SelectedIndex = -1;
            ddlStatus.SelectedIndex = -1;
            txtNomeCliente.Text = "";
        }
        #endregion

        protected void btExcelFaturamento_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["FATURAMENTO_ATC"] != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "FATURAMENTO_" + DateTime.Today.ToString("yyyy-MM-dd") + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvAtacado.AllowPaging = false;
                    gvAtacado.PageSize = 1000;
                    gvAtacado.DataSource = (Session["FATURAMENTO_ATC"] as List<SP_RELATORIO_FATURAMENTO_ATACADOResult>);
                    gvAtacado.DataBind();


                    gvAtacado.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvAtacado.HeaderRow.Cells.Count; i++)
                    {
                        gvAtacado.HeaderRow.Cells[i].Attributes.Remove("href");
                        gvAtacado.HeaderRow.Cells[i].Style.Add("pointer-events", "none");
                        gvAtacado.HeaderRow.Cells[i].Style.Add("background-color", "#FFFFFF");
                        gvAtacado.HeaderRow.Cells[i].Style.Add("color", "#333333");
                    }
                    gvAtacado.RenderControl(htw);
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
