using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;
using System.Text;
using System.Globalization;

namespace Relatorios
{
    public partial class gest_loja_desconto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        int coluna = 0;
        int colunaDetalhe = 0;

        List<SP_OBTER_TICKETS_DESCONTOResult> _desconto;
        List<FN_OBTER_DESCONTOResult> _descontoDetalhe;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarFilial();
                if (!string.IsNullOrEmpty(Request.QueryString["ref"]))
                    CarregaLabelsModulo(Request.QueryString["ref"]);
                else
                    CarregaLabelsModulo("1");
            }
        }

        private void CarregaLabelsModulo(string refID)
        {
            if (refID == "1")
            {
                lblModulo.Text = "Módulo de Gestão";
                lblSubModulo.Text = "Relatórios";
                lnkVoltar.NavigateUrl = "~/mod_gestao/gest_menu.aspx";
            }
            else if (refID == "2")
            {
                lblModulo.Text = "Módulo Financeiro";
                lblSubModulo.Text = "Loja";
                lnkVoltar.NavigateUrl = "~/DefaultFinanceiro.aspx";
            }
        }

        #region "DADOS INICIAIS"
        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");           
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");            
        }
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                //lstFilial = baseController.BuscaFiliais(usuario);
                lstFilial = baseController.BuscaFiliais_Intermediario();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                    lstFilial.Add(new FILIAI { COD_FILIAL = "999999", FILIAL = "GERAL" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();
                }
            }
        }
        #endregion

        #region "ACOES"
        private void CarregarTickets()
        {
            _desconto = new List<SP_OBTER_TICKETS_DESCONTOResult>();
            _desconto = baseController.ObterTicketsDesconto(ddlFilial.SelectedValue, txtDataInicio.Text.Trim(), txtDataFim.Text.Trim());
            if (_desconto != null)
            {
                //Obter detalhes que nao sejam vale mercadoria
                _descontoDetalhe = baseController.ObterTicketsDescontoDetalhe(ddlFilial.SelectedValue, txtDataInicio.Text.Trim(), txtDataFim.Text.Trim(), 'N');

                //Filtrar descontos
                gvTicket.DataSource = _desconto.Where(p => p.COD_TIPO != 3 && p.COD_TIPO != 1 && p.COD_TIPO != 0).ToList();
                gvTicket.DataBind();

                //Subtrair tickets total de tickets vale mercadoria
                List<SP_OBTER_TICKETS_DESCONTOResult> tickets = new List<SP_OBTER_TICKETS_DESCONTOResult>();
                tickets.AddRange(_desconto.Where(p => p.COD_TIPO == 0 || p.COD_TIPO == 2).ToList());

                tickets = tickets.GroupBy(p => new { DESCONTO_PORC = p.DESCONTO_PORC }).Select(j => new SP_OBTER_TICKETS_DESCONTOResult
                {
                    TIPO = "TOTAL DE TICKETS",
                    TOTAL_TICKETS = j.Sum(k => (k.COD_TIPO == 2) ? -k.TOTAL_TICKETS : k.TOTAL_TICKETS),
                    TOTAL_PECAS = j.Sum(k => (k.COD_TIPO == 2) ? -k.TOTAL_PECAS : k.TOTAL_PECAS),
                    TOTAL_VALOR = j.Sum(k => (k.COD_TIPO == 2) ? -k.TOTAL_VALOR : k.TOTAL_VALOR),
                    TOTAL_DESCONTO = j.Sum(k => (k.COD_TIPO == 2) ? -k.TOTAL_DESCONTO : k.TOTAL_DESCONTO),
                }).ToList();

                gvTicketTotal.DataSource = tickets;
                gvTicketTotal.DataBind();                
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;

            labErro.Text = "";
            if (txtDataInicio.Text.Trim() == "" || !DateTime.TryParse(txtDataInicio.Text.Trim(), out v_dataini))
            {
                labErro.Text = "Informe uma Data Inicial válida.";
                return;
            }
            if (txtDataFim.Text.Trim() == "" || !DateTime.TryParse(txtDataFim.Text.Trim(), out v_datafim))
            {
                labErro.Text = "Informe uma Data Final válida.";
                return;
            }
            if (ddlFilial.SelectedValue.Trim() == "" || ddlFilial.SelectedValue.Trim() == "Selecione")
            {
                labErro.Text = "Selecione uma Filial.";
                return;
            }

            try
            {
                CarregarTickets();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }            

        }
        protected void gvTicket_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_TICKETS_DESCONTOResult _descontoLinha = e.Row.DataItem as SP_OBTER_TICKETS_DESCONTOResult;

                    coluna += 1;
                    if (_descontoLinha != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litTipoDesconto = e.Row.FindControl("litTipoDesconto") as Literal;
                        if (_litTipoDesconto != null)
                            _litTipoDesconto.Text = (_descontoLinha.COD_TIPO == 2) ? "VALE MERCADORIA" : _descontoLinha.DESCONTO_PORC;

                        Literal _litTotalValor = e.Row.FindControl("litTotalValor") as Literal;
                        if (_litTotalValor != null)
                            _litTotalValor.Text = "R$ " + Convert.ToDecimal(_descontoLinha.TOTAL_VALOR).ToString("###,###,###,###,##0.00");

                        Literal _litTotalDesconto = e.Row.FindControl("litTotalDesconto") as Literal;
                        if (_litTotalDesconto != null)
                            _litTotalDesconto.Text = "R$ " + Convert.ToDecimal(_descontoLinha.TOTAL_DESCONTO).ToString("###,###,###,###,##0.00");

                        Literal _litMediaDesconto = e.Row.FindControl("litMediaDesconto") as Literal;
                        if (_litMediaDesconto != null)
                            _litMediaDesconto.Text = Convert.ToDecimal((_descontoLinha.TOTAL_DESCONTO / _descontoLinha.TOTAL_VALOR) * 100).ToString("########0.00") + "%";

                        e.Row.Height = Unit.Pixel(25);

                        GridView gvDescontoDetalhe = e.Row.FindControl("gvDescontoDetalhe") as GridView;
                        if (gvDescontoDetalhe != null)
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                            {
                                colunaDetalhe = 0;
                                if (_descontoLinha.COD_TIPO == 2)
                                    gvDescontoDetalhe.DataSource = baseController.ObterTicketsDescontoDetalhe(ddlFilial.SelectedValue, txtDataInicio.Text.Trim(), txtDataFim.Text.Trim(), 'S');
                                else
                                    gvDescontoDetalhe.DataSource = _descontoDetalhe.Where(p => p.DESCONTO_PORC.Trim() == _descontoLinha.DESCONTO_PORC.Trim()).OrderBy(g => g.MES_ANIVERSARIO).ToList();

                                gvDescontoDetalhe.DataBind();                                
                            }
                        }
                    }
                }
            }
        }
        protected void gvTicket_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTicket.FooterRow;
            if (footer != null)
            {
                var descontoRodape = _desconto.Where(p => p.COD_TIPO == 1).SingleOrDefault();
                if (descontoRodape != null)
                {
                    footer.Cells[2].Text = "Total";
                    footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                    footer.Cells[3].Text = descontoRodape.TOTAL_TICKETS.ToString();
                    footer.Cells[4].Text = descontoRodape.TOTAL_PECAS.ToString();
                    footer.Cells[5].Text = "R$ " + Convert.ToDecimal(descontoRodape.TOTAL_VALOR).ToString("###,###,###,###,##0.00");
                    footer.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    footer.Cells[6].Text = "R$ " + Convert.ToDecimal(descontoRodape.TOTAL_DESCONTO).ToString("###,###,###,###,##0.00");
                    footer.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
        protected void gvDescontoDetalhe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //string formaPgto = "";
            string desconto = "";
            string descontoPORC = "";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    FN_OBTER_DESCONTOResult _descontoDetLinha = e.Row.DataItem as FN_OBTER_DESCONTOResult;

                    colunaDetalhe += 1;
                    if (_desconto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaDetalhe.ToString();

                        Literal _litMesAniversario = e.Row.FindControl("litMesAniversario") as Literal;
                        if (_litMesAniversario != null)
                        {
                            if (_descontoDetLinha.MES_ANIVERSARIO == 0)
                                _litMesAniversario.Text = "Sim";
                            else
                                _litMesAniversario.Text = "";
                        }

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                            _litValorTotal.Text = "R$ " + Convert.ToDecimal(_descontoDetLinha.TOTAL_VALOR).ToString("###,###,###,##0.00");

                        Literal _litDesconto = e.Row.FindControl("litDesconto") as Literal;
                        if (_litDesconto != null)
                        {
                            desconto = "R$ " + Convert.ToDecimal(_descontoDetLinha.TOTAL_DESCONTO).ToString("###,###,###,###,##0.00");
                            _litDesconto.Text = desconto;
                        }

                        Literal _litTotalDescontoPorc = e.Row.FindControl("litTotalDescontoPorc") as Literal;
                        if (_litTotalDescontoPorc != null)
                        {
                            descontoPORC = Convert.ToDecimal((_descontoDetLinha.TOTAL_DESCONTO / _descontoDetLinha.TOTAL_VALOR) * 100).ToString("##0.00") + "%";
                            _litTotalDescontoPorc.Text = descontoPORC;
                        }                       
                    }
                }
            }
        }
        #endregion

        #region "TICKET TOTAL"
        protected void gvTicketTotal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_TICKETS_DESCONTOResult _descontoLinha = e.Row.DataItem as SP_OBTER_TICKETS_DESCONTOResult;

                    if (_descontoLinha != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = "";

                        Literal _litTipoDesconto = e.Row.FindControl("litTipoDesconto") as Literal;
                        if (_litTipoDesconto != null)
                            _litTipoDesconto.Text = "TOTAL DE TICKETS";

                        Literal _litTotalValor = e.Row.FindControl("litTotalValor") as Literal;
                        if (_litTotalValor != null)
                            _litTotalValor.Text = "R$ " + Convert.ToDecimal(_descontoLinha.TOTAL_VALOR).ToString("###,###,###,###,##0.00");

                        Literal _litTotalDesconto = e.Row.FindControl("litTotalDesconto") as Literal;
                        if (_litTotalDesconto != null)
                            _litTotalDesconto.Text = "R$ " + Convert.ToDecimal(_descontoLinha.TOTAL_DESCONTO).ToString("###,###,###,###,##0.00");

                        Literal _litMediaDesconto = e.Row.FindControl("litMediaDesconto") as Literal;
                        if (_litMediaDesconto != null)
                        {
                            if (_descontoLinha.TOTAL_VALOR > 0)
                                _litMediaDesconto.Text = Convert.ToDecimal((_descontoLinha.TOTAL_DESCONTO / _descontoLinha.TOTAL_VALOR) * 100).ToString("########0.00") + "%";
                            else
                                _litMediaDesconto.Text = "0,00%";
                        }
                    }
                }
            }
        }
        protected void gvTicketTotal_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvTicket.FooterRow;
            if (footer != null)
            {
                var descontoRodape = _desconto.Where(p => p.COD_TIPO == 1).SingleOrDefault();
                if (descontoRodape != null)
                {
                    footer.Cells[2].Text = "Total";
                    footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                    footer.Cells[3].Text = descontoRodape.TOTAL_TICKETS.ToString();
                    footer.Cells[4].Text = descontoRodape.TOTAL_PECAS.ToString();
                    footer.Cells[5].Text = "R$ " + Convert.ToDecimal(descontoRodape.TOTAL_VALOR).ToString("###,###,###,###,##0.00");
                    footer.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    footer.Cells[6].Text = "R$ " + Convert.ToDecimal(descontoRodape.TOTAL_DESCONTO).ToString("###,###,###,###,##0.00");
                    footer.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
        #endregion
    }
}
