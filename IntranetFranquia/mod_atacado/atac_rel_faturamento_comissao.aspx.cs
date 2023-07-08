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
    public partial class atac_rel_faturamento_comissao : System.Web.UI.Page
    {
        AtacadoController atacController = new AtacadoController();
        int coluna = 0, colunaDet = 0;
        string tagCorNegativo = "#CD2626";

        List<SP_OBTER_COMISSAO_REPRESENTANTEResult> gComissaoRepresentante = new List<SP_OBTER_COMISSAO_REPRESENTANTEResult>();

        int qtdeTotal = 0;
        decimal valorTotal = 0;
        decimal comissao = 0;
        int contPedidoZeroComissao = 0;
        string pedidoZeroComissao = "";

        int detQtdeTotal = 0;
        decimal detValorTotal = 0;
        decimal detComissao = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarDataAno();
                CarregarRepresentantes();

                string mes = (DateTime.Now.Month - 1).ToString();
                try
                {
                    ddlMes.SelectedValue = mes;
                }
                catch (Exception) { }

                Session["COMISSAO_ATACADO"] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            labErro.Text = "";
            try
            {
                CarregarComissoes();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        private List<SP_OBTER_COMISSAO_REPRESENTANTEResult> ObterComissaoRepresentante()
        {
            DateTime dataIni = DateTime.Now;
            DateTime dataFim = DateTime.Now;

            string ano = ddlAno.SelectedValue;
            string mes = ddlMes.SelectedValue;
            if (mes.Length == 1)
                mes = "0" + mes;

            int ultimoDiaMes = DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes));

            dataIni = Convert.ToDateTime(ano + "-" + mes + "-01");
            dataFim = Convert.ToDateTime(ano + "-" + mes + "-" + ultimoDiaMes.ToString());

            gComissaoRepresentante = atacController.ObterComissaoRepresentante(dataIni, dataFim, ddlRepresentante.SelectedValue.Trim());

            return gComissaoRepresentante;
        }
        private void CarregarComissoes()
        {
            //Obter comissao e gravar na lista global
            ObterComissaoRepresentante();

            Session["COMISSAO_ATACADO"] = gComissaoRepresentante;
            //Agrupar
            var comRep = gComissaoRepresentante.Where(x => x.TIPO == "F" || x.TIPO == "D").GroupBy(p => new { REPRESENTANTE = p.REPRESENTANTE, FILIAL = p.FILIAL }).Select(g => new SP_OBTER_COMISSAO_REPRESENTANTEResult
            {
                REPRESENTANTE = g.Key.REPRESENTANTE,
                FILIAL = g.Key.FILIAL,
                QTDE_TOTAL = g.Sum(c => c.QTDE_TOTAL),
                VALOR_TOTAL = g.Sum(c => ((c.COMISSAO > 0) ? c.VALOR_TOTAL : 0)),
                COMISSAO = g.Sum(c => c.COMISSAO),
                DATA_ENVIO = g.Max(c => c.DATA_ENVIO)
            }).OrderBy(o => o.REPRESENTANTE).ThenBy(u => u.FILIAL).ToList();

            gvComissao.DataSource = comRep;
            gvComissao.DataBind();
        }
        protected void gvComissao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_COMISSAO_REPRESENTANTEResult comissaoRep = e.Row.DataItem as SP_OBTER_COMISSAO_REPRESENTANTEResult;

                    coluna += 1;
                    if (comissaoRep != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litQtdeTotal = e.Row.FindControl("litQtdeTotal") as Literal;
                        if (_litQtdeTotal != null)
                            _litQtdeTotal.Text = comissaoRep.QTDE_TOTAL.ToString();

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                            _litValorTotal.Text = FormatarValor(comissaoRep.VALOR_TOTAL);

                        Literal _litComissao = e.Row.FindControl("litComissao") as Literal;
                        if (_litComissao != null)
                            _litComissao.Text = FormatarValor(comissaoRep.COMISSAO);

                        Literal _litDataUltimoEmail = e.Row.FindControl("litDataUltimoEmail") as Literal;
                        if (_litDataUltimoEmail != null)
                            if (comissaoRep.DATA_ENVIO != null)
                                _litDataUltimoEmail.Text = Convert.ToDateTime(comissaoRep.DATA_ENVIO).ToString("dd/MM/yyyy HH:mm");

                        qtdeTotal += Convert.ToInt32(comissaoRep.QTDE_TOTAL);
                        valorTotal += Convert.ToDecimal(comissaoRep.VALOR_TOTAL);
                        comissao += Convert.ToDecimal(comissaoRep.COMISSAO);

                        Literal _litRepresentante = e.Row.FindControl("litRepresentante") as Literal;
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;

                        GridView gvFaturado = e.Row.FindControl("gvFaturado") as GridView;
                        if (gvFaturado != null)
                        {
                            var faturado = gComissaoRepresentante.Where(p => p.TIPO == "F" && p.REPRESENTANTE == _litRepresentante.Text.Trim() && p.FILIAL == _litFilial.Text.Trim()).OrderBy(o => o.CLIENTE).ThenBy(y => y.PEDIDO).ThenBy(y => y.STATUS);

                            gvFaturado.DataSource = faturado;
                            gvFaturado.DataBind();

                            if (faturado != null && faturado.Count() <= 0)
                            {
                                Panel _pnlFaturamento = e.Row.FindControl("pnlFaturamento") as Panel;
                                if (_pnlFaturamento != null)
                                    _pnlFaturamento.Visible = false;
                            }

                            colunaDet = 0;
                            detQtdeTotal = 0;
                            detValorTotal = 0;
                            detComissao = 0;
                        }

                        GridView gvDevolvido = e.Row.FindControl("gvDevolvido") as GridView;
                        if (gvDevolvido != null)
                        {
                            var devolvido = gComissaoRepresentante.Where(p => p.TIPO == "D" && p.REPRESENTANTE == _litRepresentante.Text.Trim() && p.FILIAL == _litFilial.Text.Trim()).OrderBy(o => o.CLIENTE).ThenBy(y => y.PEDIDO).ThenBy(y => y.STATUS);

                            gvDevolvido.DataSource = devolvido;
                            gvDevolvido.DataBind();

                            if (devolvido != null && devolvido.Count() <= 0)
                            {
                                Panel _pnlDevolvido = e.Row.FindControl("pnlDevolvido") as Panel;
                                if (_pnlDevolvido != null)
                                    _pnlDevolvido.Visible = false;
                            }

                            colunaDet = 0;
                            detQtdeTotal = 0;
                            detValorTotal = 0;
                            detComissao = 0;
                        }

                        GridView gvCancelado = e.Row.FindControl("gvCancelado") as GridView;
                        if (gvCancelado != null)
                        {
                            var cancelado = gComissaoRepresentante.Where(p => p.TIPO == "C" && p.REPRESENTANTE == _litRepresentante.Text.Trim() && p.FILIAL == _litFilial.Text.Trim()).OrderBy(o => o.CLIENTE).ThenBy(y => y.PEDIDO).ThenBy(y => y.STATUS);

                            gvCancelado.DataSource = cancelado;
                            gvCancelado.DataBind();

                            if (cancelado != null && cancelado.Count() <= 0)
                            {
                                Panel _pnlCancelado = e.Row.FindControl("pnlCancelado") as Panel;
                                if (_pnlCancelado != null)
                                    _pnlCancelado.Visible = false;
                            }

                            colunaDet = 0;
                            detQtdeTotal = 0;
                            detValorTotal = 0;
                            detComissao = 0;
                        }
                    }
                }
            }
        }
        protected void gvComissao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvComissao.FooterRow;
            if (footer != null)
            {

                footer.Cells[2].Text = "Total";
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                //Qtde
                footer.Cells[4].Text = qtdeTotal.ToString();
                footer.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                //Valor
                footer.Cells[5].Text = FormatarValor(valorTotal);
                footer.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                //Comissao
                footer.Cells[6].Text = FormatarValor(comissao);
                footer.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        protected void gvDetalhe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_COMISSAO_REPRESENTANTEResult comissaoRepDet = e.Row.DataItem as SP_OBTER_COMISSAO_REPRESENTANTEResult;

                    colunaDet += 1;
                    if (comissaoRepDet != null)
                    {
                        Literal _colDet = e.Row.FindControl("litColunaDet") as Literal;
                        if (_colDet != null)
                            _colDet.Text = colunaDet.ToString();

                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                            _litStatus.Text = comissaoRepDet.STATUS.Substring(3, (comissaoRepDet.STATUS.Length - 3));

                        Literal _litValor = e.Row.FindControl("litValor") as Literal;
                        if (_litValor != null)
                            _litValor.Text = FormatarValor(comissaoRepDet.VALOR_TOTAL);

                        Literal _litComissao = e.Row.FindControl("litComissao") as Literal;
                        if (_litComissao != null)
                            _litComissao.Text = FormatarValor(comissaoRepDet.COMISSAO);

                        detQtdeTotal += Convert.ToInt32(comissaoRepDet.QTDE_TOTAL);
                        detValorTotal += Convert.ToDecimal(comissaoRepDet.VALOR_TOTAL);
                        detComissao += Convert.ToDecimal(comissaoRepDet.COMISSAO);

                        if (comissaoRepDet.TIPO == "F" && comissaoRepDet.COMISSAO <= 0)
                        {
                            e.Row.BackColor = Color.Moccasin;
                            contPedidoZeroComissao += 1;

                            pedidoZeroComissao = pedidoZeroComissao + comissaoRepDet.PEDIDO.Trim() + ", ";
                        }


                    }
                }
            }
        }
        protected void gvDetalhe_DataBound(object sender, EventArgs e)
        {
            GridView gvDetalhe = (GridView)sender;
            if (gvDetalhe != null)
            {
                GridViewRow footer = gvDetalhe.FooterRow;
                if (footer != null)
                {
                    footer.Cells[1].Text = "Total";
                    footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    //Qtde
                    footer.Cells[6].Text = detQtdeTotal.ToString();
                    footer.Cells[6].HorizontalAlign = HorizontalAlign.Center;

                    //Valor
                    footer.Cells[7].Text = FormatarValor(detValorTotal);
                    footer.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    //Comissao
                    footer.Cells[9].Text = FormatarValor(detComissao);
                    footer.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                }

                if (contPedidoZeroComissao > 0)
                {
                    pedidoZeroComissao = pedidoZeroComissao.Trim() + ",";
                    pedidoZeroComissao = pedidoZeroComissao.Replace(",,", "");

                    if (contPedidoZeroComissao == 1)
                        labErro.Text = "Existe " + contPedidoZeroComissao.ToString() + " pedido com a comissão zerada. Pedido: " + pedidoZeroComissao;
                    else
                        labErro.Text = "Existem " + contPedidoZeroComissao.ToString() + " pedidos com a comissão zerada. Pedidos: " + pedidoZeroComissao;
                }
            }
        }

        protected void btEmail_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string _url = "";
            Literal _litRepresentante = null;
            Literal _litFilial = null;

            if (b != null)
            {
                try
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        _litRepresentante = row.FindControl("litRepresentante") as Literal;
                        _litFilial = row.FindControl("litFilial") as Literal;

                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('atac_rel_faturamento_comissao_email.aspx?r=" + _litRepresentante.Text.Trim() + "&f=" + _litFilial.Text.Trim() + "&a=" + ddlAno.SelectedValue + "&m=" + ddlMes.SelectedValue + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }
                }
                catch (Exception ex)
                {
                    string erro = "";

                    if (_litRepresentante != null)
                        erro = "REP: " + _litRepresentante.Text.Trim() + " - ";

                    if (_litFilial != null)
                        erro = erro + "Filial: " + _litFilial.Text.Trim() + " - ";

                    //Jogar REPRESENTANTE E FILIAL NO ERRO
                    labErro.Text = erro + "ERRO: " + ex.Message;
                }
            }

        }

        protected void btExcelComissao_Click(object sender, EventArgs e)
        {
            try
            {

                //Obter comissao e gravar na lista global
                ObterComissaoRepresentante();

                gvExcel.DataSource = gComissaoRepresentante;
                gvExcel.DataBind();

                if (gComissaoRepresentante != null)
                {
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "COMISSAO_ATACADO_" + ddlAno.SelectedValue + "_" + ddlMes.SelectedValue + "_" + DateTime.Today.Day + ".xls"));
                    Response.ContentType = "application/ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gvExcel.AllowPaging = false;
                    gvExcel.PageSize = 1000;
                    gvExcel.DataSource = gComissaoRepresentante;
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
        private void CarregarDataAno()
        {
            var dataAno = new BaseController().ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }

        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            retorno = "<font size='2'  color='" + tagCor + "'>" + "R$ " + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + "&nbsp;</font> ";
            return retorno;
        }
        #endregion
    }
}
