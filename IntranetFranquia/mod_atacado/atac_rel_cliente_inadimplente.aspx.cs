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
    public partial class atac_rel_cliente_inadimplente : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();
        BaseController baseController = new BaseController();

        List<SP_OBTER_CLIENTES_INADIMPLENTESResult> gClientes = new List<SP_OBTER_CLIENTES_INADIMPLENTESResult>();

        int qtdeCliente = 0;
        decimal valorTotal = 0;
        decimal valorAtraso = 0;

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
                CarregarClientes();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private List<SP_OBTER_CLIENTES_INADIMPLENTESResult> ObterClientesInadimplentes()
        {
            var cliInad = atacController.ObterClientesInadimplentes(ddlRepresentante.SelectedValue, ddlColecao.SelectedValue, "", "");

            if (ddlClienteAtrasado.SelectedValue != "")
            {
                if (ddlClienteAtrasado.SelectedValue == "S")
                    cliInad = cliInad.Where(p => p.VALOR_EM_ATRASO > 0).ToList();
                else
                    cliInad = cliInad.Where(p => p.VALOR_EM_ATRASO <= 0).ToList();
            }

            return cliInad;
        }
        private void CarregarClientes()
        {

            gClientes = ObterClientesInadimplentes();

            //Agrupar
            var cliInadGroup = gClientes.GroupBy(p => new { REPRESENTANTE = p.REPRESENTANTE }).Select(g => new SP_OBTER_CLIENTES_INADIMPLENTESResult
            {
                REPRESENTANTE = g.Key.REPRESENTANTE,
                CLIENTE_ATACADO = g.Select(c => c.CLIENTE_ATACADO).Distinct().Count().ToString(),
                VALOR_ORIGINAL = g.Sum(c => c.VALOR_ORIGINAL),
                VALOR_EM_ATRASO = g.Sum(c => c.VALOR_EM_ATRASO)
            }).OrderBy(o => o.REPRESENTANTE).ToList();

            gvRepresentante.DataSource = cliInadGroup;
            gvRepresentante.DataBind();
        }
        protected void gvRepresentante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CLIENTES_INADIMPLENTESResult rep = e.Row.DataItem as SP_OBTER_CLIENTES_INADIMPLENTESResult;

                    if (rep != null)
                    {
                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                            _litValorTotal.Text = "R$ " + Convert.ToDecimal(rep.VALOR_ORIGINAL).ToString("###,###,###,###,##0.00");

                        Literal _litValorAtraso = e.Row.FindControl("litValorAtraso") as Literal;
                        if (_litValorAtraso != null)
                            _litValorAtraso.Text = "R$ " + Convert.ToDecimal(rep.VALOR_EM_ATRASO).ToString("###,###,###,###,##0.00");

                        Literal _litInadimplentePerc = e.Row.FindControl("litInadimplentePerc") as Literal;
                        if (_litInadimplentePerc != null)
                        {
                            if (rep.VALOR_ORIGINAL > 0)
                                _litInadimplentePerc.Text = Convert.ToDecimal(rep.VALOR_EM_ATRASO / rep.VALOR_ORIGINAL * 100).ToString("##0.00") + "%";
                            else
                                _litInadimplentePerc.Text = "0.00%";
                        }

                        GridView gvCliente = e.Row.FindControl("gvCliente") as GridView;
                        if (gvCliente != null)
                        {
                            gvCliente.DataSource = gClientes.Where(p => p.REPRESENTANTE.Trim() == rep.REPRESENTANTE.Trim());
                            gvCliente.DataBind();
                        }

                        qtdeCliente += Convert.ToInt32(rep.CLIENTE_ATACADO);
                        valorTotal += Convert.ToDecimal(rep.VALOR_ORIGINAL);
                        valorAtraso += Convert.ToDecimal(rep.VALOR_EM_ATRASO);
                    }
                }
            }
        }
        protected void gvRepresentante_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvRepresentante.FooterRow;
            if (footer != null)
            {

                footer.Cells[2].Text = "Total";
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                //Qtde
                footer.Cells[3].Text = qtdeCliente.ToString();
                footer.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                //Valor Total
                footer.Cells[4].Text = "R$ " + valorTotal.ToString("###,###,###,###,##0.00");

                //Valor em Atraso
                footer.Cells[5].Text = "R$ " + valorAtraso.ToString("###,###,###,###,##0.00");

                //Valor em Inadimplmente
                if (valorTotal > 0)
                    footer.Cells[6].Text = Convert.ToDecimal(valorAtraso / valorTotal * 100).ToString("##0.00") + "%";
                else
                    footer.Cells[6].Text = "0.00%";
                footer.Cells[6].HorizontalAlign = HorizontalAlign.Center;

            }
        }

        protected void gvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CLIENTES_INADIMPLENTESResult cli = e.Row.DataItem as SP_OBTER_CLIENTES_INADIMPLENTESResult;

                    if (cli != null)
                    {
                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                            _litValorTotal.Text = "R$ " + Convert.ToDecimal(cli.VALOR_ORIGINAL).ToString("###,###,###,###,##0.00");

                        Literal _litValorAtraso = e.Row.FindControl("litValorAtraso") as Literal;
                        if (_litValorAtraso != null)
                            _litValorAtraso.Text = "R$ " + Convert.ToDecimal(cli.VALOR_EM_ATRASO).ToString("###,###,###,###,##0.00");

                        Literal _litInadimplentePerc = e.Row.FindControl("litInadimplentePerc") as Literal;
                        if (_litInadimplentePerc != null)
                        {
                            if (cli.VALOR_ORIGINAL > 0)
                                _litInadimplentePerc.Text = Convert.ToDecimal(cli.VALOR_EM_ATRASO / cli.VALOR_ORIGINAL * 100).ToString("##0.00") + "%";
                            else
                                _litInadimplentePerc.Text = "0.00%";
                        }
                    }
                }
            }
        }
        protected void gvCliente_DataBound(object sender, EventArgs e)
        {
            GridView gvCliente = (GridView)sender;
            if (gvCliente != null)
            {
                GridViewRow footer = gvCliente.FooterRow;
                if (footer != null)
                {
                }
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {

                var cli = ObterClientesInadimplentes();

                gvExcel.DataSource = ObterClientesInadimplentes();
                gvExcel.DataBind();


                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "CLI_INADIMPLENTE_ATACADO_" + ddlRepresentante.SelectedValue.Trim() + "_" + ddlColecao.SelectedItem.Text.Trim() + "_" + DateTime.Today.Day + ".xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gvExcel.AllowPaging = false;
                gvExcel.PageSize = 1000;
                gvExcel.DataSource = cli;
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
            catch (Exception ex)
            {
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        #region "DADOS INICIAIS"
        private void CarregarRepresentantes()
        {
            ddlRepresentante.DataSource = new BaseController().BuscaRepresentanteAtacado().OrderBy(p => p.REPRESENTANTE);
            ddlRepresentante.DataBind();
            ddlRepresentante.Items.Insert(0, new ListItem("", ""));
        }
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
            colecoes.Add(new COLECOE { COLECAO = "00", DESC_COLECAO = "SEM COLEÇÃO" });
            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();
        }

        #endregion
    }
}
