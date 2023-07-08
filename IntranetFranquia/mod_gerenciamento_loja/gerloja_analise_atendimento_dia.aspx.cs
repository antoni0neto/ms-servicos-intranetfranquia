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
    public partial class gerloja_analise_atendimento_dia : System.Web.UI.Page
    {
        SaidaProdutoController saidaProdutoController = new SaidaProdutoController();
        BaseController baseController = new BaseController();

        decimal totalValor = 0;
        int totalTicket = 0;

        List<string> datas = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();
                ButtonVerDetalhe.Visible = false;
            }
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
        }

        private void CarregaGridViewVendas()
        {
            datas = baseController.RetornaDatasSemana(Convert.ToInt32(ddlMes.SelectedIndex), ddlAno.SelectedValue, ddlDia.SelectedValue);

            List<SP_Atendimento_Loja_DiaResult> lista_1 = saidaProdutoController.BuscaAtendimentosLojaDia(datas[0], ddlFilial.SelectedValue.ToString());
            List<SP_Atendimento_Loja_DiaResult> lista_2 = saidaProdutoController.BuscaAtendimentosLojaDia(datas[1], ddlFilial.SelectedValue.ToString());
            List<SP_Atendimento_Loja_DiaResult> lista_3 = saidaProdutoController.BuscaAtendimentosLojaDia(datas[2], ddlFilial.SelectedValue.ToString());
            List<SP_Atendimento_Loja_DiaResult> lista_4 = saidaProdutoController.BuscaAtendimentosLojaDia(datas[3], ddlFilial.SelectedValue.ToString());
            List<SP_Atendimento_Loja_DiaResult> lista_5 = null;

            if (datas.Count > 4)
                lista_5 = saidaProdutoController.BuscaAtendimentosLojaDia(datas[4], ddlFilial.SelectedValue.ToString());

            List<VendasHora> lista = lista = retornaResumo(lista_1, lista_2, lista_3, lista_4, lista_5);

            GridViewVendas.DataSource = lista;
            GridViewVendas.DataBind();

            GridViewVendas_1.DataSource = lista_1;
            GridViewVendas_1.DataBind();
            GridViewVendas_1.Visible = false;

            GridViewVendas_2.DataSource = lista_2;
            GridViewVendas_2.DataBind();
            GridViewVendas_2.Visible = false;

            GridViewVendas_3.DataSource = lista_3;
            GridViewVendas_3.DataBind();
            GridViewVendas_3.Visible = false;

            GridViewVendas_4.DataSource = lista_4;
            GridViewVendas_4.DataBind();
            GridViewVendas_4.Visible = false;

            if (datas.Count > 4)
            {
                GridViewVendas_5.DataSource = lista_5;
                GridViewVendas_5.DataBind();
                GridViewVendas_5.Visible = false;
            }
        }

        private List<VendasHora> retornaResumo(List<SP_Atendimento_Loja_DiaResult> lista_1,
                                               List<SP_Atendimento_Loja_DiaResult> lista_2,
                                               List<SP_Atendimento_Loja_DiaResult> lista_3,
                                               List<SP_Atendimento_Loja_DiaResult> lista_4,
                                               List<SP_Atendimento_Loja_DiaResult> lista_5)
        {
            List<VendasHora> w_lista = new List<VendasHora>();

            w_lista.Add(buscaItemVenda("10:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("11:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("12:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("13:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("14:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("15:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("16:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("17:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("18:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("19:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("20:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("21:00", lista_1, lista_2, lista_3, lista_4, lista_5));
            w_lista.Add(buscaItemVenda("22:00", lista_1, lista_2, lista_3, lista_4, lista_5));

            return w_lista;
        }

        private VendasHora buscaItemVenda(string hora,
                                          List<SP_Atendimento_Loja_DiaResult> lista_1,
                                          List<SP_Atendimento_Loja_DiaResult> lista_2,
                                          List<SP_Atendimento_Loja_DiaResult> lista_3,
                                          List<SP_Atendimento_Loja_DiaResult> lista_4,
                                          List<SP_Atendimento_Loja_DiaResult> lista_5)
        {
            VendasHora vendaHora = new VendasHora();

            foreach (SP_Atendimento_Loja_DiaResult item_1 in lista_1)
            {
                if (item_1.HORA.Equals(hora))
                {
                    vendaHora.DiaSemana = ddlDia.SelectedValue.ToString();
                    vendaHora.Hora = hora;
                    vendaHora.QtTicket = Convert.ToInt32(item_1.N_TICKET);
                    vendaHora.QtVendedor = Convert.ToInt32(item_1.N_VENDEDOR);
                    vendaHora.Valor = Convert.ToDecimal(item_1.VALOR);
                }
            }

            foreach (SP_Atendimento_Loja_DiaResult item_2 in lista_2)
            {
                if (item_2.HORA.Equals(hora))
                {
                    vendaHora.DiaSemana = ddlDia.SelectedValue.ToString();
                    vendaHora.Hora = hora;
                    vendaHora.QtTicket += Convert.ToInt32(item_2.N_TICKET);
                    vendaHora.QtVendedor += Convert.ToInt32(item_2.N_VENDEDOR);
                    vendaHora.Valor += Convert.ToDecimal(item_2.VALOR);
                }
            }

            foreach (SP_Atendimento_Loja_DiaResult item_3 in lista_3)
            {
                if (item_3.HORA.Equals(hora))
                {
                    vendaHora.DiaSemana = ddlDia.SelectedValue.ToString();
                    vendaHora.Hora = hora;
                    vendaHora.QtTicket += Convert.ToInt32(item_3.N_TICKET);
                    vendaHora.QtVendedor += Convert.ToInt32(item_3.N_VENDEDOR);
                    vendaHora.Valor += Convert.ToDecimal(item_3.VALOR);
                }
            }

            foreach (SP_Atendimento_Loja_DiaResult item_4 in lista_4)
            {
                if (item_4.HORA.Equals(hora))
                {
                    vendaHora.DiaSemana = ddlDia.SelectedValue.ToString();
                    vendaHora.Hora = hora;
                    vendaHora.QtTicket += Convert.ToInt32(item_4.N_TICKET);
                    vendaHora.QtVendedor += Convert.ToInt32(item_4.N_VENDEDOR);
                    vendaHora.Valor += Convert.ToDecimal(item_4.VALOR);
                }
            }

            if (lista_5 != null)
            {
                foreach (SP_Atendimento_Loja_DiaResult item_5 in lista_5)
                {
                    if (item_5.HORA.Equals(hora))
                    {
                        vendaHora.DiaSemana = ddlDia.SelectedValue.ToString();
                        vendaHora.Hora = hora;
                        vendaHora.QtTicket += Convert.ToInt32(item_5.N_TICKET);
                        vendaHora.QtVendedor += Convert.ToInt32(item_5.N_VENDEDOR);
                        vendaHora.Valor += Convert.ToDecimal(item_5.VALOR);
                    }
                }
            }

            return vendaHora;
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void ButtonPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (ddlFilial.SelectedValue.ToString().Equals("Selecione") || 
                ddlAno.SelectedValue.ToString().Equals("Selecione")    || 
                ddlMes.SelectedValue.ToString().Equals("Selecione")    || 
                ddlDia.SelectedValue.ToString().Equals("Selecione"))
                return;

            CarregaGridViewVendas();

            ButtonVerDetalhe.Visible = true;
        }

        protected void GridViewVendas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewVendas.FooterRow;

            foreach (GridViewRow item in GridViewVendas.Rows)
            {
                Literal literalData = item.FindControl("LiteralData") as Literal;

                if (literalData != null)
                    literalData.Text = ddlDia.SelectedValue.ToString();

                if (item.Cells[1].Text.Equals(""))
                    item.Cells[1].Text = "0";

                if (item.Cells[3].Text.Equals(""))
                    item.Cells[3].Text = "0";

                totalValor += Convert.ToDecimal(item.Cells[2].Text);
                totalTicket += Convert.ToInt32(item.Cells[4].Text);
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[2].Text = totalValor.ToString();
                footer.Cells[4].Text = totalTicket.ToString();
            }
        }

        protected void GridViewVendas_1_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendas_1.Rows)
            {
                Literal literalData1 = item.FindControl("LiteralData_1") as Literal;

                if (literalData1 != null)
                    literalData1.Text = datas[0].Substring(6, 2) + "/" + datas[0].Substring(4, 2) + "/" + datas[0].Substring(0, 4);
            }
        }

        protected void GridViewVendas_2_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendas_2.Rows)
            {
                Literal literalData2 = item.FindControl("LiteralData_2") as Literal;

                if (literalData2 != null)
                    literalData2.Text = datas[1].Substring(6, 2) + "/" + datas[1].Substring(4, 2) + "/" + datas[1].Substring(0, 4);
            }
        }

        protected void GridViewVendas_3_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendas_3.Rows)
            {
                Literal literalData3 = item.FindControl("LiteralData_3") as Literal;

                if (literalData3 != null)
                    literalData3.Text = datas[2].Substring(6, 2) + "/" + datas[2].Substring(4, 2) + "/" + datas[2].Substring(0, 4);
            }
        }

        protected void GridViewVendas_4_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendas_4.Rows)
            {
                Literal literalData4 = item.FindControl("LiteralData_4") as Literal;

                if (literalData4 != null)
                    literalData4.Text = datas[3].Substring(6, 2) + "/" + datas[3].Substring(4, 2) + "/" + datas[3].Substring(0, 4);
            }
        }

        protected void GridViewVendas_5_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow item in GridViewVendas_5.Rows)
            {
                Literal literalData5 = item.FindControl("LiteralData_5") as Literal;

                if (literalData5 != null)
                    literalData5.Text = datas[4].Substring(6, 2) + "/" + datas[4].Substring(4, 2) + "/" + datas[4].Substring(0, 4);
            }
        }

        protected void ButtonVerDetalhe_Click(object sender, EventArgs e)
        {
            Button buttonPesquisar = sender as Button;

            if (buttonPesquisar != null)
            {
                GridViewVendas_1.Visible = true;
                GridViewVendas_2.Visible = true;
                GridViewVendas_3.Visible = true;
                GridViewVendas_4.Visible = true;
                GridViewVendas_5.Visible = true;
            }
        }
    }
}
