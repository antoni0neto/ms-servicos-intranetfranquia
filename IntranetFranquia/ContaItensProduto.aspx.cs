using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class ContaItensProduto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        int totalTickets = 0;
        decimal totalVendas = 0;
        decimal totalDescontos = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
            }
        }

        private void CarregaDropDownListFilial()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            if (usuario != null)
            {
                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
            else
                Response.Redirect("~/Login.aspx");
        }

        private void CarregaGridViewVendas()
        {
            List<LOJA_VENDA> listaTickets_4 = new List<LOJA_VENDA>();
            List<LOJA_VENDA> listaTickets_3 = new List<LOJA_VENDA>();
            List<LOJA_VENDA> listaTickets_1 = new List<LOJA_VENDA>();

            int qtdeTicket_4_8_12_16 = 0;
            decimal valorTicket_4_8_12_16 = 0;
            decimal descontoTicket_4_8_12_16 = 0;

            int qtdeTicket_3_7_11_15 = 0;
            decimal valorTicket_3_7_11_15 = 0;
            decimal descontoTicket_3_7_11_15 = 0;

            int qtdeTicket_1_2_5_6_9_10_13_14 = 0;
            decimal valorTicket_1_2_5_6_9_10_13_14 = 0;
            decimal descontoTicket_1_2_5_6_9_10_13_14 = 0;

            List<ItensProduto> itensProduto = new List<ItensProduto>();

            List<LOJA_VENDA> listaVendasPeriodo = baseController.BuscaVendasPeriodo(ddlFilial.SelectedValue, CalendarDataInicio.SelectedDate, CalendarDataFim.SelectedDate);

            if (listaVendasPeriodo != null)
            {
                foreach (LOJA_VENDA item in listaVendasPeriodo)
                {
                    List<Sp_Busca_Produto_Venda_TicketResult> listaVendaProduto = baseController.BuscaItensVenda(item.CODIGO_FILIAL, item.TICKET);
                    List<Sp_Busca_Produto_Troca_TicketResult> listaTrocaProduto = baseController.BuscaItensTroca(item.CODIGO_FILIAL, item.TICKET);

                    if (listaVendaProduto != null & listaTrocaProduto != null)
                    {
                        if ((listaVendaProduto.Count - listaTrocaProduto.Count) == 4 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 8 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 12 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 16)
                        {
                            qtdeTicket_4_8_12_16++;
                            valorTicket_4_8_12_16 += Convert.ToDecimal(item.VALOR_PAGO) - Convert.ToDecimal(item.VALOR_TROCA);
                            descontoTicket_4_8_12_16 += Convert.ToDecimal(item.DESCONTO);

                            listaTickets_4.Add(item);
                        }

                        if ((listaVendaProduto.Count - listaTrocaProduto.Count) == 3 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 7 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 11 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 15)
                        {
                            qtdeTicket_3_7_11_15++;
                            valorTicket_3_7_11_15 += Convert.ToDecimal(item.VALOR_PAGO) - Convert.ToDecimal(item.VALOR_TROCA);
                            descontoTicket_3_7_11_15 += Convert.ToDecimal(item.DESCONTO);

                            listaTickets_3.Add(item);
                        }

                        if ((listaVendaProduto.Count - listaTrocaProduto.Count) == 1 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 2 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 5 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 6 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 9 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 10 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 13 ||
                            (listaVendaProduto.Count - listaTrocaProduto.Count) == 14)
                        {
                            qtdeTicket_1_2_5_6_9_10_13_14++;
                            valorTicket_1_2_5_6_9_10_13_14 += Convert.ToDecimal(item.VALOR_PAGO) - Convert.ToDecimal(item.VALOR_TROCA);
                            descontoTicket_1_2_5_6_9_10_13_14 += Convert.ToDecimal(item.DESCONTO);

                            listaTickets_1.Add(item);
                        }
                    }
                }

                Session["LISTA_4"] = listaTickets_4;
                Session["LISTA_3"] = listaTickets_3;
                Session["LISTA_1"] = listaTickets_1;

                ItensProduto ip = new ItensProduto();

                ip.QtdeItensProduto = "4-8-12-16";
                ip.QtdeTicket = qtdeTicket_4_8_12_16;
                ip.ValorTicket = valorTicket_4_8_12_16;
                ip.DescontoTicket = descontoTicket_4_8_12_16;

                itensProduto.Add(ip);

                ip = new ItensProduto();

                ip.QtdeItensProduto = "3-7-11-15";
                ip.QtdeTicket = qtdeTicket_3_7_11_15;
                ip.ValorTicket = valorTicket_3_7_11_15;
                ip.DescontoTicket = descontoTicket_3_7_11_15;

                itensProduto.Add(ip);

                ip = new ItensProduto();

                ip.QtdeItensProduto = "1-2-5-6-9-10-13-14";
                ip.QtdeTicket = qtdeTicket_1_2_5_6_9_10_13_14;
                ip.ValorTicket = valorTicket_1_2_5_6_9_10_13_14;
                ip.DescontoTicket = descontoTicket_1_2_5_6_9_10_13_14;

                itensProduto.Add(ip);

                GridViewVendas.DataSource = itensProduto;
                GridViewVendas.DataBind();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (ddlFilial.SelectedValue.ToString().Equals("0") || 
                ddlFilial.SelectedValue.ToString().Equals("") ||
                TextBoxDataInicio.Text.Equals("") ||
                TextBoxDataFim.Text.Equals(""))
                return;

            CarregaGridViewVendas();

            GridViewVendedor.Visible = false;
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataInicio_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime value = new DateTime(2013, 11, 14);

            if (e.Day.Date < value)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        }

        protected void CalendarDataFim_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime value = new DateTime(2013, 11, 14);

            if (e.Day.Date < value)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        }

        protected void GridViewVendas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewVendas.PageIndex = e.NewPageIndex;
            CarregaGridViewVendas();
        }

        protected void GridViewVendas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewVendas.FooterRow;

            foreach (GridViewRow item in GridViewVendas.Rows)
            {
                totalTickets += Convert.ToInt32(item.Cells[1].Text);
                totalVendas += Convert.ToDecimal(item.Cells[2].Text);
                totalDescontos += Convert.ToDecimal(item.Cells[3].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalTickets.ToString();
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = totalVendas.ToString();
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalDescontos.ToString();
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            GridViewVendedor.Visible = true;

            Button buttonPesquisar = sender as Button;

            if (buttonPesquisar != null)
                CarregaGridViewVendedor(buttonPesquisar.CommandArgument);
        }

        protected void GridViewVendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button buttonPesquisar = e.Row.FindControl("ButtonPequisar") as Button;

                if (buttonPesquisar != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        ItensProduto itensProduto = e.Row.DataItem as ItensProduto;

                        buttonPesquisar.CommandArgument = itensProduto.QtdeItensProduto;
                    }
                }
            }
        }

        private void CarregaGridViewVendedor(string qtdeItens)
        {
            List<LOJA_VENDA> listaLojaVenda = null;

            if (qtdeItens.Substring(0, 1).Equals("4"))
                listaLojaVenda = (List<LOJA_VENDA>)Session["LISTA_4"];

            if (qtdeItens.Substring(0, 1).Equals("3"))
                listaLojaVenda = (List<LOJA_VENDA>)Session["LISTA_3"];

            if (qtdeItens.Substring(0, 1).Equals("1"))
                listaLojaVenda = (List<LOJA_VENDA>)Session["LISTA_1"];

            GridViewVendedor.DataSource = listaLojaVenda;
            GridViewVendedor.DataBind();
        }

        protected void GridViewVendedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LOJA_VENDA lojaVenda = e.Row.DataItem as LOJA_VENDA;

            if (lojaVenda != null)
            {
                Literal literalVendedor = e.Row.FindControl("LiteralVendedor") as Literal;

                if (literalVendedor != null)
                {
                    LOJA_VENDEDORE lojaVendedor = baseController.BuscaVendedor(lojaVenda.CODIGO_FILIAL, lojaVenda.VENDEDOR);

                    if (lojaVendedor != null)
                        literalVendedor.Text = lojaVendedor.NOME_VENDEDOR;
                }
            }
        }
    }
}
